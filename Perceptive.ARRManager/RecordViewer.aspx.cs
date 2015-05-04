using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using Perceptive.ARR.HelperLibrary;
using System.Web.Security;

namespace Perceptive.ARRViewer
{
    public partial class RecordViewer : System.Web.UI.Page
    {
        private bool ProceedNext
        {
            get { return Convert.ToBoolean(ViewState["ProceedNext"]); }
            set { ViewState["ProceedNext"] = value; }
        }

        private int LastStartingRowNumber
        {
            get { return Convert.ToInt32(ViewState["LastStartingRowNumber"]); }
            set { ViewState["LastStartingRowNumber"] = value; }
        }

        private int LogsToDisplay
        {
            get { return Convert.ToInt32(ViewState["LogsToDisplay"]); }
            set { ViewState["LogsToDisplay"] = value; }
        }

        private List<SearchResult> Data
        {
            get { return ViewState["Data"] as List<SearchResult>; }
            set { ViewState["Data"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    var user = Helper.GetUserData(Context.User.Identity.Name);
                    Welcome.Text = "Hello, " + user.UserName;
                    btnNext.Visible = btnPrevious.Visible = false;
                    LogsToDisplay = Convert.ToInt32(ConfigurationManager.AppSettings["NoOfLogsToDisplay"]);
                    ProceedNext = true;
                    LastStartingRowNumber = 0;
                    //PopulateDataGrid();
                    ShowSearch(true);
                }
            }
            catch (Exception ex)
            {
                Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Website);
            }
        }

        private void PopulateDataGrid()
        {
            Data = GetLogFiles();
            resultGrid.DataSource = Data;
            resultGrid.DataBind();
            if (ProceedNext)
            {
                LastStartingRowNumber = (LastStartingRowNumber == 0) ? 1 : LastStartingRowNumber + LogsToDisplay;
                btnNext.Visible = (Data.Count == LogsToDisplay);
                btnPrevious.Visible = (LastStartingRowNumber != 1);
            }
            else
            {
                LastStartingRowNumber -= LogsToDisplay;
                btnNext.Visible = true;
                btnPrevious.Visible = LastStartingRowNumber != 1;
            }
        }

        private SearchFilter GetQueryFilter()
        {
            var user = Helper.GetUserData(Context.User.Identity.Name);

            SearchFilter filter = new SearchFilter() { UserName = user.UserName };

            # region LogRecorder

            if (!string.IsNullOrEmpty(txtIP.Text))
                filter.IPAddress = txtIP.Text;

            filter.Protocol = (MessageProtocol)Enum.Parse(typeof(MessageProtocol), ddlLogType.SelectedValue);
            filter.IsValid = ddlDataType.SelectedIndex == 0;
            string[] split;
            if (!string.IsNullOrEmpty(txtStartDate.Text))
            {
                split = txtStartDate.Text.Split("-".ToCharArray());
                filter.LoggedFrom = new DateTime(Convert.ToInt32(split[0]), Convert.ToInt32(split[1]), Convert.ToInt32(split[2]),
                        Convert.ToInt32(ddlStartTime.SelectedValue) + Convert.ToInt32(ddlStartFormat.SelectedValue), 0, 0, DateTimeKind.Local);
            }

            if (!string.IsNullOrEmpty(txtEndDate.Text))
            {
                split = txtEndDate.Text.Split("-".ToCharArray());
                filter.LoggedTill = new DateTime(Convert.ToInt32(split[0]), Convert.ToInt32(split[1]), Convert.ToInt32(split[2]),
                        Convert.ToInt32(ddlEndTime.SelectedValue) + Convert.ToInt32(ddlEndFormat.SelectedValue), 0, 0, DateTimeKind.Local);
            }

            # endregion

            # region Global Search

            if (!string.IsNullOrEmpty(txtGlobalSearch.Text))
                filter.SearchText = txtGlobalSearch.Text.Trim();            

            # endregion

            # region Valid Logs

            if (!string.IsNullOrEmpty(txtHost.Text))
                filter.HostName = txtHost.Text;

            if (!string.IsNullOrEmpty(txtApp.Text))
                filter.AppName = txtApp.Text;

            if (!string.IsNullOrEmpty(txtSendStartDate.Text))
            {
                split = txtSendStartDate.Text.Split("-".ToCharArray());
                filter.SentAfter = new DateTime(Convert.ToInt32(split[0]), Convert.ToInt32(split[1]), Convert.ToInt32(split[2]),
                        Convert.ToInt32(ddlSendStartTime.SelectedValue) + Convert.ToInt32(ddlSendStartFormat.SelectedValue), 0, 0, DateTimeKind.Local);
            }

            if (!string.IsNullOrEmpty(txtSendEndDate.Text))
            {
                split = txtSendEndDate.Text.Split("-".ToCharArray());
                filter.SentBefore = new DateTime(Convert.ToInt32(split[0]), Convert.ToInt32(split[1]), Convert.ToInt32(split[2]),
                        Convert.ToInt32(ddlSendEndTime.SelectedValue) + Convert.ToInt32(ddlSendEndFormat.SelectedValue), 0, 0, DateTimeKind.Local);
            }

            # endregion

            # region Event Identification

            if (!string.IsNullOrEmpty(txtEventIdCode.Text))
                filter.EventId_Code = txtEventIdCode.Text;

            if (!string.IsNullOrEmpty(txtEventIdCodeSystemName.Text))
                filter.EventId_CodeSystemName = txtEventIdCodeSystemName.Text;

            if (!string.IsNullOrEmpty(txtEventIdDisplayName.Text))
                filter.EventId_DisplayName = txtEventIdDisplayName.Text;

            if (!string.IsNullOrEmpty(txtEventTypeCodeCode.Text))
                filter.EventTypeCode_Code = txtEventTypeCodeCode.Text;

            if (!string.IsNullOrEmpty(txtEventTypeCodeCodeSystemName.Text))
                filter.EventTypeCode_CodeSystemName = txtEventTypeCodeCodeSystemName.Text;

            if (!string.IsNullOrEmpty(txtEventTypeCodeDisplayName.Text))
                filter.EventTypeCode_DisplayName = txtEventTypeCodeDisplayName.Text;

            if (!string.IsNullOrEmpty(txtEventOutcomeIndicator.Text))
                filter.EventOutcomeIndicator = txtEventOutcomeIndicator.Text;

            if (!string.IsNullOrEmpty(txtEventDateTime.Text))
                filter.EventDateTime = txtEventDateTime.Text;

            if (!string.IsNullOrEmpty(txtEventActionCode.Text))
                filter.EventActionCode = txtEventActionCode.Text;

            # endregion

            # region Audit Source Identification

            if (!string.IsNullOrEmpty(txtAuditSourceId.Text))
                filter.AuditSourceId = txtAuditSourceId.Text;

            if (!string.IsNullOrEmpty(txtAuditEnterpriseSiteId.Text))
                filter.AuditEnterpriseSiteId = txtAuditEnterpriseSiteId.Text;

            if (!string.IsNullOrEmpty(txtAuditSourceTypeCode.Text))
                filter.AuditSourceTypeCode = txtAuditSourceTypeCode.Text;

            if (!string.IsNullOrEmpty(txtAuditSourceCodeSystemName.Text))
                filter.AuditSourceCodeSystemName = txtAuditSourceCodeSystemName.Text;

            if (!string.IsNullOrEmpty(txtAuditSourceOriginalText.Text))
                filter.AuditSourceOriginalText = txtAuditSourceOriginalText.Text;           

            # endregion

            # region Active Participant

            if (!string.IsNullOrEmpty(txtSUId.Text))
                filter.SourceUserId = txtSUId.Text;

            if (!string.IsNullOrEmpty(txtSNId.Text))
                filter.SourceNetworkAccessPointId = txtSNId.Text;

            if (!string.IsNullOrEmpty(txtHUId.Text))
                filter.HumanRequestorUserId = txtHUId.Text;

            if (!string.IsNullOrEmpty(txtDUId.Text))
                filter.DestinationUserId = txtDUId.Text;

            if (!string.IsNullOrEmpty(txtDNId.Text))
                filter.DestinationNetworkAccessPointId = txtDNId.Text;

            # endregion       

            # region Participant Object

            if (!string.IsNullOrEmpty(txtPObjId.Text))
                filter.ParticipantObjectId = txtPObjId.Text;

            # endregion

            # region Data Counter

            filter.LastStartingRowNumber = LastStartingRowNumber;
            filter.RetrieveNext = ProceedNext;
            filter.MaxLogsToRetrieve = LogsToDisplay;

            # endregion

            return filter;
        }

        private List<SearchResult> GetLogFiles()
        {
            RestServiceUtility utility = new RestServiceUtility();
            List<SearchResult> result = utility.ProcessRequest<List<SearchResult>>(new WebRequestInput()
            {
                Method = Constants.MethodPost,
                Uri = RestServiceUtility.GetLogsUrl(),
                RequestData = GetQueryFilter()

            });

            return result;
        }

        protected void SignOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("Logon.aspx");
        }

        protected void resultGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            resultGrid.PageIndex = ((GridViewPageEventArgs)e).NewPageIndex;
            resultGrid.DataSource = Data;
            resultGrid.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ProceedNext = true;
            LastStartingRowNumber = 0;
            ShowSearch(false);
            PopulateDataGrid();
        }

        protected void btnShowSearchPanel_Click(object sender, EventArgs e)
        {
            ShowSearch(btnShowSearchPanel.Text.Equals("Show Filters"));
        }

        private void ShowSearch(bool displayPanel)
        {
            pnlSearch.Visible = displayPanel;
            btnShowSearchPanel.Text = (displayPanel) ? "Hide Filters" : "Show Filters";
            //pnlSearchButton.Visible = !displayPanel;
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            ProceedNext = false;
            PopulateDataGrid();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            ProceedNext = true;
            PopulateDataGrid();
        }

        protected void lnkManagement_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminControl.aspx");
        }
    }
}