using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace OpenDental {
	public partial class SheetComboBox:Control {
		private bool _isHovering;
		private PathGradientBrush _hoverBrush;
		private Color _surroundColor;
		public string SelectedOption;
		private string[] _arrayComboOptions;
		///<summary>A default option for the combo box to display.  Required to be set for screening tooth chart combo boxes.  E.g. "ling", "b".</summary>
		public string DefaultOption;
		public bool IsToothChart;
		private ContextMenu _contextMenu=new ContextMenu();

		[Category("Layout"),Description("Set true if this is a toothchart combo.")]
		public bool ToothChart {get { return IsToothChart; } set { IsToothChart=value; } }

		public string[] ComboOptions {
			get {
				return _arrayComboOptions;
			}
		}

		///<summary>Currently fills the combo box with the default options for combo boxes on screen charts.</summary>
		public SheetComboBox() : this(";None|S|PS|C|F|NFE|NN") {
		}

		public SheetComboBox(string values) {
			InitializeComponent();
			SelectedOption=values.Split(';')[0];
			_arrayComboOptions=values.Split(';')[1].Split('|');//Will have one empty entry if combo has no options.
			if(SelectedOption=="") {
				SelectedOption=_arrayComboOptions[0];//Select first option if there is one and one wasn't selected previously (this is new form).
			}
			foreach(string option in _arrayComboOptions) {
				_contextMenu.MenuItems.Add(new MenuItem(option,menuItemContext_Click));
			}
			SetBrushes();
		}

		private void menuItemContext_Click(object sender,EventArgs e) {
			if(sender.GetType()!=typeof(MenuItem)) {
				return;
			}
			SelectedOption=_arrayComboOptions[_contextMenu.MenuItems.IndexOf((MenuItem)sender)];
		}

		private void SheetComboBox_MouseDown(object sender,MouseEventArgs e) {
			_contextMenu.Show(this,new Point(0,Height));//Can't resize width, it's done according to width of items.
		}

		private void SetBrushes(){
			_hoverBrush=new PathGradientBrush(
				new Point[] {new Point(0,0),new Point(Width-1,0),new Point(Width-1,Height-1),new Point(0,Height-1)});
			_hoverBrush.CenterColor=Color.White;
			_surroundColor=Color.FromArgb(245,234,200);
			_hoverBrush.SurroundColors=new Color[] {_surroundColor,_surroundColor,_surroundColor,_surroundColor};
			Blend blend=new Blend();
			float[] myFactors = {0f,.5f,1f,1f,1f,1f};
			float[] myPositions = {0f,.2f,.4f,.6f,.8f,1f};
			blend.Factors=myFactors;
			blend.Positions=myPositions;
			_hoverBrush.Blend=blend;
		}

		protected override void OnPaint(PaintEventArgs pe) {
			base.OnPaint(pe);
			using(Graphics g=pe.Graphics) {
				g.SmoothingMode=SmoothingMode.HighQuality;
				g.CompositingQuality=CompositingQuality.HighQuality;
				g.FillRectangle(Brushes.White,0,0,Width,Height);//White background
				if(_isHovering) {
					g.FillRectangle(_hoverBrush,0,0,Width-1,Height-1);
					g.DrawRectangle(new Pen(_surroundColor),0,0,Width-1,Height-1);
				}
				g.DrawRectangle(Pens.Black,-1,-1,Width,Height);//Outline
				StringFormat stringFormat=new StringFormat();
				stringFormat.Alignment=StringAlignment.Center;
				stringFormat.LineAlignment=StringAlignment.Center;
				Brush brush=Brushes.Black;//Default to black.  Do not dispose brush because that will dispose of Brushes.Black... not good.
				if(ToothChart) {
					if(SelectedOption=="buc" || SelectedOption=="ling" || SelectedOption=="d" || SelectedOption=="m" || SelectedOption=="None") {
						brush=Brushes.LightGray;
					}
					if(SelectedOption=="None") {
						SelectedOption=DefaultOption;//Nothing has been selected so draw the "default" string in the combo box.  E.g. "b", "ling", etc.
					}
				}
				g.DrawString(SelectedOption,new Font(FontFamily.GenericSansSerif,IsToothChart ? 10f : this.Height-10)
					,brush,new Point(this.Width/2,this.Height/2),stringFormat);
			}
		}

		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);
			if(!_isHovering) {
				_isHovering=true;
				Invalidate();
			}
		}

		protected override void OnMouseLeave(EventArgs e) {
			base.OnMouseLeave(e);
			_isHovering=false;
			Invalidate();
		}

	}
}
