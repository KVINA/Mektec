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

        public static void Login(out string exception, string username, string password)
        {
            try
            {
                // Set default
                exception = string.Empty;
                DataService.UserInfo = null;
                DataService.IsLogin = false;

                var parameter = new object[] { username, password };
                string query = "EXEC usp_users_login @username , @password ";
                var info = SQLService.Method.ExecuteQuery(out exception, SQLService.Server.SV68_HRM, query, parameter);
                if (string.IsNullOrEmpty(exception))
                {
                    if (info != null && info.Rows.Count == 1)
                    {
                        int role_id = (int)info.Rows[0]["role_id"];
                        DataService.UserInfo = new Users(username, password, role_id);
                        DataService.IsLogin = true;
                    }
                    else if (info.Rows.Count == 0)
                    {
                        //Kiểm tra mã nhân viên có trong danh sách không
                        string query_check = $"Select count(*) from [tbHR_Employee] where EmployeeCode = '{username}';";
                        var data_check = SQLService.Method.ExecuteScalar(out exception, SQLService.Server.SV91_EZ_MEKTEC, query_check);
                        if (string.IsNullOrEmpty(exception))
                        {
                            if ((int)data_check > 0)
                            {
                                if (password == "mmcv123")
                                {
                                    string query_insert = $"Insert Into [users](role_id,username,password,status) values (2,'{username}','{password}',1);";
                                    var data_insert = SQLService.Method.ExecuteNonQuery(out exception, SQLService.Server.SV68_HRM, query_insert);
                                    if (string.IsNullOrEmpty(exception))
                                    {
                                        if (data_insert > 0)
                                        {
                                            DataService.UserInfo = new Users(username, password, 2);
                                            DataService.IsLogin = true;
                                        }
                                    }
                                }
                                else
                                {
                                    exception = "Tài khoản hoặc mật khẩu không chính xác.";
                                }
                            }
                            else
                            {
                                exception = "Không tìm thấy nhân viên trong danh sách nhân viên của nhà máy.";
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex.Message;
            }

        }

        public static void Logout()
        {
            DataService.UserInfo = null;
            DataService.IsLogin = false;
        }
    }
}
