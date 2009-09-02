using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OpenDentBusiness{
	///<summary></summary>
	public class CanadianExtracts{
	
		///<summary>The list can be 0 length.</summary>
		public static List<CanadianExtract> GetForClaim(long claimNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<CanadianExtract>>(MethodBase.GetCurrentMethod(),claimNum);
			}
			string command="SELECT * FROM canadianextract WHERE ClaimNum="+POut.PInt(claimNum);
			DataTable table=Db.GetTable(command);
			List<CanadianExtract> retVal=new List<CanadianExtract>();
			CanadianExtract extract;
			for(int i=0;i<table.Rows.Count;i++){
				extract=new CanadianExtract();
				extract.CanadianExtractNum=PIn.PInt   (table.Rows[i][0].ToString());
				extract.ClaimNum          =PIn.PInt   (table.Rows[i][1].ToString());
				extract.ToothNum          =PIn.PString(table.Rows[i][2].ToString());
				extract.DateExtraction    =PIn.PDate  (table.Rows[i][3].ToString());
				retVal.Add(extract);
			}
			return retVal;
		}

		public static int CompareByToothNum(CanadianExtract x1,CanadianExtract x2) {
			//No need to check RemotingRole; no call to db.
			string t1=x1.ToothNum;
			string t2=x2.ToothNum;
			if(t1==null || t1=="" || t2==null || t2=="") {
				return 0;//should never happen
			}
			if(!Tooth.IsValidDB(t1) || !Tooth.IsValidDB(t2)){
				return 0;
			}
			if(Tooth.IsPrimary(t1) && !Tooth.IsPrimary(t2)){
				return -1;
			}
			if(!Tooth.IsPrimary(t1) && Tooth.IsPrimary(t2)) {
				return 1;
			}
			//so either both are primary or both are permanent.
			return Tooth.ToInt(t1).CompareTo(Tooth.ToInt(t2));
		}

		public static void UpdateForClaim(long claimNum,List<CanadianExtract> missinglist) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),claimNum,missinglist);
				return;
			}
			string command="DELETE FROM canadianextract WHERE ClaimNum="+POut.PInt(claimNum);
			Db.NonQ(command);
			for(int i=0;i<missinglist.Count;i++){
				missinglist[i].ClaimNum=claimNum;
				Insert(missinglist[i]);
			}
		}

		///<summary></summary>
		private static long Insert(CanadianExtract cur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				cur.CanadianExtractNum=Meth.GetInt(MethodBase.GetCurrentMethod(),cur);
				return cur.CanadianExtractNum;
			}
			if(PrefC.RandomKeys) {
				cur.CanadianExtractNum=MiscData.GetKey("canadianextract","CanadianExtractNum");
			}
			string command="INSERT INTO canadianextract (";
			if(PrefC.RandomKeys) {
				command+="CanadianExtractNum,";
			}
			command+="ClaimNum,ToothNum,DateExtraction) VALUES(";
			if(PrefC.RandomKeys) {
				command+="'"+POut.PInt(cur.CanadianExtractNum)+"', ";
			}
			command+=
				 "'"+POut.PInt   (cur.ClaimNum)+"', "
				+"'"+POut.PString(cur.ToothNum)+"', "
				+POut.PDate  (cur.DateExtraction)+")";
			if(PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				cur.CanadianExtractNum=Db.NonQ(command,true);
			}
			return cur.CanadianExtractNum;
		}

/*update never used*/

//Delete handled in claim.delete, but not critical if left orphanned.

		///<summary>Supply a list of CanadianExtracts, and this function will filter it and return only items with dates.</summary>
		public static List<CanadianExtract> GetWithDates(List<CanadianExtract> missingList){
			//No need to check RemotingRole; no call to db.
			List<CanadianExtract> retVal=new List<CanadianExtract>();
			foreach(CanadianExtract ce in missingList){
				if(ce.DateExtraction.Year>1880){
					retVal.Add(ce);
				}
			}
			return retVal;
		}

	


		 
		 
	}
}













