using AppMMCV.Services;
using AppMMCV.View.Admin;
using LibraryHelper.Models.HRM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AppMMCV.ViewModels.Admin
{
    public class MenuSettingsVM : INotifyPropertyChanged
    {
        public MenuSettingsVM()
        {
            Get_Subject();
        }

        public void Load_Data()
        {
            if (SubjectContext.IsSubject)
            {
                Get_Subject();
                SubjectContext.IsSubject = false;
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
        #region SubjectUC
        private List<string> distinct_SubjectName;
        private SubjectVM subjectContext;
        private string subject_name;
        private ObservableCollection<app_subject> dataSubject;

        public ObservableCollection<app_subject> DataSubject
        {
            get { if (dataSubject == null) dataSubject = new ObservableCollection<app_subject>(); return dataSubject; }
            set { dataSubject = value; OnPropertyChanged(nameof(DataSubject)); }
        }
        public string Subject_name
        {
            get => subject_name;
            set { subject_name = value; OnPropertyChanged(nameof(Subject_name)); }
        }
        internal SubjectVM SubjectContext
        {
            get
            {
                if (subjectContext == null)
                {
                    subjectContext = new SubjectVM();
                    subjectContext.SubmitSubject = Submit_Subject;
                }
                return subjectContext;
            }
            set { subjectContext = value; OnPropertyChanged(nameof(SubjectContext)); }
        }
        void Get_Subject()
        {

            DataSubject.Clear();
            string query = "Select * from app_subject order by subject_id";
            var data = SQLService.Method.ExcuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
            if (string.IsNullOrEmpty(exception))
            {
                foreach (DataRow row in data.Rows) DataSubject.Add(new app_subject(row));
            }

            Distinct_SubjectName = DataSubject.Select(s => s.Subject_name).Distinct().ToList();

        }
        private void Submit_Subject()
        {
            string subject_name = SubjectContext.Subject_name.Trim();
            string subject_description = SubjectContext.Subject_description.Trim();
            if (string.IsNullOrEmpty(subject_name) || string.IsNullOrEmpty(subject_description))
            {
                MessageBox.Show("Please enter value.");
            }
            else
            {
                string query;
                var parameter = new List<object>();
                string username = DataService.User.username;
                parameter.Add(subject_name);
                parameter.Add(subject_description);
                parameter.Add(username);
                switch (SubjectContext.TypeSubmit)
                {
                    case "Add":
                        query = "Insert Into app_subject (subject_name,subject_description,create_by) values ( @name , @description , @username );";
                        break;
                    case "Edit":
                        query = "Update app_subject Set subject_name = @name ,subject_description = @description ,create_by = @username ,create_at = GetDate() Where subject_id = @id ;";

                        parameter.Add(SubjectContext.Subject_id);
                        break;
                    default:
                        query = null;
                        break;
                }
                if (!string.IsNullOrEmpty(query))
                {
                    var res = SQLService.Method.ExcuteNonQuery(out string exception, SQLService.Server.SV68_HRM, query, parameter.ToArray());
                    if (string.IsNullOrEmpty(exception))
                    {
                        if (res > 0)
                        {
                            SubjectContext.IsSubject = true;
                            Load_Data();
                            MaterialDesignThemes.Wpf.DrawerHost.CloseDrawerCommand.Execute(null, null);
                        }
                        else
                        {
                            MessageBox.Show("Fail", "Error");
                        }
                    }
                    else
                    {
                        MessageBox.Show(exception);
                    }
                }
                else
                {
                    MessageBox.Show("Error: submit type invalid.");
                }
            }
        }
        #endregion

        #region Menu

        private MenuVM menuContext;
        public MenuVM MenuContext
        {
            get
            {
                if (menuContext == null) menuContext = new MenuVM();
                menuContext.SubmitMenu = Submit_Menu;
                return menuContext;
            }
            set { menuContext = value; OnPropertyChanged(nameof(MenuContext)); }
        }

        void Submit_Menu()
        {

        }

        #endregion
        #region MenuItem
        private MenuItemVM menuItemContext;
        public MenuItemVM MenuItemContext
        {
            get { if (menuItemContext == null) menuItemContext = new MenuItemVM(); return menuItemContext; }
            set { menuItemContext = value; OnPropertyChanged(nameof(MenuItemContext)); }
        }
        #endregion

        void Get_Menu()
        {

        }

        void Get_MenuItem()
        {

        }



        private bool isOpenRightDrawer;
        private bool isMenu = true;
        private bool isMenuItem = true;
        private string headerActive;
        private UserControl usercontrolActive;


        public UserControl UsercontrolActive { get => usercontrolActive; set { usercontrolActive = value; OnPropertyChanged(nameof(UsercontrolActive)); } }
        public string HeaderActive { get => headerActive; set { headerActive = value; OnPropertyChanged(nameof(HeaderActive)); } }
        public bool IsMenu { get => isMenu; set { isMenu = value; OnPropertyChanged(nameof(IsMenu)); } }
        public bool IsMenuItem { get => isMenuItem; set { isMenuItem = value; OnPropertyChanged(nameof(IsMenuItem)); } }
        public bool IsOpenRightDrawer { get => isOpenRightDrawer; set { isOpenRightDrawer = value; OnPropertyChanged(nameof(IsOpenRightDrawer)); } }


        public List<string> Distinct_SubjectName { get => distinct_SubjectName; set { distinct_SubjectName = value; OnPropertyChanged(nameof(Distinct_SubjectName)); } }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
