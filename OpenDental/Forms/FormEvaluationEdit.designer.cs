namespace OpenDental{
	partial class FormEvaluationEdit {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEvaluationEdit));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.textCourse = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textTitle = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textGradeScaleName = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.textInstructor = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textStudent = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.textGradeShowingOverride = new System.Windows.Forms.TextBox();
			this.butStudentPicker = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.textDate = new OpenDental.ValidDate();
			this.label8 = new System.Windows.Forms.Label();
			this.textGradeNumberOverride = new System.Windows.Forms.TextBox();
			this.textGradeNumber = new System.Windows.Forms.TextBox();
			this.textGradeShowing = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(562, 530);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 5;
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
			this.butCancel.Location = new System.Drawing.Point(562, 560);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 6;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(6, 118);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(137, 17);
			this.label3.TabIndex = 134;
			this.label3.Text = "Course";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textCourse
			// 
			this.textCourse.Location = new System.Drawing.Point(149, 117);
			this.textCourse.MaxLength = 255;
			this.textCourse.Name = "textCourse";
			this.textCourse.ReadOnly = true;
			this.textCourse.Size = new System.Drawing.Size(121, 20);
			this.textCourse.TabIndex = 131;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 67);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(137, 17);
			this.label2.TabIndex = 133;
			this.label2.Text = "Evaluation title";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTitle
			// 
			this.textTitle.Location = new System.Drawing.Point(149, 66);
			this.textTitle.MaxLength = 255;
			this.textTitle.Name = "textTitle";
			this.textTitle.ReadOnly = true;
			this.textTitle.Size = new System.Drawing.Size(121, 20);
			this.textTitle.TabIndex = 129;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(6, 92);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(137, 17);
			this.label1.TabIndex = 132;
			this.label1.Text = "Grading Scale";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textGradeScaleName
			// 
			this.textGradeScaleName.Location = new System.Drawing.Point(149, 91);
			this.textGradeScaleName.MaxLength = 255;
			this.textGradeScaleName.Name = "textGradeScaleName";
			this.textGradeScaleName.ReadOnly = true;
			this.textGradeScaleName.Size = new System.Drawing.Size(121, 20);
			this.textGradeScaleName.TabIndex = 130;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(6, 40);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(137, 17);
			this.label4.TabIndex = 140;
			this.label4.Text = "Date";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(276, 92);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(137, 17);
			this.label5.TabIndex = 139;
			this.label5.Text = "Instructor";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInstructor
			// 
			this.textInstructor.Location = new System.Drawing.Point(419, 91);
			this.textInstructor.MaxLength = 255;
			this.textInstructor.Name = "textInstructor";
			this.textInstructor.ReadOnly = true;
			this.textInstructor.Size = new System.Drawing.Size(121, 20);
			this.textInstructor.TabIndex = 135;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(276, 118);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(137, 17);
			this.label6.TabIndex = 138;
			this.label6.Text = "Student";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textStudent
			// 
			this.textStudent.Location = new System.Drawing.Point(419, 117);
			this.textStudent.MaxLength = 255;
			this.textStudent.Name = "textStudent";
			this.textStudent.ReadOnly = true;
			this.textStudent.Size = new System.Drawing.Size(121, 20);
			this.textStudent.TabIndex = 136;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(276, 41);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(137, 17);
			this.label7.TabIndex = 142;
			this.label7.Text = "Overall Grade Showing";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textGradeShowingOverride
			// 
			this.textGradeShowingOverride.Location = new System.Drawing.Point(480, 40);
			this.textGradeShowingOverride.MaxLength = 255;
			this.textGradeShowingOverride.Name = "textGradeShowingOverride";
			this.textGradeShowingOverride.Size = new System.Drawing.Size(60, 20);
			this.textGradeShowingOverride.TabIndex = 2;
			// 
			// butStudentPicker
			// 
			this.butStudentPicker.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butStudentPicker.Autosize = true;
			this.butStudentPicker.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butStudentPicker.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butStudentPicker.CornerRadius = 4F;
			this.butStudentPicker.Location = new System.Drawing.Point(546, 114);
			this.butStudentPicker.Name = "butStudentPicker";
			this.butStudentPicker.Size = new System.Drawing.Size(24, 24);
			this.butStudentPicker.TabIndex = 4;
			this.butStudentPicker.Text = "...";
			this.butStudentPicker.Click += new System.EventHandler(this.butStudentPicker_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(14, 139);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(526, 415);
			this.gridMain.TabIndex = 143;
			this.gridMain.Title = "Criterion";
			this.gridMain.TranslationName = "FormEvaluationDefEdit";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// textDate
			// 
			this.textDate.Location = new System.Drawing.Point(149, 40);
			this.textDate.Name = "textDate";
			this.textDate.Size = new System.Drawing.Size(121, 20);
			this.textDate.TabIndex = 1;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(276, 67);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(137, 17);
			this.label8.TabIndex = 147;
			this.label8.Text = "Overall Grade Number";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textGradeNumberOverride
			// 
			this.textGradeNumberOverride.Location = new System.Drawing.Point(480, 66);
			this.textGradeNumberOverride.MaxLength = 255;
			this.textGradeNumberOverride.Name = "textGradeNumberOverride";
			this.textGradeNumberOverride.Size = new System.Drawing.Size(60, 20);
			this.textGradeNumberOverride.TabIndex = 3;
			// 
			// textGradeNumber
			// 
			this.textGradeNumber.Location = new System.Drawing.Point(419, 66);
			this.textGradeNumber.MaxLength = 255;
			this.textGradeNumber.Name = "textGradeNumber";
			this.textGradeNumber.ReadOnly = true;
			this.textGradeNumber.Size = new System.Drawing.Size(60, 20);
			this.textGradeNumber.TabIndex = 149;
			// 
			// textGradeShowing
			// 
			this.textGradeShowing.Location = new System.Drawing.Point(419, 40);
			this.textGradeShowing.MaxLength = 255;
			this.textGradeShowing.Name = "textGradeShowing";
			this.textGradeShowing.ReadOnly = true;
			this.textGradeShowing.Size = new System.Drawing.Size(60, 20);
			this.textGradeShowing.TabIndex = 148;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(403, 20);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(137, 17);
			this.label9.TabIndex = 150;
			this.label9.Text = "Override";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormEvaluationEdit
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(649, 596);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.textGradeNumber);
			this.Controls.Add(this.textGradeShowing);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.textGradeNumberOverride);
			this.Controls.Add(this.textDate);
			this.Controls.Add(this.butStudentPicker);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.textGradeShowingOverride);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textInstructor);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.textStudent);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textCourse);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textTitle);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textGradeScaleName);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(647, 386);
			this.Name = "FormEvaluationEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Evaluation Edit";
			this.Load += new System.EventHandler(this.FormEvaluationEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textCourse;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textTitle;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textGradeScaleName;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textInstructor;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textStudent;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox textGradeShowingOverride;
		private UI.ODGrid gridMain;
		private UI.Button butStudentPicker;
		private ValidDate textDate;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox textGradeNumberOverride;
		private System.Windows.Forms.TextBox textGradeNumber;
		private System.Windows.Forms.TextBox textGradeShowing;
		private System.Windows.Forms.Label label9;
	}
}