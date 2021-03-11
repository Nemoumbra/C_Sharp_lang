using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPF_new_tasks
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Cat> cats = new List<Cat>();
        Random rnd;
        int height = 80;
        int width = 80;
        public MainWindow()
        {
            InitializeComponent();
            rnd = new Random(132);
            DispatcherTimer walking_timer = new DispatcherTimer();
            walking_timer.Tick += manage_cats;
            walking_timer.Interval = new TimeSpan(0, 0, 0, 1, 700);
            walking_timer.IsEnabled = true;
        }
        public void manage_cats(object sender, EventArgs e)  
        {
            foreach (Cat kitten in cats) 
            {
                if (kitten.alive && !kitten.asleep)
                {
                    double cur_left = kitten.Margin.Left;
                    double cur_top = kitten.Margin.Top;
                    kitten.Margin = new Thickness(cur_left + rnd.Next(0, 5), cur_top + rnd.Next(0, 5), 0, 0);
                }
            }
        }
        private void add_button_Click(object sender, RoutedEventArgs e)
        {
            Cat kitten = new Cat();
            kitten.Margin = new Thickness(rnd.Next(50, 650), rnd.Next(50, 200), 0, 0);
            kitten.cat_label.Content = textBox1.Text;
            textBox1.Text = "";
            kitten.cat_health.Value = rnd.Next(30, 100);
            cats.Add(kitten);
            /*kitten.Height = height;
            kitten.Width = width;*/
            grid.Children.Add(kitten);
            kitten.hunger_timer = new DispatcherTimer();
            kitten.hunger_timer.Interval = new TimeSpan(0, 0, 5);
            kitten.hunger_timer.Tick += kitten.get_hungrier;
            kitten.hunger_timer.IsEnabled = true;
        }
    }
}
