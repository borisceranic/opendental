using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental {
	public partial class FormEmailInbox:Form {
		private EmailAddress AddressInbox;
		private List<EmailMessage> ListEmailMessages;

		public FormEmailInbox() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormEmailInbox_Load(object sender,EventArgs e) {
			labelInboxComputerName.Text="Computer Name Where New Email Is Retrieved: "+PrefC.GetString(PrefName.EmailInboxComputerName);
			labelThisComputer.Text+=Dns.GetHostName();
			Application.DoEvents();//Show the form contents before loading email into the grid.
			GetMessages();//If no new messages, then the user will know based on what shows in the grid.
		}

		private void FormEmailInbox_Resize(object sender,EventArgs e) {
			FillGridEmailMessages();//To resize the columnns in the grid as needed.
		}

		private void menuItemSetup_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormEmailAddresses formEA=new FormEmailAddresses();
			formEA.ShowDialog();
			labelInboxComputerName.Text="Computer Name Where New Email Is Fetched: "+PrefC.GetString(PrefName.EmailInboxComputerName);
			GetMessages();//Get new messages, just in case the user entered email information for the first time.
		}

		///<summary>Gets new messages from email inbox, as well as older messages from the db. Also fills the grid.</summary>
		private int GetMessages() {
			AddressInbox=EmailAddresses.GetByClinic(0);//Default for clinic/practice.
			Cursor=Cursors.WaitCursor;
			FillGridEmailMessages();//Show what is in db.
			Cursor=Cursors.Default;
			if(AddressInbox.EmailUsername=="" || AddressInbox.Pop3ServerIncoming=="") {//Email address not setup.
				Text="Email Inbox - Showing webmail messages only.  Either no email addresses have been setup or the default address is not setup fully.";
				return 0;
			}
			Text="Email Inbox for "+AddressInbox.EmailUsername;
			Application.DoEvents();//So that something is showing while the page is loading.
			if(!CodeBase.ODEnvironment.IdIsThisComputer(PrefC.GetString(PrefName.EmailInboxComputerName))) {//This is not the computer to get new messages from.
				return 0;
			}
			if(PrefC.GetString(PrefName.EmailInboxComputerName)=="") {
				MsgBox.Show(this,"Computer name to fetch new email from has not been setup.");
				return 0;
			}
			Cursor=Cursors.WaitCursor;
			int emailMessagesTotalCount=0;
			Text="Email Inbox for "+AddressInbox.EmailUsername+" - Fetching new email...";
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
				MessageBox.Show(Lan.g(this,"Error retrieving email messages")+": "+ex.Message);
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
			ListEmailMessages=EmailMessages.GetInboxForAddress(AddressInbox.EmailUsername,Security.CurUser.ProvNum);
			gridEmailMessages.BeginUpdate();
			gridEmailMessages.Rows.Clear();
			gridEmailMessages.Columns.Clear();
			int colReceivedDatePixCount=140;
			int colMessageTypePixCount=120;
			int colFromPixCount=200;
			int colSigPixCount=24;
			int colPatientPixCount=140;
			int variableWidth=gridEmailMessages.Width-10-colFromPixCount-colReceivedDatePixCount-colMessageTypePixCount-colSigPixCount-colPatientPixCount;
			gridEmailMessages.Columns.Add(new UI.ODGridColumn(Lan.g(this,"From"),colFromPixCount,HorizontalAlignment.Left));//0
			gridEmailMessages.Columns[gridEmailMessages.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.StringCompare;
			gridEmailMessages.Columns.Add(new UI.ODGridColumn(Lan.g(this,"Subject"),variableWidth,HorizontalAlignment.Left));//1
			gridEmailMessages.Columns[gridEmailMessages.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.StringCompare;
			gridEmailMessages.Columns.Add(new UI.ODGridColumn(Lan.g(this,"Date Received"),colReceivedDatePixCount,HorizontalAlignment.Center));//2
			gridEmailMessages.Columns[gridEmailMessages.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.DateParse;
			gridEmailMessages.Columns.Add(new UI.ODGridColumn(Lan.g(this,"MessageType"),colMessageTypePixCount,HorizontalAlignment.Center));//3
			gridEmailMessages.Columns[gridEmailMessages.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.StringCompare;
			gridEmailMessages.Columns.Add(new UI.ODGridColumn(Lan.g(this,"Sig"),colSigPixCount,HorizontalAlignment.Center));//4
			gridEmailMessages.Columns[gridEmailMessages.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.StringCompare;
			gridEmailMessages.Columns.Add(new UI.ODGridColumn(Lan.g(this,"Patient"),colPatientPixCount,HorizontalAlignment.Left));//5
			gridEmailMessages.Columns[gridEmailMessages.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.StringCompare;			
			for(int i=0;i<ListEmailMessages.Count;i++) {
				EmailMessage emailMessage=ListEmailMessages[i];
				UI.ODGridRow row=new UI.ODGridRow();
				row.Tag=emailMessage;//Used to locate the correct email message if the user decides to sort the grid.
				if(emailMessage.SentOrReceived==EmailSentOrReceived.Received || emailMessage.SentOrReceived==EmailSentOrReceived.WebMailReceived
					|| emailMessage.SentOrReceived==EmailSentOrReceived.ReceivedEncrypted || emailMessage.SentOrReceived==EmailSentOrReceived.ReceivedDirect) {
					row.Bold=true;//unread
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
					row.Cells.Add(new UI.ODGridCell(""));//5 Patient
				}
				else {
					Patient pat=Patients.GetPat(patNumRegardingPatient);
					row.Cells.Add(new UI.ODGridCell(pat.GetNameLF()));//5 Patient
				}
				gridEmailMessages.Rows.Add(row);
			}
			gridEmailMessages.EndUpdate();
		}

		private void gridEmailMessages_CellClick(object sender,UI.ODGridClickEventArgs e) {
			EmailMessage emailMessage=(EmailMessage)gridEmailMessages.Rows[e.Row].Tag;
			if(EmailMessages.IsSecureWebMail(emailMessage.SentOrReceived)) {
				//We do not yet have a preview for secure web mail messages.
				splitContainerNoFlicker.Panel2Collapsed=true;
				return;
			}
			EmailMessages.UpdateSentOrReceivedRead(emailMessage);
			emailMessage=EmailMessages.GetOne(emailMessage.EmailMessageNum);//Refresh from the database to get the full body text.
			FillGridEmailMessages();//To show the email is read.
			gridEmailMessages.SetSelected(e.Row,true);
			splitContainerNoFlicker.Panel2Collapsed=false;
			emailPreview.Width=splitContainerNoFlicker.Panel2.Width;//For some reason the anchors do not always work inside panel2.
			emailPreview.Height=splitContainerNoFlicker.Panel2.Height;//For some reason the anchors do not always work inside panel2.
			emailPreview.LoadEmailMessage(emailMessage);
			//Handle Sig column clicks.
			if(e.Col!=4) {
				return;//Not the Sig column.
			}
			for(int i=0;i<emailMessage.Attachments.Count;i++) {
				if(emailMessage.Attachments[i].DisplayedFileName.ToLower()!="smime.p7s") {
					continue;
				}
				string smimeP7sFilePath=ODFileUtils.CombinePaths(EmailMessages.GetEmailAttachPath(),emailMessage.Attachments[i].ActualFileName);
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
				if(FormWMME.ShowDialog()!=DialogResult.Abort) { //will only return Abort if validation fails on load, in which case the message will remain unread
					EmailMessages.UpdateSentOrReceivedRead(emailMessage);//Mark the message read.
				}				
			}
			else {
				//When an email is read from the database for display in the inbox, the BodyText is limited to 50 characters and the RawEmailIn is blank.
				emailMessage=EmailMessages.GetOne(emailMessage.EmailMessageNum);//Refresh the email from the database to include the full BodyText and RawEmailIn.
				FormEmailMessageEdit formEME=new FormEmailMessageEdit(emailMessage);
				formEME.ShowDialog();
				emailMessage=EmailMessages.GetOne(emailMessage.EmailMessageNum);//Fetch from DB, in case changed due to decrypt.
				if(emailMessage!=null && emailMessage.SentOrReceived!=EmailSentOrReceived.ReceivedEncrypted) {//emailMessage could be null if the message was deleted in FormEmailMessageEdit().
					EmailMessages.UpdateSentOrReceivedRead(emailMessage);
				}
			}
			FillGridEmailMessages();//To show the email is read.
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
			for(int i=0;i<gridEmailMessages.SelectedIndices.Length;i++) {
				EmailMessage emailMessage=(EmailMessage)gridEmailMessages.Rows[gridEmailMessages.SelectedIndices[i]].Tag;
				emailMessage.PatNum=form.SelectedPatNum;
				EmailMessages.UpdatePatNum(emailMessage);
			}
			MessageBox.Show(Lan.g(this,"Email messages moved successfully")+": "+gridEmailMessages.SelectedIndices.Length);
			FillGridEmailMessages();//Refresh grid to show changed patient.
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

		private void butRefresh_Click(object sender,EventArgs e) {
			GetMessages();
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
			int webMailCount=0;
			for(int i=0;i<gridEmailMessages.SelectedIndices.Length;i++) {
				EmailMessage emailMessage=(EmailMessage)gridEmailMessages.Rows[gridEmailMessages.SelectedIndices[i]].Tag;
				//We currently don't allow deleting web mail messages.
				if(emailMessage.SentOrReceived==EmailSentOrReceived.WebMailReceived
					|| emailMessage.SentOrReceived==EmailSentOrReceived.WebMailRecdRead
					|| emailMessage.SentOrReceived==EmailSentOrReceived.WebMailSent
					|| emailMessage.SentOrReceived==EmailSentOrReceived.WebMailSentRead) 
				{
					webMailCount++;
				}
				else {//Not a web mail message.
					EmailMessages.Delete(emailMessage);
				}
			}
			FillGridEmailMessages();
			Cursor=Cursors.Default;
			if(webMailCount > 0) {
				MsgBox.Show(this,"Not allowed to delete web mail messages.  Web mail messages skipped: "+webMailCount);
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}