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
        int counter1, counter2, counter3, counter4;
        int angle1, angle2, angle3, angle4;
        Point center;
        public MainWindow()
        {
            InitializeComponent();
            rand = new Random();
            center = new Point(0.5, 0.5);
            button1.RenderTransformOrigin = center;
            button2.RenderTransformOrigin = center;
            button3.RenderTransformOrigin = center;
            button4.RenderTransformOrigin = center;
            angle1 = 30;
            angle2 = 45;
            angle3 = 50;
            angle4 = 20;
            counter1 = 1;
            counter2 = 1;
            counter3 = 1;
            counter4 = 1;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            byte red = Convert.ToByte(rand.Next(250));
            byte green = Convert.ToByte(rand.Next(250));
            byte blue = Convert.ToByte(rand.Next(250));
            button1.Background = new SolidColorBrush(Color.FromRgb(red, green, blue)); 
            button1.RenderTransform = new RotateTransform(angle1*counter1);
            counter1 = (counter1 + 1) % (360 / angle1);
            
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            byte red = Convert.ToByte(rand.Next(250));
            byte green = Convert.ToByte(rand.Next(250));
            byte blue = Convert.ToByte(rand.Next(250));
            button2.Background = new SolidColorBrush(Color.FromRgb(red, green, blue));
            button2.RenderTransform = new RotateTransform(angle2 * counter2);
            counter2 = (counter2 + 1) % (360 / angle2);
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            byte red = Convert.ToByte(rand.Next(250));
            byte green = Convert.ToByte(rand.Next(250));
            byte blue = Convert.ToByte(rand.Next(250));
            button3.Background = new SolidColorBrush(Color.FromRgb(red, green, blue));
            button3.RenderTransform = new RotateTransform(angle3 * counter3);
            counter3 = (counter3 + 1) % (360 / angle3);
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            byte red = Convert.ToByte(rand.Next(250));
            byte green = Convert.ToByte(rand.Next(250));
            byte blue = Convert.ToByte(rand.Next(250));
            button4.Background = new SolidColorBrush(Color.FromRgb(red, green, blue));
            button4.RenderTransform = new RotateTransform(angle4 * counter4);
            counter4 = (counter4 + 1) % (360 / angle4);
        }

        
    }
}
