using System;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{

	public class XChargeTransactions { 

		///<summary></summary>
		public static long Insert(XChargeTransaction xChargeTransaction) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				xChargeTransaction.XChargeTransactionNum=Meth.GetLong(MethodBase.GetCurrentMethod(),xChargeTransaction);
				return xChargeTransaction.XChargeTransactionNum;
			}
			return Crud.XChargeTransactionCrud.Insert(xChargeTransaction);
		}

		///<summary>Gets one XChargeTransaction from the db by batchNum and itemNum. For example: ("1515","0001").</summary>
		public static XChargeTransaction GetOneByBatchItem(string batchNum,string itemNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<XChargeTransaction>(MethodBase.GetCurrentMethod(),batchNum,itemNum);
			}
			string command="SELECT * FROM xchargetransaction WHERE BatchNum = '"+POut.String(batchNum)+"' AND ItemNum = '"+POut.String(itemNum)+"'";
			return Crud.XChargeTransactionCrud.SelectOne(command);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<XChargeTransaction> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<XChargeTransaction>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM xchargetransaction WHERE PatNum = "+POut.Long(patNum);
			return Crud.XChargeTransactionCrud.SelectMany(command);
		}

		///<summary></summary>
		public static void Update(XChargeTransaction xChargeTransaction){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),xChargeTransaction);
				return;
			}
			Crud.XChargeTransactionCrud.Update(xChargeTransaction);
		}
	*/
		///<summary></summary>
		public static void Delete(long xChargeTransactionNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),xChargeTransactionNum);
				return;
			}
			string command= "DELETE FROM xchargetransaction WHERE XChargeTransactionNum = "+POut.Long(xChargeTransactionNum);
			Db.NonQ(command);
		}

		public static DataTable GetMissingPaymentsTable(DateTime dateStart,DateTime dateEnd) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateStart,dateEnd);
			}
			string command="SELECT TransactionDateTime,TransType,ClerkID,ItemNum,xchargetransaction.PatNum,CreditCardNum,Expiration,Result,Amount "
				+"FROM xchargetransaction "
				+"LEFT JOIN ("
					+"SELECT PatNum,DateEntry,PayAmt "
					+"FROM payment "
					//only payments with the same PaymentType as the X-Charge PaymentType for the clinic
					+"INNER JOIN ("
						+"SELECT ClinicNum,PropertyValue PaymentType FROM programproperty "
						+"WHERE ProgramNum="+POut.Long(Programs.GetProgramNum(ProgramName.Xcharge))+" AND PropertyDesc='PaymentType'"
					+") paytypes ON paytypes.ClinicNum=payment.ClinicNum AND paytypes.PaymentType=payment.PayType "
					+"WHERE DateEntry BETWEEN "+POut.Date(dateStart)+" AND "+POut.Date(dateEnd)
				+") pay ON xchargetransaction.PatNum=pay.PatNum "
					+"AND "+DbHelper.DtimeToDate("TransactionDateTime")+"=pay.DateEntry "
					+"AND xchargetransaction.Amount=pay.PayAmt "
				+"WHERE "+DbHelper.DtimeToDate("TransactionDateTime")+" BETWEEN "+POut.Date(dateStart)+" AND "+POut.Date(dateEnd)+" "
				+"AND pay.PatNum IS NULL "
				+"AND xchargetransaction.ResultCode=0";//Valid entries to count have result code 0
			return Db.GetTable(command);
		}

		public static DataTable GetMissingXTransTable(DateTime dateStart,DateTime dateEnd) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateStart,dateEnd);
			}
			string command="SELECT payment.PatNum,LName,FName,payment.DateEntry,payment.PayDate,payment.PayNote,payment.PayAmt "
				+"FROM patient "
				+"INNER JOIN payment ON payment.PatNum=patient.PatNum "
				//only payments with the same PaymentType as the X-Charge PaymentType for the clinic
				+"INNER JOIN ("
					+"SELECT ClinicNum,PropertyValue AS PaymentType FROM programproperty "
					+"WHERE ProgramNum="+POut.Long(Programs.GetProgramNum(ProgramName.Xcharge))+" AND PropertyDesc='PaymentType'"
				+") paytypes ON paytypes.ClinicNum=payment.ClinicNum AND paytypes.PaymentType=payment.PayType "
				+"LEFT JOIN xchargetransaction ON xchargetransaction.PatNum=payment.PatNum "
					+"AND "+DbHelper.DtimeToDate("TransactionDateTime")+"=payment.DateEntry "
					+"AND xchargetransaction.Amount=payment.PayAmt "
					+"AND xchargetransaction.ResultCode IN(0,10) "
				+"WHERE payment.DateEntry BETWEEN "+POut.Date(dateStart)+" AND "+POut.Date(dateEnd)+" "
				+"AND TransactionDateTime IS NULL "
				+"ORDER BY payment.PayDate ASC,LName,FName";
			return Db.GetTable(command);
		}
	



	}
}