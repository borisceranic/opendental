using System;
using System.Collections;

namespace OpenDentBusiness{

	///<summary>Stores an ongoing record of database activity for security purposes.  User not allowed to edit.</summary>
	[Serializable]
	public class SecurityLog:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long SecurityLogNum;
		///<summary>Enum:Permissions</summary>
		public Permissions PermType;
		///<summary>FK to userod.UserNum</summary>
		public long UserNum;
		///<summary>The date and time of the entry.  It's value is set when inserting and can never change.  Even if a user changes the date on their ocmputer, this remains accurate because it uses server time.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateTEntry)]
		public DateTime LogDateTime;
		///<summary>The description of exactly what was done. Varies by permission type.</summary>
		public string LogText;
		///<summary>FK to patient.PatNum.  Can be 0 if not applicable.</summary>
		public long PatNum;
		///<summary>.</summary>
		public string CompName;
		///<summary>A foreign key to a table associated with the PermType.  0 indicates not in use.  
		///This is typically used for objects that have specific audit trails so that users can see all audit entries related to a particular object.  
		///Every permission using FKey should be included and implmented in the CrudAuditPerms enum so that securitylog FKeys are note orphaned.
		///Additonaly, the tabletype will to have the [CrudTable(CrudAuditPerms=CrudAuditPerm._____] added with the new CrudAuditPerm you created.
		///For the patient portal, it is used to indicate logs created on behalf of other patients.  
		///It's uses include:  AptNum with PermType AppointmentCreate, AppointmentEdit, or AppointmentMove tracks all appointment logs for a particular 
		///appointment.
		///CodeNum with PermType ProcFeeEdit currently only tracks fee changes.  
		///PatNum with PermType PatientPortal represents an entry that a patient made on behalf of another patient.
		///	The PatNum column will represent the patient who is taking the action.  
		///PlanNum with PermType InsPlanChangeCarrierName tracks carrier name changes.</summary>
		public long FKey;
		///<summary>Enum:LogSources None, WebSched, InsPlanImport834, FHIR.</summary>
		public LogSources LogSource;

		///<summary>PatNum-NameLF</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public string PatientName;
		///<summary>Existing LogHash from SecurityLogHash table</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public string LogHash;
		///<summary>Used in Claims for custom history edits.</summary>
		public long DefNum;

	}

	///<summary>Known entities that create security logs.</summary>
	public enum LogSources {
		///<summary>0 - Open Dental and unknown entities.</summary>
		None,
		///<summary>1 - GWT Web Sched application.</summary>
		WebSched,
		///<summary>2 - X12 834 Insurance Plan Import from the Manage Module.</summary>
		InsPlanImport834,
		///<summary>3 - HL7 is an automated process which the user may not be aware of.</summary>
		HL7,
		///<summary>4 - Database maintenance.  This process creates patients which are known to be missing,
		///but the user may not be aware that the fix involves patient recreation.</summary>
		DBM,
		///<summary>5 - FHIR is an automated process which the user may not be aware of.</summary>
		FHIR
	}

	


}













