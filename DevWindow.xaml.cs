using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;

namespace sql_pract
{
    /// <summary>
    /// Логика взаимодействия для DevWindow.xaml
    /// </summary>
    public partial class DevWindow : Window
    {
        string str = @"Data Source = WIN-U669V8L9R5E; Initial Catalog = TaskManager; Trusted_Connection=True";
        public DevWindow()
        {
            InitializeComponent();

            Load_tables("SELECT Task.* FROM Task,DeveloperTask WHERE Task.ID = DeveloperTask.TaskID");
        }

        async void Load_tables(string func)
        {
            using (SqlConnection connection = new SqlConnection(str))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(func, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    if (reader.FieldCount > 0)
                    {
                        string strtmp = "";
                        list_view.Items.Clear();
                        Load_info_content(reader);

                        while (await reader.ReadAsync())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                strtmp += reader.GetValue(i) + "\t";
                            }
                            strtmp += "\n";
                            list_view.Items.Add(strtmp);
                            strtmp = "";
                        }
                    }
                }
                else
                    System.Windows.Forms.MessageBox.Show("Список пуст");
            }
        }
        void Load_info_content(SqlDataReader reader)
        {
            label_info.Content = "";
            for (int i = 0; i < reader.FieldCount; i++)
            {
                label_info.Content += reader.GetName(i) + "\t";
            }
        }
        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            if (list_view.SelectedIndex != -1)
            {

            }
            else
                System.Windows.Forms.MessageBox.Show("Задание не выбрано.");
        }
    }
}
