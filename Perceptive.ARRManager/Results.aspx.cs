using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.Data;
using System.Text;
using Perceptive.ARR.HelperLibrary;

namespace Perceptive.ARRViewer
{
    public partial class Results : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var user = Helper.GetUserData(Context.User.Identity.Name);

                RestServiceUtility utility = new RestServiceUtility();
                txtResult.Text = utility.ProcessRequest<AuditLogData>(new WebRequestInput()
                {
                    Method = Constants.MethodGet,
                    Uri = RestServiceUtility.GetSpecificLogUrl(new Guid(Request.QueryString["logId"]), user.UserId)

                }).RawMessage;
            }
            catch (Exception ex)
            {
                Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Website);
            }
        }

        private void DisplayData(DataTable dt)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName.Equals("LogId"))
                    continue;
                builder.AppendLine(dt.Columns[i].ColumnName);
                builder.AppendLine();
                builder.AppendLine(dt.Rows[0][i].ToString());
                builder.AppendLine();
                builder.AppendLine("****************************");
                builder.AppendLine();
            }
            txtResult.Text = builder.ToString();
        }
    }
}