using AppMMCV.Json;
using AppMMCV.Properties;
using AppMMCV.Services;
using AppMMCV.ViewModels.Admin;
using LibraryHelper.Models.HRM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
        private readonly MenuSettingsVM settingsVM;
        public MenuSettings()
        {
            InitializeComponent();
            settingsVM = new MenuSettingsVM();
            this.DataContext = settingsVM;
        }
        private void Btn_Click_Add(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                string type = btn.Tag.ToString();
                switch (type)
                {
                    case "Add Subject":
                        settingsVM.HeaderActive = "Add Subject";
                        var subjectUC =  new SubjectUC() { DataContext = settingsVM.SubjectContext };
                        subjectUC.LoadSubject();
                        settingsVM.UsercontrolActive = subjectUC;
                        break;
                    case "Add Menu":
                        settingsVM.HeaderActive = "Add new menu";
                        if (dtg_subject.SelectedItem is App_subject item)
                        {
                            var menuUC = new MenuUC() { DataContext = settingsVM.MenuContext };
                            App_menu app_Menu = new App_menu() { Subject_id = item.Subject_id};
                            menuUC.LoadMenu(app_Menu,"Add");
                            settingsVM.UsercontrolActive = menuUC;
                            break;
                        }
                        else return;
                    case "Add Item":
                        settingsVM.HeaderActive = "Add new menu item";
                        var menuItemUC = new MenuItemUC() { DataContext = settingsVM.MenuItemContext};
                        if (dtg_menu.SelectedItem is App_menu _menu)
                        {
                            var _menuitem = new App_menu_item() { Menu_id = _menu.Menu_id };
                            menuItemUC.LoadMenuItem(_menuitem, "Add");
                            settingsVM.UsercontrolActive = menuItemUC;
                            break;
                        }
                        else return;                        
                    default:
                        return;
                }
                settingsVM.IsOpenRightDrawer = true;
            }
        }

        private void Btn_Click_Edit(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                string type = btn.Tag.ToString();

                switch (type)
                {
                    case "Edit Subject":
                        if (dtg_subject.SelectedItem is App_subject _subject)
                        {
                            settingsVM.HeaderActive = "Edit subject";
                            var subjectUC = new SubjectUC() { DataContext = settingsVM.SubjectContext };
                            subjectUC.LoadSubject(_subject);
                            settingsVM.UsercontrolActive = subjectUC;
                            break;
                        }
                        else return;
                    case "Edit Menu":

                        if (dtg_menu.SelectedItem is App_menu _menu)
                        {
                            settingsVM.HeaderActive = "Edd menu";
                            var menuUC = new MenuUC() { DataContext = settingsVM.MenuContext };
                            menuUC.LoadMenu(_menu, "Edit");
                            settingsVM.UsercontrolActive = menuUC;
                            break;
                        }
                        else return;
                    case "Edit Item":
                        if (dtg_menuItem.SelectedItem is App_menu_item _menu_item)
                        {
                            settingsVM.HeaderActive = "Edd menu item";
                            var menuItemUC = new MenuItemUC() { DataContext = settingsVM.MenuItemContext };
                            menuItemUC.LoadMenuItem(_menu_item, "Edit");
                            settingsVM.UsercontrolActive = menuItemUC;
                            break;
                        }
                        else return;                        
                        
                    default:
                        return;
                }
                settingsVM.IsOpenRightDrawer = true;
            }
        }
    }
}
