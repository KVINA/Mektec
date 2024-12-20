﻿using AppMMCV.ViewModels.Admin;
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

namespace AppMMCV.View.Admin
{
    /// <summary>
    /// Interaction logic for AppRolesUC.xaml
    /// </summary>
    public partial class AppRolesUC : UserControl
    {
        private AppRolesVM appRolesVM;
        public AppRolesUC()
        {
            InitializeComponent();            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            appRolesVM = new AppRolesVM();
            this.DataContext = appRolesVM;
        }
    }
}
