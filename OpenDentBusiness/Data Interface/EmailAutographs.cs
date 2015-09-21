using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EmailAutographs{
		
		#region CachePattern

		///<summary>A list of all EmailAutographs.</summary>
		private static List<EmailAutograph> listt;

		///<summary>A list of all EmailAutographs.</summary>
		public static List<EmailAutograph> Listt{
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
			string command="SELECT * FROM emailautograph ORDER BY Description";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="EmailAutograph";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.EmailAutographCrud.TableToList(table);
		}
		#endregion
	
		/////<summary>Gets one EmailAutograph from the db.</summary>
		//public static EmailAutograph GetOne(long emailAutographNum){
		//	if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
		//		return Meth.GetObject<EmailAutograph>(MethodBase.GetCurrentMethod(),emailAutographNum);
		//	}
		//	return Crud.EmailAutographCrud.SelectOne(emailAutographNum);
		//}
		
		///<summary>Insert one EmailAutograph in the database.</summary>
		public static long Insert(EmailAutograph emailAutograph){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				emailAutograph.EmailAutographNum=Meth.GetLong(MethodBase.GetCurrentMethod(),emailAutograph);
				return emailAutograph.EmailAutographNum;
			}
			return Crud.EmailAutographCrud.Insert(emailAutograph);
		}
		
		///<summary>Updates an existing EmailAutograph in the database.</summary>
		public static void Update(EmailAutograph emailAutograph){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),emailAutograph);
				return;
			}
			Crud.EmailAutographCrud.Update(emailAutograph);
		}

		///<summary>Delete on EmailAutograph from the database.</summary>
		public static void Delete(long emailAutographNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),emailAutographNum);
				return;
			}
			Crud.EmailAutographCrud.Delete(emailAutographNum);
		}

	}
}