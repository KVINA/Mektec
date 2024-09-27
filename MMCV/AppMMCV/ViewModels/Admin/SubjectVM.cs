using AppMMCV.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppMMCV.ViewModels.Admin
{
    internal class SubjectVM : INotifyPropertyChanged
    {
        public Action SubmitSubject;
        private string typeSubmit;
        private string subject_description;
        private string subject_name;
        private int subject_id;
        private bool isSubject;

        public bool IsSubject { get => isSubject; set { isSubject = value; OnPropertyChanged(nameof(IsSubject)); } }
        public string Subject_description { get => subject_description; set { subject_description = value; OnPropertyChanged(nameof(Subject_description)); } }
        public string Subject_name { get => subject_name; set { subject_name = value; OnPropertyChanged(nameof(Subject_name)); } }
        public int Subject_id { get => subject_id; set { subject_id = value; OnPropertyChanged(nameof(Subject_id)); } }
        public string TypeSubmit { get => typeSubmit; set { typeSubmit = value; OnPropertyChanged(nameof(TypeSubmit)); } }

        /// <summary>
        /// Khởi tạo INotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
