namespace OpenDental{
	partial class FormPaySplitManage {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPaySplitManage));
			this.label1 = new System.Windows.Forms.Label();
			this.textAmtAvailable = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.checkShowPaid = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textSplitAmt = new System.Windows.Forms.TextBox();
			this.gridCharges = new OpenDental.UI.ODGrid();
			this.gridSplits = new OpenDental.UI.ODGrid();
			this.buttAddMany = new OpenDental.UI.Button();
			this.butClear = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butCreatePartial = new OpenDental.UI.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.textPayAmt = new System.Windows.Forms.TextBox();
			this.butEdit = new OpenDental.UI.Button();
			this.butCreateManual = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(432, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(129, 23);
			this.label1.TabIndex = 4;
			this.label1.Text = "Amount Remaining";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// textAmtAvailable
			// 
			this.textAmtAvailable.Location = new System.Drawing.Point(432, 39);
			this.textAmtAvailable.Name = "textAmtAvailable";
			this.textAmtAvailable.ReadOnly = true;
			this.textAmtAvailable.Size = new System.Drawing.Size(129, 20);
			this.textAmtAvailable.TabIndex = 5;
			this.textAmtAvailable.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Location = new System.Drawing.Point(432, 76);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(409, 32);
			this.label3.TabIndex = 8;
			this.label3.Text = "Select outstanding charges and click Create Split to create pay splits for those " +
    "charges.  Click Create Partial to create a partial pay split for selected charge" +
    "s.";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 76);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(409, 32);
			this.label4.TabIndex = 14;
			this.label4.Text = "Select payment splits and click the Delete Split button to remove selected splits" +
    ", or Clear All to remove all splits. \r\n";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// checkShowPaid
			// 
			this.checkShowPaid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.checkShowPaid.Location = new System.Drawing.Point(431, 575);
			this.checkShowPaid.Name = "checkShowPaid";
			this.checkShowPaid.Size = new System.Drawing.Size(173, 24);
			this.checkShowPaid.TabIndex = 19;
			this.checkShowPaid.Text = "Show Paid Charges";
			this.checkShowPaid.UseVisualStyleBackColor = true;
			this.checkShowPaid.CheckedChanged += new System.EventHandler(this.checkShowPaid_CheckedChanged);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(296, 13);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(129, 23);
			this.label2.TabIndex = 20;
			this.label2.Text = "Split Amount";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// textSplitAmt
			// 
			this.textSplitAmt.Location = new System.Drawing.Point(296, 39);
			this.textSplitAmt.Name = "textSplitAmt";
			this.textSplitAmt.ReadOnly = true;
			this.textSplitAmt.Size = new System.Drawing.Size(129, 20);
			this.textSplitAmt.TabIndex = 21;
			this.textSplitAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// gridCharges
			// 
			this.gridCharges.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridCharges.HScrollVisible = false;
			this.gridCharges.Location = new System.Drawing.Point(431, 109);
			this.gridCharges.Name = "gridCharges";
			this.gridCharges.ScrollValue = 0;
			this.gridCharges.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridCharges.Size = new System.Drawing.Size(409, 426);
			this.gridCharges.TabIndex = 13;
			this.gridCharges.Title = "Outstanding Charges";
			this.gridCharges.TranslationName = null;
			// 
			// gridSplits
			// 
			this.gridSplits.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridSplits.HScrollVisible = false;
			this.gridSplits.Location = new System.Drawing.Point(16, 110);
			this.gridSplits.Name = "gridSplits";
			this.gridSplits.ScrollValue = 0;
			this.gridSplits.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridSplits.Size = new System.Drawing.Size(409, 426);
			this.gridSplits.TabIndex = 12;
			this.gridSplits.Title = "Current Payment Splits";
			this.gridSplits.TranslationName = null;
			this.gridSplits.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridSplits_CellDoubleClick);
			// 
			// buttAddMany
			// 
			this.buttAddMany.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.buttAddMany.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttAddMany.Autosize = true;
			this.buttAddMany.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.buttAddMany.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.buttAddMany.CornerRadius = 4F;
			this.buttAddMany.Image = global::OpenDental.Properties.Resources.Add;
			this.buttAddMany.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttAddMany.Location = new System.Drawing.Point(431, 541);
			this.buttAddMany.Name = "buttAddMany";
			this.buttAddMany.Size = new System.Drawing.Size(96, 24);
			this.buttAddMany.TabIndex = 18;
			this.buttAddMany.Text = "Create Split";
			this.buttAddMany.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.buttAddMany.Click += new System.EventHandler(this.butCreateSplit_Click);
			// 
			// butClear
			// 
			this.butClear.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butClear.Autosize = true;
			this.butClear.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClear.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClear.CornerRadius = 4F;
			this.butClear.Location = new System.Drawing.Point(111, 542);
			this.butClear.Name = "butClear";
			this.butClear.Size = new System.Drawing.Size(89, 24);
			this.butClear.TabIndex = 17;
			this.butClear.Text = "Clear All";
			this.butClear.Click += new System.EventHandler(this.butClearAll_Click);
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
			this.butDelete.Location = new System.Drawing.Point(16, 542);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(89, 24);
			this.butDelete.TabIndex = 16;
			this.butDelete.Text = "Delete Split";
			this.butDelete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.butDelete.Click += new System.EventHandler(this.butDeleteSplit_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(766, 542);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 3;
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
			this.butCancel.Location = new System.Drawing.Point(766, 569);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butCreatePartial
			// 
			this.butCreatePartial.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCreatePartial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCreatePartial.Autosize = true;
			this.butCreatePartial.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCreatePartial.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCreatePartial.CornerRadius = 4F;
			this.butCreatePartial.Image = global::OpenDental.Properties.Resources.Add;
			this.butCreatePartial.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butCreatePartial.Location = new System.Drawing.Point(530, 541);
			this.butCreatePartial.Name = "butCreatePartial";
			this.butCreatePartial.Size = new System.Drawing.Size(103, 24);
			this.butCreatePartial.TabIndex = 22;
			this.butCreatePartial.Text = "Create Partial";
			this.butCreatePartial.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.butCreatePartial.Click += new System.EventHandler(this.butCreatePartial_Click);
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(61, 13);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(129, 23);
			this.label5.TabIndex = 23;
			this.label5.Text = "Payment Amount";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// textPayAmt
			// 
			this.textPayAmt.Location = new System.Drawing.Point(61, 39);
			this.textPayAmt.Name = "textPayAmt";
			this.textPayAmt.ReadOnly = true;
			this.textPayAmt.Size = new System.Drawing.Size(129, 20);
			this.textPayAmt.TabIndex = 24;
			this.textPayAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// butEdit
			// 
			this.butEdit.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEdit.Autosize = true;
			this.butEdit.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEdit.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEdit.CornerRadius = 4F;
			this.butEdit.Location = new System.Drawing.Point(193, 39);
			this.butEdit.Name = "butEdit";
			this.butEdit.Size = new System.Drawing.Size(45, 20);
			this.butEdit.TabIndex = 25;
			this.butEdit.Text = "Edit";
			this.butEdit.Click += new System.EventHandler(this.butEdit_Click);
			// 
			// butCreateManual
			// 
			this.butCreateManual.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCreateManual.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCreateManual.Autosize = true;
			this.butCreateManual.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCreateManual.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCreateManual.CornerRadius = 4F;
			this.butCreateManual.Image = global::OpenDental.Properties.Resources.Add;
			this.butCreateManual.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butCreateManual.Location = new System.Drawing.Point(320, 541);
			this.butCreateManual.Name = "butCreateManual";
			this.butCreateManual.Size = new System.Drawing.Size(105, 24);
			this.butCreateManual.TabIndex = 26;
			this.butCreateManual.Text = "Create Manual";
			this.butCreateManual.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.butCreateManual.Click += new System.EventHandler(this.butCreateManual_Click);
			// 
			// FormPaySplitManage
			// 
			this.AcceptButton = this.butOK;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(853, 611);
			this.Controls.Add(this.butCreateManual);
			this.Controls.Add(this.butEdit);
			this.Controls.Add(this.textPayAmt);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.butCreatePartial);
			this.Controls.Add(this.textSplitAmt);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.checkShowPaid);
			this.Controls.Add(this.buttAddMany);
			this.Controls.Add(this.butClear);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.gridCharges);
			this.Controls.Add(this.gridSplits);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textAmtAvailable);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(869, 649);
			this.Name = "FormPaySplitManage";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Pay Split Manager";
			this.Load += new System.EventHandler(this.FormPaySplitManage_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textAmtAvailable;
		private System.Windows.Forms.Label label3;
		private UI.ODGrid gridSplits;
		private UI.ODGrid gridCharges;
		private System.Windows.Forms.Label label4;
		private UI.Button butDelete;
		private UI.Button butClear;
		private UI.Button buttAddMany;
		private System.Windows.Forms.CheckBox checkShowPaid;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textSplitAmt;
		private UI.Button butCreatePartial;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textPayAmt;
		private UI.Button butEdit;
		private UI.Button butCreateManual;
	}
}