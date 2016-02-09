using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
		public static void Update(ProgramProperty programProp){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),programProp);
				return;
			}
			Crud.ProgramPropertyCrud.Update(programProp);
		}

		///<summary>This is called from FormClinicEdit and from InsertOrUpdateLocalOverridePath.  PayConnect can have clinic specific login credentials,
		///so the ProgramProperties for PayConnect are duplicated for each clinic.  The properties duplicated are Username, Password, and PaymentType.
		///There's also a 'Headquarters' or no clinic set of these props with ClinicNum 0, which is the set of props inserted with each new clinic.</summary>
		public static long Insert(ProgramProperty programProp){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				programProp.ProgramPropertyNum=Meth.GetLong(MethodBase.GetCurrentMethod(),programProp);
				return programProp.ProgramPropertyNum;
			}
			return Crud.ProgramPropertyCrud.Insert(programProp);
		}

		///<summary>Safe to call on any program. Only returns true if the program is not enabled AND the program has a property of "Disable Advertising" = 1.</summary>
		public static bool IsAdvertisingDisabled(ProgramName progName) {
			Program program = Programs.GetCur(progName);
			if(program==null || program.Enabled) {
				return false;//do not block advertising
			}
			return GetListForProgram(program.ProgramNum).Any(x => x.PropertyDesc=="Disable Advertising" && x.PropertyValue=="1");
		}

		///<summary>Returns a List of ProgramProperties attached to the specified programNum.  Does not include path overrides.</summary>
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

		///<summary>Returns a list of ProgramProperties with the specified programNum and the specified clinicNum from the cache.
		///To get properties when clinics are not enabled or properties for 'Headquarters' use clinicNum 0.
		///Does not include path overrides.</summary>
		public static List<ProgramProperty> GetListForProgramAndClinic(long programNum,long clinicNum) {
			//No need to check RemotingRole; no call to db.
			return ProgramPropertyC.GetListt().FindAll(x => x.ProgramNum==programNum)
				.FindAll(x => x.ClinicNum==clinicNum)
				.FindAll(x => x.PropertyDesc!="");
		}

		///<summary>Returns an ArrayList of ProgramProperties attached to the specified programNum.  Does not include path overrides.
		///Uses thread-safe caching pattern.  Each call to this method creates an copy of the entire ProgramProperty cache.</summary>
		public static List<ProgramProperty> GetForProgram(long programNum) {
			//No need to check RemotingRole; no call to db.
			return ProgramPropertyC.GetListt().FindAll(x => x.ProgramNum==programNum && x.PropertyDesc!="");
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
		public static ProgramProperty GetCur(List<ProgramProperty> arrayForProgram, string desc){
			//No need to check RemotingRole; no call to db.
			return arrayForProgram.FirstOrDefault(x => x.PropertyDesc==desc);
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

		public static string GetPropVal(ProgramName programName,string desc) {
			//No need to check RemotingRole; no call to db.
			long programNum=Programs.GetProgramNum(programName);
			return GetPropVal(programNum,desc);
		}

		///<summary>Returns the PropertyVal for programNum and clinicNum specified with the description specified.  If the property doesn't exist,
		///returns an empty string.  For the PropertyVal for 'Headquarters' or clincs not enabled, use clinicNum 0.</summary>
		public static string GetPropVal(long programNum,string desc,long clinicNum) {
			return GetPropValFromList(ProgramPropertyC.GetListt().FindAll(x => x.ProgramNum==programNum),desc,clinicNum);
		}

		///<summary>Returns the PropertyVal from the list by PropertyDesc and ClinicNum.
		///For the 'Headquarters' or for clinics not enabled, omit clinicNum or send clinicNum 0.  If not found returns an empty string.
		///Primarily used when a local list has been copied from the cache and may differ from what's in the database.  Also possibly useful if dealing with a filtered list </summary>
		public static string GetPropValFromList(List<ProgramProperty> listProps,string propertyDesc,long clinicNum=0) {
			string retval="";
			ProgramProperty prop=listProps.Where(x => x.ClinicNum==clinicNum).Where(x => x.PropertyDesc==propertyDesc).FirstOrDefault();
			if(prop!=null) {
				retval=prop.PropertyValue;
			}
			return retval;
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

		public static void Sync(List<ProgramProperty> listProgPropsNew,long programNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listProgPropsNew,programNum);
				return;
			}
			//prevents delete of program properties for clinics added while editing program properties.
			List<long> listClinicNums = listProgPropsNew.Select(x => x.ClinicNum).Distinct().ToList();
			List<ProgramProperty> listProgPropsDb=ProgramPropertyC.GetListt().FindAll(x => x.ProgramNum==programNum && x.PropertyDesc!="" && listClinicNums.Contains(x.ClinicNum));
			Crud.ProgramPropertyCrud.Sync(listProgPropsNew,listProgPropsDb);
		}

	}
}










