namespace OpenDental{
	partial class FormSmsTextMessaging {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSmsTextMessaging));
			this.labelClinic = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.checkShowHidden = new System.Windows.Forms.CheckBox();
			this.gridMessages = new OpenDental.UI.ODGrid();
			this.contextMenuMessages = new System.Windows.Forms.ContextMenu();
			this.menuItemChangePat = new System.Windows.Forms.MenuItem();
			this.menuItemMarkUnread = new System.Windows.Forms.MenuItem();
			this.menuItemMarkRead = new System.Windows.Forms.MenuItem();
			this.menuItemMarkJunk = new System.Windows.Forms.MenuItem();
			this.menuItemHide = new System.Windows.Forms.MenuItem();
			this.menuItemUnhide = new System.Windows.Forms.MenuItem();
			this.textPatient = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textDateTo = new ODR.ValidDate();
			this.textDateFrom = new ODR.ValidDate();
			this.comboStatus = new OpenDental.UI.ComboBoxMulti();
			this.butPatCurrent = new OpenDental.UI.Button();
			this.butPatAll = new OpenDental.UI.Button();
			this.butPatFind = new OpenDental.UI.Button();
			this.comboClinic = new OpenDental.UI.ComboBoxMulti();
			this.butRefresh = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(492, 32);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(111, 21);
			this.labelClinic.TabIndex = 6;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelClinic.Visible = false;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(15, 10);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(81, 21);
			this.label2.TabIndex = 8;
			this.label2.Text = "Date From";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(12, 31);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(84, 21);
			this.label3.TabIndex = 10;
			this.label3.Text = "Date To";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(489, 8);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(114, 21);
			this.label4.TabIndex = 12;
			this.label4.Text = "Type and Status";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkShowHidden
			// 
			this.checkShowHidden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkShowHidden.Location = new System.Drawing.Point(838, 10);
			this.checkShowHidden.Name = "checkShowHidden";
			this.checkShowHidden.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkShowHidden.Size = new System.Drawing.Size(124, 16);
			this.checkShowHidden.TabIndex = 153;
			this.checkShowHidden.Text = "Show Hidden";
			this.checkShowHidden.UseVisualStyleBackColor = true;
			// 
			// gridMessages
			// 
			this.gridMessages.AllowSortingByColumn = true;
			this.gridMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMessages.HasMultilineHeaders = false;
			this.gridMessages.HScrollVisible = false;
			this.gridMessages.Location = new System.Drawing.Point(12, 63);
			this.gridMessages.Name = "gridMessages";
			this.gridMessages.ScrollValue = 0;
			this.gridMessages.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMessages.Size = new System.Drawing.Size(950, 591);
			this.gridMessages.TabIndex = 4;
			this.gridMessages.Title = "Text Messages - Right click for options";
			this.gridMessages.TranslationName = null;
			// 
			// contextMenuMessages
			// 
			this.contextMenuMessages.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemChangePat,
            this.menuItemMarkUnread,
            this.menuItemMarkRead,
            this.menuItemMarkJunk,
            this.menuItemHide,
            this.menuItemUnhide});
			// 
			// menuItemChangePat
			// 
			this.menuItemChangePat.Index = 0;
			this.menuItemChangePat.Text = "Change Pat";
			this.menuItemChangePat.Click += new System.EventHandler(this.menuItemChangePat_Click);
			// 
			// menuItemMarkUnread
			// 
			this.menuItemMarkUnread.Index = 1;
			this.menuItemMarkUnread.Text = "Mark Unread";
			this.menuItemMarkUnread.Click += new System.EventHandler(this.menuItemMarkUnread_Click);
			// 
			// menuItemMarkRead
			// 
			this.menuItemMarkRead.Index = 2;
			this.menuItemMarkRead.Text = "Mark Read";
			this.menuItemMarkRead.Click += new System.EventHandler(this.menuItemMarkRead_Click);
			// 
			// menuItemMarkJunk
			// 
			this.menuItemMarkJunk.Index = 3;
			this.menuItemMarkJunk.Text = "Mark Junk";
			this.menuItemMarkJunk.Click += new System.EventHandler(this.menuItemMarkJunk_Click);
			// 
			// menuItemHide
			// 
			this.menuItemHide.Index = 4;
			this.menuItemHide.Text = "Hide";
			this.menuItemHide.Click += new System.EventHandler(this.menuItemHide_Click);
			// 
			// menuItemUnhide
			// 
			this.menuItemUnhide.Index = 5;
			this.menuItemUnhide.Text = "Unhide";
			this.menuItemUnhide.Click += new System.EventHandler(this.menuItemUnhide_Click);
			// 
			// textPatient
			// 
			this.textPatient.Location = new System.Drawing.Point(266, 10);
			this.textPatient.Name = "textPatient";
			this.textPatient.ReadOnly = true;
			this.textPatient.Size = new System.Drawing.Size(216, 20);
			this.textPatient.TabIndex = 156;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(181, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(84, 13);
			this.label1.TabIndex = 155;
			this.label1.Text = "Patient";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDateTo
			// 
			this.textDateTo.Location = new System.Drawing.Point(97, 31);
			this.textDateTo.Name = "textDateTo";
			this.textDateTo.Size = new System.Drawing.Size(81, 20);
			this.textDateTo.TabIndex = 9;
			// 
			// textDateFrom
			// 
			this.textDateFrom.Location = new System.Drawing.Point(97, 10);
			this.textDateFrom.Name = "textDateFrom";
			this.textDateFrom.Size = new System.Drawing.Size(81, 20);
			this.textDateFrom.TabIndex = 7;
			// 
			// comboStatus
			// 
			this.comboStatus.BackColor = System.Drawing.SystemColors.Window;
			this.comboStatus.DroppedDown = false;
			this.comboStatus.Items = ((System.Collections.ArrayList)(resources.GetObject("comboStatus.Items")));
			this.comboStatus.Location = new System.Drawing.Point(604, 10);
			this.comboStatus.Name = "comboStatus";
			this.comboStatus.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboStatus.SelectedIndices")));
			this.comboStatus.Size = new System.Drawing.Size(225, 21);
			this.comboStatus.TabIndex = 160;
			this.comboStatus.UseCommas = false;
			// 
			// butPatCurrent
			// 
			this.butPatCurrent.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPatCurrent.Autosize = true;
			this.butPatCurrent.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPatCurrent.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPatCurrent.CornerRadius = 4F;
			this.butPatCurrent.Location = new System.Drawing.Point(266, 31);
			this.butPatCurrent.Name = "butPatCurrent";
			this.butPatCurrent.Size = new System.Drawing.Size(63, 24);
			this.butPatCurrent.TabIndex = 159;
			this.butPatCurrent.Text = "Current";
			this.butPatCurrent.Click += new System.EventHandler(this.butPatCurrent_Click);
			// 
			// butPatAll
			// 
			this.butPatAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPatAll.Autosize = true;
			this.butPatAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPatAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPatAll.CornerRadius = 4F;
			this.butPatAll.Location = new System.Drawing.Point(419, 31);
			this.butPatAll.Name = "butPatAll";
			this.butPatAll.Size = new System.Drawing.Size(63, 24);
			this.butPatAll.TabIndex = 158;
			this.butPatAll.Text = "All";
			this.butPatAll.Click += new System.EventHandler(this.butPatAll_Click);
			// 
			// butPatFind
			// 
			this.butPatFind.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPatFind.Autosize = true;
			this.butPatFind.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPatFind.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPatFind.CornerRadius = 4F;
			this.butPatFind.Location = new System.Drawing.Point(342, 31);
			this.butPatFind.Name = "butPatFind";
			this.butPatFind.Size = new System.Drawing.Size(63, 24);
			this.butPatFind.TabIndex = 157;
			this.butPatFind.Text = "Find";
			this.butPatFind.Click += new System.EventHandler(this.butPatFind_Click);
			// 
			// comboClinic
			// 
			this.comboClinic.BackColor = System.Drawing.SystemColors.Window;
			this.comboClinic.DroppedDown = false;
			this.comboClinic.Items = ((System.Collections.ArrayList)(resources.GetObject("comboClinic.Items")));
			this.comboClinic.Location = new System.Drawing.Point(604, 32);
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboClinic.SelectedIndices")));
			this.comboClinic.Size = new System.Drawing.Size(225, 21);
			this.comboClinic.TabIndex = 154;
			this.comboClinic.UseCommas = false;
			this.comboClinic.Visible = false;
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(887, 31);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 24);
			this.butRefresh.TabIndex = 13;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(887, 660);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 2;
			this.butClose.Text = "Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// FormSmsTextMessaging
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(974, 696);
			this.Controls.Add(this.comboStatus);
			this.Controls.Add(this.butPatCurrent);
			this.Controls.Add(this.butPatAll);
			this.Controls.Add(this.butPatFind);
			this.Controls.Add(this.textPatient);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.checkShowHidden);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textDateTo);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textDateFrom);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.gridMessages);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(990, 734);
			this.Name = "FormSmsTextMessaging";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Text Messaging";
			this.Load += new System.EventHandler(this.FormSmsTextMessaging_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butClose;
		private UI.ODGrid gridMessages;
		private System.Windows.Forms.Label labelClinic;
		private ODR.ValidDate textDateFrom;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private ODR.ValidDate textDateTo;
		private System.Windows.Forms.Label label4;
		private UI.Button butRefresh;
		private System.Windows.Forms.CheckBox checkShowHidden;
		private System.Windows.Forms.ContextMenu contextMenuMessages;
		private System.Windows.Forms.MenuItem menuItemChangePat;
		private System.Windows.Forms.MenuItem menuItemMarkUnread;
		private System.Windows.Forms.MenuItem menuItemMarkRead;
		private System.Windows.Forms.MenuItem menuItemMarkJunk;
		private System.Windows.Forms.MenuItem menuItemHide;
		private System.Windows.Forms.MenuItem menuItemUnhide;
		private UI.ComboBoxMulti comboClinic;
		private UI.Button butPatCurrent;
		private UI.Button butPatAll;
		private UI.Button butPatFind;
		private System.Windows.Forms.TextBox textPatient;
		private System.Windows.Forms.Label label1;
		private UI.ComboBoxMulti comboStatus;
	}
}