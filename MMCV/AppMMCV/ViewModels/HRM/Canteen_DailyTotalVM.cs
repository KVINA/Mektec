using AppMMCV.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppMMCV.ViewModels.HRM
{
    public class Canteen_DailyTotalVM : INotifyPropertyChanged
    {
        public Canteen_DailyTotalVM()
        {
            Get_DataDailyTotal();
        }


        async void Get_DataDailyTotal()
        {
            if (!IsLoading) IsLoading = true;
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() => DataDailyTotal.Clear());
                string query = "Select B.id,B.meal,C.category_id,C.category_name,quantity from " +
                    "(Select meal_time_id,category_id,Count(employee_code) 'quantity' from [daily_meals] " +
                    $"Where meal_date = '{SelectedDate}' group by meal_time_id,category_id)  as A " +
                    "Inner join meal_time B On A.meal_time_id = B.id " +
                    "Inner join meal_category C On A.category_id = C.category_id;";
                var data = SQLService.Method.ExecuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
                if (string.IsNullOrEmpty(exception))
                {
                    if (data != null && data.Rows.Count > 0)
                    {
                        foreach (DataRow row in data.Rows)
                        {
                            Application.Current.Dispatcher.Invoke(() => DataDailyTotal.Add(new DailyReport(row)));
                        }
                    }
                }
            });
            if (IsLoading) IsLoading = false;
        }
        bool isLoading;
        ObservableCollection<DailyReport> dataDailyTotal;
        DateTime selectedDate = DateTime.Now;

        public ObservableCollection<DailyReport> DataDailyTotal
        {
            get { if (dataDailyTotal == null) dataDailyTotal = new ObservableCollection<DailyReport>(); return dataDailyTotal; }
            set { dataDailyTotal = value; OnPropertyChanged(nameof(DataDailyTotal)); }
        }
        public DateTime SelectedDate { get => selectedDate; set { selectedDate = value; OnPropertyChanged(nameof(SelectedDate)); Get_DataDailyTotal(); } }

        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DailyReport
    {
        public DailyReport(DataRow row)
        {
            this.meal_time_id = Convert.ToInt32(row["id"]);
            this.meal = Convert.ToString(row["meal"]);
            this.category_id = Convert.ToInt32(row["category_id"]);
            this.category_name = Convert.ToString(row["category_name"]);
            this.quantity = Convert.ToInt32(row["quantity"]);
        }

        private int meal_time_id;
        private string meal;
        private int category_id;
        private string category_name;
        private int quantity;

        public int Meal_time_id { get => meal_time_id; set => meal_time_id = value; }
        public string Meal { get => meal; set => meal = value; }
        public int Category_id { get => category_id; set => category_id = value; }
        public string Category_name { get => category_name; set => category_name = value; }
        public int Quantity { get => quantity; set => quantity = value; }
    }
}
