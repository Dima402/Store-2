using System;
using System.Collections.Generic;
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
using System.Configuration;
using System.Data;

namespace Bookbridge
{
    /// <summary>
    /// Логика взаимодействия для Administration_panel.xaml
    /// </summary>
    public partial class Administration_panel : Window
    {
        SqlDataAdapter adapter;
        DataTable booksTable;
        string ConString = ConfigurationManager.ConnectionStrings["Bookbridge.Properties.Settings.Book_storeConnectionString"].ConnectionString;
        public Administration_panel()
        {
            InitializeComponent();
            FillDataGrid();
        }

        private void FillDataGrid()
        {
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                CmdString = "SELECT Название, Год_издания,  Код_издательства,  Количество_страниц, Количество_экземпляров, Цена, Закупочная_цена, Код_жанра, Код_автора, Код_поставки FROM Книги";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Книги");
                sda.Fill(dt);
                Books_katalog.ItemsSource = dt.DefaultView;
            }
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            if (Books_katalog.SelectedItems != null)
            {
                for (int i = 0; i < Books_katalog.SelectedItems.Count; i++)
                {
                    DataRowView datarowView = Books_katalog.SelectedItems[i] as DataRowView;
                    if (datarowView != null)
                    {
                        DataRow dataRow = (DataRow)datarowView.Row;
                        dataRow.Delete();
                    }
                }
            }
            UpdateDB();
            MessageBox.Show("Удаление завершено", "Сообщение");
        }

        private void UpdateDB()
        {
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapter);
            adapter.Update(booksTable);
        }

        private void Admin_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM Книги";
            booksTable = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);

                // установка команды на добавление для вызова хранимой процедуры
                adapter.InsertCommand = new SqlCommand("sp_InsertBooks", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 100, "Название"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@god_izdaniya", SqlDbType.NVarChar, 100, "Год_издания"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@id_izdatelstva", SqlDbType.Int, 100, "Код_издательства"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@count_pages", SqlDbType.Int, 100, "Количество_страниц"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@count_copy", SqlDbType.Int, 100, "Количество_экземпляров"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@price", SqlDbType.Money, 100, "Цена"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@purchasing_price", SqlDbType.Money, 100, "Закупочная_цена"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@id_genre", SqlDbType.Int, 100, "Код_жанра"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@id_autor", SqlDbType.Int, 100, "Код_автора"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@id_supply", SqlDbType.Int, 100, "Код_поставки"));
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Код_книги");
                parameter.Direction = ParameterDirection.Output;

                connection.Open();
                adapter.Fill(booksTable);
                Books_katalog.ItemsSource = booksTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateDB();
            MessageBox.Show("Добавление завершено", "Сообщение");
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Aut aut = new Aut();
            aut.Show();
            this.Hide();
        }

        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
