using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormSheetFieldGridType:Form {
		public SheetGridType SelectedSheetGridType;

		public FormSheetFieldGridType() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSheetFieldGrid_Load(object sender,EventArgs e) {
			fillComboGridTypes();
		}

		///<summary>Calling function should almost always call fillColumnNames() after this because the selected gridType may have just been changed.</summary>
		private void fillComboGridTypes() {
			comboGridType.Items.Clear();
			for(int i=0;i<Enum.GetValues(typeof(SheetGridType)).Length;i++) {
				comboGridType.Items.Add(SheetGridDefs.TypeToDisplay((SheetGridType)i));
			}
			comboGridType.SelectedIndex=0;
		}

		private void butOK_Click(object sender,EventArgs e) {
			SelectedSheetGridType=(SheetGridType)comboGridType.SelectedIndex;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}