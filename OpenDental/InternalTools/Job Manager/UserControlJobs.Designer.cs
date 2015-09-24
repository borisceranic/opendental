namespace OpenDental {
	partial class UserControlJobs {
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
			this.components = new System.ComponentModel.Container();
			this.textJobNum = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.comboType = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.comboPriority = new System.Windows.Forms.ComboBox();
			this.labelSite = new System.Windows.Forms.Label();
			this.comboStatus = new System.Windows.Forms.ComboBox();
			this.textTitle = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.checkShowHidden = new System.Windows.Forms.CheckBox();
			this.label11 = new System.Windows.Forms.Label();
			this.textProject = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.textVersion = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textOwner = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textExpert = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.timerSearch = new System.Windows.Forms.Timer(this.components);
			this.butAdd = new OpenDental.UI.Button();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// textJobNum
			// 
			this.textJobNum.Location = new System.Drawing.Point(78, 18);
			this.textJobNum.Name = "textJobNum";
			this.textJobNum.Size = new System.Drawing.Size(171, 20);
			this.textJobNum.TabIndex = 0;
			this.textJobNum.TextChanged += new System.EventHandler(this.textJobNum_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 18);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(70, 20);
			this.label2.TabIndex = 0;
			this.label2.Text = "JobNum";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboType
			// 
			this.comboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboType.Location = new System.Drawing.Point(78, 188);
			this.comboType.MaxDropDownItems = 40;
			this.comboType.Name = "comboType";
			this.comboType.Size = new System.Drawing.Size(171, 21);
			this.comboType.TabIndex = 8;
			this.comboType.SelectedIndexChanged += new System.EventHandler(this.comboType_SelectedIndexChanged);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(8, 188);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(70, 20);
			this.labelClinic.TabIndex = 0;
			this.labelClinic.Text = "Type";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboPriority
			// 
			this.comboPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPriority.Location = new System.Drawing.Point(78, 166);
			this.comboPriority.MaxDropDownItems = 40;
			this.comboPriority.Name = "comboPriority";
			this.comboPriority.Size = new System.Drawing.Size(171, 21);
			this.comboPriority.TabIndex = 7;
			this.comboPriority.SelectedIndexChanged += new System.EventHandler(this.comboPriority_SelectedIndexChanged);
			// 
			// labelSite
			// 
			this.labelSite.Location = new System.Drawing.Point(8, 166);
			this.labelSite.Name = "labelSite";
			this.labelSite.Size = new System.Drawing.Size(70, 20);
			this.labelSite.TabIndex = 0;
			this.labelSite.Text = "Priority";
			this.labelSite.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboStatus
			// 
			this.comboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboStatus.FormattingEnabled = true;
			this.comboStatus.Location = new System.Drawing.Point(78, 144);
			this.comboStatus.Name = "comboStatus";
			this.comboStatus.Size = new System.Drawing.Size(171, 21);
			this.comboStatus.TabIndex = 6;
			this.comboStatus.SelectedIndexChanged += new System.EventHandler(this.comboStatus_SelectedIndexChanged);
			// 
			// textTitle
			// 
			this.textTitle.Location = new System.Drawing.Point(78, 123);
			this.textTitle.Name = "textTitle";
			this.textTitle.Size = new System.Drawing.Size(171, 20);
			this.textTitle.TabIndex = 5;
			this.textTitle.TextChanged += new System.EventHandler(this.textTitle_TextChanged);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 122);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(70, 20);
			this.label7.TabIndex = 0;
			this.label7.Text = "Title";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkShowHidden
			// 
			this.checkShowHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowHidden.Location = new System.Drawing.Point(8, 215);
			this.checkShowHidden.Name = "checkShowHidden";
			this.checkShowHidden.Size = new System.Drawing.Size(159, 20);
			this.checkShowHidden.TabIndex = 9;
			this.checkShowHidden.Text = "Show Hidden Jobs";
			this.checkShowHidden.CheckedChanged += new System.EventHandler(this.checkShowHidden_CheckedChanged);
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(8, 144);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(70, 20);
			this.label11.TabIndex = 0;
			this.label11.Text = "Status";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textProject
			// 
			this.textProject.Location = new System.Drawing.Point(78, 102);
			this.textProject.Name = "textProject";
			this.textProject.Size = new System.Drawing.Size(171, 20);
			this.textProject.TabIndex = 4;
			this.textProject.TextChanged += new System.EventHandler(this.textProject_TextChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 102);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(70, 20);
			this.label5.TabIndex = 0;
			this.label5.Text = "Project";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textVersion
			// 
			this.textVersion.Location = new System.Drawing.Point(78, 81);
			this.textVersion.Name = "textVersion";
			this.textVersion.Size = new System.Drawing.Size(171, 20);
			this.textVersion.TabIndex = 3;
			this.textVersion.TextChanged += new System.EventHandler(this.textVersion_TextChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 80);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(70, 20);
			this.label4.TabIndex = 0;
			this.label4.Text = "Version";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textOwner
			// 
			this.textOwner.Location = new System.Drawing.Point(78, 60);
			this.textOwner.Name = "textOwner";
			this.textOwner.Size = new System.Drawing.Size(171, 20);
			this.textOwner.TabIndex = 2;
			this.textOwner.TextChanged += new System.EventHandler(this.textOwner_TextChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 61);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(70, 20);
			this.label3.TabIndex = 0;
			this.label3.Text = "Owner";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textExpert
			// 
			this.textExpert.Location = new System.Drawing.Point(78, 39);
			this.textExpert.Name = "textExpert";
			this.textExpert.Size = new System.Drawing.Size(171, 20);
			this.textExpert.TabIndex = 1;
			this.textExpert.TextChanged += new System.EventHandler(this.textExpert_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 39);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Expert";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.textJobNum);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.comboType);
			this.groupBox2.Controls.Add(this.labelClinic);
			this.groupBox2.Controls.Add(this.comboPriority);
			this.groupBox2.Controls.Add(this.labelSite);
			this.groupBox2.Controls.Add(this.comboStatus);
			this.groupBox2.Controls.Add(this.label11);
			this.groupBox2.Controls.Add(this.textTitle);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.checkShowHidden);
			this.groupBox2.Controls.Add(this.textProject);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.textVersion);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.textOwner);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.textExpert);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(532, 35);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(279, 240);
			this.groupBox2.TabIndex = 13;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Search by:";
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(4, 5);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(522, 270);
			this.gridMain.TabIndex = 11;
			this.gridMain.TabStop = false;
			this.gridMain.Title = "Jobs";
			this.gridMain.TranslationName = null;
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// timerSearch
			// 
			this.timerSearch.Interval = 500;
			this.timerSearch.Tick += new System.EventHandler(this.timerSearch_Tick);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Location = new System.Drawing.Point(532, 5);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(75, 24);
			this.butAdd.TabIndex = 15;
			this.butAdd.Text = "Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// UserControlJobs
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.gridMain);
			this.MinimumSize = new System.Drawing.Size(450, 290);
			this.Name = "UserControlJobs";
			this.Size = new System.Drawing.Size(815, 290);
			this.Load += new System.EventHandler(this.UserControlJob_Load);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox textJobNum;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox comboType;
		private System.Windows.Forms.Label labelClinic;
		private System.Windows.Forms.ComboBox comboPriority;
		private System.Windows.Forms.Label labelSite;
		private System.Windows.Forms.ComboBox comboStatus;
		private System.Windows.Forms.TextBox textTitle;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.CheckBox checkShowHidden;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox textProject;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textVersion;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textOwner;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textExpert;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox2;
		private UI.ODGrid gridMain;
		private System.Windows.Forms.Timer timerSearch;
		private UI.Button butAdd;


	}
}
