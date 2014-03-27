using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary></summary>
	public class FormDisplayFieldOrthoEdit : System.Windows.Forms.Form{
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private TextBox textDescription;
		private Label label2;
		private Label label3;
		private ValidNum textWidth;
		private TextBox textWidthMin;
		private Label label4;
		private Label label5;
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.Container components = null;
		public DisplayField FieldCur;
		private Label labelWarning;
		private TextBox textPickList;
		private ComboBox comboFieldType;
		private Label label1;
		private Font headerFont=new Font(FontFamily.GenericSansSerif,8.5f,FontStyle.Bold);

		///<summary>Currently not referenced anywhere.  Form to be implemented for Ortho Chart Cell Dropdown Feature.  This form is very similar to form DisplayFieldEdit.  Commented many lines out for a save commit.</summary>
		public FormDisplayFieldOrthoEdit()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDisplayFieldOrthoEdit));
			this.textDescription = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textWidthMin = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.labelWarning = new System.Windows.Forms.Label();
			this.textPickList = new System.Windows.Forms.TextBox();
			this.comboFieldType = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textWidth = new OpenDental.ValidNum();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// textDescription
			// 
			this.textDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textDescription.Location = new System.Drawing.Point(141, 15);
			this.textDescription.Name = "textDescription";
			this.textDescription.Size = new System.Drawing.Size(249, 20);
			this.textDescription.TabIndex = 5;
			this.textDescription.TextChanged += new System.EventHandler(this.textDescription_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(-1, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(140, 17);
			this.label2.TabIndex = 4;
			this.label2.Text = "Description";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(-1, 68);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(140, 17);
			this.label3.TabIndex = 6;
			this.label3.Text = "Column Width";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textWidthMin
			// 
			this.textWidthMin.Location = new System.Drawing.Point(141, 41);
			this.textWidthMin.Name = "textWidthMin";
			this.textWidthMin.ReadOnly = true;
			this.textWidthMin.Size = new System.Drawing.Size(71, 20);
			this.textWidthMin.TabIndex = 9;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(2, 42);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(137, 17);
			this.label4.TabIndex = 8;
			this.label4.Text = "Minimum Width";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(215, 42);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(175, 17);
			this.label5.TabIndex = 10;
			this.label5.Text = "(based on text above)";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelWarning
			// 
			this.labelWarning.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelWarning.Location = new System.Drawing.Point(324, 96);
			this.labelWarning.Name = "labelWarning";
			this.labelWarning.Size = new System.Drawing.Size(101, 14);
			this.labelWarning.TabIndex = 89;
			this.labelWarning.Text = "One Per Line";
			this.labelWarning.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.labelWarning.Visible = false;
			// 
			// textPickList
			// 
			this.textPickList.AcceptsReturn = true;
			this.textPickList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textPickList.Location = new System.Drawing.Point(141, 122);
			this.textPickList.Multiline = true;
			this.textPickList.Name = "textPickList";
			this.textPickList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textPickList.Size = new System.Drawing.Size(249, 301);
			this.textPickList.TabIndex = 88;
			// 
			// comboFieldType
			// 
			this.comboFieldType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFieldType.DropDownWidth = 177;
			this.comboFieldType.FormattingEnabled = true;
			this.comboFieldType.Location = new System.Drawing.Point(141, 93);
			this.comboFieldType.MaxDropDownItems = 30;
			this.comboFieldType.Name = "comboFieldType";
			this.comboFieldType.Size = new System.Drawing.Size(177, 21);
			this.comboFieldType.TabIndex = 86;
			this.comboFieldType.SelectedIndexChanged += new System.EventHandler(this.comboFieldType_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(-1, 94);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(140, 17);
			this.label1.TabIndex = 90;
			this.label1.Text = "Field Type";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textWidth
			// 
			this.textWidth.Location = new System.Drawing.Point(141, 67);
			this.textWidth.MaxVal = 2000;
			this.textWidth.MinVal = 1;
			this.textWidth.Name = "textWidth";
			this.textWidth.Size = new System.Drawing.Size(71, 20);
			this.textWidth.TabIndex = 7;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(414, 365);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 1;
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
			this.butCancel.Location = new System.Drawing.Point(414, 397);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormDisplayFieldOrthoEdit
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(510, 448);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.labelWarning);
			this.Controls.Add(this.textPickList);
			this.Controls.Add(this.comboFieldType);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textWidthMin);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textWidth);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormDisplayFieldOrthoEdit";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Ortho Display Field";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDisplayFieldOrthoEdit_FormClosing);
			this.Load += new System.EventHandler(this.FormDisplayFieldOrthoEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormDisplayFieldOrthoEdit_Load(object sender,EventArgs e) {
			//textDescription.Text=FieldCur.Description;
			//textWidth.Text=FieldCur.ColumnWidth.ToString();
			//FillWidth();
			//textPickList.Visible=false;
			//labelWarning.Visible=false;
			//comboFieldType.Items.Clear();
			//comboFieldType.Items.AddRange(Enum.GetNames(typeof(DisplayFieldType)));
			//comboFieldType.SelectedIndex=(int)FieldCur.FieldType;
			//if(comboFieldType.SelectedIndex==(int)DisplayFieldType.PickList) {
			//	textPickList.Visible=true;
			//	labelWarning.Visible=true;
			//	textPickList.Text=FieldCur.PickList;
			//}
		}

		private void FillWidth(){
			Graphics g=this.CreateGraphics();
			int width;
				width=(int)g.MeasureString(textDescription.Text,headerFont).Width;
			textWidthMin.Text=width.ToString();
			g.Dispose();
		}

		private void textDescription_TextChanged(object sender,EventArgs e) {
			FillWidth();
		}

		private void comboFieldType_SelectedIndexChanged(object sender,EventArgs e) {
			//if(comboFieldType.SelectedIndex==(int)DisplayFieldType.PickList) {
			//	textPickList.Visible=true;
			//	labelWarning.Visible=true;
			//}
			//else {
			//	textPickList.Visible=false;
			//	labelWarning.Visible=false;
			//}
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			//if(textWidth.errorProvider1.GetError(textWidth)!="") {
			//	MsgBox.Show(this,"Please fix data entry errors first.");
			//	return;
			//}
			//if(FieldCur.FieldType==DisplayFieldType.PickList) {
			//	if(textPickList.Text=="") {
			//		MsgBox.Show(this,"List cannot be blank.");
			//		return;
			//	}
			//	FieldCur.PickList=textPickList.Text;
			//}
			//FieldCur.FieldType=(DisplayFieldType)comboFieldType.SelectedIndex;
			//FieldCur.Description=textDescription.Text;
			//FieldCur.ColumnWidth=PIn.Int(textWidth.Text);
			//DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormDisplayFieldOrthoEdit_FormClosing(object sender,FormClosingEventArgs e) {

		}

		

		


	}
}





















