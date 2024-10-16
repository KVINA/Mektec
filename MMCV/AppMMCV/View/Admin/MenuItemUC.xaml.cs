﻿using AppMMCV.ViewModels.Admin;
using LibraryHelper.Models.HRM;
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
    /// Interaction logic for MenuItemUC.xaml
    /// </summary>
    public partial class MenuItemUC : UserControl
    {
        private MenuItemVM menuItemVM;
        public MenuItemUC()
        {
            InitializeComponent();
        }
        public void LoadMenuItem(App_menu_item item,string type) 
        { 
            menuItemVM = this.DataContext as MenuItemVM;
            menuItemVM.MenuItemInfo = item;
            menuItemVM.TypeSubmit = type;
        }
    }
}
