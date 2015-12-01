using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;
using OpenDental;

namespace CentralManager {
	public partial class FormCentralPatientSearch:Form {
		///<summary>List of connections used to connect to databases and fill patient data dictionary.</summary>
		public List<CentralConnection> ListConns;
		/// <summary>Dataset containing tables of patients for each connection.</summary>
		private DataSet _dataConnPats;
		private object _lockObj=new object();
		private int _complConnAmt;
		private string _invalidConnsLog;
		private bool _hasWarningShown=false;

		public FormCentralPatientSearch() {
			ListConns=new List<CentralConnection>();
			InitializeComponent();
		}

		private void FormCentralPatientSearch_Load(object sender,System.EventArgs e) {
			_complConnAmt=0;
			_dataConnPats=new DataSet();
			_invalidConnsLog="";
			StartThreadsForConns();
		}

		///<summary>Loops through all connections passed in and spawns a thread for each to go fetch patient data from each db using the given filters.</summary>
		private void StartThreadsForConns() {
			_dataConnPats.Tables.Clear();
			bool hasConnsSkipped=false;
			for(int i=0;i<ListConns.Count;i++) {
				//Filter the threads by their connection name
				string connName="";
				if(ListConns[i].DatabaseName=="") {//uri
					connName=ListConns[i].ServiceURI;
				}
				else {
					connName=ListConns[i].ServerName+", "+ListConns[i].DatabaseName;
				}
				if(!connName.Contains(textConn.Text)) {
					//Do NOT spawn a thread to go fetch data for this connection because the user has filtered it out.
					//Increment the completed thread count and continue.
					hasConnsSkipped=true;
					lock(_lockObj) {
						_complConnAmt++;
					}
					continue;
				}
				//At this point we know the connection has not been filtered out, so fire up a thread to go get the patient data table for the search.
				ODThread odThread=new ODThread(GetPtDataTableForConn,new object[]{ListConns[i]});
				odThread.GroupName="FetchPats";
				odThread.Start();
			}
			if(hasConnsSkipped) {
				//There is a chance that some threads finished (by failing, etc) before the end of the loop where we spawned the threads
				//so we want to guarantee that the failure message shows if any connection was skipped.
				//This is required because FillGrid contains code that only shows a warning message when all connections have finished.
				FillGrid();
			}
		}

		private void GetPtDataTableForConn(ODThread odThread) {
			CentralConnection connection=(CentralConnection)odThread.Parameters[0];
			//Filter the threads by their connection name
			string connName="";
			if(connection.DatabaseName=="") {//uri
				connName=connection.ServiceURI;
			}
			else {
				connName=connection.ServerName+", "+connection.DatabaseName;
			}
			if(!CentralConnectionHelper.UpdateCentralConnection(connection,false)) {
				lock(_lockObj) {
					_invalidConnsLog+="\r\n"+connName;
					_complConnAmt++;
				}
				connection.ConnectionStatus="OFFLINE";
				BeginInvoke(new FillGridDelegate(FillGrid));
				return;
			}
			DataTable table=new DataTable();
			try {
				table=Patients.GetPtDataTable(checkLimit.Checked,textLName.Text,textFName.Text,textPhone.Text,
						textAddress.Text,checkHideInactive.Checked,textCity.Text,textState.Text,
						textSSN.Text,textPatNum.Text,textChartNumber.Text,0,
						checkGuarantors.Checked,!checkHideArchived.Checked,//checkHideArchived is opposite label for what this function expects, but hideArchived makes more sense
						DateTime.MinValue,0,textSubscriberID.Text,textEmail.Text,textCountry.Text,"","");
			}
			catch(ThreadAbortException tae) {
				throw tae;//ODThread needs to clean up after an abort exception is thrown.
			}
			catch(Exception) {
				//This can happen if the connection to the server was severed somehow during the execution of the query.
				lock(_lockObj) {
					_invalidConnsLog+="\r\n"+connName+"  -GetPtDataTable";
					_complConnAmt++;
				}
				BeginInvoke(new FillGridDelegate(FillGrid));//Pops up a message box if this was the last thread to finish.
				return;
			}
			table.TableName=connName;
			odThread.Tag=table;
			lock(_lockObj) {
				_complConnAmt++;
				_dataConnPats.Tables.Add((DataTable)odThread.Tag);
			}
			BeginInvoke(new FillGridDelegate(FillGrid));
		}
				
		public delegate void FillGridDelegate();

		private void FillGrid() {
			Cursor=Cursors.WaitCursor;
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lans.g(this,"Conn"),167);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"PatNum"),64);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"LName"),94);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"FName"),94);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"SSN"),94);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"PatStatus"),60);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Age"),40);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"City"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"State"),40);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Address"),167);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Phone"),94);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Email"),190);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"ChartNum"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g(this,"Country"),60);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_dataConnPats.Tables.Count;i++) {
				for(int j=0;j<_dataConnPats.Tables[i].Rows.Count;j++) {
					row=new ODGridRow();
					row.Cells.Add(_dataConnPats.Tables[i].TableName);
					row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["PatNum"].ToString());
					row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["LName"].ToString());
					row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["FName"].ToString());
					row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["SSN"].ToString());
					row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["PatStatus"].ToString());
					row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["age"].ToString());
					row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["City"].ToString());
					row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["State"].ToString());
					row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["Address"].ToString());
					row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["WkPhone"].ToString());//Put in WkPhone by default
					if(_dataConnPats.Tables[i].Rows[j]["HmPhone"].ToString()!="") {
						row.Cells[row.Cells.Count-1].Text=_dataConnPats.Tables[i].Rows[j]["HmPhone"].ToString();//Overwrite if HmPhone present
					}
					row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["Email"].ToString());
					row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["ChartNumber"].ToString());
					row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["Country"].ToString());
					row.Tag=ListConns.Find(x => (x.ServerName+", "+x.DatabaseName)==_dataConnPats.Tables[i].TableName);
					gridMain.Rows.Add(row);
				}
			}
			gridMain.EndUpdate();
			Cursor=Cursors.Default;
			if(_complConnAmt==ListConns.Count) {
				ODThread.QuitSyncThreadsByGroupName(1,"FetchPats");//Clean up finished threads.
				butRefresh.Text=Lans.g(this,"Refresh");
				labelFetch.Visible=false;
				if(!_hasWarningShown && _invalidConnsLog!="") {
					_hasWarningShown=true;//Keeps the message box from showing up for subsequent threads.
					MessageBox.Show(this,Lan.g(this,"Could not connect to the following servers")+":"+_invalidConnsLog);
				}
			}
			else {
				butRefresh.Text=Lans.g(this,"Stop Refresh");
				labelFetch.Visible=true;
			}
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			CentralConnection conn=(CentralConnection)gridMain.Rows[e.Row].Tag;
			string args=CentralConnectionHelper.GetArgsFromConnection(conn);
			args+="PatNum="+gridMain.Rows[e.Row].Cells[1].Text;//PatNum
			#if DEBUG
				Process.Start("C:\\Development\\OPEN DENTAL SUBVERSION\\head\\OpenDental\\bin\\Debug\\OpenDental.exe",args);
			#else
				Process.Start("OpenDental.exe",args);
			#endif
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			ODThread.JoinThreadsByGroupName(1,"FetchPats");//Stop fetching immediately
			_hasWarningShown=false;
			lock(_lockObj) {
				_invalidConnsLog="";
			}
			if(butRefresh.Text==Lans.g(this,"Refresh")) {
				_dataConnPats.Clear();
				butRefresh.Text=Lans.g(this,"Stop Refresh");
				labelFetch.Visible=true;
				_complConnAmt=0;
				StartThreadsForConns();
			}
			else {
				butRefresh.Text=Lans.g(this,"Refresh");
				labelFetch.Visible=false;
				_complConnAmt=ListConns.Count;
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormCentralPatientSearch_FormClosing(object sender,FormClosingEventArgs e) {
			//User could have closed the window before all threads finished.  Make sure to abort all threads instantly.
			ODThread.QuitSyncThreadsByGroupName(1,"FetchPats");
		}

	}
}
