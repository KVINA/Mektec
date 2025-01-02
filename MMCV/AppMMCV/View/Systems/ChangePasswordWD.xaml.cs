using AppMMCV.Services;
using LibraryHelper.Models;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace AppMMCV.View.Systems
{
    /// <summary>
    /// Interaction logic for ChangePasswordWD.xaml
    /// </summary>
    public partial class ChangePasswordWD : Window
    {
        public ChangePasswordWD()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string _old = pwb_old.Password;
            string _new = pwb_new.Password;
            string _renew = pwb_renew.Password;
            if (_new == _renew)
            {
                string query_select = $"Select[password] from[users] Where username = '{DataService.UserInfo.username}';";
                var data_select = SQLService.Method.ExecuteScalar(out string exception,SQLService.Server.SV68_HRM,query_select);
                if (string.IsNullOrEmpty(exception))
                {
                    if (data_select.ToString() == _old)
                    {
                        string query_update = $"Update[users] Set password = '{_new}' where username = '{DataService.UserInfo.username}';";
                        var data_update = SQLService.Method.ExecuteNonQuery(out exception, SQLService.Server.SV68_HRM, query_update);
                        if (string.IsNullOrEmpty(exception))
                        {
                            if (data_update > 0)
                            {
                                MessageBox.Show("Đổi mật khẩu thành công.");
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Đổi mật khẩu không thành công.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Lỗi: " + exception);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Mật khẩu không chính xác.");
                    }
                }
                else
                {
                    MessageBox.Show("Lỗi: " + exception);
                }
                
            }
            else
            {
                MessageBox.Show("Mật khẩu mới không khớp nhau.");
            }
        }
    }
}
