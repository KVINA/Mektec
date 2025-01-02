using AppMMCV.Services;
using AppMMCV.ViewModels.HRM;
using LibraryHelper.Methord;
using LibraryHelper.Models.HRM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace AppMMCV.View.HRM
{
    /// <summary>
    /// Interaction logic for Canteen_MealDistributionUC.xaml
    /// </summary>
    public partial class Canteen_MealDistributionUC : UserControl
    {
        private readonly Canteen_MealDistributionVM context;
        public Canteen_MealDistributionUC()
        {
            InitializeComponent();
            context = new Canteen_MealDistributionVM();
            DataContext = context;
        }

        private async void txt_cardID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string empoycode = txt_cardID.Text;                
                await Task.Run(() =>
                {
                    context.CheckInfoMealEmployee(empoycode);
                });                
                txt_cardID.Text = "";
            }
            
        }
    }
}
