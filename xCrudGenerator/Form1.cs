using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace xCrudGenerator {
	public partial class Form1:Form {
		private string crudDir;
		private string crudmDir;
		private string convertDbFile;
		private string convertDbFilem;
		private const string rn="\r\n";
		private const string t="\t";
		private const string t2="\t\t";
		private const string t3="\t\t\t";
		private const string t4="\t\t\t\t";
		private const string t5="\t\t\t\t\t";
		private const string t6="\t\t\t\t\t\t";
		private List<Type> tableTypes;
		private List<Type> tableTypesAll;
		private List<Type> tableTypesM;

		public Form1() {
			InitializeComponent();
		}

		private void Form1_Load(object sender,EventArgs e) {
			crudDir=@"..\..\..\OpenDentBusiness\Crud";
			crudmDir=@"..\..\..\OpenDentBusiness\Mobile\Crud";
			convertDbFile=@"..\..\..\OpenDentBusiness\Misc\ConvertDatabases3.cs";
			convertDbFilem=@"..\..\..\OpenDentBusiness\Mobile\ConvertDatabasem.cs";
			if(!Directory.Exists(crudDir)) {
				MessageBox.Show(crudDir+" is an invalid path.");
				Application.Exit();
			}
			if(!Directory.Exists(crudmDir)) {
				MessageBox.Show(crudmDir+" is an invalid path.");
				Application.Exit();
			}
			tableTypes=new List<Type>();
			tableTypesAll=new List<Type>();
			tableTypesM=new List<Type>();
			Type typeTableBase=typeof(TableBase);
			Assembly assembly=Assembly.GetAssembly(typeTableBase);
			foreach(Type typeClass in assembly.GetTypes()){
				if(typeClass.IsSubclassOf(typeTableBase)) {
					if(CrudGenHelper.IsMobile(typeClass)){
						tableTypesM.Add(typeClass);	
					}
					else{
						tableTypes.Add(typeClass);	
					}
					tableTypesAll.Add(typeClass);	
				}
			}
			tableTypesAll.Sort(CompareTypesByName);
			tableTypes.Sort(CompareTypesByName);
			tableTypesM.Sort(CompareTypesByName);
			for(int i=0;i<tableTypes.Count;i++){
				listClass.Items.Add(tableTypesAll[i].Name);
			}
			for(int i=0;i<Enum.GetNames(typeof(SnippetType)).Length;i++){
				comboType.Items.Add(Enum.GetNames(typeof(SnippetType))[i].ToString());
			}
			comboType.SelectedIndex=(int)SnippetType.EntireSclass;
		}

		private static int CompareTypesByName(Type x, Type y){
			return x.Name.CompareTo(y.Name);
		}

		private void butRun_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			string[] files;
			StringBuilder strb;
			string className;
			if(checkRun.Checked){
				files=Directory.GetFiles(crudDir);
				CrudGenHelper.ConnectToDatabase(textDb.Text);
				for(int i=0;i<tableTypes.Count;i++){
					className=tableTypes[i].Name+"Crud";
					strb=new StringBuilder();
					CrudGenHelper.ValidateTypes(tableTypes[i],textDb.Text);
					WriteAll(strb,className,tableTypes[i],false);
					File.WriteAllText(Path.Combine(crudDir,className+".cs"),strb.ToString());
					CrudQueries.Write(convertDbFile,tableTypes[i],textDb.Text,false);
					CrudGenDataInterface.Create(convertDbFile,tableTypes[i],textDb.Text,false);
					//CrudGenDataInterfaceWebService.Create(tableTypes[i]);
					Application.DoEvents();
				}
			}
			if(checkRunM.Checked) {
				files=Directory.GetFiles(crudmDir);
				CrudGenHelper.ConnectToDatabaseM(textDbM.Text);
				for(int i=0;i<tableTypesM.Count;i++) {
					className=tableTypesM[i].Name+"Crud";
					strb=new StringBuilder();
					CrudGenHelper.ValidateTypes(tableTypesM[i],textDbM.Text);
					WriteAll(strb,className,tableTypesM[i],true);
					File.WriteAllText(Path.Combine(crudmDir,className+".cs"),strb.ToString());
					CrudQueries.Write(convertDbFilem,tableTypesM[i],textDbM.Text,true);
					CrudGenDataInterface.Create(convertDbFilem,tableTypesM[i],textDbM.Text,true);
				}
			}
			if(checkRunSchema.Checked) {
				File.WriteAllText(@"..\..\..\OpenDentBusiness\Db\SchemaCrudTest.cs",CrudSchemaForUnitTest.Create());
			}
			Cursor=Cursors.Default;
			MessageBox.Show("Done");
			//Application.Exit();
		}

		///<summary>Example of className is 'AccountCrud' or 'PatientmCrud'.</summary>
		private void WriteAll(StringBuilder strb,string className,Type typeClass,bool isMobile) {
			#region initialize variables
			FieldInfo[] fields=typeClass.GetFields();//We can't assume they are in the correct order.
			FieldInfo priKey=null;
			FieldInfo priKey1=null;
			FieldInfo priKey2=null;
			if(isMobile) {
				priKey1=CrudGenHelper.GetPriKeyMobile1(fields,typeClass.Name);
				priKey2=CrudGenHelper.GetPriKeyMobile2(fields,typeClass.Name);
			}
			else {
				priKey=CrudGenHelper.GetPriKey(fields,typeClass.Name);
			}
			string tablename=CrudGenHelper.GetTableName(typeClass);//in lowercase now.
			string priKeyParam=null;
			string priKeyParam1=null;
			string priKeyParam2=null;
			if(isMobile) {
				priKeyParam1=priKey1.Name.Substring(0,1).ToLower()+priKey1.Name.Substring(1);//lowercase initial letter.  Example customerNum
				priKeyParam2=priKey2.Name.Substring(0,1).ToLower()+priKey2.Name.Substring(1);//lowercase initial letter.  Example patNum
			}
			else {
				priKeyParam=priKey.Name.Substring(0,1).ToLower()+priKey.Name.Substring(1);//lowercase initial letter.  Example patNum
			}
			List<Permissions> listAuditTrailPerms=CrudGenHelper.GetPermsFromCrudAuditPerm(CrudGenHelper.GetCrudAuditPermForClass(typeClass));
			string obj=typeClass.Name.Substring(0,1).ToLower()+typeClass.Name.Substring(1);//lowercase initial letter.  Example feeSched
			string oldObj="old"+typeClass.Name;//used in the second update overload.  Example oldFeeSched
			#endregion initialize variables
			#region class header
			strb.Append("//This file is automatically generated."+rn
				+"//Do not attempt to make changes to this file because the changes will be erased and overwritten."+rn
				+@"using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;"+rn);
			if(className.StartsWith("EhrLab")) {
				strb.Append("using EhrLaboratories;"+rn);
			}
			if(isMobile){
				strb.Append(rn+"namespace OpenDentBusiness.Mobile.Crud{");
			}
			else{
				strb.Append(rn+"namespace OpenDentBusiness.Crud{");
			}
			strb.Append(rn+t+"public class "+className+" {");
			#endregion class header
			#region SelectOne
			//SelectOne------------------------------------------------------------------------------------------
			if(isMobile) {
				strb.Append(rn+t2+"///<summary>Gets one "+typeClass.Name+" object from the database using primaryKey1(CustomerNum) and primaryKey2.  Returns null if not found.</summary>");
				strb.Append(rn+t2+"public static "+typeClass.Name+" SelectOne(long "+priKeyParam1+",long "+priKeyParam2+"){");
				strb.Append(rn+t3+"string command=\"SELECT * FROM "+tablename+" \"");
				strb.Append(rn+t4+"+\"WHERE "+priKey1.Name+" = \"+POut.Long("+priKeyParam1+")+\" AND "+priKey2.Name+" = \"+POut.Long("+priKeyParam2+");");
			}
			else {
				strb.Append(rn+t2+"///<summary>Gets one "+typeClass.Name+" object from the database using the primary key.  Returns null if not found.</summary>");
				strb.Append(rn+t2+"public static "+typeClass.Name+" SelectOne(long "+priKeyParam+"){");
				strb.Append(rn+t3+"string command=\"SELECT * FROM "+tablename+" \"");
				strb.Append(rn+t4+"+\"WHERE "+priKey.Name+" = \"+POut.Long("+priKeyParam+");");
			}
			strb.Append(rn+t3+"List<"+typeClass.Name+"> list=TableToList(Db.GetTable(command));");
			strb.Append(rn+t3+"if(list.Count==0) {");
			strb.Append(rn+t4+"return null;");
			strb.Append(rn+t3+"}");
			strb.Append(rn+t3+"return list[0];");
			strb.Append(rn+t2+"}");
			#endregion SelectOne
			#region SelectOne(command)
			//SelectOne(string command)--------------------------------------------------------------------------
			strb.Append(rn+rn+t2+"///<summary>Gets one "+typeClass.Name+" object from the database using a query.</summary>");
			strb.Append(rn+t2+"public static "+typeClass.Name+" SelectOne(string command){");
			strb.Append(rn+t3+@"if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException(""Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n""+command);
			}");
			strb.Append(rn+t3+"List<"+typeClass.Name+"> list=TableToList(Db.GetTable(command));");
			strb.Append(rn+t3+"if(list.Count==0) {");
			strb.Append(rn+t4+"return null;");
			strb.Append(rn+t3+"}");
			strb.Append(rn+t3+"return list[0];");
			strb.Append(rn+t2+"}");
			#endregion SelectOne(command)
			#region SelectMany
			//SelectMany-----------------------------------------------------------------------------------------
			strb.Append(rn+rn+t2+"///<summary>Gets a list of "+typeClass.Name+" objects from the database using a query.</summary>");
			strb.Append(rn+t2+"public static List<"+typeClass.Name+"> SelectMany(string command){");
			strb.Append(rn+t3+@"if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException(""Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n""+command);
			}");
			strb.Append(rn+t3+"List<"+typeClass.Name+"> list=TableToList(Db.GetTable(command));");
			strb.Append(rn+t3+"return list;");
			strb.Append(rn+t2+"}");
			#endregion SelectMany
			#region TableToList
			//TableToList----------------------------------------------------------------------------------------
			strb.Append(rn+rn+t2+"///<summary>Converts a DataTable to a list of objects.</summary>");
			strb.Append(rn+t2+"public static List<"+typeClass.Name+"> TableToList(DataTable table){");
			strb.Append(rn+t3+"List<"+typeClass.Name+"> retVal=new List<"+typeClass.Name+">();");
			strb.Append(rn+t3+typeClass.Name+" "+obj+";");
			strb.Append(rn+t3+"foreach(DataRow row in table.Rows) {");
			strb.Append(rn+t4+obj+"=new "+typeClass.Name+"();");
			List<FieldInfo> fieldsInDb=CrudGenHelper.GetFieldsExceptNotDb(fields);
			//get the longest fieldname for alignment purposes
			int longestField=0;
			for(int f=0;f<fieldsInDb.Count;f++){
				if(fieldsInDb[f].Name.Length>longestField){
					longestField=fieldsInDb[f].Name.Length;
				}
			}
			CrudSpecialColType specialType;
			for(int f=0;f<fieldsInDb.Count;f++){
				//Fields are not guaranteed to be in any particular order.
				specialType=CrudGenHelper.GetSpecialType(fieldsInDb[f]);
				if(specialType==CrudSpecialColType.EnumAsString) {
					string fieldLower=fieldsInDb[f].Name.Substring(0,1).ToLower()+fieldsInDb[f].Name.Substring(1);//lowercase initial letter.  Example clockStatus
					strb.Append(rn+t4+"string "+fieldLower+"=row[\""+fieldsInDb[f].Name+"\"].ToString();");
					strb.Append(rn+t4+"if("+fieldLower+"==\"\"){");
					strb.Append(rn+t5+obj+"."+fieldsInDb[f].Name.PadRight(longestField-2,' ')+"="
						+"("+fieldsInDb[f].FieldType.Name+")0;");
					strb.Append(rn+t4+"}");
					strb.Append(rn+t4+"else try{");
					strb.Append(rn+t5+obj+"."+fieldsInDb[f].Name.PadRight(longestField-2,' ')+"="
						+"("+fieldsInDb[f].FieldType.Name+")Enum.Parse(typeof("+fieldsInDb[f].FieldType.Name+"),"+fieldLower+");");
					strb.Append(rn+t4+"}");
					strb.Append(rn+t4+"catch{");
					strb.Append(rn+t5+obj+"."+fieldsInDb[f].Name.PadRight(longestField-2,' ')+"="
						+"("+fieldsInDb[f].FieldType.Name+")0;");
					strb.Append(rn+t4+"}");
					continue;
				}
				strb.Append(rn+t4+obj+"."+fieldsInDb[f].Name.PadRight(longestField,' ')+"= ");
				if(specialType==CrudSpecialColType.DateT
					|| specialType==CrudSpecialColType.TimeStamp
					|| specialType==CrudSpecialColType.DateTEntry
					|| specialType==CrudSpecialColType.DateTEntryEditable)
				{
					//specialTypes.DateEntry and DateEntryEditable is handled fine by the normal DateTime (date) below.
					strb.Append("PIn.DateT (");
				}
				//else if(specialType==CrudSpecialColType.EnumAsString) {//moved up
				else if(specialType==CrudSpecialColType.TimeSpanNeg) {
					strb.Append("PIn.TSpan (");
				}
				//no special treatment for specialType clob
				else if(fieldsInDb[f].FieldType.IsEnum) {
					strb.Append("("+fieldsInDb[f].FieldType.ToString()+")PIn.Int(");//.ToString() instead of .Name to get fully qualified name
				}
				else switch(fieldsInDb[f].FieldType.Name) {
					default:
						throw new ApplicationException("Type not yet supported: "+fieldsInDb[f].FieldType.Name);
					case "Bitmap":
						strb.Append("PIn.Bitmap(");
						break;
					case "Boolean":
						strb.Append("PIn.Bool  (");
						break;
					case "Byte":
						strb.Append("PIn.Byte  (");
						break;
					case "Color":
						strb.Append("Color.FromArgb(PIn.Int(");
						break;
					case "DateTime"://This ONLY handles date, not dateT which is a special type.
						strb.Append("PIn.Date  (");
						break;
					case "Double":
						strb.Append("PIn.Double(");
						break;
					case "Interval":
						strb.Append("new Interval(PIn.Int(");
						break;
					case "Int64":
						strb.Append("PIn.Long  (");
						break;
					case "Int32":
						strb.Append("PIn.Int   (");
						break;
					case "Single":
						strb.Append("PIn.Float (");
						break;
					case "String":
						strb.Append("PIn.String(");
						break;
					case "TimeSpan":
						strb.Append("PIn.Time(");
						break;
				}
				strb.Append("row[\""+fieldsInDb[f].Name+"\"].ToString())");
				if(fieldsInDb[f].FieldType.Name=="Color" || fieldsInDb[f].FieldType.Name=="Interval") {
					strb.Append(")");
				}
				strb.Append(";");
			}
			strb.Append(rn+t4+"retVal.Add("+obj+");");
			strb.Append(rn+t3+"}");
			strb.Append(rn+t3+"return retVal;");
			strb.Append(rn+t2+"}");
			#endregion TableToList
			#region Insert
			//Insert---------------------------------------------------------------------------------------------
			List<FieldInfo> fieldsExceptPri=null; 
			if(isMobile) {
				fieldsExceptPri=CrudGenHelper.GetFieldsExceptPriKey(fields,priKey2);
				//first override not used for mobile.
				//second override
				strb.Append(rn+rn+t2+"///<summary>Usually set useExistingPK=true.  Inserts one "+typeClass.Name+" into the database.</summary>");
				strb.Append(rn+t2+"public static long Insert("+typeClass.Name+" "+obj+",bool useExistingPK){");
				strb.Append(rn+t3+"if(!useExistingPK) {");// && PrefC.RandomKeys) {");PrefC.RandomKeys is always true for mobile, since autoincr is just not possible.
//Todo: ReplicationServers.GetKey() needs to work for mobile.  Not needed until we start inserting records from mobile.
				strb.Append(rn+t4+obj+"."+priKey2.Name+"=ReplicationServers.GetKey(\""+tablename+"\",\""+priKey2.Name+"\");");
				strb.Append(rn+t3+"}");
				strb.Append(rn+t3+"string command=\"INSERT INTO "+tablename+" (\";");
				//strb.Append(rn+t3+"if(useExistingPK || PrefC.RandomKeys) {");//PrefC.RandomKeys is always true
				strb.Append(rn+t3+"command+=\""+priKey2.Name+",\";");
				//strb.Append(rn+t3+"}");
				strb.Append(rn+t3+"command+=\"");
				for(int f=0;f<fieldsExceptPri.Count;f++) {//all fields except PK2.
					if(CrudGenHelper.GetSpecialType(fieldsExceptPri[f])==CrudSpecialColType.TimeStamp) {
						continue;
					}
					if(f>0) {
						strb.Append(",");
					}
					strb.Append(fieldsExceptPri[f].Name);
				}
				strb.Append(") VALUES(\";");
				//strb.Append(rn+t3+"if(useExistingPK || PrefC.RandomKeys) {");//PrefC.RandomKeys is always true
				strb.Append(rn+t3+"command+=POut.Long("+obj+"."+priKey2.Name+")+\",\";");
				//strb.Append(rn+t3+"}");
				strb.Append(rn+t3+"command+=");
			}
			else {
				fieldsExceptPri=CrudGenHelper.GetFieldsExceptPriKey(fields,priKey);
				strb.Append(rn+rn+t2+"///<summary>Inserts one "+typeClass.Name+" into the database.  Returns the new priKey.</summary>");
				strb.Append(rn+t2+"public static long Insert("+typeClass.Name+" "+obj+"){");
				strb.Append(rn+t3+"if(DataConnection.DBtype==DatabaseType.Oracle) {");
				strb.Append(rn+t4+obj+"."+priKey.Name+"=DbHelper.GetNextOracleKey(\""+tablename+"\",\""+priKey.Name+"\");");
				strb.Append(rn+t4+"int loopcount=0;");
				strb.Append(rn+t4+"while(loopcount<100){");
				strb.Append(rn+t5+"try {");
				strb.Append(rn+t5+t+"return Insert("+obj+",true);");
				strb.Append(rn+t5+"}");
				strb.Append(rn+t5+"catch(Oracle.DataAccess.Client.OracleException ex){");
				strb.Append(rn+t5+t+"if(ex.Number==1 && ex.Message.ToLower().Contains(\"unique constraint\") && ex.Message.ToLower().Contains(\"violated\")){");
				strb.Append(rn+t5+t2+obj+"."+priKey.Name+"++;");
				strb.Append(rn+t5+t2+"loopcount++;");
				strb.Append(rn+t5+t+"}");
				strb.Append(rn+t5+t+"else{");
				strb.Append(rn+t5+t2+"throw ex;");
				strb.Append(rn+t5+t+"}");
				strb.Append(rn+t5+"}");
				strb.Append(rn+t4+"}");
				strb.Append(rn+t4+"throw new ApplicationException(\"Insert failed.  Could not generate primary key.\");");
				strb.Append(rn+t3+"}");
				strb.Append(rn+t3+"else {");
				strb.Append(rn+t4+"return Insert("+obj+",false);");
				strb.Append(rn+t3+"}");
				strb.Append(rn+t2+"}");
				//second override
				strb.Append(rn+rn+t2+"///<summary>Inserts one "+typeClass.Name+" into the database.  Provides option to use the existing priKey.</summary>");
				strb.Append(rn+t2+"public static long Insert("+typeClass.Name+" "+obj+",bool useExistingPK){");
				strb.Append(rn+t3+"if(!useExistingPK && PrefC.RandomKeys) {");
				strb.Append(rn+t4+obj+"."+priKey.Name+"=ReplicationServers.GetKey(\""+tablename+"\",\""+priKey.Name+"\");");
				strb.Append(rn+t3+"}");
				strb.Append(rn+t3+"string command=\"INSERT INTO "+tablename+" (\";");
				strb.Append(rn+t3+"if(useExistingPK || PrefC.RandomKeys) {");
				strb.Append(rn+t4+"command+=\""+priKey.Name+",\";");
				strb.Append(rn+t3+"}");
				strb.Append(rn+t3+"command+=\"");
				for(int f=0;f<fieldsExceptPri.Count;f++) {
					if(CrudGenHelper.GetSpecialType(fieldsExceptPri[f])==CrudSpecialColType.TimeStamp) {
						continue;
					}
					if(f>0) {
						strb.Append(",");
					}
					strb.Append(fieldsExceptPri[f].Name);
				}
				strb.Append(") VALUES(\";");
				strb.Append(rn+t3+"if(useExistingPK || PrefC.RandomKeys) {");
				strb.Append(rn+t4+"command+=POut.Long("+obj+"."+priKey.Name+")+\",\";");
				strb.Append(rn+t3+"}");
				strb.Append(rn+t3+"command+=");
			}
			//a quick and dirty temporary list that just helps keep track of which columns take parameters
			List<OdSqlParameter> paramList=new List<OdSqlParameter>();
			for(int f=0;f<fieldsExceptPri.Count;f++) {
				strb.Append(rn+t4);
				specialType=CrudGenHelper.GetSpecialType(fieldsExceptPri[f]);
				if(specialType==CrudSpecialColType.TimeStamp) {
					strb.Append("//"+fieldsExceptPri[f].Name+" can only be set by MySQL");
					continue;
				}
				if(f==0) {
					strb.Append(" ");
				}
				else {
					strb.Append("+");
				}
				if(specialType==CrudSpecialColType.DateEntry
					|| specialType==CrudSpecialColType.DateEntryEditable
					|| specialType==CrudSpecialColType.DateTEntry
					|| specialType==CrudSpecialColType.DateTEntryEditable) 
				{
					strb.Append("    DbHelper.Now()+\"");
				}
				else if(specialType==CrudSpecialColType.DateT) {
					strb.Append("    POut.DateT ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
				}
				else if(specialType==CrudSpecialColType.EnumAsString) {
					strb.Append("\"'\"+POut.String("+obj+"."+fieldsExceptPri[f].Name+".ToString())+\"'");
				}
				else if(specialType==CrudSpecialColType.TimeSpanNeg) {
					strb.Append("\"'\"+POut.TSpan ("+obj+"."+fieldsExceptPri[f].Name+")+\"'");
				}
				else if(specialType==CrudSpecialColType.TextIsClob || specialType==CrudSpecialColType.TextIsClobNote) {
					strb.Append("    DbHelper.ParamChar+\"param"+fieldsExceptPri[f].Name);
					paramList.Add(new OdSqlParameter(fieldsExceptPri[f].Name,OdDbType.Text,specialType));
				}
				else if(fieldsExceptPri[f].FieldType.IsEnum) {
					strb.Append("    POut.Int   ((int)"+obj+"."+fieldsExceptPri[f].Name+")+\"");
				}
				else switch(fieldsExceptPri[f].FieldType.Name) {
					default:
						throw new ApplicationException("Type not yet supported: "+fieldsExceptPri[f].FieldType.Name);
					case "Bitmap":
						strb.Append("    POut.Bitmap("+obj+"."+fieldsExceptPri[f].Name+")+\"");
						break;
					case "Boolean":
						strb.Append("    POut.Bool  ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
						break;
					case "Byte":
						strb.Append("    POut.Byte  ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
						break;
					case "Color":
						strb.Append("    POut.Int   ("+obj+"."+fieldsExceptPri[f].Name+".ToArgb())+\"");
						break;
					case "DateTime"://This is only for date, not dateT.
						strb.Append("    POut.Date  ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
						break;
					case "Double":
						strb.Append("\"'\"+POut.Double("+obj+"."+fieldsExceptPri[f].Name+")+\"'");
						break;
					case "Interval":
						strb.Append("    POut.Int   ("+obj+"."+fieldsExceptPri[f].Name+".ToInt())+\"");
						break;
					case "Int64":
						strb.Append("    POut.Long  ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
						break;
					case "Int32":
						strb.Append("    POut.Int   ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
						break;
					case "Single":
						strb.Append("    POut.Float ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
						break;
					case "String":
						strb.Append("\"'\"+POut.String("+obj+"."+fieldsExceptPri[f].Name+")+\"'");
						break;
					case "TimeSpan":
						strb.Append("    POut.Time  ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
						break;
				}
				if(f==fieldsExceptPri.Count-2
					&& CrudGenHelper.GetSpecialType(fieldsExceptPri[f+1])==CrudSpecialColType.TimeStamp) 
				{
					//in case the last field is a timestamp
					strb.Append(")\";");
				}
				else if(f<fieldsExceptPri.Count-1) {
					strb.Append(",\"");
				}
				else {
					strb.Append(")\";");
				}
			}
			for(int i=0;i<paramList.Count;i++) {
				strb.Append(rn+t3+"if("+obj+"."+paramList[i].ParameterName+"==null) {");
				strb.Append(rn+t4+""+obj+"."+paramList[i].ParameterName+"=\"\";");
				strb.Append(rn+t3+"}");
				//example: OdSqlParameter paramNote=new OdSqlParameter("paramNote",
				//           OdDbType.Text,procNote.Note);
				strb.Append(rn+t3+"OdSqlParameter param"+paramList[i].ParameterName+"=new OdSqlParameter(\"param"+paramList[i].ParameterName+"\","
					+"OdDbType.Text,");
				if((CrudSpecialColType)paramList[i].Value==CrudSpecialColType.TextIsClobNote) {
					strb.Append("POut.StringNote("+obj+"."+paramList[i].ParameterName+"));");//This is where large amounts of consecutive newlines are stripped away.
				}
				else {
					strb.Append(obj+"."+paramList[i].ParameterName+");");
				}
			}
			string paramsString="";//example: ,paramNote,paramAltNote
			for(int i=0;i<paramList.Count;i++){
				paramsString+=",param"+paramList[i].ParameterName;
			}	
			if(isMobile) {
				strb.Append(rn+t3+"Db.NonQ(command"+paramsString+");//There is no autoincrement in the mobile server.");
				strb.Append(rn+t3+"return "+obj+"."+priKey2.Name+";");
				strb.Append(rn+t2+"}");
			}
			else {
				strb.Append(rn+t3+"if(useExistingPK || PrefC.RandomKeys) {");
				strb.Append(rn+t4+"Db.NonQ(command"+paramsString+");");
				strb.Append(rn+t3+"}");
				strb.Append(rn+t3+"else {");
				strb.Append(rn+t4+obj+"."+priKey.Name+"=Db.NonQ(command,true"+paramsString+");");
				strb.Append(rn+t3+"}");
				strb.Append(rn+t3+"return "+obj+"."+priKey.Name+";");
				strb.Append(rn+t2+"}");
			}
			#endregion Insert
			#region InsertNoCache
			//InsertNoCache---------------------------------------------------------------------------------------------
			fieldsExceptPri=null; 
			if(isMobile) {
				//Mobile sync does not use cache already.
			}
			else {
				fieldsExceptPri=CrudGenHelper.GetFieldsExceptPriKey(fields,priKey);
				strb.Append(rn+rn+t2+"///<summary>Inserts one "+typeClass.Name+" into the database.  Returns the new priKey.  Doesn't use the cache.</summary>");
				strb.Append(rn+t2+"public static long InsertNoCache("+typeClass.Name+" "+obj+"){");
				strb.Append(rn+t3+"if(DataConnection.DBtype==DatabaseType.MySql) {");
				strb.Append(rn+t4+"return InsertNoCache("+obj+",false);");
				strb.Append(rn+t3+"}");
				strb.Append(rn+t3+"else {");
				strb.Append(rn+t4+"if(DataConnection.DBtype==DatabaseType.Oracle) {");
				strb.Append(rn+t5+obj+"."+priKey.Name+"=DbHelper.GetNextOracleKey(\""+tablename+"\",\""+priKey.Name+"\"); //Cacheless method");
				strb.Append(rn+t4+"}");
				strb.Append(rn+t4+"return InsertNoCache("+obj+",true);");
				strb.Append(rn+t3+"}");
				strb.Append(rn+t2+"}");
				//second override
				strb.Append(rn+rn+t2+"///<summary>Inserts one "+typeClass.Name+" into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>");
				strb.Append(rn+t2+"public static long InsertNoCache("+typeClass.Name+" "+obj+",bool useExistingPK){");
				strb.Append(rn+t3+"bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);");
				strb.Append(rn+t3+"string command=\"INSERT INTO "+tablename+" (\";");
				strb.Append(rn+t3+"if(!useExistingPK && isRandomKeys) {");
				strb.Append(rn+t4+obj+"."+priKey.Name+"=ReplicationServers.GetKeyNoCache(\""+tablename+"\",\""+priKey.Name+"\");");
				strb.Append(rn+t3+"}");
				strb.Append(rn+t3+"if(isRandomKeys || useExistingPK) {");
				strb.Append(rn+t4+"command+=\""+priKey.Name+",\";");
				strb.Append(rn+t3+"}");
				strb.Append(rn+t3+"command+=\"");
				for(int f=0;f<fieldsExceptPri.Count;f++) {
					if(CrudGenHelper.GetSpecialType(fieldsExceptPri[f])==CrudSpecialColType.TimeStamp) {
						continue;
					}
					if(f>0) {
						strb.Append(",");
					}
					strb.Append(fieldsExceptPri[f].Name);
				}
				strb.Append(") VALUES(\";");
				strb.Append(rn+t3+"if(isRandomKeys || useExistingPK) {");
				strb.Append(rn+t4+"command+=POut.Long("+obj+"."+priKey.Name+")+\",\";");
				strb.Append(rn+t3+"}");
				strb.Append(rn+t3+"command+=");
			}
			//a quick and dirty temporary list that just helps keep track of which columns take parameters
			paramList=new List<OdSqlParameter>();
			for(int f=0;f<fieldsExceptPri.Count;f++) {
				strb.Append(rn+t4);
				specialType=CrudGenHelper.GetSpecialType(fieldsExceptPri[f]);
				if(specialType==CrudSpecialColType.TimeStamp) {
					strb.Append("//"+fieldsExceptPri[f].Name+" can only be set by MySQL");
					continue;
				}
				if(f==0) {
					strb.Append(" ");
				}
				else {
					strb.Append("+");
				}
				if(specialType==CrudSpecialColType.DateEntry
					|| specialType==CrudSpecialColType.DateEntryEditable
					|| specialType==CrudSpecialColType.DateTEntry
					|| specialType==CrudSpecialColType.DateTEntryEditable) 
				{
					strb.Append("    DbHelper.Now()+\"");
				}
				else if(specialType==CrudSpecialColType.DateT) {
					strb.Append("    POut.DateT ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
				}
				else if(specialType==CrudSpecialColType.EnumAsString) {
					strb.Append("\"'\"+POut.String("+obj+"."+fieldsExceptPri[f].Name+".ToString())+\"'");
				}
				else if(specialType==CrudSpecialColType.TimeSpanNeg) {
					strb.Append("\"'\"+POut.TSpan ("+obj+"."+fieldsExceptPri[f].Name+")+\"'");
				}
				else if(specialType==CrudSpecialColType.TextIsClob || specialType==CrudSpecialColType.TextIsClobNote) {
					strb.Append("    DbHelper.ParamChar+\"param"+fieldsExceptPri[f].Name);
					paramList.Add(new OdSqlParameter(fieldsExceptPri[f].Name,OdDbType.Text,specialType));
				}
				else if(fieldsExceptPri[f].FieldType.IsEnum) {
					strb.Append("    POut.Int   ((int)"+obj+"."+fieldsExceptPri[f].Name+")+\"");
				}
				else switch(fieldsExceptPri[f].FieldType.Name) {
					default:
						throw new ApplicationException("Type not yet supported: "+fieldsExceptPri[f].FieldType.Name);
					case "Bitmap":
						strb.Append("    POut.Bitmap("+obj+"."+fieldsExceptPri[f].Name+")+\"");
						break;
					case "Boolean":
						strb.Append("    POut.Bool  ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
						break;
					case "Byte":
						strb.Append("    POut.Byte  ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
						break;
					case "Color":
						strb.Append("    POut.Int   ("+obj+"."+fieldsExceptPri[f].Name+".ToArgb())+\"");
						break;
					case "DateTime"://This is only for date, not dateT.
						strb.Append("    POut.Date  ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
						break;
					case "Double":
						strb.Append("\"'\"+POut.Double("+obj+"."+fieldsExceptPri[f].Name+")+\"'");
						break;
					case "Interval":
						strb.Append("    POut.Int   ("+obj+"."+fieldsExceptPri[f].Name+".ToInt())+\"");
						break;
					case "Int64":
						strb.Append("    POut.Long  ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
						break;
					case "Int32":
						strb.Append("    POut.Int   ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
						break;
					case "Single":
						strb.Append("    POut.Float ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
						break;
					case "String":
						strb.Append("\"'\"+POut.String("+obj+"."+fieldsExceptPri[f].Name+")+\"'");
						break;
					case "TimeSpan":
						strb.Append("    POut.Time  ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
						break;
				}
				if(f==fieldsExceptPri.Count-2
					&& CrudGenHelper.GetSpecialType(fieldsExceptPri[f+1])==CrudSpecialColType.TimeStamp) 
				{
					//in case the last field is a timestamp
					strb.Append(")\";");
				}
				else if(f<fieldsExceptPri.Count-1) {
					strb.Append(",\"");
				}
				else {
					strb.Append(")\";");
				}
			}
			for(int i=0;i<paramList.Count;i++) {
				strb.Append(rn+t3+"if("+obj+"."+paramList[i].ParameterName+"==null) {");
				strb.Append(rn+t4+""+obj+"."+paramList[i].ParameterName+"=\"\";");
				strb.Append(rn+t3+"}");
				//example: OdSqlParameter paramNote=new OdSqlParameter("paramNote",
				//           OdDbType.Text,procNote.Note);
				strb.Append(rn+t3+"OdSqlParameter param"+paramList[i].ParameterName+"=new OdSqlParameter(\"param"+paramList[i].ParameterName+"\","
					+"OdDbType.Text,");
				if((CrudSpecialColType)paramList[i].Value==CrudSpecialColType.TextIsClobNote) {
					strb.Append("POut.StringNote("+obj+"."+paramList[i].ParameterName+"));");//This is where large amounts of consecutive newlines are stripped away.
				}
				else {
					strb.Append(obj+"."+paramList[i].ParameterName+");");
				}
			}
			paramsString="";//example: ,paramNote,paramAltNote
			for(int i=0;i<paramList.Count;i++){
				paramsString+=",param"+paramList[i].ParameterName;
			}	
			if(isMobile) {
				//Not supported.
			}
			else {
				strb.Append(rn+t3+"if(useExistingPK || isRandomKeys) {");
				strb.Append(rn+t4+"Db.NonQ(command"+paramsString+");");
				strb.Append(rn+t3+"}");
				strb.Append(rn+t3+"else {");
				strb.Append(rn+t4+obj+"."+priKey.Name+"=Db.NonQ(command,true"+paramsString+");");
				strb.Append(rn+t3+"}");
				strb.Append(rn+t3+"return "+obj+"."+priKey.Name+";");
				strb.Append(rn+t2+"}");
			}
			#endregion InsertNoCache			
			#region Update
			//Update---------------------------------------------------------------------------------------------
			strb.Append(rn+rn+t2+"///<summary>Updates one "+typeClass.Name+" in the database.</summary>");
			strb.Append(rn+t2+"public static void Update("+typeClass.Name+" "+obj+"){");
			strb.Append(rn+t3+"string command=\"UPDATE "+tablename+" SET \"");
			for(int f=0;f<fieldsExceptPri.Count;f++) {
				if(isMobile && fieldsExceptPri[f]==priKey1) {//2 already skipped
					continue;
				}
				specialType=CrudGenHelper.GetSpecialType(fieldsExceptPri[f]);
				if(specialType==CrudSpecialColType.DateEntry) {
					strb.Append(rn+t4+"//"+fieldsExceptPri[f].Name+" not allowed to change");
					continue;
				}
				if(specialType==CrudSpecialColType.DateTEntry) {
					strb.Append(rn+t4+"//"+fieldsExceptPri[f].Name+" not allowed to change");
					continue;
				}
				if(specialType==CrudSpecialColType.TimeStamp) {
					strb.Append(rn+t4+"//"+fieldsExceptPri[f].Name+" can only be set by MySQL");
					continue;
				}
				if(specialType==CrudSpecialColType.ExcludeFromUpdate) {
					strb.Append(rn+t4+"//"+fieldsExceptPri[f].Name+" excluded from update");
					continue;
				}
				strb.Append(rn+t4+"+\""+fieldsExceptPri[f].Name.PadRight(longestField,' ')+"= ");
				if(specialType==CrudSpecialColType.DateT){
					strb.Append(" \"+POut.DateT ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
				}
				else if(specialType==CrudSpecialColType.DateEntryEditable){
					strb.Append(" \"+POut.Date  ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
				}
				else if(specialType==CrudSpecialColType.DateTEntryEditable){
					strb.Append(" \"+POut.DateT ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
				}
				else if(specialType==CrudSpecialColType.EnumAsString) {
					strb.Append("'\"+POut.String("+obj+"."+fieldsExceptPri[f].Name+".ToString())+\"'");
				}
				else if(specialType==CrudSpecialColType.TimeSpanNeg) {
					strb.Append("'\"+POut.TSpan ("+obj+"."+fieldsExceptPri[f].Name+")+\"'");
				}
				else if(specialType==CrudSpecialColType.TextIsClob || specialType==CrudSpecialColType.TextIsClobNote) {
					strb.Append(" \"+DbHelper.ParamChar+\"param"+fieldsExceptPri[f].Name);
					//paramList is already set above
				}
				else if(fieldsExceptPri[f].FieldType.IsEnum) {
					strb.Append(" \"+POut.Int   ((int)"+obj+"."+fieldsExceptPri[f].Name+")+\"");
				}
				else switch(fieldsExceptPri[f].FieldType.Name) {
					default:
						throw new ApplicationException("Type not yet supported: "+fieldsExceptPri[f].FieldType.Name);
					case "Bitmap":
						strb.Append(" \"+POut.Bitmap("+obj+"."+fieldsExceptPri[f].Name+")+\"");
						break;
					case "Boolean":
						strb.Append(" \"+POut.Bool  ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
						break;
					case "Byte":
						strb.Append(" \"+POut.Byte  ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
						break;
					case "Color":
						strb.Append(" \"+POut.Int   ("+obj+"."+fieldsExceptPri[f].Name+".ToArgb())+\"");
						break;
					case "DateTime"://This is only for date, not dateT
						strb.Append(" \"+POut.Date  ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
						break;
					case "Double":
						strb.Append("'\"+POut.Double("+obj+"."+fieldsExceptPri[f].Name+")+\"'");
						break;
					case "Interval":
						strb.Append(" \"+POut.Int   ("+obj+"."+fieldsExceptPri[f].Name+".ToInt())+\"");
						break;
					case "Int64":
						strb.Append(" \"+POut.Long  ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
						break;
					case "Int32":
						strb.Append(" \"+POut.Int   ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
						break;
					case "Single":
						strb.Append(" \"+POut.Float ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
						break;
					case "String":
						strb.Append("'\"+POut.String("+obj+"."+fieldsExceptPri[f].Name+")+\"'");
						break;
					case "TimeSpan":
						strb.Append(" \"+POut.Time  ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
						break;
				}
				if(f==fieldsExceptPri.Count-2
					&& CrudGenHelper.GetSpecialType(fieldsExceptPri[f+1])==CrudSpecialColType.TimeStamp) 
				{
					//in case the last field is a timestamp
					//strb.Append(" \"");
				}
				else if(f<fieldsExceptPri.Count-1) {
					strb.Append(",");
				}
				strb.Append(" \"");
			}
			if(isMobile) {
				strb.Append(rn+t4+"+\"WHERE "+priKey1.Name+" = \"+POut.Long("+obj+"."+priKey1.Name+")+\" AND "+priKey2.Name+" = \"+POut.Long("+obj+"."+priKey2.Name+");");
			}
			else {
				strb.Append(rn+t4+"+\"WHERE "+priKey.Name+" = \"+POut.Long("+obj+"."+priKey.Name+");");
			}
			for(int i=0;i<paramList.Count;i++) {
				strb.Append(rn+t3+"if("+obj+"."+paramList[i].ParameterName+"==null) {");
				strb.Append(rn+t4+""+obj+"."+paramList[i].ParameterName+"=\"\";");
				strb.Append(rn+t3+"}");
				strb.Append(rn+t3+"OdSqlParameter param"+paramList[i].ParameterName+"=new OdSqlParameter(\"param"+paramList[i].ParameterName+"\","
					+"OdDbType.Text,");
				if((CrudSpecialColType)paramList[i].Value==CrudSpecialColType.TextIsClobNote) {
					strb.Append("POut.StringNote("+obj+"."+paramList[i].ParameterName+"));");//This is where large amounts of consecutive newlines are stripped away.
				}
				else {
					strb.Append(obj+"."+paramList[i].ParameterName+");");
				}
			}
			strb.Append(rn+t3+"Db.NonQ(command"+paramsString+");");
			strb.Append(rn+t2+"}");
			#endregion Update
			#region Update 2nd override
			//Update, 2nd override-------------------------------------------------------------------------------
			if(!isMobile) {
				strb.Append(rn+rn+t2+"///<summary>Updates one "+typeClass.Name+" in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>");
				strb.Append(rn+t2+"public static bool Update("+typeClass.Name+" "+obj+","+typeClass.Name+" "+oldObj+"){");
				strb.Append(rn+t3+"string command=\"\";");
				for(int f=0;f<fieldsExceptPri.Count;f++) {
					//if(isMobile && fieldsExceptPri[f]==priKey1) {//2 already skipped
					//	continue;
					//}
					specialType=CrudGenHelper.GetSpecialType(fieldsExceptPri[f]);
					if(specialType==CrudSpecialColType.DateEntry) {
						strb.Append(rn+t3+"//"+fieldsExceptPri[f].Name+" not allowed to change");
						continue;
					}
					if(specialType==CrudSpecialColType.DateTEntry) {
						strb.Append(rn+t3+"//"+fieldsExceptPri[f].Name+" not allowed to change");
						continue;
					}
					if(specialType==CrudSpecialColType.TimeStamp) {
						strb.Append(rn+t3+"//"+fieldsExceptPri[f].Name+" can only be set by MySQL");
						continue;
					}
					if(specialType==CrudSpecialColType.ExcludeFromUpdate) {
						strb.Append(rn+t3+"//"+fieldsExceptPri[f].Name+" excluded from update");
						continue;
					}
					strb.Append(rn+t3+"if("+obj+"."+fieldsExceptPri[f].Name+" != "+oldObj+"."+fieldsExceptPri[f].Name+") {");
					strb.Append(rn+t4+"if(command!=\"\"){ command+=\",\";}");
					strb.Append(rn+t4+"command+=\""+fieldsExceptPri[f].Name+" = ");
					if(specialType==CrudSpecialColType.DateT) {
						strb.Append("\"+POut.DateT("+obj+"."+fieldsExceptPri[f].Name+")+\"");
					}
					else if(specialType==CrudSpecialColType.DateEntryEditable) {
						strb.Append("\"+POut.Date("+obj+"."+fieldsExceptPri[f].Name+")+\"");
					}
					else if(specialType==CrudSpecialColType.DateTEntryEditable) {
						strb.Append("\"+POut.DateT("+obj+"."+fieldsExceptPri[f].Name+")+\"");
					}
					else if(specialType==CrudSpecialColType.EnumAsString) {
						strb.Append("'\"+POut.String("+obj+"."+fieldsExceptPri[f].Name+".ToString())+\"'");
					}
					else if(specialType==CrudSpecialColType.TimeSpanNeg) {
						strb.Append("'\"+POut.TSpan ("+obj+"."+fieldsExceptPri[f].Name+")+\"'");
					}
					else if(specialType==CrudSpecialColType.TextIsClob || specialType==CrudSpecialColType.TextIsClobNote) {
						strb.Append("\"+DbHelper.ParamChar+\"param"+fieldsExceptPri[f].Name);
						//paramList is already set above
					}
					else if(fieldsExceptPri[f].FieldType.IsEnum) {
						strb.Append("\"+POut.Int   ((int)"+obj+"."+fieldsExceptPri[f].Name+")+\"");
					}
					else switch(fieldsExceptPri[f].FieldType.Name) {
							default:
								throw new ApplicationException("Type not yet supported: "+fieldsExceptPri[f].FieldType.Name);
							case "Boolean":
								strb.Append("\"+POut.Bool("+obj+"."+fieldsExceptPri[f].Name+")+\"");
								break;
							case "Bitmap":
								strb.Append("\"+POut.Bitmap("+obj+"."+fieldsExceptPri[f].Name+")+\"");
								break;
							case "Byte":
								strb.Append("\"+POut.Byte("+obj+"."+fieldsExceptPri[f].Name+")+\"");
								break;
							case "Color":
								strb.Append("\"+POut.Int("+obj+"."+fieldsExceptPri[f].Name+".ToArgb())+\"");
								break;
							case "DateTime"://This is only for date, not dateT.
								strb.Append("\"+POut.Date("+obj+"."+fieldsExceptPri[f].Name+")+\"");
								break;
							case "Double":
								strb.Append("'\"+POut.Double("+obj+"."+fieldsExceptPri[f].Name+")+\"'");
								break;
							case "Interval":
								strb.Append("\"+POut.Int("+obj+"."+fieldsExceptPri[f].Name+".ToInt())+\"");
								break;
							case "Int64":
								strb.Append("\"+POut.Long("+obj+"."+fieldsExceptPri[f].Name+")+\"");
								break;
							case "Int32":
								strb.Append("\"+POut.Int("+obj+"."+fieldsExceptPri[f].Name+")+\"");
								break;
							case "Single":
								strb.Append("\"+POut.Float("+obj+"."+fieldsExceptPri[f].Name+")+\"");
								break;
							case "String":
								strb.Append("'\"+POut.String("+obj+"."+fieldsExceptPri[f].Name+")+\"'");
								break;
							case "TimeSpan":
								strb.Append("\"+POut.Time  ("+obj+"."+fieldsExceptPri[f].Name+")+\"");
								break;
						}
					strb.Append("\";");
					strb.Append(rn+t3+"}");
				}
				strb.Append(rn+t3+"if(command==\"\"){");
				strb.Append(rn+t4+"return false;");
				strb.Append(rn+t3+"}");
				for(int i=0;i<paramList.Count;i++) {
					strb.Append(rn+t3+"if("+obj+"."+paramList[i].ParameterName+"==null) {");
					strb.Append(rn+t4+""+obj+"."+paramList[i].ParameterName+"=\"\";");
					strb.Append(rn+t3+"}");
					strb.Append(rn+t3+"OdSqlParameter param"+paramList[i].ParameterName+"=new OdSqlParameter(\"param"+paramList[i].ParameterName+"\","
					+"OdDbType.Text,");
					if((CrudSpecialColType)paramList[i].Value==CrudSpecialColType.TextIsClobNote) {
						strb.Append("POut.StringNote("+obj+"."+paramList[i].ParameterName+"));");//This is where large amounts of consecutive newlines are stripped away.
					}
					else {
						strb.Append(obj+"."+paramList[i].ParameterName+");");
					}
				}
				strb.Append(rn+t3+"command=\"UPDATE "+tablename+" SET \"+command");
				strb.Append(rn+t4+"+\" WHERE "+priKey.Name+" = \"+POut.Long("+obj+"."+priKey.Name+");");
				strb.Append(rn+t3+"Db.NonQ(command"+paramsString+");");
				strb.Append(rn+t3+"return true;");
				strb.Append(rn+t2+"}");
			}
			#endregion Update 2nd override
			#region Delete
			//Delete---------------------------------------------------------------------------------------------
			if(CrudGenHelper.IsDeleteForbidden(typeClass)) {
				strb.Append(rn+rn+t2+"//Delete not allowed for this table");
				strb.Append(rn+t2+"//public static void Delete(long "+priKeyParam+"){");
				strb.Append(rn+t2+"//");
				strb.Append(rn+t2+"//}");
			}
			else {
				strb.Append(rn+rn+t2+"///<summary>Deletes one "+typeClass.Name+" from the database.</summary>");
				if(isMobile) {
					strb.Append(rn+t2+"public static void Delete(long "+priKeyParam1+",long "+priKeyParam2+"){");
					strb.Append(rn+t3+"string command=\"DELETE FROM "+tablename+" \"");
					strb.Append(rn+t4+"+\"WHERE "+priKey1.Name+" = \"+POut.Long("+priKeyParam1+")+\" AND "+priKey2.Name+" = \"+POut.Long("+priKeyParam2+");");
				}
				else {
					strb.Append(rn+t2+"public static void Delete(long "+priKeyParam+"){");
					if(listAuditTrailPerms!=null && listAuditTrailPerms.Count!=0) {
						strb.Append(rn+t3+"ClearFkey("+priKeyParam+");");
					}
					strb.Append(rn+t3+"string command=\"DELETE FROM "+tablename+" \"");
					strb.Append(rn+t4+"+\"WHERE "+priKey.Name+" = \"+POut.Long("+priKeyParam+");");
				}
				strb.Append(rn+t3+"Db.NonQ(command);");
				strb.Append(rn+t2+"}");
			}
			#endregion Delete
			#region Sync
			//Synch-----------------------------------------------------------------------------------------
			if(CrudGenHelper.IsSynchable(typeClass)) {
				strb.Append(rn+rn+t2+"///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>");
				strb.Append(rn+t2+"public static bool Sync(List<"+typeClass.Name+"> listNew,List<"+typeClass.Name+"> listDB) {");
				strb.Append(rn+t3+"//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.");
				strb.Append(rn+t3+"List<"+typeClass.Name+"> listIns    =new List<"+typeClass.Name+">();");
				strb.Append(rn+t3+"List<"+typeClass.Name+"> listUpdNew =new List<"+typeClass.Name+">();");
				strb.Append(rn+t3+"List<"+typeClass.Name+"> listUpdDB  =new List<"+typeClass.Name+">();");
				strb.Append(rn+t3+"List<"+typeClass.Name+"> listDel    =new List<"+typeClass.Name+">();");
				strb.Append(rn+t3+"listNew.Sort(("+typeClass.Name+" x,"+typeClass.Name+" y) => { return x."+priKey.Name+".CompareTo(y."+priKey.Name+"); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.");
				strb.Append(rn+t3+"listDB.Sort(("+typeClass.Name+" x,"+typeClass.Name+" y) => { return x."+priKey.Name+".CompareTo(y."+priKey.Name+"); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.");
				strb.Append(rn+t3+"int idxNew=0;");
				strb.Append(rn+t3+"int idxDB=0;");
				strb.Append(rn+t3+"int rowsUpdatedCount=0;");
				strb.Append(rn+t3+""+typeClass.Name+" fieldNew;");
				strb.Append(rn+t3+""+typeClass.Name+" fieldDB;");
				strb.Append(rn+t3+"//Because both lists have been sorted using the same criteria, we can now walk each list to determine which list contians the next element.  The next element is determined by Primary Key.");
				strb.Append(rn+t3+"//If the New list contains the next item it will be inserted.  If the DB contains the next item, it will be deleted.  If both lists contain the next item, the item will be updated.");
				strb.Append(rn+t3+"while(idxNew<listNew.Count || idxDB<listDB.Count) {");
				strb.Append(rn+t4+"fieldNew=null;");
				strb.Append(rn+t4+"if(idxNew<listNew.Count) {");
				strb.Append(rn+t5+"fieldNew=listNew[idxNew];");
				strb.Append(rn+t4+"}");
				strb.Append(rn+t4+"fieldDB=null;");
				strb.Append(rn+t4+"if(idxDB<listDB.Count) {");
				strb.Append(rn+t5+"fieldDB=listDB[idxDB];");
				strb.Append(rn+t4+"}");
				strb.Append(rn+t4+"//begin compare");
				strb.Append(rn+t4+"if(fieldNew!=null && fieldDB==null) {//listNew has more items, listDB does not.");
				strb.Append(rn+t5+"listIns.Add(fieldNew);");
				strb.Append(rn+t5+"idxNew++;");
				strb.Append(rn+t5+"continue;");
				strb.Append(rn+t4+"}");
				strb.Append(rn+t4+"else if(fieldNew==null && fieldDB!=null) {//listDB has more items, listNew does not.");
				strb.Append(rn+t5+"listDel.Add(fieldDB);");
				strb.Append(rn+t5+"idxDB++;");
				strb.Append(rn+t5+"continue;");
				strb.Append(rn+t4+"}");
				strb.Append(rn+t4+"else if(fieldNew."+priKey.Name+"<fieldDB."+priKey.Name+") {//newPK less than dbPK, newItem is 'next'");
				strb.Append(rn+t5+"listIns.Add(fieldNew);");
				strb.Append(rn+t5+"idxNew++;");
				strb.Append(rn+t5+"continue;");
				strb.Append(rn+t4+"}");
				strb.Append(rn+t4+"else if(fieldNew."+priKey.Name+">fieldDB."+priKey.Name+") {//dbPK less than newPK, dbItem is 'next'");
				strb.Append(rn+t5+"listDel.Add(fieldDB);");
				strb.Append(rn+t5+"idxDB++;");
				strb.Append(rn+t5+"continue;");
				strb.Append(rn+t4+"}");
				strb.Append(rn+t4+"//Both lists contain the 'next' item, update required");
				strb.Append(rn+t4+"listUpdNew.Add(fieldNew);");
				strb.Append(rn+t4+"listUpdDB.Add(fieldDB);");
				strb.Append(rn+t4+"idxNew++;");
				strb.Append(rn+t4+"idxDB++;");
				strb.Append(rn+t3+"}");
				strb.Append(rn+t3+"//Commit changes to DB");
				strb.Append(rn+t3+"for(int i=0;i<listIns.Count;i++) {");
				strb.Append(rn+t4+"Insert(listIns[i]);");
				strb.Append(rn+t3+"}");
				strb.Append(rn+t3+"for(int i=0;i<listUpdNew.Count;i++) {");
				strb.Append(rn+t4+"if(Update(listUpdNew[i],listUpdDB[i])){");
				strb.Append(rn+t5+"rowsUpdatedCount++;");
				strb.Append(rn+t4+"}");
				strb.Append(rn+t3+"}");
				strb.Append(rn+t3+"for(int i=0;i<listDel.Count;i++) {");
				if(CrudGenHelper.IsDeleteForbidden(typeClass)) {
					//When Crud.Delete() is forbidden, the only way we could possibly delete a row is if the S class has a specific Delete() function defined.
					//There are very few classes which are both Synchable and where Crud.Delete() is forbidden.
					strb.Append(rn+t4+typeClass.Name+"s.Delete(listDel[i]."+priKey.Name+");");
				}
				else {
					strb.Append(rn+t4+"Delete(listDel[i]."+priKey.Name+");");
				}
				strb.Append(rn+t3+"}");
				strb.Append(rn+t3+"if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {");
				strb.Append(rn+t4+"return true;");
				strb.Append(rn+t3+"}");
				strb.Append(rn+t3+"return false;");
				strb.Append(rn+t2+"}");
			}
			#endregion
			#region ConvertToM
			if(isMobile) {
				//ConvertToM------------------------------------------------------------------------------------------
				Type typeClassReg=CrudGenHelper.GetTypeFromMType(typeClass.Name,tableTypes);//gets the non-mobile type
				if(typeClassReg==null) {
					strb.Append(rn+rn+t2+"//ConvertToM not applicable.");
				}
				else{
					string tablenameReg=CrudGenHelper.GetTableName(typeClassReg);//in lowercase now.
					string objReg=typeClassReg.Name.Substring(0,1).ToLower()+typeClassReg.Name.Substring(1);//lowercase initial letter.  Example feeSched
					FieldInfo[] fieldsReg=typeClassReg.GetFields();//We can't assume they are in the correct order.
					List<FieldInfo> fieldsInDbReg=CrudGenHelper.GetFieldsExceptNotDb(fieldsReg);
					strb.Append(rn+rn+t2+"///<summary>Converts one "+typeClassReg.Name+" object to its mobile equivalent.  Warning! CustomerNum will always be 0.</summary>");
					strb.Append(rn+t2+"public static "+typeClass.Name+" ConvertToM("+typeClassReg.Name+" "+objReg+"){");
					strb.Append(rn+t3+typeClass.Name+" "+obj+"=new "+typeClass.Name+"();");
					for(int f=0;f<fieldsInDb.Count;f++) {
						if(fieldsInDb[f].Name=="CustomerNum") {
							strb.Append(rn+t3+"//CustomerNum cannot be set.  Remains 0.");
							continue;
						}
						bool matchfound=false;
						for(int r=0;r<fieldsInDbReg.Count;r++) {
							if(fieldsInDb[f].Name==fieldsInDbReg[r].Name) {
								strb.Append(rn+t3+obj+"."+fieldsInDb[f].Name.PadRight(longestField,' ')+"="+objReg+"."+fieldsInDbReg[r].Name+";");
								matchfound=true;
							}
						}
						if(!matchfound) {
							throw new ApplicationException("Match not found.");
						}
					}
					strb.Append(rn+t3+"return "+obj+";");
					strb.Append(rn+t2+"}");
				}
			}
			#endregion ConvertToM
			#region ClearFkey
			if(listAuditTrailPerms!=null && listAuditTrailPerms.Count!=0) {  //If there are any AuditPerms set for this table
				strb.Append(rn+rn+t2+"///<summary>Zeros securitylog FKey column for rows that are using the matching "+priKeyParam+" as FKey and are related to "+typeClass.Name+".");
				strb.Append(rn+t2+"///Permtypes are generated from the AuditPerms property of the CrudTableAttribute within the "+typeClass.Name+@" table type.</summary>");
				strb.Append(rn+t2+"public static void ClearFkey(long "+priKeyParam+") {");
				List<string> listPermTypes=new List<string>();
				for(int i=0;i<listAuditTrailPerms.Count;i++) {
					listPermTypes.Add(((int)listAuditTrailPerms[i]).ToString());
				}
				strb.Append(rn+t3+"string command=\"UPDATE securitylog SET FKey=0 WHERE FKey=\"+POut.Long("+priKeyParam+")+"
					+"\" AND PermType IN ("+String.Join(",",listPermTypes)+")\";");  
				//If we wanted to make this more readable we could put a comment into the crud file here of what the listPermTypes mean.
				strb.Append(rn+t3+"Db.NonQ(command);");
				strb.Append(rn+t2+"}");
			}
			#endregion ClearFkey
			//IsEqual is currently unfinished, but is here so that we can enhance it later to truly compare two objects. 
			//This will check all DB columns and all Non-DB columns for equality and return a boolean. 
			//The problem with implementing this at this time (3/4/2014) is that we don't have time to implement validating lists of objects.
			#region IsEqual
			//IsEqual-------------------------------------------------------------------------------
			//if(!isMobile) {
			//	if(typeClass.Name=="RxPat" || typeClass.Name=="Appointment" || typeClass.Name=="Claim" || typeClass.Name=="LetterMerge") {
			//		List<FieldInfo> allFields=CrudGenHelper.GetAllFieldsExceptPriKey(fields,priKey);
			//		strb.Append(rn+rn+t2+"///<summary>Checks two "+typeClass.Name+" objects for equality. Return true if equal and false if any variables are different. This includes special columns not included in the DB.</summary>");
			//		strb.Append(rn+t2+"public static bool IsEqual("+typeClass.Name+" "+obj+","+typeClass.Name+" "+oldObj+"){");
			//		for(int f=0;f<allFields.Count;f++) {
			//			if(CrudGenHelper.IsNotDbColumn(allFields[f])) {
			//				switch(allFields[f].FieldType.Name) {
			//					case "int":
			//						strb.Append(rn+t3+"if("+obj+"."+allFields[f].Name+" != "+oldObj+"."+allFields[f].Name+") {");
			//						strb.Append(rn+t4+"return false;");
			//						strb.Append(rn+t3+"}");
			//						break;
			//					case "List<int>":
			//						strb.Append(rn+t3+"for(int i=0;i<"+obj+"."+allFields[f].Name+".Count;i++) {");
			//						strb.Append(rn+t4+"if("+obj+"."+allFields[f].Name+" != "+oldObj+"."+allFields[f].Name+") {");
			//						strb.Append(rn+t5+"return false;");
			//						strb.Append(rn+t4+"}");
			//						strb.Append(rn+t3+"}");
			//						break;
			//					case "string":
			//						strb.Append(rn+t3+"if("+obj+"."+allFields[f].Name+" != "+oldObj+"."+allFields[f].Name+") {");
			//						strb.Append(rn+t4+"return false;");
			//						strb.Append(rn+t3+"}");
			//						break;
			//					case "List<string>":
			//						strb.Append(rn+t3+"for(int i=0;i<"+obj+"."+allFields[f].Name+".Count;i++) {");
			//						strb.Append(rn+t4+"if("+obj+"."+allFields[f].Name+" != "+oldObj+"."+allFields[f].Name+") {");
			//						strb.Append(rn+t5+"return false;");
			//						strb.Append(rn+t4+"}");
			//						strb.Append(rn+t3+"}");
			//						break;
			//					case "long":
			//						strb.Append(rn+t3+"if("+obj+"."+allFields[f].Name+" != "+oldObj+"."+allFields[f].Name+") {");
			//						strb.Append(rn+t4+"return false;");
			//						strb.Append(rn+t3+"}");
			//						break;
			//					case "List<long>":
			//						strb.Append(rn+t3+"for(int i=0;i<"+obj+"."+allFields[f].Name+".Count;i++) {");
			//						strb.Append(rn+t4+"if("+obj+"."+allFields[f].Name+" != "+oldObj+"."+allFields[f].Name+") {");
			//						strb.Append(rn+t5+"return false;");
			//						strb.Append(rn+t4+"}");
			//						strb.Append(rn+t3+"}");
			//						break;
			//					case "Color":
			//						strb.Append(rn+t3+"if("+obj+"."+allFields[f].Name+".ToArgb() != "+oldObj+"."+allFields[f].Name+".ToArgb()) {");
			//						strb.Append(rn+t4+"return false;");
			//						strb.Append(rn+t3+"}");
			//						break;
			//					case "DateTime"://This is only for date, not dateT.
			//						strb.Append(rn+t3+"if("+obj+"."+allFields[f].Name+" != "+oldObj+"."+allFields[f].Name+") {");
			//						strb.Append(rn+t4+"return false;");
			//						strb.Append(rn+t3+"}");
			//						break;
			//					case "double":
			//						strb.Append(rn+t3+"if("+obj+"."+allFields[f].Name+" != "+oldObj+"."+allFields[f].Name+") {");
			//						strb.Append(rn+t4+"return false;");
			//						strb.Append(rn+t3+"}");
			//						break;
			//					case "List<double>":
			//						strb.Append(rn+t3+"for(int i=0;i<"+obj+"."+allFields[f].Name+".Count;i++) {");
			//						strb.Append(rn+t4+"if("+obj+"."+allFields[f].Name+" != "+oldObj+"."+allFields[f].Name+") {");
			//						strb.Append(rn+t5+"return false;");
			//						strb.Append(rn+t4+"}");
			//						strb.Append(rn+t3+"}");
			//						break;
			//					case "Interval":
			//						strb.Append(rn+t3+"if("+obj+"."+allFields[f].Name+" != "+oldObj+"."+allFields[f].Name+") {");
			//						strb.Append(rn+t4+"return false;");
			//						strb.Append(rn+t3+"}");
			//						break;
			//					case "bool":
			//						strb.Append(rn+t3+"if("+obj+"."+allFields[f].Name+" != "+oldObj+"."+allFields[f].Name+") {");
			//						strb.Append(rn+t4+"return false;");
			//						strb.Append(rn+t3+"}");
			//						break;
			//					case "TimeSpan":
			//						strb.Append(rn+t3+"if("+obj+"."+allFields[f].Name+".Ticks != "+oldObj+"."+allFields[f].Name+".Ticks) {");
			//						strb.Append(rn+t4+"return false;");
			//						strb.Append(rn+t3+"}");
			//						break;
			//					default:
			//						//TODO: Add a sub-section for s-classes ending in H to have an ES added to them
			//						if(allFields[f].FieldType.Name.StartsWith("List<")) {
			//							strb.Append(rn+t3+"for(int i=0;i<"+obj+"."+allFields[f].Name+".Count;i++) {");
			//							string objName=allFields[f].FieldType.Name.Substring(5);
			//							objName=objName.Remove(objName.Length-1);
			//							string obj2=objName.Substring(0,1).ToLower()+objName.Substring(1);//lowercase initial letter.  Example feeSched
			//							string oldObj2="old"+objName;//Example oldFeeSched
			//							if(allFields[f].FieldType.Name.EndsWith("y")) {
			//								strb.Append(rn+t4+"if(!"+objName.Remove(objName.Length-1)+"ies.IsEqual("+objName+" "+obj2+","+objName+" "+oldObj2+") {");
			//								strb.Append(rn+t5+"return false;");
			//								strb.Append(rn+t4+"}");
			//							}
			//							else {
			//								strb.Append(rn+t4+"if(!"+objName+"s.IsEqual("+objName+" "+obj2+","+objName+" "+oldObj2+") {");
			//								strb.Append(rn+t5+"return false;");
			//								strb.Append(rn+t4+"}");
			//							}
			//							strb.Append(rn+t3+"}");
			//						}
			//						else if(allFields[f].FieldType.Name.EndsWith("[]")) {
			//							strb.Append(rn+t3+"for(int i=0;i<"+obj+"."+allFields[f].Name+".Count;i++) {");
			//							string objName=allFields[f].FieldType.Name.Remove(allFields[f].FieldType.Name.Length-2);
			//							string obj2=objName.Substring(0,1).ToLower()+objName.Substring(1);//lowercase initial letter.  Example feeSched
			//							string oldObj2="old"+objName;//Example oldFeeSched
			//							if(allFields[f].FieldType.Name.EndsWith("y")) {
			//								strb.Append(rn+t4+"if(!"+objName.Remove(objName.Length-1)+"ies.IsEqual("+objName+" "+obj2+","+objName+" "+oldObj2+") {");
			//								strb.Append(rn+t5+"return false;");
			//								strb.Append(rn+t4+"}");
			//							}
			//							else {
			//								strb.Append(rn+t4+"if(!"+objName+"s.IsEqual("+objName+" "+obj2+","+objName+" "+oldObj2+") {");
			//								strb.Append(rn+t5+"return false;");
			//								strb.Append(rn+t4+"}");
			//							}
			//							strb.Append(rn+t3+"}");
			//						}
			//						else {
			//							string obj2=allFields[f].FieldType.Name.Substring(0,1).ToLower()+typeClass.Name.Substring(1);//lowercase initial letter.  Example feeSched
			//							string oldObj2="old"+allFields[f].FieldType.Name;//Example oldFeeSched
			//							if(allFields[f].FieldType.Name.EndsWith("y")) {
			//								strb.Append(rn+t3+"if(!"+allFields[f].FieldType.Name.Remove(allFields[f].FieldType.Name.Length-1)+"ies.IsEqual("+allFields[f].FieldType.Name+" "+obj2+","+allFields[f].FieldType.Name+oldObj2+") {");
			//								strb.Append(rn+t4+"return false;");
			//								strb.Append(rn+t3+"}");
			//							}
			//							else {
			//								strb.Append(rn+t3+"if(!"+allFields[f].FieldType.Name+"s.IsEqual("+allFields[f].FieldType.Name+" "+obj2+","+allFields[f].FieldType.Name+oldObj2+") {");
			//								strb.Append(rn+t4+"return false;");
			//								strb.Append(rn+t3+"}");
			//							}
			//						}
			//						break;
			//				}
			//			}
			//			//TODO: Check all special crud column types and hanlde their equals in their own way
			//			strb.Append(rn+t3+"if("+obj+"."+allFields[f].Name+" != "+oldObj+"."+allFields[f].Name+") {");
			//			strb.Append(rn+t4+"return false;");
			//			strb.Append(rn+t3+"}");
			//		}
			//		strb.Append(rn+t3+"return true;");
			//		strb.Append(rn+t2+"}");
			//	}
			//}
			#endregion IsEqual
			//Footer
			strb.Append(rn);
			strb.Append(@"
	}
}");
		}

		/// <summary>
		/// 
		/// </summary>
		private void butSnippet_Click(object sender,EventArgs e) {
			if(listClass.SelectedIndex==-1){
				MessageBox.Show("Please select a class.");
				return;
			}
			//if(comboType.SelectedIndex==-1){
			//	MessageBox.Show("Please select a type.");
			//	return;
			//}
			Type typeClass=tableTypesAll[listClass.SelectedIndex];
			SnippetType snipType=(SnippetType)comboType.SelectedIndex;
			bool isMobile=CrudGenHelper.IsMobile(typeClass);
			string snippet=CrudGenDataInterface.GetSnippet(typeClass,snipType,isMobile);
			textSnippet.Text=snippet;
			Clipboard.SetText(snippet);
		}


	}
}
