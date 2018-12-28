using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MFAX01V3.Controls;

namespace MFAX01V3.Views
{
    /// <summary>
    /// Interaction logic for Inbox.xaml
    /// </summary>
    public partial class Inbox : UserControl
    {
        public Inbox()
        {
            InitializeComponent();

        }       

        private void UcTimkiem_BtnTimKiem(object sender, EventArgs e)
        {
            UcTimKiemThu uc = sender as UcTimKiemThu;           
            (this.DataContext as InBoxViewModel).TimKiemThu.Execute(uc);
        }

        private void UcTimkiem_BtnReset(object sender, EventArgs e)
        {
            (this.DataContext as InBoxViewModel).CmdReset.Execute(null);
        }
    }
}
