using AppMMCV.Services;
using AppMMCV.View.Systems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppMMCV.ViewModels.Systems
{
	internal class LoginVM : INotifyPropertyChanged
	{
		public LoginVM()
		{
			LoginCommand = new RelayCommand(ExecuteLogin);
		}
		
		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public ICommand LoginCommand { get; }
		private string username;
		private string password;
		private string statusMessage;

		public string Username { get => username; set { username = value; OnPropertyChanged(nameof(Username)); } }
		public string Password { get => password; set { password = value; OnPropertyChanged(nameof(Password)); } }
		public string StatusMessage { get => statusMessage; set { statusMessage = value; OnPropertyChanged(nameof(StatusMessage)); } }
		private void ExecuteLogin()
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
						DataService.GlobalVM.SelectUsercontrol = new MainMenuUC();
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
