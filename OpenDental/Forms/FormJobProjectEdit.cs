using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental{
	/// <summary></summary>
	public class FormJobProjectEdit : System.Windows.Forms.Form{
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		///<summary></summary>
		public bool IsNew;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textJobProjName;
		private JobProject _jobProjCur;
		private ODtextBox textJobProjDescrip;
		private Label label3;
		private ComboBox comboJobProjStatus;
		private Label label4;
		private Label label5;
		private UI.Button butDelete;
		private TextBox labelParentProj;
		private TextBox labelRootProj;
		public List<JobProject> ListJobProjects;

		///<summary></summary>
		public FormJobProjectEdit(JobProject jobProjCur)
		{
			//
			// Required for Windows Form Designer support
			//
			_jobProjCur=jobProjCur;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormJobProjectEdit));
			this.label1 = new System.Windows.Forms.Label();
			this.textJobProjName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.comboJobProjStatus = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.butDelete = new OpenDental.UI.Button();
			this.textJobProjDescrip = new OpenDental.ODtextBox();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.labelParentProj = new System.Windows.Forms.TextBox();
			this.labelRootProj = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(1, 45);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(148, 17);
			this.label1.TabIndex = 2;
			this.label1.Text = "Title";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textJobProjName
			// 
			this.textJobProjName.Location = new System.Drawing.Point(152, 44);
			this.textJobProjName.MaxLength = 255;
			this.textJobProjName.Name = "textJobProjName";
			this.textJobProjName.Size = new System.Drawing.Size(241, 20);
			this.textJobProjName.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(0, 67);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(151, 17);
			this.label2.TabIndex = 99;
			this.label2.Text = "Description";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(4, 25);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(145, 17);
			this.label3.TabIndex = 101;
			this.label3.Text = "Status";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboJobProjStatus
			// 
			this.comboJobProjStatus.FormattingEnabled = true;
			this.comboJobProjStatus.Location = new System.Drawing.Point(152, 22);
			this.comboJobProjStatus.Name = "comboJobProjStatus";
			this.comboJobProjStatus.Size = new System.Drawing.Size(116, 21);
			this.comboJobProjStatus.TabIndex = 103;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(-2, 166);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(151, 17);
			this.label4.TabIndex = 104;
			this.label4.Text = "Parent Project";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(-2, 188);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(151, 17);
			this.label5.TabIndex = 105;
			this.label5.Text = "Root Project";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
			this.butDelete.Location = new System.Drawing.Point(7, 228);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(84, 26);
			this.butDelete.TabIndex = 108;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// textJobProjDescrip
			// 
			this.textJobProjDescrip.AcceptsTab = true;
			this.textJobProjDescrip.DetectUrls = false;
			this.textJobProjDescrip.Location = new System.Drawing.Point(152, 67);
			this.textJobProjDescrip.Name = "textJobProjDescrip";
			this.textJobProjDescrip.QuickPasteType = OpenDentBusiness.QuickPasteType.None;
			this.textJobProjDescrip.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textJobProjDescrip.Size = new System.Drawing.Size(241, 96);
			this.textJobProjDescrip.TabIndex = 100;
			this.textJobProjDescrip.Text = "";
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(487, 196);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 8;
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
			this.butCancel.Location = new System.Drawing.Point(487, 228);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 9;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// labelParentProj
			// 
			this.labelParentProj.Location = new System.Drawing.Point(152, 166);
			this.labelParentProj.Name = "labelParentProj";
			this.labelParentProj.ReadOnly = true;
			this.labelParentProj.Size = new System.Drawing.Size(241, 20);
			this.labelParentProj.TabIndex = 109;
			// 
			// labelRootProj
			// 
			this.labelRootProj.Location = new System.Drawing.Point(152, 188);
			this.labelRootProj.Name = "labelRootProj";
			this.labelRootProj.ReadOnly = true;
			this.labelRootProj.Size = new System.Drawing.Size(241, 20);
			this.labelRootProj.TabIndex = 110;
			// 
			// FormJobProjectEdit
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(574, 266);
			this.Controls.Add(this.labelRootProj);
			this.Controls.Add(this.labelParentProj);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.comboJobProjStatus);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textJobProjDescrip);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textJobProjName);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormJobProjectEdit";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Project";
			this.Load += new System.EventHandler(this.FormOperatoryEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormOperatoryEdit_Load(object sender, System.EventArgs e) {
			Array arrayJobProjectStatuses=Enum.GetValues(typeof(JobProjectStatus));
			foreach(JobProjectStatus status in arrayJobProjectStatuses) {//If a JobProjectStatus ever gets added, it will automatically get added here.
				comboJobProjStatus.Items.Add(status.ToString());
			}
			if(!_jobProjCur.IsNew) {
				comboJobProjStatus.SelectedIndex=(int)_jobProjCur.JobProjectStatus;
				textJobProjName.Text=_jobProjCur.Title;
				textJobProjDescrip.Text=_jobProjCur.Description;
				labelParentProj.Text=JobProjects.GetOne(_jobProjCur.ParentProjectNum).Title;
				labelRootProj.Text=JobProjects.GetOne(_jobProjCur.RootProjectNum).Title;
			}
			else {
				comboJobProjStatus.SelectedIndex=0;
			}
		}

		private bool CheckDoneChildren() {
			int jobCount=Jobs.GetForProject(_jobProjCur.JobProjectNum,false).Count;
			if(jobCount>0) {
				MsgBox.Show(this,"Can not delete project while there are incomplete jobs in it");
				return false;
			}
			int projCount=JobProjects.GetByParentProject(_jobProjCur.JobProjectNum,false).Count;
			if(projCount>0) {
				MsgBox.Show(this,"Can not delete project while there are incomplete projects in it");
				return false;
			}
			return true;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			_jobProjCur.JobProjectStatus=(JobProjectStatus)comboJobProjStatus.SelectedIndex;
			_jobProjCur.Title=textJobProjName.Text;
			_jobProjCur.Description=textJobProjDescrip.Text;
			if(_jobProjCur.JobProjectStatus==JobProjectStatus.Done && !CheckDoneChildren()) {
				return;
			}
			if(_jobProjCur.IsNew) {
				JobProjects.Insert(_jobProjCur);//Insert into list at appropriate spot
			}
			else {
				JobProjects.Update(_jobProjCur);//Probably should check if anything has changed
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(IsNew) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Are you sure you would like to delete this job?")) {
				return;
			}
			if(!CheckDoneChildren()) {
				return;
			}
			try {
				JobProjects.Delete(_jobProjCur.JobProjectNum);
			}
			catch(ApplicationException ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			DialogResult=DialogResult.OK;
		}

		

		

		


	}
}





















