using System;
using System.Collections.Generic;
using System.Text;
using OpenDentBusiness;

namespace UnitTests {
	public class ProviderT {

		///<summary>Inserts the new provider, refreshes the cache and then returns ProvNum</summary>
		public static long CreateProvider(string abbr,string fName="",string lName="",long feeSchedNum=0) {
			Provider prov=new Provider();
			prov.Abbr=abbr;
			prov.FName=fName;
			prov.LName=lName;
			prov.FeeSched=0;
			Providers.Insert(prov);
			Providers.RefreshCache();
			return prov.ProvNum;
		}




	}
}
