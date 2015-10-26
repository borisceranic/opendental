//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class MountCrud {
		///<summary>Gets one Mount object from the database using the primary key.  Returns null if not found.</summary>
		public static Mount SelectOne(long mountNum){
			string command="SELECT * FROM mount "
				+"WHERE MountNum = "+POut.Long(mountNum);
			List<Mount> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Mount object from the database using a query.</summary>
		public static Mount SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Mount> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Mount objects from the database using a query.</summary>
		public static List<Mount> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Mount> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Mount> TableToList(DataTable table){
			List<Mount> retVal=new List<Mount>();
			Mount mount;
			foreach(DataRow row in table.Rows) {
				mount=new Mount();
				mount.MountNum   = PIn.Long  (row["MountNum"].ToString());
				mount.PatNum     = PIn.Long  (row["PatNum"].ToString());
				mount.DocCategory= PIn.Long  (row["DocCategory"].ToString());
				mount.DateCreated= PIn.Date  (row["DateCreated"].ToString());
				mount.Description= PIn.String(row["Description"].ToString());
				mount.Note       = PIn.String(row["Note"].ToString());
				mount.ImgType    = (OpenDentBusiness.ImageType)PIn.Int(row["ImgType"].ToString());
				mount.Width      = PIn.Int   (row["Width"].ToString());
				mount.Height     = PIn.Int   (row["Height"].ToString());
				retVal.Add(mount);
			}
			return retVal;
		}

		///<summary>Inserts one Mount into the database.  Returns the new priKey.</summary>
		public static long Insert(Mount mount){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				mount.MountNum=DbHelper.GetNextOracleKey("mount","MountNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(mount,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							mount.MountNum++;
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
				return Insert(mount,false);
			}
		}

		///<summary>Inserts one Mount into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Mount mount,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				mount.MountNum=ReplicationServers.GetKey("mount","MountNum");
			}
			string command="INSERT INTO mount (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="MountNum,";
			}
			command+="PatNum,DocCategory,DateCreated,Description,Note,ImgType,Width,Height) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(mount.MountNum)+",";
			}
			command+=
				     POut.Long  (mount.PatNum)+","
				+    POut.Long  (mount.DocCategory)+","
				+    POut.Date  (mount.DateCreated)+","
				+"'"+POut.String(mount.Description)+"',"
				+"'"+POut.String(mount.Note)+"',"
				+    POut.Int   ((int)mount.ImgType)+","
				+    POut.Int   (mount.Width)+","
				+    POut.Int   (mount.Height)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				mount.MountNum=Db.NonQ(command,true);
			}
			return mount.MountNum;
		}

		///<summary>Inserts one Mount into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Mount mount){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(mount,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					mount.MountNum=DbHelper.GetNextOracleKey("mount","MountNum"); //Cacheless method
				}
				return InsertNoCache(mount,true);
			}
		}

		///<summary>Inserts one Mount into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Mount mount,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO mount (";
			if(!useExistingPK && isRandomKeys) {
				mount.MountNum=ReplicationServers.GetKeyNoCache("mount","MountNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="MountNum,";
			}
			command+="PatNum,DocCategory,DateCreated,Description,Note,ImgType,Width,Height) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(mount.MountNum)+",";
			}
			command+=
				     POut.Long  (mount.PatNum)+","
				+    POut.Long  (mount.DocCategory)+","
				+    POut.Date  (mount.DateCreated)+","
				+"'"+POut.String(mount.Description)+"',"
				+"'"+POut.String(mount.Note)+"',"
				+    POut.Int   ((int)mount.ImgType)+","
				+    POut.Int   (mount.Width)+","
				+    POut.Int   (mount.Height)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				mount.MountNum=Db.NonQ(command,true);
			}
			return mount.MountNum;
		}

		///<summary>Updates one Mount in the database.</summary>
		public static void Update(Mount mount){
			string command="UPDATE mount SET "
				+"PatNum     =  "+POut.Long  (mount.PatNum)+", "
				+"DocCategory=  "+POut.Long  (mount.DocCategory)+", "
				+"DateCreated=  "+POut.Date  (mount.DateCreated)+", "
				+"Description= '"+POut.String(mount.Description)+"', "
				+"Note       = '"+POut.String(mount.Note)+"', "
				+"ImgType    =  "+POut.Int   ((int)mount.ImgType)+", "
				+"Width      =  "+POut.Int   (mount.Width)+", "
				+"Height     =  "+POut.Int   (mount.Height)+" "
				+"WHERE MountNum = "+POut.Long(mount.MountNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Mount in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Mount mount,Mount oldMount){
			string command="";
			if(mount.PatNum != oldMount.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(mount.PatNum)+"";
			}
			if(mount.DocCategory != oldMount.DocCategory) {
				if(command!=""){ command+=",";}
				command+="DocCategory = "+POut.Long(mount.DocCategory)+"";
			}
			if(mount.DateCreated != oldMount.DateCreated) {
				if(command!=""){ command+=",";}
				command+="DateCreated = "+POut.Date(mount.DateCreated)+"";
			}
			if(mount.Description != oldMount.Description) {
				if(command!=""){ command+=",";}
				command+="Description = '"+POut.String(mount.Description)+"'";
			}
			if(mount.Note != oldMount.Note) {
				if(command!=""){ command+=",";}
				command+="Note = '"+POut.String(mount.Note)+"'";
			}
			if(mount.ImgType != oldMount.ImgType) {
				if(command!=""){ command+=",";}
				command+="ImgType = "+POut.Int   ((int)mount.ImgType)+"";
			}
			if(mount.Width != oldMount.Width) {
				if(command!=""){ command+=",";}
				command+="Width = "+POut.Int(mount.Width)+"";
			}
			if(mount.Height != oldMount.Height) {
				if(command!=""){ command+=",";}
				command+="Height = "+POut.Int(mount.Height)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE mount SET "+command
				+" WHERE MountNum = "+POut.Long(mount.MountNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one Mount from the database.</summary>
		public static void Delete(long mountNum){
			string command="DELETE FROM mount "
				+"WHERE MountNum = "+POut.Long(mountNum);
			Db.NonQ(command);
		}

	}
}