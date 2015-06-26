//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class ProcedureCrud {
		///<summary>Gets one Procedure object from the database using the primary key.  Returns null if not found.</summary>
		public static Procedure SelectOne(long procNum){
			string command="SELECT * FROM procedurelog "
				+"WHERE ProcNum = "+POut.Long(procNum);
			List<Procedure> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Procedure object from the database using a query.</summary>
		public static Procedure SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Procedure> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Procedure objects from the database using a query.</summary>
		public static List<Procedure> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Procedure> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Procedure> TableToList(DataTable table){
			List<Procedure> retVal=new List<Procedure>();
			Procedure procedure;
			for(int i=0;i<table.Rows.Count;i++) {
				procedure=new Procedure();
				procedure.ProcNum           = PIn.Long  (table.Rows[i]["ProcNum"].ToString());
				procedure.PatNum            = PIn.Long  (table.Rows[i]["PatNum"].ToString());
				procedure.AptNum            = PIn.Long  (table.Rows[i]["AptNum"].ToString());
				procedure.OldCode           = PIn.String(table.Rows[i]["OldCode"].ToString());
				procedure.ProcDate          = PIn.Date  (table.Rows[i]["ProcDate"].ToString());
				procedure.ProcFee           = PIn.Double(table.Rows[i]["ProcFee"].ToString());
				procedure.Surf              = PIn.String(table.Rows[i]["Surf"].ToString());
				procedure.ToothNum          = PIn.String(table.Rows[i]["ToothNum"].ToString());
				procedure.ToothRange        = PIn.String(table.Rows[i]["ToothRange"].ToString());
				procedure.Priority          = PIn.Long  (table.Rows[i]["Priority"].ToString());
				procedure.ProcStatus        = (OpenDentBusiness.ProcStat)PIn.Int(table.Rows[i]["ProcStatus"].ToString());
				procedure.ProvNum           = PIn.Long  (table.Rows[i]["ProvNum"].ToString());
				procedure.Dx                = PIn.Long  (table.Rows[i]["Dx"].ToString());
				procedure.PlannedAptNum     = PIn.Long  (table.Rows[i]["PlannedAptNum"].ToString());
				procedure.PlaceService      = (OpenDentBusiness.PlaceOfService)PIn.Int(table.Rows[i]["PlaceService"].ToString());
				procedure.Prosthesis        = PIn.String(table.Rows[i]["Prosthesis"].ToString());
				procedure.DateOriginalProsth= PIn.Date  (table.Rows[i]["DateOriginalProsth"].ToString());
				procedure.ClaimNote         = PIn.String(table.Rows[i]["ClaimNote"].ToString());
				procedure.DateEntryC        = PIn.Date  (table.Rows[i]["DateEntryC"].ToString());
				procedure.ClinicNum         = PIn.Long  (table.Rows[i]["ClinicNum"].ToString());
				procedure.MedicalCode       = PIn.String(table.Rows[i]["MedicalCode"].ToString());
				procedure.DiagnosticCode    = PIn.String(table.Rows[i]["DiagnosticCode"].ToString());
				procedure.IsPrincDiag       = PIn.Bool  (table.Rows[i]["IsPrincDiag"].ToString());
				procedure.ProcNumLab        = PIn.Long  (table.Rows[i]["ProcNumLab"].ToString());
				procedure.BillingTypeOne    = PIn.Long  (table.Rows[i]["BillingTypeOne"].ToString());
				procedure.BillingTypeTwo    = PIn.Long  (table.Rows[i]["BillingTypeTwo"].ToString());
				procedure.CodeNum           = PIn.Long  (table.Rows[i]["CodeNum"].ToString());
				procedure.CodeMod1          = PIn.String(table.Rows[i]["CodeMod1"].ToString());
				procedure.CodeMod2          = PIn.String(table.Rows[i]["CodeMod2"].ToString());
				procedure.CodeMod3          = PIn.String(table.Rows[i]["CodeMod3"].ToString());
				procedure.CodeMod4          = PIn.String(table.Rows[i]["CodeMod4"].ToString());
				procedure.RevCode           = PIn.String(table.Rows[i]["RevCode"].ToString());
				procedure.UnitQty           = PIn.Int   (table.Rows[i]["UnitQty"].ToString());
				procedure.BaseUnits         = PIn.Int   (table.Rows[i]["BaseUnits"].ToString());
				procedure.StartTime         = PIn.Int   (table.Rows[i]["StartTime"].ToString());
				procedure.StopTime          = PIn.Int   (table.Rows[i]["StopTime"].ToString());
				procedure.DateTP            = PIn.Date  (table.Rows[i]["DateTP"].ToString());
				procedure.SiteNum           = PIn.Long  (table.Rows[i]["SiteNum"].ToString());
				procedure.HideGraphics      = PIn.Bool  (table.Rows[i]["HideGraphics"].ToString());
				procedure.CanadianTypeCodes = PIn.String(table.Rows[i]["CanadianTypeCodes"].ToString());
				procedure.ProcTime          = PIn.Time(table.Rows[i]["ProcTime"].ToString());
				procedure.ProcTimeEnd       = PIn.Time(table.Rows[i]["ProcTimeEnd"].ToString());
				procedure.DateTStamp        = PIn.DateT (table.Rows[i]["DateTStamp"].ToString());
				procedure.Prognosis         = PIn.Long  (table.Rows[i]["Prognosis"].ToString());
				procedure.DrugUnit          = (OpenDentBusiness.EnumProcDrugUnit)PIn.Int(table.Rows[i]["DrugUnit"].ToString());
				procedure.DrugQty           = PIn.Float (table.Rows[i]["DrugQty"].ToString());
				procedure.UnitQtyType       = (OpenDentBusiness.ProcUnitQtyType)PIn.Int(table.Rows[i]["UnitQtyType"].ToString());
				procedure.StatementNum      = PIn.Long  (table.Rows[i]["StatementNum"].ToString());
				procedure.IsLocked          = PIn.Bool  (table.Rows[i]["IsLocked"].ToString());
				procedure.BillingNote       = PIn.String(table.Rows[i]["BillingNote"].ToString());
				procedure.RepeatChargeNum   = PIn.Long  (table.Rows[i]["RepeatChargeNum"].ToString());
				procedure.DiagnosticCode2   = PIn.String(table.Rows[i]["DiagnosticCode2"].ToString());
				procedure.DiagnosticCode3   = PIn.String(table.Rows[i]["DiagnosticCode3"].ToString());
				procedure.DiagnosticCode4   = PIn.String(table.Rows[i]["DiagnosticCode4"].ToString());
				procedure.Discount          = PIn.Double(table.Rows[i]["Discount"].ToString());
				procedure.SnomedBodySite    = PIn.String(table.Rows[i]["SnomedBodySite"].ToString());
				procedure.ProvOrderOverride = PIn.Long  (table.Rows[i]["ProvOrderOverride"].ToString());
				procedure.IsDateProsthEst   = PIn.Bool  (table.Rows[i]["IsDateProsthEst"].ToString());
				procedure.IcdVersion        = PIn.Byte  (table.Rows[i]["IcdVersion"].ToString());
				retVal.Add(procedure);
			}
			return retVal;
		}

		///<summary>Inserts one Procedure into the database.  Returns the new priKey.</summary>
		public static long Insert(Procedure procedure){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				procedure.ProcNum=DbHelper.GetNextOracleKey("procedurelog","ProcNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(procedure,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							procedure.ProcNum++;
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
				return Insert(procedure,false);
			}
		}

		///<summary>Inserts one Procedure into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Procedure procedure,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				procedure.ProcNum=ReplicationServers.GetKey("procedurelog","ProcNum");
			}
			string command="INSERT INTO procedurelog (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ProcNum,";
			}
			command+="PatNum,AptNum,OldCode,ProcDate,ProcFee,Surf,ToothNum,ToothRange,Priority,ProcStatus,ProvNum,Dx,PlannedAptNum,PlaceService,Prosthesis,DateOriginalProsth,ClaimNote,DateEntryC,ClinicNum,MedicalCode,DiagnosticCode,IsPrincDiag,ProcNumLab,BillingTypeOne,BillingTypeTwo,CodeNum,CodeMod1,CodeMod2,CodeMod3,CodeMod4,RevCode,UnitQty,BaseUnits,StartTime,StopTime,DateTP,SiteNum,HideGraphics,CanadianTypeCodes,ProcTime,ProcTimeEnd,Prognosis,DrugUnit,DrugQty,UnitQtyType,StatementNum,IsLocked,BillingNote,RepeatChargeNum,DiagnosticCode2,DiagnosticCode3,DiagnosticCode4,Discount,SnomedBodySite,ProvOrderOverride,IsDateProsthEst,IcdVersion) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(procedure.ProcNum)+",";
			}
			command+=
				     POut.Long  (procedure.PatNum)+","
				+    POut.Long  (procedure.AptNum)+","
				+"'"+POut.String(procedure.OldCode)+"',"
				+    POut.Date  (procedure.ProcDate)+","
				+"'"+POut.Double(procedure.ProcFee)+"',"
				+"'"+POut.String(procedure.Surf)+"',"
				+"'"+POut.String(procedure.ToothNum)+"',"
				+"'"+POut.String(procedure.ToothRange)+"',"
				+    POut.Long  (procedure.Priority)+","
				+    POut.Int   ((int)procedure.ProcStatus)+","
				+    POut.Long  (procedure.ProvNum)+","
				+    POut.Long  (procedure.Dx)+","
				+    POut.Long  (procedure.PlannedAptNum)+","
				+    POut.Int   ((int)procedure.PlaceService)+","
				+"'"+POut.String(procedure.Prosthesis)+"',"
				+    POut.Date  (procedure.DateOriginalProsth)+","
				+"'"+POut.String(procedure.ClaimNote)+"',"
				+    DbHelper.Now()+","
				+    POut.Long  (procedure.ClinicNum)+","
				+"'"+POut.String(procedure.MedicalCode)+"',"
				+"'"+POut.String(procedure.DiagnosticCode)+"',"
				+    POut.Bool  (procedure.IsPrincDiag)+","
				+    POut.Long  (procedure.ProcNumLab)+","
				+    POut.Long  (procedure.BillingTypeOne)+","
				+    POut.Long  (procedure.BillingTypeTwo)+","
				+    POut.Long  (procedure.CodeNum)+","
				+"'"+POut.String(procedure.CodeMod1)+"',"
				+"'"+POut.String(procedure.CodeMod2)+"',"
				+"'"+POut.String(procedure.CodeMod3)+"',"
				+"'"+POut.String(procedure.CodeMod4)+"',"
				+"'"+POut.String(procedure.RevCode)+"',"
				+    POut.Int   (procedure.UnitQty)+","
				+    POut.Int   (procedure.BaseUnits)+","
				+    POut.Int   (procedure.StartTime)+","
				+    POut.Int   (procedure.StopTime)+","
				+    POut.Date  (procedure.DateTP)+","
				+    POut.Long  (procedure.SiteNum)+","
				+    POut.Bool  (procedure.HideGraphics)+","
				+"'"+POut.String(procedure.CanadianTypeCodes)+"',"
				+    POut.Time  (procedure.ProcTime)+","
				+    POut.Time  (procedure.ProcTimeEnd)+","
				//DateTStamp can only be set by MySQL
				+    POut.Long  (procedure.Prognosis)+","
				+    POut.Int   ((int)procedure.DrugUnit)+","
				+    POut.Float (procedure.DrugQty)+","
				+    POut.Int   ((int)procedure.UnitQtyType)+","
				+    POut.Long  (procedure.StatementNum)+","
				+    POut.Bool  (procedure.IsLocked)+","
				+"'"+POut.String(procedure.BillingNote)+"',"
				+    POut.Long  (procedure.RepeatChargeNum)+","
				+"'"+POut.String(procedure.DiagnosticCode2)+"',"
				+"'"+POut.String(procedure.DiagnosticCode3)+"',"
				+"'"+POut.String(procedure.DiagnosticCode4)+"',"
				+"'"+POut.Double(procedure.Discount)+"',"
				+"'"+POut.String(procedure.SnomedBodySite)+"',"
				+    POut.Long  (procedure.ProvOrderOverride)+","
				+    POut.Bool  (procedure.IsDateProsthEst)+","
				+    POut.Byte  (procedure.IcdVersion)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				procedure.ProcNum=Db.NonQ(command,true);
			}
			return procedure.ProcNum;
		}

		///<summary>Inserts one Procedure into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Procedure procedure){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(procedure,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					procedure.ProcNum=DbHelper.GetNextOracleKey("procedurelog","ProcNum"); //Cacheless method
				}
				return InsertNoCache(procedure,true);
			}
		}

		///<summary>Inserts one Procedure into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Procedure procedure,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO procedurelog (";
			if(!useExistingPK && isRandomKeys) {
				procedure.ProcNum=ReplicationServers.GetKeyNoCache("procedurelog","ProcNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ProcNum,";
			}
			command+="PatNum,AptNum,OldCode,ProcDate,ProcFee,Surf,ToothNum,ToothRange,Priority,ProcStatus,ProvNum,Dx,PlannedAptNum,PlaceService,Prosthesis,DateOriginalProsth,ClaimNote,DateEntryC,ClinicNum,MedicalCode,DiagnosticCode,IsPrincDiag,ProcNumLab,BillingTypeOne,BillingTypeTwo,CodeNum,CodeMod1,CodeMod2,CodeMod3,CodeMod4,RevCode,UnitQty,BaseUnits,StartTime,StopTime,DateTP,SiteNum,HideGraphics,CanadianTypeCodes,ProcTime,ProcTimeEnd,Prognosis,DrugUnit,DrugQty,UnitQtyType,StatementNum,IsLocked,BillingNote,RepeatChargeNum,DiagnosticCode2,DiagnosticCode3,DiagnosticCode4,Discount,SnomedBodySite,ProvOrderOverride,IsDateProsthEst,IcdVersion) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(procedure.ProcNum)+",";
			}
			command+=
				     POut.Long  (procedure.PatNum)+","
				+    POut.Long  (procedure.AptNum)+","
				+"'"+POut.String(procedure.OldCode)+"',"
				+    POut.Date  (procedure.ProcDate)+","
				+"'"+POut.Double(procedure.ProcFee)+"',"
				+"'"+POut.String(procedure.Surf)+"',"
				+"'"+POut.String(procedure.ToothNum)+"',"
				+"'"+POut.String(procedure.ToothRange)+"',"
				+    POut.Long  (procedure.Priority)+","
				+    POut.Int   ((int)procedure.ProcStatus)+","
				+    POut.Long  (procedure.ProvNum)+","
				+    POut.Long  (procedure.Dx)+","
				+    POut.Long  (procedure.PlannedAptNum)+","
				+    POut.Int   ((int)procedure.PlaceService)+","
				+"'"+POut.String(procedure.Prosthesis)+"',"
				+    POut.Date  (procedure.DateOriginalProsth)+","
				+"'"+POut.String(procedure.ClaimNote)+"',"
				+    DbHelper.Now()+","
				+    POut.Long  (procedure.ClinicNum)+","
				+"'"+POut.String(procedure.MedicalCode)+"',"
				+"'"+POut.String(procedure.DiagnosticCode)+"',"
				+    POut.Bool  (procedure.IsPrincDiag)+","
				+    POut.Long  (procedure.ProcNumLab)+","
				+    POut.Long  (procedure.BillingTypeOne)+","
				+    POut.Long  (procedure.BillingTypeTwo)+","
				+    POut.Long  (procedure.CodeNum)+","
				+"'"+POut.String(procedure.CodeMod1)+"',"
				+"'"+POut.String(procedure.CodeMod2)+"',"
				+"'"+POut.String(procedure.CodeMod3)+"',"
				+"'"+POut.String(procedure.CodeMod4)+"',"
				+"'"+POut.String(procedure.RevCode)+"',"
				+    POut.Int   (procedure.UnitQty)+","
				+    POut.Int   (procedure.BaseUnits)+","
				+    POut.Int   (procedure.StartTime)+","
				+    POut.Int   (procedure.StopTime)+","
				+    POut.Date  (procedure.DateTP)+","
				+    POut.Long  (procedure.SiteNum)+","
				+    POut.Bool  (procedure.HideGraphics)+","
				+"'"+POut.String(procedure.CanadianTypeCodes)+"',"
				+    POut.Time  (procedure.ProcTime)+","
				+    POut.Time  (procedure.ProcTimeEnd)+","
				//DateTStamp can only be set by MySQL
				+    POut.Long  (procedure.Prognosis)+","
				+    POut.Int   ((int)procedure.DrugUnit)+","
				+    POut.Float (procedure.DrugQty)+","
				+    POut.Int   ((int)procedure.UnitQtyType)+","
				+    POut.Long  (procedure.StatementNum)+","
				+    POut.Bool  (procedure.IsLocked)+","
				+"'"+POut.String(procedure.BillingNote)+"',"
				+    POut.Long  (procedure.RepeatChargeNum)+","
				+"'"+POut.String(procedure.DiagnosticCode2)+"',"
				+"'"+POut.String(procedure.DiagnosticCode3)+"',"
				+"'"+POut.String(procedure.DiagnosticCode4)+"',"
				+"'"+POut.Double(procedure.Discount)+"',"
				+"'"+POut.String(procedure.SnomedBodySite)+"',"
				+    POut.Long  (procedure.ProvOrderOverride)+","
				+    POut.Bool  (procedure.IsDateProsthEst)+","
				+    POut.Byte  (procedure.IcdVersion)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				procedure.ProcNum=Db.NonQ(command,true);
			}
			return procedure.ProcNum;
		}

		///<summary>Updates one Procedure in the database.</summary>
		public static void Update(Procedure procedure){
			string command="UPDATE procedurelog SET "
				+"PatNum            =  "+POut.Long  (procedure.PatNum)+", "
				+"AptNum            =  "+POut.Long  (procedure.AptNum)+", "
				+"OldCode           = '"+POut.String(procedure.OldCode)+"', "
				+"ProcDate          =  "+POut.Date  (procedure.ProcDate)+", "
				+"ProcFee           = '"+POut.Double(procedure.ProcFee)+"', "
				+"Surf              = '"+POut.String(procedure.Surf)+"', "
				+"ToothNum          = '"+POut.String(procedure.ToothNum)+"', "
				+"ToothRange        = '"+POut.String(procedure.ToothRange)+"', "
				+"Priority          =  "+POut.Long  (procedure.Priority)+", "
				+"ProcStatus        =  "+POut.Int   ((int)procedure.ProcStatus)+", "
				+"ProvNum           =  "+POut.Long  (procedure.ProvNum)+", "
				+"Dx                =  "+POut.Long  (procedure.Dx)+", "
				+"PlannedAptNum     =  "+POut.Long  (procedure.PlannedAptNum)+", "
				+"PlaceService      =  "+POut.Int   ((int)procedure.PlaceService)+", "
				+"Prosthesis        = '"+POut.String(procedure.Prosthesis)+"', "
				+"DateOriginalProsth=  "+POut.Date  (procedure.DateOriginalProsth)+", "
				+"ClaimNote         = '"+POut.String(procedure.ClaimNote)+"', "
				+"DateEntryC        =  "+POut.Date  (procedure.DateEntryC)+", "
				+"ClinicNum         =  "+POut.Long  (procedure.ClinicNum)+", "
				+"MedicalCode       = '"+POut.String(procedure.MedicalCode)+"', "
				+"DiagnosticCode    = '"+POut.String(procedure.DiagnosticCode)+"', "
				+"IsPrincDiag       =  "+POut.Bool  (procedure.IsPrincDiag)+", "
				+"ProcNumLab        =  "+POut.Long  (procedure.ProcNumLab)+", "
				+"BillingTypeOne    =  "+POut.Long  (procedure.BillingTypeOne)+", "
				+"BillingTypeTwo    =  "+POut.Long  (procedure.BillingTypeTwo)+", "
				+"CodeNum           =  "+POut.Long  (procedure.CodeNum)+", "
				+"CodeMod1          = '"+POut.String(procedure.CodeMod1)+"', "
				+"CodeMod2          = '"+POut.String(procedure.CodeMod2)+"', "
				+"CodeMod3          = '"+POut.String(procedure.CodeMod3)+"', "
				+"CodeMod4          = '"+POut.String(procedure.CodeMod4)+"', "
				+"RevCode           = '"+POut.String(procedure.RevCode)+"', "
				+"UnitQty           =  "+POut.Int   (procedure.UnitQty)+", "
				+"BaseUnits         =  "+POut.Int   (procedure.BaseUnits)+", "
				+"StartTime         =  "+POut.Int   (procedure.StartTime)+", "
				+"StopTime          =  "+POut.Int   (procedure.StopTime)+", "
				+"DateTP            =  "+POut.Date  (procedure.DateTP)+", "
				+"SiteNum           =  "+POut.Long  (procedure.SiteNum)+", "
				+"HideGraphics      =  "+POut.Bool  (procedure.HideGraphics)+", "
				+"CanadianTypeCodes = '"+POut.String(procedure.CanadianTypeCodes)+"', "
				+"ProcTime          =  "+POut.Time  (procedure.ProcTime)+", "
				+"ProcTimeEnd       =  "+POut.Time  (procedure.ProcTimeEnd)+", "
				//DateTStamp can only be set by MySQL
				+"Prognosis         =  "+POut.Long  (procedure.Prognosis)+", "
				+"DrugUnit          =  "+POut.Int   ((int)procedure.DrugUnit)+", "
				+"DrugQty           =  "+POut.Float (procedure.DrugQty)+", "
				+"UnitQtyType       =  "+POut.Int   ((int)procedure.UnitQtyType)+", "
				+"StatementNum      =  "+POut.Long  (procedure.StatementNum)+", "
				+"IsLocked          =  "+POut.Bool  (procedure.IsLocked)+", "
				+"BillingNote       = '"+POut.String(procedure.BillingNote)+"', "
				+"RepeatChargeNum   =  "+POut.Long  (procedure.RepeatChargeNum)+", "
				+"DiagnosticCode2   = '"+POut.String(procedure.DiagnosticCode2)+"', "
				+"DiagnosticCode3   = '"+POut.String(procedure.DiagnosticCode3)+"', "
				+"DiagnosticCode4   = '"+POut.String(procedure.DiagnosticCode4)+"', "
				+"Discount          = '"+POut.Double(procedure.Discount)+"', "
				+"SnomedBodySite    = '"+POut.String(procedure.SnomedBodySite)+"', "
				+"ProvOrderOverride =  "+POut.Long  (procedure.ProvOrderOverride)+", "
				+"IsDateProsthEst   =  "+POut.Bool  (procedure.IsDateProsthEst)+", "
				+"IcdVersion        =  "+POut.Byte  (procedure.IcdVersion)+" "
				+"WHERE ProcNum = "+POut.Long(procedure.ProcNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Procedure in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Procedure procedure,Procedure oldProcedure){
			string command="";
			if(procedure.PatNum != oldProcedure.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(procedure.PatNum)+"";
			}
			if(procedure.AptNum != oldProcedure.AptNum) {
				if(command!=""){ command+=",";}
				command+="AptNum = "+POut.Long(procedure.AptNum)+"";
			}
			if(procedure.OldCode != oldProcedure.OldCode) {
				if(command!=""){ command+=",";}
				command+="OldCode = '"+POut.String(procedure.OldCode)+"'";
			}
			if(procedure.ProcDate != oldProcedure.ProcDate) {
				if(command!=""){ command+=",";}
				command+="ProcDate = "+POut.Date(procedure.ProcDate)+"";
			}
			if(procedure.ProcFee != oldProcedure.ProcFee) {
				if(command!=""){ command+=",";}
				command+="ProcFee = '"+POut.Double(procedure.ProcFee)+"'";
			}
			if(procedure.Surf != oldProcedure.Surf) {
				if(command!=""){ command+=",";}
				command+="Surf = '"+POut.String(procedure.Surf)+"'";
			}
			if(procedure.ToothNum != oldProcedure.ToothNum) {
				if(command!=""){ command+=",";}
				command+="ToothNum = '"+POut.String(procedure.ToothNum)+"'";
			}
			if(procedure.ToothRange != oldProcedure.ToothRange) {
				if(command!=""){ command+=",";}
				command+="ToothRange = '"+POut.String(procedure.ToothRange)+"'";
			}
			if(procedure.Priority != oldProcedure.Priority) {
				if(command!=""){ command+=",";}
				command+="Priority = "+POut.Long(procedure.Priority)+"";
			}
			if(procedure.ProcStatus != oldProcedure.ProcStatus) {
				if(command!=""){ command+=",";}
				command+="ProcStatus = "+POut.Int   ((int)procedure.ProcStatus)+"";
			}
			if(procedure.ProvNum != oldProcedure.ProvNum) {
				if(command!=""){ command+=",";}
				command+="ProvNum = "+POut.Long(procedure.ProvNum)+"";
			}
			if(procedure.Dx != oldProcedure.Dx) {
				if(command!=""){ command+=",";}
				command+="Dx = "+POut.Long(procedure.Dx)+"";
			}
			if(procedure.PlannedAptNum != oldProcedure.PlannedAptNum) {
				if(command!=""){ command+=",";}
				command+="PlannedAptNum = "+POut.Long(procedure.PlannedAptNum)+"";
			}
			if(procedure.PlaceService != oldProcedure.PlaceService) {
				if(command!=""){ command+=",";}
				command+="PlaceService = "+POut.Int   ((int)procedure.PlaceService)+"";
			}
			if(procedure.Prosthesis != oldProcedure.Prosthesis) {
				if(command!=""){ command+=",";}
				command+="Prosthesis = '"+POut.String(procedure.Prosthesis)+"'";
			}
			if(procedure.DateOriginalProsth != oldProcedure.DateOriginalProsth) {
				if(command!=""){ command+=",";}
				command+="DateOriginalProsth = "+POut.Date(procedure.DateOriginalProsth)+"";
			}
			if(procedure.ClaimNote != oldProcedure.ClaimNote) {
				if(command!=""){ command+=",";}
				command+="ClaimNote = '"+POut.String(procedure.ClaimNote)+"'";
			}
			if(procedure.DateEntryC != oldProcedure.DateEntryC) {
				if(command!=""){ command+=",";}
				command+="DateEntryC = "+POut.Date(procedure.DateEntryC)+"";
			}
			if(procedure.ClinicNum != oldProcedure.ClinicNum) {
				if(command!=""){ command+=",";}
				command+="ClinicNum = "+POut.Long(procedure.ClinicNum)+"";
			}
			if(procedure.MedicalCode != oldProcedure.MedicalCode) {
				if(command!=""){ command+=",";}
				command+="MedicalCode = '"+POut.String(procedure.MedicalCode)+"'";
			}
			if(procedure.DiagnosticCode != oldProcedure.DiagnosticCode) {
				if(command!=""){ command+=",";}
				command+="DiagnosticCode = '"+POut.String(procedure.DiagnosticCode)+"'";
			}
			if(procedure.IsPrincDiag != oldProcedure.IsPrincDiag) {
				if(command!=""){ command+=",";}
				command+="IsPrincDiag = "+POut.Bool(procedure.IsPrincDiag)+"";
			}
			if(procedure.ProcNumLab != oldProcedure.ProcNumLab) {
				if(command!=""){ command+=",";}
				command+="ProcNumLab = "+POut.Long(procedure.ProcNumLab)+"";
			}
			if(procedure.BillingTypeOne != oldProcedure.BillingTypeOne) {
				if(command!=""){ command+=",";}
				command+="BillingTypeOne = "+POut.Long(procedure.BillingTypeOne)+"";
			}
			if(procedure.BillingTypeTwo != oldProcedure.BillingTypeTwo) {
				if(command!=""){ command+=",";}
				command+="BillingTypeTwo = "+POut.Long(procedure.BillingTypeTwo)+"";
			}
			if(procedure.CodeNum != oldProcedure.CodeNum) {
				if(command!=""){ command+=",";}
				command+="CodeNum = "+POut.Long(procedure.CodeNum)+"";
			}
			if(procedure.CodeMod1 != oldProcedure.CodeMod1) {
				if(command!=""){ command+=",";}
				command+="CodeMod1 = '"+POut.String(procedure.CodeMod1)+"'";
			}
			if(procedure.CodeMod2 != oldProcedure.CodeMod2) {
				if(command!=""){ command+=",";}
				command+="CodeMod2 = '"+POut.String(procedure.CodeMod2)+"'";
			}
			if(procedure.CodeMod3 != oldProcedure.CodeMod3) {
				if(command!=""){ command+=",";}
				command+="CodeMod3 = '"+POut.String(procedure.CodeMod3)+"'";
			}
			if(procedure.CodeMod4 != oldProcedure.CodeMod4) {
				if(command!=""){ command+=",";}
				command+="CodeMod4 = '"+POut.String(procedure.CodeMod4)+"'";
			}
			if(procedure.RevCode != oldProcedure.RevCode) {
				if(command!=""){ command+=",";}
				command+="RevCode = '"+POut.String(procedure.RevCode)+"'";
			}
			if(procedure.UnitQty != oldProcedure.UnitQty) {
				if(command!=""){ command+=",";}
				command+="UnitQty = "+POut.Int(procedure.UnitQty)+"";
			}
			if(procedure.BaseUnits != oldProcedure.BaseUnits) {
				if(command!=""){ command+=",";}
				command+="BaseUnits = "+POut.Int(procedure.BaseUnits)+"";
			}
			if(procedure.StartTime != oldProcedure.StartTime) {
				if(command!=""){ command+=",";}
				command+="StartTime = "+POut.Int(procedure.StartTime)+"";
			}
			if(procedure.StopTime != oldProcedure.StopTime) {
				if(command!=""){ command+=",";}
				command+="StopTime = "+POut.Int(procedure.StopTime)+"";
			}
			if(procedure.DateTP != oldProcedure.DateTP) {
				if(command!=""){ command+=",";}
				command+="DateTP = "+POut.Date(procedure.DateTP)+"";
			}
			if(procedure.SiteNum != oldProcedure.SiteNum) {
				if(command!=""){ command+=",";}
				command+="SiteNum = "+POut.Long(procedure.SiteNum)+"";
			}
			if(procedure.HideGraphics != oldProcedure.HideGraphics) {
				if(command!=""){ command+=",";}
				command+="HideGraphics = "+POut.Bool(procedure.HideGraphics)+"";
			}
			if(procedure.CanadianTypeCodes != oldProcedure.CanadianTypeCodes) {
				if(command!=""){ command+=",";}
				command+="CanadianTypeCodes = '"+POut.String(procedure.CanadianTypeCodes)+"'";
			}
			if(procedure.ProcTime != oldProcedure.ProcTime) {
				if(command!=""){ command+=",";}
				command+="ProcTime = "+POut.Time  (procedure.ProcTime)+"";
			}
			if(procedure.ProcTimeEnd != oldProcedure.ProcTimeEnd) {
				if(command!=""){ command+=",";}
				command+="ProcTimeEnd = "+POut.Time  (procedure.ProcTimeEnd)+"";
			}
			//DateTStamp can only be set by MySQL
			if(procedure.Prognosis != oldProcedure.Prognosis) {
				if(command!=""){ command+=",";}
				command+="Prognosis = "+POut.Long(procedure.Prognosis)+"";
			}
			if(procedure.DrugUnit != oldProcedure.DrugUnit) {
				if(command!=""){ command+=",";}
				command+="DrugUnit = "+POut.Int   ((int)procedure.DrugUnit)+"";
			}
			if(procedure.DrugQty != oldProcedure.DrugQty) {
				if(command!=""){ command+=",";}
				command+="DrugQty = "+POut.Float(procedure.DrugQty)+"";
			}
			if(procedure.UnitQtyType != oldProcedure.UnitQtyType) {
				if(command!=""){ command+=",";}
				command+="UnitQtyType = "+POut.Int   ((int)procedure.UnitQtyType)+"";
			}
			if(procedure.StatementNum != oldProcedure.StatementNum) {
				if(command!=""){ command+=",";}
				command+="StatementNum = "+POut.Long(procedure.StatementNum)+"";
			}
			if(procedure.IsLocked != oldProcedure.IsLocked) {
				if(command!=""){ command+=",";}
				command+="IsLocked = "+POut.Bool(procedure.IsLocked)+"";
			}
			if(procedure.BillingNote != oldProcedure.BillingNote) {
				if(command!=""){ command+=",";}
				command+="BillingNote = '"+POut.String(procedure.BillingNote)+"'";
			}
			if(procedure.RepeatChargeNum != oldProcedure.RepeatChargeNum) {
				if(command!=""){ command+=",";}
				command+="RepeatChargeNum = "+POut.Long(procedure.RepeatChargeNum)+"";
			}
			if(procedure.DiagnosticCode2 != oldProcedure.DiagnosticCode2) {
				if(command!=""){ command+=",";}
				command+="DiagnosticCode2 = '"+POut.String(procedure.DiagnosticCode2)+"'";
			}
			if(procedure.DiagnosticCode3 != oldProcedure.DiagnosticCode3) {
				if(command!=""){ command+=",";}
				command+="DiagnosticCode3 = '"+POut.String(procedure.DiagnosticCode3)+"'";
			}
			if(procedure.DiagnosticCode4 != oldProcedure.DiagnosticCode4) {
				if(command!=""){ command+=",";}
				command+="DiagnosticCode4 = '"+POut.String(procedure.DiagnosticCode4)+"'";
			}
			if(procedure.Discount != oldProcedure.Discount) {
				if(command!=""){ command+=",";}
				command+="Discount = '"+POut.Double(procedure.Discount)+"'";
			}
			if(procedure.SnomedBodySite != oldProcedure.SnomedBodySite) {
				if(command!=""){ command+=",";}
				command+="SnomedBodySite = '"+POut.String(procedure.SnomedBodySite)+"'";
			}
			if(procedure.ProvOrderOverride != oldProcedure.ProvOrderOverride) {
				if(command!=""){ command+=",";}
				command+="ProvOrderOverride = "+POut.Long(procedure.ProvOrderOverride)+"";
			}
			if(procedure.IsDateProsthEst != oldProcedure.IsDateProsthEst) {
				if(command!=""){ command+=",";}
				command+="IsDateProsthEst = "+POut.Bool(procedure.IsDateProsthEst)+"";
			}
			if(procedure.IcdVersion != oldProcedure.IcdVersion) {
				if(command!=""){ command+=",";}
				command+="IcdVersion = "+POut.Byte(procedure.IcdVersion)+"";
			}
			if(command==""){
				return false;
			}
			command="UPDATE procedurelog SET "+command
				+" WHERE ProcNum = "+POut.Long(procedure.ProcNum);
			Db.NonQ(command);
			return true;
		}

		//Delete not allowed for this table
		//public static void Delete(long procNum){
		//
		//}

	}
}