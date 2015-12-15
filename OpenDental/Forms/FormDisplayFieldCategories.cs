using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary></summary>
	public class FormDisplayFieldCategories:System.Windows.Forms.Form {
		private Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		//private bool changed;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private ListBox listCategory;
		//private List<DisplayField> ListShowing;
		//private List<DisplayField> ListAvailable;

		///<summary></summary>
		public FormDisplayFieldCategories()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDisplayFieldCategories));
			this.label1 = new System.Windows.Forms.Label();
			this.listCategory = new System.Windows.Forms.ListBox();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(23, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(194, 17);
			this.label1.TabIndex = 2;
			this.label1.Text = "Select a category";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listCategory
			// 
			this.listCategory.FormattingEnabled = true;
			this.listCategory.Location = new System.Drawing.Point(23, 34);
			this.listCategory.Name = "listCategory";
			this.listCategory.Size = new System.Drawing.Size(144, 160);
			this.listCategory.TabIndex = 57;
			this.listCategory.DoubleClick += new System.EventHandler(this.listCategory_DoubleClick);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(92, 207);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 56;
			this.butOK.Text = "OK";
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
			this.butCancel.Location = new System.Drawing.Point(173, 207);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormDisplayFieldCategories
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(271, 246);
			this.Controls.Add(this.listCategory);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormDisplayFieldCategories";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Setup Display Fields";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDisplayFields_FormClosing);
			this.Load += new System.EventHandler(this.FormDisplayFields_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormDisplayFields_Load(object sender,EventArgs e) {
			//Alphabetical order.  When new display fields are added this will need to be changed.
			listCategory.Items.Add(Lan.g("enumDisplayFieldCategory","AccountModule"));//0
			listCategory.Items.Add(Lan.g("enumDisplayFieldCategory","AccountPatientInformation"));//1
			listCategory.Items.Add(Lan.g("enumDisplayFieldCategory","AppointmentBubble"));//2
			listCategory.Items.Add(Lan.g("enumDisplayFieldCategory","AppointmentEdit"));//3
			listCategory.Items.Add(Lan.g("enumDisplayFieldCategory","ChartPatientInformation"));//4
			listCategory.Items.Add(Lan.g("enumDisplayFieldCategory","FamilyRecallGrid"));//5
			//skip None because user not allowed to select that
			if(!Clinics.IsMedicalPracticeOrClinic(FormOpenDental.ClinicNum)) {
				listCategory.Items.Add(Lan.g("enumDisplayFieldCategory","OrthoChart"));
			}
			listCategory.Items.Add(Lan.g("enumDisplayFieldCategory","PatientInformation"));
			listCategory.Items.Add(Lan.g("enumDisplayFieldCategory","PatientSelect"));
			listCategory.Items.Add(Lan.g("enumDisplayFieldCategory","PlannedAppointmentEdit"));
			listCategory.Items.Add(Lan.g("enumDisplayFieldCategory","ProcedureGroupNote"));
			listCategory.Items.Add(Lan.g("enumDisplayFieldCategory","RecallList"));
			listCategory.Items.Add(Lan.g("enumDisplayFieldCategory","StatementMainGrid"));
			listCategory.Items.Add(Lan.g("enumDisplayFieldCategory","TreatmentPlanModule"));
			listCategory.SelectedIndex=0;
		}

		private void listCategory_DoubleClick(object sender,EventArgs e) {
			DisplayFieldCategory selectedCategory=DisplayFieldCategory.None;
			int index=listCategory.SelectedIndex;
			if(Clinics.IsMedicalPracticeOrClinic(FormOpenDental.ClinicNum) && index >= 6) {
				index++;
			}
			//When new display fields are added this switch statement will need to be changed to match the order set in the load.
			switch(index) {
				case 0: selectedCategory=DisplayFieldCategory.AccountModule; break;
				case 1: selectedCategory=DisplayFieldCategory.AccountPatientInformation; break;
				case 2: selectedCategory=DisplayFieldCategory.AppointmentBubble; break;
				case 3: selectedCategory=DisplayFieldCategory.AppointmentEdit; break;
				case 4: selectedCategory=DisplayFieldCategory.ChartPatientInformation; break;
				case 5: selectedCategory=DisplayFieldCategory.FamilyRecallGrid; break;
				case 6: selectedCategory=DisplayFieldCategory.OrthoChart; break;
				case 7: selectedCategory=DisplayFieldCategory.PatientInformation; break;
				case 8: selectedCategory=DisplayFieldCategory.PatientSelect; break;
				case 9: selectedCategory=DisplayFieldCategory.PlannedAppointmentEdit; break;
				case 10: selectedCategory=DisplayFieldCategory.ProcedureGroupNote; break;
				case 11: selectedCategory=DisplayFieldCategory.RecallList; break;
				case 12: selectedCategory=DisplayFieldCategory.StatementMainGrid; break;
				case 13: selectedCategory=DisplayFieldCategory.TreatmentPlanModule; break;
			}
			if(selectedCategory==DisplayFieldCategory.None) {
				return;//This could happen if a programmer added a new item to the list and didn't include it in the switch statement above.
			}
			FormDisplayFields FormF=new FormDisplayFields();
			FormF.Category=selectedCategory;
			FormF.ShowDialog();
			Close();
		}

		private void butOK_Click(object sender,EventArgs e) {
			DisplayFieldCategory selectedCategory=DisplayFieldCategory.None;
			int index=listCategory.SelectedIndex;
			if(Clinics.IsMedicalPracticeOrClinic(FormOpenDental.ClinicNum) && index >= 6) {
				index++;
			}
			//When new display fields are added this switch statement will need to be changed to match the order set in the load.
			switch(index) {
				case 0: selectedCategory=DisplayFieldCategory.AccountModule; break;
				case 1: selectedCategory=DisplayFieldCategory.AccountPatientInformation; break;
				case 2: selectedCategory=DisplayFieldCategory.AppointmentBubble; break;
				case 3: selectedCategory=DisplayFieldCategory.AppointmentEdit; break;
				case 4: selectedCategory=DisplayFieldCategory.ChartPatientInformation; break;
				case 5: selectedCategory=DisplayFieldCategory.FamilyRecallGrid; break;
				case 6: selectedCategory=DisplayFieldCategory.OrthoChart; break;
				case 7: selectedCategory=DisplayFieldCategory.PatientInformation; break;
				case 8: selectedCategory=DisplayFieldCategory.PatientSelect; break;
				case 9: selectedCategory=DisplayFieldCategory.PlannedAppointmentEdit; break;
				case 10: selectedCategory=DisplayFieldCategory.ProcedureGroupNote; break;
				case 11: selectedCategory=DisplayFieldCategory.RecallList; break;
				case 12: selectedCategory=DisplayFieldCategory.StatementMainGrid; break;
				case 13: selectedCategory=DisplayFieldCategory.TreatmentPlanModule; break;
			}
			if(selectedCategory==DisplayFieldCategory.None) {
				return;//This could happen if a programmer added a new item to the list and didn't include it in the switch statement above.
			}
			FormDisplayFields FormF=new FormDisplayFields();
			FormF.Category=selectedCategory;
			FormF.ShowDialog();
			Close();
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			Close();
		}

		private void FormDisplayFields_FormClosing(object sender,FormClosingEventArgs e) {

		}

		

		

		

		

		

		

		


	}
}





















