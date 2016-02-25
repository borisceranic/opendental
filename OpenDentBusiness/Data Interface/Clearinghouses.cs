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
					hashClearinghouses.Add(entry.Key,(long)entry.Value);
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
			string command="SELECT * FROM clearinghouse WHERE ClinicNum!=0 ORDER BY Description";
			return Crud.ClearinghouseCrud.SelectMany(command);
		}

		///<summary>Refreshes the cache, which only contains HQ-level clearinghouses.</summary>
		public static DataTable RefreshCacheHq() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM clearinghouse WHERE ClinicNum=0 ORDER BY Description";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="Clearinghouse";
			FillCacheHq(table);
			return table;
		}

		///<summary>Fills the cache, which only contains HQ-level clearinghouses.</summary>
		public static void FillCacheHq(DataTable table) {
			//No need to check RemotingRole; no call to db.
			List<Clearinghouse> hqListt=Crud.ClearinghouseCrud.TableToList(table);
			Hashtable hqHList=new Hashtable();
			foreach(Clearinghouse cHouse in hqListt) {
				foreach(string payorID in cHouse.Payors.Split(',')) {
					if(!hqHList.ContainsKey(payorID)) {
						hqHList.Add(payorID,cHouse.ClearinghouseNum);
					}
				}
			}
			//Possible race condition. Mitigated by using local lists and waiting until the last moment to assign them to the private lists.
			_hqHList=hqHList;
			_hqListt=hqListt.ToArray();
		}

		///<summary>Returns a list of clearinghouses that filter out clearinghouses we no longer want to display.
		///Only includes HQ-level clearinghouses.</summary>
		public static List<Clearinghouse> GetHqListShort() {
			List<Clearinghouse> listClearinghouses=new List<Clearinghouse>(GetHqListt());
			listClearinghouses=listClearinghouses.Where(x => x.CommBridge!=EclaimsCommBridge.MercuryDE).ToList();
			return listClearinghouses;
		}

		///<summary>Inserts one clearinghouse into the database.  Use this if you know that your clearinghouse will be inserted at the HQ-level.</summary>
		public static long Insert(Clearinghouse clearinghouse){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				clearinghouse.ClearinghouseNum=Meth.GetLong(MethodBase.GetCurrentMethod(),clearinghouse);
				return clearinghouse.ClearinghouseNum;
			}
			long clearinghouseNum=Crud.ClearinghouseCrud.Insert(clearinghouse);
			clearinghouse.HqClearinghouseNum=clearinghouseNum;
			Crud.ClearinghouseCrud.Update(clearinghouse);
			return clearinghouseNum;
		}

		///<summary>Updates the clearinghouse in the database that has the same primary key as the passed-in clearinghouse.   
		///Use this if you know that your clearinghouse will be updated at the HQ-level, 
		///or if you already have a well-defined clinic-level clearinghouse.  For lists of clearinghouses, use the Sync method instead.</summary>
		public static void Update(Clearinghouse clearinghouse){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),clearinghouse);
				return;
			}
			Crud.ClearinghouseCrud.Update(clearinghouse);
		}

		public static void Update(Clearinghouse clearinghouse,Clearinghouse oldClearinghouse) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),clearinghouse,oldClearinghouse);
				return;
			}
			Crud.ClearinghouseCrud.Update(clearinghouse,oldClearinghouse);
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

		///<summary>Gets the last batch number from db for the HQ version of this clearinghouseClin and increments it by one.
		///Then saves the new value to db and returns it.  So even if the new value is not used for some reason, it will have already been incremented.
		///Remember that LastBatchNumber is never accurate with local data in memory.</summary>
		public static int GetNextBatchNumber(Clearinghouse clearinghouseClin){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod(),clearinghouseClin);
			}
			//get last batch number
			string command="SELECT LastBatchNumber FROM clearinghouse "
				+"WHERE ClearinghouseNum = "+POut.Long(clearinghouseClin.HqClearinghouseNum);
 			DataTable table=Db.GetTable(command);
			int batchNum=PIn.Int(table.Rows[0][0].ToString());
			//and increment it by one
			if(clearinghouseClin.Eformat==ElectronicClaimFormat.Canadian){
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
				+" WHERE ClearinghouseNum = "+POut.Long(clearinghouseClin.HqClearinghouseNum);
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
				return null;
			}
			string command="SELECT * FROM clearinghouse WHERE HqClearinghouseNum="+clearinghouseHq.ClearinghouseNum+" AND ClinicNum="+clinicNum;
			return Crud.ClearinghouseCrud.SelectOne(command);
		}

		///<summary>Replaces all clinic-level fields in ClearinghouseHq with non-blank fields 
		///from the clinic-level clearinghouse for the passed-in clinicNum. Non clinic-level fields are not replaced.</summary>
		public static Clearinghouse OverrideFields(Clearinghouse clearinghouseHq,long clinicNum) {
			//No need to check RemotingRole; no call to db.
			Clearinghouse clearinghouseClin=Clearinghouses.GetForClinic(clearinghouseHq,clinicNum);
			return OverrideFields(clearinghouseHq,clearinghouseClin);
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

		///<summary>Syncs a given list of clinic-level clearinghouses to a list of old clinic-level clearinghouses.</summary>
		public static void Sync(List<Clearinghouse> listClearinghouseNew,List<Clearinghouse> listClearinghouseOld) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listClearinghouseNew,listClearinghouseOld);
				return;
			}
			Crud.ClearinghouseCrud.Sync(listClearinghouseNew,listClearinghouseOld);
		}

	}
}