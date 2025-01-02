using Microsoft.SqlServer.Server;
using Microsoft.Win32;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Data;

namespace AppMMCV.Services
{
    public class EZService
    {
        
        private readonly static string FolderImage = @"\\10.80.1.91\Others";

        public string GetImagePathEmployee(out string exception, string EmployeeCode)
        {
            string query = $"Select PathToSave,Extension from tbSYS_File A with(NoLock) Inner Join tbHR_Employee B with(NoLock) On A.ID = B.PictureId  Where EmployeeCode = '{EmployeeCode}';";
            var data = SQLService.Method.ExecuteScalar(out exception, SQLService.Server.SV91_EZ_MEKTEC, query);
            if (data != null && !string.IsNullOrEmpty(data.ToString())) return data.ToString();
            else return null;
        }

        public BitmapImage GetImageEmployee(out string exception, string ImagePath)
        {
            exception = string.Empty;
            if (string.IsNullOrEmpty(ImagePath)) return AvataDefault();
            if (ImagePath.Substring(0, 12) == "Files\\Others")
            {
                string BasePath = Regex.Replace(ImagePath, @"^Files\\Others", FolderImage, RegexOptions.IgnoreCase);
                MemoryStream memoryStream = new MemoryStream();
                if (ImpersonateService.IsImpersonate())
                {
                    using (FileStream fs = new FileStream(BasePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {

                        fs.CopyTo(memoryStream);
                    }
                    ImpersonateService.UnImpersonate();
                }

                BitmapImage Bitmap = new BitmapImage();
                if (memoryStream != null)
                {
                    memoryStream.Position = 0; // Đặt lại vị trí đọc của stream
                    Bitmap.BeginInit();
                    Bitmap.StreamSource = memoryStream;
                    Bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    Bitmap.EndInit();
                }
                return Bitmap;
            }
            else
            {
                exception = $"Đường dẫn ảnh không hợp lệ. Cần bắt đầu bằng 'Files\\':\n{ImagePath}";
                return AvataDefault();
            }
        }

        BitmapImage AvataDefault()
        {
            try
            {
                string ImagePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Image\\AvataDF.png";
                MemoryStream memoryStream = new MemoryStream();
                using (FileStream fs = new FileStream(ImagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    fs.CopyTo(memoryStream);
                }
                BitmapImage Bitmap = new BitmapImage();
                if (memoryStream != null)
                {
                    memoryStream.Position = 0; // Đặt lại vị trí đọc của stream
                    Bitmap.BeginInit();
                    Bitmap.StreamSource = memoryStream;
                    Bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    Bitmap.EndInit();
                }
                return Bitmap;
            }
            catch (Exception)
            {
                return null;    
            }
            
        }       

        
    }
}
