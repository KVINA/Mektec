using AppMMCV.Services;
using AppMMCV.ViewModels.HRM;
using LibraryHelper.Methord;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts;
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
using System.Diagnostics;
using LiveCharts.Wpf.Charts.Base;
using System.Reflection.Emit;
using System.Runtime.Remoting.Contexts;

namespace AppMMCV.View.HRM
{
    /// <summary>
    /// Interaction logic for Canteen_DailyReportsUC.xaml
    /// </summary>
    public partial class Canteen_DailyReportsUC : UserControl
    {
        private readonly Canteen_DailyReportsVM context;
        public Canteen_DailyReportsUC()
        {
            InitializeComponent();
            context = new Canteen_DailyReportsVM();
            DataContext = context;
        }

    }



}
