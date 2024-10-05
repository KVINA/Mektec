using AppMMCV.Services;
using AppMMCV.ViewModels.Admin;
using AppMMCV.ViewModels.Systems;
using LibraryHelper.Models.HRM;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace AppMMCV.View.Systems
{
    /// <summary>
    /// Interaction logic for Permission.xaml
    /// </summary>
    public partial class Permission : UserControl
    {
        private readonly PermissionVM permissionVM;
        private int menu_id;
        public Permission()
        {
            InitializeComponent();
			permissionVM = new PermissionVM();
			this.DataContext = permissionVM;
		}

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            //grb_form.Header = "Add";
            //drh_body.IsRightDrawerOpen = true;
        }

        void Load_Permision()
        {
            if (menu_id > 0)
            {
                string query = "";
            }
        }


		
	}
}
