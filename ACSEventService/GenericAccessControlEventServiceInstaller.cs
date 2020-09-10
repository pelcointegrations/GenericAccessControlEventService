using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace GenericAccessControlEventService
{
    [RunInstaller(true)]
    public partial class GenericAccessControlEventServiceInstaller : Installer
    {
        public GenericAccessControlEventServiceInstaller()
        {
            InitializeComponent();
        }
    }
}
