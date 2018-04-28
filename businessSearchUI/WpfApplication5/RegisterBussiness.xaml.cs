﻿using System;
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
using Npgsql;

namespace WpfApplication5
{
    /// <summary>
    /// Interaction logic for RegisterBussiness.xaml
    /// </summary>
    public partial class RegisterBussiness : Window
    {
        public class BusinessUser
        {
            public string business_id { get; set; }
            public string name { get; set; }
            public string address { get; set; }
            public BusinessUser(string BusID)
            {
                using (var conn = new NpgsqlConnection("Host=localhost; Username=postgres; Password=6765; Database=Project"))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT * FROM businesstable WHERE business_id = '" + BusID + "'";
                        using (var reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            object[] row = new object[3];
                            reader.GetValues(row);
                            business_id = (string)row[0];
                            name = (string)row[1];
                            address = (string)row[2];

                        }



                    }

                }
            }
        }
        public RegisterBussiness()
        {
            InitializeComponent();
        }
        private string Buildcomm()
        {
            return "Host=localhost; Username=postgres; Password=6765; Database=Project";
        }
        public void addbusinessUser(string x)
        {
            using (var conn = new NpgsqlConnection("Host=localhost; Username=postgres; Password=6765; Database=Project"))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select distinct business_id from businesstable where name like'" + x + "%'";
                    //if (businessuserList != null)
                    //{
                    businessuserList.Items.Clear();
                    //}
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            businessuserList.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();
            }
        }



        private void businessnamechange_TextChanged(object sender, TextChangedEventArgs e)
        {
            addbusinessUser(businessnamechange.Text);
        }
        private string buildConnString()
        {
            return "Host=localhost; Username=postgres; Password=6765; Database = Project";
        }
        private void businessuserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (businessuserList.SelectedIndex != -1)
            {
                BusinessUser selecteduser = new BusinessUser((businessuserList.SelectedItem).ToString());
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "select name, address from businesstable where name like'" + selecteduser + "%'";
                        using (var reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                        }

                    }
                    conn.Close();
                }
                businessnameText.Text = selecteduser.name.ToString();
                addressText.Text = selecteduser.address.ToString();
            }
        }

        private void Log_in_Click(object sender, RoutedEventArgs e)
        {
            using (var conn = new NpgsqlConnection("Host=localhost; Username=postgres; Password=6765; Database=Project"))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(buildConnString()))
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select count(business_id) from businessuser where business_id='" + businessuserList.SelectedItem.ToString() + "or  business_name='" + businessnameText.Text;
                    using (var reader = cmd.ExecuteReader())
                    {
                        
                        if (reader.Read()==false )
                        {
                            cmd.CommandText = "insert into businessuser(business_id,business_name,password) values(business_id.text, name.text, password.text)";
                            cmd.ExecuteNonQuery();
                        }

                    }
                    conn.Close();
                }
            }

            BusinessLogin press = new BusinessLogin();
             press.Show();
        }
    }
}

