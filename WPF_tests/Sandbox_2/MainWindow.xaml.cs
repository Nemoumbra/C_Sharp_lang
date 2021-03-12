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

namespace Tests
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            //tixtbox.Focusable = false;
            try
            {
                //MessageBox.Show(Keyboard.FocusedElement.ToString(), "Data");
                /*foreach (UIElement elem in grid.Children) 
                {
                    if (elem.IsKeyboardFocused == true) 
                    {
                        MessageBox.Show(elem.ToString(), "data");
                    }
                }*/
                /*if (window.IsKeyboardFocused == true) 
                {
                    MessageBox.Show("LOL", "data");
                }*/
            }
            catch (Exception except) 
            {
                MessageBox.Show("ERROR! " + except.Message, "Error!");
            }
        }

        private void colorful_rectangle_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Left))
            {
                colorful_rectangle.Fill = new SolidColorBrush(Colors.AliceBlue);
            }
        }

        private void colorful_rectangle_MouseLeave(object sender, MouseEventArgs e)
        {
            colorful_rectangle.Fill = new SolidColorBrush(Colors.Black);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show("Window event handler activated! " + e.Key.ToString());
            if (!Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                //Keyboard.Focus(tixtbox);
                return;
            }
            switch (e.Key)
            {
                case Key.Right:
                    {
                        circling.Margin = new Thickness(circling.Margin.Left + 10, circling.Margin.Top, 0, 0);
                        break;
                    }
                case Key.Up:
                    {
                        circling.Margin = new Thickness(circling.Margin.Left, circling.Margin.Top - 10, 0, 0);
                        break;
                    }
                case Key.Left:
                    {
                        circling.Margin = new Thickness(circling.Margin.Left - 10, circling.Margin.Top, 0, 0);
                        break;
                    }
                case Key.Down:
                    {
                        circling.Margin = new Thickness(circling.Margin.Left, circling.Margin.Top + 10, 0, 0);
                        break;
                    }
            }
            /*
            if (e.Key == Key.Left)
            {
                circling.Margin = new Thickness(circling.Margin.Left - 10, circling.Margin.Top, circling.Margin.Right, circling.Margin.Bottom);
            }*/
        }

        private void tixtbox_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show("Tixtbox event handler activated!");
            if (e.Key == Key.Enter)
            {
                tixtbox.Text = "";
                
                //Keyboard.Focus(window);
            }
        }

        private void tixtbox_MouseLeave(object sender, MouseEventArgs e)
        {
            tixtbox.Focusable = false;
            tixtbox.CaretBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        }

        private void tixtbox_MouseEnter(object sender, MouseEventArgs e)
        {
            tixtbox.Focusable = true;
            tixtbox.CaretBrush = null;
            Keyboard.Focus(tixtbox);
        }

        private void colorful_rectangle_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show("Colorful rectangle event handler activated!");
        }

        private void circling_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show("Circling event handler activated!");
        }
    }
}

