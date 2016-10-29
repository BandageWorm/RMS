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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace RMS
{
    public partial class MainWindow : Window
    {
        static string rms = "Database='RMS';DataSource = 'localhost'; User Id = 'root'; Password = 'root'; charset = 'utf8'";
        MySqlConnection con = new MySqlConnection(rms);
        public string account { get; set; }
        public string orderNo { get; set; }
        public string currentOrderNo { get; set; }
        public string totalBill { get; set; }
        MySqlCommand cmd = new MySqlCommand();

        public MainWindow()
        {
            InitializeComponent();
            try { con.Open(); }
            catch (MySqlException ex) { MessageBox.Show(ex.Message); this.Close(); }
            tbkAccount.DataContext = this;
            this.account = "W00001";
            dgBill.ItemsSource = showBill().DefaultView;
        }

        public DataTable showSearch(string keyword)
        {
            DataTable dt = new DataTable();
            string sql;
            if (this.rbtQC.IsChecked == true)
                sql = "select * from menu_item a inner join menu_category b on a.category_id=b.category_id where item_code='" + keyword + "'";
            else
                sql = "select * from menu_item a inner join menu_category b on a.category_id=b.category_id where item_id=" + keyword;
            try
            {
                DataSet ds = new DataSet();
                MySqlDataAdapter sda = new MySqlDataAdapter(sql, con);
                sda.Fill(ds, "keyword_item");
                dt = ds.Tables["keyword_item"];
            }
            catch (MySqlException ex)
            { MessageBox.Show(ex.Message); }
            return dt;
        }

        private void tbKeyword_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = this.tbKeyword.Text;
            if (keyword != "") dgSearch.ItemsSource = showSearch(keyword).DefaultView;
        }

        public void createOrder(string table)
        {
            try
            {
                cmd.Connection = con;
                cmd.CommandText = ("insert into `order` (staff_account,actual_payment,table_no) values('" + account + "',0," + table + ")");
                if (cmd.ExecuteNonQuery() == 1)
                {
                    cmd.CommandText = "select order_no,order_time from `order` order by order_time desc";
                    uint on = (uint)cmd.ExecuteScalar();
                    orderNo = on.ToString().PadLeft(6, '0');
                    tbkOrder.Text = this.orderNo;
                    currentOrderNo = orderNo;
                    tbkCurOrder.Text = this.currentOrderNo;
                    dgOrder.ItemsSource = showOrder(currentOrderNo).DefaultView;
                    dgBill.ItemsSource = showBill().DefaultView;
                    MessageBox.Show("Create order succeed!\n\nCurrent Order Number: " + orderNo);
                }
                else
                { MessageBox.Show("Create order failed!"); }
            }
            catch (MySqlException ex)
            { MessageBox.Show(ex.Message); }
        }

        //function for "create order" button
        private void btCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            string table = this.tbOrderTable.Text;
            if (table == "") MessageBox.Show("Please input value!");
            else createOrder(table);
            dgBill.ItemsSource = showBill().DefaultView;
        }

        public DataTable showOrder(string order)
        {
            DataTable dt = new DataTable();
            string sql = "select * from order_item inner join menu_item on order_item.item_id=menu_item.item_id where order_no=" + order;
            string sqlbill = "select format(sum(price*quantity),2) from order_item a inner join menu_item b on a.item_id=b.item_id where order_no=" + order;
            try
            {//Show DataGrid
                DataSet ds = new DataSet();
                MySqlDataAdapter sda = new MySqlDataAdapter(sql, con);
                sda.Fill(ds, "order_item");
                dt = ds.Tables["order_item"];
                //Show related info
                cmd.CommandText = sqlbill;
                cmd.Connection = con;
                if (cmd.ExecuteScalar() == DBNull.Value) totalBill = "0.00";
                else totalBill = (string)cmd.ExecuteScalar();
                orderNo = order.PadLeft(6, '0');
                tbkOrder.Text = this.orderNo;
                tbkTotalBill.Text = this.totalBill;
            }
            catch (MySqlException ex)
            { MessageBox.Show(ex.Message); }
            return dt;
        }

        public void orderItem(string item, string ammount)
        {
            try
            {
                cmd.Connection = con;
                if (this.rbtID.IsChecked == true)
                    cmd.CommandText = "insert into `order_item` (quantity, category_id, item_id, order_no) values(" + ammount + ", (select category_id from menu_item where item_id = " + item + ")," + item + "," + orderNo + ")";
                else
                    cmd.CommandText = "insert into `order_item` (quantity, category_id, item_id, order_no) values(" + ammount + ", (select category_id from menu_item where item_code = '" + item + "'), (select item_id from menu_item where item_code = '" + item + "')," + orderNo + ")";
                if (cmd.ExecuteNonQuery() != 1)
                { MessageBox.Show("Order failed!"); }
            }
            catch (MySqlException ex)
            { MessageBox.Show(ex.Message); }
        }

        private void order_Click(object sender, RoutedEventArgs e)
        {
            string item = this.tbKeyword.Text;
            string ammount = this.tbAmmount.Text;
            if (item == "" || ammount == "") MessageBox.Show("Please input value!");
            else
            {
                orderItem(item, ammount);
                dgOrder.ItemsSource = showOrder(orderNo).DefaultView;
                dgBill.ItemsSource = showBill().DefaultView;
            }
        }

        public void cancelItem(string keyword)
        {

            cmd.Connection = con;
            if (this.rbtID.IsChecked == true)
                cmd.CommandText = "DELETE FROM order_item WHERE item_id=" + keyword + " and category_id =(select category_id from menu_item where item_id=" + keyword + ") and order_no='" + orderNo + "'";
            else
                cmd.CommandText = "DELETE FROM order_item WHERE item_code=" + keyword + " and category_id =(select category_id from menu_item where item_code=" + keyword + ") and order_no='" + orderNo + "'";
            try { if (cmd.ExecuteNonQuery() == 1) MessageBox.Show("Order has been canceled!"); }
            catch (MySqlException ex) { MessageBox.Show(ex.Message); }
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            string keyword = tbKeyword.Text;
            if (keyword == "") { MessageBox.Show("Please input keyword!"); return; }
            cancelItem(keyword);
            dgOrder.ItemsSource = showOrder(orderNo).DefaultView;
        }

        private void btShowOrder_Click(object sender, RoutedEventArgs e)
        {
            string orderNo = tbShowOrder.Text;
            if (orderNo == "") { MessageBox.Show("Please input order number!"); return; }
            dgOrder.ItemsSource = showOrder(orderNo).DefaultView;
        }

        private void btOrdering_Click(object sender, RoutedEventArgs e)
        {
            if (currentOrderNo == null) { MessageBox.Show("No current order!"); return; }
            dgOrder.ItemsSource = showOrder(currentOrderNo).DefaultView;
        }

        public DataTable showBill()
        {
            DataTable dt = new DataTable();
            string sql = "select c.order_no,staff_name,bill,actual_payment,table_no,order_time from" +
            "(select * from `order` inner join staff on `order`.staff_account = staff.account) c " +
            "left join (select order_no, format(sum(price * quantity), 2) as bill from order_item" +
            " a inner join menu_item b on a.item_id = b.item_id group by order_no) d on c.order_no" +
            " = d.order_no order by c.order_no asc";
            try
            {//Show DataGrid
                DataSet ds = new DataSet();
                MySqlDataAdapter sda = new MySqlDataAdapter(sql, con);
                sda.Fill(ds, "order_item");
                dt = ds.Tables["order_item"];
            }
            catch (MySqlException ex)
            { MessageBox.Show(ex.Message); }
            return dt;
        }

        //修改密码

        //日月年报告

    }
}
