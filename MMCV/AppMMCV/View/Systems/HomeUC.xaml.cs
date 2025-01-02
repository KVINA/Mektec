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
                card_renderbody.Content = new MainMenuUC(menu_id);
                DataService.GlobalVM.ActiveMenu = menu_id;
            }
        }

		private void Expander_Expanded(object sender, RoutedEventArgs e)
		{
            if (sender is Expander expander)
            {
                int subject_id = (int)expander.Tag;
                foreach (var item in homeVM.MenuInfos)
                {
                    if (item.Subject_id != subject_id)
                    {
                        item.IsOpen = false;
                    }
                }
            }
            
		}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mni_user.IsSubmenuOpen = true;
        }

        private void Event_ShowChangePassword(object sender, RoutedEventArgs e)
        {
            var wd = new ChangePasswordWD();
            wd.ShowDialog();
        }
    }


}
