using Perceptive.ARR.RestService;
using System;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;

namespace Perceptive.ARR.Service.Installer
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            RouteTable.Routes.Add(new ServiceRoute("", new WebServiceHostFactory(), typeof(RepositoryManager)));
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "http://localhost:9000");
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "POST, PUT, DELETE");

                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }
        }
    }
}
