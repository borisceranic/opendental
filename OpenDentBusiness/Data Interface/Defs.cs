using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness {
	public class Defs {
		///<summary>If using remoting, then the calling program is responsible for filling the arrays on the client since the automated part only happens on the server.  So there are TWO sets of arrays in a server situation, but only one in a small office that connects directly to the database.</summary>
		public static DataTable RefreshCache(){
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM definition ORDER BY Category,ItemOrder";
			DataConnection dcon=new DataConnection();
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="Def";
			FillCache(table);
			return table;
		}

		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			List<Def> list=Crud.DefCrud.TableToList(table);
			Def[][] arrayLong=new Def[Enum.GetValues(typeof(DefCat)).Length][];
			for(int j=0;j<Enum.GetValues(typeof(DefCat)).Length;j++) {
				arrayLong[j]=GetForCategory(j,true,list);
			}
			Def[][] arrayShort=new Def[Enum.GetValues(typeof(DefCat)).Length][];
			for(int j=0;j<Enum.GetValues(typeof(DefCat)).Length;j++) {
				arrayShort[j]=GetForCategory(j,false,list);
			}
			DefC.Long=arrayLong;
			DefC.Short=arrayShort;
		}

		///<summary>Used by the refresh method above.</summary>
		private static Def[] GetForCategory(int catIndex,bool includeHidden,List<Def> list) {
			//No need to check RemotingRole; no call to db.
			List<Def> retVal=new List<Def>();
			for(int i=0;i<list.Count;i++) {
				if((int)list[i].Category!=catIndex){
					continue;
				}
				if(list[i].IsHidden && !includeHidden){
					continue;
				}
				retVal.Add(list[i]);
			}
			return retVal.ToArray();
		}

		///<summary>Only used in FormDefinitions</summary>
		public static Def[] GetCatList(int myCat){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Def[]>(MethodBase.GetCurrentMethod(),myCat);
			}
			string command=
				"SELECT * from definition"
				+" WHERE category = '"+myCat+"'"
				+" ORDER BY ItemOrder";
			return Crud.DefCrud.SelectMany(command).ToArray();
		}

		///<summary></summary>
		public static void Update(Def def) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),def);
				return;
			}
			Crud.DefCrud.Update(def);
		}

		///<summary></summary>
		public static long Insert(Def def) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				def.DefNum=Meth.GetLong(MethodBase.GetCurrentMethod(),def);
				return def.DefNum;
			}
			return Crud.DefCrud.Insert(def);
		}

		///<summary>CAUTION.  This does not perform all validations.  It only properly validates for three def types right now; SupplyCats, ClaimCustomTracking, and InsurancePaymentType.</summary>
		public static void Delete(Def def) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),def);
				return;
			}
			if(def.Category!=DefCat.SupplyCats 
				&& def.Category!=DefCat.ClaimCustomTracking
				&& def.Category!=DefCat.InsurancePaymentType) 
			{
				throw new ApplicationException("NOT Allowed to delete this type of def.");
			}
			string command="";
			if(def.Category==DefCat.SupplyCats) {
				command="SELECT COUNT(*) FROM supply WHERE Category="+POut.Long(def.DefNum);
			}
			else if(def.Category==DefCat.ClaimCustomTracking){
				command="SELECT COUNT(*) FROM claim WHERE CustomTracking="+POut.Long(def.DefNum);
			}
			else if(def.Category==DefCat.InsurancePaymentType) {
				command="SELECT COUNT(*) FROM claimpayment WHERE PayType="+POut.Long(def.DefNum);
			}
			if(Db.GetCount(command)!="0"){
				throw new ApplicationException(Lans.g("Defs","Def is in use.  Not allowed to delete."));
			}
			command="DELETE FROM definition WHERE DefNum="+POut.Long(def.DefNum);
			Db.NonQ(command);
			command="UPDATE definition SET ItemOrder=ItemOrder-1 "
				+"WHERE Category="+POut.Long((int)def.Category)
				+" AND ItemOrder > "+POut.Long(def.ItemOrder);
			Db.NonQ(command);
		}

		///<summary></summary>
		public static void HideDef(Def def) {
			//No need to check RemotingRole; no call to db.
			def.IsHidden=true;
			Defs.Update(def);
		}

		///<summary></summary>
		public static void SetOrder(int mySelNum,int myItemOrder,Def[] list) {
			//No need to check RemotingRole; no call to db.
			Def def=list[mySelNum];
			def.ItemOrder=myItemOrder;
			//Cur=temp;
			Defs.Update(def);
		}

		///<summary>Returns true if this category has definitions that can be hidden.</summary>
		public static bool IsHidable(DefCat category) {
			if(category==DefCat.AdjTypes
				|| category==DefCat.ApptConfirmed
				|| category==DefCat.ApptProcsQuickAdd
				|| category==DefCat.BillingTypes
				|| category==DefCat.BlockoutTypes
				|| category==DefCat.CommLogTypes
				|| category==DefCat.ContactCategories
				|| category==DefCat.Diagnosis
				|| category==DefCat.ImageCats
				|| category==DefCat.LetterMergeCats
				|| category==DefCat.PaymentTypes
				|| category==DefCat.PaySplitUnearnedType
				|| category==DefCat.ProcButtonCats
				|| category==DefCat.ProcCodeCats
				|| category==DefCat.Prognosis
				|| category==DefCat.ProviderSpecialties
				|| category==DefCat.RecallUnschedStatus
				|| category==DefCat.TaskPriorities
				|| category==DefCat.TxPriorities) 
			{
				return true;
			}
			return false;
		}

		///<summary>Returns true if this definition is in use within the program. Consider enhancing this method if you add a definition category.
		///Does not check patient billing type or provider specialty since those are handled in their S-class.</summary>
		public static bool IsDefinitionInUse(Def def) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),def);
			}
			string command;
			switch(def.Category) {
				case DefCat.AdjTypes:
					command="SELECT COUNT(*) FROM adjustment WHERE AdjType="+POut.Long(def.DefNum);
					if(Db.GetCount(command)!="0") {
						return true;
					}
					if(def.DefNum==PrefC.GetLong(PrefName.BrokenAppointmentAdjustmentType)) {
						return true;
					}
					break;
				case DefCat.ApptConfirmed:
					command="SELECT COUNT(*) FROM appointment WHERE Confirmed="+POut.Long(def.DefNum);
					if(Db.GetCount(command)!="0") {
						return true;
					}
					if(def.DefNum==PrefC.GetLong(PrefName.AppointmentTimeArrivedTrigger)
						|| def.DefNum==PrefC.GetLong(PrefName.AppointmentTimeSeatedTrigger)
						|| def.DefNum==PrefC.GetLong(PrefName.AppointmentTimeDismissedTrigger)) {
						return true;
					}
					break;
				case DefCat.ContactCategories:
					command="SELECT COUNT(*) FROM contact WHERE Category="+POut.Long(def.DefNum);
					if(Db.GetCount(command)!="0") {
						return true;
					}
					break;
				case DefCat.Diagnosis:
					command="SELECT COUNT(*) FROM procedurelog WHERE Dx="+POut.Long(def.DefNum);
					if(Db.GetCount(command)!="0") {
						return true;
					}
					break;
				case DefCat.ImageCats:
					command="SELECT COUNT(*) FROM document WHERE DocCategory="+POut.Long(def.DefNum);
					if(Db.GetCount(command)!="0") {
						return true;
					}
					command="SELECT COUNT(*) FROM sheetfielddef WHERE FieldType="+POut.Int((int)SheetFieldType.PatImage)+" AND FieldName="+POut.Long(def.DefNum);
					if(Db.GetCount(command)!="0") {
						return true;
					}
					break;
				case DefCat.PaymentTypes:
					command="SELECT COUNT(*) FROM payment WHERE PayType="+POut.Long(def.DefNum);
					if(Db.GetCount(command)!="0") {
						return true;
					}
					break;
				case DefCat.PaySplitUnearnedType:
					command="SELECT COUNT(*) FROM paysplit WHERE UnearnedType="+POut.Long(def.DefNum);
					if(Db.GetCount(command)!="0") {
						return true;
					}
					break;
				case DefCat.Prognosis:
					command="SELECT COUNT(*) FROM procedurelog WHERE Prognosis="+POut.Long(def.DefNum);
					if(Db.GetCount(command)!="0") {
						return true;
					}
					break;
				case DefCat.RecallUnschedStatus:
					command="SELECT COUNT(*) FROM appointment WHERE UnschedStatus="+POut.Long(def.DefNum);
					if(Db.GetCount(command)!="0") {
						return true;
					}
					command="SELECT COUNT(*) FROM recall WHERE RecallStatus="+POut.Long(def.DefNum);
					if(Db.GetCount(command)!="0") {
						return true;
					}
					break;
				case DefCat.TaskPriorities:
					command="SELECT COUNT(*) FROM task WHERE PriorityDefNum="+POut.Long(def.DefNum);
					if(Db.GetCount(command)!="0") {
						return true;
					}
					break;
				case DefCat.TxPriorities:
					command="SELECT COUNT(*) FROM procedurelog WHERE Priority="+POut.Long(def.DefNum);
					if(Db.GetCount(command)!="0") {
						return true;
					}
					break;
			}
			return false;
		}

	}
}
