using AppMMCV.Services;
using AppMMCV.ViewModels.HRM;
using LibraryHelper.Models.HRM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Interaction logic for Canteen_MasterRegisterMealsUC.xaml
    /// </summary>
    public partial class Canteen_MasterRegisterMealsUC : UserControl
    {
        //Khai báo đăng ký một DependencyProperty
        private Canteen_MasterRegisterMealsVM dtcontext;

        public Canteen_MasterRegisterMealsUC()
        {
            InitializeComponent();
            dtcontext = new Canteen_MasterRegisterMealsVM();
            DataContext = dtcontext;
        }

        private void DepartmentTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is ViewModels.HRM.CompanyStructure selectedDepartment)
            {
                dtcontext.SelectedCompanyStructure = selectedDepartment;                
            }
        }
    }
}
