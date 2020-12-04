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
        Random what_rand, who_rand, in_what_rand, place_rand;
        int temp;
        public MainWindow()
        {
            InitializeComponent();
            what_rand = new Random(33);
            who_rand = new Random(44);
            in_what_rand = new Random(55);
            place_rand = new Random(22);
        }

        private void what_button_Click(object sender, RoutedEventArgs e)
        {
            if (!word_textBox.Text.Equals("")) 
            {
                what_listBox.Items.Add(word_textBox.Text);
                word_textBox.Text = "";
            }
        }

        private void who_button_Click(object sender, RoutedEventArgs e)
        {
            if (!word_textBox.Text.Equals(""))
            {
                who_listBox.Items.Add(word_textBox.Text);
                word_textBox.Text = "";
            }
        }

        private void in_what_button_Click(object sender, RoutedEventArgs e)
        {
            if (!word_textBox.Text.Equals(""))
            {
                in_what_listBox.Items.Add(word_textBox.Text);
                word_textBox.Text = "";
            }
        }

        private void place_button_Click(object sender, RoutedEventArgs e)
        {
            if (!word_textBox.Text.Equals(""))
            {
                place_listBox.Items.Add(word_textBox.Text);
                word_textBox.Text = "";
            }
        }

        private void generate_button_Click(object sender, RoutedEventArgs e)
        {
            //выбрать what, потом выбрать who, затем in what, напоследок place
            temp = what_rand.Next(what_listBox.Items.Count - 1);
            
            what_textBox.Text = ((ListBoxItem)what_listBox.Items[temp]).Content.ToString();

            temp = who_rand.Next(who_listBox.Items.Count - 1);
            who_textBox.Text = ((ListBoxItem)who_listBox.Items[temp]).Content.ToString();

            temp = in_what_rand.Next(in_what_listBox.Items.Count - 1);
            in_what_textBox.Text = ((ListBoxItem)in_what_listBox.Items[temp]).Content.ToString();

            temp = place_rand.Next(place_listBox.Items.Count - 1);
            place_textBox.Text = ((ListBoxItem)place_listBox.Items[temp]).Content.ToString();

        }

       
    }
}
