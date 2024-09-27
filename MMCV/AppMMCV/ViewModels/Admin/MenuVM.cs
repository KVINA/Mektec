using LibraryHelper.Models.HRM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMMCV.ViewModels.Admin
{
    public class MenuVM : INotifyPropertyChanged
    {
        public Action SubmitMenu;
        private App_menu menuInfo;
        private string typeSubmit;
        public string TypeSubmit { get => typeSubmit; set { typeSubmit = value; OnPropertyChanged(nameof(TypeSubmit)); } }

        public App_menu MenuInfo
        {
            get { if (menuInfo == null) menuInfo = new App_menu(); return menuInfo; }
            set { menuInfo = value; OnPropertyChanged(nameof(MenuInfo)); }
        }




        /// <summary>
        /// Khởi tạo INotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
