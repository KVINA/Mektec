using LibraryHelper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AppMMCV.ViewModels.Admin
{
    public class MenuSettingsVM : INotifyPropertyChanged
    {
        public MenuSettingsVM()
        {
            Load_Data();
        }

        public void Load_Data()
        {
            if (IsSubject)
            {
                Get_Subject();
                IsSubject = false;
            }
            if (IsMenu)
            {
                Get_Menu();
                IsMenu = false;
            }
            if (IsMenuItem)
            {
                IsMenuItem = false;
                Get_MenuItem();
            }
        }
        void Get_Subject()
        {
            App_Subjects.Clear();
            string query = "Select * from app_subject order by subject_id";
            var data = Services.SQLService.Method.ExcuteQuery(out string exception, Services.SQLService.Server.SV68_HRM, query);
            if (string.IsNullOrEmpty(exception))
            {
                foreach (DataRow row in data.Rows) App_Subjects.Add(new app_subject(row));
            }
        }

        void Get_Menu()
        {

        }

        void Get_MenuItem()
        {

        }

        private bool isOpenRightDrawer;
        private bool isSubject = true;
        private bool isMenu = true;
        private bool isMenuItem = true;
        private string headerActive;
        private UserControl usercontrolActive;
        private string subject_name;
        private ObservableCollection<app_subject> app_Subjects;
        public ObservableCollection<app_subject> App_Subjects { get { if (app_Subjects == null) app_Subjects = new ObservableCollection<app_subject>(); return app_Subjects; } set { app_Subjects = value; OnPropertyChanged(nameof(App_Subjects)); } }
        public string Subject_name { get => subject_name; set { subject_name = value; OnPropertyChanged(nameof(Subject_name)); } }
        public UserControl UsercontrolActive { get => usercontrolActive; set { usercontrolActive = value; OnPropertyChanged(nameof(UsercontrolActive)); } }
        public string HeaderActive { get => headerActive; set { headerActive = value; OnPropertyChanged(nameof(HeaderActive)); } }
        public bool IsSubject { get => isSubject; set { isSubject = value; OnPropertyChanged(nameof(IsSubject)); } }
        public bool IsMenu { get => isMenu; set { isMenu = value; OnPropertyChanged(nameof(IsMenu)); } }
        public bool IsMenuItem { get => isMenuItem; set { isMenuItem = value; OnPropertyChanged(nameof(IsMenuItem)); } }
        public bool IsOpenRightDrawer { get => isOpenRightDrawer; set { isOpenRightDrawer = value; OnPropertyChanged(nameof(IsOpenRightDrawer)); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
