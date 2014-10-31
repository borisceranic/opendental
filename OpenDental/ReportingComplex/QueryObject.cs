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

namespace OpenDental.ReportingComplex {

	///<summary></summary>
	public class QueryObject:ReportObject {
		private SectionCollection _sections=new SectionCollection();
		private ArrayList _dataFields=new ArrayList();
		private ReportObjectCollection _reportObjects=new ReportObjectCollection();
		private string _tableFromColumn;
		private string _query;
		private DataTable _reportTable;

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

		public ReportObjectCollection ReportObjects {
			get {
				return _reportObjects;
			}
		}

		public string TableFromColumn {
			get {
				return _tableFromColumn;
			}
		}

		public DataTable ReportTable {
			get {
				return _reportTable;
			}
		}

		public QueryObject(string query,string tableFromColumn) {
			_tableFromColumn=tableFromColumn;
			_query=query;
			SectionName="Query";
			Name="Query";
			ObjectKind=ReportObjectKind.QueryObject;
			_sections.Add(new Section(AreaSectionKind.GroupHeader,0));
			_sections.Add(new Section(AreaSectionKind.Detail,0));
			_sections.Add(new Section(AreaSectionKind.GroupFooter,0));
		}

		/// <summary>Adds all the objects necessary for a typical column, including the textObject for column header and the fieldObject for the data.  Does not add lines or shading. If the column is type Double, then the alignment is set right and a total field is added. Also, default formatstrings are set for dates and doubles.</summary>
		public void AddColumn(string dataField,int width,FieldValueType valueType) {
			_dataFields.Add(dataField);
			//FormReport FormR=new FormReport();
			//Graphics grfx=FormR.CreateGraphics();
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
				//		Size size=new Size((int)(grfx.MeasureString(title,font).Width/grfx.DpiX*100+2)
				//,(int)(grfx.MeasureString(title,font).Height/grfx.DpiY*100+2));
			if(_sections["Group Header"].Height==0) {
				_sections["Group Header"].Height=size.Height;
			}
			int xPos=0;
			//find next available xPos
			foreach(ReportObject reportObject in _reportObjects) {
				if(reportObject.SectionName!="Group Header") continue;
				if(reportObject.Location.X+reportObject.Size.Width > xPos) {
					xPos=reportObject.Location.X+reportObject.Size.Width;
				}
			}
			_reportObjects.Add(new ReportObject(dataField+"Header","Group Header"
				,new Point(xPos,0),new Size(width,size.Height),dataField,font,textAlign));
			//add fieldObject for rows in details section
			font=new Font("Tahoma",9);
			if(_sections["Detail"].Height==0) {
				_sections["Detail"].Height=size.Height;
			}
			_reportObjects.Add(new ReportObject(dataField+"Detail","Detail"
				,new Point(xPos,0),new Size(width,size.Height)
				,dataField,valueType
				//,new FieldDef(dataField,valueType)
				,font,textAlign,formatString));
			//add fieldObject for total in ReportFooter
			if(valueType==FieldValueType.Number) {
				font=new Font("Tahoma",9,FontStyle.Bold);
				//use same size as already set for otherFieldObjects above
				if(_sections["Group Footer"].Height==0) {
					_sections["Group Footer"].Height=size.Height;
				}
				_reportObjects.Add(new ReportObject(dataField+"Footer","Group Footer"
					,new Point(xPos,0),new Size(width,size.Height)
					,SummaryOperation.Sum,dataField
					//,new FieldDef("Sum"+dataField,SummaryOperation.Sum
					//,GetLastRO(ReportObjectKind.FieldObject).DataSource)
					,font,textAlign,formatString));
			}
			return;
		}

		///<summary>Submits the Query to the database and fills ReportTable with the results.  Returns false if the user clicks Cancel on the Parameters dialog.</summary>
		public bool SubmitQuery() {
			string outputQuery=_query;
			//Removed code for custom parameter selection
			_reportTable=Reports.GetTable(outputQuery);
			return true;
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
			height+=_sections["Group Header"].Height;
			height+=_sections["Detail"].Height;
			height+=_sections["Group Footer"].Height;
			return height;
		}

		///<summary>If the specified section exists, then this returns its height. Otherwise it returns 0.</summary>
		public int GetSectionHeight(string sectionName) {
			return _sections[sectionName].Height;
		}

	}
}
