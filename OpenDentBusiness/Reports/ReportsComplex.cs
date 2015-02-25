using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness {
	public class ReportsComplex {

		///<summary>Gets a table of data using normal permissions.</summary>
		public static DataTable GetTable(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),command);
			}
			return Db.GetTable(command);
		}

	}
}
