using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryHelper.Models.HRM
{
    public class App_roles
    {
        public App_roles() { }
        public App_roles(DataRow row) 
        {
            this.role_id = (int)row["role_id"];
            this.employeeCode = row["employee_code"].ToString();
            this.menu_id = (int)row["menu_id"];
            this.access = (bool)row["access"];
            this.list_item_id = row["list_item_id"].ToString();
            this.create_at = (DateTime)row["create_at"];
            this.create_by = row["create_by"].ToString();
        }
        private int role_id;
        private string employeeCode;
        private int menu_id;
        private bool access;
        private string list_item_id;
        private DateTime create_at;
        private string create_by;
        public int Role_id { get => role_id; set => role_id = value; }
        public string Employee_code { get => employeeCode; set => employeeCode = value; }
        public int Menu_id { get => menu_id; set => menu_id = value; }
        public bool Access { get => access; set => access = value; }
        public string List_item_id { get => list_item_id; set => list_item_id = value; }
        public DateTime Create_at { get => create_at; set => create_at = value; }
        public string Create_by { get => create_by; set => create_by = value; }
    }
}
