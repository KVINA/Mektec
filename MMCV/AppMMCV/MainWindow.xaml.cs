﻿using AppMMCV.Services;
using AppMMCV.View.Admin;
using AppMMCV.View.Systems;
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
using System.Windows.Shapes;

namespace AppMMCV
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static int FromHeight = 0;
		public MainWindow()
		{
			InitializeComponent();
			this.DataContext = DataService.GlobalVM;

		}
	}
}
