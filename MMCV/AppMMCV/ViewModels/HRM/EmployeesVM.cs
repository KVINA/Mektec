using AppMMCV.Services;
using LibraryHelper.Models.HRM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMMCV.ViewModels.HRM
{
	public class EmployeesVM
	{
		public static DataTable Table_Employees(string compare = null, object[] parameter = null)
		{
			if (!string.IsNullOrEmpty(compare) && parameter != null)
			{
				string query = $"Select * From MMCV_Employees With(NoLock) Where {compare} order by SerialID";
				var data = SQLService.Method.ExecuteQuery(out string exception,SQLService.Server.SV68_HRM, query, parameter);
				return data;
			}
			else
			{
				string query = "Select * From MMCV_Employees With(NoLock) order by SerialID";
				var data = SQLService.Method.ExecuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
				return data;
			}
		}
		public static bool? Exist_Employees(out string exception, string employee_code)
		{
			try
			{
				string query = $"Select count(*) from [employees] Where [employee_code] = '{employee_code}'";
				var res = SQLService.Method.ExecuteScalar(out exception, SQLService.Server.SV68_HRM, query);
				if (res != null)
				{
					var len = int.Parse(res.ToString());
					return len > 0;
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				exception = ex.Message;
				return null;
			}

		}
		public static bool Add_Employees(out string exception, Employees employees)
		{
			string query = "Insert Into [employees] ([employee_code],[full_name],[hire_date],[maternity_leave_date],[resignation_date],[department],[section],[position],[cost_center],[status]) " +
				"Values ( @employee_code , @full_name , @hire_date , @maternity_leave_date , @resignation_date , @department , @section , @position , @cost_center , @status )";
			var parameter = new object[] { employees.Employee_code, employees.Full_name, employees.Hire_date,
				employees.Maternity_leave_date,employees.Resignation_date,employees.Department,
				employees.Section,employees.Position,employees.Cost_center,employees.Status};
			var res = SQLService.Method.ExecuteNonQuery(out exception, SQLService.Server.SV68_HRM, query, parameter);
			return res > 0;
		}

		public static int Add_Employees(out string exception, List<Employees> listEmployees)
		{
			var buiderQuery = new StringBuilder();
			foreach (var item in listEmployees)
			{
				string query = "Insert Into [employees] ([employee_code],[full_name],[hire_date],[department],[section],[position],[cost_center],[status]) " +
				$"Values ( '{item.Employee_code}' , N'{item.Full_name}' , '{item.Hire_date?.ToString("yyyy-MM-dd hh:mm:ss")}' , '{item.Department}' , '{item.Section}' , '{item.Position}' , '{item.Cost_center}' , {item.Status} );";
				buiderQuery.AppendLine(query);
			}
			if (string.IsNullOrEmpty(buiderQuery.ToString()))
			{
				exception = "Danh sách nhân viên trống.";
				return -1;
			}
			else
			{
				var res = SQLService.Method.ExecuteNonTrans(out exception, SQLService.Server.SV68_HRM, buiderQuery.ToString());
				return res;
			}
		}

		public static int Edit_Employees(out string exception, Employees employees)
		{
            string query = $"Update [employees] Set [full_name] = @full_name ,[hire_date] = @hire_date ,[maternity_leave_date] = @maternity_leave_date ,[resignation_date] = @resignation_date ,[department] = @department ,[section] = @section ,[position] = @position ,[cost_center] = @cost_center , [status] = @status Where [employee_code] = @employee_code ;";
			
			var parameter = new object[] { employees.Full_name, employees.Hire_date, employees.Maternity_leave_date, employees.Resignation_date, employees.Department, employees.Section, employees.Position, employees.Cost_center, employees.Status, employees.Employee_code };
           
            var res = SQLService.Method.ExecuteNonQuery(out exception, SQLService.Server.SV68_HRM, query, parameter);
            return res;
        }
		public static DataTable Get_Employees(out string exception, Employees employees = null)
		{
			employees.Is_delete = false;
            string query = "Select * from employees";
			if (employees != null)
			{
				List<string> condition = new List<string>();
				foreach (var property in employees.GetType().GetProperties())
				{
					var value = property.GetValue(employees);
					if (value != null && !string.IsNullOrEmpty(value.ToString()))
					{
						if (property.Name != "Picture_id")
						{
                            if (property.Name == "status") 
								condition.Add($"status = '{value.ToString().Substring(0, 1)}'");
                            else if (value is DateTime date) condition.Add($"{property.Name} = '{date.ToString("yyyy-MM-dd")}'");
                            else condition.Add($"{property.Name} = N'{value.ToString()}'");
                        }						
					}
				}

				if (condition.Count > 0)
				{
					query += " Where " + string.Join(" And ", condition);
				}
			}

			var data = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV68_HRM, query);
			return data;
		}
	}
}
