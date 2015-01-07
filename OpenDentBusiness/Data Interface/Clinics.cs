using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary>There is no cache for clinics.  We assume they will almost never change.</summary>
	public class Clinics {
		///<summary>Clinics cannot be hidden or deleted, so there is only one list.</summary>
		private static Clinic[] _list;
		private static object _lockObj=new object();

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
					pats+=table.Rows[i][0].ToString()+", "+table.Rows[i][1].ToString();
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
					pats+=table.Rows[i][0].ToString()+", "+table.Rows[i][1].ToString();
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
					pats+=table.Rows[i][0].ToString()+", "+table.Rows[i][1].ToString();
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
					pats+=table.Rows[i][0].ToString()+", "+table.Rows[i][1].ToString();
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
					pats+=table.Rows[i][0].ToString()+", "+table.Rows[i][1].ToString();
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
					ops+=table.Rows[i][0].ToString();
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
					userNames+=table.Rows[i][0].ToString();
				}
				throw new Exception(Lans.g("Clinics","Cannot delete clinic because the following Open Dental users are using it:")+userNames);
			}
			#endregion
			//End checking for dependencies.
			//Clinic is not being used, OK to delete.
			command= "DELETE FROM clinic" 
				+" WHERE ClinicNum = "+POut.Long(clinic.ClinicNum);
			Db.NonQ(command);
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

	}
	


}













