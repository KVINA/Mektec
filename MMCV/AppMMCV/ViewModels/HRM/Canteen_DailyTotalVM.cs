using AppMMCV.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace AppMMCV.ViewModels.HRM
{
    public class Canteen_DailyTotalVM : INotifyPropertyChanged
    {
        public Canteen_DailyTotalVM()
        {
            Command_Get_DataDailyTotal = new RelayCommand(Get_DataDailyTotal);
            Get_DataDailyTotal();
            //GetData();
        }

        void ExcuteGet_DeatialDepartment()
        {
            if (SelectedDetail != null)
            {
                var id = (int)SelectedDetail.Row["id"];
                Get_DeatialDepartment(id);
            }
            else
            {
                ExtentDetail = null;
            }
        }
        async void Get_DeatialDepartment(int departmentId)
        {
            if (!IsLoading) IsLoading = true;
            await Task.Run(() =>
            {
                //Lấy thông tin công nhân viên trong phòng ban
                string exception = null;
                string query_item = $"Exec usp_mmcv_employee_by_department {departmentId}";
                var data_item = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV91_EZ_MEKTEC, query_item);
                if (string.IsNullOrEmpty(exception))
                {
                    if (data_item != null && data_item.Rows.Count > 0)
                    {
                        //Lấy mã nhân viên
                        string STR_Employees = string.Join(",", data_item.AsEnumerable().Select(d => $"'{d.Field<string>("EmployeeCode")}'").ToList());

                        //Lấy dữ liệu tổng
                        string query_total = "Select B.id,B.meal,C.category_id,C.category_name,quantity from " +
                        "(Select meal_time_id,category_id,Count(employee_code) 'quantity' from [daily_meals] " +
                        $"Where meal_date = '{SelectedDate.ToString("yyyy-MM-dd")}' and employee_code in ({STR_Employees}) group by meal_time_id,category_id)  as A " +
                        "Inner join meal_time B On A.meal_time_id = B.id " +
                        "Inner join meal_category C On A.category_id = C.category_id order by id,category_id;";
                        var data_total = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV68_HRM, query_total);
                        ExtentDetail = data_total.DefaultView;
                    }
                    else
                    {
                        ExtentDetail = null;
                    }
                }
                else
                {
                    MessageBox.Show($"Danh sách nhân viên trống:\n{exception}");
                }
            });
            
            if (IsLoading) IsLoading = false;
        }
        async void Get_DataDailyTotal()
        {
            if (!IsLoading) IsLoading = true;
            Application.Current.Dispatcher.Invoke(() =>
            {
                DataDailyDetail = null;
                DataDailyTotal.Clear();
            });
            await Task.Run(() =>
            {
                string exception = string.Empty;
                //Lấy ra danh sách phòng ban
                string query_department = "Select * from tbMD_CompanyStructure with(nolock) Where ParentID in (select ID from tbMD_CompanyStructure with(nolock) Where ParentID = 1);";
                var data_department = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV91_EZ_MEKTEC, query_department);
                if (!string.IsNullOrEmpty(exception)) goto The_Exception;
                if (data_department != null && data_department.Rows.Count > 0)
                {
                    //Tạo một datatable lưu trữ phòng ban
                    DataTable dt_department = new DataTable();
                    dt_department.Columns.Add("ID", typeof(int));
                    dt_department.Columns.Add("Department", typeof(string));
                    dt_department.Columns.Add("Quantity", typeof(int));
                    foreach (DataRow item_department in data_department.Rows)
                    {
                        var clone = dt_department.NewRow();
                        clone[0] = item_department.Field<int>("ID");
                        clone[1] = item_department.Field<string>("Name");
                        clone[2] = 0;
                        dt_department.Rows.Add(clone);
                    }
                    //Đi lấy danh sách công nhân viên đang làm việc tại nhà máy
                    //string query_employees = "usp_mmcv_employee_working";
                    string query_employees = "Select * from [tbHR_Employee] with(Nolock);";
                    var data_employees = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV91_EZ_MEKTEC, query_employees);
                    if (!string.IsNullOrEmpty(exception)) goto The_Exception;
                    if (data_employees != null && data_employees.Rows.Count > 0)
                    {
                        //Lấy dữ liệu tổng
                        string query_total = "Select B.id,B.meal,C.category_id,C.category_name,quantity from " +
                        "(Select meal_time_id,category_id,Count(employee_code) 'quantity' from [daily_meals] with(nolock) " +
                        $"Where meal_date = '{SelectedDate.ToString("yyyy-MM-dd")}' group by meal_time_id,category_id)  as A " +
                        "Inner join meal_time B On A.meal_time_id = B.id " +
                        "Inner join meal_category C On A.category_id = C.category_id order by id,category_id;";
                        var data_total = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV68_HRM, query_total);
                        if (!string.IsNullOrEmpty(exception)) goto The_Exception;
                        if (data_total != null && data_total.Rows.Count > 0)
                        {
                            foreach (DataRow row in data_total.Rows)
                            {
                                Application.Current.Dispatcher.Invoke(() => DataDailyTotal.Add(new DailyReport(row)));
                            }
                        }
                        //Lấy dữ liệu đăng ký suất ăn của tất cả phòng ban trong ngày
                        string query_daily = $"Select A.*,B.meal,C.category_name from daily_meals A with(nolock) " +
                               "inner join meal_time B On A.meal_time_id = B.id inner join meal_category C On A.category_id = C.category_id " +
                               $"Where A.meal_date = '{SelectedDate.ToString("yyyy-MM-dd")}';";
                        var data_daily = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV68_HRM, query_daily);
                        if (!string.IsNullOrEmpty(exception)) goto The_Exception;
                        if (data_daily != null && data_daily.Rows.Count > 0)
                        {
                            foreach (DataRow item_department in dt_department.Rows)
                            {
                                int ID = (int)item_department["ID"];
                                string query_item = $"Exec usp_mmcv_employee_by_department {ID}";
                                var data_item = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV91_EZ_MEKTEC, query_item);
                                if (!string.IsNullOrEmpty(exception)) goto The_Exception;
                                if (data_item != null && data_item.Rows.Count > 0)
                                {
                                    //Đếm số lượng người đã báo cơm
                                    var commonCodes = from A in data_daily.AsEnumerable()
                                                      join B in data_item.AsEnumerable()
                                                      on A.Field<string>("employee_code") equals B.Field<string>("EmployeeCode")
                                                      select new
                                                      {
                                                          ID = A.Field<long>("id"),
                                                          EmployeeCode = A.Field<string>("employee_code"),
                                                          Meal = A.Field<string>("meal"),
                                                          Category = A.Field<string>("category_name")
                                                      };
                                    int count = commonCodes.Count();
                                    item_department["Quantity"] = count;
                                }

                            }
                            //Đếm tổng số của các phòng
                            var sum = dt_department.AsEnumerable().Sum(s => s.Field<int>("Quantity"));
                            //Thêm môt dòng Other (Các giàm đốc, trưởng bộ phận phông thuộc phòng)
                            var other = dt_department.NewRow();
                            other[0] = 9999;
                            other[1] = "Other";
                            other[2] = data_daily.Rows.Count - sum;
                            dt_department.Rows.Add(other);

                            Application.Current.Dispatcher.Invoke(() => { DataDailyDetail = dt_department.DefaultView; });
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        MessageBox.Show("Không lấy được danh sách công nhân viên. [tbHR_Employee]");
                    }
                }
                else
                {
                    MessageBox.Show("Không lấy được thông tin phòng ban.", "ERROR");
                }
                if (IsLoading) IsLoading = false;
                return;
            The_Exception:
                MessageBox.Show(exception, "Exception");
                if (IsLoading) IsLoading = false;
            });

        }

        DataRowView selectedDetail;
        bool isLoading;
        DataView dataDailyDetail;
        DataView extentDetail;
        ObservableCollection<DailyReport> dataDailyTotal;
        DateTime selectedDate = DateTime.Now;
        public ICommand Command_Get_DataDailyTotal { get; set; }
        public ObservableCollection<DailyReport> DataDailyTotal
        {
            get { if (dataDailyTotal == null) dataDailyTotal = new ObservableCollection<DailyReport>(); return dataDailyTotal; }
            set { dataDailyTotal = value; OnPropertyChanged(nameof(DataDailyTotal)); }
        }
        public DateTime SelectedDate { get => selectedDate; set { selectedDate = value; OnPropertyChanged(nameof(SelectedDate)); Get_DataDailyTotal(); } }

        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }

        public DataView DataDailyDetail { get => dataDailyDetail; set { dataDailyDetail = value; OnPropertyChanged(nameof(DataDailyDetail)); } }

        public DataRowView SelectedDetail { get => selectedDetail; set { selectedDetail = value; OnPropertyChanged(nameof(SelectedDetail)); ExcuteGet_DeatialDepartment(); } }

        public DataView ExtentDetail { get => extentDetail; set { extentDetail = value; OnPropertyChanged(nameof(ExtentDetail)); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DailyReport
    {
        public DailyReport(DataRow row)
        {
            this.meal_time_id = Convert.ToInt32(row["id"]);
            this.meal = Convert.ToString(row["meal"]);
            this.category_id = Convert.ToInt32(row["category_id"]);
            this.category_name = Convert.ToString(row["category_name"]);
            this.quantity = Convert.ToInt32(row["quantity"]);
        }

        private int meal_time_id;
        private string meal;
        private int category_id;
        private string category_name;
        private int quantity;

        public int Meal_time_id { get => meal_time_id; set => meal_time_id = value; }
        public string Meal { get => meal; set => meal = value; }
        public int Category_id { get => category_id; set => category_id = value; }
        public string Category_name { get => category_name; set => category_name = value; }
        public int Quantity { get => quantity; set => quantity = value; }
    }
}
