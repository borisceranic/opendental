﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.34209.
// 
#pragma warning disable 1591

namespace OpenDentBusiness.WebServiceMainHQ {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WebServiceMainHQSoap", Namespace="https://www.opendental.com/OpenDentalWebServiceHQ/")]
    public partial class WebServiceMainHQ : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback PerformRefreshCacheOperationCompleted;
        
        private System.Threading.SendOrPostCallback GenerateWebAppUrlOperationCompleted;
        
        private System.Threading.SendOrPostCallback ValidateWebAppUrlOperationCompleted;
        
        private System.Threading.SendOrPostCallback SmsSendOperationCompleted;
        
        private System.Threading.SendOrPostCallback SmsSignAgreementOperationCompleted;
        
        private System.Threading.SendOrPostCallback SmsCancelServiceOperationCompleted;
        
        private System.Threading.SendOrPostCallback RequestListenerProxyPrefsOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WebServiceMainHQ() {
            this.Url = global::OpenDentBusiness.Properties.Settings.Default.OpenDentBusiness_WebServiceMainHQ_WebServiceMainHQ;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event PerformRefreshCacheCompletedEventHandler PerformRefreshCacheCompleted;
        
        /// <remarks/>
        public event GenerateWebAppUrlCompletedEventHandler GenerateWebAppUrlCompleted;
        
        /// <remarks/>
        public event ValidateWebAppUrlCompletedEventHandler ValidateWebAppUrlCompleted;
        
        /// <remarks/>
        public event SmsSendCompletedEventHandler SmsSendCompleted;
        
        /// <remarks/>
        public event SmsSignAgreementCompletedEventHandler SmsSignAgreementCompleted;
        
        /// <remarks/>
        public event SmsCancelServiceCompletedEventHandler SmsCancelServiceCompleted;
        
        /// <remarks/>
        public event RequestListenerProxyPrefsCompletedEventHandler RequestListenerProxyPrefsCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://www.opendental.com/OpenDentalWebServiceHQ/PerformRefreshCache", RequestNamespace="https://www.opendental.com/OpenDentalWebServiceHQ/", ResponseNamespace="https://www.opendental.com/OpenDentalWebServiceHQ/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string PerformRefreshCache(string officeData) {
            object[] results = this.Invoke("PerformRefreshCache", new object[] {
                        officeData});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void PerformRefreshCacheAsync(string officeData) {
            this.PerformRefreshCacheAsync(officeData, null);
        }
        
        /// <remarks/>
        public void PerformRefreshCacheAsync(string officeData, object userState) {
            if ((this.PerformRefreshCacheOperationCompleted == null)) {
                this.PerformRefreshCacheOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPerformRefreshCacheOperationCompleted);
            }
            this.InvokeAsync("PerformRefreshCache", new object[] {
                        officeData}, this.PerformRefreshCacheOperationCompleted, userState);
        }
        
        private void OnPerformRefreshCacheOperationCompleted(object arg) {
            if ((this.PerformRefreshCacheCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.PerformRefreshCacheCompleted(this, new PerformRefreshCacheCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://www.opendental.com/OpenDentalWebServiceHQ/GenerateWebAppUrl", RequestNamespace="https://www.opendental.com/OpenDentalWebServiceHQ/", ResponseNamespace="https://www.opendental.com/OpenDentalWebServiceHQ/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GenerateWebAppUrl(string officeData) {
            object[] results = this.Invoke("GenerateWebAppUrl", new object[] {
                        officeData});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GenerateWebAppUrlAsync(string officeData) {
            this.GenerateWebAppUrlAsync(officeData, null);
        }
        
        /// <remarks/>
        public void GenerateWebAppUrlAsync(string officeData, object userState) {
            if ((this.GenerateWebAppUrlOperationCompleted == null)) {
                this.GenerateWebAppUrlOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGenerateWebAppUrlOperationCompleted);
            }
            this.InvokeAsync("GenerateWebAppUrl", new object[] {
                        officeData}, this.GenerateWebAppUrlOperationCompleted, userState);
        }
        
        private void OnGenerateWebAppUrlOperationCompleted(object arg) {
            if ((this.GenerateWebAppUrlCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GenerateWebAppUrlCompleted(this, new GenerateWebAppUrlCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://www.opendental.com/OpenDentalWebServiceHQ/ValidateWebAppUrl", RequestNamespace="https://www.opendental.com/OpenDentalWebServiceHQ/", ResponseNamespace="https://www.opendental.com/OpenDentalWebServiceHQ/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ValidateWebAppUrl(string officeData) {
            object[] results = this.Invoke("ValidateWebAppUrl", new object[] {
                        officeData});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ValidateWebAppUrlAsync(string officeData) {
            this.ValidateWebAppUrlAsync(officeData, null);
        }
        
        /// <remarks/>
        public void ValidateWebAppUrlAsync(string officeData, object userState) {
            if ((this.ValidateWebAppUrlOperationCompleted == null)) {
                this.ValidateWebAppUrlOperationCompleted = new System.Threading.SendOrPostCallback(this.OnValidateWebAppUrlOperationCompleted);
            }
            this.InvokeAsync("ValidateWebAppUrl", new object[] {
                        officeData}, this.ValidateWebAppUrlOperationCompleted, userState);
        }
        
        private void OnValidateWebAppUrlOperationCompleted(object arg) {
            if ((this.ValidateWebAppUrlCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ValidateWebAppUrlCompleted(this, new ValidateWebAppUrlCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://www.opendental.com/OpenDentalWebServiceHQ/SmsSend", RequestNamespace="https://www.opendental.com/OpenDentalWebServiceHQ/", ResponseNamespace="https://www.opendental.com/OpenDentalWebServiceHQ/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SmsSend(string officeData) {
            object[] results = this.Invoke("SmsSend", new object[] {
                        officeData});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void SmsSendAsync(string officeData) {
            this.SmsSendAsync(officeData, null);
        }
        
        /// <remarks/>
        public void SmsSendAsync(string officeData, object userState) {
            if ((this.SmsSendOperationCompleted == null)) {
                this.SmsSendOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSmsSendOperationCompleted);
            }
            this.InvokeAsync("SmsSend", new object[] {
                        officeData}, this.SmsSendOperationCompleted, userState);
        }
        
        private void OnSmsSendOperationCompleted(object arg) {
            if ((this.SmsSendCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SmsSendCompleted(this, new SmsSendCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://www.opendental.com/OpenDentalWebServiceHQ/SmsSignAgreement", RequestNamespace="https://www.opendental.com/OpenDentalWebServiceHQ/", ResponseNamespace="https://www.opendental.com/OpenDentalWebServiceHQ/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SmsSignAgreement(string officeData) {
            object[] results = this.Invoke("SmsSignAgreement", new object[] {
                        officeData});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void SmsSignAgreementAsync(string officeData) {
            this.SmsSignAgreementAsync(officeData, null);
        }
        
        /// <remarks/>
        public void SmsSignAgreementAsync(string officeData, object userState) {
            if ((this.SmsSignAgreementOperationCompleted == null)) {
                this.SmsSignAgreementOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSmsSignAgreementOperationCompleted);
            }
            this.InvokeAsync("SmsSignAgreement", new object[] {
                        officeData}, this.SmsSignAgreementOperationCompleted, userState);
        }
        
        private void OnSmsSignAgreementOperationCompleted(object arg) {
            if ((this.SmsSignAgreementCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SmsSignAgreementCompleted(this, new SmsSignAgreementCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://www.opendental.com/OpenDentalWebServiceHQ/SmsCancelService", RequestNamespace="https://www.opendental.com/OpenDentalWebServiceHQ/", ResponseNamespace="https://www.opendental.com/OpenDentalWebServiceHQ/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SmsCancelService(string officeData) {
            object[] results = this.Invoke("SmsCancelService", new object[] {
                        officeData});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void SmsCancelServiceAsync(string officeData) {
            this.SmsCancelServiceAsync(officeData, null);
        }
        
        /// <remarks/>
        public void SmsCancelServiceAsync(string officeData, object userState) {
            if ((this.SmsCancelServiceOperationCompleted == null)) {
                this.SmsCancelServiceOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSmsCancelServiceOperationCompleted);
            }
            this.InvokeAsync("SmsCancelService", new object[] {
                        officeData}, this.SmsCancelServiceOperationCompleted, userState);
        }
        
        private void OnSmsCancelServiceOperationCompleted(object arg) {
            if ((this.SmsCancelServiceCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SmsCancelServiceCompleted(this, new SmsCancelServiceCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://www.opendental.com/OpenDentalWebServiceHQ/RequestListenerProxyPrefs", RequestNamespace="https://www.opendental.com/OpenDentalWebServiceHQ/", ResponseNamespace="https://www.opendental.com/OpenDentalWebServiceHQ/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string RequestListenerProxyPrefs(string officeData) {
            object[] results = this.Invoke("RequestListenerProxyPrefs", new object[] {
                        officeData});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void RequestListenerProxyPrefsAsync(string officeData) {
            this.RequestListenerProxyPrefsAsync(officeData, null);
        }
        
        /// <remarks/>
        public void RequestListenerProxyPrefsAsync(string officeData, object userState) {
            if ((this.RequestListenerProxyPrefsOperationCompleted == null)) {
                this.RequestListenerProxyPrefsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRequestListenerProxyPrefsOperationCompleted);
            }
            this.InvokeAsync("RequestListenerProxyPrefs", new object[] {
                        officeData}, this.RequestListenerProxyPrefsOperationCompleted, userState);
        }
        
        private void OnRequestListenerProxyPrefsOperationCompleted(object arg) {
            if ((this.RequestListenerProxyPrefsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RequestListenerProxyPrefsCompleted(this, new RequestListenerProxyPrefsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void PerformRefreshCacheCompletedEventHandler(object sender, PerformRefreshCacheCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PerformRefreshCacheCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal PerformRefreshCacheCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void GenerateWebAppUrlCompletedEventHandler(object sender, GenerateWebAppUrlCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GenerateWebAppUrlCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GenerateWebAppUrlCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void ValidateWebAppUrlCompletedEventHandler(object sender, ValidateWebAppUrlCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ValidateWebAppUrlCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ValidateWebAppUrlCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void SmsSendCompletedEventHandler(object sender, SmsSendCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SmsSendCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SmsSendCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void SmsSignAgreementCompletedEventHandler(object sender, SmsSignAgreementCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SmsSignAgreementCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SmsSignAgreementCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void SmsCancelServiceCompletedEventHandler(object sender, SmsCancelServiceCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SmsCancelServiceCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SmsCancelServiceCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void RequestListenerProxyPrefsCompletedEventHandler(object sender, RequestListenerProxyPrefsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RequestListenerProxyPrefsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RequestListenerProxyPrefsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591