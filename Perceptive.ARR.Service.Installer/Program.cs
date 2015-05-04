using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace Perceptive.ARR.Service.Installer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBase[] servicesToRun;
            servicesToRun = new ServiceBase[] { new ARRService() };
            ServiceBase.Run(servicesToRun);
        }
    }
}
