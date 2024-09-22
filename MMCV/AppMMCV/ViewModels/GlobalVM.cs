using LibraryHelper.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMMCV.ViewModels
{
    internal class GlobalVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //private Users user;
        //private bool isLogin;

        //public Users User { get => user; set { user = value; OnPropertyChanged(nameof(User)); } }
        //public bool IsLogin { get => isLogin; set { isLogin = value; OnPropertyChanged(nameof(IsLogin));} }
    }
}
