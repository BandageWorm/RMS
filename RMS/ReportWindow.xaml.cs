using System;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;

namespace RMS
{
    public partial class ReportWindow : Window
    {
        
        public string totalIncome { get; set; }
        public string orderNumber { get; set; }
        public string totalPay { get; set; }
        static string rms = "Database='RMS';DataSource = 'localhost'; User Id = 'root'; Password = 'root'; charset = 'utf8'";
        MySqlConnection con = new MySqlConnection(rms);
        MySqlCommand cmd = new MySqlCommand();
        public string scale;

        public ReportWindow(string timeScale, string type)
        {
            if (type == "daily") { scale = "date_format(order_time,'%Y-%m-%d')='" + timeScale + "'"; }
            else if (type == "monthly") { scale = "date_format(order_time,'%Y-%m')='" + timeScale + "'"; }
            else { scale = "date_format(order_time,'%Y')='" + timeScale + "'"; }
            try { con.Open(); }
            catch (MySqlException ex) { MessageBox.Show(ex.Message); this.Close(); }
            InitializeComponent();
            showOrderNumber();
            dgBill.ItemsSource = showBill().DefaultView;
            dgCount.ItemsSource = showCount().DefaultView;
        }

        public DataTable showBill()
        {
            DataTable dt = new DataTable();
            string sql = @"select concat(c.order_no) as order_no,staff_name,bill,actual_payment,
            format(actual_payment-bill,2) as `change`,table_no,order_time from (select * from 
            `order` inner join staff on `order`.staff_account = staff.account) c left join 
            (select order_no, sum(price * quantity) as bill from order_item a inner join menu_item
            b on a.item_id = b.item_id group by order_no) d on c.order_no = d.order_no where "
            +scale+" order by c.order_no asc";
            try
            {//Show DataGrid
                DataSet ds = new DataSet();
                MySqlDataAdapter sda = new MySqlDataAdapter(sql, con);
                sda.Fill(ds, "showBill");
                dt = ds.Tables["showBill"];
            }
            catch (MySqlException ex)
            { MessageBox.Show(ex.Message); }
            object sum = dt.Compute("sum(bill)", "TRUE");
            if (!Convert.IsDBNull(sum)) totalIncome = ((double)sum).ToString("f2");
            else totalIncome = "0.00";
            tbkTotalIncome.Text = this.totalIncome;
            sum = dt.Compute("sum(actual_payment)", "TRUE");
            if (!Convert.IsDBNull(sum)) totalPay = ((double)sum).ToString("f2");
            else totalPay = "0.00";
            tbkTotalPay.Text = this.totalPay;
            return dt;
        }

        public DataTable showCount()
        {
            DataTable dt = new DataTable();
            string sql = "select o.item_id,item_name,count(*) as count from order_item o inner join menu_item m on o.item_id=m.item_id where order_no in (select order_no from `order` where "+scale+") group by item_id";
            try
            {//Show DataGrid
                DataSet ds = new DataSet();
                MySqlDataAdapter sda = new MySqlDataAdapter(sql, con);
                sda.Fill(ds, "showCount");
                dt = ds.Tables["showCount"];
            }
            catch (MySqlException ex)
            { MessageBox.Show(ex.Message); }
            return dt;
        }

        public void showOrderNumber()
        {
            cmd.Connection = con;
            cmd.CommandText = "select count(*) from `order` where " + scale;
            if (cmd.ExecuteScalar() == DBNull.Value) orderNumber = "0";
            else orderNumber = cmd.ExecuteScalar().ToString();
            tbkTotalOrder.Text = this.orderNumber;
        }

        private void btOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
