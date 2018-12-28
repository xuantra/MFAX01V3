
using FAXCOMEXLib;
using FAXCOMLib;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace MFAX01V3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static FAXCOMEXLib.FaxServer objFaxServer = new FAXCOMEXLib.FaxServer();
        public static FaxConfiguration objFaxConfig;
        public  static string folderLuufax;
        public static FaxAccount objFaxAccount;

        public App()
        {
        
            objFaxServer.Connect("");
            objFaxConfig = objFaxServer.Configuration;
            folderLuufax = objFaxConfig.ArchiveLocation;
            objFaxAccount = objFaxServer.CurrentAccount;
        }
      

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
