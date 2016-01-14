using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ClaimPayments {

		///<summary></summary>
		public static DataTable GetForClaim(long claimNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),claimNum);
			}
			DataTable table=new DataTable();
			DataRow row;
			table.Columns.Add("amount");
			table.Columns.Add("payType");
			table.Columns.Add("BankBranch");
			table.Columns.Add("ClaimPaymentNum");
			table.Columns.Add("checkDate");
			table.Columns.Add("CheckNum");
			table.Columns.Add("Note");
			List<DataRow> rows=new List<DataRow>();
			string command="SELECT BankBranch,claimpayment.ClaimPaymentNum,CheckNum,CheckDate,"
				+"SUM(claimproc.InsPayAmt) amount,Note,PayType "
				+"FROM claimpayment,claimproc "
				+"WHERE claimpayment.ClaimPaymentNum = claimproc.ClaimPaymentNum "
				+"AND claimproc.ClaimNum = '"+POut.Long(claimNum)+"' "
				+"GROUP BY claimpayment.ClaimPaymentNum, BankBranch, CheckDate, CheckNum, Note, PayType";
			DataTable rawT=Db.GetTable(command);
			DateTime date;
			for(int i=0;i<rawT.Rows.Count;i++) {
				row=table.NewRow();
				row["amount"]=PIn.Double(rawT.Rows[i]["amount"].ToString()).ToString("F");
				row["payType"]=DefC.GetName(DefCat.InsurancePaymentType,PIn.Long(rawT.Rows[i]["PayType"].ToString()));
				row["BankBranch"]=rawT.Rows[i]["BankBranch"].ToString();
				row["ClaimPaymentNum"]=rawT.Rows[i]["ClaimPaymentNum"].ToString();
				date=PIn.Date(rawT.Rows[i]["CheckDate"].ToString());
				row["checkDate"]=date.ToShortDateString();
				row["CheckNum"]=rawT.Rows[i]["CheckNum"].ToString();
				row["Note"]=rawT.Rows[i]["Note"].ToString();
				rows.Add(row);
			}
			for(int i=0;i<rows.Count;i++) {
				table.Rows.Add(rows[i]);
			}
			return table;
		}
		
		///<summary>Gets all claimpayments within the specified date range and from the specified clinic. 0 means all clinics selected.</summary>
		public static List<ClaimPayment> GetForDateRange(DateTime dateFrom,DateTime dateTo,long clinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClaimPayment>>(MethodBase.GetCurrentMethod(),dateFrom,dateTo,clinicNum);
			}
			string command="SELECT * FROM claimpayment "
				//"SELECT ClaimPaymentNum,CheckDate,CheckAmt,"
				//+"Checknum,BankBranch,Note,DepositNum,"
				//+"ClinicNum,DepositNum,CarrierName,DateIssued "
				//+"FROM claimpayment "
				+"WHERE CheckDate >= "+POut.Date(dateFrom)
				+"AND CheckDate <= "+POut.Date(dateTo);
			if(clinicNum!=0){
				command+=" AND ClinicNum="+POut.Long(clinicNum);
			}
			command+=" ORDER BY CheckDate";
			return Crud.ClaimPaymentCrud.SelectMany(command);
		}

		///<summary>Gets all unattached claimpayments for display in a new deposit.  Excludes payments before dateStart and partials.</summary>
		public static ClaimPayment[] GetForDeposit(DateTime dateStart,long clinicNum,List<long> insPayTypes) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<ClaimPayment[]>(MethodBase.GetCurrentMethod(),dateStart,clinicNum,insPayTypes);
			}
			string command=
				"SELECT * FROM claimpayment "
				+"INNER JOIN definition ON claimpayment.PayType=definition.DefNum "
				+"WHERE DepositNum = 0 "
				+"AND definition.ItemValue='' "//Check if payment type should show in the deposit slip.  'N'=not show, empty string means should show.
				+"AND CheckDate >= "+POut.Date(dateStart);
			if(clinicNum!=0){
				command+=" AND ClinicNum="+POut.Long(clinicNum);
			}
			for(int i=0;i<insPayTypes.Count;i++) {
				if(i==0) {
					command+=" AND PayType IN ("+insPayTypes[0];
				}
				else {
					command+=","+insPayTypes[i];
				}
				if(i==insPayTypes.Count-1) {
					command+=")";
				}
			}
			command+=" AND IsPartial=0"//Don't let users attach partial insurance payments to deposits.
				//Order by the date on the check, and then the incremental order of the creation of each payment (doesn't affect random primary keys).
				//It was an internal complaint that checks on the same date show up in a 'random' order.
				//The real fix for this issue would be to add a time column and order by it by that instead of the PK.
				+" ORDER BY CheckDate,ClaimPaymentNum";//Not usual pattern to order by PK
			return Crud.ClaimPaymentCrud.SelectMany(command).ToArray();
		}

		///<summary>Gets all claimpayments for one specific deposit.</summary>
		public static ClaimPayment[] GetForDeposit(long depositNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<ClaimPayment[]>(MethodBase.GetCurrentMethod(),depositNum);
			}
			string command=
				"SELECT * FROM claimpayment "
				+"INNER JOIN definition ON claimpayment.PayType=definition.DefNum "
				+"WHERE DepositNum = "+POut.Long(depositNum)
				+" AND definition.ItemValue=''"//Check if payment type should show in the deposit slip.  'N'=not show, empty string means should show.
				//Order by the date on the check, and then the incremental order of the creation of each payment (doesn't affect random primary keys).
				//It was an internal complaint that checks on the same date show up in a 'random' order.
				//The real fix for this issue would be to add a time column and order by it by that instead of the PK.
				+" ORDER BY CheckDate,ClaimPaymentNum";//Not usual pattern to order by PK
			return Crud.ClaimPaymentCrud.SelectMany(command).ToArray();
		}

		///<summary>Gets one claimpayment directly from database.</summary>
		public static ClaimPayment GetOne(long claimPaymentNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<ClaimPayment>(MethodBase.GetCurrentMethod(),claimPaymentNum);
			}
			string command=
				"SELECT * FROM claimpayment "
				+"WHERE ClaimPaymentNum = "+POut.Long(claimPaymentNum);
			return Crud.ClaimPaymentCrud.SelectOne(command);
		}

		///<summary>Returns true if there is a partial batch insurance payment or if there are any claimprocs with status Received that have InsPayAmts but are not associated to a claim payment.  Used to warn users that reports will be inaccurate until insurance payments are finalized.</summary>
		public static bool HasPartialPayments() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod());
			}
			//Union together two queries that look for incomplete insurance payments.
			//The first query will look for any partial batch insurance payments.
			//The second query will look for any claim payments (InsPayAmt > 0) that do not have a claim payment (no check / not finalized).
			string command=@"
				SELECT COUNT(*) 
				FROM ((SELECT claimpayment.ClaimPaymentNum 
					FROM claimpayment 
					WHERE claimpayment.IsPartial = 1 
					AND claimpayment.CheckDate <= "+POut.Date(DateTime.Now)+@" 
					AND claimpayment.CheckDate >= "+POut.Date(DateTime.Now.AddMonths(-1))+@")
						UNION ALL
					(SELECT claimproc.ClaimProcNum 
					FROM claimproc
					WHERE claimproc.ClaimPaymentNum = 0
					AND claimproc.InsPayAmt != 0 
					AND claimproc.Status IN("+POut.Int((int)ClaimProcStatus.Received)+","+POut.Int((int)ClaimProcStatus.Supplemental)+","+POut.Int((int)ClaimProcStatus.CapClaim)+@") 
					AND claimproc.DateEntry <= "+POut.Date(DateTime.Now)+@" 
					AND claimproc.DateEntry >= "+POut.Date(DateTime.Now.AddMonths(-1))+@")
				) partialpayments";//claimproc.DateEntry is updated when payment is received.
			if(Db.GetCount(command)!="0") {
				return true;
			}
			return false;
		}

		///<summary></summary>
		public static long Insert(ClaimPayment cp) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				cp.SecUserNumEntry=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				cp.ClaimPaymentNum=Meth.GetLong(MethodBase.GetCurrentMethod(),cp);
				return cp.ClaimPaymentNum;
			}
			return Crud.ClaimPaymentCrud.Insert(cp);
		}

		///<summary>If trying to change the amount and attached to a deposit, it will throw an error, so surround with try catch.</summary>
		public static void Update(ClaimPayment cp){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),cp);
				return;
			}
			string command="SELECT DepositNum,CheckAmt FROM claimpayment "
				+"WHERE ClaimPaymentNum="+POut.Long(cp.ClaimPaymentNum);
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0){
				return;
			}
			if(table.Rows[0][0].ToString()!="0"//if claimpayment is already attached to a deposit
				&& PIn.Double(table.Rows[0][1].ToString())!=cp.CheckAmt)//and checkAmt changes
			{
				throw new ApplicationException(Lans.g("ClaimPayments","Not allowed to change the amount on checks attached to deposits."));
			}
			Crud.ClaimPaymentCrud.Update(cp);
		}

		///<summary>Surround by try catch, because it will throw an exception if trying to delete a claimpayment attached to a deposit or if there are eobs attached.</summary>
		public static void Delete(ClaimPayment cp){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),cp);
				return;
			}
			//validate deposits
			string command="SELECT DepositNum FROM claimpayment "
				+"WHERE ClaimPaymentNum="+POut.Long(cp.ClaimPaymentNum);
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0){
				return;
			}
			if(table.Rows[0][0].ToString()!="0"){//if claimpayment is already attached to a deposit
				#if !DEBUG
				throw new ApplicationException(Lans.g("ClaimPayments","Not allowed to delete a payment attached to a deposit."));
				#endif
			}
			//validate eobs
			command="SELECT COUNT(*) FROM eobattach WHERE ClaimPaymentNum="+POut.Long(cp.ClaimPaymentNum);
			if(Db.GetScalar(command)!="0") {
				throw new ApplicationException(Lans.g("ClaimPayments","Not allowed to delete this payment because EOBs are attached."));
			}
			command= "UPDATE claimproc SET "
				+"ClaimPaymentNum=0 "
				+"WHERE claimpaymentNum="+POut.Long(cp.ClaimPaymentNum);
			//MessageBox.Show(string command);
			Db.NonQ(command);
			command= "DELETE FROM claimpayment "
				+"WHERE ClaimPaymentnum ="+POut.Long(cp.ClaimPaymentNum);
			//MessageBox.Show(string command);
 			Db.NonQ(command);
		}


	
		
	}

	

	


}









