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
		private ArrayList _dataFields=new ArrayList();
		private ReportObjectCollection _reportObjects=new ReportObjectCollection();
		private string _columnNameToSplitOn;
		private string _query;
		private DataTable _reportTable;
		private List<int> _rowHeights;
		private List<string> _enumNames;
		private SplitByKind _splitByKind;
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
				_reportTable=ReportTable;
			}
		}

		public List<string> EnumNames {
			get {
				return _enumNames;
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
				_rowHeights=RowHeights;
			}
		}

		///<summary>Default constructor.  Do not use.  Only used from DeepCopy()</summary>
		public QueryObject() {
		}

		public QueryObject(string query,string title,string columnNameToSplitOn,SplitByKind splitByKind,List<string> enumNames) {
			_columnNameToSplitOn=columnNameToSplitOn;
			_query=query;
			SectionName="Query";
			Name="Query";
			_splitByKind=splitByKind;
			_enumNames=enumNames;
			ObjectKind=ReportObjectKind.QueryObject;
			_sections.Add(new Section(AreaSectionKind.GroupTitle,0));
			_reportObjects.Add(new ReportObject("Group Title","Group Title",new Point(0,0),TextRenderer.MeasureText(title,new Font("Tahoma",14)),title,new Font("Tahoma",14),ContentAlignment.MiddleLeft));
			_sections.Add(new Section(AreaSectionKind.GroupHeader,0));
			_sections.Add(new Section(AreaSectionKind.Detail,0));
			_sections.Add(new Section(AreaSectionKind.GroupFooter,0));
		}

		public QueryObject(DataTable query,string title,string columnNameToSplitOn,SplitByKind splitByKind,List<string> enumNames) {
			_columnNameToSplitOn=columnNameToSplitOn;
			_reportTable=query;
			SectionName="Query";
			Name="Query";
			_splitByKind=splitByKind;
			_enumNames=enumNames;
			ObjectKind=ReportObjectKind.QueryObject;
			_sections.Add(new Section(AreaSectionKind.GroupTitle,0));
			_reportObjects.Add(new ReportObject("Group Title","Group Title",new Point(0,0),TextRenderer.MeasureText(title,new Font("Tahoma",14)),title,new Font("Tahoma",14),ContentAlignment.MiddleLeft));
			_sections.Add(new Section(AreaSectionKind.GroupHeader,0));
			_sections.Add(new Section(AreaSectionKind.Detail,0));
			_sections.Add(new Section(AreaSectionKind.GroupFooter,0));
		}

		public QueryObject(string query,string title,string columnNameToSplitOn,SplitByKind splitByKind):this(query,title,columnNameToSplitOn,splitByKind,null) {
			
		}

		public QueryObject(DataTable query,string title,string columnNameToSplitOn,SplitByKind splitByKind):this(query,title,columnNameToSplitOn,splitByKind,null) {
			
		}

		///<summary>Adds all the objects necessary for a typical column, including the textObject for column header and the fieldObject for the data.  If the column is type Double, then the alignment is set right and a total field is added. Also, default formatstrings are set for dates and doubles.  Does not add lines or shading.</summary>
		public void AddColumn(string dataField,int width,FieldValueType valueType) {
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
			//add textobject for column header
			font=new System.Drawing.Font("Tahoma",8,System.Drawing.FontStyle.Bold);
			size=TextRenderer.MeasureText(dataField,font);
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
			return;
		}

		///<summary>Submits the Query to the database and fills ReportTable with the results.  Returns false if the query fails.</summary>
		public bool SubmitQuery() {
			if(String.IsNullOrWhiteSpace(_query)) {
				//Do nothing
			}
			else {
				try {
					_reportTable=Reports.GetTable(_query);
				}
				catch(Exception) {
					return false;
				}
			}
			_rowHeights=new List<int>();
			for(int i=0;i<_reportTable.Rows.Count;i++) {
				string rawText;
				string displayText="";
				string prevDisplayText="";
				int rowHeight=0;
				foreach(ReportObject reportObject in _reportObjects) {
					if(reportObject.SectionName!="Detail") {
						continue;
					}
					rawText=_reportTable.Rows[i][_dataFields.IndexOf(reportObject.DataField)].ToString();
					List<string> listString=GetDisplayString(rawText,prevDisplayText,reportObject,i);
					displayText=listString[0];
					prevDisplayText=listString[1];
					int curCellHeight=(int)TextRenderer.MeasureText(displayText,reportObject.Font,reportObject.Size).Height;
					if(curCellHeight>rowHeight) {
						rowHeight=curCellHeight;
					}
				}
				_rowHeights.Add(rowHeight);
			}
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
			queryObj._rowHeights=new List<int>();
			for(int i=0;i<this._rowHeights.Count;i++) {
				queryObj._rowHeights.Add(this._rowHeights[i]);
			}
			ReportObjectCollection reportObjectsNew=new ReportObjectCollection();
			for(int i=0;i<this._reportObjects.Count;i++) {
				reportObjectsNew.Add(_reportObjects[i].DeepCopyReportObject());
			}
			queryObj._reportObjects=reportObjectsNew;
			queryObj._columnNameToSplitOn=_columnNameToSplitOn;
			//queryObj._query=this._query;
			queryObj._reportTable=new DataTable();
			//We only care about column headers at this point.  There is no easy way to copy an entire DataTable.
			for(int i=0;i<this.ReportTable.Columns.Count;i++) {
				queryObj._reportTable.Columns.Add(new DataColumn(this.ReportTable.Columns[i].ColumnName));
			}
			List<string> enumNamesNew=new List<string>();
			for(int i=0;i<this._enumNames.Count;i++) {
				enumNamesNew.Add(this._enumNames[i]);
			}
			queryObj._enumNames=enumNamesNew;
			queryObj._splitByKind=this._splitByKind;
			queryObj.IsPrinted=this.IsPrinted;
			return queryObj;
		}


	}
}
