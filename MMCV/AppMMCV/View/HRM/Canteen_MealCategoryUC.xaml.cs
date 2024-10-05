using AppMMCV.Services;
using LibraryHelper.Models.HRM;
using System;
using System.Collections;
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
	/// Interaction logic for Canteen_MealCategoryUC.xaml
	/// </summary>
	public partial class Canteen_MealCategoryUC : UserControl
	{
		//Khai báo đăng ký một DependencyProperty
		public static readonly DependencyProperty DataMealCategoryProperty = DependencyProperty.Register("DataMealCategory", typeof(ObservableCollection<Meal_category>), typeof(Canteen_MealCategoryUC), new PropertyMetadata(new ObservableCollection<Meal_category>()));
		public static readonly DependencyProperty IsRightOpenDrawerProperty = DependencyProperty.Register("IsRightOpenDrawer", typeof(bool), typeof(Canteen_MealCategoryUC), null);
		public static readonly DependencyProperty SubmitTypeProperty = DependencyProperty.Register("SubmitType", typeof(string), typeof(Canteen_MealCategoryUC), null);
		public static readonly DependencyProperty CategoryIdProperty = DependencyProperty.Register("CategoryId", typeof(int), typeof(Canteen_MealCategoryUC), null);
		public static readonly DependencyProperty CategoryDescriptionProperty = DependencyProperty.Register("CategoryDescription", typeof(string), typeof(Canteen_MealCategoryUC), null);
		public static readonly DependencyProperty CategoryNameProperty = DependencyProperty.Register("CategoryName", typeof(string), typeof(Canteen_MealCategoryUC), null);
		//Tạo một property wrapper để truy cập DependencyProperty
		public ObservableCollection<Meal_category> DataMealCategory { get { return (ObservableCollection<Meal_category>)GetValue(DataMealCategoryProperty); } set { SetValue(DataMealCategoryProperty, value); } }
		public bool IsRightOpenDrawer { get { return (bool)GetValue(IsRightOpenDrawerProperty); } set { SetValue(IsRightOpenDrawerProperty, value); } }
		public string SubmitType { get { return (string)GetValue(SubmitTypeProperty); } set { SetValue(SubmitTypeProperty, value); } }
		public int CategoryId { get { return (int)GetValue(CategoryIdProperty); } set { SetValue(CategoryIdProperty, value); } }
		public string CategoryDescription { get { return (string)GetValue(CategoryDescriptionProperty); } set { SetValue(CategoryDescriptionProperty, value); } }
		public string CategoryName { get { return (string)GetValue(CategoryNameProperty); } set { SetValue(CategoryNameProperty, value); } }
		public Canteen_MealCategoryUC()
		{
			InitializeComponent();
			this.DataContext = this;
			Load_MealCategory();
		}

		void Load_MealCategory()
		{
			DataMealCategory.Clear();
			string query = "Select * from meal_category with(nolock) order by category_id;";
			var data = SQLService.Method.ExecuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
			if (string.IsNullOrEmpty(exception))
			{
				if (data != null && data.Rows.Count > 0) foreach (DataRow row in data.Rows) DataMealCategory.Add(new Meal_category(row));
			}
			else
			{
				MessageBox.Show(exception);
			}
		}

		void SetInputValue(Meal_category item = null)
		{
			if (item == null)
			{
				SubmitType = "Add item";
				CategoryId = 0;
				CategoryName = null;
				CategoryDescription = null;
			}
			else
			{
				SubmitType = "Edit item";
				CategoryId = item.Category_id;
				CategoryName = item.Category_name;
				CategoryDescription = item.Category_description;
			}
		}

		private void Event_Btn_Click_Submit(object sender, RoutedEventArgs e)
		{
			ExecuteSubmit();
		}

		private void Event_Btn_Click_Close(object sender, RoutedEventArgs e)
		{
			IsRightOpenDrawer = false;
		}

		private void Event_Btn_Click_Add(object sender, RoutedEventArgs e)
		{
			SetInputValue();
			IsRightOpenDrawer = true;
		}

		void ExecuteSubmit()
		{
			if (string.IsNullOrEmpty(CategoryName) || string.IsNullOrEmpty(CategoryDescription))
			{
				MessageBox.Show("Please enter value.");
			}
			else
			{
				string query;
				string update_by = DataService.UserInfo.username;
				var parameter = new List<object>() { CategoryName, CategoryDescription, update_by };
				switch (SubmitType)
				{

					case "Add item":
						query = "Insert Into [meal_category] ([category_name],[category_description],[update_at],[update_by]) " +
							"values ( @category_name , @category_description , GetDate() , @update_by )";
						break;
					case "Edit item":
						query = "Update [meal_category] set [category_name] = @category_name ,[category_description] =  @category_description ," +
							"[update_at] = GetDate() ,[update_by] = @update_by Where [category_id] = @category_id ;";
						parameter.Add(CategoryId);
						break;
					default:
						query = string.Empty;
						break;
				}
				if (!string.IsNullOrEmpty(query))
				{
					var res = SQLService.Method.ExecuteNonQuery(out string exception, SQLService.Server.SV68_HRM, query, parameter.ToArray());
					if (string.IsNullOrEmpty(exception))
					{
						if (res > 0)
						{
							Load_MealCategory();
							MessageBox.Show("Update completed.");
							IsRightOpenDrawer = false;
						}
						else
						{
							MessageBox.Show("Fail.");
						}
					}
					else
					{
						MessageBox.Show(exception);
					}
				}
			}
		}

		private void Event_Btn_Click_Edit(object sender, RoutedEventArgs e)
		{
			if (dtg_data.SelectedItem is Meal_category item)
			{
				SetInputValue(item);
				IsRightOpenDrawer = true;
			}
		}
	}
}
