using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenDentBusiness {
	///<summary>X12 834 Benefit Enrollment and Maintenance.  This transaction is used to push insurance plan information to pseudo clearinghouses.</summary>
	public class X834:X12object {

		///<summary>All segments within the current transaction set (ST) of the 834 report.</summary>
    private List<X12Segment> _listSegments;
		///<summary>The current segment within _listSegments.</summary>
		private int _segNum;

		public X834(string messageText):base(messageText) {
			ProcessMessage();
		}

		///<summary>See guide pages 22 and 208 for overview.</summary>
		private void ProcessMessage() {
			ProcessLoopGS();
		}

		///<summary>GS: Functional Group.  Required.  Repeat unlimited.  Guide page 208.</summary>
		private void ProcessLoopGS() {
			for(int i=0;i<FunctGroups.Count;i++) {
				ProcessLoopST(FunctGroups[i].Transactions);
			}
		}

		///<summary>ST: Transaction Set Header.  Required.  Repeat 1.  Guide pages 22, 31, 208.</summary>
		private void ProcessLoopST(List <X12Transaction> listTrans) {
			for(int i=0;i<listTrans.Count;i++) {
				_listSegments=listTrans[i].Segments;
				_segNum=0;
				ProcessLoopST_BGN();
				ProcessLoopST_REF();
				ProcessLoopST_DTP();
				ProcessLoopST_QTY();
				ProcessLoop1000A();
				ProcessLoop1000B();
				ProcessLoop1000C();
				ProcessLoop2000();
			}
		}

		///<summary>BGN: Beginning Segment.  Required.  Repeat 1.  Guide page 32.</summary>
		private void ProcessLoopST_BGN() {
			_segNum++;
		}

		///<summary>REF: Transaction Set Policy Number.  Situational.  Repeat 1.  Guide page 36.</summary>
		private void ProcessLoopST_REF() {
			if(_listSegments[_segNum].SegmentID!="REF") {
				return;
			}
			_segNum++;
		}

		///<summary>DTP: File Effictive Date.  Situational.  Repeat >1.  Guide page 37.</summary>
		private void ProcessLoopST_DTP() {
			while(_listSegments[_segNum].SegmentID=="DTP") {
				_segNum++;
			}
		}

		///<summary>QTY: Transaction Set Control Totals.  Situational.  Repeat 3.  Guide page 38.</summary>
		private void ProcessLoopST_QTY() {
			while(_listSegments[_segNum].SegmentID=="QTY") {
				_segNum++;
			}
		}

		///<summary>Loop 1000A: Sponsor Name.  Repeat 1.  Guide page 22.</summary>
		private void ProcessLoop1000A() {
			ProcessLoop1000A_N1();
		}

		///<summary>N1: Sponsor Name.  Required.  Repeat 1.  Guide page 39.</summary>
		private void ProcessLoop1000A_N1() {
			_segNum++;
		}

		///<summary>Loop 1000B: Payer.  Repeat 1.  Guide page 22.</summary>
		private void ProcessLoop1000B() {
			ProcessLoop1000B_N1();
		}

		///<summary>N1: Payer.  Required.  Repeat 1.  Guide page 41.</summary>
		private void ProcessLoop1000B_N1() {
			_segNum++;
		}

		///<summary>Loop 1000C: TPA/Broker Name.  Repeat 2.  Guide page 22.</summary>
		private void ProcessLoop1000C() {			
			while(_listSegments[_segNum].SegmentID=="N1") {
				ProcessLoop1000C_N1();
				ProcessLoop1100C();
			}
		}

		///<summary>N1: TPA/Broker Name.  Situational.  Repeat 1.  Guidep page 43.</summary>
		private void ProcessLoop1000C_N1() {
			if(_listSegments[_segNum].SegmentID!="N1") {
				return;
			}
			_segNum++;
		}

		///<summary>Loop 1100C: TPA/Broker Account.  Repeat 1.  Guide page 22.</summary>
		private void ProcessLoop1100C() {
			ProcessLoop1100C_ACT();
		}

		///<summary>ACT: TPA/Broker Account Information.  Situational.  Repeat 1.  Guide page 45.</summary>
		private void ProcessLoop1100C_ACT() {
			if(_listSegments[_segNum].SegmentID!="ACT") {
				return;
			}
			_segNum++;
		}

		///<summary>Loop 2000: Member Level Detail.  Repeat >1.  Guide page 22.</summary>
		private void ProcessLoop2000() {
			while(_listSegments[_segNum].SegmentID=="INS") {
				ProcessLoop2000_INS();
				ProcessLoop2000_REF_1();
				ProcessLoop2000_REF_2();
				ProcessLoop2000_REF_3();
				ProcessLoop2000_DTP();
				ProcessLoop2100A();
				ProcessLoop2100B();
				ProcessLoop2100C();
				ProcessLoop2100D();
				ProcessLoop2100E();
				ProcessLoop2100F();
				ProcessLoop2100G();
				ProcessLoop2100H();
				ProcessLoop2200();
				ProcessLoop2300();
				ProcessLoop2000_LS();
				ProcessLoop2700();
				ProcessLoop2000_LE();
			}
		}

		///<summary>INS: Member Level Detail.  Required.  Repeat 1.  Guide page 47.</summary>
		private void ProcessLoop2000_INS() {
			_segNum++;
		}

		///<summary>REF: Subscriber Identifier.  Required.  Repeat 1.  Guide page 55.</summary>
		private void ProcessLoop2000_REF_1() {
			_segNum++;
		}

		///<summary>REF: Member Policy Number.  Situational.  Repeat 1.  Guide page 56.</summary>
		private void ProcessLoop2000_REF_2() {
			if(_listSegments[_segNum].SegmentID!="REF" || _listSegments[_segNum].Get(1)!="1L") {
				return;
			}
			_segNum++;
		}

		///<summary>REF: Member Supplemental Identifier.  Situational.  Repeat 13.  Guide page 57.</summary>
		private void ProcessLoop2000_REF_3() {
			while(_listSegments[_segNum].SegmentID=="REF") {
				_segNum++;
			}
		}

		///<summary>DTP: Member Level Dates.  Situational.  Repeat 24.  Guide page 59.</summary>
		private void ProcessLoop2000_DTP() {
			while(_listSegments[_segNum].SegmentID=="DTP") {
				_segNum++;
			}
		}

		///<summary>Loop 2100A: Member Name.  Repeat 1.  Guide page 22.</summary>
		private void ProcessLoop2100A() {
			ProcessLoop2100A_NM1();
			ProcessLoop2100A_PER();
			ProcessLoop2100A_N3();
			ProcessLoop2100A_N4();
			ProcessLoop2100A_DMG();
			ProcessLoop2100A_EC();
			ProcessLoop2100A_ICM();
			ProcessLoop2100A_AMT();
			ProcessLoop2100A_HLH();
			ProcessLoop2100A_LUI();
		}

		///<summary>NM1: Member Name.  Required.  Repeat 1.  Guide page 62.</summary>
		private void ProcessLoop2100A_NM1() {
			_segNum++;
		}

		///<summary>PER: Member Communications Numbers.  Situational.  Repeat 1.  Guide page 65.</summary>
		private void ProcessLoop2100A_PER() {
			if(_listSegments[_segNum].SegmentID!="PER") {
				return;
			}
			_segNum++;
		}

		///<summary>N3: Member Residence Street Address.  Situational.  Repeat 1.  Guide page 68.</summary>
		private void ProcessLoop2100A_N3() {
			if(_listSegments[_segNum].SegmentID!="N3") {
				return;
			}
			_segNum++;
		}
		
		///<summary>N4: Member City, State, Zip Code.  Situational.  Repeat 1.  Guide page 69.</summary>
		private void ProcessLoop2100A_N4() {
			if(_listSegments[_segNum].SegmentID!="N4") {
				return;
			}
			_segNum++;
		}

		///<summary>DMG: Member Demographics.  Situational.  Repeat 1.  Guide page 72.</summary>
		private void ProcessLoop2100A_DMG() {
			if(_listSegments[_segNum].SegmentID!="DMG") {
				return;
			}
			_segNum++;
		}

		///<summary>EC: Employment Class.  Situational.  Repeat >1.  Guide page 76.</summary>
		private void ProcessLoop2100A_EC() {
			while(_listSegments[_segNum].SegmentID=="EC") {
				_segNum++;
			}
		}

		///<summary>ICM: Member Income.  Situational.  Repeat 1.  Guide page 79.</summary>
		private void ProcessLoop2100A_ICM() {
			if(_listSegments[_segNum].SegmentID!="ICM") {
				return;
			}
			_segNum++;
		}

		///<summary>AMT: Member Policy Amounts.  Situational.  Repeat 7.  Guide page 81.</summary>
		private void ProcessLoop2100A_AMT() {
			while(_listSegments[_segNum].SegmentID=="AMT") {
				_segNum++;
			}
		}

		///<summary>HLH: Member Health Information.  Situational.  Repeat 1.  Guide page 82.</summary>
		private void ProcessLoop2100A_HLH() {
			if(_listSegments[_segNum].SegmentID!="HLH") {
				return;
			}
			_segNum++;
		}

		///<summary>LUI: Member Language.  Situational.  Repeat >1.  Guide page 84.</summary>
		private void ProcessLoop2100A_LUI() {
			while(_listSegments[_segNum].SegmentID=="LUI") {
				_segNum++;
			}
		}

		///<summary>Loop 2100B: Incorrect Member Name.  Repeat 1.  Guide page 22.</summary>
		private void ProcessLoop2100B() {
			if(_listSegments[_segNum].SegmentID!="NM1" || _listSegments[_segNum].Get(1)!="70") {
				return;
			}
			ProcessLoop2100B_NM1();
			ProcessLoop2100B_DMG();
		}

		///<summary>NM1: Incorrect Member Name.  Situational.  Repeat 1.  Guide page 86.</summary>
		private void ProcessLoop2100B_NM1() {
			if(_listSegments[_segNum].SegmentID!="NM1") {
				return;
			}
			_segNum++;
		}

		///<summary>DMG: Incorrect Member Demographics.  Situational.  Repeat 1.  Guide page 89.</summary>
		private void ProcessLoop2100B_DMG() {
			if(_listSegments[_segNum].SegmentID!="DMG") {
				return;
			}
			_segNum++;
		}

		///<summary>Loop 2100C: Member Mailing Address.  Repeat 1.  Guide page 22.</summary>
		private void ProcessLoop2100C() {
			if(_listSegments[_segNum].SegmentID!="NM1" || _listSegments[_segNum].Get(1)!="31") {
				return;
			}
			ProcessLoop2100C_NM1();
			ProcessLoop2100C_N3();
			ProcessLoop2100C_N4();
		}

		///<summary>NM1: Member Mailing Address.  Situational.  Repeat 1.  Guide page 93.</summary>
		private void ProcessLoop2100C_NM1() {
			if(_listSegments[_segNum].SegmentID!="NM1") {
				return;
			}
			_segNum++;
		}

		///<summary>N3: Member Mail Street Address.  Required.  Repeat 1.  Guide page 95.</summary>
		private void ProcessLoop2100C_N3() {
			_segNum++;
		}

		///<summary>N4: Member Mail City, State, Zip Code.  Required.  Repeat 1.  Guide page 96.</summary>
		private void ProcessLoop2100C_N4() {
			_segNum++;
		}

		///<summary>Loop 2100D: Member Employer.  Repeat 3.  Guide page 23.</summary>
		private void ProcessLoop2100D() {
			while(_listSegments[_segNum].SegmentID=="NM1" && _listSegments[_segNum].Get(1)=="36") {
				ProcessLoop2100D_NM1();
				ProcessLoop2100D_PER();
				ProcessLoop2100D_N3();
				ProcessLoop2100D_N4();
			}
		}

		///<summary>NM1: Member Employer.  Situational.  Repeat 1.  Guide page 98.</summary>
		private void ProcessLoop2100D_NM1() {
			if(_listSegments[_segNum].SegmentID!="NM1") {
				return;
			}
			_segNum++;
		}

		///<summary>PER: Member Employer Communications Numbers.  Situational.  Repeat 1.  Guide page 101.</summary>
		private void ProcessLoop2100D_PER() {
			if(_listSegments[_segNum].SegmentID!="PER") {
				return;
			}
			_segNum++;
		}

		///<summary>N3: Member Employer Street Address.  Situational.  Repeat 1.  Guide page 104.</summary>
		private void ProcessLoop2100D_N3() {
			if(_listSegments[_segNum].SegmentID!="N3") {
				return;
			}
			_segNum++;
		}

		///<summary>N4: Member Employer City, State, Zip Code.  Situational.  Repeat 1.  Guide page 105.</summary>
		private void ProcessLoop2100D_N4() {
			if(_listSegments[_segNum].SegmentID!="N4") {
				return;
			}
			_segNum++;
		}

		///<summary>Loop 2100E: Member School.  Repeat 3.  Guide page 23.</summary>
		private void ProcessLoop2100E() {
			while(_listSegments[_segNum].SegmentID=="NM1" && _listSegments[_segNum].Get(1)=="M8") {
				ProcessLoop2100E_NM1();
				ProcessLoop2100E_PER();
				ProcessLoop2100E_N3();
				ProcessLoop2100E_N4();
			}
		}

		///<summary>NM1: Member School.  Situational.  Repeat 1.  Guide page 107.</summary>
		private void ProcessLoop2100E_NM1() {
			if(_listSegments[_segNum].SegmentID!="NM1") {
				return;
			}
			_segNum++;
		}

		///<summary>PER: Member School Communications Numbers.  Situational.  Repeat 1.  Guide page 109.</summary>
		private void ProcessLoop2100E_PER() {
			if(_listSegments[_segNum].SegmentID!="PER") {
				return;
			}
			_segNum++;
		}

		///<summary>N3: Member School Street Address.  Situational.  Repeat 1.  Guide page 112.</summary>
		private void ProcessLoop2100E_N3() {
			if(_listSegments[_segNum].SegmentID!="N3") {
				return;
			}
			_segNum++;
		}

		///<summary>N4: Member School City, State, Zip Code.  Repeat 1.  Guide page 113.</summary>
		private void ProcessLoop2100E_N4() {
			if(_listSegments[_segNum].SegmentID!="N4") {
				return;
			}
			_segNum++;
		}

		///<summary>Loop 2100F: Custodial Parent.  Repeat 1.  Guide page 23.</summary>
		private void ProcessLoop2100F() {
			if(_listSegments[_segNum].SegmentID!="NM1" || _listSegments[_segNum].Get(1)=="S3") {
				return;
			}
			ProcessLoop2100F_NM1();
			ProcessLoop2100F_PER();
			ProcessLoop2100F_N3();
			ProcessLoop2100F_N4();
		}

		///<summary>NM1: Custodial Parent.  Situational.  Repeat 1.  Guide page 115.</summary>
		private void ProcessLoop2100F_NM1() {
			if(_listSegments[_segNum].SegmentID!="NM1") {
				return;
			}
			_segNum++;
		}

		///<summary>PER: Custodial Parent Communications Numbers.  Situational.  Repeat 1.  Guide page 118.</summary>
		private void ProcessLoop2100F_PER() {
			if(_listSegments[_segNum].SegmentID!="PER") {
				return;
			}
			_segNum++;
		}

		///<summary>N3: Custodial Parent Street Address.  Situational.  Repeat 1.  Guide page 121.</summary>
		private void ProcessLoop2100F_N3() {
			if(_listSegments[_segNum].SegmentID!="N3") {
				return;
			}
			_segNum++;
		}

		///<summary>N4: Custodial Parent City, State, Zip Code.  Situational.  Repeat 1.  Guide page 122.</summary>
		private void ProcessLoop2100F_N4() {
			if(_listSegments[_segNum].SegmentID!="N4") {
				return;
			}
			_segNum++;
		}

		///<summary>Loop 2100G: Responsible Person.  Repeat 13.  Guide page 23.</summary>
		private void ProcessLoop2100G() {
			List <string> listNm1EntityCodes=new List<string>(new string[] { "6Y","9K","E1","EI","EXS","GB","GD","J6","LR","QD","S1","TZ","X4" });
			while(_listSegments[_segNum].SegmentID=="NM1" && listNm1EntityCodes.Contains(_listSegments[_segNum].Get(1))) {
				ProcessLoop2100G_NM1();
				ProcessLoop2100G_PER();
				ProcessLoop2100G_N3();
				ProcessLoop2100G_N4();
			}
		}

		///<summary>NM1: Responsible Person.  Situational.  Repeat 1.  Guide page 124.</summary>
		private void ProcessLoop2100G_NM1() {
			if(_listSegments[_segNum].SegmentID!="NM1") {
				return;
			}
			_segNum++;
		}

		///<summary>PER: Responsible Person Communications Numbers.  Situational.  Repeat 1.  Guide page 127.</summary>
		private void ProcessLoop2100G_PER() {
			if(_listSegments[_segNum].SegmentID!="PER") {
				return;
			}
			_segNum++;
		}

		///<summary>N3: Responsible Person Street Address.  Situational.  Repeat 1.  Guide page 130.</summary>
		private void ProcessLoop2100G_N3() {
			if(_listSegments[_segNum].SegmentID!="N3") {
				return;
			}
			_segNum++;
		}

		///<summary>N4: Responsible Person City, State, Zip Code.  Situational.  Repeat 1.  Guide page 131.</summary>
		private void ProcessLoop2100G_N4() {
			if(_listSegments[_segNum].SegmentID!="N4") {
				return;
			}
			_segNum++;
		}

		///<summary>Loop 2100H: Drop Off Location.  Repeat 1.  Guide page 23.</summary>
		private void ProcessLoop2100H() {
			if(_listSegments[_segNum].SegmentID!="NM1" || _listSegments[_segNum].Get(1)=="45") {
				return;
			}
			ProcessLoop2100H_NM1();
			ProcessLoop2100H_N3();
			ProcessLoop2100H_N4();
		}

		///<summary>NM1: Drop Off Location.  Situational.  Repeat 1.  Guide page 133.</summary>
		private void ProcessLoop2100H_NM1() {
			if(_listSegments[_segNum].SegmentID!="NM1") {
				return;
			}
			_segNum++;
		}

		///<summary>N3: Drop Off Location Street Address.  Situational.  Repeat 1.  Guide page 135.</summary>
		private void ProcessLoop2100H_N3() {
			if(_listSegments[_segNum].SegmentID!="N3") {
				return;
			}
			_segNum++;
		}

		///<summary>N4: Drop Off Location City, State, Zip Code.  Situational.  Repeat 1.  Guide page 136.</summary>
		private void ProcessLoop2100H_N4() {
			if(_listSegments[_segNum].SegmentID!="N4") {
				return;
			}
			_segNum++;
		}

		///<summary>Loop 2200: Disability Information.  Repeat >1.  Guide page 23.</summary>
		private void ProcessLoop2200() {
			while(_listSegments[_segNum].SegmentID=="DSB") {
				ProcessLoop2200_DSB();
				ProcessLoop2200_DTP();
			}
		}

		///<summary>DSB: Disability Information.  Situational.  Repeat 1.  Guide page 138.</summary>
		private void ProcessLoop2200_DSB() {
			if(_listSegments[_segNum].SegmentID!="DSB") {
				return;
			}
			_segNum++;
		}

		///<summary>DTP: Disability Eligibility Dates.  Situational.  Repeat 2.  Guide page 140.</summary>
		private void ProcessLoop2200_DTP() {
			while(_listSegments[_segNum].SegmentID=="DTP") {
				_segNum++;
			}
		}

		///<summary>Loop 2300: Health Coverage.  Repeat 99.  Guide page 23.</summary>
		private void ProcessLoop2300() {
			while(_listSegments[_segNum].SegmentID=="HD") {
				ProcessLoop2300_HD();
				ProcessLoop2300_DTP();
				ProcessLoop2300_AMT();
				ProcessLoop2300_REF_1();
				ProcessLoop2300_REF_2();
				ProcessLoop2300_IDC();
				ProcessLoop2310();
			}
		}

		///<summary>HD: Health Coverage.  Situational.  Repeat 1.  Guide page 141.</summary>
		private void ProcessLoop2300_HD() {
			if(_listSegments[_segNum].SegmentID!="HD") {
				return;
			}
			_segNum++;
		}

		///<summary>DTP: Health Coverage Dates.  Required.  Repeat 6.  Guide page 145.</summary>
		private void ProcessLoop2300_DTP() {
			while(_listSegments[_segNum].SegmentID=="DTP") {
				_segNum++;
			}
		}

		///<summary>AMT: Health Coverage Policy.  Situational.  Repeat 9.  Guide page 147.</summary>
		private void ProcessLoop2300_AMT() {
			while(_listSegments[_segNum].SegmentID=="AMT") {
				_segNum++;
			}
		}

		///<summary>REF: Health Coverage Policy Number.  Situational.  Repeat 14.  Guide page 148.</summary>
		private void ProcessLoop2300_REF_1() {
			List <string> listRefQualifiers=new List<string>(new string[] { "17","1L","9V","CE","E8","M7","PID","RB","X9","XM","XX1","XX2","ZX","ZZ" });
			while(_listSegments[_segNum].SegmentID=="REF" && listRefQualifiers.Contains(_listSegments[_segNum].Get(1))) {
				_segNum++;
			}
		}

		///<summary>REF: Prior Coverage Months.  Situational.  Repeat 1.  Guide page 150.</summary>
		private void ProcessLoop2300_REF_2() {
			if(_listSegments[_segNum].SegmentID!="REF" || _listSegments[_segNum].Get(1)!="QQ") {
				return;
			}
			_segNum++;
		}

		///<summary>IDC: IDentification Card.  Situational.  Repeat 3.  Guide page 152.</summary>
		private void ProcessLoop2300_IDC() {
			while(_listSegments[_segNum].SegmentID=="IDC") {
				_segNum++;
			}
		}

		///<summary>Loop 2310: Provider Information.  Repeat 30.  Guide page 23.</summary>
		private void ProcessLoop2310() {
			//There are two different LX segments which could be at this spot.
			//The LX segments have the same simple format, so we have to look at the following segment to figure out which LX we are looking at.
			while(_listSegments[_segNum].SegmentID=="LX" && (_segNum+1) < _listSegments.Count && _listSegments[_segNum+1].SegmentID=="NM1") {
				ProcessLoop2310_LX();
				ProcessLoop2310_NM1();
				ProcessLoop2310_N3();
				ProcessLoop2310_N4();
				ProcessLoop2310_PER();
				ProcessLoop2310_PLA();
				ProcessLoop2320();
			}
		}

		///<summary>LX: Provider Information.  Situational.  Repeat 1.  Guide page 154.</summary>
		private void ProcessLoop2310_LX() {
			if(_listSegments[_segNum].SegmentID!="LX") {
				return;
			}
			_segNum++;
		}

		///<summary>NM1: Provider Name.  Required.  Repeat 1.  Guide page 155.</summary>
		private void ProcessLoop2310_NM1() {
			_segNum++;
		}

		///<summary>N3: Provider Address. Situational.  Repeat 2.  Guide page 158.</summary>
		private void ProcessLoop2310_N3() {
			while(_listSegments[_segNum].SegmentID=="N3") {
				_segNum++;
			}
		}

		///<summary>N4: Provider City, State, Zip Code.  Situational.  Repeat 1.  Guide page 159.</summary>
		private void ProcessLoop2310_N4() {
			if(_listSegments[_segNum].SegmentID!="N4") {
				return;
			}
			_segNum++;
		}

		///<summary>PER: Provider Communications Numbers.  Situational.  Repeat 2.  Guide page 161.</summary>
		private void ProcessLoop2310_PER() {
			while(_listSegments[_segNum].SegmentID=="PER") {
				_segNum++;
			}
		}

		///<summary>PLA: PRovider Change Reason.  Situational.  Repeat 1.  Guide page 164.</summary>
		private void ProcessLoop2310_PLA() {
			if(_listSegments[_segNum].SegmentID!="PLA") {
				return;
			}
			_segNum++;
		}

		///<summary>Loop 2320: Coordination of Benefits.  Repeat 5.  Guide page 23.</summary>
		private void ProcessLoop2320() {
			while(_listSegments[_segNum].SegmentID=="COB") {
				ProcessLoop2320_COB();
				ProcessLoop2320_REF();
				ProcessLoop2320_DTP();
				ProcessLoop2330();
			}
		}

		///<summary>COB: Coordination of Benefits.  Situational.  Repeat 1.  Guide page 166.</summary>
		private void ProcessLoop2320_COB() {
			if(_listSegments[_segNum].SegmentID!="COB") {
				return;
			}
			_segNum++;
		}

		///<summary>REF: Additional Coordination of Benefits Identifiers.  Situational.  Repeat 4.  Guide page 168.</summary>
		private void ProcessLoop2320_REF() {
			List <string> listRefQualifiers=new List<string>(new string[] { "60","6P","SY","ZZ" });
			while(_listSegments[_segNum].SegmentID=="REF" && listRefQualifiers.Contains(_listSegments[_segNum].Get(1))) {
				_segNum++;
			}
		}

		///<summary>DTP: Coordination of Benefits Eligibility Dates.  Situational.  Repeat 2.  Guide page 170.</summary>
		private void ProcessLoop2320_DTP() {
			while(_listSegments[_segNum].SegmentID=="DTP") {
				_segNum++;
			}
		}

		///<summary>Loop 2330: Coordination of Benefits Related Entity.  Repeat 3.  Guide page 23.</summary>
		private void ProcessLoop2330() {
			List <string> listIdentifierCodes=new List<string>(new string[] { "36","GW","IN" });
			while(_listSegments[_segNum].SegmentID=="NM1" && listIdentifierCodes.Contains(_listSegments[_segNum].Get(1))) {
				ProcessLoop2330_NM1();
				ProcessLoop2330_N3();
				ProcessLoop2330_N4();
				ProcessLoop2330_PER();
			}
		}

		///<summary>NM1: Coordination of Benefits Releated Entity.  Situational.  Repeat 1.  Guide page 171.</summary>
		private void ProcessLoop2330_NM1() {
			if(_listSegments[_segNum].SegmentID!="NM1") {
				return;
			}
			_segNum++;
		}

		///<summary>N3: Coordination of Benefits Related Entity Address.  Situational.  Repeat 1.  Guide page 173.</summary>
		private void ProcessLoop2330_N3() {
			if(_listSegments[_segNum].SegmentID!="N3") {
				return;
			}
			_segNum++;
		}

		///<summary>N4: Coordination of Benefits Other Insurance Company City, State, Zip Code.  Repeat 1.  Guide page 174.</summary>
		private void ProcessLoop2330_N4() {
			if(_listSegments[_segNum].SegmentID!="N4") {
				return;
			}
			_segNum++;
		}

		///<summary>PER: Administrative Communications Contact.  Situational.  Repeat 1.  Guide page 176.</summary>
		private void ProcessLoop2330_PER() {
			if(_listSegments[_segNum].SegmentID!="PER") {
				return;
			}
			_segNum++;
		}

		///<summary>LS: Additional Reporting Categories.  Situational.  Repeat 1.  Guide page 178.</summary>
		private void ProcessLoop2000_LS() {
			if(_listSegments[_segNum].SegmentID!="LS") {
				return;
			}
			_segNum++;
		}

		///<summary>Loop 2700: Member Reporting Categories.  Repeat >1.  Guide page 24.</summary>
		private void ProcessLoop2700() {
			while(_listSegments[_segNum].SegmentID=="LX") {
				ProcessLoop2700_LX();
				ProcessLoop2750();
			}
		}

		///<summary>LX: Member Reporting Categories.  Situational.  Repeat 1.  Guide page 179.</summary>
		private void ProcessLoop2700_LX() {
			if(_listSegments[_segNum].SegmentID!="LX") {
				return;
			}
			_segNum++;
		}

		///<summary>Loop 2750: Reporting Category.  Repeat 1.  Guide page 24.</summary>
		private void ProcessLoop2750() {
			ProcessLoop2750_N1();
			ProcessLoop2750_REF();
			ProcessLoop2750_DTP();
		}

		///<summary>N1: Reporting Category.  Situational.  Repeat 1.  Guide page 180.</summary>
		private void ProcessLoop2750_N1() {
			if(_listSegments[_segNum].SegmentID!="N1") {
				return;
			}
			_segNum++;
		}

		///<summary>REF: Reporting Category Reference.  Situational.  Repeat 1.  Guide page 181.</summary>
		private void ProcessLoop2750_REF() {
			if(_listSegments[_segNum].SegmentID!="REF") {
				return;
			}
			_segNum++;
		}

		///<summary>DTP: Reporting Category Date.  Situational.  Repeat 1.  Guide page 183.</summary>
		private void ProcessLoop2750_DTP() {
			if(_listSegments[_segNum].SegmentID!="DTP") {
				return;
			}
			_segNum++;
		}

		///<summary>LE: Additional Reporting Categories Loop Termination.  Situational.  Repeat 1.  Guide page 185.</summary>
		private void ProcessLoop2000_LE() {
			if(_listSegments[_segNum].SegmentID!="LE") {
				return;
			}
			_segNum++;
		}

	}
}