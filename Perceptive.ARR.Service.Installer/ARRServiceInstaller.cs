using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace Perceptive.ARR.Service.Installer
{
    [RunInstaller(true)]
    public partial class ARRServiceInstaller : System.Configuration.Install.Installer
    {
        public ARRServiceInstaller()
        {
            InitializeComponent();
        }
    }
}
