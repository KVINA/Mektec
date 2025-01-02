using AppMMCV.ViewModels.HRM;
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

namespace AppMMCV.View.HRM
{
    /// <summary>
    /// Interaction logic for Canteen_ConfirmDailyMeals.xaml
    /// </summary>
    public partial class Canteen_ConfirmDailyMeals : UserControl
    {
        private readonly Canteen_ConfirmDailyMealsVM context;
        public Canteen_ConfirmDailyMeals()
        {
            InitializeComponent();
            context = new Canteen_ConfirmDailyMealsVM();
            DataContext = context;
        }
    }
}
