using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using OpenDentBusiness;
using OpenDentBusiness.UI;

namespace OpenDental{
	public class ApptViewItemL{
		///<summary>A list of the ApptViewItems for the current view.</summary>
		public static List<ApptViewItem> ForCurView;
		///<summary>Subset of ForCurView. Just items for rowElements, including apptfielddefs. If no view is selected, then the elements are filled with default info.</summary>
		public static List<ApptViewItem> ApptRows;
		public static ApptView ApptViewCur;

		//Deprecated function, we no longer store the index of the veiw in the computer pref table
		//public static void GetForCurView(int indexInList,bool isWeekly,List<Schedule> dailySched) {
		//	if(indexInList<0){//might be -1 or -2
		//		GetForCurView(null,isWeekly,dailySched);
		//	}
		//	else{
		//		GetForCurView(ApptViewC.List[indexInList],isWeekly,dailySched);
		//	}
		//}

		///<summary>Gets the 'visible' operatories for todays date with the currently selected appointment view.  This is strictly used when filtering the waiting room for clinics.  Pass in null for the dailySched if this is a weekly view or if in FormApptViewEdit.</summary>
		public static List<Operatory> GetOpsForApptView(ApptView apptViewCur,bool isWeekly,List<Schedule> dailySched) {
			List<ApptViewItem> forCurView;
			List<Provider> visProvs;
			List<Operatory> visOps;
			List<ApptViewItem> apptRows;
			int rowsPerIncr;
			FillForApptView(isWeekly,apptViewCur,out visProvs,out visOps,out forCurView,out apptRows,out rowsPerIncr);
			AddOpsForScheduledProvs(isWeekly,dailySched,apptViewCur,ref visOps);
			visOps.Sort(CompareOps);
			visProvs.Sort(CompareProvs);
			return visOps;
		}

		///<summary>Gets (list)ForCurView, ApptDrawing.VisOps, ApptDrawing.VisProvs, and ApptRows.  Also sets TwoRows.  Pass in null for the dailySched if this is a weekly view or if in FormApptViewEdit.</summary>
		public static void GetForCurView(ApptView apptViewCur,bool isWeekly,List<Schedule> dailySched) {
			ApptViewCur=apptViewCur;
			FillForApptView(isWeekly,ApptViewCur,out ApptDrawing.VisProvs,out ApptDrawing.VisOps,out ForCurView,out ApptRows,out ApptDrawing.RowsPerIncr);
			AddOpsForScheduledProvs(isWeekly,dailySched,ApptViewCur,ref ApptDrawing.VisOps);
			ApptDrawing.VisOps.Sort(CompareOps);
			ApptDrawing.VisProvs.Sort(CompareProvs);
		}

		///<summary>Fills visProvs, visOps, forCurView, apptRows, and rowsPerIncr based on the appointment view passed in and whether it is for the week view or not.  This method uses 'out' variables so that the encompassing logic doesn't ALWAYS affect the global static variables used to draw the appointment views.  We don't want the following logic to affect the global static variables in the case where we are trying to get information needed to filter the waiting room.</summary>
		private static void FillForApptView(bool isWeekly,ApptView apptViewCur,out List<Provider> visProvs,out List<Operatory> visOps,out List<ApptViewItem> forCurView,out List<ApptViewItem> apptRows,out int rowsPerIncr) {
			forCurView=new List<ApptViewItem>();
			visProvs=new List<Provider>();
			visOps=new List<Operatory>();
			apptRows=new List<ApptViewItem>();
			int index;
			//If there are no appointment views set up (therefore, none selected), then use a hard-coded default view.
			if(apptViewCur==null) {
				//make visible ops exactly the same as the short ops list (all except hidden)
				for(int i=0;i<OperatoryC.ListShort.Count;i++) {
					if(PrefC.GetBool(PrefName.EasyNoClinics) //if clinics disabled
						|| FormOpenDental.ClinicNum==0 //or if program level ClinicNum set to Headquarters
						|| OperatoryC.ListShort[i].ClinicNum==FormOpenDental.ClinicNum) //or if the op is assigned to the program level ClinicNum
					{
						visOps.Add(OperatoryC.ListShort[i]);
					}
				}
				//make visible provs exactly the same as the prov list (all except hidden)
				for(int i=0;i<ProviderC.ListShort.Count;i++) {
					visProvs.Add(ProviderC.ListShort[i]);
				}
				//Hard coded elements showing
				apptRows.Add(new ApptViewItem("PatientName",0,Color.Black));
				apptRows.Add(new ApptViewItem("ASAP",1,Color.DarkRed));
				apptRows.Add(new ApptViewItem("MedUrgNote",2,Color.DarkRed));
				apptRows.Add(new ApptViewItem("PremedFlag",3,Color.DarkRed));
				apptRows.Add(new ApptViewItem("Lab",4,Color.DarkRed));
				apptRows.Add(new ApptViewItem("Procs",5,Color.Black));
				apptRows.Add(new ApptViewItem("Note",6,Color.Black));
				rowsPerIncr=1;
			}
			//An appointment view is selected, so add provs and ops from the view to our lists of indexes.
			else {
				for(int i=0;i<ApptViewItemC.List.Length;i++) {
					if(ApptViewItemC.List[i].ApptViewNum==apptViewCur.ApptViewNum) {
						forCurView.Add(ApptViewItemC.List[i]);
						if(ApptViewItemC.List[i].OpNum>0) {//op
							if(apptViewCur.OnlyScheduledProvs && !isWeekly) {
								continue;//handled below
							}
							index=Operatories.GetOrder(ApptViewItemC.List[i].OpNum);
							if(index!=-1) {
								visOps.Add(OperatoryC.ListShort[index]);
							}
						}
						else if(ApptViewItemC.List[i].ProvNum>0) {//prov
							index=Providers.GetIndex(ApptViewItemC.List[i].ProvNum);
							if(index!=-1) {
								visProvs.Add(ProviderC.ListShort[index]);
							}
						}
						else {//element or apptfielddef
							apptRows.Add(ApptViewItemC.List[i]);
						}
					}
				}
				rowsPerIncr=apptViewCur.RowsPerIncr;
			}
		}

		///<summary>When looking at a daily appointment module and the current appointment view is has 'OnlyScheduleProvs' turned on, this method will dynamically add additional operatories to visOps for providers that are scheduled to work.</summary>
		private static void AddOpsForScheduledProvs(bool isWeekly,List<Schedule> dailySched,ApptView apptViewCur,ref List<Operatory> visOps) {
			//if this appt view has the option to show only scheduled providers and this is daily view.
			//Remember that there is no intelligence in weekly view for this option, and it behaves just like it always did.
			if(apptViewCur==null 
				|| dailySched==null
				|| apptViewCur==null
				|| visOps==null
				|| !apptViewCur.OnlyScheduledProvs
				|| isWeekly) 
			{
				return;
			}
			//intelligently decide what ops to show.  It's based on the schedule for the day.
			//visOps will be totally empty right now because it looped out of the above section of code.
			List<long> listSchedOps;
			bool opAdded;
			int indexOp;
			for(int i=0;i<OperatoryC.ListShort.Count;i++) {//loop through all ops for all views (except the hidden ones, of course)
				//find any applicable sched for the op
				opAdded=false;
				for(int s=0;s<dailySched.Count;s++) {
					if(dailySched[s].SchedType!=ScheduleType.Provider) {
						continue;
					}
					if(dailySched[s].StartTime==new TimeSpan(0)) {//skip if block starts at midnight.
						continue;
					}
					if(dailySched[s].StartTime==dailySched[s].StopTime) {//skip if block has no length.
						continue;
					}
					if(apptViewCur.OnlySchedAfterTime > new TimeSpan(0,0,0)) {
						if(dailySched[s].StartTime < apptViewCur.OnlySchedAfterTime
								|| dailySched[s].StopTime < apptViewCur.OnlySchedAfterTime) 
						{
							continue;
						}
					}
					if(apptViewCur.OnlySchedBeforeTime > new TimeSpan(0,0,0)) {
						if(dailySched[s].StartTime > apptViewCur.OnlySchedBeforeTime
								|| dailySched[s].StopTime > apptViewCur.OnlySchedBeforeTime) 
						{
							continue;
						}
					}
					//this 'sched' must apply to this situation.
					//listSchedOps is the ops for this 'sched'.
					listSchedOps=dailySched[s].Ops;
					//Add all the ops for this 'sched' to the list of visible ops
					for(int p=0;p<listSchedOps.Count;p++) {
						//Filter the ops if the clinic option was set for the appt view.
						if(apptViewCur.ClinicNum>0 && apptViewCur.ClinicNum!=Operatories.GetOperatory(listSchedOps[p]).ClinicNum) {
							continue;
						}
						if(listSchedOps[p]==OperatoryC.ListShort[i].OperatoryNum) {
							Operatory op=OperatoryC.ListShort[i];
							indexOp=Operatories.GetOrder(listSchedOps[p]);
							if(indexOp!=-1 && !visOps.Contains(op)) {//prevents adding duplicate ops
								visOps.Add(op);
								opAdded=true;
								break;
							}
						}
					}
					//If the provider is not scheduled to any op(s), add their default op(s).
					if(OperatoryC.ListShort[i].ProvDentist==dailySched[s].ProvNum && listSchedOps.Count==0) {//only if the sched does not specify any ops
						//Only add the op if the clinic option was not set in the appt view or if the op is assigned to that clinic.
						if(apptViewCur.ClinicNum==0 || apptViewCur.ClinicNum==OperatoryC.ListShort[i].ClinicNum) {
							indexOp=Operatories.GetOrder(OperatoryC.ListShort[i].OperatoryNum);
							if(indexOp!=-1 && !visOps.Contains(OperatoryC.ListShort[i])) {
								visOps.Add(OperatoryC.ListShort[i]);
								opAdded=true;
							}
						}
					}
					if(opAdded) {
						break;//break out of the loop of schedules.  Continue with the next op.
					}
				}
			}
		}

		///<summary>Sorts list of operatories by ItemOrder.</summary>
		private static int CompareOps(Operatory op1,Operatory op2) {
			if(op1.ItemOrder<op2.ItemOrder) {
				return -1;
			}
			else if(op1.ItemOrder>op2.ItemOrder) {
				return 1;
			}
			return 0;
		}

		///<summary>Sorts list of providers by ItemOrder.</summary>
		private static int CompareProvs(Provider prov1,Provider prov2) {
			if(prov1.ItemOrder<prov2.ItemOrder) {
				return -1;
			}
			else if(prov1.ItemOrder>prov2.ItemOrder) {
				return 1;
			}
			return 0;
		}

		///<summary>Only used in FormApptViewEdit. Must have run GetForCurView first.</summary>
		public static bool OpIsInView(long opNum) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<ForCurView.Count;i++) {
				if(ForCurView[i].OpNum==opNum)
					return true;
			}
			return false;
		}

		///<summary>Only used in ApptViewItem setup and ContrAppt (for search function - search for appt with prov in this view). Must have run GetForCurView first.</summary>
		public static bool ProvIsInView(long provNum) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<ForCurView.Count;i++) {
				if(ForCurView[i].ProvNum==provNum)
					return true;
			}
			return false;
		}



	}
}
