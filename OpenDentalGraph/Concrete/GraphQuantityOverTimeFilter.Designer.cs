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
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.graph = new OpenDentalGraph.GraphQuantityOverTime();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.SuspendLayout();
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
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.graph);
			this.splitContainer.Size = new System.Drawing.Size(788, 410);
			this.splitContainer.SplitterDistance = 37;
			this.splitContainer.SplitterWidth = 1;
			this.splitContainer.TabIndex = 9;
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
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.SplitContainer splitContainer;
		private GraphQuantityOverTime graph;
	}
}
