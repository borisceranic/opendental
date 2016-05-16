using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class StmtPaySplitAttaches{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all StmtPaySplitAttaches.</summary>
		private static List<StmtPaySplitAttach> listt;

		///<summary>A list of all StmtPaySplitAttaches.</summary>
		public static List<StmtPaySplitAttach> Listt{
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
			string command="SELECT * FROM stmtpaysplitattach ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="StmtPaySplitAttach";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.StmtPaySplitAttachCrud.TableToList(table);
		}
		#endregion
		*/

		///<summary></summary>
		public static long Insert(StmtPaySplitAttach stmtPSA) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				stmtPSA.StmtPaySplitAttachNum=Meth.GetLong(MethodBase.GetCurrentMethod(),stmtPSA);
				return stmtPSA.StmtPaySplitAttachNum;
			}
			return Crud.StmtPaySplitAttachCrud.Insert(stmtPSA);
		}

		public static void AttachPaySplitsToStatement(long stmtNum,List<long> listPaySplitNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),stmtNum,listPaySplitNums);
				return;
			}
			listPaySplitNums.ForEach(x => Insert(new StmtPaySplitAttach() { StatementNum=stmtNum,PaySplitNum=x }));
		}

		///<summary></summary>
		public static void Delete(long stmtPSANum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),stmtPSANum);
				return;
			}
			Crud.StmtPaySplitAttachCrud.Delete(stmtPSANum);
		}

		///<summary></summary>
		public static void DetachFromStatement(long statementNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),statementNum);
				return;
			}
			string command="DELETE FROM stmtpaysplitattach WHERE StatementNum="+POut.Long(statementNum);
			Db.NonQ(command);
		}

		///<summary>Gets PaySplitNums for all paysplits attached to a statement.  Returns an empty list if statementNum is invalid.</summary>
		public static List<long> GetForStatement(long statementNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),statementNum);
			}
			string command="SELECT PaySplitNum FROM stmtpaysplitattach WHERE StatementNum="+POut.Long(statementNum);
			return Db.GetListLong(command);
		}


		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<StmtPaySplitAttach> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<StmtPaySplitAttach>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM stmtpaysplitattach WHERE PatNum = "+POut.Long(patNum);
			return Crud.StmtPaySplitAttachCrud.SelectMany(command);
		}

		///<summary>Gets one StmtPaySplitAttach from the db.</summary>
		public static StmtPaySplitAttach GetOne(long stmtPaySplitAttachNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<StmtPaySplitAttach>(MethodBase.GetCurrentMethod(),stmtPaySplitAttachNum);
			}
			return Crud.StmtPaySplitAttachCrud.SelectOne(stmtPaySplitAttachNum);
		}

		///<summary></summary>
		public static void Update(StmtPaySplitAttach stmtPaySplitAttach){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),stmtPaySplitAttach);
				return;
			}
			Crud.StmtPaySplitAttachCrud.Update(stmtPaySplitAttach);
		}

		

		
		*/



	}
}