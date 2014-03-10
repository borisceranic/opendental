namespace OpenDental{
	partial class FormEtrans835ProcEdit {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEtrans835ProcEdit));
			this.textProcDescript = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.textProcCode = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.textProcNum = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.textProcFee = new System.Windows.Forms.TextBox();
			this.labelEquation = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.textInsPaidCalc = new System.Windows.Forms.TextBox();
			this.groupBalancing = new System.Windows.Forms.GroupBox();
			this.textOtherAdjustmentSum = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.textPatientPortionSum = new System.Windows.Forms.TextBox();
			this.textContractualObligSum = new System.Windows.Forms.TextBox();
			this.textPayorReductionSum = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.textInsPaid = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.gridProcedureAdjustments = new OpenDental.UI.ODGrid();
			this.gridRemarks = new OpenDental.UI.ODGrid();
			this.butClose = new OpenDental.UI.Button();
			this.gridSupplementalInfo = new OpenDental.UI.ODGrid();
			this.groupBalancing.SuspendLayout();
			this.SuspendLayout();
			// 
			// textProcDescript
			// 
			this.textProcDescript.Location = new System.Drawing.Point(140, 11);
			this.textProcDescript.Name = "textProcDescript";
			this.textProcDescript.ReadOnly = true;
			this.textProcDescript.Size = new System.Drawing.Size(325, 20);
			this.textProcDescript.TabIndex = 151;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(2, 11);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(138, 20);
			this.label8.TabIndex = 150;
			this.label8.Text = "Proc Descript";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textProcCode
			// 
			this.textProcCode.Location = new System.Drawing.Point(140, 31);
			this.textProcCode.Name = "textProcCode";
			this.textProcCode.ReadOnly = true;
			this.textProcCode.Size = new System.Drawing.Size(90, 20);
			this.textProcCode.TabIndex = 153;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(2, 31);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(138, 20);
			this.label9.TabIndex = 152;
			this.label9.Text = "Proc Code";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textProcNum
			// 
			this.textProcNum.Location = new System.Drawing.Point(626, 11);
			this.textProcNum.Name = "textProcNum";
			this.textProcNum.ReadOnly = true;
			this.textProcNum.Size = new System.Drawing.Size(90, 20);
			this.textProcNum.TabIndex = 192;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(488, 11);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(138, 20);
			this.label5.TabIndex = 191;
			this.label5.Text = "ProcNum";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textProcFee
			// 
			this.textProcFee.Location = new System.Drawing.Point(5, 37);
			this.textProcFee.Name = "textProcFee";
			this.textProcFee.ReadOnly = true;
			this.textProcFee.Size = new System.Drawing.Size(110, 20);
			this.textProcFee.TabIndex = 200;
			// 
			// labelEquation
			// 
			this.labelEquation.Location = new System.Drawing.Point(6, 16);
			this.labelEquation.Name = "labelEquation";
			this.labelEquation.Size = new System.Drawing.Size(944, 20);
			this.labelEquation.TabIndex = 199;
			this.labelEquation.Text = "Proc Fee                        -    Patient Portion Sum        -   Contractual O" +
    "blig. Sum  -    Payor Reduction Sum     -    Other Adjustment Sum    =    Ins Pa" +
    "id Calc";
			this.labelEquation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(119, 37);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(12, 20);
			this.label3.TabIndex = 206;
			this.label3.Text = "-";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(645, 37);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(12, 20);
			this.label10.TabIndex = 207;
			this.label10.Text = "=";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsPaidCalc
			// 
			this.textInsPaidCalc.Location = new System.Drawing.Point(662, 37);
			this.textInsPaidCalc.Name = "textInsPaidCalc";
			this.textInsPaidCalc.ReadOnly = true;
			this.textInsPaidCalc.Size = new System.Drawing.Size(110, 20);
			this.textInsPaidCalc.TabIndex = 208;
			// 
			// groupBalancing
			// 
			this.groupBalancing.Controls.Add(this.textOtherAdjustmentSum);
			this.groupBalancing.Controls.Add(this.label11);
			this.groupBalancing.Controls.Add(this.textPatientPortionSum);
			this.groupBalancing.Controls.Add(this.textContractualObligSum);
			this.groupBalancing.Controls.Add(this.textPayorReductionSum);
			this.groupBalancing.Controls.Add(this.label12);
			this.groupBalancing.Controls.Add(this.label13);
			this.groupBalancing.Controls.Add(this.textProcFee);
			this.groupBalancing.Controls.Add(this.labelEquation);
			this.groupBalancing.Controls.Add(this.textInsPaidCalc);
			this.groupBalancing.Controls.Add(this.label3);
			this.groupBalancing.Controls.Add(this.label10);
			this.groupBalancing.Location = new System.Drawing.Point(9, 61);
			this.groupBalancing.Name = "groupBalancing";
			this.groupBalancing.Size = new System.Drawing.Size(956, 65);
			this.groupBalancing.TabIndex = 210;
			this.groupBalancing.TabStop = false;
			this.groupBalancing.Text = "Balancing - Ins Paid Calc should exactly match Ins Paid";
			// 
			// textOtherAdjustmentSum
			// 
			this.textOtherAdjustmentSum.Location = new System.Drawing.Point(529, 37);
			this.textOtherAdjustmentSum.Name = "textOtherAdjustmentSum";
			this.textOtherAdjustmentSum.ReadOnly = true;
			this.textOtherAdjustmentSum.Size = new System.Drawing.Size(110, 20);
			this.textOtherAdjustmentSum.TabIndex = 217;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(511, 37);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(12, 20);
			this.label11.TabIndex = 216;
			this.label11.Text = "-";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPatientPortionSum
			// 
			this.textPatientPortionSum.Location = new System.Drawing.Point(137, 37);
			this.textPatientPortionSum.Name = "textPatientPortionSum";
			this.textPatientPortionSum.ReadOnly = true;
			this.textPatientPortionSum.Size = new System.Drawing.Size(110, 20);
			this.textPatientPortionSum.TabIndex = 211;
			// 
			// textContractualObligSum
			// 
			this.textContractualObligSum.Location = new System.Drawing.Point(265, 37);
			this.textContractualObligSum.Name = "textContractualObligSum";
			this.textContractualObligSum.ReadOnly = true;
			this.textContractualObligSum.Size = new System.Drawing.Size(110, 20);
			this.textContractualObligSum.TabIndex = 215;
			// 
			// textPayorReductionSum
			// 
			this.textPayorReductionSum.Location = new System.Drawing.Point(396, 37);
			this.textPayorReductionSum.Name = "textPayorReductionSum";
			this.textPayorReductionSum.ReadOnly = true;
			this.textPayorReductionSum.Size = new System.Drawing.Size(110, 20);
			this.textPayorReductionSum.TabIndex = 214;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(251, 37);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(12, 20);
			this.label12.TabIndex = 212;
			this.label12.Text = "-";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(377, 37);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(12, 20);
			this.label13.TabIndex = 213;
			this.label13.Text = "-";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsPaid
			// 
			this.textInsPaid.Location = new System.Drawing.Point(626, 31);
			this.textInsPaid.Name = "textInsPaid";
			this.textInsPaid.ReadOnly = true;
			this.textInsPaid.Size = new System.Drawing.Size(90, 20);
			this.textInsPaid.TabIndex = 212;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(488, 31);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(138, 20);
			this.label1.TabIndex = 211;
			this.label1.Text = "Ins Paid";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// gridProcedureAdjustments
			// 
			this.gridProcedureAdjustments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridProcedureAdjustments.HScrollVisible = false;
			this.gridProcedureAdjustments.Location = new System.Drawing.Point(9, 132);
			this.gridProcedureAdjustments.Name = "gridProcedureAdjustments";
			this.gridProcedureAdjustments.ScrollValue = 0;
			this.gridProcedureAdjustments.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridProcedureAdjustments.Size = new System.Drawing.Size(956, 107);
			this.gridProcedureAdjustments.TabIndex = 198;
			this.gridProcedureAdjustments.TabStop = false;
			this.gridProcedureAdjustments.Title = "Procedure Adjustments";
			this.gridProcedureAdjustments.TranslationName = "FormEtrans835Edit";
			// 
			// gridRemarks
			// 
			this.gridRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridRemarks.HScrollVisible = false;
			this.gridRemarks.Location = new System.Drawing.Point(9, 245);
			this.gridRemarks.Name = "gridRemarks";
			this.gridRemarks.ScrollValue = 0;
			this.gridRemarks.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridRemarks.Size = new System.Drawing.Size(956, 107);
			this.gridRemarks.TabIndex = 197;
			this.gridRemarks.TabStop = false;
			this.gridRemarks.Title = "Remarks";
			this.gridRemarks.TranslationName = "FormEtrans835Edit";
			this.gridRemarks.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridRemarks_CellDoubleClick);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(890, 470);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 3;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// gridSupplementalInfo
			// 
			this.gridSupplementalInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridSupplementalInfo.HScrollVisible = false;
			this.gridSupplementalInfo.Location = new System.Drawing.Point(9, 358);
			this.gridSupplementalInfo.Name = "gridSupplementalInfo";
			this.gridSupplementalInfo.ScrollValue = 0;
			this.gridSupplementalInfo.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridSupplementalInfo.Size = new System.Drawing.Size(956, 107);
			this.gridSupplementalInfo.TabIndex = 213;
			this.gridSupplementalInfo.TabStop = false;
			this.gridSupplementalInfo.Title = "Supplemental Info";
			this.gridSupplementalInfo.TranslationName = "FormEtrans835Edit";
			// 
			// FormEtrans835ProcEdit
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(974, 502);
			this.Controls.Add(this.gridSupplementalInfo);
			this.Controls.Add(this.textInsPaid);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBalancing);
			this.Controls.Add(this.gridProcedureAdjustments);
			this.Controls.Add(this.gridRemarks);
			this.Controls.Add(this.textProcNum);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textProcCode);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.textProcDescript);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(990, 540);
			this.Name = "FormEtrans835ProcEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Procedure Explanation of Benefits (EOB)";
			this.Load += new System.EventHandler(this.FormEtrans835ClaimEdit_Load);
			this.Resize += new System.EventHandler(this.FormEtrans835ClaimEdit_Resize);
			this.groupBalancing.ResumeLayout(false);
			this.groupBalancing.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.TextBox textProcDescript;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox textProcCode;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox textProcNum;
		private System.Windows.Forms.Label label5;
		private UI.ODGrid gridRemarks;
		private UI.ODGrid gridProcedureAdjustments;
		private System.Windows.Forms.TextBox textProcFee;
		private System.Windows.Forms.Label labelEquation;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox textInsPaidCalc;
		private System.Windows.Forms.GroupBox groupBalancing;
		private System.Windows.Forms.TextBox textInsPaid;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textOtherAdjustmentSum;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox textPatientPortionSum;
		private System.Windows.Forms.TextBox textContractualObligSum;
		private System.Windows.Forms.TextBox textPayorReductionSum;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private UI.ODGrid gridSupplementalInfo;
	}
}