using System.Windows;
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
        }

        public DataTable showItem()
        {
            DataTable dt = new DataTable();
            string sql = @"select * from menu_item m inner join menu_category c 
                        where m.category_id=c.category_id order by item_id asc";
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

        private void btDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            string itemID = this.tbItem.Text;
            if(itemID==null) { MessageBox.Show("Please input item ID!"); return; }
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
            if (categoryID == null) { MessageBox.Show("Please input category ID!"); return; }
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "DELETE FROM menu_category WHERE category_id=" + categoryID;
                if (cmd.ExecuteNonQuery() != 1) MessageBox.Show("Failed!");
            }
            catch (MySqlException ex) { MessageBox.Show(ex.Message); }
            dgCategory.ItemsSource = showCategory().DefaultView;
        }

        private void btAddItem_Click(object sender, RoutedEventArgs e)
        {
            string name = tbItemName.Text;
            string QC = tbItemQC.Text;
            string price = tbItemPrice.Text;
            string cid = tbItemCategory.Text;
            string info = tbItemInfo.Text;
            if(name == null||QC == null ||price == null ||cid == null) { MessageBox.Show("Please input values!"); return; }
            if (info == null) info = "";
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
    }
}
