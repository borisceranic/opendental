using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenDental.UI {
	///<summary>Wraps the Topaz SigPlusNET control and the alternate SignatureBox control.  Also includes both needed buttons.  Should vastly simplify using signature boxes throughout the program.</summary>
	public partial class SignatureBoxWrapper:UserControl {
		private bool sigChanged;
		//private bool allowTopaz;
		private Control sigBoxTopaz;
		///<summary>The reason for this event is so that if a different user is signing, that it properly records the change in users.  See the example pattern in FormProcGroup.</summary>
		[Category("Action"),Description("Event raised when signature is cleared or altered.")]
		public event EventHandler SignatureChanged=null;
		///<summary>Event raised when the X button is clicked.</summary>
		[Category("Action"),Description("Event raised when the X button is clicked.")]
		public event EventHandler ClearSignatureClicked=null;
		///<summary>Event raised when the Sign Topaz button is clicked.</summary>
		[Category("Action"),Description("Event raised when the Sign Topaz button is clicked.")]
		public event EventHandler SignTopazClicked=null;
		///<summary>Used for special cases where signature logic varies from our default.</summary>
		public SigMode SignatureMode=SigMode.Default;

		[Category("Property"),Description("Set the text that shows in the invalid signature label"),DefaultValue("Invalid Signature")]
		///<summary>Usually "Invalid Signature", but this can be changed for different situations.</summary>
		public string LabelText {
			get {
				return labelInvalidSig.Text;
			}
			set {
				labelInvalidSig.Text=value;
			}
		}

		///<summary>A new Width property with local scope to SignatureBoxWrapper.  
		///Sets the width of this control as well as the width of the topaz signature control.
		///This is necessary because resizing the SigBoxWrapper will cause the sigBox control (via anchors) to resize.
		///sigBoxTopaz should always be the same size as sigBox.</summary>
		public new int Width {
			get {
				return base.Width;
			}
			set {
				base.Width=value;
				if(sigBoxTopaz!=null && sigBox!=null) {
					sigBoxTopaz.Width=sigBox.Width;
				}
			}
		}

		///<summary>A new Height property with local scope to SignatureBoxWrapper.  
		///Set the height of this control as well as the width of the topaz signature control.
		///This is necessary because resizing the SigBoxWrapper will cause the sigBox control (via anchors) to resize.
		///sigBoxTopaz should always be the same size as sigBox.</summary>
		public new int Height {
			get {
				return base.Height;
			}
			set {
				base.Height=value;
				if(sigBoxTopaz!=null && sigBox!=null) {
					sigBoxTopaz.Height=sigBox.Height;
				}
			}
		}

		public SignatureBoxWrapper() {
			InitializeComponent();
			//allowTopaz=(Environment.OSVersion.Platform!=PlatformID.Unix && !CodeBase.ODEnvironment.Is64BitOperatingSystem());
			sigBox.SetTabletState(1);
			//if(!allowTopaz) {
			//  butTopazSign.Visible=false;
			//  sigBox.Visible=true;
			//}
			//else{
				//Add signature box for Topaz signatures.
				sigBoxTopaz=CodeBase.TopazWrapper.GetTopaz();
				sigBoxTopaz.Location=sigBox.Location;//this puts both boxes in the same spot.
				sigBoxTopaz.Name="sigBoxTopaz";
				sigBoxTopaz.Size=sigBox.Size;//new System.Drawing.Size(362,79);
				sigBox.Anchor=(AnchorStyles)(AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right);
				sigBoxTopaz.TabIndex=92;
				sigBoxTopaz.Text="sigPlusNET1";
				sigBoxTopaz.Visible=false;
				sigBoxTopaz.Leave+=new EventHandler(sigBoxTopaz_Leave);
				Controls.Add(sigBoxTopaz);
				butTopazSign.BringToFront();
				butClearSig.BringToFront();
			//}
		}

		protected void OnSignatureChanged() {
			sigChanged=true;
			if(SignatureChanged!=null){
				SignatureChanged(this,new EventArgs());
			}
		}

		public void FillSignature(bool sigIsTopaz,string keyData,string signature) {
			if(signature==null || signature=="") {
				return;
			}
			//This does 3 checks for both topaz and normal signatures.  These keyData replacements are due to MiddleTier & RichTextBox newline issues.
			//Most cases will not get past check 1.  Some of the following checks are taken care of by things like POut.StringNote().
			//Check 1:  keyData.Replace("\r\n","\n").  This reverts any changes middle tier made to the keydata.  
			//					Middle tier converts "\r\n" -> "\n" -> "\r\n" so we change it back to "normal".
			//Check 2:  Normal keydata.  This is for cases that had "\r\n" as the original note.  These are keydata that were captured with a textbox.
			//Check 3:  keyData.Replace("\r\n","\n").Replace("\n","\r\n").  This is for cases where the original note was captured with a textbox, and then
			//					was filled into an ODTextBox(richtextbox) and the "\r\n" was changed to "\n" then the user clicked ok changing the note.
			//Note that if the keydata was hashed before this point, these replacements have to be dealt with outside of this function.
			if(sigIsTopaz) {
				FillSignatureHelperTopaz(keyData.Replace("\r\n","\n"),signature);
				if(CodeBase.TopazWrapper.GetTopazNumberOfTabletPoints(sigBoxTopaz)==0) {
					FillSignatureHelperTopaz(keyData,signature);
				}
				if(CodeBase.TopazWrapper.GetTopazNumberOfTabletPoints(sigBoxTopaz)==0) {
					FillSignatureHelperTopaz(keyData.Replace("\r\n","\n").Replace("\n","\r\n"),signature);
				}
			}
			else {
				FillSignatureHelper(keyData.Replace("\r\n","\n"),signature);
				if(sigBox.NumberOfTabletPoints()==0) {
					FillSignatureHelper(keyData,signature);
				}
				if(sigBox.NumberOfTabletPoints()==0) {
					FillSignatureHelper(keyData.Replace("\r\n","\n").Replace("\n","\r\n"),signature);
				}
			}
		}

		private void FillSignatureHelperTopaz(string keyData,string signature) {
			sigBox.Visible=false;
			sigBoxTopaz.Visible=true;
			labelInvalidSig.Visible=false;
			CodeBase.TopazWrapper.ClearTopaz(sigBoxTopaz);
			CodeBase.TopazWrapper.SetTopazCompressionMode(sigBoxTopaz,0);
			CodeBase.TopazWrapper.SetTopazEncryptionMode(sigBoxTopaz,0);
			//Some places used different logic for keystring and autokeydata in the past.  This must be maintained to keep signatures valid.
			switch(SignatureMode) {
				case SigMode.Document:
					CodeBase.TopazWrapper.SetTopazKeyString(sigBoxTopaz,keyData);
					CodeBase.TopazWrapper.SetTopazEncryptionMode(sigBoxTopaz,2);//high encryption
					CodeBase.TopazWrapper.SetTopazCompressionMode(sigBoxTopaz,2);//high compression
					CodeBase.TopazWrapper.SetTopazSigString(sigBoxTopaz,signature);
					break;
				case SigMode.TreatPlan:
					string hashedKeyData= OpenDentBusiness.TreatPlans.GetHashStringForSignature(keyData);//Passed in KeyData still needs to be hashed.
					CodeBase.TopazWrapper.SetTopazKeyString(sigBoxTopaz,hashedKeyData);
					CodeBase.TopazWrapper.SetTopazEncryptionMode(sigBoxTopaz,2);//high encryption
					CodeBase.TopazWrapper.SetTopazCompressionMode(sigBoxTopaz,2);//high encryption
					CodeBase.TopazWrapper.SetTopazSigString(sigBoxTopaz,signature);
					sigBoxTopaz.Refresh();
					//If sig is not showing, then try encryption mode 3 for signatures signed with old SigPlusNet.dll.
					if(CodeBase.TopazWrapper.GetTopazNumberOfTabletPoints(sigBoxTopaz)==0) {
						CodeBase.TopazWrapper.SetTopazEncryptionMode(sigBoxTopaz,3);//Unknown mode (told to use via TopazSystems)
						CodeBase.TopazWrapper.SetTopazSigString(sigBoxTopaz,signature);
					}
					break;
				case SigMode.Default:
					CodeBase.TopazWrapper.SetTopazKeyString(sigBoxTopaz,"0000000000000000");
					CodeBase.TopazWrapper.SetTopazEncryptionMode(sigBoxTopaz,2);//high encryption
					CodeBase.TopazWrapper.SetTopazCompressionMode(sigBoxTopaz,2);//high compression
					CodeBase.TopazWrapper.SetTopazSigString(sigBoxTopaz,signature);
					//older items may have been signed with zeros due to a bug.  We still want to show the sig in that case.
					//but if a sig is not showing, then set the key string to try to get it to show.
					if(CodeBase.TopazWrapper.GetTopazNumberOfTabletPoints(sigBoxTopaz)==0) {
						CodeBase.TopazWrapper.SetTopazAutoKeyData(sigBoxTopaz,keyData);
						CodeBase.TopazWrapper.SetTopazSigString(sigBoxTopaz,signature);
					}
					//If sig is not showing, then try encryption mode 3 for signatures signed with old SigPlusNet.dll.
					if(CodeBase.TopazWrapper.GetTopazNumberOfTabletPoints(sigBoxTopaz)==0) {
						CodeBase.TopazWrapper.SetTopazEncryptionMode(sigBoxTopaz,3);//Unknown mode (told to use via TopazSystems)
						CodeBase.TopazWrapper.SetTopazSigString(sigBoxTopaz,signature);
					}
					break;
			}
			//If sig still not showing it must be invalid.
			if(CodeBase.TopazWrapper.GetTopazNumberOfTabletPoints(sigBoxTopaz)==0) {
				labelInvalidSig.Visible=true;
			}
			CodeBase.TopazWrapper.SetTopazState(sigBoxTopaz,0);
		}

		private void FillSignatureHelper(string keyData,string signature) {
			sigBox.Visible=true;
			sigBoxTopaz.Visible=false;
			labelInvalidSig.Visible=false;
			sigBox.ClearTablet();
			//sigBox.SetSigCompressionMode(0);
			//sigBox.SetEncryptionMode(0);
			//Some places used different logic for keystring and autokeydata in the past.  This must be maintained to keep signatures valid.
			switch(SignatureMode) {
				case SigMode.Document:
					//No auto key data set, only key string.
					sigBox.SetKeyString(keyData);
					break;
				case SigMode.TreatPlan:
					string hashedKeyData = OpenDentBusiness.TreatPlans.GetHashStringForSignature(keyData);//Passed in KeyData still needs to be hashed.
					//No auto key data set, only key string.
					sigBox.SetKeyString(hashedKeyData);
					break;
				case SigMode.Default:
					sigBox.SetKeyString("0000000000000000");
					sigBox.SetAutoKeyData(keyData);
					break;
			}
			//sigBox.SetEncryptionMode(2);//high encryption
			//sigBox.SetSigCompressionMode(2);//high compression
			sigBox.SetSigString(signature);
			if(sigBox.NumberOfTabletPoints()==0) {  //Both signature checks were invalid.
				labelInvalidSig.Visible=true;
			}
			sigBox.SetTabletState(0);//not accepting input.  To accept input, change the note, or clear the sig.
		}

		///<summary>Helper method to manipulate the visibility of all buttons on the signature wrapper.</summary>
		public void SetButtonVisibility(bool isVisible) {
			butClearSig.Visible=isVisible;
			butTopazSign.Visible=isVisible;
		}

		public int GetNumberOfTabletPoints(bool sigIsTopaz) {
			if(sigIsTopaz) {
				return CodeBase.TopazWrapper.GetTopazNumberOfTabletPoints(sigBoxTopaz);
			}
			return sigBox.NumberOfTabletPoints();
		}

		///<summary>This can be used to determine whether the signature has changed since the control was created.  It is, however, preferrable to have the parent form use the SignatureChanged event to track changes.</summary>
		public bool GetSigChanged(){
			return sigChanged;
		}

		///<summary>This should NOT be used unless GetSigChanged returns true.</summary>
		public bool GetSigIsTopaz(){
			//if(allowTopaz && sigBoxTopaz.Visible){
			if(sigBoxTopaz.Visible){
				return true;
			}
			return false;
		}

		/*
		///<summary></summary>
		public bool GetSigIsValid(){
			if(labelInvalidSig.Visible){
				return false;
			}
			return true;
		}*/

		///<summary>This should happen a lot before the box is signed.  Once it's signed, if this happens, then the signature will be invalidated.  The user would have to clear the invalidation manually.  This "invalidation" is just a visual cue; nothing actually happens to the data.</summary>
		public void SetInvalid(){
			//if(allowTopaz && sigBoxTopaz.Visible){
			if(sigBoxTopaz.Visible){
				if(CodeBase.TopazWrapper.GetTopazNumberOfTabletPoints(sigBoxTopaz)==0){
					return;//no need to do anything because no signature
				}
			}
			else{
				if(sigBox.NumberOfTabletPoints()==0) {
					return;//no need to do anything because no signature
				}
			}
			labelInvalidSig.Visible=true;
			labelInvalidSig.BringToFront();
		}

		public bool IsValid{
			get { return (!labelInvalidSig.Visible); }
		}

		public bool SigIsBlank{
			get{ 
				//if(allowTopaz && sigBoxTopaz.Visible){
				if(sigBoxTopaz.Visible){
					return(CodeBase.TopazWrapper.GetTopazNumberOfTabletPoints(sigBoxTopaz)==0);
				}
				return(sigBox.NumberOfTabletPoints()==0);
			}
		}

		///<summary>This should NOT be used unless GetSigChanged returns true.</summary>
		public string GetSignature(string keyData){
			//Topaz boxes are written in Windows native code.
			//if(allowTopaz && sigBoxTopaz.Visible){
			if(sigBoxTopaz.Visible){
				//ProcCur.SigIsTopaz=true;
				if(CodeBase.TopazWrapper.GetTopazNumberOfTabletPoints(sigBoxTopaz)==0){
					return "";
				}
				CodeBase.TopazWrapper.SetTopazCompressionMode(sigBoxTopaz,0);
				CodeBase.TopazWrapper.SetTopazEncryptionMode(sigBoxTopaz,0);
				//Some places used different logic for keystring and autokeydata in the past.  This must be maintained to keep signatures valid.
				switch(SignatureMode) {
					case SigMode.Document:
					case SigMode.TreatPlan:
						//No auto key data set, only key string.
						CodeBase.TopazWrapper.SetTopazKeyString(sigBoxTopaz,keyData);
						break;
					case SigMode.Default:
						CodeBase.TopazWrapper.SetTopazKeyString(sigBoxTopaz,"0000000000000000");
						CodeBase.TopazWrapper.SetTopazAutoKeyData(sigBoxTopaz,keyData);
						break;
				}
				CodeBase.TopazWrapper.SetTopazEncryptionMode(sigBoxTopaz,2);
				CodeBase.TopazWrapper.SetTopazCompressionMode(sigBoxTopaz,2);
				return CodeBase.TopazWrapper.GetTopazString(sigBoxTopaz);
			}
			else{
				//ProcCur.SigIsTopaz=false;
				if(sigBox.NumberOfTabletPoints()==0) {
					return "";
				}
				//sigBox.SetSigCompressionMode(0);
				//sigBox.SetEncryptionMode(0);
				//Some places used different logic for keystring and autokeydata in the past.  This must be maintained to keep signatures valid.
				switch(SignatureMode) {
					case SigMode.Document:
					case SigMode.TreatPlan:
						//No auto key data set, only key string.
						sigBox.SetKeyString(keyData);
						break;
					case SigMode.Default:
						sigBox.SetKeyString("0000000000000000");
						sigBox.SetAutoKeyData(keyData);
						break;
				}
				//sigBox.SetEncryptionMode(2);
				//sigBox.SetSigCompressionMode(2);
				return sigBox.GetSigString();
			}
		}

		private void butClearSig_Click(object sender,EventArgs e) {
			if(ClearSignatureClicked!=null) {
				ClearSignatureClicked(this,new EventArgs());
			}
			ClearSignature();
			OnSignatureChanged();
		}

		private void butTopazSign_Click(object sender,EventArgs e) {
			//this button is not even visible if Topaz is not allowed
			if(SignTopazClicked!=null) {
				SignTopazClicked(this,new EventArgs());
			}
			sigBox.Visible=false;
			sigBoxTopaz.Visible=true;
			//if(allowTopaz){
				CodeBase.TopazWrapper.ClearTopaz(sigBoxTopaz);
				CodeBase.TopazWrapper.SetTopazEncryptionMode(sigBoxTopaz,0);
				CodeBase.TopazWrapper.SetTopazState(sigBoxTopaz,1);
			//}
			labelInvalidSig.Visible=false;
			sigBoxTopaz.Focus();
			OnSignatureChanged();
		}

		private void sigBox_MouseUp(object sender,MouseEventArgs e) {
			//this is done on mouse up so that the initial pen capture won't be delayed.
			if(sigBox.GetTabletState()==1//if accepting input.
				&& !sigChanged)//and sig not changed yet
			{
				//sigBox handles its own pen input.
				OnSignatureChanged();
			}
		}

		private void sigBoxTopaz_Leave(object sender,EventArgs e) {
			if(CodeBase.TopazWrapper.GetTopazState(sigBoxTopaz)==1) {//if accepting input.
				CodeBase.TopazWrapper.SetTopazState(sigBoxTopaz,0);
			}
		}

		///<summary>Must set width and height of control and run FillSignature first.</summary>
		public Bitmap GetSigImage(){
			Bitmap sigBitmap=new Bitmap(Width-2,Height-2);
			//no outline
			//if(allowTopaz && sigBoxTopaz.Visible){
			if(sigBoxTopaz.Visible) {
				sigBoxTopaz.DrawToBitmap(sigBitmap,new Rectangle(0,0,Width-2,Height-2));//GetBitmap would probably work.
			}
			else {
				sigBitmap=(Bitmap)sigBox.GetSigImage(false);
			}
			return sigBitmap;
		}

		///<summary>If this is called externally, then the event SignatureChanged will also fire.</summary>
		public void ClearSignature(){
			sigBox.ClearTablet();
			sigBox.Visible=true;
			//if(allowTopaz) {
			  CodeBase.TopazWrapper.ClearTopaz(sigBoxTopaz);
			  sigBoxTopaz.Visible=false;//until user explicitly starts it.
			//}
			sigBox.SetTabletState(1);//on-screen box is now accepting input.
			sigChanged=true;
			labelInvalidSig.Visible=false;
			OnSignatureChanged();
		}

		public void SetControlSigBoxTopaz(Control sigBoxTopaz) {
			this.sigBoxTopaz=sigBox;
		}









		///<summary></summary>
		public enum SigMode {
			///<summary>Default case</summary>
			Default,
			///<summary>Signatures used for documents in the Images module.</summary>
			Document,
			///<summary>Signatures used for treatment plans.</summary>
			TreatPlan
		}
	}
}
