using System;

namespace OpenDentBusiness{

	///<summary>Each row is a bridge to an outside program, frequently an imaging program.  Most of the bridges are hard coded, and simply need to be enabled.  But user can also add their own custom bridge.</summary>
	[Serializable]
	public class Program:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long ProgramNum;
		///<summary>Unique name for built-in program bridges. Not user-editable. enum ProgramName</summary>
		public string ProgName;
		///<summary>Description that shows.</summary>
		public string ProgDesc;
		///<summary>True if enabled.</summary>
		public bool Enabled;
		///<summary>The path of the executable to run or file to open.</summary>
		public string Path;
		///<summary>Some programs will accept command line arguments.</summary>
		public string CommandLine;
		///<summary>Notes about this program link. Peculiarities, etc.</summary>
		public string Note;
		///<summary>If this is a Plugin, then this is the filename of the dll.  The dll must be located in the application directory.</summary>
		public string PluginDllName;

		public Program Copy(){
			return (Program)this.MemberwiseClone();
		}

	}

	///<summary>This enum is stored in the database as strings rather than as numbers, so we can do the order alphabetically and we can change it whenever we want.</summary>
	public enum ProgramName {
		None,
		Apixia,
		Apteryx,
		AudaxCeph,
		BioPAK,
		CallFire,
		Camsight,
		CaptureLink,
		Cerec,
		CliniView,
		ClioSoft,
		DBSWin,
		DemandForce,
		DentalEye,
		DentalStudio,
		DentForms,
		DentX,
		Dexis,
		Digora,
		Divvy,
		Office,
		Dolphin,
		DrCeph,
		Dxis,
		EasyNotesPro,
		eClinicalWorks,
		EvaSoft,
		EwooEZDent,
		FloridaProbe,
		Guru,
		HandyDentist,
		HouseCalls,
		IAP,
		iCat,
		ImageFX,
		iRYS,
		Lightyear,
		MediaDent,
		MiPACS,
		Mountainside,
		///<summary>electronic Rx.</summary>
		NewCrop,
		///<summary>Please use Programs.UsingOrion where possible.</summary>
		Orion,
		OrthoPlex,
		Owandy,
		PandaPerio,
		PayConnect,
		Patterson,
		PerioPal,
		Planmeca,
		PracticeWebReports,
		Progeny,
		PT,
		PTupdate,
		RayMage,
		Schick,
		Sirona,
		Sopro,
		TigerView,
		Triana,
		Trojan,
		Trophy,
		TrophyEnhanced,
		Tscan,
		UAppoint,
		Vipersoft,
		visOra,
		VistaDent,
		VixWin,
		VixWinBase36,
		VixWinBase41,
		VixWinNumbered,
		VixWinOld,
		Xcharge,
		XDR,
		XVWeb,
		ZImage
	}

	


}










