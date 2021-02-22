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
using System.Windows.Threading;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //TcpClient client = null;
        string IP = "";
        void add_message(string msg)
        {
            ListBoxItem temp = new ListBoxItem();
            temp.Content = msg;
            listbox_msg.Items.Add(temp);
        }
        void move_dot(string direction) 
        {
            try
            {
                switch (direction) 
                {
                    case "UP":
                        {
                            blue_dot.Margin = new Thickness(blue_dot.Margin.Left, blue_dot.Margin.Top - 10, 0, 0);
                            break;
                        }
                    case "DOWN": 
                        {
                            blue_dot.Margin = new Thickness(blue_dot.Margin.Left, blue_dot.Margin.Top + 10, 0, 0);
                            break;
                        }
                    case "LEFT":
                        {
                            blue_dot.Margin = new Thickness(blue_dot.Margin.Left - 10, blue_dot.Margin.Top, 0, 0);
                            break;
                        }
                    case "RIGHT":
                        {
                            blue_dot.Margin = new Thickness(blue_dot.Margin.Left + 10, blue_dot.Margin.Top, 0, 0);
                            break;
                        }
                }
            }
            catch (Exception exept) 
            {
                MessageBox.Show(exept.Message, "Error!");
            }
        }
        void get_updates(object sender, EventArgs e)
        {
            TcpClient client = new TcpClient();
            NetworkStream stream = null;
            string response = "";
            byte[] data = new byte[256];
            try
            {
                //client = new TcpClient();
                client.Connect(IP, 8888);
                data = Encoding.UTF8.GetBytes("GET");
                stream = client.GetStream();
                //сделать запрос
                stream.Write(data, 0, data.Length);
                response = "";
                //получить ответ
                do
                {
                    int count = stream.Read(data, 0, data.Length);
                    response += Encoding.UTF8.GetString(data, 0, count);
                }
                while (stream.DataAvailable);
                //обрабатываем ответ
                if (response.StartsWith("ms"))
                {
                    add_message(response.Remove(0, 2));
                }
                if (response.Equals("b1LEFT"))
                {
                    move_dot("LEFT");
                }
                if (response.Equals("b1RIGHT"))
                {
                    move_dot("RIGHT");
                }
                if (response.Equals("b1UP"))
                {
                    move_dot("UP");
                }
                if (response.Equals("b1DOWN"))
                {
                    move_dot("DOWN");
                }
            }
            catch (Exception exept)
            {
                MessageBox.Show("Error: " + exept.Message, "Error!");
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            
        }
        
        private void start_button_Click(object sender, RoutedEventArgs e)
        {
            int interval = 0;
            try
            {
                interval = Convert.ToInt32(textBox2.Text);
            }
            catch (Exception exept) 
            {
                MessageBox.Show(exept.Message, "Error!");
                return;
            }
            blue_dot.Visibility = Visibility.Visible;
            chat_label.Visibility = Visibility.Visible;
            listbox_msg.Visibility = Visibility.Visible;
            start_button.Visibility = Visibility.Hidden;
            label1.Visibility = Visibility.Hidden;
            textBox1.Visibility = Visibility.Hidden;
            textBox2.Visibility = Visibility.Hidden;
            label2.Visibility = Visibility.Hidden;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, interval);
            timer.Tick += get_updates;
            timer.IsEnabled = true;
            if (!textBox1.Text.Equals(""))
            {
                IP = textBox1.Text;
            }
            else 
            {
                IP = "127.0.0.1";
            }
        }
    }
}
