using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Linq;
using System.Drawing;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormPayPlanTreatment : ODForm {
		private ODGrid gridMain;
		private UI.Button butAdd;
		private PayPlan _payPlanCur;
		public List<PayPlanCharge> ListPayPlanCredits;
		private Patient _patCur;
		private UI.Button butOK;
		private UI.Button butCancel;
		private Label label1;
		private TextBox textTotal;
		private GroupBox groupBox1;
		private ODtextBox textNote;
		private Label label4;
		private ValidDate textDate;
		private Label label2;
		private ValidDouble textAmt;
		private Label label3;
		private UI.Button butAddManual;
		private ToolTip toolTip1;
		private Label label6;
		private System.ComponentModel.IContainer components;
		private List<Procedure> _listPayPlanProcs;

		///<summary></summary>
		public FormPayPlanTreatment(PayPlan payPlanCur,Patient patCur){
			//
			// Required for Windows Form Designer support
			//
			_patCur=patCur;
			_payPlanCur=payPlanCur;
			InitializeComponent();
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPayPlanTreatment));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.butAddManual = new OpenDental.UI.Button();
			this.textNote = new OpenDental.ODtextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textDate = new OpenDental.ValidDate();
			this.label2 = new System.Windows.Forms.Label();
			this.textAmt = new OpenDental.ValidDouble();
			this.label3 = new System.Windows.Forms.Label();
			this.textTotal = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.label6 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.butAddManual);
			this.groupBox1.Controls.Add(this.textNote);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.textDate);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.textAmt);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Location = new System.Drawing.Point(339, 42);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(167, 223);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Add Manual Credit";
			// 
			// butAddManual
			// 
			this.butAddManual.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddManual.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAddManual.Autosize = true;
			this.butAddManual.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddManual.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddManual.CornerRadius = 4F;
			this.butAddManual.Image = global::OpenDental.Properties.Resources.Left;
			this.butAddManual.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddManual.Location = new System.Drawing.Point(5, 195);
			this.butAddManual.Name = "butAddManual";
			this.butAddManual.Size = new System.Drawing.Size(92, 23);
			this.butAddManual.TabIndex = 8;
			this.butAddManual.Text = "Add Manual";
			this.butAddManual.UseVisualStyleBackColor = true;
			this.butAddManual.Click += new System.EventHandler(this.butAddManual_Click);
			// 
			// textNote
			// 
			this.textNote.AcceptsTab = true;
			this.textNote.BackColor = System.Drawing.SystemColors.Window;
			this.textNote.DetectLinksEnabled = false;
			this.textNote.DetectUrls = false;
			this.textNote.Location = new System.Drawing.Point(8, 85);
			this.textNote.Name = "textNote";
			this.textNote.QuickPasteType = OpenDentBusiness.QuickPasteType.None;
			this.textNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textNote.Size = new System.Drawing.Size(153, 104);
			this.textNote.SpellCheckIsEnabled = false;
			this.textNote.TabIndex = 5;
			this.textNote.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(6, 65);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(54, 20);
			this.label4.TabIndex = 24;
			this.label4.Text = "Note:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textDate
			// 
			this.textDate.Location = new System.Drawing.Point(65, 19);
			this.textDate.Name = "textDate";
			this.textDate.Size = new System.Drawing.Size(96, 20);
			this.textDate.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(11, 41);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(54, 20);
			this.label2.TabIndex = 20;
			this.label2.Text = "Amt:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textAmt
			// 
			this.textAmt.Location = new System.Drawing.Point(65, 42);
			this.textAmt.MaxVal = 100000000D;
			this.textAmt.MinVal = -100000000D;
			this.textAmt.Name = "textAmt";
			this.textAmt.Size = new System.Drawing.Size(96, 20);
			this.textAmt.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(11, 18);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(54, 20);
			this.label3.TabIndex = 18;
			this.label3.Text = "Date:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTotal
			// 
			this.textTotal.Location = new System.Drawing.Point(256, 418);
			this.textTotal.Name = "textTotal";
			this.textTotal.ReadOnly = true;
			this.textTotal.Size = new System.Drawing.Size(77, 20);
			this.textTotal.TabIndex = 6;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(10, 418);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(245, 20);
			this.label1.TabIndex = 5;
			this.label1.Text = "Treatment Credit Total:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butCancel.Location = new System.Drawing.Point(450, 418);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 23);
			this.butCancel.TabIndex = 4;
			this.butCancel.Text = "Cancel";
			this.butCancel.UseVisualStyleBackColor = true;
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butOK.Location = new System.Drawing.Point(450, 389);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 23);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "&OK";
			this.butOK.UseVisualStyleBackColor = true;
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(336, 8);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(100, 23);
			this.butAdd.TabIndex = 1;
			this.butAdd.Text = "Add Tx Credit";
			this.butAdd.UseVisualStyleBackColor = true;
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// gridMain
			// 
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(10, 8);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(323, 408);
			this.gridMain.TabIndex = 0;
			this.gridMain.Title = "Treatment Credits";
			this.gridMain.TranslationName = null;
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// toolTip1
			// 
			this.toolTip1.AutoPopDelay = 0;
			this.toolTip1.InitialDelay = 10;
			this.toolTip1.ReshowDelay = 100;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(336, 270);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(194, 116);
			this.label6.TabIndex = 15;
			this.label6.Text = resources.GetString("label6.Text");
			// 
			// FormPayPlanTreatment
			// 
			this.ClientSize = new System.Drawing.Size(537, 455);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.textTotal);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.gridMain);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormPayPlanTreatment";
			this.Text = "Payment Plan Treatment Credits";
			this.Load += new System.EventHandler(this.FormPayPlanTreatment_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormPayPlanTreatment_Load(object sender, System.EventArgs e) {
			_listPayPlanProcs=PayPlanCharges.GetPayPlanProcs(_payPlanCur.PayPlanNum);
			FillGrid();
		}

		private void FillGrid() {
			//Construct a list of rows.
			//then add one row for each procedure.
			//show the ProcCode | # of payplancharges attached | $ Sum of payplancharges attached
			//then, add one row for each payplancharge
			//show the Date, the principle, and Note.
			//order by Date, then by ProcNum
			//then add the rows to the grid.
			List<PayPlanEntry> listEntries=new List<PayPlanEntry>();
			foreach(Procedure procCur in _listPayPlanProcs) {
				PayPlanEntry entryCur=new PayPlanEntry() {
					ProcNum=procCur.ProcNum,
					IsCharge=false,
					ProcDate=DateTime.MinValue,
					Amt=procCur.ProcFee,
					DateStr="",
					AmtStr=ProcedureCodes.GetStringProcCode(procCur.CodeNum),
					NoteStr="Total Credits Attached: "+ListPayPlanCredits.Where(x => x.ProcNum==procCur.ProcNum).Sum(x => x.Principal).ToString("c")
				};
				listEntries.Add(entryCur);
			}
			double creditsTotal=0;
			foreach(PayPlanCharge chargeCur in ListPayPlanCredits) {
				PayPlanEntry entryCur=new PayPlanEntry() {
					ProcNum=chargeCur.ProcNum,
					IsCharge=true,
					ProcDate=chargeCur.ChargeDate,
					Amt=chargeCur.Principal,
					DateStr=(chargeCur.ChargeDate.Date==DateTime.MaxValue.Date) ? "N/A" : chargeCur.ChargeDate.ToShortDateString(),
					AmtStr=chargeCur.Principal.ToString("c"),
					NoteStr=chargeCur.Note,
					//directly reference the charge in ListPayPlanCredits.
					PayPlanCharge=chargeCur
				};
				listEntries.Add(entryCur);
				creditsTotal+=chargeCur.Principal;
			}
			listEntries=listEntries.OrderBy(x => x.ProcNum).ThenBy(x => x.IsCharge).ThenBy(x => x.ProcDate).ThenBy(x => x.Amt).ToList();
			gridMain.BeginUpdate();
			gridMain.Rows.Clear();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"Date"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Principal"),60);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Note"),0);
			gridMain.Columns.Add(col);
			ODGridRow row;
			foreach(PayPlanEntry ppeCur in listEntries) {
				row=new ODGridRow();
				row.Cells.Add(ppeCur.DateStr);
				row.Cells.Add(ppeCur.AmtStr);
				row.Cells.Add(ppeCur.NoteStr);
				if(!ppeCur.IsCharge) {
					row.ColorBackG=Color.LightCyan;
				}
				row.Tag=ppeCur;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			textTotal.Text=POut.Double(creditsTotal);
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!((PayPlanEntry)gridMain.Rows[e.Row].Tag).IsCharge) {
				return;
			}
			//this always directly references the PayPlanCharge object in ListPayPlanCredits.
			//This means that any changes made to this object here or in FormATC actually change the object in ListPayPlanCredits.
			//This also makes it easier to remove the charge from ListPayPlanCredits if the user clicked "delete."
			PayPlanCharge selectedCharge=((PayPlanEntry)(gridMain.Rows[e.Row].Tag)).PayPlanCharge;
			FormAddTxCredit FormATC=new FormAddTxCredit(_payPlanCur);
			FormATC.ListPayPlanCreditsCur=ListPayPlanCredits;
			FormATC.ChargeCur=selectedCharge;
			FormATC.IsNew=false;
			FormATC.ShowDialog();
			if(FormATC.DialogResult==DialogResult.OK) {
				//get the procedure that they just clicked
				//if that procedure did not already exist in listPayPlanProcs
				//then add it, otherwise don't.
				if(!_listPayPlanProcs.Exists(x => x.ProcNum==selectedCharge.ProcNum)) {
					_listPayPlanProcs.Add(Procedures.GetOneProc(selectedCharge.ProcNum,false));
				}
			}
			else if(FormATC.IsDeleted) {
				//if they deleted a payplancharge attached to a procedure
				//if it was the last payplancharge attached to the procedure
				//then delete the procedure from listprocs
				ListPayPlanCredits.Remove(selectedCharge);
				if(!ListPayPlanCredits.Exists(x => x.ProcNum==selectedCharge.ProcNum)) {
					_listPayPlanProcs.RemoveAll(x => x.ProcNum==selectedCharge.ProcNum);
				}
			}
			FillGrid();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormAddTxCredit FormATC=new FormAddTxCredit(_payPlanCur);
			FormATC.ListPayPlanCreditsCur=ListPayPlanCredits;
			FormATC.IsNew=true;
			FormATC.ShowDialog();
			PayPlanCharge addCharge=FormATC.ChargeCur;
			if(FormATC.DialogResult==DialogResult.OK) { //Handle all fields that aren't handled by FormAddTxPayment.
				addCharge.ClinicNum=_patCur.ClinicNum;
				addCharge.Guarantor=_payPlanCur.Guarantor;
				addCharge.PatNum=_patCur.PatNum;
				addCharge.PayPlanNum=_payPlanCur.PayPlanNum;
				addCharge.ProvNum=0;//handled when FormPayPlan is closed.
				ListPayPlanCredits.Add(addCharge);
				//get the procedure that they just clicked
				//if that procedure did not already exist in listPayPlanProcs
				//then add it, otherwise don't.
				if(!_listPayPlanProcs.Any(x => x.ProcNum==addCharge.ProcNum)) {
					_listPayPlanProcs.Add(Procedures.GetOneProc(addCharge.ProcNum,false));
				}
			}
			FillGrid();
		}

		private void butOK_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void butAddManual_Click(object sender,EventArgs e) {
			PayPlanCharge addCharge=new PayPlanCharge();
			addCharge.ClinicNum=_patCur.ClinicNum;
			addCharge.Guarantor=_payPlanCur.Guarantor;
			addCharge.PatNum=_patCur.PatNum;
			addCharge.PayPlanNum=_payPlanCur.PayPlanNum;
			addCharge.ProvNum=0;//handled when FormPayPlan is closed.
			addCharge.Principal=PIn.Double(textAmt.Text);
			addCharge.ChargeDate=PIn.Date(textDate.Text);
			addCharge.Note=textNote.Text;
			addCharge.ChargeType=PayPlanChargeType.Credit;
			ListPayPlanCredits.Add(addCharge);
			FillGrid();
			textNote.Clear();
			textDate.Clear();
			textAmt.Clear();
		}

		private class PayPlanEntry {
			//ordering fields
			public long ProcNum;
			public DateTime ProcDate;
			public double Amt;
			public bool IsCharge;
			//visible fields
			public string DateStr="";
			public string AmtStr=""; 
			public string NoteStr="";
			//other fields
			public PayPlanCharge PayPlanCharge;
		}


	}
}