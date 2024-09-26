using AppMMCV.ViewModels.Admin;
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
    /// Interaction logic for MenuSettings.xaml
    /// </summary>
    public partial class MenuSettings : UserControl
    {
        public MenuSettings()
        {
            InitializeComponent();
            this.DataContext = new MenuSettingsVM();
        }

        private void Btn_Click_Add(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (this.DataContext is MenuSettingsVM dtContext)
                {
                    string type = btn.Tag.ToString();
                    
                    switch (type)
                    {
                        case "app_subjects":
                            dtContext.HeaderActive = "Add new subject";
                            dtContext.UsercontrolActive = new SubjectUC();                            
                            break;
                        case "app_menu":
                            dtContext.HeaderActive = "Add new menu";
                            dtContext.UsercontrolActive = new MenuUC();
                            break;
                        case "app_menu_item":
                            dtContext.HeaderActive = "Add new menu item";
                            dtContext.UsercontrolActive = new MenuItemUC();
                            break;
                        default:
                            return;
                    }
                    dtContext.UsercontrolActive.DataContext = dtContext;
                    dtContext.IsOpenRightDrawer = true;
                }                
            }
        }
    }
}
