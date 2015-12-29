using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
  ///<summary></summary>
	public class Screens {

		///<summary>Gets one Screen from the db.</summary>
		public static Screen GetOne(long screenNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Screen>(MethodBase.GetCurrentMethod(),screenNum);
			}
			return Crud.ScreenCrud.SelectOne(screenNum);
		}

		///<summary>Gets all screens associated to the screen group passed in.</summary>
		public static List<Screen> GetScreensForGroup(long screenGroupNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Screen>>(MethodBase.GetCurrentMethod(),screenGroupNum);
			}
			string command="SELECT * FROM screen "
				+"WHERE ScreenGroupNum = '"+POut.Long(screenGroupNum)+"' "
				+"ORDER BY ScreenGroupOrder";
			return Crud.ScreenCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(OpenDentBusiness.Screen Cur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Cur.ScreenNum=Meth.GetLong(MethodBase.GetCurrentMethod(),Cur);
				return Cur.ScreenNum;
			}
			return Crud.ScreenCrud.Insert(Cur);
		}

		///<summary></summary>
		public static void Update(OpenDentBusiness.Screen Cur){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),Cur);
				return;
			}
			Crud.ScreenCrud.Update(Cur);
		}

		///<summary></summary>
		public static void Delete(OpenDentBusiness.Screen Cur){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),Cur);
				return;
			}
			string command = "DELETE from screen WHERE ScreenNum = '"+POut.Long(Cur.ScreenNum)+"'";
			Db.NonQ(command);
		}


	}

	

}













