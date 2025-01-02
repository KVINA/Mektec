using AppMMCV.Services;
using AppMMCV.View.Admin;
using AppMMCV.View.HRM;
using AppMMCV.View.Systems;
using LibraryHelper.Models;
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

namespace AppMMCV
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static int FromHeight = 0;
		public MainWindow()
		{
			InitializeComponent();
			string computer = Environment.MachineName.Length <= 10 ? Environment.MachineName : Environment.MachineName.Substring(Environment.MachineName.Length - 10, 10);
            if (CheckMachine(computer))
			{
				this.Content = new Canteen_MealDistributionUC();
			}
			else
			{
                this.DataContext = DataService.GlobalVM;
            }
		}

		bool CheckMachine(string machine)
		{
			string query = $"Select * from [device_distribution_meal] where [computer_name] = '{machine}'";
			var data = SQLService.Method.ExecuteQuery(out string exception,SQLService.Server.SV68_HRM,query);
			if (data != null && data.Rows.Count > 0)
			{
                DataService.UserInfo = new Users(machine, machine, 2);
                DataService.IsLogin = true;
				return true;
            }
			return false;
        }
	}
}
