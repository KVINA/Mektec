
using AppMMCV.Services;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AppMMCV.ViewModels.HRM
{
    public class Canteen_ConfirmDailyMealsVM : INotifyPropertyChanged
    {


        DateTime selectedDate;
        Brush notifyBrush;
        string notifyContent;
        bool isLoading;
        DataView tableRegister;
        DataView tableActual;

        void ExportDetailsToExcel()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Excel file (.xlsx)|*.xlsx";
            saveFile.FileName = "Canteen details.xlsx";
            if (saveFile.ShowDialog() == true)
            {
                string filePath = saveFile.FileName;
                if (!string.IsNullOrEmpty(filePath))
                {
                    string exception = null;
                    var dataRegister = GET_DetailRegister(out exception);
                    if (!string.IsNullOrEmpty(exception)) goto THE_EXCEPTION;
                    var dataActual = GET_DetalActual(out exception);
                    if (!string.IsNullOrEmpty(exception)) goto THE_EXCEPTION;

                    //Xuất excel
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    using (ExcelPackage package = new ExcelPackage())
                    {
                        var sheet1 = package.Workbook.Worksheets.Add("Register");
                        var sheet2 = package.Workbook.Worksheets.Add("Actual");

                        //Header sheet1
                        int indexRow = 1;
                        int indexCol = 0;
                        foreach (DataColumn column in dataRegister.Columns)
                        {
                            indexCol++;
                            sheet1.Cells[indexRow, indexCol].Value = column.ColumnName;
                        }                        
                        //Content sheet1
                        foreach (DataRow row in dataRegister.Rows)
                        {
                            indexRow++;
                            indexCol = 0;
                            foreach (DataColumn column in dataRegister.Columns)
                            {
                                indexCol++;
                                sheet1.Cells[indexRow, indexCol].Value = row[column.ColumnName].ToString();
                            }
                        }

                        //Header sheet2
                        indexRow = 1;
                        indexCol = 0;
                        foreach (DataColumn column in dataActual.Columns)
                        {
                            indexCol++;
                            sheet2.Cells[1, indexCol].Value = column.ColumnName;
                        }
                        //Content sheet1
                        foreach (DataRow row in dataActual.Rows)
                        {
                            indexRow++;
                            indexCol = 0;
                            foreach (DataColumn column in dataActual.Columns)
                            {
                                indexCol++;
                                sheet2.Cells[indexRow, indexCol].Value = row[column.ColumnName].ToString();
                            }
                        }
                        //Save file
                        package.SaveAs(filePath);
                    }
                    NotifyBrush = Brushes.Blue;
                    NotifyContent = "Download details completed.";
                    return;
                THE_EXCEPTION:
                    NotifyContent = exception;
                    NotifyBrush = Brushes.Red;
                }
            }          

        }
        DataTable GET_DetailRegister(out string exception)
        {
            string query = $"usp_get_detail_register '{SelectedDate.ToString("yyyy-MM-dd")}'";
            var data = SQLService.Method.ExecuteQuery(out exception,SQLService.Server.SV68_HRM, query);
            return data;
        }

        DataTable GET_DetalActual(out string exception)
        {
            string query = $"usp_get_detail_actual '{SelectedDate.ToString("yyyy-MM-dd")}'";
            var data = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV68_HRM, query);
            return data;
        }
        public Brush NotifyBrush { get => notifyBrush; set { notifyBrush = value; OnPropertyChanged(nameof(NotifyBrush)); } }
        public string NotifyContent { get => notifyContent; set { notifyContent = value; OnPropertyChanged(nameof(NotifyContent)); } }
        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }
        public DataView TableRegister { get => tableRegister; set { tableRegister = value; OnPropertyChanged(nameof(TableRegister)); } }
        public DataView TableActual { get => tableActual; set { tableActual = value; OnPropertyChanged(nameof(TableActual)); } }
        public DateTime SelectedDate { get => selectedDate; set { selectedDate = value; OnPropertyChanged(nameof(SelectedDate)); GET_DataTable(); } }

        public ICommand Command_ExecuteDataTable { get; set; }
        public ICommand Command_ExportDetailsToExcel { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        void GET_DataTable()
        {
            string exception;
            //Register
            string query_register = $"exec usp_get_total_register '{SelectedDate.ToString("yyyy-MM-dd")}';";
            var data_register = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV68_HRM, query_register);
            if (!string.IsNullOrEmpty(exception)) goto THE_EXCEPTION;
            //Actual
            string query_actual = $"exec usp_get_total_actual '{SelectedDate.ToString("yyyy-MM-dd")}';";
            var data_actual = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV68_HRM, query_actual);
            if (!string.IsNullOrEmpty(exception)) goto THE_EXCEPTION;
            //Category
            string query_category = "Select category_id,CONCAT(category_id,':',category_name) 'category' from meal_category with(nolock) order by category_id;";
            var data_category = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV68_HRM, query_category);
            if (!string.IsNullOrEmpty(exception)) goto THE_EXCEPTION;
            //Set header
            foreach (DataRow row in data_category.Rows)
            {
                int id = (int)row["category_id"];
                string category = row["category"].ToString();
                foreach (DataColumn column in data_register.Columns) if (column.ColumnName == id.ToString()) column.ColumnName = category;
                foreach (DataColumn column in data_actual.Columns) if (column.ColumnName == id.ToString()) column.ColumnName = category;
            }

            TableRegister = data_register.DefaultView;
            TableActual = data_actual.DefaultView;
            return;
        THE_EXCEPTION:
            MessageBox.Show(exception);
        }
        
      
        public Canteen_ConfirmDailyMealsVM
            () 
        {
            SelectedDate = DateTime.Today.Date;
            Command_ExecuteDataTable = new RelayCommand(GET_DataTable);
            Command_ExportDetailsToExcel = new RelayCommand(ExportDetailsToExcel);
            GET_DataTable();
        }

        
    }
}
