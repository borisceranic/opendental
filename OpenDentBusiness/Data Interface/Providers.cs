 using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

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
			ProviderC.ListLong=Crud.ProviderCrud.TableToList(table);
			List<Provider> listShort=new List<Provider>();
			for(int i=0;i<ProviderC.ListLong.Count;i++){
				if(!ProviderC.ListLong[i].IsHidden){
					listShort.Add(ProviderC.ListLong[i]);	
				}
			}
			ProviderC.ListShort=listShort;
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
				provider.ProvNum=Meth.GetLong(MethodBase.GetCurrentMethod(),provider);
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
			string command="SELECT Abbr,LName,FName,provider.IsHidden,provider.ItemOrder,provider.ProvNum,MAX(UserName) UserName, PatCount "//Max function used for Oracle compatability (some providers may have multiple user names).
				+"FROM provider "
				+"LEFT JOIN userod ON userod.ProvNum=provider.ProvNum "//there can be multiple userods attached to one provider
				+"LEFT JOIN (SELECT PriProv, COUNT(*) PatCount FROM patient "
					+"WHERE patient.PatStatus!="+POut.Int((int)PatientStatus.Deleted)+" AND patient.PatStatus!="+POut.Int((int)PatientStatus.Deceased)+" "
					+"GROUP BY PriProv) pat ON provider.ProvNum=pat.PriProv  ";
			command+="GROUP BY Abbr,LName,FName,provider.IsHidden,provider.ItemOrder,provider.ProvNum,PatCount ";
			command+="ORDER BY ItemOrder";
			return Db.GetTable(command);
		}

		///<summary>Gets table for main provider edit list when in dental school mode.  Always orders alphabetically, but there will be lots of filters to get the list shorter.  Must be very fast because refreshes while typing.  selectAll will trump selectInstructors and always return all providers.</summary>
		public static DataTable RefreshForDentalSchool(long schoolClassNum,string lastName,string firstName,string provNum,bool selectInstructors,bool selectAll) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),schoolClassNum,lastName,selectInstructors,selectAll);
			}
			string command="SELECT Abbr,LName,FName,provider.IsHidden,provider.ItemOrder,provider.ProvNum,GradYear,IsInstructor,Descript,MAX(UserName) UserName, PatCount "//Max function used for Oracle compatability (some providers may have multiple user names).
				+"FROM provider LEFT JOIN schoolclass ON provider.SchoolClassNum=schoolclass.SchoolClassNum "
				+"LEFT JOIN userod ON userod.ProvNum=provider.ProvNum "//there can be multiple userods attached to one provider
				+"LEFT JOIN (SELECT PriProv, COUNT(*) PatCount FROM patient "
					+"WHERE patient.PatStatus!="+POut.Int((int)PatientStatus.Deleted)+" AND patient.PatStatus!="+POut.Int((int)PatientStatus.Deceased)+" "
					+"GROUP BY PriProv) pat ON provider.ProvNum=pat.PriProv  "
					+"WHERE TRUE ";//This is here so that we can prevent nested if-statements
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
			command+="GROUP BY Abbr,LName,FName,provider.IsHidden,provider.ItemOrder,provider.ProvNum,GradYear,IsInstructor,Descript,PatCount "
				+"ORDER BY LName,FName";
			return Db.GetTable(command);
		}

		///<summary>Gets list of all instructors.  Returns an empty list if none are found.</summary>
		public static List<Provider> GetInstructors() {
			//No need to check RemotingRole; no call to db.
			List<Provider> provs=new List<Provider>();
			if(ProviderC.ListLong==null) {
				RefreshCache();
			}
			for(int i=0;i<ProviderC.ListLong.Count;i++) {
				if(ProviderC.ListLong[i].IsInstructor) {
					provs.Add(ProviderC.ListLong[i]);
				}
			}
			return provs;
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
		public static string GetAbbr(long provNum){
			//No need to check RemotingRole; no call to db.
			if(ProviderC.ListLong==null){
				RefreshCache();
			}
			for(int i=0;i<ProviderC.ListLong.Count;i++){
				if(ProviderC.ListLong[i].ProvNum==provNum){
					return ProviderC.ListLong[i].Abbr;
				}
			}
			return "";
		}

		///<summary>Used in the HouseCalls bridge</summary>
		public static string GetLName(long provNum){
			//No need to check RemotingRole; no call to db.
			string retStr="";
			for(int i=0;i<ProviderC.ListLong.Count;i++){
				if(ProviderC.ListLong[i].ProvNum==provNum){
					retStr=ProviderC.ListLong[i].LName;
				}
			}
			return retStr;
		}

		///<summary>First Last, Suffix</summary>
		public static string GetFormalName(long provNum){
			//No need to check RemotingRole; no call to db.
			string retStr="";
			for(int i=0;i<ProviderC.ListLong.Count;i++){
				if(ProviderC.ListLong[i].ProvNum==provNum){
					retStr=ProviderC.ListLong[i].FName+" "
						+ProviderC.ListLong[i].LName;
					if(ProviderC.ListLong[i].Suffix != ""){
						retStr+=", "+ProviderC.ListLong[i].Suffix;
					}
				}
			}
			return retStr;
		}

		///<summary>Abbr - LName, FName (hidden).  For dental schools -- ProvNum - LName, FName (hidden).</summary>
		public static string GetLongDesc(long provNum) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<ProviderC.ListLong.Count;i++) {
				if(ProviderC.ListLong[i].ProvNum==provNum) {
					return ProviderC.ListLong[i].GetLongDesc();
				}
			}
			return "";
		}

		///<summary></summary>
		public static Color GetColor(long provNum) {
			//No need to check RemotingRole; no call to db.
			Color retCol=Color.White;
			for(int i=0;i<ProviderC.ListLong.Count;i++){
				if(ProviderC.ListLong[i].ProvNum==provNum){
					retCol=ProviderC.ListLong[i].ProvColor;
				}
			}
			return retCol;
		}

		///<summary></summary>
		public static Color GetOutlineColor(long provNum){
			//No need to check RemotingRole; no call to db.
			Color retCol=Color.Black;
			for(int i=0;i<ProviderC.ListLong.Count;i++){
				if(ProviderC.ListLong[i].ProvNum==provNum){
					retCol=ProviderC.ListLong[i].OutlineColor;
				}
			}
			return retCol;
		}

		///<summary></summary>
		public static bool GetIsSec(long provNum){
			//No need to check RemotingRole; no call to db.
			bool retVal=false;
			for(int i=0;i<ProviderC.ListLong.Count;i++){
				if(ProviderC.ListLong[i].ProvNum==provNum){
					retVal=ProviderC.ListLong[i].IsSecondary;
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
			if(ProviderC.ListLong==null) {
				RefreshCache();
			}
			for(int i=0;i<ProviderC.ListLong.Count;i++) {
				if(ProviderC.ListLong[i].ProvNum==provNum) {
					return ProviderC.ListLong[i].Copy();
				}
			}
			return null;
		}

		///<summary>Gets a provider from the List.  If EcwID is not found, then it returns null.</summary>
		public static Provider GetProvByEcwID(string eID) {
			//No need to check RemotingRole; no call to db.
			if(eID=="") {
				return null;
			}
			if(ProviderC.ListLong==null) {
				RefreshCache();
			}
			for(int i=0;i<ProviderC.ListLong.Count;i++) {
				if(ProviderC.ListLong[i].EcwID==eID) {
					return ProviderC.ListLong[i].Copy();
				}
			}
			//If using eCW, a provider might have been added from the business layer.
			//The UI layer won't know about the addition.
			//So we need to refresh if we can't initially find the prov.
			RefreshCache();
			for(int i=0;i<ProviderC.ListLong.Count;i++) {
				if(ProviderC.ListLong[i].EcwID==eID) {
					return ProviderC.ListLong[i].Copy();
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
			for(int i=0;i<ProviderC.ListLong.Count;i++){
				if(ProviderC.ListLong[i].ProvNum==provNum){
					return i;
				}
			}
			return 0;//should NEVER happen, but just in case, the 0 won't crash
		}

		///<summary>Within the regular list of visible providers.  Will return -1 if the specified provider is not in the list.</summary>
		public static int GetIndex(long provNum) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<ProviderC.ListShort.Count;i++){
				if(ProviderC.ListShort[i].ProvNum==provNum){
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
			if(PrefC.GetBool(PrefName.EasyHideDentalSchools)) {//This is here to save doing the logic below for users who have no way to filter the provider picker list.
				return ProviderC.ListShort;
			}
			List<Provider> listProvs=new List<Provider>(ProviderC.ListShort);
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
			string command="DELETE FROM schedule WHERE ProvNum IN ("+provs+") AND SchedDate > "+DbHelper.Now();
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


	}
	
	

}










