using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace OpenDentBusiness {
	///<summary></summary>
	public class Diseases {
		public static Disease GetSpecificDiseaseForPatient(long patNum,long diseaseDefNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Disease>(MethodBase.GetCurrentMethod(),patNum,diseaseDefNum);
			}
			string command="SELECT * FROM disease WHERE PatNum="+POut.Long(patNum)
				+" AND DiseaseDefNum="+POut.Long(diseaseDefNum);
			return Crud.DiseaseCrud.SelectOne(command);
		}

		///<summary>Gets a list of all Diseases for a given patient.  Includes hidden. Sorted by diseasedef.ItemOrder.</summary>
		public static List<Disease> Refresh(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Disease>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT disease.* FROM disease,diseasedef "
				+"WHERE disease.DiseaseDefNum=diseasedef.DiseaseDefNum "
				+"AND PatNum="+POut.Long(patNum)
				+" ORDER BY diseasedef.ItemOrder";
			return Crud.DiseaseCrud.SelectMany(command);
		}

		///<summary></summary>
		public static void Update(Disease disease) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),disease);
				return;
			}
			Crud.DiseaseCrud.Update(disease);
		}

		///<summary></summary>
		public static long Insert(Disease disease) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				disease.DiseaseNum=Meth.GetLong(MethodBase.GetCurrentMethod(),disease);
				return disease.DiseaseNum;
			}
			return Crud.DiseaseCrud.Insert(disease);
		}

		///<summary></summary>
		public static void Delete(Disease disease) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),disease);
				return;
			}
			string command="DELETE FROM disease WHERE DiseaseNum ="+POut.Long(disease.DiseaseNum);
			Db.NonQ(command);
		}

		///<summary>Deletes all diseases for one patient.</summary>
		public static void DeleteAllForPt(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),patNum);
				return;
			}
			string command="DELETE FROM disease WHERE PatNum ="+POut.Long(patNum);
			Db.NonQ(command);
		}

		public static List<long> GetChangedSinceDiseaseNums(DateTime changedSince) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),changedSince);
			}
			string command="SELECT DiseaseNum FROM disease WHERE DateTStamp > "+POut.DateT(changedSince);
			DataTable dt=Db.GetTable(command);
			List<long> diseasenums = new List<long>(dt.Rows.Count);
			for(int i=0;i<dt.Rows.Count;i++) {
				diseasenums.Add(PIn.Long(dt.Rows[i]["DiseaseNum"].ToString()));
			}
			return diseasenums;
		}

		///<summary>Used along with GetChangedSinceDiseaseNums</summary>
		public static List<Disease> GetMultDiseases(List<long> diseaseNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Disease>>(MethodBase.GetCurrentMethod(),diseaseNums);
			}
			string strDiseaseNums="";
			DataTable table;
			if(diseaseNums.Count>0) {
				for(int i=0;i<diseaseNums.Count;i++) {
					if(i>0) {
						strDiseaseNums+="OR ";
					}
					strDiseaseNums+="DiseaseNum='"+diseaseNums[i].ToString()+"' ";
				}
				string command="SELECT * FROM disease WHERE "+strDiseaseNums;
				table=Db.GetTable(command);
			}
			else {
				table=new DataTable();
			}
			Disease[] multDiseases=Crud.DiseaseCrud.TableToList(table).ToArray();
			List<Disease> diseaseList=new List<Disease>(multDiseases);
			return diseaseList;
		}
		
		
		
		
	}

		



		
	

	

	


}










