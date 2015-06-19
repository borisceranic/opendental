using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormSmsTextMessaging:Form {

		private List<Clinic> _listClinics=null;
		///<summary>Set before showing this form.  If true, will initially show sent messages.  Otherwise will initially show received messages.</summary>
		public bool IsSent=false;
		///<summary>Allows FormSmsTextMessaging to update the unread SMS text message count in FormOpenDental as the user reads their messages.</summary>
		public delegate void SmsNotificationDelegate(string smsNotificationText,bool isSignalNeeded);
		///<summary>Set from FormOpenDental.  This can be null if the calling code does not wish to get dynamic unread message counts.</summary>
		public SmsNotificationDelegate SmsNotifier=null;
		///<summary>Set in FormOpenDental.  Initialize to the current number of unread text messages.
		///Used by FormSmsTextMessaging to determine when to call the SmsNotifier delegate.</summary>
		public long UnreadMessageCount=0;
		///<summary>This gets set externally beforehand.  Lets user quickly select messages for current patient.</summary>
		public long CurPatNum;
		///<summary>The selected patNum.  Can be 0 to include all.</summary>
		private long _patNum=0;

		public FormSmsTextMessaging() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSmsTextMessaging_Load(object sender,EventArgs e) {
			gridMessages.ContextMenu=contextMenuMessages;
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Using clinics
				labelClinic.Visible=true;
				comboClinic.Visible=true;
				comboClinic.Items.Clear();
				_listClinics=Clinics.GetForUserod(Security.CurUser);
				for(int i=0;i<_listClinics.Count;i++) {
					comboClinic.Items.Add(_listClinics[i].Description);
					comboClinic.SetSelected(i,true);
				}
			}
			textDateFrom.Text=DateTimeOD.Today.AddDays(-7).ToShortDateString();
			textDateTo.Text=DateTimeOD.Today.ToShortDateString();
			if(IsSent) {
				checkSent.Checked=true;
			}
			else {
				checkRead.Checked=true;
			}
			FillGridTextMessages();
		}

		private void FillGridTextMessages() {
			int sortByColIdx=gridMessages.SortedByColumnIdx;
			bool isSortAsc=gridMessages.SortedIsAscending;
			if(sortByColIdx==-1) {
				sortByColIdx=0;
				isSortAsc=false;
			}
			object selectedTag=null;
			if(gridMessages.GetSelectedIndex()!=-1) {
				selectedTag=gridMessages.Rows[gridMessages.GetSelectedIndex()].Tag;
			}
			gridMessages.BeginUpdate();
			gridMessages.Rows.Clear();
			gridMessages.Columns.Clear();
			gridMessages.Columns.Add(new UI.ODGridColumn("DateTime",140,HorizontalAlignment.Left));
			gridMessages.Columns[gridMessages.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.DateParse;
			gridMessages.Columns.Add(new UI.ODGridColumn("In or Out",80,HorizontalAlignment.Center));
			gridMessages.Columns[gridMessages.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.StringCompare;
			gridMessages.Columns.Add(new UI.ODGridColumn("Status",90,HorizontalAlignment.Center));
			gridMessages.Columns[gridMessages.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.StringCompare;
			if(checkHidden.Checked) {
				gridMessages.Columns.Add(new UI.ODGridColumn("Hidden",44,HorizontalAlignment.Center));
				gridMessages.Columns[gridMessages.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.StringCompare;
			}
			gridMessages.Columns.Add(new UI.ODGridColumn("Cost",32,HorizontalAlignment.Right));
			gridMessages.Columns[gridMessages.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.AmountParse;
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Using clinics
				gridMessages.Columns.Add(new UI.ODGridColumn("Clinic",130,HorizontalAlignment.Left));
				gridMessages.Columns[gridMessages.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.StringCompare;
			}
			gridMessages.Columns.Add(new UI.ODGridColumn("Patient",150,HorizontalAlignment.Left));
			gridMessages.Columns[gridMessages.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.StringCompare;
			List<long> listClinicNums=new List<long>();//Leaving this blank will cause the clinic filter to be ignored in SmsFromMobiles.GetMessages().
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Using clinics
				for(int i=0;i<comboClinic.SelectedIndices.Count;i++) {
					int index=(int)comboClinic.SelectedIndices[i];
					listClinicNums.Add(_listClinics[index].ClinicNum);
				}
			}
			List <SmsFromStatus> listSmsFromStatuses=new List<SmsFromStatus>();
			listSmsFromStatuses.Add(SmsFromStatus.ReceivedUnread);//Unread messages always show, unless we can later think of a reason why not.
			if(checkRead.Checked) {
				listSmsFromStatuses.Add(SmsFromStatus.ReceivedRead);
			}
			DateTime dateFrom=PIn.Date(textDateFrom.Text);
			DateTime dateTo=PIn.Date(textDateTo.Text);
			Dictionary<long,string> dictPatNames=Patients.GetAllPatientNames();
			if(listSmsFromStatuses.Count>0) {
				List<SmsFromMobile> listSmsFromMobile=SmsFromMobiles.GetMessages(dateFrom,dateTo,listClinicNums,_patNum,listSmsFromStatuses.ToArray());				
				for(int i=0;i<listSmsFromMobile.Count;i++) {
					if(!checkHidden.Checked && listSmsFromMobile[i].IsHidden) {
						continue;
					}
					UI.ODGridRow row=new UI.ODGridRow();
					row.Tag=listSmsFromMobile[i];					
					if(listSmsFromMobile[i].SmsStatus==SmsFromStatus.ReceivedUnread) {
						row.Bold=true;
					}
					row.Cells.Add(listSmsFromMobile[i].DateTimeReceived.ToString());//DateTime
					row.Cells.Add(Lan.g(this,"Received"));//Type
					row.Cells.Add(SmsFromMobiles.GetSmsFromStatusDescript(listSmsFromMobile[i].SmsStatus));//Status
					if(checkHidden.Checked) {
						row.Cells.Add(listSmsFromMobile[i].IsHidden?"X":"");//Hidden
					}
					row.Cells.Add("0.00");//Cost
					if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Using clinics
						Clinic clinic=Clinics.GetClinic(listSmsFromMobile[i].ClinicNum);
						row.Cells.Add(clinic.Description);//Clinic
					}
					if(listSmsFromMobile[i].PatNum==0) {
						row.Cells.Add("");//Patient
					}
					else {
						row.Cells.Add(dictPatNames[listSmsFromMobile[i].PatNum]);//Patient
					}
					gridMessages.Rows.Add(row);
				}
			}
			UnreadMessageCount=PIn.Long(SmsFromMobiles.GetSmsNotification());
			SmsNotifier(UnreadMessageCount.ToString(),true);
			if(checkSent.Checked) {
				List<SmsToMobile> listSmsToMobile=SmsToMobiles.GetMessages(dateFrom,dateTo,listClinicNums,_patNum);
				for(int i=0;i<listSmsToMobile.Count;i++) {
					if(!checkHidden.Checked && listSmsToMobile[i].IsHidden) {
						continue;
					}
					UI.ODGridRow row=new UI.ODGridRow();
					row.Tag=listSmsToMobile[i];
					row.Cells.Add(listSmsToMobile[i].DateTimeSent.ToString());//DateTime
					row.Cells.Add(Lan.g(this,"Sent"));//Type
					row.Cells.Add(listSmsToMobile[i].Status.ToString());//Status
					if(checkHidden.Checked) {
						row.Cells.Add(listSmsToMobile[i].IsHidden?"X":"");//Hidden
					}
					row.Cells.Add(listSmsToMobile[i].MsgChargeUSD.ToString("f"));//Cost
					if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Using clinics
						Clinic clinic=Clinics.GetClinic(listSmsToMobile[i].ClinicNum);
						row.Cells.Add(clinic.Description);//Clinic
					}
					if(listSmsToMobile[i].PatNum==0) {
						row.Cells.Add("");//Patient
					}
					else {
						row.Cells.Add(dictPatNames[listSmsToMobile[i].PatNum]);//Patient
					}
					gridMessages.Rows.Add(row);
				}
			}
			gridMessages.EndUpdate();
			gridMessages.SortForced(sortByColIdx,isSortAsc);
			bool isSelectionFound=false;
			for(int i=0;i<gridMessages.Rows.Count;i++) {
				if(gridMessages.Rows[i].Tag is SmsFromMobile && selectedTag is SmsFromMobile
					&& ((SmsFromMobile)gridMessages.Rows[i].Tag).SmsFromMobileNum==((SmsFromMobile)selectedTag).SmsFromMobileNum) {
					gridMessages.SetSelected(i,true);
					FillGridMessageThread(((SmsFromMobile)selectedTag).PatNum);
					isSelectionFound=true;
					break;
				}
				if(gridMessages.Rows[i].Tag is SmsToMobile && selectedTag is SmsToMobile
					&& ((SmsToMobile)gridMessages.Rows[i].Tag).SmsToMobileNum==((SmsToMobile)selectedTag).SmsToMobileNum) {
					gridMessages.SetSelected(i,true);
					FillGridMessageThread(((SmsToMobile)selectedTag).PatNum);
					isSelectionFound=true;
					break;
				}
			}
			if(!isSelectionFound) {
				FillGridMessageThread(0);
			}
		}

		private void FillGridMessageThread(long patNum) {
			if(patNum==0) {
				smsThreadView.ListSmsThreadMessages=null;
				return;
			}
			List<long> listClinicNums=new List<long>();//Leaving this blank will cause the clinic filter to be ignored in SmsFromMobiles.GetMessages().
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Using clinics
				for(int i=0;i<comboClinic.SelectedIndices.Count;i++) {
					int index=(int)comboClinic.SelectedIndices[i];
					listClinicNums.Add(_listClinics[index].ClinicNum);
				}
			}
			object selectedTag=null;
			if(gridMessages.GetSelectedIndex()!=-1) {
				selectedTag=gridMessages.Rows[gridMessages.GetSelectedIndex()].Tag;
			}
			List<SmsThreadMessage> listSmsThreadMessages=new List<SmsThreadMessage>();
			List<SmsFromMobile> listSmsFromMobile=SmsFromMobiles.GetMessages(DateTime.MinValue,DateTime.MinValue,listClinicNums,patNum);
			for(int i=0;i<listSmsFromMobile.Count;i++) {
				bool isHighlighted=false;
				if(selectedTag is SmsFromMobile	&& ((SmsFromMobile)selectedTag).SmsFromMobileNum==listSmsFromMobile[i].SmsFromMobileNum) {
					isHighlighted=true;
				}
				listSmsThreadMessages.Add(new SmsThreadMessage(listSmsFromMobile[i].DateTimeReceived,listSmsFromMobile[i].MsgText,true,false,isHighlighted));
			}
			List<SmsToMobile> listSmsToMobile=SmsToMobiles.GetMessages(DateTime.MinValue,DateTime.MinValue,listClinicNums,patNum);
			for(int i=0;i<listSmsToMobile.Count;i++) {
				bool isHighlighted=false;
				if(selectedTag is SmsToMobile	&& ((SmsToMobile)selectedTag).SmsToMobileNum==listSmsToMobile[i].SmsToMobileNum) {
					isHighlighted=true;
				}
				listSmsThreadMessages.Add(new SmsThreadMessage(listSmsToMobile[i].DateTimeSent,
					listSmsToMobile[i].MsgText,false,listSmsToMobile[i].IsTimeSensitive,isHighlighted));
			}
			listSmsThreadMessages.Sort(SmsThreadMessage.CompareMessages);
			smsThreadView.ListSmsThreadMessages=listSmsThreadMessages;
		}

		private void butPatCurrent_Click(object sender,EventArgs e) {
			_patNum=CurPatNum;
			if(_patNum==0) {
				textPatient.Text="";
			}
			else {
				textPatient.Text=Patients.GetLim(_patNum).GetNameLF();
			}
		}

		private void butPatFind_Click(object sender,EventArgs e) {
			FormPatientSelect FormP=new FormPatientSelect();
			FormP.ShowDialog();
			if(FormP.DialogResult!=DialogResult.OK) {
				return;
			}
			_patNum=FormP.SelectedPatNum;
			textPatient.Text=Patients.GetLim(_patNum).GetNameLF();
		}

		private void butPatAll_Click(object sender,EventArgs e) {
			_patNum=0;
			textPatient.Text="";
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			FillGridTextMessages();
			Cursor=Cursors.Default;
		}

		private void gridMessages_CellClick(object sender,UI.ODGridClickEventArgs e) {
			if(gridMessages.Rows[e.Row].Tag is SmsFromMobile) {
				SmsFromMobile smsFromMobile=(SmsFromMobile)gridMessages.Rows[e.Row].Tag;
				if(smsFromMobile.SmsStatus==SmsFromStatus.ReceivedUnread) {
					SmsFromMobile oldSmsFromMobile=smsFromMobile.Copy();
					smsFromMobile.SmsStatus=SmsFromStatus.ReceivedRead;
					SmsFromMobiles.Update(smsFromMobile,oldSmsFromMobile);
				}
			}
			FillGridTextMessages();
		}

		///<summary>Unselects all sent messages.  Returns true if there is at least one received message selected.</summary>
		private bool UnselectSentMessages() {
			int unselectedCount=0;
			for(int i=0;i<gridMessages.SelectedIndices.Length;i++) {
				int index=gridMessages.SelectedIndices[i];
				if(gridMessages.Rows[index].Tag.GetType()==typeof(SmsToMobile)) {//Sent message
					gridMessages.SetSelected(index,false);
					unselectedCount++;
				}
			}
			string msg="";
			if(unselectedCount>0) {
				msg=Lan.g(this,"Number of sent messages unselected")+": "+unselectedCount;
			}
			bool retVal=true;
			if(gridMessages.SelectedIndices.Length==0) {
				if(msg!="") {
					msg+="\r\n";
				}
				msg+=Lan.g(this,"Please select at least one received message.");
				retVal=false;
			}
			if(msg!="") {
				MessageBox.Show(msg);
			}
			return retVal;
		}

		///<summary>Unselects any sent messages, then sets the given status for any selected receieved messages.</summary>
		private void SetReceivedSelectedStatus(SmsFromStatus smsFromStatus) {
			if(!UnselectSentMessages()) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			for(int i=0;i<gridMessages.SelectedIndices.Length;i++) {
				int index=gridMessages.SelectedIndices[i];
				SmsFromMobile smsFromMobile=(SmsFromMobile)gridMessages.Rows[index].Tag;
				SmsFromMobile oldSmsFromMobile=smsFromMobile.Copy();
				smsFromMobile.SmsStatus=smsFromStatus;
				SmsFromMobiles.Update(smsFromMobile,oldSmsFromMobile);
			}
			FillGridTextMessages();//Refresh grid to show changed status.
			Cursor=Cursors.Default;
		}

		private void ChangePat() {
			if(!UnselectSentMessages()) {
				return;
			}
			FormPatientSelect form=new FormPatientSelect();
			if(form.ShowDialog()!=DialogResult.OK) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			for(int i=0;i<gridMessages.SelectedIndices.Length;i++) {
				int index=gridMessages.SelectedIndices[i];
				SmsFromMobile smsFromMobile=(SmsFromMobile)gridMessages.Rows[index].Tag;
				SmsFromMobile oldSmsFromMobile=smsFromMobile.Copy();
				smsFromMobile.PatNum=form.SelectedPatNum;
				SmsFromMobiles.Update(smsFromMobile,oldSmsFromMobile);
				if(smsFromMobile.CommlogNum!=0) {//Not sure if CommlogNum can be zero.
					Commlog commlog=Commlogs.GetOne(smsFromMobile.CommlogNum);
					Commlog oldCommlog=commlog.Copy();
					commlog.PatNum=form.SelectedPatNum;
					Commlogs.Update(commlog,oldCommlog);
				}
			}
			int messagesMovedCount=gridMessages.SelectedIndices.Length;
			FillGridTextMessages();//Refresh grid to show changed patient.
			Cursor=Cursors.Default;
			MessageBox.Show(Lan.g(this,"Text messages moved successfully")+": "+messagesMovedCount);
		}

		private void MarkUnread() {
			SetReceivedSelectedStatus(SmsFromStatus.ReceivedUnread);
		}

		private void MarkRead() {
			SetReceivedSelectedStatus(SmsFromStatus.ReceivedRead);
		}

		///<summary>Set isHide to true to hide the selected messages.  Set IsHide to false to unhide the selected messages.</summary>
		private void HideOrUnhideMessages(bool isHide) {
			Cursor=Cursors.WaitCursor;
			for(int i=0;i<gridMessages.SelectedIndices.Length;i++) {
				int index=gridMessages.SelectedIndices[i];
				if(gridMessages.Rows[index].Tag.GetType()==typeof(SmsFromMobile)) {
					SmsFromMobile smsFromMobile=(SmsFromMobile)gridMessages.Rows[index].Tag;
					SmsFromMobile oldSmsFromMobile=smsFromMobile.Copy();
					smsFromMobile.IsHidden=isHide;
					SmsFromMobiles.Update(smsFromMobile,oldSmsFromMobile);
				}
				else if(gridMessages.Rows[index].Tag.GetType()==typeof(SmsToMobile)) {
					SmsToMobile smsToMobile=(SmsToMobile)gridMessages.Rows[index].Tag;
					SmsToMobile oldSmsToMobile=(SmsToMobile)smsToMobile.Copy();
					smsToMobile.IsHidden=isHide;
					SmsToMobiles.Update(smsToMobile,oldSmsToMobile);
				}
			}
			FillGridTextMessages();
			Cursor=Cursors.Default;
		}

		private void MarkHidden() {
			if(gridMessages.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select at least one message before attempting to hide.");
				return;
			}
			if(!MsgBox.Show(this,true,"Hide all selected messages?")) {
				return;
			}
			HideOrUnhideMessages(true);
		}

		private void MarkUnhidden() {
			if(gridMessages.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select at least one message before attempting to unhide.");
				return;
			}
			if(!MsgBox.Show(this,true,"Unhide all selected messages?")) {
				return;
			}
			HideOrUnhideMessages(false);
		}

		private void menuItemChangePat_Click(object sender,EventArgs e) {
			ChangePat();
		}

		private void menuItemMarkUnread_Click(object sender,EventArgs e) {
			MarkUnread();
		}

		private void menuItemMarkRead_Click(object sender,EventArgs e) {
			MarkRead();
		}

		private void menuItemHide_Click(object sender,EventArgs e) {
			MarkHidden();
		}

		private void menuItemUnhide_Click(object sender,EventArgs e) {
			MarkUnhidden();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}