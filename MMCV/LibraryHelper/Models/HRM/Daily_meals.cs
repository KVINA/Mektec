using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryHelper.Models.HRM
{
    public class Daily_meals
    {
        public Daily_meals(DataRow row)
        {
            this.id = Convert.ToInt64(row["id"]);
            this.meal_date = Convert.ToDateTime(row["meal_date"]);
            this.employee_code = Convert.ToString(row["employee_code"]);
            this.meal_time_id = Convert.ToInt32(row["meal_time_id"]);
            this.category_id = Convert.ToInt32(row["category_id"]);
            this.registration_by =  Convert.ToString(row["registration_by"]);
            this.registration_at = ConvertDate(row["registration_at"]);
            this.distribution_by =  Convert.ToString(row["distribution_by"]);
            this.distribution_at = ConvertDate(row["distribution_at"]);
        }
        DateTime? ConvertDate(object value)
        {
            if (value != DBNull.Value && value is DateTime) return Convert.ToDateTime(value);
            else return null;
        }

        private long id;
        private DateTime meal_date;
        private string employee_code;
        private int meal_time_id;
        private int category_id;
        private string registration_by;
        private DateTime? registration_at;
        private string distribution_by;
        private DateTime? distribution_at;

        public long Id { get => id; set => id = value; }
        public DateTime Meal_date { get => meal_date; set => meal_date = value; }
        public string Employee_code { get => employee_code; set => employee_code = value; }
        public int Meal_time_id { get => meal_time_id; set => meal_time_id = value; }
        public int Category_id { get => category_id; set => category_id = value; }
        public string Registration_by { get => registration_by; set => registration_by = value; }
        public DateTime? Registration_at { get => registration_at; set => registration_at = value; }
        public string Distribution_by { get => distribution_by; set => distribution_by = value; }
        public DateTime? Distribution_at { get => distribution_at; set => distribution_at = value; }

    }
}
