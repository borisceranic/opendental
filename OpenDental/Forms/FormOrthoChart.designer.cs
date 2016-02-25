namespace OpenDental{
	partial class FormOrthoChart {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOrthoChart));
			this.signatureBoxWrapper = new OpenDental.UI.SignatureBoxWrapper();
			this.butOK = new OpenDental.UI.Button();
			this.butAudit = new OpenDental.UI.Button();
			this.butUseAutoNote = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.gridPat = new OpenDental.UI.ODGrid();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.SuspendLayout();
			// 
			// signatureBoxWrapper
			// 
			this.signatureBoxWrapper.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.signatureBoxWrapper.BackColor = System.Drawing.SystemColors.ControlDark;
			this.signatureBoxWrapper.Location = new System.Drawing.Point(562, 427);
			this.signatureBoxWrapper.Name = "signatureBoxWrapper";
			this.signatureBoxWrapper.SignatureMode = OpenDental.UI.SignatureBoxWrapper.SigMode.OrthoChart;
			this.signatureBoxWrapper.Size = new System.Drawing.Size(364, 81);
			this.signatureBoxWrapper.TabIndex = 110;
			this.signatureBoxWrapper.ClearSignatureClicked += new System.EventHandler(this.signatureBoxWrapper_ClearSignatureClicked);
			this.signatureBoxWrapper.SignTopazClicked += new System.EventHandler(this.signatureBoxWrapper_SignTopazClicked);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(770, 634);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 23);
			this.butOK.TabIndex = 109;
			this.butOK.Text = "&OK";
			this.butOK.UseVisualStyleBackColor = true;
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butAudit
			// 
			this.butAudit.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAudit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAudit.Autosize = true;
			this.butAudit.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAudit.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAudit.CornerRadius = 4F;
			this.butAudit.Location = new System.Drawing.Point(473, 634);
			this.butAudit.Name = "butAudit";
			this.butAudit.Size = new System.Drawing.Size(80, 23);
			this.butAudit.TabIndex = 108;
			this.butAudit.Text = "Audit Trail";
			this.butAudit.Click += new System.EventHandler(this.butAudit_Click);
			// 
			// butUseAutoNote
			// 
			this.butUseAutoNote.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUseAutoNote.Autosize = false;
			this.butUseAutoNote.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUseAutoNote.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUseAutoNote.CornerRadius = 4F;
			this.butUseAutoNote.Location = new System.Drawing.Point(473, 427);
			this.butUseAutoNote.Name = "butUseAutoNote";
			this.butUseAutoNote.Size = new System.Drawing.Size(80, 23);
			this.butUseAutoNote.TabIndex = 107;
			this.butUseAutoNote.Text = "Auto Note";
			this.butUseAutoNote.Click += new System.EventHandler(this.butUseAutoNote_Click);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Autosize = false;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Location = new System.Drawing.Point(387, 427);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(80, 23);
			this.butAdd.TabIndex = 9;
			this.butAdd.Text = "Add Date";
			this.butAdd.UseVisualStyleBackColor = true;
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(851, 634);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 23);
			this.butCancel.TabIndex = 7;
			this.butCancel.Text = "&Cancel";
			this.butCancel.UseVisualStyleBackColor = true;
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// gridPat
			// 
			this.gridPat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridPat.HasAddButton = false;
			this.gridPat.HasMultilineHeaders = false;
			this.gridPat.HScrollVisible = false;
			this.gridPat.Location = new System.Drawing.Point(10, 427);
			this.gridPat.Name = "gridPat";
			this.gridPat.ScrollValue = 0;
			this.gridPat.Size = new System.Drawing.Size(371, 230);
			this.gridPat.TabIndex = 6;
			this.gridPat.Title = "Patient Fields";
			this.gridPat.TranslationName = "TablePatientFields";
			this.gridPat.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridPat_CellDoubleClick);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = true;
			this.gridMain.Location = new System.Drawing.Point(10, 12);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridMain.Size = new System.Drawing.Size(916, 409);
			this.gridMain.TabIndex = 5;
			this.gridMain.Title = "Ortho Chart";
			this.gridMain.TranslationName = "TableOrthoChart";
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			this.gridMain.CellTextChanged += new System.EventHandler(this.gridMain_CellTextChanged);
			this.gridMain.CellLeave += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellLeave);
			this.gridMain.CellEnter += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellEnter);
			// 
			// FormOrthoChart
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(938, 665);
			this.Controls.Add(this.signatureBoxWrapper);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butAudit);
			this.Controls.Add(this.butUseAutoNote);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.gridPat);
			this.Controls.Add(this.gridMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormOrthoChart";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Ortho Chart";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormOrthoChart_FormClosing);
			this.Load += new System.EventHandler(this.FormOrthoChart_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private UI.Button butAdd;
		private UI.Button butCancel;
		private UI.ODGrid gridPat;
		private UI.ODGrid gridMain;
		private UI.Button butUseAutoNote;
		private UI.Button butAudit;
		private UI.Button butOK;
		private UI.SignatureBoxWrapper signatureBoxWrapper;
	}
}