using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace MFAX01V3
{
    /// <summary>
    /// Interaction logic for wndSendFax.xaml
    /// </summary>
    public partial class wndSendFax : Window
    {
        ObservableCollection<MyData> m_SelectedData = new ObservableCollection<MyData>();
        public wndSendFax()
        {
            InitializeComponent();
            m_multiselect.AvailableList = LoadData();
            m_SelectedData.Add(new MyData("gipermail@gmail.com"));
            m_multiselect.SelectedList = m_SelectedData;

            m_multiselect.m_createObjectDelegate += new MultiSelectTextBox.CreateObjectDelegate(m_multiselect_m_createObjectDelegate);

            this.DataContext = this;
            m_multiselect.Refresh();
        }

        private object m_multiselect_m_createObjectDelegate(string DisplayName)
        {
            return new MyData(DisplayName);
        }

        private ObservableCollection<MyData> LoadData()
        {
            ObservableCollection<MyData> data = new ObservableCollection<MyData>();

            Helpers.AddSorted(data, new MyData("red@mail.ru"));
            Helpers.AddSorted(data, new MyData("yellow@mail.ru"));
            Helpers.AddSorted(data, new MyData("alexicon@yandex.ru"));
            Helpers.AddSorted(data, new MyData("gipermail@gmail.com"));
            Helpers.AddSorted(data, new MyData("google@gmail.com"));
            return data;
        }
    }
}
