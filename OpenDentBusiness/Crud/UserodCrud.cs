//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class UserodCrud {
		///<summary>Gets one Userod object from the database using the primary key.  Returns null if not found.</summary>
		public static Userod SelectOne(long userNum){
			string command="SELECT * FROM userod "
				+"WHERE UserNum = "+POut.Long(userNum);
			List<Userod> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Userod object from the database using a query.</summary>
		public static Userod SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Userod> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Userod objects from the database using a query.</summary>
		public static List<Userod> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Userod> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Userod> TableToList(DataTable table){
			List<Userod> retVal=new List<Userod>();
			Userod userod;
			foreach(DataRow row in table.Rows) {
				userod=new Userod();
				userod.UserNum           = PIn.Long  (row["UserNum"].ToString());
				userod.UserName          = PIn.String(row["UserName"].ToString());
				userod.Password          = PIn.String(row["Password"].ToString());
				userod.UserGroupNum      = PIn.Long  (row["UserGroupNum"].ToString());
				userod.EmployeeNum       = PIn.Long  (row["EmployeeNum"].ToString());
				userod.ClinicNum         = PIn.Long  (row["ClinicNum"].ToString());
				userod.ProvNum           = PIn.Long  (row["ProvNum"].ToString());
				userod.IsHidden          = PIn.Bool  (row["IsHidden"].ToString());
				userod.TaskListInBox     = PIn.Long  (row["TaskListInBox"].ToString());
				userod.AnesthProvType    = PIn.Int   (row["AnesthProvType"].ToString());
				userod.DefaultHidePopups = PIn.Bool  (row["DefaultHidePopups"].ToString());
				userod.PasswordIsStrong  = PIn.Bool  (row["PasswordIsStrong"].ToString());
				userod.ClinicIsRestricted= PIn.Bool  (row["ClinicIsRestricted"].ToString());
				userod.InboxHidePopups   = PIn.Bool  (row["InboxHidePopups"].ToString());
				userod.UserNumCEMT       = PIn.Long  (row["UserNumCEMT"].ToString());
				retVal.Add(userod);
			}
			return retVal;
		}

		///<summary>Converts a list of Userod into a DataTable.</summary>
		public static DataTable ListToTable(List<Userod> listUserods,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="Userod";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("UserNum");
			table.Columns.Add("UserName");
			table.Columns.Add("Password");
			table.Columns.Add("UserGroupNum");
			table.Columns.Add("EmployeeNum");
			table.Columns.Add("ClinicNum");
			table.Columns.Add("ProvNum");
			table.Columns.Add("IsHidden");
			table.Columns.Add("TaskListInBox");
			table.Columns.Add("AnesthProvType");
			table.Columns.Add("DefaultHidePopups");
			table.Columns.Add("PasswordIsStrong");
			table.Columns.Add("ClinicIsRestricted");
			table.Columns.Add("InboxHidePopups");
			table.Columns.Add("UserNumCEMT");
			foreach(Userod userod in listUserods) {
				table.Rows.Add(new object[] {
					POut.Long  (userod.UserNum),
					POut.String(userod.UserName),
					POut.String(userod.Password),
					POut.Long  (userod.UserGroupNum),
					POut.Long  (userod.EmployeeNum),
					POut.Long  (userod.ClinicNum),
					POut.Long  (userod.ProvNum),
					POut.Bool  (userod.IsHidden),
					POut.Long  (userod.TaskListInBox),
					POut.Int   (userod.AnesthProvType),
					POut.Bool  (userod.DefaultHidePopups),
					POut.Bool  (userod.PasswordIsStrong),
					POut.Bool  (userod.ClinicIsRestricted),
					POut.Bool  (userod.InboxHidePopups),
					POut.Long  (userod.UserNumCEMT),
				});
			}
			return table;
		}

		///<summary>Inserts one Userod into the database.  Returns the new priKey.</summary>
		public static long Insert(Userod userod){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				userod.UserNum=DbHelper.GetNextOracleKey("userod","UserNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(userod,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							userod.UserNum++;
							loopcount++;
						}
						else{
							throw ex;
						}
					}
				}
				throw new ApplicationException("Insert failed.  Could not generate primary key.");
			}
			else {
				return Insert(userod,false);
			}
		}

		///<summary>Inserts one Userod into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Userod userod,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				userod.UserNum=ReplicationServers.GetKey("userod","UserNum");
			}
			string command="INSERT INTO userod (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="UserNum,";
			}
			command+="UserName,Password,UserGroupNum,EmployeeNum,ClinicNum,ProvNum,IsHidden,TaskListInBox,AnesthProvType,DefaultHidePopups,PasswordIsStrong,ClinicIsRestricted,InboxHidePopups,UserNumCEMT) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(userod.UserNum)+",";
			}
			command+=
				 "'"+POut.String(userod.UserName)+"',"
				+"'"+POut.String(userod.Password)+"',"
				+    POut.Long  (userod.UserGroupNum)+","
				+    POut.Long  (userod.EmployeeNum)+","
				+    POut.Long  (userod.ClinicNum)+","
				+    POut.Long  (userod.ProvNum)+","
				+    POut.Bool  (userod.IsHidden)+","
				+    POut.Long  (userod.TaskListInBox)+","
				+    POut.Int   (userod.AnesthProvType)+","
				+    POut.Bool  (userod.DefaultHidePopups)+","
				+    POut.Bool  (userod.PasswordIsStrong)+","
				+    POut.Bool  (userod.ClinicIsRestricted)+","
				+    POut.Bool  (userod.InboxHidePopups)+","
				+    POut.Long  (userod.UserNumCEMT)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				userod.UserNum=Db.NonQ(command,true);
			}
			return userod.UserNum;
		}

		///<summary>Inserts one Userod into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Userod userod){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(userod,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					userod.UserNum=DbHelper.GetNextOracleKey("userod","UserNum"); //Cacheless method
				}
				return InsertNoCache(userod,true);
			}
		}

		///<summary>Inserts one Userod into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Userod userod,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO userod (";
			if(!useExistingPK && isRandomKeys) {
				userod.UserNum=ReplicationServers.GetKeyNoCache("userod","UserNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="UserNum,";
			}
			command+="UserName,Password,UserGroupNum,EmployeeNum,ClinicNum,ProvNum,IsHidden,TaskListInBox,AnesthProvType,DefaultHidePopups,PasswordIsStrong,ClinicIsRestricted,InboxHidePopups,UserNumCEMT) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(userod.UserNum)+",";
			}
			command+=
				 "'"+POut.String(userod.UserName)+"',"
				+"'"+POut.String(userod.Password)+"',"
				+    POut.Long  (userod.UserGroupNum)+","
				+    POut.Long  (userod.EmployeeNum)+","
				+    POut.Long  (userod.ClinicNum)+","
				+    POut.Long  (userod.ProvNum)+","
				+    POut.Bool  (userod.IsHidden)+","
				+    POut.Long  (userod.TaskListInBox)+","
				+    POut.Int   (userod.AnesthProvType)+","
				+    POut.Bool  (userod.DefaultHidePopups)+","
				+    POut.Bool  (userod.PasswordIsStrong)+","
				+    POut.Bool  (userod.ClinicIsRestricted)+","
				+    POut.Bool  (userod.InboxHidePopups)+","
				+    POut.Long  (userod.UserNumCEMT)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				userod.UserNum=Db.NonQ(command,true);
			}
			return userod.UserNum;
		}

		///<summary>Updates one Userod in the database.</summary>
		public static void Update(Userod userod){
			string command="UPDATE userod SET "
				+"UserName          = '"+POut.String(userod.UserName)+"', "
				+"Password          = '"+POut.String(userod.Password)+"', "
				+"UserGroupNum      =  "+POut.Long  (userod.UserGroupNum)+", "
				+"EmployeeNum       =  "+POut.Long  (userod.EmployeeNum)+", "
				+"ClinicNum         =  "+POut.Long  (userod.ClinicNum)+", "
				+"ProvNum           =  "+POut.Long  (userod.ProvNum)+", "
				+"IsHidden          =  "+POut.Bool  (userod.IsHidden)+", "
				+"TaskListInBox     =  "+POut.Long  (userod.TaskListInBox)+", "
				+"AnesthProvType    =  "+POut.Int   (userod.AnesthProvType)+", "
				+"DefaultHidePopups =  "+POut.Bool  (userod.DefaultHidePopups)+", "
				+"PasswordIsStrong  =  "+POut.Bool  (userod.PasswordIsStrong)+", "
				+"ClinicIsRestricted=  "+POut.Bool  (userod.ClinicIsRestricted)+", "
				+"InboxHidePopups   =  "+POut.Bool  (userod.InboxHidePopups)+", "
				+"UserNumCEMT       =  "+POut.Long  (userod.UserNumCEMT)+" "
				+"WHERE UserNum = "+POut.Long(userod.UserNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Userod in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Userod userod,Userod oldUserod){
			string command="";
			if(userod.UserName != oldUserod.UserName) {
				if(command!=""){ command+=",";}
				command+="UserName = '"+POut.String(userod.UserName)+"'";
			}
			if(userod.Password != oldUserod.Password) {
				if(command!=""){ command+=",";}
				command+="Password = '"+POut.String(userod.Password)+"'";
			}
			if(userod.UserGroupNum != oldUserod.UserGroupNum) {
				if(command!=""){ command+=",";}
				command+="UserGroupNum = "+POut.Long(userod.UserGroupNum)+"";
			}
			if(userod.EmployeeNum != oldUserod.EmployeeNum) {
				if(command!=""){ command+=",";}
				command+="EmployeeNum = "+POut.Long(userod.EmployeeNum)+"";
			}
			if(userod.ClinicNum != oldUserod.ClinicNum) {
				if(command!=""){ command+=",";}
				command+="ClinicNum = "+POut.Long(userod.ClinicNum)+"";
			}
			if(userod.ProvNum != oldUserod.ProvNum) {
				if(command!=""){ command+=",";}
				command+="ProvNum = "+POut.Long(userod.ProvNum)+"";
			}
			if(userod.IsHidden != oldUserod.IsHidden) {
				if(command!=""){ command+=",";}
				command+="IsHidden = "+POut.Bool(userod.IsHidden)+"";
			}
			if(userod.TaskListInBox != oldUserod.TaskListInBox) {
				if(command!=""){ command+=",";}
				command+="TaskListInBox = "+POut.Long(userod.TaskListInBox)+"";
			}
			if(userod.AnesthProvType != oldUserod.AnesthProvType) {
				if(command!=""){ command+=",";}
				command+="AnesthProvType = "+POut.Int(userod.AnesthProvType)+"";
			}
			if(userod.DefaultHidePopups != oldUserod.DefaultHidePopups) {
				if(command!=""){ command+=",";}
				command+="DefaultHidePopups = "+POut.Bool(userod.DefaultHidePopups)+"";
			}
			if(userod.PasswordIsStrong != oldUserod.PasswordIsStrong) {
				if(command!=""){ command+=",";}
				command+="PasswordIsStrong = "+POut.Bool(userod.PasswordIsStrong)+"";
			}
			if(userod.ClinicIsRestricted != oldUserod.ClinicIsRestricted) {
				if(command!=""){ command+=",";}
				command+="ClinicIsRestricted = "+POut.Bool(userod.ClinicIsRestricted)+"";
			}
			if(userod.InboxHidePopups != oldUserod.InboxHidePopups) {
				if(command!=""){ command+=",";}
				command+="InboxHidePopups = "+POut.Bool(userod.InboxHidePopups)+"";
			}
			if(userod.UserNumCEMT != oldUserod.UserNumCEMT) {
				if(command!=""){ command+=",";}
				command+="UserNumCEMT = "+POut.Long(userod.UserNumCEMT)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE userod SET "+command
				+" WHERE UserNum = "+POut.Long(userod.UserNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one Userod from the database.</summary>
		public static void Delete(long userNum){
			string command="DELETE FROM userod "
				+"WHERE UserNum = "+POut.Long(userNum);
			Db.NonQ(command);
		}

	}
}