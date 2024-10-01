using AppMMCV.Services;
using LibraryHelper.Models.HRM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AppMMCV.ViewModels.Admin
{
  
    public class AppRolesVM : INotifyPropertyChanged
    {        
        public AppRolesVM()
        {
            Load_Subject();
            Commad_LoadFunctionAdd = new RelayCommand(LoadFunctionAdd);
            Commad_LoadFunctionEdit = new RelayCommand(LoadFunctionEdit);
            Commad_ExecuteSubmit = new RelayCommand(ExecuteSubmit);
            Commad_ExecuteSearch = new RelayCommand(Load_Roles);
        }
        public ICommand Commad_ExecuteSearch { get; }
        public ICommand Commad_LoadFunctionAdd { get; }
        public ICommand Commad_LoadFunctionEdit { get; }
        public ICommand Commad_ExecuteSubmit { get; }

        string employee_search;
        string typeSubmit;
        App_menu selectedMenu;
        App_roles selectedRoles;
        App_roles formData;
        ObservableCollection<App_subject> dataSubject;
        ObservableCollection<App_roles> dataRoles;
        ObservableCollection<App_menu> dataMenu;
        ObservableCollection<List_item_id> dataMenuItem;
        App_subject selectedSubject;
        bool isOpenDialog;
        public App_subject SelectedSubject
        {
            get => selectedSubject;
            set { selectedSubject = value; Load_Menu(); OnPropertyChanged(nameof(SelectedSubject)); }
        }      
        
        public bool IsOpenDialog { get => isOpenDialog; set { isOpenDialog = value; OnPropertyChanged(nameof(IsOpenDialog)); } }
        public App_roles SelectedRoles { get => selectedRoles; set { selectedRoles = value; OnPropertyChanged(nameof(SelectedRoles)); } }

        public App_roles FormData { get => formData; set { formData = value; OnPropertyChanged(nameof(FormData)); } }
        public ObservableCollection<App_subject> DataSubject
        {
            get { if (dataSubject == null) dataSubject = new ObservableCollection<App_subject>(); return dataSubject; }
            set => dataSubject = value;
        }
        public ObservableCollection<App_roles> DataRoles
        {
            get { if (dataRoles == null) dataRoles = new ObservableCollection<App_roles>(); return dataRoles; }
            set => dataRoles = value;
        }
        public ObservableCollection<App_menu> DataMenu
        {
            get { if (dataMenu == null) dataMenu = new ObservableCollection<App_menu>(); return dataMenu; }
            set => dataMenu = value;
        }
        public ObservableCollection<List_item_id> DataMenuItem
        {
            get { if (dataMenuItem == null) dataMenuItem = new ObservableCollection<List_item_id>(); return dataMenuItem; }
            set { dataMenuItem = value; OnPropertyChanged(nameof(DataMenuItem)); }
        }
        public App_menu SelectedMenu
        {
            get => selectedMenu;
            set { selectedMenu = value; Load_Roles(); Load_MenuItem(); OnPropertyChanged(nameof(SelectedMenu)); }
        }

        public string TypeSubmit { get => typeSubmit; set { typeSubmit = value; OnPropertyChanged(nameof(TypeSubmit)); } }

        public string Employee_search { get => employee_search; set { employee_search = value; OnPropertyChanged(nameof(Employee_search)); } }

        void LoadFunctionAdd()
        {
            if (SelectedMenu != null && DataMenuItem.Count > 0)
            {
                var app_roles = new App_roles();
                app_roles.Menu_id = SelectedMenu.Menu_id;
                foreach (var item in DataMenuItem) item.IsChecked = false;
                TypeSubmit = "Add permission";
                FormData = app_roles;
                IsOpenDialog = true;
            }
            else
            {
                MessageBox.Show("Please select subject before.", "Informartion", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        void LoadFunctionEdit()
        {
            if (SelectedMenu != null && SelectedRoles != null)
            {    
                if (string.IsNullOrEmpty(SelectedRoles.List_item_id))
                {
                    foreach (var item in DataMenuItem) item.IsChecked = false;
                }
                else
                {
                    var list_id = SelectedRoles.List_item_id.Split(',').ToList();
                    foreach (var item in DataMenuItem)
                    {
                        if(list_id.Contains(item.Item_id.ToString())) item.IsChecked = true;
                        else item.IsChecked = false;
                    }
                }
                IsOpenDialog = true;
                TypeSubmit = "Edit permission";
                FormData = SelectedRoles;
            }
            else
            {
                MessageBox.Show("Please select subject before.", "Informartion", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
        }
        void ExecuteSubmit()
        {
            if (DataMenuItem != null && FormData != null && IsOpenDialog)
            {
                //Kiểm tra value input
                if (string.IsNullOrEmpty(FormData.Employee_code) || FormData.Menu_id == 0)
                {
                    MessageBox.Show("Please enter value.");
                    return;
                }

                string query = "";
                FormData.List_item_id = string.Join(",",DataMenuItem.Where(i => i.IsChecked == true).Select(i=>i.Item_id).ToList());
                string username = DataService.UserInfo.username;
                var parameter = new List<object>() 
                {
                    FormData.Employee_code,
                    FormData.Menu_id,
                    FormData.Access,
                    FormData.List_item_id,
                    username
                };
                switch (TypeSubmit)
                {
                    case "Add permission":
                        if (CheckPermission())
                        {
                            query = "Insert Into app_roles ([employee_code],[menu_id],[access],[list_item_id],[create_at],[create_by]) " +
                            "values ( @employee_code , @menu_id , @access , @list_item_id , GetDate() , @create_by );";
                            break;
                        }
                        else
                        {
                            MessageBox.Show("Unable to add this employee, please check again.");
                            return;
                        }
                        
                    case "Edit permission":
                        query = "Update app_roles Set [employee_code] = @employee_code ,[menu_id] = @menu_id ,[access] = @access ,"+
                            "[list_item_id] = @list_item_id ,[create_at] = GetDate() ,[create_by] = @create_by Where role_id  = @role_id ";
                        parameter.Add(FormData.Role_id);
                        break;
                    default:
                        return;
                }
                var res = SQLService.Method.ExcuteNonQuery(out string exception, SQLService.Server.SV68_HRM, query, parameter.ToArray());
                if (string.IsNullOrEmpty(exception))
                {
                    if (res > 0)
                    {
                        Load_Roles();
                        MessageBox.Show("Conpleted.");
                        IsOpenDialog = false;
                    }
                    else
                    {
                        MessageBox.Show("Fail.");
                    }
                }
                else
                {
                    MessageBox.Show(exception);
                }
            }
            
        }
        /// <summary>
        /// Check employee code with menu id already exist
        /// </summary>
        /// <returns></returns>
        bool CheckPermission()
        {
            string query = $"Select Count(*) from [app_roles] where [employee_code] = '{FormData.Employee_code}' and [menu_id] = {FormData.Menu_id};";
            var res = SQLService.Method.ExcuteScalar(out string exception,SQLService.Server.SV68_HRM,query);
            if (string.IsNullOrEmpty(exception))
            {
                return (int)res == 0;
            }
            else
            {
                MessageBox.Show(exception);
                return false;
            }
        }

        void Load_MenuItem()
        {
            DataMenuItem.Clear();
            if (SelectedMenu != null)
            {
                string query = $"Select * from app_menu_item where menu_id = {SelectedMenu.Menu_id} order by item_index;";
                var data = SQLService.Method.ExcuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
                if (string.IsNullOrEmpty(exception))
                {
                    if (data != null && data.Rows.Count > 0) foreach (DataRow row in data.Rows) DataMenuItem.Add(new List_item_id(row));
                }
                else MessageBox.Show(exception, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }            
        }
        void Load_Menu()
        {
            DataMenu.Clear();
            if (SelectedSubject != null)
            {
                string query = $"Select * from app_menu Where subject_id = {SelectedSubject.Subject_id} order by menu_index";
                var data = SQLService.Method.ExcuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
                if (string.IsNullOrEmpty(exception))
                {
                    if (data != null && data.Rows.Count > 0) foreach (DataRow row in data.Rows) DataMenu.Add(new App_menu(row));                    
                }
                else MessageBox.Show(exception, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        void Load_Roles()
        {
            DataRoles.Clear();
            if (SelectedMenu != null)
            {
                string query = $"Select * from app_roles Where menu_id = {SelectedMenu.Menu_id}";
                if (!string.IsNullOrEmpty(Employee_search)) { query += $" and employee_code = '{Employee_search}'"; }
                var data = SQLService.Method.ExcuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
                if (string.IsNullOrEmpty(exception))
                {
                    if (data != null && data.Rows.Count > 0) foreach (DataRow row in data.Rows) DataRoles.Add(new App_roles(row));                    
                }
                else MessageBox.Show(exception, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        void Load_Subject()
        {
            DataSubject.Clear();
            string query = "Select * from app_subject order by subject_id";
            var data = SQLService.Method.ExcuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
            if (string.IsNullOrEmpty(exception))
            {
                if (data != null && data.Rows.Count > 0) foreach (DataRow row in data.Rows) DataSubject.Add(new App_subject(row));
            }
            else MessageBox.Show(exception, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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
