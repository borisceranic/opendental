//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ScreenCrud {
		///<summary>Gets one Screen object from the database using the primary key.  Returns null if not found.</summary>
		public static Screen SelectOne(long screenNum){
			string command="SELECT * FROM screen "
				+"WHERE ScreenNum = "+POut.Long(screenNum);
			List<Screen> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Screen object from the database using a query.</summary>
		public static Screen SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Screen> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Screen objects from the database using a query.</summary>
		public static List<Screen> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Screen> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Screen> TableToList(DataTable table){
			List<Screen> retVal=new List<Screen>();
			Screen screen;
			foreach(DataRow row in table.Rows) {
				screen=new Screen();
				screen.ScreenNum       = PIn.Long  (row["ScreenNum"].ToString());
				screen.Gender          = (OpenDentBusiness.PatientGender)PIn.Int(row["Gender"].ToString());
				screen.RaceOld         = (OpenDentBusiness.PatientRaceOld)PIn.Int(row["RaceOld"].ToString());
				screen.GradeLevel      = (OpenDentBusiness.PatientGrade)PIn.Int(row["GradeLevel"].ToString());
				screen.Age             = PIn.Byte  (row["Age"].ToString());
				screen.Urgency         = (OpenDentBusiness.TreatmentUrgency)PIn.Int(row["Urgency"].ToString());
				screen.HasCaries       = (OpenDentBusiness.YN)PIn.Int(row["HasCaries"].ToString());
				screen.NeedsSealants   = (OpenDentBusiness.YN)PIn.Int(row["NeedsSealants"].ToString());
				screen.CariesExperience= (OpenDentBusiness.YN)PIn.Int(row["CariesExperience"].ToString());
				screen.EarlyChildCaries= (OpenDentBusiness.YN)PIn.Int(row["EarlyChildCaries"].ToString());
				screen.ExistingSealants= (OpenDentBusiness.YN)PIn.Int(row["ExistingSealants"].ToString());
				screen.MissingAllTeeth = (OpenDentBusiness.YN)PIn.Int(row["MissingAllTeeth"].ToString());
				screen.Birthdate       = PIn.Date  (row["Birthdate"].ToString());
				screen.ScreenGroupNum  = PIn.Long  (row["ScreenGroupNum"].ToString());
				screen.ScreenGroupOrder= PIn.Int   (row["ScreenGroupOrder"].ToString());
				screen.Comments        = PIn.String(row["Comments"].ToString());
				screen.ScreenPatNum    = PIn.Long  (row["ScreenPatNum"].ToString());
				screen.SheetNum        = PIn.Long  (row["SheetNum"].ToString());
				retVal.Add(screen);
			}
			return retVal;
		}

		///<summary>Converts a list of Screen into a DataTable.</summary>
		public static DataTable ListToTable(List<Screen> listScreens,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="Screen";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("ScreenNum");
			table.Columns.Add("Gender");
			table.Columns.Add("RaceOld");
			table.Columns.Add("GradeLevel");
			table.Columns.Add("Age");
			table.Columns.Add("Urgency");
			table.Columns.Add("HasCaries");
			table.Columns.Add("NeedsSealants");
			table.Columns.Add("CariesExperience");
			table.Columns.Add("EarlyChildCaries");
			table.Columns.Add("ExistingSealants");
			table.Columns.Add("MissingAllTeeth");
			table.Columns.Add("Birthdate");
			table.Columns.Add("ScreenGroupNum");
			table.Columns.Add("ScreenGroupOrder");
			table.Columns.Add("Comments");
			table.Columns.Add("ScreenPatNum");
			table.Columns.Add("SheetNum");
			foreach(Screen screen in listScreens) {
				table.Rows.Add(new object[] {
					POut.Long  (screen.ScreenNum),
					POut.Int   ((int)screen.Gender),
					POut.Int   ((int)screen.RaceOld),
					POut.Int   ((int)screen.GradeLevel),
					POut.Byte  (screen.Age),
					POut.Int   ((int)screen.Urgency),
					POut.Int   ((int)screen.HasCaries),
					POut.Int   ((int)screen.NeedsSealants),
					POut.Int   ((int)screen.CariesExperience),
					POut.Int   ((int)screen.EarlyChildCaries),
					POut.Int   ((int)screen.ExistingSealants),
					POut.Int   ((int)screen.MissingAllTeeth),
					POut.DateT (screen.Birthdate,false),
					POut.Long  (screen.ScreenGroupNum),
					POut.Int   (screen.ScreenGroupOrder),
					            screen.Comments,
					POut.Long  (screen.ScreenPatNum),
					POut.Long  (screen.SheetNum),
				});
			}
			return table;
		}

		///<summary>Inserts one Screen into the database.  Returns the new priKey.</summary>
		public static long Insert(Screen screen){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				screen.ScreenNum=DbHelper.GetNextOracleKey("screen","ScreenNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(screen,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							screen.ScreenNum++;
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
				return Insert(screen,false);
			}
		}

		///<summary>Inserts one Screen into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Screen screen,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				screen.ScreenNum=ReplicationServers.GetKey("screen","ScreenNum");
			}
			string command="INSERT INTO screen (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ScreenNum,";
			}
			command+="Gender,RaceOld,GradeLevel,Age,Urgency,HasCaries,NeedsSealants,CariesExperience,EarlyChildCaries,ExistingSealants,MissingAllTeeth,Birthdate,ScreenGroupNum,ScreenGroupOrder,Comments,ScreenPatNum,SheetNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(screen.ScreenNum)+",";
			}
			command+=
				     POut.Int   ((int)screen.Gender)+","
				+    POut.Int   ((int)screen.RaceOld)+","
				+    POut.Int   ((int)screen.GradeLevel)+","
				+    POut.Byte  (screen.Age)+","
				+    POut.Int   ((int)screen.Urgency)+","
				+    POut.Int   ((int)screen.HasCaries)+","
				+    POut.Int   ((int)screen.NeedsSealants)+","
				+    POut.Int   ((int)screen.CariesExperience)+","
				+    POut.Int   ((int)screen.EarlyChildCaries)+","
				+    POut.Int   ((int)screen.ExistingSealants)+","
				+    POut.Int   ((int)screen.MissingAllTeeth)+","
				+    POut.Date  (screen.Birthdate)+","
				+    POut.Long  (screen.ScreenGroupNum)+","
				+    POut.Int   (screen.ScreenGroupOrder)+","
				+"'"+POut.String(screen.Comments)+"',"
				+    POut.Long  (screen.ScreenPatNum)+","
				+    POut.Long  (screen.SheetNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				screen.ScreenNum=Db.NonQ(command,true);
			}
			return screen.ScreenNum;
		}

		///<summary>Inserts one Screen into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Screen screen){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(screen,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					screen.ScreenNum=DbHelper.GetNextOracleKey("screen","ScreenNum"); //Cacheless method
				}
				return InsertNoCache(screen,true);
			}
		}

		///<summary>Inserts one Screen into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Screen screen,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO screen (";
			if(!useExistingPK && isRandomKeys) {
				screen.ScreenNum=ReplicationServers.GetKeyNoCache("screen","ScreenNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ScreenNum,";
			}
			command+="Gender,RaceOld,GradeLevel,Age,Urgency,HasCaries,NeedsSealants,CariesExperience,EarlyChildCaries,ExistingSealants,MissingAllTeeth,Birthdate,ScreenGroupNum,ScreenGroupOrder,Comments,ScreenPatNum,SheetNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(screen.ScreenNum)+",";
			}
			command+=
				     POut.Int   ((int)screen.Gender)+","
				+    POut.Int   ((int)screen.RaceOld)+","
				+    POut.Int   ((int)screen.GradeLevel)+","
				+    POut.Byte  (screen.Age)+","
				+    POut.Int   ((int)screen.Urgency)+","
				+    POut.Int   ((int)screen.HasCaries)+","
				+    POut.Int   ((int)screen.NeedsSealants)+","
				+    POut.Int   ((int)screen.CariesExperience)+","
				+    POut.Int   ((int)screen.EarlyChildCaries)+","
				+    POut.Int   ((int)screen.ExistingSealants)+","
				+    POut.Int   ((int)screen.MissingAllTeeth)+","
				+    POut.Date  (screen.Birthdate)+","
				+    POut.Long  (screen.ScreenGroupNum)+","
				+    POut.Int   (screen.ScreenGroupOrder)+","
				+"'"+POut.String(screen.Comments)+"',"
				+    POut.Long  (screen.ScreenPatNum)+","
				+    POut.Long  (screen.SheetNum)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				screen.ScreenNum=Db.NonQ(command,true);
			}
			return screen.ScreenNum;
		}

		///<summary>Updates one Screen in the database.</summary>
		public static void Update(Screen screen){
			string command="UPDATE screen SET "
				+"Gender          =  "+POut.Int   ((int)screen.Gender)+", "
				+"RaceOld         =  "+POut.Int   ((int)screen.RaceOld)+", "
				+"GradeLevel      =  "+POut.Int   ((int)screen.GradeLevel)+", "
				+"Age             =  "+POut.Byte  (screen.Age)+", "
				+"Urgency         =  "+POut.Int   ((int)screen.Urgency)+", "
				+"HasCaries       =  "+POut.Int   ((int)screen.HasCaries)+", "
				+"NeedsSealants   =  "+POut.Int   ((int)screen.NeedsSealants)+", "
				+"CariesExperience=  "+POut.Int   ((int)screen.CariesExperience)+", "
				+"EarlyChildCaries=  "+POut.Int   ((int)screen.EarlyChildCaries)+", "
				+"ExistingSealants=  "+POut.Int   ((int)screen.ExistingSealants)+", "
				+"MissingAllTeeth =  "+POut.Int   ((int)screen.MissingAllTeeth)+", "
				+"Birthdate       =  "+POut.Date  (screen.Birthdate)+", "
				+"ScreenGroupNum  =  "+POut.Long  (screen.ScreenGroupNum)+", "
				+"ScreenGroupOrder=  "+POut.Int   (screen.ScreenGroupOrder)+", "
				+"Comments        = '"+POut.String(screen.Comments)+"', "
				+"ScreenPatNum    =  "+POut.Long  (screen.ScreenPatNum)+", "
				+"SheetNum        =  "+POut.Long  (screen.SheetNum)+" "
				+"WHERE ScreenNum = "+POut.Long(screen.ScreenNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Screen in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Screen screen,Screen oldScreen){
			string command="";
			if(screen.Gender != oldScreen.Gender) {
				if(command!=""){ command+=",";}
				command+="Gender = "+POut.Int   ((int)screen.Gender)+"";
			}
			if(screen.RaceOld != oldScreen.RaceOld) {
				if(command!=""){ command+=",";}
				command+="RaceOld = "+POut.Int   ((int)screen.RaceOld)+"";
			}
			if(screen.GradeLevel != oldScreen.GradeLevel) {
				if(command!=""){ command+=",";}
				command+="GradeLevel = "+POut.Int   ((int)screen.GradeLevel)+"";
			}
			if(screen.Age != oldScreen.Age) {
				if(command!=""){ command+=",";}
				command+="Age = "+POut.Byte(screen.Age)+"";
			}
			if(screen.Urgency != oldScreen.Urgency) {
				if(command!=""){ command+=",";}
				command+="Urgency = "+POut.Int   ((int)screen.Urgency)+"";
			}
			if(screen.HasCaries != oldScreen.HasCaries) {
				if(command!=""){ command+=",";}
				command+="HasCaries = "+POut.Int   ((int)screen.HasCaries)+"";
			}
			if(screen.NeedsSealants != oldScreen.NeedsSealants) {
				if(command!=""){ command+=",";}
				command+="NeedsSealants = "+POut.Int   ((int)screen.NeedsSealants)+"";
			}
			if(screen.CariesExperience != oldScreen.CariesExperience) {
				if(command!=""){ command+=",";}
				command+="CariesExperience = "+POut.Int   ((int)screen.CariesExperience)+"";
			}
			if(screen.EarlyChildCaries != oldScreen.EarlyChildCaries) {
				if(command!=""){ command+=",";}
				command+="EarlyChildCaries = "+POut.Int   ((int)screen.EarlyChildCaries)+"";
			}
			if(screen.ExistingSealants != oldScreen.ExistingSealants) {
				if(command!=""){ command+=",";}
				command+="ExistingSealants = "+POut.Int   ((int)screen.ExistingSealants)+"";
			}
			if(screen.MissingAllTeeth != oldScreen.MissingAllTeeth) {
				if(command!=""){ command+=",";}
				command+="MissingAllTeeth = "+POut.Int   ((int)screen.MissingAllTeeth)+"";
			}
			if(screen.Birthdate.Date != oldScreen.Birthdate.Date) {
				if(command!=""){ command+=",";}
				command+="Birthdate = "+POut.Date(screen.Birthdate)+"";
			}
			if(screen.ScreenGroupNum != oldScreen.ScreenGroupNum) {
				if(command!=""){ command+=",";}
				command+="ScreenGroupNum = "+POut.Long(screen.ScreenGroupNum)+"";
			}
			if(screen.ScreenGroupOrder != oldScreen.ScreenGroupOrder) {
				if(command!=""){ command+=",";}
				command+="ScreenGroupOrder = "+POut.Int(screen.ScreenGroupOrder)+"";
			}
			if(screen.Comments != oldScreen.Comments) {
				if(command!=""){ command+=",";}
				command+="Comments = '"+POut.String(screen.Comments)+"'";
			}
			if(screen.ScreenPatNum != oldScreen.ScreenPatNum) {
				if(command!=""){ command+=",";}
				command+="ScreenPatNum = "+POut.Long(screen.ScreenPatNum)+"";
			}
			if(screen.SheetNum != oldScreen.SheetNum) {
				if(command!=""){ command+=",";}
				command+="SheetNum = "+POut.Long(screen.SheetNum)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE screen SET "+command
				+" WHERE ScreenNum = "+POut.Long(screen.ScreenNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(Screen,Screen) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(Screen screen,Screen oldScreen) {
			if(screen.Gender != oldScreen.Gender) {
				return true;
			}
			if(screen.RaceOld != oldScreen.RaceOld) {
				return true;
			}
			if(screen.GradeLevel != oldScreen.GradeLevel) {
				return true;
			}
			if(screen.Age != oldScreen.Age) {
				return true;
			}
			if(screen.Urgency != oldScreen.Urgency) {
				return true;
			}
			if(screen.HasCaries != oldScreen.HasCaries) {
				return true;
			}
			if(screen.NeedsSealants != oldScreen.NeedsSealants) {
				return true;
			}
			if(screen.CariesExperience != oldScreen.CariesExperience) {
				return true;
			}
			if(screen.EarlyChildCaries != oldScreen.EarlyChildCaries) {
				return true;
			}
			if(screen.ExistingSealants != oldScreen.ExistingSealants) {
				return true;
			}
			if(screen.MissingAllTeeth != oldScreen.MissingAllTeeth) {
				return true;
			}
			if(screen.Birthdate.Date != oldScreen.Birthdate.Date) {
				return true;
			}
			if(screen.ScreenGroupNum != oldScreen.ScreenGroupNum) {
				return true;
			}
			if(screen.ScreenGroupOrder != oldScreen.ScreenGroupOrder) {
				return true;
			}
			if(screen.Comments != oldScreen.Comments) {
				return true;
			}
			if(screen.ScreenPatNum != oldScreen.ScreenPatNum) {
				return true;
			}
			if(screen.SheetNum != oldScreen.SheetNum) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one Screen from the database.</summary>
		public static void Delete(long screenNum){
			string command="DELETE FROM screen "
				+"WHERE ScreenNum = "+POut.Long(screenNum);
			Db.NonQ(command);
		}

	}
}