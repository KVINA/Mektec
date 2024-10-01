using AppMMCV.View.Admin;
using AppMMCV.ViewModels.Systems;
using System;
using System.Collections.Generic;
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
		private Dictionary<string, TabItem> DIC_USERCONTROLS = new Dictionary<string, TabItem>(); 

		private MainMenuVM mainMenuVM;
		public MainMenuUC(int menu_id)
		{
			InitializeComponent();
			mainMenuVM = new MainMenuVM(menu_id);
			this.DataContext = mainMenuVM;
		}

		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			if (sender is MenuItem menuitem)
			{
				if (menuitem.Tag != null)
				{
					string usercontroller = menuitem.Tag.ToString();
					var header = menuitem.Header.ToString();
					if (DIC_USERCONTROLS.ContainsKey(header))
					{
						Active_TabItem(header);
					}
					else
					{
						var tabitem = new TabItem();
						tabitem.Header = TabItem_Header(header, tabitem);
						var content = ControlUC(out string exception, usercontroller);
						if (string.IsNullOrEmpty(exception))
						{
							content.Margin = new Thickness(0, 10, 0, 0);
							tabitem.Content = content;
							//tabitem.Tag = item;
							DIC_USERCONTROLS.Add(header, tabitem);
							tab_control.Items.Add(tabitem);
							//Active_TabItem(usercontroller);
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
		Grid TabItem_Header(string header, TabItem tabitem)
		{
			var grid = new Grid();
			grid.ColumnDefinitions.Add(new ColumnDefinition());
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength() });

			var textblock = new TextBlock() { Text = header, Margin = new Thickness(2), FontFamily = new FontFamily("Segoe UI"), Foreground = Brushes.Teal, FontWeight = FontWeights.DemiBold, FontSize = 12 };
			grid.Children.Add(textblock);

			var border = new Border() { Cursor = Cursors.Hand, DataContext = tabitem };
			border.Child = new TextBlock() { Text = "❌", Foreground = Brushes.Red, Background = Brushes.AliceBlue, Margin = new Thickness(2) };
			//border.MouseLeftButtonUp += Border_MouseLeftButtonUp;

			grid.Children.Add(border);
			Grid.SetColumn(border, 1);
			return grid;
		}
		public void Active_TabItem(string addressmenu = null)
		{
			if (!string.IsNullOrEmpty(addressmenu))
			{
				string name = addressmenu.Split('/')[1].Trim();
				if (!string.IsNullOrEmpty(name))
				{
					//TitleInfo.Content = addressmenu;
					//TitleInfo.Background = Brushes.Teal;
					//TitleInfo.Foreground = Brushes.White;
					//TabItemsSource.SelectedItem = DIC_USERCONTROLS[name];
				}
				else
				{
					//TitleInfo.Content = null;
					//TitleInfo.Background = null;
					//TitleInfo.Foreground = null;
				}
			}
			else
			{
				//TitleInfo.Content = null;
				//TitleInfo.Background = null;
				//TitleInfo.Foreground = null;
			}
		}
	}

}
