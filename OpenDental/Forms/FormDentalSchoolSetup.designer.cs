namespace OpenDental{
	partial class FormDentalSchoolSetup {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDentalSchoolSetup));
			this.butCancel = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.butStudentPicker = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.butInstructorPicker = new OpenDental.UI.Button();
			this.textStudents = new System.Windows.Forms.TextBox();
			this.textInstructors = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.butEvaluation = new OpenDental.UI.Button();
			this.butGradingScales = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(313, 184);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 7;
			this.butCancel.Text = "Close";
			this.butCancel.Click += new System.EventHandler(this.butClose_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 51);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(184, 15);
			this.label1.TabIndex = 104;
			this.label1.Text = "Security Group for Students";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butStudentPicker
			// 
			this.butStudentPicker.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butStudentPicker.Autosize = true;
			this.butStudentPicker.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butStudentPicker.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butStudentPicker.CornerRadius = 4F;
			this.butStudentPicker.Location = new System.Drawing.Point(329, 47);
			this.butStudentPicker.Name = "butStudentPicker";
			this.butStudentPicker.Size = new System.Drawing.Size(22, 22);
			this.butStudentPicker.TabIndex = 2;
			this.butStudentPicker.Text = "...";
			this.butStudentPicker.Click += new System.EventHandler(this.butStudentPicker_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 81);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(184, 15);
			this.label2.TabIndex = 107;
			this.label2.Text = "Security Group for Instructors";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butInstructorPicker
			// 
			this.butInstructorPicker.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butInstructorPicker.Autosize = true;
			this.butInstructorPicker.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butInstructorPicker.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butInstructorPicker.CornerRadius = 4F;
			this.butInstructorPicker.Location = new System.Drawing.Point(329, 77);
			this.butInstructorPicker.Name = "butInstructorPicker";
			this.butInstructorPicker.Size = new System.Drawing.Size(22, 22);
			this.butInstructorPicker.TabIndex = 4;
			this.butInstructorPicker.Text = "...";
			this.butInstructorPicker.Click += new System.EventHandler(this.butInstructorPicker_Click);
			// 
			// textStudents
			// 
			this.textStudents.Location = new System.Drawing.Point(202, 49);
			this.textStudents.MaxLength = 255;
			this.textStudents.Name = "textStudents";
			this.textStudents.ReadOnly = true;
			this.textStudents.Size = new System.Drawing.Size(121, 20);
			this.textStudents.TabIndex = 1;
			// 
			// textInstructors
			// 
			this.textInstructors.Location = new System.Drawing.Point(202, 79);
			this.textInstructors.MaxLength = 255;
			this.textInstructors.Name = "textInstructors";
			this.textInstructors.ReadOnly = true;
			this.textInstructors.Size = new System.Drawing.Size(121, 20);
			this.textInstructors.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(32, 9);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(319, 32);
			this.label3.TabIndex = 112;
			this.label3.Text = "These picker buttons will change the security user group for all users that are a" +
    " student or an instructor.";
			// 
			// butEvaluation
			// 
			this.butEvaluation.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEvaluation.Autosize = true;
			this.butEvaluation.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEvaluation.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEvaluation.CornerRadius = 4F;
			this.butEvaluation.Location = new System.Drawing.Point(202, 135);
			this.butEvaluation.Name = "butEvaluation";
			this.butEvaluation.Size = new System.Drawing.Size(105, 24);
			this.butEvaluation.TabIndex = 5;
			this.butEvaluation.Text = "Evaluations";
			this.butEvaluation.Click += new System.EventHandler(this.butEvaluation_Click);
			// 
			// butGradingScales
			// 
			this.butGradingScales.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGradingScales.Autosize = true;
			this.butGradingScales.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGradingScales.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGradingScales.CornerRadius = 4F;
			this.butGradingScales.Location = new System.Drawing.Point(202, 105);
			this.butGradingScales.Name = "butGradingScales";
			this.butGradingScales.Size = new System.Drawing.Size(105, 24);
			this.butGradingScales.TabIndex = 113;
			this.butGradingScales.Text = "Grading Scales";
			this.butGradingScales.Click += new System.EventHandler(this.butGradingScales_Click);
			// 
			// FormDentalSchoolSetup
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(400, 220);
			this.Controls.Add(this.butGradingScales);
			this.Controls.Add(this.butEvaluation);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textInstructors);
			this.Controls.Add(this.textStudents);
			this.Controls.Add(this.butInstructorPicker);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butStudentPicker);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(416, 258);
			this.Name = "FormDentalSchoolSetup";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Dental School Setup";
			this.Load += new System.EventHandler(this.FormDentalSchoolSetup_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.Label label1;
		private UI.Button butStudentPicker;
		private System.Windows.Forms.Label label2;
		private UI.Button butInstructorPicker;
		private System.Windows.Forms.TextBox textStudents;
		private System.Windows.Forms.TextBox textInstructors;
		private System.Windows.Forms.Label label3;
		private UI.Button butEvaluation;
		private UI.Button butGradingScales;
	}
}