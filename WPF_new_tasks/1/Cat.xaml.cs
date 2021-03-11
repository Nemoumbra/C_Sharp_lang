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
using System.Windows.Threading;

namespace WPF_new_tasks
{
    /// <summary>
    /// Логика взаимодействия для Cat.xaml
    /// </summary>
    public partial class Cat : UserControl
    {
        public bool asleep;
        public bool alive;
        int fullness;
        public DispatcherTimer hunger_timer, sleep_timer;
        Random random;

        public Cat()
        {
            InitializeComponent();
            fullness = 100;
            asleep = false;
            alive = true;
            random = new Random();
            sleep_timer = new DispatcherTimer();
            //принято решение обрабатывать список котов из Main-а
        }

        public void get_hungrier(object sender, EventArgs e)
        {
            if (cat_health.Value <= 8)
            {
                cat_health.Value = 0;
                alive = false;
                hunger_timer.IsEnabled = false;
                sleep_timer.IsEnabled = false;
                cat_health.Visibility = Visibility.Hidden;
                cat_image_1.Visibility = Visibility.Hidden;
                cat_image_2.Visibility = Visibility.Hidden;
                cat_label.Visibility = Visibility.Hidden;
            }
            else
            {
                cat_health.Value -= 8;
            }
        }
        public void awake(object sender, EventArgs e) 
        {
            asleep = false;
            sleep_timer.IsEnabled = false;
            cat_image_2.Visibility = Visibility.Hidden;
            cat_image_1.Visibility = Visibility.Visible;
            hunger_timer.IsEnabled = true;
        }
        private void cat_image_1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //кушать
            if (!alive/* || asleep*/)
            {
                return;
            }
            if (cat_health.Value < 100 && cat_health.Value >= 90)
            {
                cat_health.Value = 100;
            }

            if (cat_health.Value < 90)
            {
                cat_health.Value += 10;
            }
        }

        private void cat_image_1_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //уснуть
            if (!alive)
                return;
            cat_image_1.Visibility = Visibility.Hidden;
            cat_image_2.Visibility = Visibility.Visible;
            asleep = true;
            hunger_timer.IsEnabled = false;
            sleep_timer.Tick += awake;
            sleep_timer.Interval = new TimeSpan(0, 0, 3 + random.Next(7));
            sleep_timer.IsEnabled = true;
        }
    }
}
