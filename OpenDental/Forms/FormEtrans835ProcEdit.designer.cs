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
			this.gridRemarks = new OpenDental.UI.ODGrid();
			this.butClose = new OpenDental.UI.Button();
			this.gridProcedureAdjustments = new OpenDental.UI.ODGrid();
			this.textProcFee = new System.Windows.Forms.TextBox();
			this.labelEquation = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.textInsPaidCalc = new System.Windows.Forms.TextBox();
			this.textProcAdjAmtSum = new System.Windows.Forms.TextBox();
			this.groupBalancing = new System.Windows.Forms.GroupBox();
			this.textInsPaid = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
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
			// gridRemarks
			// 
			this.gridRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridRemarks.HScrollVisible = false;
			this.gridRemarks.Location = new System.Drawing.Point(9, 551);
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
			this.butClose.Location = new System.Drawing.Point(890, 664);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 3;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// gridProcedureAdjustments
			// 
			this.gridProcedureAdjustments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridProcedureAdjustments.HScrollVisible = false;
			this.gridProcedureAdjustments.Location = new System.Drawing.Point(9, 133);
			this.gridProcedureAdjustments.Name = "gridProcedureAdjustments";
			this.gridProcedureAdjustments.ScrollValue = 0;
			this.gridProcedureAdjustments.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridProcedureAdjustments.Size = new System.Drawing.Size(956, 107);
			this.gridProcedureAdjustments.TabIndex = 198;
			this.gridProcedureAdjustments.TabStop = false;
			this.gridProcedureAdjustments.Title = "Procedure Adjustments";
			this.gridProcedureAdjustments.TranslationName = "FormEtrans835Edit";
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
			this.labelEquation.Size = new System.Drawing.Size(371, 20);
			this.labelEquation.TabIndex = 199;
			this.labelEquation.Text = "Proc Fee                        -    Proc AdjAmt Sum          =    Ins Paid Calc";
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
			this.label10.Location = new System.Drawing.Point(249, 37);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(12, 20);
			this.label10.TabIndex = 207;
			this.label10.Text = "=";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsPaidCalc
			// 
			this.textInsPaidCalc.Location = new System.Drawing.Point(268, 37);
			this.textInsPaidCalc.Name = "textInsPaidCalc";
			this.textInsPaidCalc.ReadOnly = true;
			this.textInsPaidCalc.Size = new System.Drawing.Size(110, 20);
			this.textInsPaidCalc.TabIndex = 208;
			// 
			// textProcAdjAmtSum
			// 
			this.textProcAdjAmtSum.Location = new System.Drawing.Point(135, 37);
			this.textProcAdjAmtSum.Name = "textProcAdjAmtSum";
			this.textProcAdjAmtSum.ReadOnly = true;
			this.textProcAdjAmtSum.Size = new System.Drawing.Size(110, 20);
			this.textProcAdjAmtSum.TabIndex = 209;
			// 
			// groupBalancing
			// 
			this.groupBalancing.Controls.Add(this.textProcFee);
			this.groupBalancing.Controls.Add(this.textProcAdjAmtSum);
			this.groupBalancing.Controls.Add(this.labelEquation);
			this.groupBalancing.Controls.Add(this.textInsPaidCalc);
			this.groupBalancing.Controls.Add(this.label3);
			this.groupBalancing.Controls.Add(this.label10);
			this.groupBalancing.Location = new System.Drawing.Point(9, 61);
			this.groupBalancing.Name = "groupBalancing";
			this.groupBalancing.Size = new System.Drawing.Size(956, 66);
			this.groupBalancing.TabIndex = 210;
			this.groupBalancing.TabStop = false;
			this.groupBalancing.Text = "Balancing - Ins Paid Calc should exactly match Ins Paid";
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
			// FormEtrans835ProcEdit
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(974, 696);
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
			this.MinimumSize = new System.Drawing.Size(990, 734);
			this.Name = "FormEtrans835ProcEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Explanation of Benefits (EOB) for Procedure";
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
		private System.Windows.Forms.TextBox textProcAdjAmtSum;
		private System.Windows.Forms.GroupBox groupBalancing;
		private System.Windows.Forms.TextBox textInsPaid;
		private System.Windows.Forms.Label label1;
	}
}