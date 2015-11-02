using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormLanguageAndRegion:Form {
		private List<CultureInfo> _listAllCultures;

		public FormLanguageAndRegion() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormLanguageAndRegion_Load(object sender,EventArgs e) {
			CultureInfo cultureCur=PrefC.GetLanguageAndRegion();
			_listAllCultures=CultureInfo.GetCultures(CultureTypes.AllCultures).Where(x => !x.IsNeutralCulture).OrderBy(x => x.DisplayName).ToList();
			if(PrefC.GetString(PrefName.LanguageAndRegion)=="") {
				textLanguageAndRegion.Text="None";
			}
			else {
				textLanguageAndRegion.Text=cultureCur.DisplayName;
			}
			comboLanguageAndRegion.Items.Clear();
			_listAllCultures.ForEach(x => comboLanguageAndRegion.Items.Add(x.DisplayName));
			comboLanguageAndRegion.SelectedIndex=_listAllCultures.FindIndex(x => x.DisplayName==cultureCur.DisplayName);
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(comboLanguageAndRegion.SelectedIndex==-1) {
				MsgBox.Show(this,"Select a language and region.");
				return;
			}
			//_cultureCur=_listAllCultures[comboLanguageAndRegion.SelectedIndex];
			if(Prefs.UpdateString(PrefName.LanguageAndRegion,_listAllCultures[comboLanguageAndRegion.SelectedIndex].Name)) {
				MsgBox.Show(this,"Program must be restarted for changes to take full effect.");
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}