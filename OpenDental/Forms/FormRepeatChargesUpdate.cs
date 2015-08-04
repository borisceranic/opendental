using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormRepeatChargesUpdate : Form{
		// ReSharper disable once InconsistentNaming
		private UI.Button butCancel;
		// ReSharper disable once InconsistentNaming
		private UI.Button butOK;
		// ReSharper disable once InconsistentNaming
		private TextBox textBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		///<summary></summary>
		public FormRepeatChargesUpdate()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRepeatChargesUpdate));
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.SystemColors.Control;
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox1.Location = new System.Drawing.Point(43, 13);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(426, 175);
			this.textBox1.TabIndex = 3;
			this.textBox1.Text = resources.GetString("textBox1.Text");
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(393, 197);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(393, 238);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormRepeatChargesUpdate
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(520, 289);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormRepeatChargesUpdate";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Update Repeating Charges";
			this.Load += new System.EventHandler(this.FormRepeatChargesUpdate_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormRepeatChargesUpdate_Load(object sender, EventArgs e) {
		
		}

		private static Claim CreateClaim(string claimType,List<PatPlan> patPlanList,List<InsPlan> planList,List<ClaimProc> claimProcList,Procedure proc,List<InsSub> subList) {
			long claimFormNum=0;
			InsPlan planCur=new InsPlan();
			InsSub subCur=new InsSub();
			Relat relatOther=Relat.Self;
			switch(claimType) {
				case "P":
					subCur=InsSubs.GetSub(PatPlans.GetInsSubNum(patPlanList,PatPlans.GetOrdinal(PriSecMed.Primary,patPlanList,planList,subList)),subList);
					planCur=InsPlans.GetPlan(subCur.PlanNum,planList);
					break;
				case "S":
					subCur=InsSubs.GetSub(PatPlans.GetInsSubNum(patPlanList,PatPlans.GetOrdinal(PriSecMed.Secondary,patPlanList,planList,subList)),subList);
					planCur=InsPlans.GetPlan(subCur.PlanNum,planList);
					break;
				case "Med":
					//It's already been verified that a med plan exists
					subCur=InsSubs.GetSub(PatPlans.GetInsSubNum(patPlanList,PatPlans.GetOrdinal(PriSecMed.Medical,patPlanList,planList,subList)),subList);
					planCur=InsPlans.GetPlan(subCur.PlanNum,planList);
					break;
			}
			ClaimProc claimProcCur=Procedures.GetClaimProcEstimate(proc.ProcNum,claimProcList,planCur,subCur.InsSubNum);
			if(claimProcCur==null) {
				claimProcCur=new ClaimProc();
				ClaimProcs.CreateEst(claimProcCur,proc,planCur,subCur);
			}
			Claim claimCur=new Claim();
			claimCur.PatNum=proc.PatNum;
			claimCur.DateService=proc.ProcDate;
			claimCur.ClinicNum=proc.ClinicNum;
			claimCur.PlaceService=proc.PlaceService;
			claimCur.ClaimStatus="W";
			claimCur.DateSent=DateTimeOD.Today;
			claimCur.PlanNum=planCur.PlanNum;
			claimCur.InsSubNum=subCur.InsSubNum;
			InsSub sub;
			switch(claimType) {
				case "P":
					claimCur.PatRelat=PatPlans.GetRelat(patPlanList,PatPlans.GetOrdinal(PriSecMed.Primary,patPlanList,planList,subList));
					claimCur.ClaimType="P";
					claimCur.InsSubNum2=PatPlans.GetInsSubNum(patPlanList,PatPlans.GetOrdinal(PriSecMed.Secondary,patPlanList,planList,subList));
					sub=InsSubs.GetSub(claimCur.InsSubNum2,subList);
					if(sub.PlanNum>0 && InsPlans.RefreshOne(sub.PlanNum).IsMedical) {
						claimCur.PlanNum2=0;//no sec ins
						claimCur.PatRelat2=Relat.Self;
					}
					else {
						claimCur.PlanNum2=sub.PlanNum;//might be 0 if no sec ins
						claimCur.PatRelat2=PatPlans.GetRelat(patPlanList,PatPlans.GetOrdinal(PriSecMed.Secondary,patPlanList,planList,subList));
					}
					break;
				case "S":
					claimCur.PatRelat=PatPlans.GetRelat(patPlanList,PatPlans.GetOrdinal(PriSecMed.Secondary,patPlanList,planList,subList));
					claimCur.ClaimType="S";
					claimCur.InsSubNum2=PatPlans.GetInsSubNum(patPlanList,PatPlans.GetOrdinal(PriSecMed.Primary,patPlanList,planList,subList));
					sub=InsSubs.GetSub(claimCur.InsSubNum2,subList);
					claimCur.PlanNum2=sub.PlanNum;
					claimCur.PatRelat2=PatPlans.GetRelat(patPlanList,PatPlans.GetOrdinal(PriSecMed.Primary,patPlanList,planList,subList));
					break;
				case "Med":
					claimCur.PatRelat=PatPlans.GetFromList(patPlanList,subCur.InsSubNum).Relationship;
					claimCur.ClaimType="Other";
					if(PrefC.GetBool(PrefName.ClaimMedTypeIsInstWhenInsPlanIsMedical)){
						claimCur.MedType=EnumClaimMedType.Institutional;
					}
					else{
						claimCur.MedType=EnumClaimMedType.Medical;
					}
					break;
				case "Other":
					claimCur.PatRelat=relatOther;
					claimCur.ClaimType="Other";
					//plannum2 is not automatically filled in.
					claimCur.ClaimForm=claimFormNum;
					if(planCur.IsMedical){
						if(PrefC.GetBool(PrefName.ClaimMedTypeIsInstWhenInsPlanIsMedical)){
							claimCur.MedType=EnumClaimMedType.Institutional;
						}
						else{
							claimCur.MedType=EnumClaimMedType.Medical;
						}
					}
					break;
			}
			if(planCur.PlanType=="c"){//if capitation
				claimCur.ClaimType="Cap";
			}
			claimCur.ProvTreat=proc.ProvNum;
			if(Providers.GetIsSec(proc.ProvNum)) {
				claimCur.ProvTreat=Patients.GetPat(proc.PatNum).PriProv;
				//OK if zero, because auto select first in list when open claim
			}
			claimCur.IsProsthesis="N";
			claimCur.ProvBill=Providers.GetBillingProvNum(claimCur.ProvTreat,claimCur.ClinicNum);//OK if zero, because it will get fixed in claim
			claimCur.EmployRelated=YN.No;
			claimCur.ClaimForm=planCur.ClaimFormNum;
			Claims.Insert(claimCur);
			//attach procedure
			claimProcCur.ClaimNum=claimCur.ClaimNum;
			if(planCur.PlanType=="c") {//if capitation
				claimProcCur.Status=ClaimProcStatus.CapClaim;
			}
			else {
				claimProcCur.Status=ClaimProcStatus.NotReceived;
			}
			if(planCur.UseAltCode && (ProcedureCodes.GetProcCode(proc.CodeNum).AlternateCode1!="")) {
				claimProcCur.CodeSent=ProcedureCodes.GetProcCode(proc.CodeNum).AlternateCode1;
			}
			else if(planCur.IsMedical && proc.MedicalCode!="") {
				claimProcCur.CodeSent=proc.MedicalCode;
			}
			else {
				claimProcCur.CodeSent=ProcedureCodes.GetProcCode(proc.CodeNum).ProcCode;
				if(claimProcCur.CodeSent.Length>5 && claimProcCur.CodeSent.Substring(0,1)=="D") {
					claimProcCur.CodeSent=claimProcCur.CodeSent.Substring(0,5);
				}
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
					if(claimProcCur.CodeSent.Length>5) {//In Canadian e-claims, codes can contain letters or numbers and cannot be longer than 5 characters.
						claimProcCur.CodeSent=claimProcCur.CodeSent.Substring(0,5);
					}
				}
			}
			claimProcCur.LineNumber=1;
			ClaimProcs.Update(claimProcCur);
			return claimCur;
		}

		///<summary>Do not call this until after determining if the repeate charge might generate a claim.  This function checks current insurance and may not add claims if no insurance is found.</summary>
		private static List<Claim> AddClaimsHelper(RepeatCharge repeateCharge,Procedure proc) {
			List<PatPlan> patPlanList=PatPlans.Refresh(repeateCharge.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(Patients.GetFamily(repeateCharge.PatNum));
			List<InsPlan> insPlanList=InsPlans.RefreshForSubList(subList);
			List<Benefit> benefitList=Benefits.Refresh(patPlanList,subList);
			List<Claim> retVal=new List<Claim>();
			Claim claimCur;
			if(patPlanList.Count==0) {//no current insurance, do not create a claim
				return retVal;
			}
			//create the claimprocs
			Procedures.ComputeEstimates(proc,proc.PatNum,new List<ClaimProc>(),true,insPlanList,patPlanList,benefitList,
				Patients.GetPat(proc.PatNum).Age,subList);
			//get claimprocs for this proc, may be more than one
			List<ClaimProc> claimProcList=ClaimProcs.GetForProc(ClaimProcs.Refresh(proc.PatNum),proc.ProcNum);
			string claimType="P";
			if(patPlanList.Count==1 && PatPlans.GetOrdinal(PriSecMed.Medical,patPlanList,insPlanList,subList)>0) {//if there's exactly one medical plan
				claimType="Med";
			}
			claimCur=CreateClaim(claimType,patPlanList,insPlanList,claimProcList,proc,subList);
			claimProcList=ClaimProcs.Refresh(proc.PatNum);
			if(claimCur.ClaimNum==0) {
				return retVal;
			}
			retVal.Add(claimCur);
			ClaimL.CalculateAndUpdate(new List<Procedure> {proc},insPlanList,claimCur,patPlanList,benefitList,Patients.GetPat(proc.PatNum).Age,subList);
			if(PatPlans.GetOrdinal(PriSecMed.Secondary,patPlanList,insPlanList,subList)>0 //if there exists a secondary plan
			   && !CultureInfo.CurrentCulture.Name.EndsWith("CA")) //and not canada (don't create secondary claim for canada)
			{
				claimCur=CreateClaim("S",patPlanList,insPlanList,claimProcList,proc,subList);
				if(claimCur.ClaimNum==0) {
					return retVal;
				}
				retVal.Add(claimCur);
				ClaimProcs.Refresh(proc.PatNum);
				claimCur.ClaimStatus="H";
				ClaimL.CalculateAndUpdate(new List<Procedure> {proc},insPlanList,claimCur,patPlanList,benefitList,Patients.GetPat(proc.PatNum).Age,subList);
			}
			return retVal;
		}

		///<summary>Returns 1 or 2 dates to be billed given the date range. Only filtering based on date range has been performed.</summary>
		private static List<DateTime> GetBillingDatesHelper(DateTime dateStart, DateTime dateStop, int billingCycleDay=0) {
			List<DateTime> retVal=new List<DateTime>();
			if(!PrefC.GetBool(PrefName.BillingUseBillingCycleDay)) {
				billingCycleDay=dateStart.Day;
			}
			//Add dates on the first of each of the last three months
			retVal.Add(new DateTime(DateTime.Today.AddMonths(-0).Year,DateTime.Today.AddMonths(-0).Month,1));//current month -0
			retVal.Add(new DateTime(DateTime.Today.AddMonths(-1).Year,DateTime.Today.AddMonths(-1).Month,1));//current month -1
			retVal.Add(new DateTime(DateTime.Today.AddMonths(-2).Year,DateTime.Today.AddMonths(-2).Month,1));//current month -2
			//This loop fixes day of month, taking into account billing day past the end of the month.
			for(int i=0;i<retVal.Count;i++) {
				int billingDay=Math.Min(retVal[i].AddMonths(1).AddDays(-1).Day, billingCycleDay);
				retVal[i]=new DateTime(retVal[i].Year, retVal[i].Month, billingDay);//This re-adds the billing date with the proper day of month.
			}
			//Remove billing dates that are calulated before repeat charge started.
			retVal.RemoveAll(x => x < dateStart);
			//Remove billing dates older than one month and 20 days ago.
			retVal.RemoveAll(x => x < DateTime.Today.AddMonths(-1).AddDays(-20));
			//Remove any dates after today
			retVal.RemoveAll(x => x > DateTime.Today);
			//Remove billing dates past the end of the dateStop
			int monthAdd=0;
			if(PrefC.GetBool(PrefName.BillingUseBillingCycleDay)) {
				monthAdd=1;
			}
			if(dateStop.Year>1880) {
				retVal.RemoveAll(x => x >= dateStop.AddMonths(monthAdd));
			}
			return retVal;
		}

		private static Procedure AddRepeatingChargeHelper(RepeatCharge repeatCharge,DateTime billingDate) {
			Procedure procedure=new Procedure();
			procedure.CodeNum=ProcedureCodes.GetCodeNum(repeatCharge.ProcCode);
			procedure.DateEntryC=DateTimeOD.Today;
			procedure.PatNum=repeatCharge.PatNum;
			procedure.ProcDate=billingDate;
			procedure.DateTP=billingDate;
			procedure.ProcFee=repeatCharge.ChargeAmt;
			procedure.ProcStatus=ProcStat.C;
			procedure.ProvNum=PrefC.GetLong(PrefName.PracticeDefaultProv);
			procedure.MedicalCode=ProcedureCodes.GetProcCode(procedure.CodeNum).MedicalCode;
			procedure.BaseUnits=ProcedureCodes.GetProcCode(procedure.CodeNum).BaseUnits;
			procedure.DiagnosticCode=PrefC.GetString(PrefName.ICD9DefaultForNewProcs);
			if(Prefs.IsODHQ()) {
				procedure.BillingNote=ProcedureCodes.GetProcCode(repeatCharge.ProcCode).Descript+" charge for "
				                      +billingDate.AddMonths(GetBillingMonthHelper(repeatCharge.ProcCode)).ToString("MMMM yyyy")+".";
			}
			//Check if the repeating charge has been flagged to copy it's note into the billing note of the procedure.
			if(repeatCharge.CopyNoteToProc && !string.IsNullOrEmpty(repeatCharge.Note)) {
				procedure.BillingNote+="\r\n"+repeatCharge.Note;
			}
			Procedures.Insert(procedure); //no recall synch needed because dental offices don't use this feature
			return procedure;
		}

		///<summary>Some ProcCodes are billed in arrears and should have the description of</summary>
		private static int GetBillingMonthHelper(string procCode) {
			//Should not be called if not ODHQ.
			if(!Prefs.IsODHQ()) {
				return 0;
			}
			if(procCode.StartsWith("Z0")) {
				return -1;//eRX codes should be billed for one month in arears.
			}
			return 0;
		}

		///<summary>Should only be called if ODHQ.</summary>
		private static List<Procedure> AddSmsRepeatingChargesHelper() {
			DateTime dateStart=new DateTime(DateTime.Today.AddMonths(-1).AddDays(-20).Year,DateTime.Today.AddMonths(-1).AddDays(-20).Month,1);
			DateTime dateStop=DateTime.Today.AddDays(1);
			List<SmsBilling> listSmsBilling=SmsBillings.GetByDateRange(dateStart,dateStop);
			List<Patient> listPatients=Patients.GetMultPats(listSmsBilling.Select(x => x.CustPatNum).Distinct().ToList()).ToList(); //local cache
			ProcedureCode procCodeAccess=ProcedureCodes.GetProcCode("038");
			ProcedureCode procCodeUsage=ProcedureCodes.GetProcCode("039");
			List<Procedure> listProcsAccess=Procedures.GetCompletedForDateRange(dateStart,dateStop,new List<long> {procCodeAccess.CodeNum});
			List<Procedure> listProcsUsage=Procedures.GetCompletedForDateRange(dateStart,dateStop,new List<long> {procCodeUsage.CodeNum});
			List<Procedure> retVal=new List<Procedure>();
			foreach(SmsBilling smsBilling in listSmsBilling) {
				Patient pat=listPatients.FirstOrDefault(x => x.PatNum==smsBilling.CustPatNum);
				if(pat==null) {
					EServiceSignal eSignal=new EServiceSignal {
						ServiceCode=(int)eServiceCode.IntegratedTexting,
						SigDateTime=DateTime.Now,
						Severity=eServiceSignalSeverity.Error,
						Description="Sms billing row found for non existant patient PatNum:"+smsBilling.CustPatNum
					};
					EServiceSignals.Insert(eSignal);
					continue;
				}
				//Find the billing date based on the date usage.
				DateTime billingDate=new DateTime(
					smsBilling.DateUsage.Year,
					smsBilling.DateUsage.Month,
					Math.Min(pat.BillingCycleDay,smsBilling.DateUsage.AddMonths(1).AddDays(-1).Day));
				if(billingDate>DateTime.Today || billingDate<DateTime.Today.AddMonths(-1).AddDays(-20)){
					//One month and 20 day window. Bill regardless of presence of "038" repeat charge.
					continue;
				}
				//List<DateTime> listBillingDates=GetBillingDatesHelper(dateStart, dateStop, pat.BillingCycleDay);
				if(smsBilling.AccessChargeTotalUSD>0
				   && !listProcsAccess.Exists(x => x.PatNum==pat.PatNum && x.ProcDate.Year==billingDate.Year && x.ProcDate.Month==billingDate.Month)) {
					//The calculated access charge was greater than 0 and there is not an existing "038" procedure on the account for that month.
					Procedure procAccess=new Procedure();
					procAccess.CodeNum=procCodeAccess.CodeNum;
					procAccess.DateEntryC=DateTimeOD.Today;
					procAccess.PatNum=pat.PatNum;
					procAccess.ProcDate=billingDate;
					procAccess.DateTP=billingDate;
					procAccess.ProcFee=smsBilling.AccessChargeTotalUSD;
					procAccess.ProcStatus=ProcStat.C;
					procAccess.ProvNum=PrefC.GetLong(PrefName.PracticeDefaultProv);
					procAccess.MedicalCode=procCodeAccess.MedicalCode;
					procAccess.BaseUnits=procCodeAccess.BaseUnits;
					procAccess.DiagnosticCode=PrefC.GetString(PrefName.ICD9DefaultForNewProcs);
					procAccess.BillingNote="Texting Access charge for "+
					                       billingDate.ToString("MMMM yyyy")+".";
					Procedures.Insert(procAccess);
					listProcsAccess.Add(procAccess);
					retVal.Add(procAccess);
				}
				if(smsBilling.MsgChargeTotalUSD>0
				   && !listProcsUsage.Exists(x => x.PatNum==pat.PatNum && x.ProcDate.Year==billingDate.Year && x.ProcDate.Month==billingDate.Month)) {
					//Calculated Usage charge > 0 and not already billed, may exist without access charge
					Procedure procUsage=new Procedure();
					procUsage.CodeNum=procCodeUsage.CodeNum;
					procUsage.DateEntryC=DateTimeOD.Today;
					procUsage.PatNum=pat.PatNum;
					procUsage.ProcDate=billingDate;
					procUsage.DateTP=billingDate;
					procUsage.ProcFee=smsBilling.MsgChargeTotalUSD;
					procUsage.ProcStatus=ProcStat.C;
					procUsage.ProvNum=PrefC.GetLong(PrefName.PracticeDefaultProv);
					procUsage.MedicalCode=procCodeUsage.MedicalCode;
					procUsage.BaseUnits=procCodeUsage.BaseUnits;
					procUsage.DiagnosticCode=PrefC.GetString(PrefName.ICD9DefaultForNewProcs);
					procUsage.BillingNote="Texting Usage charge for "+
					                      billingDate.ToString("MMMM yyyy")+".";
					Procedures.Insert(procUsage);
					listProcsUsage.Add(procUsage);
					retVal.Add(procUsage);
				}
			}
			return retVal;
		}

		private void butOK_Click(object sender, EventArgs e) {
			List<RepeatCharge> listRepeatingCharges=RepeatCharges.Refresh(0).ToList();
			int proceduresAddedCount=0;
			int claimsAddedCount=0;
			if(Prefs.IsODHQ()) {
				//If ODHQ, handle Integrated texting repeating charges differently.
				listRepeatingCharges.RemoveAll(x => x.ProcCode=="038");
				proceduresAddedCount+=AddSmsRepeatingChargesHelper().Count;
			}
			List<Procedure> listExistingProcs=Procedures.GetCompletedForDateRange(DateTime.Today.AddMonths(-2), DateTime.Today.AddDays(1),
				listRepeatingCharges.Select(x => x.ProcCode).Distinct().Select(x => ProcedureCodes.GetProcCode(x).CodeNum).ToList()
				//,listRepeatingCharges.Select(possibleBillingDate=>possibleBillingDate.PatNum).ToList() //Passing in PatNums may make query less efficient
				);
			foreach(RepeatCharge repeatCharge in listRepeatingCharges){
				if(!repeatCharge.IsEnabled || (repeatCharge.DateStop.Year > 1880 && repeatCharge.DateStop.AddMonths(3) < DateTime.Today)) {
					continue;//This repeating charge is too old to possibly create a new charge. Not precise but greatly reduces calls to DB.
									 //We will filter by more stringently on the DateStop later on.
				}
				List<DateTime> listBillingDates;//This list will have 1 or 2 dates where a repeating charge might be added
				if(PrefC.GetBool(PrefName.BillingUseBillingCycleDay)) {
					listBillingDates=GetBillingDatesHelper(repeatCharge.DateStart,repeatCharge.DateStop,Patients.GetPat(repeatCharge.PatNum).BillingCycleDay);
				}
				else {
					listBillingDates=GetBillingDatesHelper(repeatCharge.DateStart,repeatCharge.DateStop);
				}
				long codeNum=ProcedureCodes.GetCodeNum(repeatCharge.ProcCode);
				//Remove billing dates if there is a procedure for that patient with that code for the same amount in that month and year
				listBillingDates.RemoveAll(x => 
					listExistingProcs.Exists(y => 
						y.PatNum==repeatCharge.PatNum 
						&& y.CodeNum==codeNum 
						&& x.Year==y.ProcDate.Year
						&& x.Month==y.ProcDate.Month
						&& IsRepeatDateHelper(repeatCharge,x,y.ProcDate)
						&& y.ProcFee==repeatCharge.ChargeAmt
					)
				);
				//Remove the procedure so that if there is another repeat charge with the same ProcCode and ChargeAmt, it will be added in the next iteration
				listExistingProcs.Remove(listExistingProcs.FirstOrDefault(y =>
						y.PatNum==repeatCharge.PatNum 
						&& y.CodeNum==codeNum 
						&& y.ProcFee==repeatCharge.ChargeAmt
						)
				);
				//If any billing dates have not been filtered out, add a repeating charge on those dates
				foreach (DateTime billingDate in listBillingDates) {
					Procedure procAdded=AddRepeatingChargeHelper(repeatCharge, billingDate);
					List<Claim> listClaimsAdded=new List<Claim>();
					if(repeatCharge.CreatesClaim && !ProcedureCodes.GetProcCode(repeatCharge.ProcCode).NoBillIns) {
						listClaimsAdded=AddClaimsHelper(repeatCharge,procAdded);
					}
					proceduresAddedCount++;
					claimsAddedCount+=listClaimsAdded.Count;
				}
			}
			MessageBox.Show(proceduresAddedCount+" "+Lan.g(this,"procedures added.")+"\r\n"+claimsAddedCount+" "+Lan.g(this,"claims added."));
			DialogResult=DialogResult.OK;
		}

		///<summary>Returns true if the existing procedure was for the possibleBillingDate.</summary>
		private static bool IsRepeatDateHelper(RepeatCharge repeatCharge,DateTime possibleBillingDate,DateTime existingProcedureDate) {
			if(PrefC.GetBool(PrefName.BillingUseBillingCycleDay)) {
				//Only match month and year to be equal
				return (possibleBillingDate.Month==existingProcedureDate.Month && possibleBillingDate.Year==existingProcedureDate.Year);
			}
			if(possibleBillingDate.Month!=existingProcedureDate.Month || possibleBillingDate.Year!=existingProcedureDate.Year) {
				return false;
			}
			//Itterate through dates using new logic that takes repeatCharge.DateStart.AddMonths(n) to calculate dates
			DateTime possibleDateNew=repeatCharge.DateStart;
			int dateNewMonths=0;
			//Itterate through dates using old logic that starts with repeatCharge.DateStart and adds one month at a time to calculate dates
			DateTime possibleDateOld=repeatCharge.DateStart;
			do {
				if(existingProcedureDate==possibleDateNew || existingProcedureDate==possibleDateOld) {
					return true;
				}
				dateNewMonths++;
				possibleDateNew=repeatCharge.DateStart.AddMonths(dateNewMonths);
				possibleDateOld=possibleDateOld.AddMonths(1);
			} 
			while(possibleDateNew<=existingProcedureDate);
			return false;
		}

		private void butCancel_Click(object sender, EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		


	}
}





















