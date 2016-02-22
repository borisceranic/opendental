using CodeBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDentBusiness {
	public partial class ConvertDatabases {
		public static System.Version LatestVersion=new Version("16.2.0.0");//This value must be changed when a new conversion is to be triggered.

		private static void To16_2_0() {
			if(FromVersion<new Version("16.2.0.0")) {
				ODEvent.Fire(new ODEventArgs("ConvertDatabases","Upgrading database to version: 16.2.0"));//No translation in convert script.
				string command;
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="INSERT INTO preference (PrefName,ValueString) VALUES('PatientAllSuperFamilySync','0')";
					Db.NonQ(command);
				}
				else {//oracle
					command="INSERT INTO preference (PrefNum,PrefName,ValueString) VALUES((SELECT MAX(PrefNum)+1 FROM preference),'PatientAllSuperFamilySync','0')";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE hl7def ADD IsProcApptEnforced tinyint NOT NULL";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE hl7def ADD IsProcApptEnforced number(3)";
					Db.NonQ(command);
					command="UPDATE hl7def SET IsProcApptEnforced = 0 WHERE IsProcApptEnforced IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE hl7def MODIFY IsProcApptEnforced NOT NULL";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="DROP TABLE IF EXISTS hl7procattach";
					Db.NonQ(command);
					command=@"CREATE TABLE hl7procattach (
						HL7ProcAttachNum bigint NOT NULL auto_increment PRIMARY KEY,
						HL7MsgNum bigint NOT NULL,
						ProcNum bigint NOT NULL,
						INDEX(HL7MsgNum),
						INDEX(ProcNum)
						) DEFAULT CHARSET=utf8";
					Db.NonQ(command);
				}
				else {//oracle
					command="BEGIN EXECUTE IMMEDIATE 'DROP TABLE hl7procattach'; EXCEPTION WHEN OTHERS THEN NULL; END;";
					Db.NonQ(command);
					command=@"CREATE TABLE hl7procattach (
						HL7ProcAttachNum number(20) NOT NULL,
						HL7MsgNum number(20) NOT NULL,
						ProcNum number(20) NOT NULL,
						CONSTRAINT hl7procattach_HL7ProcAttachNum PRIMARY KEY (HL7ProcAttachNum)
						)";
					Db.NonQ(command);
					command=@"CREATE INDEX hl7procattach_HL7MsgNum ON hl7procattach (HL7MsgNum)";
					Db.NonQ(command);
					command=@"CREATE INDEX hl7procattach_ProcNum ON hl7procattach (ProcNum)";
					Db.NonQ(command);
				}



				command="UPDATE preference SET ValueString = '16.2.0.0' WHERE PrefName = 'DataBaseVersion'";
				Db.NonQ(command);
			}
			//To16_2_1();
		}

	}
}
