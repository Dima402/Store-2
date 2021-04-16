using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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

namespace Tovari
{
    /// <summary>
    /// Interaction logic for Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        //string ConString = ConfigurationManager.ConnectionStrings["Tovari.Properties.Settings.ConnectionString"].ConnectionString;
        string ConString = ConfigurationManager.ConnectionStrings["Tovari.Properties.Settings.SessionsConnectionString"].ConnectionString;
        public Authorization()
        {
            InitializeComponent();
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            enter();
        }

        public void enter()
        {
            string query = "Select Login, Password, Role from Users where Login ='" + Log.Text.Trim() + "' and Password ='" + Pass.Password.Trim() + "'";
            SqlConnection myConnection = new SqlConnection(ConString);
            SqlCommand sda = new SqlCommand(query, myConnection);
            myConnection.Open();
            SqlDataReader rd = sda.ExecuteReader();
            string Login = "null";
            string Password = "null";
            string Role = "null";
            while (rd.Read())
            {
                Login = rd.GetString(0);
                Password = rd.GetString(1);
                Role = rd.GetString(2);
            }
            myConnection.Close();
            if ((Login == "null") || (Password == "null"))
            {
                MessageBox.Show(string.Format("Данные не введены или вы не зарегистрировались"), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            else
            {
                GiveID.Login = Login;
                GiveID.Role = Role;
                MessageBox.Show(string.Format("Вы успешно вошли в систему"), "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                if (Role == "Заказчик")
                {
                    User_page user_Page = new User_page();
                    user_Page.Insertion();
                    user_Page.Show();
                    this.Close();
                }
                if (Role == "Менеджер")
                {
                    Manager_page manager_Page = new Manager_page();
                    manager_Page.Show();
                    this.Hide();
                }
                if (Role == "Кладовщик")
                {
                    Storekeeper_page storekeeper_Page = new Storekeeper_page();
                    storekeeper_Page.Show();
                    this.Close();
                }
                if (Role == "Директор")
                {
                    Director_page director_Page = new Director_page();
                    director_Page.Show();
                    this.Close();
                }
            }
        }

        private void reg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                enter();
            }
        }
    }
}
