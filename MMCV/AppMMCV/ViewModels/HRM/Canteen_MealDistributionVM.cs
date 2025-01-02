
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System;
using AppMMCV.Services;
using LibraryHelper.Methord;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using System.Windows.Threading;
using System.Threading;
using LibraryHelper.Models.HRM;
using System.Text;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using System.Media;

namespace AppMMCV.ViewModels.HRM
{
    internal class Canteen_MealDistributionVM : INotifyPropertyChanged
    {
        ObservableCollection<InfoEmployeeSwipeCard> employeeSwipeCards;
        string tableName;
        int EVTGUID = 0;
        private bool isCheckRealTime;
        private bool isLoading;
        private string notifyContent;
        private Brush notifyBrush;
        private int mealCount;
        private string employeeCode;
        private string employeeName;
        private BitmapImage avata;
        private string resultStatus;
        private string computerName;
        private string deviceName;
        private string clockString;
        private string soundOkPath;
        private string soundNgPath;
       
        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }
        public string NotifyContent { get => notifyContent; set { notifyContent = value; OnPropertyChanged(nameof(NotifyContent)); } }
        public Brush NotifyBrush { get => notifyBrush; set { notifyBrush = value; OnPropertyChanged(nameof(NotifyBrush)); } }
        public int MealCount { get => mealCount; set { mealCount = value; OnPropertyChanged(nameof(MealCount)); } }
        public string EmployeeCode { get => employeeCode; set { employeeCode = value; OnPropertyChanged(nameof(EmployeeCode)); } }
        public string EmployeeName { get => employeeName; set { employeeName = value; OnPropertyChanged(nameof(EmployeeName)); } }
        public BitmapImage Avata { get => avata; set { avata = value; OnPropertyChanged(nameof(Avata)); } }
        public string ResultStatus { get => resultStatus; set { resultStatus = value; OnPropertyChanged(nameof(ResultStatus)); } }
        public string ComputerName { get => computerName; set { computerName = value; OnPropertyChanged(nameof(ComputerName)); } }
        public string DeviceName { get => deviceName; set { deviceName = value; OnPropertyChanged(nameof(DeviceName)); } }

        public bool IsCheckRealTime { get => isCheckRealTime; set { isCheckRealTime = value; OnPropertyChanged(nameof(IsCheckRealTime)); } }

        public string TableName { get => tableName; set { tableName = value; OnPropertyChanged(nameof(TableName)); } }

        public ObservableCollection<InfoEmployeeSwipeCard> EmployeeSwipeCards { get => employeeSwipeCards; set { employeeSwipeCards = value; OnPropertyChanged(nameof(EmployeeSwipeCards)); } }

        public string ClockString { get => clockString; set { clockString = value; OnPropertyChanged(nameof(ClockString)); } }

        public string SoundOkPath { get => soundOkPath; set => soundOkPath = value; }
        public string SoundNgPath { get => soundNgPath; set => soundNgPath = value; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Canteen_MealDistributionVM()
        {
            IsCheckRealTime = false;
            StartClock();            
            //Đi lấy thông tin thiết bị được cài đặt (lấy 10 ký tự cuối của tên máy)
            ComputerName = Environment.MachineName.Length <= 10 ? Environment.MachineName : Environment.MachineName.Substring(Environment.MachineName.Length - 10, 10);
            string query = $"Select [device_name] from [device_distribution_meal] with(nolock) where computer_name = '{ComputerName}';";
            var data = SQLService.Method.ExecuteScalar(out string exception, SQLService.Server.SV68_HRM, query);
            if (string.IsNullOrEmpty(exception))
            {
                if (data != null && !string.IsNullOrEmpty(data.ToString()))
                {
                    DeviceName = data.ToString();
                    NotifyBrush = Brushes.Blue;
                    NotifyContent = $"Thiêt bị quẹt thẻ cho cửa này là '{DeviceName}'\nCó thể bắt đầu quẹt thẻ.";
                    EmployeeSwipeCards = new ObservableCollection<InfoEmployeeSwipeCard>();
                    IsCheckRealTime = true;                    
                    ExecuteCheckRealTime();
                }
                else
                {
                    NotifyBrush = Brushes.Red;
                    NotifyContent = $"Thiết bị cài đặt trong master không hợp lệ.\nDeviceName is null or empty.";
                }
            }
            else
            {
                NotifyBrush = Brushes.Red;
                NotifyContent = $"Không tìm thấy thiết bị quẹt thẻ tương ứng với máy tính '{ComputerName}'.\nError: {exception}";
            }
        }

        async void StartClock()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    ClockString = DateTime.Now.ToString("HH:mm:ss");
                    Thread.Sleep(999);
                }
            });
        }
        async void ExecuteCheckRealTime()
        {
            await Task.Run(() =>
            {
                while (IsCheckRealTime)
                {
                    CheckRealTimeServer(out string exception);
                    Thread.Sleep(200);
                }
            });
        }
        /// <summary>
        /// Đếm số lượng suất ăn đã phát
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="date"></param>
        /// <param name="meal_time_id"></param>
        /// <param name="category_id"></param>
        /// <param name="computer"></param>
        void CountQuantityMeal(out string exception, DateTime date, int meal_time_id, int category_id, string computer)
        {
            string query = $"Select Count(*) from [card_swipe_history] Where [meal_date] = '{date.ToString("yyyy-MM-dd")}' and [meal_time_id] = {meal_time_id} and [category_id] = {category_id} and [device] = '{computer}' and result > 0;";
            var data = SQLService.Method.ExecuteScalar(out exception, SQLService.Server.SV68_HRM, query);
            if (string.IsNullOrEmpty(exception))
            {
                MealCount = int.Parse(data.ToString());
            }
        }
        /// <summary>
        /// Lưu lịch sử quẹt thẻ
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="employee_code"></param>
        /// <param name="meal_time_id"></param>
        /// <param name="computer"></param>
        /// <param name="result"></param>
        bool SaveInfoCardSwipe(out string exception, DateTime date, int meal_time_id, int category_id, string employee_code, string computer, int result)
        {
            var builder = new StringBuilder();
            string query_insert = $"Insert Into [card_swipe_history]([meal_date],[meal_time_id],[category_id],[employee_code],[swipe_time],[device],[result]) Values('{date.ToString("yyyy-MM-dd")}',{meal_time_id},{category_id},'{employee_code}',GetDate(),'{computer}',{result});";
            builder.AppendLine(query_insert);

            if (result == 1)
            {
                string query_update = $"Update [daily_meals] set [status] = {result}, [distribution_at] = GetDate(),[distribution_by] = '{computer}' Where [employee_code] = '{employee_code}' and [meal_date] = '{date.ToString("yyyy-MM-dd")}' And [meal_time_id] = {meal_time_id};";
                builder.AppendLine(query_update);
            }

            var res = SQLService.Method.ExecuteNonQuery(out exception, SQLService.Server.SV68_HRM, builder.ToString());
            return res > 0;
        }
        /// <summary>
        /// Lấy ra thông tin của giờ ăn hiện tại
        /// </summary>
        /// <param name="exception"></param>
        Meal_time GetMealTime(out string exception)
        {
            string query = "Exec usp_get_meal"; //Lấy ra thông tin của giờ ăn hiện tại
            var data = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV68_HRM, query);
            if (string.IsNullOrEmpty(exception))
            {
                if (data != null && data.Rows.Count > 0)
                {
                    Meal_time meal = new Meal_time(data.Rows[0], true);
                    return meal;
                }
            }
            return null;
        }
        void CheckRealTimeServer(out string exception)
        {
            string query = $"Exec USP_GetLastCardSwipe '{DeviceName}'"; //Lấy ra dữ liệu bản ghi cuối cùng.
            var data = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV34_AC, query);
            if (string.IsNullOrEmpty(exception))
            {
                if (data != null && data.Rows.Count > 0)
                {

                    int ID = data.Rows[0].Field<int>("EVTLGUID");
                    if (string.IsNullOrEmpty(TableName) && EVTGUID == 0)
                    {
                        TableName = data.TableName;
                        EVTGUID = ID;
                    }
                    else
                    {
                        if (TableName != data.TableName || EVTGUID != ID) //Kiểm tra nếu có dữ liệu mới trên server
                        {
                            string USRID = data.Rows[0].Field<string>("USRID"); //Mã nhân viên vừa được quẹt thẻ (Chỉ số)
                            CheckInfoMealEmployee(USRID);
                            TableName = data.TableName;
                            EVTGUID = ID;
                        }
                    }
                }
            }
            else
            {

            }
        }
        //BitmapImage ConvertImage(MemoryStream memoryStream)
        //{
        //    BitmapImage Bitmap = new BitmapImage();
        //    if (memoryStream != null)
        //    {
        //        memoryStream.Position = 0; // Đặt lại vị trí đọc của stream
        //        Bitmap.BeginInit();
        //        Bitmap.StreamSource = memoryStream;
        //        Bitmap.CacheOption = BitmapCacheOption.OnLoad;
        //        Bitmap.EndInit();
        //    }            
        //    return Bitmap;
        //}
        public void CheckInfoMealEmployee(string employee_number)
        {
            try
            {
                //Reset về mặc định
                EmployeeCode = "";
                EmployeeName = "";
                NotifyBrush = Brushes.Black;
                NotifyContent = "";
                Avata = null;
                //Kiểm tra dữ liệu staffno
                if (int.TryParse(employee_number, out _)) //Kiểm tra mã nhân viên có phải là kiểu số không.
                {
                    //Lấy ra thông tin của công nhân viên
                    string exception = null;
                    DataTable data_InfoEmployee = null;
                    string imagePath = null;
                    InfoEmployeeSwipeCard newitem = null;
                    //Kiểm tra trường hợp với mã nhân viên
                    if (employee_number.Trim().Substring(0, 1) == "3") //Với mã của nhà thầu, khách hàng bắt đầu từ 3
                    {
                        string query_InfoEmployee = $"SELECT employee_id ID,[employee_code] [EmployeeCode], [full_name] FullName FROM [employees] Where SUBSTRING([employee_code], 3, LEN([employee_code]) - 2) = '{employee_number}';";
                        data_InfoEmployee = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV68_HRM, query_InfoEmployee);

                    }
                    else //Công nhân viên MMCV trên hệ thống EZ
                    {
                        string query_InfoEmployee = $"SELECT [ID],[EmployeeCode],[FullName] FROM [tbHR_Employee] Where SUBSTRING([EmployeeCode], 3, LEN([EmployeeCode]) - 2) = '{employee_number}';";
                        data_InfoEmployee = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV91_EZ_MEKTEC, query_InfoEmployee);
                    }

                    if (string.IsNullOrEmpty(exception))
                    {
                        if (data_InfoEmployee != null && data_InfoEmployee.Rows.Count > 0)
                        {
                            EmployeeCode = data_InfoEmployee.Rows[0].Field<string>("EmployeeCode");
                            EmployeeName = data_InfoEmployee.Rows[0].Field<string>("FullName");
                            if (employee_number.Trim().Substring(0, 1) == "3") imagePath = null;
                            else imagePath = (new EZService()).GetImagePathEmployee(out exception, EmployeeCode);

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                var imageConvert = (new EZService()).GetImageEmployee(out exception, imagePath);
                                newitem = new InfoEmployeeSwipeCard(imageConvert, EmployeeCode, EmployeeName, DateTime.Now);
                                Avata = imageConvert;
                                EmployeeSwipeCards.Add(newitem);
                                if (EmployeeSwipeCards.Count > 3) EmployeeSwipeCards.RemoveAt(0);
                            });

                            //Lấy bữa ăn hiện tại
                            var SelectedMealTime = GetMealTime(out exception);
                            if (string.IsNullOrEmpty(exception))
                            {
                                if (SelectedMealTime != null) //Kiểm tra phát suất ăn bữa nào.
                                {
                                    int id_meal_time = SelectedMealTime.Id;
                                    int plus_day = SelectedMealTime.Plus_day;
                                    var meal_date = (DateTime)SelectedMealTime.Meal_date;

                                    //Lấy dữ liệu nhân viên đã quẹt thẻ trước đó chưa
                                    string query_history = $"Select * from [card_swipe_history] Where meal_date = '{meal_date.ToString("yyyy-MM-dd")}' and meal_time_id = '{id_meal_time}' and employee_code = '{EmployeeCode}' and result > 0;";
                                    var data_history = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV68_HRM, query_history);
                                    if (string.IsNullOrEmpty(exception))
                                    {
                                        //Kiểm tra nhân viên đã đc phát suất ăn chưa
                                        bool check_card_swipe = data_history.AsEnumerable().Any(f => f.Field<int>("result") > 1);
                                        if (check_card_swipe)
                                        {
                                            var row = data_history.AsEnumerable().Where(f => f.Field<int>("result") > 1).First();
                                            MediaServices.PlaySound(MediaServices.SoundNG);
                                            ResultStatus = "NG";
                                            NotifyBrush = Brushes.Red;
                                            NotifyContent = $"Đã ăn lúc {row.Field<DateTime>("swipe_time").ToString("yyyy-MM-dd HH:mm:ss")}.";
                                            SaveInfoCardSwipe(out exception, meal_date, id_meal_time, 1, EmployeeCode, ComputerName, 0);
                                        }
                                        else
                                        {
                                            string query = $"EXEC usp_get_employee_registed '{employee_number}','{meal_date.ToString("yyyy-MM-dd")}',{id_meal_time};";
                                            var data = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV68_HRM, query);
                                            if (string.IsNullOrEmpty(exception))
                                            {
                                                if (data != null && data.Rows.Count > 0)
                                                {
                                                    if (data.Rows.Count == 1)
                                                    {
                                                        long id_meal = data.Rows[0].Field<long>("id");
                                                        int status = data.Rows[0].Field<int>("status");
                                                        int categoryid = data.Rows[0].Field<int>("category_id");

                                                        if (categoryid == 1)
                                                        {
                                                            DateTime? distribution_at = data.Rows[0].Field<DateTime?>("distribution_at");
                                                            //Update công nhân viên đã được phát suất ăn
                                                            if (status == 0)
                                                            {

                                                                var res = SaveInfoCardSwipe(out exception, meal_date, id_meal_time, 1, EmployeeCode, ComputerName, 1);
                                                                if (string.IsNullOrEmpty(exception))
                                                                {
                                                                    MediaServices.PlaySound(MediaServices.SoundOK);
                                                                    NotifyContent = "Cung cấp suất ăn.";
                                                                    NotifyBrush = Brushes.Blue;
                                                                    CountQuantityMeal(out exception, meal_date, id_meal_time, 1, ComputerName);
                                                                    ResultStatus = "OK";

                                                                }
                                                                else
                                                                {
                                                                    MediaServices.PlaySound(MediaServices.SoundNG);
                                                                    ResultStatus = "NG";
                                                                    NotifyBrush = Brushes.Red;
                                                                    NotifyContent = exception;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                MediaServices.PlaySound(MediaServices.SoundNG);
                                                                ResultStatus = "NG";
                                                                NotifyBrush = Brushes.Red;
                                                                NotifyContent = $"Đã ăn lúc {distribution_at?.ToString("yyyy-MM-dd HH:mm:ss")}.";
                                                                SaveInfoCardSwipe(out exception, meal_date, id_meal_time, 1, EmployeeCode, ComputerName, 0);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            MediaServices.PlaySound(MediaServices.SoundNG);
                                                            ResultStatus = "NG";
                                                            NotifyContent = $"{EmployeeCode} không đăng ký ăn cơm mà đăng ký BMS.";
                                                            NotifyBrush = Brushes.Red;
                                                            SaveInfoCardSwipe(out exception, meal_date, id_meal_time, 1, EmployeeCode, ComputerName, 0);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MediaServices.PlaySound(MediaServices.SoundNG);
                                                        ResultStatus = "NG";
                                                        NotifyContent = "Vui lòng liên lạc nhân sự kiểm tra thông tin bất thường.\nTìm thấy nhiều hơn một dữ liệu đăng ký suất ăn.";
                                                        NotifyBrush = Brushes.Red;
                                                    }
                                                }
                                                else
                                                {

                                                    var res = SaveInfoCardSwipe(out exception, meal_date, id_meal_time, 1, EmployeeCode, ComputerName, 2);
                                                    if (string.IsNullOrEmpty(exception))
                                                    {
                                                        MediaServices.PlaySound(MediaServices.SoundOK);
                                                        ResultStatus = "OK";
                                                        NotifyContent = "Cung cấp suất ăn.";
                                                        NotifyBrush = Brushes.Blue;
                                                        CountQuantityMeal(out exception, meal_date, id_meal_time, 1, ComputerName);
                                                    }
                                                    else
                                                    {
                                                        MediaServices.PlaySound(MediaServices.SoundNG);
                                                        ResultStatus = "NG";
                                                        NotifyContent = exception;
                                                        NotifyBrush = Brushes.Red;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                MediaServices.PlaySound(MediaServices.SoundNG);
                                                ResultStatus = "NG";
                                                NotifyBrush = Brushes.Red;
                                                NotifyContent = exception;
                                            }
                                        }

                                    }
                                    else
                                    {
                                        MediaServices.PlaySound(MediaServices.SoundNG);
                                        ResultStatus = "NG";
                                        NotifyBrush = Brushes.Red;
                                        NotifyContent = exception;
                                    }
                                }
                                else
                                {
                                    MediaServices.PlaySound(MediaServices.SoundNG);
                                    ResultStatus = "NG";
                                    NotifyBrush = Brushes.Red;
                                    NotifyContent = "Chưa đến giờ cung cấp suất ăn.";
                                }
                            }
                            else
                            {
                                MediaServices.PlaySound(MediaServices.SoundNG);
                                ResultStatus = "NG";
                                NotifyBrush = Brushes.Red;
                                NotifyContent = exception;
                            }
                            //Ghi nội dung ra thông báo
                            newitem.Result = ResultStatus;
                            newitem.Notification = NotifyContent;

                        }
                        else
                        {
                            MediaServices.PlaySound(MediaServices.SoundNG);
                            ResultStatus = "NG";
                            NotifyBrush = Brushes.Red;
                            NotifyContent = $"Không lấy được thông tin công nhân viên từ mã '{employee_number}'.";
                        }
                    }
                    else
                    {
                        MediaServices.PlaySound(MediaServices.SoundNG);
                        ResultStatus = "NG";
                        NotifyBrush = Brushes.Red;
                        NotifyContent = exception;
                    }
                }
            }
            catch (Exception ex)
            {
                MediaServices.PlaySound(MediaServices.SoundNG);
                ResultStatus = "NG";
                NotifyBrush = Brushes.Red;
                NotifyContent = ex.Message;
            }

        }     
    }

    

    public class InfoEmployeeSwipeCard : INotifyPropertyChanged
    {
        BitmapImage avata;
        string employeeCode;
        string employeeName;
        DateTime enterTime;
        string result;
        string notification;

        public BitmapImage Avata { get => avata; set { avata = value;} }
        public string EmployeeCode { get => employeeCode; set => employeeCode = value; }
        public string EmployeeName { get => employeeName; set => employeeName = value; }
        public DateTime EnterTime { get => enterTime; set => enterTime = value; }
        public string Notification { get => notification; set { notification = value; OnPropertyChanged(nameof(Notification)); } }
        public string Result { get => result; set { result = value; OnPropertyChanged(nameof(Result)); } }

        public InfoEmployeeSwipeCard(BitmapImage imageConvert, string EmployeeCode, string EmployeeName, DateTime EnterTime) 
        {
            this.Avata = imageConvert; 
            this.EmployeeCode = EmployeeCode;
            this.EmployeeName = EmployeeName;
            this.EnterTime = EnterTime;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
