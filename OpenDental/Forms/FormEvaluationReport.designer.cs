namespace OpenDental{
	partial class FormEvaluationReport {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEvaluationReport));
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.checkAllCourses = new System.Windows.Forms.CheckBox();
			this.checkAllInstructors = new System.Windows.Forms.CheckBox();
			this.textDateStart = new ODR.ValidDate();
			this.textDateEnd = new ODR.ValidDate();
			this.gridCourses = new OpenDental.UI.ODGrid();
			this.gridInstructors = new OpenDental.UI.ODGrid();
			this.gridStudent = new OpenDental.UI.ODGrid();
			this.butStudent = new OpenDental.UI.Button();
			this.butAverage = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.butEvaluationSelect = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.textEvaluation = new System.Windows.Forms.TextBox();
			this.button1 = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label7.Location = new System.Drawing.Point(244, 20);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(83, 18);
			this.label7.TabIndex = 43;
			this.label7.Text = "Date Start:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.Location = new System.Drawing.Point(478, 20);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(99, 18);
			this.label6.TabIndex = 42;
			this.label6.Text = "Date End:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkAllCourses
			// 
			this.checkAllCourses.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllCourses.Location = new System.Drawing.Point(114, 49);
			this.checkAllCourses.Name = "checkAllCourses";
			this.checkAllCourses.Size = new System.Drawing.Size(134, 16);
			this.checkAllCourses.TabIndex = 55;
			this.checkAllCourses.Text = "All Courses";
			this.checkAllCourses.CheckedChanged += new System.EventHandler(this.checkAllCourses_CheckedChanged);
			// 
			// checkAllInstructors
			// 
			this.checkAllInstructors.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllInstructors.Location = new System.Drawing.Point(434, 49);
			this.checkAllInstructors.Name = "checkAllInstructors";
			this.checkAllInstructors.Size = new System.Drawing.Size(141, 16);
			this.checkAllInstructors.TabIndex = 56;
			this.checkAllInstructors.Text = "All Instructors";
			this.checkAllInstructors.CheckedChanged += new System.EventHandler(this.checkAllInstructors_CheckedChanged);
			// 
			// textDateStart
			// 
			this.textDateStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textDateStart.Location = new System.Drawing.Point(333, 20);
			this.textDateStart.Name = "textDateStart";
			this.textDateStart.Size = new System.Drawing.Size(98, 20);
			this.textDateStart.TabIndex = 40;
			// 
			// textDateEnd
			// 
			this.textDateEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textDateEnd.Location = new System.Drawing.Point(583, 20);
			this.textDateEnd.Name = "textDateEnd";
			this.textDateEnd.Size = new System.Drawing.Size(98, 20);
			this.textDateEnd.TabIndex = 41;
			// 
			// gridCourses
			// 
			this.gridCourses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridCourses.HScrollVisible = false;
			this.gridCourses.Location = new System.Drawing.Point(12, 68);
			this.gridCourses.Name = "gridCourses";
			this.gridCourses.ScrollValue = 0;
			this.gridCourses.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridCourses.Size = new System.Drawing.Size(308, 362);
			this.gridCourses.TabIndex = 16;
			this.gridCourses.Title = "Courses";
			this.gridCourses.TranslationName = null;
			this.gridCourses.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridCourses_CellClick);
			// 
			// gridInstructors
			// 
			this.gridInstructors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridInstructors.HScrollVisible = false;
			this.gridInstructors.Location = new System.Drawing.Point(333, 68);
			this.gridInstructors.Name = "gridInstructors";
			this.gridInstructors.ScrollValue = 0;
			this.gridInstructors.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridInstructors.Size = new System.Drawing.Size(308, 362);
			this.gridInstructors.TabIndex = 15;
			this.gridInstructors.Title = "Instructors";
			this.gridInstructors.TranslationName = null;
			this.gridInstructors.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridInstructors_CellClick);
			// 
			// gridStudent
			// 
			this.gridStudent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridStudent.HScrollVisible = false;
			this.gridStudent.Location = new System.Drawing.Point(654, 68);
			this.gridStudent.Name = "gridStudent";
			this.gridStudent.ScrollValue = 0;
			this.gridStudent.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridStudent.Size = new System.Drawing.Size(308, 362);
			this.gridStudent.TabIndex = 14;
			this.gridStudent.Title = "Students";
			this.gridStudent.TranslationName = null;
			// 
			// butStudent
			// 
			this.butStudent.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butStudent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butStudent.Autosize = true;
			this.butStudent.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butStudent.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butStudent.CornerRadius = 4F;
			this.butStudent.Location = new System.Drawing.Point(434, 482);
			this.butStudent.Name = "butStudent";
			this.butStudent.Size = new System.Drawing.Size(93, 24);
			this.butStudent.TabIndex = 20;
			this.butStudent.Text = "Student Average";
			this.butStudent.Click += new System.EventHandler(this.butStudent_Click);
			// 
			// butAverage
			// 
			this.butAverage.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAverage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAverage.Autosize = true;
			this.butAverage.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAverage.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAverage.CornerRadius = 4F;
			this.butAverage.Location = new System.Drawing.Point(434, 452);
			this.butAverage.Name = "butAverage";
			this.butAverage.Size = new System.Drawing.Size(93, 24);
			this.butAverage.TabIndex = 19;
			this.butAverage.Text = "Course Average";
			this.butAverage.Click += new System.EventHandler(this.butAverage_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(887, 482);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 2;
			this.butClose.Text = "Close";
			this.butClose.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butEvaluationSelect
			// 
			this.butEvaluationSelect.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEvaluationSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butEvaluationSelect.Autosize = true;
			this.butEvaluationSelect.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEvaluationSelect.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEvaluationSelect.CornerRadius = 4F;
			this.butEvaluationSelect.Location = new System.Drawing.Point(204, 435);
			this.butEvaluationSelect.Name = "butEvaluationSelect";
			this.butEvaluationSelect.Size = new System.Drawing.Size(22, 21);
			this.butEvaluationSelect.TabIndex = 58;
			this.butEvaluationSelect.Text = "...";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(12, 435);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(83, 18);
			this.label1.TabIndex = 60;
			this.label1.Text = "Evaluation";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textEvaluation
			// 
			this.textEvaluation.Location = new System.Drawing.Point(97, 436);
			this.textEvaluation.MaxLength = 15;
			this.textEvaluation.Name = "textEvaluation";
			this.textEvaluation.Size = new System.Drawing.Size(105, 20);
			this.textEvaluation.TabIndex = 61;
			// 
			// button1
			// 
			this.button1.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Autosize = true;
			this.button1.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.button1.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.button1.CornerRadius = 4F;
			this.button1.Location = new System.Drawing.Point(769, 41);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(78, 24);
			this.button1.TabIndex = 62;
			this.button1.Text = "All Students";
			// 
			// FormEvaluationReport
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(974, 518);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.textEvaluation);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butEvaluationSelect);
			this.Controls.Add(this.checkAllInstructors);
			this.Controls.Add(this.checkAllCourses);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.textDateStart);
			this.Controls.Add(this.textDateEnd);
			this.Controls.Add(this.butStudent);
			this.Controls.Add(this.butAverage);
			this.Controls.Add(this.gridCourses);
			this.Controls.Add(this.gridInstructors);
			this.Controls.Add(this.gridStudent);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(100, 100);
			this.Name = "FormEvaluationReport";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Evaluation Report";
			this.Load += new System.EventHandler(this.FormEvaluationReport_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butClose;
		private UI.ODGrid gridStudent;
		private UI.ODGrid gridInstructors;
		private UI.ODGrid gridCourses;
		private UI.Button butAverage;
		private UI.Button butStudent;
		private System.Windows.Forms.Label label7;
		private ODR.ValidDate textDateStart;
		private ODR.ValidDate textDateEnd;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox checkAllCourses;
		private System.Windows.Forms.CheckBox checkAllInstructors;
		private UI.Button butEvaluationSelect;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textEvaluation;
		private UI.Button button1;
	}
}