using System;
using System.Collections.Generic;
using System.Text;
using OpenDentBusiness;

namespace UnitTests {
	public class ClinicT {

		///<summary>Inserts the new clinic, refreshes the cache and then returns ClinicNum</summary>
		public static long CreateClinic(string description) {
			Clinic clinic=new Clinic();
			clinic.Description=description;
			Clinics.Insert(clinic);
			Clinics.RefreshCache();
			return clinic.ClinicNum;
		}




	}
}
