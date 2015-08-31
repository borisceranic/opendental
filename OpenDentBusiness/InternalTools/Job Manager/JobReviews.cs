using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class JobReviews{
		
		///<summary></summary>
		public static List<JobReview> GetForReviewer(long userNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<JobReview>>(MethodBase.GetCurrentMethod(),userNum);
			}
			string command="SELECT * FROM jobreview WHERE Reviewer = "+POut.Long(userNum);
			return Crud.JobReviewCrud.SelectMany(command);
		}

		///<summary>Gets one JobReview from the db.</summary>
		public static JobReview GetOne(long jobReviewNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<JobReview>(MethodBase.GetCurrentMethod(),jobReviewNum);
			}
			return Crud.JobReviewCrud.SelectOne(jobReviewNum);
		}

		///<summary></summary>
		public static long Insert(JobReview jobReview){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				jobReview.JobReviewNum=Meth.GetLong(MethodBase.GetCurrentMethod(),jobReview);
				return jobReview.JobReviewNum;
			}
			return Crud.JobReviewCrud.Insert(jobReview);
		}

		///<summary></summary>
		public static void Update(JobReview jobReview){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobReview);
				return;
			}
			Crud.JobReviewCrud.Update(jobReview);
		}

		///<summary></summary>
		public static void Delete(long jobReviewNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobReviewNum);
				return;
			}
			Crud.JobReviewCrud.Delete(jobReviewNum);
		}
		



	}
}