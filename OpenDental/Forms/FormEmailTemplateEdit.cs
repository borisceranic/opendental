using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormEmailTemplateEdit : System.Windows.Forms.Form{
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label2;
		private OpenDental.ODtextBox textBodyText;
		///<summary></summary>
		public bool IsNew;
		private Label label1;
		private Label label3;
		private UI.Button butBodyFields;
		private ODtextBox textSubject;
		private ODtextBox textDescription;
		///<summary></summary>
		public EmailTemplate ETcur;
		private UI.Button butSubjectFields;

		///<summary></summary>
		public FormEmailTemplateEdit()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmailTemplateEdit));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.textBodyText = new OpenDental.ODtextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.butBodyFields = new OpenDental.UI.Button();
			this.textSubject = new OpenDental.ODtextBox();
			this.textDescription = new OpenDental.ODtextBox();
			this.butSubjectFields = new OpenDental.UI.Button();
			this.SuspendLayout();
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
			this.butCancel.Location = new System.Drawing.Point(883, 656);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 25);
			this.butCancel.TabIndex = 6;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(802, 656);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 25);
			this.butOK.TabIndex = 5;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 33);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 20);
			this.label2.TabIndex = 0;
			this.label2.Text = "Subject";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textBodyText
			// 
			this.textBodyText.AcceptsTab = true;
			this.textBodyText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBodyText.DetectUrls = false;
			this.textBodyText.Location = new System.Drawing.Point(97, 54);
			this.textBodyText.Name = "textBodyText";
			this.textBodyText.QuickPasteType = OpenDentBusiness.QuickPasteType.Email;
			this.textBodyText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textBodyText.Size = new System.Drawing.Size(780, 598);
			this.textBodyText.TabIndex = 3;
			this.textBodyText.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 54);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Body";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 12);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 20);
			this.label3.TabIndex = 0;
			this.label3.Text = "Description";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butBodyFields
			// 
			this.butBodyFields.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBodyFields.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butBodyFields.Autosize = true;
			this.butBodyFields.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBodyFields.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBodyFields.CornerRadius = 4F;
			this.butBodyFields.Location = new System.Drawing.Point(883, 56);
			this.butBodyFields.Name = "butBodyFields";
			this.butBodyFields.Size = new System.Drawing.Size(82, 20);
			this.butBodyFields.TabIndex = 4;
			this.butBodyFields.Text = "Body Fields";
			this.butBodyFields.Click += new System.EventHandler(this.butBodyFields_Click);
			// 
			// textSubject
			// 
			this.textSubject.AcceptsTab = true;
			this.textSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textSubject.DetectUrls = false;
			this.textSubject.Location = new System.Drawing.Point(97, 33);
			this.textSubject.Multiline = false;
			this.textSubject.Name = "textSubject";
			this.textSubject.QuickPasteType = OpenDentBusiness.QuickPasteType.None;
			this.textSubject.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textSubject.Size = new System.Drawing.Size(780, 20);
			this.textSubject.TabIndex = 2;
			this.textSubject.Text = "";
			// 
			// textDescription
			// 
			this.textDescription.AcceptsTab = true;
			this.textDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textDescription.DetectUrls = false;
			this.textDescription.Location = new System.Drawing.Point(97, 12);
			this.textDescription.Multiline = false;
			this.textDescription.Name = "textDescription";
			this.textDescription.QuickPasteType = OpenDentBusiness.QuickPasteType.None;
			this.textDescription.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textDescription.Size = new System.Drawing.Size(780, 20);
			this.textDescription.TabIndex = 1;
			this.textDescription.Text = "";
			// 
			// butSubjectFields
			// 
			this.butSubjectFields.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSubjectFields.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butSubjectFields.Autosize = true;
			this.butSubjectFields.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSubjectFields.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSubjectFields.CornerRadius = 4F;
			this.butSubjectFields.Location = new System.Drawing.Point(883, 33);
			this.butSubjectFields.Name = "butSubjectFields";
			this.butSubjectFields.Size = new System.Drawing.Size(82, 20);
			this.butSubjectFields.TabIndex = 7;
			this.butSubjectFields.Text = "Subject Fields";
			this.butSubjectFields.Click += new System.EventHandler(this.butSubjectFields_Click);
			// 
			// FormEmailTemplateEdit
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(974, 695);
			this.Controls.Add(this.butSubjectFields);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(this.textSubject);
			this.Controls.Add(this.butBodyFields);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBodyText);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(933, 200);
			this.Name = "FormEmailTemplateEdit";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Email Template";
			this.Load += new System.EventHandler(this.FormEmailTemplateEdit_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormEmailTemplateEdit_Load(object sender, System.EventArgs e) {
			textSubject.Text=ETcur.Subject;
			textBodyText.Text=ETcur.BodyText;
			textDescription.Text=ETcur.Description;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textSubject.Text=="" && textBodyText.Text==""){
				MsgBox.Show(this,"Both the subject and body of the template cannot be left blank.");
				return;
			}
			if(textDescription.Text==""){
				MsgBox.Show(this,"Please enter a description.");
				return;
			}
			ETcur.Subject=textSubject.Text;
			ETcur.BodyText=textBodyText.Text;
			ETcur.Description=textDescription.Text;
			if(IsNew){
				EmailTemplates.Insert(ETcur);
			}
			else{
				EmailTemplates.Update(ETcur);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void butSubjectFields_Click(object sender,EventArgs e) {
			FormMessageReplacements FormMR=new FormMessageReplacements(
				MessageReplaceType.Appointment | MessageReplaceType.Office | MessageReplaceType.Patient | MessageReplaceType.User);
			FormMR.IsSelectionMode=true;
			FormMR.ShowDialog();
			textSubject.SelectedText=FormMR.Replacement;
		}

		private void butBodyFields_Click(object sender,EventArgs e) {
			FormMessageReplacements FormMR=new FormMessageReplacements(
				MessageReplaceType.Appointment | MessageReplaceType.Office | MessageReplaceType.Patient | MessageReplaceType.User);
			FormMR.IsSelectionMode=true;
			FormMR.ShowDialog();
			textBodyText.SelectedText=FormMR.Replacement;
		}
	}
}





















