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

namespace WPF_new_tasks
{
    /// <summary>
    /// Логика взаимодействия для Fridge.xaml
    /// </summary>
    public partial class Fridge : UserControl
    {
        List <string> products_list, current_insides;
        Random ran;
        public Fridge()
        {
            InitializeComponent();
            products_list = new List<string>() { "Рожь", "Кетчунез", "Масло", "Сосиски", "Хлеб", "Яйца", "Сельдерей" };
            current_insides = new List<string>();
            ran = new Random(33);
        }

        private void image2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            image2.Visibility = Visibility.Hidden;
            image1.Visibility = Visibility.Visible;
            listBox1.Visibility = Visibility.Hidden;
            if (current_insides.Count != 0)
            {
                int to_be_removed = ran.Next(current_insides.Count - 1);
                output_textbox.Text = current_insides[to_be_removed] + " съели";
                current_insides.RemoveAt(to_be_removed);
                listBox1.Items.RemoveAt(to_be_removed);
            }
            else
                output_textbox.Text = "";
            add_button.Visibility = Visibility.Hidden;
        }

        private void image1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            image1.Visibility = Visibility.Hidden;
            image2.Visibility = Visibility.Visible;
            listBox1.Visibility = Visibility.Visible;
            add_button.Visibility = Visibility.Visible;
            output_textbox.Text = "";
        }

        private void add_button_Click(object sender, RoutedEventArgs e)
        {
            if (current_insides.Count == 10)
            {
                output_textbox.Text = "Холодильник полон";
            }
            else 
            {
                string new_product = products_list[ran.Next(6)];
                current_insides.Add(new_product);
                ListBoxItem new_food = new ListBoxItem();
                new_food.Content = new_product;
                listBox1.Items.Add(new_food);
                output_textbox.Text = "Добавили " + new_product;
            }
        }
    }
}
