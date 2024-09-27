using AppMMCV.Json;
using AppMMCV.Services;
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
        public static readonly DependencyProperty MainContentProperty =
          DependencyProperty.Register("MainContent", typeof(UserControl), typeof(UserControl), null);
        public UserControl MainContent
        {
            get { return (UserControl)GetValue(MainContentProperty); }
            set { SetValue(MainContentProperty, value); }
        }

        private StackPanel mainMenu;
        public StackPanel MainMenu { get => mainMenu; set { mainMenu = value; } }
        public HomeUC()
        {
            InitializeComponent();
            this.DataContext = this;
            LoadJsonMenu();
        }
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
                    listBox.Margin = new Thickness(40, 0, 20, 0);

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
                    var expander = new Expander() { Header = wrapPanel, Content = listBox };
                    expander.ExpandDirection = ExpandDirection.Down;
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
            MainContent = new MainMenuUC() { DataContext = menuVM };
        }
    }
}
