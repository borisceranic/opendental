using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace OpenDentBusiness {
	///<summary>Definition of a grid that can be placed on sheets.  There is NOT a corresponfing SheetGrid table, only Defs.</summary>
	[Serializable]
	public class SheetGridDef:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long SheetGridDefNum;
		///<summary>Enum:SheetGridType Limits the number and type of columns that can be added to this SheetGridDef.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.EnumAsString)]
		public SheetGridType GridType;
		///<summary>Displays in OD, not when printing.</summary>
		public string Descritpion;
		///<summary>Printed at top of grid.</summary>
		public string Title;
		///<summary>ALWAYS sorted list of grid columns.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public List<SheetGridColDef> Columns;

		
		///<summary></summary>
		public SheetGridDef Copy() {
			return (SheetGridDef)this.MemberwiseClone();
		}

	}

	///<summary>Defines the grid type which then restricts the available columns that can be added.  <para>Should be a CrudSpecialColType.EnumAsString.</para></summary>
	public enum SheetGridType {
		///<summary>Account summary as it displays on the statement grid.</summary>
		StatementMain,
		///<summary></summary>
		StatementAgeing,
		///<summary>Should always be a single row grid that contains various account totals.  Examples: aging balances, account total, amount due, Amount enclosed (blank column)</summary>
		StatementEnclosed,
		///<summary>Standard payment plan grid as seen on statements.</summary>
		StatementPayPlan
	}

	///<summary>Contains data used to determine which content should be loaded into a sheetgrid and how to draw it.</summary>
	public class SheetArgs {
		//For Statement Grids.
		public long PatNum;
		public DateTime FromDate;
		public DateTime ToDate;
		public bool Intermingled;
		public bool SinglePatient;
		public long StatementNum;
		public bool IsReceipt;
		public bool IsInvoice;
		public bool ShowPaymentOptions;
		public bool ShowProcBreakdown;
		public bool ShowPayNotes;
		public bool ShowAdjNotes;
		public string BoldNote;
		public string NormNote;

		///<summary>Used to construct SheetArgs object for use with printing statement grid. must pass in values for all parameters.</summary>
		public void SetForStatement(long patNum,DateTime fromDate,DateTime toDate,bool intermingled,bool singlePatient,long statementNum,bool isInvoice,bool isReceipt,bool showProcBreakdown,bool showPayNotes,bool showAdjNotes,string boldNote,string normNote, bool showPaymentOptions) {
			this.PatNum=patNum;
			this.FromDate=fromDate;
			this.ToDate=toDate;
			this.Intermingled=intermingled;
			this.SinglePatient=singlePatient;
			this.StatementNum=statementNum;
			this.IsInvoice=isInvoice;
			this.IsReceipt=isReceipt;
			this.ShowProcBreakdown=showProcBreakdown;
			this.ShowPayNotes=showPayNotes;
			this.ShowAdjNotes=showAdjNotes;
			this.BoldNote=boldNote;
			this.NormNote=normNote;
			this.ShowPaymentOptions=showPaymentOptions;
			//return retVal;
		}

		public SheetArgs Copy() {
			return (SheetArgs)this.MemberwiseClone();
		}
	}
}