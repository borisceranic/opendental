using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness {
	public class RpProdInc {

		///<summary>If not using clinics then supply an empty list of clinicNums.</summary>
		public static DataSet GetAnnualDataForClinics(DateTime dateFrom,DateTime dateTo,List<long> listProvNums,List<long> listClinicNums,bool writeOffPay,bool hasAllProvs,bool hasAllClinics) {
			DataSet dataSet=GetProdIncAnnualDataSet(dateFrom,dateTo,listProvNums,listClinicNums,writeOffPay,hasAllProvs,hasAllClinics);
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
			DateTime[] dates=new DateTime[dateTo.Month-dateFrom.Month+1];
			//Get a list of clinics so that we have access to their descriptions for the report.
			List<Clinic> listClinics=Clinics.GetClinics(listClinicNums);
			for(int it=0;it<listClinicNums.Count;it++) {
				for(int i=0;i<dates.Length;i++) {//usually 12 months in loop
					dates[i]=dateFrom.AddMonths(i);//only the month and year are important
					DataRow row=dtClinic.NewRow();
					row[0]=dates[i].ToString("MMM yy");//JAN 14
					production=0;
					adjust=0;
					inswriteoff=0;	//spk 5/19/05
					totalproduction=0;
					ptincome=0;
					insincome=0;
					totalincome=0;
					for(int j=0;j<tableProduction.Rows.Count;j++) {
						if(listClinicNums[it]==0 && tableProduction.Rows[j]["ClinicNum"].ToString()!="0") {
							continue;//Only counting unassigned this time around.
						}
						else if(listClinicNums[it]!=0 && tableProduction.Rows[j]["ClinicNum"].ToString()!=POut.Long(listClinicNums[it])) {
							continue;
						}
						if(dates[i].Year==PIn.Date(tableProduction.Rows[j]["ProcDate"].ToString()).Year
								&& dates[i].Month==PIn.Date(tableProduction.Rows[j]["ProcDate"].ToString()).Month) 
						{
							production+=PIn.Decimal(tableProduction.Rows[j]["Production"].ToString());
						}
					}
					for(int j=0;j<tableAdj.Rows.Count;j++) {
						if(listClinicNums[it]==0 && tableAdj.Rows[j]["ClinicNum"].ToString()!="0") {
							continue;
						}
						else if(listClinicNums[it]!=0 && tableAdj.Rows[j]["ClinicNum"].ToString()!=POut.Long(listClinicNums[it])) {
							continue;
						}
						if(dates[i].Year==PIn.Date(tableAdj.Rows[j]["AdjDate"].ToString()).Year
								&& dates[i].Month==PIn.Date(tableAdj.Rows[j]["AdjDate"].ToString()).Month) 
						{
							adjust+=PIn.Decimal(tableAdj.Rows[j]["Adjustment"].ToString());
						}
					}
					for(int j=0;j<tableInsWriteoff.Rows.Count;j++) {
						if(listClinicNums[it]==0 && tableInsWriteoff.Rows[j]["ClinicNum"].ToString()!="0") {
							continue;
						}
						else if(listClinicNums[it]!=0 && tableInsWriteoff.Rows[j]["ClinicNum"].ToString()!=POut.Long(listClinicNums[it])) {
							continue;
						}
						if(dates[i].Year==PIn.Date(tableInsWriteoff.Rows[j]["ClaimDate"].ToString()).Year
								&& dates[i].Month==PIn.Date(tableInsWriteoff.Rows[j]["ClaimDate"].ToString()).Month) 
						{
							inswriteoff-=PIn.Decimal(tableInsWriteoff.Rows[j]["WriteOff"].ToString());
						}
					}
					for(int j=0;j<tablePay.Rows.Count;j++) {
						if(listClinicNums[it]==0 && tablePay.Rows[j]["ClinicNum"].ToString()!="0") {
							continue;
						}
						else if(listClinicNums[it]!=0 && tablePay.Rows[j]["ClinicNum"].ToString()!=POut.Long(listClinicNums[it])) {
							continue;
						}
						if(dates[i].Year==PIn.Date(tablePay.Rows[j]["DatePay"].ToString()).Year
								&& dates[i].Month==PIn.Date(tablePay.Rows[j]["DatePay"].ToString()).Month) 
						{
							ptincome+=PIn.Decimal(tablePay.Rows[j]["Income"].ToString());
						}
					}
					for(int j=0;j<tableIns.Rows.Count;j++) {
						if(listClinicNums[it]==0 && tableIns.Rows[j]["ClinicNum"].ToString()!="0") {
							continue;
						}
						else if(listClinicNums[it]!=0 && tableIns.Rows[j]["ClinicNum"].ToString()!=POut.Long(listClinicNums[it])) {
							continue;
						}
						if(dates[i].Year==PIn.Date(tableIns.Rows[j]["CheckDate"].ToString()).Year
								&& dates[i].Month==PIn.Date(tableIns.Rows[j]["CheckDate"].ToString()).Month) 
						{
							insincome+=PIn.Decimal(tableIns.Rows[j]["Ins"].ToString());
						}
					}
					totalproduction=production+adjust+inswriteoff;
					totalincome=ptincome+insincome;
					string clinicDesc=Clinics.GetDesc(listClinicNums[it]);
					row[1]=production.ToString("n");
					row[2]=adjust.ToString("n");
					row[3]=inswriteoff.ToString("n");
					row[4]=totalproduction.ToString("n");
					row[5]=ptincome.ToString("n");
					row[6]=insincome.ToString("n");
					row[7]=totalincome.ToString("n");
					row[8]=clinicDesc=="" ? Lans.g("FormRpProdInc","Unassigned"):clinicDesc;
					dtClinic.Rows.Add(row);  //adds row to table Q
				}
			}
			for(int i=0;i<dates.Length;i++) {//usually 12 months in loop
				dates[i]=dateFrom.AddMonths(i);//only the month and year are important
				DataRow row=dt.NewRow();
				row[0]=dates[i].ToString("MMM yy");//JAN 14
				production=0;
				adjust=0;
				inswriteoff=0;	//spk 5/19/05
				totalproduction=0;
				ptincome=0;
				insincome=0;
				totalincome=0;
				for(int j=0;j<tableProduction.Rows.Count;j++) {
					if(dates[i].Year==PIn.Date(tableProduction.Rows[j]["ProcDate"].ToString()).Year
						&& dates[i].Month==PIn.Date(tableProduction.Rows[j]["ProcDate"].ToString()).Month) 
					{
						production+=PIn.Decimal(tableProduction.Rows[j]["Production"].ToString());
					}
				}
				for(int j=0;j<tableAdj.Rows.Count;j++) {
					if(dates[i].Year==PIn.Date(tableAdj.Rows[j]["AdjDate"].ToString()).Year
						&& dates[i].Month==PIn.Date(tableAdj.Rows[j]["AdjDate"].ToString()).Month) 
					{
						adjust+=PIn.Decimal(tableAdj.Rows[j]["Adjustment"].ToString());
					}
				}
				for(int j=0;j<tableInsWriteoff.Rows.Count;j++) {
					if(dates[i].Year==PIn.Date(tableInsWriteoff.Rows[j]["ClaimDate"].ToString()).Year
						&& dates[i].Month==PIn.Date(tableInsWriteoff.Rows[j]["ClaimDate"].ToString()).Month) 
					{
						inswriteoff-=PIn.Decimal(tableInsWriteoff.Rows[j]["WriteOff"].ToString());
					}
				}
				for(int j=0;j<tablePay.Rows.Count;j++) {
					if(dates[i].Year==PIn.Date(tablePay.Rows[j]["DatePay"].ToString()).Year
						&& dates[i].Month==PIn.Date(tablePay.Rows[j]["DatePay"].ToString()).Month) 
					{
						ptincome+=PIn.Decimal(tablePay.Rows[j]["Income"].ToString());
					}
				}
				for(int j=0;j<tableIns.Rows.Count;j++) {
					if(dates[i].Year==PIn.Date(tableIns.Rows[j]["CheckDate"].ToString()).Year
						&& dates[i].Month==PIn.Date(tableIns.Rows[j]["CheckDate"].ToString()).Month) 
					{
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
			DataSet ds=new DataSet("AnnualData");
			ds.Tables.Add(dt);
			if(listClinicNums.Count!=0) {
				ds.Tables.Add(dtClinic);
			}
			return ds;
		}

		///<summary>Returns a dataset that contains 5 tables used to generate the annual report. If not using clinics then supply an empty list of clinicNums.</summary>
		public static DataSet GetProdIncAnnualDataSet(DateTime dateFrom,DateTime dateTo,List<long> listProvNums,List<long> listClinicNums,bool writeOffPay,bool hasAllProvs,bool hasAllClinics) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetDS(MethodBase.GetCurrentMethod(),dateFrom,dateTo,listProvNums,listClinicNums,writeOffPay,hasAllProvs,hasAllClinics);
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
				+"GROUP BY MONTH(procedurelog.ProcDate)";
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
				+"GROUP BY MONTH(adjustment.AdjDate)";
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
					+"GROUP BY MONTH(claimproc.DateCP)";
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
					+"AND (claimproc.Status=1 OR claimproc.Status=4 OR claimproc.Status=0) " //received or supplemental or notreceived
					+"GROUP BY MONTH(claimproc.ProcDate)";
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
				+"GROUP BY MONTH(paysplit.DatePay)";
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
				+" GROUP BY claimpayment.CheckDate ORDER BY checkdate";
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

	}
}
