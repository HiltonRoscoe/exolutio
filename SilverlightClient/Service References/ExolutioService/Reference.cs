﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 4.0.50826.0
// 
namespace SilverlightClient.ExolutioService {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.CollectionDataContractAttribute(Name="ArrayOfString", Namespace="http://tempuri.org/", ItemName="string")]
    public class ArrayOfString : System.Collections.ObjectModel.ObservableCollection<string> {
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ExolutioService.ProjectFilesServiceSoap")]
    public interface ProjectFilesServiceSoap {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/HelloWorld", ReplyAction="*")]
        System.IAsyncResult BeginHelloWorld(SilverlightClient.ExolutioService.HelloWorldRequest request, System.AsyncCallback callback, object asyncState);
        
        SilverlightClient.ExolutioService.HelloWorldResponse EndHelloWorld(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/GetProjectFiles", ReplyAction="*")]
        System.IAsyncResult BeginGetProjectFiles(SilverlightClient.ExolutioService.GetProjectFilesRequest request, System.AsyncCallback callback, object asyncState);
        
        SilverlightClient.ExolutioService.GetProjectFilesResponse EndGetProjectFiles(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/GetProjectFile", ReplyAction="*")]
        System.IAsyncResult BeginGetProjectFile(SilverlightClient.ExolutioService.GetProjectFileRequest request, System.AsyncCallback callback, object asyncState);
        
        SilverlightClient.ExolutioService.GetProjectFileResponse EndGetProjectFile(System.IAsyncResult result);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class HelloWorldRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="HelloWorld", Namespace="http://tempuri.org/", Order=0)]
        public SilverlightClient.ExolutioService.HelloWorldRequestBody Body;
        
        public HelloWorldRequest() {
        }
        
        public HelloWorldRequest(SilverlightClient.ExolutioService.HelloWorldRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class HelloWorldRequestBody {
        
        public HelloWorldRequestBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class HelloWorldResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="HelloWorldResponse", Namespace="http://tempuri.org/", Order=0)]
        public SilverlightClient.ExolutioService.HelloWorldResponseBody Body;
        
        public HelloWorldResponse() {
        }
        
        public HelloWorldResponse(SilverlightClient.ExolutioService.HelloWorldResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class HelloWorldResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string HelloWorldResult;
        
        public HelloWorldResponseBody() {
        }
        
        public HelloWorldResponseBody(string HelloWorldResult) {
            this.HelloWorldResult = HelloWorldResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetProjectFilesRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetProjectFiles", Namespace="http://tempuri.org/", Order=0)]
        public SilverlightClient.ExolutioService.GetProjectFilesRequestBody Body;
        
        public GetProjectFilesRequest() {
        }
        
        public GetProjectFilesRequest(SilverlightClient.ExolutioService.GetProjectFilesRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class GetProjectFilesRequestBody {
        
        public GetProjectFilesRequestBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetProjectFilesResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetProjectFilesResponse", Namespace="http://tempuri.org/", Order=0)]
        public SilverlightClient.ExolutioService.GetProjectFilesResponseBody Body;
        
        public GetProjectFilesResponse() {
        }
        
        public GetProjectFilesResponse(SilverlightClient.ExolutioService.GetProjectFilesResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetProjectFilesResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public SilverlightClient.ExolutioService.ArrayOfString GetProjectFilesResult;
        
        public GetProjectFilesResponseBody() {
        }
        
        public GetProjectFilesResponseBody(SilverlightClient.ExolutioService.ArrayOfString GetProjectFilesResult) {
            this.GetProjectFilesResult = GetProjectFilesResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetProjectFileRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetProjectFile", Namespace="http://tempuri.org/", Order=0)]
        public SilverlightClient.ExolutioService.GetProjectFileRequestBody Body;
        
        public GetProjectFileRequest() {
        }
        
        public GetProjectFileRequest(SilverlightClient.ExolutioService.GetProjectFileRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetProjectFileRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string filename;
        
        public GetProjectFileRequestBody() {
        }
        
        public GetProjectFileRequestBody(string filename) {
            this.filename = filename;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetProjectFileResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetProjectFileResponse", Namespace="http://tempuri.org/", Order=0)]
        public SilverlightClient.ExolutioService.GetProjectFileResponseBody Body;
        
        public GetProjectFileResponse() {
        }
        
        public GetProjectFileResponse(SilverlightClient.ExolutioService.GetProjectFileResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetProjectFileResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public byte[] GetProjectFileResult;
        
        public GetProjectFileResponseBody() {
        }
        
        public GetProjectFileResponseBody(byte[] GetProjectFileResult) {
            this.GetProjectFileResult = GetProjectFileResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ProjectFilesServiceSoapChannel : SilverlightClient.ExolutioService.ProjectFilesServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class HelloWorldCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public HelloWorldCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetProjectFilesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetProjectFilesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public SilverlightClient.ExolutioService.ArrayOfString Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((SilverlightClient.ExolutioService.ArrayOfString)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetProjectFileCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetProjectFileCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public byte[] Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((byte[])(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ProjectFilesServiceSoapClient : System.ServiceModel.ClientBase<SilverlightClient.ExolutioService.ProjectFilesServiceSoap>, SilverlightClient.ExolutioService.ProjectFilesServiceSoap {
        
        private BeginOperationDelegate onBeginHelloWorldDelegate;
        
        private EndOperationDelegate onEndHelloWorldDelegate;
        
        private System.Threading.SendOrPostCallback onHelloWorldCompletedDelegate;
        
        private BeginOperationDelegate onBeginGetProjectFilesDelegate;
        
        private EndOperationDelegate onEndGetProjectFilesDelegate;
        
        private System.Threading.SendOrPostCallback onGetProjectFilesCompletedDelegate;
        
        private BeginOperationDelegate onBeginGetProjectFileDelegate;
        
        private EndOperationDelegate onEndGetProjectFileDelegate;
        
        private System.Threading.SendOrPostCallback onGetProjectFileCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public ProjectFilesServiceSoapClient() {
        }
        
        public ProjectFilesServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ProjectFilesServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ProjectFilesServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ProjectFilesServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<HelloWorldCompletedEventArgs> HelloWorldCompleted;
        
        public event System.EventHandler<GetProjectFilesCompletedEventArgs> GetProjectFilesCompleted;
        
        public event System.EventHandler<GetProjectFileCompletedEventArgs> GetProjectFileCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult SilverlightClient.ExolutioService.ProjectFilesServiceSoap.BeginHelloWorld(SilverlightClient.ExolutioService.HelloWorldRequest request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginHelloWorld(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        private System.IAsyncResult BeginHelloWorld(System.AsyncCallback callback, object asyncState) {
            SilverlightClient.ExolutioService.HelloWorldRequest inValue = new SilverlightClient.ExolutioService.HelloWorldRequest();
            inValue.Body = new SilverlightClient.ExolutioService.HelloWorldRequestBody();
            return ((SilverlightClient.ExolutioService.ProjectFilesServiceSoap)(this)).BeginHelloWorld(inValue, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SilverlightClient.ExolutioService.HelloWorldResponse SilverlightClient.ExolutioService.ProjectFilesServiceSoap.EndHelloWorld(System.IAsyncResult result) {
            return base.Channel.EndHelloWorld(result);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        private string EndHelloWorld(System.IAsyncResult result) {
            SilverlightClient.ExolutioService.HelloWorldResponse retVal = ((SilverlightClient.ExolutioService.ProjectFilesServiceSoap)(this)).EndHelloWorld(result);
            return retVal.Body.HelloWorldResult;
        }
        
        private System.IAsyncResult OnBeginHelloWorld(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return this.BeginHelloWorld(callback, asyncState);
        }
        
        private object[] OnEndHelloWorld(System.IAsyncResult result) {
            string retVal = this.EndHelloWorld(result);
            return new object[] {
                    retVal};
        }
        
        private void OnHelloWorldCompleted(object state) {
            if ((this.HelloWorldCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.HelloWorldCompleted(this, new HelloWorldCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void HelloWorldAsync() {
            this.HelloWorldAsync(null);
        }
        
        public void HelloWorldAsync(object userState) {
            if ((this.onBeginHelloWorldDelegate == null)) {
                this.onBeginHelloWorldDelegate = new BeginOperationDelegate(this.OnBeginHelloWorld);
            }
            if ((this.onEndHelloWorldDelegate == null)) {
                this.onEndHelloWorldDelegate = new EndOperationDelegate(this.OnEndHelloWorld);
            }
            if ((this.onHelloWorldCompletedDelegate == null)) {
                this.onHelloWorldCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnHelloWorldCompleted);
            }
            base.InvokeAsync(this.onBeginHelloWorldDelegate, null, this.onEndHelloWorldDelegate, this.onHelloWorldCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult SilverlightClient.ExolutioService.ProjectFilesServiceSoap.BeginGetProjectFiles(SilverlightClient.ExolutioService.GetProjectFilesRequest request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetProjectFiles(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        private System.IAsyncResult BeginGetProjectFiles(System.AsyncCallback callback, object asyncState) {
            SilverlightClient.ExolutioService.GetProjectFilesRequest inValue = new SilverlightClient.ExolutioService.GetProjectFilesRequest();
            inValue.Body = new SilverlightClient.ExolutioService.GetProjectFilesRequestBody();
            return ((SilverlightClient.ExolutioService.ProjectFilesServiceSoap)(this)).BeginGetProjectFiles(inValue, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SilverlightClient.ExolutioService.GetProjectFilesResponse SilverlightClient.ExolutioService.ProjectFilesServiceSoap.EndGetProjectFiles(System.IAsyncResult result) {
            return base.Channel.EndGetProjectFiles(result);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        private SilverlightClient.ExolutioService.ArrayOfString EndGetProjectFiles(System.IAsyncResult result) {
            SilverlightClient.ExolutioService.GetProjectFilesResponse retVal = ((SilverlightClient.ExolutioService.ProjectFilesServiceSoap)(this)).EndGetProjectFiles(result);
            return retVal.Body.GetProjectFilesResult;
        }
        
        private System.IAsyncResult OnBeginGetProjectFiles(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return this.BeginGetProjectFiles(callback, asyncState);
        }
        
        private object[] OnEndGetProjectFiles(System.IAsyncResult result) {
            SilverlightClient.ExolutioService.ArrayOfString retVal = this.EndGetProjectFiles(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetProjectFilesCompleted(object state) {
            if ((this.GetProjectFilesCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetProjectFilesCompleted(this, new GetProjectFilesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetProjectFilesAsync() {
            this.GetProjectFilesAsync(null);
        }
        
        public void GetProjectFilesAsync(object userState) {
            if ((this.onBeginGetProjectFilesDelegate == null)) {
                this.onBeginGetProjectFilesDelegate = new BeginOperationDelegate(this.OnBeginGetProjectFiles);
            }
            if ((this.onEndGetProjectFilesDelegate == null)) {
                this.onEndGetProjectFilesDelegate = new EndOperationDelegate(this.OnEndGetProjectFiles);
            }
            if ((this.onGetProjectFilesCompletedDelegate == null)) {
                this.onGetProjectFilesCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetProjectFilesCompleted);
            }
            base.InvokeAsync(this.onBeginGetProjectFilesDelegate, null, this.onEndGetProjectFilesDelegate, this.onGetProjectFilesCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult SilverlightClient.ExolutioService.ProjectFilesServiceSoap.BeginGetProjectFile(SilverlightClient.ExolutioService.GetProjectFileRequest request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetProjectFile(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        private System.IAsyncResult BeginGetProjectFile(string filename, System.AsyncCallback callback, object asyncState) {
            SilverlightClient.ExolutioService.GetProjectFileRequest inValue = new SilverlightClient.ExolutioService.GetProjectFileRequest();
            inValue.Body = new SilverlightClient.ExolutioService.GetProjectFileRequestBody();
            inValue.Body.filename = filename;
            return ((SilverlightClient.ExolutioService.ProjectFilesServiceSoap)(this)).BeginGetProjectFile(inValue, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SilverlightClient.ExolutioService.GetProjectFileResponse SilverlightClient.ExolutioService.ProjectFilesServiceSoap.EndGetProjectFile(System.IAsyncResult result) {
            return base.Channel.EndGetProjectFile(result);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        private byte[] EndGetProjectFile(System.IAsyncResult result) {
            SilverlightClient.ExolutioService.GetProjectFileResponse retVal = ((SilverlightClient.ExolutioService.ProjectFilesServiceSoap)(this)).EndGetProjectFile(result);
            return retVal.Body.GetProjectFileResult;
        }
        
        private System.IAsyncResult OnBeginGetProjectFile(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string filename = ((string)(inValues[0]));
            return this.BeginGetProjectFile(filename, callback, asyncState);
        }
        
        private object[] OnEndGetProjectFile(System.IAsyncResult result) {
            byte[] retVal = this.EndGetProjectFile(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetProjectFileCompleted(object state) {
            if ((this.GetProjectFileCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetProjectFileCompleted(this, new GetProjectFileCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetProjectFileAsync(string filename) {
            this.GetProjectFileAsync(filename, null);
        }
        
        public void GetProjectFileAsync(string filename, object userState) {
            if ((this.onBeginGetProjectFileDelegate == null)) {
                this.onBeginGetProjectFileDelegate = new BeginOperationDelegate(this.OnBeginGetProjectFile);
            }
            if ((this.onEndGetProjectFileDelegate == null)) {
                this.onEndGetProjectFileDelegate = new EndOperationDelegate(this.OnEndGetProjectFile);
            }
            if ((this.onGetProjectFileCompletedDelegate == null)) {
                this.onGetProjectFileCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetProjectFileCompleted);
            }
            base.InvokeAsync(this.onBeginGetProjectFileDelegate, new object[] {
                        filename}, this.onEndGetProjectFileDelegate, this.onGetProjectFileCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override SilverlightClient.ExolutioService.ProjectFilesServiceSoap CreateChannel() {
            return new ProjectFilesServiceSoapClientChannel(this);
        }
        
        private class ProjectFilesServiceSoapClientChannel : ChannelBase<SilverlightClient.ExolutioService.ProjectFilesServiceSoap>, SilverlightClient.ExolutioService.ProjectFilesServiceSoap {
            
            public ProjectFilesServiceSoapClientChannel(System.ServiceModel.ClientBase<SilverlightClient.ExolutioService.ProjectFilesServiceSoap> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginHelloWorld(SilverlightClient.ExolutioService.HelloWorldRequest request, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = request;
                System.IAsyncResult _result = base.BeginInvoke("HelloWorld", _args, callback, asyncState);
                return _result;
            }
            
            public SilverlightClient.ExolutioService.HelloWorldResponse EndHelloWorld(System.IAsyncResult result) {
                object[] _args = new object[0];
                SilverlightClient.ExolutioService.HelloWorldResponse _result = ((SilverlightClient.ExolutioService.HelloWorldResponse)(base.EndInvoke("HelloWorld", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginGetProjectFiles(SilverlightClient.ExolutioService.GetProjectFilesRequest request, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = request;
                System.IAsyncResult _result = base.BeginInvoke("GetProjectFiles", _args, callback, asyncState);
                return _result;
            }
            
            public SilverlightClient.ExolutioService.GetProjectFilesResponse EndGetProjectFiles(System.IAsyncResult result) {
                object[] _args = new object[0];
                SilverlightClient.ExolutioService.GetProjectFilesResponse _result = ((SilverlightClient.ExolutioService.GetProjectFilesResponse)(base.EndInvoke("GetProjectFiles", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginGetProjectFile(SilverlightClient.ExolutioService.GetProjectFileRequest request, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = request;
                System.IAsyncResult _result = base.BeginInvoke("GetProjectFile", _args, callback, asyncState);
                return _result;
            }
            
            public SilverlightClient.ExolutioService.GetProjectFileResponse EndGetProjectFile(System.IAsyncResult result) {
                object[] _args = new object[0];
                SilverlightClient.ExolutioService.GetProjectFileResponse _result = ((SilverlightClient.ExolutioService.GetProjectFileResponse)(base.EndInvoke("GetProjectFile", _args, result)));
                return _result;
            }
        }
    }
}