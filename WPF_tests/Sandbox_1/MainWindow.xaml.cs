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

namespace WPF_sandbox
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            bool button_CAPS = true;
        }

        

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //button1.Visibility = Visibility.Visible;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //button1.Visibility = Visibility.Hidden;
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (button3.Content.ToString() == "button")
                button3.Content = "BUTTON";
            else
                button3.Content = "button";
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            var rand = new Random();
            byte red = Convert.ToByte(rand.Next(120));
            byte green = Convert.ToByte(rand.Next(120));
            byte blue = Convert.ToByte(rand.Next(120));
            SolidColorBrush data_for_button = new SolidColorBrush(Color.FromRgb(red, green, blue));
            button4.Background = data_for_button;
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Неверно заполнены поля настроек!", "Ошибка!", MessageBoxButton.OK);
            if (res == MessageBoxResult.OK) 
            {
                MessageBox.Show("F", "f", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
            }
        }

        private void button1_GotFocus(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Button1 gained focus", "");
        }

        private void button1_LostFocus(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Button1 lost focus", "");
        }

        private void button2_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //MessageBox.Show("Button2 gained keyboard focus", "");
        }

        private void button2_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //MessageBox.Show("Button2 lost keyboard focus", "");
        }

        private void button2_GotMouseCapture(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Button2 got mouse capture", "");
        }

        private void button2_LostMouseCapture(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Button2 lost mouse capture", "");
        }

        private void button3_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show("Button3 key down");
        }

        private void button3_KeyUp(object sender, KeyEventArgs e)
        {
            MessageBox.Show("Button3 key up");
        }

        private void button4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Button4 mouse down");
        }

        private void button5_MouseEnter(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Button5 mouse enter");
        }

        private void button6_MouseLeave(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Button6 mouse leave");
        }

        private void button7_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Button7 double click");
        }

        private void button8_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Button8 left mouse button down");
        }

        private void button9_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Button9 left mouse button up");
        }

        private void button10_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Button10 right mouse button down");
        }

        private void button11_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Button11 right mouse button up");
        }

        private void button12_MouseMove(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Button12 mouse move");
        }

        private void button13_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Button13 mouse up");
        }

        private void button14_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            MessageBox.Show("Button14 mouse wheel");
        }

        private void radioButton1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You managed to click the radiobutton");
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text.Equals("shcheeshch"))
            {
                textBox1.Text = "щищ";
            }
            else 
            {
                textBox1.Text = "shcheeshch";
                
            }
        }
    }
}
