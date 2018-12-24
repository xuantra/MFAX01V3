using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MFAX01V3
{
    public class MainWindowViewModel:BaseViewModel
    {
        private ListViewItem _SelectedMenu;
        public BaseViewModel SelectedViewmodel { get; set; }
        public bool ModemStatus { get; set; } = true;

        public ListViewItem SelectedMenu { get => _SelectedMenu;
            set {
                _SelectedMenu = value;
                SwitchViewmodel(_SelectedMenu);
            }
        }

        private void SwitchViewmodel(ListViewItem selectedMenu)
        {
            switch (selectedMenu.Name)
            {
                case "itemInbox":
                    SelectedViewmodel = new InBoxViewModel();
                    break;
                case "itemOutbox":
                    SelectedViewmodel = new OutboxViewmodel();
                    break;
                default:
                    break;
            }
           
        }

        public MainWindowViewModel()
        {
            SelectedViewmodel = new InBoxViewModel();
        }
    }
}
