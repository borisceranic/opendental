using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental.UI {
	///<summary></summary>
	public delegate void ODGridClickEventHandler(object sender,ODGridClickEventArgs e);

	///<summary>A new and improved grid control to replace the inherited ContrTable that is used so extensively in the program.</summary>
	[DefaultEvent("CellDoubleClick")]
	public class ODGrid:System.Windows.Forms.UserControl {
		///<summary>Required designer variable.</summary>
		private System.ComponentModel.Container components = null;
		private ODGridColumnCollection columns;
		///<summary></summary>
		[Category("Action"),Description("Occurs when a cell is double clicked.")]
		public event ODGridClickEventHandler CellDoubleClick=null;
		///<summary></summary>
		[Category("Action"),Description("Occurs when a cell is single clicked.")]
		public event ODGridClickEventHandler CellClick=null;
		///<summary></summary>
		[Category("Property Changed"),Description("Event used when cells are editable.  The TextChanged event is passed up from the textbox where the editing is taking place.")]
		public event EventHandler CellTextChanged=null;
		///<summary></summary>
		[Category("Focus"),Description("Event used when cells are editable.  LostFocus event is passed up from the textbox where the editing is taking place.")]
		public event ODGridClickEventHandler CellLeave=null;
		[Category("Focus"),Description("Event used when cells are editable.  GotFocus event is passed up from the textbox where the editing is taking place.")]
		public event ODGridClickEventHandler CellEnter=null;
		private string title;
		//private Font titleFont=new Font(FontFamily.GenericSansSerif,10,FontStyle.Bold);
		//private Font headerFont=new Font(FontFamily.GenericSansSerif,8.5f,FontStyle.Bold);
		//private Font cellFont=new Font(FontFamily.GenericSansSerif,8.5f);
		private float cellFontSize=8.5f;
		private int titleHeight=18;
		private int headerHeight=15;
		private Color cGridLine=Color.FromArgb(180,180,180);
		private System.Windows.Forms.VScrollBar vScroll;
		private System.Windows.Forms.HScrollBar hScroll;
		private ODGridRowCollection rows;
		private bool IsUpdating;
		///<summary>The total height of the grid.</summary>
		private int GridH;
		///<summary>The total width of the grid.</summary>
		private int GridW;
		///<summary>This array has one element for each row.  For each row, it keeps track of the vertical height of the row in pixels, not counting the note portion of the row.</summary>
		private int[] RowHeights;
		///<summary>This array has one element for each row.  For each row, it keeps track of the vertical height of only the note portion of the row in pixels.  Usually 0, unless you want notes showing.</summary>
		private int[] NoteHeights;
		///<summary>This array has one element for each row.  For each row, it keeps track of the vertical location at which to start drawing this row in pixels.  This makes it much easier to paint rows.</summary>
		private int[] RowLocs;
		private bool hScrollVisible;
		///<summary>Set at the very beginning of OnPaint.  Uses the ColWidth of each column to set up this array with one element for each column.  Contains the columns Pos for that column.</summary>
		private int[] ColPos;
		private ArrayList selectedIndices;
		private Point selectedCell;
		private int MouseDownRow;
		private int MouseDownCol;
		private bool ControlIsDown;
		private bool ShiftIsDown;
		//private bool UpDownKey;
		private GridSelectionMode selectionMode;
		private bool MouseIsDown;
		private bool MouseIsOver;//helps automate scrolling
		private string translationName;
		private Color selectedRowColor;
		private bool allowSelection;
		private bool wrapText;
		private int noteSpanStart;
		private int noteSpanStop;
		private TextBox editBox;
		private MouseButtons lastButtonPressed;
		private ArrayList selectedIndicesWhenMouseDown;
		private bool allowSortingByColumn;
		private bool mouseIsDownInHeader;
		///<summary>Typically -1 to show no triangle.  Or, specify a column to show a triangle.  The actual sorting happens at mouse down.</summary>
		private int sortedByColumnIdx;
		///<summary>True to show a triangle pointing up.  False to show a triangle pointing down.  Only works if sortedByColumnIdx is not -1.</summary>
		private bool sortedIsAscending;
		//private List<List<int>> multiPageNoteHeights;//If NoteHeights[i] won't fit on one page, get various page heights here (printing).
		//private List<List<string>> multiPageNoteSection;//Section of the note that is split up across multiple pages.
		private int RowsPrinted;
		///<summary>If we are part way through drawing a note when we reach the end of a page, this will contain the remainder of the note still to be printed.  If it is empty string, then we are not in the middle of a note.</summary>
		private string NoteRemaining;
		private Point oldSelectedCell;
		///<summary>Holds the amount of the grid that is hidden due to the user making the window too small.  We need to keep track of this so that when they resize the window the scroll bar will become visible again.</summary>
		private int widthHidden;
		///<summary>Is set when ComputeRows is called, then used . If any columns are editable HasEditableColumn is true.</summary>
		private bool HasEditableColumn;
		///<summary></summary>
		private const int EDITABLE_ROW_HEIGHT=19;
		private bool editableAcceptsCR;
		private static bool _useBlueTheme;

		///<summary></summary>
		public ODGrid() {
			//InitializeComponent();// Required for Windows.Forms Class Composition Designer support
			//Add any constructor code after InitializeComponent call
			columns=new ODGridColumnCollection();
			rows=new ODGridRowCollection();
			vScroll=new VScrollBar();
			vScroll.Scroll+=new ScrollEventHandler(vScroll_Scroll);
			vScroll.MouseEnter+=new EventHandler(vScroll_MouseEnter);
			vScroll.MouseLeave+=new EventHandler(vScroll_MouseLeave);
			vScroll.MouseMove+=new MouseEventHandler(vScroll_MouseMove);
			hScroll=new HScrollBar();
			hScroll.Scroll+=new ScrollEventHandler(hScroll_Scroll);
			hScroll.MouseEnter+=new EventHandler(hScroll_MouseEnter);
			hScroll.MouseLeave+=new EventHandler(hScroll_MouseLeave);
			hScroll.MouseMove+=new MouseEventHandler(hScroll_MouseMove);
			this.Controls.Add(vScroll);
			this.Controls.Add(hScroll);
			selectedIndices=new ArrayList();
			selectedCell=new Point(-1,-1);
			selectionMode=GridSelectionMode.One;
			selectedRowColor=Color.Silver;
			allowSelection=true;
			wrapText=true;
			noteSpanStart=0;
			noteSpanStop=0;
			sortedByColumnIdx=-1;
		}

		///<summary>Clean up any resources being used.</summary>
		protected override void Dispose(bool disposing) {
			if(disposing) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			components = new System.ComponentModel.Container();
		}
		#endregion

		///<summary></summary>
		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);
			if(this.Parent!=null) {
				this.Parent.MouseWheel+=new MouseEventHandler(Parent_MouseWheel);
				this.Parent.KeyDown+=new KeyEventHandler(Parent_KeyDown);
				this.Parent.KeyUp+=new KeyEventHandler(Parent_KeyUp);
			}
		}

		///<summary></summary>
		protected override void OnResize(EventArgs e) {
			base.OnResize(e);
			LayoutScrollBars();
			Invalidate();
		}

		#region Properties
		///<summary>Gets the collection of ODGridColumns assigned to the ODGrid control.</summary>
		//[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		//[Editor(typeof(System.ComponentModel.Design.CollectionEditor),typeof(System.Drawing.Design.UITypeEditor))]
		//[Browsable(false)]//only because MS is buggy.
		public ODGridColumnCollection Columns {
			get {
				return columns;
			}
		}

		///<summary>Gets the collection of ODGridRows assigned to the ODGrid control.</summary>
		[Browsable(false)]
		public ODGridRowCollection Rows {
			get {
				return rows;
			}
		}

		///<summary>The title of the grid which shows across the top.</summary>
		[Category("Appearance"),Description("The title of the grid which shows across the top.")]
		public string Title {
			get {
				return title;
			}
			set {
				title=value;
				Invalidate();
			}
		}

		///<summary>Set true to show a horizontal scrollbar.  Vertical scrollbar always shows, but is disabled if not needed.  If hScroll is not visible, then grid will auto reset width to match width of columns.</summary>
		[Category("Appearance"),Description("Set true to show a horizontal scrollbar.")]
		public bool HScrollVisible {
			get {
				return hScrollVisible;
			}
			set {
				hScrollVisible=value;
				LayoutScrollBars();
				Invalidate();
			}
		}

		///<summary>The index of the row that is the first row displayed on the ODGrid. Also sets ScrollValue.</summary>
		public void ScrollToIndex(int index) {
			if(index>Rows.Count) {
				return;
			}
			ScrollValue=RowLocs[index];
		}

		///<summary>The index of the row that is the last row to be displayed on the ODGrid. Also sets ScrollValue.</summary>
		public void ScrollToIndexBottom(int index) {
			if(index>Rows.Count) {
				return;
			}
			ScrollValue=((RowLocs[index]+RowHeights[index]+NoteHeights[index]+titleHeight+headerHeight)-Height)+3;//+3 accounts for the grid lines.
		}

		///<summary>Gets or sets the position of the vertical scrollbar.  Does all error checking and invalidates.</summary>
		[Browsable(false)]
		public int ScrollValue {
			get {
				return vScroll.Value;
			}
			set {
				if(!vScroll.Enabled) {
					return;
				}
				int scrollValue=0;
				if(value>vScroll.Maximum-vScroll.LargeChange){
					scrollValue=vScroll.Maximum-vScroll.LargeChange;
				}
				else if(value<vScroll.Minimum) {
					scrollValue=vScroll.Minimum;
				}
				else {
					scrollValue=value;
				}
				try {
					vScroll.Value=scrollValue;
				}
				catch(Exception e) {//This should never ever happen.
					//Showing a messagebox is NOT our normal way of handling errors on mouse events, but the user would get a popup for the unhandled exception, anyway.
					MessageBox.Show("Error: Invalid Scroll Value. \r\n"
						+"Scroll value from: "+vScroll.Value+"\r\n"
						+"Scroll value to: "+scrollValue+"\r\n"
						+"Min scroll value: "+vScroll.Minimum+"\r\n"
						+"Max scroll value: "+vScroll.Maximum+"\r\n"
						+"Large change value: "+vScroll.LargeChange+"\r\n\r\n"
						+e.ToString());
					vScroll.Value=vScroll.Minimum;
				}
				if(editBox!=null) {
					editBox.Dispose();
				}
				Invalidate();
			}
		}

		protected override void OnSizeChanged(EventArgs e) {
			base.OnSizeChanged(e);
			ScrollValue=vScroll.Value;
		}

		///<summary>Holds the int values of the indices of the selected rows.  To set selected indices, use SetSelected().</summary>
		[Browsable(false)]
		public int[] SelectedIndices {
			get {
				int[] retVal=new int[selectedIndices.Count];
				selectedIndices.CopyTo(retVal);
				Array.Sort(retVal);//they must be in numerical order
				return retVal;
			}
		}

		///<summary>Holds the x,y values of the selected cell if in OneCell mode.  -1,-1 represents no cell selected.</summary>
		[Browsable(false)]
		public Point SelectedCell {
			get {
				return selectedCell;
			}
		}

		///<summary></summary>
		[Category("Behavior"),Description("Just like the listBox.SelectionMode, except no MultiSimple, and added OneCell.")]
		[DefaultValue(typeof(GridSelectionMode),"One")]
		public GridSelectionMode SelectionMode {
			get {
				return selectionMode;
			}
			set {
				//if((GridSelectionMode)value==SelectionMode.MultiSimple){
				//	MessageBox.Show("MultiSimple not supported.");
				//	return;
				//}
				if((GridSelectionMode)value==GridSelectionMode.OneCell) {
					selectedCell=new Point(-1,-1);//?
					selectedIndices=new ArrayList();
				}
				selectionMode=value;
			}
		}

		///<summary></summary>
		[Category("Behavior"),Description("Set false to disable row selection when user clicks.  Row selection should then be handled by the form using the cellClick event.")]
		[DefaultValue(true)]
		public bool AllowSelection {
			get {
				return allowSelection;
			}
			set {
				allowSelection=value;
			}
		}

		///<summary>Uniquely identifies the grid for translation to another language.</summary>
		[Category("Appearance"),Description("Uniquely identifies the grid for translation to another language.")]
		public string TranslationName {
			get {
				return translationName;
			}
			set {
				translationName=value;
			}
		}

		///<summary>The background color that is used for selected rows.</summary>
		[Category("Appearance"),Description("The background color that is used for selected rows.")]
		[DefaultValue(typeof(Color),"Silver")]
		public Color SelectedRowColor {
			get {
				return selectedRowColor;
			}
			set {
				selectedRowColor=value;
			}
		}

		///<summary>Text within each cell will wrap, making some rows taller.</summary>
		[Category("Behavior"),Description("Text within each cell will wrap, making some rows taller.")]
		[DefaultValue(true)]
		public bool WrapText {
			get {
				return wrapText;
			}
			set {
				wrapText=value;
			}
		}

		///<summary>The starting column for notes on each row.  Notes are not part of the main row, but are displayed on subsequent lines.</summary>
		[Category("Appearance"),Description("The starting column for notes on each row.")]
		[DefaultValue(0)]//typeof(int),0)]
		public int NoteSpanStart {
			get {
				return noteSpanStart;
			}
			set {
				noteSpanStart=value;
			}
		}

		///<summary>The starting column for notes on each row.  Notes are not part of the main row, but are displayed on subsequent lines.  If this remains 0, then notes will be entirey skipped for this grid.</summary>
		[Category("Appearance"),Description("The ending column for notes on each row.")]
		[DefaultValue(0)]//typeof(int),0)]
		public int NoteSpanStop {
			get {
				return noteSpanStop;
			}
			set {
				noteSpanStop=value;
			}
		}

		///<summary>Used when drawing to PDF. We need the width of all columns summed.</summary>
		public int WidthAllColumns {
			get {
				int retVal=0;
				for(int i=0;i<columns.Count;i++) {
					retVal+=columns[i].ColWidth;
				}
				return retVal;
			}
		}

		///<summary>Set true to allow user to click on column headers to sort rows, alternating between ascending and descending.</summary>
		[Category("Behavior"),Description("Set true to allow user to click on column headers to sort rows, alternating between ascending and descending.")]
		[DefaultValue(false)]
		public bool AllowSortingByColumn {
			get {
				return allowSortingByColumn;
			}
			set {
				allowSortingByColumn=value;
				if(!allowSortingByColumn) {
					sortedByColumnIdx=-1;
				}
			}
		}

		///<summary>Only affects grids with editable columns. True allows carriage returns within cells. Falses causes carriage returns to go to the next editable cell.</summary>
		[Category("Behavior"),Description("Set true to allow editable cells to accept carriage returns.")]
		[DefaultValue(false)]
		public bool EditableAcceptsCR {
			get {
				return editableAcceptsCR;
			}
			set {
				editableAcceptsCR=value;
			}
		}

		///<summary>Container sets this for all grids simultaneously.</summary>
		[Browsable(false)]
		public static bool UseBlueTheme {
			get {
				return _useBlueTheme;
			}
			set {
				_useBlueTheme=value;
			}
		}

		///<summary>Returns current sort order.  Only used in PatientPortalManager.</summary>
		[Browsable(false)]
		public bool SortedIsAscending {
			get {
				return sortedIsAscending;
			}
		}

		///<summary>Returns current sort column index.  Only used in PatientPortalManager.</summary>
		[Browsable(false)]
		public int SortedByColumnIdx {
			get {
				return sortedByColumnIdx;
			}
		}

		#endregion Properties

		#region Computations
		///<summary>Computes the position of each column and the overall width.  Called from endUpdate and also from OnPaint.</summary>
		private void ComputeColumns() {
			if(!hScrollVisible) {//this used to be in the resize logic
				int minGridW=0;//sum of columns widths except last one.
				for(int i=0;i<columns.Count;i++) {
					if(i<columns.Count-1) {
						minGridW+=columns[i].ColWidth;
					}
				}
				if(widthHidden>0) {
					this.Width-=widthHidden;//just for a few lines
					widthHidden=0;
				}
				int minimumWidth=minGridW+2+vScroll.Width+5;
				if(this.Width<minimumWidth) {//Trying to make it too narrow.
					widthHidden=minimumWidth-Width;//Keep track of how much of the grid is being hidden.
					this.Width=minimumWidth;//make it get stuck at the last column.  User doesn't notice the part that's sticking over to the right.
				}
				else if(columns.Count>0) {//resize the last column automatically
					columns[columns.Count-1].ColWidth=Width-2-vScroll.Width-minGridW;
				}
			}
			ColPos=new int[columns.Count];
			for(int i=0;i<ColPos.Length;i++) {
				if(i==0)
					ColPos[i]=0;
				else
					ColPos[i]=ColPos[i-1]+columns[i-1].ColWidth;
			}
			if(columns.Count>0) {
				GridW=ColPos[ColPos.Length-1]+columns[columns.Count-1].ColWidth;
			}
		}

		///<summary>Called from PrintPage() and EndUpdate().  After adding rows to the grid, this calculates the height of each row because some rows may have text wrap and will take up more than one row.  Also, rows with notes, must be made much larger, because notes start on the second line.  If column images are used, rows will be enlarged to make space for the images.</summary>
		private void ComputeRows(Graphics g) {
			//Travis - 06/04/2013: Sometimes ComputeRows() will incorrectly measure the number of rows when the font of the cell will display in bold.  This can cause text that would be on a new line to not display.
			//This is because all textual measurements use cellFont which is never bold.  A possible solution would be to add another font that is bold to the current using statement.  
			//Then before any text is measured, check if the cell is bold in order to pass the appropriate font.
			//using(Graphics g=this.CreateGraphics()) {
				using(Font cellFont=new Font(FontFamily.GenericSansSerif,cellFontSize)) {
					RowHeights=new int[rows.Count];
					NoteHeights=new int[rows.Count];
					//multiPageNoteHeights=new List<List<int>>();
					//multiPageNoteSection=new List<List<string>>();
					//for(int i=0;i<rows.Count;i++) {
						//List<int> intList=new List<int>();
						//multiPageNoteHeights.Add(intList);
						//List<string> stringList=new List<string>();
						//multiPageNoteSection.Add(stringList);
					//}
					RowLocs=new int[rows.Count];
					GridH=0;
					int cellH;
					int noteW=0;
					if(NoteSpanStop>0 && NoteSpanStart<columns.Count) {
						for(int i=NoteSpanStart;i<=NoteSpanStop;i++) {
							noteW+=columns[i].ColWidth;
						}
					}
					int imageH=0;
					HasEditableColumn=false;
					for(int i=0;i<columns.Count;i++) {
						if(columns[i].IsEditable){
							HasEditableColumn=true;
						}
						if(columns[i].ImageList!=null) {
							if(columns[i].ImageList.ImageSize.Height>imageH) {
								imageH=columns[i].ImageList.ImageSize.Height+1;
							}
						}
					}
					for(int i=0;i<rows.Count;i++) {
						RowHeights[i]=0;
						if(wrapText) {
							//find the tallest col
							for(int j=0;j<rows[i].Cells.Count;j++) {
								if(HasEditableColumn) {
									//doesn't seem to calculate right when it ends in a "\r\n". It doesn't make room for the new line. Make it, by adding another one for calculations.
									cellH=(int)((1.03)*(float)(g.MeasureString(rows[i].Cells[j].Text+"\r\n",cellFont,columns[j].ColWidth).Height))+4;//because textbox will be bigger
									if(cellH < EDITABLE_ROW_HEIGHT) {
										cellH=EDITABLE_ROW_HEIGHT;//only used for single line text
									}
								}
								else {
									cellH=(int)g.MeasureString(rows[i].Cells[j].Text,cellFont,columns[j].ColWidth).Height+1;
								}
								//if(rows[i].Height==0) {//not set
								//  cellH=(int)g.MeasureString(rows[i].Cells[j].Text,cellFont,columns[j].ColWidth).Height+1;
								//}
								//else {
								//  cellH=rows[i].Height;
								//}
								if(cellH>RowHeights[i]) {
									RowHeights[i]=cellH;
								}
							}
							//Cameron 10/23/2013: Rows used to look like thick lines when the row height was 1.  When the height is less than 4, the row is not visible enough to select or edit.
							//We will use the height of the string "Any" to determine a better row height so the user can see that it is an empty row.
							//If, for whatever reason, their font really does return a row height less than 4, the following code will return that value anyway thus this change should be harmless.
							if(RowHeights[i]<4) {
								RowHeights[i]=(int)g.MeasureString("Any",cellFont,100).Height+1;
							}
						}
						else {//text not wrapping
							if(HasEditableColumn) {
								RowHeights[i]=EDITABLE_ROW_HEIGHT;
							}
							else {
								RowHeights[i]=(int)g.MeasureString("Any",cellFont,100).Height+1;
							}
							//if(rows[i].Height==0) {//not set
							//	RowHeights[i]=(int)g.MeasureString("Any",cellFont,100).Height+1;
							//}
							//else {
							//	RowHeights[i]=rows[i].Height;
							//}
						}
						if(imageH>RowHeights[i]) {
							RowHeights[i]=imageH;
						}
						if(noteW>0 && rows[i].Note!="") {
							NoteHeights[i]=(int)g.MeasureString(rows[i].Note,cellFont,noteW).Height;
						}
						if(i==0) {
							RowLocs[i]=0;
						}
						else {
							RowLocs[i]=RowLocs[i-1]+RowHeights[i-1]+NoteHeights[i-1];
						}
						GridH+=RowHeights[i]+NoteHeights[i];
					}
				}
			//}
		}

		///<summary>Returns row. -1 if no valid row.  Supply the y position in pixels.</summary>
		public int PointToRow(int y) {
			if(y<1+titleHeight+headerHeight) {
				return -1;
			}
			for(int i=0;i<rows.Count;i++) {
				if(y>-vScroll.Value+1+titleHeight+headerHeight+RowLocs[i]+RowHeights[i]+NoteHeights[i]) {
					continue;//clicked below this row.
				}
				return i;
			}
			return -1;
		}

		///<summary>Returns col.  Supply the x position in pixels. -1 if no valid column.</summary>
		public int PointToCol(int x) {
			int colRight;//the right edge of each column
			for(int i=0;i<columns.Count;i++) {
				colRight=0;
				for(int c=0;c<i+1;c++) {
					colRight+=columns[c].ColWidth;
				}
				if(x>-hScroll.Value+colRight) {
					continue;//clicked to the right of this col
				}
				return i;
			}
			return -1;
		}
		#endregion Computations

		#region Painting
		///<summary></summary>
		protected override void OnPaintBackground(PaintEventArgs pea) {
			//base.OnPaintBackground (pea);
			//don't paint background.  This reduces flickering.
		}

		///<summary>Runs any time the control is invalidated.</summary>
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
			if(IsUpdating) {
				return;
			}
			if(Width<1 || Height<1) {
				return;
			}
			ComputeColumns();//it's only here because I can't figure out how to do it when columns are added. It will be removed.
			Bitmap doubleBuffer=new Bitmap(Width,Height,e.Graphics);
			using(Graphics g=Graphics.FromImage(doubleBuffer)) {
				g.SmoothingMode=SmoothingMode.HighQuality;//for the up/down triangles
				//g.TextRenderingHint=TextRenderingHint.AntiAlias;//for accurate string measurements. Didn't work
				//g.TextRenderingHint=TextRenderingHint.SingleBitPerPixelGridFit;
				//float pagescale=g.PageScale;
				DrawBackG(g);
				DrawRows(g);
				DrawTitleAndHeaders(g);//this will draw on top of any grid stuff
				DrawOutline(g);
				e.Graphics.DrawImageUnscaled(doubleBuffer,0,0);
			}
			doubleBuffer.Dispose();
			doubleBuffer=null;
		}

		///<summary>Draws a solid gray background.</summary>
		private void DrawBackG(Graphics g) {
			//if(vScroll.Enabled){//all backg white, since no gray will show
			//	g.FillRectangle(new SolidBrush(Color.White),
			//		0,titleHeight+headerHeight+1,
			//		Width,this.Height-titleHeight-headerHeight-1);
			//}
			//else{
			Color cBackG=Color.FromArgb(224,223,227);
			if(_useBlueTheme) {
				cBackG=Color.FromArgb(202,212,222);//174,196,217);//151,180,196);
			}
			g.FillRectangle(new SolidBrush(cBackG),
				0,titleHeight+headerHeight,
				Width,Height-titleHeight-headerHeight);
			//}
		}

		///<summary>Draws the background, lines, and text for all rows that are visible.</summary>
		private void DrawRows(Graphics g) {
			Font cellFont=new Font(FontFamily.GenericSansSerif,cellFontSize);
			if(CultureInfo.CurrentCulture.Name.EndsWith("IN")) {
				cellFont.Dispose();
				cellFont=new Font("Arial",cellFontSize);
			}
			try {
				for(int i=0;i<rows.Count;i++) {
					if(-vScroll.Value+RowLocs[i]+RowHeights[i]+NoteHeights[i]<0) {
						continue;//lower edge of row above top of grid area
					}
					if(-vScroll.Value+1+titleHeight+headerHeight+RowLocs[i]>Height) {
						return;//row below lower edge of control
					}
					DrawRow(i,g,cellFont);
				}
			}
			finally {
				if(cellFont!=null) {
					cellFont.Dispose();
				}
			}
		}

		///<summary>Draws background, lines, image, and text for a single row.</summary>
		private void DrawRow(int rowI,Graphics g,Font cellFont) {
			RectangleF textRect;
			StringFormat format=new StringFormat();
			Pen gridPen=new Pen(this.cGridLine);
			Pen lowerPen=new Pen(this.cGridLine);
			if(rowI==rows.Count-1) {//last row
				lowerPen=new Pen(Color.FromArgb(120,120,120));
			}
			else {
				if(rows[rowI].ColorLborder!=Color.Empty) {
					lowerPen=new Pen(rows[rowI].ColorLborder);
				}
			}
			SolidBrush textBrush;
			//selected row color
			if(selectedIndices.Contains(rowI)) {
				g.FillRectangle(new SolidBrush(selectedRowColor),
					1,
					-vScroll.Value+1+titleHeight+headerHeight+RowLocs[rowI]+1,
					GridW,
					RowHeights[rowI]+NoteHeights[rowI]-1);
			}
			//colored row background
			else if(rows[rowI].ColorBackG!=Color.White) {
				g.FillRectangle(new SolidBrush(rows[rowI].ColorBackG),
					1,
					-vScroll.Value+1+titleHeight+headerHeight+RowLocs[rowI]+1,
					GridW,
					RowHeights[rowI]+NoteHeights[rowI]-1);
			}
			//normal row color
			else {//need to draw over the gray background
				g.FillRectangle(new SolidBrush(rows[rowI].ColorBackG),
					1,
					-vScroll.Value+1+titleHeight+headerHeight+RowLocs[rowI]+1,
					GridW,//this is a really simple width value that always works well
					RowHeights[rowI]+NoteHeights[rowI]-1);
			}
			if(selectionMode==GridSelectionMode.OneCell && selectedCell.X!=-1 && selectedCell.Y!=-1
			&& selectedCell.Y==rowI) {
				g.FillRectangle(new SolidBrush(selectedRowColor),
					-hScroll.Value+1+ColPos[selectedCell.X],
					-vScroll.Value+1+titleHeight+headerHeight+RowLocs[rowI]+1,
					columns[selectedCell.X].ColWidth,
					RowHeights[rowI]+NoteHeights[rowI]-1);
			}
			//lines for note section
			if(NoteHeights[rowI]>0) {
				//left vertical gridline
				if(NoteSpanStart!=0) {
					g.DrawLine(gridPen,
						-hScroll.Value+1+ColPos[NoteSpanStart],
						-vScroll.Value+1+titleHeight+headerHeight+RowLocs[rowI]+RowHeights[rowI],
						-hScroll.Value+1+ColPos[NoteSpanStart],
						-vScroll.Value+1+titleHeight+headerHeight+RowLocs[rowI]+RowHeights[rowI]+NoteHeights[rowI]);
				}
				//Horizontal line which divides the main part of the row from the notes section of the row
				g.DrawLine(gridPen,
					-hScroll.Value+1+ColPos[0]+1,
					-vScroll.Value+1+titleHeight+headerHeight+RowLocs[rowI]+RowHeights[rowI],
					-hScroll.Value+1+ColPos[columns.Count-1]+columns[columns.Count-1].ColWidth,
					-vScroll.Value+1+titleHeight+headerHeight+RowLocs[rowI]+RowHeights[rowI]);

			}
			for(int i=0;i<columns.Count;i++) {
				//right vertical gridline
				if(rowI==0) {
					g.DrawLine(gridPen,
						-hScroll.Value+1+ColPos[i]+columns[i].ColWidth,
						-vScroll.Value+1+titleHeight+headerHeight+RowLocs[rowI],
						-hScroll.Value+1+ColPos[i]+columns[i].ColWidth,
						-vScroll.Value+1+titleHeight+headerHeight+RowLocs[rowI]+RowHeights[rowI]);
				}
				else {
					g.DrawLine(gridPen,
						-hScroll.Value+1+ColPos[i]+columns[i].ColWidth,
						-vScroll.Value+1+titleHeight+headerHeight+RowLocs[rowI]+1,
						-hScroll.Value+1+ColPos[i]+columns[i].ColWidth,
						-vScroll.Value+1+titleHeight+headerHeight+RowLocs[rowI]+RowHeights[rowI]);
				}
				//lower horizontal gridline
				if(i==0) {
					g.DrawLine(lowerPen,
						-hScroll.Value+1+ColPos[i],
						-vScroll.Value+1+titleHeight+headerHeight+RowLocs[rowI]+RowHeights[rowI]+NoteHeights[rowI],
						-hScroll.Value+1+ColPos[i]+columns[i].ColWidth,
						-vScroll.Value+1+titleHeight+headerHeight+RowLocs[rowI]+RowHeights[rowI]+NoteHeights[rowI]);
				}
				else {
					g.DrawLine(lowerPen,
						-hScroll.Value+1+ColPos[i]+1,
						-vScroll.Value+1+titleHeight+headerHeight+RowLocs[rowI]+RowHeights[rowI]+NoteHeights[rowI],
						-hScroll.Value+1+ColPos[i]+columns[i].ColWidth,
						-vScroll.Value+1+titleHeight+headerHeight+RowLocs[rowI]+RowHeights[rowI]+NoteHeights[rowI]);
				}
				//text
				if(rows[rowI].Cells.Count-1<i) {
					continue;
				}
				switch(columns[i].TextAlign) {
					case HorizontalAlignment.Left:
						format.Alignment=StringAlignment.Near;
						break;
					case HorizontalAlignment.Center:
						format.Alignment=StringAlignment.Center;
						break;
					case HorizontalAlignment.Right:
						format.Alignment=StringAlignment.Far;
						break;
				}
				int vertical=-vScroll.Value+1+titleHeight+headerHeight+RowLocs[rowI]+1;
				int horizontal=-hScroll.Value+1+ColPos[i]+1;
				int cellW=columns[i].ColWidth;
				int cellH=RowHeights[rowI];
				if(HasEditableColumn) {//These cells are taller
						vertical+=2;//so this is to push text down to center it in the cell
						cellH-=3;//to keep it from spilling into the next cell
				}
				if(columns[i].TextAlign==HorizontalAlignment.Right) {
					if(HasEditableColumn) {
						horizontal-=4;
						cellW+=2;
					}
					else {
						horizontal-=2;
						cellW+=2;
					}
				}
				textRect=new RectangleF(horizontal,vertical,cellW,cellH);
				if(rows[rowI].Cells[i].ColorText==Color.Empty) {
					textBrush=new SolidBrush(rows[rowI].ColorText);
				}
				else {
					textBrush=new SolidBrush(rows[rowI].Cells[i].ColorText);
				}
				if(rows[rowI].Cells[i].Bold==YN.Yes) {
					cellFont=new Font(cellFont,FontStyle.Bold);
				}
				else if(rows[rowI].Cells[i].Bold==YN.No) {
					cellFont=new Font(cellFont,FontStyle.Regular);
				}
				else {//unknown.  Use row bold
					if(rows[rowI].Bold) {
						cellFont=new Font(cellFont,FontStyle.Bold);
					}
					else {
						cellFont=new Font(cellFont,FontStyle.Regular);
					}
				}
				if(columns[i].ImageList==null) {
					g.DrawString(rows[rowI].Cells[i].Text,cellFont,textBrush,textRect,format);
				}
				else {
					int imageIndex=-1;
					if(rows[rowI].Cells[i].Text!="") {
						imageIndex=PIn.Int(rows[rowI].Cells[i].Text);
					}
					if(imageIndex!=-1) {
						Image img=columns[i].ImageList.Images[imageIndex];
						g.DrawImage(img,horizontal,vertical-1);
					}
				}
			}
			//note text
			if(NoteHeights[rowI]>0 && NoteSpanStop>0 && NoteSpanStart<columns.Count) {
				int noteW=0;
				for(int i=NoteSpanStart;i<=NoteSpanStop;i++) {
					noteW+=columns[i].ColWidth;
				}
				if(rows[rowI].Bold) {
					cellFont=new Font(cellFont,FontStyle.Bold);
				}
				else {
					cellFont=new Font(cellFont,FontStyle.Regular);
				}
				textBrush=new SolidBrush(rows[rowI].ColorText);
				textRect=new RectangleF(
					-hScroll.Value+1+ColPos[NoteSpanStart]+1,
					-vScroll.Value+1+titleHeight+headerHeight+RowLocs[rowI]+RowHeights[rowI]+1,
					ColPos[NoteSpanStop]+columns[NoteSpanStop].ColWidth-ColPos[NoteSpanStart],
					NoteHeights[rowI]);
				format.Alignment=StringAlignment.Near;
				g.DrawString(rows[rowI].Note,cellFont,textBrush,textRect,format);
			}
		}

		private void DrawTitleAndHeaders(Graphics g) {
			//Title----------------------------------------------------------------------------------------------------
			Color cTitleTop=Color.White;
			Color cTitleBottom=Color.FromArgb(213,213,223);
			Color cTitleText=Color.Black;
			if(_useBlueTheme) {
				cTitleTop=Color.FromArgb(156,175,230);//191,205,245);//139,160,224);//114,136,201);//106,132,210);//109,129,191);//104,136,232);
				cTitleBottom=Color.FromArgb(60,90,150);//35,55,115);//49,63,105);//(20,47,126);
				cTitleText=Color.White;
			}
			LinearGradientBrush brushTitleBackground=new LinearGradientBrush(new Rectangle(0,0,Width,titleHeight),cTitleTop,cTitleBottom,LinearGradientMode.Vertical);
			SolidBrush brushTitleText=new SolidBrush(cTitleText);
			g.FillRectangle(brushTitleBackground,0,0,Width,titleHeight);
			Font titleFont=new Font(FontFamily.GenericSansSerif,10,FontStyle.Bold);
			g.DrawString(title,titleFont,brushTitleText,Width/2-g.MeasureString(title,titleFont).Width/2,2);
			if(brushTitleBackground!=null) {
				brushTitleBackground.Dispose();
				brushTitleBackground=null;
			}
			if(brushTitleText!=null) {
				brushTitleText.Dispose();
				brushTitleText=null;
			}
			if(titleFont!=null) {
				titleFont.Dispose();
				titleFont=null;
			}
			//Column Headers-----------------------------------------------------------------------------------------
			Color cTitleBackG=Color.FromArgb(210,210,210);
			if(_useBlueTheme){
				cTitleBackG=Color.FromArgb(223,234,245);//208,225,242);//166,185,204);
			}
			g.FillRectangle(new SolidBrush(cTitleBackG),0,titleHeight,Width,headerHeight);//background
			g.DrawLine(new Pen(Color.FromArgb(102,102,122)),0,titleHeight,Width,titleHeight);//line between title and headers
			using(Font headerFont=new Font(FontFamily.GenericSansSerif,8.5f,FontStyle.Bold)) {
				for(int i=0;i<columns.Count;i++) {
					if(i!=0) {
						//vertical lines separating column headers
						g.DrawLine(new Pen(Color.FromArgb(120,120,120)),-hScroll.Value+1+ColPos[i],titleHeight+3,
							-hScroll.Value+1+ColPos[i],titleHeight+headerHeight-2);
						g.DrawLine(new Pen(Color.White),-hScroll.Value+1+ColPos[i]+1,titleHeight+3,
							-hScroll.Value+1+ColPos[i]+1,titleHeight+headerHeight-2);
					}
					g.DrawString(columns[i].Heading,headerFont,Brushes.Black,
						-hScroll.Value+ColPos[i]+columns[i].ColWidth/2-g.MeasureString(columns[i].Heading,headerFont).Width/2,
						titleHeight+2);
					if(sortedByColumnIdx==i) {
						PointF p=new PointF(-hScroll.Value+1+ColPos[i]+6,titleHeight+(float)headerHeight/2f);
						if(sortedIsAscending) {//pointing up
							g.FillPolygon(Brushes.White,new PointF[] {
								new PointF(p.X-4.9f,p.Y+2f),//LLstub
								new PointF(p.X-4.9f,p.Y+2.5f),//LLbase
								new PointF(p.X+4.9f,p.Y+2.5f),//LRbase
								new PointF(p.X+4.9f,p.Y+2f),//LRstub
								new PointF(p.X,p.Y-2.8f)});//Top
							g.FillPolygon(Brushes.Black,new PointF[] {
								new PointF(p.X-4,p.Y+2),//LL
								new PointF(p.X+4,p.Y+2),//LR
								new PointF(p.X,p.Y-2)});//Top
						}
						else {//pointing down
							g.FillPolygon(Brushes.White,new PointF[] {//shaped like home plate
								new PointF(p.X-4.9f,p.Y-2f),//ULstub
								new PointF(p.X-4.9f,p.Y-2.7f),//ULtop
								new PointF(p.X+4.9f,p.Y-2.7f),//URtop
								new PointF(p.X+4.9f,p.Y-2f),//URstub
								new PointF(p.X,p.Y+2.8f)});//Bottom
							g.FillPolygon(Brushes.Black,new PointF[] {
								new PointF(p.X-4,p.Y-2),//UL
								new PointF(p.X+4,p.Y-2),//UR
								new PointF(p.X,p.Y+2)});//Bottom
						}
					}
				}
			}
			//line below headers
			g.DrawLine(new Pen(Color.FromArgb(120,120,120)),0,titleHeight+headerHeight,Width,titleHeight+headerHeight);
		}

		///<summary>Draws outline around entire control.</summary>
		private void DrawOutline(Graphics g) {
			if(hScroll.Visible) {//for the little square at the lower right between the two scrollbars
				g.FillRectangle(new SolidBrush(Color.FromKnownColor(KnownColor.Control)),Width-vScroll.Width-1,
					Height-hScroll.Height-1,vScroll.Width,hScroll.Height);
			}
			Color cOutline=Color.FromArgb(119,119,146);
			if(_useBlueTheme){
				cOutline=Color.FromArgb(47,70,117);
			}
			using(Pen pen=new Pen(cOutline)) {
				g.DrawRectangle(pen,0,0,Width-1,Height-1);
			}
		}
		#endregion

		#region Clicking
		///<summary></summary>
		protected void OnCellDoubleClick(int col,int row) {
			ODGridClickEventArgs gArgs=new ODGridClickEventArgs(col,row,MouseButtons.Left);
			if(CellDoubleClick!=null) {
				CellDoubleClick(this,gArgs);
			}
		}

		///<summary></summary>
		protected override void OnDoubleClick(EventArgs e) {
			base.OnDoubleClick(e);
			if(MouseDownRow==-1) {
				return;//double click was in the title or header section
			}
			if(MouseDownCol==-1) {
				return;//click was to the right of the columns
			}
			OnCellDoubleClick(MouseDownCol,MouseDownRow);
		}

		///<summary></summary>
		protected void OnCellClick(int col,int row,MouseButtons button) {
			ODGridClickEventArgs gArgs=new ODGridClickEventArgs(col,row,button);
			if(CellClick!=null) {
				CellClick(this,gArgs);
			}
		}

		///<summary></summary>
		protected override void OnClick(EventArgs e) {
			base.OnClick(e);
			if(MouseDownRow==-1) {
				return;//click was in the title or header section
			}
			if(MouseDownCol==-1) {
				return;//click was to the right of the columns
			}
			OnCellClick(MouseDownCol,MouseDownRow,lastButtonPressed);
		}
		#endregion Clicking

		#region BeginEndUpdate
		///<summary>Call this before adding any rows.  You would typically call Rows.Clear after this.</summary>
		public void BeginUpdate() {
			IsUpdating=true;
		}

		///<summary>Must be called after adding rows.  This computes the columns, computes the rows, lays out the scrollbars, clears SelectedIndices, and invalidates.  Does not zero out scrollVal.  Sometimes, it seems like scrollVal needs to be reset somehow because it's an inappropriate number, and when you first grab the scrollbar, it jumps.  No time to investigate.</summary>
		public void EndUpdate() {
			ComputeColumns();
			using(Graphics g=this.CreateGraphics()) {
				ComputeRows(g);
			}
			LayoutScrollBars();
			//ScrollValue=0;
			selectedIndices=new ArrayList();
			selectedCell=new Point(-1,-1);
			if(editBox!=null) {
				editBox.Dispose();
			}
			sortedByColumnIdx=-1;
			IsUpdating=false;
			Invalidate();
		}
		#endregion BeginEndUpdate

		#region Printing

		///<summary>If there are more pages to print, it returns -1.  If this is the last page, it returns the yPos of where the printing stopped.  Graphics will be paper, pageNumber resets some class level variables at page 0, bounds are used to contain the grid drawing, and marginTopFirstPage leaves room so as to not overwrite the title and subtitle.</summary>
		public int PrintPage(Graphics g,int pageNumber,Rectangle bounds,int marginTopFirstPage) {
			return PrintPage(g,pageNumber,bounds,marginTopFirstPage,false);
		}

		///<summary>Same as PrintPage, but added bool to signature for HasHeaderEveryPage. </summary>
		public int PrintPage(Graphics g,int pageNumber,Rectangle bounds,int marginTopFirstPage,bool HasHeaderSpaceOnEveryPage) {
			//Printers ignore TextRenderingHint.AntiAlias.  
			//And they ignore SmoothingMode.HighQuality.
			//They seem to do font themselves instead of letting us have control.
			//g.TextRenderingHint=TextRenderingHint.AntiAlias;//an attempt to fix the printing measurements.
			//g.SmoothingMode=SmoothingMode.HighQuality;
			//g.PageUnit=GraphicsUnit.Display;
			//float pagescale=g.PageScale;
			//g.PixelOffsetMode=PixelOffsetMode.HighQuality;
			//g.
			if(RowsPrinted==0) {
				//set row heights 4% larger when printing:
				ComputeRows(g);
			}
			int xPos=bounds.Left;
			//now, try to center in bounds
			if((float)GridW<bounds.Width) {
				xPos=(int)(bounds.Left+bounds.Width/2-(float)GridW/2);
			}
			StringFormat format=new StringFormat();
			Pen gridPen;
			Pen lowerPen;
			SolidBrush textBrush;
			RectangleF textRect;
			Font cellFont=new Font(FontFamily.GenericSansSerif,cellFontSize);
			//Initialize our pens for drawing.
			gridPen=new Pen(this.cGridLine);
			lowerPen=new Pen(this.cGridLine);
			int yPos=bounds.Top;
			if(HasHeaderSpaceOnEveryPage) {
				yPos=marginTopFirstPage;//Margin is lower because title and subtitle are printed externally.
			}
			if(pageNumber==0) {
				yPos=marginTopFirstPage;//Margin is lower because title and subtitle are printed externally.
				RowsPrinted=0;
				NoteRemaining="";
			}
			bool isFirstRowOnPage=true;//helps with handling a very tall first row
			if(RowsPrinted==rows.Count-1) {//last row
				lowerPen=new Pen(Color.FromArgb(120,120,120));
			}
			else {
				if(rows[RowsPrinted].ColorLborder!=Color.Empty) {
					lowerPen=new Pen(rows[RowsPrinted].ColorLborder);
				}
			}
			try {
				#region ColumnHeaders
				//Print column headers on every page.
				g.FillRectangle(Brushes.LightGray,xPos+ColPos[0],yPos,(float)GridW,headerHeight);
				g.DrawRectangle(Pens.Black,xPos+ColPos[0],yPos,(float)GridW,headerHeight);
				for(int i=1;i<ColPos.Length;i++) {
					g.DrawLine(Pens.Black,xPos+(float)ColPos[i],yPos,xPos+(float)ColPos[i],yPos+headerHeight);
				}
				using(Font headerFont=new Font(FontFamily.GenericSansSerif,8.5f,FontStyle.Bold)) {
					for(int i=0;i<columns.Count;i++) {
						g.DrawString(columns[i].Heading,headerFont,Brushes.Black,
							xPos+(float)ColPos[i]+columns[i].ColWidth/2-g.MeasureString(columns[i].Heading,headerFont).Width/2,
							yPos);
					}
				}
				yPos+=headerHeight;
				#endregion ColumnHeaders
				#region Rows
				while(RowsPrinted<rows.Count) {
					#region RowMainPart
					if(NoteRemaining=="") {//We are not in the middle of a note from a previous page. If we are in the middle of a note that will get printed next, as it is the next region of code (RowNotePart).
						//Go to next page if it doesn't fit.
						if(yPos+(float)RowHeights[RowsPrinted] > bounds.Bottom) {//The row is too tall to fit
							if(isFirstRowOnPage) {
								//todo some day: handle very tall first rows.  For now, print what we can.
							}
							else {
								break;//Go to next page.
							}
						}
						//There is enough room to print this row.
						//Draw the left vertical gridline
						g.DrawLine(gridPen,
							xPos+ColPos[0],
							yPos,
							xPos+ColPos[0],
							yPos+(float)RowHeights[RowsPrinted]);
						for(int i=0;i<columns.Count;i++) {
							//Draw the other vertical gridlines
							g.DrawLine(gridPen,
								xPos+(float)ColPos[i]+(float)columns[i].ColWidth,
								yPos,
								xPos+(float)ColPos[i]+(float)columns[i].ColWidth,
								yPos+(float)RowHeights[RowsPrinted]);
							if(rows[RowsPrinted].Note=="") {//End of row. Mark with a dark line (lowerPen).
								//Horizontal line which divides the main part of the row from the notes section of the row one column at a time.
								g.DrawLine(lowerPen,
									xPos+ColPos[0],
									yPos+(float)RowHeights[RowsPrinted],
									xPos+(float)ColPos[columns.Count-1]+(float)columns[columns.Count-1].ColWidth,
									yPos+(float)RowHeights[RowsPrinted]);
							}
							else {//Middle of row. Still need to print the note part of the row. Mark with a medium line (gridPen).
								//Horizontal line which divides the main part of the row from the notes section of the row one column at a time.
								g.DrawLine(gridPen,
									xPos+ColPos[0],
									yPos+(float)RowHeights[RowsPrinted],
									xPos+(float)ColPos[columns.Count-1]+(float)columns[columns.Count-1].ColWidth,
									yPos+(float)RowHeights[RowsPrinted]);
							}
							//text
							if(rows[RowsPrinted].Cells.Count-1<i) {
								continue;
							}
							switch(columns[i].TextAlign) {
								case HorizontalAlignment.Left:
									format.Alignment=StringAlignment.Near;
									break;
								case HorizontalAlignment.Center:
									format.Alignment=StringAlignment.Center;
									break;
								case HorizontalAlignment.Right:
									format.Alignment=StringAlignment.Far;
									break;
							}
							if(rows[RowsPrinted].Cells[i].ColorText==Color.Empty) {
								textBrush=new SolidBrush(rows[RowsPrinted].ColorText);
							}
							else {
								textBrush=new SolidBrush(rows[RowsPrinted].Cells[i].ColorText);
							}
							if(rows[RowsPrinted].Cells[i].Bold==YN.Yes) {
								cellFont=new Font(cellFont,FontStyle.Bold);
							}
							else if(rows[RowsPrinted].Cells[i].Bold==YN.No) {
								cellFont=new Font(cellFont,FontStyle.Regular);
							}
							else {//unknown.  Use row bold
								if(rows[RowsPrinted].Bold) {
									cellFont=new Font(cellFont,FontStyle.Bold);
								}
								else {
									cellFont=new Font(cellFont,FontStyle.Regular);
								}
							}
							//Some printers will malfunction (BSOD) if print bold colored fonts.  This prevents the error.
							if(textBrush.Color!=Color.Black && cellFont.Bold) {
								cellFont=new Font(cellFont,FontStyle.Regular);
							}
							if(columns[i].TextAlign==HorizontalAlignment.Right) {
								textRect=new RectangleF(
									xPos+(float)ColPos[i]-2,
									yPos,
									(float)columns[i].ColWidth+2,
									(float)RowHeights[RowsPrinted]);
								//shift the rect to account for MS issue with strings of different lengths
								//js- 5/2/11,I don't understand this.  I would like to research it.
								textRect.Location=new PointF
									(textRect.X+g.MeasureString(rows[RowsPrinted].Cells[i].Text,cellFont).Width/textRect.Width,
									textRect.Y);
								//g.DrawString(rows[RowsPrinted].Cells[i].Text,cellFont,textBrush,textRect,format);

							}
							else {
								textRect=new RectangleF(
									xPos+(float)ColPos[i],
									yPos,
									(float)columns[i].ColWidth,
									(float)RowHeights[RowsPrinted]);
								//g.DrawString(rows[RowsPrinted].Cells[i].Text,cellFont,textBrush,textRect,format);
							}
							g.DrawString(rows[RowsPrinted].Cells[i].Text,cellFont,textBrush,textRect,format);
						}
						yPos+=(int)((float)RowHeights[RowsPrinted]);//Move yPos down the length of the row (not the note).
					}
					#endregion RowMainPart
					#region NotePart
					if(rows[RowsPrinted].Note=="") {
						RowsPrinted++;//There is no note. Go to next row.
						isFirstRowOnPage=false;
						continue; 
					}
					//Figure out how much vertical distance the rest of the note will take up.
					int noteHeight;
					int noteW=0;
					format.Alignment=StringAlignment.Near;
					for(int i=NoteSpanStart;i<=NoteSpanStop;i++) {
						noteW+=(int)((float)columns[i].ColWidth);
					}
					if(NoteRemaining=="") {//We are not in the middle of a note.
						NoteRemaining=rows[RowsPrinted].Note;//The note remaining is the whole note.
					}
					noteHeight=(int)g.MeasureString(NoteRemaining,cellFont,noteW,format).Height; //This is how much height the rest of the note will take.
					bool roomForRestOfNote=false;
					//Test to see if there's enough room on the page for the rest of the note.
					if(yPos+noteHeight<bounds.Bottom) {
						roomForRestOfNote=true;
					}
					#region PrintRestOfNote
					if(roomForRestOfNote) { //There is enough room
						//print it
						//draw lines for the rest of the note
						if(noteHeight>0) {
							//left vertical gridline
							if(NoteSpanStart!=0) {
								g.DrawLine(gridPen,
									xPos+(float)ColPos[NoteSpanStart],
									yPos,
									xPos+(float)ColPos[NoteSpanStart],
									yPos+noteHeight);
							}
							//right vertical gridline
							g.DrawLine(gridPen,
								xPos+(float)ColPos[columns.Count-1]+(float)columns[columns.Count-1].ColWidth,
								yPos,
								xPos+(float)ColPos[columns.Count-1]+(float)columns[columns.Count-1].ColWidth,
								yPos+noteHeight);
							//left vertical gridline
							g.DrawLine(gridPen,
								xPos+ColPos[0],
								yPos,
								xPos+ColPos[0],
								yPos+noteHeight);
						}
						//lower horizontal gridline gets marked with the dark lowerPen since this is the end of a row
						g.DrawLine(lowerPen,
							xPos+ColPos[0],
							yPos+noteHeight,
							xPos+(float)ColPos[columns.Count-1]+(float)columns[columns.Count-1].ColWidth,
							yPos+noteHeight);
						//note text
						if(noteHeight>0 && NoteSpanStop>0 && NoteSpanStart<columns.Count) {
							if(rows[RowsPrinted].Bold) {
								cellFont=new Font(cellFont,FontStyle.Bold);
							}
							else {
								cellFont=new Font(cellFont,FontStyle.Regular);
							}
							textBrush=new SolidBrush(rows[RowsPrinted].ColorText);
							textRect=new RectangleF(
								xPos+(float)ColPos[NoteSpanStart]+1,
								yPos,
								(float)ColPos[NoteSpanStop]+(float)columns[NoteSpanStop].ColWidth-(float)ColPos[NoteSpanStart],
								noteHeight);
							g.DrawString(NoteRemaining,cellFont,textBrush,textRect,format);
						}
						NoteRemaining="";
						RowsPrinted++;
						isFirstRowOnPage=false;
						yPos+=noteHeight;
					}
					#endregion PrintRestOfNote
					#region PrintPartOfNote
					else {//The rest of the note will not fit on this page.
						//Print as much as you can.
						noteHeight=bounds.Bottom-yPos;//This is the amount of space remaining.
						if(noteHeight<15) {
							return -1; //If noteHeight is less than this we will get a negative value for the rectangle of space remaining because we subtract 15 from this for the rectangle size when using measureString. This is because one line takes 15, and if there is 1 pixel of height available, measureString will fill it with text, which will then get partly cut off. So when we use measureString we will subtract 15 from the noteHeight.
						}							
						SizeF sizeF;
						int charactersFitted;
						int linesFilled;
						string noteFitted;//This is the part of the note we will print.
						//js- I'd like to incorporate ,StringFormat.GenericTypographic into the MeasureString, but can't find the overload.
						sizeF=g.MeasureString(NoteRemaining,cellFont,new SizeF(noteW,noteHeight-15),format,out charactersFitted,out linesFilled);//Text that fits will be NoteRemaining.Substring(0,charactersFitted).
						noteFitted=NoteRemaining.Substring(0,charactersFitted);
						//draw lines for the part of the note that fits on this page
						if(noteHeight>0) {
							//left vertical gridline
							if(NoteSpanStart!=0) {
								g.DrawLine(gridPen,
									xPos+(float)ColPos[NoteSpanStart],
									yPos,
									xPos+(float)ColPos[NoteSpanStart],
									yPos+noteHeight);
							}
							//right vertical gridline
							g.DrawLine(gridPen,
								xPos+(float)ColPos[columns.Count-1]+(float)columns[columns.Count-1].ColWidth,
								yPos,
								xPos+(float)ColPos[columns.Count-1]+(float)columns[columns.Count-1].ColWidth,
								yPos+noteHeight);
							//left vertical gridline
							g.DrawLine(gridPen,
								xPos+ColPos[0],
								yPos,
								xPos+ColPos[0],
								yPos+noteHeight);
						}
						//lower horizontal gridline gets marked with gridPen since its still the middle of a row (still more note to print)
						g.DrawLine(gridPen,
							xPos+ColPos[0],
							yPos+noteHeight,
							xPos+(float)ColPos[columns.Count-1]+(float)columns[columns.Count-1].ColWidth,
							yPos+noteHeight);
						//note text
						if(noteHeight>0 && NoteSpanStop>0 && NoteSpanStart<columns.Count) {
							if(rows[RowsPrinted].Bold) {
								cellFont=new Font(cellFont,FontStyle.Bold);
							}
							else {
								cellFont=new Font(cellFont,FontStyle.Regular);
							}
							textBrush=new SolidBrush(rows[RowsPrinted].ColorText);
							textRect=new RectangleF(
								xPos+(float)ColPos[NoteSpanStart]+1,
								yPos,
								(float)ColPos[NoteSpanStop]+(float)columns[NoteSpanStop].ColWidth-(float)ColPos[NoteSpanStart],
								noteHeight);
							g.DrawString(noteFitted,cellFont,textBrush,textRect,format);
						}
						NoteRemaining=NoteRemaining.Substring(charactersFitted);
						break;
					}
					#endregion PrintPartOfNote
					#endregion Rows
				}
				#endregion Rows
			}
			finally {
				if(cellFont!=null) {
					cellFont.Dispose();
				}
			}
			if(RowsPrinted==rows.Count) {//done printing
				//set row heights back to screen heights.
				using(Graphics gfx=this.CreateGraphics()) {
					ComputeRows(gfx);
				}
				return yPos;
				
			}
			else{//more pages to print
				
				return -1;
			}
		}

		#endregion Printing

		#region Selections
		///<summary>Use to set a row selected or not.  Can handle values outside the acceptable range.</summary>
		public void SetSelected(int index,bool setValue) {
			if(setValue) {//select specified index
				if(selectionMode==GridSelectionMode.None) {
					throw new Exception("Selection mode is none.");
				}
				if(index<0 || index>rows.Count-1) {//check to see if index is within the valid range of values
					return;//if not, then ignore.
				}
				if(selectionMode==GridSelectionMode.One) {
					selectedIndices.Clear();//clear existing selection before assigning the new one.
				}
				if(!selectedIndices.Contains(index)) {
					selectedIndices.Add(index);
				}
			}
			else {//unselect specified index
				if(selectedIndices.Contains(index)) {
					selectedIndices.Remove(index);
				}
			}
			Invalidate();
		}

		///<summary>Allows setting multiple values all at once</summary>
		public void SetSelected(int[] iArray,bool setValue) {
			if(selectionMode==GridSelectionMode.None) {
				throw new Exception("Selection mode is none.");
			}
			if(selectionMode==GridSelectionMode.One) {
				throw new Exception("Selection mode is one.");
			}
			for(int i=0;i<iArray.Length;i++) {
				if(setValue) {//select specified index
					if(iArray[i]<0 || iArray[i]>rows.Count-1) {//check to see if index is within the valid range of values
						return;//if not, then ignore.
					}
					if(!selectedIndices.Contains(iArray[i])) {
						selectedIndices.Add(iArray[i]);
					}
				}
				else {//unselect specified index
					if(selectedIndices.Contains(iArray[i])) {
						selectedIndices.Remove(iArray[i]);
					}
				}
			}
			Invalidate();
		}

		///<summary>Sets all rows to specified value.</summary>
		public void SetSelected(bool setValue) {
			if(selectionMode==GridSelectionMode.None) {
				throw new Exception("Selection mode is none.");
			}
			if(selectionMode==GridSelectionMode.One && setValue==true) {
				throw new Exception("Selection mode is one.");
			}
			if(selectionMode==GridSelectionMode.OneCell) {
				throw new Exception("Selection mode is OneCell.");
			}
			selectedIndices.Clear();
			if(setValue) {//select all
				for(int i=0;i<rows.Count;i++) {
					selectedIndices.Add(i);
				}
			}
			Invalidate();
		}

		///<summary></summary>
		public void SetSelected(Point setCell) {
			if(selectionMode!=GridSelectionMode.OneCell) {
				throw new Exception("Selection mode must be OneCell.");
			}
			selectedCell=setCell;
			if(editBox!=null) {
				editBox.Dispose();
			}
			if(Columns[selectedCell.X].IsEditable) {
				CreateEditBox();
			}
			Invalidate();
		}

		///<summary>If one row is selected, it returns the index to that row.  If more than one row are selected, it returns the first selected row.  Really only useful for SelectionMode.One.  If no rows selected, returns -1.</summary>
		public int GetSelectedIndex() {
			if(selectedIndices.Count>0) {
				return (int)selectedIndices[0];
			}
			return -1;
		}
		#endregion Selections

		#region Scrolling
		private void LayoutScrollBars() {
			vScroll.Location=new Point(this.Width-vScroll.Width-1,titleHeight+headerHeight+1);
			if(this.hScrollVisible) {
				hScroll.Visible=true;
				vScroll.Height=this.Height-titleHeight-headerHeight-hScroll.Height-2;
				hScroll.Location=new Point(1,this.Height-hScroll.Height-1);
				hScroll.Width=this.Width-vScroll.Width-2;
				if(GridW<hScroll.Width) {
					hScroll.Value=0;
					hScroll.Enabled=false;
				}
				else {
					hScroll.Enabled=true;
					hScroll.Minimum = 0;
					hScroll.Maximum=GridW;
					hScroll.LargeChange=(hScroll.Width<0?0:hScroll.Width);//Don't allow negative width (will throw exception).
					hScroll.SmallChange=(int)(50);
				}

			}
			else {
				hScroll.Visible=false;
				vScroll.Height=this.Height-titleHeight-headerHeight-2;
			}
			if(vScroll.Height<=0) {
				return;
			}
			//hScroll support incomplete
			if(GridH<vScroll.Height) {
				vScroll.Value=0;
				vScroll.Enabled=false;
			}
			else {
				vScroll.Enabled=true;
				vScroll.Minimum = 0;
				vScroll.Maximum=GridH;
				vScroll.LargeChange=vScroll.Height;//it used to crash right here as it tried to assign a negative number.
				vScroll.SmallChange=(int)(14*3.4);//it's not an even number so that it is obvious to user that rows moved
			}
			//vScroll.Value=0;
		}

		private void vScroll_Scroll(object sender,System.Windows.Forms.ScrollEventArgs e) {
			if(editBox!=null) {
				editBox.Dispose();
			}
			Invalidate();
			this.Focus();
		}

		private void hScroll_Scroll(object sender,System.Windows.Forms.ScrollEventArgs e) {
			//if(UpDownKey) return;
			Invalidate();
			this.Focus();
		}

		///<summary>Usually called after entering a new list to automatically scroll to the end.</summary>
		public void ScrollToEnd() {
			ScrollValue=vScroll.Maximum;//this does all error checking and invalidates
		}
		#endregion Scrolling

		#region Sorting
		///<summary>Set sortedByColIdx to -1 to clear sorting. Copied from SortByColumn. No need to call fill grid after calling this.  Also used in PatientPortalManager.</summary>
		public void SortForced(int sortedByColIdx,bool isAsc) {
			sortedIsAscending=isAsc;
			sortedByColumnIdx=sortedByColIdx;
			if(sortedByColIdx==-1) {
				return;
			}
			List<ODGridRow> rowsSorted=new List<ODGridRow>();
			for(int i=0;i<rows.Count;i++) {
				rowsSorted.Add(rows[i]);
			}
			if(columns[sortedByColumnIdx].SortingStrategy==GridSortingStrategy.StringCompare) {
				rowsSorted.Sort(SortStringCompare);
			}
			else if(columns[sortedByColumnIdx].SortingStrategy==GridSortingStrategy.DateParse) {
				rowsSorted.Sort(SortDateParse);
			}
			else if(columns[sortedByColumnIdx].SortingStrategy==GridSortingStrategy.ToothNumberParse) {
				rowsSorted.Sort(SortToothNumberParse);
			}
			else if(columns[sortedByColumnIdx].SortingStrategy==GridSortingStrategy.AmountParse) {
				rowsSorted.Sort(SortAmountParse);
			}
			BeginUpdate();
			rows.Clear();
			for(int i=0;i<rowsSorted.Count;i++) {
				rows.Add(rowsSorted[i]);
			}
			EndUpdate();
			sortedByColumnIdx=sortedByColIdx;//Must be set again since set to -1 in EndUpdate();
		}

		///<summary>Gets run on mouse down on a column header.</summary>
		private void SortByColumn(int mouseDownCol) {
			if(mouseDownCol==-1) {
				return;
			}
			if(sortedByColumnIdx==mouseDownCol) {//already sorting by this column
				sortedIsAscending=!sortedIsAscending;//switch ascending/descending.
			}
			else {
				sortedIsAscending=true;//start out ascending
				sortedByColumnIdx=mouseDownCol;
			}
			List<ODGridRow> rowsSorted=new List<ODGridRow>();
			for(int i=0;i<rows.Count;i++) {
				rowsSorted.Add(rows[i]);
			}
			if(columns[sortedByColumnIdx].SortingStrategy==GridSortingStrategy.StringCompare) {
				rowsSorted.Sort(SortStringCompare);
			}
			else if(columns[sortedByColumnIdx].SortingStrategy==GridSortingStrategy.DateParse) {
				rowsSorted.Sort(SortDateParse);
			}
			else if(columns[sortedByColumnIdx].SortingStrategy==GridSortingStrategy.ToothNumberParse) {
				rowsSorted.Sort(SortToothNumberParse);
			}
			else if(columns[sortedByColumnIdx].SortingStrategy==GridSortingStrategy.AmountParse) {
				rowsSorted.Sort(SortAmountParse);
			}
			BeginUpdate();
			rows.Clear();
			for(int i=0;i<rowsSorted.Count;i++) {
				rows.Add(rowsSorted[i]);
			}
			EndUpdate();
			sortedByColumnIdx=mouseDownCol;//Must be set again since set to -1 in EndUpdate();
		}

		private int SortStringCompare(ODGridRow row1,ODGridRow row2) {
			return (sortedIsAscending?1:-1)*row1.Cells[sortedByColumnIdx].Text.CompareTo(row2.Cells[sortedByColumnIdx].Text);
		}

		private int SortDateParse(ODGridRow row1,ODGridRow row2) {
			string raw1=row1.Cells[sortedByColumnIdx].Text;
			string raw2=row2.Cells[sortedByColumnIdx].Text;
			DateTime date1=DateTime.MinValue;
			DateTime date2=DateTime.MinValue;
			//TryParse is a much faster operation than Parse in the event that the input won't parse to a date.
			if(DateTime.TryParse(raw1,out date1) &&
				DateTime.TryParse(raw2,out date2)) {
				return (sortedIsAscending?1:-1)*date1.CompareTo(date2);
			}
			else { //One of the inputs is not a date so default string compare.
				return SortStringCompare(row1,row2);
			}
		}

		private int SortToothNumberParse(ODGridRow row1,ODGridRow row2) {
			//remember that teeth could be in international format.
			//fail gracefully
			string raw1=row1.Cells[sortedByColumnIdx].Text;
			string raw2=row2.Cells[sortedByColumnIdx].Text;
			if(!Tooth.IsValidEntry(raw1) && !Tooth.IsValidEntry(raw2)) {//both invalid
				return 0;
			}
			int retVal=0;
			if(!Tooth.IsValidEntry(raw1)) {//only first invalid
				retVal=-1; ;
			}
			else if(!Tooth.IsValidEntry(raw2)) {//only second invalid
				retVal=1; ;
			}
			else {//both valid
				string tooth1=Tooth.FromInternat(raw1);
				string tooth2=Tooth.FromInternat(raw2);
				int toothInt1=Tooth.ToInt(tooth1);
				int toothInt2=Tooth.ToInt(tooth2);
				retVal=toothInt1.CompareTo(toothInt2);
			}
			return (sortedIsAscending?1:-1)*retVal;
		}

		private int SortAmountParse(ODGridRow row1,ODGridRow row2) {
			string raw1=row1.Cells[sortedByColumnIdx].Text;
			string raw2=row2.Cells[sortedByColumnIdx].Text;
			Decimal amt1=0;
			Decimal amt2=0;
			if(raw1!="") {
				try {
					amt1=Decimal.Parse(raw1);
				}
				catch {
					return 0;//shouldn't happen
				}
			}
			if(raw2!="") {
				try {
					amt2=Decimal.Parse(raw2);
				}
				catch {
					return 0;//shouldn't happen
				}
			}
			return (sortedIsAscending?1:-1)*amt1.CompareTo(amt2);
		}

		#endregion Sorting

		#region MouseEvents

		///<summary></summary>
		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
			lastButtonPressed=e.Button;//used in the click event.
			if(e.Button==MouseButtons.Right) {
				if(selectedIndices.Count>0) {//if there are already rows selected, then ignore right click
					return;
				}
				//otherwise, row will be selected. Useful when using context menu.
			}
			MouseIsDown=true;
			MouseDownRow=PointToRow(e.Y);
			MouseDownCol=PointToCol(e.X);
			if(e.Y < 1+titleHeight) {//mouse down was in the title section
				return;
			}
			if(e.Y < 1+titleHeight+headerHeight) {//mouse down was on a column header
				mouseIsDownInHeader=true;
				if(allowSortingByColumn) {
					if(MouseDownCol==-1) {
						return;
					}
					SortByColumn(MouseDownCol);
					Invalidate();
					return;
				}
				else {
					return;
				}
			}
			if(MouseDownRow==-1) {//mouse down was below the grid rows
				return;
			}
			if(MouseDownCol==-1) {//mouse down was to the right of columns
				return;
			}
			if(!allowSelection) {
				return;//clicks do not trigger selection of rows, but cell click event still gets fired
			}
			switch(selectionMode) {
				case GridSelectionMode.None:
					return;
				case GridSelectionMode.One:
					selectedIndices.Clear();
					selectedIndices.Add(MouseDownRow);
					break;
				case GridSelectionMode.OneCell:
					selectedIndices.Clear();
					//Point oldSelectedCell=selectedCell;
					//if(oldSelectedCell.X!=selectedCell.X || oldSelectedCell.Y!=selectedCell.Y){
					if(editBox!=null) {
						editBox.Dispose();//a lot happens right here, including a FillGrid() which sets selectedCell to -1,-1
					}
					selectedCell=new Point(MouseDownCol,MouseDownRow);
					if(Columns[selectedCell.X].IsEditable) {
						CreateEditBox();
						//When the edit text box was created, added to the control, and given focus, the chain of events stops and the OnClick event never gets fired.
						//We can guarantee that the user did in fact click on a cell at this point in the mouse down event.
						OnClick(e);
					}
					//}
					break;
				case GridSelectionMode.MultiExtended:
					if(ControlIsDown) {
						//we need to remember exactly which rows were selected the moment the mouse down started.
						//Then, if the mouse gets dragged up or down, the rows between mouse start and mouse end
						//will be set to the opposite of these remembered values.
						selectedIndicesWhenMouseDown=new ArrayList(selectedIndices);
						if(selectedIndices.Contains(MouseDownRow)) {
							selectedIndices.Remove(MouseDownRow);
						}
						else {
							selectedIndices.Add(MouseDownRow);
						}
					}
					else if(ShiftIsDown) {
						if(selectedIndices.Count==0) {
							selectedIndices.Add(MouseDownRow);
						}
						else {
							int fromRow=(int)selectedIndices[0];
							selectedIndices.Clear();
							if(MouseDownRow<fromRow) {//dragging down
								for(int i=MouseDownRow;i<=fromRow;i++) {
									selectedIndices.Add(i);
								}
							}
							else {
								for(int i=fromRow;i<=MouseDownRow;i++) {
									selectedIndices.Add(i);
								}
							}
						}
					}
					else {//ctrl or shift not down
						selectedIndices.Clear();
						selectedIndices.Add(MouseDownRow);
					}
					break;
			}
			Invalidate();
		}

		///<summary></summary>
		protected override void OnMouseUp(MouseEventArgs e) {
			base.OnMouseUp(e);
			//if(e.Button==MouseButtons.Right){
			//	return;
			//}
			MouseIsDown=false;
			mouseIsDownInHeader=false;
		}

		///<summary>When selection mode is OneCell, and user clicks in a column that isEditable, then this edit box will appear.  Pass in the location from the click event so that we can determine where to place the text cursor in the box.</summary>
		private void CreateEditBox() {
			if(-vScroll.Value+1+titleHeight+headerHeight+RowLocs[selectedCell.Y]+RowHeights[selectedCell.Y]>this.DisplayRectangle.Bottom) {//If new edit box location is below the display screen
				int onScreenPixels=vScroll.Value+DisplayRectangle.Height-titleHeight-headerHeight-(RowLocs[selectedCell.Y]);
				int offScreenPixels=RowHeights[selectedCell.Y]-onScreenPixels;
				if(offScreenPixels>0) {
					ScrollValue+=offScreenPixels;//Scrolling down
				}
			}
			else if(-vScroll.Value+1+titleHeight+headerHeight+RowLocs[selectedCell.Y]<this.DisplayRectangle.Top+titleHeight+headerHeight) {//If new edit box location is above the display screen
				ScrollToIndex(selectedCell.Y);//Scrolling up
			}
			editBox=new TextBox();
			//The problem is that it ignores the height.
			editBox.Multiline=true;
			editBox.Font=new Font(FontFamily.GenericSansSerif,cellFontSize);
			editBox.Size=new Size(Columns[selectedCell.X].ColWidth+1,RowHeights[selectedCell.Y]+1);
			editBox.Location=new Point(-hScroll.Value+1+ColPos[selectedCell.X],
				-vScroll.Value+1+titleHeight+headerHeight+RowLocs[selectedCell.Y]);
			editBox.Text=Rows[selectedCell.Y].Cells[selectedCell.X].Text;
			editBox.TextChanged+=new EventHandler(editBox_TextChanged);
			editBox.GotFocus+=new EventHandler(editBox_GotFocus);
			editBox.LostFocus+=new EventHandler(editBox_LostFocus);
			editBox.KeyDown+=new KeyEventHandler(editBox_KeyDown);
			editBox.KeyUp+=new KeyEventHandler(editBox_KeyUp);
			if(Columns[selectedCell.X].TextAlign==HorizontalAlignment.Right) {
				editBox.TextAlign=HorizontalAlignment.Right;
			}
			editBox.AcceptsTab=true;//Allow tab navigation in the ODGrid. Necessary when enter and up/down keys navigate within a cell (EditableAcceptsCR).
			if(editableAcceptsCR) {//Allow the edit box to handle carriage returns/multiline text.
				editBox.AcceptsReturn=true;
			}
			this.Controls.Add(editBox);
			if(!editableAcceptsCR) {
				editBox.SelectAll();//Only select all when not multiline (editableAcceptsCR) i.e. proc list for editing fees selects all for easy overwriting.
			}
			editBox.Focus();
			//Set the cell of the current editBox so that the value of that cell is saved when it looses focus (used for mouse click).
			oldSelectedCell=new Point(selectedCell.X,selectedCell.Y);
		}

		void editBox_LostFocus(object sender,EventArgs e) {
			//editBox_Leave wouldn't catch all scenarios
			OnCellLeave(oldSelectedCell.X,oldSelectedCell.Y);//this is the only place where OnCellLeave gets called.
			if(!editBox.Disposing || !editBox.IsDisposed) {
				editBox.Dispose();
				editBox=null;
			}
		}

		void editBox_GotFocus(object sender,EventArgs e) {
			OnCellEnter(oldSelectedCell.X,oldSelectedCell.Y);//this is the only place where OnCellEnter gets called.
		}

		void editBox_KeyDown(object sender,KeyEventArgs e) {
			if(e.Shift && e.KeyCode == Keys.Enter) {
				Rows[selectedCell.Y].Cells[selectedCell.X].Text+="\r\n";
				return;
			}
			if(e.KeyCode==Keys.Enter) {//usually move to the next cell
				if(editableAcceptsCR) {//When multiline it inserts a carriage return instead of moving to the next cell.
					return;
				}
				editBox_NextCell();
			}
			if(e.KeyCode==Keys.Down) {
				if(editableAcceptsCR) {//When multiline it moves down inside the text instead of down to the next cell.
					return;
				}
				if(selectedCell.Y<rows.Count-1) {
					editBox.Dispose();
					editBox=null;
					//OnCellLeave(selectedCell.X,selectedCell.Y);
					selectedCell=new Point(selectedCell.X,selectedCell.Y+1);
					CreateEditBox();
				}
			}
			if(e.KeyCode==Keys.Up) {
				if(editableAcceptsCR) {//When multiline it moves up inside the text instead of up to the next cell.
					return;
				}
				if(selectedCell.Y>0) {
					editBox.Dispose();
					editBox=null;
					//OnCellLeave(selectedCell.X,selectedCell.Y);
					selectedCell=new Point(selectedCell.X,selectedCell.Y-1);
					CreateEditBox();
				}
			}
			if(e.KeyCode==Keys.Tab) {
				editBox_NextCell();
			}
		}
		
		private void editBox_NextCell() {
			editBox.Dispose();//This fires editBox_LostFocus, which is where we call OnCellLeave.
			editBox=null;
			//OnCellLeave(selectedCell.X,selectedCell.Y);
			//find the next editable cell to the right.
			int nextCellToRight=-1;
			for(int i=selectedCell.X+1;i<columns.Count;i++) {
				if(columns[i].IsEditable) {
					nextCellToRight=i;
					break;
				}
			}
			if(nextCellToRight!=-1) {
				selectedCell=new Point(nextCellToRight,selectedCell.Y);
				CreateEditBox();
				return;
			}
			//can't move to the right, so attempt to move down.
			if(selectedCell.Y==rows.Count-1) {
				return;//can't move down
			}
			nextCellToRight=-1;
			for(int i=0;i<columns.Count;i++) {
				if(columns[i].IsEditable) {
					nextCellToRight=i;
					break;
				}
			}
			//guaranteed to have a value
			selectedCell=new Point(nextCellToRight,selectedCell.Y+1);
			CreateEditBox();
		}
		
		void editBox_KeyUp(object sender,KeyEventArgs e) {
			if(editBox==null) {
				return;
			}
			if(editBox.Text=="") {
				return;
			}
			Graphics g=CreateGraphics();
			Font cellFont=new Font(FontFamily.GenericSansSerif,cellFontSize);
			int cellH=(int)((1.03)*(float)(g.MeasureString(editBox.Text+"\r\n",cellFont,editBox.Width).Height))+4;
			if(cellH < EDITABLE_ROW_HEIGHT) {//if it's less than one line
			  cellH=EDITABLE_ROW_HEIGHT;//set it to one line
			}
			if(cellH>editBox.Height) {//it needs to grow so redraw it. Only measures the text of this one cell so checking here for shrinking would cause unnecessary redraws and other bugs.
				rows[selectedCell.Y].Cells[selectedCell.X].Text=editBox.Text;
				Point cellSelected=new Point(selectedCell.X,selectedCell.Y);
				int selectionStart=editBox.SelectionStart;
				List<ODGridColumn> listCols=new List<ODGridColumn>();
				for(int i=0;i<columns.Count;i++) {
					listCols.Add(new ODGridColumn(columns[i].Heading,columns[i].ColWidth,columns[i].IsEditable));
					listCols[i].TextAlign=columns[i].TextAlign;
				}
				List<ODGridRow> listRows=new List<ODGridRow>();
				ODGridRow row;
				for(int i=0;i<rows.Count;i++) {
					row=new ODGridRow();
					for(int j=0;j<rows[i].Cells.Count;j++) {
						row.Cells.Add(new ODGridCell(rows[i].Cells[j].Text));
					}
					row.Tag=rows[i].Tag;
					listRows.Add(row);
				}
				BeginUpdate();
				columns.Clear();
				for(int i=0;i<listCols.Count;i++) {
					columns.Add(new ODGridColumn(listCols[i].Heading,listCols[i].ColWidth,listCols[i].IsEditable));
					columns[i].TextAlign=listCols[i].TextAlign;
				}
				rows.Clear();
				for(int i=0;i<listRows.Count;i++) {
					row=new ODGridRow();
					for(int j=0;j<listRows[i].Cells.Count;j++) {
						row.Cells.Add(listRows[i].Cells[j].Text);
					}
					row.Tag=listRows[i].Tag;
					rows.Add(row);
				}
				EndUpdate();
				if(editBox!=null) {
					editBox.Dispose();
				}
				selectedCell=cellSelected;
				CreateEditBox();
				if(editBox!=null) {
					editBox.SelectionStart=selectionStart;
					editBox.SelectionLength=0;
				}
			}
			g.Dispose();
			cellFont.Dispose();
		}

		void editBox_TextChanged(object sender,EventArgs e) {
			if(editBox!=null) {
				Rows[selectedCell.Y].Cells[selectedCell.X].Text=editBox.Text;
			}
			OnCellTextChanged();
		}

		///<summary>The purpose of this is to allow dragging to select multiple rows.  Only makes sense if selectionMode==MultiExtended.  Doesn't matter whether ctrl is down, because that only affects the mouse down event.</summary>
		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);
			MouseIsOver=true;
			if(!MouseIsDown) {
				return;
			}
			if(selectionMode!=GridSelectionMode.MultiExtended) {
				return;
			}
			if(!allowSelection) {
				return;//dragging does not change selection of rows
			}
			if(mouseIsDownInHeader) {
				return;//started drag in header, so not allowed to select anything.
			}
			int curRow=PointToRow(e.Y);
			if(curRow==-1 || curRow==MouseDownRow) {
				return;
			}
			//because mouse might have moved faster than computer could keep up, we have to loop through all rows between
			if(ControlIsDown) {
				if(selectedIndicesWhenMouseDown==null) {
					selectedIndices=new ArrayList();
				}
				else {
					selectedIndices=new ArrayList(selectedIndicesWhenMouseDown);
				}
			}
			else {
				selectedIndices=new ArrayList();
			}
			if(MouseDownRow<curRow) {//dragging down
				for(int i=MouseDownRow;i<=curRow;i++) {
					if(i==-1) {
						continue;
					}
					if(selectedIndices.Contains(i)) {
						selectedIndices.Remove(i);
					}
					else {
						selectedIndices.Add(i);
					}
				}
			}
			else {//dragging up
				for(int i=curRow;i<=MouseDownRow;i++) {
					if(selectedIndices.Contains(i)) {
						selectedIndices.Remove(i);
					}
					else {
						selectedIndices.Add(i);
					}
				}
			}
			Invalidate();
		}

		///<summary></summary>
		protected override void OnMouseEnter(EventArgs e) {
			base.OnMouseEnter(e);
			MouseIsOver=true;
		}

		///<summary></summary>
		protected override void OnMouseLeave(EventArgs e) {
			base.OnMouseLeave(e);
			MouseIsOver=false;
		}

		private void vScroll_MouseEnter(Object sender,EventArgs e) {
			MouseIsOver=true;
		}

		private void vScroll_MouseLeave(Object sender,EventArgs e) {
			MouseIsOver=false;
		}

		private void vScroll_MouseMove(Object sender,MouseEventArgs e) {
			MouseIsOver=true;
		}

		private void hScroll_MouseEnter(Object sender,EventArgs e) {
			MouseIsOver=true;
		}

		private void hScroll_MouseLeave(Object sender,EventArgs e) {
			MouseIsOver=false;
		}

		private void hScroll_MouseMove(Object sender,MouseEventArgs e) {
			MouseIsOver=true;
		}

		private void Parent_MouseWheel(Object sender,MouseEventArgs e) {
			if(MouseIsOver) {
				//this.ac
				this.Select();//?
				//this.Focus();
			}
		}

		///<summary></summary>
		protected override void OnMouseWheel(MouseEventArgs e) {
			base.OnMouseWheel(e);
			ScrollValue-=e.Delta/3;
		}

		#endregion MouseEvents

		#region KeyEvents
		///<summary></summary>
		protected override void OnKeyDown(KeyEventArgs e) {
			base.OnKeyDown(e);
			if(e.KeyCode==Keys.ControlKey) {
				ControlIsDown=true;
			}
			if(e.KeyCode==Keys.ShiftKey) {
				ShiftIsDown=true;
			}
		}

		///<summary></summary>
		protected override void OnKeyUp(KeyEventArgs e) {
			base.OnKeyUp(e);
			if(e.KeyCode==Keys.ControlKey) {
				ControlIsDown=false;
			}
			if(e.KeyCode==Keys.ShiftKey) {
				ShiftIsDown=false;
			}
		}

		/// <summary>If the Ctrl key down is not being captured by the grid because it doesn't have focus, then this automatically handles it.  The only thing you have to do to make it work is to turn on KeyPreview for the parent form.</summary>
		private void Parent_KeyDown(Object sender,KeyEventArgs e) {
			if(e.KeyCode==Keys.ControlKey) {
				ControlIsDown=true;
			}
			if(e.KeyCode==Keys.ShiftKey) {
				ShiftIsDown=true;
			}
			if(selectionMode==GridSelectionMode.One) {
				if(e.KeyCode==Keys.Down) {
					if(selectedIndices.Count>0 && (int)selectedIndices[0] < rows.Count-1) {
						int prevRow=(int)selectedIndices[0];
						selectedIndices.Clear();
						selectedIndices.Add(prevRow+1);
						hScroll.Value=hScroll.Minimum;
					}
				}
				else if(e.KeyCode==Keys.Up) {
					if(selectedIndices.Count>0 && (int)selectedIndices[0] > 0) {
						int prevRow=(int)selectedIndices[0];
						selectedIndices.Clear();
						selectedIndices.Add(prevRow-1);
					}
				}
			}
		}

		/// <summary>If the Ctrl key down is not being captured by the grid because it doesn't have focus, then this automatically handles it.  The only thing you have to do to make it work is to turn on KeyPreview for the parent form.</summary>
		private void Parent_KeyUp(Object sender,KeyEventArgs e) {
			if(e.KeyCode==Keys.ControlKey) {
				ControlIsDown=false;
			}
			if(e.KeyCode==Keys.ShiftKey) {
				ShiftIsDown=false;
			}
			//if(e.KeyCode==Keys.Down | e.KeyCode==Keys.Up){
			//	UpDownKey=false;
			//	hScroll.Value=hScroll.Minimum;
			//}
		}

		protected void OnCellTextChanged() {
			if(CellTextChanged!=null) {
				CellTextChanged(this,new EventArgs());
			}
		}

		protected void OnCellLeave(int col,int row) {
			if(CellLeave!=null) {
				CellLeave(this,new ODGridClickEventArgs(col,row,MouseButtons.None));
			}
		}

		protected void OnCellEnter(int col,int row) {
			if(CellEnter!=null) {
				CellEnter(this,new ODGridClickEventArgs(col,row,MouseButtons.None));
			}
		}
		#endregion KeyEvents






















	}


	///<summary></summary>
	public class ODGridClickEventArgs {
		private int col;
		private int row;
		private MouseButtons button;

		///<summary></summary>
		public ODGridClickEventArgs(int col,int row,MouseButtons button) {
			this.col=col;
			this.row=row;
			this.button=button;
		}

		///<summary></summary>
		public int Row {
			get {
				return row;
			}
		}

		///<summary></summary>
		public int Col {
			get {
				return col;
			}
		}

		///<summary>Gets which mouse button was pressed.</summary>
		public MouseButtons Button {
			get {
				return button;
			}
		}

	}

	///<summary>Specifies the selection behavior of an ODGrid.</summary>   
	//[ComVisible(true)]
	public enum GridSelectionMode {
		///<summary>0-No items can be selected.</summary>  
		None=0,
		///<summary>1-Only one row can be selected.</summary>  
		One=1,
		///<summary>2-Only one cell can be selected.</summary>
		OneCell=2,
		///<summary>3-Multiple items can be selected, and the user can use the SHIFT, CTRL, and arrow keys to make selections</summary>   
		MultiExtended=3,
	}

}






/*This is a template of typical grid code which can be quickly pasted into any form.
 
		using OpenDental.UI;

		FillGrid(){
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("Table",""),);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("Table",""),);
			gridMain.Columns.Add(col);
			 
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<List.Length;i++){
				row=new ODGridRow();
				row.Cells.Add("");
				row.Cells.Add("");
			  
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

*/