using AppMMCV.Services;
using AppMMCV.View.Admin;
using LibraryHelper.Models.HRM;
using System;
using System.Collections;
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
using System.Windows.Input;

namespace AppMMCV.ViewModels.Admin
{
    public class MenuSettingsVM : INotifyPropertyChanged
    {
        public MenuSettingsVM()
        {
            Get_Subject();
            Get_Menu();
            Get_MenuItem();
        }

        public void Load_Data()
        {
            if (SubjectContext.IsSubject)
            {
                Get_Subject();
                SubjectContext.IsSubject = false;
            }
            if (MenuContext.IsMenu)
            {
                Get_Menu();
                MenuContext.IsMenu = false;
            }
            if (MenuItemContext.IsMenuItem)
            {
                MenuItemContext.IsMenuItem = false;
                Get_MenuItem();
            }
        }

        #region SubjectUC
        private List<string> distinct_SubjectName;
        private SubjectVM subjectContext;
        private string subject_name;
        private ObservableCollection<App_subject> dataSubject;

        public ObservableCollection<App_subject> DataSubject
        {
            get { if (dataSubject == null) dataSubject = new ObservableCollection<App_subject>(); return dataSubject; }
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
                    var _subject = new SubjectVM() { SubmitSubject = Submit_Subject };
                    subjectContext = _subject;
                }
                return subjectContext;
            }
            set { subjectContext = value; OnPropertyChanged(nameof(SubjectContext)); }
        }
        void Get_Subject()
        {
            DataSubject.Clear();
            string query = "Select * from app_subject order by subject_id";
            if (!string.IsNullOrEmpty(subject_name)) query = $"Select * from app_subject Where [subject_name] = N'{subject_name}' order by subject_id";
            var data = SQLService.Method.ExcuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
            if (string.IsNullOrEmpty(exception)) foreach (DataRow row in data.Rows) DataSubject.Add(new App_subject(row));
            Distinct_SubjectName = DataSubject.Select(s => s.Subject_name).Distinct().ToList();
        }
        private void Submit_Subject()
        {
            string subject_name = SubjectContext.Subject_name.Trim();
            string subject_icon = SubjectContext.Subject_icon.Trim();
            if (string.IsNullOrEmpty(subject_name) || string.IsNullOrEmpty(subject_icon)) MessageBox.Show("Please enter value.");
            else
            {
                string query;
                var parameter = new List<object>();
                string username = DataService.UserInfo.username;
                parameter.Add(subject_name);
                parameter.Add(subject_icon);
                parameter.Add(username);

                switch (SubjectContext.TypeSubmit)
                {
                    case "Add":
                        query = "Insert Into app_subject (subject_name,subject_icon,create_by) values ( @name , @description , @username );";
                        break;
                    case "Edit":
                        query = "Update app_subject Set subject_name = @name ,subject_icon = @description ,create_by = @username ,create_at = GetDate() Where subject_id = @id ;";
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
                        else MessageBox.Show("Fail", "Error");
                    }
                    else MessageBox.Show(exception);
                }
                else MessageBox.Show("Error: submit type invalid.");
            }
        }
        #endregion

        #region Menu

        private ObservableCollection<App_menu> dataMenu;
        public ObservableCollection<App_menu> DataMenu
        {
            get { if (dataMenu == null) dataMenu = new ObservableCollection<App_menu>(); return dataMenu; }
            set { dataMenu = value; OnPropertyChanged(nameof(DataMenu)); }
        }

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
        void Get_Menu()
        {
            DataMenu.Clear();
            string query = "Select * from [app_menu] order by [subject_id],[menu_index]";
            if (SelectedSubject != null) query = $"Select * from [app_menu] where [subject_id] = {SelectedSubject.Subject_id} order by [subject_id],[menu_index]";
            var data = SQLService.Method.ExcuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
            if (string.IsNullOrEmpty(exception)) foreach (DataRow row in data.Rows) DataMenu.Add(new App_menu(row));
        }
        void Submit_Menu()
        {
            string menu_name = MenuContext.MenuInfo.Menu_name;
            string menu_description = MenuContext.MenuInfo.Menu_description;
            int menu_index = MenuContext.MenuInfo.Menu_index;
            int subject_id = MenuContext.MenuInfo.Subject_id;

            if (subject_id > 0)
            {
                if (string.IsNullOrEmpty(menu_name) || string.IsNullOrEmpty(menu_description))
                {
                    MessageBox.Show("Please enter value.", "WARNING");
                }
                else
                {
                    string query = string.Empty;
                    var parameter = new List<object>() { menu_name, menu_description, menu_index, subject_id, DataService.UserInfo.username };
                    switch (MenuContext.TypeSubmit)
                    {
                        case "Add":
                            query = "Insert Into [app_menu]([menu_name],[menu_description],[menu_index],[subject_id],[create_at],[create_by]) " +
                                "Values ( @menu_name , @menu_description , @menu_index , @subject_id , GetDate() , @create_by );";
                            break;
                        case "Edit":
                            query = "Update [app_menu] Set [menu_name] = @menu_name ,[menu_description] = @menu_description ,[menu_index] = @menu_index ," +
                                "[subject_id] = @subject_id , [create_at] = GetDate(),[create_by] = @create_by Where [menu_id] = @menu_id ";
                            parameter.Add(MenuContext.MenuInfo.Menu_id);
                            break;
                        default:
                            break;
                    }

                    if (!string.IsNullOrEmpty(query))
                    {
                        var res = SQLService.Method.ExcuteNonQuery(out string exception, SQLService.Server.SV68_HRM, query, parameter.ToArray());
                        if (string.IsNullOrEmpty(exception))
                        {
                            if (res > 0)
                            {
                                MenuContext.IsMenu = true;
                                Load_Data();
                                MaterialDesignThemes.Wpf.DrawerHost.CloseDrawerCommand.Execute(null, null);
                            }
                            else MessageBox.Show("Fail", "Error");
                        }
                        else MessageBox.Show(exception);
                    }
                    else MessageBox.Show("Error: submit type invalid.");
                }
            }
        }

        #endregion

        #region MenuItem
        private MenuItemVM menuItemContext;
        private ObservableCollection<App_menu_item> dataMenuItem;

        public ObservableCollection<App_menu_item> DataMenuItem
        {
            get { if (dataMenuItem == null) { dataMenuItem = new ObservableCollection<App_menu_item>(); } return dataMenuItem; }
            set { dataMenuItem = value; OnPropertyChanged(nameof(DataMenuItem)); }
        }
        public MenuItemVM MenuItemContext
        {
            get
            {
                if (menuItemContext == null)
                {
                    var _menu_item = new MenuItemVM() { SubmitMenuitem = Submit_MenuItem };
                    menuItemContext = _menu_item;
                }
                return menuItemContext;
            }
            set { menuItemContext = value; OnPropertyChanged(nameof(MenuItemContext)); }
        }

        void Get_MenuItem()
        {
            DataMenuItem.Clear();
            string query = "Select * from [app_menu_item] order by [menu_id]";
            if (SelectedMenu != null) query = $"Select * from [app_menu_item] Where [menu_id] = {SelectedMenu.Menu_id} order by [menu_id]";
            var data = SQLService.Method.ExcuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
            if (string.IsNullOrEmpty(exception)) foreach (DataRow row in data.Rows) DataMenuItem.Add(new App_menu_item(row));
        }

        void Submit_MenuItem()
        {
            string item_group = MenuItemContext.MenuItemInfo.Item_group;
            string item_name = MenuItemContext.MenuItemInfo.Item_name;
            int item_index = MenuItemContext.MenuItemInfo.Item_index;
            string item_controller = MenuItemContext.MenuItemInfo.Item_controller;
            string item_icon = MenuItemContext.MenuItemInfo.Item_icon;
            string item_header = MenuItemContext.MenuItemInfo.Item_header;
            int menu_id = MenuItemContext.MenuItemInfo.Menu_id;

            if (menu_id > 0)
            {
                if (string.IsNullOrEmpty(item_group) || string.IsNullOrEmpty(item_name) || string.IsNullOrEmpty(item_controller)
                    || string.IsNullOrEmpty(item_icon) || string.IsNullOrEmpty(item_header)) MessageBox.Show("Please enter value.", "WARNING");
                else
                {
                    string query = string.Empty;
                    var parameter = new List<object>() { item_group.Trim(), item_name.Trim(), item_index , item_controller.Trim(),
                        item_icon.Trim(), item_header.Trim(), menu_id ,DataService.UserInfo.username};

                    switch (MenuItemContext.TypeSubmit)
                    {
                        case "Add":
                            query = "Insert Into [app_menu_item] ([item_group],[item_name],[item_index],[item_controller],[item_icon],[item_header],[menu_id],[create_at],[create_by]) " +
                                "Values ( @item_group , @item_name , @item_index , @item_controller , @item_icon , @item_header , @menu_id , GetDate() , @create_by );";

                            break;
                        case "Edit":
                            query = "Update [app_menu_item] set [item_group] = @item_group ,[item_name] = @item_name ,[item_index] = @item_index ,[item_controller] = @item_controller ," +
                                "[item_icon] = @item_icon ,[item_header] = @item_header ,[menu_id] = @menu_id ,[create_at] = GetDate() ,[create_by] = @create_by Where [item_id] = @item_id ;";
                            parameter.Add(MenuItemContext.MenuItemInfo.Item_id);
                            break;
                        default:
                            break;
                    }

                    if (!string.IsNullOrEmpty(query))
                    {
                        var res = SQLService.Method.ExcuteNonQuery(out string exception, SQLService.Server.SV68_HRM, query, parameter.ToArray());
                        if (string.IsNullOrEmpty(exception))
                        {
                            if (res > 0)
                            {
                                MenuItemContext.IsMenuItem = true;
                                Load_Data();
                                MaterialDesignThemes.Wpf.DrawerHost.CloseDrawerCommand.Execute(null, null);
                            }
                            else MessageBox.Show("Fail", "Error");
                        }
                        else MessageBox.Show(exception);
                    }
                    else MessageBox.Show("Error: submit type invalid.");
                }
            }
        }
        #endregion
        //Private
        private App_subject selectedSubject;
        private App_menu selectedMenu;
        private bool isOpenRightDrawer;
        private string headerActive;
        private UserControl usercontrolActive;

        //Public
        public UserControl UsercontrolActive { get => usercontrolActive; set { usercontrolActive = value; OnPropertyChanged(nameof(UsercontrolActive)); } }
        public string HeaderActive { get => headerActive; set { headerActive = value; OnPropertyChanged(nameof(HeaderActive)); } }
        public bool IsOpenRightDrawer { get => isOpenRightDrawer; set { isOpenRightDrawer = value; OnPropertyChanged(nameof(IsOpenRightDrawer)); } }
        public List<string> Distinct_SubjectName { get => distinct_SubjectName; set { distinct_SubjectName = value; OnPropertyChanged(nameof(Distinct_SubjectName)); } }
        public App_subject SelectedSubject
        {
            get => selectedSubject;
            set
            {
                selectedSubject = value;
                Get_Menu();
            }
        }
        public App_menu SelectedMenu { get => selectedMenu; set { selectedMenu = value; Get_MenuItem(); } }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
