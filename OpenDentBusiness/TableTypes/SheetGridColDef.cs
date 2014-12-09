using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace OpenDentBusiness {
	///<summary>Rows in this table determine which columns will display in the corresponding SheetGrid, column width, and display name of column.</summary>
	[Serializable]
	public class SheetGridColDef:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long SheetGridColDefNum;
		///<summary>FK to sheetgriddef.SheetGridDefNum.  </summary>
		public long SheetGridDefNum;
		///<summary>Internal Name of this column.  Must match the column name of the corresponding DataTable used to fill this column.</summary>
		public string ColName;
		///<summary>Displayed at the top of a given column when filled.  Can be blank.</summary>
		public string DisplayName;
		///<summary>Width of this column within the grid.</summary>
		public int Width;
		///<summary>The order in which the column appears in the grid.  Lowest item order on the left. 0 based and limited automation.</summary>
		public int ItemOrder;
		///<summary>Strings in column will use this alignment.</summary>
		public StringAlignment TextAlign;

		///<summary></summary>
		public SheetGridColDef Copy() {
			return (SheetGridColDef)this.MemberwiseClone();
		}

	}

}