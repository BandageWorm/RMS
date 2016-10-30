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
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;

namespace RMS
{
    public partial class StaffWindow : Window
    {
        static string rms = "Database='RMS';DataSource = 'localhost'; User Id = 'root'; Password = 'root'; charset = 'utf8'";
        MySqlConnection con = new MySqlConnection(rms);
        MySqlCommand cmd = new MySqlCommand();

        public StaffWindow()
        {
            InitializeComponent();
            con.Open();
            dgStaff.ItemsSource = showStaff().DefaultView;

        }

        public DataTable showStaff()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT * FROM staff";
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

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            string account = tbAccount.Text;
            if (account == null) { MessageBox.Show("Please input account!"); return; }
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "DELETE FROM staff WHERE account='"+account+"'";
                if(cmd.ExecuteNonQuery() != 1) MessageBox.Show("Failed!");
            }catch(MySqlException ex) { MessageBox.Show(ex.Message); }
            dgStaff.ItemsSource = showStaff().DefaultView;
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem crole = cbRole.SelectedItem as ComboBoxItem;
            IntPtr pw = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(this.pwbPassword.SecurePassword);
            string password = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(pw);
            string account = tbAccount.Text;
            string role = crole.Content.ToString();
            string contact = tbContact.Text;
            string manager = tbManager.Text;
            string name = tbName.Text;
            string age = tbAge.Text;
            if (account == null|| password == null|| role == null || contact == null || name == null || age == null)
            { MessageBox.Show("Please input value!"); return; }
            try
            {
                cmd.Connection = con;
                if(role=="Manager")
                cmd.CommandText = "INSERT INTO staff (account, password, staff_name, age, contact_no, role) VALUES ('"+account+"', '"+ password + "', '"+name+"', '"+age+"', '"+contact+"', '"+role+"')";
                else
                cmd.CommandText = "INSERT INTO staff (account, password, staff_name, age, contact_no, role, manager_account) VALUES ('" + account + "', '" + password + "', '" + name + "', '" + age + "', '" + contact + "', '" + role + "', '" + manager + "')";
                if (cmd.ExecuteNonQuery() != 1) MessageBox.Show("Failed!");
            }
            catch (MySqlException ex) { MessageBox.Show(ex.Message); }
            dgStaff.ItemsSource = showStaff().DefaultView;
        }
    }
}
