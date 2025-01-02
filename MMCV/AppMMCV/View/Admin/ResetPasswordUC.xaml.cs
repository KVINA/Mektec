using AppMMCV.Services;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppMMCV.View.Admin
{
    /// <summary>
    /// Interaction logic for ResetPasswordUC.xaml
    /// </summary>
    public partial class ResetPasswordUC : UserControl
    {
        public ResetPasswordUC()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var username = txt_username.Text.Trim();
            if (!string.IsNullOrEmpty(username))
            {
                string query = $"Update [CANTEEN].[dbo].[users] Set password = 'mmcv123' where username = '{username}';";
                var res = SQLService.Method.ExecuteNonQuery(out string exception,SQLService.Server.SV68_HRM,query);
                if (string.IsNullOrEmpty(exception))
                {
                    if (res > 0)
                    {
                        MessageBox.Show("Reset password thành công.");
                    }
                    else
                    {
                        MessageBox.Show("Reset password không thành công.");
                    }
                }
                else
                {
                    MessageBox.Show($"ERROR:\n{exception}");
                }
            }
            else
            {
                MessageBox.Show("Chưa nhập vào tài khoản cần reset.");
            }
        }
    }
}
