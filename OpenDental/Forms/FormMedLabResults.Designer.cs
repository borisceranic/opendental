namespace OpenDental {
	partial class FormMedLabResults {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMedLabResults));
			this.butRetrieve = new System.Windows.Forms.Button();
			this.butClose = new System.Windows.Forms.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butMove = new System.Windows.Forms.Button();
			this.labelMove = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// butRetrieve
			// 
			this.butRetrieve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRetrieve.Location = new System.Drawing.Point(752, 12);
			this.butRetrieve.Name = "butRetrieve";
			this.butRetrieve.Size = new System.Drawing.Size(75, 23);
			this.butRetrieve.TabIndex = 7;
			this.butRetrieve.Text = "Retrieve";
			this.butRetrieve.UseVisualStyleBackColor = true;
			this.butRetrieve.Click += new System.EventHandler(this.butRetrieve_Click);
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Location = new System.Drawing.Point(752, 266);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 23);
			this.butClose.TabIndex = 9;
			this.butClose.Text = "Close";
			this.butClose.UseVisualStyleBackColor = true;
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
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
			this.gridMain.Size = new System.Drawing.Size(730, 245);
			this.gridMain.TabIndex = 5;
			this.gridMain.Title = "Laboratory Results";
			this.gridMain.TranslationName = null;
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butMove
			// 
			this.butMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butMove.Location = new System.Drawing.Point(12, 266);
			this.butMove.Name = "butMove";
			this.butMove.Size = new System.Drawing.Size(75, 23);
			this.butMove.TabIndex = 10;
			this.butMove.Text = "Move";
			this.butMove.UseVisualStyleBackColor = true;
			this.butMove.Click += new System.EventHandler(this.butMove_Click);
			// 
			// labelMove
			// 
			this.labelMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelMove.Location = new System.Drawing.Point(88, 264);
			this.labelMove.Name = "labelMove";
			this.labelMove.Size = new System.Drawing.Size(330, 27);
			this.labelMove.TabIndex = 15;
			this.labelMove.Text = "Move a lab result to a different patient by first selecting the result from the l" +
    "ist above, and then selecting the correct patient.";
			// 
			// FormMedLabResults
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(839, 301);
			this.Controls.Add(this.labelMove);
			this.Controls.Add(this.butMove);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.butRetrieve);
			this.Controls.Add(this.gridMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(855, 339);
			this.Name = "FormMedLabResults";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Medical Lab Results";
			this.Load += new System.EventHandler(this.FormMedLabResults_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button butRetrieve;
		private OpenDental.UI.ODGrid gridMain;
		private System.Windows.Forms.Button butClose;
		private System.Windows.Forms.Button butMove;
		private System.Windows.Forms.Label labelMove;
	}
}