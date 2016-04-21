using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using OpenDentBusiness;

namespace xFHIR {
	public class DbConn {
		public static void ConnectIfNecessary() {
			if(string.IsNullOrEmpty(DataConnection.GetServerName()) && string.IsNullOrEmpty(DataConnection.GetConnectionString())) {
				RemotingClient.RemotingRole=RemotingRole.ServerWeb;
				Userods.LoadDatabaseInfoFromFile(HostingEnvironment.MapPath(@"~\OpenDentalFHIRConfig.xml"));
			}
		}

		public static DataTable GetDataTable(string command) {
			return DataCore.GetTable(command);
		}
	}
}