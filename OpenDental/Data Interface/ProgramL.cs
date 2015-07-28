using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using OpenDental.Bridges;
using OpenDentBusiness;
using OpenDental.UI;
using System.Drawing;

namespace OpenDental{
	

	///<summary></summary>
	public class ProgramL{

		///<summary>Typically used when user clicks a button to a Program link.  This method attempts to identify and execute the program based on the given programNum.</summary>
		public static void Execute(long programNum,Patient pat) {
			Program prog=null;
			List<Program> listPrograms=ProgramC.GetListt();
			for(int i=0;i<listPrograms.Count;i++) {
				if(listPrograms[i].ProgramNum==programNum) {
					prog=listPrograms[i];
				}
			}
			if(prog==null) {//no match was found
				MessageBox.Show("Error, program entry not found in database.");
				return;
			}
			if(PrefC.GetBool(PrefName.ShowFeaturePatientClone)) {
				Patient patClone;
				Patient patNonClone;
				List<Patient> listAmbiguousMatches;
				Patients.GetCloneAndNonClone(pat,out patClone,out patNonClone,out listAmbiguousMatches);
				if(patNonClone!=null) {
					pat=patNonClone;
				}
			}
			if(prog.PluginDllName!="") {
				if(pat==null) {
					Plugins.LaunchToolbarButton(programNum,0);
				}
				else{
					Plugins.LaunchToolbarButton(programNum,pat.PatNum);
				}
				return;
			}
			if(prog.ProgName==ProgramName.Apixia.ToString()) {
				Apixia.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Apteryx.ToString()) {
				Apteryx.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.AudaxCeph.ToString()) {
				AudaxCeph.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.BioPAK.ToString()) {
				BioPAK.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Camsight.ToString()) {
				Camsight.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.CaptureLink.ToString()) {
				CaptureLink.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Cerec.ToString()) {
				Cerec.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.CliniView.ToString()) {
				Cliniview.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.ClioSoft.ToString()) {
				ClioSoft.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.DBSWin.ToString()) {
				DBSWin.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.DemandForce.ToString()) {
				DemandForce.SendData(prog,pat);
				return;
			}
#if !DISABLE_WINDOWS_BRIDGES
			else if(prog.ProgName==ProgramName.DentalEye.ToString()) {
				DentalEye.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.DentalStudio.ToString()) {
				DentalStudio.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.DentX.ToString()) {
				DentX.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.DrCeph.ToString()) {
				DrCeph.SendData(prog,pat);
				return;
			}
#endif
			else if(prog.ProgName==ProgramName.DentalTekSmartOfficePhone.ToString()) {
				DentalTek.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.DentForms.ToString()) {
				DentForms.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Dexis.ToString()) {
				Dexis.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Digora.ToString()) {
				Digora.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Office.ToString()) {
				Office.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Dolphin.ToString()) {
				Dolphin.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Dxis.ToString()) {
				Dxis.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.EvaSoft.ToString()) {
				EvaSoft.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.EwooEZDent.ToString()) {
				Ewoo.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.FloridaProbe.ToString()) {
				FloridaProbe.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Guru.ToString()) {
				Guru.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.HandyDentist.ToString()) {
				HandyDentist.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.HouseCalls.ToString()) {
				FormHouseCalls FormHC=new FormHouseCalls();
				FormHC.ProgramCur=prog;
				FormHC.ShowDialog();
				return;
			}
			else if(prog.ProgName==ProgramName.iCat.ToString()) {
				ICat.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.ImageFX.ToString()) {
				ImageFX.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.iRYS.ToString()) {
				Irys.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Lightyear.ToString()) {
				Lightyear.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.MediaDent.ToString()) {
				MediaDent.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.MiPACS.ToString()) {
				MiPACS.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Owandy.ToString()) {
				Owandy.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.PandaPerio.ToString()) {
				PandaPerio.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Patterson.ToString()) {
				Patterson.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.PerioPal.ToString()) {
				PerioPal.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Planmeca.ToString()) {
				Planmeca.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Progeny.ToString()) {
				Progeny.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.PT.ToString()) {
				PaperlessTechnology.SendData(prog,pat,false);
				return;
			}
			else if(prog.ProgName==ProgramName.PTupdate.ToString()) {
				PaperlessTechnology.SendData(prog,pat,true);
				return;
			}
			else if(prog.ProgName==ProgramName.RayMage.ToString()) {
				RayMage.SendData(prog,pat);
				return;
			}
#if !DISABLE_WINDOWS_BRIDGES
			else if(prog.ProgName==ProgramName.Schick.ToString()) {
				Schick.SendData(prog,pat);
				return;
			}
#endif
			else if(prog.ProgName==ProgramName.Sirona.ToString()) {
				Sirona.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Sopro.ToString()) {
				Sopro.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.TigerView.ToString()) {
				TigerView.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Triana.ToString()) {
				Triana.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Trophy.ToString()) {
				Trophy.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.TrophyEnhanced.ToString()) {
				TrophyEnhanced.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Tscan.ToString()) {
				Tscan.SendData(prog,pat);
				return;
			}
#if !DISABLE_WINDOWS_BRIDGES
			else if(prog.ProgName==ProgramName.Vipersoft.ToString()) {
				Vipersoft.SendData(prog,pat);
				return;
			}
#endif
			else if(prog.ProgName==ProgramName.visOra.ToString()) {
				Visora.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.VistaDent.ToString()) {
				VistaDent.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.VixWin.ToString()) {
				VixWin.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.VixWinBase36.ToString()) {
				VixWinBase36.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.VixWinBase41.ToString()) {
				VixWinBase41.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.VixWinNumbered.ToString()) {
				VixWinNumbered.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.VixWinOld.ToString()) {
				VixWinOld.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.XDR.ToString()) {
				Dexis.SendData(prog,pat);//XDR uses the Dexis protocol
				return;
			}
			else if(prog.ProgName==ProgramName.XVWeb.ToString()) {
				XVWeb.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.ZImage.ToString()) {
				ZImage.SendData(prog,pat);
				return;
			}
			//all remaining programs:
			try{
				string cmdline=prog.CommandLine;
				string path=Programs.GetProgramPath(prog);
				if(pat!=null) {
					cmdline=cmdline.Replace("[LName]",pat.LName);
					cmdline=cmdline.Replace("[FName]",pat.FName);
					cmdline=cmdline.Replace("[PatNum]",pat.PatNum.ToString());
					cmdline=cmdline.Replace("[ChartNumber]",pat.ChartNumber);
					cmdline=cmdline.Replace("[WirelessPhone]",pat.WirelessPhone);
					cmdline=cmdline.Replace("[HmPhone]",pat.HmPhone);
					cmdline=cmdline.Replace("[WkPhone]",pat.WkPhone);
					cmdline=cmdline.Replace("[LNameLetter]",pat.LName.Substring(0,1).ToUpper());
					path=path.Replace("[LName]",pat.LName);
					path=path.Replace("[FName]",pat.FName);
					path=path.Replace("[PatNum]",pat.PatNum.ToString());
					path=path.Replace("[ChartNumber]",pat.ChartNumber);
					path=path.Replace("[WirelessPhone]",pat.WirelessPhone);
					path=path.Replace("[HmPhone]",pat.HmPhone);
					path=path.Replace("[WkPhone]",pat.WkPhone);
					path=path.Replace("[LNameLetter]",pat.LName.Substring(0,1).ToUpper());
				}
				Process.Start(path,cmdline);
			}
			catch{
				MessageBox.Show(prog.ProgDesc+" is not available.");
				return;
			}
		}

		public static void LoadToolbar(ODToolBar ToolBarMain,ToolBarsAvail toolBarsAvail) {
			ArrayList toolButItems=ToolButItems.GetForToolBar(toolBarsAvail);
			for(int i=0;i<toolButItems.Count;i++) {
				ToolButItem toolButItemCur=((ToolButItem)toolButItems[i]);
				Program programCur=Programs.GetProgram(toolButItemCur.ProgramNum);
				string key=programCur.ProgramNum.ToString()+programCur.ProgName.ToString();
				if(ToolBarMain.ImageList.Images.ContainsKey(key)) {
					//Dispose the existing image only if it already exists, because the image might have changed.
					ToolBarMain.ImageList.Images[ToolBarMain.ImageList.Images.IndexOfKey(key)].Dispose();
					ToolBarMain.ImageList.Images.RemoveByKey(key);
				}
				if(programCur.ButtonImage!="") {
					Image image=PIn.Bitmap(programCur.ButtonImage);
					ToolBarMain.ImageList.Images.Add(key,image);
				}
				if(toolBarsAvail!=ToolBarsAvail.AllModules) {
					ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
				}
				ToolBarMain.Buttons.Add(new ODToolBarButton(toolButItemCur.ButtonText,-1,"",key));
			}
			for(int i=0;i<ToolBarMain.Buttons.Count;i++) {//Reset the new index, because it might have changed due to removing/adding to the Images list.
				string key=ToolBarMain.Buttons[i].Tag.ToString();
				if(ToolBarMain.ImageList.Images.ContainsKey(key)) {
					ToolBarMain.Buttons[i].ImageIndex=ToolBarMain.ImageList.Images.IndexOfKey(key);
				}
			}
		}

	}
}