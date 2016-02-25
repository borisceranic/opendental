using AForge.Video.DirectShow;
using OpenDentBusiness;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace WebCamOD {
	public partial class FormWebCamOD:Form {
		private FilterInfoCollection _videoDeviceCollection;
		private VideoCaptureDevice _videoDevice;
		private string _ipAddressCur;

		public FormWebCamOD() {
			InitializeComponent();
		}

		private void FormWebCamOD_Load(object sender,EventArgs e) {
			#region Check WebCamOD Process
			Process[] processes=Process.GetProcessesByName("WebCamOD");
			for(int p=0;p<processes.Length;p++) {
				if(Process.GetCurrentProcess().Id==processes[p].Id) {
					continue;
				}
				//another process was found
				MessageBox.Show("WebCamOD is already running.");
				Application.Exit();
				return;
			}
			#endregion
			#region Database Connection
			//since this tool is only used at HQ, we hard code everything
			IPHostEntry iphostentry=Dns.GetHostEntry(Environment.MachineName);
			DataConnection dbcon=new DataConnection();
			try {
				dbcon.SetDb("10.10.1.200","customers","root","","","",DatabaseType.MySql);
			}
			catch {
				MessageBox.Show("This tool is not designed for general use.");
				return;
			}
			//get ipaddress on startup
			_ipAddressCur="";
			foreach(IPAddress ipaddress in iphostentry.AddressList) {
				if(ipaddress.ToString().Contains("10.10.2")) {
					_ipAddressCur=ipaddress.ToString();
				}
			}
			#endregion
			//Start the timer for taking snapshot first because it won't fire until the time has been reached (typically 5 seconds).
			//Even if the tick fires before the web cam is ready to take snapshots, it will not fail and will continue to tick.
			timerWebCamSnapshots.Interval=PrefC.GetInt(PrefName.WebCamFrequencyMS);
			timerWebCamSnapshots.Start();
			try {
				InitializeWebCam();
			}
			catch(Exception ex) {
				MessageBox.Show("Error initializing web cam:\r\n"+ex.Message);
				Application.Exit();
				return;
			}
		}

		///<summary>Starts a video feed from the web cam so that we can start taking snapshots from the active feed.  Throws exceptions.</summary>
		private void InitializeWebCam() {
			_videoDeviceCollection=new FilterInfoCollection(FilterCategory.VideoInputDevice);
			if(_videoDeviceCollection.Count < 1) {
				throw new Exception("No video capturing devices detected.");
			}
			//Set the video capture device to the first one in the list.
			_videoDevice=new VideoCaptureDevice(_videoDeviceCollection[0].MonikerString);
			//Set the resolution to the first in the list of capable resolutions.
			_videoDevice.VideoResolution=_videoDevice.VideoCapabilities[0];
			//Set the video source of the custom video previewing control to the video device we just set up.
			videoSourcePlayer.VideoSource=_videoDevice;
			//Start the video feed.
			videoSourcePlayer.Start();
		}

		private void timerWebCamSnapshots_Tick(object sender,EventArgs e) {
			try {
				//If there is a corresponding entry in phone table matching this machine IP and there is a valid video capture device save the snapshot.
				if(_ipAddressCur!="" && videoSourcePlayer!=null && videoSourcePlayer.VideoSource!=null) {
					using(Bitmap bitmapOrig=videoSourcePlayer.GetCurrentVideoFrame())
					using(Bitmap bitmapSmall=new Bitmap(50,(int)(50f/640f*480f),System.Drawing.Imaging.PixelFormat.Format24bppRgb))
					using(Graphics graphicsSmall=Graphics.FromImage(bitmapSmall)) {
						graphicsSmall.DrawImage(bitmapOrig,new Rectangle(0,0,bitmapSmall.Width,bitmapSmall.Height));
						Phones.SetWebCamImage(_ipAddressCur,bitmapSmall,Environment.MachineName);
					}
				}
			}
			catch(Exception ex) {
				string test=ex.Message;
			}//Prevents UE from losing MySQL service
		}

		private void FormWebCamOD_FormClosing(object sender,FormClosingEventArgs e) {
			//Allow the web cam to gracefully shut down so that the video memory is correctly released and the web cam actually turns off.
			if(videoSourcePlayer!=null && videoSourcePlayer.VideoSource!=null) {
				videoSourcePlayer.SignalToStop();
				videoSourcePlayer.WaitForStop();
				videoSourcePlayer.VideoSource=null;
			}
		}
	}
}
