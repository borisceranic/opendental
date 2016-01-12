using System.Collections.Generic;

namespace OpenDentBusiness{
	public interface ISignalProcessor {
		void ProcessSignals(List<Signalod> listSignals);
	}
}
