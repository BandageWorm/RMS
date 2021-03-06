﻿using System.Windows;
using MySql.Data.MySqlClient;
using System.Data;

namespace RMS
{
    public partial class MenuWindow : Window
    {
        static string rms = @"Database='RMS';DataSource = 'localhost';User Id = 'root'; 
                           Password = 'root'; charset = 'utf8'";
        MySqlConnection con = new MySqlConnection(rms);
        MySqlCommand cmd = new MySqlCommand();

        public MenuWindow()
        {
            try { con.Open(); }
            catch (MySqlException ex) { MessageBox.Show(ex.Message); this.Close(); }
            InitializeComponent();
            dgItem.ItemsSource = showItem().DefaultView;
            dgCategory.ItemsSource = showCategory().DefaultView;
            dgTable.ItemsSource = showTable().DefaultView;
        }

        public DataTable showItem()
        {
            DataTable dt = new DataTable();
            string sql = @"select * from menu_item m left join menu_category c 
                        on m.category_id=c.category_id order by item_id asc";
            try
            {   //Show DataGrid
                DataSet ds = new DataSet();
                MySqlDataAdapter sda = new MySqlDataAdapter(sql, con);
                sda.Fill(ds, "showItem");
                dt = ds.Tables["showItem"];
            }
            catch (MySqlException ex)
            { MessageBox.Show(ex.Message); }
            return dt;
        }

        public DataTable showCategory()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT * FROM menu_category";
            try
            {   //Show DataGrid
                DataSet ds = new DataSet();
                MySqlDataAdapter sda = new MySqlDataAdapter(sql, con);
                sda.Fill(ds, "showCategory");
                dt = ds.Tables["showCategory"];
            }
            catch (MySqlException ex)
            { MessageBox.Show(ex.Message); }
            return dt;
        }

        public DataTable showTable()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT * FROM `table`";
            try
            {   //Show DataGrid
                DataSet ds = new DataSet();
                MySqlDataAdapter sda = new MySqlDataAdapter(sql, con);
                sda.Fill(ds, "showTable");
                dt = ds.Tables["showTable"];
            }
            catch (MySqlException ex)
            { MessageBox.Show(ex.Message); }
            return dt;
        }

        private void btDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            string itemID = this.tbItem.Text;
            if(itemID==null||itemID=="") { MessageBox.Show("Please input item ID!"); return; }
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "DELETE FROM menu_item WHERE item_id="+itemID;
                if(cmd.ExecuteNonQuery()!=1) MessageBox.Show("Failed!");
            }
            catch(MySqlException ex) { MessageBox.Show(ex.Message); }
            dgItem.ItemsSource = showItem().DefaultView;
        }

        private void btDeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            string categoryID = this.tbCategoryID.Text;
            if (categoryID == null||categoryID=="") { MessageBox.Show("Please input category ID!"); return; }
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "DELETE FROM menu_category WHERE category_id=" + categoryID;
                if (cmd.ExecuteNonQuery() != 1) MessageBox.Show("Failed!");
            }
            catch (MySqlException ex) { MessageBox.Show(ex.Message); }
            dgCategory.ItemsSource = showCategory().DefaultView;
            dgItem.ItemsSource = showItem().DefaultView;
        }

        private void btAddItem_Click(object sender, RoutedEventArgs e)
        {
            string name = tbItemName.Text;
            string QC = tbItemQC.Text;
            string price = tbItemPrice.Text;
            string cid = tbItemCategory.Text;
            string info = tbItemInfo.Text;
            if(name == null||QC == null ||price == null) { MessageBox.Show("Please input values!"); return; }
            if (info == null) info = "";
            if (cid == null) cid = "";
            try
            {
                cmd.Connection = con;
                cmd.CommandText = @"INSERT INTO menu_item (item_name, price, item_code, item_info, 
                category_id) VALUES ('"+name+"', '"+price+"', '"+QC+"', '"+info+"', '"+cid+"')";
                if (cmd.ExecuteNonQuery() != 1) MessageBox.Show("Failed!");
            }
            catch (MySqlException ex) { MessageBox.Show(ex.Message); }
            dgItem.ItemsSource = showItem().DefaultView;
        }

        private void btAddCategory_Click(object sender, RoutedEventArgs e)
        {
            string name = tbCategory.Text;
            string info = tbCategoryInfo.Text;
            if (name == null) { MessageBox.Show("Please input values!"); return; }
            if (info == null) info = "";
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "INSERT INTO menu_category (category, category_info) VALUES ('"+name+"', '"+info+"')";
                if (cmd.ExecuteNonQuery() != 1) MessageBox.Show("Failed!");
            }
            catch (MySqlException ex) { MessageBox.Show(ex.Message); }
            dgCategory.ItemsSource = showCategory().DefaultView;
        }

        private void btDeleteTable_Click(object sender, RoutedEventArgs e)
        {
            string tableNo = this.tbTableNo.Text;
            if(tableNo==null||tableNo=="") { MessageBox.Show("Please input table No!"); return; }
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "DELETE FROM `table` WHERE table_no =" + tableNo;
                if (cmd.ExecuteNonQuery() != 1) MessageBox.Show("Failed!");
            }
            catch (MySqlException ex) { MessageBox.Show(ex.Message); }
            dgTable.ItemsSource = showTable().DefaultView;

        }

        private void btAddTable_Click(object sender, RoutedEventArgs e)
        {
            string info = tbTableInfo.Text;
            if (info == null) info = "";
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "INSERT INTO `table` (`table_info`) VALUES ('" + info + "')";
                if (cmd.ExecuteNonQuery() != 1) MessageBox.Show("Failed!");
            }
            catch (MySqlException ex) { MessageBox.Show(ex.Message); }
            dgTable.ItemsSource = showTable().DefaultView;
        }
    }
}
