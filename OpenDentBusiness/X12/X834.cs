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
		///<summary>Loop 1000A N1</summary>
		private Hx834_Name _sponsorName;
		///<summary>Loop 1000B N1</summary>
		private Hx834_Name _payerName;
		///<summary>Loop 1000C N1 and Loop 1100C ACT</summary>
		private List <Hx834_Broker> _listBrokers=new List<Hx834_Broker>();
		///<summary>Loop 2000</summary>
		private List <Hx834_Member> _listMembers=new List<Hx834_Member>();
		
		///<summary>Shortcut to get current segment based on _segNum.</summary>
		private X12Segment _segCur { get { return _listSegments[_segNum]; } }
		
		///<summary>Loop 1000A N1</summary>
		public Hx834_Name SponsorName {	get { return _sponsorName; } }
		///<summary>Loop 1000B N1</summary>
		public Hx834_Name PayerName {	get { return _payerName; } }
		///<summary>Loop 1000C N1 and Loop 1100C ACT</summary>
		public List <Hx834_Broker> ListBrokers { get { return _listBrokers; } }
		///<summary>Loop 2000</summary>
		public List <Hx834_Member> ListMembers { get { return _listMembers; } }

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
			if(_segCur.SegmentID!="REF") {
				return;
			}
			_segNum++;
		}

		///<summary>DTP: File Effictive Date.  Situational.  Repeat >1.  Guide page 37.</summary>
		private void ProcessLoopST_DTP() {
			while(_segCur.SegmentID=="DTP") {
				_segNum++;
			}
		}

		///<summary>QTY: Transaction Set Control Totals.  Situational.  Repeat 3.  Guide page 38.</summary>
		private void ProcessLoopST_QTY() {
			while(_segCur.SegmentID=="QTY") {
				_segNum++;
			}
		}

		///<summary>Loop 1000A: Sponsor Name.  Repeat 1.  Guide page 22.</summary>
		private void ProcessLoop1000A() {
			ProcessLoop1000A_N1();
		}

		///<summary>N1: Sponsor Name.  Required.  Repeat 1.  Guide page 39.</summary>
		private void ProcessLoop1000A_N1() {
			_sponsorName=new Hx834_Name(_segCur);
			_segNum++;
		}

		///<summary>Loop 1000B: Payer.  Repeat 1.  Guide page 22.</summary>
		private void ProcessLoop1000B() {
			ProcessLoop1000B_N1();
		}

		///<summary>N1: Payer.  Required.  Repeat 1.  Guide page 41.</summary>
		private void ProcessLoop1000B_N1() {
			_payerName=new Hx834_Name(_segCur);
			_segNum++;
		}

		///<summary>Loop 1000C: TPA/Broker Name.  Repeat 2.  Guide page 22.</summary>
		private void ProcessLoop1000C() {
			_listBrokers.Clear();
			while(_segCur.SegmentID=="N1") {
				Hx834_Broker broker=new Hx834_Broker();
				ProcessLoop1000C_N1(broker);
				ProcessLoop1100C(broker);
				_listBrokers.Add(broker);
			}
		}

		///<summary>N1: TPA/Broker Name.  Situational.  Repeat 1.  Guide page 43.</summary>
		private void ProcessLoop1000C_N1(Hx834_Broker broker) {
			if(_segCur.SegmentID!="N1") {
				return;
			}
			broker.Name=new Hx834_Name(_segCur);
			_segNum++;
		}

		///<summary>Loop 1100C: TPA/Broker Account.  Repeat 1.  Guide page 22.</summary>
		private void ProcessLoop1100C(Hx834_Broker broker) {
			ProcessLoop1100C_ACT(broker);
		}

		///<summary>ACT: TPA/Broker Account Information.  Situational.  Repeat 1.  Guide page 45.</summary>
		private void ProcessLoop1100C_ACT(Hx834_Broker broker) {
			if(_segCur.SegmentID!="ACT") {
				return;
			}
			broker.AccountNumber1=_segCur.Get(1);
			broker.AccountNumber2=_segCur.Get(6);
			_segNum++;
		}

		///<summary>Loop 2000: Member Level Detail.  Repeat >1.  Guide page 22.</summary>
		private void ProcessLoop2000() {
			_listMembers.Clear();
			while(_segCur.SegmentID=="INS") {
				Hx834_Member member=new Hx834_Member();
				ProcessLoop2000_INS(member);
				ProcessLoop2000_REF_1(member);
				ProcessLoop2000_REF_2(member);
				ProcessLoop2000_REF_3(member);
				ProcessLoop2000_DTP(member);
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
				_listMembers.Add(member);
			}
		}

		///<summary>INS: Member Level Detail.  Required.  Repeat 1.  Guide page 47.</summary>
		private void ProcessLoop2000_INS(Hx834_Member member) {
			member.IsSubscriber=false;
			if(_segCur.Get(1)=="Y") {
				member.IsSubscriber=true;
			}
			member.InsRelationshipCode=_segCur.Get(2);
			member.Relationship=Relat.Dependent;//Default
			if(member.InsRelationshipCode=="01") {//Spouse
				member.Relationship=Relat.Spouse;
			}
			else if(member.InsRelationshipCode=="03") {//Father or Mother
				member.Relationship=Relat.Dependent;
			}
			else if(member.InsRelationshipCode=="04") {//Grandfather or Grandmother
				member.Relationship=Relat.Dependent;
			}
			else if(member.InsRelationshipCode=="05") {//Grandson or Granddaugher
				member.Relationship=Relat.Dependent;
			}
			else if(member.InsRelationshipCode=="06") {//Uncle or Aunt
				member.Relationship=Relat.Dependent;
			}
			else if(member.InsRelationshipCode=="07") {//Nephew or Niece
				member.Relationship=Relat.Dependent;
			}
			else if(member.InsRelationshipCode=="08") {//Cousin
				member.Relationship=Relat.Dependent;
			}
			else if(member.InsRelationshipCode=="09") {//Adopted Child
				member.Relationship=Relat.Child;
			}
			else if(member.InsRelationshipCode=="10") {//Foster Child
				member.Relationship=Relat.Child;
			}
			else if(member.InsRelationshipCode=="11") {//Son-in-law or Daughter-in-law
				member.Relationship=Relat.Dependent;
			}
			else if(member.InsRelationshipCode=="12") {//Brother-in-law or Sister-in-law
				member.Relationship=Relat.Dependent;
			}
			else if(member.InsRelationshipCode=="13") {//Mother-in-law or Father-in-law
				member.Relationship=Relat.Dependent;
			}
			else if(member.InsRelationshipCode=="14") {//Brother or Sister
				member.Relationship=Relat.Dependent;
			}
			else if(member.InsRelationshipCode=="15") {//Ward
				member.Relationship=Relat.Dependent;
			}
			else if(member.InsRelationshipCode=="16") {//Stepparent
				member.Relationship=Relat.Dependent;
			}
			else if(member.InsRelationshipCode=="17") {//Stepson or Stepdaughter
				member.Relationship=Relat.Child;
			}
			else if(member.InsRelationshipCode=="18") {//Self
				member.Relationship=Relat.Self;
			}
			else if(member.InsRelationshipCode=="19") {//Child
				member.Relationship=Relat.Child;
			}
			else if(member.InsRelationshipCode=="23") {//Sponsored Dependent
				member.Relationship=Relat.Dependent;
			}
			else if(member.InsRelationshipCode=="24") {//Dependent of a Minor Dependent
				member.Relationship=Relat.Dependent;
			}
			else if(member.InsRelationshipCode=="25") {//Ex-spouse
				member.Relationship=Relat.Dependent;
			}
			else if(member.InsRelationshipCode=="26") {//Guardian
				member.Relationship=Relat.Dependent;
			}
			else if(member.InsRelationshipCode=="31") {//Court Appointed Guardian
				member.Relationship=Relat.Dependent;
			}
			else if(member.InsRelationshipCode=="38") {//Collateral Dependent
				member.Relationship=Relat.HandicapDep;
			}
			else if(member.InsRelationshipCode=="53") {//Life Partner
				member.Relationship=Relat.LifePartner;
			}
			else if(member.InsRelationshipCode=="60") {//Annuitant
				member.Relationship=Relat.Dependent;
			}
			else if(member.InsRelationshipCode=="D2") {//Trustee
				member.Relationship=Relat.Dependent;
			}
			else if(member.InsRelationshipCode=="G8") {//Other Relationship
				member.Relationship=Relat.Dependent;
			}
			else if(member.InsRelationshipCode=="G9") {//Other Relative
				member.Relationship=Relat.Dependent;
			}
			member.MaintTypeCode=_segCur.Get(3);
			member.MaintReason=Hx834_MemberMaintReason.None;
			if(member.MaintTypeCode=="001") {//Change
				member.MaintReason=Hx834_MemberMaintReason.Change;
			}
			else if(member.MaintTypeCode=="021") {//Addition
				member.MaintReason=Hx834_MemberMaintReason.Addition;
			}
			else if(member.MaintTypeCode=="024") {//Cancellation or Termination
				member.MaintReason=Hx834_MemberMaintReason.CancellationOrTermination;
			}
			else if(member.MaintTypeCode=="025") {//Reinstatement
				member.MaintReason=Hx834_MemberMaintReason.Reinstatement;
			}
			else if(member.MaintTypeCode=="030") {//Audit or Compare
				member.MaintReason=Hx834_MemberMaintReason.AuditOrCompare;
			}
			member.MaintReasonCode=_segCur.Get(4);
			member.BenefitStatusCode=_segCur.Get(5);
			if(_segCur.Get(6)!="") {
				string[] arrayMedicare=_segCur.Get(6).Split(Separators.Subelement[0]);
				member.MedicarePlanCode=arrayMedicare[0];
				if(arrayMedicare.Length>1) {
					member.MedicareEligibilityReasonCode=arrayMedicare[1];
				}
			}
			member.CobraQualifyingCode=_segCur.Get(7);
			member.EmploymentStatusCode=_segCur.Get(8);
			member.StudentStatusCode=_segCur.Get(9);
			member.IsHandicapped=false;
			if(_segCur.Get(10)=="Y") {
				member.IsHandicapped=true;
			}
			member.DateDeath=DtmToDateTime(_segCur.Get(12));
			member.IsInfoRestrictedAccess=false;
			if(_segCur.Get(13)=="R") {
				member.IsInfoRestrictedAccess=true;
			}
			member.BirthSequenceNumber=PIn.Int(_segCur.Get(17));
			_segNum++;
		}

		///<summary>REF: Subscriber Identifier.  Required.  Repeat 1.  Guide page 55.</summary>
		private void ProcessLoop2000_REF_1(Hx834_Member member) {
			member.SubscriberId=_segCur.Get(2);
			_segNum++;
		}

		///<summary>REF: Member Policy Number.  Situational.  Repeat 1.  Guide page 56.</summary>
		private void ProcessLoop2000_REF_2(Hx834_Member member) {
			if(_segCur.SegmentID!="REF" || _segCur.Get(1)!="1L") {
				return;
			}
			member.GroupPolicyNumber=_segCur.Get(2);
			_segNum++;
		}

		///<summary>REF: Member Supplemental Identifier.  Situational.  Repeat 13.  Guide page 57.</summary>
		private void ProcessLoop2000_REF_3(Hx834_Member member) {
			member.ListMemberSupplementalIds.Clear();
			while(_segCur.SegmentID=="REF") {
				Hx834_Ref href=new Hx834_Ref();
				href.ReferenceIdQualifier=_segCur.Get(1);
				href.ReferenceId=_segCur.Get(2);
				member.ListMemberSupplementalIds.Add(href);
				_segNum++;
			}
		}

		///<summary>DTP: Member Level Dates.  Situational.  Repeat 24.  Guide page 59.</summary>
		private void ProcessLoop2000_DTP(Hx834_Member member) {
			member.ListMemberDates.Clear();
			while(_segCur.SegmentID=="DTP") {
				Hx834_Dtp dtp=new Hx834_Dtp();
				dtp.DateTimeQualifier=_segCur.Get(1);
				dtp.DateT=DtmToDateTime(_segCur.Get(3));
				member.ListMemberDates.Add(dtp);
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
			if(_segCur.SegmentID!="PER") {
				return;
			}
			_segNum++;
		}

		///<summary>N3: Member Residence Street Address.  Situational.  Repeat 1.  Guide page 68.</summary>
		private void ProcessLoop2100A_N3() {
			if(_segCur.SegmentID!="N3") {
				return;
			}
			_segNum++;
		}
		
		///<summary>N4: Member City, State, Zip Code.  Situational.  Repeat 1.  Guide page 69.</summary>
		private void ProcessLoop2100A_N4() {
			if(_segCur.SegmentID!="N4") {
				return;
			}
			_segNum++;
		}

		///<summary>DMG: Member Demographics.  Situational.  Repeat 1.  Guide page 72.</summary>
		private void ProcessLoop2100A_DMG() {
			if(_segCur.SegmentID!="DMG") {
				return;
			}
			_segNum++;
		}

		///<summary>EC: Employment Class.  Situational.  Repeat >1.  Guide page 76.</summary>
		private void ProcessLoop2100A_EC() {
			while(_segCur.SegmentID=="EC") {
				_segNum++;
			}
		}

		///<summary>ICM: Member Income.  Situational.  Repeat 1.  Guide page 79.</summary>
		private void ProcessLoop2100A_ICM() {
			if(_segCur.SegmentID!="ICM") {
				return;
			}
			_segNum++;
		}

		///<summary>AMT: Member Policy Amounts.  Situational.  Repeat 7.  Guide page 81.</summary>
		private void ProcessLoop2100A_AMT() {
			while(_segCur.SegmentID=="AMT") {
				_segNum++;
			}
		}

		///<summary>HLH: Member Health Information.  Situational.  Repeat 1.  Guide page 82.</summary>
		private void ProcessLoop2100A_HLH() {
			if(_segCur.SegmentID!="HLH") {
				return;
			}
			_segNum++;
		}

		///<summary>LUI: Member Language.  Situational.  Repeat >1.  Guide page 84.</summary>
		private void ProcessLoop2100A_LUI() {
			while(_segCur.SegmentID=="LUI") {
				_segNum++;
			}
		}

		///<summary>Loop 2100B: Incorrect Member Name.  Repeat 1.  Guide page 22.</summary>
		private void ProcessLoop2100B() {
			if(_segCur.SegmentID!="NM1" || _segCur.Get(1)!="70") {
				return;
			}
			ProcessLoop2100B_NM1();
			ProcessLoop2100B_DMG();
		}

		///<summary>NM1: Incorrect Member Name.  Situational.  Repeat 1.  Guide page 86.</summary>
		private void ProcessLoop2100B_NM1() {
			if(_segCur.SegmentID!="NM1") {
				return;
			}
			_segNum++;
		}

		///<summary>DMG: Incorrect Member Demographics.  Situational.  Repeat 1.  Guide page 89.</summary>
		private void ProcessLoop2100B_DMG() {
			if(_segCur.SegmentID!="DMG") {
				return;
			}
			_segNum++;
		}

		///<summary>Loop 2100C: Member Mailing Address.  Repeat 1.  Guide page 22.</summary>
		private void ProcessLoop2100C() {
			if(_segCur.SegmentID!="NM1" || _segCur.Get(1)!="31") {
				return;
			}
			ProcessLoop2100C_NM1();
			ProcessLoop2100C_N3();
			ProcessLoop2100C_N4();
		}

		///<summary>NM1: Member Mailing Address.  Situational.  Repeat 1.  Guide page 93.</summary>
		private void ProcessLoop2100C_NM1() {
			if(_segCur.SegmentID!="NM1") {
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
			while(_segCur.SegmentID=="NM1" && _segCur.Get(1)=="36") {
				ProcessLoop2100D_NM1();
				ProcessLoop2100D_PER();
				ProcessLoop2100D_N3();
				ProcessLoop2100D_N4();
			}
		}

		///<summary>NM1: Member Employer.  Situational.  Repeat 1.  Guide page 98.</summary>
		private void ProcessLoop2100D_NM1() {
			if(_segCur.SegmentID!="NM1") {
				return;
			}
			_segNum++;
		}

		///<summary>PER: Member Employer Communications Numbers.  Situational.  Repeat 1.  Guide page 101.</summary>
		private void ProcessLoop2100D_PER() {
			if(_segCur.SegmentID!="PER") {
				return;
			}
			_segNum++;
		}

		///<summary>N3: Member Employer Street Address.  Situational.  Repeat 1.  Guide page 104.</summary>
		private void ProcessLoop2100D_N3() {
			if(_segCur.SegmentID!="N3") {
				return;
			}
			_segNum++;
		}

		///<summary>N4: Member Employer City, State, Zip Code.  Situational.  Repeat 1.  Guide page 105.</summary>
		private void ProcessLoop2100D_N4() {
			if(_segCur.SegmentID!="N4") {
				return;
			}
			_segNum++;
		}

		///<summary>Loop 2100E: Member School.  Repeat 3.  Guide page 23.</summary>
		private void ProcessLoop2100E() {
			while(_segCur.SegmentID=="NM1" && _segCur.Get(1)=="M8") {
				ProcessLoop2100E_NM1();
				ProcessLoop2100E_PER();
				ProcessLoop2100E_N3();
				ProcessLoop2100E_N4();
			}
		}

		///<summary>NM1: Member School.  Situational.  Repeat 1.  Guide page 107.</summary>
		private void ProcessLoop2100E_NM1() {
			if(_segCur.SegmentID!="NM1") {
				return;
			}
			_segNum++;
		}

		///<summary>PER: Member School Communications Numbers.  Situational.  Repeat 1.  Guide page 109.</summary>
		private void ProcessLoop2100E_PER() {
			if(_segCur.SegmentID!="PER") {
				return;
			}
			_segNum++;
		}

		///<summary>N3: Member School Street Address.  Situational.  Repeat 1.  Guide page 112.</summary>
		private void ProcessLoop2100E_N3() {
			if(_segCur.SegmentID!="N3") {
				return;
			}
			_segNum++;
		}

		///<summary>N4: Member School City, State, Zip Code.  Repeat 1.  Guide page 113.</summary>
		private void ProcessLoop2100E_N4() {
			if(_segCur.SegmentID!="N4") {
				return;
			}
			_segNum++;
		}

		///<summary>Loop 2100F: Custodial Parent.  Repeat 1.  Guide page 23.</summary>
		private void ProcessLoop2100F() {
			if(_segCur.SegmentID!="NM1" || _segCur.Get(1)=="S3") {
				return;
			}
			ProcessLoop2100F_NM1();
			ProcessLoop2100F_PER();
			ProcessLoop2100F_N3();
			ProcessLoop2100F_N4();
		}

		///<summary>NM1: Custodial Parent.  Situational.  Repeat 1.  Guide page 115.</summary>
		private void ProcessLoop2100F_NM1() {
			if(_segCur.SegmentID!="NM1") {
				return;
			}
			_segNum++;
		}

		///<summary>PER: Custodial Parent Communications Numbers.  Situational.  Repeat 1.  Guide page 118.</summary>
		private void ProcessLoop2100F_PER() {
			if(_segCur.SegmentID!="PER") {
				return;
			}
			_segNum++;
		}

		///<summary>N3: Custodial Parent Street Address.  Situational.  Repeat 1.  Guide page 121.</summary>
		private void ProcessLoop2100F_N3() {
			if(_segCur.SegmentID!="N3") {
				return;
			}
			_segNum++;
		}

		///<summary>N4: Custodial Parent City, State, Zip Code.  Situational.  Repeat 1.  Guide page 122.</summary>
		private void ProcessLoop2100F_N4() {
			if(_segCur.SegmentID!="N4") {
				return;
			}
			_segNum++;
		}

		///<summary>Loop 2100G: Responsible Person.  Repeat 13.  Guide page 23.</summary>
		private void ProcessLoop2100G() {
			List <string> listNm1EntityCodes=new List<string>(new string[] { "6Y","9K","E1","EI","EXS","GB","GD","J6","LR","QD","S1","TZ","X4" });
			while(_segCur.SegmentID=="NM1" && listNm1EntityCodes.Contains(_segCur.Get(1))) {
				ProcessLoop2100G_NM1();
				ProcessLoop2100G_PER();
				ProcessLoop2100G_N3();
				ProcessLoop2100G_N4();
			}
		}

		///<summary>NM1: Responsible Person.  Situational.  Repeat 1.  Guide page 124.</summary>
		private void ProcessLoop2100G_NM1() {
			if(_segCur.SegmentID!="NM1") {
				return;
			}
			_segNum++;
		}

		///<summary>PER: Responsible Person Communications Numbers.  Situational.  Repeat 1.  Guide page 127.</summary>
		private void ProcessLoop2100G_PER() {
			if(_segCur.SegmentID!="PER") {
				return;
			}
			_segNum++;
		}

		///<summary>N3: Responsible Person Street Address.  Situational.  Repeat 1.  Guide page 130.</summary>
		private void ProcessLoop2100G_N3() {
			if(_segCur.SegmentID!="N3") {
				return;
			}
			_segNum++;
		}

		///<summary>N4: Responsible Person City, State, Zip Code.  Situational.  Repeat 1.  Guide page 131.</summary>
		private void ProcessLoop2100G_N4() {
			if(_segCur.SegmentID!="N4") {
				return;
			}
			_segNum++;
		}

		///<summary>Loop 2100H: Drop Off Location.  Repeat 1.  Guide page 23.</summary>
		private void ProcessLoop2100H() {
			if(_segCur.SegmentID!="NM1" || _segCur.Get(1)=="45") {
				return;
			}
			ProcessLoop2100H_NM1();
			ProcessLoop2100H_N3();
			ProcessLoop2100H_N4();
		}

		///<summary>NM1: Drop Off Location.  Situational.  Repeat 1.  Guide page 133.</summary>
		private void ProcessLoop2100H_NM1() {
			if(_segCur.SegmentID!="NM1") {
				return;
			}
			_segNum++;
		}

		///<summary>N3: Drop Off Location Street Address.  Situational.  Repeat 1.  Guide page 135.</summary>
		private void ProcessLoop2100H_N3() {
			if(_segCur.SegmentID!="N3") {
				return;
			}
			_segNum++;
		}

		///<summary>N4: Drop Off Location City, State, Zip Code.  Situational.  Repeat 1.  Guide page 136.</summary>
		private void ProcessLoop2100H_N4() {
			if(_segCur.SegmentID!="N4") {
				return;
			}
			_segNum++;
		}

		///<summary>Loop 2200: Disability Information.  Repeat >1.  Guide page 23.</summary>
		private void ProcessLoop2200() {
			while(_segCur.SegmentID=="DSB") {
				ProcessLoop2200_DSB();
				ProcessLoop2200_DTP();
			}
		}

		///<summary>DSB: Disability Information.  Situational.  Repeat 1.  Guide page 138.</summary>
		private void ProcessLoop2200_DSB() {
			if(_segCur.SegmentID!="DSB") {
				return;
			}
			_segNum++;
		}

		///<summary>DTP: Disability Eligibility Dates.  Situational.  Repeat 2.  Guide page 140.</summary>
		private void ProcessLoop2200_DTP() {
			while(_segCur.SegmentID=="DTP") {
				_segNum++;
			}
		}

		///<summary>Loop 2300: Health Coverage.  Repeat 99.  Guide page 23.</summary>
		private void ProcessLoop2300() {
			while(_segCur.SegmentID=="HD") {
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
			if(_segCur.SegmentID!="HD") {
				return;
			}
			_segNum++;
		}

		///<summary>DTP: Health Coverage Dates.  Required.  Repeat 6.  Guide page 145.</summary>
		private void ProcessLoop2300_DTP() {
			while(_segCur.SegmentID=="DTP") {
				_segNum++;
			}
		}

		///<summary>AMT: Health Coverage Policy.  Situational.  Repeat 9.  Guide page 147.</summary>
		private void ProcessLoop2300_AMT() {
			while(_segCur.SegmentID=="AMT") {
				_segNum++;
			}
		}

		///<summary>REF: Health Coverage Policy Number.  Situational.  Repeat 14.  Guide page 148.</summary>
		private void ProcessLoop2300_REF_1() {
			List <string> listRefQualifiers=new List<string>(new string[] { "17","1L","9V","CE","E8","M7","PID","RB","X9","XM","XX1","XX2","ZX","ZZ" });
			while(_segCur.SegmentID=="REF" && listRefQualifiers.Contains(_segCur.Get(1))) {
				_segNum++;
			}
		}

		///<summary>REF: Prior Coverage Months.  Situational.  Repeat 1.  Guide page 150.</summary>
		private void ProcessLoop2300_REF_2() {
			if(_segCur.SegmentID!="REF" || _segCur.Get(1)!="QQ") {
				return;
			}
			_segNum++;
		}

		///<summary>IDC: IDentification Card.  Situational.  Repeat 3.  Guide page 152.</summary>
		private void ProcessLoop2300_IDC() {
			while(_segCur.SegmentID=="IDC") {
				_segNum++;
			}
		}

		///<summary>Loop 2310: Provider Information.  Repeat 30.  Guide page 23.</summary>
		private void ProcessLoop2310() {
			//There are two different LX segments which could be at this spot.
			//The LX segments have the same simple format, so we have to look at the following segment to figure out which LX we are looking at.
			while(_segCur.SegmentID=="LX" && (_segNum+1) < _listSegments.Count && _listSegments[_segNum+1].SegmentID=="NM1") {
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
			if(_segCur.SegmentID!="LX") {
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
			while(_segCur.SegmentID=="N3") {
				_segNum++;
			}
		}

		///<summary>N4: Provider City, State, Zip Code.  Situational.  Repeat 1.  Guide page 159.</summary>
		private void ProcessLoop2310_N4() {
			if(_segCur.SegmentID!="N4") {
				return;
			}
			_segNum++;
		}

		///<summary>PER: Provider Communications Numbers.  Situational.  Repeat 2.  Guide page 161.</summary>
		private void ProcessLoop2310_PER() {
			while(_segCur.SegmentID=="PER") {
				_segNum++;
			}
		}

		///<summary>PLA: PRovider Change Reason.  Situational.  Repeat 1.  Guide page 164.</summary>
		private void ProcessLoop2310_PLA() {
			if(_segCur.SegmentID!="PLA") {
				return;
			}
			_segNum++;
		}

		///<summary>Loop 2320: Coordination of Benefits.  Repeat 5.  Guide page 23.</summary>
		private void ProcessLoop2320() {
			while(_segCur.SegmentID=="COB") {
				ProcessLoop2320_COB();
				ProcessLoop2320_REF();
				ProcessLoop2320_DTP();
				ProcessLoop2330();
			}
		}

		///<summary>COB: Coordination of Benefits.  Situational.  Repeat 1.  Guide page 166.</summary>
		private void ProcessLoop2320_COB() {
			if(_segCur.SegmentID!="COB") {
				return;
			}
			_segNum++;
		}

		///<summary>REF: Additional Coordination of Benefits Identifiers.  Situational.  Repeat 4.  Guide page 168.</summary>
		private void ProcessLoop2320_REF() {
			List <string> listRefQualifiers=new List<string>(new string[] { "60","6P","SY","ZZ" });
			while(_segCur.SegmentID=="REF" && listRefQualifiers.Contains(_segCur.Get(1))) {
				_segNum++;
			}
		}

		///<summary>DTP: Coordination of Benefits Eligibility Dates.  Situational.  Repeat 2.  Guide page 170.</summary>
		private void ProcessLoop2320_DTP() {
			while(_segCur.SegmentID=="DTP") {
				_segNum++;
			}
		}

		///<summary>Loop 2330: Coordination of Benefits Related Entity.  Repeat 3.  Guide page 23.</summary>
		private void ProcessLoop2330() {
			List <string> listIdentifierCodes=new List<string>(new string[] { "36","GW","IN" });
			while(_segCur.SegmentID=="NM1" && listIdentifierCodes.Contains(_segCur.Get(1))) {
				ProcessLoop2330_NM1();
				ProcessLoop2330_N3();
				ProcessLoop2330_N4();
				ProcessLoop2330_PER();
			}
		}

		///<summary>NM1: Coordination of Benefits Releated Entity.  Situational.  Repeat 1.  Guide page 171.</summary>
		private void ProcessLoop2330_NM1() {
			if(_segCur.SegmentID!="NM1") {
				return;
			}
			_segNum++;
		}

		///<summary>N3: Coordination of Benefits Related Entity Address.  Situational.  Repeat 1.  Guide page 173.</summary>
		private void ProcessLoop2330_N3() {
			if(_segCur.SegmentID!="N3") {
				return;
			}
			_segNum++;
		}

		///<summary>N4: Coordination of Benefits Other Insurance Company City, State, Zip Code.  Repeat 1.  Guide page 174.</summary>
		private void ProcessLoop2330_N4() {
			if(_segCur.SegmentID!="N4") {
				return;
			}
			_segNum++;
		}

		///<summary>PER: Administrative Communications Contact.  Situational.  Repeat 1.  Guide page 176.</summary>
		private void ProcessLoop2330_PER() {
			if(_segCur.SegmentID!="PER") {
				return;
			}
			_segNum++;
		}

		///<summary>LS: Additional Reporting Categories.  Situational.  Repeat 1.  Guide page 178.</summary>
		private void ProcessLoop2000_LS() {
			if(_segCur.SegmentID!="LS") {
				return;
			}
			_segNum++;
		}

		///<summary>Loop 2700: Member Reporting Categories.  Repeat >1.  Guide page 24.</summary>
		private void ProcessLoop2700() {
			while(_segCur.SegmentID=="LX") {
				ProcessLoop2700_LX();
				ProcessLoop2750();
			}
		}

		///<summary>LX: Member Reporting Categories.  Situational.  Repeat 1.  Guide page 179.</summary>
		private void ProcessLoop2700_LX() {
			if(_segCur.SegmentID!="LX") {
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
			if(_segCur.SegmentID!="N1") {
				return;
			}
			_segNum++;
		}

		///<summary>REF: Reporting Category Reference.  Situational.  Repeat 1.  Guide page 181.</summary>
		private void ProcessLoop2750_REF() {
			if(_segCur.SegmentID!="REF") {
				return;
			}
			_segNum++;
		}

		///<summary>DTP: Reporting Category Date.  Situational.  Repeat 1.  Guide page 183.</summary>
		private void ProcessLoop2750_DTP() {
			if(_segCur.SegmentID!="DTP") {
				return;
			}
			_segNum++;
		}

		///<summary>LE: Additional Reporting Categories Loop Termination.  Situational.  Repeat 1.  Guide page 185.</summary>
		private void ProcessLoop2000_LE() {
			if(_segCur.SegmentID!="LE") {
				return;
			}
			_segNum++;
		}

		#region Helpers

		///<summary>Converts a date in string format YYYYMMDD into a DateTime object.</summary>
		private DateTime DtmToDateTime(string strDtm) {
			DateTime dateTime=DateTime.MinValue;
			if(strDtm.Length>=8) {
				int dtmYear=int.Parse(strDtm.Substring(0,4));
				int dtmMonth=int.Parse(strDtm.Substring(4,2));
				int dtmDay=int.Parse(strDtm.Substring(6,2));
				dateTime=new DateTime(dtmYear,dtmMonth,dtmDay);
			}
			return dateTime;
		}

		#endregion Helpers

	}

	#region Helper Classes

	///<summary>Corresponds to an N1 segment.</summary>
	public class Hx834_Name {
		///<summary>N102</summary>
		private string Name;
		///<summary>N103</summary>
		private string IdCodeQualifier;
		///<summary>N104</summary>
		private string IdCode;

		public Hx834_Name(X12Segment seg) {
			Name=seg.Get(2);
			IdCodeQualifier=seg.Get(3);
			IdCode=seg.Get(4);
		}
	}

	///<summary>Loop 1000C</summary>
	public class Hx834_Broker {
		///<summary>Loop 1000C N1</summary>
		public Hx834_Name Name;
		///<summary>Loop 1100C ACT01</summary>
		public string AccountNumber1;
		///<summary>Loop 1100C ACT06</summary>
		public string AccountNumber2;
	}

	///<summary>Loop 2000</summary>
	public class Hx834_Member {
		///<summary>Loop 2000 INS01.  True if the member is a subscriber, false if member is a dependent.</summary>
		public bool IsSubscriber;
		///<summary>Loop 2000 INS02.  An alphanumeric code representing the relationship of the member to the insured.
		///Must be set to 18=Self when INS01=Y.</summary>
		public string InsRelationshipCode;
		///<summary>The insurance relationship to the subscriber.  Closest match to the value found in INS02.</summary>
		public Relat Relationship;
		///<summary>Loop 2000 INS03</summary>
		public string MaintTypeCode;
		///<summary>Maintenance reason.  Enum value of Loop 2000 INS03 (MaintReasonCode).</summary>
		public Hx834_MemberMaintReason MaintReason;
		///<summary>Loop 2000 INS04</summary>
		public string MaintReasonCode;
		///<summary>Loop 2000 INS05</summary>
		public string BenefitStatusCode;
		///<summary>Loop 2000 INS06-1</summary>
		public string MedicarePlanCode;
		///<summary>Loop 2000 INS06-2</summary>
		public string MedicareEligibilityReasonCode;
		///<summary>Loop 2000 INS07</summary>
		public string CobraQualifyingCode;
		///<summary>Loop 2000 INS08</summary>
		public string EmploymentStatusCode;
		///<summary>Loop 2000 INS09.  Corresponds exactly to patient.StudentStatus</summary>
		public string StudentStatusCode;
		///<summary>Loop 2000 INS10</summary>
		public bool IsHandicapped;
		///<summary>Loop 2000 INS12</summary>
		public DateTime DateDeath;
		///<summary>Loop 2000 INS13</summary>
		public bool IsInfoRestrictedAccess;
		///<summary>Loop 2000 INS17</summary>
		public int BirthSequenceNumber;
		///<summary>Loop 2000 REF02_1</summary>
		public string SubscriberId;
		///<summary>Loop 2000 REF02_2</summary>
		public string GroupPolicyNumber;
		///<summary>Loop 2000 REF02_3 (repeat 13)</summary>
		public List <Hx834_Ref> ListMemberSupplementalIds=new List<Hx834_Ref>();
		///<summary>Loop 2000 DTP (repeat 24)</summary>
		public List <Hx834_Dtp> ListMemberDates=new List<Hx834_Dtp>();
	}

	public enum Hx834_MemberMaintReason {
		///<summary>0 - Not used.  Place holder.</summary>
		None,
		///<summary>1</summary>
		Change,
		///<summary>2</summary>
		Addition,
		///<summary>3</summary>
		CancellationOrTermination,
		///<summary>4</summary>
		Reinstatement,
		///<summary>5</summary>
		AuditOrCompare,
	}

	///<summary>Corresponds to a REF segment.</summary>
	public class Hx834_Ref {
		public string ReferenceIdQualifier;
		public string ReferenceId;
	}

	///<summary>Corresponds to a DTP segment.</summary>
	public class Hx834_Dtp {
		public string DateTimeQualifier;
		public DateTime DateT;
	}

	#endregion Helper Classes
}