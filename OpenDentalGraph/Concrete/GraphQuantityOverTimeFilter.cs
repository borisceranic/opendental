﻿using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using OpenDentalGraph.Cache;
using OpenDentalGraph.Enumerations;
using System.Drawing;

namespace OpenDentalGraph {
	public partial class GraphQuantityOverTimeFilter:GraphQuantityOverTimeFilterT {
		#region Private Data
		private DashboardCellType _cellType=DashboardCellType.NotDefined;
		private IncomeGraphOptionsCtrl _incomeOptionsCtrl=new IncomeGraphOptionsCtrl();
		private ProductionGraphOptionsCtrl _productionOptionsCtrl=new ProductionGraphOptionsCtrl();
		private BrokenApptGraphOptionsCtrl _brokenApptsCtrl=new BrokenApptGraphOptionsCtrl();
		#endregion

		#region Properties
		public bool CanEdit {
			get {
				switch(CellType) {
					case DashboardCellType.ProductionGraph:
					case DashboardCellType.IncomeGraph:
					case DashboardCellType.NewPatientsGraph:
					case DashboardCellType.BrokenApptGraph:
					case DashboardCellType.AccountsReceivableGraph:
						return true;
					default:
						return false;
				}
			}
		}
		[Category("Graph")]
		public new GraphQuantityOverTime Graph {
			get { return graph; }
			set { base.Graph=value; graph=value; }
		}
		[Category("Graph")]
		public bool ShowFilters {
			get { return Graph.ShowFilters; }
			set {
				Graph.ShowFilters=value;
				switch(CellType) {
					case DashboardCellType.ProductionGraph:
					case DashboardCellType.IncomeGraph:
					case DashboardCellType.BrokenApptGraph:
					case DashboardCellType.NewPatientsGraph:
						splitContainer.Panel1Collapsed=!Graph.ShowFilters;
						break;
					case DashboardCellType.AccountsReceivableGraph:
					default:
						break;					
				}
			}
		}
		[Category("Graph")]
		public DashboardCellType CellType {
			get { return _cellType; }
			set {
				_cellType=value;
				BaseGraphOptionsCtrl filterCtrl=null;
				switch(CellType) {
					case DashboardCellType.ProductionGraph:
						filterCtrl=_productionOptionsCtrl;
						Graph.GraphTitle="Production";
						Graph.MoneyItemDescription="Production $";
						Graph.CountItemDescription="Count Procedures";
						Graph.GroupByType=System.Windows.Forms.DataVisualization.Charting.IntervalType.Months;
						Graph.SeriesType=System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
						Graph.BreakdownPref=BreakdownType.none;
						Graph.LegendDock=LegendDockType.None;
						Graph.QuickRangePref=QuickRange.last12Months;
						Graph.QtyType=QuantityType.money;
						break;
					case DashboardCellType.IncomeGraph:
						filterCtrl=_incomeOptionsCtrl;
						Graph.GraphTitle="Income";
						Graph.MoneyItemDescription="Income $";
						Graph.RemoveQuantityType(QuantityType.count);
						Graph.GroupByType=System.Windows.Forms.DataVisualization.Charting.IntervalType.Months;
						Graph.SeriesType=System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
						Graph.BreakdownPref=BreakdownType.none;
						Graph.LegendDock=LegendDockType.None;
						Graph.QuickRangePref=QuickRange.last12Months;
						Graph.QtyType=QuantityType.money;
						break;
					case DashboardCellType.BrokenApptGraph:
						filterCtrl=_brokenApptsCtrl;
						Graph.GraphTitle="Broken Appointments";
						Graph.MoneyItemDescription="Fees";
						Graph.CountItemDescription="Count";
						Graph.GroupByType=System.Windows.Forms.DataVisualization.Charting.IntervalType.Months;
						Graph.SeriesType=System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
						Graph.BreakdownPref=BreakdownType.none;
						Graph.LegendDock=LegendDockType.None;
						Graph.QuickRangePref=QuickRange.last12Months;
						Graph.QtyType=QuantityType.count;
						break;
					case DashboardCellType.AccountsReceivableGraph:
						Graph.GraphTitle="Accounts Receivable";
						Graph.MoneyItemDescription="Receivable $";
						Graph.RemoveQuantityType(QuantityType.count);
						Graph.GroupByType=System.Windows.Forms.DataVisualization.Charting.IntervalType.Months;
						Graph.SeriesType=System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
						Graph.BreakdownPref=BreakdownType.none;
						Graph.LegendDock=LegendDockType.None;
						Graph.QuickRangePref=QuickRange.last12Months;
						Graph.QtyType=QuantityType.money;
						break;
					case DashboardCellType.NewPatientsGraph:
						filterCtrl=new BaseGraphOptionsCtrl();
						Graph.GraphTitle="New Patients";
						Graph.RemoveQuantityType(QuantityType.money);
						Graph.CountItemDescription="Count Patients";
						Graph.GroupByType=System.Windows.Forms.DataVisualization.Charting.IntervalType.Months;
						Graph.SeriesType=System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
						Graph.BreakdownPref=BreakdownType.none;
						Graph.LegendDock=LegendDockType.None;
						Graph.QuickRangePref=QuickRange.last12Months;
						Graph.QtyType=QuantityType.count;
						break;
					default:
						throw new Exception("Unsupported CellType: "+CellType.ToString());
				}
				splitContainerOptions.Panel2.Controls.Clear();
				if(filterCtrl==null) {
					splitContainer.Panel1Collapsed=true;
				}
				else {
					splitContainer.Panel1Collapsed=false;
					splitContainer.SplitterDistance=Math.Max(filterCtrl.GetPanelHeight(),this.groupingOptionsCtrl1.Height);
					filterCtrl.Dock=DockStyle.Fill;
					splitContainerOptions.Panel2.Controls.Add(filterCtrl);
					splitContainerOptions.Panel1Collapsed=!filterCtrl.HasGroupOptions;
				}
			}
		}
		public GroupingOptionsCtrl.Grouping CurGrouping
		{
			get
			{
				return groupingOptionsCtrl1.CurGrouping;
			}
			set
			{
				groupingOptionsCtrl1.CurGrouping=value;
			}
		}
		#endregion

		#region Ctor
		public GraphQuantityOverTimeFilter() :this (DashboardCellType.ProductionGraph) { }

		public GraphQuantityOverTimeFilter(DashboardCellType cellType,string jsonSettings="") {
			InitializeComponent();
			//We will turn IsLoading off elsewhere but make sure it is set here to prevent trying to perform FilterData() to soon.
			Graph.IsLoading=true;
			//Important that CellType is set before other properties as it gives default view.
			CellType=cellType;
			ShowFilters=false;
			Graph.LegendDock=LegendDockType.None;
			SetFilterAndGraphSettings(jsonSettings);
			_incomeOptionsCtrl.InputsChanged+=OnFormInputsChanged;
			_productionOptionsCtrl.InputsChanged+=OnFormInputsChanged;
			_brokenApptsCtrl.InputsChanged+=OnFormInputsChanged;
		}
		#endregion

		#region ODGraphFilterAbs Overrides
		protected override void OnInitDone(object sender,EventArgs e) {
			FilterData();
		}

		protected override void OnInit(bool forceCachRefresh) {
			//This is occurring in a thread so it is ok to wait for Refresh to return. The UI is already loading and available on the main thread.
			DashboardCache.RefreshCellTypeIfInvalid(CellType,Graph.Filter,true,forceCachRefresh,null);
		}
		#endregion
				
		#region Form Events
		private void OnFormInputsChanged(object sender,EventArgs e) {
			FilterData();
		}

		private void FilterData() {
			if(Graph.IsLoading) {
				//We will get 1 final event when the data is loaded and the graph is no longer in 'Loading' mode. 
				//Until then, don't bother with this resource heavy method.
				return;
			}
			List<GraphQuantityOverTime.GraphPointBase> rawData=new List<GraphQuantityOverTime.GraphPointBase>();
			//Fill the dataset that we will send to the graph. The dataset will be filled according to user preferences.
			switch(CellType) {
				case DashboardCellType.ProductionGraph: {
						SetGroupItems(CurGrouping);
						if(_productionOptionsCtrl.IncludeAdjustments) {
							rawData.AddRange(DashboardCache.Adjustments.Cache.Select(x => GetDataPointForGrouping(x,CurGrouping)));
						}
						if(_productionOptionsCtrl.IncludeCompletedProcs) {
							rawData.AddRange(DashboardCache.CompletedProcs.Cache.Select(x => GetDataPointForGrouping(x,CurGrouping)));
							rawData.AddRange(DashboardCache.Writeoffs.Cache.Where(x => x.IsCap==true).Select(x => GetDataPointForGrouping(x,CurGrouping)));
						}
						if(_productionOptionsCtrl.IncludeWriteoffs) {
							rawData.AddRange(DashboardCache.Writeoffs.Cache.Where(x => x.IsCap==false).Select(x => GetDataPointForGrouping(x,CurGrouping)));
						}
					}
					break;
				case DashboardCellType.IncomeGraph: {
						SetGroupItems(CurGrouping);
						if(_incomeOptionsCtrl.IncludePaySplits) {
							rawData.AddRange(DashboardCache.PaySplits.Cache.Select(x => GetDataPointForGrouping(x,CurGrouping)));
						}
						if(_incomeOptionsCtrl.IncludeInsuranceClaimPayments) {
							rawData.AddRange(DashboardCache.ClaimPayments.Cache.Select(x => GetDataPointForGrouping(x,CurGrouping)));
						}
					}
					break;
				case DashboardCellType.AccountsReceivableGraph: {
						rawData.AddRange(DashboardCache.AR.Cache
							.Select(x => new GraphQuantityOverTime.GraphPointBase() {
								Val=x.BalTotal,
								Count=0,
								SeriesName="All",
								DateStamp=x.DateCalc,
							})
							.ToList());
					}
					break;
				case DashboardCellType.NewPatientsGraph: {
						SetGroupItems(CurGrouping);
						rawData.AddRange(DashboardCache.Patients.Cache.Select(x => GetDataPointForGrouping(x,CurGrouping)));
					}
					break;
				case DashboardCellType.BrokenApptGraph: {
						SetGroupItems(CurGrouping);
						switch(_brokenApptsCtrl.CurRunFor) {
							case BrokenApptGraphOptionsCtrl.RunFor.appointment:
								//money is not used when counting appointments
								Graph.RemoveQuantityType(QuantityType.money);
								//use the broken appointment cache to get all relevant broken appts.
								rawData.AddRange(DashboardCache.BrokenAppts.Cache.Select(x => GetDataPointForGrouping(x,CurGrouping)));
								break;
							case BrokenApptGraphOptionsCtrl.RunFor.adjustment:
								//money should be added back in case the user looked at appointments beforehand. 
								Graph.InsertQuantityType(QuantityType.money,"Fees",0);
								//use the broken adjustment cache to get all broken adjustments filtered by the selected adjType.
								rawData.AddRange(DashboardCache.BrokenAdjs.Cache.Where(x => x.AdjType==_brokenApptsCtrl.AdjTypeDefNumCur).Select(x => GetDataPointForGrouping(x,CurGrouping)));
								break;
							case BrokenApptGraphOptionsCtrl.RunFor.procedure:
								Graph.InsertQuantityType(QuantityType.money,"Fees",0);
								//use the broken proc cache to get all relevant broken procedures.
								rawData.AddRange(DashboardCache.BrokenProcs.Cache.Select(x => GetDataPointForGrouping(x,CurGrouping)));
								break;
							default:
								throw new Exception("Unsupported CurRunFor: "+_brokenApptsCtrl.CurRunFor.ToString());
						}
					}
					break;
				default:
					throw new Exception("Unsupported CellType: "+CellType.ToString());
			}
			CommitData(rawData);
		}

		private void SetGroupItems(GroupingOptionsCtrl.Grouping CurGrouping) {
			switch(CurGrouping) {
				case GroupingOptionsCtrl.Grouping.provider:
					Graph.UseBuiltInColors=false;
					Graph.LegendTitle="Provider";
					break;
				case GroupingOptionsCtrl.Grouping.clinic:
					Graph.LegendTitle="Clinic";
					Graph.UseBuiltInColors=true;
					break;
				default:
					Graph.LegendTitle="Group";
					Graph.UseBuiltInColors=true;
					break;
			}
		}

		private GraphQuantityOverTime.GraphDataPointClinic GetDataPointForGrouping(GraphQuantityOverTime.GraphDataPointClinic x,GroupingOptionsCtrl.Grouping curGrouping) {
			switch(curGrouping) {
				case GroupingOptionsCtrl.Grouping.provider:
					return new GraphQuantityOverTime.GraphDataPointClinic() {
						DateStamp=x.DateStamp,
						SeriesName=DashboardCache.Providers.GetProvName(x.ProvNum),
						Val=x.Val,
						Count=x.Count
					};
				case GroupingOptionsCtrl.Grouping.clinic:
				default:
					return new GraphQuantityOverTime.GraphDataPointClinic() {
						DateStamp=x.DateStamp,
						SeriesName=DashboardCache.Clinics.GetClinicName(x.ClinicNum),
						Val=x.Val,
						Count=x.Count
					};
			}
		}

		private Color graph_OnGetGetColor(string seriesName) {
			switch(CellType) {
				case DashboardCellType.BrokenApptGraph:
				case DashboardCellType.NewPatientsGraph:
				case DashboardCellType.AccountsReceivableGraph:
				case DashboardCellType.IncomeGraph:
				case DashboardCellType.ProductionGraph:
				case DashboardCellType.NotDefined:
				default:
					return DashboardCache.Providers.GetProvColor(seriesName);
			}
			
		}
		#endregion

		#region GraphDataPoint Conversions
		private GraphQuantityOverTime.GraphPointBase GetBrokenApptDataPoint(GraphQuantityOverTime.GraphDataPointClinic x) {
			switch(CurGrouping) {
				case GroupingOptionsCtrl.Grouping.provider:
					return new GraphQuantityOverTime.GraphPointBase() {
						DateStamp=x.DateStamp,
						SeriesName=DashboardCache.Providers.GetProvName(x.ProvNum),
						Val=x.Val,
						Count=x.Count
					};
				case GroupingOptionsCtrl.Grouping.clinic:
				default:
					return new GraphQuantityOverTime.GraphPointBase() {
						DateStamp=x.DateStamp,
						SeriesName=DashboardCache.Clinics.GetClinicName(x.ClinicNum),
						Val=x.Val,
						Count=x.Count
					};
			}
		}

		#endregion

		#region IDashboardDockContainer Implementation
		public override DashboardDockContainer CreateDashboardDockContainer(TableBase dbItem=null) {
			string json="";
			DashboardDockContainer ret=new DashboardDockContainer(
				this,
				this.Graph,				
				CanEdit?new EventHandler((s,ea) => {
					//Entering edit mode. 
					//Set graph to loading mode to show the loading icon.
					Graph.IsLoading=true;
					//Spawn the cache thread(s) but don't block. 
					//Register for OnThreadExitLocal will invoke back to this thread when all the threads have exited and refill the form.
					DashboardCache.RefreshCellTypeIfInvalid(CellType,new DashboardFilter() { UseDateFilter=false },false,false,OnThreadExitLocal);
					//Allow filtering in edit mode.
					this.ShowFilters=true;
					//Save a copy of the current settings in case user clicks cancel.
					json=GetFilterAndGraphSettings();
				}):null,
				new EventHandler((s,ea) => {
					//Ok click. Just hide the filters.
					this.ShowFilters=false;
				}),
				new EventHandler((s,ea) => {
					//Cancel click. Just hide the filters and reset to previous settings.
					this.ShowFilters=false;
					SetFilterAndGraphSettings(json);
				}),
				new EventHandler((s,ea) => {
					//Spawn the init thread whenever this control gets dropped or dragged.
					Init();
				}),
				new EventHandler((s,ea) => {
					//Refresh button was clicked, spawn the init thread and force cache refresh.
					Init(true);
				}),
				dbItem);
			return ret;
		}

		public override DashboardCellType GetCellType() {
			return CellType;
		}

		///<summary>This class should NEVER change variable names after being released. 
		///The variable names are stored as plain-text json and need to remain back-compatible.
		///Adding new fields is OK.</summary>
		public class GraphQuantityOverTimeFilterSettings:ODGraphSettingsAbs {
			public bool IncludeCompleteProcs { get; set; }
			public bool IncludeAdjustements { get; set; }
			public bool IncludeWriteoffs { get; set; }
			public bool IncludePaySplits { get; set; }
			public bool IncludeInsuranceClaims { get; set; }
			public GroupingOptionsCtrl.Grouping CurGrouping { get;set;}
			public BrokenApptGraphOptionsCtrl.RunFor CurRunFor { get; set; }
			public long AdjTypeDefNum { get; set; }
			
		}
		#endregion

		#region IODHasGraphSettings Implementation
		public override GraphQuantityOverTimeFilterSettings GetGraphSettings() {
			switch(CellType) {
				case DashboardCellType.ProductionGraph:
					return new GraphQuantityOverTimeFilterSettings() {
						IncludeAdjustements=_productionOptionsCtrl.IncludeAdjustments,
						IncludeCompleteProcs=_productionOptionsCtrl.IncludeCompletedProcs,
						IncludeWriteoffs=_productionOptionsCtrl.IncludeWriteoffs,
						CurGrouping=this.CurGrouping,
					};
				case DashboardCellType.IncomeGraph:
					return new GraphQuantityOverTimeFilterSettings() {
						IncludePaySplits=_incomeOptionsCtrl.IncludePaySplits,
						IncludeInsuranceClaims=_incomeOptionsCtrl.IncludeInsuranceClaimPayments,
						CurGrouping=this.CurGrouping,
					};
				case DashboardCellType.BrokenApptGraph:
					return new GraphQuantityOverTimeFilterSettings() {
						CurGrouping=this.CurGrouping,
						CurRunFor=_brokenApptsCtrl.CurRunFor,
					};
				case DashboardCellType.NewPatientsGraph:
					return new GraphQuantityOverTimeFilterSettings() {
						CurGrouping=this.CurGrouping,
					};
				case DashboardCellType.AccountsReceivableGraph:
					//No custom filtering so do nothing.
					return new GraphQuantityOverTimeFilterSettings();
				default:
					throw new Exception("Unsupported CellType: "+CellType.ToString());
			}
		}

		public override void DeserializeFromJson(string json) {
			try {
				if(string.IsNullOrEmpty(json)) {
					return;
				}
				GraphQuantityOverTimeFilterSettings settings=ODGraphSettingsAbs.Deserialize<GraphQuantityOverTimeFilterSettings>(json);
				switch(CellType) {
					case DashboardCellType.ProductionGraph:
						_productionOptionsCtrl.IncludeAdjustments=settings.IncludeAdjustements;
						_productionOptionsCtrl.IncludeCompletedProcs=settings.IncludeCompleteProcs;
						_productionOptionsCtrl.IncludeWriteoffs=settings.IncludeWriteoffs;
						this.CurGrouping=settings.CurGrouping;
						break;
					case DashboardCellType.IncomeGraph:
						_incomeOptionsCtrl.IncludePaySplits=settings.IncludePaySplits;
						_incomeOptionsCtrl.IncludeInsuranceClaimPayments=settings.IncludeInsuranceClaims;
						this.CurGrouping=settings.CurGrouping;
						break;
					case DashboardCellType.BrokenApptGraph:
						this.CurGrouping=settings.CurGrouping;
						_brokenApptsCtrl.CurRunFor=settings.CurRunFor;
						break;
					case DashboardCellType.NewPatientsGraph:
						this.CurGrouping=settings.CurGrouping;
						break;
					case DashboardCellType.AccountsReceivableGraph:
						//No custom filtering so do nothing.
						return;
					default:
						throw new Exception("Unsupported CellType: "+CellType.ToString());
				}
			}
			catch(Exception e) {
#if DEBUG
				MessageBox.Show(e.Message);
#endif
			}
		}
		#endregion
	}

	///<summary>Intermediate concreate to allow final concrete to play nicely with designer.</summary>
	public class GraphQuantityOverTimeFilterT:ODGraphFilterAbs<
		GraphQuantityOverTime.GraphPointBase,
		GraphQuantityOverTimeFilter.GraphQuantityOverTimeFilterSettings,
		GraphQuantityOverTime.QuantityOverTimeGraphSettings> {

		public override GraphQuantityOverTimeFilter.GraphQuantityOverTimeFilterSettings GetGraphSettings() {
			throw new NotImplementedException();
		}
		
		public override void DeserializeFromJson(string json) {
			throw new NotImplementedException();
		}

		public override DashboardDockContainer CreateDashboardDockContainer(OpenDentBusiness.TableBase dbItem) {
			throw new NotImplementedException();
		}

		public override DashboardCellType GetCellType() {
			throw new NotImplementedException();
		}
	}

}
