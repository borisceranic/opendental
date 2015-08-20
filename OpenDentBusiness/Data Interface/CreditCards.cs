using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class CreditCards{

		///<summary></summary>
		public static List<CreditCard> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<CreditCard>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM creditcard WHERE PatNum = "+POut.Long(patNum)+" ORDER BY ItemOrder DESC";
			return Crud.CreditCardCrud.SelectMany(command);
		}

		///<summary>Gets one CreditCard from the db.</summary>
		public static CreditCard GetOne(long creditCardNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<CreditCard>(MethodBase.GetCurrentMethod(),creditCardNum);
			}
			return Crud.CreditCardCrud.SelectOne(creditCardNum);
		}

		///<summary></summary>
		public static long Insert(CreditCard creditCard){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				creditCard.CreditCardNum=Meth.GetLong(MethodBase.GetCurrentMethod(),creditCard);
				return creditCard.CreditCardNum;
			}
			return Crud.CreditCardCrud.Insert(creditCard);
		}

		///<summary></summary>
		public static void Update(CreditCard creditCard){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),creditCard);
				return;
			}
			Crud.CreditCardCrud.Update(creditCard);
		}

		///<summary></summary>
		public static void Delete(long creditCardNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),creditCardNum);
				return;
			}
			string command= "DELETE FROM creditcard WHERE CreditCardNum = "+POut.Long(creditCardNum);
			Db.NonQ(command);
		}

		///<summary>Gets the masked CC# and exp date for all cards setup for monthly charges for the specified patient.  Only used for filling [CreditCardsOnFile] variable when emailing statements.</summary>
		public static string GetMonthlyCardsOnFile(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetString(MethodBase.GetCurrentMethod(),patNum);
			}
			string result="";
			string command="SELECT * FROM creditcard WHERE PatNum="+POut.Long(patNum)
				+" AND ("+DbHelper.Year("DateStop")+"<1880 OR DateStop>"+DbHelper.Now()+") "//Recurring card is active.
				+" AND ChargeAmt>0";
			List<CreditCard> monthlyCards=Crud.CreditCardCrud.SelectMany(command);
			for(int i=0;i<monthlyCards.Count;i++) {
				if(i>0) {
					result+=", ";
				}
				result+=monthlyCards[i].CCNumberMasked+" exp:"+monthlyCards[i].CCExpiration.ToString("MM/yy");
			}
			return result;
		}

		///<summary>Returns list of active credit cards.</summary>
		public static List<CreditCard> GetActiveCards(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<CreditCard>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM creditcard WHERE PatNum="+POut.Long(patNum)
				+" AND ("+DbHelper.Year("DateStop")+"<1880 OR DateStop>="+DbHelper.Curdate()+") "
				+" AND ("+DbHelper.Year("DateStart")+">1880 AND DateStart<="+DbHelper.Curdate()+") ";//Recurring card is active.
			return Crud.CreditCardCrud.SelectMany(command);
		}

		///<summary>Returns list of credit cards that are ready for a recurring charge.</summary>
		public static DataTable GetRecurringChargeList() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod());
			}
			DataTable table=new DataTable();
			//This query will return patient information and the latest recurring payment whom:
			//	-have recurring charges setup and today's date falls within the start and stop range.
			//NOTE: Query will return patients with or without payments regardless of when that payment occurred, filtering is done below.
			string command="SELECT PayOrder,CreditCardNum,PatNum,PatName,FamBalTotal,LatestPayment,DateStart,Address,AddressPat,Zip,ZipPat,XChargeToken,"
				+"CCNumberMasked,CCExpiration,ChargeAmt,PayPlanNum,ProvNum,ClinicNum,Procedures,PayConnectToken,PayConnectTokenExp "
				+"FROM (";
			#region Payments
			//The PayOrder is used to differentiate rows attached to payment plans
			command+="(SELECT 1 AS PayOrder,cc.CreditCardNum,cc.PatNum,"+DbHelper.Concat("pat.LName","', '","pat.FName")+" PatName,"
				+"guar.BalTotal-guar.InsEst FamBalTotal,"
				+"COALESCE(MAX(pay.PayDate),"+POut.Date(DateTime.MinValue)+") LatestPayment,"
				+"cc.DateStart,cc.Address,pat.Address AddressPat,cc.Zip,pat.Zip ZipPat,cc.XChargeToken,cc.CCNumberMasked,cc.CCExpiration,cc.ChargeAmt,"
				+"cc.PayPlanNum,cc.DateStop,0 ProvNum,pat.ClinicNum,cc.Procedures,cc.PayConnectToken,cc.PayConnectTokenExp "
				+"FROM creditcard cc "
				+"INNER JOIN patient pat ON pat.PatNum=cc.PatNum "
				+"INNER JOIN patient guar ON guar.PatNum=pat.Guarantor "
				+"LEFT JOIN payment pay ON cc.PatNum=pay.PatNum AND pay.IsRecurringCC=1 "
				+"WHERE cc.PayPlanNum=0 ";//Keeps card from showing up in case they have a balance AND is setup for payment plan. 
			if(DataConnection.DBtype==DatabaseType.MySql) {
				command+="GROUP BY cc.CreditCardNum) ";
			}
			else {//Oracle
				command+="GROUP BY cc.CreditCardNum,cc.PatNum,"+DbHelper.Concat("pat.LName","', '","pat.FName")+",PatName,guar.BalTotal-guar.InsEst,"
					+"cc.Address,cc.Zip,cc.XChargeToken,cc.CCNumberMasked,cc.CCExpiration,cc.ChargeAmt,cc.PayPlanNum,cc.DateStop,pat.ClinicNum,cc.Procedures,"
					+"cc.PayConnectToken,cc.PayConnectTokenExp) ";
			}
			#endregion
			command+="UNION ALL";
			#region Payment Plans
			command+="(SELECT 2 AS PayOrder,cc.CreditCardNum,cc.PatNum,"+DbHelper.Concat("pat.LName","', '","pat.FName")+" PatName,"
				//Special select statement to figure out how much is owed on a particular payment plan.  Total amount will be labeled FamBalTotal for UNION.
				+"ROUND((SELECT COALESCE(SUM(ppc.Principal+ppc.Interest),0) FROM PayPlanCharge ppc WHERE ppc.PayPlanNum=cc.PayPlanNum "
				+"AND ppc.ChargeDate<="+DbHelper.Curdate()+")-COALESCE(SUM(ps.SplitAmt),0),2) FamBalTotal,"
				+"COALESCE(MAX(pay.PayDate),"+POut.Date(DateTime.MinValue)+") LatestPayment,"
				+"cc.DateStart,cc.Address,pat.Address AddressPat,cc.Zip,pat.Zip ZipPat,cc.XChargeToken,cc.CCNumberMasked,cc.CCExpiration,cc.ChargeAmt,"
				+"cc.PayPlanNum,cc.DateStop,(SELECT ppc1.ProvNum FROM payplancharge ppc1 WHERE ppc1.PayPlanNum=cc.PayPlanNum "+DbHelper.LimitAnd(1)+") ProvNum,"
				+"(SELECT ppc2.ClinicNum FROM payplancharge ppc2 WHERE ppc2.PayPlanNum=cc.PayPlanNum "+DbHelper.LimitAnd(1)+") ClinicNum,cc.Procedures,"
				+"cc.PayConnectToken,cc.PayConnectTokenExp "
				+"FROM creditcard cc "
				+"INNER JOIN patient pat ON pat.PatNum=cc.PatNum "
				+"LEFT JOIN paysplit ps ON ps.PayPlanNum=cc.PayPlanNum AND ps.PayPlanNum<>0 "
				+"LEFT JOIN payment pay ON pay.PayNum=ps.PayNum AND pay.IsRecurringCC=1 "
				+"WHERE cc.PayPlanNum<>0 ";
			if(DataConnection.DBtype==DatabaseType.MySql) {
				command+="GROUP BY cc.CreditCardNum)";
			}
			else {//Oracle
				command+="GROUP BY cc.CreditCardNum,cc.PatNum,"+DbHelper.Concat("pat.LName","', '","pat.FName")+",PatName,guar.BalTotal-guar.InsEst,"
					+"cc.Address,pat.Address,cc.Zip,pat.Zip,cc.XChargeToken,cc.CCNumberMasked,cc.CCExpiration,cc.ChargeAmt,cc.PayPlanNum,cc.DateStop,"
					+"cc.Procedues,cc.PayConnectToken,cc.PayConnectTokenExp)";
			}
			#endregion
			//Now we have all the results for payments and payment plans, so do an obvious filter. A more thorough filter happens later.
			command+=") due "
				+"WHERE DateStart<="+DbHelper.Curdate()+" AND "+DbHelper.Year("DateStart")+">1880 "
				+"AND (DateStop>="+DbHelper.Curdate()+" OR "+DbHelper.Year("DateStop")+"<1880) "
				+"ORDER BY PatName";
			table=Db.GetTable(command);
			FilterRecurringChargeList(table);
			return table;
		}

		/// <summary>Adds up the total fees for the procedures passed in that have been completed since the last billing day.</summary>
		public static double TotalRecurringCharges(long patNum,string procedures,int billingDay) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetDouble(MethodBase.GetCurrentMethod(),patNum,procedures,billingDay);
			}
			DateTime startBillingCycle;
			if(DateTime.Today.Day>billingDay) {//if today is 7/13/2015 and billingDay is 26, startBillingCycle will be 6/26/2015
				startBillingCycle=new DateTime(DateTime.Today.Year,DateTime.Today.Month,billingDay);
			}
			else {
				DateTime lastMonth=DateTime.Today.AddMonths(-1);
				//Make sure the billing day is not Febuary 30th or November 31st
				billingDay=Math.Min(DateTime.DaysInMonth(lastMonth.Year,lastMonth.Month),billingDay);
				startBillingCycle=new DateTime(lastMonth.Year,lastMonth.Month,billingDay);
			}
			string procStr="'"+POut.String(procedures).Replace(",","','")+"'";
			string command="SELECT SUM(pl.ProcFee) "
				+"FROM procedurelog pl "
				+"INNER JOIN procedurecode pc ON pl.CodeNum=pc.CodeNum "
				+"WHERE pl.ProcStatus=2 "
				+"AND pc.ProcCode IN ("+procStr+") "
				+"AND pl.PatNum="+POut.Long(patNum)+" "
				+"AND pl.ProcDate<="+DbHelper.Curdate()+" ";
			if(billingDay==DateTime.Today.Day) {//So that the card is not charged for today's and last month's repeat charge
				command+="AND pl.ProcDate>"+POut.Date(startBillingCycle);
			}
			else {
				command+="AND pl.ProcDate>="+POut.Date(startBillingCycle);
			}
			return PIn.Double(Db.GetScalar(command));
		}

		/// <summary>Returns true if the procedure passed in is linked to any other active card on the patient's account.</summary>
		public static bool ProcLinkedToCard(long patNum,string procCode,long cardNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),patNum,procCode,cardNum);
			}
			string command="SELECT CreditCardNum,Procedures "
				+"FROM creditcard "
				+"WHERE PatNum="+POut.Long(patNum)+" "
				+"AND DateStart<="+DbHelper.Curdate()+" AND "+DbHelper.Year("DateStart")+">1880 "
				+"AND (DateStop>="+DbHelper.Curdate()+" OR "+DbHelper.Year("DateStop")+"<1880) "
				+"AND CreditCardNum!="+POut.Long(cardNum);
			DataTable table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				string[] arrayProcs=table.Rows[i]["Procedures"].ToString().Split(',');
				for(int j=0;j<arrayProcs.Length;j++) {
					if(arrayProcs[j]==procCode) {
						return true;
					}
				}
			}
			return false;
		}

		///<summary>Table must include columns labeled LatestPayment and DateStart.</summary>
		public static void FilterRecurringChargeList(DataTable table) {
			DateTime curDate=MiscData.GetNowDateTime();
			//Loop through table and remove patients that do not need to be charged yet.
			for(int i=0;i<table.Rows.Count;i++) {
				DateTime latestPayment=PIn.Date(table.Rows[i]["LatestPayment"].ToString());
				DateTime dateStart=PIn.Date(table.Rows[i]["DateStart"].ToString());
				if(curDate>latestPayment.AddDays(31)) {//if it's been more than a month since they made any sort of payment
					//if we reduce the days below 31, then slighly more people will be charged, especially from Feb to March.  31 eliminates those false positives.
					continue;//charge them
				}
				//Not enough days in the current month so show on the last day of the month
				//Example: DateStart=8/31/2010 and the current month is February 2011 which does not have 31 days.
				//So the patient needs to show in list if current day is the 28th (or last day of the month).
				int daysInMonth=DateTime.DaysInMonth(curDate.Year,curDate.Month);
				if(daysInMonth<=dateStart.Day && daysInMonth==curDate.Day && curDate.Date!=latestPayment.Date) {//if their recurring charge would fall on an invalid day of the month, and this is that last day of the month
					continue;//we want them to show because the charge should go in on this date.
				}
				if(curDate.Day>=dateStart.Day) {//If the recurring charge date was earlier in this month, then the recurring charge will go in for this month.
					if(curDate.Month>latestPayment.Month || curDate.Year>latestPayment.Year) {//if the latest payment was last month (or earlier).  The year check catches December
						continue;//No payments were made this month, so charge.
					}
				}
				else {//Else, current date is before the recurring date in the current month, so the recurring charge will be going in for last month
					//Check if payment didn't happen last month.
					if(curDate.AddMonths(-1).Month!=latestPayment.Month && curDate.Date!=latestPayment.Date) {
						//Charge did not happen last month so the patient needs to show up in list.
						//Example: Last month had a recurring charge set at the end of the month that fell on a weekend.
						//Today is the next month and still before the recurring charge date. 
						//This will allow the charge for the previous month to happen if the 30 day check didn't catch it above.
						continue;
					}
				}
				//Patient doesn't need to be charged yet so remove from the table.
				table.Rows.RemoveAt(i);
				i--;
			}
		}

		///<summary>Checks if token is in use.  This happened once and can cause the wrong card to be charged.</summary>
		public static bool IsDuplicateXChargeToken(string token) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetBool(MethodBase.GetCurrentMethod(),token);
			}
			string command="SELECT COUNT(*) FROM creditcard WHERE XChargeToken='"+POut.String(token)+"'";
			if(Db.GetCount(command)=="1") {
				return false;
			}
			return true;
		}

		///<summary>Checks if token is in use.  This happened once and can cause the wrong card to be charged.</summary>
		public static bool IsDuplicatePayConnectToken(string token) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),token);
			}
			string command="SELECT COUNT(*) FROM creditcard WHERE PayConnectToken='"+POut.String(token)+"'";
			if(Db.GetCount(command)=="1") {
				return false;
			}
			return true;
		}

		///<summary>Gets every credit card in the db with an X-Charge token.</summary>
		public static List<CreditCard> GetCardsWithXChargeTokens() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<CreditCard>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM creditcard WHERE XChargeToken!=\"\"";
			return Crud.CreditCardCrud.SelectMany(command);
		}

	}
}