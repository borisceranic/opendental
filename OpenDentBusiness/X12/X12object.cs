using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace OpenDentBusiness
{
	///<summary>Encapsulates one entire X12 Interchange object, including multiple functional groups and transaction sets. It does not care what type of transactions are contained.  It just stores them.  It does not inherit either.  It is up to other classes to use this as needed.</summary>
	public class X12object{
		///<summary>usually *,:,and ~</summary>
		public X12Separators Separators;
		///<summary>A collection of X12FunctionalGroups.</summary>
		public List<X12FunctionalGroup> FunctGroups;
		///<summary>All segments for the entiremessage.</summary>
		public List<X12Segment> Segments;

		public static bool IsX12(string messageText){
			if(messageText==null || messageText.Length<106){
				return false;
			}
			if(messageText.Substring(0,3)!="ISA"){
				return false;
			}
			try {
				//Denti-cal sends us 835s, but they also send us "EOB" reports which start with "ISA" and look similar to X12 but are NOT X12.
				new X12object(messageText);//Only an X12 object if we can parse it.  Denti-cal "EOB" reports fail this test, as they should.
			}
			catch {
				return false;
			}
			return true;
		}

		///<summary>This override is never explicitly used.</summary>
		protected X12object(){

		}

		///<summary>Takes raw text and converts it into an X12Object.</summary>
		public X12object(string messageText){
			messageText=messageText.Replace("\r","");
			messageText=messageText.Replace("\n","");
			if(messageText.Substring(0,3)!="ISA"){
				throw new ApplicationException("ISA not found");
			}
			Separators=new X12Separators();
			Separators.Element=messageText.Substring(3,1);
			Separators.Subelement=messageText.Substring(104,1);
			Separators.Segment=messageText.Substring(105,1);
			string[] messageRows=messageText.Split(new string[] {Separators.Segment},StringSplitOptions.None);
			FunctGroups=new List<X12FunctionalGroup>();
			Segments=new List<X12Segment>();
			string row;
			X12Segment segment;
			for(int i=1;i<messageRows.Length;i++){
				row=messageRows[i];
				segment=new X12Segment(row,Separators);
				Segments.Add(segment);
				if(messageRows[i]=="") {
					//do nothing
				}
				else if(segment.SegmentID=="IEA") {//if end of interchange
					//do nothing
				}
				else if(segment.SegmentID=="GS") {//if new functional group
					FunctGroups.Add(new X12FunctionalGroup(segment));
				}
				else if(segment.SegmentID=="GE") {//if end of functional group
					//do nothing
				}
				else if(segment.SegmentID=="ST") {//if new transaction set
					if(LastGroup().Transactions==null) {
						LastGroup().Transactions=new List<X12Transaction>();
					}
					LastGroup().Transactions.Add(new X12Transaction(segment));
				}
				else if(segment.SegmentID=="SE") {//if end of transaction
					//do nothing
				}
				else if(segment.SegmentID=="TA1") {//This segment can either replace or supplement any GS segments for any ack type (997,999,277).  The TA1 will always be before the first GS segment.
					//Ignore for now.  We should eventually match TA101 with the ISA13 of the claim that we sent, so we can report the status to the user using fields TA104 and TA105.
					//This segment is neither mandated or prohibited (see 277.pdf pg. 207).
				}
				else {//it must be a detail segment within a transaction.
					if(LastTransaction().Segments==null) {
						LastTransaction().Segments=new List<X12Segment>();
					}
					LastTransaction().Segments.Add(segment);
				}
				//row=sr.ReadLine();
			}
		}

		///<summary>Example of values returned: 004010X097A1 (4010 dental), 005010X222A1 (5010 medical), 005010X223A2 (5010 institutional), 005010X224A2 (5010 dental)</summary>
		public string GetFormat() {
			for(int i=0;i<Segments.Count;i++) {
				if(Segments[i].SegmentID=="GS") {
					return Segments[i].Get(8);
				}
			}
			return "";
		}

		///<summary>Returns true if the X12 object is in 4010 format.</summary>
		public bool IsFormat4010() {
			string format=GetFormat();
			if(format.Length>=6) {
				return (format.Substring(2,4)=="4010");
			}
			return false;
		}

		///<summary>Returns true if the X12 object is in 5010 format.</summary>
		public bool IsFormat5010() {
			string format=GetFormat();
			if(format.Length>=6) {
				return (format.Substring(2,4)=="5010");
			}
			return false;
		}

		///<summary>Returns true if there is a TA1 segment. The TA1 segment is neither mandated or prohibited (see 277.pdf pg. 207).
		///The Inmidiata clearinghouse likes to use TA1 segments to replace the usual acknowledgements (format ISA-TA1-IEA).</summary>
		public bool IsAckInterchange() {
			for(int i=0;i<Segments.Count;i++) {
				if(Segments[i].SegmentID=="GS") {
					return false;//If a GS is present, it will get handled elsewhere.
				}
			}
			for(int i=0;i<Segments.Count;i++) {
				if(Segments[i].SegmentID=="TA1") {
					return true;//A TA1 can be used when there are no GS segments.  That implies that it is an interchange ack.
				}
			}
			return false;
		}

		public bool Is997() {
			//There is only one transaction set (ST/SE) per functional group (GS/GE), but I think there can be multiple functional groups
			//if acking multiple 
			if(this.FunctGroups.Count!=1) {
				return false;
			}
			if(this.FunctGroups[0].Transactions[0].Header.Get(1)=="997") {
				return true;
			}
			return false;
		}

		public bool Is999() {
			//There is only one transaction set (ST/SE) per functional group (GS/GE).
			if(this.FunctGroups.Count!=1) {
				return false;
			}
			if(this.FunctGroups[0].Transactions[0].Header.Get(1)=="999") {
				return true;
			}
			return false;
		}

		public bool Is271() {
			if(this.FunctGroups[0].Transactions[0].Header.Get(1)=="271") {
				return true;
			}
			return false;
		}

		private X12FunctionalGroup LastGroup(){
			return (X12FunctionalGroup)FunctGroups[FunctGroups.Count-1];
		}

		private X12Transaction LastTransaction(){
			return (X12Transaction)LastGroup().Transactions[LastGroup().Transactions.Count-1];
		}

		///<summary>Returns the list of unique transaction set identifiers within the X12.</summary>
		public List<string> GetTranSetIds() {
			List<string> retVal=new List<string>();
			for(int i=0;i<FunctGroups[0].Transactions.Count;i++) {
				string tranSetId=FunctGroups[0].Transactions[i].Header.Get(2);
				retVal.Add(tranSetId);
			}
			return retVal;
		}

	}


	///<summary>GS/GE combination. Contained within an interchange control combination (ISA/IEA). Contains at least one transaction (ST/SE). </summary>
	public class X12FunctionalGroup{
		///<summary>A collection of X12Transactions</summary>
		public List<X12Transaction> Transactions;
		///<summary>The segment that identifies this functional group</summary>
		public X12Segment Header;

		///<summary>Supply the functional group header(GS) when creating this object.</summary>
		public X12FunctionalGroup(X12Segment header){
			Header=header.Copy();
		}
	}


	///<summary>ST/SE combination.  Containted within functional group (GS/GE).  In claims, there will be one transaction per carrier.</summary>
	public class X12Transaction{
		///<summary>A collection of all the X12Segments for this transaction, in the order they originally appeared.</summary>
		public List<X12Segment> Segments;
		///<summary>The segment that identifies this functional group</summary>
		public X12Segment Header;

		///<summary>Supply the transaction header(ST) when creating this object.</summary>
		public X12Transaction(X12Segment header){
			Header=header.Copy();
		}

		public X12Segment GetSegmentByID(string segID){
			for(int i=0;i<Segments.Count;i++){
				if(Segments[i].SegmentID==segID){
					return Segments[i];
				}
			}
			return null;
		}
	}


	///<summary>An X12 segment is a single row of the text file.</summary>
	public class X12Segment{
		///<summary>Usually 2 or 3 letters. Can also be found at Elements[0].</summary>
		public string SegmentID;
		///<summary></summary>
		public string[] Elements;
		///<summary></summary>
		private X12Separators Separators;
		private string rawText;

		///<summary></summary>
		public X12Segment(string rawTxt,X12Separators separators){
			rawText=rawTxt.ToString();
			Separators=separators;
			//first, remove the segment terminator
			rawTxt=rawTxt.Replace(separators.Segment,"");
			//then, split the row into elements, eliminating the DataElementSeparator
			Elements=rawText.Split(Char.Parse(separators.Element));
			SegmentID=Elements[0];
		}

		private X12Segment(){

		}

		public override string ToString() {
			return rawText;
		}

		///<summary>Returns a copy of this segement</summary>
		public X12Segment Copy(){
			X12Segment retVal=new X12Segment();
			retVal.SegmentID=SegmentID;
			retVal.Elements=(string[])Elements.Clone();//shallow copy is fine since just strings.
			return retVal;
		}

		///<summary>Returns the string representation of the given element within this segment. If the element does not exist, as can happen with optional elements, then "" is returned.</summary>
		public string Get(int elementPosition){
			if(Elements.Length<=elementPosition){
				return "";
			}
			return Elements[elementPosition];
		}

		///<summary>Returns the string representation of the given element,subelement within this segment. If the element or subelement does not exist, as can happen with optional elements, then "" is returned.  Subelement is 1-based, just like the x12 specs.</summary>
		public string Get(int elementPosition,int subelementPosition){
			if(Elements.Length<=elementPosition){
				return "";
			}
			string[] subelements=Elements[elementPosition].Split(Char.Parse(Separators.Subelement));
			//example, subelement passed in is 2.  Convert to 0-indexed means [1].  If Length < 2, then we have a problem.
			if(subelements.Length < subelementPosition) {
				return "";
			}
			return subelements[subelementPosition-1];
		}

	}

	///<summary></summary>
	public struct X12Separators{
		///<summary>usually ~</summary>
		public string Segment;
		///<summary>usually *</summary>
		public string Element;
		///<summary>usually :</summary>
		public string Subelement;
	}

	#region Segments

	public class X12_ACT {
		///<summary>ACT01</summary>
		public string AccountNumber1;
		///<summary>ACT06</summary>
		public string AccountNumber2;

		public X12_ACT(X12Segment seg) {
			AccountNumber1=seg.Get(1);
			AccountNumber2=seg.Get(6);
		}
	}

	public class X12_AMT {
		///<summary>AMT01</summary>
		public string AmountQualifierCode;
		///<summary>AMT02</summary>
		public string MonetaryAmount;

		public X12_AMT(X12Segment seg) {
			AmountQualifierCode=seg.Get(1);
			MonetaryAmount=seg.Get(2);
		}
	}

	public class X12_BGN {
		///<summary>BGN01</summary>
		public string TransactionSetPurposeCode;
		///<summary>BGN02</summary>
		public string ReferenceIdentification1;
		///<summary>BGN03</summary>
		public string DateBgn;
		///<summary>BGN04</summary>
		public string TimeBgn;
		///<summary>BGN05</summary>
		public string TimeCode;
		///<summary>BGN06</summary>
		public string ReferenceIdentifcation2;
		///<summary>BGN08</summary>
		public string ActionCode;

		public X12_BGN(X12Segment seg) {
			TransactionSetPurposeCode=seg.Get(1);
			ReferenceIdentification1=seg.Get(2);
			DateBgn=seg.Get(3);
			TimeBgn=seg.Get(4);
			TimeCode=seg.Get(5);
			ReferenceIdentifcation2=seg.Get(6);
			ActionCode=seg.Get(8);
		}
	}

	public class X12_DMG {
		///<summary>DMG01</summary>
		public string DateTimePeriodFormatQualifier;
		///<summary>DMG02</summary>
		public string DateTimePeriod;
		///<summary>DMG03</summary>
		public string GenderCode;
		///<summary>DMG04</summary>
		public string MaritalStatusCode;
		///<summary>DMG05</summary>
		public string CompositeRaceOrEthnicityInformation;
		///<summary>DMG06</summary>
		public string CitizenshipStatusCode;
		///<summary>DMG10</summary>
		public string CodeListQualifierCode;
		///<summary>DMG11</summary>
		public string IndustryCode;

		public X12_DMG(X12Segment seg) {
			DateTimePeriodFormatQualifier=seg.Get(1);
			DateTimePeriod=seg.Get(2);
			GenderCode=seg.Get(3);
			MaritalStatusCode=seg.Get(4);
			CompositeRaceOrEthnicityInformation=seg.Get(5);
			CitizenshipStatusCode=seg.Get(6);
			CodeListQualifierCode=seg.Get(10);
			IndustryCode=seg.Get(11);
		}
	}

	public class X12_DSB {
		///<summary>DSP01</summary>
		public string DisabilityTypeCode;
		///<summary>DSP07</summary>
		public string ProductServiceIdQualifier;
		///<summary>DSP08</summary>
		public string MedicalCodeValue;

		public X12_DSB(X12Segment seg) {
			DisabilityTypeCode=seg.Get(1);
			ProductServiceIdQualifier=seg.Get(7);
			MedicalCodeValue=seg.Get(8);
		}
	}

	public class X12_DTP {
		///<summary>DTP01</summary>
		public string DateTimeQualifier;
		///<summary>DTP02</summary>
		public string DateTimePeriodFormatQualifier;
		///<summary>DTP03</summary>
		public string DateTimePeriod;

		public X12_DTP(X12Segment seg) {
			DateTimeQualifier=seg.Get(1);
			DateTimePeriodFormatQualifier=seg.Get(2);
			DateTimePeriod=seg.Get(3);
		}

		public DateTime DateT() {
			if(DateTimePeriodFormatQualifier=="D8") {
				return X12Parse.ToDate(DateTimePeriod);
			}
			return DateTime.MinValue;
		}
	}

	public class X12_EC {
		///<summary>EC01</summary>
		public string EmploymentClassCode1;
		///<summary>EC02</summary>
		public string EmploymentClassCode2;
		///<summary>EC03</summary>
		public string EmploymentClassCode3;

		public X12_EC(X12Segment seg) {
			EmploymentClassCode1=seg.Get(1);
			EmploymentClassCode2=seg.Get(2);
			EmploymentClassCode3=seg.Get(3);
		}
	}

	public class X12_HLH {
		///<summary>HLH01</summary>
		public string HealthRelatedCode;
		///<summary>HLH02</summary>
		public string Height;
		///<summary>HLH03</summary>
		public string Weight;

		public X12_HLH(X12Segment seg) {
			HealthRelatedCode=seg.Get(1);
			Height=seg.Get(2);
			Weight=seg.Get(3);
		}
	}

	public class X12_ICM {
		///<summary>ICM01</summary>
		public string FrequencyCode;
		///<summary>ICM02</summary>
		public string MonetaryAmount;
		///<summary>ICM03</summary>
		public string Quantity;
		///<summary>ICM04</summary>
		public string LocationIdentifier;
		///<summary>ICM05</summary>
		public string SalaryGrade;

		public X12_ICM(X12Segment seg) {
			FrequencyCode=seg.Get(1);
			MonetaryAmount=seg.Get(2);
			Quantity=seg.Get(3);
			LocationIdentifier=seg.Get(4);
			SalaryGrade=seg.Get(5);
		}
	}

	public class X12_INS {
		///<summary>INS01</summary>
		public static string YesNoConditionOrResponseCode1;
		///<summary>INS02</summary>
		public static string IndividualRelationshipCode;
		///<summary>INS03</summary>
		public static string MaintenanceTypeCode;
		///<summary>INS04</summary>
		public static string MaintenanceReasonCode;
		///<summary>INS05</summary>
		public static string BenefitStatusCode;
		///<summary>INS06</summary>
		public static string MedicareStatusCode;
		///<summary>INS07</summary>
		public static string CobraQualifying;
		///<summary>INS08</summary>
		public static string EmploymentStatusCode;
		///<summary>INS09</summary>
		public static string StudentStatusCode;
		///<summary>INS10</summary>
		public static string YesNoConditionOrResponseCode2;
		///<summary>INS11</summary>
		public static string DateTimePeriodFormatQualifier;
		///<summary>INS12</summary>
		public static string DateTimePeriod;
		///<summary>INS13</summary>
		public static string ConfidentialityCode;
		///<summary>INS17</summary>
		public static string Number;

		public X12_INS(X12Segment seg) {
			YesNoConditionOrResponseCode1=seg.Get(1);
			IndividualRelationshipCode=seg.Get(2);
			MaintenanceTypeCode=seg.Get(3);
			MaintenanceReasonCode=seg.Get(4);
			BenefitStatusCode=seg.Get(5);
			MedicareStatusCode=seg.Get(6);
			CobraQualifying=seg.Get(7);
			EmploymentStatusCode=seg.Get(8);
			StudentStatusCode=seg.Get(9);
			YesNoConditionOrResponseCode2=seg.Get(10);
			DateTimePeriodFormatQualifier=seg.Get(11);
			DateTimePeriod=seg.Get(12);
			ConfidentialityCode=seg.Get(13);
			Number=seg.Get(17);
		}
	}

	public class X12_LUI {
		///<summary>LUI01</summary>
		public string IdentificationCodeQualifier;
		///<summary>LUI02</summary>
		public string IdentificationCode;
		///<summary>LUI03</summary>
		public string Description;
		///<summary>LUI04</summary>
		public string UseOfLanguageIndicator;

		public X12_LUI(X12Segment seg) {
			IdentificationCodeQualifier=seg.Get(1);
			IdentificationCode=seg.Get(2);
			Description=seg.Get(3);
			UseOfLanguageIndicator=seg.Get(4);
		}
	}

	public class X12_N1 {
		///<summary>N101</summary>
		public string EntityIdentifierCode;
		///<summary>N102</summary>
		private string Name;
		///<summary>N103</summary>
		private string IdentificationCodeQualifier;
		///<summary>N104</summary>
		private string IdentificationCode;

		public X12_N1(X12Segment seg) {
			EntityIdentifierCode=seg.Get(1);
			Name=seg.Get(2);
			IdentificationCodeQualifier=seg.Get(3);
			IdentificationCode=seg.Get(4);
		}
	}

	public class X12_N3 {
		///<summary>N301</summary>
		public string AddressInformation1;
		///<summary>N302</summary>
		public string AddressInformation2;

		public X12_N3(X12Segment seg) {
			AddressInformation1=seg.Get(1);
			AddressInformation2=seg.Get(2);
		}
	}

	public class X12_N4 {
		///<summary>N401</summary>
		public string CityName;
		///<summary>N402</summary>
		public string StateOrProvinceCode;
		///<summary>N403</summary>
		public string PostalCode;
		///<summary>N404</summary>
		public string CountryCode;
		///<summary>N405</summary>
		public string LocationQualifier;
		///<summary>N406</summary>
		public string LocationIdentifier;
		///<summary>N407</summary>
		public string CountrySubdivisionCode;

		public X12_N4(X12Segment seg) {
			CityName=seg.Get(1);
			StateOrProvinceCode=seg.Get(2);
			PostalCode=seg.Get(3);
			CountryCode=seg.Get(4);
			LocationQualifier=seg.Get(5);
			LocationIdentifier=seg.Get(6);
			CountrySubdivisionCode=seg.Get(7);
		}
	}

	public class X12_NM1 {
		///<summary>NM101</summary>
		public string EntityIdentifierCode;
		///<summary>NM102</summary>
		public string EntityTypeQualifier;
		///<summary>NM103</summary>
		public string NameLast;
		///<summary>NM104</summary>
		public string NameFirst;
		///<summary>NM105</summary>
		public string NameMiddle;
		///<summary>NM106</summary>
		public string NamePrefix;
		///<summary>NM107</summary>
		public string NameSuffix;
		///<summary>NM108</summary>
		public string IdentificationCodeQualifier;
		///<summary>NM109</summary>
		public string IdentificationCode;

		public X12_NM1(X12Segment seg) {
			EntityIdentifierCode=seg.Get(1);
			EntityTypeQualifier=seg.Get(2);
			NameLast=seg.Get(3);
			NameFirst=seg.Get(4);
			NameMiddle=seg.Get(5);
			NamePrefix=seg.Get(6);
			NameSuffix=seg.Get(7);
			IdentificationCodeQualifier=seg.Get(8);
			IdentificationCode=seg.Get(9);
		}
	}

	public class X12_PER {
		///<summary>PER01</summary>
		public string ContactFunctionCode;
		///<summary>PER03</summary>
		public string CommunicationNumberQualifier1;
		///<summary>PER04</summary>
		public string CommunicationNumber1;
		///<summary>PER05</summary>
		public string CommunicationNumberQualifier2;
		///<summary>PER06</summary>
		public string CommunicationNumber2;
		///<summary>PER07</summary>
		public string CommunicationNumberQualifier3;
		///<summary>PER08</summary>
		public string CommunicationNumber3;

		public X12_PER(X12Segment seg) {
			ContactFunctionCode=seg.Get(1);
			CommunicationNumberQualifier1=seg.Get(3);
			CommunicationNumber1=seg.Get(4);
			CommunicationNumberQualifier2=seg.Get(5);
			CommunicationNumber2=seg.Get(6);
			CommunicationNumberQualifier3=seg.Get(7);
			CommunicationNumber3=seg.Get(8);
		}
	}

	public class X12_REF {
		///<summary>REF01</summary>
		public string ReferenceIdQualifier;
		///<summary>REF02</summary>
		public string ReferenceId;

		public X12_REF(X12Segment seg) {
			ReferenceIdQualifier=seg.Get(1);
			ReferenceId=seg.Get(2);
		}
	}

	public class X12_QTY {
		///<summary>QTY01</summary>
		public string QuantityQualifier;
		///<summary>QTY02</summary>
		public string Quantity;

		public X12_QTY(X12Segment seg) {
			QuantityQualifier=seg.Get(1);
			Quantity=seg.Get(2);
		}

	}

	#endregion Segments

}












