using CodeBase;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OpenDentalGraph {
	///<summary>Simple interface to mark an MSChart UserControl as an OD graph.</summary>
	/// <typeparam name="DATAPOINT">The type of ODGraphDataPoint required by the graph control.</typeparam>
	/// <typeparam name="GRAPHSETTINGS">The type of ODGraphSettingsAbs required by the graph.</typeparam>
	public interface IODGraph<DATAPOINT, GRAPHSETTINGS>: IODHasGraphSettings<GRAPHSETTINGS>, IODGraphPrinter 
		where DATAPOINT : ODGraphPointBase
		where GRAPHSETTINGS : ODGraphSettingsAbs {
		void SetRawData(List<DATAPOINT> RawData);	
		bool IsLoading { get; set; }	
	}

	///<summary>Must be implemented in order to facilitate printing from dashboard.</summary>
	public interface IODGraphPrinter {
		void PrintPreview();
	}

	///<summary>Must be implemented in order to facilitate printing from dashboard cell editor.</summary>
	public interface IHasODGraphPrinter {
		IODGraphPrinter GetPrinter();
	}

	///<summary>Base class for graph data points. This is the basic raw data input source for IODGraph.</summary>
	public class ODGraphPointBase {
		public string SeriesName { get; set; }
		public object Tag { get; set; }
	}

	///<summary>All graphs and filters must extend IODHasGraphSettings. Provides I/O for user provided settings.</summary>
	/// <typeparam name="GRAPHSETTINGS">The type of ODGraphSettingsBase required by the graph.</typeparam>
	public interface IODHasGraphSettings<GRAPHSETTINGS> where GRAPHSETTINGS : ODGraphSettingsAbs {
		GRAPHSETTINGS GetGraphSettings();
		void DeserializeFromJson(string json);
		string SerializeToJson();
	}

	///<summary>Extends ODGraphSettingsAbs by adding filter fields and title.</summary>
	public class ODGraphBaseSettingsAbs:ODGraphSettingsAbs {
		public string Title { get; set; }
		public Enumerations.QuickRange QuickRangePref { get; set; }
		public DateTime DateFrom { get; set; }
		public DateTime DateTo { get; set; }
		///<summary>Helper property to give easy access to the current DashboardFilter settings for this graph.</summary>
		public Cache.DashboardFilter Filter {
			get {
				return new Cache.DashboardFilter() { DateTo=this.DateTo,DateFrom=this.DateFrom,UseDateFilter=this.QuickRangePref!=Enumerations.QuickRange.allTime };
			}
		}

		///<summary>Helper method to convert from json string to ODGraphBaseSettingsAbs. Most importantly, provides filter settings for a given DashboardCell.</summary>
		public static ODGraphBaseSettingsAbs Deserialize(string json) {
			ODGraphBaseSettingsAbs ret=JsonConvert.DeserializeObject<ODGraphBaseSettingsAbs>(json);
			Cache.DashboardFilter filter=GetDatesFromQuickRange(ret.QuickRangePref,ret.DateFrom,ret.DateTo);
			ret.DateFrom=filter.DateFrom;
			ret.DateTo=filter.DateTo;
			return ret;
		}

		///<summary>If quickRange==QuickRange.custom then return a filter containing customDateFrom and customDateTo. 
		///In all other cases, the date range will be calculated given the quickRange.</summary>
		public static Cache.DashboardFilter GetDatesFromQuickRange(Enumerations.QuickRange quickRange,DateTime customDateFrom,DateTime customDateTo) {
			Cache.DashboardFilter filter=new Cache.DashboardFilter();
			switch(quickRange) {
				case Enumerations.QuickRange.custom:
					filter.DateTo=customDateTo;
					filter.DateFrom=customDateFrom;
					break;
				case Enumerations.QuickRange.last7Days:
					filter.DateTo=DateTime.Today;
					filter.DateFrom=filter.DateTo.AddDays(-7);
					break;
				case Enumerations.QuickRange.last30Days:
					filter.DateTo=DateTime.Today;
					filter.DateFrom=filter.DateTo.AddDays(-30);
					break;
				case Enumerations.QuickRange.last365Days:
					filter.DateTo=DateTime.Today;
					filter.DateFrom=filter.DateTo.AddDays(-365);
					break;
				case Enumerations.QuickRange.last12Months:
					filter.DateTo=new DateTime(DateTime.Today.Year,DateTime.Today.Month,1).AddDays(-1);
					filter.DateFrom=filter.DateTo.AddMonths(-12).AddDays(1);
					break;
				case Enumerations.QuickRange.previousWeek:
					filter.DateFrom=DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek).AddDays(-7);
					filter.DateTo=filter.DateFrom.AddDays(7);
					break;
				case Enumerations.QuickRange.previousMonth:
					filter.DateFrom=new DateTime(DateTime.Today.Year,DateTime.Today.Month,1).AddMonths(-1);
					filter.DateTo=filter.DateFrom.AddMonths(1);
					break;
				case Enumerations.QuickRange.previousYear:
					filter.DateFrom=new DateTime(DateTime.Today.Year-1,1,1);
					filter.DateTo=filter.DateFrom.AddYears(1);
					break;
				case Enumerations.QuickRange.thisWeek:
					filter.DateFrom=DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
					filter.DateTo=filter.DateFrom.AddDays(7);
					break;
				case Enumerations.QuickRange.thisMonth:
					filter.DateFrom=new DateTime(DateTime.Today.Year,DateTime.Today.Month,1);
					filter.DateTo=filter.DateFrom.AddMonths(1);
					break;
				case Enumerations.QuickRange.thisYear:
					filter.DateFrom=new DateTime(DateTime.Today.Year,1,1);
					filter.DateTo=filter.DateFrom.AddYears(1);
					break;
				case Enumerations.QuickRange.weekToDate:
					filter.DateFrom=DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
					filter.DateTo=DateTime.Today;
					break;
				case Enumerations.QuickRange.monthToDate:
					filter.DateFrom=new DateTime(DateTime.Today.Year,DateTime.Today.Month,1);
					filter.DateTo=DateTime.Today;
					break;
				case Enumerations.QuickRange.yearToDate:
					filter.DateFrom=new DateTime(DateTime.Today.Year,1,1);
					filter.DateTo=DateTime.Today;
					break;
				case Enumerations.QuickRange.allTime:
					filter.DateFrom=new DateTime(1880,1,1);
					filter.DateTo=filter.DateFrom.AddYears(300);
					filter.UseDateFilter=false;
					break;
				default:
					throw new Exception("Unsupported QuickRange: "+quickRange.ToString());
			}
			return filter;
		}

	}

	///<summary>Wraps JSON serialization. Any class that extends this class should NEVER change variable names after being released. 
	///The variable names are stored as plain-text json and neeed to remain back-compatible.
	///Adding new fields is OK.</summary>
	public class ODGraphSettingsAbs {
		public static string Serialize(ODGraphSettingsAbs obj) {
			return JsonConvert.SerializeObject(obj);
		}
		public static T Deserialize<T>(string json) {
			return JsonConvert.DeserializeObject<T>(json);
		}		
	}

	///<summary>Wrapper class which allows for 2 json fields. 1 for the graph and 1 for the filter.</summary>
	public class ODGraphJson {
		public string GraphJson;
		public string FilterJson;

		public static string Serialize(ODGraphJson obj) {
			return JsonConvert.SerializeObject(obj);
		}

		public static ODGraphJson Deserialize(string json) {
			return JsonConvert.DeserializeObject<ODGraphJson>(json);
		}
	}

	public delegate void ODGraphDataPointEventHandler(object sender,ODGraphPointBase dataPoint);
	public delegate Color OnGetColorArgs(string seriesName);

	///<summary>This is a generic base class. 
	///This means we need an intermediate class to explicitly implement the generic type before we create our final concrete. 
	///This intermediate class allows the final concrete class to have no generic type definitions, which will allow it to play nicely with the designer.
	///http://stackoverflow.com/questions/1700783/c-sharp-compiler-complains-that-abstract-class-does-not-implement-interface. 
	///http://stackoverflow.com/a/31968096. </summary>
	/// <typeparam name="DATAPOINT">The type of ODGraphDataPoint required by the graph control.</typeparam>
	/// <typeparam name="FILTERSETTINGS">The type of ODGraphSettingsAbs required by the filter control.</typeparam>
	/// <typeparam name="GRAPHSETTINGS">The type of ODGraphSettingsAbs required by the graph.</typeparam>
	public abstract class ODGraphFilterAbs<DATAPOINT,FILTERSETTINGS,GRAPHSETTINGS>:UserControl,IODHasGraphSettings<FILTERSETTINGS>, IDashboardDockContainer, IHasODGraphPrinter
		where DATAPOINT : ODGraphPointBase
		where FILTERSETTINGS : ODGraphSettingsAbs
		where GRAPHSETTINGS : ODGraphSettingsAbs {
		private ODThread _thread=null;
		private IODGraph<DATAPOINT,GRAPHSETTINGS> _graph;
		public IODGraph<DATAPOINT,GRAPHSETTINGS> Graph { get { return _graph; } set { _graph=value; } }
		protected virtual void OnInitDone(object sender,EventArgs e) { }
		protected virtual void OnInit(bool forceCachRefresh) { }
		protected virtual string ThreadName { get { return "ODGraphFilterAbs Thread"; } }
		
		protected virtual void CommitData(List<DATAPOINT> rawData) {
			if(_graph==null) {
				return;
			}
			if(rawData==null) {
				rawData=new List<DATAPOINT>();
			}
			_graph.SetRawData(rawData);
		}

		///<summary>Starts the thread which will populate the graph's data. DB context should be set before calling this method.</summary>
		/// <param name="forceCacheRefesh">Will pass this flag along to the thread which retrieves the cache. If true then cache will be invalidated and re-initialiazed from the db. Use sparingly.</param>
		/// <param name="onException">Exception handler if something fails. If not set then all exceptions will be swallowed.</param>
		/// <param name="onThreadDone">Done event handler. Will be invoked when init thread has completed.</param>
		public void Init(bool forceCacheRefesh=false,ODThread.ExceptionDelegate onException=null,EventHandler onThreadDone=null) {
			if(_thread!=null) { 
				return;
			}
			if(onThreadDone==null) {
				onThreadDone=new EventHandler(delegate(object o,EventArgs e) { });
			}
			if(onException==null) {
				onException=new ODThread.ExceptionDelegate((Exception e) => { });
			}
			_thread=new ODThread(new ODThread.WorkerDelegate((ODThread x) => {
				//The thread may have run and return before the window is even ready to display.
				if(this.IsHandleCreated) {
					OnThreadStartLocal(this,new EventArgs());
				}
				else {
					this.HandleCreated+=OnThreadStartLocal;
				}
				//Alert caller that it's time to start querying the db.
				OnInit(forceCacheRefesh);
			}));
			_thread.Name=ThreadName;
			_thread.AddExceptionHandler(onException);
			_thread.AddThreadExitHandler(new ODThread.WorkerDelegate((ODThread x) => {
				try {
					_thread=null;
					//The thread may have run and return before the window is even ready to display.
					if(this.IsHandleCreated) {
						OnThreadExitLocal(this,new EventArgs()); 
					}
					else {
						this.HandleCreated+=OnThreadExitLocal;
					}
					//Alert caller that db querying is done.
					onThreadDone(this,new EventArgs());
				}
				catch(Exception){ }				
			}));
			_thread.Start(true);
		}

		///<summary>Called when thread is started AND control handle has been created.</summary>
		private void OnThreadStartLocal(object sender,EventArgs e) {
			try {
				this.HandleCreated-=OnThreadStartLocal;
				this.Invoke((Action)delegate () {
					//Alert graph.
					_graph.IsLoading=true;
				});
			}
			catch(Exception) { }			
		}

		///<summary>Called after data has been initialized AND control handle has been created.</summary>
		protected void OnThreadExitLocal(object sender,EventArgs e) {
			try {
				this.Invoke((Action)delegate () {
					//Alert graph.
					_graph.IsLoading=false;
					//Alert inheriting class.
					OnInitDone(this,new EventArgs());					
				});
			}
			catch(Exception) { }
		}

		//IODHasGraphSettings must be implemented as abstract. http://stackoverflow.com/a/31968096.
		public abstract FILTERSETTINGS GetGraphSettings();
		public abstract void DeserializeFromJson(string json);
		public abstract DashboardDockContainer CreateDashboardDockContainer(OpenDentBusiness.TableBase dbItem);
		public abstract OpenDentBusiness.DashboardCellType GetCellType();

		///<summary>Serializes GetGraphSettings() abstract method returned value. Won't normally need to override this.</summary>
		public virtual string SerializeToJson() {
			return ODGraphSettingsAbs.Serialize(GetGraphSettings());
		}
		
		///<summary>Set json settings for both the filter and the graph it is linked to. Input json should be a serialized ODGraphJson object.</summary>
		public void SetFilterAndGraphSettings(string jsonSettings) {
			if(string.IsNullOrEmpty(jsonSettings)) {
				return;
			}
			ODGraphJson graphJson=ODGraphJson.Deserialize(jsonSettings);
			if(Graph!=null) {
				Graph.DeserializeFromJson(graphJson.GraphJson);
			}
			DeserializeFromJson(graphJson.FilterJson);
		}

		///<summary>Get json settings for both the filter and the graph it is linked to. Output will be json in the form of a serialized ODGraphJson object.</summary>
		public string GetFilterAndGraphSettings() {
			return ODGraphJson.Serialize(new ODGraphJson() {
				FilterJson=SerializeToJson(),
				GraphJson=Graph==null ? "" : Graph.SerializeToJson(),
			});
		}

		public string GetCellSettings() {
			return GetFilterAndGraphSettings();
		}

		public IODGraphPrinter GetPrinter() {
			return Graph;
		}		
	}	
}

namespace OpenDentalGraph.Enumerations {
	public enum QuickRange {
		///<summary>0</summary>
		allTime,
		///<summary>1</summary>
		custom,
		///<summary>2</summary>
		thisWeek,
		///<summary>3</summary>
		weekToDate,
		///<summary>4</summary>
		thisMonth,
		///<summary>5</summary>
		monthToDate,
		///<summary>6</summary>
		thisYear,
		///<summary>7</summary>
		yearToDate,
		///<summary>8</summary>
		previousWeek,
		///<summary>9</summary>
		previousMonth,
		///<summary>10</summary>
		previousYear,
		///<summary>11</summary>
		last7Days,
		///<summary>12</summary>
		last30Days,
		///<summary>13</summary>
		last365Days,
		///<summary>14</summary>
		last12Months,
	}

	public enum BreakdownType {
		///<summary>0 - Show every series as it's own series. Do not group.</summary>
		all,
		///<summary>1 - Show only 1 series as the group of all series combined.</summary>
		none,
		///<summary>2 - Show top x items each in their own series where x is defined by BreakdownVal. All remaining series will be grouped as one series.</summary>
		items,
		///<summary>3 - Show top x percentage of items each in their own series where x is defined by BreakdownVal. All remaining series will be grouped as one series.</summary>
		percent
	}

	public enum QuantityType {
		///<summary>0</summary>
		money,
		///<summary>1</summary>
		count
	}

	public enum LegendDockType {
		Bottom,
		Left,
		None
	}

}
