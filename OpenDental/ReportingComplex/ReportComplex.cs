using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental.ReportingComplex
{


	/// <summary>This class is loosely modeled after CrystalReports.ReportDocument, but with less inheritence and heirarchy.</summary>
	public class ReportComplex{
		private ArrayList dataFields;
		private SectionCollection sections;
		private ReportObjectCollection reportObjects;
		private ParameterFieldCollection parameterFields;
		//private Margins reportMargins; //Never set anywhere, so it is not needed!
		private bool _isLandscape;
		private bool _hasGridLines;
		private List<string> _listQueries;
		private List<DataTable> _reportTables;
		private string reportName;
		private string description;
		private string authorID;
		private int letterOrder;
		///<summary>This is simply used to measure strings for alignment purposes.</summary>
		private Graphics grfx;

		#region Properties
		///<summary>Collection of strings representing available datatable field names. For now, only one table is allowed, so each string will represent a column.</summary>
		public ArrayList DataFields{
			get{
				return dataFields;
			}
			set{
				dataFields=value;
			}
		}
		///<summary>Collection of Sections.</summary>
		public SectionCollection Sections{
			get{
				return sections;
			}
			set{
				sections=value;
			}
		}
		///<summary>A collection of ReportObjects</summary>
		public ReportObjectCollection ReportObjects{
			get{
				return reportObjects;
			}
			set{
				reportObjects=value;
			}
		}
		///<summary>Collection of ParameterFields that are available for the query.</summary>
		public ParameterFieldCollection ParameterFields{
			get{
				return parameterFields;
			}
			set{
				parameterFields=value;
			}
		}
		///<summary>margins will be null unless set by user. When printing, if margins are null, the defaults will depend on the page orientation.</summary>
		public Margins ReportMargins{
			get{
				//return reportMargins; //reportMargins is always null!
				return null;
			}
		}
		///<summary></summary>
		public bool IsLandscape{
			get{
				return _isLandscape;
			}
			set{
				_isLandscape=value;
			}
		}
		///<summary>The query will get altered before it is actually used to retrieve. Any parameters will be replaced with user entered data without saving those changes.</summary>
		public List<string> Queries{
			get{
				return _listQueries;
			}
			set{
				_listQueries=value;
			}
		}
		///<summary>The datatable that is returned from the database.</summary>
		public List<DataTable> ReportTables{
			get{
				return _reportTables;
			}
			set{
				_reportTables=value;
			}
		}
		///<summary>The name to display in the menu.</summary>
		public string ReportName{
			get{
				return reportName;
			}
			set{
				reportName=value;
			}
		}
		///<summary>Gives the user a description and some guidelines about what they can expect from this report.</summary>
		public string Description{
			get{
				return description;
			}
			set{
				description=value;
			}
		}
		///<summary>For instance OD12 or JoeDeveloper9.  If you are a developer releasing reports, then this should be your name or company followed by a unique number.  This will later make it easier to maintain your reports for your customers.  All reports that we release will be of the form OD##.  Reports that the user creates will have this field blank.</summary>
		public string AuthorID{
			get{
				return authorID;
			}
			set{
				authorID=value;
			}
		}
		///<summary>The 1-based order to show in the Letter menu, or 0 to not show in that menu.</summary>
		public int LetterOrder{
			get{
				return letterOrder;
			}
			set{
				letterOrder=value;
			}
		}
		
		#endregion

		///<summary>This can add a title, subtitle, grid lines, and pagenums to the report using defaults. If the parameters are blank or false the object will not be added.</summary>
		public ReportComplex(string title,string subTitle,bool hasGridLines,bool hasPageNums) {
			sections=new SectionCollection();
			reportObjects=new ReportObjectCollection();
			dataFields=new ArrayList();
			parameterFields=new ParameterFieldCollection();
			grfx=Graphics.FromImage(new Bitmap(1,1));//I'm still trying to find a better way to do this
			if(!String.IsNullOrWhiteSpace(title)) {
				AddTitle(title);
			}
			if(!String.IsNullOrWhiteSpace(subTitle)) {
				AddSubTitle("Default",subTitle);
			}
			if(hasGridLines) {
				AddGridLines();
			}
			if(hasPageNums) {
				AddPageNum();
			}
		}

		/// <summary>Adds a ReportObject large, centered, and bold, to the top of the Report Header Section.  Should only be done once, and done before any subTitles.</summary>
		/// <param name="title">The text of the title.</param>
		private void AddTitle(string title){
			//FormReport FormR=new FormReport();
			//this is just to get a graphics object. There must be a better way.
			//Graphics grfx=FormR.CreateGraphics();
			Font font=new Font("Tahoma",17,FontStyle.Bold);
			Size size=new Size((int)(grfx.MeasureString(title,font).Width/grfx.DpiX*100+2)
				,(int)(grfx.MeasureString(title,font).Height/grfx.DpiY*100+2));
			int xPos;
			if(_isLandscape) {
				xPos=1100/2;
				xPos-=50;
			}
			else {
				xPos=850/2;
				xPos-=30;
			}
			xPos-=(int)(size.Width/2);
			if(sections["Report Header"]==null) {
				sections.Add(new Section(AreaSectionKind.ReportHeader,0));
			}
			reportObjects.Add(
				new ReportObject("Title","Report Header",new Point(xPos,0),size,title,font,ContentAlignment.MiddleCenter));
			//this it the only place a white buffer is added to a header.
			sections["Report Header"].Height=(int)size.Height+20;
			//grfx.Dispose();
			//FormR.Dispose();
		}

		/// <summary>Adds a ReportObject, centered and bold, at the bottom of the Report Header Section.  Should only be done after AddTitle.  You can add as many subtitles as you want.</summary>
		public void AddSubTitle(string name,string subTitle){
			Font font=new Font("Tahoma",10,FontStyle.Bold);
			Size size=new Size((int)(grfx.MeasureString(subTitle,font).Width/grfx.DpiX*100+2)
				,(int)(grfx.MeasureString(subTitle,font).Height/grfx.DpiY*100+2));
			int xPos;
			if(_isLandscape) {
				xPos=1100/2;
				xPos-=50;
			}
			else {
				xPos=850/2;
			xPos-=30;
			}
			xPos-=(int)(size.Width/2);
			if(sections["Report Header"]==null){
				sections.Add(new Section(AreaSectionKind.ReportHeader,0));	
			}
			//find the yPos+Height of the last reportObject in the Report Header section
			int yPos=0;
			foreach(ReportObject reportObject in reportObjects){
				if(reportObject.SectionName!="Report Header") continue;
				if(reportObject.Location.Y+reportObject.Size.Height > yPos){
					yPos=reportObject.Location.Y+reportObject.Size.Height;
				}
			}
			reportObjects.Add(
				new ReportObject(name,"Report Header",new Point(xPos,yPos+5),size,subTitle,font,ContentAlignment.MiddleCenter));
			sections["Report Header"].Height+=(int)size.Height+5;
		}

		public QueryObject AddQuery(string query,string tableFromColumn) {
			QueryObject queryObj=new QueryObject(query,tableFromColumn);
			reportObjects.Add(queryObj);
			return queryObj;
		}

		/// <summary></summary>
		public void AddLine(string name,string sectionName,Color color,float lineThickness,LineOrientation lineOrientation,LinePosition linePosition,int linePercent,int offSetX,int offSetY) {
			reportObjects.Add(new ReportObject(name,sectionName,color,lineThickness,lineOrientation,linePosition,linePercent,offSetX,offSetY));
		}

		/// <summary></summary>
		public void AddBox(string name,string sectionName,Color color,float lineThickness,int offSetX,int offSetY) {
			reportObjects.Add(new ReportObject(name,sectionName,color,lineThickness,offSetX,offSetY));
		}

		public ReportObject GetObjectByName(string name){
			for(int i=reportObjects.Count-1;i>=0;i--){//search from the end backwards
				if(reportObjects[i].Name==name) {
					return ReportObjects[i];
				}
			}
			MessageBox.Show("end of loop");
			return null;
		}

		public ReportObject GetTitle() {
			//ReportObject ro=null;
			for(int i=reportObjects.Count-1;i>=0;i--) {//search from the end backwards
				if(reportObjects[i].Name=="Title") {
					return ReportObjects[i];
				}
			}
			MessageBox.Show("end of loop");
			return null;
		}

		public ReportObject GetSubTitle() {
			//ReportObject ro=null;
			for(int i=reportObjects.Count-1;i>=0;i--) {//search from the end backwards
				if(reportObjects[i].Name=="SubTitle") {
					return ReportObjects[i];
				}
			}
			MessageBox.Show("end of loop");
			return null;
		}

		/// <summary>Put a pagenumber object on lower left of page footer section. Object is named PageNum.</summary>
		public void AddPageNum(){
			//add page number
			Font font=new Font("Tahoma",9);
			Size size=new Size(150,(int)(grfx.MeasureString("anytext",font).Height/grfx.DpiY*100+2));
			if(sections["Page Footer"]==null){
				sections.Add(new Section(AreaSectionKind.PageFooter,0));	
			}
			if(sections["Page Footer"].Height==0){
				sections["Page Footer"].Height=size.Height;
			}
			reportObjects.Add(new ReportObject("PageNum","Page Footer"
				,new Point(0,0),size
				,FieldValueType.String,SpecialFieldType.PageNumber
				,font,ContentAlignment.MiddleLeft,""));
		}

		public void AddGridLines() {
			_hasGridLines=true;
		}
		
		/*public void AddParameter(string name,ParameterValueKind valueKind){
			ParameterFields.Add(new ParameterFieldDefinition(name,valueKind));
		}*/

		///<summary>Adds a parameterField which will be used in the query to represent user-entered data.</summary>
		///<param name="myName">The unique formula name of the parameter.</param>
		///<param name="myValueType">The data type that this parameter stores.</param>
		///<param name="myDefaultValue">The default value of the parameter</param>
		///<param name="myPromptingText">The text to prompt the user with.</param>
		///<param name="mySnippet">The SQL snippet that this parameter represents.</param>
		public void AddParameter(string myName,FieldValueType myValueType
			,object myDefaultValue,string myPromptingText,string mySnippet){
			parameterFields.Add(new ParameterField(myName,myValueType,myDefaultValue,myPromptingText,mySnippet));
		}

		/// <summary>Overload for ValueKind enum.</summary>
		public void AddParameter(string myName,FieldValueType myValueType
			,ArrayList myDefaultValues,string myPromptingText,string mySnippet,EnumType myEnumerationType){
			parameterFields.Add(new ParameterField(myName,myValueType,myDefaultValues,myPromptingText,mySnippet,myEnumerationType));
		}

		/// <summary>Overload for ValueKind defCat.</summary>
		public void AddParameter(string myName,FieldValueType myValueType
			,ArrayList myDefaultValues,string myPromptingText,string mySnippet,DefCat myDefCategory){
			parameterFields.Add(new ParameterField(myName,myValueType,myDefaultValues,myPromptingText,mySnippet,myDefCategory));
		}

		/// <summary>Overload for ValueKind defCat.</summary>
		public void AddParameter(string myName,FieldValueType myValueType
			,ArrayList myDefaultValues,string myPromptingText,string mySnippet,ReportFKType myReportFKType){
			parameterFields.Add(new ParameterField(myName,myValueType,myDefaultValues,myPromptingText,mySnippet,myReportFKType));
		}

		///<summary>Submits the Query to the database and fills ReportTable with the results.  Returns false if the user clicks Cancel on the Parameters dialog.</summary>
		public bool SubmitQueries(){
			sections.Add(new Section(AreaSectionKind.Query,0));
			for(int i=0;i<reportObjects.Count;i++) {
				if(reportObjects[i].ObjectKind==ReportObjectKind.QueryObject) {
					QueryObject query=(QueryObject)reportObjects[i];
					query.SubmitQuery();
				}
			}
			return true;
		}

		///<summary>If the specified section exists, then this returns its height. Otherwise it returns 0.</summary>
		public int GetSectionHeight(string sectionName) {
			if(!sections.Contains(sectionName)) {
				return 0;
			}
			return sections[sectionName].Height;
		}

		public bool HasGridLines() {
			return _hasGridLines;
		}

		/*
		/// <summary>Add Simple. This is used when there is only a single page in the report and all elements are added to the report header.  Height is set to one row, and all width is set to full page width of 850. There are no other sections to the report.</summary>
		public void AddSimp(string text,int xPos,int yPos,Font font){
			FormReport FormR=new FormReport();
			Graphics grfx=FormR.CreateGraphics();
			Size size=grfx.MeasureString(text,font);
			Section section=Sections.GetOfKind(AreaSectionKind.ReportHeader);
			if(section.Height<1100)
				section.Height=1100;
			ReportObjects.Add(new TextObject(Sections.IndexOf(section),new Point(xPos,yPos)
				,new Size(850,size.Height+2),text,font,ContentAlignment.MiddleLeft));
			grfx.Dispose();
			FormR.Dispose();
		}

		public void AddSimp(string text,int xPos,int yPos){
			AddSimp(text,xPos,yPos,new Font("Tahoma",9));
		}*/

		

	}
}











