using System;

namespace CodeBase {
	public class ODException:Exception {
		private int _errorCode=0;

		///<summary>Gets the error code associated to this exception.  Defaults to 0 if no error code was explicitly set.</summary>		
		public int ErrorCode {
			get {
				return _errorCode;
			}
		}

		///<summary>Convert an int to an Enum typed ErrorCode. Returns NotDefined if the input errorCode is not defined in ErrorCodes.</summary>		
		public static ErrorCodes GetErrorCodeAsEnum(int errorCode) {
			if(!Enum.IsDefined(typeof(ErrorCodes),errorCode)) {
				return ErrorCodes.NotDefined;
			}
			return (ErrorCodes)errorCode;
		}

		///<summary>Gets the pre-defined error code associated to this exception.  
		///Defaults to NotDefined if the error code (int) specified is not defined in ErrorCodes enum.</summary>		
		public ErrorCodes ErrorCodeAsEnum {
			get {
				return GetErrorCodeAsEnum(_errorCode);
			}
		}

		public ODException() { }

		public ODException(int errorCode) : this("",errorCode) { }

		public ODException(string message) : this(message,0) { }

		public ODException(string message,ErrorCodes errorCodeAsEnum) : this(message,(int)errorCodeAsEnum) { }

		public ODException(string message,int errorCode)
			: base(message) {
			_errorCode=errorCode;
		}

		///<summary>Predefined ODException.ErrorCode field values. ErrorCode field is not limited to these values but this is a convenient way defined known error types.
		///These values must be converted to/from int in order to be stored in ODException.ErrorCode.
		///Number ranges are arbitrary but should reserve plenty of padding for the future of a given range.
		///Each number range should share a similar prefix between all of it's elements.</summary>
		public enum ErrorCodes {
			///<summary>0 is the default. If the given (int) ErrorCode is not defined here, it will be returned at 0 - NotDefined.</summary>
			NotDefined=0,
			//100-199 range. Values used by ODSocket architecture.
			///<summary>No immortal socket connection found for this RegistrationKeyNum. 
			///The Proxy is trying to communicate with this eConnector but the eConnector does not have an active connection.</summary>		
			ODSocketNotFoundForRegKeyNum=100,
			///<summary>Immortal socket connection was found by Proxy but the remote eConnector socket is not responding. 
			///Most likely because the eConnector has been turned off but the Proxy has not performed an ACK to discover that it's off.</summary>		
			ODSocketEConnectorNotResponding=101,
		}
	}
}
