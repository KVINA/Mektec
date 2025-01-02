using AppMMCV.Services;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace AppMMCV.ViewModels.HRM
{
    internal class Canteen_MasterRegisterMealsVM : INotifyPropertyChanged
    {
        public Canteen_MasterRegisterMealsVM()
        {
            Get_DataDepartment();
            Command_ExecuteDownloadMaster = new RelayCommand(Download_Master);
        }

        public ICommand Command_ExecuteDownloadMaster { get; }
        public ICommand Command_ExecuteAsyncData { get; }

        private ObservableCollection<CompanyStructure> dataCompanyStructure;
        private CompanyStructure selectedCompanyStructure;
        private ObservableCollection<MasterEmployees> dataEmployees;
        private ObservableCollection<string> dataDepartment;
        private ObservableCollection<string> dataSection;
        private string section;
        private bool isLoading;
        public ObservableCollection<MasterEmployees> DataEmployees
        {
            get { if (dataEmployees == null) dataEmployees = new ObservableCollection<MasterEmployees>(); return dataEmployees; }
            set { dataEmployees = value; OnPropertyChanged(nameof(DataEmployees)); }
        }
        public ObservableCollection<CompanyStructure> DataCompanyStructure
        {
            get { if (dataCompanyStructure == null) dataCompanyStructure = new ObservableCollection<CompanyStructure>(); return dataCompanyStructure; }
            set => dataCompanyStructure = value;
        }
        public ObservableCollection<string> DataDepartment
        {
            get { if (dataDepartment == null) dataDepartment = new ObservableCollection<string>(); return dataDepartment; }
            set { dataDepartment = value; OnPropertyChanged(nameof(DataDepartment)); }
        }
        
        public ObservableCollection<string> DataSection
        {
            get { if (dataSection == null) dataSection = new ObservableCollection<string>(); return dataSection; }
            set { dataSection = value; OnPropertyChanged(nameof(DataSection)); }
        }
        
        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }

        public CompanyStructure SelectedCompanyStructure
        {
            get => selectedCompanyStructure;
            set { selectedCompanyStructure = value;
                OnDepartmentChanged();
                OnPropertyChanged(nameof(SelectedCompanyStructure)); 
            }
        }
        public async void OnDepartmentChanged()
        {
            IsLoading = true;
            try
            {                
                await Task.Run(() =>
                {                    
                    //Get_Section();
                    Get_DataEmployees();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }
      
        async void Get_DataDepartment()
        {
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() => DataDepartment.Clear());
                string query = "select * FROM [tbMD_CompanyStructure] with(NoLock) order by ID;";
                var data = SQLService.Method.ExecuteQuery(out string exception, SQLService.Server.SV91_EZ_MEKTEC, query);
                if (string.IsNullOrEmpty(exception))
                {
                    if (data != null && data.Rows.Count > 0)
                    {
                        // Tạo tất cả các phòng từ DataTable
                        Dictionary<int, CompanyStructure> departmentDict = new Dictionary<int, CompanyStructure>();
                        foreach (DataRow row in data.Rows)
                        {
                            var department = new CompanyStructure()
                            {
                                ID = Convert.ToInt32(row["ID"]),
                                Code = row["Code"].ToString(),
                                Name = row["Name"].ToString(),
                                ParentID = row["ParentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["ParentID"])
                            };
                            departmentDict[department.ID] = department;
                        }
                        // Thiết lập quan hệ cha - con
                        foreach (var department in departmentDict.Values)
                        {
                            if (department.ParentID.HasValue && departmentDict.ContainsKey(department.ParentID.Value))
                            {
                                departmentDict[department.ParentID.Value].Children.Add(department);
                            }
                            else
                            {
                                Application.Current.Dispatcher.Invoke(() => DataCompanyStructure.Add(department));
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(exception);
                }
            });

        }        
        void Get_DataEmployees()
        {
            Application.Current.Dispatcher.Invoke((Action)(() => DataEmployees.Clear()));
            if (SelectedCompanyStructure != null)
            {
                string query = $"usp_mmcv_employee_by_department {SelectedCompanyStructure.ID}";               
                var data = SQLService.Method.ExecuteQuery(out string exception, SQLService.Server.SV91_EZ_MEKTEC, query);
                if (string.IsNullOrEmpty(exception))
                {

                    if (data != null && data.Rows.Count > 0)
                    {
                        foreach (DataRow row in data.Rows)
                            Application.Current.Dispatcher.Invoke((Action)(() => DataEmployees.Add(new MasterEmployees(row))));

                    }
                }
                else
                {
                    MessageBox.Show(exception);
                }
            }
        }       

        async void Download_Master()
        {
            try
            {
                IsLoading = true;
                await Task.Run(() =>
                {
                    var _master = new MasterRegisterMeals();
                    _master.ExportMasterRegisterMeal(DataEmployees);
                });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                IsLoading = false;
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
    public class CompanyStructure
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? ParentID { get; set; }
        public List<CompanyStructure> Children { get; set; } = new List<CompanyStructure>();
    }
}
