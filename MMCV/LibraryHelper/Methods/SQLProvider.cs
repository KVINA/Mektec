using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryHelper.Methord
{
    public class SQLProvider
    {
        #region MyRegion
        /// <summary>
        /// Trả ra data từ query
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="server"></param>
        /// <param name="query"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public DataTable ExcuteQuery(out string exception, SERVER server, string query, object[] parameter = null)
        {
            exception = string.Empty;
            try
            {
                DataTable data = new DataTable();
                using (SqlConnection conn = new SqlConnection(server.ConnectionString()))
                {
                    conn.Open();
                    try
                    {
                        SqlCommand cmd = new SqlCommand(query, conn);
                        if (parameter != null)
                        {
                            var listpara = query.Split(' ');
                            int i = 0;
                            foreach (var para in listpara)
                            {
                                if (para.Contains('@'))
                                {
                                    if (parameter[i] == null) cmd.Parameters.AddWithValue(para, DBNull.Value);
                                    else cmd.Parameters.AddWithValue(para, parameter[i]);
                                    i++;
                                }
                            }
                        }
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(data);
                    }
                    catch (Exception ex)
                    {
                        exception = ex.Message;
                    }
                    finally { conn.Close(); }
                }
                return data;
            }
            catch (Exception ex)
            {
                exception = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// Trả ra số dòng thực hiện thay đổi thành công.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="server"></param>
        /// <param name="query"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public int ExcuteNonQuery(out string exception, SERVER server, string query, object[] parameter = null)
        {
            exception = string.Empty;
            int data = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(server.ConnectionString()))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(query, conn);
                        if (parameter != null)
                        {
                            var listpara = query.Split(' ');
                            int i = 0;
                            foreach (var para in listpara)
                            {
                                if (para.Contains('@'))
                                {
                                    if (parameter[i] == null) cmd.Parameters.AddWithValue(para, DBNull.Value);
                                    else cmd.Parameters.AddWithValue(para, parameter[i]);
                                    i++;
                                }
                            }
                        }
                        data = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        exception = ex.Message;
                    }
                    finally { conn.Close(); }
                }
            }
            catch (Exception ex)
            {
                exception = ex.Message;
            }

            return data;
        }
        /// <summary>
        /// Dùng transaction thực hiện lệnh thay đổi dữ liệu SQL
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="server"></param>
        /// <param name="query"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public int ExcuteNonTrans(out string exception, SERVER server, string query, object[] parameter = null)
        {
            exception = string.Empty;
            int data = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(server.ConnectionString()))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    try
                    {
                        if (parameter != null)
                        {
                            var listpara = query.Split(' ');
                            int i = 0;
                            foreach (var para in listpara)
                            {
                                if (para.Contains('@'))
                                {
                                    if (parameter[i] == null) cmd.Parameters.AddWithValue(para, DBNull.Value);
                                    else cmd.Parameters.AddWithValue(para, parameter[i]);
                                    i++;
                                }
                            }
                        }

                        cmd.Transaction = transaction;
                        data = cmd.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        exception = ex.Message;
                        transaction.Rollback();
                    }
                    finally
                    {
                        transaction.Dispose();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex.Message;
            }
            return data;
        }
        /// <summary>
        /// Trả ra giá trị của ô đầu tiên lấy được
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="server"></param>
        /// <param name="query"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public object ExcuteScalar(out string exception, SERVER server, string query, object[] parameter = null)
        {
            exception = string.Empty;
            object data = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(server.ConnectionString()))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(query, conn);
                        if (parameter != null)
                        {
                            var listpara = query.Split(' ');
                            int i = 0;
                            foreach (var para in listpara)
                            {
                                if (para.Contains('@'))
                                {
                                    if (parameter[i] == null) cmd.Parameters.AddWithValue(para, DBNull.Value);
                                    else cmd.Parameters.AddWithValue(para, parameter[i]);
                                    i++;
                                }
                            }
                        }
                        data = cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        exception = ex.Message;
                    }
                    finally { conn.Close(); }
                }
            }
            catch (Exception ex)
            {
                exception = ex.Message;
            }
            return data;
        }
        #endregion
    }   
    public class SERVER
    {
        private string dataSource;
        private string initialCatalog;
        private string userId;
        private string password;
        private bool integratedSecurity = false;
        private bool persistSecurityInfo = false;
        private int connectTimeout = 30;
        private bool encrypt = false;
        private bool multipleActiveResultSets = false;
        public string DataSource { get => dataSource; set => dataSource = value; }
        public string InitialCatalog { get => initialCatalog; set => initialCatalog = value; }
        public string UserId { get => userId; set => userId = value; }
        public string Password { get => password; set => password = value; }
        public bool IntegratedSecurity { get => integratedSecurity; set => integratedSecurity = value; }
        public bool PersistSecurityInfo { get => persistSecurityInfo; set => persistSecurityInfo = value; }
        public int ConnectTimeout { get => connectTimeout; set => connectTimeout = value; }
        public bool Encrypt { get => encrypt; set => encrypt = value; }
        public bool MultipleActiveResultSets { get => multipleActiveResultSets; set => multipleActiveResultSets = value; }
        public string ConnectionString()
        {
            string connect = $"Data Source={DataSource};Initial Catalog={InitialCatalog};User Id={UserId};" +
                $"Password={Password};Integrated Security={IntegratedSecurity};" +
                $"Persist Security Info={PersistSecurityInfo};Connect Timeout={ConnectTimeout};" +
                $"Encrypt={Encrypt};MultipleActiveResultSets={MultipleActiveResultSets};";
            return connect;
        }

        public SERVER(string DataSource, string InitialCatalog, string UserId, string Password)
        {
            this.DataSource = DataSource;
            this.InitialCatalog = InitialCatalog;
            this.UserId = UserId;
            this.Password = Password;
        }
    }
}
