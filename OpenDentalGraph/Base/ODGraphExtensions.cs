using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenDentalGraph.Extensions {

	public static class GraphExtensions {
		public static double RoundSignificant(this double val) {
			int asInt=(int)val;
			double ret=(Math.Truncate(asInt/Math.Pow(10,asInt.ToString().Length-1))+1)*Math.Pow(10,asInt.ToString().Length-1);
			if(val<0) {
				ret*=-1;
			}
			return ret;
		}
	}

	public class ComboItemIntValue:ComboItem<int> {
		public enum AllTrueFalseFlag { All, False, True }

		public static AllTrueFalseFlag GetAllTrueFalseFilter(ComboBox combo) {
			if(combo.SelectedItem==null || (!(combo.SelectedItem is ComboItemIntValue))) {
				return AllTrueFalseFlag.All;
			}
			return (AllTrueFalseFlag)((ComboItemIntValue)combo.SelectedItem).Value;
		}

		public static bool IncludeInFilter(AllTrueFalseFlag filterFlag,bool value) {
			if(filterFlag==AllTrueFalseFlag.All) {
				return true;
			}
			return value?filterFlag==AllTrueFalseFlag.True:filterFlag==AllTrueFalseFlag.False;
		}
	}

	public class ComboItem<T> {
		public T Value { get; set; }
		public string Display { get; set; }

		public static string GetDisplay(ComboBox combo) {
			ComboItem<string> item=(ComboItem<string>)combo.SelectedItem;
			if(item.Value==null) {
				return item.Display;
			}
			return item.Value;
		}

		public static long GetValueLong(ComboBox combo) {
			ComboItem<long> item=(ComboItem<long>)combo.SelectedItem;
			return item.Value;
		}
	}


	public static class ComboBoxEx {
		public static void SetDataToAllTrueFalse(this ComboBox combo) {
			SetDataToEnums<ComboItemIntValue.AllTrueFalseFlag>(combo,false,false);
		}

		public delegate string StringFromEnumArgs<T>(T item);

		public static void SetDataToEnums<T>(this ComboBox combo,bool includeAllAtTop,bool showValueIndDisplay=true,int min=-1,int max=-1,StringFromEnumArgs<T> getStringFromEnum=null) where T:struct, IConvertible {
			//Make sure the IConvertibleType is actually an enum.
			if(!typeof(T).IsEnum) {
				throw new Exception("T must be an Enum type");
			}
			SetDataToEnums<T>(combo,Enum.GetValues(typeof(T)).Cast<T>().ToList(),includeAllAtTop,showValueIndDisplay,min,max,getStringFromEnum);
		}

		public static void SetDataToEnumsPrimitive<T>(this ComboBox combo,StringFromEnumArgs<T> getStringFromEnum) where T : struct, IConvertible {
			//Make sure the IConvertibleType is actually an enum.
			if(!typeof(T).IsEnum) {
				throw new Exception("T must be an Enum type");
			}
			List<T> list=Enum.GetValues(typeof(T)).Cast<T>().ToList();
			SetDataToEnumsPrimitive<T>(combo,list,0,list.Count-1,getStringFromEnum);
		}

		public static void SetDataToEnumsPrimitive<T>(this ComboBox combo,int min=-1,int max=-1,StringFromEnumArgs<T> getStringFromEnum=null) where T:struct, IConvertible {
			//Make sure the IConvertibleType is actually an enum.
			if(!typeof(T).IsEnum) {
				throw new Exception("T must be an Enum type");
			}
			SetDataToEnumsPrimitive<T>(combo,Enum.GetValues(typeof(T)).Cast<T>().ToList(),min,max,getStringFromEnum);
		}

		public static T GetValue<T>(this ComboBox combo) {
			ComboItem<T> item=(ComboItem<T>)combo.SelectedItem;
			return item.Value;
		}

		public static string GetDisplay<T>(this ComboBox combo) {
			ComboItem<T> item=(ComboItem<T>)combo.SelectedItem;
			return item.Display;
		}

		public static ComboItem<T> GetItem<T>(this ComboBox combo,T item) where T:struct, IConvertible {
			for(int i=0; i<combo.Items.Count; i++){
				ComboItem<T> comboItem=(ComboItem<T>)combo.Items[i];
				if(comboItem.Value.ToString()==item.ToString()) {
					return comboItem;
				}				
			}
			return null;
		}

		public static void SetItem<T>(this ComboBox combo,T item) where T:struct, IConvertible {
			for(int i=0;i<combo.Items.Count;i++) {
				ComboItem<T> comboItem=(ComboItem<T>)combo.Items[i];
				if(comboItem.Value.ToString()==item.ToString()) {
					combo.SelectedItem=comboItem;
					return;
				}
			}
		}

		public static void SetDataToEnumsPrimitive<T>(this ComboBox combo,List<T> enumValues,int min=-1,int max=-1,StringFromEnumArgs<T> getStringFromEnum=null) where T:struct, IConvertible {
			List<ComboItem<T>> listItems=new List<ComboItem<T>>();			
			enumValues.ForEach(x => {
				int val=Convert.ToInt32(x);
				if(min>=0 && val < min) {
					return;
				}
				if(max>=0 && val > max) {
					return;
				}
				string display=x.ToString();
				if(getStringFromEnum!=null) {
					display=getStringFromEnum(x);
				}
				listItems.Add(new ComboItem<T>() { Value=x,Display=display });
			});
			//Try to retrain previous selection.
			int selIdx=-1;
			//if(combo.SelectedItem!=null && (combo.SelectedItem is ComboItemIntValue)) {
			//	selIdx=listItems.FindIndex(x => x.Value==((ComboItemIntValue)combo.SelectedItem).Value);
			//}
			combo.ValueMember="Value";
			combo.DisplayMember="Display";
			combo.DataSource=listItems;
			if(selIdx>=0) {
				combo.SelectedIndex=selIdx;
			}
		}

		public static void SetDataToEnums<T>(this ComboBox combo,List<T> enumValues,bool includeAllAtTop,bool showValueIndDisplay=true,int min=-1,int max=-1,StringFromEnumArgs<T> getStringFromEnum=null) where T:struct, IConvertible {			
			List<ComboItemIntValue> listItems=new List<ComboItemIntValue>();
			if(includeAllAtTop) {
				listItems.Add(new ComboItemIntValue() { Value=-1,Display="All" });
			}
			enumValues.ForEach(x => {
				int val=Convert.ToInt32(x);
				if(min>=0 && val < min) {
					return;
				}
				if(max>=0 && val > max) {
					return;
				}
				string display=(showValueIndDisplay?(Convert.ToInt32(x).ToString()+" - "):"")+x.ToString();
				if(getStringFromEnum!=null) {
					display=getStringFromEnum(x);
				}
				listItems.Add(new ComboItemIntValue() { Value=Convert.ToInt32(x),Display=display });
			});
			//Try to retrain previous selection.
			int selIdx=-1;
			if(combo.SelectedItem!=null && (combo.SelectedItem is ComboItemIntValue)) {
				selIdx=listItems.FindIndex(x => x.Value==((ComboItemIntValue)combo.SelectedItem).Value);
			}
			combo.ValueMember="Value";
			combo.DisplayMember="Display";
			combo.DataSource=listItems;
			if(selIdx>=0) {
				combo.SelectedIndex=selIdx;
			}
		}

		public static void SetDataToComboItems<T>(this ComboBox combo,bool includeAllAtTop,bool includeBlank,List<ComboItem<T>> items) where T:IEquatable<T> {
			if(includeBlank) {
				items.Insert(0,new ComboItem<T>() { Display="Invalid" });
				items.Insert(0,new ComboItem<T>() { Display="N/A" });
				items.Insert(0,new ComboItem<T>() { Display="" });
			}
			if(includeAllAtTop) {
				items.Insert(0,new ComboItem<T>() { Display="All" });
			}
			//Try to retrain previous selection.
			int selIdx=-1;
			if(combo.SelectedItem!=null && (combo.SelectedItem is ComboItem<T>)) {
				T sel=((ComboItem<T>)combo.SelectedItem).Value;
				if(sel!=null) {
					selIdx=items.FindIndex(x => x.Value!=null && x.Value.Equals(sel));
				}
			}
			combo.ValueMember="Value";
			combo.DisplayMember="Display";
			combo.DataSource=items;
			if(selIdx>=0) {
				combo.SelectedIndex=selIdx;
			}
		}
	}
}
