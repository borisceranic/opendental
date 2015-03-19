using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental.User_Controls {
	public partial class EmailPreviewControl:UserControl {

		private bool _isLoading=false;
		private EmailMessage MessageCur=null;
		private bool _isComposing=false;
		///<summary>TODO: Replace this flag with a new flag on the email address object.</summary>
		private bool _isSigningEnabled=true;
		private X509Certificate2 _certSig=null;
		private List<EmailAttach> _listEmailAttachDisplayed=null;
		///<summary>Used when sending to get Clinic.</summary>
		private Patient _patCur=null;
		private bool _isMessageChanged=false;

		public bool IsComposing {
			get { return _isComposing; }
		}

		public bool IsMessageChanged {
			get { return _isMessageChanged; }
		}

		///<summary>Can return null.</summary>
		public Patient PatCur {
			get { return _patCur; }
		}

		public string Subject {
			get { return textSubject.Text; }
			set { textSubject.Text=value; }
		}

		public string BodyText {
			get { return textBodyText.Text; }
			set { textBodyText.Text=value; }
		}

		public string FromAddress {
			get { return textFromAddress.Text; }
		}

		public string ToAddress {
			get { return textToAddress.Text; }
			set { textToAddress.Text=value; }
		}

		public EmailAddress GetEmailAddress() {
			if(_patCur==null) {//can happen if sending deposit slip by email
				return EmailAddresses.GetByClinic(0);//gets the practice default address
			}
			return EmailAddresses.GetByClinic(_patCur.ClinicNum);
		}

		public bool IsSigned {
			get { return (_isSigningEnabled && _certSig!=null); }
		}

		public X509Certificate2 Signature {
			get {
				if(IsSigned) {
					return _certSig;
				}
				return null;
			}
		}

		public EmailPreviewControl() {
			InitializeComponent();
			gridAttachments.ContextMenu=contextMenuAttachments;
		}

		public void LoadEmailMessage(EmailMessage emailMessage) {
			MessageCur=emailMessage;
			_patCur=Patients.GetPat(MessageCur.PatNum);//we could just as easily pass this in.
			if(MessageCur.SentOrReceived==EmailSentOrReceived.Neither) {//Composing a message
				_isComposing=true;
				if(_isSigningEnabled) {
					SetSig(EmailMessages.GetCertFromPrivateStore(MessageCur.FromAddress));
				}
			}
			else {//sent or received (not composing)
				_isComposing=false;
				textMsgDateTime.Text=MessageCur.MsgDateTime.ToString();
				textMsgDateTime.ForeColor=Color.Black;
				butAttach.Visible=false;
				textFromAddress.ReadOnly=true;
				textToAddress.ReadOnly=true;
				textSubject.ReadOnly=true;
				textSubject.SpellCheckIsEnabled=false;//Prevents slowness resizing the window, because spell checker runs each time resize event is fired.
				textBodyText.ReadOnly=true;
				textBodyText.SpellCheckIsEnabled=false;//Prevents slowness resizing the window, because spell checker runs each time resize event is fired.
			}
			textSentOrReceived.Text=MessageCur.SentOrReceived.ToString();
			textFromAddress.Text=MessageCur.FromAddress;
			textToAddress.Text=MessageCur.ToAddress;
			textSubject.Text=MessageCur.Subject;
			textBodyText.Text=MessageCur.BodyText;
			textBodyText.Visible=true;
			webBrowser.Visible=false;
			//For all email received types, we disable most of the controls and put the form into a mostly read-only state.
			//There is no reason a user should ever edit a received message.
			//The user can copy the content and send a new email if needed (perhaps we will have forward capabilities in the future).
			if(MessageCur.SentOrReceived==EmailSentOrReceived.ReceivedEncrypted ||
				MessageCur.SentOrReceived==EmailSentOrReceived.ReceivedDirect ||
				MessageCur.SentOrReceived==EmailSentOrReceived.ReadDirect ||
				MessageCur.SentOrReceived==EmailSentOrReceived.Received ||
				MessageCur.SentOrReceived==EmailSentOrReceived.Read ||
				MessageCur.SentOrReceived==EmailSentOrReceived.WebMailReceived ||
				MessageCur.SentOrReceived==EmailSentOrReceived.WebMailRecdRead)
			{
				//If an html body is received, then we display the body using a webbrowser control, so the user sees the message formatted as intended.
				if(MessageCur.BodyText.ToLower().Contains("<html")) {
					textBodyText.Visible=false;
					_isLoading=true;
					webBrowser.DocumentText=MessageCur.BodyText;
					webBrowser.Location=textBodyText.Location;
					webBrowser.Size=textBodyText.Size;
					webBrowser.Anchor=textBodyText.Anchor;
					webBrowser.Visible=true;
				}
			}
			FillAttachments();
			textBodyText.Select();
		}

		private void webBrowser_Navigating(object sender,WebBrowserNavigatingEventArgs e) {
			if(_isLoading) {
				return;
			}
			e.Cancel=true;//Cancel browser navigation (for links clicked within the email message).
			Process.Start(e.Url.ToString());//Instead launch the URL into a new default browser window.
		}

		private void webBrowser_Navigated(object sender,WebBrowserNavigatedEventArgs e) {
			_isLoading=false;
		}

		private void SetSig(X509Certificate2 certSig) {
			_certSig=certSig;
			labelSignedBy.Visible=false;
			textSignedBy.Visible=false;
			textSignedBy.Text="";
			butSig.Visible=false;
			if(certSig!=null) {
				labelSignedBy.Visible=true;
				textSignedBy.Visible=true;
				textSignedBy.Text=EmailMessages.GetSubjectEmailNameFromSignature(certSig);
				butSig.Visible=true;
			}
		}

		#region Attachments

		public void FillAttachments() {
			_listEmailAttachDisplayed=new List<EmailAttach>();
			if(!_isComposing) {
				SetSig(null);
			}
			gridAttachments.BeginUpdate();
			gridAttachments.Rows.Clear();
			gridAttachments.Columns.Clear();
			gridAttachments.Columns.Add(new OpenDental.UI.ODGridColumn("",0));//No name column, since there is only one column.
			for(int i=0;i<MessageCur.Attachments.Count;i++) {
				if(MessageCur.Attachments[i].DisplayedFileName.ToLower()=="smime.p7s") {
					if(!_isComposing) {
						string smimeP7sFilePath=ODFileUtils.CombinePaths(EmailMessages.GetEmailAttachPath(),MessageCur.Attachments[i].ActualFileName);
						SetSig(EmailMessages.GetEmailSignatureFromSmimeP7sFile(smimeP7sFilePath));
					}
					//Do not display email signatures in the attachment list, because "smime.p7s" has no meaning to a user
					//Also, Windows will install the smime.p7s into an useless place in the Windows certificate store.
					continue;
				}
				OpenDental.UI.ODGridRow row=new UI.ODGridRow();
				row.Cells.Add(MessageCur.Attachments[i].DisplayedFileName);
				gridAttachments.Rows.Add(row);
				_listEmailAttachDisplayed.Add(MessageCur.Attachments[i]);
			}
			gridAttachments.EndUpdate();
			if(gridAttachments.Rows.Count>0) {
				gridAttachments.SetSelected(0,true);
			}
		}

		private void contextMenuAttachments_Popup(object sender,EventArgs e) {
			menuItemOpen.Enabled=false;
			menuItemRename.Enabled=false;
			menuItemRemove.Enabled=false;
			if(gridAttachments.SelectedIndices.Length>0) {
				menuItemOpen.Enabled=true;
			}
			if(gridAttachments.SelectedIndices.Length>0 && _isComposing) {
				menuItemRename.Enabled=true;
				menuItemRemove.Enabled=true;
			}
		}

		private void menuItemOpen_Click(object sender,EventArgs e) {
			OpenFile();
		}

		private void menuItemRename_Click(object sender,EventArgs e) {
			InputBox input=new InputBox(Lan.g(this,"Filename"));
			EmailAttach emailAttach=_listEmailAttachDisplayed[gridAttachments.SelectedIndices[0]];
			input.textResult.Text=emailAttach.DisplayedFileName;
			input.ShowDialog();
			if(input.DialogResult!=DialogResult.OK) {
				return;
			}
			emailAttach.DisplayedFileName=input.textResult.Text;
			FillAttachments();
		}

		private void menuItemRemove_Click(object sender,EventArgs e) {
			EmailAttach emailAttach=_listEmailAttachDisplayed[gridAttachments.SelectedIndices[0]];
			for(int i=0;i<MessageCur.Attachments.Count;i++) {
				if(MessageCur.Attachments[i].EmailAttachNum==emailAttach.EmailAttachNum) {
					MessageCur.Attachments.RemoveAt(i);
					break;
				}
			}
			FillAttachments();
		}

		private void gridAttachments_MouseDown(object sender,MouseEventArgs e) {
			//A right click also needs to select an items so that the context menu will work properly.
			if(e.Button==MouseButtons.Right) {
				int clickedIndex=gridAttachments.PointToRow(e.Y);
				if(clickedIndex!=-1) {
					gridAttachments.SetSelected(clickedIndex,true);
				}
			}
		}

		private void gridAttachments_CellDoubleClick(object sender,UI.ODGridClickEventArgs e) {
			OpenFile();
		}

		private void OpenFile() {
			EmailAttach emailAttach=_listEmailAttachDisplayed[gridAttachments.SelectedIndices[0]];
			string strFilePathAttach=ODFileUtils.CombinePaths(EmailMessages.GetEmailAttachPath(),emailAttach.ActualFileName);
			try {
				if(EhrCCD.IsCcdEmailAttachment(emailAttach)) {
					string strTextXml=File.ReadAllText(strFilePathAttach);
					if(EhrCCD.IsCCD(strTextXml)) {
						Patient patEmail=null;//Will be null for most email messages.
						if(MessageCur.SentOrReceived==EmailSentOrReceived.ReadDirect || MessageCur.SentOrReceived==EmailSentOrReceived.ReceivedDirect) {
							patEmail=_patCur;//Only allow reconcile if received via Direct.
						}
						string strAlterateFilPathXslCCD="";
						//Try to find a corresponding stylesheet. This will only be used in the event that the default stylesheet cannot be loaded from the EHR dll.
						for(int i=0;i<_listEmailAttachDisplayed.Count;i++) {
							if(Path.GetExtension(_listEmailAttachDisplayed[i].ActualFileName).ToLower()==".xsl") {
								strAlterateFilPathXslCCD=ODFileUtils.CombinePaths(EmailMessages.GetEmailAttachPath(),_listEmailAttachDisplayed[i].ActualFileName);
								break;
							}
						}
						FormEhrSummaryOfCare.DisplayCCD(strTextXml,patEmail,strAlterateFilPathXslCCD);
						return;
					}
				}
				else if(IsORU_R01message(strFilePathAttach)) {
					if(DataConnection.DBtype==DatabaseType.Oracle) {
						MsgBox.Show(this,"Labs not supported with Oracle.  Opening raw file instead.");
					}
					else {
						FormEhrLabOrderImport FormELOI =new FormEhrLabOrderImport();
						FormELOI.Hl7LabMessage=File.ReadAllText(strFilePathAttach);
						FormELOI.ShowDialog();
						return;
					}
				}
				//We have to create a copy of the file because the name is different.
				//There is also a high probability that the attachment no longer exists if
				//the A to Z folders are disabled, since the file will have originally been
				//placed in the temporary directory.
				string tempFile=ODFileUtils.CombinePaths(PrefL.GetTempFolderPath(),emailAttach.DisplayedFileName);
				File.Copy(strFilePathAttach,tempFile,true);
				Process.Start(tempFile);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void butAttach_Click(object sender,EventArgs e) {
			OpenFileDialog dlg=new OpenFileDialog();
			dlg.Multiselect=true;
			if(_patCur.ImageFolder!="") {
				if(PrefC.AtoZfolderUsed) {
					dlg.InitialDirectory=ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(),
						_patCur.ImageFolder.Substring(0,1).ToUpper(),
						_patCur.ImageFolder);
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
			string attachPath=EmailMessages.GetEmailAttachPath();
			try {
				for(int i=0;i<dlg.FileNames.Length;i++) {
					//copy the file
					newName=DateTime.Now.ToString("yyyyMMdd")+"_"+DateTime.Now.TimeOfDay.Ticks.ToString()+rnd.Next(1000).ToString()+Path.GetExtension(dlg.FileNames[i]);
					File.Copy(dlg.FileNames[i],ODFileUtils.CombinePaths(attachPath,newName));
					//create the attachment
					attach=new EmailAttach();
					attach.DisplayedFileName=Path.GetFileName(dlg.FileNames[i]);
					attach.ActualFileName=newName;
					MessageCur.Attachments.Add(attach);
				}
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
			FillAttachments();
		}

		///<summary>Attempts to parse message and detects if it is an ORU_R01 HL7 message.  Returns false if it fails, or does not detect message type.</summary>
		private bool IsORU_R01message(string strFilePathAttach) {
			try {
				string[] ArrayMSHFields=File.ReadAllText(strFilePathAttach).Split(new string[] { "\r\n" },StringSplitOptions.RemoveEmptyEntries)[0].Split('|');
				if(ArrayMSHFields[8]!="ORU^R01^ORU_R01") {
					return false;
				}
			}
			catch(Exception ex) {
				return false;
			}
			return true;
		}

		#endregion Attachments

		private void textFromAddress_KeyUp(object sender,KeyEventArgs e) {
			if(!_isComposing || !_isSigningEnabled) {
				return;
			}
			SetSig(EmailMessages.GetCertFromPrivateStore(textFromAddress.Text));
		}

		private void textFromAddress_Leave(object sender,EventArgs e) {
			if(!_isComposing || !_isSigningEnabled) {
				return;
			}
			SetSig(EmailMessages.GetCertFromPrivateStore(textFromAddress.Text));
		}

		private void butSig_Click(object sender,EventArgs e) {
			FormEmailDigitalSignature form=new FormEmailDigitalSignature(_certSig);
			if(form.ShowDialog()==DialogResult.OK) {
				//If the user just added trust, then refresh to pull the newly added certificate into the memory cache.
				EmailMessages.RefreshCertStoreExternal(GetEmailAddress());
			}
		}		
		
		private void textBodyText_TextChanged(object sender,EventArgs e) {
			_isMessageChanged=true;
		}

		///<summary>Saves the UI input values into the emailMessage.  Allowed to save message with invalid fields, so no validation here.</summary>
		public void SaveMsg(EmailMessage emailMessage) {
			emailMessage.FromAddress=textFromAddress.Text;
			emailMessage.ToAddress=textToAddress.Text;
			emailMessage.Subject=textSubject.Text;
			emailMessage.BodyText=textBodyText.Text;
			emailMessage.MsgDateTime=DateTime.Now;
			emailMessage.SentOrReceived=MessageCur.SentOrReceived;//Status does not ever change.
		}

	}
}
