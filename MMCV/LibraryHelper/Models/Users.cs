using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryHelper.Models
{
    public class Users
    {
        public int user_id { get; set; }
        public int role_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int status { get; set; }
        public DateTime created_at { get; set; }
        public string creator { get; set; }
        public Users() { }
        public Users(string username, string password, int role_id)
        {
            this.username = username;
            this.password = password;
            this.role_id = role_id;
        }
    }
}
