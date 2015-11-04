using System;
using System.Collections;

namespace OpenDentBusiness{

	///<summary>A patient can only have one recall object per type.  The recall table stores a few dates that must be kept synchronized with other information in the database.  This is difficult.  Anytime one of the following items changes, things need to be synchronized: procedurecode.SetRecall, any procedurelog change for a patient (procs added, deleted, completed, status changed, date changed, etc), patient status changed.  There are expected to be a few bugs in the synchronization logic, so anytime a patient's recall is opened, it will also update.
	///
	///During synchronization, the program will frequently alter DateDueCalc, DateDue, and DatePrevious based on trigger procs.  The system will also add and delete recalls as necessary. But it will not delete a recall unless all values are default and there is no useful information.  When a user tries to delete a recall, they will only be successful if the trigger conditions do not apply.  Otherwise, they will have to disable the recall instead.</summary>
	[Serializable]
	public class Recall:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long RecallNum;
		///<summary>FK to patient.PatNum.</summary>
		public long PatNum;
		///<summary>Not editable.  The calculated date due. Generated by the program and subject to change anytime the conditions change. It can be blank (0001-01-01) if no appropriate triggers. </summary>
		public DateTime DateDueCalc;
		///<summary>This is the date that is actually used when doing reports for recall. It will usually be the same as DateDueCalc unless user has changed it. System will only update this field if it is the same as DateDueCalc.  Otherwise, it will be left alone.  Gets cleared along with DateDueCalc when resetting recall.  When setting disabled, this field will also be cleared.  This is the field to use if converting from another software.</summary>
		public DateTime DateDue;
		///<summary>Not editable. Previous date that procedures were done to trigger this recall. It is calculated and enforced automatically.  If you want to affect this date, add a procedure to the chart with a status of C, EC, or EO.</summary>
		public DateTime DatePrevious;
		///<summary>The interval between recalls.  The Interval struct combines years, months, weeks, and days into a single integer value.</summary>
		public Interval RecallInterval;
		///<summary>FK to definition.DefNum, or 0 for none.</summary>
		public long RecallStatus;
		///<summary>An administrative note for staff use.</summary>
		public string Note;
		///<summary>If true, this recall type will be disabled (there's only one type right now). This is usually used rather than deleting the recall type from the patient because the program must enforce the trigger conditions for all patients.</summary>
		public bool IsDisabled;
		///<summary>Last datetime that this row was inserted or updated.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TimeStamp)]
		public DateTime DateTStamp;
		///<summary>FK to recalltype.RecallTypeNum.</summary>
		public long RecallTypeNum;
		///<summary>Default is 0.  If a positive number is entered, then the family balance must be less in order for this recall to show in the recall list.</summary>
		public double DisableUntilBalance;
		///<summary>If a date is entered, then this recall will be disabled until that date.</summary>
		public DateTime DisableUntilDate;
		/// <summary>This will only have a value if a recall is scheduled.</summary>
		public DateTime DateScheduled;

		///<summary>Returns a copy of this Recall.</summary>
		public Recall Copy(){
			return (Recall)this.MemberwiseClone();
		}

	}

	///<summary></summary>
	public enum WebSchedAutomaticSend {
		///<summary>0 - Do not send Web Sched notifications automatically.</summary>
		DoNotSend,
		///<summary>1 - Send to all patients with email address</summary>
		SendToEmail,
		///<summary>2 - Send to patients with email address and no other preferred recall method is selected.</summary>
		SendToEmailNoPreferred,
		///<summary>3 - Send to patients with email address and email is selected as their preferred recall method.</summary>
		SendToEmailOnlyPreferred
	}

}









