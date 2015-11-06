using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using OpenDentBusiness.Crud;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Claims{
		
		///<summary>Gets claimpaysplits attached to a claimpayment with the associated patient, insplan, and carrier. If showUnattached it also shows all claimpaysplits that have not been attached to a claimpayment. Pass (0,true) to just get all unattached (outstanding) claimpaysplits.</summary>
		public static List<ClaimPaySplit> RefreshByCheckOld(long claimPaymentNum,bool showUnattached) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClaimPaySplit>>(MethodBase.GetCurrentMethod(),claimPaymentNum,showUnattached);
			}
			string command=
				"SELECT claim.DateService,claim.ProvTreat,CONCAT(CONCAT(patient.LName,', '),patient.FName) patName_"//Changed from \"_patName\" to patName_ for MySQL 5.5. Also added checks for #<table> and $<table>
				+",carrier.CarrierName,SUM(claimproc.FeeBilled) feeBilled_,SUM(claimproc.InsPayAmt) insPayAmt_,claim.ClaimNum"
				+",claimproc.ClaimPaymentNum,(SELECT clinic.Description FROM clinic WHERE claimproc.ClinicNum = clinic.ClinicNum) Description,claim.PatNum,PaymentRow "
				+" FROM claim,patient,insplan,carrier,claimproc"
				+" WHERE claimproc.ClaimNum = claim.ClaimNum"
				+" AND patient.PatNum = claim.PatNum"
				+" AND insplan.PlanNum = claim.PlanNum"
				+" AND insplan.CarrierNum = carrier.CarrierNum"
				+" AND (claimproc.Status = '1' OR claimproc.Status = '4' OR claimproc.Status=5)"//received or supplemental or capclaim
 				+" AND (claimproc.ClaimPaymentNum = '"+POut.Long(claimPaymentNum)+"'";
			if(showUnattached){
				command+=" OR (claimproc.InsPayAmt != 0 AND claimproc.ClaimPaymentNum = '0')";
			}
			//else shows only items attached to this payment
			command+=")"
				+" GROUP BY claim.DateService,claim.ProvTreat,CONCAT(CONCAT(patient.LName,', '),patient.FName) "
				+",carrier.CarrierName,claim.ClaimNum"
				+",claimproc.ClaimPaymentNum,claim.PatNum";
			command+=" ORDER BY patName_";
			DataTable table=Db.GetTable(command);
			return ClaimPaySplitTableToList(table);
		}

		///<summary></summary>
		public static List<Claim> GetClaimsByCheck(long claimPaymentNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Claim>>(MethodBase.GetCurrentMethod(),claimPaymentNum);
			}
			string command=
				"SELECT * "
				+"FROM claim "
				+"WHERE claim.ClaimNum IN "
				+"(SELECT DISTINCT claimproc.ClaimNum "
				+"FROM claimproc "
				+"WHERE claimproc.ClaimPaymentNum="+claimPaymentNum+")";
			return ClaimCrud.SelectMany(command);
		}

		/// <summary>Gets all outstanding claims for the batch payment window.</summary>
		public static List<ClaimPaySplit> GetOutstandingClaims(string carrierName,string LName,string FName) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClaimPaySplit>>(MethodBase.GetCurrentMethod(),carrierName,LName,FName);
			}
			string command="SELECT claim.DateService,claim.ProvTreat,"+DbHelper.Concat("patient.LName","', '","patient.FName")+" patName_,"
				+"carrierA.CarrierName,ClaimFee feeBilled_,SUM(claimproc.InsPayAmt) insPayAmt_,claim.ClaimNum,"//SUM(claimproc.FeeBilled) feeBilled_ was low if inspay 0 on proc
				+"claimproc.ClaimPaymentNum,clinic.Description,claim.PatNum,PaymentRow "
				+"FROM (SELECT CarrierNum, CarrierName FROM carrier WHERE CarrierName LIKE '"+POut.String(carrierName)+"%') carrierA "
				+"INNER JOIN insplan ON insplan.CarrierNum = carrierA.CarrierNum "
				+"INNER JOIN claim ON insplan.PlanNum = claim.PlanNum "
				+"INNER JOIN claimproc ON claimproc.ClaimNum = claim.ClaimNum "
				+"INNER JOIN patient ON patient.PatNum = claimproc.PatNum "
					+"AND patient.LName LIKE '"+POut.String(LName)+"%'"
					+"AND patient.FName LIKE '"+POut.String(FName)+"%'"
				+"LEFT JOIN clinic ON clinic.ClinicNum = claimproc.ClinicNum "
				+"WHERE (claim.ClaimStatus = 'S' "
					+"OR (claim.ClaimStatus='R' AND claimproc.InsPayAmt!=0)) "//certain (very few) received claims will have payment amounts entered but not attached to payment
				+"AND ClaimType != 'PreAuth' AND claimproc.ClaimPaymentNum=0 ";
			if(DataConnection.DBtype==DatabaseType.MySql) {
				command+="GROUP BY claim.ClaimNum ";
			}
			else {//oracle
				command+="GROUP BY claim.DateService,claim.ProvTreat,"+DbHelper.Concat("patient.LName","', '","patient.FName")+","
					+"carrierA.CarrierName,ClaimFee,claim.ClaimNum,claimproc.ClaimPaymentNum,Description,claim.PatNum,PaymentRow ";
			}
			command+="ORDER BY CarrierName,patName_";
			DataTable table=Db.GetTable(command);
			return ClaimPaySplitTableToList(table);
		}

		/// <summary>Gets all 'claims' attached to the claimpayment.</summary>
		public static List<ClaimPaySplit> GetAttachedToPayment(long claimPaymentNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClaimPaySplit>>(MethodBase.GetCurrentMethod(),claimPaymentNum);
			}
			string command=
				"SELECT claim.DateService,claim.ProvTreat,"+DbHelper.Concat("patient.LName","', '","patient.FName")+" patName_,"
				+"carrier.CarrierName,ClaimFee feeBilled_,SUM(claimproc.InsPayAmt) insPayAmt_,claim.ClaimNum,"
				+"claimproc.ClaimPaymentNum,clinic.Description,claim.PatNum,PaymentRow "
				+" FROM claim,patient,insplan,carrier,claimproc"
				+" LEFT JOIN clinic ON clinic.ClinicNum = claimproc.ClinicNum"
				+" WHERE claimproc.ClaimNum = claim.ClaimNum"
				+" AND patient.PatNum = claim.PatNum"
				+" AND insplan.PlanNum = claim.PlanNum"
				+" AND insplan.CarrierNum = carrier.CarrierNum"
				+" AND claimproc.ClaimPaymentNum = "+claimPaymentNum+" ";
			if(DataConnection.DBtype==DatabaseType.MySql) {
				command+="GROUP BY claim.ClaimNum ";
			}
			else {//oracle
				command+="GROUP BY claim.DateService,claim.ProvTreat,"+DbHelper.Concat("patient.LName","', '","patient.FName")
					+",carrier.CarrierName,claim.ClaimNum,claimproc.ClaimPaymentNum,claim.PatNum,ClaimFee,clinic.Description,PaymentRow ";
			}
			command+="ORDER BY claimproc.PaymentRow";
			DataTable table=Db.GetTable(command);
			return ClaimPaySplitTableToList(table);
		}

		/// <summary>Gets all secondary claims for the related ClaimPaySplits. Called after a payment has been received.</summary>
		public static DataTable GetSecondaryClaims(List<ClaimPaySplit> claimsAttached) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),claimsAttached);
			}
			string command="SELECT DISTINCT ProcNum FROM claimproc WHERE ClaimNum IN (";
			string claimNums="";//used twice
			for(int i=0;i<claimsAttached.Count;i++) {
				if(i>0) {
					claimNums+=",";
				}
				claimNums+=claimsAttached[i].ClaimNum;
			}
			command+=claimNums+") AND ProcNum!=0";
			//List<ClaimProc> tempClaimProcs=ClaimProcCrud.SelectMany(command);
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0) {
				return new DataTable();//No procedures are attached to these claims.  This frequently happens in conversions.  No need to look for related secondary claims.
			}
			command="SELECT claimproc.PatNum,claimproc.ProcDate"
				+" FROM claimproc"
				+" JOIN claim ON claimproc.ClaimNum=claim.ClaimNum"
				+" WHERE ProcNum IN (";
			for(int i=0;i<table.Rows.Count;i++) {
				if(i>0) {
					command+=",";
				}
				command+=table.Rows[i]["ProcNum"].ToString();
			}
			command+=") AND claimproc.ClaimNum NOT IN ("+claimNums+")"
				+" AND ClaimType = 'S'"
				+" GROUP BY claimproc.ClaimNum,claimproc.PatNum,claimproc.ProcDate";
			DataTable secondaryClaims=Db.GetTable(command);
			return secondaryClaims;
		}

		///<summary></summary>
		public static List<ClaimPaySplit> GetInsPayNotAttachedForFixTool() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClaimPaySplit>>(MethodBase.GetCurrentMethod());
			}
			string command=
				"SELECT claim.DateService,claim.ProvTreat,CONCAT(CONCAT(patient.LName,', '),patient.FName) patName_"
				+",carrier.CarrierName,SUM(claimproc.FeeBilled) feeBilled_,SUM(claimproc.InsPayAmt) insPayAmt_,claim.ClaimNum"
				+",claimproc.ClaimPaymentNum,(SELECT clinic.Description FROM clinic WHERE claimproc.ClinicNum = clinic.ClinicNum) Description,claim.PatNum,PaymentRow "
				+" FROM claim,patient,insplan,carrier,claimproc"
				+" WHERE claimproc.ClaimNum = claim.ClaimNum"
				+" AND patient.PatNum = claim.PatNum"
				+" AND insplan.PlanNum = claim.PlanNum"
				+" AND insplan.CarrierNum = carrier.CarrierNum"
				+" AND (claimproc.Status = '1' OR claimproc.Status = '4' OR claimproc.Status=5)"//received or supplemental or capclaim
				+" AND (claimproc.InsPayAmt != 0 AND claimproc.ClaimPaymentNum = '0')"
				+" GROUP BY claim.DateService,claim.ProvTreat,CONCAT(CONCAT(patient.LName,', '),patient.FName)"
				+",carrier.CarrierName,claim.ClaimNum,claimproc.ClaimPaymentNum,claim.PatNum"
				+" ORDER BY patName_";
			DataTable table=Db.GetTable(command);
			return ClaimPaySplitTableToList(table);
		}

		///<summary></summary>
		private static List<ClaimPaySplit> ClaimPaySplitTableToList(DataTable table) {
			//No need to check RemotingRole; no call to db.
			List<ClaimPaySplit> splits=new List<ClaimPaySplit>();
			ClaimPaySplit split;
			for(int i=0;i<table.Rows.Count;i++){
				split=new ClaimPaySplit();
				split.DateClaim      =PIn.Date  (table.Rows[i]["DateService"].ToString());
				split.ProvAbbr       =Providers.GetAbbr(PIn.Long(table.Rows[i]["ProvTreat"].ToString()));
				split.PatName        =PIn.String(table.Rows[i]["patName_"].ToString());
				split.PatNum         =PIn.Long  (table.Rows[i]["PatNum"].ToString());
				split.Carrier        =PIn.String(table.Rows[i]["CarrierName"].ToString());
				split.FeeBilled      =PIn.Double(table.Rows[i]["feeBilled_"].ToString());
				split.InsPayAmt      =PIn.Double(table.Rows[i]["insPayAmt_"].ToString());
				split.ClaimNum       =PIn.Long  (table.Rows[i]["ClaimNum"].ToString());
				split.ClaimPaymentNum=PIn.Long  (table.Rows[i]["ClaimPaymentNum"].ToString());
				split.PaymentRow     =PIn.Int   (table.Rows[i]["PaymentRow"].ToString());
				split.ClinicDesc		 =PIn.String(table.Rows[i]["Description"].ToString());
				splits.Add(split);
			}
			return splits;
		}

		///<summary>Gets the specified claim from the database.  Can be null.</summary>
		public static Claim GetClaim(long claimNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Claim>(MethodBase.GetCurrentMethod(),claimNum);
			}
			string command="SELECT * FROM claim"
				+" WHERE ClaimNum = "+claimNum.ToString();
			Claim retClaim=Crud.ClaimCrud.SelectOne(command);
			if(retClaim==null){
				return null;
			}
			command="SELECT * FROM claimattach WHERE ClaimNum = "+POut.Long(claimNum);
			retClaim.Attachments=Crud.ClaimAttachCrud.SelectMany(command);
			return retClaim;
		}

		///<summary>Gets all claims for the specified patient. But without any attachments.</summary>
		public static List<Claim> Refresh(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Claim>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command=
				"SELECT * FROM claim"
				+" WHERE PatNum = "+patNum.ToString()
				+" ORDER BY dateservice";
			return Crud.ClaimCrud.SelectMany(command);
		}

		public static Claim GetFromList(List<Claim> list,long claimNum) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<list.Count;i++) {
				if(list[i].ClaimNum==claimNum) {
					return list[i].Copy();
				}
			}
			return null;
		}

		///<summary></summary>
		public static long Insert(Claim claim) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				claim.ClaimNum=Meth.GetLong(MethodBase.GetCurrentMethod(),claim);
				return claim.ClaimNum;
			}
			return Crud.ClaimCrud.Insert(claim);
		}

		///<summary></summary>
		public static void Update(Claim claim){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),claim);
				return;
			}
			Crud.ClaimCrud.Update(claim);
			//now, delete all attachments and recreate.
			string command="DELETE FROM claimattach WHERE ClaimNum="+POut.Long(claim.ClaimNum);
			Db.NonQ(command);
			for(int i=0;i<claim.Attachments.Count;i++) {
				claim.Attachments[i].ClaimNum=claim.ClaimNum;
				ClaimAttaches.Insert(claim.Attachments[i]);
			}
		}

		///<summary></summary>
		public static void Delete(Claim claim){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),claim);
				return;
			}
			Crud.ClaimCrud.Delete(claim.ClaimNum);
		}

		///<summary></summary>
		public static void DetachProcsFromClaim(Claim Cur){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),Cur);
				return;
			}
			string command = "UPDATE procedurelog SET "
				+"claimnum = '0' "
				+"WHERE claimnum = '"+POut.Long(Cur.ClaimNum)+"'";
			//MessageBox.Show(string command);
			Db.NonQ(command);
		}

		/*
		///<summary>Called from claimsend window and from Claim edit window.  Use 0 to get all waiting claims, or an actual claimnum to get just one claim.</summary>
		public static ClaimSendQueueItem[] GetQueueList(){
			return GetQueueList(0,0);
		}*/

		///<summary>Called from claimsend window and from Claim edit window.  Use 0 to get all waiting claims, or an actual claimnum to get just one claim.</summary>
		public static ClaimSendQueueItem[] GetQueueList(long claimNum,long clinicNum,long customTracking) {
			List<long> listClaimNums=new List<long>();
			if(claimNum!=0) {
				listClaimNums.Add(claimNum);
			}
			return GetQueueList(listClaimNums,clinicNum,customTracking);
		}

		///<summary>Called from claimsend window and from Claim edit window.  Use an empty listClaimNums to get all waiting claims.</summary>
		public static ClaimSendQueueItem[] GetQueueList(List<long> listClaimNums,long clinicNum,long customTracking) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<ClaimSendQueueItem[]>(MethodBase.GetCurrentMethod(),listClaimNums,clinicNum,customTracking);
			}
			string command=
				"SELECT claim.ClaimNum,carrier.NoSendElect"
				+",CONCAT(CONCAT(CONCAT(concat(patient.LName,', '),patient.FName),' '),patient.MiddleI)"
				+",claim.ClaimStatus,carrier.CarrierName,patient.PatNum,carrier.ElectID,MedType,claim.DateService,claim.ClinicNum,claim.CustomTracking "
				+"FROM claim "
				+"Left join insplan on claim.PlanNum = insplan.PlanNum "
				+"Left join carrier on insplan.CarrierNum = carrier.CarrierNum "
				+"Left join patient on patient.PatNum = claim.PatNum ";
			if(listClaimNums.Count==0){
				command+="WHERE (claim.ClaimStatus = 'W' OR claim.ClaimStatus = 'P') ";
			}
			else{
				command+="WHERE claim.ClaimNum IN("+string.Join(",",listClaimNums)+") ";
			}
			if(clinicNum>0) {
				command+="AND claim.ClinicNum="+POut.Long(clinicNum)+" ";
			}
			if(customTracking>0) {
				command+="AND claim.CustomTracking="+POut.Long(customTracking)+" ";
			}
			command+="ORDER BY claim.DateService,patient.LName";
			DataTable table=Db.GetTable(command);
			ClaimSendQueueItem[] listQueue=new ClaimSendQueueItem[table.Rows.Count];
			for(int i=0;i<table.Rows.Count;i++){
				listQueue[i]=new ClaimSendQueueItem();
				listQueue[i].ClaimNum        = PIn.Long  (table.Rows[i][0].ToString());
				listQueue[i].NoSendElect     = PIn.Bool  (table.Rows[i][1].ToString());
				listQueue[i].PatName         = PIn.String(table.Rows[i][2].ToString());
				listQueue[i].ClaimStatus     = PIn.String(table.Rows[i][3].ToString());
				listQueue[i].Carrier         = PIn.String(table.Rows[i][4].ToString());
				listQueue[i].PatNum          = PIn.Long  (table.Rows[i][5].ToString());
				string payorID=PIn.String(table.Rows[i]["ElectID"].ToString());
				EnumClaimMedType medType=(EnumClaimMedType)PIn.Int(table.Rows[i]["MedType"].ToString());
				listQueue[i].ClearinghouseNum=Clearinghouses.AutomateClearinghouseHqSelection(payorID,medType);
				listQueue[i].MedType=medType;
				listQueue[i].DateService     = PIn.Date  (table.Rows[i]["DateService"].ToString());
				listQueue[i].ClinicNum		 = PIn.Long	 (table.Rows[i]["ClinicNum"].ToString());
				listQueue[i].CustomTracking		= PIn.Long (table.Rows[i]["CustomTracking"].ToString());
			}
			return listQueue;
		}

		///<summary>Supply claimnums. Called from X12 to begin the sorting process on claims going to one clearinghouse.</summary>
		public static List<X12TransactionItem> GetX12TransactionInfo(long claimNum) {
			//No need to check RemotingRole; no call to db.
			List<long> claimNums=new List<long>();
			claimNums.Add(claimNum);
			return GetX12TransactionInfo(claimNums);
		}

		///<summary>Supply claimnums. Called from X12 to begin the sorting process on claims going to one clearinghouse.</summary>
		public static List<X12TransactionItem> GetX12TransactionInfo(List<long> claimNums) {//ArrayList queueItemss){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<X12TransactionItem>>(MethodBase.GetCurrentMethod(),claimNums);
			}
			List<X12TransactionItem> retVal=new List<X12TransactionItem>();
			if(claimNums.Count<1) {
				return retVal;
			}
			string command;
			command="SELECT carrier.ElectID,claim.ProvBill,inssub.Subscriber,"
				+"claim.PatNum,claim.ClaimNum,CASE WHEN inssub.Subscriber!=claim.PatNum THEN 1 ELSE 0 END AS subscNotPatient "
				+"FROM claim,insplan,inssub,carrier "
				+"WHERE claim.PlanNum=insplan.PlanNum "
				+"AND claim.InsSubNum=inssub.InsSubNum "
				+"AND carrier.CarrierNum=insplan.CarrierNum "
				+"AND claim.ClaimNum IN ("+String.Join(",",claimNums)+") "
				+"ORDER BY carrier.ElectID,claim.ProvBill,inssub.Subscriber,subscNotPatient,claim.PatNum";
			DataTable table=Db.GetTable(command);
			//object[,] myA=new object[5,table.Rows.Count];
			X12TransactionItem item;
			for(int i=0;i<table.Rows.Count;i++){
				item=new X12TransactionItem();
				item.PayorId0=PIn.String(table.Rows[i][0].ToString());
				item.ProvBill1=PIn.Long   (table.Rows[i][1].ToString());
				item.Subscriber2=PIn.Long   (table.Rows[i][2].ToString());
				item.PatNum3=PIn.Long   (table.Rows[i][3].ToString());
				item.ClaimNum4=PIn.Long   (table.Rows[i][4].ToString());
				retVal.Add(item);
			}
			return retVal;
		}

		///<summary>Also sets the DateSent to today.</summary>
		public static void SetCanadianClaimSent(long claimNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),claimNum);
				return;
			}
			string command="UPDATE claim SET ClaimStatus = 'S',"
					+"DateSent= "+POut.Date(MiscData.GetNowDateTime())
					+" WHERE ClaimNum = "+POut.Long(claimNum);
			Db.NonQ(command);
		}

		public static bool IsClaimIdentifierInUse(string claimIdentifier,long claimNumExclude,string claimType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),claimIdentifier,claimNumExclude);
			}
			string command="SELECT COUNT(*) FROM claim WHERE ClaimIdentifier='"+POut.String(claimIdentifier)+"' AND ClaimNum<>"+POut.Long(claimNumExclude);
			if(claimType=="PreAuth") {
				command+=" AND ClaimType='PreAuth'";
			}
			else {
				command+=" AND ClaimType!='PreAuth'";
			}
			return (Db.GetTable(command).Rows[0][0].ToString()!="0");
		}

		///<summary>Returns the claim with the specified fee and dates of service.  The returned claim will also either begin with the specified claimIdentifier, or
		///will be for the patient name and subscriber ID specified.  If no match was found, or multiple matches were found, then null is returned.</summary>
		public static Claim GetClaimFromX12(string claimIdentifier,double claimFee,DateTime dateServiceStart,DateTime dateServiceEnd,string patFname,string patLname,string subscriberId) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Claim>(MethodBase.GetCurrentMethod(),claimIdentifier,claimFee,dateServiceStart,dateServiceEnd,patFname,patLname,subscriberId);
			}
			if(dateServiceStart.Year<1880 || dateServiceEnd.Year<1880) {
				//Service dates are required for us to continue.
				//In 227s, the claim dates of service are required and should be present.
				//In 835s, pull the procedure dates up into the claim dates of service if the claim dates are of service are not present.
				return null;
			}
			//We always require the claim fee and dates of service to match, then we use other criteria below to wisely choose from the shorter list of claims.
			//The list of claims with matching fee and date of service should be very short.  Worst case, the list would contain all of the appointments for a few days if every claim had the same fee (rare).
			string command="SELECT claim.ClaimNum,claim.ClaimIdentifier,claim.ClaimStatus,patient.LName,patient.FName,inssub.SubscriberID "
				+"FROM claim "
				+"INNER JOIN patient ON patient.PatNum=claim.PatNum "
				+"INNER JOIN inssub ON inssub.InsSubNum=claim.InsSubNum AND claim.PlanNum=inssub.PlanNum "
				+"WHERE ROUND(ClaimFee,2)="+POut.Double(claimFee)+" AND "+DbHelper.DtimeToDate("DateService")+">="+POut.Date(dateServiceStart)+" AND "+DbHelper.DtimeToDate("DateService")+"<="+POut.Date(dateServiceEnd);
			DataTable tableClaims=Db.GetTable(command);
			if(tableClaims.Rows.Count==0) {
				return null;//No matches found for the specific claim fee and date of service.  Aboloutely no suitable matches.
			}
			//Look for a single exact match by claim identifier.  This step is first, so that the user can override claim association to the 835 or 277 by changing the claim identifier if desired.
			List<int> listIndiciesForIdentifier=new List<int>();
			for(int i=0;i<tableClaims.Rows.Count;i++) {
				string claimId=PIn.String(tableClaims.Rows[i]["ClaimIdentifier"].ToString());
				if(claimId==claimIdentifier) {
					listIndiciesForIdentifier.Add(i);
				}
			}
			if(listIndiciesForIdentifier.Count==0 && claimIdentifier.Length>15) {//No exact match found.  Look for similar claim identifiers if the identifer was possibly truncated when sent out.
				//Our claim identifiers can be longer than 20 characters (mostly when using replication). When the claim identifier is sent out on the claim, it is truncated to 20
				//characters. Therefore, if the claim identifier is longer than 20 characters, then it was truncated when sent out, so we have to look for claims beginning with the 
				//claim identifier given if there is not an exact match.  We also send shorter identifiers for some clearinghouses.  For example, the maximum claim identifier length
				//for Denti-Cal is 17 characters.
				for(int i=0;i<tableClaims.Rows.Count;i++) {
					string claimId=PIn.String(tableClaims.Rows[i]["ClaimIdentifier"].ToString());
					if(claimId.StartsWith(claimIdentifier)) {
						listIndiciesForIdentifier.Add(i);
					}
				}
			}
			if(listIndiciesForIdentifier.Count==0) {
				//No matches were found for the identifier.  Continue to more advanced matching below.
			}
			else if(listIndiciesForIdentifier.Count==1) {
				//A single match based on claim identifier, claim date of service, and claim fee.
				long claimNum=PIn.Long(tableClaims.Rows[listIndiciesForIdentifier[0]]["ClaimNum"].ToString());
				return Claims.GetClaim(claimNum);
			}
			else if(listIndiciesForIdentifier.Count>1) {//Edge case.
				//Multiple matches for the specified claim identifier AND date service AND fee.  The claim must have been split (rare because the split claims must have the same fee).
				//Continue to more advanced matching below, although it probably will not help.  We could enhance this specific scenario by picking a claim based on the procedures attached, but that is not a guarantee either.
			}
			//Locate claims exactly matching patient last name.
			List<DataRow> listMatches=new List<DataRow>();
			patLname=patLname.Trim().ToLower();
			for(int i=0;i<tableClaims.Rows.Count;i++) {
				string lastNameInDb=PIn.String(tableClaims.Rows[i]["LName"].ToString()).Trim().ToLower();
				if(lastNameInDb==patLname) {
					listMatches.Add(tableClaims.Rows[i]);
				}
			}
			//Locate claims matching exact first name or partial first name, with a preference for exact match.
			List<DataRow> listExactFirst=new List<DataRow>();
			List<DataRow> listPartFirst=new List<DataRow>();
			patFname=patFname.Trim().ToLower();
			for(int i=0;i<listMatches.Count;i++) {
				string firstNameInDb=PIn.String(listMatches[i]["FName"].ToString()).Trim().ToLower();
				if(firstNameInDb==patFname) {
					listExactFirst.Add(listMatches[i]);
				}
				else if(firstNameInDb.Length>=2 && patFname.StartsWith(firstNameInDb)) {
					//Unfortunately, in the real world, we have observed carriers returning the patients first name followed by a space followed by the patient middle name all within the first name field.
					//This issue is probably due to human error when the carrier's staff typed the patient name into their system.  All we can do is try to cope with this situation.
					listPartFirst.Add(listMatches[i]);
				}
			}
			if(listExactFirst.Count>0) {
				listMatches=listExactFirst;//One or more exact matches found.  Ignore any partial matches.
			}
			else {
				listMatches=listPartFirst;//Use partial matches only if no exact matches were found.
			}
			//Locate claims matching exact subscriber ID or partial subscriber ID, with a preference for exact match.
			List<DataRow> listExactId=new List<DataRow>();
			List<DataRow> listPartId=new List<DataRow>();
			subscriberId=subscriberId.Trim();
			for(int i=0;i<listMatches.Count;i++) {
				string subIdInDb=PIn.String(listMatches[i]["SubscriberID"].ToString()).Trim();
				if(subIdInDb==subscriberId) {
					listExactId.Add(listMatches[i]);
				}
				else if(subIdInDb.Length>=3 && (subscriberId==subIdInDb.Substring(0,subIdInDb.Length-1) || subscriberId==subIdInDb.Substring(0,subIdInDb.Length-2))) {
					//Partial subscriber ID matches are somewhat common.
					//Insurance companies sometimes create a base subscriber ID for all family members, then append a one or two digit number to make IDs unique for each family member.
					//We have seen at least one real world example where the ERA contained the base subscriber ID instead of the patient specific ID.
					//We also check that the subscriber ID in OD is at least 3 characters long, because we must allow for the 2 optional ending characters and we require an extra leading character to avoid matching blank IDs.
					listPartId.Add(listMatches[i]);
				}
				else if(subscriberId.Length>=3 && (subIdInDb==subscriberId.Substring(0,subscriberId.Length-1) || subIdInDb==subscriberId.Substring(0,subscriberId.Length-2))) {
					//Partial match in the other direction.  Comparable to the scenario above.
					listPartId.Add(listMatches[i]);
				}
			}
			if(listExactId.Count>0) {
				listMatches=listExactId;//One or more exact matches found.  Ignore any partial matches.
			}
			else {
				listMatches=listPartId;//Use partial matches only if no exact matches were found.
			}
			//We have finished locating the matches.  Now decide what to do based on the number of matches found.
			if(listMatches.Count==0) {
				return null;//No suitable matches.
			}
			else if(listMatches.Count==1) {
				//A single match based on patient first name, patient last name, subscriber ID, claim date of service, and claim fee.
				long claimNum=PIn.Long(listMatches[0]["ClaimNum"].ToString());
				return Claims.GetClaim(claimNum);
			}
			else if(listMatches.Count>1) {//Edge case.
				//Multiple matches (rare).  We might be able to pick the correct claim based on the attached procedures, but we can worry about this situation later if it happens more than we expect.
			}
			return null;
		}

		///<summary>Returns the number of received claims attached to specified insplan.</summary>
		public static int GetCountReceived(long planNum) {
			//No need to check RemotingRole; no call to db.
			return GetCountReceived(planNum,0);
		}

		///<summary>Returns the number of received claims attached to specified subscriber with specified insplan.  Set insSubNum to zero to check all claims for all patients for the plan.</summary>
		public static int GetCountReceived(long planNum,long insSubNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod(),planNum,insSubNum);
			}
			string command;
			command="SELECT COUNT(*) "
				+"FROM claim "
				+"WHERE claim.ClaimStatus='R' "
				+"AND claim.PlanNum="+POut.Long(planNum)+" ";
			if(insSubNum!=0) {
				command+="AND claim.InsSubNum="+POut.Long(insSubNum);
			}
			return PIn.Int(Db.GetCount(command));
		}

		///<summary>Returns a human readable ClaimStatus string.</summary>
		public static string GetClaimStatusString(string claimStatus) {
			string retVal="";
			switch(claimStatus){
				case "U":
					retVal="Unsent";
					break;
				case "H":
					retVal="Hold until Pri received";
					break;
				case "W":
					retVal="Waiting to Send";
					break;
				case "P":
					retVal="Probably Sent";
					break;
				case "S":
					retVal="Sent - Verified";
					break;
				case "R":
					retVal="Received";
					break;
			}
			return retVal;
		}

		///<summary>Updates ClaimIdentifier for specified claim.</summary>
		public static void UpdateClaimIdentifier(long claimNum,string claimIdentifier) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),claimNum,claimIdentifier);
				return;
			}
			string command="UPDATE claim SET ClaimIdentifier="+POut.String(claimIdentifier)+" WHERE ClaimNum="+POut.Long(claimNum);
			Db.NonQ(command);
		}

		///<summary>Zeros securitylog FKey column for rows that are using the matching claimNum as FKey and are related to Claim.
		///Permtypes are generated from the AuditPerms property of the CrudTableAttribute within the Claim table type.</summary>
		public static void ClearFkey(long claimNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),claimNum);
				return;
			}
			Crud.ClaimCrud.ClearFkey(claimNum);
		}

		///<summary>Zeros securitylog FKey column for rows that are using the matching claimNums as FKey and are related to Claim.
		///Permtypes are generated from the AuditPerms property of the CrudTableAttribute within the Claim table type.</summary>
		public static void ClearFkey(List<long> listClaimNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listClaimNums);
				return;
			}
			Crud.ClaimCrud.ClearFkey(listClaimNums);
		}
	}//end class Claims

	///<summary>This is an odd class.  It holds data for the X12 (4010 only) generation process.  It replaces an older multi-dimensional array, so the names are funny, but helpful to prevent bugs.  Not an actual database table.</summary>
	public class X12TransactionItem{
		public string PayorId0;
		public long ProvBill1;
		public long Subscriber2;
		public long PatNum3;
		public long ClaimNum4;
	}

	///<summary>Holds a list of claims to show in the claims 'queue' waiting to be sent.  Not an actual database table.</summary>
	public class ClaimSendQueueItem{
		///<summary></summary>
		public long ClaimNum;
		///<summary></summary>
		public bool NoSendElect;
		///<summary></summary>
		public string PatName;
		///<summary>Single char: U,H,W,P,S,or R.</summary>
		///<remarks>U=Unsent, H=Hold until pri received, W=Waiting in queue, P=Probably sent, S=Sent, R=Received.  A(adj) is no longer used.</remarks>
		public string ClaimStatus;
		///<summary></summary>
		public string Carrier;
		///<summary></summary>
		public long PatNum;
		///<summary>ClearinghouseNum of HQ.</summary>
		public long ClearinghouseNum;
		///<summary></summary>
		public long ClinicNum;
		///<summary>Enum:EnumClaimMedType 0=Dental, 1=Medical, 2=Institutional</summary>
		public EnumClaimMedType MedType;
		///<summary></summary>
		public string MissingData;
		///<summary></summary>
		public string Warnings;
		///<summary></summary>
		public DateTime DateService;
		///<summary>False by default.  For speed purposes, claims should only be validated once, which is just before they are sent.</summary>
		public bool IsValid;
		/// <summary>Used to save what tracking is used for filtering.</summary>
		public long CustomTracking;

		public ClaimSendQueueItem Copy(){
			return (ClaimSendQueueItem)MemberwiseClone();
		}
	}

	///<summary>Holds a list of claims to show in the Claim Pay Edit window.  Not an actual database table.</summary>
	public class ClaimPaySplit{
		///<summary></summary>
		public long ClaimNum;
		///<summary></summary>
		public string PatName;
		///<summary></summary>
		public long PatNum;
		///<summary></summary>
		public string Carrier;
		///<summary></summary>
		public DateTime DateClaim;
		///<summary></summary>
		public string ProvAbbr;
		///<summary></summary>
		public double FeeBilled;
		///<summary></summary>
		public double InsPayAmt;
		///<summary></summary>
		public long ClaimPaymentNum;
		///<summary>1-based</summary>
		public int PaymentRow;
		///<summary></summary>
		public string ClinicDesc;
	}
	
}