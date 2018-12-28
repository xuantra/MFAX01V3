using MFAX01V3.Controls;
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

namespace MFAX01V3.Views
{
    /// <summary>
    /// Interaction logic for Outbox.xaml
    /// </summary>
    public partial class Outbox : UserControl
    {
        public Outbox()
        {
            InitializeComponent();
        }

        private void UcTimKiem_BtnReset(object sender, EventArgs e)
        {
            UcTimKiemThu uc = sender as UcTimKiemThu;
            (this.DataContext as OutboxViewmodel).TimKiemThu.Execute(uc);
        }

        private void UcTimKiem_BtnTimKiem(object sender, EventArgs e)
        {
            (this.DataContext as OutboxViewmodel).CmdReset.Execute(null);
        }
    }
}
