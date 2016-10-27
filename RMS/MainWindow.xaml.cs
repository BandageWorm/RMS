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
        static string account;
        static string rms = "Database='RMS';DataSource = 'localhost'; User Id = 'root'; Password = 'root'; charset = 'utf8'";
        MySqlConnection con = new MySqlConnection(rms);
        public void getAccount(string e)
        {
            account = e;
        }
        public MainWindow()
        {
            InitializeComponent();
            con.Open();
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
                order_item.ItemsSource = searchitem(this.tbSearch.Text).DefaultView;
        }
        public DataTable searchitem(string search)
        {
            DataTable dt = new DataTable();
                string sql;
                if (this.rbtQC.IsChecked == true)
                { sql = "select * from menu_item where item_code='" + search+"'"; }
                else
                { sql = "select * from menu_item where item_ID=" + search; }
            try
            {
                DataSet ds = new DataSet();
                MySqlDataAdapter sda = new MySqlDataAdapter(sql,con);
                sda.Fill(ds, "item_id");
                dt = ds.Tables["item_id"];
            }catch (MySqlException e)
            {
                MessageBox.Show(e.Message);
            }
            return dt;
        }
        
    }
}
