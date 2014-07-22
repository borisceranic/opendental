using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenDental {
	public partial class EscalationViewControl:UserControl, INotifyPropertyChanged {
		
		public event PropertyChangedEventHandler PropertyChanged;

		private bool _isUpdating=false;

		private BindingList<String> _items=new BindingList<string>();
		[Category("Appearance")]
		[Description("Strings to be printed")]
		public BindingList<String> Items {
			get {
				return _items;
			}
			set {
				_items=value;
				PropertyChanged(this,new PropertyChangedEventArgs("Items"));
			}
		}

		private int _borderThickness=6;
		[Category("Appearance")]
		[Description("Thickness of the border drawn around the control")]
		public int BorderThickness {
			get {
				return _borderThickness;
			}
			set {
				_borderThickness=value;
				PropertyChanged(this,new PropertyChangedEventArgs("BorderThickness"));
			}
		}

		private Color _outerColor=Color.Black;
		[Category("Appearance")]
		[Description("Exterior Border Color")]
		public Color OuterColor {
			get {
				return _outerColor;
			}
			set {
				_outerColor=value;
				PropertyChanged(this,new PropertyChangedEventArgs("OuterColor"));
			}
		}

		private int _linePadding=-6;
		[Category("Appearance")]
		[Description("Padding of each line. Suggest -6 for 0 padding between lines. Must be an even number.")]
		public int LinePadding {
			get {
				return _linePadding;
			}
			set {
				_linePadding=value;
				PropertyChanged(this,new PropertyChangedEventArgs("LinePadding"));
			}
		}

		private int _startFadeIndex=4;
		[Category("Appearance")]
		[Description("Lines will start to fade at this 0-based index.")]
		public int StartFadeIndex {
			get {
				return _startFadeIndex;
			}
			set {
				_startFadeIndex=value;
				PropertyChanged(this,new PropertyChangedEventArgs("StartFadeIndex"));
			}
		}

		private int _fadeAlphaIncrement=40;
		[Category("Appearance")]
		[Description("Alpha increment to be subtracted from each faded line. Will be subtracted from each line after StartFadeIndex and eventually bottom out at 0 (fully transparent).")]
		public int FadeAlphaIncrement {
			get {
				return _fadeAlphaIncrement;
			}
			set {
				_fadeAlphaIncrement=value;
				PropertyChanged(this,new PropertyChangedEventArgs("FadeAlphaIncrement"));
			}
		}

		private int _minAlpha=60;
		[Category("Appearance")]
		[Description("Minimum alpha transparency value. Set to 0 if full transparency is desired. Otherwise a number between 0-255. 0 is full transparent, 255 is full opaque.")]
		public int MinAlpha {
			get {
				return _minAlpha;
			}
			set {
				_minAlpha=value;
				PropertyChanged(this,new PropertyChangedEventArgs("MinAlpha"));
			}
		}
	
		public EscalationViewControl() {
			InitializeComponent();
			PropertyChanged+=EscalationViewControl_PropertyChanged;

		}

		private void EscalationViewControl_PropertyChanged(object sender,PropertyChangedEventArgs e) {
			Invalidate();
		}

		private void EscalationViewControl_Paint(object sender,PaintEventArgs e) {
			if(_isUpdating) {
				return;
			}
			Pen penOuter=new Pen(OuterColor,BorderThickness);			
			try {
				RectangleF rcOuter=this.ClientRectangle;
				//clear control canvas
				e.Graphics.Clear(this.BackColor);
				Size sz=TextRenderer.MeasureText("a",Font);
				float halfPenThickness=BorderThickness/(float)2;
				//deflate for border
				rcOuter.Inflate(-halfPenThickness,-halfPenThickness);
				//draw border
				e.Graphics.DrawRectangle(penOuter,rcOuter.X,rcOuter.Y,rcOuter.Width,rcOuter.Height);
				//deflate to drawable region
				rcOuter.Inflate(-halfPenThickness,-halfPenThickness);
				int alpha=255;
				for(int i=0;i<_items.Count;i++) {
					string item=_items[i];
					if(i>StartFadeIndex) { //Only start fading after the user defined fade index.
						//Move toward transparency.
						alpha=Math.Max(MinAlpha,alpha-FadeAlphaIncrement);
					}
					//Set the bounds of the drawing rectangle.
					float y=rcOuter.Y+(i*sz.Height)+(i*(2*LinePadding));
					float height=sz.Height+(LinePadding*2);
					RectangleF rcItem=new RectangleF(rcOuter.X,y,rcOuter.Width,height);
					StringFormat sf=new StringFormat(StringFormatFlags.NoWrap);
					sf.LineAlignment=StringAlignment.Center;
					using(Brush brushText=new SolidBrush(Color.FromArgb(alpha,ForeColor))) {
						e.Graphics.DrawString(item,Font,brushText,rcItem,sf);
						//e.Graphics.DrawRectangle(Pens.Blue,Rectangle.Round(rcItem));
						//e.Graphics.FillEllipse(Brushes.Red,rcItem.X,rcItem.Y,3,3);
					}
				}
			}
			catch {
			}
			finally {
				penOuter.Dispose();
			}
		}

		public void BeginUpdate() {
			_isUpdating=true;
		}

		public void EndUpdate() {
			_isUpdating=false;
			Invalidate();
		}
	}
}
