using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AppMMCV.Services
{
    public class ImpersonateService
    {
        private static System.Security.Principal.WindowsImpersonationContext impersonationContext;

        public static bool IsImpersonate()
        {
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            try
            {
                // Logon user với quyền được chỉ định
                if (LogonUser("mmcv-swuser", "mmcv.mekjpn.ngnet", "A86js659nk",
                    LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                {
                    // Tạo token mạo danh
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        var tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                        impersonationContext = tempWindowsIdentity.Impersonate();
                        return impersonationContext != null;
                    }
                    else
                    {
                        return false;
                    }
                }
                // Mạo danh thất bại
                else
                {
                    var ex = Marshal.GetLastWin32Error();
                    return false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thực hiện mạo danh: {ex.Message}");
                return false;
            }
            finally
            {
                // Đảm bảo đóng các token để tránh rò rỉ tài nguyên
                if (tokenDuplicate != IntPtr.Zero)
                {
                    CloseHandle(tokenDuplicate);
                }

                if (token != IntPtr.Zero)
                {
                    CloseHandle(token);
                }
            }

        }

        public static void UnImpersonate()
        {
            // Hoàn tác mạo danh nếu có
            impersonationContext?.Undo();
            impersonationContext = null;
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int LogonUser(
            string lpszUserName,
            string lpszDomain,
            string lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(
            IntPtr hToken,
            int impersonationLevel,
            ref IntPtr hNewToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        private const int LOGON32_LOGON_INTERACTIVE = 2;
        private const int LOGON32_PROVIDER_DEFAULT = 0;
    }
}
