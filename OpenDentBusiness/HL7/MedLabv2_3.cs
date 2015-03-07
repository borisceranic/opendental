using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness.HL7 {
	///<summary></summary>
	public class MedLabv2_3 {

		public static HL7Def GetDeepInternal(HL7Def def) {
			//ok to pass in null
			if(def==null) {//wasn't in the database
				def=new HL7Def();
				def.IsNew=true;
				def.Description="MedLab version 2.3";
				def.ModeTx=ModeTxHL7.File;
				def.IncomingFolder="";
				def.OutgoingFolder="";
				def.IncomingPort="";
				def.OutgoingIpPort="";
				def.FieldSeparator="|";
				def.ComponentSeparator="^";
				def.SubcomponentSeparator="&";
				def.RepetitionSeparator="~";
				def.EscapeCharacter=@"\";
				def.IsInternal=true;
				def.InternalType=HL7InternalType.MedLabv2_3;
				def.InternalTypeVersion=Assembly.GetAssembly(typeof(Db)).GetName().Version.ToString();
				def.IsEnabled=false;
				def.Note="";
				def.ShowDemographics=HL7ShowDemographics.ChangeAndAdd;//these last four properties will not be editable for a lab interface type
				def.ShowAccount=true;
				def.ShowAppts=true;
				def.IsQuadAsToothNum=false;
			}
			def.hl7DefMessages=new List<HL7DefMessage>();
			HL7DefMessage msg=new HL7DefMessage();
			HL7DefSegment seg=new HL7DefSegment();
			#region Inbound Messages
				#region ORU - Unsolicited Observation Message
				def.AddMessage(msg,MessageTypeHL7.ORU,MessageStructureHL7.ORU_R01,InOutHL7.Incoming,0);
					#region MSH - Message Header
					msg.AddSegment(seg,0,SegmentNameHL7.MSH);
						//Fields-------------------------------------------------------------------------------------------------------------
						//MSH.2, Sending Application.  To identify the LabCorp Lab System sending the results.
						//Possible values for LabCorp (as of their v10.7 specs): '1100' - LabCorp Lab System, 'DIANON' - DIANON Systems,
						//'ADL' - Acupath Diagnostic Laboratories, 'EGL' - Esoterix Genetic Laboratories.
						//For backward compatibility only: 'CMBP', 'LITHOLINK', 'USLABS'
						seg.AddField(2,"sendingApp");
						//MSH.3, Sending Facility.  Identifies the LabCorp laboratory responsible for the client.
						//It could be a LabCorp assigned 'Responsible Lab Code' representing the responsible laboratory or it could be a CLIA number.
						seg.AddField(3,"sendingFacility");
						//MSH.8, Message Type
						seg.AddField(8,"messageType");
						//MSH.9, Message Control ID
						seg.AddField(9,"messageControlId");
					#endregion MSH - Message Header
					#region PID - Patient Identification
					seg=new HL7DefSegment();
					msg.AddSegment(seg,1,SegmentNameHL7.PID);
						//Fields-------------------------------------------------------------------------------------------------------------
						//PID.2, External Patient ID.  LabCorp defines this as 'client' assigned patient id, just like they do PID.4.
						//This should be the Open Dental patient number, sent in outbound PID.4 and returned in PID.2.
						seg.AddField(2,"pat.PatNum");
						//PID.3, Lab Assigned Patient ID.  LabCorp assigned specimen number.
						seg.AddField(3,"labPatId");
						//PID.4, Alternate Patient ID.  LabCorp defines this as a 'client' assigned patient id, just like they do PID.2.
						//This will be in outbound PID.2, returned in PID.4.
						seg.AddField(4,"altPatId");
						//PID.5, Patient Name
						//This will contain the last, first, and middle names as well as the title
						//Example:  LName^FName^MiddleI
						seg.AddField(5,"pat.nameLFM");
						//PID.7, Date/Time of Birth with Age
						//LabCorp uses this for the birthdate as well as the age in years, months, and days of the patient in the format bday^years^months^days.
						//All age components are left padded with 0's, the years is padded to 3 chars, the months and days are padded to 2 chars
						//Example: 19811213^033^02^19
						seg.AddField(7,"patBirthdateAge");
						//PID.18, Patient Account Number.  LabCorp assigned account number.  This field is also used to send the Fasting flag in component 7.
						//Fasting flag values are 'Y', 'N', or blank
						seg.AddField(18,"accountNum");
					#endregion PID - Patient Identification
					#region NK1 - Next of Kin
					//This segment is for future use only, nothing is currently imported from this segment
					seg=new HL7DefSegment();
					msg.AddSegment(seg,2,false,true,SegmentNameHL7.NK1);
						//Fields-------------------------------------------------------------------------------------------------------------
						//NK1.2, Next of Kin Name
						//Example: LName^FName^Middle
						//seg.AddField(2,"nextOfKinName");
						//NK1.4, Next of Kin Address
						//Example: Address^Address2^City^State^Zip
						//seg.AddField(4,"nextOfKinAddress");
						//NK1.5, Next of Kin Phone
						//seg.AddField(5,"nextOfKinPhone");
					#endregion NK1 - Next of Kin
					#region NTE - Notes and Comments
					seg=new HL7DefSegment();
					msg.AddSegment(seg,3,true,true,SegmentNameHL7.NTE);
						//Fields-------------------------------------------------------------------------------------------------------------
						//NTE.2, Comment Source, ID data type
						//LabCorp supported values: 'L' - Laboratory is the source of comment, 'AC' - Accession Comment,
						//'RC' - Result comment, 'RI' - Normal Comment, 'UK' - Undefined comment type
						//We might pull out the source and prepend it to the note, but we won't explicitly add it to the definition or store it separately
						//NTE.3, Comment Text, FT data type (formatted text)
						//Stored in the medlab.NotePat field
						seg.AddField(3,"patNote");
					#endregion NTE - Notes and Comments
					#region ORC - Common Order
					seg=new HL7DefSegment();
					msg.AddSegment(seg,4,true,false,SegmentNameHL7.ORC);
						//Fields-------------------------------------------------------------------------------------------------------------
						//ORC.2, Unique Foreign Accession or Specimen ID
						//Must match the value in OBR.2 and is the ID value sent on the specimen container and is unique per patient order, not test order.
						//ORC.2.2 is the constant value 'LAB'
						//Example: L2435^LAB
						seg.AddField(2,"specimenId");
						//ORC.12, Ordering Provider, XCN Data Type
						//ProvID^ProvLName^ProvFName^ProvMiddleI^^^^SourceTable
						//This field repeats for every ID available for the provider with the SourceTable component identifying the type of ID in each repetition.
						//SourceTable possible values: 'U' - UPIN, 'P' - Provider Number (Medicaid or Commercial Insurance Provider ID),
						//'N' - NPI Number (Required for Third Party Billing), 'L' - Local (Physician ID)
						//Example: A12345^LNAME^FNAME^M^^^^U~23462^LNAME^FNAME^M^^^^L~0123456789^LNAME^FNAME^M^^^^N~1234567890^LNAME^FNAME^M^^^^P
						seg.AddField(12,"orderingProv");
					#endregion ORC - Common Order
					#region OBR - Observation Request
					seg=new HL7DefSegment();
					msg.AddSegment(seg,5,true,false,SegmentNameHL7.OBR);
						//Fields-------------------------------------------------------------------------------------------------------------
						//OBR.3, Unique Foreign Accession or Specimen ID
						//Must match the value in ORC.2 and is the ID value sent on the specimen container and is unique per patient order, not test order.
						//OBR.2.2 is the constant value 'LAB'
						//Example: L2435^LAB
						seg.AddField(2,"specimenId");
						//OBR.4, Universal Service Identifier, CWE data type
						//This identifies the observation.  This will be the ID and text description of the test, as well as the LOINC code and description.
						//Example: 006072^RPR^L^20507-0^Reagin Ab^LN
						seg.AddField(4,"obsTestId");
						//OBR.7, Observation/Specimen Collection Date/Time
						//Format for LabCorp: yyyyMMddHHmm
						seg.AddField(7,"dateTimeCollected");
						//OBR.9, Collection/Urine Volume
						seg.AddField(9,"totalVolume");
						//OBR.11, Action Code
						//Used to identify the type of result being returned.  'A' - Add on, 'G' - Reflex, Blank for standard results
						seg.AddField(11,"specimenAction");
						//OBR.13, Relevant Clinical Information.  Used for informational purposes.
						seg.AddField(13,"clinicalInfo");
						//OBR.14, Date/Time of Specimen Receipt in Lab.
						//LabCorp format: yyyMMddHHmm
						seg.AddField(14,"dateTimeEntered");
						//OBR.16, Ordering Provider.
						//ProvID^ProvLName^ProvFName^ProvMiddleI^^^^SourceTable
						//This field repeats for every ID available for the provider with the SourceTable component identifying the type of ID in each repetition.
						//SourceTable possible values: 'U' - UPIN, 'P' - Provider Number (Medicaid or Commercial Insurance Provider ID),
						//'N' - NPI Number (Required for Third Party Billing), 'L' - Local (Physician ID)
						//Example: A12345^LNAME^FNAME^M^^^^U~23462^LNAME^FNAME^M^^^^L~0123456789^LNAME^FNAME^M^^^^N~1234567890^LNAME^FNAME^M^^^^P
						seg.AddField(16,"orderingProv");
						//OBR.18, Alternate Specimen ID.
						seg.AddField(18,"specimenIdAlt");
						//OBR.22, Date/Time Observation Reported.
						//LabCorp format: yyyyMMddHHmm
						seg.AddField(22,"dateTimeReported");
						//OBR.25, Order Result Status.
						//LabCorp values: 'F' - Final, 'P' - Preliminary, 'X' - Cancelled, 'C' - Corrected
						seg.AddField(25,"resultStatus");
						//OBR.26, Link to Parent Result.
						//If this is a reflex result, the value from the OBX.3.1 field of the parent result will be here.
						seg.AddField(26,"parentObsId");
						//OBR.29, Link to Parent Order.
						//If this is a reflex test, the value from the OBR.4.1 field of the parent test will be here.
						seg.AddField(29,"parentObsTestId");
					#endregion OBR - Observation Request
					#region NTE - Notes and Comments
					seg=new HL7DefSegment();
					msg.AddSegment(seg,6,true,true,SegmentNameHL7.NTE);
						//Fields-------------------------------------------------------------------------------------------------------------
						//NTE.2, Comment Source, ID data type
						//LabCorp supported values: 'L' - Laboratory is the source of comment, 'AC' - Accession Comment,
						//'RC' - Result comment, 'RI' - Normal Comment, 'UK' - Undefined comment type
						//We might pull out the source and prepend it to the note, but we won't explicitly add it to the definition or store it separately
						//NTE.3, Comment Text, FT data type (formatted text)
						//Stored in the medlab.NoteLab field
						seg.AddField(3,"labNote");
					#endregion NTE - Notes and Comments
					#region OBX - Observation/Result
					seg=new HL7DefSegment();
					msg.AddSegment(seg,7,true,false,SegmentNameHL7.OBX);
						//Fields-------------------------------------------------------------------------------------------------------------
						//OBX.3, Observation ID.  This field has the same structure as the OBR.4.
						//ID^Text^CodeSystem^AltID^AltIDText^AltIDCodeSystem, the AltID is the LOINC code so the AltIDCodeSystem will be 'LN'
						//Example: 006072^RPR^L^20507-0^Reagin Ab^LN
						seg.AddField(3,"obsId");
						//OBX.5, Observation Value.
						//ObsValue^TypeOfData^DataSubtype^Encoding^Data
						//LabCorp report will display OBX.5.1 as the result
						seg.AddField(5,"obsValue");
						//OBX.6, Units.
						//Identifier^Text^CodeSystem.  Id is units of measure abbreviation, text is full text version of units, coding system is 'L' (local id).
						seg.AddField(6,"obsUnits");
						//OBX.7, Reference Range.
						seg.AddField(7,"obsRefRange");
						//OBX.8, Abnormal Flags.  For values see enum OpenDentBusiness.AbnormalFlag
						seg.AddField(8,"obsAbnormalFlag");
						//OBX.11, Observation Result Status.
						//LabCorp values: 'F' - Final, 'P' - Preliminary, 'X' - Cancelled, 'C' - Corrected, 'I' - Incomplete
						seg.AddField(11,"resultStatus");
						//OBX.14, Date/Time of Observation.
						//LabCorp format yyyyMMddHHmm
						seg.AddField(14,"dateTimeObs");
						//OBX.15, Producer's ID.
						//For LabCorp this is used to report the facility responsible for performing the testing.  This will hold the lab ID that will reference
						//a ZPS segment with the lab facility name, address, and director details.
						seg.AddField(15,"facilityId");
					#endregion OBX - Observation/Result
					#region ZEF - Encapsulated Data Format
					seg=new HL7DefSegment();
					msg.AddSegment(seg,8,true,true,SegmentNameHL7.ZEF);
						//Fields-------------------------------------------------------------------------------------------------------------
						//ZEF.3, Embedded File.
						//Base64 embedded file, sent in 50k blocks and will be concatenated together to and converted back into
						seg.AddField(3,"base64File");
					#endregion ZEF - Encapsulated Data Format
					#region NTE - Notes and Comments
					seg=new HL7DefSegment();
					msg.AddSegment(seg,9,true,true,SegmentNameHL7.NTE);
						//Fields-------------------------------------------------------------------------------------------------------------
						//NTE.2, Comment Source, ID data type
						//LabCorp supported values: 'L' - Laboratory is the source of comment, 'AC' - Accession Comment,
						//'RC' - Result comment, 'RI' - Normal Comment, 'UK' - Undefined comment type
						//We might pull out the source and prepend it to the note, but we won't explicitly add it to the definition or store it separately
						//NTE.3, Comment Text, FT data type (formatted text)
						//Stored in the medlabresult.Note field
						seg.AddField(3,"obsNote");
					#endregion NTE - Notes and Comments
					#region SPM - Specimen
					seg=new HL7DefSegment();
					msg.AddSegment(seg,10,true,true,SegmentNameHL7.SPM);
						//Fields-------------------------------------------------------------------------------------------------------------
						//SPM.2, Specimen ID.
						//Unique ID of the specimen as sent on the specimen container.  Same as the value in ORC.2.
						seg.AddField(2,"specimenId");
						//SPM.4, Specimen Type
						//SPM.8, Specimen Source Site
						//SPM.4, Specimen Source Site Modifier
						//SPM.14, Specimen Description.  Text field used to send additional information about the specimen
						seg.AddField(14,"specimenDescript");
						//SPM.17, Date/Time Specimen Collected
						seg.AddField(17,"dateTimeSpecimen");
					#endregion SPM - Specimen
					#region ZPS - Place of Service
					seg=new HL7DefSegment();
					msg.AddSegment(seg,11,true,false,SegmentNameHL7.ZPS);
						//Fields-------------------------------------------------------------------------------------------------------------
						//ZPS.2, Facility Mnemonic.  Footnote ID for the lab facility used to reference this segment from OBX.15.
						seg.AddField(2,"facilityId");
						//ZPS.3, Facility Name.
						seg.AddField(3,"facilityName");
						//ZPS.4, Facility Address.
						//Address^^City^State^Zip
						seg.AddField(4,"facilityAddress");
						//ZPS.5, Facility Phone.  (LabCorp document says numberic characters only, so we can assume no dashes or parentheses.)
						seg.AddField(5,"facilityPhone");
						//ZPS.7, Facility Director.
						//Title^LName^FName^MiddleI
						seg.AddField(7,"facilityDirector");
					#endregion ZPS - Place of Service
				#endregion ORU - Unsolicited Observation Message
			#endregion Inbound Messages
			#region Outbound Messages
				//Results only interface for now, so no outbound messages yet
				//In the future, we will implement the orders portion of the interface and have outbound ORM messages
			#endregion Outbound Messages
			return def;
		}

	}
}

