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
                }

                //if admin go to admin panel 
              /*  if (true)
                {

                    GoToAdminPage();
                }*//*  if (true)
                {

                    GoToAdminPage();
                }*/

                
            }
            catch (Exception ex )
            {

                MessageBox.Show($"failed : {ex}");
            }


        }
        // 

        //get Hashed Pass By UserName from users table
        public string GetHashedPassByUserName(string str)
        {
            string hashedPassword = "";
            try {
            string connectionString ="Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=Wpf;User ID=ahmad;Password=123123asd;";
            string query = "SELECT HashedPassword FROM Users WHERE Username = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                    // The connection will be automatically closed and disposed of when the using block is exited
                    connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@Username", username.Text);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            hashedPassword = reader.GetString(0);
                        }
                    }
                }
            }

            }
            catch(SqlException ex)
            {
                MessageBox.Show("Connection failed: " + ex.Message);
            }
            return hashedPassword;
        }

        //get Hashed Pass By UserName from admins table
        public string GetHashedPassByUserNameAdmins()
        {
            string hashedPassword = null;
            try
            {
                string connectionString = "Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=Wpf;User ID=ahmad;Password=123123asd;";
                string query = "SELECT HashedPassword FROM Users WHERE Username = @Username";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // The connection will be automatically closed and disposed of when the using block is exited
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@Username", username);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                hashedPassword = reader.GetString(0);
                            }
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Connection failed: " + ex.Message);
            }
            return hashedPassword;
        }


        // check if user exist and user is valid 
        private bool ValidateUser()
        {
            try {
                SqlConnection DbConiction = new SqlConnection(@"Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=Wpf;User ID=ahmad;Password=123123asd;");
                
           
                if (ValidateInput())
                {
                    string storedHashedPassword = GetHashedPassByUserName(username.Text); // Retrieve the hashed password from the user database
                    string salt = storedHashedPassword.Substring(0, 29);
                    string providedPassword = password.Password;
                    string saltedProvidedPassword = salt + providedPassword;
                    bool passwordsMatch = BCrypt.Net.BCrypt.Verify(saltedProvidedPassword, storedHashedPassword);

                    if (passwordsMatch)
                    {
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

        // add admins 
        private void addAdmins()
        {
          

            // Connection string for SQL Server
            string connectionString = "Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=Wpf;User ID=ahmad;Password=123123asd;";

            // SQL statement to insert admin into admins table
            string insertQuery = "INSERT INTO Admins (UserName, PasswordHash) VALUES (@UserName, @PasswordHash)";

            // Admin data to be inserted
            string UserName = "Ahmad420";
            string PasswordHash = encrypt("asd123A!"); // Note: You should hash the password before inserting it into the database

            try
            {
                // Create a new SqlConnection object with the connection string
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the database connection
                    connection.Open();

                    // Create a new SqlCommand object with the SQL statement and SqlConnection object
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        // Add parameters to the SqlCommand object
                        command.Parameters.AddWithValue("@UserName", UserName);
                        command.Parameters.AddWithValue("@PasswordHash", PasswordHash);

                        // Execute the SQL statement
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {

                MessageBox.Show($"{ex}");
            }
     
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
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
