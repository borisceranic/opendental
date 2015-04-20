using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using OpenDental.ReportingComplex;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormPayPlan : System.Windows.Forms.Form{
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label label2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.GroupBox groupBox2;
		private OpenDental.ValidDate textDate;
		private OpenDental.ValidDouble textAmount;
		private OpenDental.ValidDate textDateFirstPay;
		private OpenDental.ValidDouble textAPR;
		private OpenDental.ValidNum textTerm;
		private OpenDental.UI.Button butPrint;
		private System.Windows.Forms.TextBox textGuarantor;
		///<summary></summary>
		public bool IsNew;
		private OpenDental.UI.Button butGoToGuar;
		private OpenDental.UI.Button butGoToPat;
		private System.Windows.Forms.TextBox textPatient;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.GroupBox groupBox3;
		private OpenDental.ValidDouble textDownPayment;
		private System.Drawing.Printing.PrintDocument pd2;
		private System.Windows.Forms.Label label12;
		/// <summary>Go to the specified patnum.  Upon dialog close, if this number is not 0, then patients.Cur will be changed to this new patnum, and Account refreshed to the new patient.</summary>
		public long GotoPatNum;
		private System.Windows.Forms.Label label13;
		//private double amtPaid;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.TextBox textTotalCost;
		private System.Windows.Forms.Label label10;
		private OpenDental.UI.Button butDelete;
		private OpenDental.ODtextBox textNote;
		private Patient PatCur;
		private System.Windows.Forms.TextBox textAccumulatedDue;
		private OpenDental.UI.Button butCreateSched;
		private OpenDental.ValidDouble textPeriodPayment;
		private PayPlan PayPlanCur;
		private OpenDental.UI.Button butChangeGuar;
		private System.Windows.Forms.TextBox textInsPlan;
		private OpenDental.UI.Button butChangePlan;
		private System.Windows.Forms.CheckBox checkIns;
		private System.Windows.Forms.Label labelGuarantor;
		private System.Windows.Forms.Label labelInsPlan;
		///<summary>Only used for new payment plan.  Pass in the starting amount.  Usually the patient account balance.</summary>
		public double TotalAmt;
		///<summary>Family for the patient of this payplan.  Used to display insurance info.</summary>
		private Family FamCur;
		///<summary>Used to display insurance info.</summary>
		private List <InsPlan> InsPlanList;
		private OpenDental.UI.ODGrid gridCharges;
		private OpenDental.UI.Button butClear;
		private OpenDental.UI.Button butAdd;
		private System.Windows.Forms.TextBox textAmtPaid;
		private System.Windows.Forms.TextBox textPrincPaid;
		private System.Windows.Forms.Label label14;
		//private List<PayPlanCharge> ChargeList;
		private double AmtPaid;
		private double TotPrinc;
		private double TotInt;
		private Label label1;
		private ValidDouble textCompletedAmt;
		private Label label3;
		private OpenDental.UI.Button butPickProv;
		private ComboBox comboProv;
		private ComboBox comboClinic;
		private Label labelClinic;
		private Label label16;
		private GroupBox groupBox1;
		private double TotPrincInt;
		private UI.Button butMoreOptions;
		private List<InsSub> SubList;
		///<summary>This form is reused as long as this parent form remains open.</summary>
		private FormPaymentPlanOptions FormPayPlanOpts;
		///<summary>Cached list of PayPlanCharges.</summary>
		private List<PayPlanCharge> _listPayPlanCharges;
		private ValidDouble textBalance;
		private TextBox textInterest;
		private ValidDouble textPayment;
		private ValidDouble textPrincipal;
		private Label labelTotals;
		private Def[] _arrayAccountColors;//Putting this here so we do one DB call for colors instead of many.  They'll never change.

		///<summary>The supplied payment plan should already have been saved in the database.</summary>
		public FormPayPlan(Patient patCur,PayPlan payPlanCur){
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			PatCur=patCur.Copy();
			PayPlanCur=payPlanCur.Copy();
			FamCur=Patients.GetFamily(PatCur.PatNum);
			SubList=InsSubs.RefreshForFam(FamCur);
			InsPlanList=InsPlans.RefreshForSubList(SubList);
			FormPayPlanOpts=new FormPaymentPlanOptions(PayPlanCur.PaySchedule);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPayPlan));
			this.labelGuarantor = new System.Windows.Forms.Label();
			this.textGuarantor = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label11 = new System.Windows.Forms.Label();
			this.textTotalCost = new System.Windows.Forms.TextBox();
			this.label15 = new System.Windows.Forms.Label();
			this.textPatient = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.pd2 = new System.Drawing.Printing.PrintDocument();
			this.label12 = new System.Windows.Forms.Label();
			this.textAmtPaid = new System.Windows.Forms.TextBox();
			this.textAccumulatedDue = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.textInsPlan = new System.Windows.Forms.TextBox();
			this.labelInsPlan = new System.Windows.Forms.Label();
			this.checkIns = new System.Windows.Forms.CheckBox();
			this.textPrincPaid = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.comboProv = new System.Windows.Forms.ComboBox();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.textInterest = new System.Windows.Forms.TextBox();
			this.labelTotals = new System.Windows.Forms.Label();
			this.gridCharges = new OpenDental.UI.ODGrid();
			this.textBalance = new OpenDental.ValidDouble();
			this.textPayment = new OpenDental.ValidDouble();
			this.textPrincipal = new OpenDental.ValidDouble();
			this.butPickProv = new OpenDental.UI.Button();
			this.textCompletedAmt = new OpenDental.ValidDouble();
			this.butAdd = new OpenDental.UI.Button();
			this.butClear = new OpenDental.UI.Button();
			this.butChangePlan = new OpenDental.UI.Button();
			this.textNote = new OpenDental.ODtextBox();
			this.butDelete = new OpenDental.UI.Button();
			this.butGoToPat = new OpenDental.UI.Button();
			this.butGoToGuar = new OpenDental.UI.Button();
			this.textDate = new OpenDental.ValidDate();
			this.butChangeGuar = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butMoreOptions = new OpenDental.UI.Button();
			this.textAPR = new OpenDental.ValidDouble();
			this.textPeriodPayment = new OpenDental.ValidDouble();
			this.textTerm = new OpenDental.ValidNum();
			this.textDownPayment = new OpenDental.ValidDouble();
			this.textDateFirstPay = new OpenDental.ValidDate();
			this.textAmount = new OpenDental.ValidDouble();
			this.butCreateSched = new OpenDental.UI.Button();
			this.butPrint = new OpenDental.UI.Button();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelGuarantor
			// 
			this.labelGuarantor.Location = new System.Drawing.Point(28, 32);
			this.labelGuarantor.Name = "labelGuarantor";
			this.labelGuarantor.Size = new System.Drawing.Size(126, 17);
			this.labelGuarantor.TabIndex = 0;
			this.labelGuarantor.Text = "Guarantor";
			this.labelGuarantor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textGuarantor
			// 
			this.textGuarantor.Location = new System.Drawing.Point(156, 32);
			this.textGuarantor.Name = "textGuarantor";
			this.textGuarantor.ReadOnly = true;
			this.textGuarantor.Size = new System.Drawing.Size(199, 20);
			this.textGuarantor.TabIndex = 0;
			this.textGuarantor.TabStop = false;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(21, 190);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(133, 17);
			this.label2.TabIndex = 0;
			this.label2.Text = "Date of Agreement";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(5, 14);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(134, 17);
			this.label4.TabIndex = 0;
			this.label4.Text = "Total Amount";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(5, 36);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(135, 17);
			this.label5.TabIndex = 0;
			this.label5.Text = "Date of First Payment";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(3, 80);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(138, 17);
			this.label6.TabIndex = 0;
			this.label6.Text = "APR (for example 0 or 18)";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 40);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(122, 17);
			this.label7.TabIndex = 0;
			this.label7.Text = "Payment Amt";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(7, 18);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(124, 17);
			this.label8.TabIndex = 0;
			this.label8.Text = "Number of Payments";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.butMoreOptions);
			this.groupBox2.Controls.Add(this.textAPR);
			this.groupBox2.Controls.Add(this.groupBox3);
			this.groupBox2.Controls.Add(this.textDownPayment);
			this.groupBox2.Controls.Add(this.label11);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.textDateFirstPay);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.textAmount);
			this.groupBox2.Controls.Add(this.butCreateSched);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(14, 210);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(415, 170);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Terms";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label7);
			this.groupBox3.Controls.Add(this.textPeriodPayment);
			this.groupBox3.Controls.Add(this.textTerm);
			this.groupBox3.Controls.Add(this.label8);
			this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox3.Location = new System.Drawing.Point(9, 101);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(235, 64);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Either";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(4, 59);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(136, 17);
			this.label11.TabIndex = 0;
			this.label11.Text = "Down Payment";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTotalCost
			// 
			this.textTotalCost.Location = new System.Drawing.Point(156, 385);
			this.textTotalCost.Name = "textTotalCost";
			this.textTotalCost.ReadOnly = true;
			this.textTotalCost.Size = new System.Drawing.Size(85, 20);
			this.textTotalCost.TabIndex = 0;
			this.textTotalCost.TabStop = false;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(19, 385);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(134, 17);
			this.label15.TabIndex = 0;
			this.label15.Text = "Total Cost of Loan";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPatient
			// 
			this.textPatient.Location = new System.Drawing.Point(156, 10);
			this.textPatient.Name = "textPatient";
			this.textPatient.ReadOnly = true;
			this.textPatient.Size = new System.Drawing.Size(199, 20);
			this.textPatient.TabIndex = 0;
			this.textPatient.TabStop = false;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(30, 10);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(124, 17);
			this.label9.TabIndex = 0;
			this.label9.Text = "Patient";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(22, 431);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(133, 17);
			this.label12.TabIndex = 0;
			this.label12.Text = "Paid so far";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textAmtPaid
			// 
			this.textAmtPaid.Location = new System.Drawing.Point(156, 429);
			this.textAmtPaid.Name = "textAmtPaid";
			this.textAmtPaid.ReadOnly = true;
			this.textAmtPaid.Size = new System.Drawing.Size(85, 20);
			this.textAmtPaid.TabIndex = 0;
			this.textAmtPaid.TabStop = false;
			// 
			// textAccumulatedDue
			// 
			this.textAccumulatedDue.Location = new System.Drawing.Point(156, 407);
			this.textAccumulatedDue.Name = "textAccumulatedDue";
			this.textAccumulatedDue.ReadOnly = true;
			this.textAccumulatedDue.Size = new System.Drawing.Size(85, 20);
			this.textAccumulatedDue.TabIndex = 0;
			this.textAccumulatedDue.TabStop = false;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(20, 409);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(135, 17);
			this.label13.TabIndex = 0;
			this.label13.Text = "Accumulated Due";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(23, 507);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(148, 17);
			this.label10.TabIndex = 0;
			this.label10.Text = "Note";
			this.label10.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textInsPlan
			// 
			this.textInsPlan.Location = new System.Drawing.Point(156, 167);
			this.textInsPlan.Name = "textInsPlan";
			this.textInsPlan.ReadOnly = true;
			this.textInsPlan.Size = new System.Drawing.Size(199, 20);
			this.textInsPlan.TabIndex = 0;
			this.textInsPlan.TabStop = false;
			// 
			// labelInsPlan
			// 
			this.labelInsPlan.Location = new System.Drawing.Point(21, 167);
			this.labelInsPlan.Name = "labelInsPlan";
			this.labelInsPlan.Size = new System.Drawing.Size(132, 17);
			this.labelInsPlan.TabIndex = 0;
			this.labelInsPlan.Text = "Insurance Plan";
			this.labelInsPlan.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkIns
			// 
			this.checkIns.Location = new System.Drawing.Point(156, 148);
			this.checkIns.Name = "checkIns";
			this.checkIns.Size = new System.Drawing.Size(268, 18);
			this.checkIns.TabIndex = 14;
			this.checkIns.Text = "Use for tracking expected insurance payments";
			this.checkIns.Click += new System.EventHandler(this.checkIns_Click);
			// 
			// textPrincPaid
			// 
			this.textPrincPaid.Location = new System.Drawing.Point(156, 451);
			this.textPrincPaid.Name = "textPrincPaid";
			this.textPrincPaid.ReadOnly = true;
			this.textPrincPaid.Size = new System.Drawing.Size(85, 20);
			this.textPrincPaid.TabIndex = 0;
			this.textPrincPaid.TabStop = false;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(22, 453);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(133, 17);
			this.label14.TabIndex = 0;
			this.label14.Text = "Principal paid so far";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(4, 475);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(151, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Tx Completed Amt";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(264, 474);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(160, 40);
			this.label3.TabIndex = 0;
			this.label3.Text = "This should usually match the total amount of the pay plan.";
			// 
			// comboProv
			// 
			this.comboProv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProv.Location = new System.Drawing.Point(142, 14);
			this.comboProv.MaxDropDownItems = 30;
			this.comboProv.Name = "comboProv";
			this.comboProv.Size = new System.Drawing.Size(158, 21);
			this.comboProv.TabIndex = 1;
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(142, 39);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(177, 21);
			this.comboClinic.TabIndex = 3;
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(26, 41);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(114, 16);
			this.labelClinic.TabIndex = 0;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(41, 18);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(100, 16);
			this.label16.TabIndex = 0;
			this.label16.Text = "Provider";
			this.label16.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.comboClinic);
			this.groupBox1.Controls.Add(this.butPickProv);
			this.groupBox1.Controls.Add(this.label16);
			this.groupBox1.Controls.Add(this.comboProv);
			this.groupBox1.Controls.Add(this.labelClinic);
			this.groupBox1.Location = new System.Drawing.Point(14, 76);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(347, 65);
			this.groupBox1.TabIndex = 13;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Same for all charges";
			// 
			// textInterest
			// 
			this.textInterest.Location = new System.Drawing.Point(761, 528);
			this.textInterest.Name = "textInterest";
			this.textInterest.ReadOnly = true;
			this.textInterest.Size = new System.Drawing.Size(60, 20);
			this.textInterest.TabIndex = 141;
			this.textInterest.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelTotals
			// 
			this.labelTotals.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTotals.Location = new System.Drawing.Point(435, 531);
			this.labelTotals.Name = "labelTotals";
			this.labelTotals.Size = new System.Drawing.Size(266, 15);
			this.labelTotals.TabIndex = 142;
			this.labelTotals.Text = "Current Totals";
			this.labelTotals.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// gridCharges
			// 
			this.gridCharges.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridCharges.HScrollVisible = false;
			this.gridCharges.Location = new System.Drawing.Point(435, 9);
			this.gridCharges.Name = "gridCharges";
			this.gridCharges.ScrollValue = 0;
			this.gridCharges.Size = new System.Drawing.Size(536, 515);
			this.gridCharges.TabIndex = 41;
			this.gridCharges.Title = "Amortization Schedule";
			this.gridCharges.TranslationName = "PayPlanAmortization";
			this.gridCharges.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridCharges_CellDoubleClick);
			// 
			// textBalance
			// 
			this.textBalance.Location = new System.Drawing.Point(881, 528);
			this.textBalance.MaxVal = 100000000D;
			this.textBalance.MinVal = -100000000D;
			this.textBalance.Name = "textBalance";
			this.textBalance.ReadOnly = true;
			this.textBalance.Size = new System.Drawing.Size(60, 20);
			this.textBalance.TabIndex = 144;
			this.textBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textPayment
			// 
			this.textPayment.Location = new System.Drawing.Point(821, 528);
			this.textPayment.MaxVal = 100000000D;
			this.textPayment.MinVal = -100000000D;
			this.textPayment.Name = "textPayment";
			this.textPayment.ReadOnly = true;
			this.textPayment.Size = new System.Drawing.Size(60, 20);
			this.textPayment.TabIndex = 140;
			this.textPayment.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textPrincipal
			// 
			this.textPrincipal.Location = new System.Drawing.Point(701, 528);
			this.textPrincipal.MaxVal = 100000000D;
			this.textPrincipal.MinVal = -100000000D;
			this.textPrincipal.Name = "textPrincipal";
			this.textPrincipal.ReadOnly = true;
			this.textPrincipal.Size = new System.Drawing.Size(60, 20);
			this.textPrincipal.TabIndex = 139;
			this.textPrincipal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// butPickProv
			// 
			this.butPickProv.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickProv.Autosize = false;
			this.butPickProv.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickProv.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickProv.CornerRadius = 2F;
			this.butPickProv.Location = new System.Drawing.Point(301, 14);
			this.butPickProv.Name = "butPickProv";
			this.butPickProv.Size = new System.Drawing.Size(18, 21);
			this.butPickProv.TabIndex = 2;
			this.butPickProv.Text = "...";
			// 
			// textCompletedAmt
			// 
			this.textCompletedAmt.Location = new System.Drawing.Point(156, 473);
			this.textCompletedAmt.MaxVal = 100000000D;
			this.textCompletedAmt.MinVal = -100000000D;
			this.textCompletedAmt.Name = "textCompletedAmt";
			this.textCompletedAmt.Size = new System.Drawing.Size(85, 20);
			this.textCompletedAmt.TabIndex = 2;
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(435, 611);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(84, 24);
			this.butAdd.TabIndex = 4;
			this.butAdd.Text = "Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butClear
			// 
			this.butClear.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butClear.Autosize = true;
			this.butClear.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClear.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClear.CornerRadius = 4F;
			this.butClear.Location = new System.Drawing.Point(534, 611);
			this.butClear.Name = "butClear";
			this.butClear.Size = new System.Drawing.Size(99, 24);
			this.butClear.TabIndex = 5;
			this.butClear.Text = "Clear Schedule";
			this.butClear.Click += new System.EventHandler(this.butClear_Click);
			// 
			// butChangePlan
			// 
			this.butChangePlan.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butChangePlan.Autosize = true;
			this.butChangePlan.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butChangePlan.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butChangePlan.CornerRadius = 4F;
			this.butChangePlan.Location = new System.Drawing.Point(354, 166);
			this.butChangePlan.Name = "butChangePlan";
			this.butChangePlan.Size = new System.Drawing.Size(75, 22);
			this.butChangePlan.TabIndex = 15;
			this.butChangePlan.Text = "C&hange";
			this.butChangePlan.Click += new System.EventHandler(this.butChangePlan_Click);
			// 
			// textNote
			// 
			this.textNote.AcceptsTab = true;
			this.textNote.DetectUrls = false;
			this.textNote.Location = new System.Drawing.Point(22, 528);
			this.textNote.Name = "textNote";
			this.textNote.QuickPasteType = OpenDentBusiness.QuickPasteType.PayPlan;
			this.textNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textNote.Size = new System.Drawing.Size(392, 121);
			this.textNote.TabIndex = 3;
			this.textNote.TabStop = false;
			this.textNote.Text = "";
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(22, 660);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(84, 24);
			this.butDelete.TabIndex = 9;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butGoToPat
			// 
			this.butGoToPat.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGoToPat.Autosize = true;
			this.butGoToPat.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGoToPat.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGoToPat.CornerRadius = 4F;
			this.butGoToPat.Location = new System.Drawing.Point(354, 9);
			this.butGoToPat.Name = "butGoToPat";
			this.butGoToPat.Size = new System.Drawing.Size(75, 22);
			this.butGoToPat.TabIndex = 10;
			this.butGoToPat.Text = "&Go To";
			this.butGoToPat.Click += new System.EventHandler(this.butGoToPat_Click);
			// 
			// butGoToGuar
			// 
			this.butGoToGuar.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGoToGuar.Autosize = true;
			this.butGoToGuar.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGoToGuar.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGoToGuar.CornerRadius = 4F;
			this.butGoToGuar.Location = new System.Drawing.Point(354, 31);
			this.butGoToGuar.Name = "butGoToGuar";
			this.butGoToGuar.Size = new System.Drawing.Size(75, 22);
			this.butGoToGuar.TabIndex = 11;
			this.butGoToGuar.Text = "Go &To";
			this.butGoToGuar.Click += new System.EventHandler(this.butGoTo_Click);
			// 
			// textDate
			// 
			this.textDate.Location = new System.Drawing.Point(156, 189);
			this.textDate.Name = "textDate";
			this.textDate.Size = new System.Drawing.Size(85, 20);
			this.textDate.TabIndex = 16;
			// 
			// butChangeGuar
			// 
			this.butChangeGuar.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butChangeGuar.Autosize = true;
			this.butChangeGuar.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butChangeGuar.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butChangeGuar.CornerRadius = 4F;
			this.butChangeGuar.Location = new System.Drawing.Point(354, 53);
			this.butChangeGuar.Name = "butChangeGuar";
			this.butChangeGuar.Size = new System.Drawing.Size(75, 22);
			this.butChangeGuar.TabIndex = 12;
			this.butChangeGuar.Text = "C&hange";
			this.butChangeGuar.Click += new System.EventHandler(this.butChangeGuar_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(787, 660);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 7;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
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
			this.butCancel.Location = new System.Drawing.Point(880, 660);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 8;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butMoreOptions
			// 
			this.butMoreOptions.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMoreOptions.Autosize = true;
			this.butMoreOptions.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMoreOptions.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMoreOptions.CornerRadius = 4F;
			this.butMoreOptions.Location = new System.Drawing.Point(250, 110);
			this.butMoreOptions.Name = "butMoreOptions";
			this.butMoreOptions.Size = new System.Drawing.Size(99, 24);
			this.butMoreOptions.TabIndex = 7;
			this.butMoreOptions.Text = "More Options";
			this.butMoreOptions.Click += new System.EventHandler(this.butMoreOptions_Click);
			// 
			// textAPR
			// 
			this.textAPR.Location = new System.Drawing.Point(142, 78);
			this.textAPR.MaxVal = 100000000D;
			this.textAPR.MinVal = 0D;
			this.textAPR.Name = "textAPR";
			this.textAPR.Size = new System.Drawing.Size(47, 20);
			this.textAPR.TabIndex = 4;
			// 
			// textPeriodPayment
			// 
			this.textPeriodPayment.Location = new System.Drawing.Point(133, 39);
			this.textPeriodPayment.MaxVal = 100000000D;
			this.textPeriodPayment.MinVal = 0.01D;
			this.textPeriodPayment.Name = "textPeriodPayment";
			this.textPeriodPayment.Size = new System.Drawing.Size(85, 20);
			this.textPeriodPayment.TabIndex = 2;
			this.textPeriodPayment.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textPeriodPayment_KeyPress);
			// 
			// textTerm
			// 
			this.textTerm.Location = new System.Drawing.Point(133, 17);
			this.textTerm.MaxVal = 255;
			this.textTerm.MinVal = 0;
			this.textTerm.Name = "textTerm";
			this.textTerm.Size = new System.Drawing.Size(47, 20);
			this.textTerm.TabIndex = 1;
			this.textTerm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textTerm_KeyPress);
			// 
			// textDownPayment
			// 
			this.textDownPayment.Location = new System.Drawing.Point(142, 56);
			this.textDownPayment.MaxVal = 100000000D;
			this.textDownPayment.MinVal = 0D;
			this.textDownPayment.Name = "textDownPayment";
			this.textDownPayment.Size = new System.Drawing.Size(85, 20);
			this.textDownPayment.TabIndex = 3;
			// 
			// textDateFirstPay
			// 
			this.textDateFirstPay.Location = new System.Drawing.Point(142, 34);
			this.textDateFirstPay.Name = "textDateFirstPay";
			this.textDateFirstPay.Size = new System.Drawing.Size(85, 20);
			this.textDateFirstPay.TabIndex = 2;
			// 
			// textAmount
			// 
			this.textAmount.Location = new System.Drawing.Point(142, 13);
			this.textAmount.MaxVal = 100000000D;
			this.textAmount.MinVal = 0.01D;
			this.textAmount.Name = "textAmount";
			this.textAmount.Size = new System.Drawing.Size(85, 20);
			this.textAmount.TabIndex = 1;
			this.textAmount.Validating += new System.ComponentModel.CancelEventHandler(this.textAmount_Validating);
			// 
			// butCreateSched
			// 
			this.butCreateSched.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCreateSched.Autosize = true;
			this.butCreateSched.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCreateSched.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCreateSched.CornerRadius = 4F;
			this.butCreateSched.Location = new System.Drawing.Point(250, 138);
			this.butCreateSched.Name = "butCreateSched";
			this.butCreateSched.Size = new System.Drawing.Size(99, 24);
			this.butCreateSched.TabIndex = 6;
			this.butCreateSched.Text = "Create Schedule";
			this.butCreateSched.Click += new System.EventHandler(this.butCreateSched_Click);
			// 
			// butPrint
			// 
			this.butPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrint.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.butPrint.Autosize = true;
			this.butPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrint.CornerRadius = 4F;
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrintSmall;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(563, 660);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(85, 24);
			this.butPrint.TabIndex = 6;
			this.butPrint.Text = "&Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// FormPayPlan
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(974, 698);
			this.Controls.Add(this.textBalance);
			this.Controls.Add(this.textInterest);
			this.Controls.Add(this.textPayment);
			this.Controls.Add(this.textPrincipal);
			this.Controls.Add(this.labelTotals);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textCompletedAmt);
			this.Controls.Add(this.textPrincPaid);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butClear);
			this.Controls.Add(this.checkIns);
			this.Controls.Add(this.butChangePlan);
			this.Controls.Add(this.textInsPlan);
			this.Controls.Add(this.labelInsPlan);
			this.Controls.Add(this.gridCharges);
			this.Controls.Add(this.textNote);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.textAccumulatedDue);
			this.Controls.Add(this.textAmtPaid);
			this.Controls.Add(this.butGoToPat);
			this.Controls.Add(this.textPatient);
			this.Controls.Add(this.butGoToGuar);
			this.Controls.Add(this.textDate);
			this.Controls.Add(this.butChangeGuar);
			this.Controls.Add(this.textGuarantor);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.labelGuarantor);
			this.Controls.Add(this.textTotalCost);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.butPrint);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormPayPlan";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Payment Plan";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormPayPlan_Closing);
			this.Load += new System.EventHandler(this.FormPayPlan_Load);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormPayPlan_Load(object sender, System.EventArgs e) {
			textPatient.Text=Patients.GetLim(PayPlanCur.PatNum).GetNameLF();
			textGuarantor.Text=Patients.GetLim(PayPlanCur.Guarantor).GetNameLF();
			if(PayPlanCur.NumberOfPayments!=0) {
				textTerm.Text=PayPlanCur.NumberOfPayments.ToString();
			}
			else {
				textPeriodPayment.Text=PayPlanCur.PayAmt.ToString("f");
			}
			textDownPayment.Text=PayPlanCur.DownPayment.ToString("f");
			for(int i=0;i<ProviderC.ListShort.Count;i++) {
				comboProv.Items.Add(ProviderC.ListShort[i].GetLongDesc());
				if(IsNew && ProviderC.ListShort[i].ProvNum==PatCur.PriProv) {//new payment plans default to pri prov
					comboProv.SelectedIndex=i;
				}
				//but if not new, then the provider will be selected in FillCharges().
			}
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				labelClinic.Visible=false;
				comboClinic.Visible=false;
			}
			else {
				comboClinic.Items.Add("none");
				if(IsNew) {
					comboClinic.SelectedIndex=0;//this is for patients with no clinic assigned, an unusual situation.
				}
				else {
					//we don't want to do this.  The -1 indicates to pull clinic from charges on first loop in FillCharges().
				}
				for(int i=0;i<Clinics.List.Length;i++) {
					comboClinic.Items.Add(Clinics.List[i].Description);
					if(IsNew && Clinics.List[i].ClinicNum==PatCur.ClinicNum) {//new payment plans default to pat clinic
						comboClinic.SelectedIndex=i+1;
					}
				}
			}
			textDate.Text=PayPlanCur.PayPlanDate.ToShortDateString();
			if(IsNew) {
				textAmount.Text=TotalAmt.ToString("f");//it won't get filled in FillCharges because there are no charges yet
				//If a plan is created "today" with the customer making their first payment on the spot, they will over pay interest.  
				//If there  is a larger gap than 1 month before the first payment, interest will be under calculated.
				//For now, our temporary solution is to prefill the date of first payment box starting with next months date which is the most accurate for calculating interest.
				textDateFirstPay.Text=DateTime.Now.AddMonths(1).ToShortDateString();
				_listPayPlanCharges=new List<PayPlanCharge>();
			}
			else {
				_listPayPlanCharges=PayPlanCharges.GetForPayPlan(PayPlanCur.PayPlanNum);
			}
			textAPR.Text=PayPlanCur.APR.ToString();
			AmtPaid=PayPlans.GetAmtPaid(PayPlanCur.PayPlanNum);//Only counts amount paid for Patient Payment Plans and not Insurance Payment Plans.  Could be changed in the future
			textAmtPaid.Text=AmtPaid.ToString("f");
			textCompletedAmt.Text=PayPlanCur.CompletedAmt.ToString("f");
			textNote.Text=PayPlanCur.Note;
			if(PayPlanCur.PlanNum==0){
				labelInsPlan.Visible=false;
				textInsPlan.Visible=false;
				butChangePlan.Visible=false;
			}
			else{
				textInsPlan.Text=InsPlans.GetDescript(PayPlanCur.PlanNum,FamCur,InsPlanList,PayPlanCur.InsSubNum,SubList);
				checkIns.Checked=true;
				labelGuarantor.Visible=false;
				textGuarantor.Visible=false;
				butGoToGuar.Visible=false;
				butChangeGuar.Visible=false;
			}
			_arrayAccountColors=DefC.GetList(DefCat.AccountColors);
			FillCharges();
		}

		/// <summary>Called 5 times.  This also fills prov and clinic based on the first charge if not new.</summary>
		private void FillCharges(){
			gridCharges.BeginUpdate();
			gridCharges.Columns.Clear();
			ODGridColumn col;
			//If this column is changed from a date column then the comparer method (ComparePayPlanRows) needs to be updated.
			col=new ODGridColumn(Lan.g("PayPlanAmortization","Date"),65,HorizontalAlignment.Right);//0
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g("PayPlanAmortization","Provider"),50);//1
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g("PayPlanAmortization","Description"),150);//2
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g("PayPlanAmortization","Principal"),60,HorizontalAlignment.Right);//3
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g("PayPlanAmortization","Interest"),60,HorizontalAlignment.Right);//4
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g("PayPlanAmortization","Payment"),60,HorizontalAlignment.Right);//5
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g("PayPlanAmortization","Balance"),60,HorizontalAlignment.Right);//6
			gridCharges.Columns.Add(col);
			col=new ODGridColumn("",107);//7 filler
			gridCharges.Columns.Add(col);
			gridCharges.Rows.Clear();
			List<ODGridRow> listPayPlanRows=new List<ODGridRow>();
			for(int i=0;i<_listPayPlanCharges.Count;i++){//Payplan Charges
				listPayPlanRows.Add(CreateRowForPayPlanCharge(_listPayPlanCharges[i],i+1));
			}
			if(PayPlanCur.PlanNum==0) {//Normal payplan
				List<PaySplit> listPaySplits=new List<PaySplit>();
				DataTable bundledPayments=PaySplits.GetForPayPlan(PayPlanCur.PayPlanNum,listPaySplits);
				for(int i=0;i<listPaySplits.Count;i++) {
					listPayPlanRows.Add(CreateRowForPaySplit(bundledPayments.Rows[i],listPaySplits[i]));
				}
			}
			else {//Insurance payplan
				DataTable bundledClaimProcs=ClaimProcs.GetBundlesForPayPlan(PayPlanCur.PayPlanNum);
				for(int i=0;i<bundledClaimProcs.Rows.Count;i++) {
					listPayPlanRows.Add(CreateRowForClaimProcs(bundledClaimProcs.Rows[i]));
				}
			}
			listPayPlanRows.Sort(ComparePayPlanRows);
			for(int i=0;i<listPayPlanRows.Count;i++) {
				gridCharges.Rows.Add(listPayPlanRows[i]);
			}
			TotPrinc=0;
			TotInt=0;
			for(int i=0;i<_listPayPlanCharges.Count;i++){
				TotPrinc+=_listPayPlanCharges[i].Principal;
				TotInt+=_listPayPlanCharges[i].Interest;
			}
			TotPrincInt=TotPrinc+TotInt;
			if(_listPayPlanCharges.Count==0) {
				//don't damage what's already present in textAmount.Text
			}
			else{
				textAmount.Text=TotPrinc.ToString("f");
			}
			textTotalCost.Text=TotPrincInt.ToString("f");
			if(_listPayPlanCharges.Count>0){
				textDateFirstPay.Text=_listPayPlanCharges[0].ChargeDate.ToShortDateString();
			}
			else{
				//don't damage what's already in textDateFirstPay.Text
			}
			gridCharges.EndUpdate();
			double balanceAmt=0;
			TotPrinc=0;
			TotInt=0;
			double TotPay=0;
			int totalsRowIndex=0;
			for(int i=0;i<gridCharges.Rows.Count;i++){//Filling row cells with balance information.
				if(gridCharges.Rows[i].Cells[3].Text!="") {//Principal
					TotPrinc+=PIn.Double(gridCharges.Rows[i].Cells[3].Text);
					balanceAmt+=PIn.Double(gridCharges.Rows[i].Cells[3].Text);
				}
				if(gridCharges.Rows[i].Cells[4].Text!="") {//Interest
					TotInt+=PIn.Double(gridCharges.Rows[i].Cells[4].Text);
					balanceAmt+=PIn.Double(gridCharges.Rows[i].Cells[4].Text);
				}
				else if(gridCharges.Rows[i].Cells[5].Text!="") {//Payment
					TotPay+=PIn.Double(gridCharges.Rows[i].Cells[5].Text);
					balanceAmt-=PIn.Double(gridCharges.Rows[i].Cells[5].Text);
				}
				gridCharges.Rows[i].Cells[6].Text=balanceAmt.ToString("f");
				if(DateTime.Parse(listPayPlanRows[i].Cells[0].Text)<=DateTime.Today) {
					textPrincipal.Text=TotPrinc.ToString("f");
					textInterest.Text=TotInt.ToString("f");
					textPayment.Text=TotPay.ToString("f");
					textBalance.Text=balanceAmt.ToString("f");
					totalsRowIndex=i;
				}
			}
			gridCharges.Rows[totalsRowIndex].ColorLborder=Color.Black;
			gridCharges.Rows[totalsRowIndex].Cells[6].Bold=YN.Yes;
			textAccumulatedDue.Text=PayPlans.GetAccumDue(PayPlanCur.PayPlanNum,_listPayPlanCharges).ToString("f");
			textPrincPaid.Text=PayPlans.GetPrincPaid(AmtPaid,PayPlanCur.PayPlanNum,_listPayPlanCharges).ToString("f");
			if(!IsNew && _listPayPlanCharges.Count>0) {
				if(comboProv.SelectedIndex==-1) {//This avoids resetting the combo every time FillCharges is run.
					comboProv.SelectedIndex=Providers.GetIndex(_listPayPlanCharges[0].ProvNum);//could still be -1
				}
				if(!PrefC.GetBool(PrefName.EasyNoClinics) && comboClinic.SelectedIndex==-1) {
					if(_listPayPlanCharges[0].ClinicNum==0){
						comboClinic.SelectedIndex=0;
					}
					else{
						comboClinic.SelectedIndex=Clinics.GetIndex(_listPayPlanCharges[0].ClinicNum)+1;
					}
				}
			}
		}

		private ODGridRow CreateRowForPayPlanCharge(PayPlanCharge payPlanCharge,int payPlanChargeOrdinal) {
			string descript="#"+payPlanChargeOrdinal;
			if(payPlanCharge.Note!="") {
				descript+=" "+payPlanCharge.Note;
			}
			ODGridRow row=new ODGridRow();//Charge row
			row.Cells.Add(payPlanCharge.ChargeDate.ToShortDateString());//0 Date
			row.Cells.Add(Providers.GetAbbr(payPlanCharge.ProvNum));//1 Prov Abbr
			row.Cells.Add(descript);//2 Descript
			row.Cells.Add((payPlanCharge.Principal).ToString("n"));//3 Principal
			row.Cells.Add(payPlanCharge.Interest.ToString("n"));//4 Interest
			row.Cells.Add("");//5 Payment
			row.Cells.Add("");//6 Balance (filled later)
			row.Cells.Add("");//7 Empty filler column
			row.Tag=payPlanCharge;
			return row;
		}

		private ODGridRow CreateRowForPaySplit(DataRow rowBundlePayment,PaySplit paySplit) {
			string descript=DefC.GetName(DefCat.PaymentTypes,PIn.Long(rowBundlePayment["PayType"].ToString()));
			if(rowBundlePayment["CheckNum"].ToString()!="") {
				descript+=" #"+rowBundlePayment["CheckNum"].ToString();
			}
			descript+=" "+paySplit.SplitAmt.ToString("c");//Not sure if we really want to convert from string to double then back to string.. maybe a better way to format this?
			if(Convert.ToDouble(rowBundlePayment["PayAmt"].ToString())!=paySplit.SplitAmt) { 
				descript+=Lans.g(this,"(split)");
			}
			ODGridRow row=new ODGridRow();
			row.Cells.Add(paySplit.DatePay.ToShortDateString());//0 Date
			row.Cells.Add(Providers.GetAbbr(Convert.ToInt32(rowBundlePayment["ProvNum"])));//1 Prov Abbr
			row.Cells.Add(descript);//2 Descript
			row.Cells.Add("");//3 Principal
			row.Cells.Add("");//4 Interest
			row.Cells.Add(paySplit.SplitAmt.ToString("n"));//5 Payment
			row.Cells.Add("");//6 Balance (filled later)
			row.Cells.Add("");//7 Empty filler column
			row.Tag=paySplit;
			row.ColorText=_arrayAccountColors[3].ItemColor;//Setup | Definitions | Account Colors | Payment;
			return row;
		}

		private ODGridRow CreateRowForClaimProcs(DataRow rowBundleClaimProc) {//Either a claimpayment or a bundle of claimprocs with no claimpayment that were on the same date.
			string descript=DefC.GetName(DefCat.InsurancePaymentType,PIn.Long(rowBundleClaimProc["PayType"].ToString()));
			if(rowBundleClaimProc["CheckNum"].ToString()!=""){
				descript+=" #"+rowBundleClaimProc["CheckNum"];
			}
			if(PIn.Long(rowBundleClaimProc["ClaimPaymentNum"].ToString())==0) {
				descript+="No Finalized Payment";
			}
			else {
				double checkAmt=PIn.Double(rowBundleClaimProc["CheckAmt"].ToString());
				descript+=" "+checkAmt.ToString("c");
				double insPayAmt=PIn.Double(rowBundleClaimProc["InsPayAmt"].ToString());
				if(checkAmt!=insPayAmt){
					descript+=" "+Lans.g(this,"(split)");
				}
			}
			ODGridRow row=new ODGridRow();
			row.Cells.Add(PIn.DateT(rowBundleClaimProc["DateCP"].ToString()).ToShortDateString());//0 Date
			row.Cells.Add(Providers.GetLName(Convert.ToInt32(rowBundleClaimProc["ProvNum"])));//1 Prov Abbr
			row.Cells.Add(descript);//2 Descript
			row.Cells.Add("");//3 Principal
			row.Cells.Add("");//4 Interest
			row.Cells.Add(PIn.Double(rowBundleClaimProc["InsPayAmt"].ToString()).ToString("n"));//5 Payment
			row.Cells.Add("");//6 Balance (filled later)
			row.Cells.Add("");//7 Empty filler column
			row.Tag=rowBundleClaimProc;
			row.ColorText=_arrayAccountColors[7].ItemColor;//Setup | Definitions | Account Colors | Insurance Payment
			return row;
		}

		private void butGoToPat_Click(object sender, System.EventArgs e) {
			if(!SaveData()){
				return;
			}
			GotoPatNum=PayPlanCur.PatNum;
			DialogResult=DialogResult.OK;
		}

		private void butGoTo_Click(object sender, System.EventArgs e) {
			if(!SaveData()){
				return;
			}
			GotoPatNum=PayPlanCur.Guarantor;
			DialogResult=DialogResult.OK;
		}

		private void butChangeGuar_Click(object sender, System.EventArgs e) {
			if(PayPlans.GetAmtPaid(PayPlanCur.PayPlanNum)!=0){
				MsgBox.Show(this,"Not allowed to change the guarantor because payments are attached.");
				return;
			}
			if(gridCharges.Rows.Count>0){
				MsgBox.Show(this,"Not allowed to change the guarantor without first clearing the amortization schedule.");
				return;
			}
			FormPatientSelect FormPS=new FormPatientSelect();
			FormPS.SelectionModeOnly=true;
			FormPS.ShowDialog();
			if(FormPS.DialogResult!=DialogResult.OK){
				return;
			}
			PayPlanCur.Guarantor=FormPS.SelectedPatNum;
			textGuarantor.Text=Patients.GetLim(PayPlanCur.Guarantor).GetNameLF();
		}

		private void checkIns_Click(object sender, System.EventArgs e) {
			if(PayPlans.GetAmtPaid(PayPlanCur.PayPlanNum)!=0){
				MsgBox.Show(this,"Not allowed because payments are attached.");
				checkIns.Checked=!checkIns.Checked;
				return;
			}
			if(gridCharges.Rows.Count>0){
				MsgBox.Show(this,"Not allowed without first clearing the amortization schedule.");
				checkIns.Checked=!checkIns.Checked;
				return;
			}
			if(checkIns.Checked){
				FormInsPlanSelect FormI=new FormInsPlanSelect(PayPlanCur.PatNum);
				FormI.ShowDialog();
				if(FormI.DialogResult==DialogResult.Cancel){
					checkIns.Checked=false;
					return;
				}
				PayPlanCur.PlanNum=FormI.SelectedPlan.PlanNum;
				PayPlanCur.InsSubNum=FormI.SelectedSub.InsSubNum;
				PayPlanCur.Guarantor=PayPlanCur.PatNum;
				textInsPlan.Text=InsPlans.GetDescript(PayPlanCur.PlanNum,FamCur,InsPlanList,PayPlanCur.InsSubNum,SubList);
				labelGuarantor.Visible=false;
				textGuarantor.Visible=false;
				butGoToGuar.Visible=false;
				butChangeGuar.Visible=false;
				labelInsPlan.Visible=true;
				textInsPlan.Visible=true;
				butChangePlan.Visible=true;
			}
			else{//not insurance
				PayPlanCur.Guarantor=PayPlanCur.PatNum;
				textGuarantor.Text=Patients.GetLim(PayPlanCur.Guarantor).GetNameLF();
				PayPlanCur.PlanNum=0;
				PayPlanCur.InsSubNum=0;
				labelGuarantor.Visible=true;
				textGuarantor.Visible=true;
				butGoToGuar.Visible=true;
				butChangeGuar.Visible=true;
				labelInsPlan.Visible=false;
				textInsPlan.Visible=false;
				butChangePlan.Visible=false;
			}
		}

		private void textAmount_Validating(object sender,CancelEventArgs e) {
			if(textCompletedAmt.Text==""){
				return;
			}
			if(PIn.Double(textCompletedAmt.Text)==PIn.Double(textAmount.Text)){
				return;
			}
			if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Change Tx Completed Amt to match?")){
				textCompletedAmt.Text=textAmount.Text;
			}
		}

		private void butChangePlan_Click(object sender, System.EventArgs e) {
			FormInsPlanSelect FormI=new FormInsPlanSelect(PayPlanCur.PatNum);
			FormI.ShowDialog();
			if(FormI.DialogResult==DialogResult.Cancel){
				return;
			}
			PayPlanCur.PlanNum=FormI.SelectedPlan.PlanNum;
			PayPlanCur.InsSubNum=FormI.SelectedSub.InsSubNum;
			textInsPlan.Text=InsPlans.GetDescript(PayPlanCur.PlanNum,Patients.GetFamily(PayPlanCur.PatNum),new List <InsPlan> (),PayPlanCur.InsSubNum,new List<InsSub>());
		}

		private void textTerm_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) {
			textPeriodPayment.Text="";
		}

		private void textPeriodPayment_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) {
			textTerm.Text="";
		}

		private void butMoreOptions_Click(object sender,EventArgs e) {
			FormPayPlanOpts.ShowDialog();
		}

		private void butCreateSched_Click(object sender, System.EventArgs e) {
			//this is also where the terms get saved
			if(  textDate.errorProvider1.GetError(textDate)!=""
				|| textAmount.errorProvider1.GetError(textAmount)!=""
				|| textDateFirstPay.errorProvider1.GetError(textDateFirstPay)!=""
				|| textDownPayment.errorProvider1.GetError(textDownPayment)!=""
				|| textAPR.errorProvider1.GetError(textAPR)!=""
				|| textTerm.errorProvider1.GetError(textTerm)!=""
				|| textPeriodPayment.errorProvider1.GetError(textPeriodPayment)!=""
				|| textCompletedAmt.errorProvider1.GetError(textCompletedAmt)!=""
				){
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return;
			}
			if(textAmount.Text=="" || PIn.Double(textAmount.Text)==0){
				MsgBox.Show(this,"Please enter an amount first.");
				return;
			}
			if(textDateFirstPay.Text==""){
				textDateFirstPay.Text=DateTime.Today.ToShortDateString();
			}
			if(textDownPayment.Text==""){
				textDownPayment.Text="0";
			}
			if(textAPR.Text==""){
				textAPR.Text="0";
			}
			if(textTerm.Text=="" && textPeriodPayment.Text==""){
				MsgBox.Show(this,"Please enter a term or payment amount first.");
				return;
			}
			if(textTerm.Text=="" && PIn.Double(textPeriodPayment.Text)==0){
				MsgBox.Show(this,"Payment cannot be 0.");
				return;
			}
			if(textTerm.Text!="" && textPeriodPayment.Text!="") {
				MsgBox.Show(this,"Please choose either Number of Payments or Payment Amt.");
				return;
			}
			if(textPeriodPayment.Text=="" && PIn.Long(textTerm.Text)<1){
				MsgBox.Show(this,"Term cannot be less than 1.");
				return;
			}
			if(PIn.Double(textAmount.Text)-PIn.Double(textDownPayment.Text)<0) {
				MsgBox.Show(this,"Down paymnent must be less than or equal to total amount.");
				return;
			}
			if(gridCharges.Rows.Count>0){
				if(!MsgBox.Show(this,true,"Replace existing amortization schedule?")){
					return;
				}
				_listPayPlanCharges.Clear();
			}
			PayPlanCharge ppCharge;
			//down payment
			double downpayment=PIn.Double(textDownPayment.Text);
			if(downpayment!=0){
				ppCharge=new PayPlanCharge();
				ppCharge.PayPlanNum=PayPlanCur.PayPlanNum;
				ppCharge.Guarantor=PayPlanCur.Guarantor;
				ppCharge.PatNum=PayPlanCur.PatNum;
				ppCharge.ChargeDate=DateTimeOD.Today;
				ppCharge.Interest=0;
				ppCharge.Principal=downpayment;
				ppCharge.Note=Lan.g(this,"Downpayment");
				ppCharge.ProvNum=PatCur.PriProv;//will be changed at the end.
				ppCharge.ClinicNum=PatCur.ClinicNum;//will be changed at the end.
				_listPayPlanCharges.Add(ppCharge);
			}
			double principal=PIn.Double(textAmount.Text)-PIn.Double(textDownPayment.Text);//Always >= 0 due to validation.
			PayPlanCur.DownPayment=PIn.Double(textDownPayment.Text);
			double APR=PIn.Double(textAPR.Text);
			PayPlanCur.APR=APR;
			double periodRate;
			decimal periodPayment;
			if(APR==0){
				periodRate=0;
			}
			else{
				if(FormPayPlanOpts.radioWeekly.Checked){
					periodRate=APR/100/52;
					PayPlanCur.PaySchedule=PaymentSchedule.Weekly;
				}
				else if(FormPayPlanOpts.radioEveryOtherWeek.Checked){
					periodRate=APR/100/26;
					PayPlanCur.PaySchedule=PaymentSchedule.BiWeekly;
				}
				else if(FormPayPlanOpts.radioOrdinalWeekday.Checked){
					periodRate=APR/100/12;
					PayPlanCur.PaySchedule=PaymentSchedule.MonthlyDayOfWeek;
				}
				else if(FormPayPlanOpts.radioMonthly.Checked){
					periodRate=APR/100/12;
					PayPlanCur.PaySchedule=PaymentSchedule.Monthly;
				}
				else{//quarterly
					periodRate=APR/100/4;
					PayPlanCur.PaySchedule=PaymentSchedule.Quarterly;
				}
			}
			int roundDec=CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits;
			int term=0;
			if(textTerm.Text!=""){//Use term to determine period payment
				term=PIn.Int(textTerm.Text);
				double periodExactAmt=0;
				if(APR==0){
					periodExactAmt=principal/term;
				}
				else{
					periodExactAmt=principal*periodRate/(1-Math.Pow(1+periodRate,-term));
				}
				//Round up to the nearest penny (or international equivalent).  This causes the principal on the last payment to be less than or equal to the other principal amounts.
				periodPayment=(decimal)(Math.Ceiling(periodExactAmt*Math.Pow(10,roundDec))/Math.Pow(10,roundDec));
				PayPlanCur.NumberOfPayments=term;
			}
			else{//Use period payment supplied
				periodPayment=PIn.Decimal(textPeriodPayment.Text);
				PayPlanCur.PayAmt=(double)periodPayment;
			}
			decimal principalDecrementing=(decimal)principal;//The principal which will be decreased to zero in the loop.  Always starts >= 0, due to validation.
			DateTime firstDate=PIn.Date(textDateFirstPay.Text);
			int countCharges=0;
			while(principalDecrementing>0 && countCharges<2000){//the 2000 limit prevents infinite loop
				ppCharge=new PayPlanCharge();
				ppCharge.PayPlanNum=PayPlanCur.PayPlanNum;
				ppCharge.Guarantor=PayPlanCur.Guarantor;
				ppCharge.PatNum=PayPlanCur.PatNum;
				if(FormPayPlanOpts.radioWeekly.Checked) {
					ppCharge.ChargeDate=firstDate.AddDays(7*countCharges);
				}
				else if(FormPayPlanOpts.radioEveryOtherWeek.Checked) {
					ppCharge.ChargeDate=firstDate.AddDays(14*countCharges);
				}
				else if(FormPayPlanOpts.radioOrdinalWeekday.Checked) {//First/second/etc Mon/Tue/etc of month
					DateTime roughMonth=firstDate.AddMonths(1*countCharges);//this just gets us into the correct month and year
					DayOfWeek dayOfWeekFirstDate=firstDate.DayOfWeek;
					//find the starting point for the given month: the first day that matches day of week
					DayOfWeek dayOfWeekFirstMonth=(new DateTime(roughMonth.Year,roughMonth.Month,1)).DayOfWeek;
					if(dayOfWeekFirstMonth==dayOfWeekFirstDate) {//1st is the proper day of the week
						ppCharge.ChargeDate=new DateTime(roughMonth.Year,roughMonth.Month,1);
					}
					else if(dayOfWeekFirstMonth<dayOfWeekFirstDate) {//Example, 1st is a Tues (2), but we need to start on a Thursday (4)
						ppCharge.ChargeDate=new DateTime(roughMonth.Year,roughMonth.Month,dayOfWeekFirstDate-dayOfWeekFirstMonth+1);//4-2+1=3.  The 3rd is a Thursday
					}
					else {//Example, 1st is a Thursday (4), but we need to start on a Monday (1) 
						ppCharge.ChargeDate=new DateTime(roughMonth.Year,roughMonth.Month,7-(dayOfWeekFirstMonth-dayOfWeekFirstDate)+1);//7-(4-1)+1=5.  The 5th is a Monday
					}
					int ordinalOfMonth=GetOrdinalOfMonth(firstDate);//for example 3 if it's supposed to be the 3rd Friday of each month
					ppCharge.ChargeDate=ppCharge.ChargeDate.AddDays(7*(ordinalOfMonth-1));//to get to the 3rd Friday, and starting from the 1st Friday, we add 2 weeks.
				}
				else if(FormPayPlanOpts.radioMonthly.Checked) {
					ppCharge.ChargeDate=firstDate.AddMonths(1*countCharges);
				}
				else {//quarterly
					ppCharge.ChargeDate=firstDate.AddMonths(3*countCharges);
				}
				ppCharge.Interest=Math.Round(((double)principalDecrementing*periodRate),roundDec);//2 decimals
				ppCharge.Principal=(double)periodPayment-ppCharge.Interest;
				ppCharge.ProvNum=PatCur.PriProv;
				if(term>0 && countCharges==(term-1)) {//Using # payments method and this is the last payment.
					//The purpose of this code block is to fix any rounding issues.  Corrects principal when off by a few pennies.  Principal will decrease slightly and interest will increase slightly to keep payment amounts consistent.
					ppCharge.Principal=(double)principalDecrementing;//All remaining principal.  Causes loop to exit.  This is where the rounding error is eliminated.
					if(periodRate!=0) {//Interest amount on last entry must stay zero for payment plans with zero APR. When APR is zero, the interest amount is set to zero above, and the last payment amount might be less than the other payment amounts.
						ppCharge.Interest=((double)periodPayment)-ppCharge.Principal;//Force the payment amount to match the rest of the period payments.
					}
				}
				else if(term==0 && principalDecrementing+((decimal)ppCharge.Interest) <= periodPayment) {//Payment amount method, last payment.
					ppCharge.Principal=(double)principalDecrementing;//All remaining principal.  Causes loop to exit.
					//Interest was calculated above.
				}
				principalDecrementing-=(decimal)ppCharge.Principal;
				//If somehow principalDecrementing was slightly negative right here due to rounding errors, then at worst the last charge amount would wrong by a few pennies and the loop would immediately exit.
				_listPayPlanCharges.Add(ppCharge);
				countCharges++;
			}
			FillCharges();
			textNote.Text+=DateTime.Today.ToShortDateString()
				+" - Date of Agreement: "+textDate.Text
				+", Total Amount: "+textAmount.Text
				+", APR: "+textAPR.Text
				+", Total Cost of Loan: "+textTotalCost.Text;
		}

		///<summary>For example, date is the 3rd Friday of the month, then this returns 3.</summary>
		private int GetOrdinalOfMonth(DateTime date) {
			if(date.AddDays(-28).Month==date.Month) {
				return 4;//treat a 5 like a 4
			}
			else if(date.AddDays(-21).Month==date.Month) {//4
				return 4;
			}
			else if(date.AddDays(-14).Month==date.Month) {
				return 3;
			}
			if(date.AddDays(-7).Month==date.Month) {
				return 2;
			}
			return 1;
		}

		private void gridCharges_CellDoubleClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			if(gridCharges.Rows[e.Row].Tag==null) {//Prevent double clicking on the "Current Totals" row
				return;
			}
			if(gridCharges.Rows[e.Row].Tag.GetType()==typeof(PayPlanCharge)){
				PayPlanCharge payPlanCharge=(PayPlanCharge)gridCharges.Rows[e.Row].Tag;
				FormPayPlanChargeEdit FormP=new FormPayPlanChargeEdit(payPlanCharge);//This automatically takes care of our in-memory list because the Tag is referencing our list of objects.
				FormP.ShowDialog();
				if(FormP.DialogResult==DialogResult.Cancel){
					return;
				}
				if(FormP.PayPlanChargeCur==null) {//The user deleted the payplancharge.
					_listPayPlanCharges.Remove(payPlanCharge);//We know the payPlanCharge object is inside _listPayPlanCharges.
					gridCharges.BeginUpdate();
					gridCharges.Rows.RemoveAt(e.Row);
					gridCharges.EndUpdate();
					return;
				}
			}
			else if(gridCharges.Rows[e.Row].Tag.GetType()==typeof(PaySplit)){
				PaySplit paySplit=(PaySplit)gridCharges.Rows[e.Row].Tag;
				FormPayment FormPayment2=new FormPayment(PatCur,FamCur,Payments.GetPayment(paySplit.PayNum));//FormPayment may inserts and/or update the paysplits. 
				FormPayment2.IsNew=false;
				FormPayment2.ShowDialog();
				if(FormPayment2.DialogResult==DialogResult.Cancel){
					return;
				}
			}
			else if(gridCharges.Rows[e.Row].Tag.GetType()==typeof(DataRow)){//Claim payment or bundle.
				DataRow bundledClaimProc=(DataRow)gridCharges.Rows[e.Row].Tag;
				Claim claimCur=Claims.GetClaim(PIn.Long(bundledClaimProc["ClaimNum"].ToString()));
				FormClaimEdit FormCE=new FormClaimEdit(claimCur,PatCur,FamCur);//FormClaimEdit inserts and/or updates the claim and/or claimprocs, which could potentially change the bundle.
				FormCE.IsNew=false;
				FormCE.ShowDialog();
				//Cancel from FormClaimEdit does not cancel payment edits, fill grid every time
			}
			FillCharges();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			PayPlanCharge ppCharge=new PayPlanCharge();
			ppCharge.PayPlanNum=PayPlanCur.PayPlanNum;
			ppCharge.Guarantor=PayPlanCur.Guarantor;
			ppCharge.ChargeDate=DateTime.Today;
			ppCharge.ProvNum=PatCur.PriProv;//will be changed at the end.
			ppCharge.ClinicNum=PatCur.ClinicNum;//will be changed at the end.
			FormPayPlanChargeEdit FormP=new FormPayPlanChargeEdit(ppCharge);
			FormP.IsNew=true;
			FormP.ShowDialog();
			if(FormP.DialogResult==DialogResult.Cancel){
				return;
			}
			_listPayPlanCharges.Add(ppCharge);
			FillCharges();
		}

		private void butClear_Click(object sender, System.EventArgs e) {
			if(!MsgBox.Show(this,true,"Clear all charges from amortization schedule?")){
				return;
			}
			_listPayPlanCharges.Clear();
			gridCharges.BeginUpdate();
			gridCharges.Rows.Clear();
			gridCharges.EndUpdate();
		}

		private void butPrint_Click(object sender, System.EventArgs e) {
			if(!SaveData()){
				return;
			}
			Font font=new Font("Tahoma",9);
			Font fontBold=new Font("Tahoma",9,FontStyle.Bold);
			Font fontTitle=new Font("Tahoma",17,FontStyle.Bold);
			Font fontSubTitle=new Font("Tahoma",10,FontStyle.Bold);
			ReportComplex report=new ReportComplex(false,false);
			report.AddTitle("Title",Lan.g(this,"Payment Plan Terms"),fontTitle);
			report.AddSubTitle("PracTitle",PrefC.GetString(PrefName.PracticeTitle),fontSubTitle);
			report.AddSubTitle("Date SubTitle",DateTime.Today.ToShortDateString(),fontSubTitle);
			string sectName="Report Header";
			Section section=report.Sections["Report Header"];
			//int sectIndex=report.Sections.GetIndexOfKind(AreaSectionKind.ReportHeader);
			Size size=new Size(300,20);//big enough for any text
			ContentAlignment alignL=ContentAlignment.MiddleLeft;
			ContentAlignment alignR=ContentAlignment.MiddleRight;
			int yPos=140;
			int space=30;
			int x1=175;
			int x2=275;
			report.ReportObjects.Add(new ReportObject
				("Patient Title",sectName,new Point(x1,yPos),size,"Patient",font,alignL));
			report.ReportObjects.Add(new ReportObject
				("Patient Detail",sectName,new Point(x2,yPos),size,textPatient.Text,font,alignR));
			yPos+=space;
			report.ReportObjects.Add(new ReportObject
				("Guarantor Title",sectName,new Point(x1,yPos),size,"Guarantor",font,alignL));
			report.ReportObjects.Add(new ReportObject
				("Guarantor Detail",sectName,new Point(x2,yPos),size,textGuarantor.Text,font,alignR));
			yPos+=space;
			report.ReportObjects.Add(new ReportObject
				("Date of Agreement Title",sectName,new Point(x1,yPos),size,"Date of Agreement",font,alignL));
			report.ReportObjects.Add(new ReportObject
				("Date of Agreement Detail",sectName,new Point(x2,yPos),size,PayPlanCur.PayPlanDate.ToString("d"),font,alignR));
			yPos+=space;
			report.ReportObjects.Add(new ReportObject
				("Principal Title",sectName,new Point(x1,yPos),size,"Principal",font,alignL));
			report.ReportObjects.Add(new ReportObject
				("Principal Detail",sectName,new Point(x2,yPos),size,TotPrinc.ToString("n"),font,alignR));
			yPos+=space;
			report.ReportObjects.Add(new ReportObject
				("Annual Percentage Rate Title",sectName,new Point(x1,yPos),size,"Annual Percentage Rate",font,alignL));
			report.ReportObjects.Add(new ReportObject
				("Annual Percentage Rate Detail",sectName,new Point(x2,yPos),size,PayPlanCur.APR.ToString("f1"),font,alignR));
			yPos+=space;
			report.ReportObjects.Add(new ReportObject
				("Total Finance Charges Title",sectName,new Point(x1,yPos),size,"Total Finance Charges",font,alignL));
			report.ReportObjects.Add(new ReportObject
				("Total Finance Charges Detail",sectName,new Point(x2,yPos),size,TotInt.ToString("n"),font,alignR));
			yPos+=space;
			report.ReportObjects.Add(new ReportObject
				("Total Cost of Loan Title",sectName,new Point(x1,yPos),size,"Total Cost of Loan",font,alignL));
			report.ReportObjects.Add(new ReportObject
				("Total Cost of Loan Detail",sectName,new Point(x2,yPos),size,TotPrincInt.ToString("n"),font,alignR));
			yPos+=space;
			section.Height=yPos+30;
			DataTable tbl=new DataTable();
			tbl.Columns.Add("date");
			tbl.Columns.Add("prov");
			tbl.Columns.Add("description");
			tbl.Columns.Add("principal");
			tbl.Columns.Add("interest");
			tbl.Columns.Add("payment");
			tbl.Columns.Add("balance");
			DataRow row;
			for(int i=0;i<gridCharges.Rows.Count;i++) {
				row=tbl.NewRow();
				row["date"]=gridCharges.Rows[i].Cells[0].Text;
				row["prov"]=gridCharges.Rows[i].Cells[1].Text;
				row["description"]=gridCharges.Rows[i].Cells[2].Text;
				row["principal"]=gridCharges.Rows[i].Cells[3].Text;
				row["interest"]=gridCharges.Rows[i].Cells[4].Text;
				row["payment"]=gridCharges.Rows[i].Cells[5].Text;
				row["balance"]=gridCharges.Rows[i].Cells[6].Text;
				tbl.Rows.Add(row);
			}
			QueryObject query=report.AddQuery(tbl,"","",SplitByKind.None,1,true);
			query.AddColumn("ChargeDate",80,FieldValueType.Date,font);
			query.GetColumnHeader("ChargeDate").StaticText="Date";
			query.AddColumn("Provider",50,FieldValueType.String,font);
			query.AddColumn("Description",100,FieldValueType.String,font);
			query.AddColumn("Principal",50,FieldValueType.Number,font);
			query.AddColumn("Interest",50,FieldValueType.Number,font);
			query.AddColumn("Payment",50,FieldValueType.Number,font);
			query.AddColumn("Balance",60,FieldValueType.String,font);
			query.GetColumnHeader("Balance").ContentAlignment=ContentAlignment.MiddleRight;
			query.GetColumnDetail("Balance").ContentAlignment=ContentAlignment.MiddleRight;
			report.ReportObjects.Add(new ReportObject
			("Signature","Report Footer",new Point(x1,70),size,"Signature of Guarantor:",font,alignL));
			if(!report.SubmitQueries()) {
				return;
			}
			FormReportComplex FormR=new FormReportComplex(report);
			FormR.ShowDialog();
		}

		private void pd2_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e) {
			int xPos=15;//starting pos
			int yPos=(int)27.5;//starting pos
			e.Graphics.DrawString("Payment Plan Truth in Lending Statement"
				,new Font("Arial",8),Brushes.Black,(float)xPos,(float)yPos);
      //e.Graphics.DrawImage(imageTemp,xPos,yPos);
		}

		///<summary></summary>
		private bool SaveData(){
			if(textDate.errorProvider1.GetError(textDate)!=""
				|| textCompletedAmt.errorProvider1.GetError(textCompletedAmt)!="")
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return false;
			}
			if(gridCharges.Rows.Count==0) {
				MsgBox.Show(this,"An amortization schedule must be created first.");
				return false;
			}
			if(comboProv.SelectedIndex==-1) {
				MsgBox.Show(this,"A provider must be selected first.");
				return false;
			}
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				if(comboClinic.SelectedIndex==-1) {
					MsgBox.Show(this,"A clinic must be selected first.");
					return false;
				}
			}
			if(textAPR.Text==""){
				textAPR.Text="0";
			}
			//PatNum not editable.
			//Guarantor set already
			PayPlanCur.PayPlanDate=PIn.Date(textDate.Text);
			//The following variables were handled when the amortization schedule was created.
			//PayPlanCur.APR
			//PayPlanCur.PaySchedule
			//PayPlanCur.NumberOfPayments
			//PayPlanCur.PayAmt
			//PayPlanCur.DownPayment
			PayPlanCur.Note=textNote.Text;
			PayPlanCur.CompletedAmt=PIn.Double(textCompletedAmt.Text);
			//PlanNum set already
			PayPlans.Update(PayPlanCur);//always saved to db before opening this form
			long provNum=ProviderC.ListShort[comboProv.SelectedIndex].ProvNum;//already verified that there's a provider selected
			long clinicNum=0;
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				if(comboClinic.SelectedIndex==0) {
					clinicNum=0;
				}
				else {
					clinicNum=Clinics.List[comboClinic.SelectedIndex-1].ClinicNum;
				}
			}
			for(int i=0;i<_listPayPlanCharges.Count;i++) {
				_listPayPlanCharges[i].ClinicNum=clinicNum;
				_listPayPlanCharges[i].ProvNum=provNum;
			}
			PayPlanCharges.Sync(_listPayPlanCharges,PayPlanCur.PayPlanNum);
			return true;
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			if(!MsgBox.Show(this,true,"Delete payment plan?")){
				return;
			}
			//later improvement if needed: possibly prevent deletion of some charges like older ones.
			try{
				PayPlans.Delete(PayPlanCur);
			}
			catch(ApplicationException ex){
				MessageBox.Show(ex.Message);
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender, System.EventArgs e){
			if(PIn.Double(textCompletedAmt.Text)!=PIn.Double(textAmount.Text)){
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Tx Completed Amt and Total Amount do not match, continue?")) {
					return;
				}
			}
			if(!SaveData()){
				return;
			}
      DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		///<summary>Sorts by the first column, as a date column, in ascending order.</summary>
		private int ComparePayPlanRows(ODGridRow x,ODGridRow y) {
			DateTime dateTimeX=DateTime.Parse(x.Cells[0].Text);
			DateTime dateTimeY=DateTime.Parse(y.Cells[0].Text);
			if(dateTimeX<dateTimeY) {
				return -1;
			}
			else if(dateTimeX>dateTimeY) {
				return 1;
			}
			//dateTimeX==dateTimeY
			//Show charges before Payment on the same date.
			if(x.Tag.GetType()==typeof(PayPlanCharge)) {//x is charge (Type.Equals doesn't seem to work in sorters for some reason)
				if(y.Tag.GetType()==typeof(PaySplit) || y.Tag.GetType()==typeof(DataRow)) {//y is credit, x goes first
					return -1;
				}
				//x and y are both charges (Not likely, they shouldn't have same dates)
			}
			else {//x is credit
				if(y.Tag.GetType()==typeof(PayPlanCharge)) {//y is charge
					return 1;
				}
				//x and y are both Payments
			}
			return x.Cells[2].Text.CompareTo(y.Cells[2].Text);//Sort by description.  This orders the payment plan charges which are on the same date by their charge number.  Might order payments by check number as well.
		}

		private void FormPayPlan_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(DialogResult==DialogResult.OK){
				return;
			}
			if(IsNew){
				try{
					PayPlans.Delete(PayPlanCur);
				}
				catch(Exception ex){
					MessageBox.Show(ex.Message);
					e.Cancel=true;
					return;
				}
			}
		}

		

		
		

		

		

		

		

		

		

		

		

		

		

		
		

		

		

		

		

		
	

		

		


	}
}





















