using System;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness{
		///<summary>(User OD since "user" is a reserved word) Users are a completely separate entity from Providers and Employees even though they can be linked.  A usernumber can never be changed, ensuring a permanent way to record database entries and leave an audit trail.  A user can be a provider, employee, or neither.</summary>
	public class Userod{
		///<summary>Primary key.</summary>
		public long UserNum;
		///<summary>.</summary>
		public string UserName;
		///<summary>The password hash, not the actual password.  If no password has been entered, then this will be blank.</summary>
		public string Password;
		///<summary>FK to usergroup.UserGroupNum.  Every user belongs to exactly one user group.  The usergroup determines the permissions.</summary>
		public long UserGroupNum;
		///<summary>FK to employee.EmployeeNum. Cannot be used if provnum is used. Used for timecards to block access by other users.</summary>
		public long EmployeeNum;
		///<summary>FK to clinic.ClinicNum.  If 0, then user has access to all clinics.</summary>
		public long ClinicNum;
		///<summary>FK to provider.ProvNum.  Cannot be used if EmployeeNum is used.</summary>
		public long ProvNum;
		///<summary>Set true to hide user from login list.</summary>
		public bool IsHidden;
		///<summary>FK to tasklist.TaskListNum.  0 if no inbox setup yet.  It is assumed that the TaskList is in the main trunk, but this is not strictly enforced.  User can't delete an attached TaskList, but they could move it.</summary>
		public long TaskListInBox;
		/// <summary> Defaults to 3 (regular user) unless specified. Helps populates the Anesthetist, Surgeon, Assistant and Circulator dropdowns properly on FormAnestheticRecord/// </summary>
		public int AnesthProvType;


		public Userod(){

		}

		public Userod(long userNum, string userName, string password, long userGroupNum, long employeeNum, long clinicNum, long provNum, bool isHidden, int anesthProvType)
		{ 
			UserNum=userNum;
			UserName=userName;
			Password=password;
			UserGroupNum=userGroupNum;
			EmployeeNum=employeeNum;
			ClinicNum=clinicNum;
			ProvNum=provNum;
			IsHidden=isHidden;
			AnesthProvType=anesthProvType;
		}
		
		///<summary></summary>
		public Userod Copy(){
			return (Userod)this.MemberwiseClone();
		}

		public override string ToString(){
			return UserName;
		}
	}

	//public class DtoUserodRefresh:DtoQueryBase {
	//}

}
