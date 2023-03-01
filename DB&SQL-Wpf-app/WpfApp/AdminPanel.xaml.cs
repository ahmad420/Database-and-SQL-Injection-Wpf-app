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

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        public AdminPanel()
        {
            InitializeComponent();

        }
        private int key=0;

        //take index of ComboBox and store it in the local key varible 
        private void returnIndex(int index)
        {
             key = index;
        }

        // get data from DB and Display in DataGrid
        private void ReRenderList()
        {

        }

        //get all users 
        private void GetUsers()
        {


            // Create a SqlConnection object with your connection string
            SqlConnection conn = new SqlConnection("Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=WPF_DB;User ID=ahmad;Password=123123asd;");

            // Open the connection
            conn.Open();

            // Create a SqlCommand object with the query and connection
            SqlCommand cmd = new SqlCommand("SELECT * FROM Users ", conn);



            // Execute the query and get the results
            SqlDataReader reader = cmd.ExecuteReader();

            // Check if the query returned any rows
            if (reader.HasRows)
            {
                // Loop through the rows and do something with the data
                while (reader.Read())
                {


                    string username = reader.GetString(0);
                    string email = reader.GetString(1);
                    string password = reader.GetString(2);
                    MessageBox.Show($"{username}  {email}{password}");
                    // Do something with the data
                }
            }

            // Close the reader and connection
            reader.Close();
            conn.Close();
        }

        //select user from table 
        private void Select()
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }

        //insert new user to users 
        private void Insert()
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }

        }
        //update existing user
        private void UpdateUser()
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }

        }
        // delete existing user 
        private void Delete()
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }
        // add user using stored Procedure
        private void AddUserProcedure()
        {

            // create a SqlConnection object
            using (SqlConnection conn = new SqlConnection("Data Source=LAPTOP-3NQL7H3O\\SQLEXPRESS;Initial Catalog=WPF_DB;User ID=ahmad;Password=123123asd;"))
            {
                // create a SqlCommand object
                using (SqlCommand cmd = new SqlCommand("AddUser", conn))
                {
                    // set the command type to stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;

                    // add parameters to the command object
                    cmd.Parameters.AddWithValue("@Username", "okko3");
                    cmd.Parameters.AddWithValue("@Email", "koko@example.com");
                    cmd.Parameters.AddWithValue("@Password", "password1233");

                    // open the database connection
                    conn.Open();

                    // execute the stored procedure
                    cmd.ExecuteNonQuery();

                    // close the database connection
                    conn.Close();
                }
            }
        }

        // send defult mail to user 
        private void SendMail()
        {

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

        //submit button on click do somthing 
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
                    UpdateUser();
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
