using System;

namespace OpenDentBusiness {
	public static class WebServiceMainHQProxy {

		public static WebServiceMainHQ.WebServiceMainHQ GetWebServiceMainHQInstance() {
			WebServiceMainHQ.WebServiceMainHQ service=new WebServiceMainHQ.WebServiceMainHQ();
#if DEBUG
			//service.Url="http://localhost/OpenDentalWebServiceHQ/WebServiceMainHQ.asmx";//localhost
			service.Url="http://10.10.2.18/OpenDentalWebServiceHQ/WebServiceMainHQ.asmx";//Sam's Computer
			service.Timeout=(int)TimeSpan.FromMinutes(20).TotalMilliseconds;
#endif
			return service;
		}
	}


}
