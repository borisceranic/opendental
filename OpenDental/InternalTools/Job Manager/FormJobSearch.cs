using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;
using System.Drawing.Text;
using System.Linq;
using System.Xml;

namespace OpenDental {
	public partial class FormJobSearch:Form {
		private List<Job> _listJobsAll;
		private List<Job> _listJobsFiltered;
		private List<Task> _listTasksAll;
		private List<Patient> _listPatientAll;
		private List<FeatureRequest> _listFeatureRequestsAll;
		private List<Bug> _listBugsAll;
		private List<Userod> _listUserExperts;
		private List<Userod> _listUserOwners;
		private List<JobStat> _listJobStatuses;
		private List<JobCategory> _listJobCategory;
		private Job _selectedJob;

		public FormJobSearch() {
			InitializeComponent();
		}

		public Job SelectedJob{
			get {return _selectedJob;}
		}

		public List<Job> GetSearchResults() {
			return _listJobsFiltered??new List<Job>();
		}

		private void FormJobNew_Load(object sender,EventArgs e) {
			//Experts
			_listUserExperts=Userods.GetUsersByJobRole(JobPerm.Engineer,true);
			_listUserExperts.Add(new Userod() {UserNum=0,UserName="Un-Assigned"});
			_listUserExperts.ForEach(x=>listBoxExpert.Items.Add(x.UserName));
			//Owners
			_listUserOwners=Userods.GetUsersByJobRole(JobPerm.Engineer,true);
			_listUserOwners.Add(new Userod() { UserNum=0,UserName="Un-Assigned" });
			_listUserOwners.ForEach(x => listBoxOwner.Items.Add(x.UserName));
			//Statuses
			_listJobStatuses=Enum.GetValues(typeof(JobStat)).Cast<JobStat>().ToList();
			_listJobStatuses.ForEach(x=>listBoxStatus.Items.Add(x.ToString()));
			//Categories
			_listJobCategory=Enum.GetValues(typeof(JobCategory)).Cast<JobCategory>().ToList();
			_listJobCategory.ForEach(x => listBoxCategory.Items.Add(x.ToString()));
			ODThread thread=new ODThread((o) => {
				//We can reduce these calls to DB by passing in more data from calling class. if available.
				_listJobsAll=Jobs.GetAll();
				Jobs.FillInMemoryLists(_listJobsAll);
				_listTasksAll=Tasks.GetMany(_listJobsAll.SelectMany(x=>x.ListJobLinks).Where(x=>x.LinkType==JobLinkType.Task).Select(x=>x.FKey).Distinct().ToList());
				_listPatientAll=Patients.GetMultPats(_listJobsAll.SelectMany(x => x.ListJobQuotes).Select(x => x.PatNum).Distinct().ToList()).ToList();
				_listPatientAll.AddRange(Patients.GetMultPats(_listTasksAll.FindAll(x=>x.ObjectType==TaskObjectType.Patient).Select(x=>x.KeyNum).ToList()));
				try {
					_listFeatureRequestsAll=FeatureRequest.GetAll();
				}
				catch (Exception ex){
					this.Invoke((Action)(()=>{textFeatReq.Enabled=false;MessageBox.Show("Unable to retreive feature requests.\r\n"+ex.Message);}));
				}
				_listBugsAll=Bugs.GetAll();
				this.Invoke((Action)ReadyForSearch);
				//this.Invoke((Action)FillTree);
				//this.Invoke((Action)FillWorkSummary);
			});
			thread.AddExceptionHandler((ex) => {/*todo*/});
			thread.Start();
		}

		private void ReadyForSearch() {
			butSearch.Enabled=true;
		}

		private void butSearch_Click(object sender,EventArgs e) {
			_selectedJob=null;
			_listJobsFiltered=_listJobsAll.Select(x=>x.Copy()).ToList();//start with all jobs then pare down.
			//Title Filter
			if(!string.IsNullOrWhiteSpace(textTitle.Text)) {
				long jobNum=0;
				long.TryParse(textTitle.Text,out jobNum);
				_listJobsFiltered=_listJobsFiltered.FindAll(x=>x.Title.Contains(textTitle.Text) || x.JobNum==jobNum);
			}
			//Expert filter
			if(listBoxExpert.SelectedIndices.Count>0) {
				List<long> expertNums=listBoxExpert.SelectedIndices.Cast<int>().Select(x=>_listUserExperts[x].UserNum).ToList();
				_listJobsFiltered=_listJobsFiltered.FindAll(x=>expertNums.Contains(x.ExpertNum));
			}
			//Owner filter
			if(listBoxOwner.SelectedIndices.Count>0) {
				List<long> ownerNums=listBoxOwner.SelectedIndices.Cast<int>().Select(x => _listUserOwners[x].UserNum).ToList();
				_listJobsFiltered=_listJobsFiltered.FindAll(x => ownerNums.Contains(x.OwnerNum));
			}
			//Status Filter
			if(listBoxStatus.SelectedIndices.Count>0) {
				List<JobStat> listStats=listBoxStatus.SelectedIndices.Cast<JobStat>().ToList();
				_listJobsFiltered=_listJobsFiltered.FindAll(x => listStats.Contains(x.JobStatus));
			}
			//Category Filter
			if(listBoxCategory.SelectedIndices.Count>0) {
				List<JobCategory> listCats=listBoxCategory.SelectedIndices.Cast<JobCategory>().ToList();
				_listJobsFiltered=_listJobsFiltered.FindAll(x => listCats.Contains(x.Category));
			}
			if(!string.IsNullOrWhiteSpace(textTask.Text)) {
				long taskNumIn=0;
				long.TryParse(textTask.Text,out taskNumIn);
				long[] taskNums=_listTasksAll.FindAll(x=>x.Descript.Contains(textTask.Text) || x.TaskNum==taskNumIn).Select(x=>x.TaskNum).ToArray();
				_listJobsFiltered=_listJobsFiltered.FindAll(x=>x.ListJobLinks.Any(y=>y.LinkType==JobLinkType.Task && taskNums.Contains(y.FKey)));
			}
			if(!string.IsNullOrWhiteSpace(textFeatReq.Text)) {
				long featReqIn=0;
				long.TryParse(textFeatReq.Text,out featReqIn);
				long[] featReqNums=_listFeatureRequestsAll.FindAll(x=>x.FeatReqNum==featReqIn || x.Description.Contains(textFeatReq.Text)).Select(x=>x.FeatReqNum).ToArray();
				_listJobsFiltered=_listJobsFiltered.FindAll(x=>x.ListJobLinks.Any(y=>y.LinkType==JobLinkType.Request && featReqNums.Contains(y.FKey)));
			}
			if(!string.IsNullOrWhiteSpace(textCust.Text)) {
				long patNumIn=0;
				long.TryParse(textCust.Text,out patNumIn);
				long[] patNums=_listPatientAll.FindAll(x=>x.PatNum==patNumIn || x.GetNameFL().Contains(textCust.Text)).Select(x=>x.PatNum).ToArray();
				long[] taskNums=_listTasksAll.FindAll(x=>x.ObjectType==TaskObjectType.Patient && (x.KeyNum==patNumIn || patNums.Contains(x.KeyNum))).Select(x=>x.TaskNum).ToArray();
				_listJobsFiltered=_listJobsFiltered.FindAll(x=>x.ListJobQuotes.Any(y=>patNums.Contains(y.PatNum)) || x.ListJobLinks.Any(y=>y.LinkType==JobLinkType.Task&& taskNums.Contains(y.FKey)));
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Columns.Add(new ODGridColumn("Item",100));
			gridMain.Columns.Add(new ODGridColumn("Job Title",470));
			gridMain.Columns.Add(new ODGridColumn("Expert",55));
			gridMain.Columns.Add(new ODGridColumn("Owner",55));
			gridMain.Columns.Add(new ODGridColumn("Status",230));
			gridMain.Columns.Add(new ODGridColumn("Date",70));
			gridMain.Rows.Clear();
			foreach(Job jobCur in _listJobsFiltered) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(jobCur.Category.ToString().Substring(0,1)+jobCur.JobNum);
				row.Cells.Add(jobCur.Title);
				row.Cells.Add(Userods.GetName(jobCur.ExpertNum));
				row.Cells.Add(Userods.GetName(jobCur.OwnerNum));
				row.Cells.Add(jobCur.JobStatus.ToString());
				row.Cells.Add(jobCur.DateTimeEntry.ToShortDateString());
				row.Tag=jobCur;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private class FeatureRequest {
			public long FeatReqNum;
			public long TotalVotes;
			public long Difficulty;
			public float Weight;
			public string Approval;
			public string Description;

			public static List<FeatureRequest> GetAll() {
				//prepare the xml document to send--------------------------------------------------------------------------------------
				XmlWriterSettings settings=new XmlWriterSettings();
				settings.Indent=true;
				settings.IndentChars=("    ");
				StringBuilder strbuild=new StringBuilder();
				using(XmlWriter writer=XmlWriter.Create(strbuild,settings)) {
					writer.WriteStartElement("FeatureRequestGetList");
					writer.WriteStartElement("RegistrationKey");
					writer.WriteString(PrefC.GetString(PrefName.RegistrationKey));
					writer.WriteEndElement();
					writer.WriteStartElement("SearchString");
					writer.WriteString("");
					writer.WriteEndElement();
					writer.WriteEndElement();
				}
#if DEBUG
				OpenDental.localhost.Service1 updateService=new OpenDental.localhost.Service1();
#else
				OpenDental.customerUpdates.Service1 updateService=new OpenDental.customerUpdates.Service1();
				updateService.Url=PrefC.GetString(PrefName.UpdateServerAddress);
			#endif
				//Send the message and get the result-------------------------------------------------------------------------------------
				string result="";
				try {
					result=updateService.FeatureRequestGetList(strbuild.ToString());
				}
				catch(Exception ex) {
					throw;
				}
				XmlDocument doc=new XmlDocument();
				doc.LoadXml(result);
				//Process errors------------------------------------------------------------------------------------------------------------
				XmlNode node=doc.SelectSingleNode("//Error");
				if(node!=null) {
					throw new Exception("Error");
				}
				node=doc.SelectSingleNode("//KeyDisabled");
				if(node==null) {
					//no error, and no disabled message
					if(Prefs.UpdateBool(PrefName.RegistrationKeyIsDisabled,false)) {//this is one of two places in the program where this happens.
						DataValid.SetInvalid(InvalidType.Prefs);
					}
				}
				else {
					//textConnectionMessage.Text=node.InnerText;
					if(Prefs.UpdateBool(PrefName.RegistrationKeyIsDisabled,true)) {//this is one of two places in the program where this happens.
						DataValid.SetInvalid(InvalidType.Prefs);
					}
					throw new Exception(node.InnerText);
				}
				//Process a valid return value------------------------------------------------------------------------------------------------
				node=doc.SelectSingleNode("//ResultTable");
				ODDataTable table=new ODDataTable(node.InnerXml);
				List<FeatureRequest> retVal=new List<FeatureRequest>();
				foreach(var dataRow in table.Rows) {
					FeatureRequest req=new FeatureRequest() {
						FeatReqNum=PIn.Long(dataRow["RequestId"].ToString()),
						TotalVotes=PIn.Long(dataRow["totalVotes"].ToString()),
						Difficulty=PIn.Long(dataRow["Difficulty"].ToString()),
						Weight=PIn.Float(dataRow["Weight"].ToString()),
						Approval=dataRow["Weight"].ToString(),
						Description=dataRow["Description"].ToString()
					};
					retVal.Add(req);
				}
				return retVal;
			}
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			if(e.Row<0 || e.Row>gridMain.Rows.Count || !(gridMain.Rows[e.Row].Tag is Job)) {
				_selectedJob=null;
				return;
			}
			_selectedJob=(Job)gridMain.Rows[e.Row].Tag;
		}

		private void butOK_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(e.Row<0 || e.Row>gridMain.Rows.Count || !(gridMain.Rows[e.Row].Tag is Job)) {
				_selectedJob=null;
				return;
			}
			_selectedJob=(Job)gridMain.Rows[e.Row].Tag;
			DialogResult=DialogResult.OK;
		}

	}
}