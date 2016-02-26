using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Web;
using CodeBase;
using System.Windows.Forms;

namespace OpenDentBusiness {
	///<summary>(Users OD)</summary>
	public class Userods {
		//private static bool webServerConfigHasLoadedd=false;
		
		///<summary></summary>
		public static DataTable RefreshCache() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM userod ORDER BY UserName";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="Userod";
			FillCache(table);
			return table;
		}

		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			List<Userod> listUserodsLong=Crud.UserodCrud.TableToList(table);
			List<Userod> listUserodsShort=new List<Userod>();
			for(int i=0;i<listUserodsLong.Count;i++) {//This logic used to be in UserodC, but didn't really follow our new pattern.
				if(listUserodsLong[i].IsHidden || listUserodsLong[i].UserNumCEMT!=0) {
					continue;
				}
				listUserodsShort.Add(listUserodsLong[i]);
			}
			UserodC.Listt=listUserodsLong;
			UserodC.ShortList=listUserodsShort;
		}

		///<summary></summary>
		public static Userod GetUser(long userNum) {
			//No need to check RemotingRole; no call to db.
			List<Userod> listUserods=UserodC.GetListt();
			if(listUserods==null){
				RefreshCache();
			}
			for(int i=0;i<listUserods.Count;i++) {
				if(listUserods[i].UserNum==userNum){
					return listUserods[i];
				}
			}
			return null;
		}

		///<summary>Returns a list of all non-hidden users.  Does not include CEMT users.</summary>
		public static List<Userod> GetUsers() {
			return GetUsers(false);
		}

		///<summary>Returns a list of all non-hidden users.  Set includeCEMT to true if you want CEMT users included.</summary>
		public static List<Userod> GetUsers(bool includeCEMT) {
			List<Userod> retVal=new List<Userod>();
			List<Userod> listUsersLong=UserodC.GetListt();
			for(int i=0;i<listUsersLong.Count;i++) {
				if(listUsersLong[i].IsHidden) {
					continue;
				}
				if(!includeCEMT && listUsersLong[i].UserNumCEMT!=0) {
					continue;
				}
				retVal.Add(listUsersLong[i]);
			}
			return retVal;
		}
		
		///<summary>Returns a list of all users without using the local cache.  Useful for multithreaded connections.</summary>
		public static List<Userod> GetUsersNoCache() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Userod>>(MethodBase.GetCurrentMethod());
			}
			List<Userod> retVal=new List<Userod>();
			string command="SELECT * FROM userod";
			DataTable tableUsers=Db.GetTable(command);
			retVal=Crud.UserodCrud.TableToList(tableUsers);
			return retVal;
		}

		///<summary>Returns a list of all non-hidden CEMT users.</summary>
		public static List<Userod> GetUsersForCEMT() {
			List<Userod> retVal=new List<Userod>();
			List<Userod> listUsers=UserodC.GetListt();
			for(int i=0;i<listUsers.Count;i++) {
				if(listUsers[i].UserNumCEMT==0) {
					continue;
				}
				retVal.Add(listUsers[i]);
			}
			return retVal;
		}

		///<summary>Returns null if not found.  isEcwTight is not case sensitive.</summary>
		public static Userod GetUserByName(string userName,bool isEcwTight) {
			//No need to check RemotingRole; no call to db.
			List<Userod> listUserods=UserodC.GetListt();
			if(listUserods==null) {
				RefreshCache();
			}
			for(int i=0;i<listUserods.Count;i++) {
				if(listUserods[i].IsHidden){
					continue;
				}
				if(isEcwTight) {
					if(listUserods[i].UserName.ToLower()==userName.ToLower()) {
						return listUserods[i];
					}
				}
				else {
					if(listUserods[i].UserName==userName) {//exact
						return listUserods[i];
					}
				}
			}
			return null;
		}

		///<summary>Returns null if not found.</summary>
		public static Userod GetUserByEmployeeNum(long employeeNum) {
			//No need to check RemotingRole; no call to db.
			List<Userod> listUserods=UserodC.GetListt();
			for(int i=0;i<listUserods.Count;i++) {
				if(listUserods[i].EmployeeNum==employeeNum) {
					return listUserods[i];
				}
			}
			return null;
		}

		///<summary>Returns all users that are associated to the employee passed in.  Returns empty list if no matches found.</summary>
		public static List<Userod> GetUsersByEmployeeNum(long employeeNum) {
			//No need to check RemotingRole; no call to db.
			List<Userod> listUserodsLong=UserodC.GetListt();
			List<Userod> listUserods=new List<Userod>();
			for(int i=0;i<listUserodsLong.Count;i++) {
				if(listUserodsLong[i].EmployeeNum==employeeNum) {
					listUserods.Add(listUserodsLong[i]);
				}
			}
			return listUserods;
		}

		///<summary>Returns all users that are associated to the permission passed in.  Returns empty list if no matches found.</summary>
		public static List<Userod> GetUsersByPermission(Permissions permission,bool showHidden) {
			//No need to check RemotingRole; no call to db.
			List<Userod> listAllUsers=new List<Userod>();UserodC.GetListShort();
			if(showHidden) {
				listAllUsers=UserodC.GetListt();
			}
			else {
				listAllUsers=UserodC.GetListShort();
			}
			List<Userod> listUserods=new List<Userod>();
			for(int i=0;i<listAllUsers.Count;i++) {
				if(GroupPermissions.HasPermission(listAllUsers[i].UserGroupNum,permission)) {
					listUserods.Add(listAllUsers[i]);
				}
			}
			return listUserods;
		}

		///<summary>Returns all users that are associated to the permission passed in.  Returns empty list if no matches found.</summary>
		public static List<Userod> GetUsersByJobRole(JobPerm jobPerm,bool showHidden) {
			List<JobPermission> listJobRoles=JobPermissions.GetList().FindAll(x=>x.JobPermType==jobPerm);
			if(showHidden) {
				return UserodC.GetListt().FindAll(x=>listJobRoles.Any(y=>x.UserNum==y.UserNum));
			}
			return UserodC.GetListShort().FindAll(x=>listJobRoles.Any(y=>x.UserNum==y.UserNum));
		}

		///<summary>This handles situations where we have a usernum, but not a user.  And it handles usernum of zero.  Pass in a list of users to save making a deep copy of the userod cache if you are going to be calling this method repeatedly.  js Must maintain 2 overloads instead of optional parameter for my dll.</summary>
		public static string GetName(long userNum) {
			return GetName(userNum,null);
		}

		///<summary>This handles situations where we have a usernum, but not a user.  And it handles usernum of zero.  Pass in a list of users to save making a deep copy of the userod cache if you are going to be calling this method repeatedly.  js Must maintain 2 overloads instead of optional parameter for my dll.</summary>
		public static string GetName(long userNum,List<Userod> listUserods) {
			//No need to check RemotingRole; no call to db.
			if(userNum==0) {
				return "";
			}
			Userod user=null;
			if(listUserods!=null) {
				user=listUserods.FirstOrDefault(x => x.UserNum==userNum);
			}
			else {
				user=GetUser(userNum);
			}
			if(user==null) {
				return "";
			}
			return user.UserName;
		}

		///<summary>Returns true if the user passed in is associated with a provider that has (or had) an EHR prov key.</summary>
		public static bool IsUserCpoe(Userod user) {
			//No need to check RemotingRole; no call to db.
			if(user==null) {
				return false;
			}
			Provider prov=Providers.GetProv(user.ProvNum);
			if(prov==null) {
				return false;
			}
			//Check to see if this provider has had a valid key at any point in history.
			return EhrProvKeys.HasProvHadKey(prov.LName,prov.FName);
		}

		///<summary>Only used in one place on the server when first attempting to log on.  The password will be hashed and checked against the one in the database.  Password is required, so empty string will return null.  Returns a user object if user and password are valid.  Otherwise, returns null.  If usingEcw, password will actually be the hash.  If usingEcw, then the username is not case sensitive.</summary>
		public static Userod CheckUserAndPassword(string username,string password,bool usingEcw) {
			//No need to check RemotingRole; no call to db.
			if(password==""){
				return null;
			}
			RefreshCache();
			Userod user=GetUserByName(username,usingEcw);
			if(user==null){
				return null;
			}
			if(usingEcw){
				if(user.Password==password) {
					return user;
				}
			}
			else if(user.Password==HashPassword(password)) {
				return user;
			}
			return null;
		}

		///<summary>Used by Server.  Throws exception if bad username or passhash or if either are blank.  It uses cached user list, refreshing it if null.  This is used everywhere except in the log on screen.</summary>
		public static void CheckCredentials(Credentials cred){
			//No need to check RemotingRole; no call to db.
			#if DEBUG
				return;//skip checking credentials when in debug for faster testing.
			#endif
			if(cred.Username=="" || cred.Password==""){
				throw new ApplicationException("Invalid username or password.");
			}
			Userod userod=null;
			List<Userod> listUserods=UserodC.GetListt();
			for(int i=0;i<listUserods.Count;i++){
				if(listUserods[i].UserName==cred.Username){
					userod=listUserods[i];
					break;
				}
			}
			if(userod==null){
				throw new ApplicationException("Invalid username or password.");
			}
			bool useEcwAlgorithm=Programs.IsEnabled(ProgramName.eClinicalWorks);
			if(useEcwAlgorithm){
				if(userod.Password!=cred.Password){
					throw new ApplicationException("Invalid username or password.");
				}
			}
			else if(userod.Password!=HashPassword(cred.Password)){
				throw new ApplicationException("Invalid username or password.");
			}
		}

		///<summary>Will throw an exception if it fails for any reason.  This will directly access the config file on the disk, read the values, and set the DataConnection to the new database.  This is only triggered when someone tries to log on.</summary>
		public static void LoadDatabaseInfoFromFile(string configFilePath){
			//No need to check RemotingRole; no call to db.
			if(!File.Exists(configFilePath)){
				throw new Exception("Could not find "+configFilePath+" on the web server.");
			}
			XmlDocument doc=new XmlDocument();
			try {
				doc.Load(configFilePath);
			}
			catch{
				throw new Exception("Web server "+configFilePath+" could not be opened or is in an invalid format.");
			}
			XPathNavigator Navigator=doc.CreateNavigator();
			//always picks the first database entry in the file:
			XPathNavigator navConn=Navigator.SelectSingleNode("//DatabaseConnection");//[Database='"+database+"']");
			if(navConn==null) {
				throw new Exception(configFilePath+" does not contain a valid database entry.");//database+" is not an allowed database.");
			}
			string connString="",server="",database="",mysqlUser="",mysqlPassword="",mysqlUserLow="",mysqlPasswordLow="";
			XPathNavigator navConString=navConn.SelectSingleNode("ConnectionString");
			if(navConString!=null) {//If there is a connection string then use it.
				connString=navConString.Value;
			}
			else {
				//return navOne.SelectSingleNode("summary").Value;
				//now, get the values for this connection
				server=navConn.SelectSingleNode("ComputerName").Value;
				database=navConn.SelectSingleNode("Database").Value;
				mysqlUser=navConn.SelectSingleNode("User").Value;
				mysqlPassword=navConn.SelectSingleNode("Password").Value;
				mysqlUserLow=navConn.SelectSingleNode("UserLow").Value;
				mysqlPasswordLow=navConn.SelectSingleNode("PasswordLow").Value;
			}
			XPathNavigator dbTypeNav=navConn.SelectSingleNode("DatabaseType");
			DatabaseType dbtype=DatabaseType.MySql;
			if(dbTypeNav!=null){
				if(dbTypeNav.Value=="Oracle"){
					dbtype=DatabaseType.Oracle;
				}
			}
			DataConnection dcon=new DataConnection();
			if(connString!="") {
				try {
					dcon.SetDb(connString,"",dbtype);
				}
				catch(Exception e) {
					throw new Exception(e.Message+"\r\n"+"Connection to database failed.  Check the values in the config file on the web server "+configFilePath);
				}
			}
			else {
				try {
					dcon.SetDb(server,database,mysqlUser,mysqlPassword,mysqlUserLow,mysqlPasswordLow,dbtype);
				}
				catch(Exception e) {
					throw new Exception(e.Message+"\r\n"+"Connection to database failed.  Check the values in the config file on the web server "+configFilePath);
				}
			}
			//todo?: make sure no users have blank passwords.
		}

		/*
		///<summary>Used by the SL logon window to validate credentials.  Send in the password unhashed.  If invalid, it will always throw an exception of some type.  If it is valid, then it will return the hashed password.  This is the only place where the config file is read, and it's only read on startup.  So the web service needs to be restarted if the config file changes.</summary>
		public static string CheckDbUserPassword(string configFilePath,string username,string password){
			//for some reason, this static variable was remaining true even if the webservice was restarted.
			//So we're not going to use it anymore.  Always load from file.
			//if(!webServerConfigHasLoadedd){
				LoadDatabaseInfoFromFile(configFilePath);
			//	webServerConfigHasLoadedd=true;
			//}
			DataConnection dcon=new DataConnection();
			//Then, check username and password
			string passhash="";
			string command="SELECT Password FROM userod WHERE UserName='"+POut.PString(username)+"'";
			DataTable table=dcon.GetTable(command);
			if(table.Rows.Count!=0){
				passhash=table.Rows[0][0].ToString();
			}
			if(passhash=="" || passhash!=EncryptPassword(password)){
				throw new Exception("Invalid username or password.");
			}
			return passhash;
		}*/

		///<summary></summary>
		public static string HashPassword(string inputPass) {
			//No need to check RemotingRole; no call to db.
			bool useEcwAlgorithm=Programs.IsEnabled(ProgramName.eClinicalWorks);
			return HashPassword(inputPass,useEcwAlgorithm);
		}

		///<summary></summary>
		public static string HashPassword(string inputPass,bool useEcwAlgorithm) {
			//No need to check RemotingRole; no call to db.
			if(inputPass=="") {
				return "";
			}
			HashAlgorithm algorithm=HashAlgorithm.Create("MD5");
			if(useEcwAlgorithm){// && Programs.IsEnabled("eClinicalWorks")) {
				byte[] asciiBytes=Encoding.ASCII.GetBytes(inputPass);
				byte[] hashbytes=algorithm.ComputeHash(asciiBytes);//length=16
				byte digit1;
				byte digit2;
				string char1;
				string char2;
				StringBuilder strbuild=new StringBuilder();
				for(int i=0;i<hashbytes.Length;i++) {
					if(hashbytes[i]==0) {
						digit1=0;
						digit2=0;
					}
					else {
						digit1=(byte)Math.Floor((double)hashbytes[i]/16d);
						//double remainder=Math.IEEERemainder((double)hashbytes[i],16d);
						digit2=(byte)(hashbytes[i]-(byte)(16*digit1));
					}
					char1=ByteToStr(digit1);
					char2=ByteToStr(digit2);
					strbuild.Append(char1);
					strbuild.Append(char2);
				}
				return strbuild.ToString();
			}
			else {//typical
				byte[] unicodeBytes=Encoding.Unicode.GetBytes(inputPass);
				byte[] hashbytes2=algorithm.ComputeHash(unicodeBytes);
				return Convert.ToBase64String(hashbytes2);
			}
		}


		///<summary>The only valid input is a value between 0 and 15.  Text returned will be 1-9 or a-f.</summary>
		private static string ByteToStr(Byte byteVal) {
			//No need to check RemotingRole; no call to db.
			switch(byteVal) {
				case 10:
					return "a";
				case 11:
					return "b";
				case 12:
					return "c";
				case 13:
					return "d";
				case 14:
					return "e";
				case 15:
					return "f";
				default:
					return byteVal.ToString();
			}
		}

		///<summary>Used from log on screen, phoneUI, and when logging in via command line.</summary>
		public static bool CheckTypedPassword(string inputPass,string hashedPass) {
			//No need to check RemotingRole; no call to db.
			if(hashedPass=="") {
				return inputPass=="";
			}
			string hashedInput=HashPassword(inputPass);
			//MessageBox.Show(
			//Debug.WriteLine(hashedInput+","+hashedPass);
			return hashedInput==hashedPass;
		}		

		///<summary>usertype can be 'all', 'prov', 'emp', 'stu', 'inst', or 'other'.</summary>
		public static List<Userod> RefreshSecurity(string usertype,long schoolClassNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Userod>>(MethodBase.GetCurrentMethod(),usertype,schoolClassNum);
			}
			string command;
			if(usertype=="stu" || usertype=="inst") {
				command="SELECT userod.* FROM userod,provider "
					+"WHERE userod.ProvNum=provider.ProvNum "
					+"AND provider.IsInstructor=";
				if(usertype=="inst") {
					command+="1 ";
				}
				else {
					command+="0 ";
				}
				if(usertype=="stu" && schoolClassNum>0) {
					command+="AND SchoolClassNum="+POut.Long(schoolClassNum)+" ";
				}
				command+="ORDER BY UserName";
				return Crud.UserodCrud.SelectMany(command);
			}
			command="SELECT * FROM userod WHERE UserNumCEMT=0  ";
			if(usertype=="emp"){
				command+="AND EmployeeNum!=0 ";
			}
			else if(usertype=="prov") {//and all schoolclassnums
				command+="AND ProvNum!=0 ";
			}
			else if(usertype=="all") {
				//command+="";
			}
			else if(usertype=="other") {
				command+="AND ProvNum=0 AND EmployeeNum=0 ";
			}
			command+="ORDER BY UserName";
			return Crud.UserodCrud.SelectMany(command);
		}

		///<summary>Updates all students/instructors to the specified user group.  Surround with try/catch because it can throw exceptions.</summary>
		public static void UpdateUserGroupsForDentalSchools(UserGroup userGroup,bool isInstructor) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),userGroup,isInstructor);
				return;
			}
			string command;
			//Check if the user group that the students or instructors are trying to go to has the SecurityAdmin permission.
			if(!GroupPermissions.HasPermission(userGroup.UserGroupNum,Permissions.SecurityAdmin)) {
				//We need to make sure that moving these users to the new user group does not eliminate all SecurityAdmin users in db.
				command="SELECT COUNT(*) FROM userod "
					+"INNER JOIN usergroup ON userod.UserGroupNum=usergroup.UserGroupNum "
					+"INNER JOIN grouppermission ON grouppermission.UserGroupNum=usergroup.UserGroupNum "
					+"WHERE userod.UserNum NOT IN "
					+"(SELECT userod.UserNum FROM userod,provider "
						+"WHERE userod.ProvNum=provider.ProvNum ";
				if(!isInstructor) {
					command+="AND provider.IsInstructor="+POut.Bool(isInstructor)+" ";
					command+="AND provider.SchoolClassNum!=0) ";
				}
				else {
					command+="AND provider.IsInstructor="+POut.Bool(isInstructor)+") ";
				}
					command+="AND grouppermission.PermType="+POut.Int((int)Permissions.SecurityAdmin)+" ";
				int lastAdmin=PIn.Int(Db.GetCount(command));
				if(lastAdmin==0) {
					throw new Exception("Cannot move students or instructors to the new user group because it would leave no users with the SecurityAdmin permission.");
				}
			}
			command="UPDATE userod INNER JOIN provider ON userod.ProvNum=provider.ProvNum "
					+"SET UserGroupNum="+POut.Long(userGroup.UserGroupNum)+" "
					+"WHERE provider.IsInstructor="+POut.Bool(isInstructor);
			if(!isInstructor) {
				command+=" AND provider.SchoolClassNum!=0";
			}
			Db.NonQ(command);
		}

		///<summary>Surround with try/catch because it can throw exceptions.</summary>
		public static void Update(Userod userod) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),userod);
				return;
			}
			Validate(false,userod,false);
			Crud.UserodCrud.Update(userod);
		}

		///<summary>Update for CEMT only.  Used when updating Remote databases with information from the CEMT.  Because of potentially different primary keys we have to update based on UserNumCEMT.</summary>
		public static void UpdateCEMT(Userod userod) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),userod);
				return;
			}
			//Validate(false,userod,false);//Can't use this validate. it's for normal updating only.
			string command="UPDATE userod SET "
				+"UserName          = '"+POut.String(userod.UserName)+"', "
				+"Password          = '"+POut.String(userod.Password)+"', "
				+"UserGroupNum      =  "+POut.Long(userod.UserGroupNum)+", "//need to find primary key of remote user group
				+"EmployeeNum       =  "+POut.Long  (userod.EmployeeNum)+", "
				+"ClinicNum         =  "+POut.Long  (userod.ClinicNum)+", "
				+"ProvNum           =  "+POut.Long  (userod.ProvNum)+", "
				+"IsHidden          =  "+POut.Bool  (userod.IsHidden)+", "
				+"TaskListInBox     =  "+POut.Long  (userod.TaskListInBox)+", "
				+"AnesthProvType    =  "+POut.Int   (userod.AnesthProvType)+", "
				+"DefaultHidePopups =  "+POut.Bool  (userod.DefaultHidePopups)+", "
				+"PasswordIsStrong  =  "+POut.Bool  (userod.PasswordIsStrong)+", "
				+"ClinicIsRestricted=  "+POut.Bool  (userod.ClinicIsRestricted)+", "
				+"InboxHidePopups   =  "+POut.Bool  (userod.InboxHidePopups)+" "
				+"WHERE UserNumCEMT = "+POut.Long(userod.UserNumCEMT);
			Db.NonQ(command);
		}

		///<summary>Surround with try/catch because it can throw exceptions.  Only used from FormOpenDental.menuItemPassword_Click().  Same as Update(), only the Validate call skips checking duplicate names for hidden users.</summary>
		public static void UpdatePassword(Userod userod){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),userod);
				return;
			}
			Validate(false,userod,true);
			Crud.UserodCrud.Update(userod);
		}

		///<summary>Surround with try/catch because it can throw exceptions.</summary>
		public static long Insert(Userod userod){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				userod.UserNum=Meth.GetLong(MethodBase.GetCurrentMethod(),userod);
				return userod.UserNum;
			}
			Validate(true,userod,false);
			return Crud.UserodCrud.Insert(userod);
		}

		public static long InsertNoCache(Userod userod){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetLong(MethodBase.GetCurrentMethod(),userod);
			}
			return Crud.UserodCrud.InsertNoCache(userod);
		}

		///<summary>Surround with try/catch because it can throw exceptions.  We don't really need to make this public, but it's required in order to follow the RemotingRole pattern.</summary>
		public static void Validate(bool isNew,Userod user,bool excludeHiddenUsers){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),isNew,user,excludeHiddenUsers);
				return;
			}
			//should add a check that employeenum and provnum are not both set.
			//make sure username is not already taken
			string command;
			long excludeUserNum;
			if(isNew){
				excludeUserNum=0;
			}
			else{
				excludeUserNum=user.UserNum;//it's ok if the name matches the current username
			}
			//It doesn't matter if the UserName is already in use if the user being updated is going to be hidden.  This check will block them from unhiding duplicate users.
			if(!user.IsHidden) {//if the user is now not hidden
				//CEMT users will not be visible from within Open Dental.  Therefore, make a different check so that we can know if the name
				//the user typed in is a duplicate of a CEMT user.  In doing this, we are able to give a better message.
				if(!IsUserNameUnique(user.UserName,excludeUserNum,excludeHiddenUsers,true)) {
					throw new Exception(Lans.g("Userods","UserName already in use by CEMT member."));
				}
				if(!IsUserNameUnique(user.UserName,excludeUserNum,excludeHiddenUsers)) {
					//IsUserNameUnique doesn't care if it's a CEMT user or not.. It just gets a count based on username.
					throw new Exception(Lans.g("Userods","UserName already in use."));
				}
			}
			//make sure that there would still be at least one user with security admin permissions
			if(!isNew){
				command="SELECT COUNT(*) FROM grouppermission "
					+"WHERE PermType='"+POut.Long((int)Permissions.SecurityAdmin)+"' "
					+"AND UserGroupNum="+POut.Long(user.UserGroupNum);
				if(Db.GetCount(command)=="0"){//if this user would not have admin
					//make sure someone else has admin
					command="SELECT COUNT(*) FROM userod,grouppermission "
						+"WHERE grouppermission.PermType='"+POut.Long((int)Permissions.SecurityAdmin)+"'"
						+" AND userod.UserGroupNum=grouppermission.UserGroupNum"
						+" AND userod.IsHidden =0"
						+" AND userod.UserNum != "+POut.Long(user.UserNum);
					if(Db.GetCount(command)=="0"){//there are no other users with this permission
						throw new Exception(Lans.g("Users","At least one user must have Security Admin permission."));
					}
				}
			}
			//an admin user can never be hidden
			command="SELECT COUNT(*) FROM grouppermission "
				+"WHERE PermType='"+POut.Long((int)Permissions.SecurityAdmin)+"' "
				+"AND UserGroupNum="+POut.Long(user.UserGroupNum);
			if(Db.GetCount(command)!="0"//if this user is admin
				&& user.IsHidden //and hidden
				&& user.UserNumCEMT==0) //and non-CEMT
			{
				throw new Exception(Lans.g("Userods","Admins cannot be hidden."));
			}
		}

		public static bool IsUserNameUnique(string username,long excludeUserNum,bool excludeHiddenUsers) {
			return IsUserNameUnique(username,excludeUserNum,excludeHiddenUsers,false);
		}

		///<summary>Supply 0 or -1 for the excludeUserNum to not exclude any.</summary>
		public static bool IsUserNameUnique(string username,long excludeUserNum,bool excludeHiddenUsers,bool searchCEMTUsers) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),username,excludeUserNum,excludeHiddenUsers);
			}
			if(username==""){
				return false;
			}
			string command="SELECT COUNT(*) FROM userod WHERE ";
			//if(Programs.UsingEcwTight()){
			//	command+="BINARY ";//allows different usernames based on capitalization.//we no longer allow this
				//Does not need to be tested under Oracle because eCW users do not use Oracle.
			//}
			command+="UserName='"+POut.String(username)+"' "
				+"AND UserNum !="+POut.Long(excludeUserNum)+" ";
			if(excludeHiddenUsers) {
				command+="AND IsHidden=0 ";//not hidden
			}
			if(searchCEMTUsers) {
				command+="AND UserNumCEMT!=0";
			}
			DataTable table=Db.GetTable(command);
			if(table.Rows[0][0].ToString()=="0") {
				return true;
			}
			return false;
		}

		///<summary>Used in FormSecurity.FillTreeUsers</summary>
		public static List<Userod> GetForGroup(long userGroupNum) {
			//No need to check RemotingRole; no call to db.
			//ArrayList al=new ArrayList();
			List<Userod> retVal=new List<Userod>();
			List<Userod> listUserods=UserodC.GetListt();
			for(int i=0;i<listUserods.Count;i++){
				if(listUserods[i].UserGroupNum==userGroupNum){
					retVal.Add(listUserods[i]);
				}
			}
			//User[] retVal=new User[al.Count];
			//al.CopyTo(retVal);
			return retVal;
		}

		///<summary>This always returns one admin user.  There must be one and there is rarely more than one.  Only used on startup to determine if security is being used.</summary>
		public static Userod GetAdminUser() {
			//No need to check RemotingRole; no call to db.
			//just find any permission for security admin.  There has to be one.
			for(int i=0;i<GroupPermissionC.List.Length;i++) {
				if(GroupPermissionC.List[i].PermType!=Permissions.SecurityAdmin) {
					continue;
				}
				List<Userod> listUserods=UserodC.GetListt();
				for(int j=0;j<listUserods.Count;j++) {
					if(listUserods[j].UserGroupNum==GroupPermissionC.List[i].UserGroupNum) {
						return listUserods[j];
					}
				}
			}
			return null;//will never happen
		}

		/// <summary>Will return 0 if no inbox found for user.</summary>
		public static long GetInbox(long userNum) {
			//No need to check RemotingRole; no call to db.
			List<Userod> listUserods=UserodC.GetListt();
			for(int i=0;i<listUserods.Count;i++) {
				if(listUserods[i].UserNum==userNum){
					return listUserods[i].TaskListInBox;
				}
			}
			return 0;
		}

		///<summary></summary>
		public static List<Userod> GetNotHidden(){
			//No need to check RemotingRole; no call to db.
			return UserodC.GetListShort();
		}

		//Return 3, which is non-admin provider type
		public static long GetAnesthProvType(long anesthProvType) {
			//No need to check RemotingRole; no call to db.
			List<Userod> listUserods=UserodC.GetListt();
			for(int i = 0;i < listUserods.Count;i++) {
				if(listUserods[i].AnesthProvType == anesthProvType) {
					return listUserods[i].AnesthProvType;
				}
			}
			return 3;
		}

		public static List<Userod> GetUsersForJobs() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Userod>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM userod "
				+"INNER JOIN jobrole ON userod.UserNum=jobrole.UserNum "
				+"WHERE IsHidden=0 GROUP BY userod.UserNum ORDER BY UserName";
			return Crud.UserodCrud.SelectMany(command);
		}

		///<summary>Returns empty string if password is strong enough.  Otherwise, returns explanation of why it's not strong enough.</summary>
		public static string IsPasswordStrong(string pass) {
			//No need to check RemotingRole; no call to db.
			if(pass=="") {
				return Lans.g("FormUserPassword","Password may not be blank when the strong password feature is turned on.");
			}
			if(pass.Length<8) {
				return Lans.g("FormUserPassword","Password must be at least eight characters long when the strong password feature is turned on.");
			}
			bool containsCap=false;
			for(int i=0;i<pass.Length;i++) {
				if(Char.IsUpper(pass[i])) {
					containsCap=true;
				}
			}
			if(!containsCap) {
				return Lans.g("FormUserPassword","Password must contain at least one capital letter when the strong password feature is turned on.");
			}
			bool containsLower=false;
			for(int i=0;i<pass.Length;i++) {
				if(Char.IsLower(pass[i])) {
					containsLower=true;
				}
			}
			if(!containsLower) {
				return Lans.g("FormUserPassword","Password must contain at least one lower case letter when the strong password feature is turned on.");
			}
			/*
			bool containsPunct=false;
			for(int i=0;i<pass.Length;i++) {
				if(!Char.IsLetterOrDigit(pass[i])) {
					containsPunct=true;
				}
			}
			if(!containsPunct) {
				return Lans.g("FormUserPassword","Password must contain at least one punctuation or symbol character when the strong password feature is turned on.");
			}*/
			bool containsNum=false;
			for(int i=0;i<pass.Length;i++) {
				if(Char.IsNumber(pass[i])) {
					containsNum=true;
				}
			}
			if(!containsNum) {
				return Lans.g("FormUserPassword","Password must contain at least one number when the strong password feature is turned on.");
			}
			return "";
		}

		///<summary>This resets the strong password flag on all users after an admin turns off pref PasswordsMustBeStrong.  If strong passwords are again turned on later, then each user will have to edit their password in order set the strong password flag again.</summary>
		public static void ResetStrongPasswordFlags() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod());
				return;
			}
			string command="UPDATE userod SET PasswordIsStrong=0";
			Db.NonQ(command);
		}



	}
}
