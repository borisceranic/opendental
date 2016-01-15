 using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness{

	///<summary></summary>
	public class Providers{
		
		///<summary></summary>
		public static DataTable RefreshCache() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM provider";
			if(PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
				command+=" ORDER BY ItemOrder";
			}
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="Provider";
			FillCache(table);
			return table;
		}

		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			List<Provider> listProvidersShort=new List<Provider>();
			List<Provider> listProvidersLong=Crud.ProviderCrud.TableToList(table);
			for(int i=0;i<listProvidersLong.Count;i++){
				if(!listProvidersLong[i].IsHidden){
					listProvidersShort.Add(listProvidersLong[i]);	
				}
			}
			ProviderC.ListShort=listProvidersShort;
			ProviderC.ListLong=listProvidersLong;
		}

		///<summary></summary>
		public static void Update(Provider provider){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),provider);
				return;
			}
			Crud.ProviderCrud.Update(provider);
		}

		///<summary></summary>
		public static long Insert(Provider provider){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				provider.ProvNum=Meth.GetLong(MethodBase.GetCurrentMethod(),provider);
				return provider.ProvNum;
			}
			return Crud.ProviderCrud.Insert(provider);
		}

		/// <summary>This checks for the maximum number of provnum in the database and then returns the one directly after.  Not guaranteed to be a unique primary key.</summary>
		public static long GetNextAvailableProvNum() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod());
			}
			string command="SELECT MAX(provNum) FROM provider";
			return PIn.Long(Db.GetScalar(command))+1;
		}

		///<summary>Increments all (privider.ItemOrder)s that are >= the ItemOrder of the provider passed in 
		///but does not change the item order of the provider passed in.</summary>
		public static void MoveDownBelow(Provider provider) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),provider);
				return;
			}
			//Add 1 to all item orders equal to or greater than new provider's item order
			Db.NonQ("UPDATE provider SET ItemOrder=ItemOrder+1"
				+" WHERE ProvNum!="+provider.ProvNum
				+" AND ItemOrder>="+provider.ItemOrder);
		}

		///<summary>Only used from FormProvEdit if user clicks cancel before finishing entering a new provider.</summary>
		public static void Delete(Provider prov){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),prov);
				return;
			}
			string command="DELETE from provider WHERE provnum = '"+prov.ProvNum.ToString()+"'";
 			Db.NonQ(command);
		}

		///<summary>Gets table for main provider edit list.  Always orders by ItemOrder.</summary>
		public static DataTable RefreshStandard(){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod());
			}
			string command="SELECT Abbr,LName,FName,provider.IsHidden,provider.ItemOrder,provider.ProvNum,MAX(UserName) UserName,PatCountPri,PatCountSec,ProvStatus "//Max function used for Oracle compatability (some providers may have multiple user names).
				+"FROM provider "
				+"LEFT JOIN userod ON userod.ProvNum=provider.ProvNum "//there can be multiple userods attached to one provider
				+"LEFT JOIN (SELECT PriProv, COUNT(*) PatCountPri FROM patient "
					+"WHERE patient.PatStatus!="+POut.Int((int)PatientStatus.Deleted)+" AND patient.PatStatus!="+POut.Int((int)PatientStatus.Deceased)+" "
					+"GROUP BY PriProv) patPri ON provider.ProvNum=patPri.PriProv  ";
			command+="LEFT JOIN (SELECT SecProv,COUNT(*) PatCountSec FROM patient "
				+"WHERE patient.PatStatus!="+POut.Int((int)PatientStatus.Deleted)+" AND patient.PatStatus!="+POut.Int((int)PatientStatus.Deceased)+" "
				+"GROUP BY SecProv) patSec ON provider.ProvNum=patSec.SecProv ";
			command+="GROUP BY Abbr,LName,FName,provider.IsHidden,provider.ItemOrder,provider.ProvNum,PatCountPri,PatCountSec ";
			command+="ORDER BY ItemOrder";
			return Db.GetTable(command);
		}

		///<summary>Gets table for main provider edit list when in dental school mode.  Always orders alphabetically, but there will be lots of filters to get the list shorter.  Must be very fast because refreshes while typing.  selectAll will trump selectInstructors and always return all providers.</summary>
		public static DataTable RefreshForDentalSchool(long schoolClassNum,string lastName,string firstName,string provNum,bool selectInstructors,bool selectAll) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),schoolClassNum,lastName,firstName,provNum,selectInstructors,selectAll);
			}
			string command="SELECT Abbr,LName,FName,provider.IsHidden,provider.ItemOrder,provider.ProvNum,GradYear,IsInstructor,Descript,MAX(UserName) UserName,PatCountPri,PatCountSec,ProvStatus "//Max function used for Oracle compatability (some providers may have multiple user names).
				+"FROM provider LEFT JOIN schoolclass ON provider.SchoolClassNum=schoolclass.SchoolClassNum "
				+"LEFT JOIN userod ON userod.ProvNum=provider.ProvNum "//there can be multiple userods attached to one provider
				+"LEFT JOIN (SELECT PriProv, COUNT(*) PatCountPri FROM patient "
					+"WHERE patient.PatStatus!="+POut.Int((int)PatientStatus.Deleted)+" AND patient.PatStatus!="+POut.Int((int)PatientStatus.Deceased)+" "
					+"GROUP BY PriProv) pat ON provider.ProvNum=pat.PriProv ";
			command+="LEFT JOIN (SELECT SecProv,COUNT(*) PatCountSec FROM patient "
				+"WHERE patient.PatStatus!="+POut.Int((int)PatientStatus.Deleted)+" AND patient.PatStatus!="+POut.Int((int)PatientStatus.Deceased)+" "
				+"GROUP BY SecProv) patSec ON provider.ProvNum=patSec.SecProv ";
			command+="WHERE TRUE ";//This is here so that we can prevent nested if-statements
			if(schoolClassNum>0) {
				command+="AND provider.SchoolClassNum="+POut.Long(schoolClassNum)+" ";
			}
			if(lastName!="") {
				command+="AND provider.LName LIKE '%"+POut.String(lastName)+"%' ";
			}
			if(firstName!="") {
				command+="AND provider.FName LIKE '%"+POut.String(firstName)+"%' ";
			}
			if(provNum!="") {
				command+="AND provider.ProvNum LIKE '%"+POut.String(provNum)+"%' ";
			}
			if(!selectAll) {
				command+="AND provider.IsInstructor="+POut.Bool(selectInstructors)+" ";
				if(!selectInstructors) {
					command+="AND provider.SchoolClassNum!=0 ";
				}
			}
			command+="GROUP BY Abbr,LName,FName,provider.IsHidden,provider.ItemOrder,provider.ProvNum,GradYear,IsInstructor,Descript,PatCountPri,PatCountSec "
				+"ORDER BY LName,FName";
			return Db.GetTable(command);
		}

		///<summary>Gets list of all instructors.  Returns an empty list if none are found.</summary>
		public static List<Provider> GetInstructors() {
			//No need to check RemotingRole; no call to db.
			List<Provider> listInstructors=new List<Provider>();
			List<Provider> listProvs=ProviderC.GetListLong();
			for(int i=0;i<listProvs.Count;i++) {
				if(listProvs[i].IsInstructor) {
					listInstructors.Add(listProvs[i]);
				}
			}
			return listInstructors;
		}

		public static List<Provider> GetChangedSince(DateTime changedSince) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Provider>>(MethodBase.GetCurrentMethod(),changedSince);
			}
			string command="SELECT * FROM provider WHERE DateTStamp > "+POut.DateT(changedSince);
			//DataTable table=Db.GetTable(command);
			//return TableToList(table);
			return Crud.ProviderCrud.SelectMany(command);
		}

		///<summary></summary>
		public static string GetAbbr(long provNum) {
			//No need to check RemotingRole; no call to db.
			List<Provider> listProvs=ProviderC.GetListLong();
			for(int i=0;i<listProvs.Count;i++) {
				if(listProvs[i].ProvNum==provNum) {
					return listProvs[i].Abbr;
				}
			}
			return "";
		}

	  ///<summary>Used in the HouseCalls bridge</summary>
		public static string GetLName(long provNum){
			//No need to check RemotingRole; no call to db.
			string retStr="";
			List<Provider> listProvs=ProviderC.GetListLong();
			for(int i=0;i<listProvs.Count;i++){
				if(listProvs[i].ProvNum==provNum){
					retStr=listProvs[i].LName;
				}
			}
			return retStr;
		}

		///<summary>First Last, Suffix</summary>
		public static string GetFormalName(long provNum) {
			//No need to check RemotingRole; no call to db.
			string retStr="";
			List<Provider> listProvs=ProviderC.GetListLong();
			for(int i=0;i<listProvs.Count;i++){
				if(listProvs[i].ProvNum==provNum){
					retStr=listProvs[i].FName+" "
						+listProvs[i].LName;
					if(listProvs[i].Suffix != ""){
						retStr+=", "+listProvs[i].Suffix;
					}
				}
			}
			return retStr;
		}
		
		///<summary>Abbr - LName, FName (hidden).  For dental schools -- ProvNum - LName, FName (hidden).</summary>
		public static string GetLongDesc(long provNum) {
			//No need to check RemotingRole; no call to db.
			List<Provider> listProvs=ProviderC.GetListLong();
			for(int i=0;i<listProvs.Count;i++) {
				if(listProvs[i].ProvNum==provNum) {
					return listProvs[i].GetLongDesc();
				}
			}
			return "";
		}

		///<summary></summary>
		public static Color GetColor(long provNum) {
			//No need to check RemotingRole; no call to db.
			Color retCol=Color.White;
			List<Provider> listProvs=ProviderC.GetListLong();
			for(int i=0;i<listProvs.Count;i++){
				if(listProvs[i].ProvNum==provNum){
					retCol=listProvs[i].ProvColor;
				}
			}
			return retCol;
		}

		///<summary></summary>
		public static Color GetOutlineColor(long provNum){
			//No need to check RemotingRole; no call to db.
			Color retCol=Color.Black;
			List<Provider> listProvs=ProviderC.GetListLong();
			for(int i=0;i<listProvs.Count;i++){
				if(listProvs[i].ProvNum==provNum){
					retCol=listProvs[i].OutlineColor;
				}
			}
			return retCol;
		}

		///<summary></summary>
		public static bool GetIsSec(long provNum){
			//No need to check RemotingRole; no call to db.
			bool retVal=false;
			List<Provider> listProvs=ProviderC.GetListLong();
			for(int i=0;i<listProvs.Count;i++){
				if(listProvs[i].ProvNum==provNum){
					retVal=listProvs[i].IsSecondary;
				}
			}
			return retVal;
		}

		///<summary>Gets a provider from ListLong.  If provnum is not valid, then it returns null.</summary>
		public static Provider GetProv(long provNum) {
			//No need to check RemotingRole; no call to db.
			if(provNum==0){
				return null;
			}
			List<Provider> listProvs=ProviderC.GetListLong();
			for(int i=0;i<listProvs.Count;i++) {
				if(listProvs[i].ProvNum==provNum) {
					return listProvs[i].Copy();
				}
			}
			return null;
		}

		///<summary>Gets a provider from a given list.  If provnum is not valid it returns null.</summary>
		public static Provider GetProv(long provNum,List<Provider> listProvs) {
			//No need to check RemotingRole; no call to db.
			if(provNum==0) {
				return null;
			}
			for(int i=0;i<listProvs.Count;i++) {
				if(listProvs[i].ProvNum==provNum) {
					return listProvs[i].Copy();
				}
			}
			return null;
		}

		///<summary>Gets a list of providers from ListLong.  If none found or if either LName or FName are an empty string, returns an empty list.  There may be more than on provider with the same FName and LName so we will return a list of all such providers.  Usually only one will exist with the FName and LName provided so list returned will have count 0 or 1 normally.  Name match is not case sensitive.</summary>
		public static List<Provider> GetProvsByFLName(string lName,string fName) {
			//No need to check RemotingRole; no call to db.
			List<Provider> retval=new List<Provider>();
			if(lName=="" || lName==null || fName=="" || fName==null) {
				return retval;
			}
			List<Provider> listProvs=ProviderC.GetListLong();
			for(int i=0;i<listProvs.Count;i++) {
				if(listProvs[i].LName.ToLower()==lName.ToLower() && listProvs[i].FName.ToLower()==fName.ToLower()) {
					retval.Add(listProvs[i].Copy());
				}
			}
			return retval;
		}

		///<summary>Gets a list of providers from ListLong with either the NPI provided or a blank NPI and the Medicaid ID provided.
		///medicaidId can be blank.  If the npi param is blank, or there are no matching provs, returns an empty list.
		///Shouldn't be two separate functions or we would have to compare the results of the two lists.</summary>
		public static List<Provider> GetProvsByNpiOrMedicaidId(string npi,string medicaidId) {
			//No need to check RemotingRole; no call to db.
			List<Provider> retval=new List<Provider>();
			if(npi=="") {
				return retval;
			}
			List<Provider> listProvs=ProviderC.GetListLong();
			for(int i=0;i<listProvs.Count;i++) {
				//if the prov has a NPI set and it's a match, add this prov to the list
				if(listProvs[i].NationalProvID!="") {
					if(listProvs[i].NationalProvID.Trim().ToLower()==npi.Trim().ToLower()) {
						retval.Add(listProvs[i].Copy());
					}
				}
				else {//if the NPI is blank and the Medicaid ID is set and it's a match, add this prov to the list
					if(listProvs[i].MedicaidID!=""
						&& listProvs[i].MedicaidID.Trim().ToLower()==medicaidId.Trim().ToLower())
					{
						retval.Add(listProvs[i].Copy());
					}
				}
			}
			return retval;
		}

		///<summary>Gets all providers associated to users that have a clinic set to the clinic passed in.  Passing in 0 will get a list of providers not assigned to any clinic or to any users.</summary>
		public static List<Provider> GetProvsByClinic(long clinicNum) {
			//No need to check RemotingRole; no call to db.
			List<Provider> listProvsWithClinics=new List<Provider>();
			List<Userod> listUsersShort=UserodC.GetListShort();
			for(int i=0;i<listUsersShort.Count;i++) {
				Provider prov=Providers.GetProv(listUsersShort[i].ProvNum);
				if(prov==null) {
					continue;
				}
				List<UserClinic> listUserClinics=UserClinics.GetForUser(listUsersShort[i].UserNum);
				//If filtering by a specific clinic, make sure the clinic matches the clinic passed in.
				//If the user is associated to multiple clinics we check to make sure one of them isn't the clinic in question.
				if(clinicNum > 0 && !listUserClinics.Exists(x => x.ClinicNum==clinicNum)) {
					continue;
				}
				if(listUsersShort[i].ClinicNum > 0) {//User is associated to a clinic, add the provider to the list of provs with clinics.
					listProvsWithClinics.Add(prov);
				}
			}
			if(clinicNum==0) {//Return the list of providers without clinics.
				//We need to find all providers not associated to a clinic (via userod) and also include all providers not even associated to a user.
				//Since listProvsWithClinics is comprised of all providers associated to a clinic, simply loop through the provider cache and remove providers present in listProvsWithClinics.
				List<Provider> listProvsUnassigned=ProviderC.GetListShort();
				for(int i=listProvsUnassigned.Count-1;i>=0;i--) {
					for(int j=0;j<listProvsWithClinics.Count;j++) {
						if(listProvsWithClinics[j].ProvNum==listProvsUnassigned[i].ProvNum) {
							listProvsUnassigned.RemoveAt(i);
							break;
						}
					}
				}
				return listProvsUnassigned;
			}
			else {
				return listProvsWithClinics;
			}
		}

		///<summary>Gets all providers from the database.  Doesn't use the cache.</summary>
		public static List<Provider> GetProvsNoCache() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Provider>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM provider";
			return Crud.ProviderCrud.SelectMany(command);

		}

		///<summary>Gets a provider from the List.  If EcwID is not found, then it returns null.</summary>
		public static Provider GetProvByEcwID(string eID) {
			//No need to check RemotingRole; no call to db.
			if(eID=="") {
				return null;
			}
			List<Provider> listProvs=ProviderC.GetListLong();
			for(int i=0;i<listProvs.Count;i++) {
				if(listProvs[i].EcwID==eID) {
					return listProvs[i].Copy();
				}
			}
			//If using eCW, a provider might have been added from the business layer.
			//The UI layer won't know about the addition.
			//So we need to refresh if we can't initially find the prov.
			RefreshCache();
			listProvs=ProviderC.GetListLong();
			for(int i=0;i<listProvs.Count;i++) {
				if(listProvs[i].EcwID==eID) {
					return listProvs[i].Copy();
				}
			}
			return null;
		}

		/// <summary>Takes a provNum. Normally returns that provNum. If in Orion mode, returns the user's ProvNum, if that user is a primary provider. Otherwise, in Orion Mode, returns 0.</summary>
		public static long GetOrionProvNum(long provNum) {
			if(Programs.UsingOrion){
				Userod user=Security.CurUser;
				if(user!=null){
					Provider prov=Providers.GetProv(user.ProvNum);
					if(prov!=null){
						if(!prov.IsSecondary){
							return user.ProvNum;
						}
					}
				}
				return 0;
			}
			return provNum;
		}

		///<summary></summary>
		public static int GetIndexLong(long provNum) {
			//No need to check RemotingRole; no call to db.
			return GetIndexLong(provNum,ProviderC.GetListLong());
		}

		///<summary></summary>
		public static int GetIndexLong(long provNum,List<Provider> listProvs) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<listProvs.Count;i++) {
				if(listProvs[i].ProvNum==provNum) {
					return i;
				}
			}
			return 0;//should NEVER happen, but just in case, the 0 won't crash
		}

		///<summary>Within the regular list of visible providers.  Will return -1 if the specified provider is not in the list.</summary>
		public static int GetIndex(long provNum) {
			//No need to check RemotingRole; no call to db.
			List<Provider> listProvs=ProviderC.GetListShort();
			for(int i=0;i<listProvs.Count;i++){
				if(listProvs[i].ProvNum==provNum){
					return i;
				}
			}
			return -1;
		}

		public static List<Userod> GetAttachedUsers(long provNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Userod>>(MethodBase.GetCurrentMethod(),provNum);
			}
			string command="SELECT userod.* FROM userod,provider "
					+"WHERE userod.ProvNum=provider.ProvNum "
					+"AND provider.provNum="+POut.Long(provNum);
			return Crud.UserodCrud.SelectMany(command);
		}

		///<summary>If useClinic, then clinicInsBillingProv will be used.  Otherwise, the pref for the practice.  Either way, there are three different choices for getting the billing provider.  One of the three is to use the treating provider, so supply that as an argument.  It will return a valid provNum unless the supplied treatProv was invalid.</summary>
		public static long GetBillingProvNum(long treatProv,long clinicNum) {//,bool useClinic,int clinicInsBillingProv){
			//No need to check RemotingRole; no call to db.
			long clinicInsBillingProv=0;
			bool useClinic=false;
			if(clinicNum>0) {
				useClinic=true;
				clinicInsBillingProv=Clinics.GetClinic(clinicNum).InsBillingProv;
			}
			if(useClinic){
				if(clinicInsBillingProv==0) {//default=0
					return PrefC.GetLong(PrefName.PracticeDefaultProv);
				}
				else if(clinicInsBillingProv==-1) {//treat=-1
					return treatProv;
				}
				else {
					return clinicInsBillingProv;
				}
			}
			else{
				if(PrefC.GetLong(PrefName.InsBillingProv)==0) {//default=0
					return PrefC.GetLong(PrefName.PracticeDefaultProv);
				}
				else if(PrefC.GetLong(PrefName.InsBillingProv)==-1) {//treat=-1
					return treatProv;
				}
				else {
					return PrefC.GetLong(PrefName.InsBillingProv);
				}
			}
		}

		/*
		///<summary>Used when adding a provider to get the next available itemOrder.</summary>
		public static int GetNextItemOrder(){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod());
			}
			//Is this valid in Oracle??
			string command="SELECT MAX(ItemOrder) FROM provider";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0){
				return 0;
			}
			return PIn.Int(table.Rows[0][0].ToString())+1;
		}*/

		///<Summary>Used once in the Provider Select window to warn user of duplicate Abbrs.</Summary>
		public static string GetDuplicateAbbrs(){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod());
			}
			string command="SELECT Abbr FROM provider p1 WHERE EXISTS"
				+"(SELECT * FROM provider p2 WHERE p1.ProvNum!=p2.ProvNum AND p1.Abbr=p2.Abbr) GROUP BY Abbr";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0) {
				return "";
			}
			string retVal="";
			for(int i=0;i<table.Rows.Count;i++){
				if(i>0){
					retVal+=",";
				}
				retVal+=table.Rows[i][0].ToString();
			}
			return retVal;
		}

		///<summary>Returns the default practice provider</summary>
		public static Provider GetDefaultProvider() {
			//No need to check RemotingRole; no call to db.
			return GetDefaultProvider(0);
		}

		///<summary>Returns the default provider for the clinic if it exists, else returns the default practice provider.  Pass 0 to get practice default.  Can return null if no clinic or practice default provider found.</summary>
		public static Provider GetDefaultProvider(long clinicNum) {
			//No need to check RemotingRole; no call to db.
			Clinic clinic=Clinics.GetClinic(clinicNum);
			Provider provider=null;
			if(clinic!=null && clinic.DefaultProv!=0) {//the clinic exists
				provider=Providers.GetProv(clinic.DefaultProv);
			}
			if(provider==null) {//If not using clinics or if the specified clinic does not have a valid default provider set.
				provider=Providers.GetProv(PrefC.GetLong(PrefName.PracticeDefaultProv));//Try to get the practice default.
			}
			return provider;
		}

		public static DataTable GetDefaultPracticeProvider(){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod());
			}
			string command=@"SELECT FName,LName,Suffix,StateLicense
				FROM provider
        WHERE provnum="+PrefC.GetString(PrefName.PracticeDefaultProv);
			return Db.GetTable(command);
		}

		///<summary>We should merge these results with GetDefaultPracticeProvider(), but
		///that would require restructuring indexes in different places in the code and this is
		///faster to do as we are just moving the queries down in to the business layer for now.</summary>
		public static DataTable GetDefaultPracticeProvider2() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod());
			}
			string command=@"SELECT FName,LName,Specialty "+
				"FROM provider WHERE provnum="+
				POut.Long(PrefC.GetLong(PrefName.PracticeDefaultProv));
				//Convert.ToInt32(((Pref)PrefC.HList["PracticeDefaultProv"]).ValueString);
			return Db.GetTable(command);
		}

		///<summary>We should merge these results with GetDefaultPracticeProvider(), but
		///that would require restructuring indexes in different places in the code and this is
		///faster to do as we are just moving the queries down in to the business layer for now.</summary>
		public static DataTable GetDefaultPracticeProvider3() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod());
			}
			string command=@"SELECT NationalProvID "+
				"FROM provider WHERE provnum="+
				POut.Long(PrefC.GetLong(PrefName.PracticeDefaultProv));
			return Db.GetTable(command);
		}

		public static DataTable GetPrimaryProviders(long PatNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),PatNum);
			}
			string command=@"SELECT Fname,Lname from provider
                        WHERE provnum in (select priprov from 
                        patient where patnum = "+PatNum+")";
			return Db.GetTable(command);
		}

		///<summary>Returns the patient's last seen hygienist.  Returns null if no hygienist has been seen.</summary>
		public static Provider GetLastSeenHygienistForPat(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Provider>(MethodBase.GetCurrentMethod(),patNum);
			}
			//Look at all completed appointments and get the most recent secondary provider on it.
			string command=@"SELECT appointment.ProvHyg
				FROM appointment
				WHERE appointment.PatNum="+POut.Long(patNum)+@"
				AND appointment.ProvHyg!=0
				AND appointment.AptStatus="+POut.Int((int)ApptStatus.Complete)+@"
				ORDER BY AptDateTime DESC";
			List<long> listPatHygNums=Db.GetListLong(command);
			//Now that we have all hygienists for this patient.  Lets find the last non-hidden hygienist and return that one.
			List<Provider> listProviders=ProviderC.GetListShort();
			List<long> listProvNums=listProviders.Select(x => x.ProvNum).Distinct().ToList();
			long lastHygNum=listPatHygNums.FirstOrDefault(x => listProvNums.Contains(x));
			return listProviders.FirstOrDefault(x => x.ProvNum==lastHygNum);
		}

		///<summary>Gets a list of providers based for the patient passed in based on the WebSchedProviderRule preference.</summary>
		public static List<Provider> GetProvidersForWebSched(long patNum) {
			//No need to check RemotingRole; no call to db.
			List<Provider> listProviders=ProviderC.GetListShort();
			WebSchedProviderRules providerRule=(WebSchedProviderRules)PrefC.GetInt(PrefName.WebSchedProviderRule);
			switch(providerRule) {
				case WebSchedProviderRules.PrimaryProvider:
					Patient patPri=Patients.GetPat(patNum);
					Provider patPriProv=listProviders.Find(x => x.ProvNum==patPri.PriProv);
					if(patPriProv==null) {
						return listProviders;//Use all providers (default behavior)
					}
					return new List<Provider>() { patPriProv };
				case WebSchedProviderRules.SecondaryProvider:
					Patient patSec=Patients.GetPat(patNum);
					Provider patSecProv=listProviders.Find(x => x.ProvNum==patSec.SecProv);
					if(patSecProv==null) {
						return listProviders;//Use all providers (default behavior)
					}
					return new List<Provider>() { patSecProv };
				case WebSchedProviderRules.LastSeenHygienist:
					Provider lastHygProvider=GetLastSeenHygienistForPat(patNum);
					if(lastHygProvider==null) {
						return listProviders;//Use all providers (default behavior)
					}
					return new List<Provider>() { lastHygProvider };
				case WebSchedProviderRules.FirstAvailable:
				default:
					return listProviders;
			}
		}

		public static List<long> GetChangedSinceProvNums(DateTime changedSince) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),changedSince);
			}
			string command="SELECT ProvNum FROM provider WHERE DateTStamp > "+POut.DateT(changedSince);
			DataTable dt=Db.GetTable(command);
			List<long> provnums = new List<long>(dt.Rows.Count);
			for(int i=0;i<dt.Rows.Count;i++) {
				provnums.Add(PIn.Long(dt.Rows[i]["ProvNum"].ToString()));
			}
			return provnums;
		}

		///<summary>Used along with GetChangedSinceProvNums</summary>
		public static List<Provider> GetMultProviders(List<long> provNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Provider>>(MethodBase.GetCurrentMethod(),provNums);
			}
			string strProvNums="";
			DataTable table;
			if(provNums.Count>0) {
				for(int i=0;i<provNums.Count;i++) {
					if(i>0) {
						strProvNums+="OR ";
					}
					strProvNums+="ProvNum='"+provNums[i].ToString()+"' ";
				}
				string command="SELECT * FROM provider WHERE "+strProvNums;
				table=Db.GetTable(command);
			}
			else {
				table=new DataTable();
			}
			Provider[] multProviders=Crud.ProviderCrud.TableToList(table).ToArray();
			List<Provider> providerList=new List<Provider>(multProviders);
			return providerList;
		}

		/// <summary>Currently only used for Dental Schools and will only return ProviderC.ListShort if Dental Schools is not active.  Otherwise this will return a filtered provider list.</summary>
		public static List<Provider> GetFilteredProviderList(long provNum,string lName,string fName,long classNum) {
			//No need to check RemotingRole; no call to db.
			List<Provider> listProvs=ProviderC.GetListShort();
			if(PrefC.GetBool(PrefName.EasyHideDentalSchools)) {//This is here to save doing the logic below for users who have no way to filter the provider picker list.
				return listProvs;
			}
			for(int i=listProvs.Count-1;i>=0;i--) {
				if(provNum!=0 && !listProvs[i].ProvNum.ToString().Contains(provNum.ToString())) {
					listProvs.Remove(listProvs[i]);
					continue;
				}
				if(!String.IsNullOrWhiteSpace(lName) && !listProvs[i].LName.Contains(lName)) {
					listProvs.Remove(listProvs[i]);
					continue;
				}
				if(!String.IsNullOrWhiteSpace(fName) && !listProvs[i].FName.Contains(fName)) {
					listProvs.Remove(listProvs[i]);
					continue;
				}
				if(classNum!=0 && classNum!=listProvs[i].SchoolClassNum) {
					listProvs.Remove(listProvs[i]);
					continue;
				}
			}
			return listProvs;
		}

		///<summary>Removes a provider from the future schedule.  Currently called after a provider is hidden.</summary>
		public static void RemoveProvFromFutureSchedule(long provNum) {
			//No need to check RemotingRole; no call to db.
			if(provNum<1) {//Invalid provNum, nothing to do.
				return;
			}
			List<long> provNums=new List<long>();
			provNums.Add(provNum);
			RemoveProvsFromFutureSchedule(provNums);
		}

		///<summary>Removes the providers from the future schedule.  Currently called from DBM to clean up hidden providers still on the schedule.</summary>
		public static void RemoveProvsFromFutureSchedule(List<long> provNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),provNums);
				return;
			}
			string provs="";
			for(int i=0;i<provNums.Count;i++) {
				if(provNums[i]<1) {//Invalid provNum, nothing to do.
					continue;
				}
				if(i>0) {
					provs+=",";
				}
				provs+=provNums[i].ToString();
			}
			if(provs=="") {//No valid provNums were passed in.  Simply return.
				return;
			}
			string command="SELECT ScheduleNum FROM schedule WHERE ProvNum IN ("+provs+") AND SchedDate > "+DbHelper.Now();
			DataTable table=Db.GetTable(command);
			List<string> listScheduleNums=new List<string>();//Used for deleting scheduleops below
			for(int i=0;i<table.Rows.Count;i++) {
				//Add entry to deletedobjects table if it is a provider schedule type
				DeletedObjects.SetDeleted(DeletedObjectType.ScheduleProv,PIn.Long(table.Rows[i]["ScheduleNum"].ToString()));				
				listScheduleNums.Add(table.Rows[i]["ScheduleNum"].ToString());
			}
			if(listScheduleNums.Count!=0) {
				command="DELETE FROM scheduleop WHERE ScheduleNum IN("+POut.String(String.Join(",",listScheduleNums))+")";
				Db.NonQ(command);
			}
			command="DELETE FROM schedule WHERE ProvNum IN ("+provs+") AND SchedDate > "+DbHelper.Now();
			Db.NonQ(command);
		}

		public static bool IsAttachedToUser(long provNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),provNum);
			}
			string command="SELECT COUNT(*) FROM userod,provider "
					+"WHERE userod.ProvNum=provider.ProvNum "
					+"AND provider.provNum="+POut.Long(provNum);
			int count=PIn.Int(Db.GetCount(command));
			if(count>0) {
				return true;
			}
			return false;
		}

		///<summary>Used to check if a specialty is in use when user is trying to hide it.</summary>
		public static bool IsSpecialtyInUse(long defNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),defNum);
			}
			string command="SELECT COUNT(*) FROM provider WHERE Specialty="+POut.Long(defNum);
			if(Db.GetCount(command)=="0") {
				return false;
			}
			return true;
		}

		///<summary>Provider merge tool.  Returns the number of rows changed when the tool is used.</summary>
		public static long Merge(long provNumFrom, long provNumInto) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),provNumFrom,provNumInto);
			}
			string[] provNumForeignKeys=new string[] { //add any new FKs to this list.
				"adjustment.ProvNum",
				"anestheticrecord.ProvNum",
				"appointment.ProvNum",
				"appointment.ProvHyg",
				"apptviewitem.ProvNum",
				"claim.ProvTreat",
				"claim.ProvBill",
				"claim.ReferringProv",
				"claim.ProvOrderOverride",
				"claimproc.ProvNum",
				"clinic.DefaultProv",
				"clinic.InsBillingProv",
				"dispsupply.ProvNum",
				"ehrnotperformed.ProvNum",
				"emailmessage.ProvNumWebMail",
				"encounter.ProvNum",
				"equipment.ProvNumCheckedOut",
				"erxlog.ProvNum",
				"evaluation.InstructNum",
				"evaluation.StudentNum",
				"fee.ProvNum",
				"intervention.ProvNum",
        "labcase.ProvNum",
				"medicalorder.ProvNum",
				"medicationpat.ProvNum",
				"operatory.ProvDentist",
				"operatory.ProvHygienist",
				"patient.PriProv",
				"patient.SecProv",
				"payplancharge.ProvNum",
        "paysplit.ProvNum",
				"perioexam.ProvNum",
				"proccodenote.ProvNum",
				"procedurecode.ProvNumDefault",
        "procedurelog.ProvNum",
				"procedurelog.ProvOrderOverride",
				"provider.ProvNumBillingOverride",
				"providerident.ProvNum",
				"refattach.ProvNum",
				"reqstudent.ProvNum",
				"reqstudent.InstructorNum",
				"rxpat.ProvNum",
				"schedule.ProvNum",
				"userod.ProvNum",
				"vaccinepat.ProvNumAdminister",
				"vaccinepat.ProvNumOrdering"
			};
			string command="";
			long retVal=0;
			for(int i=0;i<provNumForeignKeys.Length;i++) { //actually change all of the FKs in the above tables.
				string[] tableAndKeyName=provNumForeignKeys[i].Split(new char[] { '.' });
				command="UPDATE "+tableAndKeyName[0]
					+" SET "+tableAndKeyName[1]+"="+POut.Long(provNumInto)
					+" WHERE "+tableAndKeyName[1]+"="+POut.Long(provNumFrom);
				retVal+=Db.NonQ(command);
			}
			command="UPDATE provider SET IsHidden=1 WHERE ProvNum="+POut.Long(provNumFrom);
			Db.NonQ(command);
			command="UPDATE provider SET ProvStatus="+POut.Int((int)ProviderStatus.Deleted)+" WHERE ProvNum="+POut.Long(provNumFrom);
			Db.NonQ(command);
			return retVal;
		}

		public static long CountPats(long provNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),provNum);
			}
			string command="SELECT COUNT(DISTINCT patient.PatNum) FROM patient WHERE (patient.PriProv="+POut.Long(provNum)
				+" OR patient.SecProv="+POut.Long(provNum)+")"
				+" AND patient.PatStatus=0";
			string retVal=Db.GetScalar(command);
			return PIn.Long(retVal);
		}

		public static long CountClaims(long provNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),provNum);
			}
			string command="SELECT COUNT(DISTINCT claim.ClaimNum) FROM claim WHERE claim.ProvBill="+POut.Long(provNum)
				+" OR claim.ProvTreat="+POut.Long(provNum);
			string retVal=Db.GetScalar(command);
			return PIn.Long(retVal);
		}

	}
	
	

}










