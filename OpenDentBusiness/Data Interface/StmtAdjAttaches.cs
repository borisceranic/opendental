using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class StmtAdjAttaches{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all StmtAdjAttaches.</summary>
		private static List<StmtAdjAttach> listt;

		///<summary>A list of all StmtAdjAttaches.</summary>
		public static List<StmtAdjAttach> Listt{
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
			string command="SELECT * FROM stmtadjattach ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="StmtAdjAttach";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.StmtAdjAttachCrud.TableToList(table);
		}
		#endregion
		*/

		///<summary></summary>
		public static long Insert(StmtAdjAttach stmtAdjAttach) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				stmtAdjAttach.StmtAdjAttachNum=Meth.GetLong(MethodBase.GetCurrentMethod(),stmtAdjAttach);
				return stmtAdjAttach.StmtAdjAttachNum;
			}
			return Crud.StmtAdjAttachCrud.Insert(stmtAdjAttach);
		}

		public static void AttachAdjsToStatement(long stmtNum,List<long> listAdjNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),stmtNum,listAdjNums);
				return;
			}
			listAdjNums.ForEach(x => Insert(new StmtAdjAttach() { StatementNum=stmtNum,AdjNum=x }));
		}

		///<summary></summary>
		public static void Delete(long stmtAdjAttachNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),stmtAdjAttachNum);
				return;
			}
			Crud.StmtAdjAttachCrud.Delete(stmtAdjAttachNum);
		}

		///<summary></summary>
		public static void DetachFromStatement(long statementNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),statementNum);
				return;
			}
			string command="DELETE FROM stmtadjattach WHERE StatementNum="+POut.Long(statementNum);
			Db.NonQ(command);
		}

		///<summary>Gets AdjNums for all adjustments attached to a statement.  Returns an empty list if statementNum is invalid.</summary>
		public static List<long> GetForStatement(long statementNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),statementNum);
			}
			string command="SELECT AdjNum FROM stmtadjattach WHERE StatementNum="+POut.Long(statementNum);
			return Db.GetListLong(command);
		}


		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<StmtAdjAttach> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<StmtAdjAttach>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM stmtadjattach WHERE PatNum = "+POut.Long(patNum);
			return Crud.StmtAdjAttachCrud.SelectMany(command);
		}

		///<summary>Gets one StmtAdjAttach from the db.</summary>
		public static StmtAdjAttach GetOne(long stmtAdjAttachNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<StmtAdjAttach>(MethodBase.GetCurrentMethod(),stmtAdjAttachNum);
			}
			return Crud.StmtAdjAttachCrud.SelectOne(stmtAdjAttachNum);
		}

		///<summary></summary>
		public static long Insert(StmtAdjAttach stmtAdjAttach){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				stmtAdjAttach.StmtAdjAttachNum=Meth.GetLong(MethodBase.GetCurrentMethod(),stmtAdjAttach);
				return stmtAdjAttach.StmtAdjAttachNum;
			}
			return Crud.StmtAdjAttachCrud.Insert(stmtAdjAttach);
		}

		///<summary></summary>
		public static void Update(StmtAdjAttach stmtAdjAttach){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),stmtAdjAttach);
				return;
			}
			Crud.StmtAdjAttachCrud.Update(stmtAdjAttach);
		}

		///<summary></summary>
		public static void Delete(long stmtAdjAttachNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),stmtAdjAttachNum);
				return;
			}
			Crud.StmtAdjAttachCrud.Delete(stmtAdjAttachNum);
		}

		

		
		*/



	}
}