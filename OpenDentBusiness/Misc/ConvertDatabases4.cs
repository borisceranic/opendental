using CodeBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace OpenDentBusiness {
	public partial class ConvertDatabases {
		public static System.Version LatestVersion=new Version("16.3.0.0");//This value must be changed when a new conversion is to be triggered.

		private static void To16_2_1() {
			if(FromVersion<new Version("16.2.1.0")) {
				ODEvent.Fire(new ODEventArgs("ConvertDatabases","Upgrading database to version: 16.2.1"));//No translation in convert script.
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
				command="UPDATE creditcard SET CCSource = 1 WHERE XChargeToken != ''";//CreditCardSource.XServer
				Db.NonQ(command);
				command="UPDATE creditcard SET CCSource = 3 WHERE PayConnectToken != ''";//CreditCardSource.PayConnect
				Db.NonQ(command);
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
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE computerpref ADD NoShowDecimal tinyint NOT NULL";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE computerpref ADD NoShowDecimal number(3)";
					Db.NonQ(command);
					command="UPDATE computerpref SET NoShowDecimal = 0 WHERE NoShowDecimal IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE computerpref MODIFY NoShowDecimal NOT NULL";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE creditcard ADD ClinicNum bigint NOT NULL";
					Db.NonQ(command);
					command="ALTER TABLE creditcard ADD INDEX (ClinicNum)";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE creditcard ADD ClinicNum number(20)";
					Db.NonQ(command);
					command="UPDATE creditcard SET ClinicNum = 0 WHERE ClinicNum IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE creditcard MODIFY ClinicNum NOT NULL";
					Db.NonQ(command);
					command=@"CREATE INDEX creditcard_ClinicNum ON creditcard (ClinicNum)";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE payment ADD PaymentSource tinyint NOT NULL";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE payment ADD PaymentSource number(3)";
					Db.NonQ(command);
					command="UPDATE payment SET PaymentSource = 0 WHERE PaymentSource IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE payment MODIFY PaymentSource NOT NULL";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE payment ADD ProcessStatus tinyint NOT NULL";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE payment ADD ProcessStatus number(3)";
					Db.NonQ(command);
					command="UPDATE payment SET ProcessStatus = 0 WHERE ProcessStatus IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE payment MODIFY ProcessStatus NOT NULL";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE dunning ADD DaysInAdvance int NOT NULL";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE dunning ADD DaysInAdvance number(11)";
					Db.NonQ(command);
					command="UPDATE dunning SET DaysInAdvance = 0 WHERE DaysInAdvance IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE dunning MODIFY DaysInAdvance NOT NULL";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="DROP TABLE IF EXISTS displayreport";
					Db.NonQ(command);
					command=@"CREATE TABLE displayreport (
						DisplayReportNum bigint NOT NULL auto_increment PRIMARY KEY,
						InternalName varchar(255) NOT NULL,
						ItemOrder int NOT NULL,
						Description varchar(255) NOT NULL,
						Category tinyint NOT NULL,
						IsHidden tinyint NOT NULL
						) DEFAULT CHARSET=utf8";
					Db.NonQ(command);
				}
				else {//oracle
					command="BEGIN EXECUTE IMMEDIATE 'DROP TABLE displayreport'; EXCEPTION WHEN OTHERS THEN NULL; END;";
					Db.NonQ(command);
					command=@"CREATE TABLE displayreport (
						DisplayReportNum number(20) NOT NULL,
						InternalName varchar2(255),
						ItemOrder number(11) NOT NULL,
						Description varchar2(255),
						Category number(3) NOT NULL,
						IsHidden number(3) NOT NULL,
						CONSTRAINT displayreport_DisplayReportNum PRIMARY KEY (DisplayReportNum)
						)";
					Db.NonQ(command);
				}
				//default display reports.
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command=@"
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODToday',0,'Today',0,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODYesterday',1,'Yesterday',0,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODThisMonth',2,'This Month',0,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODLastMonth',3,'Last Month',0,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODThisYear',4,'This Year',0,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODMoreOptions',5,'More Options',0,0);

					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODAdjustments',0,'Adjustments',1,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODPayments',1,'Payments',1,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODProcedures',2,'Procedures',1,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODWriteoffs',3,'Writeoffs',1,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODIncompleteProcNotes',4,'Incomplete Procedure Notes',1,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODRoutingSlips',5,'Routing Slips',1,0);

					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODAgingAR',0,'Aging of A/R',2,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODClaimsNotSent',1,'Claims Not Sent',2,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODCapitation',2,'Capitation Utilization',2,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODFinanceCharge',3,'Finance Charge Report',2,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODOutstandingInsClaims',4,'Outstanding Insurance Claims',2,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODProcsNotBilled',5,'Procedures Not Billed to Insurance',2,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODPPOWriteoffs',6,'PPO Writeoffs',2,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODPaymentPlans',7,'Payment Plans',2,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODReceivablesBreakdown',8,'Receivables Breakdown',2,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODUnearnedIncome',9,'Unearned Income',2,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODInsuranceOverpaid',10,'Insurance Overpaid',2,0);

					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODActivePatients',0,'Active Patients',3,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODAppointments',1,'Appointments',3,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODBirthdays',2,'Birthdays',3,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODBrokenAppointments',3,'BrokenAppointments',3,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODInsurancePlans',4,'Insurance Plans',3,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODNewPatients',5,'New Patients',3,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODPatientsRaw',6,'Patients - Raw',3,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODPatientNotes',7,'Patient Notes',3,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODPrescriptions',8,'Prescriptions',3,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODProcedureCodes',9,'Procedure Codes',3,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODReferralsRaw',10,'Referrals - Raw',3,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODReferralAnalysis',11,'Referral Analysis',3,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODReferredProcTracking',12,'Referred Proc Tracking',3,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODTreatmentFinder',13,'Treatment Finder',3,0);

					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODRawScreeningData',0,'Raw Screening Data',4,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODRawPopulationData',1,'Raw Population Data',4,0);

					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODEligibilityFile',0,'Eligibility File',5,0);
					INSERT INTO displayreport(InternalName,ItemOrder,Description,Category,IsHidden) VALUES('ODEncounterFile',1,'Encounter File',5,0);";
					Db.NonQ(command);
				}
				else {//oracle
					command=@"
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT COALESCE(MAX(DisplayReportNum), 0)+1 FROM displayreport),'ODToday',0,'Today',0,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODYesterday',1,'Yesterday',0,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODThisMonth',2,'This Month',0,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODLastMonth',3,'Last Month',0,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODThisYear',4,'This Year',0,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODMoreOptions',5,'More Options',0,0);

					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODAdjustments',0,'Adjustments',1,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODPayments',1,'Payments',1,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODProcedures',2,'Procedures',1,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODWriteoffs',3,'Writeoffs',1,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODIncompleteProcNotes',4,'Incomplete Procedure Notes',1,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODRoutingSlips',5,'Routing Slips',1,0);

					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODAgingAR',0,'Aging of A/R',2,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODClaimsNotSent',1,'Claims Not Sent',2,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODCapitation',2,'Capitation Utilization',2,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODFinanceCharge',3,'Finance Charge Report',2,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODOutstandingInsClaims',4,'Outstanding Insurance Claims',2,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODProcsNotBilled',5,'Procedures Not Billed to Insurance',2,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODPPOWriteoffs',6,'PPO Writeoffs',2,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODPaymentPlans',7,'Payment Plans',2,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODReceivablesBreakdown',8,'Receivables Breakdown',2,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODUnearnedIncome',9,'Unearned Income',2,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODInsuranceOverpaid',10,'Insurance Overpaid',2,0);

					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODActivePatients',0,'Active Patients',3,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODAppointments',1,'Appointments',3,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODBirthdays',2,'Birthdays',3,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODBrokenAppointments',3,'BrokenAppointments',3,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODInsurancePlans',4,'Insurance Plans',3,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODNewPatients',5,'New Patients',3,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODPatientsRaw',6,'Patients - Raw',3,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODPatientNotes',7,'Patient Notes',3,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODPrescriptions',8,'Prescriptions',3,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODProcedureCodes',9,'Procedure Codes',3,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODReferralsRaw',10,'Referrals - Raw',3,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODReferralAnalysis',11,'Referral Analysis',3,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODReferredProcTracking',12,'Referred Proc Tracking',3,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODTreatmentFinder',13,'Treatment Finder',3,0);

					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODRawScreeningData',0,'Raw Screening Data',4,0);
					INSERT INTO displayreport (DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES ((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODRawPopulationData',1,'Raw Population Data',4,0);

					INSERT INTO displayreport(DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODEligibilityFile',0,'Eligibility File',5,0);
					INSERT INTO displayreport(DisplayReportNum,InternalName,ItemOrder,Description,Category,IsHidden) VALUES((SELECT MAX(DisplayReportNum)+1 FROM displayreport),'ODEncounterFile',1,'Encounter File',5,0);";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {//Insert PaySplitUnearnedType definition if it doesn't already exist.
					command="SELECT COUNT(*) FROM definition WHERE Category=29 AND ItemName='Prepayment'";
					if(PIn.Int(Db.GetCount(command))==0) {//PaySplitUnearnedType definition doesn't already exist
						command="SELECT MAX(ItemOrder)+1 FROM definition WHERE Category=29";
						string maxOrder=Db.GetScalar(command);
						if(maxOrder=="") {
							maxOrder="0";
						}
						command="INSERT INTO definition (Category, ItemOrder, ItemName) VALUES (29,"+maxOrder+",'Prepayment')";
						Db.NonQ(command);
					}
				}
				else {//oracle (Note for reviewer: I'm not at all sure this is oracle compatible)
					command="SELECT COUNT(*) FROM definition WHERE Category=29 AND ItemName='Prepayment'";
					if(PIn.Int(Db.GetCount(command))==0) {//PaySplitUnearnedType definition doesn't already exist
						command="SELECT MAX(ItemOrder)+1 FROM definition WHERE Category=29";
						string maxOrder=Db.GetScalar(command);
						if(maxOrder=="") {
							maxOrder="0";
						}
						command="INSERT INTO definition (DefNum,Category, ItemOrder, ItemName) VALUES ((SELECT MAX(DefNum)+1 FROM definition),29,"+maxOrder+",'Prepayment')";
						Db.NonQ(command);
					}
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="SELECT DefNum FROM definition WHERE Category=29 AND ItemName='Prepayment'";
					string defNum=Db.GetScalar(command);
					command="INSERT INTO preference(PrefName,ValueString) VALUES('PrepaymentUnearnedType','"+defNum+"')";
					Db.NonQ(command);
				}
				else {//oracle
					command="SELECT DefNum FROM definition WHERE Category=29 AND ItemName='Prepayment'";
					string defNum=Db.GetScalar(command);
					command="INSERT INTO preference(PrefNum,PrefName,ValueString) VALUES((SELECT MAX(PrefNum)+1 FROM preference),'PrepaymentUnearnedType','"+defNum+"')";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE paysplit ADD PrepaymentNum bigint NOT NULL";
					Db.NonQ(command);
					command="ALTER TABLE paysplit ADD INDEX (PrepaymentNum)";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE paysplit ADD PrepaymentNum number(20)";
					Db.NonQ(command);
					command="UPDATE paysplit SET PrepaymentNum = 0 WHERE PrepaymentNum IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE paysplit MODIFY PrepaymentNum NOT NULL";
					Db.NonQ(command);
					command=@"CREATE INDEX paysplit_PrepaymentNum ON paysplit (PrepaymentNum)";
					Db.NonQ(command);
				}
				//Add InsPlanEdit permission to everyone------------------------------------------------------
				command="SELECT DISTINCT UserGroupNum FROM grouppermission";
				DataTable table=Db.GetTable(command);
				long groupNum;
				if(DataConnection.DBtype==DatabaseType.MySql) {
				   foreach(DataRow row in table.Rows) {
					  groupNum=PIn.Long(row["UserGroupNum"].ToString());
					  command="INSERT INTO grouppermission (UserGroupNum,PermType) "
						 +"VALUES("+POut.Long(groupNum)+",110)";//110 - InsPlanEdit
					  Db.NonQ(command);
				   }
				}
				else {//oracle
				   foreach(DataRow row in table.Rows) {
					  groupNum=PIn.Long(row["UserGroupNum"].ToString());
					  command="INSERT INTO grouppermission (GroupPermNum,NewerDays,UserGroupNum,PermType) "
						 +"VALUES((SELECT MAX(GroupPermNum)+1 FROM grouppermission),0,"+POut.Long(groupNum)+",110)";//110 - InsPlanEdit
					  Db.NonQ(command);
				   }
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {//Insert Canceled Appointment Procedure account color
					command="SELECT MAX(ItemOrder)+1 FROM definition WHERE Category=0";//0 is AccountColor
					string maxOrder=Db.GetScalar(command);
					if(maxOrder=="") {
						maxOrder="0";
					}
					command="SELECT ItemColor FROM definition WHERE Category=0 AND ItemName='Broken Appointment Procedure'";
					string color=Db.GetScalar(command);
					if(color=="") {
						color="-16777031";//blue
					}
					command="INSERT INTO definition (Category, ItemOrder, ItemName, ItemColor) "
						+"VALUES (0,"+maxOrder+",'Canceled Appointment Procedure','"+color+"')";
					Db.NonQ(command);
				}
				else {//oracle 
					command="SELECT MAX(ItemOrder)+1 FROM definition WHERE Category=0";//0 is AccountColor
					string maxOrder=Db.GetScalar(command);
					if(maxOrder=="") {
						maxOrder="0";
					}
					command="SELECT ItemColor FROM definition WHERE Category=0 AND ItemName='Broken Appointment Procedure'";
					string color=Db.GetScalar(command);
					if(color=="") {
						color="-16777031";//blue
					}
					command="INSERT INTO definition (DefNum, Category, ItemOrder, ItemName, ItemColor) "
						+"VALUES ((SELECT MAX(DefNum)+1 FROM definition),0,"+maxOrder+",'Canceled Appointment Procedure','"+color+"')";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE ehrmeasureevent ADD TobaccoCessationDesire tinyint unsigned NOT NULL";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE ehrmeasureevent ADD TobaccoCessationDesire number(3)";
					Db.NonQ(command);
					command="UPDATE ehrmeasureevent SET TobaccoCessationDesire = 0 WHERE TobaccoCessationDesire IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE ehrmeasureevent MODIFY TobaccoCessationDesire NOT NULL";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE ehrmeasureevent ADD DateStartTobacco date NOT NULL DEFAULT '0001-01-01'";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE ehrmeasureevent ADD DateStartTobacco date";
					Db.NonQ(command);
					command="UPDATE ehrmeasureevent SET DateStartTobacco = TO_DATE('0001-01-01','YYYY-MM-DD') WHERE DateStartTobacco IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE ehrmeasureevent MODIFY DateStartTobacco NOT NULL";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE procedurelog ADD DateComplete date NOT NULL DEFAULT '0001-01-01'";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE procedurelog ADD DateComplete date";
					Db.NonQ(command);
					command="UPDATE procedurelog SET DateComplete = TO_DATE('0001-01-01','YYYY-MM-DD') WHERE DateComplete IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE procedurelog MODIFY DateComplete NOT NULL";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE claimproc ADD DateSuppReceived date NOT NULL DEFAULT '0001-01-01'";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE claimproc ADD DateSuppReceived date";
					Db.NonQ(command);
					command="UPDATE claimproc SET DateSuppReceived = TO_DATE('0001-01-01','YYYY-MM-DD') WHERE DateSuppReceived IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE claimproc MODIFY DateSuppReceived NOT NULL";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="INSERT INTO preference (PrefName,ValueString) VALUES('WikiDetectLinks','0')";
					Db.NonQ(command);
				}
				else {//oracle
					command="INSERT INTO preference (PrefNum,PrefName,ValueString) VALUES((SELECT MAX(PrefNum)+1 FROM preference),'WikiDetectLinks','0')";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="INSERT INTO preference (PrefName,ValueString) VALUES('WikiCreatePageFromLink','0')";
					Db.NonQ(command);
				}
				else {//oracle
					command="INSERT INTO preference (PrefNum,PrefName,ValueString) VALUES((SELECT MAX(PrefNum)+1 FROM preference),'WikiCreatePageFromLink','0')";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="INSERT INTO preference (PrefName,ValueString) VALUES('BadDebtAdjustmentTypes','')";
					Db.NonQ(command);
				}
				else {//oracle
					command="INSERT INTO preference (PrefNum,PrefName,ValueString) VALUES((SELECT MAX(PrefNum)+1 FROM preference),'BadDebtAdjustmentTypes','')";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE automation ADD AptStatus tinyint NOT NULL";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE automation ADD AptStatus number(3)";
					Db.NonQ(command);
					command="UPDATE automation SET AptStatus = 0 WHERE AptStatus IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE automation MODIFY AptStatus NOT NULL";
					Db.NonQ(command);
				}
				//Insert Carestream bridge-----------------------------------------------------------------
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
						+") VALUES("
						+"'Carestream', "
						+"'Carestream from www.carestreamdental.com', "
						+"'0', "
						+"'"+POut.String(@"pwimage.exe")+"', "
						+"'', "//leave blank if none
						+"'')";
					long programNum=Db.NonQ(command,true);
					command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
						+") VALUES("
						+"'"+POut.Long(programNum)+"', "
						+"'Enter 0 to use PatientNum, or 1 to use ChartNum', "
						+"'0')";
					Db.NonQ(command);
					command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
						+") VALUES("
						+"'"+POut.Long(programNum)+"', "
						+"'Patient.ini path', "
						+"'"+POut.String(@"C:\Carestream\Patient.ini")+"')";
					Db.NonQ(command);
					command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
						+"VALUES ("
						+"'"+POut.Long(programNum)+"', "
						+"'2', "//ToolBarsAvail.ChartModule
						+"'Carestream')";
					Db.NonQ(command);
				}
				else {//oracle
					command="INSERT INTO program (ProgramNum,ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
						+") VALUES("
						+"(SELECT MAX(ProgramNum)+1 FROM program),"
						+"'Carestream', "
						+"'Carestream from www.carestreamdental.com', "
						+"'0', "
						+"'"+POut.String(@"pwimage.exe")+"', "
						+"'', "//leave blank if none
						+"'')";
					long programNum=Db.NonQ(command,true);
					command="INSERT INTO programproperty (ProgramPropertyNum,ProgramNum,PropertyDesc,PropertyValue,ClinicNum"
						+") VALUES("
						+"(SELECT MAX(ProgramPropertyNum+1) FROM programproperty),"
						+"'"+POut.Long(programNum)+"', "
						+"'Enter 0 to use PatientNum, or 1 to use ChartNum', "
						+"'0', "
						+"'0')";
					Db.NonQ(command);
					command="INSERT INTO programproperty (ProgramPropertyNum,ProgramNum,PropertyDesc,PropertyValue,ClinicNum"
						+") VALUES("
						+"(SELECT MAX(ProgramPropertyNum+1) FROM programproperty),"
						+"'"+POut.Long(programNum)+"', "
						+"'Patient.ini path', "
						+"'"+POut.String(@"C:\Carestream\Patient.ini")+"', "
						+"'0')";
					Db.NonQ(command);
					command="INSERT INTO toolbutitem (ToolButItemNum,ProgramNum,ToolBar,ButtonText) "
						+"VALUES ("
						+"(SELECT MAX(ToolButItemNum)+1 FROM toolbutitem),"
						+"'"+POut.Long(programNum)+"', "
						+"'2', "//ToolBarsAvail.ChartModule
						+"'Carestream')";
					Db.NonQ(command);
				}//end Carestream bridge
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="INSERT INTO preference(PrefName,ValueString) VALUES('CentralManagerSyncCode','')";
					Db.NonQ(command);
				}
				else {//oracle
					command="INSERT INTO preference (PrefNum,PrefName,ValueString) VALUES((SELECT MAX(PrefNum)+1 FROM preference),'CentralManagerSyncCode','')";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE claimsnapshot ADD ClaimProcNum bigint NOT NULL";
					Db.NonQ(command);
					command="ALTER TABLE claimsnapshot ADD INDEX (ClaimProcNum)";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE claimsnapshot ADD ClaimProcNum number(20)";
					Db.NonQ(command);
					command="UPDATE claimsnapshot SET ClaimProcNum = 0 WHERE ClaimProcNum IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE claimsnapshot MODIFY ClaimProcNum NOT NULL";
					Db.NonQ(command);
					command=@"CREATE INDEX claimsnapshot_ClaimProcNum ON claimsnapshot (ClaimProcNum)";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE claimsnapshot ADD SnapshotTrigger tinyint NOT NULL";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE claimsnapshot ADD SnapshotTrigger number(3)";
					Db.NonQ(command);
					command="UPDATE claimsnapshot SET SnapshotTrigger = 0 WHERE SnapshotTrigger IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE claimsnapshot MODIFY SnapshotTrigger NOT NULL";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
				   command="INSERT INTO preference(PrefName,ValueString) VALUES('ClaimSnapshotRunTime','1881-01-01 23:30:00')";
				   Db.NonQ(command);
				}
				else {//oracle
				   command="INSERT INTO preference(PrefNum,PrefName,ValueString) VALUES((SELECT MAX(PrefNum)+1 FROM preference),'ClaimSnapshotRunTime','1881-01-01 23:30:00')";
				   Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
				   command="INSERT INTO preference(PrefName,ValueString) VALUES('ClaimSnapshotTriggerType','ClaimCreate')";
				   Db.NonQ(command);
				}
				else {//oracle
				   command="INSERT INTO preference(PrefNum,PrefName,ValueString) VALUES((SELECT MAX(PrefNum)+1 FROM preference),'ClaimSnapshotTriggerType','ClaimCreate')";
				   Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE clinic ADD ItemOrder int NOT NULL";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE clinic ADD ItemOrder number(11)";
					Db.NonQ(command);
					command="UPDATE clinic SET ItemOrder = 0 WHERE ItemOrder IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE clinic MODIFY ItemOrder NOT NULL";
					Db.NonQ(command);
				}
				command="SELECT * FROM clinic";
				DataTable clinics=Db.GetTable(command);
				for(int i=0;i<clinics.Rows.Count;i++) {
					command="UPDATE clinic SET ItemOrder="+POut.Int(i)+" WHERE ClinicNum="+clinics.Rows[i]["ClinicNum"].ToString();
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="INSERT INTO preference(PrefName,ValueString) VALUES ('ClinicListIsAlphabetical','0')";
					Db.NonQ(command);
				}
				else {//oracle
					command="INSERT INTO preference(PrefNum,PrefName,ValueString) VALUES((SELECT MAX(PrefNum)+1 FROM preference),'ClinicListIsAlphabetical','0')";
					Db.NonQ(command);
				}
				command="UPDATE preference SET ValueString='1' WHERE PrefName='ClaimSnapshotEnabled'";
				Db.NonQ(command);
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE clockevent ADD ClinicNum bigint NOT NULL";
					Db.NonQ(command);
					command="ALTER TABLE clockevent ADD INDEX (ClinicNum)";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE clockevent ADD ClinicNum number(20)";
					Db.NonQ(command);
					command="UPDATE clockevent SET ClinicNum = 0 WHERE ClinicNum IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE clockevent MODIFY ClinicNum NOT NULL";
					Db.NonQ(command);
					command=@"CREATE INDEX clockevent_ClinicNum ON clockevent (ClinicNum)";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE timeadjust ADD ClinicNum bigint NOT NULL";
					Db.NonQ(command);
					command="ALTER TABLE timeadjust ADD INDEX (ClinicNum)";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE timeadjust ADD ClinicNum number(20)";
					Db.NonQ(command);
					command="UPDATE timeadjust SET ClinicNum = 0 WHERE ClinicNum IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE timeadjust MODIFY ClinicNum NOT NULL";
					Db.NonQ(command);
					command=@"CREATE INDEX timeadjust_ClinicNum ON timeadjust (ClinicNum)";
					Db.NonQ(command);
				}
				//Users with ProcComplEdit permission will be granted ProcComplEditLimited permission with the same days/date restriction.
				command="SELECT NewerDate,NewerDays,UserGroupNum FROM grouppermission WHERE PermType=10"; //ProcComplEdit
				table=Db.GetTable(command);
				DateTime newerDate;
				int newerDays;
				if(DataConnection.DBtype==DatabaseType.MySql) {
					foreach(DataRow row in table.Rows) {
						newerDate=PIn.Date(row["NewerDate"].ToString());
						newerDays=PIn.Int(row["NewerDays"].ToString());
						groupNum=PIn.Long(row["UserGroupNum"].ToString());
						command="INSERT INTO grouppermission (NewerDate,NewerDays,UserGroupNum,PermType) "
							+"VALUES("+POut.Date(newerDate)+","+POut.Int(newerDays)+","+POut.Long(groupNum)+",117)";//117 - ProcComplEditLimited
						Db.NonQ(command);
					}
				}
				else {//oracle
					foreach(DataRow row in table.Rows) {
						newerDate=PIn.Date(row["NewerDate"].ToString());
						newerDays=PIn.Int(row["NewerDays"].ToString());
						groupNum=PIn.Long(row["UserGroupNum"].ToString());
						command="INSERT INTO grouppermission (GroupPermNum,NewerDate,NewerDays,UserGroupNum,PermType) "
							+"VALUES((SELECT MAX(GroupPermNum)+1 FROM grouppermission),"+POut.Date(newerDate)+","+POut.Int(newerDays)+","
							+POut.Long(groupNum)+",117)";//117 - ProcComplEditLimited
						Db.NonQ(command);
					}
				}
				//Add ClaimDelete permission for everyone
				command="SELECT DISTINCT UserGroupNum FROM grouppermission";
				table=Db.GetTable(command);
				if(DataConnection.DBtype==DatabaseType.MySql) {
					foreach(DataRow row in table.Rows) {
						groupNum=PIn.Long(row["UserGroupNum"].ToString());
						command="INSERT INTO grouppermission (UserGroupNum,PermType) "
							+"VALUES("+POut.Long(groupNum)+",118)";//118 - ClaimDelete
						Db.NonQ(command);
					}
				}
				else {//oracle
					foreach(DataRow row in table.Rows) {
						groupNum=PIn.Long(row["UserGroupNum"].ToString());
						command="INSERT INTO grouppermission (GroupPermNum,UserGroupNum,PermType) "
							+"VALUES((SELECT MAX(GroupPermNum)+1 FROM grouppermission),"+POut.Long(groupNum)+",118)";//118 - ClaimDelete
						Db.NonQ(command);
					}
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="INSERT INTO preference(PrefName,ValueString) VALUES('ClaimProcsNotBilledToInsAutoGroup','1')";
					Db.NonQ(command);
				}
				else {//oracle
					command="INSERT INTO preference(PrefNum,PrefName,ValueString) VALUES((SELECT MAX(PrefNum)+1 FROM preference),'ClaimProcsNotBilledToInsAutoGroup','1')";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="DROP TABLE IF EXISTS xwebresponse";
					Db.NonQ(command);
					command=@"CREATE TABLE xwebresponse (
						XWebResponseNum bigint NOT NULL auto_increment PRIMARY KEY,
						PatNum bigint NOT NULL,
						ProvNum bigint NOT NULL,
						ClinicNum bigint NOT NULL,
						PaymentNum bigint NOT NULL,
						DateTEntry datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
						DateTUpdate datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
						TransactionStatus tinyint NOT NULL,
						ResponseCode int NOT NULL,
						XWebResponseCode varchar(255) NOT NULL,
						ResponseDescription varchar(255) NOT NULL,
						OTK varchar(255) NOT NULL,
						HpfUrl text NOT NULL,
						HpfExpiration datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
						TransactionID varchar(255) NOT NULL,
						TransactionType varchar(255) NOT NULL,
						Alias varchar(255) NOT NULL,
						CardType varchar(255) NOT NULL,
						CardBrand varchar(255) NOT NULL,
						CardBrandShort varchar(255) NOT NULL,
						MaskedAcctNum varchar(255) NOT NULL,
						Amount double NOT NULL,
						ApprovalCode varchar(255) NOT NULL,
						CardCodeResponse varchar(255) NOT NULL,
						ReceiptID int NOT NULL,
						ExpDate varchar(255) NOT NULL,
						EntryMethod varchar(255) NOT NULL,
						ProcessorResponse varchar(255) NOT NULL,
						BatchNum int NOT NULL,
						BatchAmount double NOT NULL,
						AccountExpirationDate date NOT NULL DEFAULT '0001-01-01',
						DebugError text NOT NULL,
						PayNote text NOT NULL,
						INDEX(PatNum),
						INDEX(ProvNum),
						INDEX(ClinicNum),
						INDEX(PaymentNum)
						) DEFAULT CHARSET=utf8";
					Db.NonQ(command);
				}
				else {//oracle
					command="BEGIN EXECUTE IMMEDIATE 'DROP TABLE xwebresponse'; EXCEPTION WHEN OTHERS THEN NULL; END;";
					Db.NonQ(command);
					command=@"CREATE TABLE xwebresponse (
						XWebResponseNum number(20) NOT NULL,
						PatNum number(20) NOT NULL,
						ProvNum number(20) NOT NULL,
						ClinicNum number(20) NOT NULL,
						PaymentNum number(20) NOT NULL,
						DateTEntry date DEFAULT TO_DATE('0001-01-01','YYYY-MM-DD') NOT NULL,
						DateTUpdate date DEFAULT TO_DATE('0001-01-01','YYYY-MM-DD') NOT NULL,
						TransactionStatus number(3) NOT NULL,
						ResponseCode number(11) NOT NULL,
						XWebResponseCode varchar2(255),
						ResponseDescription varchar2(255),
						OTK varchar2(255),
						HpfUrl clob,
						HpfExpiration date DEFAULT TO_DATE('0001-01-01','YYYY-MM-DD') NOT NULL,
						TransactionID varchar2(255),
						TransactionType varchar2(255),
						Alias varchar2(255),
						CardType varchar2(255),
						CardBrand varchar2(255),
						CardBrandShort varchar2(255),
						MaskedAcctNum varchar2(255),
						Amount number(38,8) NOT NULL,
						ApprovalCode varchar2(255),
						CardCodeResponse varchar2(255),
						ReceiptID number(11) NOT NULL,
						ExpDate varchar2(255),
						EntryMethod varchar2(255),
						ProcessorResponse varchar2(255),
						BatchNum number(11) NOT NULL,
						BatchAmount number(38,8) NOT NULL,
						AccountExpirationDate date DEFAULT TO_DATE('0001-01-01','YYYY-MM-DD') NOT NULL,
						DebugError clob,
						PayNote clob,
						CONSTRAINT xwebresponse_XWebResponseNum PRIMARY KEY (XWebResponseNum)
						)";
					Db.NonQ(command);
					command=@"CREATE INDEX xwebresponse_PatNum ON xwebresponse (PatNum)";
					Db.NonQ(command);
					command=@"CREATE INDEX xwebresponse_ProvNum ON xwebresponse (ProvNum)";
					Db.NonQ(command);
					command=@"CREATE INDEX xwebresponse_ClinicNum ON xwebresponse (ClinicNum)";
					Db.NonQ(command);
					command=@"CREATE INDEX xwebresponse_PaymentNum ON xwebresponse (PaymentNum)";
					Db.NonQ(command);
				}
				//Add InsWriteOffEdit permission for everyone
				command="SELECT DISTINCT UserGroupNum FROM grouppermission";
				table=Db.GetTable(command);
				if(DataConnection.DBtype==DatabaseType.MySql) {
					foreach(DataRow row in table.Rows) {
						groupNum=PIn.Long(row["UserGroupNum"].ToString());
						command="INSERT INTO grouppermission (UserGroupNum,PermType) "
							+"VALUES("+POut.Long(groupNum)+",119)";//119 - InsWriteOffEdit
						Db.NonQ(command);
					}
				}
				else {//oracle
					foreach(DataRow row in table.Rows) {
						groupNum=PIn.Long(row["UserGroupNum"].ToString());
						command="INSERT INTO grouppermission (GroupPermNum,UserGroupNum,PermType) "
							+"VALUES((SELECT MAX(GroupPermNum)+1 FROM grouppermission),"+POut.Long(groupNum)+",119)";//119 - InsWriteOffEdit
						Db.NonQ(command);
					}
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
				   command="INSERT INTO preference(PrefName,ValueString) VALUES('InsVerifyExcludePatientClones','0')";
				   Db.NonQ(command);
				}
				else {//oracle
				   command="INSERT INTO preference(PrefNum,PrefName,ValueString) VALUES((SELECT MAX(PrefNum)+1 FROM preference),'InsVerifyExcludePatientClones','0')";
				   Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
				   command="INSERT INTO preference(PrefName,ValueString) VALUES('InsVerifyExcludePatVerify','0')";
				   Db.NonQ(command);
				}
				else {//oracle
				   command="INSERT INTO preference(PrefNum,PrefName,ValueString) VALUES((SELECT MAX(PrefNum)+1 FROM preference),'InsVerifyExcludePatVerify','0')";
				   Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE clinic ADD IsInsVerifyExcluded tinyint NOT NULL";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE clinic ADD IsInsVerifyExcluded number(3)";
					Db.NonQ(command);
					command="UPDATE clinic SET IsInsVerifyExcluded = 0 WHERE IsInsVerifyExcluded IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE clinic MODIFY IsInsVerifyExcluded NOT NULL";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="INSERT INTO preference (PrefName,ValueString) VALUES('PayPlansVersion','1')";
					Db.NonQ(command);
				}
				else {//oracle
					command="INSERT INTO preference (PrefNum,PrefName,ValueString) VALUES((SELECT MAX(PrefNum)+1 FROM preference),'PayPlansVersion','1')";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE payplancharge ADD ChargeType tinyint NOT NULL";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE payplancharge ADD ChargeType number(3)";
					Db.NonQ(command);
					command="UPDATE payplancharge SET ChargeType = 0 WHERE ChargeType IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE payplancharge MODIFY ChargeType NOT NULL";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE payplancharge ADD ProcNum bigint NOT NULL";
					Db.NonQ(command);
					command="ALTER TABLE payplancharge ADD INDEX (ProcNum)";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE payplancharge ADD ProcNum number(20)";
					Db.NonQ(command);
					command="UPDATE payplancharge SET ProcNum = 0 WHERE ProcNum IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE payplancharge MODIFY ProcNum NOT NULL";
					Db.NonQ(command);
					command=@"CREATE INDEX payplancharge_ProcNum ON payplancharge (ProcNum)";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="INSERT INTO preference (PrefName,ValueString) VALUES('AccountShowCompletedPaymentPlans','0')";
					Db.NonQ(command);
				}
				else {//oracle
					command="INSERT INTO preference (PrefNum,PrefName,ValueString) VALUES((SELECT MAX(PrefNum)+1 FROM preference),'AccountShowCompletedPaymentPlans','0')";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE payplan ADD IsClosed tinyint NOT NULL";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE payplan ADD IsClosed number(3)";
					Db.NonQ(command);
					command="UPDATE payplan SET IsClosed = 0 WHERE IsClosed IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE payplan MODIFY IsClosed NOT NULL";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="INSERT INTO preference (PrefName,ValueString) VALUES ('PayPeriodIntervalSetting','0')";
					Db.NonQ(command);
					command="INSERT INTO preference (PrefName,ValueString) VALUES('PayPeriodPayAfterNumberOfDays','5')";
					Db.NonQ(command);
					command="INSERT INTO preference (PrefName,ValueString) VALUES('PayPeriodPayDateBeforeWeekend','1')";
					Db.NonQ(command);
					command="INSERT INTO preference (PrefName,ValueString) VALUES('PayPeriodPayDateExcludesWeekends','1')";
					Db.NonQ(command);
					command="INSERT INTO preference (PrefName,ValueString) VALUES('PayPeriodPayDay','0')";
					Db.NonQ(command);
				}
				else {//oracle
					command="INSERT INTO preference (PrefNum,PrefName,ValueString) VALUES ((SELECT MAX(PrefNum)+1 FROM preference),'PayPeriodIntervalSetting','0')";
					Db.NonQ(command);
					command="INSERT INTO preference (PrefNum,PrefName,ValueString) VALUES((SELECT MAX(PrefNum)+1 FROM preference),'PayPeriodPayAfterNumberOfDays','5')";
					Db.NonQ(command);
					command="INSERT INTO preference (PrefNum,PrefName,ValueString) VALUES((SELECT MAX(PrefNum)+1 FROM preference),'PayPeriodPayDateBeforeWeekend','1')";
					Db.NonQ(command);
					command="INSERT INTO preference (PrefNum,PrefName,ValueString) VALUES((SELECT MAX(PrefNum)+1 FROM preference),'PayPeriodPayDateExcludesWeekends','1')";
					Db.NonQ(command);
					command="INSERT INTO preference (PrefNum,PrefName,ValueString) VALUES((SELECT MAX(PrefNum)+1 FROM preference),'PayPeriodPayDay','0')";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="DROP TABLE IF EXISTS orthocharttab";
					Db.NonQ(command);
					command=@"CREATE TABLE orthocharttab (
						OrthoChartTabNum bigint NOT NULL auto_increment PRIMARY KEY,
						TabName varchar(255) NOT NULL,
						ItemOrder int NOT NULL,
						IsHidden tinyint NOT NULL
						) DEFAULT CHARSET=utf8";
					Db.NonQ(command);
				}
				else {//oracle
					command="BEGIN EXECUTE IMMEDIATE 'DROP TABLE orthocharttab'; EXCEPTION WHEN OTHERS THEN NULL; END;";
					Db.NonQ(command);
					command=@"CREATE TABLE orthocharttab (
						OrthoChartTabNum number(20) NOT NULL,
						TabName varchar2(255),
						ItemOrder number(11) NOT NULL,
						IsHidden number(3) NOT NULL,
						CONSTRAINT orthocharttab_OrthoChartTabNum PRIMARY KEY (OrthoChartTabNum)
						)";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="DROP TABLE IF EXISTS orthocharttablink";
					Db.NonQ(command);
					command=@"CREATE TABLE orthocharttablink (
						OrthoChartTabLinkNum bigint NOT NULL auto_increment PRIMARY KEY,
						ItemOrder int NOT NULL,
						OrthoChartTabNum bigint NOT NULL,
						DisplayFieldNum bigint NOT NULL,
						INDEX(OrthoChartTabNum),
						INDEX(DisplayFieldNum)
						) DEFAULT CHARSET=utf8";
					Db.NonQ(command);
				}
				else {//oracle
					command="BEGIN EXECUTE IMMEDIATE 'DROP TABLE orthocharttablink'; EXCEPTION WHEN OTHERS THEN NULL; END;";
					Db.NonQ(command);
					command=@"CREATE TABLE orthocharttablink (
						OrthoChartTabLinkNum number(20) NOT NULL,
						ItemOrder number(11) NOT NULL,
						OrthoChartTabNum number(20) NOT NULL,
						DisplayFieldNum number(20) NOT NULL,
						CONSTRAINT orthocharttablink_TabLinkNum PRIMARY KEY (OrthoChartTabLinkNum)
						)";
					Db.NonQ(command);
					command=@"CREATE INDEX orthocharttablink_TabNum ON orthocharttablink (OrthoChartTabNum)";
					Db.NonQ(command);
					command=@"CREATE INDEX orthocharttablink_DisplayField ON orthocharttablink (DisplayFieldNum)";
					Db.NonQ(command);
				}
				string tabNameDefault="Ortho Chart";
				if(!CultureInfo.CurrentCulture.Name.EndsWith("US")) {//Not United States
					command="SELECT Translation FROM languageforeign "
						+"WHERE ClassType='ContrChart' AND English='Ortho Chart' AND Culture='"+CultureInfo.CurrentCulture.Name+"'";
					DataTable tableTrans=Db.GetTable(command);
					if(tableTrans.Rows.Count > 0) {
						tabNameDefault=tableTrans.Rows[0][0].ToString();
					}
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="INSERT INTO orthocharttab(TabName,ItemOrder,IsHidden) VALUES ('"+POut.String(tabNameDefault)+"',0,0)";
				}
				else {//oracle
					command="INSERT INTO orthocharttab(OrthoChartTabNum,TabName,ItemOrder,IsHidden) "
						+"VALUES ((SELECT COALESCE(MAX(OrthoChartTabNum),0)+1 FROM orthocharttab),'"+POut.String(tabNameDefault)+"',0,0)";					
				}
				long orthoChartTabNum=Db.NonQ(command,true);				
				command="SELECT DisplayFieldNum FROM displayfield WHERE Category=8 ORDER BY ItemOrder";//Ortho Chart display fields
				table=Db.GetTable(command);
				//Move all existing display fields into the default ortho chart tab.
				for(int i=0;i<table.Rows.Count;i++) {
					long displayFieldNum=PIn.Long(table.Rows[i]["DisplayFieldNum"].ToString());
					if(DataConnection.DBtype==DatabaseType.MySql) {
						command="INSERT INTO orthocharttablink (ItemOrder,OrthoChartTabNum,DisplayFieldNum) "
							+"VALUES ("+POut.Int(i)+","+POut.Long(orthoChartTabNum)+","+POut.Long(displayFieldNum)+")";
					}
					else {//oracle
						command="INSERT INTO orthocharttablink (OrthoChartTabLinkNum,ItemOrder,OrthoChartTabNum,DisplayFieldNum) "
							+"VALUES ((SELECT COALESCE(MAX(OrthoChartTabLinkNum),0)+1 FROM orthocharttablink),"
							+POut.Int(i)+","+POut.Long(orthoChartTabNum)+","+POut.Long(displayFieldNum)+")";
					}
					Db.NonQ(command);
				}
				command="SELECT ValueString from preference WHERE PrefName='ClearinghouseDefaultDent'";
				string value=Db.GetScalar(command);
				if(value=="0") {
					command="SELECT ValueString from preference WHERE PrefName='ClearinghouseDefaultMed'";
					value=Db.GetScalar(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
				   command="INSERT INTO preference(PrefName,ValueString) VALUES('ClearinghouseDefaultEligibility','"+value+"')";
				   Db.NonQ(command);
				}
				else {//oracle
				   command="INSERT INTO preference(PrefNum,PrefName,ValueString) VALUES((SELECT MAX(PrefNum)+1 FROM preference),'ClearinghouseDefaultEligibility','"+value+"')";
				   Db.NonQ(command);
				}
				//Insert programproperty IsOnlinePaymentsEnabled for X-Charge per clinic to indicate whether to use for patient portal web payments
				command="SELECT ProgramNum FROM program WHERE ProgName='Xcharge'";
				long xChargeProgNum=PIn.Long(Db.GetScalar(command));
				command="SELECT DISTINCT ClinicNum FROM programproperty WHERE ProgramNum="+POut.Long(xChargeProgNum);
				List<long> listClinicNums=Db.GetListLong(command);
				if(DataConnection.DBtype==DatabaseType.MySql) {
					foreach(long clinicNum in listClinicNums) {
						command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue,ComputerName,ClinicNum) "
							+"VALUES ("+POut.Long(xChargeProgNum)+",'IsOnlinePaymentsEnabled','0','',"+POut.Long(clinicNum)+")";
						Db.NonQ(command);
					}
				}
				else {//oracle
					foreach(long clinicNum in listClinicNums) {
						command="INSERT INTO programproperty (ProgramPropertyNum,ProgramNum,PropertyDesc,PropertyValue,ComputerName,ClinicNum) "
							+"VALUES ((SELECT MAX(ProgramPropertyNum)+1 FROM programproperty),"
							+POut.Long(xChargeProgNum)+",'IsOnlinePaymentsEnabled','0','',"+POut.Long(clinicNum)+")";
						Db.NonQ(command);
					}
				}
				//Insert programproperty IsOnlinePaymentsEnabled for PayConnect per clinic to indicate whether to use for patient portal web payments
				command="SELECT ProgramNum FROM program WHERE ProgName='PayConnect'";
				long payConnectProgNum=PIn.Long(Db.GetScalar(command));
				command="SELECT DISTINCT ClinicNum FROM programproperty WHERE ProgramNum="+POut.Long(payConnectProgNum);
				listClinicNums=Db.GetListLong(command);
				if(DataConnection.DBtype==DatabaseType.MySql) {
					foreach(long clinicNum in listClinicNums) {
						command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue,ComputerName,ClinicNum) "
							+"VALUES ("+POut.Long(payConnectProgNum)+",'IsOnlinePaymentsEnabled','0','',"+POut.Long(clinicNum)+")";
						Db.NonQ(command);
					}
				}
				else {//oracle
					foreach(long clinicNum in listClinicNums) {
						command="INSERT INTO programproperty (ProgramPropertyNum,ProgramNum,PropertyDesc,PropertyValue,ComputerName,ClinicNum) "
							+"VALUES ((SELECT MAX(ProgramPropertyNum)+1 FROM programproperty),"
							+POut.Long(payConnectProgNum)+",'IsOnlinePaymentsEnabled','0','',"+POut.Long(clinicNum)+")";
						Db.NonQ(command);
					}
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE site ADD Address varchar(100) NOT NULL";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE site ADD Address varchar2(100)";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE site ADD Address2 varchar(100) NOT NULL";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE site ADD Address2 varchar2(100)";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE site ADD City varchar(100) NOT NULL";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE site ADD City varchar2(100)";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE site ADD State varchar(100) NOT NULL";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE site ADD State varchar2(100)";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE site ADD Zip varchar(100) NOT NULL";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE site ADD Zip varchar2(100)";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE site ADD ProvNum bigint NOT NULL";
					Db.NonQ(command);
					command="ALTER TABLE site ADD INDEX (ProvNum)";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE site ADD ProvNum number(20)";
					Db.NonQ(command);
					command="UPDATE site SET ProvNum = 0 WHERE ProvNum IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE site MODIFY ProvNum NOT NULL";
					Db.NonQ(command);
					command=@"CREATE INDEX site_ProvNum ON site (ProvNum)";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE site ADD PlaceService tinyint NOT NULL";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE site ADD PlaceService number(3)";
					Db.NonQ(command);
					command="UPDATE site SET PlaceService = 0 WHERE PlaceService IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE site MODIFY PlaceService NOT NULL";
					Db.NonQ(command);
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="ALTER TABLE emailmessage ADD HideIn tinyint NOT NULL";
					Db.NonQ(command);
				}
				else {//oracle
					command="ALTER TABLE emailmessage ADD HideIn number(3)";
					Db.NonQ(command);
					command="UPDATE emailmessage SET HideIn = 0 WHERE HideIn IS NULL";
					Db.NonQ(command);
					command="ALTER TABLE emailmessage MODIFY HideIn NOT NULL";
					Db.NonQ(command);
				}
				command="UPDATE preference SET ValueString = '16.2.1.0' WHERE PrefName = 'DataBaseVersion'";
				Db.NonQ(command);
			}
			To16_3_0();
		}

		private static void To16_3_0() {
			if(FromVersion<new Version("16.3.0.0")) {
				ODEvent.Fire(new ODEventArgs("ConvertDatabases","Upgrading database to version: 16.3.0"));//No translation in convert script.
				string command;




				command="UPDATE preference SET ValueString = '16.3.0.0' WHERE PrefName = 'DataBaseVersion'";
				Db.NonQ(command);
			}
			//To16_4_0();
		}

	}
}
