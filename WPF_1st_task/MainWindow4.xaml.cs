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
            button2.Visibility = Visibility.Hidden;
            button3.Visibility = Visibility.Hidden;
        }

        private void button1_MouseEnter(object sender, MouseEventArgs e)
        {
            button2.Visibility = Visibility.Visible;
            button1.Visibility = Visibility.Hidden;
        }

        private void button2_MouseEnter(object sender, MouseEventArgs e)
        {
            button2.Visibility = Visibility.Hidden;
            if (rand.Next(100) < 50)
            {
                button1.Visibility = Visibility.Visible;
            }
            else 
            {
                button3.Visibility = Visibility.Visible;
            }
        }

        private void button3_MouseEnter(object sender, MouseEventArgs e)
        {
            button2.Visibility = Visibility.Visible;
            button3.Visibility = Visibility.Hidden;
        }

        
    }
}
