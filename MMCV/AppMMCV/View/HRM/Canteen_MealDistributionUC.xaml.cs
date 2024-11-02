using AppMMCV.Services;
using LibraryHelper.Methord;
using LibraryHelper.Models.HRM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppMMCV.View.HRM
{
    /// <summary>
    /// Interaction logic for Canteen_MealDistributionUC.xaml
    /// </summary>
    public partial class Canteen_MealDistributionUC : UserControl
    {
        private Meal_time SelectedMealTime;
        public Canteen_MealDistributionUC()
        {
            InitializeComponent();
            CreatOptionMenu();
        }

        private void txt_cardID_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if ( sender is TextBox tbx)
            //{

            //}
        }
        void CreatOptionMenu()
        {
            string query = "Select * from [meal_time];";
            var data = SQLService.Method.ExecuteQuery(out string exception,SQLService.Server.SV68_HRM, query);
            if (string.IsNullOrEmpty(exception))
            {
                foreach (DataRow row in data.Rows)
                {
                    var item = new Meal_time(row);
                    var btn = new Button() { Tag = item, FontSize = 30, Content = item.Meal, Margin = new Thickness(20), Width = 200, Height = 200 };
                    btn.Click += Btn_Click_SelectMealTime;
                    ufg_menu.Children.Add(btn);
                }
            }
            else
            {
                MessageBox.Show(exception);
            }
        }

        private void Btn_Click_SelectMealTime(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Meal_time item)
            {
                SelectedMealTime = item;
                dlh_main.IsOpen = false;
            }
        }

        void CheckInfoMealEmployee(string staffno)
        {
            if (int.TryParse(staffno, out _)) //Kiểm tra mã nhân viên có phải là kiểu số không.
            {
                if (SelectedMealTime != null) //Kiểm tra phát suất ăn bữa nào.
                {
                    int id_meal_time = SelectedMealTime.Id;
                    int plus_day = SelectedMealTime.Plus_day;                    
                    string query = $"EXEC usp_get_employee_registed '{staffno}',{plus_day},{id_meal_time};";
                    var data = SQLService.Method.ExecuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
                    if (string.IsNullOrEmpty(exception))
                    {
                        if (data != null && data.Rows.Count > 0)
                        {
                            if (data.Rows.Count == 1)
                            {
                                long id_meal = data.Rows[0].Field<long>("id");
                                int status = data.Rows[0].Field<int>("status");                                
                                DateTime ? distribution_at = data.Rows[0].Field<DateTime?>("distribution_at");
                                //Update công nhân viên đã được phát suất ăn
                                if (status == 0)
                                {
                                    string query_update = $"Update [daily_meals] set [status] = 1, [distribution_at] = GetDate() Where [id] = {id_meal};";
                                    var res = SQLService.Method.ExecuteNonQuery(out exception,SQLService.Server.SV68_HRM,query_update);
                                    if (string.IsNullOrEmpty(exception))
                                    {
                                        if (res > 0) Notification("Hãy phát suất ăn.", Brushes.Blue);
                                        else
                                        {
                                            Notification("Hãy phát suất ăn.", Brushes.Blue);
                                        }
                                    }
                                    else
                                    {
                                        Notification(exception, Brushes.Red);
                                    }                                    
                                }
                                else
                                {
                                    Notification($"Nhân viên này đã được phát suất ăn trước đó lúc {distribution_at?.ToString("yyyy-MM-dd HH:mm:ss")}.", Brushes.Red);
                                }                                
                            }
                            else
                            {
                                Notification("Vui lòng liên lạc nhân sự kiểm tra thông tin bất thường.\nTìm thấy nhiều hơn một dữ liệu đăng ký suất ăn.", Brushes.Red);
                            }
                        }
                        else
                        {
                            Notification("Nhân viên không đăng ký suất ăn cho bữa này.", Brushes.Red);
                        }
                    }
                    else
                    {
                        Notification(exception, Brushes.Red);
                    }
                }
                else
                {
                    Notification("Không xác định được bữa ăn, vui lòng khởi đông lại chức năng này.",Brushes.Red);
                }                
            }
        }
        void Notification(string content, Brush color)
        {
            txt_notification.Text = content;
            txt_notification.Foreground = color;
        }
        int ConvertToSN(int card_number, int bit = 128)
        {
            int _byte = bit / 8;
            string binaryBit = Convert.ToString(bit, 2).PadLeft(_byte, '0');
            string binaryCard = Convert.ToString(card_number, 2).PadLeft(_byte, '0');
            string binary = binaryBit + binaryCard;
            return Convert.ToInt32(binary, 2);
        }
        private void txt_cardID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && sender is TextBox tbx)
            {
                CheckInfoMealEmployee(tbx.Text);
            }
        }
    }
}
