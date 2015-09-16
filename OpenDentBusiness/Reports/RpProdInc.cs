using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness {
	public class RpProdInc {
		///<summary>Used in RowComparer to limit using the cache in the CEMT.</summary>
		private static bool _hasClinics;

		//IMPORTANT NOTE FOR ANYBODY WHO CODES IN HERE:  This is used in the CEMT so everything MUST be coded in such a way that they don't use the 
		//cache to look up information.  The CEMT does NOT keep copies of the remote database caches when this is used so things such as 
		//PrefC.GetBool or Clinics.GetDesc will return incorrect results.

		#region Daily and Provider P&I Reports
		///<summary>If not using clinics then supply an empty list of clinicNums.  Also used for the CEMT Provider P and I report.</summary>
		public static DataSet GetDailyData(DateTime dateFrom,DateTime dateTo,List<Provider> listProvs,List<Clinic> listClinics,bool writeOffPay,bool hasAllProvs,bool hasAllClinics,bool hasBreakdown) {
			//No need to check RemotingRole; no call to db.
			if(listClinics.Count>0) {
				_hasClinics=true;
			}
			DataSet dataSet=GetDailyProdIncDataSet(dateFrom,dateTo,listProvs,listClinics,writeOffPay,hasAllProvs,hasAllClinics);
			DataTable tableProduction=dataSet.Tables["tableProduction"];
			DataTable tableAdj=dataSet.Tables["tableAdj"];
			DataTable tableInsWriteoff=dataSet.Tables["tableInsWriteoff"];
			DataTable tablePay=dataSet.Tables["tablePay"];
			DataTable tableIns=dataSet.Tables["tableIns"];
			DataTable tableDailyProd=new DataTable("DailyProd");
			tableDailyProd.Columns.Add(new DataColumn("Date"));
			tableDailyProd.Columns.Add(new DataColumn("Name"));
			tableDailyProd.Columns.Add(new DataColumn("Description"));
			tableDailyProd.Columns.Add(new DataColumn("Provider"));
			if(_hasClinics) {
				tableDailyProd.Columns.Add(new DataColumn("Clinic"));
			}
			tableDailyProd.Columns.Add(new DataColumn("Production"));
			tableDailyProd.Columns.Add(new DataColumn("Adjust"));
			tableDailyProd.Columns.Add(new DataColumn("Writeoff"));
			tableDailyProd.Columns.Add(new DataColumn("Pt Income"));
			tableDailyProd.Columns.Add(new DataColumn("Ins Income"));
			tableDailyProd.Columns.Add(new DataColumn("ClinicSplit"));
			for(int i=0;i<tableProduction.Rows.Count;i++) {
				if(_hasClinics && !listClinics.Exists(x => x.ClinicNum==PIn.Long(tableProduction.Rows[i]["Clinic"].ToString()))) {
					continue;//Using clinics and the current row is for a clinic that is NOT in the list of clinics we care about.
				}
				DataRow row=tableDailyProd.NewRow();
				row["Date"]=PIn.Date(tableProduction.Rows[i]["Date"].ToString()).ToShortDateString();
				row["Name"]=tableProduction.Rows[i]["namelf"].ToString();
				row["Description"]=tableProduction.Rows[i]["Description"].ToString();
				row["Provider"]=tableProduction.Rows[i]["Abbr"].ToString();
				if(_hasClinics) {
					row["Clinic"]=tableProduction.Rows[i]["Clinic"].ToString();
				}
				row["Production"]=tableProduction.Rows[i]["Production"].ToString();
				row["Adjust"]=0;
				row["Writeoff"]=0;
				row["Pt Income"]=0;
				row["Ins Income"]=0;
				row["ClinicSplit"]=hasBreakdown ? tableProduction.Rows[i]["Clinic"].ToString():"";
				tableDailyProd.Rows.Add(row);
			}
			for(int i=0;i<tableAdj.Rows.Count;i++) {
				if(_hasClinics && !listClinics.Exists(x => x.ClinicNum==PIn.Long(tableAdj.Rows[i]["Clinic"].ToString()))) {
					continue;//Using clinics and the current row is for a clinic that is NOT in the list of clinics we care about.
				}
				DataRow row=tableDailyProd.NewRow();
				row["Date"]=PIn.Date(tableAdj.Rows[i]["Date"].ToString()).ToShortDateString();
				row["Name"]=tableAdj.Rows[i]["namelf"].ToString();
				row["Description"]=tableAdj.Rows[i]["Description"].ToString();
				row["Provider"]=tableAdj.Rows[i]["Abbr"].ToString();
				if(_hasClinics) {
					row["Clinic"]=tableAdj.Rows[i]["Clinic"].ToString();
				}
				row["Production"]=0;
				row["Adjust"]=tableAdj.Rows[i]["AdjAmt"].ToString();
				row["Writeoff"]=0;
				row["Pt Income"]=0;
				row["Ins Income"]=0;
				row["ClinicSplit"]=hasBreakdown ? tableAdj.Rows[i]["Clinic"].ToString():"";
				tableDailyProd.Rows.Add(row);
			}
			for(int i=0;i<tableInsWriteoff.Rows.Count;i++) {
				if(_hasClinics && !listClinics.Exists(x => x.ClinicNum==PIn.Long(tableInsWriteoff.Rows[i]["Clinic"].ToString()))) {
					continue;//Using clinics and the current row is for a clinic that is NOT in the list of clinics we care about.
				}
				DataRow row=tableDailyProd.NewRow();
				row["Date"]=PIn.Date(tableInsWriteoff.Rows[i]["Date"].ToString()).ToShortDateString();
				row["Name"]=tableInsWriteoff.Rows[i]["namelf"].ToString();
				row["Description"]=tableInsWriteoff.Rows[i]["Description"].ToString();
				row["Provider"]=tableInsWriteoff.Rows[i]["Abbr"].ToString();
				if(_hasClinics) {
					row["Clinic"]=tableInsWriteoff.Rows[i]["Clinic"].ToString();
				}
				row["Production"]=0;
				row["Adjust"]=0;
				row["Writeoff"]=tableInsWriteoff.Rows[i]["WriteOff"].ToString();
				row["Pt Income"]=0;
				row["Ins Income"]=0;
				row["ClinicSplit"]=hasBreakdown ? tableInsWriteoff.Rows[i]["Clinic"].ToString():"";
				tableDailyProd.Rows.Add(row);
			}
			for(int i=0;i<tablePay.Rows.Count;i++) {
				if(_hasClinics && !listClinics.Exists(x => x.ClinicNum==PIn.Long(tablePay.Rows[i]["Clinic"].ToString()))) {
					continue;//Using clinics and the current row is for a clinic that is NOT in the list of clinics we care about.
				}
				DataRow row=tableDailyProd.NewRow();
				row["Date"]=PIn.Date(tablePay.Rows[i]["Date"].ToString()).ToShortDateString();
				row["Name"]=tablePay.Rows[i]["namelf"].ToString();
				row["Description"]=tablePay.Rows[i]["Description"].ToString();
				row["Provider"]=tablePay.Rows[i]["Abbr"].ToString();
				if(_hasClinics) {
					row["Clinic"]=tablePay.Rows[i]["Clinic"].ToString();
				}
				row["Production"]=0;
				row["Adjust"]=0;
				row["Writeoff"]=0;
				row["Pt Income"]=tablePay.Rows[i]["PayAmt"].ToString();
				row["Ins Income"]=0;
				row["ClinicSplit"]=hasBreakdown ? tablePay.Rows[i]["Clinic"].ToString():"";
				tableDailyProd.Rows.Add(row);
			}
			for(int i=0;i<tableIns.Rows.Count;i++) {
				if(_hasClinics && !listClinics.Exists(x => x.ClinicNum==PIn.Long(tableIns.Rows[i]["Clinic"].ToString()))) {
					continue;//Using clinics and the current row is for a clinic that is NOT in the list of clinics we care about.
				}
				DataRow row=tableDailyProd.NewRow();
				row["Date"]=PIn.Date(tableIns.Rows[i]["Date"].ToString()).ToShortDateString();
				row["Name"]=tableIns.Rows[i]["namelf"].ToString();
				row["Description"]=tableIns.Rows[i]["Description"].ToString();
				row["Provider"]=tableIns.Rows[i]["Abbr"].ToString();
				if(_hasClinics) {
					row["Clinic"]=tableIns.Rows[i]["Clinic"].ToString();
				}
				row["Production"]=0;
				row["Adjust"]=0;
				row["Writeoff"]=0;
				row["Pt Income"]=0;
				row["Ins Income"]=tableIns.Rows[i]["InsPayAmt"].ToString();
				row["ClinicSplit"]=hasBreakdown ? tableIns.Rows[i]["Clinic"].ToString():"";
				tableDailyProd.Rows.Add(row);
			}
			List<DataRow> listTableDailyProdRows=new List<DataRow>();
			for(int i=0;i<tableDailyProd.Rows.Count;i++) {
				listTableDailyProdRows.Add(tableDailyProd.Rows[i]);
			}
			listTableDailyProdRows.Sort(RowComparer);
			DataTable tableDailyProdSorted=tableDailyProd.Clone();
			tableDailyProdSorted.Rows.Clear();
			for(int i=0;i<listTableDailyProdRows.Count;i++) {
				tableDailyProdSorted.Rows.Add(listTableDailyProdRows[i].ItemArray);
				//Replace the ClinicNum with the actual description of the clinic.
				if(_hasClinics) {
					string clinicDesc=listClinics.Find(x => x.ClinicNum==PIn.Long(tableDailyProdSorted.Rows[i]["Clinic"].ToString())).Description;
					tableDailyProdSorted.Rows[i]["Clinic"]=clinicDesc=="" ? Lans.g("FormRpProdInc","Unassigned"):clinicDesc;
					if(hasBreakdown) {
						tableDailyProdSorted.Rows[i]["ClinicSplit"]=clinicDesc=="" ? Lans.g("FormRpProdInc","Unassigned"):clinicDesc;
					}
				}
			}
			DataSet ds=new DataSet("DailyData");
			ds.Tables.Add(tableDailyProdSorted);
			return ds;
		}

		///<summary></summary>
		public static DataSet GetProviderDataForClinics(DateTime dateFrom,DateTime dateTo,List<Provider> listProvs,List<Clinic> listClinics,bool writeOffPay,bool hasAllProvs,bool hasAllClinics) {
			if(listClinics.Count>0) {
				_hasClinics=true;
			}
			DataSet dataSet=GetDailyProdIncDataSet(dateFrom,dateTo,listProvs,listClinics,writeOffPay,hasAllProvs,hasAllClinics);
			DataTable tableProduction=dataSet.Tables["tableProduction"];
			DataTable tableAdj=dataSet.Tables["tableAdj"];
			DataTable tableInsWriteoff=dataSet.Tables["tableInsWriteoff"];
			DataTable tablePay=dataSet.Tables["tablePay"];
			DataTable tableIns=dataSet.Tables["tableIns"];
			decimal production;
			decimal adjust;
			decimal inswriteoff;
			decimal totalproduction;
			decimal ptincome;
			decimal insincome;
			decimal totalincome;
			DataTable dt=new DataTable("Total");
			dt.Columns.Add(new DataColumn("Provider"));
			dt.Columns.Add(new DataColumn("Production"));
			dt.Columns.Add(new DataColumn("Adjustments"));
			dt.Columns.Add(new DataColumn("Writeoff"));
			dt.Columns.Add(new DataColumn("Tot Prod"));
			dt.Columns.Add(new DataColumn("Pt Income"));
			dt.Columns.Add(new DataColumn("Ins Income"));
			dt.Columns.Add(new DataColumn("Total Income"));
			DataTable dtClinic=new DataTable("Clinic");
			dtClinic.Columns.Add(new DataColumn("Provider"));
			dtClinic.Columns.Add(new DataColumn("Production"));
			dtClinic.Columns.Add(new DataColumn("Adjustments"));
			dtClinic.Columns.Add(new DataColumn("Writeoff"));
			dtClinic.Columns.Add(new DataColumn("Tot Prod"));
			dtClinic.Columns.Add(new DataColumn("Pt Income"));
			dtClinic.Columns.Add(new DataColumn("Ins Income"));
			dtClinic.Columns.Add(new DataColumn("Total Income"));
			dtClinic.Columns.Add(new DataColumn("Clinic"));
			//length of array is number of months between the two dates plus one.
			//The from date and to date will not be more than one year and must will be within the same year due to FormRpProdInc UI validation enforcement.
			//Get a list of clinics so that we have access to their descriptions for the report.
			bool hasData;
			for(int it=0;it<listClinics.Count;it++) {//For each clinic
				for(int i=0;i<listProvs.Count;i++) {
					Provider provCur=listProvs[i];
					hasData=false;
					DataRow row=dtClinic.NewRow();
					row[0]=provCur.Abbr;
					production=0;
					adjust=0;
					inswriteoff=0;
					totalproduction=0;
					ptincome=0;
					insincome=0;
					totalincome=0;
					for(int j=0;j<tableProduction.Rows.Count;j++) {
						if(listClinics[it].ClinicNum==0 && tableProduction.Rows[j]["Clinic"].ToString()!="0") {
							continue;//Only counting unassigned this time around.
						}
						else if(listClinics[it].ClinicNum!=0 && tableProduction.Rows[j]["Clinic"].ToString()!=POut.Long(listClinics[it].ClinicNum)) {
							continue;
						}
						if(provCur.Abbr==PIn.String(tableProduction.Rows[j]["Abbr"].ToString())) {
							production+=PIn.Decimal(tableProduction.Rows[j]["Production"].ToString());
							hasData=true;
						}
					}
					for(int j=0;j<tableAdj.Rows.Count;j++) {
						if(listClinics[it].ClinicNum==0 && tableAdj.Rows[j]["Clinic"].ToString()!="0") {
							continue;
						}
						else if(listClinics[it].ClinicNum!=0 && tableAdj.Rows[j]["Clinic"].ToString()!=POut.Long(listClinics[it].ClinicNum)) {
							continue;
						}
						if(provCur.Abbr==PIn.String(tableAdj.Rows[j]["Abbr"].ToString())) {
							adjust+=PIn.Decimal(tableAdj.Rows[j]["AdjAmt"].ToString());
							hasData=true;
						}
					}
					for(int j=0;j<tableInsWriteoff.Rows.Count;j++) {
						if(listClinics[it].ClinicNum==0 && tableInsWriteoff.Rows[j]["Clinic"].ToString()!="0") {
							continue;
						}
						else if(listClinics[it].ClinicNum!=0 && tableInsWriteoff.Rows[j]["Clinic"].ToString()!=POut.Long(listClinics[it].ClinicNum)) {
							continue;
						}
						if(provCur.Abbr==PIn.String(tableInsWriteoff.Rows[j]["Abbr"].ToString())) {
							inswriteoff+=PIn.Decimal(tableInsWriteoff.Rows[j]["Writeoff"].ToString());
							hasData=true;
						}
					}
					for(int j=0;j<tablePay.Rows.Count;j++) {
						if(listClinics[it].ClinicNum==0 && tablePay.Rows[j]["Clinic"].ToString()!="0") {
							continue;
						}
						else if(listClinics[it].ClinicNum!=0 && tablePay.Rows[j]["Clinic"].ToString()!=POut.Long(listClinics[it].ClinicNum)) {
							continue;
						}
						if(provCur.Abbr==PIn.String(tablePay.Rows[j]["Abbr"].ToString())) {
							ptincome+=PIn.Decimal(tablePay.Rows[j]["PayAmt"].ToString());
							hasData=true;
						}
					}
					for(int j=0;j<tableIns.Rows.Count;j++) {
						if(listClinics[it].ClinicNum==0 && tableIns.Rows[j]["Clinic"].ToString()!="0") {
							continue;
						}
						else if(listClinics[it].ClinicNum!=0 && tableIns.Rows[j]["Clinic"].ToString()!=POut.Long(listClinics[it].ClinicNum)) {
							continue;
						}
						if(provCur.Abbr==PIn.String(tableIns.Rows[j]["Abbr"].ToString())) {
							insincome+=PIn.Decimal(tableIns.Rows[j]["InsPayAmt"].ToString());
							hasData=true;
						}
					}
					totalproduction=production+adjust+inswriteoff;
					totalincome=ptincome+insincome;
					string clinicDesc=listClinics[it].Description;
					row[1]=production.ToString("n");
					row[2]=adjust.ToString("n");
					row[3]=inswriteoff.ToString("n");
					row[4]=totalproduction.ToString("n");
					row[5]=ptincome.ToString("n");
					row[6]=insincome.ToString("n");
					row[7]=totalincome.ToString("n");
					row[8]=clinicDesc=="" ? Lans.g("FormRpProdInc","Unassigned"):clinicDesc;
					if(hasData) {
						dtClinic.Rows.Add(row);//prevents adding row if there is no data for the provider
					}
				}
			}
			for(int i=0;i<listProvs.Count;i++) {
				Provider provCur=listProvs[i];
				hasData=false;
				DataRow row=dt.NewRow();
				row[0]=provCur.Abbr;
				production=0;
				adjust=0;
				inswriteoff=0;
				totalproduction=0;
				ptincome=0;
				insincome=0;
				totalincome=0;
				for(int j=0;j<tableProduction.Rows.Count;j++) {
					if(provCur.Abbr==PIn.String(tableProduction.Rows[j]["Abbr"].ToString())) {
						production+=PIn.Decimal(tableProduction.Rows[j]["Production"].ToString());
						hasData=true;
					}
				}
				for(int j=0;j<tableAdj.Rows.Count;j++) {
					if(provCur.Abbr==PIn.String(tableAdj.Rows[j]["Abbr"].ToString())) {
						adjust+=PIn.Decimal(tableAdj.Rows[j]["AdjAmt"].ToString());
						hasData=true;
					}
				}
				for(int j=0;j<tableInsWriteoff.Rows.Count;j++) {
					if(provCur.Abbr==PIn.String(tableInsWriteoff.Rows[j]["Abbr"].ToString())) {
						inswriteoff+=PIn.Decimal(tableInsWriteoff.Rows[j]["Writeoff"].ToString());
						hasData=true;
					}
				}
				for(int j=0;j<tablePay.Rows.Count;j++) {
					if(provCur.Abbr==PIn.String(tablePay.Rows[j]["Abbr"].ToString())) {
						ptincome+=PIn.Decimal(tablePay.Rows[j]["PayAmt"].ToString());
						hasData=true;
					}
				}
				for(int j=0;j<tableIns.Rows.Count;j++) {
					if(provCur.Abbr==PIn.String(tableIns.Rows[j]["Abbr"].ToString())) {
						insincome+=PIn.Decimal(tableIns.Rows[j]["InsPayAmt"].ToString());
						hasData=true;
					}
				}
				totalproduction=production+adjust+inswriteoff;
				totalincome=ptincome+insincome;
				row[1]=production.ToString("n");
				row[2]=adjust.ToString("n");
				row[3]=inswriteoff.ToString("n");
				row[4]=totalproduction.ToString("n");
				row[5]=ptincome.ToString("n");
				row[6]=insincome.ToString("n");
				row[7]=totalincome.ToString("n");
				if(hasData) {
					dt.Rows.Add(row);//prevents adding row if there is no data for the provider
				}
			}
			DataSet ds=null;
			ds=new DataSet("ProviderData");
			ds.Tables.Add(dt);
			if(listClinics.Count!=0) {
				ds.Tables.Add(dtClinic);
			}
			return ds;
		}

		///<summary>Returns a dataset that contains 5 tables used to generate the daily report.  If not using clinics then simply supply an empty list of clinicNums.  Also used for the CEMT Provider P and I report</summary>
		public static DataSet GetDailyProdIncDataSet(DateTime dateFrom,DateTime dateTo,List<Provider> listProvs,List<Clinic> listClinics,bool writeOffPay,bool hasAllProvs,bool hasAllClinics) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetDS(MethodBase.GetCurrentMethod(),dateFrom,dateTo,listProvs,listClinics,writeOffPay,hasAllProvs,hasAllClinics);
			}
			List<long> listProvNums=new List<long>();
			for(int i=0;i<listProvs.Count;i++) {
				listProvNums.Add(listProvs[i].ProvNum);
			}
			List<long> listClinicNums=new List<long>();
			for(int i=0;i<listClinics.Count;i++) {
				listClinicNums.Add(listClinics[i].ClinicNum);
			}
			//Procedures------------------------------------------------------------------------------
			string whereProv="";
			if(!hasAllProvs && listProvNums.Count>0) {
				whereProv="AND provider.ProvNum IN ("+String.Join(",",listProvNums)+") ";
			}
			string whereClin="";
			if(!hasAllClinics && listClinicNums.Count>0) {
				whereClin="AND procedurelog.ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
			string command="SELECT "
				+"procedurelog.ProcDate Date, "
				+"CONCAT(CONCAT(CONCAT(CONCAT(patient.LName, ', '),patient.FName),' '),patient.MiddleI) namelf, "
				+"procedurecode.Descript Description, "
				+"provider.Abbr, "
				+"procedurelog.ClinicNum Clinic, "
				+"procedurelog.ProcFee*(CASE procedurelog.UnitQty+procedurelog.BaseUnits WHEN 0 THEN 1 ELSE procedurelog.UnitQty+procedurelog.BaseUnits END)-IFNULL(SUM(claimproc.WriteOff),0) Production, "
				+"procedurelog.ProcNum "
				+"FROM patient "
				+"INNER JOIN procedurelog ON patient.PatNum=procedurelog.PatNum "
					+"AND procedurelog.ProcStatus='2' "
					+"AND DATE(procedurelog.ProcDate) >= "+POut.Date(dateFrom)+" "
					+"AND DATE(procedurelog.ProcDate) <= "+POut.Date(dateTo)+" "
					+whereClin+" "
				+"LEFT JOIN claimproc ON procedurelog.ProcNum=claimproc.ProcNum "
					+"AND claimproc.Status='7' "
				+"INNER JOIN provider ON procedurelog.ProvNum=provider.ProvNum "
					+whereProv
				+"INNER JOIN procedurecode ON procedurelog.CodeNum=procedurecode.CodeNum "
				+"GROUP BY procedurelog.ProcNum "
				+"ORDER BY Date,namelf";
			DataTable tableProduction=Db.GetTable(command);
			tableProduction.TableName="tableProduction";
			//Adjustments----------------------------------------------------------------------------
			if(!hasAllClinics && listClinicNums.Count>0) {
				whereClin="AND adjustment.ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
			command="SELECT "
				+"adjustment.AdjDate Date, "
				+"CONCAT(CONCAT(CONCAT(CONCAT(patient.LName, ', '),patient.FName),' '),patient.MiddleI) namelf, "
				+"definition.ItemName Description, "
				+"provider.Abbr, "
				+"adjustment.ClinicNum Clinic, "
				+"adjustment.AdjAmt AdjAmt, "
				+"adjustment.AdjNum "
				+"FROM adjustment "
				+"INNER JOIN patient ON adjustment.PatNum=patient.PatNum "
				+"INNER JOIN definition ON adjustment.AdjType=definition.DefNum "
				+"INNER JOIN provider ON adjustment.ProvNum=provider.ProvNum "
					+whereProv
				+"WHERE adjustment.AdjDate >= "+POut.Date(dateFrom)+" "
					+"AND adjustment.AdjDate <= "+POut.Date(dateTo)+" "
					+whereClin+" "
				+"ORDER BY Date,namelf";
			DataTable tableAdj=Db.GetTable(command);
			tableAdj.TableName="tableAdj";
			//InsWriteoff--------------------------------------------------------------------------
			string whereInsWriteoffProvs="";
			if(!hasAllProvs && listProvNums.Count>0) {
				whereInsWriteoffProvs="AND claimproc.ProvNum IN ("+String.Join(",",listProvNums)+") ";
			}
			if(!hasAllClinics && listClinicNums.Count>0) {
				whereClin=" AND claimproc.ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
			if(writeOffPay) {
				command="SELECT claimproc.DateCP Date, "
					+"CONCAT(CONCAT(CONCAT(CONCAT(patient.LName,', '),patient.FName),' '),patient.MiddleI) namelf, "
					+"CONCAT(CONCAT(procedurecode.AbbrDesc,' '),carrier.CarrierName) Description, "//AbbrDesc might be null, which is ok.
					+"provider.Abbr, "
					+"claimproc.ClinicNum Clinic, "
					+"-SUM(claimproc.WriteOff) WriteOff, "
					+"claimproc.ClaimNum "
					+"FROM claimproc "
					+"LEFT JOIN patient ON claimproc.PatNum = patient.PatNum "
					+"LEFT JOIN provider ON provider.ProvNum = claimproc.ProvNum "
					+"LEFT JOIN insplan ON insplan.PlanNum = claimproc.PlanNum "
					+"LEFT JOIN carrier ON carrier.CarrierNum = insplan.CarrierNum "
					+"LEFT JOIN procedurelog ON procedurelog.ProcNum=claimproc.ProcNum "
					+"LEFT JOIN procedurecode ON procedurelog.CodeNum=procedurecode.CodeNum "
					+"WHERE (claimproc.Status=1 OR claimproc.Status=4) "//received or supplemental
					+whereInsWriteoffProvs
					+whereClin
					+"AND claimproc.WriteOff > '.0001' "
					+"AND claimproc.DateCP >= "+POut.Date(dateFrom)+" "
					+"AND claimproc.DateCP <= "+POut.Date(dateTo)+" "
					+"GROUP BY claimproc.ClaimProcNum "
					+"ORDER BY Date,namelf";
			}
			else {
				command="SELECT claimproc.ProcDate Date,"
					+"CONCAT(CONCAT(CONCAT(CONCAT(patient.LName,', '),patient.FName),' '),patient.MiddleI) namelf,"
					+"CONCAT(CONCAT(procedurecode.AbbrDesc,' '),carrier.CarrierName) Description,"
					+"provider.Abbr,"
					+"claimproc.ClinicNum Clinic,"
					+"-SUM(claimproc.WriteOff) WriteOff,"
					+"claimproc.ClaimNum "
					+"FROM claimproc "
					+"LEFT JOIN patient ON claimproc.PatNum = patient.PatNum "
					+"LEFT JOIN provider ON provider.ProvNum = claimproc.ProvNum "
					+"LEFT JOIN insplan ON insplan.PlanNum = claimproc.PlanNum "
					+"LEFT JOIN carrier ON carrier.CarrierNum = insplan.CarrierNum "
					+"LEFT JOIN procedurelog ON procedurelog.ProcNum=claimproc.ProcNum "
					+"LEFT JOIN procedurecode ON procedurelog.CodeNum=procedurecode.CodeNum "
					+"WHERE (claimproc.Status=1 OR claimproc.Status=4 OR claimproc.Status=0) "//received or supplemental or notreceived
					+whereInsWriteoffProvs
					+whereClin
					+"AND claimproc.WriteOff > '.0001' "
					+"AND claimproc.ProcDate >= "+POut.Date(dateFrom)+" "
					+"AND claimproc.ProcDate <= "+POut.Date(dateTo)+" "
					+"GROUP BY claimproc.ClaimProcNum "
					+"ORDER BY Date,namelf";
			}
			DataTable tableInsWriteoff=Db.GetTable(command);
			tableInsWriteoff.TableName="tableInsWriteoff";
			//PtIncome--------------------------------------------------------------------------------
			string wherePtIncomeProvs="";
			if(!hasAllProvs && listProvNums.Count>0) {
				wherePtIncomeProvs="AND paysplit.ProvNum IN ("+String.Join(",",listProvNums)+") ";
			}
			if(!hasAllClinics && listClinicNums.Count>0) {
				whereClin=" AND paysplit.ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
			command="SELECT "
				+"paysplit.DatePay Date, "
				+"CONCAT(CONCAT(CONCAT(CONCAT(patient.LName,', '),patient.FName),' '),patient.MiddleI) namelf, "
				+"definition.ItemName Description, "
				+"provider.Abbr, "
				+"paysplit.ClinicNum Clinic, "
				+"SUM(paysplit.SplitAmt) PayAmt, "
				+"payment.PayNum "
				+"FROM paysplit "
				+"LEFT JOIN payment ON payment.PayNum=paysplit.PayNum "
				+"LEFT JOIN patient ON patient.PatNum=paysplit.PatNum "
				+"LEFT JOIN provider ON provider.ProvNum=paysplit.ProvNum "
				+"LEFT JOIN definition ON payment.PayType=definition.DefNum "
				+"WHERE payment.PayDate >= "+POut.Date(dateFrom)+" "
				+"AND payment.PayDate <= "+POut.Date(dateTo)+" "
				+wherePtIncomeProvs
				+whereClin
				+"GROUP BY paysplit.PatNum,paysplit.ProvNum,paysplit.ClinicNum,PayType,paysplit.DatePay "
				+"ORDER BY Date,namelf";
			DataTable tablePay=Db.GetTable(command);
			tablePay.TableName="tablePay";
			//InsIncome---------------------------------------------------------------------------------
			if(!hasAllClinics && listClinicNums.Count>0) {
				whereClin=" AND claimproc.ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
			command="SELECT "
				+"claimpayment.CheckDate Date, "
				+"CONCAT(CONCAT(CONCAT(CONCAT(patient.LName,', '),patient.FName),' '),patient.MiddleI) namelf, "
				+"carrier.CarrierName Description, "
				+"provider.Abbr, "
				+"claimproc.ClinicNum Clinic, "
				+"SUM(claimproc.InsPayAmt) InsPayAmt, "
				+"claimproc.ClaimNum "
				+"FROM claimproc "
				+"INNER JOIN patient ON claimproc.PatNum=patient.PatNum "
				+"INNER JOIN insplan ON claimproc.PlanNum=insplan.PlanNum "
				+"INNER JOIN carrier ON insplan.CarrierNum=carrier.CarrierNum "
				+"INNER JOIN provider ON claimproc.ProvNum=provider.ProvNum "
					+whereProv+" "
				+"INNER JOIN claimpayment ON claimproc.ClaimPaymentNum=claimpayment.ClaimPaymentNum "
				+"WHERE (claimproc.Status=1 OR claimproc.Status=4) "//received or supplemental
					+"AND claimpayment.CheckDate >= "+POut.Date(dateFrom)+" "
					+"AND claimpayment.CheckDate <= "+POut.Date(dateTo)+" "
					+whereClin+" "
				+"GROUP BY claimproc.PatNum,claimproc.ProvNum,claimproc.PlanNum,claimproc.ClinicNum,claimpayment.CheckDate "
				+"ORDER BY Date,namelf";
			DataTable tableIns=Db.GetTable(command);
			tableIns.TableName="tableIns";
			DataSet dataSet=new DataSet();
			dataSet.Tables.Add(tableProduction);
			dataSet.Tables.Add(tableAdj);
			dataSet.Tables.Add(tableInsWriteoff);
			dataSet.Tables.Add(tablePay);
			dataSet.Tables.Add(tableIns);
			return dataSet;
		}
		#endregion

		#region Monthly P&I Report
		///<summary>If not using clinics then supply an empty list of clinics.</summary>
		public static DataSet GetMonthlyData(DateTime dateFrom,DateTime dateTo,List<Provider> listProvs,List<Clinic> listClinics,bool writeOffPay,bool hasAllProvs,bool hasAllClinics) {
			//No need to check RemotingRole; no call to db.
			if(listClinics.Count>0) {
				_hasClinics=true;
			}
			DataSet dataSet=GetMonthlyProdIncDataSet(dateFrom,dateTo,listProvs,listClinics,writeOffPay,hasAllProvs,hasAllClinics);
			DataTable tableProduction=dataSet.Tables["tableProduction"];
			DataTable tableAdj=dataSet.Tables["tableAdj"];
			DataTable tableInsWriteoff=dataSet.Tables["tableInsWriteoff"];
			DataTable tablePay=dataSet.Tables["tablePay"];
			DataTable tableIns=dataSet.Tables["tableIns"];
			DataTable tableSched=dataSet.Tables["tableSched"];
			decimal sched;
			decimal production;
			decimal adjust;
			decimal inswriteoff;	//spk 5/19/05
			decimal totalproduction;
			decimal ptincome;
			decimal insincome;
			decimal totalincome;
			DataTable dt=new DataTable("Total");
			dt.Columns.Add(new DataColumn("Month"));
			dt.Columns.Add(new DataColumn("Weekday"));
			dt.Columns.Add(new DataColumn("Production"));
			dt.Columns.Add(new DataColumn("Sched"));
			dt.Columns.Add(new DataColumn("Adjustments"));
			dt.Columns.Add(new DataColumn("Writeoff"));
			dt.Columns.Add(new DataColumn("Tot Prod"));
			dt.Columns.Add(new DataColumn("Pt Income"));
			dt.Columns.Add(new DataColumn("Ins Income"));
			dt.Columns.Add(new DataColumn("Total Income"));
			DataTable dtClinic=new DataTable("Clinic");
			dtClinic.Columns.Add(new DataColumn("Month"));
			dtClinic.Columns.Add(new DataColumn("Weekday"));
			dtClinic.Columns.Add(new DataColumn("Production"));
			dtClinic.Columns.Add(new DataColumn("Sched"));
			dtClinic.Columns.Add(new DataColumn("Adjustments"));
			dtClinic.Columns.Add(new DataColumn("Writeoff"));
			dtClinic.Columns.Add(new DataColumn("Tot Prod"));
			dtClinic.Columns.Add(new DataColumn("Pt Income"));
			dtClinic.Columns.Add(new DataColumn("Ins Income"));
			dtClinic.Columns.Add(new DataColumn("Total Income"));
			dtClinic.Columns.Add(new DataColumn("Clinic"));
			//length of array is number of months between the two dates plus one.
			//The from date and to date will not be more than one year and must will be within the same year due to FormRpProdInc UI validation enforcement.
			DateTime[] dates=null;
			dates=new DateTime[dateTo.Subtract(dateFrom).Days+1];//Make a DateTime array with one position for each day in the report.
			//Get a list of clinics so that we have access to their descriptions for the report.
			for(int it=0;it<listClinics.Count;it++) {//For each clinic
				for(int i=0;i<dates.Length;i++) {//usually 12 months in loop for annual.  Loop through the DateTime array, each position represents one date in the report.
					dates[i]=dateFrom.AddDays(i);//Monthly/Daily report, add a day
					DataRow row=dtClinic.NewRow();
					row[0]=dates[i].ToShortDateString();
					row[1]=dates[i].DayOfWeek.ToString();
					sched=0;
					production=0;
					adjust=0;
					inswriteoff=0;	//spk 5/19/05
					totalproduction=0;
					ptincome=0;
					insincome=0;
					totalincome=0;
					for(int j=0;j<tableProduction.Rows.Count;j++) {
						if(listClinics[it].ClinicNum==0 && tableProduction.Rows[j]["ClinicNum"].ToString()!="0") {
							continue;//Only counting unassigned this time around.
						}
						else if(listClinics[it].ClinicNum!=0 && tableProduction.Rows[j]["ClinicNum"].ToString()!=POut.Long(listClinics[it].ClinicNum)) {
							continue;
						}
						if(dates[i].Year==PIn.Date(tableProduction.Rows[j]["ProcDate"].ToString()).Year
								&& dates[i].Month==PIn.Date(tableProduction.Rows[j]["ProcDate"].ToString()).Month) //If the proc was in the month and year that we're making a row for
						{
							if(dates[i].Day==PIn.Date(tableProduction.Rows[j]["ProcDate"].ToString()).Day) {//If the proc is also on the day (Only monthly report)
								production+=PIn.Decimal(tableProduction.Rows[j]["Production"].ToString());
							}
						}
					}
					for(int j=0;j<tableAdj.Rows.Count;j++) {
						if(listClinics[it].ClinicNum==0 && tableAdj.Rows[j]["ClinicNum"].ToString()!="0") {
							continue;
						}
						else if(listClinics[it].ClinicNum!=0 && tableAdj.Rows[j]["ClinicNum"].ToString()!=POut.Long(listClinics[it].ClinicNum)) {
							continue;
						}
						if(dates[i].Year==PIn.Date(tableAdj.Rows[j]["AdjDate"].ToString()).Year
								&& dates[i].Month==PIn.Date(tableAdj.Rows[j]["AdjDate"].ToString()).Month) //If the adjustment was in the month and year that we're making a row for.
						{
							if(dates[i].Day==PIn.Date(tableAdj.Rows[j]["AdjDate"].ToString()).Day) {//If the adjustment is also on the day (Only monthly report)
								adjust+=PIn.Decimal(tableAdj.Rows[j]["Adjustment"].ToString());
							}
						}
					}
					for(int j=0;j<tableInsWriteoff.Rows.Count;j++) {
						if(listClinics[it].ClinicNum==0 && tableInsWriteoff.Rows[j]["ClinicNum"].ToString()!="0") {
							continue;
						}
						else if(listClinics[it].ClinicNum!=0 && tableInsWriteoff.Rows[j]["ClinicNum"].ToString()!=POut.Long(listClinics[it].ClinicNum)) {
							continue;
						}
						if(dates[i].Year==PIn.Date(tableInsWriteoff.Rows[j]["ClaimDate"].ToString()).Year
								&& dates[i].Month==PIn.Date(tableInsWriteoff.Rows[j]["ClaimDate"].ToString()).Month) //If the claim writeoff was in the month and year that we're making a row for.
						{
							if(dates[i].Day==PIn.Date(tableInsWriteoff.Rows[j]["ClaimDate"].ToString()).Day) {//If the claim is also on the day (Only monthly report)
								inswriteoff-=PIn.Decimal(tableInsWriteoff.Rows[j]["Writeoff"].ToString());
							}
						}
					}
					for(int j=0;j<tableSched.Rows.Count;j++) {
						if(listClinics[it].ClinicNum==0 && tableSched.Rows[j]["ClinicNum"].ToString()!="0") {
							continue;
						}
						else if(listClinics[it].ClinicNum!=0 && tableSched.Rows[j]["ClinicNum"].ToString()!=POut.Long(listClinics[it].ClinicNum)) {
							continue;
						}
						if(dates[i]==(PIn.Date(tableSched.Rows[j]["SchedDate"].ToString()))) {
							sched+=PIn.Decimal(tableSched.Rows[j]["Amount"].ToString());
						}
					}
					for(int j=0;j<tablePay.Rows.Count;j++) {
						if(listClinics[it].ClinicNum==0 && tablePay.Rows[j]["ClinicNum"].ToString()!="0") {
							continue;
						}
						else if(listClinics[it].ClinicNum!=0 && tablePay.Rows[j]["ClinicNum"].ToString()!=POut.Long(listClinics[it].ClinicNum)) {
							continue;
						}
						if(dates[i].Year==PIn.Date(tablePay.Rows[j]["DatePay"].ToString()).Year
								&& dates[i].Month==PIn.Date(tablePay.Rows[j]["DatePay"].ToString()).Month) //If the payment was in the month and year that we're making a row for.
						{
							if(dates[i].Day==PIn.Date(tablePay.Rows[j]["DatePay"].ToString()).Day) {//If the payment is also on the day (Only monthly report)
								ptincome+=PIn.Decimal(tablePay.Rows[j]["Income"].ToString());
							}
						}
					}
					for(int j=0;j<tableIns.Rows.Count;j++) {
						if(listClinics[it].ClinicNum==0 && tableIns.Rows[j]["ClinicNum"].ToString()!="0") {
							continue;
						}
						else if(listClinics[it].ClinicNum!=0 && tableIns.Rows[j]["ClinicNum"].ToString()!=POut.Long(listClinics[it].ClinicNum)) {
							continue;
						}
						if(dates[i].Year==PIn.Date(tableIns.Rows[j]["CheckDate"].ToString()).Year
								&& dates[i].Month==PIn.Date(tableIns.Rows[j]["CheckDate"].ToString()).Month) //If the ins payment was in the month and year that we're making a row for.
						{
							if(dates[i].Day==PIn.Date(tableIns.Rows[j]["CheckDate"].ToString()).Day) {//If the ins payment is also on the day (Only monthly report)
								insincome+=PIn.Decimal(tableIns.Rows[j]["Ins"].ToString());
							}
						}
					}
					totalproduction=production+adjust+inswriteoff+sched;
					totalincome=ptincome+insincome;
					string clinicDesc=listClinics[it].Description;
					row[2]=production.ToString("n");
					row[3]=sched.ToString("n");
					row[4]=adjust.ToString("n");
					row[5]=inswriteoff.ToString("n");
					row[6]=totalproduction.ToString("n");
					row[7]=ptincome.ToString("n");
					row[8]=insincome.ToString("n");
					row[9]=totalincome.ToString("n");
					row[10]=clinicDesc=="" ? Lans.g("FormRpProdInc","Unassigned"):clinicDesc;
					dtClinic.Rows.Add(row);
				}
			}
			for(int i=0;i<dates.Length;i++) {//usually 12 months in loop
				dates[i]=dateFrom.AddDays(i);
				DataRow row=dt.NewRow();
				row[0]=dates[i].ToShortDateString();
				row[1]=dates[i].DayOfWeek.ToString();
				sched=0;
				production=0;
				adjust=0;
				inswriteoff=0;	//spk 5/19/05
				totalproduction=0;
				ptincome=0;
				insincome=0;
				totalincome=0;
				for(int j=0;j<tableProduction.Rows.Count;j++) {
					if(dates[i].Year==PIn.Date(tableProduction.Rows[j]["ProcDate"].ToString()).Year
						&& dates[i].Month==PIn.Date(tableProduction.Rows[j]["ProcDate"].ToString()).Month) {
						if(dates[i].Day==PIn.Date(tableProduction.Rows[j]["ProcDate"].ToString()).Day) {//If the proc is also on the day (Only monthly report)
							production+=PIn.Decimal(tableProduction.Rows[j]["Production"].ToString());
						}
					}
				}
				for(int j=0;j<tableAdj.Rows.Count;j++) {
					if(dates[i].Year==PIn.Date(tableAdj.Rows[j]["AdjDate"].ToString()).Year
						&& dates[i].Month==PIn.Date(tableAdj.Rows[j]["AdjDate"].ToString()).Month) {
						if(dates[i].Day==PIn.Date(tableAdj.Rows[j]["AdjDate"].ToString()).Day) {
							adjust+=PIn.Decimal(tableAdj.Rows[j]["Adjustment"].ToString());
						}
					}
				}
				for(int j=0;j<tableInsWriteoff.Rows.Count;j++) {
					if(dates[i].Year==PIn.Date(tableInsWriteoff.Rows[j]["ClaimDate"].ToString()).Year
						&& dates[i].Month==PIn.Date(tableInsWriteoff.Rows[j]["ClaimDate"].ToString()).Month) {
						if(dates[i].Day==PIn.Date(tableInsWriteoff.Rows[j]["ClaimDate"].ToString()).Day) {
							inswriteoff-=PIn.Decimal(tableInsWriteoff.Rows[j]["Writeoff"].ToString());
						}
					}
				}
				for(int j=0;j<tableSched.Rows.Count;j++) {
					if(dates[i]==(PIn.Date(tableSched.Rows[j]["SchedDate"].ToString()))) {
						sched+=PIn.Decimal(tableSched.Rows[j]["Amount"].ToString());
					}
				}
				for(int j=0;j<tablePay.Rows.Count;j++) {
					if(dates[i].Year==PIn.Date(tablePay.Rows[j]["DatePay"].ToString()).Year
						&& dates[i].Month==PIn.Date(tablePay.Rows[j]["DatePay"].ToString()).Month) {
						if(dates[i].Day==PIn.Date(tablePay.Rows[j]["DatePay"].ToString()).Day) {
							ptincome+=PIn.Decimal(tablePay.Rows[j]["Income"].ToString());
						}
					}
				}
				for(int j=0;j<tableIns.Rows.Count;j++) {
					if(dates[i].Year==PIn.Date(tableIns.Rows[j]["CheckDate"].ToString()).Year
						&& dates[i].Month==PIn.Date(tableIns.Rows[j]["CheckDate"].ToString()).Month) {
						if(dates[i].Day==PIn.Date(tableIns.Rows[j]["CheckDate"].ToString()).Day) {
							insincome+=PIn.Decimal(tableIns.Rows[j]["Ins"].ToString());
						}
					}
				}
				totalproduction=production+adjust+inswriteoff+sched;
				totalincome=ptincome+insincome;
				row[2]=production.ToString("n");
				row[3]=sched.ToString("n");
				row[4]=adjust.ToString("n");
				row[5]=inswriteoff.ToString("n");
				row[6]=totalproduction.ToString("n");
				row[7]=ptincome.ToString("n");
				row[8]=insincome.ToString("n");
				row[9]=totalincome.ToString("n");
				dt.Rows.Add(row);
			}
			DataSet ds=null;
			ds=new DataSet("MonthlyData");
			ds.Tables.Add(dt);
			if(listClinics.Count!=0) {
				ds.Tables.Add(dtClinic);
			}
			return ds;
		}

		///<summary>Returns a dataset that contains 5 tables used to generate the monthly report. If not using clinics then supply an empty list of clinics.
		/// Does not work for Oracle (by chance not by design). Consider enhancing with DbHelper.Year(),DbHelper.Month(), DbHelper.Day() and enhancing the GroupBy Logic.</summary>
		public static DataSet GetMonthlyProdIncDataSet(DateTime dateFrom,DateTime dateTo,List<Provider> listProvs,List<Clinic> listClinics,bool writeOffPay,bool hasAllProvs,bool hasAllClinics) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetDS(MethodBase.GetCurrentMethod(),dateFrom,dateTo,listProvs,listClinics,writeOffPay,hasAllProvs,hasAllClinics);
			}
			List<long> listClinicNums=new List<long>();
			for(int i=0;i<listClinics.Count;i++) {
				listClinicNums.Add(listClinics[i].ClinicNum);
			}
			List<long> listProvNums=new List<long>();
			for(int i=0;i<listProvs.Count;i++) {
				listProvNums.Add(listProvs[i].ProvNum);
			}
			//Procedures------------------------------------------------------------------------------
			string whereProv="";
			if(!hasAllProvs && listProvNums.Count>0) {
				whereProv=" AND procedurelog.ProvNum IN ("+String.Join(",",listProvNums)+") ";
			}
			string whereClin="";
			if(!hasAllClinics && listClinicNums.Count>0) {
				whereClin=" AND procedurelog.ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
			//==Travis (04/11/2014): In the case that you have two capitation plans for a single patient the query below will cause a duplicate row, incorectly increasing your production.
			//	We now state in the manual that having two capitation plans is not advised and will cause reporting to be off.
			string command="SELECT "
				+"procedurelog.ProcDate,procedurelog.ClinicNum,"
				+"SUM(procedurelog.ProcFee*(procedurelog.UnitQty+procedurelog.BaseUnits))-IFNULL(SUM(claimproc.WriteOff),0) Production "
				+"FROM procedurelog "
				+"LEFT JOIN claimproc ON procedurelog.ProcNum=claimproc.ProcNum "
				+"AND claimproc.Status='7' "//only CapComplete writeoffs are subtracted here.
				+"WHERE procedurelog.ProcStatus = '2' "
				+whereProv
				+whereClin
				+"AND procedurelog.ProcDate >= " +POut.Date(dateFrom)+" "
				+"AND procedurelog.ProcDate <= " +POut.Date(dateTo)+" "
				+"GROUP BY ClinicNum,YEAR(procedurelog.ProcDate),MONTH(procedurelog.ProcDate),DAY(procedurelog.ProcDate)";//Does not work for Oracle. Consider enhancing with DbHelper.Year(),DbHelper.Month()
			command+=" ORDER BY ClinicNum,ProcDate";
			DataTable tableProduction=Db.GetTable(command);
			tableProduction.TableName="tableProduction";
			//Adjustments----------------------------------------------------------------------------
			if(!hasAllProvs && listProvNums.Count>0) {
				whereProv=" AND adjustment.ProvNum IN ("+String.Join(",",listProvNums)+") ";
			}
			if(!hasAllClinics && listClinicNums.Count>0) {
				whereClin=" AND adjustment.ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
			command="SELECT "
				+"adjustment.AdjDate,"
				+"adjustment.ClinicNum,"
				+"SUM(adjustment.AdjAmt) Adjustment "
				+"FROM adjustment "
				+"WHERE adjustment.AdjDate >= "+POut.Date(dateFrom)+" "
				+"AND adjustment.AdjDate <= "+POut.Date(dateTo)+" "
				+whereProv
				+whereClin
				+"GROUP BY ClinicNum,YEAR(adjustment.AdjDate),MONTH(adjustment.AdjDate),DAY(adjustment.AdjDate)";
			command+=" ORDER BY ClinicNum,AdjDate";
			DataTable tableAdj=Db.GetTable(command);
			tableAdj.TableName="tableAdj";
			//TableInsWriteoff--------------------------------------------------------------------------
			if(!hasAllProvs && listProvNums.Count>0) {
				whereProv=" AND claimproc.ProvNum IN ("+String.Join(",",listProvNums)+") ";
			}
			if(!hasAllClinics && listClinicNums.Count>0) {
				whereClin=" AND claimproc.ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
			if(writeOffPay) {
				command="SELECT "
					+"claimproc.DateCP ClaimDate," 
					+"claimproc.ClinicNum,"
					+"SUM(claimproc.WriteOff) WriteOff "
					+"FROM claimproc "
					+"WHERE claimproc.DateCP >= "+POut.Date(dateFrom)+" "
					+"AND claimproc.DateCP <= "+POut.Date(dateTo)+" "
					+whereProv
					+whereClin
					+"AND (claimproc.Status=1 OR claimproc.Status=4) "//Received or supplemental
					+"GROUP BY ClinicNum,YEAR(claimproc.DateCP),MONTH(claimproc.DateCP),DAY(claimproc.DateCP)";
				command+=" ORDER BY ClinicNum,DateCP";
			}
			else {
				command="SELECT "
					+"claimproc.ProcDate ClaimDate," 
					+"claimproc.ClinicNum,"
					+"SUM(claimproc.WriteOff) WriteOff "
					+"FROM claimproc "
					+"WHERE claimproc.ProcDate >= "+POut.Date(dateFrom)+" "
					+"AND claimproc.ProcDate <= "+POut.Date(dateTo)+" "
					+whereProv
					+whereClin
					+"AND (claimproc.Status=1 OR claimproc.Status=4 OR claimproc.Status=0) " //received or supplemental or notreceived
					+"GROUP BY ClinicNum,YEAR(claimproc.ProcDate), MONTH(claimproc.ProcDate),DAY(claimproc.ProcDate)";
				command+=" ORDER BY ClinicNum,ProcDate";
			}
			DataTable tableInsWriteoff=Db.GetTable(command);
			tableInsWriteoff.TableName="tableInsWriteoff";
			//TableSched------------------------------------------------------------------------------
			DataTable tableSched=new DataTable();
			//Reads from the procedurelog table instead of claimproc because we are looking for scheduled procedures.
			if(!hasAllProvs && listProvNums.Count>0) {
				whereProv=" AND procedurelog.ProvNum IN ("+String.Join(",",listProvNums)+") ";
			}
			if(!hasAllClinics && listClinicNums.Count>0) {
				whereClin=" AND procedurelog.ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
			command= "SELECT "+DbHelper.DtimeToDate("t.AptDateTime")+" SchedDate,SUM(t.Fee-t.WriteoffEstimate) Amount,ClinicNum "
				+"FROM (SELECT appointment.AptDateTime,IFNULL(procedurelog.ProcFee,0) Fee,appointment.ClinicNum,";
			if(Prefs.GetBoolNoCache(PrefName.ReportPandIschedProdSubtractsWO)) {
				command+="SUM(IFNULL(CASE WHEN WriteOffEstOverride != -1 THEN WriteOffEstOverride ELSE WriteOffEst END,0)) WriteoffEstimate ";
			}
			else {
				command+="0 WriteoffEstimate ";
			}
			command+="FROM appointment "
				+"LEFT JOIN procedurelog ON appointment.AptNum = procedurelog.AptNum AND procedurelog.ProcStatus="+POut.Int((int)ProcStat.TP)+" "
				+"LEFT JOIN claimproc ON procedurelog.ProcNum = claimproc.ProcNum AND claimproc.Status="+POut.Int((int)ClaimProcStatus.Estimate)+" "
					+" AND (WriteOffEst != -1 OR WriteOffEstOverride != -1) "
				+"WHERE (appointment.AptStatus = "+POut.Int((int)ApptStatus.Scheduled)+" OR "
					+"appointment.AptStatus = "+POut.Int((int)ApptStatus.ASAP)+") "
				+"AND "+DbHelper.DtimeToDate("appointment.AptDateTime")+" >= "+POut.Date(dateFrom)+" "
				+"AND "+DbHelper.DtimeToDate("appointment.AptDateTime")+" <= "+POut.Date(dateTo)+" "
				+whereProv
				+whereClin
				+" GROUP BY procedurelog.ProcNum) t "//without this, there can be duplicate proc rows due to the claimproc join with dual insurance.
				+"GROUP BY SchedDate,ClinicNum "
				+"ORDER BY SchedDate";
			tableSched=Db.GetTable(command);
			tableSched.TableName="tableSched";
			//PtIncome--------------------------------------------------------------------------------
			if(!hasAllProvs && listProvNums.Count>0) {
				whereProv=" AND paysplit.ProvNum IN ("+String.Join(",",listProvNums)+") ";
			}
			if(!hasAllClinics && listClinicNums.Count>0) {
				whereClin=" AND paysplit.ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
			command="SELECT "
				+"paysplit.DatePay,"
				+"paysplit.ClinicNum,"
				+"SUM(paysplit.SplitAmt) Income "
				+"FROM paysplit "
				+"WHERE paysplit.IsDiscount=0 "//AND paysplit.PayNum=payment.PayNum "
				+whereProv
				+whereClin
				+"AND paysplit.DatePay >= "+POut.Date(dateFrom)+" "
				+"AND paysplit.DatePay <= "+POut.Date(dateTo)+" "
				+"GROUP BY ClinicNum,YEAR(paysplit.DatePay),MONTH(paysplit.DatePay),DAY(paysplit.DatePay)";
			command+=" ORDER BY ClinicNum,DatePay";
			DataTable tablePay=Db.GetTable(command);
			tablePay.TableName="tablePay";
			//InsIncome---------------------------------------------------------------------------------
			if(!hasAllProvs && listProvNums.Count>0) {
				whereProv=" AND claimproc.ProvNum IN ("+String.Join(",",listProvNums)+") ";
			}
			if(!hasAllClinics && listClinicNums.Count>0) {
				whereClin=" AND claimproc.ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
			command="SELECT claimpayment.CheckDate,claimproc.ClinicNum,SUM(claimproc.InsPayamt) Ins "
				+"FROM claimpayment,claimproc WHERE "
				+"claimproc.ClaimPaymentNum = claimpayment.ClaimPaymentNum "
				+"AND claimpayment.CheckDate >= " + POut.Date(dateFrom)+" "
				+"AND claimpayment.CheckDate <= " + POut.Date(dateTo)+" "
				+whereProv
				+whereClin
				+" GROUP BY claimpayment.CheckDate,ClinicNum ORDER BY ClinicNum,CheckDate";
			DataTable tableIns=Db.GetTable(command);
			tableIns.TableName="tableIns";
			DataSet dataSet=new DataSet();
			dataSet.Tables.Add(tableProduction);
			dataSet.Tables.Add(tableAdj);
			dataSet.Tables.Add(tableInsWriteoff);
			dataSet.Tables.Add(tablePay);
			dataSet.Tables.Add(tableIns);
			dataSet.Tables.Add(tableSched);
			return dataSet;
		}
		#endregion

		#region Annual P&I Report
		///<summary>If not using clinics then supply an empty list of clinics.</summary>
		public static DataSet GetAnnualData(DateTime dateFrom,DateTime dateTo,List<Provider> listProvs,List<Clinic> listClinics,bool writeOffPay,bool hasAllProvs,bool hasAllClinics) {
			//No need to check RemotingRole; no call to db.
			//No need to check RemotingRole; no call to db.
			if(listClinics.Count>0) {
				_hasClinics=true;
			}
			DataSet dataSet=GetAnnualProdIncDataSet(dateFrom,dateTo,listProvs,listClinics,writeOffPay,hasAllProvs,hasAllClinics);
			DataTable tableProduction=dataSet.Tables["tableProduction"];
			DataTable tableAdj=dataSet.Tables["tableAdj"];
			DataTable tableInsWriteoff=dataSet.Tables["tableInsWriteoff"];
			DataTable tablePay=dataSet.Tables["tablePay"];
			DataTable tableIns=dataSet.Tables["tableIns"];
			decimal production;
			decimal adjust;
			decimal inswriteoff;	//spk 5/19/05
			decimal totalproduction;
			decimal ptincome;
			decimal insincome;
			decimal totalincome;
			DataTable dt=new DataTable("Total");
			dt.Columns.Add(new DataColumn("Month"));
			dt.Columns.Add(new DataColumn("Production"));
			dt.Columns.Add(new DataColumn("Adjustments"));
			dt.Columns.Add(new DataColumn("Writeoff"));
			dt.Columns.Add(new DataColumn("Tot Prod"));
			dt.Columns.Add(new DataColumn("Pt Income"));
			dt.Columns.Add(new DataColumn("Ins Income"));
			dt.Columns.Add(new DataColumn("Total Income"));
			DataTable dtClinic=new DataTable("Clinic");
			dtClinic.Columns.Add(new DataColumn("Month"));
			dtClinic.Columns.Add(new DataColumn("Production"));
			dtClinic.Columns.Add(new DataColumn("Adjustments"));
			dtClinic.Columns.Add(new DataColumn("Writeoff"));
			dtClinic.Columns.Add(new DataColumn("Tot Prod"));
			dtClinic.Columns.Add(new DataColumn("Pt Income"));
			dtClinic.Columns.Add(new DataColumn("Ins Income"));
			dtClinic.Columns.Add(new DataColumn("Total Income"));
			dtClinic.Columns.Add(new DataColumn("Clinic"));
			//length of array is number of months between the two dates plus one.
			//The from date and to date will not be more than one year and must will be within the same year due to FormRpProdInc UI validation enforcement.
			DateTime[] dates=null;
			dates=new DateTime[(dateTo.Year-dateFrom.Year)*12+(dateTo.Month-dateFrom.Month)+1];
			//Make a DateTime array representing one position for each month in the report.  User can't specify different years in the report.
			//Get a list of clinics so that we have access to their descriptions for the report.
			for(int it=0;it<listClinics.Count;it++) {//For each clinic
				for(int i=0;i<dates.Length;i++) {//usually 12 months in loop for annual.  Loop through the DateTime array, each position represents one date in the report.
					dates[i]=dateFrom.AddMonths(i);//only the month and year are important.  For each month slot, add i to the dateFrom and put it in the array.
					DataRow row=dtClinic.NewRow();
					row[0]=dates[i].ToString("MMM yyyy");//JAN 2014
					production=0;
					adjust=0;
					inswriteoff=0;	//spk 5/19/05
					totalproduction=0;
					ptincome=0;
					insincome=0;
					totalincome=0;
					for(int j=0;j<tableProduction.Rows.Count;j++) {
						if(listClinics[it].ClinicNum==0 && tableProduction.Rows[j]["ClinicNum"].ToString()!="0") {
							continue;//Only counting unassigned this time around.
						}
						else if(listClinics[it].ClinicNum!=0 && tableProduction.Rows[j]["ClinicNum"].ToString()!=POut.Long(listClinics[it].ClinicNum)) {
							continue;
						}
						if(dates[i].Year==PIn.Date(tableProduction.Rows[j]["ProcDate"].ToString()).Year
								&& dates[i].Month==PIn.Date(tableProduction.Rows[j]["ProcDate"].ToString()).Month) //If the proc was in the month and year that we're making a row for
						{
							production+=PIn.Decimal(tableProduction.Rows[j]["Production"].ToString());
						}
					}
					for(int j=0;j<tableAdj.Rows.Count;j++) {
						if(listClinics[it].ClinicNum==0 && tableAdj.Rows[j]["ClinicNum"].ToString()!="0") {
							continue;
						}
						else if(listClinics[it].ClinicNum!=0 && tableAdj.Rows[j]["ClinicNum"].ToString()!=POut.Long(listClinics[it].ClinicNum)) {
							continue;
						}
						if(dates[i].Year==PIn.Date(tableAdj.Rows[j]["AdjDate"].ToString()).Year
								&& dates[i].Month==PIn.Date(tableAdj.Rows[j]["AdjDate"].ToString()).Month) //If the adjustment was in the month and year that we're making a row for.
						{
							adjust+=PIn.Decimal(tableAdj.Rows[j]["Adjustment"].ToString());
						}
					}
					for(int j=0;j<tableInsWriteoff.Rows.Count;j++) {
						if(listClinics[it].ClinicNum==0 && tableInsWriteoff.Rows[j]["ClinicNum"].ToString()!="0") {
							continue;
						}
						else if(listClinics[it].ClinicNum!=0 && tableInsWriteoff.Rows[j]["ClinicNum"].ToString()!=POut.Long(listClinics[it].ClinicNum)) {
							continue;
						}
						if(dates[i].Year==PIn.Date(tableInsWriteoff.Rows[j]["ClaimDate"].ToString()).Year
								&& dates[i].Month==PIn.Date(tableInsWriteoff.Rows[j]["ClaimDate"].ToString()).Month) //If the claim writeoff was in the month and year that we're making a row for.
						{
							inswriteoff-=PIn.Decimal(tableInsWriteoff.Rows[j]["WriteOff"].ToString());
						}
					}
					for(int j=0;j<tablePay.Rows.Count;j++) {
						if(listClinics[it].ClinicNum==0 && tablePay.Rows[j]["ClinicNum"].ToString()!="0") {
							continue;
						}
						else if(listClinics[it].ClinicNum!=0 && tablePay.Rows[j]["ClinicNum"].ToString()!=POut.Long(listClinics[it].ClinicNum)) {
							continue;
						}
						if(dates[i].Year==PIn.Date(tablePay.Rows[j]["DatePay"].ToString()).Year
								&& dates[i].Month==PIn.Date(tablePay.Rows[j]["DatePay"].ToString()).Month) //If the payment was in the month and year that we're making a row for.
						{
							ptincome+=PIn.Decimal(tablePay.Rows[j]["Income"].ToString());
						}
					}
					for(int j=0;j<tableIns.Rows.Count;j++) {
						if(listClinics[it].ClinicNum==0 && tableIns.Rows[j]["ClinicNum"].ToString()!="0") {
							continue;
						}
						else if(listClinics[it].ClinicNum!=0 && tableIns.Rows[j]["ClinicNum"].ToString()!=POut.Long(listClinics[it].ClinicNum)) {
							continue;
						}
						if(dates[i].Year==PIn.Date(tableIns.Rows[j]["CheckDate"].ToString()).Year
								&& dates[i].Month==PIn.Date(tableIns.Rows[j]["CheckDate"].ToString()).Month) //If the ins payment was in the month and year that we're making a row for.
						{
							insincome+=PIn.Decimal(tableIns.Rows[j]["Ins"].ToString());
						}
					}
					totalproduction=production+adjust+inswriteoff;
					totalincome=ptincome+insincome;
					string clinicDesc=listClinics[it].Description;
					row[1]=production.ToString("n");
					row[2]=adjust.ToString("n");
					row[3]=inswriteoff.ToString("n");
					row[4]=totalproduction.ToString("n");
					row[5]=ptincome.ToString("n");
					row[6]=insincome.ToString("n");
					row[7]=totalincome.ToString("n");
					row[8]=clinicDesc=="" ? Lans.g("FormRpProdInc","Unassigned"):clinicDesc;
					dtClinic.Rows.Add(row);  //adds row to table
				}
			}
			for(int i=0;i<dates.Length;i++) {//usually 12 months in loop
				dates[i]=dateFrom.AddMonths(i);//only the month and year are important
				DataRow row=dt.NewRow();
				row[0]=dates[i].ToString("MMM yyyy");//JAN 2014
				production=0;
				adjust=0;
				inswriteoff=0;	//spk 5/19/05
				totalproduction=0;
				ptincome=0;
				insincome=0;
				totalincome=0;
				for(int j=0;j<tableProduction.Rows.Count;j++) {
					if(dates[i].Year==PIn.Date(tableProduction.Rows[j]["ProcDate"].ToString()).Year
						&& dates[i].Month==PIn.Date(tableProduction.Rows[j]["ProcDate"].ToString()).Month) {
						production+=PIn.Decimal(tableProduction.Rows[j]["Production"].ToString());
					}
				}
				for(int j=0;j<tableAdj.Rows.Count;j++) {
					if(dates[i].Year==PIn.Date(tableAdj.Rows[j]["AdjDate"].ToString()).Year
						&& dates[i].Month==PIn.Date(tableAdj.Rows[j]["AdjDate"].ToString()).Month) {
						adjust+=PIn.Decimal(tableAdj.Rows[j]["Adjustment"].ToString());
					}
				}
				for(int j=0;j<tableInsWriteoff.Rows.Count;j++) {
					if(dates[i].Year==PIn.Date(tableInsWriteoff.Rows[j]["ClaimDate"].ToString()).Year
						&& dates[i].Month==PIn.Date(tableInsWriteoff.Rows[j]["ClaimDate"].ToString()).Month) {
						inswriteoff-=PIn.Decimal(tableInsWriteoff.Rows[j]["WriteOff"].ToString());
					}
				}
				for(int j=0;j<tablePay.Rows.Count;j++) {
					if(dates[i].Year==PIn.Date(tablePay.Rows[j]["DatePay"].ToString()).Year
						&& dates[i].Month==PIn.Date(tablePay.Rows[j]["DatePay"].ToString()).Month) {
						ptincome+=PIn.Decimal(tablePay.Rows[j]["Income"].ToString());
					}
				}
				for(int j=0;j<tableIns.Rows.Count;j++) {
					if(dates[i].Year==PIn.Date(tableIns.Rows[j]["CheckDate"].ToString()).Year
						&& dates[i].Month==PIn.Date(tableIns.Rows[j]["CheckDate"].ToString()).Month) {
						insincome+=PIn.Decimal(tableIns.Rows[j]["Ins"].ToString());
					}
				}
				totalproduction=production+adjust+inswriteoff;
				totalincome=ptincome+insincome;
				row[1]=production.ToString("n");
				row[2]=adjust.ToString("n");
				row[3]=inswriteoff.ToString("n");
				row[4]=totalproduction.ToString("n");
				row[5]=ptincome.ToString("n");
				row[6]=insincome.ToString("n");
				row[7]=totalincome.ToString("n");
				dt.Rows.Add(row);
			}
			DataSet ds=null;
			ds=new DataSet("AnnualData");
			ds.Tables.Add(dt);
			if(listClinics.Count!=0) {
				ds.Tables.Add(dtClinic);
			}
			return ds;
		}

		///<summary>Returns a dataset that contains 5 tables used to generate the annual report. If not using clinics then supply an empty list of clinics.
		/// Does not work for Oracle (by chance not by design). Consider enhancing with DbHelper.Year(),DbHelper.Month(), DbHelper.Day() and enhancing the GroupBy Logic.</summary>
		public static DataSet GetAnnualProdIncDataSet(DateTime dateFrom,DateTime dateTo,List<Provider> listProvs,List<Clinic> listClinics,bool writeOffPay,bool hasAllProvs,bool hasAllClinics) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetDS(MethodBase.GetCurrentMethod(),dateFrom,dateTo,listProvs,listClinics,writeOffPay,hasAllProvs,hasAllClinics);
			}
			List<long> listClinicNums=new List<long>();
			for(int i=0;i<listClinics.Count;i++) {
				listClinicNums.Add(listClinics[i].ClinicNum);
			}
			List<long> listProvNums=new List<long>();
			for(int i=0;i<listProvs.Count;i++) {
				listProvNums.Add(listProvs[i].ProvNum);
			}
			//Procedures------------------------------------------------------------------------------
			string whereProv="";
			if(!hasAllProvs && listProvNums.Count>0) {
				whereProv=" AND procedurelog.ProvNum IN ("+String.Join(",",listProvNums)+") ";
			}
			string whereClin="";
			if(!hasAllClinics && listClinicNums.Count>0) {
				whereClin=" AND procedurelog.ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
			//==Travis (04/11/2014): In the case that you have two capitation plans for a single patient the query below will cause a duplicate row, incorectly increasing your production.
			//	We now state in the manual that having two capitation plans is not advised and will cause reporting to be off.
			string command="SELECT "
				+"procedurelog.ProcDate,procedurelog.ClinicNum,"
				+"SUM(procedurelog.ProcFee*(procedurelog.UnitQty+procedurelog.BaseUnits))-IFNULL(SUM(claimproc.WriteOff),0) Production "
				+"FROM procedurelog "
				+"LEFT JOIN claimproc ON procedurelog.ProcNum=claimproc.ProcNum "
				+"AND claimproc.Status='7' "//only CapComplete writeoffs are subtracted here.
				+"WHERE procedurelog.ProcStatus = '2' "
				+whereProv
				+whereClin
				+"AND procedurelog.ProcDate >= " +POut.Date(dateFrom)+" "
				+"AND procedurelog.ProcDate <= " +POut.Date(dateTo)+" "
				+"GROUP BY ClinicNum,YEAR(procedurelog.ProcDate),MONTH(procedurelog.ProcDate)";//Does not work for Oracle. Consider enhancing with DbHelper.Year(),DbHelper.Month()
			command+=" ORDER BY ClinicNum,ProcDate";
			DataTable tableProduction=Db.GetTable(command);
			tableProduction.TableName="tableProduction";
			//Adjustments----------------------------------------------------------------------------
			if(!hasAllProvs && listProvNums.Count>0) {
				whereProv=" AND adjustment.ProvNum IN ("+String.Join(",",listProvNums)+") ";
			}
			if(!hasAllClinics && listClinicNums.Count>0) {
				whereClin=" AND adjustment.ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
			command="SELECT "
				+"adjustment.AdjDate,"
				+"adjustment.ClinicNum,"
				+"SUM(adjustment.AdjAmt) Adjustment "
				+"FROM adjustment "
				+"WHERE adjustment.AdjDate >= "+POut.Date(dateFrom)+" "
				+"AND adjustment.AdjDate <= "+POut.Date(dateTo)+" "
				+whereProv
				+whereClin
				+"GROUP BY ClinicNum,YEAR(adjustment.AdjDate),MONTH(adjustment.AdjDate)";
			command+=" ORDER BY ClinicNum,AdjDate";
			DataTable tableAdj=Db.GetTable(command);
			tableAdj.TableName="tableAdj";
			//TableInsWriteoff--------------------------------------------------------------------------
			if(!hasAllProvs && listProvNums.Count>0) {
				whereProv=" AND claimproc.ProvNum IN ("+String.Join(",",listProvNums)+") ";
			}
			if(!hasAllClinics && listClinicNums.Count>0) {
				whereClin=" AND claimproc.ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
			if(writeOffPay) {
				command="SELECT "
					+"claimproc.DateCP ClaimDate," 
					+"claimproc.ClinicNum,"
					+"SUM(claimproc.WriteOff) WriteOff "
					+"FROM claimproc "
					+"WHERE claimproc.DateCP >= "+POut.Date(dateFrom)+" "
					+"AND claimproc.DateCP <= "+POut.Date(dateTo)+" "
					+whereProv
					+whereClin
					+"AND (claimproc.Status=1 OR claimproc.Status=4) "//Received or supplemental
					+"GROUP BY ClinicNum,YEAR(claimproc.DateCP),MONTH(claimproc.DateCP)";
				command+=" ORDER BY ClinicNum,DateCP";
			}
			else {
				command="SELECT "
					+"claimproc.ProcDate ClaimDate," 
					+"claimproc.ClinicNum,"
					+"SUM(claimproc.WriteOff) WriteOff "
					+"FROM claimproc "
					+"WHERE claimproc.ProcDate >= "+POut.Date(dateFrom)+" "
					+"AND claimproc.ProcDate <= "+POut.Date(dateTo)+" "
					+whereProv
					+whereClin
					+"AND (claimproc.Status=1 OR claimproc.Status=4 OR claimproc.Status=0) " //received or supplemental or notreceived
					+"GROUP BY ClinicNum,YEAR(claimproc.ProcDate), MONTH(claimproc.ProcDate)";
				command+=" ORDER BY ClinicNum,ProcDate";
			}
			DataTable tableInsWriteoff=Db.GetTable(command);
			tableInsWriteoff.TableName="tableInsWriteoff";
			//PtIncome--------------------------------------------------------------------------------
			if(!hasAllProvs && listProvNums.Count>0) {
				whereProv=" AND paysplit.ProvNum IN ("+String.Join(",",listProvNums)+") ";
			}
			if(!hasAllClinics && listClinicNums.Count>0) {
				whereClin=" AND paysplit.ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
			command="SELECT "
				+"paysplit.DatePay,"
				+"paysplit.ClinicNum,"
				+"SUM(paysplit.SplitAmt) Income "
				+"FROM paysplit "
				+"WHERE paysplit.IsDiscount=0 "//AND paysplit.PayNum=payment.PayNum "
				+whereProv
				+whereClin
				+"AND paysplit.DatePay >= "+POut.Date(dateFrom)+" "
				+"AND paysplit.DatePay <= "+POut.Date(dateTo)+" "
				+"GROUP BY ClinicNum,YEAR(paysplit.DatePay),MONTH(paysplit.DatePay)";
			command+=" ORDER BY ClinicNum,DatePay";
			DataTable tablePay=Db.GetTable(command);
			tablePay.TableName="tablePay";
			//InsIncome---------------------------------------------------------------------------------
			if(!hasAllProvs && listProvNums.Count>0) {
				whereProv=" AND claimproc.ProvNum IN ("+String.Join(",",listProvNums)+") ";
			}
			if(!hasAllClinics && listClinicNums.Count>0) {
				whereClin=" AND claimproc.ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
			command="SELECT claimpayment.CheckDate,claimproc.ClinicNum,SUM(claimproc.InsPayamt) Ins "
				+"FROM claimpayment,claimproc WHERE "
				+"claimproc.ClaimPaymentNum = claimpayment.ClaimPaymentNum "
				+"AND claimpayment.CheckDate >= " + POut.Date(dateFrom)+" "
				+"AND claimpayment.CheckDate <= " + POut.Date(dateTo)+" "
				+whereProv
				+whereClin
				+" GROUP BY claimpayment.CheckDate,ClinicNum ORDER BY ClinicNum,CheckDate";
			DataTable tableIns=Db.GetTable(command);
			tableIns.TableName="tableIns";
			DataSet dataSet=new DataSet();
			dataSet.Tables.Add(tableProduction);
			dataSet.Tables.Add(tableAdj);
			dataSet.Tables.Add(tableInsWriteoff);
			dataSet.Tables.Add(tablePay);
			dataSet.Tables.Add(tableIns);
			return dataSet;
		}
		#endregion

		private static int RowComparer(DataRow x,DataRow y) {
			if(_hasClinics) {
				string xClinic=x["Clinic"].ToString();
				string yClinic=y["Clinic"].ToString();
				if(xClinic!=yClinic) {//Sort by clinic first, if no clinic then they'll be the same empty string.
					return String.Compare(xClinic,yClinic);
				}
			}
			DateTime xDate=PIn.Date(x["Date"].ToString());
			DateTime yDate=PIn.Date(y["Date"].ToString());
			if(xDate!=yDate) {//Then by date
				return DateTime.Compare(xDate,yDate);
			}
			string xName=x["Name"].ToString();
			string yName=y["Name"].ToString();
			if(xName!=yName) {//Then by name
				return String.Compare(xName,yName);
			}
			//We might want to include transaction type here but procedures have all different kinds of descriptions.
			string xProvider=x["Provider"].ToString();
			string yProvider=y["Provider"].ToString();
			if(xProvider!=yProvider) {//Then by provider (just for looks).
				return String.Compare(xProvider,yProvider);
			}
			return 0;
		}

	}
}
