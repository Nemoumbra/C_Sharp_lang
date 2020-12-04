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

        bool active, is_writing, is_folder;
        List<KeyValuePair<string, string>> records;
        string russian_text, maths_text;
        public MainWindow()
        {
            InitializeComponent();
            active = true;
            is_writing = false;
            is_folder = false;
            records = new List<KeyValuePair<string, string>>();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (active)
            {
                active = false;
                grid1.Background = new SolidColorBrush(Colors.Black);
                button1.Visibility = Visibility.Hidden;
                button2.Visibility = Visibility.Hidden;
                button4.Visibility = Visibility.Hidden;
                if (is_writing) 
                {
                    button5.Visibility = Visibility.Hidden;
                    button6.Visibility = Visibility.Hidden;
                    textBox1.Visibility = Visibility.Hidden;
                    radioButton1.Visibility = Visibility.Hidden;
                    radioButton2.Visibility = Visibility.Hidden;
                    is_writing = false;
                }
                if (is_folder) 
                {
                    tabControl1.Visibility = Visibility.Hidden;
                    is_folder = false;
                }
            }
            else 
            {
                active = true;
                grid1.Background = new SolidColorBrush(Colors.White);
                button1.Visibility = Visibility.Visible;
                button2.Visibility = Visibility.Visible;
                button4.Visibility = Visibility.Visible;
            }
            
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (!is_writing)
            {
                textBox1.Visibility = Visibility.Visible;
                button5.Visibility = Visibility.Visible;
                button6.Visibility = Visibility.Visible;
                radioButton1.Visibility = Visibility.Visible;
                radioButton2.Visibility = Visibility.Visible;
                is_writing = true;
            }
            else 
            {
                textBox1.Visibility = Visibility.Hidden;
                button5.Visibility = Visibility.Hidden;
                button6.Visibility = Visibility.Hidden;
                radioButton1.Visibility = Visibility.Hidden;
                radioButton2.Visibility = Visibility.Hidden;
                is_writing = false;
            }
            if (is_folder) 
            {
                tabControl1.Visibility = Visibility.Hidden;
                is_folder = false;
            }
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            if (!textBox1.Text.Equals(""))
            {
                if (radioButton2.IsChecked == true)
                {
                    russian_text = textBox1.Text;
                }
                else 
                {
                    maths_text = textBox1.Text;
                }
                textBox1.Text = "";
                is_writing = false;
                button5.Visibility = Visibility.Hidden;
                button6.Visibility = Visibility.Hidden;
                textBox1.Visibility = Visibility.Hidden;
                radioButton1.Visibility = Visibility.Hidden;
                radioButton2.Visibility = Visibility.Hidden;
            }
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            if (!textBox1.Text.Equals("")) 
            {
                textBox1.Text = "";
                is_writing = false;
                button5.Visibility = Visibility.Hidden;
                button6.Visibility = Visibility.Hidden;
                textBox1.Visibility = Visibility.Hidden;
                radioButton1.Visibility = Visibility.Hidden;
                radioButton2.Visibility = Visibility.Hidden;
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (!is_folder)
            {
                if (is_writing)
                {
                    textBox1.Visibility = Visibility.Hidden;
                    button5.Visibility = Visibility.Hidden;
                    button6.Visibility = Visibility.Hidden;
                    radioButton1.Visibility = Visibility.Hidden;
                    radioButton2.Visibility = Visibility.Hidden;
                    is_writing = false;
                }
                is_folder = true;
                tabControl1.Visibility = Visibility.Visible;
            }
            else 
            {
                tabControl1.Visibility = Visibility.Hidden;
                is_folder = false;
            }
        }
    }
}
