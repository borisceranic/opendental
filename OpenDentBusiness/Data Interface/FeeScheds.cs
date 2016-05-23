using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class FeeScheds{
		///<summary></summary>
		public static DataTable RefreshCache() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string c="SELECT * FROM feesched ORDER BY ItemOrder";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),c);
			table.TableName="FeeSched";
			FillCache(table);
			return table;
		}

		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			//FeeSchedC.ListLong=new List<FeeSched>();
			List<FeeSched> listFeeScheds=Crud.FeeSchedCrud.TableToList(table);
			List<FeeSched> listFeeSchedsShort=new List<FeeSched>();
			for(int i=0;i<listFeeScheds.Count;i++) {
				if(!listFeeScheds[i].IsHidden) {
					listFeeSchedsShort.Add(listFeeScheds[i]);
				}
			}
			FeeSchedC.ListShort=listFeeSchedsShort;
			FeeSchedC.ListLong=listFeeScheds;
		}

		///<summary></summary>
		public static long Insert(FeeSched feeSched) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				feeSched.SecUserNumEntry=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				feeSched.FeeSchedNum=Meth.GetLong(MethodBase.GetCurrentMethod(),feeSched);
				return feeSched.FeeSchedNum;
			}
			return Crud.FeeSchedCrud.Insert(feeSched);
		}

		///<summary></summary>
		public static void Update(FeeSched feeSched) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),feeSched);
				return;
			}
			Crud.FeeSchedCrud.Update(feeSched);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  No need to pass in userNum, it's set before remoting role check
		///and passed to the server if necessary.</summary>
		public static void Sync(List<FeeSched> listNew,List<FeeSched> listOld,long userNum=0) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				userNum=Security.CurUser.UserNum;
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listNew,listOld,userNum);
				return;
			}
			Crud.FeeSchedCrud.Sync(listNew,listOld,userNum);
		}

		///<summary>Gets one fee sched from the cache.  Will return null if not found.</summary>
		public static FeeSched GetOne(long feeSchedNum,List<FeeSched> listFeeScheds) {
			for(int i=0;i<listFeeScheds.Count;i++) {
				if(listFeeScheds[i].FeeSchedNum==feeSchedNum) {
					return listFeeScheds[i].Copy();
				}
			}
			return null;//Shouldn't ever happen since we're looking up by primary key.
		}

		///<summary>Returns the description of the fee schedule.  Appends (hidden) if the fee schedule has been hidden.</summary>
		public static string GetDescription(long feeSchedNum) {
			//No need to check RemotingRole; no call to db.
			string feeSchedDesc="";
			if(feeSchedNum==0) {
				return feeSchedDesc;
			}
			List<FeeSched> listFeeScheds=FeeSchedC.GetListLong();
			for(int i=0;i<listFeeScheds.Count;i++) {
				if(listFeeScheds[i].FeeSchedNum!=feeSchedNum) {
					continue;
				}
				feeSchedDesc=listFeeScheds[i].Description;
				feeSchedDesc+=listFeeScheds[i].IsHidden?" ("+Lans.g("FeeScheds","hidden")+")":"";
				break;
			}
			return feeSchedDesc;
		}

		public static bool GetIsHidden(long feeSchedNum) {
			//No need to check RemotingRole; no call to db.
			List<FeeSched> listFeeScheds=FeeSchedC.GetListLong();
			for(int i=0;i<listFeeScheds.Count;i++){
				if(listFeeScheds[i].FeeSchedNum==feeSchedNum){
					return listFeeScheds[i].IsHidden;
				}
			}
			return true;
		}

		///<summary>Will return null if exact name not found.</summary>
		public static FeeSched GetByExactName(string description){
			//No need to check RemotingRole; no call to db.
			List<FeeSched> listFeeScheds=FeeSchedC.GetListLong();
			for(int i=0;i<listFeeScheds.Count;i++){
				if(listFeeScheds[i].Description==description){
					return listFeeScheds[i].Copy();
				}
			}
			return null;
		}

		///<summary>Will return null if exact name not found.</summary>
		public static FeeSched GetByExactName(string description,FeeScheduleType feeSchedType){
			//No need to check RemotingRole; no call to db.
			List<FeeSched> listFeeScheds=FeeSchedC.GetListLong();
			for(int i=0;i<listFeeScheds.Count;i++){
				if(listFeeScheds[i].FeeSchedType!=feeSchedType){
					continue;
				}
				if(listFeeScheds[i].Description==description){
					return listFeeScheds[i].Copy();
				}
			}
			return null;
		}

		///<summary>Only used in FormInsPlan and FormFeeScheds.</summary>
		public static List<FeeSched> GetListForType(FeeScheduleType feeSchedType,bool includeHidden) {
			return GetListForType(feeSchedType,includeHidden,FeeSchedC.GetListLong());
		}

		///<summary>Used to find FeeScheds of a certain type from within a given list.</summary>
		public static List<FeeSched> GetListForType(FeeScheduleType feeSchedType,bool includeHidden,List<FeeSched> listFeeScheds) {
			//No need to check RemotingRole; no call to db.
			List<FeeSched> retVal=new List<FeeSched>();
			for(int i=0;i<listFeeScheds.Count;i++) {
				if(!includeHidden && listFeeScheds[i].IsHidden){
					continue;
				}
				if(listFeeScheds[i].FeeSchedType==feeSchedType){
					retVal.Add(listFeeScheds[i]);
				}
			}
			return retVal;
		}

		///<summary>Deletes FeeScheds that are hidden and not attached to any insurance plans.  Returns the number of deleted fee scheds.</summary>
		public static long CleanupAllowedScheds() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetLong(MethodBase.GetCurrentMethod());
			}
			long result;
			//Detach allowed FeeSchedules from any hidden InsPlans.
			string command="UPDATE insplan "
				+"SET AllowedFeeSched=0 "
				+"WHERE IsHidden=1";
			Db.NonQ(command);
			//Delete unattached FeeSchedules.
			command="DELETE FROM feesched "
				+"WHERE FeeSchedNum NOT IN (SELECT AllowedFeeSched FROM insplan) "
				+"AND FeeSchedType="+POut.Int((int)FeeScheduleType.OutNetwork);
			result=Db.NonQ(command);
			//Delete all orphaned fees.
			command="DELETE FROM fee "
				+"WHERE FeeSched NOT IN (SELECT FeeSchedNum FROM feesched)";
			Db.NonQ(command);
			return result;
		}
	}
}