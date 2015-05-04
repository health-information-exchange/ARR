using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Perceptive.ARR.HelperLibrary;

namespace Perceptive.ARRViewer
{
    public partial class SupportedLogs : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateGridView();
            }
        }

        private void PopulateGridView()
        {
            RestServiceUtility utility = new RestServiceUtility();
            resultGrid.DataSource = utility.ProcessRequest<List<SupportedLog>>(new WebRequestInput()
            {
                Method = Constants.MethodGet,
                Uri = RestServiceUtility.GetSupportedLogTypesUrl()
            });
            resultGrid.DataBind();
        }

        protected void btnAddSupportedLogType_Click(object sender, EventArgs e)
        {
            lblErrorMessage.Text = string.Empty;
            if (string.IsNullOrEmpty(txtCode.Text.Trim()))
                lblErrorMessage.Text = "Code cannot be empty. ";
            if (string.IsNullOrEmpty(txtDisplayName.Text.Trim()))
                lblErrorMessage.Text += "DisplayName cannot be empty.";

            if (string.IsNullOrEmpty(lblErrorMessage.Text))
            {
                RestServiceUtility utility = new RestServiceUtility();
                bool result = utility.ProcessRequest<bool>(new WebRequestInput()
                {
                    Method = Constants.MethodGet,
                    Uri = RestServiceUtility.AddSupportedLogTypeUrl(txtCode.Text.Trim(), txtDisplayName.Text.Trim())
                });
                if (result)
                {
                    PopulateGridView();
                    txtCode.Text = txtDisplayName.Text = string.Empty;
                }
            }
        }

        protected void resultGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DELETE")
            {
                LinkButton lnkView = (LinkButton)e.CommandSource;
                string[] arguements = lnkView.CommandArgument.Split("|".ToCharArray());

                RestServiceUtility utility = new RestServiceUtility();
                bool result = utility.ProcessRequest<bool>(new WebRequestInput()
                {
                    Method = Constants.MethodGet,
                    Uri = RestServiceUtility.DeleteSupportedLogTypeUrl(arguements[0], arguements[1])
                });
                if (result)
                    PopulateGridView();
            }
        }

        protected void resultGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}