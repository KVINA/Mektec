using AppMMCV.Services;
using LibraryHelper.Models.HRM;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Table.PivotTable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Xml;

namespace AppMMCV.ViewModels.HRM
{
    public class Canteen_RegisterDailyMealVM : INotifyPropertyChanged
    {
        public ICommand Command_ExecuteRegisterMeals { get; }
        public ICommand Command_ExecuteGetAttachmentPath { get; }
        public ICommand Command_ExecuteClearAttachmentPath { get; }
        public Canteen_RegisterDailyMealVM()
        {
            Get_MealTime();
            Command_ExecuteGetAttachmentPath = new RelayCommand(Get_AttachmentPath);
            Command_ExecuteClearAttachmentPath = new RelayCommand(Clear_AttachmentPath);
            Command_ExecuteRegisterMeals = new RelayCommand(RegisterMeals);
        }
        string registration_by = DataService.UserInfo.username;
        bool isLoading;
        ObservableCollection<RegisterDailyMeails> dataRegisterDaily;
        ObservableCollection<Meal_time> dataMealTime;
        Meal_time selectedMealTime;
        string attachmentPath;
        DateTime dateRegister = DateTime.Now;

        public ObservableCollection<Meal_time> DataMealTime
        {
            get { if (dataMealTime == null) dataMealTime = new ObservableCollection<Meal_time>(); return dataMealTime; }
            set { dataMealTime = value; OnPropertyChanged(nameof(DataMealTime)); }
        }

        public string AttachmentPath { get => attachmentPath; set { attachmentPath = value; OnPropertyChanged(nameof(AttachmentPath)); } }

        public ObservableCollection<RegisterDailyMeails> DataRegisterDaily
        {
            get { if (dataRegisterDaily == null) dataRegisterDaily = new ObservableCollection<RegisterDailyMeails>(); return dataRegisterDaily; }
            set { dataRegisterDaily = value; OnPropertyChanged(nameof(DataRegisterDaily)); }
        }

        public DateTime DateRegister { get => dateRegister; set { dateRegister = value; Get_DataRegisterDaily(); OnPropertyChanged(nameof(DateRegister)); } }

        public Meal_time SelectedMealTime { get => selectedMealTime; set { selectedMealTime = value; Get_DataRegisterDaily(); OnPropertyChanged(nameof(SelectedMealTime)); } }

        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }

        async void Get_DataRegisterDaily()
        {
            IsLoading = true;
            Application.Current.Dispatcher.Invoke(() => { DataRegisterDaily.Clear(); });
            await Task.Run(() =>
            {
                if (SelectedMealTime != null && DateRegister != null)
                {
                    string query = $"Select A.*,B.meal,C.category_name,D.full_name from daily_meals A with(nolock) " +
                    "inner join meal_time B On A.meal_time_id = B.id inner join meal_category C On A.category_id = C.category_id inner join employees D On A.employee_code = D.employee_code " +
                    $"Where A.meal_date = '{DateRegister.ToString("yyy-MM-dd")}' and A.meal_time_id = {SelectedMealTime.Id} and A.registration_by = '{registration_by}';";
                    var data = SQLService.Method.ExecuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
                    if (string.IsNullOrEmpty(exception))
                    {
                        if (data != null && data.Rows.Count > 0)
                        {
                            int i = 0;
                            foreach (DataRow row in data.Rows)
                            {
                                i++;
                                Application.Current.Dispatcher.Invoke(() => DataRegisterDaily.Add(new RegisterDailyMeails(i, row)));
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(exception);
                    }
                }
            });
            IsLoading = false;
        }

        void Get_MealTime()
        {
            DataMealTime.Clear();
            string query = "Select * from [meal_time] order by Id;";
            var data = SQLService.Method.ExecuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
            if (string.IsNullOrEmpty(exception))
            {
                if (data != null && data.Rows.Count > 0)
                {
                    foreach (DataRow row in data.Rows)
                    {
                        DataMealTime.Add(new Meal_time(row));
                    }
                }
            }
            else
            {
                MessageBox.Show(exception);
            }
        }


        bool CheckDateRegister()
        {
            string query = "Select GETDATE() as 'DateSystem'";
            var data = SQLService.Method.ExecuteScalar(out string exception, SQLService.Server.SV68_HRM, query);
            if (string.IsNullOrEmpty(exception))
            {
                if (data != null && data is DateTime dateSystem)
                {
                    var dateCompare = new DateTime(DateRegister.Year, DateRegister.Month, DateRegister.Day, 9, 0, 0, 0);
                    if (dateSystem > dateCompare) return false;
                    else return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                MessageBox.Show(exception);
                return false;
            }

        }


        async void RegisterMeals()
        {
            if (!string.IsNullOrEmpty(AttachmentPath) && SelectedMealTime != null)
            {
                try
                {
                    IsLoading = true;
                    await Task.Run(() =>
                    {
                        if (CheckDateRegister())
                        {
                            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                            FileInfo fileInfo = new FileInfo(AttachmentPath);
                            var buiderQuery = new StringBuilder();
                            bool IsFileMaster;
                            using (ExcelPackage package = new ExcelPackage(fileInfo, false))
                            {
                                var workbook = package.Workbook;
                                IsFileMaster = workbook.Worksheets.Any(s => s.Name == "Meal Register");
                                if (IsFileMaster)
                                {
                                    var sheet = workbook.Worksheets["Meal Register"];
                                    int start_r = 7;
                                    string meal_date = DateRegister.ToString("yyyy-MM-dd");
                                    string dateInfo = Convert_DateTimeEpplus(sheet.Cells[3, 3].Value).ToString("yyyy-MM-dd");
                                    string mealInfo = sheet.Cells[4, 3].Text.Trim();

                                    if (mealInfo == $"{SelectedMealTime.Id}:{SelectedMealTime.Meal}" && dateInfo == meal_date)
                                    {
                                        if (string.IsNullOrEmpty(mealInfo))
                                        {
                                            MessageBox.Show("Invalid meal time.");
                                            return;
                                        }
                                        int meal_time_id = Convert.ToInt32(mealInfo.Split(':')[0]);                                        
                                        while (!string.IsNullOrEmpty(sheet.Cells[start_r, 3].Text))
                                        {
                                            string category = sheet.Cells[start_r, 5].Text.Trim();
                                            if (category != "N/A")
                                            {
                                                if (string.IsNullOrEmpty(category))
                                                {
                                                    MessageBox.Show($"Invalid data. Row: {start_r}");
                                                    return;
                                                }

                                                int category_id = Convert.ToInt32(category.Split(':')[0]);
                                                string employee_code = sheet.Cells[start_r, 3].Text;
                                                string employee_name = sheet.Cells[start_r, 4].Text;

                                                string query_check = $"Select count(*) from [daily_meals] Where meal_date = '{DateRegister.ToString("yyyy-MM-dd")}' and employee_code = '{employee_code}' and meal_time_id = '{meal_time_id}';";
                                                var res = SQLService.Method.ExecuteScalar(out string exception, SQLService.Server.SV68_HRM, query_check);
                                                if (string.IsNullOrEmpty(exception))
                                                {
                                                    if ((int)res > 0)
                                                    {
                                                        string query = $"Update [daily_meals] Set [category_id] = {category_id},[registration_by] = '{registration_by}',[registration_at] = GETDATE() " +
                                                            $"Where meal_date = '{meal_date}' and employee_code = '{employee_code}' and meal_time_id = '{meal_time_id}';";
                                                        buiderQuery.AppendLine(query);
                                                    }
                                                    else
                                                    {
                                                        string query = "Insert Into [daily_meals] ([meal_date],[employee_code],[meal_time_id],[category_id],[registration_by],[registration_at]) " +
                                                            $"Values ( '{meal_date}','{employee_code}',{meal_time_id},{category_id},'{registration_by}',GETDATE());";
                                                        buiderQuery.AppendLine(query);
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show(exception);
                                                    return;
                                                }
                                            }
                                            else
                                            {

                                            }
                                            start_r++;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Please recheck Date and Meal time, does not match the selected information.");
                                    }

                                }
                            }
                            // Check format
                            if (IsFileMaster)
                            {
                                if (buiderQuery.Length > 0)
                                {
                                    var res = SQLService.Method.ExecuteNonTrans(out string exception, SQLService.Server.SV68_HRM, buiderQuery.ToString());
                                    if (string.IsNullOrEmpty(exception))
                                    {
                                        if ((int)res > 0) { 
                                            Get_DataRegisterDaily();
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
                                else
                                {
                                    MessageBox.Show("No data to update.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("This is not master. Please recheck.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Cannot register for this date.");
                        }

                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally { IsLoading = false; }
            }
            else
            {
                MessageBox.Show("Attachment, meal time is null or empty.");
            }
        }

        DateTime Convert_DateTimeEpplus(object cellValue)
        {
            if (cellValue is DateTime)
            {
                return (DateTime)cellValue;
            }
            else if (cellValue is double)
            {
                // Nếu giá trị là số serial (Excel lưu ngày tháng dưới dạng số)
                return DateTime.FromOADate((double)cellValue);
            }
            else
            {
                // Nếu không thể chuyển đổi, xử lý lỗi hoặc giá trị mặc định
                throw new InvalidCastException("Cell value is not a valid date.");
            }
        }
        void Get_AttachmentPath()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Excel file|*.xlsx";
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == true)
                AttachmentPath = dialog.FileName;
        }

        void Clear_AttachmentPath()
        {
            AttachmentPath = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RegisterDailyMeails
    {
        public RegisterDailyMeails(int id, DataRow row)
        {
            this.id = id;
            this.meal_date = Convert.ToDateTime(row["meal_date"]);
            this.employee_code = Convert.ToString(row["employee_code"]);
            this.Full_name = Convert.ToString(row["full_name"]);
            this.meal_time_id = Convert.ToInt32(row["meal_time_id"]);
            this.meal = row["meal"].ToString();
            this.category_id = Convert.ToInt32(row["category_id"]);
            this.category_name = row["category_name"].ToString();
            this.registration_by = Convert.ToString(row["registration_by"]);
            this.registration_at = ConvertDate(row["registration_at"]);
        }
        DateTime? ConvertDate(object value)
        {
            if (value != DBNull.Value && value is DateTime) return Convert.ToDateTime(value);
            else return null;
        }

        private int id;
        private DateTime meal_date;
        private string employee_code;
        private string full_name;
        private int meal_time_id;
        private string meal;
        private int category_id;
        private string category_name;
        private string registration_by;
        private DateTime? registration_at;

        public int Id { get => id; set => id = value; }
        public DateTime Meal_date { get => meal_date; set => meal_date = value; }
        public string Employee_code { get => employee_code; set => employee_code = value; }
        public string Full_name { get => full_name; set => full_name = value; }
        public int Meal_time_id { get => meal_time_id; set => meal_time_id = value; }
        public string Meal { get => meal; set => meal = value; }
        public int Category_id { get => category_id; set => category_id = value; }
        public string Category_name { get => category_name; set => category_name = value; }
        public string Registration_by { get => registration_by; set => registration_by = value; }
        public DateTime? Registration_at { get => registration_at; set => registration_at = value; }
        
    }
}
