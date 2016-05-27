using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental{
///<summary></summary>
	public class FormRecallSetup : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox textPostcardsPerSheet;
		private System.Windows.Forms.CheckBox checkReturnAdd;
		private GroupBox groupBox2;
		private ValidDouble textDown;
		private Label label12;
		private ValidDouble textRight;
		private Label label13;
		private CheckBox checkGroupFamilies;
		private Label label14;
		private Label label15;
		private GroupBox groupBox3;
		private Label label25;
		private ComboBox comboStatusMailedRecall;
		private ComboBox comboStatusEmailedRecall;
		private Label label26;
		private ListBox listTypes;
		private Label label1;
		private ValidNumber textDaysPast;
		private ValidNumber textDaysFuture;
		private GroupBox groupBox1;
		private ValidNumber textDaysSecondReminder;
		private ValidNumber textDaysFirstReminder;
		private Label label2;
		private Label label3;
		private OpenDental.UI.ODGrid gridMain;
		private System.ComponentModel.Container components = null;
		private ValidNumber textMaxReminders;
		private Label label4;
		private ComboBox comboStatusEmailedConfirm;
		private Label label5;
		private GroupBox groupBox4;
		private RadioButton radioUseEmailFalse;
		private RadioButton radioUseEmailTrue;
		private Label label6;
		private ComboBox comboStatusTextMessagedConfirm;
		private RadioButton radioExcludeFutureNo;
		private RadioButton radioExcludeFutureYes;
		private TabControl tabControlSetup;
		private TabPage tabRecallConfirmationSetup;
		private TabPage tabConfirmationAutomation;
		private ODGrid gridConfirmationRules;
		private UI.Button butAdd;
		private TabPage tabReminderSetup;
		private CheckBox checkSendAll;
		private Label labelTextMessage;
		private UI.Button butDown;
		private UI.Button butUp;
		private ODGrid gridPriorities;
		private Label label10;
		private Label label11;
		private ValidDouble textHoursPrior;
		private ValidDouble textDaysPrior;
		private bool _changed;
		private RichTextBox textMessageText;
		private Label label16;
		private TabPage tabAutomationSettings;
		private GroupBox groupBox5;
		private RadioButton radioSendToEmailNoPreferred;
		private RadioButton radioSendToEmail;
		private Label label19;
		private GroupBox groupBox6;
		private DateTimePicker dateRunEnd;
		private DateTimePicker dateRunStart;
		private Label label17;
		private Label label18;
		private GroupBox groupBox7;
		private GroupBox groupBox8;
		private RadioButton radioSendToEmailOnlyPreferred;
		private RadioButton radioDoNotSend;
		private ValidNumber textTextCount;
		private Label label23;
		private Label label20;
		private ValidNumber textApptCount;
		private Label label31;
		private ValidNumber textNeitherCount;
		private Label label30;
		private ValidNumber textEmailCount;
		private Label label29;
		private GroupBox groupBox9;
		private string[] _arrayPriorities;
		private double _daysPrior;
		private double _hoursPrior;
		private TimeSpan _automationStart;
		private GroupBox groupReminders;
		private GroupBox groupBox10;
		private Label label7;
		private Label label36;
		private GroupBox groupBox11;
		private Label label38;
		private RichTextBox textMessageEmail;
		private TimeSpan _automationEnd;

		///<summary></summary>
		public FormRecallSetup(){
			InitializeComponent();
			Lan.F(this);
			//Lan.C(this, new System.Windows.Forms.Control[] {
				//textBox1,
				//textBox6
			//});
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
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRecallSetup));
			this.textPostcardsPerSheet = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.checkReturnAdd = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.textDown = new OpenDental.ValidDouble();
			this.label12 = new System.Windows.Forms.Label();
			this.textRight = new OpenDental.ValidDouble();
			this.label13 = new System.Windows.Forms.Label();
			this.checkGroupFamilies = new System.Windows.Forms.CheckBox();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.textDaysFuture = new OpenDental.ValidNumber();
			this.textDaysPast = new OpenDental.ValidNumber();
			this.label25 = new System.Windows.Forms.Label();
			this.comboStatusMailedRecall = new System.Windows.Forms.ComboBox();
			this.comboStatusEmailedRecall = new System.Windows.Forms.ComboBox();
			this.label26 = new System.Windows.Forms.Label();
			this.listTypes = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.textMaxReminders = new OpenDental.ValidNumber();
			this.label4 = new System.Windows.Forms.Label();
			this.textDaysSecondReminder = new OpenDental.ValidNumber();
			this.textDaysFirstReminder = new OpenDental.ValidNumber();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.comboStatusEmailedConfirm = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.radioUseEmailFalse = new System.Windows.Forms.RadioButton();
			this.radioUseEmailTrue = new System.Windows.Forms.RadioButton();
			this.label6 = new System.Windows.Forms.Label();
			this.comboStatusTextMessagedConfirm = new System.Windows.Forms.ComboBox();
			this.radioExcludeFutureNo = new System.Windows.Forms.RadioButton();
			this.radioExcludeFutureYes = new System.Windows.Forms.RadioButton();
			this.tabControlSetup = new System.Windows.Forms.TabControl();
			this.tabRecallConfirmationSetup = new System.Windows.Forms.TabPage();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.tabConfirmationAutomation = new System.Windows.Forms.TabPage();
			this.butAdd = new OpenDental.UI.Button();
			this.gridConfirmationRules = new OpenDental.UI.ODGrid();
			this.tabReminderSetup = new System.Windows.Forms.TabPage();
			this.groupBox11 = new System.Windows.Forms.GroupBox();
			this.groupReminders = new System.Windows.Forms.GroupBox();
			this.label38 = new System.Windows.Forms.Label();
			this.textMessageEmail = new System.Windows.Forms.RichTextBox();
			this.label36 = new System.Windows.Forms.Label();
			this.labelTextMessage = new System.Windows.Forms.Label();
			this.groupBox10 = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.textHoursPrior = new OpenDental.ValidDouble();
			this.textDaysPrior = new OpenDental.ValidDouble();
			this.label20 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.gridPriorities = new OpenDental.UI.ODGrid();
			this.groupBox9 = new System.Windows.Forms.GroupBox();
			this.label23 = new System.Windows.Forms.Label();
			this.textApptCount = new OpenDental.ValidNumber();
			this.textTextCount = new OpenDental.ValidNumber();
			this.label31 = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.textNeitherCount = new OpenDental.ValidNumber();
			this.textEmailCount = new OpenDental.ValidNumber();
			this.label30 = new System.Windows.Forms.Label();
			this.butUp = new OpenDental.UI.Button();
			this.butDown = new OpenDental.UI.Button();
			this.checkSendAll = new System.Windows.Forms.CheckBox();
			this.textMessageText = new System.Windows.Forms.RichTextBox();
			this.label16 = new System.Windows.Forms.Label();
			this.tabAutomationSettings = new System.Windows.Forms.TabPage();
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.radioDoNotSend = new System.Windows.Forms.RadioButton();
			this.radioSendToEmailOnlyPreferred = new System.Windows.Forms.RadioButton();
			this.radioSendToEmailNoPreferred = new System.Windows.Forms.RadioButton();
			this.radioSendToEmail = new System.Windows.Forms.RadioButton();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.label19 = new System.Windows.Forms.Label();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.dateRunEnd = new System.Windows.Forms.DateTimePicker();
			this.dateRunStart = new System.Windows.Forms.DateTimePicker();
			this.label17 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.tabControlSetup.SuspendLayout();
			this.tabRecallConfirmationSetup.SuspendLayout();
			this.tabConfirmationAutomation.SuspendLayout();
			this.tabReminderSetup.SuspendLayout();
			this.groupReminders.SuspendLayout();
			this.groupBox10.SuspendLayout();
			this.groupBox9.SuspendLayout();
			this.tabAutomationSettings.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.SuspendLayout();
			// 
			// textPostcardsPerSheet
			// 
			this.textPostcardsPerSheet.Location = new System.Drawing.Point(223, 485);
			this.textPostcardsPerSheet.Name = "textPostcardsPerSheet";
			this.textPostcardsPerSheet.Size = new System.Drawing.Size(34, 20);
			this.textPostcardsPerSheet.TabIndex = 18;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(47, 488);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(176, 16);
			this.label8.TabIndex = 19;
			this.label8.Text = "Postcards per sheet (1,3,or 4)";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkReturnAdd
			// 
			this.checkReturnAdd.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkReturnAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkReturnAdd.Location = new System.Drawing.Point(89, 506);
			this.checkReturnAdd.Name = "checkReturnAdd";
			this.checkReturnAdd.Size = new System.Drawing.Size(147, 19);
			this.checkReturnAdd.TabIndex = 43;
			this.checkReturnAdd.Text = "Show return address";
			this.checkReturnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.textDown);
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Controls.Add(this.textRight);
			this.groupBox2.Controls.Add(this.label13);
			this.groupBox2.Location = new System.Drawing.Point(716, 400);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(191, 67);
			this.groupBox2.TabIndex = 48;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Adjust Postcard Position in Inches";
			// 
			// textDown
			// 
			this.textDown.Location = new System.Drawing.Point(110, 43);
			this.textDown.MaxVal = 100000000D;
			this.textDown.MinVal = -100000000D;
			this.textDown.Name = "textDown";
			this.textDown.Size = new System.Drawing.Size(73, 20);
			this.textDown.TabIndex = 6;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(48, 42);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(60, 20);
			this.label12.TabIndex = 5;
			this.label12.Text = "Down";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textRight
			// 
			this.textRight.Location = new System.Drawing.Point(110, 18);
			this.textRight.MaxVal = 100000000D;
			this.textRight.MinVal = -100000000D;
			this.textRight.Name = "textRight";
			this.textRight.Size = new System.Drawing.Size(73, 20);
			this.textRight.TabIndex = 4;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(48, 17);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(60, 20);
			this.label13.TabIndex = 4;
			this.label13.Text = "Right";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkGroupFamilies
			// 
			this.checkGroupFamilies.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkGroupFamilies.Location = new System.Drawing.Point(85, 15);
			this.checkGroupFamilies.Name = "checkGroupFamilies";
			this.checkGroupFamilies.Size = new System.Drawing.Size(121, 18);
			this.checkGroupFamilies.TabIndex = 49;
			this.checkGroupFamilies.Text = "Group Families";
			this.checkGroupFamilies.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkGroupFamilies.UseVisualStyleBackColor = true;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(6, 32);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(184, 20);
			this.label14.TabIndex = 50;
			this.label14.Text = "Days Past (e.g. 1095, blank, etc)";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(9, 53);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(181, 20);
			this.label15.TabIndex = 52;
			this.label15.Text = "Days Future (e.g. 7)";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.textDaysFuture);
			this.groupBox3.Controls.Add(this.textDaysPast);
			this.groupBox3.Controls.Add(this.checkGroupFamilies);
			this.groupBox3.Controls.Add(this.label14);
			this.groupBox3.Controls.Add(this.label15);
			this.groupBox3.Location = new System.Drawing.Point(444, 400);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(253, 78);
			this.groupBox3.TabIndex = 54;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Recall List Default View";
			// 
			// textDaysFuture
			// 
			this.textDaysFuture.Location = new System.Drawing.Point(192, 54);
			this.textDaysFuture.MaxVal = 10000;
			this.textDaysFuture.MinVal = 0;
			this.textDaysFuture.Name = "textDaysFuture";
			this.textDaysFuture.Size = new System.Drawing.Size(53, 20);
			this.textDaysFuture.TabIndex = 66;
			// 
			// textDaysPast
			// 
			this.textDaysPast.Location = new System.Drawing.Point(192, 32);
			this.textDaysPast.MaxVal = 10000;
			this.textDaysPast.MinVal = 0;
			this.textDaysPast.Name = "textDaysPast";
			this.textDaysPast.Size = new System.Drawing.Size(53, 20);
			this.textDaysPast.TabIndex = 65;
			// 
			// label25
			// 
			this.label25.Location = new System.Drawing.Point(64, 399);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(157, 16);
			this.label25.TabIndex = 57;
			this.label25.Text = "Status for mailed recall";
			this.label25.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboStatusMailedRecall
			// 
			this.comboStatusMailedRecall.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboStatusMailedRecall.FormattingEnabled = true;
			this.comboStatusMailedRecall.Location = new System.Drawing.Point(223, 395);
			this.comboStatusMailedRecall.MaxDropDownItems = 20;
			this.comboStatusMailedRecall.Name = "comboStatusMailedRecall";
			this.comboStatusMailedRecall.Size = new System.Drawing.Size(206, 21);
			this.comboStatusMailedRecall.TabIndex = 58;
			// 
			// comboStatusEmailedRecall
			// 
			this.comboStatusEmailedRecall.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboStatusEmailedRecall.FormattingEnabled = true;
			this.comboStatusEmailedRecall.Location = new System.Drawing.Point(223, 417);
			this.comboStatusEmailedRecall.MaxDropDownItems = 20;
			this.comboStatusEmailedRecall.Name = "comboStatusEmailedRecall";
			this.comboStatusEmailedRecall.Size = new System.Drawing.Size(206, 21);
			this.comboStatusEmailedRecall.TabIndex = 60;
			// 
			// label26
			// 
			this.label26.Location = new System.Drawing.Point(64, 421);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(157, 16);
			this.label26.TabIndex = 59;
			this.label26.Text = "Status for e-mailed recall";
			this.label26.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// listTypes
			// 
			this.listTypes.FormattingEnabled = true;
			this.listTypes.Location = new System.Drawing.Point(223, 524);
			this.listTypes.Name = "listTypes";
			this.listTypes.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listTypes.Size = new System.Drawing.Size(120, 82);
			this.listTypes.TabIndex = 64;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(66, 524);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(157, 65);
			this.label1.TabIndex = 63;
			this.label1.Text = "Types to show in recall list (typically just prophy, perio, and user-added types)" +
    "";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.textMaxReminders);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.textDaysSecondReminder);
			this.groupBox1.Controls.Add(this.textDaysFirstReminder);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Location = new System.Drawing.Point(444, 518);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(253, 86);
			this.groupBox1.TabIndex = 65;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Also show in list if # of days since";
			// 
			// textMaxReminders
			// 
			this.textMaxReminders.Location = new System.Drawing.Point(192, 60);
			this.textMaxReminders.MaxVal = 10000;
			this.textMaxReminders.MinVal = 0;
			this.textMaxReminders.Name = "textMaxReminders";
			this.textMaxReminders.Size = new System.Drawing.Size(53, 20);
			this.textMaxReminders.TabIndex = 68;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(44, 59);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(146, 20);
			this.label4.TabIndex = 67;
			this.label4.Text = "Max # Reminders (e.g. 4)";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDaysSecondReminder
			// 
			this.textDaysSecondReminder.Location = new System.Drawing.Point(192, 38);
			this.textDaysSecondReminder.MaxVal = 10000;
			this.textDaysSecondReminder.MinVal = 0;
			this.textDaysSecondReminder.Name = "textDaysSecondReminder";
			this.textDaysSecondReminder.Size = new System.Drawing.Size(53, 20);
			this.textDaysSecondReminder.TabIndex = 66;
			// 
			// textDaysFirstReminder
			// 
			this.textDaysFirstReminder.Location = new System.Drawing.Point(192, 16);
			this.textDaysFirstReminder.MaxVal = 10000;
			this.textDaysFirstReminder.MinVal = 0;
			this.textDaysFirstReminder.Name = "textDaysFirstReminder";
			this.textDaysFirstReminder.Size = new System.Drawing.Size(53, 20);
			this.textDaysFirstReminder.TabIndex = 65;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(89, 15);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(101, 20);
			this.label2.TabIndex = 50;
			this.label2.Text = "Initial Reminder";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(44, 37);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(146, 20);
			this.label3.TabIndex = 52;
			this.label3.Text = "Second (or more) Reminder";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboStatusEmailedConfirm
			// 
			this.comboStatusEmailedConfirm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboStatusEmailedConfirm.FormattingEnabled = true;
			this.comboStatusEmailedConfirm.Location = new System.Drawing.Point(223, 439);
			this.comboStatusEmailedConfirm.MaxDropDownItems = 20;
			this.comboStatusEmailedConfirm.Name = "comboStatusEmailedConfirm";
			this.comboStatusEmailedConfirm.Size = new System.Drawing.Size(206, 21);
			this.comboStatusEmailedConfirm.TabIndex = 69;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(32, 443);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(189, 16);
			this.label5.TabIndex = 68;
			this.label5.Text = "Status for e-mailed confirmation";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.radioUseEmailFalse);
			this.groupBox4.Controls.Add(this.radioUseEmailTrue);
			this.groupBox4.Location = new System.Drawing.Point(716, 473);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(191, 57);
			this.groupBox4.TabIndex = 70;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Use e-mail if";
			// 
			// radioUseEmailFalse
			// 
			this.radioUseEmailFalse.Location = new System.Drawing.Point(7, 34);
			this.radioUseEmailFalse.Name = "radioUseEmailFalse";
			this.radioUseEmailFalse.Size = new System.Drawing.Size(181, 18);
			this.radioUseEmailFalse.TabIndex = 1;
			this.radioUseEmailFalse.Text = "E-mail is preferred recall method";
			this.radioUseEmailFalse.UseVisualStyleBackColor = true;
			// 
			// radioUseEmailTrue
			// 
			this.radioUseEmailTrue.Location = new System.Drawing.Point(7, 17);
			this.radioUseEmailTrue.Name = "radioUseEmailTrue";
			this.radioUseEmailTrue.Size = new System.Drawing.Size(181, 18);
			this.radioUseEmailTrue.TabIndex = 0;
			this.radioUseEmailTrue.Text = "Has e-mail address";
			this.radioUseEmailTrue.UseVisualStyleBackColor = true;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(32, 465);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(189, 16);
			this.label6.TabIndex = 68;
			this.label6.Text = "Status for text messaged confirmation";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboStatusTextMessagedConfirm
			// 
			this.comboStatusTextMessagedConfirm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboStatusTextMessagedConfirm.FormattingEnabled = true;
			this.comboStatusTextMessagedConfirm.Location = new System.Drawing.Point(223, 461);
			this.comboStatusTextMessagedConfirm.MaxDropDownItems = 20;
			this.comboStatusTextMessagedConfirm.Name = "comboStatusTextMessagedConfirm";
			this.comboStatusTextMessagedConfirm.Size = new System.Drawing.Size(206, 21);
			this.comboStatusTextMessagedConfirm.TabIndex = 69;
			// 
			// radioExcludeFutureNo
			// 
			this.radioExcludeFutureNo.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioExcludeFutureNo.Location = new System.Drawing.Point(433, 480);
			this.radioExcludeFutureNo.Name = "radioExcludeFutureNo";
			this.radioExcludeFutureNo.Size = new System.Drawing.Size(217, 18);
			this.radioExcludeFutureNo.TabIndex = 71;
			this.radioExcludeFutureNo.Text = "Exclude from list if recall scheduled";
			this.radioExcludeFutureNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioExcludeFutureNo.UseVisualStyleBackColor = true;
			// 
			// radioExcludeFutureYes
			// 
			this.radioExcludeFutureYes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioExcludeFutureYes.Location = new System.Drawing.Point(433, 498);
			this.radioExcludeFutureYes.Name = "radioExcludeFutureYes";
			this.radioExcludeFutureYes.Size = new System.Drawing.Size(217, 18);
			this.radioExcludeFutureYes.TabIndex = 72;
			this.radioExcludeFutureYes.Text = "Exclude from list if any future appt";
			this.radioExcludeFutureYes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioExcludeFutureYes.UseVisualStyleBackColor = true;
			// 
			// tabControlSetup
			// 
			this.tabControlSetup.Controls.Add(this.tabRecallConfirmationSetup);
			this.tabControlSetup.Controls.Add(this.tabConfirmationAutomation);
			this.tabControlSetup.Controls.Add(this.tabReminderSetup);
			this.tabControlSetup.Controls.Add(this.tabAutomationSettings);
			this.tabControlSetup.Location = new System.Drawing.Point(12, 12);
			this.tabControlSetup.Name = "tabControlSetup";
			this.tabControlSetup.SelectedIndex = 0;
			this.tabControlSetup.Size = new System.Drawing.Size(950, 641);
			this.tabControlSetup.TabIndex = 73;
			// 
			// tabRecallConfirmationSetup
			// 
			this.tabRecallConfirmationSetup.Controls.Add(this.gridMain);
			this.tabRecallConfirmationSetup.Controls.Add(this.radioExcludeFutureYes);
			this.tabRecallConfirmationSetup.Controls.Add(this.label8);
			this.tabRecallConfirmationSetup.Controls.Add(this.radioExcludeFutureNo);
			this.tabRecallConfirmationSetup.Controls.Add(this.textPostcardsPerSheet);
			this.tabRecallConfirmationSetup.Controls.Add(this.groupBox4);
			this.tabRecallConfirmationSetup.Controls.Add(this.checkReturnAdd);
			this.tabRecallConfirmationSetup.Controls.Add(this.comboStatusTextMessagedConfirm);
			this.tabRecallConfirmationSetup.Controls.Add(this.groupBox2);
			this.tabRecallConfirmationSetup.Controls.Add(this.label6);
			this.tabRecallConfirmationSetup.Controls.Add(this.groupBox3);
			this.tabRecallConfirmationSetup.Controls.Add(this.comboStatusEmailedConfirm);
			this.tabRecallConfirmationSetup.Controls.Add(this.label25);
			this.tabRecallConfirmationSetup.Controls.Add(this.label5);
			this.tabRecallConfirmationSetup.Controls.Add(this.comboStatusMailedRecall);
			this.tabRecallConfirmationSetup.Controls.Add(this.label26);
			this.tabRecallConfirmationSetup.Controls.Add(this.groupBox1);
			this.tabRecallConfirmationSetup.Controls.Add(this.comboStatusEmailedRecall);
			this.tabRecallConfirmationSetup.Controls.Add(this.listTypes);
			this.tabRecallConfirmationSetup.Controls.Add(this.label1);
			this.tabRecallConfirmationSetup.Location = new System.Drawing.Point(4, 22);
			this.tabRecallConfirmationSetup.Name = "tabRecallConfirmationSetup";
			this.tabRecallConfirmationSetup.Padding = new System.Windows.Forms.Padding(3);
			this.tabRecallConfirmationSetup.Size = new System.Drawing.Size(942, 615);
			this.tabRecallConfirmationSetup.TabIndex = 0;
			this.tabRecallConfirmationSetup.Text = "Recalls & Confirmations";
			this.tabRecallConfirmationSetup.UseVisualStyleBackColor = true;
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(35, 14);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(872, 374);
			this.gridMain.TabIndex = 67;
			this.gridMain.Title = "Messages";
			this.gridMain.TranslationName = "TableRecallMsgs";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// tabConfirmationAutomation
			// 
			this.tabConfirmationAutomation.Controls.Add(this.butAdd);
			this.tabConfirmationAutomation.Controls.Add(this.gridConfirmationRules);
			this.tabConfirmationAutomation.Location = new System.Drawing.Point(4, 22);
			this.tabConfirmationAutomation.Name = "tabConfirmationAutomation";
			this.tabConfirmationAutomation.Padding = new System.Windows.Forms.Padding(3);
			this.tabConfirmationAutomation.Size = new System.Drawing.Size(942, 615);
			this.tabConfirmationAutomation.TabIndex = 1;
			this.tabConfirmationAutomation.Text = "Confirmation Automation";
			this.tabConfirmationAutomation.UseVisualStyleBackColor = true;
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(35, 394);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(75, 24);
			this.butAdd.TabIndex = 69;
			this.butAdd.Text = "Add";
			this.butAdd.UseVisualStyleBackColor = true;
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// gridConfirmationRules
			// 
			this.gridConfirmationRules.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridConfirmationRules.HasAddButton = false;
			this.gridConfirmationRules.HasMultilineHeaders = false;
			this.gridConfirmationRules.HScrollVisible = false;
			this.gridConfirmationRules.Location = new System.Drawing.Point(35, 14);
			this.gridConfirmationRules.Name = "gridConfirmationRules";
			this.gridConfirmationRules.ScrollValue = 0;
			this.gridConfirmationRules.Size = new System.Drawing.Size(872, 374);
			this.gridConfirmationRules.TabIndex = 68;
			this.gridConfirmationRules.Title = "Confirmation Automation Rules";
			this.gridConfirmationRules.TranslationName = "TableConfirmationRules";
			this.gridConfirmationRules.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridConfirmationRules_CellDoubleClick);
			// 
			// tabReminderSetup
			// 
			this.tabReminderSetup.Controls.Add(this.groupBox11);
			this.tabReminderSetup.Controls.Add(this.groupReminders);
			this.tabReminderSetup.Controls.Add(this.label16);
			this.tabReminderSetup.Location = new System.Drawing.Point(4, 22);
			this.tabReminderSetup.Name = "tabReminderSetup";
			this.tabReminderSetup.Padding = new System.Windows.Forms.Padding(3);
			this.tabReminderSetup.Size = new System.Drawing.Size(942, 615);
			this.tabReminderSetup.TabIndex = 2;
			this.tabReminderSetup.Text = "Appt Reminder";
			this.tabReminderSetup.UseVisualStyleBackColor = true;
			// 
			// groupBox11
			// 
			this.groupBox11.Location = new System.Drawing.Point(478, 48);
			this.groupBox11.Name = "groupBox11";
			this.groupBox11.Size = new System.Drawing.Size(440, 561);
			this.groupBox11.TabIndex = 94;
			this.groupBox11.TabStop = false;
			this.groupBox11.Text = "Confirmations";
			this.groupBox11.Visible = false;
			// 
			// groupReminders
			// 
			this.groupReminders.Controls.Add(this.label38);
			this.groupReminders.Controls.Add(this.textMessageEmail);
			this.groupReminders.Controls.Add(this.label36);
			this.groupReminders.Controls.Add(this.labelTextMessage);
			this.groupReminders.Controls.Add(this.groupBox10);
			this.groupReminders.Controls.Add(this.gridPriorities);
			this.groupReminders.Controls.Add(this.groupBox9);
			this.groupReminders.Controls.Add(this.butUp);
			this.groupReminders.Controls.Add(this.butDown);
			this.groupReminders.Controls.Add(this.checkSendAll);
			this.groupReminders.Controls.Add(this.textMessageText);
			this.groupReminders.Location = new System.Drawing.Point(16, 48);
			this.groupReminders.Name = "groupReminders";
			this.groupReminders.Size = new System.Drawing.Size(440, 561);
			this.groupReminders.TabIndex = 91;
			this.groupReminders.TabStop = false;
			this.groupReminders.Text = "Reminders";
			// 
			// label38
			// 
			this.label38.Location = new System.Drawing.Point(49, 105);
			this.label38.Name = "label38";
			this.label38.Size = new System.Drawing.Size(204, 20);
			this.label38.TabIndex = 94;
			this.label38.Text = "Email Message";
			this.label38.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textMessageEmail
			// 
			this.textMessageEmail.Location = new System.Drawing.Point(52, 128);
			this.textMessageEmail.Name = "textMessageEmail";
			this.textMessageEmail.Size = new System.Drawing.Size(337, 82);
			this.textMessageEmail.TabIndex = 95;
			this.textMessageEmail.Text = "Appointment Reminder: [nameF] is scheduled for [apptTime] on [apptDate] at [clini" +
    "cName]. [clinicPhone]";
			// 
			// label36
			// 
			this.label36.Location = new System.Drawing.Point(88, 315);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(301, 29);
			this.label36.TabIndex = 93;
			this.label36.Text = "Prioritize contact methods, \"Send All\" to send email AND text.";
			// 
			// labelTextMessage
			// 
			this.labelTextMessage.Location = new System.Drawing.Point(49, 12);
			this.labelTextMessage.Name = "labelTextMessage";
			this.labelTextMessage.Size = new System.Drawing.Size(204, 23);
			this.labelTextMessage.TabIndex = 21;
			this.labelTextMessage.Text = "Text Message";
			this.labelTextMessage.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// groupBox10
			// 
			this.groupBox10.Controls.Add(this.label7);
			this.groupBox10.Controls.Add(this.label11);
			this.groupBox10.Controls.Add(this.textHoursPrior);
			this.groupBox10.Controls.Add(this.textDaysPrior);
			this.groupBox10.Controls.Add(this.label20);
			this.groupBox10.Controls.Add(this.label10);
			this.groupBox10.Location = new System.Drawing.Point(52, 347);
			this.groupBox10.Name = "groupBox10";
			this.groupBox10.Size = new System.Drawing.Size(337, 83);
			this.groupBox10.TabIndex = 90;
			this.groupBox10.TabStop = false;
			this.groupBox10.Text = "Interval";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(132, 46);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(199, 27);
			this.label7.TabIndex = 73;
			this.label7.Text = "Send a reminder days before an appointment. (0 to disable)";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(6, 48);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(71, 23);
			this.label11.TabIndex = 14;
			this.label11.Text = "Days Prior";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textHoursPrior
			// 
			this.textHoursPrior.Location = new System.Drawing.Point(77, 18);
			this.textHoursPrior.MaxVal = 100000000D;
			this.textHoursPrior.MinVal = 0D;
			this.textHoursPrior.Name = "textHoursPrior";
			this.textHoursPrior.Size = new System.Drawing.Size(51, 20);
			this.textHoursPrior.TabIndex = 13;
			this.textHoursPrior.Text = "0";
			this.textHoursPrior.Leave += new System.EventHandler(this.textHourInterval_Leave);
			// 
			// textDaysPrior
			// 
			this.textDaysPrior.Location = new System.Drawing.Point(77, 50);
			this.textDaysPrior.MaxVal = 100000000D;
			this.textDaysPrior.MinVal = 0D;
			this.textDaysPrior.Name = "textDaysPrior";
			this.textDaysPrior.Size = new System.Drawing.Size(51, 20);
			this.textDaysPrior.TabIndex = 12;
			this.textDaysPrior.Text = "0";
			this.textDaysPrior.Leave += new System.EventHandler(this.textDayInterval_Leave);
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(132, 14);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(199, 27);
			this.label20.TabIndex = 72;
			this.label20.Text = "Send a reminder hours before an appointment. (0 to disable)";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(6, 16);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(71, 23);
			this.label10.TabIndex = 15;
			this.label10.Text = "Hours Prior";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// gridPriorities
			// 
			this.gridPriorities.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridPriorities.HasAddButton = false;
			this.gridPriorities.HasMultilineHeaders = false;
			this.gridPriorities.HScrollVisible = false;
			this.gridPriorities.Location = new System.Drawing.Point(88, 216);
			this.gridPriorities.Name = "gridPriorities";
			this.gridPriorities.ScrollValue = 0;
			this.gridPriorities.Size = new System.Drawing.Size(301, 96);
			this.gridPriorities.TabIndex = 68;
			this.gridPriorities.Title = "Reminder Method Order";
			this.gridPriorities.TranslationName = null;
			// 
			// groupBox9
			// 
			this.groupBox9.Controls.Add(this.label23);
			this.groupBox9.Controls.Add(this.textApptCount);
			this.groupBox9.Controls.Add(this.textTextCount);
			this.groupBox9.Controls.Add(this.label31);
			this.groupBox9.Controls.Add(this.label29);
			this.groupBox9.Controls.Add(this.textNeitherCount);
			this.groupBox9.Controls.Add(this.textEmailCount);
			this.groupBox9.Controls.Add(this.label30);
			this.groupBox9.Location = new System.Drawing.Point(52, 432);
			this.groupBox9.Name = "groupBox9";
			this.groupBox9.Size = new System.Drawing.Size(337, 125);
			this.groupBox9.TabIndex = 89;
			this.groupBox9.TabStop = false;
			this.groupBox9.Text = "Reminder Forecast (next 7 days)";
			// 
			// label23
			// 
			this.label23.Location = new System.Drawing.Point(23, 16);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(226, 20);
			this.label23.TabIndex = 75;
			this.label23.Text = "Texts to be sent:";
			this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textApptCount
			// 
			this.textApptCount.Location = new System.Drawing.Point(255, 94);
			this.textApptCount.MaxVal = 255;
			this.textApptCount.MinVal = 0;
			this.textApptCount.Name = "textApptCount";
			this.textApptCount.ReadOnly = true;
			this.textApptCount.Size = new System.Drawing.Size(51, 20);
			this.textApptCount.TabIndex = 88;
			this.textApptCount.Text = "0";
			// 
			// textTextCount
			// 
			this.textTextCount.Location = new System.Drawing.Point(255, 16);
			this.textTextCount.MaxVal = 255;
			this.textTextCount.MinVal = 0;
			this.textTextCount.Name = "textTextCount";
			this.textTextCount.ReadOnly = true;
			this.textTextCount.Size = new System.Drawing.Size(51, 20);
			this.textTextCount.TabIndex = 76;
			this.textTextCount.Text = "0";
			// 
			// label31
			// 
			this.label31.Location = new System.Drawing.Point(23, 94);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(226, 20);
			this.label31.TabIndex = 87;
			this.label31.Text = "Total potential reminders for the next 7 days:";
			this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label29
			// 
			this.label29.Location = new System.Drawing.Point(23, 42);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(226, 20);
			this.label29.TabIndex = 83;
			this.label29.Text = "Emails to be sent:";
			this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textNeitherCount
			// 
			this.textNeitherCount.Location = new System.Drawing.Point(255, 68);
			this.textNeitherCount.MaxVal = 255;
			this.textNeitherCount.MinVal = 0;
			this.textNeitherCount.Name = "textNeitherCount";
			this.textNeitherCount.ReadOnly = true;
			this.textNeitherCount.Size = new System.Drawing.Size(51, 20);
			this.textNeitherCount.TabIndex = 86;
			this.textNeitherCount.Text = "0";
			// 
			// textEmailCount
			// 
			this.textEmailCount.Location = new System.Drawing.Point(255, 42);
			this.textEmailCount.MaxVal = 255;
			this.textEmailCount.MinVal = 0;
			this.textEmailCount.Name = "textEmailCount";
			this.textEmailCount.ReadOnly = true;
			this.textEmailCount.Size = new System.Drawing.Size(51, 20);
			this.textEmailCount.TabIndex = 84;
			this.textEmailCount.Text = "0";
			// 
			// label30
			// 
			this.label30.Location = new System.Drawing.Point(23, 68);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(226, 20);
			this.label30.TabIndex = 85;
			this.label30.Text = "No send method (can\'t be sent):";
			this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUp.Autosize = false;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.Location = new System.Drawing.Point(52, 216);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(30, 30);
			this.butUp.TabIndex = 17;
			this.butUp.UseVisualStyleBackColor = true;
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDown.Autosize = false;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.Location = new System.Drawing.Point(52, 252);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(30, 30);
			this.butDown.TabIndex = 18;
			this.butDown.UseVisualStyleBackColor = true;
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// checkSendAll
			// 
			this.checkSendAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSendAll.Location = new System.Drawing.Point(14, 288);
			this.checkSendAll.Name = "checkSendAll";
			this.checkSendAll.Size = new System.Drawing.Size(68, 24);
			this.checkSendAll.TabIndex = 23;
			this.checkSendAll.Text = "Send All";
			this.checkSendAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSendAll.UseVisualStyleBackColor = true;
			this.checkSendAll.CheckedChanged += new System.EventHandler(this.checkSendAll_CheckedChanged);
			// 
			// textMessageText
			// 
			this.textMessageText.Location = new System.Drawing.Point(52, 38);
			this.textMessageText.Name = "textMessageText";
			this.textMessageText.Size = new System.Drawing.Size(337, 64);
			this.textMessageText.TabIndex = 69;
			this.textMessageText.Text = "Appointment Reminder: [nameF] is scheduled for [apptTime] on [apptDate] at [clini" +
    "cName]. [clinicPhone]";
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(3, 6);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(933, 39);
			this.label16.TabIndex = 71;
			this.label16.Text = resources.GetString("label16.Text");
			// 
			// tabAutomationSettings
			// 
			this.tabAutomationSettings.Controls.Add(this.groupBox8);
			this.tabAutomationSettings.Controls.Add(this.groupBox7);
			this.tabAutomationSettings.Location = new System.Drawing.Point(4, 22);
			this.tabAutomationSettings.Name = "tabAutomationSettings";
			this.tabAutomationSettings.Padding = new System.Windows.Forms.Padding(3);
			this.tabAutomationSettings.Size = new System.Drawing.Size(942, 615);
			this.tabAutomationSettings.TabIndex = 3;
			this.tabAutomationSettings.Text = "Automation Settings";
			this.tabAutomationSettings.UseVisualStyleBackColor = true;
			// 
			// groupBox8
			// 
			this.groupBox8.Controls.Add(this.groupBox5);
			this.groupBox8.Location = new System.Drawing.Point(185, 169);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new System.Drawing.Size(572, 120);
			this.groupBox8.TabIndex = 76;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "Web Sched";
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.radioDoNotSend);
			this.groupBox5.Controls.Add(this.radioSendToEmailOnlyPreferred);
			this.groupBox5.Controls.Add(this.radioSendToEmailNoPreferred);
			this.groupBox5.Controls.Add(this.radioSendToEmail);
			this.groupBox5.Location = new System.Drawing.Point(61, 19);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(451, 95);
			this.groupBox5.TabIndex = 73;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Send Automatically To";
			// 
			// radioDoNotSend
			// 
			this.radioDoNotSend.Location = new System.Drawing.Point(7, 16);
			this.radioDoNotSend.Name = "radioDoNotSend";
			this.radioDoNotSend.Size = new System.Drawing.Size(438, 18);
			this.radioDoNotSend.TabIndex = 77;
			this.radioDoNotSend.Text = "Do Not Send";
			this.radioDoNotSend.UseVisualStyleBackColor = true;
			// 
			// radioSendToEmailOnlyPreferred
			// 
			this.radioSendToEmailOnlyPreferred.Location = new System.Drawing.Point(7, 69);
			this.radioSendToEmailOnlyPreferred.Name = "radioSendToEmailOnlyPreferred";
			this.radioSendToEmailOnlyPreferred.Size = new System.Drawing.Size(438, 18);
			this.radioSendToEmailOnlyPreferred.TabIndex = 74;
			this.radioSendToEmailOnlyPreferred.Text = "Patients with email address and email is selected as their preferred recall metho" +
    "d.";
			this.radioSendToEmailOnlyPreferred.UseVisualStyleBackColor = true;
			// 
			// radioSendToEmailNoPreferred
			// 
			this.radioSendToEmailNoPreferred.Location = new System.Drawing.Point(7, 51);
			this.radioSendToEmailNoPreferred.Name = "radioSendToEmailNoPreferred";
			this.radioSendToEmailNoPreferred.Size = new System.Drawing.Size(438, 18);
			this.radioSendToEmailNoPreferred.TabIndex = 1;
			this.radioSendToEmailNoPreferred.Text = "Patients with email address and no other preferred recall method is selected.";
			this.radioSendToEmailNoPreferred.UseVisualStyleBackColor = true;
			// 
			// radioSendToEmail
			// 
			this.radioSendToEmail.Location = new System.Drawing.Point(7, 34);
			this.radioSendToEmail.Name = "radioSendToEmail";
			this.radioSendToEmail.Size = new System.Drawing.Size(438, 18);
			this.radioSendToEmail.TabIndex = 0;
			this.radioSendToEmail.Text = "Patients with email addresses";
			this.radioSendToEmail.UseVisualStyleBackColor = true;
			// 
			// groupBox7
			// 
			this.groupBox7.Controls.Add(this.label19);
			this.groupBox7.Controls.Add(this.groupBox6);
			this.groupBox7.Location = new System.Drawing.Point(185, 6);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(572, 157);
			this.groupBox7.TabIndex = 75;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "General";
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(6, 16);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(560, 41);
			this.label19.TabIndex = 72;
			this.label19.Text = resources.GetString("label19.Text");
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.dateRunEnd);
			this.groupBox6.Controls.Add(this.dateRunStart);
			this.groupBox6.Controls.Add(this.label17);
			this.groupBox6.Controls.Add(this.label18);
			this.groupBox6.Location = new System.Drawing.Point(193, 60);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(186, 68);
			this.groupBox6.TabIndex = 74;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Run Times";
			// 
			// dateRunEnd
			// 
			this.dateRunEnd.CustomFormat = " ";
			this.dateRunEnd.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.dateRunEnd.Location = new System.Drawing.Point(78, 36);
			this.dateRunEnd.Name = "dateRunEnd";
			this.dateRunEnd.ShowUpDown = true;
			this.dateRunEnd.Size = new System.Drawing.Size(90, 20);
			this.dateRunEnd.TabIndex = 7;
			this.dateRunEnd.Value = new System.DateTime(2015, 11, 3, 22, 0, 0, 0);
			// 
			// dateRunStart
			// 
			this.dateRunStart.CustomFormat = " ";
			this.dateRunStart.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.dateRunStart.Location = new System.Drawing.Point(78, 16);
			this.dateRunStart.Name = "dateRunStart";
			this.dateRunStart.ShowUpDown = true;
			this.dateRunStart.Size = new System.Drawing.Size(90, 20);
			this.dateRunStart.TabIndex = 6;
			this.dateRunStart.Value = new System.DateTime(2015, 11, 3, 7, 0, 0, 0);
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(46, 38);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(32, 15);
			this.label17.TabIndex = 5;
			this.label17.Text = "End";
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(45, 18);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(32, 15);
			this.label18.TabIndex = 4;
			this.label18.Text = "Start";
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(806, 659);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
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
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(887, 659);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 4;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormRecallSetup
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(974, 695);
			this.Controls.Add(this.tabControlSetup);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(990, 734);
			this.MinimizeBox = false;
			this.Name = "FormRecallSetup";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Setup Recall and Confirmation";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormRecallSetup_FormClosing);
			this.Load += new System.EventHandler(this.FormRecallSetup_Load);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.tabControlSetup.ResumeLayout(false);
			this.tabRecallConfirmationSetup.ResumeLayout(false);
			this.tabRecallConfirmationSetup.PerformLayout();
			this.tabConfirmationAutomation.ResumeLayout(false);
			this.tabReminderSetup.ResumeLayout(false);
			this.groupReminders.ResumeLayout(false);
			this.groupBox10.ResumeLayout(false);
			this.groupBox10.PerformLayout();
			this.groupBox9.ResumeLayout(false);
			this.groupBox9.PerformLayout();
			this.tabAutomationSettings.ResumeLayout(false);
			this.groupBox8.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.groupBox7.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormRecallSetup_Load(object sender, System.EventArgs e) {
			FillRecallAndConfirmationTab();
			//TODO: remove DEBUG region when ready to release to the public.
#if DEBUG
			FillConfirmationAutomationTab();
#else
			tabControlSetup.TabPages.Remove(tabConfirmationAutomation);
#endif
			FillPriorityTab();//Automated appointment reminders and confirmations
			FillAutomationTab();
		}

		#region Recalls & Confirmations

		///<summary>Called on load to initially load the recall and confirmation tab with values from the database.  Calls FillGrid at the end.</summary>
		private void FillRecallAndConfirmationTab() {
			checkGroupFamilies.Checked = PrefC.GetBool(PrefName.RecallGroupByFamily);
			textPostcardsPerSheet.Text=PrefC.GetLong(PrefName.RecallPostcardsPerSheet).ToString();
			checkReturnAdd.Checked=PrefC.GetBool(PrefName.RecallCardsShowReturnAdd);
			checkGroupFamilies.Checked=PrefC.GetBool(PrefName.RecallGroupByFamily);
			if(PrefC.GetLong(PrefName.RecallDaysPast)==-1) {
				textDaysPast.Text="";
			}
			else {
				textDaysPast.Text=PrefC.GetLong(PrefName.RecallDaysPast).ToString();
			}
			if(PrefC.GetLong(PrefName.RecallDaysFuture)==-1) {
				textDaysFuture.Text="";
			}
			else {
				textDaysFuture.Text=PrefC.GetLong(PrefName.RecallDaysFuture).ToString();
			}
			if(PrefC.GetBool(PrefName.RecallExcludeIfAnyFutureAppt)) {
				radioExcludeFutureYes.Checked=true;
			}
			else {
				radioExcludeFutureNo.Checked=true;
			}
			textRight.Text=PrefC.GetDouble(PrefName.RecallAdjustRight).ToString();
			textDown.Text=PrefC.GetDouble(PrefName.RecallAdjustDown).ToString();
			//comboStatusMailedRecall.Items.Clear();
			for(int i=0;i<DefC.Short[(int)DefCat.RecallUnschedStatus].Length;i++) {
				comboStatusMailedRecall.Items.Add(DefC.Short[(int)DefCat.RecallUnschedStatus][i].ItemName);
				comboStatusEmailedRecall.Items.Add(DefC.Short[(int)DefCat.RecallUnschedStatus][i].ItemName);
				if(DefC.Short[(int)DefCat.RecallUnschedStatus][i].DefNum==PrefC.GetLong(PrefName.RecallStatusMailed)) {
					comboStatusMailedRecall.SelectedIndex=i;
				}
				if(DefC.Short[(int)DefCat.RecallUnschedStatus][i].DefNum==PrefC.GetLong(PrefName.RecallStatusEmailed)) {
					comboStatusEmailedRecall.SelectedIndex=i;
				}
			}
			for(int i=0;i<DefC.Short[(int)DefCat.ApptConfirmed].Length;i++) {
				comboStatusEmailedConfirm.Items.Add(DefC.Short[(int)DefCat.ApptConfirmed][i].ItemName);
				if(DefC.Short[(int)DefCat.ApptConfirmed][i].DefNum==PrefC.GetLong(PrefName.ConfirmStatusEmailed)) {
					comboStatusEmailedConfirm.SelectedIndex=i;
				}
			}
			for(int i=0;i<DefC.Short[(int)DefCat.ApptConfirmed].Length;i++) {
				comboStatusTextMessagedConfirm.Items.Add(DefC.Short[(int)DefCat.ApptConfirmed][i].ItemName);
				if(DefC.Short[(int)DefCat.ApptConfirmed][i].DefNum==PrefC.GetLong(PrefName.ConfirmStatusTextMessaged)) {
					comboStatusTextMessagedConfirm.SelectedIndex=i;
				}
			}
			List<long> recalltypes=new List<long>();
			string[] typearray=PrefC.GetString(PrefName.RecallTypesShowingInList).Split(',');
			if(typearray.Length>0) {
				for(int i=0;i<typearray.Length;i++) {
					recalltypes.Add(PIn.Long(typearray[i]));
				}
			}
			for(int i=0;i<RecallTypeC.Listt.Count;i++) {
				listTypes.Items.Add(RecallTypeC.Listt[i].Description);
				if(recalltypes.Contains(RecallTypeC.Listt[i].RecallTypeNum)) {
					listTypes.SetSelected(i,true);
				}
			}
			if(PrefC.GetLong(PrefName.RecallShowIfDaysFirstReminder)==-1) {
				textDaysFirstReminder.Text="";
			}
			else {
				textDaysFirstReminder.Text=PrefC.GetLong(PrefName.RecallShowIfDaysFirstReminder).ToString();
			}
			if(PrefC.GetLong(PrefName.RecallShowIfDaysSecondReminder)==-1) {
				textDaysSecondReminder.Text="";
			}
			else {
				textDaysSecondReminder.Text=PrefC.GetLong(PrefName.RecallShowIfDaysSecondReminder).ToString();
			}
			if(PrefC.GetLong(PrefName.RecallMaxNumberReminders)==-1) {
				textMaxReminders.Text="";
			}
			else {
				textMaxReminders.Text=PrefC.GetLong(PrefName.RecallMaxNumberReminders).ToString();
			}
			if(PrefC.GetBool(PrefName.RecallUseEmailIfHasEmailAddress)) {
				radioUseEmailTrue.Checked=true;
			}
			else {
				radioUseEmailFalse.Checked=true;
			}
			FillGrid();
		}

		private void FillGrid(){
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g("TableRecallMsgs","Remind#"),50);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRecallMsgs","Mode"),61);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("",300);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRecallMsgs","Message"),500);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			#region 1st Reminder
			//
			row=new ODGridRow();
			row.Cells.Add("1");
			row.Cells.Add(Lan.g(this,"E-mail"));
			row.Cells.Add(Lan.g(this,"Subject line"));
			row.Cells.Add(PrefC.GetString(PrefName.RecallEmailSubject));//old
			row.Tag="RecallEmailSubject";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("1");
			row.Cells.Add(Lan.g(this,"E-mail"));
			row.Cells.Add(Lan.g(this,"Available variables: [DueDate], [NameFL], [NameF]."));
			row.Cells.Add(PrefC.GetString(PrefName.RecallEmailMessage));
			row.Tag="RecallEmailMessage";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("1");
			row.Cells.Add(Lan.g(this,"E-mail"));
			row.Cells.Add(Lan.g(this,"For multiple patients in one family.  Use [FamilyList] where the list of family members should show."));
			row.Cells.Add(PrefC.GetString(PrefName.RecallEmailFamMsg));
			row.Tag="RecallEmailFamMsg";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("1");
			row.Cells.Add(Lan.g(this,"Postcard"));
			row.Cells.Add(Lan.g(this,"Available variables: [DueDate], [NameFL], [NameF]."));
			row.Cells.Add(PrefC.GetString(PrefName.RecallPostcardMessage));//old
			row.Tag="RecallPostcardMessage";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("1");
			row.Cells.Add(Lan.g(this,"Postcard"));
			row.Cells.Add(Lan.g(this,"For multiple patients in one family.  Use [FamilyList] where the list of family members should show."));
			row.Cells.Add(PrefC.GetString(PrefName.RecallPostcardFamMsg));//old
			row.Tag="RecallPostcardFamMsg";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("1");
			row.Cells.Add(Lan.g(this,"WebSched"));
			row.Cells.Add(Lan.g(this,"Subject line.  Available variables")+": [NameF]");
			row.Cells.Add(PrefC.GetString(PrefName.WebSchedSubject));
			row.Tag="WebSchedSubject";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("1");
			row.Cells.Add(Lan.g(this,"WebSched"));
			row.Cells.Add(Lan.g(this,"Available variables")+": [NameF], [DueDate], [OfficePhone], [URL]");
			row.Cells.Add(PrefC.GetString(PrefName.WebSchedMessage));
			row.Tag="WebSchedMessage";
			gridMain.Rows.Add(row);
			#endregion
			#region 2nd Reminder
			//2---------------------------------------------------------------------------------------------
			//
			row=new ODGridRow();
			row.Cells.Add("2");
			row.Cells.Add(Lan.g(this,"E-mail"));
			row.Cells.Add(Lan.g(this,"Subject line"));
			row.Cells.Add(PrefC.GetString(PrefName.RecallEmailSubject2));
			row.Tag="RecallEmailSubject2";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("2");
			row.Cells.Add(Lan.g(this,"E-mail"));
			row.Cells.Add(Lan.g(this,"Available variables: [DueDate], [NameFL], [NameF]."));
			row.Cells.Add(PrefC.GetString(PrefName.RecallEmailMessage2));
			row.Tag="RecallEmailMessage2";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("2");
			row.Cells.Add(Lan.g(this,"E-mail"));
			row.Cells.Add(Lan.g(this,"For multiple patients in one family.  Use [FamilyList]."));
			row.Cells.Add(PrefC.GetString(PrefName.RecallEmailFamMsg2));
			row.Tag="RecallEmailFamMsg2";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("2");
			row.Cells.Add(Lan.g(this,"Postcard"));
			row.Cells.Add(Lan.g(this,"Available variables: [DueDate], [NameFL], [NameF]."));
			row.Cells.Add(PrefC.GetString(PrefName.RecallPostcardMessage2));
			row.Tag="RecallPostcardMessage2";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("2");
			row.Cells.Add(Lan.g(this,"Postcard"));
			row.Cells.Add(Lan.g(this,"For multiple patients in one family.  Use [FamilyList]."));
			row.Cells.Add(PrefC.GetString(PrefName.RecallPostcardFamMsg2));
			row.Tag="RecallPostcardFamMsg2";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("2");
			row.Cells.Add(Lan.g(this,"WebSched"));
			row.Cells.Add(Lan.g(this,"Subject line.  Available variables")+": [NameF]");
			row.Cells.Add(PrefC.GetString(PrefName.WebSchedSubject2));
			row.Tag="WebSchedSubject2";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("2");
			row.Cells.Add(Lan.g(this,"WebSched"));
			row.Cells.Add(Lan.g(this,"Available variables")+": [NameF], [DueDate], [OfficePhone], [URL]");
			row.Cells.Add(PrefC.GetString(PrefName.WebSchedMessage2));
			row.Tag="WebSchedMessage2";
			gridMain.Rows.Add(row);
			#endregion
			#region 3rd Reminder
			//3---------------------------------------------------------------------------------------------
			//
			row=new ODGridRow();
			row.Cells.Add("3");
			row.Cells.Add(Lan.g(this,"E-mail"));
			row.Cells.Add(Lan.g(this,"Subject line"));
			row.Cells.Add(PrefC.GetString(PrefName.RecallEmailSubject3));
			row.Tag="RecallEmailSubject3";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("3");
			row.Cells.Add(Lan.g(this,"E-mail"));
			row.Cells.Add(Lan.g(this,"Available variables: [DueDate], [NameFL], [NameF]."));
			row.Cells.Add(PrefC.GetString(PrefName.RecallEmailMessage3));
			row.Tag="RecallEmailMessage3";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("3");
			row.Cells.Add(Lan.g(this,"E-mail"));
			row.Cells.Add(Lan.g(this,"For multiple patients in one family.  Use [FamilyList]."));
			row.Cells.Add(PrefC.GetString(PrefName.RecallEmailFamMsg3));
			row.Tag="RecallEmailFamMsg3";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("3");
			row.Cells.Add(Lan.g(this,"Postcard"));
			row.Cells.Add(Lan.g(this,"Available variables: [DueDate], [NameFL], [NameF]."));
			row.Cells.Add(PrefC.GetString(PrefName.RecallPostcardMessage3));
			row.Tag="RecallPostcardMessage3";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("3");
			row.Cells.Add(Lan.g(this,"Postcard"));
			row.Cells.Add(Lan.g(this,"For multiple patients in one family.  Use [FamilyList]."));
			row.Cells.Add(PrefC.GetString(PrefName.RecallPostcardFamMsg3));
			row.Tag="RecallPostcardFamMsg3";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("3");
			row.Cells.Add(Lan.g(this,"WebSched"));
			row.Cells.Add(Lan.g(this,"Subject line.  Available variables")+": [NameF]");
			row.Cells.Add(PrefC.GetString(PrefName.WebSchedSubject3));
			row.Tag="WebSchedSubject3";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("3");
			row.Cells.Add(Lan.g(this,"WebSched"));
			row.Cells.Add(Lan.g(this,"Available variables")+": [NameF], [DueDate], [OfficePhone], [URL]");
			row.Cells.Add(PrefC.GetString(PrefName.WebSchedMessage3));
			row.Tag="WebSchedMessage3";
			gridMain.Rows.Add(row);
			#endregion
			#region Confirmation
			//Confirmation---------------------------------------------------------------------------------------------
			row=new ODGridRow();
			row.Cells.Add("");
			row.Cells.Add(Lan.g(this,"Postcard"));
			row.Cells.Add(Lan.g(this,"Confirmation message.  Use [date]  and [time] where you want those values to be inserted"));
			row.Cells.Add(PrefC.GetString(PrefName.ConfirmPostcardMessage));
			row.Tag="ConfirmPostcardMessage";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("");
			row.Cells.Add(Lan.g(this,"E-mail"));
			row.Cells.Add(Lan.g(this,"Confirmation subject line."));
			row.Cells.Add(PrefC.GetString(PrefName.ConfirmEmailSubject));
			row.Tag="ConfirmEmailSubject";
			gridMain.Rows.Add(row);
			//
			row=new ODGridRow();
			row.Cells.Add("");
			row.Cells.Add(Lan.g(this,"E-mail"));
			row.Cells.Add(Lan.g(this,"Confirmation message.  Available variables: [NameF], [NameFL], [date], and [time]."));
			row.Cells.Add(PrefC.GetString(PrefName.ConfirmEmailMessage));
			row.Tag="ConfirmEmailMessage";
			gridMain.Rows.Add(row);
			#endregion
			#region Text Messaging
			//Text Messaging----------------------------------------------------------------------------------------------
			row=new ODGridRow();
			row.Cells.Add("");
			row.Cells.Add(Lan.g(this,"Text"));
			row.Cells.Add(Lan.g(this,"Confirmation message.  Available variables: [NameF], [NameFL], [date], and [time]."));
			row.Cells.Add(PrefC.GetString(PrefName.ConfirmTextMessage));
			row.Tag="ConfirmTextMessage";
			gridMain.Rows.Add(row);
			#endregion
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			PrefName prefName=(PrefName)Enum.Parse(typeof(PrefName),gridMain.Rows[e.Row].Tag.ToString());
			FormRecallMessageEdit FormR=new FormRecallMessageEdit(prefName);
			FormR.MessageVal=PrefC.GetString(prefName);
			FormR.ShowDialog();
			if(FormR.DialogResult!=DialogResult.OK) {
				return;
			}
			Prefs.UpdateString(prefName,FormR.MessageVal);
			//Prefs.RefreshCache();//above line handles it.
			FillGrid();
			_changed=true;
		}

		#endregion

		#region Confirmation Automation

		///<summary>Called on load to initially load the Confirmation Automation tab with values from the database.
		///Calls FillGridConfirmationAutomation at the end.</summary>
		private void FillConfirmationAutomationTab() {
			//TODO: prep any text boxes and controls here.
			FillGridConfirmationAutomation();
		}

		private void FillGridConfirmationAutomation() {
			gridConfirmationRules.BeginUpdate();
			gridConfirmationRules.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g("TableConfirmationRules","Remind#"),50);
			gridConfirmationRules.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableConfirmationRules","Mode"),61);
			gridConfirmationRules.Columns.Add(col);
			col=new ODGridColumn("",300);
			gridConfirmationRules.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableConfirmationRules","Message"),500);
			gridConfirmationRules.Columns.Add(col);
			gridConfirmationRules.Rows.Clear();
			ODGridRow row;
			//TODO: fill correctly with the values from the db
			row=new ODGridRow();
			row.Cells.Add("1");
			row.Cells.Add(Lan.g(this,"E-mail"));
			row.Cells.Add(Lan.g(this,"Subject line"));
			row.Cells.Add(PrefC.GetString(PrefName.RecallEmailSubject));//old
			row.Tag="RecallEmailSubject";
			gridConfirmationRules.Rows.Add(row);
			gridConfirmationRules.EndUpdate();
		}

		private void gridConfirmationRules_CellDoubleClick(object sender,ODGridClickEventArgs e) {

		}

		private void butAdd_Click(object sender,EventArgs e) {

		}

		#endregion

		#region Appt Reminder Setup

		private void FillPriorityTab() {
			//if(SmsPhones.IsIntegratedTextingEnabled() && SmsPhones.IsTextingForCountry("US")) {
			//	labelTextMessage.Text+=" (Not editable for US)";
			//	textMessageText.ReadOnly=true;
			//	textMessageText.Text=ApptComms.ApptReminderMsgUS;
			//}
			//else {
			textMessageText.Text=PrefC.GetString(PrefName.ApptReminderDayMessage);
			//}
			textMessageEmail.Text=PrefC.GetString(PrefName.ApptReminderEmailMessage);
			_arrayPriorities=PrefC.GetString(PrefName.ApptReminderSendOrder).Split(',');
			FillGridPriority();
			_daysPrior=PrefC.GetDouble(PrefName.ApptReminderDayInterval);
			textDaysPrior.Text=_daysPrior.ToString();
			_hoursPrior=PrefC.GetDouble(PrefName.ApptReminderHourInterval);
			textHoursPrior.Text=_hoursPrior.ToString();
			checkSendAll.Checked=PrefC.GetBool(PrefName.ApptReminderSendAll);
			FillPriorityMetrics();
		}

		private void FillPriorityMetrics() {
			double daysPrior;
			double hoursPrior;
			if(!double.TryParse(textDaysPrior.Text,out daysPrior) || !double.TryParse(textHoursPrior.Text,out hoursPrior)) {
				return;//ValidDouble displays the error message.  This is to prevent this code executing prior to ValidDouble performing validation.
			}
			DateTime dateDayStart=MiscData.GetNowDateTime().AddDays(daysPrior);
			DateTime dateHourStart= MiscData.GetNowDateTime().AddHours(hoursPrior);
			DateTime dateDayStop=dateDayStart.AddDays(7);
			DateTime dateHourStop=dateHourStart.AddDays(7);
			List<Appointment> listApptsForDays=new List<Appointment>();
			List<Appointment> listApptsForHours=new List<Appointment>();
			if(daysPrior!=0) {
				listApptsForDays=Appointments.GetSchedApptsForPeriod(dateDayStart,dateDayStop);//These run queries, sadly.  At least they're simple...
			}
			if(hoursPrior!=0) {
				listApptsForHours=Appointments.GetSchedApptsForPeriod(dateHourStart,dateHourStop);
			}
			textApptCount.Text=(listApptsForDays.Count+listApptsForHours.Count).ToString();
			int textSent=0;
			int emailSent=0;
			int cannotSend=0;
			bool hasTextingEnabled=SmsPhones.IsIntegratedTextingEnabled();
			if(checkSendAll.Checked) {//Much simpler, just take each patient on the appointments and see if both text and email are valid and tally them up.
				textApptCount.Text=((listApptsForDays.Count+listApptsForHours.Count)*2).ToString();//This assumes that email and text will both be successful for all appointments hourly and daily.
				EmailAddress emailAddress=EmailAddresses.GetByClinic(Clinics.ClinicNum);//Gets an address based on cascading priorities.
				for(int i=listApptsForDays.Count-1;i>=0;i--) {
					Patient pat=Patients.GetPat(listApptsForDays[i].PatNum);
					bool isContacted=false;
					if(emailAddress.EmailUsername!="" && CanEmail(pat)) {//Check for email validity
						isContacted=true;
						emailSent++;
					}
					else {
						cannotSend++;
					}
					if(hasTextingEnabled && CanText(pat)) {//Check for text validity
						isContacted=true;
						textSent++;
					}
					else {
						cannotSend++;
					}
					if(isContacted) {
						listApptsForDays.RemoveAt(i);//If they're contacted any of the above ways remove them from the list.
					}
				}
				for(int i=listApptsForHours.Count-1;i>=0;i--) {
					bool isContacted=false;
					Patient pat=Patients.GetPat(listApptsForHours[i].PatNum);
					if(emailAddress.EmailUsername!="" && CanEmail(pat)) {//Check for email validity
						isContacted=true;
						emailSent++;
					}
					else {
						cannotSend++;
					}
					if(hasTextingEnabled && CanText(pat)) {//Check for text validity
						isContacted=true;
						textSent++;
					}
					else {
						cannotSend++;
					}
					if(isContacted) {
						listApptsForHours.RemoveAt(i);//If they're contacted any of the above ways remove them from the list.
					}
				}
			}
			else {
				foreach(string commTypeString in _arrayPriorities) {
					//If an appt confirmation is sent by one method that appt can't have another reminder sent using a different method.
					CommType commType=(CommType)PIn.Int(commTypeString);
					if(commType==CommType.Email) {
						EmailAddress emailAddress=EmailAddresses.GetByClinic(Clinics.ClinicNum);//Gets an address based on cascading priorities.
						if(emailAddress.EmailUsername=="") {
							continue;//No email set up at all, just go to the next CommType
						}
						//Remove appointments from the list if either the patient or guarantor has a valid email.
						for(int i=listApptsForDays.Count-1;i>=0;i--) {
							Patient pat=Patients.GetPat(listApptsForDays[i].PatNum);
							if(CanEmail(pat)) {
								listApptsForDays.RemoveAt(i);
								emailSent++;
							}
						}
						for(int i=listApptsForHours.Count-1;i>=0;i--) {
							Patient pat=Patients.GetPat(listApptsForHours[i].PatNum);
							if(CanEmail(pat)) {
								listApptsForHours.RemoveAt(i);
								emailSent++;
							}
						}
					}
					if(commType==CommType.Text) {
						if(!hasTextingEnabled) {
							continue;//Integrated texting not set up, just go to the next CommType
						}
						for(int i=listApptsForDays.Count-1;i>=0;i--) {
							Patient pat=Patients.GetPat(listApptsForDays[i].PatNum);
							if(CanText(pat)) {
								//Either the patient or the guarantor has a valid phone number and texting is allowed.
								listApptsForDays.RemoveAt(i);
								textSent++;
							}
						}
						for(int i=listApptsForHours.Count-1;i>=0;i--) {
							Patient pat=Patients.GetPat(listApptsForHours[i].PatNum);
							if(CanText(pat)) {
								listApptsForHours.RemoveAt(i);
								textSent++;
							}
						}
					}
					if(commType==CommType.Preferred) {
						for(int i=listApptsForDays.Count-1;i>=0;i--) {
							Patient pat=Patients.GetPat(listApptsForDays[i].PatNum);
							if(pat.PreferContactMethod==ContactMethod.Email && CanEmail(pat)) {
								listApptsForDays.RemoveAt(i);
								emailSent++;
							}
							if(pat.PreferContactMethod==ContactMethod.TextMessage && CanText(pat) && hasTextingEnabled) {
								listApptsForDays.RemoveAt(i);
								textSent++;
							}
						}
						for(int i=listApptsForHours.Count-1;i>=0;i--) {
							Patient pat=Patients.GetPat(listApptsForHours[i].PatNum);
							if(pat.PreferContactMethod==ContactMethod.Email && CanEmail(pat)) {
								listApptsForHours.RemoveAt(i);
								emailSent++;
							}
							if(pat.PreferContactMethod==ContactMethod.TextMessage && CanText(pat) && hasTextingEnabled) {
								listApptsForHours.RemoveAt(i);
								textSent++;
							}
						}
					}
				}
			}
			textTextCount.Text=textSent.ToString();
			textEmailCount.Text=emailSent.ToString();
			textNeitherCount.Text=(listApptsForDays.Count+listApptsForHours.Count).ToString();
			if(checkSendAll.Checked) {
				textNeitherCount.Text=cannotSend.ToString();
			}
		}

		///<summary>Returns a boolean representing if this patient or the patient's guarantor can be contacted by email.  Takes into account DoNotCall contact methods.</summary>
		private bool CanEmail(Patient pat) {
			if(pat.PreferContactMethod==ContactMethod.DoNotCall) {
				return false;//Don't contact patients who have contact method of DoNotCall
			}
			if(pat.Email!="") {
				return true;
			}
			Family fam=Patients.GetFamily(pat.PatNum);
			if(fam.ListPats[0].PreferContactMethod==ContactMethod.DoNotCall) {
				return false;//Don't contact guarantors who have contact method of DoNotCall
			}
			if(fam.ListPats[0].Email!="") {
				return true;
			}
			return false;
		}

		private bool CanText(Patient pat) {
			if(pat.PreferContactMethod==ContactMethod.DoNotCall) {
				return false;//Don't contact patients who have contact method of DoNotCall
			}
			bool txtUnknownIsNo=PrefC.GetBool(PrefName.TextMsgOkStatusTreatAsNo);
			//If texting is marked as no, the phone is blank, or unknown are treated as no, look for guarantor texting status.
			if(pat.TxtMsgOk==YN.No || pat.WirelessPhone=="" || (pat.TxtMsgOk==YN.Unknown && txtUnknownIsNo)) {
				Family fam=Patients.GetFamily(pat.PatNum);
				if(fam.ListPats[0].PreferContactMethod==ContactMethod.DoNotCall) {
					return false;//Don't contact guarantors who have contact method of DoNotCall
				}
				if(fam.ListPats[0].TxtMsgOk==YN.No || fam.ListPats[0].WirelessPhone=="" || (fam.ListPats[0].TxtMsgOk==YN.Unknown && txtUnknownIsNo)) {
					return false;
				}
			}
			return true;//Either the patient or the guarantor has a valid wireless phone that is OK to text.
		}

		private void FillGridPriority() {
			gridPriorities.BeginUpdate();
			gridPriorities.Columns.Clear();
			gridPriorities.Columns.Add(new ODGridColumn("",100));
			gridPriorities.Columns.Add(new ODGridColumn("Status",50));
			gridPriorities.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_arrayPriorities.Length;i++) {
				row = new ODGridRow();
				CommType commType = (CommType)PIn.Int(_arrayPriorities[i]);
				row.Cells.Add(commType.ToString());
				switch(commType) {
					case CommType.Email:
						if(string.IsNullOrEmpty(EmailAddresses.GetByClinic(Clinics.ClinicNum).EmailUsername)) {
							row.Cells.Add("Not configured");
							row.ColorBackG=Color.LightGray;
						}
						break;
					case CommType.Text:
						if(!SmsPhones.IsIntegratedTextingEnabled()) {
							row.Cells.Add("Not configured");
							row.ColorBackG=Color.LightGray;
						}
						break;
					case CommType.Preferred:
					default:
						break;
				}
				row.Tag=(int)commType;
				gridPriorities.Rows.Add(row);
			}
			gridPriorities.EndUpdate();
		}

		private void textDayInterval_Leave(object sender,EventArgs e) {
			FillPriorityMetrics();
		}

		private void textHourInterval_Leave(object sender,EventArgs e) {
			FillPriorityMetrics();
		}

		private void checkSendAll_CheckedChanged(object sender,EventArgs e) {
			FillPriorityMetrics();
		}

		private void butUp_Click(object sender,EventArgs e) {
			if(gridPriorities.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select a row to move.");
				return;
			}
			if(gridPriorities.GetSelectedIndex()==0){//Return if it's already at the first position.
				return;
			}
			int selectedIdx=gridPriorities.GetSelectedIndex();
			string destinationString=_arrayPriorities[selectedIdx-1];
			_arrayPriorities[selectedIdx-1]=_arrayPriorities[selectedIdx];
			_arrayPriorities[selectedIdx]=destinationString;
			FillGridPriority();
			gridPriorities.SetSelected(selectedIdx-1,true);
			FillPriorityMetrics();
		}

		private void butDown_Click(object sender,EventArgs e) {
			if(gridPriorities.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select a row to move.");
				return;
			}
			if(gridPriorities.GetSelectedIndex()==gridPriorities.Rows.Count-1) {//Return if it's already at the last position.
				return;
			}
			int selectedIdx=gridPriorities.GetSelectedIndex();
			string destinationString=_arrayPriorities[selectedIdx+1];
			_arrayPriorities[selectedIdx+1]=_arrayPriorities[selectedIdx];
			_arrayPriorities[selectedIdx]=destinationString;
			FillGridPriority();
			gridPriorities.SetSelected(selectedIdx+1,true);
			FillPriorityMetrics();
		}

		#endregion Appt Reminder Setup

		#region Automation Settings
		private void FillAutomationTab() {
			//.NET has a bug in the DateTimePicker control where the text will not get updated and will instead default to showing DateTime.Now.
			//In order to get the control into a mode where it will display the correct value that we set, we need to set the property Checked to true.
			//Today's date will show even when the property is defaulted to true (via the designer), so we need to do it programmatically right here.
			//E.g. set your computer region to Assamese (India) and the DateTimePickers on the Automation Setting tab will both be set to todays date
			// if the tab is NOT set to be the first tab to display (don't ask me why it works then).
			//This is bad for our customers because setting both of the date pickers to the same date and time will cause automation to stop.
			dateRunStart.Checked=true;
			dateRunEnd.Checked=true;
			//Now that the DateTimePicker controls are ready to display the DateTime we set, go ahead and set them.
			dateRunStart.Value=PrefC.GetDateT(PrefName.AutomaticCommunicationTimeStart);
			_automationStart=dateRunStart.Value.TimeOfDay;
			dateRunEnd.Value=PrefC.GetDateT(PrefName.AutomaticCommunicationTimeEnd);
			_automationEnd=dateRunEnd.Value.TimeOfDay;
			switch(PrefC.GetInt(PrefName.WebSchedAutomaticSendSetting)) {
				case (int)WebSchedAutomaticSend.DoNotSend:
					radioDoNotSend.Checked=true;
					break;
				case (int)WebSchedAutomaticSend.SendToEmail:
					radioSendToEmail.Checked=true;
					break;
				case (int)WebSchedAutomaticSend.SendToEmailNoPreferred:
					radioSendToEmailNoPreferred.Checked=true;
					break;
				case (int)WebSchedAutomaticSend.SendToEmailOnlyPreferred:
					radioSendToEmailOnlyPreferred.Checked=true;
					break;
			}
		}
		#endregion

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textRight.errorProvider1.GetError(textRight)!=""
				|| textDown.errorProvider1.GetError(textDown)!=""
				|| textDaysPast.errorProvider1.GetError(textDaysPast)!=""
				|| textDaysFuture.errorProvider1.GetError(textDaysFuture)!=""
				|| textDaysFirstReminder.errorProvider1.GetError(textDaysFirstReminder)!=""
				|| textDaysSecondReminder.errorProvider1.GetError(textDaysSecondReminder)!=""
				|| textMaxReminders.errorProvider1.GetError(textMaxReminders)!=""
				|| textDaysPrior.errorProvider1.GetError(textDaysPrior)!=""
				|| textHoursPrior.errorProvider1.GetError(textHoursPrior)!="")
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(textDaysFirstReminder.Text=="") {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Initial Reminder box should not be blank, or the recall list will be blank.")) {
					return;
				}
			}
			if(textPostcardsPerSheet.Text!="1"
				&& textPostcardsPerSheet.Text!="3"
				&& textPostcardsPerSheet.Text!="4")
			{
				MsgBox.Show(this,"The value in postcards per sheet must be 1, 3, or 4");
				return;
			}
			if(comboStatusMailedRecall.SelectedIndex==-1 || comboStatusMailedRecall.SelectedIndex==-1){
				MsgBox.Show(this,"Both status options at the bottom must be set.");
				return; 
			}
			if(string.IsNullOrWhiteSpace(textMessageText.Text)) {
				MsgBox.Show(this,"Appointment reminder text message cannot be blank.");
				return;
			}
			//End of Validation
			if(Prefs.UpdateString(PrefName.RecallPostcardsPerSheet,textPostcardsPerSheet.Text)) {
				if(textPostcardsPerSheet.Text=="1"){
					MsgBox.Show(this,"If using 1 postcard per sheet, you must adjust the position, and also the preview will not work");
				}
			}
			Prefs.UpdateBool(PrefName.RecallCardsShowReturnAdd,checkReturnAdd.Checked);
			Prefs.UpdateBool(PrefName.RecallGroupByFamily,checkGroupFamilies.Checked);
			if(textDaysPast.Text=="") {
				Prefs.UpdateLong(PrefName.RecallDaysPast,-1);
			}
			else {
				Prefs.UpdateLong(PrefName.RecallDaysPast,PIn.Long(textDaysPast.Text));
			}
			if(textDaysFuture.Text=="") {
				Prefs.UpdateLong(PrefName.RecallDaysFuture,-1);
			}
			else {
				Prefs.UpdateLong(PrefName.RecallDaysFuture,PIn.Long(textDaysFuture.Text));
			}
			Prefs.UpdateBool(PrefName.RecallExcludeIfAnyFutureAppt,radioExcludeFutureYes.Checked);
			Prefs.UpdateDouble(PrefName.RecallAdjustRight,PIn.Double(textRight.Text));
			Prefs.UpdateDouble(PrefName.RecallAdjustDown,PIn.Double(textDown.Text));
			if(comboStatusEmailedRecall.SelectedIndex==-1){
				Prefs.UpdateLong(PrefName.RecallStatusEmailed,0);
			}
			else{
				Prefs.UpdateLong(PrefName.RecallStatusEmailed,DefC.Short[(int)DefCat.RecallUnschedStatus][comboStatusEmailedRecall.SelectedIndex].DefNum);
			}
			if(comboStatusMailedRecall.SelectedIndex==-1){
				Prefs.UpdateLong(PrefName.RecallStatusMailed,0);
			}
			else{
				Prefs.UpdateLong(PrefName.RecallStatusMailed,DefC.Short[(int)DefCat.RecallUnschedStatus][comboStatusMailedRecall.SelectedIndex].DefNum);
			}
			if(comboStatusEmailedConfirm.SelectedIndex==-1) {
				Prefs.UpdateLong(PrefName.ConfirmStatusEmailed,0);
			}
			else {
				Prefs.UpdateLong(PrefName.ConfirmStatusEmailed,DefC.Short[(int)DefCat.ApptConfirmed][comboStatusEmailedConfirm.SelectedIndex].DefNum);
			}
			if(comboStatusTextMessagedConfirm.SelectedIndex==-1) {
				Prefs.UpdateLong(PrefName.ConfirmStatusTextMessaged,0);
			}
			else {
				Prefs.UpdateLong(PrefName.ConfirmStatusTextMessaged,DefC.Short[(int)DefCat.ApptConfirmed][comboStatusTextMessagedConfirm.SelectedIndex].DefNum);
			}
			string recalltypes="";
			for(int i=0;i<listTypes.SelectedIndices.Count;i++){
				if(i>0){
					recalltypes+=",";
				}
				recalltypes+=RecallTypeC.Listt[listTypes.SelectedIndices[i]].RecallTypeNum.ToString();
			}
			Prefs.UpdateString(PrefName.RecallTypesShowingInList,recalltypes);
			if(textDaysFirstReminder.Text=="") {
				Prefs.UpdateLong(PrefName.RecallShowIfDaysFirstReminder,-1);
			}
			else {
				Prefs.UpdateLong(PrefName.RecallShowIfDaysFirstReminder,PIn.Long(textDaysFirstReminder.Text));
			}
			if(textDaysSecondReminder.Text=="") {
				Prefs.UpdateLong(PrefName.RecallShowIfDaysSecondReminder,-1);
			}
			else {
				Prefs.UpdateLong(PrefName.RecallShowIfDaysSecondReminder,PIn.Long(textDaysSecondReminder.Text));
			}
			if(textMaxReminders.Text=="") {
				Prefs.UpdateLong(PrefName.RecallMaxNumberReminders,-1);
			}
			else {
				Prefs.UpdateLong(PrefName.RecallMaxNumberReminders,PIn.Long(textMaxReminders.Text));
			}
			if(radioUseEmailTrue.Checked){
				Prefs.UpdateBool(PrefName.RecallUseEmailIfHasEmailAddress,true);
			}
			else{
				Prefs.UpdateBool(PrefName.RecallUseEmailIfHasEmailAddress,false);
			}
			if(radioDoNotSend.Checked) {
				Prefs.UpdateInt(PrefName.WebSchedAutomaticSendSetting,(int)WebSchedAutomaticSend.DoNotSend);
			}
			else if(radioSendToEmail.Checked) {
				Prefs.UpdateInt(PrefName.WebSchedAutomaticSendSetting,(int)WebSchedAutomaticSend.SendToEmail);
			}
			else if(radioSendToEmailNoPreferred.Checked) {
				Prefs.UpdateInt(PrefName.WebSchedAutomaticSendSetting,(int)WebSchedAutomaticSend.SendToEmailNoPreferred);
			}
			else {
				Prefs.UpdateInt(PrefName.WebSchedAutomaticSendSetting,(int)WebSchedAutomaticSend.SendToEmailOnlyPreferred);
			}
			Prefs.UpdateDateT(PrefName.AutomaticCommunicationTimeStart,dateRunStart.Value);
			Prefs.UpdateDateT(PrefName.AutomaticCommunicationTimeEnd,dateRunEnd.Value);
			//ApptComm preference updates MUST be after the AutomationStart and AutomationEnd updates.  Creating ApptComms depends on those values.
			Prefs.UpdateBool(PrefName.ApptReminderSendAll,checkSendAll.Checked);
			if(_daysPrior!=PIn.Double(textDaysPrior.Text) 
				|| _hoursPrior!=PIn.Double(textHoursPrior.Text) 
				//If they changed either of the automation start/end values and the automation start/end isn't disabled, re-do reminders.
				|| _automationStart!=dateRunStart.Value.TimeOfDay 
				|| _automationEnd!=dateRunEnd.Value.TimeOfDay) 
			{
				Prefs.UpdateDouble(PrefName.ApptReminderDayInterval,PIn.Double(textDaysPrior.Text));
				Prefs.UpdateDouble(PrefName.ApptReminderHourInterval,PIn.Double(textHoursPrior.Text));
				//Update ApptComms with new reminder entries.
				ApptComms.InsertForFutureAppts();
			}
			if(!SmsPhones.IsIntegratedTextingEnabled() || !SmsPhones.IsTextingForCountry("US")) {
				Prefs.UpdateString(PrefName.ApptReminderDayMessage,PIn.String(textMessageText.Text));
			}
			Prefs.UpdateString(PrefName.ApptReminderEmailMessage,textMessageEmail.Text);
			string sendOrder="";
			for(int i=0;i<gridPriorities.Rows.Count;i++) {
				if(i>0) {
					sendOrder+=",";
				}
				sendOrder+=((int)gridPriorities.Rows[i].Tag).ToString();
			}
			Prefs.UpdateString(PrefName.ApptReminderSendOrder,sendOrder);
			_changed=true;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormRecallSetup_FormClosing(object sender,FormClosingEventArgs e) {
			if(_changed) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}
	}
}
