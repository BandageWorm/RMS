using System.Windows;
using MySql.Data.MySqlClient;
using System.Data;

namespace RMS
{
    public partial class EditInfoWindow : Window
    {
        static string rms = "Database='RMS';DataSource = 'localhost'; User Id = 'root'; Password = 'root'; charset = 'utf8'";
        MySqlConnection con = new MySqlConnection(rms);
        MySqlCommand cmd = new MySqlCommand();

        public EditInfoWindow()
        {
            InitializeComponent();
            con.Open();
        }

        public DataTable showInfo(string No)
        {
            DataTable dt = new DataTable();
            string sql;
            if (this.rbtMenu.IsChecked == true)
                sql = "select item_id as id,item_name as item,item_info as info from menu_item where item_id=" + No;
            else if(this.rbtCategory.IsChecked == true)
                sql = "select category_id as id,category as item,category_info as info from menu_category where category_id=" + No;
            else
                sql = "select table_no as id,table_info as info from `table` where table_no=" + No;
            try
            {
                DataSet ds = new DataSet();
                MySqlDataAdapter sda = new MySqlDataAdapter(sql, con);
                sda.Fill(ds, "Info");
                dt = ds.Tables["Info"];
            }
            catch (MySqlException ex)
            { MessageBox.Show(ex.Message); }
            return dt;
        }

        private void tbEditInfoNo_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string No = this.tbEditInfoNo.Text;
            if (No != "") dgInfo.ItemsSource = showInfo(No).DefaultView;
        }

        private void btOk_Click(object sender, RoutedEventArgs e)
        {
            string info = tbEditInfo.Text;
            string No = tbEditInfoNo.Text;
            if (info == null || No == null) { MessageBox.Show("Please input info or item No.!"); return; }
            string sql;
            if (this.rbtMenu.IsChecked == true)
                sql = "UPDATE menu_item SET item_info='"+info+"' WHERE item_id=" + No;
            else if (this.rbtCategory.IsChecked == true)
                sql = "UPDATE menu_category SET category_info='"+info+"' WHERE category_id=" + No;
            else
                sql = "UPDATE `table` SET table_info='"+info+"' WHERE table_no="+No;
            try
            {
                cmd.Connection = con;
                cmd.CommandText = sql;
                if (cmd.ExecuteNonQuery() == 1) MessageBox.Show("Info has changed!");
                else MessageBox.Show("Failed!");
            }catch(MySqlException ex) { MessageBox.Show(ex.Message); }
            dgInfo.ItemsSource = showInfo(No).DefaultView;
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void rbtChecked_Changed(object sender, RoutedEventArgs e)
        {
            string No = this.tbEditInfoNo.Text;
            if (this.tbEditInfoNo.Text != "") dgInfo.ItemsSource = showInfo(No).DefaultView;
        }
    }
}
