using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental.Bridges {
	public class PayConnect {

		private static PayConnectService.Credentials GetCredentials(Program prog,long clinicNum){
			PayConnectService.Credentials cred=new PayConnectService.Credentials();
			cred.Username=ProgramProperties.GetPropVal(prog.ProgramNum,"Username",clinicNum);
			cred.Password=ProgramProperties.GetPropVal(prog.ProgramNum,"Password",clinicNum);
			cred.Client="OpenDental2";
#if DEBUG
			cred.ServiceID="DCI Web Service ID: 002778";//Testing
#else
			cred.ServiceID="DCI Web Service ID: 006328";//Production
#endif
			cred.version="0310";
			return cred;
		}

		public static PayConnectService.creditCardRequest BuildSaleRequest(decimal amount,string cardNumber,int expYear,int expMonth,string nameOnCard,string securityCode,string zip,string magData,PayConnectService.transType transtype,string refNumber,bool tokenRequested,string authCode="") {
			PayConnectService.creditCardRequest request=new PayConnectService.creditCardRequest();
			request.Amount=amount;
			request.AmountSpecified=true;
			request.CardNumber=cardNumber;
			request.Expiration=new PayConnectService.expiration();
			request.Expiration.year=expYear;
			request.Expiration.month=expMonth;
			if(magData!=null) { //MagData is the data returned from magnetic card readers. Will only be present if a card was swiped.
				request.MagData=magData;
			}
			request.NameOnCard=nameOnCard;
			request.RefNumber=refNumber;
			request.SecurityCode=securityCode;
			request.TransType=transtype;
			request.Zip=zip;
			request.PaymentTokenRequested=tokenRequested;
			//request.AuthCode=authCode;//This field does not exist in the WSDL yet.  Dentalxchange will let us know once they finish adding it.
			return request;
		}

		///<summary>Shows a message box on error.</summary>
		public static PayConnectService.transResponse ProcessCreditCard(PayConnectService.creditCardRequest request,long clinicNum) {
			try {
				Program prog=Programs.GetCur(ProgramName.PayConnect);
				PayConnectService.Credentials cred=GetCredentials(prog,clinicNum);
				PayConnectService.MerchantService ms=new PayConnectService.MerchantService();
#if DEBUG
				ms.Url="https://prelive2.dentalxchange.com/merchant/MerchantService?wsdl";
#else
				ms.Url="https://webservices.dentalxchange.com/merchant/MerchantService?wsdl";
#endif
				PayConnectService.transResponse response=ms.processCreditCard(cred,request);
				ms.Dispose();
				if(response.Status.code!=0) {//Error
					MessageBox.Show(Lan.g("PayConnect","Payment failed")+". \r\n"+Lan.g("PayConnect","Error message from")+" Pay Connect: \""+response.Status.description+"\"");
				}
				return response;
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g("PayConnect","Payment failed")+". \r\n"+Lan.g("PayConnect","Error message from")+" Open Dental: \""+ex.Message+"\"");
			}
			return null;
		}

		public static bool IsValidCardAndExp(string cardNumber,int expYear,int expMonth) {
			bool isValid=false;
			try {
				PayConnectService.expiration pcExp=new PayConnectService.expiration();
				pcExp.year=expYear;
				pcExp.month=expMonth;
				PayConnectService.MerchantService ms=new PayConnectService.MerchantService();
#if DEBUG
				ms.Url="https://prelive2.dentalxchange.com/merchant/MerchantService?wsdl";
#else
				ms.Url="https://webservices.dentalxchange.com/merchant/MerchantService?wsdl";
#endif
				isValid=(ms.isValidCard(cardNumber) && ms.isValidExpiration(pcExp));
				ms.Dispose();
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g("PayConnect","Credit Card validation failed")+". \r\n"+Lan.g("PayConnect","Error message from")
					+" Open Dental: \""+ex.Message+"\"");
			}
			return isValid;
		}

	}
}
