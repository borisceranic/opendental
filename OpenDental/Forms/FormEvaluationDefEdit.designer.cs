namespace OpenDental{
	partial class FormEvaluationDefEdit {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEvaluationDefEdit));
			this.labelCriterion = new System.Windows.Forms.Label();
			this.listCriterion = new System.Windows.Forms.ListBox();
			this.textGradeScaleName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textTitle = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textCourse = new System.Windows.Forms.TextBox();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butCoursePicker = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.butCriterionAdd = new OpenDental.UI.Button();
			this.butGradingScale = new OpenDental.UI.Button();
			this.butRemove = new OpenDental.UI.Button();
			this.butLeft = new OpenDental.UI.Button();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// labelCriterion
			// 
			this.labelCriterion.Location = new System.Drawing.Point(370, 89);
			this.labelCriterion.Name = "labelCriterion";
			this.labelCriterion.Size = new System.Drawing.Size(213, 17);
			this.labelCriterion.TabIndex = 64;
			this.labelCriterion.Text = "Criterion Available";
			this.labelCriterion.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listCriterion
			// 
			this.listCriterion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listCriterion.FormattingEnabled = true;
			this.listCriterion.IntegralHeight = false;
			this.listCriterion.Location = new System.Drawing.Point(373, 109);
			this.listCriterion.Name = "listCriterion";
			this.listCriterion.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listCriterion.Size = new System.Drawing.Size(158, 396);
			this.listCriterion.TabIndex = 63;
			this.listCriterion.DoubleClick += new System.EventHandler(this.listCriterion_DoubleClick);
			// 
			// textGradeScaleName
			// 
			this.textGradeScaleName.Location = new System.Drawing.Point(154, 41);
			this.textGradeScaleName.MaxLength = 255;
			this.textGradeScaleName.Name = "textGradeScaleName";
			this.textGradeScaleName.ReadOnly = true;
			this.textGradeScaleName.Size = new System.Drawing.Size(121, 20);
			this.textGradeScaleName.TabIndex = 112;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(11, 42);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(137, 17);
			this.label1.TabIndex = 114;
			this.label1.Text = "Grading Scale";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(11, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(137, 17);
			this.label2.TabIndex = 125;
			this.label2.Text = "Evaluation title";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTitle
			// 
			this.textTitle.Location = new System.Drawing.Point(154, 15);
			this.textTitle.MaxLength = 255;
			this.textTitle.Name = "textTitle";
			this.textTitle.Size = new System.Drawing.Size(121, 20);
			this.textTitle.TabIndex = 124;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(11, 68);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(137, 17);
			this.label3.TabIndex = 128;
			this.label3.Text = "Course";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textCourse
			// 
			this.textCourse.Location = new System.Drawing.Point(154, 67);
			this.textCourse.MaxLength = 255;
			this.textCourse.Name = "textCourse";
			this.textCourse.ReadOnly = true;
			this.textCourse.Size = new System.Drawing.Size(121, 20);
			this.textCourse.TabIndex = 127;
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 93);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(292, 412);
			this.gridMain.TabIndex = 60;
			this.gridMain.Title = "Criterion Used";
			this.gridMain.TranslationName = "FormEvaluationDefEdit";
			this.gridMain.DoubleClick += new System.EventHandler(this.gridMain_DoubleClick);
			// 
			// butCoursePicker
			// 
			this.butCoursePicker.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCoursePicker.Autosize = true;
			this.butCoursePicker.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCoursePicker.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCoursePicker.CornerRadius = 4F;
			this.butCoursePicker.Location = new System.Drawing.Point(280, 65);
			this.butCoursePicker.Name = "butCoursePicker";
			this.butCoursePicker.Size = new System.Drawing.Size(24, 24);
			this.butCoursePicker.TabIndex = 126;
			this.butCoursePicker.Text = "...";
			this.butCoursePicker.Click += new System.EventHandler(this.butCoursePicker_Click);
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
			this.butDelete.Location = new System.Drawing.Point(456, 542);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(75, 24);
			this.butDelete.TabIndex = 123;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butCriterionAdd
			// 
			this.butCriterionAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCriterionAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butCriterionAdd.Autosize = true;
			this.butCriterionAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCriterionAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCriterionAdd.CornerRadius = 4F;
			this.butCriterionAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butCriterionAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butCriterionAdd.Location = new System.Drawing.Point(566, 281);
			this.butCriterionAdd.Name = "butCriterionAdd";
			this.butCriterionAdd.Size = new System.Drawing.Size(75, 24);
			this.butCriterionAdd.TabIndex = 113;
			this.butCriterionAdd.Text = "Add";
			this.butCriterionAdd.Click += new System.EventHandler(this.butCriterionAdd_Click);
			// 
			// butGradingScale
			// 
			this.butGradingScale.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGradingScale.Autosize = true;
			this.butGradingScale.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGradingScale.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGradingScale.CornerRadius = 4F;
			this.butGradingScale.Location = new System.Drawing.Point(280, 39);
			this.butGradingScale.Name = "butGradingScale";
			this.butGradingScale.Size = new System.Drawing.Size(24, 24);
			this.butGradingScale.TabIndex = 68;
			this.butGradingScale.Text = "...";
			this.butGradingScale.Click += new System.EventHandler(this.butGradingScale_Click);
			// 
			// butRemove
			// 
			this.butRemove.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRemove.Autosize = true;
			this.butRemove.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRemove.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRemove.CornerRadius = 4F;
			this.butRemove.Location = new System.Drawing.Point(231, 512);
			this.butRemove.Name = "butRemove";
			this.butRemove.Size = new System.Drawing.Size(73, 24);
			this.butRemove.TabIndex = 66;
			this.butRemove.Text = "Remove";
			this.butRemove.Click += new System.EventHandler(this.butRemove_Click);
			// 
			// butLeft
			// 
			this.butLeft.AdjustImageLocation = new System.Drawing.Point(-1, 0);
			this.butLeft.Autosize = true;
			this.butLeft.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLeft.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLeft.CornerRadius = 4F;
			this.butLeft.Image = global::OpenDental.Properties.Resources.Left;
			this.butLeft.Location = new System.Drawing.Point(321, 281);
			this.butLeft.Name = "butLeft";
			this.butLeft.Size = new System.Drawing.Size(35, 24);
			this.butLeft.TabIndex = 65;
			this.butLeft.Click += new System.EventHandler(this.butLeft_Click);
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(109, 511);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(82, 24);
			this.butDown.TabIndex = 62;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(12, 511);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(82, 24);
			this.butUp.TabIndex = 61;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(566, 512);
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
			this.butCancel.Location = new System.Drawing.Point(566, 542);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormEvaluationDefEdit
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(653, 578);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textCourse);
			this.Controls.Add(this.butCoursePicker);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textTitle);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butCriterionAdd);
			this.Controls.Add(this.textGradeScaleName);
			this.Controls.Add(this.butGradingScale);
			this.Controls.Add(this.butRemove);
			this.Controls.Add(this.butLeft);
			this.Controls.Add(this.labelCriterion);
			this.Controls.Add(this.listCriterion);
			this.Controls.Add(this.butDown);
			this.Controls.Add(this.butUp);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(669, 616);
			this.Name = "FormEvaluationDefEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Evaluation Definition Edit";
			this.Load += new System.EventHandler(this.FormEvaluationDefEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private UI.Button butRemove;
		private UI.Button butLeft;
		private System.Windows.Forms.Label labelCriterion;
		private System.Windows.Forms.ListBox listCriterion;
		private UI.Button butDown;
		private UI.Button butUp;
		private UI.ODGrid gridMain;
		private UI.Button butGradingScale;
		private System.Windows.Forms.TextBox textGradeScaleName;
		private UI.Button butCriterionAdd;
		private System.Windows.Forms.Label label1;
		private UI.Button butDelete;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textTitle;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textCourse;
		private UI.Button butCoursePicker;
	}
}