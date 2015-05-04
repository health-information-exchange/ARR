using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Perceptive.ARR.HelperLibrary;

namespace Perceptive.ARRViewer
{
    public partial class Users : System.Web.UI.UserControl
    {
        private class UserDisplay
        {
            public bool Enabled { get; set; }
            public Guid UserId { get; set; }
            public string UserName { get; set; }
            public UserRole Role { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                PopulateData();
        }

        private void PopulateDropdownlist(UserRole role)
        {
            switch (role)
            {
                case UserRole.Super:
                    ddlRole.Items.Add(new ListItem(UserRole.Viewer.ToString(), "0"));
                    ddlRole.Items.Add(new ListItem(UserRole.Power.ToString(), "1"));
                    ddlRole.Items.Add(new ListItem(UserRole.Admin.ToString(), "2"));
                    break;
                case UserRole.Admin:
                default:
                    ddlRole.Items.Add(new ListItem(UserRole.Viewer.ToString(), "0"));
                    ddlRole.Items.Add(new ListItem(UserRole.Power.ToString(), "1"));
                    break;
            }
            ddlRole.SelectedIndex = 0;
        }

        private List<UserDisplay> GetUsers(UserRole userRole)
        {
            List<UserDisplay> users = new List<UserDisplay>();

            RestServiceUtility utility = new RestServiceUtility();
            List<User> result = utility.ProcessRequest<List<User>>(new WebRequestInput()
            {
                Method = Constants.MethodGet,
                Uri = RestServiceUtility.GetUsersUrl()
            });

            foreach(var user in result)
            {
                UserDisplay u = new UserDisplay();
                u.Role = user.Role;
                u.UserId = user.UserId;
                u.UserName = user.UserName;
                u.Enabled = CanDelete(userRole, u.Role);
                users.Add(u);
            }

            return users;
        }

        private void PopulateData()
        {
            var user = Helper.GetUserData(Context.User.Identity.Name);
            PopulateDropdownlist(user.Role);
            PopulateGrid(user.Role);
        }

        private void PopulateGrid(UserRole userRole)
        {
            resultGrid.DataSource = GetUsers(userRole);
            resultGrid.DataBind();
        }

        private static bool CanDelete(UserRole userRole, UserRole memberRole)
        {
            if (userRole == UserRole.Super && memberRole != UserRole.Super)
                return true;

            if (userRole == UserRole.Admin && (memberRole != UserRole.Admin && memberRole != UserRole.Super))
                return true;

            return false;
        }

        protected void resultGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DELETE")
            {
                LinkButton lnkDelete = (LinkButton)e.CommandSource;
                string userId = lnkDelete.CommandArgument;

                RestServiceUtility utility = new RestServiceUtility();
                bool result = utility.ProcessRequest<bool>(new WebRequestInput()
                {
                    Method = Constants.MethodGet,
                    Uri = RestServiceUtility.DeleteUserUrl(userId)
                });

                if (result)
                {
                    var user = Helper.GetUserData(Context.User.Identity.Name);
                    PopulateGrid(user.Role);
                }
            }
        }

        protected void resultGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            lblErrorMessage.Text = string.Empty;
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
                lblErrorMessage.Text = "User Name cannot be empty. ";
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
                lblErrorMessage.Text += "Password cannot be empty.";

            if (string.IsNullOrEmpty(lblErrorMessage.Text))
            {
                User newUser = new User() { Password = txtPassword.Text.Trim(), UserName = txtUserName.Text.Trim()};
                newUser.Role = (UserRole)Enum.ToObject(typeof(UserRole), Convert.ToInt32(ddlRole.Items[ddlRole.SelectedIndex].Value));

                var utility = new RestServiceUtility();
                bool result = utility.ProcessRequest<bool>(new WebRequestInput()
                {
                    Method = Constants.MethodPost,
                    Uri = RestServiceUtility.ModifyUserUrl(),
                    RequestData = newUser
                });

                if (result)
                {
                    var user = Helper.GetUserData(Context.User.Identity.Name);
                    PopulateGrid(user.Role);
                    txtUserName.Text = txtPassword.Text = string.Empty;
                    ddlRole.SelectedIndex = 0;
                }
                else
                    lblErrorMessage.Text = "User could not be added. Check for duplicate entry.";
            }
        }
    }
}