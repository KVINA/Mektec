using AppMMCV.ViewModels.Admin;
using LibraryHelper.Models.HRM;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppMMCV.View.Admin
{
    /// <summary>
    /// Interaction logic for MenuUC.xaml
    /// </summary>
    public partial class MenuUC : UserControl
    {
        private MenuVM menuVM;
        public MenuUC()
        {
            InitializeComponent();
        }

        public void LoadMenu(App_menu app_menu,string type)
        {            
            menuVM = this.DataContext as MenuVM;
            menuVM.TypeSubmit = type;
            menuVM.MenuInfo = app_menu;
        }

        private void Button_Click_Submit(object sender, RoutedEventArgs e)
        {
            menuVM.SubmitMenu?.Invoke();
        }
    }
}
