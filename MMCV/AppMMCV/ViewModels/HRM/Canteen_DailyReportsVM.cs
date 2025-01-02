using AppMMCV.Services;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static OfficeOpenXml.ExcelErrorValue;

namespace AppMMCV.ViewModels.HRM
{
    internal class Canteen_DailyReportsVM : INotifyPropertyChanged
    {

        bool isLoading;
        SeriesCollection pieCollection;
        SeriesCollection barCollection;
        string[] barLabels;
        Func<double, string> barFormatter;
        DateTime dateStart = DateTime.Now.Date;
        DateTime dateEnd = DateTime.Now.Date;

        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }
        public SeriesCollection PieCollection { get => pieCollection; set { pieCollection = value; OnPropertyChanged(nameof(PieCollection)); } }
        public SeriesCollection BarCollection { get => barCollection; set { barCollection = value; OnPropertyChanged(nameof(BarCollection)); } }
        public string[] BarLabels { get => barLabels; set { barLabels = value; OnPropertyChanged(nameof(BarLabels)); } }
        public Func<double, string> BarFormatter { get => barFormatter; set { barFormatter = value; OnPropertyChanged(nameof(BarFormatter)); } }
        public DateTime DateStart { get => dateStart; set { dateStart = value; OnPropertyChanged(nameof(DateStart)); Execute_GetData(); } }
        public DateTime DateEnd { get => dateEnd; set { dateEnd = value; OnPropertyChanged(nameof(DateEnd)); Execute_GetData(); } }

        void Execute_GetData()
        {
            if (DateStart > DateEnd)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.");
            }
            else if (DateStart.AddDays(31) <= DateEnd)
            {
                MessageBox.Show("Dữ liệu tối đa tìm kiếm là trong một tháng.");
            }
            else
            {
                GetData();
            }
        }
        async void GetData()
        {
            await Task.Run(() =>
            {
                IsLoading = true;
                PieCollection = null;
                BarCollection = null;
                BarLabels = null;

                string exception = string.Empty;
                string query_label = "Select CONCAT(category_id,':',category_name) category from meal_category order by category_id;";
                var data_label = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV68_HRM, query_label);
                if (!string.IsNullOrEmpty(exception)) goto The_Exception;
                if (data_label != null && data_label.Rows.Count > 0)
                {
                    BarLabels = data_label.AsEnumerable().Select(x => x.Field<string>("category")).ToArray();
                    string query_total = $"Select * from [daily_total] with(nolock) Where meal_date between '{DateStart.ToString("yyyy-MM-dd")}' and '{DateEnd.ToString("yyyy-MM-dd")}';";
                    var data_total = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV68_HRM, query_total);
                    if (!string.IsNullOrEmpty(exception)) goto The_Exception;
                    if (data_total != null && data_total.Rows.Count > 0)
                    {
                        var dic_data = new Dictionary<string, double>();
                        var dic_data1 = new Dictionary<string, double>();
                        foreach (DataRow row in data_label.Rows)
                        {
                            string key = (row.Field<string>("category"));
                            int id = int.Parse(key.Split(':')[0]);
                            double values = data_total.AsEnumerable().Where(x => x.Field<int>("category_id") == id).Sum(x => x.Field<int>("quantity_before"));
                            double values1 = data_total.AsEnumerable()
      .Where(x => x.Field<int?>("category_id") == id)
      .Sum(x => x.Field<int?>("quantity_after") ?? 0);
                            dic_data.Add(key, values);
                            dic_data1.Add(key, values1);
                        }
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            PieCollection = DataPie(dic_data1);
                            BarCollection = DataBar(dic_data, dic_data1);
                        });

                    }
                }
                else
                {
                    exception = "Không lấy được dữ liệu trong meal_category.";
                    goto The_Exception;
                }
                IsLoading = false;
                return;
            The_Exception:
                IsLoading = false;
                MessageBox.Show(exception);

            });
        }
        /// <summary>
        /// Biểu đồ tròn
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        SeriesCollection DataPie(Dictionary<string, double> data)
        {
            if (data != null && data.Count > 0)
            {
                var series = new SeriesCollection();

                foreach (string key in data.Keys)
                {
                    var chartValues = new ChartValues<double>(new double[] { data[key] });
                    var item = new PieSeries
                    {
                        Title = key,
                        Values = chartValues,
                        DataLabels = true,
                        LabelPoint = chartPoint => $"{chartPoint.Participation:P2}"
                    };
                    series.Add(item);
                }
                return series;
            }
            else return null;
        }
        /// <summary>
        /// Biều đồ cột
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        SeriesCollection DataBar(Dictionary<string, double> data, Dictionary<string, double> data1)
        {
            if (data != null && data.Count > 0)
            {
                var chartValues = new ChartValues<double>(data.Values);
                var chartValues1 = new ChartValues<double>(data1.Values);
                var item = new ColumnSeries()
                {
                    Title = "Đăng ký",
                    Values = chartValues
                };
                var item1 = new ColumnSeries()
                {
                    Title = "Thực tế",
                    Values = chartValues1
                };
                var series = new SeriesCollection();
                series.Add(item);
                series.Add(item1);
                return series;
            }
            else
            {
                return null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Canteen_DailyReportsVM()
        {
            BarFormatter = value => value + " Suất";
            GetData();
        }
    }
}
