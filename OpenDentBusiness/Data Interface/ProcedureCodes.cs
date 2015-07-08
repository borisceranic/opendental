using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ProcedureCodes {
		public const string GroupProcCode="~GRP~";

		///<summary></summary>
		public static DataTable RefreshCache() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string c="SELECT * FROM procedurecode ORDER BY ProcCat,ProcCode";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),c);
			table.TableName="ProcedureCode";
			FillCache(table);
			return table;
		}

		public static void FillCache(DataTable table) {
			//No need to check RemotingRole; no call to db.
			ProcedureCodeC.Listt=Crud.ProcedureCodeCrud.TableToList(table);
			Hashtable hashProcCodes=new Hashtable();
			List<ProcedureCode> listProcedureCodes=ProcedureCodeC.GetListLong();
			for(int i=0;i<listProcedureCodes.Count;i++) {
				try {
					hashProcCodes.Add(listProcedureCodes[i].ProcCode,listProcedureCodes[i].Copy());
				}
				catch {//in case of duplicate in db
				}
			}
			ProcedureCodeC.HList=hashProcCodes;
		}

		public static List<ProcedureCode> GetChangedSince(DateTime changedSince) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ProcedureCode>>(MethodBase.GetCurrentMethod(),changedSince);
			}
			string command="SELECT * FROM procedurecode WHERE DateTStamp > "+POut.DateT(changedSince);
			return Crud.ProcedureCodeCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(ProcedureCode code) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				code.CodeNum=Meth.GetLong(MethodBase.GetCurrentMethod(),code);
				return code.CodeNum;
			}
			//must have already checked procCode for nonduplicate.
			return Crud.ProcedureCodeCrud.Insert(code);
		}

		///<summary></summary>
		public static void Update(ProcedureCode code){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),code);
				return;
			}
			Crud.ProcedureCodeCrud.Update(code);
		}

		///<summary>Counts all procedure codes, including hidden codes.</summary>
		public static long GetCodeCount() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod());
			}
			string command="SELECT COUNT(*) FROM procedurecode";
			return PIn.Long(Db.GetCount(command));
		}

		///<summary>Returns the ProcedureCode for the supplied procCode such as such as D####.</summary>
		public static ProcedureCode GetProcCode(string myCode){
			//No need to check RemotingRole; no call to db.
			if(myCode==null){
				//MessageBox.Show(Lans.g("ProcCodes","Error. Invalid procedure code."));
				return new ProcedureCode();
			}
			Hashtable hashProcedureCodes=ProcedureCodeC.GetHList();
			if(hashProcedureCodes.Contains(myCode)) {
				return (ProcedureCode)hashProcedureCodes[myCode];
			}
			else{
				return new ProcedureCode();
			}
		}

		///<summary>The new way of getting a procCode. Uses the primary key instead of string code.</summary>
		public static ProcedureCode GetProcCode(long codeNum) {
			//No need to check RemotingRole; no call to db.
			if(codeNum==0) {
				//MessageBox.Show(Lans.g("ProcCodes","Error. Invalid procedure code."));
				return new ProcedureCode();
			}
			List<ProcedureCode> listProcedureCodes=ProcedureCodeC.GetListLong();
			for(int i=0;i<listProcedureCodes.Count;i++) {
				if(listProcedureCodes[i].CodeNum==codeNum) {
					return listProcedureCodes[i];
				}
			}
			return new ProcedureCode();
		}

		///<summary>Gets code from db to avoid having to constantly refresh in FormProcCodes</summary>
		public static ProcedureCode GetProcCodeFromDb(long codeNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<ProcedureCode>(MethodBase.GetCurrentMethod(),codeNum);
			}
			ProcedureCode retval=Crud.ProcedureCodeCrud.SelectOne(codeNum);
			if(retval==null) {
				//We clasically return an empty procedurecode object here instead of null.
				return new ProcedureCode();
			}
			return retval;
		}

		///<summary>Supply the human readable proc code such as D####</summary>
		public static long GetCodeNum(string myCode) {
			//No need to check RemotingRole; no call to db.
			if(myCode==null || myCode=="") {
				return 0;
			}
			Hashtable hashProcedureCodes=ProcedureCodeC.GetHList();
			if(hashProcedureCodes.Contains(myCode)) {
				return ((ProcedureCode)hashProcedureCodes[myCode]).CodeNum;
			}
			return 0;
			//else {
			//	throw new ApplicationException("Missing code");
			//}
		}

		///<summary>If a substitute exists for the given proc code, then it will give the CodeNum of that code.  Otherwise, it will return the codeNum for the given procCode.</summary>
		public static long GetSubstituteCodeNum(string procCode,string toothNum) {
			//No need to check RemotingRole; no call to db.
			if(procCode==null || procCode=="") {
				return 0;
			}
			Hashtable hashProcedureCodes=ProcedureCodeC.GetHList();
			if(!hashProcedureCodes.Contains(procCode)) {
				return 0;
			}
			ProcedureCode proc=(ProcedureCode)hashProcedureCodes[procCode];
			if(proc.SubstitutionCode!="" && hashProcedureCodes.Contains(proc.SubstitutionCode)) {
				if(proc.SubstOnlyIf==SubstitutionCondition.Always){
					return ((ProcedureCode)hashProcedureCodes[proc.SubstitutionCode]).CodeNum;
				}
				if(proc.SubstOnlyIf==SubstitutionCondition.Molar && Tooth.IsMolar(toothNum)){
					return ((ProcedureCode)hashProcedureCodes[proc.SubstitutionCode]).CodeNum;
				}
				if(proc.SubstOnlyIf==SubstitutionCondition.SecondMolar && Tooth.IsSecondMolar(toothNum)) {
					return ((ProcedureCode)hashProcedureCodes[proc.SubstitutionCode]).CodeNum;
				}
			}
			return proc.CodeNum;
		}

		public static string GetStringProcCode(long codeNum) {
			//No need to check RemotingRole; no call to db.
			if(codeNum==0) {
				return "";
				//throw new ApplicationException("CodeNum cannot be zero.");
			}
			List<ProcedureCode> listProcedureCodes=ProcedureCodeC.GetListLong();
			for(int i=0;i<listProcedureCodes.Count;i++) {
				if(listProcedureCodes[i].CodeNum==codeNum) {
					return listProcedureCodes[i].ProcCode;
				}
			}
			throw new ApplicationException("Missing codenum");
		}

		///<summary></summary>
		public static bool IsValidCode(string myCode){
			//No need to check RemotingRole; no call to db.
			if(myCode==null || myCode=="") {
				return false;
			}
			Hashtable hashProcedureCodes=ProcedureCodeC.GetHList();
			if(hashProcedureCodes.Contains(myCode)) {
				return true;
			}
			else {
				return false;
			}
		}

		///<summary>Grouped by Category.  Used only in FormRpProcCodes.</summary>
		public static ProcedureCode[] GetProcList(){
			//No need to check RemotingRole; no call to db.
			List<ProcedureCode> retVal=new List<ProcedureCode>();
			List<ProcedureCode> listProcedureCodes=ProcedureCodeC.GetListLong();
			Def[][] arrayDefs=DefC.GetArrayShort();
			for(int j=0;j<arrayDefs[(int)DefCat.ProcCodeCats].Length;j++) {
				for(int k=0;k<listProcedureCodes.Count;k++) {
					if(arrayDefs[(int)DefCat.ProcCodeCats][j].DefNum==listProcedureCodes[k].ProcCat) {
						retVal.Add(listProcedureCodes[k].Copy());
					}
				}
			}
			return retVal.ToArray();
		}

		///<summary>Gets a list of procedure codes directly from the database.  If categories.length==0, then we will get for all categories.  Categories are defnums.  FeeScheds are, for now, defnums.</summary>
		public static DataTable GetProcTable(string abbr,string desc,string code,List<long> categories,long feeSchedNum,
			long feeSched2Num,long feeSched3Num)
		{
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),abbr,desc,code,categories,feeSchedNum,feeSched2Num,feeSched3Num);
			}
			string whereCat;
			if(categories.Count==0){
				whereCat="1";
			}
			else{
				whereCat="(";
				for(int i=0;i<categories.Count;i++){
					if(i>0){
						whereCat+=" OR ";
					}
					whereCat+="ProcCat="+POut.Long(categories[i]);
				}
				whereCat+=")";
			}
			List<FeeSched> listFeeScheds=FeeSchedC.GetListLong();
			FeeSched feeSched=FeeScheds.GetOne(feeSchedNum,listFeeScheds);//Gets from cache.
			FeeSched feeSched2=FeeScheds.GetOne(feeSched2Num,listFeeScheds);//Gets from cache.
			FeeSched feeSched3=FeeScheds.GetOne(feeSched3Num,listFeeScheds);//Gets from cache.
			//Query changed to be compatible with both MySQL and Oracle (not tested).
			string command="SELECT ProcCat,Descript,AbbrDesc,procedurecode.ProcCode,";
			if(feeSched==null) {
				command+="-1 FeeAmt1,";
			}
			else {
				command+="CASE ";
				if(!feeSched.IsGlobal && Clinics.ClinicNum!=0) {//Use local clinic fee if there's one present
					command+="WHEN (feeclinic1.Amount IS NOT NULL) THEN feeclinic1.Amount ";
				}
				command+="WHEN (feehq1.Amount IS NOT NULL) THEN feehq1.Amount ELSE -1 END FeeAmt1,";
			}
			if(feeSched2==null) {
				command+="-1 FeeAmt2,";
			}
			else {
				command+="CASE ";
				if(!feeSched2.IsGlobal && Clinics.ClinicNum!=0) {//Use local clinic fee if there's one present
					command+="WHEN (feeclinic2.Amount IS NOT NULL) THEN feeclinic2.Amount ";
				}
				command+="WHEN (feehq2.Amount IS NOT NULL) THEN feehq2.Amount ELSE -1 END FeeAmt2,";
			}
			if(feeSched3==null) {
				command+="-1 FeeAmt3,";
			}
			else {
				command+="CASE ";
				if(!feeSched3.IsGlobal && Clinics.ClinicNum!=0) {//Use local clinic fee if there's one present
					command+="WHEN (feeclinic3.Amount IS NOT NULL) THEN feeclinic3.Amount ";
				}
				command+="WHEN (feehq3.Amount IS NOT NULL) THEN feehq3.Amount ELSE -1 END FeeAmt3,";				
			}
			command+="procedurecode.CodeNum ";
			if(feeSched!=null && !feeSched.IsGlobal && Clinics.ClinicNum!=0) {
				command+=",CASE WHEN (feeclinic1.Amount IS NOT NULL) THEN 1 ELSE 0 END IsClinic1 ";
			}
			if(feeSched2!=null && !feeSched2.IsGlobal && Clinics.ClinicNum!=0) {
				command+=",CASE WHEN (feeclinic2.Amount IS NOT NULL) THEN 1 ELSE 0 END IsClinic2 ";
			}
			if(feeSched3!=null && !feeSched3.IsGlobal && Clinics.ClinicNum!=0) {
				command+=",CASE WHEN (feeclinic3.Amount IS NOT NULL) THEN 1 ELSE 0 END IsClinic3 ";
			}
			command+="FROM procedurecode ";
			if(feeSched!=null) {
				if(!feeSched.IsGlobal && Clinics.ClinicNum!=0) {//Get local clinic fee if there's one present
					command+="LEFT JOIN fee feeclinic1 ON feeclinic1.CodeNum=procedurecode.CodeNum AND feeclinic1.FeeSched="+POut.Long(feeSched.FeeSchedNum)
						+" AND feeclinic1.ClinicNum="+POut.Long(Clinics.ClinicNum)+" ";
				}
				//Get the hq clinic fee if there's one present
				command+="LEFT JOIN fee feehq1 ON feehq1.CodeNum=procedurecode.CodeNum AND feehq1.FeeSched="+POut.Long(feeSched.FeeSchedNum)
					+" AND feehq1.ClinicNum=0 ";
			}
			if(feeSched2!=null) {
				if(!feeSched2.IsGlobal && Clinics.ClinicNum!=0) {//Get local clinic fee if there's one present
					command+="LEFT JOIN fee feeclinic2 ON feeclinic2.CodeNum=procedurecode.CodeNum AND feeclinic2.FeeSched="+POut.Long(feeSched2.FeeSchedNum)
						+" AND feeclinic2.ClinicNum="+POut.Long(Clinics.ClinicNum)+" ";
				}
				//Get the hq clinic fee if there's one present
				command+="LEFT JOIN fee feehq2 ON feehq2.CodeNum=procedurecode.CodeNum AND feehq2.FeeSched="+POut.Long(feeSched2.FeeSchedNum)
					+" AND feehq2.ClinicNum=0 ";
			}
			if(feeSched3!=null) {
				if(!feeSched3.IsGlobal && Clinics.ClinicNum!=0) {//Get local clinic fee if there's one present
					command+="LEFT JOIN fee feeclinic3 ON feeclinic3.CodeNum=procedurecode.CodeNum AND feeclinic3.FeeSched="+POut.Long(feeSched3.FeeSchedNum)
						+" AND feeClinic3.ClinicNum="+POut.Long(Clinics.ClinicNum)+" ";
				}
				//Get the hq clinic fee if there's one present
				command+="LEFT JOIN fee feehq3 ON feehq3.CodeNum=procedurecode.CodeNum AND feehq3.FeeSched="+POut.Long(feeSched3.FeeSchedNum)
					+" AND feehq3.ClinicNum=0 ";
			}
			command+="LEFT JOIN definition ON definition.DefNum=procedurecode.ProcCat "
			+"WHERE "+whereCat+" "
			+"AND Descript LIKE '%"+POut.String(desc)+"%' "
			+"AND AbbrDesc LIKE '%"+POut.String(abbr)+"%' "
			+"AND procedurecode.ProcCode LIKE '%"+POut.String(code)+"%' "
			+"ORDER BY definition.ItemOrder,procedurecode.ProcCode";
			return Db.GetTable(command);
		}

		///<summary>Returns the LaymanTerm for the supplied codeNum, or the description if none present.</summary>
		public static string GetLaymanTerm(long codeNum) {
			//No need to check RemotingRole; no call to db.
			List<ProcedureCode> listProcedureCodes=ProcedureCodeC.GetListLong();
			for(int i=0;i<listProcedureCodes.Count;i++) {
				if(listProcedureCodes[i].CodeNum==codeNum) {
					if(listProcedureCodes[i].LaymanTerm !="") {
						return listProcedureCodes[i].LaymanTerm;
					}
					return listProcedureCodes[i].Descript;
				}
			}
			return "";
		}

		///<summary>Used to check whether codes starting with T exist and are in a visible category.  If so, it moves them to the Obsolete category.  If the T code has never been used, then it deletes it.</summary>
		public static void TcodesClear() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod());
				return;
			}
			//first delete any unused T codes
			string command=@"SELECT CodeNum,ProcCode FROM procedurecode
				WHERE NOT EXISTS(SELECT * FROM procedurelog WHERE procedurelog.CodeNum=procedurecode.CodeNum)
				AND ProcCode LIKE 'T%'";
			DataTable table=Db.GetTable(command);
			long codenum;
			for(int i=0;i<table.Rows.Count;i++) {
				codenum=PIn.Long(table.Rows[i]["CodeNum"].ToString());
				command="DELETE FROM fee WHERE CodeNum="+POut.Long(codenum);
				Db.NonQ(command);
				command="DELETE FROM procedurecode WHERE CodeNum="+POut.Long(codenum);
				Db.NonQ(command);
			}
			//then, move any other T codes to obsolete category
			command=@"SELECT DISTINCT ProcCat FROM procedurecode,definition 
				WHERE procedurecode.ProcCode LIKE 'T%'
				AND definition.IsHidden=0
				AND procedurecode.ProcCat=definition.DefNum";
			table=Db.GetTable(command);
			long catNum=DefC.GetByExactName(DefCat.ProcCodeCats,"Obsolete");//check to make sure an Obsolete category exists.
			Def def;
			if(catNum!=0) {//if a category exists with that name
				def=DefC.GetDef(DefCat.ProcCodeCats,catNum);
				if(!def.IsHidden) {
					def.IsHidden=true;
					Defs.Update(def);
					Defs.RefreshCache();
				}
			}
			if(catNum==0) {
				Def[][] arrayDefs=DefC.GetArrayLong();
				def=new Def();
				def.Category=DefCat.ProcCodeCats;
				def.ItemName="Obsolete";
				def.ItemOrder=arrayDefs[(int)DefCat.ProcCodeCats].Length;
				def.IsHidden=true;
				Defs.Insert(def);
				Defs.RefreshCache();
				catNum=def.DefNum;
			}
			for(int i=0;i<table.Rows.Count;i++) {
				command="UPDATE procedurecode SET ProcCat="+POut.Long(catNum)
					+" WHERE ProcCat="+table.Rows[i][0].ToString()
					+" AND procedurecode.ProcCode LIKE 'T%'";
				Db.NonQ(command);
			}
			//finally, set Never Used category to be hidden.  This isn't really part of clearing Tcodes, but is required
			//because many customers won't have that category hidden
			catNum=DefC.GetByExactName(DefCat.ProcCodeCats,"Never Used");
			if(catNum!=0) {//if a category exists with that name
				def=DefC.GetDef(DefCat.ProcCodeCats,catNum);
				if(!def.IsHidden) {
					def.IsHidden=true;
					Defs.Update(def);
					Defs.RefreshCache();
				}
			}
		}

		public static void ResetApptProcsQuickAdd() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod());
				return;
			}
			string command= "DELETE FROM definition WHERE Category=3";
			Db.NonQ(command);
			string[] array=new string[] {
				"CompEx-4BW-Pano-Pro-Flo","D0150,D0274,D0330,D1110,D1208",
				"CompEx-2BW-Pano-ChPro-Flo","D0150,D0272,D0330,D1120,D1208",
				"PerEx-4BW-Pro-Flo","D0120,D0274,D1110,D1208",
				"LimEx-PA","D0140,D0220",
				"PerEx-4BW-Pro-Flo","D0120,D0274,D1110,D1208",
				"PerEx-2BW-ChildPro-Flo","D0120,D0272,D1120,D1208",
				"Comp Exam","D0150",
				"Per Exam","D0120",
				"Lim Exam","D0140",
				"1 PA","D0220",
				"2BW","D0272",
				"4BW","D0274",
				"Pano","D0330",
				"Pro Adult","D1110",
				"Fluor","D1208",
				"Pro Child","D1120",
				"PostOp","N4101",
				"DentAdj","N4102",
				"Consult","D9310"
			};
			Def def;
			string[] codelist;
			bool allvalid;
			int itemorder=0;
			for(int i=0;i<array.Length;i+=2) {
				//first, test all procedures for valid
				codelist=array[i+1].Split(',');
				allvalid=true;
				for(int c=0;c<codelist.Length;c++) {
					if(!ProcedureCodes.IsValidCode(codelist[c])) {
						allvalid=false;
					}
				}
				if(!allvalid) {
					continue;
				}
				def=new Def();
				def.Category=DefCat.ApptProcsQuickAdd;
				def.ItemOrder=itemorder;
				def.ItemName=array[i];
				def.ItemValue=array[i+1];
				Defs.Insert(def);
				itemorder++;
			}
		}

		///<summary>Resets the descriptions for all ADA codes to the official wording.  Required by the license.</summary>
		public static int ResetADAdescriptions() {
			//No need to check RemotingRole; no call to db.
			return ResetADAdescriptions(CDT.Class1.GetADAcodes());
		}

		///<summary>Resets the descriptions for all ADA codes to the official wording.  Required by the license.</summary>
		public static int ResetADAdescriptions(List<ProcedureCode> codeList) {
			//No need to check RemotingRole; no call to db.
			ProcedureCode code;
			int count=0;
			for(int i=0;i<codeList.Count;i++) {
				if(!ProcedureCodes.IsValidCode(codeList[i].ProcCode)) {//If this code is not in this database
					continue;
				}
				code=ProcedureCodes.GetProcCode(codeList[i].ProcCode);
				if(code.Descript==codeList[i].Descript) {
					continue;
				}
				code.Descript=codeList[i].Descript;
				ProcedureCodes.Update(code);
				count++;
			}
			return count;
			//don't forget to refresh procedurecodes.
		}

		public static List<ProcedureCode> GetAllCodes() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ProcedureCode>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * from procedurecode ORDER BY ProcCode";
			return Crud.ProcedureCodeCrud.SelectMany(command);
		}


		/* js These are not currently in use.  This probably needs to be consolidated with code from other places.  ProcsColored and InsSpans comes to mind.
		///<summary>Returns true if any of the codes in the list fall within the code range.</summary>
		public static bool IsCodeInRange(List<string> myCodes,string range) {
			for(int i=0;i<myCodes.Count;i++) {
				if(IsCodeInRange(myCodes[i],range)) {
					return true;
				}
			}
			return false;
		}

		///<summary>Returns true if myCode is within the code range.  Ex: myCode="D####", range="D####-D####"</summary>
		public static bool IsCodeInRange(string myCode,string range) {
			string code1="";
			string code2="";
			if(range.Contains("-")) {
				string[] codeSplit=range.Split('-');
				code1=codeSplit[0].Trim();
				code2=codeSplit[1].Trim();
			}
			else{
				code1=range.Trim();
				code2=range.Trim();
			}
			if(myCode.CompareTo(code1)<0 || myCode.CompareTo(code2)>0) {
				return false;
			}
			return true;
		}*/



	}

	
	
	


}