using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
///<summary></summary>
	public class FormProviderSetup:System.Windows.Forms.Form {
		private OpenDental.UI.Button butClose;
		private OpenDental.UI.Button butDown;
		private OpenDental.UI.Button butUp;
		private OpenDental.UI.Button butAdd;
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.ODGrid gridMain;
		private GroupBox groupDentalSchools;
		private ComboBox comboClass;
		private Label label1;
		private bool changed;
		private OpenDental.UI.Button butCreateUsers;
		private GroupBox groupCreateUsers;
		private Label label3;
		private ComboBox comboUserGroup;
		private GroupBox groupMovePats;
		private Label label2;
		private UI.Button butMovePri;
		private UI.Button butReassign;
		private Label label5;
		private RadioButton radioInstructors;
		private RadioButton radioStudents;
		private RadioButton radioAll;
		private Label label4;
		private TextBox textLastName;
		private UI.Button butProvPick;
		private TextBox textMoveTo;
		//private User user;
		private DataTable table;
		private Label label7;
		private TextBox textProvNum;
		private Label label6;
		private TextBox textFirstName;
		private UI.Button butStudBulkEdit;
		private Label label8;
		private UI.Button butMoveSec;
		///<summary>Set when prov picker button is used.  textMoveTo shows this prov in human readable format.</summary>
		private long _provNumMoveTo=-1;
		private List<UserGroup> _listUserGroups;

		///<summary>Not used for selection.  Use FormProviderPick or FormProviderMultiPick for that.</summary>
		public FormProviderSetup(){
			InitializeComponent();
			Lan.F(this);
			if(PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
				this.Width=841;
			}
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}    

		#region Windows Form Designer generated code

		private void InitializeComponent(){
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProviderSetup));
			this.butClose = new OpenDental.UI.Button();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.groupDentalSchools = new System.Windows.Forms.GroupBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.textProvNum = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textFirstName = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textLastName = new System.Windows.Forms.TextBox();
			this.radioInstructors = new System.Windows.Forms.RadioButton();
			this.radioStudents = new System.Windows.Forms.RadioButton();
			this.radioAll = new System.Windows.Forms.RadioButton();
			this.comboClass = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.butCreateUsers = new OpenDental.UI.Button();
			this.groupCreateUsers = new System.Windows.Forms.GroupBox();
			this.comboUserGroup = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.groupMovePats = new System.Windows.Forms.GroupBox();
			this.butMoveSec = new OpenDental.UI.Button();
			this.butProvPick = new OpenDental.UI.Button();
			this.textMoveTo = new System.Windows.Forms.TextBox();
			this.butReassign = new OpenDental.UI.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.butMovePri = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butStudBulkEdit = new OpenDental.UI.Button();
			this.groupDentalSchools.SuspendLayout();
			this.groupCreateUsers.SuspendLayout();
			this.groupMovePats.SuspendLayout();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butClose.Location = new System.Drawing.Point(885, 665);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(82, 26);
			this.butClose.TabIndex = 8;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(885, 450);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(82, 26);
			this.butDown.TabIndex = 5;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(885, 411);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(82, 26);
			this.butUp.TabIndex = 4;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(885, 522);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(82, 26);
			this.butAdd.TabIndex = 6;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// groupDentalSchools
			// 
			this.groupDentalSchools.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupDentalSchools.Controls.Add(this.label8);
			this.groupDentalSchools.Controls.Add(this.label7);
			this.groupDentalSchools.Controls.Add(this.textProvNum);
			this.groupDentalSchools.Controls.Add(this.label6);
			this.groupDentalSchools.Controls.Add(this.textFirstName);
			this.groupDentalSchools.Controls.Add(this.label4);
			this.groupDentalSchools.Controls.Add(this.textLastName);
			this.groupDentalSchools.Controls.Add(this.radioInstructors);
			this.groupDentalSchools.Controls.Add(this.radioStudents);
			this.groupDentalSchools.Controls.Add(this.radioAll);
			this.groupDentalSchools.Controls.Add(this.comboClass);
			this.groupDentalSchools.Controls.Add(this.label1);
			this.groupDentalSchools.Location = new System.Drawing.Point(703, 12);
			this.groupDentalSchools.Name = "groupDentalSchools";
			this.groupDentalSchools.Size = new System.Drawing.Size(273, 174);
			this.groupDentalSchools.TabIndex = 1;
			this.groupDentalSchools.TabStop = false;
			this.groupDentalSchools.Text = "Dental Schools Search by:";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(116, 48);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(148, 50);
			this.label8.TabIndex = 26;
			this.label8.Text = "These selections will also affect the functionality of the Add button.";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 146);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(90, 18);
			this.label7.TabIndex = 25;
			this.label7.Text = "ProvNum";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textProvNum
			// 
			this.textProvNum.Location = new System.Drawing.Point(98, 145);
			this.textProvNum.MaxLength = 15;
			this.textProvNum.Name = "textProvNum";
			this.textProvNum.Size = new System.Drawing.Size(166, 20);
			this.textProvNum.TabIndex = 6;
			this.textProvNum.TextChanged += new System.EventHandler(this.textProvNum_TextChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 124);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(90, 18);
			this.label6.TabIndex = 23;
			this.label6.Text = "First Name";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textFirstName
			// 
			this.textFirstName.Location = new System.Drawing.Point(98, 123);
			this.textFirstName.MaxLength = 15;
			this.textFirstName.Name = "textFirstName";
			this.textFirstName.Size = new System.Drawing.Size(166, 20);
			this.textFirstName.TabIndex = 5;
			this.textFirstName.TextChanged += new System.EventHandler(this.textFirstName_TextChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 102);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(90, 18);
			this.label4.TabIndex = 21;
			this.label4.Text = "Last Name";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textLastName
			// 
			this.textLastName.Location = new System.Drawing.Point(98, 101);
			this.textLastName.MaxLength = 15;
			this.textLastName.Name = "textLastName";
			this.textLastName.Size = new System.Drawing.Size(166, 20);
			this.textLastName.TabIndex = 4;
			this.textLastName.TextChanged += new System.EventHandler(this.textLastName_TextChanged);
			// 
			// radioInstructors
			// 
			this.radioInstructors.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioInstructors.Location = new System.Drawing.Point(6, 80);
			this.radioInstructors.Name = "radioInstructors";
			this.radioInstructors.Size = new System.Drawing.Size(104, 18);
			this.radioInstructors.TabIndex = 3;
			this.radioInstructors.Text = "Instructors";
			this.radioInstructors.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioInstructors.UseVisualStyleBackColor = true;
			this.radioInstructors.Click += new System.EventHandler(this.radioInstructors_Click);
			// 
			// radioStudents
			// 
			this.radioStudents.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioStudents.Location = new System.Drawing.Point(6, 63);
			this.radioStudents.Name = "radioStudents";
			this.radioStudents.Size = new System.Drawing.Size(104, 18);
			this.radioStudents.TabIndex = 2;
			this.radioStudents.Text = "Students";
			this.radioStudents.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioStudents.UseVisualStyleBackColor = true;
			this.radioStudents.Click += new System.EventHandler(this.radioStudents_Click);
			// 
			// radioAll
			// 
			this.radioAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioAll.Checked = true;
			this.radioAll.Location = new System.Drawing.Point(6, 46);
			this.radioAll.Name = "radioAll";
			this.radioAll.Size = new System.Drawing.Size(104, 18);
			this.radioAll.TabIndex = 1;
			this.radioAll.TabStop = true;
			this.radioAll.Text = "All";
			this.radioAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioAll.UseVisualStyleBackColor = true;
			this.radioAll.Click += new System.EventHandler(this.radioAll_Click);
			// 
			// comboClass
			// 
			this.comboClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClass.FormattingEnabled = true;
			this.comboClass.Location = new System.Drawing.Point(98, 19);
			this.comboClass.Name = "comboClass";
			this.comboClass.Size = new System.Drawing.Size(166, 21);
			this.comboClass.TabIndex = 0;
			this.comboClass.SelectionChangeCommitted += new System.EventHandler(this.comboClass_SelectionChangeCommitted);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(7, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(90, 18);
			this.label1.TabIndex = 16;
			this.label1.Text = "Classes";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butCreateUsers
			// 
			this.butCreateUsers.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCreateUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butCreateUsers.Autosize = true;
			this.butCreateUsers.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCreateUsers.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCreateUsers.CornerRadius = 4F;
			this.butCreateUsers.Location = new System.Drawing.Point(182, 42);
			this.butCreateUsers.Name = "butCreateUsers";
			this.butCreateUsers.Size = new System.Drawing.Size(82, 26);
			this.butCreateUsers.TabIndex = 15;
			this.butCreateUsers.Text = "Create";
			this.butCreateUsers.Click += new System.EventHandler(this.butCreateUsers_Click);
			// 
			// groupCreateUsers
			// 
			this.groupCreateUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupCreateUsers.Controls.Add(this.comboUserGroup);
			this.groupCreateUsers.Controls.Add(this.label3);
			this.groupCreateUsers.Controls.Add(this.butCreateUsers);
			this.groupCreateUsers.Location = new System.Drawing.Point(703, 192);
			this.groupCreateUsers.Name = "groupCreateUsers";
			this.groupCreateUsers.Size = new System.Drawing.Size(273, 76);
			this.groupCreateUsers.TabIndex = 2;
			this.groupCreateUsers.TabStop = false;
			this.groupCreateUsers.Text = "Create Users";
			// 
			// comboUserGroup
			// 
			this.comboUserGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboUserGroup.FormattingEnabled = true;
			this.comboUserGroup.Location = new System.Drawing.Point(98, 15);
			this.comboUserGroup.Name = "comboUserGroup";
			this.comboUserGroup.Size = new System.Drawing.Size(166, 21);
			this.comboUserGroup.TabIndex = 17;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(6, 17);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(91, 18);
			this.label3.TabIndex = 18;
			this.label3.Text = "User Group";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupMovePats
			// 
			this.groupMovePats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupMovePats.Controls.Add(this.butMoveSec);
			this.groupMovePats.Controls.Add(this.butProvPick);
			this.groupMovePats.Controls.Add(this.textMoveTo);
			this.groupMovePats.Controls.Add(this.butReassign);
			this.groupMovePats.Controls.Add(this.label5);
			this.groupMovePats.Controls.Add(this.label2);
			this.groupMovePats.Controls.Add(this.butMovePri);
			this.groupMovePats.Location = new System.Drawing.Point(703, 273);
			this.groupMovePats.Name = "groupMovePats";
			this.groupMovePats.Size = new System.Drawing.Size(273, 132);
			this.groupMovePats.TabIndex = 3;
			this.groupMovePats.TabStop = false;
			this.groupMovePats.Text = "Move Patients";
			// 
			// butMoveSec
			// 
			this.butMoveSec.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMoveSec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butMoveSec.Autosize = true;
			this.butMoveSec.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMoveSec.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMoveSec.CornerRadius = 4F;
			this.butMoveSec.Location = new System.Drawing.Point(182, 46);
			this.butMoveSec.Name = "butMoveSec";
			this.butMoveSec.Size = new System.Drawing.Size(82, 26);
			this.butMoveSec.TabIndex = 15;
			this.butMoveSec.Text = "Move Sec";
			this.butMoveSec.UseVisualStyleBackColor = true;
			this.butMoveSec.Click += new System.EventHandler(this.butMoveSec_Click);
			// 
			// butProvPick
			// 
			this.butProvPick.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butProvPick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butProvPick.Autosize = true;
			this.butProvPick.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butProvPick.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butProvPick.CornerRadius = 4F;
			this.butProvPick.Location = new System.Drawing.Point(237, 17);
			this.butProvPick.Name = "butProvPick";
			this.butProvPick.Size = new System.Drawing.Size(27, 26);
			this.butProvPick.TabIndex = 23;
			this.butProvPick.Text = "...";
			this.butProvPick.Click += new System.EventHandler(this.butProvPick_Click);
			// 
			// textMoveTo
			// 
			this.textMoveTo.Location = new System.Drawing.Point(98, 19);
			this.textMoveTo.MaxLength = 15;
			this.textMoveTo.Name = "textMoveTo";
			this.textMoveTo.ReadOnly = true;
			this.textMoveTo.Size = new System.Drawing.Size(135, 20);
			this.textMoveTo.TabIndex = 22;
			// 
			// butReassign
			// 
			this.butReassign.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butReassign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butReassign.Autosize = true;
			this.butReassign.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butReassign.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butReassign.CornerRadius = 4F;
			this.butReassign.Location = new System.Drawing.Point(182, 98);
			this.butReassign.Name = "butReassign";
			this.butReassign.Size = new System.Drawing.Size(82, 26);
			this.butReassign.TabIndex = 15;
			this.butReassign.Text = "Reassign";
			this.butReassign.Click += new System.EventHandler(this.butReassign_Click);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 98);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(168, 31);
			this.label5.TabIndex = 18;
			this.label5.Text = "Reassigns primary provider to most-used provider\r\n";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(3, 21);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(94, 18);
			this.label2.TabIndex = 18;
			this.label2.Text = "To Provider";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butMovePri
			// 
			this.butMovePri.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMovePri.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butMovePri.Autosize = true;
			this.butMovePri.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMovePri.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMovePri.CornerRadius = 4F;
			this.butMovePri.Location = new System.Drawing.Point(94, 46);
			this.butMovePri.Name = "butMovePri";
			this.butMovePri.Size = new System.Drawing.Size(82, 26);
			this.butMovePri.TabIndex = 15;
			this.butMovePri.Text = "Move Pri";
			this.butMovePri.Click += new System.EventHandler(this.butMovePri_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HScrollVisible = true;
			this.gridMain.Location = new System.Drawing.Point(7, 12);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(688, 679);
			this.gridMain.TabIndex = 13;
			this.gridMain.Title = "Providers";
			this.gridMain.TranslationName = "TableProviderSetup";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butStudBulkEdit
			// 
			this.butStudBulkEdit.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butStudBulkEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butStudBulkEdit.Autosize = true;
			this.butStudBulkEdit.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butStudBulkEdit.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butStudBulkEdit.CornerRadius = 4F;
			this.butStudBulkEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butStudBulkEdit.Location = new System.Drawing.Point(865, 554);
			this.butStudBulkEdit.Name = "butStudBulkEdit";
			this.butStudBulkEdit.Size = new System.Drawing.Size(102, 26);
			this.butStudBulkEdit.TabIndex = 7;
			this.butStudBulkEdit.Text = "Student Bulk Edit";
			this.butStudBulkEdit.Click += new System.EventHandler(this.butStudBulkEdit_Click);
			// 
			// FormProviderSetup
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(982, 707);
			this.Controls.Add(this.butStudBulkEdit);
			this.Controls.Add(this.groupMovePats);
			this.Controls.Add(this.groupCreateUsers);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butDown);
			this.Controls.Add(this.butUp);
			this.Controls.Add(this.groupDentalSchools);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(553, 660);
			this.Name = "FormProviderSetup";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Provider Setup";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormProviderSelect_Closing);
			this.Load += new System.EventHandler(this.FormProviderSetup_Load);
			this.groupDentalSchools.ResumeLayout(false);
			this.groupDentalSchools.PerformLayout();
			this.groupCreateUsers.ResumeLayout(false);
			this.groupMovePats.ResumeLayout(false);
			this.groupMovePats.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormProviderSetup_Load(object sender, System.EventArgs e) {
			//There are two permissions which allow access to this window: Providers and AdminDentalStudents.  SecurityAdmin allows some extra functions.
			Cache.Refresh(InvalidType.Providers);
			if(Security.IsAuthorized(Permissions.SecurityAdmin,true)){
				_listUserGroups=UserGroups.GetList();
				for(int i=0;i<_listUserGroups.Count;i++){
					comboUserGroup.Items.Add(_listUserGroups[i].Description);
				}
			}
			else{
				groupCreateUsers.Enabled=false;
				groupMovePats.Enabled=false;
			}
			if(PrefC.GetBool(PrefName.EasyHideDentalSchools)){
				groupDentalSchools.Visible=false;
				butStudBulkEdit.Visible=false;
			}
			else{
				comboClass.Items.Add(Lan.g(this,"All"));
				comboClass.SelectedIndex=0;
				for(int i=0;i<SchoolClasses.List.Length;i++){
					comboClass.Items.Add(Lan.g(this,SchoolClasses.GetDescript(SchoolClasses.List[i])));
				}
				butUp.Visible=false;
				butDown.Visible=false;
			}
			FillGrid();
		}

		private void FillGrid(){
			long selectedProvNum=0;
			if(gridMain.SelectedIndices.Length==1){
				selectedProvNum=(long)gridMain.Rows[gridMain.SelectedIndices[0]].Tag;
			}
			int scroll=gridMain.ScrollValue;
			if(groupDentalSchools.Visible) {
				long schoolClass=0;
				if(comboClass.SelectedIndex>0) {
					schoolClass=SchoolClasses.List[comboClass.SelectedIndex-1].SchoolClassNum;
				}
				table=Providers.RefreshForDentalSchool(schoolClass,textLastName.Text,textFirstName.Text,textProvNum.Text,radioInstructors.Checked,radioAll.Checked);
			}
			else {
				table=Providers.RefreshStandard();
				//fix orders
				bool neededFixing=false;
				Provider prov;
				for(int i=0;i<table.Rows.Count;i++) {
					if(table.Rows[i]["ItemOrder"].ToString()!=i.ToString()) {
						prov=Providers.GetProv(PIn.Long(table.Rows[i]["ProvNum"].ToString()));
						prov.ItemOrder=i;
						Providers.Update(prov);
						neededFixing=true;
					}
				}
				if(neededFixing) {
					DataValid.SetInvalid(InvalidType.Providers);
					table=Providers.RefreshStandard();
				}
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			if(!PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
				col=new ODGridColumn(Lan.g("TableProviderSetup","ProvNum"),60);
				gridMain.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g("TableProviderSetup","Abbrev"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProviderSetup","Last Name"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProviderSetup","First Name"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProviderSetup","User Name"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProviderSetup","Hidden"),50,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			if(!PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
				col=new ODGridColumn(Lan.g("TableProviderSetup","Class"),90);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableProviderSetup","Instructor"),60,HorizontalAlignment.Center);
				gridMain.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g("TableProviderSetup","PriPats"),50,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProviderSetup","SecPats"),50,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<table.Rows.Count;i++){
				row=new ODGridRow();
				if(table.Rows[i]["ProvStatus"].ToString()==POut.Int((int)ProviderStatus.Deleted)) {
					continue;
				}
				if(!PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
					row.Cells.Add(table.Rows[i]["ProvNum"].ToString());
				}
				row.Cells.Add(table.Rows[i]["Abbr"].ToString());
				row.Cells.Add(table.Rows[i]["LName"].ToString());
				row.Cells.Add(table.Rows[i]["FName"].ToString());
				row.Cells.Add(table.Rows[i]["UserName"].ToString());
				if(table.Rows[i]["IsHidden"].ToString()=="1"){
					row.Cells.Add("X");
				}
				else{
					row.Cells.Add("");
				}
				if(!PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
					if(table.Rows[i]["GradYear"].ToString()!=""){
						row.Cells.Add(table.Rows[i]["GradYear"].ToString()+"-"+table.Rows[i]["Descript"].ToString());
					}
					else{
						row.Cells.Add("");
					}
					if(table.Rows[i]["IsInstructor"].ToString()=="1") {
						row.Cells.Add("X");
					}
					else {
						row.Cells.Add("");
					}
				}
				row.Cells.Add(table.Rows[i]["PatCountPri"].ToString());
				row.Cells.Add(table.Rows[i]["PatCountSec"].ToString());
				row.Tag=PIn.Long(table.Rows[i]["ProvNum"].ToString());
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			for(int i=0;i<gridMain.Rows.Count;i++){
				if((long)gridMain.Rows[i].Tag==selectedProvNum){
					gridMain.SetSelected(i,true);
					break;
				}
			}
			gridMain.ScrollValue=scroll;
		}

		private void comboClass_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
		}

		private void textLastName_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void textFirstName_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void textProvNum_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void radioAll_Click(object sender,EventArgs e) {
			comboClass.SelectedIndex=0;//Only students are attached to classes
			FillGrid();
		}

		private void radioStudents_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void radioInstructors_Click(object sender,EventArgs e) {
			comboClass.SelectedIndex=0;//Only students are attached to classes
			FillGrid();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			FormProvEdit FormPE=new FormProvEdit();
			FormPE.ProvCur=new Provider();
			FormPE.ProvCur.IsNew=true;
			FormProvStudentEdit FormPSE=new FormProvStudentEdit();
			FormPSE.ProvStudent=new Provider();
			FormPSE.ProvStudent.IsNew=true;
			Provider provCur=new Provider();
			if(groupDentalSchools.Visible) {
				//Dental schools do not worry about item orders.
				if(radioStudents.Checked) {
					if(!Security.IsAuthorized(Permissions.AdminDentalStudents)) {
						return;
					}
					if(comboClass.SelectedIndex==0) {
						MsgBox.Show(this,"A class must be selected from the drop down box before a new student can be created");
						return;
					}
					FormPSE.ProvStudent.SchoolClassNum=SchoolClasses.List[comboClass.SelectedIndex-1].SchoolClassNum;
					FormPSE.ProvStudent.FName=textFirstName.Text;
					FormPSE.ProvStudent.LName=textLastName.Text;
				}
				if(radioInstructors.Checked && !Security.IsAuthorized(Permissions.AdminDentalInstructors)) {
					return;
				}
				FormPE.ProvCur.IsInstructor=radioInstructors.Checked;
				FormPE.ProvCur.FName=textFirstName.Text;
				FormPE.ProvCur.LName=textLastName.Text;
			}
			else {//Not using Dental Schools feature.
				Cache.Refresh(InvalidType.Providers);//Refresh the cache to get current information for the item orders
				if(gridMain.SelectedIndices.Length>0) {//place new provider after the first selected index. No changes are made to DB until after provider is actually inserted.
					FormPE.ProvCur.ItemOrder=Providers.GetProv((long)gridMain.Rows[gridMain.SelectedIndices[0]].Tag).ItemOrder;//now two with this itemorder
				}
				else if(gridMain.Rows.Count>0) {
					FormPE.ProvCur.ItemOrder=Providers.GetProv((long)gridMain.Rows[gridMain.Rows.Count-1].Tag).ItemOrder+1;
				}
				else {
					FormPE.ProvCur.ItemOrder=0;
				}
			}
			if(!radioStudents.Checked) {
				if(radioInstructors.Checked && PrefC.GetLong(PrefName.SecurityGroupForInstructors)==0) {
					MsgBox.Show(this,"Security Group for Instructors must be set from the Dental School Setup window before adding instructors.");
					return;
				}
				FormPE.IsNew=true;
				FormPE.ShowDialog();
				if(FormPE.DialogResult!=DialogResult.OK) {
					return;
				}
				provCur=FormPE.ProvCur;
			}
			else {
				if(radioStudents.Checked && PrefC.GetLong(PrefName.SecurityGroupForStudents)==0) {
					MsgBox.Show(this,"Security Group for Students must be set from the Dental School Setup window before adding students.");
					return;
				}
				FormPSE.ShowDialog();
				if(FormPSE.DialogResult!=DialogResult.OK) {
					return;
				}
				provCur=FormPSE.ProvStudent;
			}
			//new provider has already been inserted into DB from above
			Providers.MoveDownBelow(provCur);//safe to run even if none selected.
			changed=true;
			Cache.Refresh(InvalidType.Providers);//This refresh may be unnecessary, but it is here for safety reasons
			FillGrid();
			gridMain.ScrollToEnd();//should change this to scroll to the same place as before.
			for(int i=0;i<gridMain.Rows.Count;i++) {//ProviderC.ListLong.Count;i++) {
				if((long)gridMain.Rows[i].Tag==provCur.ProvNum) {
					//ProviderC.ListLong[i].ProvNum==FormP.ProvCur.ProvNum) {
					gridMain.SetSelected(i,true);
					break;
				}
			}
		}

		private void butStudBulkEdit_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.AdminDentalStudents)) {
				return;
			}
			FormProvStudentBulkEdit FormPSBE=new FormProvStudentBulkEdit();
			FormPSBE.ShowDialog();
		}

		///<summary>Won't be visible if using Dental Schools.  So list will be unfiltered and ItemOrders won't get messed up.</summary>
		private void butUp_Click(object sender, System.EventArgs e) {
			if(gridMain.SelectedIndices.Length!=1) {
				MsgBox.Show(this,"Please select exactly one provider first.");
				return;
			}
			if(gridMain.SelectedIndices[0]==0) {//already at top
				return;
			}
			Cache.Refresh(InvalidType.Providers);//Get the most recent information from the cache so we do not have null references to providers
			Provider prov=Providers.GetProv((long)gridMain.Rows[gridMain.SelectedIndices[0]].Tag);
			Provider otherprov=Providers.GetProv((long)gridMain.Rows[gridMain.SelectedIndices[0]-1].Tag);
			int oldItemOrder = prov.ItemOrder;
			prov.ItemOrder=otherprov.ItemOrder;
			Providers.Update(prov);
			otherprov.ItemOrder=oldItemOrder;
			Providers.Update(otherprov);
			changed=true;
			int oldSelectedInx=gridMain.SelectedIndices[0];
			gridMain.SetSelected(false);
			FillGrid();
			gridMain.SetSelected(oldSelectedInx-1,true);
		}

		///<summary>Won't be visible if using Dental Schools.  So list will be unfiltered and ItemOrders won't get messed up.</summary>
		private void butDown_Click(object sender, System.EventArgs e) {
			if(gridMain.SelectedIndices.Length!=1) {
				MsgBox.Show(this,"Please select exactly one provider first.");
				return;
			}
			if(gridMain.SelectedIndices[0]==ProviderC.ListLong.Count-1) {//already at bottom
				return;
			}
			Cache.Refresh(InvalidType.Providers);//Get the most recent information from the cache so we do not have null references to providers
			Provider prov=Providers.GetProv((long)gridMain.Rows[gridMain.SelectedIndices[0]].Tag);
			Provider otherprov=Providers.GetProv((long)gridMain.Rows[gridMain.SelectedIndices[0]+1].Tag);
			int oldItemOrder = prov.ItemOrder;
			prov.ItemOrder=otherprov.ItemOrder;
			Providers.Update(prov);
			otherprov.ItemOrder=oldItemOrder;
			Providers.Update(otherprov);
			changed=true;
			int oldSelectedInx=gridMain.SelectedIndices[0];
			gridMain.SetSelected(false);
			FillGrid();
			gridMain.SetSelected(oldSelectedInx+1,true);
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Cache.Refresh(InvalidType.Providers);//Get the most recent information from the cache so we do not have null references to providers
			Provider provSelected=Providers.GetProv((long)gridMain.Rows[gridMain.SelectedIndices[0]].Tag);
			if(!PrefC.GetBool(PrefName.EasyHideDentalSchools) && Providers.IsAttachedToUser(provSelected.ProvNum)) {//Dental schools is turned on and the provider selected is attached to a user.
				//provSelected could be a provider or a student at this point.
				if(!provSelected.IsInstructor && !Security.IsAuthorized(Permissions.AdminDentalStudents)) {
					return;
				}
				if(provSelected.IsInstructor && !Security.IsAuthorized(Permissions.AdminDentalInstructors)) {
					return;
				}
				if(!radioStudents.Checked) {
					FormProvEdit FormPE=new FormProvEdit();
					FormPE.ProvCur=Providers.GetProv((long)gridMain.Rows[gridMain.SelectedIndices[0]].Tag);
					FormPE.ShowDialog();
					if(FormPE.DialogResult!=DialogResult.OK) {
						return;
					}
				}
				else {
					FormProvStudentEdit FormPSE=new FormProvStudentEdit();
					FormPSE.ProvStudent=Providers.GetProv((long)gridMain.Rows[gridMain.SelectedIndices[0]].Tag);
					FormPSE.ShowDialog();
					if(FormPSE.DialogResult!=DialogResult.OK) {
						return;
					}
				}
			}
			else {//No Dental Schools or provider is not attached to a user
				FormProvEdit FormPE=new FormProvEdit();
				FormPE.ProvCur=Providers.GetProv((long)gridMain.Rows[gridMain.SelectedIndices[0]].Tag);
				FormPE.ShowDialog();
				if(FormPE.DialogResult!=DialogResult.OK) {
					return;
				}
			}
			changed=true;
			FillGrid();
		}

		private void butProvPick_Click(object sender,EventArgs e) {
			//This button is used instead of a dropdown because the order of providers can frequently change in the grid.
			Cache.Refresh(InvalidType.Providers);//Get the most recent information from the cache so we do not have null references to providers
			FormProviderPick formPick=new FormProviderPick();
			formPick.IsNoneAvailable=true;
			formPick.ShowDialog();
			if(formPick.DialogResult!=DialogResult.OK) {
				return;
			}
			_provNumMoveTo=formPick.SelectedProvNum;
			if(_provNumMoveTo>0) {
				Provider provTo=Providers.GetProv(_provNumMoveTo);
				textMoveTo.Text=provTo.GetLongDesc();
			}
			else {
				textMoveTo.Text="None";
			}
		}

		///<summary>Not possible if no security admin.</summary>
		private void butMovePri_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length!=1) {
				MsgBox.Show(this,"You must select exactly one provider to move patients from.");
				return;
			}
			if(_provNumMoveTo==-1){
				MsgBox.Show(this,"You must pick a To provider in the box above to move patients to.");
				return;
			}
			if(_provNumMoveTo==0) {
				MsgBox.Show(this,"None is not a valid primary provider.");
				return;
			}
			Cache.Refresh(InvalidType.Providers);//Get the most recent information from the cache so we do not have null references to providers
			Provider provFrom=Providers.GetProv((long)gridMain.Rows[gridMain.SelectedIndices[0]].Tag);
			Provider provTo=Providers.GetProv(_provNumMoveTo);
			string msg=Lan.g(this,"Move all primary patients from")+" "+provFrom.GetLongDesc()+" "+Lan.g(this,"to")+" "+provTo.GetLongDesc()+"?";
			if(MessageBox.Show(msg,"",MessageBoxButtons.OKCancel)==DialogResult.OK) {
				Patients.ChangePrimaryProviders(provFrom.ProvNum,provTo.ProvNum);
			}
			changed=true;
			FillGrid();
		}

		///<summary>Not possible if no security admin.</summary>
		private void butMoveSec_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length!=1) {
				MsgBox.Show(this,"You must select exactly one provider to move patients from.");
				return;
			}
			if(_provNumMoveTo==-1) {
				MsgBox.Show(this,"You must pick a provider in the box above to move patients to.");
				return;
			}
			Cache.Refresh(InvalidType.Providers);//Get the most recent information from the cache so we do not have null references to providers
			Provider provFrom=Providers.GetProv((long)gridMain.Rows[gridMain.SelectedIndices[0]].Tag);
			Provider provTo=Providers.GetProv(_provNumMoveTo);
			string msg;
			if(provTo==null) {
				msg=Lan.g(this,"Remove all secondary patients from")+" "+provFrom.GetLongDesc()+"?";
			}
			else {
				msg=Lan.g(this,"Move all secondary patients from")+" "+provFrom.GetLongDesc()+" "+Lan.g(this,"to")+" "+provTo.GetLongDesc()+"?";
			}
			if(MessageBox.Show(msg,"",MessageBoxButtons.OKCancel)==DialogResult.OK) {
				if(provTo!=null) {
					Patients.ChangeSecondaryProviders(provFrom.ProvNum,provTo.ProvNum);
				}
				else {
					Patients.ChangeSecondaryProviders(provFrom.ProvNum,0);
				}
			}
			changed=true;
			FillGrid();
		}

		private void butReassign_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Ready to look for possible reassignments.  This will take a few minutes, and may make the program unresponsive on other computers during that time.  You will be given one more chance after this to cancel before changes are made to database.  Continue?")) {
				return;
			}
			Cursor=Cursors.WaitCursor;//On a very large database we have seen this take as long as 106 seconds.  The first loop takes about 80% of the time.
			List<long> provsFrom=new List<long>();
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				provsFrom.Add((long)gridMain.Rows[gridMain.SelectedIndices[0]].Tag);
			}
			DataTable tablePats=Patients.GetPatsByPriProvs(provsFrom);//list of all patients who are using the selected providers.
			if(tablePats==null || gridMain.Rows.Count==0) {
				Cursor=Cursors.Default;
				MsgBox.Show(this,"No patients to reassign.");
				return;
			}
			int countPatsToUpdate=0;
			List<long> mostUsedProvs=new List<long>();//1:1 relationship with table.
			for(int i=0;i<tablePats.Rows.Count;i++) {
				long provNumMostUsed=Patients.ReassignProvGetMostUsed(PIn.Long(tablePats.Rows[i]["PatNum"].ToString()));
				mostUsedProvs.Add(provNumMostUsed);
				if(mostUsedProvs[i]==0) {
					continue;
				}
				if(tablePats.Rows[i]["PriProv"].ToString()!=provNumMostUsed.ToString()) {//Provnums don't match.
					countPatsToUpdate++;
				}
			}
			//inform user of count. Continue?
			Cursor=Cursors.Default;
			string msg=Lan.g(this,"You are about to reassign")+" "+countPatsToUpdate.ToString()+" "+Lan.g(this,"patients to different providers.  Continue?");
			if(MessageBox.Show(msg,"",MessageBoxButtons.OKCancel)!=DialogResult.OK) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			for(int i=0;i<tablePats.Rows.Count;i++) {
				if(mostUsedProvs[i]==0) {
					continue;
				}
				if(tablePats.Rows[i]["PriProv"].ToString()!=mostUsedProvs[i].ToString()) {//Provnums don't match, so update
					Patients.ReassignProv(PIn.Long(tablePats.Rows[i]["PatNum"].ToString()),mostUsedProvs[i]);
				}
			}
			Cursor=Cursors.Default;
			//changed=true;//We didn't change any providers
			FillGrid();
		}

		///<summary>Not possible if no security admin.</summary>
		private void butCreateUsers_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0){
				MsgBox.Show(this,"Please select one or more providers first.");
				return;
			}
			for(int i=0;i<gridMain.SelectedIndices.Length;i++){
				if(Providers.IsAttachedToUser((long)gridMain.Rows[gridMain.SelectedIndices[0]].Tag)) {
					MsgBox.Show(this,"Not allowed to create users on providers which already have users.");
					return;
				}
			}
			if(comboUserGroup.SelectedIndex==-1){
				MsgBox.Show(this,"Please select a User Group first.");
				return;
			}
			for(int i=0;i<gridMain.SelectedIndices.Length;i++){
				Provider prov = Providers.GetProv((long)gridMain.Rows[gridMain.SelectedIndices[0]].Tag);
				Userod user=new Userod();
				user.UserGroupNum=_listUserGroups[comboUserGroup.SelectedIndex].UserGroupNum;
				user.ProvNum=(long)gridMain.Rows[gridMain.SelectedIndices[0]].Tag;
				user.UserName=GetUniqueUserName(prov.LName,prov.FName);
				user.Password=user.UserName;//this will be enhanced later.
				try{
					Userods.Insert(user);
				}
				catch(ApplicationException ex){
					MessageBox.Show(ex.Message);
					changed=true;
					return;
				}
			}
			changed=true;
			FillGrid();
		}

		private string GetUniqueUserName(string lname,string fname){
			string name=lname;
			if(fname.Length>0){
				name+=fname.Substring(0,1);
			}
			if(Userods.IsUserNameUnique(name,0,false)){
				return name;
			}
			int fnameI=1;
			while(fnameI<fname.Length){
				name+=fname.Substring(fnameI,1);
				if(Userods.IsUserNameUnique(name,0,false)) {
					return name;
				}
				fnameI++;
			}
			//should be entire lname+fname at this point, but still not unique
			do{
				name+="x";
			}
			while(!Userods.IsUserNameUnique(name,0,false));
			return name;
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

		private void FormProviderSelect_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			string duplicates=Providers.GetDuplicateAbbrs();
			if(duplicates!="" && PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
				if(MessageBox.Show(Lan.g(this,"Warning.  The following abbreviations are duplicates.  Continue anyway?\r\n")+duplicates,
					"",MessageBoxButtons.OKCancel)!=DialogResult.OK)
				{
					e.Cancel=true;
					return;
				}
			}
			if(changed){
				DataValid.SetInvalid(InvalidType.Providers, InvalidType.Security);
			}
			//SecurityLogs.MakeLogEntry("Providers","Altered Providers",user);
		}



		

		

	

		

		

		

	

	}
}
