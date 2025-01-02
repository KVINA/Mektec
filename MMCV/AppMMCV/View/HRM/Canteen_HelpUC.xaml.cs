using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AppMMCV.View.HRM
{
    /// <summary>
    /// Interaction logic for Canteen_HelpUC.xaml
    /// </summary>
    public partial class Canteen_HelpUC : UserControl
    {
        public Canteen_HelpUC()
        {
            InitializeComponent();
            InitializeWebView();
        }

        private async void InitializeWebView()
        {
            // Đường dẫn tương đối tới file PDF
            string relativePath = @"Documents\Canteen\Help.pdf";
            string absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

            // Kiểm tra nếu file tồn tại
            if (File.Exists(absolutePath))
            {
                // Khởi tạo WebView2
                await webView.EnsureCoreWebView2Async(null);

                // Load file PDF
                webView.CoreWebView2.Navigate(absolutePath);
            }
            else
            {
                MessageBox.Show("Không tìm thấy file PDF!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
