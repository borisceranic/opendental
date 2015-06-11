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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.listStatus = new System.Windows.Forms.ListBox();
			this.label4 = new System.Windows.Forms.Label();
			this.checkShowHidden = new System.Windows.Forms.CheckBox();
			this.textDateTo = new ODR.ValidDate();
			this.textDateFrom = new ODR.ValidDate();
			this.comboClinic = new ODR.ComboBoxMulti();
			this.gridMessages = new OpenDental.UI.ODGrid();
			this.butUnhide = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.butJunk = new OpenDental.UI.Button();
			this.butChangePat = new OpenDental.UI.Button();
			this.butMarkUnread = new OpenDental.UI.Button();
			this.butMarkRead = new OpenDental.UI.Button();
			this.butRefresh = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(13, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(90, 21);
			this.label1.TabIndex = 6;
			this.label1.Text = "Clinic";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(13, 30);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(90, 21);
			this.label2.TabIndex = 8;
			this.label2.Text = "Date From";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(13, 50);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(90, 21);
			this.label3.TabIndex = 10;
			this.label3.Text = "Date To";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// listStatus
			// 
			this.listStatus.FormattingEnabled = true;
			this.listStatus.Location = new System.Drawing.Point(448, 9);
			this.listStatus.Name = "listStatus";
			this.listStatus.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listStatus.Size = new System.Drawing.Size(120, 56);
			this.listStatus.TabIndex = 11;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(335, 9);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(112, 21);
			this.label4.TabIndex = 12;
			this.label4.Text = "Type and Status";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkShowHidden
			// 
			this.checkShowHidden.Location = new System.Drawing.Point(358, 66);
			this.checkShowHidden.Name = "checkShowHidden";
			this.checkShowHidden.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkShowHidden.Size = new System.Drawing.Size(104, 20);
			this.checkShowHidden.TabIndex = 153;
			this.checkShowHidden.Text = "Show Hidden";
			this.checkShowHidden.UseVisualStyleBackColor = true;
			// 
			// textDateTo
			// 
			this.textDateTo.Location = new System.Drawing.Point(104, 50);
			this.textDateTo.Name = "textDateTo";
			this.textDateTo.Size = new System.Drawing.Size(81, 20);
			this.textDateTo.TabIndex = 9;
			// 
			// textDateFrom
			// 
			this.textDateFrom.Location = new System.Drawing.Point(104, 30);
			this.textDateFrom.Name = "textDateFrom";
			this.textDateFrom.Size = new System.Drawing.Size(81, 20);
			this.textDateFrom.TabIndex = 7;
			// 
			// comboClinic
			// 
			this.comboClinic.BackColor = System.Drawing.SystemColors.Window;
			this.comboClinic.DroppedDown = false;
			this.comboClinic.Items = ((System.Collections.ArrayList)(resources.GetObject("comboClinic.Items")));
			this.comboClinic.Location = new System.Drawing.Point(104, 9);
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboClinic.SelectedIndices")));
			this.comboClinic.Size = new System.Drawing.Size(225, 21);
			this.comboClinic.TabIndex = 5;
			// 
			// gridMessages
			// 
			this.gridMessages.AllowSortingByColumn = true;
			this.gridMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMessages.HScrollVisible = false;
			this.gridMessages.Location = new System.Drawing.Point(12, 92);
			this.gridMessages.Name = "gridMessages";
			this.gridMessages.ScrollValue = 0;
			this.gridMessages.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMessages.Size = new System.Drawing.Size(950, 562);
			this.gridMessages.TabIndex = 4;
			this.gridMessages.Title = "Text Messages";
			this.gridMessages.TranslationName = null;
			// 
			// butUnhide
			// 
			this.butUnhide.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUnhide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butUnhide.Autosize = true;
			this.butUnhide.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUnhide.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUnhide.CornerRadius = 4F;
			this.butUnhide.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUnhide.Location = new System.Drawing.Point(97, 660);
			this.butUnhide.Name = "butUnhide";
			this.butUnhide.Size = new System.Drawing.Size(83, 24);
			this.butUnhide.TabIndex = 152;
			this.butUnhide.Text = "Unhide";
			this.butUnhide.Click += new System.EventHandler(this.butUnhide_Click);
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(12, 660);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(83, 24);
			this.butDelete.TabIndex = 151;
			this.butDelete.Text = "Hide";
			this.butDelete.Click += new System.EventHandler(this.butHide_Click);
			// 
			// butJunk
			// 
			this.butJunk.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butJunk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butJunk.Autosize = true;
			this.butJunk.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butJunk.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butJunk.CornerRadius = 4F;
			this.butJunk.Location = new System.Drawing.Point(886, 66);
			this.butJunk.Name = "butJunk";
			this.butJunk.Size = new System.Drawing.Size(76, 24);
			this.butJunk.TabIndex = 150;
			this.butJunk.Text = "Mark Junk";
			this.butJunk.Click += new System.EventHandler(this.butJunk_Click);
			// 
			// butChangePat
			// 
			this.butChangePat.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butChangePat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butChangePat.Autosize = true;
			this.butChangePat.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butChangePat.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butChangePat.CornerRadius = 4F;
			this.butChangePat.Location = new System.Drawing.Point(655, 66);
			this.butChangePat.Name = "butChangePat";
			this.butChangePat.Size = new System.Drawing.Size(75, 24);
			this.butChangePat.TabIndex = 149;
			this.butChangePat.Text = "Change Pat";
			this.butChangePat.Click += new System.EventHandler(this.butChangePat_Click);
			// 
			// butMarkUnread
			// 
			this.butMarkUnread.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMarkUnread.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butMarkUnread.Autosize = true;
			this.butMarkUnread.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMarkUnread.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMarkUnread.CornerRadius = 4F;
			this.butMarkUnread.Location = new System.Drawing.Point(732, 66);
			this.butMarkUnread.Name = "butMarkUnread";
			this.butMarkUnread.Size = new System.Drawing.Size(75, 24);
			this.butMarkUnread.TabIndex = 148;
			this.butMarkUnread.Text = "Mark Unread";
			this.butMarkUnread.Click += new System.EventHandler(this.butMarkUnread_Click);
			// 
			// butMarkRead
			// 
			this.butMarkRead.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMarkRead.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butMarkRead.Autosize = true;
			this.butMarkRead.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMarkRead.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMarkRead.CornerRadius = 4F;
			this.butMarkRead.Location = new System.Drawing.Point(809, 66);
			this.butMarkRead.Name = "butMarkRead";
			this.butMarkRead.Size = new System.Drawing.Size(75, 24);
			this.butMarkRead.TabIndex = 147;
			this.butMarkRead.Text = "Mark Read";
			this.butMarkRead.Click += new System.EventHandler(this.butMarkRead_Click);
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(578, 66);
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
			this.Controls.Add(this.checkShowHidden);
			this.Controls.Add(this.butUnhide);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butJunk);
			this.Controls.Add(this.butChangePat);
			this.Controls.Add(this.butMarkUnread);
			this.Controls.Add(this.butMarkRead);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.listStatus);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textDateTo);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textDateFrom);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.comboClinic);
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
		private ODR.ComboBoxMulti comboClinic;
		private System.Windows.Forms.Label label1;
		private ODR.ValidDate textDateFrom;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private ODR.ValidDate textDateTo;
		private System.Windows.Forms.ListBox listStatus;
		private System.Windows.Forms.Label label4;
		private UI.Button butRefresh;
		private UI.Button butChangePat;
		private UI.Button butMarkUnread;
		private UI.Button butMarkRead;
		private UI.Button butJunk;
		private UI.Button butDelete;
		private UI.Button butUnhide;
		private System.Windows.Forms.CheckBox checkShowHidden;
	}
}