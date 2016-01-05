namespace OpenDental{
	partial class FormJobSearch {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormJobSearch));
			this.gridMain = new OpenDental.UI.ODGrid();
			this.label2 = new System.Windows.Forms.Label();
			this.textTitle = new System.Windows.Forms.TextBox();
			this.listBoxExpert = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.listBoxOwner = new System.Windows.Forms.ListBox();
			this.label4 = new System.Windows.Forms.Label();
			this.listBoxStatus = new System.Windows.Forms.ListBox();
			this.label5 = new System.Windows.Forms.Label();
			this.textTask = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textFeatReq = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.textBug = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.textCust = new System.Windows.Forms.TextBox();
			this.butSearch = new OpenDental.UI.Button();
			this.label9 = new System.Windows.Forms.Label();
			this.listBoxCategory = new System.Windows.Forms.ListBox();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// gridMain
			// 
			this.gridMain.AllowSortingByColumn = true;
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = true;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 250);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(1022, 379);
			this.gridMain.TabIndex = 227;
			this.gridMain.Title = "Details Grid";
			this.gridMain.TranslationName = "";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Location = new System.Drawing.Point(9, 5);
			this.label2.Margin = new System.Windows.Forms.Padding(0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(55, 20);
			this.label2.TabIndex = 229;
			this.label2.Text = "Title";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textTitle
			// 
			this.textTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textTitle.Location = new System.Drawing.Point(12, 28);
			this.textTitle.Name = "textTitle";
			this.textTitle.Size = new System.Drawing.Size(212, 20);
			this.textTitle.TabIndex = 228;
			// 
			// listBoxExpert
			// 
			this.listBoxExpert.FormattingEnabled = true;
			this.listBoxExpert.Location = new System.Drawing.Point(230, 28);
			this.listBoxExpert.Name = "listBoxExpert";
			this.listBoxExpert.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBoxExpert.Size = new System.Drawing.Size(144, 212);
			this.listBoxExpert.TabIndex = 230;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(227, 5);
			this.label1.Margin = new System.Windows.Forms.Padding(0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(55, 20);
			this.label1.TabIndex = 231;
			this.label1.Text = "Expert";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Location = new System.Drawing.Point(377, 5);
			this.label3.Margin = new System.Windows.Forms.Padding(0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(55, 20);
			this.label3.TabIndex = 233;
			this.label3.Text = "Owner";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listBoxOwner
			// 
			this.listBoxOwner.FormattingEnabled = true;
			this.listBoxOwner.Location = new System.Drawing.Point(380, 28);
			this.listBoxOwner.Name = "listBoxOwner";
			this.listBoxOwner.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBoxOwner.Size = new System.Drawing.Size(144, 212);
			this.listBoxOwner.TabIndex = 232;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Location = new System.Drawing.Point(528, 5);
			this.label4.Margin = new System.Windows.Forms.Padding(0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(55, 20);
			this.label4.TabIndex = 235;
			this.label4.Text = "Status";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listBoxStatus
			// 
			this.listBoxStatus.FormattingEnabled = true;
			this.listBoxStatus.Location = new System.Drawing.Point(531, 28);
			this.listBoxStatus.Name = "listBoxStatus";
			this.listBoxStatus.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBoxStatus.Size = new System.Drawing.Size(144, 212);
			this.listBoxStatus.TabIndex = 234;
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.Location = new System.Drawing.Point(828, 5);
			this.label5.Margin = new System.Windows.Forms.Padding(0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(55, 20);
			this.label5.TabIndex = 237;
			this.label5.Text = "Task";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textTask
			// 
			this.textTask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textTask.Location = new System.Drawing.Point(831, 28);
			this.textTask.Name = "textTask";
			this.textTask.Size = new System.Drawing.Size(203, 20);
			this.textTask.TabIndex = 236;
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.Location = new System.Drawing.Point(828, 51);
			this.label6.Margin = new System.Windows.Forms.Padding(0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(119, 20);
			this.label6.TabIndex = 239;
			this.label6.Text = "Feature Request";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textFeatReq
			// 
			this.textFeatReq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textFeatReq.Location = new System.Drawing.Point(831, 74);
			this.textFeatReq.Name = "textFeatReq";
			this.textFeatReq.Size = new System.Drawing.Size(203, 20);
			this.textFeatReq.TabIndex = 238;
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label7.Location = new System.Drawing.Point(828, 97);
			this.label7.Margin = new System.Windows.Forms.Padding(0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(119, 20);
			this.label7.TabIndex = 241;
			this.label7.Text = "Bug";
			this.label7.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textBug
			// 
			this.textBug.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textBug.Location = new System.Drawing.Point(831, 120);
			this.textBug.Name = "textBug";
			this.textBug.Size = new System.Drawing.Size(203, 20);
			this.textBug.TabIndex = 240;
			// 
			// label8
			// 
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label8.Location = new System.Drawing.Point(828, 143);
			this.label8.Margin = new System.Windows.Forms.Padding(0);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(119, 20);
			this.label8.TabIndex = 243;
			this.label8.Text = "Customer";
			this.label8.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textCust
			// 
			this.textCust.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textCust.Location = new System.Drawing.Point(831, 166);
			this.textCust.Name = "textCust";
			this.textCust.Size = new System.Drawing.Size(203, 20);
			this.textCust.TabIndex = 242;
			// 
			// butSearch
			// 
			this.butSearch.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butSearch.Autosize = true;
			this.butSearch.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSearch.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSearch.CornerRadius = 4F;
			this.butSearch.Enabled = false;
			this.butSearch.Location = new System.Drawing.Point(954, 220);
			this.butSearch.Name = "butSearch";
			this.butSearch.Size = new System.Drawing.Size(80, 24);
			this.butSearch.TabIndex = 244;
			this.butSearch.Text = "Power Search";
			this.butSearch.Click += new System.EventHandler(this.butSearch_Click);
			// 
			// label9
			// 
			this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label9.Location = new System.Drawing.Point(678, 5);
			this.label9.Margin = new System.Windows.Forms.Padding(0);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(55, 20);
			this.label9.TabIndex = 246;
			this.label9.Text = "Category";
			this.label9.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listBoxCategory
			// 
			this.listBoxCategory.FormattingEnabled = true;
			this.listBoxCategory.Location = new System.Drawing.Point(681, 28);
			this.listBoxCategory.Name = "listBoxCategory";
			this.listBoxCategory.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBoxCategory.Size = new System.Drawing.Size(144, 212);
			this.listBoxCategory.TabIndex = 245;
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(954, 635);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(80, 24);
			this.butCancel.TabIndex = 247;
			this.butCancel.Text = "Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(867, 635);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(80, 24);
			this.butOK.TabIndex = 248;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// FormJobSearch
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1046, 671);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.listBoxCategory);
			this.Controls.Add(this.butSearch);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.textCust);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.textBug);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.textFeatReq);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textTask);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.listBoxStatus);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.listBoxOwner);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.listBoxExpert);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textTitle);
			this.Controls.Add(this.gridMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormJobSearch";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Search Jobs";
			this.Load += new System.EventHandler(this.FormJobNew_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private UI.ODGrid gridMain;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textTitle;
		private System.Windows.Forms.ListBox listBoxExpert;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ListBox listBoxOwner;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ListBox listBoxStatus;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textTask;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textFeatReq;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox textBug;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox textCust;
		private UI.Button butSearch;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ListBox listBoxCategory;
		private UI.Button butCancel;
		private UI.Button butOK;


	}
}