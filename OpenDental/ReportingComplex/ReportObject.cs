using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental.ReportingComplex {
	///<summary>There is one ReportObject for each element of an ODReport that gets printed on the page.  There are many different kinds of reportObjects.</summary>
	public class ReportObject{
		private string _sectionName;
		private Point _location;
		private Size _size;
		private string _name;
		private ReportObjectKind _reportObjectKind;
		private Font _font;
		private ContentAlignment _contentAlignment;
		private Color _foreColor;
		private string _staticText;
		private string _stringFormat;
		private bool _suppressIfDuplicate;
		private float _floatLineThickness;
		private FieldDefKind _fieldDefKind;
		private FieldValueType _fieldValueType;
		private SpecialFieldType _specialFieldType;
		private SummaryOperation _summaryOperation;
		private LineOrientation _lineOrientation;
		private LinePosition _linePosition;
		private int _intLinePercent;
		private int _offSetX;
		private int _offSetY;
		private bool _isUnderlined;
		private string _summarizedFieldName;
		private string _dataFieldName;
		private SummaryOrientation _summaryOrientation;
		private List<int> _summaryGroupValues;
		

#region Properties

		///<summary>The name of the section to which this object is attached.  For lines and boxes that span multiple sections, this is the section in which the upper part of the object resides.</summary>
		public string SectionName{
			get{
				return _sectionName;
			}
			set{
				_sectionName=value;
			}
		}

		///<summary>Location within the section. Frequently, y=0</summary>
		public Point Location{
			get{
				return _location;
			}
			set{
				_location=value;
			}
		}

		///<summary>Typically not set since this is set by a helper function when important properties for size change.</summary>
		public Size Size{
			get{
				return _size;
			}
			set{
				_size=value;
			}
		}

		///<summary>The unique name of the ReportObject.</summary>
		public string Name{
			get{
				return _name;
			}
			set{
				_name=value;
			}
		}

		///<summary>For instance, FieldObject, or TextObject.</summary>
		public ReportObjectKind ReportObjectKind{
			get{
				return _reportObjectKind;
			}
			set{
				_reportObjectKind=value;
			}
		}

		///<summary>Setting this will also set the size.</summary>
		public Font Font{
			get{
				return _font;
			}
			set{
				_font=value;
				_size=CalculateNewSize(_staticText,_font);
			}
		}

		///<summary>Horizontal alignment of the text.</summary>
		public ContentAlignment ContentAlignment{
			get{
				return _contentAlignment;
			}
			set{
				_contentAlignment=value;
			}
		}

		///<summary>Can be used for text color or for line color.</summary>
		public Color ForeColor{
			get{
				return _foreColor;
			}
			set{
				_foreColor=value;
			}
		}

		///<summary>The text to display for a TextObject. Setting this will also set the size.</summary>
		public string StaticText{
			get{
				return _staticText;
			}
			set{
				_staticText=value;
				_size=CalculateNewSize(_staticText,_font);
			}
		}

		///<summary>For a FieldObject, a C# format string that specifies how to print dates, times, numbers, and currency based on the country or on a custom format.</summary>
		///<remarks>There are a LOT of options for this string.  Look in C# help under Standard Numeric Format Strings, Custom Numeric Format Strings, Standard DateTime Format Strings, Custom DateTime Format Strings, and Enumeration Format Strings.  Once users are allowed to edit reports, we will assemble a help page with all of the common options. The best options are "n" for number, and "d" for date.</remarks>
		public string StringFormat{
			get{
				return _stringFormat;
			}
			set{
				_stringFormat=value;
			}
		}

		///<summary>Suppresses this field if the field for the previous record was the same.  Only used with data fields.  E.g. So that a query ordered by a date column doesn't print the same date over and over.</summary>
		public bool SuppressIfDuplicate{
			get{
				return _suppressIfDuplicate;
			}
			set{
				_suppressIfDuplicate=value;
			}
		}

		///<summary></summary>
		public float FloatLineThickness{
			get{
				return _floatLineThickness;
			}
			set{
				_floatLineThickness=value;
			}
		}

		///<summary>Used to determine whether the line is vertical or horizontal.</summary>
		public LineOrientation LineOrientation {
			get {
				return _lineOrientation;
			}
			set {
				_lineOrientation=value;
			}
		}

		///<summary>Used to determine intial starting position of the line.</summary>
		public LinePosition LinePosition {
			get {
				return _linePosition;
			}
			set {
				_linePosition=value;
			}
		}

		///<summary>Used to determine what percentage of the section the line will draw on.</summary>
		public int IntLinePercent {
			get {
				return _intLinePercent;
			}
			set {
				_intLinePercent=value;
			}
		}

		///<summary>Used to offset lines, boxes, and text by a specific number of pixels.</summary>
		public int OffSetX {
			get {
				return _offSetX;
			}
			set {
				_offSetX=value;
			}
		}

		///<summary>Used to offset lines, boxes, and text by a specific number of pixels.</summary>
		public int OffSetY {
			get {
				return _offSetY;
			}
			set {
				_offSetY=value;
			}
		}

		///<summary>Used to underline text objects and titles.</summary>
		public bool IsUnderlined {
			get {
				return _isUnderlined;
			}
			set {
				_isUnderlined=value;
			}
		}

		///<summary>The kind of field, like FormulaField, SummaryField, or DataTableField.</summary>
		public FieldDefKind FieldDefKind{
			get{
				return _fieldDefKind;
			}
			set{
				_fieldDefKind=value;
			}
		}

		///<summary>The value type of field, like string or datetime.</summary>
		public FieldValueType FieldValueType{
			get{
				return _fieldValueType;
			}
			set{
				_fieldValueType=value;
			}
		}

		///<summary>For FieldKind=FieldDefKind.SpecialField, this is the type.  eg. pagenumber</summary>
		public SpecialFieldType SpecialFieldType{
			get{
				return _specialFieldType;
			}
			set{
				_specialFieldType=value;
			}
		}

		///<summary>For FieldKind=FieldDefKind.SummaryField, the summary operation type.</summary>
		public SummaryOperation SummaryOperation{
			get{
				return _summaryOperation;
			}
			set{
				_summaryOperation=value;
			}
		}

		///<summary>For FieldKind=FieldDefKind.SummaryField, the name of the dataField that is being summarized.  This might later be changed to refer to a ReportObject name instead (or maybe not).</summary>
		public string SummarizedField{
			get{
				return _summarizedFieldName;
			}
			set{
				_summarizedFieldName=value;
			}
		}

		///<summary>For objectKind=ReportObjectKind.FieldObject, the name of the dataField column.</summary>
		public string DataField{
			get{
				return _dataFieldName;
			}
			set{
				_dataFieldName=value;
			}
		}

		///<summary>The location of the summary label around the summary field</summary>
		public SummaryOrientation SummaryOrientation {
			get {
				return _summaryOrientation;
			}
			set {
				_summaryOrientation=value;
			}
		}

		///<summary>The numeric value of the QueryGroup. Used when summarizing groups of queries.</summary>
		public List<int> SummaryGroups {
			get {
				return _summaryGroupValues;
			}
			set {
				_summaryGroupValues=value;
			}
		}

#endregion

		///<summary>Default constructor.</summary>
		public ReportObject(){

		}

		///<summary>Overload for TextObject with offsets.</summary>
		public ReportObject(string name,string sectionName,Point location,Size size,string staticText,Font font,ContentAlignment contentAlignment,int offSetX,int offSetY){
			_name=name;
			_sectionName=sectionName;
			_location=location;
			_size=size;
			_staticText=staticText;
			_font=font;
			_contentAlignment=contentAlignment;
			_offSetX=offSetX;
			_offSetY=offSetY;
			_foreColor=Color.Black;
			_reportObjectKind=ReportObjectKind.TextObject;
		}

		///<summary>Overload for TextObject.</summary>
		public ReportObject(string name,string sectionName,Point location,Size size,string staticText,Font font,ContentAlignment contentAlignment) {
			_name=name;
			_sectionName=sectionName;
			_location=location;
			_size=size;
			_staticText=staticText;
			_font=font;
			_contentAlignment=contentAlignment;
			_foreColor=Color.Black;
			_reportObjectKind=ReportObjectKind.TextObject;
		}

		///<summary>Overload for BoxObject.</summary>
		public ReportObject(string name,string sectionName,Color color,float lineThickness,int offSetX,int offSetY) {
			_name=name;
			_sectionName=sectionName;
			_foreColor=color;
			_floatLineThickness=lineThickness;
			_offSetX=offSetX;
			_offSetY=offSetY;
			_reportObjectKind=ReportObjectKind.BoxObject;
		}

		///<summary>Overload for LineObject.</summary>
		public ReportObject(string name,string sectionName,Color color,float lineThickness,LineOrientation lineOrientation,LinePosition linePosition,int linePercent,int offSetX,int offSetY) {
			_name=name;
			_sectionName=sectionName;
			_foreColor=color;
			_floatLineThickness=lineThickness;
			_lineOrientation=lineOrientation;
			_linePosition=linePosition;
			_intLinePercent=linePercent;
			_offSetX=offSetX;
			_offSetY=offSetY;
			_reportObjectKind=ReportObjectKind.LineObject;
		}

		///<summary>Overload for DataTableField ReportObject</summary>
		public ReportObject(string name,string sectionName,Point location,Size size
			,string dataFieldName,FieldValueType fieldValueType
			,Font font,ContentAlignment contentAlignment,string stringFormat) {
			_name=name;
			_sectionName=sectionName;
			_location=location;
			_size=size;
			_font=font;
			_contentAlignment=contentAlignment;
			_stringFormat=stringFormat;
			_fieldDefKind=FieldDefKind.DataTableField;
			_dataFieldName=dataFieldName;
			_fieldValueType=fieldValueType;
			//defaults:
			_foreColor=Color.Black;
			_reportObjectKind=ReportObjectKind.FieldObject;
		}

		///<summary>Overload for SummaryField ReportObject</summary>
		public ReportObject(string name,string sectionName,Point location,Size size,SummaryOperation summaryOperation,string summarizedFieldName,Font font,ContentAlignment contentAlignment,string stringFormat) {
			_name=name;
			_sectionName=sectionName;
			_location=location;
			_size=size;
			_font=font;
			_contentAlignment=contentAlignment;
			_stringFormat=stringFormat;
			_fieldDefKind=FieldDefKind.SummaryField;
			_fieldValueType=FieldValueType.Number;
			_summaryOperation=summaryOperation;
			_summarizedFieldName=summarizedFieldName;
			//defaults:
			_foreColor=Color.Black;
			_reportObjectKind=ReportObjectKind.FieldObject;
		}

		///<summary>Overload for GroupSummary ReportObject</summary>
		public ReportObject(string name,string sectionName,Point location,Size size,Color color,string summarizedFieldName,Font font,SummaryOperation summaryOperation,int offSetX,int offSetY) {
			_name=name;
			_sectionName=sectionName;
			_location=location;
			_size=size;
			_font=font;
			_fieldDefKind=FieldDefKind.SummaryField;
			_fieldValueType=FieldValueType.Number;
			_summaryOperation=summaryOperation;
			_summarizedFieldName=summarizedFieldName;
			_offSetX=offSetX;
			_offSetY=offSetY;
			//defaults:
			_contentAlignment=ContentAlignment.MiddleLeft;
			_foreColor=color;
			_reportObjectKind=ReportObjectKind.TextObject;
		}

		///<summary>Overload for SpecialField ReportObject</summary>
		public ReportObject(string name,string sectionName,Point location,Size size,FieldValueType fieldValueType,SpecialFieldType specialType,Font font,ContentAlignment contentAlignment,string stringFormat) {
			_name=name;
			_sectionName=sectionName;
			_location=location;
			_size=size;
			_font=font;
			_contentAlignment=contentAlignment;
			_stringFormat=stringFormat;
			_fieldDefKind=FieldDefKind.SpecialField;
			_fieldValueType=fieldValueType;
			_specialFieldType=specialType;
			//defaults:
			_foreColor=Color.Black;
			_reportObjectKind=ReportObjectKind.FieldObject;
		}

		///<summary>Converts contentAlignment into a combination of StringAlignments used to format strings.  This method is mostly called for drawing text on reportObjects.</summary>
		public static StringFormat GetStringFormatAlignment(ContentAlignment contentAlignment){
			if(!Enum.IsDefined(typeof(ContentAlignment),(int)contentAlignment))
				throw new System.ComponentModel.InvalidEnumArgumentException(
					"contentAlignment",(int)contentAlignment,typeof(ContentAlignment));
			StringFormat stringFormat = new StringFormat();
			switch (contentAlignment){
				case ContentAlignment.MiddleCenter:
					stringFormat.LineAlignment = StringAlignment.Center;
					stringFormat.Alignment = StringAlignment.Center;
					break;
				case ContentAlignment.MiddleLeft:
					stringFormat.LineAlignment = StringAlignment.Center;
					stringFormat.Alignment = StringAlignment.Near;
					break;
				case ContentAlignment.MiddleRight:
					stringFormat.LineAlignment = StringAlignment.Center;
					stringFormat.Alignment = StringAlignment.Far;
					break;
				case ContentAlignment.TopCenter:
					stringFormat.LineAlignment = StringAlignment.Near;
					stringFormat.Alignment = StringAlignment.Center;
					break;
				case ContentAlignment.TopLeft:
					stringFormat.LineAlignment = StringAlignment.Near;
					stringFormat.Alignment = StringAlignment.Near;
					break;
				case ContentAlignment.TopRight:
					stringFormat.LineAlignment = StringAlignment.Near;
					stringFormat.Alignment = StringAlignment.Far;
					break;
				case ContentAlignment.BottomCenter:
					stringFormat.LineAlignment = StringAlignment.Far;
					stringFormat.Alignment = StringAlignment.Center;
					break;
				case ContentAlignment.BottomLeft:
					stringFormat.LineAlignment = StringAlignment.Far;
					stringFormat.Alignment = StringAlignment.Near;
					break;
				case ContentAlignment.BottomRight:
					stringFormat.LineAlignment = StringAlignment.Far;
					stringFormat.Alignment = StringAlignment.Far;
					break;
			}
			return stringFormat;
		}

		///<summary>Used to copy a report object when creating new QueryObjects.</summary>
		public ReportObject DeepCopyReportObject() {
			ReportObject reportObj=new ReportObject();
			reportObj._sectionName=this._sectionName;
			reportObj._location=new Point(this._location.X,this._location.Y);
			reportObj._size=new Size(this._size.Width,this._size.Height);
			reportObj._name=this._name;
			reportObj._reportObjectKind=this._reportObjectKind;
			reportObj._font=(Font)this._font.Clone();
			reportObj._contentAlignment=this._contentAlignment;
			reportObj._foreColor=this._foreColor;
			reportObj._staticText=this._staticText;
			reportObj._stringFormat=this._stringFormat;
			reportObj._suppressIfDuplicate=this._suppressIfDuplicate;
			reportObj._floatLineThickness=this._floatLineThickness;
			reportObj._fieldDefKind=this._fieldDefKind;
			reportObj._fieldValueType=this._fieldValueType;
			reportObj._specialFieldType=this._specialFieldType;
			reportObj._summaryOperation=this._summaryOperation;
			reportObj._lineOrientation=this._lineOrientation;
			reportObj._linePosition=this._linePosition;
			reportObj._intLinePercent=this._intLinePercent;
			reportObj._offSetX=this._offSetX;
			reportObj._offSetY=this._offSetY;
			reportObj._isUnderlined=this._isUnderlined;
			reportObj._summarizedFieldName=this._summarizedFieldName;
			reportObj._dataFieldName=this._dataFieldName;
			reportObj._summaryOrientation=this._summaryOrientation;
			List<int> summaryGroupsNew=new List<int>();
			if(this._summaryGroupValues!=null) {
				for(int i=0;i<this._summaryGroupValues.Count;i++) {
					summaryGroupsNew.Add(this._summaryGroupValues[i]);
				}
			}
			reportObj._summaryGroupValues=summaryGroupsNew;
			return reportObj;
		}

		///<summary>Once a dataTable has been set, this method can be run to get the summary value of this field.  It will still need to be formatted.  It loops through all records to get this value.</summary>
		public double GetSummaryValue(DataTable dataTable,int col){
			double retVal=0;
			for(int i=0;i<dataTable.Rows.Count;i++) {
				if(SummaryOperation==SummaryOperation.Sum) {
					retVal+=PIn.Double(dataTable.Rows[i][col].ToString());
				}
				else if(SummaryOperation==SummaryOperation.Count) {
					retVal++;
				}
			}
			return retVal;
		}

		///<summary>Used to automatically calculate the new size when something important changes. Also recalculates location for report headers.</summary>
		private Size CalculateNewSize(string text,Font font) {
			Graphics grfx=Graphics.FromImage(new Bitmap(1,1));
			Size size;
			if(_sectionName=="Group Header" || _sectionName=="Group Footer" || _sectionName=="Detail") {
				size=new Size(_size.Width,(int)(grfx.MeasureString(text,font).Height/grfx.DpiY*100+2));
			}
			else {
				size=new Size((int)(grfx.MeasureString(text,font).Width/grfx.DpiX*100+2),(int)(grfx.MeasureString(text,font).Height/grfx.DpiY*100+2));
			}
			if(_sectionName=="Report Header") {
				_location.X+=(_size.Width/2);
				_location.X-=(size.Width/2);
			}
			return size;
		}

	}

	///<summary>Specifies the field kind in the FieldKind property of the ReportObject class.</summary>
	public enum FieldDefKind{
		///<summary></summary>
		DataTableField,
		///<summary></summary>
		FormulaField,
		///<summary></summary>
		SpecialField,
		///<summary></summary>
		SummaryField
		//RunningTotalField
		//GroupNameField
	}

	///<summary>Used in the Kind field of each ReportObject to provide a quick way to tell what kind of reportObject.</summary>
	public enum ReportObjectKind{
		//BlobFieldObject Object is a blob field. 
		///<summary>Object is a box.</summary>
		BoxObject,
		//ChartObject Object is a chart. 
		//CrossTabObject Object is a cross tab. 
		///<summary>Object is a field object.</summary>
		FieldObject,
		///<summary>Object is a line. </summary>
		LineObject,
		//PictureObject Object is a picture. 
		//SubreportObject Object is a subreport.
		///<summary>Object is a text object. </summary>
		TextObject,
		///<summary>Object is a text object. </summary>
		QueryObject
	}

	///<summary>Specifies the special field type in the SpecialType property of the ReportObject class.</summary>
	public enum SpecialFieldType{
		///<summary>Field returns "Page [current page number] of [total page count]" formula. Not functional yet.</summary>
		PageNofM,
		///<summary>Field returns the current page number.</summary>
		PageNumber,
		///<summary>Field returns the current date.</summary>
		PrintDate
	}

	///<summary></summary>
	public enum SummaryOperation{
		//Average Summary returns the average of a field.
		///<summary>Summary counts the number of values, from the field.</summary>
		Count,
		//DistinctCount Summary returns the number of none repeating values, from the field. 
		//Maximum Summary returns the largest value from the field. 
		//Median Summary returns the middle value in a sequence of numeric values. 
		//Minimum Summary returns the smallest value from the field. 
		//Percentage Summary returns as a percentage of the grand total summary. 
		///<summary>Summary returns the total of all the values for the field.</summary>
		Sum
	}

	///<summary>Used to determine how a line draws in a section.</summary>
	public enum LineOrientation{
		///<summary></summary>
		Horizontal,
		///<summary></summary>
		Vertical
	}

	///<summary>Used to determine where a line draws in a section.</summary>
	public enum LinePosition{
		///<summary>Used in Horizontal and Vertical Orientation</summary>
		Center,
		///<summary>Used in Vertical Orientation</summary>
		Left,
		///<summary>Used in Vertical Orientation</summary>
		Right,
		///<summary>Used in Horizontal Orientation</summary>
		Top,
		///<summary>Used in Horizontal Orientation</summary>
		Bottom
	}

	///<summary>This determines what type of column the table will be splitting on. Default is none.</summary>
	public enum SplitByKind {
		///<summary>0</summary>
		None,
		///<summary>1</summary>
		Date,
		///<summary>2</summary>
		Enum,
		///<summary>3</summary>
		Definition,
		///<summary>4</summary>
		Value
	}

	///<summary>This determines which side of the summaryfield the label will be drawn on.</summary>
	public enum SummaryOrientation {
		///<summary>0</summary>
		None,
		///<summary>1</summary>
		North,
		///<summary>2</summary>
		South,
		///<summary>3</summary>
		East,
		///<summary>4</summary>
		West
	}

	



}
