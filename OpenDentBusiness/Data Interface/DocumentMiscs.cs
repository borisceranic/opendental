using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class DocumentMiscs{
		///<summary>Can return null</summary>
		public static DocumentMisc GetUpdateFilesZip() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<DocumentMisc>(MethodBase.GetCurrentMethod());
			}
			//There will be either one or zero
			string command="SELECT * FROM documentmisc WHERE DocMiscType="+POut.Long((int)DocumentMiscType.UpdateFiles);
			return Crud.DocumentMiscCrud.SelectOne(command);
		}

		///<summary>Completely deletes the UpdateFiles row and then inserts a new one and returns the PK of the new record.</summary>
		public static long SetUpdateFilesZip() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod());
			}
			string command="DELETE FROM documentmisc WHERE DocMiscType="+POut.Long((int)DocumentMiscType.UpdateFiles);
			Db.NonQ(command);
			DocumentMisc doc=new DocumentMisc();
			doc.DateCreated=DateTime.Today;
			doc.DocMiscType=DocumentMiscType.UpdateFiles;
			doc.FileName="UpdateFiles.zip";
			doc.RawBase64="";
			return Crud.DocumentMiscCrud.Insert(doc);
		}

		///<summary>Appends the passed in rawBase64 string to the RawBase64 column in the db for the corresponding document.</summary>
		public static void AppendRawBase64ForDoc(string rawBase64,long docMiscNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),rawBase64,docMiscNum);
				return;
			}
			string command="UPDATE documentmisc SET RawBase64=CONCAT(RawBase64,"+DbHelper.ParamChar+"paramRawBase64) WHERE DocMiscNum="+POut.Long(docMiscNum);
			OdSqlParameter paramRawBase64=new OdSqlParameter("paramRawBase64",OdDbType.Text,rawBase64);
			Db.NonQ(command,paramRawBase64);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<DocumentMisc> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<DocumentMisc>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM documentmisc WHERE PatNum = "+POut.Long(patNum);
			return Crud.DocumentMiscCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(DocumentMisc documentMisc){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				documentMisc.DocMiscNum=Meth.GetLong(MethodBase.GetCurrentMethod(),documentMisc);
				return documentMisc.DocMiscNum;
			}
			return Crud.DocumentMiscCrud.Insert(documentMisc);
		}

		///<summary></summary>
		public static void Update(DocumentMisc documentMisc){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),documentMisc);
				return;
			}
			Crud.DocumentMiscCrud.Update(documentMisc);
		}

		///<summary></summary>
		public static void Delete(long docMiscNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),docMiscNum);
				return;
			}
			string command= "DELETE FROM documentmisc WHERE DocMiscNum = "+POut.Long(docMiscNum);
			Db.NonQ(command);
		}
		*/



	}
}