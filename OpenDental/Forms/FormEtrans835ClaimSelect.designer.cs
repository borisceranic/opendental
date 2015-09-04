namespace OpenDental{
	partial class FormEtrans835ClaimSelect {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEtrans835ClaimSelect));
			this.textPatient = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.gridClaims = new OpenDental.UI.ODGrid();
			this.textDateTo = new ODR.ValidDate();
			this.textDateFrom = new ODR.ValidDate();
			this.textClaimFee = new OpenDental.ValidDouble();
			this.butPatFind = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butRefresh = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// textPatient
			// 
			this.textPatient.Location = new System.Drawing.Point(87, 12);
			this.textPatient.Name = "textPatient";
			this.textPatient.ReadOnly = true;
			this.textPatient.Size = new System.Drawing.Size(155, 20);
			this.textPatient.TabIndex = 161;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 13);
			this.label1.TabIndex = 160;
			this.label1.Text = "Patient";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(251, 33);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(114, 21);
			this.label3.TabIndex = 168;
			this.label3.Text = "Date To";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(248, 12);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(117, 21);
			this.label2.TabIndex = 166;
			this.label2.Text = "Date From";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(454, 12);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(81, 21);
			this.label4.TabIndex = 169;
			this.label4.Text = "Claim Fee";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// gridClaims
			// 
			this.gridClaims.AllowSortingByColumn = true;
			this.gridClaims.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridClaims.HasMultilineHeaders = false;
			this.gridClaims.HScrollVisible = false;
			this.gridClaims.Location = new System.Drawing.Point(19, 63);
			this.gridClaims.Name = "gridClaims";
			this.gridClaims.ScrollValue = 0;
			this.gridClaims.Size = new System.Drawing.Size(744, 453);
			this.gridClaims.TabIndex = 171;
			this.gridClaims.Title = "Claims";
			this.gridClaims.TranslationName = null;
			this.gridClaims.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridClaims_CellDoubleClick);
			// 
			// textDateTo
			// 
			this.textDateTo.Location = new System.Drawing.Point(366, 33);
			this.textDateTo.Name = "textDateTo";
			this.textDateTo.Size = new System.Drawing.Size(81, 20);
			this.textDateTo.TabIndex = 5;
			// 
			// textDateFrom
			// 
			this.textDateFrom.Location = new System.Drawing.Point(366, 12);
			this.textDateFrom.Name = "textDateFrom";
			this.textDateFrom.Size = new System.Drawing.Size(81, 20);
			this.textDateFrom.TabIndex = 4;
			// 
			// textClaimFee
			// 
			this.textClaimFee.Location = new System.Drawing.Point(536, 12);
			this.textClaimFee.MaxVal = 100000000D;
			this.textClaimFee.MinVal = -100000000D;
			this.textClaimFee.Name = "textClaimFee";
			this.textClaimFee.Size = new System.Drawing.Size(84, 20);
			this.textClaimFee.TabIndex = 6;
			// 
			// butPatFind
			// 
			this.butPatFind.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPatFind.Autosize = true;
			this.butPatFind.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPatFind.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPatFind.CornerRadius = 4F;
			this.butPatFind.Location = new System.Drawing.Point(87, 33);
			this.butPatFind.Name = "butPatFind";
			this.butPatFind.Size = new System.Drawing.Size(63, 24);
			this.butPatFind.TabIndex = 2;
			this.butPatFind.Text = "Find";
			this.butPatFind.Click += new System.EventHandler(this.butPatFind_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(607, 528);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 7;
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
			this.butCancel.Location = new System.Drawing.Point(688, 528);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 8;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(688, 9);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 24);
			this.butRefresh.TabIndex = 172;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// FormEtrans835ClaimSelect
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(784, 562);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.gridClaims);
			this.Controls.Add(this.textClaimFee);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textDateTo);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textDateFrom);
			this.Controls.Add(this.butPatFind);
			this.Controls.Add(this.textPatient);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(100, 100);
			this.Name = "FormEtrans835ClaimSelect";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ERA 835 Claim Select";
			this.Load += new System.EventHandler(this.FormEtrans835ClaimSelect_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private UI.Button butPatFind;
		private System.Windows.Forms.TextBox textPatient;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private ODR.ValidDate textDateTo;
		private System.Windows.Forms.Label label2;
		private ODR.ValidDate textDateFrom;
		private System.Windows.Forms.Label label4;
		private ValidDouble textClaimFee;
		private UI.ODGrid gridClaims;
		private UI.Button butRefresh;
	}
}