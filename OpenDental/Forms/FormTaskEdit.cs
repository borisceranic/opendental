using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Linq;

namespace OpenDental {
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormTaskEdit:System.Windows.Forms.Form {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private OpenDental.ODtextBox textDescript;
		private Task TaskCur;
		private Task TaskOld;
		private OpenDental.ValidDate textDateTask;
		private OpenDental.UI.Button butChange;
		private OpenDental.UI.Button butGoto;
		///<summary></summary>
		public bool IsNew;
		private System.Windows.Forms.CheckBox checkFromNum;
		private System.Windows.Forms.Label labelObjectDesc;
		private System.Windows.Forms.TextBox textObjectDesc;
		private System.Windows.Forms.ListBox listObjectType;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Panel panelObject;
		///<summary>After closing, if this is not zero, then it will jump to the object specified in GotoKeyNum.</summary>
		public TaskObjectType GotoType;
		private Label label5;
		private TextBox textDateTimeEntry;
		private OpenDental.UI.Button butNow;
		private OpenDental.UI.Button butDelete;
		private TextBox textUser;
		private Label label16;
		private OpenDental.UI.Button butNowFinished;
		private TextBox textDateTimeFinished;
		private Label label7;
		private TextBox textTaskNum;
		private Label labelTaskNum;
		///<summary>After closing, if this is not zero, then it will jump to the specified patient.</summary>
		public long GotoKeyNum;
		private Label labelReply;
		private OpenDental.UI.Button butReply;
		private OpenDental.UI.Button butSend;
		private TextBox textTaskList;
		private Label label10;
		private ComboBox comboDateType;
		private TaskList TaskListCur;
		private UI.ODGrid gridMain;
		///<summary>Will be set to true if any note was added or an existing note changed. Does not track changes in the description.</summary>
		public bool NotesChanged;
		private UI.Button butAddNote;
		private UI.Button butChangeUser;
		private List<TaskNote> NoteList;
		private CheckBox checkNew;
		private CheckBox checkDone;
		private Label labelDoneAffectsAll;
		///<summary>If the reply button is visible, this stores who to reply to.  It's determined when loading the form.</summary>
		private long ReplyToUserNum;
		///<summary>Gets set to true externally if this window popped up without user interaction.  It will behave slightly differently.  
		///Specifically, the New checkbox will be unchecked so that if user clicks OK, the task will be marked as read.
		///Also if IsPop is set to true, this window will not steal focus from other windows when poping up.</summary>
		public bool IsPopup;
		///<summary>When tracking status by user, this tracks whether it has changed.  This is so that if it has changed, a signal can be sent for a refresh of lists.</summary>
		private bool StatusChanged;
		///<summary>If this task starts out 'unread', then this starts out true.  If the user changes the description or changes a note, then the task gets set to read.  But the user can manually change it back and this variable gets set to false.  From then on, any changes to description or note do not trigger the task to get set to read.  In other words, the automation only happens once.</summary>
		private bool MightNeedSetRead;
		private UI.Button butCopy;
		private TextBox textBox1;
		private UI.Button butRed;
		private UI.Button butBlue;
		private ComboBox comboTaskPriorities;
		private Label label8;
		///<summary>When this window is first opened, if this task is in someone else's inbox, then the "new" status is meaningless and will not show.  In that case, this variable is set to true.  Only used when tracking new status by user.</summary>
		private bool StartedInOthersInbox;
		private System.Windows.Forms.Button butColor;
		///<summary>Filled on load with all non-hidden task priority definitions.</summary>
		private List<Def> _listTaskPriorities;
		private long _pritoryDefNumSelected;
		///<summary>Keeps track of the number of notes that were associated to this task on load and after refilling the task note grid.  Only used for HQ in order to keep track of task note manipulation.</summary>
		private int _numNotes=-1;
		///<summary>FK to the definition.DefNum at HQ for the triage priority color for red.</summary>
		private const long _triageRedNum=501;
		///<summary>FK to the definition.DefNum at HQ for the triage priority color for blue.</summary>
		private const long _triageBlueNum=502;
		///<summary>FK to the definition.DefNum at HQ for the triage priority color for white.</summary>
		private const long _triageWhiteNum=503;
		private UI.Button butAudit;
		private ComboBox comboJobs;
		private Label labelJobs;
		private List<JobLink> _jobLinks;
		private List<Job> _listJobs;
		private bool _isLoading;


		///<summary>FK to tasklist.TaskListNum. </summary>
		private const long _triageTaskListNum=1697;

		///<summary>This is used to make the task window not steal focus when opening as a popup.</summary>
		protected override bool ShowWithoutActivation
		{
			get { return IsPopup; }
		}

		///<summary>Task gets inserted ahead of time, then frequently altered before passing in here.  The taskOld that is passed in should be the task as it is in the database.  When saving, taskOld will be compared with db to make sure no changes.</summary>
		public FormTaskEdit(Task taskCur,Task taskOld) {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			TaskCur=taskCur;
			TaskOld=taskOld;
			TaskListCur=TaskLists.GetOne(taskCur.TaskListNum);
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing) {
			if(disposing) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTaskEdit));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.checkFromNum = new System.Windows.Forms.CheckBox();
			this.labelObjectDesc = new System.Windows.Forms.Label();
			this.textObjectDesc = new System.Windows.Forms.TextBox();
			this.listObjectType = new System.Windows.Forms.ListBox();
			this.label6 = new System.Windows.Forms.Label();
			this.panelObject = new System.Windows.Forms.Panel();
			this.butGoto = new OpenDental.UI.Button();
			this.butChange = new OpenDental.UI.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.textDateTimeEntry = new System.Windows.Forms.TextBox();
			this.textUser = new System.Windows.Forms.TextBox();
			this.label16 = new System.Windows.Forms.Label();
			this.textDateTimeFinished = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.textTaskNum = new System.Windows.Forms.TextBox();
			this.labelTaskNum = new System.Windows.Forms.Label();
			this.labelReply = new System.Windows.Forms.Label();
			this.textTaskList = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.comboDateType = new System.Windows.Forms.ComboBox();
			this.checkNew = new System.Windows.Forms.CheckBox();
			this.checkDone = new System.Windows.Forms.CheckBox();
			this.labelDoneAffectsAll = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.comboTaskPriorities = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.butColor = new System.Windows.Forms.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butAudit = new OpenDental.UI.Button();
			this.butBlue = new OpenDental.UI.Button();
			this.butRed = new OpenDental.UI.Button();
			this.butChangeUser = new OpenDental.UI.Button();
			this.butAddNote = new OpenDental.UI.Button();
			this.butSend = new OpenDental.UI.Button();
			this.butReply = new OpenDental.UI.Button();
			this.butNowFinished = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.butNow = new OpenDental.UI.Button();
			this.textDateTask = new OpenDental.ValidDate();
			this.butCopy = new OpenDental.UI.Button();
			this.textDescript = new OpenDental.ODtextBox();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.comboJobs = new System.Windows.Forms.ComboBox();
			this.labelJobs = new System.Windows.Forms.Label();
			this.panelObject.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(3, 85);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(102, 19);
			this.label1.TabIndex = 2;
			this.label1.Text = "Description";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.Location = new System.Drawing.Point(3, 538);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(102, 19);
			this.label2.TabIndex = 4;
			this.label2.Text = "Date";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.Location = new System.Drawing.Point(198, 535);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(185, 32);
			this.label3.TabIndex = 6;
			this.label3.Text = "Leave blank unless you want this task to show on a dated list";
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label4.Location = new System.Drawing.Point(3, 564);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(102, 19);
			this.label4.TabIndex = 7;
			this.label4.Text = "Date Type";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkFromNum
			// 
			this.checkFromNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkFromNum.CheckAlign = System.Drawing.ContentAlignment.TopRight;
			this.checkFromNum.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkFromNum.Location = new System.Drawing.Point(3, 590);
			this.checkFromNum.Name = "checkFromNum";
			this.checkFromNum.Size = new System.Drawing.Size(116, 18);
			this.checkFromNum.TabIndex = 3;
			this.checkFromNum.Text = "Is From Repeating";
			this.checkFromNum.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelObjectDesc
			// 
			this.labelObjectDesc.Location = new System.Drawing.Point(26, 1);
			this.labelObjectDesc.Name = "labelObjectDesc";
			this.labelObjectDesc.Size = new System.Drawing.Size(77, 19);
			this.labelObjectDesc.TabIndex = 8;
			this.labelObjectDesc.Text = "ObjectDesc";
			this.labelObjectDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textObjectDesc
			// 
			this.textObjectDesc.Location = new System.Drawing.Point(103, 0);
			this.textObjectDesc.Multiline = true;
			this.textObjectDesc.Name = "textObjectDesc";
			this.textObjectDesc.Size = new System.Drawing.Size(302, 34);
			this.textObjectDesc.TabIndex = 0;
			this.textObjectDesc.Text = "line 1\r\nline 2";
			// 
			// listObjectType
			// 
			this.listObjectType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.listObjectType.Location = new System.Drawing.Point(431, 565);
			this.listObjectType.Name = "listObjectType";
			this.listObjectType.Size = new System.Drawing.Size(120, 43);
			this.listObjectType.TabIndex = 13;
			this.listObjectType.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listObjectType_MouseDown);
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label6.Location = new System.Drawing.Point(312, 564);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(116, 19);
			this.label6.TabIndex = 14;
			this.label6.Text = "Object Type";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panelObject
			// 
			this.panelObject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.panelObject.Controls.Add(this.textObjectDesc);
			this.panelObject.Controls.Add(this.labelObjectDesc);
			this.panelObject.Controls.Add(this.butGoto);
			this.panelObject.Controls.Add(this.butChange);
			this.panelObject.Location = new System.Drawing.Point(3, 609);
			this.panelObject.Name = "panelObject";
			this.panelObject.Size = new System.Drawing.Size(590, 34);
			this.panelObject.TabIndex = 15;
			// 
			// butGoto
			// 
			this.butGoto.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGoto.Autosize = true;
			this.butGoto.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGoto.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGoto.CornerRadius = 4F;
			this.butGoto.Location = new System.Drawing.Point(501, 6);
			this.butGoto.Name = "butGoto";
			this.butGoto.Size = new System.Drawing.Size(75, 22);
			this.butGoto.TabIndex = 12;
			this.butGoto.Text = "Go To";
			this.butGoto.Click += new System.EventHandler(this.butGoto_Click);
			// 
			// butChange
			// 
			this.butChange.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butChange.Autosize = true;
			this.butChange.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butChange.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butChange.CornerRadius = 4F;
			this.butChange.Location = new System.Drawing.Point(418, 6);
			this.butChange.Name = "butChange";
			this.butChange.Size = new System.Drawing.Size(75, 22);
			this.butChange.TabIndex = 10;
			this.butChange.Text = "Change";
			this.butChange.Click += new System.EventHandler(this.butChange_Click);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(3, 25);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(102, 19);
			this.label5.TabIndex = 17;
			this.label5.Text = "Date/Time Entry";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDateTimeEntry
			// 
			this.textDateTimeEntry.Location = new System.Drawing.Point(107, 24);
			this.textDateTimeEntry.Name = "textDateTimeEntry";
			this.textDateTimeEntry.Size = new System.Drawing.Size(151, 20);
			this.textDateTimeEntry.TabIndex = 18;
			// 
			// textUser
			// 
			this.textUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textUser.Location = new System.Drawing.Point(582, 16);
			this.textUser.Name = "textUser";
			this.textUser.ReadOnly = true;
			this.textUser.Size = new System.Drawing.Size(134, 20);
			this.textUser.TabIndex = 0;
			// 
			// label16
			// 
			this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label16.Location = new System.Drawing.Point(485, 18);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(94, 16);
			this.label16.TabIndex = 125;
			this.label16.Text = "From User";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDateTimeFinished
			// 
			this.textDateTimeFinished.Location = new System.Drawing.Point(107, 49);
			this.textDateTimeFinished.Name = "textDateTimeFinished";
			this.textDateTimeFinished.Size = new System.Drawing.Size(151, 20);
			this.textDateTimeFinished.TabIndex = 131;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(3, 50);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(102, 19);
			this.label7.TabIndex = 130;
			this.label7.Text = "Date/Time Finished";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTaskNum
			// 
			this.textTaskNum.Location = new System.Drawing.Point(407, 27);
			this.textTaskNum.Name = "textTaskNum";
			this.textTaskNum.ReadOnly = true;
			this.textTaskNum.Size = new System.Drawing.Size(54, 20);
			this.textTaskNum.TabIndex = 134;
			this.textTaskNum.Visible = false;
			// 
			// labelTaskNum
			// 
			this.labelTaskNum.Location = new System.Drawing.Point(332, 28);
			this.labelTaskNum.Name = "labelTaskNum";
			this.labelTaskNum.Size = new System.Drawing.Size(73, 16);
			this.labelTaskNum.TabIndex = 133;
			this.labelTaskNum.Text = "TaskNum";
			this.labelTaskNum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelTaskNum.Visible = false;
			// 
			// labelReply
			// 
			this.labelReply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelReply.Location = new System.Drawing.Point(312, 652);
			this.labelReply.Name = "labelReply";
			this.labelReply.Size = new System.Drawing.Size(162, 19);
			this.labelReply.TabIndex = 141;
			this.labelReply.Text = "(Send to author)";
			this.labelReply.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textTaskList
			// 
			this.textTaskList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textTaskList.Location = new System.Drawing.Point(582, 39);
			this.textTaskList.Name = "textTaskList";
			this.textTaskList.ReadOnly = true;
			this.textTaskList.Size = new System.Drawing.Size(134, 20);
			this.textTaskList.TabIndex = 146;
			// 
			// label10
			// 
			this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label10.Location = new System.Drawing.Point(485, 41);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(94, 16);
			this.label10.TabIndex = 147;
			this.label10.Text = "Task List";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboDateType
			// 
			this.comboDateType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.comboDateType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDateType.FormattingEnabled = true;
			this.comboDateType.Location = new System.Drawing.Point(107, 564);
			this.comboDateType.Name = "comboDateType";
			this.comboDateType.Size = new System.Drawing.Size(145, 21);
			this.comboDateType.TabIndex = 148;
			// 
			// checkNew
			// 
			this.checkNew.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkNew.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkNew.Location = new System.Drawing.Point(27, 5);
			this.checkNew.Name = "checkNew";
			this.checkNew.Size = new System.Drawing.Size(94, 17);
			this.checkNew.TabIndex = 152;
			this.checkNew.Text = "New";
			this.checkNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkNew.Click += new System.EventHandler(this.checkNew_Click);
			// 
			// checkDone
			// 
			this.checkDone.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkDone.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkDone.Location = new System.Drawing.Point(147, 5);
			this.checkDone.Name = "checkDone";
			this.checkDone.Size = new System.Drawing.Size(94, 17);
			this.checkDone.TabIndex = 153;
			this.checkDone.Text = "Done";
			this.checkDone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkDone.Click += new System.EventHandler(this.checkDone_Click);
			// 
			// labelDoneAffectsAll
			// 
			this.labelDoneAffectsAll.Location = new System.Drawing.Point(247, 5);
			this.labelDoneAffectsAll.Name = "labelDoneAffectsAll";
			this.labelDoneAffectsAll.Size = new System.Drawing.Size(167, 16);
			this.labelDoneAffectsAll.TabIndex = 154;
			this.labelDoneAffectsAll.Text = "(affects all users)";
			this.labelDoneAffectsAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(454, -72);
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(54, 20);
			this.textBox1.TabIndex = 134;
			this.textBox1.Visible = false;
			// 
			// comboTaskPriorities
			// 
			this.comboTaskPriorities.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboTaskPriorities.FormattingEnabled = true;
			this.comboTaskPriorities.Location = new System.Drawing.Point(582, 61);
			this.comboTaskPriorities.Name = "comboTaskPriorities";
			this.comboTaskPriorities.Size = new System.Drawing.Size(134, 21);
			this.comboTaskPriorities.TabIndex = 157;
			this.comboTaskPriorities.SelectedIndexChanged += new System.EventHandler(this.comboTaskPriorities_SelectedIndexChanged);
			// 
			// label8
			// 
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label8.Location = new System.Drawing.Point(486, 62);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(94, 16);
			this.label8.TabIndex = 158;
			this.label8.Text = "Task Priority";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butColor
			// 
			this.butColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butColor.Enabled = false;
			this.butColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.butColor.Location = new System.Drawing.Point(719, 61);
			this.butColor.Name = "butColor";
			this.butColor.Size = new System.Drawing.Size(24, 21);
			this.butColor.TabIndex = 159;
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 196);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(798, 338);
			this.gridMain.TabIndex = 149;
			this.gridMain.Title = "Notes";
			this.gridMain.TranslationName = "FormTaskEdit";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butAudit
			// 
			this.butAudit.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAudit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAudit.Autosize = true;
			this.butAudit.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAudit.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAudit.CornerRadius = 4F;
			this.butAudit.Location = new System.Drawing.Point(749, 59);
			this.butAudit.Name = "butAudit";
			this.butAudit.Size = new System.Drawing.Size(61, 24);
			this.butAudit.TabIndex = 160;
			this.butAudit.Text = "Audit";
			this.butAudit.Click += new System.EventHandler(this.butAudit_Click);
			// 
			// butBlue
			// 
			this.butBlue.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBlue.Autosize = true;
			this.butBlue.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBlue.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBlue.CornerRadius = 4F;
			this.butBlue.Location = new System.Drawing.Point(62, 139);
			this.butBlue.Name = "butBlue";
			this.butBlue.Size = new System.Drawing.Size(43, 24);
			this.butBlue.TabIndex = 156;
			this.butBlue.Text = "Blue";
			this.butBlue.Visible = false;
			this.butBlue.Click += new System.EventHandler(this.butBlue_Click);
			// 
			// butRed
			// 
			this.butRed.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRed.Autosize = true;
			this.butRed.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRed.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRed.CornerRadius = 4F;
			this.butRed.Location = new System.Drawing.Point(62, 169);
			this.butRed.Name = "butRed";
			this.butRed.Size = new System.Drawing.Size(43, 24);
			this.butRed.TabIndex = 155;
			this.butRed.Text = "Red";
			this.butRed.Visible = false;
			this.butRed.Click += new System.EventHandler(this.butRed_Click);
			// 
			// butChangeUser
			// 
			this.butChangeUser.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butChangeUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butChangeUser.Autosize = true;
			this.butChangeUser.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butChangeUser.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butChangeUser.CornerRadius = 4F;
			this.butChangeUser.Location = new System.Drawing.Point(719, 14);
			this.butChangeUser.Name = "butChangeUser";
			this.butChangeUser.Size = new System.Drawing.Size(24, 22);
			this.butChangeUser.TabIndex = 151;
			this.butChangeUser.Text = "...";
			this.butChangeUser.Click += new System.EventHandler(this.butChangeUser_Click);
			// 
			// butAddNote
			// 
			this.butAddNote.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAddNote.Autosize = true;
			this.butAddNote.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddNote.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddNote.CornerRadius = 4F;
			this.butAddNote.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddNote.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddNote.Location = new System.Drawing.Point(735, 540);
			this.butAddNote.Name = "butAddNote";
			this.butAddNote.Size = new System.Drawing.Size(75, 24);
			this.butAddNote.TabIndex = 150;
			this.butAddNote.Text = "Add";
			this.butAddNote.Click += new System.EventHandler(this.butAddNote_Click);
			// 
			// butSend
			// 
			this.butSend.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butSend.Autosize = true;
			this.butSend.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSend.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSend.CornerRadius = 4F;
			this.butSend.Location = new System.Drawing.Point(478, 649);
			this.butSend.Name = "butSend";
			this.butSend.Size = new System.Drawing.Size(75, 24);
			this.butSend.TabIndex = 142;
			this.butSend.Text = "Send To...";
			this.butSend.Click += new System.EventHandler(this.butSend_Click);
			// 
			// butReply
			// 
			this.butReply.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butReply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butReply.Autosize = true;
			this.butReply.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butReply.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butReply.CornerRadius = 4F;
			this.butReply.Location = new System.Drawing.Point(233, 649);
			this.butReply.Name = "butReply";
			this.butReply.Size = new System.Drawing.Size(75, 24);
			this.butReply.TabIndex = 140;
			this.butReply.Text = "Reply";
			this.butReply.Click += new System.EventHandler(this.butReply_Click);
			// 
			// butNowFinished
			// 
			this.butNowFinished.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNowFinished.Autosize = true;
			this.butNowFinished.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNowFinished.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNowFinished.CornerRadius = 4F;
			this.butNowFinished.Location = new System.Drawing.Point(264, 47);
			this.butNowFinished.Name = "butNowFinished";
			this.butNowFinished.Size = new System.Drawing.Size(62, 24);
			this.butNowFinished.TabIndex = 132;
			this.butNowFinished.Text = "Now";
			this.butNowFinished.Click += new System.EventHandler(this.butNowFinished_Click);
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(21, 649);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(80, 24);
			this.butDelete.TabIndex = 124;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butNow
			// 
			this.butNow.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNow.Autosize = true;
			this.butNow.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNow.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNow.CornerRadius = 4F;
			this.butNow.Location = new System.Drawing.Point(264, 22);
			this.butNow.Name = "butNow";
			this.butNow.Size = new System.Drawing.Size(62, 24);
			this.butNow.TabIndex = 19;
			this.butNow.Text = "Now";
			this.butNow.Click += new System.EventHandler(this.butNow_Click);
			// 
			// textDateTask
			// 
			this.textDateTask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textDateTask.Location = new System.Drawing.Point(107, 538);
			this.textDateTask.Name = "textDateTask";
			this.textDateTask.Size = new System.Drawing.Size(87, 20);
			this.textDateTask.TabIndex = 2;
			// 
			// butCopy
			// 
			this.butCopy.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCopy.Autosize = true;
			this.butCopy.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCopy.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCopy.CornerRadius = 4F;
			this.butCopy.Image = global::OpenDental.Properties.Resources.butCopy;
			this.butCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butCopy.Location = new System.Drawing.Point(653, 540);
			this.butCopy.Name = "butCopy";
			this.butCopy.Size = new System.Drawing.Size(75, 24);
			this.butCopy.TabIndex = 4;
			this.butCopy.Text = "Copy";
			this.butCopy.Click += new System.EventHandler(this.butCopy_Click);
			// 
			// textDescript
			// 
			this.textDescript.AcceptsTab = true;
			this.textDescript.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textDescript.BackColor = System.Drawing.SystemColors.Window;
			this.textDescript.DetectUrls = false;
			this.textDescript.Location = new System.Drawing.Point(107, 85);
			this.textDescript.Name = "textDescript";
			this.textDescript.QuickPasteType = OpenDentBusiness.QuickPasteType.Task;
			this.textDescript.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textDescript.Size = new System.Drawing.Size(703, 108);
			this.textDescript.TabIndex = 1;
			this.textDescript.Text = "";
			this.textDescript.TextChanged += new System.EventHandler(this.textDescript_TextChanged);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(653, 649);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 4;
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
			this.butCancel.Location = new System.Drawing.Point(735, 649);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 5;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// comboJobs
			// 
			this.comboJobs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboJobs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboJobs.FormattingEnabled = true;
			this.comboJobs.Location = new System.Drawing.Point(431, 540);
			this.comboJobs.Name = "comboJobs";
			this.comboJobs.Size = new System.Drawing.Size(208, 21);
			this.comboJobs.TabIndex = 163;
			this.comboJobs.Visible = false;
			this.comboJobs.SelectedIndexChanged += new System.EventHandler(this.comboJobs_SelectedIndexChanged);
			// 
			// labelJobs
			// 
			this.labelJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelJobs.Location = new System.Drawing.Point(382, 540);
			this.labelJobs.Name = "labelJobs";
			this.labelJobs.Size = new System.Drawing.Size(47, 19);
			this.labelJobs.TabIndex = 162;
			this.labelJobs.Text = "Jobs";
			this.labelJobs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelJobs.Visible = false;
			// 
			// FormTaskEdit
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(836, 676);
			this.Controls.Add(this.comboJobs);
			this.Controls.Add(this.labelJobs);
			this.Controls.Add(this.butAudit);
			this.Controls.Add(this.butColor);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.comboTaskPriorities);
			this.Controls.Add(this.butBlue);
			this.Controls.Add(this.butRed);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.textTaskNum);
			this.Controls.Add(this.labelTaskNum);
			this.Controls.Add(this.checkDone);
			this.Controls.Add(this.labelDoneAffectsAll);
			this.Controls.Add(this.checkNew);
			this.Controls.Add(this.butChangeUser);
			this.Controls.Add(this.butAddNote);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.comboDateType);
			this.Controls.Add(this.textTaskList);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.butSend);
			this.Controls.Add(this.labelReply);
			this.Controls.Add(this.butReply);
			this.Controls.Add(this.butNowFinished);
			this.Controls.Add(this.textDateTimeFinished);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.textUser);
			this.Controls.Add(this.label16);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butNow);
			this.Controls.Add(this.textDateTimeEntry);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.panelObject);
			this.Controls.Add(this.listObjectType);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.textDateTask);
			this.Controls.Add(this.butCopy);
			this.Controls.Add(this.textDescript);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.checkFromNum);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(852, 714);
			this.Name = "FormTaskEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Task";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTaskEdit_FormClosing);
			this.Load += new System.EventHandler(this.FormTaskListEdit_Load);
			this.panelObject.ResumeLayout(false);
			this.panelObject.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormTaskListEdit_Load(object sender,System.EventArgs e) {
			#if DEBUG
				labelTaskNum.Visible=true;
				textTaskNum.Visible=true;
				textTaskNum.Text=TaskCur.TaskNum.ToString();
			#endif
			if(PrefC.IsODHQ) {//If HQ
				labelTaskNum.Visible=true;
				textTaskNum.Visible=true;
				textTaskNum.Text=TaskCur.TaskNum.ToString();
				if(!IsNew) {//to simplify the code, only allow jobs to be attached to existing tasks, not new tasks.
					comboJobs.Visible=true;
					labelJobs.Visible=true;
					FillComboJobs();
				}
			}
			if(IsNew) {
				//butDelete.Enabled always stays true
				//textDescript always editable
			}
			else {//trying to edit an existing task, so need to block some things
				bool isTaskForCurUser=true;
				if(TaskCur.UserNum!=Security.CurUser.UserNum) {//current user didn't write this task, so block them.
					isTaskForCurUser=false;//Delete will only be enabled if the user has the TaskEdit and TaskNoteEdit permissions.
				}
				if(TaskCur.TaskListNum!=Security.CurUser.TaskListInBox) {//the task is not in the logged-in user's inbox
					isTaskForCurUser=false;
				}
				if(isTaskForCurUser) {//this just allows getting the NoteList less often
					NoteList=TaskNotes.GetForTask(TaskCur.TaskNum);//so we can check so see if other users have added notes
					for(int i=0;i<NoteList.Count;i++) {
						if(Security.CurUser.UserNum!=NoteList[i].UserNum) {
							isTaskForCurUser=false;
							break;
						}
					}
				}
				if(!isTaskForCurUser && !Security.IsAuthorized(Permissions.TaskNoteEdit,true)) {//but only need to block them if they don't have TaskNoteEdit permission
					butDelete.Enabled=false;
				}
				if(!isTaskForCurUser && !Security.IsAuthorized(Permissions.TaskEdit,true)) {
					butDelete.Enabled=false;
					textDescript.ReadOnly=true;
					textDescript.BackColor=System.Drawing.SystemColors.Window;
				}
			}
			_listTaskPriorities=new List<Def>();
			_listTaskPriorities.AddRange(DefC.GetList(DefCat.TaskPriorities));//Fill list with non-hidden priorities.  We do this because we need a list instead of an array.
			//There must be at least one priority in Setup | Definitions.  Do not let them load the task edit window without at least one priority.
			if(_listTaskPriorities.Count < 1) {
				MsgBox.Show(this,"There are no task priorities in Setup | Definitions.  There must be at least one in order to use the task system.");
				DialogResult=DialogResult.Cancel;
				Close();
			}
			bool hasDefault=false;
			_pritoryDefNumSelected=TaskCur.PriorityDefNum;
			comboTaskPriorities.Items.Clear();
			for(int i=0;i<_listTaskPriorities.Count;i++) {//Add non-hidden defs first
				comboTaskPriorities.Items.Add(_listTaskPriorities[i].ItemName);
				if(!IsNew && _pritoryDefNumSelected==_listTaskPriorities[i].DefNum) {//Task is not new, we should select the priority it is listed with in the DB.
					comboTaskPriorities.SelectedIndex=i;//Sets combo text too
				}
				if((IsNew || _pritoryDefNumSelected==0) && _listTaskPriorities[i].ItemValue=="D") {//Task is new or does not have a priority.
					//Select the default value in the combo box and assign it to the task.
					comboTaskPriorities.SelectedIndex=i;
					_pritoryDefNumSelected=_listTaskPriorities[i].DefNum;
					hasDefault=true;
				}
			}
			if((IsNew || _pritoryDefNumSelected==0) && !hasDefault) {//If no default has been set in the definitions, select the last item in the list.
				comboTaskPriorities.SelectedIndex=comboTaskPriorities.Items.Count-1;
				_pritoryDefNumSelected=_listTaskPriorities[_listTaskPriorities.Count-1].DefNum;
			}
			if(TaskListCur!=null && IsNew && TaskListCur.TaskListNum==1697 && PrefC.GetBool(PrefName.DockPhonePanelShow)) {//Set to triage blue if HQ, triage list, and is new.
				for(int i=0;i<_listTaskPriorities.Count;i++) {
					if(_listTaskPriorities[i].DefNum==_triageBlueNum) {//Finding the option that is triageBlue to select it in the combobox (Combobox mirrors _listTaskPriorityDefs)
						comboTaskPriorities.SelectedIndex=i;
						break;
					}
				}
				_pritoryDefNumSelected=_triageBlueNum;
			}
			if(comboTaskPriorities.SelectedIndex==-1) {//Priority for task wasn't found in the non-hidden priorities list (and isn't triageBlue), so it must be a hidden priority.
				Def[] arrayTaskDefsLong=DefC.Long[(int)DefCat.TaskPriorities];//Get all priorities
				for(int i=0;i<arrayTaskDefsLong.Length;i++) {
					if(arrayTaskDefsLong[i].DefNum==_pritoryDefNumSelected) {//We find the hidden priority and set the text of the combo box.
						comboTaskPriorities.Text=(arrayTaskDefsLong[i].ItemName+" (Hidden)");
						butColor.BackColor=arrayTaskDefsLong[i].ItemColor;
					}
				}
			}
			textUser.Text=Userods.GetName(TaskCur.UserNum);//might be blank.
			if(TaskListCur!=null) {
				textTaskList.Text=TaskListCur.Descript;
			}
			if(TaskCur.DateTimeEntry.Year<1880) {
				textDateTimeEntry.Text=DateTime.Now.ToString();
			}
			else {
				textDateTimeEntry.Text=TaskCur.DateTimeEntry.ToString();
			}
			if(TaskCur.DateTimeFinished.Year<1880) {
				textDateTimeFinished.Text="";//DateTime.Now.ToString();
			}
			else {
				textDateTimeFinished.Text=TaskCur.DateTimeFinished.ToString();
			}
			textDescript.Text=TaskCur.Descript;
			if(!IsPopup) {//otherwise, TextUser is selected, and it cannot accept input.  This prevents a popup from accepting using input accidentally.
				textDescript.Select();//Focus does not work for some reason.
				textDescript.Select(TaskCur.Descript.Length,0);//Place the cursor at the end of the description box.
			}
			if(PrefC.GetBool(PrefName.TasksNewTrackedByUser) && TaskCur.TaskListNum !=0) {
				long mailboxUserNum=TaskLists.GetMailboxUserNum(TaskCur.TaskListNum);
				if(mailboxUserNum != 0 && mailboxUserNum != Security.CurUser.UserNum) {
					StartedInOthersInbox=true;
					checkNew.Checked=false;
					checkNew.Enabled=false;
				}
			}
			//this section must come after textDescript is set:
			if(TaskCur.TaskStatus==TaskStatusEnum.Done) {//global even if new status is tracked by user
				checkDone.Checked=true;
			}
			else {//because it can't be both new and done.
				if(IsPopup) {//It clearly is Unread, but we don't want to leave it that way upon close OK.
					checkNew.Checked=false;
					StatusChanged=true;
				}
				else if(PrefC.GetBool(PrefName.TasksNewTrackedByUser)) {
					if(!StartedInOthersInbox && TaskUnreads.IsUnread(Security.CurUser.UserNum,TaskCur.TaskNum)) {
						checkNew.Checked=true;
						MightNeedSetRead=true;
					}
				}
				else {//tracked globally, the old way
					if(TaskCur.TaskStatus==TaskStatusEnum.New) {
						checkNew.Checked=true;
					}
				}
			}
			if(TaskCur.DateTask.Year>1880) {
				textDateTask.Text=TaskCur.DateTask.ToShortDateString();
			}
			if(TaskCur.IsRepeating) {
				checkNew.Enabled=false;
				checkDone.Enabled=false;
				textDateTask.Enabled=false;
				listObjectType.Enabled=false;
				if(TaskCur.TaskListNum!=0) {//not a main parent
					comboDateType.Enabled=false;
				}
			}
			for(int i=0;i<Enum.GetNames(typeof(TaskDateType)).Length;i++) {
				comboDateType.Items.Add(Lan.g("enumTaskDateType",Enum.GetNames(typeof(TaskDateType))[i]));
				if((int)TaskCur.DateType==i) {
					comboDateType.SelectedIndex=i;
				}
			}
			if(TaskCur.FromNum==0) {
				checkFromNum.Checked=false;
				checkFromNum.Enabled=false;
			}
			else {
				checkFromNum.Checked=true;
			}
			for(int i=0;i<Enum.GetNames(typeof(TaskObjectType)).Length;i++) {
				listObjectType.Items.Add(Lan.g("enumTaskObjectType",Enum.GetNames(typeof(TaskObjectType))[i]));
			}
			_listTaskPriorities=new List<Def>();
			_listTaskPriorities.AddRange(DefC.GetList(DefCat.TaskPriorities));//Fill list with non-hidden priorities.  We do this because we need a list instead of an array.
			//There must be at least one priority in Setup | Definitions.  Do not let them load the task edit window without at least one priority.
			if(_listTaskPriorities.Count < 1) {
				MsgBox.Show(this,"There are no task priorities in Setup | Definitions.  There must be at least one in order to use the task system.");
				DialogResult=DialogResult.Cancel;
				Close();
			}
			FillObject();
			FillGrid();//Need this in order to pick ReplyToUserNum next.
			if(IsNew) {
				labelReply.Visible=false;
				butReply.Visible=false;
			}
			else if(TaskListCur==null) {
				//|| TaskListCur.TaskListNum!=Security.CurUser.TaskListInBox) {//if this task is not in my inbox
				labelReply.Visible=false;
				butReply.Visible=false;
			}
			else if(NoteList.Count==0 && TaskCur.UserNum==Security.CurUser.UserNum) {//if this is my task
				labelReply.Visible=false;
				butReply.Visible=false;
			}
			else {//reply button will be visible
				if(NoteList.Count==0) {//no notes, so reply to owner
					ReplyToUserNum=TaskCur.UserNum;
				}
				else {//reply to most recent author who is not me
					//loop backward through the notes to find who to reply to
					for(int i=NoteList.Count-1;i>=0;i--) {
						if(NoteList[i].UserNum!=Security.CurUser.UserNum) {
							ReplyToUserNum=NoteList[i].UserNum;
							break;
						}
					}
					if(ReplyToUserNum==0) {//can't figure out who to reply to.
						labelReply.Visible=false;
						butReply.Visible=false;
					}
				}
				labelReply.Text=Lan.g(this,"(Send to ")+Userods.GetName(ReplyToUserNum)+")";
			}
			if(PrefC.GetBool(PrefName.DockPhonePanelShow)) {//Show red and blue buttons for HQ always
				butRed.Visible=true;
				butBlue.Visible=true;
			}
			if(!Security.IsAuthorized(Permissions.TaskEdit,true)){
				butAudit.Visible=false;
			}
		}

		private void FillComboJobs() {
			if(!PrefC.IsODHQ) {
				return;
			}
			_isLoading=true;
			comboJobs.Items.Clear();
			_jobLinks = JobLinks.GetForTask(this.TaskCur.TaskNum);
			_listJobs = Jobs.GetMany(_jobLinks.Select(x => x.JobNum).ToList());
			foreach(Job job in _listJobs) {
				comboJobs.Items.Add(job.ToString());
			}
			if(_listJobs.Count==0) {
				comboJobs.Items.Add("None");
			}
			comboJobs.Items.Add("Attach");
			comboJobs.SelectedIndex=0;
			labelJobs.Text=Lan.g(this,"Jobs")+" ("+_listJobs.Count+")";
			_isLoading=false;
		}

		private void comboJobs_SelectedIndexChanged(object sender,EventArgs e) {
			if(_isLoading) {
				return;
			}
			if(comboJobs.SelectedIndex<1 && _listJobs.Count==0) {
				return;//selected none
			}
			if(comboJobs.SelectedIndex==comboJobs.Items.Count-1) {
				//Atach new job
				FormJobSearch FormJS = new FormJobSearch();
				FormJS.ShowDialog();
				if(FormJS.DialogResult!=DialogResult.OK || FormJS.SelectedJob==null) {
					return;
				}
				JobLink jobLink = new JobLink();
				jobLink.JobNum=FormJS.SelectedJob.JobNum;
				jobLink.FKey=TaskCur.TaskNum;
				jobLink.LinkType=JobLinkType.Task;
				JobLinks.Insert(jobLink);
				Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,jobLink.JobNum);
				FillComboJobs();
				return;
			}
			FormOpenDental.GoToJob(_listJobs[comboJobs.SelectedIndex].JobNum);
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Date Time"),120);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"User"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Note"),400);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			NoteList=TaskNotes.GetForTask(TaskCur.TaskNum);
			//Only do weird logic when editing a task associated with the triage task list.
			if(PrefC.GetBool(PrefName.DockPhonePanelShow)) {
				if(_numNotes==-1) {//Only fill _numNotes here the first time FillGrid is called.  This is used for coloring triage tasks.
					_numNotes=NoteList.Count;
				}
				if(_pritoryDefNumSelected==_triageBlueNum && _numNotes==0 && NoteList.Count!=0) {//Blue triage task with an added note
					_pritoryDefNumSelected=_triageWhiteNum;//Change priority to white
					for(int i=0;i<_listTaskPriorities.Count;i++) {
						if(_listTaskPriorities[i].DefNum==_triageWhiteNum) {
							comboTaskPriorities.SelectedIndex=i;//Change selection to the triage white
						}
					}
				}
				else if(_pritoryDefNumSelected==_triageWhiteNum && _numNotes!=0 && NoteList.Count==0) {//White triage task with note that has been deleted, change it back to blue.
					_pritoryDefNumSelected=_triageBlueNum;
					for(int i=0;i<_listTaskPriorities.Count;i++) {
						if(_listTaskPriorities[i].DefNum==_triageBlueNum) {
							comboTaskPriorities.SelectedIndex=i;//Change selection to the triage blue
						}
					}
				}
				_numNotes=NoteList.Count;
			}
			for(int i=0;i<NoteList.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(NoteList[i].DateTimeNote.ToShortDateString()+" "+NoteList[i].DateTimeNote.ToShortTimeString());
				row.Cells.Add(Userods.GetName(NoteList[i].UserNum));
				row.Cells.Add(NoteList[i].Note);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			gridMain.ScrollToEnd();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(OwnedForms.Length>0) {
				MsgBox.Show(this,"One or more task note edit windows are open and must be closed.");
				return;
			}
			FormTaskNoteEdit form=new FormTaskNoteEdit();
			form.TaskNoteCur=NoteList[e.Row];
			form.EditComplete=OnNoteEditComplete_CellDoubleClick;
			form.Show(this);//non-modal subwindow, but if the parent is closed by the user when the child is open, then the child is forced closed along with the parent and after the parent.
		}

		private void OnNoteEditComplete_CellDoubleClick(object sender) {
			NotesChanged=true;
			if(TaskOld.TaskStatus==TaskStatusEnum.Done) {//If task was marked Done when opened, we uncheck the Done checkbox so people can see the changes.
				checkDone.Checked=false;
			}
			FillGrid();
		}

		private void butAddNote_Click(object sender,EventArgs e) {
			if(OwnedForms.Length>0) {
				MsgBox.Show(this,"One or more task note edit windows are open and must be closed.");
				return;
			}
			FormTaskNoteEdit form=new FormTaskNoteEdit();
			form.TaskNoteCur=new TaskNote();
			form.TaskNoteCur.TaskNum=TaskCur.TaskNum;
			form.TaskNoteCur.DateTimeNote=DateTime.Now;//Will be slightly adjusted at server.
			form.TaskNoteCur.UserNum=Security.CurUser.UserNum;
			form.TaskNoteCur.IsNew=true;
			form.TaskNoteCur.Note="";
			form.EditComplete=OnNoteEditComplete_Add;
			form.Show(this);//non-modal subwindow, but if the parent is closed by the user when the child is open, then the child is forced closed along with the parent and after the parent.
		}

		private void OnNoteEditComplete_Add(object sender) {
			NotesChanged=true;
			if(TaskOld.TaskStatus==TaskStatusEnum.Done) {//If task was marked Done when opened, we uncheck the Done checkbox so people can see the changes.
				checkDone.Checked=false;
			}
			FillGrid();
			if(MightNeedSetRead) {//'new' box is checked
				checkNew.Checked=false;
				StatusChanged=true;
				MightNeedSetRead=false;//so that the automation won't happen again
			}
		}

		private void checkNew_Click(object sender,EventArgs e) {
			if(checkNew.Checked && checkDone.Checked) {
				checkDone.Checked=false;
			}
			StatusChanged=true;
			MightNeedSetRead=false;//don't override user's intent
		}

		private void checkDone_Click(object sender,EventArgs e) {
			if(checkNew.Checked && checkDone.Checked) {
				checkNew.Checked=false;
			}
			MightNeedSetRead=false;//don't override user's intent
		}

		private void FillObject() {
			if(TaskCur.ObjectType==TaskObjectType.None) {
				listObjectType.SelectedIndex=0;
				panelObject.Visible=false;
			}
			else if(TaskCur.ObjectType==TaskObjectType.Patient) {
				listObjectType.SelectedIndex=1;
				panelObject.Visible=true;
				labelObjectDesc.Text=Lan.g(this,"Patient Name");
				if(TaskCur.KeyNum>0) {
					textObjectDesc.Text=Patients.GetPat(TaskCur.KeyNum).GetNameLF();
				}
				else {
					textObjectDesc.Text="";
				}
			}
			else if(TaskCur.ObjectType==TaskObjectType.Appointment) {
				listObjectType.SelectedIndex=2;
				panelObject.Visible=true;
				labelObjectDesc.Text=Lan.g(this,"Appointment Desc");
				if(TaskCur.KeyNum>0) {
					Appointment AptCur=Appointments.GetOneApt(TaskCur.KeyNum);
					if(AptCur==null) {
						textObjectDesc.Text=Lan.g(this,"(appointment deleted)");
					}
					else {
						textObjectDesc.Text=Patients.GetPat(AptCur.PatNum).GetNameLF()
							+"  "+AptCur.AptDateTime.ToString()
							+"  "+AptCur.ProcDescript
							+"  "+AptCur.Note;
					}
				}
				else {
					textObjectDesc.Text="";
				}
			}
		}

		private void butNow_Click(object sender,EventArgs e) {
			textDateTimeEntry.Text=MiscData.GetNowDateTime().ToString();
		}

		private void butNowFinished_Click(object sender,EventArgs e) {
			textDateTimeFinished.Text=MiscData.GetNowDateTime().ToString();
		}

		private void butBlue_Click(object sender,EventArgs e) {
			if(_pritoryDefNumSelected==_triageBlueNum) {//Blue button is clicked while it's already blue
				_pritoryDefNumSelected=_triageWhiteNum;//Change to white.
				for(int i=0;i<_listTaskPriorities.Count;i++) {
					if(_listTaskPriorities[i].DefNum==_triageWhiteNum) {
						comboTaskPriorities.SelectedIndex=i;//Change selection to the triage white
					}
				}	
			}
			else {//Blue button is clicked while it's red or white, simply change it to blue
				_pritoryDefNumSelected=_triageBlueNum;//Change to blue.
				for(int i=0;i<_listTaskPriorities.Count;i++) {
					if(_listTaskPriorities[i].DefNum==_triageBlueNum) {
						comboTaskPriorities.SelectedIndex=i;//Change selection to the triage blue
					}
				}	
			}
		}

		private void butRed_Click(object sender,EventArgs e) {
			if(_pritoryDefNumSelected==_triageRedNum) {//Red button is clicked while it's already red
				_pritoryDefNumSelected=_triageWhiteNum;//Change to white.
				for(int i=0;i<_listTaskPriorities.Count;i++) {
					if(_listTaskPriorities[i].DefNum==_triageWhiteNum) {
						comboTaskPriorities.SelectedIndex=i;//Change combo selection to the triage white
					}
				}	
			}
			else {//Red button is clicked while it's blue or white, simply change it to red
				_pritoryDefNumSelected=_triageRedNum;//Change to red.
				for(int i=0;i<_listTaskPriorities.Count;i++) {
					if(_listTaskPriorities[i].DefNum==_triageRedNum) {
						comboTaskPriorities.SelectedIndex=i;//Change combo selection to the triage red
					}
				}	
			}
		}

		///<summary>This event is fired whenever the combo box is changed manually or the index is changed programmatically.</summary>
		private void comboTaskPriorities_SelectedIndexChanged(object sender,EventArgs e) {
			_pritoryDefNumSelected=_listTaskPriorities[comboTaskPriorities.SelectedIndex].DefNum;
			butColor.BackColor=DefC.GetColor(DefCat.TaskPriorities,_pritoryDefNumSelected);//Change the color swatch so people know the priority's color
		}

		private void listObjectType_MouseDown(object sender,System.Windows.Forms.MouseEventArgs e) {
			if(TaskCur.KeyNum>0) {
				if(!MsgBox.Show(this,true,"The linked object will no longer be attached.  Continue?")) {
					FillObject();
					return;
				}
			}
			TaskCur.KeyNum=0;
			TaskCur.ObjectType=(TaskObjectType)listObjectType.SelectedIndex;
			FillObject();
		}

		private void butAudit_Click(object sender,EventArgs e) {
			FormTaskHist FormTH=new FormTaskHist();
			FormTH.TaskNumCur=TaskCur.TaskNum;
			FormTH.Show();
		}

		private void butChange_Click(object sender,System.EventArgs e) {
			FormPatientSelect FormPS=new FormPatientSelect();
			FormPS.SelectionModeOnly=true;
			FormPS.ShowDialog();
			if(FormPS.DialogResult!=DialogResult.OK) {
				return;
			}
			if(TaskCur.ObjectType==TaskObjectType.Patient) {
				TaskCur.KeyNum=FormPS.SelectedPatNum;
			}
			if(TaskCur.ObjectType==TaskObjectType.Appointment) {
				FormApptsOther FormA=new FormApptsOther(FormPS.SelectedPatNum);
				FormA.SelectOnly=true;
				FormA.ShowDialog();
				if(FormA.DialogResult==DialogResult.Cancel) {
					return;
				}
				TaskCur.KeyNum=FormA.AptNumsSelected[0];
			}
			FillObject();
		}

		private void butGoto_Click(object sender,System.EventArgs e) {
			if(OwnedForms.Length>0) {
				MsgBox.Show(this,"One or more task note edit windows are open and must be closed.");
				return;
			}
			if(!SaveCur()) {
				return;
			}
			GotoType=TaskCur.ObjectType;
			GotoKeyNum=TaskCur.KeyNum;
			DialogResult=DialogResult.OK;
			Close();
		}

		private void butChangeUser_Click(object sender,EventArgs e) {
			FormUserPick FormP=new FormUserPick();
			FormP.ShowDialog();
			if(FormP.DialogResult==DialogResult.OK) {
				TaskCur.UserNum=FormP.SelectedUserNum;
				textUser.Text=Userods.GetName(TaskCur.UserNum);
			}
		}

		private void textDescript_TextChanged(object sender,EventArgs e) {
			if(MightNeedSetRead) {//'new' box is checked
				checkNew.Checked=false;
				StatusChanged=true;
				MightNeedSetRead=false;//so that the automation won't happen again
			}
			if(TaskOld.TaskStatus==TaskStatusEnum.Done && textDescript.Text!=TaskOld.Descript) {
				checkDone.Checked=false;
			}
		}

		private void butCopy_Click(object sender,EventArgs e) {
			string taskText=TaskCur.DateTimeEntry.ToShortDateString()+" "+TaskCur.DateTimeEntry.ToShortTimeString()+(textObjectDesc.Visible?" - "+textObjectDesc.Text:"")+" - "+textUser.Text+" - "+textDescript.Text;
			for(int i=0;i<NoteList.Count;i++) {
				taskText+="\r\n--------------------------------------------------\r\n";
				taskText+="=="+Userods.GetName(NoteList[i].UserNum)+" - "+NoteList[i].DateTimeNote.ToShortDateString()+" "+NoteList[i].DateTimeNote.ToShortTimeString()+" - "+NoteList[i].Note;
			}
			System.Windows.Forms.Clipboard.SetText(taskText);
			Tasks.TaskEditCreateLog(Lan.g(this,"Copied Task Note"),TaskCur);
		}

		private bool SaveCur() {
			if(textDateTask.errorProvider1.GetError(textDateTask)!="") {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return false;
			}
			if(textDateTimeEntry.Text!="") {
				try {
					DateTime.Parse(textDateTimeEntry.Text);
				}
				catch {
					MsgBox.Show(this,"Please fix Date/Time Entry.");
					return false;
				}
			}
			if(textDateTimeFinished.Text!="") {
				try {
					DateTime.Parse(textDateTimeFinished.Text);
				}
				catch {
					MsgBox.Show(this,"Please fix Date/Time Finished.");
					return false;
				}
			}
			if(TaskCur.TaskListNum==-1) {
				MsgBox.Show(this,"Since no task list is selected, the Send To button must be used.");
				return false;
			}
			if(textDescript.Text=="") {
				MsgBox.Show(this,"Please enter a description.");
				return false;
			}
			//Techs want to be notified of any changes made to completed tasks.
			//Check if the task list changed on a task marked Done.
			if(TaskCur.TaskListNum!=TaskOld.TaskListNum	&& TaskOld.TaskStatus==TaskStatusEnum.Done) {
				//Forcing the status to viewed will put the task in other user's "New for" task list but not the user that made the change.
				TaskCur.TaskStatus=TaskStatusEnum.Viewed;
				checkDone.Checked=false;
			}
			if(checkDone.Checked) {
				TaskCur.TaskStatus=TaskStatusEnum.Done;//global even if new status is tracked by user
				TaskUnreads.DeleteForTask(TaskCur.TaskNum);//clear out taskunreads. We have too many tasks to read the done ones.
			}
			else {//because it can't be both new and done.
				if(PrefC.GetBool(PrefName.TasksNewTrackedByUser)) {
					if(TaskCur.TaskStatus==TaskStatusEnum.Done) {
						TaskCur.TaskStatus=TaskStatusEnum.Viewed;
					}
					//This is done explicitly instead of automatically like it was the old way
					if(!StartedInOthersInbox) {
						if(checkNew.Checked) {
							TaskUnreads.SetUnread(Security.CurUser.UserNum,TaskCur.TaskNum);
						}
						else {
							TaskUnreads.SetRead(Security.CurUser.UserNum,TaskCur.TaskNum);
						}
					}
				}
				else {//tracked globally, the old way
					if(checkNew.Checked) {
						TaskCur.TaskStatus=TaskStatusEnum.New;
					}
					else {
						TaskCur.TaskStatus=TaskStatusEnum.Viewed;
					}
				}
			}
			//UserNum no longer allowed to change automatically
			//if(resetUser && TaskCur.Descript!=textDescript.Text){
			//	TaskCur.UserNum=Security.CurUser.UserNum;
			//}
			TaskCur.DateTimeEntry=PIn.DateT(textDateTimeEntry.Text);
			if(TaskCur.TaskStatus==TaskStatusEnum.Done && textDateTimeFinished.Text=="") {
				TaskCur.DateTimeFinished=DateTime.Now;
			}
			else {
				TaskCur.DateTimeFinished=PIn.DateT(textDateTimeFinished.Text);
			}
			TaskCur.Descript=textDescript.Text;
			TaskCur.DateTask=PIn.Date(textDateTask.Text);
			TaskCur.DateType=(TaskDateType)comboDateType.SelectedIndex;
			if(!checkFromNum.Checked) {//user unchecked the box. Never allowed to check if initially unchecked
				TaskCur.FromNum=0;
			}
			//ObjectType already handled
			//Cur.KeyNum already handled
			TaskCur.PriorityDefNum=_pritoryDefNumSelected;
			try {
				if(IsNew) {
					TaskCur.IsNew=true;
					Tasks.Update(TaskCur,TaskOld);
				}
				else {
					if(!TaskCur.Equals(TaskOld)) {//If user clicks OK without making any changes, then skip.
						Tasks.Update(TaskCur,TaskOld);//if task has already been altered, then this is where it will fail.
					}
					if(!TaskCur.Equals(TaskOld) || NotesChanged) {//We want to make a TaskHist entry if notes were changed as well as if the task was changed.
						TaskHist taskHist=new TaskHist(TaskOld);
						taskHist.UserNumHist=Security.CurUser.UserNum;
						taskHist.IsNoteChange=NotesChanged;
						TaskHists.Insert(taskHist);
					}
				}
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return false;
			}
			return true;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(OwnedForms.Length>0) {
				MsgBox.Show(this,"One or more task note edit windows are open and must be closed.");
				return;
			}
			if(!IsNew) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete?")) {
					return;
				}
				//TaskListNum=-1 is only possible if it's new.  This will never get hit if it's new.
				if(TaskCur.TaskListNum==0) {
					Tasks.TaskEditCreateLog(Lan.g(this,"Deleted task"),TaskCur);
				}
				else {
					Tasks.TaskEditCreateLog(Lan.g(this,"Deleted task from tasklist")+" "+TaskLists.GetOne(TaskCur.TaskListNum).Descript,TaskCur);
				}
			}
			Tasks.Delete(TaskCur.TaskNum);//always do it this way to clean up all four tables
			DataValid.SetInvalidTask(TaskCur.TaskNum,false);//no popup
			TaskHist taskHistory=new TaskHist(TaskOld);
			taskHistory.IsNoteChange=NotesChanged;
			taskHistory.UserNum=Security.CurUser.UserNum;
			TaskHists.Insert(taskHistory);
			DialogResult=DialogResult.OK;
			Close();
		}

		private void butReply_Click(object sender,EventArgs e) {
			//This can't happen if IsNew
			//This also can't happen if the task is mine with no replies.
			//Button not visible unless a ReplyToUserNum has been calculated successfully.
			if(OwnedForms.Length>0) {
				MsgBox.Show(this,"One or more task note edit windows are open and must be closed.");
				return;
			}
			long inbox=Userods.GetInbox(ReplyToUserNum);
			if(inbox==0) {
				MsgBox.Show(this,"No inbox has been set up for this user yet.");
				return;
			}
			if(!NotesChanged && textDescript.Text==TaskCur.Descript) {//nothing changed
				FormTaskNoteEdit form=new FormTaskNoteEdit();
				form.TaskNoteCur=new TaskNote();
				form.TaskNoteCur.TaskNum=TaskCur.TaskNum;
				form.TaskNoteCur.DateTimeNote=DateTime.Now;//Will be slightly adjusted at server.
				form.TaskNoteCur.UserNum=Security.CurUser.UserNum;
				form.TaskNoteCur.IsNew=true;
				form.TaskNoteCur.Note="";
				form.EditComplete=OnNoteEditComplete_Reply;
				form.Show(this);//non-modal subwindow, but if the parent is closed by the user when the child is open, then the child is forced closed along with the parent and after the parent.
				return;
			}
			TaskCur.TaskListNum=inbox;
			if(!SaveCur()) {
				return;
			}
			DataValid.SetInvalidTask(TaskCur.TaskNum,true);//popup
			DialogResult=DialogResult.OK;
			Close();
		}

		private void OnNoteEditComplete_Reply(object sender) {
			if(MightNeedSetRead) {//'new' box is checked
				checkNew.Checked=false;
				StatusChanged=true;
				MightNeedSetRead=false;//so that the automation won't happen again
			}
			TaskCur.TaskListNum=Userods.GetInbox(ReplyToUserNum);
			if(!SaveCur()) {
				return;
			}
			DataValid.SetInvalidTask(TaskCur.TaskNum,true);//popup
			DialogResult=DialogResult.OK;
			Close();
		}

		///<summary>Send to another user.</summary>
		private void butSend_Click(object sender,EventArgs e) {
			//This button is always present.
			if(OwnedForms.Length>0) {
				MsgBox.Show(this,"One or more task note edit windows are open and must be closed.");
				return;
			}
			if(listObjectType.SelectedIndex==(int)TaskObjectType.Patient) {
				FormTaskListSelect FormT=new FormTaskListSelect(TaskObjectType.Patient);
				FormT.ShowDialog();
				if(FormT.DialogResult!=DialogResult.OK) {
					return;
				}
				TaskCur.TaskListNum=FormT.SelectedTaskListNum;
				TaskListCur=TaskLists.GetOne(TaskCur.TaskListNum);
				textTaskList.Text=TaskListCur.Descript;
				if(!SaveCur()) {
					return;
				}
			}
			else {//to an in-box
				FormTaskSendUser FormT=new FormTaskSendUser();
				FormT.ShowDialog();
				if(FormT.DialogResult!=DialogResult.OK) {
					return;
				}
				TaskCur.TaskListNum=FormT.TaskListNum;
				TaskListCur=TaskLists.GetOne(TaskCur.TaskListNum);
				textTaskList.Text=TaskListCur.Descript;
				if(!SaveCur()) {
					return;
				}
			}
			//Check for changes.  If something changed, send a signal.
			if(NotesChanged || !TaskCur.Equals(TaskOld) || StatusChanged) {
				DataValid.SetInvalidTask(TaskCur.TaskNum,true);//popup
			}
			DialogResult=DialogResult.OK;
			Close();
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(OwnedForms.Length>0) {
				MsgBox.Show(this,"One or more task note edit windows are open and must be closed.");
				return;
			}
			if(!SaveCur()) {//If user clicked OK without changing anything, then this will have no effect.
				return;
			}
			if(!NotesChanged && TaskCur.Equals(TaskOld) && !StatusChanged) {//if there were no changes, then don't bother with the signal
				DialogResult=DialogResult.OK;
				Close();
				return;
			}
			if(IsNew) {
				DataValid.SetInvalidTask(TaskCur.TaskNum,true);//popup
			}
			else if(NotesChanged || textDescript.Text!=TaskCur.Descript) {//notes or descript changed
				DataValid.SetInvalidTask(TaskCur.TaskNum,true);//popup
			}
			else {
				DataValid.SetInvalidTask(TaskCur.TaskNum,false);//no popup
			}
			DialogResult=DialogResult.OK;
			Close();
		}

		private void butCancel_Click(object sender,System.EventArgs e) {
			if(OwnedForms.Length>0) {
				MsgBox.Show(this,"One or more task note edit windows are open and must be closed.");
				return;
			}
			DialogResult=DialogResult.Cancel;
			Close();
		}

		private void FormTaskEdit_FormClosing(object sender,FormClosingEventArgs e) {
			if(DialogResult==DialogResult.None && OwnedForms.Length>0) {//This can only happen if the user closes the window using the X in the upper right.
				MsgBox.Show(this,"One or more task note edit windows are open and must be closed.");
				e.Cancel=true;
				return;
			}
			if(PrefC.GetBool(PrefName.TasksNewTrackedByUser)) {
				//No more automation here
			}
			else {
				if(Security.CurUser!=null) {//Because tasks are modal, user may log off and this form may close with no user.
					TaskUnreads.SetRead(Security.CurUser.UserNum,TaskCur.TaskNum);//no matter why it was closed
				}
			}
			if(DialogResult==DialogResult.OK) {
				return;
			}
			if(IsNew) {
				Tasks.Delete(TaskCur.TaskNum);
			}
			//If a note was added to a Done task and the user hits cancel, the task status is set to Viewed because the note is still there and the task didn't move lists.
			if(NotesChanged && TaskOld.TaskStatus==TaskStatusEnum.Done) {//notes changed on a task marked Done when the task was opened.
				if(checkDone.Checked) {//Will only happen when the Done checkbox has been manually re-checked by the user.
					return;
				}
				TaskCur.TaskStatus=TaskStatusEnum.Viewed;
				try {
					Tasks.Update(TaskCur,TaskOld);//if task has already been altered, then this is where it will fail.
				}
				catch {
					return;//Silently leave because the user could be trying to cancel out of a task that had been edited by another user.
				}
				DataValid.SetInvalidTask(TaskCur.TaskNum,false);//no popup
			}
		}
	}
}





















