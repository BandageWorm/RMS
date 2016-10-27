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
        MySqlConnection con = new MySqlConnection(rms);
        public void getAccount(string e)
        { account = e; }
        public MainWindow()
        {
            InitializeComponent();
            con.Open();
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            string search = this.tbSearch.Text;
            if (search == "" )
            { MessageBox.Show("Please input value!");}
            else
            { order_item.ItemsSource = searchItem(search).DefaultView; }
        }

        public DataTable searchItem(string search)
        {
            DataTable dt = new DataTable();
            string sql;
            if (this.rbtQC.IsChecked == true)
            { sql = "select * from menu_item where item_code='" + search + "'"; }
            else 
            { sql = "select * from menu_item where item_ID=" + search; }
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
                { MessageBox.Show("Create order succeed!"); }
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

        //public DataTable orderItem(string item,int ammount,int orderNo)
        //{
        //    DataTable dt = new DataTable();
        //    string sql;
        //    if (this.rbtQC.IsChecked == true)
        //    { sql = "select * from menu_item where item_code='" + search + "'"; }
        //    else
        //    { sql = "select * from menu_item where item_ID=" + search; }
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        MySqlDataAdapter sda = new MySqlDataAdapter(sql, con);
        //        sda.Fill(ds, "search_item");
        //        dt = ds.Tables["search_item"];
        //    }
        //    catch (MySqlException ex)
        //    { MessageBox.Show(ex.Message); }
        //    return dt;
        //}

    }
}
