using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Text.RegularExpressions;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormXchargeSetup : System.Windows.Forms.Form{
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private LinkLabel linkLabel1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private CheckBox checkEnabled;
		private TextBox textPath;
		private Label label3;
		private Label label1;
		private ComboBox comboPaymentType;
		private Program prog;
		private TextBox textPassword;
		private Label labelPassword;
		private TextBox textUser;
		private TextBox textOverride;
		private Label labelOverride;
		private Label labelUser;
		private GroupBox groupXWeb;
		private Label label6;
		private TextBox textAuthKey;
		private Label labelAuthKey;
		private TextBox textXWebID;
		private Label labelXWebID;
		private TextBox textTerminalID;
		private Label labelTerminalID;
		private string pathOverrideOld;

		///<summary></summary>
		public FormXchargeSetup()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormXchargeSetup));
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.checkEnabled = new System.Windows.Forms.CheckBox();
			this.textPath = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.comboPaymentType = new System.Windows.Forms.ComboBox();
			this.textPassword = new System.Windows.Forms.TextBox();
			this.labelPassword = new System.Windows.Forms.Label();
			this.textUser = new System.Windows.Forms.TextBox();
			this.labelUser = new System.Windows.Forms.Label();
			this.textOverride = new System.Windows.Forms.TextBox();
			this.labelOverride = new System.Windows.Forms.Label();
			this.groupXWeb = new System.Windows.Forms.GroupBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textAuthKey = new System.Windows.Forms.TextBox();
			this.labelAuthKey = new System.Windows.Forms.Label();
			this.textXWebID = new System.Windows.Forms.TextBox();
			this.labelXWebID = new System.Windows.Forms.Label();
			this.textTerminalID = new System.Windows.Forms.TextBox();
			this.labelTerminalID = new System.Windows.Forms.Label();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.groupXWeb.SuspendLayout();
			this.SuspendLayout();
			// 
			// linkLabel1
			// 
			this.linkLabel1.LinkArea = new System.Windows.Forms.LinkArea(27, 38);
			this.linkLabel1.Location = new System.Drawing.Point(20, 20);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(425, 16);
			this.linkLabel1.TabIndex = 3;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "The X-Charge website is at http://xchargepayments.com/opendental/";
			this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.linkLabel1.UseCompatibleTextRendering = true;
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// checkEnabled
			// 
			this.checkEnabled.Location = new System.Drawing.Point(20, 50);
			this.checkEnabled.Name = "checkEnabled";
			this.checkEnabled.Size = new System.Drawing.Size(104, 18);
			this.checkEnabled.TabIndex = 4;
			this.checkEnabled.Text = "Enabled";
			this.checkEnabled.UseVisualStyleBackColor = true;
			// 
			// textPath
			// 
			this.textPath.Location = new System.Drawing.Point(19, 188);
			this.textPath.Name = "textPath";
			this.textPath.Size = new System.Drawing.Size(425, 20);
			this.textPath.TabIndex = 7;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(17, 167);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(231, 18);
			this.label3.TabIndex = 50;
			this.label3.Text = "Program Path";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(17, 255);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(154, 16);
			this.label1.TabIndex = 53;
			this.label1.Text = "Payment Type";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// comboPaymentType
			// 
			this.comboPaymentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPaymentType.FormattingEnabled = true;
			this.comboPaymentType.Location = new System.Drawing.Point(20, 274);
			this.comboPaymentType.MaxDropDownItems = 25;
			this.comboPaymentType.Name = "comboPaymentType";
			this.comboPaymentType.Size = new System.Drawing.Size(205, 21);
			this.comboPaymentType.TabIndex = 9;
			// 
			// textPassword
			// 
			this.textPassword.Location = new System.Drawing.Point(18, 144);
			this.textPassword.Name = "textPassword";
			this.textPassword.Size = new System.Drawing.Size(425, 20);
			this.textPassword.TabIndex = 6;
			this.textPassword.UseSystemPasswordChar = true;
			// 
			// labelPassword
			// 
			this.labelPassword.Location = new System.Drawing.Point(16, 123);
			this.labelPassword.Name = "labelPassword";
			this.labelPassword.Size = new System.Drawing.Size(231, 18);
			this.labelPassword.TabIndex = 55;
			this.labelPassword.Text = "Password";
			this.labelPassword.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textUser
			// 
			this.textUser.Location = new System.Drawing.Point(19, 100);
			this.textUser.Name = "textUser";
			this.textUser.Size = new System.Drawing.Size(425, 20);
			this.textUser.TabIndex = 5;
			// 
			// labelUser
			// 
			this.labelUser.Location = new System.Drawing.Point(17, 79);
			this.labelUser.Name = "labelUser";
			this.labelUser.Size = new System.Drawing.Size(231, 18);
			this.labelUser.TabIndex = 57;
			this.labelUser.Text = "User Id";
			this.labelUser.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textOverride
			// 
			this.textOverride.Location = new System.Drawing.Point(19, 232);
			this.textOverride.Name = "textOverride";
			this.textOverride.Size = new System.Drawing.Size(425, 20);
			this.textOverride.TabIndex = 8;
			// 
			// labelOverride
			// 
			this.labelOverride.Location = new System.Drawing.Point(17, 211);
			this.labelOverride.Name = "labelOverride";
			this.labelOverride.Size = new System.Drawing.Size(425, 18);
			this.labelOverride.TabIndex = 59;
			this.labelOverride.Text = "Local path override.  Usually left blank.";
			this.labelOverride.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// groupXWeb
			// 
			this.groupXWeb.Controls.Add(this.textTerminalID);
			this.groupXWeb.Controls.Add(this.labelTerminalID);
			this.groupXWeb.Controls.Add(this.label6);
			this.groupXWeb.Controls.Add(this.textAuthKey);
			this.groupXWeb.Controls.Add(this.labelAuthKey);
			this.groupXWeb.Controls.Add(this.textXWebID);
			this.groupXWeb.Controls.Add(this.labelXWebID);
			this.groupXWeb.Location = new System.Drawing.Point(18, 301);
			this.groupXWeb.Name = "groupXWeb";
			this.groupXWeb.Size = new System.Drawing.Size(427, 193);
			this.groupXWeb.TabIndex = 60;
			this.groupXWeb.TabStop = false;
			this.groupXWeb.Text = "X-Web";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(6, 16);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(413, 39);
			this.label6.TabIndex = 66;
			this.label6.Text = "The following settings are required to enable receiving online payments via the P" +
    "atient Portal.  These settings are provided by X-Charge when you sign up for X-W" +
    "eb.";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textAuthKey
			// 
			this.textAuthKey.Location = new System.Drawing.Point(6, 120);
			this.textAuthKey.Name = "textAuthKey";
			this.textAuthKey.Size = new System.Drawing.Size(413, 20);
			this.textAuthKey.TabIndex = 11;
			this.textAuthKey.TextChanged += new System.EventHandler(this.textAuthKey_TextChanged);
			// 
			// labelAuthKey
			// 
			this.labelAuthKey.Location = new System.Drawing.Point(4, 99);
			this.labelAuthKey.Name = "labelAuthKey";
			this.labelAuthKey.Size = new System.Drawing.Size(413, 18);
			this.labelAuthKey.TabIndex = 65;
			this.labelAuthKey.Text = "Auth Key";
			this.labelAuthKey.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textXWebID
			// 
			this.textXWebID.Location = new System.Drawing.Point(5, 76);
			this.textXWebID.Name = "textXWebID";
			this.textXWebID.Size = new System.Drawing.Size(413, 20);
			this.textXWebID.TabIndex = 10;
			// 
			// labelXWebID
			// 
			this.labelXWebID.Location = new System.Drawing.Point(4, 55);
			this.labelXWebID.Name = "labelXWebID";
			this.labelXWebID.Size = new System.Drawing.Size(231, 18);
			this.labelXWebID.TabIndex = 63;
			this.labelXWebID.Text = "XWeb ID";
			this.labelXWebID.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textTerminalID
			// 
			this.textTerminalID.Location = new System.Drawing.Point(6, 164);
			this.textTerminalID.Name = "textTerminalID";
			this.textTerminalID.Size = new System.Drawing.Size(413, 20);
			this.textTerminalID.TabIndex = 12;
			// 
			// labelTerminalID
			// 
			this.labelTerminalID.Location = new System.Drawing.Point(4, 143);
			this.labelTerminalID.Name = "labelTerminalID";
			this.labelTerminalID.Size = new System.Drawing.Size(413, 18);
			this.labelTerminalID.TabIndex = 68;
			this.labelTerminalID.Text = "Terminal ID";
			this.labelTerminalID.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(273, 506);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 13;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(370, 506);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 14;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormXchargeSetup
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(480, 544);
			this.Controls.Add(this.groupXWeb);
			this.Controls.Add(this.textOverride);
			this.Controls.Add(this.labelOverride);
			this.Controls.Add(this.textUser);
			this.Controls.Add(this.labelUser);
			this.Controls.Add(this.textPassword);
			this.Controls.Add(this.labelPassword);
			this.Controls.Add(this.comboPaymentType);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textPath);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.checkEnabled);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormXchargeSetup";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "X-Charge Setup";
			this.Load += new System.EventHandler(this.FormXchargeSetup_Load);
			this.groupXWeb.ResumeLayout(false);
			this.groupXWeb.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormXchargeSetup_Load(object sender,EventArgs e) {
			prog=Programs.GetCur(ProgramName.Xcharge);
			if(prog==null){
				return;
			}
			checkEnabled.Checked=prog.Enabled;
			textPath.Text=prog.Path;
			pathOverrideOld=ProgramProperties.GetLocalPathOverrideForProgram(prog.ProgramNum);
			textOverride.Text=pathOverrideOld;
			textUser.Text=ProgramProperties.GetPropVal(prog.ProgramNum,"Username");
			textPassword.Text=CodeBase.MiscUtils.Decrypt(ProgramProperties.GetPropVal(prog.ProgramNum,"Password"));
			string paymentType=ProgramProperties.GetPropVal(prog.ProgramNum,"PaymentType");
			for(int i=0;i<DefC.Short[(int)DefCat.PaymentTypes].Length;i++) {
				comboPaymentType.Items.Add(DefC.Short[(int)DefCat.PaymentTypes][i].ItemName);
				if(DefC.Short[(int)DefCat.PaymentTypes][i].DefNum.ToString()==paymentType)
					comboPaymentType.SelectedIndex=i;
			}
			textXWebID.Text=ProgramProperties.GetPropVal(prog.ProgramNum,"XWebID");
			string authKey=CodeBase.MiscUtils.Decrypt(ProgramProperties.GetPropVal(prog.ProgramNum,"AuthKey"));
			if(authKey.Length>0) {
				//X-Charge does not show the Auth Key within their server set up.  We shall do the same.
				textAuthKey.UseSystemPasswordChar=true;
				textAuthKey.Text=authKey;
			}
			textTerminalID.Text=ProgramProperties.GetPropVal(prog.ProgramNum,"TerminalID");
		}

		///<summary>Call to validate that the password typed in meets the X-Web password strength requirements.  Passwords must be between 8 and 15 characters in length, and must contain at least one letter, one number, and one of these special characters: $%^&+=</summary>
		private bool IsPasswordXWebValid() {
			string password=textPassword.Text.Trim();
			if(password.Length < 8 || password.Length > 15) {//between 8 - 15 chars
				return false;
			}
			if(!Regex.IsMatch(password,"[A-Za-z]+")) {//must contain at least one letter
				return false;
			}
			if(!Regex.IsMatch(password,"[0-9]+")) {//must contain at least one number
				return false;
			}
			if(!Regex.IsMatch(password,"[$%^&+=]+")) {//must contain at least one special character
				return false;
			}
			return true;
		}

		private void textAuthKey_TextChanged(object sender,EventArgs e) {
			//We want to let users see what they are typing in if they cleared out the AuthKey field completely or are typing in for the first time.
			//X-Charge does this in their server settings window.  We shall do the same.
			if(textAuthKey.Text.Trim()=="") {
				textAuthKey.UseSystemPasswordChar=false;
			}
		}

		private void linkLabel1_LinkClicked(object sender,LinkLabelLinkClickedEventArgs e) {
			Process.Start("http://xchargepayments.com/opendental/");
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(prog==null){
				MsgBox.Show(this,"X-Charge entry is missing from the database.");//should never happen
				return;
			}
			if(textOverride.Text!="") {
				if(!File.Exists(textOverride.Text)) {
					MsgBox.Show(this,"Local override path is not valid.");
					return;
				}
			}
			else if(textPath.Text=="" && textXWebID.Text!="" && textAuthKey.Text!="" && textTerminalID.Text!="") {
				//If XWeb is enabled and the client application is disabled, then we do not need to check the client application path.
			}
			else if(!File.Exists(textPath.Text)) {
				MsgBox.Show(this,"Path is not valid.");
				return;
			}
			if(comboPaymentType.SelectedIndex==-1){
				MsgBox.Show(this,"Please select a payment type first.");
				return;
			}
			//Check to see if ANY X-Web settings have been set.
			if(textXWebID.Text.Trim().Length > 0 
				|| textAuthKey.Text.Trim().Length > 0
				|| textTerminalID.Text.Trim().Length > 0) 
			{
				//Validate ALL XWebID, AuthKey, and TerminalID.  Each is required for X-Web to work.
				if(!Regex.IsMatch(textXWebID.Text.Trim(),"^[0-9]{12}$")) {
					MsgBox.Show(this,"XWeb ID must be 12 digits.");
					return;
				}
				if(!Regex.IsMatch(textAuthKey.Text.Trim(),"^[A-Za-z0-9]{32}$")) {
					MsgBox.Show(this,"Auth Key must be 32 alphanumeric characters.");
					return;
				}
				if(!Regex.IsMatch(textTerminalID.Text.Trim(),"^[0-9]{8}$")) {
					MsgBox.Show(this,"Terminal ID must be 8 digits.");
					return;
				}
				//We are not going to give the option for users to use their User Name and Password.
				//The following password strength requirement would need to be enforced if we want to start allowing use of User Names and Passwords in lieu of the Auth Key.
				/****************************************************************************************************************************
				//XWebID and TerminalID are valid.  Make sure the password meets the required complexity for XWeb.
				if(!IsPasswordXWebValid()) {
					MessageBox.Show(this,Lan.g(this,"Passwords must be between 8 and 15 characters in length, and must contain at least one letter, one number, and one of these special characters")+": $%^&+=");
					return;
				}
				****************************************************************************************************************************/
			}
			prog.Enabled=checkEnabled.Checked;
			prog.Path=textPath.Text;
			if(pathOverrideOld!=textOverride.Text) {
				ProgramProperties.InsertOrUpdateLocalOverridePath(prog.ProgramNum,textOverride.Text);
				ProgramProperties.RefreshCache();
			}
			Programs.Update(prog);
			string paymentType=DefC.Short[(int)DefCat.PaymentTypes][comboPaymentType.SelectedIndex].DefNum.ToString();
			ProgramProperties.SetProperty(prog.ProgramNum,"PaymentType",paymentType);
			ProgramProperties.SetProperty(prog.ProgramNum,"Username",textUser.Text);
			ProgramProperties.SetProperty(prog.ProgramNum,"Password",CodeBase.MiscUtils.Encrypt(textPassword.Text));
			ProgramProperties.SetProperty(prog.ProgramNum,"XWebID",textXWebID.Text.Trim());
			ProgramProperties.SetProperty(prog.ProgramNum,"AuthKey",CodeBase.MiscUtils.Encrypt(textAuthKey.Text.Trim()));
			ProgramProperties.SetProperty(prog.ProgramNum,"TerminalID",textTerminalID.Text.Trim());
			DataValid.SetInvalid(InvalidType.Programs);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}


		

		


	}
}





















