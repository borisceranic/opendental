using OpenDentBusiness;
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
		//public enum ShowDataType { production, income, ar, newPatients };
		private DashboardCellType _dispalyDataType=DashboardCellType.NotDefined;
		//private DashboardCacheNewPatient _cacheNewPatients=new DashboardCacheNewPatient();
		//private DashboardCacheCompletedProc _cacheCompletedProcs=new DashboardCacheCompletedProc();
		//private DashboardCacheWriteoff _cacheWriteoffs=new DashboardCacheWriteoff();
		//private DashboardCacheAdjustment _cacheAdjustments=new DashboardCacheAdjustment();
		//private DashboardCachePaySplit _cachePaySplits=new DashboardCachePaySplit();
		//private DashboardCacheClaimPayment _cacheClaimPayments=new DashboardCacheClaimPayment();
		//private DashboardCacheAR _cacheAR=new DashboardCacheAR();
		//private DashboardCacheProvider _cacheProvider=new DashboardCacheProvider();
		#endregion

		#region Properties
		public bool CanEdit {
			get {
				switch(CellType) {
					case DashboardCellType.ProductionGraph:
					case DashboardCellType.IncomeGraph:
					case DashboardCellType.NewPatientsGraph:
						return true;
					case DashboardCellType.AccountsReceivableGraph:
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
						splitContainer.Panel1Collapsed=!Graph.ShowFilters;
						break;
					case DashboardCellType.AccountsReceivableGraph:
					case DashboardCellType.NewPatientsGraph:
						break;					
				}
			}
		}
		[Category("Graph")]
		public DashboardCellType CellType {
			get { return _dispalyDataType; }
			set {
				_dispalyDataType=value;
				switch(CellType) {
					case DashboardCellType.ProductionGraph:
						splitContainerProductionIncome.Panel1Collapsed=false;
						splitContainerProductionIncome.Panel2Collapsed=true;
						Graph.GraphTitle="Production";
						Graph.MoneyItemDescription="Production $";
						Graph.CountItemDescription="Count Procedures";
						Graph.GroupByType=System.Windows.Forms.DataVisualization.Charting.IntervalType.Months;
						Graph.SeriesType=System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
						Graph.BreakdownPref=BreakdownType.none;
						Graph.LegendDock=LegendDockType.None;
						Graph.QuickRangePref=QuickRange.last12Months;
						break;
					case DashboardCellType.IncomeGraph:
						splitContainerProductionIncome.Panel1Collapsed=true;
						splitContainerProductionIncome.Panel2Collapsed=false;
						Graph.GraphTitle="Income";
						Graph.MoneyItemDescription="Income $";
						Graph.CountItemDescription="Count Payments";
						Graph.GroupByType=System.Windows.Forms.DataVisualization.Charting.IntervalType.Months;
						Graph.SeriesType=System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
						Graph.BreakdownPref=BreakdownType.none;
						Graph.LegendDock=LegendDockType.None;
						Graph.QuickRangePref=QuickRange.last12Months;
						break;
					case DashboardCellType.AccountsReceivableGraph:
						splitContainer.Panel1Collapsed=true;
						Graph.GraphTitle="Accounts Receivable";
						Graph.MoneyItemDescription="Receivable $";
						Graph.CountItemDescription="Not Used";
						Graph.GroupByType=System.Windows.Forms.DataVisualization.Charting.IntervalType.Months;
						Graph.SeriesType=System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
						Graph.BreakdownPref=BreakdownType.none;
						Graph.LegendDock=LegendDockType.None;
						Graph.QuickRangePref=QuickRange.last12Months;
						break;
					case DashboardCellType.NewPatientsGraph:
						splitContainer.Panel1Collapsed=true;
						Graph.GraphTitle="New Patients";
						Graph.MoneyItemDescription="Not Used";
						Graph.CountItemDescription="Count Patients";
						Graph.GroupByType=System.Windows.Forms.DataVisualization.Charting.IntervalType.Months;
						Graph.SeriesType=System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
						Graph.BreakdownPref=BreakdownType.none;
						Graph.LegendDock=LegendDockType.None;
						Graph.QuickRangePref=QuickRange.last12Months;
						Graph.QtyType=QuantityType.count;
						break;
					default:
						throw new Exception("Unsupported DisplayDataType: "+CellType.ToString());
				}
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
			switch(CellType) {
				case DashboardCellType.ProductionGraph: {
						if(checkIncludeAdjustments.Checked) {
							rawData.AddRange(DashboardCache.Adjustments.Cache.FindAll(x => x.DateStamp.Year>1880)
								.Select(x => new GraphQuantityOverTime.GraphPointBase(x) { SeriesName=DashboardCache.Provider.GetProvName(x.ProvNum) })
								.ToList());
						}
						if(checkIncludeCompletedProcs.Checked||checkIncludeWriteoffs.Checked) {
							rawData.AddRange(DashboardCache.CompletedProcs.Cache
								.Select(x => GetProcDataPoint(x))
								.ToList());
						}
					}
					break;
				case DashboardCellType.IncomeGraph: {
						if(checkIncludePaySplits.Checked) {
							rawData.AddRange(DashboardCache.PaySplits.Cache.FindAll(x => x.DateStamp.Year>1880)
								.Select(x => new GraphQuantityOverTime.GraphPointBase(x){ SeriesName=DashboardCache.Provider.GetProvName(x.ProvNum) })
								.ToList());
						}
						if(checkIncludeInsuranceClaimPayments.Checked) {
							rawData.AddRange(DashboardCache.ClaimPayments.Cache
								.Select(x => new GraphQuantityOverTime.GraphPointBase(x) { SeriesName=DashboardCache.Provider.GetProvName(x.ProvNum) })
								.ToList());
						}
					}
					break;
				case DashboardCellType.AccountsReceivableGraph: {
						rawData.AddRange(DashboardCache.AR.Cache.FindAll(x => x.DateCalc.Year>1880)
							.Select(x => GetARDataPoint(x))
							.ToList());
					}
					break;
				case DashboardCellType.NewPatientsGraph: {
						rawData.AddRange(DashboardCache.Patients.Cache.FindAll(x => x.DateStamp.Year>1880));
					}
					break;
				default:
					throw new Exception("Unsupported DisplayDataType: "+CellType.ToString());
			}
			CommitData(rawData);
		}

		private Color graph_OnGetGetColor(string seriesName) {
			return DashboardCache.Provider.GetProvColor(seriesName);
		}
		#endregion

		#region GraphDataPoint Conversions
		private GraphQuantityOverTime.GraphPointBase GetARDataPoint(DashboardAR x) {
			return new GraphQuantityOverTime.GraphPointBase() {
				Val=x.BalTotal,
				SeriesName="All",
				DateStamp=x.DateCalc,
			};
		}

		private GraphQuantityOverTime.GraphPointBase GetProcDataPoint(CompletedProc x) {
			double finalValue=0;
			if(checkIncludeCompletedProcs.Checked) {
				finalValue+=x.Val;
			}
			if(checkIncludeWriteoffs.Checked) {
				Writeoff writeoff=DashboardCache.Writeoffs.Cache.FirstOrDefault(y => y.ProvNum==x.ProvNum && y.DateStamp==x.DateStamp);
				if(writeoff!=null) {
					finalValue-=writeoff.Val;
				}
			}
			return new GraphQuantityOverTime.GraphPointBase() {
				Val=finalValue,
				SeriesName=DashboardCache.Provider.GetProvName(x.ProvNum),
				DateStamp=x.DateStamp,
			};
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
		
		public class ProductionGraphSettings:ODGraphSettingsAbs {
			public bool IncludeCompleteProcs { get; set; }
			public bool IncludeAdjustements { get; set; }
			public bool IncludeWriteoffs { get; set; }
			public bool IncludePaySplits { get; set; }
			public bool IncludeInsuranceClaims { get; set; }
		}		
		#endregion

		#region IODHasGraphSettings Implementation
		public override ProductionGraphSettings GetGraphSettings() {
			return new ProductionGraphSettings() {
				IncludeAdjustements=checkIncludeAdjustments.Checked,
				IncludeCompleteProcs=checkIncludeCompletedProcs.Checked,
				IncludeWriteoffs=checkIncludeWriteoffs.Checked,
				IncludePaySplits=checkIncludePaySplits.Checked,
				IncludeInsuranceClaims=checkIncludeInsuranceClaimPayments.Checked,
			};
		}
				
		public override void DeserializeFromJson(string json) {
			try {
				if(string.IsNullOrEmpty(json)) {
					return;
				}
				ProductionGraphSettings settings=ODGraphSettingsAbs.Deserialize<ProductionGraphSettings>(json);
				this.checkIncludeAdjustments.Checked=settings.IncludeAdjustements;
				this.checkIncludeCompletedProcs.Checked=settings.IncludeCompleteProcs;
				this.checkIncludeWriteoffs.Checked=settings.IncludeWriteoffs;
				this.checkIncludePaySplits.Checked=settings.IncludePaySplits;
				this.checkIncludeInsuranceClaimPayments.Checked=settings.IncludeInsuranceClaims;				
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
		GraphQuantityOverTimeFilter.ProductionGraphSettings,
		GraphQuantityOverTime.QuantityOverTimeGraphSettings> {

		public override GraphQuantityOverTimeFilter.ProductionGraphSettings GetGraphSettings() {
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
