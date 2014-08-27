//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class PatientCrud {
		///<summary>Gets one Patient object from the database using the primary key.  Returns null if not found.</summary>
		public static Patient SelectOne(long patNum){
			string command="SELECT * FROM patient "
				+"WHERE PatNum = "+POut.Long(patNum);
			List<Patient> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Patient object from the database using a query.</summary>
		public static Patient SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Patient> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Patient objects from the database using a query.</summary>
		public static List<Patient> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Patient> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Patient> TableToList(DataTable table){
			List<Patient> retVal=new List<Patient>();
			Patient patient;
			for(int i=0;i<table.Rows.Count;i++) {
				patient=new Patient();
				patient.PatNum                   = PIn.Long  (table.Rows[i]["PatNum"].ToString());
				patient.LName                    = PIn.String(table.Rows[i]["LName"].ToString());
				patient.FName                    = PIn.String(table.Rows[i]["FName"].ToString());
				patient.MiddleI                  = PIn.String(table.Rows[i]["MiddleI"].ToString());
				patient.Preferred                = PIn.String(table.Rows[i]["Preferred"].ToString());
				patient.PatStatus                = (PatientStatus)PIn.Int(table.Rows[i]["PatStatus"].ToString());
				patient.Gender                   = (PatientGender)PIn.Int(table.Rows[i]["Gender"].ToString());
				patient.Position                 = (PatientPosition)PIn.Int(table.Rows[i]["Position"].ToString());
				patient.Birthdate                = PIn.Date  (table.Rows[i]["Birthdate"].ToString());
				patient.SSN                      = PIn.String(table.Rows[i]["SSN"].ToString());
				patient.Address                  = PIn.String(table.Rows[i]["Address"].ToString());
				patient.Address2                 = PIn.String(table.Rows[i]["Address2"].ToString());
				patient.City                     = PIn.String(table.Rows[i]["City"].ToString());
				patient.State                    = PIn.String(table.Rows[i]["State"].ToString());
				patient.Zip                      = PIn.String(table.Rows[i]["Zip"].ToString());
				patient.HmPhone                  = PIn.String(table.Rows[i]["HmPhone"].ToString());
				patient.WkPhone                  = PIn.String(table.Rows[i]["WkPhone"].ToString());
				patient.WirelessPhone            = PIn.String(table.Rows[i]["WirelessPhone"].ToString());
				patient.Guarantor                = PIn.Long  (table.Rows[i]["Guarantor"].ToString());
				patient.CreditType               = PIn.String(table.Rows[i]["CreditType"].ToString());
				patient.Email                    = PIn.String(table.Rows[i]["Email"].ToString());
				patient.Salutation               = PIn.String(table.Rows[i]["Salutation"].ToString());
				patient.EstBalance               = PIn.Double(table.Rows[i]["EstBalance"].ToString());
				patient.PriProv                  = PIn.Long  (table.Rows[i]["PriProv"].ToString());
				patient.SecProv                  = PIn.Long  (table.Rows[i]["SecProv"].ToString());
				patient.FeeSched                 = PIn.Long  (table.Rows[i]["FeeSched"].ToString());
				patient.BillingType              = PIn.Long  (table.Rows[i]["BillingType"].ToString());
				patient.ImageFolder              = PIn.String(table.Rows[i]["ImageFolder"].ToString());
				patient.AddrNote                 = PIn.String(table.Rows[i]["AddrNote"].ToString());
				patient.FamFinUrgNote            = PIn.String(table.Rows[i]["FamFinUrgNote"].ToString());
				patient.MedUrgNote               = PIn.String(table.Rows[i]["MedUrgNote"].ToString());
				patient.ApptModNote              = PIn.String(table.Rows[i]["ApptModNote"].ToString());
				patient.StudentStatus            = PIn.String(table.Rows[i]["StudentStatus"].ToString());
				patient.SchoolName               = PIn.String(table.Rows[i]["SchoolName"].ToString());
				patient.ChartNumber              = PIn.String(table.Rows[i]["ChartNumber"].ToString());
				patient.MedicaidID               = PIn.String(table.Rows[i]["MedicaidID"].ToString());
				patient.Bal_0_30                 = PIn.Double(table.Rows[i]["Bal_0_30"].ToString());
				patient.Bal_31_60                = PIn.Double(table.Rows[i]["Bal_31_60"].ToString());
				patient.Bal_61_90                = PIn.Double(table.Rows[i]["Bal_61_90"].ToString());
				patient.BalOver90                = PIn.Double(table.Rows[i]["BalOver90"].ToString());
				patient.InsEst                   = PIn.Double(table.Rows[i]["InsEst"].ToString());
				patient.BalTotal                 = PIn.Double(table.Rows[i]["BalTotal"].ToString());
				patient.EmployerNum              = PIn.Long  (table.Rows[i]["EmployerNum"].ToString());
				patient.EmploymentNote           = PIn.String(table.Rows[i]["EmploymentNote"].ToString());
				patient.Race                     = (PatientRaceOld)PIn.Int(table.Rows[i]["Race"].ToString());
				patient.County                   = PIn.String(table.Rows[i]["County"].ToString());
				patient.GradeLevel               = (PatientGrade)PIn.Int(table.Rows[i]["GradeLevel"].ToString());
				patient.Urgency                  = (TreatmentUrgency)PIn.Int(table.Rows[i]["Urgency"].ToString());
				patient.DateFirstVisit           = PIn.Date  (table.Rows[i]["DateFirstVisit"].ToString());
				patient.ClinicNum                = PIn.Long  (table.Rows[i]["ClinicNum"].ToString());
				patient.HasIns                   = PIn.String(table.Rows[i]["HasIns"].ToString());
				patient.TrophyFolder             = PIn.String(table.Rows[i]["TrophyFolder"].ToString());
				patient.PlannedIsDone            = PIn.Bool  (table.Rows[i]["PlannedIsDone"].ToString());
				patient.Premed                   = PIn.Bool  (table.Rows[i]["Premed"].ToString());
				patient.Ward                     = PIn.String(table.Rows[i]["Ward"].ToString());
				patient.PreferConfirmMethod      = (ContactMethod)PIn.Int(table.Rows[i]["PreferConfirmMethod"].ToString());
				patient.PreferContactMethod      = (ContactMethod)PIn.Int(table.Rows[i]["PreferContactMethod"].ToString());
				patient.PreferRecallMethod       = (ContactMethod)PIn.Int(table.Rows[i]["PreferRecallMethod"].ToString());
				patient.SchedBeforeTime          = PIn.Time(table.Rows[i]["SchedBeforeTime"].ToString());
				patient.SchedAfterTime           = PIn.Time(table.Rows[i]["SchedAfterTime"].ToString());
				patient.SchedDayOfWeek           = PIn.Byte  (table.Rows[i]["SchedDayOfWeek"].ToString());
				patient.Language                 = PIn.String(table.Rows[i]["Language"].ToString());
				patient.AdmitDate                = PIn.Date  (table.Rows[i]["AdmitDate"].ToString());
				patient.Title                    = PIn.String(table.Rows[i]["Title"].ToString());
				patient.PayPlanDue               = PIn.Double(table.Rows[i]["PayPlanDue"].ToString());
				patient.SiteNum                  = PIn.Long  (table.Rows[i]["SiteNum"].ToString());
				patient.DateTStamp               = PIn.DateT (table.Rows[i]["DateTStamp"].ToString());
				patient.ResponsParty             = PIn.Long  (table.Rows[i]["ResponsParty"].ToString());
				patient.CanadianEligibilityCode  = PIn.Byte  (table.Rows[i]["CanadianEligibilityCode"].ToString());
				patient.AskToArriveEarly         = PIn.Int   (table.Rows[i]["AskToArriveEarly"].ToString());
				patient.OnlinePassword           = PIn.String(table.Rows[i]["OnlinePassword"].ToString());
				patient.PreferContactConfidential= (ContactMethod)PIn.Int(table.Rows[i]["PreferContactConfidential"].ToString());
				patient.SuperFamily              = PIn.Long  (table.Rows[i]["SuperFamily"].ToString());
				patient.TxtMsgOk                 = (YN)PIn.Int(table.Rows[i]["TxtMsgOk"].ToString());
				patient.SmokingSnoMed            = PIn.String(table.Rows[i]["SmokingSnoMed"].ToString());
				patient.Country                  = PIn.String(table.Rows[i]["Country"].ToString());
				patient.DateTimeDeceased         = PIn.DateT (table.Rows[i]["DateTimeDeceased"].ToString());
				retVal.Add(patient);
			}
			return retVal;
		}

		///<summary>Inserts one Patient into the database.  Returns the new priKey.</summary>
		public static long Insert(Patient patient){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				patient.PatNum=DbHelper.GetNextOracleKey("patient","PatNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(patient,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							patient.PatNum++;
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
				return Insert(patient,false);
			}
		}

		///<summary>Inserts one Patient into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Patient patient,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				patient.PatNum=ReplicationServers.GetKey("patient","PatNum");
			}
			string command="INSERT INTO patient (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="PatNum,";
			}
			command+="LName,FName,MiddleI,Preferred,PatStatus,Gender,Position,Birthdate,SSN,Address,Address2,City,State,Zip,HmPhone,WkPhone,WirelessPhone,Guarantor,CreditType,Email,Salutation,EstBalance,PriProv,SecProv,FeeSched,BillingType,ImageFolder,AddrNote,FamFinUrgNote,MedUrgNote,ApptModNote,StudentStatus,SchoolName,ChartNumber,MedicaidID,Bal_0_30,Bal_31_60,Bal_61_90,BalOver90,InsEst,BalTotal,EmployerNum,EmploymentNote,Race,County,GradeLevel,Urgency,DateFirstVisit,ClinicNum,HasIns,TrophyFolder,PlannedIsDone,Premed,Ward,PreferConfirmMethod,PreferContactMethod,PreferRecallMethod,SchedBeforeTime,SchedAfterTime,SchedDayOfWeek,Language,AdmitDate,Title,PayPlanDue,SiteNum,ResponsParty,CanadianEligibilityCode,AskToArriveEarly,OnlinePassword,PreferContactConfidential,SuperFamily,TxtMsgOk,SmokingSnoMed,Country,DateTimeDeceased) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(patient.PatNum)+",";
			}
			command+=
				 "'"+POut.String(patient.LName)+"',"
				+"'"+POut.String(patient.FName)+"',"
				+"'"+POut.String(patient.MiddleI)+"',"
				+"'"+POut.String(patient.Preferred)+"',"
				+    POut.Int   ((int)patient.PatStatus)+","
				+    POut.Int   ((int)patient.Gender)+","
				+    POut.Int   ((int)patient.Position)+","
				+    POut.Date  (patient.Birthdate)+","
				+"'"+POut.String(patient.SSN)+"',"
				+"'"+POut.String(patient.Address)+"',"
				+"'"+POut.String(patient.Address2)+"',"
				+"'"+POut.String(patient.City)+"',"
				+"'"+POut.String(patient.State)+"',"
				+"'"+POut.String(patient.Zip)+"',"
				+"'"+POut.String(patient.HmPhone)+"',"
				+"'"+POut.String(patient.WkPhone)+"',"
				+"'"+POut.String(patient.WirelessPhone)+"',"
				+    POut.Long  (patient.Guarantor)+","
				+"'"+POut.String(patient.CreditType)+"',"
				+"'"+POut.String(patient.Email)+"',"
				+"'"+POut.String(patient.Salutation)+"',"
				+"'"+POut.Double(patient.EstBalance)+"',"
				+    POut.Long  (patient.PriProv)+","
				+    POut.Long  (patient.SecProv)+","
				+    POut.Long  (patient.FeeSched)+","
				+    POut.Long  (patient.BillingType)+","
				+"'"+POut.String(patient.ImageFolder)+"',"
				+"'"+POut.String(patient.AddrNote)+"',"
				+    DbHelper.ParamChar+"paramFamFinUrgNote,"
				+"'"+POut.String(patient.MedUrgNote)+"',"
				+"'"+POut.String(patient.ApptModNote)+"',"
				+"'"+POut.String(patient.StudentStatus)+"',"
				+"'"+POut.String(patient.SchoolName)+"',"
				+"'"+POut.String(patient.ChartNumber)+"',"
				+"'"+POut.String(patient.MedicaidID)+"',"
				+"'"+POut.Double(patient.Bal_0_30)+"',"
				+"'"+POut.Double(patient.Bal_31_60)+"',"
				+"'"+POut.Double(patient.Bal_61_90)+"',"
				+"'"+POut.Double(patient.BalOver90)+"',"
				+"'"+POut.Double(patient.InsEst)+"',"
				+"'"+POut.Double(patient.BalTotal)+"',"
				+    POut.Long  (patient.EmployerNum)+","
				+"'"+POut.String(patient.EmploymentNote)+"',"
				+    POut.Int   ((int)patient.Race)+","
				+"'"+POut.String(patient.County)+"',"
				+    POut.Int   ((int)patient.GradeLevel)+","
				+    POut.Int   ((int)patient.Urgency)+","
				+    POut.Date  (patient.DateFirstVisit)+","
				+    POut.Long  (patient.ClinicNum)+","
				+"'"+POut.String(patient.HasIns)+"',"
				+"'"+POut.String(patient.TrophyFolder)+"',"
				+    POut.Bool  (patient.PlannedIsDone)+","
				+    POut.Bool  (patient.Premed)+","
				+"'"+POut.String(patient.Ward)+"',"
				+    POut.Int   ((int)patient.PreferConfirmMethod)+","
				+    POut.Int   ((int)patient.PreferContactMethod)+","
				+    POut.Int   ((int)patient.PreferRecallMethod)+","
				+    POut.Time  (patient.SchedBeforeTime)+","
				+    POut.Time  (patient.SchedAfterTime)+","
				+    POut.Byte  (patient.SchedDayOfWeek)+","
				+"'"+POut.String(patient.Language)+"',"
				+    POut.Date  (patient.AdmitDate)+","
				+"'"+POut.String(patient.Title)+"',"
				+"'"+POut.Double(patient.PayPlanDue)+"',"
				+    POut.Long  (patient.SiteNum)+","
				//DateTStamp can only be set by MySQL
				+    POut.Long  (patient.ResponsParty)+","
				+    POut.Byte  (patient.CanadianEligibilityCode)+","
				+    POut.Int   (patient.AskToArriveEarly)+","
				+"'"+POut.String(patient.OnlinePassword)+"',"
				+    POut.Int   ((int)patient.PreferContactConfidential)+","
				+    POut.Long  (patient.SuperFamily)+","
				+    POut.Int   ((int)patient.TxtMsgOk)+","
				+"'"+POut.String(patient.SmokingSnoMed)+"',"
				+"'"+POut.String(patient.Country)+"',"
				+    POut.DateT (patient.DateTimeDeceased)+")";
			if(patient.FamFinUrgNote==null) {
				patient.FamFinUrgNote="";
			}
			OdSqlParameter paramFamFinUrgNote=new OdSqlParameter("paramFamFinUrgNote",OdDbType.Text,POut.StringNote(patient.FamFinUrgNote));
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramFamFinUrgNote);
			}
			else {
				patient.PatNum=Db.NonQ(command,true,paramFamFinUrgNote);
			}
			return patient.PatNum;
		}

		///<summary>Updates one Patient in the database.</summary>
		public static void Update(Patient patient){
			string command="UPDATE patient SET "
				+"LName                    = '"+POut.String(patient.LName)+"', "
				+"FName                    = '"+POut.String(patient.FName)+"', "
				+"MiddleI                  = '"+POut.String(patient.MiddleI)+"', "
				+"Preferred                = '"+POut.String(patient.Preferred)+"', "
				+"PatStatus                =  "+POut.Int   ((int)patient.PatStatus)+", "
				+"Gender                   =  "+POut.Int   ((int)patient.Gender)+", "
				+"Position                 =  "+POut.Int   ((int)patient.Position)+", "
				+"Birthdate                =  "+POut.Date  (patient.Birthdate)+", "
				+"SSN                      = '"+POut.String(patient.SSN)+"', "
				+"Address                  = '"+POut.String(patient.Address)+"', "
				+"Address2                 = '"+POut.String(patient.Address2)+"', "
				+"City                     = '"+POut.String(patient.City)+"', "
				+"State                    = '"+POut.String(patient.State)+"', "
				+"Zip                      = '"+POut.String(patient.Zip)+"', "
				+"HmPhone                  = '"+POut.String(patient.HmPhone)+"', "
				+"WkPhone                  = '"+POut.String(patient.WkPhone)+"', "
				+"WirelessPhone            = '"+POut.String(patient.WirelessPhone)+"', "
				+"Guarantor                =  "+POut.Long  (patient.Guarantor)+", "
				+"CreditType               = '"+POut.String(patient.CreditType)+"', "
				+"Email                    = '"+POut.String(patient.Email)+"', "
				+"Salutation               = '"+POut.String(patient.Salutation)+"', "
				+"EstBalance               = '"+POut.Double(patient.EstBalance)+"', "
				+"PriProv                  =  "+POut.Long  (patient.PriProv)+", "
				+"SecProv                  =  "+POut.Long  (patient.SecProv)+", "
				+"FeeSched                 =  "+POut.Long  (patient.FeeSched)+", "
				+"BillingType              =  "+POut.Long  (patient.BillingType)+", "
				+"ImageFolder              = '"+POut.String(patient.ImageFolder)+"', "
				+"AddrNote                 = '"+POut.String(patient.AddrNote)+"', "
				+"FamFinUrgNote            =  "+DbHelper.ParamChar+"paramFamFinUrgNote, "
				+"MedUrgNote               = '"+POut.String(patient.MedUrgNote)+"', "
				+"ApptModNote              = '"+POut.String(patient.ApptModNote)+"', "
				+"StudentStatus            = '"+POut.String(patient.StudentStatus)+"', "
				+"SchoolName               = '"+POut.String(patient.SchoolName)+"', "
				+"ChartNumber              = '"+POut.String(patient.ChartNumber)+"', "
				+"MedicaidID               = '"+POut.String(patient.MedicaidID)+"', "
				+"Bal_0_30                 = '"+POut.Double(patient.Bal_0_30)+"', "
				+"Bal_31_60                = '"+POut.Double(patient.Bal_31_60)+"', "
				+"Bal_61_90                = '"+POut.Double(patient.Bal_61_90)+"', "
				+"BalOver90                = '"+POut.Double(patient.BalOver90)+"', "
				+"InsEst                   = '"+POut.Double(patient.InsEst)+"', "
				+"BalTotal                 = '"+POut.Double(patient.BalTotal)+"', "
				+"EmployerNum              =  "+POut.Long  (patient.EmployerNum)+", "
				+"EmploymentNote           = '"+POut.String(patient.EmploymentNote)+"', "
				+"Race                     =  "+POut.Int   ((int)patient.Race)+", "
				+"County                   = '"+POut.String(patient.County)+"', "
				+"GradeLevel               =  "+POut.Int   ((int)patient.GradeLevel)+", "
				+"Urgency                  =  "+POut.Int   ((int)patient.Urgency)+", "
				+"DateFirstVisit           =  "+POut.Date  (patient.DateFirstVisit)+", "
				+"ClinicNum                =  "+POut.Long  (patient.ClinicNum)+", "
				+"HasIns                   = '"+POut.String(patient.HasIns)+"', "
				+"TrophyFolder             = '"+POut.String(patient.TrophyFolder)+"', "
				+"PlannedIsDone            =  "+POut.Bool  (patient.PlannedIsDone)+", "
				+"Premed                   =  "+POut.Bool  (patient.Premed)+", "
				+"Ward                     = '"+POut.String(patient.Ward)+"', "
				+"PreferConfirmMethod      =  "+POut.Int   ((int)patient.PreferConfirmMethod)+", "
				+"PreferContactMethod      =  "+POut.Int   ((int)patient.PreferContactMethod)+", "
				+"PreferRecallMethod       =  "+POut.Int   ((int)patient.PreferRecallMethod)+", "
				+"SchedBeforeTime          =  "+POut.Time  (patient.SchedBeforeTime)+", "
				+"SchedAfterTime           =  "+POut.Time  (patient.SchedAfterTime)+", "
				+"SchedDayOfWeek           =  "+POut.Byte  (patient.SchedDayOfWeek)+", "
				+"Language                 = '"+POut.String(patient.Language)+"', "
				+"AdmitDate                =  "+POut.Date  (patient.AdmitDate)+", "
				+"Title                    = '"+POut.String(patient.Title)+"', "
				+"PayPlanDue               = '"+POut.Double(patient.PayPlanDue)+"', "
				+"SiteNum                  =  "+POut.Long  (patient.SiteNum)+", "
				//DateTStamp can only be set by MySQL
				+"ResponsParty             =  "+POut.Long  (patient.ResponsParty)+", "
				+"CanadianEligibilityCode  =  "+POut.Byte  (patient.CanadianEligibilityCode)+", "
				+"AskToArriveEarly         =  "+POut.Int   (patient.AskToArriveEarly)+", "
				+"OnlinePassword           = '"+POut.String(patient.OnlinePassword)+"', "
				+"PreferContactConfidential=  "+POut.Int   ((int)patient.PreferContactConfidential)+", "
				+"SuperFamily              =  "+POut.Long  (patient.SuperFamily)+", "
				+"TxtMsgOk                 =  "+POut.Int   ((int)patient.TxtMsgOk)+", "
				+"SmokingSnoMed            = '"+POut.String(patient.SmokingSnoMed)+"', "
				+"Country                  = '"+POut.String(patient.Country)+"', "
				+"DateTimeDeceased         =  "+POut.DateT (patient.DateTimeDeceased)+" "
				+"WHERE PatNum = "+POut.Long(patient.PatNum);
			if(patient.FamFinUrgNote==null) {
				patient.FamFinUrgNote="";
			}
			OdSqlParameter paramFamFinUrgNote=new OdSqlParameter("paramFamFinUrgNote",OdDbType.Text,POut.StringNote(patient.FamFinUrgNote));
			Db.NonQ(command,paramFamFinUrgNote);
		}

		///<summary>Updates one Patient in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Patient patient,Patient oldPatient){
			string command="";
			if(patient.LName != oldPatient.LName) {
				if(command!=""){ command+=",";}
				command+="LName = '"+POut.String(patient.LName)+"'";
			}
			if(patient.FName != oldPatient.FName) {
				if(command!=""){ command+=",";}
				command+="FName = '"+POut.String(patient.FName)+"'";
			}
			if(patient.MiddleI != oldPatient.MiddleI) {
				if(command!=""){ command+=",";}
				command+="MiddleI = '"+POut.String(patient.MiddleI)+"'";
			}
			if(patient.Preferred != oldPatient.Preferred) {
				if(command!=""){ command+=",";}
				command+="Preferred = '"+POut.String(patient.Preferred)+"'";
			}
			if(patient.PatStatus != oldPatient.PatStatus) {
				if(command!=""){ command+=",";}
				command+="PatStatus = "+POut.Int   ((int)patient.PatStatus)+"";
			}
			if(patient.Gender != oldPatient.Gender) {
				if(command!=""){ command+=",";}
				command+="Gender = "+POut.Int   ((int)patient.Gender)+"";
			}
			if(patient.Position != oldPatient.Position) {
				if(command!=""){ command+=",";}
				command+="Position = "+POut.Int   ((int)patient.Position)+"";
			}
			if(patient.Birthdate != oldPatient.Birthdate) {
				if(command!=""){ command+=",";}
				command+="Birthdate = "+POut.Date(patient.Birthdate)+"";
			}
			if(patient.SSN != oldPatient.SSN) {
				if(command!=""){ command+=",";}
				command+="SSN = '"+POut.String(patient.SSN)+"'";
			}
			if(patient.Address != oldPatient.Address) {
				if(command!=""){ command+=",";}
				command+="Address = '"+POut.String(patient.Address)+"'";
			}
			if(patient.Address2 != oldPatient.Address2) {
				if(command!=""){ command+=",";}
				command+="Address2 = '"+POut.String(patient.Address2)+"'";
			}
			if(patient.City != oldPatient.City) {
				if(command!=""){ command+=",";}
				command+="City = '"+POut.String(patient.City)+"'";
			}
			if(patient.State != oldPatient.State) {
				if(command!=""){ command+=",";}
				command+="State = '"+POut.String(patient.State)+"'";
			}
			if(patient.Zip != oldPatient.Zip) {
				if(command!=""){ command+=",";}
				command+="Zip = '"+POut.String(patient.Zip)+"'";
			}
			if(patient.HmPhone != oldPatient.HmPhone) {
				if(command!=""){ command+=",";}
				command+="HmPhone = '"+POut.String(patient.HmPhone)+"'";
			}
			if(patient.WkPhone != oldPatient.WkPhone) {
				if(command!=""){ command+=",";}
				command+="WkPhone = '"+POut.String(patient.WkPhone)+"'";
			}
			if(patient.WirelessPhone != oldPatient.WirelessPhone) {
				if(command!=""){ command+=",";}
				command+="WirelessPhone = '"+POut.String(patient.WirelessPhone)+"'";
			}
			if(patient.Guarantor != oldPatient.Guarantor) {
				if(command!=""){ command+=",";}
				command+="Guarantor = "+POut.Long(patient.Guarantor)+"";
			}
			if(patient.CreditType != oldPatient.CreditType) {
				if(command!=""){ command+=",";}
				command+="CreditType = '"+POut.String(patient.CreditType)+"'";
			}
			if(patient.Email != oldPatient.Email) {
				if(command!=""){ command+=",";}
				command+="Email = '"+POut.String(patient.Email)+"'";
			}
			if(patient.Salutation != oldPatient.Salutation) {
				if(command!=""){ command+=",";}
				command+="Salutation = '"+POut.String(patient.Salutation)+"'";
			}
			if(patient.EstBalance != oldPatient.EstBalance) {
				if(command!=""){ command+=",";}
				command+="EstBalance = '"+POut.Double(patient.EstBalance)+"'";
			}
			if(patient.PriProv != oldPatient.PriProv) {
				if(command!=""){ command+=",";}
				command+="PriProv = "+POut.Long(patient.PriProv)+"";
			}
			if(patient.SecProv != oldPatient.SecProv) {
				if(command!=""){ command+=",";}
				command+="SecProv = "+POut.Long(patient.SecProv)+"";
			}
			if(patient.FeeSched != oldPatient.FeeSched) {
				if(command!=""){ command+=",";}
				command+="FeeSched = "+POut.Long(patient.FeeSched)+"";
			}
			if(patient.BillingType != oldPatient.BillingType) {
				if(command!=""){ command+=",";}
				command+="BillingType = "+POut.Long(patient.BillingType)+"";
			}
			if(patient.ImageFolder != oldPatient.ImageFolder) {
				if(command!=""){ command+=",";}
				command+="ImageFolder = '"+POut.String(patient.ImageFolder)+"'";
			}
			if(patient.AddrNote != oldPatient.AddrNote) {
				if(command!=""){ command+=",";}
				command+="AddrNote = '"+POut.String(patient.AddrNote)+"'";
			}
			if(patient.FamFinUrgNote != oldPatient.FamFinUrgNote) {
				if(command!=""){ command+=",";}
				command+="FamFinUrgNote = "+DbHelper.ParamChar+"paramFamFinUrgNote";
			}
			if(patient.MedUrgNote != oldPatient.MedUrgNote) {
				if(command!=""){ command+=",";}
				command+="MedUrgNote = '"+POut.String(patient.MedUrgNote)+"'";
			}
			if(patient.ApptModNote != oldPatient.ApptModNote) {
				if(command!=""){ command+=",";}
				command+="ApptModNote = '"+POut.String(patient.ApptModNote)+"'";
			}
			if(patient.StudentStatus != oldPatient.StudentStatus) {
				if(command!=""){ command+=",";}
				command+="StudentStatus = '"+POut.String(patient.StudentStatus)+"'";
			}
			if(patient.SchoolName != oldPatient.SchoolName) {
				if(command!=""){ command+=",";}
				command+="SchoolName = '"+POut.String(patient.SchoolName)+"'";
			}
			if(patient.ChartNumber != oldPatient.ChartNumber) {
				if(command!=""){ command+=",";}
				command+="ChartNumber = '"+POut.String(patient.ChartNumber)+"'";
			}
			if(patient.MedicaidID != oldPatient.MedicaidID) {
				if(command!=""){ command+=",";}
				command+="MedicaidID = '"+POut.String(patient.MedicaidID)+"'";
			}
			if(patient.Bal_0_30 != oldPatient.Bal_0_30) {
				if(command!=""){ command+=",";}
				command+="Bal_0_30 = '"+POut.Double(patient.Bal_0_30)+"'";
			}
			if(patient.Bal_31_60 != oldPatient.Bal_31_60) {
				if(command!=""){ command+=",";}
				command+="Bal_31_60 = '"+POut.Double(patient.Bal_31_60)+"'";
			}
			if(patient.Bal_61_90 != oldPatient.Bal_61_90) {
				if(command!=""){ command+=",";}
				command+="Bal_61_90 = '"+POut.Double(patient.Bal_61_90)+"'";
			}
			if(patient.BalOver90 != oldPatient.BalOver90) {
				if(command!=""){ command+=",";}
				command+="BalOver90 = '"+POut.Double(patient.BalOver90)+"'";
			}
			if(patient.InsEst != oldPatient.InsEst) {
				if(command!=""){ command+=",";}
				command+="InsEst = '"+POut.Double(patient.InsEst)+"'";
			}
			if(patient.BalTotal != oldPatient.BalTotal) {
				if(command!=""){ command+=",";}
				command+="BalTotal = '"+POut.Double(patient.BalTotal)+"'";
			}
			if(patient.EmployerNum != oldPatient.EmployerNum) {
				if(command!=""){ command+=",";}
				command+="EmployerNum = "+POut.Long(patient.EmployerNum)+"";
			}
			if(patient.EmploymentNote != oldPatient.EmploymentNote) {
				if(command!=""){ command+=",";}
				command+="EmploymentNote = '"+POut.String(patient.EmploymentNote)+"'";
			}
			if(patient.Race != oldPatient.Race) {
				if(command!=""){ command+=",";}
				command+="Race = "+POut.Int   ((int)patient.Race)+"";
			}
			if(patient.County != oldPatient.County) {
				if(command!=""){ command+=",";}
				command+="County = '"+POut.String(patient.County)+"'";
			}
			if(patient.GradeLevel != oldPatient.GradeLevel) {
				if(command!=""){ command+=",";}
				command+="GradeLevel = "+POut.Int   ((int)patient.GradeLevel)+"";
			}
			if(patient.Urgency != oldPatient.Urgency) {
				if(command!=""){ command+=",";}
				command+="Urgency = "+POut.Int   ((int)patient.Urgency)+"";
			}
			if(patient.DateFirstVisit != oldPatient.DateFirstVisit) {
				if(command!=""){ command+=",";}
				command+="DateFirstVisit = "+POut.Date(patient.DateFirstVisit)+"";
			}
			if(patient.ClinicNum != oldPatient.ClinicNum) {
				if(command!=""){ command+=",";}
				command+="ClinicNum = "+POut.Long(patient.ClinicNum)+"";
			}
			if(patient.HasIns != oldPatient.HasIns) {
				if(command!=""){ command+=",";}
				command+="HasIns = '"+POut.String(patient.HasIns)+"'";
			}
			if(patient.TrophyFolder != oldPatient.TrophyFolder) {
				if(command!=""){ command+=",";}
				command+="TrophyFolder = '"+POut.String(patient.TrophyFolder)+"'";
			}
			if(patient.PlannedIsDone != oldPatient.PlannedIsDone) {
				if(command!=""){ command+=",";}
				command+="PlannedIsDone = "+POut.Bool(patient.PlannedIsDone)+"";
			}
			if(patient.Premed != oldPatient.Premed) {
				if(command!=""){ command+=",";}
				command+="Premed = "+POut.Bool(patient.Premed)+"";
			}
			if(patient.Ward != oldPatient.Ward) {
				if(command!=""){ command+=",";}
				command+="Ward = '"+POut.String(patient.Ward)+"'";
			}
			if(patient.PreferConfirmMethod != oldPatient.PreferConfirmMethod) {
				if(command!=""){ command+=",";}
				command+="PreferConfirmMethod = "+POut.Int   ((int)patient.PreferConfirmMethod)+"";
			}
			if(patient.PreferContactMethod != oldPatient.PreferContactMethod) {
				if(command!=""){ command+=",";}
				command+="PreferContactMethod = "+POut.Int   ((int)patient.PreferContactMethod)+"";
			}
			if(patient.PreferRecallMethod != oldPatient.PreferRecallMethod) {
				if(command!=""){ command+=",";}
				command+="PreferRecallMethod = "+POut.Int   ((int)patient.PreferRecallMethod)+"";
			}
			if(patient.SchedBeforeTime != oldPatient.SchedBeforeTime) {
				if(command!=""){ command+=",";}
				command+="SchedBeforeTime = "+POut.Time  (patient.SchedBeforeTime)+"";
			}
			if(patient.SchedAfterTime != oldPatient.SchedAfterTime) {
				if(command!=""){ command+=",";}
				command+="SchedAfterTime = "+POut.Time  (patient.SchedAfterTime)+"";
			}
			if(patient.SchedDayOfWeek != oldPatient.SchedDayOfWeek) {
				if(command!=""){ command+=",";}
				command+="SchedDayOfWeek = "+POut.Byte(patient.SchedDayOfWeek)+"";
			}
			if(patient.Language != oldPatient.Language) {
				if(command!=""){ command+=",";}
				command+="Language = '"+POut.String(patient.Language)+"'";
			}
			if(patient.AdmitDate != oldPatient.AdmitDate) {
				if(command!=""){ command+=",";}
				command+="AdmitDate = "+POut.Date(patient.AdmitDate)+"";
			}
			if(patient.Title != oldPatient.Title) {
				if(command!=""){ command+=",";}
				command+="Title = '"+POut.String(patient.Title)+"'";
			}
			if(patient.PayPlanDue != oldPatient.PayPlanDue) {
				if(command!=""){ command+=",";}
				command+="PayPlanDue = '"+POut.Double(patient.PayPlanDue)+"'";
			}
			if(patient.SiteNum != oldPatient.SiteNum) {
				if(command!=""){ command+=",";}
				command+="SiteNum = "+POut.Long(patient.SiteNum)+"";
			}
			//DateTStamp can only be set by MySQL
			if(patient.ResponsParty != oldPatient.ResponsParty) {
				if(command!=""){ command+=",";}
				command+="ResponsParty = "+POut.Long(patient.ResponsParty)+"";
			}
			if(patient.CanadianEligibilityCode != oldPatient.CanadianEligibilityCode) {
				if(command!=""){ command+=",";}
				command+="CanadianEligibilityCode = "+POut.Byte(patient.CanadianEligibilityCode)+"";
			}
			if(patient.AskToArriveEarly != oldPatient.AskToArriveEarly) {
				if(command!=""){ command+=",";}
				command+="AskToArriveEarly = "+POut.Int(patient.AskToArriveEarly)+"";
			}
			if(patient.OnlinePassword != oldPatient.OnlinePassword) {
				if(command!=""){ command+=",";}
				command+="OnlinePassword = '"+POut.String(patient.OnlinePassword)+"'";
			}
			if(patient.PreferContactConfidential != oldPatient.PreferContactConfidential) {
				if(command!=""){ command+=",";}
				command+="PreferContactConfidential = "+POut.Int   ((int)patient.PreferContactConfidential)+"";
			}
			if(patient.SuperFamily != oldPatient.SuperFamily) {
				if(command!=""){ command+=",";}
				command+="SuperFamily = "+POut.Long(patient.SuperFamily)+"";
			}
			if(patient.TxtMsgOk != oldPatient.TxtMsgOk) {
				if(command!=""){ command+=",";}
				command+="TxtMsgOk = "+POut.Int   ((int)patient.TxtMsgOk)+"";
			}
			if(patient.SmokingSnoMed != oldPatient.SmokingSnoMed) {
				if(command!=""){ command+=",";}
				command+="SmokingSnoMed = '"+POut.String(patient.SmokingSnoMed)+"'";
			}
			if(patient.Country != oldPatient.Country) {
				if(command!=""){ command+=",";}
				command+="Country = '"+POut.String(patient.Country)+"'";
			}
			if(patient.DateTimeDeceased != oldPatient.DateTimeDeceased) {
				if(command!=""){ command+=",";}
				command+="DateTimeDeceased = "+POut.DateT(patient.DateTimeDeceased)+"";
			}
			if(command==""){
				return false;
			}
			if(patient.FamFinUrgNote==null) {
				patient.FamFinUrgNote="";
			}
			OdSqlParameter paramFamFinUrgNote=new OdSqlParameter("paramFamFinUrgNote",OdDbType.Text,POut.StringNote(patient.FamFinUrgNote));
			command="UPDATE patient SET "+command
				+" WHERE PatNum = "+POut.Long(patient.PatNum);
			Db.NonQ(command,paramFamFinUrgNote);
			return true;
		}

		///<summary>Deletes one Patient from the database.</summary>
		public static void Delete(long patNum){
			string command="DELETE FROM patient "
				+"WHERE PatNum = "+POut.Long(patNum);
			Db.NonQ(command);
		}

	}
}