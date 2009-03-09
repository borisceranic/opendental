using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace OpenDentBusiness{

	///<summary></summary>
	public class Programs{

		///<summary></summary>
		public static DataTable RefreshCache() {
			string command="SELECT * FROM program ORDER BY ProgDesc";
			DataTable table=General.GetTable(command);
			table.TableName="Program";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table) {
			ProgramC.HList=new Hashtable();
			Program prog = new Program();
			ProgramC.Listt=new List<Program>();
			for (int i=0;i<table.Rows.Count;i++){
				prog=new Program();
				prog.ProgramNum =PIn.PInt(table.Rows[i][0].ToString());
				prog.ProgName   =PIn.PString(table.Rows[i][1].ToString());
				prog.ProgDesc   =PIn.PString(table.Rows[i][2].ToString());
				prog.Enabled    =PIn.PBool(table.Rows[i][3].ToString());
				prog.Path       =PIn.PString(table.Rows[i][4].ToString());
				prog.CommandLine=PIn.PString(table.Rows[i][5].ToString());
				prog.Note       =PIn.PString(table.Rows[i][6].ToString());
				ProgramC.Listt.Add(prog);
				if(!ProgramC.HList.ContainsKey(prog.ProgName)) {
					ProgramC.HList.Add(prog.ProgName,prog);
				}
			}
		}

		///<summary></summary>
		public static void Update(Program Cur){
			string command= "UPDATE program SET "
				+"ProgName = '"     +POut.PString(Cur.ProgName)+"'"
				+",ProgDesc  = '"   +POut.PString(Cur.ProgDesc)+"'"
				+",Enabled  = '"    +POut.PBool  (Cur.Enabled)+"'"
				+",Path = '"        +POut.PString(Cur.Path)+"'"
				+",CommandLine  = '"+POut.PString(Cur.CommandLine)+"'"
				+",Note  = '"       +POut.PString(Cur.Note)+"'"
				+" WHERE programnum = '"+POut.PInt(Cur.ProgramNum)+"'";
			General.NonQ(command);
		}

		///<summary></summary>
		public static void Insert(Program Cur){
			string command= "INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
				+") VALUES("
				+"'"+POut.PString(Cur.ProgName)+"', "
				+"'"+POut.PString(Cur.ProgDesc)+"', "
				+"'"+POut.PBool  (Cur.Enabled)+"', "
				+"'"+POut.PString(Cur.Path)+"', "
				+"'"+POut.PString(Cur.CommandLine)+"', "
				+"'"+POut.PString(Cur.Note)+"')";
			//MessageBox.Show(cmd.CommandText);
			Cur.ProgramNum=General.NonQ(command, true);
		}

		///<summary>This can only be called by the user if it is a program link that they created. Included program links cannot be deleted.  If calling this from ClassConversion, must delete any dependent ProgramProperties first.  It will delete ToolButItems for you.</summary>
		public static void Delete(Program prog){
			string command = "DELETE from toolbutitem WHERE ProgramNum = "+POut.PInt(prog.ProgramNum);
			General.NonQ(command);
			command = "DELETE from program WHERE ProgramNum = '"+prog.ProgramNum.ToString()+"'";
			General.NonQ(command);
		}

		///<summary>Returns true if a Program link with the given name or number exists and is enabled.</summary>
		public static bool IsEnabled(string progName){
			if(ProgramC.HList.ContainsKey(progName) && ((Program)ProgramC.HList[progName]).Enabled) {
				return true;
			}
			return false;
		}

		///<summary></summary>
		public static bool IsEnabled(int programNum){
			for(int i=0;i<ProgramC.Listt.Count;i++) {
				if(ProgramC.Listt[i].ProgramNum==programNum && ProgramC.Listt[i].Enabled) {
					return true;
				}
			}
			return false;
		}

		///<summary>Supply a valid program Name, and this will set Cur to be the corresponding Program object.</summary>
		public static Program GetCur(string progName){
			for(int i=0;i<ProgramC.Listt.Count;i++) {
				if(ProgramC.Listt[i].ProgName==progName) {
					return ProgramC.Listt[i];
				}
			}
			return null;//to signify that the program could not be located. (user deleted it in an older version)
		}

		///<summary>Supply a valid program Name.  Will return 0 if not found.</summary>
		public static int GetProgramNum(string progName) {
			for(int i=0;i<ProgramC.Listt.Count;i++) {
				if(ProgramC.Listt[i].ProgName==progName) {
					return ProgramC.Listt[i].ProgramNum;
				}
			}
			return 0;
		}

		



	}

	

	


}










