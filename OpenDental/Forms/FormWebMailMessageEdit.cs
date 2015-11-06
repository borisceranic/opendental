using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;
using System.Linq;

namespace OpenDental {
	///<summary>DialogResult will be Abort if message was unable to be read. If message is read successfully (Ok or Cancel), then caller is responsible
	///for updating SentOrReceived to read (where applicable).</summary>
	public partial class FormWebMailMessageEdit:Form {
		private EmailMessage _secureMessage;
		private EmailMessage _insecureMessage;
		private EmailAddress _emailAddressSender;
		private long _patNum;
		///<summary>This is the emailMessageNum parameter from the constructors, either 0 if composing a new message or the EmailMessageNum of the
		///existing message being viewed/replied to.</summary>
		private long _replyToEmailMessageNum;
		private bool _allowSendSecureMessage=true;
		private bool _allowSendNotificationMessage=true;
		///<summary>If viewing existing message, this will be a list with only the "regarding" patient in it.  If replying or composing, this will contain
		///all family members of the current patient of which the patient is eligible to view given PHI constraints.</summary>
		private List<Patient> _listPatients=null;
		///<summary>Set to the message with EmailMessageNum=_replyToEmailMessageNum for viewing existing messages or replying to an existing message.
		///If we are composing a new message this will be null.</summary>
		private EmailMessage _emailMessage;
		///<summary>Attachment objects will be set right before inserting _secureMessage into db. Until then they will be held separate.  If viewing an
		///existing message, this will hold the list of attachments sent with the message.</summary>
		private List<EmailAttach> _listAttachments=new List<EmailAttach>();
		///<summary>On load, the form will be Compose mode if emailMessageNum is not sent in or is 0.
		///Otherwise, on load the form will be View mode, meaning we are viewing an existing message.
		///Once the user presses the reply button, the form reloads in Reply mode.</summary>
		private _formMode _formModeCur;

		private enum _formMode {
			Compose,
			View,
			Reply
		}

		///<summary>Default constructor. This implies that we are composing a new message, NOT replying to an existing message.
		///Provider attached to this message should be Security.CurUser.ProvNum</summary>
		public FormWebMailMessageEdit(long patNum) : this(patNum,0) { }

		///<summary>This implies that we are replying to an existing message. Provider attached to this message will be the ProvNum attached to the
		///original message. If this ProvNum does not match Security.CurUser.ProvNum then the send action will be blocked.</summary>
		public FormWebMailMessageEdit(long patNum,long emailMessageNum) {
			InitializeComponent();
			Lan.F(this);
			_replyToEmailMessageNum=emailMessageNum;
			_patNum=patNum;
		}

		private void FormWebMailMessageEdit_Load(object sender,EventArgs e) {
			_formModeCur=_formMode.View;
			if(_replyToEmailMessageNum<1) {
				_formModeCur=_formMode.Compose;
			}
			FillFields();
		}

		///<summary>On load, we are either viewing an existing message (_isReply is false) or composing a new message (_replyToEmailMessageNum is 0).
		///If _replyToEmailMessageNum > 0 and _isReply is false, fill the fields with the existing message information.
		///Otherwise call VerifyInputs to fill the fields with the appropriate information for either a reply message or a new message.</summary>
		private void FillFields() {
			if(_formModeCur!=_formMode.View) {//composing a new message or replying to an existing message
				_listAttachments=new List<EmailAttach>();
				FillAttachments();
				SetEnabledHelper();
				VerifyInputs();
				return;
			}
			//_emailMessage is not null and _isReply is false, viewing an existing message
			_emailMessage=EmailMessages.GetOne(_replyToEmailMessageNum);
			if(_emailMessage==null) {
				MsgBox.Show(this,"Invalid email message");
				DialogResult=DialogResult.Abort;//nothing to show so abort, caller should be waiting for abort to determine if message should be marked read
				return;
			}
			SetEnabledHelper();
			Patient patRegarding=Patients.GetPat(_emailMessage.PatNumSubj);
			comboRegardingPatient.Items.Clear();
			_listPatients=new List<Patient>();
			if(patRegarding!=null) {
				_listPatients.Add(patRegarding);
				comboRegardingPatient.Items.Add(patRegarding.GetNameFL());
				comboRegardingPatient.SelectedIndex=0;
			}
			textTo.Text=_emailMessage.ToAddress;
			textFrom.Text=_emailMessage.FromAddress;
			textSubject.Text=_emailMessage.Subject;
			textBody.Text=_emailMessage.BodyText;
			_listAttachments=_emailMessage.Attachments;
			FillAttachments();
		}

		///<summary>Sets Enabled/ReadOnly status for fields based on whether we are viewing an existing message or replying to/composing a message.</summary>
		private void SetEnabledHelper() {
			if(_formModeCur!=_formMode.View) {//if _emailMessage is null, we are composing a new message
				comboRegardingPatient.Enabled=true;
				textSubject.ReadOnly=false;
				textBody.ReadOnly=false;
				textBody.BackColor=SystemColors.Window;
				butAttach.Enabled=true;
				butPreview.Enabled=true;
				butSend.Text=Lan.g(this,"&Send");
				butSend.Tag="";
				listAttachments.ContextMenu=contextMenuAttachments;//contains a remove and open
				//labelNotification.Text will be set in VerifyInputs based on input values
			}
			else {
				comboRegardingPatient.Enabled=false;
				textSubject.ReadOnly=true;
				textBody.ReadOnly=true;
				textBody.BackColor=SystemColors.Control;
				butAttach.Enabled=false;
				butPreview.Enabled=false;
				butSend.Text=Lan.g(this,"&Reply");
				butSend.Tag="Reply";
				listAttachments.ContextMenu=new ContextMenu(new[] { menuItemAttachmentPreview });
				labelNotification.Text="";
			}
		}

		private void FillAttachments() {
			listAttachments.Items.Clear();
			for(int i=0;i<_listAttachments.Count;i++) {
				listAttachments.Items.Add(_listAttachments[i].DisplayedFileName);
			}
			if(_listAttachments.Count>0) {
				listAttachments.SelectedIndex=0;
			}
		}

		private void BlockSendSecureMessage(string reason) {
			_allowSendSecureMessage=false;
			butSend.Enabled=false;
			butPreview.Enabled=false;
			labelNotification.Text=Lan.g(this,"Warning")+": "+Lan.g(this,"Secure email send prevented")+" - "+Lan.g(this,reason);
			labelNotification.ForeColor=Color.Red;
		}

		private void BlockSendNotificationMessage(string reason) {
			_allowSendNotificationMessage=false;
			butSend.Enabled=false;
			butPreview.Enabled=false;
			labelNotification.Text=Lan.g(this,"Warning")+": "+Lan.g(this,"Notification email send prevented")+" - "+Lan.g(this,reason);
			labelNotification.ForeColor=Color.Red;
		}

		public void AllowSendMessages() {
			_allowSendSecureMessage=true;
			_allowSendNotificationMessage=true;
			butSend.Enabled=true;
			butPreview.Enabled=true;
			labelNotification.ForeColor=SystemColors.ControlText;
			labelNotification.ForeColor=SystemColors.ControlText;
		}

		private void VerifyInputs() {
			AllowSendMessages();
			long priProvNum=0;
			long patNumSubj=_patNum;
			string notificationSubject;
			string notificationBodyNoUrl;
			string notificationURL;
			List<Patient> listPatsForFamily=Patients.GetPatientsForPhi(_patNum);
			Patient patCur=Patients.GetPat(_patNum);
			comboRegardingPatient.Items.Clear();
			if(patCur==null) {
				BlockSendSecureMessage("Patient is invalid.");
				_listPatients=null;
			}
			else {
				textTo.Text=patCur.GetNameFL();
				Provider priProv=Providers.GetProv(patCur.PriProv);
				if(priProv==null) {
					BlockSendSecureMessage("Invalid primary provider for this patient.");
				}
				else {
					priProvNum=priProv.ProvNum;
					Provider userODProv=Providers.GetProv(Security.CurUser.ProvNum);
					if(userODProv==null) {
						BlockSendSecureMessage("Not logged in as valid provider. Login as patient's primary provider to send message.");
					}
					else if(userODProv.ProvNum!=priProv.ProvNum) {
						BlockSendSecureMessage("The patient's primary provider does not match the provider attached to the user currently logged in. "
							+"Login as patient's primary provider to send message.");
					}
					else {
						textFrom.Text=priProv.GetFormalName();
					}
				}
				if(patCur.Email=="") {
					BlockSendNotificationMessage("Missing patient email. Setup patient email using Family module.");
				}
				if(patCur.OnlinePassword=="") {
					BlockSendNotificationMessage("Patient has not been given online access. Setup patient online access using Chart module.");
				}
			}
			//We are replying to an existing message so verify that the provider linked to this message matches our currently logged in provider.  
			//This is because web mail communications will be visible in the patients Chart Module.
			if(_replyToEmailMessageNum>0) {
				_emailMessage=EmailMessages.GetOne(_replyToEmailMessageNum);
				if(_emailMessage==null) {
					MsgBox.Show(this,"Invalid input email message");
					DialogResult=DialogResult.Abort;//nothing to show so abort, caller should be waiting for abort to determine if message should be marked read
					return;
				}
				textSubject.Text=_emailMessage.Subject;
				if(_emailMessage.Subject.IndexOf("RE:")!=0) {
					textSubject.Text="RE: "+textSubject.Text;
				}
				patNumSubj=_emailMessage.PatNumSubj;
				Patient patRegarding=Patients.GetOnePat(listPatsForFamily.ToArray(),patNumSubj);
				if(patRegarding.PatNum==0) {
					BlockSendNotificationMessage("Patient who sent this message cannot access PHI for regarding patient.");
				}
				textBody.Text="\r\n\r\n-----"+Lan.g(this,"Original Message")+"-----\r\n"
					+(patRegarding.PatNum==0 ? "" : (Lan.g(this,"Regarding Patient")+": "+patRegarding.GetNameFL()+"\r\n"))
					+Lan.g(this,"From")+": "+_emailMessage.FromAddress+"\r\n"
					+Lan.g(this,"Sent")+": "+_emailMessage.MsgDateTime.ToShortDateString()+" "+_emailMessage.MsgDateTime.ToShortTimeString()+"\r\n"
					+Lan.g(this,"To")+": "+_emailMessage.ToAddress+"\r\n"
					+Lan.g(this,"Subject")+": "+_emailMessage.Subject
					+"\r\n\r\n"+_emailMessage.BodyText;
			}
			if(patCur==null || listPatsForFamily.Count==0) {
				BlockSendSecureMessage("Patient's family not setup propertly. Make sure guarantor is valid.");
			}
			else {
				_listPatients=new List<Patient>();
				for(int i=0;i<listPatsForFamily.Count;i++) {
					Patient patFamilyMember=listPatsForFamily[i];
					_listPatients.Add(patFamilyMember);
					comboRegardingPatient.Items.Add(patFamilyMember.GetNameFL());
					if(patFamilyMember.PatNum==patNumSubj) {
						comboRegardingPatient.SelectedIndex=(comboRegardingPatient.Items.Count-1);
					}
				}
			}
			notificationSubject=PrefC.GetString(PrefName.PatientPortalNotifySubject);
			notificationBodyNoUrl=PrefC.GetString(PrefName.PatientPortalNotifyBody);
			notificationURL=PrefC.GetString(PrefName.PatientPortalURL);
			//Webmail notification email address.  One notification email per database (not clinic specific).
			_emailAddressSender=EmailAddresses.GetOne(PrefC.GetLong(PrefName.EmailNotifyAddressNum));
			if(_emailAddressSender==null 
				|| _emailAddressSender.EmailAddressNum==0
				|| _emailAddressSender.EmailUsername=="")
			{
				//No valid "Notify" email setup for this practice yet.
				BlockSendNotificationMessage("Practice email is not setup properly. Setup practice email in setup.");
			}
			if(notificationSubject=="") {
				BlockSendNotificationMessage("Missing notification email subject. Create a subject in setup.");
			}
			if(notificationBodyNoUrl=="") {
				BlockSendNotificationMessage("Missing notification email body. Create a body in setup.");
			}
			if(_allowSendSecureMessage) {
				_secureMessage=new EmailMessage();
				_secureMessage.FromAddress=textFrom.Text;
				_secureMessage.ToAddress=textTo.Text;
				_secureMessage.PatNum=patCur.PatNum;
				_secureMessage.SentOrReceived=EmailSentOrReceived.WebMailSent;  //this is secure so mark as webmail sent
				_secureMessage.ProvNumWebMail=priProvNum;
			}
			if(_allowSendNotificationMessage) {
				_insecureMessage=new EmailMessage();
				_insecureMessage.FromAddress=_emailAddressSender.SenderAddress;
				_insecureMessage.ToAddress=patCur.Email;
				_insecureMessage.PatNum=patCur.PatNum;
				_insecureMessage.Subject=notificationSubject;
				_insecureMessage.BodyText=notificationBodyNoUrl.Replace("[URL]",notificationURL);
				_insecureMessage.SentOrReceived=EmailSentOrReceived.Sent; //this is not secure so just mark as regular sent
			}
			if(_allowSendSecureMessage && _allowSendNotificationMessage) {
				labelNotification.Text=Lan.g(this,"Notification email will be sent to patient")+": "+patCur.Email;
			}
		}

		private bool VerifyOutputs() {
			if(textSubject.Text=="") {
				MsgBox.Show(this,"Enter a subject");
				textSubject.Focus();
				return false;
			}
			if(textBody.Text=="") {
				MsgBox.Show(this,"Email body is empty");
				textBody.Focus();
				return false;
			}
			if(GetPatNumSubj()<=0) {
				MsgBox.Show(this,"Select a valid patient");
				comboRegardingPatient.Focus();
				return false;
			}
			return true;
		}

		private long GetPatNumSubj() {
			try {
				if(_listPatients==null) {
					return 0;
				}
				return _listPatients[comboRegardingPatient.SelectedIndex].PatNum;
			}
			catch {
				return 0;
			}
		}

		private void listAttachments_MouseDown(object sender,MouseEventArgs e) {
			//A right click also needs to select an items so that the context menu will work properly.
			if(e.Button==MouseButtons.Right) {
				int clickedIndex=listAttachments.IndexFromPoint(e.X,e.Y);
				if(clickedIndex!=-1) {
					listAttachments.SelectedIndex=clickedIndex;
				}
			}
		}

		private void listAttachments_DoubleClick(object sender,EventArgs e) {
			try {
				if(listAttachments.SelectedIndex==-1) {
					return;
				}
				string strFilePathAttach=ODFileUtils.CombinePaths(EmailAttaches.GetAttachPath(),_listAttachments[listAttachments.SelectedIndex].ActualFileName);
				//We have to create a copy of the file because the name is different.
				//There is also a high probability that the attachment no longer exists if
				//the A to Z folders are disabled, since the file will have originally been
				//placed in the temporary directory.
				string tempFile=ODFileUtils.CombinePaths(PrefL.GetTempFolderPath(),_listAttachments[listAttachments.SelectedIndex].DisplayedFileName);
				File.Copy(strFilePathAttach,tempFile,true);
				Process.Start(tempFile);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void menuItemAttachmentPreview_Click(object sender,EventArgs e) {
			listAttachments_DoubleClick(sender,e);
		}

		private void menuItemAttachmentRemove_Click(object sender,EventArgs e) {
			try {
				if(listAttachments.SelectedIndex==-1) {
					return;
				}
				File.Delete(ODFileUtils.CombinePaths(EmailAttaches.GetAttachPath(),_listAttachments[listAttachments.SelectedIndex].ActualFileName));
				_listAttachments.RemoveAt(listAttachments.SelectedIndex);
				FillAttachments();
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void menuItemSetup_Click(object sender,EventArgs e) {
			FormEServicesSetup formPPS=new FormEServicesSetup();
			formPPS.ShowDialog();
			if(formPPS.DialogResult==DialogResult.OK) {
				VerifyInputs();
			}
		}

		private void butPreview_Click(object sender,EventArgs e) {
			if(!VerifyOutputs()) {
				return;
			}
			StringBuilder sb=new StringBuilder();
			sb.AppendLine("------ "+Lan.g(this,"Notification email that will be sent to the patient's email address:"));
			if(_allowSendNotificationMessage) {
				sb.AppendLine(Lan.g(this,"Subject")+": "+_insecureMessage.Subject);
				sb.AppendLine(Lan.g(this,"Body")+": "+_insecureMessage.BodyText);
			}
			else {
				sb.AppendLine(Lan.g(this,"------ "+Lan.g(this,"Notification email settings are not set up.  Click Setup from the web mail message edit window"
					+" to set up notification emails")+" ------"));
			}
			sb.AppendLine();
			sb.AppendLine("------ "+Lan.g(this,"Secure web mail message that will be sent to the patient's portal:"));
			sb.AppendLine(Lan.g(this,"Subject")+": "+textSubject.Text);
			sb.AppendLine(Lan.g(this,"Body")+": "+textBody.Text.Replace("\n","\r\n"));
			MsgBoxCopyPaste msgBox=new MsgBoxCopyPaste(sb.ToString());
			msgBox.ShowDialog();
		}

		private void butAttach_Click(object sender,EventArgs e) {
			OpenFileDialog dlg=new OpenFileDialog();
			dlg.Multiselect=true;
			Patient patCur=Patients.GetPat(_patNum);
			if(patCur!=null && patCur.ImageFolder!="") {
				if(PrefC.AtoZfolderUsed) {
					dlg.InitialDirectory=ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(),
						patCur.ImageFolder.Substring(0,1).ToUpper(),patCur.ImageFolder);
				}
				else {
					//Use the OS default directory for this type of file viewer.
					dlg.InitialDirectory="";
				}
			}
			if(dlg.ShowDialog()!=DialogResult.OK) {
				return;
			}
			Random rnd=new Random();
			string newName;
			EmailAttach attach;
			string attachPath=EmailAttaches.GetAttachPath();
			try {
				for(int i=0;i<dlg.FileNames.Length;i++) {
					//copy the file
					newName=DateTime.Now.ToString("yyyyMMdd")+"_"+DateTime.Now.TimeOfDay.Ticks.ToString()+rnd.Next(1000).ToString()
						+Path.GetExtension(dlg.FileNames[i]);
					File.Copy(dlg.FileNames[i],ODFileUtils.CombinePaths(attachPath,newName));
					//create the attachment
					attach=new EmailAttach();
					attach.DisplayedFileName=Path.GetFileName(dlg.FileNames[i]);
					attach.ActualFileName=newName;
					_listAttachments.Add(attach);
				}
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
			FillAttachments();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(_emailMessage==null) {
				DialogResult=DialogResult.Abort;//Nothing to do the message doesn't exist.
				return;
			}
			if(!MsgBox.Show(this,true,"Are you sure you want to delete this webmail?")) {
				return;
			}
			EmailMessages.Delete(_emailMessage);
			string logText="";
			logText+="\r\n"+Lan.g(this,"From")+": "+_emailMessage.FromAddress+". ";
			logText+="\r\n"+Lan.g(this,"To")+": "+_emailMessage.ToAddress+". ";
			if(!String.IsNullOrEmpty(_emailMessage.Subject)) {
				logText+="\r\n"+Lan.g(this,"Subject")+": "+_emailMessage.Subject+". ";
			}
			if(!String.IsNullOrEmpty(_emailMessage.BodyText)) {
				if(_emailMessage.BodyText.Length > 50) {
					logText+="\r\n"+Lan.g(this,"Body Text")+": "+_emailMessage.BodyText.Substring(0,49)+"... ";
				}
				else {
					logText+="\r\n"+Lan.g(this,"Body Text")+": "+_emailMessage.BodyText;
				}
			}
			if(_emailMessage.MsgDateTime != DateTime.MinValue) {
				logText+="\r\n"+Lan.g(this,"Date")+": "+_emailMessage.MsgDateTime.ToShortDateString()+". ";
			}
			SecurityLogs.MakeLogEntry(Permissions.WebmailDelete,_emailMessage.PatNum,Lan.g(this,"Webmail deleted.")+" "+logText);
			DialogResult=DialogResult.Abort;//We want to abort here to avoid using the email in parent windows when it's been deleted.
		}

		///<summary>When viewing an existing message, the "Send" button text will be "Reply" and _formModeCur will be View.  Pressing the button will
		///reload this form as a reply message.
		///When composing a new message or replying to an existing message, the button text will be "Send" and _formModeCur will be
		///either Compose or Reply.  Pressing the button will cause an attempt to send the secure and insecure message if applicable.</summary>
		private void butSend_Click(object sender,EventArgs e) {
			if(_formModeCur==_formMode.View) {
				_formModeCur=_formMode.Reply;
				FillFields();
				return;
			}
			if(!Security.IsAuthorized(Permissions.WebmailSend,false)) {
				return;
			}
			if(!_allowSendSecureMessage) {
				MsgBox.Show(this,"Send not permitted");
				return;
			}
			if(!VerifyOutputs()) {
				return;
			}
			//Insert the message. The patient will not see this as an actual email.
			//Rather, they must login to the patient portal (secured) and view the message that way.
			//This is how we get around sending the patient a secure message, which would be a hassle for all involved.
			_secureMessage.Subject=textSubject.Text;
			_secureMessage.BodyText=textBody.Text;
			_secureMessage.MsgDateTime=DateTime.Now;
			_secureMessage.PatNumSubj=GetPatNumSubj();
			if(_allowSendNotificationMessage) {
				//Send an insecure notification email to the patient.
				_insecureMessage.MsgDateTime=DateTime.Now;
				_insecureMessage.PatNumSubj=GetPatNumSubj();
				try {
					EmailMessages.SendEmailUnsecure(_insecureMessage,_emailAddressSender);
					//Insert the notification email into the emailmessage table so we have a record that it was sent.
					EmailMessages.Insert(_insecureMessage);
				}
				catch(Exception ex) {
					MsgBox.Show(this,ex.Message);
					return;
				}
			}
			_secureMessage.Attachments=_listAttachments;
			EmailMessages.Insert(_secureMessage);
			MsgBox.Show(this,"Message Sent");
			SecurityLogs.MakeLogEntry(Permissions.WebmailSend,0,"Webmail Sent");
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}