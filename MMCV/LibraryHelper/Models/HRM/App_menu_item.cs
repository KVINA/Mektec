﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryHelper.Models.HRM
{
    public class List_item_id
    {
        private int item_id;
        private string item_name;
        private bool isChecked;
        public int Item_id { get => item_id; set => item_id = value; }
        public string Item_name { get => item_name; set => item_name = value; }
        public bool IsChecked { get => isChecked; set => isChecked = value; }
        public List_item_id(DataRow row)
        {
            this.item_id = (int)row["item_id"];
            this.item_name = row["item_name"].ToString();
        }
    }
    public class App_menu_item
    {
        public App_menu_item() { }
        public App_menu_item(DataRow row)
        {
            this.item_id = (int)row["item_id"];
            this.item_group = row["item_group"].ToString();
            this.item_name = row["item_name"].ToString();
            this.item_index = (int)row["item_index"];
            this.item_controller = row["item_controller"].ToString();
            this.item_icon = row["item_icon"].ToString();
            this.item_header = row["item_header"].ToString();
            this.menu_id = (int)row["menu_id"];
            this.create_at = (DateTime)row["create_at"];
            this.create_by = row["create_by"].ToString();
        }


        private int item_id;
        private string item_group;
        private string item_name;
        private int item_index;
        private string item_controller;
        private string item_icon;
        private string item_header;
        private int menu_id;
        private DateTime create_at;
        private string create_by;

        public int Item_id { get => item_id; set => item_id = value; }
        public string Item_group { get => item_group; set => item_group = value; }
        public string Item_name { get => item_name; set => item_name = value; }
        public int Item_index { get => item_index; set => item_index = value; }
        public string Item_controller { get => item_controller; set => item_controller = value; }
        public string Item_icon { get => item_icon; set => item_icon = value; }
        public string Item_header { get => item_header; set => item_header = value; }
        public int Menu_id { get => menu_id; set => menu_id = value; }
        public DateTime Create_at { get => create_at; set => create_at = value; }
        public string Create_by { get => create_by; set => create_by = value; }
    }
}