using System;
using System.Collections;

namespace OpenDentBusiness{

	///<summary>Every user group has certain permissions.  This defines a permission for a group.  The absense of permission would cause that row to be deleted from this table.</summary>
	[Serializable]
	public class GroupPermission:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long GroupPermNum;
		///<summary>Only granted permission if newer than this date.  Can be Minimum (01-01-0001) to always grant permission.</summary>
		public DateTime NewerDate;
		///<summary>Can be 0 to always grant permission.  Otherwise, only granted permission if item is newer than the given number of days.  1 would mean only if entered today.</summary>
		public int NewerDays;
		///<summary>FK to usergroup.UserGroupNum.  The user group for which this permission is granted.  If not authorized, then this groupPermission will have been deleted.</summary>
		public long UserGroupNum;
		///<summary>Enum:Permissions</summary>
		public Permissions PermType;

		///<summary></summary>
		public GroupPermission Copy(){
			return (GroupPermission)this.MemberwiseClone();
		}

	}

	///<summary>A hard-coded list of permissions which may be granted to usergroups.</summary>
	public enum Permissions {
		///<summary>0</summary>
		None,
		///<summary>1</summary>
		AppointmentsModule,
		///<summary>2</summary>
		FamilyModule,
		///<summary>3</summary>
		AccountModule,
		///<summary>4</summary>
		TPModule,
		///<summary>5</summary>
		ChartModule,
		///<summary>6</summary>
		ImagesModule,
		///<summary>7</summary>
		ManageModule,
		///<summary>8. Currently covers a wide variety of setup functions. </summary>
		Setup,
		///<summary>9</summary>
		RxCreate,
		///<summary>10. Uses date restrictions.  Covers editing AND deleting of completed procs.  Deleting non-completed procs is covered by ProcDelete.</summary>
		ProcComplEdit,
		///<summary>11</summary>
		ChooseDatabase,
		///<summary>12</summary>
		Schedules,
		///<summary>13</summary>
		Blockouts,
		///<summary>14. Uses date restrictions.</summary>
		ClaimSentEdit,
		///<summary>15</summary>
		PaymentCreate,
		///<summary>16. Uses date restrictions.</summary>
		PaymentEdit,
		///<summary>17</summary>
		AdjustmentCreate,
		///<summary>18. Uses date restrictions.</summary>
		AdjustmentEdit,
		///<summary>19</summary>
		UserQuery,
		///<summary>20.  Not used anymore.</summary>
		StartupSingleUserOld,
		///<summary>21 Not used anymore.</summary>
		StartupMultiUserOld,
		///<summary>22</summary>
		Reports,
		///<summary>23. Includes setting procedures complete.</summary>
		ProcComplCreate,
		///<summary>24. At least one user must have this permission.</summary>
		SecurityAdmin,
		///<summary>25. </summary>
		AppointmentCreate,
		///<summary>26</summary>
		AppointmentMove,
		///<summary>27</summary>
		AppointmentEdit,
		///<summary>28</summary>
		Backup,
		///<summary>29</summary>
		TimecardsEditAll,
		///<summary>30</summary>
		DepositSlips,
		///<summary>31. Uses date restrictions.</summary>
		AccountingEdit,
		///<summary>32. Uses date restrictions.</summary>
		AccountingCreate,
		///<summary>33</summary>
		Accounting,
		///<summary>34</summary>
		AnesthesiaIntakeMeds,
		///<summary>35</summary>
		AnesthesiaControlMeds,
		///<summary>36</summary>
		InsPayCreate,
		///<summary>37. Uses date restrictions. Edit Batch Insurance Payment.</summary>
		InsPayEdit,
		///<summary>38. Uses date restrictions.</summary>
		TreatPlanEdit,
		///<summary>39</summary>
		ReportProdInc,
		///<summary>40. Uses date restrictions.</summary>
		TimecardDeleteEntry,
		///<summary>41. Uses date restrictions. All other equipment functions are covered by .Setup.</summary>
		EquipmentDelete,
		///<summary>42. Uses date restrictions. Also used in audit trail to log web form importing.</summary>
		SheetEdit,
		///<summary>43. Uses date restrictions.</summary>
		CommlogEdit,
		///<summary>44. Uses date restrictions.</summary>
		ImageDelete,
		///<summary>45. Uses date restrictions.</summary>
		PerioEdit,
		///<summary>46. Shows the fee textbox in the proc edit window.</summary>
		ProcEditShowFee,
		///<summary>47</summary>
		AdjustmentEditZero,
		///<summary>48</summary>
		EhrEmergencyAccess,
		///<summary>49. Uses date restrictions.  This only applies to non-completed procs.  Deletion of completed procs is covered by ProcComplEdit.</summary>
		ProcDelete,
		///<summary>50 - Only used at OD HQ.  No user interface.</summary>
		EhrKeyAdd,
		///<summary>51- Allows user to edit all providers. This is not fine-grained enough for extremely large organizations such as dental schools, so other permissions are being added as well.</summary>
		Providers,
		///<summary>52</summary>
		EcwAppointmentRevise,
		///<summary>53</summary>
		ProcedureNote,
		///<summary>54</summary>
		ReferralAdd,
		///<summary>55</summary>
		InsPlanChangeSubsc,
		///<summary>56</summary>
		RefAttachAdd,
		///<summary>57</summary>
		RefAttachDelete,
		///<summary>58</summary>
		CarrierCreate,
		///<summary>59</summary>
		ReportDashboard,
		///<summary>60</summary>
		AutoNoteQuickNoteEdit,
		///<summary>61</summary>
		EquipmentSetup,
		///<summary>62</summary>
		Billing,
		///<summary>63</summary>
		ProblemEdit,
		///<summary>64- There is no user interface in the security window for this permission.  It is only used for tracking.  FK to CodeNum.</summary>
		ProcFeeEdit,
		///<summary>65- There is no user interface in the security window for this permission.  It is only used for tracking.  Only tracks changes to carriername, not any other carrier info.  FK to PlanNum for tracking.</summary>
		InsPlanChangeCarrierName,
		///<summary>66- (Was named TaskEdit prior to version 14.2.39) When editing an existing task: delete the task, edit original description, or double click on note rows.  Even if you don't have the permission, you can still edit your own task description (but not the notes) as long as it's in your inbox and as long as nobody but you has added any notes. </summary>
		TaskNoteEdit,
		///<summary>67- Add or delete lists and list columns..</summary>
		WikiListSetup,
		///<summary>68- There is no user interface in the security window for this permission.  It is only used for tracking.  Tracks copying of patient information.  Required by EHR.</summary>
		Copy,
		///<summary>69- There is no user interface in the security window for this permission.  It is only used for tracking.  Tracks printing of patient information.  Required by EHR.</summary>
		Printing,
		///<summary>70- There is no user interface in the security window for this permission.  It is only used for tracking.  Tracks viewing of patient medical information.</summary>
		MedicalInfoViewed,
		///<summary>71- There is no user interface in the security window for this permission.  It is only used for tracking.  Tracks creation and editing of patient problems.</summary>
		PatProblemListEdit,
		///<summary>72- There is no user interface in the security window for this permission.  It is only used for tracking.  Tracks creation and edting of patient medications.</summary>
		PatMedicationListEdit,
		///<summary>73- There is no user interface in the security window for this permission.  It is only used for tracking.  Tracks creation and editing of patient allergies.</summary>
		PatAllergyListEdit,
		///<summary>74- There is no user interface in the security window for this permission.  It is only used for tracking.  Tracks creation and editing of patient family health history.</summary>
		PatFamilyHealthEdit,
		///<summary>75- There is no user interface in the security window for this permission.  It is only used for tracking.  Patient Portal access of patient information.  Required by EHR.</summary>
		PatientPortal,
		///<summary>76</summary>
		RxEdit,
		///<summary>77- Assign this permission to a staff person who will administer setting up and editing Dental School Students in the system.</summary>
		AdminDentalStudents,
		///<summary>78- Assign this permission to an instructor who will be allowed to assign Grades to Dental School Students as well as manage classes assigned to them.</summary>
		AdminDentalInstructors,
		///<summary>79- Uses date restrictions.  Has a unique audit trail so that users can track specific ortho chart edits.  FK to OrthoChartNum.</summary>
		OrthoChartEdit,
		///<summary>80- There is no user interface in the security window for this permission.  It is only used for tracking.  Mainly used for ortho clinics.</summary>
		PatientFieldEdit,
		///<summary>81- Assign this permission to a staff person who will edit evaluations in case of an emergency.  This is not meant to be a permanent permission given to a group.</summary>
		AdminDentalEvaluations,
		///<summary>82- There is no user interface in the security window for this permission.  It is only used for tracking.</summary>
		TreatPlanDiscountEdit,
		///<summary>83- There is no user interface in the security window for this permission.  It is only used for tracking.</summary>
		UserLogOnOff,
		///<summary>84- Allows user to edit other users' tasks.</summary>
		TaskEdit,
		///<summary>85- Allows user to send unsecured email</summary>
		EmailSend,
		///<summary>86- Allows user to send webmail</summary>
		WebmailSend,
		///<summary>87- Allows user to run command queries. Command queries are any non-SELECT queries for any non-temporary table.</summary>
		UserQueryAdmin,
		///<summary>88- Security permission for assignment of benefits.</summary>
		InsPlanChangeAssign,
		///<summary>89- Audit trail for images and documents in the image module.  There is no user interface in the security window for this permission because it is only used for tracking.</summary>
		ImageEdit,
		///<summary>90- Allows editing of all measure events.  Also used to track changes made to events.</summary>
		EhrMeasureEventEdit,
		///<summary>91- Allows users to edit settings in the eServices Setup window.  Also causes the Listener Service monitor thread to start upon logging in.</summary>
		EServicesSetup,
		///<summary>92- There is no user interface in the security window for this permission.  It is only used for tracking.  Tracks editing of fee schedule properties.</summary>
		FeeSchedEdit,
		///<summary>93- Allows user to edit and delete provider specific fees overrides.</summary>
		ProviderFeeEdit,
		///<summary>94- Allows user to merge patients.</summary>
		PatientMerge,
		///<summary>95- Only used in Claim History Status Edit</summary>
		ClaimHistoryEdit,
		///<summary>96- Allows user to edit a completed appointment.</summary>
		AppointmentCompleteEdit,
		///<summary>97- Audit trail for deleting webmail messages.  There is no user interface in the security window for this permission.</summary>
		WebmailDelete,
		///<summary>98- Audit trail for saving a patient with required fields missing.  There is no user interface in the security window for this 
		///permission.</summary>
		RequiredFields,
		///<summary>99- Allows user to merge referrals.</summary>
		ReferralMerge,
		///<summary>100- There is no user interface in the security window for this permission.  It is only used for tracking.
		///Currently only used for tracking automatically changing the IsCpoe flag on procedures.  Can be enhanced to do more in the future.
		///There is only one place where we could have automatically changed IsCpoe without a corresponding log of a different permission.
		///That place is in the OnClosing of the Procedure Edit window.  We update this flag even when the user Cancels out of it.</summary>
		ProcEdit
	}

	
}













