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
			base.OnLoad(e);
			InstalledFontCollection installedFonts=new InstalledFontCollection();
			foreach(FontFamily font in installedFonts.Families) {
				comboFontType.Items.Add(font.Name);
				if(font.Name.Contains("Microsoft Sans Serif")) {
					comboFontType.SelectedIndex=comboFontType.Items.Count-1;
				}
			}
			//Sizes 7-20
			for(int i=7;i<21;i++) {
				comboFontSize.Items.Add(i);
			}
			comboFontSize.SelectedIndex=1;//Size 8;
			textDescription.ContextMenu.MenuItems.Clear();
			MenuItem menuItem;
			textDescription.ContextMenu.MenuItems.Add("",menuItem_Click);//These five menu items will hold the suggested spelling for misspelled words.  If no misspelled words, they will not be visible.
			textDescription.ContextMenu.MenuItems.Add("",menuItem_Click);
			textDescription.ContextMenu.MenuItems.Add("",menuItem_Click);
			textDescription.ContextMenu.MenuItems.Add("",menuItem_Click);
			textDescription.ContextMenu.MenuItems.Add("",menuItem_Click);
			textDescription.ContextMenu.MenuItems.Add("-");
			textDescription.ContextMenu.MenuItems.Add("Add to Dictionary",menuItem_Click);
			textDescription.ContextMenu.MenuItems.Add("Disable Spell Check",menuItem_Click);
			textDescription.ContextMenu.MenuItems.Add("-");
			menuItem=new MenuItem(Lan.g(this,"Insert Date"),menuItem_Click,Shortcut.CtrlD);
			textDescription.ContextMenu.MenuItems.Add(menuItem);
			menuItem=new MenuItem(Lan.g(this,"Insert Quick Note"),menuItem_Click,Shortcut.CtrlQ);
			textDescription.ContextMenu.MenuItems.Add(menuItem);
			textDescription.ContextMenu.MenuItems.Add("-");
			menuItem=new MenuItem(Lan.g(this,"Cut"),menuItem_Click,Shortcut.CtrlX);
			textDescription.ContextMenu.MenuItems.Add(menuItem);
			menuItem=new MenuItem(Lan.g(this,"Copy"),menuItem_Click,Shortcut.CtrlC);
			textDescription.ContextMenu.MenuItems.Add(menuItem);
			menuItem=new MenuItem(Lan.g(this,"Paste"),menuItem_Click,Shortcut.CtrlV);
			textDescription.ContextMenu.MenuItems.Add(menuItem);
		}

		private void OdtextEditor_Layout(object sender,LayoutEventArgs e) {
			if(!_hasSaveButton) {
				butSave.Visible=false;
			}
			if(_isReadOnly) {
				textDescription.ReadOnly=true;
				butCut.Enabled=false;
				butCopy.Enabled=false;
				butPaste.Enabled=false;
				butUndo.Enabled=false;
				butRedo.Enabled=false;
				butBold.Enabled=false;
				butItalics.Enabled=false;
				butUnderline.Enabled=false;
				butStrikeout.Enabled=false;
				butBullet.Enabled=false;
				comboFontSize.Enabled=false;
				comboFontType.Enabled=false;
				butColor.Enabled=false;
				butColorSelect.Enabled=false;
				butHighlight.Enabled=false;
				butHighlightSelect.Enabled=false;
				butSave.Enabled=false;
				textDescription.SpellCheckIsEnabled=false;
			}
			else {
				textDescription.ReadOnly=false;
				foreach(Control c in this.Controls) {
					c.Enabled=true;
				}
			}
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

		private void menuItem_Click(object sender,System.EventArgs e) {
			if(ReadOnly && textDescription.ContextMenu.MenuItems.IndexOf((MenuItem)sender)!=13) {
				MsgBox.Show(this,"This feature is currently disabled due to this text box being read only.");
				return;
			}
			switch(textDescription.ContextMenu.MenuItems.IndexOf((MenuItem)sender)) {
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
					if(!textDescription.SpellCheckIsEnabled || !PrefC.GetBool(PrefName.SpellCheckIsEnabled)) {//if spell check disabled, break.  Should never happen since the suggested words won't show if spell check disabled
						break;
					}
					textDescription.SelectionStart=textDescription.ReplWord.StartIndex;
					textDescription.SelectionLength=textDescription.ReplWord.Value.Length;
					textDescription.SelectedText=textDescription.ContextMenu.MenuItems[textDescription.ContextMenu.MenuItems.IndexOf((MenuItem)sender)].Text;
					textDescription.timerSpellCheck.Start();
					break;
				//case 5 is separator
				case 6://Add to dict
					if(!textDescription.SpellCheckIsEnabled || !PrefC.GetBool(PrefName.SpellCheckIsEnabled)) {//if spell check disabled, break.  Should never happen since Add to Dict won't show if spell check disabled
						break;
					}
					string newWord=textDescription.ReplWord.Value;
					//guaranteed to not already exist in custom dictionary, or it wouldn't be underlined.
					DictCustom word=new DictCustom();
					word.WordText=newWord;
					DictCustoms.Insert(word);
					DataValid.SetInvalid(InvalidType.DictCustoms);
					textDescription.ListIncorrect.Remove(textDescription.ReplWord.Value);
					textDescription.ListCorrect.Add(textDescription.ReplWord.Value);
					textDescription.timerSpellCheck.Start();
					break;
				case 7://Disable spell check
					if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This will disable spell checking.  To re-enable, go to Setup | Spell Check and check the \"Spell Check Enabled\" box.")) {
						break;
					}
					Prefs.UpdateBool(PrefName.SpellCheckIsEnabled,false);
					DataValid.SetInvalid(InvalidType.Prefs);
					textDescription.ClearWavyLines();
					break;
				//case 8 is separator
				case 9://Insert Date
					textDescription.SelectionLength=0;
					string strPaste=DateTime.Today.ToShortDateString();
					textDescription.SelectedText=strPaste;
					break;
				case 10://Insert Quick Note
					//ShowFullDialog();
					break;
				//case 11 is separator
				case 12://cut
					textDescription.Cut();
					break;
				case 13://copy
					textDescription.Copy();
					break;
				case 14://paste
					textDescription.Paste();
					break;
			}
		}

		///<summary></summary>
		protected override void OnKeyDown(KeyEventArgs e) {
			string textRtf=textDescription.Rtf;
			base.OnKeyUp(e);
			textDescription.Rtf=textRtf;
			int originalLength=textDescription.TextLength;
			int originalCaret=textDescription.SelectionStart;
			string newText=QuickPasteNotes.Substitute(Text,textDescription.QuickPasteType);
			if(textDescription.Text!=newText) {
				Clipboard.SetText(newText);
				textDescription.Paste();
			}
			if(e.KeyData == (Keys.Control | Keys.V)) {
				textDescription.Paste();
			}
			else {
				Message msg=new Message();
				base.ProcessCmdKey(ref msg,e.KeyData);
			}
		}

		private void textDescription_TextChanged(object sender,EventArgs e) {
			if(OnTextEdited!=null) {
				OnTextEdited();
			}
		}

	}

}
