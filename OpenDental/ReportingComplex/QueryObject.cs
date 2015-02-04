using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data;
using OpenDentBusiness;
using System.IO;

namespace OpenDental.ReportingComplex {

	///<summary>For every query added to a report there will be at least one QueryObject.</summary>
	public class QueryObject:ReportObject  {
		private SectionCollection _sections=new SectionCollection();
		private ArrayList _arrDataFields=new ArrayList();
		private ReportObjectCollection _reportObjects=new ReportObjectCollection();
		private string _columnNameToSplitOn;
		private string _stringQuery;
		private DataTable _reportTable;
		private DataTable _exportTable;
		private List<int> _rowHeightValues;
		private List<string> _listEnumNames;
		private Dictionary<long,string> _dictDefNames;
		private SplitByKind _splitByKind;
		private int _queryGroupValue;
		private int _queryWidth;
		private bool _isCentered;
		private bool _suppressHeaders;
		private bool _isLastSplit;
		private bool _isNegativeSummary;
		public bool IsPrinted;

		#region Properties
		public SectionCollection Sections {
			get {
				return _sections;
			}
		}

		public ArrayList ArrDataFields {
			get {
				return _arrDataFields;
			}
		}

		///<summary>A collection of report objects that comprise a single query.  This will contain a title, column headers, data fields, etc.</summary>
		public ReportObjectCollection ReportObjects {
			get {
				return _reportObjects;
			}
		}

		///<summary>When the content of the data field changes within the column that has this name a new table will be created.  E.g. splitting up one query into multiple tables by payment types.</summary>
		public string ColumnNameToSplitOn {
			get {
				return _columnNameToSplitOn;
			}
		}

		public DataTable ReportTable {
			get {
				return _reportTable;
			}
			set {
				_reportTable=value;
			}
		}

		public DataTable ExportTable {
			get {
				return _exportTable;
			}
			set {
				_exportTable=value;
			}
		}

		public List<string> ListEnumNames {
			get {
				return _listEnumNames;
			}
		}

		public Dictionary<long,string> DictDefNames {
			get {
				return _dictDefNames;
			}
		}

		public SplitByKind SplitByKind {
			get {
				return _splitByKind;
			}
		}

		public List<int> RowHeightValues {
			get {
				return _rowHeightValues;
			}
			set {
				_rowHeightValues=value;
			}
		}

		public int QueryGroupValue {
			get {
				return _queryGroupValue;
			}
			set {
				_queryGroupValue=value;
			}
		}

		public bool IsCentered {
			get {
				return _isCentered;
			}
			set {
				_isCentered=value;
			}
		}

		public int QueryWidth {
			get {
				return _queryWidth;
			}
			set {
				_queryWidth=value;
			}
		}

		public bool SuppressHeaders {
			get {
				return _suppressHeaders;
			}
			set {
				_suppressHeaders=value;
			}
		}

		public bool IsLastSplit {
			get {
				return _isLastSplit;
			}
			set {
				_isLastSplit=value;
			}
		}

		public bool IsNegativeSummary {
			get {
				return _isNegativeSummary;
			}
			set {
				_isNegativeSummary=value;
			}
		}
		#endregion

		///<summary>Default constructor.  Do not use.  Only used from DeepCopy()</summary>
		public QueryObject() {
		}

		///<summary>Creates a QueryObject from the given query string. If a column is specified for splitting, then this will create</summary>
		public QueryObject(string stringQuery,string title,string columnNameToSplitOn,SplitByKind splitByKind,int queryGroupValue,bool isCentered,List<string> listEnumNames,Dictionary<long,string> dictDefNames,Font font) {
			Graphics grfx=Graphics.FromImage(new Bitmap(1,1));
			_columnNameToSplitOn=columnNameToSplitOn;
			_stringQuery=stringQuery;
			SectionName="Query";
			Name="Query";
			_splitByKind=splitByKind;
			_listEnumNames=listEnumNames;
			_dictDefNames=dictDefNames;
			_queryGroupValue=queryGroupValue;
			_isCentered=isCentered;
			ReportObjectKind=ReportObjectKind.QueryObject;
			_sections.Add(new Section(AreaSectionKind.GroupTitle,0));
			_reportObjects.Add(new ReportObject("Group Title","Group Title",new Point(0,0),new Size((int)(grfx.MeasureString(title,font).Width/grfx.DpiX*100+2),(int)(grfx.MeasureString(title,font).Height/grfx.DpiY*100+2)),title,font,ContentAlignment.MiddleLeft,0,0));
			_reportObjects["Group Title"].IsUnderlined=true;
			_sections.Add(new Section(AreaSectionKind.GroupHeader,0));
			_sections.Add(new Section(AreaSectionKind.Detail,0));
			_sections.Add(new Section(AreaSectionKind.GroupFooter,0));
			_queryWidth=0;
			_suppressHeaders=true;
			_isLastSplit=true;
			_exportTable=new DataTable();
			grfx.Dispose();
		}

		public QueryObject(DataTable tableQuery,string title,string columnNameToSplitOn,SplitByKind splitByKind,int queryGroupValue,bool isCentered,List<string> listEnumNames,Dictionary<long,string> dictDefNames,Font font) {
			Graphics grfx=Graphics.FromImage(new Bitmap(1,1));
			_columnNameToSplitOn=columnNameToSplitOn;
			_reportTable=tableQuery;
			SectionName="Query";
			Name="Query";
			_splitByKind=splitByKind;
			_listEnumNames=listEnumNames;
			_dictDefNames=dictDefNames;
			_queryGroupValue=queryGroupValue;
			_isCentered=isCentered;
			ReportObjectKind=ReportObjectKind.QueryObject;
			_sections.Add(new Section(AreaSectionKind.GroupTitle,0));
			_reportObjects.Add(new ReportObject("Group Title","Group Title",new Point(0,0),new Size((int)(grfx.MeasureString(title,font).Width/grfx.DpiX*100+2),(int)(grfx.MeasureString(title,font).Height/grfx.DpiY*100+2)),title,font,ContentAlignment.MiddleLeft));
			_reportObjects["Group Title"].IsUnderlined=true;
			_sections.Add(new Section(AreaSectionKind.GroupHeader,0));
			_sections.Add(new Section(AreaSectionKind.Detail,0));
			_sections.Add(new Section(AreaSectionKind.GroupFooter,0));
			_queryWidth=0;
			_suppressHeaders=true;
			_isLastSplit=true;
			_exportTable=new DataTable();
			grfx.Dispose();
		}

		public QueryObject(string stringQuery,string title,string columnNameToSplitOn,SplitByKind splitByKind,int queryGroupValue,bool isCentered,Font font)
			: this(stringQuery,title,columnNameToSplitOn,splitByKind,queryGroupValue,isCentered,null,null,font) {
			
		}

		public QueryObject(DataTable tableQuery,string title,string columnNameToSplitOn,SplitByKind splitByKind,int queryGroupValue,bool isCentered,Font font)
			: this(tableQuery,title,columnNameToSplitOn,splitByKind,queryGroupValue,isCentered,null,null,font) {
			
		}

		public QueryObject(string stringQuery,string title,string columnNameToSplitOn,SplitByKind splitByKind,int queryGroupValue,bool isCentered)
			: this(stringQuery,title,columnNameToSplitOn,splitByKind,queryGroupValue,isCentered,null,null,new Font("Tahoma",9)) {

		}

		public QueryObject(DataTable tableQuery,string title,string columnNameToSplitOn,SplitByKind splitByKind,int queryGroupValue,bool isCentered)
			: this(tableQuery,title,columnNameToSplitOn,splitByKind,queryGroupValue,isCentered,null,null,new Font("Tahoma",9)) {

		}

		public QueryObject(string stringQuery,string title,string columnNameToSplitOn,SplitByKind splitByKind,int queryGroupValue)
			: this(stringQuery,title,columnNameToSplitOn,splitByKind,queryGroupValue,true,null,null,new Font("Tahoma",9)) {

		}

		public QueryObject(DataTable tableQuery,string title,string columnNameToSplitOn,SplitByKind splitByKind,int queryGroupValue)
			: this(tableQuery,title,columnNameToSplitOn,splitByKind,queryGroupValue,true,null,null,new Font("Tahoma",9)) {

		}

		///<summary>Adds all the objects necessary for a typical column, including the textObject for column header and the fieldObject for the data.  If the column is type Double, then the alignment is set right and a total field is added. Also, default formatstrings are set for dates and doubles.  Does not add lines or shading.</summary>
		public void AddColumn(string dataField,int width,FieldValueType fieldValueType,Font font) {
			Graphics grfx=Graphics.FromImage(new Bitmap(1,1));
			_arrDataFields.Add(dataField);
			Font fontHeader=new Font(font.FontFamily,font.Size-1,FontStyle.Bold);
			Font fontFooter=new Font(font.FontFamily,font.Size,FontStyle.Bold);
			ContentAlignment textAlign;
			if(fieldValueType==FieldValueType.Number) {
				textAlign=ContentAlignment.MiddleRight;
			}
			else {
				textAlign=ContentAlignment.MiddleLeft;
			}
			string formatString="";
			if(fieldValueType==FieldValueType.Number) {
				formatString="n";
			}
			if(fieldValueType==FieldValueType.Date) {
				formatString="d";
			}
			_queryWidth+=width;
			//add textobject for column header
			Size sizeHeader=new Size((int)grfx.MeasureString(dataField,fontHeader,(int)(width/grfx.DpiX*100+2)).Width,(int)grfx.MeasureString(dataField,fontHeader,(int)(width/grfx.DpiY*100+2)).Height);
			Size sizeDetail=new Size((int)grfx.MeasureString(dataField,font,(int)(width/grfx.DpiX*100+2)).Width,(int)grfx.MeasureString(dataField,font,(int)(width/grfx.DpiY*100+2)).Height);
			Size sizeFooter=new Size((int)grfx.MeasureString(dataField,fontFooter,(int)(width/grfx.DpiX*100+2)).Width,(int)grfx.MeasureString(dataField,fontFooter,(int)(width/grfx.DpiY*100+2)).Height);
			int xPos=0;
			//find next available xPos
			foreach(ReportObject reportObject in _reportObjects) {
				if(reportObject.SectionName!="Group Header") {
					continue;
				}
				if(reportObject.Location.X+reportObject.Size.Width > xPos) {
					xPos=reportObject.Location.X+reportObject.Size.Width;
				}
			}
			_reportObjects.Add(new ReportObject(dataField+"Header","Group Header"
				,new Point(xPos,0),new Size(width,sizeHeader.Height),dataField,fontHeader,textAlign));
			//add fieldObject for rows in details section
			_reportObjects.Add(new ReportObject(dataField+"Detail","Detail"
				,new Point(xPos,0),new Size(width,sizeDetail.Height)
				,dataField,fieldValueType
				,font,textAlign,formatString));
			//add fieldObject for total in ReportFooter
			if(fieldValueType==FieldValueType.Number) {
				//use same size as already set for otherFieldObjects above
				_reportObjects.Add(new ReportObject(dataField+"Footer","Group Footer"
					,new Point(xPos,0),new Size(width,sizeFooter.Height)
					,SummaryOperation.Sum,dataField
					,fontFooter,textAlign,formatString));
			}
			_exportTable.Columns.Add(dataField);
			grfx.Dispose();
			return;
		}

		///<summary>Font default is Tahoma 9pt</summary>
		public void AddColumn(string dataField,int width,FieldValueType fieldValueType) {
			AddColumn(dataField,width,fieldValueType,new Font("Tahoma",9));
		}

		///<summary>Add a label to a summaryfield based on the orientation given.</summary>
		public void AddSummaryLabel(string dataFieldName,string summaryText,SummaryOrientation summaryOrientation,bool hasWordWrap,Font font) {
			Graphics grfx=Graphics.FromImage(new Bitmap(1,1));
			ReportObject summaryField=GetObjectByName(dataFieldName+"Footer");
			Size size;
			if(hasWordWrap) {
				size=new Size(summaryField.Size.Width,(int)(grfx.MeasureString(summaryText,font,summaryField.Size.Width).Height/grfx.DpiY*100+2));
			}
			else {
				size=new Size((int)(grfx.MeasureString(summaryText,font).Width/grfx.DpiX*100+2),(int)(grfx.MeasureString(summaryText,font).Height/grfx.DpiY*100+2));
			}
			if(summaryOrientation==SummaryOrientation.North) {
				ReportObject summaryLabel=new ReportObject(dataFieldName+"Label","Group Footer"
						,summaryField.Location
						,size
						,summaryText
						,font
						,summaryField.ContentAlignment);
				summaryLabel.DataField=dataFieldName;
				summaryLabel.SummaryOrientation=summaryOrientation;
				_reportObjects.Insert(_reportObjects.IndexOf(summaryField),summaryLabel);
			}
			else if(summaryOrientation==SummaryOrientation.South) {
				ReportObject summaryLabel=new ReportObject(dataFieldName+"Label","Group Footer"
						,summaryField.Location
						,size
						,summaryText
						,font
						,summaryField.ContentAlignment);
				summaryLabel.DataField=dataFieldName;
				summaryLabel.SummaryOrientation=summaryOrientation;
				_reportObjects.Add(summaryLabel);
			}
			else if(summaryOrientation==SummaryOrientation.West) {
				ReportObject summaryLabel=new ReportObject(dataFieldName+"Label","Group Footer"
						,new Point(summaryField.Location.X-size.Width)
						,size
						,summaryText
						,font
						,summaryField.ContentAlignment);
				summaryLabel.DataField=dataFieldName;
				summaryLabel.SummaryOrientation=summaryOrientation;
				_reportObjects.Insert(_reportObjects.IndexOf(summaryField),summaryLabel);
			}
			else {
				ReportObject summaryLabel=new ReportObject(dataFieldName+"Label","Group Footer"
						,new Point(summaryField.Location.X+size.Width+summaryField.Size.Width)
						,size
						,summaryText
						,font
						,summaryField.ContentAlignment);
				summaryLabel.DataField=dataFieldName;
				summaryLabel.SummaryOrientation=summaryOrientation;
				_reportObjects.Insert(_reportObjects.IndexOf(summaryField)+1,summaryLabel);
			}
			grfx.Dispose();
		}

		///<summary>Font default is Tahoma 9pt</summary>
		public void AddSummaryLabel(string dataFieldName,string summaryText,SummaryOrientation summaryOrientation,bool hasWordWrap) {
			AddSummaryLabel(dataFieldName,summaryText,summaryOrientation,hasWordWrap,new Font("Tahoma",9));
		}

		/// <summary>Adds a line to a section.</summary>
		public void AddLine(string name,string sectionName,Color color,float floatLineThickness,LineOrientation lineOrientation,LinePosition linePosition,int linePercentValue,int offSetX,int offSetY) {
			_reportObjects.Add(new ReportObject(name,sectionName,color,floatLineThickness,lineOrientation,linePosition,linePercentValue,offSetX,offSetY));
		}

		/// <summary></summary>
		public void AddLine(string name,string sectionName,Color color,float floatLineThickness,LineOrientation lineOrientation,LinePosition linePosition,int linePercentValue) {
			AddLine(name,sectionName,color,floatLineThickness,lineOrientation,linePosition,linePercentValue,0,0);
		}

		/// <summary>Line is drawn in 50% of the available space.</summary>
		public void AddLine(string name,string sectionName,Color color,float floatLineThickness,LineOrientation lineOrientation,LinePosition linePosition) {
			AddLine(name,sectionName,color,floatLineThickness,lineOrientation,linePosition,50,0,0);
		}

		/// <summary>Line is drawn in 50% of the available space, black in color and in 2pt size.</summary>
		public void AddLine(string name,string sectionName,LineOrientation lineOrientation,LinePosition linePosition) {
			AddLine(name,sectionName,Color.Black,2,lineOrientation,linePosition,50,0,0);
		}

		///<summary>Do not use. Only used when splitting a table on a column.</summary>
		public void AddInitialHeader(string title,Font font) {
			Graphics grfx=Graphics.FromImage(new Bitmap(1,1));
			Font newFont=new Font(font.FontFamily,font.Size+2,font.Style);
			_reportObjects.Insert(0,new ReportObject("Initial Group Title","Group Title",new Point(0,0),new Size((int)(grfx.MeasureString(title,newFont).Width/grfx.DpiX*100+2),(int)(grfx.MeasureString(title,newFont).Height/grfx.DpiY*100+2)),title,newFont,ContentAlignment.MiddleLeft));
			_reportObjects["Initial Group Title"].IsUnderlined=true;
			grfx.Dispose();
		}

		///<summary>Adds a summary value to a group of QueryObjects.</summary>
		public void AddGroupSummaryField(string staticText,Color color,string columnName,string dataFieldName,SummaryOperation summaryOperation,List<int> queryGroupValues,Font font,int offSetX,int offSetY) {
			Graphics grfx=Graphics.FromImage(new Bitmap(1,1));
			Point location=GetObjectByName(columnName+"Header").Location;
			Size labelSize=new Size((int)(grfx.MeasureString(staticText,font).Width/grfx.DpiX*100+2)
				,(int)(grfx.MeasureString(staticText,font).Height/grfx.DpiY*100+2));
			int i=_reportObjects.Add(new ReportObject(columnName+"GroupSummaryLabel","Group Footer",new Point(location.X-labelSize.Width,0),labelSize,staticText,font,ContentAlignment.MiddleLeft,offSetX,offSetY));
			_reportObjects[i].DataField=dataFieldName;
			_reportObjects[i].SummaryGroups=queryGroupValues;
			_sections["Group Footer"].Height+=(int)((grfx.MeasureString(staticText,font)).Height/grfx.DpiY*100+2)+offSetY;
			i=_reportObjects.Add(new ReportObject(columnName+"GroupSummaryText","Group Footer",location,new Size(0,0),color,columnName,dataFieldName,font,summaryOperation,offSetX,offSetY));
			_reportObjects[i].SummaryGroups=queryGroupValues;
			grfx.Dispose();
		}

		///<summary></summary>
		public void AddGroupSummaryField(string staticText,Color color,string columnName,string dataFieldName,SummaryOperation summaryOperation,List<int> queryGroupValues,Font font) {
			AddGroupSummaryField(staticText,color,columnName,dataFieldName,summaryOperation,queryGroupValues,font,0,0);
		}

		///<summary>Default Font is Tahoma 8pt Bold.</summary>
		public void AddGroupSummaryField(string staticText,Color color,string columnName,string dataFieldName,SummaryOperation summaryOperation,List<int> queryGroupValues,int offSetX,int offSetY) {
			AddGroupSummaryField(staticText,color,columnName,dataFieldName,summaryOperation,queryGroupValues,new Font("Tahoma",8,FontStyle.Bold),offSetX,offSetY);
		}

		///<summary>Defaults: Font Tahoma 8pt Bold, summary group 1.</summary>
		public void AddGroupSummaryField(string staticText,Color color,string columnName,string dataFieldName,SummaryOperation summaryOperation) {
			AddGroupSummaryField(staticText,color,columnName,dataFieldName,summaryOperation,new List<int>() { 1 },new Font("Tahoma",8,FontStyle.Bold),0,0);
		}

		///<summary>Default Font is Tahoma 8pt Bold.  Summary group is set to 1.  Color is Black.</summary>
		public void AddGroupSummaryField(string staticText,string columnName,string dataFieldName,SummaryOperation summaryOperation) {
			AddGroupSummaryField(staticText,Color.Black,columnName,dataFieldName,summaryOperation,new List<int>() { 1 },new Font("Tahoma",8,FontStyle.Bold),0,0);
		}

		///<summary>Default Font is Tahoma 8pt Bold. Summary group is set to 1., Color is set to Black.</summary>
		public void AddGroupSummaryField(string staticText,string columnName,string dataFieldName,SummaryOperation summaryOperation,int offSetX,int offSetY) {
			AddGroupSummaryField(staticText,Color.Black,columnName,dataFieldName,summaryOperation,new List<int>() { 1 },new Font("Tahoma",8,FontStyle.Bold),offSetX,offSetY);
		}

		///<summary>Summary group is set to 1., Color is set to Black.</summary>
		public void AddGroupSummaryField(string staticText,string columnName,string dataFieldName,SummaryOperation summaryOperation,Font font,int offSetX,int offSetY) {
			AddGroupSummaryField(staticText,Color.Black,columnName,dataFieldName,summaryOperation,new List<int>() { 1 },font,offSetX,offSetY);
		}

		///<summary>Summary group is set to 1.</summary>
		public void AddGroupSummaryField(string staticText,Color color,string columnName,string dataFieldName,SummaryOperation summaryOperation,Font font,int offSetX,int offSetY) {
			AddGroupSummaryField(staticText,color,columnName,dataFieldName,summaryOperation,new List<int>() { 1 },font,offSetX,offSetY);
		}

		///<summary>Submits the Query to the database and fills ReportTable with the results.  Returns false if the query fails.</summary>
		public bool SubmitQuery() {
			if(String.IsNullOrWhiteSpace(_stringQuery)) {
				//The programmer must have prefilled the data table already, so no reason to try and run a query.
			}
			else {
				try {
					_reportTable=Reports.GetTable(_stringQuery);
				}
				catch(Exception) {
					return false;
				}
			}
			_rowHeightValues=new List<int>();
			Graphics g=Graphics.FromImage(new Bitmap(1,1));
			for(int i=0;i<_reportTable.Rows.Count;i++) {
				string rawText;
				string displayText="";
				string prevDisplayText="";
				int rowHeight=0;
				foreach(ReportObject reportObject in _reportObjects) {
					if(reportObject.SectionName!="Detail") {
						continue;
					}
					if(reportObject.ReportObjectKind==ReportObjectKind.FieldObject) {
						rawText=_reportTable.Rows[i][_arrDataFields.IndexOf(reportObject.DataField)].ToString();
						if(String.IsNullOrWhiteSpace(rawText)) {
							continue;
						}
						List<string> listString=GetDisplayString(rawText,prevDisplayText,reportObject,i);
						displayText=listString[0];
						prevDisplayText=listString[1];
						int curCellHeight=(int)(g.MeasureString(displayText,reportObject.Font,(int)(reportObject.Size.Width),ReportObject.GetStringFormatAlignment(reportObject.ContentAlignment))).Height;
						if(curCellHeight>rowHeight) {
							rowHeight=curCellHeight;
						}
					}
				}
				_rowHeightValues.Add(rowHeight);
			}
			DataRow row;
			for(int i=0;i<_reportTable.Rows.Count;i++) {
				row=_exportTable.NewRow();
				for(int j=0;j<_exportTable.Columns.Count;j++) {
					row[j]=_reportTable.Rows[i][j];
				}
				_exportTable.Rows.Add(row);
			}
			g.Dispose();
			return true;
		}

		private List<string> GetDisplayString(string rawText,string prevDisplayText,ReportObject reportObject,int i) {
			string displayText="";
			List<string> retVals=new List<string>();
			if(reportObject.FieldValueType==FieldValueType.Age) {
				displayText=Patients.AgeToString(Patients.DateToAge(PIn.Date(rawText)));//(fieldObject.FormatString);
			}
			else if(reportObject.FieldValueType==FieldValueType.Boolean) {
				displayText=PIn.Bool(_reportTable.Rows[i][_arrDataFields.IndexOf(reportObject.DataField)].ToString()).ToString();//(fieldObject.FormatString);
				if(i>0 && reportObject.SuppressIfDuplicate) {
					prevDisplayText=PIn.Bool(_reportTable.Rows[i-1][_arrDataFields.IndexOf(reportObject.DataField)].ToString()).ToString();
				}
			}
			else if(reportObject.FieldValueType==FieldValueType.Date) {
				displayText=PIn.DateT(_reportTable.Rows[i][_arrDataFields.IndexOf(reportObject.DataField)].ToString()).ToString(reportObject.StringFormat);
				if(i>0 && reportObject.SuppressIfDuplicate) {
					prevDisplayText=PIn.DateT(_reportTable.Rows[i-1][_arrDataFields.IndexOf(reportObject.DataField)].ToString()).ToString(reportObject.StringFormat);
				}
			}
			else if(reportObject.FieldValueType==FieldValueType.Integer) {
				displayText=PIn.Long(_reportTable.Rows[i][_arrDataFields.IndexOf(reportObject.DataField)].ToString()).ToString(reportObject.StringFormat);
				if(i>0 && reportObject.SuppressIfDuplicate) {
					prevDisplayText=PIn.Long(_reportTable.Rows[i-1][_arrDataFields.IndexOf(reportObject.DataField)].ToString()).ToString(reportObject.StringFormat);
				}
			}
			else if(reportObject.FieldValueType==FieldValueType.Number) {
				displayText=PIn.Double(_reportTable.Rows[i][_arrDataFields.IndexOf(reportObject.DataField)].ToString()).ToString(reportObject.StringFormat);
				if(i>0 && reportObject.SuppressIfDuplicate) {
					prevDisplayText=PIn.Double(_reportTable.Rows[i-1][_arrDataFields.IndexOf(reportObject.DataField)].ToString()).ToString(reportObject.StringFormat);
				}
			}
			else if(reportObject.FieldValueType==FieldValueType.String) {
				displayText=rawText;
				if(i>0 && reportObject.SuppressIfDuplicate) {
					prevDisplayText=_reportTable.Rows[i-1][_arrDataFields.IndexOf(reportObject.DataField)].ToString();
				}
			}
			retVals.Add(displayText);
			retVals.Add(prevDisplayText);
			return retVals;
		}

		public ReportObject GetGroupTitle() {
			return ReportObjects["Group Title"];
		}

		public ReportObject GetColumnHeader(string columnName) {
			return ReportObjects[columnName+"Header"];
		}

		public ReportObject GetColumnDetail(string columnName) {
			return ReportObjects[columnName+"Detail"];
		}

		public ReportObject GetColumnFooter(string columnName) {
			return ReportObjects[columnName+"Footer"];
		}

		public ReportObject GetObjectByName(string name) {
			for(int i=_reportObjects.Count-1;i>=0;i--) {//search from the end backwards
				if(_reportObjects[i].Name==name) {
					return ReportObjects[i];
				}
			}
			MessageBox.Show("end of loop");
			return null;
		}

		public int GetTotalHeight() {
			int height=0;
			height+=_sections["Group Title"].Height;
			height+=_sections["Group Header"].Height;
			height+=_sections["Detail"].Height;
			height+=_sections["Group Footer"].Height;
			return height;
		}

		///<summary>If the specified section exists, then this returns its height. Otherwise it returns 0.</summary>
		public int GetSectionHeight(string sectionName) {
			return _sections[sectionName].Height;
		}

		public QueryObject DeepCopyQueryObject() {
			QueryObject queryObj=new QueryObject();
			queryObj.Name=this.Name;//Doesn't need to be a deep copy.
			queryObj.SectionName=this.SectionName;//Doesn't need to be a deep copy.
			queryObj.ReportObjectKind=this.ReportObjectKind;//Doesn't need to be a deep copy.
			queryObj._sections=this._sections;//Doesn't need to be a deep copy.
			queryObj._arrDataFields=this._arrDataFields;//Doesn't need to be a deep copy.
			queryObj._queryGroupValue=this._queryGroupValue;//Doesn't need to be a deep copy.
			queryObj._isCentered=this._isCentered;//Doesn't need to be a deep copy.
			queryObj._queryWidth=this._queryWidth;//Doesn't need to be a deep copy.
			queryObj._suppressHeaders=this._suppressHeaders;//Doesn't need to be a deep copy.
			queryObj._columnNameToSplitOn=this._columnNameToSplitOn;//Doesn't need to be a deep copy.
			queryObj._splitByKind=this._splitByKind;//Doesn't need to be a deep copy.
			queryObj.IsPrinted=this.IsPrinted;//Doesn't need to be a deep copy.
			queryObj.SummaryOrientation=this.SummaryOrientation;//Doesn't need to be a deep copy.
			queryObj.SummaryGroups=this.SummaryGroups;//Doesn't need to be a deep copy.
			queryObj._isLastSplit=this._isLastSplit;//Doesn't need to be a deep copy.
			queryObj._rowHeightValues=new List<int>();
			queryObj._isNegativeSummary=this._isNegativeSummary;
			for(int i=0;i<this._rowHeightValues.Count;i++) {
				queryObj._rowHeightValues.Add(this._rowHeightValues[i]);
			}
			ReportObjectCollection reportObjectsNew=new ReportObjectCollection();
			for(int i=0;i<this._reportObjects.Count;i++) {
				reportObjectsNew.Add(_reportObjects[i].DeepCopyReportObject());
			}
			queryObj._reportObjects=reportObjectsNew;
			//queryObj._query=this._query;
			queryObj._reportTable=new DataTable();
			//We only care about column headers at this point.  There is no easy way to copy an entire DataTable.
			for(int i=0;i<this.ReportTable.Columns.Count;i++) {
				queryObj._reportTable.Columns.Add(new DataColumn(this.ReportTable.Columns[i].ColumnName));
			}
			queryObj._exportTable=new DataTable();
			//We only care about column headers at this point.  There is no easy way to copy an entire DataTable.
			for(int i=0;i<this._exportTable.Columns.Count;i++) {
				queryObj._exportTable.Columns.Add(new DataColumn(this._exportTable.Columns[i].ColumnName));
			}
			List<string> enumNamesNew=new List<string>();
			if(this._listEnumNames!=null) {
				for(int i=0;i<this._listEnumNames.Count;i++) {
					enumNamesNew.Add(this._listEnumNames[i]);
				}
			}
			queryObj._listEnumNames=enumNamesNew;
			Dictionary<long,string> defNamesNew=new Dictionary<long,string>();
			if(this._dictDefNames!=null) {
				foreach(long defNum in _dictDefNames.Keys) {
					defNamesNew.Add(defNum,this._dictDefNames[defNum]);
				}
			}
			queryObj._dictDefNames=defNamesNew;
			return queryObj;
		}


	}
}
