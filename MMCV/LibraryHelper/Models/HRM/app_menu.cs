using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryHelper.Models.HRM
{
    public class App_menu
    {
        public App_menu() { }
        public App_menu(DataRow row)
        {
            this.menu_id = (int)row["menu_id"];
            this.menu_name = row["menu_name"].ToString();
            this.menu_description = row["menu_description"].ToString();
            this.menu_index = (int)row["menu_index"];
            this.subject_id = (int)row["subject_id"];
            this.create_at = (DateTime)row["create_at"];
            this.create_by = row["create_by"].ToString();
        }

        private int menu_id;
        private string menu_name;
        private string menu_description;
        private int menu_index;
        private int subject_id;
        private DateTime create_at;
        private string create_by;

        public int Menu_id { get => menu_id; set => menu_id = value; }
        public string Menu_name { get => menu_name; set => menu_name = value; }
        public string Menu_description { get => menu_description; set => menu_description = value; }
        public int Menu_index { get => menu_index; set => menu_index = value; }
        public int Subject_id { get => subject_id; set => subject_id = value; }
        public DateTime Create_at { get => create_at; set => create_at = value; }
        public string Create_by { get => create_by; set => create_by = value; }
    }
}
