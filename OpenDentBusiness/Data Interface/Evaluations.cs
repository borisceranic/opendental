using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Evaluations{
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all Evaluations.</summary>
		private static List<Evaluation> listt;

		///<summary>A list of all Evaluations.</summary>
		public static List<Evaluation> Listt{
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
			string command="SELECT * FROM evaluation ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="Evaluation";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.EvaluationCrud.TableToList(table);
		}
		#endregion
		*/

		///<summary></summary>
		public static List<Evaluation> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Evaluation>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM evaluation WHERE PatNum = "+POut.Long(patNum);
			return Crud.EvaluationCrud.SelectMany(command);
		}

		///<summary>Gets one Evaluation from the db.</summary>
		public static Evaluation GetOne(long evaluationNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<Evaluation>(MethodBase.GetCurrentMethod(),evaluationNum);
			}
			return Crud.EvaluationCrud.SelectOne(evaluationNum);
		}

		///<summary>Gets all Evaluations from the DB.  Multiple filters are available.  Dates must be valid before calling this.</summary>
		public static DataTable GetFilteredList(DateTime dateStart,DateTime dateEnd,string lastName,string firstName,long uniqueID,long courseNum,long instructorNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateStart,dateEnd,lastName,firstName,uniqueID,courseNum,instructorNum);
			}
			string command="SELECT evaluation.EvaluationNum,evaluation.EvalTitle,evaluation.DateEval,evaluation.StudentNum,evaluation.InstructorNum,"
			+"provider.LName,provider.FName,schoolcourse.Descript,gradingscale.Description,evaluation.OverallGradeShowing FROM evaluation "
				+"INNER JOIN provider ON provider.ProvNum=evaluation.InstructorNum || provider.ProvNum=evaluation.StudentNum) "
				+"INNER JOIN schoolcourse ON schoolcourse.SchoolCourseNum=evaluation.SchoolCourseNum "
				+"INNER JOIN gradingscale ON gradingscale.GradingScaleNum=evaluation.GradingScaleNum "
				+"WHERE TRUE";
			if(!String.IsNullOrWhiteSpace(lastName)) {
				command+=" AND provider.LName LIKE '%"+POut.String(lastName)+"%'";
			}
			if(!String.IsNullOrWhiteSpace(firstName)) {
				command+=" AND provider.FName LIKE '%"+POut.String(firstName)+"%'";
			}
			if(uniqueID!=0) {
				command+=" AND evaluation.StudentNum = '"+POut.Long(uniqueID)+"'";
			}
			if(courseNum!=0) {
				command+=" AND schoolcourse.SchoolCourseNum = '"+POut.Long(courseNum)+"'";
			}
			if(instructorNum!=0) {
				command+=" AND evaluation.InstructorNum = '"+POut.Long(instructorNum)+"'";
			}
			command+=" AND evaluation.DateEval BETWEEN "+POut.Date(dateStart)+" AND "+POut.Date(dateEnd);
			command+=" ORDER BY DateEval,LName";
			return Db.GetTable(command);
		}

		///<summary></summary>
		public static long Insert(Evaluation evaluation){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				evaluation.EvaluationNum=Meth.GetLong(MethodBase.GetCurrentMethod(),evaluation);
				return evaluation.EvaluationNum;
			}
			return Crud.EvaluationCrud.Insert(evaluation);
		}

		///<summary></summary>
		public static void Update(Evaluation evaluation){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),evaluation);
				return;
			}
			Crud.EvaluationCrud.Update(evaluation);
		}

		///<summary></summary>
		public static void Delete(long evaluationNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),evaluationNum);
				return;
			}
			string command= "DELETE FROM evaluation WHERE EvaluationNum = "+POut.Long(evaluationNum);
			Db.NonQ(command);
		}
		



	}
}