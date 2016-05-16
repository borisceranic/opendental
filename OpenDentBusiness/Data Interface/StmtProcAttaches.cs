using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class StmtProcAttaches{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all StmtProcAttaches.</summary>
		private static List<StmtProcAttach> listt;

		///<summary>A list of all StmtProcAttaches.</summary>
		public static List<StmtProcAttach> Listt{
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
			string command="SELECT * FROM stmtprocattach ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="StmtProcAttach";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.StmtProcAttachCrud.TableToList(table);
		}
		#endregion
		*/

		///<summary></summary>
		public static long Insert(StmtProcAttach stmtProcAttach) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				stmtProcAttach.StmtProcAttachNum=Meth.GetLong(MethodBase.GetCurrentMethod(),stmtProcAttach);
				return stmtProcAttach.StmtProcAttachNum;
			}
			return Crud.StmtProcAttachCrud.Insert(stmtProcAttach);
		}

		public static void AttachProcsToStatement(long stmtNum,List<long> listProcNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),stmtNum,listProcNums);
				return;
			}
			listProcNums.ForEach(x => Insert(new StmtProcAttach() { StatementNum=stmtNum,ProcNum=x }));
		}

		///<summary></summary>
		public static void Delete(long stmtProcAttachNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),stmtProcAttachNum);
				return;
			}
			Crud.StmtProcAttachCrud.Delete(stmtProcAttachNum);
		}

		///<summary></summary>
		public static void DetachFromStatement(long statementNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),statementNum);
				return;
			}
			string command="DELETE FROM stmtprocattach WHERE StatementNum="+POut.Long(statementNum);
			Db.NonQ(command);
		}

		///<summary>Gets ProcNums for all procedures attached to a statement.  Returns an empty list if statementNum is invalid.</summary>
		public static List<long> GetForStatement(long statementNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),statementNum);
			}
			string command="SELECT ProcNum FROM stmtprocattach WHERE StatementNum="+POut.Long(statementNum);
			return Db.GetListLong(command);
		}



		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<StmtProcAttach> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<StmtProcAttach>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM stmtprocattach WHERE PatNum = "+POut.Long(patNum);
			return Crud.StmtProcAttachCrud.SelectMany(command);
		}

		///<summary></summary>
		public static void Update(StmtProcAttach stmtProcAttach){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),stmtProcAttach);
				return;
			}
			Crud.StmtProcAttachCrud.Update(stmtProcAttach);
		}

		

		
		*/



	}
}