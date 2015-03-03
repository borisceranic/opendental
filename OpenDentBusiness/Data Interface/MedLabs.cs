using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class MedLabs{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all MedLabs.</summary>
		private static List<MedLab> listt;

		///<summary>A list of all MedLabs.</summary>
		public static List<MedLab> Listt{
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
			string command="SELECT * FROM medlab ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="MedLab";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.MedLabCrud.TableToList(table);
		}
		#endregion
		*/


		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<MedLab> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<MedLab>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM medlab WHERE PatNum = "+POut.Long(patNum);
			return Crud.MedLabCrud.SelectMany(command);
		}

		///<summary>Gets one MedLab from the db.</summary>
		public static MedLab GetOne(long medLabNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<MedLab>(MethodBase.GetCurrentMethod(),medLabNum);
			}
			return Crud.MedLabCrud.SelectOne(medLabNum);
		}

		///<summary></summary>
		public static long Insert(MedLab medLab){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				medLab.MedLabNum=Meth.GetLong(MethodBase.GetCurrentMethod(),medLab);
				return medLab.MedLabNum;
			}
			return Crud.MedLabCrud.Insert(medLab);
		}

		///<summary></summary>
		public static void Update(MedLab medLab){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),medLab);
				return;
			}
			Crud.MedLabCrud.Update(medLab);
		}

		///<summary></summary>
		public static void Delete(long medLabNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),medLabNum);
				return;
			}
			string command= "DELETE FROM medlab WHERE MedLabNum = "+POut.Long(medLabNum);
			Db.NonQ(command);
		}
		*/



	}
}