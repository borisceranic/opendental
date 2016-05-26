using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Drawing;
using System.Text;

namespace OpenDental {
	public partial class FormMapHQ:ODForm {
		#region Private Members
		///<summary>Keep track of full screen state</summary>
		private bool _isFullScreen;
		///<summary>This is the difference between server time and local computer time.  Used to ensure that times displayed are accurate to the second.  This value is usally just a few seconds, but possibly a few minutes.</summary>
		private TimeSpan _timeDelta;
		private List<MapAreaContainer> _listMaps;
		private MapAreaContainer _mapCur;
		#endregion Private Members

		#region Initialize

		public FormMapHQ() {
			InitializeComponent();
			//Do not do anything to do with database or control init here. We will be using this ctor later in order to create a temporary object so we can figure out what size the form should be when the user comes back from full screen mode. Wait until FormMapHQ_Load to do anything meaningful.
			_isFullScreen=false;
			_timeDelta=MiscData.GetNowDateTime()-DateTime.Now;
			//Add the mousewheel event to allow mousewheel scrolling to repaint the grid as well.
			mapAreaPanelHQ.MouseWheel+=new MouseEventHandler(mapAreaPanelHQ_Scroll);
		}

		private void FormMapHQ_Load(object sender,EventArgs e) {
			FormOpenDental.AddMapToList(this);
			_listMaps=PhoneMapJSON.GetFromDb();
			_mapCur=_listMaps[0];
			FillCombo();
			FillMapAreaPanel();
			labelTriageOpsStaff.SetTriageColors();
		}
		private void FillCombo() {
			comboRoom.Items.Clear();
			foreach(MapAreaContainer mapCur in _listMaps) {
				comboRoom.Items.Add(mapCur.Description);
			}
			comboRoom.SelectedIndex=0;
		}

		///<summary>Setup the map panel with the cubicles and labels before filling with real-time data. Call this on load or anytime the cubicle layout has changed.</summary>
		private void FillMapAreaPanel() {
			mapAreaPanelHQ.Controls.Clear();
			mapAreaPanelHQ.FloorHeightFeet=_mapCur.FloorHeightFeet;
			mapAreaPanelHQ.FloorWidthFeet=_mapCur.FloorWidthFeet;
			//fill the panel
			List<MapArea> clinicMapItems=MapAreas.Refresh();
			for(int i=0;i<clinicMapItems.Count;i++) {
				if(clinicMapItems[i].MapAreaContainerNum!=_listMaps[comboRoom.SelectedIndex].MapAreaContainerNum) {
					continue;
				}
				if(clinicMapItems[i].ItemType==MapItemType.Room) {
					mapAreaPanelHQ.AddCubicle(clinicMapItems[i]);
				}
				else if(clinicMapItems[i].ItemType==MapItemType.DisplayLabel) {
					mapAreaPanelHQ.AddDisplayLabel(clinicMapItems[i]);
				}
			}
		}

		#endregion

		#region Set label text and colors

		public void SetEServiceMetrics(EServiceMetrics metricsToday) {
			eServiceMetricsControl.AccountBalance=metricsToday.AccountBalanceEuro;
			if(metricsToday.Severity==eServiceSignalSeverity.Critical) {
				eServiceMetricsControl.StartFlashing();
			}
			else {
				eServiceMetricsControl.StopFlashing();
			}
			switch(metricsToday.Severity) {
				case eServiceSignalSeverity.Working:
					eServiceMetricsControl.AlertColor=Color.LimeGreen;
					break;
				case eServiceSignalSeverity.Warning:
					eServiceMetricsControl.AlertColor=Color.Yellow;
					break;
				case eServiceSignalSeverity.Error:
					eServiceMetricsControl.AlertColor=Color.Orange;
					break;
				case eServiceSignalSeverity.Critical:
					eServiceMetricsControl.AlertColor=Color.Red;
					break;
			}
		}

		///<summary>Refresh the phone panel every X seconds after it has already been setup.  Make sure to call FillMapAreaPanel before calling this the first time.</summary>
		public void SetPhoneList(List<PhoneEmpDefault> peds,List<Phone> phones) {
			try {
				string title="Call Center Status Map - Triage Coordinator - ";
				try { //get the triage coord label but don't fail just because we can't find it
					title+=Employees.GetNameFL(PrefC.GetLong(PrefName.HQTriageCoordinator));
				}
				catch {
					title+="Not Set";
				}
				labelTriageCoordinator.Text=title;
				labelCurrentTime.Text=DateTime.Now.ToShortTimeString();
				int triageStaffCount=0;
				for(int i=0;i<this.mapAreaPanelHQ.Controls.Count;i++) { //loop through all of our cubicles and labels and find the matches
					if(!(this.mapAreaPanelHQ.Controls[i] is MapAreaRoomControl)) {
						continue;
					}
					MapAreaRoomControl room=(MapAreaRoomControl)this.mapAreaPanelHQ.Controls[i];
					if(room.MapAreaItem.Extension==0) { //This cubicle has not been given an extension yet.
						room.Empty=true;
						continue;
					}
					Phone phone=Phones.GetPhoneForExtension(phones,room.MapAreaItem.Extension);
					if(phone==null) {//We have a cubicle with no corresponding phone entry.
						room.Empty=true;
						continue;
					}
					PhoneEmpDefault phoneEmpDefault=PhoneEmpDefaults.GetEmpDefaultFromList(phone.EmployeeNum,peds);
					if(phoneEmpDefault==null) {//We have a cubicle with no corresponding phone emp default entry.
						room.Empty=true;
						continue;
					}
					//we got this far so we found a corresponding cubicle for this phone entry
					room.EmployeeNum=phone.EmployeeNum;
					room.EmployeeName=phone.EmployeeName;
					if(phone.DateTimeStart.Date==DateTime.Today) {
						TimeSpan span=DateTime.Now-phone.DateTimeStart+_timeDelta;
						DateTime timeOfDay=DateTime.Today+span;
						room.Elapsed=timeOfDay.ToString("H:mm:ss");
					}
					else {
						room.Elapsed="";
					}
					string status=ConvertClockStatusToString(phone.ClockStatus);
					//Check if the user is logged in.
					if(phone.ClockStatus==ClockStatusEnum.None
						|| phone.ClockStatus==ClockStatusEnum.Home) {
						status="Home";
					}
					room.Status=status;
					if(phone.Description=="") {
						room.PhoneImage=null;
					}
					else {
						room.PhoneImage=Properties.Resources.phoneInUse;
					}
					Color outerColor;
					Color innerColor;
					Color fontColor;
					bool isTriageOperatorOnTheClock=false;
					//get the cubicle color and triage status
					Phones.GetPhoneColor(phone,phoneEmpDefault,true,out outerColor,out innerColor,out fontColor,out isTriageOperatorOnTheClock);
					if(!room.IsFlashing) { //if the control is already flashing then don't overwrite the colors. this would cause a "spastic" flash effect.
						room.OuterColor=outerColor;
						room.InnerColor=innerColor;					
					}
					room.ForeColor=fontColor;
					if(isTriageOperatorOnTheClock) {
						triageStaffCount++;
					}
					if(phone.ClockStatus==ClockStatusEnum.NeedsHelp) { //turn on flashing
						room.StartFlashing();
					}
					else { //turn off flashing
						room.StopFlashing();
					}
					room.Invalidate(true);					
				}
				this.labelTriageOpsStaff.Text=triageStaffCount.ToString();
				SetEscalationList(peds,phones);
			}
			catch {
				//something failed unexpectedly
			}
		}

		private static string ConvertClockStatusToString(ClockStatusEnum status) {
			switch(status) {
				case ClockStatusEnum.Lunch:
					return "Lunch";
				case ClockStatusEnum.Break:
					return "Break";
				case ClockStatusEnum.Available:
					return "Avail";
				case ClockStatusEnum.WrapUp:
					return "WrapU";
				case ClockStatusEnum.Training:
					return "Train";
				case ClockStatusEnum.TeamAssist:
					return "TmAst";
				case ClockStatusEnum.OfflineAssist:
					return "OffAst";
				case ClockStatusEnum.Backup:
					return "BackU";
				case ClockStatusEnum.Unavailable:
					return "UnAv";
				case ClockStatusEnum.NeedsHelp:
					return "Help";
				case ClockStatusEnum.Off:
				case ClockStatusEnum.None:
				case ClockStatusEnum.Home:
				default:
					return status.ToString();
			}
		}

		private void SetEscalationList(List<PhoneEmpDefault> peds,List<Phone> phones) {
			try {
				escalationView.BeginUpdate();
				escalationView.Items.Clear();
				List<PhoneEmpDefault> listSorted=new List<PhoneEmpDefault>(peds);
				listSorted.Sort(new PhoneEmpDefaults.PhoneEmpDefaultComparer(PhoneEmpDefaults.PhoneEmpDefaultComparer.SortBy.escalation));
				for(int i=0;i<listSorted.Count;i++) {
					PhoneEmpDefault ped=listSorted[i];
					if(ped.EscalationOrder<=0) { //Filter out employees that do not have an escalation order set.
						continue;
					}
					Phone phone=Phones.GetPhoneForEmployeeNum(phones,ped.EmployeeNum);
					if(phone==null || phone.Description!="") { //Filter out invalid employees or employees that are already on the phone.
						continue;
					}
					if(phone.ClockStatus!=ClockStatusEnum.Available
					&& phone.ClockStatus!=ClockStatusEnum.Backup) { //Employees must either be Available or Backup.
						continue;
					}
					//We got this far so add the employee to the escalation list.
					escalationView.Items.Add(ped.EmpName);
				}
			}
			catch {
			}
			finally {
				escalationView.EndUpdate();
			}
		}
		
		public void SetOfficesDownList(List<Task> listOfficesDown) {
			try {
				officesDownView.BeginUpdate();
				officesDownView.Items.Clear();
				//Sort list by oldest.
				listOfficesDown.Sort(delegate(Task t1,Task t2) {
					return Comparer<DateTime>.Default.Compare(t1.DateTimeEntry,t2.DateTimeEntry);
				});
				for(int i=0;i<listOfficesDown.Count;i++) {
					Task task=listOfficesDown[i];
					if(task.TaskStatus==TaskStatusEnum.Done) { //Filter out old tasks. Should not be any but just in case.
						continue;
					}
					TimeSpan timeActive=DateTime.Now.Subtract(task.DateTimeEntry);
					//We got this far so the office is down.
					officesDownView.Items.Add(timeActive.ToStringHmmss()+" - "+task.KeyNum.ToString());
				}
			}
			catch {
			}
			finally {
				officesDownView.EndUpdate();
			}		
		}

		public void SetTriageUrgent(int calls,TimeSpan timeBehind) {
			this.labelTriageRedCalls.Text=calls.ToString();
			if(timeBehind==TimeSpan.Zero) { //format the string special for this case
				this.labelTriageRedTimeSpan.Text="00:00";
			}
			else {
				this.labelTriageRedTimeSpan.Text=timeBehind.ToStringmmss();
			}
			if(calls>1) { //we are behind
				labelTriageRedCalls.SetAlertColors();
			}
			else { //we are ok
				labelTriageRedCalls.SetNormalColors();
			}
			if(timeBehind>TimeSpan.FromMinutes(1)) { //we are behind
				labelTriageRedTimeSpan.SetAlertColors();
			}
			else { //we are ok
				labelTriageRedTimeSpan.SetNormalColors();
			}
		}

		public void SetVoicemailRed(int calls,TimeSpan timeBehind) {
			this.labelVoicemailCalls.Text=calls.ToString();
			if(timeBehind==TimeSpan.Zero) { //format the string special for this case
				this.labelVoicemailTimeSpan.Text="00:00";
			}
			else {
				this.labelVoicemailTimeSpan.Text=timeBehind.ToStringmmss();
			}
			if(calls>5) { //we are behind
				labelVoicemailCalls.SetAlertColors();
			}
			else { //we are ok
				labelVoicemailCalls.SetNormalColors();
			}
			if(timeBehind>TimeSpan.FromMinutes(5)) { //we are behind
				labelVoicemailTimeSpan.SetAlertColors();
			}
			else { //we are ok
				labelVoicemailTimeSpan.SetNormalColors();
			}
		}
		
		///<summary>Sets the time for current triage tasks and colors it according to how far behind we are.</summary>
		public void SetTriageNormal(int callsWithNotes,int callsWithNoNotes,TimeSpan timeBehind,int triageRed) {
			if(timeBehind==TimeSpan.Zero) { //format the string special for this case
				this.labelTriageTimeSpan.Text="0";
			}
			else {
				this.labelTriageTimeSpan.Text=((int)timeBehind.TotalMinutes).ToString();			
			}
			if(callsWithNoNotes>0 || triageRed>0) { //we have calls which don't have notes or a red triage task so display that number
				this.labelTriageCalls.Text=(callsWithNoNotes+triageRed).ToString();
			}
			else { //we don't have any calls with no notes nor any red triage tasks so display count of total tasks
				this.labelTriageCalls.Text="("+callsWithNotes.ToString()+")";
			}
			if(callsWithNoNotes+triageRed>10) { //we are behind
				labelTriageCalls.SetAlertColors();
			}
			else { //we are ok
				labelTriageCalls.SetNormalColors();
			}
			if(timeBehind>TimeSpan.FromMinutes(19)) { //we are behind
				labelTriageTimeSpan.SetAlertColors();
			}
			else if(timeBehind>TimeSpan.FromMinutes(9)) { //we are approaching being behind
				labelTriageTimeSpan.SetWarnColors();
			}
			else { //we are ok
				labelTriageTimeSpan.SetNormalColors();
			}
		}

		#endregion Set label text and colors

		private void comboRoom_SelectionChangeCommitted(object sender,EventArgs e) {
			_mapCur=_listMaps[comboRoom.SelectedIndex];
			FillMapAreaPanel();
		}

		private void fullScreenToolStripMenuItem_Click(object sender,EventArgs e) {
			_isFullScreen=!_isFullScreen;
			if(_isFullScreen) { //switch to full screen
				this.fullScreenToolStripMenuItem.Text="Restore";
				this.setupToolStripMenuItem.Visible=false;
				this.WindowState=FormWindowState.Normal;
				this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.None;
				this.Bounds=System.Windows.Forms.Screen.FromControl(this).Bounds;
				this.mapAreaPanelHQ.PixelsPerFoot=18;
			}
			else { //set back to defaults
				this.fullScreenToolStripMenuItem.Text="Full Screen";
				FormMapHQ FormCMS=new FormMapHQ();
				this.FormBorderStyle=FormCMS.FormBorderStyle;
				this.Size=FormCMS.Size;
				this.CenterToScreen();
				this.setupToolStripMenuItem.Visible=true;
				this.mapAreaPanelHQ.PixelsPerFoot=17;
			}
		}

		private void mapToolStripMenuItem_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormMapSetup FormMS=new FormMapSetup();
			FormMS.ShowDialog();
			if(FormMS.DialogResult!=DialogResult.OK) {
				return;
			}
			_listMaps=FormMS.ListMaps;
			_mapCur=_listMaps[comboRoom.SelectedIndex];
			FillCombo();
			FillMapAreaPanel();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"MapHQ layout changed");
		}

		private void escalationToolStripMenuItem_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormPhoneEmpDefaultEscalationEdit FormE=new FormPhoneEmpDefaultEscalationEdit();
			FormE.ShowDialog();
			SetEscalationList(PhoneEmpDefaults.Refresh(),Phones.GetPhoneList());
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Escalation team changed");
		}
		
		///<summary>Starts the timer that will refresh the panel so that ugly lines don't stay on the panel when scrolling.
		///The timer prevents lag due to repainting every time the scroll bar moves.</summary>
		private void mapAreaPanelHQ_Scroll(object sender,EventArgs e) {
			timer1.Stop();
			timer1.Start();
		}

		private void timer1_Tick(object sender,EventArgs e) {
			mapAreaPanelHQ.Refresh();
			timer1.Stop();
		}
		

		private void toggleTriageToolStripMenuItem_Click(object sender,EventArgs e) {
			splitContainer2.Panel1Collapsed=!splitContainer2.Panel1Collapsed;
		}

		private void openNewMapToolStripMenuItem_Click(object sender,EventArgs e) {
			new FormMapHQ().Show();
		}

		private void FormMapHQ_FormClosed(object sender,FormClosedEventArgs e) {
			FormOpenDental.RemoveMapFromList(this);
		}

	}
}
