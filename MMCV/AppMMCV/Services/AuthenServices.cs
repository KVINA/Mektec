using LibraryHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AppMMCV.Services
{
    internal class AuthenServices
    {
      
        public static void Login(string username, string password)
        {
            // Set default
            DataService.User = null;
            DataService.IsLogin = false;

            var parameter = new object[] { username, password };
            string query = "EXEC usp_users_login @username , @password ";
            var info = SQLService.Method.ExcuteQuery(out string exception, SQLService.Server.SV68_HRM, query, parameter);
            if (string.IsNullOrEmpty(exception))
            {
                if (info != null && info.Rows.Count == 1)
                {
                    int role_id = (int)info.Rows[0]["role_id"];
                    DataService.User = new Users(username,password,role_id);
                    DataService.IsLogin = true;
                }
            }
        }

        public static void Logout()
        {
            DataService.User = null;
            DataService.IsLogin = false;
        }        
    }
}
