using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	///<summary></summary>
	public class LabCases {

		///<summary>Gets a filtered list of all labcases.</summary>
		public static DataTable Refresh(DateTime aptStartDate,DateTime aptEndDate) {
			string command="SELECT * FROM labcase WHERE DateTimeChecked ORDER BY DateTimeCreated";
			DataTable table=General.GetTable(command);

			/*
			DataTable table=new DataTable("Procedure");
			DataRow row;
			//columns that start with lowercase are altered for display rather than being raw data.
			table.Columns.Add("attached");//0 or 1
			table.Columns.Add("CodeNum");
			table.Columns.Add("descript");
			table.Columns.Add("fee");
			table.Columns.Add("priority");
			table.Columns.Add("ProcCode");
			table.Columns.Add("ProcNum");
			table.Columns.Add("ProcStatus");
			table.Columns.Add("status");
			table.Columns.Add("toothNum");
			table.Columns.Add("Surf");
			string command="SELECT procedurecode.ProcCode,AptNum,PlannedAptNum,Priority,ProcFee,ProcNum,ProcStatus,Surf,ToothNum, "
				+"procedurecode.Descript,procedurelog.CodeNum "
				+"FROM procedurelog LEFT JOIN procedurecode ON procedurelog.CodeNum=procedurecode.CodeNum "
				+"WHERE PatNum="+patNum//sort later
				+" AND (ProcStatus=1 OR ";//tp
			if(apptStatus=="6") {//planned
				command+="PlannedAptNum="+aptNum+")";
			}
			else {
				command+="AptNum="+aptNum+")";
			}
			DataTable rawProc=dcon.GetTable(command);
			for(int i=0;i<rawProc.Rows.Count;i++) {
				row=table.NewRow();
				if(apptStatus=="6") {//planned
					row["attached"]=(rawProc.Rows[i]["PlannedAptNum"].ToString()==aptNum) ? "1" : "0";
				}
				else {
					row["attached"]=(rawProc.Rows[i]["AptNum"].ToString()==aptNum) ? "1" : "0";
				}
				row["CodeNum"]=rawProc.Rows[i]["CodeNum"].ToString();
				row["descript"]=rawProc.Rows[i]["Descript"].ToString();
				row["fee"]=PIn.PDouble(rawProc.Rows[i]["ProcFee"].ToString()).ToString("F");
				row["priority"]=DefB.GetName(DefCat.TxPriorities,PIn.PInt(rawProc.Rows[i]["Priority"].ToString()));
				row["ProcCode"]=rawProc.Rows[i]["ProcCode"].ToString();
				row["ProcNum"]=rawProc.Rows[i]["ProcNum"].ToString();
				row["ProcStatus"]=rawProc.Rows[i]["ProcStatus"].ToString();
				row["status"]=((ProcStat)PIn.PInt(rawProc.Rows[i]["ProcStatus"].ToString())).ToString();
				row["toothNum"]=Tooth.ToInternat(rawProc.Rows[i]["ToothNum"].ToString());
				row["Surf"]=rawProc.Rows[i]["Surf"].ToString();
				table.Rows.Add(row);
			}*/
			return table;
		}

		///<Summary>Used when drawing the appointments for a day.</Summary>
		public static List<LabCase> GetForPeriod(DateTime startDate,DateTime endDate) {
			string command="SELECT labcase.* FROM labcase,appointment "
				+"WHERE labcase.AptNum=appointment.AptNum "
				+"AND (appointment.AptStatus=1 || appointment.AptStatus=2 || appointment.AptStatus=4) "//scheduled,complete,or ASAP
				+"AND AptDateTime >= "+POut.PDate(startDate)
				+"AND AptDateTime < "+POut.PDate(endDate.AddDays(1));//midnight of the next morning.
			return FillFromCommand(command);
		}

		///<Summary>Gets one labcase from database.</Summary>
		public static LabCase GetOne(int labCaseNum){
			string command="SELECT * FROM labcase WHERE LabCaseNum="+POut.PInt(labCaseNum);
			return FillFromCommand(command)[0];
		}

		///<Summary>Gets all labcases for a patient which have not been attached to an appointment.  Usually one or none.  Only used when attaching a labcase from within an appointment.</Summary>
		public static List<LabCase> GetForPat(int patNum,bool isPlanned) {
			string command="SELECT * FROM labcase WHERE PatNum="+POut.PInt(patNum)+" AND ";
			if(isPlanned){
				command+="PlannedAptNum=0";
			}
			else{
				command+="AptNum=0";
			}
			return FillFromCommand(command);
		}

		public static List<LabCase> FillFromCommand(string command){
			DataTable table=General.GetTable(command);
			LabCase lab;
			List<LabCase> retVal=new List<LabCase>();
			for(int i=0;i<table.Rows.Count;i++) {
				lab=new LabCase();
				lab.LabCaseNum     = PIn.PInt   (table.Rows[i][0].ToString());
				lab.PatNum         = PIn.PInt   (table.Rows[i][1].ToString());
				lab.LaboratoryNum  = PIn.PInt   (table.Rows[i][2].ToString());
				lab.AptNum         = PIn.PInt   (table.Rows[i][3].ToString());
				lab.PlannedAptNum  = PIn.PInt   (table.Rows[i][4].ToString());
				lab.DateTimeDue    = PIn.PDateT (table.Rows[i][5].ToString());
				lab.DateTimeCreated= PIn.PDateT (table.Rows[i][6].ToString());
				lab.DateTimeSent   = PIn.PDateT (table.Rows[i][7].ToString());
				lab.DateTimeRecd   = PIn.PDateT (table.Rows[i][8].ToString());
				lab.DateTimeChecked= PIn.PDateT (table.Rows[i][9].ToString());
				lab.ProvNum        = PIn.PInt   (table.Rows[i][10].ToString());
				lab.Instructions   = PIn.PString(table.Rows[i][11].ToString());
				retVal.Add(lab);
			}
			return retVal;
		}

		///<summary></summary>
		public static void Insert(LabCase lab){
			if(PrefB.RandomKeys) {
				lab.LabCaseNum=MiscData.GetKey("labcase","LabCaseNum");
			}
			string command="INSERT INTO labcase (";
			if(PrefB.RandomKeys) {
				command+="LabCaseNum,";
			}
			command+="PatNum,LaboratoryNum,AptNum,PlannedAptNum,DateTimeDue,DateTimeCreated,"
				+"DateTimeSent,DateTimeRecd,DateTimeChecked,ProvNum,Instructions) VALUES(";
			if(PrefB.RandomKeys) {
				command+="'"+POut.PInt(lab.LabCaseNum)+"', ";
			}
			command+=
				 "'"+POut.PInt   (lab.PatNum)+"', "
				+"'"+POut.PInt   (lab.LaboratoryNum)+"', "
				+"'"+POut.PInt   (lab.AptNum)+"', "
				+"'"+POut.PInt   (lab.PlannedAptNum)+"', "
				    +POut.PDateT (lab.DateTimeDue)+", "
				    +POut.PDateT (lab.DateTimeCreated)+", "
				    +POut.PDateT (lab.DateTimeSent)+", "
				    +POut.PDateT (lab.DateTimeRecd)+", "
				    +POut.PDateT (lab.DateTimeChecked)+", "
				+"'"+POut.PInt   (lab.ProvNum)+"', "
				+"'"+POut.PString(lab.Instructions)+"')";
			if(PrefB.RandomKeys) {
				General.NonQ(command);
			}
			else {
				lab.LabCaseNum=General.NonQ(command,true);
			}
		}

		///<summary></summary>
		public static void Update(LabCase lab){
			string command= "UPDATE labcase SET " 
				+ "PatNum = '"          +POut.PInt   (lab.PatNum)+"'"
				+ ",LaboratoryNum = '"  +POut.PInt   (lab.LaboratoryNum)+"'"
				+ ",AptNum = '"         +POut.PInt   (lab.AptNum)+"'"
				+ ",PlannedAptNum = '"  +POut.PInt   (lab.PlannedAptNum)+"'"
				+ ",DateTimeDue = "     +POut.PDateT (lab.DateTimeDue)
				+ ",DateTimeCreated = " +POut.PDateT (lab.DateTimeCreated)
				+ ",DateTimeSent = "    +POut.PDateT (lab.DateTimeSent)
				+ ",DateTimeRecd = "    +POut.PDateT (lab.DateTimeRecd)
				+ ",DateTimeChecked = " +POut.PDateT (lab.DateTimeChecked)
				+ ",ProvNum = '"        +POut.PInt   (lab.ProvNum)+"'"
				+ ",Instructions = '"   +POut.PString(lab.Instructions)+"'"
				+" WHERE LabCaseNum = '"+POut.PInt(lab.LabCaseNum)+"'";
 			General.NonQ(command);
		}


		///<summary>Checks dependencies first.  Throws exception if can't delete.</summary>
		public static void Delete(int labCaseNum){
			string command;
			/*
			//check patients for dependencies
			string command="SELECT LName,FName FROM patient WHERE LabCaseNum ="
				+POut.PInt(LabCase.LabCaseNum);
			DataTable table=General.GetTable(command);
			if(table.Rows.Count>0){
				string pats="";
				for(int i=0;i<table.Rows.Count;i++){
					pats+="\r";
					pats+=table.Rows[i][0].ToString()+", "+table.Rows[i][1].ToString();
				}
				throw new Exception(Lan.g("LabCases","Cannot delete LabCase because ")+pats);
			}*/
			//delete
			command= "DELETE FROM labcase WHERE LabCaseNum = "+POut.PInt(labCaseNum);
 			General.NonQ(command);
		}

		///<summary>Attaches a labcase to an appointment.</summary>
		public static void AttachToAppt(int labCaseNum,int aptNum){
			string command="UPDATE labcase SET AptNum="+POut.PInt(aptNum)+" WHERE LabCaseNum="+POut.PInt(labCaseNum);
			General.NonQ(command);
		}

		///<summary>Attaches a labcase to a planned appointment.</summary>
		public static void AttachToPlannedAppt(int labCaseNum,int plannedAptNum) {
			string command="UPDATE labcase SET PlannedAptNum="+POut.PInt(plannedAptNum)+" WHERE LabCaseNum="+POut.PInt(labCaseNum);
			General.NonQ(command);
		}

		///<Summary>Frequently returns null.</Summary>
		public static LabCase GetOneFromList(List<LabCase> labCaseList,int aptNum){
			for(int i=0;i<labCaseList.Count;i++){
				if(labCaseList[i].AptNum==aptNum){
					return labCaseList[i];
				}
			}
			return null;
		}

	}
	


}













