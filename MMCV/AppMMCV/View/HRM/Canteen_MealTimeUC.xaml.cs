using AppMMCV.Services;
using LibraryHelper.Models.HRM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	/// Interaction logic for Canteen_MealTimeUC.xaml
	/// </summary>
	public partial class Canteen_MealTimeUC : UserControl
	{
		//Khai báo đăng ký một DependencyProperty
		public static readonly DependencyProperty SubmitTypeProperty = DependencyProperty.Register("SubmitType", typeof(string), typeof(Canteen_MealTimeUC), null);
		public static readonly DependencyProperty IsRightDrawerOpenProperty = DependencyProperty.Register("IsRightDrawerOpen", typeof(bool), typeof(Canteen_MealTimeUC), null);
		public static readonly DependencyProperty DataMealTimeProperty = DependencyProperty.Register("DataMealTime", typeof(ObservableCollection<Meal_time>), typeof(Canteen_MealTimeUC), new PropertyMetadata(new ObservableCollection<Meal_time>()));
		//Tạo một property wrapper để truy cập DependencyProperty
		public string SubmitType { get { return (string)GetValue(SubmitTypeProperty); } set { SetValue(SubmitTypeProperty, value); } }
		public bool IsRightDrawerOpen { get { return (bool)GetValue(IsRightDrawerOpenProperty); } set { SetValue(IsRightDrawerOpenProperty, value); } }
		public ObservableCollection<Meal_time> DataMealTime { get { return (ObservableCollection<Meal_time>)GetValue(DataMealTimeProperty); } set { SetValue(DataMealTimeProperty, value); } }
		private int MealID = 0;

		public Canteen_MealTimeUC()
		{
			InitializeComponent();
			this.DataContext = this;
			Load_DataMealTiem();

		}

		private void Event_Btn_Click_OpenHost(object sender, RoutedEventArgs e)
		{
			SetInputValue();
			IsRightDrawerOpen = true;
		}

		private void Event_Btn_Click_CloseHost(object sender, RoutedEventArgs e)
		{
			IsRightDrawerOpen = false;
		}
		private void Event_Btn_Click_Submit(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(txt_Meal.Text) || string.IsNullOrEmpty(txt_PlusDate.Text)
				|| string.IsNullOrEmpty(txt_Hour.Text) || string.IsNullOrEmpty(txt_PlusDate.Text))
			{
				MessageBox.Show("Please enter value.");
			}
			else
			{
				try
				{
					string meal = txt_Meal.Text.Trim();
					DateTime startTime = txt_StartTime.SelectedTime.Value;
					int hour = int.Parse(txt_Hour.Text.Trim());
					int plusDay = int.Parse(txt_PlusDate.Text.Trim());
					DateTime update_at = DateTime.Now;
					string updator = DataService.UserInfo.username;
					string query;
					var parameter = new List<object>() { meal, startTime, hour, plusDay, update_at, updator };
					switch (SubmitType)
					{
						case "Add item":
							query = "Insert Into meal_time (meal,start_time,hour_duration,plus_day,update_at,updator) " +
								"values( @meal , @start_time , @hour_duration , @plus_day , @update_at , @updator );";
							break;
						case "Edit item":
							query = "Update meal_time set meal = @meal ,start_time = @start_time ,hour_duration = @hour_duration ," +
								"plus_day = @plus_day ,update_at = @update_at ,updator = @updator Where id = @id ;";
							parameter.Add(MealID);
							break;
						default:
							query = string.Empty;
							break;
					}
					if (!string.IsNullOrEmpty(query))
					{
						var res = SQLService.Method.ExecuteNonQuery(out string exception, SQLService.Server.SV68_HRM, query,parameter.ToArray());
						if (string.IsNullOrEmpty(exception))
						{
							if (res> 0)
							{
								Load_DataMealTiem();
								MessageBox.Show("Update completed.");
								IsRightDrawerOpen = false;
							}
							else
							{
								MessageBox.Show("Fail!");
							}
						}
						else
						{
							MessageBox.Show(exception);
						}
					}
				}
				catch (Exception ex)
				{

					MessageBox.Show(ex.Message);
				}
			}
		}
		void SetInputValue(Meal_time meal_time = null)
		{
			if (meal_time == null)
			{
				SubmitType = "Add item";
				MealID = 0;
				txt_Meal.Text = string.Empty;
				txt_StartTime.Text = string.Empty;
				txt_Hour.Text = string.Empty;
				txt_PlusDate.Text = string.Empty;
			}
			else
			{
				SubmitType = "Edit item";
				MealID = meal_time.Id;
				txt_Meal.Text = meal_time.Meal;
				txt_StartTime.Text = meal_time.Start_time.ToString(@"hh\:mm");
				txt_Hour.Text = meal_time.Hour_duration.ToString();
				txt_PlusDate.Text = meal_time.Plus_day.ToString();
			}
		}

		void Load_DataMealTiem()
		{
			DataMealTime.Clear();
			string query = "Select * from meal_time with(nolock) order by id;";
			var data = SQLService.Method.ExecuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
			if (string.IsNullOrEmpty(exception))
			{
				if (data != null && data.Rows.Count > 0)
				{
					foreach (DataRow row in data.Rows) DataMealTime.Add(new Meal_time(row));
				}
			}
			else
			{
				MessageBox.Show(exception);
			}
		}

		private void Event_Btn_Click_Edit(object sender, RoutedEventArgs e)
		{
			if (dtg_MealTime.SelectedItem is Meal_time item)
			{
				SetInputValue(item);
				IsRightDrawerOpen = true;
			}
        }
    }
}
