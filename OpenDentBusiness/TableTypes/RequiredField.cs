using System;
using System.Collections.Generic;

namespace OpenDentBusiness {
	///<summary>Each row represents a field that is required to be filled out.</summary>
	[Serializable]
	public class RequiredField:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long RequiredFieldNum;
		///<summary>Enum:RequiredFieldType. The area of the program that uses this field.</summary>
		public RequiredFieldType FieldType;
		///<summary>Enum:RequiredFieldName.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.EnumAsString)]
		public RequiredFieldName FieldName;
		///<summary>This is not a data column but is stored in a seperate table named RequiredFieldCondition.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		private List<RequiredFieldCondition> _listRequiredFieldConditions;

		public List<RequiredFieldCondition> ListRequiredFieldConditions {
			get {
				if(_listRequiredFieldConditions==null) {
					if(RequiredFieldNum==0) {
						_listRequiredFieldConditions=new List<RequiredFieldCondition>();
					}
					else {
						_listRequiredFieldConditions=RequiredFieldConditions.GetForRequiredField(RequiredFieldNum);
					}
				}
				return _listRequiredFieldConditions;
			}
			set {
				_listRequiredFieldConditions=value;
			}
		}

		///<summary>Refreshes the list holding the requirefieldconditions for this requiredfield.</summary>
		public void RefreshConditions() {
			_listRequiredFieldConditions=null;
			RequiredFieldConditions.RefreshCache();
		}

		///<summary></summary>
		public RequiredField Clone() {
			return (RequiredField)this.MemberwiseClone();
		}
	}

	public enum RequiredFieldType {
		///<summary>0 - Edit Patient Information window and Add Family window.</summary>
		PatientInfo
	}

	public enum RequiredFieldName {
		Address,
		Address2,
		AddressPhoneNotes,
		AdmitDate,
		AskArriveEarly,
		BillingType,
		Birthdate,
		Carrier,
		ChartNumber,
		City,
		Clinic,
		CollegeName,
		County,
		CreditType,
		DateFirstVisit,
		DateTimeDeceased,
		EligibilityExceptCode,
		EmailAddress,
		Employer,
		Ethnicity,
		FeeSchedule,
		FirstName,
		Gender,
		GradeLevel,
		GroupName,
		GroupNum,
		HomePhone,
		InsurancePhone,
		InsuranceSubscriber,
		InsuranceSubscriberID,
		Language,
		LastName,
		Position,
		MedicaidID,
		MedicaidState,
		MiddleInitial,
		MothersMaidenFirstName,
		MothersMaidenLastName,
		PatientStatus,
		PreferConfirmMethod,
		PreferContactMethod,
		PreferRecallMethod,
		PreferredName,
		PrimaryProvider,
		Race,
		ReferredFrom,
		ResponsibleParty,
		Salutation,
		SecondaryProvider,
		Site,
		SocialSecurityNumber,
		State,
		StudentStatus,
		TextOK,
		Title,
		TreatmentUrgency,
		TrophyFolder,
		Ward,
		WirelessPhone,
		WorkPhone,
		Zip
	}
}