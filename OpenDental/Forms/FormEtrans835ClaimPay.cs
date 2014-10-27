using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	///<summary></summary>
	public class FormEtrans835ClaimPay : System.Windows.Forms.Form {
		private OpenDental.ValidDouble textWriteOff;
		private System.Windows.Forms.TextBox textInsPayAllowed;
		private System.Windows.Forms.TextBox textClaimFee;
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
		private OpenDental.UI.ODGrid gridPayments;
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
		private Claim _claim;
		private TextBox textDedApplied;
		private TextBox textInsPayAmt;
		private TextBox textEobInsPayAmt;
		private TextBox textEobDedApplied;
		private ValidDouble textEobWriteOff;
		private TextBox textEobInsPayAllowed;
		private TextBox textEobClaimFee;
		private Label label5;
		///<summary>The claim procs shown in the grid.  These procs are saved to/from the grid, but changes are not saved to the database unless the OK button is pressed or an individual claim proc is double-clicked for editing.</summary>
		public List <ClaimProc> ListClaimProcsForClaim;

		///<summary></summary>
		public FormEtrans835ClaimPay(Hx835_Claim claimPaid,Claim claim,Patient patCur,Family famCur,List<InsPlan> planList,List<PatPlan> patPlanList,List<InsSub> subList) {
			InitializeComponent();
			_claimPaid=claimPaid;
			_claim=claim;
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
			this.gridClaimAdjustments = new OpenDental.UI.ODGrid();
			this.gridProcedureBreakdown = new OpenDental.UI.ODGrid();
			this.gridPayments = new OpenDental.UI.ODGrid();
			this.textDedApplied = new System.Windows.Forms.TextBox();
			this.textInsPayAmt = new System.Windows.Forms.TextBox();
			this.butViewEobDetails = new OpenDental.UI.Button();
			this.butWriteOff = new OpenDental.UI.Button();
			this.butDeductible = new OpenDental.UI.Button();
			this.textWriteOff = new OpenDental.ValidDouble();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.textEobInsPayAmt = new System.Windows.Forms.TextBox();
			this.textEobDedApplied = new System.Windows.Forms.TextBox();
			this.textEobWriteOff = new OpenDental.ValidDouble();
			this.textEobInsPayAllowed = new System.Windows.Forms.TextBox();
			this.textEobClaimFee = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// textInsPayAllowed
			// 
			this.textInsPayAllowed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textInsPayAllowed.Location = new System.Drawing.Point(455, 608);
			this.textInsPayAllowed.Name = "textInsPayAllowed";
			this.textInsPayAllowed.ReadOnly = true;
			this.textInsPayAllowed.Size = new System.Drawing.Size(62, 20);
			this.textInsPayAllowed.TabIndex = 116;
			this.textInsPayAllowed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textClaimFee
			// 
			this.textClaimFee.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textClaimFee.Location = new System.Drawing.Point(331, 608);
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
			this.label1.Location = new System.Drawing.Point(177, 611);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(150, 16);
			this.label1.TabIndex = 117;
			this.label1.Text = "Totals";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Location = new System.Drawing.Point(346, 658);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(348, 39);
			this.label2.TabIndex = 122;
			this.label2.Text = "Before you click OK, the Deductible and the Ins Pay amounts should exactly match " +
    "the insurance EOB.";
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.Location = new System.Drawing.Point(20, 622);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(116, 34);
			this.label3.TabIndex = 123;
			this.label3.Text = "Assign to selected payment line:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label4.Location = new System.Drawing.Point(164, 627);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(108, 29);
			this.label4.TabIndex = 124;
			this.label4.Text = "On all unpaid procedure amounts:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
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
			this.gridClaimAdjustments.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridClaimAdjustments_CellDoubleClick);
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
			this.gridProcedureBreakdown.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridProcedureBreakdown_CellDoubleClick);
			// 
			// gridPayments
			// 
			this.gridPayments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridPayments.HScrollVisible = false;
			this.gridPayments.Location = new System.Drawing.Point(9, 345);
			this.gridPayments.Name = "gridPayments";
			this.gridPayments.ScrollValue = 0;
			this.gridPayments.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridPayments.Size = new System.Drawing.Size(956, 257);
			this.gridPayments.TabIndex = 125;
			this.gridPayments.Title = "Enter Payments";
			this.gridPayments.TranslationName = "TableClaimProc";
			this.gridPayments.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridPayments.CellTextChanged += new System.EventHandler(this.gridMain_CellTextChanged);
			// 
			// textDedApplied
			// 
			this.textDedApplied.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textDedApplied.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textDedApplied.Location = new System.Drawing.Point(393, 608);
			this.textDedApplied.Name = "textDedApplied";
			this.textDedApplied.ReadOnly = true;
			this.textDedApplied.Size = new System.Drawing.Size(62, 20);
			this.textDedApplied.TabIndex = 202;
			this.textDedApplied.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textInsPayAmt
			// 
			this.textInsPayAmt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textInsPayAmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textInsPayAmt.Location = new System.Drawing.Point(517, 608);
			this.textInsPayAmt.Name = "textInsPayAmt";
			this.textInsPayAmt.ReadOnly = true;
			this.textInsPayAmt.Size = new System.Drawing.Size(62, 20);
			this.textInsPayAmt.TabIndex = 203;
			this.textInsPayAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// butViewEobDetails
			// 
			this.butViewEobDetails.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butViewEobDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butViewEobDetails.Autosize = true;
			this.butViewEobDetails.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butViewEobDetails.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butViewEobDetails.CornerRadius = 4F;
			this.butViewEobDetails.Location = new System.Drawing.Point(703, 659);
			this.butViewEobDetails.Name = "butViewEobDetails";
			this.butViewEobDetails.Size = new System.Drawing.Size(100, 25);
			this.butViewEobDetails.TabIndex = 201;
			this.butViewEobDetails.Text = "EOB Details";
			this.butViewEobDetails.Click += new System.EventHandler(this.butViewEobDetails_Click);
			// 
			// butWriteOff
			// 
			this.butWriteOff.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butWriteOff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butWriteOff.Autosize = true;
			this.butWriteOff.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butWriteOff.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butWriteOff.CornerRadius = 4F;
			this.butWriteOff.Location = new System.Drawing.Point(163, 659);
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
			this.butDeductible.Location = new System.Drawing.Point(23, 659);
			this.butDeductible.Name = "butDeductible";
			this.butDeductible.Size = new System.Drawing.Size(92, 25);
			this.butDeductible.TabIndex = 120;
			this.butDeductible.Text = "&Deductible";
			this.butDeductible.Click += new System.EventHandler(this.butDeductible_Click);
			// 
			// textWriteOff
			// 
			this.textWriteOff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textWriteOff.Location = new System.Drawing.Point(579, 608);
			this.textWriteOff.MaxVal = 100000000D;
			this.textWriteOff.MinVal = -100000000D;
			this.textWriteOff.Name = "textWriteOff";
			this.textWriteOff.ReadOnly = true;
			this.textWriteOff.Size = new System.Drawing.Size(62, 20);
			this.textWriteOff.TabIndex = 119;
			this.textWriteOff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
			this.butCancel.Location = new System.Drawing.Point(890, 659);
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
			this.butOK.Location = new System.Drawing.Point(809, 659);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 25);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// textEobInsPayAmt
			// 
			this.textEobInsPayAmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textEobInsPayAmt.Location = new System.Drawing.Point(517, 292);
			this.textEobInsPayAmt.Name = "textEobInsPayAmt";
			this.textEobInsPayAmt.ReadOnly = true;
			this.textEobInsPayAmt.Size = new System.Drawing.Size(62, 20);
			this.textEobInsPayAmt.TabIndex = 209;
			this.textEobInsPayAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textEobDedApplied
			// 
			this.textEobDedApplied.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textEobDedApplied.Location = new System.Drawing.Point(393, 292);
			this.textEobDedApplied.Name = "textEobDedApplied";
			this.textEobDedApplied.ReadOnly = true;
			this.textEobDedApplied.Size = new System.Drawing.Size(62, 20);
			this.textEobDedApplied.TabIndex = 208;
			this.textEobDedApplied.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textEobWriteOff
			// 
			this.textEobWriteOff.Location = new System.Drawing.Point(579, 292);
			this.textEobWriteOff.MaxVal = 100000000D;
			this.textEobWriteOff.MinVal = -100000000D;
			this.textEobWriteOff.Name = "textEobWriteOff";
			this.textEobWriteOff.ReadOnly = true;
			this.textEobWriteOff.Size = new System.Drawing.Size(62, 20);
			this.textEobWriteOff.TabIndex = 207;
			this.textEobWriteOff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textEobInsPayAllowed
			// 
			this.textEobInsPayAllowed.Location = new System.Drawing.Point(455, 292);
			this.textEobInsPayAllowed.Name = "textEobInsPayAllowed";
			this.textEobInsPayAllowed.ReadOnly = true;
			this.textEobInsPayAllowed.Size = new System.Drawing.Size(62, 20);
			this.textEobInsPayAllowed.TabIndex = 204;
			this.textEobInsPayAllowed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textEobClaimFee
			// 
			this.textEobClaimFee.Location = new System.Drawing.Point(331, 292);
			this.textEobClaimFee.Name = "textEobClaimFee";
			this.textEobClaimFee.ReadOnly = true;
			this.textEobClaimFee.Size = new System.Drawing.Size(62, 20);
			this.textEobClaimFee.TabIndex = 206;
			this.textEobClaimFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(177, 295);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(150, 16);
			this.label5.TabIndex = 205;
			this.label5.Text = "EOB Totals";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// FormEtrans835ClaimPay
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(974, 696);
			this.Controls.Add(this.textEobInsPayAmt);
			this.Controls.Add(this.textEobDedApplied);
			this.Controls.Add(this.textEobWriteOff);
			this.Controls.Add(this.textEobInsPayAllowed);
			this.Controls.Add(this.textEobClaimFee);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textInsPayAmt);
			this.Controls.Add(this.textDedApplied);
			this.Controls.Add(this.butViewEobDetails);
			this.Controls.Add(this.gridClaimAdjustments);
			this.Controls.Add(this.gridProcedureBreakdown);
			this.Controls.Add(this.gridPayments);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butWriteOff);
			this.Controls.Add(this.butDeductible);
			this.Controls.Add(this.textWriteOff);
			this.Controls.Add(this.textInsPayAllowed);
			this.Controls.Add(this.textClaimFee);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(990, 734);
			this.Name = "FormEtrans835ClaimPay";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " ";
			this.Load += new System.EventHandler(this.FormEtrans835ClaimPay_Load);
			this.Shown += new System.EventHandler(this.FormEtrans835ClaimPay_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormEtrans835ClaimPay_Load(object sender, System.EventArgs e) {
			_listProcs=Procedures.Refresh(_patCur.PatNum);
			FillGridClaimAdjustments();
			FillGridProcedureBreakdown();
			textEobClaimFee.Text=_claimPaid.ClaimFee.ToString("F");
			textEobDedApplied.Text=_claimPaid.PatientPortion.ToString("F");
			textEobInsPayAllowed.Text=_claimPaid.AllowedAmt.ToString("F");
			textEobInsPayAmt.Text=_claimPaid.InsPaid.ToString("F");
			textEobWriteOff.Text=_claimPaid.Writeoff.ToString("F");
			FillGridProcedures();
		}

		private void FormEtrans835ClaimPay_Shown(object sender,EventArgs e) {
			InsPlan plan=InsPlans.GetPlan(ListClaimProcsForClaim[0].PlanNum,_listPlans);
			if(plan.AllowedFeeSched!=0){//allowed fee sched
				gridPayments.SetSelected(new Point(7,0));//Allowed column of the first row.
			}
			else{
				gridPayments.SetSelected(new Point(8,0));//InsPay column of the first row.
			}
		}

		private void FillGridClaimAdjustments() {
			if(_claimPaid.ListClaimAdjustments.Count==0) {
				gridClaimAdjustments.Title="EOB Claim Adjustments (None Reported)";
			}
			else {
				gridClaimAdjustments.Title="EOB Claim Adjustments";
			}
			gridClaimAdjustments.BeginUpdate();
			gridClaimAdjustments.Columns.Clear();
			gridClaimAdjustments.Columns.Add(new UI.ODGridColumn("Reason",445,HorizontalAlignment.Left));
			gridClaimAdjustments.Columns.Add(new UI.ODGridColumn("Allowed",62,HorizontalAlignment.Right));
			gridClaimAdjustments.Columns.Add(new UI.ODGridColumn("Ins Pay",62,HorizontalAlignment.Right));
			gridClaimAdjustments.Columns.Add(new UI.ODGridColumn("Writeoff",62,HorizontalAlignment.Right));
			gridClaimAdjustments.Columns.Add(new UI.ODGridColumn("",80,HorizontalAlignment.Center));
			gridClaimAdjustments.Columns.Add(new UI.ODGridColumn("Remarks",0,HorizontalAlignment.Left));
			gridClaimAdjustments.Rows.Clear();
			for(int i=0;i<_claimPaid.ListClaimAdjustments.Count;i++) {
				Hx835_Adj adj=_claimPaid.ListClaimAdjustments[i];
				ODGridRow row=new ODGridRow();
				row.Tag=adj;
				row.Cells.Add(new ODGridCell(adj.ReasonDescript));//Reason
				row.Cells.Add(new ODGridCell((-adj.AdjAmt).ToString("f2")));//Allowed
				row.Cells.Add(new ODGridCell((-adj.AdjAmt).ToString("f2")));//Ins Pay
				row.Cells.Add(new ODGridCell(adj.AdjAmt.ToString("f2")));//Writeoff
				row.Cells.Add(new ODGridCell(""));//Blank
				row.Cells.Add(new ODGridCell(adj.AdjustDescript));//Description
				gridClaimAdjustments.Rows.Add(row);
			}
			gridClaimAdjustments.EndUpdate();
		}

		private void FillGridProcedureBreakdown() {
			if(_claimPaid.ListProcs.Count==0) {
				gridProcedureBreakdown.Title="EOB Procedure Breakdown (None Reported)";
			}
			else {
				gridProcedureBreakdown.Title="EOB Procedure Breakdown";
			}
			gridProcedureBreakdown.BeginUpdate();
			gridProcedureBreakdown.Columns.Clear();
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("ProcNum",116,HorizontalAlignment.Left));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("Code",50,HorizontalAlignment.Center));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("",25,HorizontalAlignment.Center));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("Description",130,HorizontalAlignment.Left));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("Fee Billed",62,HorizontalAlignment.Right));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("Deduct",62,HorizontalAlignment.Right));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("Allowed",62,HorizontalAlignment.Right));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("InsPay",62,HorizontalAlignment.Right));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("Writeoff",62,HorizontalAlignment.Right));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("",80,HorizontalAlignment.Center));
			gridProcedureBreakdown.Columns.Add(new ODGridColumn("Remarks",0,HorizontalAlignment.Left));
			gridProcedureBreakdown.Rows.Clear();
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
				row.Cells.Add(new ODGridCell(proc.ProcCodeAdjudicated));//Code
				row.Cells.Add(new ODGridCell(""));//Blank
				string procDescript="";
				if(ProcedureCodes.IsValidCode(proc.ProcCodeAdjudicated)) {
					ProcedureCode procCode=ProcedureCodes.GetProcCode(proc.ProcCodeAdjudicated);
					procDescript=procCode.AbbrDesc;
				}
				row.Cells.Add(new ODGridCell(procDescript));//Description
				row.Cells.Add(new ODGridCell(proc.ProcFee.ToString("f2")));//Fee Billed
				row.Cells.Add(new ODGridCell(proc.PatientPortion.ToString("f2")));//Deduct
				row.Cells.Add(new ODGridCell(proc.AllowedAmt.ToString("f2")));//Allowed
				row.Cells.Add(new ODGridCell(proc.InsPaid.ToString("f2")));//InsPay
				row.Cells.Add(new ODGridCell(proc.Writeoff.ToString("f2")));//Writeoff
				row.Cells.Add(new ODGridCell(""));//Blank
				row.Cells.Add(new ODGridCell(proc.GetRemarks()));//Remarks
				gridProcedureBreakdown.Rows.Add(row);
			}
			gridProcedureBreakdown.EndUpdate();
		}

		private void FillGridProcedures(){
			//Changes made in this window do not get saved until after this window closes.
			//But if you double click on a row, then you will end up saving.  That shouldn't hurt anything, but could be improved.
			//also calculates totals for this "payment"
			//the payment itself is imaginary and is simply the sum of the claimprocs on this form
			gridPayments.BeginUpdate();
			gridPayments.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableClaimProc","Date"),66);
			gridPayments.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Prov"),50);
			gridPayments.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Code"),50);
			gridPayments.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Tth"),25);
			gridPayments.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Description"),130);
			gridPayments.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Fee Billed"),62,HorizontalAlignment.Right);
			gridPayments.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Deduct"),62,HorizontalAlignment.Right,true);
			gridPayments.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Allowed"),62,HorizontalAlignment.Right,true);
			gridPayments.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Ins Pay"),62,HorizontalAlignment.Right,true);
			gridPayments.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Writeoff"),62,HorizontalAlignment.Right,true);
			gridPayments.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Status"),50,HorizontalAlignment.Center);
			gridPayments.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Pmt"),30,HorizontalAlignment.Center);
			gridPayments.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Remarks"),0,true);
			gridPayments.Columns.Add(col);
			gridPayments.Rows.Clear();
			ODGridRow row;
			Procedure ProcCur;
			for(int i=0;i<ListClaimProcsForClaim.Count;i++){
				row=new ODGridRow();
				row.Cells.Add(ListClaimProcsForClaim[i].ProcDate.ToShortDateString());
				row.Cells.Add(Providers.GetAbbr(ListClaimProcsForClaim[i].ProvNum));
				if(ListClaimProcsForClaim[i].ProcNum==0) {
					row.Cells.Add("");
					row.Cells.Add("");
					row.Cells.Add(Lan.g(this,"Total Payment"));
				}
				else {
					ProcCur=Procedures.GetProcFromList(_listProcs,ListClaimProcsForClaim[i].ProcNum);
					row.Cells.Add(ProcedureCodes.GetProcCode(ProcCur.CodeNum).ProcCode);
					row.Cells.Add(Tooth.ToInternat(ProcCur.ToothNum));
					row.Cells.Add(ProcedureCodes.GetProcCode(ProcCur.CodeNum).Descript);
				}
				row.Cells.Add(ListClaimProcsForClaim[i].FeeBilled.ToString("F"));
				row.Cells.Add(ListClaimProcsForClaim[i].DedApplied.ToString("F"));
				if(ListClaimProcsForClaim[i].AllowedOverride==-1){
					row.Cells.Add("");
				}
				else{
					row.Cells.Add(ListClaimProcsForClaim[i].AllowedOverride.ToString("F"));
				}
				row.Cells.Add(ListClaimProcsForClaim[i].InsPayAmt.ToString("F"));
				row.Cells.Add(ListClaimProcsForClaim[i].WriteOff.ToString("F"));
				switch(ListClaimProcsForClaim[i].Status){
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
				if(ListClaimProcsForClaim[i].ClaimPaymentNum>0){
					row.Cells.Add("X");
				}
				else{
					row.Cells.Add("");
				}
				row.Cells.Add(ListClaimProcsForClaim[i].Remarks);
				gridPayments.Rows.Add(row);
			}
			gridPayments.EndUpdate();
			FillTotals();
		}

		private void gridClaimAdjustments_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Hx835_Adj adj=(Hx835_Adj)gridClaimAdjustments.Rows[e.Row].Tag;
			MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(adj.AdjCode+" "+adj.AdjustDescript+"\r\r"+adj.ReasonDescript+"\r\n"+adj.AdjAmt.ToString("f2"));
			msgbox.Show(this);
		}

		private void gridProcedureBreakdown_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Hx835_Proc proc=(Hx835_Proc)gridProcedureBreakdown.Rows[e.Row].Tag;
			FormEtrans835ProcEdit Form=new FormEtrans835ProcEdit(proc);
			Form.Show();
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
			ClaimProc claimProc=ListClaimProcsForClaim[e.Row];
			FormClaimProc FormCP=new FormClaimProc(claimProc,null,_famCur,_patCur,_listPlans,histList,ref loopList,_listPatPlans,false,_listInsSubs);
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
			for(int i=0;i<gridPayments.Rows.Count;i++){
				claimFee+=ListClaimProcsForClaim[i].FeeBilled;//5
				dedApplied+=PIn.Double(gridPayments.Rows[i].Cells[6].Text);//6.deduct
				insPayAmtAllowed+=PIn.Double(gridPayments.Rows[i].Cells[7].Text);//7.allowed
				insPayAmt+=PIn.Double(gridPayments.Rows[i].Cells[8].Text);//8.inspayest
				writeOff+=PIn.Double(gridPayments.Rows[i].Cells[9].Text);//9.writeoff
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
			for(int i=0;i<gridPayments.Rows.Count;i++){
				if(gridPayments.Rows[i].Cells[6].Text!=""){//deduct
					try{
						dbl=Convert.ToDouble(gridPayments.Rows[i].Cells[6].Text);
					}
					catch{
						throw new ApplicationException(Lan.g(this,"Deductible not valid: ")+gridPayments.Rows[i].Cells[6].Text);
					}
				}
				if(gridPayments.Rows[i].Cells[7].Text!=""){//allowed
					try{
						dbl=Convert.ToDouble(gridPayments.Rows[i].Cells[7].Text);
					}
					catch{
						throw new ApplicationException(Lan.g(this,"Allowed amt not valid: ")+gridPayments.Rows[i].Cells[7].Text);
					}
				}
				if(gridPayments.Rows[i].Cells[8].Text!=""){//inspay
					try{
						dbl=Convert.ToDouble(gridPayments.Rows[i].Cells[8].Text);
					}
					catch{
						throw new ApplicationException(Lan.g(this,"Ins Pay not valid: ")+gridPayments.Rows[i].Cells[8].Text);
					}
				}
				if(gridPayments.Rows[i].Cells[9].Text!=""){//writeoff
					try{
						dbl=Convert.ToDouble(gridPayments.Rows[i].Cells[9].Text);
						if(dbl<0){
							throw new ApplicationException(Lan.g(this,"Writeoff cannot be negative: ")+gridPayments.Rows[i].Cells[9].Text);
						}
					}
					catch{
						throw new ApplicationException(Lan.g(this,"Writeoff not valid: ")+gridPayments.Rows[i].Cells[9].Text);
					}
				}
			}
			for(int i=0;i<ListClaimProcsForClaim.Count;i++){
				ListClaimProcsForClaim[i].DedApplied=PIn.Double(gridPayments.Rows[i].Cells[6].Text);
				if(gridPayments.Rows[i].Cells[7].Text==""){
					ListClaimProcsForClaim[i].AllowedOverride=-1;
				}
				else{
					ListClaimProcsForClaim[i].AllowedOverride=PIn.Double(gridPayments.Rows[i].Cells[7].Text);
				}
				ListClaimProcsForClaim[i].InsPayAmt=PIn.Double(gridPayments.Rows[i].Cells[8].Text);
				ListClaimProcsForClaim[i].WriteOff=PIn.Double(gridPayments.Rows[i].Cells[9].Text);
				ListClaimProcsForClaim[i].Remarks=gridPayments.Rows[i].Cells[12].Text;
			}
		}

		private void butDeductible_Click(object sender, System.EventArgs e) {
			if(gridPayments.SelectedCell.X==-1) {
				MessageBox.Show(Lan.g(this,"Please select one payment line.  Then click this button to assign the deductible to that line."));
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
			//remove the existing deductible from each payment line and move it to dedAmt.
			for(int i=0;i<ListClaimProcsForClaim.Count;i++) {
				if(ListClaimProcsForClaim[i].DedApplied > 0){
					dedAmt+=ListClaimProcsForClaim[i].DedApplied;
					ListClaimProcsForClaim[i].InsPayEst+=ListClaimProcsForClaim[i].DedApplied;//dedAmt might be more
					ListClaimProcsForClaim[i].InsPayAmt+=ListClaimProcsForClaim[i].DedApplied;
					ListClaimProcsForClaim[i].DedApplied=0;
				}
			}
			if(dedAmt==0){
				MessageBox.Show(Lan.g(this,"There does not seem to be a deductible to apply.  You can still apply a deductible manually by double clicking on a payment line."));
				return;
			}
			//then move dedAmt to the selected proc
			ListClaimProcsForClaim[gridPayments.SelectedCell.Y].DedApplied=dedAmt;
			ListClaimProcsForClaim[gridPayments.SelectedCell.Y].InsPayEst-=dedAmt;
			ListClaimProcsForClaim[gridPayments.SelectedCell.Y].InsPayAmt-=dedAmt;
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
			for(int i=0;i<ListClaimProcsForClaim.Count;i++) {
				if(ListClaimProcsForClaim[i].ProcNum==0) {
					continue;//Ignore "Total Payment" lines.
				}
				unpaidAmt=Procedures.GetProcFromList(ProcList,ListClaimProcsForClaim[i].ProcNum).ProcFee
					//((Procedure)Procedures.HList[ClaimProcsToEdit[i].ProcNum]).ProcFee
					-ListClaimProcsForClaim[i].DedApplied
					-ListClaimProcsForClaim[i].InsPayAmt;
				if(unpaidAmt > 0){
					ListClaimProcsForClaim[i].WriteOff=unpaidAmt;
				}
			}
			FillGridProcedures();
		}

		private void SaveAllowedFees(){
			//if no allowed fees entered, then nothing to do 
			bool allowedFeesEntered=false;
			for(int i=0;i<gridPayments.Rows.Count;i++){
				if(gridPayments.Rows[i].Cells[7].Text!=""){
					allowedFeesEntered=true;
					break;
				}
			}
			if(!allowedFeesEntered){
				return;
			}
			//if no allowed fee schedule, then nothing to do
			InsPlan plan=InsPlans.GetPlan(ListClaimProcsForClaim[0].PlanNum,_listPlans);
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
			feeSched=plan.AllowedFeeSched;
			if(FeeScheds.GetIsHidden(feeSched)){
				MsgBox.Show(this,"Allowed fee schedule is hidden, so no changes can be made.");
				return;
			}
			Fee feeCur=null;
			long codeNum;
			List<Procedure> listProcs=Procedures.Refresh(_patCur.PatNum);
			Procedure proc;
			for(int i=0;i<ListClaimProcsForClaim.Count;i++) {
				proc=Procedures.GetProcFromList(listProcs,ListClaimProcsForClaim[i].ProcNum);
				codeNum=proc.CodeNum;
				//skip total payments
				if(codeNum==0){
					continue;
				}
				feeCur=Fees.GetFee(codeNum,feeSched);
				if(feeCur==null){
					feeCur=new Fee();
					feeCur.FeeSched=feeSched;
					feeCur.CodeNum=codeNum;
					feeCur.Amount=PIn.Double(gridPayments.Rows[i].Cells[7].Text);
					Fees.Insert(feeCur);
				}
				else{
					feeCur.Amount=PIn.Double(gridPayments.Rows[i].Cells[7].Text);
					Fees.Update(feeCur);
				}
				SecurityLogs.MakeLogEntry(Permissions.ProcFeeEdit,0,Lan.g(this,"Procedure")+": "+ProcedureCodes.GetStringProcCode(feeCur.CodeNum)
					+", "+Lan.g(this,"Fee: ")+""+feeCur.Amount.ToString("c")+", "+Lan.g(this,"Fee Schedule")+" "+FeeScheds.GetDescription(feeCur.FeeSched)
					+". "+Lan.g(this,"Automatic change to allowed fee in Enter Payment window.  Confirmed by user."),feeCur.CodeNum);
			}
			DataValid.SetInvalid(InvalidType.Fees);
		}

		private void butViewEobDetails_Click(object sender,EventArgs e) {
			FormEtrans835ClaimEdit FormE=new FormEtrans835ClaimEdit(_claimPaid);
			FormE.ShowDialog();
		}

		///<summary>Called when OK is clicked to receive the claim and to set the claim dates and totals properly.
		///Public so it can also be used for automatic payment entry.</summary>
		public void ReceivePayment() {
			//Recalculate insurance paid, deductible, and writeoff amounts for the claim based on the final claimproc values, then save the results to the database.
			_claim.InsPayAmt=0;
			_claim.DedApplied=0;
			_claim.WriteOff=0;
			for(int i=0;i<ListClaimProcsForClaim.Count;i++) {
				_claim.InsPayAmt+=ListClaimProcsForClaim[i].InsPayAmt;
				_claim.DedApplied+=ListClaimProcsForClaim[i].DedApplied;
				_claim.WriteOff+=ListClaimProcsForClaim[i].WriteOff;
				if(ListClaimProcsForClaim[i].ClaimProcNum==0) {//Total payment claimproc which was created in FormEtrans835Edit just before loading this window.
					ClaimProcs.Insert(ListClaimProcsForClaim[i]);
				}
				else {//Procedure claimproc, because the estimate already existed before entering payment.
					ClaimProcs.Update(ListClaimProcsForClaim[i]);
				}
			}
			_claim.ClaimStatus="R";//Received.
			_claim.DateReceived=_claimPaid.DateReceived;
			Claims.Update(_claim);
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			try {
				SaveGridChanges();
			}
			catch(ApplicationException ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			if(textEobClaimFee.Text!=textClaimFee.Text || textEobDedApplied.Text!=textDedApplied.Text || textEobInsPayAllowed.Text!=textInsPayAllowed.Text
				|| textEobInsPayAmt.Text!=textInsPayAmt.Text || textEobWriteOff.Text!=textWriteOff.Text) 
			{
				if(MessageBox.Show(Lan.g(this,"Some of the EOB totals do not match the totals entered")+".  "+Lan.g(this,"Continue")+"?","",MessageBoxButtons.OKCancel)!=DialogResult.OK) {
					return;
				}
			}
			SaveAllowedFees();
			ReceivePayment();
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}
