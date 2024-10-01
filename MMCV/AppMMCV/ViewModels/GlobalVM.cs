using AppMMCV.Json;
using AppMMCV.View.Admin;
using AppMMCV.View.Systems;
using LibraryHelper.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AppMMCV.ViewModels
{
	internal class GlobalVM : INotifyPropertyChanged
	{
		public GlobalVM()
		{
			CheckStatusLogin();
		}

		private int activeMenu;
		private readonly LoginUC loginUC = new LoginUC();
		private readonly HomeUC homeUC = new HomeUC();

		private UserControl selectUsercontrol;
		public UserControl SelectUsercontrol { get => selectUsercontrol; set { selectUsercontrol = value; OnPropertyChanged(nameof(SelectUsercontrol)); } }
	

		public int ActiveMenu { get => activeMenu; set { activeMenu = value; OnPropertyChanged(nameof(ActiveMenu)); } }

		public void CheckStatusLogin()
		{
			if (Services.DataService.IsLogin)
			{

			}
			else
			{
				SelectUsercontrol = loginUC;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
