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
    public class MenuItemVM : INotifyPropertyChanged
    {
        public MenuItemVM()
        {
            Command_SubmitMenuItem = new RelayCommand(SubmitMenuItem);
        }
        public ICommand Command_SubmitMenuItem { get; }
        public Action SubmitMenuitem { get; set; }
        public bool IsMenuItem { get;set; }
        public string TypeSubmit { get; set; }
        public App_menu_item MenuItemInfo
        {
            get => menuItemInfo;
            set { menuItemInfo = value; OnPropertyChanged(nameof(MenuItemInfo)); }
        }

        private App_menu_item menuItemInfo;

        void SubmitMenuItem()
        {
            SubmitMenuitem?.Invoke();
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
