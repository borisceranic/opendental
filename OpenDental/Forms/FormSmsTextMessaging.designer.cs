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
			this.checkHidden = new System.Windows.Forms.CheckBox();
			this.contextMenuMessages = new System.Windows.Forms.ContextMenu();
			this.menuItemChangePat = new System.Windows.Forms.MenuItem();
			this.menuItemMarkUnread = new System.Windows.Forms.MenuItem();
			this.menuItemMarkRead = new System.Windows.Forms.MenuItem();
			this.menuItemHide = new System.Windows.Forms.MenuItem();
			this.menuItemUnhide = new System.Windows.Forms.MenuItem();
			this.textPatient = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.checkSent = new System.Windows.Forms.CheckBox();
			this.checkReceivedAndRead = new System.Windows.Forms.CheckBox();
			this.checkReceivedAndUnread = new System.Windows.Forms.CheckBox();
			this.gridMessageThread = new OpenDental.UI.ODGrid();
			this.gridMessages = new OpenDental.UI.ODGrid();
			this.textDateTo = new ODR.ValidDate();
			this.textDateFrom = new ODR.ValidDate();
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
			this.labelClinic.Location = new System.Drawing.Point(154, 61);
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
			// checkHidden
			// 
			this.checkHidden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkHidden.Location = new System.Drawing.Point(643, 11);
			this.checkHidden.Name = "checkHidden";
			this.checkHidden.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkHidden.Size = new System.Drawing.Size(150, 16);
			this.checkHidden.TabIndex = 153;
			this.checkHidden.Text = "Hidden";
			this.checkHidden.UseVisualStyleBackColor = true;
			// 
			// contextMenuMessages
			// 
			this.contextMenuMessages.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemChangePat,
            this.menuItemMarkUnread,
            this.menuItemMarkRead,
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
			// menuItemHide
			// 
			this.menuItemHide.Index = 3;
			this.menuItemHide.Text = "Hide";
			this.menuItemHide.Click += new System.EventHandler(this.menuItemHide_Click);
			// 
			// menuItemUnhide
			// 
			this.menuItemUnhide.Index = 4;
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
			// checkSent
			// 
			this.checkSent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkSent.Location = new System.Drawing.Point(493, 11);
			this.checkSent.Name = "checkSent";
			this.checkSent.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkSent.Size = new System.Drawing.Size(150, 16);
			this.checkSent.TabIndex = 161;
			this.checkSent.Text = "Sent";
			this.checkSent.UseVisualStyleBackColor = true;
			// 
			// checkReceivedAndRead
			// 
			this.checkReceivedAndRead.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkReceivedAndRead.Location = new System.Drawing.Point(493, 35);
			this.checkReceivedAndRead.Name = "checkReceivedAndRead";
			this.checkReceivedAndRead.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkReceivedAndRead.Size = new System.Drawing.Size(150, 16);
			this.checkReceivedAndRead.TabIndex = 162;
			this.checkReceivedAndRead.Text = "Received and Read";
			this.checkReceivedAndRead.UseVisualStyleBackColor = true;
			// 
			// checkReceivedAndUnread
			// 
			this.checkReceivedAndUnread.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkReceivedAndUnread.Location = new System.Drawing.Point(643, 35);
			this.checkReceivedAndUnread.Name = "checkReceivedAndUnread";
			this.checkReceivedAndUnread.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkReceivedAndUnread.Size = new System.Drawing.Size(150, 16);
			this.checkReceivedAndUnread.TabIndex = 163;
			this.checkReceivedAndUnread.Text = "Recieved and Unread";
			this.checkReceivedAndUnread.UseVisualStyleBackColor = true;
			// 
			// gridMessageThread
			// 
			this.gridMessageThread.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMessageThread.HasMultilineHeaders = false;
			this.gridMessageThread.HScrollVisible = false;
			this.gridMessageThread.Location = new System.Drawing.Point(712, 88);
			this.gridMessageThread.Name = "gridMessageThread";
			this.gridMessageThread.ScrollValue = 0;
			this.gridMessageThread.Size = new System.Drawing.Size(250, 566);
			this.gridMessageThread.TabIndex = 0;
			this.gridMessageThread.Title = "Message Thread";
			this.gridMessageThread.TranslationName = null;
			// 
			// gridMessages
			// 
			this.gridMessages.AllowSortingByColumn = true;
			this.gridMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMessages.HasMultilineHeaders = false;
			this.gridMessages.HScrollVisible = false;
			this.gridMessages.Location = new System.Drawing.Point(12, 88);
			this.gridMessages.Name = "gridMessages";
			this.gridMessages.ScrollValue = 0;
			this.gridMessages.Size = new System.Drawing.Size(698, 566);
			this.gridMessages.TabIndex = 4;
			this.gridMessages.Title = "Text Messages - Right click for options";
			this.gridMessages.TranslationName = null;
			this.gridMessages.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMessages_CellClick);
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
			this.comboClinic.Location = new System.Drawing.Point(266, 61);
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboClinic.SelectedIndices")));
			this.comboClinic.Size = new System.Drawing.Size(225, 21);
			this.comboClinic.TabIndex = 154;
			this.comboClinic.UseCommas = true;
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
			this.butRefresh.Location = new System.Drawing.Point(887, 59);
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
			this.Controls.Add(this.gridMessageThread);
			this.Controls.Add(this.gridMessages);
			this.Controls.Add(this.checkReceivedAndUnread);
			this.Controls.Add(this.checkReceivedAndRead);
			this.Controls.Add(this.checkSent);
			this.Controls.Add(this.butPatCurrent);
			this.Controls.Add(this.butPatAll);
			this.Controls.Add(this.butPatFind);
			this.Controls.Add(this.textPatient);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.checkHidden);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textDateTo);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textDateFrom);
			this.Controls.Add(this.labelClinic);
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
		private UI.Button butRefresh;
		private System.Windows.Forms.CheckBox checkHidden;
		private System.Windows.Forms.ContextMenu contextMenuMessages;
		private System.Windows.Forms.MenuItem menuItemChangePat;
		private System.Windows.Forms.MenuItem menuItemMarkUnread;
		private System.Windows.Forms.MenuItem menuItemMarkRead;
		private System.Windows.Forms.MenuItem menuItemHide;
		private System.Windows.Forms.MenuItem menuItemUnhide;
		private UI.ComboBoxMulti comboClinic;
		private UI.Button butPatCurrent;
		private UI.Button butPatAll;
		private UI.Button butPatFind;
		private System.Windows.Forms.TextBox textPatient;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox checkSent;
		private System.Windows.Forms.CheckBox checkReceivedAndRead;
		private System.Windows.Forms.CheckBox checkReceivedAndUnread;
		private UI.ODGrid gridMessageThread;
	}
}