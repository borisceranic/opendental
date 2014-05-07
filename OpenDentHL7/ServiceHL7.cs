using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using CodeBase;
using OpenDentBusiness;
using OpenDentBusiness.HL7;

namespace OpenDentHL7 {
	///<summary>State object for reading client data asynchronously</summary>
	public class StateObject {
		//Client socket
		public Socket workSocket=null;
		//Size of receive buffer
		public const int BufferSize=256;
		//Receive buffer
		public byte[] buffer=new byte[BufferSize];
		//Received data string
		public StringBuilder strbFullMsg=new StringBuilder();
	}

	public partial class ServiceHL7:ServiceBase {
		private bool IsVerboseLogging;
		private System.Threading.Timer timerSendFiles;
		private System.Threading.Timer timerReceiveFiles;
		private System.Threading.Timer timerSendTCP;
		private System.Threading.Timer timerSendConnectTCP;
		private Socket socketIncomingMain;
		private string hl7FolderIn;
		private string hl7FolderOut;
		private static bool isReceivingFiles;
		private const char MLLP_START_CHAR=(char)11;// HEX 0B
		private const char MLLP_END_CHAR=(char)28;// HEX 1C
		private const char MLLP_ENDMSG_CHAR=(char)13;// HEX 0D
		private const char MLLP_ACK_CHAR=(char)6;// HEX 06
		private const char MLLP_NACK_CHAR=(char)21;// HEX 15
		private HL7Def HL7DefEnabled;
		private static bool IsSendTCPConnected;
		private static bool _ecwFileModeIsSending;
		private static DateTime _ecwDateTimeOldMsgsDeleted;
		private static bool _ecwTCPModeIsSending;
		private static bool _ecwTCPSendSocketIsConnected;
		// ManualResetEvent instances signal completion.
		private static ManualResetEvent connectDone=new ManualResetEvent(false);
		private static ManualResetEvent sendDone=new ManualResetEvent(false);

		public ServiceHL7() {
			InitializeComponent();
			CanStop = true;
			IsSendTCPConnected=true;//bool to keep track of whether connection attempts have failed in the past. At startup, the 'previous' attempt is assumed successful.
			EventLog.WriteEntry("OpenDentHL7",DateTime.Now.ToLongTimeString()+" - Initialized.");
		}

		protected override void OnStart(string[] args) {
			StartManually();
		}

		public void StartManually() {
			//connect to OD db.
			XmlDocument document=new XmlDocument();
			string pathXml=Path.Combine(Application.StartupPath,"FreeDentalConfig.xml");
			try{
				document.Load(pathXml);
			}
			catch{
				EventLog.WriteEntry("OpenDentHL7",DateTime.Now.ToLongTimeString()+" - Could not find "+pathXml,EventLogEntryType.Error);
				throw new ApplicationException("Could not find "+pathXml);
			}
			XPathNavigator Navigator=document.CreateNavigator();
			XPathNavigator nav;
			DataConnection.DBtype=DatabaseType.MySql;
			nav=Navigator.SelectSingleNode("//DatabaseConnection");
			string computerName=nav.SelectSingleNode("ComputerName").Value;
			string database=nav.SelectSingleNode("Database").Value;
			string user=nav.SelectSingleNode("User").Value;
			string password=nav.SelectSingleNode("Password").Value;
			XPathNavigator verboseNav=Navigator.SelectSingleNode("//HL7verbose");
			if(verboseNav!=null && verboseNav.Value=="True") {
				IsVerboseLogging=true;
				EventLog.WriteEntry("OpenDentHL7","Verbose mode.",EventLogEntryType.Information);
			}
			OpenDentBusiness.DataConnection dcon=new OpenDentBusiness.DataConnection();
			//Try to connect to the database directly
			try {
				dcon.SetDb(computerName,database,user,password,"","",DataConnection.DBtype);
				//a direct connection does not utilize lower privileges.
				RemotingClient.RemotingRole=RemotingRole.ClientDirect;
			}
			catch {//(Exception ex){
				throw new ApplicationException("Connection to database failed.");
			}
			//check db version
			string dbVersion=PrefC.GetString(PrefName.ProgramVersion);
			if(Application.ProductVersion.ToString() != dbVersion) {
				EventLog.WriteEntry("OpenDentHL7","Versions do not match.  Db version:"+dbVersion+".  Application version:"+Application.ProductVersion.ToString(),EventLogEntryType.Error);
				throw new ApplicationException("Versions do not match.  Db version:"+dbVersion+".  Application version:"+Application.ProductVersion.ToString());
			}
			if(Programs.IsEnabled(ProgramName.eClinicalWorks) && !HL7Defs.IsExistingHL7Enabled()) {//eCW enabled, and no HL7def enabled.
				//prevent startup:
				long progNum=Programs.GetProgramNum(ProgramName.eClinicalWorks);
				string hl7Server=ProgramProperties.GetPropVal(progNum,"HL7Server");
				string hl7ServiceName=ProgramProperties.GetPropVal(progNum,"HL7ServiceName");
				if(hl7Server=="") {//for the first time run
					ProgramProperties.SetProperty(progNum,"HL7Server",System.Environment.MachineName);
					hl7Server=System.Environment.MachineName;
				}
				if(hl7ServiceName=="") {//for the first time run
					ProgramProperties.SetProperty(progNum,"HL7ServiceName",this.ServiceName);
					hl7ServiceName=this.ServiceName;
				}
				if(hl7Server.ToLower()!=System.Environment.MachineName.ToLower()) {
					EventLog.WriteEntry("OpenDentHL7","The HL7 Server name does not match the name set in Program Links eClinicalWorks Setup.  Server name: "+System.Environment.MachineName
						+", Server name in Program Links: "+hl7Server,EventLogEntryType.Error);
					throw new ApplicationException("The HL7 Server name does not match the name set in Program Links eClinicalWorks Setup.  Server name: "+System.Environment.MachineName
						+", Server name in Program Links: "+hl7Server);
				}
				if(hl7ServiceName.ToLower()!=this.ServiceName.ToLower()) {
					EventLog.WriteEntry("OpenDentHL7","The HL7 Service Name does not match the name set in Program Links eClinicalWorks Setup.  Service name: "+this.ServiceName+", Service name in Program Links: "
						+hl7ServiceName,EventLogEntryType.Error);
					throw new ApplicationException("The HL7 Service Name does not match the name set in Program Links eClinicalWorks Setup.  Service name: "+this.ServiceName+", Service name in Program Links: "
						+hl7ServiceName);
				}
				EcwOldSendAndReceive();
				return;
			}
			HL7Def hL7Def=HL7Defs.GetOneDeepEnabled();
			if(hL7Def==null) {
				return;
			}
			if(hL7Def.HL7Server=="") {
				hL7Def.HL7Server=System.Environment.MachineName;
				HL7Defs.Update(hL7Def);
			}
			if(hL7Def.HL7ServiceName=="") {
				hL7Def.HL7ServiceName=this.ServiceName;
				HL7Defs.Update(hL7Def);
			}
			if(hL7Def.HL7Server.ToLower()!=System.Environment.MachineName.ToLower()) {
				EventLog.WriteEntry("OpenDentHL7","The HL7 Server name does not match the name in the enabled HL7Def Setup.  Server name: "+System.Environment.MachineName+", Server name in HL7Def: "+hL7Def.HL7Server,
					EventLogEntryType.Error);
				throw new ApplicationException("The HL7 Server name does not match the name in the enabled HL7Def Setup.  Server name: "+System.Environment.MachineName+", Server name in HL7Def: "+hL7Def.HL7Server);
			}
			if(hL7Def.HL7ServiceName.ToLower()!=this.ServiceName.ToLower()) {
				EventLog.WriteEntry("OpenDentHL7","The HL7 Service Name does not match the name in the enabled HL7Def Setup.  Service name: "+this.ServiceName+", Service name in HL7Def: "+hL7Def.HL7ServiceName,
					EventLogEntryType.Error);
				throw new ApplicationException("The HL7 Service Name does not match the name in the enabled HL7Def Setup.  Service name: "+this.ServiceName+", Service name in HL7Def: "+hL7Def.HL7ServiceName);
			}
			HL7DefEnabled=hL7Def;//so we can access it later from other methods
			if(HL7DefEnabled.ModeTx==ModeTxHL7.File) {
				hl7FolderOut=HL7DefEnabled.OutgoingFolder;
				hl7FolderIn=HL7DefEnabled.IncomingFolder;
				if(!Directory.Exists(hl7FolderOut)) {
					EventLog.WriteEntry("OpenDentHL7","The outgoing HL7 folder does not exist.  Path is set to: "+hl7FolderOut,EventLogEntryType.Error);
					throw new ApplicationException("The outgoing HL7 folder does not exist.  Path is set to: "+hl7FolderOut);
				}
				if(!Directory.Exists(hl7FolderIn)) {
					EventLog.WriteEntry("OpenDentHL7","The incoming HL7 folder does not exist.  Path is set to: "+hl7FolderIn,EventLogEntryType.Error);
					throw new ApplicationException("The incoming HL7 folder does not exist.  Path is set to: "+hl7FolderIn);
				}
				_ecwDateTimeOldMsgsDeleted=DateTime.MinValue;
				_ecwFileModeIsSending=false;
				//start polling the folder for waiting messages to import.  Every 5 seconds.
				TimerCallback timercallbackReceive=new TimerCallback(TimerCallbackReceiveFiles);
				timerReceiveFiles=new System.Threading.Timer(timercallbackReceive,null,5000,5000);
				//start polling the db for new HL7 messages to send. Every 1.8 seconds.
				TimerCallback timercallbackSend=new TimerCallback(TimerCallbackSendFiles);
				timerSendFiles=new System.Threading.Timer(timercallbackSend,null,1800,1800);
			}
			else {//TCP/IP
				CreateIncomingTcpListener();
				_ecwTCPSendSocketIsConnected=false;
				_ecwTCPModeIsSending=false;
				//start a timer to connect to the send socket every 20 seconds.  The socket will be reused, so if _ecwTCPSendSocketIsConnected is true, this will just return
				TimerCallback timercallbackSendConnectTCP=new TimerCallback(TimerCallbackSendConnectTCP);
				timerSendConnectTCP=new System.Threading.Timer(timercallbackSendConnectTCP,null,1800,20000);//every 20 seconds, re-connect to the socket if the connection has been closed
			}
		}

		private void TimerCallbackReceiveFiles(Object stateInfo) {
			//process all waiting messages
			if(isReceivingFiles) {
				return;//already in the middle of processing files
			}
			isReceivingFiles=true;
			string[] existingFiles=Directory.GetFiles(hl7FolderIn);
			for(int i=0;i<existingFiles.Length;i++) {
				ProcessMessageFile(existingFiles[i]);
			}
			isReceivingFiles=false;
		}

		private void ProcessMessageFile(string fullPath) {
			string msgtext="";
			int i=0;
			while(i<5) {
				try {
					msgtext=File.ReadAllText(fullPath);
					break;
				}
				catch {
				}
				Thread.Sleep(200);
				i++;
				if(i==5) {
					EventLog.WriteEntry("Could not read text from file due to file locking issues.",EventLogEntryType.Error);
					return;
				}
			}
			try {
				MessageHL7 msg=new MessageHL7(msgtext);//this creates an entire heirarchy of objects.
				MessageParser.Process(msg,IsVerboseLogging);
				if(IsVerboseLogging) {
					EventLog.WriteEntry("OpenDentHL7","Processed message "+msg.MsgType.ToString(),EventLogEntryType.Information);
				}
			}
			catch(Exception ex) {
				EventLog.WriteEntry(ex.Message+"\r\n"+ex.StackTrace,EventLogEntryType.Error);
				return;
			}
			try {
				File.Delete(fullPath);
			}
			catch(Exception ex) {
				EventLog.WriteEntry("Delete failed for "+fullPath+"\r\n"+ex.Message,EventLogEntryType.Error);
			}
		}
		
		protected override void OnStop() {
			//later: inform od via signal that this service has shut down
			EcwOldStop();
			if(timerSendFiles!=null) {
				timerSendFiles.Dispose();
			}
		}

		private void TimerCallbackSendFiles(Object stateInfo) {
			if(_ecwFileModeIsSending) {//if there is a thread that is still sending, return
				return;
			}
			try {
				_ecwFileModeIsSending=true;
				if(IsVerboseLogging) {
					EventLog.WriteEntry("GetOnePending Start");
				}
				List<HL7Msg> list=HL7Msgs.GetOnePending();
				if(IsVerboseLogging) {
					EventLog.WriteEntry("GetOnePending Finished");
				}
				string filename;
				for(int i=0;i<list.Count;i++) {//Right now, there will only be 0 or 1 item in the list.
					filename=ODFileUtils.CreateRandomFile(hl7FolderOut,".txt");
					File.WriteAllText(filename,list[i].MsgText);
					list[i].HL7Status=HL7MessageStatus.OutSent;
					HL7Msgs.Update(list[i]);//set the status to sent.
				}
				if(_ecwDateTimeOldMsgsDeleted.Date<DateTime.Now.Date) {
					if(IsVerboseLogging) {
						EventLog.WriteEntry("DeleteOldMsgText Starting");
					}
					_ecwDateTimeOldMsgsDeleted=DateTime.Now;//If DeleteOldMsgText fails for any reason.  This will cause it to not get called until the next day instead of with every msg.
					HL7Msgs.DeleteOldMsgText();//this function deletes if DateTStamp is less than CURDATE-INTERVAL 4 MONTH.  That means it will delete message text only once a day, not time based.
					if(IsVerboseLogging) {
						EventLog.WriteEntry("DeleteOldMsgText Finished");
					}
				}
			}
			catch(Exception ex) {
				EventLog.WriteEntry("OpenDentHL7 error when sending HL7 message: "+ex.Message,EventLogEntryType.Warning);//Warning because the service will spawn a new thread in 1.8 seconds
			}
			finally {
				_ecwFileModeIsSending=false;
			}
		}

		private void CreateIncomingTcpListener() {
				//Use Minimal Lower Layer Protocol (MLLP):
				//To send a message:              StartBlockChar(11) -          Payload            - EndBlockChar(28) - EndDataChar(13).
				//An ack message looks like this: StartBlockChar(11) - AckChar(0x06)/NakChar(0x15) - EndBlockChar(28) - EndDataChar(13).
				//Ack is part of MLLP V2.  In it, every message requires an ack or nak.  It's unclear when a nak would be useful.
				//Also in V2, every incoming message must be persisted by storing in our db.
				//We will just start with version 1 and not do acks at first unless needed.
			try {
				socketIncomingMain=new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
				IPEndPoint endpointLocal=new IPEndPoint(IPAddress.Any,int.Parse(HL7DefEnabled.IncomingPort));
				//socketIncomingMain=new Socket(AddressFamily.InterNetworkV6,SocketType.Stream,ProtocolType.Tcp);
				//socketIncomingMain.SetSocketOption(SocketOptionLevel.IPv6,SocketOptionName.IPv6Only,false);
				////IPAddress ipaddress = Dns.GetHostAddresses(Dns.GetHostName())[0];
				//IPEndPoint endpointLocal=new IPEndPoint(IPAddress.IPv6Any,int.Parse(HL7DefEnabled.IncomingPort));
				socketIncomingMain.Bind(endpointLocal);
				socketIncomingMain.Listen(1);//Listen for and queue incoming connection requests.  There should only be one.
				if(IsVerboseLogging) {
					EventLog.WriteEntry("OpenDentHL7","Listening",EventLogEntryType.Information);
				}
				//Asynchronously process incoming connection attempts:
				socketIncomingMain.BeginAccept(new AsyncCallback(OnConnectionAccepted),socketIncomingMain);
			}
			catch(Exception ex) {
				EventLog.WriteEntry("OpenDentHL7","Error creating incoming TCP listener\r\n"+ex.Message+"\r\n"+ex.StackTrace,EventLogEntryType.Error);
				throw ex;
				//service will stop working at this point.
			}
		}

		///<summary>Runs in a separate thread</summary>
		private void OnConnectionAccepted(IAsyncResult asyncResult) {
			if(IsVerboseLogging) {
				EventLog.WriteEntry("OpenDentHL7","Connection Accepted",EventLogEntryType.Information);
			}
			try {
				Socket socketIncomingHandler=socketIncomingMain.EndAccept(asyncResult);//end the BeginAccept.  Get reference to new Socket.
				//Use the worker socket to wait for data.
				//This is very short for testing.  Once we are confident in splicing together multiple chunks, lengthen this.
				//dataBufferIncoming=new byte[8];
				//strbFullMsg=new StringBuilder();
				//We will keep reusing the same workerSocket instead of maintaining a list of worker sockets
				//because this program is guaranteed to only have one incoming connection at a time.
				if(IsVerboseLogging) {
					EventLog.WriteEntry("OpenDentHL7","BeginReceive",EventLogEntryType.Information);
				}
				StateObject state=new StateObject();
				state.workSocket=socketIncomingHandler;
				socketIncomingHandler.BeginReceive(state.buffer,0,StateObject.BufferSize,SocketFlags.None,new AsyncCallback(OnDataReceived),state);
				//socketIncomingWorker.BeginReceive(dataBufferIncoming,0,dataBufferIncoming.Length,SocketFlags.None,new AsyncCallback(OnDataReceived),null);
				//the main socket is now free to wait for another connection.
				socketIncomingMain.BeginAccept(new AsyncCallback(OnConnectionAccepted),socketIncomingMain);
			}
			catch(ObjectDisposedException){      
				//Socket has been closed.  Try to start over.
				CreateIncomingTcpListener();//If this fails, service stops running
			}   
			catch(Exception ex){      
				//not sure what went wrong.
				EventLog.WriteEntry("OpenDentHL7","Error in OnConnectionAccpeted:\r\n"+ex.Message+"\r\n"+ex.StackTrace,EventLogEntryType.Error);
				throw;//service will stop working at this point.
			}
		}

		///<summary>Runs in a separate thread</summary>
		private void OnDataReceived(IAsyncResult asyncResult) {
			StateObject state=(StateObject)asyncResult.AsyncState;
			if(state==null) {
				EventLog.WriteEntry("OpenDentalHL7","Error in OnDataReceived: The IAsyncResult parameter could not be cast to a StateObject.",EventLogEntryType.Error);
				return;
			}
			Socket socketIncomingHandler=state.workSocket;
			int byteCountReceived=0;
			try {
				byteCountReceived=socketIncomingHandler.EndReceive(asyncResult);//blocks until data is recieved.
			}
			catch(Exception ex) {
				//Socket has been disposed or is null or something went wrong.
				EventLog.WriteEntry("OpenDentalHL7","Error in OnDataReceived:\r\n"+ex.Message+"\r\n"+ex.StackTrace,EventLogEntryType.Error);
				return;
			}
			char[] chars=new char[byteCountReceived];
			Decoder decoder=Encoding.UTF8.GetDecoder();
			decoder.GetChars(state.buffer,0,byteCountReceived,chars,0);//doesn't necessarily get all bytes from the buffer because buffer could be half full.
			state.strbFullMsg.Append(chars);//sb might already have partial data
			Array.Clear(state.buffer,0,StateObject.BufferSize);//Clear the buffer, ready to receive more
			//I think we are guaranteed to have received at least one char.
			bool isFullMsg=false;
			bool isMalformed=false;
			if(state.strbFullMsg.Length==1 && state.strbFullMsg[0]==MLLP_ENDMSG_CHAR){//the only char in the message is the end char
				state.strbFullMsg.Clear();//this must be the very end of a previously processed message.  Discard.
				isFullMsg=false;
			}
			//else if(strbFullMsg[0]!=MLLP_START_CHAR) {
			else if(state.strbFullMsg.Length>0 && state.strbFullMsg[0]!=MLLP_START_CHAR) {
				//Malformed message. 
				isFullMsg=true;//we're going to do this so that the error gets saved in the database further down.
				isMalformed=true;
			}
			else if(state.strbFullMsg.Length>=3//so that the next two lines won't crash
				&& state.strbFullMsg[state.strbFullMsg.Length-1]==MLLP_ENDMSG_CHAR//last char is the endmsg char.
				&& state.strbFullMsg[state.strbFullMsg.Length-2]==MLLP_END_CHAR)//the second-to-the-last char is the end char.
			{
				//we have a complete message
				state.strbFullMsg.Remove(0,1);//strip off the start char
				state.strbFullMsg.Remove(state.strbFullMsg.Length-2,2);//strip off the end chars
				isFullMsg=true;
			}
			else if(state.strbFullMsg.Length>=2//so that the next line won't crash
				&& state.strbFullMsg[state.strbFullMsg.Length-1]==MLLP_END_CHAR)//the last char is the end char.
			{
				//we will treat this as a complete message, because the endmsg char is optional.
				//if the endmsg char gets sent in a subsequent block, the code above will discard it.
				state.strbFullMsg.Remove(0,1);//strip off the start char
				state.strbFullMsg.Remove(state.strbFullMsg.Length-1,1);//strip off the end char
				isFullMsg=true;
			}
			else {
				isFullMsg=false;//this is an incomplete message.  Continue to receive more blocks.
			}
			//end of big if statement-------------------------------------------------
			if(!isFullMsg) {
				try {
					//the buffer was cleared after appending the chars to the string builder
					socketIncomingHandler.BeginReceive(state.buffer,0,StateObject.BufferSize,SocketFlags.None,new AsyncCallback(OnDataReceived),state);
				}
				catch(Exception ex) {
					EventLog.WriteEntry("OpenDentalHL7","An error occurred with BeginReceive on an incoming TCP/IP HL7 message.\r\nException: "+ex.Message);
				}
				return;//get another block
			}
			//Prepare to save message to database if malformed and not processed
			HL7Msg hl7Msg=new HL7Msg();
			hl7Msg.MsgText=state.strbFullMsg.ToString();
			state.strbFullMsg.Clear();//just in case, ready for the next message
			bool isProcessed=true;
			string messageControlId="";
			string ackEvent="";
			if(isMalformed){
				hl7Msg.HL7Status=HL7MessageStatus.InFailed;
				hl7Msg.Note="This message is malformed so it was not processed.";
				HL7Msgs.Insert(hl7Msg);
				isProcessed=false;
			}
			else{
				MessageHL7 messageHl7Object=new MessageHL7(hl7Msg.MsgText);//this creates an entire heirarchy of objects.
				try {
					MessageParser.Process(messageHl7Object,IsVerboseLogging);//also saves the message to the db
					messageControlId=messageHl7Object.ControlId;
					ackEvent=messageHl7Object.AckEvent;
				}
				catch(Exception ex) {
					EventLog.WriteEntry("OpenDentHL7","Error in OnDataRecieved when processing message:\r\n"+ex.Message+"\r\n"+ex.StackTrace,EventLogEntryType.Error);
					isProcessed=false;
				}
			}
			MessageHL7 hl7Ack=MessageConstructor.GenerateACK(messageControlId,isProcessed,ackEvent);
			if(hl7Ack==null) {
				EventLog.WriteEntry("OpenDentHL7","No ACK defined for the enabled HL7 definition or no HL7 definition enabled.",EventLogEntryType.Information);
				return;
			}
			byte[] ackByteOutgoing=Encoding.ASCII.GetBytes(MLLP_START_CHAR+hl7Ack.ToString()+MLLP_END_CHAR+MLLP_ENDMSG_CHAR);
			if(IsVerboseLogging) {
				EventLog.WriteEntry("OpenDentHL7","Beginning to send ACK.\r\n"+MLLP_START_CHAR+hl7Ack.ToString()+MLLP_END_CHAR+MLLP_ENDMSG_CHAR,EventLogEntryType.Information);
			}
			socketIncomingHandler.Send(ackByteOutgoing);//this is a locking call
			//eCW uses the same worker socket to send the next message. Without this call to BeginReceive, they would attempt to send again
			//and the send would fail since we were no longer listening in this thread. eCW would timeout after 30 seconds of waiting for their
			//acknowledgement, then they would close their end and create a new socket for the next message. With this call, we can accept message
			//after message without waiting for a new connection.
			//the buffer was cleared after appending the chars to the string builder
			//the string builder was cleared after setting the message text in the table
			socketIncomingHandler.BeginReceive(state.buffer,0,StateObject.BufferSize,SocketFlags.None,new AsyncCallback(OnDataReceived),state);
		}

		private void TimerCallbackSendConnectTCP(Object stateInfo) {
			if(_ecwTCPSendSocketIsConnected) {
				return;
			}
			_ecwTCPSendSocketIsConnected=true;
			Socket socketMain=new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
			IPEndPoint endpoint=null;
			try {
				string[] strIpPort=HL7DefEnabled.OutgoingIpPort.Split(':');//this was already validated in the HL7DefEdit window.
				IPAddress ipaddress=IPAddress.Parse(strIpPort[0]);//already validated
				int port=int.Parse(strIpPort[1]);//already validated
				endpoint=new IPEndPoint(ipaddress,port);
			}
			catch(Exception ex) {//not likely, but want to make sure to reset the bool that the send socket is connected
				EventLog.WriteEntry("OpenDentalHL7","The HL7 send TCP/IP socket connection failed during IPEndPoint creation.\r\nException message: "+ex.Message,EventLogEntryType.Warning);
				socketMain.Dispose();
				_ecwTCPSendSocketIsConnected=false;
				return;//will try to create a socket again in 20 seconds
			}
			try {
				connectDone.Reset();
				socketMain.BeginConnect(endpoint,new AsyncCallback(ConnectCallback),socketMain);
				connectDone.WaitOne();//connection attempt will timeout and set the manual reset event
				if(!socketMain.Connected) {
					socketMain.Dispose();
					_ecwTCPSendSocketIsConnected=false;
					return;//will try to connect again in 20 seconds
				}
				if(!IsSendTCPConnected) {//Previous run failed to connect. This time connected so make log entry that connection was successful
					EventLog.WriteEntry("OpenDentHL7","The HL7 send TCP/IP socket connection failed to connect previously and was successful this time.",EventLogEntryType.Information);
					IsSendTCPConnected=true;
				}
			}
			catch(SocketException ex) {
				if(IsSendTCPConnected) {//Previous run connected fine, make log entry and set bool to false
					EventLog.WriteEntry("OpenDentHL7","The HL7 send TCP/IP socket connection failed to connect.\r\nSocket Exception: "+ex.Message,EventLogEntryType.Warning);
					IsSendTCPConnected=false;
				}
				socketMain.Dispose();
				_ecwTCPSendSocketIsConnected=false;
				return;//will try to connect again in 20 seconds
			}
			catch(Exception ex) {
				if(IsSendTCPConnected) {//Previous run connected fine, make log entry and set bool to false
					EventLog.WriteEntry("OpenDentHL7","The HL7 send TCP/IP socket connection failed to connect.\r\nException: "+ex.Message,EventLogEntryType.Warning);
					IsSendTCPConnected=false;
				}
				socketMain.Dispose();
				_ecwTCPSendSocketIsConnected=false;
				return;//will try to connect again in 20 seconds
			}
			//start a timer to poll the database and to send messages as needed.  Every 6 seconds.  We increased the time between polling the database from 3 seconds to 6 seconds because we are now waiting 5 seconds for a message acknowledgment from eCW.
			TimerCallback timercallbackSendTCP=new TimerCallback(TimerCallbackSendTCP);
			timerSendTCP=new System.Threading.Timer(timercallbackSendTCP,socketMain,1800,6000);
		}

		private void ConnectCallback(IAsyncResult ar) {
			try {
				// Retrieve the socket from the state object.
				Socket client = (Socket)ar.AsyncState;
				// Complete the connection.
				client.EndConnect(ar);
			}
			catch(Exception ex) {
				EventLog.WriteEntry("OpenDentalHL7","The TCP send socket encountered an issue attempting to connect.\r\nException: "+ex.Message,EventLogEntryType.Warning);
			}
			connectDone.Set();
		}
		
		private void TimerCallbackSendTCP(Object socketObject) {
			Socket socketWorker=(Socket)socketObject;
			if(socketWorker==null//null socket object
				|| !socketWorker.Connected//or socket object is no longer connected (based on last activity, like last send/receive)
				|| !socketWorker.Poll(500000,SelectMode.SelectWrite))//blocking poll (time in microseconds, so half a second) to attempt to write to socket.
			{
				//If socket is null, or not connected, or poll fails, socket was likely not shutdown gracefully so we will dispose of the send timer and worker socket
				//the connect timer will initiate a new connection with a new socket every 20 seconds if _ecwTCPSendSocketIsConnected=false;
				timerSendTCP.Dispose();
				socketWorker.Dispose();
				EventLog.WriteEntry("OpenDentalHL7","The TCP send socket has been closed.  A new socket connection attempt will occur within 20 seconds.",EventLogEntryType.Warning);
				_ecwTCPSendSocketIsConnected=false;
				return;
			}
			if(_ecwTCPModeIsSending) {
				return;
			}
			try {
				Send(socketWorker);
			}
			catch(Exception ex) {
				EventLog.WriteEntry("OpenDentalHL7","The TCP HL7 outgoing message socket encountered a problem during a send.  Another attempt to send will occur within 6 seconds.\r\nException message: "+ex.Message);
			}
			_ecwTCPModeIsSending=false;
		}

		private void Send(Socket socketWorker) {
			_ecwTCPModeIsSending=true;
			List<HL7Msg> list=HL7Msgs.GetOnePending();
			while(list.Count>0) {
				string sendMsgControlId=HL7Msgs.GetControlId(list[0]);//could be empty string
				string data=MLLP_START_CHAR+list[0].MsgText+MLLP_END_CHAR+MLLP_ENDMSG_CHAR;
				byte[] byteData=Encoding.ASCII.GetBytes(data);
				try {
					sendDone.Reset();
					socketWorker.BeginSend(byteData,0,byteData.Length,SocketFlags.None,new AsyncCallback(SendCallback),socketWorker);
					sendDone.WaitOne();
				}
				catch(Exception ex) {
					throw new Exception("BeginSend exception: "+ex.Message);
				}
				//sendSocket.Send(byteData);//this is a blocking call
				#region RecieveAndProcessAck
				//For MLLP V2, do a blocking Receive here, along with a timeout.
				byte[] ackBuffer=new byte[256];//plenty big enough to receive the entire ack/nack response
				socketWorker.ReceiveTimeout=5000;//5 second timeout. Database is polled every 6 seconds for a new message to send, but if already sending and waiting for an ack, the new thread will just return
				int byteCountReceived=0;
				try {
					byteCountReceived=socketWorker.Receive(ackBuffer);//blocking Receive
				}
				catch(Exception ex) {
					throw new Exception("Timeout or other error waiting to receive an acknowledgment.\r\nException: "+ex.Message);
				}
				char[] chars=new char[byteCountReceived];
				Encoding.UTF8.GetDecoder().GetChars(ackBuffer,0,byteCountReceived,chars,0);
				StringBuilder strbAckMsg=new StringBuilder();
				strbAckMsg.Append(chars);
				if(strbAckMsg.Length>0 && strbAckMsg[0]!=MLLP_START_CHAR) {
					list[0].Note=list[0].Note+"Malformed acknowledgment.\r\n";
					HL7Msgs.Update(list[0]);
					throw new Exception("Malformed acknowledgment.");
				}
				else if(strbAckMsg.Length>=3
					&& strbAckMsg[strbAckMsg.Length-1]==MLLP_ENDMSG_CHAR//last char is the endmsg char.
					&& strbAckMsg[strbAckMsg.Length-2]==MLLP_END_CHAR)//the second-to-the-last char is the end char.
				{
					//we have a complete message
					strbAckMsg.Remove(0,1);//strip off the start char
					strbAckMsg.Remove(strbAckMsg.Length-2,2);//strip off the end chars
				}
				else if(strbAckMsg.Length>=2//so that the next line won't crash
					&& strbAckMsg[strbAckMsg.Length-1]==MLLP_END_CHAR)//the last char is the end char.
				{
					//we will treat this as a complete message, because the endmsg char is optional.
					strbAckMsg.Remove(0,1);//strip off the start char
					strbAckMsg.Remove(strbAckMsg.Length-1,1);//strip off the end char
				}
				else {
					list[0].Note=list[0].Note+"Malformed acknowledgment.\r\n";
					HL7Msgs.Update(list[0]);
					throw new Exception("Malformed acknowledgment.");
				}
				MessageHL7 ackMsg=new MessageHL7(strbAckMsg.ToString());
				try {
					MessageParser.ProcessAck(ackMsg,IsVerboseLogging);
				}
				catch(Exception ex) {
					list[0].Note=list[0].Note+ackMsg.ToString()+"\r\nError processing acknowledgment.\r\n";
					HL7Msgs.Update(list[0]);
					throw new Exception("Error processing acknowledgment.\r\n"+ex.Message);
				}
				if(ackMsg.AckCode=="" || ackMsg.ControlId=="") {
					list[0].Note=list[0].Note+ackMsg.ToString()+"\r\nInvalid ACK message.  Attempt to resend.\r\n";
					HL7Msgs.Update(list[0]);
					throw new Exception("Invalid ACK message received.");
				}
				if(ackMsg.ControlId==sendMsgControlId && ackMsg.AckCode=="AA") {//acknowledged received (Application acknowledgment: Accept)
					list[0].Note=list[0].Note+ackMsg.ToString()+"\r\nMessage ACK (acknowledgment) received.\r\n";
				}
				else if(ackMsg.ControlId==sendMsgControlId && ackMsg.AckCode!="AA") {//ACK received for this message, but ack code was not acknowledgment accepted
					if(list[0].Note.Contains("NACK4")) {//this is the 5th negative acknowledgment, don't try again
						list[0].Note=list[0].Note+"Ack code: "+ackMsg.AckCode+"\r\nThis is NACK5, the message status has been changed to OutFailed. We will not attempt to send again.\r\n";
						list[0].HL7Status=HL7MessageStatus.OutFailed;
					}
					else if(list[0].Note.Contains("NACK")) {//failed sending at least once already
						list[0].Note=list[0].Note+"Ack code: "+ackMsg.AckCode+"\r\nNACK"+list[0].Note.Split(new string[] { "NACK" },StringSplitOptions.None).Length+"\r\n";
					}
					else {
						list[0].Note=list[0].Note+"Ack code: "+ackMsg.AckCode+"\r\nMessage NACK (negative acknowlegment) received. We will try to send again.\r\n";
					}
					HL7Msgs.Update(list[0]);//Add NACK note to hl7msg table entry
					return;
				}
				else {//ack received for control ID that does not match the control ID of message just sent
					list[0].Note=list[0].Note+"Sent message control ID: "+sendMsgControlId+"\r\nAck message control ID: "+ackMsg.ControlId
					+"\r\nAck received for message other than message just sent.  We will try to send again.\r\n";
					HL7Msgs.Update(list[0]);
					return;
				}
				#endregion
				list[0].HL7Status=HL7MessageStatus.OutSent;
				HL7Msgs.Update(list[0]);//set the status to sent and save ack message in Note field.
				if(_ecwDateTimeOldMsgsDeleted.Date<DateTime.Now.Date) {
					if(IsVerboseLogging) {
						EventLog.WriteEntry("DeleteOldMsgText Starting");
					}
					_ecwDateTimeOldMsgsDeleted=DateTime.Now;//If DeleteOldMsgText fails for any reason.  This will cause it to not get called until the next day instead of with every msg.
					HL7Msgs.DeleteOldMsgText();//this function deletes if DateTStamp is less than CURDATE-INTERVAL 4 MONTH.  That means it will delete message text only once a day, not time based.
					if(IsVerboseLogging) {
						EventLog.WriteEntry("DeleteOldMsgText Finished");
					}
				}
				list=HL7Msgs.GetOnePending();//returns 0 or 1 pending message
			}
		}

		private void SendCallback(IAsyncResult ar) {
			try {
				// Retrieve the socket from the state object.
				Socket socketWorker=(Socket)ar.AsyncState;
				// Complete sending the data to the remote device.
				socketWorker.EndSend(ar);
			}
			catch(Exception ex) {
				EventLog.WriteEntry("OpenDentalHL7","A TCP send attempt failed in SendCallback.\r\nException: "+ex.Message,EventLogEntryType.Warning);
			}
			sendDone.Set();
		}		
	}
}
