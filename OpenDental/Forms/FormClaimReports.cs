using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using OpenDental.Eclaims;
using OpenDentBusiness;
using CodeBase;
using System.Net;
using System.Collections.Generic;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormClaimReports : ODForm {
		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.Label labelRetrieving;
		private System.Windows.Forms.ComboBox comboClearhouse;
		private OpenDental.UI.Button butRetrieve;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		///<summary>If true, then reports will be automatically retrieved for default clearinghouse.  Then this form will close.</summary>
		public bool AutomaticMode;

		///<summary></summary>
		public FormClaimReports()
		{
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClaimReports));
			this.labelRetrieving = new System.Windows.Forms.Label();
			this.comboClearhouse = new System.Windows.Forms.ComboBox();
			this.butRetrieve = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// labelRetrieving
			// 
			this.labelRetrieving.Font = new System.Drawing.Font("Microsoft Sans Serif",8.25F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.labelRetrieving.Location = new System.Drawing.Point(12,72);
			this.labelRetrieving.Name = "labelRetrieving";
			this.labelRetrieving.Size = new System.Drawing.Size(366,20);
			this.labelRetrieving.TabIndex = 1;
			this.labelRetrieving.Text = "Retrieving reports from selected clearinghouse.";
			this.labelRetrieving.Visible = false;
			// 
			// comboClearhouse
			// 
			this.comboClearhouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClearhouse.Location = new System.Drawing.Point(18,32);
			this.comboClearhouse.Name = "comboClearhouse";
			this.comboClearhouse.Size = new System.Drawing.Size(187,21);
			this.comboClearhouse.TabIndex = 2;
			// 
			// butRetrieve
			// 
			this.butRetrieve.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butRetrieve.Autosize = true;
			this.butRetrieve.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRetrieve.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRetrieve.CornerRadius = 4F;
			this.butRetrieve.Location = new System.Drawing.Point(222,29);
			this.butRetrieve.Name = "butRetrieve";
			this.butRetrieve.Size = new System.Drawing.Size(90,26);
			this.butRetrieve.TabIndex = 5;
			this.butRetrieve.Text = "Retrieve";
			this.butRetrieve.Click += new System.EventHandler(this.butRetrieve_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(289,152);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75,26);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// FormClaimReports
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.ClientSize = new System.Drawing.Size(401,202);
			this.Controls.Add(this.butRetrieve);
			this.Controls.Add(this.comboClearhouse);
			this.Controls.Add(this.labelRetrieving);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormClaimReports";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "E-claim Reports";
			this.Load += new System.EventHandler(this.FormClaimReports_Load);
			this.Shown += new System.EventHandler(this.FormClaimReports_Shown);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormClaimReports_Load(object sender, System.EventArgs e) {
			Clearinghouse[] arrayHqClearinghouses=Clearinghouses.GetHqListt();
			for(int i=0;i<arrayHqClearinghouses.Length;i++){
				comboClearhouse.Items.Add(arrayHqClearinghouses[i].Description);
				if(PrefC.GetLong(PrefName.ClearinghouseDefaultDent)==arrayHqClearinghouses[i].ClearinghouseNum){
					comboClearhouse.SelectedIndex=i;
				}
			}
			if(comboClearhouse.Items.Count>0 && comboClearhouse.SelectedIndex==-1){
				comboClearhouse.SelectedIndex=0;
			}
		}

		private void FormClaimReports_Shown(object sender,EventArgs e) {
			if(AutomaticMode) {
				labelRetrieving.Visible=true;
				Cursor=Cursors.WaitCursor;
				Clearinghouse[] arrayHqClearinghouses=Clearinghouses.GetHqListt();
				Clearinghouse clearinghouseHq=arrayHqClearinghouses[comboClearhouse.SelectedIndex];
				Clearinghouse clearinghouseClin=Clearinghouses.OverrideFields(clearinghouseHq,Clinics.ClinicNum);
				string errorMessage=RetrieveAndImport(clearinghouseClin,AutomaticMode);
				if(errorMessage!="") {
					MessageBox.Show(errorMessage);
				}
				Cursor=Cursors.Default;
				Close();
			}
		}

		private void butRetrieve_Click(object sender,EventArgs e) {
			if(comboClearhouse.SelectedIndex==-1) {
				MsgBox.Show(this,"Please select a clearinghouse first.");
				return;
			}
			Clearinghouse[] arrayClearinghouses=Clearinghouses.GetHqListt();
			Clearinghouse clearhouseHq=arrayClearinghouses[comboClearhouse.SelectedIndex];
			Clearinghouse clearinghouseClin=Clearinghouses.OverrideFields(clearhouseHq,Clinics.ClinicNum);

			if(!Directory.Exists(clearinghouseClin.ResponsePath)) {
				MsgBox.Show(this,"Clearinghouse does not have a valid Report Path set.");
				return;
			}
			//For Tesia, user wouldn't normally manually retrieve.
			if(clearhouseHq.ISA08=="113504607") {
				if((PrefC.RandomKeys && !PrefC.GetBool(PrefName.EasyNoClinics))//See FormClaimsSend_Load
					|| PrefC.GetLong(PrefName.ClearinghouseDefaultDent)!=clearhouseHq.ClearinghouseNum) //default
				{
					//But they might need to in these situations.
					string errorMessage=RetrieveAndImport(clearinghouseClin,false);
					if(errorMessage=="") {
						MsgBox.Show("FormClaimReports","Retrieval successful");
					}
					else {
						MessageBox.Show(errorMessage);
					}
					MsgBox.Show(this,"Done");
				}
				else{
					MsgBox.Show(this,"No need to Retrieve.  Available reports are automatically downloaded every three minutes.");
				}
				return;
			}
			if(clearhouseHq.CommBridge==EclaimsCommBridge.None
				|| clearhouseHq.CommBridge==EclaimsCommBridge.Renaissance
				|| clearhouseHq.CommBridge==EclaimsCommBridge.RECS) {
				MsgBox.Show(this,"No built-in functionality for retrieving reports from this clearinghouse.");
				return;
			}
			if(!MsgBox.Show(this,true,"Connect to clearinghouse to retrieve reports?")) {
				return;
			}
			labelRetrieving.Visible=true;
			Cursor=Cursors.WaitCursor;
			string errorMesssage=RetrieveAndImport(clearinghouseClin,false);
			if(errorMesssage=="") {
				MsgBox.Show("FormClaimReports","Retrieval successful");
			}
			else {
				MessageBox.Show(errorMesssage);
			}
			Cursor=Cursors.Default;
			labelRetrieving.Visible=false;
			MsgBox.Show(this,"Done");
		}

		private static string RetrieveReports(Clearinghouse clearinghouseClin,bool isAutomaticMode) {
			if(clearinghouseClin.ISA08=="113504607") {//TesiaLink
				//But the import will still happen
				return "";
			}
			if(clearinghouseClin.CommBridge==EclaimsCommBridge.None
				|| clearinghouseClin.CommBridge==EclaimsCommBridge.Renaissance
				|| clearinghouseClin.CommBridge==EclaimsCommBridge.RECS)
			{
				return "";
			}
			if(clearinghouseClin.CommBridge==EclaimsCommBridge.WebMD){
				if(!WebMD.Launch(clearinghouseClin,0,isAutomaticMode)){
					return Lan.g("FormClaimReports","Error retrieving.")+"\r\n"+WebMD.ErrorMessage;
				}
			}
			else if(clearinghouseClin.CommBridge==EclaimsCommBridge.BCBSGA){
				if(!BCBSGA.Retrieve(clearinghouseClin)){
					return Lan.g("FormClaimReports","Error retrieving.")+"\r\n"+BCBSGA.ErrorMessage;
				}
			}
			else if(clearinghouseClin.CommBridge==EclaimsCommBridge.ClaimConnect){
				if(!ClaimConnect.Retrieve(clearinghouseClin)) {
					if(isAutomaticMode && ClaimConnect.ErrorMessage.Contains(": 150\r\n")) {//Error message 150 "Service Not Contracted"
						return "";//Pretend that there is no error when loading FormClaimsSend for those customers who do not pay for ERA service.
					}
					return Lan.g("FormClaimReports","Error retrieving.")+"\r\n"+ClaimConnect.ErrorMessage;
				}
			}
			else if(clearinghouseClin.CommBridge==EclaimsCommBridge.AOS){
				try{
					//his path would never exist on Unix, so no need to handle back slashes.
					Process.Start(@"C:\Program files\AOS\AOSCommunicator\AOSCommunicator.exe");
				}
				catch{
					return "Could not locate the file.";
				}
			} 
			else if(clearinghouseClin.CommBridge==EclaimsCommBridge.MercuryDE){
				if(!MercuryDE.Launch(clearinghouseClin,0)) {
					return Lan.g("FormClaimReports","Error retrieving.")+"\r\n"+MercuryDE.ErrorMessage;
				}
			}
			else if(clearinghouseClin.CommBridge==EclaimsCommBridge.EmdeonMedical) {
				if(!EmdeonMedical.Retrieve(clearinghouseClin)) {
					return Lan.g("FormClaimReports","Error retrieving.")+"\r\n"+EmdeonMedical.ErrorMessage;
				}
			}
			else if(clearinghouseClin.CommBridge==EclaimsCommBridge.DentiCal) {
				if(!DentiCal.Launch(clearinghouseClin,0)) {
					return Lan.g("FormClaimReports","Error retrieving.")+"\r\n"+DentiCal.ErrorMessage;
				}
			}
			return "";
		}

		///<summary>Takes any files found in the reports folder for the clearinghouse, and imports them into the database.  Deletes the original files.
		///No longer any such thing as archive.</summary>
		private static void ImportReportFiles(Clearinghouse clearinghouseClin) { //uses clinic-level clearinghouse where necessary.
			if(!Directory.Exists(clearinghouseClin.ResponsePath)) {
				return;
			}
			if(clearinghouseClin.Eformat==ElectronicClaimFormat.Canadian) {
				//the report path is shared with many other important files.  Do not process anything.  Comm is synchronous only.
				return;
			}
			string[] files=Directory.GetFiles(clearinghouseClin.ResponsePath);
			string archiveDir=ODFileUtils.CombinePaths(clearinghouseClin.ResponsePath,"Archive");
			if(!Directory.Exists(archiveDir)){
				Directory.CreateDirectory(archiveDir);
			}
			for(int i=0;i<files.Length;i++) {
				Etranss.ProcessIncomingReport(
					File.GetCreationTime(files[i]),
					clearinghouseClin.HqClearinghouseNum,
					File.ReadAllText(files[i]));
				try{
					File.Copy(files[i],ODFileUtils.CombinePaths(archiveDir,Path.GetFileName(files[i])));
				}
				catch{}
				File.Delete(files[i]);
			}
		}

		///<summary></summary>
		public static string RetrieveAndImport(Clearinghouse clearinghouseClin,bool isAutomaticMode) {
			string errorMessage=RetrieveReports(clearinghouseClin,isAutomaticMode);
			if(errorMessage!="") {
				return errorMessage;
			}
			ImportReportFiles(clearinghouseClin);
			return "";
		}

		/*private void listMain_DoubleClick(object sender, System.EventArgs e) {
			if(listMain.SelectedIndices.Count==0){
				return;
			}
			string messageText=File.ReadAllText((string)listMain.SelectedItem);
			if(X12object.IsX12(messageText)){
				X12object xobj=new X12object(messageText);
				if(X277U.Is277U(xobj)){
					MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(X277U.MakeHumanReadable(xobj));
					msgbox.ShowDialog();
				}
				else if(X997.Is997(xobj)) {
					//MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(X997.MakeHumanReadable(xobj));
					//msgbox.ShowDialog();
				}
			}
			else{
				MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(messageText);
				msgbox.ShowDialog();
			}
			
			//if the file is an X12 file (277 for now), then display it differently
			if(Path.GetExtension((string)listMain.SelectedItem)==".txt"){
				//List<string> messageLines=new List<string>();
				//X12object xObj=new X12object(File.ReadAllText(fileName));
				string firstLine="";
				using(StreamReader sr=new StreamReader((string)listMain.SelectedItem)){
					firstLine=sr.ReadLine();
				}
				if(firstLine!=null && firstLine.Length==106 && firstLine.Substring(0,3)=="ISA"){
					//try{
						string humanText=X277U.MakeHumanReadable((string)listMain.SelectedItem);
						ArchiveFile((string)listMain.SelectedItem);
						//now the file will be gone
						//create a new file from humanText with same name as original file
						StreamWriter sw=File.CreateText((string)listMain.SelectedItem);
						sw.Write(humanText);
						sw.Close();
						//now, it will try to launch the new text file
					//}
					//catch(Exception ex){
					//	MessageBox.Show(ex.Message);
					//	return;
					//}
				}
			}
			try{
				Process.Start((string)listMain.SelectedItem);
			}
			catch{
				MsgBox.Show(this,"Could not open the item. You could try open it directly from the folder where it is located.");
			}
			//FillGrid();
		}*/

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

	

		

		

		

		


	}
}





















