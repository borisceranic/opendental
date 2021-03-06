using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace OpenDental {
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormFeatureRequest:ODForm {
		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.Label label1;
		private IContainer components;
		private Label labelVote;
		private Label label5;
		private TextBox textSearch;
		private OpenDental.UI.ODGrid gridMain;
		private OpenDental.UI.Button buttonAdd;
		private Label labelSearchFirst;
		private OpenDental.UI.Button butSearch;
		private ODDataTable table;
		private bool isAdminMode;
		private UI.Button butOK;
		///<summary>Used in the JobManager system for attaching features to jobs.</summary>
		public bool IsSelectionMode;
		///<summary>Only for IsSelectionMode, returns the selected num.</summary>
		public long SelectedFeatureNum=0;

		///<summary></summary>
		public FormFeatureRequest()
		{
			components=new System.ComponentModel.Container();
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFeatureRequest));
			this.label1 = new System.Windows.Forms.Label();
			this.labelVote = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.textSearch = new System.Windows.Forms.TextBox();
			this.labelSearchFirst = new System.Windows.Forms.Label();
			this.butSearch = new OpenDental.UI.Button();
			this.buttonAdd = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butClose = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 23);
			this.label1.TabIndex = 0;
			// 
			// labelVote
			// 
			this.labelVote.Location = new System.Drawing.Point(359, 7);
			this.labelVote.Name = "labelVote";
			this.labelVote.Size = new System.Drawing.Size(511, 16);
			this.labelVote.TabIndex = 51;
			this.labelVote.Text = "Vote for your favorite features here.  Please remember that we cannot ever give a" +
    "ny time estimates.";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(4, 5);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(90, 18);
			this.label5.TabIndex = 56;
			this.label5.Text = "Search terms";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textSearch
			// 
			this.textSearch.Location = new System.Drawing.Point(93, 5);
			this.textSearch.Name = "textSearch";
			this.textSearch.Size = new System.Drawing.Size(167, 20);
			this.textSearch.TabIndex = 57;
			// 
			// labelSearchFirst
			// 
			this.labelSearchFirst.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelSearchFirst.Location = new System.Drawing.Point(91, 633);
			this.labelSearchFirst.Name = "labelSearchFirst";
			this.labelSearchFirst.Size = new System.Drawing.Size(180, 18);
			this.labelSearchFirst.TabIndex = 61;
			this.labelSearchFirst.Text = "A search is required first";
			this.labelSearchFirst.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelSearchFirst.Visible = false;
			// 
			// butSearch
			// 
			this.butSearch.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSearch.Autosize = true;
			this.butSearch.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSearch.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSearch.CornerRadius = 4F;
			this.butSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butSearch.Location = new System.Drawing.Point(266, 2);
			this.butSearch.Name = "butSearch";
			this.butSearch.Size = new System.Drawing.Size(75, 24);
			this.butSearch.TabIndex = 62;
			this.butSearch.Text = "Search";
			this.butSearch.Click += new System.EventHandler(this.butSearch_Click);
			// 
			// buttonAdd
			// 
			this.buttonAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonAdd.Autosize = true;
			this.buttonAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.buttonAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.buttonAdd.CornerRadius = 4F;
			this.buttonAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.buttonAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonAdd.Location = new System.Drawing.Point(12, 630);
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.Size = new System.Drawing.Size(75, 24);
			this.buttonAdd.TabIndex = 60;
			this.buttonAdd.Text = "Add";
			this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 28);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(861, 599);
			this.gridMain.TabIndex = 59;
			this.gridMain.Title = "Feature Requests";
			this.gridMain.TranslationName = null;
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(798, 630);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(717, 630);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 63;
			this.butOK.Text = "&OK";
			this.butOK.Visible = false;
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// FormFeatureRequest
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(882, 657);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butSearch);
			this.Controls.Add(this.labelSearchFirst);
			this.Controls.Add(this.buttonAdd);
			this.Controls.Add(this.textSearch);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.labelVote);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(870, 300);
			this.Name = "FormFeatureRequest";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Feature Requests";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormUpdate_FormClosing);
			this.Load += new System.EventHandler(this.FormFeatureRequest_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormFeatureRequest_Load(object sender, System.EventArgs e) {
			if(IsSelectionMode) {
				this.Text="Select a Feature Request";
				butClose.Text="Cancel";
				butOK.Visible=true;
				buttonAdd.Visible=false;
				labelSearchFirst.Visible=false;
				labelVote.Visible=false;
			}
			/*
				if(Security.IsAuthorized(Permissions.Setup,true)) {
					butCheck2.Visible=true;
				}
				else {
					textConnectionMessage.Text=Lan.g(this,"Not authorized for")+" "+GroupPermissions.GetDesc(Permissions.Setup);
				}
			*/
			//if(!Synch()){
			//	return;
			//}
			//FillGrid();
		}

		private void butSearch_Click(object sender,EventArgs e) {
			gridMain.SetSelected(false);
			try{
				FillGrid();
			}
			catch{
				MsgBox.Show(this,"This feature won't work until you install Microsoft dotNET 3.5.");
			}
		}

		private void FillGrid(){
			//if(textSearch.Text.Length<3){
			//	MsgBox.Show(this,"Please enter a search term with at least three letters in it.");
			//	return;
			//}
			Cursor=Cursors.WaitCursor;
			//Yes, this would be slicker if it were asynchronous, but no time right now.
			//prepare the xml document to send--------------------------------------------------------------------------------------
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = ("    ");
			StringBuilder strbuild=new StringBuilder();
			using(XmlWriter writer=XmlWriter.Create(strbuild,settings)){
				writer.WriteStartElement("FeatureRequestGetList");
				writer.WriteStartElement("RegistrationKey");
				writer.WriteString(PrefC.GetString(PrefName.RegistrationKey));
				writer.WriteEndElement();
				writer.WriteStartElement("SearchString");
				writer.WriteString(textSearch.Text);
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
			#if DEBUG
				OpenDental.localhost.Service1 updateService=new OpenDental.localhost.Service1();
			#else
				OpenDental.customerUpdates.Service1 updateService=new OpenDental.customerUpdates.Service1();
				updateService.Url=PrefC.GetString(PrefName.UpdateServerAddress);
			#endif
			//Send the message and get the result-------------------------------------------------------------------------------------
			string result="";
			try {
				result=updateService.FeatureRequestGetList(strbuild.ToString());
			}
			catch(Exception ex) {
				Cursor=Cursors.Default;
				MessageBox.Show("Error: "+ex.Message);
				return;
			}
			//textConnectionMessage.Text=Lan.g(this,"Connection successful.");
			//Application.DoEvents();
			Cursor=Cursors.Default;
			//MessageBox.Show(result);
			XmlDocument doc=new XmlDocument();
			doc.LoadXml(result);
			//Process errors------------------------------------------------------------------------------------------------------------
			XmlNode node=doc.SelectSingleNode("//Error");
			if(node!=null) {
				//textConnectionMessage.Text=node.InnerText;
				MessageBox.Show(node.InnerText,"Error");
				return;
			}
			node=doc.SelectSingleNode("//KeyDisabled");
			if(node==null) {
				//no error, and no disabled message
				if(Prefs.UpdateBool(PrefName.RegistrationKeyIsDisabled,false)) {//this is one of two places in the program where this happens.
					DataValid.SetInvalid(InvalidType.Prefs);
				}
			}
			else {
				//textConnectionMessage.Text=node.InnerText;
				MessageBox.Show(node.InnerText);
				if(Prefs.UpdateBool(PrefName.RegistrationKeyIsDisabled,true)) {//this is one of two places in the program where this happens.
					DataValid.SetInvalid(InvalidType.Prefs);
				}
				return;
			}
			//Process a valid return value------------------------------------------------------------------------------------------------
			node=doc.SelectSingleNode("//ResultTable");
			table=new ODDataTable(node.InnerXml);
			table.Rows.Sort(FeatureRequestSort);//Sort user submited/voted features to the top. 
			//Admin mode----------------------------------------------------------------------------------------------------------------
			node=doc.SelectSingleNode("//IsAdminMode");
			if(node.InnerText=="true"){
				isAdminMode=true;
			}
			else{
				isAdminMode=false;
			}
			//FillGrid used to start here------------------------------------------------
			long selectedRequestId=0;
			int selectedIndex=gridMain.GetSelectedIndex();
			if(selectedIndex!=-1){
				if(table.Rows.Count>selectedIndex){
					selectedRequestId=PIn.Long(table.Rows[gridMain.GetSelectedIndex()]["RequestId"]);
				}
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableRequest","Req#"),40);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRequest","Mine"),40);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRequest","My Votes"),60);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRequest","Total Votes"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRequest","Diff"),40);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRequest","Weight"),45);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRequest","Approval"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRequest","Description"),500);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<table.Rows.Count;i++){
				row=new ODGridRow();
				row.Cells.Add(table.Rows[i]["RequestId"]);
				row.Cells.Add(table.Rows[i]["isMine"]);
				row.Cells.Add(table.Rows[i]["myVotes"]);
			  row.Cells.Add(table.Rows[i]["totalVotes"]);
				row.Cells.Add(table.Rows[i]["Difficulty"]);
				row.Cells.Add(table.Rows[i]["Weight"]);
				row.Cells.Add(table.Rows[i]["approval"]);
				row.Cells.Add(table.Rows[i]["Description"]);
				//If they voted or pledged on this feature, mark it so they can see. Can be re-added when/if they need to be more visible.
				if(table.Rows[i]["isMine"].ToString()!=""
					&& table.Rows[i]["personalVotes"].ToString()=="0"
					&& table.Rows[i]["personalCrit"].ToString()=="0"
					&& table.Rows[i]["personalPledged"].ToString()=="0"
					&& table.Rows[i]["approval"].ToString()!="Complete") 
				{
					row.ColorBackG=Color.FromArgb(255,255,230);//light yellow.
				}
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			for(int i=0;i<table.Rows.Count;i++){
				if(selectedRequestId.ToString()==table.Rows[i]["RequestId"]){
					gridMain.SetSelected(i,true);
				}
			}
		}

		private void buttonAdd_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"The majority of feature requests that users submit are duplicates of existing requests.  Please take the time to do a thorough search for different keywords and become familiar with similar requests before adding one of your own.  Continue?")) 
			{
				return;
			}
			FormRequestEdit FormR=new FormRequestEdit();
			//FormR.IsNew=true;
			FormR.IsAdminMode=isAdminMode;
			FormR.ShowDialog();
			textSearch.Text="";//so we can see our new request
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(IsSelectionMode) {
				SelectedFeatureNum=PIn.Long(table.Rows[e.Row]["RequestId"]);
				DialogResult=DialogResult.OK;
				return;
			}
			FormRequestEdit FormR=new FormRequestEdit();
			FormR.RequestId=PIn.Long(table.Rows[e.Row]["RequestId"]);
			FormR.IsAdminMode=isAdminMode;
			FormR.ShowDialog();
			FillGrid();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select a feature request.");
				return;
			}
			SelectedFeatureNum=PIn.Long(table.Rows[gridMain.GetSelectedIndex()]["RequestId"]);
			DialogResult=DialogResult.OK;
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			if(IsSelectionMode) {
				DialogResult=DialogResult.Cancel;
			}
			Close();
		}

		private void FormUpdate_FormClosing(object sender,FormClosingEventArgs e) {
			
		}

		///<summary>For sorting FRs because we do not have access to the ApprovalEnum in Resuests.cs in the WebServiceCustomerUpdates solution.</summary>
		private static List<string> _arrayApprovalEnumStrings=new List<string> {
			"New",//0
			"NeedsClarification",//1
			"Redundant",//2
			"TooBroad",//3
			"NotARequest",//4
			"AlreadyDone",//5
			"Obsolete",//6
			"Approved",//7
			"InProgress",//8
			"Complete"//9
		};

		///<summary>Used to sort feature requests for user to the top of the list.</summary>
		private int FeatureRequestSort(ODDataRow x,ODDataRow y) {
			//Sorting order
			//  1) Complete items to the bottom
			//  2) Personal requests to top
			//  3) Personal requests sorted by weight
			//  4) Group by "Mine"
			//  5) Weight by magnitude
			//  6) Request ID by magnitude
			//Part 1; complete items to the bottom
			int xIdx=_arrayApprovalEnumStrings.FindIndex(e=>e==x["approval"].ToString());
			int yIdx=_arrayApprovalEnumStrings.FindIndex(e=>e==y["approval"].ToString());
			if(xIdx!=yIdx && (xIdx==9||yIdx==9)) {// 9=="complete"
				return (xIdx==9).CompareTo(yIdx==9);
			}
			//Part 2
			bool xIsPersonal=false;
			bool yIsPersonal=false;
			xIsPersonal=x["personalVotes"].ToString()!="0" || x["personalCrit"].ToString()!="0" || x["personalPledged"].ToString()!="0";
			yIsPersonal=y["personalVotes"].ToString()!="0" || y["personalCrit"].ToString()!="0" || y["personalPledged"].ToString()!="0";
			if(xIsPersonal!=yIsPersonal) {
				return -xIsPersonal.CompareTo(yIsPersonal);//negative comparison to move personal items to top.
			}
			//Part 3
			if(xIsPersonal && yIsPersonal) {
				if(x["Weight"].ToString()!=y["Weight"].ToString()) {
					return -(PIn.Double(x["Weight"].ToString()).CompareTo(PIn.Double(y["Weight"].ToString())));//Larger number of votes go to the top
				}
			}
			//Part 4; Sort "Mine" entries above non-"Mine" entries.  "Mine" means any feature that this office has submitted.
			if(x["isMine"].ToString()!=y["isMine"].ToString()) {//One is the customer's, the other isn't.
				return -(x["isMine"].ToString().CompareTo(y["isMine"].ToString()));//It will either be "" or X, and "X" goes to the top
			}
			//Part 5
			if(x["Weight"].ToString()!=y["Weight"].ToString()) {
				return -(PIn.Double(x["Weight"].ToString()).CompareTo(PIn.Double(y["Weight"].ToString())));//Larger number of votes go to the top
			}
			//Part 6
			return PIn.Long(x["RequestId"].ToString()).CompareTo(PIn.Long(y["RequestId"].ToString()));
		}

	

		

		

		

		

		

		

	

	

		

	

	}

	
}





















