using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_tasks
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rand;
        public MainWindow()
        {
            InitializeComponent();
            rand = new Random();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            grid1.Background = new SolidColorBrush(Color.FromRgb(Convert.ToByte(rand.Next() % 256), Convert.ToByte(rand.Next() % 256), Convert.ToByte(rand.Next() % 256)));
        }
    }
}