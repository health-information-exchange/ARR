using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Perceptive.SPA.ARRViewer
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string fullName = HttpContext.Current.User.Identity.Name;
                txtWelcome.InnerText = fullName.Substring(fullName.IndexOf(@"\") + 1);
            }
        }
    }
}