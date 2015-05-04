using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Perceptive.ARR.HelperLibrary;

namespace Perceptive.ARRViewer
{
    public partial class Appsettings : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                PopulateGrid();
        }

        private void PopulateGrid()
        {
            var utility = new RestServiceUtility();
            List<AppSettingItem> result = utility.ProcessRequest<List<AppSettingItem>>(new WebRequestInput()
            {
                Method = Constants.MethodGet,
                Uri = RestServiceUtility.GetAppSettingUrl()
            });

            resultGrid.DataSource = result;
            resultGrid.DataBind();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string key;
            string value;

            List<AppSettingItem> updatedSettings = new List<AppSettingItem>();

            foreach (GridViewRow row in resultGrid.Rows)
            {
                key = ((Label)row.Cells[0].FindControl("lblKey")).Text;
                value = ((TextBox)row.Cells[1].FindControl("txtValue")).Text;
                updatedSettings.Add(new AppSettingItem() { Key = key, Value = value });
            }

            var utility = new RestServiceUtility();
            bool result = utility.ProcessRequest<bool>(new WebRequestInput()
            {
                Method = Constants.MethodPost,
                Uri = RestServiceUtility.SetAppSettingUrl(),
                RequestData = updatedSettings
            });

            if(result)
                PopulateGrid();
        }
    }
}