using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Linq;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Clearinghouses {
		///<summary>List of all HQ-level clearinghouses.</summary>
		private static Clearinghouse[] _hqListt;
		///<summary>Key=PayorID. Value=ClearingHouseNum.</summary>
		private static Hashtable _hqHList;
		private static object _lockObj=new object();

		public static Clearinghouse[] HqListt{
			//No need to check RemotingRole; no call to db.
			get{
				return GetHqListt();
			}
			set{
				lock(_lockObj) {
					_hqListt=value;
				}
			}
		}

		///<summary>key:PayorID, value:ClearingHouseNum</summary>
		public static Hashtable HqHList {
			get {
				return GetHqHList();
			}
			set {
				lock(_lockObj) {
					_hqHList=value;
				}
			}
		}

		///<summary></summary>
		public static Clearinghouse[] GetHqListt() {
			bool isListNull=false;
			lock(_lockObj) {
				if(_hqListt==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				RefreshCacheHq();
			}
			Clearinghouse[] arrayClearinghouse=new Clearinghouse[_hqListt.Length];
			lock(_lockObj) {
				for(int i=0;i<_hqListt.Length;i++) {
					arrayClearinghouse[i]=_hqListt[i].Copy();
				}
			}
			return arrayClearinghouse;
		}

		///<summary>key:PayorID, value:ClearingHouseNum</summary>
		public static Hashtable GetHqHList() {
			bool isListNull=false;
			lock(_lockObj) {
				if(_hqHList==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				RefreshCacheHq();
			}
			Hashtable hashClearinghouses=new Hashtable();
			lock(_lockObj) {
				foreach(DictionaryEntry entry in _hqHList) {
					hashClearinghouses.Add(entry.Key,((Clearinghouse)entry.Value).Copy());
				}
			}
			return hashClearinghouses;
		}

		///<summary>Gets all clearinghouses for the specified clinic.  Returns an empty list if clinicNum=0.  
		///Use the cache if you want all HQ Clearinghouses.</summary>
		public static List<Clearinghouse> GetAllNonHq() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Clearinghouse>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM clearinghouse WHERE ClinicNum!=0";
			return Crud.ClearinghouseCrud.SelectMany(command);
		}

		///<summary>Refreshes the cache, which only contains HQ-level clearinghouses.</summary>
		public static DataTable RefreshCacheHq() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM clearinghouse WHERE ClinicNum=0";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="Clearinghouse";
			FillCacheHq(table);
			return table;
		}

		///<summary>Fills the cache, which only contains HQ-level clearinghouses.</summary>
		public static void FillCacheHq(DataTable table) {
			//No need to check RemotingRole; no call to db.
			_hqListt=Crud.ClearinghouseCrud.TableToList(table).ToArray();
			HqHList=new Hashtable();
			string[] payors;
			Clearinghouse[] arrayClearinghouse=GetHqListt();
			for(int i=0;i<arrayClearinghouse.Length;i++) {
				payors=arrayClearinghouse[i].Payors.Split(',');
				for(int j=0;j<payors.Length;j++) {
					if(!HqHList.ContainsKey(payors[j])) {
						HqHList.Add(payors[j],arrayClearinghouse[i].ClearinghouseNum);
					}
				}
			}
		}

		///<summary>Returns a list of clearinghouses that filter out clearinghouses we no longer want to display.
		///Only includes HQ-level clearinghouses.</summary>
		public static List<Clearinghouse> GetHqListShort() {
			List<Clearinghouse> listClearinghouses=new List<Clearinghouse>(GetHqListt());
			listClearinghouses=listClearinghouses.Where(x => x.CommBridge!=EclaimsCommBridge.MercuryDE).ToList();
			return listClearinghouses;
		}

		///<summary>Inserts this clearinghouse into database.  You may use this if you know that your clearinghouse will be inserted at the HQ-level,
		///or if you alreayd have a well-defined clinic-level clearinghouse.  Otherwise, you should use InsertOrUpdateForClinic instead.</summary>
		public static long Insert(Clearinghouse clearinghouse){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				clearinghouse.ClearinghouseNum=Meth.GetLong(MethodBase.GetCurrentMethod(),clearinghouse);
				return clearinghouse.ClearinghouseNum;
			}
			return Crud.ClearinghouseCrud.Insert(clearinghouse);
		}

		///<summary>Updates the clearinghouse in the database that has the same primary key as the passed-in clearinghouse.   
		///You may use this if you know that your clearinghouse will be updated at the HQ-level, 
		///or if you alreayd have a well-defined clinic-level clearinghouse.  Otherwise, you should use InsertOrUpdateForClinic instead. </summary>
		public static void Update(Clearinghouse clearinghouse){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),clearinghouse);
				return;
			}
			Crud.ClearinghouseCrud.Update(clearinghouse);
		}

		///<summary>Inserts a clinic as HQ if the passed-in clearinghouseClin.ClearinghouseNum==0, otherwise updates.
		///Only certain fields get updated in the clinic-level clearinghouse. All other fields are updated as HQ.
		///clearinghouseClin.ClinicNum must be set to the correct clinic before this method is called.   
		///The passed-in clearinghouses are set to the values that are put into the database for caching purposes.</summary>
		public static void InsertOrUpdateForClinic(Clearinghouse clearinghouseHq,Clearinghouse clearinghouseClin) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),clearinghouseHq,clearinghouseClin);
				return;
			}
			//HQ unconditional values.  Copies values the user typed in from the clinic and overrides HQ values.  This maintains old behavior.
			clearinghouseHq.Description=clearinghouseClin.Description;
			clearinghouseClin.Description="";
			clearinghouseHq.Payors=clearinghouseClin.Payors;
			clearinghouseClin.Payors="";
			clearinghouseHq.Eformat=clearinghouseClin.Eformat;
			clearinghouseClin.Eformat=ElectronicClaimFormat.None;
			clearinghouseHq.ISA05=clearinghouseClin.ISA05;
			clearinghouseClin.ISA05="";
			clearinghouseHq.ISA07=clearinghouseClin.ISA07;
			clearinghouseClin.ISA07="";
			clearinghouseHq.ISA08=clearinghouseClin.ISA08;
			clearinghouseClin.ISA08="";
			clearinghouseHq.ISA15=clearinghouseClin.ISA15;
			clearinghouseClin.ISA15="";
			clearinghouseHq.CommBridge=clearinghouseClin.CommBridge;
			clearinghouseClin.CommBridge=EclaimsCommBridge.None;
			//clearinghouseHq.LastBatchNumber=;//Not editable is UI and should not be updated here.  See GetNextBatchNumber() above.
			clearinghouseHq.ModemPort=clearinghouseClin.ModemPort;
			clearinghouseClin.ModemPort=0;
			clearinghouseHq.GS03=clearinghouseClin.GS03;
			clearinghouseClin.GS03="";
			clearinghouseHq.ISA02=clearinghouseClin.ISA02;
			clearinghouseClin.ISA02="";
			clearinghouseHq.ISA04=clearinghouseClin.ISA04;
			clearinghouseClin.ISA04="";
			clearinghouseHq.ISA16=clearinghouseClin.ISA16;
			clearinghouseClin.ISA16="";
			clearinghouseHq.SeparatorData=clearinghouseClin.SeparatorData;
			clearinghouseClin.SeparatorData="";
			clearinghouseHq.SeparatorSegment=clearinghouseClin.SeparatorSegment;
			clearinghouseClin.SeparatorSegment="";
			//Clinic unconditional values
			clearinghouseClin.HqClearinghouseNum=clearinghouseHq.ClearinghouseNum;
			//Clinic override values
			if(clearinghouseClin.ExportPath==clearinghouseHq.ExportPath) {
				clearinghouseClin.ExportPath="";//The value is the same as the default.  Save blank so that default can be updated dynamically.
			}
			else {
				//Maintain the override value that the user typed.
			}
			if(clearinghouseClin.SenderTIN==clearinghouseHq.SenderTIN) {
				clearinghouseClin.SenderTIN="";//The value is the same as the default.  Save blank so that default can be updated dynamically.
			}
			else {
				//Maintain the override value that the user typed.
			}
			if(clearinghouseClin.Password==clearinghouseHq.Password) {
				clearinghouseClin.Password="";//The value is the same as the default.  Save blank so that default can be updated dynamically.
			}
			else {
				//Maintain the override value that the user typed.
			}
			if(clearinghouseClin.ResponsePath==clearinghouseHq.ResponsePath) {
				clearinghouseClin.ResponsePath="";//The value is the same as the default.  Save blank so that default can be updated dynamically.
			}
			else {
				//Maintain the override value that the user typed.
			}
			if(clearinghouseClin.ClientProgram==clearinghouseHq.ClientProgram) {
				clearinghouseClin.ClientProgram="";//The value is the same as the default.  Save blank so that default can be updated dynamically.
			}
			else {
				//Maintain the override value that the user typed.
			}
			if(clearinghouseClin.LoginID==clearinghouseHq.LoginID) {
				clearinghouseClin.LoginID="";//The value is the same as the default.  Save blank so that default can be updated dynamically.
			}
			else {
				//Maintain the override value that the user typed.
			}
			if(clearinghouseClin.SenderName==clearinghouseHq.SenderName) {
				clearinghouseClin.SenderName="";//The value is the same as the default.  Save blank so that default can be updated dynamically.
			}
			else {
				//Maintain the override value that the user typed.
			}
			if(clearinghouseClin.SenderTelephone==clearinghouseHq.SenderTelephone) {
				clearinghouseClin.SenderTelephone="";//The value is the same as the default.  Save blank so that default can be updated dynamically.
			}
			else {
				//Maintain the override value that the user typed.
			}
			if(clearinghouseClin.ClearinghouseNum==0) {
				Crud.ClearinghouseCrud.Insert(clearinghouseClin);
			}
			else {
				Crud.ClearinghouseCrud.Update(clearinghouseClin);
			}
			Crud.ClearinghouseCrud.Update(clearinghouseHq);
		}

		///<summary>Deletes the passed-in Hq clearinghouse for all clinics.  Only pass in clearinghouses with ClinicNum==0.</summary>
		public static void Delete(Clearinghouse clearinghouseHq){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),clearinghouseHq);
				return;
			}
			string command="DELETE FROM clearinghouse WHERE ClearinghouseNum = '"+POut.Long(clearinghouseHq.ClearinghouseNum)+"'";
			Db.NonQ(command);
			command="DELETE FROM clearinghouse WHERE HqClearinghouseNum='"+POut.Long(clearinghouseHq.ClearinghouseNum)+"'";
			Db.NonQ(command);
		}

		///<summary>Gets the last batch number for this clearinghouse and increments it by one.  Saves the new value, then returns it.
		///So even if the new value is not used for some reason, it will have already been incremented.
		///Remember that LastBatchNumber is never accurate with local data in memory.  The clearinghouse can be HQ or clinic level.</summary>
		public static int GetNextBatchNumber(Clearinghouse clearinghouse){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod(),clearinghouse);
			}
			if(clearinghouse.HqClearinghouseNum!=0) {//Clinic level clearinghouse.
				clearinghouse=Clearinghouses.GetClearinghouse(clearinghouse.HqClearinghouseNum);//Use the HQ clearinghouse instead.  Gets from cache.
			}
			//get last batch number
			string command="SELECT LastBatchNumber FROM clearinghouse "
				+"WHERE ClearinghouseNum = "+POut.Long(clearinghouse.ClearinghouseNum);
 			DataTable table=Db.GetTable(command);
			int batchNum=PIn.Int(table.Rows[0][0].ToString());
			//and increment it by one
			if(clearinghouse.Eformat==ElectronicClaimFormat.Canadian){
				if(batchNum==999999){
					batchNum=1;
				}
				else{
					batchNum++;
				}
			}
			else{
				if(batchNum==999){
					batchNum=1;
				}
				else{
					batchNum++;
				}
			}
			//save the new batch number. Even if user cancels, it will have incremented.
			command="UPDATE clearinghouse SET LastBatchNumber="+batchNum.ToString()
				+" WHERE ClearinghouseNum = "+POut.Long(clearinghouse.ClearinghouseNum);
			Db.NonQ(command);
			return batchNum;
		}

		///<summary>Returns the clearinghouseNum for claims for the supplied payorID.  If the payorID was not entered or if no default was set, then 0 is returned.</summary>
		public static long AutomateClearinghouseHqSelection(string payorID,EnumClaimMedType medType){
			//No need to check RemotingRole; no call to db.
			//payorID can be blank.  For example, Renaissance does not require payorID.
			if(HqHList==null) {
				RefreshCacheHq();
			}
			Clearinghouse clearinghouseHq=null;
			if(medType==EnumClaimMedType.Dental){
				if(PrefC.GetLong(PrefName.ClearinghouseDefaultDent)==0){
					return 0;
				}
				clearinghouseHq=GetClearinghouse(PrefC.GetLong(PrefName.ClearinghouseDefaultDent));
			}
			if(medType==EnumClaimMedType.Medical || medType==EnumClaimMedType.Institutional){
				if(PrefC.GetLong(PrefName.ClearinghouseDefaultMed)==0){
					return 0;
				}
				clearinghouseHq=GetClearinghouse(PrefC.GetLong(PrefName.ClearinghouseDefaultMed));
			}
			if(clearinghouseHq==null){//we couldn't find a default clearinghouse for that medType.  Needs to always be a default.
				return 0;
			}
			if(payorID!="" && HqHList.ContainsKey(payorID)){//an override exists for this payorID
				Clearinghouse ch=GetClearinghouse((long)HqHList[payorID]);
				if(ch.Eformat==ElectronicClaimFormat.x837D_4010 || ch.Eformat==ElectronicClaimFormat.x837D_5010_dental || ch.Eformat==ElectronicClaimFormat.Canadian){//all dental formats
					if(medType==EnumClaimMedType.Dental){//med type matches
						return ch.ClearinghouseNum;
					}
				}
				if(ch.Eformat==ElectronicClaimFormat.x837_5010_med_inst){
					if(medType==EnumClaimMedType.Medical || medType==EnumClaimMedType.Institutional){//med type matches
						return ch.ClearinghouseNum;
					}
				}
			}
			//no override, so just return the default.
			return clearinghouseHq.ClearinghouseNum;
		}

		///<summary>Returns the HQ-level default clearinghouse.  You must manually override using OverrideFields if needed.  If no default present, returns null.</summary>
		public static Clearinghouse GetDefaultDental(){
			//No need to check RemotingRole; no call to db.
			return GetClearinghouse(PrefC.GetLong(PrefName.ClearinghouseDefaultDent));
		}

		///<summary>Gets an HQ clearinghouse from cache.  Will return null if invalid.</summary>
		public static Clearinghouse GetClearinghouse(long clearinghouseNum){
			//No need to check RemotingRole; no call to db.
			Clearinghouse[] arrayClearinghouses=Clearinghouses.GetHqListt();
			for(int i=0;i<arrayClearinghouses.Length;i++){
				if(clearinghouseNum==arrayClearinghouses[i].ClearinghouseNum){
					return arrayClearinghouses[i];
				}
			}
			return null;
		}

		///<summary>Returns the clinic-level clearinghouse for the passed in Clearinghouse.  Usually used in conjunction with ReplaceFields().
		///Can return null.</summary>
		public static Clearinghouse GetForClinic(Clearinghouse clearinghouseHq,long clinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Clearinghouse>(MethodBase.GetCurrentMethod(),clearinghouseHq,clinicNum);
			}
			if(clinicNum==0) { //HQ
				return clearinghouseHq.Copy();
			}
			string command="SELECT * FROM clearinghouse WHERE HqClearinghouseNum="+clearinghouseHq.ClearinghouseNum+" AND ClinicNum="+clinicNum;
			return Crud.ClearinghouseCrud.SelectOne(command);
		}

		///<summary>Replaces all clinic-level fields in ClearinghouseHq with non-blank fields in clearinghouseClin.
		///Non clinic-level fields are commented out and not replaced.</summary>
		public static Clearinghouse OverrideFields(Clearinghouse clearinghouseHq,Clearinghouse clearinghouseClin) {
			//No need to check RemotingRole; no call to db.
			if(clearinghouseHq==null) {
				return null;
			}
			Clearinghouse clearinghouseRetVal=clearinghouseHq.Copy();
			if(clearinghouseClin==null) { //if a null clearingHouseClin was passed in, just return clearinghouseHq.
				return clearinghouseRetVal;
			}
			//HqClearinghouseNum must be set for refreshing the cache when deleting.
			clearinghouseRetVal.HqClearinghouseNum=clearinghouseClin.HqClearinghouseNum;
			//ClearinghouseNum must be set so that updates do not create new entries every time.
			clearinghouseRetVal.ClearinghouseNum=clearinghouseClin.ClearinghouseNum;
			//ClinicNum must be set so that the correct clinic is assigned when inserting new clinic level clearinghouses.
			clearinghouseRetVal.ClinicNum=clearinghouseClin.ClinicNum;
//fields that should not be replaced are commented out.
			//if(!String.IsNullOrEmpty(clearinghouseClin.Description)) {
			//	clearinghouseRetVal.Description=clearinghouseClin.Description;
			//}
			if(!String.IsNullOrEmpty(clearinghouseClin.ExportPath)) {
				clearinghouseRetVal.ExportPath=clearinghouseClin.ExportPath;
			}
			//if(!String.IsNullOrEmpty(clearinghouseClin.Payors)) {
			//	clearinghouseRetVal.Payors=clearinghouseClin.Payors;
			//}
			//if(clearinghouseClin.Eformat!=0 && clearinghouseClin.Eformat!=null) {
			//	clearinghouseRetVal.Eformat=clearinghouseClin.Eformat;
			//}
			//if(!String.IsNullOrEmpty(clearinghouseClin.ISA05)) {
			//	clearinghouseRetVal.ISA05=clearinghouseClin.ISA05;
			//}
			if(!String.IsNullOrEmpty(clearinghouseClin.SenderTIN)) {
				clearinghouseRetVal.SenderTIN=clearinghouseClin.SenderTIN;
			}
			//if(!String.IsNullOrEmpty(clearinghouseClin.ISA07)) {
			//	clearinghouseRetVal.ISA07=clearinghouseClin.ISA07;
			//}
			//if(!String.IsNullOrEmpty(clearinghouseClin.ISA08)) {
			//	clearinghouseRetVal.ISA08=clearinghouseClin.ISA08;
			//}
			//if(!String.IsNullOrEmpty(clearinghouseClin.ISA15)) {
			//	clearinghouseRetVal.ISA15=clearinghouseClin.ISA15;
			//}
			if(!String.IsNullOrEmpty(clearinghouseClin.Password)) {
				clearinghouseRetVal.Password=clearinghouseClin.Password;
			}
			if(!String.IsNullOrEmpty(clearinghouseClin.ResponsePath)) {
				clearinghouseRetVal.ResponsePath=clearinghouseClin.ResponsePath;
			}
			//if(clearinghouseClin.CommBridge!=0 && clearinghouseClin.CommBridge!=null) {
			//	clearinghouseRetVal.CommBridge=clearinghouseClin.CommBridge;
			//}
			if(!String.IsNullOrEmpty(clearinghouseClin.ClientProgram)) {
				clearinghouseRetVal.ClientProgram=clearinghouseClin.ClientProgram;
			}
			//clearinghouseRetVal.LastBatchNumber=;//Not editable is UI and should not be updated here.  See GetNextBatchNumber() above.
			//if(clearinghouseClin.ModemPort!=0 && clearinghouseClin.ModemPort!=null) {
			//	clearinghouseRetVal.ModemPort=clearinghouseClin.ModemPort;
			//}
			if(!String.IsNullOrEmpty(clearinghouseClin.LoginID)) {
				clearinghouseRetVal.LoginID=clearinghouseClin.LoginID;
			}
			if(!String.IsNullOrEmpty(clearinghouseClin.SenderName)) {
				clearinghouseRetVal.SenderName=clearinghouseClin.SenderName;
			}
			if(!String.IsNullOrEmpty(clearinghouseClin.SenderTelephone)) {
				clearinghouseRetVal.SenderTelephone=clearinghouseClin.SenderTelephone;
			}
			//if(!String.IsNullOrEmpty(clearinghouseClin.GS03)) {
			//	clearinghouseRetVal.GS03=clearinghouseClin.GS03;
			//}
			//if(!String.IsNullOrEmpty(clearinghouseClin.ISA02)) {
			//	clearinghouseRetVal.ISA02=clearinghouseClin.ISA02;
			//}
			//if(!String.IsNullOrEmpty(clearinghouseClin.ISA04)) {
			//	clearinghouseRetVal.ISA04=clearinghouseClin.ISA04;
			//}
			//if(!String.IsNullOrEmpty(clearinghouseClin.ISA16)) {
			//	clearinghouseRetVal.ISA16=clearinghouseClin.ISA16;
			//}
			//if(!String.IsNullOrEmpty(clearinghouseClin.SeparatorData)) {
			//	clearinghouseRetVal.SeparatorData=clearinghouseClin.SeparatorData;
			//}
			//if(!String.IsNullOrEmpty(clearinghouseClin.SeparatorSegment)) {
			//	clearinghouseRetVal.SeparatorSegment=clearinghouseClin.SeparatorSegment;
			//}
			return clearinghouseRetVal;
		}


	}
}