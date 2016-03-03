using System;
using System.Drawing;
using System.Collections.Generic;
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
		public List<DisplayField> ListShowing;
		private Label labelLine;
		private TextBox textPickList;
		private UI.Button butDown;
		private UI.Button butUp;
		private CheckBox checkSignature;
		private Font headerFont=new Font(FontFamily.GenericSansSerif,8.5f,FontStyle.Bold);

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
			this.labelLine = new System.Windows.Forms.Label();
			this.textPickList = new System.Windows.Forms.TextBox();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.textWidth = new OpenDental.ValidNum();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.checkSignature = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// textDescription
			// 
			this.textDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textDescription.Location = new System.Drawing.Point(142, 30);
			this.textDescription.Name = "textDescription";
			this.textDescription.Size = new System.Drawing.Size(249, 20);
			this.textDescription.TabIndex = 5;
			this.textDescription.TextChanged += new System.EventHandler(this.textDescription_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 31);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(134, 17);
			this.label2.TabIndex = 4;
			this.label2.Text = "Description";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(6, 83);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(134, 17);
			this.label3.TabIndex = 6;
			this.label3.Text = "Column Width";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textWidthMin
			// 
			this.textWidthMin.Location = new System.Drawing.Point(142, 56);
			this.textWidthMin.Name = "textWidthMin";
			this.textWidthMin.ReadOnly = true;
			this.textWidthMin.Size = new System.Drawing.Size(71, 20);
			this.textWidthMin.TabIndex = 9;
			this.textWidthMin.TabStop = false;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(9, 57);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(131, 17);
			this.label4.TabIndex = 8;
			this.label4.Text = "Minimum Width";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(216, 57);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(175, 17);
			this.label5.TabIndex = 10;
			this.label5.Text = "(based on text above)";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelLine
			// 
			this.labelLine.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelLine.Location = new System.Drawing.Point(6, 130);
			this.labelLine.Name = "labelLine";
			this.labelLine.Size = new System.Drawing.Size(130, 14);
			this.labelLine.TabIndex = 89;
			this.labelLine.Text = "One Entry Per Line";
			this.labelLine.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// textPickList
			// 
			this.textPickList.AcceptsReturn = true;
			this.textPickList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textPickList.HideSelection = false;
			this.textPickList.Location = new System.Drawing.Point(142, 130);
			this.textPickList.Multiline = true;
			this.textPickList.Name = "textPickList";
			this.textPickList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textPickList.Size = new System.Drawing.Size(249, 260);
			this.textPickList.TabIndex = 20;
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(1, 0);
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.Location = new System.Drawing.Point(445, 164);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(25, 24);
			this.butDown.TabIndex = 30;
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(1, 0);
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.Location = new System.Drawing.Point(414, 164);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(25, 24);
			this.butUp.TabIndex = 25;
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// textWidth
			// 
			this.textWidth.Location = new System.Drawing.Point(142, 82);
			this.textWidth.MaxVal = 2000;
			this.textWidth.MinVal = 1;
			this.textWidth.Name = "textWidth";
			this.textWidth.Size = new System.Drawing.Size(71, 20);
			this.textWidth.TabIndex = 10;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(414, 329);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 35;
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
			this.butCancel.Location = new System.Drawing.Point(414, 364);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 40;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// checkSignature
			// 
			this.checkSignature.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSignature.Location = new System.Drawing.Point(142, 106);
			this.checkSignature.Name = "checkSignature";
			this.checkSignature.Size = new System.Drawing.Size(328, 18);
			this.checkSignature.TabIndex = 91;
			this.checkSignature.TabStop = false;
			this.checkSignature.Text = "Check to show a signature box in the Ortho Chart.";
			this.checkSignature.CheckedChanged += new System.EventHandler(this.checkSignature_CheckedChanged);
			// 
			// FormDisplayFieldOrthoEdit
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(510, 402);
			this.Controls.Add(this.checkSignature);
			this.Controls.Add(this.butDown);
			this.Controls.Add(this.butUp);
			this.Controls.Add(this.labelLine);
			this.Controls.Add(this.textPickList);
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
			this.Load += new System.EventHandler(this.FormDisplayFieldOrthoEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormDisplayFieldOrthoEdit_Load(object sender,EventArgs e) {
			textDescription.Text=FieldCur.Description;
			textWidth.Text=FieldCur.ColumnWidth.ToString();
			textPickList.Text=FieldCur.PickList;
			if(FieldCur.InternalName=="Signature") {
				checkSignature.Checked=true;
				labelLine.Visible=false;
				textPickList.Visible=false;
				butUp.Visible=false;
				butDown.Visible=false;
			}
			FillWidth();
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

		private void butUp_Click(object sender,EventArgs e) {
			if(textPickList.Text==""){
				return;
			}
			int selectionStart=textPickList.SelectionStart;
			//calculate which row to highlight, based on selection start.
			int selectedRow=0;
			int sumPreviousLines=0;
			string[] linesOrig=new string[textPickList.Lines.Length];
			textPickList.Lines.CopyTo(linesOrig,0);
			for(int i=0;i<textPickList.Lines.Length;i++) {
				if(i>0) {
					sumPreviousLines+=textPickList.Lines[i-1].Length+2;//the 2 is for \r\n
				}
				if(selectionStart < sumPreviousLines+textPickList.Lines[i].Length) {
					selectedRow=i;
					break;
				}
			}
			//swap rows
			int newSelectedRow;
			if(selectedRow==0) {
				newSelectedRow=0;//and no swap
			}
			else {
				//doesn't allow me to directly set lines, so:
				string newtext="";
				for(int i=0;i<textPickList.Lines.Length;i++) {
					if(i>0) {
						newtext+="\r\n";
					}
					if(i==selectedRow) {
						newtext+=linesOrig[selectedRow-1];
					}
					else if(i==selectedRow-1) {
						newtext+=linesOrig[selectedRow];
					}
					else {
						newtext+=linesOrig[i];
					}
				}
				textPickList.Text=newtext;
				newSelectedRow=selectedRow-1;
			}
			//highlight the newSelectedRow
			sumPreviousLines=0;
			for(int i=0;i<textPickList.Lines.Length;i++) {
				if(i>0) {
					sumPreviousLines+=textPickList.Lines[i-1].Length+2;//the 2 is for \r\n
				}
				if(newSelectedRow==i) {
					textPickList.Select(sumPreviousLines,textPickList.Lines[i].Length);
					break;
				}
			}
		}

		private void butDown_Click(object sender,EventArgs e) {
			if(textPickList.Text=="") {
				return;
			}
			int selectionStart=textPickList.SelectionStart;
			//calculate which row to highlight, based on selection start.
			int selectedRow=0;
			int sumPreviousLines=0;
			string[] linesOrig=new string[textPickList.Lines.Length];
			textPickList.Lines.CopyTo(linesOrig,0);
			for(int i=0;i<textPickList.Lines.Length;i++) {
				if(i>0) {
					sumPreviousLines+=textPickList.Lines[i-1].Length+2;//the 2 is for \r\n
				}
				if(selectionStart < sumPreviousLines+textPickList.Lines[i].Length) {
					selectedRow=i;
					break;
				}
			}
			//swap rows
			int newSelectedRow;
			if(selectedRow==textPickList.Lines.Length-1) {
				newSelectedRow=textPickList.Lines.Length-1;//and no swap
			}
			else {
				//doesn't allow me to directly set lines, so:
				string newtext="";
				for(int i=0;i<textPickList.Lines.Length;i++) {
					if(i>0) {
						newtext+="\r\n";
					}
					if(i==selectedRow) {
						newtext+=linesOrig[selectedRow+1];
					}
					else if(i==selectedRow+1) {
						newtext+=linesOrig[selectedRow];
					}
					else {
						newtext+=linesOrig[i];
					}
				}
				textPickList.Text=newtext;
				newSelectedRow=selectedRow+1;
			}
			//highlight the newSelectedRow
			sumPreviousLines=0;
			for(int i=0;i<textPickList.Lines.Length;i++) {
				if(i>0) {
					sumPreviousLines+=textPickList.Lines[i-1].Length+2;//the 2 is for \r\n
				}
				if(newSelectedRow==i) {
					textPickList.Select(sumPreviousLines,textPickList.Lines[i].Length);
					break;
				}
			}
		}

		private void checkSignature_CheckedChanged(object sender,EventArgs e) {
			if(checkSignature.Checked && textPickList.Text != "") {
				MsgBox.Show(this,"To make this display field a signature field, remove the pick list values first.");
				checkSignature.Checked=false;
				return;
			}
			labelLine.Visible=(!checkSignature.Checked);
			textPickList.Visible=(!checkSignature.Checked);
			butUp.Visible=(!checkSignature.Checked);
			butDown.Visible=(!checkSignature.Checked);
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textWidth.errorProvider1.GetError(textWidth)!="") {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			FieldCur.Description=textDescription.Text;
			FieldCur.ColumnWidth=PIn.Int(textWidth.Text);
			FieldCur.PickList=textPickList.Text;
			if(checkSignature.Checked) {
				FieldCur.InternalName="Signature";
			}
			else {
				FieldCur.InternalName="";
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}





















