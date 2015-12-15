//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class CarrierCrud {
		///<summary>Gets one Carrier object from the database using the primary key.  Returns null if not found.</summary>
		public static Carrier SelectOne(long carrierNum){
			string command="SELECT * FROM carrier "
				+"WHERE CarrierNum = "+POut.Long(carrierNum);
			List<Carrier> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Carrier object from the database using a query.</summary>
		public static Carrier SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Carrier> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Carrier objects from the database using a query.</summary>
		public static List<Carrier> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Carrier> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Carrier> TableToList(DataTable table){
			List<Carrier> retVal=new List<Carrier>();
			Carrier carrier;
			foreach(DataRow row in table.Rows) {
				carrier=new Carrier();
				carrier.CarrierNum              = PIn.Long  (row["CarrierNum"].ToString());
				carrier.CarrierName             = PIn.String(row["CarrierName"].ToString());
				carrier.Address                 = PIn.String(row["Address"].ToString());
				carrier.Address2                = PIn.String(row["Address2"].ToString());
				carrier.City                    = PIn.String(row["City"].ToString());
				carrier.State                   = PIn.String(row["State"].ToString());
				carrier.Zip                     = PIn.String(row["Zip"].ToString());
				carrier.Phone                   = PIn.String(row["Phone"].ToString());
				carrier.ElectID                 = PIn.String(row["ElectID"].ToString());
				carrier.NoSendElect             = PIn.Bool  (row["NoSendElect"].ToString());
				carrier.IsCDA                   = PIn.Bool  (row["IsCDA"].ToString());
				carrier.CDAnetVersion           = PIn.String(row["CDAnetVersion"].ToString());
				carrier.CanadianNetworkNum      = PIn.Long  (row["CanadianNetworkNum"].ToString());
				carrier.IsHidden                = PIn.Bool  (row["IsHidden"].ToString());
				carrier.CanadianEncryptionMethod= PIn.Byte  (row["CanadianEncryptionMethod"].ToString());
				carrier.CanadianSupportedTypes  = (OpenDentBusiness.CanSupTransTypes)PIn.Int(row["CanadianSupportedTypes"].ToString());
				retVal.Add(carrier);
			}
			return retVal;
		}

		///<summary>Converts a list of EServiceFeatures into a DataTable.</summary>
		public static DataTable ListToTable(List<Carrier> listCarriers) {
			DataTable table=new DataTable("Carriers");
			table.Columns.Add("CarrierNum");
			table.Columns.Add("CarrierName");
			table.Columns.Add("Address");
			table.Columns.Add("Address2");
			table.Columns.Add("City");
			table.Columns.Add("State");
			table.Columns.Add("Zip");
			table.Columns.Add("Phone");
			table.Columns.Add("ElectID");
			table.Columns.Add("NoSendElect");
			table.Columns.Add("IsCDA");
			table.Columns.Add("CDAnetVersion");
			table.Columns.Add("CanadianNetworkNum");
			table.Columns.Add("IsHidden");
			table.Columns.Add("CanadianEncryptionMethod");
			table.Columns.Add("CanadianSupportedTypes");
			foreach(Carrier carrier in listCarriers) {
				table.Rows.Add(new object[] {
					POut.Long  (carrier.CarrierNum),
					POut.String(carrier.CarrierName),
					POut.String(carrier.Address),
					POut.String(carrier.Address2),
					POut.String(carrier.City),
					POut.String(carrier.State),
					POut.String(carrier.Zip),
					POut.String(carrier.Phone),
					POut.String(carrier.ElectID),
					POut.Bool  (carrier.NoSendElect),
					POut.Bool  (carrier.IsCDA),
					POut.String(carrier.CDAnetVersion),
					POut.Long  (carrier.CanadianNetworkNum),
					POut.Bool  (carrier.IsHidden),
					POut.Byte  (carrier.CanadianEncryptionMethod),
					POut.Int   ((int)carrier.CanadianSupportedTypes),
				});
			}
			return table;
		}

		///<summary>Inserts one Carrier into the database.  Returns the new priKey.</summary>
		public static long Insert(Carrier carrier){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				carrier.CarrierNum=DbHelper.GetNextOracleKey("carrier","CarrierNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(carrier,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							carrier.CarrierNum++;
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
				return Insert(carrier,false);
			}
		}

		///<summary>Inserts one Carrier into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Carrier carrier,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				carrier.CarrierNum=ReplicationServers.GetKey("carrier","CarrierNum");
			}
			string command="INSERT INTO carrier (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="CarrierNum,";
			}
			command+="CarrierName,Address,Address2,City,State,Zip,Phone,ElectID,NoSendElect,IsCDA,CDAnetVersion,CanadianNetworkNum,IsHidden,CanadianEncryptionMethod,CanadianSupportedTypes) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(carrier.CarrierNum)+",";
			}
			command+=
				 "'"+POut.String(carrier.CarrierName)+"',"
				+"'"+POut.String(carrier.Address)+"',"
				+"'"+POut.String(carrier.Address2)+"',"
				+"'"+POut.String(carrier.City)+"',"
				+"'"+POut.String(carrier.State)+"',"
				+"'"+POut.String(carrier.Zip)+"',"
				+"'"+POut.String(carrier.Phone)+"',"
				+"'"+POut.String(carrier.ElectID)+"',"
				+    POut.Bool  (carrier.NoSendElect)+","
				+    POut.Bool  (carrier.IsCDA)+","
				+"'"+POut.String(carrier.CDAnetVersion)+"',"
				+    POut.Long  (carrier.CanadianNetworkNum)+","
				+    POut.Bool  (carrier.IsHidden)+","
				+    POut.Byte  (carrier.CanadianEncryptionMethod)+","
				+    POut.Int   ((int)carrier.CanadianSupportedTypes)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				carrier.CarrierNum=Db.NonQ(command,true);
			}
			return carrier.CarrierNum;
		}

		///<summary>Inserts one Carrier into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Carrier carrier){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(carrier,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					carrier.CarrierNum=DbHelper.GetNextOracleKey("carrier","CarrierNum"); //Cacheless method
				}
				return InsertNoCache(carrier,true);
			}
		}

		///<summary>Inserts one Carrier into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Carrier carrier,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO carrier (";
			if(!useExistingPK && isRandomKeys) {
				carrier.CarrierNum=ReplicationServers.GetKeyNoCache("carrier","CarrierNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="CarrierNum,";
			}
			command+="CarrierName,Address,Address2,City,State,Zip,Phone,ElectID,NoSendElect,IsCDA,CDAnetVersion,CanadianNetworkNum,IsHidden,CanadianEncryptionMethod,CanadianSupportedTypes) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(carrier.CarrierNum)+",";
			}
			command+=
				 "'"+POut.String(carrier.CarrierName)+"',"
				+"'"+POut.String(carrier.Address)+"',"
				+"'"+POut.String(carrier.Address2)+"',"
				+"'"+POut.String(carrier.City)+"',"
				+"'"+POut.String(carrier.State)+"',"
				+"'"+POut.String(carrier.Zip)+"',"
				+"'"+POut.String(carrier.Phone)+"',"
				+"'"+POut.String(carrier.ElectID)+"',"
				+    POut.Bool  (carrier.NoSendElect)+","
				+    POut.Bool  (carrier.IsCDA)+","
				+"'"+POut.String(carrier.CDAnetVersion)+"',"
				+    POut.Long  (carrier.CanadianNetworkNum)+","
				+    POut.Bool  (carrier.IsHidden)+","
				+    POut.Byte  (carrier.CanadianEncryptionMethod)+","
				+    POut.Int   ((int)carrier.CanadianSupportedTypes)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				carrier.CarrierNum=Db.NonQ(command,true);
			}
			return carrier.CarrierNum;
		}

		///<summary>Updates one Carrier in the database.</summary>
		public static void Update(Carrier carrier){
			string command="UPDATE carrier SET "
				+"CarrierName             = '"+POut.String(carrier.CarrierName)+"', "
				+"Address                 = '"+POut.String(carrier.Address)+"', "
				+"Address2                = '"+POut.String(carrier.Address2)+"', "
				+"City                    = '"+POut.String(carrier.City)+"', "
				+"State                   = '"+POut.String(carrier.State)+"', "
				+"Zip                     = '"+POut.String(carrier.Zip)+"', "
				+"Phone                   = '"+POut.String(carrier.Phone)+"', "
				+"ElectID                 = '"+POut.String(carrier.ElectID)+"', "
				+"NoSendElect             =  "+POut.Bool  (carrier.NoSendElect)+", "
				+"IsCDA                   =  "+POut.Bool  (carrier.IsCDA)+", "
				+"CDAnetVersion           = '"+POut.String(carrier.CDAnetVersion)+"', "
				+"CanadianNetworkNum      =  "+POut.Long  (carrier.CanadianNetworkNum)+", "
				+"IsHidden                =  "+POut.Bool  (carrier.IsHidden)+", "
				+"CanadianEncryptionMethod=  "+POut.Byte  (carrier.CanadianEncryptionMethod)+", "
				+"CanadianSupportedTypes  =  "+POut.Int   ((int)carrier.CanadianSupportedTypes)+" "
				+"WHERE CarrierNum = "+POut.Long(carrier.CarrierNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Carrier in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Carrier carrier,Carrier oldCarrier){
			string command="";
			if(carrier.CarrierName != oldCarrier.CarrierName) {
				if(command!=""){ command+=",";}
				command+="CarrierName = '"+POut.String(carrier.CarrierName)+"'";
			}
			if(carrier.Address != oldCarrier.Address) {
				if(command!=""){ command+=",";}
				command+="Address = '"+POut.String(carrier.Address)+"'";
			}
			if(carrier.Address2 != oldCarrier.Address2) {
				if(command!=""){ command+=",";}
				command+="Address2 = '"+POut.String(carrier.Address2)+"'";
			}
			if(carrier.City != oldCarrier.City) {
				if(command!=""){ command+=",";}
				command+="City = '"+POut.String(carrier.City)+"'";
			}
			if(carrier.State != oldCarrier.State) {
				if(command!=""){ command+=",";}
				command+="State = '"+POut.String(carrier.State)+"'";
			}
			if(carrier.Zip != oldCarrier.Zip) {
				if(command!=""){ command+=",";}
				command+="Zip = '"+POut.String(carrier.Zip)+"'";
			}
			if(carrier.Phone != oldCarrier.Phone) {
				if(command!=""){ command+=",";}
				command+="Phone = '"+POut.String(carrier.Phone)+"'";
			}
			if(carrier.ElectID != oldCarrier.ElectID) {
				if(command!=""){ command+=",";}
				command+="ElectID = '"+POut.String(carrier.ElectID)+"'";
			}
			if(carrier.NoSendElect != oldCarrier.NoSendElect) {
				if(command!=""){ command+=",";}
				command+="NoSendElect = "+POut.Bool(carrier.NoSendElect)+"";
			}
			if(carrier.IsCDA != oldCarrier.IsCDA) {
				if(command!=""){ command+=",";}
				command+="IsCDA = "+POut.Bool(carrier.IsCDA)+"";
			}
			if(carrier.CDAnetVersion != oldCarrier.CDAnetVersion) {
				if(command!=""){ command+=",";}
				command+="CDAnetVersion = '"+POut.String(carrier.CDAnetVersion)+"'";
			}
			if(carrier.CanadianNetworkNum != oldCarrier.CanadianNetworkNum) {
				if(command!=""){ command+=",";}
				command+="CanadianNetworkNum = "+POut.Long(carrier.CanadianNetworkNum)+"";
			}
			if(carrier.IsHidden != oldCarrier.IsHidden) {
				if(command!=""){ command+=",";}
				command+="IsHidden = "+POut.Bool(carrier.IsHidden)+"";
			}
			if(carrier.CanadianEncryptionMethod != oldCarrier.CanadianEncryptionMethod) {
				if(command!=""){ command+=",";}
				command+="CanadianEncryptionMethod = "+POut.Byte(carrier.CanadianEncryptionMethod)+"";
			}
			if(carrier.CanadianSupportedTypes != oldCarrier.CanadianSupportedTypes) {
				if(command!=""){ command+=",";}
				command+="CanadianSupportedTypes = "+POut.Int   ((int)carrier.CanadianSupportedTypes)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE carrier SET "+command
				+" WHERE CarrierNum = "+POut.Long(carrier.CarrierNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Deletes one Carrier from the database.</summary>
		public static void Delete(long carrierNum){
			string command="DELETE FROM carrier "
				+"WHERE CarrierNum = "+POut.Long(carrierNum);
			Db.NonQ(command);
		}

	}
}