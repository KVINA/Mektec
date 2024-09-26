using AppMMCV.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace AppMMCV.ViewModels.Systems
{
    internal class PermissionVM : INotifyPropertyChanged
    {
        public PermissionVM()
        {
            ParentID = DataService.GlobalVM.ParentMenuActive.ParentID;
            var listitems = DataService.GlobalVM.ParentMenuActive.ListItems;
            if (listitems.Count > 0)
            {
                foreach (var item in listitems)
                {
                    var cbk = new CheckBox() { Content = item.Header };
                    CheckBoxes.Add(cbk);
                }
            }
            CommandGetCheckBox = new RelayCommand(() => GetCheckBox(true));
            CommandSubmit = new RelayCommand(Submit);
        }

        void ResetCheckBox()
        {

        }

        /// <summary>
        /// Xử lý sự kiện Add, Edit
        /// </summary>
        private void Submit()
        {
            string query;
            switch (Status)
            {
                case "Add":
                    query = "Insert Into app_permission values";
                    break;
                case "Edit":
                    query = "Insert Into";
                    break;
                default:
                    return;
            }

            var res = SQLService.Method.ExcuteNonQuery(out string exception, SQLService.Server.SV68_HRM, query);
            if (string.IsNullOrEmpty(exception))
            {
                if (res > 0) MessageBox.Show("Completed!", "Infomation");
                else MessageBox.Show("Fail!", "Infomation");
            }
            else MessageBox.Show(exception, "Error");
        }

        /// <summary>
        /// Lấy danh sách check box được chọn
        /// </summary>
        /// <param name="value"></param>
        string GetCheckBox(bool value)
        {
            var list = CheckBoxes.Where(c => c.IsChecked == value).Select(c => c.Content.ToString()).ToList();
            var str = string.Join(",", list);
            return str;
        }

        private string username;
        private string status;
        public ICommand CommandGetCheckBox { get; }
        public ICommand CommandSubmit { get; }

        private string ParentID;

        private ObservableCollection<CheckBox> checkBoxes;
        public ObservableCollection<CheckBox> CheckBoxes { get => checkBoxes; set { checkBoxes = value; OnPropertyChanged(nameof(CheckBoxes)); } }
        public string Status { get => status; set { status = value; OnPropertyChanged(nameof(Status)); } }
        public string Username { get => username; set { username = value; OnPropertyChanged(nameof(Username)); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
