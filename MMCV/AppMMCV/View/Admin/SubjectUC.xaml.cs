using AppMMCV.Services;
using AppMMCV.ViewModels.Admin;
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
    /// Interaction logic for SubjectUC.xaml
    /// </summary>
    public partial class SubjectUC : UserControl
    {
        private SubjectVM subjectVM;
        public SubjectUC()
        {
            InitializeComponent();            
        }

        public void LoadSubject(App_subject item = null)
        {
            subjectVM = this.DataContext as SubjectVM;
            if (item == null)
            {
                subjectVM.TypeSubmit = "Add";
                subjectVM.Subject_id = 0;
                subjectVM.Subject_name = "";
                subjectVM.Subject_icon = "";
            }
            else
            {
                subjectVM.TypeSubmit = "Edit";
                subjectVM.Subject_id = item.Subject_id;
                subjectVM.Subject_name = item.Subject_name;
                subjectVM.Subject_icon = item.Subject_icon;                
            }
        }

        private void Button_Click_Submit(object sender, RoutedEventArgs e)
        {
            subjectVM.SubmitSubject?.Invoke();
        }

    }
}
