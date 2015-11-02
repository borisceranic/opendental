using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary>There is no cache for clinics.  We assume they will almost never change.</summary>
	public class Clinics {
		///<summary>Clinics cannot be hidden or deleted, so there is only one list.</summary>
		private static Clinic[] _list;
		private static object _lockObj=new object();
		///<summary>Currently active clinic within OpenDental.  Reflects FormOpenDental.ClinicNum</summary>
		public static long ClinicNum=0;

		public static Clinic[] List{
			//No need to check RemotingRole; no call to db.
			get {
				return GetList();
			}
			set {
				lock(_lockObj) {
					_list=value;
				}
			}
		}

		public static Clinic[] GetList() {
			bool isListNull=false;
			lock(_lockObj) {
				if(_list==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				RefreshCache();
			}
			Clinic[] arrayClinics;
			lock(_lockObj) {
				arrayClinics=new Clinic[_list.Length];
				for(int i=0;i<_list.Length;i++) {
					arrayClinics[i]=_list[i].Copy();
				}
			}
			return arrayClinics;
		}

		///<summary>Refresh all clinics.  Not actually part of official cache.</summary>
		public static DataTable RefreshCache() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM clinic";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="clinic";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table) {
			//No need to check RemotingRole; no call to db.
			List=Crud.ClinicCrud.TableToList(table).ToArray();
		}

		///<summary></summary>
		public static long Insert(Clinic clinic){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				clinic.ClinicNum=Meth.GetLong(MethodBase.GetCurrentMethod(),clinic);
				return clinic.ClinicNum;
			}
			return Crud.ClinicCrud.Insert(clinic);
		}

		///<summary></summary>
		public static void Update(Clinic clinic){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),clinic);
				return;
			}
			Crud.ClinicCrud.Update(clinic);
		}

		///<summary>Checks dependencies first.  Throws exception if can't delete.</summary>
		public static void Delete(Clinic clinic) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),clinic);
				return;
			}
			//Check FK dependencies.
			#region Patients
			string command="SELECT LName,FName FROM patient WHERE ClinicNum ="
				+POut.Long(clinic.ClinicNum);
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count>0) {
				string pats="";
				for(int i=0;i<table.Rows.Count;i++) {
					pats+="\r";
					if(i==15) {
						pats+=Lans.g("Clinics","And")+" "+(table.Rows.Count-i)+" "+Lans.g("Clinics","others");
						break;
					}
					pats+=table.Rows[i]["LName"].ToString()+", "+table.Rows[i]["FName"].ToString();
				}
				throw new Exception(Lans.g("Clinics","Cannot delete clinic because it is in use by the following patients:")+pats);
			}
			#endregion
			#region Payments
			command="SELECT patient.LName,patient.FName FROM patient,payment "
				+"WHERE payment.ClinicNum ="+POut.Long(clinic.ClinicNum)
				+" AND patient.PatNum=payment.PatNum";
			table=Db.GetTable(command);
			if(table.Rows.Count>0) {
				string pats="";
				for(int i=0;i<table.Rows.Count;i++) {
					pats+="\r";
					if(i==15) {
						pats+=Lans.g("Clinics","And")+" "+(table.Rows.Count-i)+" "+Lans.g("Clinics","others");
						break;
					}
					pats+=table.Rows[i]["LName"].ToString()+", "+table.Rows[i]["FName"].ToString();
				}
				throw new Exception(Lans.g("Clinics","Cannot delete clinic because the following patients have payments using it:")+pats);
			}
			#endregion
			#region ClaimPayments
			command="SELECT patient.LName,patient.FName FROM patient,claimproc,claimpayment "
				+"WHERE claimpayment.ClinicNum ="+POut.Long(clinic.ClinicNum)
				+" AND patient.PatNum=claimproc.PatNum"
				+" AND claimproc.ClaimPaymentNum=claimpayment.ClaimPaymentNum "
				+"GROUP BY patient.LName,patient.FName,claimpayment.ClaimPaymentNum";
			table=Db.GetTable(command);
			if(table.Rows.Count>0) {
				string pats="";
				for(int i=0;i<table.Rows.Count;i++) {
					pats+="\r";
					if(i==15) {
						pats+=Lans.g("Clinics","And")+" "+(table.Rows.Count-i)+" "+Lans.g("Clinics","others");
						break;
					}
					pats+=table.Rows[i]["LName"].ToString()+", "+table.Rows[i]["FName"].ToString();
				}
				throw new Exception(Lans.g("Clinics","Cannot delete clinic because the following patients have claim payments using it:")+pats);
			}
			#endregion
			#region Appointments
			command="SELECT patient.LName,patient.FName FROM patient,appointment "
				+"WHERE appointment.ClinicNum ="+POut.Long(clinic.ClinicNum)
				+" AND patient.PatNum=appointment.PatNum";
			table=Db.GetTable(command);
			if(table.Rows.Count>0) {
				string pats="";
				for(int i=0;i<table.Rows.Count;i++) {
					pats+="\r";
					if(i==15) {
						pats+=Lans.g("Clinics","And")+" "+(table.Rows.Count-i)+" "+Lans.g("Clinics","others");
						break;
					}
					pats+=table.Rows[i]["LName"].ToString()+", "+table.Rows[i]["FName"].ToString();
				}
				throw new Exception(Lans.g("Clinics","Cannot delete clinic because the following patients have appointments using it:")+pats);
			}
			#endregion
			#region Procedures
			//reassign procedure.ClinicNum=0 if the procs are status D.
			command="UPDATE procedurelog SET ClinicNum=0 WHERE ProcStatus="+POut.Int((int)ProcStat.D);
			Db.NonQ(command);
			command="SELECT patient.LName,patient.FName FROM patient,procedurelog "
				+"WHERE procedurelog.ClinicNum ="+POut.Long(clinic.ClinicNum)
				+" AND patient.PatNum=procedurelog.PatNum";
			table=Db.GetTable(command);
			if(table.Rows.Count>0) {
				string pats="";
				for(int i=0;i<table.Rows.Count;i++) {
					pats+="\r";
					if(i==15) {
						pats+=Lans.g("Clinics","And")+" "+(table.Rows.Count-i)+" "+Lans.g("Clinics","others");
						break;
					}
					pats+=table.Rows[i]["LName"].ToString()+", "+table.Rows[i]["FName"].ToString();
				}
				throw new Exception(Lans.g("Clinics","Cannot delete clinic because the following patients have procedures using it:")+pats);
			}
			#endregion
			#region Operatories
			command="SELECT OpName FROM operatory "
				+"WHERE ClinicNum ="+POut.Long(clinic.ClinicNum);
			table=Db.GetTable(command);
			if(table.Rows.Count>0) {
				string ops="";
				for(int i=0;i<table.Rows.Count;i++) {
					ops+="\r";
					if(i==15) {
						ops+=Lans.g("Clinics","And")+" "+(table.Rows.Count-i)+" "+Lans.g("Clinics","others");
						break;
					}
					ops+=table.Rows[i]["OpName"].ToString();
				}
				throw new Exception(Lans.g("Clinics","Cannot delete clinic because the following operatories are using it:")+ops);
			}
			#endregion
			#region Userod
			command="SELECT UserName FROM userod "
				+"WHERE ClinicNum ="+POut.Long(clinic.ClinicNum);
			table=Db.GetTable(command);
			if(table.Rows.Count>0) {
				string userNames="";
				for(int i=0;i<table.Rows.Count;i++) {
					userNames+="\r";
					if(i==15) {
						userNames+=Lans.g("Clinics","And")+" "+(table.Rows.Count-i)+" "+Lans.g("Clinics","others");
						break;
					}
					userNames+=table.Rows[i]["UserName"].ToString();
				}
				throw new Exception(Lans.g("Clinics","Cannot delete clinic because the following Open Dental users are using it:")+userNames);
			}
			#endregion
			//End checking for dependencies.
			//Clinic is not being used, OK to delete.
			//Delete clinic specific program properties.
			command="DELETE FROM programproperty WHERE ClinicNum="+POut.Long(clinic.ClinicNum)+" AND ClinicNum!=0";//just in case a programming error tries to delete an invalid clinic.
			Db.NonQ(command);
			Crud.ClinicCrud.Delete(clinic.ClinicNum);
		}

		///<summary>Returns null if clinic not found.  Pulls from cache.</summary>
		public static Clinic GetClinic(long clinicNum) {
			//No need to check RemotingRole; no call to db.
			Clinic[] arrayClinics=GetList();
			for(int i=0;i<arrayClinics.Length;i++){
				if(arrayClinics[i].ClinicNum==clinicNum){
					return arrayClinics[i].Copy();
				}
			}
			return null;
		}

		///<summary>Pulls from cache.  Can contain a null clinic if not found.</summary>
		public static List<Clinic> GetClinics(List<long> listClinicNums) {
			//No need to check RemotingRole; no call to db.
			List<Clinic> listClinics=new List<Clinic>();
			for(int i=0;i<listClinicNums.Count;i++) {
				if(listClinicNums[i]==0) {
					continue;
				}
				listClinics.Add(GetClinic(listClinicNums[i]));
			}
			return listClinics;
		}

		///<summary>Returns the patient's clinic based on the recall passed in.
		///If the patient is no longer associated to a clinic, 
		///  returns the clinic associated to the appointment (scheduled or completed) with the largest date.
		///Returns null if the patient doesn't have a clinic or if the clinics feature is not activate.</summary>
		public static Clinic GetClinicForRecall(long recallNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Clinic>(MethodBase.GetCurrentMethod(),recallNum);
			}
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				return null;
			}
			List<Clinic> listClinics=Clinics.GetList().ToList();
			string command="SELECT patient.ClinicNum FROM patient "
				+"INNER JOIN recall ON patient.PatNum=recall.PatNum "
				+"WHERE recall.RecallNum="+POut.Long(recallNum)+" "
				+DbHelper.LimitAnd(1);
			long patientClinicNum=PIn.Long(DataCore.GetScalar(command));
			if(patientClinicNum>0) {
				return listClinics.FirstOrDefault(x => x.ClinicNum==patientClinicNum);
			}
			//Patient does not have an assigned clinic.  Grab the clinic from a scheduled or completed appointment with the largest date.
			command=@"SELECT appointment.ClinicNum,appointment.AptDateTime 
				FROM appointment
				INNER JOIN recall ON appointment.PatNum=recall.PatNum AND recall.RecallNum="+POut.Long(recallNum)+@"
				WHERE appointment.AptStatus IN ("+POut.Int((int)ApptStatus.Scheduled)+","+POut.Int((int)ApptStatus.Complete)+")"+@"
				ORDER BY AptDateTime DESC";
			command=DbHelper.LimitOrderBy(command,1);
			long appointmentClinicNum=PIn.Long(DataCore.GetScalar(command));
			if(appointmentClinicNum>0) {
				return listClinics.FirstOrDefault(x => x.ClinicNum==appointmentClinicNum);
			}
			return null;
		}

		///<summary>Gets a list of all clinics.  Doesn't use the cache.</summary>
		public static List<Clinic> GetClinicsNoCache() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Clinic>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM clinic";
			return Crud.ClinicCrud.SelectMany(command);
		}

		///<summary>Returns an empty string for invalid clinicNums.</summary>
		public static string GetDesc(long clinicNum) {
			//No need to check RemotingRole; no call to db.
			Clinic[] arrayClinics=GetList();
			for(int i=0;i<arrayClinics.Length;i++){
				if(arrayClinics[i].ClinicNum==clinicNum){
					return arrayClinics[i].Description;
				}
			}
			return "";
		}
	
		///<summary>Returns practice default for invalid clinicNums.</summary>
		public static PlaceOfService GetPlaceService(long clinicNum) {
			//No need to check RemotingRole; no call to db.
			Clinic[] arrayClinics=GetList(); 
			for(int i=0;i<arrayClinics.Length;i++){
				if(arrayClinics[i].ClinicNum==clinicNum){
					return arrayClinics[i].DefaultPlaceService;
				}
			}
			return (PlaceOfService)PrefC.GetLong(PrefName.DefaultProcedurePlaceService);
			//return PlaceOfService.Office;
		}

		///<summary>Clinics cannot be hidden, so if clinicNum=0, this will return -1.</summary>
		public static int GetIndex(long clinicNum) {
			//No need to check RemotingRole; no call to db.
			Clinic[] arrayClinics=GetList();
			for(int i=0;i<arrayClinics.Length;i++) {
				if(arrayClinics[i].ClinicNum==clinicNum) {
					return i;
				}
			}
			return -1;
		}

		///<summary>Used by HL7 when parsing incoming messages.  Returns the ClinicNum of the clinic with Description matching exactly (not case sensitive) the description provided.  Returns 0 if no clinic is found with this exact description.  There may be more than one clinic with the same description, but this will return the first one in the list.</summary>
		public static long GetByDesc(string description) {
			//No need to check RemotingRole; no call to db.
			Clinic[] arrayClinics=GetList();
			for(int i=0;i<arrayClinics.Length;i++) {
				if(arrayClinics[i].Description.ToLower()==description.ToLower()) {
					return arrayClinics[i].ClinicNum;
				}
			}
			return 0;
		}

		///<summary>Returns a list of clinics the curUser has permission to access.  If the user is restricted to a clinic, the list will contain a single clinic.  If the user is not restricted, the list will contain all of the clinics.  In the future, users may be restricted to multiple clinics and this will allow the list returned to contain a subset of all clinics.</summary>
		public static List<Clinic> GetForUserod(Userod curUser) {
			List<Clinic> listClinics=new List<Clinic>();
			//user is restricted to a single clinic, so return a list with only that clinic in it
			if(curUser.ClinicIsRestricted && curUser.ClinicNum>0) {//for now a user can only be restricted to a single clinic, but in the future we will likely allow users to be restricted to more than one clinic
				listClinics.Add(GetClinic(curUser.ClinicNum));
				return listClinics;
			}
			Clinic[] arrayClinics=GetList();
			for(int i=0;i<arrayClinics.Length;i++) {
				listClinics.Add(arrayClinics[i].Copy());
			}
			return listClinics;
		}

		///<summary>This method returns true if the given provider is set as the default clinic provider for any clinic.</summary>
		public static bool IsDefaultClinicProvider(long provNum) {
			//No need to check RemotingRole; no call to db.
			Clinic[] arrayClinics=GetList();
			for(int i=0;i<arrayClinics.Length;i++) {
				if(arrayClinics[i].DefaultProv==provNum) {
					return true;
				}
			}
			return false;
		}

		///<summary>This method returns true if the given provider is set as the default ins billing provider for any clinic.</summary>
		public static bool IsInsBillingProvider(long provNum) {
			//No need to check RemotingRole; no call to db.
			Clinic[] arrayClinics=GetList();
			for(int i=0;i<arrayClinics.Length;i++) {
				if(arrayClinics[i].InsBillingProv==provNum) {
					return true;
				}
			}
			return false;
		}

		public static bool IsTextingEnabled(long clinicNum) {
			Clinic clinic=GetClinic(clinicNum);
			if(clinic==null) {
				return false;
			}
			return clinic.SmsContractDate.Year>1880;
		}

		///<summary>Provide the currently selected clinic num (FormOpenDental.ClinicNum).  If clinics are not enabled, this will return true if the pref
		///PracticeIsMedicalOnly is true.  If clinics are enabled, this will return true if either the headquarters 'clinic' is selected
		///(FormOpenDental.ClinicNum=0) and the pref PracticeIsMedicalOnly is true OR if the currently selected clinic's IsMedicalOnly flag is true.
		///Otherwise returns false.</summary>
		public static bool IsMedicalPracticeOrClinic(long clinicNum) {
			if(clinicNum==0) {//either headquarters is selected or the clinics feature is not enabled, use practice pref
				return PrefC.GetBool(PrefName.PracticeIsMedicalOnly);
			}
			Clinic clinicCur=Clinics.GetClinic(clinicNum);
			if(clinicCur!=null) {
				return clinicCur.IsMedicalOnly;
			}
			return false;
		}

		///<summary>Returns a clinic object with ClinicNum=0, and values filled using practice level preferences. 
		/// Caution: do not attempt to save the clinic back to the DB. This should be used for read only purposes.</summary>
		public static Clinic GetPracticeAsClinicZero() {
			return new Clinic {
				ClinicNum=0,
				Description=PrefC.GetString(PrefName.PracticeTitle),
				Address=PrefC.GetString(PrefName.PracticeAddress),
				Address2=PrefC.GetString(PrefName.PracticeAddress2),
				City=PrefC.GetString(PrefName.PracticeCity),
				State=PrefC.GetString(PrefName.PracticeST),
				Zip=PrefC.GetString(PrefName.PracticeZip),
				BillingAddress=PrefC.GetString(PrefName.PracticeBillingAddress),
				BillingAddress2=PrefC.GetString(PrefName.PracticeBillingAddress2),
				BillingCity=PrefC.GetString(PrefName.PracticeBillingCity),
				BillingState=PrefC.GetString(PrefName.PracticeBillingST),
				BillingZip=PrefC.GetString(PrefName.PracticeBillingZip),
				PayToAddress=PrefC.GetString(PrefName.PracticePayToAddress),
				PayToAddress2=PrefC.GetString(PrefName.PracticePayToAddress2),
				PayToCity=PrefC.GetString(PrefName.PracticePayToCity),
				PayToState=PrefC.GetString(PrefName.PracticePayToST),
				PayToZip=PrefC.GetString(PrefName.PracticePayToZip),
				Phone=PrefC.GetString(PrefName.PracticePhone),
				BankNumber=PrefC.GetString(PrefName.PracticeBankNumber),
				DefaultPlaceService=(PlaceOfService)PrefC.GetInt(PrefName.DefaultProcedurePlaceService),
				InsBillingProv=PrefC.GetLong(PrefName.InsBillingProv),
				Fax=PrefC.GetString(PrefName.PracticeFax),
				EmailAddressNum=PrefC.GetLong(PrefName.EmailDefaultAddressNum),
				DefaultProv=PrefC.GetLong(PrefName.PracticeDefaultProv),
				SmsContractDate=PrefC.GetDate(PrefName.SmsContractDate),
				SmsMonthlyLimit=PrefC.GetDouble(PrefName.SmsMonthlyLimit),
				IsMedicalOnly=PrefC.GetBool(PrefName.PracticeIsMedicalOnly)
			};
		}

	}
	


}













