using CodeBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDentBusiness {
	public partial class ConvertDatabases {
		public static System.Version LatestVersion=new Version("16.2.0.0");//This value must be changed when a new conversion is to be triggered.

		private static void To16_2_0() {
			if(FromVersion<new Version("16.2.0.0")) {
				ODEvent.Fire(new ODEventArgs("ConvertDatabases","Upgrading database to version: 16.2.0"));//No translation in convert script.
				string command;




				command="UPDATE preference SET ValueString = '16.2.0.0' WHERE PrefName = 'DataBaseVersion'";
				Db.NonQ(command);
			}
			//To16_2_1();
		}

	}
}
