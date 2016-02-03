namespace OpenDentalGraph {
	partial class GraphQuantityOverTimeFilter {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkIncludeAdjustments = new System.Windows.Forms.CheckBox();
			this.checkIncludeCompletedProcs = new System.Windows.Forms.CheckBox();
			this.checkIncludeWriteoffs = new System.Windows.Forms.CheckBox();
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.splitContainerProductionIncome = new System.Windows.Forms.SplitContainer();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.checkIncludeInsuranceClaimPayments = new System.Windows.Forms.CheckBox();
			this.checkIncludePaySplits = new System.Windows.Forms.CheckBox();
			this.graph = new OpenDentalGraph.GraphQuantityOverTime();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerProductionIncome)).BeginInit();
			this.splitContainerProductionIncome.Panel1.SuspendLayout();
			this.splitContainerProductionIncome.Panel2.SuspendLayout();
			this.splitContainerProductionIncome.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.checkIncludeAdjustments);
			this.groupBox1.Controls.Add(this.checkIncludeCompletedProcs);
			this.groupBox1.Controls.Add(this.checkIncludeWriteoffs);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(422, 37);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Included Production Sources";
			// 
			// checkIncludeAdjustments
			// 
			this.checkIncludeAdjustments.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeAdjustments.Checked = true;
			this.checkIncludeAdjustments.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkIncludeAdjustments.Location = new System.Drawing.Point(124, 13);
			this.checkIncludeAdjustments.Name = "checkIncludeAdjustments";
			this.checkIncludeAdjustments.Size = new System.Drawing.Size(87, 24);
			this.checkIncludeAdjustments.TabIndex = 3;
			this.checkIncludeAdjustments.Text = "Adjustments";
			this.checkIncludeAdjustments.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeAdjustments.UseVisualStyleBackColor = true;
			this.checkIncludeAdjustments.CheckedChanged += new System.EventHandler(this.OnFormInputsChanged);
			// 
			// checkIncludeCompletedProcs
			// 
			this.checkIncludeCompletedProcs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeCompletedProcs.Checked = true;
			this.checkIncludeCompletedProcs.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkIncludeCompletedProcs.Location = new System.Drawing.Point(6, 13);
			this.checkIncludeCompletedProcs.Name = "checkIncludeCompletedProcs";
			this.checkIncludeCompletedProcs.Size = new System.Drawing.Size(112, 24);
			this.checkIncludeCompletedProcs.TabIndex = 5;
			this.checkIncludeCompletedProcs.Text = "Completed Procs";
			this.checkIncludeCompletedProcs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeCompletedProcs.UseVisualStyleBackColor = true;
			this.checkIncludeCompletedProcs.CheckedChanged += new System.EventHandler(this.OnFormInputsChanged);
			// 
			// checkIncludeWriteoffs
			// 
			this.checkIncludeWriteoffs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeWriteoffs.Checked = true;
			this.checkIncludeWriteoffs.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkIncludeWriteoffs.Location = new System.Drawing.Point(217, 13);
			this.checkIncludeWriteoffs.Name = "checkIncludeWriteoffs";
			this.checkIncludeWriteoffs.Size = new System.Drawing.Size(70, 24);
			this.checkIncludeWriteoffs.TabIndex = 4;
			this.checkIncludeWriteoffs.Text = "Writeoffs";
			this.checkIncludeWriteoffs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeWriteoffs.UseVisualStyleBackColor = true;
			this.checkIncludeWriteoffs.CheckedChanged += new System.EventHandler(this.OnFormInputsChanged);
			// 
			// splitContainer
			// 
			this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer.IsSplitterFixed = true;
			this.splitContainer.Location = new System.Drawing.Point(0, 0);
			this.splitContainer.Name = "splitContainer";
			this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.splitContainerProductionIncome);
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.graph);
			this.splitContainer.Size = new System.Drawing.Size(788, 410);
			this.splitContainer.SplitterDistance = 37;
			this.splitContainer.SplitterWidth = 1;
			this.splitContainer.TabIndex = 9;
			// 
			// splitContainerProductionIncome
			// 
			this.splitContainerProductionIncome.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainerProductionIncome.IsSplitterFixed = true;
			this.splitContainerProductionIncome.Location = new System.Drawing.Point(0, 0);
			this.splitContainerProductionIncome.Name = "splitContainerProductionIncome";
			// 
			// splitContainerProductionIncome.Panel1
			// 
			this.splitContainerProductionIncome.Panel1.Controls.Add(this.groupBox1);
			// 
			// splitContainerProductionIncome.Panel2
			// 
			this.splitContainerProductionIncome.Panel2.Controls.Add(this.groupBox2);
			this.splitContainerProductionIncome.Size = new System.Drawing.Size(788, 37);
			this.splitContainerProductionIncome.SplitterDistance = 422;
			this.splitContainerProductionIncome.TabIndex = 7;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.checkIncludeInsuranceClaimPayments);
			this.groupBox2.Controls.Add(this.checkIncludePaySplits);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox2.Location = new System.Drawing.Point(0, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(362, 37);
			this.groupBox2.TabIndex = 7;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Included Income Sources";
			// 
			// checkIncludeInsuranceClaimPayments
			// 
			this.checkIncludeInsuranceClaimPayments.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeInsuranceClaimPayments.Checked = true;
			this.checkIncludeInsuranceClaimPayments.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkIncludeInsuranceClaimPayments.Location = new System.Drawing.Point(99, 13);
			this.checkIncludeInsuranceClaimPayments.Name = "checkIncludeInsuranceClaimPayments";
			this.checkIncludeInsuranceClaimPayments.Size = new System.Drawing.Size(168, 24);
			this.checkIncludeInsuranceClaimPayments.TabIndex = 3;
			this.checkIncludeInsuranceClaimPayments.Text = "Insurance Claim Payments";
			this.checkIncludeInsuranceClaimPayments.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeInsuranceClaimPayments.UseVisualStyleBackColor = true;
			this.checkIncludeInsuranceClaimPayments.CheckedChanged += new System.EventHandler(this.OnFormInputsChanged);
			// 
			// checkIncludePaySplits
			// 
			this.checkIncludePaySplits.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludePaySplits.Checked = true;
			this.checkIncludePaySplits.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkIncludePaySplits.Location = new System.Drawing.Point(6, 13);
			this.checkIncludePaySplits.Name = "checkIncludePaySplits";
			this.checkIncludePaySplits.Size = new System.Drawing.Size(87, 24);
			this.checkIncludePaySplits.TabIndex = 5;
			this.checkIncludePaySplits.Text = "Pay Splits";
			this.checkIncludePaySplits.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludePaySplits.UseVisualStyleBackColor = true;
			this.checkIncludePaySplits.CheckedChanged += new System.EventHandler(this.OnFormInputsChanged);
			// 
			// graph
			// 
			this.graph.BackColor = System.Drawing.Color.Transparent;
			this.graph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.graph.BreakdownPref = OpenDentalGraph.Enumerations.BreakdownType.items;
			this.graph.ChartSubTitle = "";
			this.graph.CountItemDescription = "Completed Procedures";
			this.graph.DateFrom = new System.DateTime(1880, 1, 1, 0, 0, 0, 0);
			this.graph.DateTo = new System.DateTime(2180, 1, 1, 0, 0, 0, 0);
			this.graph.Dock = System.Windows.Forms.DockStyle.Fill;
			this.graph.GraphTitle = "Production";
			this.graph.GroupByType = System.Windows.Forms.DataVisualization.Charting.IntervalType.Weeks;
			this.graph.IsLoading = false;
			this.graph.LegendDock = OpenDentalGraph.Enumerations.LegendDockType.Bottom;
			this.graph.LegendTitle = "Provider";
			this.graph.Location = new System.Drawing.Point(0, 0);
			this.graph.MoneyItemDescription = "Income";
			this.graph.Name = "graph";
			this.graph.QtyType = OpenDentalGraph.Enumerations.QuantityType.money;
			this.graph.QuickRangePref = OpenDentalGraph.Enumerations.QuickRange.allTime;
			this.graph.SeriesType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedArea;
			this.graph.ShowFilters = true;
			this.graph.Size = new System.Drawing.Size(788, 372);
			this.graph.TabIndex = 1;
			this.graph.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.graph.OnGetGetColor += new OpenDentalGraph.OnGetColorArgs(this.graph_OnGetGetColor);
			// 
			// GraphQuantityOverTimeFilter
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.splitContainer);
			this.Graph = this.graph;
			this.Name = "GraphQuantityOverTimeFilter";
			this.Size = new System.Drawing.Size(788, 410);
			this.groupBox1.ResumeLayout(false);
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.splitContainerProductionIncome.Panel1.ResumeLayout(false);
			this.splitContainerProductionIncome.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainerProductionIncome)).EndInit();
			this.splitContainerProductionIncome.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.CheckBox checkIncludeAdjustments;
		private System.Windows.Forms.CheckBox checkIncludeWriteoffs;
		private System.Windows.Forms.CheckBox checkIncludeCompletedProcs;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.SplitContainer splitContainer;
		private GraphQuantityOverTime graph;
		private System.Windows.Forms.SplitContainer splitContainerProductionIncome;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox checkIncludeInsuranceClaimPayments;
		private System.Windows.Forms.CheckBox checkIncludePaySplits;
	}
}
