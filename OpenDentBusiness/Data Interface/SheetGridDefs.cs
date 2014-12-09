using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness {
	///<summary></summary>
	public class SheetGridDefs {
		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<SheetGridDef> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SheetGridDef>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM sheetgriddef WHERE PatNum = "+POut.Long(patNum);
			return Crud.SheetGridDefCrud.SelectMany(command);
		}
		*/

		///<summary></summary>
		public static long Insert(SheetGridDef sheetGridDef) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				sheetGridDef.SheetGridDefNum=Meth.GetLong(MethodBase.GetCurrentMethod(),sheetGridDef);
				return sheetGridDef.SheetGridDefNum;
			}
			long retVal=Crud.SheetGridDefCrud.Insert(sheetGridDef);
			foreach(SheetGridColDef colDef in sheetGridDef.Columns) {
				colDef.SheetGridDefNum=retVal;
				Crud.SheetGridColDefCrud.Insert(colDef);
			}
			return retVal;
		}

		///<summary></summary>
		public static void Update(SheetGridDef sheetGridDef) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),sheetGridDef);
				return;
			}
			Crud.SheetGridDefCrud.Update(sheetGridDef);
			foreach(SheetGridColDef colDef in sheetGridDef.Columns) {
				SheetGridColDefs.Update(colDef);
			}
		}

		///<summary></summary>
		public static void Delete(long sheetGridDefNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),sheetGridDefNum);
				return;
			}
			Crud.SheetGridDefCrud.Delete(sheetGridDefNum);
			SheetGridColDefs.DeleteForGridDef(sheetGridDefNum);
		}

		///<summary>Gets one SheetGridDef from the db.  Can return null.</summary>
		public static SheetGridDef GetOne(long sheetGridDefNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<SheetGridDef>(MethodBase.GetCurrentMethod(),sheetGridDefNum);
			}
			SheetGridDef retVal=Crud.SheetGridDefCrud.SelectOne(sheetGridDefNum);
			if(retVal==null) {
				return null;
			}
			retVal.Columns=SheetGridColDefs.GetForGridDef(retVal.SheetGridDefNum);
			return retVal;
		}

		public static DataTable GetDataTableForGridType(SheetGridType type,SheetArgs ga) {
			DataTable retVal=new DataTable();
			switch(type) {
				case SheetGridType.StatementMain:
					retVal=getTable_StatementMain(ga);
					break;
				case SheetGridType.StatementAgeing:
					retVal=getTable_StatementAging(ga);
					break;
				case SheetGridType.StatementPayPlan:
					retVal=getTable_StatementPayPlan(ga);
					break;
				case SheetGridType.StatementEnclosed:
					retVal=getTable_StatementEnclosed(ga);
					break;
				default:
					break;
			}
			return retVal;
		}

		///<summary>Gets account tables by calling AccountModules.GetAccount and then appends dataRows together into a single table. </summary>
		private static DataTable getTable_StatementMain(SheetArgs a) {
			DataTable retVal=null;
			DataSet ds=AccountModules.GetAccount(a.PatNum,a.FromDate,a.ToDate,a.Intermingled,a.SinglePatient,a.StatementNum,a.ShowProcBreakdown,a.ShowPayNotes,a.IsInvoice,a.ShowAdjNotes,true);
			foreach(DataTable t in ds.Tables) {
				if(!t.TableName.StartsWith("account")) {
					continue;
				}
				if(retVal==null) {//first pass
					retVal=t.Clone();
				}
				foreach(DataRow r in t.Rows) {
					retVal.ImportRow(r);
				}
				if(t.Rows.Count==0) {
					Patient p=Patients.GetPat(PIn.Long(t.TableName.Replace("account","")));
					if(p==null) {
						p=Patients.GetPat(a.PatNum);
					}
					retVal.Rows.Add(
						"",//"AdjNum"          
						"",//"balance"         
						0,//"balanceDouble"   
						"",//"charges"         
						0,//"chargesDouble"   
						"",//"ClaimNum"        
						"",//"ClaimPaymentNum" 
						"",//"clinic"          
						"",//"colorText"       
						"",//"credits"         
						0,//"creditsDouble"   
						DateTime.Today.ToShortDateString(),//"date"            
						DateTime.Today,//"DateTime"        
						Lans.g("Statements","No Account Activity"),//"description"     
						p.FName,//"patient"         
						p.PatNum,//"PatNum"          
						0,//"PayNum"          
						0,//"PayPlanNum"      
						0,//"PayPlanChargeNum"
						"",//"ProcCode"        
						0,//"ProcNum"         
						0,//"ProcNumLab"      
						0,//"procsOnObj"      
						0,//"prov"            
						0,//"StatementNum"    
						"",//"ToothNum"        
						"",//"ToothRange"      
						""//"tth"       
						);
				}
			}
			return retVal;
		}

		private static DataTable getTable_StatementAging(SheetArgs a) {
			DataTable retVal=new DataTable();
			retVal.Columns.Add(new DataColumn("Age00to30"));
			retVal.Columns.Add(new DataColumn("Age31to60"));
			retVal.Columns.Add(new DataColumn("Age61to90"));
			retVal.Columns.Add(new DataColumn("Age90plus"));
			Patient guar=Patients.GetPat(Patients.GetPat(a.PatNum).Guarantor);
			DataRow row=retVal.NewRow();
			row[0]=guar.Bal_0_30.ToString("F");
			row[1]=guar.Bal_31_60.ToString("F");
			row[2]=guar.Bal_61_90.ToString("F");
			row[3]=guar.BalOver90.ToString("F");
			retVal.Rows.Add(row);
			return retVal;
		}

		private static DataTable getTable_StatementPayPlan(SheetArgs a) {
			DataTable retVal=new DataTable();
			DataSet ds=AccountModules.GetAccount(a.PatNum,a.FromDate,a.ToDate,a.Intermingled,a.SinglePatient,a.StatementNum,a.ShowProcBreakdown,a.ShowPayNotes,a.IsInvoice,a.ShowAdjNotes,true);
			foreach(DataTable t in ds.Tables) {
				if(!t.TableName.StartsWith("payplan")) {
					continue;
				}
				retVal=t.Clone();
				foreach(DataRow r in t.Rows) {
					retVal.ImportRow(r);
				}
			}
			return retVal;
		}

		private static DataTable getTable_StatementEnclosed(SheetArgs a) {
			DataSet dataSet=AccountModules.GetStatementDataSet(Statements.CreateObject(a.StatementNum));
			DataTable tableMisc=dataSet.Tables["misc"];
			string text="";
			DataTable table=new DataTable();
			table.Columns.Add(new DataColumn("AmountDue"));
			table.Columns.Add(new DataColumn("DateDue"));
			table.Columns.Add(new DataColumn("AmountEnclosed"));
			DataRow row=table.NewRow();
			Patient patGuar=Patients.GetPat(Patients.GetPat(a.PatNum).Guarantor);
			double balTotal=patGuar.BalTotal;
			if(!PrefC.GetBool(PrefName.BalancesDontSubtractIns)) {//this is typical
				balTotal-=patGuar.InsEst;
			}
			for(int m=0;m<tableMisc.Rows.Count;m++){
				if(tableMisc.Rows[m]["descript"].ToString()=="payPlanDue"){
					balTotal+=PIn.Double(tableMisc.Rows[m]["value"].ToString());
					//payPlanDue;//PatGuar.PayPlanDue;
				}
			}
			InstallmentPlan installPlan=InstallmentPlans.GetOneForFam(patGuar.PatNum);
			if(installPlan!=null){
				//show lesser of normal total balance or the monthly payment amount.
				if(installPlan.MonthlyPayment < balTotal) {
					text=installPlan.MonthlyPayment.ToString("F");
				}
				else {
					text=balTotal.ToString("F");
				}
			}
			else {//no installmentplan
				text=balTotal.ToString("F");
			}
			row[0]=text;
			if(PrefC.GetLong(PrefName.StatementsCalcDueDate)==-1) {
				text=Lans.g("Statements","Upon Receipt");
			}
			else {
				text=DateTime.Today.AddDays(PrefC.GetLong(PrefName.StatementsCalcDueDate)).ToShortDateString();
			}
			row[1]=text;
			row[2]="";
			table.Rows.Add(row);
			return table;
		}

		///<summary>Returns the current list of all columns available for the grid in the data table.</summary>
		public static List<SheetGridColDef> GetColumnsAvailable(SheetGridType type) {
			int i=0;
			List<SheetGridColDef> retVal=new List<SheetGridColDef>();
			switch(type) {
				case SheetGridType.StatementMain:
					#region all columns
					//Columns below are all of the columns available in the data table, many of them are not intended for display, but can be by uncommenting below.
					//	retVal.Add(new SheetGridColDef { ColName="AdjNum"          ,DisplayName="AdjNum"          ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="balance"         ,DisplayName="balance"         ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="balanceDouble"   ,DisplayName="balanceDouble"   ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="charges"         ,DisplayName="charges"         ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="chargesDouble"   ,DisplayName="chargesDouble"   ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="ClaimNum"        ,DisplayName="ClaimNum"        ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="ClaimPaymentNum" ,DisplayName="ClaimPaymentNum" ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="clinic"          ,DisplayName="clinic"          ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="colorText"       ,DisplayName="colorText"       ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="credits"         ,DisplayName="credits"         ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="creditsDouble"   ,DisplayName="creditsDouble"   ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="date"            ,DisplayName="date"            ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="DateTime"        ,DisplayName="DateTime"        ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="description"     ,DisplayName="description"     ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="patient"         ,DisplayName="patient"         ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="PatNum"          ,DisplayName="PatNum"          ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="PayNum"          ,DisplayName="PayNum"          ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="PayPlanNum"      ,DisplayName="PayPlanNum"      ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="PayPlanChargeNum",DisplayName="PayPlanChargeNum",Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="ProcCode"        ,DisplayName="ProcCode"        ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="ProcNum"         ,DisplayName="ProcNum"         ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="ProcNumLab"      ,DisplayName="ProcNumLab"      ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="procsOnObj"      ,DisplayName="procsOnObj"      ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="prov"            ,DisplayName="prov"            ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="StatementNum"    ,DisplayName="StatementNum"    ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="ToothNum"        ,DisplayName="ToothNum"        ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="ToothRange"      ,DisplayName="ToothRange"      ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="tth"             ,DisplayName="tth"             ,Width=80,ItemOrder=++i });
					#endregion
					retVal.Add(new SheetGridColDef { ColName="date"             ,DisplayName="Date"            ,Width=75 ,TextAlign=StringAlignment.Near,ItemOrder=++i });
					retVal.Add(new SheetGridColDef { ColName="patient"          ,DisplayName="Patient"         ,Width=100,TextAlign=StringAlignment.Near,ItemOrder=++i });
					retVal.Add(new SheetGridColDef { ColName="ProcCode"         ,DisplayName="Code"            ,Width=45 ,TextAlign=StringAlignment.Near,ItemOrder=++i });
					retVal.Add(new SheetGridColDef { ColName="tth"              ,DisplayName="Tooth"           ,Width=45 ,TextAlign=StringAlignment.Near,ItemOrder=++i });
					retVal.Add(new SheetGridColDef { ColName="description"      ,DisplayName="Description"     ,Width=275,TextAlign=StringAlignment.Near,ItemOrder=++i });
					retVal.Add(new SheetGridColDef { ColName="charges"          ,DisplayName="Charges"         ,Width=60 ,TextAlign=StringAlignment.Far,ItemOrder=++i });
					retVal.Add(new SheetGridColDef { ColName="credits"          ,DisplayName="Credits"         ,Width=60 ,TextAlign=StringAlignment.Far,ItemOrder=++i });
					retVal.Add(new SheetGridColDef { ColName="balance"          ,DisplayName="Balance"         ,Width=60 ,TextAlign=StringAlignment.Far,ItemOrder=++i });
					break;
				case SheetGridType.StatementEnclosed:
					retVal.Add(new SheetGridColDef { ColName="AmountDue"        ,DisplayName="Amount Due"      ,Width=107,TextAlign=StringAlignment.Center,ItemOrder=++i });
					retVal.Add(new SheetGridColDef { ColName="DateDue"          ,DisplayName="Date Due"        ,Width=107,TextAlign=StringAlignment.Center,ItemOrder=++i });
					retVal.Add(new SheetGridColDef { ColName="AmountEnclosed"   ,DisplayName="Amount Enclosed" ,Width=107,TextAlign=StringAlignment.Center,ItemOrder=++i });
					break;
				case SheetGridType.StatementAgeing:
					retVal.Add(new SheetGridColDef { ColName="Age00to30"        ,DisplayName="0-30"     ,Width=100,TextAlign=StringAlignment.Center,ItemOrder=++i });
					retVal.Add(new SheetGridColDef { ColName="Age31to60"        ,DisplayName="31-60"    ,Width=100,TextAlign=StringAlignment.Center,ItemOrder=++i });
					retVal.Add(new SheetGridColDef { ColName="Age61to90"        ,DisplayName="61-90"    ,Width=100,TextAlign=StringAlignment.Center,ItemOrder=++i });
					retVal.Add(new SheetGridColDef { ColName="Age90plus"        ,DisplayName="over 90"  ,Width=100,TextAlign=StringAlignment.Center,ItemOrder=++i });
					break;
				case SheetGridType.StatementPayPlan:
					#region all columns
					//	retVal.Add(new SheetGridColDef { ColName="AdjNum"          ,DisplayName="AdjNum"          ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="balance"         ,DisplayName="balance"         ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="balanceDouble"   ,DisplayName="balanceDouble"   ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="charges"         ,DisplayName="charges"         ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="chargesDouble"   ,DisplayName="chargesDouble"   ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="ClaimNum"        ,DisplayName="ClaimNum"        ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="ClaimPaymentNum" ,DisplayName="ClaimPaymentNum" ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="clinic"          ,DisplayName="clinic"          ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="colorText"       ,DisplayName="colorText"       ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="credits"         ,DisplayName="credits"         ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="creditsDouble"   ,DisplayName="creditsDouble"   ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="date"            ,DisplayName="date"            ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="DateTime"        ,DisplayName="DateTime"        ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="description"     ,DisplayName="description"     ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="patient"         ,DisplayName="patient"         ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="PatNum"          ,DisplayName="PatNum"          ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="PayNum"          ,DisplayName="PayNum"          ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="PayPlanNum"      ,DisplayName="PayPlanNum"      ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="PayPlanChargeNum",DisplayName="PayPlanChargeNum",Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="ProcCode"        ,DisplayName="ProcCode"        ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="ProcNum"         ,DisplayName="ProcNum"         ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="ProcNumLab"      ,DisplayName="ProcNumLab"      ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="procsOnObj"      ,DisplayName="procsOnObj"      ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="prov"            ,DisplayName="prov"            ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="StatementNum"    ,DisplayName="StatementNum"    ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="ToothNum"        ,DisplayName="ToothNum"        ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="ToothRange"      ,DisplayName="ToothRange"      ,Width=80,ItemOrder=++i });
					//	retVal.Add(new SheetGridColDef { ColName="tth"             ,DisplayName="tth"             ,Width=80,ItemOrder=++i });
					#endregion
					retVal.Add(new SheetGridColDef { ColName="date"            ,DisplayName="Date"            ,Width=80 ,TextAlign=StringAlignment.Near,ItemOrder=++i });
					retVal.Add(new SheetGridColDef { ColName="description"     ,DisplayName="Description"     ,Width=250,TextAlign=StringAlignment.Near,ItemOrder=++i });
					retVal.Add(new SheetGridColDef { ColName="charges"         ,DisplayName="Charges"         ,Width=60 ,TextAlign=StringAlignment.Far,ItemOrder=++i });
					retVal.Add(new SheetGridColDef { ColName="credits"         ,DisplayName="Credits"         ,Width=60 ,TextAlign=StringAlignment.Far,ItemOrder=++i });
					retVal.Add(new SheetGridColDef { ColName="balance"         ,DisplayName="Balance"         ,Width=60 ,TextAlign=StringAlignment.Far,ItemOrder=++i });
					break;
			}
			return retVal;
		}

		public static string GetName(SheetGridDef def) {
			//No call to db, no need for roles check
			return TypeToDisplay(def.GridType);
		}

		public static string TypeToDisplay(SheetGridType sheetGridType) {
			switch(sheetGridType) {
				case SheetGridType.StatementMain:
					return "Account";
				case SheetGridType.StatementEnclosed:
					return "Enclosed Amount";
				case SheetGridType.StatementAgeing:
					return "Ageing Grid";
				case SheetGridType.StatementPayPlan:
					return "Pay Plan Grid";
				default:
					#if DEBUG
						throw new ApplicationException("SheetGridDefs.typeToDisplay() needs a new value.");
					#endif
					return "Unknown Grid Type";//should never happen.
			}
		}

		public static bool gridHasDefaultTitle(SheetGridType sheetGridType) {
			switch(sheetGridType) {
				//case SheetGridType.StatementMain:
				//case SheetGridType.StatementEnclosed:
				//case SheetGridType.StatementAgeing:
				case SheetGridType.StatementPayPlan:
					return true;//should never happen.
			}
			return false;
		}


	}
}