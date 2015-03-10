using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using OpenDental;

namespace CentralManager {
	public partial class FormCentralManager:Form {
		public static byte[] EncryptionKey;
		private List<CentralConnection> ConnList;
		private bool IsStartingUp;

		public FormCentralManager() {
			InitializeComponent();
			UTF8Encoding enc=new UTF8Encoding();
			EncryptionKey=enc.GetBytes("mQlEGebnokhGFEFV");
		}

		private void FormCentralManager_Load(object sender,EventArgs e) {
			IsStartingUp=true;
			if(!GetConfigAndConnect()){
				return;
			}
			Cache.Refresh(InvalidType.Prefs);
			Version storedVersion=new Version(PrefC.GetString(PrefName.ProgramVersion));
			Version currentVersion=Assembly.GetAssembly(typeof(Db)).GetName().Version;
			if(storedVersion.CompareTo(currentVersion)!=0){
				MessageBox.Show("Program version: "+currentVersion.ToString()+"\r\n"
					+"Database version: "+storedVersion.ToString()+"\r\n"
					+"Versions must match.  Please manually connect to the database through the main program in order to update the version.");
				Application.Exit();
				return;
			}
			if(PrefC.GetString(PrefName.CentralManagerPassHash)!=""){
				FormCentralPasswordCheck formC=new FormCentralPasswordCheck();
				formC.ShowDialog();
				if(formC.DialogResult!=DialogResult.OK){
					Application.Exit();
					return;
				}
			}
			FillGrid();
			IsStartingUp=false;
		}

		///<summary>Gets the settings from the config file and attempts to connect.</summary>
		private bool GetConfigAndConnect(){
			string xmlPath=Path.Combine(Application.StartupPath,"CentralManagerConfig.xml");
			if(!File.Exists(xmlPath)){
				MessageBox.Show("Please create CentralManagerConfig.xml according to the manual before using this tool.");
				Application.Exit();
				return false;
			}
			XmlDocument document=new XmlDocument();
			string computerName="";
			string database="";
			string user="";
			string password="";
			try{
				document.Load(xmlPath);
				XPathNavigator Navigator=document.CreateNavigator();
				XPathNavigator nav;
				DataConnection.DBtype=DatabaseType.MySql;	
				//See if there's a DatabaseConnection
				nav=Navigator.SelectSingleNode("//DatabaseConnection");
				if(nav==null) {
					MessageBox.Show("DatabaseConnection element missing from CentralManagerConfig.xml.");
					Application.Exit();
					return false;
				}
				computerName=nav.SelectSingleNode("ComputerName").Value;
				database=nav.SelectSingleNode("Database").Value;
				user=nav.SelectSingleNode("User").Value;
				password=nav.SelectSingleNode("Password").Value;
			}
			catch(Exception ex) {
				//Common error: root element is missing
				MessageBox.Show(ex.Message);
				Application.Exit();
				return false;
			}
			DataConnection.DBtype=DatabaseType.MySql;
			OpenDentBusiness.DataConnection dcon=new OpenDentBusiness.DataConnection();
			//Try to connect to the database directly
			try {
				dcon.SetDb(computerName,database,user,password,"","",DataConnection.DBtype);
				RemotingClient.RemotingRole=RemotingRole.ClientDirect;
				Cache.RefreshCache(((int)InvalidType.AllLocal).ToString());
				return true;
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
				Application.Exit();
				return false;
			}
		}

		private void menuPassword_Click(object sender,EventArgs e) {
			FormCentralPasswordChange formC=new FormCentralPasswordChange();
			formC.ShowDialog();
		}

		private void menuConSetup_Click(object sender,EventArgs e) {
			FormCentralConnectionsSetup formS=new FormCentralConnectionsSetup();
			formS.ShowDialog();
			FillGrid();
		}

		private void menuUserEdit_Click(object sender,EventArgs e) {
			FormCentralSecurity FormCUS=new FormCentralSecurity();
			FormCUS.ShowDialog();
			GetConfigAndConnect();
		}

		private void textSearch_TextChanged(object sender,EventArgs e) {
			if(IsStartingUp) {
				return;
			}
			FillGrid();
		}

		private void menuProdInc_Click(object sender,EventArgs e) {
			List<CentralConnection> listSelectedConn=new List<CentralConnection>();
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				listSelectedConn.Add((CentralConnection)gridMain.Rows[gridMain.SelectedIndices[i]].Tag);//The tag of this grid is the CentralConnection object
			}
			if(listSelectedConn.Count==0) {
				MsgBox.Show(this,"Please select at least one connection to run this report against.");
				return;
			}
			FormCentralProdInc FormCPI=new FormCentralProdInc();
			FormCPI.ConnList=listSelectedConn;
			FormCPI.EncryptionKey=EncryptionKey;
			FormCPI.ShowDialog();
			GetConfigAndConnect();//Set the connection settings back to the central manager db.
		}

		private void FillGrid() {
			ConnList=CentralConnections.Refresh(textSearch.Text);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn("#",40);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Database",320);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Note",300);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<ConnList.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(ConnList[i].ItemOrder.ToString());
				if(ConnList[i].DatabaseName=="") {//uri
					row.Cells.Add(ConnList[i].ServiceURI);
				}
				else {
					row.Cells.Add(ConnList[i].ServerName+", "+ConnList[i].DatabaseName);
				}
				row.Cells.Add(ConnList[i].Note);
				row.Tag=ConnList[i];
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			string args="";
			if(ConnList[e.Row].DatabaseName!="") {
				//ServerName=localhost DatabaseName=opendental MySqlUser=root MySqlPassword=
				args+="ServerName=\""+ConnList[e.Row].ServerName+"\" "
					+"DatabaseName=\""+ConnList[e.Row].DatabaseName+"\" "
					+"MySqlUser=\""+ConnList[e.Row].MySqlUser+"\" ";
				if(ConnList[e.Row].MySqlPassword!="") {
					args+="MySqlPassword=\""+CentralConnections.Decrypt(ConnList[e.Row].MySqlPassword,EncryptionKey)+"\" ";
				}
			}
			else if(ConnList[e.Row].ServiceURI!="") {
				args+="WebServiceURI=\""+ConnList[e.Row].ServiceURI+"\" ";
				if(ConnList[e.Row].WebServiceIsEcw){
					args+="WebServiceIsEcw=True ";
				}
			}
			else {
				MessageBox.Show("Either a database or a Middle Tier URI must be specified in the connection.");
				return;
			}
			//od username and password always allowed
			if(ConnList[e.Row].OdUser!="") {
				args+="UserName=\""+ConnList[e.Row].OdUser+"\" ";
			}
			if(ConnList[e.Row].OdPassword!="") {
				args+="OdPassword=\""+CentralConnections.Decrypt(ConnList[e.Row].OdPassword,EncryptionKey)+"\" ";
			}
			#if DEBUG
				Process.Start("C:\\Development\\OPEN DENTAL SUBVERSION\\head\\OpenDental\\bin\\Debug\\OpenDental.exe",args);
			#else
				Process.Start("OpenDental.exe",args);
			#endif
		}
		
	}
}
