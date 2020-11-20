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
        string order;
        CheckBox[] ingredients;
        public MainWindow()
        {
            InitializeComponent();
            ingredients = new CheckBox[] {checkBox1, checkBox2, checkBox3, checkBox4, checkBox5, checkBox6, checkBox7, checkBox8, checkBox9, checkBox10};
        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            if (!button1.IsEnabled) 
            {
                button1.IsEnabled = true;   
            }
            order = "Большая пицца\nИнгредиенты:\n";
        }

        private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            if (!button1.IsEnabled)
            {
                button1.IsEnabled = true;
            }
            order = "Средняя пицца\nИнгредиенты:\n";
        }

        private void radioButton3_Checked(object sender, RoutedEventArgs e)
        {
            if (!button1.IsEnabled)
            {
                button1.IsEnabled = true;  
            }
            order = "Маленькая пицца\nИнгредиенты:\n";
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++) 
            {
                if (ingredients[i].IsChecked == true) 
                {
                    order = string.Concat(order, ingredients[i].Content, '\n');
                }
            }
            textBox1.Text = order;
        }

        
    }
}
