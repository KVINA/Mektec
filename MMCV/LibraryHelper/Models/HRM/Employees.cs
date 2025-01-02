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
        private int? picture_id;
        private bool is_delete;

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
        public int? Picture_id { get => picture_id; set => picture_id = value; }
        public bool Is_delete { get => is_delete; set => is_delete = value; }
        public bool CheckData(string type = "Add")
        {
            if (string.IsNullOrEmpty(this.Employee_code)) return false;
            if (string.IsNullOrEmpty(this.Full_name)) return false;
            if (string.IsNullOrEmpty(this.Position)) return false;
            if (string.IsNullOrEmpty(this.Department)) return false;
            //if (type == "Edit") if (employee_id == null) return false;
            return true;
        }

        public Employees() { }
        public Employees(DataRow row)
        {
            this.employee_id = row.Field<int?>("employee_id");
            this.employee_code = row.Field<string>("employee_code");
            this.full_name = row.Field<string>("full_name");
            this.hire_date = row.Field<DateTime?>("hire_date");
            this.maternity_leave_date = row.Field<DateTime?>("maternity_leave_date");
            this.resignation_date = row.Field<DateTime?>("resignation_date");
            this.department = row.Field<string>("department");
            this.section = row.Field<string>("section");
            this.position = row.Field<string>("position");
            this.cost_center = row.Field<string>("cost_center");
            this.status = row.Field<int?>("status");
            this.picture_id = row.Field<int?>("picture_id");
            this.is_delete = row.Field<bool>("is_delete");
        }
    }
   


}
