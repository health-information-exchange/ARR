using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Perceptive.ARR.HelperLibrary;
using System.Web.Security;

namespace Perceptive.ARRViewer
{
    public partial class AccountManager : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateData();
            }
        }

        private void PopulateData()
        {
            var user = Helper.GetUserData(Context.User.Identity.Name);
            txtUserName.Text = user.UserName;
            txtPassword.Attributes["value"] = user.Password;
            txtRole.Text = user.Role.ToString();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CancelEdit();
        }

        private void CancelEdit()
        {
            txtUserName.ReadOnly = txtPassword.ReadOnly = true;
            btnModify.Text = "Edit";
            btnCancel.Visible = false;
            lblMessage.Text = string.Empty;
            PopulateData();
        }

        protected void btnModify_Click(object sender, EventArgs e)
        {
            if (btnModify.Text == "Edit")
            {
                txtUserName.ReadOnly = txtPassword.ReadOnly = false;
                btnModify.Text = "Update";
                btnCancel.Visible = true;
                lblMessage.Text = "Re login is required if user profile is updated";
            }
            else
            {
                string modifiedUserName = txtUserName.Text.Trim();
                string modifiedPassword = txtPassword.Text.Trim();

                if (string.IsNullOrEmpty(modifiedUserName) || string.IsNullOrEmpty(modifiedPassword))
                {
                    lblMessage.Text = "Both user name and password fields are mandatory. Please try again";
                    return;
                }

                var user = Helper.GetUserData(Context.User.Identity.Name);
                if (!user.UserName.Equals(modifiedUserName))
                    user.UserName = modifiedUserName;
                else
                {
                    if (user.Password.Equals(modifiedPassword))
                    {
                        CancelEdit();
                        lblMessage.Text = "Modified user credential match existing one.";
                        return;
                    }
                }

                user.Password = modifiedPassword;                

                var utility = new RestServiceUtility();
                bool result = utility.ProcessRequest<bool>(new WebRequestInput()
                {
                    Method = Constants.MethodPost,
                    Uri = RestServiceUtility.ModifyUserUrl(),
                    RequestData =  user
                });

                if (result)
                {
                    FormsAuthentication.SignOut();
                    Response.Redirect("Logon.aspx");
                }
            }
        }
    }
}