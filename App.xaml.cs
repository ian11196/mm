using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Syncfusion.Licensing;

namespace GSBFLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            SyncfusionLicenseProvider.RegisterLicense("NTQ5OTU3QDMxMzkyZTM0MmUzMEFCdS90VHdwSzlmcFJ6eGFRZGx4ckEva0xPMXdPYkppNzcrOFlCbHpKRkE9");
        }
    }
}
