using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using System.Diagnostics;

namespace OpenDentBusiness.HL7 {
	///<summary>This is the engine that will parse our incoming HL7 messages for MedLab interfaces.</summary>
	public class MessageParserMedLab {
		private static bool _isVerboseLogging;
		private static MedLab _medLabCur;
		private static MedLabResult _medLabResult;
		private static Patient _patCur;

		public static void Process(MessageHL7 msg,bool isVerboseLogging) {
			_isVerboseLogging=isVerboseLogging;
			HL7Def def=HL7Defs.GetOneDeepEnabled(true);
			if(def==null) {
				throw new Exception("Could not process the MedLab HL7 message.  No MedLab HL7 definition is enabled.");
			}
			HL7DefMessage hl7defmsg=null;
			for(int i=0;i<def.hl7DefMessages.Count;i++) {
				//for now there are only incoming ORU messages supported, so there should only be one defined message type and it should be inbound
				if(def.hl7DefMessages[i].MessageType==msg.MsgType && def.hl7DefMessages[i].InOrOut==InOutHL7.Incoming) {
					hl7defmsg=def.hl7DefMessages[i];
					break;
				}
			}
			if(hl7defmsg==null) {//No message definition matches this message's MessageType and is Incoming
				throw new Exception("Could not process the MedLab HL7 message.  There is no definition for this type of message in the enabled MedLab HL7Def.");
			}
			#region Locate Patient
			//for MedLab interfaces, we limit the ability to rearrange the message structure, so the PID segment is always in position 1
			if(hl7defmsg.hl7DefSegments.Count<2 || msg.Segments.Count<2) {
				throw new Exception("Could not process the MedLab HL7 message.  "
					+"The message or message definition for this type of message does not contain the correct number of segments.");
			}
			HL7DefSegment pidSegDef=hl7defmsg.hl7DefSegments[1];
			SegmentHL7 pidSegCur=msg.Segments[1];
			if(pidSegDef.SegmentName!=SegmentNameHL7.PID || pidSegCur.Name!=SegmentNameHL7.PID) {
				throw new Exception("Could not process the MedLab HL7 message.  "
					+"The second segment in the message or message definition is not the PID segment.");
			}
			//get the patient from the PID segment
			_patCur=GetPatFromPID(pidSegDef,pidSegCur);
			if(_patCur==null) {
				//TODO: Handle this, might process the message anyway and just not link it to a patient so the user can later link it manually, possibly throw an exception
			}
			#endregion Locate Patient
			#region Validate Message Structure
			if(hl7defmsg.hl7DefSegments.Count<1 || msg.Segments.Count<1) {
				throw new Exception("Could not process the MedLab HL7 message.  "
					+"The message or message definition for this type of message does not contain the correct number of segments.");
			}
			SegmentHL7 mshSegCur=msg.Segments[0];
			if(hl7defmsg.hl7DefSegments[0].SegmentName!=SegmentNameHL7.MSH || mshSegCur.Name!=SegmentNameHL7.MSH) {
				throw new Exception("Could not process the MedLab HL7 message.  "
					+"The first segment in the message or the message definition is not the MSH segment.");
			}
			SegmentHL7 nk1SegCur=null;
			List<SegmentHL7> listNteSegs=new List<SegmentHL7>();
			List<SegmentHL7> listZpsSegs=new List<SegmentHL7>();
			int indexFirstOrc=0;
			int indexFirstZps=0;
			//the third item could be the optional NK1 segment, followed by an optional and repeatable NTE segment, so the first ORC segment
			//could be anywhere after the PID segment in position 2.  This will tell us where the first ORC (order) repeatable group begins.
			for(int i=2;i<msg.Segments.Count;i++) {
				if(indexFirstZps>0 && msg.Segments[i].Name!=SegmentNameHL7.ZPS) {
					throw new Exception("Could not process the MedLab HL7 message.  There is a "+msg.Segments[i].Name.ToString()
						+" segment after a ZPS segment.  Incorrect message structure.");
				}
				if(msg.Segments[i].Name==SegmentNameHL7.NK1) {
					//the NK1 segment must come after the PID segment
					if(msg.Segments[i-1].Name!=SegmentNameHL7.PID) {
						throw new Exception("Could not process the MedLab HL7 message.  The NK1 segment was in the wrong position.  Incorrect message structure.");
					}
					nk1SegCur=msg.Segments[i];
					continue;
				}
				if(indexFirstOrc==0 && msg.Segments[i].Name==SegmentNameHL7.NTE) {//if we find an ORC segment, the NTEs that follow won't be at the PID level
					//the PID level NTE segments can follow after the PID segment, the optional NK1 segment, or other repetitions of the NTE segment
					if(msg.Segments[i-1].Name!=SegmentNameHL7.PID
						&& msg.Segments[i-1].Name!=SegmentNameHL7.NK1
						&& msg.Segments[i-1].Name!=SegmentNameHL7.NTE)
					{
						throw new Exception("Could not process the MedLab HL7 message.  Found a NTE segment before an ORC segment but after a "
							+msg.Segments[i-1].Name.ToString()+" segment.  Incorrect message structure.");
					}
					listNteSegs.Add(msg.Segments[i]);
					continue;
				}
				if(msg.Segments[i].Name==SegmentNameHL7.ZPS) {
					if(indexFirstZps==0) {//this is the first ZPS segment we've encountered, set the index
						indexFirstZps=i;
					}
					listZpsSegs.Add(msg.Segments[i]);
					continue;
				}
				if(indexFirstOrc==0 && msg.Segments[i].Name==SegmentNameHL7.ORC) {//this is the first ORC segment we've encountered, set the index
					indexFirstOrc=i;
				}
			}
			#endregion Validate Message Structure
			#region Process Order Observations
			List<SegmentHL7> listRepeatSegs=new List<SegmentHL7>();
			for(int i=indexFirstOrc;i<indexFirstZps;i++) {
				//for every repetition of the order observation, process the MSH, PID, NK1 and PID level NTE segments for a new MedLab object
				if(msg.Segments[i].Name==SegmentNameHL7.ORC) {
					if(_medLabCur!=null) {
						//update _medLabCur if we've processed an ORC and every segment after and have reached another ORC segment
						//ProcessMSH will instantiate a new MedLab object, so update to save all of the other segment details
						MedLabs.Update(_medLabCur);
					}
					ProcessSeg(hl7defmsg,new List<SegmentHL7> { mshSegCur },msg);//instatiates a new MedLab object _medLabCur and inserts to get PK
					ProcessSeg(hl7defmsg,new List<SegmentHL7> { pidSegCur },msg);
					ProcessSeg(hl7defmsg,new List<SegmentHL7> { nk1SegCur },msg);
					ProcessSeg(hl7defmsg,listNteSegs,msg,SegmentNameHL7.PID);
					ProcessSeg(hl7defmsg,listZpsSegs,msg);
					listRepeatSegs=new List<SegmentHL7>();
				}
				if(msg.Segments[i].Name==SegmentNameHL7.NTE
					|| msg.Segments[i].Name==SegmentNameHL7.ZEF
					|| msg.Segments[i].Name==SegmentNameHL7.SPM)
				{
					SegmentNameHL7 prevSegName=msg.Segments[i-1].Name;
					listRepeatSegs.Add(msg.Segments[i]);
					for(int j=i+1;j<indexFirstZps;j++) {
						if(msg.Segments[j].Name!=msg.Segments[i].Name) {
							i=j-1;
							break;
						}
						listRepeatSegs.Add(msg.Segments[j]);
					}
					ProcessSeg(hl7defmsg,listRepeatSegs,msg,prevSegName);
					listRepeatSegs=new List<SegmentHL7>();//clear out list for next repeatable segment
					continue;
				}
				//if the segment is an OBX, ProcessOBX will instantiate a new MedLabResult object, one for each repetition of the OBX, ZEF, NTE group
				ProcessSeg(hl7defmsg,new List<SegmentHL7> { msg.Segments[i] },msg);
			}
			#endregion Process Order Observations
		}

		///<summary>Finds a patient from the information in the PID segment, using the PID segment definition in the enabled HL7 def.
		///Will return null if a patient cannot be found using the information in the PID segment.  If an alternate patient ID is
		///provided and the name and birthdate in the PID segment match the name and birthdate of the patient located,
		///it will be stored in the oidexternal table linked to the patient's PatNum.</summary>
		public static Patient GetPatFromPID(HL7DefSegment pidSegDef,SegmentHL7 pidSeg) {
			//PID.2 should contain the OpenDental PatNum.
			//If there is no patient with PatNum=PID.2, we will attempt to find the value in PID.4 in the oidexternals table IDExternal column
			//with root MedLabv2_3.Patient and type IdentifierType.Patient.
			//The PatNum found in the oidexternals table will only be trusted if the name and birthdate of the pat match the name and birthdate in the msg.
			//If there is no entry in the oidexternals table, we will attempt to match by name and birthdate alone.
			//If a patient is found, an entry will be made in the oidexternals table with IDExternal=PID.4, rootExternal=MedLabv2_3.Patient,
			//IDType=IdentifierType.Patient, and IDInternal=PatNum if one does not already exist.
			long patNum=0;
			long patNumFromAlt=0;
			string altPatId="";
			string patLName="";
			string patFName="";
			DateTime birthdate=DateTime.MinValue;
			//Go through fields of PID segment and get patnum, altPatNum, patient name, and birthdate to locate patient
			for(int i=0;i<pidSegDef.hl7DefFields.Count;i++) {
				HL7DefField fieldDefCur=pidSegDef.hl7DefFields[i];
				switch(fieldDefCur.FieldName) {
					case "pat.PatNum":
						try {
							patNum=PIn.Long(pidSeg.GetFieldComponent(fieldDefCur.OrdinalPos));
						}
						catch(Exception ex) {
							//do nothing, patNum will remain 0
						}
						continue;
					case "altPatId":
						altPatId=pidSeg.GetFieldComponent(fieldDefCur.OrdinalPos);
						if(altPatId=="") {
							continue;
						}
						OIDExternal oidCur=OIDExternals.GetByRootAndExtension(HL7InternalType.MedLabv2_3.ToString()+".Patient",altPatId);
						if(oidCur==null || oidCur.IDType!=IdentifierType.Patient) {
							//not in the oidexternals table or the oidexternal located is not for a patient object, patNumFromAlt will remain 0
							continue;
						}
						patNumFromAlt=oidCur.IDInternal;
						continue;
					case "pat.nameLFM":
						patLName=pidSeg.GetFieldComponent(fieldDefCur.OrdinalPos,0);
						patFName=pidSeg.GetFieldComponent(fieldDefCur.OrdinalPos,1);
						continue;
					case "patBirthdateAge":
						//LabCorp sends the birthdate and age in years, months, and days like yyyyMMdd^YYY^MM^DD
						birthdate=FieldParser.DateTimeParse(pidSeg.GetFieldComponent(fieldDefCur.OrdinalPos));
						continue;
					default:
						continue;
				}
			}
			//We now have patNum, patNumFromAlt, patLName, patFName, and birthdate so locate pat
			Patient patCur=Patients.GetPat(patNum);//will be null if not found or patNum=0
			//store the altPatId if certain criteria is met
			if(patCur!=null //found a patient
				&& altPatId!="" //the altPatId field was populated in the PID segment (usually PID.4)
				&& patNumFromAlt==0 //the altPatId is not stored in the oidexternals table as the IDExternal
				&& IsMatchNameBirthdate(patCur,patLName,patFName,birthdate)) //and the name and birthdate in the PID segment match the patient located
			{
				OIDExternal oidCur=new OIDExternal();
				oidCur.IDType=IdentifierType.Patient;
				oidCur.IDInternal=patCur.PatNum;
				oidCur.IDExternal=altPatId;
				oidCur.rootExternal=HL7InternalType.MedLabv2_3.ToString()+".Patient";
				OIDExternals.Insert(oidCur);
			}
			//patCur is null so try to find a patient from the altPatId (PID.4)
			if(patCur==null && patNumFromAlt>0) {
				patCur=Patients.GetPat(patNumFromAlt);
				//We will only trust the altPatId if the name and birthdate of the patient match the name and birthdate in the message.
				if(!IsMatchNameBirthdate(patCur,patLName,patFName,birthdate)) {
					patCur=null;
				}
			}
			//if we haven't located a patient yet, attempt with name and birthdate
			if(patCur==null && patLName!="" && patFName!="" && birthdate>DateTime.MinValue) {
				long patNumByName=Patients.GetPatNumByNameAndBirthday(patLName,patFName,birthdate);
				if(patNumByName>0) {
					patCur=Patients.GetPat(patNumByName);
				}
			}
			return patCur;
		}

		///<summary>Compare the first and last name of the patient to the string values.  The string value for lname cannot exceed 25 chars,
		///so we only compare the first 25 chars.  The string value for fname cannot exceed 15 chars, so we only compare the first 15 chars.
		///These limitations are set by LabCorp in their HL7 documentation.  If name and birthdate match, this returns true.  Otherwise false.
		///If patCur is null, returns false.</summary>
		public static bool IsMatchNameBirthdate(Patient patCur,string lname,string fname,DateTime birthdate) {
			if(patCur!=null
				&& patCur.Birthdate.Date==birthdate.Date
				&& patCur.LName.ToLower().PadRight(25).Substring(0,25)==lname.ToLower().PadRight(25).Substring(0,25)
				&& patCur.FName.ToLower().PadRight(15).Substring(0,15)==fname.ToLower().PadRight(15).Substring(0,15))
			{
				return true;
			}
			return false;
		}

		///<summary>Loops through the msgDef and finds the segment by name.  Only use if a segment can only be found in the message definition 0 or 1 time.
		///If a segment is allowed to repeat in different locations in the message, this will always return the first instance of the segment.</summary>
		public static HL7DefSegment GetSegDefByName(HL7DefMessage msgDef,SegmentNameHL7 segName) {
			HL7DefSegment segDef=null;
			for(int i=0;i<msgDef.hl7DefSegments.Count;i++) {
				if(msgDef.hl7DefSegments[i].SegmentName==segName) {
					segDef=msgDef.hl7DefSegments[i];
					break;
				}
			}
			return segDef;
		}
		
		///<summary>Returns the segment definion based on the name supplied, and only if it follows the previous segment name supplied.
		///Used when a segment can appear in a message definition more than one time in multiple locations.  For example, the NTE segment
		///can follow the PID, OBR, and OBX segments.  Each location can have repetitions, but this will return the definition that
		///immediately follows the previous segment name given.</summary>
		public static HL7DefSegment GetSegDefByName(HL7DefMessage msgDef,SegmentNameHL7 segName,SegmentNameHL7 prevSegDefName) {
			HL7DefSegment segDef=null;
			for(int i=0;i<msgDef.hl7DefSegments.Count;i++) {
				if(msgDef.hl7DefSegments[i].SegmentName==segName) {
					segDef=msgDef.hl7DefSegments[i];
					break;
				}
			}
			return segDef;
		}

		///<summary>listSegs will only contain more than one segment if the segment repeats.
		///prevSegName is only required when processing NTE segment(s) to determine which level NTE we are processing,
		///either a PID NTE, a OBR NTE, or a OBX NTE.</summary>
		public static void ProcessSeg(HL7DefMessage msgDef,List<SegmentHL7> listSegs,MessageHL7 msg,SegmentNameHL7 prevSegName=SegmentNameHL7.Unknown) {
			HL7DefSegment segDefCur=GetSegDefByName(msgDef,listSegs[0].Name);
			switch(listSegs[0].Name) {
				case SegmentNameHL7.MSH://required segment
					if(segDefCur==null) {
						throw new Exception("Could not process the MedLab HL7 message.  "
							+"The message definition for this type of message did not contain a MSH segment definition.");
					}
					ProcessMSH(segDefCur,listSegs[0],msg);//listSegs will only contain one segment, the MSH segment
					return;
				case SegmentNameHL7.NK1://optional segment
					//NK1 is currently not processed
					if(segDefCur==null) {//do not process the NK1 segment if it's not defined
						return;
					}
					ProcessNK1(segDefCur,listSegs,msg);
					return;
				case SegmentNameHL7.NTE://optional segment
					if(segDefCur==null) {//do not process the NTE segment if it's not defined
						return;
					}
					ProcessNTE(segDefCur,listSegs,msg,prevSegName);
					return;
				case SegmentNameHL7.OBR://required segment
					if(segDefCur==null) {
						throw new Exception("Could not process the MedLab HL7 message.  "
							+"The message definition for this type of message did not contain a OBR segment definition.");
					}
					ProcessOBR(segDefCur,listSegs,msg);
					return;
				case SegmentNameHL7.OBX://required segment
					if(segDefCur==null) {
						throw new Exception("Could not process the MedLab HL7 message.  "
							+"The message definition for this type of message did not contain a OBX segment definition.");
					}
					ProcessOBX(segDefCur,listSegs,msg);
					return;
				case SegmentNameHL7.ORC://required segment
					if(segDefCur==null) {
						throw new Exception("Could not process the MedLab HL7 message.  "
							+"The message definition for this type of message did not contain a ORC segment definition.");
					}
					ProcessORC(segDefCur,listSegs,msg);
					return;
				case SegmentNameHL7.PID://required segment
					if(segDefCur==null) {
						throw new Exception("Could not process the MedLab HL7 message.  "
							+"The message definition for this type of message did not contain a PID segment definition.");
					}
					ProcessPID(segDefCur,listSegs[0]);//listSegs will only contain one segment, the PID segment
					return;
				case SegmentNameHL7.SPM://optional segment
					if(segDefCur==null) {//do not process the SPM segment if it's not defined
						return;
					}
					ProcessSPM(segDefCur,listSegs,msg);
					return;
				case SegmentNameHL7.ZEF://optional segment
					if(segDefCur==null) {//do not process the ZEF segment if it's not defined
						return;
					}
					ProcessZEF(segDefCur,listSegs,msg);
					return;
				case SegmentNameHL7.ZPS://required segment
					if(segDefCur==null) {
						throw new Exception("Could not process the MedLab HL7 message.  "
							+"The message definition for this type of message did not contain a ZPS segment definition.");
					}
					ProcessZPS(segDefCur,listSegs,msg);
					return;
				default:
					return;
			}
		}

		///<summary>This will insert a new MedLab object and set _medLabCur.</summary>
		public static void ProcessMSH(HL7DefSegment segDef,SegmentHL7 mshSeg,MessageHL7 msg) {
			_medLabCur=new MedLab();
			for(int i=0;i<segDef.hl7DefFields.Count;i++) {
				HL7DefField fieldDefCur=segDef.hl7DefFields[i];
				switch(fieldDefCur.FieldName) {
					case "messageControlId":
						msg.ControlId=mshSeg.GetFieldComponent(fieldDefCur.OrdinalPos);
						continue;
					case "sendingApp":
						_medLabCur.SendingApp=mshSeg.GetFieldComponent(fieldDefCur.OrdinalPos);
						continue;
					case "sendingFacility":
						_medLabCur.SendingFacility=mshSeg.GetFieldComponent(fieldDefCur.OrdinalPos);
						continue;
					default:
						continue;
				}
			}
			MedLabs.Insert(_medLabCur);
			return;
		}
		
		///<summary>Not currently processing the NK1 segment.</summary>
		public static void ProcessNK1(HL7DefSegment segDef,List<SegmentHL7> listSegs,MessageHL7 msg) {
			return;
		}
		
		///<summary></summary>
		public static void ProcessNTE(HL7DefSegment segDef,List<SegmentHL7> listSegs,MessageHL7 msg,SegmentNameHL7 prevSegName) {
			return;
		}

		///<summary></summary>
		public static void ProcessOBR(HL7DefSegment segDef,List<SegmentHL7> listSegs,MessageHL7 msg) {
			return;
		}

		///<summary></summary>
		public static void ProcessOBX(HL7DefSegment segDef,List<SegmentHL7> listSegs,MessageHL7 msg) {
			return;
		}

		///<summary></summary>
		public static void ProcessORC(HL7DefSegment segDef,List<SegmentHL7> listSegs,MessageHL7 msg) {
			return;
		}

		///<summary>Sets values on _medLabCur based on the PID segment fields.  Also sets medlab.OriginalPIDSegment to the full PID segment.</summary>
		public static void ProcessPID(HL7DefSegment segDef,SegmentHL7 pidSeg) {
			_medLabCur.OriginalPIDSegment=pidSeg.ToString();
			for(int i=0;i<segDef.hl7DefFields.Count;i++) {
				HL7DefField fieldDefCur=segDef.hl7DefFields[i];
				switch(fieldDefCur.FieldName) {
					case "accountNum":
						_medLabCur.PatAccountNum=pidSeg.GetFieldComponent(fieldDefCur.OrdinalPos);
						string fastingStr=pidSeg.GetFieldComponent(fieldDefCur.OrdinalPos,6);
						//if the component is blank or has a value other than "Y" or "N", the PatFasting field will be 0 - Unknown
						if(fastingStr.Length>0) {
							fastingStr=fastingStr.ToLower().Substring(0,1);
							if(fastingStr=="y") {
								_medLabCur.PatFasting=YN.Yes;
							}
							else if(fastingStr=="n") {
								_medLabCur.PatFasting=YN.No;
							}
						}
						continue;
					case "altPatId":
						_medLabCur.PatIDAlt=pidSeg.GetFieldComponent(fieldDefCur.OrdinalPos);
						continue;
					case "labPatId":
						_medLabCur.PatIDLab=pidSeg.GetFieldComponent(fieldDefCur.OrdinalPos);
						continue;
					case "pat.nameLFM":
						//not currently using this to update any db fields, only used to validate the correct patient was selected
						continue;
					case "pat.PatNum":
						if(_patCur!=null) {
							//If a patient was not located, the MedLab object will have a 0 for PatNum so the user can see a list of all MedLabs
							//that were not linked to a patient so they can manually select the correct patient.
							_medLabCur.PatNum=_patCur.PatNum;
						}
						continue;
					case "patBirthdateAge":
						_medLabCur.PatAge=pidSeg.GetFieldComponent(fieldDefCur.OrdinalPos,1)+"/"+pidSeg.GetFieldComponent(fieldDefCur.OrdinalPos,2)
							+"/"+pidSeg.GetFieldComponent(fieldDefCur.OrdinalPos,3);
						continue;
					default:
						continue;
				}
			}
			return;
		}

		///<summary></summary>
		public static void ProcessSPM(HL7DefSegment segDef,List<SegmentHL7> listSegs,MessageHL7 msg) {
			return;
		}

		///<summary></summary>
		public static void ProcessZEF(HL7DefSegment segDef,List<SegmentHL7> listSegs,MessageHL7 msg) {
			return;
		}

		///<summary></summary>
		public static void ProcessZPS(HL7DefSegment segDef,List<SegmentHL7> listSegs,MessageHL7 msg) {
			return;
		}

	}
}
