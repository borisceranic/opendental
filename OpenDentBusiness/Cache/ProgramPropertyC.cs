using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness {
	public class ProgramPropertyC {
		///<summary>A list of all program (link) properties.</summary>
		private static List<ProgramProperty> _listt;
		private static object _lockObj=new object();

		///<summary>A list of all program (link) properties.</summary>
		public static List<ProgramProperty> Listt {
			get {
				return GetListt();
			}
			set {
				lock(_lockObj) {
					_listt=value;
				}
			}
		}

		public static List<ProgramProperty> GetListt() {
			bool isListNull=false;
			lock(_lockObj) {
				if(_listt==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				ProgramProperties.RefreshCache();
			}
			List<ProgramProperty> listProgramProperties=new List<ProgramProperty>();
			lock(_lockObj) {
				for(int i=0;i<_listt.Count;i++) {
					listProgramProperties.Add(_listt[i].Copy());
				}
			}
			return listProgramProperties;
		}
		
	}
}
