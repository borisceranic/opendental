using System;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness{
	public class SheetFieldsAvailable {
		///<Summary>This is the list of input or output fieldnames for user to pick from.  The only two options are input or output.</Summary>
		public static List<SheetFieldDef> GetList(SheetTypeEnum sheetType,bool isOutput){
			switch(sheetType){
				case SheetTypeEnum.LabelPatient:
					return GetLabelPatient(isOutput);
				case SheetTypeEnum.LabelCarrier:
					return GetLabelCarrier(isOutput);
				case SheetTypeEnum.LabelReferral:
					return GetLabelReferral(isOutput);
				case SheetTypeEnum.ReferralSlip:
					return GetReferralSlip(isOutput);
			}
			return new List<SheetFieldDef>();
		}

		private static SheetFieldDef NewOutput(string fieldName){
			return new SheetFieldDef(SheetFieldType.OutputText,fieldName,"",0,"",false,0,0,0,0,GrowthBehaviorEnum.None);
		}

		private static SheetFieldDef NewInput(string fieldName){
			return new SheetFieldDef(SheetFieldType.InputField,fieldName,"",0,"",false,0,0,0,0,GrowthBehaviorEnum.None);
		}

		private static List<SheetFieldDef> GetLabelPatient(bool isOutput){
			List<SheetFieldDef> list=new List<SheetFieldDef>();
			if(isOutput){
				list.Add(NewOutput("nameFL"));
				list.Add(NewOutput("nameLF"));
				list.Add(NewOutput("address"));//includes address2
				list.Add(NewOutput("cityStateZip"));
				list.Add(NewOutput("ChartNumber"));
				list.Add(NewOutput("PatNum"));
				list.Add(NewOutput("dateTime.Today"));
				list.Add(NewOutput("birthdate"));
				list.Add(NewOutput("priProvName"));
			}
			else{

			}
			return list;
		}

		private static List<SheetFieldDef> GetLabelCarrier(bool isOutput) {
			List<SheetFieldDef> list=new List<SheetFieldDef>();
			if(isOutput){
				list.Add(NewOutput("CarrierName"));
				list.Add(NewOutput("address"));//includes address2
				list.Add(NewOutput("cityStateZip"));
			}
			else{

			}
			return list;
		}

		private static List<SheetFieldDef> GetLabelReferral(bool isOutput) {
			List<SheetFieldDef> list=new List<SheetFieldDef>();
			if(isOutput){
				list.Add(NewOutput("nameFL"));//includes Title
				list.Add(NewOutput("address"));//includes address2
				list.Add(NewOutput("cityStateZip"));
			}
			else{

			}
			return list;
		}

		private static List<SheetFieldDef> GetReferralSlip(bool isOutput) {
			List<SheetFieldDef> list=new List<SheetFieldDef>();
			if(isOutput){
				list.Add(NewOutput("referral.nameFL"));
				list.Add(NewOutput("referral.address"));
				list.Add(NewOutput("referral.cityStateZip"));
				list.Add(NewOutput("patient.nameFL"));
				list.Add(NewOutput("dateTime.Today"));
				list.Add(NewOutput("patient.WkPhone"));
				list.Add(NewOutput("patient.HmPhone"));
				list.Add(NewOutput("patient.WirelessPhone"));
				list.Add(NewOutput("patient.address"));
				list.Add(NewOutput("patient.cityStateZip"));
				list.Add(NewOutput("patient.provider"));
			}
			else{
				list.Add(NewInput("notes"));
			}
			return list;
		}





	}

}
