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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sql_pract
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Team_Lead_btn_Click(object sender, RoutedEventArgs e)
        {
            TeamLeadWindow form = new TeamLeadWindow();
            form.ShowDialog();
        }

        private void Dev_btn_Click(object sender, RoutedEventArgs e)
        {
            DevWindow form = new DevWindow();
            form.ShowDialog();
        }

        private void Tester_btn_Click(object sender, RoutedEventArgs e)
        {
            TesterWindow form = new TesterWindow();
            form.ShowDialog();
        }
    }
}
