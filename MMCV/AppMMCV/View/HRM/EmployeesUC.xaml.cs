using AppMMCV.ViewModels.HRM;
using LibraryHelper.Models.HRM;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
			//string compare = "";
			//object[] parameter = null;
			//int index = 1;
			//foreach (var item in wpn_inputsearch.Children.OfType<TextBox>())
			//{
			//    if (!string.IsNullOrEmpty(item.Text))
			//    {
			//        string title = item.Tag.ToString();
			//        if (!string.IsNullOrEmpty(compare)) compare = compare + "And ";
			//        compare = compare + "[" + title + "] = @" + title + " ";
			//        Array.Resize(ref parameter, index);
			//        parameter[index - 1] = item.Text.Trim();
			//    }
			//}
			//var data = (new EmployeesDAO()).Table_Employees(compare,parameter);
			//if (data != null)
			//{
			//    dtg_employees.ItemsSource = data.DefaultView;
			//}
			//else
			//{
			//    dtg_employees.ItemsSource = null;
			//}

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

		private void Event_ExportExcel(object sender, RoutedEventArgs e)
		{
			var data = EmployeesVM.Get_Employees(out string exception);
			if (string.IsNullOrEmpty(exception))
			{
				SaveFileDialog saveFile = new SaveFileDialog();
				saveFile.FileName = "EMPLOYEE MASTER DATA.xlsx";
				if (saveFile.ShowDialog() == true)
				{
					var fullPath = saveFile.FileName;
					if (!string.IsNullOrEmpty(fullPath))
					{
						ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
						using (ExcelPackage excel = new ExcelPackage())
						{
							var sht1 = excel.Workbook.Worksheets.Add("Đang làm");
							var sht2 = excel.Workbook.Worksheets.Add("Thai sản");
							var sht3 = excel.Workbook.Worksheets.Add("Nghỉ việc");
							int wrow1 = 7;
							int wrow2 = 7;
							int wrow3 = 7;
							foreach (DataRow row in data.Rows)
							{
								var status = row["Status"].ToString();
								switch (status)
								{
									case "1":
										sht1.Cells[wrow1, 1].Value = row["SerialID"];
										sht1.Cells[wrow1, 2].Value = row["EmployeesCode"];
										sht1.Cells[wrow1, 3].Value = row["FullName"];
										sht1.Cells[wrow1, 4].Value = row["HireDate"];
										sht1.Cells[wrow1, 5].Value = row["Department"];
										sht1.Cells[wrow1, 6].Value = row["Section"];
										sht1.Cells[wrow1, 7].Value = row["Position"];
										sht1.Cells[wrow1, 8].Value = row["CostCenter"];
										wrow1++;
										break;
									case "2":
										sht2.Cells[wrow2, 1].Value = row["SerialID"];
										sht2.Cells[wrow2, 2].Value = row["EmployeesCode"];
										sht2.Cells[wrow2, 3].Value = row["FullName"];
										sht2.Cells[wrow2, 4].Value = row["HireDate"];
										sht2.Cells[wrow2, 5].Value = row["MaternityLeaveDate"];
										sht2.Cells[wrow2, 6].Value = row["Department"];
										sht2.Cells[wrow2, 7].Value = row["Section"];
										sht2.Cells[wrow2, 8].Value = row["Position"];
										sht2.Cells[wrow2, 9].Value = row["CostCenter"];
										wrow2++;
										break;
									case "0":
										sht3.Cells[wrow3, 1].Value = row["SerialID"];
										sht3.Cells[wrow3, 2].Value = row["EmployeesCode"];
										sht3.Cells[wrow3, 3].Value = row["FullName"];
										sht3.Cells[wrow3, 4].Value = row["HireDate"];
										sht3.Cells[wrow3, 5].Value = row["ResignationDate"];
										sht3.Cells[wrow3, 6].Value = row["Department"];
										sht3.Cells[wrow3, 7].Value = row["Section"];
										sht3.Cells[wrow3, 8].Value = row["Position"];
										sht3.Cells[wrow3, 9].Value = row["CostCenter"];
										wrow3++;
										break;
									default:
										break;
								}
							}
							excel.SaveAs(fullPath);
						}
						MessageBox.Show("Xong");
					}
				}
			}
			else
			{
				MessageBox.Show(exception, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void Event_AddFromExcel(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFile = new OpenFileDialog();
			openFile.Multiselect = false;
			openFile.Filter = "File .xlsx|*.xlsx";
			if (openFile.ShowDialog() == true)
			{
				string filePath = openFile.FileName;
				if (!string.IsNullOrEmpty(filePath))
				{
					var buiderError = new StringBuilder();
					var listEmployees = new List<Employees>();
					ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
					using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(filePath), true))
					{
						var workbook = package.Workbook;
						string sheetname = "Đang làm";
						var exits = workbook.Worksheets.Any(s => s.Name == sheetname);
						if (exits)
						{
							var worksheet = workbook.Worksheets[sheetname];
							int r_row = 7;
							while (!string.IsNullOrEmpty(worksheet.Cells[r_row, 2].Text))
							{
								string code = worksheet.Cells[r_row, 2].Text.Trim();
								if (worksheet.Cells[r_row, 4].Value is DateTime hireDate)
								{
									var checkCode = EmployeesVM.Exist_Employees(out string exception, code);
									if (string.IsNullOrEmpty(exception))
									{
										if (checkCode == false)
										{
											var employees = new Employees();
											employees.Employee_code = code;
											employees.Full_name = worksheet.Cells[r_row, 3].Text.Trim();
											employees.Hire_date = hireDate;
											employees.Department = worksheet.Cells[r_row, 5].Text.Trim();
											employees.Section = worksheet.Cells[r_row, 6].Text.Trim();
											employees.Position = worksheet.Cells[r_row, 7].Text.Trim();
											employees.Cost_center = worksheet.Cells[r_row, 8].Text.Trim();
											employees.Status = 1;
											listEmployees.Add(employees);
										}
									}
									else
									{
										buiderError.Append($"Error: {exception}");
									}
								}
								else
								{
									buiderError.Append($"Nhân viên {code} có DateHire không hợp lệ. Định dạng phải là DateTime.");
								}
								r_row++;
							}
						}
						else
						{
							buiderError.Append("Không tìm thấy Sheet 'Đang làm' trong tệp excel.");
						}
					}
					//Add employees
					if (string.IsNullOrEmpty(buiderError.ToString()))
					{
						var res = EmployeesVM.Add_Employees(out string exception, listEmployees);
						if (string.IsNullOrEmpty(exception))
						{
							if (res > 0) MessageBox.Show($"Đã thêm {res} nhân viên mới.");
							else MessageBox.Show("Không có nhân viên nào được thêm mới.");
						}
						else
						{
							MessageBox.Show($"Error: {exception}");
						}
					}
					else
					{
						MessageBox.Show(buiderError.ToString());
					}
				}
			}
		}
	}
}
