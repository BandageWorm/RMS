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

namespace RMS
{
    public partial class PWWindow : Window
    {
        static string rms = "Database='RMS'; DataSource = 'localhost'; User Id = 'root'; Password = 'root'; charset = 'utf8'";
        MySqlConnection con = new MySqlConnection(rms);
        
        public string account;

        public PWWindow(string act)
        {
            account = act;
            InitializeComponent();
            con.Open();
        }

        private void btOK_Click(object sender, RoutedEventArgs e)
        {
            IntPtr op = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(this.oldPW.SecurePassword);
            string oldP = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(op);
            IntPtr np = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(this.newPW.SecurePassword);
            string newP = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(np);
            if (oldP == null || newP == null) { MessageBox.Show("Please input value!"); return; }
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;
                cmd.CommandText = ("select password from staff where account='" + account + "'");
                    string opw = (string)cmd.ExecuteScalar();
                    if (opw.Equals(oldP))
                    {
                        cmd.CommandText = "UPDATE staff SET password='" + newP + "' WHERE account='" + account + "'";
                        if (cmd.ExecuteNonQuery() == 1) { MessageBox.Show("Password changed successfully!"); }
                        else { MessageBox.Show("Failed!"); }
                        this.Close();
                }
                    else { MessageBox.Show("Wrong password!"); }
            }catch (MySqlException ex) { MessageBox.Show(ex.Message); }
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
