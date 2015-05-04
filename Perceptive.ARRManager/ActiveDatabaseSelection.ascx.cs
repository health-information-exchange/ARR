using Perceptive.ARR.HelperLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Perceptive.ARRViewer
{
    public partial class ActiveDatabaseSelection : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                PopulateDatabaseList();
        }

        protected void btnSetActiveDB_Click(object sender, EventArgs e)
        {
            var user = Helper.GetUserData(Context.User.Identity.Name);
            var utility = new RestServiceUtility();
            bool result = utility.ProcessRequest<bool>(new WebRequestInput()
            {
                Method = Constants.MethodGet,
                Uri = RestServiceUtility.SetDatabaseListUrl(user.UserName, ddlActiveDatabase.SelectedValue)
            });
        }

        private void PopulateDatabaseList()
        {
            var user = Helper.GetUserData(Context.User.Identity.Name);
            var utility = new RestServiceUtility();
            List<Db> result = utility.ProcessRequest<List<Db>>(new WebRequestInput()
            {
                Method = Constants.MethodGet,
                Uri = RestServiceUtility.GetDatabaseListUrl(user.UserName)
            });

            ddlActiveDatabase.Items.Clear();
            foreach (var item in result)
                ddlActiveDatabase.Items.Add(new ListItem(item.Name, item.Name));

            ddlActiveDatabase.Items.FindByText(result.First(l => l.IsActive).Name).Selected = true;
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateDatabaseList();
        }
    }
}