using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class RequiredFieldConditions {
		#region CachePattern
		///<summary>A list of all RequiredFieldConditions.</summary>
		private static List<RequiredFieldCondition> listt;

		///<summary>A list of all RequiredFieldConditions.</summary>
		public static List<RequiredFieldCondition> Listt {
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
		public static DataTable RefreshCache() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM requiredfieldcondition ORDER BY ConditionType,RequiredFieldConditionNum";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="RequiredFieldCondition";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table) {
			//No need to check RemotingRole; no call to db.
			listt=Crud.RequiredFieldConditionCrud.TableToList(table);
		}
		#endregion

		///<summary>Gets the requiredfieldconditions for one required field.</summary>
		public static List<RequiredFieldCondition> GetForRequiredField(long requiredFieldNum) {
			//No need to check RemotingRole; no call to db.
			return Listt.FindAll(x => x.RequiredFieldNum==requiredFieldNum);
		}

		///<summary></summary>
		public static long Insert(RequiredFieldCondition requiredFieldCondition){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				requiredFieldCondition.RequiredFieldConditionNum=Meth.GetLong(MethodBase.GetCurrentMethod(),requiredFieldCondition);
				return requiredFieldCondition.RequiredFieldConditionNum;
			}
			return Crud.RequiredFieldConditionCrud.Insert(requiredFieldCondition);
		}

		///<summary></summary>
		public static void Update(RequiredFieldCondition requiredFieldCondition){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),requiredFieldCondition);
				return;
			}
			Crud.RequiredFieldConditionCrud.Update(requiredFieldCondition);
		}

		public static void DeleteAll(List<long> listRequiredFieldCondNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listRequiredFieldCondNums);
				return;
			}
			if(listRequiredFieldCondNums.Count<1) {
				return;
			}
			string command="DELETE FROM requiredfieldcondition WHERE RequiredFieldConditionNum IN("+string.Join(",",listRequiredFieldCondNums)+")";
			Db.NonQ(command);
		}

		/*
		///<summary></summary>
		public static void Delete(long requiredFieldConditionNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),requiredFieldConditionNum);
				return;
			}
			Crud.RequiredFieldConditionCrud.Delete(requiredFieldConditionNum);
		}

		///<summary></summary>
		public static List<RequiredFieldCondition> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<RequiredFieldCondition>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM requiredfieldcondition WHERE PatNum = "+POut.Long(patNum);
			return Crud.RequiredFieldConditionCrud.SelectMany(command);
		}

		///<summary>Gets one RequiredFieldCondition from the db.</summary>
		public static RequiredFieldCondition GetOne(long requiredFieldConditionNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<RequiredFieldCondition>(MethodBase.GetCurrentMethod(),requiredFieldConditionNum);
			}
			return Crud.RequiredFieldConditionCrud.SelectOne(requiredFieldConditionNum);
		}
		*/

	}
}