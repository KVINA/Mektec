using AppMMCV.Services;
using AppMMCV.View.HRM;
using LibraryHelper.Models.HRM;
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

        private ObservableCollection<Employees> dataEmployees;
        private ObservableCollection<string> dataDepartment;
        private ObservableCollection<string> dataSection;
        private string department;
        private string section;
        private bool isLoading;
        public ObservableCollection<Employees> DataEmployees
        {
            get { if (dataEmployees == null) dataEmployees = new ObservableCollection<Employees>(); return dataEmployees; }
            set { dataEmployees = value; OnPropertyChanged(nameof(DataEmployees)); }
        }

        public ObservableCollection<string> DataDepartment
        {
            get { if (dataDepartment == null) dataDepartment = new ObservableCollection<string>(); return dataDepartment; }
            set { dataDepartment = value; OnPropertyChanged(nameof(DataDepartment)); }
        }
        public string Department
        {
            get => department;
            set
            {
                department = value;
                OnPropertyChanged(nameof(Department));
                OnDepartmentChanged();
            }
        }
        public ObservableCollection<string> DataSection
        {
            get { if (dataSection == null) dataSection = new ObservableCollection<string>(); return dataSection; }
            set { dataSection = value; OnPropertyChanged(nameof(DataSection)); }
        }
        public string Section { get => section; set { section = value; OnSectionChanged(); OnPropertyChanged(nameof(Section)); } }
        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }



        async void OnDepartmentChanged()
        {
            IsLoading = true;
            try
            {
                await Task.Run(() =>
                {
                    Get_Section();
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
        async void OnSectionChanged()
        {
            IsLoading = true;
            try
            {
                await Task.Run(() =>
                {
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
                string query = "Select Distinct(department) from employees with(nolock) order by department;";
                var data = SQLService.Method.ExecuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
                if (string.IsNullOrEmpty(exception))
                {
                    if (data != null && data.Rows.Count > 0)
                    {
                        foreach (DataRow row in data.Rows) Application.Current.Dispatcher.Invoke(() => DataDepartment.Add(row["department"].ToString()));
                    }
                }
                else
                {
                    MessageBox.Show(exception);
                }
            });
            
        }

        void Get_Section()
        {
            Application.Current.Dispatcher.Invoke(new Action(() => DataSection.Clear()));
            if (!string.IsNullOrEmpty(Department))
            {
                string query = $"Select Distinct(section) from employees where department = '{Department}' order by section";
                var data = SQLService.Method.ExecuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
                if (string.IsNullOrEmpty(exception))
                {
                    if (data != null && data.Rows.Count > 0)
                    {
                        foreach (DataRow row in data.Rows)
                        {
                            Application.Current.Dispatcher.Invoke(new Action(() => DataSection.Add(row["section"].ToString())));
                            //DataSection.Add(row["section"].ToString());
                        }
                    }
                }
                else
                {
                    MessageBox.Show(exception);
                }
            }
        }

        void Get_DataEmployees()
        {
            Application.Current.Dispatcher.Invoke((Action)(() => DataEmployees.Clear()));
            if (!string.IsNullOrEmpty(Department))
            {
                string query = $"Select * from employees with(nolock) where status = 1 and department = '{Department}'";
                if (!string.IsNullOrEmpty(Section)) query += $" And section = '{section}'";
                query += " order by employee_code";
                var data = SQLService.Method.ExecuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
                if (string.IsNullOrEmpty(exception))
                {
                    if (data != null && data.Rows.Count > 0)
                    {
                        foreach (DataRow row in data.Rows)
                            Application.Current.Dispatcher.Invoke((Action)(() => DataEmployees.Add(new Employees(row))));

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
                    if (DataEmployees.Count > 0)
                    {
                        var option_meal = Option_Meal();
                        var option_category = Option_Category();
                        if (option_category != null && option_meal != null)
                        {
                            option_category.Add("N/A");
                            SaveFileDialog saveFile = new SaveFileDialog();
                            saveFile.Filter = "Excel Files|*.xlsx";
                            saveFile.FileName = "Master register for meal";
                            string savePath = string.Empty;
                            if (saveFile.ShowDialog() == true) savePath = saveFile.FileName;
                            if (!string.IsNullOrEmpty(savePath))
                            {
                                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                                using (ExcelPackage package = new ExcelPackage())
                                {
                                    var workbook = package.Workbook;
                                    var sheet = workbook.Worksheets.Add("Meal Register");
                                    int start_r = 6;
                                    int write_r = start_r;
                                    sheet.Cells[write_r, 2].Value = "STT";
                                    sheet.Cells[write_r, 3].Value = "Mã nhân viên";
                                    sheet.Cells[write_r, 4].Value = "Tên nhân viên";
                                    sheet.Cells[write_r, 5].Value = "Suất ăn";
                                    sheet.Cells[write_r, 2, write_r, 5].Style.Font.Bold = true;
                                    sheet.Cells[write_r, 2, write_r, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    sheet.Cells[write_r, 2, write_r, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    sheet.Cells[write_r, 2, write_r, 5].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.AliceBlue);  // Màu xanh nhạt
                                    int stt = 0;
                                    foreach (var item in DataEmployees)
                                    {
                                        stt++;
                                        write_r++;
                                        sheet.Cells[write_r, 2].Value = stt;
                                        sheet.Cells[write_r, 3].Value = item.Employee_code;
                                        sheet.Cells[write_r, 4].Value = item.Full_name;
                                    }
                                    sheet.Column(1).Width = 4;
                                    sheet.Column(2).Width = 8;
                                    sheet.Column(3).Width = 15;
                                    sheet.Column(4).Width = 40;
                                    sheet.Column(5).Width = 15;
                                    sheet.Cells[2, 2].Value = "MASTER ĐĂNG KÝ SUẤT ĂN";
                                    sheet.Cells[2, 2].Style.Font.Bold = true;
                                    sheet.Cells[3, 2].Value = "Ngày";
                                    sheet.Cells[3, 3].Value = DateTime.Now;
                                    sheet.Cells[3, 3].Style.Numberformat.Format = "yyy/MM/dd";
                                    sheet.Cells[3, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    sheet.Cells[4, 2].Value = "Bữa ăn";
                                    var validation_meal = sheet.DataValidations.AddListValidation(sheet.Cells[4, 3].Address);
                                    foreach (var option in option_meal) validation_meal.Formula.Values.Add(option);
                                    sheet.View.ShowGridLines = false;
                                    var dataRange = sheet.Cells[start_r, 2, write_r, 5];
                                    var validation_category = sheet.DataValidations.AddListValidation(sheet.Cells[start_r + 1, 5, write_r, 5].Address);
                                    foreach (var option in option_category) validation_category.Formula.Values.Add(option);
                                    // Kẻ bảng với màu nâu nhạt cho đường kẻ
                                    dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                    dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                    // Chọn màu cho đường kẻ (màu nâu nhạt)
                                    var lightBrown = ColorTranslator.FromHtml("#A52A2A");  // Màu nâu (brown)
                                    dataRange.Style.Border.Top.Color.SetColor(lightBrown);
                                    dataRange.Style.Border.Bottom.Color.SetColor(lightBrown);
                                    dataRange.Style.Border.Left.Color.SetColor(lightBrown);
                                    dataRange.Style.Border.Right.Color.SetColor(lightBrown);
                                    sheet.Cells.Style.Font.Name = "Palatino Linotype";
                                    package.SaveAs(savePath);
                                }

                            }
                        }
                        else
                        {
                            MessageBox.Show("The category cannot be retrieved, or the category has not been declared.");
                        }

                    }
                    else
                    {
                        MessageBox.Show("No data to download");
                    }
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

        List<string> Option_Category()
        {
            string query = "Select CONCAT(category_id,':',category_name) as category from [meal_category]";
            var data = SQLService.Method.ExecuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
            if (string.IsNullOrEmpty(exception))
            {
                if (data != null && data.Rows.Count > 0)
                {
                    var option_Meal = new List<string>();
                    foreach (DataRow row in data.Rows)
                    {
                        option_Meal.Add(row[0].ToString());
                    }
                    return option_Meal;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                MessageBox.Show(exception);
                return null;
            }
        }
        List<string> Option_Meal()
        {
            string query = "select CONCAT([id],':',[meal]) 'meal' from [meal_time] with(nolock) order by id;";
            var data = SQLService.Method.ExecuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
            if (string.IsNullOrEmpty(exception))
            {
                if (data != null && data.Rows.Count > 0)
                {
                    var option_Meal = new List<string>();
                    foreach (DataRow row in data.Rows)
                    {
                        option_Meal.Add(row[0].ToString());
                    }
                    return option_Meal;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                MessageBox.Show(exception);
                return null;
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
