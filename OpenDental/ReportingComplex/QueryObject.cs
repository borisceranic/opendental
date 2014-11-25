﻿using System;
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
		private ArrayList _dataFields=new ArrayList();
		private ReportObjectCollection _reportObjects=new ReportObjectCollection();
		private string _columnNameToSplitOn;
		private string _query;
		private DataTable _reportTable;
		private List<int> _rowHeights;
		private List<string> _enumNames;
		private Dictionary<long,string> _dictDefNames;
		private SplitByKind _splitByKind;
		private int _queryGroupValue;
		private int _queryWidth;
		private bool _isCentered;
		private bool _suppressHeaders;
		private bool _isLastSplit;
		public bool IsPrinted;

		public SectionCollection Sections {
			get {
				return _sections;
			}
		}

		public ArrayList DataFields {
			get {
				return _dataFields;
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

		public List<string> EnumNames {
			get {
				return _enumNames;
			}
		}

		public Dictionary<long,string> DefNames {
			get {
				return _dictDefNames;
			}
		}

		public SplitByKind SplitKind {
			get {
				return _splitByKind;
			}
		}

		public List<int> RowHeights {
			get {
				return _rowHeights;
			}
			set {
				_rowHeights=value;
			}
		}

		public int QueryGroup {
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

		///<summary>Default constructor.  Do not use.  Only used from DeepCopy()</summary>
		public QueryObject() {
		}

		public QueryObject(string query,string title,string columnNameToSplitOn,SplitByKind splitByKind,int queryGroup,bool isCentered,List<string> enumNames,Dictionary<long,string> dictDefNames) {
			Graphics grfx=Graphics.FromImage(new Bitmap(1,1));
			_columnNameToSplitOn=columnNameToSplitOn;
			_query=query;
			SectionName="Query";
			Name="Query";
			_splitByKind=splitByKind;
			_enumNames=enumNames;
			_dictDefNames=dictDefNames;
			_queryGroupValue=queryGroup;
			_isCentered=isCentered;
			ObjectKind=ReportObjectKind.QueryObject;
			_sections.Add(new Section(AreaSectionKind.GroupTitle,0));
			Font font=new Font("Tahoma",14);
			_reportObjects.Add(new ReportObject("Group Title","Group Title",new Point(0,0),new Size((int)(grfx.MeasureString(title,font).Width/grfx.DpiX*100+2),(int)(grfx.MeasureString(title,font).Height/grfx.DpiY*100+2)),title,new Font("Tahoma",14),ContentAlignment.MiddleLeft,0,0));
			_reportObjects["Group Title"].IsUnderlined=true;
			_sections.Add(new Section(AreaSectionKind.GroupHeader,0));
			_sections.Add(new Section(AreaSectionKind.Detail,0));
			_sections.Add(new Section(AreaSectionKind.GroupFooter,0));
			_queryWidth=0;
			_suppressHeaders=true;
			_isLastSplit=true;
			grfx.Dispose();
		}

		public QueryObject(DataTable query,string title,string columnNameToSplitOn,SplitByKind splitByKind,int queryGroup,bool isCentered,List<string> enumNames,Dictionary<long,string> dictDefNames) {
			Graphics grfx=Graphics.FromImage(new Bitmap(1,1));
			_columnNameToSplitOn=columnNameToSplitOn;
			_reportTable=query;
			SectionName="Query";
			Name="Query";
			_splitByKind=splitByKind;
			_enumNames=enumNames;
			_dictDefNames=dictDefNames;
			_queryGroupValue=queryGroup;
			_isCentered=isCentered;
			ObjectKind=ReportObjectKind.QueryObject;
			_sections.Add(new Section(AreaSectionKind.GroupTitle,0));
			Font font=new Font("Tahoma",14);
			_reportObjects.Add(new ReportObject("Group Title","Group Title",new Point(0,0),new Size((int)(grfx.MeasureString(title,font).Width/grfx.DpiX*100+2),(int)(grfx.MeasureString(title,font).Height/grfx.DpiY*100+2)),title,new Font("Tahoma",14),ContentAlignment.MiddleLeft));
			_reportObjects["Group Title"].IsUnderlined=true;
			_sections.Add(new Section(AreaSectionKind.GroupHeader,0));
			_sections.Add(new Section(AreaSectionKind.Detail,0));
			_sections.Add(new Section(AreaSectionKind.GroupFooter,0));
			_queryWidth=0;
			_suppressHeaders=true;
			_isLastSplit=true;
			grfx.Dispose();
		}

		public QueryObject(string query,string title,string columnNameToSplitOn,SplitByKind splitByKind,int queryGroup,bool isCentered)
			: this(query,title,columnNameToSplitOn,splitByKind,queryGroup,isCentered,null,null) {
			
		}

		public QueryObject(DataTable query,string title,string columnNameToSplitOn,SplitByKind splitByKind,int queryGroup,bool isCentered)
			: this(query,title,columnNameToSplitOn,splitByKind,queryGroup,isCentered,null,null) {
			
		}

		///<summary>Adds all the objects necessary for a typical column, including the textObject for column header and the fieldObject for the data.  If the column is type Double, then the alignment is set right and a total field is added. Also, default formatstrings are set for dates and doubles.  Does not add lines or shading.</summary>
		public void AddColumn(string dataField,int width,FieldValueType valueType) {
			Graphics grfx=Graphics.FromImage(new Bitmap(1,1));
			_dataFields.Add(dataField);
			Font font;
			Size size;
			ContentAlignment textAlign;
			if(valueType==FieldValueType.Number) {
				textAlign=ContentAlignment.MiddleRight;
			}
			else {
				textAlign=ContentAlignment.MiddleLeft;
			}
			string formatString="";
			if(valueType==FieldValueType.Number) {
				formatString="n";
			}
			if(valueType==FieldValueType.Date) {
				formatString="d";
			}
			_queryWidth+=width;
			//add textobject for column header
			font=new System.Drawing.Font("Tahoma",8,System.Drawing.FontStyle.Bold);
			size=new Size((int)grfx.MeasureString(dataField,font,(int)(width/grfx.DpiX*100+2)).Width,(int)grfx.MeasureString(dataField,font,(int)(width/grfx.DpiY*100+2)).Height);
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
				,new Point(xPos,0),new Size(width,size.Height),dataField,font,textAlign));
			//add fieldObject for rows in details section
			font=new Font("Tahoma",9);
			_reportObjects.Add(new ReportObject(dataField+"Detail","Detail"
				,new Point(xPos,0),new Size(width,size.Height)
				,dataField,valueType
				,font,textAlign,formatString));
			//add fieldObject for total in ReportFooter
			if(valueType==FieldValueType.Number) {
				font=new Font("Tahoma",9,FontStyle.Bold);
				//use same size as already set for otherFieldObjects above
				_reportObjects.Add(new ReportObject(dataField+"Footer","Group Footer"
					,new Point(xPos,0),new Size(width,size.Height)
					,SummaryOperation.Sum,dataField
					,font,textAlign,formatString));
			}
			grfx.Dispose();
			return;
		}

		///<summary>Add a label to a summaryfield based on the orientation given.</summary>
		public void AddSummaryLabel(string dataField,string summaryText,SummaryOrientation orientation,bool hasWordWrap) {
			Graphics grfx=Graphics.FromImage(new Bitmap(1,1));
			ReportObject summaryField=GetObjectByName(dataField+"Footer");
			Size size;
			if(hasWordWrap) {
				size=new Size(summaryField.Size.Width,(int)(grfx.MeasureString(summaryText,summaryField.Font,summaryField.Size.Width).Height/grfx.DpiY*100+2));
			}
			else {
				size=new Size((int)(grfx.MeasureString(summaryText,summaryField.Font).Width/grfx.DpiX*100+2),(int)(grfx.MeasureString(summaryText,summaryField.Font).Height/grfx.DpiY*100+2));
			}
			if(orientation==SummaryOrientation.North) {
				ReportObject summaryLabel=new ReportObject(dataField+"Label","Group Footer"
						,summaryField.Location
						,size
						,summaryText
						,summaryField.Font
						,summaryField.TextAlign);
				summaryLabel.DataField=dataField;
				summaryLabel.SummaryOrientation=orientation;
				_reportObjects.Insert(_reportObjects.IndexOf(summaryField),summaryLabel);
			}
			else if(orientation==SummaryOrientation.South) {
				ReportObject summaryLabel=new ReportObject(dataField+"Label","Group Footer"
						,summaryField.Location
						,size
						,summaryText
						,summaryField.Font
						,summaryField.TextAlign);
				summaryLabel.DataField=dataField;
				summaryLabel.SummaryOrientation=orientation;
				_reportObjects.Add(summaryLabel);
			}
			else if(orientation==SummaryOrientation.West) {
				ReportObject summaryLabel=new ReportObject(dataField+"Label","Group Footer"
						,new Point(summaryField.Location.X-size.Width)
						,size
						,summaryText
						,summaryField.Font
						,summaryField.TextAlign);
				summaryLabel.DataField=dataField;
				summaryLabel.SummaryOrientation=orientation;
				_reportObjects.Insert(_reportObjects.IndexOf(summaryField),summaryLabel);
			}
			else {
				ReportObject summaryLabel=new ReportObject(dataField+"Label","Group Footer"
						,new Point(summaryField.Location.X+size.Width+summaryField.Size.Width)
						,size
						,summaryText
						,summaryField.Font
						,summaryField.TextAlign);
				summaryLabel.DataField=dataField;
				summaryLabel.SummaryOrientation=orientation;
				_reportObjects.Insert(_reportObjects.IndexOf(summaryField)+1,summaryLabel);
			}
			grfx.Dispose();
		}

		///<summary>Do not use. Only used when splitting a table on a column.</summary>
		public void AddInitialHeader(string title,Font font) {
			Graphics grfx=Graphics.FromImage(new Bitmap(1,1));
			Font newFont=new Font(font.FontFamily,font.Size+2,font.Style);
			_reportObjects.Insert(0,new ReportObject("Initial Group Title","Group Title",new Point(0,0),new Size((int)(grfx.MeasureString(title,newFont).Width/grfx.DpiX*100+2),(int)(grfx.MeasureString(title,newFont).Height/grfx.DpiY*100+2)),title,newFont,ContentAlignment.MiddleLeft));
			_reportObjects["Initial Group Title"].IsUnderlined=true;
			grfx.Dispose();
		}

		public void AddGroupSummaryField(Color color,string staticText,string columnName,string dataFieldName,SummaryOperation operation,List<int> queryGroups,int offSetX,int offSetY) {
			Graphics grfx=Graphics.FromImage(new Bitmap(1,1));
			Point location=GetObjectByName(columnName+"Header").Location;
			Size labelSize=new Size((int)(grfx.MeasureString(staticText,new Font("Tahoma",9,FontStyle.Bold)).Width/grfx.DpiX*100+2)
				,(int)(grfx.MeasureString(staticText,new Font("Tahoma",9,FontStyle.Bold)).Height/grfx.DpiY*100+2));
			int i=_reportObjects.Add(new ReportObject("GroupSummaryLabel","Group Footer",new Point(location.X-labelSize.Width,0),labelSize,staticText,new Font("Tahoma",9,FontStyle.Bold),ContentAlignment.MiddleLeft,offSetX,offSetY));
			_reportObjects[i].DataField=dataFieldName;
			_reportObjects[i].SummaryGroups=queryGroups;
			_sections["Group Footer"].Height+=(int)((grfx.MeasureString(staticText,new Font("Tahoma",9,FontStyle.Bold))).Height/grfx.DpiY*100+2)+offSetY;
			i=_reportObjects.Add(new ReportObject("GroupSummaryText","Group Footer",location,new Size(0,0),color,dataFieldName,operation,offSetX,offSetY));
			_reportObjects[i].SummaryGroups=queryGroups;
			grfx.Dispose();
		}

		///<summary>Submits the Query to the database and fills ReportTable with the results.  Returns false if the query fails.</summary>
		public bool SubmitQuery() {
			if(String.IsNullOrWhiteSpace(_query)) {
				//The programmer must have prefilled the data table already, so no reason to try and run a query.
			}
			else {
				try {
					_reportTable=Reports.GetTable(_query);
				}
				catch(Exception) {
					return false;
				}
			}
			if(_reportTable.Rows.Count==0) {//This should never throw an exception due to reportTable being null unless the programmer messed up when creating their report.
				return false;
			}
			_rowHeights=new List<int>();
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
					if(reportObject.ObjectKind==ReportObjectKind.FieldObject) {
						rawText=_reportTable.Rows[i][_dataFields.IndexOf(reportObject.DataField)].ToString();
						if(String.IsNullOrWhiteSpace(rawText)) {
							continue;
						}
						List<string> listString=GetDisplayString(rawText,prevDisplayText,reportObject,i);
						displayText=listString[0];
						prevDisplayText=listString[1];
						int curCellHeight=(int)(g.MeasureString(displayText,reportObject.Font,(int)(reportObject.Size.Width),ReportObject.GetStringFormatAlignment(reportObject.TextAlign))).Height;
						if(curCellHeight>rowHeight) {
							rowHeight=curCellHeight;
						}
					}
				}
				_rowHeights.Add(rowHeight);
			}
			g.Dispose();
			return true;
		}

		private List<string> GetDisplayString(string rawText,string prevDisplayText,ReportObject reportObject,int i) {
			string displayText="";
			List<string> retVals=new List<string>();
			if(reportObject.ValueType==FieldValueType.Age) {
				displayText=Patients.AgeToString(Patients.DateToAge(PIn.Date(rawText)));//(fieldObject.FormatString);
			}
			else if(reportObject.ValueType==FieldValueType.Boolean) {
				displayText=PIn.Bool(_reportTable.Rows[i][_dataFields.IndexOf(reportObject.DataField)].ToString()).ToString();//(fieldObject.FormatString);
				if(i>0 && reportObject.SuppressIfDuplicate) {
					prevDisplayText=PIn.Bool(_reportTable.Rows[i-1][_dataFields.IndexOf(reportObject.DataField)].ToString()).ToString();
				}
			}
			else if(reportObject.ValueType==FieldValueType.Date) {
				displayText=PIn.DateT(_reportTable.Rows[i][_dataFields.IndexOf(reportObject.DataField)].ToString()).ToString(reportObject.FormatString);
				if(i>0 && reportObject.SuppressIfDuplicate) {
					prevDisplayText=PIn.DateT(_reportTable.Rows[i-1][_dataFields.IndexOf(reportObject.DataField)].ToString()).ToString(reportObject.FormatString);
				}
			}
			else if(reportObject.ValueType==FieldValueType.Integer) {
				displayText=PIn.Long(_reportTable.Rows[i][_dataFields.IndexOf(reportObject.DataField)].ToString()).ToString(reportObject.FormatString);
				if(i>0 && reportObject.SuppressIfDuplicate) {
					prevDisplayText=PIn.Long(_reportTable.Rows[i-1][_dataFields.IndexOf(reportObject.DataField)].ToString()).ToString(reportObject.FormatString);
				}
			}
			else if(reportObject.ValueType==FieldValueType.Number) {
				displayText=PIn.Double(_reportTable.Rows[i][_dataFields.IndexOf(reportObject.DataField)].ToString()).ToString(reportObject.FormatString);
				if(i>0 && reportObject.SuppressIfDuplicate) {
					prevDisplayText=PIn.Double(_reportTable.Rows[i-1][_dataFields.IndexOf(reportObject.DataField)].ToString()).ToString(reportObject.FormatString);
				}
			}
			else if(reportObject.ValueType==FieldValueType.String) {
				displayText=rawText;
				if(i>0 && reportObject.SuppressIfDuplicate) {
					prevDisplayText=_reportTable.Rows[i-1][_dataFields.IndexOf(reportObject.DataField)].ToString();
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
			queryObj.ObjectKind=this.ObjectKind;//Doesn't need to be a deep copy.
			queryObj._sections=this._sections;//Doesn't need to be a deep copy.
			queryObj._dataFields=this._dataFields;//Doesn't need to be a deep copy.
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
			queryObj._rowHeights=new List<int>();
			for(int i=0;i<this._rowHeights.Count;i++) {
				queryObj._rowHeights.Add(this._rowHeights[i]);
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
			List<string> enumNamesNew=new List<string>();
			if(this._enumNames!=null) {
				for(int i=0;i<this._enumNames.Count;i++) {
					enumNamesNew.Add(this._enumNames[i]);
				}
			}
			queryObj._enumNames=enumNamesNew;
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
