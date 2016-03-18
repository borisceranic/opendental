using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;
using System.Drawing.Text;
using System.ComponentModel;
using System.Linq;

namespace OpenDental {
	///<summary></summary>
	public delegate void ODtextEditorSaveEventHandler(object sender,EventArgs e);

	[DefaultEvent("SaveClick")]
	public partial class OdtextEditor:UserControl {
		private bool _hasSaveButton;
		private bool _isReadOnly;
		///<summary>Used to set the button color back after hovering</summary>
		private Color _backColor;
		///<summary>Used when highlighting.</summary>
		private Color _highlightColor;

		///<summary></summary>
		[Category("Appearance"),Description("Toggles whether the control contains a save button or not.")]
		public bool HasSaveButton {
			get {
				return _hasSaveButton;
			}
			set {
				_hasSaveButton=value;
				OdtextEditor_Layout(this,null);
				Invalidate();
			}
		}

		///<summary></summary>
		[Category("Appearance"),Description("Toggles whether the control can be edited.")]
		public bool ReadOnly {
			get {
				return _isReadOnly;
			}
			set {
				_isReadOnly=value;
				OdtextEditor_Layout(this,null);
				Invalidate();
			}
		}

		///<summary>Gets or sets the main textbox text.</summary>
		public string MainText {
			get {
				return textDescription.Text;
			}
			set {
				textDescription.Text=value;
				OdtextEditor_Layout(this,null);
				Invalidate();
			}
		}

		///<summary>Gets or sets the main textbox RTF format text.</summary>
		public string MainRtf {
			get {
				return textDescription.Rtf;
			}
			set {
				textDescription.Rtf=value;
				OdtextEditor_Layout(this,null);
				Invalidate();
			}
		}

		[Category("Action"),Description("Occurs when the save button is clicked.")]
		public event ODtextEditorSaveEventHandler SaveClick=null;

		public delegate void textChangedEventHandler();
		[Category("Action"),Description("Occurs as text is changed.")]
		public event textChangedEventHandler OnTextEdited=null;

		public OdtextEditor() {
			InitializeComponent();
			_hasSaveButton=true;
		}

		///<summary></summary>
		protected override void OnLoad(EventArgs e) {
			//Fill and set font
			InstalledFontCollection installedFonts=new InstalledFontCollection();
			installedFonts.Families.ToList().ForEach(x => comboFontType.Items.Add(x.Name));
			comboFontType.SelectedIndex=installedFonts.Families.ToList().FindIndex(x => x.Name.Contains("Microsoft Sans Serif"));			
			//Fill and set font size
			Enumerable.Range(7,14).ToList().ForEach(x => comboFontSize.Items.Add(x));//Sizes 7-20
			comboFontSize.SelectedIndex=1;//Size 8;
		}

    private void OdtextEditor_Layout(object sender,LayoutEventArgs e) {
			butSave.Visible=_hasSaveButton;
      textDescription.ReadOnly=_isReadOnly;
      bool isEnabled=!_isReadOnly;//Only here for readability of the following code.
      butCut.Enabled=isEnabled;
      butCopy.Enabled=isEnabled;
      butPaste.Enabled=isEnabled;
      butUndo.Enabled=isEnabled;
      butRedo.Enabled=isEnabled;
      butBold.Enabled=isEnabled;
      butItalics.Enabled=isEnabled;
      butUnderline.Enabled=isEnabled;
      butStrikeout.Enabled=isEnabled;
      butBullet.Enabled=isEnabled;
      comboFontSize.Enabled=isEnabled;
      comboFontType.Enabled=isEnabled;
      butColor.Enabled=isEnabled;
      butColorSelect.Enabled=isEnabled;
      butHighlight.Enabled=isEnabled;
      butHighlightSelect.Enabled=isEnabled;
      butSave.Enabled=isEnabled;
			butSpellCheck.Enabled=isEnabled;
			butClearFormatting.Enabled=isEnabled;
    }

		private void HoverColorEnter(object sender,EventArgs e) {
			System.Windows.Forms.Button btn=(System.Windows.Forms.Button)sender;
			_backColor=btn.BackColor;
			btn.BackColor=Color.PaleTurquoise;
		}

		private void HoverColorLeave(object sender,EventArgs e) {
			System.Windows.Forms.Button btn=(System.Windows.Forms.Button)sender;
			btn.BackColor=_backColor;
		}

		private void butCut_Click(object sender,EventArgs e) {
			Clipboard.SetText(textDescription.SelectedRtf);
			textDescription.SelectedRtf="";
		}

		private void butCopy_Click(object sender,EventArgs e) {
			Clipboard.SetText(textDescription.SelectedRtf);
		}

		private void butPaste_Click(object sender,EventArgs e) {
			textDescription.SelectedRtf=Clipboard.GetText();
		}

		private void butUndo_Click(object sender,EventArgs e) {
			textDescription.Undo();
		}

		private void butRedo_Click(object sender,EventArgs e) {
			textDescription.Redo();
		}

		private void butBold_Click(object sender,EventArgs e) {
			try {
				textDescription.SelectionFont=new Font(textDescription.SelectionFont,textDescription.SelectionFont.Style ^ FontStyle.Bold);
			}
			catch {
				//labelWarning.Text="Cannot format multiple Fonts";
			}
		}

		private void butItalics_Click(object sender,EventArgs e) {
			try {
				textDescription.SelectionFont=new Font(textDescription.SelectionFont,textDescription.SelectionFont.Style ^ FontStyle.Italic);
			}
			catch {
				//labelWarning.Text="Cannot format multiple Fonts";
			}
		}

		private void butUnderline_Click(object sender,EventArgs e) {
			try {
				textDescription.SelectionFont=new Font(textDescription.SelectionFont,textDescription.SelectionFont.Style ^ FontStyle.Underline);
			}
			catch {
				//labelWarning.Text="Cannot format multiple Fonts";
			}
		}

		private void butStrikeout_Click(object sender,EventArgs e) {
			try {
				textDescription.SelectionFont=new Font(textDescription.SelectionFont,textDescription.SelectionFont.Style ^ FontStyle.Strikeout);
			}
			catch {
				//labelWarning.Text="Cannot format multiple Fonts";
			}
		}

		private void butBullet_Click(object sender,EventArgs e) {
			if(textDescription.SelectionBullet) {
				textDescription.SelectionBullet=false;
			}
			else {
				textDescription.SelectionBullet=true;
			}
		}

		private void comboFontType_SelectionChangeCommitted(object sender,EventArgs e) {
			try {
				textDescription.SelectionFont=new Font((string)comboFontType.SelectedItem,(int)comboFontSize.SelectedItem,textDescription.SelectionFont.Style);
			}
			catch {

			}
		}

		private void comboFontSize_SelectionChangeCommitted(object sender,EventArgs e) {
			try {
				textDescription.SelectionFont=new Font((string)comboFontType.SelectedItem,(int)comboFontSize.SelectedItem,textDescription.SelectionFont.Style);
			}
			catch {

			}
		}

		private void butColor_Click(object sender,EventArgs e) {
			textDescription.SelectionColor=butColor.ForeColor;
		}

		private void butColorSelect_Click(object sender,EventArgs e) {
			colorDialog1.Color=butColor.ForeColor;
			colorDialog1.ShowDialog();
			butColor.ForeColor=colorDialog1.Color;
		}

		private void butHighlight_Click(object sender,EventArgs e) {
			textDescription.SelectionBackColor=_highlightColor;
		}

		private void butHighlightSelect_Click(object sender,EventArgs e) {
			colorDialog1.Color=butColor.ForeColor;
			colorDialog1.ShowDialog();
			butHighlight.BackColor=colorDialog1.Color;
			_highlightColor=colorDialog1.Color;
		}

		///<summary></summary>
		private void butSave_Click(object sender,EventArgs e) {
			EventArgs gArgs=new EventArgs();
			if(SaveClick!=null) {
				SaveClick(this,gArgs);
			}
		}

		private void textDescription_TextChanged(object sender,EventArgs e) {
			if(OnTextEdited!=null) {
				OnTextEdited();
			}
		}

		private void butSpellCheck_Click(object sender,EventArgs e) {
			FormSpellChecker FormSC = new FormSpellChecker();
			FormSC.SetText(textDescription.Rtf);
			FormSC.ShowDialog();
			if(FormSC.DialogResult!=DialogResult.OK) {
				return;
			}
			textDescription.Rtf=FormSC.TextRtf;
		}

		private void butClearFormatting_Click(object sender,EventArgs e) {
			try {
				textDescription.SelectionFont=new Font((string)comboFontType.SelectedItem,(int)comboFontSize.SelectedItem);
				textDescription.SelectionBullet=false;
				textDescription.SelectionColor=Color.Black;
				textDescription.SelectionBackColor=Color.Transparent;
			}
			catch {

			}
		}
	}

}
