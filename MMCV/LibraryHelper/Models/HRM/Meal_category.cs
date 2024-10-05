using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryHelper.Models.HRM
{
	public class Meal_category
	{
		
		public Meal_category() { }
		public Meal_category(DataRow row) 
		{ 
			this.category_id = Convert.ToInt32(row["category_id"]);
			this.category_name = Convert.ToString(row["category_name"]);
			this.category_description = Convert.ToString(row["category_description"]);
			this.update_at = Convert.ToDateTime(row["update_at"]);
			this.update_by = Convert.ToString(row["update_by"]);
		}

		private int category_id;
		private string category_name;
		private string category_description;
		private DateTime update_at;
		private string update_by;

		public int Category_id { get => category_id; set => category_id = value; }
		public string Category_name { get => category_name; set => category_name = value; }
		public string Category_description { get => category_description; set => category_description = value; }
		public DateTime Update_at { get => update_at; set => update_at = value; }
		public string Update_by { get => update_by; set => update_by = value; }
	}
}
