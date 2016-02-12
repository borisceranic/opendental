using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class UpdateHistories{
		///<summary>Gets one UpdateHistory from the db.</summary>
		public static UpdateHistory GetOne(long updateNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<UpdateHistory>(MethodBase.GetCurrentMethod(),updateNum);
			}
			return Crud.UpdateHistoryCrud.SelectOne(updateNum);
		}

		///<summary></summary>
		public static long Insert(UpdateHistory updateHistory){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				updateHistory.UpdateHistoryNum=Meth.GetLong(MethodBase.GetCurrentMethod(),updateHistory);
				return updateHistory.UpdateHistoryNum;
			}
			return Crud.UpdateHistoryCrud.Insert(updateHistory);
		}

		///<summary></summary>
		public static void Update(UpdateHistory updateHistory){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),updateHistory);
				return;
			}
			Crud.UpdateHistoryCrud.Update(updateHistory);
		}

		///<summary></summary>
		public static void Delete(long updateNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),updateNum);
				return;
			}
			Crud.UpdateHistoryCrud.Delete(updateNum);
		}

		///<summary>All updatehistory entries ordered by DateTimeUpdated.</summary>
		public static List<UpdateHistory> GetAll() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<UpdateHistory>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM updatehistory ORDER BY DateTimeUpdated";
			return Crud.UpdateHistoryCrud.SelectMany(command);
		}

		///<summary>Returns the latest version information.</summary>
		public static UpdateHistory GetForVersion(string version) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<UpdateHistory>(MethodBase.GetCurrentMethod(),version);
			}
			string command="SELECT * FROM updatehistory WHERE ProgramVersion='"+POut.String(version.ToString())+"'";
			return Crud.UpdateHistoryCrud.SelectOne(command);
		}

		///<summary>Returns the earliest datetime that a version was reached. If that version has not been reached, returns the MinDate.</summary>
		public static DateTime GetDateForVersion(Version version) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<DateTime>(MethodBase.GetCurrentMethod(),version);
			}
			List<UpdateHistory> listUpdates=UpdateHistories.GetAll();
			foreach(UpdateHistory update in listUpdates) {
				if(new Version(update.ProgramVersion) >= version) {
					return update.DateTimeUpdated;
				}
			}
			//The earliest version was later than the version passed in.
			return new DateTime(1,1,1);
		}



	}
}