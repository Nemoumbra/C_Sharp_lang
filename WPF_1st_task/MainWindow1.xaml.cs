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
        bool is_caps;
        bool is_painted;
        bool is_small;
        public MainWindow()
        {
            InitializeComponent();
            is_caps = false;
            button3.Content = "caps lock";
            is_painted = false;
            is_small = false;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            button1.Visibility = Visibility.Hidden;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            button1.Visibility = Visibility.Visible;
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (is_caps)
            {
                button3.Content = "caps lock";
                is_caps = false;
            }
            else 
            {
                button3.Content = "CAPS LOCK";
                is_caps = true;
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (!is_painted) 
            {
                button4.Background = new SolidColorBrush(Color.FromRgb(177, 91, 91));
                is_painted = true;
            }
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            ScaleTransform make_smaller = new ScaleTransform(0.9, 0.9);
            ScaleTransform make_bigger = new ScaleTransform(1, 1);
            if (is_small)
            {
                button5.RenderTransform = make_bigger;
                is_small = false;
            }
            else 
            {
                button5.RenderTransform = make_smaller;
                is_small = true;
            }
        }
    }
}
