using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CodeBase {
	public static class ODUIExtentions {

		///<summary>Attempts to select the index provided for the combo box. If idx is -1 or otherwise invalid, then getOverrideText() will be used to get displayed text instead.</summary>
		/// <param name="combo">This is an extension method. This is the control that will be affected.</param>
		/// <param name="idx">Instead of providing an idx directly, use a function that determines the index to select. i.e. _listObj.FindIndex(x=>x.Num==SelectedNum).</param>
		/// <param name="getOverrideText">This should be a function that takes no parameters and returns a string. Linq statements and anonymous functions work best. 
		/// I.E. comboBox.SelectIndex(myCombo,_listObj.FindIndex(x=>x.Num==SelectedNum),</param>
		public static void IndexSelectOrSetText(this ComboBox combo,int idx,Func<string> getOverrideText) {
			combo.SelectedIndexChanged-=ComboSelectedIndex;//Reset visual style when user selects from dropdown.
			combo.KeyPress-=NullKeyPressHandler;//re-enable editing if neccesary
			combo.KeyDown-=NullKeyEventHandler;//re-enable editing if neccesary
			combo.KeyUp-=NullKeyEventHandler;//re-enable editing if neccesary
			//Normal index selection. No special logic needed.
			if(idx>-1 && idx<combo.Items.Count) {
				combo.DropDownStyle=ComboBoxStyle.DropDownList;
				combo.SelectedIndex=idx;
				return;
			}
			//Selected index is not part of the selected list, use the getOverrideText function provided.
			combo.SelectedIndexChanged+=ComboSelectedIndex;//Reset visual style when user selects from dropdown.
			combo.KeyPress+=NullKeyPressHandler;//disable editing.
			combo.KeyDown+=NullKeyEventHandler;//disable editing.
			combo.KeyUp+=NullKeyEventHandler;//disable editing.
			combo.SelectedIndex=-1;
			combo.DropDownStyle=ComboBoxStyle.DropDown;
			combo.Text=getOverrideText();
			if(string.IsNullOrEmpty(combo.Text)) {
				//In case the getOverrideText function returns an empty string
				combo.DropDownStyle=ComboBoxStyle.DropDownList;
			}
		}

		///<summary>Only use this overload if you already have a display string available. 
		///Use SelectIndex(int idx,Func&lt;string> getOverrideText) instead to delay execution of the delegate if it is not needed.</summary>
		public static void SelectIndex(this ComboBox combo,int idx,string OverrideText) {
			combo.IndexSelectOrSetText(idx,() => { return OverrideText; });
		}

		///<summary>Sets e.Handled to true to prevent event from firing.</summary>
		private static void NullKeyPressHandler(object sender,KeyPressEventArgs e) {
			e.Handled=true;
		}

		///<summary>Sets e.Handled to true to prevent event from firing.</summary>
		private static void NullKeyEventHandler(object sender,KeyEventArgs e) {
			e.Handled=true;
		}

		///<summary>Used to call comboBox.SelectIndex after user manually selects an item from the list. Should reset visual style to dropdown.</summary>
		private static void ComboSelectedIndex(object sender,EventArgs e) {
			ComboBox box = (ComboBox)sender;//capturing variable for readability
			box.IndexSelectOrSetText(box.SelectedIndex,() => { return box.Text; });
		}

	}
}
