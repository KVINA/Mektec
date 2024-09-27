using AppMMCV.Services;
using AppMMCV.View.Admin;
using AppMMCV.ViewModels;
using AppMMCV.ViewModels.Systems;
using LibraryHelper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginUC : UserControl
    {
        #region Variable
        //Khai báo đăng ký một DependencyProperty
        public static readonly DependencyProperty StatusMessageProperty =
            DependencyProperty.Register("StatusMessage", typeof(string), typeof(UserControl), null);
        public static readonly DependencyProperty UsernameProperty =
            DependencyProperty.Register("Username", typeof(string), typeof(UserControl), null);
        public static readonly DependencyProperty PasswordProperty =
           DependencyProperty.Register("Password", typeof(string), typeof(UserControl), null);
        //Tạo một property wrapper để truy cập DependencyProperty
        public string Username
        {
            get { return (string)GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public string StatusMessage
        {
            get { return (string)GetValue(StatusMessageProperty); }
            set { SetValue(StatusMessageProperty, value); }
        }
        #endregion

        public LoginUC()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;            
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
		{
			var passwordBox = sender as PasswordBox;
			Password = passwordBox.Password;
		}
        private void Btn_Click_Login(object sender, RoutedEventArgs e)
        {
            // Logic để xác thực thông tin đăng nhập
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                StatusMessage = "Please enter username, password.";
            }
            else
            {
                AuthenServices.Login(out string exception, Username, Password);
                if (string.IsNullOrEmpty(exception))
                {
                    if (DataService.IsLogin)
                    {
                        StatusMessage = "Đăng nhập thành công!";
                        //DataService.GlobalVM.SelectUsercontrol = new HomeUC();
                        DataService.GlobalVM.SelectUsercontrol = new MenuSettings();
                    }
                    else
                    {
                        StatusMessage = "Incorrect username or password!";
                    }
                }
                else
                {
                    StatusMessage = exception;
                }
            }
        }
    }
}
