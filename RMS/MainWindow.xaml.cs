using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MySql.Data.MySqlClient;
using System.Data;

namespace RMS
{
    public partial class MainWindow : Window
    {
        public string account { get; set; }
        public string name { get; set; }
        public string orderNo { get; set; }
        public string currentOrderNo { get; set; }
        public string totalBill { get; set; }
        public string totalBills { get; set; }
        static string rms = @"Database='RMS';DataSource = 'localhost';User Id = 'root'; 
                           Password = 'root'; charset = 'utf8'";
        MySqlConnection con = new MySqlConnection(rms);
        MySqlCommand cmd = new MySqlCommand();
        string today = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");

        public MainWindow(string act)
        {
            //string act = "M00001";
            account = act;
            try { con.Open(); }
            catch (MySqlException ex) { MessageBox.Show(ex.Message); this.Close(); }
            InitializeComponent();
            if (isManager()) {
                btEditUser.Visibility = Visibility.Visible;
                btEditMenu.Visibility = Visibility.Visible;
                imgSmile.Visibility = Visibility.Hidden;
            }
            getName();
            tbkaccount.Text = this.name;
            dgBill.ItemsSource = showBill(today,today).DefaultView;
        }

        public void getName()
        {
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "select staff_name from staff where account='" + account+"'";
                name = cmd.ExecuteScalar().ToString();
            }
            catch (MySqlException ex) { MessageBox.Show(ex.Message); }
        }

        public bool isManager()
        {
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "select role from staff where account='" + account + "'";
                if (cmd.ExecuteScalar().ToString() == "Manager") return true;
                else return false;
            }
            catch (MySqlException ex) { MessageBox.Show(ex.Message); }
            return false;
        }

        public DataTable showSearch(string keyword)
        {
            DataTable dt = new DataTable();
            string sql;
            if (this.rbtQC.IsChecked == true)
                sql = @"select * from menu_item a inner join menu_category b 
                    on a.category_id=b.category_id where item_code='" + keyword + "'";
            else
                sql = @"select * from menu_item a inner join menu_category b 
                    on a.category_id=b.category_id where item_id='" + keyword +"'";
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
                cmd.CommandText = @"insert into `order` (staff_account,actual_payment,table_no) 
                                values('" + account + "',0," + table + ")" ;
                if (cmd.ExecuteNonQuery() == 1)
                {
                    cmd.CommandText = "select order_no,order_time from `order` order by order_time desc";
                    uint on = (uint)cmd.ExecuteScalar();
                    orderNo = on.ToString().PadLeft(6, '0');
                    tbkOrder.Text = this.orderNo;
                    currentOrderNo = orderNo;
                    tbkCurOrder.Text = this.currentOrderNo;
                    dgOrder.ItemsSource = showOrder(currentOrderNo).DefaultView;
                    dgBill.ItemsSource = showBill(today, today).DefaultView;
                    MessageBox.Show("Create order succeed!\n\nCurrent Order Number: " + orderNo);
                }
                else
                { MessageBox.Show("Create order failed!"); }
            }
            catch (MySqlException ex)
            { MessageBox.Show(ex.Message); }
        }

        private void btCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            string table = this.tbOrderTable.Text;
            if (table == "") MessageBox.Show("Please input value!");
            else createOrder(table);
        }

        public DataTable showOrder(string order)
        {
            DataTable dt = new DataTable();
            string sql = @"select * from order_item inner join menu_item on 
                        order_item.item_id=menu_item.item_id where order_no=" + order;
            string sqlbill = @"select format(sum(price*quantity),2) from order_item a 
                        inner join menu_item b on a.item_id=b.item_id where order_no=" + order;
            try
            {   //Show DataGrid
                DataSet ds = new DataSet();
                MySqlDataAdapter sda = new MySqlDataAdapter(sql, con);
                MySqlCommand check = new MySqlCommand();
                check.Connection = con;
                check.CommandText = sql;
                if(check.ExecuteScalar()==DBNull.Value) { MessageBox.Show("Order Number does not exist!"); return null; }
                sda.Fill(ds, "order_item");
                dt = ds.Tables["order_item"];
                //Show Bill
                cmd.CommandText = sqlbill;
                cmd.Connection = con;
                if (cmd.ExecuteScalar() == DBNull.Value) { totalBill = ""; }
                else totalBill = (string)cmd.ExecuteScalar();
                tbkTotalBill.Text = this.totalBill;
                //Show Order No
                orderNo = order.PadLeft(6, '0');
                tbkOrder.Text = this.orderNo;
            }
            catch (MySqlException ex)
            { MessageBox.Show(ex.Message); }
            return dt;
        }

        //public DataTable showNewOrder(string order)
        //{
        //    DataTable dt = new DataTable();
        //    string sql = @"select * from order_item inner join menu_item on 
        //                order_item.item_id=menu_item.item_id where order_no=" + order;
        //    try
        //    {   //Show DataGrid
        //        DataSet ds = new DataSet();
        //        MySqlDataAdapter sda = new MySqlDataAdapter(sql, con);
        //        sda.Fill(ds, "order_item");
        //        dt = ds.Tables["order_item"];
        //        //Show Order No
        //        orderNo = order.PadLeft(6, '0');
        //        tbkOrder.Text = this.orderNo;
        //    }
        //    catch (MySqlException ex)
        //    { MessageBox.Show(ex.Message); }
        //    return dt;
        //}

        public void orderItem(string item, string ammount)
        {
            try
            {
                cmd.Connection = con;
                if (this.rbtID.IsChecked == true)
                    cmd.CommandText = @"insert into `order_item` (quantity, item_id, 
                                    order_no) values(" + ammount + "," + item + "," + orderNo + ")";
                else
                    cmd.CommandText = @"insert into `order_item` (quantity, item_id, 
                                    order_no) values(" + ammount + @", (select item_id 
                                    from menu_item where item_code = '" + item + "')," + orderNo + ")";
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
                dgBill.ItemsSource = showBill(today, today).DefaultView;
            }
        }

        public void cancelItem(string keyword)
        {
            cmd.Connection = con;
            if (this.rbtID.IsChecked == true)
                cmd.CommandText = "DELETE FROM order_item WHERE item_id=" + keyword + 
                                " and order_no=" + orderNo;
            else
                cmd.CommandText = @"DELETE FROM order_item WHERE item_id=(select item_id from 
                                menu_item where item_code='"+keyword+"') and order_no=" + orderNo;
            try { if (cmd.ExecuteNonQuery() >= 1) MessageBox.Show("Order has been canceled!"); }
            catch (MySqlException ex) { MessageBox.Show(ex.Message); }
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            string keyword = tbKeyword.Text;
            if (keyword == "") { MessageBox.Show("Please input keyword!"); return; }
            cancelItem(keyword);
            dgOrder.ItemsSource = showOrder(currentOrderNo).DefaultView;
            dgBill.ItemsSource = showBill(today, today).DefaultView;
        }

        private void btorderDetail_Click(object sender, RoutedEventArgs e)
        {
            string orderNo = tbShowOrder.Text;
            if (orderNo == "") { MessageBox.Show("Please input order number!"); return; }
            if(showOrder(orderNo)!=null) dgOrder.ItemsSource = showOrder(orderNo).DefaultView;
        }

        public DataTable showBill()
        {
            DataTable dt = new DataTable();
            string sql = @"select concat(c.order_no) as order_no,staff_name,bill,actual_payment,format(actual_payment-bill,2) as `change`,
            table_no,order_time from (select * from `order` inner join staff on `order`.staff_account 
            = staff.account) c left join (select order_no, sum(price * quantity) as bill from 
            order_item a inner join menu_item b on a.item_id = b.item_id group by order_no) d on 
            c.order_no = d.order_no order by c.order_no asc";
            try
            {//Show DataGrid
                DataSet ds = new DataSet();
                MySqlDataAdapter sda = new MySqlDataAdapter(sql, con);
                sda.Fill(ds, "order_item");
                dt = ds.Tables["order_item"];
            }
            catch (MySqlException ex)
            { MessageBox.Show(ex.Message); }
            object sum = dt.Compute("sum(bill)", "TRUE");
            if (!Convert.IsDBNull(sum)) totalBills = ((double)sum).ToString("f2");
            else totalBills = "";
            tbkTotalBills.Text = this.totalBills;
            return dt;
        }

        public DataTable showBill(string table)
        {
            DataTable dt = new DataTable();
            string sql = @"select concat(c.order_no) as order_no,staff_name,bill,actual_payment,format(actual_payment-bill,2) as `change`,
            table_no,order_time from (select * from `order` inner join staff on `order`.staff_account 
            = staff.account) c left join (select order_no, sum(price * quantity) as bill from 
            order_item a inner join menu_item b on a.item_id = b.item_id group by order_no) d 
            on c.order_no = d.order_no where table_no = "+table+" order by c.order_no asc";
            try
            {//Show DataGrid
                cmd.Connection = con;
                cmd.CommandText = "select table_no from `table` where table_no='" + table + "'";
                if(cmd.ExecuteScalar() == null) { MessageBox.Show("Table does not exist!"); return null; }
                DataSet ds = new DataSet();
                MySqlDataAdapter sda = new MySqlDataAdapter(sql, con);
                sda.Fill(ds, "order_item");
                dt = ds.Tables["order_item"];
            }
            catch (MySqlException ex)
            { MessageBox.Show(ex.Message); }
            object sum = dt.Compute("sum(bill)", "TRUE");
            if (!Convert.IsDBNull(sum)) totalBills = ((double)sum).ToString("f2");
            else totalBills = "";
            tbkTotalBills.Text = this.totalBills;
            return dt;
        }

        public DataTable showBill(string startDate,string endDate)
        {
            DataTable dt = new DataTable();
            string sql = @"select concat(c.order_no) as order_no,staff_name,bill,actual_payment,
            format(actual_payment-bill,2) as `change`,table_no,order_time from (select * from 
            `order` inner join staff on `order`.staff_account = staff.account) c left join 
            (select order_no, sum(price * quantity) as bill from order_item a inner join menu_item
            b on a.item_id = b.item_id group by order_no) d on c.order_no = d.order_no where 
            date_format(order_time,'%Y-%m-%d')>='" + startDate+ 
            "' and date_format(order_time,'%Y-%m-%d')<='" + endDate+"' order by c.order_no asc";
            try
            {//Show DataGrid
                DataSet ds = new DataSet();
                MySqlDataAdapter sda = new MySqlDataAdapter(sql, con);
                sda.Fill(ds, "order_item");
                dt = ds.Tables["order_item"];
            }
            catch (MySqlException ex)
            { MessageBox.Show(ex.Message); }
            object sum = dt.Compute("sum(bill)", "TRUE");
            if (!Convert.IsDBNull(sum)) totalBills = ((double)sum).ToString("f2");
            else totalBills = "";
            tbkTotalBills.Text = this.totalBills;
            return dt;
        }

        private void btPassword_Click(object sender, RoutedEventArgs e)
        {
            PWWindow pw = new PWWindow(account);
            pw.Show();
        }

        private void btsetCurrent_Click(object sender, RoutedEventArgs e)
        {
            string orderNo = tbShowOrder.Text;
            if (orderNo == "") { MessageBox.Show("Please input order number!"); return; }
            currentOrderNo = orderNo.PadLeft(6, '0');
            dgOrder.ItemsSource = showOrder(currentOrderNo).DefaultView;
            this.tbkCurOrder.Text = this.currentOrderNo;
        }

        private void btSetPay_Click(object sender, RoutedEventArgs e)
        {
            string pay = tbPayOrder.Text;
            if (pay == "") { MessageBox.Show("Please input payment!"); return; }
            cmd.Connection = con;
            cmd.CommandText = "UPDATE `order` SET actual_payment='" + pay + "' WHERE order_no='" 
                + currentOrderNo + "'";
            if(cmd.ExecuteNonQuery() != 1) MessageBox.Show("Failed to pay!");
            dgBill.ItemsSource = showBill(today, today).DefaultView;
        }

        private void btResetPay_Click(object sender, RoutedEventArgs e)
        {
            cmd.Connection = con;
            cmd.CommandText = "UPDATE `order` SET actual_payment='0' WHERE order_no='" 
                + currentOrderNo + "'";
            if (cmd.ExecuteNonQuery() != 1) MessageBox.Show("Failed to reset!");
            dgBill.ItemsSource = showBill(today, today).DefaultView;
        }

        private void btEditInfo_Click(object sender, RoutedEventArgs e)
        {
            EditInfoWindow EditInfo = new EditInfoWindow();
            EditInfo.Show();
        }

        private void btshowOrder_Click(object sender, RoutedEventArgs e)
        {//search order by table no
            string table = this.tbOrderTable.Text;
            if (table == "") dgBill.ItemsSource = showBill(today, today).DefaultView;
            else if(showBill(table) != null) dgBill.ItemsSource = showBill(table).DefaultView;
        }

        private void tbkCurOrder_MouseEnter(object sender, MouseEventArgs e)
        {
            if(currentOrderNo != null) dgOrder.ItemsSource = showOrder(currentOrderNo).DefaultView;
        }

        private void btEditUser_Click(object sender, RoutedEventArgs e)
        {
            StaffWindow staffEdit = new StaffWindow();
            staffEdit.Show();
        }

        private void btsearchOrder_Click(object sender, RoutedEventArgs e)
        {
            string startDate = Convert.ToDateTime(dpStart.Text).ToString("yyyy-MM-dd");
            string endDate = Convert.ToDateTime(dpEnd.Text).ToString("yyyy-MM-dd");
            dgBill.ItemsSource = showBill(startDate,endDate).DefaultView;
        }

        private void btAllOrder_Click(object sender, RoutedEventArgs e)
        {
            dgBill.ItemsSource = showBill().DefaultView;
        }

        private void btReportDaily_Click(object sender, RoutedEventArgs e)
        {
            string timeScale = Convert.ToDateTime(dpReport.Text).ToString("yyyy-MM-dd");
            ReportWindow report = new ReportWindow(timeScale, "daily");
            report.Show();
        }

        private void btReportMonthly_Click(object sender, RoutedEventArgs e)
        {
            string timeScale = Convert.ToDateTime(dpReport.Text).ToString("yyyy-MM");
            ReportWindow report = new ReportWindow(timeScale, "monthly");
            report.Show();
        }

        private void btReportYearly_Click(object sender, RoutedEventArgs e)
        {
            string timeScale = Convert.ToDateTime(dpReport.Text).ToString("yyyy");
            ReportWindow report = new ReportWindow(timeScale, "yearly");
            report.Show();
        }

        private void btEditMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menu = new MenuWindow();
            menu.Show();
        }
    }
}
