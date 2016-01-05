using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Bugs{

		//[Serializable]
		//public class Version {
		//	public int Major;
		//	public int Minor;
		//	public int Revision;
		//	///<summary>Allows for quick integer sorting. Formatted MMMmmmrrr as a long</summary>
		//	public long Sortable;
		//}

		//public static List<Version> GetBugVersions() {
		//	if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
		//		return Meth.GetObject<List<Version>>(MethodBase.GetCurrentMethod());
		//	}
		//	string command="SELECT CONCAT(VersionsFound,';',VersionsFixed) FROM bug";
		//	DataTable table=Crud.BugCrud.BugDb.GetTable(command);
		//	List<string> listVerionsStr=new List<string>();
		//	foreach(DataRow row in table.Rows) {
		//		listVerionsStr.Add(row[0].ToString());
		//	}
		//	listVerionsStr=listVerionsStr.SelectMany(x=>x.Split(new[] {";"},StringSplitOptions.RemoveEmptyEntries)).Distinct().ToList();
		//	List<Version> listVersions=listVerionsStr.Select(x=>new Version {
		//		Major=PIn.Int(x.Split('.')[0]),
		//		Minor=PIn.Int(x.Split('.')[1]),
		//		Revision=PIn.Int(x.Split('.')[0])
		//	}).ToList();
		//	listVersions.ForEach(x=>x.Sortable=PIn.Long(x.Major+x.Minor.ToString().PadLeft(3,'0')+x.Revision.ToString().PadLeft(3,'0')));
		//	return listVersions.OrderBy(x=>x.Sortable).ToList();
		//}

		///<summary>Must pass in version as "Maj" or "Maj.Min" or "Maj.Min.Rev". Uses like operator.</summary>
		public static List<Bug> GetByVersion(string versionMajMin,string filter="") {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Bug>>(MethodBase.GetCurrentMethod(),versionMajMin,filter);
			}
			string command="SELECT * FROM bug WHERE (VersionsFound LIKE '"+POut.String(versionMajMin)+"%' OR VersionsFound LIKE '%;"+POut.String(versionMajMin)+"%' OR "+
				"VersionsFixed LIKE '"+POut.String(versionMajMin)+"%' OR VersionsFixed LIKE '%;"+POut.String(versionMajMin)+"%') ";
			if(filter!="") {
				command+="AND Description LIKE '%"+POut.String(filter)+"%'";
			}
			return Crud.BugCrud.SelectMany(command);
		}

		///<summary></summary>
		public static List<Bug> GetAll() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Bug>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM bug ORDER BY CreationDate DESC";
			return Crud.BugCrud.SelectMany(command);
		}

		///<summary>Gets one Bug from the db.</summary>
		public static Bug GetOne(long bugId) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Bug>(MethodBase.GetCurrentMethod(),bugId);
			}
			return Crud.BugCrud.SelectOne(bugId);
		}

		///<summary></summary>
		public static long Insert(Bug bug) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				bug.BugId=Meth.GetLong(MethodBase.GetCurrentMethod(),bug);
				return bug.BugId;
			}
			return Crud.BugCrud.Insert(bug);
		}

		///<summary></summary>
		public static void Update(Bug bug) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),bug);
				return;
			}
			Crud.BugCrud.Update(bug);
		}

		///<summary></summary>
		public static void Delete(long bugId) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),bugId);
				return;
			}
			Crud.BugCrud.Delete(bugId);
		}



	}
}