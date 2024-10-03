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
					string nameuc = usercontroller.Split('.').Last();
					if (DIC_USERCONTROLS.ContainsKey(nameuc))
					{
						Active_TabItem(nameuc);
					}
					else
					{
						var tabitem = new TabItem() { Tag = nameuc};
						tabitem.Header = TabItem_Header(nameuc);
						var content = ControlUC(out string exception, usercontroller);
						if (string.IsNullOrEmpty(exception))
						{
							content.Margin = new Thickness(0, 10, 0, 0);
							tabitem.Content = content;
							//tabitem.Tag = item;
							DIC_USERCONTROLS.Add(nameuc, tabitem);
							tab_control.Items.Add(DIC_USERCONTROLS[nameuc]);
							Active_TabItem(nameuc);
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
		Grid TabItem_Header(string header)
		{
			var grid = new Grid();
			grid.ColumnDefinitions.Add(new ColumnDefinition());
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength() });

			var textblock = new TextBlock() { Text = header, Margin = new Thickness(2), FontFamily = new FontFamily("Segoe UI"), Foreground = Brushes.Teal, FontWeight = FontWeights.DemiBold, FontSize = 12 };
			grid.Children.Add(textblock);

			var border = new Border() { Cursor = Cursors.Hand, Tag = header};
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
				string nameuc = border.Tag.ToString();
				tab_control.Items.Remove(DIC_USERCONTROLS[nameuc]);
				DIC_USERCONTROLS.Remove(nameuc);
				Active_TabItem();
				GC.Collect();
			}
		}

		public void Active_TabItem(string nameuc = null)
		{
			if (!string.IsNullOrEmpty(nameuc))
			{
				if (!string.IsNullOrEmpty(nameuc))
				{
					tab_control.SelectedItem = DIC_USERCONTROLS[nameuc];
				}
			}
		}
	}

}
