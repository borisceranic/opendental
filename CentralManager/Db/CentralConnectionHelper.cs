using OpenDentBusiness;
using System.Text;
using System.Windows.Forms;

namespace CentralManager {
	public class CentralConnectionHelper {

		///<summary>Updates the current data connection settings of the central manager to the connection settings passed in.  Automatically refreshes the local cache to reflect the cache of the connection passed in.</summary>
		public static bool UpdateCentralConnection(CentralConnection centralConnection) {
			return UpdateCentralConnection(centralConnection,true);
		}

		///<summary>Updates the current data connection settings of the central manager to the connection settings passed in.  Setting refreshCache to true will cause the entire local cache to get updated with the cache from the connection passed in if the new connection settings are successful.</summary>
		public static bool UpdateCentralConnection(CentralConnection centralConnection,bool refreshCache) {
			UTF8Encoding enc=new UTF8Encoding();
			byte[] EncryptionKey=enc.GetBytes("mQlEGebnokhGFEFV");//Gotten from FormCentralManager constructor. Only place that does anything like this.
			string webServiceURI="";
			string computerName="";
			string database="";
			string user="";
			string password="";
			string odPassword="";
			if(centralConnection.DatabaseName!="") {
				computerName=centralConnection.ServerName;
				database=centralConnection.DatabaseName;
				user=centralConnection.MySqlUser;
				if(centralConnection.MySqlPassword!="") {
					password=CentralConnections.Decrypt(centralConnection.MySqlPassword,EncryptionKey);
				}
				RemotingClient.ServerURI="";
			}
			else if(centralConnection.ServiceURI!="") {
				webServiceURI=centralConnection.ServiceURI;
				RemotingClient.ServerURI=webServiceURI;
				try {
					odPassword=CentralConnections.Decrypt(centralConnection.OdPassword,EncryptionKey);
					Security.CurUser=Security.LogInWeb(centralConnection.OdUser,odPassword,"",Application.ProductVersion,centralConnection.WebServiceIsEcw);
					Security.PasswordTyped=odPassword;
				}
				catch {
					return false;
				}
			}
			else {
				MessageBox.Show("Either a database or a Middle Tier URI must be specified in the connection.");
				return false;
			}
			try {
				if(RemotingClient.ServerURI!="") {
					RemotingClient.RemotingRole=RemotingRole.ClientWeb;
				}
				else {
					DataConnection.DBtype=DatabaseType.MySql;
					OpenDentBusiness.DataConnection dcon=new OpenDentBusiness.DataConnection();
					dcon.SetDb(computerName,database,user,password,"","",DataConnection.DBtype);
					RemotingClient.RemotingRole=RemotingRole.ClientDirect;
				}
				if(refreshCache) {
					Cache.RefreshCache(((int)InvalidType.AllLocal).ToString());
				}
			}
			catch {
				return false;
			}
			return true;
		}
	}
}
