using AppMMCV.Services;
using AppMMCV.View.Admin;
using AppMMCV.ViewModels.Systems;
using LibraryHelper.Models.HRM;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
    /// Interaction logic for MainMenuUC.xaml
    /// </summary>
    public partial class MainMenuUC : UserControl
    {
        private readonly Dictionary<int, TabItem> DIC_TabItem = new Dictionary<int, TabItem>();
        private readonly Dictionary<string, MenuItem> DIC_MenuItem = new Dictionary<string, MenuItem>();
        
        public MainMenuUC(int menu_id)
        {
            InitializeComponent();
            GET_MenuInfo(menu_id);
            SET_Permission(menu_id);
        }

        void GET_MenuInfo(int menu_id)
        {
            menu_main.Items.Clear();
            DIC_MenuItem.Clear();
            string employee_code = DataService.UserInfo.username;

            if (!string.IsNullOrEmpty(employee_code))
            {
                string query = $"Select * FROM [app_menu_item] where menu_id = {menu_id} order by item_index;";
                var data = SQLService.Method.ExecuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
                if (string.IsNullOrEmpty(exception))
                {
                    if (data != null && data.Rows.Count > 0)
                    {
                        foreach (DataRow row in data.Rows)
                        {
                            var item = new App_menu_item(row);
                            var icon = new PackIcon() { Kind = (PackIconKind)Enum.Parse(typeof(PackIconKind), item.Item_icon) };
                            if (!DIC_MenuItem.Keys.Contains(item.Item_group))
                            {
                                var menuParent = new MenuItem() { Header = item.Item_group };
                                DIC_MenuItem.Add(item.Item_group, menuParent);
                                menu_main.Items.Add(menuParent);
                            }

                            var menuChild = new MenuItem()
                            {
                                Header = item.Item_name,
                                Icon = icon,
                                Tag = item,
                                IsEnabled = false
                            };

                            DIC_MenuItem[item.Item_group].Items.Add(menuChild);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(exception);
                }
            }
        }


        void SET_Permission(int menu_id)
        {
            if (DIC_MenuItem.Count > 0)
            {
                string employee_code = DataService.UserInfo.username;
                string query = $"SELECT [list_item_id] FROM [HRM].[dbo].[app_roles] Where menu_id = {menu_id} and employee_code = '{employee_code}';";
                var data = SQLService.Method.ExecuteScalar(out string exception, SQLService.Server.SV68_HRM, query);
                if (string.IsNullOrEmpty(exception))
                {
                    if (data != null)
                    {
                        var list_MenuId = data.ToString().Split(',').ToList();
                        foreach (var menuParent in DIC_MenuItem.Values)
                        {
                            foreach (MenuItem menuItem in menuParent.Items)
                            {
                                if (menuItem.Tag is App_menu_item item && list_MenuId.Contains(item.Item_id.ToString()))
                                {                                    
									menuItem.IsEnabled = true;                                    
                                }
                                else
                                {
                                    menuItem.IsEnabled = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(exception);
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuitem)
            {
                if (menuitem.Tag is App_menu_item item)
                {
                    if (DIC_TabItem.ContainsKey(item.Item_id))
                    {
                        Active_TabItem(item.Item_id);
                    }
                    else
                    {
                        var tabitem = new TabItem() { Tag = item.Item_id };
                        tabitem.Header = TabItem_Header(item.Item_name,item.Item_id);
                        var content = ControlUC(out string exception, item.Item_controller);
                        if (string.IsNullOrEmpty(exception))
                        {
                            content.Margin = new Thickness(0, 10, 0, 0);
                            tabitem.Content = content;
                            DIC_TabItem.Add(item.Item_id, tabitem);
                            tab_control.Items.Add(DIC_TabItem[item.Item_id]);
                            Active_TabItem(item.Item_id);
                        }
                        else
                        {
                            MessageBox.Show(exception);
                        }
                    }
                }
            }
        }
        public static UserControl ControlUC(out string exception, string userControlName)
        {
            exception = null;
            try
            {
                object[] constructorParameters = new object[] { "MyObject" };
                UserControl newUC = (UserControl)Activator.CreateInstance(Type.GetType(userControlName));
                return newUC;

            }
            catch (Exception ex)
            {
                exception = ex.Message;
                return null;
            }
        }
        Grid TabItem_Header(string header, int item_id)
        {
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength() });

            var textblock = new TextBlock() { Text = header, Margin = new Thickness(2), FontFamily = new FontFamily("Segoe UI"), Foreground = Brushes.Teal, FontWeight = FontWeights.DemiBold, FontSize = 12 };
            grid.Children.Add(textblock);

            var border = new Border() { Cursor = Cursors.Hand, Tag = item_id };
            border.Child = new TextBlock() { Text = "❌", Foreground = Brushes.Red, Background = Brushes.AliceBlue, Margin = new Thickness(2) };
            border.MouseLeftButtonUp += Border_MouseLeftButtonUp; ;

            grid.Children.Add(border);
            Grid.SetColumn(border, 1);
            return grid;
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag != null)
            {
                var item_it = (int)border.Tag;
                tab_control.Items.Remove(DIC_TabItem[item_it]);
                DIC_TabItem.Remove(item_it);
                GC.Collect();
            }
        }

        public void Active_TabItem(int item_id)
        {
            tab_control.SelectedItem = DIC_TabItem[item_id];
        }
    }
}
