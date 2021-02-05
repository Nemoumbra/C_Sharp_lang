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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Cat> cats;
        public MainWindow()
        {
            InitializeComponent();
            cats = new List<Cat>();
            int lol;
        }

        private void add_button_Click(object sender, RoutedEventArgs e)
        {
            Cat kitten = new Cat();
            grid.Children.Add(kitten);
        }
    }
}
