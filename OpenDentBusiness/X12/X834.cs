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
		///<summary>Loop ST BGN</summary>
		public X12_BGN BeginningSegment;
		///<summary>Loop ST REF</summary>
		public X12_REF TransactionSetPolicyNumber;
		///<summary>Loop ST DTP.  Repeat >1.</summary>
		public List<X12_DTP> ListFileEffectiveDates=new List<X12_DTP>();
		///<summary>Loop ST QTY</summary>
		public List <X12_QTY> ListTransactionSetControlTotals=new List<X12_QTY>();
		///<summary>Loop 1000A N1</summary>
		public X12_N1 SponsorName;
		///<summary>Loop 1000B N1</summary>
		public X12_N1 Payer;
		///<summary>Loop 1000C N1 and Loop 1100C ACT</summary>
		public List <Hx834_Broker> ListBrokers=new List<Hx834_Broker>();
		///<summary>Loop 2000</summary>
		public List <Hx834_Member> ListMembers=new List<Hx834_Member>();
		
		///<summary>Shortcut to get current segment based on _segNum.</summary>
		private X12Segment _segCur { get { return _listSegments[_segNum]; } }

		public X834(string messageText):base(messageText) {
			ReadMessage();
		}

		///<summary>See guide pages 22 and 208 for overview.</summary>
		private void ReadMessage() {
			ReadLoopGS();
		}

		///<summary>GS: Functional Group.  Required.  Repeat unlimited.  Guide page 208.</summary>
		private void ReadLoopGS() {
			for(int i=0;i<FunctGroups.Count;i++) {
				ReadLoopST(FunctGroups[i].Transactions);
			}
		}

		///<summary>ST: Transaction Set Header.  Required.  Repeat 1.  Guide pages 22, 31, 208.</summary>
		private void ReadLoopST(List <X12Transaction> listTrans) {
			for(int i=0;i<listTrans.Count;i++) {
				_listSegments=listTrans[i].Segments;
				_segNum=0;
				ReadLoopST_BGN();
				ReadLoopST_REF();
				ReadLoopST_DTP();
				ReadLoopST_QTY();
				ReadLoop1000A();
				ReadLoop1000B();
				ReadLoop1000C();
				ReadLoop2000();
			}
		}

		///<summary>BGN: Beginning Segment.  Required.  Repeat 1.  Guide page 32.</summary>
		private void ReadLoopST_BGN() {
			BeginningSegment=new X12_BGN(_segCur);
			_segNum++;
		}

		///<summary>REF: Transaction Set Policy Number.  Situational.  Repeat 1.  Guide page 36.</summary>
		private void ReadLoopST_REF() {
			if(_segCur.SegmentID!="REF") {
				return;
			}
			TransactionSetPolicyNumber=new X12_REF(_segCur);
			_segNum++;
		}

		///<summary>DTP: File Effictive Date.  Situational.  Repeat >1.  Guide page 37.</summary>
		private void ReadLoopST_DTP() {
			ListFileEffectiveDates.Clear();
			while(_segCur.SegmentID=="DTP") {
				ListFileEffectiveDates.Add(new X12_DTP(_segCur));
				_segNum++;
			}
		}

		///<summary>QTY: Transaction Set Control Totals.  Situational.  Repeat 3.  Guide page 38.</summary>
		private void ReadLoopST_QTY() {
			ListTransactionSetControlTotals.Clear();
			while(_segCur.SegmentID=="QTY") {
				ListTransactionSetControlTotals.Add(new X12_QTY(_segCur));
				_segNum++;
			}
		}

		///<summary>Loop 1000A: Sponsor Name.  Repeat 1.  Guide page 22.</summary>
		private void ReadLoop1000A() {
			ReadLoop1000A_N1();
		}

		///<summary>N1: Sponsor Name.  Required.  Repeat 1.  Guide page 39.</summary>
		private void ReadLoop1000A_N1() {
			SponsorName=new X12_N1(_segCur);
			_segNum++;
		}

		///<summary>Loop 1000B: Payer.  Repeat 1.  Guide page 22.</summary>
		private void ReadLoop1000B() {
			ReadLoop1000B_N1();
		}

		///<summary>N1: Payer.  Required.  Repeat 1.  Guide page 41.</summary>
		private void ReadLoop1000B_N1() {
			Payer=new X12_N1(_segCur);
			_segNum++;
		}

		///<summary>Loop 1000C: TPA/Broker Name.  Repeat 2.  Guide page 22.</summary>
		private void ReadLoop1000C() {
			ListBrokers.Clear();
			while(_segCur.SegmentID=="N1") {
				Hx834_Broker broker=new Hx834_Broker();
				ReadLoop1000C_N1(broker);
				ReadLoop1100C(broker);
				ListBrokers.Add(broker);
			}
		}

		///<summary>N1: TPA/Broker Name.  Situational.  Repeat 1.  Guide page 43.</summary>
		private void ReadLoop1000C_N1(Hx834_Broker broker) {
			if(_segCur.SegmentID!="N1") {
				return;
			}
			broker.Name=new X12_N1(_segCur);
			_segNum++;
		}

		///<summary>Loop 1100C: TPA/Broker Account.  Repeat 1.  Guide page 22.</summary>
		private void ReadLoop1100C(Hx834_Broker broker) {
			ReadLoop1100C_ACT(broker);
		}

		///<summary>ACT: TPA/Broker Account Information.  Situational.  Repeat 1.  Guide page 45.</summary>
		private void ReadLoop1100C_ACT(Hx834_Broker broker) {
			if(_segCur.SegmentID!="ACT") {
				return;
			}
			broker.TpaBrokerAccountInformation=new X12_ACT(_segCur);
			_segNum++;
		}

		///<summary>Loop 2000: Member Level Detail.  Repeat >1.  Guide page 22.</summary>
		private void ReadLoop2000() {
			ListMembers.Clear();
			while(_segCur.SegmentID=="INS") {
				Hx834_Member member=new Hx834_Member();
				ReadLoop2000_INS(member);
				ReadLoop2000_REF_1(member);
				ReadLoop2000_REF_2(member);
				ReadLoop2000_REF_3(member);
				ReadLoop2000_DTP(member);
				ReadLoop2100A(member);
				ReadLoop2100B(member);
				ReadLoop2100C(member);
				ReadLoop2100D(member);
				ReadLoop2100E(member);
				ReadLoop2100F(member);
				ReadLoop2100G(member);
				ReadLoop2100H(member);
				ReadLoop2200(member);
				ReadLoop2300(member);
				ReadLoop2000_LS();
				ReadLoop2700();
				ReadLoop2000_LE();
				ListMembers.Add(member);
			}
		}

		///<summary>INS: Member Level Detail.  Required.  Repeat 1.  Guide page 47.</summary>
		private void ReadLoop2000_INS(Hx834_Member member) {
			member.MemberLevelDetail=new X12_INS(_segCur);
			_segNum++;
		}

		///<summary>REF: Subscriber Identifier.  Required.  Repeat 1.  Guide page 55.</summary>
		private void ReadLoop2000_REF_1(Hx834_Member member) {
			member.SubscriberIdentifier=new X12_REF(_segCur);
			_segNum++;
		}

		///<summary>REF: Member Policy Number.  Situational.  Repeat 1.  Guide page 56.</summary>
		private void ReadLoop2000_REF_2(Hx834_Member member) {
			if(_segCur.SegmentID!="REF" || _segCur.Get(1)!="1L") {
				return;
			}
			member.MemberPolicyNumber=new X12_REF(_segCur);
			_segNum++;
		}

		///<summary>REF: Member Supplemental Identifier.  Situational.  Repeat 13.  Guide page 57.</summary>
		private void ReadLoop2000_REF_3(Hx834_Member member) {
			member.ListMemberSupplementalIdentifiers.Clear();
			while(_segCur.SegmentID=="REF") {
				X12_REF href=new X12_REF(_segCur);
				member.ListMemberSupplementalIdentifiers.Add(href);
				_segNum++;
			}
		}

		///<summary>DTP: Member Level Dates.  Situational.  Repeat 24.  Guide page 59.</summary>
		private void ReadLoop2000_DTP(Hx834_Member member) {
			member.ListMemberLevelDates.Clear();
			while(_segCur.SegmentID=="DTP") {
				X12_DTP dtp=new X12_DTP(_segCur);
				member.ListMemberLevelDates.Add(dtp);
				_segNum++;
			}
		}

		///<summary>Loop 2100A: Member Name.  Repeat 1.  Guide page 22.</summary>
		private void ReadLoop2100A(Hx834_Member member) {
			ReadLoop2100A_NM1(member);
			ReadLoop2100A_PER(member);
			ReadLoop2100A_N3(member);
			ReadLoop2100A_N4(member);
			ReadLoop2100A_DMG(member);
			ReadLoop2100A_EC(member);
			ReadLoop2100A_ICM(member);
			ReadLoop2100A_AMT(member);
			ReadLoop2100A_HLH(member);
			ReadLoop2100A_LUI(member);
		}

		///<summary>NM1: Member Name.  Required.  Repeat 1.  Guide page 62.</summary>
		private void ReadLoop2100A_NM1(Hx834_Member member) {
			member.MemberName=new X12_NM1(_segCur);
			_segNum++;
		}

		///<summary>PER: Member Communications Numbers.  Situational.  Repeat 1.  Guide page 65.</summary>
		private void ReadLoop2100A_PER(Hx834_Member member) {
			if(_segCur.SegmentID!="PER") {
				return;
			}
			member.MemberCommunicationsNumbers=new X12_PER(_segCur);
			_segNum++;
		}

		///<summary>N3: Member Residence Street Address.  Situational.  Repeat 1.  Guide page 68.</summary>
		private void ReadLoop2100A_N3(Hx834_Member member) {
			if(_segCur.SegmentID!="N3") {
				return;
			}
			member.MemberResidenceStreetAddress=new X12_N3(_segCur);
			_segNum++;
		}
		
		///<summary>N4: Member City, State, Zip Code.  Situational.  Repeat 1.  Guide page 69.</summary>
		private void ReadLoop2100A_N4(Hx834_Member member) {
			if(_segCur.SegmentID!="N4") {
				return;
			}
			member.MemberCityStateZipCode=new X12_N4(_segCur);
			_segNum++;
		}

		///<summary>DMG: Member Demographics.  Situational.  Repeat 1.  Guide page 72.</summary>
		private void ReadLoop2100A_DMG(Hx834_Member member) {
			if(_segCur.SegmentID!="DMG") {
				return;
			}
			member.MemberDemographics=new X12_DMG(_segCur);
			_segNum++;
		}

		///<summary>EC: Employment Class.  Situational.  Repeat >1.  Guide page 76.</summary>
		private void ReadLoop2100A_EC(Hx834_Member member) {
			member.ListEmploymentClass.Clear();
			while(_segCur.SegmentID=="EC") {
				member.ListEmploymentClass.Add(new X12_EC(_segCur));
				_segNum++;
			}
		}

		///<summary>ICM: Member Income.  Situational.  Repeat 1.  Guide page 79.</summary>
		private void ReadLoop2100A_ICM(Hx834_Member member) {
			if(_segCur.SegmentID!="ICM") {
				return;
			}
			member.MemberIncome=new X12_ICM(_segCur);
			_segNum++;
		}

		///<summary>AMT: Member Policy Amounts.  Situational.  Repeat 7.  Guide page 81.</summary>
		private void ReadLoop2100A_AMT(Hx834_Member member) {
			member.ListMemberPolicyAmounts.Clear();
			while(_segCur.SegmentID=="AMT") {
				member.ListMemberPolicyAmounts.Add(new X12_AMT(_segCur));
				_segNum++;
			}
		}

		///<summary>HLH: Member Health Information.  Situational.  Repeat 1.  Guide page 82.</summary>
		private void ReadLoop2100A_HLH(Hx834_Member member) {
			if(_segCur.SegmentID!="HLH") {
				return;
			}
			member.MemberHealthInformation=new X12_HLH(_segCur);
			_segNum++;
		}

		///<summary>LUI: Member Language.  Situational.  Repeat >1.  Guide page 84.</summary>
		private void ReadLoop2100A_LUI(Hx834_Member member) {
			member.ListMemberLanguages.Clear();
			while(_segCur.SegmentID=="LUI") {
				member.ListMemberLanguages.Add(new X12_LUI(_segCur));
				_segNum++;
			}
		}

		///<summary>Loop 2100B: Incorrect Member Name.  Repeat 1.  Guide page 22.</summary>
		private void ReadLoop2100B(Hx834_Member member) {
			if(_segCur.SegmentID!="NM1" || _segCur.Get(1)!="70") {
				return;
			}
			ReadLoop2100B_NM1(member);
			ReadLoop2100B_DMG(member);
		}

		///<summary>NM1: Incorrect Member Name.  Situational.  Repeat 1.  Guide page 86.</summary>
		private void ReadLoop2100B_NM1(Hx834_Member member) {
			if(_segCur.SegmentID!="NM1") {
				return;
			}
			member.IncorrectMemberName=new X12_NM1(_segCur);
			_segNum++;
		}

		///<summary>DMG: Incorrect Member Demographics.  Situational.  Repeat 1.  Guide page 89.</summary>
		private void ReadLoop2100B_DMG(Hx834_Member member) {
			if(_segCur.SegmentID!="DMG") {
				return;
			}
			member.IncorrectMemberDemographics=new X12_DMG(_segCur);
			_segNum++;
		}

		///<summary>Loop 2100C: Member Mailing Address.  Repeat 1.  Guide page 22.</summary>
		private void ReadLoop2100C(Hx834_Member member) {
			if(_segCur.SegmentID!="NM1" || _segCur.Get(1)!="31") {
				return;
			}
			ReadLoop2100C_NM1(member);
			ReadLoop2100C_N3(member);
			ReadLoop2100C_N4(member);
		}

		///<summary>NM1: Member Mailing Address.  Situational.  Repeat 1.  Guide page 93.</summary>
		private void ReadLoop2100C_NM1(Hx834_Member member) {
			if(_segCur.SegmentID!="NM1") {
				return;
			}
			member.MemberMailingAddress=new X12_NM1(_segCur);
			_segNum++;
		}

		///<summary>N3: Member Mail Street Address.  Required.  Repeat 1.  Guide page 95.</summary>
		private void ReadLoop2100C_N3(Hx834_Member member) {
			member.MemberMailStreetAddress=new X12_N3(_segCur);
			_segNum++;
		}

		///<summary>N4: Member Mail City, State, Zip Code.  Required.  Repeat 1.  Guide page 96.</summary>
		private void ReadLoop2100C_N4(Hx834_Member member) {
			member.MemberMailCityStateZipCode=new X12_N4(_segCur);
			_segNum++;
		}

		///<summary>Loop 2100D: Member Employer.  Repeat 3.  Guide page 23.</summary>
		private void ReadLoop2100D(Hx834_Member member) {
			member.ListMemberEmployers.Clear();
			while(_segCur.SegmentID=="NM1" && _segCur.Get(1)=="36") {
				Hx834_Employer employer=new Hx834_Employer();
				ReadLoop2100D_NM1(employer);
				ReadLoop2100D_PER(employer);
				ReadLoop2100D_N3(employer);
				ReadLoop2100D_N4(employer);
				member.ListMemberEmployers.Add(employer);
			}
		}

		///<summary>NM1: Member Employer.  Situational.  Repeat 1.  Guide page 98.</summary>
		private void ReadLoop2100D_NM1(Hx834_Employer employer) {
			if(_segCur.SegmentID!="NM1") {
				return;
			}
			employer.MemberEmployer=new X12_NM1(_segCur);
			_segNum++;
		}

		///<summary>PER: Member Employer Communications Numbers.  Situational.  Repeat 1.  Guide page 101.</summary>
		private void ReadLoop2100D_PER(Hx834_Employer employer) {
			if(_segCur.SegmentID!="PER") {
				return;
			}
			employer.MemberEmployerCommunicationsNumbers=new X12_PER(_segCur);
			_segNum++;
		}

		///<summary>N3: Member Employer Street Address.  Situational.  Repeat 1.  Guide page 104.</summary>
		private void ReadLoop2100D_N3(Hx834_Employer employer) {
			if(_segCur.SegmentID!="N3") {
				return;
			}
			employer.MemberEmployerStreetAddress=new X12_N3(_segCur);
			_segNum++;
		}

		///<summary>N4: Member Employer City, State, Zip Code.  Situational.  Repeat 1.  Guide page 105.</summary>
		private void ReadLoop2100D_N4(Hx834_Employer employer) {
			if(_segCur.SegmentID!="N4") {
				return;
			}
			employer.MemberEmployerCityStateZipCode=new X12_N4(_segCur);
			_segNum++;
		}

		///<summary>Loop 2100E: Member School.  Repeat 3.  Guide page 23.</summary>
		private void ReadLoop2100E(Hx834_Member member) {
			member.ListMemberSchools.Clear();
			while(_segCur.SegmentID=="NM1" && _segCur.Get(1)=="M8") {
				Hx834_School school=new Hx834_School();
				ReadLoop2100E_NM1(school);
				ReadLoop2100E_PER(school);
				ReadLoop2100E_N3(school);
				ReadLoop2100E_N4(school);
				member.ListMemberSchools.Add(school);
			}
		}

		///<summary>NM1: Member School.  Situational.  Repeat 1.  Guide page 107.</summary>
		private void ReadLoop2100E_NM1(Hx834_School school) {
			if(_segCur.SegmentID!="NM1") {
				return;
			}
			school.MemberSchool=new X12_NM1(_segCur);
			_segNum++;
		}

		///<summary>PER: Member School Communications Numbers.  Situational.  Repeat 1.  Guide page 109.</summary>
		private void ReadLoop2100E_PER(Hx834_School school) {
			if(_segCur.SegmentID!="PER") {
				return;
			}
			school.MemberSchoolCommunicationsNumbers=new X12_PER(_segCur);
			_segNum++;
		}

		///<summary>N3: Member School Street Address.  Situational.  Repeat 1.  Guide page 112.</summary>
		private void ReadLoop2100E_N3(Hx834_School school) {
			if(_segCur.SegmentID!="N3") {
				return;
			}
			school.MemberSchoolStreetAddress=new X12_N3(_segCur);
			_segNum++;
		}

		///<summary>N4: Member School City, State, Zip Code.  Repeat 1.  Guide page 113.</summary>
		private void ReadLoop2100E_N4(Hx834_School school) {
			if(_segCur.SegmentID!="N4") {
				return;
			}
			school.MemberSchoolCityStateZipCode=new X12_N4(_segCur);
			_segNum++;
		}

		///<summary>Loop 2100F: Custodial Parent.  Repeat 1.  Guide page 23.</summary>
		private void ReadLoop2100F(Hx834_Member member) {
			if(_segCur.SegmentID!="NM1" || _segCur.Get(1)=="S3") {
				return;
			}
			ReadLoop2100F_NM1(member);
			ReadLoop2100F_PER(member);
			ReadLoop2100F_N3(member);
			ReadLoop2100F_N4(member);
		}

		///<summary>NM1: Custodial Parent.  Situational.  Repeat 1.  Guide page 115.</summary>
		private void ReadLoop2100F_NM1(Hx834_Member member) {
			if(_segCur.SegmentID!="NM1") {
				return;
			}
			member.CustodialParent=new X12_NM1(_segCur);
			_segNum++;
		}

		///<summary>PER: Custodial Parent Communications Numbers.  Situational.  Repeat 1.  Guide page 118.</summary>
		private void ReadLoop2100F_PER(Hx834_Member member) {
			if(_segCur.SegmentID!="PER") {
				return;
			}
			member.CustodialParentCommunicationsNumbers=new X12_PER(_segCur);
			_segNum++;
		}

		///<summary>N3: Custodial Parent Street Address.  Situational.  Repeat 1.  Guide page 121.</summary>
		private void ReadLoop2100F_N3(Hx834_Member member) {
			if(_segCur.SegmentID!="N3") {
				return;
			}
			member.CustodialParentStreetAddress=new X12_N3(_segCur);
			_segNum++;
		}

		///<summary>N4: Custodial Parent City, State, Zip Code.  Situational.  Repeat 1.  Guide page 122.</summary>
		private void ReadLoop2100F_N4(Hx834_Member member) {
			if(_segCur.SegmentID!="N4") {
				return;
			}
			member.CustodialParentCityStateZipCode=new X12_N4(_segCur);
			_segNum++;
		}

		///<summary>Loop 2100G: Responsible Person.  Repeat 13.  Guide page 23.</summary>
		private void ReadLoop2100G(Hx834_Member member) {
			member.ListResponsiblePerson.Clear();
			List <string> listNm1EntityCodes=new List<string>(new string[] { "6Y","9K","E1","EI","EXS","GB","GD","J6","LR","QD","S1","TZ","X4" });
			while(_segCur.SegmentID=="NM1" && listNm1EntityCodes.Contains(_segCur.Get(1))) {
				Hx834_ResponsiblePerson person=new Hx834_ResponsiblePerson();
				ReadLoop2100G_NM1(person);
				ReadLoop2100G_PER(person);
				ReadLoop2100G_N3(person);
				ReadLoop2100G_N4(person);
				member.ListResponsiblePerson.Add(person);
			}
		}

		///<summary>NM1: Responsible Person.  Situational.  Repeat 1.  Guide page 124.</summary>
		private void ReadLoop2100G_NM1(Hx834_ResponsiblePerson person) {
			if(_segCur.SegmentID!="NM1") {
				return;
			}
			person.ResponsiblePerson=new X12_NM1(_segCur);
			_segNum++;
		}

		///<summary>PER: Responsible Person Communications Numbers.  Situational.  Repeat 1.  Guide page 127.</summary>
		private void ReadLoop2100G_PER(Hx834_ResponsiblePerson person) {
			if(_segCur.SegmentID!="PER") {
				return;
			}
			person.ResponsiblePersonCommunicationsNumbers=new X12_PER(_segCur);
			_segNum++;
		}

		///<summary>N3: Responsible Person Street Address.  Situational.  Repeat 1.  Guide page 130.</summary>
		private void ReadLoop2100G_N3(Hx834_ResponsiblePerson person) {
			if(_segCur.SegmentID!="N3") {
				return;
			}
			person.ResponsiblePersonStreetAddress=new X12_N3(_segCur);
			_segNum++;
		}

		///<summary>N4: Responsible Person City, State, Zip Code.  Situational.  Repeat 1.  Guide page 131.</summary>
		private void ReadLoop2100G_N4(Hx834_ResponsiblePerson person) {
			if(_segCur.SegmentID!="N4") {
				return;
			}
			person.ResponsiblePersonCityStateZipCode=new X12_N4(_segCur);
			_segNum++;
		}

		///<summary>Loop 2100H: Drop Off Location.  Repeat 1.  Guide page 23.</summary>
		private void ReadLoop2100H(Hx834_Member member) {
			if(_segCur.SegmentID!="NM1" || _segCur.Get(1)=="45") {
				return;
			}
			ReadLoop2100H_NM1(member);
			ReadLoop2100H_N3(member);
			ReadLoop2100H_N4(member);
		}

		///<summary>NM1: Drop Off Location.  Situational.  Repeat 1.  Guide page 133.</summary>
		private void ReadLoop2100H_NM1(Hx834_Member member) {
			if(_segCur.SegmentID!="NM1") {
				return;
			}
			member.DropOffLocation=new X12_NM1(_segCur);
			_segNum++;
		}

		///<summary>N3: Drop Off Location Street Address.  Situational.  Repeat 1.  Guide page 135.</summary>
		private void ReadLoop2100H_N3(Hx834_Member member) {
			if(_segCur.SegmentID!="N3") {
				return;
			}
			member.DropOffLocationStreetAddress=new X12_N3(_segCur);
			_segNum++;
		}

		///<summary>N4: Drop Off Location City, State, Zip Code.  Situational.  Repeat 1.  Guide page 136.</summary>
		private void ReadLoop2100H_N4(Hx834_Member member) {
			if(_segCur.SegmentID!="N4") {
				return;
			}
			member.DropOffLocationCityStateZipCode=new X12_N4(_segCur);
			_segNum++;
		}

		///<summary>Loop 2200: Disability Information.  Repeat >1.  Guide page 23.</summary>
		private void ReadLoop2200(Hx834_Member member) {
			member.ListDisabilityInformation.Clear();
			while(_segCur.SegmentID=="DSB") {
				Hx834_DisabilityInformation disabilityInfo=new Hx834_DisabilityInformation();
				ReadLoop2200_DSB(disabilityInfo);
				ReadLoop2200_DTP(disabilityInfo);
				member.ListDisabilityInformation.Add(disabilityInfo);
			}
		}

		///<summary>DSB: Disability Information.  Situational.  Repeat 1.  Guide page 138.</summary>
		private void ReadLoop2200_DSB(Hx834_DisabilityInformation disabilityInfo) {
			if(_segCur.SegmentID!="DSB") {
				return;
			}
			disabilityInfo.DisabilityInformation=new X12_DSB(_segCur);
			_segNum++;
		}

		///<summary>DTP: Disability Eligibility Dates.  Situational.  Repeat 2.  Guide page 140.</summary>
		private void ReadLoop2200_DTP(Hx834_DisabilityInformation disabilityInfo) {
			disabilityInfo.ListDisabilityEligibilityDates.Clear();
			while(_segCur.SegmentID=="DTP") {
				disabilityInfo.ListDisabilityEligibilityDates.Add(new X12_DTP(_segCur));
				_segNum++;
			}
		}

		///<summary>Loop 2300: Health Coverage.  Repeat 99.  Guide page 23.</summary>
		private void ReadLoop2300(Hx834_Member member) {
			member.ListHealthCoverage.Clear();
			while(_segCur.SegmentID=="HD") {
				Hx834_HealthCoverage healthCoverage=new Hx834_HealthCoverage();
				ReadLoop2300_HD(healthCoverage);
				ReadLoop2300_DTP(healthCoverage);
				ReadLoop2300_AMT(healthCoverage);
				ReadLoop2300_REF_1(healthCoverage);
				ReadLoop2300_REF_2(healthCoverage);
				ReadLoop2300_IDC(healthCoverage);
				ReadLoop2310(healthCoverage);
				ReadLoop2320(healthCoverage);
				member.ListHealthCoverage.Add(healthCoverage);
			}
		}

		///<summary>HD: Health Coverage.  Situational.  Repeat 1.  Guide page 141.</summary>
		private void ReadLoop2300_HD(Hx834_HealthCoverage healthCoverage) {
			if(_segCur.SegmentID!="HD") {
				return;
			}
			healthCoverage.HealthCoverage=new X12_HD(_segCur);
			_segNum++;
		}

		///<summary>DTP: Health Coverage Dates.  Required.  Repeat 6.  Guide page 145.</summary>
		private void ReadLoop2300_DTP(Hx834_HealthCoverage healthCoverage) {
			healthCoverage.ListHealthCoverageDates.Clear();
			while(_segCur.SegmentID=="DTP") {
				healthCoverage.ListHealthCoverageDates.Add(new X12_DTP(_segCur));
				_segNum++;
			}
		}

		///<summary>AMT: Health Coverage Policy.  Situational.  Repeat 9.  Guide page 147.</summary>
		private void ReadLoop2300_AMT(Hx834_HealthCoverage healthCoverage) {
			healthCoverage.ListHealthCoveragePolicies.Clear();
			while(_segCur.SegmentID=="AMT") {
				healthCoverage.ListHealthCoveragePolicies.Add(new X12_AMT(_segCur));
				_segNum++;
			}
		}

		///<summary>REF: Health Coverage Policy Number.  Situational.  Repeat 14.  Guide page 148.</summary>
		private void ReadLoop2300_REF_1(Hx834_HealthCoverage healthCoverage) {
			healthCoverage.ListHealthCoveragePolicyNumbers.Clear();
			List <string> listRefQualifiers=new List<string>(new string[] { "17","1L","9V","CE","E8","M7","PID","RB","X9","XM","XX1","XX2","ZX","ZZ" });
			while(_segCur.SegmentID=="REF" && listRefQualifiers.Contains(_segCur.Get(1))) {
				healthCoverage.ListHealthCoveragePolicyNumbers.Add(new X12_REF(_segCur));
				_segNum++;
			}
		}

		///<summary>REF: Prior Coverage Months.  Situational.  Repeat 1.  Guide page 150.</summary>
		private void ReadLoop2300_REF_2(Hx834_HealthCoverage healthCoverage) {
			if(_segCur.SegmentID!="REF" || _segCur.Get(1)!="QQ") {
				return;
			}
			healthCoverage.PriorCoverageMonths=new X12_REF(_segCur);
			_segNum++;
		}

		///<summary>IDC: IDentification Card.  Situational.  Repeat 3.  Guide page 152.</summary>
		private void ReadLoop2300_IDC(Hx834_HealthCoverage healthCoverage) {
			healthCoverage.ListIdentificationCards.Clear();
			while(_segCur.SegmentID=="IDC") {
				healthCoverage.ListIdentificationCards.Add(new X12_IDC(_segCur));
				_segNum++;
			}
		}

		///<summary>Loop 2310: Provider Information.  Repeat 30.  Guide page 23.</summary>
		private void ReadLoop2310(Hx834_HealthCoverage healthCoverage) {
			healthCoverage.ListProviderInformation.Clear();
			//There are two different LX segments which could be at this spot.
			//The LX segments have the same simple format, so we have to look at the following segment to figure out which LX we are looking at.
			while(_segCur.SegmentID=="LX" && (_segNum+1) < _listSegments.Count && _listSegments[_segNum+1].SegmentID=="NM1") {
				Hx834_Provider prov=new Hx834_Provider();
				ReadLoop2310_LX(prov);
				ReadLoop2310_NM1(prov);
				ReadLoop2310_N3(prov);
				ReadLoop2310_N4(prov);
				ReadLoop2310_PER(prov);
				ReadLoop2310_PLA(prov);
				healthCoverage.ListProviderInformation.Add(prov);
			}
		}

		///<summary>LX: Provider Information.  Situational.  Repeat 1.  Guide page 154.</summary>
		private void ReadLoop2310_LX(Hx834_Provider prov) {
			if(_segCur.SegmentID!="LX") {
				return;
			}
			prov.ProviderInformation=new X12_LX(_segCur);
			_segNum++;
		}

		///<summary>NM1: Provider Name.  Required.  Repeat 1.  Guide page 155.</summary>
		private void ReadLoop2310_NM1(Hx834_Provider prov) {
			prov.ProviderName=new X12_NM1(_segCur);
			_segNum++;
		}

		///<summary>N3: Provider Address. Situational.  Repeat 2.  Guide page 158.</summary>
		private void ReadLoop2310_N3(Hx834_Provider prov) {
			prov.ListProviderAddresses.Clear();
			while(_segCur.SegmentID=="N3") {
				prov.ListProviderAddresses.Add(new X12_N3(_segCur));
				_segNum++;
			}
		}

		///<summary>N4: Provider City, State, Zip Code.  Situational.  Repeat 1.  Guide page 159.</summary>
		private void ReadLoop2310_N4(Hx834_Provider prov) {
			if(_segCur.SegmentID!="N4") {
				return;
			}
			prov.ProviderCityStateZipCode=new X12_N4(_segCur);
			_segNum++;
		}

		///<summary>PER: Provider Communications Numbers.  Situational.  Repeat 2.  Guide page 161.</summary>
		private void ReadLoop2310_PER(Hx834_Provider prov) {
			prov.ListProviderCommunicationsNumbers.Clear();
			while(_segCur.SegmentID=="PER") {
				prov.ListProviderCommunicationsNumbers.Add(new X12_PER(_segCur));
				_segNum++;
			}
		}

		///<summary>PLA: PRovider Change Reason.  Situational.  Repeat 1.  Guide page 164.</summary>
		private void ReadLoop2310_PLA(Hx834_Provider prov) {
			if(_segCur.SegmentID!="PLA") {
				return;
			}
			prov.ProviderChangeReason=new X12_PLA(_segCur);
			_segNum++;
		}

		///<summary>Loop 2320: Coordination of Benefits.  Repeat 5.  Guide page 23.</summary>
		private void ReadLoop2320(Hx834_HealthCoverage healthCoverage) {
			healthCoverage.ListCoordinationOfBeneifts.Clear();
			while(_segCur.SegmentID=="COB") {
				Hx834_Cob cob=new Hx834_Cob();
				ReadLoop2320_COB();
				ReadLoop2320_REF();
				ReadLoop2320_DTP();
				ReadLoop2330();
				healthCoverage.ListCoordinationOfBeneifts.Add(cob);
			}
		}

		///<summary>COB: Coordination of Benefits.  Situational.  Repeat 1.  Guide page 166.</summary>
		private void ReadLoop2320_COB() {
			if(_segCur.SegmentID!="COB") {
				return;
			}
			_segNum++;
		}

		///<summary>REF: Additional Coordination of Benefits Identifiers.  Situational.  Repeat 4.  Guide page 168.</summary>
		private void ReadLoop2320_REF() {
			List <string> listRefQualifiers=new List<string>(new string[] { "60","6P","SY","ZZ" });
			while(_segCur.SegmentID=="REF" && listRefQualifiers.Contains(_segCur.Get(1))) {
				_segNum++;
			}
		}

		///<summary>DTP: Coordination of Benefits Eligibility Dates.  Situational.  Repeat 2.  Guide page 170.</summary>
		private void ReadLoop2320_DTP() {
			while(_segCur.SegmentID=="DTP") {
				_segNum++;
			}
		}

		///<summary>Loop 2330: Coordination of Benefits Related Entity.  Repeat 3.  Guide page 23.</summary>
		private void ReadLoop2330() {
			List <string> listIdentifierCodes=new List<string>(new string[] { "36","GW","IN" });
			while(_segCur.SegmentID=="NM1" && listIdentifierCodes.Contains(_segCur.Get(1))) {
				ReadLoop2330_NM1();
				ReadLoop2330_N3();
				ReadLoop2330_N4();
				ReadLoop2330_PER();
			}
		}

		///<summary>NM1: Coordination of Benefits Releated Entity.  Situational.  Repeat 1.  Guide page 171.</summary>
		private void ReadLoop2330_NM1() {
			if(_segCur.SegmentID!="NM1") {
				return;
			}
			_segNum++;
		}

		///<summary>N3: Coordination of Benefits Related Entity Address.  Situational.  Repeat 1.  Guide page 173.</summary>
		private void ReadLoop2330_N3() {
			if(_segCur.SegmentID!="N3") {
				return;
			}
			_segNum++;
		}

		///<summary>N4: Coordination of Benefits Other Insurance Company City, State, Zip Code.  Repeat 1.  Guide page 174.</summary>
		private void ReadLoop2330_N4() {
			if(_segCur.SegmentID!="N4") {
				return;
			}
			_segNum++;
		}

		///<summary>PER: Administrative Communications Contact.  Situational.  Repeat 1.  Guide page 176.</summary>
		private void ReadLoop2330_PER() {
			if(_segCur.SegmentID!="PER") {
				return;
			}
			_segNum++;
		}

		///<summary>LS: Additional Reporting Categories.  Situational.  Repeat 1.  Guide page 178.</summary>
		private void ReadLoop2000_LS() {
			if(_segCur.SegmentID!="LS") {
				return;
			}
			_segNum++;
		}

		///<summary>Loop 2700: Member Reporting Categories.  Repeat >1.  Guide page 24.</summary>
		private void ReadLoop2700() {
			while(_segCur.SegmentID=="LX") {
				ReadLoop2700_LX();
				ReadLoop2750();
			}
		}

		///<summary>LX: Member Reporting Categories.  Situational.  Repeat 1.  Guide page 179.</summary>
		private void ReadLoop2700_LX() {
			if(_segCur.SegmentID!="LX") {
				return;
			}
			_segNum++;
		}

		///<summary>Loop 2750: Reporting Category.  Repeat 1.  Guide page 24.</summary>
		private void ReadLoop2750() {
			ReadLoop2750_N1();
			ReadLoop2750_REF();
			ReadLoop2750_DTP();
		}

		///<summary>N1: Reporting Category.  Situational.  Repeat 1.  Guide page 180.</summary>
		private void ReadLoop2750_N1() {
			if(_segCur.SegmentID!="N1") {
				return;
			}
			_segNum++;
		}

		///<summary>REF: Reporting Category Reference.  Situational.  Repeat 1.  Guide page 181.</summary>
		private void ReadLoop2750_REF() {
			if(_segCur.SegmentID!="REF") {
				return;
			}
			_segNum++;
		}

		///<summary>DTP: Reporting Category Date.  Situational.  Repeat 1.  Guide page 183.</summary>
		private void ReadLoop2750_DTP() {
			if(_segCur.SegmentID!="DTP") {
				return;
			}
			_segNum++;
		}

		///<summary>LE: Additional Reporting Categories Loop Termination.  Situational.  Repeat 1.  Guide page 185.</summary>
		private void ReadLoop2000_LE() {
			if(_segCur.SegmentID!="LE") {
				return;
			}
			_segNum++;
		}

	}

	#region Helper Classes

	///<summary>Loop 1000C</summary>
	public class Hx834_Broker {
		///<summary>Loop 1000C N1</summary>
		public X12_N1 Name;
		///<summary>Loop 1100C</summary>
		public X12_ACT TpaBrokerAccountInformation;
	}

	///<summary>Loop 2320</summary>
	public class Hx834_Cob {

	}

	///<summary>Loop 2200</summary>
	public class Hx834_DisabilityInformation {
		///<summary>Loop 2200 DSB</summary>
		public X12_DSB DisabilityInformation;
		///<summary>Loop 2200 DTP.  Repeat 2.</summary>
		public List <X12_DTP> ListDisabilityEligibilityDates=new List<X12_DTP>();
	}

	///<summary>Loop 2100D</summary>
	public class Hx834_Employer {
		///<summary>Loop 2100D NM1</summary>
		public X12_NM1 MemberEmployer;
		///<summary>Loop 2100D PER</summary>
		public X12_PER MemberEmployerCommunicationsNumbers;
		///<summary>Loop 2100D N3</summary>
		public X12_N3 MemberEmployerStreetAddress;
		///<summary>Loop 2100D N4</summary>
		public X12_N4 MemberEmployerCityStateZipCode;
	}

	///<summary>Loop 2300</summary>
	public class Hx834_HealthCoverage {
		///<summary>Loop 2300 HD</summary>
		public X12_HD HealthCoverage;
		///<summary>Loop 2300 DTP.  Repeat 6.</summary>
		public List<X12_DTP> ListHealthCoverageDates=new List<X12_DTP>();
		///<summary>Loop 2300 AMT.  Repeat 9.</summary>
		public List<X12_AMT> ListHealthCoveragePolicies=new List<X12_AMT>();
		///<summary>Loop 2300 REF_1.  Repeat 14.</summary>
		public List<X12_REF> ListHealthCoveragePolicyNumbers=new List<X12_REF>();
		///<summary>Loop 2300 REF_2.</summary>
		public X12_REF PriorCoverageMonths;
		///<summary>Loop2300 IDC.  Repeat 3.</summary>
		public List<X12_IDC> ListIdentificationCards=new List<X12_IDC>();
		///<summary>Loop 2310.  Repeat 30.</summary>
		public List<Hx834_Provider> ListProviderInformation=new List<Hx834_Provider>();
		///<summary>Loop 2320.  Repeat 5.</summary>
		public List<Hx834_Cob> ListCoordinationOfBeneifts=new List<Hx834_Cob>();
	}

	///<summary>Loop 2000</summary>
	public class Hx834_Member {
		///<summary>Loop 2000 INS</summary>
		public X12_INS MemberLevelDetail;
		///<summary>Loop 2000 REF_1</summary>
		public X12_REF SubscriberIdentifier;
		///<summary>Loop 2000 REF_2</summary>
		public X12_REF MemberPolicyNumber;
		///<summary>Loop 2000 REF_3 (repeat 13)</summary>
		public List <X12_REF> ListMemberSupplementalIdentifiers=new List<X12_REF>();
		///<summary>Loop 2000 DTP (repeat 24)</summary>
		public List <X12_DTP> ListMemberLevelDates=new List<X12_DTP>();
		///<summary>Loop 2100A NM1</summary>
		public X12_NM1 MemberName;
		///<summary>Loop 2100A PER.</summary>
		public X12_PER MemberCommunicationsNumbers;
		///<summary>Loop 2100A N3</summary>
		public X12_N3 MemberResidenceStreetAddress;
		///<summary>Loop 2100A N4</summary>
		public X12_N4 MemberCityStateZipCode;
		///<summary>Loop 2100A DMG</summary>
		public X12_DMG MemberDemographics;
		///<summary>Loop 2100A EC</summary>
		public List<X12_EC> ListEmploymentClass=new List<X12_EC>();
		///<summary>Loop 2100A ICM</summary>
		public X12_ICM MemberIncome;
		///<summary>Loop 2100A AMT</summary>
		public List<X12_AMT> ListMemberPolicyAmounts=new List<X12_AMT>();
		///<summary>Loop 2100A HLH</summary>
		public X12_HLH MemberHealthInformation;
		///<summary>Loop 2100A LUI</summary>
		public List<X12_LUI> ListMemberLanguages=new List<X12_LUI>();
		///<summary>Loop 2100B NM1</summary>
		public X12_NM1 IncorrectMemberName;
		///<summary>Loop 2100B DMG</summary>
		public X12_DMG IncorrectMemberDemographics;
		///<summary>Loop 2100C NM1</summary>
		public X12_NM1 MemberMailingAddress;
		///<summary>Loop 2100C N3</summary>
		public X12_N3 MemberMailStreetAddress;
		///<summary>Loop 2100C N4</summary>
		public X12_N4 MemberMailCityStateZipCode;
		///<summary>Loop 2100D</summary>
		public List <Hx834_Employer> ListMemberEmployers=new List<Hx834_Employer>();
		///<summary>Loop 2100E</summary>
		public List <Hx834_School> ListMemberSchools=new List<Hx834_School>();
		///<summary>Loop 2100F NM1</summary>
		public X12_NM1 CustodialParent;
		///<summary>Loop 2100F PER</summary>
		public X12_PER CustodialParentCommunicationsNumbers;
		///<summary>Loop 2100F N3</summary>
		public X12_N3 CustodialParentStreetAddress;
		///<summary>Loop 2100F N4</summary>
		public X12_N4 CustodialParentCityStateZipCode;
		///<summary>Loop 2100G</summary>
		public List<Hx834_ResponsiblePerson> ListResponsiblePerson=new List<Hx834_ResponsiblePerson>();
		///<summary>Loop 2100H NM1</summary>
		public X12_NM1 DropOffLocation;
		///<summary>Loop 2100H N3</summary>
		public X12_N3 DropOffLocationStreetAddress;
		///<summary>Loop 2100H N4</summary>
		public X12_N4 DropOffLocationCityStateZipCode;
		///<summary>Loop 2200</summary>
		public List<Hx834_DisabilityInformation> ListDisabilityInformation=new List<Hx834_DisabilityInformation>();
		///<summary>Loop 2300</summary>
		public List<Hx834_HealthCoverage> ListHealthCoverage=new List<Hx834_HealthCoverage>();
	}

	///<summary>Loop 2310</summary>
	public class Hx834_Provider {
		///<summary>Loop 2310 LX</summary>
		public X12_LX ProviderInformation;
		///<summary>Loop 2310 NM1</summary>
		public X12_NM1 ProviderName;
		///<summary>Loop 2310 N3.  Repeat 2.</summary>
		public List<X12_N3> ListProviderAddresses=new List<X12_N3>();
		///<summary>Loop 2310 N4</summary>
		public X12_N4 ProviderCityStateZipCode;
		///<summary>Loop 2310 PER.  Repeat 2.</summary>
		public List <X12_PER> ListProviderCommunicationsNumbers=new List<X12_PER>();
		///<summary>Loop 2310 PLA</summary>
		public X12_PLA ProviderChangeReason;
	}

	///<summary>Loop 2100G</summary>
	public class Hx834_ResponsiblePerson {
		///<summary>Loop 2100G NM1</summary>
		public X12_NM1 ResponsiblePerson;
		///<summary>Loop 2100G PER</summary>
		public X12_PER ResponsiblePersonCommunicationsNumbers;
		///<summary>Loop 2100G N3</summary>
		public X12_N3 ResponsiblePersonStreetAddress;
		///<summary>Loop 2100G N4</summary>
		public X12_N4 ResponsiblePersonCityStateZipCode;
	}

	///<summary>Loop 2100E</summary>
	public class Hx834_School {
		///<summary>Loop 2100E NM1</summary>
		public X12_NM1 MemberSchool;
		///<summary>Loop 2100E PER</summary>
		public X12_PER MemberSchoolCommunicationsNumbers;
		///<summary>Loop 2100E N3</summary>
		public X12_N3 MemberSchoolStreetAddress;
		///<summary>Loop 2100E N4</summary>
		public X12_N4 MemberSchoolCityStateZipCode;
	}

	#endregion Helper Classes
}