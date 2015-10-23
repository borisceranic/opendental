using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	///<summary></summary>
	public class ClearinghouseL {

		///<summary>Returns the clearinghouse specified by the given num.  Will only return an HQ-level clearinghouse.
		///Do not attempt to pass in a clinic-level clearinghouseNum.</summary>
		public static Clearinghouse GetClearinghouseHq(long hqClearinghouseNum) {
			return GetClearinghouseHq(hqClearinghouseNum,false);
		}

		///<summary>Returns the clearinghouse specified by the given num.  Will only return an HQ-level clearinghouse.
		///Do not attempt to pass in a clinic-level clearinghouseNum.</summary>
		public static Clearinghouse GetClearinghouseHq(long hqClearinghouseNum,bool suppressError) {
			Clearinghouse[] arrayClearinghouses=Clearinghouses.GetHqListt();
			for(int i=0;i<arrayClearinghouses.Length;i++){
				if(arrayClearinghouses[i].ClearinghouseNum==hqClearinghouseNum) {
					return arrayClearinghouses[i];
				}
			}
			if(!suppressError) {
				MessageBox.Show("Error. Could not locate Clearinghouse.");
			}
			return null;
		}

		///<summary>Gets the index of this clearinghouse within List</summary>
		public static int GetIndex(long clearinghouseNum) {
			Clearinghouse[] arrayClearinghouses=Clearinghouses.GetHqListt();
			for(int i=0;i<arrayClearinghouses.Length;i++) {
				if(arrayClearinghouses[i].ClearinghouseNum==clearinghouseNum) {
					return i;
				}
			}
			MessageBox.Show("Clearinghouses.GetIndex failed.");
			return -1;
		}

		///<summary></summary>
		public static string GetDescript(long clearinghouseNum) {
			if(clearinghouseNum==0) {
				return "";
			}
			return GetClearinghouseHq(clearinghouseNum).Description;
		}

	}
}