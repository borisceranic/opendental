using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class PhoneConfs{

		///<summary></summary>
		public static List<PhoneConf> GetAll() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PhoneConf>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM phoneconf";
			try {
				return Crud.PhoneConfCrud.SelectMany(command);
			}
			catch {
				return new List<PhoneConf>();
			}
		}

		///<summary>Increments the Occupants column by one for the corresponding extension passed in.
		///A new row will be inserted if no row is found for the extension passed in.</summary>
		public static void AddOccupantForExtension(int extension) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),extension);
				return;
			}
			string command="UPDATE phoneconf SET Occupants = Occupants+1 WHERE Extension="+POut.Int(extension);
			Db.NonQ(command);
		}

		///<summary>Decrements the Occupants column by one for the corresponding extension passed in.</summary>
		public static void RemoveOccupantForExtension(int extension) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),extension);
				return;
			}
			string command="UPDATE phoneconf SET Occupants = Occupants-1 "
				+"WHERE Extension="+POut.Int(extension)+" "
				+"AND Occupants > 0";//Never go negative.
			Db.NonQ(command);
		}

		///<summary>Sets the Occupants column to zero for the corresponding extension passed in.</summary>
		public static void ClearOccupantsForExtension(int extension) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),extension);
				return;
			}
			string command="UPDATE phoneconf SET Occupants = 0 WHERE Extension="+POut.Int(extension);
			Db.NonQ(command);
		}

	}
}


/*
Only pull out the methods below as you need them.  Otherwise, leave them commented out.
		 
///<summary></summary>
public static long Insert(PhoneConf phoneConf){
	if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
		phoneConf.PhoneConfNum=Meth.GetLong(MethodBase.GetCurrentMethod(),phoneConf);
		return phoneConf.PhoneConfNum;
	}
	return Crud.PhoneConfCrud.Insert(phoneConf);
}

///<summary></summary>
public static void DeleteAll(long phoneConfNum) {
	if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
		Meth.GetVoid(MethodBase.GetCurrentMethod(),phoneConfNum);
		return;
	}
	string command= "DELETE FROM phoneconf WHERE PhoneConfNum = "+POut.Long(phoneConfNum);
	Db.NonQ(command);
}

///<summary></summary>
public static void Delete(long phoneConfNum) {
	if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
		Meth.GetVoid(MethodBase.GetCurrentMethod(),phoneConfNum);
		return;
	}
	string command= "DELETE FROM phoneconf WHERE PhoneConfNum = "+POut.Long(phoneConfNum);
	Db.NonQ(command);
}

///<summary></summary>
public static List<PhoneConf> Refresh(long patNum){
	if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
		return Meth.GetObject<List<PhoneConf>>(MethodBase.GetCurrentMethod(),patNum);
	}
	string command="SELECT * FROM phoneconf WHERE PatNum = "+POut.Long(patNum);
	return Crud.PhoneConfCrud.SelectMany(command);
}

///<summary>Gets one PhoneConf from the db.</summary>
public static PhoneConf GetOne(long phoneConfNum){
	if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
		return Meth.GetObject<PhoneConf>(MethodBase.GetCurrentMethod(),phoneConfNum);
	}
	return Crud.PhoneConfCrud.SelectOne(phoneConfNum);
}

///<summary></summary>
public static void Update(PhoneConf phoneConf){
	if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
		Meth.GetVoid(MethodBase.GetCurrentMethod(),phoneConf);
		return;
	}
	Crud.PhoneConfCrud.Update(phoneConf);
}
*/

/*Because this is a IsMissingInGeneral Table, this is the SQL to create the table.
	"DROP TABLE IF EXISTS phoneconf";
	"CREATE TABLE phoneconf (
		PhoneConfNum bigint NOT NULL auto_increment PRIMARY KEY,
		ButtonIndex int NOT NULL,
		Occupants int NOT NULL,
		Extension int NOT NULL
		) DEFAULT CHARSET=utf8"
 Initialize table data:
	TRUNCATE TABLE phoneconf;
	INSERT INTO phoneconf VALUES (),(),(),(),(),(),(),(),(),(),(),(),();
	UPDATE phoneconf SET buttonIndex=PhoneConfNum+6;
	UPDATE phoneconf SET occupants=FLOOR(RAND()*4);
	UPDATE phoneconf SET extension=phoneConfNum+700;
*/