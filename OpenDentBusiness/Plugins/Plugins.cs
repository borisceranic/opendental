using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using CodeBase;

namespace OpenDentBusiness {
	public class Plugins {
		private static List<PluginContainer> PluginList;
		//public static bool Active=false;

		public static bool PluginsAreLoaded {
			get {
				if(PluginList==null) {
					return false;
				}
				else {
					return true;
				}
			}
		}

		///<summary>If this is middle tier, pass in null.</summary>
		public static void LoadAllPlugins(Form host) {
			//No need to check RemotingRole; no call to db.
			PluginList=new List<PluginContainer>();
			//if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
			//  js 6/20/14 Don't do this.  We will now support plugins for middle tier.
			//	return;//no plugins will load.  So from now on, we can assume a direct connection.
			//}
			for(int i=0;i<ProgramC.Listt.Count;i++) {
				if(!ProgramC.Listt[i].Enabled) {
					continue;
				}
				if(ProgramC.Listt[i].PluginDllName=="") {
					continue;
				}
				string dllPath=ODFileUtils.CombinePaths(Application.StartupPath,ProgramC.Listt[i].PluginDllName);
				if(RemotingClient.RemotingRole==RemotingRole.ServerWeb) {
					dllPath=ODFileUtils.CombinePaths(System.Web.HttpContext.Current.Server.MapPath(null),ProgramC.Listt[i].PluginDllName);
				}
				if(dllPath.Contains("[VersionMajMin]")) {
					Version vers=new Version(Application.ProductVersion);
					string dllPathWithVersion=dllPath.Replace("[VersionMajMin]",vers.Major.ToString()+"."+vers.Minor.ToString());
					dllPath=dllPath.Replace("[VersionMajMin]","");//now stripped clean
					if(File.Exists(dllPathWithVersion)) {
						File.Copy(dllPathWithVersion,dllPath,true);
					}
					else{
						//try the Plugins folder
						#if !DEBUG
						if(PrefC.AtoZfolderUsed) {//must be using AtoZ folder
							string dllPathVersionCentral=ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(),"Plugins",
								ProgramC.Listt[i].PluginDllName.Replace("[VersionMajMin]",vers.Major.ToString()+"."+vers.Minor.ToString()));
							if(File.Exists(dllPathVersionCentral)) {
								File.Copy(dllPathVersionCentral,dllPath,true);
							}
						}
						#endif
					}
				}
				if(!File.Exists(dllPath)) {
					continue;
				}
				PluginBase plugin=null;
				Assembly ass=null;
				string assName="";
				try {
					ass=Assembly.LoadFile(dllPath);
					assName=Path.GetFileNameWithoutExtension(dllPath);
					string typeName=assName+".Plugin";
					Type type=ass.GetType(typeName);
					plugin=(PluginBase)Activator.CreateInstance(type);
					plugin.Host=host;
				}
				catch(Exception ex) {
					//how to handle this for RemotingRole.ServerWeb?:
					MessageBox.Show("Error loading Plugin:"+ProgramC.Listt[i].PluginDllName+"\r\n"
						+ex.Message);
					continue;//don't add it to plugin list.
				}
				PluginContainer container=new PluginContainer();
				container.Plugin=plugin;
				container.ProgramNum=ProgramC.Listt[i].ProgramNum;
				container.Assemb=ass;
				container.Name=assName;
				PluginList.Add(container);
			}
		}

		///<summary>Returns null if no plugin assembly loaded with the given name.  So OpenDentBusiness can be passed through here quickly to return null.</summary>
		public static Assembly GetAssembly(string name) {
			if(PluginList==null){
				if(RemotingClient.RemotingRole==RemotingRole.ServerWeb) {//on middle tier server.
					LoadAllPlugins(null);
				}
				else {
					//this is going to be rare.  Only during UnitTest and in the first Security.LogInWeb call.
					return null;
				}
			}
			for(int i=0;i<PluginList.Count;i++) {
				if(PluginList[i].Name==name) {
					return PluginList[i].Assemb;
				}
			}
			return null;
		}

		///<summary>Will return true if a plugin implements this method, replacing the default behavior.</summary>
		public static bool HookMethod(object sender,string hookName,params object[] parameters) {
			for(int i=0;i<PluginList.Count;i++) {
				//if there are multiple plugins, we use the first implementation that we come to.
				if(PluginList[i].Plugin.HookMethod(sender,hookName,parameters)) {
					return true;
				}
			}
			return false;//no implementation was found
		}

		///<summary>Adds code without disrupting existing code.</summary>
		public static void HookAddCode(object sender,string hookName,params object[] parameters) {
			if(PluginList==null && RemotingClient.RemotingRole==RemotingRole.ServerWeb) {//on middle tier server.
				LoadAllPlugins(null);
			}
			//Plugins are not currently supported with the GWT web service.
			if(PluginList==null) {
				return;//Fail silently if plugins could not be loaded.
			}
			for(int i=0;i<PluginList.Count;i++) {
				//if there are multiple plugins, we run them all
				PluginList[i].Plugin.HookAddCode(sender,hookName,parameters);
			}
		}

		public static void LaunchToolbarButton(long programNum,long patNum) {
			for(int i=0;i<PluginList.Count;i++) {
				if(PluginList[i].ProgramNum==programNum) {
					PluginList[i].Plugin.LaunchToolbarButton(patNum);
					return;
				}
			}
		}


	}
}