using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	///<summary>Internet browser window for NewCrop.  This is essentially a Microsoft Internet Explorer control embedded into our form.
	///The browser.ScriptErrorsSuppressed is true in order to prevent javascript error popups from annoying the user.</summary>
	public partial class FormErx:ODForm {

		private string urlBrowseTo="";
		///<summary>The PatNum of the patient eRx was opened for.  The patient is tied to the window so that when the window is closed the Chart
		///knows which patient to refresh.  If the patient is different than the patient modified in the eRx window then the Chart does not need to 
		///refresh.</summary>
		public Patient PatCur=null;
		///<summary>This XML contains the patient information, provider information, employee information, practice information, etc...</summary>
		public string ClickThroughXml="";

		public FormErx() {
			InitializeComponent();
			Lan.F(this);
			SHDocVw.WebBrowser axBrowser=(SHDocVw.WebBrowser)browser.ActiveXInstance;
			if(axBrowser!=null) {//This was null once during testing.  Not sure when null can happen.  Not sure if we should allow the user to continue.
				axBrowser.NewWindow2+=axBrowser_NewWindow2;
				axBrowser.NewWindow3+=axBrowser_NewWindow3;
			}
			browser.DocumentTitleChanged+=browser_DocumentTitleChanged;
		}

		///<summary>Used when opening a new browser window via a link.</summary>
		public FormErx(string url) {
			InitializeComponent();
			Lan.F(this);
			urlBrowseTo=url;
		}

		private void FormErx_Load(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;//Is set back to default cursor after the document loads inside the browser.
			Application.DoEvents();//To show cursor change.
			Text=Lan.g(this,"Loading")+"...";
			LayoutToolBars();
			if(urlBrowseTo!="") { //Use the window as a simple web browswer when a URL is passed in.
				browser.Navigate(urlBrowseTo);
				Cursor=Cursors.Default;
				return;
			}
			ComposeNewRx();
		}

		///<summary></summary>
		public void LayoutToolBars() {
			ToolBarMain.Buttons.Clear();
			//ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Back"),0,"","Back"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Forward"),1,"","Forward"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Refresh"),-1,"","Refresh"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Close"),-1,"","Close"));
			ToolBarMain.Invalidate();
		}

		///<summary>Sends the ClickThroughXml to eRx and loads the result within the browser control.
		///Loads the compose tab in NewCrop's web interface.  Can be called externally to send provider information to eRx
		///without allowing the user to write any prescriptions.</summary>
		public void ComposeNewRx() {
			string xmlBase64=System.Web.HttpUtility.HtmlEncode(Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(ClickThroughXml)));
			xmlBase64=xmlBase64.Replace("+","%2B");//A common base 64 character which needs to be escaped within URLs.
			xmlBase64=xmlBase64.Replace("/","%2F");//A common base 64 character which needs to be escaped within URLs.
			xmlBase64=xmlBase64.Replace("=","%3D");//Base 64 strings usually end in '=', but parameters also use '=' so we must escape.
			String postdata="RxInput=base64:"+xmlBase64;
			byte[] arrayPostDataBytes=System.Text.Encoding.UTF8.GetBytes(postdata);
			string additionalHeaders="Content-Type: application/x-www-form-urlencoded\r\n";
#if DEBUG
			string newCropUrl="http://preproduction.newcropaccounts.com/interfaceV7/rxentry.aspx";
#else //Debug
			string newCropUrl="https://secure.newcropaccounts.com/interfacev7/rxentry.aspx";
#endif
			browser.Navigate(newCropUrl,"",arrayPostDataBytes,additionalHeaders);
		}

		///<summary>Linked up to the browser in the designer.
		///This event fires when a link is clicked within the webbrowser control which opens in a new window.
		///The browser.IsWebBrowserContextMenuEnabled is set to false to disable the popup menu that shows up when right clicking on links or images,
		///because right clicking a link and choosing to open in a new window causes this function to fire but the destination URL is unknown and thus
		///we cannot handle that situation.  Best to hide the context menu since there is little or no need for it.</summary>
		private void browser_NewWindow(object sender,CancelEventArgs e) {
			CreateNewWindow(browser.StatusText);//This is the URL of the page that is supposed to open in a new window.
			e.Cancel=true;//Cancel Internet Explorer from launching.
		}

		///<summary>This event fires when a javascript snippet calls window.open() to open a URL in a new	browser window.
		///When window.open() is called, our browser_NewWindow() event function does not fire.</summary>
		private void axBrowser_NewWindow2(ref object ppDisp,ref bool Cancel) {
			//We could not get this event to fire in testing.  Here just in case we need it.
			CreateNewWindow(browser.StatusText);//This is the URL of the page that is supposed to open in a new window.
			Cancel=true;//Cancel Internet Explorer from launching.
		}

		///<summary>We are not sure when this event function fires, but we implemented it just in case.</summary>
		void axBrowser_NewWindow3(ref object ppDisp,ref bool Cancel,uint dwFlags,string bstrUrlContext,string bstrUrl) {
			//We could not get this event to fire in testing.  Here just in case we need it.
			CreateNewWindow(bstrUrl);
			Cancel=true;//Cancel Internet Explorer from launching.
		}

		///<summary>This helper function is called any time a new browser window needs to be opened.  By default, new windows launched by clicking a link
		///from within the webbrowser control will open in Internet Explorer, even if the system default is another web browser such as Mozilla.  We had a
		///problem with cookies not being carried over from our webbrowser control into Internet Explorer when a link is clicked.  To preserve cookies, we
		///intercept the new window creation, cancel it, then launch the destination URL in a new OD browser window.  Cancel the new window creation
		///inside the calling event.</summary>
		private void CreateNewWindow(string url) {
			//For example, the "ScureScripts Drug History" link within the "Compose Rx" tab.
			if(Regex.IsMatch(url,"^.*javascript\\:.*$",RegexOptions.IgnoreCase)) {//Ignore tab clicks because the user is not navigating to a new page.
				return;
			}
			FormErx formNew=new FormErx(url);//Open the page in a new window, but stay inside of OD.
			formNew.WindowState=FormWindowState.Normal;
			formNew.Show();//Non-modal, so that we get the effect of opening in an independent window.
		}

		///<summary>Called after a document has finished loading, including initial page load and when Back and Forward buttons are pressed.</summary>
		public void browser_DocumentCompleted(object sender,WebBrowserDocumentCompletedEventArgs e) {
			Cursor=Cursors.Default;
			SetTitle();
		}

		private void browser_DocumentTitleChanged(object sender,EventArgs e) {
			SetTitle();
		}

		private void SetTitle() {
			Text=Lan.g(this,"eRx");
			if(browser.DocumentTitle.Trim()!="") {
				Text+=" - "+browser.DocumentTitle;
			}
			if(PatCur!=null) {//Can only be null when a subwindow is opened by clicking on a link from inside another FormErx instance.
				Text+=" - "+PatCur.GetNameFL();
			}
		}

		private void ToolBarMain_ButtonClick(object sender,ODToolBarButtonClickEventArgs e) {
			switch(e.Button.Tag.ToString()) {
				case "Back":
					if(browser.CanGoBack) {
						Cursor=Cursors.WaitCursor;//Is set back to default cursor after the document loads inside the browser.
						Application.DoEvents();//To show cursor change.
						Text=Lan.g(this,"Loading")+"...";
						browser.GoBack();
					}
					break;
				case "Forward":
					if(browser.CanGoForward) {
						Cursor=Cursors.WaitCursor;//Is set back to default cursor after the document loads inside the browser.
						Application.DoEvents();//To show cursor change.
						Text=Lan.g(this,"Loading")+"...";
						browser.GoForward();
					}
					break;
				case "Refresh":
					browser.Refresh();
					break;
				case "Close":
					DialogResult=DialogResult.Cancel;
					Close();//For when we launch the window in a non-modal manner.
					break;
			}
		}

		private void FormErx_FormClosed(object sender,FormClosedEventArgs e) {
			ODEvent.Fire(new ODEventArgs("ErxBrowserClosed",PatCur));
		}

	}
}