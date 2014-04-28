using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EvaluationCriterionDefs{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all EvaluationCriterionDefs.</summary>
		private static List<EvaluationCriterionDef> listt;

		///<summary>A list of all EvaluationCriterionDefs.</summary>
		public static List<EvaluationCriterionDef> Listt{
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
			string command="SELECT * FROM evaluationcriteriondef ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="EvaluationCriterionDef";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.EvaluationCriterionDefCrud.TableToList(table);
		}
		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<EvaluationCriterionDef> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<EvaluationCriterionDef>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM evaluationcriteriondef WHERE PatNum = "+POut.Long(patNum);
			return Crud.EvaluationCriterionDefCrud.SelectMany(command);
		}

		///<summary>Gets one EvaluationCriterionDef from the db.</summary>
		public static EvaluationCriterionDef GetOne(long evaluationCriterionDefNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<EvaluationCriterionDef>(MethodBase.GetCurrentMethod(),evaluationCriterionDefNum);
			}
			return Crud.EvaluationCriterionDefCrud.SelectOne(evaluationCriterionDefNum);
		}

		///<summary></summary>
		public static long Insert(EvaluationCriterionDef evaluationCriterionDef){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				evaluationCriterionDef.EvaluationCriterionDefNum=Meth.GetLong(MethodBase.GetCurrentMethod(),evaluationCriterionDef);
				return evaluationCriterionDef.EvaluationCriterionDefNum;
			}
			return Crud.EvaluationCriterionDefCrud.Insert(evaluationCriterionDef);
		}

		///<summary></summary>
		public static void Update(EvaluationCriterionDef evaluationCriterionDef){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),evaluationCriterionDef);
				return;
			}
			Crud.EvaluationCriterionDefCrud.Update(evaluationCriterionDef);
		}

		///<summary></summary>
		public static void Delete(long evaluationCriterionDefNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),evaluationCriterionDefNum);
				return;
			}
			string command= "DELETE FROM evaluationcriteriondef WHERE EvaluationCriterionDefNum = "+POut.Long(evaluationCriterionDefNum);
			Db.NonQ(command);
		}
		*/



	}
}