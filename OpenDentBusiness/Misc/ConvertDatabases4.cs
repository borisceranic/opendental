﻿using CodeBase;
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
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE creditcard ADD CCSource tinyint NOT NULL";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE creditcard ADD CCSource number(3)";
					Db.NonQ(command);
					command="UPDATE creditcard SET CCSource = 0 WHERE CCSource IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE creditcard MODIFY CCSource NOT NULL";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="INSERT INTO preference (PrefName,ValueString) VALUES('TextingDefaultClinicNum','0')";
					Db.NonQ(command);
				}
				else {//oracle
					command="INSERT INTO preference (PrefNum,PrefName,ValueString) VALUES((SELECT MAX(PrefNum)+1 FROM preference),'TextingDefaultClinicNum','0')";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE sheet ADD IsDeleted tinyint NOT NULL";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE sheet ADD IsDeleted number(3)";
					Db.NonQ(command);
					command="UPDATE sheet SET IsDeleted = 0 WHERE IsDeleted IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE sheet MODIFY IsDeleted NOT NULL";
					Db.NonQ(command);
				}				
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE sheetfield ADD DateTimeSig datetime NOT NULL DEFAULT '0001-01-01 00:00:00'";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE sheetfield ADD DateTimeSig date";
					Db.NonQ(command);
					command="UPDATE sheetfield SET DateTimeSig = TO_DATE('0001-01-01','YYYY-MM-DD') WHERE DateTimeSig IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE sheetfield MODIFY DateTimeSig NOT NULL";
					Db.NonQ(command);
				}
				//Insert RapidCall bridge-----------------------------------------------------------------
				if(DataConnection.DBtype==DatabaseType.MySql) {
				command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+") VALUES("
					+"'RapidCall', "
					+"'Rapid Call from www.dentaltek.com', "
					+"'0', "
					+"'"+POut.String(@"C:\DentalTek\CallTray\CallTray.exe")+"', "
					+"'"+POut.String(@"/DeepLink=RapidCall")+"', "//leave blank if none
					+"'')";
				long programNum=Db.NonQ(command,true);
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
					+") VALUES("
					+"'"+POut.Long(programNum)+"', "
					+"'Disable Advertising', "
					+"'0')";
				Db.NonQ(command);
				}
				else {//oracle
				command="INSERT INTO program (ProgramNum,ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+") VALUES("
					+"(SELECT MAX(ProgramNum)+1 FROM program),"
					+"'RapidCall', "
					+"'Rapid Call from www.dentaltek.com', "
					+"'0', "
					+"'"+POut.String(@"C:\DentalTek\CallTray\CallTray.exe")+"', "
					+"'"+POut.String(@"/DeepLink=RapidCall")+"', "//leave blank if none
					+"'')";
				long programNum=Db.NonQ(command,true);
				command="INSERT INTO programproperty (ProgramPropertyNum,ProgramNum,PropertyDesc,PropertyValue,ClinicNum"
					+") VALUES("
					+"(SELECT MAX(ProgramPropertyNum+1) FROM programproperty),"
					+"'"+POut.Long(programNum)+"', "
					+"'Disable Advertising', "
					+"'0', "
					+"'0')";
				Db.NonQ(command);
				}//end RapidCall bridge
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE schedule ADD ClinicNum bigint NOT NULL";
					Db.NonQ(command);
					command="ALTER TABLE schedule ADD INDEX (ClinicNum)";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE schedule ADD ClinicNum number(20)";
					Db.NonQ(command);
					command="UPDATE schedule SET ClinicNum = 0 WHERE ClinicNum IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE schedule MODIFY ClinicNum NOT NULL";
					Db.NonQ(command);
					command=@"CREATE INDEX schedule_ClinicNum ON schedule (ClinicNum)";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE apptview ADD IsApptBubblesDisabled tinyint NOT NULL";
					Db.NonQ(command);
					command="UPDATE apptview SET IsApptBubblesDisabled=(SELECT ValueString FROM preference WHERE PrefName='AppointmentBubblesDisabled')";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE apptview ADD IsApptBubblesDisabled number(3)";
					Db.NonQ(command);
					command="UPDATE apptview SET IsApptBubblesDisabled = 0 WHERE IsApptBubblesDisabled IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE apptview MODIFY IsApptBubblesDisabled NOT NULL";
					Db.NonQ(command);
					command="UPDATE apptview SET IsApptBubblesDisabled=(SELECT ValueString FROM preference WHERE PrefName='AppointmentBubblesDisabled')";
					Db.NonQ(command);
				}

				command="UPDATE preference SET ValueString = '16.2.0.0' WHERE PrefName = 'DataBaseVersion'";
				Db.NonQ(command);
			}
			//To16_2_1();
		}

	}
}