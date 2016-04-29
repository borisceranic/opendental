/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;
using System.Linq;
using CodeBase;

namespace OpenDental{
	///<summary></summary>
	public class FormAdjust : ODForm {
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label2;
		private System.ComponentModel.Container components = null;// Required designer variable.
		///<summary></summary>
		public bool IsNew;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.Label label3;
		private OpenDental.UI.Button butDelete;
		private OpenDental.ValidDouble textAmount;
		private System.Windows.Forms.ListBox listTypePos;
		private System.Windows.Forms.ListBox listTypeNeg;
		private ArrayList PosIndex=new ArrayList();
		private OpenDental.ODtextBox textNote;
		private ArrayList NegIndex=new ArrayList();
		private Patient PatCur;
		private OpenDental.ValidDate textProcDate;
		private System.Windows.Forms.Label label7;
		private OpenDental.ValidDate textAdjDate;
		private Adjustment AdjustmentCur;
		private OpenDental.ValidDate textDateEntry;
		private System.Windows.Forms.Label label8;
		private OpenDental.UI.Button butPickProv;
		private ComboBox comboProv;
		private ComboBox comboClinic;
		private Label labelClinic;
		///<summary></summary>
		private DateTime dateLimit=DateTime.MinValue;
		private GroupBox groupProcedure;
		private TextBox textProcWriteoff;
		private Label label16;
		private TextBox textProcTooth;
		private Label labelProcTooth;
		private TextBox textProcProv;
		private TextBox textProcDescription;
		private TextBox textProcDate2;
		private Label labelProcRemain;
		private TextBox textProcPaidHere;
		private TextBox textProcPrevPaid;
		private TextBox textProcAdj;
		private TextBox textProcInsEst;
		private TextBox textProcInsPaid;
		private TextBox textProcFee;
		private Label label13;
		private Label label12;
		private Label label11;
		private Label label10;
		private Label label9;
		private Label label14;
		private Label label15;
		private Label label17;
		private Label label18;
		private Label label19;
		private UI.Button butDetachProc;
		private UI.Button butAttachProc;
		///<summary>When true, the OK click will not let the user leave the window unless the check amount is 0.</summary>
		private bool _checkZeroAmount;
		private Procedure _procCur;
		private double _procFee;
		private double _procWriteoff;
		private double _procInsPaid;
		private double _procInsEst;
		private double _procAdj;
		private double _procPrevPaid;
		private double _procPaidHere;
		private decimal _remainAmt;
		///<summary>All positive adjustment defs.</summary>
		private List<Def> _listAdjPosCats;
		///<summary>All negative adjustment defs.</summary>
		private List<Def> _listAdjNegCats;
		///<summary>Cached list of clinics available to user. Also includes a dummy Clinic at index 0 for "none".</summary>
		private List<Clinic> _listClinics;
		///<summary>Filtered list of providers based on which clinic is selected. If no clinic is selected displays all providers. Also includes a dummy clinic at index 0 for "none"</summary>
		private List<Provider> _listProviders;
		///<summary>Used to keep track of the current clinic selected. This is because it may be a clinic that is not in _listClinics.</summary>
		private long _selectedClinicNum;
		///<summary>Instead of relying on _listProviders[comboProv.SelectedIndex] to determine the selected Provider we use this variable to store it explicitly.</summary>
		private long _selectedProvNum;

		///<summary></summary>
		public FormAdjust(Patient patCur,Adjustment adjustmentCur){
			InitializeComponent();
			PatCur=patCur;
			AdjustmentCur=adjustmentCur;
			Lan.F(this);
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		private void InitializeComponent(){
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAdjust));
			this.label1 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.textAdjDate = new OpenDental.ValidDate();
			this.label3 = new System.Windows.Forms.Label();
			this.butDelete = new OpenDental.UI.Button();
			this.textAmount = new OpenDental.ValidDouble();
			this.listTypePos = new System.Windows.Forms.ListBox();
			this.listTypeNeg = new System.Windows.Forms.ListBox();
			this.textProcDate = new OpenDental.ValidDate();
			this.label7 = new System.Windows.Forms.Label();
			this.textDateEntry = new OpenDental.ValidDate();
			this.label8 = new System.Windows.Forms.Label();
			this.butPickProv = new OpenDental.UI.Button();
			this.comboProv = new System.Windows.Forms.ComboBox();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.textNote = new OpenDental.ODtextBox();
			this.groupProcedure = new System.Windows.Forms.GroupBox();
			this.textProcWriteoff = new System.Windows.Forms.TextBox();
			this.label16 = new System.Windows.Forms.Label();
			this.textProcTooth = new System.Windows.Forms.TextBox();
			this.labelProcTooth = new System.Windows.Forms.Label();
			this.textProcProv = new System.Windows.Forms.TextBox();
			this.textProcDescription = new System.Windows.Forms.TextBox();
			this.textProcDate2 = new System.Windows.Forms.TextBox();
			this.labelProcRemain = new System.Windows.Forms.Label();
			this.textProcPaidHere = new System.Windows.Forms.TextBox();
			this.textProcPrevPaid = new System.Windows.Forms.TextBox();
			this.textProcAdj = new System.Windows.Forms.TextBox();
			this.textProcInsEst = new System.Windows.Forms.TextBox();
			this.textProcInsPaid = new System.Windows.Forms.TextBox();
			this.textProcFee = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.butDetachProc = new OpenDental.UI.Button();
			this.butAttachProc = new OpenDental.UI.Button();
			this.groupProcedure.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(4, 54);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Adjustment Date";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(176, 396);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 16);
			this.label4.TabIndex = 3;
			this.label4.Text = "Note";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 102);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(100, 16);
			this.label5.TabIndex = 4;
			this.label5.Text = "Amount";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(315, 14);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(167, 16);
			this.label6.TabIndex = 5;
			this.label6.Text = "Additions";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 128);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 16);
			this.label2.TabIndex = 10;
			this.label2.Text = "Provider";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(614, 433);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 6;
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
			this.butCancel.Location = new System.Drawing.Point(614, 471);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 7;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// textAdjDate
			// 
			this.textAdjDate.Location = new System.Drawing.Point(109, 52);
			this.textAdjDate.Name = "textAdjDate";
			this.textAdjDate.Size = new System.Drawing.Size(80, 20);
			this.textAdjDate.TabIndex = 8;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(528, 14);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(182, 16);
			this.label3.TabIndex = 16;
			this.label3.Text = "Subtractions";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Location = new System.Drawing.Point(24, 469);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(75, 24);
			this.butDelete.TabIndex = 17;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// textAmount
			// 
			this.textAmount.Location = new System.Drawing.Point(109, 100);
			this.textAmount.MaxVal = 100000000D;
			this.textAmount.MinVal = -100000000D;
			this.textAmount.Name = "textAmount";
			this.textAmount.Size = new System.Drawing.Size(68, 20);
			this.textAmount.TabIndex = 0;
			// 
			// listTypePos
			// 
			this.listTypePos.Location = new System.Drawing.Point(299, 34);
			this.listTypePos.Name = "listTypePos";
			this.listTypePos.Size = new System.Drawing.Size(202, 160);
			this.listTypePos.TabIndex = 3;
			this.listTypePos.SelectedIndexChanged += new System.EventHandler(this.listTypePos_SelectedIndexChanged);
			// 
			// listTypeNeg
			// 
			this.listTypeNeg.Location = new System.Drawing.Point(515, 34);
			this.listTypeNeg.Name = "listTypeNeg";
			this.listTypeNeg.Size = new System.Drawing.Size(206, 160);
			this.listTypeNeg.TabIndex = 4;
			this.listTypeNeg.SelectedIndexChanged += new System.EventHandler(this.listTypeNeg_SelectedIndexChanged);
			// 
			// textProcDate
			// 
			this.textProcDate.Location = new System.Drawing.Point(109, 76);
			this.textProcDate.Name = "textProcDate";
			this.textProcDate.Size = new System.Drawing.Size(80, 20);
			this.textProcDate.TabIndex = 9;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(4, 78);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(104, 16);
			this.label7.TabIndex = 18;
			this.label7.Text = "(procedure date)";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDateEntry
			// 
			this.textDateEntry.Location = new System.Drawing.Point(109, 28);
			this.textDateEntry.Name = "textDateEntry";
			this.textDateEntry.ReadOnly = true;
			this.textDateEntry.Size = new System.Drawing.Size(80, 20);
			this.textDateEntry.TabIndex = 21;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(4, 30);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(104, 16);
			this.label8.TabIndex = 20;
			this.label8.Text = "Entry Date";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butPickProv
			// 
			this.butPickProv.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickProv.Autosize = false;
			this.butPickProv.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickProv.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickProv.CornerRadius = 2F;
			this.butPickProv.Location = new System.Drawing.Point(268, 124);
			this.butPickProv.Name = "butPickProv";
			this.butPickProv.Size = new System.Drawing.Size(18, 21);
			this.butPickProv.TabIndex = 165;
			this.butPickProv.Text = "...";
			this.butPickProv.Click += new System.EventHandler(this.butPickProv_Click);
			// 
			// comboProv
			// 
			this.comboProv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProv.Location = new System.Drawing.Point(109, 124);
			this.comboProv.MaxDropDownItems = 30;
			this.comboProv.Name = "comboProv";
			this.comboProv.Size = new System.Drawing.Size(158, 21);
			this.comboProv.TabIndex = 1;
			this.comboProv.SelectedIndexChanged += new System.EventHandler(this.comboProv_SelectedIndexChanged);
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(109, 149);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(177, 21);
			this.comboClinic.TabIndex = 2;
			this.comboClinic.SelectedIndexChanged += new System.EventHandler(this.comboClinic_SelectedIndexChanged);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(-7, 151);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(114, 16);
			this.labelClinic.TabIndex = 163;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textNote
			// 
			this.textNote.DetectLinksEnabled = false;
			this.textNote.Location = new System.Drawing.Point(176, 415);
			this.textNote.Name = "textNote";
			this.textNote.QuickPasteType = OpenDentBusiness.QuickPasteType.Adjustment;
			this.textNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textNote.Size = new System.Drawing.Size(355, 79);
			this.textNote.SpellCheckIsEnabled = false;
			this.textNote.TabIndex = 5;
			this.textNote.Text = "";
			// 
			// groupProcedure
			// 
			this.groupProcedure.Controls.Add(this.textProcWriteoff);
			this.groupProcedure.Controls.Add(this.label16);
			this.groupProcedure.Controls.Add(this.textProcTooth);
			this.groupProcedure.Controls.Add(this.labelProcTooth);
			this.groupProcedure.Controls.Add(this.textProcProv);
			this.groupProcedure.Controls.Add(this.textProcDescription);
			this.groupProcedure.Controls.Add(this.textProcDate2);
			this.groupProcedure.Controls.Add(this.labelProcRemain);
			this.groupProcedure.Controls.Add(this.textProcPaidHere);
			this.groupProcedure.Controls.Add(this.textProcPrevPaid);
			this.groupProcedure.Controls.Add(this.textProcAdj);
			this.groupProcedure.Controls.Add(this.textProcInsEst);
			this.groupProcedure.Controls.Add(this.textProcInsPaid);
			this.groupProcedure.Controls.Add(this.textProcFee);
			this.groupProcedure.Controls.Add(this.label13);
			this.groupProcedure.Controls.Add(this.label12);
			this.groupProcedure.Controls.Add(this.label11);
			this.groupProcedure.Controls.Add(this.label10);
			this.groupProcedure.Controls.Add(this.label9);
			this.groupProcedure.Controls.Add(this.label14);
			this.groupProcedure.Controls.Add(this.label15);
			this.groupProcedure.Controls.Add(this.label17);
			this.groupProcedure.Controls.Add(this.label18);
			this.groupProcedure.Controls.Add(this.label19);
			this.groupProcedure.Controls.Add(this.butDetachProc);
			this.groupProcedure.Controls.Add(this.butAttachProc);
			this.groupProcedure.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupProcedure.Location = new System.Drawing.Point(104, 201);
			this.groupProcedure.Name = "groupProcedure";
			this.groupProcedure.Size = new System.Drawing.Size(615, 192);
			this.groupProcedure.TabIndex = 166;
			this.groupProcedure.TabStop = false;
			this.groupProcedure.Text = "Procedure";
			// 
			// textProcWriteoff
			// 
			this.textProcWriteoff.Location = new System.Drawing.Point(513, 39);
			this.textProcWriteoff.Name = "textProcWriteoff";
			this.textProcWriteoff.ReadOnly = true;
			this.textProcWriteoff.Size = new System.Drawing.Size(76, 20);
			this.textProcWriteoff.TabIndex = 50;
			this.textProcWriteoff.Text = "5.00";
			this.textProcWriteoff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(405, 41);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(104, 16);
			this.label16.TabIndex = 49;
			this.label16.Text = "- Writeoff";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textProcTooth
			// 
			this.textProcTooth.Location = new System.Drawing.Point(115, 95);
			this.textProcTooth.Name = "textProcTooth";
			this.textProcTooth.ReadOnly = true;
			this.textProcTooth.Size = new System.Drawing.Size(43, 20);
			this.textProcTooth.TabIndex = 46;
			// 
			// labelProcTooth
			// 
			this.labelProcTooth.Location = new System.Drawing.Point(9, 98);
			this.labelProcTooth.Name = "labelProcTooth";
			this.labelProcTooth.Size = new System.Drawing.Size(104, 16);
			this.labelProcTooth.TabIndex = 45;
			this.labelProcTooth.Text = "Tooth";
			this.labelProcTooth.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textProcProv
			// 
			this.textProcProv.Location = new System.Drawing.Point(115, 75);
			this.textProcProv.Name = "textProcProv";
			this.textProcProv.ReadOnly = true;
			this.textProcProv.Size = new System.Drawing.Size(76, 20);
			this.textProcProv.TabIndex = 44;
			// 
			// textProcDescription
			// 
			this.textProcDescription.Location = new System.Drawing.Point(115, 115);
			this.textProcDescription.Name = "textProcDescription";
			this.textProcDescription.ReadOnly = true;
			this.textProcDescription.Size = new System.Drawing.Size(241, 20);
			this.textProcDescription.TabIndex = 43;
			// 
			// textProcDate2
			// 
			this.textProcDate2.Location = new System.Drawing.Point(115, 55);
			this.textProcDate2.Name = "textProcDate2";
			this.textProcDate2.ReadOnly = true;
			this.textProcDate2.Size = new System.Drawing.Size(76, 20);
			this.textProcDate2.TabIndex = 42;
			// 
			// labelProcRemain
			// 
			this.labelProcRemain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelProcRemain.Location = new System.Drawing.Point(514, 166);
			this.labelProcRemain.Name = "labelProcRemain";
			this.labelProcRemain.Size = new System.Drawing.Size(73, 18);
			this.labelProcRemain.TabIndex = 41;
			this.labelProcRemain.Text = "$85.00";
			this.labelProcRemain.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textProcPaidHere
			// 
			this.textProcPaidHere.Location = new System.Drawing.Point(513, 139);
			this.textProcPaidHere.Name = "textProcPaidHere";
			this.textProcPaidHere.ReadOnly = true;
			this.textProcPaidHere.Size = new System.Drawing.Size(76, 20);
			this.textProcPaidHere.TabIndex = 40;
			this.textProcPaidHere.Text = "0.00";
			this.textProcPaidHere.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textProcPrevPaid
			// 
			this.textProcPrevPaid.Location = new System.Drawing.Point(513, 119);
			this.textProcPrevPaid.Name = "textProcPrevPaid";
			this.textProcPrevPaid.ReadOnly = true;
			this.textProcPrevPaid.Size = new System.Drawing.Size(76, 20);
			this.textProcPrevPaid.TabIndex = 39;
			this.textProcPrevPaid.Text = "0.00";
			this.textProcPrevPaid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textProcAdj
			// 
			this.textProcAdj.Location = new System.Drawing.Point(513, 99);
			this.textProcAdj.Name = "textProcAdj";
			this.textProcAdj.ReadOnly = true;
			this.textProcAdj.Size = new System.Drawing.Size(76, 20);
			this.textProcAdj.TabIndex = 38;
			this.textProcAdj.Text = "10.00";
			this.textProcAdj.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textProcInsEst
			// 
			this.textProcInsEst.Location = new System.Drawing.Point(513, 79);
			this.textProcInsEst.Name = "textProcInsEst";
			this.textProcInsEst.ReadOnly = true;
			this.textProcInsEst.Size = new System.Drawing.Size(76, 20);
			this.textProcInsEst.TabIndex = 37;
			this.textProcInsEst.Text = "55.00";
			this.textProcInsEst.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textProcInsPaid
			// 
			this.textProcInsPaid.Location = new System.Drawing.Point(513, 59);
			this.textProcInsPaid.Name = "textProcInsPaid";
			this.textProcInsPaid.ReadOnly = true;
			this.textProcInsPaid.Size = new System.Drawing.Size(76, 20);
			this.textProcInsPaid.TabIndex = 36;
			this.textProcInsPaid.Text = "45.00";
			this.textProcInsPaid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textProcFee
			// 
			this.textProcFee.Location = new System.Drawing.Point(513, 19);
			this.textProcFee.Name = "textProcFee";
			this.textProcFee.ReadOnly = true;
			this.textProcFee.Size = new System.Drawing.Size(76, 20);
			this.textProcFee.TabIndex = 35;
			this.textProcFee.Text = "200.00";
			this.textProcFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(405, 141);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(104, 16);
			this.label13.TabIndex = 34;
			this.label13.Text = "- Paid Here";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(405, 167);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(104, 16);
			this.label12.TabIndex = 33;
			this.label12.Text = "= Remaining";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(382, 121);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(127, 16);
			this.label11.TabIndex = 32;
			this.label11.Text = "- Previously Paid";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(405, 101);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(104, 16);
			this.label10.TabIndex = 31;
			this.label10.Text = "- Adjustments";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(405, 81);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(104, 16);
			this.label9.TabIndex = 30;
			this.label9.Text = "- Ins Est";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(405, 61);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(104, 16);
			this.label14.TabIndex = 29;
			this.label14.Text = "- Ins Paid";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(405, 21);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(104, 16);
			this.label15.TabIndex = 28;
			this.label15.Text = "Fee";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(9, 78);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(104, 16);
			this.label17.TabIndex = 27;
			this.label17.Text = "Provider";
			this.label17.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(9, 118);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(104, 16);
			this.label18.TabIndex = 26;
			this.label18.Text = "Description";
			this.label18.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(8, 57);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(104, 16);
			this.label19.TabIndex = 25;
			this.label19.Text = "Date";
			this.label19.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butDetachProc
			// 
			this.butDetachProc.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDetachProc.Autosize = true;
			this.butDetachProc.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDetachProc.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDetachProc.CornerRadius = 4F;
			this.butDetachProc.Location = new System.Drawing.Point(99, 21);
			this.butDetachProc.Name = "butDetachProc";
			this.butDetachProc.Size = new System.Drawing.Size(75, 24);
			this.butDetachProc.TabIndex = 9;
			this.butDetachProc.Text = "Detach";
			this.butDetachProc.Click += new System.EventHandler(this.butDetachProc_Click);
			// 
			// butAttachProc
			// 
			this.butAttachProc.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAttachProc.Autosize = true;
			this.butAttachProc.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAttachProc.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAttachProc.CornerRadius = 4F;
			this.butAttachProc.Location = new System.Drawing.Point(12, 21);
			this.butAttachProc.Name = "butAttachProc";
			this.butAttachProc.Size = new System.Drawing.Size(75, 24);
			this.butAttachProc.TabIndex = 8;
			this.butAttachProc.Text = "Attach";
			this.butAttachProc.Click += new System.EventHandler(this.butAttachProc_Click);
			// 
			// FormAdjust
			// 
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(731, 528);
			this.Controls.Add(this.groupProcedure);
			this.Controls.Add(this.butPickProv);
			this.Controls.Add(this.comboProv);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.textDateEntry);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.textProcDate);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.textNote);
			this.Controls.Add(this.listTypeNeg);
			this.Controls.Add(this.listTypePos);
			this.Controls.Add(this.textAmount);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textAdjDate);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormAdjust";
			this.ShowInTaskbar = false;
			this.Text = "Edit Adjustment";
			this.Load += new System.EventHandler(this.FormAdjust_Load);
			this.groupProcedure.ResumeLayout(false);
			this.groupProcedure.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormAdjust_Load(object sender, System.EventArgs e) {
			if(IsNew){
				if(!Security.IsAuthorized(Permissions.AdjustmentCreate,true)) {//Date not checked here.  Message will show later.
					if(!Security.IsAuthorized(Permissions.AdjustmentEditZero,true)) {//Let user create an adjustment of zero if they have this perm.
						MessageBox.Show(Lans.g("Security","Not authorized for")+"\r\n"+GroupPermissions.GetDesc(Permissions.AdjustmentCreate));
						DialogResult=DialogResult.Cancel;
						return;
					}
					//Make sure amount is 0 after OK click.
					_checkZeroAmount=true;
				}
			}
			else{
				if(!Security.IsAuthorized(Permissions.AdjustmentEdit,AdjustmentCur.AdjDate)){
					butOK.Enabled=false;
					butDelete.Enabled=false;
					//User can't edit but has edit zero amount perm.  Allow delete only if date is today.
					if(Security.IsAuthorized(Permissions.AdjustmentEditZero,true) 
						&& AdjustmentCur.AdjAmt==0
						&& AdjustmentCur.DateEntry.Date==MiscData.GetNowDateTime().Date) 
					{
						butDelete.Enabled=true;
					}
				}
			}
			textDateEntry.Text=AdjustmentCur.DateEntry.ToShortDateString();
			textAdjDate.Text=AdjustmentCur.AdjDate.ToShortDateString();
			textProcDate.Text=AdjustmentCur.ProcDate.ToShortDateString();
			if(DefC.GetValue(DefCat.AdjTypes,AdjustmentCur.AdjType)=="+"){//pos
				textAmount.Text=AdjustmentCur.AdjAmt.ToString("F");
			}
			else{//neg
				textAmount.Text=(-AdjustmentCur.AdjAmt).ToString("F");//shows without the neg sign
			}
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				labelClinic.Visible=false;
				comboClinic.Visible=false;
			}
			_listClinics=new List<Clinic>() { new Clinic() { Description=Lan.g(this,"none") } }; //Seed with "None"
			Clinics.GetForUserod(Security.CurUser).ForEach(x => _listClinics.Add(x));//do not re-organize from cache. They could either be alphabetizeded or sorted by item order.
			_listClinics.ForEach(x => comboClinic.Items.Add(x.Description));
			_selectedClinicNum=AdjustmentCur.ClinicNum;
			_selectedProvNum=AdjustmentCur.ProvNum;
			comboProv.SelectedIndex=-1;
			comboClinic.IndexSelectOrSetText(_listClinics.FindIndex(x => x.ClinicNum==_selectedClinicNum),() => { return Clinics.GetDesc(_selectedClinicNum); });
			fillComboProvHyg();
			List<Def> adjCat = DefC.Short[(int)DefCat.AdjTypes].ToList();
			//Positive adjustment types
			_listAdjPosCats = adjCat.FindAll(x => x.ItemValue=="+");
			_listAdjPosCats.ForEach(x => listTypePos.Items.Add(x.ItemName));
			listTypePos.SelectedIndex=_listAdjPosCats.FindIndex(x => x.DefNum==AdjustmentCur.AdjType);//can be -1
			//Negative adjustment types
			_listAdjNegCats = adjCat.FindAll(x => x.ItemValue=="-");
			_listAdjNegCats.ForEach(x => listTypeNeg.Items.Add(x.ItemName));
			listTypeNeg.SelectedIndex=_listAdjNegCats.FindIndex(x => x.DefNum==AdjustmentCur.AdjType);//can be -1
			FillProcedure();
			this.textNote.Text=AdjustmentCur.AdjNote;
		}

		private void listTypePos_SelectedIndexChanged(object sender,System.EventArgs e) {
			if(listTypePos.SelectedIndex!=-1) listTypeNeg.SelectedIndex=-1;
		}

		private void listTypeNeg_SelectedIndexChanged(object sender,System.EventArgs e) {
			if(listTypeNeg.SelectedIndex!=-1) listTypePos.SelectedIndex=-1;
		}

		private void butPickProv_Click(object sender,EventArgs e) {
			FormProviderPick FormPP = new FormProviderPick(_listProviders);
			FormPP.SelectedProvNum=_selectedProvNum;
			FormPP.ShowDialog();
			if(FormPP.DialogResult!=DialogResult.OK) {
				return;
			}
			_selectedProvNum=FormPP.SelectedProvNum;
			comboProv.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==_selectedProvNum),() => { return Providers.GetAbbr(_selectedProvNum); });
		}

		private void comboClinic_SelectedIndexChanged(object sender,EventArgs e) {
			if(comboClinic.SelectedIndex>-1) {
				_selectedClinicNum=_listClinics[comboClinic.SelectedIndex].ClinicNum;
			}
			fillComboProvHyg();
		}

		private void comboProv_SelectedIndexChanged(object sender,EventArgs e) {
			if(comboProv.SelectedIndex>-1) {
				_selectedProvNum=_listProviders[comboProv.SelectedIndex].ProvNum;
			}
		}

		///<summary>Fills combo provider based on which clinic is selected and attempts to preserve provider selection if any.</summary>
		private void fillComboProvHyg() {
			if(comboProv.SelectedIndex>-1) {//valid prov selected, non none or nothing.
				_selectedProvNum = _listProviders[comboProv.SelectedIndex].ProvNum;
			}
			_listProviders=Providers.GetProvsForClinic(_selectedClinicNum);
			comboProv.Items.Clear();
			_listProviders.ForEach(x => comboProv.Items.Add(x.Abbr));
			comboProv.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==_selectedProvNum),() => { return Providers.GetAbbr(_selectedProvNum); });
		}

		private void FillProcedure(){
			if(AdjustmentCur.ProcNum==0) {
				textProcDate2.Text="";
				textProcProv.Text="";
				textProcTooth.Text="";
				textProcDescription.Text="";
				_procFee=0;
				textProcFee.Text="";
				_procWriteoff=0;
				textProcWriteoff.Text="";
				_procInsPaid=0;
				textProcInsPaid.Text="";
				_procInsEst=0;
				textProcInsEst.Text="";
				_procAdj=0;
				textProcAdj.Text="";
				_procPrevPaid=0;
				textProcPrevPaid.Text="";
				_procPaidHere=0;
				textProcPaidHere.Text="";
				labelProcRemain.Text="";
				return;
			}
			_procCur=Procedures.GetOneProc(AdjustmentCur.ProcNum,false);
			List<ClaimProc> ClaimProcList=ClaimProcs.Refresh(_procCur.PatNum);
			Adjustment[] AdjustmentList=Adjustments.Refresh(_procCur.PatNum);
			PaySplit[] PaySplitList=PaySplits.Refresh(_procCur.PatNum);
			textProcDate.Text=_procCur.ProcDate.ToShortDateString();
			textProcDate2.Text=_procCur.ProcDate.ToShortDateString();
			textProcProv.Text=Providers.GetAbbr(_procCur.ProvNum);
			textProcTooth.Text=Tooth.ToInternat(_procCur.ToothNum);
			textProcDescription.Text=ProcedureCodes.GetProcCode(_procCur.CodeNum).Descript;
			_procFee=_procCur.ProcFee;
			_procWriteoff=-ClaimProcs.ProcWriteoff(ClaimProcList,_procCur.ProcNum);
			_procInsPaid=-ClaimProcs.ProcInsPay(ClaimProcList,_procCur.ProcNum);
			_procInsEst=-ClaimProcs.ProcEstNotReceived(ClaimProcList,_procCur.ProcNum);
			_procAdj=Adjustments.GetTotForProc(_procCur.ProcNum,AdjustmentList,AdjustmentCur.AdjNum);
			//next line will still work even if IsNew
			_procPrevPaid=-PaySplits.GetTotForProc(_procCur.ProcNum,PaySplitList);
			textProcFee.Text=_procFee.ToString("F");
			if(_procWriteoff==0){
				textProcWriteoff.Text="";
			}
			else{
				textProcWriteoff.Text=_procWriteoff.ToString("F");
			}
			if(_procInsPaid==0){
				textProcInsPaid.Text="";
			}
			else{
				textProcInsPaid.Text=_procInsPaid.ToString("F");
			}
			if(_procInsEst==0){
				textProcInsEst.Text="";
			}
			else{
				textProcInsEst.Text=_procInsEst.ToString("F");
			}
			if(_procAdj==0){
				textProcAdj.Text="";
			}
			else{
				textProcAdj.Text=_procAdj.ToString("F");
			}
			if(_procPrevPaid==0){
				textProcPrevPaid.Text="";
			}
			else{
				textProcPrevPaid.Text=_procPrevPaid.ToString("F");
			}
			ComputeProcTotals();
		}

		///<summary>Does not alter any of the proc amounts except PaidHere and Remaining.</summary>
		private void ComputeProcTotals() {
			_procPaidHere=0;
			if(textAmount.errorProvider1.GetError(textAmount)==""){
				_procPaidHere=-PIn.Double(textAmount.Text);	
			}
			if(_procPaidHere==0){
				textProcPaidHere.Text="";
			}
			else{
				textProcPaidHere.Text=_procPaidHere.ToString("F");
			}
			//most of these are negative values, so add
			_remainAmt=
				(decimal)_procFee
				+(decimal)_procWriteoff
				+(decimal)_procInsPaid
				+(decimal)_procInsEst
				+(decimal)_procAdj
				+(decimal)_procPrevPaid
				+(decimal)_procPaidHere;
			labelProcRemain.Text=_remainAmt.ToString("c");
		}

		private void butAttachProc_Click(object sender, System.EventArgs e) {
			FormProcSelect FormPS=new FormProcSelect(AdjustmentCur.PatNum);
			FormPS.ShowDialog();
			if(FormPS.DialogResult!=DialogResult.OK){
				return;
			}
			AdjustmentCur.ProcNum=FormPS.ListSelectedProcs[0].ProcNum;
			FillProcedure();
			textProcDate.Text=FormPS.ListSelectedProcs[0].ProcDate.ToShortDateString();
		}

		private void butDetachProc_Click(object sender, System.EventArgs e) {
			AdjustmentCur.ProcNum=0;
			FillProcedure();
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if( textAdjDate.errorProvider1.GetError(textAdjDate)!=""
				|| textProcDate.errorProvider1.GetError(textProcDate)!=""
				|| textAmount.errorProvider1.GetError(textAmount)!=""
				){
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(textAmount.Text==""){
				MessageBox.Show(Lan.g(this,"Please enter an amount."));	
				return;
			}
			if(listTypeNeg.SelectedIndex==-1 && listTypePos.SelectedIndex==-1){
				MsgBox.Show(this,"Please select a type first.");
				return;
			}
			if(IsNew){
				//prevents backdating of initial adjustment
				if(!Security.IsAuthorized(Permissions.AdjustmentCreate,PIn.Date(textAdjDate.Text),true)){//Give message later.
					if(!_checkZeroAmount) {//Let user create as long as Amount is zero and has edit zero permissions.  This was checked on load.
						MessageBox.Show(Lans.g("Security","Not authorized for")+"\r\n"+GroupPermissions.GetDesc(Permissions.AdjustmentCreate));
						return;
					}
				}
			}
			else{
				//Editing an old entry will already be blocked if the date was too old, and user will not be able to click OK button
				//This catches it if user changed the date to be older.
				if(!Security.IsAuthorized(Permissions.AdjustmentEdit,PIn.Date(textAdjDate.Text))){
					return;
				}
			}
			//DateEntry not allowed to change
			AdjustmentCur.AdjDate=PIn.Date(textAdjDate.Text);
			AdjustmentCur.ProcDate=PIn.Date(textProcDate.Text);
			AdjustmentCur.ProvNum=_selectedProvNum;
			AdjustmentCur.ClinicNum=_selectedClinicNum;
			if(listTypePos.SelectedIndex!=-1) {
				AdjustmentCur.AdjType=_listAdjPosCats[listTypePos.SelectedIndex].DefNum;
				AdjustmentCur.AdjAmt=PIn.Double(textAmount.Text);
			}
			if(listTypeNeg.SelectedIndex!=-1) {
				AdjustmentCur.AdjType=_listAdjNegCats[listTypeNeg.SelectedIndex].DefNum;
				AdjustmentCur.AdjAmt=-PIn.Double(textAmount.Text);
			}
			if(_checkZeroAmount && AdjustmentCur.AdjAmt!=0) {
				MsgBox.Show(this,"Amount has to be 0.00 due to security permission.");
				return;
			}
			AdjustmentCur.AdjNote=textNote.Text;
			try{
				if(IsNew) {
					Adjustments.Insert(AdjustmentCur);
					SecurityLogs.MakeLogEntry(Permissions.AdjustmentCreate,AdjustmentCur.PatNum,
						Patients.GetLim(AdjustmentCur.PatNum).GetNameLF()+", "
						+AdjustmentCur.AdjAmt.ToString("c"));
				}
				else {
					Adjustments.Update(AdjustmentCur);
					SecurityLogs.MakeLogEntry(Permissions.AdjustmentEdit,AdjustmentCur.PatNum,
						Patients.GetLim(AdjustmentCur.PatNum).GetNameLF()+", "
						+AdjustmentCur.AdjAmt.ToString("c"));
				}
			}
			catch(Exception ex){//even though it doesn't currently throw any exceptions
				MessageBox.Show(ex.Message);
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			if(IsNew){
				DialogResult=DialogResult.Cancel;
			}
			else{
				SecurityLogs.MakeLogEntry(Permissions.AdjustmentEdit,AdjustmentCur.PatNum,
					"Delete for patient: "
					+Patients.GetLim(AdjustmentCur.PatNum).GetNameLF()+", "
					+AdjustmentCur.AdjAmt.ToString("c"));
				Adjustments.Delete(AdjustmentCur);
				DialogResult=DialogResult.OK;
			}
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}

	///<summary></summary>
	public struct AdjustmentItem{
		///<summary></summary>
		public string ItemText;
		///<summary></summary>
		public int ItemIndex;
	}

}
