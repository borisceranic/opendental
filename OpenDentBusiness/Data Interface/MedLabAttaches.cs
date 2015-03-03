using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class MedLabAttaches{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all MedLabAttaches.</summary>
		private static List<MedLabAttach> listt;

		///<summary>A list of all MedLabAttaches.</summary>
		public static List<MedLabAttach> Listt{
			get {
				if(listt==null) {
					RefreshCache();
				}
				return listt;
			}
			set {
				listt=value;
			}
		}

		///<summary></summary>
		public static DataTable RefreshCache(){
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM medlabattach ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="MedLabAttach";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.MedLabAttachCrud.TableToList(table);
		}
		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<MedLabAttach> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<MedLabAttach>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM medlabattach WHERE PatNum = "+POut.Long(patNum);
			return Crud.MedLabAttachCrud.SelectMany(command);
		}

		///<summary>Gets one MedLabAttach from the db.</summary>
		public static MedLabAttach GetOne(long medLabAttachNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<MedLabAttach>(MethodBase.GetCurrentMethod(),medLabAttachNum);
			}
			return Crud.MedLabAttachCrud.SelectOne(medLabAttachNum);
		}

		///<summary></summary>
		public static long Insert(MedLabAttach medLabAttach){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				medLabAttach.MedLabAttachNum=Meth.GetLong(MethodBase.GetCurrentMethod(),medLabAttach);
				return medLabAttach.MedLabAttachNum;
			}
			return Crud.MedLabAttachCrud.Insert(medLabAttach);
		}

		///<summary></summary>
		public static void Update(MedLabAttach medLabAttach){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),medLabAttach);
				return;
			}
			Crud.MedLabAttachCrud.Update(medLabAttach);
		}

		///<summary></summary>
		public static void Delete(long medLabAttachNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),medLabAttachNum);
				return;
			}
			string command= "DELETE FROM medlabattach WHERE MedLabAttachNum = "+POut.Long(medLabAttachNum);
			Db.NonQ(command);
		}
		*/



	}
}