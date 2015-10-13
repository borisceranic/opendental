using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class RequiredFields{
		#region CachePattern
		///<summary>A list of all RequiredFields.</summary>
		private static List<RequiredField> listt;

		///<summary>A list of all RequiredFields.</summary>
		public static List<RequiredField> Listt{
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
			string command="SELECT * FROM requiredfield ORDER BY FieldType,FieldName";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="RequiredField";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.RequiredFieldCrud.TableToList(table);
		}
		#endregion

		///<summary></summary>
		public static long Insert(RequiredField requiredField){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				requiredField.RequiredFieldNum=Meth.GetLong(MethodBase.GetCurrentMethod(),requiredField);
				return requiredField.RequiredFieldNum;
			}
			return Crud.RequiredFieldCrud.Insert(requiredField);
		}
		
		///<summary></summary>
		public static void Update(RequiredField requiredField){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),requiredField);
				return;
			}
			Crud.RequiredFieldCrud.Update(requiredField);
		}

		///<summary></summary>
		public static void Delete(long requiredFieldNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),requiredFieldNum);
				return;
			}
			string command="DELETE FROM requiredfieldcondition WHERE RequiredFieldNum="+POut.Long(requiredFieldNum);
			Db.NonQ(command);
			Crud.RequiredFieldCrud.Delete(requiredFieldNum);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<RequiredField> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<RequiredField>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM requiredfield WHERE PatNum = "+POut.Long(patNum);
			return Crud.RequiredFieldCrud.SelectMany(command);
		}

		///<summary>Gets one RequiredField from the db.</summary>
		public static RequiredField GetOne(long requiredFieldNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<RequiredField>(MethodBase.GetCurrentMethod(),requiredFieldNum);
			}
			return Crud.RequiredFieldCrud.SelectOne(requiredFieldNum);
		}
		*/	

	}
}