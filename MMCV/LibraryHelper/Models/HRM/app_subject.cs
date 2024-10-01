using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryHelper.Models.HRM
{
    public class App_subject
    {
        /*
        subject_id int identity(1,1) primary key,
		subject_name varchar(50) unique,
		subject_icon nvarchar(200),
		create_at datetime default getdate(), 
		create_by varchar(10)
         */
        public App_subject() { }
        public App_subject(DataRow row) 
        {
            this.subject_id = (int)row["subject_id"];
            this.subject_name = row["subject_name"].ToString();
            this.subject_icon = row["subject_icon"].ToString();
            this.create_at = (DateTime)row["create_at"];
            this.create_by = row["create_by"].ToString();
        }

        private int subject_id;
        private string subject_name;
        private string subject_icon;
        private DateTime create_at;
        private string create_by;

        public int Subject_id { get => subject_id; set => subject_id = value; }
        public string Subject_name { get => subject_name; set => subject_name = value; }
        public string Subject_icon { get => subject_icon; set => subject_icon = value; }
        public DateTime Create_at { get => create_at; set => create_at = value; }
        public string Create_by { get => create_by; set => create_by = value; }
    }
}
