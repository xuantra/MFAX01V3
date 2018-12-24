using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace MFAX01V3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public bool IsApplicationAlreadyRunning()
        {
            return Process.GetProcesses().Count(p => p.ProcessName.Contains(Assembly.GetExecutingAssembly().FullName.Split(',')[0]) && !p.Modules[0].FileName.Contains("vshost")) > 1;
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            if (IsApplicationAlreadyRunning())
            {
                MessageBox.Show("Chương trình đang chạy!");
                Environment.Exit(Environment.ExitCode);
                return;
            }
            base.OnStartup(e);
        }
    }
}
