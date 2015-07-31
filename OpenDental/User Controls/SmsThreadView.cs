using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenDental {
	///<summary>For SMS Text Messaging.  Used in the Text Messaging window to display an SMS message thread much like a cell phone.
	///Since users are used to seeing text message threads on cell phones, this control will be intuitive to users.</summary>
	public partial class SmsThreadView:UserControl {

		private List<SmsThreadMessage> _listSmsThreadMessages=null;
		///<summary>Set this value externally before showing the control.</summary>
		public List<SmsThreadMessage> ListSmsThreadMessages {
			get {
				return _listSmsThreadMessages;
			}
			set {
				_listSmsThreadMessages=value;
				FillMessageThread();
			}
		}

		public SmsThreadView() {
			InitializeComponent();
		}

		private void FillMessageThread() {
			panelScroll.Controls.Clear();
			Invalidate();
			if(_listSmsThreadMessages==null) {
				return;
			}
			int bodyWidth=panelScroll.Width-SystemInformation.VerticalScrollBarWidth;
			int verticalPadding=5;
			int horizontalMargin=(int)(bodyWidth*0.02);
			int y=0;
			Control controlHighlighted=null;
			for(int i=0;i<_listSmsThreadMessages.Count;i++) {
				y+=verticalPadding;
				Label labelDateTime=new Label();
				labelDateTime.Name="labelSmsDateTime"+i;
				labelDateTime.Text=_listSmsThreadMessages[i].MsgDateTime.ToString();
				if(_listSmsThreadMessages[i].IsAlignedLeft) {
					labelDateTime.TextAlign=ContentAlignment.MiddleLeft;
				}
				else {//Aligned right
					labelDateTime.TextAlign=ContentAlignment.MiddleRight;
				}
				Size textSize=TextRenderer.MeasureText(labelDateTime.Text,panelScroll.Font,
					new Size(bodyWidth,Int32.MaxValue),TextFormatFlags.WordBreak);
				labelDateTime.Width=bodyWidth;
				labelDateTime.Height=textSize.Height+2;//Extra vertical padding to ensure that the text will fit when including the border.
				labelDateTime.Location=new Point(0,y);
				panelScroll.Controls.Add(labelDateTime);
				y+=labelDateTime.Height;
				TextBox textBoxMessage=new TextBox();
				textBoxMessage.BackColor=_listSmsThreadMessages[i].BackColor;
				if(_listSmsThreadMessages[i].IsHighlighted) {
					controlHighlighted=textBoxMessage;
				}
				if(_listSmsThreadMessages[i].IsImportant) {
					textBoxMessage.ForeColor=Color.Red;
				}
				textBoxMessage.Name="textSmsThreadMsg"+i;
				textBoxMessage.BorderStyle=BorderStyle.FixedSingle;
				textBoxMessage.Multiline=true;
				textBoxMessage.Text=_listSmsThreadMessages[i].Message;
				//Each message wraps horizontally.
				textSize=TextRenderer.MeasureText(textBoxMessage.Text,panelScroll.Font,
					new Size((int)(bodyWidth*0.7),Int32.MaxValue),TextFormatFlags.WordBreak);
				textBoxMessage.Width=textSize.Width+4;//Extra horizontal padding to ensure that the text will fit when including the border.
				textBoxMessage.Height=textSize.Height+4;//Extra vertical padding to ensure that the text will fit when including the border.
				textBoxMessage.ReadOnly=true;
				if(_listSmsThreadMessages[i].IsAlignedLeft) {
					textBoxMessage.Location=new Point(horizontalMargin,y);
				}
				else {//Right aligned
					textBoxMessage.Location=new Point(bodyWidth-horizontalMargin-textBoxMessage.Width,y);
				}
				panelScroll.Controls.Add(textBoxMessage);
				y+=textBoxMessage.Height;
			}
			Label labelBottomSpacer=new Label();
			labelBottomSpacer.Name="labelBottomSpacer";
			labelBottomSpacer.Width=bodyWidth;
			labelBottomSpacer.Height=verticalPadding;
			labelBottomSpacer.Location=new Point(0,y);
			panelScroll.Controls.Add(labelBottomSpacer);
			y+=labelBottomSpacer.Height;
			if(controlHighlighted==null) {
				controlHighlighted=labelBottomSpacer;
			}
			if(panelScroll.VerticalScroll.Value!=panelScroll.VerticalScroll.Maximum) {
				panelScroll.VerticalScroll.Value=panelScroll.VerticalScroll.Maximum; //scroll to the end first then scroll to control
			}
			panelScroll.ScrollControlIntoView(controlHighlighted);//Scroll to highlighted control, or if none highlighted, then scroll to the end.
		}

	}

	public class SmsThreadMessage {
		///<summary>The date and time the message was sent or received.</summary>
		public DateTime MsgDateTime;
		///<summary>The message itself.</summary>
		public string Message;
		///<summary>If true, the message will be left aligned.  Otherwise the message will be right aligned.  Left aligned messages will be messages from
		///the patient, and right aligned messages will be from the office.  The left/right alignment is used as a quick way to show the user who
		///wrote each part of the message thread.</summary>
		public bool IsAlignedLeft;
		///<summary>Causes the message text to show in red.</summary>
		public bool IsImportant;
		public bool IsHighlighted;

		public Color BackColor {
			get {
				Color retVal;
				if(IsAlignedLeft) {//From Customer
					retVal=Color.FromArgb(244,255,244);
					if(IsHighlighted) {
						retVal=Color.FromArgb(220,255,220);
					}
				}
				else {//Right aligned
					retVal=Color.White;
					if(IsHighlighted) {
						retVal=Color.FromArgb(220,220,220);
					}
				}
				return retVal;
			}
		}

		public SmsThreadMessage(DateTime msgDateTime,string message,bool isAlignedLeft,bool isImportant,bool isHighlighted) {
			MsgDateTime=msgDateTime;
			Message=message;
			IsAlignedLeft=isAlignedLeft;
			IsImportant=isImportant;
			IsHighlighted=isHighlighted;
		}

		public static int CompareMessages(SmsThreadMessage msg1,SmsThreadMessage msg2) {
			return msg1.MsgDateTime.CompareTo(msg2.MsgDateTime);
		}

	}

}
