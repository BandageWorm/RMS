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
        static string account="W00001";
        static string rms = "Database='RMS';DataSource = 'localhost'; User Id = 'root'; Password = 'root'; charset = 'utf8'";
        string orderNo="";
        MySqlConnection con = new MySqlConnection(rms);
        public void getAccount(string e)
        { account = e; }
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                con.Open();
            }
            catch (MySqlException ex) { MessageBox.Show(ex.Message); }
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            string search = this.tbSearch.Text;
            if (search == "" )
            { MessageBox.Show("Please input value!");}
            else
            { search_item.ItemsSource = searchItem(search).DefaultView; }
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
                MySqlDataAdapter sda = new MySqlDataAdapter(sql,con);
                sda.Fill(ds, "search_item");
                dt = ds.Tables["search_item"];
            }catch (MySqlException ex)
            { MessageBox.Show(ex.Message); }
            return dt;
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
                    orderNo = on.ToString();
                    MessageBox.Show("Create order succeed!\n\nCurrent Order Number: "+orderNo.ToString().PadLeft(6,'0'));
                }
                else
                { MessageBox.Show("Create order failed!"); }
            }catch(MySqlException ex)
            { MessageBox.Show(ex.Message); }
        }

        private void btAddOrder_Click(object sender, RoutedEventArgs e)
        {
            string table = this.tbOrderTable.Text;
            if (table == "")
            { MessageBox.Show("Please input value!"); }
            else
            { createOrder(table); }
        }

        public void orderItem(string item, string ammount)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;
                if (this.rbtID.IsChecked == true)
                { cmd.CommandText = "insert into `order_item` (quantity, category_id, item_id, order_no) values(" + ammount + ", (select category_id from menu_item where item_id = " + item + ")," + item + "," + orderNo + ")"; }
                else
                { cmd.CommandText = "insert into `order_item` (quantity, category_id, item_id, order_no) values(" + ammount + ", (select category_id from menu_item where item_code = '" + item + "'), (select item_id from menu_item where item_code = '" + item + "')," + orderNo + ")"; }
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
                order_item.ItemsSource = showOrder().DefaultView; }
        }

        public DataTable showOrder()
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

    }
}
