using AppMMCV.ViewModels.HRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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


namespace AppMMCV.View.HRM
{
	/// <summary>
	/// Interaction logic for Canteen_RegisterDailyMealUC.xaml
	/// </summary>
	public partial class Canteen_RegisterDailyMealUC : UserControl
	{
		public Canteen_RegisterDailyMealUC()
		{
			InitializeComponent();
			DataContext = new Canteen_RegisterDailyMealVM();
		}        
    }
}
