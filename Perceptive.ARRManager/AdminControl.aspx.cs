using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Perceptive.ARR.HelperLibrary;

namespace Perceptive.ARRViewer
{
    public partial class AdminControl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnTab1.CssClass = "Clicked";
                MainView.ActiveViewIndex = 0;
                var user = Helper.GetUserData(Context.User.Identity.Name);
                Welcome.Text = "Hello, " + user.UserName;
                SetTabVisibility(user.Role);
            }
        }

        private void SetTabVisibility(UserRole role)
        {
            switch (role)
            {
                case UserRole.Admin:
                case UserRole.Super:
                    break;

                case UserRole.Power:
                    btnTab6.Visible = false;
                    btnTab5.Visible = false;
                    btnTab4.Visible = false;
                    break;

                case UserRole.Viewer:
                    btnTab6.Visible = false;
                    btnTab5.Visible = false;
                    btnTab4.Visible = false;
                    btnTab3.Visible = false;
                    btnTab2.Visible = false;
                    break;

                case UserRole.Unauthorized:
                default:
                    btnTab7.Visible = false;
                    btnTab6.Visible = false;
                    btnTab5.Visible = false;
                    btnTab4.Visible = false;
                    btnTab3.Visible = false;
                    btnTab2.Visible = false;
                    btnTab1.Visible = false;
                    break;
            }
        }

        protected void btnTab1_Click(object sender, EventArgs e)
        {
            btnTab1.CssClass = "Clicked";
            btnTab2.CssClass = "Initial";
            btnTab3.CssClass = "Initial";
            btnTab4.CssClass = "Initial";
            btnTab5.CssClass = "Initial";
            btnTab6.CssClass = "Initial";
            btnTab7.CssClass = "Initial";
            MainView.ActiveViewIndex = 0;
        }

        protected void btnTab2_Click(object sender, EventArgs e)
        {
            btnTab1.CssClass = "Initial";
            btnTab2.CssClass = "Clicked";
            btnTab3.CssClass = "Initial";
            btnTab4.CssClass = "Initial";
            btnTab5.CssClass = "Initial";
            btnTab6.CssClass = "Initial";
            btnTab7.CssClass = "Initial";
            MainView.ActiveViewIndex = 1;
        }

        protected void btnTab3_Click(object sender, EventArgs e)
        {
            btnTab1.CssClass = "Initial";
            btnTab2.CssClass = "Initial";
            btnTab3.CssClass = "Clicked";
            btnTab4.CssClass = "Initial";
            btnTab5.CssClass = "Initial";
            btnTab6.CssClass = "Initial";
            btnTab7.CssClass = "Initial";
            MainView.ActiveViewIndex = 2;
        }

        protected void btnTab4_Click(object sender, EventArgs e)
        {
            btnTab1.CssClass = "Initial";
            btnTab2.CssClass = "Initial";
            btnTab3.CssClass = "Initial";
            btnTab4.CssClass = "Clicked";
            btnTab5.CssClass = "Initial";
            btnTab6.CssClass = "Initial";
            btnTab7.CssClass = "Initial";
            MainView.ActiveViewIndex = 3;
        }

        protected void btnTab5_Click(object sender, EventArgs e)
        {
            btnTab1.CssClass = "Initial";
            btnTab2.CssClass = "Initial";
            btnTab3.CssClass = "Initial";
            btnTab4.CssClass = "Initial";
            btnTab5.CssClass = "Clicked";
            btnTab6.CssClass = "Initial";
            btnTab7.CssClass = "Initial";
            MainView.ActiveViewIndex = 4;
        }

        protected void btnTab6_Click(object sender, EventArgs e)
        {
            btnTab1.CssClass = "Initial";
            btnTab2.CssClass = "Initial";
            btnTab3.CssClass = "Initial";
            btnTab4.CssClass = "Initial";
            btnTab5.CssClass = "Initial";
            btnTab6.CssClass = "Clicked";
            btnTab7.CssClass = "Initial";
            MainView.ActiveViewIndex = 5;
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("RecordViewer.aspx");
        }

        protected void btnTab7_Click(object sender, EventArgs e)
        {
            btnTab1.CssClass = "Initial";
            btnTab2.CssClass = "Initial";
            btnTab3.CssClass = "Initial";
            btnTab4.CssClass = "Initial";
            btnTab5.CssClass = "Initial";
            btnTab6.CssClass = "Initial";
            btnTab7.CssClass = "Clicked";
            MainView.ActiveViewIndex = 6;
        }
    }
}