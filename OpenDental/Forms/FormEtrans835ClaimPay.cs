using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	///<summary></summary>
	public class FormEtrans835ClaimPay : System.Windows.Forms.Form {
		private OpenDental.ValidDouble textWriteOff;
		private System.Windows.Forms.TextBox textInsPayAllowed;
		private OpenDental.ValidDouble textInsPayAmt;
		private System.Windows.Forms.TextBox textClaimFee;
		private OpenDental.ValidDouble textDedApplied;
		private System.Windows.Forms.Label label1;
		///<summary>Required designer variable.</summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butDeductible;
		private OpenDental.UI.Button butWriteOff;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private OpenDental.UI.ODGrid gridMain;
		private ODGrid gridClaimAdjustments;
		private ODGrid gridProcedureBreakdown;
		private UI.Button butViewEobDetails;
		private List<Procedure> _listProcs;
		private Patient _patCur;
		private Family _famCur;
		private List<InsPlan> _listPlans;
		private List<PatPlan> _listPatPlans;
		private List<InsSub> _listInsSubs;
		private Hx835_Claim _claimPaid;
		private decimal _claimAdjAmtSum;
		private decimal _procAdjAmtSum;
		///<summary>The claim procs shown in the grid.  These procs are saved to/from the grid, but changes are not saved to the database unless the OK button is pressed or an individual claim proc is double-clicked for editing.</summary>
		private List <ClaimProc> _listClaimProcsForClaim;

		///<summary></summary>
		public FormEtrans835ClaimPay(Hx835_Claim claimPaid,Patient patCur,Family famCur,List<InsPlan> planList,List<PatPlan> patPlanList,List<InsSub> subList) {
			InitializeComponent();
			_claimPaid=claimPaid;
			_famCur=famCur;
			_patCur=patCur;
			_listPlans=planList;
			_listInsSubs=subList;
			_listPatPlans=patPlanList;
			Lan.F(this);
		}

		///<summary>Clean up any resources being used.</summary>
		protected override void Dispose(bool disposing) {
			if(disposing) {
				if(components!=null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEtrans835ClaimPay));
			this.textInsPayAllowed = new System.Windows.Forms.TextBox();
			this.textClaimFee = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butWriteOff = new OpenDental.UI.Button();
			this.butDeductible = new OpenDental.UI.Button();
			this.textWriteOff = new OpenDental.ValidDouble();
			this.textInsPayAmt = new OpenDental.ValidDouble();
			this.textDedApplied = new OpenDental.ValidDouble();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.gridClaimAdjustments = new OpenDental.UI.ODGrid();
			this.gridProcedureBreakdown = new OpenDental.UI.ODGrid();
			this.butViewEobDetails = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// textInsPayAllowed
			// 
			this.textInsPayAllowed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textInsPayAllowed.Location = new System.Drawing.Point(455, 555);
			this.textInsPayAllowed.Name = "textInsPayAllowed";
			this.textInsPayAllowed.ReadOnly = true;
			this.textInsPayAllowed.Size = new System.Drawing.Size(50, 20);
			this.textInsPayAllowed.TabIndex = 116;
			this.textInsPayAllowed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textClaimFee
			// 
			this.textClaimFee.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textClaimFee.Location = new System.Drawing.Point(339, 555);
			this.textClaimFee.Name = "textClaimFee";
			this.textClaimFee.ReadOnly = true;
			this.textClaimFee.Size = new System.Drawing.Size(62, 20);
			this.textClaimFee.TabIndex = 118;
			this.textClaimFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(251, 558);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(84, 16);
			this.label1.TabIndex = 117;
			this.label1.Text = "Totals";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.Location = new System.Drawing.Point(346, 605);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(348, 39);
			this.label2.TabIndex = 122;
			this.label2.Text = "Before you click OK, the Deductible and the Ins Pay amounts should exactly match " +
    "the insurance EOB.";
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.Location = new System.Drawing.Point(20, 565);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(116, 34);
			this.label3.TabIndex = 123;
			this.label3.Text = "Assign to selected procedure:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label4.Location = new System.Drawing.Point(164, 571);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(108, 29);
			this.label4.TabIndex = 124;
			this.label4.Text = "On all unpaid amounts:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(9, 292);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridMain.Size = new System.Drawing.Size(956, 257);
			this.gridMain.TabIndex = 125;
			this.gridMain.Title = "Procedures";
			this.gridMain.TranslationName = "TableClaimProc";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellTextChanged += new System.EventHandler(this.gridMain_CellTextChanged);
			// 
			// butWriteOff
			// 
			this.butWriteOff.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butWriteOff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butWriteOff.Autosize = true;
			this.butWriteOff.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butWriteOff.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butWriteOff.CornerRadius = 4F;
			this.butWriteOff.Location = new System.Drawing.Point(163, 606);
			this.butWriteOff.Name = "butWriteOff";
			this.butWriteOff.Size = new System.Drawing.Size(90, 25);
			this.butWriteOff.TabIndex = 121;
			this.butWriteOff.Text = "&Write Off";
			this.butWriteOff.Click += new System.EventHandler(this.butWriteOff_Click);
			// 
			// butDeductible
			// 
			this.butDeductible.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDeductible.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDeductible.Autosize = true;
			this.butDeductible.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDeductible.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDeductible.CornerRadius = 4F;
			this.butDeductible.Location = new System.Drawing.Point(23, 606);
			this.butDeductible.Name = "butDeductible";
			this.butDeductible.Size = new System.Drawing.Size(92, 25);
			this.butDeductible.TabIndex = 120;
			this.butDeductible.Text = "&Deductible";
			this.butDeductible.Click += new System.EventHandler(this.butDeductible_Click);
			// 
			// textWriteOff
			// 
			this.textWriteOff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textWriteOff.Location = new System.Drawing.Point(559, 555);
			this.textWriteOff.MaxVal = 100000000D;
			this.textWriteOff.MinVal = -100000000D;
			this.textWriteOff.Name = "textWriteOff";
			this.textWriteOff.ReadOnly = true;
			this.textWriteOff.Size = new System.Drawing.Size(54, 20);
			this.textWriteOff.TabIndex = 119;
			this.textWriteOff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textInsPayAmt
			// 
			this.textInsPayAmt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textInsPayAmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textInsPayAmt.Location = new System.Drawing.Point(505, 555);
			this.textInsPayAmt.MaxVal = 100000000D;
			this.textInsPayAmt.MinVal = -100000000D;
			this.textInsPayAmt.Name = "textInsPayAmt";
			this.textInsPayAmt.ReadOnly = true;
			this.textInsPayAmt.Size = new System.Drawing.Size(54, 20);
			this.textInsPayAmt.TabIndex = 115;
			this.textInsPayAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textDedApplied
			// 
			this.textDedApplied.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textDedApplied.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textDedApplied.Location = new System.Drawing.Point(401, 555);
			this.textDedApplied.MaxVal = 100000000D;
			this.textDedApplied.MinVal = -100000000D;
			this.textDedApplied.Name = "textDedApplied";
			this.textDedApplied.ReadOnly = true;
			this.textDedApplied.Size = new System.Drawing.Size(54, 20);
			this.textDedApplied.TabIndex = 114;
			this.textDedApplied.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(890, 606);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 25);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
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
			this.butOK.Location = new System.Drawing.Point(809, 606);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 25);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// gridClaimAdjustments
			// 
			this.gridClaimAdjustments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridClaimAdjustments.HScrollVisible = false;
			this.gridClaimAdjustments.Location = new System.Drawing.Point(9, 12);
			this.gridClaimAdjustments.Name = "gridClaimAdjustments";
			this.gridClaimAdjustments.ScrollValue = 0;
			this.gridClaimAdjustments.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridClaimAdjustments.Size = new System.Drawing.Size(956, 100);
			this.gridClaimAdjustments.TabIndex = 200;
			this.gridClaimAdjustments.TabStop = false;
			this.gridClaimAdjustments.Title = "EOB Claim Adjustments";
			this.gridClaimAdjustments.TranslationName = "FormEtrans835Edit";
			// 
			// gridProcedureBreakdown
			// 
			this.gridProcedureBreakdown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridProcedureBreakdown.HScrollVisible = false;
			this.gridProcedureBreakdown.Location = new System.Drawing.Point(9, 118);
			this.gridProcedureBreakdown.Name = "gridProcedureBreakdown";
			this.gridProcedureBreakdown.ScrollValue = 0;
			this.gridProcedureBreakdown.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridProcedureBreakdown.Size = new System.Drawing.Size(956, 168);
			this.gridProcedureBreakdown.TabIndex = 199;
			this.gridProcedureBreakdown.TabStop = false;
			this.gridProcedureBreakdown.Title = "EOB Procedure Breakdown";
			this.gridProcedureBreakdown.TranslationName = "FormEtrans835Edit";
			// 
			// butViewEobDetails
			// 
			this.butViewEobDetails.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butViewEobDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butViewEobDetails.Autosize = true;
			this.butViewEobDetails.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butViewEobDetails.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butViewEobDetails.CornerRadius = 4F;
			this.butViewEobDetails.Location = new System.Drawing.Point(703, 606);
			this.butViewEobDetails.Name = "butViewEobDetails";
			this.butViewEobDetails.Size = new System.Drawing.Size(100, 25);
			this.butViewEobDetails.TabIndex = 201;
			this.butViewEobDetails.Text = "EOB Details";
			this.butViewEobDetails.Click += new System.EventHandler(this.butViewEobDetails_Click);
			// 
			// FormEtrans835ClaimPay
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(974, 643);
			this.Controls.Add(this.butViewEobDetails);
			this.Controls.Add(this.gridClaimAdjustments);
			this.Controls.Add(this.gridProcedureBreakdown);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butWriteOff);
			this.Controls.Add(this.butDeductible);
			this.Controls.Add(this.textWriteOff);
			this.Controls.Add(this.textInsPayAllowed);
			this.Controls.Add(this.textInsPayAmt);
			this.Controls.Add(this.textClaimFee);
			this.Controls.Add(this.textDedApplied);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormEtrans835ClaimPay";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Enter ERA Payment";
			this.Activated += new System.EventHandler(this.FormEtrans835ClaimPay_Activated);
			this.Load += new System.EventHandler(this.FormEtrans835ClaimPay_Load);
			this.Shown += new System.EventHandler(this.FormEtrans835ClaimPay_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormEtrans835ClaimPay_Load(object sender, System.EventArgs e) {
			_listProcs=Procedures.Refresh(_patCur.PatNum);
			FillGridProcedures();
		}

		private void FormEtrans835ClaimPay_Shown(object sender,EventArgs e) {
			InsPlan plan=InsPlans.GetPlan(_listClaimProcsForClaim[0].PlanNum,_listPlans);
			if(plan.AllowedFeeSched!=0){//allowed fee sched
				gridMain.SetSelected(new Point(7,0));//Allowed, first row.
			}
			else{
				gridMain.SetSelected(new Point(8,0));//InsPay, first row.
			}
		}

		private void FillGridClaimAdjustments() {
			if(_claimPaid.ListClaimAdjustments.Count==0) {
				gridClaimAdjustments.Title="Claim Adjustments (None Reported)";
			}
			else {
				gridClaimAdjustments.Title="Claim Adjustments";
			}
			gridClaimAdjustments.BeginUpdate();
			gridClaimAdjustments.Columns.Clear();
			const int colWidthDescription=200;
			const int colWidthAdjAmt=80;
			int colWidthVariable=gridClaimAdjustments.Width-10-colWidthDescription-colWidthAdjAmt;
			gridClaimAdjustments.Columns.Add(new UI.ODGridColumn("Description",colWidthDescription,HorizontalAlignment.Left));
			gridClaimAdjustments.Columns.Add(new UI.ODGridColumn("Reason",colWidthVariable,HorizontalAlignment.Left));
			gridClaimAdjustments.Columns.Add(new UI.ODGridColumn("AdjAmt",colWidthAdjAmt,HorizontalAlignment.Right));
			gridClaimAdjustments.Rows.Clear();
			_claimAdjAmtSum=0;
			for(int i=0;i<_claimPaid.ListClaimAdjustments.Count;i++) {
				Hx835_Adj adj=_claimPaid.ListClaimAdjustments[i];
				ODGridRow row=new ODGridRow();
				row.Tag=adj;
				row.Cells.Add(new ODGridCell(adj.AdjustDescript));//Description
				row.Cells.Add(new ODGridCell(adj.ReasonDescript));//Reason
				row.Cells.Add(new ODGridCell(adj.AdjAmt.ToString("f2")));//AdjAmt
				_claimAdjAmtSum+=_claimPaid.ListClaimAdjustments[i].AdjAmt;
				gridClaimAdjustments.Rows.Add(row);
			}
			gridClaimAdjustments.EndUpdate();
		}

		private void FillGridProcedureBreakdown() {
			if(_claimPaid.ListProcs.Count==0) {
				gridProcedureBreakdown.Title="Procedure Breakdown (None Reported)";
			}
			else {
				gridProcedureBreakdown.Title="Procedure Breakdown";
			}
			gridProcedureBreakdown.BeginUpdate();
			const int colWidthProcNum=80;
			const int colWidthProcCode=80;
			const int colWidthProcFee=80;
			const int colWidthAdjAmt=80;
			const int colWidthInsPaid=80;
			int colWidthVariable=gridProcedureBreakdown.Width-10-colWidthProcNum-colWidthProcCode-colWidthProcFee-4*colWidthAdjAmt-colWidthInsPaid;
			gridProcedureBreakdown.Columns.Clear();
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("ProcNum",colWidthProcNum,HorizontalAlignment.Left));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("ProcCode",colWidthProcCode,HorizontalAlignment.Center));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("ProcDescript",colWidthVariable,HorizontalAlignment.Left));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("ProcFee",colWidthProcFee,HorizontalAlignment.Right));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("PatPortion",colWidthAdjAmt,HorizontalAlignment.Right));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("Contractual",colWidthAdjAmt,HorizontalAlignment.Right));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("PayorReduct",colWidthAdjAmt,HorizontalAlignment.Right));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("OtherAdjust",colWidthAdjAmt,HorizontalAlignment.Right));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("InsPaid",colWidthInsPaid,HorizontalAlignment.Right));
			gridProcedureBreakdown.Rows.Clear();
			_procAdjAmtSum=0;
			for(int i=0;i<_claimPaid.ListProcs.Count;i++) {
				Hx835_Proc proc=_claimPaid.ListProcs[i];
				ODGridRow row=new ODGridRow();
				row.Tag=proc;
				if(proc.ProcNum==0) {
					row.Cells.Add(new ODGridCell(""));//ProcNum
				}
				else {
					row.Cells.Add(new ODGridCell(proc.ProcNum.ToString()));//ProcNum
				}
				row.Cells.Add(new ODGridCell(proc.ProcCodeAdjudicated));//ProcCode
				string procDescript="";
				if(ProcedureCodes.IsValidCode(proc.ProcCodeAdjudicated)) {
					ProcedureCode procCode=ProcedureCodes.GetProcCode(proc.ProcCodeAdjudicated);
					procDescript=procCode.AbbrDesc;
				}
				row.Cells.Add(new ODGridCell(procDescript));//ProcDescript
				row.Cells.Add(new ODGridCell(proc.ProcFee.ToString("f2")));//ProcFee
				decimal adjAmtForProc=0;
				decimal patPortionForProc=0;
				decimal contractualForProc=0;
				decimal payorInitReductForProc=0;
				decimal otherAdjustForProc=0;
				for(int j=0;j<proc.ListProcAdjustments.Count;j++) {
					Hx835_Adj adj=proc.ListProcAdjustments[j];
					if(adj.AdjCode=="PR") {//Patient Responsibility
						patPortionForProc+=adj.AdjAmt;
					}
					else if(adj.AdjCode=="CO") {//Contractual Obligations
						contractualForProc+=adj.AdjAmt;
					}
					else if(adj.AdjCode=="PI") {//Payor Initiated Reductions
						payorInitReductForProc+=adj.AdjAmt;
					}
					else {//Other Adjustments
						otherAdjustForProc+=adj.AdjAmt;
					}
					adjAmtForProc+=adj.AdjAmt;
					_procAdjAmtSum+=adj.AdjAmt;
				}
				row.Cells.Add(new ODGridCell(patPortionForProc.ToString("f2")));//PatPortion
				row.Cells.Add(new ODGridCell(contractualForProc.ToString("f2")));//Contractual
				row.Cells.Add(new ODGridCell(payorInitReductForProc.ToString("f2")));//PayorReduct
				row.Cells.Add(new ODGridCell(otherAdjustForProc.ToString("f2")));//OtherAdjust
				row.Cells.Add(new ODGridCell(proc.InsPaid.ToString("f2")));//InsPaid
				gridProcedureBreakdown.Rows.Add(row);
			}
			gridProcedureBreakdown.EndUpdate();
		}

		private void FillGridProcedures(){
			//Changes made in this window do not get saved until after this window closes.
			//But if you double click on a row, then you will end up saving.  That shouldn't hurt anything, but could be improved.
			//also calculates totals for this "payment"
			//the payment itself is imaginary and is simply the sum of the claimprocs on this form
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableClaimProc","Date"),66);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Prov"),50);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Code"),50);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Tth"),25);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Description"),130);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Fee Billed"),62,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Deduct"),54,HorizontalAlignment.Right,true);//A little wider because the dedutible total textbox contains bold text.
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Allowed"),50,HorizontalAlignment.Right,true);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Ins Pay"),54,HorizontalAlignment.Right,true);//A little wider because the insurance payment total textbox contains bold text.
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Writeoff"),54,HorizontalAlignment.Right,true);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Status"),50,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Pmt"),30,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Remarks"),0,true);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			Procedure ProcCur;
			_listClaimProcsForClaim=ClaimProcs.RefreshForClaim(_claimPaid.GetOriginalClaimNum());
			for(int i=0;i<_listClaimProcsForClaim.Count;i++){
				row=new ODGridRow();
				row.Cells.Add(_listClaimProcsForClaim[i].ProcDate.ToShortDateString());
				row.Cells.Add(Providers.GetAbbr(_listClaimProcsForClaim[i].ProvNum));
				if(_listClaimProcsForClaim[i].ProcNum==0) {
					row.Cells.Add("");
					row.Cells.Add("");
					row.Cells.Add(Lan.g(this,"Total Payment"));
				}
				else {
					ProcCur=Procedures.GetProcFromList(_listProcs,_listClaimProcsForClaim[i].ProcNum);
					row.Cells.Add(ProcedureCodes.GetProcCode(ProcCur.CodeNum).ProcCode);
					row.Cells.Add(Tooth.ToInternat(ProcCur.ToothNum));
					row.Cells.Add(ProcedureCodes.GetProcCode(ProcCur.CodeNum).Descript);
				}
				row.Cells.Add(_listClaimProcsForClaim[i].FeeBilled.ToString("F"));
				row.Cells.Add(_listClaimProcsForClaim[i].DedApplied.ToString("F"));
				if(_listClaimProcsForClaim[i].AllowedOverride==-1){
					row.Cells.Add("");
				}
				else{
					row.Cells.Add(_listClaimProcsForClaim[i].AllowedOverride.ToString("F"));
				}
				row.Cells.Add(_listClaimProcsForClaim[i].InsPayAmt.ToString("F"));
				row.Cells.Add(_listClaimProcsForClaim[i].WriteOff.ToString("F"));
				switch(_listClaimProcsForClaim[i].Status){
					case ClaimProcStatus.Received:
						row.Cells.Add("Recd");
						break;
					case ClaimProcStatus.NotReceived:
						row.Cells.Add("");
						break;
					//adjustment would never show here
					case ClaimProcStatus.Preauth:
						row.Cells.Add("PreA");
						break;
					case ClaimProcStatus.Supplemental:
						row.Cells.Add("Supp");
						break;
					case ClaimProcStatus.CapClaim:
						row.Cells.Add("Cap");
						break;
					//Estimate would never show here
					//Cap would never show here
				}
				if(_listClaimProcsForClaim[i].ClaimPaymentNum>0){
					row.Cells.Add("X");
				}
				else{
					row.Cells.Add("");
				}
				row.Cells.Add(_listClaimProcsForClaim[i].Remarks);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			FillTotals();
		}

		private void gridMain_CellDoubleClick(object sender,OpenDental.UI.ODGridClickEventArgs e) {
			try{
				SaveGridChanges();
			}
			catch(ApplicationException ex){
				MessageBox.Show(ex.Message);
				return;
			}
			List<ClaimProcHist> histList=null;
			List<ClaimProcHist> loopList=null;
			FormClaimProc FormCP=new FormClaimProc(_listClaimProcsForClaim[e.Row],null,_famCur,_patCur,_listPlans,histList,ref loopList,_listPatPlans,false,_listInsSubs);
			FormCP.IsInClaim=true;
			//no need to worry about permissions here
			FormCP.ShowDialog();
			if(FormCP.DialogResult!=DialogResult.OK){
				return;
			}
			FillGridProcedures();
			FillTotals();
		}

		private void gridMain_CellTextChanged(object sender,EventArgs e) {
			FillTotals();
		}

		///<Summary>Fails silently if text is in invalid format.</Summary>
		private void FillTotals(){
			double claimFee=0;
			double dedApplied=0;
			double insPayAmtAllowed=0;
			double insPayAmt=0;
			double writeOff=0;
			//double amt;
			for(int i=0;i<gridMain.Rows.Count;i++){
				claimFee+=_listClaimProcsForClaim[i].FeeBilled;//5
				try{//6.deduct
					dedApplied+=Convert.ToDouble(gridMain.Rows[i].Cells[6].Text);
				}catch{}
				try{//7.allowed
					insPayAmtAllowed+=Convert.ToDouble(gridMain.Rows[i].Cells[7].Text);
				}catch{}
				try{//8.inspayest
					insPayAmt+=Convert.ToDouble(gridMain.Rows[i].Cells[8].Text);
				}catch{}
				try{//9.writeoff
					writeOff+=Convert.ToDouble(gridMain.Rows[i].Cells[9].Text);
				}catch{}
			}
			textClaimFee.Text=claimFee.ToString("F");
			textDedApplied.Text=dedApplied.ToString("F");
			textInsPayAllowed.Text=insPayAmtAllowed.ToString("F");
			textInsPayAmt.Text=insPayAmt.ToString("F");
			textWriteOff.Text=writeOff.ToString("F");
		}

		///<Summary>Surround with try-catch.</Summary>
		private void SaveGridChanges(){
			//validate all grid cells
			double dbl;
			for(int i=0;i<gridMain.Rows.Count;i++){
				if(gridMain.Rows[i].Cells[6].Text!=""){//deduct
					try{
						dbl=Convert.ToDouble(gridMain.Rows[i].Cells[6].Text);
					}
					catch{
						throw new ApplicationException(Lan.g(this,"Deductible not valid: ")+gridMain.Rows[i].Cells[6].Text);
					}
				}
				if(gridMain.Rows[i].Cells[7].Text!=""){//allowed
					try{
						dbl=Convert.ToDouble(gridMain.Rows[i].Cells[7].Text);
					}
					catch{
						throw new ApplicationException(Lan.g(this,"Allowed amt not valid: ")+gridMain.Rows[i].Cells[7].Text);
					}
				}
				if(gridMain.Rows[i].Cells[8].Text!=""){//inspay
					try{
						dbl=Convert.ToDouble(gridMain.Rows[i].Cells[8].Text);
					}
					catch{
						throw new ApplicationException(Lan.g(this,"Ins Pay not valid: ")+gridMain.Rows[i].Cells[8].Text);
					}
				}
				if(gridMain.Rows[i].Cells[9].Text!=""){//writeoff
					try{
						dbl=Convert.ToDouble(gridMain.Rows[i].Cells[9].Text);
						if(dbl<0){
							throw new ApplicationException(Lan.g(this,"Writeoff cannot be negative: ")+gridMain.Rows[i].Cells[9].Text);
						}
					}
					catch{
						throw new ApplicationException(Lan.g(this,"Writeoff not valid: ")+gridMain.Rows[i].Cells[9].Text);
					}
				}
			}
			for(int i=0;i<_listClaimProcsForClaim.Count;i++){
				_listClaimProcsForClaim[i].DedApplied=PIn.Double(gridMain.Rows[i].Cells[6].Text);
				if(gridMain.Rows[i].Cells[7].Text==""){
					_listClaimProcsForClaim[i].AllowedOverride=-1;
				}
				else{
					_listClaimProcsForClaim[i].AllowedOverride=PIn.Double(gridMain.Rows[i].Cells[7].Text);
				}
				_listClaimProcsForClaim[i].InsPayAmt=PIn.Double(gridMain.Rows[i].Cells[8].Text);
				_listClaimProcsForClaim[i].WriteOff=PIn.Double(gridMain.Rows[i].Cells[9].Text);
				_listClaimProcsForClaim[i].Remarks=gridMain.Rows[i].Cells[12].Text;
			}
		}

		private void butDeductible_Click(object sender, System.EventArgs e) {
			if(gridMain.SelectedCell.X==-1){
				MessageBox.Show(Lan.g(this,"Please select one procedure.  Then click this button to assign the deductible to that procedure."));
				return;
			}
			try {
				SaveGridChanges();
			}
			catch(ApplicationException ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			Double dedAmt=0;
			//remove the existing deductible from each procedure and move it to dedAmt.
			for(int i=0;i<_listClaimProcsForClaim.Count;i++) {
				if(_listClaimProcsForClaim[i].DedApplied > 0){
					dedAmt+=_listClaimProcsForClaim[i].DedApplied;
					_listClaimProcsForClaim[i].InsPayEst+=_listClaimProcsForClaim[i].DedApplied;//dedAmt might be more
					_listClaimProcsForClaim[i].InsPayAmt+=_listClaimProcsForClaim[i].DedApplied;
					_listClaimProcsForClaim[i].DedApplied=0;
				}
			}
			if(dedAmt==0){
				MessageBox.Show(Lan.g(this,"There does not seem to be a deductible to apply.  You can still apply a deductible manually by double clicking on a procedure."));
				return;
			}
			//then move dedAmt to the selected proc
			_listClaimProcsForClaim[gridMain.SelectedCell.Y].DedApplied=dedAmt;
			_listClaimProcsForClaim[gridMain.SelectedCell.Y].InsPayEst-=dedAmt;
			_listClaimProcsForClaim[gridMain.SelectedCell.Y].InsPayAmt-=dedAmt;
			FillGridProcedures();
		}

		private void butWriteOff_Click(object sender, System.EventArgs e) {
			if(MessageBox.Show(Lan.g(this,"Write off unpaid amount on each procedure?"),""
				,MessageBoxButtons.OKCancel)!=DialogResult.OK){
				return;
			}
			try {
				SaveGridChanges();
			}
			catch(ApplicationException ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			//fix later: does not take into account other payments.
			double unpaidAmt=0;
			List<Procedure> ProcList=Procedures.Refresh(_patCur.PatNum);
			for(int i=0;i<_listClaimProcsForClaim.Count;i++) {
				unpaidAmt=Procedures.GetProcFromList(ProcList,_listClaimProcsForClaim[i].ProcNum).ProcFee
					//((Procedure)Procedures.HList[ClaimProcsToEdit[i].ProcNum]).ProcFee
					-_listClaimProcsForClaim[i].DedApplied
					-_listClaimProcsForClaim[i].InsPayAmt;
				if(unpaidAmt > 0){
					_listClaimProcsForClaim[i].WriteOff=unpaidAmt;
				}
			}
			FillGridProcedures();
		}

		private void SaveAllowedFees(){
			//if no allowed fees entered, then nothing to do 
			bool allowedFeesEntered=false;
			for(int i=0;i<gridMain.Rows.Count;i++){
				if(gridMain.Rows[i].Cells[7].Text!=""){
					allowedFeesEntered=true;
					break;
				}
			}
			if(!allowedFeesEntered){
				return;
			}
			//if no allowed fee schedule, then nothing to do
			InsPlan plan=InsPlans.GetPlan(_listClaimProcsForClaim[0].PlanNum,_listPlans);
			if(plan.AllowedFeeSched==0){//no allowed fee sched
				//plan.PlanType!="p" && //not ppo, and 
				return;
			}
			//ask user if they want to save the fees
			if(!MsgBox.Show(this,true,"Save the allowed amounts to the allowed fee schedule?")){
				return;
			}
			//select the feeSchedule
			long feeSched=-1;
			//if(plan.PlanType=="p"){//ppo
			//	feeSched=plan.FeeSched;
			//}
			//else if(plan.AllowedFeeSched!=0){//an allowed fee schedule exists
			feeSched=plan.AllowedFeeSched;
			//}
			if(FeeScheds.GetIsHidden(feeSched)){
				MsgBox.Show(this,"Allowed fee schedule is hidden, so no changes can be made.");
				return;
			}
			Fee FeeCur=null;
			long codeNum;
			List<Procedure> ProcList=Procedures.Refresh(_patCur.PatNum);
			Procedure proc;
			for(int i=0;i<_listClaimProcsForClaim.Count;i++) {
				//this gives error message if proc not found:
				proc=Procedures.GetProcFromList(ProcList,_listClaimProcsForClaim[i].ProcNum);
				codeNum=proc.CodeNum;
				if(codeNum==0){
					continue;
				}
				FeeCur=Fees.GetFee(codeNum,feeSched);
				if(FeeCur==null){
					FeeCur=new Fee();
					FeeCur.FeeSched=feeSched;
					FeeCur.CodeNum=codeNum;
					FeeCur.Amount=PIn.Double(gridMain.Rows[i].Cells[7].Text);
					Fees.Insert(FeeCur);
				}
				else{
					FeeCur.Amount=PIn.Double(gridMain.Rows[i].Cells[7].Text);
					Fees.Update(FeeCur);
				}
				SecurityLogs.MakeLogEntry(Permissions.ProcFeeEdit,0,Lan.g(this,"Procedure")+": "+ProcedureCodes.GetStringProcCode(FeeCur.CodeNum)
					+", "+Lan.g(this,"Fee: ")+""+FeeCur.Amount.ToString("c")+", "+Lan.g(this,"Fee Schedule")+" "+FeeScheds.GetDescription(FeeCur.FeeSched)
					+". "+Lan.g(this,"Automatic change to allowed fee in Enter Payment window.  Confirmed by user."),FeeCur.CodeNum);
			}
			//Fees.Refresh();//redundant?
			DataValid.SetInvalid(InvalidType.Fees);
		}

		private void butViewEobDetails_Click(object sender,EventArgs e) {
			FormEtrans835ClaimEdit formE=new FormEtrans835ClaimEdit(_claimPaid);
			formE.ShowDialog();
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			try {
				SaveGridChanges();
			}
			catch(ApplicationException ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			SaveAllowedFees();
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormEtrans835ClaimPay_Activated(object sender,EventArgs e) {

		}

	}
}
