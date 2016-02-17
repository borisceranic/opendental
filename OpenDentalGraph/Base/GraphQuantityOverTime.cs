using OpenDentalGraph.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using OpenDentalGraph.Enumerations;
using System.Drawing.Printing;

namespace OpenDentalGraph {
	public partial class GraphQuantityOverTime:UserControl, IODGraph<GraphQuantityOverTime.GraphPointBase,GraphQuantityOverTime.QuantityOverTimeGraphSettings> {
		#region Private Data
		private bool _isLoading=true;
		private bool _allowFilter=false;
		private List<GraphPointBase> _rawData=new List<GraphPointBase>();
		private DateTimeIntervalType _selectedMajorGridInterval {
			get {
				return (DateTimeIntervalType)Enum.Parse(typeof(DateTimeIntervalType),GroupByType.ToString(),true);
			}
		}		
		private ChartArea _chartAreaDefault {
			get {
				return chart1.ChartAreas["Default"];
			}
		}

		private string _yAxisFormat="D";
		private string _xAxisFormat {
			get {
				switch(GroupByType) {
					case IntervalType.Years:
						return "\\'yy";
					case IntervalType.Months:
						return QuickRangePref==QuickRange.last12Months?"MM":"MMM \\'yy";
					case IntervalType.Weeks:
					case IntervalType.Days:
						if(DateRange.TotalDays<365) {
							return "MM/dd";
						}
						break;			
				}
				return "MM/dd/yy";
			}
		}
		private string _graphTitle="";
		private List<Tuple<DateTime,DateTime>> _intervals=new List<Tuple<DateTime,DateTime>>();
		private double _yAxisMaxVal=double.NaN;
		private double _yAxisMinVal=double.NaN;
		private bool _isLayoutPending=false;
		private string _legendTitle="";
		private bool _useBuiltInColors=false;
		#endregion

		#region Helper Classes
		public class GraphDataPointClinic:GraphDataPointProv {
			public long ClinicNum;
		}

		public class GraphDataPointProv:GraphPointBase {
			public long ProvNum;
		}

		public class GraphPointBase:ODGraphPointBase {
			public DateTime DateStamp;
			public double Val;
			public long Count=0;//1; //default to having one data point count for one unit when in Count mode.
						
			public GraphPointBase() {
			}

			public GraphPointBase(GraphPointBase dataPoint) {
				SeriesName=dataPoint.SeriesName;
				DateStamp=dataPoint.DateStamp;
				Val=dataPoint.Val;
				Count=dataPoint.Count;
				Tag=dataPoint.Tag;
			}

			public override string ToString() {
				return SeriesName+" "+DateStamp.ToShortDateString()+" - "+Val.ToString();
			}
		}
		#endregion

		#region Public Properties
		[Description("Show or hide the animated 'Loading' gif.")]
		[Category("Graph")]
		public bool IsLoading {
			get { return _isLoading; }
			set {
				_isLoading=value;
				if(IsLoading) {
					pictureBoxLoading.BringToFront();
					chart1.SendToBack();
				}
				else {
					chart1.BringToFront();
					pictureBoxLoading.SendToBack();
				}
			}
		}
		[Description("Used in main chart title, breakdown group box, breakdown combo box and legend.")]
		[Category("Graph")]
		public string GraphTitle {
			get { return _graphTitle; }
			set {
				if(_graphTitle==value) {
					return;
				}
				_graphTitle=value;
				SetGraphTitles();
			}
		}

		[Description("Used in legend title and also to describe series grouping.")]
		[Category("Graph")]
		public string LegendTitle {
			get { return _legendTitle; }
			set { _legendTitle=value; SetGraphTitles(); }
		}

		[Description("Sub-text below main chart area. If empty then sub-text is hidden.")]
		[Category("Graph")]
		public string ChartSubTitle {
			get { return chart1.Titles["ChartSubTitle"].Text; }
			set {
				chart1.Titles["ChartSubTitle"].Text=value;
				chart1.Titles["ChartSubTitle"].Visible=!string.IsNullOrEmpty(value);
			}
		}

		[Description("String shown for Values As = 'Money'.")]
		[Category("Graph")]
		public string MoneyItemDescription {
			get { return comboQuantityType.GetItem<QuantityType>(QuantityType.money).Display; }
			set { comboQuantityType.GetItem<QuantityType>(QuantityType.money).Display=value; }
		}

		[Description("String shown for Values As = 'Count'.")]
		[Category("Graph")]
		public string CountItemDescription {
			get { return comboQuantityType.GetItem<QuantityType>(QuantityType.count).Display; }
			set { comboQuantityType.GetItem<QuantityType>(QuantityType.count).Display=value; }
		}

		[Description("Show Filters")]
		[Category("Graph")]
		public bool ShowFilters {
			get { return !splitContainerMain.Panel1Collapsed; }
			set { splitContainerMain.Panel1Collapsed=!value; }
		}

		[Description("Gets or sets the Chart Title's Font")]
		[Category("Graph")]
		public System.Drawing.Font TitleFont {
			get { return chart1.Titles["ChartTitle"].Font; }
			set { chart1.Titles["ChartTitle"].Font=value; }
		}
		
		[Description("Controls the 'Display' combo box. Change value type used on y-axis.")]
		[Category("Graph")]
		public QuantityType QtyType {
			get { return comboQuantityType.GetValue<QuantityType>(); }
			set { comboQuantityType.SetItem<QuantityType>(value); }
		}

		[Description("Controls the 'Series Type' combo box. Change the way each series is displayed on the chart.")]
		[Category("Graph")]
		///<summary>Supported types: StackedArea,StackedColumn,Column,Line.</summary>
		public SeriesChartType SeriesType {
			get { return comboChartType.GetValue<SeriesChartType>(); }
			set { comboChartType.SetItem<SeriesChartType>(value); }
		}

		[Description("Controls the 'Group By' combo box. Change date groupings.")]
		[Category("Graph")]
		public IntervalType GroupByType {
			get { return comboGroupBy.GetValue<IntervalType>(); }
			set { comboGroupBy.SetItem<IntervalType>(value); }
		}

		[Description("Controls the 'Legend' combo box. Gets or sets Legend Docking")]
		[Category("Graph")]
		public LegendDockType LegendDock {
			get { return chartLegend1.LegendDock; }
			set {
				_isLayoutPending=true;
				chartLegend1.LegendDock=value;
				LayoutChartContainer();
				comboLegendDock.SelectedIndex=(int)value;
				_isLayoutPending=false;
			}
		}

		[Description("Controls the 'Quick Range' combo box. Gets or sets date range to include.")]
		[Category("Graph")]
		public QuickRange QuickRangePref {
			get { return comboQuickRange.GetValue<QuickRange>(); }
			set { comboQuickRange.SetItem<QuickRange>(value); }
		}

		[Description("Controls the 'Top Value' numeric control. Gets or sets number of series to include.")]
		[Category("Graph")]
		public int BreakdownVal {
			get { return (int)numericTop.Value; }
			set { numericTop.Value=value; }
		}

		[Description("Controls the 'Show Breakdown For' group box. Gets or sets number of series to include.")]
		[Category("Graph")]
		public BreakdownType BreakdownPref {
			get {
				if(radBreakdownAll.Checked) {
					return BreakdownType.all;
				}
				else if(radBreakdownNone.Checked) {
					return BreakdownType.none;
				}
				return comboBreakdownBy.GetValue<BreakdownType>();
			}
			set {
				switch(value) {
					case BreakdownType.all:
						radBreakdownAll.Checked=true;
						break;
					case BreakdownType.none:
						radBreakdownNone.Checked=true;
						break;
					case BreakdownType.items:
					case BreakdownType.percent:
						radBreakdownTop.Checked=true;
						radBreakdownTop.Checked=true;
						comboBreakdownBy.SetItem<BreakdownType>(value);
						break;
				}
			}
		}

		[Description("Controls the 'From' filter date.")]
		[Category("Graph")]
		public DateTime DateFrom {
			get {
				return dateTimeFrom.Value;
			}
			set {
				if(value.Year>=1880) {
					dateTimeFrom.Value=value;
				}
			}
		}

		[Description("Controls the 'To' filter date.")]
		[Category("Graph")]
		public DateTime DateTo {
			get {
				return dateTimeTo.Value;
			}
			set {
				if(value.Year>1880) {
					dateTimeTo.Value=value;
				}
			}
		}

		[Description("If true then built-in color palette will be used. If false then value returned by OnGetGetColor will be used for each series.")]
		[Category("Graph")]
		public bool UseBuiltInColors {
			get {
				return _useBuiltInColors;
			}
			set {
				_useBuiltInColors=value;
			}
		}

		[Description("Total absolute value duration between DateFrom and DateTo.")]
		[Category("Graph")]
		public TimeSpan DateRange {
			get {
				return DateFrom.Subtract(DateTo).Duration();
			}
		}

		public Cache.DashboardFilter Filter {
			get {
				return ODGraphBaseSettingsAbs.GetDatesFromQuickRange(this.QuickRangePref,this.DateFrom,this.DateTo);
			}
		}
		#endregion

		#region Ctor
		public GraphQuantityOverTime() {
			InitializeComponent();
			comboChartType.SetDataToEnumsPrimitive<SeriesChartType>(
				new List<SeriesChartType>(new SeriesChartType[] { SeriesChartType.StackedArea,SeriesChartType.StackedColumn,SeriesChartType.Column,SeriesChartType.Line }));
			comboGroupBy.SetDataToEnumsPrimitive<IntervalType>((int)IntervalType.Years,(int)IntervalType.Days);
			comboBreakdownBy.SetDataToEnumsPrimitive<BreakdownType>((int)BreakdownType.items,(int)BreakdownType.percent,new ComboBoxEx.StringFromEnumArgs<BreakdownType>(bt => {
				switch(bt) {
					case BreakdownType.percent:
						return "Percent";
					case BreakdownType.items: //Taken care of in SetGraphTitles().					
					case BreakdownType.all: //Not included.
					case BreakdownType.none: //Not included.
					default:
						return bt.ToString();
				}
			}));
			comboQuantityType.SetDataToEnumsPrimitive<QuantityType>((int)QuantityType.money,(int)QuantityType.count);
			comboLegendDock.SetDataToEnumsPrimitive<LegendDockType>();
			comboQuickRange.SetDataToEnumsPrimitive(new ComboBoxEx.StringFromEnumArgs<QuickRange>(qr => {
				switch(qr) {
					case QuickRange.allTime:
						return "All Time";
					case QuickRange.thisWeek:
						return "This Week";
					case QuickRange.thisMonth:
						return "This Month";
					case QuickRange.thisYear:
						return "This Year";
					case QuickRange.weekToDate:
						return "Week To Date";
					case QuickRange.monthToDate:
						return "Month To Date";
					case QuickRange.yearToDate:
						return "Year To Date";
					case QuickRange.previousWeek:
						return "Previous Week";
					case QuickRange.previousMonth:
						return "Previous Month";
					case QuickRange.previousYear:
						return "Previous Year";
					case QuickRange.last7Days:
						return "Last 7 Days";
					case QuickRange.last30Days:
						return "Last 30 Days";
					case QuickRange.last365Days:
						return "Last 365 Days";
					case QuickRange.last12Months:
						return "Last 12 Months";
					case QuickRange.custom:
						return "Custom";
					default:
						return "N\\A";
				}
			}));
		}
		#endregion

		#region Events
		public event ODGraphDataPointEventHandler DataPointDoubleClick;

		public event EventHandler FilterDataComplete;

		public event OnGetColorArgs OnGetGetColor;

		private void radBreakdown_CheckedChanged(object sender,EventArgs e) {
			if(!((RadioButton)sender).Checked) {
				return;
			}
			FilterData(sender,e);
		}

		private void chart1_GetToolTipText(object sender,ToolTipEventArgs e) {
			switch(e.HitTestResult.ChartElementType) {
				case ChartElementType.DataPoint:
					Series series=e.HitTestResult.Series;
					string prefix="";
					if(QtyType==QuantityType.money) {
						prefix="$";
					}					
					e.Text=series.Name+" - "+DateTime.FromOADate(series.Points[e.HitTestResult.PointIndex].XValue).ToShortDateString()+" - "+prefix+Math.Round(series.Points[e.HitTestResult.PointIndex].YValues[0],2);
					break;
			}
		}

		private void chart1_MouseDoubleClick(object sender,MouseEventArgs e) {
			HitTestResult result = chart1.HitTest(e.X,e.Y);
			if(result.ChartElementType!=ChartElementType.DataPoint) {
				return;
			}
			if(DataPointDoubleClick!=null) {
				DataPoint point=result.Series.Points[result.PointIndex];
				GraphPointBase dp=new GraphPointBase() {
					DateStamp=point==null?DateTime.Now:DateTime.FromOADate(point.XValue),
					Val=point==null?0:point.YValues[0],
					SeriesName=result.Series.Name,
					Tag=result.Series.Tag??new object()
				};
				DataPointDoubleClick(this,dp);
			}
		}

		private void comboQuickRange_SelectedIndexChanged(object sender,EventArgs e) {
			_allowFilter=false;
			if(QuickRangePref==QuickRange.custom) {
				dateTimeTo.Enabled=true;
				dateTimeFrom.Enabled=true;
			}
			else {
				dateTimeTo.Enabled=false;
				dateTimeFrom.Enabled=false;
				Cache.DashboardFilter filter=Filter;
				DateFrom=filter.DateFrom;
				DateTo=filter.DateTo;
			}
			if(_rawData.Count<=0) {
				chart1.Series.Clear();
				return;
			}
			_allowFilter=true;
			FilterData(sender,e);
		}

		private void comboLegendDock_SelectedIndexChanged(object sender,EventArgs e) {
			if(_isLayoutPending) {
				return;
			}
			LegendDock=comboLegendDock.GetValue<LegendDockType>();
		}

		private void ChartQuantityOverTime_Resize(object sender,EventArgs e) {
			SetXAxisLabelDensity();
		}
		
		private void textChartTitle_TextChanged(object sender,EventArgs e) {
			GraphTitle=textChartTitle.Text;
		}
		#endregion
				
		#region Helpers
		private void AddSeries(string name,Dictionary<DateTime,double> filteredData,object tag) {
			Series series=new Series(name);
			series.Tag=tag;
			series.ChartArea=_chartAreaDefault.Name;
			series.ChartType=SeriesType;
			if(!UseBuiltInColors && OnGetGetColor!=null) {
				series.Color=OnGetGetColor(name);
			}
			//series.Legend="Default";			
			series.Points.DataBindXY(filteredData.Keys,filteredData.Values);
			chart1.Series.Add(series);
		}

		private void FilterData(object sender,EventArgs e) {
			if(!_allowFilter) {
				return;
			}
			switch(BreakdownPref) {
				case Enumerations.BreakdownType.all:
				case Enumerations.BreakdownType.none:
					numericTop.Enabled=false;
					comboBreakdownBy.Enabled=false;
					break;
				case Enumerations.BreakdownType.items:
				case Enumerations.BreakdownType.percent:
					numericTop.Enabled=true;
					comboBreakdownBy.Enabled=true;
					break;
			}
			chart1.Series.Clear();
			if(_rawData.Count<=0) {
				return;
			}
			List<GraphPointBase> rawDataLocal;
			switch(QtyType) {
				case Enumerations.QuantityType.count: //YVal in count mode will come from Count instead of Val.
					rawDataLocal=_rawData.Select(x => new GraphPointBase(x) { Val=x.Count }).ToList();
					break;
				case Enumerations.QuantityType.money: //YVal will come from the Val. Nothing is changed.
					rawDataLocal=_rawData.Select(x => new GraphPointBase(x)).ToList();
					break;
				default:
					throw new Exception("Unsupported QtyType.");
			}
			//1 grouping for each series name.
			List<string> seriesNames=rawDataLocal.Select(x => x.SeriesName).Distinct().ToList();
			//Project into a data types that are suitable for grouping and sorting.
			var seriesTags=rawDataLocal.GroupBy(x => x.SeriesName).ToDictionary(x => x.Key,x => x.First().Tag);
			var rawDataFormatted=seriesNames.Select(x => new { SeriesName=x,Dict=new Dictionary<DateTime,double>(),Tag=seriesTags[x] }).ToDictionary(x=>x.SeriesName,x=>x);
			_intervals.Clear();
			DateTime thisDate=DateFrom;
			DateTime toDate=DateTo;
			if(QuickRangePref==Enumerations.QuickRange.allTime) {
				if(rawDataLocal.Count>0) {
					thisDate=rawDataLocal.Min(x => x.DateStamp).Date;
					toDate=rawDataLocal.Max(x => x.DateStamp).Date;
					_allowFilter=false;
					DateFrom=thisDate;
					DateTo=toDate;
					_allowFilter=true;
				}
			}
			//Move start and end date according to filter and grouping selections.
			switch(GroupByType) {
				case IntervalType.Weeks:
					//Start at Sunday before lower bound date.
					thisDate=thisDate.AddDays(-(int)thisDate.DayOfWeek);
					//End at first Sunday following the upper bound date.						
					toDate=toDate.AddDays((7-(int)toDate.DayOfWeek)%7);
					break;
				case IntervalType.Months:
					//Start at 1st of lower bound month.
					thisDate=new DateTime(thisDate.Year,thisDate.Month,1);
					//Send at 1st of month after upper bound month.
					toDate=new DateTime(toDate.Year,toDate.Month,1).AddMonths(1);
					break;
				case IntervalType.Years:
					//Start at 1st of lower bound month.
					thisDate=new DateTime(thisDate.Year,1,1);
					//Send at 1st of month after upper bound month.
					toDate=new DateTime(toDate.Year,1,1).AddYears(1);
					break;
				case IntervalType.Days:
				default:
					//Start at day after lower bound date.
					toDate=toDate.AddDays(1);
					//End at upper bound date (no change).
					break;
			}
			//Create timespan intervals according to selected grouping.
			while(thisDate<toDate) {
				switch(GroupByType) {
					case IntervalType.Weeks:
						_intervals.Add(new Tuple<DateTime,DateTime>(thisDate,thisDate.AddDays(7)));
						thisDate=thisDate.AddDays(7);
						break;
					case IntervalType.Months:
						_intervals.Add(new Tuple<DateTime,DateTime>(thisDate,thisDate.AddMonths(1)));
						thisDate=thisDate.AddMonths(1);
						break;
					case IntervalType.Years:
						_intervals.Add(new Tuple<DateTime,DateTime>(thisDate,thisDate.AddYears(1)));
						thisDate=thisDate.AddYears(1);
						break;
					case IntervalType.Days:
					default:
						_intervals.Add(new Tuple<DateTime,DateTime>(thisDate,thisDate.AddDays(1)));
						thisDate=thisDate.AddDays(1);
						break;
				}
			}
			if(_intervals.Count<=0) {
				return;
			}
			//Initialize eache interval in each series to 0. We will set actual values below. This ensures that are omitted dates are zeroed.
			foreach(var kvp in rawDataFormatted) {
				_intervals.ForEach(y => kvp.Value.Dict[y.Item1]=0);
			}
			//Each interval is given a bucket and each bucket is the sum of all y-values at the given x-value interval. 
			//This will be used to determine max y-axis value for the entire chart.
			Dictionary<DateTime,double> coutingBuckets=new Dictionary<DateTime,double>();
			_intervals.ForEach(y => {
				coutingBuckets[y.Item1]=0;
				foreach(var kvp in rawDataFormatted) {
					kvp.Value.Dict[y.Item1]=0;
				}
			});
			//Now that we have established intervals, we can project the raw data into grouped DataPoints.
			//The sum of all the DataPoints per a given group will be added to that single group.
			List<GraphPointBase> groupedData=new List<GraphPointBase>();
			switch(GroupByType) {
				case IntervalType.Weeks:
					groupedData=rawDataLocal
						.FindAll(x => x.DateStamp>=_intervals[0].Item1&&x.DateStamp<_intervals[_intervals.Count-1].Item2)
						.Select(x =>
							new GraphPointBase() {
								SeriesName=x.SeriesName,
								DateStamp=x.DateStamp.Date.AddDays(-(int)x.DateStamp.DayOfWeek),
								Tag=x.Tag,
								Val=x.Val
							}).ToList();
					break;
				case IntervalType.Months:
					groupedData=rawDataLocal
						.FindAll(x => x.DateStamp>=_intervals[0].Item1&&x.DateStamp<_intervals[_intervals.Count-1].Item2)
						.Select(x =>
							new GraphPointBase() {
								SeriesName=x.SeriesName,
								DateStamp=new DateTime(x.DateStamp.Year,x.DateStamp.Month,1),
								Tag=x.Tag,
								Val=x.Val
							}).ToList();
					break;
				case IntervalType.Years:
					groupedData=rawDataLocal
						.FindAll(x => x.DateStamp>=_intervals[0].Item1&&x.DateStamp<_intervals[_intervals.Count-1].Item2)
						.Select(x =>
							new GraphPointBase() {
								SeriesName=x.SeriesName,
								DateStamp=new DateTime(x.DateStamp.Year,1,1),
								Tag=x.Tag,
								Val=x.Val
							}).ToList();
					break;
				case IntervalType.Days:
				default:
					groupedData=rawDataLocal
						.FindAll(x => x.DateStamp>=_intervals[0].Item1&&x.DateStamp<_intervals[_intervals.Count-1].Item2)
						.Select(x =>
							new GraphPointBase() {
								SeriesName=x.SeriesName,
								DateStamp=x.DateStamp.Date,
								Tag=x.Tag,
								Val=x.Val
							}).ToList();
					break;
			}
			//Now that we have grouped data we can add up the y-axis values of each bucket.
			groupedData.ForEach(x => {
				rawDataFormatted[x.SeriesName].Dict[x.DateStamp]+=x.Val;
				coutingBuckets[x.DateStamp]+=x.Val;
			});
			//Get the max and min y-value for the entire chart.
			if(rawDataFormatted.Count>0) {
				_yAxisMaxVal=coutingBuckets.Values.Max();
				_yAxisMinVal=coutingBuckets.Values.Min();
			}
			//Filter out by selected _breakdownType.
			int topGrossingCount=rawDataFormatted.Count;
			if(BreakdownPref==Enumerations.BreakdownType.items) { //Order descending and take top x items.
				topGrossingCount=(int)numericTop.Value;
			}
			else if(BreakdownPref==Enumerations.BreakdownType.percent) {  //Order descending and take top by percentage.				
				topGrossingCount=(int)Math.Ceiling(rawDataFormatted.Count*(numericTop.Value/(decimal)100));
			}
			else if(BreakdownPref==Enumerations.BreakdownType.none) {  //Don't take any. This will leave all to be grouped into 1 single group below.
				topGrossingCount=0;
			}
			topGrossingCount=Math.Min(topGrossingCount,100);
			//Add each individual series that is withing the filtered range.
			rawDataFormatted.OrderByDescending(x => x.Value.Dict.Sum(y => y.Value))
				.Take(topGrossingCount).ToList()
				.ForEach(x => {
					AddSeries(x.Key,x.Value.Dict,x.Value.Tag);
				});
			//If necessary, add 1 single series to sum up all remaining series.
			if(rawDataFormatted.Count>topGrossingCount) {
				var item=rawDataFormatted.OrderByDescending(x => x.Value.Dict.Sum(y => y.Value))
					.ElementAt(topGrossingCount);
				rawDataFormatted.OrderByDescending(x => x.Value.Dict.Sum(y => y.Value))
					.Skip(topGrossingCount+1).ToList()
					.ForEach(x => {
						foreach(KeyValuePair<DateTime,double> kvp in x.Value.Dict) {
							item.Value.Dict[kvp.Key]+=kvp.Value;
						}
					});
				//This is the 'All Other' series, everything that wasn't included individually above will be grouped into 1 single series.
				AddSeries(BreakdownPref==Enumerations.BreakdownType.none ? "All" : "All Other",item.Value.Dict,new object());
				//Include this final series in a our max and min y-axis check.
				_yAxisMaxVal=Math.Max(_yAxisMaxVal,item.Value.Dict.Values.Max());
				_yAxisMinVal=Math.Min(_yAxisMinVal,item.Value.Dict.Values.Min());
			}
			_chartAreaDefault.AxisX.IntervalType=_selectedMajorGridInterval;
			//Allowing MSChart to calculate AxisY.Maximum (via RecalculateAxesScale) causes a second rendering of the chart which slows it down significantly.
			//We will calculate the max ourselves.
			switch(SeriesType) {
				case SeriesChartType.StackedArea:
				case SeriesChartType.StackedColumn:
					_yAxisMaxVal=coutingBuckets.Values.Max();
					_yAxisMinVal=coutingBuckets.Values.Min();
					break;
			}
			//Round up.
			_yAxisMaxVal=_yAxisMaxVal.RoundSignificant();
			_chartAreaDefault.AxisX.IntervalOffsetType=_selectedMajorGridInterval;
			_chartAreaDefault.AxisX.LabelStyle.Format=_xAxisFormat;
			if(_yAxisMaxVal<=0) {
				_yAxisMaxVal=0;
			}
			else if(_yAxisMaxVal<10){
				_yAxisMaxVal=10;
			}
			_yAxisMinVal=_yAxisMinVal.RoundSignificant();
			_yAxisMinVal=_yAxisMinVal>=0?0:_yAxisMinVal;
			double yAxisMinMax=Math.Max(Math.Abs(_yAxisMaxVal),Math.Abs(_yAxisMinVal));
			if(yAxisMinMax==0) {
				_yAxisFormat="D";
			}
			else {
				int max=(int)yAxisMinMax;				
				string scaleFactor="".PadRight((max.ToString().Length-2)/3,',');
				string[] factors=new[] { "","K","M","B","T","?" };				
				if(yAxisMinMax>=10) {
					_yAxisFormat="{0:0"+scaleFactor+"}"+factors[Math.Min(factors.Length-1,scaleFactor.Length)];
				}
				else {					
					_yAxisFormat=QtyType==QuantityType.count? "{0:0}" : "{0:0.0}";
				}
				if(this.QtyType==Enumerations.QuantityType.money) {
					_yAxisFormat="$"+_yAxisFormat;
				}
			}			
			_chartAreaDefault.AxisY.LabelStyle.Format=_yAxisFormat;			
			bool doRecalc=false;
			if(_yAxisMaxVal!=_chartAreaDefault.AxisY.Maximum) { //Only set when this has changed to prevent extra rendering.
				_chartAreaDefault.AxisY.Maximum=_yAxisMaxVal;
				doRecalc=true;
			}
			if(_yAxisMinVal!=_chartAreaDefault.AxisY.Minimum) { //Only set when this has changed to prevent extra rendering.
				_chartAreaDefault.AxisY.Minimum=_yAxisMinVal;
				doRecalc=true;				
			}
			if(doRecalc) { //Only recalc when necessary to prevent extra rendering.
				_chartAreaDefault.RecalculateAxesScale();
			}			
			if(_yAxisMinVal!=0||_yAxisMaxVal!=0) {
				_chartAreaDefault.AxisX.ScaleView.ZoomReset();
			}
			SetXAxisLabelDensity();
			chart1.ApplyPaletteColors();
			if(FilterDataComplete!=null) {
				FilterDataComplete(this,new EventArgs());
			}
			chartLegend1.SetLegendItems(chart1.Series.ToDictionary(x => x.Name,x => x.Color));
		}

		private void SetXAxisLabelDensity() {
			if(_intervals.Count<=0||this.Width<=0) {
				return;
			}
			double xInterval=_intervals.Count/Math.Min(_intervals.Count,_intervals.Count/(int)Math.Ceiling(_intervals.Count/(this.Width/30f)));
			_chartAreaDefault.AxisX.Interval=Math.Round(xInterval);
			_chartAreaDefault.AxisX.IntervalOffset=0;
		}

		private void SetGraphTitles() {
			try {
				chart1.Titles["ChartTitle"].Text=GraphTitle;
				textChartTitle.Text=GraphTitle;
				comboBreakdownBy.GetItem<BreakdownType>(BreakdownType.items).Display=LegendTitle.ToString()+"s";
			}
			catch(Exception e) { }
		}

		public const int WM_WINDOWPOSCHANGED=0x47;
		protected override void WndProc(ref Message m) {
			base.WndProc(ref m);
			if(m.Msg==WM_WINDOWPOSCHANGED && LegendDock==LegendDockType.Bottom) {
				//This control has been repositioned so update the splitter position if necessary.
				int newSplitterDistance=Math.Max(splitContainerChart.Height-30,1);
				if(splitContainerChart.SplitterDistance!=newSplitterDistance) {
					splitContainerChart.SplitterDistance=newSplitterDistance;
				}
			}
		}

		private void LayoutChartContainer() {
			switch(LegendDock) {
				case LegendDockType.Bottom:
					splitContainerChart.Panel1Collapsed=false;
					splitContainerChart.Orientation=Orientation.Horizontal;
					splitContainerChart.FixedPanel=FixedPanel.Panel2;
					splitContainerChart.SplitterDistance=Math.Max(splitContainerChart.Height-30,1);
					splitContainerChart.Panel1.Controls.Clear();
					splitContainerChart.Panel2.Controls.Clear();
					splitContainerChart.Panel1.Controls.Add(panelChart);
					splitContainerChart.Panel2.Controls.Add(chartLegend1);
					break;
				case LegendDockType.Left:
					splitContainerChart.Panel1Collapsed=false;
					splitContainerChart.Orientation=Orientation.Vertical;
					splitContainerChart.FixedPanel=FixedPanel.Panel1;
					splitContainerChart.SplitterDistance=120;
					splitContainerChart.Panel1.Controls.Clear();
					splitContainerChart.Panel2.Controls.Clear();
					splitContainerChart.Panel1.Controls.Add(chartLegend1);
					splitContainerChart.Panel2.Controls.Add(panelChart);
					break;
				case LegendDockType.None:
					splitContainerChart.Panel1Collapsed=true;
					splitContainerChart.Panel1.Controls.Clear();
					splitContainerChart.Panel2.Controls.Clear();
					splitContainerChart.Panel2.Controls.Add(panelChart);
					break;
				default:
					break;
			}
		}

		#endregion

		#region Printing
		public void SaveImage() {
			SaveFileDialog sd=new SaveFileDialog() {
				Filter="Bitmap (*.bmp)|*.bmp|JPEG (*.jpg)|*.jpg|EMF (*.emf)|*.emf|PNG (*.png)|*.png|GIF (*.gif)|*.gif|TIFF (*.tif)|*.tif",
				FilterIndex=2,
				RestoreDirectory=true,
			};			
			if(sd.ShowDialog()!=DialogResult.OK) {
				return;
			}
			ChartImageFormat format = ChartImageFormat.Bmp;
			if(sd.FileName.EndsWith("bmp")) {
				format=ChartImageFormat.Bmp;
			}
			else if(sd.FileName.EndsWith("jpg")) {
				format=ChartImageFormat.Jpeg;
			}
			else if(sd.FileName.EndsWith("emf")) {
				format=ChartImageFormat.Emf;
			}
			else if(sd.FileName.EndsWith("gif")) {
				format=ChartImageFormat.Gif;
			}
			else if(sd.FileName.EndsWith("png")) {
				format=ChartImageFormat.Png;
			}
			else if(sd.FileName.EndsWith("tif")) {
				format=ChartImageFormat.Tiff;
			}
			chart1.SaveImage(sd.FileName,format);
		}

		public bool CanPrint() {
			return true;
		}

		public void PrintPageSetup() {
			chart1.Printing.PageSetup();
		}

		public void Print() {
			chart1.Printing.Print(true);
		}

		public void PrintPreview() {
			chart1.Printing.PrintPreview();
			//new FormPrintSettings(chart1).ShowDialog(); //currently working on. -andrew 2/16
		}
		#endregion

		#region I/O
		public void SetRawData(List<GraphPointBase> rawData) {
			_rawData=rawData;
			_rawData.RemoveAll(x => x.DateStamp.Year<1880);
			if(_rawData.Count>0) {
				_allowFilter=true;
			}
			FilterData(this,new EventArgs());
		}
		
		public QuantityOverTimeGraphSettings GetGraphSettings() {
			return new QuantityOverTimeGraphSettings() {
				QtyType=this.QtyType,
				SeriesType=this.SeriesType,
				GroupByType=this.GroupByType,
				LegendDock=this.LegendDock,
				QuickRangePref=this.QuickRangePref,
				BreakdownPref=this.BreakdownPref,
				BreakdownVal=this.BreakdownVal,
				DateFrom=this.DateFrom,
				DateTo=this.DateTo,
				Title=this.GraphTitle,
			};
		}

		public string SerializeToJson() {
			return ODGraphSettingsAbs.Serialize(GetGraphSettings());
		}

		public void DeserializeFromJson(string json) {
			try {
				QuantityOverTimeGraphSettings settings=ODGraphSettingsAbs.Deserialize<QuantityOverTimeGraphSettings>(json);
				this.QtyType=settings.QtyType;
				this.SeriesType=settings.SeriesType;
				this.GroupByType=settings.GroupByType;
				this.LegendDock=settings.LegendDock;
				this.BreakdownPref=settings.BreakdownPref;
				this.BreakdownVal=settings.BreakdownVal;
				this.DateFrom=settings.DateFrom;
				this.DateTo=settings.DateTo;
				this.GraphTitle=settings.Title;
				//Important that quick range is set last to prevent FilterData from firing multiple times.
				this.QuickRangePref=settings.QuickRangePref;
			}
			catch(Exception e) {
				MessageBox.Show(e.Message);
			}			
		}
		
		public class QuantityOverTimeGraphSettings:ODGraphBaseSettingsAbs {
			public QuantityType QtyType { get; set; }
			///<summary>Supported types: StackedArea,StackedColumn,Column,Line.</summary>
			public SeriesChartType SeriesType { get; set; }
			public IntervalType GroupByType { get; set; }
			public LegendDockType LegendDock { get; set; }
			public BreakdownType BreakdownPref { get; set; }
			public int BreakdownVal { get; set; }
		}
		#endregion
	}
}
