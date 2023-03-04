using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Data;
using System.Collections;
using BCrypt.Net;

namespace WpfApp
{

    public partial class MainWindow : Window
    {
        //regex
        private string UserNamePattern = @"^[a-zA-Z]{1}[0-9a-zA-Z]{2,9}$";
        private string PasswordPattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[!#$%&'()*+,\-./:;<=>?@^_`{|}~])[A-Za-z0-9_!#$%&'()*+,\-./:;<=>?@^_`{|}~]{7,12}$";


        public MainWindow()
        {
            InitializeComponent();

        }

        //main login function
        private void Login()
        {

            try
            {
                if (ValidateUser())
                {
                    //if normal user go to userpage 

                    GoToUserProfile();
                    return;
                }

                //if admin go to admin panel 
                if (ValidateAdmin())
                {

                    GoToAdminPage();
                    return;
                }
                

            }
            catch (Exception ex)
            {

                MessageBox.Show($"failed : {ex}");
            }


        }
        // 

        //get Hashed Pass By UserName from users table
        private string GetHashedPassByUserName(string str)
        {
            string hashedPassword = null;
            string userName = str;
            string connectionString = "Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=Wpf;User ID=ahmad;Password=123123asd;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SelectUserPasswordHashByUsername", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserName", userName);

                    hashedPassword = (string)command.ExecuteScalar();


                }
            }
            return hashedPassword;
        }

       
        //check if admin exiast and admin is valid 
        private bool ValidateAdmin()
        {
            try
            {
                SqlConnection DbConiction = new SqlConnection(@"Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=Wpf;User ID=ahmad;Password=123123asd;");


                if (ValidateInput())
                {
                    string admin = username.Text;
                    string pass = password.Password;

                    string storedHashedPassword = GetHashedPassByUserNameAdmins(admin); // Retrieve the hashed password from the user database

                    if (BCrypt.Net.BCrypt.Verify(pass, storedHashedPassword))
                    {
                        Console.WriteLine("Password matches!");
                        MessageBox.Show(" Authentication successful");
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Authentication failed");
                        return false;
                    }

                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Connection failed: " + ex.Message);
            }

            return false;

        }
        


        // check if user exist and user is valid 
        private bool ValidateUser()
        {
            try
            {
                SqlConnection DbConiction = new SqlConnection(@"Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=Wpf;User ID=ahmad;Password=123123asd;");
                

                if (ValidateInput())
                {
                    string user = username.Text;
                    string pass = password.Password;
                  
                    string storedHashedPassword = GetHashedPassByUserName(user); // Retrieve the hashed password from the user database

                    if (BCrypt.Net.BCrypt.Verify(pass, storedHashedPassword))
                    {
                        Console.WriteLine("Password matches!");
                        MessageBox.Show(" Authentication successful");
                        return true;
                    }
                    else
                    {
                       
                        return false;
                    }

                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Connection failed: " + ex.Message);
            }

            return false;
        }


        //get Hashed Pass By UserName from admins table
        private string GetHashedPassByUserNameAdmins(string name)
        {
            string hashedAdminPassword = null;
            string amdinUserName = name;
            string connectionString = "Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=Wpf;User ID=ahmad;Password=123123asd;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SelectAdminPasswordHashByUsername", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserName", amdinUserName);

                    hashedAdminPassword = (string)command.ExecuteScalar();


                }
            }
            return hashedAdminPassword;
        }

        //login btn
        private void login_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        // go to Admin page 
        private void GoToAdminPage()
        {
            AdminPanel adminPage = new AdminPanel();
            adminPage.Show();
            this.Close();

        }

        //go to user profile 
        private void GoToUserProfile()
        {
            userPage up = new userPage();
            up.Show();
            this.Close();
        }

        // go to register page
        private void Register_Click(object sender, RoutedEventArgs e)
        {

            Register RegisterPage = new Register();
            RegisterPage.Show();
            this.Close();
        }

        // input validation 
        public bool ValidateInput()
        {

            if (username.Text == String.Empty)
            {
                MessageBox.Show("username is required", "Faild", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (password.Password == String.Empty)
            {
                MessageBox.Show("password is required", "Faild", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(username.Text, UserNamePattern))
            {
                MessageBox.Show("Invalid username ...");
                return false;
            }

            if (!Regex.IsMatch(password.Password, PasswordPattern))
            {
                MessageBox.Show("Invalid password ...");
                return false;
            }
            return true;
        }

   

        //encrypt password beforre sending to db
        private string encrypt(string pass)
        {
            string password = pass;
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            return hashedPassword;
        }

        // DB conniction Test
        private void CheckSqlConnection()
        {
            // Set the connection string
            string connectionString = "Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=Wpf;User ID=ahmad;Password=123123asd;";

            // Create a new SqlConnection object
            using (SqlConnection connection = new SqlConnection(connectionString))
            {`
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
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
