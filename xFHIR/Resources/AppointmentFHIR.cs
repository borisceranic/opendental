using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenDentBusiness;

namespace xFHIR {
	///<summary>A booking of a healthcare event among patient(s), practitioner(s), related person(s) and/or device(s) for a specific date/time. 
	///This may result in one or more Encounter(s).</summary>
	public class AppointmentFHIR {
		///<summary>External Ids for this item.</summary>
		public List<Identifier> identifier;
		///<summary>Enum:AppointmentStatus. The status of the appointment.</summary>
		public AppointmentStatus status;
		///<summary>SNOMED CT code giving the type of appointment. Because OD does not have a good way to implement a SNOMED appointment type,
		///not implemented yet. https://www.hl7.org/fhir/valueset-c80-practice-codes.html </summary>
		//public CodeableConcept type;
		///<summary>SNOMED CT code giving the reason for the appointment. OD doesn't have a way to specify this yet, not implemented yet.</summary>
		//public CodeableConcept reason;
		///<summary>0 as undefined, 1 as highest, 9 as lowest priority. Used to make informed decisions if needing to re-prioritize.</summary>
		public int priority;
		///<summary>Uses appointment.ProcDescript.</summary>
		public string description;
		///<summary>The time the appointment starts. Only proposed or cancelled appointments can be missing start/end dates. Either start and end 
		///are specified, or neither. Uses appointment.AptDateTime.</summary>
		public DateTime start;
		///<summary>The time the appointment ends. Only proposed or cancelled appointments can be missing start/end dates. Either start and end are 
		///specified, or neither. Uses appointment.AptDateTime+Pattern.</summary>
		public DateTime end;
		///<summary>Can be less than start/end (e.g. estimate). Uses appointment.Pattern.</summary>
		public int minutesDuration;
		///<summary>Additional comments. Uses appointment.Note</summary>
		public string comment;
		///<summary>Participants involved in appointment. Either the type or actor on the participant MUST be specified</summary>
		public List<AppointmentParticipant> participant;
	}


	///<summary>.</summary>
	public class AppointmentParticipant {
		///<summary>.</summary>
		public List<CodeableConcept> type;
		///<summary>Patient | Practitioner | RelatedPerson | Device | HealthcareService | Location.</summary>
		public Reference actor;
		///<summary>.</summary>
		public ParticipantRequired required;
		///<summary>.</summary>
		public ParticipationStatus status;

	}

	public enum AppointmentStatus {
		undefined,
		///<summary>  Proposed  None of the participant(s) have finalized their acceptance of the appointment request, and the start/end time may not 
		///be set yet.</summary>
		proposed,
		///<summary> Pending Some or all of the participant(s) have not finalized their acceptance of the appointment request.</summary>
		pending,
		///<summary>  Booked  All participant(s) have been considered and the appointment is confirmed to go ahead at the date/times specified.</summary>
		booked,
		///<summary> Arrived Some of the patients have arrived.</summary>
		arrived,
		///<summary> Fulfilled This appointment has completed and may have resulted in an encounter.</summary>
		fulfilled,
		///<summary> Cancelled The appointment has been cancelled.</summary>
		cancelled,
		///<summary>  No Show Some or all of the participant(s) have not/did not appear for the appointment (usually the patient).</summary>
		noshow
	}

	public enum ParticipantRequired {
		///<summary>The participant is required to attend the appointment.</summary>
		required,
		///<summary> The participant may optionally attend the appointment.</summary>
		optional,
		///<summary>The participant is excluded from the appointment, and may not be informed of the appointment taking place. (Appointment is about
		///them, not for them - such as 2 doctors discussing results about a patient's test).</summary>
		informationonly
	}

	public enum ParticipationStatus {
		///<summary>.</summary>
		accepted,
		///<summary>.</summary>
		declined,
		///<summary>.</summary>
		tentative,
		///<summary>.</summary>
		needsaction
	}

	public enum ParticipationType {
		none,
		///<summary>http://hl7.org/fhir/participant-type	Translator.</summary>
		translator,
		///<summary>http://hl7.org/fhir/participant-type	Emergency.</summary>
		emergency,
		///<summary>http://hl7.org/fhir/v3/ParticipationType	admitter.</summary>
		ADM,
		///<summary>  http://hl7.org/fhir/v3/ParticipationType	attender.</summary>
		ATND,
		///<summary> http://hl7.org/fhir/v3/ParticipationType	callback contact.</summary>
		CALLBCK,
		///<summary> http://hl7.org/fhir/v3/ParticipationType	consultan.t</summary>
		CON,
		///<summary> http://hl7.org/fhir/v3/ParticipationType	discharger.</summary>
		DIS,
		///<summary> http://hl7.org/fhir/v3/ParticipationType	escort.</summary>
		ESC,
		///<summary> http://hl7.org/fhir/v3/ParticipationType	referrer.</summary>
		REF,
		///<summary>  http://hl7.org/fhir/v3/ParticipationType	secondary performer.</summary>
		SPRF,
		///<summary>  http://hl7.org/fhir/v3/ParticipationType	primary performer.</summary>
		PPRF,
		///<summary>  http://hl7.org/fhir/v3/ParticipationType	Participation.</summary>
		PART
	}


}