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
        bool asleep;
        bool alive;
        int fullness;
        DispatcherTimer timer;
        Random random;

        public Cat()
        {
            InitializeComponent();
            fullness = 100;
            asleep = false;
            alive = true;
            //принято решение обрабатывать список котов из Main-а
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
        }
    }
}
