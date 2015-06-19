using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental{
	public class FormEmailMessageEdit : System.Windows.Forms.Form {
		private OpenDental.UI.Button butCancel;
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.Button butSend;
		private OpenDental.UI.Button butDeleteTemplate;
		private OpenDental.UI.Button butAddTemplate;
		private System.Windows.Forms.Label labelTemplate;
		private System.Windows.Forms.ListBox listTemplates;
		private OpenDental.UI.Button butInsertTemplate;
		private UI.Button butRefresh;
		public bool IsNew;
		private bool _hasTemplatesChanged;
		private System.Windows.Forms.Panel panelTemplates;
		private OpenDental.UI.Button butSave;
		private OpenDental.UI.Button butDelete;
		private OpenDental.UI.Button buttonFuchsMailDSF;
		private OpenDental.UI.Button buttonFuchsMailDMF;
		private EmailMessage _emailMessage;
		private Label labelDecrypt;
		private UI.Button butDecrypt;
		private UI.Button butDirectMessage;
		private UI.Button butRawMessage;
		private EmailPreviewControl emailPreview;

		///<summary></summary>
		public FormEmailMessageEdit(EmailMessage emailMessage){
			InitializeComponent();// Required for Windows Form Designer support
			Lan.F(this);
			_emailMessage=emailMessage.Copy();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmailMessageEdit));
			this.labelTemplate = new System.Windows.Forms.Label();
			this.listTemplates = new System.Windows.Forms.ListBox();
			this.panelTemplates = new System.Windows.Forms.Panel();
			this.butInsertTemplate = new OpenDental.UI.Button();
			this.butDeleteTemplate = new OpenDental.UI.Button();
			this.butAddTemplate = new OpenDental.UI.Button();
			this.labelDecrypt = new System.Windows.Forms.Label();
			this.butRefresh = new OpenDental.UI.Button();
			this.butRawMessage = new OpenDental.UI.Button();
			this.butDirectMessage = new OpenDental.UI.Button();
			this.butDecrypt = new OpenDental.UI.Button();
			this.buttonFuchsMailDMF = new OpenDental.UI.Button();
			this.buttonFuchsMailDSF = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.butSave = new OpenDental.UI.Button();
			this.butSend = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.emailPreview = new OpenDental.EmailPreviewControl();
			this.panelTemplates.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelTemplate
			// 
			this.labelTemplate.Location = new System.Drawing.Point(8, 7);
			this.labelTemplate.Name = "labelTemplate";
			this.labelTemplate.Size = new System.Drawing.Size(166, 14);
			this.labelTemplate.TabIndex = 18;
			this.labelTemplate.Text = "E-mail Template";
			this.labelTemplate.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listTemplates
			// 
			this.listTemplates.Location = new System.Drawing.Point(10, 26);
			this.listTemplates.Name = "listTemplates";
			this.listTemplates.Size = new System.Drawing.Size(164, 277);
			this.listTemplates.TabIndex = 0;
			this.listTemplates.TabStop = false;
			this.listTemplates.DoubleClick += new System.EventHandler(this.listTemplates_DoubleClick);
			// 
			// panelTemplates
			// 
			this.panelTemplates.Controls.Add(this.butInsertTemplate);
			this.panelTemplates.Controls.Add(this.butDeleteTemplate);
			this.panelTemplates.Controls.Add(this.butAddTemplate);
			this.panelTemplates.Controls.Add(this.labelTemplate);
			this.panelTemplates.Controls.Add(this.listTemplates);
			this.panelTemplates.Location = new System.Drawing.Point(8, 9);
			this.panelTemplates.Name = "panelTemplates";
			this.panelTemplates.Size = new System.Drawing.Size(180, 370);
			this.panelTemplates.TabIndex = 0;
			// 
			// butInsertTemplate
			// 
			this.butInsertTemplate.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butInsertTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butInsertTemplate.Autosize = true;
			this.butInsertTemplate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butInsertTemplate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butInsertTemplate.CornerRadius = 4F;
			this.butInsertTemplate.Image = global::OpenDental.Properties.Resources.Right;
			this.butInsertTemplate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butInsertTemplate.Location = new System.Drawing.Point(102, 305);
			this.butInsertTemplate.Name = "butInsertTemplate";
			this.butInsertTemplate.Size = new System.Drawing.Size(74, 26);
			this.butInsertTemplate.TabIndex = 2;
			this.butInsertTemplate.Text = "Insert";
			this.butInsertTemplate.Click += new System.EventHandler(this.butInsertTemmplate_Click);
			// 
			// butDeleteTemplate
			// 
			this.butDeleteTemplate.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDeleteTemplate.Autosize = true;
			this.butDeleteTemplate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDeleteTemplate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDeleteTemplate.CornerRadius = 4F;
			this.butDeleteTemplate.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDeleteTemplate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDeleteTemplate.Location = new System.Drawing.Point(7, 339);
			this.butDeleteTemplate.Name = "butDeleteTemplate";
			this.butDeleteTemplate.Size = new System.Drawing.Size(75, 26);
			this.butDeleteTemplate.TabIndex = 3;
			this.butDeleteTemplate.Text = "Delete";
			this.butDeleteTemplate.Click += new System.EventHandler(this.butDeleteTemplate_Click);
			// 
			// butAddTemplate
			// 
			this.butAddTemplate.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddTemplate.Autosize = true;
			this.butAddTemplate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddTemplate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddTemplate.CornerRadius = 4F;
			this.butAddTemplate.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddTemplate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddTemplate.Location = new System.Drawing.Point(7, 305);
			this.butAddTemplate.Name = "butAddTemplate";
			this.butAddTemplate.Size = new System.Drawing.Size(75, 26);
			this.butAddTemplate.TabIndex = 1;
			this.butAddTemplate.Text = "&Add";
			this.butAddTemplate.Click += new System.EventHandler(this.butAddTemplate_Click);
			// 
			// labelDecrypt
			// 
			this.labelDecrypt.Location = new System.Drawing.Point(5, 401);
			this.labelDecrypt.Name = "labelDecrypt";
			this.labelDecrypt.Size = new System.Drawing.Size(267, 59);
			this.labelDecrypt.TabIndex = 31;
			this.labelDecrypt.Text = "Previous attempts to decrypt this message have failed.\r\nDecryption usually fails " +
    "when your private decryption key is not installed on the local computer.\r\nUse th" +
    "e Decrypt button to try again.";
			this.labelDecrypt.Visible = false;
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(356, 659);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 25);
			this.butRefresh.TabIndex = 37;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.UseVisualStyleBackColor = true;
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// butRawMessage
			// 
			this.butRawMessage.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRawMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butRawMessage.Autosize = true;
			this.butRawMessage.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRawMessage.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRawMessage.CornerRadius = 4F;
			this.butRawMessage.Location = new System.Drawing.Point(138, 659);
			this.butRawMessage.Name = "butRawMessage";
			this.butRawMessage.Size = new System.Drawing.Size(89, 26);
			this.butRawMessage.TabIndex = 36;
			this.butRawMessage.Text = "Raw Message";
			this.butRawMessage.Visible = false;
			this.butRawMessage.Click += new System.EventHandler(this.butRawMessage_Click);
			// 
			// butDirectMessage
			// 
			this.butDirectMessage.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDirectMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDirectMessage.Autosize = true;
			this.butDirectMessage.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDirectMessage.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDirectMessage.CornerRadius = 4F;
			this.butDirectMessage.Location = new System.Drawing.Point(692, 659);
			this.butDirectMessage.Name = "butDirectMessage";
			this.butDirectMessage.Size = new System.Drawing.Size(106, 25);
			this.butDirectMessage.TabIndex = 8;
			this.butDirectMessage.Text = "Direct Message";
			this.butDirectMessage.Visible = false;
			this.butDirectMessage.Click += new System.EventHandler(this.butDirectMessage_Click);
			// 
			// butDecrypt
			// 
			this.butDecrypt.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDecrypt.Autosize = true;
			this.butDecrypt.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDecrypt.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDecrypt.CornerRadius = 4F;
			this.butDecrypt.Location = new System.Drawing.Point(8, 463);
			this.butDecrypt.Name = "butDecrypt";
			this.butDecrypt.Size = new System.Drawing.Size(75, 25);
			this.butDecrypt.TabIndex = 7;
			this.butDecrypt.Text = "Decrypt";
			this.butDecrypt.Visible = false;
			this.butDecrypt.Click += new System.EventHandler(this.butDecrypt_Click);
			// 
			// buttonFuchsMailDMF
			// 
			this.buttonFuchsMailDMF.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.buttonFuchsMailDMF.Autosize = true;
			this.buttonFuchsMailDMF.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.buttonFuchsMailDMF.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.buttonFuchsMailDMF.CornerRadius = 4F;
			this.buttonFuchsMailDMF.Location = new System.Drawing.Point(197, 177);
			this.buttonFuchsMailDMF.Name = "buttonFuchsMailDMF";
			this.buttonFuchsMailDMF.Size = new System.Drawing.Size(78, 22);
			this.buttonFuchsMailDMF.TabIndex = 30;
			this.buttonFuchsMailDMF.Text = "To DMF";
			this.buttonFuchsMailDMF.Visible = false;
			this.buttonFuchsMailDMF.Click += new System.EventHandler(this.buttonFuchsMailDMF_Click);
			// 
			// buttonFuchsMailDSF
			// 
			this.buttonFuchsMailDSF.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.buttonFuchsMailDSF.Autosize = true;
			this.buttonFuchsMailDSF.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.buttonFuchsMailDSF.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.buttonFuchsMailDSF.CornerRadius = 4F;
			this.buttonFuchsMailDSF.Location = new System.Drawing.Point(197, 149);
			this.buttonFuchsMailDSF.Name = "buttonFuchsMailDSF";
			this.buttonFuchsMailDSF.Size = new System.Drawing.Size(78, 22);
			this.buttonFuchsMailDSF.TabIndex = 29;
			this.buttonFuchsMailDSF.Text = "To DSF";
			this.buttonFuchsMailDSF.Visible = false;
			this.buttonFuchsMailDSF.Click += new System.EventHandler(this.buttonFuchsMailDSF_Click);
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(8, 659);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(75, 26);
			this.butDelete.TabIndex = 11;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butSave
			// 
			this.butSave.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butSave.Autosize = true;
			this.butSave.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSave.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSave.CornerRadius = 4F;
			this.butSave.Location = new System.Drawing.Point(278, 659);
			this.butSave.Name = "butSave";
			this.butSave.Size = new System.Drawing.Size(75, 25);
			this.butSave.TabIndex = 6;
			this.butSave.Text = "Save";
			this.butSave.Click += new System.EventHandler(this.butSave_Click);
			// 
			// butSend
			// 
			this.butSend.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSend.Autosize = true;
			this.butSend.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSend.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSend.CornerRadius = 4F;
			this.butSend.Location = new System.Drawing.Point(804, 659);
			this.butSend.Name = "butSend";
			this.butSend.Size = new System.Drawing.Size(75, 25);
			this.butSend.TabIndex = 9;
			this.butSend.Text = "&Send";
			this.butSend.Click += new System.EventHandler(this.butSend_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(885, 659);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 25);
			this.butCancel.TabIndex = 10;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// emailPreview
			// 
			this.emailPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.emailPreview.BodyText = "";
			this.emailPreview.Location = new System.Drawing.Point(189, 1);
			this.emailPreview.Name = "emailPreview";
			this.emailPreview.Size = new System.Drawing.Size(771, 652);
			this.emailPreview.Subject = "";
			this.emailPreview.TabIndex = 38;
			this.emailPreview.ToAddress = "";
			// 
			// FormEmailMessageEdit
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(974, 696);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.butRawMessage);
			this.Controls.Add(this.butDirectMessage);
			this.Controls.Add(this.butDecrypt);
			this.Controls.Add(this.labelDecrypt);
			this.Controls.Add(this.buttonFuchsMailDMF);
			this.Controls.Add(this.buttonFuchsMailDSF);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butSave);
			this.Controls.Add(this.panelTemplates);
			this.Controls.Add(this.butSend);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.emailPreview);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(875, 575);
			this.Name = "FormEmailMessageEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Email Message";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormEmailMessageEdit_Closing);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEmailMessageEdit_FormClosing);
			this.Load += new System.EventHandler(this.FormEmailMessageEdit_Load);
			this.panelTemplates.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormEmailMessageEdit_Load(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup,true)) {
				butAddTemplate.Enabled=false;
				butDeleteTemplate.Enabled=false;
			}
			if(PrefC.GetBool(PrefName.ShowFeatureEhr)) {
				butDirectMessage.Visible=true;
			}
			Cursor=Cursors.WaitCursor;
			RefreshAll();
			Cursor=Cursors.Default;
		}

		private void RefreshAll() {
			emailPreview.LoadEmailMessage(_emailMessage);
			if(!emailPreview.IsComposing) {
				panelTemplates.Visible=false;
				butDirectMessage.Enabled=false;//not allowed to send again.
				butSend.Visible=false;//not allowed to send again.
				butSave.Visible=false;//not allowed to save changes.
				//When opening an email from FormEmailInbox, the email status will change to read automatically,
				//and changing the text on the cancel button helps convey that to the user.
				butCancel.Text="Close";
			}
			FillTemplates();
			if(PrefC.GetBool(PrefName.FuchsOptionsOn)) {
				buttonFuchsMailDMF.Visible=true;
				buttonFuchsMailDSF.Visible=true;
			}
			butRefresh.Visible=false;
			//For all email received types, we disable most of the controls and put the form into a mostly read-only state.
			//There is no reason a user should ever edit a received message. The user can copy the content and send a new email if needed (perhaps we will have forward capabilities in the future).
			if(_emailMessage.SentOrReceived==EmailSentOrReceived.ReceivedEncrypted ||
				_emailMessage.SentOrReceived==EmailSentOrReceived.ReceivedDirect ||
				_emailMessage.SentOrReceived==EmailSentOrReceived.ReadDirect ||
				_emailMessage.SentOrReceived==EmailSentOrReceived.Received ||
				_emailMessage.SentOrReceived==EmailSentOrReceived.Read ||
				_emailMessage.SentOrReceived==EmailSentOrReceived.WebMailReceived ||
				_emailMessage.SentOrReceived==EmailSentOrReceived.WebMailRecdRead)
			{
				butRefresh.Visible=true;
				butRawMessage.Visible=true;
			}
			labelDecrypt.Visible=false;
			butDecrypt.Visible=false;
			if(_emailMessage.SentOrReceived==EmailSentOrReceived.ReceivedEncrypted) {
				labelDecrypt.Visible=true;
				butDecrypt.Visible=true;
				butRefresh.Visible=false;
			}
		}

		#region Templates

		private void FillTemplates() {
			listTemplates.Items.Clear();
			for(int i=0;i<EmailTemplates.List.Length;i++) {
				listTemplates.Items.Add(EmailTemplates.List[i].Subject);
			}
		}

		private void listTemplates_DoubleClick(object sender, System.EventArgs e) {
			if(listTemplates.SelectedIndex==-1){
				return;
			}
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormEmailTemplateEdit FormE=new FormEmailTemplateEdit();
			FormE.ETcur=EmailTemplates.List[listTemplates.SelectedIndex];
			FormE.ShowDialog();
			if(FormE.DialogResult!=DialogResult.OK){
				return;
			}
			EmailTemplates.RefreshCache();
			_hasTemplatesChanged=true;
			FillTemplates();
		}

		private void butAddTemplate_Click(object sender, System.EventArgs e) {
			FormEmailTemplateEdit FormE=new FormEmailTemplateEdit();
			FormE.IsNew=true;
			FormE.ETcur=new EmailTemplate();
			FormE.ShowDialog();
			if(FormE.DialogResult!=DialogResult.OK){
				return;
			}
			EmailTemplates.RefreshCache();
			_hasTemplatesChanged=true;
			FillTemplates();
		}

		private void butDeleteTemplate_Click(object sender, System.EventArgs e) {
			if(listTemplates.SelectedIndex==-1){
				MessageBox.Show(Lan.g(this,"Please select an item first."));
				return;
			}
			if(MessageBox.Show(Lan.g(this,"Delete e-mail template?"),"",MessageBoxButtons.OKCancel)
				!=DialogResult.OK){
				return;
			}
			EmailTemplates.Delete(EmailTemplates.List[listTemplates.SelectedIndex]);
			EmailTemplates.RefreshCache();
			_hasTemplatesChanged=true;
			FillTemplates();
		}

		private void butInsertTemmplate_Click(object sender, System.EventArgs e) {
			if(listTemplates.SelectedIndex==-1){
				MessageBox.Show(Lan.g(this,"Please select an item first."));
				return;
			}
			if(emailPreview.HasMessageChanged){
				if(MessageBox.Show(Lan.g(this,"Replace exising e-mail text with text from the template?"),"",MessageBoxButtons.OKCancel)
					!=DialogResult.OK){
					return;
				}
			}
			emailPreview.LoadTemplate(EmailTemplates.List[listTemplates.SelectedIndex].Subject,EmailTemplates.List[listTemplates.SelectedIndex].BodyText);
		}

		///<summary>Hard coded template.</summary>
		private void buttonFuchsMailDSF_Click(object sender,EventArgs e) {
			emailPreview.ToAddress="skimom@springfielddental.net";
			emailPreview.LoadTemplate("Statement to DSF","For accounting, sent statement to skimom@springfielddental.net"+emailPreview.BodyText);
		}

		///<summary>Hard coded template.</summary>
		private void buttonFuchsMailDMF_Click(object sender,EventArgs e) {
			emailPreview.ToAddress="smilecouple@yahoo.com";
			emailPreview.LoadTemplate("Statement to DMF","For accounting, sent statement to smilecouple@yahoo.com"+emailPreview.BodyText);
		}

		#endregion Templates

		private void butDecrypt_Click(object sender,EventArgs e) {
			if(!EmailMessages.IsAddressTrusted(_emailMessage.FromAddress)) {//Not trusted yet.
				string strTrustMessage=Lan.g(this,"The sender address must be added to your trusted addresses before you can decrypt the email")
					+". "+Lan.g(this,"Add")+" "+_emailMessage.FromAddress+" "+Lan.g(this,"to trusted addresses")+"?";
				if(MessageBox.Show(strTrustMessage,"",MessageBoxButtons.OKCancel)==DialogResult.OK) {
					Cursor=Cursors.WaitCursor;
					EmailMessages.TryAddTrustDirect(_emailMessage.FromAddress);
					Cursor=Cursors.Default;
					if(!EmailMessages.IsAddressTrusted(_emailMessage.FromAddress)) {
						MsgBox.Show(this,"Failed to trust sender because a valid certificate could not be located.");
						return;
					}
				}
				else {
					MsgBox.Show(this,"Cannot decrypt message from untrusted sender.");
					return;
				}
			}
			Cursor=Cursors.WaitCursor;
			EmailAddress emailAddress=emailPreview.GetEmailAddress();
			try {
				_emailMessage=EmailMessages.ProcessRawEmailMessageIn(_emailMessage.BodyText,_emailMessage.EmailMessageNum,emailAddress,true);//If decryption is successful, sets status to ReceivedDirect.
				//The Direct message was decrypted.
				EmailMessages.UpdateSentOrReceivedRead(_emailMessage);//Mark read, because we are already viewing the message within the current window.					
				RefreshAll();
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"Decryption failed.")+"\r\n"+ex.Message);
				//Error=InvalidEncryption: means that someone used the wrong certificate when sending the email to this inbox, and we tried to decrypt with a different certificate.
				//Error=NoTrustedRecipients: means the sender is not added to the trust anchors in mmc.
			}
			Cursor=Cursors.Default;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(IsNew){
				DialogResult=DialogResult.Cancel;
				//It will be deleted in the FormClosing() Event.
			}
			else{
				if(MsgBox.Show(this,true,"Delete this email?")){
					EmailMessages.Delete(_emailMessage);
					DialogResult=DialogResult.OK;
				}
			}
		}

		private void butRawMessage_Click(object sender,EventArgs e) {
			MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(_emailMessage.RawEmailIn);
			msgbox.ShowDialog();
		}		

		private void butSave_Click(object sender,EventArgs e) {
			//this will not be available if already sent.
			SaveMsg();
			DialogResult=DialogResult.OK;
		}

		private void SaveMsg(){
			//allowed to save message with invalid fields, so no validation here.  Only validate when sending.
			emailPreview.SaveMsg(_emailMessage);
			if(IsNew) {
				EmailMessages.Insert(_emailMessage);
				IsNew=false;//As soon as the message is saved to the database, it is no longer new because it has a primary key.  Prevents new email from being duplicated if saved multiple times.
			}
			else {
				EmailMessages.Update(_emailMessage);
			}
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			List<EmailAttach> listEmailAttachOld=_emailMessage.Attachments;
			Cursor=Cursors.WaitCursor;
			EmailAddress emailAddress=emailPreview.GetEmailAddress();
			try {
				_emailMessage=EmailMessages.ProcessRawEmailMessageIn(_emailMessage.RawEmailIn,_emailMessage.EmailMessageNum,emailAddress,false);
				EmailMessages.UpdateSentOrReceivedRead(_emailMessage);//Mark read, because we are already viewing the message within the current window.
				RefreshAll();
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"Refreshing failed.")+"\r\n"+ex.Message);
				Cursor=Cursors.Default;
				return;
			}
			Cursor=Cursors.Default;
			string attachPath=EmailAttaches.GetAttachPath();
			for(int i=0;i<listEmailAttachOld.Count;i++) {//For each old attachment, attempt to delete.  The new attachments were created with different names in ProcessRawEmailMessage().
				string filePath=ODFileUtils.CombinePaths(attachPath,listEmailAttachOld[i].ActualFileName);
				try {
					File.Delete(filePath);//Deleting old file. If it fails there was no file, or permission problems.
				}
				catch {
					//The file will take a little bit of extra hard drive space, but the file is not referened and cannot hurt anything.
				}
			}
		}

		private void butDirectMessage_Click(object sender,EventArgs e) {
			//this will not be available if already sent.
			if(emailPreview.FromAddress=="" || emailPreview.ToAddress=="") {
				MessageBox.Show("Addresses not allowed to be blank.");
				return;
			}
			EmailAddress emailAddressFrom=emailPreview.GetEmailAddress();
			if(emailPreview.FromAddress!=emailAddressFrom.EmailUsername) {
				//Without this block, encryption would fail with an obscure error message, because the from address would not match the digital signature of the sender.
				MessageBox.Show(Lan.g(this,"From address must match email address username in email setup.")+"\r\n"+Lan.g(this,"From address must be exactly")+" "+emailAddressFrom.EmailUsername);
				return;
			}
			if(emailAddressFrom.SMTPserver=="") {
				MsgBox.Show(this,"The email address in email setup must have an SMTP server.");
				return;
			}
			if(emailPreview.ToAddress.Contains(",")) {
				MsgBox.Show(this,"Multiple recipient addresses not allowed for direct messaging.");
				return;
			}
			Cursor=Cursors.WaitCursor;
			if(!EmailMessages.IsAddressTrusted(emailPreview.ToAddress)) {//Not trusted yet.
				EmailMessages.TryAddTrustDirect(emailPreview.ToAddress);
				if(!EmailMessages.IsAddressTrusted(emailPreview.ToAddress)) {
					Cursor=Cursors.Default;
					MsgBox.Show(this,"Failed to trust recipient because a valid certificate could not be located.");
					return;
				}
			}
			SaveMsg();
			try {
				string strErrors=EmailMessages.SendEmailDirect(_emailMessage,emailAddressFrom);
				if(strErrors!="") {
					Cursor=Cursors.Default;
					MessageBox.Show(strErrors);
					return;
				}
				else {
					_emailMessage.SentOrReceived=EmailSentOrReceived.SentDirect;
					EmailMessages.Update(_emailMessage);
					MsgBox.Show(this,"Sent");
				}
			}
			catch(Exception ex) {
				Cursor=Cursors.Default;
				MsgBoxCopyPaste msgBox=new MsgBoxCopyPaste(ex.Message);
				msgBox.ShowDialog();
				return;
			}
			Cursor=Cursors.Default;
			DialogResult=DialogResult.OK;
		}

		///<summary></summary>
		private void butSend_Click(object sender, System.EventArgs e) {
			//this will not be available if already sent.
			if(emailPreview.FromAddress=="" || emailPreview.ToAddress=="") {
				MsgBox.Show(this,"Addresses not allowed to be blank.");
				return;
			}
			if(EhrCCD.HasCcdEmailAttachment(_emailMessage)) {
				MsgBox.Show(this,"The email has a summary of care attachment which may contain sensitive patient data.  Use the Direct Message button instead.");
				return;
			}
			EmailAddress emailAddress=emailPreview.GetEmailAddress();
			if(emailAddress.SMTPserver==""){
				MsgBox.Show(this,"The email address in email setup must have an SMTP server.");
				return;
			}
			Cursor=Cursors.WaitCursor;
			SaveMsg();
			try{
				if(emailPreview.IsSigned) {
					EmailMessages.SendEmailUnsecureWithSig(_emailMessage,emailAddress,emailPreview.Signature);
				}
				else {
					EmailMessages.SendEmailUnsecure(_emailMessage,emailAddress);
				}
				_emailMessage.SentOrReceived=EmailSentOrReceived.Sent;
				EmailMessages.Update(_emailMessage);
				MsgBox.Show(this,"Sent");
			}
			catch(Exception ex){
				Cursor=Cursors.Default;
				MsgBoxCopyPaste msgBox=new MsgBoxCopyPaste(ex.Message);
				msgBox.ShowDialog();
				return;
			}
			Cursor=Cursors.Default;
			//MessageCur.MsgDateTime=DateTime.Now;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormEmailMessageEdit_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(_hasTemplatesChanged){
				DataValid.SetInvalid(InvalidType.Email);
			}
		}

		private void FormEmailMessageEdit_FormClosing(object sender,FormClosingEventArgs e) {
			if(DialogResult==DialogResult.OK){
				return;
			}
			if(IsNew){
				EmailMessages.Delete(_emailMessage);
			}
		}

	}
}