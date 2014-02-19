namespace OpenDental{
	partial class FormEtrans835ClaimEdit {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEtrans835ClaimEdit));
			this.textPatientName = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.textDateService = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.butClose = new OpenDental.UI.Button();
			this.textClaimIdentifier = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textPayorControlNum = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textStatus = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textClaimFee = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.textInsPaid = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textPatientPortion = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.gridAdjudicationDetails = new OpenDental.UI.ODGrid();
			this.gridProcedureDetails = new OpenDental.UI.ODGrid();
			this.SuspendLayout();
			// 
			// textPatientName
			// 
			this.textPatientName.Location = new System.Drawing.Point(140, 11);
			this.textPatientName.Name = "textPatientName";
			this.textPatientName.ReadOnly = true;
			this.textPatientName.Size = new System.Drawing.Size(325, 20);
			this.textPatientName.TabIndex = 151;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(2, 11);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(138, 20);
			this.label8.TabIndex = 150;
			this.label8.Text = "Patient";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDateService
			// 
			this.textDateService.Location = new System.Drawing.Point(140, 31);
			this.textDateService.Name = "textDateService";
			this.textDateService.ReadOnly = true;
			this.textDateService.Size = new System.Drawing.Size(90, 20);
			this.textDateService.TabIndex = 153;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(2, 31);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(138, 20);
			this.label9.TabIndex = 152;
			this.label9.Text = "Date Service";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
			// textClaimIdentifier
			// 
			this.textClaimIdentifier.Location = new System.Drawing.Point(140, 51);
			this.textClaimIdentifier.Name = "textClaimIdentifier";
			this.textClaimIdentifier.ReadOnly = true;
			this.textClaimIdentifier.Size = new System.Drawing.Size(90, 20);
			this.textClaimIdentifier.TabIndex = 186;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(2, 51);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(138, 20);
			this.label1.TabIndex = 185;
			this.label1.Text = "Claim Identfier";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPayorControlNum
			// 
			this.textPayorControlNum.Location = new System.Drawing.Point(140, 71);
			this.textPayorControlNum.Name = "textPayorControlNum";
			this.textPayorControlNum.ReadOnly = true;
			this.textPayorControlNum.Size = new System.Drawing.Size(90, 20);
			this.textPayorControlNum.TabIndex = 188;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(2, 71);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(138, 20);
			this.label2.TabIndex = 187;
			this.label2.Text = "Payor Control Num";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textStatus
			// 
			this.textStatus.Location = new System.Drawing.Point(626, 11);
			this.textStatus.Name = "textStatus";
			this.textStatus.ReadOnly = true;
			this.textStatus.Size = new System.Drawing.Size(325, 20);
			this.textStatus.TabIndex = 190;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(488, 11);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(138, 20);
			this.label4.TabIndex = 189;
			this.label4.Text = "Status";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textClaimFee
			// 
			this.textClaimFee.Location = new System.Drawing.Point(626, 31);
			this.textClaimFee.Name = "textClaimFee";
			this.textClaimFee.ReadOnly = true;
			this.textClaimFee.Size = new System.Drawing.Size(90, 20);
			this.textClaimFee.TabIndex = 192;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(488, 31);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(138, 20);
			this.label5.TabIndex = 191;
			this.label5.Text = "Claim Fee";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsPaid
			// 
			this.textInsPaid.Location = new System.Drawing.Point(626, 51);
			this.textInsPaid.Name = "textInsPaid";
			this.textInsPaid.ReadOnly = true;
			this.textInsPaid.Size = new System.Drawing.Size(90, 20);
			this.textInsPaid.TabIndex = 194;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(488, 51);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(138, 20);
			this.label6.TabIndex = 193;
			this.label6.Text = "Ins Paid";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPatientPortion
			// 
			this.textPatientPortion.Location = new System.Drawing.Point(626, 71);
			this.textPatientPortion.Name = "textPatientPortion";
			this.textPatientPortion.ReadOnly = true;
			this.textPatientPortion.Size = new System.Drawing.Size(90, 20);
			this.textPatientPortion.TabIndex = 196;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(488, 71);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(138, 20);
			this.label7.TabIndex = 195;
			this.label7.Text = "Patient Portion";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// gridAdjudicationDetails
			// 
			this.gridAdjudicationDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridAdjudicationDetails.HScrollVisible = false;
			this.gridAdjudicationDetails.Location = new System.Drawing.Point(9, 101);
			this.gridAdjudicationDetails.Name = "gridAdjudicationDetails";
			this.gridAdjudicationDetails.ScrollValue = 0;
			this.gridAdjudicationDetails.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridAdjudicationDetails.Size = new System.Drawing.Size(956, 84);
			this.gridAdjudicationDetails.TabIndex = 197;
			this.gridAdjudicationDetails.TabStop = false;
			this.gridAdjudicationDetails.Title = "Adjudication Details";
			this.gridAdjudicationDetails.TranslationName = "FormEtrans835Edit";
			// 
			// gridProcedureDetails
			// 
			this.gridProcedureDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridProcedureDetails.HScrollVisible = false;
			this.gridProcedureDetails.Location = new System.Drawing.Point(9, 191);
			this.gridProcedureDetails.Name = "gridProcedureDetails";
			this.gridProcedureDetails.ScrollValue = 0;
			this.gridProcedureDetails.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridProcedureDetails.Size = new System.Drawing.Size(956, 467);
			this.gridProcedureDetails.TabIndex = 0;
			this.gridProcedureDetails.TabStop = false;
			this.gridProcedureDetails.Title = "Procedure Details";
			this.gridProcedureDetails.TranslationName = "FormEtrans835Edit";
			// 
			// FormEtrans835ClaimEdit
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(974, 696);
			this.Controls.Add(this.gridAdjudicationDetails);
			this.Controls.Add(this.textPatientPortion);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.textInsPaid);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.textClaimFee);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textStatus);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textPayorControlNum);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textClaimIdentifier);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textDateService);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.textPatientName);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.gridProcedureDetails);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(990, 734);
			this.Name = "FormEtrans835ClaimEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Electronic Claim Explanation of Benefits (EOB)";
			this.Load += new System.EventHandler(this.FormEtrans835ClaimEdit_Load);
			this.Resize += new System.EventHandler(this.FormEtrans835ClaimEdit_Resize);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butClose;
		private OpenDental.UI.ODGrid gridProcedureDetails;
		private System.Windows.Forms.TextBox textPatientName;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox textDateService;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox textClaimIdentifier;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textPayorControlNum;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textStatus;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textClaimFee;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textInsPaid;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textPatientPortion;
		private System.Windows.Forms.Label label7;
		private UI.ODGrid gridAdjudicationDetails;
	}
}