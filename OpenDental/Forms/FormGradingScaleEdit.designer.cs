namespace OpenDental{
	partial class FormGradingScaleEdit {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGradingScaleEdit));
			this.label2 = new System.Windows.Forms.Label();
			this.textDescription = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butAdd = new OpenDental.UI.Button();
			this.labelIsPercentage = new System.Windows.Forms.Label();
			this.comboScaleType = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textMaxPointsPossible = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(14, 12);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(122, 17);
			this.label2.TabIndex = 127;
			this.label2.Text = "Description";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDescription
			// 
			this.textDescription.Location = new System.Drawing.Point(137, 11);
			this.textDescription.MaxLength = 255;
			this.textDescription.Name = "textDescription";
			this.textDescription.Size = new System.Drawing.Size(181, 20);
			this.textDescription.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(234, 34);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(165, 27);
			this.label1.TabIndex = 129;
			this.label1.Text = "Assumes a 0-100% grading scale.";
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(234, 337);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 4;
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
			this.butCancel.Location = new System.Drawing.Point(315, 337);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 5;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 83);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(378, 238);
			this.gridMain.TabIndex = 8;
			this.gridMain.Title = "Grading Scale Items";
			this.gridMain.TranslationName = null;
			this.gridMain.DoubleClick += new System.EventHandler(this.gridMain_DoubleClick);
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
			this.butAdd.Location = new System.Drawing.Point(12, 337);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(75, 24);
			this.butAdd.TabIndex = 2;
			this.butAdd.Text = "Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// labelIsPercentage
			// 
			this.labelIsPercentage.ForeColor = System.Drawing.Color.Red;
			this.labelIsPercentage.Location = new System.Drawing.Point(93, 324);
			this.labelIsPercentage.Name = "labelIsPercentage";
			this.labelIsPercentage.Size = new System.Drawing.Size(135, 42);
			this.labelIsPercentage.TabIndex = 130;
			this.labelIsPercentage.Text = "Grading scale items are only used for PickList scale types.";
			this.labelIsPercentage.Visible = false;
			// 
			// comboScaleType
			// 
			this.comboScaleType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboScaleType.FormattingEnabled = true;
			this.comboScaleType.Location = new System.Drawing.Point(137, 34);
			this.comboScaleType.Name = "comboScaleType";
			this.comboScaleType.Size = new System.Drawing.Size(91, 21);
			this.comboScaleType.TabIndex = 131;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(17, 35);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(119, 18);
			this.label3.TabIndex = 132;
			this.label3.Text = "Scale Type";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(14, 60);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(122, 17);
			this.label4.TabIndex = 134;
			this.label4.Text = "Max Points";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textMaxPointsPossible
			// 
			this.textMaxPointsPossible.Location = new System.Drawing.Point(137, 59);
			this.textMaxPointsPossible.MaxLength = 255;
			this.textMaxPointsPossible.Name = "textMaxPointsPossible";
			this.textMaxPointsPossible.Size = new System.Drawing.Size(91, 20);
			this.textMaxPointsPossible.TabIndex = 133;
			// 
			// FormGradingScaleEdit
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(402, 375);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textMaxPointsPossible);
			this.Controls.Add(this.comboScaleType);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.labelIsPercentage);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butAdd);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(418, 388);
			this.Name = "FormGradingScaleEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Grading Scale Edit";
			this.Load += new System.EventHandler(this.FormGradingScaleEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private UI.ODGrid gridMain;
		private UI.Button butAdd;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textDescription;
		private System.Windows.Forms.Label label1;
		private UI.Button butOK;
		private UI.Button butCancel;
		private System.Windows.Forms.Label labelIsPercentage;
		private System.Windows.Forms.ComboBox comboScaleType;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textMaxPointsPossible;
	}
}