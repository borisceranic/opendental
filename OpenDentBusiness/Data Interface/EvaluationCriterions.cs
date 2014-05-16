using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EvaluationCriterions{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all EvaluationCriterions.</summary>
		private static List<EvaluationCriterion> listt;

		///<summary>A list of all EvaluationCriterions.</summary>
		public static List<EvaluationCriterion> Listt{
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
			string command="SELECT * FROM evaluationcriterion ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="EvaluationCriterion";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.EvaluationCriterionCrud.TableToList(table);
		}
		#endregion
		*/

		///<summary>Get all Criterion attached to and Evaluation.</summary>
		public static List<EvaluationCriterion> Refresh(long evaluationNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<EvaluationCriterion>>(MethodBase.GetCurrentMethod(),evaluationNum);
			}
			string command="SELECT * FROM evaluationcriterion WHERE EvaluationNum = "+POut.Long(evaluationNum);
			return Crud.EvaluationCriterionCrud.SelectMany(command);
		}

		///<summary>Gets one EvaluationCriterion from the db.</summary>
		public static EvaluationCriterion GetOne(long evaluationCriterionNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<EvaluationCriterion>(MethodBase.GetCurrentMethod(),evaluationCriterionNum);
			}
			return Crud.EvaluationCriterionCrud.SelectOne(evaluationCriterionNum);
		}

		///<summary></summary>
		public static long Insert(EvaluationCriterion evaluationCriterion){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				evaluationCriterion.EvaluationCriterionNum=Meth.GetLong(MethodBase.GetCurrentMethod(),evaluationCriterion);
				return evaluationCriterion.EvaluationCriterionNum;
			}
			return Crud.EvaluationCriterionCrud.Insert(evaluationCriterion);
		}

		///<summary></summary>
		public static void Update(EvaluationCriterion evaluationCriterion){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),evaluationCriterion);
				return;
			}
			Crud.EvaluationCriterionCrud.Update(evaluationCriterion);
		}

		///<summary></summary>
		public static void Delete(long evaluationCriterionNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),evaluationCriterionNum);
				return;
			}
			string command= "DELETE FROM evaluationcriterion WHERE EvaluationCriterionNum = "+POut.Long(evaluationCriterionNum);
			Db.NonQ(command);
		}



	}
}