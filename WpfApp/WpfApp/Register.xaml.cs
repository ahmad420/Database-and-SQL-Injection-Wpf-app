using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Text.RegularExpressions;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Register : Window
    {
        //regex
        private const string EmailregexPattern = @"^[a-zA-Z0-9\.\-_]+@[a-zA-Z]{2,15}(?:\.[a-zA-Z]+){1,2}$";
        private const string UserNamePattern = @"^[a-zA-Z]{1}[0-9a-zA-Z]{2,9}$";
        private const string PasswordPattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[!#$%&'()*+,\-./:;<=>?@^_`{|}~])[A-Za-z0-9_!#$%&'()*+,\-./:;<=>?@^_`{|}~]{7,12}$";
        
        SqlConnection DbConiction = new SqlConnection(@"Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=WPF_DB;User ID=ahmad;Password=123123asd;");


        public Register()
        {
            InitializeComponent();
            CheckSqlConnection();
        }


        private void RegisterBtn(object sender, RoutedEventArgs e)
        {
            
            try
            {
                if (ValidateInput())
                {
                    //establish conniction and try to create a user to insert him and if registired go to admin panel 



                    GoToAdminPanel();
                }


            }
            catch (Exception ex)
            {
                //else return the error 
                MessageBox.Show("Connection failed: " + ex.Message);
                throw;

            }
            finally
            {

            }

               
            }


        //encrypt password beforre sending to db
        private void encrypt()
        {
            string password = "my_password";
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

        }

        //check if user is already registeired 
        private bool checkIfUserExist()
        {
            try {

                if (true)
                {
                    MessageBox.Show("User Already registerd ");
                }

                return true;
            }
            catch 
            { 
            return false;
            
            }

        }

        // form validation + regex
        private bool ValidateInput()
        {

            if (txtFullName.Text == String.Empty)
            {
                MessageBox.Show("Full Name is required", "Faild", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (txtEmail.Text == String.Empty)
            {
                MessageBox.Show("Email is required", "Faild", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (txtEmail.Text == String.Empty)
            {
                MessageBox.Show("Email is required", "Faild", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(txtEmail.Text, EmailregexPattern))
            {
                MessageBox.Show("Invalid Email...");
                return false;
            }
            if (txtUsername.Text == String.Empty)
            {
                MessageBox.Show(" Username is required", "Faild", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(txtUsername.Text, UserNamePattern))
            {
                MessageBox.Show("Invalid Username...");
                return false;
            }
            if (txtPassword.Password == String.Empty)
            {
                MessageBox.Show("Password is required", "Faild", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (txtPassword.Password == String.Empty)
            {
                MessageBox.Show("Password is required", "Faild", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (txtConfirmPassword.Password == String.Empty)
            {
                MessageBox.Show("Confirm Password is required", "Faild", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (txtConfirmPassword.Password != txtPassword.Password)
            {
                MessageBox.Show("Passwords  is doesnt match", "Faild", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(txtPassword.Password, PasswordPattern) || !Regex.IsMatch(txtConfirmPassword.Password, PasswordPattern))
            {
                MessageBox.Show("Invalid password must contain one capital and small letter and spical one charcter at least   ...");
                return false;
            }


            return true;
        }

        //go to admin panel
        private void GoToAdminPanel()
        {
            this.Close();
            AdminPanel adminPage = new AdminPanel();
            adminPage.Show();
        }

        //go to sign in page 
        private void LoginPage_Click(object sender, RoutedEventArgs e)
        {
            MainWindow LoginPage = new MainWindow();
            LoginPage.Show();
            this.Close();
        }

        //check the database conniction 
        private void CheckSqlConnection()
        {
            // Set the connection string
            string connectionString = "Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=WPF_DB;User ID=ahmad;Password=123123asd;";

            // Create a new SqlConnection object
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();

                    // Check if the connection state is open
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        MessageBox.Show("Connection successful.");

                    }
                    else
                    {
                        MessageBox.Show("Connection failed.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Connection failed: " + ex.Message);
                }
                finally {
                    connection.Close();
                }
            }
        }

        //clear textbox input 
        public void Clear()
        {
            txtFullName.Clear();
            txtEmail.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();
        }
    }
}
