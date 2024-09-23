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
		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private UserControl selectUsercontrol;

		public UserControl SelectUsercontrol { get => selectUsercontrol; set { selectUsercontrol = value; OnPropertyChanged(nameof(SelectUsercontrol));} }

		public void CheckStatusLogin()
		{
			if (Services.DataService.IsLogin)
			{
				
			}
			else
			{
				SelectUsercontrol = new LoginUC();
			}
		}
	}
}
