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
    /// Interaction logic for Canten_MilkBreadDistribution.xaml
    /// </summary>
    public partial class Canten_MilkBreadDistribution : UserControl
    {
        private readonly Canten_MilkBreadDistributionVM context;
        public Canten_MilkBreadDistribution()
        {
            InitializeComponent();
            context = new Canten_MilkBreadDistributionVM();
            DataContext = context;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && sender is TextBox tbx)
            {
                context.CardID = tbx.Text;
                tbx.SelectAll();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txt_notifi.Foreground = context.NotifiBrush;
        }
    }
}
