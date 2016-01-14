using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace OpenDentBusiness{

	///<summary>A treatment plan saved by a user.  Does not include the default tp, which is just a list of procedurelog entries with a status of tp.  A treatplan has many proctp's attached to it.</summary>
	[Serializable]
	[CrudTable(IsSecurityStamped=true)]
	public class TreatPlan:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long TreatPlanNum;
		///<summary>FK to patient.PatNum.</summary>
		public long PatNum;
		///<summary>The date of the treatment plan</summary>
		public DateTime DateTP;
		///<summary>The heading that shows at the top of the treatment plan.  Usually 'Proposed Treatment Plan'</summary>
		public string Heading;
		///<summary>A note specific to this treatment plan that shows at the bottom.</summary>
		public string Note;
		///<summary>The encrypted and bound signature in base64 format.  The signature is bound to the concatenation of the tp Note, DateTP, and to each proctp Descript and PatAmt.</summary>
		public string Signature;
		///<summary>True if the signature is in Topaz format rather than OD format.</summary>
		public bool SigIsTopaz;
		///<summary>FK to patient.PatNum. Can be 0.  The patient responsible for approving the treatment.  Public health field not visible to everyone else.</summary>
		public long ResponsParty;
		///<summary>FK to document.DocNum. Can be 0.  If signed, this is the pdf document of the TP at time of signing. See PrefName.TreatPlanSaveSignedToPdf</summary>
		public long DocNum;
		///<summary>Determines the type of treatment plan this is.</summary>
		public TreatPlanStatus TPStatus;
		///<summary>FK to userod.UserNum.  Set to the user logged in when the row was inserted at SecDateEntry date and time.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.ExcludeFromUpdate)]
		public long SecUserNumEntry;
		///<summary>Timestamp automatically generated and user not allowed to change.  The actual date of entry.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateEntry)]
		public DateTime SecDateEntry;
		///<summary>Automatically updated by MySQL every time a row is added or changed. Could be changed due to user editing, custom queries or program
		///updates.  Not user editable with the UI.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TimeStamp)]
		public DateTime SecDateTEdit;

		///<summary>Used to pass the list of ProcTPs in memory with the TreatPlan.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		[XmlIgnore]
		public List<ProcTP> ListProcTPs;

		public TreatPlan() {
			ListProcTPs=new List<ProcTP>();
		}

		///<summary></summary>
		public TreatPlan Copy(){
			TreatPlan newTP=(TreatPlan)MemberwiseClone();
			newTP.ListProcTPs=this.ListProcTPs.Select(x => x.Copy()).ToList();
			return newTP;
		}

	}

	public enum TreatPlanStatus {
		///<summary>0 - Saved treatment plans. Prior to version 15.4.1 all treatment plans were considered archived. Archived TPs are linked to ProcTPs.</summary>
		Saved=0,
		///<summary>1 - Current active TP. There should be only one Active TP per patient. This is a TP linked directly to procedures via the TreatPlanAttach table.</summary>
		Active=1,
		///<summary>2 - Current inactive TP. This is a TP linked directly to procedures via the TreatPlanAttach table.</summary>
		Inactive=2
	}






}




















