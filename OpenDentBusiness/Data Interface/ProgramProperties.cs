using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness {

	///<summary></summary>
	public class ProgramProperties{
		///<summary></summary>
		public static DataTable RefreshCache() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM programproperty";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="ProgramProperty";
			FillCache(table);
			return table;
		}
	
		///<summary></summary>
		public static void FillCache(DataTable table) {
			//No need to check RemotingRole; no call to db.
			List<ProgramProperty> listProgramProperties=Crud.ProgramPropertyCrud.TableToList(table);
			//This is where code should go if there is ever a short list implemented for program properties.
			ProgramPropertyC.Listt=listProgramProperties;
		}

		///<summary></summary>
		public static void Update(ProgramProperty Cur){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),Cur);
				return;
			}
			Crud.ProgramPropertyCrud.Update(Cur);
		}

		///<summary>This can only be called from ClassConversions. Users not allowed to add properties so there is no user interface.</summary>
		public static long Insert(ProgramProperty Cur){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Cur.ProgramPropertyNum=Meth.GetLong(MethodBase.GetCurrentMethod(),Cur);
				return Cur.ProgramPropertyNum;
			}
			return Crud.ProgramPropertyCrud.Insert(Cur);
		}

		///<summary>Returns a List of programproperties attached to the specified programNum.  Does not include path overrides.
		///DO NOT MODIFY the returned properties.  Read only.</summary>
		public static List<ProgramProperty> GetListForProgram(long programNum) {
			//No need to check RemotingRole; no call to db.
			List<ProgramProperty> listProgPropsResult=new List<ProgramProperty>();
			List<ProgramProperty> listProgPropsCache=ProgramPropertyC.GetListt();
			for(int i=0;i<listProgPropsCache.Count;i++) {
				if(listProgPropsCache[i].ProgramNum==programNum && listProgPropsCache[i].PropertyDesc!="") {
					listProgPropsResult.Add(listProgPropsCache[i]);
				}
			}
			return listProgPropsResult;
		}

		///<summary>Returns an ArrayList of programproperties attached to the specified programNum.  Does not include path overrides.
		///DO NOT MODIFY the returned properties.  Read only.</summary>
		public static ArrayList GetForProgram(long programNum) {
			//No need to check RemotingRole; no call to db.
			ArrayList ForProgram=new ArrayList();
			List<ProgramProperty> listProgramProperties=ProgramPropertyC.GetListt();
			for(int i=0;i<listProgramProperties.Count;i++) {
				if(listProgramProperties[i].ProgramNum==programNum && listProgramProperties[i].PropertyDesc!="") {
					ForProgram.Add(listProgramProperties[i]);
				}
			}
			return ForProgram;
		}

		public static void SetProperty(long programNum,string desc,string propval) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),programNum,desc,propval);
				return;
			}
			string command="UPDATE programproperty SET PropertyValue='"+POut.String(propval)+"' "
				+"WHERE ProgramNum="+POut.Long(programNum)+" "
				+"AND PropertyDesc='"+POut.String(desc)+"'";
			Db.NonQ(command);
		}

		///<summary>After GetForProgram has been run, this gets one of those properties.  DO NOT MODIFY the returned property.  Read only.</summary>
		public static ProgramProperty GetCur(ArrayList ForProgram, string desc){
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<ForProgram.Count;i++){
				if(((ProgramProperty)ForProgram[i]).PropertyDesc==desc){
					return (ProgramProperty)ForProgram[i];
				}
			}
			return null;
		}

		public static string GetPropVal(long programNum,string desc) {
			//No need to check RemotingRole; no call to db.
			List<ProgramProperty> listProgramProperties=ProgramPropertyC.GetListt();
			for(int i=0;i<listProgramProperties.Count;i++) {
				if(listProgramProperties[i].ProgramNum!=programNum) {
					continue;
				}
				if(listProgramProperties[i].PropertyDesc!=desc) {
					continue;
				}
				return listProgramProperties[i].PropertyValue;
			}
			throw new ApplicationException("Property not found: "+desc);
		}

		public static string GetPropVal(ProgramName progName,string desc) {
			//No need to check RemotingRole; no call to db.
			long programNum=Programs.GetProgramNum(progName);
			return GetPropVal(programNum,desc);
		}

		///<summary>Returns the property with the matching description from the provided list.  Null if the property cannot be found by the description.</summary>
		public static ProgramProperty GetPropByDesc(string propertyDesc,List<ProgramProperty> listProperties) {
			//No need to check RemotingRole; no call to db.
			ProgramProperty property=null;
			for(int i=0;i<listProperties.Count;i++) {
				if(listProperties[i].PropertyDesc==propertyDesc) {
					property=listProperties[i];
					break;
				}
			}
			return property;
		}

		///<summary>Used in FormUAppoint to get frequent and current data.</summary>
		public static string GetValFromDb(long programNum,string desc) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),programNum,desc);
			}
			string command="SELECT PropertyValue FROM programproperty WHERE ProgramNum="+POut.Long(programNum)
				+" AND PropertyDesc='"+POut.String(desc)+"'";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0){
				return "";
			}
			return table.Rows[0][0].ToString();
		}

		///<summary>Returns the path override for the current computer and the specified programNum.  Returns empty string if no override found.</summary>
		public static string GetLocalPathOverrideForProgram(long programNum) {
			//No need to check RemotingRole; no call to db.
			List<ProgramProperty> listProgramProperties=ProgramPropertyC.GetListt();
			for(int i=0;i<listProgramProperties.Count;i++) {
				if(listProgramProperties[i].ProgramNum==programNum
					&& listProgramProperties[i].PropertyDesc==""
					&& listProgramProperties[i].ComputerName.ToUpper()==Environment.MachineName.ToUpper()) 
				{
					return listProgramProperties[i].PropertyValue;
				}
			}
			return "";
		}

		///<summary>This will insert or update a local path override property for the specified programNum.</summary>
		public static void InsertOrUpdateLocalOverridePath(long programNum,string newPath) {
			//No need to check RemotingRole; no call to db.
			List<ProgramProperty> listProgramProperties=ProgramPropertyC.GetListt();
			for(int i=0;i<listProgramProperties.Count;i++) {
				if(listProgramProperties[i].ProgramNum==programNum
					&& listProgramProperties[i].PropertyDesc==""
					&& listProgramProperties[i].ComputerName.ToUpper()==Environment.MachineName.ToUpper()) 
				{
					listProgramProperties[i].PropertyValue=newPath;
					ProgramProperties.Update(listProgramProperties[i]);
					return;//Will only be one override per computer per program.
				}
			}
			//Path override does not exist for the current computer so create a new one.
			ProgramProperty pp=new ProgramProperty();
			pp.ProgramNum=programNum;
			pp.PropertyValue=newPath;
			pp.ComputerName=Environment.MachineName.ToUpper();
			ProgramProperties.Insert(pp);
		}




		
	}

	

	


}










