//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class EtransMessageTextCrud {
		///<summary>Gets one EtransMessageText object from the database using the primary key.  Returns null if not found.</summary>
		public static EtransMessageText SelectOne(long etransMessageTextNum){
			string command="SELECT * FROM etransmessagetext "
				+"WHERE EtransMessageTextNum = "+POut.Long(etransMessageTextNum);
			List<EtransMessageText> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one EtransMessageText object from the database using a query.</summary>
		public static EtransMessageText SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EtransMessageText> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of EtransMessageText objects from the database using a query.</summary>
		public static List<EtransMessageText> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EtransMessageText> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<EtransMessageText> TableToList(DataTable table){
			List<EtransMessageText> retVal=new List<EtransMessageText>();
			EtransMessageText etransMessageText;
			for(int i=0;i<table.Rows.Count;i++) {
				etransMessageText=new EtransMessageText();
				etransMessageText.EtransMessageTextNum= PIn.Long  (table.Rows[i]["EtransMessageTextNum"].ToString());
				etransMessageText.MessageText         = PIn.String(table.Rows[i]["MessageText"].ToString());
				retVal.Add(etransMessageText);
			}
			return retVal;
		}

		///<summary>Inserts one EtransMessageText into the database.  Returns the new priKey.</summary>
		public static long Insert(EtransMessageText etransMessageText){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				etransMessageText.EtransMessageTextNum=DbHelper.GetNextOracleKey("etransmessagetext","EtransMessageTextNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(etransMessageText,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							etransMessageText.EtransMessageTextNum++;
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
				return Insert(etransMessageText,false);
			}
		}

		///<summary>Inserts one EtransMessageText into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(EtransMessageText etransMessageText,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				etransMessageText.EtransMessageTextNum=ReplicationServers.GetKey("etransmessagetext","EtransMessageTextNum");
			}
			string command="INSERT INTO etransmessagetext (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="EtransMessageTextNum,";
			}
			command+="MessageText) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(etransMessageText.EtransMessageTextNum)+",";
			}
			command+=
				     DbHelper.ParamChar+"paramMessageText)";
			if(etransMessageText.MessageText==null) {
				etransMessageText.MessageText="";
			}
			OdSqlParameter paramMessageText=new OdSqlParameter("paramMessageText",OdDbType.Text,etransMessageText.MessageText);
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramMessageText);
			}
			else {
				etransMessageText.EtransMessageTextNum=Db.NonQ(command,true,paramMessageText);
			}
			return etransMessageText.EtransMessageTextNum;
		}

		///<summary>Updates one EtransMessageText in the database.</summary>
		public static void Update(EtransMessageText etransMessageText){
			string command="UPDATE etransmessagetext SET "
				+"MessageText         =  "+DbHelper.ParamChar+"paramMessageText "
				+"WHERE EtransMessageTextNum = "+POut.Long(etransMessageText.EtransMessageTextNum);
			if(etransMessageText.MessageText==null) {
				etransMessageText.MessageText="";
			}
			OdSqlParameter paramMessageText=new OdSqlParameter("paramMessageText",OdDbType.Text,etransMessageText.MessageText);
			Db.NonQ(command,paramMessageText);
		}

		///<summary>Updates one EtransMessageText in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(EtransMessageText etransMessageText,EtransMessageText oldEtransMessageText){
			string command="";
			if(etransMessageText.MessageText != oldEtransMessageText.MessageText) {
				if(command!=""){ command+=",";}
				command+="MessageText = "+DbHelper.ParamChar+"paramMessageText";
			}
			if(command==""){
				return false;
			}
			if(etransMessageText.MessageText==null) {
				etransMessageText.MessageText="";
			}
			OdSqlParameter paramMessageText=new OdSqlParameter("paramMessageText",OdDbType.Text,etransMessageText.MessageText);
			command="UPDATE etransmessagetext SET "+command
				+" WHERE EtransMessageTextNum = "+POut.Long(etransMessageText.EtransMessageTextNum);
			Db.NonQ(command,paramMessageText);
			return true;
		}

		///<summary>Deletes one EtransMessageText from the database.</summary>
		public static void Delete(long etransMessageTextNum){
			string command="DELETE FROM etransmessagetext "
				+"WHERE EtransMessageTextNum = "+POut.Long(etransMessageTextNum);
			Db.NonQ(command);
		}

	}
}