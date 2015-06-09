namespace OpenDental {
	partial class FormEhrMeasureEventEdit {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEhrMeasureEventEdit));
			this.butOK = new System.Windows.Forms.Button();
			this.butCancel = new System.Windows.Forms.Button();
			this.textMoreInfo = new System.Windows.Forms.TextBox();
			this.labelDateTime = new System.Windows.Forms.Label();
			this.textType = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.labelMoreInfo = new System.Windows.Forms.Label();
			this.butDelete = new System.Windows.Forms.Button();
			this.textResult = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textPatient = new System.Windows.Forms.TextBox();
			this.labelPatient = new System.Windows.Forms.Label();
			this.textDateTime = new OpenDental.ValidDate();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(394, 207);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 23);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "OK";
			this.butOK.UseVisualStyleBackColor = true;
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(475, 207);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 23);
			this.butCancel.TabIndex = 4;
			this.butCancel.Text = "Cancel";
			this.butCancel.UseVisualStyleBackColor = true;
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// textMoreInfo
			// 
			this.textMoreInfo.Location = new System.Drawing.Point(199, 116);
			this.textMoreInfo.Multiline = true;
			this.textMoreInfo.Name = "textMoreInfo";
			this.textMoreInfo.Size = new System.Drawing.Size(317, 55);
			this.textMoreInfo.TabIndex = 2;
			// 
			// labelDateTime
			// 
			this.labelDateTime.Location = new System.Drawing.Point(79, 18);
			this.labelDateTime.Name = "labelDateTime";
			this.labelDateTime.Size = new System.Drawing.Size(116, 17);
			this.labelDateTime.TabIndex = 19;
			this.labelDateTime.Text = "Date Time";
			this.labelDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textType
			// 
			this.textType.Location = new System.Drawing.Point(199, 64);
			this.textType.Name = "textType";
			this.textType.ReadOnly = true;
			this.textType.Size = new System.Drawing.Size(317, 20);
			this.textType.TabIndex = 20;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(79, 65);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(116, 17);
			this.label1.TabIndex = 21;
			this.label1.Text = "Event";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelMoreInfo
			// 
			this.labelMoreInfo.Location = new System.Drawing.Point(12, 117);
			this.labelMoreInfo.Name = "labelMoreInfo";
			this.labelMoreInfo.Size = new System.Drawing.Size(183, 54);
			this.labelMoreInfo.TabIndex = 22;
			this.labelMoreInfo.Text = "More information about the event.";
			this.labelMoreInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butDelete
			// 
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Location = new System.Drawing.Point(15, 207);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(75, 23);
			this.butDelete.TabIndex = 5;
			this.butDelete.Text = "Delete";
			this.butDelete.UseVisualStyleBackColor = true;
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// textResult
			// 
			this.textResult.Location = new System.Drawing.Point(199, 90);
			this.textResult.Name = "textResult";
			this.textResult.ReadOnly = true;
			this.textResult.Size = new System.Drawing.Size(317, 20);
			this.textResult.TabIndex = 24;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(79, 91);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(116, 17);
			this.label3.TabIndex = 25;
			this.label3.Text = "Result";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPatient
			// 
			this.textPatient.Location = new System.Drawing.Point(199, 41);
			this.textPatient.Name = "textPatient";
			this.textPatient.ReadOnly = true;
			this.textPatient.Size = new System.Drawing.Size(140, 20);
			this.textPatient.TabIndex = 26;
			// 
			// labelPatient
			// 
			this.labelPatient.Location = new System.Drawing.Point(79, 42);
			this.labelPatient.Name = "labelPatient";
			this.labelPatient.Size = new System.Drawing.Size(116, 17);
			this.labelPatient.TabIndex = 27;
			this.labelPatient.Text = "Patient";
			this.labelPatient.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDateTime
			// 
			this.textDateTime.Location = new System.Drawing.Point(199, 17);
			this.textDateTime.Name = "textDateTime";
			this.textDateTime.Size = new System.Drawing.Size(140, 20);
			this.textDateTime.TabIndex = 85;
			// 
			// FormEhrMeasureEventEdit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(564, 242);
			this.Controls.Add(this.textDateTime);
			this.Controls.Add(this.labelPatient);
			this.Controls.Add(this.textPatient);
			this.Controls.Add(this.textResult);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.labelMoreInfo);
			this.Controls.Add(this.textType);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.labelDateTime);
			this.Controls.Add(this.textMoreInfo);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(580, 280);
			this.Name = "FormEhrMeasureEventEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "EhrMeasureEvent Edit";
			this.Load += new System.EventHandler(this.FormEhrMeasureEventEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button butOK;
		private System.Windows.Forms.Button butCancel;
		private System.Windows.Forms.TextBox textMoreInfo;
		private System.Windows.Forms.Label labelDateTime;
		private System.Windows.Forms.TextBox textType;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label labelMoreInfo;
		private System.Windows.Forms.Button butDelete;
		private System.Windows.Forms.TextBox textResult;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textPatient;
		private System.Windows.Forms.Label labelPatient;
		private ValidDate textDateTime;
	}
}