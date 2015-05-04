using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Perceptive.ARR.HelperLibrary;

namespace Perceptive.ARRViewer
{
    public partial class Tasks : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPageControlValues();
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            SetPageControlValues();
        }

        protected void btnManulProcessing_Click(object sender, EventArgs e)
        {
            RestServiceUtility utility = new RestServiceUtility();
            bool result = utility.ProcessRequest<bool>(new WebRequestInput()
            {
                Method = Constants.MethodGet,
                Uri = RestServiceUtility.ProcessLogsUrl("DeleteLogs", int.Parse(txtDays.Text))
            });

            lblMessage.Text = (result) ? "Task processing started successfully. Click on refresh to get the current status" : "Task could not be processed. Please try again later";
            lblMessage.ForeColor = (result) ? System.Drawing.Color.Black : System.Drawing.Color.Red;
        }

        private void SetPageControlValues()
        {
            RestServiceUtility utility = new RestServiceUtility();
            SchedulerProperty property = utility.ProcessRequest<SchedulerProperty>(new WebRequestInput()
            {
                Method = Constants.MethodGet,
                Uri = RestServiceUtility.GetSchedulerUrl()
            });

            if (property != null)
            {   
                lblMessage.Text = property.ActiveProcessStatus;
                lblMessage.ForeColor = (property.ActiveProcessStatus == Constants.NoActiveProgess) ? System.Drawing.Color.Black : System.Drawing.Color.Red;
                btnManulProcessing.Enabled = property.ActiveProcessStatus == Constants.NoActiveProgess;
            }
        }

        protected void btnArchive_Click(object sender, EventArgs e)
        {
            RestServiceUtility utility = new RestServiceUtility();
            bool result = utility.ProcessRequest<bool>(new WebRequestInput()
            {
                Method = Constants.MethodGet,
                Uri = RestServiceUtility.ProcessLogsUrl("ArchiveLogs", 1)
            });

            lblMessage.Text = (result) ? "Task processing started successfully. Click on refresh to get the current status" : "Task could not be processed. Please try again later";
            lblMessage.ForeColor = (result) ? System.Drawing.Color.Black : System.Drawing.Color.Red;
        }
    }
}