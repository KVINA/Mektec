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

		private LoginUC loginUC = new LoginUC();
		private HomeUC homeUC = new HomeUC();

		private UserControl selectUsercontrol;

		private ParentMenuInfo parentMenuActive;
		public UserControl SelectUsercontrol { get => selectUsercontrol; set { selectUsercontrol = value; OnPropertyChanged(nameof(SelectUsercontrol));} }
        public ParentMenuInfo ParentMenuActive { get { if (parentMenuActive == null) parentMenuActive = new ParentMenuInfo(); return parentMenuActive; }
			set { parentMenuActive = value; OnPropertyChanged(nameof(ParentMenuActive)); } }
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

	public class ParentMenuInfo
	{
		public string ParentID { get; set; }
		public List<MenuItems> ListItems { get; set; }
	}
}
