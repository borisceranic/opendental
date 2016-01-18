using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using System.Linq;

namespace OpenDental {
	public partial class FormEmailInbox:Form {
		///<summary>Do not access directly.  Instead use AddressInbox.</summary>
		private EmailAddress _addressInbox=null;
		private List<EmailMessage> ListEmailMessages;
		private List<EmailAddress> _listEmailAddresses;

		private EmailAddress AddressInbox {
			get {
				if(comboEmailAddress.SelectedIndex==0 && Security.CurUser.ProvNum!=0) {//Webmail only shows for providers.
					EmailAddress webEmailAddress=new EmailAddress();
					webEmailAddress.EmailUsername="WebMail";
					_addressInbox=webEmailAddress;
				}
				else if(Security.CurUser.ProvNum!=0) {//When the user is a provider, the Webmail option is available, so subtract 1 from index.
					_addressInbox=_listEmailAddresses[comboEmailAddress.SelectedIndex-1];
				}
				else {
					_addressInbox=_listEmailAddresses[comboEmailAddress.SelectedIndex];
				}
				return _addressInbox;
			}
		}

		public FormEmailInbox() {
			InitializeComponent();
			Lan.F(this);			
		}

		private void FormEmailInbox_Resize(object sender,EventArgs e) {
			if(_listEmailAddresses!=null) {//Since Resize() is called once before Load(), we must ensure FillComboEmail() has been called at least once.
				FillGridEmailMessages();//To resize the columnns in the grid as needed.
			}
		}

		private void FormEmailInbox_Load(object sender,EventArgs e) {
			FillComboEmail();
			if(_listEmailAddresses.Count==0) {
				MsgBox.Show(this,"No email addresses available for current user.  Edit email address info in the File menu or Setup menu.");
				DialogResult=DialogResult.Cancel;
				return;
			}
			textComputerNameReceive.Text=PrefC.GetString(PrefName.EmailInboxComputerName);
			textComputerName.Text=Dns.GetHostName();
			Application.DoEvents();//Show the form contents before loading email into the grid.
			GetMessages();//If no new messages, then the user will know based on what shows in the grid.
		}

		private void FillComboEmail() {
			_listEmailAddresses=EmailAddresses.GetListt();//Does not include user specific email addresses.
			//Exclude any email addresses which are associated to clinics.
			if(PrefC.HasClinicsEnabled) {
				Clinic[] listClinicsAll=Clinics.GetList();
				for(int i=0;i<listClinicsAll.Length;i++) {
					_listEmailAddresses.RemoveAll(x => x.EmailAddressNum==listClinicsAll[i].EmailAddressNum);
				}
			}
			//Exclude default practice email address, since it is added on another line below.
			_listEmailAddresses.RemoveAll(x => x.EmailAddressNum==PrefC.GetLong(PrefName.EmailDefaultAddressNum));
			//Exclude web mail notification email address.
			_listEmailAddresses.RemoveAll(x => x.EmailAddressNum==PrefC.GetLong(PrefName.EmailNotifyAddressNum));
			//Add clinic defaults that the user has access to.  Do not add duplicates.
			if(PrefC.HasClinicsEnabled) {
				List<Clinic> listClinicForUser=Clinics.GetForUserod(Security.CurUser);
				for(int i=0;i < listClinicForUser.Count;i++) {
					EmailAddress emailClinic=EmailAddresses.GetByClinic(listClinicForUser[i].ClinicNum);
					if(_listEmailAddresses.Any(x => x.EmailAddressNum == emailClinic.EmailAddressNum)) {
						continue;
					}
					_listEmailAddresses.Insert(0,emailClinic);
				}
			}
			//Add addresses which are: not associated to anything, or not default, or unique per clinic.
			for(int i=0;i<_listEmailAddresses.Count();i++) {
				comboEmailAddress.Items.Add(_listEmailAddresses[i].EmailUsername);
			}
			//Add the practice default.
			EmailAddress emailAddressPractice=EmailAddresses.GetOne(PrefC.GetLong(PrefName.EmailDefaultAddressNum));
			if(emailAddressPractice!=null) {
				comboEmailAddress.Items.Insert(0,"Default <"+emailAddressPractice.EmailUsername+">");
				_listEmailAddresses.Insert(0,emailAddressPractice);//Practice Default
			}
			//Add the personal email address for the current user.
			EmailAddress emailAddressMe=EmailAddresses.GetForUser(Security.CurUser.UserNum);
			if(emailAddressMe!=null) {
				comboEmailAddress.Items.Insert(0,"Me <"+emailAddressMe.EmailUsername+">");
				_listEmailAddresses.Insert(0,emailAddressMe);//"Me"
			}
			if(comboEmailAddress.Items.Count > 0) {
				comboEmailAddress.SelectedIndex=0;//This could be the default practice address, or personal address, or another address.
			}
			if(Security.CurUser.ProvNum!=0) { //If the first item in the combobox is selected, make checks to see if the current user has a provnum.
				comboEmailAddress.Items.Insert(0,"WebMail");//Only providers have access to see Webmail messages.
			}
		}

		private void menuItemSetup_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormEmailAddresses formEA=new FormEmailAddresses();
			formEA.ShowDialog();
			textComputerNameReceive.Text=PrefC.GetString(PrefName.EmailInboxComputerName);
			GetMessages();//Get new messages, just in case the user entered email information for the first time.
		}

		///<summary>Gets new messages from email inbox, as well as older messages from the db. Also fills the grid.</summary>
		private int GetMessages() {
			Cursor=Cursors.WaitCursor;
			FillGridEmailMessages();//Show what is in db.
			Cursor=Cursors.Default;
			if(comboEmailAddress.SelectedIndex==0 && Security.CurUser.ProvNum!=0) { //WebMail is selected
				Text="Email Inbox for "+AddressInbox.EmailUsername;
				return 0;//Webmail messages are in the database, so we will not need to receive email from the email server.
			}
			if(AddressInbox.EmailUsername=="" || AddressInbox.Pop3ServerIncoming=="") {//Email address not setup.
				Text="Email Inbox - The currently selected email address is not setup to receive email.";
				return 0;
			}
			Text="Email Inbox for "+AddressInbox.EmailUsername;
			Application.DoEvents();//So that something is showing while the page is loading.
			if(!CodeBase.ODEnvironment.IdIsThisComputer(PrefC.GetString(PrefName.EmailInboxComputerName))) {//This is not the computer to get new messages from.
				return 0;
			}
			if(PrefC.GetString(PrefName.EmailInboxComputerName)=="") {
				MsgBox.Show(this,"Computer name to receive new email from has not been setup.");
				return 0;
			}
			Cursor=Cursors.WaitCursor;
			int emailMessagesTotalCount=0;
			Text="Email Inbox for "+AddressInbox.EmailUsername+" - Receiving new email...";
			try {
				bool hasMoreEmail=true;
				while(hasMoreEmail) {
					List<EmailMessage> emailMessages=EmailMessages.ReceiveFromInbox(1,AddressInbox);
					emailMessagesTotalCount+=emailMessages.Count;
					if(emailMessages.Count==0) {
						hasMoreEmail=false;
					}
					else { //Show messages as they are downloaded, to indicate to the user that the program is still processing.
						FillGridEmailMessages();
						Application.DoEvents();
					}
				}
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"Error receiving email messages")+": "+ex.Message);
			}
			finally {
				Text="Email Inbox for "+AddressInbox.EmailUsername;
			}
			Text="Email Inbox for "+AddressInbox.EmailUsername+" - Resending any acknowledgments which previously failed...";
			EmailMessages.SendOldestUnsentAck(AddressInbox);
			Text="Email Inbox for "+AddressInbox.EmailUsername;
			Cursor=Cursors.Default;
			return emailMessagesTotalCount;
		}

		///<summary>Gets new emails and also shows older emails from the database.</summary>
		private void FillGridEmailMessages() {
			//Remember current email selections.
			List<long> listEmailMessageNumsSelected=new List<long>();
			for(int i=0;i<gridEmailMessages.SelectedIndices.Length;i++) {
				EmailMessage emailMessage=(EmailMessage)gridEmailMessages.Rows[gridEmailMessages.SelectedIndices[i]].Tag;
				listEmailMessageNumsSelected.Add(emailMessage.EmailMessageNum);
			}
			int sortByColIdx=gridEmailMessages.SortedByColumnIdx;
			bool isSortAsc=gridEmailMessages.SortedIsAscending;
			if(sortByColIdx==-1) {
				//Default to sorting by Date Received descending.
				sortByColIdx=2;
				isSortAsc=false;
			}
			//Refresh the list and grid from the database.
			if(comboEmailAddress.SelectedIndex==0 && Security.CurUser.ProvNum!=0) { //webmail
				ListEmailMessages=EmailMessages.GetInboxForAddress(AddressInbox.EmailUsername,Security.CurUser.ProvNum);
			}
			else{
				ListEmailMessages=EmailMessages.GetInboxForAddress(AddressInbox.EmailUsername,0);
			}
			gridEmailMessages.BeginUpdate();
			gridEmailMessages.Rows.Clear();
			gridEmailMessages.Columns.Clear();
			int colReceivedDatePixCount=140;
			int colMessageTypePixCount=120;
			int colFromPixCount=200;
			int colSigPixCount=40;
			int colPatientPixCount=140;
			int variableWidth=gridEmailMessages.Width-10-colFromPixCount-colReceivedDatePixCount-colMessageTypePixCount-colSigPixCount-colPatientPixCount;
			gridEmailMessages.Columns.Add(new UI.ODGridColumn(Lan.g(this,"From"),colFromPixCount,HorizontalAlignment.Left));//0
			gridEmailMessages.Columns[gridEmailMessages.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.StringCompare;
			gridEmailMessages.Columns.Add(new UI.ODGridColumn(Lan.g(this,"Subject"),variableWidth,HorizontalAlignment.Left));//1
			gridEmailMessages.Columns[gridEmailMessages.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.StringCompare;
			gridEmailMessages.Columns.Add(new UI.ODGridColumn(Lan.g(this,"Date Received"),colReceivedDatePixCount,HorizontalAlignment.Left));//2
			gridEmailMessages.Columns[gridEmailMessages.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.DateParse;
			gridEmailMessages.Columns.Add(new UI.ODGridColumn(Lan.g(this,"MessageType"),colMessageTypePixCount,HorizontalAlignment.Left));//3
			gridEmailMessages.Columns[gridEmailMessages.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.StringCompare;
			gridEmailMessages.Columns.Add(new UI.ODGridColumn(Lan.g(this,"Sig"),colSigPixCount,HorizontalAlignment.Center));//4
			gridEmailMessages.Columns[gridEmailMessages.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.StringCompare;
			gridEmailMessages.Columns.Add(new UI.ODGridColumn(Lan.g(this,"Patient"),colPatientPixCount,HorizontalAlignment.Left));//5
			gridEmailMessages.Columns[gridEmailMessages.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.StringCompare;
			Dictionary<long,string> dictPatNames=Patients.GetAllPatientNames();
			for(int i=0;i<ListEmailMessages.Count;i++) {
				EmailMessage emailMessage=ListEmailMessages[i];
				UI.ODGridRow row=new UI.ODGridRow();
				row.Tag=emailMessage;//Used to locate the correct email message if the user decides to sort the grid.
				if(emailMessage.SentOrReceived==EmailSentOrReceived.Received || emailMessage.SentOrReceived==EmailSentOrReceived.WebMailReceived
					|| emailMessage.SentOrReceived==EmailSentOrReceived.ReceivedEncrypted || emailMessage.SentOrReceived==EmailSentOrReceived.ReceivedDirect) {
					row.Bold=true;//unread
					//row.ColorText=UI.ODPaintTools.ColorNotify;
				}
				row.Cells.Add(new UI.ODGridCell(emailMessage.FromAddress));//0 From
				row.Cells.Add(new UI.ODGridCell(emailMessage.Subject));//1 Subject
				row.Cells.Add(new UI.ODGridCell(emailMessage.MsgDateTime.ToString()));//2 ReceivedDate
				row.Cells.Add(new UI.ODGridCell(EmailMessages.GetEmailSentOrReceivedDescript(emailMessage.SentOrReceived)));//3 MessageType
				string sigTrust="";//Blank for no signature, N for untrusted signature, Y for trusted signature.
				for(int j=0;j<emailMessage.Attachments.Count;j++) {
					if(emailMessage.Attachments[j].DisplayedFileName.ToLower()!="smime.p7s") {
						continue;//Not a digital signature.
					}
					sigTrust="N";
					//A more accurate way to test for trust would be to read the subject name from the certificate, then check the trust for the subject name instead of the from address.
					//We use the more accurate way inside FormEmailDigitalSignature.  However, we cannot use the accurate way inside the inbox because it would cause the inbox to load very slowly.
					if(EmailMessages.IsAddressTrusted(emailMessage.FromAddress)) {
						sigTrust="Y";
					}
					break;
				}
				row.Cells.Add(new UI.ODGridCell(sigTrust));//4 Sig
				long patNumRegardingPatient=emailMessage.PatNum;
				//Webmail messages should list the patient as the PatNumSubj, which means "the patient whom this message is regarding".
				if(emailMessage.SentOrReceived==EmailSentOrReceived.WebMailReceived || emailMessage.SentOrReceived==EmailSentOrReceived.WebMailRecdRead) {
					patNumRegardingPatient=emailMessage.PatNumSubj;
				}
				if(patNumRegardingPatient==0) {
					row.Cells.Add("");//5 Patient
				}
				else {
					row.Cells.Add(dictPatNames[patNumRegardingPatient]);//5 Patient
				}
				gridEmailMessages.Rows.Add(row);
			}
			gridEmailMessages.EndUpdate();
			gridEmailMessages.SortForced(sortByColIdx,isSortAsc);
			//Selection must occur after EndUpdate().
			for(int i=0;i<gridEmailMessages.Rows.Count;i++) {
				EmailMessage emailMessage=(EmailMessage)gridEmailMessages.Rows[i].Tag;
				if(listEmailMessageNumsSelected.Contains(emailMessage.EmailMessageNum)) {
					gridEmailMessages.SetSelected(i,true);
				}
			}
		}

		private void gridEmailMessages_CellClick(object sender,UI.ODGridClickEventArgs e) {
			if(gridEmailMessages.SelectedIndices.Length>=2) {
				splitContainerNoFlicker.Panel2Collapsed=true;//Do not show preview if there are more than one emails selected.
				return;
			}
			EmailMessage emailMessage=(EmailMessage)gridEmailMessages.Rows[e.Row].Tag;
			if(EmailMessages.IsSecureWebMail(emailMessage.SentOrReceived)) {
				//We do not yet have a preview for secure web mail messages.
				splitContainerNoFlicker.Panel2Collapsed=true;
				return;
			}
			Cursor=Cursors.WaitCursor;
			EmailMessages.UpdateSentOrReceivedRead(emailMessage);
			emailMessage=EmailMessages.GetOne(emailMessage.EmailMessageNum);//Refresh from the database to get the full body text.
			FillGridEmailMessages();//To show the email is read.
			splitContainerNoFlicker.Panel2Collapsed=false;
			emailPreview.Width=splitContainerNoFlicker.Panel2.Width;//For some reason the anchors do not always work inside panel2.
			emailPreview.Height=splitContainerNoFlicker.Panel2.Height;//For some reason the anchors do not always work inside panel2.
			emailPreview.EmailAddressPreview=AddressInbox;
			emailPreview.LoadEmailMessage(emailMessage);
			Cursor=Cursors.Default;
			//Handle Sig column clicks.
			if(e.Col!=4) {
				return;//Not the Sig column.
			}
			for(int i=0;i<emailMessage.Attachments.Count;i++) {
				if(emailMessage.Attachments[i].DisplayedFileName.ToLower()!="smime.p7s") {
					continue;
				}
				string smimeP7sFilePath=ODFileUtils.CombinePaths(EmailAttaches.GetAttachPath(),emailMessage.Attachments[i].ActualFileName);
				X509Certificate2 certSig=EmailMessages.GetEmailSignatureFromSmimeP7sFile(smimeP7sFilePath);
				FormEmailDigitalSignature form=new FormEmailDigitalSignature(certSig);
				if(form.ShowDialog()==DialogResult.OK) {
					//If the user just added trust, then refresh to pull the newly added certificate into the memory cache.
					EmailMessages.RefreshCertStoreExternal(AddressInbox);
					//Refresh the entire inbox, because there may be multiple email messages from the same address that the user just added trust for.
					//The Sig column may need to be updated on multiple rows.
					GetMessages();
				}
				break;
			}
		}

		private void gridEmailMessages_CellDoubleClick(object sender,UI.ODGridClickEventArgs e) {
			if(e.Row==-1) {
				return;
			}
			EmailMessage emailMessage=(EmailMessage)gridEmailMessages.Rows[e.Row].Tag;
			if(emailMessage.SentOrReceived==EmailSentOrReceived.WebMailReceived
					|| emailMessage.SentOrReceived==EmailSentOrReceived.WebMailRecdRead
					|| emailMessage.SentOrReceived==EmailSentOrReceived.WebMailSent
					|| emailMessage.SentOrReceived==EmailSentOrReceived.WebMailSentRead) 
			{
				//web mail uses special secure messaging portal
				FormWebMailMessageEdit FormWMME=new FormWebMailMessageEdit(emailMessage.PatNum,emailMessage.EmailMessageNum);
				//Will return Abort if validation fails on load or message was deleted, in which case do not set email as read.
				if(FormWMME.ShowDialog() != DialogResult.Abort) {
					EmailMessages.UpdateSentOrReceivedRead(emailMessage);//Mark the message read.
				}				
			}
			else {
				//When an email is read from the database for display in the inbox, the BodyText is limited to 50 characters and the RawEmailIn is blank.
				emailMessage=EmailMessages.GetOne(emailMessage.EmailMessageNum);//Refresh the email from the database to include the full BodyText and RawEmailIn.
				FormEmailMessageEdit formEME=new FormEmailMessageEdit(emailMessage);
				formEME.EmailPreviewAddress=AddressInbox;
				formEME.ShowDialog();
				emailMessage=EmailMessages.GetOne(emailMessage.EmailMessageNum);//Fetch from DB, in case changed due to decrypt.
				if(emailMessage!=null && emailMessage.SentOrReceived!=EmailSentOrReceived.ReceivedEncrypted) {//emailMessage could be null if the message was deleted in FormEmailMessageEdit().
					EmailMessages.UpdateSentOrReceivedRead(emailMessage);
				}
			}
			FillGridEmailMessages();//To show the email is read.
		}

		private void butMarkUnread_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			for(int i=0;i<gridEmailMessages.SelectedIndices.Length;i++) {
				EmailMessage emailMessage=(EmailMessage)gridEmailMessages.Rows[gridEmailMessages.SelectedIndices[i]].Tag;
				EmailMessages.UpdateSentOrReceivedUnread(emailMessage);
			}
			FillGridEmailMessages();
			Cursor=Cursors.Default;
		}

		private void butMarkRead_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			for(int i=0;i<gridEmailMessages.SelectedIndices.Length;i++) {
				EmailMessage emailMessage=(EmailMessage)gridEmailMessages.Rows[gridEmailMessages.SelectedIndices[i]].Tag;
				EmailMessages.UpdateSentOrReceivedRead(emailMessage);
			}
			FillGridEmailMessages();
			Cursor=Cursors.Default;
		}

		private void butChangePat_Click(object sender,EventArgs e) {
			if(gridEmailMessages.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an email message.");
				return;
			}
			FormPatientSelect form=new FormPatientSelect();
			if(form.ShowDialog()!=DialogResult.OK) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			for(int i=0;i<gridEmailMessages.SelectedIndices.Length;i++) {
				EmailMessage emailMessage=(EmailMessage)gridEmailMessages.Rows[gridEmailMessages.SelectedIndices[i]].Tag;
				emailMessage.PatNum=form.SelectedPatNum;
				EmailMessages.UpdatePatNum(emailMessage);
			}
			int messagesMovedCount=gridEmailMessages.SelectedIndices.Length;
			FillGridEmailMessages();//Refresh grid to show changed patient.
			Cursor=Cursors.Default;
			MessageBox.Show(Lan.g(this,"Email messages moved successfully")+": "+messagesMovedCount);
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			GetMessages();
			Cursor=Cursors.Default;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(gridEmailMessages.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select email to delete.");
				return;
			}
			if(!MsgBox.Show(this,true,"Permanently delete all selected email?")) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			for(int i=0;i<gridEmailMessages.SelectedIndices.Length;i++) {
				EmailMessage emailMessage=(EmailMessage)gridEmailMessages.Rows[gridEmailMessages.SelectedIndices[i]].Tag;
				if(EmailMessages.IsSecureWebMail(emailMessage.SentOrReceived)) {
						EmailMessages.Delete(emailMessage);
						string logText="";
						logText+="\r\n"+Lan.g(this,"From")+": "+emailMessage.FromAddress+". ";
						logText+="\r\n"+Lan.g(this,"To")+": "+emailMessage.ToAddress+". ";
						if(!String.IsNullOrEmpty(emailMessage.Subject)) {
							logText+="\r\n"+Lan.g(this,"Subject")+": "+emailMessage.Subject+". ";
						}
						if(!String.IsNullOrEmpty(emailMessage.BodyText)) {
							if(emailMessage.BodyText.Length > 50) {
								logText+="\r\n"+Lan.g(this,"Body Text")+": "+emailMessage.BodyText.Substring(0,49)+"... ";
							}
							else {
								logText+="\r\n"+Lan.g(this,"Body Text")+": "+emailMessage.BodyText;
							}
						}
						if(emailMessage.MsgDateTime != DateTime.MinValue) {
							logText+="\r\n"+Lan.g(this,"Date")+": "+emailMessage.MsgDateTime.ToShortDateString()+". ";
						}
						SecurityLogs.MakeLogEntry(Permissions.WebmailDelete,emailMessage.PatNum,Lan.g(this,"Webmail deleted.")+" "+logText);
				}
				else {//Not a web mail message.
					EmailMessages.Delete(emailMessage);
				}
			}
			FillGridEmailMessages();
			Cursor=Cursors.Default;
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}