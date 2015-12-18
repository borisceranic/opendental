//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class StatementCrud {
		///<summary>Gets one Statement object from the database using the primary key.  Returns null if not found.</summary>
		public static Statement SelectOne(long statementNum){
			string command="SELECT * FROM statement "
				+"WHERE StatementNum = "+POut.Long(statementNum);
			List<Statement> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Statement object from the database using a query.</summary>
		public static Statement SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Statement> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Statement objects from the database using a query.</summary>
		public static List<Statement> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Statement> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Statement> TableToList(DataTable table){
			List<Statement> retVal=new List<Statement>();
			Statement statement;
			foreach(DataRow row in table.Rows) {
				statement=new Statement();
				statement.StatementNum = PIn.Long  (row["StatementNum"].ToString());
				statement.PatNum       = PIn.Long  (row["PatNum"].ToString());
				statement.DateSent     = PIn.Date  (row["DateSent"].ToString());
				statement.DateRangeFrom= PIn.Date  (row["DateRangeFrom"].ToString());
				statement.DateRangeTo  = PIn.Date  (row["DateRangeTo"].ToString());
				statement.Note         = PIn.String(row["Note"].ToString());
				statement.NoteBold     = PIn.String(row["NoteBold"].ToString());
				statement.Mode_        = (OpenDentBusiness.StatementMode)PIn.Int(row["Mode_"].ToString());
				statement.HidePayment  = PIn.Bool  (row["HidePayment"].ToString());
				statement.SinglePatient= PIn.Bool  (row["SinglePatient"].ToString());
				statement.Intermingled = PIn.Bool  (row["Intermingled"].ToString());
				statement.IsSent       = PIn.Bool  (row["IsSent"].ToString());
				statement.DocNum       = PIn.Long  (row["DocNum"].ToString());
				statement.DateTStamp   = PIn.DateT (row["DateTStamp"].ToString());
				statement.IsReceipt    = PIn.Bool  (row["IsReceipt"].ToString());
				statement.IsInvoice    = PIn.Bool  (row["IsInvoice"].ToString());
				statement.IsInvoiceCopy= PIn.Bool  (row["IsInvoiceCopy"].ToString());
				statement.EmailSubject = PIn.String(row["EmailSubject"].ToString());
				statement.EmailBody    = PIn.String(row["EmailBody"].ToString());
				retVal.Add(statement);
			}
			return retVal;
		}

		///<summary>Converts a list of Statement into a DataTable.</summary>
		public static DataTable ListToTable(List<Statement> listStatements,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="Statement";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("StatementNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("DateSent");
			table.Columns.Add("DateRangeFrom");
			table.Columns.Add("DateRangeTo");
			table.Columns.Add("Note");
			table.Columns.Add("NoteBold");
			table.Columns.Add("Mode_");
			table.Columns.Add("HidePayment");
			table.Columns.Add("SinglePatient");
			table.Columns.Add("Intermingled");
			table.Columns.Add("IsSent");
			table.Columns.Add("DocNum");
			table.Columns.Add("DateTStamp");
			table.Columns.Add("IsReceipt");
			table.Columns.Add("IsInvoice");
			table.Columns.Add("IsInvoiceCopy");
			table.Columns.Add("EmailSubject");
			table.Columns.Add("EmailBody");
			foreach(Statement statement in listStatements) {
				table.Rows.Add(new object[] {
					POut.Long  (statement.StatementNum),
					POut.Long  (statement.PatNum),
					POut.Date  (statement.DateSent),
					POut.Date  (statement.DateRangeFrom),
					POut.Date  (statement.DateRangeTo),
					POut.String(statement.Note),
					POut.String(statement.NoteBold),
					POut.Int   ((int)statement.Mode_),
					POut.Bool  (statement.HidePayment),
					POut.Bool  (statement.SinglePatient),
					POut.Bool  (statement.Intermingled),
					POut.Bool  (statement.IsSent),
					POut.Long  (statement.DocNum),
					POut.DateT (statement.DateTStamp),
					POut.Bool  (statement.IsReceipt),
					POut.Bool  (statement.IsInvoice),
					POut.Bool  (statement.IsInvoiceCopy),
					POut.String(statement.EmailSubject),
					POut.String(statement.EmailBody),
				});
			}
			return table;
		}

		///<summary>Inserts one Statement into the database.  Returns the new priKey.</summary>
		public static long Insert(Statement statement){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				statement.StatementNum=DbHelper.GetNextOracleKey("statement","StatementNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(statement,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							statement.StatementNum++;
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
				return Insert(statement,false);
			}
		}

		///<summary>Inserts one Statement into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Statement statement,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				statement.StatementNum=ReplicationServers.GetKey("statement","StatementNum");
			}
			string command="INSERT INTO statement (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="StatementNum,";
			}
			command+="PatNum,DateSent,DateRangeFrom,DateRangeTo,Note,NoteBold,Mode_,HidePayment,SinglePatient,Intermingled,IsSent,DocNum,IsReceipt,IsInvoice,IsInvoiceCopy,EmailSubject,EmailBody) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(statement.StatementNum)+",";
			}
			command+=
				     POut.Long  (statement.PatNum)+","
				+    POut.Date  (statement.DateSent)+","
				+    POut.Date  (statement.DateRangeFrom)+","
				+    POut.Date  (statement.DateRangeTo)+","
				+"'"+POut.String(statement.Note)+"',"
				+"'"+POut.String(statement.NoteBold)+"',"
				+    POut.Int   ((int)statement.Mode_)+","
				+    POut.Bool  (statement.HidePayment)+","
				+    POut.Bool  (statement.SinglePatient)+","
				+    POut.Bool  (statement.Intermingled)+","
				+    POut.Bool  (statement.IsSent)+","
				+    POut.Long  (statement.DocNum)+","
				//DateTStamp can only be set by MySQL
				+    POut.Bool  (statement.IsReceipt)+","
				+    POut.Bool  (statement.IsInvoice)+","
				+    POut.Bool  (statement.IsInvoiceCopy)+","
				+"'"+POut.String(statement.EmailSubject)+"',"
				+    DbHelper.ParamChar+"paramEmailBody)";
			if(statement.EmailBody==null) {
				statement.EmailBody="";
			}
			OdSqlParameter paramEmailBody=new OdSqlParameter("paramEmailBody",OdDbType.Text,statement.EmailBody);
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramEmailBody);
			}
			else {
				statement.StatementNum=Db.NonQ(command,true,paramEmailBody);
			}
			return statement.StatementNum;
		}

		///<summary>Inserts one Statement into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Statement statement){
			if(DataConnection.DBtype==DatabaseType.MySql) {
				return InsertNoCache(statement,false);
			}
			else {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					statement.StatementNum=DbHelper.GetNextOracleKey("statement","StatementNum"); //Cacheless method
				}
				return InsertNoCache(statement,true);
			}
		}

		///<summary>Inserts one Statement into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Statement statement,bool useExistingPK){
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO statement (";
			if(!useExistingPK && isRandomKeys) {
				statement.StatementNum=ReplicationServers.GetKeyNoCache("statement","StatementNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="StatementNum,";
			}
			command+="PatNum,DateSent,DateRangeFrom,DateRangeTo,Note,NoteBold,Mode_,HidePayment,SinglePatient,Intermingled,IsSent,DocNum,IsReceipt,IsInvoice,IsInvoiceCopy,EmailSubject,EmailBody) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(statement.StatementNum)+",";
			}
			command+=
				     POut.Long  (statement.PatNum)+","
				+    POut.Date  (statement.DateSent)+","
				+    POut.Date  (statement.DateRangeFrom)+","
				+    POut.Date  (statement.DateRangeTo)+","
				+"'"+POut.String(statement.Note)+"',"
				+"'"+POut.String(statement.NoteBold)+"',"
				+    POut.Int   ((int)statement.Mode_)+","
				+    POut.Bool  (statement.HidePayment)+","
				+    POut.Bool  (statement.SinglePatient)+","
				+    POut.Bool  (statement.Intermingled)+","
				+    POut.Bool  (statement.IsSent)+","
				+    POut.Long  (statement.DocNum)+","
				//DateTStamp can only be set by MySQL
				+    POut.Bool  (statement.IsReceipt)+","
				+    POut.Bool  (statement.IsInvoice)+","
				+    POut.Bool  (statement.IsInvoiceCopy)+","
				+"'"+POut.String(statement.EmailSubject)+"',"
				+    DbHelper.ParamChar+"paramEmailBody)";
			if(statement.EmailBody==null) {
				statement.EmailBody="";
			}
			OdSqlParameter paramEmailBody=new OdSqlParameter("paramEmailBody",OdDbType.Text,statement.EmailBody);
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramEmailBody);
			}
			else {
				statement.StatementNum=Db.NonQ(command,true,paramEmailBody);
			}
			return statement.StatementNum;
		}

		///<summary>Updates one Statement in the database.</summary>
		public static void Update(Statement statement){
			string command="UPDATE statement SET "
				+"PatNum       =  "+POut.Long  (statement.PatNum)+", "
				+"DateSent     =  "+POut.Date  (statement.DateSent)+", "
				+"DateRangeFrom=  "+POut.Date  (statement.DateRangeFrom)+", "
				+"DateRangeTo  =  "+POut.Date  (statement.DateRangeTo)+", "
				+"Note         = '"+POut.String(statement.Note)+"', "
				+"NoteBold     = '"+POut.String(statement.NoteBold)+"', "
				+"Mode_        =  "+POut.Int   ((int)statement.Mode_)+", "
				+"HidePayment  =  "+POut.Bool  (statement.HidePayment)+", "
				+"SinglePatient=  "+POut.Bool  (statement.SinglePatient)+", "
				+"Intermingled =  "+POut.Bool  (statement.Intermingled)+", "
				+"IsSent       =  "+POut.Bool  (statement.IsSent)+", "
				+"DocNum       =  "+POut.Long  (statement.DocNum)+", "
				//DateTStamp can only be set by MySQL
				+"IsReceipt    =  "+POut.Bool  (statement.IsReceipt)+", "
				+"IsInvoice    =  "+POut.Bool  (statement.IsInvoice)+", "
				+"IsInvoiceCopy=  "+POut.Bool  (statement.IsInvoiceCopy)+", "
				+"EmailSubject = '"+POut.String(statement.EmailSubject)+"', "
				+"EmailBody    =  "+DbHelper.ParamChar+"paramEmailBody "
				+"WHERE StatementNum = "+POut.Long(statement.StatementNum);
			if(statement.EmailBody==null) {
				statement.EmailBody="";
			}
			OdSqlParameter paramEmailBody=new OdSqlParameter("paramEmailBody",OdDbType.Text,statement.EmailBody);
			Db.NonQ(command,paramEmailBody);
		}

		///<summary>Updates one Statement in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Statement statement,Statement oldStatement){
			string command="";
			if(statement.PatNum != oldStatement.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(statement.PatNum)+"";
			}
			if(statement.DateSent.Date != oldStatement.DateSent.Date) {
				if(command!=""){ command+=",";}
				command+="DateSent = "+POut.Date(statement.DateSent)+"";
			}
			if(statement.DateRangeFrom.Date != oldStatement.DateRangeFrom.Date) {
				if(command!=""){ command+=",";}
				command+="DateRangeFrom = "+POut.Date(statement.DateRangeFrom)+"";
			}
			if(statement.DateRangeTo.Date != oldStatement.DateRangeTo.Date) {
				if(command!=""){ command+=",";}
				command+="DateRangeTo = "+POut.Date(statement.DateRangeTo)+"";
			}
			if(statement.Note != oldStatement.Note) {
				if(command!=""){ command+=",";}
				command+="Note = '"+POut.String(statement.Note)+"'";
			}
			if(statement.NoteBold != oldStatement.NoteBold) {
				if(command!=""){ command+=",";}
				command+="NoteBold = '"+POut.String(statement.NoteBold)+"'";
			}
			if(statement.Mode_ != oldStatement.Mode_) {
				if(command!=""){ command+=",";}
				command+="Mode_ = "+POut.Int   ((int)statement.Mode_)+"";
			}
			if(statement.HidePayment != oldStatement.HidePayment) {
				if(command!=""){ command+=",";}
				command+="HidePayment = "+POut.Bool(statement.HidePayment)+"";
			}
			if(statement.SinglePatient != oldStatement.SinglePatient) {
				if(command!=""){ command+=",";}
				command+="SinglePatient = "+POut.Bool(statement.SinglePatient)+"";
			}
			if(statement.Intermingled != oldStatement.Intermingled) {
				if(command!=""){ command+=",";}
				command+="Intermingled = "+POut.Bool(statement.Intermingled)+"";
			}
			if(statement.IsSent != oldStatement.IsSent) {
				if(command!=""){ command+=",";}
				command+="IsSent = "+POut.Bool(statement.IsSent)+"";
			}
			if(statement.DocNum != oldStatement.DocNum) {
				if(command!=""){ command+=",";}
				command+="DocNum = "+POut.Long(statement.DocNum)+"";
			}
			//DateTStamp can only be set by MySQL
			if(statement.IsReceipt != oldStatement.IsReceipt) {
				if(command!=""){ command+=",";}
				command+="IsReceipt = "+POut.Bool(statement.IsReceipt)+"";
			}
			if(statement.IsInvoice != oldStatement.IsInvoice) {
				if(command!=""){ command+=",";}
				command+="IsInvoice = "+POut.Bool(statement.IsInvoice)+"";
			}
			if(statement.IsInvoiceCopy != oldStatement.IsInvoiceCopy) {
				if(command!=""){ command+=",";}
				command+="IsInvoiceCopy = "+POut.Bool(statement.IsInvoiceCopy)+"";
			}
			if(statement.EmailSubject != oldStatement.EmailSubject) {
				if(command!=""){ command+=",";}
				command+="EmailSubject = '"+POut.String(statement.EmailSubject)+"'";
			}
			if(statement.EmailBody != oldStatement.EmailBody) {
				if(command!=""){ command+=",";}
				command+="EmailBody = "+DbHelper.ParamChar+"paramEmailBody";
			}
			if(command==""){
				return false;
			}
			if(statement.EmailBody==null) {
				statement.EmailBody="";
			}
			OdSqlParameter paramEmailBody=new OdSqlParameter("paramEmailBody",OdDbType.Text,statement.EmailBody);
			command="UPDATE statement SET "+command
				+" WHERE StatementNum = "+POut.Long(statement.StatementNum);
			Db.NonQ(command,paramEmailBody);
			return true;
		}

		///<summary>Deletes one Statement from the database.</summary>
		public static void Delete(long statementNum){
			string command="DELETE FROM statement "
				+"WHERE StatementNum = "+POut.Long(statementNum);
			Db.NonQ(command);
		}

	}
}