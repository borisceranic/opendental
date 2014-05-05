using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EvaluationDefs{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all EvaluationDefs.</summary>
		private static List<EvaluationDef> listt;

		///<summary>A list of all EvaluationDefs.</summary>
		public static List<EvaluationDef> Listt{
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
			string command="SELECT * FROM evaluationdef ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="EvaluationDef";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.EvaluationDefCrud.TableToList(table);
		}
		#endregion
		*/

		///<summary>Gets all EvaluationDefs from the DB.</summary>
		public static List<EvaluationDef> Refresh(){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<EvaluationDef>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM evaluationdef";
			return Crud.EvaluationDefCrud.SelectMany(command);
		}

		///<summary>Gets all EvaluationDefs from the DB.</summary>
		public static DataTable RefreshByCourse(string courseDescript) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),courseDescript);
			}
			string command="SELECT evaluationdef.EvaluationDefNum EvaluationDefNum, evaluationdef.EvalTitle EvalTitle, schoolcourse.CourseDescript CourseDescript FROM evaluationdef "
				+"INNER JOIN schoolcourse ON schoolcourse.SchoolCourseNum=evaluationdef.SchoolCourseNum "
				+"WHERE TRUE";
			if(courseDescript!="") {
				command+=" AND schoolcourse.CourseDescript LIKE %"+POut.String(courseDescript)+"%'";
			}
			command+=" ORDER BY CourseDescript,EvalTitle";
			return Db.GetTable(command);
		}

		///<summary>Gets one EvaluationDef from the db.</summary>
		public static EvaluationDef GetOne(long evaluationDefNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<EvaluationDef>(MethodBase.GetCurrentMethod(),evaluationDefNum);
			}
			return Crud.EvaluationDefCrud.SelectOne(evaluationDefNum);
		}

		///<summary></summary>
		public static long Insert(EvaluationDef evaluationDef){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				evaluationDef.EvaluationDefNum=Meth.GetLong(MethodBase.GetCurrentMethod(),evaluationDef);
				return evaluationDef.EvaluationDefNum;
			}
			return Crud.EvaluationDefCrud.Insert(evaluationDef);
		}

		///<summary></summary>
		public static void Update(EvaluationDef evaluationDef){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),evaluationDef);
				return;
			}
			Crud.EvaluationDefCrud.Update(evaluationDef);
		}

		///<summary></summary>
		public static void Delete(long evaluationDefNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),evaluationDefNum);
				return;
			}
			string command= "DELETE FROM evaluationdef WHERE EvaluationDefNum = "+POut.Long(evaluationDefNum);
			Db.NonQ(command);
		}

		///<summary>Updates list of criterion and grading scale for the def.  Inserts or Updates the EvaluationDef as necessary.</summary>
		public static void SaveListForDef(List<EvaluationCriterionDef> listCriterionDefs,EvaluationDef evalDefCur) {
			//if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
			//	Meth.GetVoid(MethodBase.GetCurrentMethod(),listCriterionDefs,evalDefCur);
			//	return;
			//}
			//bool isDefault=true;
			//List<DisplayField> defaultList=GetDefaultList(category);
			//if(listCriterionDefs.Count!=defaultList.Count) {
			//	isDefault=false;
			//}
			//else {
			//	for(int i=0;i<listCriterionDefs.Count;i++) {
			//		if(listCriterionDefs[i].Description!="") {
			//			isDefault=false;
			//			break;
			//		}
			//		if(listCriterionDefs[i].InternalName!=defaultList[i].InternalName) {
			//			isDefault=false;
			//			break;
			//		}
			//		if(listCriterionDefs[i].ColumnWidth!=defaultList[i].ColumnWidth) {
			//			isDefault=false;
			//			break;
			//		}
			//	}
			//}
			//string command="DELETE FROM displayfield WHERE Category="+POut.Long((int)category);
			//Db.NonQ(command);
			//if(isDefault) {
			//	return;
			//}
			//for(int i=0;i<listCriterionDefs.Count;i++) {
			//	listCriterionDefs[i].ItemOrder=i;
			//	Insert(listCriterionDefs[i]);
			//}
		}



	}
}