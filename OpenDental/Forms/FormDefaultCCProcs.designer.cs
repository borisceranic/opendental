namespace OpenDental{
	partial class FormDefaultCCProcs {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDefaultCCProcs));
			this.listProcs = new System.Windows.Forms.ListBox();
			this.label15 = new System.Windows.Forms.Label();
			this.butRemoveProc = new OpenDental.UI.Button();
			this.butAddProc = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// listProcs
			// 
			this.listProcs.FormattingEnabled = true;
			this.listProcs.Location = new System.Drawing.Point(15, 58);
			this.listProcs.Name = "listProcs";
			this.listProcs.Size = new System.Drawing.Size(220, 134);
			this.listProcs.TabIndex = 134;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(12, 12);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(304, 33);
			this.label15.TabIndex = 133;
			this.label15.Text = "Procedures that will be authorized for recurring charges when a new credit card i" +
    "s added.";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butRemoveProc
			// 
			this.butRemoveProc.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRemoveProc.Autosize = true;
			this.butRemoveProc.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRemoveProc.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRemoveProc.CornerRadius = 4F;
			this.butRemoveProc.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butRemoveProc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butRemoveProc.Location = new System.Drawing.Point(242, 88);
			this.butRemoveProc.Name = "butRemoveProc";
			this.butRemoveProc.Size = new System.Drawing.Size(78, 24);
			this.butRemoveProc.TabIndex = 136;
			this.butRemoveProc.Text = "Remove";
			this.butRemoveProc.Click += new System.EventHandler(this.butRemoveProc_Click);
			// 
			// butAddProc
			// 
			this.butAddProc.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddProc.Autosize = true;
			this.butAddProc.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddProc.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddProc.CornerRadius = 4F;
			this.butAddProc.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddProc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddProc.Location = new System.Drawing.Point(242, 58);
			this.butAddProc.Name = "butAddProc";
			this.butAddProc.Size = new System.Drawing.Size(78, 24);
			this.butAddProc.TabIndex = 135;
			this.butAddProc.Text = "Add";
			this.butAddProc.Click += new System.EventHandler(this.butAddProc_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(242, 138);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(78, 24);
			this.butOK.TabIndex = 3;
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
			this.butCancel.Location = new System.Drawing.Point(242, 168);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(78, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormDefaultCCProcs
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(329, 202);
			this.Controls.Add(this.listProcs);
			this.Controls.Add(this.butRemoveProc);
			this.Controls.Add(this.butAddProc);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(345, 241);
			this.Name = "FormDefaultCCProcs";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Default Procedures";
			this.Load += new System.EventHandler(this.FormDefaultCCProcs_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.ListBox listProcs;
		private UI.Button butRemoveProc;
		private System.Windows.Forms.Label label15;
		private UI.Button butAddProc;
	}
}