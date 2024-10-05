using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryHelper.Models.HRM
{
	public class Meal_time
	{
		public Meal_time() { }
		public Meal_time(DataRow row) 
		{
			this.id = (int)row["id"];
			this.meal = row["meal"].ToString();
			this.start_time = (TimeSpan)row["start_time"];
			this.hour_duration = (int)row["hour_duration"];
			this.plus_day = (int)row["plus_day"];
			this.update_at = (DateTime)row["update_at"];
			this.updator = row["updator"].ToString();
		}
		
		private int id;
		private string meal;
		private TimeSpan start_time;
		private int hour_duration;
		private int plus_day;
		private DateTime update_at;
		private string updator;

		public int Id { get => id; set => id = value; }
		public string Meal { get => meal; set => meal = value; }
		public TimeSpan Start_time { get => start_time; set => start_time = value; }
		public int Hour_duration { get => hour_duration; set => hour_duration = value; }
		public int Plus_day { get => plus_day; set => plus_day = value; }
		public DateTime Update_at { get => update_at; set => update_at = value; }
		public string Updator { get => updator; set => updator = value; }
	}
}
