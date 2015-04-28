namespace OpenDental {
	partial class FormMedLabs {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMedLabs));
			this.gridMain = new OpenDental.UI.ODGrid();
			this.labelMove = new System.Windows.Forms.Label();
			this.butClose = new OpenDental.UI.Button();
			this.butMove = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 12);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(860, 256);
			this.gridMain.TabIndex = 5;
			this.gridMain.Title = "Labs";
			this.gridMain.TranslationName = null;
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// labelMove
			// 
			this.labelMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelMove.Location = new System.Drawing.Point(88, 275);
			this.labelMove.Name = "labelMove";
			this.labelMove.Size = new System.Drawing.Size(330, 27);
			this.labelMove.TabIndex = 15;
			this.labelMove.Text = "Move a lab result to a different patient by first selecting the result from the l" +
    "ist above, and then selecting the correct patient.";
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(797, 277);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 335;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butMove
			// 
			this.butMove.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butMove.Autosize = true;
			this.butMove.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMove.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMove.CornerRadius = 4F;
			this.butMove.Location = new System.Drawing.Point(12, 277);
			this.butMove.Name = "butMove";
			this.butMove.Size = new System.Drawing.Size(75, 24);
			this.butMove.TabIndex = 336;
			this.butMove.Text = "Move";
			this.butMove.Click += new System.EventHandler(this.butMove_Click);
			// 
			// FormMedLabs
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(884, 312);
			this.Controls.Add(this.butMove);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.labelMove);
			this.Controls.Add(this.gridMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(774, 156);
			this.Name = "FormMedLabs";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Medical Labs";
			this.Load += new System.EventHandler(this.FormMedLabs_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.ODGrid gridMain;
		private System.Windows.Forms.Label labelMove;
		private UI.Button butClose;
		private UI.Button butMove;
	}
}