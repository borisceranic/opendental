using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormSmsTextMessaging:Form {

		private List<Clinic> _listClinics=null;
		///<summary>Set before showing this form.  If true, will initially show sent messages.</summary>
		public bool IsSent=false;
		///<summary>Set before showing this form.  If true, will initially show received messages.</summary>
		public bool IsReceived=false;
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
		///<summary>The column index of the Status column within the Messages grid.
		///This is a class-wide variable to prevent bugs if we decide to change the column order of the Messages grid.</summary>
		private int _columnStatusIdx=0;
		public PatientSelectedEventHandler PatientGoTo;
		private List<SmsFromMobile> _listSmsFromMobile=new List<SmsFromMobile>();
		private List<SmsToMobile> _listSmsToMobile=new List<SmsToMobile>();

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
			checkSent.Checked=IsSent;			
			checkRead.Checked=IsReceived;
			FillGridTextMessages();
			Plugins.HookAddCode(this,"FormSmsTextMessaging.Load_end");
		}

		private void FillGridTextMessages(bool isRedrawOnly=false) {
			Color colorSelect=Color.FromArgb(224,243,255);
			int sortByColIdx=gridMessages.SortedByColumnIdx;
			bool isSortAsc=gridMessages.SortedIsAscending;
			if(sortByColIdx==-1) {
				sortByColIdx=0;
				isSortAsc=false;
			}
			object selectedTag=null;
			long selectedPatNum=0;
			if(gridMessages.GetSelectedIndex()!=-1) {
				selectedTag=gridMessages.Rows[gridMessages.GetSelectedIndex()].Tag;
				if(selectedTag is SmsToMobile) {
					selectedPatNum=((SmsToMobile)selectedTag).PatNum;
				}
				else if(selectedTag is SmsFromMobile) {
					selectedPatNum=((SmsFromMobile)selectedTag).PatNum;
				}
			}
			gridMessages.BeginUpdate();
			gridMessages.Rows.Clear();
			gridMessages.Columns.Clear();
			gridMessages.Columns.Add(new UI.ODGridColumn("DateTime",140,HorizontalAlignment.Left) { SortingStrategy=UI.GridSortingStrategy.DateParse });
			gridMessages.Columns.Add(new UI.ODGridColumn("Sent /\r\nReceived",80,HorizontalAlignment.Center){SortingStrategy=UI.GridSortingStrategy.StringCompare} );
			gridMessages.Columns.Add(new UI.ODGridColumn("Status",70,HorizontalAlignment.Center) { SortingStrategy=UI.GridSortingStrategy.StringCompare });
			_columnStatusIdx=gridMessages.Columns.Count-1;
			gridMessages.Columns.Add(new UI.ODGridColumn("#Phone\r\nMatches",60,HorizontalAlignment.Center) { SortingStrategy=UI.GridSortingStrategy.AmountParse });
			gridMessages.Columns.Add(new UI.ODGridColumn("Patient\r\nPhone",100,HorizontalAlignment.Center) { SortingStrategy=UI.GridSortingStrategy.StringCompare });
			gridMessages.Columns.Add(new UI.ODGridColumn("Patient",150,HorizontalAlignment.Left) { SortingStrategy=UI.GridSortingStrategy.StringCompare });
			gridMessages.Columns.Add(new UI.ODGridColumn("Cost",32,HorizontalAlignment.Right) { SortingStrategy=UI.GridSortingStrategy.AmountParse });
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Using clinics
				gridMessages.Columns.Add(new UI.ODGridColumn("Clinic",130,HorizontalAlignment.Left) { SortingStrategy=UI.GridSortingStrategy.StringCompare });
			}
			if(checkHidden.Checked) {
				gridMessages.Columns.Add(new UI.ODGridColumn("Hidden",46,HorizontalAlignment.Center){SortingStrategy=UI.GridSortingStrategy.StringCompare});
			}
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
				if(!isRedrawOnly) {
					_listSmsFromMobile=SmsFromMobiles.GetMessages(dateFrom,dateTo,listClinicNums,_patNum,false,listSmsFromStatuses.ToArray());
				}
				for(int i=0;i<_listSmsFromMobile.Count;i++) {
					if(!checkHidden.Checked && _listSmsFromMobile[i].IsHidden) {
						continue;
					}
					UI.ODGridRow row=new UI.ODGridRow();
					row.Tag=_listSmsFromMobile[i];					
					if(_listSmsFromMobile[i].SmsStatus==SmsFromStatus.ReceivedUnread) {
						row.Bold=true;
					}
					row.Cells.Add(_listSmsFromMobile[i].DateTimeReceived.ToString());//DateTime
					row.Cells.Add(Lan.g(this,"Received"));//Type
					row.Cells.Add(SmsFromMobiles.GetSmsFromStatusDescript(_listSmsFromMobile[i].SmsStatus));//Status
					row.Cells.Add(_listSmsFromMobile[i].MatchCount.ToString());//#Phone Matches
					row.Cells.Add(_listSmsFromMobile[i].MobilePhoneNumber);//Patient Phone
					row.Cells.Add(_listSmsFromMobile[i].PatNum==0?"Unassigned":dictPatNames[_listSmsFromMobile[i].PatNum]);//Patient
					row.Cells.Add("0.00");//Cost
					if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Using clinics
						if(_listSmsFromMobile[i].ClinicNum==0) {
							row.Cells.Add(PrefC.GetString(PrefName.PracticeTitle)+"("+Lan.g(this,"Practice")+")");
						}
						else { 
							Clinic clinic=Clinics.GetClinic(_listSmsFromMobile[i].ClinicNum);
							row.Cells.Add(clinic.Description);//Clinic
						}
					}
					if(checkHidden.Checked) {
						row.Cells.Add(_listSmsFromMobile[i].IsHidden?"X":"");//Hidden
					}
					if(selectedPatNum!=0 && _listSmsFromMobile[i].PatNum==selectedPatNum) {
						row.ColorBackG=colorSelect;
					}
					gridMessages.Rows.Add(row);
				}
			}
			UnreadMessageCount=PIn.Long(SmsFromMobiles.GetSmsNotification());
			SmsNotifier(UnreadMessageCount.ToString(),true);
			if(checkSent.Checked) {
				if(!isRedrawOnly) {
					_listSmsToMobile=SmsToMobiles.GetMessages(dateFrom,dateTo,listClinicNums,_patNum);
				}
				for(int i=0;i<_listSmsToMobile.Count;i++) {
					if(!checkHidden.Checked && _listSmsToMobile[i].IsHidden) {
						continue;
					}
					UI.ODGridRow row=new UI.ODGridRow();
					row.Tag=_listSmsToMobile[i];
					row.Cells.Add(_listSmsToMobile[i].DateTimeSent.ToString());//DateTime
					row.Cells.Add(Lan.g(this,"Sent"));//Type
					string smsStatus=_listSmsToMobile[i].SmsStatus.ToString(); //Default to the actual status.
					switch(_listSmsToMobile[i].SmsStatus) {
						case SmsDeliveryStatus.DeliveryConf:
						case SmsDeliveryStatus.DeliveryUnconf:
							//Treated the same as far as the user is concerned.
							smsStatus="Delivered";
							break;
						case SmsDeliveryStatus.FailWithCharge:
						case SmsDeliveryStatus.FailNoCharge:
							//Treated the same as far as the user is concerned.
							smsStatus="Failed";							
							break;
					}
					row.Cells.Add(smsStatus);//Status
					row.Cells.Add("-");//#Phone Matches, not applicable to outbound messages.
					row.Cells.Add(_listSmsToMobile[i].MobilePhoneNumber);//Patient Phone
					row.Cells.Add(_listSmsToMobile[i].PatNum==0?"":dictPatNames[_listSmsToMobile[i].PatNum]);//Patient
					row.Cells.Add(_listSmsToMobile[i].MsgChargeUSD.ToString("f"));//Cost
					if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Using clinics
						if(_listSmsToMobile[i].ClinicNum==0) {
							row.Cells.Add(PrefC.GetString(PrefName.PracticeTitle)+"("+Lan.g(this,"Practice")+")");
						}
						else { 
							Clinic clinic=Clinics.GetClinic(_listSmsToMobile[i].ClinicNum);
							row.Cells.Add(clinic.Description);//Clinic
						}
					}
					if(checkHidden.Checked) {
						row.Cells.Add(_listSmsToMobile[i].IsHidden?"X":"");//Hidden
					}
					if(selectedPatNum!=0 && _listSmsToMobile[i].PatNum==selectedPatNum) {
						row.ColorBackG=colorSelect;
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
				if(gridMessages.SelectedIndices.Length>0) {
					if((object)gridMessages.Rows[gridMessages.SelectedIndices[0]].Tag is SmsToMobile) {
						List<SmsThreadMessage> listSmsThreadMessage=new List<SmsThreadMessage>();
						SmsToMobile smsToMobile=(SmsToMobile)gridMessages.Rows[gridMessages.SelectedIndices[0]].Tag;
						listSmsThreadMessage.Add(new SmsThreadMessage(smsToMobile.DateTimeSent,smsToMobile.MsgText,true,true,true));
						smsThreadView.ListSmsThreadMessages=listSmsThreadMessage;
					}
					else {  //Tag is SmsFromMobile
						List<SmsThreadMessage> listSmsThreadMessage=new List<SmsThreadMessage>();
						SmsFromMobile smsFromMobile=(SmsFromMobile)gridMessages.Rows[gridMessages.SelectedIndices[0]].Tag;
						listSmsThreadMessage.Add(new SmsThreadMessage(smsFromMobile.DateTimeReceived,smsFromMobile.MsgText,true,true,true));
						smsThreadView.ListSmsThreadMessages=listSmsThreadMessage;
					}
				}
				else {
					smsThreadView.ListSmsThreadMessages=null;
				}
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
			List<SmsFromMobile> listSmsFromMobile=SmsFromMobiles.GetMessages(DateTime.MinValue,DateTime.MinValue,listClinicNums,patNum,true);
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
				bool isImportant=false;
				if(listSmsToMobile[i].SmsStatus==SmsDeliveryStatus.FailNoCharge || listSmsToMobile[i].SmsStatus==SmsDeliveryStatus.FailWithCharge) {
					isImportant=true;
				}
				listSmsThreadMessages.Add(new SmsThreadMessage(listSmsToMobile[i].DateTimeSent,					
					listSmsToMobile[i].MsgText,false,isImportant,isHighlighted));
			}
			listSmsThreadMessages.Sort(SmsThreadMessage.CompareMessages);
			smsThreadView.ListSmsThreadMessages=listSmsThreadMessages;
			SetReplyHelper();
		}

		private void SetReplyHelper() {
			if(gridMessages.GetSelectedIndex()==-1) {
				butSend.Enabled=false;
				textReply.Enabled=false;
				textReply.Text="";
				return;
			}
			butSend.Enabled=true;
			textReply.Enabled=true;
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
				//Update the message thread control to show the message thread for the currently selected message.
				SmsFromMobile smsFromMobile=(SmsFromMobile)gridMessages.Rows[e.Row].Tag;
				//If the clicked/selected message was a ReceivedUnread message, then mark the message ReceivedRead in the db as well as the grid.
				if(smsFromMobile.SmsStatus==SmsFromStatus.ReceivedUnread) {
					SmsFromMobile oldSmsFromMobile=smsFromMobile.Copy();
					smsFromMobile.SmsStatus=SmsFromStatus.ReceivedRead;
					SmsFromMobiles.Update(smsFromMobile,oldSmsFromMobile);
					gridMessages.Rows[e.Row].Cells[_columnStatusIdx].Text=SmsFromMobiles.GetSmsFromStatusDescript(SmsFromStatus.ReceivedRead);
					gridMessages.Rows[e.Row].Bold=false;
					gridMessages.Invalidate();//To show the status changes in the grid.
					UnreadMessageCount=PIn.Long(SmsFromMobiles.GetSmsNotification());
					SmsNotifier(UnreadMessageCount.ToString(),true);
				}
			}
			if(gridMessages.Rows[e.Row].Tag is SmsToMobile) {
				//Update the message thread control to show the message thread for the currently selected message.
				SmsToMobile smsToMobile=(SmsToMobile)gridMessages.Rows[e.Row].Tag;
			}
			FillGridTextMessages(true);
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
				gridMessages.Rows[index].Cells[_columnStatusIdx].Text=SmsFromMobiles.GetSmsFromStatusDescript(smsFromStatus);
				gridMessages.Rows[index].Bold=false;
				if(smsFromStatus==SmsFromStatus.ReceivedUnread) {
					gridMessages.Rows[index].Bold=true;
				}
			}
			gridMessages.Invalidate();//To show the status changes in the grid.
			UnreadMessageCount=PIn.Long(SmsFromMobiles.GetSmsNotification());
			SmsNotifier(UnreadMessageCount.ToString(),true);
			Cursor=Cursors.Default;
		}

		private void ChangePat() {
			if(!UnselectSentMessages()) {
				return;
			}
			FormPatientSelect form=new FormPatientSelect();
			form.ExplicitPatNums=SmsFromMobiles.FindPatNums(
				gridMessages.SelectedIndices.Select(x => (SmsFromMobile)gridMessages.Rows[x].Tag).ToList()[0].MobilePhoneNumber,//find mobile number of first sms
				CultureInfo.CurrentCulture.Name.Substring(CultureInfo.CurrentCulture.Name.Length-2) //get country code of current environment
				).Select(x=>x[0]).ToList();//convert to a list of patnums.
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
				if(smsFromMobile.CommlogNum==0) {
					//When a new sms comes in on the server, a corresponding commlog is inserted, unless the sms could not be matched to a patient by phone #.
					//We need to insert a new commlog when the patient is selected, for the case when the message has not been asssigned to a patient yet.
					//This way we can ensure that all sms with a patient attached have a corresponding commlog.
					Commlog commlog=new Commlog();
					commlog.CommDateTime=smsFromMobile.DateTimeReceived;
					commlog.CommType=Commlogs.GetTypeAuto(CommItemTypeAuto.MISC);
					commlog.Mode_=CommItemMode.Text;
					commlog.Note=smsFromMobile.MsgText;
					commlog.PatNum=smsFromMobile.PatNum;
					commlog.SentOrReceived=CommSentOrReceived.Received;
					Commlogs.Insert(commlog);
				}
				else {
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

		private void menuItemSelectPatient_Click(object sender,EventArgs e) {
			if(gridMessages.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select a message first.");
				return;
			}
			object selectedTag=gridMessages.Rows[gridMessages.GetSelectedIndex()].Tag;
			long patNum=0;
			if(selectedTag is SmsFromMobile) {
				patNum=((SmsFromMobile)selectedTag).PatNum;//can be 0
			}
			else if(selectedTag is SmsToMobile) {
				patNum=((SmsToMobile)selectedTag).PatNum;//can be 0
			}
			if(patNum==0) {
				MsgBox.Show(this,"Please select a message with a valid patient attached.");
				return;
			}
			PatientSelectedEventArgs psea=new PatientSelectedEventArgs(Patients.GetPat(patNum));
			PatientGoTo(this,psea);
		}

		private void butReply_Click(object sender,EventArgs e) {
			if(gridMessages.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select a message thread to reply to.");
				return;
			}
			if(textReply.Text=="") {
				MsgBox.Show(this,"Please input reply text first.");
				return;
			}
			object selectedTag=gridMessages.Rows[gridMessages.GetSelectedIndex()].Tag;
			string mobileNumber="";
			long patNum=0;
			long clinicNum=0;
			if(selectedTag is SmsFromMobile) {
				mobileNumber=((SmsFromMobile)selectedTag).MobilePhoneNumber;
				patNum=((SmsFromMobile)selectedTag).PatNum;//can be 0
				clinicNum=((SmsFromMobile)selectedTag).ClinicNum;//can be 0
			}
			else if(selectedTag is SmsToMobile) {
				mobileNumber=((SmsToMobile)selectedTag).MobilePhoneNumber;
				patNum=((SmsToMobile)selectedTag).PatNum;//can be 0
				clinicNum=((SmsToMobile)selectedTag).ClinicNum;//can be 0
			}
			else {
				//should never happen.
			}
			if(mobileNumber=="") {
				//should never happen.
				MsgBox.Show(this,"Selected message does not have a valid phone number to send to.");
				return;
			}
			try {
        if(Plugins.HookMethod(this,"FormSmsTextMessaging.butReply_Click_sendSmsSingle",patNum,mobileNumber,textReply.Text,YN.Yes))
        {
          goto HookSkipSmsCall;
        }
				SmsToMobiles.SendSmsSingle(patNum,mobileNumber,textReply.Text,clinicNum,SmsMessageSource.DirectSms);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return;
			}
      HookSkipSmsCall: { }
			textReply.Text="";
			FillGridMessageThread(patNum);
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

	}
}