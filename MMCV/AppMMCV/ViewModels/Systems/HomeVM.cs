using AppMMCV.Json;
using AppMMCV.Services;
using AppMMCV.View.Systems;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
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
using System.Windows.Input;
using System.Windows.Media;

namespace AppMMCV.ViewModels.Systems
{
    internal class HomeVM : INotifyPropertyChanged
    {
        /*
		private StackPanel mainMenu;
        private UserControl mainContent;
		private string controlSource;
        public StackPanel MainMenu { get => mainMenu; set { mainMenu = value; OnPropertyChanged(nameof(MainMenu)); } }
        public UserControl MainContent { get => mainContent; set { mainContent = value; OnPropertyChanged(nameof(MainContent)); }  }
        public string ControlSource { get => controlSource; set { controlSource = value; OnPropertyChanged(nameof(ControlSource)); } }

		/// <summary>
		/// Read menu json
		/// </summary>
        void LoadJsonMenu()
		{
			string Path_Json = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Json\\menu.json");
			string json = System.IO.File.ReadAllText(Path_Json);
			List<MenuJson> menus = JsonConvert.DeserializeObject<RootMenu>(json).MenuJson;
			MainMenu = CreateMainMenu(menus);
		}

		/// <summary>
		/// Create Main Menu 
		/// </summary>
		/// <param name="menus"></param>
		/// <returns></returns>
		private StackPanel CreateMainMenu(List<MenuJson> menus)
		{
			if (menus != null && menus.Count > 0)
			{
				var stackPanel = new StackPanel();
				foreach (var menu in menus)
				{
					var icon = new MaterialDesignThemes.Wpf.PackIcon();
					icon.Kind = (PackIconKind)Enum.Parse(typeof(PackIconKind), menu.Kind);
					icon.VerticalAlignment = System.Windows.VerticalAlignment.Center;
					icon.Width = 30; icon.Height = 30;

					var textblock = new TextBlock();
					textblock.Text = menu.DisplayName;
					textblock.VerticalAlignment = System.Windows.VerticalAlignment.Center;
					textblock.FontWeight = FontWeights.Bold;
					textblock.FontSize = 14;

					var wrapPanel = new WrapPanel();
					wrapPanel.Children.Add(icon);
					wrapPanel.Children.Add(textblock);

					var listBox = new ListBox();
					listBox.Style = (Style)Application.Current.FindResource("MaterialDesignCardsListBox");
					listBox.Margin = new Thickness(40,0,20,0);

					if (menu.ListMenu != null && menu.ListMenu.Count > 0)
					{

						foreach (ListMenu item in menu.ListMenu)
						{
							var listboxItem = new ListBoxItem();
							listboxItem.Tag = item;
							listboxItem.Content = item.ParentMenu;
							listboxItem.ToolTip = item.Description;
							listboxItem.Cursor = Cursors.Hand;
							listboxItem.Style = (Style)Application.Current.FindResource("MaterialDesignNavigationListBoxItem");
                            listboxItem.PreviewMouseLeftButtonDown += ListboxItem_PreviewMouseLeftButtonDown;


                            listBox.Items.Add(listboxItem);							
						}
					}
					//Expander
					var expander = new Expander() { Header = wrapPanel, Content = listBox};
					expander.ExpandDirection =  ExpandDirection.Down;
					expander.Margin = new Thickness(1);
                    expander.Expanded += Expander_Expanded;	
					//Border
					var border = new Border() { Child = expander };
					border.BorderThickness = new Thickness(0, 0, 0.5, 0.5);
					border.BorderBrush = Brushes.Gray;

					stackPanel.Children.Add(border);
				}
				return stackPanel;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Close menu is not active
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            foreach (var item in MainMenu.Children.OfType<Border>())
            {
                var myExpender = item.Child as Expander;
                if (myExpender != (sender as Expander))
                {
                    myExpender.IsExpanded = false;
                }
            }
        }

		/// <summary>
		/// Select MenuItem
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void ListboxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
			var control = sender as ListBoxItem;
			var listmenu = control.Tag as ListMenu;
			var children = listmenu.ChildrenMenu;
			DataService.GlobalVM.ParentMenuActive.ParentID = listmenu.ParentID;

			var listItem = new List<MenuItems>();
			foreach (var parent in children) foreach (var item in parent.MenuItems) listItem.Add(item);
            DataService.GlobalVM.ParentMenuActive.ListItems = listItem;

            var list = children.Select(s => s.MenuItems).ToList();
            var menuVM = new MainMenuVM(children);
            MainContent = new MainMenuUC() { DataContext = menuVM};			
		}
        

		*/

        ObservableCollection<MenuInfo> menuInfos;

        public ICommand Command_ExecuteActiveMenu;
        public ObservableCollection<MenuInfo> MenuInfos
        {
            get { if (menuInfos == null) menuInfos = new ObservableCollection<MenuInfo>(); return menuInfos; }
            set { menuInfos = value; OnPropertyChanged(nameof(MenuInfos)); }
        }

        public HomeVM()
        {
            Load_Permision();
        }

        void Load_Permision()
        {
            MenuInfos.Clear();
            if (DataService.UserInfo != null)
            {
                string employee_code = DataService.UserInfo.username;
                string query = "Select C.subject_id,subject_name,subject_icon,B.menu_id,B.menu_name,B.menu_description " +
                "from app_roles A " +
                "Inner join app_menu B On B.menu_id = A.menu_id " +
                "Inner join app_subject C On C.subject_id = B.subject_id " +
                $"Where employee_code = '{employee_code}' and access = 1 order by subject_id,menu_index";

                var data = SQLService.Method.ExecuteQuery(out string exception, SQLService.Server.SV68_HRM, query);
                if (string.IsNullOrEmpty(exception))
                {
                    if (data != null && data.Rows.Count > 0)
                    {
                        foreach (DataRow row in data.Rows)
                        {
                            int subject_id = (int)row["subject_id"];
                            string subject_name = row["subject_name"].ToString();
                            string subject_icon = row["subject_icon"].ToString();
                            int menu_id = (int)row["menu_id"];
                            string menu_name = row["menu_name"].ToString();
                            string menu_description = row["menu_description"].ToString();
                            MenuInfo item;
                            if (MenuInfos.Any(m => m.Subject_id == subject_id))
                            {
                                item = MenuInfos.Where(i => i.Subject_id == subject_id).FirstOrDefault();
                            }
                            else
                            {
                                item = new MenuInfo()
                                {
                                    Subject_id = subject_id,
                                    Subject_name = subject_name,
                                    Subject_icon = subject_icon,
                                    ChildsInfo = new ObservableCollection<MenuChildInfo>()
                                };
                                MenuInfos.Add(item);
                            }
                            item.ChildsInfo.Add(new MenuChildInfo()
                            {
                                Menu_id = menu_id,
                                Menu_name = menu_name,
                                Menu_description = menu_description
                            });
                        }
                    }
                }
            }            
        }

		/// <summary>
		/// Close menu is not active
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		

		public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class MenuInfo : INotifyPropertyChanged
    {
        private int subject_id;
        private string subject_name;
        private string subject_icon;
        private bool isOpen;
        private ObservableCollection<MenuChildInfo> childsInfo;
        public int Subject_id { get => subject_id; set { subject_id = value; OnPropertyChanged(nameof(Subject_id)); } }
        public string Subject_name { get => subject_name; set { subject_name = value; OnPropertyChanged(nameof(Subject_name)); } }
        public string Subject_icon { get => subject_icon; set { subject_icon = value; OnPropertyChanged(nameof(subject_icon)); } }
        public ObservableCollection<MenuChildInfo> ChildsInfo { get => childsInfo; set { childsInfo = value; OnPropertyChanged(nameof(ChildsInfo)); } }
		public bool IsOpen { get => isOpen; set { isOpen = value; OnPropertyChanged(nameof(IsOpen)); }  }

		public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class MenuChildInfo : INotifyPropertyChanged
    {
        private int menu_id;
        private string menu_name;
        private int menu_index;
        private string menu_description;

        public int Menu_id { get => menu_id; set { menu_id = value; OnPropertyChanged(nameof(Menu_id)); } }
        public string Menu_name { get => menu_name; set { menu_name = value; OnPropertyChanged(nameof(Menu_name)); } }
        public int Menu_index { get => menu_index; set { menu_index = value; OnPropertyChanged(nameof(Menu_index)); } }
        public string Menu_description { get => menu_description; set { menu_description = value; OnPropertyChanged(nameof(Menu_description)); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
