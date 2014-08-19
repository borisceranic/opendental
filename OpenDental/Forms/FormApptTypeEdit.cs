using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormApptTypeEdit:Form {
		public AppointmentType AppointmentTypeCur;
		private AppointmentType _appointmentTypeOriginal;
		public bool IsNew;

		public FormApptTypeEdit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormApptTypeEdit_Load(object sender,EventArgs e) {
			_appointmentTypeOriginal=AppointmentTypeCur.Clone();
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
			if(IsNew) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			else {
				try {
					AppointmentTypes.Delete(AppointmentTypeCur.AppointmentTypeNum);
				}
				catch(Exception ex) {
					MessageBox.Show(ex.Message);
					return;
				}
				DataValid.SetInvalid(InvalidType.AppointmentTypes);
				DialogResult=DialogResult.OK;
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			AppointmentTypeCur.AppointmentTypeName=textName.Text;
			AppointmentTypeCur.AppointmentTypeColor=butColor.BackColor;
			AppointmentTypeCur.IsHidden=checkIsHidden.Checked;
			if(IsNew) {
				AppointmentTypes.Insert(AppointmentTypeCur);
			}
			else {
				AppointmentTypes.Update(AppointmentTypeCur);
			}
			DataValid.SetInvalid(InvalidType.AppointmentTypes);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}