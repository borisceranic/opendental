using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using OpenDentBusiness;
using OpenDentBusiness.Crud;

namespace xFHIR {
	public class AppointmentController:ApiController {
		// GET: api/Appointment
		[ResponseType(typeof(IEnumerable<string>))]
		public IHttpActionResult Get() {
			DbConn.ConnectIfNecessary();
			#region Construct Query command
			var urlParams=ControllerContext.Request.GetQueryNameValuePairs();
			string command=@"SELECT appointment.AptNum AS IDExternal
				#oidexternal.IDExternal
				FROM appointment
				#INNER JOIN oidexternal ON oidexternal.IDInternal=appointment.AptNum
				LEFT JOIN oidexternal ON oidexternal.IDInternal=appointment.AptNum
					AND oidexternal.rootExternal='FHIR'
					AND IDType='Appointment'
				WHERE 1 ";
			foreach(KeyValuePair<string,string> kvp in urlParams) {
				if(kvp.Key.ToLower()=="patient") {
					command+="AND appointment.PatNum="+POut.String(kvp.Value)+" ";
				}
				if(kvp.Key.ToLower()=="location") {
					command+="AND appointment.Op="+POut.String(kvp.Value)+" ";
				}
				if(kvp.Key.ToLower()=="identifier") {
					command+="AND appointment.AptNum="+POut.String(kvp.Value)+" ";
				}
				if(kvp.Key.ToLower()=="practitioner") {
					command+="AND (appointment.ProvNum="+POut.String(kvp.Value)+" OR appointment.ProvHyg="+POut.String(kvp.Value)+") ";
				}
				if(kvp.Key.ToLower()=="status") {
					switch(kvp.Value) {
						case "booked":
							command+="AND appointment.AptStatus IN (1,4)";//Scheduled, ASAP
							break;
						case "cancelled":
						case "noshow":
							command+="AND appointment.AptStatus=5";//Broken
							break;
						case "proposed":
							command+="AND appointment.AptStatus=6";//Planned
							break;
						case "pending":
						case "undefined":
							command+="AND appointment.AptStatus=3";//Unsheduled
							break;
						case "fulfilled":
							command+="AND appointment.AptStatus=2";//Complete
							break;
						default:
							return BadRequest("Improper appointment status: "+kvp.Value);
					}
				}

				if(kvp.Key.ToLower()=="date") {
					string dateStr="AND appointment.AptDateTime";
					try {
						DateTime date;
						if(DateTime.TryParse(kvp.Value,out date)) {//A straight datetime with no prefixes
							dateStr+="="+POut.DateT(date)+" ";
							continue;
						}
						if(!DateTime.TryParse(kvp.Value.Substring(2),out date)) {
							return BadRequest("Improperly formatted date: "+kvp.Value);
						}
						string prefix=kvp.Value.Substring(0,2).ToLower();
						if(prefix=="eq") {
							dateStr+="="+POut.DateT(DateTime.Parse(kvp.Value.Substring(2)))+" ";
						}
						else if(prefix=="ne") {
							dateStr+="!="+POut.DateT(DateTime.Parse(kvp.Value.Substring(2)))+" ";
						}
						else if(prefix=="gt") {
							dateStr+=">"+POut.DateT(DateTime.Parse(kvp.Value.Substring(2)))+" ";
						}
						else if(prefix=="lt") {
							dateStr+="<"+POut.DateT(DateTime.Parse(kvp.Value.Substring(2)))+" ";
						}
						else if(prefix=="ge") {
							dateStr+=">="+POut.DateT(DateTime.Parse(kvp.Value.Substring(2)))+" ";
						}
						else if(prefix=="le") {
							dateStr+="<="+POut.DateT(DateTime.Parse(kvp.Value.Substring(2)))+" ";
						}
						else {//prefix not supported
							return BadRequest("Date parameter prefix '"+prefix+"' not supported.");
						}
						command+=dateStr;
					}
					catch {//Catch badly formatted dates
						return BadRequest("Improperly formatted date: "+kvp.Value);
					}
				}
			}
			#endregion
			DataTable tableIDEx=DbConn.GetDataTable(command);			
			string localPath=Request.RequestUri.GetLeftPart(UriPartial.Path);
			if(!localPath.EndsWith("/")) {
				localPath+="/";
			}
			List<string> listURLs=tableIDEx.Rows.Cast<DataRow>().Select(x=>localPath+x["IDExternal"]).ToList();
			return Ok(listURLs);
		}

		// GET: api/Appointment/5
		[ResponseType(typeof(AppointmentFHIR))]
		public IHttpActionResult Get(long id) {
			DbConn.ConnectIfNecessary();
			//OIDExternal oidEx=OIDExternals.GetByRootAndExtensionAndType("FHIR",POut.Long(id),IdentifierType.Appointment);
			//if(oidEx==null) {
			//	return StatusCode(HttpStatusCode.NoContent);
			//}
			//Appointment aptCur=AppointmentCrud.SelectOne(oidEx.IDInternal);
			Appointment aptCur=AppointmentCrud.SelectOne(id.ToString());
			if(aptCur==null) {
				return StatusCode(HttpStatusCode.NoContent);
			}
			return Ok(AppoinmentODBizToFHIR(aptCur));
		}
			
		// POST: api/Appointment. Creates a new appointment
		[ResponseType(typeof(AppointmentFHIR))]
		public IHttpActionResult Post(AppointmentFHIR aptF) {
			DbConn.ConnectIfNecessary();
			Appointment apt=AppointmentFHIRtoODBiz(aptF);
			if(apt.PatNum==0) {
				return BadRequest("Patient participant required");
			}
			if(Patients.GetPat(apt.PatNum)==null) {
				return BadRequest("Patient participant does not exist");
			}
			if(apt.Op==0 && new[] { AppointmentStatus.booked,AppointmentStatus.fulfilled,AppointmentStatus.arrived }.Contains(aptF.status)) {
				return BadRequest("Location (Operatory) required if appointment status is booked, fulfilled, or arrived");
			}
			if(apt.ProvNum!=0 && !ProviderC.GetListLong().Exists(x => x.ProvNum==apt.ProvNum)) {
				return BadRequest("Primary provider does not exist");
			}
			if(apt.ProvHyg!=0 && !ProviderC.GetListLong().Exists(x => x.ProvNum==apt.ProvHyg)) {
				return BadRequest("Secondary provider does not exist");
			}
			apt.AptNum=Appointments.InsertIncludeAptNum(apt,false);
			string logText=Lans.g("FormAudit","Created from FHIR.")+" "+apt.AptDateTime+" "+apt.ProcDescript;
			SecurityLogs.MakeLogEntry(Permissions.AppointmentCreate,apt.PatNum,logText,apt.AptNum,LogSources.FHIR);
			OIDExternal oidEx=new OIDExternal();
			oidEx.IDInternal=apt.AptNum;
			oidEx.IDType=IdentifierType.Appointment;
			oidEx.IDExternal=Shared.CreateExternalID(IdentifierType.Appointment);
			//OIDExternals.Insert(oidEx);
			Identifier ident=new Identifier();
			ident.use=IdentifierUse.usual;
			ident.type=new CodeableConcept() { text="Open Dental FK to appointment.AptNum" };
			ident.value=POut.Long(apt.AptNum);
			aptF.identifier=new List<Identifier>() { ident };
			return Ok(aptF);
		}			

		// PUT: api/Appointment/5. Updates an existing appointment
		[ResponseType(typeof(AppointmentFHIR))]
		public IHttpActionResult Put(long id,AppointmentFHIR aptF) {
			DbConn.ConnectIfNecessary();
			Appointment apt=AppointmentFHIRtoODBiz(aptF);
			if(apt.PatNum==0) {
				return BadRequest("Patient participant required");
			}
			if(Patients.GetPat(apt.PatNum)==null) {
				return BadRequest("Patient participant does not exist");
			}
			if(apt.Op==0 && new[] { AppointmentStatus.booked,AppointmentStatus.fulfilled,AppointmentStatus.arrived }.Contains(aptF.status)) {
				return BadRequest("Location (Operatory) required if appointment status is booked, fulfilled, or arrived");
			}
			if(apt.ProvNum!=0 && !ProviderC.GetListLong().Exists(x =>x.ProvNum==apt.ProvNum)) {
				return BadRequest("Primary provider does not exist");
			}
			if(apt.ProvHyg!=0 && !ProviderC.GetListLong().Exists(x => x.ProvNum==apt.ProvHyg)) {
				return BadRequest("Secondary provider does not exist");
			}
			//OIDExternal oidEx=OIDExternals.GetByRootAndExtensionAndType("FHIR",POut.Long(id),IdentifierType.Appointment);
			//if(oidEx==null) {
			//	return StatusCode(HttpStatusCode.NoContent);
			//}
			//apt.AptNum=oidEx.IDInternal;
			apt.AptNum=id;
			Appointment aptOld=Appointments.GetOneApt(apt.AptNum);
			if(aptOld==null) {
				return StatusCode(HttpStatusCode.NoContent);
			}
			Appointments.Update(apt,aptOld);
			string logText=Lans.g("FormAudit","Updated from FHIR.")+" "+apt.AptDateTime+" "+apt.ProcDescript;
			if(aptOld.AptStatus==ApptStatus.Complete) {
				SecurityLogs.MakeLogEntry(Permissions.AppointmentCompleteEdit,apt.PatNum,logText,apt.AptNum,LogSources.FHIR);
			}
			else {
				SecurityLogs.MakeLogEntry(Permissions.AppointmentEdit,apt.PatNum,logText,apt.AptNum,LogSources.FHIR);
			}
			return Ok(aptF);
		}

		// DELETE: api/Appointment/5
		public IHttpActionResult Delete(long id) {
			DbConn.ConnectIfNecessary();
			//OIDExternal oidEx=OIDExternals.GetByRootAndExtensionAndType("FHIR",POut.Long(id),IdentifierType.Appointment);
			//if(oidEx==null) {
			//	return StatusCode(HttpStatusCode.NoContent);
			//}
			//long aptNum=oidEx.IDInternal;
			//OIDExternals.Delete(oidEx.OIDExternalNum);
			long aptNum=id;
			Appointments.Delete(aptNum);
			Appointment apt=Appointments.GetOneApt(aptNum);
			string logText=Lans.g("FormAudit","Deleted from FHIR.")+" "+apt.AptDateTime+" "+apt.ProcDescript;
			if(apt.AptStatus==ApptStatus.Complete) {
				SecurityLogs.MakeLogEntry(Permissions.AppointmentCompleteEdit,apt.PatNum,logText,apt.AptNum,LogSources.FHIR);
			}
			else {
				SecurityLogs.MakeLogEntry(Permissions.AppointmentEdit,apt.PatNum,logText,apt.AptNum,LogSources.FHIR);
			}
			return Ok();
		}

		#region Helper Methods

		private Appointment AppointmentFHIRtoODBiz(AppointmentFHIR aptF) {
			Appointment apt=new Appointment();
			switch(aptF.status) {
				case AppointmentStatus.booked:
					if(aptF.priority<4) {
						apt.AptStatus=ApptStatus.ASAP;
					}
					else {
						apt.AptStatus=ApptStatus.Scheduled;
					}
					break;
				case AppointmentStatus.cancelled:
				case AppointmentStatus.noshow:
					apt.AptStatus=ApptStatus.Broken;
					break;
				case AppointmentStatus.proposed:
					apt.AptStatus=ApptStatus.Planned;
					break;
				case AppointmentStatus.pending:
				case AppointmentStatus.undefined:
					apt.AptStatus=ApptStatus.UnschedList;
					break;
				case AppointmentStatus.fulfilled:
					apt.AptStatus=ApptStatus.Complete;
					break;
				default:
					apt.AptStatus=ApptStatus.UnschedList;
					break;
			}
			//if(aptF.type!=null) {
			//	AppointmentType aptType=AppointmentTypes.Listt.FirstOrDefault(x => x.SnomedCode==aptF.type.code.code) ?? new AppointmentType();
			//	apt.AppointmentTypeNum=aptType.AppointmentTypeNum;
			//}
			apt.ProcDescript=aptF.description;
			apt.AptDateTime=aptF.start;
			apt.Pattern=new string('/',aptF.minutesDuration/5);
			apt.Note=aptF.comment;
			foreach(AppointmentParticipant participant in aptF.participant) {
				if(participant.actor==null || participant.actor.reference==null) {
					continue;
				}
				string refID=participant.actor.reference.Substring(participant.actor.reference.LastIndexOf("/")+1);//If reference='Patient/37' then refID=37
				if(participant.actor.reference.Contains("Location")) {
					apt.Op=PIn.Long(refID);
				}
				if(participant.type==null) {
					continue;
				}
				foreach(Coding code in participant.type.SelectMany(x => x.code)) {
					if(code==null) {
						continue;
					}
					if(code.code=="PART") {//Participation
						apt.PatNum=PIn.Long(refID);
					}
					else if(code.code=="PPRF") {//Primary performer
						apt.ProvNum=PIn.Long(refID);
					}
					else if(code.code=="SPRF") {//Secondary performer
						apt.ProvHyg=PIn.Long(refID);
					}
				}
			}
			return apt;
		}

		private AppointmentFHIR AppoinmentODBizToFHIR(Appointment apt) {
			AppointmentFHIR aptF=new AppointmentFHIR();
			Identifier ident=new Identifier();
			ident.use=IdentifierUse.usual;
			ident.type=new CodeableConcept() { text="Open Dental FK to appointment.AptNum" };
			ident.value=POut.Long(apt.AptNum);
			aptF.identifier=new List<Identifier>() { ident };
			aptF.status=AptStatusHelper(apt);
			//aptF.type=AptTypeHelper(apt);
			//aptF.reason=new CodeableConcept();
			aptF.priority=PriorityHelper(apt);
			aptF.description=apt.ProcDescript;
			aptF.start=apt.AptDateTime;
			aptF.minutesDuration=apt.Pattern.Length*5;
			aptF.end=apt.AptDateTime.AddMinutes(aptF.minutesDuration);
			aptF.comment=apt.Note;
			aptF.participant=GetAptParticipants(apt);
			return aptF;
		}

		private AppointmentStatus AptStatusHelper(Appointment apt) {
			switch(apt.AptStatus) {
				case ApptStatus.ASAP:
				case ApptStatus.Scheduled:
					return AppointmentStatus.booked;
				case ApptStatus.Complete:
					return AppointmentStatus.fulfilled;
				case ApptStatus.Planned:
				case ApptStatus.UnschedList:
					return AppointmentStatus.proposed;
				case ApptStatus.Broken:
					return AppointmentStatus.noshow;
			}
			return AppointmentStatus.undefined;
		}

		private int PriorityHelper(Appointment apt) {
			if(apt.AptStatus==ApptStatus.ASAP) {
				return 1;
			}
			else {
				return 5;
			}
		}

		private string ClinicHelper(long clinicNum) {
			if(clinicNum==0) {
				return "Unassigned";
			}
			else {
				return Clinics.GetClinic(clinicNum).Description;
			}
		}

		private CodeableConcept AptTypeHelper(Appointment apt) {
			AppointmentType aptType=AppointmentTypes.GetOne(apt.AppointmentTypeNum);
			CodeableConcept type=new CodeableConcept();
			if(aptType!=null) {
				type.text=aptType.AppointmentTypeName;
				//type.code.code=aptType.SnomedCode;
			}
			else {
				type.text="none";
				//type.code.code="none";
			}
			return type;

		}

		private string AptEndTimeHelper(Appointment apt) {
			return DateTime.SpecifyKind(apt.AptDateTime,DateTimeKind.Local).AddMinutes(apt.Pattern.Length*5).ToString("o");
		}

		private List<AppointmentParticipant> GetAptParticipants(Appointment apt) {
			List<AppointmentParticipant> listParticipants=new List<AppointmentParticipant>();
			AppointmentParticipant participant;
			if(apt.PatNum != 0) {
				participant=new AppointmentParticipant();
				Coding partCode=new Coding();
				partCode.code="PART";
				partCode.system="http://hl7.org/fhir/v3/ParticipationType";
				partCode.display="Participation";
				participant.type=new List<CodeableConcept> { new CodeableConcept { code=new List<Coding>() { partCode }  } };
				Reference partRef=new Reference();
				partRef.reference="Patient/"+apt.PatNum;
				partRef.display=Patients.GetLim(apt.PatNum).GetNameFirstOrPrefML();
				participant.actor=partRef;
				participant.required=ParticipantRequired.required;
				participant.status=ParticipationStatus.accepted;
				listParticipants.Add(participant);
			}
			if(apt.ProvNum != 0) {
				participant=new AppointmentParticipant();
				Coding partCode=new Coding();
				partCode.code="PPRF";
				partCode.system="http://hl7.org/fhir/v3/ParticipationType";
				partCode.display="primary performer";
				participant.type=new List<CodeableConcept> { new CodeableConcept { code=new List<Coding>() { partCode } } };
				Reference partRef=new Reference();
				partRef.reference="Practitioner/"+apt.ProvNum;
				partRef.display=OpenDentBusiness.Providers.GetFormalName(apt.ProvNum);
				participant.actor=partRef;
				participant.required=ParticipantRequired.required;
				participant.status=ParticipationStatus.accepted;
				listParticipants.Add(participant);
			}
			if(apt.ProvHyg != 0) {
				participant=new AppointmentParticipant();
				Coding partCode=new Coding();
				partCode.code="SPRF";
				partCode.system="http://hl7.org/fhir/v3/ParticipationType";
				partCode.display="secondary performer";
				participant.type=new List<CodeableConcept> { new CodeableConcept { code=new List<Coding>() { partCode } } };
				Reference partRef=new Reference();
				partRef.reference="Practitioner/"+apt.ProvHyg;
				partRef.display=OpenDentBusiness.Providers.GetFormalName(apt.ProvHyg);
				participant.actor=partRef;
				participant.required=ParticipantRequired.required;
				participant.status=ParticipationStatus.accepted;
				listParticipants.Add(participant);
			}
			if(apt.Op != 0) {
				participant=new AppointmentParticipant();
				Reference partRef=new Reference();
				partRef.reference="Location/"+apt.Op;
				partRef.display=Operatories.GetAbbrev(apt.Op);
				participant.actor=partRef;
				participant.required=ParticipantRequired.required;
				participant.status=ParticipationStatus.accepted;
				listParticipants.Add(participant);
			}
			return listParticipants;
		}
		#endregion

	}
}
