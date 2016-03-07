using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormApptTypeEdit:ODForm {
		public AppointmentType AppointmentTypeCur;

		public FormApptTypeEdit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormApptTypeEdit_Load(object sender,EventArgs e) {
			textName.Text=AppointmentTypeCur.AppointmentTypeName;
			butColor.BackColor=AppointmentTypeCur.AppointmentTypeColor;
			checkIsHidden.Checked=AppointmentTypeCur.IsHidden;
		}

		private void butColor_Click(object sender,EventArgs e) {
			ColorDialog colorDialog1=new ColorDialog();
			colorDialog1.Color=butColor.BackColor;
			colorDialog1.ShowDialog();
			butColor.BackColor=colorDialog1.Color;
		}

		private void butColorClear_Click(object sender,EventArgs e) {
			butColor.BackColor=System.Drawing.Color.FromArgb(0);
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(AppointmentTypeCur.IsNew) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			else {
				AppointmentTypeCur=null;
				DialogResult=DialogResult.OK;
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			AppointmentTypeCur.AppointmentTypeName=textName.Text;
			AppointmentTypeCur.AppointmentTypeColor=butColor.BackColor;
			AppointmentTypeCur.IsHidden=checkIsHidden.Checked;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}