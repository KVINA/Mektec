using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Collections.ObjectModel;

namespace AppMMCV.Services
{
    public class MasterRegisterMeals
    {
        #region MyRegion
        private List<string> Option_Category()
        {
            string query = "Select CONCAT(category_id,':',category_name) as category from [meal_category]";
            var data = SQLService.Method.ExecuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
            if (string.IsNullOrEmpty(exception))
            {
                if (data != null && data.Rows.Count > 0)
                {
                    var option_Meal = new List<string>();
                    foreach (DataRow row in data.Rows)
                    {
                        option_Meal.Add(row[0].ToString());
                    }
                    return option_Meal;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                MessageBox.Show(exception);
                return null;
            }
        }
        private List<string> Option_Meal()
        {
            string query = "select CONCAT([id],':',[meal]) 'meal' from [meal_time] with(nolock) order by id;";
            var data = SQLService.Method.ExecuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
            if (string.IsNullOrEmpty(exception))
            {
                if (data != null && data.Rows.Count > 0)
                {
                    var option_Meal = new List<string>();
                    foreach (DataRow row in data.Rows)
                    {
                        option_Meal.Add(row[0].ToString());
                    }
                    return option_Meal;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                MessageBox.Show(exception);
                return null;
            }

        }
        private void FillBorderExcel(ExcelRange range, ExcelBorderStyle style, System.Drawing.Color color)
        {
            // Kẻ bảng với màu nâu nhạt cho đường kẻ
            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            // Chọn màu cho đường kẻ (màu nâu nhạt)
            var lightBrown = ColorTranslator.FromHtml("#A52A2A");  // Màu nâu (brown)
            range.Style.Border.Top.Color.SetColor(lightBrown);
            range.Style.Border.Bottom.Color.SetColor(lightBrown);
            range.Style.Border.Left.Color.SetColor(lightBrown);
            range.Style.Border.Right.Color.SetColor(lightBrown);
        }
        #endregion

        public void ExportMasterRegisterMeal(ObservableCollection<MasterEmployees> DataEmployees)
        {
            try
            {
                if (DataEmployees != null && DataEmployees.Count > 0)
                {
                    var option_meal = Option_Meal();
                    var option_category = Option_Category();
                    if (option_category != null && option_meal != null)
                    {
                        option_category.Add("N/A");
                        SaveFileDialog saveFile = new SaveFileDialog();
                        saveFile.Filter = "Excel Files|*.xlsx";
                        saveFile.FileName = "Master register for meal";
                        string savePath = string.Empty;
                        if (saveFile.ShowDialog() == true) savePath = saveFile.FileName;
                        if (!string.IsNullOrEmpty(savePath))
                        {
                            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                            using (ExcelPackage package = new ExcelPackage())
                            {
                                var workbook = package.Workbook;
                                var sheet = workbook.Worksheets.Add("Meal Register");
                                sheet.Column(1).Width = 4;
                                sheet.Column(2).Width = 8;
                                sheet.Column(3).Width = 15;
                                sheet.Column(4).Width = 40;
                                sheet.Cells[2, 2].Value = "MASTER ĐĂNG KÝ SUẤT ĂN";
                                sheet.Cells[2, 2].Style.Font.Bold = true;
                                sheet.Cells[3, 2].Value = "Ngày";
                                sheet.Cells[3, 3].Value = DateTime.Now;
                                sheet.Cells[3, 3].Style.Numberformat.Format = "yyy/MM/dd";
                                sheet.Cells[3, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                sheet.Cells[4, 2].Value = "Version";
                                sheet.Cells[4, 3].Value = "Ver20241101";
                                int start_r = 5;
                                int write_r = start_r;
                                foreach (var option in option_category) // Thực đơn
                                {
                                    write_r++;
                                    int column = 5 + option_meal.Count;
                                    sheet.Cells[write_r, column].Value = option;
                                }
                                write_r++;
                                sheet.Cells[write_r, 2].Value = "STT";
                                sheet.Cells[write_r, 3].Value = "Mã nhân viên";
                                sheet.Cells[write_r, 4].Value = "Tên nhân viên";

                                int start_c = 4;
                                int write_c = start_c;
                                foreach (var option in option_meal)
                                {
                                    write_c++;
                                    sheet.Cells[start_r, write_c].Value = option;
                                    sheet.Cells[write_r, write_c].Value = option;
                                    sheet.Column(write_c).Width = 15;
                                }
                                write_c++;
                                sheet.Cells[start_r, write_c].Value = "Suất ăn"; //Suất ăn
                                sheet.Cells[write_r, write_c].Value = "Ghi chú"; //Ghi chú
                                sheet.Column(write_c).Width = 15;

                                FillBorderExcel(sheet.Cells[start_r, start_c + 1, write_r - 1, write_c], ExcelBorderStyle.Thin, ColorTranslator.FromHtml("#A52A2A"));

                                for (int i = start_r + 1; i < write_r; i++)
                                    for (int j = start_c + 1; j < write_c; j++)
                                        sheet.Cells[i, j].FormulaR1C1 = $"=COUNTIF(R{write_r + 1}C{j}:R[5000]C,R{i}C{write_c})";

                                sheet.Cells[start_r + 1, start_c + 1, write_r - 1, write_c - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                sheet.Cells[start_r, start_c + 1, start_r, write_c].Style.Font.Bold = true;
                                sheet.Cells[start_r, start_c + 1, start_r, write_c].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[start_r, start_c + 1, start_r, write_c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[start_r, start_c + 1, start_r, write_c].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.AliceBlue);

                                sheet.Cells[write_r, 2, write_r, write_c].Style.Font.Bold = true;
                                sheet.Cells[write_r, 2, write_r, write_c].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet.Cells[write_r, 2, write_r, write_c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[write_r, 2, write_r, write_c].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.AliceBlue);  // Màu xanh nhạt
                                start_r = write_r;

                                sheet.Cells[5, 2].Value = "Row"; //Dòng bắt đầu chứa dữ liệu công nhân viên 
                                sheet.Cells[5, 3].Value = write_r; //Giá trị dòng
                                sheet.Cells[5, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; //Căn lề trái

                                int stt = 0;
                                foreach (var item in DataEmployees)
                                {
                                    stt++;
                                    write_r++;
                                    sheet.Cells[write_r, 2].Value = stt;
                                    sheet.Cells[write_r, 3].Value = item.EmployeeCode;
                                    sheet.Cells[write_r, 4].Value = item.FullName;
                                    sheet.Cells[write_r, write_c].Value = item.Department;
                                }

                                sheet.View.ShowGridLines = false;

                                var validation_category = sheet.DataValidations.AddListValidation(sheet.Cells[start_r + 1, start_c + 1, write_r, write_c - 1].Address);
                                foreach (var option in option_category) validation_category.Formula.Values.Add(option);

                                var dataRange = sheet.Cells[start_r, 2, write_r, write_c];
                                FillBorderExcel(dataRange, ExcelBorderStyle.Thin, ColorTranslator.FromHtml("#A52A2A"));

                                sheet.Cells.Style.Font.Name = "Palatino Linotype";
                                package.SaveAs(savePath);
                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show("The category cannot be retrieved, or the category has not been declared.");
                    }

                }
                else
                {
                    MessageBox.Show("No data to download");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    public class MasterEmployees
    {
        private int? iD;
        private string employeeCode;
        private string fullName;
        private string position;
        private string department;

        public int? ID { get => iD; set => iD = value; }
        public string EmployeeCode { get => employeeCode; set => employeeCode = value; }
        public string FullName { get => fullName; set => fullName = value; }
        public string Position { get => position; set => position = value; }
        public string Department { get => department; set => department = value; }
        public MasterEmployees() { }
        /// <summary>
        /// type = false: Dữ liệu của EZ. type = true: Dữ liệu của Canteen
        /// </summary>
        /// <param name="row">ID int, EmployeeCode string, FullName string, Position string, Department string</param>
        public MasterEmployees(DataRow row)
        {
            this.ID = row.Field<int?>("ID");
            this.EmployeeCode = row["EmployeeCode"].ToString();
            this.FullName = row["FullName"].ToString();
            this.Position = row["Position"].ToString();
            this.Department = row["Department"].ToString();
        }
    }
}
