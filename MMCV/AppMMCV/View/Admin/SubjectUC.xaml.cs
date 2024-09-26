using AppMMCV.Services;
using AppMMCV.ViewModels.Admin;
using LibraryHelper.Models;
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
        private int subject_id;
        private string typeSubmit;
        public bool IsLoadData = false;
        public SubjectUC(app_subject item = null)
        {
            InitializeComponent();
            if (item == null)
            {
                typeSubmit = "Add";
            }
            else
            {
                typeSubmit = "Edit";
                subject_id = item.Subject_id;
                txt_subjectName.Text = item.Subject_name;
                txt_subjectDescription.Text = item.Subject_description;
            }
        }        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string subject_name = txt_subjectName.Text;
            string subject_description = txt_subjectDescription.Text;
            if (string.IsNullOrEmpty(subject_name) || string.IsNullOrEmpty(subject_description))
            {
                MessageBox.Show("Please enter value.");
            }
            else
            {
                string query;
                var parameter = new List<object>();
                string username = DataService.User.username;
                parameter.Add(subject_name);
                parameter.Add(subject_description);
                parameter.Add(username);
                switch (typeSubmit)
                {
                    case "Add":
                        query = "Insert Into app_subject (subject_name,subject_description,create_by) values ( @name , @description , @username );";
                        break;
                    case "Edit":
                        query = "Update app_subject Set subject_name = @name ,subject_description = @description ,create_by = @username ,create_at = GetDate() Where subject_id = @id ;";
                        
                        parameter.Add(subject_id);
                        break;
                    default:
                        query = null;
                        break;
                }
                if (!string.IsNullOrEmpty(query))
                {
                    var res = SQLService.Method.ExcuteNonQuery(out string exception, SQLService.Server.SV68_HRM,query,parameter.ToArray());
                    if (string.IsNullOrEmpty(exception))
                    {
                        if(res > 0)
                        {
                            var dtContext = this.DataContext as MenuSettingsVM;
                            dtContext.IsSubject = true;
                            dtContext.Load_Data();
                        }
                        else
                        {
                            MessageBox.Show("Fail","Error");
                        }
                    }
                    else
                    {
                        MessageBox.Show(exception);
                    }
                }
                else
                {
                    MessageBox.Show("Error: submit type invalid.");
                }
            }
        }
    }
}
