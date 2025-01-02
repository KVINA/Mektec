using AppMMCV.ViewModels.HRM;
using LibraryHelper.Models.HRM;
using System;
using System.Collections.Generic;
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
	/// Interaction logic for EmployeesSubmitUC.xaml
	/// </summary>
	public partial class EmployeesSubmitUC : UserControl
	{

		public static readonly DependencyProperty TransportProperty =
			DependencyProperty.Register("Transport", typeof(DataTable), typeof(EmployeesSubmitUC));
		public DataTable Transport
		{
			get { return (DataTable)GetValue(TransportProperty); }
			set { SetValue(TransportProperty, value); }
		}
		public EmployeesSubmitUC()
		{
			InitializeComponent();

		}
		private void Event_Submit(object sender, RoutedEventArgs e)
		{
			if (sender is Button btn)
			{
				switch (btn.Content.ToString())
				{
					case "Search":
						Submit_Search();
						break;
					case "Add":
						Submit_Add();
						break;
					case "Edit":
						Submit_Edit();
						break;
					default:
						MessageBox.Show("Phương thức không hợp lệ", "ERROR", MessageBoxButton.OK, MessageBoxImage.Stop);
						break;
				}
			}
		}

		#region MyRegion

		void EmptyInput()
		{
			txt_full_name.Text = null;
			txt_code.Text = null;
			txt_code.IsEnabled = true;
			txt_dateHire.SelectedDate = null;
			txt_dateMaternity.SelectedDate = null;
			txt_dateResign.SelectedDate = null;
			cbb_department.Text = null;
			cbb_section.Text = null;
			cbb_position.Text = null;
			cbb_cost_center.Text = null;
			cbb_status.Text = null;
			btn_submit.Content = "Submit";
		}
		public void SetType(object[] target = null)
		{
			if (target == null)
			{
				if (!txt_code.IsEnabled) txt_code.IsEnabled = true;
				btn_submit.Content = "Search";
			}
			else if (target.Length == 1 && target[0].ToString() == "Add")
			{
				EmptyInput();
				btn_submit.Content = "Add";
			}
			else if (target.Length == 2)
			{
				if (target[1] is Employees employess)
				{
					switch (target[0].ToString())
					{
						case "Add":
							EmptyInput();
							btn_submit.Content = "Add";
							SetInfo(employess, "Add");
							break;
						case "Edit":
							EmptyInput();
							btn_submit.Content = "Edit";
							SetInfo(employess, "Edit");
							break;
						default:
							break;
					}
				}
				else
				{
					MessageBox.Show("Kiểu dữ liệu truyền vào không hợp lệ", "ERROR", MessageBoxButton.OK, MessageBoxImage.Stop);
				}
			}
		}
		void SetInfo(Employees employess, string type)
		{
			txt_full_name.Text = employess.Full_name;
			txt_code.Text = employess.Employee_code;
			if (type == "Edit") txt_code.IsEnabled = false;

			if (employess.Hire_date != null) txt_dateHire.SelectedDate = employess.Hire_date;
			if (employess.Maternity_leave_date != null) txt_dateMaternity.SelectedDate = employess.Maternity_leave_date;
			if (employess.Resignation_date != null) txt_dateResign.SelectedDate = employess.Resignation_date;

			cbb_department.Text = employess.Department;
			cbb_section.Text = employess.Section;
			cbb_position.Text = employess.Position;
			cbb_cost_center.Text = employess.Cost_center;
			cbb_status.Text = Converstatus(employess.Status);

		}

		string Converstatus(int? status)
		{
			if (status == null) return null;
			else
			{
				switch (status)
				{
					case 0:
						return "0: Nghỉ việc";
					case 1:
						return "1: Đang làm";
					case 2:
						return "2: Thai sản";
					default:
						return null;
				}
			}
		}

		Employees Create_Employees()
		{
			var employees = new Employees();
            employees.Employee_code = txt_code.Text;
            employees.Full_name = txt_full_name.Text;
            employees.Hire_date = txt_dateHire.SelectedDate;
            employees.Maternity_leave_date = txt_dateMaternity.SelectedDate;
            employees.Resignation_date = txt_dateResign.SelectedDate;
            employees.Department = cbb_department.Text;
            employees.Section = cbb_section.Text;
            employees.Position = cbb_position.Text;
            employees.Cost_center = cbb_cost_center.Text;
			if (!string.IsNullOrEmpty(cbb_status.Text)) employees.Status = int.Parse(cbb_status.Text.Substring(0, 1));
			else employees.Status = null;
			return employees;
		}
		void Submit_Add()
		{
			var employees = Create_Employees();
			if (employees.CheckData("Add"))
			{
				var res = EmployeesVM.Add_Employees(out string exception, employees);
				if (string.IsNullOrEmpty(exception))
				{
					if (res)
					{
						Transport = EmployeesVM.Get_Employees(out exception, employees);
						MaterialDesignThemes.Wpf.DrawerHost.CloseDrawerCommand.Execute(null, null);
					}
					else
					{
						MessageBox.Show("Không thêm được nhân viên này. Xảy ra lỗi trong quá trình xử lý.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
				else
				{
					MessageBox.Show(exception, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			else
			{
				MessageBox.Show("Chưa nhập đủ thông tin. Hãy kiểm tra lại.", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}

        void Submit_Edit()
        {
            var employees = Create_Employees();
            if (employees.CheckData("Edit"))
            {
                var res = EmployeesVM.Edit_Employees(out string exception, employees);
                if (string.IsNullOrEmpty(exception))
                {
                    if (res > 0)
                    {
                        Transport = EmployeesVM.Get_Employees(out exception, employees);
                        MaterialDesignThemes.Wpf.DrawerHost.CloseDrawerCommand.Execute(null, null);
                    }
                    else
                    {
                        MessageBox.Show("Xảy ra lỗi trong quá trình xử lý.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show(exception, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Chưa nhập đủ thông tin. Hãy kiểm tra lại.", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        void Submit_Search()
		{
			var employees = Create_Employees();
			Transport = EmployeesVM.Get_Employees(out string exception, employees);
			if (!string.IsNullOrEmpty(exception)) MessageBox.Show(exception, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
			else
			{
				EmptyInput();
				MaterialDesignThemes.Wpf.DrawerHost.CloseDrawerCommand.Execute(null, null);
			}
			//Transport
		}
		#endregion


	}
}
