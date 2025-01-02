using AppMMCV.Services;
using LibraryHelper.Models.HRM;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Ranges;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table.PivotTable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
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
        public class EmployeeInfo
        {
            public string EmployeeCode { get; set; }
            public string FullName { get; set; }
            public string Department { get; set; }
        }
        public ICommand Command_ExecuteSaveDialogEdit { get; }
        public ICommand Command_ExecuteShowDialogEdit { get; }
        public ICommand Command_ExecuteRegisterMeals { get; }
        public ICommand Command_ExecuteGetAttachmentPath { get; }
        public ICommand Command_ExecuteClearAttachmentPath { get; }
        public ICommand Command_Get_DataRegisterDaily { get; }
        public ICommand Command_ExportExcel { get; }
        public Canteen_RegisterDailyMealVM()
        {
            Get_MealTime();
            Get_DataRegisterDaily();
            Command_ExecuteGetAttachmentPath = new RelayCommand(Get_AttachmentPath);
            Command_ExecuteClearAttachmentPath = new RelayCommand(Clear_AttachmentPath);
            Command_ExecuteRegisterMeals = new RelayCommand(RegisterMeals);
            Command_ExecuteShowDialogEdit = new RelayCommand(ShowDialogEdit);
            Command_ExecuteSaveDialogEdit = new RelayCommand(SaveDialogEdit);
            Command_Get_DataRegisterDaily = new RelayCommand(Get_DataRegisterDaily);
            Command_ExportExcel = new RelayCommand(ExportExcel);
        }
        string employeeCode;
        string employeeName;
        ObservableCollection<string> mealCategorys;
        ObservableCollection<CategoryDetail> listMeal;
        DataView dataDailyMeal;
        DataView dataDailyTotal;
        string registration_by = DataService.UserInfo.username;
        bool isLoading;
        bool isOpenDialogEdit;
        ObservableCollection<RegisterDailyMeails> dataRegisterDaily;
        DataRowView selectedRegisterDaily;
        ObservableCollection<Meal_time> dataMealTime;
        //Meal_time selectedMealTime;
        string attachmentPath;
        DateTime dateRegister = DateTime.Now;
        string searchValue;

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

        //public Meal_time SelectedMealTime { get => selectedMealTime; set { selectedMealTime = value; Get_DataRegisterDaily(); OnPropertyChanged(nameof(SelectedMealTime)); } }

        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }



        public DataView DataDailyMeal { get => dataDailyMeal; set { dataDailyMeal = value; OnPropertyChanged(nameof(DataDailyMeal)); } }

        public DataView DataDailyTotal { get => dataDailyTotal; set { dataDailyTotal = value; OnPropertyChanged(nameof(DataDailyTotal)); } }

        public DataRowView SelectedRegisterDaily { get => selectedRegisterDaily; set { selectedRegisterDaily = value; OnPropertyChanged(nameof(SelectedRegisterDaily)); } }

        public bool IsOpenDialogEdit { get => isOpenDialogEdit; set { isOpenDialogEdit = value; OnPropertyChanged(nameof(IsOpenDialogEdit)); } }

        public string EmployeeCode { get => employeeCode; set { employeeCode = value; OnPropertyChanged(nameof(EmployeeCode)); } }
        public string EmployeeName { get => employeeName; set { employeeName = value; OnPropertyChanged(nameof(EmployeeName)); } }

        public ObservableCollection<CategoryDetail> ListMeal { get => listMeal; set { listMeal = value; OnPropertyChanged(nameof(ListMeal)); } }

        public ObservableCollection<string> MealCategorys { get => mealCategorys; set { mealCategorys = value; OnPropertyChanged(nameof(ListMeal)); } }

        public string SearchValue { get => searchValue; set { searchValue = value; OnPropertyChanged(nameof(SearchValue)); } }

        /// <summary>
        /// Lấy thông tin các suất ăn
        /// </summary>
        void GET_MealCategory(out string exception)
        {
            string query = "Select CONCAT(category_id,':',category_name) category from [meal_category] order by category_id;";
            var data = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV68_HRM, query);
            if (string.IsNullOrEmpty(exception))
            {
                if (data != null && data.Rows.Count > 0)
                {
                    var list = data.AsEnumerable().Select(s => s.Field<string>("category")).ToList();
                    list.Add("N/A");
                    MealCategorys = new ObservableCollection<string>(list);
                }
                else
                {
                    exception = "Không lấy được thông tin suất ăn [meal_category].";
                }
            }
        }
        void SaveDialogEdit()
        {
            if (SelectedRegisterDaily != null)
            {
                if (string.IsNullOrEmpty(EmployeeCode))
                {
                    MessageBox.Show("Mã nhân viên null hoặc empty.");
                    return;
                }
                if (string.IsNullOrEmpty(EmployeeName))
                {
                    MessageBox.Show("Tên nhân viên null hoặc empty.");
                    return;
                }
                string exception;
                if (ListMeal != null)
                {
                    //var clonedRow = SelectedRegisterDaily.Row.Table.NewRow();
                    //clonedRow.ItemArray = SelectedRegisterDaily.Row.ItemArray.Clone() as object[];
                    var builder = new StringBuilder();
                    foreach (var item in ListMeal)
                    {
                        if (string.IsNullOrEmpty(item.SelectedCategory)) item.SelectedCategory = "N/A";
                        //clonedRow[item.MealName] = item.SelectedCategory;
                        string date = DateRegister.ToString("yyyy-MM-dd");
                        string meal_id = item.MealName.Split(':')[0];
                        string query_check = $"Select Count(*) from [daily_meals] Where meal_date = '{date}' And employee_code = '{EmployeeCode}' and meal_time_id = '{meal_id}';";
                        var data_check = SQLService.Method.ExecuteScalar(out exception, SQLService.Server.SV68_HRM, query_check);
                        if (!string.IsNullOrEmpty(exception)) goto THE_EXCEPTION;

                        if (item.SelectedCategory == "N/A")
                        {
                            if ((int)data_check > 0)
                            {
                                builder.AppendLine($"Update [daily_meals] Set [is_deleted] = 1, [update_by] = '{registration_by}', [update_at] = GetDate() Where [meal_date] = '{date}' And employee_code = '{EmployeeCode}' and meal_time_id = '{meal_id}';");
                            }
                        }
                        else
                        {
                            string category_id = item.SelectedCategory.Split(':')[0];
                            if ((int)data_check > 0)
                            {
                                builder.AppendLine($"Update [daily_meals] Set [category_id] = '{category_id}', [is_deleted] = 0, [update_by] = '{registration_by}', [update_at] = GetDate() Where [meal_date] = '{date}' And employee_code = '{EmployeeCode}' and meal_time_id = '{meal_id}';");
                            }
                            else
                            {
                                builder.AppendLine($"Insert Into [daily_meals] ([meal_date],[employee_code],[meal_time_id],[category_id],[registration_by],[registration_at]) Values ('{date}','{EmployeeCode}',{meal_id},{category_id},'{registration_by}',GETDATE());");
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(builder.ToString())) //Kiểm tra có lệnh update không
                    {
                        var data = SQLService.Method.ExecuteNonTrans(out exception, SQLService.Server.SV68_HRM, builder.ToString());
                        if (!string.IsNullOrEmpty(exception)) goto THE_EXCEPTION;
                        if ((int)data > 0)
                        {
                            MessageBox.Show("Lưu thành công");
                            //SelectedRegisterDaily.Row.ItemArray = clonedRow.ItemArray.Clone() as object[];
                            Get_DataRegisterDaily();
                            IsOpenDialogEdit = false;
                        }
                        else
                        {
                            MessageBox.Show("Không có thông tin nào được thay đổi");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Danh sách bữa ăn null.");
                }
                return;
            THE_EXCEPTION:
                MessageBox.Show(exception);
            }
            else
            {
                MessageBox.Show("Selected item error: Item is null.");
            }
        }
        /// <summary>
        /// Hiển thị thông tin để Edit
        /// </summary>
        void ShowDialogEdit()
        {

            if (SelectedRegisterDaily != null)
            {
                if (MealCategorys == null) //Nếu chưa có thông tin bữa ăn thì đi lấy
                {
                    GET_MealCategory(out string exception); //Lỗi sẽ thông báo ra màn hình
                    if (string.IsNullOrEmpty(exception)) goto THE_CONTINUE;
                    else
                    {
                        MessageBox.Show(exception);
                        return;
                    }
                }
            THE_CONTINUE:
                EmployeeCode = SelectedRegisterDaily.Row.Field<string>("EmployeeCode");
                EmployeeName = SelectedRegisterDaily.Row.Field<string>("EmployeeName");
                var list = new ObservableCollection<CategoryDetail>();
                foreach (DataColumn header in SelectedRegisterDaily.DataView.Table.Columns)
                {
                    if (header.ColumnName != null && header.ColumnName.Contains(':'))
                    {
                        var category = new CategoryDetail();
                        category.Categorys = MealCategorys;
                        category.MealName = header.ColumnName;
                        category.SelectedCategory = SelectedRegisterDaily.Row.Field<string>(header.ColumnName);
                        list.Add(category);
                    }
                }
                ListMeal = list;
                IsOpenDialogEdit = true;
            }
            else
            {
                IsOpenDialogEdit = false;
            }
        }
        void FillBorderExcel(ExcelRange range, ExcelBorderStyle style, System.Drawing.Color color)
        {
            // Kẻ bảng với màu nâu nhạt cho đường kẻ
            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            // Chọn màu cho đường kẻ (màu nâu nhạt)
            var lightBrown = ColorTranslator.FromHtml("#A52A2A");  // Màu nâu (brown)
            range.Style.Border.Top.Color.SetColor(lightBrown);
            range.Style.Border.Bottom.Color.SetColor(lightBrown);
            range.Style.Border.Left.Color.SetColor(lightBrown);
            range.Style.Border.Right.Color.SetColor(lightBrown);
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
        
        async void ExportExcel()
        {
            try
            {
                IsLoading = true;
                await Task.Run(() =>
                {
                    if (DataDailyMeal.Count > 0)
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
                                    sheet.Column(1).Width = 4;
                                    sheet.Column(2).Width = 8;
                                    sheet.Column(3).Width = 15;
                                    sheet.Column(4).Width = 40;
                                    sheet.Cells[2, 2].Value = "MASTER ĐĂNG KÝ SUẤT ĂN";
                                    sheet.Cells[2, 2].Style.Font.Bold = true;
                                    sheet.Cells[3, 2].Value = "Ngày";
                                    sheet.Cells[3, 3].Value = DateRegister;
                                    sheet.Cells[3, 3].Style.Numberformat.Format = "yyy/MM/dd";
                                    sheet.Cells[3, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    sheet.Cells[4, 2].Value = "Version";
                                    sheet.Cells[4, 3].Value = "Ver20241101";
                                    int start_r = 5;
                                    int write_r = start_r;
                                    foreach (var option in option_category) // Thực đơn
                                    {
                                        write_r++;
                                        int column = 5 + option_meal.Count;
                                        sheet.Cells[write_r, column].Value = option;
                                    }
                                    write_r++;
                                    sheet.Cells[write_r, 2].Value = "STT";
                                    sheet.Cells[write_r, 3].Value = "Mã nhân viên";
                                    sheet.Cells[write_r, 4].Value = "Tên nhân viên";

                                    int start_c = 4;
                                    int write_c = start_c;
                                    foreach (var option in option_meal)
                                    {
                                        write_c++;
                                        sheet.Cells[start_r, write_c].Value = option;
                                        sheet.Cells[write_r, write_c].Value = option;
                                        sheet.Column(write_c).Width = 15;
                                    }
                                    write_c++;
                                    sheet.Cells[start_r, write_c].Value = "Suất ăn"; //Suất ăn
                                    sheet.Cells[write_r, write_c].Value = "Ghi chú"; //Ghi chú
                                    sheet.Column(write_c).Width = 15;

                                    FillBorderExcel(sheet.Cells[start_r, start_c + 1, write_r - 1, write_c], ExcelBorderStyle.Thin, ColorTranslator.FromHtml("#A52A2A"));

                                    for (int i = start_r + 1; i < write_r; i++)
                                        for (int j = start_c + 1; j < write_c; j++)
                                            sheet.Cells[i, j].FormulaR1C1 = $"=COUNTIF(R{write_r + 1}C{j}:R[5000]C,R{i}C{write_c})";

                                    sheet.Cells[start_r + 1, start_c + 1, write_r - 1, write_c - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                    sheet.Cells[start_r, start_c + 1, start_r, write_c].Style.Font.Bold = true;
                                    sheet.Cells[start_r, start_c + 1, start_r, write_c].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    sheet.Cells[start_r, start_c + 1, start_r, write_c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    sheet.Cells[start_r, start_c + 1, start_r, write_c].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.AliceBlue);

                                    sheet.Cells[write_r, 2, write_r, write_c].Style.Font.Bold = true;
                                    sheet.Cells[write_r, 2, write_r, write_c].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    sheet.Cells[write_r, 2, write_r, write_c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    sheet.Cells[write_r, 2, write_r, write_c].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.AliceBlue);  // Màu xanh nhạt
                                    start_r = write_r;

                                    sheet.Cells[5, 2].Value = "Row"; //Dòng bắt đầu chứa dữ liệu công nhân viên 
                                    sheet.Cells[5, 3].Value = write_r; //Giá trị dòng
                                    sheet.Cells[5, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; //Căn lề trái

                                    int stt = 0;
                                    foreach (DataRow row in DataDailyMeal.Table.Rows)
                                    {
                                        stt++;
                                        write_r++;
                                        sheet.Cells[write_r, 2].Value = stt;
                                        sheet.Cells[write_r, 3].Value = row.Field<string>("EmployeeCode");
                                        sheet.Cells[write_r, 4].Value = row.Field<string>("EmployeeName");
                                        int columnIndex = 4;
                                        foreach (var option in option_meal)
                                        {
                                            columnIndex++;

                                            sheet.Cells[write_r, columnIndex].Value = row.Field<string>(option);
                                        }
                                        sheet.Cells[write_r, write_c].Value = row.Field<string>("Department");
                                    }

                                    sheet.View.ShowGridLines = false;

                                    var validation_category = sheet.DataValidations.AddListValidation(sheet.Cells[start_r + 1, start_c + 1, write_r, write_c - 1].Address);
                                    foreach (var option in option_category) validation_category.Formula.Values.Add(option);

                                    var dataRange = sheet.Cells[start_r, 2, write_r, write_c];
                                    FillBorderExcel(dataRange, ExcelBorderStyle.Thin, ColorTranslator.FromHtml("#A52A2A"));

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
        async void Get_DataRegisterDaily()
        {
            IsLoading = true;
            string exception = string.Empty;
            Application.Current.Dispatcher.Invoke(() =>
            {
                DataDailyMeal = null;
                DataDailyTotal = null;
            });
            await Task.Run(() =>
            {                
                //Lấy dữ liệu các bữa ăn
                string query_meal = "Select [id],[meal] from [meal_time]";
                var data_meal = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV68_HRM, query_meal);
                if (!string.IsNullOrEmpty(exception)) goto The_Exception;
                if (data_meal != null && data_meal.Rows.Count > 0)
                {
                    //Tab table lưu trữ thông tin tổng
                    DataTable tb_total = new DataTable();
                    tb_total.Columns.Add("Bữa ăn", typeof(string));

                    //Tạo data lưu trữ thông tin đăng ký suất ăn
                    DataTable data = new DataTable();
                    data.Columns.Add("No", typeof(long));
                    data.Columns.Add("Date", typeof(DateTime));
                    data.Columns.Add("EmployeeCode", typeof(string));
                    data.Columns.Add("EmployeeName", typeof(string));
                    data.Columns.Add("Department", typeof(string));
                    foreach (DataRow row_meal in data_meal.Rows)
                    {
                        string meal = $"{row_meal.Field<int>("Id")}:{row_meal.Field<string>("meal")}";
                        data.Columns.Add(meal, typeof(string));
                        tb_total.Columns.Add(meal, typeof(int));
                    }
                    //Đi lấy thông tin các suất ăn
                    string query_category = "Select * from [meal_category] order by [category_id]";
                    var data_category = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV68_HRM, query_category);
                    if (!string.IsNullOrEmpty(exception)) goto The_Exception;
                    if (data_category != null && data_category.Rows.Count > 0)
                    {
                        foreach (DataRow item_category in data_category.Rows)
                        {
                            var clone = tb_total.NewRow();
                            clone[0] = $"{item_category.Field<int>("category_id")}:{item_category.Field<string>("category_name")}";
                            for (int i = 1; i < clone.ItemArray.Length; i++) clone[i] = 0;
                            tb_total.Rows.Add(clone);
                        }
                    }
                    //Đi lấy danh sách công nhân viên đang làm việc tại nhà máy string query_employees = "usp_mmcv_employee_working";                    
                    string query_employees = "SELECT A.*,C.Name 'Department' from [tbHR_Employee] A with(nolock) Inner Join tbHR_DepartmentHistory B with(NoLock) On A.ID = B.EmployeeID Inner Join tbMD_CompanyStructure C with(NoLock) On B.DepartmentID = C.ID Where LastWorkingDay is null or LastWorkingDay >= Convert(DATE,GetDate());";
                    var data_employees = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV91_EZ_MEKTEC, query_employees);                    
                    if (!string.IsNullOrEmpty(exception)) goto The_Exception;
                    if (data_employees != null && data_employees.Rows.Count > 0)
                    {
                        var listInfo = data_employees.AsEnumerable().Select(d=> new EmployeeInfo()
                        {
                            EmployeeCode = d.Field<string>("EmployeeCode"),
                            FullName = d.Field<string>("FullName"),
                            Department = d.Field<string>("Department")
                        }).ToList();

                        //Đi lấy nhân viên nhà thầu làm việc tại nhà máy
                        string query_contractor = "Select employee_code,full_name,department FROM [CANTEEN].[dbo].[employees] with(nolock);";
                        var data_contractor = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV68_HRM, query_contractor);
                        if (!string.IsNullOrEmpty(exception)) goto The_Exception;
                        if (data_contractor != null && data_contractor.Rows.Count > 0)
                        {
                            var listInfo_Sub = data_contractor.AsEnumerable().Select(d => new EmployeeInfo()
                            {
                                EmployeeCode = d.Field<string>("employee_code"),
                                FullName = d.Field<string>("full_name"),
                                Department = d.Field<string>("department")
                            }).ToList();
                            listInfo.AddRange(listInfo_Sub);
                        }

                        //Đi lấy danh sách nhân viên đăng ký ăn
                        string query_daily = $"Select A.*,B.meal,C.category_name from daily_meals A with(nolock) " +
                        "inner join meal_time B On A.meal_time_id = B.id inner join meal_category C On A.category_id = C.category_id " +
                        $"Where A.meal_date = '{DateRegister.ToString("yyy-MM-dd")}' and A.registration_by = '{registration_by}' And A.[is_deleted] = 0 order by A.[employee_code];";
                        var data_daily = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV68_HRM, query_daily);
                        if (!string.IsNullOrEmpty(exception)) goto The_Exception;
                        if (data_daily != null && data_daily.Rows.Count > 0)
                        {
                            int id = 1;
                            foreach (DataRow row_daily in data_daily.Rows)
                            {
                                string employee = row_daily.Field<string>("employee_code");
                                string header = $"{row_daily.Field<int>("meal_time_id")}:{row_daily.Field<string>("meal")}";
                                string category = $"{row_daily.Field<int>("category_id")}:{row_daily.Field<string>("category_name")}";
                                if (data.AsEnumerable().Any(d => d.Field<string>("EmployeeCode") == employee))
                                {
                                    var item = data.AsEnumerable().Where(d => d.Field<string>("EmployeeCode") == employee).First();
                                    item[header] = category;
                                }
                                else
                                {
                                    var item = data.NewRow();
                                    for (int i = 3; i < item.ItemArray.Length; i++) item[i] = "N/A";
                                    item["No"] = id;
                                    item["Date"] = DateRegister;
                                    item["EmployeeCode"] = employee;

                                    var itemEmployee = listInfo.FirstOrDefault(s => s.EmployeeCode == employee);
                                    if (itemEmployee == null)
                                    {
                                        string _txt = "Không thuộc danh sách nhân viên công ty hoặc đã nghỉ việc";
                                        item["EmployeeName"] = _txt;
                                        item["Department"] = _txt;
                                    }
                                    else
                                    {
                                        item["EmployeeName"] = itemEmployee.FullName;
                                        item["Department"] = itemEmployee.Department;        
                                    }
                                    item[header] = category;
                                    data.Rows.Add(item);
                                    id++;
                                }
                                //Cộng vào total xuất ăn
                                var item_categor = tb_total.AsEnumerable()
                                        .Where(f => f.Field<string>("Bữa ăn") == category).First();
                                item_categor[header] = (int)item_categor[header] + 1;
                            }
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                DataDailyTotal = tb_total.DefaultView;
                                if (string.IsNullOrEmpty(SearchValue))
                                {
                                    DataDailyMeal = data.DefaultView;
                                }
                                else
                                {
                                    var filteredRows = data.AsEnumerable().Where(f => f.Field<string>("EmployeeCode") == SearchValue);
                                    if (filteredRows.Any())
                                    {
                                        DataDailyMeal = filteredRows.CopyToDataTable().DefaultView;
                                    }
                                    else
                                    {
                                        DataDailyMeal = data.DefaultView;
                                    }
                                }
                                
                            });
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không lấy được thông tin công nhân viên [tbHR_Employee].");
                    }
                }
                else
                {
                    MessageBox.Show("Không lấy được thông tin bữa ăn [meal_time].");
                }
                return;
            The_Exception:
                MessageBox.Show(exception, "Exception");
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
                    dateCompare = dateCompare.AddDays(1);//Lùi lại một ngày
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
            if (!string.IsNullOrEmpty(AttachmentPath))
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
                                IsFileMaster = workbook.Worksheets.Any(s => s.Name == "Meal Register"); // Kiểm tra tên sheet
                                if (IsFileMaster)
                                {
                                    var sheet = workbook.Worksheets["Meal Register"];
                                    string meal_date = DateRegister.ToString("yyyy-MM-dd"); //Ngày báo cơm trên phần mềm
                                    string dateInfo = Convert_DateTimeEpplus(sheet.Cells[3, 3].Value).ToString("yyyy-MM-dd"); //Ngày báo cơm trong file excel
                                    string versionInfo = sheet.Cells[4, 3].Text.Trim(); //Version file master

                                    if (dateInfo == meal_date)
                                    {
                                        int start_r = int.Parse(sheet.Cells[5,3].Text);
                                        int start_c = 5;
                                        int write_c = start_c;

                                        while (!string.IsNullOrEmpty(sheet.Cells[start_r, write_c].Text))
                                        {
                                            string mealInfo = sheet.Cells[start_r, write_c].Text.Trim();
                                            var chekMeal = DataMealTime.AsEnumerable().Any(s => $"{s.Id}:{s.Meal}" == mealInfo);
                                            if (chekMeal)
                                            {
                                                int meal_time_id = Convert.ToInt32(mealInfo.Split(':')[0]);
                                                int write_r = start_r + 1;
                                                while (!string.IsNullOrEmpty(sheet.Cells[write_r, 3].Text))
                                                {
                                                    string category = sheet.Cells[write_r, write_c].Text.Trim();                                                    
                                                    string employee_code = sheet.Cells[write_r, 3].Text; // StaffNo
                                                    string employee_name = sheet.Cells[write_r, 4].Text; // Tên
                                                    if (category != "N/A")
                                                    {
                                                        int category_id = Convert.ToInt32(category.Split(':')[0]); // Id meal
                                                        if (string.IsNullOrEmpty(category))
                                                        {
                                                            MessageBox.Show($"Bạn chưa chọn suất ăn chon công nhân viên dòng: {write_r}");
                                                            return;
                                                        }

                                                        string query_check = $"Select count(*) from [daily_meals] Where meal_date = '{DateRegister.ToString("yyyy-MM-dd")}' and employee_code = '{employee_code}' and meal_time_id = '{meal_time_id}';";
                                                        var res = SQLService.Method.ExecuteScalar(out string exception, SQLService.Server.SV68_HRM, query_check);
                                                        if (string.IsNullOrEmpty(exception))
                                                        {
                                                            if ((int)res > 0)
                                                            {
                                                                string query = $"Update [daily_meals] Set [is_deleted] = 0, [category_id] = {category_id},[registration_by] = '{registration_by}',[registration_at] = GETDATE() " +
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
                                                        string query_check = $"Select count(*) from [daily_meals] Where meal_date = '{DateRegister.ToString("yyyy-MM-dd")}' and employee_code = '{employee_code}' and meal_time_id = '{meal_time_id}';";
                                                        var res = SQLService.Method.ExecuteScalar(out string exception, SQLService.Server.SV68_HRM, query_check);
                                                        if (string.IsNullOrEmpty(exception))
                                                        {

                                                            if ((int)res > 0)
                                                            {
                                                                string query = $"Update [daily_meals] Set [is_deleted] = 1, [registration_by] = '{registration_by}',[registration_at] = GETDATE() " +
                                                                    $"Where meal_date = '{meal_date}' and employee_code = '{employee_code}' and meal_time_id = '{meal_time_id}';";
                                                                buiderQuery.AppendLine(query);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show(exception);
                                                            return;
                                                        }
                                                    }
                                                    write_r++;
                                                }
                                            }
                                            write_c++;
                                        }
                                    }
                                    else
                                    {
                                        IsLoading = false;
                                        MessageBox.Show("Ngày bạn chọn không giống với ngày trong file đăng ký.");
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
                                        if ((int)res > 0)
                                        {
                                            Get_DataRegisterDaily();
                                        }
                                        else
                                        {
                                            IsLoading = false;
                                            MessageBox.Show("Fail.");
                                        }
                                    }
                                    else
                                    {
                                        IsLoading = false;
                                        MessageBox.Show(exception);
                                    }
                                }
                                else
                                {
                                    IsLoading = false;
                                    //MessageBox.Show("No data to update.");
                                }
                            }
                            else
                            {
                                IsLoading = false;
                                MessageBox.Show("This is not master. Please recheck.");
                            }
                        }
                        else
                        {
                            IsLoading = false;
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
            //this.Full_name = Convert.ToString(row["full_name"]);
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
    public class CategoryDetail
    {
        public string MealName { get; set; }
        public string SelectedCategory { get; set; }
        public ObservableCollection<string> Categorys { get; set; }
    }
}
