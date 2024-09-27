using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryHelper.Models.HRM
{
    public class app_subject
    {
        /*
        subject_id int identity(1,1) primary key,
		subject_name varchar(50) unique,
		subject_description nvarchar(200),
		create_at datetime default getdate(), 
		create_by varchar(10)
         */
        public app_subject() { }
        public app_subject(DataRow row) 
        {
            this.subject_id = (int)row["subject_id"];
            this.subject_name = row["subject_name"].ToString();
            this.subject_description = row["subject_description"].ToString();
            this.create_at = (DateTime)row["create_at"];
            this.create_by = row["create_by"].ToString();
        }

        private int subject_id;
        private string subject_name;
        private string subject_description;
        private DateTime create_at;
        private string create_by;

        public int Subject_id { get => subject_id; set => subject_id = value; }
        public string Subject_name { get => subject_name; set => subject_name = value; }
        public string Subject_description { get => subject_description; set => subject_description = value; }
        public DateTime Create_at { get => create_at; set => create_at = value; }
        public string Create_by { get => create_by; set => create_by = value; }
    }
}
