using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;
using OpenDentBusiness;

namespace OpenDentalServer {
	/// <summary></summary>
	[WebService(Namespace="http://www.open-dent.com/OpenDentalServer")]
	[WebServiceBinding(ConformsTo=WsiProfiles.BasicProfile1_1)]
	[ToolboxItem(false)]
	public class ServiceMain:System.Web.Services.WebService {

		/// <summary>Pass in a serialized dto.  It returns a dto which must be deserialized by the client.</summary>
		[WebMethod]
		public string ProcessRequest(string dtoString) {
			//The web service (xml) serializer/deserializer is removing the '\r' portion of our newlines during the data transfer. 
			//Replacing the string is not the best solution but it works for now. The replacing happens here (server side) and after result is returned on the client side.
			//It's done server side for usage purposes within the methods being called (exampe: inserting into db) and then on the client side for displaying purposes.
			dtoString=dtoString.Replace("\n","\r\n");
			#if DEBUG
				//System.Threading.Thread.Sleep(100);//to test slowness issues with web service.
			#endif
			DataTransferObject dto=DataTransferObject.Deserialize(dtoString);
			//XmlSerializer serializer;
			try {
				Type type = dto.GetType();
				if(type == typeof(DtoGetTable)) {
					DtoGetTable dtoGetTable=(DtoGetTable)dto;
					Userods.CheckCredentials(dtoGetTable.Credentials);//will throw exception if fails.
					string[] fullNameComponents=GetComponentsFromDtoMeth(dtoGetTable.MethodName);
					string assemblyName=fullNameComponents[0];//OpenDentBusiness or else a plugin name
					string className=fullNameComponents[1];
					string methodName=fullNameComponents[2];
					Type classType=null;
					Assembly ass=Plugins.GetAssembly(assemblyName);
					if(ass==null) {
						classType=Type.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className+","+assemblyName);
					}
					else {//plugin was found
						classType=ass.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className);
					}
					DtoObject[] parameters=dtoGetTable.Params;
					Type[] paramTypes=DtoObject.GenerateTypes(parameters,assemblyName);
					MethodInfo methodInfo=classType.GetMethod(methodName,paramTypes);
					if(methodInfo==null) {
						throw new ApplicationException("Method not found with "+parameters.Length.ToString()+" parameters: "+dtoGetTable.MethodName);
					}
					object[] paramObjs=DtoObject.GenerateObjects(parameters);
					DataTable dt=(DataTable)methodInfo.Invoke(null,paramObjs);
					String response=XmlConverter.TableToXml(dt);
					return response;
				}
				else if(type == typeof(DtoGetTableLow)) {
					DtoGetTableLow dtoGetTableLow=(DtoGetTableLow)dto;
					Userods.CheckCredentials(dtoGetTableLow.Credentials);//will throw exception if fails.
					DtoObject[] parameters=dtoGetTableLow.Params;
					object[] paramObjs=DtoObject.GenerateObjects(parameters);
					DataTable dt=Reports.GetTable((string)paramObjs[0]);
					String response=XmlConverter.TableToXml(dt);
					return response;
				}
				else if(type == typeof(DtoGetDS)) {
					DtoGetDS dtoGetDS=(DtoGetDS)dto;
					Userods.CheckCredentials(dtoGetDS.Credentials);//will throw exception if fails.
					string[] fullNameComponents=GetComponentsFromDtoMeth(dtoGetDS.MethodName);
					string assemblyName=fullNameComponents[0];//OpenDentBusiness or else a plugin name
					string className=fullNameComponents[1];
					string methodName=fullNameComponents[2];
					Type classType=null;
					Assembly ass=Plugins.GetAssembly(assemblyName);
					if(ass==null) {
						classType=Type.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className+","+assemblyName);
					}
					else {//plugin was found
						classType=ass.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className);
					}
					DtoObject[] parameters=dtoGetDS.Params;
					Type[] paramTypes=DtoObject.GenerateTypes(parameters,assemblyName);
					MethodInfo methodInfo=classType.GetMethod(methodName,paramTypes);
					if(methodInfo==null) {
						throw new ApplicationException("Method not found with "+parameters.Length.ToString()+" parameters: "+dtoGetDS.MethodName);
					}
					object[] paramObjs=DtoObject.GenerateObjects(parameters);
					DataSet ds=(DataSet)methodInfo.Invoke(null,paramObjs);
					String response=XmlConverter.DsToXml(ds);
					return response;
				}
				else if(type == typeof(DtoGetLong)) {
					DtoGetLong dtoGetLong=(DtoGetLong)dto;
					Userods.CheckCredentials(dtoGetLong.Credentials);//will throw exception if fails.
					string[] fullNameComponents=GetComponentsFromDtoMeth(dtoGetLong.MethodName);
					string assemblyName=fullNameComponents[0];//OpenDentBusiness or else a plugin name
					string className=fullNameComponents[1];
					string methodName=fullNameComponents[2];
					Type classType=null;
					Assembly ass=Plugins.GetAssembly(assemblyName);
					if(ass==null) {
						classType=Type.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className+","+assemblyName);
					}
					else {//plugin was found
						classType=ass.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className);
					}
					DtoObject[] parameters=dtoGetLong.Params;
					Type[] paramTypes=DtoObject.GenerateTypes(parameters,assemblyName);
					MethodInfo methodInfo=classType.GetMethod(methodName,paramTypes);
					if(methodInfo==null) {
						throw new ApplicationException("Method not found with "+parameters.Length.ToString()+" parameters: "+dtoGetLong.MethodName);
					}
					object[] paramObjs=DtoObject.GenerateObjects(parameters);
					long longResult=(long)methodInfo.Invoke(null,paramObjs);
					return longResult.ToString();
				}
				else if(type == typeof(DtoGetInt)) {
					DtoGetInt dtoGetInt=(DtoGetInt)dto;
					Userods.CheckCredentials(dtoGetInt.Credentials);//will throw exception if fails.
					string[] fullNameComponents=GetComponentsFromDtoMeth(dtoGetInt.MethodName);
					string assemblyName=fullNameComponents[0];//OpenDentBusiness or else a plugin name
					string className=fullNameComponents[1];
					string methodName=fullNameComponents[2];
					Type classType=null;
					Assembly ass=Plugins.GetAssembly(assemblyName);
					if(ass==null) {
						classType=Type.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className+","+assemblyName);
					}
					else {//plugin was found
						classType=ass.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className);
					}
					DtoObject[] parameters=dtoGetInt.Params;
					Type[] paramTypes=DtoObject.GenerateTypes(parameters,assemblyName);
					MethodInfo methodInfo=classType.GetMethod(methodName,paramTypes);
					if(methodInfo==null) {
						throw new ApplicationException("Method not found with "+parameters.Length.ToString()+" parameters: "+dtoGetInt.MethodName);
					}
					object[] paramObjs=DtoObject.GenerateObjects(parameters);
					int intResult=(int)methodInfo.Invoke(null,paramObjs);
					return intResult.ToString();
				}
				else if(type == typeof(DtoGetDouble)) {
					DtoGetDouble dtoGetDouble=(DtoGetDouble)dto;
					Userods.CheckCredentials(dtoGetDouble.Credentials);//will throw exception if fails.
					string[] fullNameComponents=GetComponentsFromDtoMeth(dtoGetDouble.MethodName);
					string assemblyName=fullNameComponents[0];//OpenDentBusiness or else a plugin name
					string className=fullNameComponents[1];
					string methodName=fullNameComponents[2];
					Type classType=null;
					Assembly ass=Plugins.GetAssembly(assemblyName);
					if(ass==null) {
						classType=Type.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className+","+assemblyName);
					}
					else {//plugin was found
						classType=ass.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className);
					}
					DtoObject[] parameters=dtoGetDouble.Params;
					Type[] paramTypes=DtoObject.GenerateTypes(parameters,assemblyName);
					MethodInfo methodInfo=classType.GetMethod(methodName,paramTypes);
					if(methodInfo==null) {
						throw new ApplicationException("Method not found with "+parameters.Length.ToString()+" parameters: "+dtoGetDouble.MethodName);
					}
					object[] paramObjs=DtoObject.GenerateObjects(parameters);
					double doubleResult=(double)methodInfo.Invoke(null,paramObjs);
					return doubleResult.ToString();
				}
				else if(type == typeof(DtoGetVoid)) {
					DtoGetVoid dtoGetVoid=(DtoGetVoid)dto;
					Userods.CheckCredentials(dtoGetVoid.Credentials);//will throw exception if fails.
					string[] fullNameComponents=GetComponentsFromDtoMeth(dtoGetVoid.MethodName);
					string assemblyName=fullNameComponents[0];//OpenDentBusiness or else a plugin name
					string className=fullNameComponents[1];
					string methodName=fullNameComponents[2];
					Type classType=null;
					Assembly ass=Plugins.GetAssembly(assemblyName);
					if(ass==null) {
						classType=Type.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className+","+assemblyName);
					}
					else {//plugin was found
						classType=ass.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className);
					}
					DtoObject[] parameters=dtoGetVoid.Params;
					Type[] paramTypes=DtoObject.GenerateTypes(parameters,assemblyName);
					MethodInfo methodInfo=classType.GetMethod(methodName,paramTypes);
					if(methodInfo==null) {
						throw new ApplicationException("Method not found with "+parameters.Length.ToString()+" parameters: "+dtoGetVoid.MethodName);
					}
					object[] paramObjs=DtoObject.GenerateObjects(parameters);
					methodInfo.Invoke(null,paramObjs);
					return "0";
				}
				else if(type == typeof(DtoGetObject)) {
					DtoGetObject dtoGetObject=(DtoGetObject)dto;
					string[] fullNameComponents=GetComponentsFromDtoMeth(dtoGetObject.MethodName);
					string assemblyName=fullNameComponents[0];//OpenDentBusiness or else a plugin name
					string className=fullNameComponents[1];
					string methodName=fullNameComponents[2];
					if(className != "Security" || methodName != "LogInWeb") {//because credentials will be checked inside that method
						Userods.CheckCredentials(dtoGetObject.Credentials);//will throw exception if fails.
					}
					Type classType=null;
					Assembly ass=null;
					if(className!="Security" || methodName!="LogInWeb") {//Do this for everything except Security.LogInWeb, because Plugins.GetAssembly will fail in that case.
						ass=Plugins.GetAssembly(assemblyName);
					}
					if(ass==null) {
						classType=Type.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className+","+assemblyName);
					}
					else {//plugin was found
						classType=ass.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className);
					}
					DtoObject[] parameters=dtoGetObject.Params;
					Type[] paramTypes=DtoObject.GenerateTypes(parameters,assemblyName);
					MethodInfo methodInfo=classType.GetMethod(methodName,paramTypes);
					if(methodInfo==null) {
						throw new ApplicationException("Method not found with "+parameters.Length.ToString()+" parameters: "+dtoGetObject.MethodName);
					}
					if(className=="Security" && methodName=="LogInWeb") {
						string mappedPath=Server.MapPath(".");
						parameters[2]=new DtoObject(mappedPath,typeof(string));//because we can't access this variable from within OpenDentBusiness.
						RemotingClient.RemotingRole=RemotingRole.ServerWeb;
					}
					object[] paramObjs=DtoObject.GenerateObjects(parameters);
					Object objResult=methodInfo.Invoke(null,paramObjs);
					Type returnType=methodInfo.ReturnType;
					return XmlConverter.Serialize(returnType,objResult);
				}
				else if(type == typeof(DtoGetString)) {
					DtoGetString dtoGetString=(DtoGetString)dto;
					Userods.CheckCredentials(dtoGetString.Credentials);//will throw exception if fails.
					string[] fullNameComponents=GetComponentsFromDtoMeth(dtoGetString.MethodName);
					string assemblyName=fullNameComponents[0];//OpenDentBusiness or else a plugin name
					string className=fullNameComponents[1];
					string methodName=fullNameComponents[2];
					Type classType=null;
					Assembly ass=Plugins.GetAssembly(assemblyName);
					if(ass==null) {
						classType=Type.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className+","+assemblyName);
					}
					else {//plugin was found
						classType=ass.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className);
					}
					DtoObject[] parameters=dtoGetString.Params;
					Type[] paramTypes=DtoObject.GenerateTypes(parameters,assemblyName);
					MethodInfo methodInfo=classType.GetMethod(methodName,paramTypes);
					if(methodInfo==null) {
						throw new ApplicationException("Method not found with "+parameters.Length.ToString()+" parameters: "+dtoGetString.MethodName);
					}
					object[] paramObjs=DtoObject.GenerateObjects(parameters);
					string strResult=(string)methodInfo.Invoke(null,paramObjs);
					//strResult=strResult.Replace("\r","\\r");
					//return XmlConverter.Serialize(typeof(string),strResult);
					return strResult;
				}
				else if(type == typeof(DtoGetBool)) {
					DtoGetBool dtoGetBool=(DtoGetBool)dto;
					Userods.CheckCredentials(dtoGetBool.Credentials);//will throw exception if fails.
					string[] fullNameComponents=GetComponentsFromDtoMeth(dtoGetBool.MethodName);
					string assemblyName=fullNameComponents[0];//OpenDentBusiness or else a plugin name
					string className=fullNameComponents[1];
					string methodName=fullNameComponents[2];
					Type classType=null;
					Assembly ass=Plugins.GetAssembly(assemblyName);
					if(ass==null) {
						classType=Type.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className+","+assemblyName);
					}
					else {//plugin was found
						classType=ass.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className);
					}
					DtoObject[] parameters=dtoGetBool.Params;
					Type[] paramTypes=DtoObject.GenerateTypes(parameters,assemblyName);
					MethodInfo methodInfo=classType.GetMethod(methodName,paramTypes);
					if(methodInfo==null) {
						throw new ApplicationException("Method not found with "+parameters.Length.ToString()+" parameters: "+dtoGetBool.MethodName);
					}
					object[] paramObjs=DtoObject.GenerateObjects(parameters);
					bool boolResult=(bool)methodInfo.Invoke(null,paramObjs);
					return boolResult.ToString();
				}
				else {
					throw new NotSupportedException("Dto type not supported: "+type.FullName);
				}
			}
			catch(Exception e) {
				DtoException exception = new DtoException();
				exception.ExceptionType=e.GetType().BaseType.Name;//Since the exception was down converted to a regular exception, we need the BaseType.
				if(e.InnerException==null) {
					exception.Message = e.Message;
				}
				else {
					exception.Message = e.InnerException.Message;
				}
				return exception.Serialize();
			}
		}

		///<summary>Helper function to handle full method name with 2 components or 3 components.  Versions prior to 14.3 will send 2 components.  14.3 and above will send the assembly name OpenDentBusiness or plugin assembly name.  If only 2 components are received, we will prepend OpenDentBusiness so this will be backward compatible with versions prior to 14.3.</summary>
		private string[] GetComponentsFromDtoMeth(string methodName) {
			if(methodName.Split('.').Length==2) {
				methodName="OpenDentBusiness."+methodName;
			}
			return methodName.Split('.');
		}

		

	}
}
