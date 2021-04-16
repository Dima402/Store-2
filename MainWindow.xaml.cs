using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Users_last
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlDataAdapter adapter;
        DataTable Table;
        string ConString = ConfigurationManager.ConnectionStrings["Users_last.Properties.Settings.ConnectionString"].ConnectionString;
        public MainWindow()
        {
            InitializeComponent();
            FillDataGrid();
        }

        public void FillDataGrid()
        {
            string sql = "SELECT * from Users_last";
            Table = new DataTable();
            SqlConnection connection = null;

            connection = new SqlConnection(ConString);
            SqlCommand command = new SqlCommand(sql, connection);
            adapter = new SqlDataAdapter(command);

            connection.Open();
            adapter.Fill(Table);
            users.ItemsSource = Table.DefaultView;
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            if (users.SelectedItems != null)
            {
                MessageBoxResult result = MessageBox.Show("Подтвердить удаление?", "Сообщение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    DataRowView row = users.SelectedItem as DataRowView;

                    SqlConnection myConnection = new SqlConnection(ConString);
                    myConnection.Open();
                    string sInsSql = "Delete from Users_last where Id = '" + row.Row.ItemArray[0] + "'";

                    string sInsSotr = string.Format(sInsSql);
                    SqlCommand cmdIns = new SqlCommand(sInsSotr, myConnection);
                    cmdIns.ExecuteNonQuery();
                    FillDataGrid();
                    MessageBox.Show("Удаление завершено", "Сообщение");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выделите требуемую строку в таблице");
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Addition addition = new Addition();
            addition.Show();
            this.Close();
        }

        private void Red_Click(object sender, RoutedEventArgs e)
        {
            /*SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapter);
            adapter.Update(Table);
            MessageBox.Show("Обновление завершено.");*/
            DataRowView row = users.SelectedItem as DataRowView;
            GiveID.redact = row.Row.ItemArray[0].ToString();
            Redaction redaction = new Redaction();
            redaction.Show();
            this.Hide();
        }
    }
}
