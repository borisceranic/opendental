using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OpenDental {
	public partial class FormAutoNotePromptOneResp:ODForm {
		///<summary>Set this value externally.</summary>
		public string PromptText;
		///<summary>What the user picked.</summary>
		public string ResultText;
		///<summary>The string value representing the list to pick from.  One item per line.</summary>
		public string PromptOptions;

		public FormAutoNotePromptOneResp() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormAutoNotePromptOneResp_Load(object sender,EventArgs e) {
			Location=new Point(Left,Top+150);
			labelPrompt.Text=PromptText;
			string[] lines=PromptOptions.Split(new string[] {"\r\n"},StringSplitOptions.RemoveEmptyEntries);
			int stringWidthMax=120;//Minimum column width
			for(int i=0;i<lines.Length;i++) {
				int lineWidth=TextRenderer.MeasureText(lines[i],listMain.Font).Width;
				if(lineWidth>stringWidthMax) {
					stringWidthMax=lineWidth;
				}
				listMain.Items.Add(lines[i]);
			}
			if(stringWidthMax>listMain.Width-20) {//Give them some room
				listMain.MultiColumn=false;
			}
			else {
				listMain.ColumnWidth=stringWidthMax;
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(listMain.SelectedIndex==-1) {
				MsgBox.Show(this,"One response must be selected");
				return;
			}
			ResultText=listMain.SelectedItem.ToString();
			DialogResult=DialogResult.OK;
		}

		private void butSkip_Click(object sender,EventArgs e) {
			ResultText="";
			DialogResult=DialogResult.OK;
		}

		private void butPreview_Click(object sender,EventArgs e) {
			if(listMain.SelectedIndex==-1) {
				MsgBox.Show(this,"One response must be selected");
				return;
			}
			ResultText=listMain.SelectedItem.ToString();
			FormAutoNotePromptPreview FormP=new FormAutoNotePromptPreview();
			FormP.ResultText=ResultText;
			FormP.ShowDialog();
			if(FormP.DialogResult==DialogResult.OK) {
				ResultText=FormP.ResultText;
				DialogResult=DialogResult.OK;
			}
		}

		private void butCancel_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Abort autonote entry?")) {
				return;
			}
			DialogResult=DialogResult.Cancel;
		}

		

		

		
	}
}