using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;

namespace OpenDentBusiness {
	public class RpProcCodes {

		public static DataTable GetData(long feeSchedNum,long clinicNum,long provNum,bool isCategories,bool includeBlanks){
			DataTable data=GetDataSet(feeSchedNum,clinicNum,provNum);
			DataTable retVal=new DataTable("ProcCodes");
			if(isCategories) {
				retVal.Columns.Add(new DataColumn("Category"));
			}
			retVal.Columns.Add(new DataColumn("Code"));
			retVal.Columns.Add(new DataColumn("Desc"));
			retVal.Columns.Add(new DataColumn("Abbr"));
			retVal.Columns.Add(new DataColumn("Fee"));
			List<ProcedureCode> listProcCodes=new List<ProcedureCode>();
			if(isCategories) {
				listProcCodes=new List<ProcedureCode>(ProcedureCodes.GetProcList()); //Ordered by category
			}
			else {
				listProcCodes=ProcedureCodes.GetAllCodes(); //Ordered by ProcCode, used for the non-category version of the report if they want blanks.
			}
			bool isFound;
			for(int i=0;i<listProcCodes.Count;i++){
				isFound=false;
				DataRow row=retVal.NewRow();
				if(isCategories) {
					row[0]=DefC.GetName(DefCat.ProcCodeCats,listProcCodes[i].ProcCat);
					row[1]=listProcCodes[i].ProcCode;
					row[2]=listProcCodes[i].Descript;
					row[3]=listProcCodes[i].AbbrDesc;
				}
				else {
					row[0]=listProcCodes[i].ProcCode;
					row[1]=listProcCodes[i].Descript;
					row[2]=listProcCodes[i].AbbrDesc;
				}
				for(int j=0;j<data.Rows.Count;j++){
					if(data.Rows[j]["ProcCode"].ToString()==listProcCodes[i].ProcCode) {
						isFound=true;
						double amt=PIn.Double(data.Rows[j]["Amount"].ToString());
						if(isCategories) {
							if(amt==-1) {
								row[4]="";
								isFound=false;
							}
							else {
								row[4]=amt.ToString("n");
							}

						}
						else {
							if(amt==-1) {
								row[3]="";
								isFound=false;
							}
							else {
								row[3]=amt.ToString("n");
							}
						}
						break;
					}
				}
				if(includeBlanks && !isFound) {
					retVal.Rows.Add(row); //Including a row that has a blank fee.
				}
				else if(isFound) {
					retVal.Rows.Add(row);
				}
				//All other rows (empty rows where we don't want blanks) are not added to the dataset.
			}
			return retVal;
		}

		private static DataTable GetDataSet(long feeSchedNum,long clinicNum,long provNum) {
			string command="SELECT procedurecode.ProcCode,fee.Amount,procedurecode.Descript,"
				+"procedurecode.AbbrDesc FROM procedurecode,fee "
				+"WHERE procedurecode.CodeNum=fee.CodeNum "
				+"AND fee.FeeSched='"+POut.String(feeSchedNum.ToString())+"' "
				+"AND fee.ClinicNum='"+POut.String(clinicNum.ToString())+"' "
				+"AND fee.ProvNum='"+POut.String(provNum.ToString())+"' "
				+"ORDER BY procedurecode.ProcCode";
			return Db.GetTable(command);
		}

	}
}
