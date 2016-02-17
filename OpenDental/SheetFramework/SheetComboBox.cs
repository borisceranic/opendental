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
		private Pen pen;
		private bool isHovering;
		private PathGradientBrush hoverBrush;
		private Color surroundColor;
		public string SelectedOption;
		private string[] _arrayComboOptions;
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

		public SheetComboBox() {
			InitializeComponent();
			//_selectedOption="";
			_arrayComboOptions=new string[7];
			_arrayComboOptions[0]="None";
			_arrayComboOptions[1]="S";
			_arrayComboOptions[2]="PS";
			_arrayComboOptions[3]="C";
			_arrayComboOptions[4]="F";
			_arrayComboOptions[5]="NFE";
			_arrayComboOptions[6]="NN";
			foreach(string option in _arrayComboOptions) {
				_contextMenu.MenuItems.Add(new MenuItem(option,menuItemContext_Click));
			}
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

		protected override void OnSizeChanged(EventArgs e) {
			base.OnSizeChanged(e);
			SetBrushes();
		}

		private void SheetComboBox_MouseDown(object sender,MouseEventArgs e) {
			_contextMenu.Show(this,new Point(0,Height));//Can't resize width, it's done according to width of items.
		}

		private void SetBrushes(){
			pen=new Pen(Color.Black,1.6f);
			hoverBrush=new PathGradientBrush(
				new Point[] {new Point(0,0),new Point(Width-1,0),new Point(Width-1,Height-1),new Point(0,Height-1)});
			hoverBrush.CenterColor=Color.White;
			surroundColor=Color.FromArgb(245,234,200);
			hoverBrush.SurroundColors=new Color[] {surroundColor,surroundColor,surroundColor,surroundColor};
			Blend blend=new Blend();
			float[] myFactors = {0f,.5f,1f,1f,1f,1f};
			float[] myPositions = {0f,.2f,.4f,.6f,.8f,1f};
			blend.Factors=myFactors;
			blend.Positions=myPositions;
			hoverBrush.Blend=blend;
		}

		protected override void OnPaint(PaintEventArgs pe) {
			base.OnPaint(pe);
			Graphics g=pe.Graphics;
			g.SmoothingMode=SmoothingMode.HighQuality;
			g.CompositingQuality=CompositingQuality.HighQuality;
			g.FillRectangle(Brushes.White,0,0,Width,Height);//White background
			if(isHovering){
				g.FillRectangle(hoverBrush,0,0,Width-1,Height-1);
				g.DrawRectangle(new Pen(surroundColor),0,0,Width-1,Height-1);
			}
			g.DrawRectangle(Pens.Black,-1,-1,Width,Height);//Outline
			//g.DrawRectangle(Pens.Black,Width-3,0,this.Width-3,this.Height-1);//Outline
			StringFormat stringFormat=new StringFormat();
			stringFormat.Alignment=StringAlignment.Center;
			stringFormat.LineAlignment=StringAlignment.Center;
			if(SelectedOption!="buc" && SelectedOption!="ling" && SelectedOption!="d" && SelectedOption!="m" && SelectedOption!="Clear") {
				g.DrawString(SelectedOption,new Font(FontFamily.GenericSansSerif,IsToothChart ? 10f:this.Height-10),Brushes.Black,new Point(this.Width/2,this.Height/2),stringFormat);//Draws selected option in box, centering it on the point supplied.
			}
			else if(SelectedOption=="None") {
				SelectedOption=DefaultOption;
				g.DrawString(DefaultOption,new Font(FontFamily.GenericSansSerif,IsToothChart ? 10f:this.Height-10),Brushes.LightGray,new Point(this.Width/2,this.Height/2),stringFormat);//Draws default option in box for D3 sheets
			}
			else {
				g.DrawString(DefaultOption,new Font(FontFamily.GenericSansSerif,IsToothChart ? 10f:this.Height-10),Brushes.LightGray,new Point(this.Width/2,this.Height/2),stringFormat);//Draws default option in box for D3 sheets
			}
			g.Dispose();
		}

		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
			
		}

		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);
			if(!isHovering) {
				isHovering=true;
				Invalidate();
			}
		}

		protected override void OnMouseLeave(EventArgs e) {
			base.OnMouseLeave(e);
			isHovering=false;
			Invalidate();
		}

	}
}
