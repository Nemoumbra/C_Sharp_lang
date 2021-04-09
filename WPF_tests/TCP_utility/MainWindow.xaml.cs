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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Threading;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TcpClient client = null;
        string message, response;
        int port;
        string server_ip;
        bool active;
        DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            active = false;
            timer = new DispatcherTimer();
        }
        public void hide(UIElement elem) 
        {
            elem.Visibility = Visibility.Hidden;
        }
        public void show(UIElement elem) 
        {
            elem.Visibility = Visibility.Visible;
        }
        public void manage_requests(string message)
        {
            //Проверить, был ли этот запрос раньше
            //Если он был, ничего не делать
            //Если его не было, добавить в combobox
            /*ComboBoxItem temp = new ComboBoxItem();
            temp.Content = message;
            if (!comboBox1.Items.Contains(temp)) 
            {
                comboBox1.Items.Add(temp);
            }*/
            if (!comboBox1.Items.Contains(message))
            {
                comboBox1.Items.Add(message);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client = new TcpClient();
                if (checkBox1.IsChecked == true)
                {
                    server_ip = "127.0.0.1";
                }
                else
                {
                    server_ip = IPTextBox.Text;
                }
                port = Convert.ToInt32(PortTextBox.Text);
                client.Connect(server_ip, port);


                NetworkStream stream = client.GetStream();
                message = Sender_button.Text;
                manage_requests(message);
                byte[] data = Encoding.UTF8.GetBytes(message);
                /*if (checkBox2.IsChecked == false)
                {
                    stream.Write(data, 0, data.Length);
                }
                else 
                {
                    for (int i = 0; i < data.Length; i++) 
                    {
                        stream.Write(data, i, 1);
                        Thread.Sleep(Convert.ToInt32(textBox1.Text));
                    }
                }*/
                if (checkBox2.IsChecked == true)
                {
                    Thread.Sleep(Convert.ToInt32(textBox1.Text));
                }
                stream.Write(data, 0, data.Length);
                response = "";
                do
                {
                    int bytes = stream.Read(data, 0, data.Length);
                    response += Encoding.UTF8.GetString(data, 0, bytes);
                }
                while (stream.DataAvailable);
                Receiver_button.Text = response + "\n";
                stream.Close();
                client.Close();
                Sender_button.Text = "";
            }
            catch (Exception exept)
            {
                MessageBox.Show(exept.Message);
            }
        }
        private void Polling(object sender, EventArgs e)
        {
            try
            {
                client = new TcpClient();
                client.Connect(server_ip, port);
                NetworkStream stream = client.GetStream();
                manage_requests(message);
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
                response = "";
                do
                {
                    int bytes = stream.Read(data, 0, data.Length);
                    response += Encoding.UTF8.GetString(data, 0, bytes);
                }
                while (stream.DataAvailable);
                Receiver_button.Text = response + "\n";
                stream.Close();
                client.Close();
            }
            catch (Exception exept)
            {
                MessageBox.Show(exept.Message);
            }
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            label1.IsEnabled = false;
            IPTextBox.IsEnabled = false;
        }

        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            label1.IsEnabled = true;
            IPTextBox.IsEnabled = true;
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Sender_button.Text = (comboBox1.SelectedItem as ComboBoxItem).Content.ToString();
            Sender_button.Text = comboBox1.SelectedItem.ToString();
        }

        private void checkBox2_Checked(object sender, RoutedEventArgs e)
        {
            label3.IsEnabled = true;
            textBox1.IsEnabled = true;
        }

        private void checkBox2_Unchecked(object sender, RoutedEventArgs e)
        {
            label3.IsEnabled = false;
            textBox1.IsEnabled = false;
        }

        private void Auto_checkbox_Checked(object sender, RoutedEventArgs e)
        {
            hide(checkBox2);
            hide(label3);
            hide(textBox1);
            //Button.Click += Auto_Button_Click;
            hide(Button);
            show(Auto_Button);
            show(Poller_radiobutton);
            show(DOS_radiobutton);
            if (DOS_radiobutton.IsChecked == true)
            {
                show(textBlock2);
                show(textBox2);
            }
            if (Poller_radiobutton.IsChecked == true) 
            {
                show(textBox3);
                show(textBlock3);
            }
        }

        private void Auto_checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            show(checkBox2);
            show(label3);
            show(textBox1);
            show(Button);
            hide(Auto_Button);
            hide(Poller_radiobutton);
            hide(DOS_radiobutton);
            if (DOS_radiobutton.IsChecked == true) 
            {
                hide(textBlock2);
                hide(textBox2);
            }
            if (Poller_radiobutton.IsChecked == true)
            {
                hide(textBox3);
                hide(textBlock3);
            }
            //Button.Click += Button_Click;
        }
        private void DOS() 
        {
            while (active) 
            {
                try
                {
                    client = new TcpClient();
                    client.Connect(server_ip, port);
                    NetworkStream stream = client.GetStream();
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                    client.Close();
                }
                catch (Exception exept)
                {
                    MessageBox.Show(exept.Message);
                }
            }
        }
        private void Auto_Button_Click(object sender, RoutedEventArgs e) 
        {
            if (!active)
            {
                active = true;
                if (checkBox1.IsChecked == true)
                {
                    server_ip = "127.0.0.1";
                }
                else
                {
                    server_ip = IPTextBox.Text;
                }
                port = Convert.ToInt32(PortTextBox.Text);
                message = Sender_button.Text;

                if (Poller_radiobutton.IsChecked == true)
                {
                    int N;
                    try
                    {
                        N = Convert.ToInt32(textBox3.Text);
                        timer.Interval = new TimeSpan(0, 0, 0, 0, N);
                        timer.Tick += Polling;
                        
                        timer.Start();
                    }
                    catch (Exception exept)
                    {
                        MessageBox.Show(exept.Message, "Error!");
                    }
                }
                if (DOS_radiobutton.IsChecked == true)
                {
                    int N;
                    try
                    {
                        N = Convert.ToInt32(textBox2.Text);
                        for (int i = 0; i < N; i++)
                        {
                            Thread thread = new Thread(DOS);
                            thread.Start();
                        }
                    }
                    catch (Exception exept)
                    {
                        MessageBox.Show(exept.Message, "Error!");
                    }
                }
                
            }
            else 
            {
                if (timer.IsEnabled == true) 
                {
                    timer.Stop();
                }
                active = false;
            }
        }

        private void Poller_radiobutton_Checked(object sender, RoutedEventArgs e)
        {
            /*
            try
            {
                hide(textBlock2);
                hide(textBox2);
            }
            catch (Exception exept) 
            {
                MessageBox.Show(exept.ToString(), "Error!");
            }
            */
        }

        private void DOS_radiobutton_Checked(object sender, RoutedEventArgs e)
        {
            show(textBlock2);
            show(textBox2);
        }

        private void DOS_radiobutton_Unchecked(object sender, RoutedEventArgs e)
        {
            hide(textBox2);
            hide(textBlock2);
            show(textBlock3);
            show(textBox3);
        }

        private void Poller_radiobutton_Unchecked(object sender, RoutedEventArgs e)
        {
            hide(textBlock3);
            hide(textBox3);
        }
        /*
        private void Poller_checkbox_Checked(object sender, RoutedEventArgs e)
        {
            DOS_checkbox.IsEnabled = false;
            DOS_checkbox.IsChecked = false;
        }

        private void Poller_checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            DOS_checkbox.IsEnabled = true;
        }

        private void DOS_checkbox_Checked(object sender, RoutedEventArgs e)
        {
            Poller_checkbox.IsChecked = false;
            Poller_checkbox.IsEnabled = false;
        }

        private void DOS_checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            Poller_checkbox.IsEnabled = true;
        }
        */
    }
}

