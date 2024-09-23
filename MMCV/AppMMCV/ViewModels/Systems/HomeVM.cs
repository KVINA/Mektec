using AppMMCV.Json;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		private StackPanel mainMenu;
		public StackPanel MainMenu { get => mainMenu; set { mainMenu = value; OnPropertyChanged(nameof(MainMenu)); } }

		void LoadJsonMenu()
		{
			string Path_Json = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Json\\menu.json");
			string json = System.IO.File.ReadAllText(Path_Json);
			List<MenuJson> menus = JsonConvert.DeserializeObject<RootMenu>(json).MenuJson;
			MainMenu = CreateMainMenu(menus);
		}

		private StackPanel CreateMainMenu(List<MenuJson> menus)
		{
			if (menus != null && menus.Count > 0)
			{
				var stackPanel = new System.Windows.Controls.StackPanel();
				foreach (var menu in menus)
				{
					var icon = new MaterialDesignThemes.Wpf.PackIcon();
					icon.Kind = (PackIconKind)Enum.Parse(typeof(PackIconKind), menu.Kind);
					icon.VerticalAlignment = System.Windows.VerticalAlignment.Center;
					icon.Width = 30; icon.Height = 30;

					var textblock = new System.Windows.Controls.TextBlock();
					textblock.Text = menu.DisplayName;
					textblock.VerticalAlignment = System.Windows.VerticalAlignment.Center;
					textblock.FontWeight = FontWeights.Bold;
					textblock.FontSize = 14;

					var wrapPanel = new System.Windows.Controls.WrapPanel();
					wrapPanel.Children.Add(icon);
					wrapPanel.Children.Add(textblock);

					var listBox = new System.Windows.Controls.ListBox();
					listBox.Style = (Style)Application.Current.FindResource("MaterialDesignCardsListBox");
					listBox.Margin = new Thickness(40,0,20,0);

					if (menu.ListMenu != null && menu.ListMenu.Count > 0)
					{

						foreach (ListMenu item in menu.ListMenu)
						{
							var listboxItem = new System.Windows.Controls.ListBoxItem();
							listboxItem.Tag = item;
							listboxItem.Content = item.ParentMenu;
							listboxItem.ToolTip = item.Description;
							listboxItem.Cursor = Cursors.Hand;
							listboxItem.Style = (Style)Application.Current.FindResource("MaterialDesignNavigationListBoxItem");
							listBox.Items.Add(listboxItem);							
						}
					}
					//Expander
					var expander = new System.Windows.Controls.Expander() { Header = wrapPanel, Content = listBox};
					expander.ExpandDirection =  System.Windows.Controls.ExpandDirection.Down;
					expander.Margin = new Thickness(1);
					//Border
					var border = new System.Windows.Controls.Border() { Child = expander };
					border.BorderThickness = new System.Windows.Thickness(0, 0, 0.5, 0.5);
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

		public HomeVM()
		{
			LoadJsonMenu();
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
