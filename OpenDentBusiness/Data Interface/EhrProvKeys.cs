using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EhrProvKeys{
		///<summary></summary>
		public static List<EhrProvKey> RefreshForFam(long guarantor){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<EhrProvKey>>(MethodBase.GetCurrentMethod(),guarantor);
			}
			string command="SELECT ehrprovkey.* FROM ehrprovkey,patient "
				+"WHERE ehrprovkey.PatNum=patient.PatNum "
				+"AND patient.Guarantor="+POut.Long(guarantor)+" "
				+"GROUP BY ehrprovkey.EhrProvKeyNum "
				+"ORDER BY ehrprovkey.LName,ehrprovkey.FName";
			return Crud.EhrProvKeyCrud.SelectMany(command);
		}

		///<summary>Get a list of all EhrProvKeys. Ordered by LName and then YearValue.</summary>
		public static List<EhrProvKey> GetAllKeys() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<EhrProvKey>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT ehrprovkey.* FROM ehrprovkey "
			+"ORDER BY LName,YearValue";
			return Crud.EhrProvKeyCrud.SelectMany(command);
		}

		///<summary>Get a list of all EhrProvKeys for a provider matching the given first and last name.  Ordered by year value.</summary>
		public static List<EhrProvKey> GetKeysByFLName(string lName, string fName) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<EhrProvKey>>(MethodBase.GetCurrentMethod(),lName,fName);
			}
			string command="SELECT ehrprovkey.* FROM ehrprovkey"
			+" WHERE ehrprovkey.LName='"+POut.String(lName)
			+"' AND ehrprovkey.FName='"+POut.String(fName)
			+"' ORDER BY ehrprovkey.YearValue DESC";
			return Crud.EhrProvKeyCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(EhrProvKey ehrProvKey){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				ehrProvKey.EhrProvKeyNum=Meth.GetLong(MethodBase.GetCurrentMethod(),ehrProvKey);
				return ehrProvKey.EhrProvKeyNum;
			}
			return Crud.EhrProvKeyCrud.Insert(ehrProvKey);
		}

		///<summary></summary>
		public static void Update(EhrProvKey ehrProvKey){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),ehrProvKey);
				return;
			}
			Crud.EhrProvKeyCrud.Update(ehrProvKey);
		}

		///<summary></summary>
		public static void Delete(long ehrProvKeyNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),ehrProvKeyNum);
				return;
			}
			string command= "DELETE FROM ehrprovkey WHERE EhrProvKeyNum = "+POut.Long(ehrProvKeyNum);
			Db.NonQ(command);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		

		///<summary>Gets one EhrProvKey from the db.</summary>
		public static EhrProvKey GetOne(long ehrProvKeyNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<EhrProvKey>(MethodBase.GetCurrentMethod(),ehrProvKeyNum);
			}
			return Crud.EhrProvKeyCrud.SelectOne(ehrProvKeyNum);
		}

		
		*/



	}
}