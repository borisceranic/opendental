using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenDentBusiness
{
	///<summary>X12 835 Health Care Claim Payment/Advice. This transaction type is a response to an 837 claim submission. The 835 will always come after a 277 is received and a 277 will always come after a 999. Neither the 277 nor the 999 are required, so it is possible that an 835 will be received directly after the 837. The 835 is not required either, so it is possible that none of the 997, 999, 277 or 835 reports will be returned from the carrier.</summary>
	public class X835:X12object {
		
		///<summary>All segments within the transaction.</summary>
    private List<X12Segment> _listSegments;
		///<summary>BPR segment (pg. 69). Financial Information segment. Required.</summary>
		private X12Segment _segBPR;
		///<summary>TRN segment (pg. 77). Reassociation Trace Number segment. Required.</summary>
		private X12Segment _segTRN;
		///<summary>N1*PR segment of loop 1000A (pg. 87). Payer Identification segment. Required.</summary>
		private X12Segment _segN1_PR;
		///<summary>N3 segment of loop 1000A (pg. 89). Payer Address. Required.</summary>
		private X12Segment _segN3_PR;
		///<summary>N4 segment of loop 1000A (pg. 90). Payer City, Sate, Zip code. Required.</summary>
		private X12Segment _segN4_PR;
		///<summary>PER*BL segment of loop 1000A (pg. 97). Payer technical contact information. Required (but is not included in any of the examples, so we assume situational). Can repeat more than once, but we only are about the first occurrence.</summary>
		private X12Segment _segPER_BL;
		///<summary>N1*PE segment of loop 1000B (pg. 102). Payee identification. Required. We include this information because it could be helpful for those customers who are using clinics.</summary>
		private X12Segment _segN1_PE;
		///<summary>CLP of loop 2100 (pg. 123). Claim payment information.</summary>
		private List<int> _listSegNumsCLP;
		///<summary>SVC of loop 2110 (pg. 186). Service (procedure) payment information.</summary>
		private List<int> _listSegNumsSVC;
		///<summary>PLB segments (pg.217). Provider Adjustment. Situational. This is the footer and table 3 if pesent.</summary>
		private List<int> _listSegNumsPLB;

    public static bool Is835(X12object xobj) {
      if(xobj.FunctGroups.Count!=1) {//Exactly 1 GS segment in each 835.
        return false;
      }
      if(xobj.FunctGroups[0].Header.Get(1)=="HP") {//GS01 (pg. 279)
        return true;
      }
      return false;
    }

		///<summary>See guide page 62 for format outline.</summary>
    public X835(string messageText):base(messageText) {
			//Table 1 - Header
			//ST: Transaction Set Header.  Required.  Repeat 1.  Guide page 68.  The GS segment contains exactly one ST segment below it.
      _listSegments=FunctGroups[0].Transactions[0].Segments;
			//BPR: Financial Information.  Required.  Repeat 1.  Guide page 69.
			_segBPR=_listSegments[0];
			//TRN: Reassociation Trace Number.  Required.  Repeat 1.  Guide page 77.
			_segTRN=_listSegments[1];
			//CUR: Foreign Currency Information.  Situational.  Repeat 1.  Guide page 79.  We do not use.
			//REF: Receiver Identification.  Situational.  Repeat 1.  Guide page 82.  We do not use.
			//REF: Version Identification.  Situational.  Repeat 1.  Guide page 84.  We do not use.
			//DTM: Production Date.  Situational.  Repeat 1.  Guide page 85.  We do not use.
			_listSegNumsCLP=new List<int>();
			_listSegNumsSVC=new List<int>();
			_listSegNumsPLB=new List<int>();
			for(int i=0;i<_listSegments.Count;i++) {
				X12Segment seg=_listSegments[i];
				//Loop 1000A Payer Identification. Repeat 1.
				if(seg.SegmentID=="N1" && seg.Get(1)=="PR") {//Locates the first segment in loop 1000A.
					//1000A N1: Payer Identification. Required.  Repeat 1. Guide page 87.
					_segN1_PR=seg;
					//1000A N3: Payer Address.  Required.  Repeat 1.  Guide page 89.
					_segN3_PR=_listSegments[i+1];
					//1000A N4: Payer City, State, ZIP Code.  Required.  Repeat 1.  Guide page 90.
					_segN4_PR=_listSegments[i+2];
					//1000A REF: Additional Payer Identification.  Situational.  Repeat 4.  Guide page 92.  We do not use.
					//1000A PER: Payer Business Contact Information.  Situational.  Repeat 1.  Guide page 94.  We do not use.
					//1000A PER: Payer Technical Contact Information.  Required (but is not included in any of the examples, so we assume situational).  Repeat >1.  Guide page 97.  We only care about the first occurrence.
					for(int j=i+3;j<i+6;j++) {//Since the previous 2 segments are situational, we must skip them if they exist.
						if(_listSegments[j].SegmentID=="PER" && _listSegments[j].Get(1)=="BL") {
							_segPER_BL=_listSegments[j];
							break;
						}
					}
					//1000A PER: Payer WEB Site.  Situational.  Repeat 1.  Guide page 100.  We do not use.
				}
				//Loop 1000B Payee Identification.  Repeat 1.
				if(seg.SegmentID=="N1" && seg.Get(1)=="PE") {//Locates the first segment in loop 1000B.
					//1000B N1: Payee Identification.  Required.  Repeat 1.  Guide page 102.
					_segN1_PE=seg;
					//1000B N3: Payee Address.  Situational.  Repeat 1.  Guide page 104.  We do not use because the payee already knows their own address, and because it is not required.
					//1000B N4: Payee City, State, ZIP Code.  Situational.  Repeat 1.  Guide page 105.  We do not use because the payee already knows their own address, and because it is not required.
					//1000B REF: Payee Additional Identification.  Situational.  Repeat >1.  Guide page 107.  We do not use.
					//1000B RDM: Remittance Delivery Method.  Situational.  Repeat 1.  Guide page 109.  We do not use.
				}
				//Table 2 - Detail
				//Loop 2000 Header Number.  Repeat >1.  We do not need the information in this loop, because claim payments include the unique claim identifiers that we need to match to the claims one-by-one.
				//2000 LX: Header Number.  Situational.  Repeat 1.  Guide page 111.  We do not use.
				//2000 TS3: Provider Summary Information.  Repeat 1.  Guide page 112.  We do not use.
				//2000 TS2: Provider Supplemental Summary Infromation.  Guide page 117.  We do not use.
				//Loop 2100 Claim Payment Information.  Repeat >1.
				if(seg.SegmentID=="CLP") {//The CLP segment only exists one time within the 2100 loop.
					//2100 CLP: Claim Payment Information.  Required.  Repeat 1.  Guide page 123.
					_listSegNumsCLP.Add(i);
					//2100 CAS: Claim Adjustment.  Situational.  Repeat 99.  Guide page 129.  We do not use.
					//2100 NM1: Patient Name.  Required.  Repeat 1.  Guide page 137.  We do not use.
					//2100 NM1: Insured Name.  Situational.  Repeat 1.  Guide page 140.  We do not use.
					//2100 NM1: Corrected Patient/Insured Name.  Situational.  Repeat 1.  Guide page 143.  We do not use.
					//2100 NM1: Service Provider Name.  Situational.  Repeat 1.  Guide page 146.  We do not use.
					//2100 NM1: Crossover Carrier Name.  Situational.  Repeat 1.  Guide page 150.  We do not use.
					//2100 NM1: Corrected Priority Payer Name.  Situational.  Repeat 1.  Guide page 153.  We do not use.
					//2100 NM1: Other Subscriber Name.  Situational.  Repeat 1.  Guide page 156.  We do not use.
					//2100 MIA: Inpatient Adjudication Information.  Situational.  Repeat 1.  Guide page 159. 
					//2100 MOA: Outpatient Adjudication Information.  Situational.  Repeat 1.  Guide page 166.
					//2100 REF: Other Claim Releated Identification.  Situational.  Repeat 5.  Guide page 169.  We do not use.
					//2100 REF: Rendering Provider Identification.  Situational.  Repeat 10.  Guide page 171.  We do not use.
					//2100 DTM: Statement From or To Date.  Situational.  Repeat 2.  Guide page 173.  We do not use.
					//2100 DTM: Coverage Expiration Date.  Situational.  Repeat 1.  Guide page 175.  We do not use.
					//2100 DTM: Claim Received Date.  Situational.  Repeat 1.  Guide page 177.  We do not use.
					//2100 PER: Claim Contact Information.  Situational.  Repeat 2.  Guide page 179.  We do not use.
					//2100 AMT: Claim Supplemental Information.  Situational.  Repeat 13.  Guide page 182.  We do not use.
					//2100 QTY: Claim Supplemental Information Quantity.  Situational.  Repeat 14.  Guide page 184.  We do not use.
				}
				//Loop 2110 Service Payment Information.  Repeat 999.
				if(seg.SegmentID=="SVC") {//The SVC segment only exists one time within the 2110 loop.
					//2110 SVC: Service Payment Information.  Situational.  Repeat 1.  Guide page 186.
					_listSegNumsSVC.Add(i);
					//2110 DTM: Service Date.  Situational.  Repeat 2.  Guide page 194.  We do not use.
					//2110 CAS: Service Adjustment.  Situational.  Repeat 99.  Guide page 196.
					//2110 REF: Service Identification.  Situational.  Repeat 8.  Guide page 204.  We do not use.
					//2110 REF: Line Item Control Number.  Situational.  Repeat 1.  Guide page 206.
					//2110 REF: Rendering Provider Information.  Situational.  Repeat 10.  Guide page 207.  We do not use.
					//2110 REF: HealthCare Policy Identification.  Situational.  Repeat 5.  Guide page 209.  We do not use.
					//2110 AMT: Service Supplemental Amount.  Situational.  Repeat 9.  Guide page 211.  We do not use.
					//2110 QTY: Service Supplemental Quantity.  Situational.  Repeat 6.  Guide page 213.  We do not use.
					//2110 LQ: Health Care Remark Codes.  Repeat 99.  Guide page 215.
				}
				//Table 3 - Summary
				if(seg.SegmentID=="PLB") {
					//PLB: Provider Admustment.  Situational.  Repeat >1.  Guide page 217.
					_listSegNumsPLB.Add(i);
				}
			}
    }

		#region Header
		///<summary>Gets the description for the transaction handling code in Table 1 (Header) BPR01. Required.</summary>
		public string GetTransactionHandlingCodeDescription() {
			string transactionHandlingCode=_segBPR.Get(1);
			if(transactionHandlingCode=="C") {
				return "Payment Accompanies Remittance Advice";
			}
			else if(transactionHandlingCode=="D") {
				return "Make Payment Only";
			}
			else if(transactionHandlingCode=="H") {
				return "Notification Only";
			}
			else if(transactionHandlingCode=="I") {
				return "Remittance Information Only";
			}
			else if(transactionHandlingCode=="P") {
				return "Prenotification of Future Transfers";
			}
			else if(transactionHandlingCode=="U") {
				return "Split Payment and Remittance";
			}
			else if(transactionHandlingCode=="X") {
				return "Handling Party's Option to Split Payment and Remittance";
			}
			return "UNKNOWN";
		}

		///<summary>Gets the payment credit or debit amount in Table 1 (Header) BPR02. Required.</summary>
		public string GetPaymentAmount() {
			return PIn.Decimal(_segBPR.Get(2)).ToString("f2");
		}

		///<summary>Gets the payment credit or debit flag in Table 1 (Header) BPR03. Returns true if "Credit" or false if "Debit". Required.</summary>
		public bool IsCredit() {
			string creditDebitFlag=_segBPR.Get(3);
			if(creditDebitFlag=="C") {
				return true;
			}
			return false;
		}

		///<summary>Gets the description for the payment method in Table 1 (Header) BPR04. Required.</summary>
		public string GetPaymentMethodDescription() {
			string paymentMethodCode=_segBPR.Get(4);
			if(paymentMethodCode=="ACH") {
				return "Automated Clearing House (ACH)";
			}
			else if(paymentMethodCode=="BOP") {
				return "Financial Institution Option";
			}
			else if(paymentMethodCode=="CHK") {
				return "Check";
			}
			else if(paymentMethodCode=="FWT") {
				return "Federal Reserve Funds/Wire Transfer - Nonrepetitive";
			}
			else if(paymentMethodCode=="NON") {
				return "Non-payment Data";
			}
			return "UNKNOWN";
		}

		///<summary>Gets the last 4 digits of the account number for the receiving company (the provider office) in Table 1 (Header) BPR15. Situational. If not present, then returns empty string.</summary>
		public string GetAccountNumReceivingShort() {
			string accountNumber=_segBPR.Get(15);
			if(accountNumber.Length<=4) {
				return accountNumber;
			}
			return accountNumber.Substring(accountNumber.Length-4);
		}

		///<summary>Gets the effective payment date in Table 1 (Header) BPR16. Required.</summary>
		public DateTime GetDateEffective() {
			string dateEffectiveStr=_segBPR.Get(16);//BPR16 will be blank if the payment is a check.
			if(dateEffectiveStr.Length<8) {
				return DateTime.MinValue;
			}
			int dateEffectiveYear=int.Parse(dateEffectiveStr.Substring(0,4));
			int dateEffectiveMonth=int.Parse(dateEffectiveStr.Substring(4,2));
			int dateEffectiveDay=int.Parse(dateEffectiveStr.Substring(6,2));
			return new DateTime(dateEffectiveYear,dateEffectiveMonth,dateEffectiveDay);
		}

		///<summary>Gets the check number or transaction reference number in Table 1 (Header) TRN02. Required.</summary>
		public string GetTransactionReferenceNumber() {
			return _segTRN.Get(2);
		}

		///<summary>Gets the payer name in Table 1 (Header) N102. Required.</summary>
		public string GetPayerName() {
			return _segN1_PR.Get(2);
		}

		///<summary>Gets the payer electronic ID in Table 1 (Header) N104 of segment N1*PR. Situational. If not present, then returns empty string.</summary>
		public string GetPayerID() {
			if(_segN1_PR.Elements.Length>=4) {
				return _segN1_PR.Get(4);
			}
			return "";
		}

		///<summary>Gets the payer address line 1 in Table 1 (Header) N301 of segment N1*PR. Required.</summary>
		public string GetPayerAddress1() {
			return _segN3_PR.Get(1);
		}

		///<summary>Gets the payer city name in Table 1 (Header) N401 of segment N1*PR. Required.</summary>
		public string GetPayerCityName() {
			return _segN4_PR.Get(1);
		}

		///<summary>Gets the payer state in Table 1 (Header) N402 of segment N1*PR. Required when in USA or Canada.</summary>
		public string GetPayerState() {
			return _segN4_PR.Get(2);
		}

		///<summary>Gets the payer zip code in Table 1 (Header) N403 of segment N1*PR. Required when in USA or Canada.</summary>
		public string GetPayerZip() {
			return _segN4_PR.Get(3);
		}

		///<summary>Gets the contact information from segment PER*BL. Phone/email in PER04 or the contact phone/email in PER06 or both. If neither PER04 nor PER06 are present, then returns empty string.</summary>
		public string GetPayerContactInfo() {
			if(_segPER_BL==null) {
				return "";
			}
			string contact_info="";
			if(_segPER_BL.Elements.Length>=4 && _segPER_BL.Get(4)!="") {//Contact number 1.
				contact_info=_segPER_BL.Get(4);
			}
			if(_segPER_BL.Elements.Length>=6 && _segPER_BL.Get(6)!="") {//Contact number 2.
				if(contact_info!="") {
					contact_info+=" or ";
				}
				contact_info+=_segPER_BL.Get(6);
			}
			if(_segPER_BL.Elements.Length>=8 && _segPER_BL.Get(8)!="") {//Telephone extension for contact number 2.
				if(contact_info!="") {
					contact_info+=" x";
				}
				contact_info+=_segPER_BL.Get(8);
			}
			return contact_info;
		}

		///<summary>Gets the payee name in Table 1 (Header) N102 of segment N1*PE. Required.</summary>
		public string GetPayeeName() {
			return _segN1_PE.Get(2);
		}

		///<summary>Gets a human readable description for the identifiation code qualifier found in N103 of segment N1*PE. Required.</summary>
		public string GetPayeeIdType() {
			string qualifier=_segN1_PE.Get(3);
			if(qualifier=="FI") {
				return "TIN";
			}
			else if(qualifier=="XV") {
				return "Medicaid ID";
			}
			else if(qualifier=="XX") {
				return "NPI";
			}
			return "";
		}

		///<summary>Gets the payee identification number found in N104 of segment N1*PE. Required. Usually the NPI number.</summary>
		public string GetPayeeId() {
			return _segN1_PE.Get(4);
		}

		#endregion Header
		#region Provider Level
		///<summary>Each item returned contains a string[] with values:
		///00 Provider NPI (PLB01), 
		///01 Fiscal Period Date (PLB02), 
		///02 ReasonCodeDescription, 
		///03 ReasonCode (PLB03-1 or PLB05-1 or PLB07-1 or PLB09-1 or PLB11-1 or PLB13-1 2/2), 
		///04 ReferenceIdentification (PLB03-2 or PLB05-2 or PLB07-2 or PLB09-2 or PLB11-2 or PLB13-2 1/50),
		///05 Amount (PLB04 1/18).</summary>
		public List<string[]> GetProviderLevelAdjustments() {
			List<string[]> result=new List<string[]>();
			for(int i=0;i<_listSegNumsPLB.Count;i++) {
				X12Segment segPLB=_listSegments[_listSegNumsPLB[i]];
				string provNPI=segPLB.Get(1);//PLB01 is required.
				string dateFiscalPeriodStr=segPLB.Get(2);//PLB02 is required.
				string dateFiscalPeriod="";
				try {
					int dateEffectiveYear=int.Parse(dateFiscalPeriodStr.Substring(0,4));
					int dateEffectiveMonth=int.Parse(dateFiscalPeriodStr.Substring(4,2));
					int dateEffectiveDay=int.Parse(dateFiscalPeriodStr.Substring(6,2));
					dateFiscalPeriod=(new DateTime(dateEffectiveYear,dateEffectiveMonth,dateEffectiveDay)).ToShortDateString();
				}
				catch {
					//Oh well, not very important infomration anyway.
				}
				//After PLB02, the segments are in pairs, with a minimum of one pair, and a maximum of six pairs.  Starting with PLB03 and PLB04 (both are required), the remaining pairs are optional.
				//Each pair represents a single provider adjustment and reason for adjustment.  The provider is identified in PLB01 by NPI.
				//There can be more than one PLB segment, therefore it is possible to create more than six adjustments for a single provider by creating more than one PLB segment.
				//The loop below is intended to capture all adjustments within the current PLB segment.
				int segNumAdjCode=3;//PLB03 and PLB04 are required.  We start at segment 3 and increment by 2 with each iteration of the loop.
				while(segNumAdjCode<segPLB.Elements.Length) {
					string reasonCode=segPLB.Get(segNumAdjCode,1);
					//For each adjustment reason code, the reference identification is situational.
					string referenceIdentification="";
					if(segPLB.Get(3).Length>reasonCode.Length) {
						referenceIdentification=segPLB.Get(3,2);
					}
					//For each adjustment reason code, an amount is required.
					string amount=PIn.Decimal(segPLB.Get(segNumAdjCode+1)).ToString("f2");
					result.Add(new string[] { provNPI,dateFiscalPeriod,GetDescriptForProvAdjCode(reasonCode),reasonCode,referenceIdentification,amount });
					segNumAdjCode+=2;
				}
			}
			return result;
		}
		#endregion Provider Level
		#region Claim Level

		///<summary>CLP01 in loop 2100. Referred to in this format as a Patient Control Number. Do this first to get a list of all claim tracking numbers that are contained within this 835.  Then, for each claim tracking number, we can later retrieve specific information for that single claim. The claim tracking numbers correspond to CLM01 exactly as submitted in the 837. We refer to CLM01 as the claim identifier on our end. We allow more than just digits in our claim identifiers, so we must return a list of strings.</summary>
		public List<string> GetClaimTrackingNumbers() {
			List<string> retVal=new List<string>();
			for(int i=0;i<_listSegNumsCLP.Count;i++) {
				X12Segment seg=_listSegments[_listSegNumsCLP[i]];//CLP segment.
				retVal.Add(seg.Get(1));//CLP01
			}
			return retVal;
		}

		///<summary>Result will contain strings in the following order:
		///00 Claim Status Code Description (CLP02)
		///01 Total Claim Charge Amount (CLP03)
		///02 Claim Payment Amount (CLP04)
		///03 Patient Portion Amount (CLP05)
		///04 Payer Claim Control Number (CLP07).</summary>
    public string[] GetClaimInfo(string claimTrackingNumber) {
      string[] result=new string[5];
      for(int i=0;i<result.Length;i++) {
        result[i]="";
      }
      for(int i=0;i<_listSegNumsCLP.Count;i++) {
        int segNum=_listSegNumsCLP[i];
				X12Segment segCLP=_listSegments[segNum];
				if(segCLP.Get(1)!=claimTrackingNumber) {//CLP01 Patient Control Number
					continue;
				}
				result[0]=GetDescriptForClaimStatusCode(segCLP.Get(2));//CLP02 Claim Status Code Description
				result[1]=PIn.Decimal(segCLP.Get(3)).ToString("f2");//CLP03 Total Claim Charge Amount
				result[2]=PIn.Decimal(segCLP.Get(4)).ToString("f2");//CLP04 Claim Payment Amount
				result[3]=PIn.Decimal(segCLP.Get(5)).ToString("f2");//CLP05 Patient Portion Amount
				result[4]=segCLP.Get(7);//CLP07 Payer Claim Control Number
				break;
      }
      return result;
    }

		///<summary>Returns a list of strings with values in fours, such that the first item is the adjustment description, the second is the reason description, and the third is the monetary amount, the fourth is the adjustment code (CO,PI,PR, or OA).</summary>
		public List<string> GetClaimAdjustmentInfo(string claimTrackingNumber) {
			List<X12Segment> listClaimCasSegments=new List<X12Segment>();
			for(int i=0;i<_listSegNumsCLP.Count;i++) {
				int segNum=_listSegNumsCLP[i];
				X12Segment segCLP=_listSegments[segNum];
				if(segCLP.Get(1)!=claimTrackingNumber) {//CLP01 Patient Control Number
					continue;
				}
				int startSegNum=segNum+1;
				int endSegNum=_listSegments.Count;//this variable tracks where the segments end for claim i.
				if(i<_listSegNumsCLP.Count-1) {
					endSegNum=_listSegNumsCLP[i+1];
				}
				for(int j=startSegNum;j<endSegNum;j++) {
					X12Segment seg=_listSegments[j];
					if(seg.SegmentID=="SVC") {
						//If a procedure segment is encountered, then we have finished processing all of the claim-level adjustments.  Discontinue processing.
						break;
					}
					if(seg.SegmentID!="CAS") {
						continue;//Not an adjustment segment.
					}
					listClaimCasSegments.Add(seg);					
				}
			}
			return ConvertCasAdjustmentsToHumanReadable(listClaimCasSegments);
		}

		///<summary>This function gets extra notes regarding the adjuducation of the claim if present.
		///The data comes from two different segments, MIA (Inpatient Adjudication Information) and MOA (Outpatient Adjudication Information).
		///Both segments MIA and MOA are situational, and if present, there will only be one or the other.
		///The values returned are in pairs, such that the first item in each pair is a field name and the second item in the pair is the field value.</summary>
		public List<string> GetClaimAdjudicationInfo(string claimTrackingNumber) {
			List<string> listAdjudicationInfo=new List<string>();
			for(int i=0;i<_listSegNumsCLP.Count;i++) {
				int segNum=_listSegNumsCLP[i];
				X12Segment segCLP=_listSegments[segNum];
				if(segCLP.Get(1)!=claimTrackingNumber) {//CLP01 Patient Control Number
					continue;
				}
				int startSegNum=segNum+1;
				int endSegNum=_listSegments.Count;//this variable tracks where the segments end for claim i.
				if(i<_listSegNumsCLP.Count-1) {
					endSegNum=_listSegNumsCLP[i+1];
				}
				for(int j=startSegNum;j<endSegNum;j++) {
					X12Segment seg=_listSegments[j];
					if(seg.SegmentID=="MIA") {
						if(seg.Get(1)!="") {
							listAdjudicationInfo.Add("Covered Days or Visits Count");		listAdjudicationInfo.Add(seg.Get(1));
						}
						if(seg.Get(2)!="") {
							listAdjudicationInfo.Add("PPS Operating Outlier Amount"); listAdjudicationInfo.Add(PIn.Decimal(seg.Get(2)).ToString("f2"));
						}
						if(seg.Get(3)!="") {
							listAdjudicationInfo.Add("Lifetime Psychiatric Days Count"); listAdjudicationInfo.Add(seg.Get(3));
						}
						if(seg.Get(4)!="") {
							listAdjudicationInfo.Add("Claim Diagnosis Related Group Amount"); listAdjudicationInfo.Add(PIn.Decimal(seg.Get(4)).ToString("f2"));
						}
						if(seg.Get(5)!="") {
							listAdjudicationInfo.Add("Claim Payment Remark"); listAdjudicationInfo.Add(GetDescriptFrom411(seg.Get(5)));
						}
						if(seg.Get(6)!="") {
							listAdjudicationInfo.Add("Disproportionate Share Amount"); listAdjudicationInfo.Add(PIn.Decimal(seg.Get(6)).ToString("f2"));
						}
						if(seg.Get(7)!="") {
							listAdjudicationInfo.Add("Medicare Secondary Payer (MSP) Pass-Through Amount"); listAdjudicationInfo.Add(PIn.Decimal(seg.Get(7)).ToString("f2"));
						}
						if(seg.Get(8)!="") {
							listAdjudicationInfo.Add("Prospective Payment System (PPS) Capital Amount"); listAdjudicationInfo.Add(PIn.Decimal(seg.Get(8)).ToString("f2"));
						}
						if(seg.Get(9)!="") {
							listAdjudicationInfo.Add("Prospectice Payment System (PPS) Capital, Federal Specific Portion, Diagnosis Related Group (DRG) Amount"); listAdjudicationInfo.Add(PIn.Decimal(seg.Get(9)).ToString("f2"));
						}
						if(seg.Get(10)!="") {
							listAdjudicationInfo.Add("Prospective Payment System (PPS) Capital, Hospital Specific Portion, Diagnosis Related Group (DRG) Amount"); listAdjudicationInfo.Add(PIn.Decimal(seg.Get(10)).ToString("f2"));
						}
						if(seg.Get(11)!="") {
							listAdjudicationInfo.Add("Prospective Payment System (PPS) Capital, Disproportionate Share, Hospital Diagnosis Related Group (DRG) Amount"); listAdjudicationInfo.Add(PIn.Decimal(seg.Get(11)).ToString("f2"));
						}
						if(seg.Get(12)!="") {
							listAdjudicationInfo.Add("Old Capital Amount"); listAdjudicationInfo.Add(PIn.Decimal(seg.Get(12)).ToString("f2"));
						}
						if(seg.Get(13)!="") {
							listAdjudicationInfo.Add("Prospective Payment System (PPS) Capital Indirect Medical Education Claim Amount"); listAdjudicationInfo.Add(PIn.Decimal(seg.Get(13)).ToString("f2"));
						}
						if(seg.Get(14)!="") {
							listAdjudicationInfo.Add("Hospital Specific Diagnosis Related Group (DRG) Amount"); listAdjudicationInfo.Add(PIn.Decimal(seg.Get(14)).ToString("f2"));
						}
						if(seg.Get(15)!="") {
							listAdjudicationInfo.Add("Cost Report Day Count"); listAdjudicationInfo.Add(seg.Get(15));
						}
						if(seg.Get(16)!="") {
							listAdjudicationInfo.Add("Federal Specific Diagnosis Related Group (DRG) Amount"); listAdjudicationInfo.Add(PIn.Decimal(seg.Get(16)).ToString("f2"));
						}
						if(seg.Get(17)!="") {
							listAdjudicationInfo.Add("Prospective Payment System (PPS) Capital Outlier Amount"); listAdjudicationInfo.Add(PIn.Decimal(seg.Get(17)).ToString("f2"));
						}
						if(seg.Get(18)!="") {
							listAdjudicationInfo.Add("Indirect Teaching Amount"); listAdjudicationInfo.Add(PIn.Decimal(seg.Get(18)).ToString("f2"));
						}
						if(seg.Get(19)!="") {
							listAdjudicationInfo.Add("Professional Component Amount Billed But Not Payable"); listAdjudicationInfo.Add(PIn.Decimal(seg.Get(19)).ToString("f2"));
						}
						if(seg.Get(20)!="") {
							listAdjudicationInfo.Add("Claim Payment Remark"); listAdjudicationInfo.Add(GetDescriptFrom411(seg.Get(20)));
						}
						if(seg.Get(21)!="") {
							listAdjudicationInfo.Add("Claim Payment Remark"); listAdjudicationInfo.Add(GetDescriptFrom411(seg.Get(21)));
						}
						if(seg.Get(22)!="") {
							listAdjudicationInfo.Add("Claim Payment Remark"); listAdjudicationInfo.Add(GetDescriptFrom411(seg.Get(22)));
						}
						if(seg.Get(23)!="") {
							listAdjudicationInfo.Add("Claim Payment Remark"); listAdjudicationInfo.Add(GetDescriptFrom411(seg.Get(23)));
						}
						if(seg.Get(24)!="") {
							listAdjudicationInfo.Add("Prospective Payment System (PPS) Capital Exception Amount"); listAdjudicationInfo.Add(PIn.Decimal(seg.Get(24)).ToString("f2"));
						}
					}
					else if(seg.SegmentID=="MOA") {
						if(seg.Get(1)!="") {
							listAdjudicationInfo.Add("Reimbursement Rate"); listAdjudicationInfo.Add((PIn.Decimal(seg.Get(1))*100).ToString()+"%");
						}
						if(seg.Get(2)!="") {
							listAdjudicationInfo.Add("Claim Health Care Financing Administration Common Procedural Coding System (HCPCS) Payable Amount"); listAdjudicationInfo.Add(PIn.Decimal(seg.Get(2)).ToString("f2"));
						}
						if(seg.Get(3)!="") {
							listAdjudicationInfo.Add("Claim Payment Remark"); listAdjudicationInfo.Add(GetDescriptFrom411(seg.Get(3)));
						}
						if(seg.Get(4)!="") {
							listAdjudicationInfo.Add("Claim Payment Remark"); listAdjudicationInfo.Add(GetDescriptFrom411(seg.Get(4)));
						}
						if(seg.Get(5)!="") {
							listAdjudicationInfo.Add("Claim Payment Remark"); listAdjudicationInfo.Add(GetDescriptFrom411(seg.Get(5)));
						}
						if(seg.Get(6)!="") {
							listAdjudicationInfo.Add("Claim Payment Remark"); listAdjudicationInfo.Add(GetDescriptFrom411(seg.Get(6)));
						}
						if(seg.Get(7)!="") {
							listAdjudicationInfo.Add("Claim Payment Remark"); listAdjudicationInfo.Add(GetDescriptFrom411(seg.Get(7)));
						}
						if(seg.Get(8)!="") {
							listAdjudicationInfo.Add("End Stage Renal Disease (ESRD) Payment Amount"); listAdjudicationInfo.Add(PIn.Decimal(seg.Get(8)).ToString("f2"));
						}
						if(seg.Get(9)!="") {
							listAdjudicationInfo.Add("Professional Component Amount Billed But Not Payable"); listAdjudicationInfo.Add(PIn.Decimal(seg.Get(9)).ToString("f2"));
						}
					}
				}
				break;
			}
			return listAdjudicationInfo;
		}

		#endregion Claim Level
		#region Procedure Level

		///<summary>Procedure level information is not required, but is useful if present.
		///The returned list of strings will include 5 values for each procedure found.  Fields will be in the following order:
		///00 SVC Segment Index within _listSegNumsSVC. More convenient than the line item number, beacuse it is always available and easier to use in later steps.
		///01 Procedure Code.
		///02 Procedure Fee.
		///03 Ins Paid Amount.
		///04 ProcNum (REF*6R from the 837), or blank for older claims.</summary>
		public List<string> GetProcInfo(string claimTrackingNumber) {
			List<string> listProcInfo=new List<string>();
			for(int i=0;i<_listSegNumsCLP.Count;i++) {
				int segNum=_listSegNumsCLP[i];
				X12Segment segCLP=_listSegments[segNum];
				if(segCLP.Get(1)!=claimTrackingNumber) {//CLP01 Patient Control Number
					continue;
				}
				int startSegNumCLP=segNum+1;
				int endSegNumCLP=_listSegments.Count;//this variable tracks where the segments end for claim i.
				if(i<_listSegNumsCLP.Count-1) {
					endSegNumCLP=_listSegNumsCLP[i+1];
				}
				for(int j=0;j<_listSegNumsSVC.Count;j++) {
					int segSvcIndex=_listSegNumsSVC[j];
					if(segSvcIndex>=startSegNumCLP && segSvcIndex<endSegNumCLP) {
						listProcInfo.Add(j.ToString());//00 SVC Segment Index.
						X12Segment segSVC=_listSegments[segSvcIndex];
						listProcInfo.Add(segSVC.Get(1).Split(new string[] { Separators.Subelement },StringSplitOptions.None)[1]);//01 SVC1-2: Procedure code.
						listProcInfo.Add(PIn.Decimal(segSVC.Get(2)).ToString("f2"));//02 SVC2: Procedure Fee.
						listProcInfo.Add(PIn.Decimal(segSVC.Get(3)).ToString("f2"));//03 SVC3: Ins Paid Amount.
						int segRefIndex=-1;
						for(int k=segSvcIndex+1;k<_listSegments.Count;k++) {//Find the first REF*6R segment.
							if(_listSegments[k].SegmentID=="REF" && _listSegments[k].Get(1)=="6R") {
								segRefIndex=k;
								break;
							}
						}
						//Back track to see if the located REF*6R segment belongs to the current procedure (because the REF*6F segments are optional and could be on some procedures but not others).
						bool isRefMatch=false;
						if(segRefIndex!=-1) {
							for(int k=segRefIndex;k>=segSvcIndex;k--) {
								if(_listSegments[k].SegmentID!="SVC") {
									continue;
								}
								if(k==segSvcIndex) {
									isRefMatch=true;
								}
								break;
							}
						}
						string procNum="";
						if(isRefMatch) {
							string strRef02=_listSegments[segRefIndex].Get(2);
							if(strRef02.StartsWith("p")) {
								//If the control number is prefixed with a "p", then it is a ProcNum.
								//Otherwise, for older versions, it will be the Line Counter from LX01 in the 837, which is basically an index.  We will ignore these older index based values.
								procNum=strRef02.Substring(1);//Remove the leading "p".
							}							
						}
						listProcInfo.Add(procNum);//04 REF02: Line Item Control Number.  This is the REF*6R from the 837.
					}
				}
			}
			return listProcInfo;
		}

		///<summaryReturns a list of strings with values in fours, such that the first item is the adjustment description, the second is the reason description, and the third is the monetary amount, the fourth is the adjustment code (CO,PI,PR, or OA).</summary>
		public List<string> GetProcAdjustmentInfo(int segSvcIndex) {
			List<X12Segment> listClaimCasSegments=new List<X12Segment>();
			int startSegNum=_listSegNumsSVC[segSvcIndex];
			int endSegNum=_listSegments.Count;
			if(segSvcIndex<_listSegNumsSVC.Count-1) {
				endSegNum=_listSegNumsSVC[segSvcIndex+1];
			}
			for(int j=startSegNum;j<endSegNum;j++) {
				X12Segment seg=_listSegments[j];
				if(seg.SegmentID=="CLP") {
					//If another claim segment is encountered, then we have finished processing all of the procedure-level adjustments.  Discontinue processing.
					break;
				}
				if(seg.SegmentID!="CAS") {
					continue;//Not an adjustment segment.
				}
				listClaimCasSegments.Add(seg);
			}
			return ConvertCasAdjustmentsToHumanReadable(listClaimCasSegments);
		}

		public List<string> GetProcRemarks(int segSvcIndex) {
			List<string> listRemarks=new List<string>();
			int startSegNum=_listSegNumsSVC[segSvcIndex];
			int endSegNum=_listSegments.Count;
			if(segSvcIndex<_listSegNumsSVC.Count-1) {
				endSegNum=_listSegNumsSVC[segSvcIndex+1];
			}
			for(int j=startSegNum;j<endSegNum;j++) {
				X12Segment seg=_listSegments[j];
				if(seg.SegmentID=="CLP") {
					//If another claim segment is encountered, then we have finished processing all of the procedure-level remarks.  Discontinue processing.
					break;
				}
				if(seg.SegmentID!="LQ") {
					continue;//Not a remark segment.
				}
				string code=seg.Get(2);
				if(seg.Get(1)=="HE") {//Claim Payment Remark Codes
					listRemarks.Add(GetDescriptFrom411(code));
				}
				else if(seg.Get(1)=="RX") {//National Council for Prescription Drug Programs Reject/Payment Codes.
					//We do not send prescriptions with X12, so we should never get responses with these codes.
					listRemarks.Add("Rx Rejection Reason Code: "+code);//just in case, output a generic message so the user can look it up.
				}
				else {//Should not be possible, but here for future versions of X12 just in case.
					listRemarks.Add("Code List Qualifier: "+seg.Get(1)+" Code: "+code);
				}
			}
			return listRemarks;
		}

		///<summaryReturns a list of strings with values in twos, such that the first item is the amount description, the second is the amount.</summary>
		public List<string> GetProcSupplementalInfo(int segSvcIndex) {
			List<string> listSupplementalInfo=new List<string>();
			int startSegNum=_listSegNumsSVC[segSvcIndex];
			int endSegNum=_listSegments.Count;
			if(segSvcIndex<_listSegNumsSVC.Count-1) {
				endSegNum=_listSegNumsSVC[segSvcIndex+1];
			}
			for(int j=startSegNum;j<endSegNum;j++) {
				X12Segment seg=_listSegments[j];
				if(seg.SegmentID=="CLP") {
					//If another claim segment is encountered, then we have finished processing all of the procedure-level supplemental info.  Discontinue processing.
					break;
				}
				if(seg.SegmentID!="AMT") {
					continue;//Not a supplemental info segment.
				}
				string code=seg.Get(1);
				decimal amount=PIn.Decimal(seg.Get(2));
				listSupplementalInfo.Add(GetDescriptForAmountQualifierCode(code)); listSupplementalInfo.Add(amount.ToString("f2"));
			}
			return listSupplementalInfo;
		}

		#endregion Procedure Level
		#region Helpers
		///<summary>Converts a list of CAS segments into human readable data.  Helper function for both claim level and procedure level.
		///Returns a list of strings with values in fours, such that the first item is the adjustment description, the second is the reason description, and the third is the monetary amount, the fourth is the adjustment code (CO,PI,PR, or OA).</summary>
		private List<string> ConvertCasAdjustmentsToHumanReadable(List<X12Segment> listCasSegments) {
			List<string> listAdjustments=new List<string>();
			for(int i=0;i<listCasSegments.Count;i++) {
				X12Segment seg=listCasSegments[i];
				string adjCode=seg.Get(1);
				string adjDescript="";
				if(adjCode=="CO") {
					adjDescript="Contractual Obligations";
				}
				else if(adjCode=="PI") {
					adjDescript="Payor Initiated Reductions";
				}
				else if(adjCode=="PR") {//Patient Responsibility
					adjDescript="Patient Portion";
				}
				else { //adjCode=="OA"
					adjDescript="Other Adjustments";
				}
				//Each CAS segment can contain up to 6 adjustments of the same type.
				for(int k=2;k<=17;k+=3) {
					string strAdjReasonCode=seg.Get(k);
					string strAmt=seg.Get(k+1);
					if(strAdjReasonCode=="" && strAmt=="") {
						continue;
					}
					decimal amt=PIn.Decimal(strAmt);
					if(amt==0) {
						continue;
					}
					string strAdjReasonDescript="";
					if(strAdjReasonCode!="") {
						strAdjReasonDescript=GetDescriptFrom139(strAdjReasonCode);
					}
					listAdjustments.Add(adjDescript); listAdjustments.Add(strAdjReasonDescript); listAdjustments.Add(amt.ToString("f2")); listAdjustments.Add(adjCode);
				}
			}
			return listAdjustments;
		}

		public string GetHumanReadable() {
			string result=
				"Claim Status Reponse From "+GetPayerName()+Environment.NewLine
				+"Effective Pay Date: "+GetDateEffective().ToShortDateString()+Environment.NewLine
				+"Amount: "+GetPaymentAmount()+Environment.NewLine
				+"Individual Claim Status List: "+Environment.NewLine
				+"Status	ClaimAmt	PaidAmt	PatientPortion	PayerControlNum";
			List<string> claimTrackingNumbers=GetClaimTrackingNumbers();
			for(int i=0;i<claimTrackingNumbers.Count;i++) {
				string[] claimInfo=GetClaimInfo(claimTrackingNumbers[i]);
				for(int j=0;j<claimInfo.Length;j++) {
					result+=claimInfo[j]+"\\t";
				}
				result+=Environment.NewLine;
			}
			return result;
		}

		#endregion Helpers
		#region Code To Description

		private string GetDescriptForClaimStatusCode(string code) {
			string claimStatusCodeDescript="";
			if(code=="1") {
				claimStatusCodeDescript="Processed as Primary";
			}
			else if(code=="2") {
				claimStatusCodeDescript="Processed as Secondary";
			}
			else if(code=="3") {
				claimStatusCodeDescript="Processed as Tertiary";
			}
			else if(code=="4") {
				claimStatusCodeDescript="Denied";
			}
			else if(code=="19") {
				claimStatusCodeDescript="Processed as Primary, Forwarded to Additional Payer(s)";
			}
			else if(code=="20") {
				claimStatusCodeDescript="Processed as Secondary, Forwarded to Additional Payer(s)";
			}
			else if(code=="21") {
				claimStatusCodeDescript="Processed as Tertiary, Forwarded to Additional Payer(s)";
			}
			else if(code=="22") {
				claimStatusCodeDescript="Reversal of Previous Payment";
			}
			else if(code=="23") {
				claimStatusCodeDescript="Not Our Claim, Forwarded to Additional Payer(s)";
			}
			else if(code=="25") {
				claimStatusCodeDescript="Predetermination Pricing Only - No Payment";
			}
			return claimStatusCodeDescript;
		}

		///<summary>Used for the reason codes in the PLB segment.</summary>
		private string GetDescriptForProvAdjCode(string code) {
			if(code=="50") {
				return "Late Charge";
			}
			if(code=="51") {
				return "Interest Penalty Charge";
			}
			if(code=="72") {
				return "Authorized Return";
			}
			if(code=="90") {
				return "Early Payment Allowance";
			}
			if(code=="AH") {
				return "Origination Fee";
			}
			if(code=="AM") {
				return "Applied to Borrower's Account";
			}
			if(code=="AP") {
				return "Acceleration of Benefits";
			}
			if(code=="B2") {
				return "Rebate";
			}
			if(code=="B3") {
				return "Recovery Allowance";
			}
			if(code=="BD") {
				return "Bad Debt Adjustment";
			}
			if(code=="BN") {
				return "Bonus";
			}
			if(code=="C5") {
				return "Temporary Allowance";
			}
			if(code=="CR") {
				return "Capitation Interest";
			}
			if(code=="CS") {
				return "Adjustment";
			}
			if(code=="CT") {
				return "Capitation Payment";
			}
			if(code=="CV") {
				return "Capital Passthru";
			}
			if(code=="CW") {
				return "Certified Registered Nurse Anesthetist Passthru";
			}
			if(code=="DM") {
				return "Direct Medical Education Passthru";
			}
			if(code=="E3") {
				return "Withholding";
			}
			if(code=="FB") {
				return "Forwarding Balance";
			}
			if(code=="FC") {
				return "Fund Allocation";
			}
			if(code=="GO") {
				return "Graduate Medical Education Passthru";
			}
			if(code=="HM") {
				return "Hemophilia Clotting Factor Supplement";
			}
			if(code=="IP") {
				return "Incentive Premium Payment";
			}
			if(code=="IR") {
				return "Internal Revenue Service Withholding";
			}
			if(code=="IS") {
				return "Interim Settlement";
			}
			if(code=="J1") {
				return "Nonreimbursable";
			}
			if(code=="L3") {
				return "Penalty";
			}
			if(code=="L6") {
				return "Interest Owed";
			}
			if(code=="LE") {
				return "Levy";
			}
			if(code=="LS") {
				return "Lump Sum";
			}
			if(code=="OA") {
				return "Organ Acquisition Passthru";
			}
			if(code=="OB") {
				return "Offset for Affiliated Providers";
			}
			if(code=="PI") {
				return "Periodic Interim Payment";
			}
			if(code=="PL") {
				return "Payment Final";
			}
			if(code=="RA") {
				return "Retro-activity Adjustment";
			}
			if(code=="RE") {
				return "Return on Equity";
			}
			if(code=="SL") {
				return "Student Loan Repayment";
			}
			if(code=="TL") {
				return "Third Party Liability";
			}
			if(code=="WO") {
				return "Overpayment Recovery";
			}
			if(code=="WU") {
				return "Unspecified Recovery";
			}
			return "Reason "+code;
		}

		private string GetDescriptForAmountQualifierCode(string code) {
			if(code=="B6") {
				return "Allowed - Actual";
			}
			else if(code=="KH") {
				return "Late Filing Reduction";
			}
			else if(code=="T") {
				return "Tax";
			}
			else if(code=="T2") {
				return "Total Claim Before Taxes";
			}
			else if(code=="ZK") {
				return "Federal Medicare or Medicaid Payment Mandate - Category 1";
			}
			else if(code=="ZL") {
				return "Federal Medicare or Medicaid Payment Mandate - Category 2";
			}
			else if(code=="ZM") {
				return "Federal Medicare or Medicaid Payment Mandate - Category 3";
			}
			else if(code=="ZN") {
				return "Federal Medicare or Medicaid Payment Mandate - Category 4";
			}
			else if(code=="ZO") {
				return "Federal Medicare or Medicaid Payment Mandate - Category 5";
			}
			return "Qualifier Code: "+code;
		}

		///<summary>Code Source 139. Claim Adjustment Reason Codes.  http://www.wpc-edi.com/reference/codelists/healthcare/claim-adjustment-reason-codes/ .
		///Used for claim and procedure reason codes.</summary>
		private string GetDescriptFrom139(string code) {
			if(code=="1") {
				return "Deductible Amount";
			}
			if(code=="2") {
				return "Coinsurance Amount";
			}
			if(code=="3") {
				return "Co-payment Amount";
			}
			if(code=="4") {
				return "The procedure code is inconsistent with the modifier used or a required modifier is missing.";
			}
			if(code=="5") {
				return "The procedure code/bill type is inconsistent with the place of service.";
			}
			if(code=="6") {
				return "	The procedure/revenue code is inconsistent with the patient's age.";
			}
			if(code=="7") {
				return "The procedure/revenue code is inconsistent with the patient's gender.";
			}
			if(code=="8") {
				return "The procedure code is inconsistent with the provider type/specialty (taxonomy).";
			}
			if(code=="9") {
				return "The diagnosis is inconsistent with the patient's age.";
			}
			if(code=="10") {
				return "The diagnosis is inconsistent with the patient's gender.";
			}
			if(code=="11") {
				return "The diagnosis is inconsistent with the procedure.";
			}
			if(code=="12") {
				return "The diagnosis is inconsistent with the provider type.";
			}
			if(code=="13") {
				return "The date of death precedes the date of service.";
			}
			if(code=="14") {
				return "The date of birth follows the date of service.";
			}
			if(code=="15") {
				return "The authorization number is missing, invalid, or does not apply to the billed services or provider.";
			}
			if(code=="16") {
				return "Claim/service lacks information which is needed for adjudication.";
			}
			if(code=="18") {
				return "Exact duplicate claim/service";
			}
			if(code=="19") {
				return "This is a work-related injury/illness and thus the liability of the Worker's Compensation Carrier.";
			}
			if(code=="20") {
				return "This injury/illness is covered by the liability carrier.";
			}
			if(code=="21") {
				return "This injury/illness is the liability of the no-fault carrier.";
			}
			if(code=="22") {
				return "This care may be covered by another payer per coordination of benefits.";
			}
			if(code=="23") {
				return "The impact of prior payer(s) adjudication including payments and/or adjustments.";
			}
			if(code=="24") {
				return "Charges are covered under a capitation agreement/managed care plan.";
			}
			if(code=="26") {
				return "Expenses incurred prior to coverage.";
			}
			if(code=="27") {
				return "Expenses incurred after coverage terminated.";
			}
			if(code=="29") {
				return "The time limit for filing has expired.";
			}	
			if(code=="Patient cannot be identified as our insured.") {
				return "";
			}	
			if(code=="32") {
				return "Our records indicate that this dependent is not an eligible dependent as defined.";
			}	
			if(code=="33") {
				return "Insured has no dependent coverage.";
			}
			if(code=="34") {
				return "Insured has no coverage for newborns.";
			}
			if(code=="35") {
				return "Lifetime benefit maximum has been reached.";
			}
			if(code=="39") {
				return "Services denied at the time authorization/pre-certification was requested.";
			}
			if(code=="40") {
				return "Charges do not meet qualifications for emergent/urgent care. Note: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present.";
			}
			if(code=="44") {
				return "Prompt-pay discount.";
			}
			if(code=="45") {
				return "Charge exceeds fee schedule/maximum allowable or contracted/legislated fee arrangement.";
			}
			if(code=="49") {
				return "These are non-covered services because this is a routine exam or screening procedure done in conjunction with a routine exam.";
			}
			if(code=="50") {
				return "These are non-covered services because this is not deemed a 'medical necessity' by the payer.";
			}
			if(code=="51") {
				return "These are non-covered services because this is a pre-existing condition.";
			}
			if(code=="53") {
				return "Services by an immediate relative or a member of the same household are not covered.";
			}
			if(code=="54") {
				return "Multiple physicians/assistants are not covered in this case.";
			}
			if(code=="55") {
				return "Procedure/treatment is deemed experimental/investigational by the payer.";
			}
			if(code=="56") {
				return "Procedure/treatment has not been deemed 'proven to be effective' by the payer.";
			}
			if(code=="58") {
				return "Treatment was deemed by the payer to have been rendered in an inappropriate or invalid place of service.";
			}
			if(code=="59") {
				return "Processed based on multiple or concurrent procedure rules.";
			}
			if(code=="60") {
				return "Charges for outpatient services are not covered when performed within a period of time prior to or after inpatient services.";
			}
			if(code=="61") {
				return "Penalty for failure to obtain second surgical opinion.";
			}
			if(code=="66") {
				return "Blood Deductible.";
			}
			if(code=="69") {
				return "Day outlier amount.";
			}
			if(code=="70") {
				return "Cost outlier - Adjustment to compensate for additional costs.";
			}
			if(code=="74") {
				return "Indirect Medical Education Adjustment.";
			}
			if(code=="75") {
				return "Direct Medical Education Adjustment.";
			}
			if(code=="76") {
				return "Disproportionate Share Adjustment.";
			}
			if(code=="78") {
				return "Non-Covered days/Room charge adjustment.";
			}
			if(code=="85") {//Use only Group Code PR
				return "Patient Interest Adjustment";
			}
			if(code=="89") {
				return "Professional fees removed from charges.";
			}
			if(code=="90") {
				return "Ingredient cost adjustment.";
			}
			if(code=="91") {
				return "Dispensing fee adjustment.";
			}
			if(code=="94") {
				return "Processed in Excess of charges.";
			}
			if(code=="95") {
				return "Plan procedures not followed.";
			}
			if(code=="96") {
				return "Non-covered charge(s).";
			}
			if(code=="97") {
				return "The benefit for this service is included in the payment/allowance for another service/procedure that has already been adjudicated.";
			}
			if(code=="100") {
				return "Payment made to patient/insured/responsible party/employer.";
			}
			if(code=="101") {
				return "Predetermination: anticipated payment upon completion of services or claim adjudication.";
			}
			if(code=="102") {
				return "Major Medical Adjustment.";
			}
			if(code=="103") {
				return "Provider promotional discount";
			}
			if(code=="104") {
				return "Managed care withholding.";
			}
			if(code=="105") {
				return "Tax withholding.";
			}
			if(code=="106") {
				return "Patient payment option/election not in effect.";
			}
			if(code=="107") {
				return "The related or qualifying claim/service was not identified on this claim.";
			}
			if(code=="108") {
				return "Rent/purchase guidelines were not met.";
			}
			if(code=="109") {
				return "Claim/service not covered by this payer/contractor.";
			}
			if(code=="110") {
				return "Billing date predates service date.";
			}
			if(code=="111") {
				return "Not covered unless the provider accepts assignment.";
			}
			if(code=="112") {
				return "Service not furnished directly to the patient and/or not documented.";
			}
			if(code=="114") {
				return "Procedure/product not approved by the Food and Drug Administration.";
			}
			if(code=="115") {
				return "Procedure postponed, canceled, or delayed.";
			}
			if(code=="116") {
				return "The advance indemnification notice signed by the patient did not comply with requirements.";
			}
			if(code=="117") {
				return "Transportation is only covered to the closest facility that can provide the necessary care.";
			}
			if(code=="118") {
				return "ESRD network support adjustment.";
			}
			if(code=="119") {
				return "Benefit maximum for this time period or occurrence has been reached.";
			}
			if(code=="121") {
				return "Indemnification adjustment - compensation for outstanding member responsibility.";
			}
			if(code=="122") {
				return "Psychiatric reduction.";
			}
			if(code=="125") {
				return "Submission/billing error(s).";
			}
			if(code=="128") {
				return "Newborn's services are covered in the mother's Allowance.";
			}
			if(code=="129") {
				return "Prior processing information appears incorrect.";
			}
			if(code=="130") {
				return "Claim submission fee.";
			}
			if(code=="131") {
				return "Claim specific negotiated discount.";
			}
			if(code=="132") {
				return "Prearranged demonstration project adjustment.";
			}
			if(code=="133") { //Use only with Group Code OA
				return "The disposition of the claim/service is pending further review.";
			}
			if(code=="134") {
				return "Technical fees removed from charges.";
			}
			if(code=="135") {
				return "Interim bills cannot be processed.";
			}
			if(code=="136") { //Use Group Code OA
				return "Failure to follow prior payer's coverage rules.";
			}
			if(code=="137") {
				return "Regulatory Surcharges, Assessments, Allowances or Health Related Taxes.";
			}
			if(code=="138") {
				return "Appeal procedures not followed or time limits not met.";
			}
			if(code=="139") {
				return "Contracted funding agreement - Subscriber is employed by the provider of services.";
			}
			if(code=="140") {
				return "Patient/Insured health identification number and name do not match.";
			}
			if(code=="142") {
				return "Monthly Medicaid patient liability amount.";
			}
			if(code=="143") {
				return "Portion of payment deferred.";
			}
			if(code=="144") {
				return "Incentive adjustment, e.g. preferred product/service.";
			}
			if(code=="146") {
				return "Diagnosis was invalid for the date(s) of service reported.";
			}
			if(code=="147") {
				return "Provider contracted/negotiated rate expired or not on file.";
			}
			if(code=="148") {
				return "Information from another provider was not provided or was insufficient/incomplete.";
			}
			if(code=="149") {
				return "Lifetime benefit maximum has been reached for this service/benefit category.";
			}
			if(code=="150") {
				return "Payer deems the information submitted does not support this level of service.";
			}
			if(code=="151") {
				return "Payment adjusted because the payer deems the information submitted does not support this many/frequency of services.";
			}
			if(code=="152") {
				return "Payer deems the information submitted does not support this length of service.";
			}
			if(code=="153") {
				return "Payer deems the information submitted does not support this dosage.";
			}
			if(code=="154") {
				return "Payer deems the information submitted does not support this day's supply.";
			}
			if(code=="155") {
				return "Patient refused the service/procedure.";
			}
			if(code=="157") {
				return "Service/procedure was provided as a result of an act of war.";
			}
			if(code=="158") {
				return "Service/procedure was provided outside of the United States.";
			}
			if(code=="159") {
				return "Service/procedure was provided as a result of terrorism.";
			}
			if(code=="160") {
				return "Injury/illness was the result of an activity that is a benefit exclusion.";
			}
			if(code=="161") {
				return "Provider performance bonus";
			}
			if(code=="162") {
				return "State-mandated Requirement for Property and Casualty, see Claim Payment Remarks Code for specific explanation.";
			}
			if(code=="163") {
				return "Attachment referenced on the claim was not received.";
			}
			if(code=="164") {
				return "Attachment referenced on the claim was not received in a timely fashion.";
			}
			if(code=="165") {
				return "Referral absent or exceeded.";
			}
			if(code=="166") {
				return "These services were submitted after this payers responsibility for processing claims under this plan ended.";
			}
			if(code=="167") {
				return "This (these) diagnosis(es) is (are) not covered.";
			}
			if(code=="168") {
				return "Service(s) have been considered under the patient's medical plan. Benefits are not available under this dental plan.";
			}
			if(code=="169") {
				return "Alternate benefit has been provided.";
			}
			if(code=="170") {
				return "Payment is denied when performed/billed by this type of provider.";
			}
			if(code=="171") {
				return "Payment is denied when performed/billed by this type of provider in this type of facility.";
			}
			if(code=="172") {
				return "Payment is adjusted when performed/billed by a provider of this specialty.";
			}
			if(code=="173") {
				return "Service was not prescribed by a physician.";
			}
			if(code=="174") {
				return "Service was not prescribed prior to delivery.";
			}
			if(code=="175") {
				return "Prescription is incomplete.";
			}
			if(code=="176") {
				return "Prescription is not current.";
			}
			if(code=="177") {
				return "Patient has not met the required eligibility requirements.";
			}
			if(code=="178") {
				return "Patient has not met the required spend down requirements.";
			}
			if(code=="179") {
				return "Patient has not met the required waiting requirements.";
			}
			if(code=="180") {
				return "Patient has not met the required residency requirements.";
			}
			if(code=="181") {
				return "Procedure code was invalid on the date of service.";
			}
			if(code=="182") {
				return "Procedure modifier was invalid on the date of service.";
			}
			if(code=="183") {
				return "The referring provider is not eligible to refer the service billed.";
			}
			if(code=="184") {
				return "The prescribing/ordering provider is not eligible to prescribe/order the service billed.";
			}
			if(code=="185") {
				return "The rendering provider is not eligible to perform the service billed.";
			}
			if(code=="186") {
				return "Level of care change adjustment.";
			}
			if(code=="187") {
				return "Consumer Spending Account payments.";
			}
			if(code=="188") {
				return "This product/procedure is only covered when used according to FDA recommendations.";
			}
			if(code=="189") {
				return "'Not otherwise classified' or 'unlisted' procedure code (CPT/HCPCS) was billed when there is a specific procedure code for this procedure/service.";
			}
			if(code=="190") {
				return "Payment is included in the allowance for a Skilled Nursing Facility (SNF) qualified stay.";
			}
			if(code=="191") {
				return "Not a work related injury/illness and thus not the liability of the workers' compensation carrier";
			}
			if(code=="192") {
				return "Non standard adjustment code from paper remittance.";
			}
			if(code=="193") {
				return "Original payment decision is being maintained. Upon review, it was determined that this claim was processed properly.";
			}
			if(code=="194") {
				return "Anesthesia performed by the operating physician, the assistant surgeon or the attending physician.";
			}
			if(code=="195") {
				return "Refund issued to an erroneous priority payer for this claim/service.";
			}
			if(code=="197") {
				return "Precertification/authorization/notification absent.";
			}
			if(code=="198") {
				return "Precertification/authorization exceeded.";
			}
			if(code=="199") {
				return "Revenue code and Procedure code do not match.";
			}
			if(code=="200") {
				return "Expenses incurred during lapse in coverage";
			}
			if(code=="201") { //Use group code PR
				return "Workers' Compensation case settled. Patient is responsible for amount of this claim/service through WC 'Medicare set aside arrangement' or other agreement.";
			}
			if(code=="202") {
				return "Non-covered personal comfort or convenience services.";
			}
			if(code=="203") {
				return "Discontinued or reduced service.";
			}
			if(code=="204") {
				return "This service/equipment/drug is not covered under the patient’s current benefit plan.";
			}
			if(code=="205") {
				return "Pharmacy discount card processing fee";
			}
			if(code=="206") {
				return "National Provider Identifier - missing.";
			}
			if(code=="207") {
				return "National Provider identifier - Invalid format";
			}
			if(code=="208") {
				return "National Provider Identifier - Not matched.";
			}
			if(code=="209") { //Use Group code OA
				return "Per regulatory or other agreement. The provider cannot collect this amount from the patient. However, this amount may be billed to subsequent payer. Refund to patient if collected.";
			}
			if(code=="210") {
				return "Payment adjusted because pre-certification/authorization not received in a timely fashion";
			}
			if(code=="211") {
				return "National Drug Codes (NDC) not eligible for rebate, are not covered.";
			}
			if(code=="212") {
				return "Administrative surcharges are not covered";
			}
			if(code=="213") {
				return "Non-compliance with the physician self referral prohibition legislation or payer policy.";
			}
			if(code=="214") {
				return "Workers' Compensation claim adjudicated as non-compensable. This Payer not liable for claim or service/treatment.";
			}
			if(code=="215") {
				return "Based on subrogation of a third party settlement";
			}
			if(code=="216") {
				return "Based on the findings of a review organization";
			}
			if(code=="217") {
				return "Based on payer reasonable and customary fees. No maximum allowable defined by legislated fee arrangement.";
			}
			if(code=="218") {
				return "Based on entitlement to benefits.";
			}
			if(code=="219") {
				return "Based on extent of injury.";
			}
			if(code=="220") {
				return "The applicable fee schedule/fee database does not contain the billed code. Please resubmit a bill with the appropriate fee schedule/fee database code(s) that best describe the service(s) provided and supporting documentation if required.";
			}
			if(code=="221") {
				return "Workers' Compensation claim is under investigation.";
			}
			if(code=="222") {
				return "Exceeds the contracted maximum number of hours/days/units by this provider for this period. This is not patient specific.";
			}
			if(code=="223") {
				return "Adjustment code for mandated federal, state or local law/regulation that is not already covered by another code and is mandated before a new code can be created.";
			}
			if(code=="224") {
				return "Patient identification compromised by identity theft. Identity verification required for processing this and future claims.";
			}
			if(code=="225") {
				return "Penalty or Interest Payment by Payer";
			}
			if(code=="226") {
				return "Information requested from the Billing/Rendering Provider was not provided or was insufficient/incomplete.";
			}
			if(code=="227") {
				return "Information requested from the patient/insured/responsible party was not provided or was insufficient/incomplete.";
			}
			if(code=="228") {
				return "Denied for failure of this provider, another provider or the subscriber to supply requested information to a previous payer for their adjudication.";
			}
			if(code=="229") { //Use only with Group Code PR
				return "Partial charge amount not considered by Medicare due to the initial claim Type of Bill being 12X.";
			}
			if(code=="230") {
				return "No available or correlating CPT/HCPCS code to describe this service.";
			}
			if(code=="231") {
				return "Mutually exclusive procedures cannot be done in the same day/setting.";
			}
			if(code=="232") {
				return "Institutional Transfer Amount.";
			}
			if(code=="233") {
				return "Services/charges related to the treatment of a hospital-acquired condition or preventable medical error.";
			}
			if(code=="234") {
				return "This procedure is not paid separately.";
			}
			if(code=="235") {
				return "Sales Tax";
			}
			if(code=="236") {
				return "This procedure or procedure/modifier combination is not compatible with another procedure or procedure/modifier combination provided on the same day according to the National Correct Coding Initiative.";
			}
			if(code=="237") {
				return "Legislated/Regulatory Penalty.";
			}
			if(code=="238") { //Use Group Code PR
				return "Claim spans eligible and ineligible periods of coverage, this is the reduction for the ineligible period.";
			}
			if(code=="239") {
				return "Claim spans eligible and ineligible periods of coverage. Rebill separate claims.";
			}
			if(code=="240") {
				return "The diagnosis is inconsistent with the patient's birth weight.";
			}
			if(code=="241") {
				return "Low Income Subsidy (LIS) Co-payment Amount";
			}
			if(code=="242") {
				return "Services not provided by network/primary care providers.";
			}
			if(code=="243") {
				return "Services not authorized by network/primary care providers.";
			}
			if(code=="244") {
				return "Payment reduced to zero due to litigation. Additional information will be sent following the conclusion of litigation.";
			}
			if(code=="245") {
				return "Provider performance program withhold.";
			}
			if(code=="246") {
				return "This non-payable code is for required reporting only.";
			}
			if(code=="247") {
				return "Deductible for Professional service rendered in an Institutional setting and billed on an Institutional claim.";
			}
			if(code=="248") {
				return "Coinsurance for Professional service rendered in an Institutional setting and billed on an Institutional claim.";
			}
			if(code=="249") { //Use only with Group Code CO
				return "This claim has been identified as a readmission.";
			}
			if(code=="250") {
				return "The attachment content received is inconsistent with the expected content.";
			}
			if(code=="251") {
				return "The attachment content received did not contain the content required to process this claim or service.";
			}
			if(code=="252") {
				return "An attachment is required to adjudicate this claim/service.";
			}
			if(code=="A0") {
				return "Patient refund amount.";
			}
			if(code=="A1") {
				return "Claim/Service denied.";
			}
			if(code=="A5") {
				return "Medicare Claim PPS Capital Cost Outlier Amount.";
			}
			if(code=="A6") {
				return "Prior hospitalization or 30 day transfer requirement not met.";
			}
			if(code=="A7") {
				return "Presumptive Payment Adjustment";
			}
			if(code=="A8") {
				return "Ungroupable DRG.";
			}
			if(code=="B1") {
				return "Non-covered visits.";
			}
			if(code=="B4") {
				return "Late filing penalty.";
			}
			if(code=="B5") {
				return "Coverage/program guidelines were not met or were exceeded.";
			}
			if(code=="B7") {
				return "This provider was not certified/eligible to be paid for this procedure/service on this date of service.";
			}
			if(code=="B8") {
				return "Alternative services were available, and should have been utilized.";
			}
			if(code=="B9") {
				return "Patient is enrolled in a Hospice.";
			}
			if(code=="B10") {
				return "Allowed amount has been reduced because a component of the basic procedure/test was paid. The beneficiary is not liable for more than the charge limit for the basic procedure/test.";
			}
			if(code=="B11") {
				return "The claim/service has been transferred to the proper payer/processor for processing. Claim/service not covered by this payer/processor.";
			}
			if(code=="B12") {
				return "Services not documented in patients' medical records.";
			}
			if(code=="B13") {
				return "Previously paid. Payment for this claim/service may have been provided in a previous payment.";
			}
			if(code=="B14") {
				return "Only one visit or consultation per physician per day is covered.";
			}
			if(code=="B15") {
				return "This service/procedure requires that a qualifying service/procedure be received and covered. The qualifying other service/procedure has not been received/adjudicated.";
			}
			if(code=="B16") {
				return "'New Patient' qualifications were not met.";
			}
			if(code=="B20") {
				return "Procedure/service was partially or fully furnished by another provider.";
			}
			if(code=="B22") {
				return "This payment is adjusted based on the diagnosis.";
			}
			if(code=="B23") {
				return "Procedure billed is not authorized per your Clinical Laboratory Improvement Amendment (CLIA) proficiency test.";
			}
			if(code=="W1") {
				return "Workers' compensation jurisdictional fee schedule adjustment.";
			}
			if(code=="W2") {
				return "Payment reduced or denied based on workers' compensation jurisdictional regulations or payment policies, use only if no other code is applicable.";
			}
			if(code=="W3") {
				return "The Benefit for this Service is included in the payment/allowance for another service/procedure that has been performed on the same day.";
			}
			if(code=="W4") {
				return "Workers' Compensation Medical Treatment Guideline Adjustment.";
			}
			if(code=="Y1") {
				return "Payment denied based on Medical Payments Coverage (MPC) or Personal Injury Protection (PIP) Benefits jurisdictional regulations or payment policies, use only if no other code is applicable.";
			}
			if(code=="Y2") {
				return "Payment adjusted based on Medical Payments Coverage (MPC) or Personal Injury Protection (PIP) Benefits jurisdictional regulations or payment policies, use only if no other code is applicable.";
			}
			if(code=="Y3") {
				return "Medical Payments Coverage (MPC) or Personal Injury Protection (PIP) Benefits jurisdictional fee schedule adjustment.";
			}
			return "Reason code "+code;//Worst case, if we do not recognize the code, display it verbatim so the user can look it up.
		}

		///<summary>Code Source 411.  Remittance Advice Remark Codes.  https://www.wpc-edi.com/reference/codelists/healthcare/remittance-advice-remark-codes/ </summary>
		private string GetDescriptFrom411(string code) {
			if(code=="M1") { return "X-ray not taken within the past 12 months or near enough to the start of treatment."; }
			else if(code=="M2") { return "Not paid separately when the patient is an inpatient."; }
			else if(code=="M3") { return "Equipment is the same or similar to equipment already being used."; }
			else if(code=="M4") { return "Alert: This is the last monthly installment payment for this durable medical equipment."; }
			else if(code=="M5") { return "Monthly rental payments can continue until the earlier of the 15th month from the first rental month, or the month when the equipment is no longer needed."; }
			else if(code=="M6") { return "Alert: You must furnish and service this item for any period of medical need for the remainder of the reasonable useful lifetime of the equipment."; }
			else if(code=="M7") { return "No rental payments after the item is purchased, or after the total of issued rental payments equals the purchase price."; }
			else if(code=="M8") { return "We do not accept blood gas tests results when the test was conducted by a medical supplier or taken while the patient is on oxygen."; }
			else if(code=="M9") { return "Alert: This is the tenth rental month. You must offer the patient the choice of changing the rental to a purchase agreement."; }
			else if(code=="M10") { return "Equipment purchases are limited to the first or the tenth month of medical necessity."; }
			else if(code=="M11") { return "DME, orthotics and prosthetics must be billed to the DME carrier who services the patient's zip code."; }
			else if(code=="M12") { return "Diagnostic tests performed by a physician must indicate whether purchased services are included on the claim."; }
			else if(code=="M13") { return "Only one initial visit is covered per specialty per medical group."; }
			else if(code=="M14") { return "No separate payment for an injection administered during an office visit, and no payment for a full office visit if the patient only received an injection."; }
			else if(code=="M15") { return "Separately billed services/tests have been bundled as they are considered components of the same procedure. Separate payment is not allowed."; }
			else if(code=="M16") { return "Alert: Please see our web site, mailings, or bulletins for more details concerning this policy/procedure/decision."; }
			else if(code=="M17") { return "Alert: Payment approved as you did not know, and could not reasonably have been expected to know, that this would not normally have been covered for this patient. In the future, you will be liable for charges for the same service(s) under the same or similar conditions."; }
			else if(code=="M18") { return "Certain services may be approved for home use. Neither a hospital nor a Skilled Nursing Facility (SNF) is considered to be a patient's home."; }
			else if(code=="M19") { return "Missing oxygen certification/re-certification."; }
			else if(code=="M20") { return "Missing/incomplete/invalid HCPCS."; }
			else if(code=="M21") { return "Missing/incomplete/invalid place of residence for this service/item provided in a home."; }
			else if(code=="M22") { return "Missing/incomplete/invalid number of miles traveled."; }
			else if(code=="M23") { return "Missing invoice."; }
			else if(code=="M24") { return "Missing/incomplete/invalid number of doses per vial."; }
			else if(code=="M25") { return "The information furnished does not substantiate the need for this level of service. If you believe the service should have been fully covered as billed, or if you did not know and could not reasonably have been expected to know that we would not pay for this level of service, or if you notified the patient in writing in advance that we would not pay for this level of service and he/she agreed in writing to pay, ask us to review your claim within 120 days of the date of this notice. If you do not request an appeal, we will, upon application from the patient, reimburse him/her for the amount you have collected from him/her in excess of any deductible and coinsurance amounts. We will recover the reimbursement from you as an overpayment."; }
			else if(code=="M26") { return "The information furnished does not substantiate the need for this level of service. If you have collected any amount from the patient for this level of service /any amount that exceeds the limiting charge for the less extensive service, the law requires you to refund that amount to the patient within 30 days of receiving this notice. The requirements for refund are in 1824(I) of the Social Security Act and 42CFR411.408. The section specifies that physicians who knowingly and willfully fail to make appropriate refunds may be subject to civil monetary penalties and/or exclusion from the program. If you have any questions about this notice, please contact this office."; }
			else if(code=="M27") { return "Alert: The patient has been relieved of liability of payment of these items and services under the limitation of liability provision of the law. The provider is ultimately liable for the patient's waived charges, including any charges for coinsurance, since the items or services were not reasonable and necessary or constituted custodial care, and you knew or could reasonably have been expected to know, that they were not covered. You may appeal this determination. You may ask for an appeal regarding both the coverage determination and the issue of whether you exercised due care. The appeal request must be filed within 120 days of the date you receive this notice. You must make the request through this office."; }
			else if(code=="M28") { return "This does not qualify for payment under Part B when Part A coverage is exhausted or not otherwise available."; }
			else if(code=="M29") { return "Missing operative note/report."; }
			else if(code=="M30") { return "Missing pathology report."; }
			else if(code=="M31") { return "Missing radiology report."; }
			else if(code=="M32") { return "Alert: This is a conditional payment made pending a decision on this service by the patient's primary payer. This payment may be subject to refund upon your receipt of any additional payment for this service from another payer. You must contact this office immediately upon receipt of an additional payment for this service."; }
			else if(code=="M33") { return "Missing/incomplete/invalid UPIN for the ordering/referring/performing provider."; }
			else if(code=="M34") { return "Claim lacks the CLIA certification number."; }
			else if(code=="M35") { return "Missing/incomplete/invalid pre-operative photos or visual field results."; }
			else if(code=="M36") { return "This is the 11th rental month. We cannot pay for this until you indicate that the patient has been given the option of changing the rental to a purchase."; }
			else if(code=="M37") { return "Not covered when the patient is under age 35."; }
			else if(code=="M38") { return "The patient is liable for the charges for this service as you informed the patient in writing before the service was furnished that we would not pay for it, and the patient agreed to pay."; }
			else if(code=="M39") { return "The patient is not liable for payment for this service as the advance notice of non-coverage you provided the patient did not comply with program requirements."; }
			else if(code=="M40") { return "Claim must be assigned and must be filed by the practitioner's employer."; }
			else if(code=="M41") { return "We do not pay for this as the patient has no legal obligation to pay for this."; }
			else if(code=="M42") { return "The medical necessity form must be personally signed by the attending physician."; }
			else if(code=="M43") { return "Payment for this service previously issued to you or another provider by another carrier/intermediary."; }
			else if(code=="M44") { return "Missing/incomplete/invalid condition code."; }
			else if(code=="M45") { return "Missing/incomplete/invalid occurrence code(s)."; }
			else if(code=="M46") { return "Missing/incomplete/invalid occurrence span code(s)."; }
			else if(code=="M47") { return "Missing/incomplete/invalid internal or document control number."; }
			else if(code=="M48") { return "Payment for services furnished to hospital inpatients (other than professional services of physicians) can only be made to the hospital. You must request payment from the hospital rather than the patient for this service."; }
			else if(code=="M49") { return "Missing/incomplete/invalid value code(s) or amount(s)."; }
			else if(code=="M50") { return "Missing/incomplete/invalid revenue code(s)."; }
			else if(code=="M51") { return "Missing/incomplete/invalid procedure code(s)."; }
			else if(code=="M52") { return "Missing/incomplete/invalid “from” date(s) of service."; }
			else if(code=="M53") { return "Missing/incomplete/invalid days or units of service."; }
			else if(code=="M54") { return "Missing/incomplete/invalid total charges."; }
			else if(code=="M55") { return "We do not pay for self-administered anti-emetic drugs that are not administered with a covered oral anti-cancer drug."; }
			else if(code=="M56") { return "Missing/incomplete/invalid payer identifier."; }
			else if(code=="M57") { return "Missing/incomplete/invalid provider identifier."; }
			else if(code=="M58") { return "Missing/incomplete/invalid claim information. Resubmit claim after corrections."; }
			else if(code=="M59") { return "Missing/incomplete/invalid “to” date(s) of service."; }
			else if(code=="M60") { return "Missing Certificate of Medical Necessity."; }
			else if(code=="M61") { return "We cannot pay for this as the approval period for the FDA clinical trial has expired."; }
			else if(code=="M62") { return "Missing/incomplete/invalid treatment authorization code."; }
			else if(code=="M63") { return "We do not pay for more than one of these on the same day."; }
			else if(code=="M64") { return "Missing/incomplete/invalid other diagnosis."; }
			else if(code=="M65") { return "One interpreting physician charge can be submitted per claim when a purchased diagnostic test is indicated. Please submit a separate claim for each interpreting physician."; }
			else if(code=="M66") { return "Our records indicate that you billed diagnostic tests subject to price limitations and the procedure code submitted includes a professional component. Only the technical component is subject to price limitations. Please submit the technical and professional components of this service as separate line items."; }
			else if(code=="M67") { return "Missing/incomplete/invalid other procedure code(s)."; }
			else if(code=="M68") { return "Missing/incomplete/invalid attending, ordering, rendering, supervising or referring physician identification."; }
			else if(code=="M69") { return "Paid at the regular rate as you did not submit documentation to justify the modified procedure code."; }
			else if(code=="M70") { return "Alert: The NDC code submitted for this service was translated to a HCPCS code for processing, but please continue to submit the NDC on future claims for this item."; }
			else if(code=="M71") { return "Total payment reduced due to overlap of tests billed."; }
			else if(code=="M72") { return "Did not enter full 8-digit date (MM/DD/CCYY)."; }
			else if(code=="M73") { return "The HPSA/Physician Scarcity bonus can only be paid on the professional component of this service. Rebill as separate professional and technical components."; }
			else if(code=="M74") { return "This service does not qualify for a HPSA/Physician Scarcity bonus payment."; }
			else if(code=="M75") { return "Multiple automated multichannel tests performed on the same day combined for payment."; }
			else if(code=="M76") { return "Missing/incomplete/invalid diagnosis or condition."; }
			else if(code=="M77") { return "Missing/incomplete/invalid place of service."; }
			else if(code=="M78") { return "Missing/incomplete/invalid HCPCS modifier."; }
			else if(code=="M79") { return "Missing/incomplete/invalid charge."; }
			else if(code=="M80") { return "Not covered when performed during the same session/date as a previously processed service for the patient."; }
			else if(code=="M81") { return "You are required to code to the highest level of specificity."; }
			else if(code=="M82") { return "Service is not covered when patient is under age 50."; }
			else if(code=="M83") { return "Service is not covered unless the patient is classified as at high risk."; }
			else if(code=="M84") { return "Medical code sets used must be the codes in effect at the time of service"; }
			else if(code=="M85") { return "Subjected to review of physician evaluation and management services."; }
			else if(code=="M86") { return "Service denied because payment already made for same/similar procedure within set time frame."; }
			else if(code=="M87") { return "Claim/service(s) subjected to CFO-CAP prepayment review."; }
			else if(code=="M88") { return "We cannot pay for laboratory tests unless billed by the laboratory that did the work."; }
			else if(code=="M89") { return "Not covered more than once under age 40."; }
			else if(code=="M90") { return "Not covered more than once in a 12 month period."; }
			else if(code=="M91") { return "Lab procedures with different CLIA certification numbers must be billed on separate claims."; }
			else if(code=="M92") { return "Services subjected to review under the Home Health Medical Review Initiative."; }
			else if(code=="M93") { return "Information supplied supports a break in therapy. A new capped rental period began with delivery of this equipment."; }
			else if(code=="M94") { return "Information supplied does not support a break in therapy. A new capped rental period will not begin."; }
			else if(code=="M95") { return "Services subjected to Home Health Initiative medical review/cost report audit."; }
			else if(code=="M96") { return "The technical component of a service furnished to an inpatient may only be billed by that inpatient facility. You must contact the inpatient facility for technical component reimbursement. If not already billed, you should bill us for the professional component only."; }
			else if(code=="M97") { return "Not paid to practitioner when provided to patient in this place of service. Payment included in the reimbursement issued the facility."; }
			else if(code=="M98") { return "Begin to report the Universal Product Number on claims for items of this type. We will soon begin to deny payment for items of this type if billed without the correct UPN."; }
			else if(code=="M99") { return "Missing/incomplete/invalid Universal Product Number/Serial Number."; }
			else if(code=="M100") { return "We do not pay for an oral anti-emetic drug that is not administered for use immediately before, at, or within 48 hours of administration of a covered chemotherapy drug."; }
			else if(code=="M101") { return "Begin to report a G1-G5 modifier with this HCPCS. We will soon begin to deny payment for this service if billed without a G1-G5 modifier."; }
			else if(code=="M102") { return "Service not performed on equipment approved by the FDA for this purpose."; }
			else if(code=="M103") { return "Information supplied supports a break in therapy. However, the medical information we have for this patient does not support the need for this item as billed. We have approved payment for this item at a reduced level, and a new capped rental period will begin with the delivery of this equipment."; }
			else if(code=="M104") { return "Information supplied supports a break in therapy. A new capped rental period will begin with delivery of the equipment. This is the maximum approved under the fee schedule for this item or service."; }
			else if(code=="M105") { return "Information supplied does not support a break in therapy. The medical information we have for this patient does not support the need for this item as billed. We have approved payment for this item at a reduced level, and a new capped rental period will not begin."; }
			else if(code=="M106") { return "Information supplied does not support a break in therapy. A new capped rental period will not begin. This is the maximum approved under the fee schedule for this item or service."; }
			else if(code=="M107") { return "Payment reduced as 90-day rolling average hematocrit for ESRD patient exceeded 36.5%."; }
			else if(code=="M108") { return "Missing/incomplete/invalid provider identifier for the provider who interpreted the diagnostic test."; }
			else if(code=="M109") { return "We have provided you with a bundled payment for a teleconsultation. You must send 25 percent of the teleconsultation payment to the referring practitioner."; }
			else if(code=="M110") { return "Missing/incomplete/invalid provider identifier for the provider from whom you purchased interpretation services."; }
			else if(code=="M111") { return "We do not pay for chiropractic manipulative treatment when the patient refuses to have an x-ray taken."; }
			else if(code=="M112") { return "Reimbursement for this item is based on the single payment amount required under the DMEPOS Competitive Bidding Program for the area where the patient resides."; }
			else if(code=="M113") { return "Our records indicate that this patient began using this item/service prior to the current contract period for the DMEPOS Competitive Bidding Program."; }
			else if(code=="M114") { return "This service was processed in accordance with rules and guidelines under the DMEPOS Competitive Bidding Program or a Demonstration Project. For more information regarding these projects, contact your local contractor."; }
			else if(code=="M115") { return "This item is denied when provided to this patient by a non-contract or non-demonstration supplier."; }
			else if(code=="M116") { return "Processed under a demonstration project or program. Project or program is ending and additional services may not be paid under this project or program."; }
			else if(code=="M117") { return "Not covered unless submitted via electronic claim."; }
			else if(code=="M118") { return "Letter to follow containing further information."; }
			else if(code=="M119") { return "Missing/incomplete/invalid/ deactivated/withdrawn National Drug Code (NDC)."; }
			else if(code=="M120") { return "Missing/incomplete/invalid provider identifier for the substituting physician who furnished the service(s) under a reciprocal billing or locum tenens arrangement."; }
			else if(code=="M121") { return "We pay for this service only when performed with a covered cryosurgical ablation."; }
			else if(code=="M122") { return "Missing/incomplete/invalid level of subluxation."; }
			else if(code=="M123") { return "Missing/incomplete/invalid name, strength, or dosage of the drug furnished."; }
			else if(code=="M124") { return "Missing indication of whether the patient owns the equipment that requires the part or supply."; }
			else if(code=="M125") { return "Missing/incomplete/invalid information on the period of time for which the service/supply/equipment will be needed."; }
			else if(code=="M126") { return "Missing/incomplete/invalid individual lab codes included in the test."; }
			else if(code=="M127") { return "Missing patient medical record for this service."; }
			else if(code=="M128") { return "Missing/incomplete/invalid date of the patient’s last physician visit."; }
			else if(code=="M129") { return "Missing/incomplete/invalid indicator of x-ray availability for review."; }
			else if(code=="M130") { return "Missing invoice or statement certifying the actual cost of the lens, less discounts, and/or the type of intraocular lens used."; }
			else if(code=="M131") { return "Missing physician financial relationship form."; }
			else if(code=="M132") { return "Missing pacemaker registration form."; }
			else if(code=="M133") { return "Claim did not identify who performed the purchased diagnostic test or the amount you were charged for the test."; }
			else if(code=="M134") { return "Performed by a facility/supplier in which the provider has a financial interest."; }
			else if(code=="M135") { return "Missing/incomplete/invalid plan of treatment."; }
			else if(code=="M136") { return "Missing/incomplete/invalid indication that the service was supervised or evaluated by a physician."; }
			else if(code=="M137") { return "Part B coinsurance under a demonstration project or pilot program."; }
			else if(code=="M138") { return "Patient identified as a demonstration participant but the patient was not enrolled in the demonstration at the time services were rendered. Coverage is limited to demonstration participants."; }
			else if(code=="M139") { return "Denied services exceed the coverage limit for the demonstration."; }
			else if(code=="M140") { return "Service not covered until after the patient’s 50th birthday, i.e., no coverage prior to the day after the 50th birthday"; }
			else if(code=="M141") { return "Missing physician certified plan of care."; }
			else if(code=="M142") { return "Missing American Diabetes Association Certificate of Recognition."; }
			else if(code=="M143") { return "The provider must update license information with the payer."; }
			else if(code=="M144") { return "Pre-/post-operative care payment is included in the allowance for the surgery/procedure."; }
			else if(code=="MA01") { return "Alert: If you do not agree with what we approved for these services, you may appeal our decision. To make sure that we are fair to you, we require another individual that did not process your initial claim to conduct the appeal. However, in order to be eligible for an appeal, you must write to us within 120 days of the date you received this notice, unless you have a good reason for being late."; }
			else if(code=="MA02") { return "Alert: If you do not agree with this determination, you have the right to appeal. You must file a written request for an appeal within 180 days of the date you receive this notice."; }
			else if(code=="MA03") { return "If you do not agree with the approved amounts and $100 or more is in dispute (less deductible and coinsurance), you may ask for a hearing within six months of the date of this notice. To meet the $100, you may combine amounts on other claims that have been denied, including reopened appeals if you received a revised decision. You must appeal each claim on time."; }
			else if(code=="MA04") { return "Secondary payment cannot be considered without the identity of or payment information from the primary payer. The information was either not reported or was illegible."; }
			else if(code=="MA05") { return "Incorrect admission date patient status or type of bill entry on claim."; }
			else if(code=="MA06") { return "Missing/incomplete/invalid beginning and/or ending date(s)."; }
			else if(code=="MA07") { return "Alert: The claim information has also been forwarded to Medicaid for review."; }
			else if(code=="MA08") { return "Alert: Claim information was not forwarded because the supplemental coverage is not with a Medigap plan, or you do not participate in Medicare."; }
			else if(code=="MA09") { return "Claim submitted as unassigned but processed as assigned. You agreed to accept assignment for all claims."; }
			else if(code=="MA10") { return "Alert: The patient's payment was in excess of the amount owed. You must refund the overpayment to the patient."; }
			else if(code=="MA11") { return "Payment is being issued on a conditional basis. If no-fault insurance, liability insurance, Workers' Compensation, Department of Veterans Affairs, or a group health plan for employees and dependents also covers this claim, a refund may be due us. Please contact us if the patient is covered by any of these sources."; }
			else if(code=="MA12") { return "You have not established that you have the right under the law to bill for services furnished by the person(s) that furnished this (these) service(s)."; }
			else if(code=="MA13") { return "Alert: You may be subject to penalties if you bill the patient for amounts not reported with the PR (patient portion) group code."; }
			else if(code=="MA14") { return "Alert: The patient is a member of an employer-sponsored prepaid health plan. Services from outside that health plan are not covered. However, as you were not previously notified of this, we are paying this time. In the future, we will not pay you for non-plan services."; }
			else if(code=="MA15") { return "Alert: Your claim has been separated to expedite handling. You will receive a separate notice for the other services reported."; }
			else if(code=="MA16") { return "The patient is covered by the Black Lung Program. Send this claim to the Department of Labor, Federal Black Lung Program, P.O. Box 828, Lanham-Seabrook MD 20703."; }
			else if(code=="MA17") { return "We are the primary payer and have paid at the primary rate. You must contact the patient's other insurer to refund any excess it may have paid due to its erroneous primary payment."; }
			else if(code=="MA18") { return "Alert: The claim information is also being forwarded to the patient's supplemental insurer. Send any questions regarding supplemental benefits to them."; }
			else if(code=="MA19") { return "Alert: Information was not sent to the Medigap insurer due to incorrect/invalid information you submitted concerning that insurer. Please verify your information and submit your secondary claim directly to that insurer."; }
			else if(code=="MA20") { return "Skilled Nursing Facility (SNF) stay not covered when care is primarily related to the use of an urethral catheter for convenience or the control of incontinence."; }
			else if(code=="MA21") { return "SSA records indicate mismatch with name and sex."; }
			else if(code=="MA22") { return "Payment of less than $1.00 suppressed."; }
			else if(code=="MA23") { return "Demand bill approved as result of medical review."; }
			else if(code=="MA24") { return "Christian Science Sanitarium/ Skilled Nursing Facility (SNF) bill in the same benefit period."; }
			else if(code=="MA25") { return "A patient may not elect to change a hospice provider more than once in a benefit period."; }
			else if(code=="MA26") { return "Alert: Our records indicate that you were previously informed of this rule."; }
			else if(code=="MA27") { return "Missing/incomplete/invalid entitlement number or name shown on the claim."; }
			else if(code=="MA28") { return "Alert: Receipt of this notice by a physician or supplier who did not accept assignment is for information only and does not make the physician or supplier a party to the determination. No additional rights to appeal this decision, above those rights already provided for by regulation/instruction, are conferred by receipt of this notice."; }
			else if(code=="MA29") { return "Missing/incomplete/invalid provider name, city, state, or zip code."; }
			else if(code=="MA30") { return "Missing/incomplete/invalid type of bill."; }
			else if(code=="MA31") { return "Missing/incomplete/invalid beginning and ending dates of the period billed."; }
			else if(code=="MA32") { return "Missing/incomplete/invalid number of covered days during the billing period."; }
			else if(code=="MA33") { return "Missing/incomplete/invalid noncovered days during the billing period."; }
			else if(code=="MA34") { return "Missing/incomplete/invalid number of coinsurance days during the billing period."; }
			else if(code=="MA35") { return "Missing/incomplete/invalid number of lifetime reserve days."; }
			else if(code=="MA36") { return "Missing/incomplete/invalid patient name."; }
			else if(code=="MA37") { return "Missing/incomplete/invalid patient's address."; }
			else if(code=="MA38") { return "Missing/incomplete/invalid birth date."; }
			else if(code=="MA39") { return "Missing/incomplete/invalid gender."; }
			else if(code=="MA40") { return "Missing/incomplete/invalid admission date."; }
			else if(code=="MA41") { return "Missing/incomplete/invalid admission type."; }
			else if(code=="MA42") { return "Missing/incomplete/invalid admission source."; }
			else if(code=="MA43") { return "Missing/incomplete/invalid patient status."; }
			else if(code=="MA44") { return "Alert: No appeal rights. Adjudicative decision based on law."; }
			else if(code=="MA45") { return "Alert: As previously advised, a portion or all of your payment is being held in a special account."; }
			else if(code=="MA46") { return "The new information was considered but additional payment will not be issued."; }
			else if(code=="MA47") { return "Our records show you have opted out of Medicare, agreeing with the patient not to bill Medicare for services/tests/supplies furnished. As result, we cannot pay this claim. The patient is responsible for payment."; }
			else if(code=="MA48") { return "Missing/incomplete/invalid name or address of responsible party or primary payer."; }
			else if(code=="MA49") { return "Missing/incomplete/invalid six-digit provider identifier for home health agency or hospice for physician(s) performing care plan oversight services."; }
			else if(code=="MA50") { return "Missing/incomplete/invalid Investigational Device Exemption number for FDA-approved clinical trial services."; }
			else if(code=="MA51") { return "Missing/incomplete/invalid CLIA certification number for laboratory services billed by physician office laboratory."; }
			else if(code=="MA52") { return "Missing/incomplete/invalid date."; }
			else if(code=="MA53") { return "Missing/incomplete/invalid Competitive Bidding Demonstration Project identification."; }
			else if(code=="MA54") { return "Physician certification or election consent for hospice care not received timely."; }
			else if(code=="MA55") { return "Not covered as patient received medical health care services, automatically revoking his/her election to receive religious non-medical health care services."; }
			else if(code=="MA56") { return "Our records show you have opted out of Medicare, agreeing with the patient not to bill Medicare for services/tests/supplies furnished. As result, we cannot pay this claim. The patient is responsible for payment, but under Federal law, you cannot charge the patient more than the limiting charge amount."; }
			else if(code=="MA57") { return "Patient submitted written request to revoke his/her election for religious non-medical health care services."; }
			else if(code=="MA58") { return "Missing/incomplete/invalid release of information indicator."; }
			else if(code=="MA59") { return "Alert: The patient overpaid you for these services. You must issue the patient a refund within 30 days for the difference between his/her payment and the total amount shown as patient portion on this notice."; }
			else if(code=="MA60") { return "Missing/incomplete/invalid patient relationship to insured."; }
			else if(code=="MA61") { return "Missing/incomplete/invalid social security number or health insurance claim number."; }
			else if(code=="MA62") { return "Alert: This is a telephone review decision."; }
			else if(code=="MA63") { return "Missing/incomplete/invalid principal diagnosis."; }
			else if(code=="MA64") { return "Our records indicate that we should be the third payer for this claim. We cannot process this claim until we have received payment information from the primary and secondary payers."; }
			else if(code=="MA65") { return "Missing/incomplete/invalid admitting diagnosis."; }
			else if(code=="MA66") { return "Missing/incomplete/invalid principal procedure code."; }
			else if(code=="MA67") { return "Correction to a prior claim."; }
			else if(code=="MA68") { return "Alert: We did not crossover this claim because the secondary insurance information on the claim was incomplete. Please supply complete information or use the PLANID of the insurer to assure correct and timely routing of the claim."; }
			else if(code=="MA69") { return "Missing/incomplete/invalid remarks."; }
			else if(code=="MA70") { return "Missing/incomplete/invalid provider representative signature."; }
			else if(code=="MA71") { return "Missing/incomplete/invalid provider representative signature date."; }
			else if(code=="MA72") { return "Alert: The patient overpaid you for these assigned services. You must issue the patient a refund within 30 days for the difference between his/her payment to you and the total of the amount shown as patient portion and as paid to the patient on this notice."; }
			else if(code=="MA73") { return "Informational remittance associated with a Medicare demonstration. No payment issued under fee-for-service Medicare as patient has elected managed care."; }
			else if(code=="MA74") { return "This payment replaces an earlier payment for this claim that was either lost, damaged or returned."; }
			else if(code=="MA75") { return "Missing/incomplete/invalid patient or authorized representative signature."; }
			else if(code=="MA76") { return "Missing/incomplete/invalid provider identifier for home health agency or hospice when physician is performing care plan oversight services."; }
			else if(code=="MA77") { return "Alert: The patient overpaid you. You must issue the patient a refund within 30 days for the difference between the patient’s payment less the total of our and other payer payments and the amount shown as patient portion on this notice."; }
			else if(code=="MA78") { return "The patient overpaid you. You must issue the patient a refund within 30 days for the difference between our allowed amount total and the amount paid by the patient."; }
			else if(code=="MA79") { return "Billed in excess of interim rate."; }
			else if(code=="MA80") { return "Informational notice. No payment issued for this claim with this notice. Payment issued to the hospital by its intermediary for all services for this encounter under a demonstration project."; }
			else if(code=="MA81") { return "Missing/incomplete/invalid provider/supplier signature."; }
			else if(code=="MA82") { return "Missing/incomplete/invalid provider/supplier billing number/identifier or billing name, address, city, state, zip code, or phone number."; }
			else if(code=="MA83") { return "Did not indicate whether we are the primary or secondary payer."; }
			else if(code=="MA84") { return "Patient identified as participating in the National Emphysema Treatment Trial but our records indicate that this patient is either not a participant, or has not yet been approved for this phase of the study. Contact Johns Hopkins University, the study coordinator, to resolve if there was a discrepancy."; }
			else if(code=="MA85") { return "Our records indicate that a primary payer exists (other than ourselves); however, you did not complete or enter accurately the insurance plan/group/program name or identification number. Enter the PlanID when effective."; }
			else if(code=="MA86") { return "Missing/incomplete/invalid group or policy number of the insured for the primary coverage."; }
			else if(code=="MA87") { return "Missing/incomplete/invalid insured's name for the primary payer."; }
			else if(code=="MA88") { return "Missing/incomplete/invalid insured's address and/or telephone number for the primary payer."; }
			else if(code=="MA89") { return "Missing/incomplete/invalid patient's relationship to the insured for the primary payer."; }
			else if(code=="MA90") { return "Missing/incomplete/invalid employment status code for the primary insured."; }
			else if(code=="MA91") { return "This determination is the result of the appeal you filed."; }
			else if(code=="MA92") { return "Missing plan information for other insurance."; }
			else if(code=="MA93") { return "Non-PIP (Periodic Interim Payment) claim."; }
			else if(code=="MA94") { return "Did not enter the statement “Attending physician not hospice employee” on the claim form to certify that the rendering physician is not an employee of the hospice."; }
			else if(code=="MA95") { return "A not otherwise classified or unlisted procedure code(s) was billed but a narrative description of the procedure was not entered on the claim. Refer to item 19 on the HCFA-1500."; }
			else if(code=="MA96") { return "Claim rejected. Coded as a Medicare Managed Care Demonstration but patient is not enrolled in a Medicare managed care plan."; }
			else if(code=="MA97") { return "Missing/incomplete/invalid Medicare Managed Care Demonstration contract number or clinical trial registry number."; }
			else if(code=="MA98") { return "Claim Rejected. Does not contain the correct Medicare Managed Care Demonstration contract number for this beneficiary."; }
			else if(code=="MA99") { return "Missing/incomplete/invalid Medigap information."; }
			else if(code=="MA100") { return "Missing/incomplete/invalid date of current illness or symptoms"; }
			else if(code=="MA101") { return "A Skilled Nursing Facility (SNF) is responsible for payment of outside providers who furnish these services/supplies to residents."; }
			else if(code=="MA102") { return "Missing/incomplete/invalid name or provider identifier for the rendering/referring/ ordering/ supervising provider."; }
			else if(code=="MA103") { return "Hemophilia Add On."; }
			else if(code=="MA104") { return "Missing/incomplete/invalid date the patient was last seen or the provider identifier of the attending physician."; }
			else if(code=="MA105") { return "Missing/incomplete/invalid provider number for this place of service."; }
			else if(code=="MA106") { return "PIP (Periodic Interim Payment) claim."; }
			else if(code=="MA107") { return "Paper claim contains more than three separate data items in field 19."; }
			else if(code=="MA108") { return "Paper claim contains more than one data item in field 23."; }
			else if(code=="MA109") { return "Claim processed in accordance with ambulatory surgical guidelines."; }
			else if(code=="MA110") { return "Missing/incomplete/invalid information on whether the diagnostic test(s) were performed by an outside entity or if no purchased tests are included on the claim."; }
			else if(code=="MA111") { return "Missing/incomplete/invalid purchase price of the test(s) and/or the performing laboratory's name and address."; }
			else if(code=="MA112") { return "Missing/incomplete/invalid group practice information."; }
			else if(code=="MA113") { return "Incomplete/invalid taxpayer identification number (TIN) submitted by you per the Internal Revenue Service. Your claims cannot be processed without your correct TIN, and you may not bill the patient pending correction of your TIN. There are no appeal rights for unprocessable claims, but you may resubmit this claim after you have notified this office of your correct TIN."; }
			else if(code=="MA114") { return "Missing/incomplete/invalid information on where the services were furnished."; }
			else if(code=="MA115") { return "Missing/incomplete/invalid physical location (name and address, or PIN) where the service(s) were rendered in a Health Professional Shortage Area (HPSA)."; }
			else if(code=="MA116") { return "Did not complete the statement 'Homebound' on the claim to validate whether laboratory services were performed at home or in an institution."; }
			else if(code=="MA117") { return "This claim has been assessed a $1.00 user fee."; }
			else if(code=="MA118") { return "Coinsurance and/or deductible amounts apply to a claim for services or supplies furnished to a Medicare-eligible veteran through a facility of the Department of Veterans Affairs. No Medicare payment issued."; }
			else if(code=="MA119") { return "Provider level adjustment for late claim filing applies to this claim."; }
			else if(code=="MA120") { return "Missing/incomplete/invalid CLIA certification number."; }
			else if(code=="MA121") { return "Missing/incomplete/invalid x-ray date."; }
			else if(code=="MA122") { return "Missing/incomplete/invalid initial treatment date."; }
			else if(code=="MA123") { return "Your center was not selected to participate in this study, therefore, we cannot pay for these services."; }
			else if(code=="MA124") { return "Processed for IME only."; }
			else if(code=="MA125") { return "Per legislation governing this program, payment constitutes payment in full."; }
			else if(code=="MA126") { return "Pancreas transplant not covered unless kidney transplant performed."; }
			else if(code=="MA127") { return "Reserved for future use."; }
			else if(code=="MA128") { return "Missing/incomplete/invalid FDA approval number."; }
			else if(code=="MA129") { return "This provider was not certified for this procedure on this date of service."; }
			else if(code=="MA130") { return "Your claim contains incomplete and/or invalid information, and no appeal rights are afforded because the claim is unprocessable. Please submit a new claim with the complete/correct information."; }
			else if(code=="MA131") { return "Physician already paid for services in conjunction with this demonstration claim. You must have the physician withdraw that claim and refund the payment before we can process your claim."; }
			else if(code=="MA132") { return "Adjustment to the pre-demonstration rate."; }
			else if(code=="MA133") { return "Claim overlaps inpatient stay. Rebill only those services rendered outside the inpatient stay."; }
			else if(code=="MA134") { return "Missing/incomplete/invalid provider number of the facility where the patient resides."; }
			else if(code=="N1") { return "Alert: You may appeal this decision in writing within the required time limits following receipt of this notice by following the instructions included in your contract, plan benefit documents or jurisdiction statutes."; }
			else if(code=="N2") { return "This allowance has been made in accordance with the most appropriate course of treatment provision of the plan."; }
			else if(code=="N3") { return "Missing consent form."; }
			else if(code=="N4") { return "Missing/Incomplete/Invalid prior Insurance Carrier(s) EOB."; }
			else if(code=="N5") { return "EOB received from previous payer. Claim not on file."; }
			else if(code=="N6") { return "Under FEHB law (U.S.C. 8904(b)), we cannot pay more for covered care than the amount Medicare would have allowed if the patient were enrolled in Medicare Part A and/or Medicare Part B."; }
			else if(code=="N7") { return "Alert: Processing of this claim/service has included consideration under Major Medical provisions."; }
			else if(code=="N8") { return "Crossover claim denied by previous payer and complete claim data not forwarded. Resubmit this claim to this payer to provide adequate data for adjudication."; }
			else if(code=="N9") { return "Adjustment represents the estimated amount a previous payer may pay."; }
			else if(code=="N10") { return "Payment based on the findings of a review organization/professional consult/manual adjudication/medical advisor/dental advisor/peer review."; }
			else if(code=="N11") { return "Denial reversed because of medical review."; }
			else if(code=="N12") { return "Policy provides coverage supplemental to Medicare. As the member does not appear to be enrolled in the applicable part of Medicare, the member is responsible for payment of the portion of the charge that would have been covered by Medicare."; }
			else if(code=="N13") { return "Payment based on professional/technical component modifier(s)."; }
			else if(code=="N14") { return "Payment based on a contractual amount or agreement, fee schedule, or maximum allowable amount."; }
			else if(code=="N15") { return "Services for a newborn must be billed separately."; }
			else if(code=="N16") { return "Family/member Out-of-Pocket maximum has been met. Payment based on a higher percentage."; }
			else if(code=="N17") { return "Per admission deductible."; }
			else if(code=="N18") { return "Payment based on the Medicare allowed amount."; }
			else if(code=="N19") { return "Procedure code incidental to primary procedure."; }
			else if(code=="N20") { return "Service not payable with other service rendered on the same date."; }
			else if(code=="N21") { return "Alert: Your line item has been separated into multiple lines to expedite handling."; }
			else if(code=="N22") { return "This procedure code was added/changed because it more accurately describes the services rendered."; }
			else if(code=="N23") { return "Alert: Patient liability may be affected due to coordination of benefits with other carriers and/or maximum benefit provisions."; }
			else if(code=="N24") { return "Missing/incomplete/invalid Electronic Funds Transfer (EFT) banking information."; }
			else if(code=="N25") { return "This company has been contracted by your benefit plan to provide administrative claims payment services only. This company does not assume financial risk or obligation with respect to claims processed on behalf of your benefit plan."; }
			else if(code=="N26") { return "Missing itemized bill/statement."; }
			else if(code=="N27") { return "Missing/incomplete/invalid treatment number."; }
			else if(code=="N28") { return "Consent form requirements not fulfilled."; }
			else if(code=="N29") { return "Missing documentation/orders/notes/summary/report/chart."; }
			else if(code=="N30") { return "Patient ineligible for this service."; }
			else if(code=="N31") { return "Missing/incomplete/invalid prescribing provider identifier."; }
			else if(code=="N32") { return "Claim must be submitted by the provider who rendered the service."; }
			else if(code=="N33") { return "No record of health check prior to initiation of treatment."; }
			else if(code=="N34") { return "Incorrect claim form/format for this service."; }
			else if(code=="N35") { return "Program integrity/utilization review decision."; }
			else if(code=="N36") { return "Claim must meet primary payer’s processing requirements before we can consider payment."; }
			else if(code=="N37") { return "Missing/incomplete/invalid tooth number/letter."; }
			else if(code=="N38") { return "Missing/incomplete/invalid place of service."; }
			else if(code=="N39") { return "Procedure code is not compatible with tooth number/letter."; }
			else if(code=="N40") { return "Missing radiology film(s)/image(s)."; }
			else if(code=="N41") { return "Authorization request denied."; }
			else if(code=="N42") { return "No record of mental health assessment."; }
			else if(code=="N43") { return "Bed hold or leave days exceeded."; }
			else if(code=="N44") { return "Payer’s share of regulatory surcharges, assessments, allowances or health care-related taxes paid directly to the regulatory authority."; }
			else if(code=="N45") { return "Payment based on authorized amount."; }
			else if(code=="N46") { return "Missing/incomplete/invalid admission hour."; }
			else if(code=="N47") { return "Claim conflicts with another inpatient stay."; }
			else if(code=="N48") { return "Claim information does not agree with information received from other insurance carrier."; }
			else if(code=="N49") { return "Court ordered coverage information needs validation."; }
			else if(code=="N50") { return "Missing/incomplete/invalid discharge information."; }
			else if(code=="N51") { return "Electronic interchange agreement not on file for provider/submitter."; }
			else if(code=="N52") { return "Patient not enrolled in the billing provider's managed care plan on the date of service."; }
			else if(code=="N53") { return "Missing/incomplete/invalid point of pick-up address."; }
			else if(code=="N54") { return "Claim information is inconsistent with pre-certified/authorized services."; }
			else if(code=="N55") { return "Procedures for billing with group/referring/performing providers were not followed."; }
			else if(code=="N56") { return "Procedure code billed is not correct/valid for the services billed or the date of service billed."; }
			else if(code=="N57") { return "Missing/incomplete/invalid prescribing date."; }
			else if(code=="N58") { return "Missing/incomplete/invalid patient liability amount."; }
			else if(code=="N59") { return "Please refer to your provider manual for additional program and provider information."; }
			else if(code=="N60") { return "A valid NDC is required for payment of drug claims effective October 02."; }
			else if(code=="N61") { return "Rebill services on separate claims."; }
			else if(code=="N62") { return "Dates of service span multiple rate periods. Resubmit separate claims."; }
			else if(code=="N63") { return "Rebill services on separate claim lines."; }
			else if(code=="N64") { return "The “from” and “to” dates must be different."; }
			else if(code=="N65") { return "Procedure code or procedure rate count cannot be determined, or was not on file, for the date of service/provider."; }
			else if(code=="N66") { return "Missing/incomplete/invalid documentation."; }
			else if(code=="N67") { return "Professional provider services not paid separately. Included in facility payment under a demonstration project. Apply to that facility for payment, or resubmit your claim if: the facility notifies you the patient was excluded from this demonstration; or if you furnished these services in another location on the date of the patient’s admission or discharge from a demonstration hospital. If services were furnished in a facility not involved in the demonstration on the same date the patient was discharged from or admitted to a demonstration facility, you must report the provider ID number for the non-demonstration facility on the new claim."; }
			else if(code=="N68") { return "Prior payment being cancelled as we were subsequently notified this patient was covered by a demonstration project in this site of service. Professional services were included in the payment made to the facility. You must contact the facility for your payment. Prior payment made to you by the patient or another insurer for this claim must be refunded to the payer within 30 days."; }
			else if(code=="N69") { return "PPS (Prospective Payment System) code changed by claims processing system."; }
			else if(code=="N70") { return "Consolidated billing and payment applies."; }
			else if(code=="N71") { return "Your unassigned claim for a drug or biological, clinical diagnostic laboratory services or ambulance service was processed as an assigned claim. You are required by law to accept assignment for these types of claims."; }
			else if(code=="N72") { return "PPS (Prospective Payment System) code changed by medical reviewers. Not supported by clinical records."; }
			else if(code=="N73") { return "A Skilled Nursing Facility is responsible for payment of outside providers who furnish these services/supplies under arrangement to its residents."; }
			else if(code=="N74") { return "Resubmit with multiple claims, each claim covering services provided in only one calendar month."; }
			else if(code=="N75") { return "Missing/incomplete/invalid tooth surface information."; }
			else if(code=="N76") { return "Missing/incomplete/invalid number of riders."; }
			else if(code=="N77") { return "Missing/incomplete/invalid designated provider number."; }
			else if(code=="N78") { return "The necessary components of the child and teen checkup (EPSDT) were not completed."; }
			else if(code=="N79") { return "Service billed is not compatible with patient location information."; }
			else if(code=="N80") { return "Missing/incomplete/invalid prenatal screening information."; }
			else if(code=="N81") { return "Procedure billed is not compatible with tooth surface code."; }
			else if(code=="N82") { return "Provider must accept insurance payment as payment in full when a third party payer contract specifies full reimbursement."; }
			else if(code=="N83") { return "No appeal rights. Adjudicative decision based on the provisions of a demonstration project."; }
			else if(code=="N84") { return "Alert: Further installment payments are forthcoming."; }
			else if(code=="N85") { return "Alert: This is the final installment payment."; }
			else if(code=="N86") { return "A failed trial of pelvic muscle exercise training is required in order for biofeedback training for the treatment of urinary incontinence to be covered."; }
			else if(code=="N87") { return "Home use of biofeedback therapy is not covered."; }
			else if(code=="N88") { return "Alert: This payment is being made conditionally. An HHA episode of care notice has been filed for this patient. When a patient is treated under a HHA episode of care, consolidated billing requires that certain therapy services and supplies, such as this, be included in the HHA's payment. This payment will need to be recouped from you if we establish that the patient is concurrently receiving treatment under a HHA episode of care."; }
			else if(code=="N89") { return "Alert: Payment information for this claim has been forwarded to more than one other payer, but format limitations permit only one of the secondary payers to be identified in this remittance advice."; }
			else if(code=="N90") { return "Covered only when performed by the attending physician."; }
			else if(code=="N91") { return "Services not included in the appeal review."; }
			else if(code=="N92") { return "This facility is not certified for digital mammography."; }
			else if(code=="N93") { return "A separate claim must be submitted for each place of service. Services furnished at multiple sites may not be billed in the same claim."; }
			else if(code=="N94") { return "Claim/Service denied because a more specific taxonomy code is required for adjudication."; }
			else if(code=="N95") { return "This provider type/provider specialty may not bill this service."; }
			else if(code=="N96") { return "Patient must be refractory to conventional therapy (documented behavioral, pharmacologic and/or surgical corrective therapy) and be an appropriate surgical candidate such that implantation with anesthesia can occur."; }
			else if(code=="N97") { return "Patients with stress incontinence, urinary obstruction, and specific neurologic diseases (e.g., diabetes with peripheral nerve involvement) which are associated with secondary manifestations of the above three indications are excluded."; }
			else if(code=="N98") { return "Patient must have had a successful test stimulation in order to support subsequent implantation. Before a patient is eligible for permanent implantation, he/she must demonstrate a 50 percent or greater improvement through test stimulation. Improvement is measured through voiding diaries."; }
			else if(code=="N99") { return "Patient must be able to demonstrate adequate ability to record voiding diary data such that clinical results of the implant procedure can be properly evaluated."; }
			else if(code=="N100") { return "PPS (Prospect Payment System) code corrected during adjudication."; }
			else if(code=="N101") { return "Additional information is needed in order to process this claim. Please resubmit the claim with the identification number of the provider where this service took place. The Medicare number of the site of service provider should be preceded with the letters 'HSP' and entered into item #32 on the claim form. You may bill only one site of service provider number per claim."; }
			else if(code=="N102") { return "This claim has been denied without reviewing the medical/dental record because the requested records were not received or were not received timely."; }
			else if(code=="N103") { return "Records indicate this patient was a prisoner or in custody of a Federal, State, or local authority when the service was rendered. This payer does not cover items and services furnished to an individual while he or she is in custody under a penal statute or rule, unless under State or local law, the individual is personally liable for the cost of his or her health care while in custody and the State or local government pursues the collection of such debt in the same way and with the same vigor as the collection of its other debts. The provider can collect from the Federal/State/ Local Authority as appropriate."; }
			else if(code=="N104") { return "This claim/service is not payable under our claims jurisdiction area. You can identify the correct Medicare contractor to process this claim/service through the CMS website at www.cms.gov."; }
			else if(code=="N105") { return "This is a misdirected claim/service for an RRB beneficiary. Submit paper claims to the RRB carrier: Palmetto GBA, P.O. Box 10066, Augusta, GA 30999. Call 866-749-4301 for RRB EDI information for electronic claims processing."; }
			else if(code=="N106") { return "Payment for services furnished to Skilled Nursing Facility (SNF) inpatients (except for excluded services) can only be made to the SNF. You must request payment from the SNF rather than the patient for this service."; }
			else if(code=="N107") { return "Services furnished to Skilled Nursing Facility (SNF) inpatients must be billed on the inpatient claim. They cannot be billed separately as outpatient services."; }
			else if(code=="N108") { return "Missing/incomplete/invalid upgrade information."; }
			else if(code=="N109") { return "This claim/service was chosen for complex review and was denied after reviewing the medical records."; }
			else if(code=="N110") { return "This facility is not certified for film mammography."; }
			else if(code=="N111") { return "No appeal right except duplicate claim/service issue. This service was included in a claim that has been previously billed and adjudicated."; }
			else if(code=="N112") { return "This claim is excluded from your electronic remittance advice."; }
			else if(code=="N113") { return "Only one initial visit is covered per physician, group practice or provider."; }
			else if(code=="N114") { return "During the transition to the Ambulance Fee Schedule, payment is based on the lesser of a blended amount calculated using a percentage of the reasonable charge/cost and fee schedule amounts, or the submitted charge for the service. You will be notified yearly what the percentages for the blended payment calculation will be."; }
			else if(code=="N115") { return "This decision was based on a Local Coverage Determination (LCD). An LCD provides a guide to assist in determining whether a particular item or service is covered. A copy of this policy is available at www.cms.gov/mcd, or if you do not have web access, you may contact the contractor to request a copy of the LCD."; }
			else if(code=="N116") { return "This payment is being made conditionally because the service was provided in the home, and it is possible that the patient is under a home health episode of care. When a patient is treated under a home health episode of care, consolidated billing requires that certain therapy services and supplies, such as this, be included in the home health agency’s (HHA’s) payment. This payment will need to be recouped from you if we establish that the patient is concurrently receiving treatment under an HHA episode of care."; }
			else if(code=="N117") { return "This service is paid only once in a patient’s lifetime."; }
			else if(code=="N118") { return "This service is not paid if billed more than once every 28 days."; }
			else if(code=="N119") { return "This service is not paid if billed once every 28 days, and the patient has spent 5 or more consecutive days in any inpatient or Skilled /nursing Facility (SNF) within those 28 days."; }
			else if(code=="N120") { return "Payment is subject to home health prospective payment system partial episode payment adjustment. Patient was transferred/discharged/readmitted during payment episode."; }
			else if(code=="N121") { return "Medicare Part B does not pay for items or services provided by this type of practitioner for beneficiaries in a Medicare Part A covered Skilled Nursing Facility (SNF) stay."; }
			else if(code=="N122") { return "Add-on code cannot be billed by itself."; }
			else if(code=="N123") { return "This is a split service and represents a portion of the units from the originally submitted service."; }
			else if(code=="N124") { return "Payment has been denied for the/made only for a less extensive service/item because the information furnished does not substantiate the need for the (more extensive) service/item. The patient is liable for the charges for this service/item as you informed the patient in writing before the service/item was furnished that we would not pay for it, and the patient agreed to pay."; }
			else if(code=="N125") { return "Payment has been (denied for the/made only for a less extensive) service/item because the information furnished does not substantiate the need for the (more extensive) service/item. If you have collected any amount from the patient, you must refund that amount to the patient within 30 days of receiving this notice. The requirements for a refund are in §1834(a)(18) of the Social Security Act (and in §§1834(j)(4) and 1879(h) by cross-reference to §1834(a)(18)). Section 1834(a)(18)(B) specifies that suppliers which knowingly and willfully fail to make appropriate refunds may be subject to civil money penalties and/or exclusion from the Medicare program. If you have any questions about this notice, please contact this office."; }
			else if(code=="N126") { return "Social Security Records indicate that this individual has been deported. This payer does not cover items and services furnished to individuals who have been deported."; }
			else if(code=="N127") { return "This is a misdirected claim/service for a United Mine Workers of America (UMWA) beneficiary. Please submit claims to them."; }
			else if(code=="N128") { return "This amount represents the prior to coverage portion of the allowance."; }
			else if(code=="N129") { return "Not eligible due to the patient's age."; }
			else if(code=="N130") { return "Consult plan benefit documents/guidelines for information about restrictions for this service."; }
			else if(code=="N131") { return "Total payments under multiple contracts cannot exceed the allowance for this service."; }
			else if(code=="N132") { return "Alert: Payments will cease for services rendered by this US Government debarred or excluded provider after the 30 day grace period as previously notified."; }
			else if(code=="N133") { return "Alert: Services for predetermination and services requesting payment are being processed separately."; }
			else if(code=="N134") { return "Alert: This represents your scheduled payment for this service. If treatment has been discontinued, please contact Customer Service."; }
			else if(code=="N135") { return "Record fees are the patient's portion and limited to the specified co-payment."; }
			else if(code=="N136") { return "Alert: To obtain information on the process to file an appeal in Arizona, call the Department's Consumer Assistance Office at (602) 912-8444 or (800) 325-2548."; }
			else if(code=="N137") { return "Alert: The provider acting on the Member's behalf, may file an appeal with the Payer. The provider, acting on the Member's behalf, may file a complaint with the State Insurance Regulatory Authority without first filing an appeal, if the coverage decision involves an urgent condition for which care has not been rendered. The address may be obtained from the State Insurance Regulatory Authority."; }
			else if(code=="N138") { return "Alert: In the event you disagree with the Dental Advisor's opinion and have additional information relative to the case, you may submit radiographs to the Dental Advisor Unit at the subscriber's dental insurance carrier for a second Independent Dental Advisor Review."; }
			else if(code=="N139") { return "Alert: Under the Code of Federal Regulations, Chapter 32, Section 199.13 a non-participating provider is not an appropriate appealing party. Therefore, if you disagree with the Dental Advisor's opinion, you may appeal the determination if appointed in writing, by the beneficiary, to act as his/her representative. Should you be appointed as a representative, submit a copy of this letter, a signed statement explaining the matter in which you disagree, and any radiographs and relevant information to the subscriber's Dental insurance carrier within 90 days from the date of this letter."; }
			else if(code=="N140") { return "Alert: You have not been designated as an authorized OCONUS provider therefore are not considered an appropriate appealing party. If the beneficiary has appointed you, in writing, to act as his/her representative and you disagree with the Dental Advisor's opinion, you may appeal by submitting a copy of this letter, a signed statement explaining the matter in which you disagree, and any relevant information to the subscriber's Dental insurance carrier within 90 days from the date of this letter."; }
			else if(code=="N141") { return "The patient was not residing in a long-term care facility during all or part of the service dates billed."; }
			else if(code=="N142") { return "The original claim was denied. Resubmit a new claim, not a replacement claim."; }
			else if(code=="N143") { return "The patient was not in a hospice program during all or part of the service dates billed."; }
			else if(code=="N144") { return "The rate changed during the dates of service billed."; }
			else if(code=="N145") { return "Missing/incomplete/invalid provider identifier for this place of service."; }
			else if(code=="N146") { return "Missing screening document."; }
			else if(code=="N147") { return "Long term care case mix or per diem rate cannot be determined because the patient ID number is missing, incomplete, or invalid on the assignment request."; }
			else if(code=="N148") { return "Missing/incomplete/invalid date of last menstrual period."; }
			else if(code=="N149") { return "Rebill all applicable services on a single claim."; }
			else if(code=="N150") { return "Missing/incomplete/invalid model number."; }
			else if(code=="N151") { return "Telephone contact services will not be paid until the face-to-face contact requirement has been met."; }
			else if(code=="N152") { return "Missing/incomplete/invalid replacement claim information."; }
			else if(code=="N153") { return "Missing/incomplete/invalid room and board rate."; }
			else if(code=="N154") { return "Alert: This payment was delayed for correction of provider's mailing address."; }
			else if(code=="N155") { return "Alert: Our records do not indicate that other insurance is on file. Please submit other insurance information for our records."; }
			else if(code=="N156") { return "Alert: The patient is responsible for the difference between the approved treatment and the elective treatment."; }
			else if(code=="N157") { return "Transportation to/from this destination is not covered."; }
			else if(code=="N158") { return "Transportation in a vehicle other than an ambulance is not covered."; }
			else if(code=="N159") { return "Payment denied/reduced because mileage is not covered when the patient is not in the ambulance."; }
			else if(code=="N160") { return "The patient must choose an option before a payment can be made for this procedure/ equipment/ supply/ service."; }
			else if(code=="N161") { return "This drug/service/supply is covered only when the associated service is covered."; }
			else if(code=="N162") { return "Alert: Although your claim was paid, you have billed for a test/specialty not included in your Laboratory Certification. Your failure to correct the laboratory certification information will result in a denial of payment in the near future."; }
			else if(code=="N163") { return "Medical record does not support code billed per the code definition."; }
			else if(code=="N164") { return "Transportation to/from this destination is not covered."; }
			else if(code=="N165") { return "Transportation in a vehicle other than an ambulance is not covered."; }
			else if(code=="N166") { return "Payment denied/reduced because mileage is not covered when the patient is not in the ambulance."; }
			else if(code=="N167") { return "Charges exceed the post-transplant coverage limit."; }
			else if(code=="N168") { return "The patient must choose an option before a payment can be made for this procedure/ equipment/ supply/ service."; }
			else if(code=="N169") { return "This drug/service/supply is covered only when the associated service is covered."; }
			else if(code=="N170") { return "A new/revised/renewed certificate of medical necessity is needed."; }
			else if(code=="N171") { return "Payment for repair or replacement is not covered or has exceeded the purchase price."; }
			else if(code=="N172") { return "The patient is not liable for the denied/adjusted charge(s) for receiving any updated service/item."; }
			else if(code=="N173") { return "No qualifying hospital stay dates were provided for this episode of care."; }
			else if(code=="N174") { return "This is not a covered service/procedure/ equipment/bed, however patient liability is limited to amounts shown in the adjustments under group 'PR'."; }
			else if(code=="N175") { return "Missing review organization approval."; }
			else if(code=="N176") { return "Services provided aboard a ship are covered only when the ship is of United States registry and is in United States waters. In addition, a doctor licensed to practice in the United States must provide the service."; }
			else if(code=="N177") { return "Alert: We did not send this claim to patient’s other insurer. They have indicated no additional payment can be made."; }
			else if(code=="N178") { return "Missing pre-operative images/visual field results."; }
			else if(code=="N179") { return "Additional information has been requested from the member. The charges will be reconsidered upon receipt of that information."; }
			else if(code=="N180") { return "This item or service does not meet the criteria for the category under which it was billed."; }
			else if(code=="N181") { return "Additional information is required from another provider involved in this service."; }
			else if(code=="N182") { return "This claim/service must be billed according to the schedule for this plan."; }
			else if(code=="N183") { return "Alert: This is a predetermination advisory message, when this service is submitted for payment additional documentation as specified in plan documents will be required to process benefits."; }
			else if(code=="N184") { return "Rebill technical and professional components separately."; }
			else if(code=="N185") { return "Alert: Do not resubmit this claim/service."; }
			else if(code=="N186") { return "Non-Availability Statement (NAS) required for this service. Contact the nearest Military Treatment Facility (MTF) for assistance."; }
			else if(code=="N187") { return "Alert: You may request a review in writing within the required time limits following receipt of this notice by following the instructions included in your contract or plan benefit documents."; }
			else if(code=="N188") { return "The approved level of care does not match the procedure code submitted."; }
			else if(code=="N189") { return "Alert: This service has been paid as a one-time exception to the plan's benefit restrictions."; }
			else if(code=="N190") { return "Missing contract indicator."; }
			else if(code=="N191") { return "The provider must update insurance information directly with payer."; }
			else if(code=="N192") { return "Patient is a Medicaid/Qualified Medicare Beneficiary."; }
			else if(code=="N193") { return "Specific federal/state/local program may cover this service through another payer."; }
			else if(code=="N194") { return "Technical component not paid if provider does not own the equipment used."; }
			else if(code=="N195") { return "The technical component must be billed separately."; }
			else if(code=="N196") { return "Alert: Patient eligible to apply for other coverage which may be primary."; }
			else if(code=="N197") { return "The subscriber must update insurance information directly with payer."; }
			else if(code=="N198") { return "Rendering provider must be affiliated with the pay-to provider."; }
			else if(code=="N199") { return "Additional payment/recoupment approved based on payer-initiated review/audit."; }
			else if(code=="N200") { return "The professional component must be billed separately."; }
			else if(code=="N201") { return "A mental health facility is responsible for payment of outside providers who furnish these services/supplies to residents."; }
			else if(code=="N202") { return "Additional information/explanation will be sent separately"; }
			else if(code=="N203") { return "Missing/incomplete/invalid anesthesia time/units"; }
			else if(code=="N204") { return "Services under review for possible pre-existing condition. Send medical records for prior 12 months"; }
			else if(code=="N205") { return "Information provided was illegible"; }
			else if(code=="N206") { return "The supporting documentation does not match the information sent on the claim."; }
			else if(code=="N207") { return "Missing/incomplete/invalid weight."; }
			else if(code=="N208") { return "Missing/incomplete/invalid DRG code"; }
			else if(code=="N209") { return "Missing/incomplete/invalid taxpayer identification number (TIN)."; }
			else if(code=="N210") { return "Alert: You may appeal this decision"; }
			else if(code=="N211") { return "Alert: You may not appeal this decision"; }
			else if(code=="N212") { return "Charges processed under a Point of Service benefit"; }
			else if(code=="N213") { return "Missing/incomplete/invalid facility/discrete unit DRG/DRG exempt status information"; }
			else if(code=="N214") { return "Missing/incomplete/invalid history of the related initial surgical procedure(s)"; }
			else if(code=="N215") { return "Alert: A payer providing supplemental or secondary coverage shall not require a claims determination for this service from a primary payer as a condition of making its own claims determination."; }
			else if(code=="N216") { return "We do not offer coverage for this type of service or the patient is not enrolled in this portion of our benefit package"; }
			else if(code=="N217") { return "We pay only one site of service per provider per claim"; }
			else if(code=="N218") { return "You must furnish and service this item for as long as the patient continues to need it. We can pay for maintenance and/or servicing for the time period specified in the contract or coverage manual."; }
			else if(code=="N219") { return "Payment based on previous payer's allowed amount."; }
			else if(code=="N220") { return "Alert: See the payer's web site or contact the payer's Customer Service department to obtain forms and instructions for filing a provider dispute."; }
			else if(code=="N221") { return "Missing Admitting History and Physical report."; }
			else if(code=="N222") { return "Incomplete/invalid Admitting History and Physical report."; }
			else if(code=="N223") { return "Missing documentation of benefit to the patient during initial treatment period."; }
			else if(code=="N224") { return "Incomplete/invalid documentation of benefit to the patient during initial treatment period."; }
			else if(code=="N225") { return "Incomplete/invalid documentation/orders/notes/summary/report/chart."; }
			else if(code=="N226") { return "Incomplete/invalid American Diabetes Association Certificate of Recognition."; }
			else if(code=="N227") { return "Incomplete/invalid Certificate of Medical Necessity."; }
			else if(code=="N228") { return "Incomplete/invalid consent form."; }
			else if(code=="N229") { return "Incomplete/invalid contract indicator."; }
			else if(code=="N230") { return "Incomplete/invalid indication of whether the patient owns the equipment that requires the part or supply."; }
			else if(code=="N231") { return "Incomplete/invalid invoice or statement certifying the actual cost of the lens, less discounts, and/or the type of intraocular lens used."; }
			else if(code=="N232") { return "Incomplete/invalid itemized bill/statement."; }
			else if(code=="N233") { return "Incomplete/invalid operative note/report."; }
			else if(code=="N234") { return "Incomplete/invalid oxygen certification/re-certification."; }
			else if(code=="N235") { return "Incomplete/invalid pacemaker registration form."; }
			else if(code=="N236") { return "Incomplete/invalid pathology report."; }
			else if(code=="N237") { return "Incomplete/invalid patient medical record for this service."; }
			else if(code=="N238") { return "Incomplete/invalid physician certified plan of care"; }
			else if(code=="N239") { return "Incomplete/invalid physician financial relationship form."; }
			else if(code=="N240") { return "Incomplete/invalid radiology report."; }
			else if(code=="N241") { return "Incomplete/invalid review organization approval."; }
			else if(code=="N242") { return "Incomplete/invalid radiology film(s)/image(s)."; }
			else if(code=="N243") { return "Incomplete/invalid/not approved screening document."; }
			else if(code=="N244") { return "Incomplete/Invalid pre-operative images/visual field results."; }
			else if(code=="N245") { return "Incomplete/invalid plan information for other insurance"; }
			else if(code=="N246") { return "State regulated patient payment limitations apply to this service."; }
			else if(code=="N247") { return "Missing/incomplete/invalid assistant surgeon taxonomy."; }
			else if(code=="N248") { return "Missing/incomplete/invalid assistant surgeon name."; }
			else if(code=="N249") { return "Missing/incomplete/invalid assistant surgeon primary identifier."; }
			else if(code=="N250") { return "Missing/incomplete/invalid assistant surgeon secondary identifier."; }
			else if(code=="N251") { return "Missing/incomplete/invalid attending provider taxonomy."; }
			else if(code=="N252") { return "Missing/incomplete/invalid attending provider name."; }
			else if(code=="N253") { return "Missing/incomplete/invalid attending provider primary identifier."; }
			else if(code=="N254") { return "Missing/incomplete/invalid attending provider secondary identifier."; }
			else if(code=="N255") { return "Missing/incomplete/invalid billing provider taxonomy."; }
			else if(code=="N256") { return "Missing/incomplete/invalid billing provider/supplier name."; }
			else if(code=="N257") { return "Missing/incomplete/invalid billing provider/supplier primary identifier."; }
			else if(code=="N258") { return "Missing/incomplete/invalid billing provider/supplier address."; }
			else if(code=="N259") { return "Missing/incomplete/invalid billing provider/supplier secondary identifier."; }
			else if(code=="N260") { return "Missing/incomplete/invalid billing provider/supplier contact information."; }
			else if(code=="N261") { return "Missing/incomplete/invalid operating provider name."; }
			else if(code=="N262") { return "Missing/incomplete/invalid operating provider primary identifier."; }
			else if(code=="N263") { return "Missing/incomplete/invalid operating provider secondary identifier."; }
			else if(code=="N264") { return "Missing/incomplete/invalid ordering provider name."; }
			else if(code=="N265") { return "Missing/incomplete/invalid ordering provider primary identifier."; }
			else if(code=="N266") { return "Missing/incomplete/invalid ordering provider address."; }
			else if(code=="N267") { return "Missing/incomplete/invalid ordering provider secondary identifier."; }
			else if(code=="N268") { return "Missing/incomplete/invalid ordering provider contact information."; }
			else if(code=="N269") { return "Missing/incomplete/invalid other provider name."; }
			else if(code=="N270") { return "Missing/incomplete/invalid other provider primary identifier."; }
			else if(code=="N271") { return "Missing/incomplete/invalid other provider secondary identifier."; }
			else if(code=="N272") { return "Missing/incomplete/invalid other payer attending provider identifier."; }
			else if(code=="N273") { return "Missing/incomplete/invalid other payer operating provider identifier."; }
			else if(code=="N274") { return "Missing/incomplete/invalid other payer other provider identifier."; }
			else if(code=="N275") { return "Missing/incomplete/invalid other payer purchased service provider identifier."; }
			else if(code=="N276") { return "Missing/incomplete/invalid other payer referring provider identifier."; }
			else if(code=="N277") { return "Missing/incomplete/invalid other payer rendering provider identifier."; }
			else if(code=="N278") { return "Missing/incomplete/invalid other payer service facility provider identifier."; }
			else if(code=="N279") { return "Missing/incomplete/invalid pay-to provider name."; }
			else if(code=="N280") { return "Missing/incomplete/invalid pay-to provider primary identifier."; }
			else if(code=="N281") { return "Missing/incomplete/invalid pay-to provider address."; }
			else if(code=="N282") { return "Missing/incomplete/invalid pay-to provider secondary identifier."; }
			else if(code=="N283") { return "Missing/incomplete/invalid purchased service provider identifier."; }
			else if(code=="N284") { return "Missing/incomplete/invalid referring provider taxonomy."; }
			else if(code=="N285") { return "Missing/incomplete/invalid referring provider name."; }
			else if(code=="N286") { return "Missing/incomplete/invalid referring provider primary identifier."; }
			else if(code=="N287") { return "Missing/incomplete/invalid referring provider secondary identifier."; }
			else if(code=="N288") { return "Missing/incomplete/invalid rendering provider taxonomy."; }
			else if(code=="N289") { return "Missing/incomplete/invalid rendering provider name."; }
			else if(code=="N290") { return "Missing/incomplete/invalid rendering provider primary identifier."; }
			else if(code=="N291") { return "Missing/incomplete/invalid rendering provider secondary identifier."; }
			else if(code=="N292") { return "Missing/incomplete/invalid service facility name."; }
			else if(code=="N293") { return "Missing/incomplete/invalid service facility primary identifier."; }
			else if(code=="N294") { return "Missing/incomplete/invalid service facility primary address."; }
			else if(code=="N295") { return "Missing/incomplete/invalid service facility secondary identifier."; }
			else if(code=="N296") { return "Missing/incomplete/invalid supervising provider name."; }
			else if(code=="N297") { return "Missing/incomplete/invalid supervising provider primary identifier."; }
			else if(code=="N298") { return "Missing/incomplete/invalid supervising provider secondary identifier."; }
			else if(code=="N299") { return "Missing/incomplete/invalid occurrence date(s)."; }
			else if(code=="N300") { return "Missing/incomplete/invalid occurrence span date(s)."; }
			else if(code=="N301") { return "Missing/incomplete/invalid procedure date(s)."; }
			else if(code=="N302") { return "Missing/incomplete/invalid other procedure date(s)."; }
			else if(code=="N303") { return "Missing/incomplete/invalid principal procedure date."; }
			else if(code=="N304") { return "Missing/incomplete/invalid dispensed date."; }
			else if(code=="N305") { return "Missing/incomplete/invalid accident date."; }
			else if(code=="N306") { return "Missing/incomplete/invalid acute manifestation date."; }
			else if(code=="N307") { return "Missing/incomplete/invalid adjudication or payment date."; }
			else if(code=="N308") { return "Missing/incomplete/invalid appliance placement date."; }
			else if(code=="N309") { return "Missing/incomplete/invalid assessment date."; }
			else if(code=="N310") { return "Missing/incomplete/invalid assumed or relinquished care date."; }
			else if(code=="N311") { return "Missing/incomplete/invalid authorized to return to work date."; }
			else if(code=="N312") { return "Missing/incomplete/invalid begin therapy date."; }
			else if(code=="N313") { return "Missing/incomplete/invalid certification revision date."; }
			else if(code=="N314") { return "Missing/incomplete/invalid diagnosis date."; }
			else if(code=="N315") { return "Missing/incomplete/invalid disability from date."; }
			else if(code=="N316") { return "Missing/incomplete/invalid disability to date."; }
			else if(code=="N317") { return "Missing/incomplete/invalid discharge hour."; }
			else if(code=="N318") { return "Missing/incomplete/invalid discharge or end of care date."; }
			else if(code=="N319") { return "Missing/incomplete/invalid hearing or vision prescription date."; }
			else if(code=="N320") { return "Missing/incomplete/invalid Home Health Certification Period."; }
			else if(code=="N321") { return "Missing/incomplete/invalid last admission period."; }
			else if(code=="N322") { return "Missing/incomplete/invalid last certification date."; }
			else if(code=="N323") { return "Missing/incomplete/invalid last contact date."; }
			else if(code=="N324") { return "Missing/incomplete/invalid last seen/visit date."; }
			else if(code=="N325") { return "Missing/incomplete/invalid last worked date."; }
			else if(code=="N326") { return "Missing/incomplete/invalid last x-ray date."; }
			else if(code=="N327") { return "Missing/incomplete/invalid other insured birth date."; }
			else if(code=="N328") { return "Missing/incomplete/invalid Oxygen Saturation Test date."; }
			else if(code=="N329") { return "Missing/incomplete/invalid patient birth date."; }
			else if(code=="N330") { return "Missing/incomplete/invalid patient death date."; }
			else if(code=="N331") { return "Missing/incomplete/invalid physician order date."; }
			else if(code=="N332") { return "Missing/incomplete/invalid prior hospital discharge date."; }
			else if(code=="N333") { return "Missing/incomplete/invalid prior placement date."; }
			else if(code=="N334") { return "Missing/incomplete/invalid re-evaluation date"; }
			else if(code=="N335") { return "Missing/incomplete/invalid referral date."; }
			else if(code=="N336") { return "Missing/incomplete/invalid replacement date."; }
			else if(code=="N337") { return "Missing/incomplete/invalid secondary diagnosis date."; }
			else if(code=="N338") { return "Missing/incomplete/invalid shipped date."; }
			else if(code=="N339") { return "Missing/incomplete/invalid similar illness or symptom date."; }
			else if(code=="N340") { return "Missing/incomplete/invalid subscriber birth date."; }
			else if(code=="N341") { return "Missing/incomplete/invalid surgery date."; }
			else if(code=="N342") { return "Missing/incomplete/invalid test performed date."; }
			else if(code=="N343") { return "Missing/incomplete/invalid Transcutaneous Electrical Nerve Stimulator (TENS) trial start date."; }
			else if(code=="N344") { return "Missing/incomplete/invalid Transcutaneous Electrical Nerve Stimulator (TENS) trial end date."; }
			else if(code=="N345") { return "Date range not valid with units submitted."; }
			else if(code=="N346") { return "Missing/incomplete/invalid oral cavity designation code."; }
			else if(code=="N347") { return "Your claim for a referred or purchased service cannot be paid because payment has already been made for this same service to another provider by a payment contractor representing the payer."; }
			else if(code=="N348") { return "You chose that this service/supply/drug would be rendered/supplied and billed by a different practitioner/supplier."; }
			else if(code=="N349") { return "The administration method and drug must be reported to adjudicate this service."; }
			else if(code=="N350") { return "Missing/incomplete/invalid description of service for a Not Otherwise Classified (NOC) code or for an Unlisted/By Report procedure."; }
			else if(code=="N351") { return "Service date outside of the approved treatment plan service dates."; }
			else if(code=="N352") { return "Alert: There are no scheduled payments for this service. Submit a claim for each patient visit."; }
			else if(code=="N353") { return "Alert: Benefits have been estimated, when the actual services have been rendered, additional payment will be considered based on the submitted claim."; }
			else if(code=="N354") { return "Incomplete/invalid invoice"; }
			else if(code=="N355") { return "Alert: The law permits exceptions to the refund requirement in two cases: - If you did not know, and could not have reasonably been expected to know, that we would not pay for this service; or - If you notified the patient in writing before providing the service that you believed that we were likely to deny the service, and the patient signed a statement agreeing to pay for the service. If you come within either exception, or if you believe the carrier was wrong in its determination that we do not pay for this service, you should request appeal of this determination within 30 days of the date of this notice. Your request for review should include any additional information necessary to support your position. If you request an appeal within 30 days of receiving this notice, you may delay refunding the amount to the patient until you receive the results of the review. If the review decision is favorable to you, you do not need to make any refund. If, however, the review is unfavorable, the law specifies that you must make the refund within 15 days of receiving the unfavorable review decision. The law also permits you to request an appeal at any time within 120 days of the date you receive this notice. However, an appeal request that is received more than 30 days after the date of this notice, does not permit you to delay making the refund. Regardless of when a review is requested, the patient will be notified that you have requested one, and will receive a copy of the determination. The patient has received a separate notice of this denial decision. The notice advises that he/she may be entitled to a refund of any amounts paid, if you should have known that we would not pay and did not tell him/her. It also instructs the patient to contact our office if he/she does not hear anything about a refund within 30 days"; }
			else if(code=="N356") { return "Not covered when performed with, or subsequent to, a non-covered service."; }
			else if(code=="N357") { return "Time frame requirements between this service/procedure/supply and a related service/procedure/supply have not been met."; }
			else if(code=="N358") { return "Alert: This decision may be reviewed if additional documentation as described in the contract or plan benefit documents is submitted."; }
			else if(code=="N359") { return "Missing/incomplete/invalid height."; }
			else if(code=="N360") { return "Alert: Coordination of benefits has not been calculated when estimating benefits for this pre-determination. Submit payment information from the primary payer with the secondary claim."; }
			else if(code=="N361") { return "Payment adjusted based on multiple diagnostic imaging procedure rules"; }
			else if(code=="N362") { return "The number of Days or Units of Service exceeds our acceptable maximum."; }
			else if(code=="N363") { return "Alert: in the near future we are implementing new policies/procedures that would affect this determination."; }
			else if(code=="N364") { return "Alert: According to our agreement, you must waive the deductible and/or coinsurance amounts."; }
			else if(code=="N365") { return "This procedure code is not payable. It is for reporting/information purposes only."; }
			else if(code=="N366") { return "Requested information not provided. The claim will be reopened if the information previously requested is submitted within one year after the date of this denial notice."; }
			else if(code=="N367") { return "Alert: The claim information has been forwarded to a Consumer Spending Account processor for review; for example, flexible spending account or health savings account."; }
			else if(code=="N368") { return "You must appeal the determination of the previously adjudicated claim."; }
			else if(code=="N369") { return "Alert: Although this claim has been processed, it is deficient according to state legislation/regulation."; }
			else if(code=="N370") { return "Billing exceeds the rental months covered/approved by the payer."; }
			else if(code=="N371") { return "Alert: title of this equipment must be transferred to the patient."; }
			else if(code=="N372") { return "Only reasonable and necessary maintenance/service charges are covered."; }
			else if(code=="N373") { return "It has been determined that another payer paid the services as primary when they were not the primary payer. Therefore, we are refunding to the payer that paid as primary on your behalf."; }
			else if(code=="N374") { return "Primary Medicare Part A insurance has been exhausted and a Part B Remittance Advice is required."; }
			else if(code=="N375") { return "Missing/incomplete/invalid questionnaire/information required to determine dependent eligibility."; }
			else if(code=="N376") { return "Subscriber/patient is assigned to active military duty, therefore primary coverage may be TRICARE."; }
			else if(code=="N377") { return "Payment based on a processed replacement claim."; }
			else if(code=="N378") { return "Missing/incomplete/invalid prescription quantity."; }
			else if(code=="N379") { return "Claim level information does not match line level information."; }
			else if(code=="N380") { return "The original claim has been processed, submit a corrected claim."; }
			else if(code=="N381") { return "Consult our contractual agreement for restrictions/billing/payment information related to these charges."; }
			else if(code=="N382") { return "Missing/incomplete/invalid patient identifier."; }
			else if(code=="N383") { return "Not covered when deemed cosmetic."; }
			else if(code=="N384") { return "Records indicate that the referenced body part/tooth has been removed in a previous procedure."; }
			else if(code=="N385") { return "Notification of admission was not timely according to published plan procedures."; }
			else if(code=="N386") { return "This decision was based on a National Coverage Determination (NCD). An NCD provides a coverage determination as to whether a particular item or service is covered. A copy of this policy is available at www.cms.gov/mcd/search.asp. If you do not have web access, you may contact the contractor to request a copy of the NCD."; }
			else if(code=="N387") { return "Alert: Submit this claim to the patient's other insurer for potential payment of supplemental benefits. We did not forward the claim information."; }
			else if(code=="N388") { return "Missing/incomplete/invalid prescription number"; }
			else if(code=="N389") { return "Duplicate prescription number submitted."; }
			else if(code=="N390") { return "This service/report cannot be billed separately."; }
			else if(code=="N391") { return "Missing emergency department records."; }
			else if(code=="N392") { return "Incomplete/invalid emergency department records."; }
			else if(code=="N393") { return "Missing progress notes/report."; }
			else if(code=="N394") { return "Incomplete/invalid progress notes/report."; }
			else if(code=="N395") { return "Missing laboratory report."; }
			else if(code=="N396") { return "Incomplete/invalid laboratory report."; }
			else if(code=="N397") { return "Benefits are not available for incomplete service(s)/undelivered item(s)."; }
			else if(code=="N398") { return "Missing elective consent form."; }
			else if(code=="N399") { return "Incomplete/invalid elective consent form."; }
			else if(code=="N400") { return "Alert: Electronically enabled providers should submit claims electronically."; }
			else if(code=="N401") { return "Missing periodontal charting."; }
			else if(code=="N402") { return "Incomplete/invalid periodontal charting."; }
			else if(code=="N403") { return "Missing facility certification."; }
			else if(code=="N404") { return "Incomplete/invalid facility certification."; }
			else if(code=="N405") { return "This service is only covered when the donor's insurer(s) do not provide coverage for the service."; }
			else if(code=="N406") { return "This service is only covered when the recipient's insurer(s) do not provide coverage for the service."; }
			else if(code=="N407") { return "You are not an approved submitter for this transmission format."; }
			else if(code=="N408") { return "This payer does not cover deductibles assessed by a previous payer."; }
			else if(code=="N409") { return "This service is related to an accidental injury and is not covered unless provided within a specific time frame from the date of the accident."; }
			else if(code=="N410") { return "Not covered unless the prescription changes."; }
			else if(code=="N411") { return "This service is allowed one time in a 6-month period. (This temporary code will be deactivated on 2/1/09. Must be used with Reason Code 119.)"; }
			else if(code=="N412") { return "This service is allowed 2 times in a 12-month period. (This temporary code will be deactivated on 2/1/09. Must be used with Reason Code 119.)"; }
			else if(code=="N413") { return "This service is allowed 2 times in a benefit year. (This temporary code will be deactivated on 2/1/09. Must be used with Reason Code 119.)"; }
			else if(code=="N414") { return "This service is allowed 4 times in a 12-month period. (This temporary code will be deactivated on 2/1/09. Must be used with Reason Code 119.)"; }
			else if(code=="N415") { return "This service is allowed 1 time in an 18-month period. (This temporary code will be deactivated on 2/1/09. Must be used with Reason Code 119.)"; }
			else if(code=="N416") { return "This service is allowed 1 time in a 3-year period. (This temporary code will be deactivated on 2/1/09. Must be used with Reason Code 119.)"; }
			else if(code=="N417") { return "This service is allowed 1 time in a 5-year period. (This temporary code will be deactivated on 2/1/09. Must be used with Reason Code 119.)"; }
			else if(code=="N418") { return "Misrouted claim. See the payer's claim submission instructions."; }
			else if(code=="N419") { return "Claim payment was the result of a payer's retroactive adjustment due to a retroactive rate change."; }
			else if(code=="N420") { return "Claim payment was the result of a payer's retroactive adjustment due to a Coordination of Benefits or Third Party Liability Recovery."; }
			else if(code=="N421") { return "Claim payment was the result of a payer's retroactive adjustment due to a review organization decision."; }
			else if(code=="N422") { return "Claim payment was the result of a payer's retroactive adjustment due to a payer's contract incentive program."; }
			else if(code=="N423") { return "Claim payment was the result of a payer's retroactive adjustment due to a non standard program."; }
			else if(code=="N424") { return "Patient does not reside in the geographic area required for this type of payment."; }
			else if(code=="N425") { return "Statutorily excluded service(s)."; }
			else if(code=="N426") { return "No coverage when self-administered."; }
			else if(code=="N427") { return "Payment for eyeglasses or contact lenses can be made only after cataract surgery."; }
			else if(code=="N428") { return "Not covered when performed in this place of service."; }
			else if(code=="N429") { return "Not covered when considered routine."; }
			else if(code=="N430") { return "Procedure code is inconsistent with the units billed."; }
			else if(code=="N431") { return "Not covered with this procedure."; }
			else if(code=="N432") { return "Adjustment based on a Recovery Audit."; }
			else if(code=="N433") { return "Resubmit this claim using only your National Provider Identifier (NPI)"; }
			else if(code=="N434") { return "Missing/Incomplete/Invalid Present on Admission indicator."; }
			else if(code=="N435") { return "Exceeds number/frequency approved /allowed within time period without support documentation."; }
			else if(code=="N436") { return "The injury claim has not been accepted and a mandatory medical reimbursement has been made."; }
			else if(code=="N437") { return "Alert: If the injury claim is accepted, these charges will be reconsidered."; }
			else if(code=="N438") { return "This jurisdiction only accepts paper claims"; }
			else if(code=="N439") { return "Missing anesthesia physical status report/indicators."; }
			else if(code=="N440") { return "Incomplete/invalid anesthesia physical status report/indicators."; }
			else if(code=="N441") { return "This missed/cancelled appointment is not covered."; }
			else if(code=="N442") { return "Payment based on an alternate fee schedule."; }
			else if(code=="N443") { return "Missing/incomplete/invalid total time or begin/end time."; }
			else if(code=="N444") { return "Alert: This facility has not filed the Election for High Cost Outlier form with the Division of Workers' Compensation."; }
			else if(code=="N445") { return "Missing document for actual cost or paid amount."; }
			else if(code=="N446") { return "Incomplete/invalid document for actual cost or paid amount."; }
			else if(code=="N447") { return "Payment is based on a generic equivalent as required documentation was not provided."; }
			else if(code=="N448") { return "This drug/service/supply is not included in the fee schedule or contracted/legislated fee arrangement"; }
			else if(code=="N449") { return "Payment based on a comparable drug/service/supply."; }
			else if(code=="N450") { return "Covered only when performed by the primary treating physician or the designee."; }
			else if(code=="N451") { return "Missing Admission Summary Report."; }
			else if(code=="N452") { return "Incomplete/invalid Admission Summary Report."; }
			else if(code=="N453") { return "Missing Consultation Report."; }
			else if(code=="N454") { return "Incomplete/invalid Consultation Report."; }
			else if(code=="N455") { return "Missing Physician Order."; }
			else if(code=="N456") { return "Incomplete/invalid Physician Order."; }
			else if(code=="N457") { return "Missing Diagnostic Report."; }
			else if(code=="N458") { return "Incomplete/invalid Diagnostic Report."; }
			else if(code=="N459") { return "Missing Discharge Summary."; }
			else if(code=="N460") { return "Incomplete/invalid Discharge Summary."; }
			else if(code=="N461") { return "Missing Nursing Notes."; }
			else if(code=="N462") { return "Incomplete/invalid Nursing Notes."; }
			else if(code=="N463") { return "Missing support data for claim."; }
			else if(code=="N464") { return "Incomplete/invalid support data for claim."; }
			else if(code=="N465") { return "Missing Physical Therapy Notes/Report."; }
			else if(code=="N466") { return "Incomplete/invalid Physical Therapy Notes/Report."; }
			else if(code=="N467") { return "Missing Report of Tests and Analysis Report."; }
			else if(code=="N468") { return "Incomplete/invalid Report of Tests and Analysis Report."; }
			else if(code=="N469") { return "Alert: Claim/Service(s) subject to appeal process, see section 935 of Medicare Prescription Drug, Improvement, and Modernization Act of 2003 (MMA)."; }
			else if(code=="N470") { return "This payment will complete the mandatory medical reimbursement limit."; }
			else if(code=="N471") { return "Missing/incomplete/invalid HIPPS Rate Code."; }
			else if(code=="N472") { return "Payment for this service has been issued to another provider."; }
			else if(code=="N473") { return "Missing certification."; }
			else if(code=="N474") { return "Incomplete/invalid certification"; }
			else if(code=="N475") { return "Missing completed referral form."; }
			else if(code=="N476") { return "Incomplete/invalid completed referral form"; }
			else if(code=="N477") { return "Missing Dental Models."; }
			else if(code=="N478") { return "Incomplete/invalid Dental Models"; }
			else if(code=="N479") { return "Missing Explanation of Benefits (Coordination of Benefits or Medicare Secondary Payer)."; }
			else if(code=="N480") { return "Incomplete/invalid Explanation of Benefits (Coordination of Benefits or Medicare Secondary Payer)."; }
			else if(code=="N481") { return "Missing Models."; }
			else if(code=="N482") { return "Incomplete/invalid Models"; }
			else if(code=="N483") { return "Missing Periodontal Charts."; }
			else if(code=="N484") { return "Incomplete/invalid Periodontal Charts"; }
			else if(code=="N485") { return "Missing Physical Therapy Certification."; }
			else if(code=="N486") { return "Incomplete/invalid Physical Therapy Certification."; }
			else if(code=="N487") { return "Missing Prosthetics or Orthotics Certification."; }
			else if(code=="N488") { return "Incomplete/invalid Prosthetics or Orthotics Certification"; }
			else if(code=="N489") { return "Missing referral form."; }
			else if(code=="N490") { return "Incomplete/invalid referral form"; }
			else if(code=="N491") { return "Missing/Incomplete/Invalid Exclusionary Rider Condition."; }
			else if(code=="N492") { return "Alert: A network provider may bill the member for this service if the member requested the service and agreed in writing, prior to receiving the service, to be financially responsible for the billed charge."; }
			else if(code=="N493") { return "Missing Doctor First Report of Injury."; }
			else if(code=="N494") { return "Incomplete/invalid Doctor First Report of Injury."; }
			else if(code=="N495") { return "Missing Supplemental Medical Report."; }
			else if(code=="N496") { return "Incomplete/invalid Supplemental Medical Report."; }
			else if(code=="N497") { return "Missing Medical Permanent Impairment or Disability Report."; }
			else if(code=="N498") { return "Incomplete/invalid Medical Permanent Impairment or Disability Report."; }
			else if(code=="N499") { return "Missing Medical Legal Report."; }
			else if(code=="N500") { return "Incomplete/invalid Medical Legal Report."; }
			else if(code=="N501") { return "Missing Vocational Report."; }
			else if(code=="N502") { return "Incomplete/invalid Vocational Report."; }
			else if(code=="N503") { return "Missing Work Status Report."; }
			else if(code=="N504") { return "Incomplete/invalid Work Status Report."; }
			else if(code=="N505") { return "Alert: This response includes only services that could be estimated in real time. No estimate will be provided for the services that could not be estimated in real time."; }
			else if(code=="N506") { return "Alert: This is an estimate of the member’s liability based on the information available at the time the estimate was processed. Actual coverage and member liability amounts will be determined when the claim is processed. This is not a pre-authorization or a guarantee of payment."; }
			else if(code=="N507") { return "Plan distance requirements have not been met."; }
			else if(code=="N508") { return "Alert: This real time claim adjudication response represents the member responsibility to the provider for services reported. The member will receive an Explanation of Benefits electronically or in the mail. Contact the insurer if there are any questions."; }
			else if(code=="N509") { return "Alert: A current inquiry shows the member’s Consumer Spending Account contains sufficient funds to cover the member liability for this claim/service. Actual payment from the Consumer Spending Account will depend on the availability of funds and determination of eligible services at the time of payment processing."; }
			else if(code=="N510") { return "Alert: A current inquiry shows the member’s Consumer Spending Account does not contain sufficient funds to cover the member's liability for this claim/service. Actual payment from the Consumer Spending Account will depend on the availability of funds and determination of eligible services at the time of payment processing."; }
			else if(code=="N511") { return "Alert: Information on the availability of Consumer Spending Account funds to cover the member liability on this claim/service is not available at this time."; }
			else if(code=="N512") { return "Alert: This is the initial remit of a non-NCPDP claim originally submitted real-time without change to the adjudication."; }
			else if(code=="N513") { return "Alert: This is the initial remit of a non-NCPDP claim originally submitted real-time with a change to the adjudication."; }
			else if(code=="N514") { return "Consult plan benefit documents/guidelines for information about restrictions for this service."; }
			else if(code=="N515") { return "Alert: Submit this claim to the patient's other insurer for potential payment of supplemental benefits. We did not forward the claim information. (use N387 instead)"; }
			else if(code=="N516") { return "Records indicate a mismatch between the submitted NPI and EIN."; }
			else if(code=="N517") { return "Resubmit a new claim with the requested information."; }
			else if(code=="N518") { return "No separate payment for accessories when furnished for use with oxygen equipment."; }
			else if(code=="N519") { return "Invalid combination of HCPCS modifiers."; }
			else if(code=="N520") { return "Alert: Payment made from a Consumer Spending Account."; }
			else if(code=="N521") { return "Mismatch between the submitted provider information and the provider information stored in our system."; }
			else if(code=="N522") { return "Duplicate of a claim processed, or to be processed, as a crossover claim."; }
			else if(code=="N523") { return "The limitation on outlier payments defined by this payer for this service period has been met. The outlier payment otherwise applicable to this claim has not been paid."; }
			else if(code=="N524") { return "Based on policy this payment constitutes payment in full."; }
			else if(code=="N525") { return "These services are not covered when performed within the global period of another service."; }
			else if(code=="N526") { return "Not qualified for recovery based on employer size."; }
			else if(code=="N527") { return "We processed this claim as the primary payer prior to receiving the recovery demand."; }
			else if(code=="N528") { return "Patient is entitled to benefits for Institutional Services only."; }
			else if(code=="N529") { return "Patient is entitled to benefits for Professional Services only."; }
			else if(code=="N530") { return "Not Qualified for Recovery based on enrollment information."; }
			else if(code=="N531") { return "Not qualified for recovery based on direct payment of premium."; }
			else if(code=="N532") { return "Not qualified for recovery based on disability and working status."; }
			else if(code=="N533") { return "Services performed in an Indian Health Services facility under a self-insured tribal Group Health Plan."; }
			else if(code=="N534") { return "This is an individual policy, the employer does not participate in plan sponsorship."; }
			else if(code=="N535") { return "Payment is adjusted when procedure is performed in this place of service based on the submitted procedure code and place of service."; }
			else if(code=="N536") { return "We are not changing the prior payer's determination of patient portion, which you may collect, as this service is not covered by us."; }
			else if(code=="N537") { return "We have examined claims history and no records of the services have been found."; }
			else if(code=="N538") { return "A facility is responsible for payment to outside providers who furnish these services/supplies/drugs to its patients/residents."; }
			else if(code=="N539") { return "Alert: We processed appeals/waiver requests on your behalf and that request has been denied."; }
			else if(code=="N540") { return "Payment adjusted based on the interrupted stay policy."; }
			else if(code=="N541") { return "Mismatch between the submitted insurance type code and the information stored in our system."; }
			else if(code=="N542") { return "Missing income verification."; }
			else if(code=="N543") { return "Incomplete/invalid income verification"; }
			else if(code=="N544") { return "Alert: Although this was paid, you have billed with a referring/ordering provider that does not match our system record. Unless, corrected, this will not be paid in the future."; }
			else if(code=="N545") { return "Payment reduced based on status as an unsuccessful eprescriber per the Electronic Prescribing (eRx) Incentive Program."; }
			else if(code=="N546") { return "Payment represents a previous reduction based on the Electronic Prescribing (eRx) Incentive Program."; }
			else if(code=="N547") { return "A refund request (Frequency Type Code 8) was processed previously."; }
			else if(code=="N548") { return "Alert: Patient's calendar year deductible has been met."; }
			else if(code=="N549") { return "Alert: Patient's calendar year out-of-pocket maximum has been met."; }
			else if(code=="N550") { return "Alert: You have not responded to requests to revalidate your provider/supplier enrollment information. Your failure to revalidate your enrollment information will result in a payment hold in the near future."; }
			else if(code=="N551") { return "Payment adjusted based on the Ambulatory Surgical Center (ASC) Quality Reporting Program."; }
			else if(code=="N552") { return "Payment adjusted to reverse a previous withhold/bonus amount."; }
			else if(code=="N553") { return "Payment adjusted based on a Low Income Subsidy (LIS) retroactive coverage or status change."; }
			else if(code=="N554") { return "Missing/Incomplete/Invalid Family Planning Indicator"; }
			else if(code=="N555") { return "Missing medication list."; }
			else if(code=="N556") { return "Incomplete/invalid medication list."; }
			else if(code=="N557") { return "This claim/service is not payable under our service area. The claim must be filed to the Payer/Plan in whose service area the specimen was collected."; }
			else if(code=="N558") { return "This claim/service is not payable under our service area. The claim must be filed to the Payer/Plan in whose service area the equipment was received."; }
			else if(code=="N559") { return "This claim/service is not payable under our service area. The claim must be filed to the Payer/Plan in whose service area the Ordering Physician is located."; }
			else if(code=="N560") { return "The pilot program requires an interim or final claim within 60 days of the Notice of Admission. A claim was not received."; }
			else if(code=="N561") { return "The bundled claim originally submitted for this episode of care includes related readmissions. You may resubmit the original claim to receive a corrected payment based on this readmission."; }
			else if(code=="N562") { return "The provider number of your incoming claim does not match the provider number on the processed Notice of Admission (NOA) for this bundled payment."; }
			else if(code=="N563") { return "Missing required provider/supplier issuance of advance patient notice of non-coverage. The patient is not liable for payment for this service."; }
			else if(code=="N564") { return "Patient did not meet the inclusion criteria for the demonstration project or pilot program."; }
			else if(code=="N565") { return "Alert: This non-payable reporting code requires a modifier. Future claims containing this non-payable reporting code must include an appropriate modifier for the claim to be processed."; }
			else if(code=="N566") { return "Alert: This procedure code requires functional reporting. Future claims containing this procedure code must include an applicable non-payable code and appropriate modifiers for the claim to be processed."; }
			else if(code=="N567") { return "Not covered when considered preventative."; }
			else if(code=="N568") { return "Alert: Initial payment based on the Notice of Admission (NOA) under the Bundled Payment Model IV initiative."; }
			else if(code=="N569") { return "Not covered when performed for the reported diagnosis."; }
			else if(code=="N570") { return "Missing/incomplete/invalid credentialing data"; }
			else if(code=="N571") { return "Alert: Payment will be issued quarterly by another payer/contractor."; }
			else if(code=="N572") { return "This procedure is not payable unless non-payable reporting codes and appropriate modifiers are submitted."; }
			else if(code=="N573") { return "Alert: You have been overpaid and must refund the overpayment. The refund will be requested separately by another payer/contractor."; }
			else if(code=="N574") { return "Our records indicate the ordering/referring provider is of a type/specialty that cannot order or refer. Please verify that the claim ordering/referring provider information is accurate or contact the ordering/referring provider."; }
			else if(code=="N575") { return "Mismatch between the submitted ordering/referring provider name and the ordering/referring provider name stored in our records."; }
			else if(code=="N576") { return "Services not related to the specific incident/claim/accident/loss being reported."; }
			else if(code=="N577") { return "Personal Injury Protection (PIP) Coverage."; }
			else if(code=="N578") { return "Coverages do not apply to this loss."; }
			else if(code=="N579") { return "Medical Payments Coverage (MPC)."; }
			else if(code=="N580") { return "Determination based on the provisions of the insurance policy."; }
			else if(code=="N581") { return "Investigation of coverage eligibility is pending."; }
			else if(code=="N582") { return "Benefits suspended pending the patient's cooperation."; }
			else if(code=="N583") { return "Patient was not an occupant of our insured vehicle and therefore, is not an eligible injured person."; }
			else if(code=="N584") { return "Not covered based on the insured's noncompliance with policy or statutory conditions."; }
			else if(code=="N585") { return "Benefits are no longer available based on a final injury settlement."; }
			else if(code=="N586") { return "The injured party does not qualify for benefits."; }
			else if(code=="N587") { return "Policy benefits have been exhausted."; }
			else if(code=="N588") { return "The patient has instructed that medical claims/bills are not to be paid."; }
			else if(code=="N589") { return "Coverage is excluded to any person injured as a result of operating a motor vehicle while in an intoxicated condition or while the ability to operate such a vehicle is impaired by the use of a drug."; }
			else if(code=="N590") { return "Missing independent medical exam detailing the cause of injuries sustained and medical necessity of services rendered."; }
			else if(code=="N591") { return "Payment based on an Independent Medical Examination (IME) or Utilization Review (UR)."; }
			else if(code=="N592") { return "Adjusted because this is not the initial prescription or exceeds the amount allowed for the initial prescription."; }
			else if(code=="N593") { return "Not covered based on failure to attend a scheduled Independent Medical Exam (IME)."; }
			else if(code=="N594") { return "Records reflect the injured party did not complete an Application for Benefits for this loss."; }
			else if(code=="N595") { return "Records reflect the injured party did not complete an Assignment of Benefits for this loss."; }
			else if(code=="N596") { return "Records reflect the injured party did not complete a Medical Authorization for this loss."; }
			else if(code=="N597") { return "Adjusted based on a medical/dental provider's apportionment of care between related injuries and other unrelated medical/dental conditions/injuries."; }
			else if(code=="N598") { return "Health care policy coverage is primary."; }
			else if(code=="N599") { return "Our payment for this service is based upon a reasonable amount pursuant to both the terms and conditions of the policy of insurance under which the subject claim is being made as well as the Florida No-Fault Statute, which permits, when determining a reasonable charge for a service, an insurer to consider usual and customary charges and payments accepted by the provider, reimbursement levels in the community and various federal and state fee schedules applicable to automobile and other insurance coverages, and other information relevant to the reasonableness of the reimbursement for the service. The payment for this service is based upon 200% of the Participating Level of Medicare Part B fee schedule for the locale in which the services were rendered."; }
			else if(code=="N600") { return "Adjusted based on the applicable fee schedule for the region in which the service was rendered."; }
			else if(code=="N601") { return "In accordance with Hawaii Administrative Rules, Title 16, Chapter 23 Motor Vehicle Insurance Law payment is recommended based on Medicare Resource Based Relative Value Scale System applicable to Hawaii."; }
			else if(code=="N602") { return "Adjusted based on the Redbook maximum allowance."; }
			else if(code=="N603") { return "This fee is calculated according to the New Jersey medical fee schedules for Automobile Personal Injury Protection and Motor Bus Medical Expense Insurance Coverage."; }
			else if(code=="N604") { return "In accordance with New York No-Fault Law, Regulation 68, this base fee was calculated according to the New York Workers' Compensation Board Schedule of Medical Fees, pursuant to Regulation 83 and / or Appendix 17-C of 11 NYCRR."; }
			else if(code=="N605") { return "This fee was calculated based upon New York All Patients Refined Diagnosis Related Groups (APR-DRG), pursuant to Regulation 68."; }
			else if(code=="N606") { return "The Oregon allowed amount for this procedure is based upon the Workers Compensation Fee Schedule (OAR 436-009). The allowed amount has been calculated in accordance with Section 4 of ORS 742.524."; }
			else if(code=="N607") { return "Service provided for non-compensable condition(s)."; }
			else if(code=="N608") { return "The fee schedule amount allowed is calculated at 110% of the Medicare Fee Schedule for this region, specialty and type of service. This fee is calculated in compliance with Act 6."; }
			else if(code=="N609") { return "80% of the providers billed amount is being recommended for payment according to Act 6."; }
			else if(code=="N610") { return "Alert: Payment based on an appropriate level of care."; }
			else if(code=="N611") { return "Claim in litigation. Contact insurer for more information."; }
			else if(code=="N612") { return "Medical provider not authorized/certified to provide treatment to injured workers in this jurisdiction."; }
			else if(code=="N613") { return "Alert: Although this was paid, you have billed with an ordering provider that needs to update their enrollment record. Please verify that the ordering provider information you submitted on the claim is accurate and if it is, contact the ordering provider instructing them to update their enrollment record. Unless corrected, a claim with this ordering provider will not be paid in the future."; }
			else if(code=="N614") { return "Alert: Additional information is included in the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information)."; }
			else if(code=="N615") { return "Alert: This enrollee receiving advance payments of the premium tax credit is in the grace period of three consecutive months for non-payment of premium. Under the Code of Federal Regulations, Title 45, Part 156.270, a Qualified Health Plan issuer must pay all appropriate claims for services rendered to the enrollee during the first month of the grace period and may pend claims for services rendered to the enrollee in the second and third months of the grace period."; }
			else if(code=="N616") { return "Alert: This enrollee is in the first month of the advance premium tax credit grace period."; }
			else if(code=="N617") { return "This enrollee is in the second or third month of the advance premium tax credit grace period."; }
			else if(code=="N618") { return "Alert: This claim will automatically be reprocessed if the enrollee pays their premiums."; }
			else if(code=="N619") { return "Coverage terminated for non-payment of premium."; }
			else if(code=="N620") { return "Alert: This procedure code is for quality reporting/informational purposes only."; }
			else if(code=="N621") { return "Charges for Jurisdiction required forms, reports, or chart notes are not payable."; }
			else if(code=="N622") { return "Not covered based on the date of injury/accident."; }
			else if(code=="N623") { return "Not covered when deemed unscientific/unproven/outmoded/experimental/excessive/inappropriate."; }
			else if(code=="N624") { return "The associated Workers' Compensation claim has been withdrawn."; }
			else if(code=="N625") { return "Missing/Incomplete/Invalid Workers' Compensation Claim Number."; }
			else if(code=="N626") { return "New or established patient E/M codes are not payable with chiropractic care codes."; }
			else if(code=="N627") { return "Service not payable per managed care contract."; }
			else if(code=="N628") { return "Out-patient follow up visits on the same date of service as a scheduled test or treatment is disallowed."; }
			else if(code=="N629") { return "Reviews/documentation/notes/summaries/reports/charts not requested."; }
			else if(code=="N630") { return "Referral not authorized by attending physician."; }
			else if(code=="N631") { return "Medical Fee Schedule does not list this code. An allowance was made for a comparable service."; }
			else if(code=="N632") { return "According to the Official Medical Fee Schedule this service has a relative value of zero and therefore no payment is due."; }
			else if(code=="N633") { return "Additional anesthesia time units are not allowed."; }
			else if(code=="N634") { return "The allowance is calculated based on anesthesia time units."; }
			else if(code=="N635") { return "The Allowance is calculated based on the anesthesia base units plus time."; }
			else if(code=="N636") { return "Adjusted because this is reimbursable only once per injury."; }
			else if(code=="N637") { return "Consultations are not allowed once treatment has been rendered by the same provider."; }
			else if(code=="N638") { return "Reimbursement has been made according to the home health fee schedule."; }
			else if(code=="N639") { return "Reimbursement has been made according to the inpatient rehabilitation facilities fee schedule."; }
			else if(code=="N640") { return "Exceeds number/frequency approved/allowed within time period."; }
			else if(code=="N641") { return "Reimbursement has been based on the number of body areas rated."; }
			else if(code=="N642") { return "Adjusted when billed as individual tests instead of as a panel."; }
			else if(code=="N643") { return "The services billed are considered Not Covered or Non-Covered (NC) in the applicable state fee schedule."; }
			else if(code=="N644") { return "Reimbursement has been made according to the bilateral procedure rule."; }
			else if(code=="N645") { return "Mark-up allowance"; }
			else if(code=="N646") { return "Reimbursement has been adjusted based on the guidelines for an assistant."; }
			else if(code=="N647") { return "Adjusted based on diagnosis-related group (DRG)."; }
			else if(code=="N648") { return "Adjusted based on Stop Loss."; }
			else if(code=="N649") { return "Payment based on invoice."; }
			else if(code=="N650") { return "This policy was not in effect for this date of loss. No coverage is available."; }
			else if(code=="N651") { return "No Personal Injury Protection/Medical Payments Coverage on the policy at the time of the loss."; }
			else if(code=="N652") { return "The date of service is before the date of loss."; }
			else if(code=="N653") { return "The date of injury does not match the reported date of loss."; }
			else if(code=="N654") { return "Adjusted based on achievement of maximum medical improvement (MMI)."; }
			else if(code=="N655") { return "Payment based on provider's geographic region."; }
			else if(code=="N656") { return "An interest payment is being made because benefits are being paid outside the statutory requirement."; }
			else if(code=="N657") { return "This should be billed with the appropriate code for these services."; }
			else if(code=="N658") { return "The billed service(s) are not considered medical expenses."; }
			else if(code=="N659") { return "This item is exempt from sales tax."; }
			else if(code=="N660") { return "Sales tax has been included in the reimbursement."; }
			else if(code=="N661") { return "Documentation does not support that the services rendered were medically necessary."; }
			else if(code=="N662") { return "Alert: Consideration of payment will be made upon receipt of a final bill."; }
			else if(code=="N663") { return "Adjusted based on an agreed amount."; }
			else if(code=="N664") { return "Adjusted based on a legal settlement."; }
			else if(code=="N665") { return "Services by an unlicensed provider are not reimbursable."; }
			else if(code=="N666") { return "Only one evaluation and management code at this service level is covered during the course of care."; }
			else if(code=="N667") { return "Missing prescription"; }
			else if(code=="N668") { return "Incomplete/invalid prescription"; }
			else if(code=="N669") { return "Adjusted based on the Medicare fee schedule."; }
			else if(code=="N670") { return "This service code has been identified as the primary procedure code subject to the Medicare Multiple Procedure Payment Reduction (MPPR) rule."; }
			else if(code=="N671") { return "Payment based on a jurisdiction cost-charge ratio."; }
			else if(code=="N672") { return "Alert: Amount applied to Health Insurance Offset."; }
			else if(code=="N673") { return "Reimbursement has been calculated based on an outpatient per diem or an outpatient factor and/or fee schedule amount."; }
			else if(code=="N674") { return "Not covered unless a pre-requisite procedure/service has been provided."; }
			else if(code=="N675") { return "Additional information is required from the injured party."; }
			else if(code=="N676") { return "Service does not qualify for payment under the Outpatient Facility Fee Schedule."; }
			else if(code=="N677") { return "Alert: Films/Images will not be returned."; }
			else if(code=="N678") { return "Missing post-operative images/visual field results."; }
			else if(code=="N679") { return "Incomplete/Invalid post-operative images/visual field results."; }
			else if(code=="N680") { return "Missing/Incomplete/Invalid date of previous dental extractions."; }
			else if(code=="N681") { return "Missing/Incomplete/Invalid full arch series."; }
			else if(code=="N682") { return "Missing/Incomplete/Invalid history of prior periodontal therapy/maintenance."; }
			else if(code=="N683") { return "Missing/Incomplete/Invalid prior treatment documentation."; }
			else if(code=="N684") { return "Payment denied as this is a specialty claim submitted as a general claim."; }
			else if(code=="N685") { return "Missing/Incomplete/Invalid Prosthesis, Crown or Inlay Code."; }
			else if(code=="N686") { return "Missing/incomplete/Invalid questionnaire needed to complete payment determination."; }
			else if(code=="N687") { return "Alert: This reversal is due to a retroactive disenrollment. (Note: To be used with claim/service reversal)"; }
			else if(code=="N688") { return "Alert: This reversal is due to a medical or utilization review decision. (Note: To be used with claim/service reversal)"; }
			else if(code=="N689") { return "Alert: This reversal is due to a retroactive rate change. (Note: To be used with claim/service reversal)"; }
			else if(code=="N690") { return "Alert: This reversal is due to a provider submitted appeal. (Note: To be used with claim/service reversal)"; }
			else if(code=="N691") { return "Alert: This reversal is due to a patient submitted appeal. (Note: To be used with claim/service reversal)"; }
			else if(code=="N692") { return "Alert: This reversal is due to an incorrect rate on the initial adjudication. (Note: To be used with claim/service reversal)"; }
			else if(code=="N693") { return "Alert: This reversal is due to a cancelation of the claim by the provider."; }
			else if(code=="N694") { return "Alert: This reversal is due to a resubmission/change to the claim by the provider."; }
			else if(code=="N695") { return "Alert: This reversal is due to incorrect patient financial portion information on the initial adjudication."; }
			else if(code=="N696") { return "Alert: This reversal is due to a Coordination of Benefits or Third Party Liability Recovery retroactive adjustment. (Note: To be used with claim/service reversal)"; }
			else if(code=="N697") { return "Alert: This reversal is due to a payer's retroactive contract incentive program adjustment. (Note: To be used with claim/service reversal)"; }
			else if(code=="N698") { return "Alert: This reversal is due to non-payment of the Health Insurance Exchange premiums by the end of the premium payment grace period, resulting in loss of coverage. (Note: To be used with claim/service reversal)"; }


			return "Remark code "+code;//catch all.  The user can look up the code manually in this case.
		}

		#endregion Code To Description

	}
}

#region Examples

//Example 1 From 835 Specification:
//ISA*00*          *00*          *ZZ*810624427      *ZZ*113504607      *140217*1450*^*00501*000000001*0*P*:~
//GS*HC*810624427*113504607*20140217*1450*1*X*005010X224A2~
//ST*835*1234~
//BPR*C*150000*C*ACH*CTX*01*999999992*DA*123456*1512345678**01*999988880*DA*98765*20020913~
//TRN*1*12345*1512345678~
//DTM*405*20020916~
//N1*PR*INSURANCE COMPANY OF TIMBUCKTU~
//N3*1 MAIN STREET~
//N4*TIMBUCKTU*AK*89111~
//REF*2U*999~
//N1*PE*REGIONAL HOPE HOSPITAL*XX*6543210903~
//LX*110212~
//TS3*6543210903*11*20021231*1*211366.97****138018.4**73348.57~
//TS2*2178.45*1919.71**56.82*197.69*4.23~
//CLP*666123*1*211366.97*138018.4**MA*1999999444444*11*1~
//CAS*CO*45*73348.57~
//NM1*QC*1*JONES*SAM*O***HN*666666666A~
//MIA*0***138018.4~
//DTM*232*20020816~
//DTM*233*20020824~
//QTY*CA*8~
//LX*130212~
//TS3*6543210909*13*19961231*1*15000****11980.33**3019.67~
//CLP*777777*1*15000*11980.33**MB*1999999444445*13*1~
//CAS*CO*45*3019.67~
//NM1*QC*1*BORDER*LIZ*E***HN*996669999B~
//MOA***MA02~
//DTM*232*20020512~
//PLB*6543210903*20021231*CV:CP*-1.27~
//SE*28*1234~
//GE*1*1~
//IEA*1*000000001~

//Example 2 From 835 Specification (modified to include a procedure line item control number in REF*6R, procedure supplemental info in AMT, and procedure remarks in LQ):
//ISA*00*          *00*          *ZZ*810624427      *ZZ*113504607      *140217*1450*^*00501*000000001*0*P*:~
//GS*HC*810624427*113504607*20140217*1450*1*X*005010X224A2~
//ST*835*12233~
//BPR*I*945*C*ACH*CCP*01*888999777*DA*24681012*1935665544**01*111333555*DA*144444*20020316~
//TRN*1*71700666555*1935665544~
//DTM*405*20020314~
//N1*PR*RUSHMORE LIFE~
//N3*10 SOUTH AVENUE~
//N4*RAPID CITY*SD*55111~
//N1*PE*ACME MEDICAL CENTER*XX*5544667733~
//REF*TJ*777667755~
//LX*1~
//CLP*55545554444*1*800*450*300*12*94060555410000~
//CAS*CO*A2*50~
//NM1*QC*1*BUDD*WILLIAM****MI*33344555510~
//SVC*HC:99211*800*500~
//DTM*150*20020301~
//DTM*151*20020304~
//CAS*PR*1*300~
//AMT*T*10~
//LQ*HE*M16~
//CLP*8765432112*1*1200*495*600*12*9407779923000~
//CAS*CO*A2*55~
//NM1*QC*1*SETTLE*SUSAN****MI*44455666610~
//SVC*HC:93555*1200*550~
//DTM*150*20020310~
//DTM*151*20020312~
//CAS*PR*1*600~
//CAS*CO*45*50~
//REF*6R*p1000~
//SE*25*112233~
//GE*1*1~
//IEA*1*000000001~

//Example 3 From 835 Specification (modified to include a procedure line item control number in REF*6R):
//ISA*00*          *00*          *ZZ*810624427      *ZZ*113504607      *140217*1450*^*00501*000000001*0*P*:~
//GS*HC*810624427*113504607*20140217*1450*1*X*005010X224A2~
//ST*835*0001~
//BPR*I*1222*C*CHK************20050412~
//TRN*1*0012524965*1559123456~
//REF*EV*030240928~
//DTM*405*20050412~
//N1*PR*YOUR TAX DOLLARS AT WORK~
//N3*481A00 DEER RUN ROAD~
//N4*WEST PALM BCH*FL*11114~
//N1*PE*ACME MEDICAL CENTER*FI*5999944521~
//N3*PO BOX 863382~
//N4*ORLANDO*FL*55115~
//REF*PQ*10488~
//LX*1~
//CLP*L0004828311*2*10323.64*912**12*05090256390*11*1~
//CAS*OA*23*9411.64~
//NM1*QC*1*TOWNSEND*WILLIAM*P***MI*XXX123456789~
//NM1*82*2*ACME MEDICAL CENTER*****BD*987~
//DTM*232*20050303~
//DTM*233*20050304~
//AMT*AU*912~
//LX*2~
//CLP*0001000053*2*751.50*310*220*12*50630626430~
//NM1*QC*1*BAKI*ANGI****MI*456789123~
//NM1*82*2*SMITH JONES PA*****BS*34426~
//DTM*232*20050106~
//DTM*233*20050106~
//SVC*HC>12345>26*166.5*30**1~
//DTM*472*20050106~
//CAS*OA*23*136.50~
//REF*6R*p1001~
//REF*1B*43285~
//AMT*AU*150~
//SVC*HC>66543>26*585*280*220*1~
//DTM*472*20050106~
//CAS*PR*1*150**2*70~
//CAS*CO*42*85~
//REF*6R*123456~
//REF*1B*43285~
//AMT*AU*500~
//SE*38*0001~
//GE*1*1~
//IEA*1*000000001~

//Example 4 From 835 Specification:
//ISA*00*          *00*          *ZZ*810624427      *ZZ*113504607      *140217*1450*^*00501*000000001*0*P*:~
//GS*HC*810624427*113504607*20140217*1450*1*X*005010X224A2~
//ST*835*0001~
//BPR*I*187.50*C*CHK************20050412~
//TRN*1*0012524879*1559123456~
//REF*EV*030240928~
//DTM*405*20050412~
//N1*PR*YOUR TAX DOLLARS AT WORK~
//N3*481A00 DEER RUN ROAD~
//N4*WEST PALM BCH*FL*11114~
//N1*PE*ACME MEDICAL CENTER*FI*599944521~
//N3*PO BOX 863382~
//N4*ORLANDO*FL*55115~
//REF*PQ*10488~
//LX*1~
//CLP*0001000054*3*1766.5*187.50**12*50580155533~
//NM1*QC*1*ISLAND*ELLIS*E****MI*789123456~
//NM1*82*2*JONES JONES ASSOCIATES*****BS*AB34U~
//DTM*232*20050120~
//SVC*HC*24599*1766.5*187.50**1~
//DTM*472*20050120~
//CAS*OA*23*1579~
//REF*1B*44280~
//AMT*AU*1700~
//SE*38*0001~
//GE*1*1~
//IEA*1*000000001~

//Example 5 From 835 Specification:
//ISA*00*          *00*          *ZZ*810624427      *ZZ*113504607      *140217*1450*^*00501*000000001*0*P*:~
//GS*HC*810624427*113504607*20140217*1450*1*X*005010X224A2~
//ST*835*0001~
//BPR*I*34.00*C*CHK************20050318~
//TRN*1*0063158ABC*1566339911~
//REF*EV*030240928~
//DTM*405*20050318~
//N1*PR*YOUR TAX DOLLARS AT WORK~
//N3*481A00 DEER RUN ROAD~
//N4*WEST PALM BCH*FL*11114~
//N1*PE*ATONEWITHHEALTH*FI*3UR334563~
//N3*3501 JOHNSON STREET~
//N4*SUNSHINE*FL*12345~
//REF*PQ*11861~
//LX*1~
//CLP*0001000055*2*541*34**12*50650619501~
//NM1*QC*1*BRUCK*RAYMOND*W***MI*987654321~
//NM1*82*2*PROFESSIONAL TEST 1*****BS*34426~
//DTM*232*20050202~
//DTM*233*20050202~
//SVC*HC>55669*541*34**1~
//DTM*472*20050202~
//CAS*OA*23*516~
//CAS*OA*94*-9~
//REF*1B*44280~
//AMT*AU*550~
//SE*38*0001~
//GE*1*1~
//IEA*1*000000001~

#endregion Examples