using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class JobNotes{

		public static List<JobNote> GetForJob(long jobNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<JobNote>>(MethodBase.GetCurrentMethod(),jobNum);
			}
			string command="SELECT * FROM jobnote WHERE JobNum = "+POut.Long(jobNum)+" ORDER BY DateTimeNote";
			return Crud.JobNoteCrud.SelectMany(command);
		}

		///<summary>Gets one JobNote from the db.</summary>
		public static JobNote GetOne(long jobNoteNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<JobNote>(MethodBase.GetCurrentMethod(),jobNoteNum);
			}
			return Crud.JobNoteCrud.SelectOne(jobNoteNum);
		}

		///<summary></summary>
		public static long Insert(JobNote jobNote){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				jobNote.JobNoteNum=Meth.GetLong(MethodBase.GetCurrentMethod(),jobNote);
				return jobNote.JobNoteNum;
			}
			return Crud.JobNoteCrud.Insert(jobNote);
		}

		///<summary></summary>
		public static void Update(JobNote jobNote){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobNote);
				return;
			}
			Crud.JobNoteCrud.Update(jobNote);
		}

		///<summary></summary>
		public static void Delete(long jobNoteNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobNoteNum);
				return;
			}
			Crud.JobNoteCrud.Delete(jobNoteNum);
		}
		



	}
}