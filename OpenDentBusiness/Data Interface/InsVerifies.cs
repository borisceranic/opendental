using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class InsVerifies {

		///<summary>Gets one InsVerify from the db.</summary>
		public static InsVerify GetOne(long insVerifyNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<InsVerify>(MethodBase.GetCurrentMethod(),insVerifyNum);
			}
			return Crud.InsVerifyCrud.SelectOne(insVerifyNum);
		}
		
		///<summary>Gets one InsVerify from the db that has the given fkey and verify type.</summary>
		public static InsVerify GetOneByFKey(long fkey,VerifyTypes verifyType) {
			//TODO Remoting Role
			string command="SELECT * FROM insverify WHERE FKey="+POut.Long(fkey)+" AND VerifyType="+POut.Int((int)verifyType)+"";
			return Crud.InsVerifyCrud.SelectOne(command);
		}
		
		///<summary>Gets one InsVerifyNum from the db that has the given fkey and verify type.</summary>
		public static long GetInsVerifyNumByFKey(long fkey,VerifyTypes verifyType) {
			string command="SELECT * FROM insverify WHERE FKey="+POut.Long(fkey)+" AND VerifyType="+POut.Int((int)verifyType)+"";
			InsVerify insVerify=Crud.InsVerifyCrud.SelectOne(command);
			if(insVerify==null) {
				return 0;
			}
			return insVerify.InsVerifyNum;
		}

		///<summary></summary>
		public static long Insert(InsVerify insVerify) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				insVerify.InsVerifyNum=Meth.GetLong(MethodBase.GetCurrentMethod(),insVerify);
				return insVerify.InsVerifyNum;
			}
			return Crud.InsVerifyCrud.Insert(insVerify);
		}

		///<summary></summary>
		public static void Update(InsVerify insVerify) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),insVerify);
				return;
			}
			Crud.InsVerifyCrud.Update(insVerify);
		}

		///<summary></summary>
		public static void Delete(long insVerifyNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),insVerifyNum);
				return;
			}
			Crud.InsVerifyCrud.Delete(insVerifyNum);
		}
		
		///<summary>Inserts a default InsVerify into the database based on the passed in patplan.  Used when inserting a new patplan.
		///Returns the primary key of the new InsVerify.</summary>
		public static long InsertForPatPlanNum(long patPlanNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),patPlanNum);
			}
			InsVerify insVerify=new InsVerify();
			insVerify.VerifyType=VerifyTypes.PatientEnrollment;
			insVerify.FKey=patPlanNum;
			return Crud.InsVerifyCrud.Insert(insVerify);
		}
		
		///<summary>Inserts a default InsVerify into the database based on the passed in insplan.  Used when inserting a new insplan.
		///Returns the primary key of the new InsVerify.</summary>
		public static long InsertForPlanNum(long planNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),planNum);
			}
			InsVerify insVerify=new InsVerify();
			insVerify.VerifyType=VerifyTypes.InsuranceBenefit;
			insVerify.FKey=planNum;
			return Crud.InsVerifyCrud.Insert(insVerify);
		}
		
		///<summary>Deletes an InsVerify with the passed in FKey and VerifyType.</summary>
		public static void DeleteByFKey(long fkey,VerifyTypes verifyType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetLong(MethodBase.GetCurrentMethod(),fkey,verifyType);
				return;
			}
			long insVerifyNum=GetInsVerifyNumByFKey(fkey,verifyType);
			Crud.InsVerifyCrud.Delete(insVerifyNum);//Will do nothing if insVerifyNum was 0.
		}

		public static List<InsVerify> GetAll() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<InsVerify>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM insverify";
			return Crud.InsVerifyCrud.SelectMany(command);
        }

		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add RefreshCache and FillCache 
		//to the Cache.cs file with all the other Cache types.

		///<summary>A list of all InsVerifies.</summary>
		private static List<InsVerify> listt;

		///<summary>A list of all InsVerifies.</summary>
		public static List<InsVerify> Listt{
			get {
				if(listt==null) {
					RefreshCache();
				}
				return listt;
			}
			set {
				listt=value;
			}
		}

		///<summary></summary>
		public static DataTable RefreshCache(){
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM insverify ORDER BY ItemOrder";//stub query probably needs to be changed
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="InsVerify";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.InsVerifyCrud.TableToList(table);
		}
		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<InsVerify> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<InsVerify>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM insverify WHERE PatNum = "+POut.Long(patNum);
			return Crud.InsVerifyCrud.SelectMany(command);
		}
		*/
	}
}