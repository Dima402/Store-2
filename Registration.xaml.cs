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
using Checker;

namespace Tovari
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        //string ConString = ConfigurationManager.ConnectionStrings["Tovari.Properties.Settings.ConnectionString"].ConnectionString;
        string ConString = ConfigurationManager.ConnectionStrings["Tovari.Properties.Settings.SessionsConnectionString"].ConnectionString;
        Class1 class1 = new Class1();
        public Registration()
        {
            InitializeComponent();
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            if (Log.Text == "" || Pass.Password == "" || FIO.Text == "" || Email.Text == "" || Phone.Text == ""  || Pass_Copy.Password == "")
            {
                MessageBox.Show(
                "Пожалуйста, заполните все поля.",
                "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                string context = class1.First(FIO.Text);
                string context1 = class1.Second(Email.Text);
                string context2 = class1.Third(Phone.Text);

                if (context == "")
                {
                    if (context1 == "")
                    {
                        if (context2 == "")
                        {

                            SqlConnection myConnection = new SqlConnection(ConString);

                            myConnection.Open();

                            string sInsSql = "Insert into Users( Login, Password, Name, Email, Phone, Role) Values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')";

                            string Логин = Log.Text;
                            string Пароль = Pass.Password;
                            string ФИО = FIO.Text;
                            string Почта = Email.Text;
                            string Телефон = Phone.Text;
                            string Роль = "Заказчик";

                            string sInsSotr = string.Format(sInsSql, Логин, Пароль, ФИО, Почта, Телефон, Роль);
                            SqlCommand cmdIns = new SqlCommand(sInsSotr, myConnection);

                            cmdIns.ExecuteNonQuery();

                            MessageBox.Show(
                            "Регистрация прошла успешно.",
                            "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);

                            myConnection.Close();
                        }
                        else
                        {
                            MessageBox.Show(class1.Third(Phone.Text), "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(class1.Second(Email.Text), "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show(class1.First(FIO.Text), "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Authorization authorization = new Authorization();
            authorization.Show();
            this.Close();
        }
    }
}
