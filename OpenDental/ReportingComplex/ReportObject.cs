using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental.ReportingComplex {
	///<summary>There is one ReportObject for each element of an ODReport that gets printed on the page.  There are many different kinds of reportObjects.</summary>
	public class ReportObject{
		private string _sectionName;
		private Point _location;
		private Size _size;
		private string _name;
		private ReportObjectKind _objectKind;
		private Font _font;
		private ContentAlignment _textAlign;
		private Color _foreColor;
		private string _staticText;
		private string _formatString;
		private bool _suppressIfDuplicate;
		private string _endSectionName;
		private Point _locationLowerRight;
		private float _lineThickness;
		private FieldDefKind _fieldKind;
		private FieldValueType _valueType;
		private SpecialFieldType _specialType;
		private SummaryOperation _operation;
		private LineOrientation _lineOrientation;
		private LinePosition _linePosition;
		private int _linePercent;
		private int _offSetX;
		private int _offSetY;
		private bool _isUnderlined;
		private string _summarizedField;
		private string _dataField;
		

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
		///<summary></summary>
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
		public ReportObjectKind ObjectKind{
			get{
				return _objectKind;
			}
			set{
				_objectKind=value;
			}
		}
		///<summary></summary>
		public Font Font{
			get{
				return _font;
			}
			set{
				_font=value;
			}
		}
		///<summary>Horizontal alignment of the text.</summary>
		public ContentAlignment TextAlign{
			get{
				return _textAlign;
			}
			set{
				_textAlign=value;
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
		///<summary>The text to display for a TextObject.  Will later include XML formatting markup.</summary>
		public string StaticText{
			get{
				return _staticText;
			}
			set{
				_staticText=value;
			}
		}
		///<summary>For a FieldObject, a C# format string that specifies how to print dates, times, numbers, and currency based on the country or on a custom format.</summary>
		///<remarks>There are a LOT of options for this string.  Look in C# help under Standard Numeric Format Strings, Custom Numeric Format Strings, Standard DateTime Format Strings, Custom DateTime Format Strings, and Enumeration Format Strings.  Once users are allowed to edit reports, we will assemble a help page with all of the common options. The best options are "n" for number, and "d" for date.</remarks>
		public string FormatString{
			get{
				return _formatString;
			}
			set{
				_formatString=value;
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
		///<summary>For graphics, the name of the Section to which the lower part of the object extends.  This will normally be the same as the sectionName unless the object spans multiple sections.  The object will then be drawn across all sections in between.</summary>
		public string EndSectionName{
			get{
				return _endSectionName;
			}
			set{
				_endSectionName=value;
			}
		}
		///<summary>The position of the lower right corner of the box or line in the coordinates of the endSection.</summary>
		public Point LocationLowerRight{
			get{
				return _locationLowerRight;
			}
			set{
				_locationLowerRight=value;
			}
		}
		///<summary></summary>
		public float LineThickness{
			get{
				return _lineThickness;
			}
			set{
				_lineThickness=value;
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
		public int LinePercent {
			get {
				return _linePercent;
			}
			set {
				_linePercent=value;
			}
		}
		///<summary>Used to offset lines and boxes by a specific number of pixels.</summary>
		public int OffSetX {
			get {
				return _offSetX;
			}
			set {
				_offSetX=value;
			}
		}
		///<summary>Used to offset lines and boxes by a specific number of pixels.</summary>
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
		public FieldDefKind FieldKind{
			get{
				return _fieldKind;
			}
			set{
				_fieldKind=value;
			}
		}
		///<summary>The value type of field, like string or datetime.</summary>
		public FieldValueType ValueType{
			get{
				return _valueType;
			}
			set{
				_valueType=value;
			}
		}
		///<summary>For FieldKind=FieldDefKind.SpecialField, this is the type.  eg. pagenumber</summary>
		public SpecialFieldType SpecialType{
			get{
				return _specialType;
			}
			set{
				_specialType=value;
			}
		}
		///<summary>For FieldKind=FieldDefKind.SummaryField, the summary operation type.</summary>
		public SummaryOperation Operation{
			get{
				return _operation;
			}
			set{
				_operation=value;
			}
		}
		///<summary>For FieldKind=FieldDefKind.SummaryField, the name of the dataField that is being summarized.  This might later be changed to refer to a ReportObject name instead (or maybe not).</summary>
		public string SummarizedField{
			get{
				return _summarizedField;
			}
			set{
				_summarizedField=value;
			}
		}
		///<summary>For objectKind=ReportObjectKind.FieldObject, the name of the dataField column.</summary>
		public string DataField{
			get{
				return _dataField;
			}
			set{
				_dataField=value;
			}
		}
#endregion

		///<summary>Default constructor.</summary>
		public ReportObject(){

		}

		///<summary>Overload for TextObject.</summary>
		public ReportObject(string name,string sectionName,Point location,Size size,string staticText,Font font,ContentAlignment textAlign){
			_name=name;
			_sectionName=sectionName;
			_location=location;
			_size=size;
			_staticText=staticText;
			_font=font;
			_textAlign=textAlign;
			_foreColor=Color.Black;
			_objectKind=ReportObjectKind.TextObject;
		}

		///<summary>Overload for BoxObject.</summary>
		public ReportObject(string name,string sectionName,Color color,float lineThickness,int offSetX,int offSetY) {
			_name=name;
			_sectionName=sectionName;
			_foreColor=color;
			_lineThickness=lineThickness;
			_offSetX=offSetX;
			_offSetY=offSetY;
			_objectKind=ReportObjectKind.BoxObject;
		}

		///<summary>Overload for LineObject.</summary>
		public ReportObject(string name,string sectionName,Color color,float lineThickness,LineOrientation lineOrientation,LinePosition linePosition,int linePercent,int offSetX,int offSetY) {
			_name=name;
			_sectionName=sectionName;
			_foreColor=color;
			_lineThickness=lineThickness;
			_lineOrientation=lineOrientation;
			_linePosition=linePosition;
			_linePercent=linePercent;
			_offSetX=offSetX;
			_offSetY=offSetY;
			_objectKind=ReportObjectKind.LineObject;
		}

		///<summary>Overload for DataTableField ReportObject</summary>
		public ReportObject(string name,string sectionName,Point location,Size size
			,string thisDataField,FieldValueType thisValueType
			,Font thisFont,ContentAlignment thisTextAlign,string thisFormatString) {
			_name=name;
			_sectionName=sectionName;
			_location=location;
			_size=size;
			_font=thisFont;
			_textAlign=thisTextAlign;
			_formatString=thisFormatString;
			_fieldKind=FieldDefKind.DataTableField;
			_dataField=thisDataField;
			_valueType=thisValueType;
			//defaults:
			_foreColor=Color.Black;
			_objectKind=ReportObjectKind.FieldObject;
		}

		///<summary>Overload for SummaryField ReportObject</summary>
		public ReportObject(string name,string sectionName,Point location,Size size,SummaryOperation operation,string summarizedField,Font font,ContentAlignment textAlign,string formatString) {
			_name=name;
			_sectionName=sectionName;
			_location=location;
			_size=size;
			_font=font;
			_textAlign=textAlign;
			_formatString=formatString;
			_fieldKind=FieldDefKind.SummaryField;
			_valueType=FieldValueType.Number;
			_operation=operation;
			_summarizedField=summarizedField;
			//defaults:
			_foreColor=Color.Black;
			_objectKind=ReportObjectKind.FieldObject;
		}

		///<summary>Overload for SpecialField ReportObject</summary>
		public ReportObject(string name,string sectionName,Point location,Size size,FieldValueType valueType,SpecialFieldType specialType,Font font,ContentAlignment textAlign,string formatString) {
			_name=name;
			_sectionName=sectionName;
			_location=location;
			_size=size;
			_font=font;
			_textAlign=textAlign;
			_formatString=formatString;
			_fieldKind=FieldDefKind.SpecialField;
			_valueType=valueType;
			_specialType=specialType;
			//defaults:
			_foreColor=Color.Black;
			_objectKind=ReportObjectKind.FieldObject;
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

		public ReportObject DeepCopyReportObject() {
			ReportObject reportObj=new ReportObject();
			reportObj._sectionName=this._sectionName;
			reportObj._location=new Point(this._location.X,this._location.Y);
			reportObj._size=new Size(this._size.Width,this._size.Height);
			reportObj._name=this._name;
			reportObj._objectKind=this._objectKind;
			reportObj._font=(Font)this._font.Clone();
			reportObj._textAlign=this._textAlign;
			reportObj._foreColor=this._foreColor;
			reportObj._staticText=this._staticText;
			reportObj._formatString=this._formatString;
			reportObj._suppressIfDuplicate=this._suppressIfDuplicate;
			reportObj._endSectionName=this._endSectionName;
			reportObj._locationLowerRight=new Point(this._locationLowerRight.X,this._locationLowerRight.Y);
			reportObj._lineThickness=this._lineThickness;
			reportObj._fieldKind=this._fieldKind;
			reportObj._valueType=this._valueType;
			reportObj._specialType=this._specialType;
			reportObj._operation=this._operation;
			reportObj._lineOrientation=this._lineOrientation;
			reportObj._linePosition=this._linePosition;
			reportObj._linePercent=this._linePercent;
			reportObj._offSetX=this._offSetX;
			reportObj._offSetY=this._offSetY;
			reportObj._isUnderlined=this._isUnderlined;
			reportObj._summarizedField=this._summarizedField;
			reportObj._dataField=this._dataField;
			return reportObj;
		}

		///<summary>Once a dataTable has been set, this method can be run to get the summary value of this field.  It will still need to be formatted.  It loops through all records to get this value.</summary>
		public double GetSummaryValue(DataTable dataTable,int col){
			double retVal=0;
			for(int i=0;i<dataTable.Rows.Count;i++){
				if(Operation==SummaryOperation.Sum){
					retVal+=PIn.Double(dataTable.Rows[i][col].ToString());
				}
				else if(Operation==SummaryOperation.Count){
					retVal++;
				}
			}
			return retVal;
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
		Enum
	}

	



}
