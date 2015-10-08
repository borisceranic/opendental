namespace OpenDental {
	partial class UserControlManage {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlManage));
			this.groupDetails = new System.Windows.Forms.GroupBox();
			this.labelActualHrs = new System.Windows.Forms.Label();
			this.labelEstHrs = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.groupSummary = new System.Windows.Forms.GroupBox();
			this.labelOwnerJobs = new System.Windows.Forms.Label();
			this.labelOwnerHrs = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkStatus = new System.Windows.Forms.CheckBox();
			this.checkDate = new System.Windows.Forms.CheckBox();
			this.checkOwner = new System.Windows.Forms.CheckBox();
			this.checkExpert = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.comboBoxMultiOwner = new OpenDental.UI.ComboBoxMulti();
			this.comboBoxMultiStatus = new OpenDental.UI.ComboBoxMulti();
			this.comboBoxMultiExpert = new OpenDental.UI.ComboBoxMulti();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.labelExpertJobs = new System.Windows.Forms.Label();
			this.labelExpertHrs = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butRefresh = new OpenDental.UI.Button();
			this.groupDetails.SuspendLayout();
			this.groupSummary.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupDetails
			// 
			this.groupDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupDetails.Controls.Add(this.labelActualHrs);
			this.groupDetails.Controls.Add(this.labelEstHrs);
			this.groupDetails.Controls.Add(this.label8);
			this.groupDetails.Controls.Add(this.label9);
			this.groupDetails.Location = new System.Drawing.Point(763, 105);
			this.groupDetails.Name = "groupDetails";
			this.groupDetails.Size = new System.Drawing.Size(223, 82);
			this.groupDetails.TabIndex = 12;
			this.groupDetails.TabStop = false;
			this.groupDetails.Text = "Job Details";
			// 
			// labelActualHrs
			// 
			this.labelActualHrs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelActualHrs.Location = new System.Drawing.Point(117, 43);
			this.labelActualHrs.Name = "labelActualHrs";
			this.labelActualHrs.Size = new System.Drawing.Size(100, 23);
			this.labelActualHrs.TabIndex = 9;
			this.labelActualHrs.Text = "0";
			this.labelActualHrs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelEstHrs
			// 
			this.labelEstHrs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelEstHrs.Location = new System.Drawing.Point(117, 20);
			this.labelEstHrs.Name = "labelEstHrs";
			this.labelEstHrs.Size = new System.Drawing.Size(100, 23);
			this.labelEstHrs.TabIndex = 8;
			this.labelEstHrs.Text = "0";
			this.labelEstHrs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 43);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(83, 23);
			this.label8.TabIndex = 7;
			this.label8.Text = "Actual Hours:";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(8, 20);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(68, 23);
			this.label9.TabIndex = 6;
			this.label9.Text = "Est Hours:";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupSummary
			// 
			this.groupSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupSummary.Controls.Add(this.labelOwnerJobs);
			this.groupSummary.Controls.Add(this.labelOwnerHrs);
			this.groupSummary.Controls.Add(this.label6);
			this.groupSummary.Controls.Add(this.label7);
			this.groupSummary.Location = new System.Drawing.Point(763, 278);
			this.groupSummary.Name = "groupSummary";
			this.groupSummary.Size = new System.Drawing.Size(223, 82);
			this.groupSummary.TabIndex = 13;
			this.groupSummary.TabStop = false;
			this.groupSummary.Text = "Owner Summary";
			// 
			// labelOwnerJobs
			// 
			this.labelOwnerJobs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelOwnerJobs.Location = new System.Drawing.Point(116, 43);
			this.labelOwnerJobs.Name = "labelOwnerJobs";
			this.labelOwnerJobs.Size = new System.Drawing.Size(100, 23);
			this.labelOwnerJobs.TabIndex = 13;
			this.labelOwnerJobs.Text = "0";
			this.labelOwnerJobs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelOwnerHrs
			// 
			this.labelOwnerHrs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelOwnerHrs.Location = new System.Drawing.Point(116, 20);
			this.labelOwnerHrs.Name = "labelOwnerHrs";
			this.labelOwnerHrs.Size = new System.Drawing.Size(100, 23);
			this.labelOwnerHrs.TabIndex = 12;
			this.labelOwnerHrs.Text = "0";
			this.labelOwnerHrs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 43);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(102, 23);
			this.label6.TabIndex = 3;
			this.label6.Text = "Incompl Jobs:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 20);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(102, 23);
			this.label7.TabIndex = 2;
			this.label7.Text = "Incompl Job Hrs:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.checkStatus);
			this.groupBox1.Controls.Add(this.checkDate);
			this.groupBox1.Controls.Add(this.checkOwner);
			this.groupBox1.Controls.Add(this.checkExpert);
			this.groupBox1.Location = new System.Drawing.Point(4, 4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(211, 95);
			this.groupBox1.TabIndex = 14;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Group By";
			// 
			// checkStatus
			// 
			this.checkStatus.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkStatus.Location = new System.Drawing.Point(110, 50);
			this.checkStatus.Name = "checkStatus";
			this.checkStatus.Size = new System.Drawing.Size(80, 24);
			this.checkStatus.TabIndex = 3;
			this.checkStatus.Text = "Status";
			this.checkStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkStatus.UseVisualStyleBackColor = true;
			// 
			// checkDate
			// 
			this.checkDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkDate.Location = new System.Drawing.Point(110, 20);
			this.checkDate.Name = "checkDate";
			this.checkDate.Size = new System.Drawing.Size(80, 24);
			this.checkDate.TabIndex = 2;
			this.checkDate.Text = "Date";
			this.checkDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkDate.UseVisualStyleBackColor = true;
			// 
			// checkOwner
			// 
			this.checkOwner.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkOwner.Location = new System.Drawing.Point(28, 50);
			this.checkOwner.Name = "checkOwner";
			this.checkOwner.Size = new System.Drawing.Size(80, 24);
			this.checkOwner.TabIndex = 1;
			this.checkOwner.Text = "Owner";
			this.checkOwner.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkOwner.UseVisualStyleBackColor = true;
			// 
			// checkExpert
			// 
			this.checkExpert.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkExpert.Location = new System.Drawing.Point(28, 20);
			this.checkExpert.Name = "checkExpert";
			this.checkExpert.Size = new System.Drawing.Size(80, 24);
			this.checkExpert.TabIndex = 0;
			this.checkExpert.Text = "Expert";
			this.checkExpert.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkExpert.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.comboBoxMultiOwner);
			this.groupBox2.Controls.Add(this.comboBoxMultiStatus);
			this.groupBox2.Controls.Add(this.comboBoxMultiExpert);
			this.groupBox2.Location = new System.Drawing.Point(221, 4);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(241, 95);
			this.groupBox2.TabIndex = 15;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Filter By";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(20, 62);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(68, 21);
			this.label3.TabIndex = 5;
			this.label3.Text = "Owner";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(20, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(68, 21);
			this.label2.TabIndex = 4;
			this.label2.Text = "Status";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(20, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(68, 21);
			this.label1.TabIndex = 3;
			this.label1.Text = "Expert";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboBoxMultiOwner
			// 
			this.comboBoxMultiOwner.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiOwner.DroppedDown = false;
			this.comboBoxMultiOwner.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiOwner.Items")));
			this.comboBoxMultiOwner.Location = new System.Drawing.Point(89, 62);
			this.comboBoxMultiOwner.Name = "comboBoxMultiOwner";
			this.comboBoxMultiOwner.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiOwner.SelectedIndices")));
			this.comboBoxMultiOwner.Size = new System.Drawing.Size(120, 21);
			this.comboBoxMultiOwner.TabIndex = 2;
			// 
			// comboBoxMultiStatus
			// 
			this.comboBoxMultiStatus.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiStatus.DroppedDown = false;
			this.comboBoxMultiStatus.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiStatus.Items")));
			this.comboBoxMultiStatus.Location = new System.Drawing.Point(89, 40);
			this.comboBoxMultiStatus.Name = "comboBoxMultiStatus";
			this.comboBoxMultiStatus.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiStatus.SelectedIndices")));
			this.comboBoxMultiStatus.Size = new System.Drawing.Size(120, 21);
			this.comboBoxMultiStatus.TabIndex = 1;
			// 
			// comboBoxMultiExpert
			// 
			this.comboBoxMultiExpert.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiExpert.DroppedDown = false;
			this.comboBoxMultiExpert.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiExpert.Items")));
			this.comboBoxMultiExpert.Location = new System.Drawing.Point(89, 18);
			this.comboBoxMultiExpert.Name = "comboBoxMultiExpert";
			this.comboBoxMultiExpert.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiExpert.SelectedIndices")));
			this.comboBoxMultiExpert.Size = new System.Drawing.Size(120, 21);
			this.comboBoxMultiExpert.TabIndex = 0;
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.labelExpertJobs);
			this.groupBox3.Controls.Add(this.labelExpertHrs);
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Controls.Add(this.label4);
			this.groupBox3.Location = new System.Drawing.Point(763, 193);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(223, 82);
			this.groupBox3.TabIndex = 14;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Expert Summary";
			// 
			// labelExpertJobs
			// 
			this.labelExpertJobs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelExpertJobs.Location = new System.Drawing.Point(117, 43);
			this.labelExpertJobs.Name = "labelExpertJobs";
			this.labelExpertJobs.Size = new System.Drawing.Size(100, 23);
			this.labelExpertJobs.TabIndex = 11;
			this.labelExpertJobs.Text = "0";
			this.labelExpertJobs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelExpertHrs
			// 
			this.labelExpertHrs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelExpertHrs.Location = new System.Drawing.Point(117, 20);
			this.labelExpertHrs.Name = "labelExpertHrs";
			this.labelExpertHrs.Size = new System.Drawing.Size(100, 23);
			this.labelExpertHrs.TabIndex = 10;
			this.labelExpertHrs.Text = "0";
			this.labelExpertHrs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 43);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(102, 23);
			this.label5.TabIndex = 1;
			this.label5.Text = "Incompl Jobs:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 20);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(102, 23);
			this.label4.TabIndex = 0;
			this.label4.Text = "Incompl Job Hrs:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// gridMain
			// 
			this.gridMain.AllowSortingByColumn = true;
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = true;
			this.gridMain.Location = new System.Drawing.Point(4, 105);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(753, 626);
			this.gridMain.TabIndex = 11;
			this.gridMain.TabStop = false;
			this.gridMain.Title = "Manage";
			this.gridMain.TranslationName = null;
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(468, 76);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 23);
			this.butRefresh.TabIndex = 16;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.UseVisualStyleBackColor = true;
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// UserControlManage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupSummary);
			this.Controls.Add(this.groupDetails);
			this.Controls.Add(this.gridMain);
			this.MinimumSize = new System.Drawing.Size(450, 290);
			this.Name = "UserControlManage";
			this.Size = new System.Drawing.Size(990, 734);
			this.Load += new System.EventHandler(this.UserControlManage_Load);
			this.groupDetails.ResumeLayout(false);
			this.groupSummary.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private UI.ODGrid gridMain;
		private System.Windows.Forms.GroupBox groupDetails;
		private System.Windows.Forms.GroupBox groupSummary;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private UI.ComboBoxMulti comboBoxMultiOwner;
		private UI.ComboBoxMulti comboBoxMultiStatus;
		private UI.ComboBoxMulti comboBoxMultiExpert;
		private UI.Button butRefresh;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox checkStatus;
		private System.Windows.Forms.CheckBox checkDate;
		private System.Windows.Forms.CheckBox checkOwner;
		private System.Windows.Forms.CheckBox checkExpert;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label labelActualHrs;
		private System.Windows.Forms.Label labelEstHrs;
		private System.Windows.Forms.Label labelOwnerJobs;
		private System.Windows.Forms.Label labelOwnerHrs;
		private System.Windows.Forms.Label labelExpertJobs;
		private System.Windows.Forms.Label labelExpertHrs;


	}
}
