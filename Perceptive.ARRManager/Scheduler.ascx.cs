using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Perceptive.ARR.HelperLibrary;

namespace Perceptive.ARRViewer
{
    public partial class Scheduler : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                DisplaySchedulerProperties();
        }

        private void DisplaySchedulerProperties()
        {
            RestServiceUtility utility = new RestServiceUtility();
            SchedulerProperty property = utility.ProcessRequest<SchedulerProperty>(new WebRequestInput()
            {
                Method = Constants.MethodGet,
                Uri = RestServiceUtility.GetSchedulerUrl()
            });

            if (property != null)
            {                
                chkArchive.Checked = property.IsRunning;
                txtArchiveDays.Text = property.ArchiveDays.ToString();
                lblServerStatus.Text = property.ActiveProcessStatus;
                lblServerStatus.ForeColor = (property.ActiveProcessStatus == Constants.NoActiveProgess) ? System.Drawing.Color.Black : System.Drawing.Color.Red;
                btnScheduler.Enabled = property.ActiveProcessStatus == Constants.NoActiveProgess;
            }
        }

        protected void btnScheduler_Click(object sender, EventArgs e)
        {
            SchedulerProperty property = new SchedulerProperty();
            property.IsRunning = chkArchive.Checked;
            property.ArchiveDays = int.Parse(txtArchiveDays.Text);
            property.ActiveProcessStatus = lblServerStatus.Text;

            RestServiceUtility utility = new RestServiceUtility();
            bool result = utility.ProcessRequest<bool>(new WebRequestInput()
            {
                Method = Constants.MethodPost,
                Uri = RestServiceUtility.SetSchedulerUrl(),
                RequestData = property
            });

            if (!result)
                lblError.Text = "Setting could not be set because scheduler process is active. Please try again later.";

            DisplaySchedulerProperties();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            DisplaySchedulerProperties();
        }
    }
}