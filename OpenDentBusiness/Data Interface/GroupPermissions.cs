using System;
using System.Collections;
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
			return Crud.GroupPermissionCrud.Insert(gp);
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
				command="SELECT COUNT(*) FROM grouppermission WHERE PermType='"+POut.Long((int)permType)+"'";
				DataTable table=Db.GetTable(command);
				if(table.Rows[0][0].ToString()=="1"){//only one, so this would delete the last one.
					throw new Exception(Lans.g("FormSecurity","At least one group must have Security Admin permission."));
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
				case Permissions.Backup:
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
				return false;//Does not have audit Trail.
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
				case Permissions.ClaimSentEdit:
					return Lans.g("enumPermissions","Claim Sent Edit");
				case Permissions.CommlogEdit:
					return Lans.g("enumPermissions","Commlog Edit");
				case Permissions.DepositSlips:
					return Lans.g("enumPermissions","Deposit Slips");
				case Permissions.EhrEmergencyAccess:
					return Lans.g("enumPermissions","EHR Emergency Access");
				//case Permissions.EhrInfoButton:
				//	return Lans.g("enumPermissions","EHR Access Info Button");
				//case Permissions.EhrShowCDS:
				//	return Lans.g("enumPermissions","EHR Show Clinical Decision Support");
				case Permissions.EquipmentDelete:
					return Lans.g("enumPermissions","Equipment Delete");
				case Permissions.EquipmentSetup:
					return Lans.g("enumPermissions","Equipment Setup");
				case Permissions.FamilyModule:
					return Lans.g("enumPermissions","Family Module");
				case Permissions.ImageDelete:
					return Lans.g("enumPermissions","Image Delete");
				case Permissions.ImagesModule:
					return Lans.g("enumPermissions","Images Module");
				case Permissions.InsPayCreate:
					return Lans.g("enumPermissions","Insurance Payment Create");
				case Permissions.InsPayEdit:
					return Lans.g("enumPermissions","Insurance Payment Edit");
				case Permissions.InsPlanChangeSubsc:
					return Lans.g("enumPermissions","Insurance Plan Change Subscriber");
				case Permissions.ManageModule:
					return Lans.g("enumPermissions","Manage Module");
				case Permissions.None:
					return "";
				case Permissions.OrthoChartEdit:
					return Lans.g("enumPermissions","Ortho Chart Edit");
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
				case Permissions.Reports:
					return Lans.g("enumPermissions","Reports");
				case Permissions.RefAttachAdd:
					return Lans.g("enumPermissions","Referral, Add to Patient");
				case Permissions.RefAttachDelete:
					return Lans.g("enumPermissions","Referral, Delete from Patient");
				case Permissions.ReferralAdd:
					return Lans.g("enumPermissions","Referral Add");
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
				case Permissions.ReportProdInc:
					return Lans.g("enumPermissions","Reports - Production and Income, Aging");
				case Permissions.ReportDashboard:
					return Lans.g("enumPermissions","Reports - Dashboard");
				case Permissions.TaskNoteEdit:
					return Lans.g("enumPermissions","Task Note Edit");
				case Permissions.WikiListSetup:
					return Lans.g("enumPermissions","Wiki List Setup");
				case Permissions.TaskEdit:
					return Lans.g("enumPermissions","Task Edit");
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
		

	}
 
	

	
}













