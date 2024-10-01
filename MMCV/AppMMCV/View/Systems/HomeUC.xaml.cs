using AppMMCV.Json;
using AppMMCV.Services;
using AppMMCV.View.Admin;
using AppMMCV.ViewModels.Systems;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AppMMCV.View.Systems
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class HomeUC : UserControl
    {
        private readonly HomeVM homeVM;
        public HomeUC()
        {
            InitializeComponent();
            homeVM = new HomeVM();
            this.DataContext = homeVM;
        }

        private void ListBoxItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem item)
            {
                int menu_id = (int)item.Tag;
                if (menu_id == 1)
                {
                    card_renderbody.Content = new MainMenuUC(menu_id);
                }
            }
        }

		private void PackIcon_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
            var wd = new Window() { Content = new AppRolesUC() };
            wd.ShowDialog();
        }
    }


}
