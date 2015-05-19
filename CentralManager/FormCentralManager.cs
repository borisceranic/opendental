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
using CodeBase;

namespace CentralManager {
	public partial class FormCentralManager:Form {
		public static byte[] EncryptionKey;
		private List<CentralConnection> _listConns;
		private List<ConnectionGroup> _listConnectionGroups;

		public FormCentralManager() {
			InitializeComponent();
			UTF8Encoding enc=new UTF8Encoding();
			EncryptionKey=enc.GetBytes("mQlEGebnokhGFEFV");
		}

		private void FormCentralManager_Load(object sender,EventArgs e) {
			if(!GetConfigAndConnect()){
				return;
			}
			Cache.Refresh(InvalidType.Prefs);
			Version storedVersion=new Version(PrefC.GetString(PrefName.ProgramVersion));
			Version currentVersion=Assembly.GetAssembly(typeof(Db)).GetName().Version;
			if(storedVersion.CompareTo(currentVersion)!=0){
				MessageBox.Show(Lan.g(this,"Program version")+": "+currentVersion.ToString()+"\r\n"
					+Lan.g(this,"Database version")+": "+storedVersion.ToString()+"\r\n"
					+Lan.g(this,"Versions must match.  Please manually connect to the database through the main program in order to update the version."));
				Application.Exit();
				return;
			}
			FormCentralLogOn FormCLO=new FormCentralLogOn();
			if(FormCLO.ShowDialog()!=DialogResult.OK) {
				Application.Exit();
				return;
			}
			this.Text+=" - "+Security.CurUser.UserName;
			_listConnectionGroups=ConnectionGroups.GetListt();
			comboConnectionGroups.Items.Clear();
			comboConnectionGroups.Items.Add("All");
			comboConnectionGroups.SelectedIndex=0;
			for(int i=0;i<_listConnectionGroups.Count;i++) {
				comboConnectionGroups.Items.Add(_listConnectionGroups[i].Description);
				if(_listConnectionGroups[i].ConnectionGroupNum==PrefC.GetLong(PrefName.ConnGroupCEMT)) {
					comboConnectionGroups.SelectedIndex=i+1;//0 is "All"
				}
			}
			FillGrid();
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

		private void FillGrid() {
			_listConns=CentralConnections.Refresh(textSearch.Text);
			if(comboConnectionGroups.SelectedIndex>0) {
				_listConns=ConnectionGroups.FilterConnsByGroup(_listConns,_listConnectionGroups[comboConnectionGroups.SelectedIndex-1]);
			}
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
			for(int i=0;i<_listConns.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listConns[i].ItemOrder.ToString());
				if(_listConns[i].DatabaseName=="") {//uri
					row.Cells.Add(_listConns[i].ServiceURI);
				}
				else {
					row.Cells.Add(_listConns[i].ServerName+", "+_listConns[i].DatabaseName);
				}
				row.Cells.Add(_listConns[i].Note);
				row.Tag=_listConns[i];
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			string args="";
			if(_listConns[e.Row].DatabaseName!="") {
				//ServerName=localhost DatabaseName=opendental MySqlUser=root MySqlPassword=
				args+="ServerName=\""+_listConns[e.Row].ServerName+"\" "
					+"DatabaseName=\""+_listConns[e.Row].DatabaseName+"\" "
					+"MySqlUser=\""+_listConns[e.Row].MySqlUser+"\" ";
				if(_listConns[e.Row].MySqlPassword!="") {
					args+="MySqlPassword=\""+CentralConnections.Decrypt(_listConns[e.Row].MySqlPassword,EncryptionKey)+"\" ";
				}
			}
			else if(_listConns[e.Row].ServiceURI!="") {
				args+="WebServiceURI=\""+_listConns[e.Row].ServiceURI+"\" ";
				if(_listConns[e.Row].WebServiceIsEcw){
					args+="WebServiceIsEcw=True ";
				}
			}
			else {
				MessageBox.Show("Either a database or a Middle Tier URI must be specified in the connection.");
				return;
			}
			//od username and password always allowed
			args+="UserName=\""+Security.CurUser.UserName+"\" ";
			args+="OdPassword=\""+Security.PasswordTyped+"\" ";
			#if DEBUG
				Process.Start("C:\\Development\\OPEN DENTAL SUBVERSION\\head\\OpenDental\\bin\\Debug\\OpenDental.exe",args);
			#else
				Process.Start("OpenDental.exe",args);
			#endif
		}

		private void textSearch_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}
		
		private void comboConnectionGroups_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboConnectionGroups.SelectedIndex==0) {
				Prefs.UpdateLong(PrefName.ConnGroupCEMT,0);
			}
			else {
				Prefs.UpdateLong(PrefName.ConnGroupCEMT,_listConnectionGroups[comboConnectionGroups.SelectedIndex-1].ConnectionGroupNum);
			}
			FillGrid();
		}

		#region Menu Setup

		private void menuConnSetup_Click(object sender,EventArgs e) {
			FormCentralConnections FormCC=new FormCentralConnections();
			FormCC.LabelText.Text=Lans.g("FormCentralConnections","Double click an existing connection to edit or click the 'Add' button to add a new connection.");
			FormCC.Text=Lans.g("FormCentralConnections","Connection Setup");
			FormCC.ShowDialog();
			FillGrid();
		}

		private void menuGroups_Click(object sender,EventArgs e) {
			ConnectionGroup connGroupCur=null;
			if(comboConnectionGroups.SelectedIndex>0) {
				connGroupCur=_listConnectionGroups[comboConnectionGroups.SelectedIndex-1];
			}
			FormCentralConnectionGroups FormCCG=new FormCentralConnectionGroups();
			FormCCG.ShowDialog();
			ConnectionGroups.RefreshCache();
			_listConnectionGroups=ConnectionGroups.GetListt();
			comboConnectionGroups.Items.Clear();
			comboConnectionGroups.Items.Add("All");
			comboConnectionGroups.SelectedIndex=0;//default to "All"
			for(int i=0;i<_listConnectionGroups.Count;i++) {
				comboConnectionGroups.Items.Add(_listConnectionGroups[i].Description);
				if(connGroupCur!=null && connGroupCur.ConnectionGroupNum==_listConnectionGroups[i].ConnectionGroupNum) {
					comboConnectionGroups.SelectedIndex=i+1;//Reselect the connection group that the user had before.
				}
			}
			if(comboConnectionGroups.SelectedIndex==0) {
				Prefs.UpdateLong(PrefName.ConnGroupCEMT,0);
			}
			else {
				Prefs.UpdateLong(PrefName.ConnGroupCEMT,_listConnectionGroups[comboConnectionGroups.SelectedIndex-1].ConnectionGroupNum);
			}
			FillGrid();
		}

		private void menuUserEdit_Click(object sender,EventArgs e) {
			FormCentralSecurity FormCUS=new FormCentralSecurity();
			FormCUS.ShowDialog();
			GetConfigAndConnect();
		}

		#endregion

		#region Menu Reports

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

		#endregion

		private void menuItemLogoff_Click(object sender,EventArgs e) {
			FormCentralLogOn FormCLO=new FormCentralLogOn();
			if(FormCLO.ShowDialog()!=DialogResult.OK) {
				Application.Exit();
				return;
			}
			this.Text="Central Manager - "+Security.CurUser.UserName;
		}
		
		private void menuItemPassword_Click(object sender,EventArgs e) {
			FormCentralUserPasswordEdit FormCPE=new FormCentralUserPasswordEdit(false,Security.CurUser.UserName);
			if(FormCPE.ShowDialog()==DialogResult.Cancel){
				return;
			}
			Security.CurUser.Password=FormCPE.HashedResult;
			Userods.Update(Security.CurUser);
		}

		private void butSearch_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select at least one connection to search first.");
				return;
			}
			List<CentralConnection> listConns=new List<CentralConnection>();
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				listConns.Add((CentralConnection)gridMain.Rows[gridMain.SelectedIndices[i]].Tag);
			}
			FormCentralPatientSearch FormCPS=new FormCentralPatientSearch();
			FormCPS.ListConns=listConns;
			FormCPS.ShowDialog();
		}

		private void FormCentralManager_FormClosing(object sender,FormClosingEventArgs e) {
			ODThread.QuitSyncAllOdThreads();
		}

		
	}
}
