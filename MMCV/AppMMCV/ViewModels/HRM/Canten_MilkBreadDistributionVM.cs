using AppMMCV.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace AppMMCV.ViewModels.HRM
{
    public class Canten_MilkBreadDistributionVM : INotifyPropertyChanged
    {
        int ConvertToSN(int card_number, int bit = 128)
        {
            int _byte = bit / 8;
            string binaryBit = Convert.ToString(bit, 2).PadLeft(_byte, '0');
            string binaryCard = Convert.ToString(card_number, 2).PadLeft(_byte, '0');
            string binary = binaryBit + binaryCard;
            return Convert.ToInt32(binary, 2);
        }

        private BitmapImage avata;
        string result;
        MilkBreadInfo breadInfo;
        bool isAuto;
        bool isLoading;
        string notifiContent;
        Brush notifiBrush = Brushes.Red;
        DateTime selectedDate = DateTime.Now.Date;
        string cardID;

        public string Result { get => result; set { result = value; OnPropertyChanged(nameof(Result)); } }
        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }
        public string NotifiContent { get => notifiContent; set { notifiContent = value; OnPropertyChanged(nameof(NotifiContent)); } }
        public Brush NotifiBrush { get => notifiBrush; set { notifiBrush = value; OnPropertyChanged(nameof(NotifiBrush)); } }
        public DateTime SelectedDate { get => selectedDate; set { selectedDate = value; OnPropertyChanged(nameof(SelectedDate)); GET_CardInfor(); } }
        public string CardID { get => cardID; set { cardID = value; OnPropertyChanged(nameof(CardID)); GET_CardInfor(); } }
        public MilkBreadInfo BreadInfo { get => breadInfo; set { breadInfo = value; OnPropertyChanged(nameof(BreadInfo)); } }

        public ICommand Command_ExecuteConfirmMilkBreak { get; }
        public bool IsAuto { get => isAuto; set => isAuto = value; }
        public BitmapImage Avata { get => avata; set { avata = value; OnPropertyChanged(nameof(Avata)); } }

        public Canten_MilkBreadDistributionVM()
        {
            IsAuto = true;
            Command_ExecuteConfirmMilkBreak = new RelayCommand(ConfirmMilkBreak);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        void ConfirmMilkBreak()
        {

            if (BreadInfo != null)
            {
                if (!string.IsNullOrEmpty(BreadInfo.EmployeeCode))
                {
                    if (BreadInfo.Quantity1 > 0 || BreadInfo.Quantity2 > 0)
                    {
                        string update_by = DataService.UserInfo.username;
                        var builder = new StringBuilder();
                        string computer = Environment.MachineName.Length <= 10 ? Environment.MachineName : Environment.MachineName.Substring(Environment.MachineName.Length - 10, 10);

                        builder.AppendLine($"Insert Into [card_swipe_history](meal_date,meal_time_id,category_id,employee_code,swipe_time,device,result) Select meal_date,meal_time_id,category_id,employee_code,GETDATE() 'swipe_time','{computer}' 'device',1 'result' from daily_meals Where meal_date = '{SelectedDate.ToString("yyyy-MM-dd")}' And category_id in (2,3) And employee_code = '{BreadInfo.EmployeeCode}' And status = 0 And is_deleted = 0;");

                        builder.AppendLine($"Update [daily_meals] Set status = 1, distribution_by = '{update_by}', distribution_at = GetDate() Where meal_date = '{SelectedDate.ToString("yyyy-MM-dd")}' And category_id in (2,3) And employee_code = '{BreadInfo.EmployeeCode}' And status = 0 And is_deleted = 0;");

                        SQLService.Method.ExecuteNonTrans(out string exception, SQLService.Server.SV68_HRM, builder.ToString());
                        if (string.IsNullOrEmpty(exception))
                        {
                            //BreadInfo.QuantityAc1 += BreadInfo.Quantity1;
                            //BreadInfo.Quantity1 = 0;
                            //BreadInfo.QuantityAc2 += BreadInfo.Quantity2;
                            //BreadInfo.Quantity2 = 0;
                            //NotifiBrush = Brushes.Blue;
                            //NotifiContent = $"Đã phát xong suất BMS cho {BreadInfo.EmployeeName}";
                        }
                        else
                        {
                            MediaServices.PlaySound(MediaServices.SoundNG);
                            NotifiBrush = Brushes.Red;
                            NotifiContent = exception;
                        }
                    }
                    else
                    {
                        MediaServices.PlaySound(MediaServices.SoundNG);
                        NotifiBrush = Brushes.Red;
                        NotifiContent = "Nhân viên đã được phát hết suất BMS trước đó.";
                    }
                }
            }
        }

        void GET_CardInfor()
        {
            Avata = null;
            BreadInfo = null;
            Result = "";
            NotifiContent = "";
            if (IsAuto) CheckCardID();
            else CheckEmployeeCode(CardID);
        }

        void CheckCardID()
        {
            if (!string.IsNullOrEmpty(CardID))
            {
                string exception;
                var Tem = CardID.Split('-');
                if (Tem.Length == 2)
                {
                    //Check số Bit
                    if (!int.TryParse(Tem[0], out int bit))
                    {
                        exception = "Không convert được số Bit - Định dạng thẻ không hợp lệ";
                        goto THE_EXCEPTION;
                    }

                    //Check số thẻ
                    if (int.TryParse(Tem[1], out int id))
                    {
                        if (bit > 0)
                        {
                            var SN = ConvertToSN(id, bit);
                            string query = $"Select A.USRID from [T_USR] A with(NoLock) Inner Join [T_USRCRD] B with(NoLock) On A.USRUID = B.USRUID Inner Join [T_CRD] C with(NoLock) On B.CRDUID = C.CRDUID Where C.CRDCSN = {SN};";
                            var USRID = SQLService.Method.ExecuteScalar(out exception, SQLService.Server.SV34_AC, query);
                            if (!string.IsNullOrEmpty(exception)) goto THE_EXCEPTION;
                            if (USRID != null) //Nếu lấy được mã nhân viên từ SN
                            {
                                CheckEmployeeCode(USRID);
                            }
                            else
                            {
                                MediaServices.PlaySound(MediaServices.SoundNG);
                                Result = "NG";
                                NotifiBrush = Brushes.Red;
                                NotifiContent = "Không lấy được Mã nhân viên từ CardID.";
                            }
                        }
                        else
                        {
                            exception = "Không convert được số Bit - Định dạng thẻ không hợp lệ";
                            goto THE_EXCEPTION;
                        }
                    }
                    else
                    {
                        MediaServices.PlaySound(MediaServices.SoundNG);
                        Result = "NG";
                        NotifiBrush = Brushes.Red;
                        NotifiContent = "CardID không hợp lệ.";
                    }
                }
                else
                {
                    MediaServices.PlaySound(MediaServices.SoundNG);
                    Result = "NG";
                    NotifiContent = "CardID không hợp lê.";
                    NotifiBrush = Brushes.Red;
                }
                return;
            THE_EXCEPTION:
                MediaServices.PlaySound(MediaServices.SoundNG);
                Result = "NG";
                NotifiContent = exception;
                NotifiBrush = Brushes.Red;
            }
        }
        //int ConvertBit(int number)
        //{
        //    switch (number)
        //    {
        //        case 128:
        //            return 128;
        //        case 264:
        //            return 8;
        //        case 274:
        //            return 18;
        //        case 8:
        //            return 8;
        //        default :
        //            return 0;
        //    }
        //}
        void CheckEmployeeCode(object USRID)
        {
            string exception = "";
            if (int.TryParse(USRID.ToString(), out _)) //Kiểm tra mã nhân viên có phải là kiểu số không.
            {
                string staffno = USRID.ToString();
                //Lấy ra thông tin của công nhân viên
                string query_InfoEmployee = $"SELECT [ID],[EmployeeCode],[FullName] FROM [tbHR_Employee] Where SUBSTRING([EmployeeCode], 3, LEN([EmployeeCode]) - 2) = '{staffno}';";
                var data_InfoEmployee = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV91_EZ_MEKTEC, query_InfoEmployee);
                if (!string.IsNullOrEmpty(exception)) goto THE_EXCEPTION;
                if (data_InfoEmployee != null && data_InfoEmployee.Rows.Count > 0)
                {


                    BreadInfo = new MilkBreadInfo();
                    BreadInfo.EmployeeCode = data_InfoEmployee.Rows[0].Field<string>("EmployeeCode");
                    BreadInfo.EmployeeName = data_InfoEmployee.Rows[0].Field<string>("FullName");
                    BreadInfo.Quantity1 = 0;
                    BreadInfo.Quantity2 = 0;
                    BreadInfo.Quantity3 = 0;
                    BreadInfo.QuantityAc1 = 0;
                    BreadInfo.QuantityAc2 = 0;
                    BreadInfo.QuantityAc3 = 0;

                    //Lấy ảnh nhân viên
                    var imagePath = (new EZService()).GetImagePathEmployee(out exception, BreadInfo.EmployeeCode);
                    Avata = (new EZService()).GetImageEmployee(out exception, imagePath);

                    //Đi lấy số lượng phiêu bánh mì 1:1 và 2:2 trong ngày:
                    string query_break = $"Select category_id,status,Count(*) 'quantity' from [daily_meals] Where meal_date = '{SelectedDate.ToString("yyyy-MM-dd")}' and employee_code = '{BreadInfo.EmployeeCode}' and is_deleted = 0 and category_id in (2,3,4) and [is_deleted] = 0 Group by category_id,status";
                    var data_break = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV68_HRM, query_break);
                    if (!string.IsNullOrEmpty(exception)) goto THE_EXCEPTION;
                    if (data_break != null && data_break.Rows.Count > 0)
                    {
                        foreach (DataRow row in data_break.Rows)
                        {
                            var category_id = row.Field<int>("category_id");
                            var status = row.Field<int>("status");


                            if (status > 0)
                            {
                                switch (category_id)
                                {
                                    case 2://BMS 2:2
                                        BreadInfo.QuantityAc1 = row.Field<int>("quantity");
                                        break;
                                    case 3://BMS 1:1
                                        BreadInfo.QuantityAc2 = row.Field<int>("quantity");
                                        break;
                                    case 4://BMS 4:4
                                        BreadInfo.QuantityAc3 = row.Field<int>("quantity");
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                switch (category_id)
                                {
                                    case 2://BMS 2:2
                                        BreadInfo.Quantity1 = row.Field<int>("quantity");
                                        break;
                                    case 3://BMS 1:1
                                        BreadInfo.Quantity2 = row.Field<int>("quantity");
                                        break;
                                    case 4://BMS 4:4
                                        BreadInfo.Quantity3 = row.Field<int>("quantity");
                                        break;
                                    default:
                                        break;
                                }
                            }

                        }

                        //Kiểm tra xem đã được phát bánh mỳ sữa chưa

                        if (breadInfo.Quantity1 > 0 || breadInfo.Quantity2 > 0)
                        {
                            MediaServices.PlaySound(MediaServices.SoundOK);
                            Result = "OK";
                            NotifiBrush = Brushes.Blue;
                            NotifiContent = "Hãy phát suất ăn phụ.";
                            ConfirmMilkBreak();
                        }
                        else
                        {
                            MediaServices.PlaySound(MediaServices.SoundNG);
                            Result = "NG";
                            NotifiBrush = Brushes.Red;
                            NotifiContent = "Đã phát hết suất BMS.";
                        }

                    }
                    else
                    {
                        MediaServices.PlaySound(MediaServices.SoundNG);
                        Result = "NG";
                        NotifiBrush = Brushes.Red;
                        NotifiContent = "Bạn không đăng ký suất ăn phụ.";

                    }
                }
                else
                {
                    MediaServices.PlaySound(MediaServices.SoundNG);
                    Result = "NG";
                    NotifiBrush = Brushes.Red;
                    NotifiContent = $"Không lấy được thông tin nhân viên trong EZ [tbHR_Employee]: {USRID.ToString()}";
                }
            }
            else
            {
                MediaServices.PlaySound(MediaServices.SoundNG);
                Result = "NG";
                NotifiBrush = Brushes.Red;
                NotifiContent = $"Mã nhân viên không hợp lệ: {USRID.ToString()}";
            }
            return;
        THE_EXCEPTION:
            MediaServices.PlaySound(MediaServices.SoundNG);
            Result = "NG";
            NotifiContent = exception;
            NotifiBrush = Brushes.Red;
        }

    }

    public class MilkBreadInfo : INotifyPropertyChanged
    {
        string employeeCode;
        string employeeName;
        int quantity1;
        int quantity2;
        int quantity3;
        int quantityAc1;
        int quantityAc2;
        int quantityAc3;

        public string EmployeeCode { get => employeeCode; set { employeeCode = value; OnPropertyChanged(nameof(EmployeeCode)); } }
        public string EmployeeName { get => employeeName; set { employeeName = value; OnPropertyChanged(nameof(EmployeeName)); } }
        public int Quantity1 { get => quantity1; set { quantity1 = value; OnPropertyChanged(nameof(Quantity1)); } }
        public int Quantity2 { get => quantity2; set { quantity2 = value; OnPropertyChanged(nameof(Quantity2)); } }
        public int QuantityAc1 { get => quantityAc1; set { quantityAc1 = value; OnPropertyChanged(nameof(QuantityAc1)); } }
        public int QuantityAc2 { get => quantityAc2; set { quantityAc2 = value; OnPropertyChanged(nameof(QuantityAc2)); } }

        public int Quantity3 { get => quantity3; set { quantity3 = value; OnPropertyChanged(nameof(Quantity3)); } }

        public int QuantityAc3 { get => quantityAc3; set { quantityAc3 = value; OnPropertyChanged(nameof(QuantityAc3)); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
