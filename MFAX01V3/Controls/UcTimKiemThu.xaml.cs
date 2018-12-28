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

namespace MFAX01V3.Controls
{
    /// <summary>
    /// Interaction logic for UcTimKiemThu.xaml
    /// </summary>
    /// 
   
    public partial class UcTimKiemThu : UserControl
    {
       public event EventHandler BtnTimKiem;
       private DateTime _ngayBatDau;
        private DateTime _ngayKetThuc;
        public event EventHandler BtnReset;

        public DateTime NgayKetThuc { get => dtpNgayKetThuc.SelectedDate?? DateTime.Now; set => _ngayKetThuc = value; }
        public DateTime NgayBatDau { get => dtpNgayBatDau.SelectedDate ?? DateTime.Now; set => _ngayBatDau = value; }

        public UcTimKiemThu()
        {
            InitializeComponent();
            dtpNgayBatDau.SelectedDate = new DateTime(DateTime.Now.Year, 1, 1);
            dtpNgayKetThuc.SelectedDate = DateTime.Now;
        }

        private void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            BtnTimKiem?.Invoke(this,e);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            BtnReset?.Invoke(this, e);
            dtpNgayBatDau.SelectedDate = new DateTime(DateTime.Now.Year, 1, 1);
            dtpNgayKetThuc.SelectedDate = DateTime.Now;
        }
    }
}
