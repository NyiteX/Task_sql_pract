using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Numerics;

namespace sql_pract
{
    /// <summary>
    /// Логика взаимодействия для TeamLeadWindow.xaml
    /// </summary>
    public partial class TeamLeadWindow : Window
    {
        List<int> dev_ID = new List<int>();
        List<int> task_ID = new List<int>();
        List<int> tester_ID = new List<int>();
        DispatcherTimer timer = new DispatcherTimer();
        string str = @"Data Source = WIN-U669V8L9R5E; Initial Catalog = TaskManager; Trusted_Connection=True";
        public TeamLeadWindow()
        {
            InitializeComponent();


            list_view.Visibility = Visibility.Hidden;
            PrintALL_btn.Visibility = Visibility.Hidden;
            DevTask_btn.Visibility = Visibility.Hidden;
            TestTask_btn.Visibility = Visibility.Hidden;
            ComplitedTask_btn.Visibility = Visibility.Hidden;

            ForRunningName("SELECT Names FROM TeamLead");

            timer.Interval = TimeSpan.FromMilliseconds(150);
            timer.Tick += new EventHandler(RunningName);
            timer.Start();
        }

        public void RunningName(object sender, EventArgs e)
        {
            if (label_adminName.Content.ToString().Count() > 0)
            {
                label_adminName.Content = label_adminName.Content.ToString() + label_adminName.Content.ToString()[0];
                label_adminName.Content = label_adminName.Content.ToString().Remove(0, 1);
            }
        }

        async void ForRunningName(string func)
        {
            using (SqlConnection connection = new SqlConnection(str))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(func, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                       label_adminName.Content += "Welcome, " + reader.GetValue(0);
                        for (int i = 0; i < 100-label_adminName.Content.ToString().Count(); i++)
                        {
                            label_adminName.Content += " ";
                        }
                    }
                }
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
        async void Load_tables(string func)
        {
            task_ID.Clear();
            using (SqlConnection connection = new SqlConnection(str))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(func, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                list_view.Items.Clear();
                
                if (reader.HasRows)
                {
                    if (reader.FieldCount > 0)
                    {
                        string strtmp = "";                       
                        Load_info_content(reader);

                        while (await reader.ReadAsync())
                        {
                            if (func == "SELECT * FROM Task") task_ID.Add((int)reader.GetValue(0));
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

        async void Add_task()
        {
            if (textbox_.Text.Count() > 0)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(str))
                    {
                        await connection.OpenAsync();
                        SqlCommand command = new SqlCommand("INSERT INTO Task(Names,Dates,TeamLeadID,Statuss) VALUES('" + textbox_.Text + "','" + DateTime.Now.ToString() + "',1,0)", connection);
                        await command.ExecuteReaderAsync();
                        System.Windows.Forms.MessageBox.Show("Добавлено");
                        textbox_.Text = "";
                    }
                } catch (Exception e) { System.Windows.Forms.MessageBox.Show(e.Message); }
            }
            else
                System.Windows.Forms.MessageBox.Show("Таска пуста");
        }
        private void PrintALL_btn_Click(object sender, RoutedEventArgs e)
        {
            delete_btn.Visibility = Visibility.Visible;
            feedDev_btn.Visibility = Visibility.Visible;
            Load_tables("SELECT * FROM Task");            
        }

        private void DevTask_btn_Click(object sender, RoutedEventArgs e)
        {
            delete_btn.Visibility = Visibility.Hidden;
            feedDev_btn.Visibility = Visibility.Hidden;
            Load_tables("SELECT Task.* FROM Task,DeveloperTask WHERE Task.ID = DeveloperTask.TaskID");
        }

        private void TestTask_btn_Click(object sender, RoutedEventArgs e)
        {
            delete_btn.Visibility = Visibility.Hidden;
            feedDev_btn.Visibility = Visibility.Hidden;
            Load_tables("SELECT Task.*FROM Task,TesterTask WHERE Task.ID = TesterTask.TaskID");
        }

        private void ComplitedTask_btn_Click(object sender, RoutedEventArgs e)
        {
            delete_btn.Visibility = Visibility.Hidden;
            feedDev_btn.Visibility = Visibility.Hidden;
            Load_tables("SELECT * FROM Task WHERE Statuss = 1");
        }

        private void PrintALL_btn_Copy_Click(object sender, RoutedEventArgs e)
        {
            Add_btn.Visibility = Visibility.Hidden;
            PrintALL_btn.Visibility = Visibility.Visible;
            DevTask_btn.Visibility = Visibility.Visible;
            TestTask_btn.Visibility = Visibility.Visible;
            ComplitedTask_btn.Visibility = Visibility.Visible;

            PrintALL_btn_Copy.Visibility = Visibility.Hidden;
            list_view.Visibility = Visibility.Visible;
            textbox_.Visibility = Visibility.Hidden;           
        }

        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            Add_btn.Visibility = Visibility.Visible;
            PrintALL_btn.Visibility = Visibility.Hidden;
            DevTask_btn.Visibility = Visibility.Hidden;
            TestTask_btn.Visibility = Visibility.Hidden;
            ComplitedTask_btn.Visibility = Visibility.Hidden;

            PrintALL_btn_Copy.Visibility = Visibility.Visible;
            list_view.Visibility = Visibility.Hidden;
            textbox_.Visibility = Visibility.Visible;
            label_info.Content = "";
            list_view.Items.Clear();
            delete_btn.Visibility = Visibility.Hidden;
            feedDev_btn.Visibility = Visibility.Hidden;

            accept_btn_add.Visibility = Visibility.Hidden;
            list_tmp.Visibility = Visibility.Hidden;
        }

        private void Add_btn_Click(object sender, RoutedEventArgs e)
        {
            Add_task();
        }

        async void delete_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var connection = new SqlConnection(str))
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("DELETE FROM Task WHERE ID = (" + task_ID[list_view.SelectedIndex] + ")", connection);
                    await command.ExecuteReaderAsync();
                    System.Windows.Forms.MessageBox.Show("Удалено");
                }
                Load_tables("SELECT * FROM Task");
            }
            catch(Exception er) { System.Windows.Forms.MessageBox.Show("Нельзя удалить. Уже используется."); }
        }

        async private void feedDev_btn_Click(object sender, RoutedEventArgs e)
        {
            if (list_view.SelectedIndex >= 0)
            {
                dev_ID.Clear();
                list_tmp.Visibility = Visibility.Visible;
                using (SqlConnection connection = new SqlConnection(str))
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("SELECT * FROM Developer", connection);
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    list_tmp.Items.Clear();
                    if (reader.HasRows)
                    {
                        if (reader.FieldCount > 0)
                        {
                            string strtmp = "";
                            Load_info_content(reader);

                            while (await reader.ReadAsync())
                            {
                                dev_ID.Add((int)reader.GetValue(0));
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    strtmp += reader.GetValue(i) + "\t";
                                }
                                strtmp += "\n";
                                list_tmp.Items.Add(strtmp);
                                strtmp = "";
                            }
                            delete_btn.Visibility = Visibility.Hidden;
                            list_tmp.Visibility = Visibility.Visible;
                            accept_btn_add.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
        }

         async void accept_btn_add_Click(object sender, RoutedEventArgs e)
        {
            
            using (var connection = new SqlConnection(str))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("INSERT INTO DeveloperTask(DevID,TaskID) VALUES(" + dev_ID[list_tmp.SelectedIndex] + "," + task_ID[list_view.SelectedIndex] + ")", connection);
                await command.ExecuteReaderAsync();
                System.Windows.Forms.MessageBox.Show("Добавлено");
            }
            delete_btn.Visibility = Visibility.Visible;
            list_tmp.Visibility = Visibility.Hidden;
            accept_btn_add.Visibility = Visibility.Hidden;
        }
    }
}
