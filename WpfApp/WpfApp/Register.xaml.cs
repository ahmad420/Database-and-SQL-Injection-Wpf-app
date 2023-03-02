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
        
      /*  SqlConnection DbConiction = new SqlConnection(@"Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=Wpf;User ID=ahmad;Password=123123asd;");*/


        public Register()
        {
            InitializeComponent();
           
        }


        private void UserRegister()
        {
            
            try
            {
                if (ValidateInput() && CheckIfUserExist())
                {
                   
                    //establish conniction and try to create and add  user
                    GoToUserProfile();
                    addUser();
                }


            }
            catch (Exception ex)
            {
                //else return the error 
                MessageBox.Show("Connection failed: " + ex.Message);
                

            }
                    
            }

        // add user 
        private void addUser()
        {
                string connectionString = "Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=Wpf;User ID=ahmad;Password=123123asd;";
                string query = "INSERT INTO Users VALUES(@FullName, @UserName, @Email, @PasswordHash)";
                string fullName = txtFullName.Text;
                string email = txtEmail.Text;
                string username = txtUsername.Text;
                string PasswordHash = encrypt(txtPassword.Password);

                try
                {

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        // The connection will be automatically closed and disposed of when the using block is exited
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                        

                                command.Parameters.AddWithValue("@FullName", fullName);
                                command.Parameters.AddWithValue("@Username", username);
                                command.Parameters.AddWithValue("@Email", email);
                                command.Parameters.AddWithValue("@PasswordHash", PasswordHash);
                         

                            using (SqlDataReader reader = command.ExecuteReader())
                            {

                                if (reader.Read())
                                {
                                    MessageBox.Show($"Success");

                                }
                            }
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

        //check if user is already registeired (not working fully)
        private bool CheckIfUserExist()
        {
            try
            {
                // create a SqlConnection object
                string connectionString = "Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=Wpf;User ID=ahmad;Password=123123asd;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // create a SqlCommand object
                    string query = "SELECT COUNT(*) FROM users WHERE username = '@username' OR email = '@email'";
                    SqlCommand command = new SqlCommand(query, connection);

                    // set the parameter values
                    command.Parameters.AddWithValue("@username", txtUsername.Text);
                    command.Parameters.AddWithValue("@email", txtEmail.Text);

                    // open the connection and execute the query
                    connection.Open();
                    int result = (int)command.ExecuteScalar();

                    // check if a matching user was found
                    if (result > 0)
                    {
                        MessageBox.Show($"failed: user exist {result}");
                        return false;
                    }
                    else
                    {
                        MessageBox.Show($"scsess {result}");
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {

                MessageBox.Show($"failed: {ex}");
            }
            return true;
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

        //go to user profile 
        private void GoToUserProfile()
        {
            userPage up = new userPage();
            up.Show();
            this.Close();
        }

        //register btn 
        private void register_Click(object sender, RoutedEventArgs e)
        {
            /*CheckIfUserExist();*/
            UserRegister();
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
            string connectionString = "Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=Wpf;User ID=ahmad;Password=123123asd;";

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
