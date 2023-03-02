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
       /* SqlConnection DbConiction = new SqlConnection(@"Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=WPF_DB;User ID=ahmad;Password=123123asd;");*/

        public MainWindow()
        {
            InitializeComponent();
           /* CheckSqlConnection();*/
           
        }
     
        //main login function
        private void Login(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    if (ValidateUser())
                    {
                        GetUsers();
                        GoToAdminPage();
                    }

                }

            }
            catch (Exception)
            {

                throw;
            }


        }

        //get Hashed Pass By UserName

     
        public string GetHashedPassByUserName(string username)
        {
            string hashedPassword = null;
            try {
            string connectionString ="Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=Wpf;User ID=ahmad;Password=123123asd;";
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
            catch(SqlException ex)
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
                
                string storedHashedPassword = GetHashedPassByUserName("ds"); // Retrieve the hashed password from the user database
                string salt = storedHashedPassword.Substring(0, 29);
                string providedPassword = "user_provided_password";
                string saltedProvidedPassword = salt + providedPassword;
                bool passwordsMatch = BCrypt.Net.BCrypt.Verify(saltedProvidedPassword, storedHashedPassword);

                if (passwordsMatch)
                {
                    MessageBox.Show(" Authentication successful");
                  
                }
                else
                {
                    MessageBox.Show("Authentication failed");
                    
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Connection failed: " + ex.Message);
            }


            try
            {

            }
            catch (Exception ex)
            {

                MessageBox.Show("Connection failed: " + ex.Message);

                return false;
            }

            return true;
        }

        //get users from DB
        private void GetUsers()
        {


            // Create a SqlConnection object with your connection string
            SqlConnection conn = new SqlConnection("Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=Wpf;User ID=ahmad;Password=123123asd;");

            // Open the connection
            conn.Open();

            // Create a SqlCommand object with the query and connection
            SqlCommand cmd = new SqlCommand("SELECT * FROM Users ", conn);



            // Execute the query and get the results
            SqlDataReader reader = cmd.ExecuteReader();

            // Check if the query returned any rows
            if (reader.HasRows)
            {
               
                MessageBox.Show($"yay");
               

            }

            // Close the reader and connection
            reader.Close();
            conn.Close();
        }
        // go to Admin page 
        private void GoToAdminPage()
        {
            AdminPanel adminPage = new AdminPanel();
            adminPage.Show();
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
