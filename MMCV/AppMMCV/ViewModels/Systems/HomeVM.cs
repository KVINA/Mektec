using AppMMCV.Json;
using AppMMCV.Services;
using AppMMCV.View.Systems;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AppMMCV.ViewModels.Systems
{
    internal class HomeVM : INotifyPropertyChanged
    {
        
        ObservableCollection<MenuInfo> menuInfos;

        //public ICommand Command_ExecuteActiveMenu;
        public ObservableCollection<MenuInfo> MenuInfos
        {
            get { if (menuInfos == null) menuInfos = new ObservableCollection<MenuInfo>(); return menuInfos; }
            set { menuInfos = value; OnPropertyChanged(nameof(MenuInfos)); }
        }

        public HomeVM()
        {
            Load_Permision();
        }

        void Load_Permision()
        {
            MenuInfos.Clear();
            if (DataService.UserInfo != null)
            {
                string employee_code = DataService.UserInfo.username;
                string query = "Select C.subject_id,subject_name,subject_icon,B.menu_id,B.menu_name,B.menu_description " +
                "from app_roles A " +
                "Inner join app_menu B On B.menu_id = A.menu_id " +
                "Inner join app_subject C On C.subject_id = B.subject_id " +
                $"Where employee_code = '{employee_code}' and access = 1 order by subject_id,menu_index";

                var data = SQLService.Method.ExecuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
                if (string.IsNullOrEmpty(exception))
                {
                    if (data != null && data.Rows.Count > 0)
                    {
                        foreach (DataRow row in data.Rows)
                        {
                            int subject_id = (int)row["subject_id"];
                            string subject_name = row["subject_name"].ToString();
                            string subject_icon = row["subject_icon"].ToString();
                            int menu_id = (int)row["menu_id"];
                            string menu_name = row["menu_name"].ToString();
                            string menu_description = row["menu_description"].ToString();
                            MenuInfo item;
                            if (MenuInfos.Any(m => m.Subject_id == subject_id))
                            {
                                item = MenuInfos.Where(i => i.Subject_id == subject_id).FirstOrDefault();
                            }
                            else
                            {
                                item = new MenuInfo()
                                {
                                    Subject_id = subject_id,
                                    Subject_name = subject_name,
                                    Subject_icon = subject_icon,
                                    ChildsInfo = new ObservableCollection<MenuChildInfo>()
                                };
                                MenuInfos.Add(item);
                            }
                            item.ChildsInfo.Add(new MenuChildInfo()
                            {
                                Menu_id = menu_id,
                                Menu_name = menu_name,
                                Menu_description = menu_description
                            });
                        }
                    }
                }
            }            
        }

		/// <summary>
		/// Close menu is not active
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		

		public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class MenuInfo : INotifyPropertyChanged
    {
        private int subject_id;
        private string subject_name;
        private string subject_icon;
        private bool isOpen;
        private ObservableCollection<MenuChildInfo> childsInfo;
        public int Subject_id { get => subject_id; set { subject_id = value; OnPropertyChanged(nameof(Subject_id)); } }
        public string Subject_name { get => subject_name; set { subject_name = value; OnPropertyChanged(nameof(Subject_name)); } }
        public string Subject_icon { get => subject_icon; set { subject_icon = value; OnPropertyChanged(nameof(subject_icon)); } }
        public ObservableCollection<MenuChildInfo> ChildsInfo { get => childsInfo; set { childsInfo = value; OnPropertyChanged(nameof(ChildsInfo)); } }
		public bool IsOpen { get => isOpen; set { isOpen = value; OnPropertyChanged(nameof(IsOpen)); }  }

		public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class MenuChildInfo : INotifyPropertyChanged
    {
        private int menu_id;
        private string menu_name;
        private int menu_index;
        private string menu_description;

        public int Menu_id { get => menu_id; set { menu_id = value; OnPropertyChanged(nameof(Menu_id)); } }
        public string Menu_name { get => menu_name; set { menu_name = value; OnPropertyChanged(nameof(Menu_name)); } }
        public int Menu_index { get => menu_index; set { menu_index = value; OnPropertyChanged(nameof(Menu_index)); } }
        public string Menu_description { get => menu_description; set { menu_description = value; OnPropertyChanged(nameof(Menu_description)); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
