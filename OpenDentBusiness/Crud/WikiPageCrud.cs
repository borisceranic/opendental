//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class WikiPageCrud {
		///<summary>Gets one WikiPage object from the database using the primary key.  Returns null if not found.</summary>
		public static WikiPage SelectOne(long wikiPageNum){
			string command="SELECT * FROM wikipage "
				+"WHERE WikiPageNum = "+POut.Long(wikiPageNum);
			List<WikiPage> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one WikiPage object from the database using a query.</summary>
		public static WikiPage SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<WikiPage> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of WikiPage objects from the database using a query.</summary>
		public static List<WikiPage> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<WikiPage> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<WikiPage> TableToList(DataTable table){
			List<WikiPage> retVal=new List<WikiPage>();
			WikiPage wikiPage;
			foreach(DataRow row in table.Rows) {
				wikiPage=new WikiPage();
				wikiPage.WikiPageNum  = PIn.Long  (row["WikiPageNum"].ToString());
				wikiPage.UserNum      = PIn.Long  (row["UserNum"].ToString());
				wikiPage.PageTitle    = PIn.String(row["PageTitle"].ToString());
				wikiPage.KeyWords     = PIn.String(row["KeyWords"].ToString());
				wikiPage.PageContent  = PIn.String(row["PageContent"].ToString());
				wikiPage.DateTimeSaved= PIn.DateT (row["DateTimeSaved"].ToString());
				retVal.Add(wikiPage);
			}
			return retVal;
		}

		///<summary>Inserts one WikiPage into the database.  Returns the new priKey.</summary>
		public static long Insert(WikiPage wikiPage){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				wikiPage.WikiPageNum=DbHelper.GetNextOracleKey("wikipage","WikiPageNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(wikiPage,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							wikiPage.WikiPageNum++;
							loopcount++;
						}
						else{
							throw ex;
						}
					}
				}
				throw new ApplicationException("Insert failed.  Could not generate primary key.");
			}
			else {
				return Insert(wikiPage,false);
			}
		}

		///<summary>Inserts one WikiPage into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(WikiPage wikiPage,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				wikiPage.WikiPageNum=ReplicationServers.GetKey("wikipage","WikiPageNum");
			}
			string command="INSERT INTO wikipage (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="WikiPageNum,";
			}
			command+="UserNum,PageTitle,KeyWords,PageContent,DateTimeSaved) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(wikiPage.WikiPageNum)+",";
			}
			command+=
				     POut.Long  (wikiPage.UserNum)+","
				+"'"+POut.String(wikiPage.PageTitle)+"',"
				+"'"+POut.String(wikiPage.KeyWords)+"',"
				+"'"+POut.String(wikiPage.PageContent)+"',"
				+    DbHelper.Now()+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				wikiPage.WikiPageNum=Db.NonQ(command,true);
			}
			return wikiPage.WikiPageNum;
		}

		///<summary>Inserts one WikiPage into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(WikiPage wikiPage){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(wikiPage,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					wikiPage.WikiPageNum=DbHelper.GetNextOracleKey("wikipage","WikiPageNum"); //Cacheless method
				}
				return InsertNoCache(wikiPage,true);
			}
		}

		///<summary>Inserts one WikiPage into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(WikiPage wikiPage,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO wikipage (";
			if(!useExistingPK && isRandomKeys) {
				wikiPage.WikiPageNum=ReplicationServers.GetKeyNoCache("wikipage","WikiPageNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="WikiPageNum,";
			}
			command+="UserNum,PageTitle,KeyWords,PageContent,DateTimeSaved) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(wikiPage.WikiPageNum)+",";
			}
			command+=
				     POut.Long  (wikiPage.UserNum)+","
				+"'"+POut.String(wikiPage.PageTitle)+"',"
				+"'"+POut.String(wikiPage.KeyWords)+"',"
				+"'"+POut.String(wikiPage.PageContent)+"',"
				+    DbHelper.Now()+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				wikiPage.WikiPageNum=Db.NonQ(command,true);
			}
			return wikiPage.WikiPageNum;
		}

		///<summary>Updates one WikiPage in the database.</summary>
		public static void Update(WikiPage wikiPage){
			string command="UPDATE wikipage SET "
				+"UserNum      =  "+POut.Long  (wikiPage.UserNum)+", "
				+"PageTitle    = '"+POut.String(wikiPage.PageTitle)+"', "
				+"KeyWords     = '"+POut.String(wikiPage.KeyWords)+"', "
				+"PageContent  = '"+POut.String(wikiPage.PageContent)+"', "
				//DateTimeSaved not allowed to change
				+"WHERE WikiPageNum = "+POut.Long(wikiPage.WikiPageNum);
			Db.NonQ(command);
		}

		///<summary>Updates one WikiPage in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(WikiPage wikiPage,WikiPage oldWikiPage){
			string command="";
			if(wikiPage.UserNum != oldWikiPage.UserNum) {
				if(command!=""){ command+=",";}
				command+="UserNum = "+POut.Long(wikiPage.UserNum)+"";
			}
			if(wikiPage.PageTitle != oldWikiPage.PageTitle) {
				if(command!=""){ command+=",";}
				command+="PageTitle = '"+POut.String(wikiPage.PageTitle)+"'";
			}
			if(wikiPage.KeyWords != oldWikiPage.KeyWords) {
				if(command!=""){ command+=",";}
				command+="KeyWords = '"+POut.String(wikiPage.KeyWords)+"'";
			}
			if(wikiPage.PageContent != oldWikiPage.PageContent) {
				if(command!=""){ command+=",";}
				command+="PageContent = '"+POut.String(wikiPage.PageContent)+"'";
			}
			//DateTimeSaved not allowed to change
			if(command==""){
				return false;
			}
			command="UPDATE wikipage SET "+command
				+" WHERE WikiPageNum = "+POut.Long(wikiPage.WikiPageNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(WikiPage,WikiPage) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(WikiPage wikiPage,WikiPage oldWikiPage) {
			if(wikiPage.UserNum != oldWikiPage.UserNum) {
				return true;
			}
			if(wikiPage.PageTitle != oldWikiPage.PageTitle) {
				return true;
			}
			if(wikiPage.KeyWords != oldWikiPage.KeyWords) {
				return true;
			}
			if(wikiPage.PageContent != oldWikiPage.PageContent) {
				return true;
			}
			//DateTimeSaved not allowed to change
			return false;
		}

		///<summary>Deletes one WikiPage from the database.</summary>
		public static void Delete(long wikiPageNum){
			string command="DELETE FROM wikipage "
				+"WHERE WikiPageNum = "+POut.Long(wikiPageNum);
			Db.NonQ(command);
		}

	}
}