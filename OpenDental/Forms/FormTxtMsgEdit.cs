using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormTxtMsgEdit:Form {
		public long PatNum;
		public string WirelessPhone;
		public string Message;
		public YN TxtMsgOk;
		
		public FormTxtMsgEdit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormTxtMsgEdit_Load(object sender,EventArgs e) {
			textWirelessPhone.Text=WirelessPhone;
			textMessage.Text=Message;
		}

		/// <summary>May be called from other parts of the program without showing this form. You must still create an instance of this form though. Checks CallFire bridge, if it is OK to send a text, etc. (Buttons to load this form are usually  disabled if it is not OK, but this is needed for Confirmations, Recalls, etc.) </summary>
		public bool SendText(long patNum,string wirelessPhone,string message,YN txtMsgOk) {
			if(Plugins.HookMethod(this,"FormTxtMsgEdit.SendText_Start",patNum,wirelessPhone,message,txtMsgOk)) {
				return false;
			}
			if(Plugins.HookMethod(this,"FormTxtMsgEdit.SendText_Start2",patNum,wirelessPhone,message,txtMsgOk)) {
				return true;
			}
			if(wirelessPhone=="") {
				MsgBox.Show(this,"Please enter a phone number.");
				return false;
			}
			if(!Programs.IsEnabled(ProgramName.CallFire)) {
				MsgBox.Show(this,"CallFire Program Link must be enabled.");
				return false;
			}
			if(txtMsgOk==YN.Unknown && PrefC.GetBool(PrefName.TextMsgOkStatusTreatAsNo)){
				MsgBox.Show(this,"It is not OK to text this patient.");
				return false;
			}
			if(txtMsgOk==YN.No){
				MsgBox.Show(this,"It is not OK to text this patient.");
				return false;
			}
			if(message.Length>160) {
				MsgBox.Show(this,"Text length must be less than 160 characters.");
				return false;
			}
			string key=ProgramProperties.GetPropVal(ProgramName.CallFire,"Key From CallFire");
			string msg=wirelessPhone+","+message.Replace(",","");//ph#,msg Commas in msg cause error.
			try {
				CallFireService.SMSService callFire=new CallFireService.SMSService();
				callFire.sendSMSCampaign(
					key,
					new string[] { msg },
					"Open Dental");
			}
			catch(Exception ex) {
				MsgBox.Show(this,"Error sending text message.\r\n\r\n"+ex.Message);
				return false;
			}
			Commlog commlog=new Commlog();
			commlog.CommDateTime=DateTime.Now;
			commlog.DateTStamp=DateTime.Now;
			commlog.CommType=DefC.Short[(int)DefCat.CommLogTypes][0].DefNum;//The first one in the list.  We can enhance later.
			commlog.Mode_=CommItemMode.Text;
			commlog.Note=msg;//phone,note
			commlog.PatNum=patNum;
			commlog.SentOrReceived=CommSentOrReceived.Sent;
			commlog.UserNum=Security.CurUser.UserNum;
			commlog.DateTimeEnd=DateTime.Now;
			Commlogs.Insert(commlog);
			SecurityLogs.MakeLogEntry(Permissions.CommlogEdit,commlog.PatNum,"Insert Text Message");
			return true;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textMessage.Text=="") {
				MsgBox.Show(this,"Please enter a message first.");
				return;
			}
			if(!SendText(PatNum,textWirelessPhone.Text,textMessage.Text,TxtMsgOk)) {
				return;//Allow the user to try again.  A message was already shown to the user inside SendText().
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}


	}
}