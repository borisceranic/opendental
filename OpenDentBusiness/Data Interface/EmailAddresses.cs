using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EmailAddresses{
		#region CachePattern
		///<summary>A list of all EmailAddresses whose UserNum=0.</summary>
		private static List<EmailAddress> _listt;
		private static object _lockObj=new object();

		///<summary>A list of all EmailAddresses whose UserNum=0.</summary>
		public static List<EmailAddress> Listt{
			get {
				return GetListt();
			}
			set {
				lock(_lockObj) {
					_listt=value;
				}
			}
		}

		///<summary>A list of all EmailAddresses whose UserNum=0.</summary>
		public static List<EmailAddress> GetListt() {
			bool isListNull=false;
			lock(_lockObj) {
				if(_listt==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				RefreshCache();
			}
			List<EmailAddress> listEmailAddresses=new List<EmailAddress>();
			lock(_lockObj) {
				for(int i=0;i<_listt.Count;i++) {
					listEmailAddresses.Add(_listt[i].Clone());
				}
			}
			return listEmailAddresses;
		}

		///<summary></summary>
		public static DataTable RefreshCache(){
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM emailaddress WHERE UserNum = 0 ORDER BY EmailUsername";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="EmailAddress";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			Listt=Crud.EmailAddressCrud.TableToList(table);
		}
		#endregion

		///<summary>Gets the default email address for the clinic/practice. Takes a clinic num. If clinic num is 0 or there is no default for that clinic, it will get practice default. May return a new blank object.</summary>
		public static EmailAddress GetByClinic(long clinicNum) {
			EmailAddress emailAddress=null;
			Clinic clinic=Clinics.GetClinic(clinicNum);
			if(PrefC.GetBool(PrefName.EasyNoClinics) || clinic==null) {//No clinic, get practice default
				emailAddress=GetOne(PrefC.GetLong(PrefName.EmailDefaultAddressNum));
			}
			else {
				emailAddress=GetOne(clinic.EmailAddressNum);
				if(emailAddress==null) {//clinic.EmailAddressNum 0. Use default.
					emailAddress=GetOne(PrefC.GetLong(PrefName.EmailDefaultAddressNum));
				}
			}
			if(emailAddress==null) {
				List<EmailAddress> listEmailAddresses=GetListt();
				if(listEmailAddresses.Count>0) {//user didn't set a default, so just pick the first email in the list.
					emailAddress=listEmailAddresses[0];
				}
				else {
					emailAddress=new EmailAddress();//To avoid null checks.
					emailAddress.EmailPassword="";
					emailAddress.EmailUsername="";
					emailAddress.Pop3ServerIncoming="";
					emailAddress.SenderAddress="";
					emailAddress.SMTPserver="";
				}
			}
			return emailAddress;
		}

		///<summary>Executes a query to the database to get the email address associated to the passed-in user.  
		///Does not use the cache.  Returns null if no email address in the database matches the passed-in user.</summary>
		public static EmailAddress GetForUser(long userNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<EmailAddress>(MethodBase.GetCurrentMethod(),userNum);
			}
			string command="SELECT * FROM emailaddress WHERE emailaddress.UserNum = "+userNum;
			return Crud.EmailAddressCrud.SelectOne(command);
		}

		///<summary>Gets one EmailAddress from the cached listt.  Might be null.</summary>
		public static EmailAddress GetOne(long emailAddressNum){
			//No need to check RemoteRole; Calls GetTableRemotelyIfNeeded().
			List<EmailAddress> listEmailAddresses=GetListt();
			for(int i=0;i<listEmailAddresses.Count;i++) {
				if(listEmailAddresses[i].EmailAddressNum==emailAddressNum) {
					return listEmailAddresses[i];
				}
			}
			return null;
		}

		///<summary>Gets all email addresses, including those email addresses which are not in the cache.</summary>
		public static List<EmailAddress> GetAll() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<EmailAddress>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM emailaddress";
			return Crud.EmailAddressCrud.SelectMany(command);
		}

		///<summary>Checks to make sure at least one non-user email address has a valid (not blank) SMTP server.</summary>
		public static bool ExistsValidEmail() {
			List<EmailAddress> listEmailAddresses=GetListt();
			for(int i=0;i<listEmailAddresses.Count;i++) {
				if(listEmailAddresses[i].SMTPserver!="") {
					return true;
				}
			}
			return false;
		}

		///<summary></summary>
		public static long Insert(EmailAddress emailAddress) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				emailAddress.EmailAddressNum=Meth.GetLong(MethodBase.GetCurrentMethod(),emailAddress);
				return emailAddress.EmailAddressNum;
			}
			return Crud.EmailAddressCrud.Insert(emailAddress);
		}

		///<summary></summary>
		public static void Update(EmailAddress emailAddress){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),emailAddress);
				return;
			}
			Crud.EmailAddressCrud.Update(emailAddress);
		}

		///<summary></summary>
		public static void Delete(long emailAddressNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),emailAddressNum);
				return;
			}
			string command= "DELETE FROM emailaddress WHERE EmailAddressNum = "+POut.Long(emailAddressNum);
			Db.NonQ(command);
		}



	}
}