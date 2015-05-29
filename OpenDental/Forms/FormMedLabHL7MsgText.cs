using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace OpenDental {
	public partial class FormMedLabHL7MsgText:Form {
		public List<string[]> ListFileNamesDatesMod; 

 		public FormMedLabHL7MsgText() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormMedLabHL7MsgText_Load(object sender,EventArgs e) {
			for(int i=0;i<ListFileNamesDatesMod.Count;i++) {
				string dateAndName="";
				if(ListFileNamesDatesMod[i][1]!="") {
					dateAndName+=ListFileNamesDatesMod[i][1];
				}
				dateAndName+="  -  "+ListFileNamesDatesMod[i][0];
				listFileNames.Items.Add(dateAndName);
			}
			listFileNames.SelectedIndex=0;
		}

		private void listFileNames_SelectedIndexChanged(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			textMain.Clear();
			string msgText="";
			try {
				msgText=File.ReadAllText(ListFileNamesDatesMod[listFileNames.SelectedIndex][0]);
			}
			catch(Exception ex) {
				Cursor=Cursors.Default;
				MsgBox.Show(this,"The selected file could not be read.");
				return;
			}
			textMain.Text=msgText;
			Cursor=Cursors.Default;
			return;
		}

		private void butClose_Click(object sender,EventArgs e) {
			this.Close();
		}
		
	}
}
