using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PropertyChanged;


namespace MFAX01V3
{
    public class LoginViewmodel : BaseViewModel
    {
        private string _UserName;
        public string User { get => _UserName; set { _UserName = value; OnPropertyChanged(); } }
        public string PassWord { get => _PassWord; set{ _PassWord = value; OnPropertyChanged(); } }
        private string _PassWord;
        public bool IsLogin { get; set; }
        public bool IsNotLoginRunning { get; set; }

        

        public ICommand LoginComand { get; set; }
       



        private void LoginAsync(Window p)
        {
            if (p == null) return;
            if (User == "admin" )
            {
                IsLogin = true;
                new MainWindow().Show();
                p.Close();               
            }

        }

        public LoginViewmodel()
        {
            IsLogin = false;
            IsNotLoginRunning = true;
            LoginComand = new RelayCommand<Window>((p) => { return IsNotLoginRunning; }, (p) => LoginAsync(p));
        }
    }
}
