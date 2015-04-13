using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CodeBase;

namespace OpenDentBusiness{
	
	///<summary></summary>
	public class Patients{
		
		///<summary>Returns a Family object for the supplied patNum.  Use Family.GetPatient to extract the desired patient from the family.</summary>
		public static Family GetFamily(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Family>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command=//GetFamilySelectCommand(patNum);
				"SELECT patient.*,CASE WHEN Guarantor!=PatNum THEN 1 ELSE 0 END AS IsNotGuar FROM patient WHERE Guarantor = ("
				+"SELECT Guarantor FROM patient WHERE PatNum="+POut.Long(patNum)+") "
				+"ORDER BY IsNotGuar,Birthdate";
			Family fam=new Family();
			List<Patient> patients=Crud.PatientCrud.SelectMany(command);
			foreach(Patient patient in patients) {
				patient.Age = DateToAge(patient.Birthdate);
			}
			fam.ListPats=new Patient[patients.Count];
			patients.CopyTo(fam.ListPats,0);
			return fam;
		}

		/// <summary>Returns a patient, or null, based on an internally defined or externally defined globaly unique identifier.  This can be an OID, GUID, IID, UUID, etc.</summary>
		/// <param name="IDNumber">The extension portion of the GUID/OID.  Example: 333224444 if using SSN as a the unique identifier</param>
		/// <param name="OID">root OID that the IDNumber extends.  Example: 2.16.840.1.113883.4.1 is the OID for the Social Security Numbers.</param>
		public static Patient GetByGUID(string IDNumber, string OID){
			if(OID==OIDInternals.GetForType(IdentifierType.Patient).IDRoot) {//OID matches the localy defined patnum OID.
				return Patients.GetPat(PIn.Long(IDNumber));
			}
			else {
				OIDExternal oidExt=OIDExternals.GetByRootAndExtension(OID,IDNumber);
				if(oidExt==null || oidExt.IDType!=IdentifierType.Patient) {
					return null;//OID either not found, or does not represent a patient.
				}
				return Patients.GetPat(oidExt.IDInternal);
			}
		}
							//patcur=Patients.GetByGUID(fields[3].Split('~')[i].Split('^')[1],								//ID Number
							//													fields[3].Split('~')[i].Split('^')[4].Split('&')[2],	//Assigning Authority ID
							//													fields[3].Split('~')[i].Split('^')[5]);								//Identifier Type Code

		/*
		public static string GetFamilySelectCommand(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),patNum);
			}
			string command= 
				"SELECT guarantor FROM patient "
				+"WHERE patnum = '"+POut.Long(patNum)+"'";
 			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0){
				return null;
			}
			command= 
				"SELECT patient.* "
				+"FROM patient "
				+"WHERE Guarantor = '"+table.Rows[0][0].ToString()+"'"
				+" ORDER BY Guarantor!=PatNum,Birthdate";
			return command;
		}*/

		///<summary>This is a way to get a single patient from the database if you don't already have a family object to use.  Will return null if not found.</summary>
		public static Patient GetPat(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Patient>(MethodBase.GetCurrentMethod(),patNum);
			} 
			if(patNum==0) {
				return null;
			}
			string command="SELECT * FROM patient WHERE PatNum="+POut.Long(patNum);
			Patient pat=null;
			try {
				pat=Crud.PatientCrud.SelectOne(patNum);
			}
			catch { }
			if(pat==null) {
				return null;//used in eCW bridge
			}
			pat.Age = DateToAge(pat.Birthdate);
			return pat;
		}

		///<summary>Will return null if not found.</summary>
		public static Patient GetPatByChartNumber(string chartNumber) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Patient>(MethodBase.GetCurrentMethod(),chartNumber);
			}
			if(chartNumber=="") {
				return null;
			}
			string command="SELECT * FROM patient WHERE ChartNumber='"+POut.String(chartNumber)+"'";
			Patient pat=null;
			try {
				pat=Crud.PatientCrud.SelectOne(command);
			}
			catch { }
			if(pat==null) {
				return null;
			}
			pat.Age = DateToAge(pat.Birthdate);
			return pat;
		}

		///<summary>Will return null if not found.</summary>
		public static Patient GetPatBySSN(string ssn) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Patient>(MethodBase.GetCurrentMethod(),ssn);
			}
			if(ssn=="") {
				return null;
			}
			string command="SELECT * FROM patient WHERE SSN='"+POut.String(ssn)+"'";
			Patient pat=null;
			try {
				pat=Crud.PatientCrud.SelectOne(command);
			}
			catch { }
			if(pat==null) {
				return null;
			}
			pat.Age = DateToAge(pat.Birthdate);
			return pat;
		}

		public static List<Patient> GetChangedSince(DateTime changedSince) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod(),changedSince);
			}
			string command="SELECT * FROM patient WHERE DateTStamp > "+POut.DateT(changedSince);
			//command+=" "+DbHelper.LimitAnd(1000);
			return Crud.PatientCrud.SelectMany(command);
		}

		///<summary>Used if the number of records are very large, in which case using GetChangedSince(DateTime changedSince) is not the preffered route due to memory problems caused by large recordsets. </summary>
		public static List<long> GetChangedSincePatNums(DateTime changedSince) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),changedSince);
			}
			string command="SELECT PatNum From patient WHERE DateTStamp > "+POut.DateT(changedSince);
			DataTable dt=Db.GetTable(command);
			List<long> patnums = new List<long>(dt.Rows.Count);
			for(int i=0;i<dt.Rows.Count;i++) {
				patnums.Add(PIn.Long(dt.Rows[i]["PatNum"].ToString()));
			}
			return patnums;
		}

		/// <summary>Gets PatNums of patients whose online password is not blank</summary>
		public static List<long> GetPatNumsEligibleForSynch() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT PatNum From patient where OnlinePassword!=''";
			DataTable dt=Db.GetTable(command);
			List<long> patnums = new List<long>(dt.Rows.Count);
			for(int i=0;i<dt.Rows.Count;i++) {
				patnums.Add(PIn.Long(dt.Rows[i]["PatNum"].ToString()));
			}
			return patnums;
		}

		/// <summary>Gets PatNums of patients whose online password is  blank</summary>
		public static List<long> GetPatNumsForDeletion() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT PatNum From patient where OnlinePassword=''";
			DataTable dt=Db.GetTable(command);
			List<long> patnums = new List<long>(dt.Rows.Count);
			for(int i=0;i<dt.Rows.Count;i++) {
				patnums.Add(PIn.Long(dt.Rows[i]["PatNum"].ToString()));
			}
			return patnums;
		}

		///<summary>ONLY for new patients. Set includePatNum to true for use the patnum from the import function.  Used in HL7.  Otherwise, uses InsertID to fill PatNum.</summary>
		public static long Insert(Patient pat,bool useExistingPK) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				pat.PatNum=Meth.GetLong(MethodBase.GetCurrentMethod(),pat,useExistingPK);
				return pat.PatNum;
			}
			if(!useExistingPK) {
				return Crud.PatientCrud.Insert(pat);
			}
			return Crud.PatientCrud.Insert(pat,useExistingPK);
		}

		///<summary>Updates only the changed columns and returns the number of rows affected.  Supply the old Patient object to compare for changes.</summary>
		public static void Update(Patient patient,Patient oldPatient) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),patient,oldPatient);
				return;
			}
			Crud.PatientCrud.Update(patient,oldPatient);
		}

		//This can never be used anymore, or it will mess up 
		///<summary>This is only used when entering a new patient and user clicks cancel.  It used to actually delete the patient, but that will mess up UAppoint synch function.  DateTStamp needs to track deleted patients. So now, the PatStatus is simply changed to 4.</summary>
		public static void Delete(Patient pat) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),pat);
				return;
			}
			string command="UPDATE patient SET PatStatus="+POut.Long((int)PatientStatus.Deleted)+", "
				+"Guarantor=PatNum "
				+"WHERE PatNum ="+pat.PatNum.ToString();
			Db.NonQ(command);
		}

 		///<summary>Only used for the Select Patient dialog.  Pass in a billing type of 0 for all billing types.</summary>
		public static DataTable GetPtDataTable(bool limit,string lname,string fname,string phone,
			string address,bool hideInactive,string city,string state,string ssn,string patnum,string chartnumber,
			long billingtype,bool guarOnly,bool showArchived,DateTime birthdate,
			long siteNum,string subscriberId,string email,string country,string clinicNums)
		{
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),limit,lname,fname,phone,address,hideInactive,city,state,ssn,patnum,chartnumber,billingtype,guarOnly,showArchived,birthdate,siteNum,subscriberId,email,country,clinicNums);
			}
			string billingsnippet=" ";
			if(billingtype!=0){
				billingsnippet+="AND BillingType="+POut.Long(billingtype)+" ";
			}
			/*for(int i=0;i<billingtypes.Length;i++){//if length==0, it will get all billing types
				if(i==0){
					billingsnippet+="AND (";
				}
				else{
					billingsnippet+="OR ";
				}
				billingsnippet+="BillingType ='"+billingtypes[i].ToString()+"' ";
				if(i==billingtypes.Length-1){//if there is only one row, this will also be triggered.
					billingsnippet+=") ";
				}
			}*/
			string phonedigits="";
			for(int i=0;i<phone.Length;i++){
				if(Regex.IsMatch(phone[i].ToString(),"[0-9]")){
					phonedigits=phonedigits+phone[i];
				}
			}
			string regexp="";
			for(int i=0;i<phonedigits.Length;i++){
				if(i!=0){
					regexp+="[^0-9]*";//zero or more intervening digits that are not numbers
				}
				regexp+=phonedigits[i];
			}
			string command="SELECT patient.PatNum,LName,FName,MiddleI,Preferred,Birthdate,SSN,HmPhone,WkPhone,Address,PatStatus"
				+",BillingType,ChartNumber,City,State,PriProv,SiteNum,Email,Country,patient.ClinicNum ";
			if(PrefC.GetBool(PrefName.DistributorKey)) {//if for OD HQ, so never going to be Oracle
				command+=",GROUP_CONCAT(DISTINCT phonenumber.PhoneNumberVal) AS OtherPhone ";//this customer might have multiple extra phone numbers that match the param.
			}
			if(subscriberId!=""){
				command+=",inssub.SubscriberId ";
			}
			command+="FROM patient ";
			if(PrefC.GetBool(PrefName.DistributorKey)) {//if for OD HQ, so never going to be Oracle
				command+="LEFT JOIN phonenumber ON phonenumber.PatNum=patient.PatNum ";
				if(regexp!="") {
					command+="AND phonenumber.PhoneNumberVal REGEXP '"+POut.String(regexp)+"' ";
				}
			}
			if(subscriberId!=""){
				command+="LEFT JOIN patplan ON patplan.PatNum=patient.PatNum "
					+"LEFT JOIN inssub ON patplan.InsSubNum=inssub.InsSubNum "
					+"LEFT JOIN insplan ON inssub.PlanNum=insplan.PlanNum ";
			}
			command+="WHERE PatStatus != '4' ";//not status 'deleted'
			if(DataConnection.DBtype==DatabaseType.MySql) {//LIKE is case insensitive in mysql.
				if(lname.Length>0) {
					if(limit) {//normal behavior is fast
						if(PrefC.GetBool(PrefName.DistributorKey)) {
							command+="AND (LName LIKE '"+POut.String(lname)+"%' OR Preferred LIKE '"+POut.String(lname)+"%') ";
						}
						else {
							command+="AND LName LIKE '"+POut.String(lname)+"%' ";
						}
					}
					else {//slower, but more inclusive.  User explicitly looking for all matches.
						if(PrefC.GetBool(PrefName.DistributorKey)) {
							command+="AND (LName LIKE '"+POut.String(lname)+"%' OR Preferred LIKE '"+POut.String(lname)+"%') ";
						}
						else {
							command+="AND LName LIKE '"+POut.String(lname)+"%' ";
						}
					}
				}
				if(fname.Length>0){
						if(PrefC.GetBool(PrefName.DistributorKey)) {
							command+="AND (FName LIKE '"+POut.String(fname)+"%' OR Preferred LIKE '"+POut.String(fname)+"%') ";
						}
						else {
							command+="AND FName LIKE '"+POut.String(fname)+"%' ";
						}
				}
			}
			else {//oracle, case matters in a like statement
				if(lname.Length>0) {
					if(limit) {
						command+="AND LOWER(LName) LIKE '"+POut.String(lname)+"%' ";
					}
					else {
						command+="AND LOWER(LName) LIKE '%"+POut.String(lname)+"%' ";
					}
				}
				if(fname.Length>0) {
					command+="AND LOWER(FName) LIKE '"+POut.String(fname)+"%' ";
				}
			}
			if(regexp!="") {
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command+="AND (HmPhone REGEXP '"+POut.String(regexp)+"' "
						+"OR WkPhone REGEXP '"+POut.String(regexp)+"' "
						+"OR WirelessPhone REGEXP '"+POut.String(regexp)+"' ";
					if(PrefC.GetBool(PrefName.DistributorKey)) {//if for OD HQ, so never going to be Oracle
						command+="OR phonenumber.PhoneNumberVal REGEXP '"+POut.String(regexp)+"' ";
					}
					command+=") ";
				} 
				else {//oracle
					command+="AND ((SELECT REGEXP_INSTR(p.HmPhone,'"+POut.String(regexp)+"') FROM dual)<>0"
						+"OR (SELECT REGEXP_INSTR(p.WkPhone,'"+POut.String(regexp)+"') FROM dual)<>0 "
						+"OR (SELECT REGEXP_INSTR(p.WirelessPhone,'"+POut.String(regexp)+"') FROM dual)<>0) ";
				}
			}
			if(DataConnection.DBtype==DatabaseType.MySql) {
				command+=
					(address.Length>0?"AND Address LIKE '%"+POut.String(address)+"%' ":"")//LIKE is case insensitive in mysql.
					+(city.Length>0?"AND City LIKE '"+POut.String(city)+"%' ":"")//LIKE is case insensitive in mysql.
					+(state.Length>0?"AND State LIKE '"+POut.String(state)+"%' ":"")//LIKE is case insensitive in mysql.
					+(ssn.Length>0?"AND SSN LIKE '"+POut.String(ssn)+"%' ":"")//LIKE is case insensitive in mysql.
					+(patnum.Length>0?"AND patient.PatNum LIKE '"+POut.String(patnum)+"%' ":"")//LIKE is case insensitive in mysql.
					+(chartnumber.Length>0?"AND ChartNumber LIKE '"+POut.String(chartnumber)+"%' ":"")//LIKE is case insensitive in mysql.
					+(email.Length>0?"AND Email LIKE '%"+POut.String(email)+"%' ":"")//LIKE is case insensitive in mysql.
					+(country.Length>0?"AND Country LIKE '%"+POut.String(country)+"%' ":"");//LIKE is case insensitive in mysql.
			}
			else {//oracle
				command+=
					(address.Length>0?"AND LOWER(Address) LIKE '%"+POut.String(address).ToLower()+"%' ":"")//case matters in a like statement in oracle.
					+(city.Length>0?"AND LOWER(City) LIKE '"+POut.String(city).ToLower()+"%' ":"")//case matters in a like statement in oracle.
					+(state.Length>0?"AND LOWER(State) LIKE '"+POut.String(state).ToLower()+"%' ":"")//case matters in a like statement in oracle.
					+(ssn.Length>0?"AND LOWER(SSN) LIKE '"+POut.String(ssn).ToLower()+"%' ":"")//In case an office uses this field for something else.
					+(patnum.Length>0?"AND patient.PatNum LIKE '"+POut.String(patnum)+"%' ":"")//case matters in a like statement in oracle.
					+(chartnumber.Length>0?"AND LOWER(ChartNumber) LIKE '"+POut.String(chartnumber).ToLower()+"%' ":"")//case matters in a like statement in oracle.
					+(email.Length>0?"AND LOWER(Email) LIKE '%"+POut.String(email).ToLower()+"%' ":"")//LIKE is case insensitive in mysql.
					+(country.Length>0?"AND LOWER(Country) LIKE '%"+POut.String(country).ToLower()+"%' ":"");//LIKE is case insensitive in mysql.
			}
			if(birthdate.Year>1880 && birthdate.Year<2100){
				command+="AND Birthdate ="+POut.Date(birthdate)+" ";
			}
			command+=billingsnippet;
			if(hideInactive){
				command+="AND PatStatus != '"+POut.Int((int)PatientStatus.Inactive)+"' ";
			}
			if(!showArchived) {
				command+="AND PatStatus != '"+POut.Int((int)PatientStatus.Archived)+"' AND PatStatus != '"+POut.Int((int)PatientStatus.Deceased)+"' ";
			}
			//if(showProspectiveOnly) {
			//	command+="AND PatStatus = "+POut.Int((int)PatientStatus.Prospective)+" ";
			//}
			//if(!showProspectiveOnly) {
			//	command+="AND PatStatus != "+POut.Int((int)PatientStatus.Prospective)+" ";
			//}
			if(guarOnly){
				command+="AND patient.PatNum = Guarantor ";
			}
			if(clinicNums!="") {
				command+="AND patient.Guarantor IN ( "
				+"SELECT DISTINCT Guarantor FROM patient "
				+"LEFT JOIN procedurelog ON patient.PatNum=procedurelog.PatNum "
					+"AND (procedurelog.ProcStatus="+POut.Int((int)ProcStat.TP)+" OR procedurelog.ProcStatus="+POut.Int((int)ProcStat.C)+") "
					+"AND procedurelog.ClinicNum IN ("+POut.String(clinicNums)+") "
				+"WHERE patient.PatStatus !="+POut.Int((int)PatientStatus.Deleted)+" "
				+"AND (procedurelog.PatNum IS NOT NULL OR patient.ClinicNum IN (0,"+POut.String(clinicNums)+"))) "; //Includes patients that are not assigned to any clinic.  May need to restrict selection of these patients in the future.
			}
			if(siteNum>0) {
				command+="AND SiteNum="+POut.Long(siteNum)+" ";
			}
			if(subscriberId!=""){
				command+="AND inssub.SubscriberId LIKE '%"+POut.String(subscriberId)+"%' ";
			}
			if(PrefC.GetBool(PrefName.DistributorKey)) {//if for OD HQ
				command+="GROUP BY patient.PatNum ";
			}
			command+="ORDER BY LName,FName ";
			if(limit){
				command=DbHelper.LimitOrderBy(command,40);
			}
			//MessageBox.Show(command);
 			DataTable table=Db.GetTable(command);
			DataTable PtDataTable=table.Clone();//does not copy any data
			PtDataTable.TableName="table";
			PtDataTable.Columns.Add("age");
			PtDataTable.Columns.Add("clinic");
			PtDataTable.Columns.Add("site");
			for(int i=0;i<PtDataTable.Columns.Count;i++){
				PtDataTable.Columns[i].DataType=typeof(string);
			}
			//if(limit && table.Rows.Count==36){
			//	retval=true;
			//}
			DataRow r;
			DateTime date;
			for(int i=0;i<table.Rows.Count;i++){//table.Rows.Count && i<44;i++){
				r=PtDataTable.NewRow();
				//PatNum,LName,FName,MiddleI,Preferred,Birthdate,SSN,HmPhone,WkPhone,Address,PatStatus"
				//+",BillingType,ChartNumber,City,State
				r["PatNum"]=table.Rows[i]["PatNum"].ToString();
				r["LName"]=table.Rows[i]["LName"].ToString();
				r["FName"]=table.Rows[i]["FName"].ToString();
				r["MiddleI"]=table.Rows[i]["MiddleI"].ToString();
				r["Preferred"]=table.Rows[i]["Preferred"].ToString();
				date=PIn.Date(table.Rows[i]["Birthdate"].ToString());
				if(date.Year>1880){
					r["age"]=DateToAge(date);
					r["Birthdate"]=date.ToShortDateString();
				}
				else{
					r["age"]="";
					r["Birthdate"]="";
				}				
				r["SSN"]=table.Rows[i]["SSN"].ToString();
				r["HmPhone"]=table.Rows[i]["HmPhone"].ToString();
				r["WkPhone"]=table.Rows[i]["WkPhone"].ToString();
				r["Address"]=table.Rows[i]["Address"].ToString();
				r["PatStatus"]=((PatientStatus)PIn.Long(table.Rows[i]["PatStatus"].ToString())).ToString();
				r["BillingType"]=DefC.GetName(DefCat.BillingTypes,PIn.Long(table.Rows[i]["BillingType"].ToString()));
				r["ChartNumber"]=table.Rows[i]["ChartNumber"].ToString();
				r["City"]=table.Rows[i]["City"].ToString();
				r["State"]=table.Rows[i]["State"].ToString();
				r["PriProv"]=Providers.GetAbbr(PIn.Long(table.Rows[i]["PriProv"].ToString()));
				r["site"]=Sites.GetDescription(PIn.Long(table.Rows[i]["SiteNum"].ToString()));
				r["Email"]=table.Rows[i]["Email"].ToString();
				r["Country"]=table.Rows[i]["Country"].ToString();
				r["clinic"]=Clinics.GetDesc(PIn.Long(table.Rows[i]["ClinicNum"].ToString()));
				if(PrefC.GetBool(PrefName.DistributorKey)) {//if for OD HQ
					r["OtherPhone"]=table.Rows[i]["OtherPhone"].ToString();
				}
				PtDataTable.Rows.Add(r);
			}
			return PtDataTable;
		}

		///<summary>Used when filling appointments for an entire day. Gets a list of Pats, multPats, of all the specified patients.  Then, use GetOnePat to pull one patient from this list.  This process requires only one call to the database.</summary>
		public static Patient[] GetMultPats(List<long> patNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Patient[]>(MethodBase.GetCurrentMethod(),patNums);
			}
			DataTable table=new DataTable();
			if(patNums.Count>0) {
				string command="SELECT * FROM patient WHERE PatNum IN ("+String.Join<long>(",",patNums)+") ";
				table=Db.GetTable(command);
			}
			Patient[] multPats=Crud.PatientCrud.TableToList(table).ToArray();
			return multPats;
		}

		///<summary>First call GetMultPats to fill the list of multPats. Then, use this to return one patient from that list.</summary>
		public static Patient GetOnePat(Patient[] multPats,long patNum) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<multPats.Length;i++){
				if(multPats[i].PatNum==patNum){
					return multPats[i];
				}
			}
			return new Patient();
		}

		/// <summary>Gets nine of the most useful fields from the db for the given patnum.  If invalid PatNum, returns new Patient rather than null.</summary>
		public static Patient GetLim(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Patient>(MethodBase.GetCurrentMethod(),patNum);
			}
			if(patNum==0){
				return new Patient();
			}
			string command= 
				"SELECT PatNum,LName,FName,MiddleI,Preferred,CreditType,Guarantor,HasIns,SSN " 
				+"FROM patient "
				+"WHERE PatNum = '"+patNum.ToString()+"'";
 			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0){
				return new Patient();
			}
			Patient Lim=new Patient();
			Lim.PatNum     = PIn.Long   (table.Rows[0][0].ToString());
			Lim.LName      = PIn.String(table.Rows[0][1].ToString());
			Lim.FName      = PIn.String(table.Rows[0][2].ToString());
			Lim.MiddleI    = PIn.String(table.Rows[0][3].ToString());
			Lim.Preferred  = PIn.String(table.Rows[0][4].ToString());
			Lim.CreditType = PIn.String(table.Rows[0][5].ToString());
			Lim.Guarantor  = PIn.Long   (table.Rows[0][6].ToString());
			Lim.HasIns     = PIn.String(table.Rows[0][7].ToString());
			Lim.SSN        = PIn.String(table.Rows[0][8].ToString());
			return Lim;
		}
		
		///<summary>Gets the patient and provider balances for all patients in the family.  Used from the payment window to help visualize and automate the family splits.</summary>
		public static DataTable GetPaymentStartingBalances(long guarNum,long excludePayNum) {
			return GetPaymentStartingBalances(guarNum,excludePayNum,false);
		}

		///<summary>Gets the patient and provider balances for all patients in the family.  Used from the payment window to help visualize and automate the family splits. groupByProv means group by provider only not provider/clinic.</summary>
		public static DataTable GetPaymentStartingBalances(long guarNum,long excludePayNum,bool groupByProv) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),guarNum,excludePayNum,groupByProv);
			}
			//This method no longer uses a temporary table due to the problems it was causing replication users.
			//The in-memory table name was left as "tempfambal" for nostalgic purposes because veteran engineers know exactly where to look when "tempfambal" is mentioned.
			//This query will be using UNION ALLs so that duplicate-row removal does not occur. 
			string command=@"
					SELECT tempfambal.PatNum,tempfambal.ProvNum,
						tempfambal.ClinicNum,SUM(tempfambal.AmtBal) StartBal,
						SUM(tempfambal.AmtBal-tempfambal.InsEst) AfterIns,patient.FName,patient.Preferred,0.0 EndBal,
						CASE WHEN patient.Guarantor!=patient.PatNum THEN 1 ELSE 0 END IsNotGuar,patient.Birthdate
					FROM(
						/*Completed procedures*/
						(SELECT patient.PatNum,procedurelog.ProvNum,procedurelog.ClinicNum,SUM(procedurelog.ProcFee) AmtBal,0 InsEst
						FROM procedurelog,patient
						WHERE patient.PatNum=procedurelog.PatNum
						AND procedurelog.ProcStatus="+POut.Int((int)ProcStat.C)+@"
						AND patient.Guarantor="+POut.Long(guarNum)+@"
						GROUP BY patient.PatNum,procedurelog.ProvNum,procedurelog.ClinicNum)
					UNION ALL			
						/*Received insurance payments*/
						(SELECT patient.PatNum,claimproc.ProvNum,claimproc.ClinicNum,-SUM(claimproc.InsPayAmt)-SUM(claimproc.Writeoff) AmtBal,0 InsEst
						FROM claimproc,patient
						WHERE patient.PatNum=claimproc.PatNum
						AND (claimproc.Status="+POut.Int((int)ClaimProcStatus.Received)+@" 
							OR claimproc.Status="+POut.Int((int)ClaimProcStatus.Supplemental)+@" 
							OR claimproc.Status="+POut.Int((int)ClaimProcStatus.CapClaim)+@" 
							OR claimproc.Status="+POut.Int((int)ClaimProcStatus.CapComplete)+@")
						AND patient.Guarantor="+POut.Long(guarNum)+@"
						GROUP BY patient.PatNum,claimproc.ProvNum,claimproc.ClinicNum)
					UNION ALL
						/*Insurance estimates*/
						(SELECT patient.PatNum,claimproc.ProvNum,claimproc.ClinicNum,0 AmtBal,SUM(claimproc.InsPayEst)+SUM(claimproc.Writeoff) InsEst
						FROM claimproc,patient
						WHERE patient.PatNum=claimproc.PatNum
						AND claimproc.Status="+POut.Int((int)ClaimProcStatus.NotReceived)+@"
						AND patient.Guarantor="+POut.Long(guarNum)+@"
						GROUP BY patient.PatNum,claimproc.ProvNum,claimproc.ClinicNum)
					UNION ALL
						/*Adjustments*/
						(SELECT patient.PatNum,adjustment.ProvNum,adjustment.ClinicNum,SUM(adjustment.AdjAmt) AmtBal,0 InsEst
						FROM adjustment,patient
						WHERE patient.PatNum=adjustment.PatNum
						AND patient.Guarantor="+POut.Long(guarNum)+@"
						GROUP BY patient.PatNum,adjustment.ProvNum,adjustment.ClinicNum)
					UNION ALL
						/*Patient payments*/
						(SELECT patient.PatNum,paysplit.ProvNum,paysplit.ClinicNum,-SUM(SplitAmt) AmtBal,0 InsEst
						FROM paysplit,patient
						WHERE patient.PatNum=paysplit.PatNum
						AND paysplit.PayNum!="+POut.Long(excludePayNum)+@"
						AND patient.Guarantor="+POut.Long(guarNum)+@"
						AND paysplit.PayPlanNum=0
						GROUP BY patient.PatNum,paysplit.ProvNum,paysplit.ClinicNum)
					UNION ALL
						/*payplan princ reduction*/
						(SELECT patient.PatNum,payplancharge.ProvNum,payplancharge.ClinicNum,-SUM(payplancharge.Principal) AmtBal,0 InsEst
						FROM payplancharge,patient
						WHERE patient.PatNum=payplancharge.PatNum
						AND patient.Guarantor="+POut.Long(guarNum)+@"
						GROUP BY patient.PatNum,payplancharge.ProvNum,payplancharge.ClinicNum)
					) tempfambal,patient
					WHERE tempfambal.PatNum=patient.PatNum 
					GROUP BY tempfambal.PatNum,tempfambal.ProvNum,";
			if(!groupByProv) {
				command+=@"tempfambal.ClinicNum,";
			}
			command+=@"patient.FName,patient.Preferred";
			//Probably an unnecessary MySQL / Oracle split but I didn't want to affect the old GROUP BY functionality for MySQL just be Oracle is lame.
			if(DataConnection.DBtype==DatabaseType.MySql) {
				command+=@"
					HAVING ((StartBal>0.005 OR StartBal<-0.005) OR (AfterIns>0.005 OR AfterIns<-0.005))
					ORDER BY IsNotGuar,Birthdate,ProvNum,FName,Preferred";
			}
			else {//Oracle.
				command+=@",(CASE WHEN Guarantor!=patient.PatNum THEN 1 ELSE 0 END),Birthdate
					HAVING ((SUM(AmtBal)>0.005 OR SUM(AmtBal)<-0.005) OR (SUM(AmtBal-tempfambal.InsEst)>0.005 OR SUM(AmtBal-tempfambal.InsEst)<-0.005))
					ORDER BY IsNotGuar,patient.Birthdate,tempfambal.ProvNum,patient.FName,patient.Preferred";
			}
			return Db.GetTable(command);
		}

		///<summary></summary>
		public static void ChangeGuarantorToCur(Family Fam,Patient Pat){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),Fam,Pat);
				return;
			}
			//Move famfinurgnote to current patient:
			string command= "UPDATE patient SET "
				+"FamFinUrgNote = '"+POut.String(Fam.ListPats[0].FamFinUrgNote)+"' "
				+"WHERE PatNum = "+POut.Long(Pat.PatNum);
 			Db.NonQ(command);
			command= "UPDATE patient SET FamFinUrgNote = '' "
				+"WHERE PatNum = '"+Pat.Guarantor.ToString()+"'";
			Db.NonQ(command);
			//Move family financial note to current patient:
			command="SELECT FamFinancial FROM patientnote "
				+"WHERE PatNum = "+POut.Long(Pat.Guarantor);
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==1){
				command= "UPDATE patientnote SET "
					+"FamFinancial = '"+POut.String(table.Rows[0][0].ToString())+"' "
					+"WHERE PatNum = "+POut.Long(Pat.PatNum);
				Db.NonQ(command);
			}
			command= "UPDATE patientnote SET FamFinancial = '' "
				+"WHERE PatNum = "+POut.Long(Pat.Guarantor);
			Db.NonQ(command);
			//change guarantor of all family members:
			command= "UPDATE patient SET "
				+"Guarantor = "+POut.Long(Pat.PatNum)
				+" WHERE Guarantor = "+POut.Long(Pat.Guarantor);
			Db.NonQ(command);
		}
		
		///<summary></summary>
		public static void CombineGuarantors(Family Fam,Patient Pat){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),Fam,Pat);
				return;
			}
			//concat cur notes with guarantor notes
			string command= 
				"UPDATE patient SET "
				//+"addrnote = '"+POut.PString(FamilyList[GuarIndex].FamAddrNote)
				//									+POut.PString(cur.FamAddrNote)+"', "
				+"famfinurgnote = '"+POut.String(Fam.ListPats[0].FamFinUrgNote)
				+POut.String(Pat.FamFinUrgNote)+"' "
				+"WHERE patnum = '"+Pat.Guarantor.ToString()+"'";
 			Db.NonQ(command);
			//delete cur notes
			command= 
				"UPDATE patient SET "
				//+"famaddrnote = '', "
				+"famfinurgnote = '' "
				+"WHERE patnum = '"+Pat.PatNum+"'";
			Db.NonQ(command);
			//concat family financial notes
			PatientNote PatientNoteCur=PatientNotes.Refresh(Pat.PatNum,Pat.Guarantor);
			//patientnote table must have been refreshed for this to work.
			//Makes sure there are entries for patient and for guarantor.
			//Also, PatientNotes.cur.FamFinancial will now have the guar info in it.
			string strGuar=PatientNoteCur.FamFinancial;
			command= 
				"SELECT famfinancial "
				+"FROM patientnote WHERE patnum ='"+POut.Long(Pat.PatNum)+"'";
			//MessageBox.Show(string command);
			DataTable table=Db.GetTable(command);
			string strCur=PIn.String(table.Rows[0][0].ToString());
			command= 
				"UPDATE patientnote SET "
				+"famfinancial = '"+POut.String(strGuar+strCur)+"' "
				+"WHERE patnum = '"+Pat.Guarantor.ToString()+"'";
			Db.NonQ(command);
			//delete cur financial notes
			command= 
				"UPDATE patientnote SET "
				+"famfinancial = ''"
				+"WHERE patnum = '"+Pat.PatNum.ToString()+"'";
			Db.NonQ(command);
		}

		///<summary>Key=patNum, value=formatted name.  Used for reports, FormASAP, FormTrackNext, and FormUnsched.</summary>
		public static Dictionary<long,string> GetAllPatientNames() {
			//No need to check RemotingRole; no call to db.
			DataTable table=GetAllPatientNamesTable();
			Dictionary<long,string> dict=new Dictionary<long,string>();
			long patnum;
			string lname,fname,middlei,preferred;
			for(int i=0;i<table.Rows.Count;i++) {
				patnum=PIn.Long(table.Rows[i][0].ToString());
				lname=PIn.String(table.Rows[i][1].ToString());
				fname=PIn.String(table.Rows[i][2].ToString());
				middlei=PIn.String(table.Rows[i][3].ToString());
				preferred=PIn.String(table.Rows[i][4].ToString());
				if(preferred=="") {
					dict.Add(patnum,lname+", "+fname+" "+middlei);
				}
				else {
					dict.Add(patnum,lname+", '"+preferred+"' "+fname+" "+middlei);
				}
			}
			return dict;
		}

		public static DataTable GetAllPatientNamesTable() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod());
			}
			string command="SELECT patnum,lname,fname,middlei,preferred "
				+"FROM patient";
			DataTable table=Db.GetTable(command);
			return table;
		}

		///<summary></summary>
		public static void UpdateAddressForFam(Patient pat){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),pat);
				return;
			}
			string command= "UPDATE patient SET " 
				+"Address = '"    +POut.String(pat.Address)+"'"
				+",Address2 = '"   +POut.String(pat.Address2)+"'"
				+",City = '"       +POut.String(pat.City)+"'"
				+",State = '"      +POut.String(pat.State)+"'"
				+",Country = '"    +POut.String(pat.Country)+"'"
				+",Zip = '"        +POut.String(pat.Zip)+"'"
				+",HmPhone = '"    +POut.String(pat.HmPhone)+"'"
				+",credittype = '" +POut.String(pat.CreditType)+"'"
				+",priprov = '"    +POut.Long   (pat.PriProv)+"'"
				+",secprov = '"    +POut.Long   (pat.SecProv)+"'"
				+",feesched = '"   +POut.Long   (pat.FeeSched)+"'"
				+",billingtype = '"+POut.Long   (pat.BillingType)+"'"
				+" WHERE guarantor = '"+POut.Long(pat.Guarantor)+"'";
			Db.NonQ(command);
		}

		///<summary>Used in patient terminal, aka sheet import.  Synchs less fields than the normal synch.</summary>
		public static void UpdateAddressForFamTerminal(Patient pat) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),pat);
				return;
			}
			string command= "UPDATE patient SET " 
				+"Address = '"    +POut.String(pat.Address)+"'"
				+",Address2 = '"   +POut.String(pat.Address2)+"'"
				+",City = '"       +POut.String(pat.City)+"'"
				+",State = '"      +POut.String(pat.State)+"'"
				+",Zip = '"        +POut.String(pat.Zip)+"'"
				+",HmPhone = '"    +POut.String(pat.HmPhone)+"'"
				+" WHERE guarantor = '"+POut.Long(pat.Guarantor)+"'";
			Db.NonQ(command);
		}

		///<summary></summary>
		public static void UpdateArriveEarlyForFam(Patient pat){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),pat);
				return;
			}
			string command= "UPDATE patient SET " 
				+"AskToArriveEarly = '"   +POut.Int(pat.AskToArriveEarly)+"'"
				+" WHERE guarantor = '"+POut.Long(pat.Guarantor)+"'";
			DataTable table=Db.GetTable(command);
		}

		///<summary></summary>
		public static void UpdateNotesForFam(Patient pat){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),pat);
				return;
			}
			string command= "UPDATE patient SET " 
				+"addrnote = '"   +POut.String(pat.AddrNote)+"'"
				+" WHERE guarantor = '"+POut.Long(pat.Guarantor)+"'";
			Db.NonQ(command);
		}

		///<summary>Only used from FormRecallListEdit.  Updates two fields for family if they are already the same for the entire family.  If they start out different for different family members, then it only changes the two fields for the single patient.</summary>
		public static void UpdatePhoneAndNoteIfNeeded(string newphone,string newnote,long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),newphone,newnote,patNum);
				return;
			}
			string command="SELECT Guarantor,HmPhone,AddrNote FROM patient WHERE Guarantor="
				+"(SELECT Guarantor FROM patient WHERE PatNum="+POut.Long(patNum)+")";
			DataTable table=Db.GetTable(command);
			bool phoneIsSame=true;
			bool noteIsSame=true;
			long guar=PIn.Long(table.Rows[0]["Guarantor"].ToString());
			for(int i=0;i<table.Rows.Count;i++){
				if(table.Rows[i]["HmPhone"].ToString()!=table.Rows[0]["HmPhone"].ToString()){
					phoneIsSame=false;
				}
				if(table.Rows[i]["AddrNote"].ToString()!=table.Rows[0]["AddrNote"].ToString()) {
					noteIsSame=false;
				}
			}
			command="UPDATE patient SET HmPhone='"+POut.String(newphone)+"' WHERE ";
			if(phoneIsSame){
				command+="Guarantor="+POut.Long(guar);
			}
			else{
				command+="PatNum="+POut.Long(patNum);
			}
			Db.NonQ(command);
			command="UPDATE patient SET AddrNote='"+POut.String(newnote)+"' WHERE ";
			if(noteIsSame) {
				command+="Guarantor="+POut.Long(guar);
			}
			else {
				command+="PatNum="+POut.Long(patNum);
			}
			Db.NonQ(command);
		}

		///<summary>This is only used in the Billing dialog</summary>
		public static List<PatAging> GetAgingList(string age,DateTime lastStatement,List<long> billingNums,bool excludeAddr,
			bool excludeNeg,double excludeLessThan,bool excludeInactive,bool includeChanged,bool excludeInsPending,
			bool excludeIfUnsentProcs,bool ignoreInPerson,List<long> clinicNums)
		{
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PatAging>>(MethodBase.GetCurrentMethod(),age,lastStatement,billingNums,excludeAddr,excludeNeg,excludeLessThan,excludeInactive,includeChanged,excludeInsPending,excludeIfUnsentProcs,ignoreInPerson,clinicNums);
			}
			string command="";
			Random rnd=new Random();
			string rndStr=rnd.Next(1000000).ToString();
			if(includeChanged){
				command+=@"DROP TABLE IF EXISTS templastproc"+rndStr+@";
					CREATE TABLE templastproc"+rndStr+@"(
					Guarantor bigint unsigned NOT NULL,
					LastProc date NOT NULL,
					PRIMARY KEY (Guarantor));
					INSERT INTO templastproc"+rndStr+@"
					SELECT patient.Guarantor,MAX(ProcDate)
					FROM procedurelog,patient
					WHERE patient.PatNum=procedurelog.PatNum
					AND procedurelog.ProcStatus=2
					AND procedurelog.ProcFee>0
					GROUP BY patient.Guarantor;
					
					DROP TABLE IF EXISTS templastpay"+rndStr+@";
					CREATE TABLE templastpay"+rndStr+@"(
					Guarantor bigint unsigned NOT NULL,
					LastPay date NOT NULL,
					PRIMARY KEY (Guarantor));
					INSERT INTO templastpay"+rndStr+@"
					SELECT patient.Guarantor,MAX(DateCP)
					FROM claimproc,patient
					WHERE claimproc.PatNum=patient.PatNum
					AND claimproc.InsPayAmt>0
					GROUP BY patient.Guarantor;";					
			}
			if(excludeInsPending) {
				command+=@"DROP TABLE IF EXISTS tempclaimspending"+rndStr+@";
					CREATE TABLE tempclaimspending"+rndStr+@"(
					Guarantor bigint unsigned NOT NULL,
					PendingClaimCount int NOT NULL,
					PRIMARY KEY (Guarantor));
					INSERT INTO tempclaimspending"+rndStr+@"
					SELECT patient.Guarantor,COUNT(*)
					FROM claim,patient
					WHERE claim.PatNum=patient.PatNum
					AND (ClaimStatus='U' OR ClaimStatus='H' OR ClaimStatus='W' OR ClaimStatus='S')
					AND (ClaimType='P' OR ClaimType='S' OR ClaimType='Other')
					GROUP BY patient.Guarantor;";
			}
			if(excludeIfUnsentProcs) {
				command+=@"DROP TABLE IF EXISTS tempunsentprocs"+rndStr+@";
					CREATE TABLE tempunsentprocs"+rndStr+@"(
					Guarantor bigint unsigned NOT NULL,
					UnsentProcCount int NOT NULL,
					PRIMARY KEY (Guarantor));
					INSERT INTO tempunsentprocs"+rndStr+@"
					SELECT patient.Guarantor,COUNT(*)
					FROM patient,procedurecode,procedurelog,claimproc 
					WHERE claimproc.procnum=procedurelog.procnum
					AND patient.PatNum=procedurelog.PatNum
					AND procedurelog.CodeNum=procedurecode.CodeNum
					AND claimproc.NoBillIns=0
					AND procedurelog.ProcFee>0
					AND claimproc.Status=6
					AND procedurelog.procstatus=2
					AND procedurelog.ProcDate > "+DbHelper.DateAddMonth(DbHelper.Curdate(),"-6")+" "
					+"GROUP BY patient.Guarantor;";
			}
			command+="SELECT patient.PatNum,Bal_0_30,Bal_31_60,Bal_61_90,BalOver90,BalTotal,BillingType,"
				+"InsEst,LName,FName,MiddleI,PayPlanDue,Preferred, "
				+"IFNULL(MAX(statement.DateSent),'0001-01-01') AS LastStatement ";
			if(includeChanged){
				command+=",IFNULL(templastproc"+rndStr+@".LastProc,'0001-01-01') AS LastChange,"
					+"IFNULL(templastpay"+rndStr+@".LastPay,'0001-01-01') AS LastPayment ";
			}
			if(excludeInsPending){
				command+=",IFNULL(tempclaimspending"+rndStr+@".PendingClaimCount,'0') AS ClaimCount ";
			}
			if(excludeIfUnsentProcs) {
				command+=",IFNULL(tempunsentprocs"+rndStr+@".UnsentProcCount,'0') AS unsentProcCount_ ";
			}
			command+=
				"FROM patient "//actually only gets guarantors since others are 0.
				+"LEFT JOIN statement ON patient.PatNum=statement.PatNum ";
			if(ignoreInPerson) {
				command+="AND statement.Mode_ != 1 ";
			}
			if(includeChanged){
				command+="LEFT JOIN templastproc"+rndStr+@" ON patient.PatNum=templastproc"+rndStr+@".Guarantor "
					+"LEFT JOIN templastpay"+rndStr+@" ON patient.PatNum=templastpay"+rndStr+@".Guarantor ";
			}
			if(excludeInsPending){
				command+="LEFT JOIN tempclaimspending"+rndStr+@" ON patient.PatNum=tempclaimspending"+rndStr+@".Guarantor ";
			}
			if(excludeIfUnsentProcs) {
				command+="LEFT JOIN tempunsentprocs"+rndStr+@" ON patient.PatNum=tempunsentprocs"+rndStr+@".Guarantor ";
			}
			command+="WHERE ";
			if(excludeInactive){
				command+="EXISTS (SELECT * FROM patient p2 WHERE p2.Guarantor=patient.Guarantor AND p2.PatStatus != "+POut.Int((int)PatientStatus.Inactive)+") AND ";
			}
			if(PrefC.GetBool(PrefName.BalancesDontSubtractIns)) {
				command+="(BalTotal";
			}
			else {
				command+="(BalTotal - InsEst";
			}
			command+=" > '"+POut.Double(excludeLessThan+.005)+"'"//add half a penny for rounding error
				+" OR PayPlanDue > 0";
			if(!excludeNeg){
				if(PrefC.GetBool(PrefName.BalancesDontSubtractIns)) {
					command+=" OR BalTotal < '-.005')";
				}
				else {
					command+=" OR BalTotal - InsEst < '-.005')";
				}
			}
			else{
				command+=")";
			}
			switch(age){
				//where is age 0. Is it missing because no restriction
				case "30":
					command+=" AND (Bal_31_60 > '0' OR Bal_61_90 > '0' OR BalOver90 > '0' OR PayPlanDue > 0)";
					break;
				case "60":
					command+=" AND (Bal_61_90 > '0' OR BalOver90 > '0' OR PayPlanDue > 0)";
					break;
				case "90":
					command+=" AND (BalOver90 > '0' OR PayPlanDue > 0)";
					break;
			}
			//if billingNums.Count==0, then we'll include all billing types
			for(int i=0;i<billingNums.Count;i++){
				if(i==0){
					command+=" AND (billingtype = '";
				}
				else{
					command+=" OR billingtype = '";
				}
				command+=POut.Long(billingNums[i])+"'";
					//DefC.Short[(int)DefCat.BillingTypes][billingIndices[i]].DefNum.ToString()+"'";
				if(i==billingNums.Count-1){
					command+=")";
				}
			}
			if(excludeAddr){
				command+=" AND (zip !='')";
			}
			if(clinicNums.Count>0) {
				command+="AND patient.ClinicNum IN ("+string.Join(",",clinicNums)+") ";
			}
			command+=" GROUP BY patient.PatNum,Bal_0_30,Bal_31_60,Bal_61_90,BalOver90,BalTotal,BillingType,"
				+"InsEst,LName,FName,MiddleI,PayPlanDue,Preferred "
				+"HAVING (LastStatement < "+POut.Date(lastStatement.AddDays(1))+" ";//<midnight of lastStatement date
				//+"OR PayPlanDue>0 ";we don't have a great way to trigger due to a payplancharge yet
			if(includeChanged){
				command+=
					 "OR LastChange > LastStatement "//eg '2005-10-25' > '2005-10-24 15:00:00'
					+"OR LastPayment > LastStatement) ";
			}
			else{
				command+=") ";
			}
			if(excludeInsPending){
				command+="AND ClaimCount=0 ";
			}
			if(excludeIfUnsentProcs) {
				command+="AND unsentProcCount_=0 ";
			}
			command+="ORDER BY LName,FName";
			//Debug.WriteLine(command);
			DataTable table=Db.GetTable(command);
			List<PatAging> agingList=new List<PatAging>();
			PatAging patage;
			Patient pat;
			for(int i=0;i<table.Rows.Count;i++){
				patage=new PatAging();
				patage.PatNum   = PIn.Long   (table.Rows[i]["PatNum"].ToString());
				patage.Bal_0_30 = PIn.Double(table.Rows[i]["Bal_0_30"].ToString());
				patage.Bal_31_60= PIn.Double(table.Rows[i]["Bal_31_60"].ToString());
				patage.Bal_61_90= PIn.Double(table.Rows[i]["Bal_61_90"].ToString());
				patage.BalOver90= PIn.Double(table.Rows[i]["BalOver90"].ToString());
				patage.BalTotal = PIn.Double(table.Rows[i]["BalTotal"].ToString());
				patage.InsEst   = PIn.Double(table.Rows[i]["InsEst"].ToString());
				pat=new Patient();
				pat.LName=PIn.String(table.Rows[i]["LName"].ToString());
				pat.FName=PIn.String(table.Rows[i]["FName"].ToString());
				pat.MiddleI=PIn.String(table.Rows[i]["MiddleI"].ToString());
				pat.Preferred=PIn.String(table.Rows[i]["Preferred"].ToString());
				patage.PatName=pat.GetNameLF();
				patage.AmountDue=patage.BalTotal-patage.InsEst;
				patage.DateLastStatement=PIn.Date(table.Rows[i]["LastStatement"].ToString());
				patage.BillingType=PIn.Long(table.Rows[i]["BillingType"].ToString());
				patage.PayPlanDue =PIn.Double(table.Rows[i]["PayPlanDue"].ToString());
				//if(excludeInsPending && patage.InsEst>0){
					//don't add
				//}
				//else{
				agingList.Add(patage);
				//}
			}
			//PatAging[] retVal=new PatAging[agingList.Count];
			//for(int i=0;i<retVal.Length;i++){
			//	retVal[i]=agingList[i];
			//}
			if(includeChanged){
				command="DROP TABLE IF EXISTS templastproc"+rndStr+@"";
				Db.NonQ(command);
				command="DROP TABLE IF EXISTS templastpay"+rndStr+@"";
				Db.NonQ(command);
			}
			if(excludeInsPending){
				command="DROP TABLE IF EXISTS tempclaimspending"+rndStr+@"";
				Db.NonQ(command);
			}
			if(excludeIfUnsentProcs){
				command="DROP TABLE IF EXISTS tempunsentprocs"+rndStr+@"";
				Db.NonQ(command);
			}
			return agingList;
		}

		///<summary>Used only to run finance charges, so it ignores negative balances.</summary>
		public static PatAging[] GetAgingListArray(){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<PatAging[]>(MethodBase.GetCurrentMethod());
			}
			string command =
				"SELECT patnum,Bal_0_30,Bal_31_60,Bal_61_90,BalOver90,BalTotal,InsEst,LName,FName,MiddleI,PriProv,BillingType "
				+"FROM patient "//actually only gets guarantors since others are 0.
				+" WHERE Bal_0_30 + Bal_31_60 + Bal_61_90 + BalOver90 - InsEst > '0.005'"//more that 1/2 cent
				+" ORDER BY LName,FName";
			DataTable table=Db.GetTable(command);
			PatAging[] AgingList=new PatAging[table.Rows.Count];
			for(int i=0;i<table.Rows.Count;i++){
				AgingList[i]=new PatAging();
				AgingList[i].PatNum   = PIn.Long   (table.Rows[i][0].ToString());
				AgingList[i].Bal_0_30 = PIn.Double(table.Rows[i][1].ToString());
				AgingList[i].Bal_31_60= PIn.Double(table.Rows[i][2].ToString());
				AgingList[i].Bal_61_90= PIn.Double(table.Rows[i][3].ToString());
				AgingList[i].BalOver90= PIn.Double(table.Rows[i][4].ToString());
				AgingList[i].BalTotal = PIn.Double(table.Rows[i][5].ToString());
				AgingList[i].InsEst   = PIn.Double(table.Rows[i][6].ToString());
				AgingList[i].PatName=PIn.String(table.Rows[i][7].ToString())
					+", "+PIn.String(table.Rows[i][8].ToString())
					+" "+PIn.String(table.Rows[i][9].ToString());;
				//AgingList[i].Balance=AgingList[i].Bal_0_30+AgingList[i].Bal_31_60
				//	+AgingList[i].Bal_61_90+AgingList[i].BalOver90;
				AgingList[i].AmountDue=AgingList[i].BalTotal-AgingList[i].InsEst;
				AgingList[i].PriProv=PIn.Long(table.Rows[i][10].ToString());
				AgingList[i].BillingType=PIn.Long(table.Rows[i][11].ToString());
			}
			return AgingList;
		}

		///<summary>Gets the next available integer chart number.  Will later add a where clause based on preferred format.</summary>
		public static string GetNextChartNum(){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod());
			}
			string command="SELECT ChartNumber from patient WHERE "
				+DbHelper.Regexp("ChartNumber","^[0-9]+$")+" "//matches any positive number of digits
				+"ORDER BY (chartnumber+0) DESC";//1/13/05 by Keyush Shaw-added 0.
			command=DbHelper.LimitOrderBy(command,1);
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0){//no existing chart numbers
				return "1";
			}
			string lastChartNum=PIn.String(table.Rows[0][0].ToString());
			//or could add more match conditions
			try {
				return (Convert.ToInt64(lastChartNum)+1).ToString();
			}
			catch {
				throw new ApplicationException(lastChartNum+" is an existing ChartNumber.  It's too big to convert to a long int, so it's not possible to add one to automatically increment.");
			}
		}

		///<summary>Returns the name(only one) of the patient using this chartnumber.</summary>
		public static string ChartNumUsedBy(string chartNum,long excludePatNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),chartNum,excludePatNum);
			}
			string command="SELECT LName,FName from patient WHERE "
				+"ChartNumber = '"+POut.String(chartNum)
				+"' AND PatNum != '"+excludePatNum.ToString()+"'";
			DataTable table=Db.GetTable(command);
			string retVal="";
			if(table.Rows.Count!=0){//found duplicate chart number
				retVal=PIn.String(table.Rows[0][1].ToString())+" "+PIn.String(table.Rows[0][0].ToString());
			}
			return retVal;
		}

		///<summary>Used in the patient select window to determine if a trial version user is over their limit.</summary>
		public static int GetNumberPatients(){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod());
			}
			string command="SELECT Count(*) FROM patient";
			DataTable table=Db.GetTable(command);
			return PIn.Int(table.Rows[0][0].ToString());
		}

		///<summary>Makes a call to the db to figure out if the current HasIns status is correct.  If not, then it changes it.</summary>
		public static void SetHasIns(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),patNum);
				return;
			}
			string command="SELECT patient.HasIns,COUNT(patplan.PatNum) FROM patient "
				+"LEFT JOIN patplan ON patplan.PatNum=patient.PatNum"
				+" WHERE patient.PatNum="+POut.Long(patNum)
				+" GROUP BY patplan.PatNum,patient.HasIns";
			DataTable table=Db.GetTable(command);
			string newVal="";
			if(table.Rows[0][1].ToString()!="0"){
				newVal="I";
			}
			if(newVal!=table.Rows[0][0].ToString()){
				command="UPDATE patient SET HasIns='"+POut.String(newVal)
					+"' WHERE PatNum="+POut.Long(patNum);
				Db.NonQ(command);
			}
		}

		///<summary></summary>
		public static DataTable GetBirthdayList(DateTime dateFrom,DateTime dateTo){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateFrom,dateTo);
			}
			string command="SELECT LName,FName,Preferred,Address,Address2,City,State,Zip,Birthdate "
				+"FROM patient " 
				+"WHERE SUBSTRING(Birthdate,6,5) >= '"+dateFrom.ToString("MM-dd")+"' "
				+"AND SUBSTRING(Birthdate,6,5) <= '"+dateTo.ToString("MM-dd")+"' "
				+"AND Birthdate > '1880-01-01' "
				+"AND PatStatus=0	"
				//+"ORDER BY "+DbHelper.DateFormatColumn("Birthdate","%m/%d/%Y");
				+"ORDER BY MONTH(Birthdate),DAY(Birthdate)";
			DataTable table=Db.GetTable(command);
			table.Columns.Add("Age");
			for(int i=0;i<table.Rows.Count;i++){
				table.Rows[i]["Age"]=DateToAge(PIn.Date(table.Rows[i]["Birthdate"].ToString()),dateTo.AddDays(1)).ToString();
			}
			return table;
		}

		///<summary>Gets the provider for this patient.  If provNum==0, then it gets the practice default prov.</summary>
		public static long GetProvNum(Patient pat) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),pat);
			}
			if(pat.PriProv!=0)
				return pat.PriProv;
			if(PrefC.GetLong(PrefName.PracticeDefaultProv)==0) {
				MessageBox.Show(Lans.g("Patients","Please set a default provider in the practice setup window."));
				List<Provider> listProvs=ProviderC.GetListShort();
				return listProvs[0].ProvNum;
			}
			return PrefC.GetLong(PrefName.PracticeDefaultProv);
		}

		///<summary>Gets the list of all valid patient primary keys. Allows user to specify whether to include non-deleted patients. Used when checking for missing ADA procedure codes after a user has begun entering them manually. This function is necessary because not all patient numbers are necessarily consecutive (say if the database was created due to a conversion from another program and the customer wanted to keep their old patient ids after the conversion).</summary>
		public static long[] GetAllPatNums(bool HasDeleted) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<long[]>(MethodBase.GetCurrentMethod());
			}
			string command="";
			if(HasDeleted) {
				command="SELECT PatNum FROM patient";
			}
			else {
				command="SELECT PatNum FROM patient WHERE patient.PatStatus!=4";
			}
			DataTable dt=Db.GetTable(command);
			long[] patnums=new long[dt.Rows.Count];
			for(int i=0;i<patnums.Length;i++){
				patnums[i]=PIn.Long(dt.Rows[i]["PatNum"].ToString());
			}
			return patnums;
		}

		///<summary>Converts a date to an age. If age is over 115, then returns 0.</summary>
		public static int DateToAge(DateTime date){
			//No need to check RemotingRole; no call to db.
			if(date.Year<1880)
				return 0;
			if(date.Month < DateTime.Now.Month){//birthday in previous month
				return DateTime.Now.Year-date.Year;
			}
			if(date.Month == DateTime.Now.Month && date.Day <= DateTime.Now.Day){//birthday in this month
				return DateTime.Now.Year-date.Year;
			}
			return DateTime.Now.Year-date.Year-1;
		}

		///<summary>Converts a date to an age. If age is over 115, then returns 0.</summary>
		public static int DateToAge(DateTime birthdate,DateTime asofDate) {
			//No need to check RemotingRole; no call to db.
			if(birthdate.Year<1880)
				return 0;
			if(birthdate.Month < asofDate.Month) {//birthday in previous month
				return asofDate.Year-birthdate.Year;
			}
			if(birthdate.Month == asofDate.Month && birthdate.Day <= asofDate.Day) {//birthday in this month
				return asofDate.Year-birthdate.Year;
			}
			return asofDate.Year-birthdate.Year-1;
		}

		///<summary>If zero, returns empty string.  Otherwise returns simple year.  Also see PatientLogic.DateToAgeString().</summary>
		public static string AgeToString(int age){
			//No need to check RemotingRole; no call to db.
			if(age==0) {
				return "";
			}
			else {
				return age.ToString();
			}
		}

		public static void ReformatAllPhoneNumbers() {
			string oldTel;
			string newTel;
			string idNum;
			string command="select * from patient";
			DataTable table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				idNum=PIn.String(table.Rows[i][0].ToString());
				//home
				oldTel=PIn.String(table.Rows[i][15].ToString());
				newTel=TelephoneNumbers.ReFormat(oldTel);
				if(oldTel!=newTel) {
					command="UPDATE patient SET hmphone = '"
						+POut.String(newTel)+"' WHERE patNum = '"+idNum+"'";
					Db.NonQ(command);
				}
				//wk:
				oldTel=PIn.String(table.Rows[i][16].ToString());
				newTel=TelephoneNumbers.ReFormat(oldTel);
				if(oldTel!=newTel) {
					command="UPDATE patient SET wkphone = '"
						+POut.String(newTel)+"' WHERE patNum = '"+idNum+"'";
					Db.NonQ(command);
				}
				//wireless
				oldTel=PIn.String(table.Rows[i][17].ToString());
				newTel=TelephoneNumbers.ReFormat(oldTel);
				if(oldTel!=newTel) {
					command="UPDATE patient SET wirelessphone = '"
						+POut.String(newTel)+"' WHERE patNum = '"+idNum+"'";
					Db.NonQ(command);
				}
			}
			command="select * from carrier";
			Db.NonQ(command);
			for(int i=0;i<table.Rows.Count;i++) {
				idNum=PIn.String(table.Rows[i][0].ToString());
				//ph
				oldTel=PIn.String(table.Rows[i][7].ToString());
				newTel=TelephoneNumbers.ReFormat(oldTel);
				if(oldTel!=newTel) {
					command="UPDATE carrier SET Phone = '"
						+POut.String(newTel)+"' WHERE CarrierNum = '"+idNum+"'";
					Db.NonQ(command);
				}
			}
		}

		public static DataTable GetGuarantorInfo(long PatientID) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),PatientID);
			}
			string command=@"SELECT FName,MiddleI,LName,Guarantor,Address,
								Address2,City,State,Zip,Email,EstBalance,
								BalTotal,Bal_0_30,Bal_31_60,Bal_61_90,BalOver90
						FROM Patient Where Patnum="+PatientID+
				" AND patnum=guarantor";
			return Db.GetTable(command);
		}

		///<summary>Will return 0 if can't find exact matching pat.</summary>
		public static long GetPatNumByNameAndBirthday(string lName,string fName,DateTime birthdate) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),lName,fName,birthdate);
			}
			string command="SELECT PatNum FROM patient WHERE "
				+"LName='"+POut.String(lName)+"' "
				+"AND FName='"+POut.String(fName)+"' "
				+"AND Birthdate="+POut.Date(birthdate)+" "
				+"AND PatStatus!="+POut.Int((int)PatientStatus.Archived)+" "//Not Archived
				+"AND PatStatus!="+POut.Int((int)PatientStatus.Deleted);//Not Deleted
			return PIn.Long(Db.GetScalar(command));
		}

		///<summary>Will return 0 if can't find an exact matching pat.  Because it does not include birthdate, it's not specific enough for most situations.</summary>
		public static long GetPatNumByName(string lName,string fName) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),lName,fName);
			}
			string command="SELECT PatNum FROM patient WHERE "
				+"LName='"+POut.String(lName)+"' "
				+"AND FName='"+POut.String(fName)+"' "
				+"AND PatStatus!=4 "//not deleted
				+"LIMIT 1";
			return PIn.Long(Db.GetScalar(command));
		}

		/// <summary>When importing webforms, if it can't find an exact match, this method attempts a similar match.</summary>
		public static List<Patient> GetSimilarList(string lName,string fName,DateTime birthdate) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod(),lName,fName,birthdate);
			}
			int subStrIndexlName=2;
			int subStrIndexfName=2;
			if(lName.Length<2) {
				subStrIndexlName=lName.Length;
			}
			if(fName.Length<2) {
				subStrIndexfName=fName.Length;
			}
			string command="SELECT * FROM patient WHERE "
				+"LName LIKE '"+POut.String(lName.Substring(0,subStrIndexlName))+"%' "
				+"AND FName LIKE '"+POut.String(fName.Substring(0,subStrIndexfName))+"%' "
				+"AND (Birthdate="+POut.Date(birthdate)+" "//either a matching bd
				+"OR Birthdate < "+POut.Date(new DateTime(1880,1,1))+") "//or no bd
				+"AND PatStatus!="+POut.Int((int)PatientStatus.Archived)+" "//Not Archived
				+"AND PatStatus!="+POut.Int((int)PatientStatus.Deleted);//Not Deleted
			return Crud.PatientCrud.SelectMany(command);
		}

		///<summary>Returns a list of patients that match last and first name.  Case insensitive.</summary>
		public static List<Patient> GetListByName(string lName,string fName,long PatNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod(),lName,fName,PatNum);
			}
			string command="SELECT * FROM patient WHERE "
				+"LOWER(LName)=LOWER('"+POut.String(lName)+"') "
				+"AND LOWER(FName)=LOWER('"+POut.String(fName)+"') "
				+"AND PatNum!="+POut.Long(PatNum)
				+" AND PatStatus!=4";//not deleted
			return Crud.PatientCrud.SelectMany(command);
		}

		///<summary>Finds any patients with the same first name, last name, and birthdate.  The birthdate must be a valid date, not 0001-01-01.
		///<para>If patCur has all-caps first and last names and there is exactly one matching patient who does not have all-caps first and last names, then patClone is set to patCur, patNonClone is set to the matching patient, and listAmbiguousMatches will be an empty list.</para>
		///<para>If the matching patient has an all-caps first and last name and patCur does not, then patClone will be set to the matching patient, patNonClone will be set to patCur, and listAmbiguousMatches will be an empty list.</para>
		///<para>If there are no matching patients, patClone and patNonClone will be null and listAmbiguousMatches will be an empty list.</para>
		///<para>If more than one patient has the same first and last name and birthdate, patClone and patNonClone will be null and listAmbiguousMatches will contain all the matching patients.</para>
		///<para>If there is one match, but there is not an all-caps to not all-caps relationship (meaning both are all-caps or both are mixed case or both are lower), patClone and patNonClone will be null and listAmbiguousMatches will contain the matching patient.</para></summary>
		public static void GetCloneAndNonClone(Patient patCur,out Patient patClone,out Patient patNonClone,out List<Patient> listAmbiguousMatches) {
			//No need to check RemotingRole; no call to db.
			//if niether patClone or patNonClone is set after this method, the patient does not have a clone and is also not a clone
			patClone=null;
			patNonClone=null;
			listAmbiguousMatches=new List<Patient>();
			if(patCur==null) {
				return;
			}
			if(patCur.Birthdate.Year<1880) {
				return;//in order to clone a patient or synch two patients, the birthdate for the patients must be a valid date
			}
			//listAllMatches should only contain 0 or 1 patient
			//if more than 1 other patient has the same first and last name and birthdate, then there is ambiguity that has to be fixed manually
			List<Patient> listAllMatches=GetListByNameAndBirthdate(patCur.PatNum,patCur.LName,patCur.FName,patCur.Birthdate);
			if(listAllMatches.Count==0) {
				return;//no matches, not a clone and does not have a clone
			}
			if(listAllMatches.Count>1) {
				for(int i=0;i<listAllMatches.Count;i++) {
					listAmbiguousMatches.Add(listAllMatches[i]);
				}
				return;//more than one match, cannot determine which is supposed to be linked, return the list of patients to notify the user that there is ambiguity and to fix manually
			}
			//there must be one and only one match, so determine if patCur is the clone or the non-clone
			//if patCur has all-caps first and last name, and the patient found does not, then patCur is the clone and the patient found is the non-clone
			if(patCur.LName.ToUpper()==patCur.LName
				&& patCur.FName.ToUpper()==patCur.FName
				&& (listAllMatches[0].LName.ToUpper()!=listAllMatches[0].LName || listAllMatches[0].FName.ToUpper()!=listAllMatches[0].FName))//using an or here so a patient name A Smith can be cloned to A SMITH and found based on first names both being upper case, but last names not or vice versa
			{
				patClone=patCur.Copy();
				patNonClone=listAllMatches[0].Copy();
			}
			//if patCur does not have all-caps first and last name, but the patient found does, then the patient found is the clone and patCur is the non-clone
			else if((patCur.LName.ToUpper()!=patCur.LName || patCur.FName.ToUpper()!=patCur.FName)//using an or here so original can have all uppercase first or last name but not both.  So A Smith can be cloned to A SMITH and both uppercase A first names will be ok.
				&& listAllMatches[0].LName.ToUpper()==listAllMatches[0].LName
				&& listAllMatches[0].FName.ToUpper()==listAllMatches[0].FName)
			{
				patNonClone=patCur.Copy();
				patClone=listAllMatches[0].Copy();
			}
			else {
				//either both patCur and the patient found have all-caps first and last names or both have mixed case or all lower case first and last names
				//either way, we do not know if patCur is a clone or has a clone, there is ambiguity
				//populate the ambiguous list with the patient found to notify user to fix manually if it is supposed to be linked
				listAmbiguousMatches.Add(listAllMatches[0]);
			}
		}

		///<summary>Returns a list of patients that have the same last name, first name, and birthdate, ignoring case sensitivity, but different patNum.  Used to find duplicate patients that may be clones of the patient identified by the patNum parameter, or are the non-clone version of the patient.  Currently only used with GetCloneAndNonClone to find the non-clone and clone patients for the pateint sent in if they exist.</summary>
		public static List<Patient> GetListByNameAndBirthdate(long patNum,string lName,string fName,DateTime birthdate) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod(),patNum,lName,fName,birthdate);
			}
			string command="SELECT * FROM patient WHERE LName LIKE '"+POut.String(lName)+"' AND FName LIKE '"+POut.String(fName)+"' "
				+"AND Birthdate="+POut.Date(birthdate,true)+" AND PatNum!="+POut.Long(patNum) +" AND PatStatus!="+POut.Int((int)PatientStatus.Deleted);
			return Crud.PatientCrud.SelectMany(command);
		}

		public static void UpdateFamilyBillingType(long billingType,long Guarantor) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),billingType,Guarantor);
				return;
			}
			string command="UPDATE patient SET BillingType="+POut.Long(billingType)+
				" WHERE Guarantor="+POut.Long(Guarantor);
			Db.NonQ(command);
		}

		public static DataTable GetPartialPatientData(long PatNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),PatNum);
			}
			string command="SELECT FName,LName,"+DbHelper.DateFormatColumn("birthdate","%m/%d/%Y")+" BirthDate,Gender "
				+"FROM patient WHERE patient.PatNum="+PatNum;
			return Db.GetTable(command);
		}

		public static DataTable GetPartialPatientData2(long PatNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),PatNum);
			}
			string command=@"SELECT FName,LName,"+DbHelper.DateFormatColumn("birthdate","%m/%d/%Y")+" BirthDate,Gender "
				+"FROM patient WHERE PatNum In (SELECT Guarantor FROM PATIENT WHERE patnum = "+PatNum+")";
			return Db.GetTable(command);
		}

		public static string GetEligibilityDisplayName(long patId) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),patId);
			}
			string command = @"SELECT FName,LName,"+DbHelper.DateFormatColumn("birthdate","%m/%d/%Y")+" BirthDate,Gender "
				+"FROM patient WHERE patient.PatNum=" + POut.Long(patId);
			DataTable table = Db.GetTable(command);
			if(table.Rows.Count == 0) {
				return "Patient(???) is Eligible";
			}
			return PIn.String(table.Rows[0][1].ToString()) + ", "+ PIn.String(table.Rows[0][0].ToString()) + " is Eligible";
		}

		///<summary>Gets the DataTable to display for treatment finder report</summary>
		public static DataTable GetTreatmentFinderList(bool noIns,bool patsWithAppts,int monthStart,DateTime dateSince,double aboveAmount,ArrayList providerFilter,
			ArrayList billingFilter,string code1,string code2) 
		{
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),noIns,patsWithAppts,monthStart,dateSince,aboveAmount,providerFilter,billingFilter,code1,code2);
			}
			DataTable table=new DataTable();
			DataRow row;
			//columns that start with lowercase are altered for display rather than being raw data.
			table.Columns.Add("PatNum");
			table.Columns.Add("LName");
			table.Columns.Add("FName");
			table.Columns.Add("contactMethod");
			table.Columns.Add("address");
			table.Columns.Add("City");
			table.Columns.Add("State");
			table.Columns.Add("Zip");
			table.Columns.Add("annualMax");
			table.Columns.Add("amountUsed");
			table.Columns.Add("amountPending");
			table.Columns.Add("amountRemaining");
			table.Columns.Add("treatmentPlan");
			table.Columns.Add("carrierName");
			List<DataRow> rows=new List<DataRow>();
			Def[][] arrayDefs=DefC.GetArrayShort();
			List<Provider> listProvs=ProviderC.GetListShort();
			Random rnd=new Random();
			string rndStr=rnd.Next(1000000).ToString();
			string command=@"
				DROP TABLE IF EXISTS tempused"+rndStr+@";
				DROP TABLE IF EXISTS temppending"+rndStr+@";
				DROP TABLE IF EXISTS tempplanned"+rndStr+@";
				DROP TABLE IF EXISTS tempannualmax"+rndStr+@";

				CREATE TABLE tempused"+rndStr+@"(
				PatPlanNum bigint unsigned NOT NULL,
				AmtUsed double NOT NULL,
				PRIMARY KEY (PatPlanNum));

				CREATE TABLE temppending"+rndStr+@"(
				PatPlanNum bigint unsigned NOT NULL,
				PendingAmt double NOT NULL,
				PRIMARY KEY (PatPlanNum));

				CREATE TABLE tempplanned"+rndStr+@"(
				PatNum bigint unsigned NOT NULL,
				AmtPlanned double NOT NULL,
				PRIMARY KEY (PatNum));

				CREATE TABLE tempannualmax"+rndStr+@"(
				PlanNum bigint unsigned NOT NULL,
				AnnualMax double,
				PRIMARY KEY (PlanNum));";
			Db.NonQ(command);
			DateTime renewDate=BenefitLogic.ComputeRenewDate(DateTime.Now,monthStart);
			command=@"INSERT INTO tempused"+rndStr+@"
SELECT patplan.PatPlanNum,
SUM(IFNULL(claimproc.InsPayAmt,0))
FROM claimproc
LEFT JOIN patplan ON patplan.PatNum = claimproc.PatNum
AND patplan.InsSubNum = claimproc.InsSubNum
WHERE claimproc.Status IN (1, 3, 4) /*Received, Adjustment, Supplemental*/
AND claimproc.ProcDate >= "+POut.Date(renewDate)+" "  //  MAKEDATE("+renewDate.Year+", "+renewDate.Month+") "
+"AND claimproc.ProcDate < "+POut.Date(renewDate.AddYears(1))+" "   //MAKEDATE("+renewDate.Year+"+1, "+renewDate.Month+") "
+"GROUP BY patplan.PatPlanNum";
			Db.NonQ(command);
			command=@"INSERT INTO temppending"+rndStr+@"
SELECT patplan.PatPlanNum,
SUM(IFNULL(claimproc.InsPayEst,0))
FROM claimproc
LEFT JOIN patplan ON patplan.PatNum = claimproc.PatNum
AND patplan.InsSubNum = claimproc.InsSubNum
WHERE claimproc.Status = 0 /*NotReceived*/
AND claimproc.InsPayAmt = 0
AND claimproc.ProcDate >= "+POut.Date(renewDate)+@" 
AND claimproc.ProcDate < "+POut.Date(renewDate.AddYears(1))+@" 
GROUP BY patplan.PatPlanNum";
			Db.NonQ(command);
			command=@"INSERT INTO tempplanned"+rndStr+@"
SELECT PatNum, SUM(ProcFee)
FROM procedurelog
LEFT JOIN procedurecode ON procedurecode.CodeNum = procedurelog.CodeNum
WHERE ProcStatus = 1 /*treatment planned*/";
			if(code1!="") {
				command+=" AND procedurecode.ProcCode >= '"+POut.String(code1)+"' "
					+" AND procedurecode.ProcCode <= '"+POut.String(code2)+"' ";
				//command+=@" AND (((SELECT STRCMP('"+POut.String(code1)+@"', ProcCode))=0) OR ((SELECT STRCMP('"+POut.String(code1)+@"', ProcCode))=-1))
				//AND (((SELECT STRCMP('"+POut.String(code2)+@"', ProcCode))=0) OR ((SELECT STRCMP('"+POut.String(code2)+@"', ProcCode))=1))";
			}
			command+="AND procedurelog.ProcDate>"+POut.DateT(dateSince)+" "
				+"GROUP BY PatNum";
			Db.NonQ(command);
			command=@"INSERT INTO tempannualmax"+rndStr+@"
SELECT insplan.PlanNum, 
(SELECT MAX(MonetaryAmt)/*for oracle in case there's more than one*/
FROM benefit
LEFT JOIN covcat ON benefit.CovCatNum=covcat.CovCatNum
WHERE benefit.PlanNum=insplan.PlanNum 
AND (covcat.EbenefitCat=1 OR ISNULL(covcat.EbenefitCat))
AND benefit.BenefitType = 5 /* limitation */
AND benefit.MonetaryAmt > 0
AND benefit.QuantityQualifier=0
GROUP BY insplan.PlanNum)
FROM insplan"; 
			//WHERE insplan.MonthRenew='"+POut.Int(monthStart)+"'";
			//if(monthStart!=13) {//belongs further down, in the WHERE of the final query
			//	command+="AND insplan.MonthRenew='"+POut.Int(monthStart)+"' ";
			//}
			Db.NonQ(command);
			command=@"SELECT patient.PatNum, patient.LName, patient.FName,
				patient.Email, patient.HmPhone, patient.PreferRecallMethod,
				patient.WirelessPhone, patient.WkPhone, patient.Address,
				patient.Address2, patient.City, patient.State, patient.Zip,
				patient.PriProv, patient.BillingType,
				tempannualmax"+rndStr+@".AnnualMax ""$AnnualMax"",
				tempused"+rndStr+@".AmtUsed ""$AmountUsed"",
				temppending"+rndStr+@".PendingAmt ""$AmountPending"",
				tempannualmax"+rndStr+@".AnnualMax-IFNULL(tempused"+rndStr+@".AmtUsed,0)-IFNULL(temppending"+rndStr+@".PendingAmt,0) ""$AmtRemaining"",
				tempplanned"+rndStr+@".AmtPlanned ""$TreatmentPlan"", carrier.CarrierName
				FROM patient
				LEFT JOIN tempplanned"+rndStr+@" ON tempplanned"+rndStr+@".PatNum=patient.PatNum
				LEFT JOIN patplan ON patient.PatNum=patplan.PatNum
				LEFT JOIN inssub ON patplan.InsSubNum=inssub.InsSubNum
				LEFT JOIN insplan ON insplan.PlanNum=inssub.PlanNum
				LEFT JOIN carrier ON insplan.CarrierNum=carrier.CarrierNum
				LEFT JOIN tempused"+rndStr+@" ON tempused"+rndStr+@".PatPlanNum=patplan.PatPlanNum
				LEFT JOIN temppending"+rndStr+@" ON temppending"+rndStr+@".PatPlanNum=patplan.PatPlanNum
				LEFT JOIN tempannualmax"+rndStr+@" ON tempannualmax"+rndStr+@".PlanNum=inssub.PlanNum
				AND (tempannualmax"+rndStr+@".AnnualMax IS NOT NULL AND tempannualmax"+rndStr+@".AnnualMax>0)/*may not be necessary*/
				WHERE tempplanned"+rndStr+@".AmtPlanned>0 ";
			if(!noIns) {//if we don't want patients without insurance
				command+=@"AND patplan.Ordinal=1 AND insplan.MonthRenew="+POut.Int(monthStart)+" ";
			}
			if(!patsWithAppts) {
				//Changed from '>' to '>=' on 02/27/15 because patients with an appointment scheduled for today were showing on the report
				command+=@"AND patient.PatNum NOT IN (SELECT PatNum FROM appointment WHERE AptStatus IN (1,4) AND "+DbHelper.DtimeToDate("AptDateTime")+">="+DbHelper.Curdate()+") ";//Scheduled and ASAP appointments.
			}
			if(aboveAmount>0) {
				command+=@"AND (tempannualmax"+rndStr+@".AnnualMax IS NULL OR tempannualmax"+rndStr+@".AnnualMax-IFNULL(tempused"+rndStr+@".AmtUsed,0)>"+POut.Double(aboveAmount)+") ";
			}
			for(int i=0;i<providerFilter.Count;i++) {
				if(i==0) {
					command+=" AND (patient.PriProv=";
				}
				else {
					command+=" OR patient.PriProv=";
				}
				command+=POut.Long(listProvs[(int)providerFilter[i]-1].ProvNum);
				if(i==providerFilter.Count-1) {
					command+=") ";
				}
			}
			for(int i=0;i<billingFilter.Count;i++) {
				if(i==0) {
					command+=" AND (patient.BillingType=";
				}
				else {
					command+=" OR patient.BillingType=";
				}
				command+=POut.Long(arrayDefs[(int)DefCat.BillingTypes][(int)billingFilter[i]-1].DefNum);
				if(i==billingFilter.Count-1) {
					command+=") ";
				}
			}
			command+=@"
				AND patient.PatStatus =0
				ORDER BY tempplanned"+rndStr+@".AmtPlanned DESC";
			DataTable rawtable=Db.GetTable(command);
			command=@"DROP TABLE tempused"+rndStr+@";
				DROP TABLE temppending"+rndStr+@";
				DROP TABLE tempplanned"+rndStr+@";
				DROP TABLE tempannualmax"+rndStr+@";";
			Db.NonQ(command);
			ContactMethod contmeth;
			for(int i=0;i<rawtable.Rows.Count;i++) {
				row=table.NewRow();
				row["PatNum"]=PIn.Long(rawtable.Rows[i]["PatNum"].ToString());
				row["LName"]=rawtable.Rows[i]["LName"].ToString();
				row["FName"]=rawtable.Rows[i]["FName"].ToString();
				contmeth=(ContactMethod)PIn.Long(rawtable.Rows[i]["PreferRecallMethod"].ToString());
				if(contmeth==ContactMethod.None){
					if(PrefC.GetBool(PrefName.RecallUseEmailIfHasEmailAddress)){//if user only wants to use email if contact method is email
						if(rawtable.Rows[i]["Email"].ToString() != "") {
							row["contactMethod"]=rawtable.Rows[i]["Email"].ToString();
						}
						else{
							row["contactMethod"]=Lans.g("FormRecallList","Hm:")+rawtable.Rows[i]["HmPhone"].ToString();
						}
					}
					else{
						row["contactMethod"]=Lans.g("FormRecallList","Hm:")+rawtable.Rows[i]["HmPhone"].ToString();
					}
				}
				if(contmeth==ContactMethod.HmPhone){
					row["contactMethod"]=Lans.g("FormRecallList","Hm:")+rawtable.Rows[i]["HmPhone"].ToString();
				}
				if(contmeth==ContactMethod.WkPhone) {
					row["contactMethod"]=Lans.g("FormRecallList","Wk:")+rawtable.Rows[i]["WkPhone"].ToString();
				}
				if(contmeth==ContactMethod.WirelessPh) {
					row["contactMethod"]=Lans.g("FormRecallList","Cell:")+rawtable.Rows[i]["WirelessPhone"].ToString();
				}
				if(contmeth==ContactMethod.Email) {
					row["contactMethod"]=rawtable.Rows[i]["Email"].ToString();
				}
				if(contmeth==ContactMethod.Mail) {
					row["contactMethod"]=Lans.g("FormRecallList","Mail");
				}
				if(contmeth==ContactMethod.DoNotCall || contmeth==ContactMethod.SeeNotes) {
					row["contactMethod"]=Lans.g("enumContactMethod",contmeth.ToString());
				}
				row["address"]=rawtable.Rows[i]["Address"].ToString();
					if(rawtable.Rows[i]["Address2"].ToString()!="") {
						row["address"]+="\r\n"+rawtable.Rows[i]["Address2"].ToString();
					}
					row["City"]=rawtable.Rows[i]["City"].ToString();
					row["State"]=rawtable.Rows[i]["State"].ToString();
					row["Zip"]=rawtable.Rows[i]["Zip"].ToString();
				row["annualMax"]=(PIn.Double(rawtable.Rows[i]["$AnnualMax"].ToString())).ToString("N");
				row["amountUsed"]=(PIn.Double(rawtable.Rows[i]["$AmountUsed"].ToString())).ToString("N");
				row["amountPending"]=(PIn.Double(rawtable.Rows[i]["$AmountPending"].ToString())).ToString("N");
				row["amountRemaining"]=(PIn.Double(rawtable.Rows[i]["$AmtRemaining"].ToString())).ToString("N");
				row["treatmentPlan"]=(PIn.Double(rawtable.Rows[i]["$TreatmentPlan"].ToString())).ToString("N");
				row["carrierName"]=rawtable.Rows[i]["CarrierName"].ToString();
				rows.Add(row);
			}
			for(int i=0;i<rows.Count;i++) {
				table.Rows.Add(rows[i]);
			}
			return table;
		}

		///<summary>Only a partial folderName will be sent in.  Not the .rvg part.</summary>
		public static bool IsTrophyFolderInUse(string folderName) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),folderName);
			}
			string command ="SELECT COUNT(*) FROM patient WHERE TrophyFolder LIKE '%"+POut.String(folderName)+"%'";
			if(Db.GetCount(command)=="0") {
				return false;
			}
			return true;
		}

		///<summary>Used to check if a billing type is in use when user is trying to hide it.</summary>
		public static bool IsBillingTypeInUse(long defNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),defNum);
			}
			string command ="SELECT COUNT(*) FROM patient WHERE BillingType="+POut.Long(defNum)+" AND PatStatus!="+POut.Int((int)PatientStatus.Deleted);
			if(Db.GetCount(command)=="0") {
				return false;
			}
			return true;
		}

		///<summary>Updated 05/09/2014 v14.2.  To prevent orphaned patients, if patFrom is a guarantor then all family members of patFrom are moved into the family patTo belongs to, and then the merge of the two specified accounts is performed.  Returns false if the merge was canceled by the user.</summary>
		public static bool MergeTwoPatients(long patTo,long patFrom){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),patTo,patFrom);
			}
			if(patTo==patFrom) {
				//Do not merge the same patient onto itself.
				return true;
			}
			string[] patNumForeignKeys=new string[]{
				"adjustment.PatNum",
				"allergy.PatNum",
				"anestheticrecord.PatNum",
				"anesthvsdata.PatNum",
				"appointment.PatNum",
				"claim.PatNum",
				"claimproc.PatNum",
				"commlog.PatNum",
				"creditcard.PatNum",
				"custrefentry.PatNumCust",
				"custrefentry.PatNumRef",
				//"custreference.PatNum",  //This is handled below.  We do not want to change patnum, the references form only shows entries for active patients.
				"disease.PatNum",
				//"document.PatNum",  //This is handled below when images are stored in the database and on the client side for images stored in the AtoZ folder due to the middle tier.
				"ehramendment.PatNum",
				"ehrcareplan.PatNum",
				"ehrlab.PatNum",
				"ehrmeasureevent.PatNum",
				"ehrnotperformed.PatNum",				
				//"ehrpatient.PatNum",  //This is handled below.  We do not want to change patnum here because there can only be one entry per patient.
				"ehrprovkey.PatNum",
				"ehrquarterlykey.PatNum",
				"ehrsummaryccd.PatNum",
				"emailmessage.PatNum",
				"encounter.PatNum",
				"erxlog.PatNum",
				"etrans.PatNum",
				"familyhealth.PatNum",
				"formpat.PatNum",
				"hl7msg.PatNum",
				"inssub.Subscriber",
				"installmentplan.PatNum",
				"intervention.PatNum",
				"labcase.PatNum",
				"labpanel.PatNum",
				"medicalorder.PatNum",
				"medicationpat.PatNum",
				"mount.PatNum",
				"orthochart.PatNum",
				//"patfield.PatNum", //Taken care of below
				"patient.ResponsParty",
				//"patientnote.PatNum"	//The patientnote table is ignored because only one record can exist for each patient.  The record in 'patFrom' remains so it can be accessed again if needed.
				//"patientrace.PatNum", //The patientrace table is ignored because we don't want duplicate races.  We could merge them but we would have to add specific code to stop duplicate races being inserted.
				"patplan.PatNum",
				"payment.PatNum",
				"payortype.PatNum",
				"payplan.Guarantor",//Treated as a patnum, because it is actually a guarantor for the payment plan, and not a patient guarantor.
				"payplan.PatNum",				
				"payplancharge.Guarantor",//Treated as a patnum, because it is actually a guarantor for the payment plan, and not a patient guarantor.
				"payplancharge.PatNum",
				"paysplit.PatNum",
				"perioexam.PatNum",
				"phonenumber.PatNum",
				"plannedappt.PatNum",
				"popup.PatNum",
				"procedurelog.PatNum",
				"procnote.PatNum",
				"proctp.PatNum",
				"question.PatNum",
				"recall.PatNum",
				"refattach.PatNum",
				//"referral.PatNum",  //This is synched with the new information below.
				"registrationkey.PatNum",
				"repeatcharge.PatNum",
				"reqstudent.PatNum",
				"reseller.PatNum",
				"rxpat.PatNum",
				"screenpat.PatNum",
				"securitylog.PatNum",
				"sheet.PatNum",
				"statement.PatNum",
				//task.KeyNum,  //Taken care of in a seperate step, because it is not always a patnum.
				"terminalactive.PatNum",
				"toothinitial.PatNum",
				"treatplan.PatNum",
				"treatplan.ResponsParty",
				"vaccinepat.PatNum",
				//vaccinepat.VaccinePatNum IS NOT a PatNum so it is should not be merged. It is the primary key.
				//vaccineobs.VaccinePatNum IS NOT a PatNum so it is should not be merged. It is the FK to the vaccinepat.VaccinePatNum.
				"vitalsign.PatNum",
				"xchargetransaction.PatNum"
			};
			string command="";
			Patient patientFrom=Patients.GetPat(patFrom);
			Patient patientTo=Patients.GetPat(patTo);
			//We need to test patfields before doing anything else because the user may wish to cancel and abort the merge.
			PatField[] patToFields=PatFields.Refresh(patTo);
			PatField[] patFromFields=PatFields.Refresh(patFrom);
			List<PatField> patFieldsToDelete=new List<PatField>();
			List<PatField> patFieldsToUpdate=new List<PatField>();
			for(int i=0;i<patFromFields.Length;i++) {
				bool hasMatch=false;
				for(int j=0;j<patToFields.Length;j++) {
					//Check patient fields that are the same to see if they have different values.
					if(patFromFields[i].FieldName==patToFields[j].FieldName) {
						hasMatch=true;
						if(patFromFields[i].FieldValue!=patToFields[j].FieldValue) {
							//Get input from user on which value to use.
							DialogResult result=MessageBox.Show("The two patients being merged have different values set for the patient field:\r\n\""+patFromFields[i].FieldName+"\"\r\n\r\n"
								+"The merge into patient has the value: \""+patToFields[j].FieldValue+"\"\r\n"
								+"The merge from patient has the value: \""+patFromFields[i].FieldValue+"\"\r\n\r\n"
								+"Would you like to overwrite the merge into value with the merge from value?\r\n(Cancel will abort the merge)","Warning",MessageBoxButtons.YesNoCancel);
							if(result==DialogResult.Yes) {
								//User chose to use the merge from patient field info.
								patFromFields[i].PatNum=patTo;
								patFieldsToUpdate.Add(patFromFields[i]);
								patFieldsToDelete.Add(patToFields[j]);
							}
							else if(result==DialogResult.Cancel) {
								return false;//Completely cancels the entire merge.  No changes have been made at this point.
							}
						}
					}
				}
				if(!hasMatch) {//The patient field does not exist in the merge into account.
					patFromFields[i].PatNum=patTo;
					patFieldsToUpdate.Add(patFromFields[i]);
				}
			}

			//Do not allow the user to abort below this point.  Any checks that could let a user abort the merge should be done above.
			#region Point of no return
			//Update and remove all patfields that were added to the list above.
			for(int i=0;i<patFieldsToDelete.Count;i++) {
				PatFields.Delete(patFieldsToDelete[i]);
			}
			for(int j=0;j<patFieldsToUpdate.Count;j++) {
				PatFields.Update(patFieldsToUpdate[j]);
			}
			//CustReference.  We need to combine patient from and patient into entries to have the into patient information from both.
			CustReference custRefFrom=CustReferences.GetOneByPatNum(patientFrom.PatNum);
			CustReference custRefTo=CustReferences.GetOneByPatNum(patientTo.PatNum);
			if(custRefFrom!=null && custRefTo!=null) { //If either of these are null, do nothing.  This is an internal only table so we didn't bother fixing it/warning users here.
				CustReference newCustRef=new CustReference();
				newCustRef.CustReferenceNum=custRefTo.CustReferenceNum; //Use same primary key so we can update.
				newCustRef.PatNum=patientTo.PatNum;
				if(custRefTo.DateMostRecent > custRefFrom.DateMostRecent) {
					newCustRef.DateMostRecent=custRefTo.DateMostRecent; //Use the most recent date.
				}
				else {
					newCustRef.DateMostRecent=custRefFrom.DateMostRecent; //Use the most recent date.
				}
				if(custRefTo.Note=="") {
					newCustRef.Note=custRefFrom.Note;
				}
				else if(custRefFrom.Note=="") {
					newCustRef.Note=custRefTo.Note;
				}
				else {//Both entries have a note
					newCustRef.Note=(custRefTo.Note+" | "+custRefFrom.Note); /*Combine in a | delimited string*/
				}
				newCustRef.IsBadRef=(custRefFrom.IsBadRef || custRefTo.IsBadRef);  //If either entry is a bad reference, count as a bad reference.
				CustReferences.Update(newCustRef); //Overwrites the old custRefTo entry.
			}
			//Merge ehrpatient.  We only do something here if there is a FROM patient entry and no INTO patient entry, in which case we change the patnum on the row to bring it over.
			EhrPatient ehrPatFrom=EhrPatients.GetOne(patientFrom.PatNum);
			EhrPatient ehrPatTo=EhrPatients.GetOne(patientTo.PatNum);
			if(ehrPatFrom!=null && ehrPatTo==null) {  //There is an entry for the FROM patient, but not the INTO patient.
				ehrPatFrom.PatNum=patientTo.PatNum;
				EhrPatients.Update(ehrPatFrom); //Bring the patfrom entry over to the new.
			}
			//Move the patient documents if they are stored in the database.
			//We do not have to worry about documents having the same name when storing within the database, only physical documents need to be renamed.
			//Physical documents are handled on the client side (not here) due to middle tier issues.
			if(!PrefC.AtoZfolderUsed) {
				//Storing documents in the database.  Simply update the PatNum column accordingly. 
				//This query cannot be ran below where all the other tables are handled dyncamically because we do NOT want to update the PatNums in the case that documents are stored physically.
				command="UPDATE document "
					+"SET PatNum="+POut.Long(patTo)+" "
					+"WHERE PatNum="+POut.Long(patFrom);
				Db.NonQ(command);
			}
			//If the 'patFrom' had any ties to guardians, they should be deleted to prevent duplicate entries.
			command="DELETE FROM guardian"
				+" WHERE PatNumChild="+POut.Long(patFrom)
				+" OR PatNumGuardian="+POut.Long(patFrom);
			Db.NonQ(command);
			//Update all guarantor foreign keys to change them from 'patFrom' to 
			//the guarantor of 'patTo'. This will effectively move all 'patFrom' family members 
			//to the family defined by 'patTo' in the case that 'patFrom' is a guarantor. If
			//'patFrom' is not a guarantor, then this command will have no effect and is
			//thus safe to always be run.
			command="UPDATE patient "
				+"SET Guarantor="+POut.Long(patientTo.Guarantor)+" "
				+"WHERE Guarantor="+POut.Long(patFrom);
			Db.NonQ(command);
			//At this point, the 'patFrom' is a regular patient and is absoloutely not a guarantor.
			//Now modify all PatNum foreign keys from 'patFrom' to 'patTo' to complete the majority of the
			//merge of the records between the two accounts.			
			for(int i=0;i<patNumForeignKeys.Length;i++) {
				if(DataConnection.DBtype==DatabaseType.Oracle 
					&& patNumForeignKeys[i]=="ehrlab.PatNum") //Oracle does not currently support EHR labs.
				{
					continue;
				}
				string[] tableAndKeyName=patNumForeignKeys[i].Split(new char[] {'.'});
				command="UPDATE "+tableAndKeyName[0]
					+" SET "+tableAndKeyName[1]+"="+POut.Long(patTo)
					+" WHERE "+tableAndKeyName[1]+"="+POut.Long(patFrom);
				Db.NonQ(command);
			}
			//We have to move over the tasks belonging to the 'patFrom' patient in a seperate step because
			//the KeyNum field of the task table might be a foreign key to something other than a patnum,
			//including possibly an appointment number.
			command="UPDATE task "
				+"SET KeyNum="+POut.Long(patTo)+" "
				+"WHERE KeyNum="+POut.Long(patFrom)+" AND ObjectType="+((int)TaskObjectType.Patient);
			Db.NonQ(command);
			//Mark the patient where data was pulled from as archived unless the patient is already marked as deceased.
			//We need to have the patient marked either archived or deceased so that it is hidden by default, and
			//we also need the customer to be able to access the account again in case a particular table gets missed
			//in the merge tool after an update to Open Dental. This will allow our customers to remerge the missing
			//data after a bug fix is released. 
			command="UPDATE patient "
				+"SET PatStatus="+((int)PatientStatus.Archived)+" "
				+"WHERE PatNum="+POut.Long(patFrom)+" "
				+"AND PatStatus<>"+((int)PatientStatus.Deceased)+" "
				+DbHelper.LimitAnd(1);
			Db.NonQ(command);
			//This updates the referrals with the new patient information from the merge.
			for(int i=0;i<Referrals.List.Length;i++){
				if(Referrals.List[i].PatNum==patFrom){
					//Referrals.Cur=Referrals.List[i];
					Referrals.List[i].PatNum=patientTo.PatNum;
					Referrals.List[i].LName=patientTo.LName;
					Referrals.List[i].FName=patientTo.FName;
					Referrals.List[i].MName=patientTo.MiddleI;
					Referrals.List[i].Address=patientTo.Address;
					Referrals.List[i].Address2=patientTo.Address2;
					Referrals.List[i].City=patientTo.City;
					Referrals.List[i].ST=patientTo.State;
					Referrals.List[i].SSN=patientTo.SSN;
					Referrals.List[i].Zip=patientTo.Zip;
					Referrals.List[i].Telephone=TelephoneNumbers.FormatNumbersExactTen(patientTo.HmPhone);
					Referrals.List[i].EMail=patientTo.Email;
					Referrals.Update(Referrals.List[i]);
					Referrals.RefreshCache();
					break;
				}
			}
			#endregion
			return true;
		}

		///<summary>LName, 'Preferred' FName M</summary>
		public static string GetNameLF(string LName,string FName,string Preferred,string MiddleI) {
			//No need to check RemotingRole; no call to db.
			string retVal="";
			//if(Title!=""){
			//	retVal+=Title+" ";
			//}
			retVal+=LName+", ";
			if(Preferred!=""){
				retVal+="'"+Preferred+"' ";
			}
			retVal+=FName;
			if(MiddleI!=""){
				retVal+=" "+MiddleI;
			}
			return retVal;
		}

		///<summary>FName 'Preferred' M LName</summary>
		public static string GetNameFL(string LName,string FName,string Preferred,string MiddleI) {
			//No need to check RemotingRole; no call to db.
			string retVal="";
			//if(Title!="") {
				//retVal+=Title+" ";
			//}
			retVal+=FName+" ";
			if(Preferred!="") {
				retVal+="'"+Preferred+"' ";
			}
			if(MiddleI!="") {
				retVal+=MiddleI+" ";
			}
			retVal+=LName;
			return retVal;
		}

		///<summary>FName M LName</summary>
		public static string GetNameFLnoPref(string LName,string FName,string MiddleI) {
			//No need to check RemotingRole; no call to db.
			string retVal="";
			retVal+=FName+" ";
			if(MiddleI!="") {
				retVal+=MiddleI+" ";
			}
			retVal+=LName;
			return retVal;
		}

		///<summary>FName/Preferred LName</summary>
		public static string GetNameFirstOrPrefL(string LName,string FName,string Preferred) {
			//No need to check RemotingRole; no call to db.
			string retVal="";
			if(Preferred=="") {
				retVal+=FName+" ";
			}
			else {
				retVal+=Preferred+" ";
			}
			retVal+=LName;
			return retVal;
		}

		///<summary>FName/Preferred M. LName</summary>
		public static string GetNameFirstOrPrefML(string LName,string FName,string Preferred,string MiddleI) {
			//No need to check RemotingRole; no call to db.
			string retVal="";
			if(Preferred=="") {
				retVal+=FName+" ";
			}
			else {
				retVal+=Preferred+" ";
			}
			if(MiddleI!="") {
				retVal+=MiddleI+". ";
			}
			retVal+=LName;
			return retVal;
		}

		///<summary>Title FName M LName</summary>
		public static string GetNameFLFormal(string LName,string FName,string MiddleI,string Title) {
			//No need to check RemotingRole; no call to db.
			string retVal="";
			if(Title!="") {
				retVal+=Title+" ";
			}
			retVal+=FName+" "+MiddleI+" "+LName;
			return retVal;
		}

		///<summary>Includes preferred.</summary>
		public static string GetNameFirst(string FName,string Preferred) {
			//No need to check RemotingRole; no call to db.
			string retVal=FName;
			if(Preferred!="") {
				retVal+=" '"+Preferred+"'";
			}
			return retVal;
		}

		///<summary>Returns preferred name if one exists, otherwise returns first name.</summary>
		public static string GetNameFirstOrPreferred(string FName,string Preferred) {
			//No need to check RemotingRole; no call to db.
			if(Preferred!="") {
				return Preferred;
			}
			return FName;
		}

		///<summary>Dear __.  Does not include the "Dear" or the comma.</summary>
		public static string GetSalutation(string Salutation,string Preferred,string FName) {
			//No need to check RemotingRole; no call to db.
			if(Salutation!="") {
				return Salutation;
			}
			if(Preferred!="") {
				return Preferred;
			}
			return FName;
		}

		/// <summary>Result will be multiline.</summary>
		public static string GetAddressFull(string address,string address2,string city,string state,string zip) {
			string retVal=address;
			if(address2!="") {
				retVal+="\r\n"+address2;
			}
			retVal+="\r\n"+city+", "+state+" "+zip;
			return retVal;
		}

		/// <summary>Change preferred provider for all patients with provNumFrom to provNumTo.</summary>
		public static void ChangePrimaryProviders(long provNumFrom,long provNumTo) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),provNumFrom,provNumTo);
				return;
			}
			string command= 
				"UPDATE patient " 
				+"SET PriProv = '"+provNumTo+"' "
				+"WHERE PriProv = '"+provNumFrom+"'";
			Db.NonQ(command);
		}

		///<summary>Change secondary provider for all patients with provNumFrom to provNumTo.</summary>
		public static void ChangeSecondaryProviders(long provNumFrom,long provNumTo) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),provNumFrom,provNumTo);
				return;
			}
			string command="UPDATE patient " 
				+"SET SecProv = '"+provNumTo+"' "
				+"WHERE SecProv = '"+provNumFrom+"'";
			Db.NonQ(command);
		}
		
		/// <summary>Gets all patients whose primary provider PriProv is in the list provNums.</summary>
		public static DataTable GetPatsByPriProvs(List<long> provNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),provNums);
			}
			if(provNums.Count==0) {
				return null;
			}
			string providers="";
			for(int i=0;i<provNums.Count;i++) {
				providers+=provNums[i].ToString();
				if(i<provNums.Count-1) {
					providers+=",";
				}
			}
			string command="SELECT PatNum,PriProv FROM patient WHERE PriProv IN ("+providers+")";
			return Db.GetTable(command);
		}

		/// <summary>Find the most used provider for a single patient. Bias towards the most recently used provider if they have done an equal number of procedures.</summary>
		public static long ReassignProvGetMostUsed(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT ProvNum,MAX(ProcDate) MaxProcDate,COUNT(ProvNum) ProcCount "
				+"FROM procedurelog "
				+"WHERE PatNum="+POut.Long(patNum)+" "
				+"AND ProcStatus="+POut.Int((int)ProcStat.C)+" "
				+"GROUP BY ProvNum";
			DataTable table=Db.GetTable(command);
			long newProv=0;
			int mostVisits=0;
			DateTime maxProcDate=new DateTime();
			for(int i=0;i<table.Rows.Count;i++) {//loop through providers
				if(PIn.Int(table.Rows[i]["ProcCount"].ToString())>mostVisits) {//New leader for most visits.
					mostVisits=PIn.Int(table.Rows[i]["ProcCount"].ToString());
					maxProcDate=PIn.DateT(table.Rows[i]["MaxProcDate"].ToString());
					newProv=PIn.Long(table.Rows[i]["ProvNum"].ToString());
				}
				else if(PIn.Int(table.Rows[i]["ProcCount"].ToString())==mostVisits) {//Tie for most visits, use MaxProcDate as a tie breaker.
					if(PIn.DateT(table.Rows[i]["MaxProcDate"].ToString())>maxProcDate) {
						//mostVisits same as before
						maxProcDate=PIn.DateT(table.Rows[i]["MaxProcDate"].ToString());
						newProv=PIn.Long(table.Rows[i]["ProvNum"].ToString());
					}
				}
			}
			return newProv;
		}

		/// <summary>Change preferred provider PriProv to provNum for patient with PatNum=patNum.</summary>
		public static void ReassignProv(long patNum,long provNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),patNum,provNum);
				return;
			}
			string command= 
				"UPDATE patient " 
				+"SET PriProv = '"+POut.Long(provNum)+"' "
				+"WHERE PatNum = '"+POut.Long(patNum)+"'";
			Db.NonQ(command);
		}

		///<summary>Gets the number of patients with unknown Zip.</summary>
		public static int GetZipUnknown(DateTime dateFrom, DateTime dateTo) {
			string command="SELECT COUNT(*) "
				+"FROM patient "
				+"WHERE "+DbHelper.Regexp("Zip","^[0-9]{5}",false)+" "//Does not start with five numbers
				+"AND PatNum IN ( "
					+"SELECT DISTINCT PatNum FROM procedurelog "
					+"WHERE ProcStatus="+POut.Int((int)ProcStat.C)+" "
					+"AND DateEntryC >= "+POut.Date(dateFrom)+" "
					+"AND DateEntryC <= "+POut.Date(dateTo)+") "
				+"AND Birthdate<=CURDATE() "//Birthday not in the future (at least 0 years old)
				+"AND Birthdate>SUBDATE(CURDATE(),INTERVAL 200 YEAR) ";//Younger than 200 years old
			return PIn.Int(Db.GetCount(command));
		}

		///<summary>Gets the number of qualified patients (having a completed procedure within the given time frame) in zip codes with less than 9 other qualified patients in that same zip code.</summary>
		public static int GetZipOther(DateTime dateFrom, DateTime dateTo) {
			string command="SELECT SUM(Patients) FROM "
				+"(SELECT SUBSTR(Zip,1,5) Zip_Code,COUNT(*) Patients "//Column headings Zip_Code and Patients are provided by the USD 2010 Manual.
				+"FROM patient "
				+"WHERE "+DbHelper.Regexp("Zip","^[0-9]{5}")+" "//Starts with five numbers
				+"AND PatNum IN ( "
					+"SELECT DISTINCT PatNum FROM procedurelog "
					+"WHERE ProcStatus="+POut.Int((int)ProcStat.C)+" "
					+"AND DateEntryC >= "+POut.Date(dateFrom)+" "
					+"AND DateEntryC <= "+POut.Date(dateTo)+") "
				+"AND Birthdate<=CURDATE() "//Birthday not in the future (at least 0 years old)
				+"AND Birthdate>SUBDATE(CURDATE(),INTERVAL 200 YEAR) "//Younger than 200 years old
				+"GROUP BY Zip "
				+"HAVING COUNT(*) < 10) patzip";//Has less than 10 patients in that zip code for the given time frame.
			return PIn.Int(Db.GetCount(command));
		}
		
		///<summary>Gets the total number of patients with completed procedures between dateFrom and dateTo. Also checks for age between 0 and 200.</summary>
		public static int GetPatCount(DateTime dateFrom, DateTime dateTo) {
			string command="SELECT COUNT(*) "
				+"FROM patient "
				+"WHERE PatNum IN ( "
					+"SELECT DISTINCT PatNum FROM procedurelog "
					+"WHERE ProcStatus="+POut.Int((int)ProcStat.C)+" "
					+"AND DateEntryC >= "+POut.Date(dateFrom)+" "
					+"AND DateEntryC <= "+POut.Date(dateTo)+") "
				+"AND Birthdate<=CURDATE() "//Birthday not in the future (at least 0 years old)
				+"AND Birthdate>SUBDATE(CURDATE(),INTERVAL 200 YEAR) ";//Younger than 200 years old
			return PIn.Int(Db.GetCount(command));
		}

		///<summary>Gets the total number of patients with completed procedures between dateFrom and dateTo who are at least agelow and strictly younger than agehigh.</summary>
		public static int GetAgeGenderCount(int agelow,int agehigh,PatientGender gender,DateTime dateFrom, DateTime dateTo) {
			bool male=true;//Since all the numbers must add up to equal, we count unknown and other genders as female.
			if(gender!=0) {
				male=false;
			}
			string command="SELECT COUNT(*) "
				+"FROM patient pat "
				+"WHERE PatNum IN ( "
					+"SELECT DISTINCT PatNum FROM procedurelog "
					+"WHERE ProcStatus="+POut.Int((int)ProcStat.C)+" "
					+"AND DateEntryC >= "+POut.Date(dateFrom)+" "
					+"AND DateEntryC <= "+POut.Date(dateTo)+") "
				+"AND Gender"+(male?"=0":"!=0")+" "
				+"AND Birthdate<=SUBDATE(CURDATE(),INTERVAL "+agelow+" YEAR) "//Born before this date
				+"AND Birthdate>SUBDATE(CURDATE(),INTERVAL "+agehigh+" YEAR)";//Born after this date
			return PIn.Int(Db.GetCount(command));
		}

		///<summary>Returns a list of patients belonging to the SuperFamily</summary>
		public static List<Patient> GetBySuperFamily(long SuperFamilyNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod(),SuperFamilyNum);
			}
			if(SuperFamilyNum==0) {
				return new List<Patient>();//return empty list
			}
			string command="SELECT * FROM patient WHERE SuperFamily="+POut.Long(SuperFamilyNum);
			return Crud.PatientCrud.TableToList(Db.GetTable(command));
		}

		///<summary>Returns a list of patients that are the guarantors for the patients in the Super Family</summary>
		public static List<Patient> GetSuperFamilyGuarantors(long SuperFamilyNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod(),SuperFamilyNum);
			}
			if(SuperFamilyNum==0) {
				return new List<Patient>();//return empty list
			}
			//Should also work in Oracle.
			//this query was taking 2.5 seconds on a large database
			//string command = "SELECT DISTINCT * FROM patient WHERE PatNum IN (SELECT Guarantor FROM patient WHERE SuperFamily="+POut.Long(SuperFamilyNum)+") "
			//	+"AND PatStatus!="+POut.Int((int)PatientStatus.Deleted);
			//optimized to 0.001 second runtime on same db
			string command = "SELECT DISTINCT * FROM patient WHERE SuperFamily="+POut.Long(SuperFamilyNum)
				+" AND PatStatus!="+POut.Int((int)PatientStatus.Deleted)+" AND PatNum=Guarantor";
			return Crud.PatientCrud.TableToList(Db.GetTable(command));
		}

		public static void AssignToSuperfamily(long guarantor,long superFamilyNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),guarantor,superFamilyNum);
				return;
			}
			string command="UPDATE patient SET SuperFamily="+POut.Long(superFamilyNum)+" WHERE Guarantor="+POut.Long(guarantor);
			Db.NonQ(command);
		}

		public static void MoveSuperFamily(long oldSuperFamilyNum,long newSuperFamilyNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),oldSuperFamilyNum,newSuperFamilyNum);
				return;
			}
			if(oldSuperFamilyNum==0) {
				return;
			}
			string command="UPDATE patient SET SuperFamily="+newSuperFamilyNum+" WHERE SuperFamily="+oldSuperFamilyNum;
			Db.NonQ(command);
		}

		public static void DisbandSuperFamily(long SuperFamilyNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),SuperFamilyNum);
			}
			if(SuperFamilyNum==0) {
				return;
			}
			string command = "UPDATE patient SET SuperFamily=0 WHERE SuperFamily="+POut.Long(SuperFamilyNum);
			Db.NonQ(command);
		}

		public static List<Patient> GetPatsForScreenGroup(long screenGroupNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod(),screenGroupNum);
			}
			if(screenGroupNum==0) {
				return new List<Patient>();
			}
			string command = "SELECT * FROM patient WHERE PatNum IN (SELECT PatNum FROM screenpat WHERE ScreenGroupNum="+POut.Long(screenGroupNum)+")";
			return Crud.PatientCrud.SelectMany(command);
		}

		///<summary>Get a list of patients for FormEhrPatientExport. If provNum, clinicNum, or siteNum are =0 get all.</summary>
		public static DataTable GetExportList(long patNum, string firstName,string lastName,long provNum,long clinicNum,long siteNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),patNum,firstName,lastName,provNum,clinicNum,siteNum);
			}
			string command = "SELECT patient.PatNum, patient.FName, patient.LName, provider.Abbr AS Provider, clinic.Description AS Clinic, site.Description AS Site "
				+"FROM patient "
				+"INNER JOIN provider ON patient.PriProv=provider.ProvNum "
				+"LEFT JOIN clinic ON patient.ClinicNum=clinic.ClinicNum "
				+"LEFT JOIN site ON patient.SiteNum=site.SiteNum "
				+"WHERE patient.PatStatus=0 ";
			if(patNum != 0) {
				command+="AND patient.PatNum LIKE '%"+POut.Long(patNum)+"%' ";
			}
			if(firstName != "") {
				command+="AND patient.FName LIKE '%"+POut.String(firstName)+"%' ";
			}
			if(lastName != "") {
				command+="AND patient.LName LIKE '%"+POut.String(lastName)+"%' ";
			}
			if(provNum>0) {
				command+="AND provider.ProvNum = "+POut.Long(provNum)+" ";
			}
			if(clinicNum>0) {
				command+="AND clinic.ClinicNum = "+POut.Long(clinicNum)+" ";
			}
			if(siteNum>0) {
				command+="AND site.SiteNum = "+POut.Long(siteNum)+" ";
			}
			command+="ORDER BY patient.LName,patient.FName ";
			return (Db.GetTable(command));
		}

		///<summary>Returns a list of Patients of which this PatNum is eligible to view given PHI constraints.</summary>
		public static List<Patient> GetPatientsForPhi(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod(),patNum);
			}
			List<long> listPatNums=GetPatNumsForPhi(patNum);
			//If there are duplicates in listPatNums, then they will be removed because of the IN statement in the query below.
			string command="SELECT * FROM patient WHERE PatNum IN ("+string.Join(",",listPatNums)+")";
			return Crud.PatientCrud.SelectMany(command);
		}

		///<summary>Returns a list of PatNum(s) of which this PatNum is eligible to view given PHI constraints.  Used internally and also used by Patient Portal.</summary>
		public static List<long> GetPatNumsForPhi(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),patNum);
			}
			List<long> listPatNums=new List<long>();
			listPatNums.Add(patNum);
			if(OpenDentBusiness.PrefC.GetBool(OpenDentBusiness.PrefName.FamPhiAccess)) { //Include guarantor's family if pref is set.
				//Include any patient where this PatNum is the Guarantor.
				DataTable tablePatientsG=OpenDentBusiness.DataCore.GetTable("SELECT PatNum FROM patient WHERE Guarantor = "+OpenDentBusiness.POut.Long(patNum));
				for(int i=0;i<tablePatientsG.Rows.Count;i++) {
					listPatNums.Add(OpenDentBusiness.PIn.Long(tablePatientsG.Rows[i]["PatNum"].ToString()));
				}
			}
			//Include any patient where the given patient is the responsible party.
			DataTable tablePatientsR=OpenDentBusiness.DataCore.GetTable("SELECT PatNum FROM patient WHERE ResponsParty = "+OpenDentBusiness.POut.Long(patNum));
			for(int i=0;i<tablePatientsR.Rows.Count;i++) {
				listPatNums.Add(OpenDentBusiness.PIn.Long(tablePatientsR.Rows[i]["PatNum"].ToString()));
			}
			//Include any patient where this patient is the guardian.
			DataTable tablePatientsD=OpenDentBusiness.DataCore.GetTable("SELECT PatNum FROM patient WHERE PatNum IN (SELECT guardian.PatNumChild FROM guardian WHERE guardian.IsGuardian = 1 AND guardian.PatNumGuardian="+OpenDentBusiness.POut.Long(patNum)+") ");
			for(int i=0;i<tablePatientsD.Rows.Count;i++) {
				listPatNums.Add(OpenDentBusiness.PIn.Long(tablePatientsD.Rows[i]["PatNum"].ToString()));
			}
			return listPatNums;//There might be dupliates, but the list should be short enough that it does not matter.
		}

		///<summary>Validate password against strong password rules. Currently only used for patient portal passwords. Requirements: 8 characters, 1 uppercase character, 1 lowercase character, 1 number. Returns non-empty string if validation failed. Return string will be translated.</summary>
		public static string IsPortalPasswordValid(string newPassword) {
			if(newPassword.Length<8) {
				return Lans.g("FormPatientPortal","Password must be at least 8 characters long.");
			}
			if(!Regex.IsMatch(newPassword,"[A-Z]+")) {
				return Lans.g("FormPatientPortal","Password must contain an uppercase letter.");
			}
			if(!Regex.IsMatch(newPassword,"[a-z]+")) {
				return Lans.g("FormPatientPortal","Password must contain an lowercase letter.");
			}
			if(!Regex.IsMatch(newPassword,"[0-9]+")) {
				return Lans.g("FormPatientPortal","Password must contain a number.");
			}
			return "";
		}

		///<summary>Returns a distinct list of PatNums for guarantors that have any family member with passed in clinics, or have had work done at passed in clinics.</summary>
		public static string GetClinicGuarantors(string clinicNums) {
			string clinicGuarantors="";
			//Get guarantor of patients with clinic from comma delimited list
			string command="SELECT DISTINCT Guarantor FROM patient WHERE ClinicNum IN ("+clinicNums+")";
			DataTable table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				if(i>0 || clinicGuarantors!="") {
					clinicGuarantors+=",";
				}
				clinicGuarantors+=PIn.String(table.Rows[i]["Guarantor"].ToString());
			}
			//Get guarantor of patients who have had work done at clinic in comma delimited list
			command="SELECT DISTINCT Guarantor "
				+"FROM procedurelog "
				+"INNER JOIN patient ON patient.PatNum=procedurelog.PatNum "
					+"AND patient.PatStatus !=4 "
				+"WHERE procedurelog.ProcStatus IN (1,2) "
				+"AND procedurelog.ClinicNum IN ("+clinicNums+")";
			table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				if(i>0 || clinicGuarantors!="") {
					clinicGuarantors+=",";
				}
				clinicGuarantors+=PIn.String(table.Rows[i]["Guarantor"].ToString());
			}
			return clinicGuarantors;
		}
	}

	///<summary>Not a database table.  Just used in billing and finance charges.</summary>
	public class PatAging{
		///<summary></summary>
		public long PatNum;
		///<summary></summary>
		public double Bal_0_30;
		///<summary></summary>
		public double Bal_31_60;
		///<summary></summary>
		public double Bal_61_90;
		///<summary></summary>
		public double BalOver90;
		///<summary></summary>
		public double InsEst;
		///<summary></summary>
		public string PatName;
		///<summary></summary>
		public double BalTotal;
		///<summary></summary>
		public double AmountDue;
		///<summary>The patient priprov to assign the finance charge to.</summary>
		public long PriProv;
		///<summary>The date of the last statement.</summary>
		public DateTime DateLastStatement;
		///<summary>FK to defNum.</summary>
		public long BillingType;
		///<summary></summary>
		public double PayPlanDue;
	}

}