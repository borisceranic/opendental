using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary>Handles database commands related to the adjustment table in the db.</summary>
	public class Adjustments {
		///<summary>Gets all adjustments for a single patient.</summary>
		public static Adjustment[] Refresh(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Adjustment[]>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command=
				"SELECT * FROM adjustment"
				+" WHERE PatNum = "+POut.Long(patNum)+" ORDER BY AdjDate";
			return Crud.AdjustmentCrud.SelectMany(command).ToArray();
		}

		///<summary>Gets one adjustment from the db.</summary>
		public static Adjustment GetOne(long adjNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Adjustment>(MethodBase.GetCurrentMethod(),adjNum);
			}
			string command=
				"SELECT * FROM adjustment"
				+" WHERE AdjNum = "+POut.Long(adjNum);
			return Crud.AdjustmentCrud.SelectOne(adjNum);
		}

		public static void DetachFromInvoice(long statementNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),statementNum);
				return;
			}
			string command="UPDATE adjustment SET StatementNum=0 WHERE StatementNum='"+POut.Long(statementNum)+"'";
			Db.NonQ(command);
		}

		///<summary>Gets all negative or positive adjustments for a patient depending on how isPositive is set.</summary>
		public static List<Adjustment> GetAdjustForPats(List<long> listPatNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Adjustment>>(MethodBase.GetCurrentMethod(),listPatNums);
			}
			string command="SELECT * FROM adjustment "
				+"WHERE PatNum IN("+String.Join(", ",listPatNums)+") ";
			return Crud.AdjustmentCrud.SelectMany(command);
		}

		///<summary></summary>
		public static void Update(Adjustment adj){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),adj);
				return;
			}
			Crud.AdjustmentCrud.Update(adj);
		}

		///<summary></summary>
		public static long Insert(Adjustment adj) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				adj.AdjNum=Meth.GetLong(MethodBase.GetCurrentMethod(),adj);
				return adj.AdjNum;
			}
			return Crud.AdjustmentCrud.Insert(adj);
		}

		///<summary>This will soon be eliminated or changed to only allow deleting on same day as EntryDate.</summary>
		public static void Delete(Adjustment adj){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),adj);
				return;
			}
			Crud.AdjustmentCrud.Delete(adj.AdjNum);
		}

		///<summary>Loops through the supplied list of adjustments and returns an ArrayList of adjustments for the given proc.</summary>
		public static ArrayList GetForProc(long procNum,Adjustment[] List) {
			//No need to check RemotingRole; no call to db.
			ArrayList retVal=new ArrayList();
			for(int i=0;i<List.Length;i++){
				if(List[i].ProcNum==procNum){
					retVal.Add(List[i]);
				}
			}
			return retVal;
		}

		///<summary>Sums all adjustments for a proc then returns that sum.</summary>
		public static double GetTotForProc(long procNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetDouble(MethodBase.GetCurrentMethod(),procNum);
			}
			string command="SELECT SUM(AdjAmt) FROM adjustment"
				+" WHERE ProcNum="+POut.Long(procNum);
			return PIn.Double(Db.GetScalar(command));
		}

		///<summary>Creates a new discount adjustment for the given procedure.</summary>
		public static void CreateAdjustmentForDiscount(Procedure procedure) {
			//No need to check RemotingRole; no call to db.
			Adjustment AdjustmentCur=new Adjustment();
			AdjustmentCur.DateEntry=DateTime.Today;
			AdjustmentCur.AdjDate=DateTime.Today;
			AdjustmentCur.ProcDate=procedure.ProcDate;
			AdjustmentCur.ProvNum=procedure.ProvNum;
			AdjustmentCur.PatNum=procedure.PatNum;
			AdjustmentCur.AdjType=PrefC.GetLong(PrefName.TreatPlanDiscountAdjustmentType);
			AdjustmentCur.ClinicNum=procedure.ClinicNum;
			AdjustmentCur.AdjAmt=-procedure.Discount;//Discount must be negative here.
			AdjustmentCur.ProcNum=procedure.ProcNum;
			Adjustments.Insert(AdjustmentCur);
		}

		///<summary>Deletes all adjustments for a procedure</summary>
		public static void DeleteForProcedure(long procNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),procNum);
				return;
			}
			//Create log for each adjustment that is going to be deleted.
			string command="SELECT * FROM adjustment WHERE ProcNum = "+POut.Long(procNum); //query for all adjustments of a procedure 
			List<Adjustment> listAdjustments=Crud.AdjustmentCrud.SelectMany(command);
			for(int i=0;i<listAdjustments.Count;i++) { //loops through the rows
				SecurityLogs.MakeLogEntry(Permissions.AdjustmentEdit,listAdjustments[i].PatNum, //and creates audit trail entry for every row to be deleted
				"Delete adjustment for patient: "
				+Patients.GetLim(listAdjustments[i].PatNum).GetNameLF()+", "
				+(listAdjustments[i].AdjAmt).ToString("c"));
			}
			//Delete each adjustment for the procedure.
			command="DELETE FROM adjustment WHERE ProcNum = "+POut.Long(procNum);
			Db.NonQ(command);
		}

		///<summary>Used from ContrAccount and ProcEdit to display and calculate adjustments attached to procs.</summary>
		public static double GetTotForProc(long procNum,Adjustment[] List) {
			//No need to check RemotingRole; no call to db.
			double retVal=0;
			for(int i=0;i<List.Length;i++){
				if(List[i].ProcNum==procNum){
					retVal+=List[i].AdjAmt;
				}
			}
			return retVal;
		}

		/*
		///<summary>Must make sure Refresh is done first.  Returns the sum of all adjustments for this patient.  Amount might be pos or neg.</summary>
		public static double ComputeBal(Adjustment[] List){
			double retVal=0;
			for(int i=0;i<List.Length;i++){
				retVal+=List[i].AdjAmt;
			}
			return retVal;
		}*/

		///<summary>Returns the number of finance charges deleted.</summary>
		public static long UndoFinanceCharges(DateTime dateUndo) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),dateUndo);
			}
			string command;
			long numAdj;
			DataTable table;
			command="SELECT ValueString FROM preference WHERE PrefName = 'FinanceChargeAdjustmentType'";
			table=Db.GetTable(command);
			numAdj=PIn.Long(table.Rows[0][0].ToString());
			command="SELECT * FROM adjustment WHERE AdjDate="+POut.Date(dateUndo)
				+" AND AdjType="+POut.Long(numAdj);
			//Similar to code in DeleteForProcedure, but uses a AdjDate instead of ProcNum
			List<Adjustment> listAdjustments=Crud.AdjustmentCrud.SelectMany(command);
			for(int i=0;i<listAdjustments.Count;i++) { //loops through the rows
				SecurityLogs.MakeLogEntry(Permissions.AdjustmentEdit,listAdjustments[i].PatNum, //and creates audit trail entry for every row to be deleted
				"Delete adjustment for patient, undo finance charges: "
				+Patients.GetLim(listAdjustments[i].PatNum).GetNameLF()+", "
				+(listAdjustments[i].AdjAmt).ToString("c"));
			}
			command="DELETE FROM adjustment WHERE AdjDate="+POut.Date(dateUndo) 
				+" AND AdjType="+POut.Long(numAdj);
			return Db.NonQ(command);
		}

		///<summary>Returns the number of billing charges deleted.</summary>
		public static long UndoBillingCharges(DateTime dateUndo) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),dateUndo);
			}
			string command;
			long numAdj;
			DataTable table;
			//Similar to code in DeleteForProcedure, but uses a AdjDate instead of ProcNum
			command="SELECT ValueString FROM preference WHERE PrefName = 'BillingChargeAdjustmentType'";
			table=Db.GetTable(command);
			numAdj=PIn.Long(table.Rows[0][0].ToString());
			command="SELECT * FROM adjustment WHERE AdjDate="+POut.Date(dateUndo)
				+" AND AdjType="+POut.Long(numAdj);
			//Similar to code in DeleteForProcedure, but uses a AdjDate instead of ProcNum
			List<Adjustment> listAdjustments=Crud.AdjustmentCrud.SelectMany(command);
			for(int i=0;i<listAdjustments.Count;i++) { //loops through the rows
				SecurityLogs.MakeLogEntry(Permissions.AdjustmentEdit,listAdjustments[i].PatNum, //and creates audit trail entry for every row to be deleted
				"Delete adjustment for patient, undo billing charges: "
				+Patients.GetLim(listAdjustments[i].PatNum).GetNameLF()+", "
				+(listAdjustments[i].AdjAmt).ToString("c"));
			}
			command="DELETE FROM adjustment WHERE AdjDate="+POut.Date(dateUndo)
				+" AND AdjType="+POut.Long(numAdj);
			return Db.NonQ(command);
		}

	}

	


	


}










