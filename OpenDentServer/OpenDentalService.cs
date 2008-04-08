using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using OpenDentBusiness;
using CodeBase;
using OpenDental.DataAccess;
using System.Reflection;
using OpenDentServer.Properties;

namespace OpenDentServer {
	public partial class OpenDentalService:ServiceBase {
		private Thread workerThread = null;
		public static TcpListener server;
		private static XPathNavigator Navigator;

		public OpenDentalService() {
			InitializeComponent();
		}

		static void Main(string[] args) {
			System.ServiceProcess.ServiceBase.Run(new OpenDentalService());
		}

		protected override void OnStart(string[] args) {
			if((workerThread==null) || 
				((workerThread.ThreadState & (System.Threading.ThreadState.Unstarted | System.Threading.ThreadState.Stopped)) != 0))
			{
				workerThread = new Thread(new ThreadStart(ServiceWorkerMethod));
				workerThread.Start();
			}
		}

		public static int Login(string database,string oduser,string odpasshash) {
			//Get the database server, user, and password from the config file
			//make sure there's an entry for the requested database:
			XPathNavigator navConn=Navigator.SelectSingleNode("//DatabaseConnection[Database='"+database+"']");
			if(navConn==null) {
				throw new Exception(database+" is not an allowed database.");
			}
			//return navOne.SelectSingleNode("summary").Value;
			//now, get the values for this connection
			string server=navConn.SelectSingleNode("ComputerName").Value;
			string mysqlUser=navConn.SelectSingleNode("User").Value;
			string mysqlPassword=navConn.SelectSingleNode("Password").Value;
			string mysqlUserLow=navConn.SelectSingleNode("UserLow").Value;
			string mysqlPasswordLow=navConn.SelectSingleNode("PasswordLow").Value;
			XPathNavigator dbTypeNav=navConn.SelectSingleNode("DatabaseType");
			DatabaseType dbtype=DatabaseType.MySql;
			if(dbTypeNav!=null){
				if(dbTypeNav.Value=="Oracle"){
					dbtype=DatabaseType.Oracle;
				}
			}
			DataConnection dcon=new DataConnection();
			//Try to connect to the database
			try {
				dcon.SetDb(server,database,mysqlUser,mysqlPassword,mysqlUserLow,mysqlPasswordLow,dbtype);
				Console.WriteLine(oduser);
			}
			catch {
				throw new Exception(@"Connection to database failed.  Check the values in the config file on the server, usually at C:\Program Files\Open Dental Server\OpenDentServerConfig.xml");
			}
			//Then, check username and password
			if(!Userods.CheckUserAndPassword(oduser,odpasshash)) {
				throw new Exception("Invalid username or password.");
			}
			return 0;//meaningless
		}

		// Define a simple method that runs as the worker thread for the service.  
		public void ServiceWorkerMethod() {
			try {
				do {
                    // Look in the program directory first (e.g. if the program is at c:\foo\bar.exe, this will be c:\foo)
					string configfile=ODFileUtils.CombinePaths(Application.StartupPath,"OpenDentServerConfig.xml");
					if(!File.Exists(configfile)){
                        // Else, try the current working directory. This is required to get the NUnit tests working.
                        configfile=ODFileUtils.CombinePaths(Environment.CurrentDirectory,"OpenDentServerConfig.xml");
                        if(!File.Exists(configfile)){
                            throw new Exception("Could not find " + configfile);
                        }
					}
					XmlDocument doc=new XmlDocument();
					try {
						doc.Load(configfile);
					}
					catch {
						throw new Exception(configfile+" is not a valid format.");
					}
					Navigator=doc.CreateNavigator();
					XPathNavigator navport=Navigator.SelectSingleNode("//ServerPort");
					if(navport==null) {
						throw new Exception("ServerPort element not found in config file.");
					}
					int port=PIn.PInt(navport.Value);
					server=null;
					try {
						server = new TcpListener(port);//localAddr,port);//but this is the only way that works!
						server.Start();
						Console.Write("Waiting...");
						while(true) {// Enter the listening loop.
							//Each loop will pick up a new client computer, and pass off the connection to a worker thread. 
							// Perform a blocking call to accept requests.
							// You could also use server.AcceptSocket() here.
							TcpClient client = server.AcceptTcpClient();
							NetworkStream netStream = client.GetStream();
							Console.Write("Connecting...");
							WorkerClass worker = new WorkerClass(netStream);
							Thread workerThread = new Thread(new ThreadStart(worker.DoWork));
							workerThread.Start();
						}
					}
					catch(SocketException e) {
						Console.WriteLine("SocketException: {0}", e);
					}
					finally {
						// Stop listening for new clients.
						server.Stop();
						Console.WriteLine("Server stopped");
					}
				}
				while(true);
			}
			catch(ThreadAbortException) {
				// Another thread has signalled that this worker
				// thread must terminate.  Typically, this occurs when
				// the main service thread receives a service stop 
				// command.
			}
		}
	}

	public class WorkerClass {
		private NetworkStream netStream;

		// The constructor obtains the state information.
		public WorkerClass(NetworkStream stream) {
			netStream=stream;
		}

		public void DoWork() {
			while(true) {//Each loop gets and returns one message pair.
				byte[] data =  null;
				// Retrieve data from client
				try {
					data = RemotingClient.ReadDataFromStream(netStream);
				}
				catch {//if connection was closed by client.
					break;
				}
				DataTransferObject dto=DataTransferObject.Deserialize(data);
				//Process and send response to client--------------------------------------------------------------
				XmlSerializer serializer;
				using (MemoryStream memStream = new MemoryStream()) {
					try {
						Type type = dto.GetType();
						if (type == typeof(DtoGetDS)) {
							DataSet ds = DataCore.GetDsByMethod(((DtoGetDS)dto).MethodNameDS, ((DtoGetDS)dto).Parameters);
							serializer = new XmlSerializer(typeof(DataSet));
							serializer.Serialize(memStream, ds);
						}
						else if (type == typeof(DtoGetTable)) {
							DataTable tb = DataCore.GetTableByMethod(((DtoGetTable)dto).MethodNameTable, ((DtoGetTable)dto).Parameters);
							serializer = new XmlSerializer(typeof(DataTable));
							serializer.Serialize(memStream, tb);
						}
						else if (type.BaseType == typeof(DtoCommandBase)) {
							int result = BusinessLayer.ProcessCommand((DtoCommandBase)dto);
							DtoServerAck ack = new DtoServerAck();
							ack.IDorRows = result;
							serializer = new XmlSerializer(typeof(DtoServerAck));
							serializer.Serialize(memStream, ack);
						}
						else if (type.BaseType == typeof(DtoQueryBase)) {
							DataSet ds = BusinessLayer.ProcessQuery((DtoQueryBase)dto);
							serializer = new XmlSerializer(typeof(DataSet));
							serializer.Serialize(memStream, ds);
						}
						else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(FactoryTransferObject<>)) {
							// Pass the DTO to the FactoryServer<T>
							Type factoryServerType = typeof(FactoryServer<>);
							factoryServerType = factoryServerType.MakeGenericType(type.GetGenericArguments());

							MethodInfo processCommandMethod = factoryServerType.GetMethod("ProcessCommand", BindingFlags.Public | BindingFlags.Static);
							processCommandMethod.Invoke(null, new object[] { memStream, dto });
						}
						else {
							throw new NotSupportedException(string.Format(Resources.DtoNotSupportedException, type.FullName));
						}
					}
					catch (Exception e) {
						DtoException exception = new DtoException();
						exception.Message = e.Message;
						serializer = new XmlSerializer(typeof(DtoException));
						serializer.Serialize(memStream, exception);
					}
					data = memStream.ToArray();
					RemotingClient.WriteDataToStream(netStream, data);
				}
			}//wait for the next message
			//connection was lost.  Client probably closed program
			netStream.Close();
		}
	}
}
