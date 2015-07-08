using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeBase {
	public class ODException : Exception {
		private int _errorCode=0;

		/** Gets the error code associated to this exception.  Defaults to 0 if no error code was explicitly set. */
		public int ErrorCode {
			get {
				return _errorCode;
			}
		}

		public ODException() : this("") {
			
		}

		public ODException(int errorCode) : this("",errorCode) {
			
		}

		public ODException(string message) : this(message,0) {
			
		}

		public ODException(string message,int errorCode) : base(message) {
			_errorCode=errorCode;
		}
	}
}
