using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OpenDentalGraph.Enumerations;
using System.Linq;

namespace OpenDentalGraph {
	public partial class Legend:UserControl {
		#region Private Data
		private LegendDockType _legendDock=LegendDockType.Left;
		private Dictionary<string,Color> _legendItems=new Dictionary<string,Color>();
		private float _paddingPx=3f;
		private int _scrollOffsetX=0;
		private int _contentWidth=0;
		private DateTime _stepDownStart;
		private DateTime _stepUpStart;
		private bool _mouseIsDown=false;
		private int _lastMouseX=0;
		#endregion

		#region Properties
		public LegendDockType LegendDock {
			get { return _legendDock; }
			set {
				_legendDock=value;
				_scrollOffsetX=0;
				try {
					switch(_legendDock) {
						case LegendDockType.Bottom:
							panelDraw.AutoScroll=false;
							panelDraw.AutoScrollMinSize=new Size(0,0);
							tableLayoutPanel1.ColumnStyles[0].Width=68;
							tableLayoutPanel1.ColumnStyles[2].Width=68;
							break;
						case LegendDockType.Left:
							panelDraw.AutoScroll=true;
							tableLayoutPanel1.ColumnStyles[0].Width=0;
							tableLayoutPanel1.ColumnStyles[2].Width=0;
							break;
						case LegendDockType.None:
						default:
							break;
					}
				}
				catch(Exception e) {					
				}
				panelDraw.Invalidate();
			}
		}				
		public float PaddingPx {
			get { return _paddingPx; }
			set { _paddingPx=value; panelDraw.Invalidate(); }
		}
		#endregion

		#region Ctor/Init
		public Legend() {
			InitializeComponent();
		}

		public void SetLegendItems(Dictionary<string,Color> items) {
			_legendItems=items;
			float leftBoxPx=0;
			float boxEdgePx=0;
			using(Graphics g = panelDraw.CreateGraphics()) {
				boxEdgePx=g.MeasureString("A",this.Font).Height;
				_legendItems.Keys.ToList().ForEach(x => {
					leftBoxPx+=PaddingPx+boxEdgePx+PaddingPx+g.MeasureString(x,this.Font).Width+PaddingPx;
				});
			}
			_contentWidth=(int)Math.Ceiling(leftBoxPx);
			panelDraw.Invalidate();
		}
		#endregion

		#region Drawing
		private void DrawLegendTop(PaintEventArgs e) {			
			float topPx=5;
			float leftBoxPx=0;
			float boxEdgePx=e.Graphics.MeasureString("A",this.Font).Height;
			float maxBottom=boxEdgePx+PaddingPx;
			e.Graphics.TranslateTransform(_scrollOffsetX,0);
			using(Brush brushText = new SolidBrush(this.ForeColor)) {
				foreach(KeyValuePair<string,Color> kvp in _legendItems) {
					leftBoxPx+=PaddingPx;
					SizeF sizeText=e.Graphics.MeasureString(kvp.Key,this.Font);
					using(Brush brushBox = new SolidBrush(kvp.Value)) {
						e.Graphics.FillRectangle(brushBox,leftBoxPx,topPx,boxEdgePx,boxEdgePx);
					}
					float textLeftPx=leftBoxPx+boxEdgePx+PaddingPx;
					e.Graphics.DrawString(kvp.Key,this.Font,brushText,textLeftPx,topPx);
					leftBoxPx=textLeftPx+sizeText.Width+PaddingPx;					
				}
			}
			Size sizeContents=new Size((int)Math.Ceiling(leftBoxPx),(int)Math.Ceiling(maxBottom));
		}

		private void DrawLegendLeft(PaintEventArgs e) {
			float topPx=0;
			float leftBoxPx=PaddingPx;
			float boxEdgePx=e.Graphics.MeasureString("A",this.Font).Height;
			float maxRightEdge=0f;
			float maxBottom=0f;
			e.Graphics.TranslateTransform(panelDraw.AutoScrollPosition.X,panelDraw.AutoScrollPosition.Y);
			using(Brush brushText = new SolidBrush(this.ForeColor)) {
				foreach(KeyValuePair<string,Color> kvp in _legendItems) {
					topPx+=PaddingPx;
					SizeF size=e.Graphics.MeasureString(kvp.Key,this.Font);
					using(Brush brushBox = new SolidBrush(kvp.Value)) {
						e.Graphics.FillRectangle(brushBox,leftBoxPx,topPx,boxEdgePx,boxEdgePx);
					}
					float textLeftPx=leftBoxPx+boxEdgePx+PaddingPx;
					e.Graphics.DrawString(kvp.Key,this.Font,brushText,textLeftPx,topPx);
					topPx+=boxEdgePx;
					maxRightEdge=Math.Max(maxRightEdge,textLeftPx+size.Width);
					maxBottom+=PaddingPx+size.Height;
				}
			}
			Size sizeContents=new Size((int)Math.Ceiling(maxRightEdge),(int)Math.Ceiling(maxBottom));			
			if(panelDraw.AutoScrollMinSize!=sizeContents) {
				panelDraw.AutoScrollMinSize=sizeContents;
				//this.AutoScrollPosition=new Point(0,0);
			}
		}

		private void Legend_Paint(object sender,PaintEventArgs e) {
			e.Graphics.Clear(this.BackColor);
			switch(LegendDock) {
				case LegendDockType.Bottom:
					DrawLegendTop(e);
					break;
				case LegendDockType.Left:
					DrawLegendLeft(e);
					break;
				case LegendDockType.None:
				default:
					break;
			}

		}
		#endregion

		#region Scrolling
		private void butScrollEnd_Click(object sender,EventArgs e) {
			int maxScroll=_contentWidth-panelDraw.Width;
			_scrollOffsetX=-maxScroll;
			panelDraw.Invalidate();
		}

		private void butScrollDownStep_Click(object sender,EventArgs e) {
			_scrollOffsetX-=50;
			int maxScroll=_contentWidth-panelDraw.Width;
			if(Math.Abs(_scrollOffsetX)>maxScroll) {
				_scrollOffsetX=-maxScroll;
			}
			if(_scrollOffsetX>0) {
				_scrollOffsetX=0;
			}
			panelDraw.Invalidate();
		}

		private void butScrollStart_Click(object sender,EventArgs e) {
			_scrollOffsetX=0;
			panelDraw.Invalidate();
		}

		private void butScrollUpStep_Click(object sender,EventArgs e) {
			_scrollOffsetX+=50;
			if(_scrollOffsetX>0) {
				_scrollOffsetX=0;
			}
			panelDraw.Invalidate();
		}
		
		private void timerStepUp_Tick(object sender,EventArgs e) {
			if(DateTime.Now.Subtract(_stepUpStart).TotalMilliseconds>400) {
				timerStepUp.Interval=20;
			}
			butScrollUpStep_Click(sender,e);
		}

		private void timerStepDown_Tick(object sender,EventArgs e) {
			if(DateTime.Now.Subtract(_stepDownStart).TotalMilliseconds>400) {
				timerStepDown.Interval=20;
			}
			butScrollDownStep_Click(sender,e);
		}

		private void butScrollDownStep_MouseDown(object sender,MouseEventArgs e) {
			_stepDownStart=DateTime.Now;
			timerStepDown.Interval=200;
			timerStepDown.Start();
		}

		private void butScrollDownStep_MouseUp(object sender,MouseEventArgs e) {
			timerStepDown.Stop();
		}
		
		private void butScrollUpStep_MouseDown(object sender,MouseEventArgs e) {
			_stepUpStart=DateTime.Now;
			timerStepUp.Interval=200;
			timerStepUp.Start();
		}

		private void butScrollUpStep_MouseUp(object sender,MouseEventArgs e) {
			timerStepUp.Stop();
		}
		
		private void panelDraw_MouseDown(object sender,MouseEventArgs e) {
			if(LegendDock!=LegendDockType.Bottom) {
				return;
			}
			_mouseIsDown=true;
			_lastMouseX=e.X;
		}

		private void panelDraw_MouseMove(object sender,MouseEventArgs e) {
			if(LegendDock!=LegendDockType.Bottom) {
				return;
			}
			if(!_mouseIsDown) {
				return;
			}
			int move=e.X-_lastMouseX;
			_lastMouseX=e.X;
			_scrollOffsetX+=move;
			int maxScroll=_contentWidth-panelDraw.Width;
			if(Math.Abs(_scrollOffsetX)>maxScroll) {
				_scrollOffsetX=-maxScroll;
			}
			if(_scrollOffsetX>0) {
				_scrollOffsetX=0;
			}
			panelDraw.Invalidate();
		}

		private void panelDraw_MouseUp(object sender,MouseEventArgs e) {
			_mouseIsDown=false;
			_lastMouseX=0;
		}
		#endregion

		private class PanelNoFlicker:Panel {
			public PanelNoFlicker() {
				this.DoubleBuffered=true;
			}
		}
	}
}
