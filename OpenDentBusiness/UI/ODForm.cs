using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace OpenDentBusiness {
	public class ODForm:Form {
		///<summary>Globally unique process ID even between two workstations.</summary>
		public static string ProcessID;
		///<summary>Globally unique id for each instance so that this instance doesn't get "reprocessed"</summary>
		public string FormID;
		///<summary>Globally accessible list of open forms that implement this base form class.</summary>
		private static List<ODForm> _listOpenForms=new List<ODForm>();

		public ODForm() {
			if(string.IsNullOrEmpty(ProcessID)) {
				ProcessID=Guid.NewGuid().ToString();
			}
			FormID=Guid.NewGuid().ToString();
			_listOpenForms.Add(this);
			this.FormClosed+=BaseFormClosed;
		}

		private void BaseFormClosed(object sender,FormClosedEventArgs e) {
			_listOpenForms.Remove(this);
		}
		
		///<summary>Calls ProcessSignal for each signal on each open base form.</summary>
		public static void ProcessAllSignals(List<Signalod> listSignals) {
			if(listSignals==null || listSignals.Count==0) {
				return;
			}
			foreach(ODForm form in _listOpenForms.Where(x => x!=null && !x.IsDisposed)) {
				foreach(Signalod signal in listSignals) {
					List<string> fromArgs=signal.FromUser.Split(';').ToList();
					string processID="";
					string formID="";
					string objectType="";
					long fKey=0;
					if(fromArgs.Count==4) {
						processID=fromArgs[0];
						formID=fromArgs[1];
						objectType=fromArgs[2];
						fKey=PIn.Long(fromArgs[3]);
					}
					if(processID==ODForm.ProcessID) {
						continue;
					}
					if(formID==form.FormID) {
						continue;
					}
					List<InvalidType> listITypes=signal.ITypes.Split(',').Select(x => (InvalidType)PIn.Int(x)).ToList();
					foreach(InvalidType invalidType in listITypes) {
						form.ProcessSignal(signal,invalidType,processID,formID,objectType,fKey);
					}
				}
			}
		}

		protected virtual void ProcessSignal(Signalod signal,InvalidType invalidType,string processID, string formID, string objectType, long fKey) {
			//implement this in inherited form.
		}



	}
}