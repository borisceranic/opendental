using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormSheetGridColDefEdit:Form {
		public SheetGridColDef ColDef;

		public FormSheetGridColDefEdit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSheetGridColDefEdit_Load(object sender,EventArgs e) {
			textNameInternal.Text=ColDef.ColName;
			textNameDisplay.Text=ColDef.DisplayName;
			textWidth.Text=ColDef.Width.ToString();
			for(int i=0;i<Enum.GetNames(typeof(StringAlignment)).Length;i++) {
				comboTextAlign.Items.Add(Enum.GetNames(typeof(StringAlignment))[i]);
				if((int)ColDef.TextAlign==i) {
					comboTextAlign.SelectedIndex=i;
				}
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			ColDef.DisplayName=textNameDisplay.Text;
			ColDef.Width=PIn.Int(textWidth.Text);
			ColDef.TextAlign=(StringAlignment)comboTextAlign.SelectedIndex;
			//do not save to DB here
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}