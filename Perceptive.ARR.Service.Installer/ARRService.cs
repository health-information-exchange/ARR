using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using Perceptive.ARR.RestService;

namespace Perceptive.ARR.Service.Installer
{
    partial class ARRService : ServiceBase
    {
        ServiceHost managerServiceHost;

        public ARRService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            AuditMessageReceiverCacheService.Instance.Start();
            managerServiceHost = new ServiceHost(typeof(RepositoryManager));
            managerServiceHost.Open();
        }

        protected override void OnStop()
        {
            managerServiceHost.Close();
            AuditMessageReceiverCacheService.Instance.Dispose();
        }
    }
}
