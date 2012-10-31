using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Drawing;

namespace OpenDentalWebService {
	///<summary>This file is generated automatically by the crud, do not make any changes to this file because they will get overwritten.</summary>
	public class LabCase {

		///<summary></summary>
		public static string Serialize(OpenDentBusiness.LabCase labcase) {
			StringBuilder sb=new StringBuilder();
			sb.Append("<LabCase>");
			sb.Append("<LabCaseNum>").Append(labcase.LabCaseNum).Append("</LabCaseNum>");
			sb.Append("<PatNum>").Append(labcase.PatNum).Append("</PatNum>");
			sb.Append("<LaboratoryNum>").Append(labcase.LaboratoryNum).Append("</LaboratoryNum>");
			sb.Append("<AptNum>").Append(labcase.AptNum).Append("</AptNum>");
			sb.Append("<PlannedAptNum>").Append(labcase.PlannedAptNum).Append("</PlannedAptNum>");
			sb.Append("<DateTimeDue>").Append(labcase.DateTimeDue.ToLongDateString()).Append("</DateTimeDue>");
			sb.Append("<DateTimeCreated>").Append(labcase.DateTimeCreated.ToLongDateString()).Append("</DateTimeCreated>");
			sb.Append("<DateTimeSent>").Append(labcase.DateTimeSent.ToLongDateString()).Append("</DateTimeSent>");
			sb.Append("<DateTimeRecd>").Append(labcase.DateTimeRecd.ToLongDateString()).Append("</DateTimeRecd>");
			sb.Append("<DateTimeChecked>").Append(labcase.DateTimeChecked.ToLongDateString()).Append("</DateTimeChecked>");
			sb.Append("<ProvNum>").Append(labcase.ProvNum).Append("</ProvNum>");
			sb.Append("<Instructions>").Append(SerializeStringEscapes.EscapeForXml(labcase.Instructions)).Append("</Instructions>");
			sb.Append("<LabFee>").Append(labcase.LabFee).Append("</LabFee>");
			sb.Append("</LabCase>");
			return sb.ToString();
		}

		///<summary></summary>
		public static OpenDentBusiness.LabCase Deserialize(string xml) {
			OpenDentBusiness.LabCase labcase=new OpenDentBusiness.LabCase();
			using(XmlReader reader=XmlReader.Create(new StringReader(xml))) {
				reader.MoveToContent();
				while(reader.Read()) {
					//Only detect start elements.
					if(!reader.IsStartElement()) {
						continue;
					}
					switch(reader.Name) {
						case "LabCaseNum":
							labcase.LabCaseNum=reader.ReadContentAsLong();
							break;
						case "PatNum":
							labcase.PatNum=reader.ReadContentAsLong();
							break;
						case "LaboratoryNum":
							labcase.LaboratoryNum=reader.ReadContentAsLong();
							break;
						case "AptNum":
							labcase.AptNum=reader.ReadContentAsLong();
							break;
						case "PlannedAptNum":
							labcase.PlannedAptNum=reader.ReadContentAsLong();
							break;
						case "DateTimeDue":
							labcase.DateTimeDue=DateTime.Parse(reader.ReadContentAsString());
							break;
						case "DateTimeCreated":
							labcase.DateTimeCreated=DateTime.Parse(reader.ReadContentAsString());
							break;
						case "DateTimeSent":
							labcase.DateTimeSent=DateTime.Parse(reader.ReadContentAsString());
							break;
						case "DateTimeRecd":
							labcase.DateTimeRecd=DateTime.Parse(reader.ReadContentAsString());
							break;
						case "DateTimeChecked":
							labcase.DateTimeChecked=DateTime.Parse(reader.ReadContentAsString());
							break;
						case "ProvNum":
							labcase.ProvNum=reader.ReadContentAsLong();
							break;
						case "Instructions":
							labcase.Instructions=reader.ReadContentAsString();
							break;
						case "LabFee":
							labcase.LabFee=reader.ReadContentAsDouble();
							break;
					}
				}
			}
			return labcase;
		}


	}
}