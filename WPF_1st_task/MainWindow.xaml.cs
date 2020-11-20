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
        string text; //to be shown on the button
        string[] colors = {"белый", "красный","оранжевый","жёлтый","зелёный","голубой","синий","фиолетовый","коричневый"};
        string[] words;
        char[] splitter = new char[] {' '};
        public MainWindow()
        {
            InitializeComponent(); 
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            text = textBox1.Text;
            //words = text.Split(splitter);

            button1.Content = text;
        } 
    }
}
