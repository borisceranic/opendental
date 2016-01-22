using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDental.ReportingComplex;
using OpenDentBusiness;

namespace OpenDental {
	///<summary></summary>
	public partial class FormRpBrokenAppointments:Form {

		private List<Clinic> _listClinics;
		private List<Def> _listPosAdjTypes;

		///<summary></summary>
		public FormRpBrokenAppointments() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormRpBrokenAppointments_Load(object sender,EventArgs e) {
			dateStart.SelectionStart=DateTime.Today;
			dateEnd.SelectionStart=DateTime.Today;
			List<Provider> listShort=ProviderC.GetListShort();
			for(int i=0;i<listShort.Count;i++) {
				listProvs.Items.Add(listShort[i].GetLongDesc());
			}
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				listClinics.Visible=false;
				labelClinics.Visible=false;
				checkAllClinics.Visible=false;
			}
			else {
				_listClinics=Clinics.GetForUserod(Security.CurUser);
				if(!Security.CurUser.ClinicIsRestricted) {
					listClinics.Items.Add(Lan.g(this,"Unassigned"));
					listClinics.SetSelected(0,true);
				}
				for(int i=0;i<_listClinics.Count;i++) {
					int curIndex=listClinics.Items.Add(_listClinics[i].Description);
					if(FormOpenDental.ClinicNum==0) {
						listClinics.SetSelected(curIndex,true);
						checkAllClinics.Checked=true;
					}
					if(_listClinics[i].ClinicNum==FormOpenDental.ClinicNum) {
						listClinics.SelectedIndices.Clear();
						listClinics.SetSelected(curIndex,true);
					}
				}
			}
			_listPosAdjTypes=DefC.GetPositiveAdjTypes();
			long brokenApptAdjDefNum=PrefC.GetLong(PrefName.BrokenAppointmentAdjustmentType);
			for(int i=0; i<_listPosAdjTypes.Count;i++) {
				listAdj.Items.Add(_listPosAdjTypes[i].ItemName);
				if(_listPosAdjTypes[i].DefNum==brokenApptAdjDefNum) {
					listAdj.SelectedIndices.Add(i);
				}
			}
			if(PrefC.GetBool(PrefName.BrokenApptProcedure)) {
				radioProcs.Checked=true;
			}
			else if(PrefC.GetBool(PrefName.BrokenApptAdjustment)) {
				radioAdj.Checked=true;
			}
			else {
				radioAptStatus.Checked=true;
			}
		}

		private void checkAllProvs_Click(object sender,EventArgs e) {
			if(checkAllProvs.Checked) {
				listProvs.SelectedIndices.Clear();
			}
		}

		private void checkAllClinics_Click(object sender,EventArgs e) {
			if(checkAllClinics.Checked) {
				for(int i=0;i<listClinics.Items.Count;i++) {
					listClinics.SetSelected(i,true);
				}
			}
			else {
				listClinics.SelectedIndices.Clear();
			}
		}

		private void listProvs_Click(object sender,EventArgs e) {
			if(listProvs.SelectedIndices.Count>0) {
				checkAllProvs.Checked=false;
			}
		}

		private void listClinics_Click(object sender,EventArgs e) {
			if(listClinics.SelectedIndices.Count>0) {
				checkAllClinics.Checked=false;
			}
		}

		private void radioProcs_CheckedChanged(object sender,EventArgs e) {
			if(radioProcs.Checked) {
				labelDescr.Text=Lan.g(this,"Broken appointments based on ADA code D9986");
			}
		}

		private void radioAdj_CheckedChanged(object sender,EventArgs e) {
			if(radioAdj.Checked) {
				labelDescr.Text=Lan.g(this,"Broken appointments based on broken appointment adjustments");
				listAdj.Visible=true;
			}
			else {
				listAdj.Visible=false;
			}
		}

		private void radioAptStatus_CheckedChanged(object sender,EventArgs e) {
			if(radioAptStatus.Checked) {
				labelDescr.Text=Lan.g(this,"Broken appointments based on appointment status");
			}
		}

		private string BuildProvFilter() {
			string whereProv="";
			if(checkAllProvs.Checked) {
				return whereProv;
			}
			if(radioProcs.Checked) {//Report looking at ADA procedure code D9986
				whereProv=" AND procedurelog.ProvNum IN (";
				for(int i=0;i<listProvs.SelectedIndices.Count;i++) {
					if(i>0) {
						whereProv+=",";
					}
					whereProv+="'"+POut.Long(ProviderC.ListShort[listProvs.SelectedIndices[i]].ProvNum)+"'";
				}
				whereProv+=") ";
			}
			else if(radioAdj.Checked) {//Report looking at broken appointment adjustments
				whereProv=" AND adjustment.ProvNum IN (";
				for(int i=0;i<listProvs.SelectedIndices.Count;i++) {
					if(i>0) {
						whereProv+=",";
					}
					whereProv+="'"+POut.Long(ProviderC.ListShort[listProvs.SelectedIndices[i]].ProvNum)+"'";
				}
				whereProv+=") ";
			}
			else {//Report looking at appointments with a status of 'Broken'
				whereProv=" AND (appointment.ProvNum IN (";
				for(int i=0;i<listProvs.SelectedIndices.Count;i++) {
					if(i>0) {
						whereProv+=",";
					}
					whereProv+="'"+POut.Long(ProviderC.ListShort[listProvs.SelectedIndices[i]].ProvNum)+"'";
				}
				whereProv+=") ";
				whereProv+="OR appointment.ProvHyg IN (";
				for(int i=0;i<listProvs.SelectedIndices.Count;i++) {
					if(i>0) {
						whereProv+=",";
					}
					whereProv+="'"+POut.Long(ProviderC.ListShort[listProvs.SelectedIndices[i]].ProvNum)+"'";
				}
				whereProv+=")) ";
			}
			return whereProv;
		}

		private string BuildClinicFilter() {
			string whereClin="";
			if(!PrefC.HasClinicsEnabled) {
				return whereClin;
			}		
			if(radioProcs.Checked) {//Report looking at ADA procedure code D9986
				whereClin+=" AND procedurelog.ClinicNum IN(";
			}
			else if(radioAdj.Checked) {//Report looking at broken appointment adjustments
				whereClin+=" AND adjustment.ClinicNum IN(";
			}
			else {//Report looking at appointments with a status of 'Broken'
				whereClin+=" AND appointment.ClinicNum IN(";
			}
			for(int i=0;i<listClinics.SelectedIndices.Count;i++) {
				if(i>0) {
					whereClin+=",";
				}
				if(Security.CurUser.ClinicIsRestricted) {
					whereClin+=POut.Long(_listClinics[listClinics.SelectedIndices[i]].ClinicNum);//we know that the list is a 1:1 to _listClinics
				}
				else {//Unassigned or 'Headquarters'
					if(listClinics.SelectedIndices[i]==0) {
						whereClin+="0";
					}
					else {
						whereClin+=POut.Long(_listClinics[listClinics.SelectedIndices[i]-1].ClinicNum);//Minus 1 from the selected index
					}
				}
			}
			whereClin+=") ";
			return whereClin;
		}

		private string ByProceduresQuery() {
			string queryBrokenApts=
				"SELECT procedurelog.ProcDate ProcDate,provider.Abbr Provider,"
				+DbHelper.Concat("patient.LName","', '","patient.FName")+" Patient, "
				+"procedurelog.ProcFee ProcFee ";
			if(PrefC.HasClinicsEnabled) {
				queryBrokenApts+=",COALESCE(clinic.Description,'"+POut.String(Lan.g(this,"Unassigned"))+"') ClinicDesc ";
			}
			queryBrokenApts+=
				"FROM procedurelog "
				+"INNER JOIN procedurecode ON procedurecode.CodeNum=procedurelog.CodeNum AND procedurecode.ProcCode='D9986' "
				+"INNER JOIN patient ON patient.PatNum=procedurelog.PatNum "
				+"INNER JOIN provider ON provider.ProvNum=procedurelog.ProvNum "
				+BuildProvFilter();
			if(PrefC.HasClinicsEnabled) {
				queryBrokenApts+="LEFT JOIN clinic ON clinic.ClinicNum=procedurelog.ClinicNum ";
			}
			queryBrokenApts+="WHERE procedurelog.ProcDate BETWEEN "+POut.Date(dateStart.SelectionStart)+" AND "+POut.Date(dateEnd.SelectionStart)+" "
				+"AND procedurelog.ProcStatus="+POut.Int((int)ProcStat.C)+" ";
			if(PrefC.HasClinicsEnabled) {
				queryBrokenApts+=BuildClinicFilter()+" "
					+"ORDER BY clinic.Description,procedurelog.ProcDate,patient.LName,patient.FName";
			}
			else {
				queryBrokenApts+="ORDER BY procedurelog.ProcDate,patient.LName,patient.FName";
			}
			return queryBrokenApts;
		}

		private string ByAdjustmentsQuery() {
			List<long> listAdjDefNums=new List<long>();
			for(int i=0;i<listAdj.SelectedIndices.Count;i++) {
				listAdjDefNums.Add(_listPosAdjTypes[listAdj.SelectedIndices[i]].DefNum);
			}
			string queryBrokenApts=
				"SELECT adjustment.AdjDate AdjDate,provider.Abbr Provider,"+DbHelper.Concat("patient.LName","', '","patient.FName")+" Patient,"
				+"adjustment.AdjAmt AdjAmt,adjustment.AdjNote AdjNote";
			if(PrefC.HasClinicsEnabled) {
				queryBrokenApts+=",COALESCE(clinic.Description,'"+POut.String(Lan.g(this,"Unassigned"))+"') ClinicDesc ";
			}
			queryBrokenApts+=
				"FROM adjustment "
				+"INNER JOIN patient ON patient.PatNum=adjustment.PatNum "
				+"INNER JOIN provider ON provider.ProvNum=adjustment.ProvNum "
				+BuildProvFilter();
			if(PrefC.HasClinicsEnabled) {
				queryBrokenApts+="LEFT JOIN clinic ON clinic.ClinicNum=adjustment.ClinicNum ";
			}
			queryBrokenApts+="WHERE adjustment.AdjDate BETWEEN "+POut.Date(dateStart.SelectionStart)+" AND "+POut.Date(dateEnd.SelectionStart)+" "
				+"AND adjustment.AdjType IN("+string.Join(",",listAdjDefNums)+") ";
			if(PrefC.HasClinicsEnabled) {
				queryBrokenApts+=BuildClinicFilter()+" "
					+"ORDER BY clinic.Description,adjustment.AdjDate,patient.LName,patient.FName";
			}
			else {
				queryBrokenApts+="ORDER BY adjustment.AdjDate,patient.LName,patient.FName";
			}
			return queryBrokenApts;
		}

		private string ByApptStatusQuery() {
			string queryBrokenApts=
				"SELECT "+DbHelper.DateTFormatColumn("appointment.AptDateTime","%m/%d/%Y %H:%i:%s")+" AptDateTime, "
				+""+DbHelper.Concat("patient.LName","', '","patient.FName")+" Patient,doctor.Abbr Doctor,hygienist.Abbr Hygienist, "
				+"appointment.IsHygiene IsHygieneApt ";
			if(PrefC.HasClinicsEnabled) {
				queryBrokenApts+=",COALESCE(clinic.Description,'"+POut.String(Lan.g(this,"Unassigned"))+"') ClinicDesc ";//Coalesce is Oracle compatible
			}
			queryBrokenApts+=
				"FROM appointment "
				+"INNER JOIN patient ON appointment.PatNum=patient.PatNum "
				+"LEFT JOIN provider doctor ON doctor.ProvNum=appointment.ProvNum "
				+"LEFT JOIN provider hygienist ON hygienist.ProvNum=appointment.ProvHyg ";
			if(PrefC.HasClinicsEnabled) {
				queryBrokenApts+="LEFT JOIN clinic ON clinic.ClinicNum=appointment.ClinicNum ";
			}
			queryBrokenApts+=
				"WHERE "+DbHelper.DtimeToDate("appointment.AptDateTime")+" BETWEEN "+POut.Date(dateStart.SelectionStart)
				+" AND "+POut.Date(dateEnd.SelectionStart)+" "
				+"AND appointment.AptStatus="+POut.Int((int)ApptStatus.Broken)+" "
				+BuildProvFilter();
			if(PrefC.HasClinicsEnabled) {
				queryBrokenApts+=BuildClinicFilter()+" "
					+"ORDER BY clinic.Description,appointment.AptDateTime,patient.LName,patient.FName";
			}
			else {
				queryBrokenApts+="ORDER BY appointment.AptDateTime,patient.LName,patient.FName ";
			}
			return queryBrokenApts;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!checkAllProvs.Checked && listProvs.SelectedIndices.Count==0) {
				MsgBox.Show(this,"At least one provider must be selected.");
				return;
			}
			if(PrefC.HasClinicsEnabled) {
				if(!checkAllClinics.Checked && listClinics.SelectedIndices.Count==0) {
					MsgBox.Show(this,"At least one clinic must be selected.");
					return;
				}
			}
			if(radioAdj.Checked && listAdj.SelectedIndices.Count==0) {
				MsgBox.Show(this,"At least one adjustment type must be selected.");
				return;
			}
			string whereProv=BuildProvFilter();
			string whereClin=BuildClinicFilter();
			string queryBrokenApts="";
			if(radioProcs.Checked) {//Report looking at ADA procedure code D9986
				queryBrokenApts=ByProceduresQuery();
			}
			else if(radioAdj.Checked) {//Report looking at broken appointment adjustments
				queryBrokenApts=ByAdjustmentsQuery();
			}
			else {//Report looking at appointments with a status of 'Broken'
				queryBrokenApts=ByApptStatusQuery();
			}
			string subtitleProvs="";
			string subtitleClinics="";
			if(checkAllProvs.Checked) {
				subtitleProvs=Lan.g(this,"All Providers");
			}
			else {
				for(int i=0;i<listProvs.SelectedIndices.Count;i++) {
					if(i>0) {
						subtitleProvs+=", ";
					}
					subtitleProvs+=ProviderC.ListShort[listProvs.SelectedIndices[i]].Abbr;
				}
			}
			if(PrefC.HasClinicsEnabled) {
				if(checkAllClinics.Checked) {
					subtitleClinics=Lan.g(this,"All Clinics");
				}
				else {
					for(int i=0;i<listClinics.SelectedIndices.Count;i++) {
						if(i>0) {
							subtitleClinics+=", ";
						}
						if(Security.CurUser.ClinicIsRestricted) {
							subtitleClinics+=_listClinics[listClinics.SelectedIndices[i]].Description;
						}
						else {
							if(listClinics.SelectedIndices[i]==0) {
								subtitleClinics+=Lan.g(this,"Unassigned");
							}
							else {
								subtitleClinics+=_listClinics[listClinics.SelectedIndices[i]-1].Description;//Minus 1 from the selected index
							}
						}
					}
				}
			}
			Font font=new Font("Tahoma",10);
			Font fontBold=new Font("Tahoma",10,FontStyle.Bold);
			Font fontTitle=new Font("Tahoma",17,FontStyle.Bold);
			Font fontSubTitle=new Font("Tahoma",11,FontStyle.Bold);
			ReportComplex report=new ReportComplex(true,false);
			report.ReportName=Lan.g(this,"Broken Appointments");
			report.AddTitle("Title",Lan.g(this,"Broken Appointments"),fontTitle);
			if(radioProcs.Checked) {//Report looking at ADA procedure code D9986
				report.AddSubTitle("Report Description",Lan.g(this,"By ADA Code D9986"),fontSubTitle);
			}
			else if(radioAdj.Checked) {//Report looking at broken appointment adjustments
				report.AddSubTitle("Report Description",Lan.g(this,"By Broken Appointment Adjustment"),fontSubTitle);
			}
			else {//Report looking at appointments with a status of 'Broken'
				report.AddSubTitle("Report Description",Lan.g(this,"By Appointment Status"),fontSubTitle);
			}
			report.AddSubTitle("Providers",subtitleProvs,fontSubTitle);
			report.AddSubTitle("Clinics",subtitleClinics,fontSubTitle);
			QueryObject query;
			if(PrefC.HasClinicsEnabled) {//Split the query up by clinics.
				query=report.AddQuery(queryBrokenApts,Lan.g(this,"Broken Appointments"),"ClinicDesc",SplitByKind.Value,0,true);
			}
			else {
				query=report.AddQuery(queryBrokenApts,Lan.g(this,"Broken Appointments"),"",SplitByKind.None,0,true);
			}
			//Add columns to report
			if(radioProcs.Checked) {//Report looking at ADA procedure code D9986
				query.AddColumn(Lan.g(this,"Date"),85,FieldValueType.Date,font);
				query.AddColumn(Lan.g(this,"Provider"),180,FieldValueType.String,font);
				query.AddColumn(Lan.g(this,"Patient"),220,FieldValueType.String,font);
				query.AddColumn(Lan.g(this,"Fee"),200,FieldValueType.Number,font);
				query.AddGroupSummaryField(Lan.g(this,"Total Broken Appointment Fees")+":",Lan.g(this,"Fee"),"ProcFee",SummaryOperation.Sum,fontBold,0,10);
				query.AddGroupSummaryField(Lan.g(this,"Total Broken Appointments")+":",Lan.g(this,"Fee"),"ProcFee",SummaryOperation.Count,fontBold,0,10);
			}
			else if(radioAdj.Checked) {//Report looking at broken appointment adjustments
				query.AddColumn(Lan.g(this,"Date"),85,FieldValueType.Date,font);
				query.AddColumn(Lan.g(this,"Provider"),100,FieldValueType.String,font);
				query.AddColumn(Lan.g(this,"Patient"),220,FieldValueType.String,font);
				query.AddColumn(Lan.g(this,"Amount"),80,FieldValueType.Number,font);
				query.AddColumn(Lan.g(this,"Note"),300,FieldValueType.String,font);
				query.AddGroupSummaryField(Lan.g(this,"Total Broken Appointment Adjustment Amount")+":",
					Lan.g(this,"Amount"),"AdjAmt",SummaryOperation.Sum,fontBold,0,10);
				query.AddGroupSummaryField(Lan.g(this,"Total Broken Appointments")+":",
					Lan.g(this,"Amount"),"AdjAmt",SummaryOperation.Count,fontBold,0,10);
			}
			else {//Report looking at appointments with a status of 'Broken'
				query.AddColumn(Lan.g(this,"AptDate"),85,FieldValueType.Date,font);
				query.AddColumn(Lan.g(this,"Patient"),220,FieldValueType.String,font);
				query.AddColumn(Lan.g(this,"Doctor"),165,FieldValueType.String,font);
				query.AddColumn(Lan.g(this,"Hygienist"),165,FieldValueType.String,font);
				query.AddColumn(Lan.g(this,"IsHyg"),50,FieldValueType.Boolean,font);
				query.GetColumnDetail(Lan.g(this,"IsHyg")).ContentAlignment = ContentAlignment.MiddleCenter;
				query.AddGroupSummaryField(Lan.g(this,"Total Broken Appointments")+":",Lan.g(this,"IsHyg"),"AptDateTime",SummaryOperation.Count,fontBold,0,10);
			}
			query.ContentAlignment=ContentAlignment.MiddleRight;
			report.AddPageNum(font);
			//execute query
			if(!report.SubmitQueries()) {
				return;
			}
			//display report
			FormReportComplex FormR=new FormReportComplex(report);
			//FormR.MyReport=report;
			FormR.ShowDialog();
			DialogResult=DialogResult.OK;
		}

	}
}