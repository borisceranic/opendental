using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Collections;

namespace CodeBase {
	///<summary>Used to log messages to our internal log file, or to other resources, such as message boxes.</summary>
	public class Logger{
		///<summary>Levels of logging severity to indicate importance.</summary> 
		public enum Severity{
			NONE=0,//Must be first.
			DEBUG=1,
			INFO=2,
			WARNING=3,
			ERROR=4,
			FATAL_ERROR=5,
		};

		///<summary>The number of bytes it takes to move the current log to the backup/old log, to prevent the log files from growing infinately.</summary>
		private const int logRollByteCount=1048576;
		public static Logger openlog=new Logger(ODFileUtils.GetProgramDirectory()+"openlog.txt");
		
		private string logFile="";
		///<summary>Specifies the current logging level. Any severity less than the given level is not logged.</summary> 
#if(DEBUG)
		public Severity level=Severity.DEBUG;
#else
		public Severity level=Severity.NONE;
#endif
		#region WebCore Logger Copy
		public static int MAX_FILE_SIZE_KB=1000;
		private static Dictionary<string /*sub-directory, can be empty string (not null though)*/,object[]/*{StreamWriter, Create DateTime} the file currently linked to this sub-directory*/> _files=new Dictionary<string,object[]>();
		private static object _lock=new object();
		#endregion

		public Logger(string pLogFile){
				logFile=pLogFile;
		}

		///<summary>Convert a severity code into a string.</summary>
		public static string SeverityToString(Severity sev){
			switch(sev){
				case Severity.NONE:
					return "NONE";
				case Severity.DEBUG:
					return "DEBUG";
				case Severity.INFO:
					return "INFO";
				case Severity.WARNING:
					return "WARNING";
				case Severity.ERROR:
					return "ERROR";
				case Severity.FATAL_ERROR:
					return "FATAL ERROR";
				default:
					break;
			}
			return "UNKNOWN SEVERITY";
		}

		public static int MaxSeverityStringLength(){
			int maxlen=0;
			for(Severity sev=(Severity)1;sev<(Severity)7;sev++){
				maxlen=Math.Max(maxlen,SeverityToString(sev).Length);
			}
			return maxlen;
		}

		///<summary>Log a message from an unknown source.</summary>
		public bool Log(string message,Severity severity) {
			return Log(null,"",message,false,severity);
		}

		public bool LogMB(string message,Severity severity) {
			return Log(null,"",message,true,severity);
		}

		public bool Log(Object sender,string sendingFunctionName,string message,Severity severity) {
			return Log(sender,sendingFunctionName,message,false,severity);
		}

		public bool LogMB(Object sender,string sendingFunctionName,string message,Severity severity) {
			return Log(sender,sendingFunctionName,message,true,severity);
		}

		///<summary>Log a message to the log text file and add a description of the sender (for debugging purposes). If sender is null then a description of the sender is not printed. Always returns false so that a calling boolean function can return at the same time that it logs an error message.</summary>
		public bool Log(Object sender,string sendingFunctionName,string message,bool msgBox,Severity severity){
			if(severity>level){//Only log messages with a severity matches the current level. This will even skip message boxes.
				return false;
			}
			try{
				if(sender!=null){
					if(sendingFunctionName!=null && sendingFunctionName.Length>0){
						message=sender.ToString()+"."+sendingFunctionName+": "+message;
					}else{
						message=sender.ToString()+": "+message;
					}
				}else if(sendingFunctionName!=null && sendingFunctionName.Length>0){
					message=sendingFunctionName+": "+message;
				}
				int procId=System.Diagnostics.Process.GetCurrentProcess().Id;
				message=DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")+" "+procId.ToString().PadLeft(6,'0')+" "+
					SeverityToString(severity)+" "+message;
				if(msgBox) {
					MsgBoxCopyPaste mbox=new MsgBoxCopyPaste(message);
					mbox.ShowDialog();
				}
			}catch(Exception e){
				MessageBox.Show(e.ToString());
			}
			//File access is always exclusive, so if we cannot access the file, we can try again for a little while
			//and hope that the other process will release the file.
			bool tryagain=true;
			int numtries=0;
			while(tryagain && numtries<5){
				tryagain=false;
				numtries++;
				try{
					if(logFile!=null){
						//Ensure that the log file always exists before trying to read it.
						if(!File.Exists(logFile)){
							try{
								FileStream fs=File.Create(logFile);
								fs.Dispose();
							}catch{
							}
						}else{
							//Make the log file roll into the old log file when it reaches the roll byte size.
							System.IO.StreamReader sr=new System.IO.StreamReader(logFile);
							if(sr!=null){
								Stream st=sr.BaseStream;
								long fileLength=st.Length;
								if(fileLength>=logRollByteCount) {
									try {
										File.Copy(logFile,logFile+".old.txt");
									}catch{
									}
									try{
										File.Delete(logFile);
									}catch{
									}
									fileLength=0;
								}
								st.Dispose();
								sr.Dispose();
								if(fileLength<1){
									try{
										FileStream fs=File.Create(logFile);
										fs.Dispose();
									}catch{
									}
								}
							}
						}
						//Re-open the log file 
						System.IO.StreamWriter sw=new System.IO.StreamWriter(logFile,true);//Open the file exclusively.
						if(sw!=null) {
							sw.WriteLine(message);
							sw.Flush();
							sw.Dispose();//Close the file to allow exclusive access by other instances of OpenDental.
						}
					}
				}catch{
					tryagain=true;
				}
			}
			return false;
		}

		#region WebCore Logger Copy
		public static string GetDirectory(string subDirectory) {
			string ret = ODFileUtils.CombinePaths(AppDomain.CurrentDomain.BaseDirectory,"Logger");
			if(!string.IsNullOrEmpty(subDirectory)) {
				ret=ODFileUtils.CombinePaths(ret,subDirectory);
			}
			return ret;
		}

		public static void WriteLine(string line,string subDirectory) {
			WriteLine(line,subDirectory,false,true);
		}

		public static void WriteLine(string line,string subDirectory,bool singleFileOnly,bool includeTimestamp) {
			lock (_lock) {
				StreamWriter file = Open(subDirectory,singleFileOnly);
				if(file==null) {
					return;
				}
				string timeStamp = includeTimestamp ? (DateTime.Now.ToString()+"\t") : "";
				file.WriteLine(timeStamp+line);
			}
		}

		public static void WriteError(string line,string subDirectory) {
			WriteLine("failed - "+line,subDirectory,false,true);
		}

		public static void WriteException(Exception e,string subDirectory) {
			WriteError(e.Message+"\r\n"+e.StackTrace,subDirectory);
		}

		public static void CloseLogger() {
			lock (_lock) {
				while(_files.Count>=1) {
					IEnumerator enumerator = _files.Keys.GetEnumerator();
					if(enumerator==null||!enumerator.MoveNext()) {
						break;
					}
					CloseFile((string)enumerator.Current);
				}
			}
		}

		private static void CloseFile(string subDirectory) {
			try {
				lock (_lock) {
					StreamWriter file = null;
					DateTime created = DateTime.Now;
					if(!TryGetFile(subDirectory,out file,out created)) {
						return;
					}
					file.Dispose();
					_files.Remove(subDirectory);
				}
			}
			catch { }
		}

		public static void ParseLogs(string directory) {
			try {
				DirectoryInfo di = new DirectoryInfo(directory);
				if(!di.Exists) {
					return;
				}
				using(StreamWriter sw = new StreamWriter(ODFileUtils.CombinePaths(AppDomain.CurrentDomain.BaseDirectory,"Errors - "+DateTime.Now.ToString("MM-dd-yy HH-mm-ss")+".txt"))) {
					ParseDirectory(di,sw);
					foreach(DirectoryInfo diSub in di.GetDirectories()) {
						ParseDirectory(diSub,sw);
					}
				}
			}
			catch(Exception e) {
				throw e;
			}
		}

		private static void ParseDirectory(DirectoryInfo di,StreamWriter sw) {
			FileInfo[] files = di.GetFiles("*.txt");
			foreach(FileInfo fi in files) {
				using(StreamReader sr = new StreamReader(fi.FullName)) {
					string line = "";
					string lower = "";
					while(sr.Peek()>0) {
						line=sr.ReadLine();
						lower=line.ToLower();
						if(!lower.Contains("failed")) {
							continue;
						}
						sw.WriteLine(line);
					}
				}
			}
		}

		public static bool SingleFileLoggerExists(string subDirectory) {
			FileInfo fi = new FileInfo(ODFileUtils.CombinePaths(GetDirectory(subDirectory),subDirectory+".txt"));
			return fi.Exists;
		}

		private static bool TryGetFile(string subDirectory,out StreamWriter file,out DateTime created) {
			file=null;
			created=DateTime.MinValue;
			lock (_lock) {
				object[] obj = null;
				if(!_files.TryGetValue(subDirectory,out obj)) {
					return false;
				}
				file=(StreamWriter)obj[0];
				created=(DateTime)obj[1];
				return true;
			}
		}

		private static StreamWriter Open(string subDirectory,bool singleFileOnly) {
			try {
				lock (_lock) {
					StreamWriter file = null;
					DateTime created = DateTime.MinValue;
					if(TryGetFile(subDirectory,out file,out created)) { //file has been created
						if(singleFileOnly) {
							return file;
						}
						if(DateTime.Today==created.Date) { //it was created today
							if((file.BaseStream.Length/1024)<=MAX_FILE_SIZE_KB) { //it is within the acceptable size limit
								return file;
							}
						}
						CloseFile(subDirectory);
					}
					file=new StreamWriter(GetFileName(subDirectory,DateTime.Today,singleFileOnly),true);
					file.AutoFlush=true;
					_files[subDirectory]=new object[] { file,DateTime.Now };
					return file;
				}
			}
			catch {
				return null;
			}
		}

		private static string GetFileNameSingleFileOnly(string subDirectory) {
			DirectoryInfo di = new DirectoryInfo(GetDirectory(subDirectory));
			FileInfo fi = new FileInfo(ODFileUtils.CombinePaths(di.FullName,subDirectory+".txt"));
			if(!di.Exists) {
				di.Create();
			}
			return fi.FullName;
		}

		private static string GetFileName(string subDirectory,DateTime date,bool singleFileOnly) {
			if(singleFileOnly) {
				return GetFileNameSingleFileOnly(subDirectory);
			}
			string formattedDate = date.ToString("yy-MM-dd");
			DirectoryInfo di = new DirectoryInfo(ODFileUtils.CombinePaths(GetDirectory(subDirectory),formattedDate));
			if(!di.Exists) {
				di.Create();
			}
			int fileNum = 1;
			do {
				FileInfo fi = new FileInfo(ODFileUtils.CombinePaths(di.FullName,formattedDate+" ("+fileNum.ToString("D3")+").txt"));
				if(!fi.Exists) { //file doesn't exist yet
					return fi.FullName;
				}
				if((fi.Length/1024)<=MAX_FILE_SIZE_KB) { //file is small enough to use
					return fi.FullName;
				}
				if(++fileNum>=1000) { //only create 1000 files max
					List<FileInfo> fileInfos = new List<FileInfo>(di.GetFiles(formattedDate+"*"));
					fileInfos.Sort(SortFileByModifiedTimeDesc);
					fileInfos[0].Delete();
					return fileInfos[0].FullName;
				}
			} while(true);
		}

		private static int SortFileByModifiedTimeDesc(FileInfo x,FileInfo y) {
			return x.LastWriteTime.CompareTo(y.LastWriteTime);
		} 
		#endregion

	}
}
