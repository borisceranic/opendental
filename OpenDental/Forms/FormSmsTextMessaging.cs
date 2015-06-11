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
		public delegate void SmsNotificationDelegate(string smsNotificationText);
		///<summary>Set from FormOpenDental.  This can be null if the calling code does not wish to get dynamic unread message counts.</summary>
		public SmsNotificationDelegate SmsNotifier=null;
		///<summary>Set in FormOpenDental.  Initialize to the current number of unread text messages.
		///Used by FormSmsTextMessaging to determine when to call the SmsNotifier delegate.</summary>
		public long UnreadMessageCount=0;

		public FormSmsTextMessaging() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSmsTextMessaging_Load(object sender,EventArgs e) {
			_listClinics=Clinics.GetForUserod(Security.CurUser);
			comboClinic.Items.Clear();
			for(int i=0;i<_listClinics.Count;i++) {
				comboClinic.Items.Add(_listClinics[i].Description);
				comboClinic.SetSelected(i,true);
			}
			textDateFrom.Text=DateTimeOD.Today.AddDays(-7).ToShortDateString();
			textDateTo.Text=DateTimeOD.Today.ToShortDateString();
			listStatus.Items.Clear();
			listStatus.Items.Add("Sent");//0
			if(IsSent) {
				listStatus.SetSelected(listStatus.Items.Count-1,true);
			}
			listStatus.Items.Add("Received Unread");//1
			if(!IsSent) {
				listStatus.SetSelected(listStatus.Items.Count-1,true);
			}
			listStatus.Items.Add("Received Read");//2
			if(!IsSent) {
				listStatus.SetSelected(listStatus.Items.Count-1,true);
			}
			listStatus.Items.Add("Received Junk");//3
			FillGridTextMessages();
		}

		private void FillGridTextMessages() {
			int sortByColIdx=gridMessages.SortedByColumnIdx;
			bool isSortAsc=gridMessages.SortedIsAscending;
			if(sortByColIdx==-1) {
				//Default to sorting by Date descending.
				sortByColIdx=1;
				isSortAsc=false;
			}
			gridMessages.BeginUpdate();
			gridMessages.Rows.Clear();
			gridMessages.Columns.Clear();
			gridMessages.Columns.Add(new UI.ODGridColumn("Patient",150,HorizontalAlignment.Left));
			gridMessages.Columns.Add(new UI.ODGridColumn("DateTime",140,HorizontalAlignment.Left));
			gridMessages.Columns.Add(new UI.ODGridColumn("Type",80,HorizontalAlignment.Center));
			gridMessages.Columns.Add(new UI.ODGridColumn("Status",90,HorizontalAlignment.Center));
			if(checkShowHidden.Checked) {
				gridMessages.Columns.Add(new UI.ODGridColumn("Hidden",44,HorizontalAlignment.Center));
			}
			gridMessages.Columns.Add(new UI.ODGridColumn("Cost",32,HorizontalAlignment.Right));
			gridMessages.Columns.Add(new UI.ODGridColumn("Message",0,HorizontalAlignment.Left));
			List<long> listClinicNums=new List<long>();
			for(int i=0;i<_listClinics.Count;i++) {
				listClinicNums.Add(_listClinics[i].ClinicNum);
			}
			bool isSent=false;
			List <SmsFromStatus> listSmsFromStatuses=new List<SmsFromStatus>();
			for(int i=0;i<listStatus.SelectedIndices.Count;i++) {
				int index=listStatus.SelectedIndices[i];
				if(index==0) {
					isSent=true;
				}
				else if(index==1) {
					listSmsFromStatuses.Add(SmsFromStatus.ReceivedUnread);
				}
				else if(index==2) {
					listSmsFromStatuses.Add(SmsFromStatus.ReceivedRead);
				}
				else if(index==3) {
					listSmsFromStatuses.Add(SmsFromStatus.ReceivedJunk);
				}
			}
			DateTime dateFrom=PIn.Date(textDateFrom.Text);
			DateTime dateTo=PIn.Date(textDateTo.Text);
			Dictionary<long,string> dictPatNames=Patients.GetAllPatientNames();
			long unreadMsgCount=0;
			if(listSmsFromStatuses.Count>0) {
				List<SmsFromMobile> listSmsFromMobile=SmsFromMobiles.GetMessages(dateFrom,dateTo,listClinicNums,listSmsFromStatuses.ToArray());				
				for(int i=0;i<listSmsFromMobile.Count;i++) {
					if(!checkShowHidden.Checked && listSmsFromMobile[i].IsHidden) {
						continue;
					}
					UI.ODGridRow row=new UI.ODGridRow();
					row.Tag=listSmsFromMobile[i];
					if(listSmsFromMobile[i].PatNum==0) {
						row.Cells.Add("");//Patient
					}
					else {
						row.Cells.Add(dictPatNames[listSmsFromMobile[i].PatNum]);//Patient
					}
					if(listSmsFromMobile[i].SmsStatus==SmsFromStatus.ReceivedUnread) {
						row.Bold=true;
						unreadMsgCount++;
					}
					row.Cells.Add(listSmsFromMobile[i].DateTimeReceived.ToString());//DateTime
					row.Cells.Add(Lan.g(this,"Received"));//Type
					row.Cells.Add(SmsFromMobiles.GetSmsFromStatusDescript(listSmsFromMobile[i].SmsStatus));//Status
					if(checkShowHidden.Checked) {
						row.Cells.Add(listSmsFromMobile[i].IsHidden?"X":"");//Hidden
					}
					row.Cells.Add("0.00");//Cost
					row.Cells.Add(listSmsFromMobile[i].MsgText);//Message
					gridMessages.Rows.Add(row);
				}
			}
			if(unreadMsgCount>99) {
				unreadMsgCount=99;//The maximum notification count which shows in FormOpenDental is 99.
			}
			if(unreadMsgCount!=UnreadMessageCount) {
				UnreadMessageCount=unreadMsgCount;
				SmsNotifier(unreadMsgCount.ToString());
			}
			if(isSent) {
				List<SmsToMobile> listSmsToMobile=SmsToMobiles.GetMessages(dateFrom,dateTo,listClinicNums);
				for(int i=0;i<listSmsToMobile.Count;i++) {
					if(!checkShowHidden.Checked && listSmsToMobile[i].IsHidden) {
						continue;
					}
					UI.ODGridRow row=new UI.ODGridRow();
					row.Tag=listSmsToMobile[i];
					if(listSmsToMobile[i].PatNum==0) {
						row.Cells.Add("");//Patient
					}
					else {
						row.Cells.Add(dictPatNames[listSmsToMobile[i].PatNum]);//Patient
					}
					row.Cells.Add(listSmsToMobile[i].DateTimeSent.ToString());//DateTime
					row.Cells.Add(Lan.g(this,"Sent"));//Type
					row.Cells.Add(listSmsToMobile[i].Status.ToString());//Status
					if(checkShowHidden.Checked) {
						row.Cells.Add(listSmsToMobile[i].IsHidden?"X":"");//Hidden
					}
					row.Cells.Add(listSmsToMobile[i].MsgCostUSD.ToString("f"));//Cost
					row.Cells.Add(listSmsToMobile[i].MsgText);//Message
					gridMessages.Rows.Add(row);
				}
			}
			gridMessages.EndUpdate();
			gridMessages.SortForced(sortByColIdx,isSortAsc);
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
			if(unselectedCount>0) {
				MessageBox.Show(Lan.g(this,"Number of sent messages unselected")+": "+unselectedCount);
			}
			if(gridMessages.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select at least one received message.");
				return false;
			}
			return true;
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

		private void butJunk_Click(object sender,EventArgs e) {
			SetReceivedSelectedStatus(SmsFromStatus.ReceivedJunk);
		}

		private void butMarkUnread_Click(object sender,EventArgs e) {
			SetReceivedSelectedStatus(SmsFromStatus.ReceivedUnread);
		}

		private void butMarkRead_Click(object sender,EventArgs e) {
			SetReceivedSelectedStatus(SmsFromStatus.ReceivedRead);
		}

		private void butChangePat_Click(object sender,EventArgs e) {
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
				Commlog commlog=Commlogs.GetOne(smsFromMobile.CommlogNum);
				Commlog oldCommlog=commlog.Copy();
				commlog.PatNum=form.SelectedPatNum;
				Commlogs.Update(commlog,oldCommlog);
			}
			int messagesMovedCount=gridMessages.SelectedIndices.Length;
			FillGridTextMessages();//Refresh grid to show changed patient.
			Cursor=Cursors.Default;
			MessageBox.Show(Lan.g(this,"Text messages moved successfully")+": "+messagesMovedCount);
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			FillGridTextMessages();
			Cursor=Cursors.Default;
		}

		private void butHide_Click(object sender,EventArgs e) {
			if(gridMessages.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select at least one message before attempting to hide.");
				return;
			}
			if(!MsgBox.Show(this,true,"Hide all selected messages?")) {
				return;
			}
			HideOrUnhideMessages(true);
		}

		private void butUnhide_Click(object sender,EventArgs e) {
			if(gridMessages.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select at least one message before attempting to unhide.");
				return;
			}
			if(!MsgBox.Show(this,true,"Unhide all selected messages?")) {
				return;
			}
			HideOrUnhideMessages(false);
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

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}