using System;
using System.Collections;
using OpenDentBusiness.DataAccess;


namespace OpenDentBusiness{

	/// <summary>THIS TABLE IS NOT BEING USED.  These are always attached to mountdefs.  Can be deleted without any problems.</summary>
	[DataObject("mountitemdef")]
	public class MountItemDef : DataObjectBase {

		[DataField("MountItemDefNum")]
		private long mountItemDefNum;
		/// <summary>Primary key.</summary>
		public long MountItemDefNum {
			get { return mountItemDefNum; }
			set { mountItemDefNum = value; MarkDirty(); mountItemDefNumChanged = true; }
		}
		bool mountItemDefNumChanged;
		public bool MountItemDefNumChanged {
			get { return mountItemDefNumChanged; }
		}

		[DataField("MountDefNum")]
		private long mountDefNum;
		/// <summary>FK to mountdef.MountDefNum.</summary>
		public long MountDefNum {
			get { return mountDefNum; }
			set { mountDefNum = value; MarkDirty(); mountDefNumChanged = true; }
		}
		bool mountDefNumChanged;
		public bool MountDefNumChanged {
			get { return mountDefNumChanged; }
		}

		[DataField("Xpos")]
		private int xpos;
		/// <summary>The x position, in pixels, of the item on the mount.</summary>
		public int Xpos {
			get { return xpos; }
			set { xpos = value; MarkDirty(); xposChanged = true; }
		}
		bool xposChanged;
		public bool XposChanged {
			get { return xposChanged; }
		}

		[DataField("Ypos")]
		private int ypos;
		/// <summary>The y position, in pixels, of the item on the mount.</summary>
		public int Ypos {
			get { return ypos; }
			set { ypos = value; MarkDirty(); yposChanged = true; }
		}
		bool yposChanged;
		public bool YposChanged {
			get { return yposChanged; }
		}

		[DataField("Width")]
		private int width;
		/// <summary>Ignored if mount IsRadiograph.  For other mounts, the image will be scaled to fit within this space.  Any cropping, rotating, etc, will all be defined in the original image itself.</summary>
		public int Width {
			get { return width; }
			set { width = value; MarkDirty(); widthChanged = true; }
		}
		bool widthChanged;
		public bool WidthChanged {
			get { return widthChanged; }
		}

		[DataField("Height")]
		private int height;
		/// <summary>Ignored if mount IsRadiograph.  For other mounts, the image will be scaled to fit within this space.  Any cropping, rotating, etc, will all be defined in the original image itself.</summary>
		public int Height {
			get { return height; }
			set { height = value; MarkDirty(); heightChanged = true; }
		}
		bool heightChanged;
		public bool HeightChanged {
			get { return heightChanged; }
		}

		///<summary></summary>
		public MountItemDef Copy() {
			MountItemDef m=new MountItemDef();
			m.MountItemDefNum=MountItemDefNum;
			m.MountDefNum=MountDefNum;
			m.Xpos=Xpos;
			m.Ypos=Ypos;
			m.Width=Width;
			m.Height=Height;
			return m;
		}

		
	}

		



		
	

	

	


}










