using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class JobQuotes{

		///<summary></summary>
		public static List<JobQuote> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<JobQuote>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM jobquote WHERE PatNum = "+POut.Long(patNum);
			return Crud.JobQuoteCrud.SelectMany(command);
		}

		///<summary>Gets one JobQuote from the db.</summary>
		public static JobQuote GetOne(long jobQuoteNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<JobQuote>(MethodBase.GetCurrentMethod(),jobQuoteNum);
			}
			return Crud.JobQuoteCrud.SelectOne(jobQuoteNum);
		}

		///<summary></summary>
		public static long Insert(JobQuote jobQuote){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				jobQuote.JobQuoteNum=Meth.GetLong(MethodBase.GetCurrentMethod(),jobQuote);
				return jobQuote.JobQuoteNum;
			}
			return Crud.JobQuoteCrud.Insert(jobQuote);
		}

		///<summary></summary>
		public static void Update(JobQuote jobQuote){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobQuote);
				return;
			}
			Crud.JobQuoteCrud.Update(jobQuote);
		}

		///<summary></summary>
		public static void Delete(long jobQuoteNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobQuoteNum);
				return;
			}
			Crud.JobQuoteCrud.Delete(jobQuoteNum);
		}



	}
}

