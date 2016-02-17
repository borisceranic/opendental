namespace OpenDentalGraph {
	partial class BrokenApptGraphOptionsCtrl {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing&&(components!=null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.comboAdjType = new System.Windows.Forms.ComboBox();
			this.radioRunAdjs = new System.Windows.Forms.RadioButton();
			this.radioRunApts = new System.Windows.Forms.RadioButton();
			this.radioRunProcs = new System.Windows.Forms.RadioButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioGroupClinics = new System.Windows.Forms.RadioButton();
			this.radioGroupProvs = new System.Windows.Forms.RadioButton();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.groupBox3);
			this.groupBox2.Controls.Add(this.groupBox1);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox2.Location = new System.Drawing.Point(0, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(506, 137);
			this.groupBox2.TabIndex = 9;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Options";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label3);
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.Controls.Add(this.label1);
			this.groupBox3.Controls.Add(this.comboAdjType);
			this.groupBox3.Controls.Add(this.radioRunAdjs);
			this.groupBox3.Controls.Add(this.radioRunApts);
			this.groupBox3.Controls.Add(this.radioRunProcs);
			this.groupBox3.Location = new System.Drawing.Point(111, 19);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(383, 112);
			this.groupBox3.TabIndex = 4;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Count By";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(97, 37);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(280, 18);
			this.label2.TabIndex = 7;
			this.label2.Text = "Count broken appointments left on the schedule.";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(97, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(280, 18);
			this.label1.TabIndex = 6;
			this.label1.Text = "Count completed D9986 procedures.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboAdjType
			// 
			this.comboAdjType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAdjType.Enabled = false;
			this.comboAdjType.FormattingEnabled = true;
			this.comboAdjType.Location = new System.Drawing.Point(100, 79);
			this.comboAdjType.Name = "comboAdjType";
			this.comboAdjType.Size = new System.Drawing.Size(214, 21);
			this.comboAdjType.TabIndex = 5;
			this.comboAdjType.SelectedIndexChanged += new System.EventHandler(this.radioRunForChanged);
			// 
			// radioRunAdjs
			// 
			this.radioRunAdjs.AutoSize = true;
			this.radioRunAdjs.Location = new System.Drawing.Point(6, 59);
			this.radioRunAdjs.Name = "radioRunAdjs";
			this.radioRunAdjs.Size = new System.Drawing.Size(82, 17);
			this.radioRunAdjs.TabIndex = 4;
			this.radioRunAdjs.Text = "Adjustments";
			this.radioRunAdjs.UseVisualStyleBackColor = true;
			this.radioRunAdjs.CheckedChanged += new System.EventHandler(this.radioRunForChanged);
			// 
			// radioRunApts
			// 
			this.radioRunApts.AutoSize = true;
			this.radioRunApts.Location = new System.Drawing.Point(6, 38);
			this.radioRunApts.Name = "radioRunApts";
			this.radioRunApts.Size = new System.Drawing.Size(89, 17);
			this.radioRunApts.TabIndex = 1;
			this.radioRunApts.Text = "Appointments";
			this.radioRunApts.UseVisualStyleBackColor = true;
			this.radioRunApts.CheckedChanged += new System.EventHandler(this.radioRunForChanged);
			// 
			// radioRunProcs
			// 
			this.radioRunProcs.AutoSize = true;
			this.radioRunProcs.Checked = true;
			this.radioRunProcs.Location = new System.Drawing.Point(6, 17);
			this.radioRunProcs.Name = "radioRunProcs";
			this.radioRunProcs.Size = new System.Drawing.Size(79, 17);
			this.radioRunProcs.TabIndex = 3;
			this.radioRunProcs.TabStop = true;
			this.radioRunProcs.Text = "Procedures";
			this.radioRunProcs.UseVisualStyleBackColor = true;
			this.radioRunProcs.CheckedChanged += new System.EventHandler(this.radioRunForChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioGroupClinics);
			this.groupBox1.Controls.Add(this.radioGroupProvs);
			this.groupBox1.Location = new System.Drawing.Point(6, 19);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(99, 60);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Group By";
			// 
			// radioGroupClinics
			// 
			this.radioGroupClinics.AutoSize = true;
			this.radioGroupClinics.Location = new System.Drawing.Point(6, 38);
			this.radioGroupClinics.Name = "radioGroupClinics";
			this.radioGroupClinics.Size = new System.Drawing.Size(50, 17);
			this.radioGroupClinics.TabIndex = 2;
			this.radioGroupClinics.Text = "Clinic";
			this.radioGroupClinics.UseVisualStyleBackColor = true;
			this.radioGroupClinics.CheckedChanged += new System.EventHandler(this.radioGroupByChanged);
			// 
			// radioGroupProvs
			// 
			this.radioGroupProvs.AutoSize = true;
			this.radioGroupProvs.Checked = true;
			this.radioGroupProvs.Location = new System.Drawing.Point(6, 17);
			this.radioGroupProvs.Name = "radioGroupProvs";
			this.radioGroupProvs.Size = new System.Drawing.Size(64, 17);
			this.radioGroupProvs.TabIndex = 0;
			this.radioGroupProvs.TabStop = true;
			this.radioGroupProvs.Text = "Provider";
			this.radioGroupProvs.UseVisualStyleBackColor = true;
			this.radioGroupProvs.CheckedChanged += new System.EventHandler(this.radioGroupByChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(97, 58);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(280, 18);
			this.label3.TabIndex = 8;
			this.label3.Text = "Count the selected adjustment type.";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// BrokenApptGraphOptionsCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox2);
			this.Name = "BrokenApptGraphOptionsCtrl";
			this.Size = new System.Drawing.Size(506, 137);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton radioGroupClinics;
		private System.Windows.Forms.RadioButton radioRunApts;
		private System.Windows.Forms.RadioButton radioGroupProvs;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.RadioButton radioRunAdjs;
		private System.Windows.Forms.RadioButton radioRunProcs;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox comboAdjType;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
	}
}
