﻿using AppMMCV.ViewModels.HRM;
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
    /// Interaction logic for Canteen_DailyTotalUC.xaml
    /// </summary>
    public partial class Canteen_DailyTotalUC : UserControl
    {
        private readonly Canteen_DailyTotalVM dailyTotalVM;
        public Canteen_DailyTotalUC()
        {
            InitializeComponent();
            dailyTotalVM = new Canteen_DailyTotalVM();
            DataContext = dailyTotalVM;
        }
    }
}
