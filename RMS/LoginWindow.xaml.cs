using System;
using System.Windows;
using MySql.Data.MySqlClient;

namespace RMS
{
    public partial class Login : Window
    {
        static string rms = "Database='RMS';DataSource = 'localhost'; User Id = 'root'; Password = 'root'; charset = 'utf8'";
        MySqlConnection con = new MySqlConnection(rms);
        public Login()
        {
            InitializeComponent();
            try
            {
                con.Open();
            } catch (MySqlException e)
            {
                    MessageBox.Show(e.Message);
                    this.Close();
            }
        }

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
            IntPtr p = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(this.tbPassword.SecurePassword);
            string password = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(p);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = ("select account from staff where account='"+tbAccount.Text+"' and password='"+password+"'");
            if (cmd.ExecuteScalar()!=null)
            {
                MainWindow mWindow = new MainWindow((string)cmd.ExecuteScalar());
                mWindow.Show();
                this.Close(); 
            }
            else
            {
                MessageBox.Show("Incorrect account or password.");
            }
        }
        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
