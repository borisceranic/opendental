﻿namespace CentralManager {
	partial class FormCentralManager {
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
			this.butConSetup = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.butPassword = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.textSearch = new System.Windows.Forms.TextBox();
			this.butProdInc = new System.Windows.Forms.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// butConSetup
			// 
			this.butConSetup.Location = new System.Drawing.Point(101, 19);
			this.butConSetup.Name = "butConSetup";
			this.butConSetup.Size = new System.Drawing.Size(87, 24);
			this.butConSetup.TabIndex = 0;
			this.butConSetup.Text = "Connections";
			this.butConSetup.UseVisualStyleBackColor = true;
			this.butConSetup.Click += new System.EventHandler(this.butConSetup_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.butPassword);
			this.groupBox1.Controls.Add(this.butConSetup);
			this.groupBox1.Location = new System.Drawing.Point(11, 10);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(204, 51);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Setup";
			// 
			// butPassword
			// 
			this.butPassword.Location = new System.Drawing.Point(15, 19);
			this.butPassword.Name = "butPassword";
			this.butPassword.Size = new System.Drawing.Size(78, 24);
			this.butPassword.TabIndex = 1;
			this.butPassword.Text = "Password";
			this.butPassword.UseVisualStyleBackColor = true;
			this.butPassword.Click += new System.EventHandler(this.butPassword_Click);
			// 
			// label2
			// 
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(237, 36);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(111, 17);
			this.label2.TabIndex = 212;
			this.label2.Text = "Search";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textSearch
			// 
			this.textSearch.Location = new System.Drawing.Point(351, 33);
			this.textSearch.Name = "textSearch";
			this.textSearch.Size = new System.Drawing.Size(190, 20);
			this.textSearch.TabIndex = 211;
			this.textSearch.TextChanged += new System.EventHandler(this.textSearch_TextChanged);
			// 
			// butProdInc
			// 
			this.butProdInc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butProdInc.Location = new System.Drawing.Point(684, 31);
			this.butProdInc.Name = "butProdInc";
			this.butProdInc.Size = new System.Drawing.Size(87, 24);
			this.butProdInc.TabIndex = 213;
			this.butProdInc.Text = "Annual P&&I";
			this.butProdInc.UseVisualStyleBackColor = true;
			this.butProdInc.Click += new System.EventHandler(this.butProdInc_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(11, 72);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(760, 612);
			this.gridMain.TabIndex = 5;
			this.gridMain.Title = "Connections - double click to launch";
			this.gridMain.TranslationName = "";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// FormCentralManager
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 696);
			this.Controls.Add(this.butProdInc);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textSearch);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.gridMain);
			this.MinimumSize = new System.Drawing.Size(657, 267);
			this.Name = "FormCentralManager";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Central Manager";
			this.Load += new System.EventHandler(this.FormCentralManager_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button butConSetup;
		private OpenDental.UI.ODGrid gridMain;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button butPassword;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textSearch;
		private System.Windows.Forms.Button butProdInc;
	}
}

