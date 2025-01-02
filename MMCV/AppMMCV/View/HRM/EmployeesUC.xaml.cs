using AppMMCV.Services;
using AppMMCV.ViewModels.HRM;
using LibraryHelper.Models.HRM;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
	/// Interaction logic for EmployeesUC.xaml
	/// </summary>
	public partial class EmployeesUC : UserControl
	{
		public EmployeesUC()
		{
			InitializeComponent();
			//dtg_employees.ItemsSource = uc_submit.Transport.DefaultView;
			//Load_Data();
		}

		void Load_Data()
		{
			var data = EmployeesVM.Table_Employees();
			if (data != null)
			{
				dtg_employees.ItemsSource = data.DefaultView;
			}
			else
			{
				data = null;
			}
		}

		private void Event_Search(object sender, RoutedEventArgs e)
		{
			drh_main.IsRightDrawerOpen = true;
			uc_submit.SetType();			
		}

		private void Event_AddEmployees(object sender, RoutedEventArgs e)
		{
			uc_submit.SetType(new object[] { "Add" });
			drh_main.IsRightDrawerOpen = true;
		}

		private void Event_CopyEmployees(object sender, RoutedEventArgs e)
		{
			if (dtg_employees.SelectedItem is DataRowView dataRow)
			{
				var employees = new Employees(dataRow.Row);
				uc_submit.SetType(new object[] { "Add", employees });
				drh_main.IsRightDrawerOpen = true;
			}

		}

		private void Event_EditEmployees(object sender, RoutedEventArgs e)
		{
			if (dtg_employees.SelectedItem is DataRowView dataRow)
			{
				var employees = new Employees(dataRow.Row);
				uc_submit.SetType(new object[] { "Edit", employees });
				drh_main.IsRightDrawerOpen = true;
			}
		}

		void Event_ExportExcel(object sender, RoutedEventArgs e)
		{
			if (dtg_employees != null && dtg_employees.Items.Count > 0)
			{
				var data = dtg_employees.ItemsSource as DataView;
				var DataEmployees = data.Table.AsEnumerable().Select(d => new MasterEmployees() {
					EmployeeCode = d.Field<string>("employee_code"),
					FullName = d.Field<string>("full_name"),
					Position = d.Field<string>("position"),
					Department = d.Field<string>("department"),
				}).ToList();
                var _master = new MasterRegisterMeals();
                _master.ExportMasterRegisterMeal(new ObservableCollection<MasterEmployees>(DataEmployees));
            }
			else
			{
				MessageBox.Show("Không tìm thấy dữ liệu cần tải xuống.");
			}
        }

		private void Event_AddFromExcel(object sender, RoutedEventArgs e)
		{
			//OpenFileDialog openFile = new OpenFileDialog();
			//openFile.Multiselect = false;
			//openFile.Filter = "File .xlsx|*.xlsx";
			//if (openFile.ShowDialog() == true)
			//{
			//	string filePath = openFile.FileName;
			//	if (!string.IsNullOrEmpty(filePath))
			//	{
			//		var buiderError = new StringBuilder();
			//		var listEmployees = new List<Employees>();
			//		ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
			//		using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(filePath), true))
			//		{
			//			var workbook = package.Workbook;
			//			string sheetname = "Đang làm";
			//			var exits = workbook.Worksheets.Any(s => s.Name == sheetname);
			//			if (exits)
			//			{
			//				var worksheet = workbook.Worksheets[sheetname];
			//				int r_row = 7;
			//				while (!string.IsNullOrEmpty(worksheet.Cells[r_row, 2].Text))
			//				{
			//					string code = worksheet.Cells[r_row, 2].Text.Trim();
			//					if (worksheet.Cells[r_row, 4].Value is DateTime hireDate)
			//					{
			//						var checkCode = EmployeesVM.Exist_Employees(out string exception, code);
			//						if (string.IsNullOrEmpty(exception))
			//						{
			//							if (checkCode == false)
			//							{
			//								var employees = new Employees();
			//								employees.Employee_code = code;
			//								employees.Full_name = worksheet.Cells[r_row, 3].Text.Trim();
			//								employees.Hire_date = hireDate;
			//								employees.Department = worksheet.Cells[r_row, 5].Text.Trim();
			//								employees.Section = worksheet.Cells[r_row, 6].Text.Trim();
			//								employees.Position = worksheet.Cells[r_row, 7].Text.Trim();
			//								employees.Cost_center = worksheet.Cells[r_row, 8].Text.Trim();
			//								employees.Status = 1;
			//								listEmployees.Add(employees);
			//							}
			//						}
			//						else
			//						{
			//							buiderError.Append($"Error: {exception}");
			//						}
			//					}
			//					else
			//					{
			//						buiderError.Append($"Nhân viên {code} có DateHire không hợp lệ. Định dạng phải là DateTime.");
			//					}
			//					r_row++;
			//				}
			//			}
			//			else
			//			{
			//				buiderError.Append("Không tìm thấy Sheet 'Đang làm' trong tệp excel.");
			//			}
			//		}
			//		//Add employees
			//		if (string.IsNullOrEmpty(buiderError.ToString()))
			//		{
			//			var res = EmployeesVM.Add_Employees(out string exception, listEmployees);
			//			if (string.IsNullOrEmpty(exception))
			//			{
			//				if (res > 0) MessageBox.Show($"Đã thêm {res} nhân viên mới.");
			//				else MessageBox.Show("Không có nhân viên nào được thêm mới.");
			//			}
			//			else
			//			{
			//				MessageBox.Show($"Error: {exception}");
			//			}
			//		}
			//		else
			//		{
			//			MessageBox.Show(buiderError.ToString());
			//		}
			//	}
			//}
		}
	}
}
