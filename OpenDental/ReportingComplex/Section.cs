using System;

namespace OpenDental.ReportingComplex {
	///<summary>Every ReportObject in an ODReport must be attached to a Section.</summary>
	public class Section{
		///<summary></summary>
		private string _name;
		///<summary></summary>
		private int _height;
		///<summary>Width is usually the entire page unless set differently here.</summary>
		private int _width;
		///<summary>Specifies which kind, like ReportHeader, or GroupFooter.</summary>
		private AreaSectionKind _kind;

		///<summary></summary>
		public Section(AreaSectionKind kind,int height){
			_kind=kind;
			//name is not user editable, so:
			switch(_kind){
				case AreaSectionKind.ReportHeader:
					_name="Report Header";
					break;
				case AreaSectionKind.PageHeader:
					_name="Page Header";
					break;
				case AreaSectionKind.GroupTitle:
					_name="Group Title";
					break;
				case AreaSectionKind.GroupHeader:
					_name="Group Header";
					break;
				case AreaSectionKind.Detail:
					_name="Detail";
					break;
				case AreaSectionKind.GroupFooter:
					_name="Group Footer";
					break;
				case AreaSectionKind.PageFooter:
					_name="Page Footer";
					break;
				case AreaSectionKind.ReportFooter:
					_name="Report Footer";
					break;
				case AreaSectionKind.Query:
					_name="Query";
					break;
			}
			_height=height;
		}

#region Properties
		///<summary>Not user editable.</summary>
		public string Name{
			get{
				return _name;
			}
		}
		///<summary></summary>
		public int Height{
			get{
				return _height;
			}
			set{
				_height=value;
			}
		}
		///<summary></summary>
		public int Width{
			get{
				return _width;
			}
			set{
				_width=value;
			}
		}
		///<summary></summary>
		public AreaSectionKind Kind{
			get{
				return _kind;
			}
			set{
				_kind=value;
			}
		}
#endregion


	}


	///<summary>The type of section is used in the Section class.  Only ONE of each type is allowed except for the GroupHeader and GroupFooter which are optional and can have one pair for each group.  The order of the sections is locked and user cannot change.</summary>
	public enum AreaSectionKind{
		///<summary>Printed at the top of the report.</summary>
		ReportHeader,
		///<summary>Printed at the top of each page.</summary>
		PageHeader,
		///<summary>Title of a specific group</summary>
		GroupTitle,
		///<summary>Will print at the top of a specific group.</summary>
		GroupHeader,
		///<summary>This is the data of the report and represents one row of data.  This section gets printed once for each record in the datatable.</summary>
		Detail,
		///<summary>Contains a buffer and/or a total of a column</summary>
		GroupFooter,
		///<summary>Prints at the bottom of each page, including after the reportFooter</summary>
		PageFooter,
		///<summary>Prints at the bottom of the report, but before the page footer for the last page.</summary>
		ReportFooter,
		///<summary>Query Section, contains groups of queries.</summary>
		Query
	}



}












