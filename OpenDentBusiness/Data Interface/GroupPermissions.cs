using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

namespace OpenDentBusiness{
	///<summary></summary>
	public class GroupPermissions {
		///<summary></summary>
		public static DataTable RefreshCache() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM grouppermission";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="GroupPermission";
			FillCache(table);
			return table;
		}

		private static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			GroupPermissionC.List=Crud.GroupPermissionCrud.TableToList(table).ToArray();
		}

		///<summary></summary>
		public static void Update(GroupPermission gp){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),gp);
				return;
			}
			if(gp.NewerDate.Year>1880 && gp.NewerDays>0) {
				throw new Exception(Lans.g("GroupPermissions","Date or days can be set, but not both."));
			}
			if(!GroupPermissions.PermTakesDates(gp.PermType)) {
				if(gp.NewerDate.Year>1880 || gp.NewerDays>0) {
					throw new Exception(Lans.g("GroupPermissions","This type of permission may not have a date or days set."));
				}
			}
			Crud.GroupPermissionCrud.Update(gp);
		}

		///<summary>Update that doesnt use the local cache.  Useful for multithreaded connections.</summary>
		public static void UpdateNoCache(GroupPermission gp) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),gp);
				return;
			}
			string command="UPDATE grouppermission SET "
				+"NewerDate   =  "+POut.Date  (gp.NewerDate)+", "
				+"NewerDays   =  "+POut.Int   (gp.NewerDays)+", "
				+"UserGroupNum=  "+POut.Long  (gp.UserGroupNum)+", "
				+"PermType    =  "+POut.Int   ((int)gp.PermType)+" "
				+"WHERE GroupPermNum = "+POut.Long(gp.GroupPermNum);
			Db.NonQ(command);
		}

		///<summary>Deletes GroupPermissions based on primary key.  Do not call this method unless you have checked specific dependencies first.  E.g. after deleting this permission, there will still be a security admin user.  This method is only called from the CEMT sync.  RemovePermission should probably be used instead.</summary>
		public static void Delete(GroupPermission gp) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),gp);
				return;
			}
			string command="DELETE FROM grouppermission WHERE GroupPermNum = "+POut.Long(gp.GroupPermNum);
			Db.NonQ(command);
		}

		///<summary>Deletes without using the cache.  Useful for multithreaded connections.</summary>
		public static void DeleteNoCache(GroupPermission gp) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),gp);
				return;
			}
			string command="DELETE FROM grouppermission WHERE GroupPermNum="+POut.Long(gp.GroupPermNum);
			Db.NonQ(command);
		}

		///<summary></summary>
		public static long Insert(GroupPermission gp){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				gp.GroupPermNum=Meth.GetLong(MethodBase.GetCurrentMethod(),gp);
				return gp.GroupPermNum;
			}
			if(gp.NewerDate.Year>1880 && gp.NewerDays>0) {
				throw new Exception(Lans.g("GroupPermissions","Date or days can be set, but not both."));
			}
			if(!GroupPermissions.PermTakesDates(gp.PermType)) {
				if(gp.NewerDate.Year>1880 || gp.NewerDays>0) {
					throw new Exception(Lans.g("GroupPermissions","This type of permission may not have a date or days set."));
				}
			}
			if(gp.PermType==Permissions.SecurityAdmin) {
				//Make sure there are no hidden users in the group that is about to get the Security Admin permission.
				string command="SELECT COUNT(*) FROM userod WHERE IsHidden=1"
				+" AND UserGroupNum="+gp.UserGroupNum;
				int count=PIn.Int(Db.GetCount(command));
				if(count!=0) {//there are hidden users in this group
					throw new Exception(Lans.g("FormSecurity","Hidden users cannot have the Security Admin permission."));
				}
			}
			return Crud.GroupPermissionCrud.Insert(gp);
		}

		///<summary>Insertion logic that doesn't use the cache. Has special cases for generating random PK's and handling Oracle insertions.</summary>
		public static long InsertNoCache(GroupPermission gp) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),gp);
			}
			return Crud.GroupPermissionCrud.InsertNoCache(gp);
		}

		///<summary></summary>
		public static void RemovePermission(long groupNum,Permissions permType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),groupNum,permType);
				return;
			}
			string command;
			if(permType==Permissions.SecurityAdmin){
				//need to make sure that at least one other user has this permission
				command="SELECT COUNT(*) FROM (SELECT DISTINCT grouppermission.UserGroupNum "
					+"FROM grouppermission "
					+"INNER JOIN userod ON userod.UserGroupNum=grouppermission.UserGroupNum AND userod.IsHidden=0 "
					+"WHERE PermType='"+POut.Long((int)permType)+"') t";//This query is Oracle compatable
				if(Db.GetScalar(command)=="1") {//only one, so this would delete the last one.
					throw new Exception(Lans.g("FormSecurity","There must always be at least one user in a user group that has the Security Admin permission."));
				}
			}
			command="DELETE from grouppermission WHERE UserGroupNum='"+POut.Long(groupNum)+"' "
				+"AND PermType='"+POut.Long((int)permType)+"'";
 			Db.NonQ(command);
		}

		///<summary>Gets a GroupPermission based on the supplied userGroupNum and permType.  If not found, then it returns null.  Used in FormSecurity when double clicking on a dated permission or when clicking the all button.</summary>
		public static GroupPermission GetPerm(long userGroupNum,Permissions permType) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<GroupPermissionC.List.Length;i++){
				if(GroupPermissionC.List[i].UserGroupNum==userGroupNum && GroupPermissionC.List[i].PermType==permType){
					return GroupPermissionC.List[i].Copy();
				}
			}
			return null;
		}

		///<summary>Gets a list of GroupPermissions for the supplied UserGroupNum.</summary>
		public static List<GroupPermission> GetPerms(long userGroupNum) {
			//No need to check RemotingRole; no call to db.
			List<GroupPermission> listGroupPerms=new List<GroupPermission>();
			for(int i=0;i<GroupPermissionC.List.Length;i++) {
				if(GroupPermissionC.List[i].UserGroupNum==userGroupNum) {
					listGroupPerms.Add(GroupPermissionC.List[i].Copy());
				}
			}
			return listGroupPerms;
		}

		///<summary>Gets a list of GroupPermissions for the supplied UserGroupNum without using the local cache.  Useful for multithreaded connections.</summary>
		public static List<GroupPermission> GetPermsNoCache(long userGroupNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<GroupPermission>>(MethodBase.GetCurrentMethod(),userGroupNum);
			}
			List<GroupPermission> retVal=new List<GroupPermission>();
			string command="SELECT * FROM grouppermission WHERE UserGroupNum="+POut.Long(userGroupNum);
			DataTable tableGroupPerms=Db.GetTable(command);
			retVal=Crud.GroupPermissionCrud.TableToList(tableGroupPerms);
			return retVal;
		}

		///<summary>Used in Security.IsAuthorized</summary>
		public static bool HasPermission(long userGroupNum,Permissions permType){
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<GroupPermissionC.List.Length;i++){
				if(GroupPermissionC.List[i].UserGroupNum!=userGroupNum || GroupPermissionC.List[i].PermType!=permType){
					continue;
				}
				return true;
			}
			return false;
		}

		///<summary>Gets permissions that actually generate audit trail entries.</summary>
		public static bool HasAuditTrail(Permissions permType) {
			//No need to check RemotingRole; no call to db.
			switch(permType) {//If commented, has an audit trail. In the order they appear in Permissions enumeration
				//Normal pattern is to comment out the FALSE cases. 
				//This is the opposite so that the default behavior for new security permissions to be to show in the audit trail. In case it wasn't added to this function.
				case Permissions.None:
				//case Permissions.AppointmentsModule:
				//case Permissions.FamilyModule:
				//case Permissions.AccountModule:
				//case Permissions.TPModule:
				//case Permissions.ChartModule:
				//case Permissions.ImagesModule:
				case Permissions.ManageModule:
				//case Permissions.Setup:
				//case Permissions.RxCreate:
				//case Permissions.ProcComplEdit:
				//case Permissions.ChooseDatabase:
				//case Permissions.Schedules:
				//case Permissions.Blockouts:
				//case Permissions.ClaimSentEdit:
				//case Permissions.PaymentCreate:
				//case Permissions.PaymentEdit:
				//case Permissions.AdjustmentCreate:
				//case Permissions.AdjustmentEdit:
				//case Permissions.UserQuery:
				case Permissions.StartupSingleUserOld:
				case Permissions.StartupMultiUserOld:
				//case Permissions.Reports:
				//case Permissions.ProcComplCreate:
				//case Permissions.SecurityAdmin:
				//case Permissions.AppointmentCreate:
				//case Permissions.AppointmentMove:
				//case Permissions.AppointmentEdit:
				//case Permissions.AppointmentCompleteEdit:
				//case Permissions.Backup:
				case Permissions.TimecardsEditAll:
				//case Permissions.DepositSlips:
				//case Permissions.AccountingEdit:
				//case Permissions.AccountingCreate:
				case Permissions.Accounting:
				case Permissions.AnesthesiaIntakeMeds:
				case Permissions.AnesthesiaControlMeds:
				case Permissions.InsPayCreate:
				//case Permissions.InsPayEdit:
				//case Permissions.TreatPlanEdit:
				//case Permissions.ReportProdInc:
				//case Permissions.TimecardDeleteEntry:
				case Permissions.EquipmentDelete:
				//case Permissions.SheetEdit:
				//case Permissions.CommlogEdit:
				case Permissions.ImageDelete:
				case Permissions.PerioEdit:
				case Permissions.ProcEditShowFee:
				case Permissions.AdjustmentEditZero:
				case Permissions.EhrEmergencyAccess:
				//case Permissions.ProcDelete:
				case Permissions.EhrKeyAdd:
				case Permissions.Providers:
				case Permissions.EcwAppointmentRevise:
				case Permissions.ProcedureNote:
				case Permissions.ReferralAdd:
				case Permissions.InsPlanChangeSubsc:
				//case Permissions.RefAttachAdd:
				//case Permissions.RefAttachDelete:
				case Permissions.CarrierCreate:
				case Permissions.ReportDashboard:
				case Permissions.AutoNoteQuickNoteEdit:
				case Permissions.EquipmentSetup:
				//case Permissions.Billing:
				//case Permissions.ProblemEdit:
				//case Permissions.ProcFeeEdit:
				//case Permissions.InsPlanChangeCarrierName:
				case Permissions.TaskNoteEdit:
				case Permissions.WikiListSetup:
				case Permissions.Copy:
				case Permissions.Printing:
				//case Permissions.MedicalInfoViewed:
				//case Permissions.PatProblemListEdit:
				//case Permissions.PatMedicationListEdit:
				//case Permissions.PatAllergyListEdit:
				case Permissions.PatFamilyHealthEdit:
				case Permissions.PatientPortal:
				//case Permissions.RxEdit:
				case Permissions.AdminDentalStudents:
				case Permissions.AdminDentalInstructors:
				//case Permissions.OrthoChartEdit:
				//case Permissions.PatientFieldEdit:
				case Permissions.AdminDentalEvaluations:
				case Permissions.TreatPlanDiscountEdit:
				//case Permissions.UserLogOnOff:
				//case Permissions.TaskEdit:
				//case Permissions.EmailSend:
				//case Permissions.WebmailSend:
				//case Permissions.UserQueryAdmin:
				//case Permissions.InsPlanChangeAssign:
				//case Permissions.ImageEdit:
				//case Permissions.EhrMeasureEventEdit:
				//case Permissions.EServicesSetup:
				//case Permissions.FeeSchedEdit:
				case Permissions.ProviderFeeEdit:
				case Permissions.ClaimHistoryEdit:
				//case Permissions.FeatureRequestEdit:
				//case Permissions.QueryRequestEdit:
				//case Permissions.JobApproval:
				//case Permissions.JobDocumentation:
				//case Permissions.JobEdit:
				//case Permissions.JobManager:
				//case Permissions.JobReview:
				//case Permissions.WebmailDelete:
				//case Permissions.MissingRequiredField:
				//case Permissions.ReferralMerge:
				//case Permissions.ProcEdit:
				//case Permissions.ProviderMerge:
				//case Permissions.MedicationMerge:
				//case Permissions.AccountQuickCharge:
				//case Permissions.ClaimSend:
				return false;//Does not have audit Trail if uncommented.
			}
			return true;
		}

		///<summary>Gets the description for the specified permisssion.  Already translated.</summary>
		public static string GetDesc(Permissions perm){
			//No need to check RemotingRole; no call to db.
			switch(perm){
				case Permissions.Accounting:
					return Lans.g("enumPermissions","Accounting");
				case Permissions.AccountingCreate:
					return Lans.g("enumPermissions","Accounting Create Entry");
				case Permissions.AccountingEdit:
					return Lans.g("enumPermissions","Accounting Edit Entry");
				case Permissions.AccountModule:
					return Lans.g("enumPermissions","Account Module");
				case Permissions.AccountProcsQuickAdd:
					return Lans.g("enumPermissions","Account Procs Quick Add");
				case Permissions.AdjustmentCreate:
					return Lans.g("enumPermissions","Adjustment Create");
				case Permissions.AdjustmentEdit:
					return Lans.g("enumPermissions","Adjustment Edit");
				case Permissions.AdjustmentEditZero:
					return Lans.g("enumPermissions","Adjustment Edit Zero Amount");
				case Permissions.AdminDentalEvaluations:
					return Lans.g("enumPermissions","Admin Evaluation Edit");
				case Permissions.AdminDentalInstructors:
					return Lans.g("enumPermissions","Instructor Edit");
				case Permissions.AdminDentalStudents:
					return Lans.g("enumPermissions","Student Edit");
				case Permissions.AnesthesiaIntakeMeds:
					return Lans.g("enumPermissions","Intake Anesthetic Medications into Inventory");
				case Permissions.AnesthesiaControlMeds:
					return Lans.g("enumPermissions","Edit Anesthetic Records; Edit/Adjust Inventory Counts");
				case Permissions.AppointmentCreate:
					return Lans.g("enumPermissions","Appointment Create");
				case Permissions.AppointmentEdit:
					return Lans.g("enumPermissions","Appointment Edit");
				case Permissions.AppointmentMove:
					return Lans.g("enumPermissions","Appointment Move");
				case Permissions.AppointmentsModule:
					return Lans.g("enumPermissions","Appointments Module");
				case Permissions.AutoNoteQuickNoteEdit:
					return Lans.g("enumPermissions","Auto/Quick Note Edit");
				case Permissions.EcwAppointmentRevise:
					return Lans.g("enumPermissions","eCW Appointment Revise");
				case Permissions.AppointmentCompleteEdit:
					return Lans.g("enumPermissions","Completed Appointment Edit");
				case Permissions.Backup:
					return Lans.g("enumPermissions","Backup");
				case Permissions.Billing:
					return Lans.g("enumPermissions","Billing");
				case Permissions.Blockouts:
					return Lans.g("enumPermissions","Blockouts");
				case Permissions.ChartModule:
					return Lans.g("enumPermissions","Chart Module");
				case Permissions.CarrierCreate:
					return Lans.g("enumPermissions","Carrier Create");
				case Permissions.ChooseDatabase:
					return Lans.g("enumPermissions","Choose Database");
				case Permissions.ClaimHistoryEdit:
					return Lans.g("enumPermissions","Claim History Edit");
				case Permissions.ClaimSend:
					return Lans.g("enumPermissions","Claim Send");
				case Permissions.ClaimSentEdit:
					return Lans.g("enumPermissions","Claim Sent Edit");
				case Permissions.CommlogEdit:
					return Lans.g("enumPermissions","Commlog Edit");
				case Permissions.DepositSlips:
					return Lans.g("enumPermissions","Deposit Slips");
				case Permissions.EhrEmergencyAccess:
					return Lans.g("enumPermissions","EHR Emergency Access");
				case Permissions.EhrMeasureEventEdit:
					return Lans.g("enumPermissions","EHR Measure Event Edit");
				//case Permissions.EhrInfoButton:
				//	return Lans.g("enumPermissions","EHR Access Info Button");
				//case Permissions.EhrShowCDS:
				//	return Lans.g("enumPermissions","EHR Show Clinical Decision Support");
				case Permissions.EmailSend:
					return Lans.g("enumPermissions","Email Send");
				case Permissions.EquipmentDelete:
					return Lans.g("enumPermissions","Equipment Delete");
				case Permissions.EquipmentSetup:
					return Lans.g("enumPermissions","Equipment Setup");
				case Permissions.EServicesSetup:
					return Lans.g("enumPermissions","EServices Setup");
				case Permissions.FamilyModule:
					return Lans.g("enumPermissions","Family Module");
				case Permissions.ImageDelete:
					return Lans.g("enumPermissions","Image Delete");
				case Permissions.ImageEdit:
					return Lans.g("enumPermissions","Image Edit");
				case Permissions.ImagesModule:
					return Lans.g("enumPermissions","Images Module");
				case Permissions.InsPayCreate:
					return Lans.g("enumPermissions","Insurance Payment Create");
				case Permissions.InsPayEdit:
					return Lans.g("enumPermissions","Insurance Payment Edit");
				case Permissions.InsPlanChangeAssign:
					return Lans.g("enumPermissions","Insurance Plan Change Assignment of Benefits");
				case Permissions.InsPlanChangeSubsc:
					return Lans.g("enumPermissions","Insurance Plan Change Subscriber");
				case Permissions.ManageModule:
					return Lans.g("enumPermissions","Manage Module");
				case Permissions.MedicationMerge:
					return Lans.g("enumPermissions","Medication Merge");
				case Permissions.None:
					return "";
				case Permissions.OrthoChartEdit:
					return Lans.g("enumPermissions","Ortho Chart Edit");
				case Permissions.PatientMerge:
					return Lans.g("enumPermissions","Patient Merge");
				case Permissions.PaymentCreate:
					return Lans.g("enumPermissions","Payment Create");
				case Permissions.PaymentEdit:
					return Lans.g("enumPermissions","Payment Edit");
				case Permissions.PerioEdit:
					return Lans.g("enumPermissions","Perio Chart Edit");
				case Permissions.ProblemEdit:
					return Lans.g("enumPermissions","Problem Edit");
				case Permissions.ProcComplCreate:
					return Lans.g("enumPermissions","Create Completed Procedure (or set complete)");
				case Permissions.ProcedureNote:
					return Lans.g("enumPermissions","Procedure Note");
				case Permissions.ProcDelete:
					return Lans.g("enumPermissions","Delete Procedure");
				case Permissions.ProcComplEdit:
					return Lans.g("enumPermissions","Edit Completed Procedure");
				case Permissions.ProcEditShowFee:
					return Lans.g("enumPermissions","Show Procedure Fee");
				case Permissions.Providers:
					return Lans.g("enumPermissions","Providers");
				case Permissions.ProviderFeeEdit:
					return Lans.g("enumPermissions","Provider Fee Edit");
				case Permissions.ProviderMerge:
					return Lans.g("enumPermissions","Provider Merge");
				case Permissions.Reports:
					return Lans.g("enumPermissions","Reports");
				case Permissions.RefAttachAdd:
					return Lans.g("enumPermissions","Referral, Add to Patient");
				case Permissions.RefAttachDelete:
					return Lans.g("enumPermissions","Referral, Delete from Patient");
				case Permissions.ReferralAdd:
					return Lans.g("enumPermissions","Referral Add");
				case Permissions.ReferralMerge:
					return Lans.g("enumPermissions","Referral Merge");
				case Permissions.ReportDashboard:
					return Lans.g("enumPermissions","Reports - Dashboard");
				case Permissions.ReportProdInc:
					return Lans.g("enumPermissions","Reports - Production and Income, Aging");
				case Permissions.RequiredFields:
					return Lans.g("enumPermissions","Required Fields Missing");
				case Permissions.RxCreate:
					return Lans.g("enumPermissions","Rx Create");
				case Permissions.RxEdit:
					return Lans.g("enumPermissions","Rx Edit");
				case Permissions.Schedules:
					return Lans.g("enumPermissions","Schedules - Practice and Provider");
				case Permissions.SecurityAdmin:
					return Lans.g("enumPermissions","Security Admin");
				case Permissions.Setup:
					return Lans.g("enumPermissions","Setup - Covers a wide variety of setup functions");
				case Permissions.SheetEdit:
					return Lans.g("enumPermissions","Sheet Edit");
				case Permissions.TaskEdit:
					return Lans.g("enumPermissions","Task Edit");
				case Permissions.TaskNoteEdit:
					return Lans.g("enumPermissions","Task Note Edit");
				case Permissions.TimecardDeleteEntry:
					return Lans.g("enumPermissions","Timecard Delete Entry");
				case Permissions.TimecardsEditAll:
					return Lans.g("enumPermissions","Edit All Timecards");
				case Permissions.TPModule:
					return Lans.g("enumPermissions","TreatmentPlan Module");
				case Permissions.TreatPlanEdit:
					return Lans.g("enumPermissions","Edit Treatment Plan");
				case Permissions.UserQuery:
					return Lans.g("enumPermissions","User Query");
				case Permissions.UserQueryAdmin:
					return Lans.g("enumPermissions","Command Query");
				case Permissions.WebmailDelete:
					return Lans.g("enumPermissions","Webmail Delete");
				case Permissions.WebmailSend:
					return Lans.g("enumPermissions","Webmail Send");
				case Permissions.WikiListSetup:
					return Lans.g("enumPermissions","Wiki List Setup");
			}
			return "";//should never happen
		}

		///<summary></summary>
		public static bool PermTakesDates(Permissions permType){
			//No need to check RemotingRole; no call to db.
			if(permType==Permissions.AccountingCreate//prevents backdating
				|| permType==Permissions.AccountingEdit
				|| permType==Permissions.AdjustmentEdit
				|| permType==Permissions.ClaimSentEdit
				|| permType==Permissions.CommlogEdit
				|| permType==Permissions.DepositSlips//prevents backdating
				|| permType==Permissions.EquipmentDelete
				|| permType==Permissions.ImageDelete
				|| permType==Permissions.InsPayEdit
				|| permType==Permissions.OrthoChartEdit
				|| permType==Permissions.PaymentEdit
				|| permType==Permissions.PerioEdit
				|| permType==Permissions.ProcComplEdit
				|| permType==Permissions.ProcDelete
				|| permType==Permissions.SheetEdit
				|| permType==Permissions.TimecardDeleteEntry
				|| permType==Permissions.TreatPlanEdit
				)
			{
				return true;
			}
			return false;
		}

		///<summary>Returns a list of permissions that are included in the bitwise enum crudSLFKeyPerms passed in.
		///Used in DBM and the crud generator.  Needs to be updated every time a new CrudAuditPerm is added.</summary>
		public static List<Permissions> GetPermsFromCrudAuditPerm(CrudAuditPerm crudSLFKeyPerms) {
			List<Permissions> listPerms=new List<Permissions>();
			//No check for none.
			if(crudSLFKeyPerms.HasFlag(CrudAuditPerm.AppointmentCompleteEdit)) { //b01
				listPerms.Add(Permissions.AppointmentCompleteEdit);
			}
			if(crudSLFKeyPerms.HasFlag(CrudAuditPerm.AppointmentCreate)) { //b010
				listPerms.Add(Permissions.AppointmentCreate);
			}
			if(crudSLFKeyPerms.HasFlag(CrudAuditPerm.AppointmentEdit)) { //b0100
				listPerms.Add(Permissions.AppointmentEdit);
			}
			if(crudSLFKeyPerms.HasFlag(CrudAuditPerm.AppointmentMove)) { //b01000
				listPerms.Add(Permissions.AppointmentMove);
			}
			if(crudSLFKeyPerms.HasFlag(CrudAuditPerm.ClaimHistoryEdit)) { //b010000
				listPerms.Add(Permissions.ClaimHistoryEdit);
			}
			if(crudSLFKeyPerms.HasFlag(CrudAuditPerm.ImageDelete)) { //b0100000
				listPerms.Add(Permissions.ImageDelete);
			}
			if(crudSLFKeyPerms.HasFlag(CrudAuditPerm.ImageEdit)) { //b01000000
				listPerms.Add(Permissions.ImageEdit);
			}
			if(crudSLFKeyPerms.HasFlag(CrudAuditPerm.InsPlanChangeCarrierName)) { //b010000000
				listPerms.Add(Permissions.InsPlanChangeCarrierName);
			}
			if(crudSLFKeyPerms.HasFlag(CrudAuditPerm.RxCreate)) { //b0100000000
				listPerms.Add(Permissions.RxCreate);
			}
			if(crudSLFKeyPerms.HasFlag(CrudAuditPerm.RxEdit)) { //b01000000000
				listPerms.Add(Permissions.RxEdit);
			}
			if(crudSLFKeyPerms.HasFlag(CrudAuditPerm.TaskNoteEdit)) { //b010000000000
				listPerms.Add(Permissions.TaskNoteEdit);
			}
			if(crudSLFKeyPerms.HasFlag(CrudAuditPerm.PatientPortal)) { //b0100000000000
				listPerms.Add(Permissions.PatientPortal);
			}
			if(crudSLFKeyPerms.HasFlag(CrudAuditPerm.ProcFeeEdit)) { //b01000000000000
				listPerms.Add(Permissions.ProcFeeEdit);
			}
			return listPerms;
		}

	}
 
	

	
}













