namespace OpenDental{
	partial class FormPayPlanUpdate {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPayPlanUpdate));
			this.butUpdate = new OpenDental.UI.Button();
			this.butLater = new OpenDental.UI.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// butUpdate
			// 
			this.butUpdate.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butUpdate.Autosize = true;
			this.butUpdate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUpdate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUpdate.CornerRadius = 4F;
			this.butUpdate.Location = new System.Drawing.Point(207, 118);
			this.butUpdate.Name = "butUpdate";
			this.butUpdate.Size = new System.Drawing.Size(75, 24);
			this.butUpdate.TabIndex = 3;
			this.butUpdate.Text = "Update";
			this.butUpdate.Click += new System.EventHandler(this.butUpdate_Click);
			// 
			// butLater
			// 
			this.butLater.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butLater.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butLater.Autosize = true;
			this.butLater.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLater.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLater.CornerRadius = 4F;
			this.butLater.Location = new System.Drawing.Point(288, 118);
			this.butLater.Name = "butLater";
			this.butLater.Size = new System.Drawing.Size(75, 24);
			this.butLater.TabIndex = 2;
			this.butLater.Text = "Later";
			this.butLater.Click += new System.EventHandler(this.butLater_Click);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(12, 6);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(355, 109);
			this.label6.TabIndex = 16;
			this.label6.Text = resources.GetString("label6.Text");
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FormPayPlanUpdate
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(375, 148);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.butUpdate);
			this.Controls.Add(this.butLater);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormPayPlanUpdate";
			this.Text = "Update Payment Plans";
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butUpdate;
		private OpenDental.UI.Button butLater;
		private System.Windows.Forms.Label label6;
	}
}