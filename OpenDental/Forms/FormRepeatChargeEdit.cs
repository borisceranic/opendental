using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OpenDentBusiness;
using Button=OpenDental.UI.Button;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormRepeatChargeEdit :ODForm{
		private Button butCancel;
		private Button butOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;
		///<summary></summary>
		public bool IsNew;
		private Label label1;
		private TextBox textCode;
		private Label label2;
		private ValidDouble textChargeAmt;
		private ValidDate textDateStart;
		private Label label3;
		private Label label4;
		private ValidDate textDateStop;
		private TextBox textNote;
		private Label label5;
		private TextBox textDesc;
		private Label label6;
		private Button butDelete;
		private Label label7;
		private CheckBox checkCopyNoteToProc;
		private Label label8;
		private Label label9;
		private Button butManual;
		private CheckBox checkCreatesClaim;
		private CheckBox checkIsEnabled;
		private TextBox textTotalAmount;
		private TextBox textNumOfCharges;
		private Label label10;
		private GroupBox groupBox1;
		private Button butCalculate;
		private Label labelBillingCycleDay;
		private ValidNumber textBillingDay;
		private Label labelPatNum;
		private TextBox textPatNum;
		private Button butMoveTo;
		private RepeatCharge RepeatCur;

		///<summary></summary>
		public FormRepeatChargeEdit(RepeatCharge repeatCur)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			RepeatCur=repeatCur;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRepeatChargeEdit));
			this.label1 = new System.Windows.Forms.Label();
			this.textCode = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textNote = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.textDesc = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.checkCopyNoteToProc = new System.Windows.Forms.CheckBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.checkCreatesClaim = new System.Windows.Forms.CheckBox();
			this.checkIsEnabled = new System.Windows.Forms.CheckBox();
			this.textTotalAmount = new System.Windows.Forms.TextBox();
			this.textNumOfCharges = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.butCalculate = new OpenDental.UI.Button();
			this.butManual = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.textDateStop = new OpenDental.ValidDate();
			this.textDateStart = new OpenDental.ValidDate();
			this.textChargeAmt = new OpenDental.ValidDouble();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.labelBillingCycleDay = new System.Windows.Forms.Label();
			this.textBillingDay = new OpenDental.ValidNumber();
			this.labelPatNum = new System.Windows.Forms.Label();
			this.textPatNum = new System.Windows.Forms.TextBox();
			this.butMoveTo = new OpenDental.UI.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(4, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(156, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Code";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textCode
			// 
			this.textCode.Location = new System.Drawing.Point(162, 17);
			this.textCode.MaxLength = 15;
			this.textCode.Name = "textCode";
			this.textCode.ReadOnly = true;
			this.textCode.Size = new System.Drawing.Size(100, 20);
			this.textCode.TabIndex = 3;
			this.textCode.TabStop = false;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(4, 139);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(156, 16);
			this.label2.TabIndex = 4;
			this.label2.Text = "Charge Amount";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(4, 168);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(156, 16);
			this.label3.TabIndex = 7;
			this.label3.Text = "Date Start";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(4, 196);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(156, 16);
			this.label4.TabIndex = 9;
			this.label4.Text = "Date Stop";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textNote
			// 
			this.textNote.Location = new System.Drawing.Point(162, 260);
			this.textNote.MaxLength = 10000;
			this.textNote.Multiline = true;
			this.textNote.Name = "textNote";
			this.textNote.Size = new System.Drawing.Size(424, 114);
			this.textNote.TabIndex = 6;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(4, 263);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(156, 16);
			this.label5.TabIndex = 10;
			this.label5.Text = "Note";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDesc
			// 
			this.textDesc.BackColor = System.Drawing.SystemColors.Control;
			this.textDesc.Location = new System.Drawing.Point(267, 17);
			this.textDesc.Name = "textDesc";
			this.textDesc.Size = new System.Drawing.Size(241, 20);
			this.textDesc.TabIndex = 40;
			this.textDesc.TabStop = false;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(265, 1);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(224, 16);
			this.label6.TabIndex = 39;
			this.label6.Text = "Procedure Description:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label7.Location = new System.Drawing.Point(128, 491);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(238, 29);
			this.label7.TabIndex = 42;
			this.label7.Text = "It\'s OK to delete an obsolete repeating charge.   It does not affect any charges " +
    "already billed.";
			this.label7.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// checkCopyNoteToProc
			// 
			this.checkCopyNoteToProc.Location = new System.Drawing.Point(162, 378);
			this.checkCopyNoteToProc.Name = "checkCopyNoteToProc";
			this.checkCopyNoteToProc.Size = new System.Drawing.Size(250, 18);
			this.checkCopyNoteToProc.TabIndex = 7;
			this.checkCopyNoteToProc.Text = "Copy note to procedure billing note.";
			this.checkCopyNoteToProc.UseVisualStyleBackColor = true;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(17, 22);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(136, 16);
			this.label8.TabIndex = 44;
			this.label8.Text = "Total Amount";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(17, 48);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(136, 16);
			this.label9.TabIndex = 46;
			this.label9.Text = "Number of Charges";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkCreatesClaim
			// 
			this.checkCreatesClaim.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkCreatesClaim.Location = new System.Drawing.Point(7, 218);
			this.checkCreatesClaim.Name = "checkCreatesClaim";
			this.checkCreatesClaim.Size = new System.Drawing.Size(169, 18);
			this.checkCreatesClaim.TabIndex = 4;
			this.checkCreatesClaim.Text = "Creates Claim";
			this.checkCreatesClaim.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkCreatesClaim.UseVisualStyleBackColor = true;
			// 
			// checkIsEnabled
			// 
			this.checkIsEnabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsEnabled.Location = new System.Drawing.Point(7, 239);
			this.checkIsEnabled.Name = "checkIsEnabled";
			this.checkIsEnabled.Size = new System.Drawing.Size(169, 18);
			this.checkIsEnabled.TabIndex = 5;
			this.checkIsEnabled.Text = "Enabled";
			this.checkIsEnabled.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsEnabled.UseVisualStyleBackColor = true;
			// 
			// textTotalAmount
			// 
			this.textTotalAmount.Location = new System.Drawing.Point(155, 19);
			this.textTotalAmount.Name = "textTotalAmount";
			this.textTotalAmount.Size = new System.Drawing.Size(100, 20);
			this.textTotalAmount.TabIndex = 0;
			// 
			// textNumOfCharges
			// 
			this.textNumOfCharges.Location = new System.Drawing.Point(155, 45);
			this.textNumOfCharges.Name = "textNumOfCharges";
			this.textNumOfCharges.Size = new System.Drawing.Size(100, 20);
			this.textNumOfCharges.TabIndex = 1;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(245, 419);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(225, 29);
			this.label10.TabIndex = 53;
			this.label10.Text = "This will add a completed procedure of the code listed above to this patient\'s ac" +
    "count.";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.butCalculate);
			this.groupBox1.Controls.Add(this.textTotalAmount);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.textNumOfCharges);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Location = new System.Drawing.Point(7, 48);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(359, 79);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Calculate Charge Amount (optional)";
			// 
			// butCalculate
			// 
			this.butCalculate.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCalculate.Autosize = true;
			this.butCalculate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCalculate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCalculate.CornerRadius = 4F;
			this.butCalculate.Location = new System.Drawing.Point(261, 44);
			this.butCalculate.Name = "butCalculate";
			this.butCalculate.Size = new System.Drawing.Size(75, 24);
			this.butCalculate.TabIndex = 2;
			this.butCalculate.Text = "Calculate";
			this.butCalculate.Click += new System.EventHandler(this.butCalculate_Click);
			// 
			// butManual
			// 
			this.butManual.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butManual.Autosize = true;
			this.butManual.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butManual.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butManual.CornerRadius = 4F;
			this.butManual.Location = new System.Drawing.Point(162, 421);
			this.butManual.Name = "butManual";
			this.butManual.Size = new System.Drawing.Size(75, 24);
			this.butManual.TabIndex = 12;
			this.butManual.Text = "Manual";
			this.butManual.Click += new System.EventHandler(this.butManual_Click);
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
			this.butDelete.Location = new System.Drawing.Point(35, 493);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(83, 26);
			this.butDelete.TabIndex = 11;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// textDateStop
			// 
			this.textDateStop.Location = new System.Drawing.Point(162, 194);
			this.textDateStop.Name = "textDateStop";
			this.textDateStop.Size = new System.Drawing.Size(100, 20);
			this.textDateStop.TabIndex = 3;
			// 
			// textDateStart
			// 
			this.textDateStart.Location = new System.Drawing.Point(162, 165);
			this.textDateStart.Name = "textDateStart";
			this.textDateStart.Size = new System.Drawing.Size(100, 20);
			this.textDateStart.TabIndex = 2;
			// 
			// textChargeAmt
			// 
			this.textChargeAmt.Location = new System.Drawing.Point(162, 136);
			this.textChargeAmt.MaxVal = 100000000D;
			this.textChargeAmt.MinVal = -100000000D;
			this.textChargeAmt.Name = "textChargeAmt";
			this.textChargeAmt.Size = new System.Drawing.Size(100, 20);
			this.textChargeAmt.TabIndex = 1;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(595, 450);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 9;
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
			this.butCancel.Location = new System.Drawing.Point(595, 491);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 10;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// labelBillingCycleDay
			// 
			this.labelBillingCycleDay.Location = new System.Drawing.Point(372, 67);
			this.labelBillingCycleDay.Name = "labelBillingCycleDay";
			this.labelBillingCycleDay.Size = new System.Drawing.Size(137, 16);
			this.labelBillingCycleDay.TabIndex = 55;
			this.labelBillingCycleDay.Text = "Billing Cycle Day";
			this.labelBillingCycleDay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelBillingCycleDay.Visible = false;
			// 
			// textBillingDay
			// 
			this.textBillingDay.Location = new System.Drawing.Point(511, 66);
			this.textBillingDay.MaxVal = 31;
			this.textBillingDay.MinVal = 1;
			this.textBillingDay.Name = "textBillingDay";
			this.textBillingDay.Size = new System.Drawing.Size(75, 20);
			this.textBillingDay.TabIndex = 8;
			this.textBillingDay.Visible = false;
			// 
			// labelPatNum
			// 
			this.labelPatNum.Location = new System.Drawing.Point(400, 93);
			this.labelPatNum.Name = "labelPatNum";
			this.labelPatNum.Size = new System.Drawing.Size(110, 16);
			this.labelPatNum.TabIndex = 58;
			this.labelPatNum.Text = "PatNum";
			this.labelPatNum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelPatNum.Visible = false;
			// 
			// textPatNum
			// 
			this.textPatNum.Location = new System.Drawing.Point(511, 92);
			this.textPatNum.Name = "textPatNum";
			this.textPatNum.ReadOnly = true;
			this.textPatNum.Size = new System.Drawing.Size(75, 20);
			this.textPatNum.TabIndex = 57;
			this.textPatNum.Visible = false;
			// 
			// butMoveTo
			// 
			this.butMoveTo.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMoveTo.Autosize = true;
			this.butMoveTo.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMoveTo.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMoveTo.CornerRadius = 4F;
			this.butMoveTo.Location = new System.Drawing.Point(588, 90);
			this.butMoveTo.Name = "butMoveTo";
			this.butMoveTo.Size = new System.Drawing.Size(75, 24);
			this.butMoveTo.TabIndex = 56;
			this.butMoveTo.Text = "Move To";
			this.butMoveTo.Visible = false;
			this.butMoveTo.Click += new System.EventHandler(this.butMoveTo_Click);
			// 
			// FormRepeatChargeEdit
			// 
			this.ClientSize = new System.Drawing.Size(705, 545);
			this.Controls.Add(this.labelPatNum);
			this.Controls.Add(this.textPatNum);
			this.Controls.Add(this.butMoveTo);
			this.Controls.Add(this.textBillingDay);
			this.Controls.Add(this.labelBillingCycleDay);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.checkIsEnabled);
			this.Controls.Add(this.checkCreatesClaim);
			this.Controls.Add(this.butManual);
			this.Controls.Add(this.checkCopyNoteToProc);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.textDesc);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.textNote);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textDateStop);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textDateStart);
			this.Controls.Add(this.textChargeAmt);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textCode);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormRepeatChargeEdit";
			this.ShowInTaskbar = false;
			this.Text = "Edit Repeat Charge";
			this.Load += new System.EventHandler(this.FormRepeatChargeEdit_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormRepeatChargeEdit_Load(object sender,EventArgs e) {
			SetPatient();
			if(IsNew){
				FormProcCodes FormP=new FormProcCodes();
				FormP.IsSelectionMode=true;
				FormP.ShowDialog();
				if(FormP.DialogResult!=DialogResult.OK){
					DialogResult=DialogResult.Cancel;
					return;
				}
				ProcedureCode procCode=ProcedureCodes.GetProcCode(FormP.SelectedCodeNum);
				if(procCode.TreatArea!=TreatmentArea.Mouth 
					&& procCode.TreatArea!=TreatmentArea.None){
					MsgBox.Show(this,"Procedure codes that require tooth numbers are not allowed.");
					DialogResult=DialogResult.Cancel;
					return;
				}
				RepeatCur.ProcCode=ProcedureCodes.GetStringProcCode(FormP.SelectedCodeNum);
				RepeatCur.IsEnabled=true;
				RepeatCur.CreatesClaim=false;
			}
			else {//Existing repeating charge
				if(PrefC.GetBool(PrefName.DistributorKey) && Regex.IsMatch(RepeatCur.ProcCode,"^Z[0-9]{3,}$")) {//Only visible if HQ and a eRx Z code.
					labelPatNum.Visible=true;
					textPatNum.Visible=true;
					butMoveTo.Visible=true;
				}
			}
			textCode.Text=RepeatCur.ProcCode;
			textDesc.Text=ProcedureCodes.GetProcCode(RepeatCur.ProcCode).Descript;
			textChargeAmt.Text=RepeatCur.ChargeAmt.ToString("F");
			if(RepeatCur.DateStart.Year>1880){
				textDateStart.Text=RepeatCur.DateStart.ToShortDateString();
			}
			if(RepeatCur.DateStop.Year>1880){
				textDateStop.Text=RepeatCur.DateStop.ToShortDateString();
			}
			textNote.Text=RepeatCur.Note;
			checkCopyNoteToProc.Checked=RepeatCur.CopyNoteToProc;
			checkCreatesClaim.Checked=RepeatCur.CreatesClaim;
			checkIsEnabled.Checked=RepeatCur.IsEnabled;
			if(PrefC.GetBool(PrefName.DistributorKey)) {//OD HQ disable the IsEnabled and CreatesClaim checkboxes
				checkCreatesClaim.Enabled=false;
				checkIsEnabled.Enabled=false;
			}
			if(PrefC.IsODHQ && RepeatCur.ProcCode=="038") {//Integrated Texting Access Charge
				textDesc.ReadOnly=true;
				textTotalAmount.ReadOnly=true;
				textNumOfCharges.ReadOnly=true;
				textChargeAmt.ReadOnly=true;
				textDateStart.ReadOnly=true;
				textDateStop.ReadOnly=true;
				textNote.ReadOnly=true;
				checkCreatesClaim.AutoCheck=false;
				checkIsEnabled.AutoCheck=false;
				checkCopyNoteToProc.AutoCheck=false;
				butDelete.Enabled=false;
				butManual.Enabled=false;
				butCalculate.Enabled=false;
				butOK.Enabled=false;
				butCancel.Text=Lan.g(this,"Close");
			}
			Patient pat=Patients.GetPat(RepeatCur.PatNum);//pat should never be null. If it is, this will fail.
			//If this is a new repeat charge and no other active repeat charges exist, set the billing cycle day to today
			if(IsNew && !RepeatCharges.ActiveRepeatChargeExists(RepeatCur.PatNum)) {
				textBillingDay.Text=DateTimeOD.Today.Day.ToString();
			}
			else {
				textBillingDay.Text=pat.BillingCycleDay.ToString();
			}
			if(PrefC.GetBool(PrefName.BillingUseBillingCycleDay)) {
				labelBillingCycleDay.Visible=true;
				textBillingDay.Visible=true;
			}
		}

		private void SetPatient() {
			//Set the title bar to show the patient's name much like the main screen does.
			Text+=" - "+Patients.GetLim(RepeatCur.PatNum).GetNameLF();
			textPatNum.Text=RepeatCur.PatNum.ToString();
		}

		///<summary>Adds the procedure code of the repeating charge to a credit card on the patient's account if the user okays it.</summary>
		private void AddProcedureToCC() {
			List<CreditCard> activeCards=CreditCards.GetActiveCards(RepeatCur.PatNum);
			if(activeCards.Count==0) {
				return;
			}
			CreditCard cardToAddProc=null;
			if(activeCards.Count==1) { //Only one active card so ask the user to add the procedure to that one
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"There is one active credit card on this patient's account.\r\nDo you want to add this procedure to "+
					"that card?")) {
					cardToAddProc=activeCards[0];
				}
			}
			else if(activeCards.FindAll(x => x.Procedures!="").Count==1) { //Only one card has procedures attached so ask the user to add to that card
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"There is one active credit card on this patient's account with authorized procedures attached.\r\n"
					+"Do you want to add this procedure to that card?")) {
					cardToAddProc=activeCards.FirstOrDefault(x => x.Procedures!="");
				}
			}
			else { //At least two cards have procedures attached to them or there are multiple active cards and none have procedures attached
				MsgBox.Show(this,"If you would like to add this procedure to a credit card, go to Credit Card Manage to choose the card.");
			}
			if(cardToAddProc==null) {
				return;
			}
			//Check if the procedure is already attached to this card; CreditCard.Procedures is a comma delimited list.
			List<string> procsOnCard=cardToAddProc.Procedures.Split(new string[] { "," },StringSplitOptions.RemoveEmptyEntries).ToList();
			if(procsOnCard.Exists(x => x==RepeatCur.ProcCode)) {
				return;
			}
			procsOnCard.Add(RepeatCur.ProcCode);
			cardToAddProc.Procedures=String.Join(",",procsOnCard);
			CreditCards.Update(cardToAddProc);
		}

		private void butManual_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.ProcComplCreate)) {
				return;
			}
			if(string.IsNullOrEmpty(textChargeAmt.Text)) {
				MsgBox.Show(this,"You must first enter a charge amount.");
				return;
			}
			double procFee;
			try {
				procFee=Double.Parse(textChargeAmt.Text);
			}
			catch {
				MsgBox.Show(this,"Invalid charge amount.");
				return;
			}
			Procedures.SetDateFirstVisit(DateTime.Today,1,Patients.GetPat(RepeatCur.PatNum));
			Procedure proc=new Procedure();
			proc.CodeNum=ProcedureCodes.GetCodeNum(textCode.Text);
			proc.DateEntryC=DateTimeOD.Today;
			proc.PatNum=RepeatCur.PatNum;
			proc.ProcDate=DateTimeOD.Today;
			proc.DateTP=DateTimeOD.Today;
			proc.ProcFee=procFee;
			proc.ProcStatus=ProcStat.C;
			proc.ProvNum=PrefC.GetLong(PrefName.PracticeDefaultProv);
			proc.MedicalCode=ProcedureCodes.GetProcCode(proc.CodeNum).MedicalCode;
			proc.BaseUnits=ProcedureCodes.GetProcCode(proc.CodeNum).BaseUnits;
			proc.DiagnosticCode=PrefC.GetString(PrefName.ICD9DefaultForNewProcs);
			proc.RepeatChargeNum=RepeatCur.RepeatChargeNum;
			//Check if the repeating charge has been flagged to copy it's note into the billing note of the procedure.
			if(RepeatCur.CopyNoteToProc) {
				proc.BillingNote=RepeatCur.Note;
			}
			if(!PrefC.GetBool(PrefName.EasyHidePublicHealth)) {
				Patient pat=Patients.GetPat(RepeatCur.PatNum);
				proc.SiteNum=pat.SiteNum;
			}
			Procedures.Insert(proc);
			Recalls.Synch(RepeatCur.PatNum);
			MsgBox.Show(this,"Procedure added.");
		}

		private void butCalculate_Click(object sender,EventArgs e) {
			if(PIn.Double(textNumOfCharges.Text).IsZero()	|| PIn.Double(textTotalAmount.Text).IsZero()) {
				textChargeAmt.Text=RepeatCur.ChargeAmt.ToString("F");
				return;
			}
			textChargeAmt.Text=(PIn.Double(textTotalAmount.Text)/PIn.Double(textNumOfCharges.Text)).ToString("F");
		}

		private void butMoveTo_Click(object sender,EventArgs e) {
			FormPatientSelect form=new FormPatientSelect();
			if(form.ShowDialog()!=DialogResult.OK) {
				return;
			}
			RepeatCur.PatNum=form.SelectedPatNum;
			SetPatient();
			Patient pat=Patients.GetPat(RepeatCur.PatNum);
			textBillingDay.Text=pat.BillingCycleDay.ToString();
		}

		private void butDelete_Click(object sender, EventArgs e) {
			RepeatCharges.Delete(RepeatCur);
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender, EventArgs e){
			if( textChargeAmt.errorProvider1.GetError(textChargeAmt)!=""
				|| textDateStart.errorProvider1.GetError(textDateStart)!=""
				|| textDateStop.errorProvider1.GetError(textDateStop)!=""
				|| textBillingDay.errorProvider1.GetError(textBillingDay)!=""
				)
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(textDateStart.Text==""){
				MsgBox.Show(this,"Start date cannot be left blank.");
				return;
			}
			if(PIn.Date(textDateStart.Text)!=RepeatCur.DateStart){//if the user changed the date
				if(PIn.Date(textDateStart.Text)<DateTime.Today.AddDays(-3)) {//and if the date the user entered is more than three days in the past
					MsgBox.Show(this,"Start date cannot be more than three days in the past.  You should enter previous charges manually in the account.");
					return;
				}
			}
			if(textDateStop.Text.Trim()!="" && PIn.Date(textDateStart.Text)>PIn.Date(textDateStop.Text)) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"The start date is after the stop date.  Continue?")) {
					return;
				}
			}
			if(PrefC.GetBool(PrefName.BillingUseBillingCycleDay) && textBillingDay.Text!="") {
				Patient patOld=Patients.GetPat(RepeatCur.PatNum);
				Patient patNew=patOld.Copy();
				patNew.BillingCycleDay=PIn.Int(textBillingDay.Text);
				Patients.Update(patNew,patOld);
			}
			RepeatCur.ProcCode=textCode.Text;
			RepeatCur.ChargeAmt=PIn.Double(textChargeAmt.Text);
			RepeatCur.DateStart=PIn.Date(textDateStart.Text);
			RepeatCur.DateStop=PIn.Date(textDateStop.Text);
			RepeatCur.Note=textNote.Text;
			RepeatCur.CopyNoteToProc=checkCopyNoteToProc.Checked;
			RepeatCur.IsEnabled=checkIsEnabled.Checked;
			RepeatCur.CreatesClaim=checkCreatesClaim.Checked;
			if(IsNew){
				if(!RepeatCharges.ActiveRepeatChargeExists(RepeatCur.PatNum) 
					&& (textBillingDay.Text=="" || textBillingDay.Text=="0"))
				{
					Patient patOld=Patients.GetPat(RepeatCur.PatNum);
					Patient patNew=patOld.Copy();
					patNew.BillingCycleDay=PIn.Date(textDateStart.Text).Day;
					Patients.Update(patNew,patOld);
				}
				RepeatCharges.Insert(RepeatCur);
				if(PrefC.IsODHQ) {
					AddProcedureToCC();
				}
			}
			else{ //not a new repeat charge
				RepeatCharges.Update(RepeatCur);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}



