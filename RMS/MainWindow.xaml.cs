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
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace RMS
{
    public partial class MainWindow : Window
    {
        public string account { get; set; }
        static string rms = "Database='RMS';DataSource = 'localhost'; User Id = 'root'; Password = 'root'; charset = 'utf8'";
        public string currentOrderNo { get; set; }
        MySqlConnection con = new MySqlConnection(rms);

        public MainWindow()
        {
            InitializeComponent();
            try { con.Open(); }
            catch (MySqlException ex) { MessageBox.Show(ex.Message); this.Close(); }
            tbkAccount.DataContext = this;
            this.account = "W00001";
        }

        public DataTable searchItem(string search)
        {
            DataTable dt = new DataTable();
            string sql;
            if (this.rbtQC.IsChecked == true)
            { sql = "select * from menu_item a inner join menu_category b on a.category_id=b.category_id where item_code='" + search + "'"; }
            else
            { sql = "select * from menu_item a inner join menu_category b on a.category_id=b.category_id where item_id=" + search; }
            try
            {
                DataSet ds = new DataSet();
                MySqlDataAdapter sda = new MySqlDataAdapter(sql, con);
                sda.Fill(ds, "search_item");
                dt = ds.Tables["search_item"];
            }
            catch (MySqlException ex)
            { MessageBox.Show(ex.Message); }
            return dt;
        }

        private void search_Change(object sender, TextChangedEventArgs e)
        {
            string search = this.tbSearch.Text;
            if (search != "")
            { search_item.ItemsSource = searchItem(search).DefaultView; }
        }

        public void createOrder(string table)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;
                cmd.CommandText = ("insert into `order` (staff_account,actual_payment,table_no) values('" + account + "',0," + table + ")");
                if (cmd.ExecuteNonQuery() == 1)
                {
                    cmd.CommandText = "select order_no,order_time from `order` order by order_time desc";
                    uint on = (uint)cmd.ExecuteScalar();
                    currentOrderNo = on.ToString().PadLeft(6, '0');
                    MessageBox.Show("Create order succeed!\n\nCurrent Order Number: "+ currentOrderNo);
                    tbkOrder.Text = this.currentOrderNo;
                }
                else
                { MessageBox.Show("Create order failed!"); }
            }catch(MySqlException ex)
            { MessageBox.Show(ex.Message); }
        }

        private void btCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            string table = this.tbOrderTable.Text;
            if (table == "")
            { MessageBox.Show("Please input value!"); }
            else
            { createOrder(table); }
        }

        public DataTable showOrder(string orderNo)
        {
            DataTable dt = new DataTable();
            string sql = "select * from order_item inner join menu_item on order_item.item_id=menu_item.item_id where order_no=" + orderNo;
            try
            {
                DataSet ds = new DataSet();
                MySqlDataAdapter sda = new MySqlDataAdapter(sql, con);
                sda.Fill(ds, "order_item");
                dt = ds.Tables["order_item"];
            }
            catch (MySqlException ex)
            { MessageBox.Show(ex.Message); }
            return dt;
        }

        public void orderItem(string item, string ammount)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;
                if (this.rbtID.IsChecked == true)
                { cmd.CommandText = "insert into `order_item` (quantity, category_id, item_id, order_no) values(" + ammount + ", (select category_id from menu_item where item_id = " + item + ")," + item + "," + currentOrderNo + ")"; }
                else
                { cmd.CommandText = "insert into `order_item` (quantity, category_id, item_id, order_no) values(" + ammount + ", (select category_id from menu_item where item_code = '" + item + "'), (select item_id from menu_item where item_code = '" + item + "')," + currentOrderNo + ")"; }
                if (cmd.ExecuteNonQuery() != 1)
                { MessageBox.Show("Order failed!"); }
            }
            catch (MySqlException ex)
            { MessageBox.Show(ex.Message); }
        }

        private void order_Click(object sender, RoutedEventArgs e)
        {
            string item = this.tbSearch.Text;
            string ammount = this.tbAmmount.Text;
            if (item==""||ammount=="")
            { MessageBox.Show("Please input value!"); }
            else { orderItem(item, ammount);
                order_item.ItemsSource = showOrder(currentOrderNo).DefaultView; }
        }

        public void cancelItem(string keyword)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = con;
            if (this.rbtID.IsChecked == true)
            { cmd.CommandText = "DELETE FROM order_item WHERE item_id=" + keyword + " and category_id =(select category_id from menu_item where item_id=" + keyword + ") and order_no='" + currentOrderNo + "'"; }
            else
            { cmd.CommandText = "DELETE FROM order_item WHERE item_code=" + keyword + " and category_id =(select category_id from menu_item where item_code=" + keyword + ") and order_no='" + currentOrderNo + "'"; }
            try { if (cmd.ExecuteNonQuery() == 1) { MessageBox.Show("Order has been canceled!"); } }
            catch (MySqlException ex) { MessageBox.Show(ex.Message); }
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            string keyword = tbSearch.Text;
            if (keyword == "") { MessageBox.Show("Please input keyword!"); return; }
            cancelItem(keyword);
            order_item.ItemsSource = showOrder(currentOrderNo).DefaultView;
        }

        private void btCancel_MouseEnter(object sender, MouseEventArgs e)
        {
            order_item.ItemsSource = showOrder(currentOrderNo).DefaultView;
        }

        private void btShowOrder_Click(object sender, RoutedEventArgs e)
        {
            string orderNo = tbShowOrder.Text;
            if (orderNo == "") { MessageBox.Show("Please input order number!"); return; }
            order_item.ItemsSource = showOrder(orderNo).DefaultView;
        }

        private void btReturn_Click(object sender, RoutedEventArgs e)
        {
            order_item.ItemsSource = showOrder(currentOrderNo).DefaultView;
        }

        //修改密码

        //日月年报告

    }
}
