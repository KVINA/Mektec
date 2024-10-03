using AppMMCV.Json;
using AppMMCV.Services;
using LibraryHelper.Models.HRM;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace AppMMCV.ViewModels.Systems
{
	class GroupInfo : INotifyPropertyChanged
	{
		string item_group;
		ObservableCollection<ItemInfo> list_item;
		public string Item_group { get => item_group; set { item_group = value; OnPropertyChanged(nameof(Item_group)); } }
		public ObservableCollection<ItemInfo> List_item { get => list_item; set { list_item = value; OnPropertyChanged(nameof(List_item)); } }

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
	class ItemInfo : INotifyPropertyChanged
	{
		int item_id;
		string item_name;
		string item_controller;
		string item_icon;
		string item_header;

		public string Item_name { get => item_name; set => item_name = value; }
		public string Item_controller { get => item_controller; set => item_controller = value; }
		public string Item_icon { get => item_icon; set => item_icon = value; }
		public string Item_header { get => item_header; set => item_header = value; }
		public int Item_id { get => item_id; set { item_id = value; OnPropertyChanged(nameof(Item_id)); } }

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	internal class MainMenuVM : INotifyPropertyChanged
	{
		public MainMenuVM(int menu_id)
		{
			GroupMenuItem.Clear();
			string employee_code = DataService.UserInfo.username;
			if (!string.IsNullOrEmpty(employee_code))
			{
				string query = $"Select * FROM [app_menu_item] where menu_id = {menu_id} order by item_index;";
				var data = SQLService.Method.ExcuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
				if (string.IsNullOrEmpty(exception))
				{
					if (data != null && data.Rows.Count > 0)
					{
						foreach (DataRow row in data.Rows)
						{
							string item_group = row["item_group"].ToString();
							GroupInfo groupInfo;
							if (GroupMenuItem.Any(g => g.Item_group == item_group.Trim()))
							{
								groupInfo = GroupMenuItem.Where(g => g.Item_group == item_group.Trim()).FirstOrDefault();
							}
							else
							{
								groupInfo = new GroupInfo()
								{
									Item_group = item_group.Trim(),
									List_item = new ObservableCollection<ItemInfo>()
								};
								GroupMenuItem.Add(groupInfo);
							}
							groupInfo.List_item.Add(new ItemInfo()
							{
								Item_id = (int)row["item_id"],
								Item_controller = row["item_controller"].ToString(),
								Item_header = row["item_header"].ToString(),
								Item_icon = row["item_icon"].ToString(),
								Item_name = row["item_name"].ToString()
							});
						}
					}
				}
				else
				{
					MessageBox.Show(exception);
				}
			}
		}

		ObservableCollection<GroupInfo> groupMenuItem;
		public ObservableCollection<GroupInfo> GroupMenuItem
		{
			get { if (groupMenuItem == null) groupMenuItem = new ObservableCollection<GroupInfo>(); return groupMenuItem; }
			set { groupMenuItem = value; OnPropertyChanged(nameof(GroupMenuItem)); }
		}

		//public MainMenuVM(List<ChildrenMenu> listmenu)
		//{
		//    ParentMenu = Gennerate(listmenu);
		//}
		//List<MenuItem> Gennerate(List<ChildrenMenu> listmenu)
		//{
		//    var menu = new List<MenuItem>();
		//    if (listmenu != null && listmenu.Count > 0)
		//    {
		//        foreach (var item in listmenu)
		//        {
		//            var menuitem = new MenuItem();
		//            menuitem.Header = item.ChildrenName;
		//            foreach (var child_item in item.MenuItems)
		//            {
		//                var child_menuitem = new MenuItem();
		//                child_menuitem.Header = child_item.Header;
		//                child_menuitem.Tag = child_item;
		//                child_menuitem.Icon = new PackIcon() { Kind = (PackIconKind)Enum.Parse(typeof(PackIconKind), child_item.Icon) };
		//                //if (permission.Contains(child_item.Header)) child_menuitem.IsEnabled = true;
		//                //else child_menuitem.IsEnabled = false;
		//                child_menuitem.Click += Child_menuitem_Click; ;
		//                menuitem.Items.Add(child_menuitem);
		//            }
		//            menu.Add(menuitem);
		//        }
		//    }
		//    return menu;
		//}
		//private void Child_menuitem_Click(object sender, System.Windows.RoutedEventArgs e)
		//{
		//    if (sender is MenuItem menuitem)
		//    {
		//        var header = menuitem.Header.ToString();
		//        if (menuitem.Tag is MenuItems item)
		//        {
		//            if (DIC_USERCONTROLS.ContainsKey(item.NameUsercontrol))
		//            {
		//                Active_TabItem($"{item.Namespace} / {item.NameUsercontrol}");
		//            }
		//            else
		//            {
		//                var tabitem = new TabItem();
		//                tabitem.Header = TabItem_Header(header, tabitem);
		//                var content = ControlUC(out string exception, item.Namespace, item.NameUsercontrol);
		//                if (string.IsNullOrEmpty(exception))
		//                {
		//                    content.Margin = new Thickness(0, 10, 0, 0);
		//                    tabitem.Content = content;
		//                    tabitem.Tag = item;
		//                    DIC_USERCONTROLS.Add(item.NameUsercontrol, tabitem);
		//                    TabItemsSource.Items.Add(tabitem);
		//                    Active_TabItem($"{item.Namespace} / {item.NameUsercontrol}");
		//                }
		//                else
		//                {
		//                    MessageBox.Show(exception);
		//                }
		//            }
		//        }
		//        else
		//        {
		//            MessageBox.Show("ERROR: MenuItem.Tag is NULL");
		//        }
		//    }
		//}

		///// <summary>
		///// Gọi Usercontrol
		///// </summary>
		///// <param name="exception"></param>
		///// <param name="Namespace"></param>
		///// <param name="NameUC"></param>
		///// <returns></returns>
		//public static UserControl ControlUC(out string exception, string Namespace, string NameUC)
		//{
		//    exception = null;
		//    try
		//    {
		//        string userControlName = $"{Namespace}.{NameUC}"; // Tên UserControl cần gọi
		//        object[] constructorParameters = new object[] { "MyObject" };
		//        UserControl newUC = (UserControl)Activator.CreateInstance(Type.GetType(userControlName));
		//        return newUC;

		//    }
		//    catch (Exception ex)
		//    {
		//        exception = ex.Message;
		//        return null;
		//    }            
		//}
		//Grid TabItem_Header(string header, TabItem tabitem)
		//{
		//    var grid = new Grid();
		//    grid.ColumnDefinitions.Add(new ColumnDefinition());
		//    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength() });

		//    var textblock = new TextBlock() { Text = header, Margin = new Thickness(2), FontFamily = new FontFamily("Segoe UI"), Foreground = Brushes.Teal, FontWeight = FontWeights.DemiBold, FontSize = 12 };
		//    grid.Children.Add(textblock);

		//    var border = new Border() { Cursor = Cursors.Hand, DataContext = tabitem };
		//    border.Child = new TextBlock() { Text = "❌", Foreground = Brushes.Red, Background = Brushes.AliceBlue, Margin = new Thickness(2) };
		//    border.MouseLeftButtonUp += Border_MouseLeftButtonUp;

		//    grid.Children.Add(border);
		//    Grid.SetColumn(border, 1);
		//    return grid; 
		//}

		//private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		//{
		//    if (sender is Border border && border.DataContext is TabItem tabitem)
		//    {
		//        if (tabitem.Tag is MenuItems item)
		//        {
		//            DIC_USERCONTROLS.Remove(item.NameUsercontrol);
		//            TabItemsSource.Items.Remove(tabitem);
		//            Active_TabItem();
		//            GC.Collect();
		//        }
		//    }
		//}

		//public void Active_TabItem(string addressmenu = null)
		//{
		//    if (!string.IsNullOrEmpty(addressmenu))
		//    {
		//        string name = addressmenu.Split('/')[1].Trim();
		//        if (!string.IsNullOrEmpty(name))
		//        {
		//            TitleInfo.Content = addressmenu;
		//            TitleInfo.Background = Brushes.Teal;
		//            TitleInfo.Foreground = Brushes.White;
		//            TabItemsSource.SelectedItem = DIC_USERCONTROLS[name];
		//        }
		//        else
		//        {
		//            TitleInfo.Content = null;
		//            TitleInfo.Background = null;
		//            TitleInfo.Foreground = null;
		//        }
		//    }
		//    else
		//    {
		//        TitleInfo.Content = null;
		//        TitleInfo.Background = null;
		//        TitleInfo.Foreground = null;
		//    }
		//}

		//private Dictionary<string, TabItem> DIC_USERCONTROLS = new Dictionary<string, TabItem>();
		//private List<MenuItem> parentMenu;
		//private TitleInfo titleInfo;
		//private TabControl tabItemsSource;
		//public List<MenuItem> ParentMenu { get => parentMenu; set { parentMenu = value; OnPropertyChanged(nameof(ParentMenu)); } }
		//public TitleInfo TitleInfo
		//{
		//    get { if (titleInfo == null) titleInfo = new TitleInfo(); return titleInfo; }
		//    set { titleInfo = value; OnPropertyChanged(nameof(ParentMenu)); }
		//}
		//public TabControl TabItemsSource
		//{
		//    get { if (tabItemsSource == null) tabItemsSource = new TabControl(); 
		//        tabItemsSource.SelectionChanged += TabItemsSource_SelectionChanged;
		//        return tabItemsSource; }
		//    set { tabItemsSource = value; OnPropertyChanged(nameof(TabItemsSource)); }
		//}

		//private void TabItemsSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
		//{
		//    if (sender is TabControl tabControl)
		//    {
		//        if (tabControl.SelectedItem is TabItem tabitem)
		//        {
		//            if (tabitem.Tag is MenuItems item)
		//            {
		//                Active_TabItem($"{item.Namespace} / {item.NameUsercontrol}");
		//            }
		//        }
		//    }
		//}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	class TitleInfo
	{
		public string Content { get; set; }
		public SolidColorBrush Background { get; set; }
		public SolidColorBrush Foreground { get; set; }
	}
}
