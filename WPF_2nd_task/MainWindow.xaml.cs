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
        Rectangle[] bricks;
        public MainWindow()
        {
            InitializeComponent();
            bricks = new Rectangle[] { rectangle1, rectangle2, rectangle3, rectangle4, rectangle5, rectangle6, rectangle7, rectangle8, rectangle9};
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            grid1.Background = new SolidColorBrush(Colors.Black);
            button1.Visibility = Visibility.Hidden;
            for (int i = 0; i < 9; i++) 
            {
                bricks[i].Visibility = Visibility.Hidden;
            }
        }     
    }
}