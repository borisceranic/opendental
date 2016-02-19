using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;

namespace OpenDental {
	public partial class FormInsVerificationList:Form {
		///<summary>-1 represents "All", and 0 represents "none".</summary>
		private long _verifyUserNum=-1;
		///<summary>0 represents "Unassign".</summary>
		private long _assignUserNum;
		///<summary>-1 represents "All", and 0 represents "Unassigned".</summary>
		private long _clinicNumVerifyClinicsFilter=-1;
		///<summary>-1 and 0 represent "All".</summary>
		private long _defNumVerifyRegionsFilter=-1;
		private long _defNumVerifyStatusFilter;
		private long _defNumVerifyStatusAssign;
		///<summary>This will only have a selection if selecting from gridMain.</summary>
		private InsVerify _insVerifySelected;
		///<summary>This will only have a selection if selecting from gridAssign.</summary>
		private List<InsVerify> _listInsVerifiesSelected;
		private long _patNumSelected;
		private List<Clinic> _listClinicsDb;
		private List<Clinic> _listClinicsFiltered;

		public FormInsVerificationList() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormInsVerificationList_Load(object sender,EventArgs e) {
			if(PrefC.GetBool(PrefName.InsVerifyDefaultToCurrentUser)) {
				_verifyUserNum=Security.CurUser.UserNum;
			}
			if(!PrefC.HasClinicsEnabled) {
				labelClinic.Visible=false;
				comboVerifyClinics.Visible=false;
				labelRegion.Visible=false;
				comboVerifyRegions.Visible=false;
			}
			textAppointmentScheduledDays.Text=POut.Int(PrefC.GetInt(PrefName.InsVerifyAppointmentScheduledDays));
			textInsBenefitEligibilityDays.Text=POut.Int(PrefC.GetInt(PrefName.InsVerifyBenefitEligibilityDays));
			textPatientEnrollmentDays.Text=POut.Int(PrefC.GetInt(PrefName.InsVerifyPatientEnrollmentDays));
			_listClinicsDb=Clinics.GetForUserod(Security.CurUser);
			FillGrids();
		}

		public void FillGrids() {
			FillUsers();
			FillComboBoxes();
			FillGridMain();
			FillGridAssign();
		}

		private void FillDisplayInfo(InsVerify insVerifySelected) {
			switch(insVerifySelected.VerifyType) {
				case VerifyTypes.InsuranceBenefit:
					FillInsuranceDisplay(InsPlans.GetPlan(insVerifySelected.FKey,null));
					textSubscriberName.Text="";
					textSubscriberBirthdate.Text="";
					textSubscriberSSN.Text="";
					textSubscriberID.Text="";
					break;
				case VerifyTypes.PatientEnrollment:
					PatPlan patPlanVerify = PatPlans.GetByPatPlanNum(insVerifySelected.FKey);
					if(patPlanVerify==null) {//Should never happen, but if it does, return because it is just for display purposes.
						return;
					}
					InsSub insSubVerify = InsSubs.GetOne(patPlanVerify.InsSubNum);
					textSubscriberID.Text=insSubVerify.SubscriberID;
					Patient patSubscriberVerify = Patients.GetPat(insSubVerify.Subscriber);
					if(patSubscriberVerify!=null) {
						textSubscriberName.Text=patSubscriberVerify.GetNameFL();
						textSubscriberBirthdate.Text=patSubscriberVerify.Birthdate.ToShortDateString();
						textSubscriberSSN.Text=patSubscriberVerify.SSN;
					}
					FillInsuranceDisplay(InsPlans.GetPlan(insSubVerify.PlanNum,null));
					break;
				case VerifyTypes.None:
				default:
					return;//Should never happen, but this is only for display, so we can just return here.
			}
		}

		private void FillInsuranceDisplay(InsPlan insPlanVerify) {
			if(insPlanVerify==null) {//Should never happen, but if it does, return because it is just for display purposes.
				return;
			}
			textInsPlanGroupName.Text=insPlanVerify.GroupName;
			textInsPlanGroupNumber.Text=insPlanVerify.GroupNum;
			textInsPlanNote.Text=insPlanVerify.PlanNote;
			Employer employer = Employers.GetEmployer(insPlanVerify.EmployerNum);
			if(employer!=null) {
				textInsPlanEmployer.Text=employer.EmpName;
			}
			Carrier carrierVerify = Carriers.GetCarrier(insPlanVerify.CarrierNum);
			if(carrierVerify!=null) {
				textCarrierName.Text=carrierVerify.CarrierName;
				textCarrierPhoneNumber.Text=carrierVerify.Phone;
			}
		}

		///<summary>Does not fill the Clinic Combo Box</summary>
		private void FillComboBoxes() {
			comboSetVerifyStatus.Items.Clear();
			comboFilterVerifyStatus.Items.Clear();
			comboFilterVerifyStatus.Items.Add("All");
			comboSetVerifyStatus.Items.Add("none");
			Def[] arrayVerifyStatusDefs = DefC.Short[(int)DefCat.InsuranceVerificationStatus];
			for(int i = 0;i<arrayVerifyStatusDefs.Length;i++) {
				comboFilterVerifyStatus.Items.Add(arrayVerifyStatusDefs[i].ItemName);
				comboSetVerifyStatus.Items.Add(arrayVerifyStatusDefs[i].ItemName);
				if(arrayVerifyStatusDefs[i].DefNum==_defNumVerifyStatusFilter) {
					comboFilterVerifyStatus.SelectedIndex=i+1;
				}
				if(arrayVerifyStatusDefs[i].DefNum==_defNumVerifyStatusAssign) {
					comboSetVerifyStatus.SelectedIndex=i+1;
				}
			}
			if(comboFilterVerifyStatus.SelectedIndex==-1) {
				comboFilterVerifyStatus.SelectedIndex=0;
			}
			if(comboSetVerifyStatus.SelectedIndex==-1) {
				comboSetVerifyStatus.SelectedIndex=0;
			}
			comboVerifyRegions.Items.Clear();
			if(PrefC.HasClinicsEnabled) {
				List<Def> listRegionDefs = DefC.Short[(int)DefCat.Regions].ToList();
				if(listRegionDefs.Count!=0) {
					listRegionDefs.RemoveAll(x => !_listClinicsDb.Any(y => y.Region==x.DefNum));
					comboVerifyRegions.Items.Add(Lan.g(this,"All"));
					for(int i = 0;i<listRegionDefs.Count;i++) {
						comboVerifyRegions.Items.Add(listRegionDefs[i].ItemName);
						if(listRegionDefs[i].DefNum==_defNumVerifyRegionsFilter) {
							comboVerifyRegions.SelectedIndex=i+1;
						}
					}
					if(comboVerifyRegions.SelectedIndex==-1) {//Will select either "All" or the restricted clinic's region.
							comboVerifyRegions.SelectedIndex=0;
					}
				}
				else {
					comboVerifyRegions.Visible=false;
					labelRegion.Visible=false;
					_defNumVerifyRegionsFilter=-1;
				}
				FillClinicComboBox();
			}
		}

		private void FillClinicComboBox() {
			_listClinicsDb=Clinics.GetForUserod(Security.CurUser);
			_listClinicsFiltered=_listClinicsDb.Where(x=>x.Region==_defNumVerifyRegionsFilter).ToList();
			comboVerifyClinics.Items.Clear();
			comboVerifyClinics.SelectedIndex=-1;
			int indexOffset = 0;
			if(!Security.CurUser.ClinicIsRestricted) {
				comboVerifyClinics.Items.Add(Lan.g(this,"All"));
				indexOffset=1;
			}
			if(_defNumVerifyRegionsFilter<1) {
				for(int i = 0;i<_listClinicsDb.Count;i++) {
					comboVerifyClinics.Items.Add(_listClinicsDb[i].Description);
					if(_clinicNumVerifyClinicsFilter==_listClinicsDb[i].ClinicNum) {
						comboVerifyClinics.SelectedIndex=i+indexOffset;
					}
				}
				if(!Security.CurUser.ClinicIsRestricted) {
					comboVerifyClinics.Items.Add(Lan.g(this,"Unassigned"));//Add this at the end so it is on the bottom
					if(_clinicNumVerifyClinicsFilter==0) {
						comboVerifyClinics.SelectedIndex=comboVerifyClinics.Items.Count-1;
					}
				}
				if(comboVerifyClinics.SelectedIndex==-1 && comboVerifyClinics.Items.Count>0) {
					comboVerifyClinics.SelectedIndex=0;
				}
			}
			else {//User selected a region to filter
				for(int i = 0;i<_listClinicsFiltered.Count;i++) {
					comboVerifyClinics.Items.Add(_listClinicsFiltered[i].Description);
					if(_clinicNumVerifyClinicsFilter==_listClinicsFiltered[i].ClinicNum) {
						comboVerifyClinics.SelectedIndex=i+indexOffset;
					}
				}
				if(comboVerifyClinics.SelectedIndex==-1 && comboVerifyClinics.Items.Count>0) {
					comboVerifyClinics.SelectedIndex=0;
				}
			}
		}

		private void FillUsers() {
			List<Userod> listUsers = UserodC.GetListShort();
			for(int i = 0;i<listUsers.Count;i++) {
				if(_assignUserNum==listUsers[i].UserNum) {
					textAssignUser.Text=listUsers[i].UserName;
				}
				if(_verifyUserNum==listUsers[i].UserNum) {
					textVerifyUser.Text=listUsers[i].UserName;
				}
			}
			if(_assignUserNum==0) {
				textAssignUser.Text="Unassign";
			}
			if(_verifyUserNum==-1) {
				textVerifyUser.Text="All Users";
			}
			else if(_verifyUserNum==0) {
				textVerifyUser.Text="Unassigned";
			}
		}

		private void PickUser(bool isAssigning) {
			FormUserPick FormUP = new FormUserPick();
			FormUP.IsSelectionmode=true;
			if(!isAssigning) {
				FormUP.IsPickAllAllowed=true;
			}
			FormUP.IsPickNoneAllowed=true;
			FormUP.ShowDialog();
			if(FormUP.DialogResult==DialogResult.OK) {
				if(isAssigning) {//Setting the user
					_assignUserNum=FormUP.SelectedUserNum;
				}
				else {//Filter by user
					_verifyUserNum=FormUP.SelectedUserNum;
				}
				FillGrids();
			}
		}

		#region Grid Verify
		private void FillGridMain() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lans.g(this,"Type"),35);
			gridMain.Columns.Add(col);
			if(PrefC.HasClinicsEnabled) {
				col=new ODGridColumn(Lans.g(this,"Clinic"),90);
				gridMain.Columns.Add(col);
			}
			col=new ODGridColumn(Lans.g(this,"Patient"),120);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Next Appt Date"),135,HorizontalAlignment.Center,GridSortingStrategy.DateParse);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Carrier"),160);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Last Verified"),90,HorizontalAlignment.Center,GridSortingStrategy.DateParse);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Status"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Last Assigned"),90,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Assigned to"),0);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			List<VerifyGridRow> listGridRows = GetRowsForVerifyGrid();
			listGridRows.Sort(CompareVerifyGrid);
			for(int i = 0;i<listGridRows.Count;i++) {
				gridMain.Rows.Add(VerifyRowToODGridRow(listGridRows[i]));
			}
			gridMain.EndUpdate();
		}

		private List<VerifyGridRow> GetRowsForVerifyGrid() {
			List<VerifyGridRow> listGridRows = new List<VerifyGridRow>();
			List<long> listInsPlanVerifyNums = new List<long>();
			List<InsVerify> insVerifiesForGrid = InsVerifies.GetAll();
			Dictionary<Tuple<long,VerifyTypes>,InsVerify> dictInsVerify = new Dictionary<Tuple<long,VerifyTypes>,InsVerify>();
			foreach(InsVerify insVerifyCur in insVerifiesForGrid) {
				//Add the Fkey and VerifyTypes combination as the key for the dictionary to get the InsVerify object.
				//This will be used to save calls to the database instead of calling GetOne() every time we need an InsVerify object.
				dictInsVerify.Add(new Tuple<long,VerifyTypes>(insVerifyCur.FKey,insVerifyCur.VerifyType),insVerifyCur);
			}
			DateTime dateTimeStart = DateTime.Today;
			//End the following day at midnight.  There shouldn't be any appointments at midnight, and if there is they get to verify slightly early.
			DateTime dateTimeEnd = DateTime.Today.AddDays(PIn.Int(textAppointmentScheduledDays.Text)+1);
			List<Appointment> listSchedAppts = Appointments.GetAppointmentsForPeriod(dateTimeStart,dateTimeEnd);
			VerifyGridRow row;
			foreach(Appointment apptCur in listSchedAppts) {
				Clinic clinicCur = Clinics.GetClinic(apptCur.ClinicNum);
				//ClinicNum was either invalid or has a region that isn't in the restricted clinic region list, so don't display the appointment.
				string apptClinicName = "";
				if(clinicCur!=null) {
					apptClinicName = clinicCur.Description;
				}
				if(_clinicNumVerifyClinicsFilter==0 && apptCur.ClinicNum!=0) {
					continue;
				}
				if((_clinicNumVerifyClinicsFilter!=-1 && _clinicNumVerifyClinicsFilter!=0)//Not "All" and "Unassigned"
					 && apptCur.ClinicNum!=_clinicNumVerifyClinicsFilter) 
				{
					continue;
				}
				if(_defNumVerifyRegionsFilter>=1 && !_listClinicsFiltered.Any(x => x.ClinicNum==apptCur.ClinicNum)) {
					continue;
				}
				Patient patCur = Patients.GetPat(apptCur.PatNum);
				if(patCur==null) {//Should never happen.  This means the patnum is orphaned and we don't want to show it anyways.
					continue;
				}
				List<PatPlan> listPatPlans = PatPlans.Refresh(patCur.PatNum);
				foreach(PatPlan patPlanCur in listPatPlans) {
					row=new VerifyGridRow();
					InsSub insSubCur = InsSubs.GetOne(patPlanCur.InsSubNum);
					if(insSubCur==null) {//Should never happen.  It means the patplan was orphaned and we can't display this insurance.
						continue;
					}
					InsPlan insPlanCur = InsPlans.GetPlan(insSubCur.PlanNum,null);
					if(insPlanCur==null) {//Should never happen.  It means the inssub row was orphaned and we can't display this insurance.
						continue;
					}
					Carrier carrierCur = Carriers.GetCarrier(insPlanCur.CarrierNum);
					if(carrierCur==null || !carrierCur.CarrierName.ToLower().Contains(textVerifyCarrier.Text.ToLower())) {
						continue;
					}
					Userod userCur = null;
					DateTime dateLastPatVerified = DateTime.MinValue;
					string userNamePatVerify = "";
					string verifyStatus = "";
					#region Patient Enrollment Row
					InsVerify lastPatVerified = null;
					Tuple<long,VerifyTypes> key = new Tuple<long,VerifyTypes>(patPlanCur.PatPlanNum,VerifyTypes.PatientEnrollment);
					if(dictInsVerify.ContainsKey(key)) {
						lastPatVerified=dictInsVerify[key];
					}
					if(lastPatVerified!=null) {
						//If the last date this patplan was verified hasn't been textPatientEnrollmentDays days ago, then don't show them on the list.
						if(lastPatVerified.DateLastVerified.AddDays(PIn.Int(textPatientEnrollmentDays.Text))>DateTime.Today) {
							continue;
						}
						dateLastPatVerified=lastPatVerified.DateLastVerified;
						userCur=Userods.GetUser(lastPatVerified.UserNum);
						if(userCur!=null) {
							userNamePatVerify=userCur.UserName;
						}
						Def defVerifyStatus = DefC.GetDef(DefCat.InsuranceVerificationStatus,lastPatVerified.DefNum);
						if(defVerifyStatus!=null) {
							verifyStatus=defVerifyStatus.ItemName;
						}
					}
					if(_verifyUserNum!=-1&&_verifyUserNum!=0&&userNamePatVerify!=textVerifyUser.Text) {
						continue;
					}
					if(_verifyUserNum==0&&userNamePatVerify!="") {//If they selected the None user or "Unassigned"
						continue;
					}
					if(comboFilterVerifyStatus.SelectedIndex!=0&&verifyStatus!=comboFilterVerifyStatus.Text) {
						continue;
					}
					row.Type="Pat";
					row.Clinic=apptClinicName;
					row.PatientName=patCur.GetNameFL();
					row.NextApptDate=apptCur.AptDateTime;
					row.CarrierName=carrierCur.CarrierName;
					row.DateLastVerified=dateLastPatVerified;
					row.Tag=new InsVerifyWithPatNum(apptCur.PatNum,lastPatVerified);
					row.VerifyStatus=verifyStatus;
					row.DateLastAssigned=lastPatVerified.DateLastAssigned;
					row.AssignedTo=userNamePatVerify;
					listGridRows.Add(row);//Add patplan if !isAssignList 
					#endregion
					#region Insurance Benefits Row
					if(insPlanCur.HideFromVerifyList) {
						continue;
					}
					if(listInsPlanVerifyNums.Contains(insPlanCur.PlanNum)) {
						continue;
					}
					string userNameInsVerify = "";
					verifyStatus="";
					row=new VerifyGridRow();
					InsVerify lastInsVerified = null;
					key=new Tuple<long,VerifyTypes>(insPlanCur.PlanNum,VerifyTypes.InsuranceBenefit);
					if(dictInsVerify.ContainsKey(key)) {
						lastInsVerified=dictInsVerify[key];
					}
					DateTime dateLastInsVerified = DateTime.MinValue;
					if(lastInsVerified!=null) {
						//If the last date this insurance was verified hasn't been textInsBenefitEligibilityDays days ago, then don't show them on the list.
						if(lastInsVerified.DateLastVerified.AddDays(PIn.Int(textInsBenefitEligibilityDays.Text))>DateTime.Today) {
							continue;
						}
						dateLastInsVerified=lastInsVerified.DateLastVerified;
						userCur=Userods.GetUser(lastInsVerified.UserNum);
						if(userCur!=null) {
							userNameInsVerify=userCur.UserName;
						}
						Def defVerifyStatus = DefC.GetDef(DefCat.InsuranceVerificationStatus,lastInsVerified.DefNum);
						if(defVerifyStatus!=null) {
							verifyStatus=defVerifyStatus.ItemName;
						}
					}
					if(_verifyUserNum!=-1&&_verifyUserNum!=0&&userNameInsVerify!=textVerifyUser.Text) {
						continue;
					}
					if(_verifyUserNum==0&&userNameInsVerify!="") {//If they selected the None user or "Unassigned"
						continue;
					}
					row.Type="Ins";
					row.Clinic=apptClinicName;
					row.PatientName="";
					row.NextApptDate=apptCur.AptDateTime;
					row.CarrierName=carrierCur.CarrierName;
					row.DateLastVerified=dateLastInsVerified;
					row.VerifyStatus=verifyStatus;
					row.DateLastAssigned=lastInsVerified.DateLastAssigned;
					row.AssignedTo=userNameInsVerify;
					row.Tag=new InsVerifyWithPatNum(apptCur.PatNum,lastInsVerified);
					listGridRows.Add(row);//Add insplan if !isAssignList, otherwise add patplan.
					listInsPlanVerifyNums.Add(insPlanCur.PlanNum);//add PlanNum to the list so that this insurance plan doesn't add another row to the grid. 
					#endregion
				}
			}
			return listGridRows;
		}

		private int CompareVerifyGrid(VerifyGridRow x,VerifyGridRow y) {
			if(x.Type==y.Type && x.Clinic==y.Clinic && x.NextApptDate==y.NextApptDate) {
				return x.CarrierName.CompareTo(y.CarrierName);
			}
			if(x.Type==y.Type && x.Clinic==y.Clinic) {
				return x.NextApptDate.CompareTo(y.NextApptDate);
			}
			if(x.Type==y.Type) {
				return x.Clinic.CompareTo(y.Clinic);
			}
			return x.Type.CompareTo(y.Type);
		}

		private ODGridRow VerifyRowToODGridRow(VerifyGridRow vrow) {
			ODGridRow row = new ODGridRow();
			row.Cells.Add(vrow.Type);
			if(PrefC.HasClinicsEnabled) {
				row.Cells.Add(vrow.Clinic);
			}
			row.Cells.Add(vrow.PatientName);
			row.Cells.Add(vrow.NextApptDate.ToShortDateString());
			row.Cells.Add(vrow.CarrierName);
			row.Cells.Add(vrow.DateLastVerified.ToShortDateString());
			row.Cells.Add(vrow.VerifyStatus);
			row.Cells.Add(vrow.DateLastAssigned.ToShortDateString());
			row.Cells.Add(vrow.AssignedTo);
			row.Tag=vrow.Tag;
			return row;
		}

		private void gridMain_CellClick(object sender,UI.ODGridClickEventArgs e) {
			_insVerifySelected=((InsVerifyWithPatNum)gridMain.Rows[e.Row].Tag).InsVerify;
			_patNumSelected=((InsVerifyWithPatNum)gridMain.Rows[e.Row].Tag).PatNum;
			if(_insVerifySelected!=null) {
				FillDisplayInfo(_insVerifySelected);
				textInsVerifyReadOnlyNote.Text=_insVerifySelected.Note;
			}
		}
		#endregion

		#region Verify Logic
		private void butVerify_Click(object sender,EventArgs e) {
			OnVerify();
		}

		private void OnVerify() {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an insurance to verify.");
				return;
			}
			InsVerify insVerifyCur = ((InsVerifyWithPatNum)gridMain.Rows[gridMain.GetSelectedIndex()].Tag).InsVerify;
			string verifyType = "";
			switch(insVerifyCur.VerifyType) {
				case VerifyTypes.InsuranceBenefit:
					verifyType="insurance";
					break;
				case VerifyTypes.PatientEnrollment:
					verifyType="patient's insurance";
					break;
				case VerifyTypes.None:
				default:
					break;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Are you sure you want to verify the selected "+verifyType+"?")) {
				return;
			}
			insVerifyCur.DateLastVerified=DateTime.Today;
			InsVerifyHists.InsertFromInsVerify(insVerifyCur);
			FillGrids();
		}

		private void gridMain_MouseUp(object sender,MouseEventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				return;
			}
			if(e.Button==MouseButtons.Right && gridMain.SelectedIndices.Length>0) {
				_menuRightClick.Items.Clear();
				_menuRightClick.Items.Add(Lan.g(this,"See Family")+" ("+Patients.GetPat(_patNumSelected).GetNameFL()+")",null,new EventHandler(gridMainRight_click));
				_menuRightClick.Items.Add(Lan.g(this,"Verify"),null,new EventHandler(gridMainRight_click));
				_menuRightClick.Show(gridMain,new Point(e.X,e.Y));
			}
		}

		private void gridMainRight_click(object sender,System.EventArgs e) {
			switch(_menuRightClick.Items.IndexOf((ToolStripMenuItem)sender)) {
				case 0:
					if(!Security.IsAuthorized(Permissions.FamilyModule)) {
						return;
					}
					GotoModule.GotoFamily(_patNumSelected);
					break;
				case 1:
					OnVerify();
					break;
			}
		}
		#endregion

		#region Grid Assign
		private void FillGridAssign() {
			gridAssign.BeginUpdate();
			gridAssign.Columns.Clear();
			ODGridColumn col;
			if(PrefC.HasClinicsEnabled) {
				col=new ODGridColumn(Lans.g(this,"Clinic"),85);
				gridAssign.Columns.Add(col);
			}
			col=new ODGridColumn(Lans.g(this,"Patient"),120);
			gridAssign.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Next Appt Date"),135,GridSortingStrategy.DateParse);
			col.TextAlign=HorizontalAlignment.Center;
			gridAssign.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Carrier"),160);
			gridAssign.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Pat Last Verified"),110,GridSortingStrategy.DateParse);
			col.TextAlign=HorizontalAlignment.Center;
			gridAssign.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Ins Last Verified"),110,GridSortingStrategy.DateParse);
			col.TextAlign=HorizontalAlignment.Center;
			gridAssign.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Status"),90);
			gridAssign.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Assigned to"),0);
			gridAssign.Columns.Add(col);
			gridAssign.Rows.Clear();
			List<AssignGridRow> listGridRows = GetRowsForAssignGrid();
			listGridRows.Sort(CompareAssignGrid);
			for(int i = 0;i<listGridRows.Count;i++) {
				gridAssign.Rows.Add(AssignRowToODGridRow(listGridRows[i]));
			}
			gridAssign.EndUpdate();
		}

		private List<AssignGridRow> GetRowsForAssignGrid() {
			List<AssignGridRow> listGridRows = new List<AssignGridRow>();
			List<InsVerify> insVerifiesForGrid = InsVerifies.GetAll();
			Dictionary<Tuple<long,VerifyTypes>,InsVerify> dictInsVerify = new Dictionary<Tuple<long,VerifyTypes>,InsVerify>();
			foreach(InsVerify insVerifyCur in insVerifiesForGrid) {
				//Add the Fkey and VerifyTypes combination as the key for the dictionary to get the InsVerify object.
				//This will be used to save calls to the database instead of calling GetOne() every time we need an InsVerify object.
				dictInsVerify.Add(new Tuple<long,VerifyTypes>(insVerifyCur.FKey,insVerifyCur.VerifyType),insVerifyCur);
			}
			DateTime dateTimeStart = DateTime.Today;
			//End the following day at midnight.  There shouldn't be any appointments at midnight, and if there is they get to verify slightly early.
			DateTime dateTimeEnd = DateTime.Today.AddDays(PIn.Int(textAppointmentScheduledDays.Text)+1);
			List<Appointment> listSchedAppts = Appointments.GetAppointmentsForPeriod(dateTimeStart,dateTimeEnd);
			AssignGridRow row;
			foreach(Appointment apptCur in listSchedAppts) {
				Clinic clinicCur = Clinics.GetClinic(apptCur.ClinicNum);
				//ClinicNum was either invalid or has a region that isn't in the restricted clinic region list, so don't display the appointment.
				string apptClinicName = "";
				if(clinicCur!=null) {
					apptClinicName = clinicCur.Description;
				}
				if(_clinicNumVerifyClinicsFilter==0 && apptCur.ClinicNum!=0) {
					continue;
				}
				if((_clinicNumVerifyClinicsFilter!=-1 && _clinicNumVerifyClinicsFilter!=0)//Not "All" or "Unassigned"
					 && apptCur.ClinicNum!=_clinicNumVerifyClinicsFilter) {
					continue;
				}
				if(_defNumVerifyRegionsFilter>=1 && !_listClinicsFiltered.Any(x => x.ClinicNum==apptCur.ClinicNum)) {
					continue;
				}
				Patient patCur = Patients.GetPat(apptCur.PatNum);
				if(patCur==null) {//Should never happen.  This means the patnum is orphaned and we don't want to show it anyways.
					continue;
				}
				List<PatPlan> listPatPlans = PatPlans.Refresh(patCur.PatNum);
				foreach(PatPlan patPlanCur in listPatPlans) {
					row=new AssignGridRow();
					InsSub insSubCur = InsSubs.GetOne(patPlanCur.InsSubNum);
					if(insSubCur==null) {//Should never happen.  It means the patplan was orphaned and we can't display this insurance.
						continue;
					}
					InsPlan insPlanCur = InsPlans.GetPlan(insSubCur.PlanNum,null);
					if(insPlanCur==null) {//Should never happen.  It means the inssub row was orphaned and we can't display this insurance.
						continue;
					}
					Carrier carrierCur = Carriers.GetCarrier(insPlanCur.CarrierNum);
					if(carrierCur==null||!carrierCur.CarrierName.ToLower().Contains(textVerifyCarrier.Text.ToLower())) {
						continue;
					}
					InsVerify lastPatVerified = null;
					Tuple<long,VerifyTypes> key = new Tuple<long,VerifyTypes>(patPlanCur.PatPlanNum,VerifyTypes.PatientEnrollment);
					if(dictInsVerify.ContainsKey(key)) {
						lastPatVerified=dictInsVerify[key];
					}
					Userod userCur = null;
					DateTime dateLastPatVerified = DateTime.MinValue;
					string userNamePatVerify = "";
					string verifyStatus = "";
					if(lastPatVerified!=null) {
						//If the last date this patplan was verified hasn't been textPatientEnrollmentDays days ago, then don't show them on the list.
						if(lastPatVerified.DateLastVerified.AddDays(PIn.Int(textPatientEnrollmentDays.Text))>DateTime.Today) {
							continue;
						}
						dateLastPatVerified=lastPatVerified.DateLastVerified;
						userCur=Userods.GetUser(lastPatVerified.UserNum);
						if(userCur!=null) {
							userNamePatVerify=userCur.UserName;
						}
						Def defVerifyStatus = DefC.GetDef(DefCat.InsuranceVerificationStatus,lastPatVerified.DefNum);
						if(defVerifyStatus!=null) {
							verifyStatus=defVerifyStatus.ItemName;
						}
					}
					if(_verifyUserNum!=-1 && _verifyUserNum!=0 && userNamePatVerify!=textVerifyUser.Text) {
						continue;
					}
					if(_verifyUserNum==0 && userNamePatVerify!="") {//If they selected the None user or "Unassigned"
						continue;
					}
					if(comboFilterVerifyStatus.SelectedIndex!=0 && verifyStatus!=comboFilterVerifyStatus.Text) {
						continue;
					}
					string userNameInsVerify = "";
					InsVerify lastInsVerified = null;
					key=new Tuple<long,VerifyTypes>(insPlanCur.PlanNum,VerifyTypes.InsuranceBenefit);
					if(dictInsVerify.ContainsKey(key)) {
						lastInsVerified=dictInsVerify[key];
					}
					DateTime dateLastInsVerified = DateTime.MinValue;
					if(lastInsVerified!=null) {
						//If the last date this insurance was verified hasn't been textInsBenefitEligibilityDays days ago, then don't show them on the list.
						dateLastInsVerified=lastInsVerified.DateLastVerified;
						userCur=Userods.GetUser(lastInsVerified.UserNum);
						if(userCur!=null) {
							userNameInsVerify=userCur.UserName;
						}
					}
					row.Clinic=apptClinicName;
					row.PatientName=patCur.GetNameFL();
					row.NextApptDate=apptCur.AptDateTime;
					row.CarrierName=carrierCur.CarrierName;
					row.DatePatLastVerified=dateLastPatVerified;
					row.DateInsLastVerified=dateLastInsVerified;
					row.VerifyStatus=verifyStatus;
					row.AssignedTo=userNamePatVerify;
					InsVerifyWithPatNum insVerifyPatNumForBenefits = new InsVerifyWithPatNum(apptCur.PatNum,lastInsVerified);
					InsVerifyWithPatNum insVerifyPatNumForPatPlan = new InsVerifyWithPatNum(apptCur.PatNum,lastPatVerified);
					List<InsVerifyWithPatNum> listInsVerifiesToAssign = new List<InsVerifyWithPatNum>();
					listInsVerifiesToAssign.Add(insVerifyPatNumForPatPlan);
					if(lastInsVerified.DateLastVerified.AddDays(PIn.Int(textInsBenefitEligibilityDays.Text))<=DateTime.Today
						 && !insPlanCur.HideFromVerifyList) {
						listInsVerifiesToAssign.Add(insVerifyPatNumForBenefits);
					}
					row.Tag=listInsVerifiesToAssign;
					listGridRows.Add(row);
				}
			}
			return listGridRows;
		}

		private int CompareAssignGrid(AssignGridRow x,AssignGridRow y) {
			if(x.Clinic==y.Clinic && x.NextApptDate==y.NextApptDate) {
				return x.CarrierName.CompareTo(y.CarrierName);
			}
			if(x.Clinic==y.Clinic) {
				return x.NextApptDate.CompareTo(y.NextApptDate);
			}
			return x.Clinic.CompareTo(y.Clinic);
		}

		private ODGridRow AssignRowToODGridRow(AssignGridRow arow) {
			ODGridRow row = new ODGridRow();
			if(PrefC.HasClinicsEnabled) {
				row.Cells.Add(arow.Clinic);
			}
			row.Cells.Add(arow.PatientName);
			row.Cells.Add(arow.NextApptDate.ToShortDateString());
			row.Cells.Add(arow.CarrierName);
			row.Cells.Add(arow.DatePatLastVerified.ToShortDateString());
			row.Cells.Add(arow.DateInsLastVerified.ToShortDateString());
			row.Cells.Add(arow.VerifyStatus);
			row.Cells.Add(arow.AssignedTo);
			row.Tag=arow.Tag;
			return row;
		}

		private void gridAssign_CellClick(object sender,ODGridClickEventArgs e) {
			_listInsVerifiesSelected=((List<InsVerifyWithPatNum>)gridAssign.Rows[e.Row].Tag).Select(x => x.InsVerify).ToList();
			_patNumSelected=((List<InsVerifyWithPatNum>)gridAssign.Rows[e.Row].Tag)[0].PatNum;//All PatNums in list are the same.
		}
		#endregion

		#region Assigning Logic
		private void butAssignUser_Click(object sender,EventArgs e) {
			if(gridAssign.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an insurance to assign.");
				return;
			}
			if(_assignUserNum==0) {
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Are you sure you want to unassign the selected plan?")) {
					return;
				}
			}
			foreach(InsVerify insVerifyCur in _listInsVerifiesSelected) {
				insVerifyCur.DefNum=_defNumVerifyStatusAssign;
				insVerifyCur.UserNum=_assignUserNum;
				insVerifyCur.Note=textInsVerifyNote.Text;
				insVerifyCur.DateLastAssigned=DateTime.Today;
				InsVerifies.Update(insVerifyCur);
			}
			FillGrids();
		}

		private void gridAssign_MouseUp(object sender,MouseEventArgs e) {
			if(gridAssign.GetSelectedIndex()==-1) {
				return;
			}
			if(e.Button==MouseButtons.Right && gridAssign.SelectedIndices.Length>0) {
				_menuRightClick.Items.Clear();
				_menuRightClick.Items.Add(Lan.g(this,"Set User to")+" ("+textAssignUser.Text+")",null,new EventHandler(gridAssignRight_click));
				_menuRightClick.Items.Add(Lan.g(this,"Set Verify Status to")+" ("+comboSetVerifyStatus.Text+")",null,new EventHandler(gridAssignRight_click));
				_menuRightClick.Show(gridAssign,new Point(e.X,e.Y));
			}
		}

		private void gridAssignRight_click(object sender,System.EventArgs e) {
			switch(_menuRightClick.Items.IndexOf((ToolStripMenuItem)sender)) {
				case 0:
					if(_assignUserNum==0) {
						if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Are you sure you want to unassign the selected plan?")) {
							return;
						}
					}
					foreach(InsVerify insVerifyCur in _listInsVerifiesSelected) {
						insVerifyCur.UserNum=_assignUserNum;
						insVerifyCur.DateLastAssigned=DateTime.Today;
						InsVerifies.Update(insVerifyCur);
					}
					break;
				case 1:
					foreach(InsVerify insVerifyCur in _listInsVerifiesSelected) {
						insVerifyCur.DefNum=_defNumVerifyStatusAssign;
						InsVerifies.Update(insVerifyCur);
					}
					break;
			}
			FillGrids();
		}

		private void butAssignUserPick_Click(object sender,EventArgs e) {
			PickUser(true);
		}

		private void comboSetVerifyStatus_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboSetVerifyStatus.SelectedIndex<1) {
				_defNumVerifyStatusAssign=0;
				return;
			}
			_defNumVerifyStatusAssign=DefC.Short[(int)DefCat.InsuranceVerificationStatus][comboSetVerifyStatus.SelectedIndex-1].DefNum;
		}
		#endregion

		#region Grid Filters
		private void butVerifyUserPick_Click(object sender,EventArgs e) {
			PickUser(false);
			FillGrids();
		}

		private void comboFilterVerifyStatus_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboFilterVerifyStatus.SelectedIndex<1) {
				_defNumVerifyStatusFilter=0;
			}
			else {
				_defNumVerifyStatusFilter=DefC.Short[(int)DefCat.InsuranceVerificationStatus][comboFilterVerifyStatus.SelectedIndex-1].DefNum;
			}
			FillGrids();
		}

		private void comboVerifyClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			if(Security.CurUser.ClinicIsRestricted) {
				if(_defNumVerifyRegionsFilter<1) {
					_clinicNumVerifyClinicsFilter=_listClinicsDb[comboVerifyClinics.SelectedIndex].ClinicNum;
				}
				else if(_defNumVerifyRegionsFilter>=1) {
					_clinicNumVerifyClinicsFilter=_listClinicsFiltered[comboVerifyClinics.SelectedIndex].ClinicNum;
				}
			}
			else {
				if(comboVerifyClinics.SelectedIndex<1) {
					_clinicNumVerifyClinicsFilter=-1;
				}
				else if(_defNumVerifyRegionsFilter<1) {
					if(comboVerifyClinics.SelectedIndex==comboVerifyClinics.Items.Count-1) {
						_clinicNumVerifyClinicsFilter=0;
					}
					else {
						_clinicNumVerifyClinicsFilter=_listClinicsDb[comboVerifyClinics.SelectedIndex-1].ClinicNum;
					}
				}
				else if(_defNumVerifyRegionsFilter>=1) {
					_clinicNumVerifyClinicsFilter=_listClinicsFiltered[comboVerifyClinics.SelectedIndex-1].ClinicNum;
				}
			}
			FillGrids();
		}

		private void comboVerifyRegions_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboVerifyRegions.SelectedIndex<1) {
				_defNumVerifyRegionsFilter=-1;
			}
			else {
				_defNumVerifyRegionsFilter=DefC.Short[(int)DefCat.Regions][comboVerifyRegions.SelectedIndex-1].DefNum;
			}
			FillGrids();
		}

		private void textVerifyCarrier_TextChanged(object sender,EventArgs e) {
			timerRefresh.Stop();
			timerRefresh.Start();
		}

		private void textAppointmentScheduledDays_TextChanged(object sender,EventArgs e) {
			timerRefresh.Stop();
			timerRefresh.Start();
		}

		private void textInsBenefitEligibilityDays_TextChanged(object sender,EventArgs e) {
			timerRefresh.Stop();
			timerRefresh.Start();
		}

		private void textPatientEnrollmentDays_TextChanged(object sender,EventArgs e) {
			timerRefresh.Stop();
			timerRefresh.Start();
		}

		private void timerRefresh_Tick(object sender,EventArgs e) {
			//This timer was set by textVerifyCarrier_TextChanged in order to prevent refreshing too frequently.
			timerRefresh.Stop();
			FillGrids();
		}
		#endregion

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

		private class InsVerifyWithPatNum {
			public long PatNum;
			public InsVerify InsVerify;

			public InsVerifyWithPatNum(long patNum,InsVerify insVerify) {
				this.PatNum=patNum;
				this.InsVerify=insVerify;
			}
		}

		private class VerifyGridRow {
			public string Type;
			public string Clinic;
			public string PatientName;
			public DateTime NextApptDate;
			public string CarrierName;
			public DateTime DateLastVerified;
			public string VerifyStatus;
			public DateTime DateLastAssigned;
			public string AssignedTo;
			public Object Tag;
		}

		private class AssignGridRow {
			public string Clinic;
			public string PatientName;
			public DateTime NextApptDate;
			public string CarrierName;
			public DateTime DatePatLastVerified;
			public DateTime DateInsLastVerified;
			public string VerifyStatus;
			public string AssignedTo;
			public Object Tag;
		}
	}
}