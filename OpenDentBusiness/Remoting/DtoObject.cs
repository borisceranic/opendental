using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace OpenDentBusiness {
	///<summary>Packages any object with a TypeName so that it can be serialized and deserialized better.</summary>
	public class DtoObject:IXmlSerializable {
		///<summary>Fully qualified name, including the namespace but not the assembly.  Examples: System.Int32, OpenDentBusiness.Patient, OpenDentBusiness.Patient[], List&lt;OpenDentBusiness.Patient&gt;, PluginOC.OcContainer.  When the xml element is created for the Obj, the namespace is not included.  So this field properly stores it.</summary>
		public string TypeName;
		///<summary>The actual object.</summary>
		public object Obj;

		///<summary>Empty constructor as required by IXmlSerializable</summary>
		public DtoObject() {
		}

		///<summary>This is the constructor that should be used normally because it automatically creates the TypeName.</summary>
		public DtoObject(object obj,Type objType) {
			Obj=obj;
			//Type type=obj.GetType();
			//This will eventually become much more complex:
			//Arrays automatically become "ArrayOf..." and serialize just fine, with TypeName=...[]
			//Lists:
			if(objType.IsGenericType) {
				Type listType=objType.GetGenericArguments()[0];
				TypeName="List<"+listType.FullName+">";
			}
			else {
				TypeName=objType.FullName;
			}
		}

		///<summary>This is not explicitly called by our code.  It's required for the IXmlSerializable interface, which we have defined for this class.  So C# will call this when we call Serialize().</summary>
		public void WriteXml(XmlWriter writer) {
			/* we want the result to look like this:
			<TypeName>Patient</TypeName>
			<Obj>
				<Patient>
					<LName>Smith</LName>
					<PatNum>22</PatNum>
					<IsGuar>True</IsGuar>
				</Patient>
			</Obj>
			*/
			writer.WriteStartElement("TypeName");
			Type type=ConvertNameToType(TypeName);
			if(type==typeof(object)) {
				type=Obj.GetType();
				TypeName=type.FullName;
			}
			writer.WriteString(TypeName);
			writer.WriteEndElement();//TypeName
			writer.WriteStartElement("Obj");
			if(TypeName=="System.Drawing.Color") {
				XmlSerializer serializer = new XmlSerializer(typeof(int));
				serializer.Serialize(writer,((Color)Obj).ToArgb());
			}
			else if(TypeName=="System.Data.DataTable") {
				writer.WriteRaw(XmlConverter.TableToXml((DataTable)Obj));
			}
			else {
				//string assemb=Assembly.GetAssembly(typeof(Db)).FullName;//"OpenDentBusiness, Version=14.3.0.0, Culture=neutral, PublicKeyToken=null"
				XmlSerializer serializer=new XmlSerializer(type);
				serializer.Serialize(writer,Obj);
			}
			writer.WriteEndElement();//Obj
		}

		public void ReadXml(XmlReader reader) {
			reader.ReadToFollowing("TypeName");
			reader.ReadStartElement("TypeName");
			TypeName=reader.ReadString();
			reader.ReadEndElement();//TypeName
			while(reader.NodeType!=XmlNodeType.Element) {
				reader.Read();//gets rid of whitespace if in debug mode.
			}
			reader.ReadStartElement("Obj");
			while(reader.NodeType!=XmlNodeType.Element) {
				reader.Read();
			}
			string strObj=reader.ReadOuterXml();
			//now get the reader to the correct location
			while(reader.NodeType!=XmlNodeType.EndElement) {
				reader.Read();
			}
			reader.ReadEndElement();//Obj
			while(reader.NodeType!=XmlNodeType.EndElement) {
				reader.Read();
			}
			reader.ReadEndElement();//DtoObject
			//Now, process what we read.
			Type type=null;
			if(TypeName=="System.Drawing.Color") {
				type=typeof(int);
			}
			else{
				type=ConvertNameToType(TypeName);
			}
			XmlSerializer serializer = new XmlSerializer(type);
			//XmlReader reader2=XmlReader.Create(new StringReader(strObj));
			XmlTextReader reader2=new XmlTextReader(new StringReader(strObj));
			if(TypeName=="System.Drawing.Color") {
				Obj=Color.FromArgb((int)serializer.Deserialize(reader2));
			}
			else if(TypeName=="System.Data.DataTable") {
				Obj=XmlConverter.XmlToTable(strObj);
			}
			else {
				Obj=serializer.Deserialize(reader2);
			}
			//Convert.ChangeType(serializer.Deserialize(reader2),type);
		}

		///<summary>Required by IXmlSerializable</summary>
		public XmlSchema GetSchema() {
			return (null);
		}

		///<summary>We must pass in a matching array of types for situations where nulls are used in parameters.
		///Otherwise, we won't know the parameter type.
		///This method needs to create a "deep copy" of all objects passed in because we will be escaping characters within string variables.
		///Since we do not know what objects are being passed in, we decided to use JSON to generically serialize each object.
		///JSON seems to handle invalid characters better than XML even though it has its own quirks.</summary>
		public static DtoObject[] ConstructArray(object[] objArray,Type[] objTypes) {
			DtoObject[] retVal=new DtoObject[objArray.Length];
			JsonSerializerSettings jsonSettings=new JsonSerializerSettings() { TypeNameHandling=TypeNameHandling.Auto };
			for(int i=0;i<objArray.Length;i++) {
				object objCur=objArray[i];
				Type typeCur=objTypes[i];
				DtoObject dtoCur=new DtoObject(objCur,typeCur);
				Type typeElement=typeCur;
				if(typeCur!=typeof(string) && typeCur.IsGenericType) {//get the type of the items in the List
					typeElement=typeCur.GetGenericArguments()[0];
				}
				if(objCur==null
					|| (typeCur==typeof(string) && string.IsNullOrEmpty(objCur.ToString()))//if object is a string and is null or empty, just add as-is
					|| (typeElement!=typeof(string) && typeElement.IsValueType))//if object is either value typed or a list of value typed objs, just add as-is
				{
					retVal[i]=dtoCur;
					continue;
				}
				//we only need to create a deep copy of all objects that are strings or complex objects which may contain strings.
				retVal[i]=JsonConvert.DeserializeObject<DtoObject>(JsonConvert.SerializeObject(dtoCur,jsonSettings),jsonSettings);
			}
			return retVal;
		}

		public static object[] GenerateObjects(DtoObject[] parameters) {
			object[] retVal=new object[parameters.Length];
			for(int i=0;i<parameters.Length;i++) {
				retVal[i]=parameters[i].Obj;
			}
			return retVal;
		}

		public static Type[] GenerateTypes(DtoObject[] parameters,string strAssembNameDeprecating) {
			Type[] retVal=new Type[parameters.Length];
			for(int i=0;i<parameters.Length;i++) {
				retVal[i]=ConvertNameToType(parameters[i].TypeName);//,strAssembNameDeprecating);
			}
			return retVal;
		}

		///<summary>Examples of strTypeName passed in: System.Int32, System.Drawing.Color, OpenDentBusiness.Patient, OpenDentBusiness.Patient[], List&lt;OpenDentBusiness.Patient&gt;, PluginOC.OcContainer</summary>
		private static Type ConvertNameToType(string strTypeName){//,string strAssembDeprecating) {
			Type typeObj=null;
			if(strTypeName.StartsWith("List<")) {
				string strTypeGenName=strTypeName.Substring(5,strTypeName.Length-6);//strips off the List<>
				Type typeGen=null;
				Assembly assemb=Plugins.GetAssembly(strTypeGenName);//usually null, unless the type is for a plugin
				if(assemb==null) {
					typeGen=Type.GetType(strTypeGenName);//strTypeName includes the namespace, which we require to be same as assembly by convention
				}
				else {//plugin was found
					typeGen=assemb.GetType(strTypeGenName);//strTypeName includes the namespace, which we require to be same as assembly by convention
				}
				Type typeList=typeof(List<>);
				typeObj=typeList.MakeGenericType(typeGen);
			}
			else if(strTypeName=="System.Drawing.Color") {
				typeObj=typeof(Color);
			}
			else if(strTypeName=="System.Data.DataTable") {
				typeObj=typeof(DataTable);
			}
			else {//system types, OpenDentBusiness, and plugins
				string strAssembName=strTypeName.Substring(0,strTypeName.IndexOf("."));//example: System.String: index=6, substring(0,6)="System"
				Assembly assemb=Plugins.GetAssembly(strAssembName);//usually null, unless the type is for a plugin
				if(assemb==null) {
					typeObj=Type.GetType(strTypeName);//strTypeName includes the namespace, which we require to be same as assembly by convention
				}
				else {//plugin was found
					typeObj=assemb.GetType(strTypeName);//strTypeName includes the namespace, which we require to be same as assembly by convention
				}
			}
			return typeObj;
		}

	}
}
