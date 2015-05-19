using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{

	///<summary></summary>
	public class Medications{
		///<summary>All medications.  Not refreshed with local data.  Only refreshed as needed.</summary>
		public static Medication[] _listt;
		///<summary></summary>
		private static Hashtable _hList;
		private static object _lockObj=new object();
		
		///<summary>All medications.  Not refreshed with local data.  Only refreshed as needed.</summary>
		public static Medication[] Listt {
			get {
				return GetListt();
			}
			set {
				lock(_lockObj) {
					_listt=value;
				}
			}
		}

		public static Hashtable HList {
			get {
				return GetHList();
			}
			set {
				lock(_lockObj) {
					_hList=value;
				}
			}
		}
		
		///<summary>All medications.  Not refreshed with local data.  Only refreshed as needed.</summary>
		public static Medication[] GetListt() {
			bool isListNull=false;
			lock(_lockObj) {
				if(_listt==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				Refresh();
			}
			Medication[] arrayMedications;
			lock(_lockObj) {
				arrayMedications=new Medication[_listt.Length];
				for(int i=0;i<_listt.Length;i++) {
					arrayMedications[i]=_listt[i].Copy();
				}
			}
			return arrayMedications;
		}

		public static Hashtable GetHList() {
			bool isListNull=false;
			lock(_lockObj) {
				if(_hList==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				Refresh();
			}
			Hashtable hashMedications=new Hashtable();
			lock(_lockObj) {
				foreach(DictionaryEntry entry in _hList) {
					hashMedications.Add(entry.Key,((Medication)entry.Value).Copy());
				}
			}
			return hashMedications;
		}

		///<summary>This must refresh Listt on client, not on server.</summary>
		public static void Refresh() {
			//No need to check RemotingRole; no call to db.
			List<Medication> list=GetListFromDb();
			Hashtable hashList=new Hashtable();
			for(int i=0;i<list.Count;i++) {
				hashList.Add(list[i].MedicationNum,list[i]);
			}
			HList=hashList;
			Listt=list.ToArray();
		}

		///<summary>Checks to see if the medication exists in the current cache.  If not, the local cache will get refreshed and then searched again.  If med is still not found, false is returned because the med does not exist.</summary>
		private static bool HasMedicationInCache(long medicationNum) {
			//Check if the medication exists in the cache.
			Hashtable hashMedications=GetHList();
			if(!hashMedications.ContainsKey(medicationNum)) {
				//Medication not found.  Refresh the cache and check again.
				Refresh();
				if(!hashMedications.ContainsKey(medicationNum)) {
					return false;//Medication does not exist in db.
				}
			}
			return true;
		}

		///<summary>Only public so that the remoting works.  Do not call this from anywhere except in this class.</summary>
		public static List<Medication> GetListFromDb() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Medication>>(MethodBase.GetCurrentMethod());
			}
			string command ="SELECT * FROM medication ORDER BY MedName";// WHERE MedName LIKE '%"+POut.String(str)+"%' ORDER BY MedName";
			return Crud.MedicationCrud.SelectMany(command);
		}

		public static List<Medication> TableToList(DataTable table) {
			//No need to check RemotingRole; no call to db.
			return Crud.MedicationCrud.TableToList(table);
		}

		///<summary>Returns medications that contain the passed in string.  Blank for all.</summary>
		public static List<Medication> GetList(string str) {
			//No need to check RemotingRole; no call to db.
			//This is not the correct way... The real answer is to implement invalid types for Medications.
			Refresh();
			List<Medication> retVal=new List<Medication>();
			Medication[] arrayMeds=GetListt();
			for(int i=0;i<arrayMeds.Length;i++) {
				if(str=="" || arrayMeds[i].MedName.ToUpper().Contains(str.ToUpper())) {
					retVal.Add(arrayMeds[i]);
				}
			}
			return retVal;
		}

		///<summary></summary>
		public static void Update(Medication Cur){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),Cur);
				return;
			}
			Crud.MedicationCrud.Update(Cur);
		}

		///<summary></summary>
		public static long Insert(Medication Cur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Cur.MedicationNum=Meth.GetLong(MethodBase.GetCurrentMethod(),Cur);
				return Cur.MedicationNum;
			}
			return Crud.MedicationCrud.Insert(Cur);
		}

		///<summary>Dependent brands and patients will already be checked.  Be sure to surround with try-catch.</summary>
		public static void Delete(Medication Cur){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),Cur);
				return;
			}
			string s=IsInUse(Cur);
			if(s!="") {
				throw new ApplicationException(Lans.g("Medications",s));
			}
			string command = "DELETE from medication WHERE medicationNum = '"+Cur.MedicationNum.ToString()+"'";
			Db.NonQ(command);
		}

		///<summary>Returns a string if medication is in use in medicationpat, allergydef, eduresources, or preference.MedicationsIndicateNone. The string will explain where the medication is in use.</summary>
		public static string IsInUse(Medication med) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),med);
			}
			string[] brands;
			if(med.MedicationNum==med.GenericNum) {
				brands=GetBrands(med.MedicationNum);
			}
			else {
				brands=new string[0];
			}
			if(brands.Length>0) {
				return "You can not delete a medication that has brand names attached.";
			}
			string command="SELECT COUNT(*) FROM medicationpat WHERE MedicationNum="+POut.Long(med.MedicationNum);
			if(PIn.Int(Db.GetCount(command))!=0) {
				return "Not allowed to delete medication because it is in use by a patient";
			}
			command="SELECT COUNT(*) FROM allergydef WHERE MedicationNum="+POut.Long(med.MedicationNum);
			if(PIn.Int(Db.GetCount(command))!=0) {
				return "Not allowed to delete medication because it is in use by an allergy";
			}
			command="SELECT COUNT(*) FROM eduresource WHERE MedicationNum="+POut.Long(med.MedicationNum);
			if(PIn.Int(Db.GetCount(command))!=0) {
				return "Not allowed to delete medication because it is in use by an education resource";
			}
			if(PrefC.GetLong(PrefName.MedicationsIndicateNone)==med.MedicationNum) {
				return "Not allowed to delete medication because it is in use by a medication";
			}
			return "";
		}

		///<summary>Returns an array of all patient names who are using this medication.</summary>
		public static string[] GetPatNamesForMed(long medicationNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<string[]>(MethodBase.GetCurrentMethod(),medicationNum);
			}
			string command =
				"SELECT CONCAT(CONCAT(CONCAT(CONCAT(LName,', '),FName),' '),Preferred) FROM medicationpat,patient "
				+"WHERE medicationpat.PatNum=patient.PatNum "
				+"AND medicationpat.MedicationNum="+POut.Long(medicationNum);
			DataTable table=Db.GetTable(command);
			string[] retVal=new string[table.Rows.Count];
			for(int i=0;i<table.Rows.Count;i++){
				retVal[i]=PIn.String(table.Rows[i][0].ToString());
			}
			return retVal;
		}

		///<summary>Returns a list of all brands dependend on this generic. Only gets run if this is a generic.</summary>
		public static string[] GetBrands(long medicationNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<string[]>(MethodBase.GetCurrentMethod(),medicationNum);
			}
			string command =
				"SELECT MedName FROM medication "
				+"WHERE GenericNum="+medicationNum.ToString()
				+" AND MedicationNum !="+medicationNum.ToString();//except this med
			DataTable table=Db.GetTable(command);
			string[] retVal=new string[table.Rows.Count];
			for(int i=0;i<table.Rows.Count;i++){
				retVal[i]=PIn.String(table.Rows[i][0].ToString());
			}
			return retVal;
		}

		///<summary>Returns null if not found.</summary>
		public static Medication GetMedication(long medNum) {
			//No need to check RemotingRole; no call to db.
			if(!HasMedicationInCache(medNum)) {
				return null;//Should never happen.
			}
			Hashtable hashMedications=GetHList();
			return (Medication)hashMedications[medNum];
		}

		///<summary>Deprecated.  Use GetMedication instead.</summary>
		public static Medication GetMedicationFromDb(long medicationNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Medication>(MethodBase.GetCurrentMethod(),medicationNum);
			}
			string command="SELECT * FROM medication WHERE MedicationNum="+POut.Long(medicationNum);
			return Crud.MedicationCrud.SelectOne(command);
		}

		///<summary>//Returns first medication with matching MedName, if not found returns null.</summary>
		public static Medication GetMedicationFromDbByName(string medicationName) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Medication>(MethodBase.GetCurrentMethod(),medicationName);
			}
			string command="SELECT * FROM medication WHERE MedName='"+POut.String(medicationName)+"' ORDER BY MedicationNum";
			List<Medication> retVal=Crud.MedicationCrud.SelectMany(command);
			if(retVal.Count>0) {
				return retVal[0];
			}
			return null;
		}

		///<summary>Gets the generic medication for the specified medication Num. Returns null if not found.</summary>
		public static Medication GetGeneric(long medNum) {
			//No need to check RemotingRole; no call to db.
			if(!HasMedicationInCache(medNum)) {
				return null;
			}
			Hashtable hashMedications=GetHList();
			return (Medication)hashMedications[((Medication)hashMedications[medNum]).GenericNum];
		}

		///<summary>Gets the medication name.  Also, generic in () if applicable.  Returns empty string if not found.</summary>
		public static string GetDescription(long medNum) {
			//No need to check RemotingRole; no call to db.
			if(!HasMedicationInCache(medNum)) {
				return "";
			}
			Hashtable hashMedications=GetHList();
			Medication med=(Medication)hashMedications[medNum];
			string retVal=med.MedName;
			if(med.GenericNum==med.MedicationNum){//this is generic
				return retVal;
			}
			if(!hashMedications.ContainsKey(med.GenericNum)) {
				return retVal;
			}
			Medication generic=(Medication)hashMedications[med.GenericNum];
			return retVal+"("+generic.MedName+")";
		}

		///<summary>Gets the medication name. Copied from GetDescription.</summary>
		public static string GetNameOnly(long medNum) {
			//No need to check RemotingRole; no call to db.
			if(!HasMedicationInCache(medNum)) {
				return "";
			}
			Hashtable hashMedications=GetHList();
			return ((Medication)hashMedications[medNum]).MedName;
		}

		///<summary>Gets the generic medication name, given it's generic Num.</summary>
		public static string GetGenericName(long genericNum) {
			//No need to check RemotingRole; no call to db.
			if(!HasMedicationInCache(genericNum)) {
				return "";
			}
			Hashtable hashMedications=GetHList();
			return ((Medication)hashMedications[genericNum]).MedName;
		}

		///<summary>Gets the generic medication name, given it's generic Num.  Will search through the passed in list before resorting to cache.</summary>
		public static string GetGenericName(long genericNum,Hashtable hlist) {
			//No need to check RemotingRole; no call to db.
			if(!hlist.ContainsKey(genericNum)) {
				//Medication not found.  Refresh the cache and check again.
				return GetGenericName(genericNum);
			}
			return ((Medication)hlist[genericNum]).MedName;
		}

		public static List<long> GetChangedSinceMedicationNums(DateTime changedSince) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),changedSince);
			}
			string command="SELECT MedicationNum FROM medication WHERE DateTStamp > "+POut.DateT(changedSince);
			DataTable dt=Db.GetTable(command);
			List<long> medicationNums = new List<long>(dt.Rows.Count);
			for(int i=0;i<dt.Rows.Count;i++) {
				medicationNums.Add(PIn.Long(dt.Rows[i]["MedicationNum"].ToString()));
			}
			return medicationNums;
		}

		///<summary>Used along with GetChangedSinceMedicationNums</summary>
		public static List<Medication> GetMultMedications(List<long> medicationNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Medication>>(MethodBase.GetCurrentMethod(),medicationNums);
			}
			string strMedicationNums="";
			DataTable table;
			if(medicationNums.Count>0) {
				for(int i=0;i<medicationNums.Count;i++) {
					if(i>0) {
						strMedicationNums+="OR ";
					}
					strMedicationNums+="MedicationNum='"+medicationNums[i].ToString()+"' ";
				}
				string command="SELECT * FROM medication WHERE "+strMedicationNums;
				table=Db.GetTable(command);
			}
			else {
				table=new DataTable();
			}
			Medication[] multMedications=Crud.MedicationCrud.TableToList(table).ToArray();
			List<Medication> MedicationList=new List<Medication>(multMedications);
			return MedicationList;
		}
		
		///<summary>Deprecated.  Use MedicationPat.Refresh() instead.  Returns medication list for a specific patient.</summary>
		public static List<Medication> GetMedicationsByPat(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Medication>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT medication.* "
				+"FROM medication, medicationpat "
				+"WHERE medication.MedicationNum=medicationpat.MedicationNum "
				+"AND medicationpat.PatNum="+POut.Long(patNum);
			return Crud.MedicationCrud.SelectMany(command);
		}

		public static Medication GetMedicationFromDbByRxCui(long rxcui) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Medication>(MethodBase.GetCurrentMethod(),rxcui);
			}
			//an RxCui could be linked to multiple medications, the ORDER BY ensures we get the same medication every time we call this function
			string command="SELECT * FROM medication WHERE RxCui="+POut.Long(rxcui)+" ORDER BY MedicationNum";
			return Crud.MedicationCrud.SelectOne(command);
		}

		public static bool AreMedicationsEqual(Medication medication,Medication medicationOld) {
			//No need to check RemotingRole; no call to db.
			if((medicationOld==null || medication==null)
				|| medicationOld.MedicationNum!=medication.MedicationNum
				|| medicationOld.MedName!=medication.MedName
				|| medicationOld.GenericNum!=medication.GenericNum
				|| medicationOld.Notes!=medication.Notes
				|| medicationOld.RxCui!=medication.RxCui) 
			{
				return false;
			}
			return true;
		}

	}

	




}










