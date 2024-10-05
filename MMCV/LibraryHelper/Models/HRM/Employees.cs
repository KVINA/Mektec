using LibraryHelper.Methord;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryHelper.Models.HRM
{
	public class Employees
	{
		private int? employee_id;
		private string employee_code;
		private string full_name;
		private DateTime? hire_date;
		private DateTime? maternity_leave_date;
		private DateTime? resignation_date;
		private string department;
		private string section;
		private string position;
		private string cost_center;
		private int? status;

		public int? Employee_id { get => employee_id; set => employee_id = value; }
		public string Employee_code { get => employee_code; set => employee_code = value; }
		public string Full_name { get => full_name; set => full_name = value; }
		public DateTime? Hire_date { get => hire_date; set => hire_date = value; }
		public DateTime? Maternity_leave_date { get => maternity_leave_date; set => maternity_leave_date = value; }
		public DateTime? Resignation_date { get => resignation_date; set => resignation_date = value; }
		public string Department { get => department; set => department = value; }
		public string Section { get => section; set => section = value; }
		public string Position { get => position; set => position = value; }
		public string Cost_center { get => cost_center; set => cost_center = value; }
		public int? Status { get => status; set => status = value; }

		public bool CheckData(string type = "Add")
		{
			if (string.IsNullOrEmpty(this.Employee_code)) return false;
			if (string.IsNullOrEmpty(this.Full_name)) return false;
			if (this.Hire_date == null) return false;
			if (string.IsNullOrEmpty(this.Department)) return false;
			if (string.IsNullOrEmpty(this.Section)) return false;
			if (string.IsNullOrEmpty(this.Position)) return false;
			if (string.IsNullOrEmpty(this.Cost_center)) return false;
			if (this.Status == null) return false;
			if (type == "Edit") if (Employee_id == null) return false;
			return true;
		}

		public Employees() { }
		public Employees(DataRow row)
		{
			this.employee_id = (int?)row["employee_id"];
			this.employee_code = row["employee_code"].ToString();
			this.full_name = row["full_name"].ToString();

			if (row["hire_date"] == DBNull.Value) this.hire_date = null;
			else this.hire_date = (DateTime?)row["hire_date"];

			if (row["maternity_leave_date"] == DBNull.Value) this.maternity_leave_date = null;
			else this.maternity_leave_date = (DateTime?)row["maternity_leave_date"];

			if (row["resignation_date"] == DBNull.Value) this.resignation_date = null;
			else this.resignation_date = (DateTime?)row["resignation_date"];

			this.Department = row["Department"].ToString();
			this.Section = row["Section"].ToString();
			this.Position = row["Position"].ToString();
			this.cost_center = row["cost_center"].ToString();
			this.Status = (int?)row["Status"];
		}
	}

}
