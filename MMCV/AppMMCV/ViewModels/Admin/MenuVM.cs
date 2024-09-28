using LibraryHelper.Models.HRM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppMMCV.ViewModels.Admin
{
    public class MenuVM : INotifyPropertyChanged
    {
        public MenuVM()
        {
            Command_SubmitEvent = new RelayCommand(SubmitAction);
        }
        public ICommand Command_SubmitEvent { get; }
        public Action SubmitMenu;
        private App_menu menuInfo;
        private string typeSubmit;
        private bool isMenu;
        public string TypeSubmit { get => typeSubmit; set { typeSubmit = value; OnPropertyChanged(nameof(TypeSubmit)); } }
        public App_menu MenuInfo
        {
            get { if (menuInfo == null) menuInfo = new App_menu(); return menuInfo; }
            set { menuInfo = value; OnPropertyChanged(nameof(MenuInfo)); }
        }
        public bool IsMenu { get => isMenu; set => isMenu = value; }
        void SubmitAction()
        {
            SubmitMenu?.Invoke();
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
