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
using System.Windows.Shapes;
using System.Reflection;
using Org.BouncyCastle.Crypto.Generators;
using BCrypt.Net;
using System.Collections;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Windows.Controls.Primitives;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        private int key = 0;
        private const string EmailregexPattern = @"^[a-zA-Z0-9\.\-_]+@[a-zA-Z]{2,15}(?:\.[a-zA-Z]+){1,2}$";
        private const string UserNamePattern = @"^[a-zA-Z]{1}[0-9a-zA-Z]{2,9}$";
        private const string PasswordPattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[!#$%&'()*+,\-./:;<=>?@^_`{|}~])[A-Za-z0-9_!#$%&'()*+,\-./:;<=>?@^_`{|}~]{7,12}$";
        private const string idPattern = @"\d+";
        /*  SqlConnection DbConiction = new SqlConnection(@"Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=Wpf;User ID=ahmad;Password=123123asd;");*/

        public AdminPanel()
        {
            InitializeComponent();
            ReRenderList();
        }
       

        //take index of ComboBox and store it in the local key varible 
        private void returnIndex(int index)
        {
             key = index;
        }

        // get data from DB and Display in DataGrid
        private void ReRenderList()
        {
            string connectionString = "Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=Wpf;User ID=ahmad;Password=123123asd;";
            string query = "SELECT * FROM Users";
            DataTable dt = new DataTable();
            try
            {
              
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // The connection will be automatically closed and disposed of when the using block is exited
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                       
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                               
                                 dt.Load(reader);
                                dataGrid.ItemsSource = dt.DefaultView;
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

        // validaate id is a number and check if id is less than taable rows count 
        private bool valisdateId()
        {
            string connectionString = "Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=Wpf;User ID=ahmad;Password=123123asd;";

            int rows = 0; 
            if (!Regex.IsMatch(UserIDtxt.Text, idPattern))
            {
                MessageBox.Show($"Failed . not a number ");
                return false;
            }
            //check if id is in the table range 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM users;";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                 rows = (int)command.ExecuteScalar();
               
            }

            int input = (int.Parse(UserIDtxt.Text));
            if (!(input <= rows && input >= 0) )
            {
                MessageBox.Show($"Failed no such id  ");
                return false;
            }

            return true;
        }


        //select user from table (working but datagrid not updating ) 
        private void Select()
        {
            string connectionString = "Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=Wpf;User ID=ahmad;Password=123123asd;";
            string query = "SELECT * FROM Users Where id = @id; ";
            DataTable dt = new DataTable();

            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // The connection will be automatically closed and disposed of when the using block is exited
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (valisdateId())
                        {
                            string id = UserIDtxt.Text;
                            command.Parameters.AddWithValue("@id", id);
                        
                        }
                        else
                        {
                            MessageBox.Show($"Failed invalid input ");
                            return;
                        }


                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                MessageBox.Show($"Success");
                                dt.Load(reader);
                                dataGrid.ItemsSource = dt.DefaultView;
                            }
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show($"{ex}");

            }
            ReRenderList();
            ClearInputData();
        }

        //insert new user to users (perfect)
        private void Insert()
        {
            string connectionString = "Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=Wpf;User ID=ahmad;Password=123123asd;";
            string query = "INSERT INTO Users VALUES(@FullName, @UserName, @Email, @PasswordHash)";
            DataTable dt = new DataTable();

            string fullName = FullNametxt.Text;
            string email = Emailtxt.Text;
            string username = UserName.Text;
            string PasswordHash = encrypt(Passwordtxt.Text);

            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // The connection will be automatically closed and disposed of when the using block is exited
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (ValidateInsert())
                        {

                        
                        command.Parameters.AddWithValue("@FullName", fullName);
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@PasswordHash", PasswordHash);
                        }
                        else
                        {
                            MessageBox.Show($"Failed invalid input ");
                            return;
                        }
                        

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

            ReRenderList();

        }

        //update existing user (not working :( )
        private void UpdateUserEmail()
        {
            string connectionString = "Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=Wpf;User ID=ahmad;Password=123123asd;";


            string query = "UPDATE Users SET Email = '@Email' WHERE UserName = '@UserName' :";
            DataTable dt = new DataTable();

            string email = Emailtxt.Text;
            string username = UserName.Text;

            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // The connection will be automatically closed and disposed of when the using block is exited
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (ValidateUpdate())
                        { 
                            command.Parameters.AddWithValue("@Username", username);
                            command.Parameters.AddWithValue("@Email", email);
                           
                        }
                        else
                        {
                            MessageBox.Show($"Failed. invalid input.. ");
                            return;
                        }


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            try
                            {
                                MessageBox.Show($"${reader.Read()} ");
                                if (reader.Read())
                                {

                                    MessageBox.Show($"Success {username} email was updated to {email} ");
                                    
                                }
                                else
                                {
                                    MessageBox.Show($"user not found");
                                }
                            }
                            catch (SqlException ex)
                            {

                                MessageBox.Show($"{ex}");
                            }
                      
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show($"{ex}");

            }
            ReRenderList();
            ClearInputData();
        }

        // delete existing user by id (working)
        private void Delete()
        {
            string connectionString = "Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=Wpf;User ID=ahmad;Password=123123asd;";
            string query = "Delete users where id = @id; ";
            DataTable dt = new DataTable();

            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // The connection will be automatically closed and disposed of when the using block is exited
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (valisdateId())
                        {
                            string id = UserIDtxt.Text;
                            command.Parameters.AddWithValue("@id", id);

                        }
                        else
                        {
                            MessageBox.Show($"Failed invalid input ");
                            return;
                        }


                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                MessageBox.Show($"Success");
                                dt.Load(reader);
                                dataGrid.ItemsSource = dt.DefaultView;
                            }
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show($"{ex}");

            }
            ReRenderList();
            ClearInputData();
        }
        // add user using stored Procedure
        private void AddUserProcedure()
        {
            string connectionString = "Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=Wpf;User ID=ahmad;Password=123123asd;";

            string fullName = FullNametxt.Text;
            string email = Emailtxt.Text;
            string username = UserName.Text;
            string PasswordHash = encrypt(Passwordtxt.Text);

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (ValidateInsert())
                    {
                        SqlCommand command = new SqlCommand("AddUser", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@FullName", fullName);
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@PasswordHash", PasswordHash);
                        connection.Open();
                        command.ExecuteNonQuery();

                    }

                }
            }
            catch (SqlException ex)
            {

                MessageBox.Show($"failed : {ex}");
            }
            ReRenderList();
            ClearInputData();
       }
        
       

        //password BCrypt function

        private string encrypt(string pass)
        {
            string password = pass;
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            return hashedPassword;
        }

        // send defult mail to user 
        private void SendMail()
        {

        }
       
        //validation before inserting 
        private bool ValidateInsert()
        {
            if (FullNametxt.Text == String.Empty)
            {
                MessageBox.Show("Full Name is required", "Faild", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (Emailtxt.Text == String.Empty)
            {
                MessageBox.Show("Email is required", "Faild", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (UserName.Text == String.Empty)
            {
                MessageBox.Show("User Name is required", "Faild", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (Passwordtxt.Text == String.Empty)
            {
                MessageBox.Show(" Password is required", "Faild", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(Emailtxt.Text, EmailregexPattern))
            {
                MessageBox.Show("Invalid Email...");
                return false;
            }

            if (!Regex.IsMatch(UserName.Text, UserNamePattern))
            {
                MessageBox.Show("Invalid Username...");
                return false;
            }
            if (!Regex.IsMatch(Passwordtxt.Text, PasswordPattern))
            {
                MessageBox.Show("Invalid Password ...");
                return false;
            }
            return true;
        }


        //validaton before updating email
        private bool ValidateUpdate()
        {
            if (UserName.Text == String.Empty)
            {
                MessageBox.Show("User Name is required", "Faild", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (Emailtxt.Text == String.Empty)
            {
                MessageBox.Show("Email is required", "Faild", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Regex.IsMatch(UserName.Text, UserNamePattern))
            {
                MessageBox.Show("Invalid Username...");
                return false;
            }

            if (!Regex.IsMatch(Emailtxt.Text, EmailregexPattern))
            {
                MessageBox.Show("Invalid Email...");
                return false;
            }

            return true;
        }

        // clear onclick btn
        private void clear_btn(object sender, RoutedEventArgs e)
        {
            ClearInputData();

        }

        // clear input data  
        public void ClearInputData()
        {
            UserIDtxt.Clear();
            FullNametxt.Clear();
            Emailtxt.Clear();
            UserName.Clear();
            Passwordtxt.Clear();
        }

        //get index of slection in ComboBox onchange 
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          /*  MessageBox.Show($"{combox.Items.IndexOf(combox.SelectedItem)}");*/
            returnIndex(combox.Items.IndexOf(combox.SelectedItem));
        }

        // exit and go to login page
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow LoginPage = new MainWindow();
            LoginPage.Show();
            this.Close();
        }

        //submit button on click do something from the orders list 
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (key)
            {
                case 0:
                    MessageBox.Show($"Delete Case {key}");
                        Delete();
                    break;
                case 1:
                    MessageBox.Show($"Insert Case {key}");
                    Insert();
                    break;
                case 2:
                    MessageBox.Show($"Select Case {key}");
                    Select();
                    break;
                case 3:
                    MessageBox.Show($"Update Case {key}");
                    UpdateUserEmail();
                    break;
                case 4:
                    MessageBox.Show($"Add User ProcedureCase {key}");
                    AddUserProcedure();
                    break;
                case 5:
                    MessageBox.Show($"Send Mail Case {key}");
                    SendMail();
                    break;
                default:
                    MessageBox.Show("something went wrong");
                    break;
            }

        }
    }
    
}
