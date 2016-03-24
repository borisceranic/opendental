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
		private List<Clinic> _listClinicsDb;
		private List<Clinic> _listClinicsFiltered;
		private List<Userod> _listUsersInRegionWithAssignedIns=new List<Userod>();
		private List<Userod> _listUsersInRegion=new List<Userod>();
		private List<Def> _listVerifyStatuses=new List<Def>();
		private long _userNumVerifyGrid=0;

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

		private void butRefresh_Click(object sender,EventArgs e) {
			FillGrids();
		}

		public void FillGrids() {
			FillComboBoxes();
			FillUsers();
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
					PatPlan patPlanVerify=PatPlans.GetByPatPlanNum(insVerifySelected.FKey);
					if(patPlanVerify==null) {//Should never happen, but if it does, return because it is just for display purposes.
						return;
					}
					InsSub insSubVerify=InsSubs.GetOne(patPlanVerify.InsSubNum);
					textSubscriberID.Text=insSubVerify.SubscriberID;
					Patient patSubscriberVerify=Patients.GetPat(insSubVerify.Subscriber);
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
			Employer employer=Employers.GetEmployer(insPlanVerify.EmployerNum);
			if(employer!=null) {
				textInsPlanEmployer.Text=employer.EmpName;
			}
			Carrier carrierVerify=Carriers.GetCarrier(insPlanVerify.CarrierNum);
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
			_listVerifyStatuses=DefC.Short[(int)DefCat.InsuranceVerificationStatus].ToList();
			for(int i=0;i<_listVerifyStatuses.Count;i++) {
				comboFilterVerifyStatus.Items.Add(_listVerifyStatuses[i].ItemName);
				comboSetVerifyStatus.Items.Add(_listVerifyStatuses[i].ItemName);
				if(_listVerifyStatuses[i].DefNum==_defNumVerifyStatusFilter) {
					comboFilterVerifyStatus.SelectedIndex=i+1;
				}
				if(_listVerifyStatuses[i].DefNum==_defNumVerifyStatusAssign) {
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
				List<Def> listRegionDefs=DefC.Short[(int)DefCat.Regions].ToList();
				if(listRegionDefs.Count!=0) {
					listRegionDefs.RemoveAll(x => !_listClinicsDb.Any(y => y.Region==x.DefNum));
					comboVerifyRegions.Items.Add(Lan.g(this,"All"));
					for(int i=0;i<listRegionDefs.Count;i++) {
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
			int indexOffset=0;
			if(!Security.CurUser.ClinicIsRestricted) {
				comboVerifyClinics.Items.Add(Lan.g(this,"All"));
				indexOffset=1;
			}
			if(_defNumVerifyRegionsFilter<1) {
				for(int i=0;i<_listClinicsDb.Count;i++) {
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
				for(int i=0;i<_listClinicsFiltered.Count;i++) {
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
			comboVerifyUser.Items.Clear();
			comboVerifyUser.SelectedIndex=-1;
			comboVerifyUser.Items.Add("All Users");
			comboVerifyUser.Items.Add("Unassigned");
			List<long> listClinicNums=new List<long>();
			if(_clinicNumVerifyClinicsFilter!=-1) {
				listClinicNums=new List<long>() { _clinicNumVerifyClinicsFilter };
			}
			else if(_defNumVerifyRegionsFilter>0) {
				listClinicNums=Clinics.GetListByRegion(_defNumVerifyRegionsFilter);
			}
			_listUsersInRegion=Userods.GetUsersForVerifyList(listClinicNums,true);
			_listUsersInRegionWithAssignedIns=Userods.GetUsersForVerifyList(listClinicNums,false);
			for(int i=0;i<_listUsersInRegionWithAssignedIns.Count;i++) {
				comboVerifyUser.Items.Add(_listUsersInRegionWithAssignedIns[i].UserName);
				if(_verifyUserNum==_listUsersInRegionWithAssignedIns[i].UserNum) {
					comboVerifyUser.SelectedIndex=i+2;//Add 2 because of the "All Users" and "Unassigned" combo items.
				}
			}
			if(_verifyUserNum==-1) {
				comboVerifyUser.SelectedIndex=0;//"All Users"
			}
			if(_verifyUserNum==0) {
				comboVerifyUser.SelectedIndex=1;//"Unassigned"
			}
			for(int i=0;i<_listUsersInRegion.Count;i++) {
				if(_assignUserNum==_listUsersInRegion[i].UserNum) {
					textAssignUser.Text=_listUsersInRegion[i].UserName;
				}
			}
			if(_assignUserNum==0) {
				textAssignUser.Text="Unassign";
			}
		}

		private void PickUser(bool isAssigning) {
			FormUserPick FormUP=new FormUserPick();
			FormUP.IsSelectionmode=true;
			FormUP.ListUserodsFiltered=_listUsersInRegion;
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
					FillGrids();
				}
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
			col=new ODGridColumn(Lans.g(this,"Appt Date"),70,HorizontalAlignment.Center,GridSortingStrategy.DateParse);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Carrier"),160);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Last Verified"),90,HorizontalAlignment.Center,GridSortingStrategy.DateParse);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Status"),110);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Status Date"),80,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Assigned to"),0);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			List<VerifyGridRow> listGridRows=GetRowsForVerifyGrid();
			listGridRows.Sort(CompareVerifyGrid);
			for(int i=0;i<listGridRows.Count;i++) {
				gridMain.Rows.Add(VerifyRowToODGridRow(listGridRows[i]));
			}
			gridMain.EndUpdate();
		}

		private List<VerifyGridRow> GetRowsForVerifyGrid() {
			List<VerifyGridRow> listGridRows=new List<VerifyGridRow>();
			List<long> listInsPlanVerifyNums=new List<long>();
			List<InsVerify> insVerifiesForGrid=InsVerifies.GetAll();
			Dictionary<Tuple<long,VerifyTypes>,InsVerify> dictInsVerify=new Dictionary<Tuple<long,VerifyTypes>,InsVerify>();
			foreach(InsVerify insVerifyCur in insVerifiesForGrid) {
				//Add the Fkey and VerifyTypes combination as the key for the dictionary to get the InsVerify object.
				//This will be used to save calls to the database instead of calling GetOne() every time we need an InsVerify object.
				dictInsVerify.Add(new Tuple<long,VerifyTypes>(insVerifyCur.FKey,insVerifyCur.VerifyType),insVerifyCur);
			}
			DateTime dateTimeStart=DateTime.Today;
			//End the following day at midnight.  There shouldn't be any appointments at midnight, and if there is they get to verify slightly early.
			DateTime dateTimeEnd=DateTime.Today.AddDays(PIn.Int(textAppointmentScheduledDays.Text)+1);
			List<Appointment> listSchedAppts=Appointments.GetAppointmentsForPeriod(dateTimeStart,dateTimeEnd);
			VerifyGridRow row;
			foreach(Appointment apptCur in listSchedAppts) {
				Clinic clinicCur=Clinics.GetClinic(apptCur.ClinicNum);
				//ClinicNum was either invalid or has a region that isn't in the restricted clinic region list, so don't display the appointment.
				string apptClinicName="";
				if(clinicCur!=null) {
					apptClinicName=clinicCur.Description;
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
				Patient patCur=Patients.GetPat(apptCur.PatNum);
				if(patCur==null) {//Should never happen.  This means the patnum is orphaned and we don't want to show it anyways.
					continue;
				}
				List<PatPlan> listPatPlans=PatPlans.Refresh(patCur.PatNum);
				foreach(PatPlan patPlanCur in listPatPlans) {
					row=new VerifyGridRow();
					InsSub insSubCur=InsSubs.GetOne(patPlanCur.InsSubNum);
					if(insSubCur==null) {//Should never happen.  It means the patplan was orphaned and we can't display this insurance.
						continue;
					}
					InsPlan insPlanCur=InsPlans.GetPlan(insSubCur.PlanNum,null);
					if(insPlanCur==null) {//Should never happen.  It means the inssub row was orphaned and we can't display this insurance.
						continue;
					}
					Carrier carrierCur=Carriers.GetCarrier(insPlanCur.CarrierNum);
					if(carrierCur==null || !carrierCur.CarrierName.ToLower().Contains(textVerifyCarrier.Text.ToLower())) {
						continue;
					}
					Userod userCur=null;
					DateTime dateLastPatVerified=DateTime.MinValue;
					string userNamePatVerify="";
					string verifyStatus="";
					#region Patient Enrollment Row
					InsVerify lastPatVerified=null;
					Tuple<long,VerifyTypes> key=new Tuple<long,VerifyTypes>(patPlanCur.PatPlanNum,VerifyTypes.PatientEnrollment);
					if(dictInsVerify.ContainsKey(key)) {
						lastPatVerified=dictInsVerify[key];
					}
					DateTime dateLastAssigned=DateTime.MinValue;
					if(lastPatVerified!=null) {
						dateLastPatVerified=lastPatVerified.DateLastVerified;
						userCur=Userods.GetUser(lastPatVerified.UserNum);
						if(userCur!=null) {
							userNamePatVerify=userCur.UserName;
						}
						Def defVerifyStatus=DefC.GetDef(DefCat.InsuranceVerificationStatus,lastPatVerified.DefNum);
						if(defVerifyStatus!=null) {
							verifyStatus=defVerifyStatus.ItemName;
						}
						dateLastAssigned=lastPatVerified.DateLastAssigned;
					}
					//Only add the patplan row if it hasn't been verified in the days prior specified
					if(lastPatVerified.DateLastVerified.AddDays(PIn.Int(textPatientEnrollmentDays.Text))<=DateTime.Today
						&& ((_verifyUserNum!=-1 && _verifyUserNum!=0 && userNamePatVerify==comboVerifyUser.Text)//Has a user filter and the name matches the ins verify name
						|| (_verifyUserNum==0 && userNamePatVerify=="")//Has the "Unassigned" user filter and there is no username
						|| _verifyUserNum==-1)//Has "All Users" selected, so don't filter.
						&& (comboFilterVerifyStatus.SelectedIndex==0 || verifyStatus==comboFilterVerifyStatus.Text))
					{
						row.Type="Pat";
						row.Clinic=apptClinicName;
						row.PatientName=patCur.GetNameFL();
						row.NextApptDate=apptCur.AptDateTime;
						row.CarrierName=carrierCur.CarrierName;
						row.DateLastVerified=dateLastPatVerified;
						row.Tag=lastPatVerified;
						row.VerifyStatus=verifyStatus;
						row.DateLastAssigned=dateLastAssigned;
						row.AssignedTo=userNamePatVerify;
						listGridRows.Add(row);//Add patplan if !isAssignList 
					}
					#endregion
					#region Insurance Benefits Row
					if(insPlanCur.HideFromVerifyList) {
						continue;
					}
					if(listInsPlanVerifyNums.Contains(insPlanCur.PlanNum)) {
						continue;
					}
					string userNameInsVerify="";
					verifyStatus="";
					dateLastAssigned=DateTime.MinValue;
					InsVerify lastInsVerified=null;
					key=new Tuple<long,VerifyTypes>(insPlanCur.PlanNum,VerifyTypes.InsuranceBenefit);
					if(dictInsVerify.ContainsKey(key)) {
						lastInsVerified=dictInsVerify[key];
					}
					DateTime dateLastInsVerified=DateTime.MinValue;
					if(lastInsVerified!=null) {
						userCur=Userods.GetUser(lastInsVerified.UserNum);
						if(userCur!=null) {
							userNameInsVerify=userCur.UserName;
						}
						dateLastInsVerified=lastInsVerified.DateLastVerified;
						userCur=Userods.GetUser(lastInsVerified.UserNum);
						if(userCur!=null) {
							userNameInsVerify=userCur.UserName;
						}
						Def defVerifyStatus=DefC.GetDef(DefCat.InsuranceVerificationStatus,lastInsVerified.DefNum);
						if(defVerifyStatus!=null) {
							verifyStatus=defVerifyStatus.ItemName;
						}
						dateLastAssigned=lastInsVerified.DateLastAssigned;
					}
					if(lastInsVerified.DateLastVerified.AddDays(PIn.Int(textInsBenefitEligibilityDays.Text))<=DateTime.Today
						&& ((_verifyUserNum!=-1 && _verifyUserNum!=0 && userNameInsVerify==comboVerifyUser.Text)//Has a user filter and the name matches the ins verify name
						|| (_verifyUserNum==0 && userNameInsVerify=="")//Has the "Unassigned" user filter and there is no username
						|| _verifyUserNum==-1)//Has "All Users" selected, so don't filter.
						&& (comboFilterVerifyStatus.SelectedIndex==0 || verifyStatus==comboFilterVerifyStatus.Text)
						&& !insPlanCur.HideFromVerifyList) 
					{
						row=new VerifyGridRow();
						row.Type="Ins";
						row.Clinic=apptClinicName;
						row.PatientName="";
						row.NextApptDate=apptCur.AptDateTime;
						row.CarrierName=carrierCur.CarrierName;
						row.DateLastVerified=dateLastInsVerified;
						row.VerifyStatus=verifyStatus;
						row.DateLastAssigned=dateLastAssigned;
						row.AssignedTo=userNameInsVerify;
						row.Tag=lastInsVerified;
						listGridRows.Add(row);
						listInsPlanVerifyNums.Add(insPlanCur.PlanNum);//add PlanNum to the list so that this insurance plan doesn't add another row to the grid. 
					}
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
			return y.Type.CompareTo(x.Type);//x and y are flipped to order by Type descending (Z-A)
		}

		private ODGridRow VerifyRowToODGridRow(VerifyGridRow vrow) {
			ODGridRow row=new ODGridRow();
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
			_insVerifySelected=((InsVerify)gridMain.Rows[e.Row].Tag);
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
			InsVerify insVerifyCur=((InsVerify)gridMain.Rows[gridMain.GetSelectedIndex()].Tag);
			string verifyType="";
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
				string verifyDescription=Lan.g(this,"Go to Insurance Plan");
				if(_insVerifySelected.VerifyType==VerifyTypes.PatientEnrollment) {
					verifyDescription=Lan.g(this,"Go to Patient Plan");
				}
				_menuRightClick.Items.Add(verifyDescription,null,new EventHandler(gridMainRight_click));
				ToolStripMenuItem assignUserToolItem=new ToolStripMenuItem(Lan.g(this,"Assign to User"));
				foreach(Userod user in _listUsersInRegion) {
					ToolStripMenuItem assignUserDropDownCur=new ToolStripMenuItem(user.UserName);
					assignUserDropDownCur.Tag=user;
					assignUserDropDownCur.Click+=new EventHandler(assignUserToolItemDropDown_Click);
					assignUserToolItem.DropDownItems.Add(assignUserDropDownCur);
				}
				_menuRightClick.Items.Add(assignUserToolItem);
				ToolStripMenuItem verifyStatusToolItem=new ToolStripMenuItem(Lan.g(this,"Set Verify Status to"));
				foreach(Def status in _listVerifyStatuses) {
					ToolStripMenuItem verifyStatusDropDownCur=new ToolStripMenuItem(status.ItemName);
					verifyStatusDropDownCur.Tag=status;
					verifyStatusDropDownCur.Click+=new EventHandler(verifyStatusToolItemDropDown_Click);
					verifyStatusToolItem.DropDownItems.Add(verifyStatusDropDownCur);
				}
				_menuRightClick.Items.Add(verifyStatusToolItem);
				_menuRightClick.Items.Add(Lan.g(this,"Verify"),null,new EventHandler(gridMainRight_click));
				_menuRightClick.Show(gridMain,new Point(e.X,e.Y));
			}
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			OnOpenInsPlan();
		}

		private void OnOpenInsPlan() {	
			FormInsPlan FormIP;
			if(_insVerifySelected.VerifyType==VerifyTypes.InsuranceBenefit) {
				FormIP=new FormInsPlan(InsPlans.GetPlan(_insVerifySelected.FKey,new List<InsPlan>()),null,null);
				FormIP.ShowDialog();
				if(FormIP.DialogResult==DialogResult.OK) {
					FillGrids();
				}
			}
			else if(_insVerifySelected.VerifyType==VerifyTypes.PatientEnrollment) {
				PatPlan pp=PatPlans.GetByPatPlanNum(_insVerifySelected.FKey);
				InsSub insSub=InsSubs.GetOne(pp.InsSubNum);
				InsPlan ip=InsPlans.GetPlan(insSub.PlanNum,new List<InsPlan>());
				FormIP=new FormInsPlan(ip,pp,insSub);
				FormIP.ShowDialog();
				if(FormIP.DialogResult==DialogResult.OK) {
					FillGrids();
				}
			}
		}

		private void gridMainRight_click(object sender,System.EventArgs e) {
			switch(_menuRightClick.Items.IndexOf((ToolStripMenuItem)sender)) {
				case 0:
					OnOpenInsPlan();
					break;
				case 1:
					//No need for action on Assign click
					break;
				case 2:
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
			col=new ODGridColumn(Lans.g(this,"Type"),35);
			gridAssign.Columns.Add(col);
			if(PrefC.HasClinicsEnabled) {
				col=new ODGridColumn(Lans.g(this,"Clinic"),90);
				gridAssign.Columns.Add(col);
			}
			col=new ODGridColumn(Lans.g(this,"Patient"),120);
			gridAssign.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Appt Date"),70,GridSortingStrategy.DateParse);
			col.TextAlign=HorizontalAlignment.Center;
			gridAssign.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Carrier"),160);
			gridAssign.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Last Verified"),90,GridSortingStrategy.DateParse);
			col.TextAlign=HorizontalAlignment.Center;
			gridAssign.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Status"),110);
			gridAssign.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Assigned to"),0);
			gridAssign.Columns.Add(col);
			gridAssign.Rows.Clear();
			List<AssignGridRow> listGridRows=GetRowsForAssignGrid();
			listGridRows.Sort(CompareAssignGrid);
			for(int i=0;i<listGridRows.Count;i++) {
				gridAssign.Rows.Add(AssignRowToODGridRow(listGridRows[i]));
			}
			gridAssign.EndUpdate();
		}

		private List<AssignGridRow> GetRowsForAssignGrid() {
			List<AssignGridRow> listGridRows=new List<AssignGridRow>();
			List<InsVerify> insVerifiesForGrid=InsVerifies.GetAll();
			Dictionary<Tuple<long,VerifyTypes>,InsVerify> dictInsVerify=new Dictionary<Tuple<long,VerifyTypes>,InsVerify>();
			foreach(InsVerify insVerifyCur in insVerifiesForGrid) {
				//Add the Fkey and VerifyTypes combination as the key for the dictionary to get the InsVerify object.
				//This will be used to save calls to the database instead of calling GetOne() every time we need an InsVerify object.
				dictInsVerify.Add(new Tuple<long,VerifyTypes>(insVerifyCur.FKey,insVerifyCur.VerifyType),insVerifyCur);
			}
			DateTime dateTimeStart=DateTime.Today;
			//End the following day at midnight.  There shouldn't be any appointments at midnight, and if there is they get to verify slightly early.
			DateTime dateTimeEnd=DateTime.Today.AddDays(PIn.Int(textAppointmentScheduledDays.Text)+1);
			List<Appointment> listSchedAppts=Appointments.GetAppointmentsForPeriod(dateTimeStart,dateTimeEnd);
			AssignGridRow row;
			foreach(Appointment apptCur in listSchedAppts) {
				Clinic clinicCur=Clinics.GetClinic(apptCur.ClinicNum);
				//ClinicNum was either invalid or has a region that isn't in the restricted clinic region list, so don't display the appointment.
				string apptClinicName="";
				if(clinicCur!=null) {
					apptClinicName=clinicCur.Description;
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
				Patient patCur=Patients.GetPat(apptCur.PatNum);
				if(patCur==null) {//Should never happen.  This means the patnum is orphaned and we don't want to show it anyways.
					continue;
				}
				List<PatPlan> listPatPlans=PatPlans.Refresh(patCur.PatNum);
				foreach(PatPlan patPlanCur in listPatPlans) {
					InsSub insSubCur=InsSubs.GetOne(patPlanCur.InsSubNum);
					if(insSubCur==null) {//Should never happen.  It means the patplan was orphaned and we can't display this insurance.
						continue;
					}
					InsPlan insPlanCur=InsPlans.GetPlan(insSubCur.PlanNum,null);
					if(insPlanCur==null) {//Should never happen.  It means the inssub row was orphaned and we can't display this insurance.
						continue;
					}
					Carrier carrierCur=Carriers.GetCarrier(insPlanCur.CarrierNum);
					if(carrierCur==null||!carrierCur.CarrierName.ToLower().Contains(textVerifyCarrier.Text.ToLower())) {
						continue;
					}
					InsVerify lastPatVerified=null;
					Tuple<long,VerifyTypes> key=new Tuple<long,VerifyTypes>(patPlanCur.PatPlanNum,VerifyTypes.PatientEnrollment);
					if(dictInsVerify.ContainsKey(key)) {
						lastPatVerified=dictInsVerify[key];
					}
					Userod userCur=null;
					DateTime dateLastPatVerified=DateTime.MinValue;
					string userNamePatVerify="";
					string verifyStatus="";
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
						Def defVerifyStatus=DefC.GetDef(DefCat.InsuranceVerificationStatus,lastPatVerified.DefNum);
						if(defVerifyStatus!=null) {
							verifyStatus=defVerifyStatus.ItemName;
						}
					}
					//Only add the patplan row if it hasn't been verified in the days prior specified
					if(lastPatVerified.DateLastVerified.AddDays(PIn.Int(textPatientEnrollmentDays.Text))<=DateTime.Today
						&& ((_verifyUserNum!=-1 && _verifyUserNum!=0 && userNamePatVerify==comboVerifyUser.Text)//Has a user filter and the name matches the ins verify name
						|| (_verifyUserNum==0 && userNamePatVerify=="")//Has the "Unassigned" user filter and there is no username
						|| _verifyUserNum==-1)//Has "All Users" selected, so don't filter.
						&& (comboFilterVerifyStatus.SelectedIndex==0 || verifyStatus==comboFilterVerifyStatus.Text))
					{
						row=new AssignGridRow();
						row.Type="Pat";
						row.Clinic=apptClinicName;
						row.PatientName=patCur.GetNameFL();
						row.NextApptDate=apptCur.AptDateTime;
						row.CarrierName=carrierCur.CarrierName;
						row.DateLastVerified=dateLastPatVerified;
						row.Tag=lastPatVerified;
						row.VerifyStatus=verifyStatus;
						row.AssignedTo=userNamePatVerify;
						listGridRows.Add(row);//Add patplan if !isAssignList 
					}
					string userNameInsVerify="";
					verifyStatus="";
					InsVerify lastInsVerified=null;
					key=new Tuple<long,VerifyTypes>(insPlanCur.PlanNum,VerifyTypes.InsuranceBenefit);
					if(dictInsVerify.ContainsKey(key)) {
						lastInsVerified=dictInsVerify[key];
					}
					DateTime dateLastInsVerified=DateTime.MinValue;
					if(lastInsVerified!=null) {
						//If the last date this insurance was verified hasn't been textInsBenefitEligibilityDays days ago, then don't show them on the list.
						dateLastInsVerified=lastInsVerified.DateLastVerified;
						userCur=Userods.GetUser(lastInsVerified.UserNum);
						if(userCur!=null) {
							userNameInsVerify=userCur.UserName;
						}
						Def defVerifyStatus=DefC.GetDef(DefCat.InsuranceVerificationStatus,lastInsVerified.DefNum);
						if(defVerifyStatus!=null) {
							verifyStatus=defVerifyStatus.ItemName;
						}
					}
					if(lastInsVerified.DateLastVerified.AddDays(PIn.Int(textInsBenefitEligibilityDays.Text))<=DateTime.Today
						&& ((_verifyUserNum!=-1 && _verifyUserNum!=0 && userNameInsVerify==comboVerifyUser.Text)//Has a user filter and the name matches the ins verify name
						|| (_verifyUserNum==0 && userNameInsVerify=="")//Has the "Unassigned" user filter and there is no username
						|| _verifyUserNum==-1)//Has "All Users" selected, so don't filter.
						&& (comboFilterVerifyStatus.SelectedIndex==0 || verifyStatus==comboFilterVerifyStatus.Text)
						&& !insPlanCur.HideFromVerifyList) 
					{
						row=new AssignGridRow();
						row.Type="Ins";
						row.Clinic=apptClinicName;
						row.PatientName="";
						row.NextApptDate=apptCur.AptDateTime;
						row.CarrierName=carrierCur.CarrierName;
						row.DateLastVerified=dateLastInsVerified;
						row.VerifyStatus=verifyStatus;
						row.AssignedTo=userNameInsVerify;
						row.Tag=lastInsVerified;
						listGridRows.Add(row);
					}
				}
			}
			return listGridRows;
		}

		private int CompareAssignGrid(AssignGridRow x,AssignGridRow y) {
			if(x.Type==y.Type && x.Clinic==y.Clinic && x.NextApptDate==y.NextApptDate) {
				return x.CarrierName.CompareTo(y.CarrierName);
			}
			if(x.Type==y.Type && x.Clinic==y.Clinic) {
				return x.NextApptDate.CompareTo(y.NextApptDate);
			}
			if(x.Type==y.Type) {
				return x.Clinic.CompareTo(y.Clinic);
			}
			return y.Type.CompareTo(x.Type);//x and y are flipped to order by Type descending (Z-A)
		}

		private ODGridRow AssignRowToODGridRow(AssignGridRow arow) {
			ODGridRow row=new ODGridRow();
			row.Cells.Add(arow.Type);
			if(PrefC.HasClinicsEnabled) {
				row.Cells.Add(arow.Clinic);
			}
			row.Cells.Add(arow.PatientName);
			row.Cells.Add(arow.NextApptDate.ToShortDateString());
			row.Cells.Add(arow.CarrierName);
			row.Cells.Add(arow.DateLastVerified.ToShortDateString());
			row.Cells.Add(arow.VerifyStatus);
			row.Cells.Add(arow.AssignedTo);
			row.Tag=arow.Tag;
			return row;
		}

		private List<InsVerify> GetSelectedInsVerifyList() {
			List<InsVerify> listInsVerifiesSelected=new List<InsVerify>();
			for(int i=0;i<gridAssign.SelectedIndices.Length;i++) {
				listInsVerifiesSelected.Add(((InsVerify)gridAssign.Rows[gridAssign.SelectedIndices[i]].Tag));
			}
			return listInsVerifiesSelected;
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
			List<InsVerify> listInsVerifiesSelected=GetSelectedInsVerifyList();
			foreach(InsVerify insVerifyCur in listInsVerifiesSelected) {
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
				ToolStripMenuItem assignUserToolItem=new ToolStripMenuItem(Lan.g(this,"Assign to User"));
				foreach(Userod user in _listUsersInRegion) {
					ToolStripMenuItem assignUserDropDownCur=new ToolStripMenuItem(user.UserName);
					assignUserDropDownCur.Tag=user;
					assignUserDropDownCur.Click+=new EventHandler(assignUserToolItemDropDown_Click);
					assignUserToolItem.DropDownItems.Add(assignUserDropDownCur);
				}
				_menuRightClick.Items.Add(assignUserToolItem);
				ToolStripMenuItem verifyStatusToolItem=new ToolStripMenuItem(Lan.g(this,"Set Verify Status to"));
				foreach(Def status in _listVerifyStatuses) {
					ToolStripMenuItem verifyStatusDropDownCur=new ToolStripMenuItem(status.ItemName);
					verifyStatusDropDownCur.Tag=status;
					verifyStatusDropDownCur.Click+=new EventHandler(verifyStatusToolItemDropDown_Click);
					verifyStatusToolItem.DropDownItems.Add(verifyStatusDropDownCur);
				}
				_menuRightClick.Items.Add(verifyStatusToolItem);
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
					List<InsVerify> listInsVerifiesSelected=GetSelectedInsVerifyList();
					foreach(InsVerify insVerifyCur in listInsVerifiesSelected) {
						insVerifyCur.UserNum=_assignUserNum;
						insVerifyCur.DateLastAssigned=DateTime.Today;
						InsVerifies.Update(insVerifyCur);
					}
					break;
				case 1:
					//Not clickable due to being a dropdown menu.
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
				comboSetVerifyStatus.Text="none";
			}
			else {
				_defNumVerifyStatusAssign=_listVerifyStatuses[comboSetVerifyStatus.SelectedIndex-1].DefNum;
				comboSetVerifyStatus.Text=_listVerifyStatuses[comboSetVerifyStatus.SelectedIndex-1].ItemName;
			}
			if(gridMain.GetSelectedIndex()!=-1) {
				SetStatus(_defNumVerifyStatusAssign,true);
			}
			FillGrids();
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
				_defNumVerifyStatusFilter=_listVerifyStatuses[comboFilterVerifyStatus.SelectedIndex-1].DefNum;
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

		private void comboVerifyUser_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboVerifyUser.SelectedIndex==1) {//Selected "Unassigned"
				_verifyUserNum=0;
			}
			else if(comboVerifyUser.SelectedIndex<1) {//Selected "All Users" or selected index is invalid
				_verifyUserNum=-1;
			}
			else {//Selected a real User.
				_verifyUserNum=_listUsersInRegionWithAssignedIns[comboVerifyUser.SelectedIndex-2].UserNum;
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

		private void SetStatus(long statusDefNum,bool isVerifyGrid) {
			string statusNote="";
			bool hasChanged=false;
			InputBox ib=new InputBox(Lan.g(this,"Add a status note:"));
			ib.setTitle(Lan.g(this,"Add Status Note"));
			ib.IsMultiline=true;
			if(!isVerifyGrid) {
				ib.textResult.Text=textInsVerifyNote.Text;
			}
			ib.ShowDialog();
			if(ib.DialogResult==DialogResult.OK) {
				statusNote=ib.textResult.Text;
				hasChanged=true;
			}
			if(isVerifyGrid) {
				_insVerifySelected.DefNum=statusDefNum;
				if(hasChanged) {
					_insVerifySelected.Note=statusNote;
				}
				_insVerifySelected.DateLastAssigned=DateTime.Today;
				InsVerifies.Update(_insVerifySelected);
			}
			else {
				List<InsVerify> listInsVerifiesSelected=GetSelectedInsVerifyList();
				foreach(InsVerify insVerifyCur in listInsVerifiesSelected) {
					insVerifyCur.DefNum=statusDefNum;
					if(hasChanged) {
						insVerifyCur.Note=statusNote;
					}
					insVerifyCur.DateLastAssigned=DateTime.Today;
					InsVerifies.Update(insVerifyCur);
				}
			}
		}

		private void verifyStatusToolItemDropDown_Click(object sender, EventArgs e) {
			Def status=(Def)((ToolStripMenuItem)sender).Tag;
			if(tabControl1.SelectedTab==tabVerify) {
				SetStatus(status.DefNum,true);
			}
			if(tabControl1.SelectedTab==tabAssign) {
				SetStatus(status.DefNum,false);
			}
			FillGrids();
		}

		private void assignUserToolItemDropDown_Click(object sender, EventArgs e) {
			Userod user=(Userod)((ToolStripMenuItem)sender).Tag;
			if(tabControl1.SelectedTab==tabVerify) {
				_insVerifySelected.UserNum=user.UserNum;
				_insVerifySelected.DateLastAssigned=DateTime.Today;
				InsVerifies.Update(_insVerifySelected);
			}
			if(tabControl1.SelectedTab==tabAssign) {
				List<InsVerify> listInsVerifiesSelected=GetSelectedInsVerifyList();
				foreach(InsVerify insVerifyCur in listInsVerifiesSelected) {
					insVerifyCur.UserNum=user.UserNum;
					insVerifyCur.DateLastAssigned=DateTime.Today;
					InsVerifies.Update(insVerifyCur);
				}
			}
			FillGrids();
		}

		private void tabControl1_Selected(object sender,TabControlEventArgs e) {
			if(e.TabPage==tabAssign) {
				if(_verifyUserNum!=0) {
					_userNumVerifyGrid=_verifyUserNum;
					_verifyUserNum=0;//Set filter user to Unassigned when switching to Assign tab.=
				}
			}
			else if(e.TabPage==tabVerify) {
				if(_verifyUserNum==0) {
					_verifyUserNum=_userNumVerifyGrid;
				}
			}
			FillGrids();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
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
			public string Type;
			public string Clinic;
			public string PatientName;
			public DateTime NextApptDate;
			public string CarrierName;
			public DateTime DateLastVerified;
			public string VerifyStatus;
			public string AssignedTo;
			public Object Tag;
		}
	}
}