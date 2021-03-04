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
        List<Rectangle> circles = null;
        //TcpClient client = null;
        string IP = "";
        void hide(UIElement elem) 
        {
            elem.Visibility = Visibility.Hidden;
        }
        void show(UIElement elem) 
        {
            elem.Visibility = Visibility.Visible;
        }

        void add_message(string msg)
        {
            ListBoxItem temp = new ListBoxItem();
            temp.Content = msg;
            listbox_msg.Items.Add(temp);
        }
        /*
        void move_dot(int index, string direction) 
        {
            try
            {
                switch (direction) 
                {
                    case "UP":
                        {
                            circles[index].Margin = new Thickness(circles[index].Margin.Left, circles[index].Margin.Top - 10, 0, 0);
                            break;
                        }
                    case "DOWN": 
                        {
                            circles[index].Margin = new Thickness(circles[index].Margin.Left, circles[index].Margin.Top + 10, 0, 0);
                            break;
                        }
                    case "LEFT":
                        {
                            circles[index].Margin = new Thickness(circles[index].Margin.Left - 10, circles[index].Margin.Top, 0, 0);
                            break;
                        }
                    case "RIGHT":
                        {
                            circles[index].Margin = new Thickness(circles[index].Margin.Left + 10, circles[index].Margin.Top, 0, 0);
                            break;
                        }
                }
            }
            catch (Exception exept) 
            {
                MessageBox.Show(exept.Message, "Error!");
            }
        }*/
        void get_messages(object sender, EventArgs e) 
        {
            TcpClient client = new TcpClient();
            NetworkStream stream = null;
            string response = "";
            byte[] data = new byte[256];
            try
            {
                //client = new TcpClient();
                client.Connect(IP, 8888);
                data = Encoding.UTF8.GetBytes("GET_TEXT:"+listbox_msg.Items.Count.ToString());
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
                string[] words = response.Split(':');
                int N = Convert.ToInt32(words[0]);
                if (N != 0) 
                {
                    for (int i = 1; i <= N; i++) 
                    {
                        add_message(words[i]);
                    }
                }
            }
            catch (Exception exept)
            {
                MessageBox.Show("Error: " + exept.Message, "Error!");
            }
        }
        void get_circles(object sender, EventArgs e) 
        {
            TcpClient client = new TcpClient();
            NetworkStream stream = null;
            string response = "";
            byte[] data = new byte[256];
            try
            {
                //client = new TcpClient();
                client.Connect(IP, 8888);
                data = Encoding.UTF8.GetBytes("GET_CIRCLES");
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
                //предполагается, что уже существующие кружки пропасть не могут
                //это потом можно будет поправить
                /*
                if (response.Equals("b1LEFT"))
                {
                    move_dot(0, "LEFT");
                }
                if (response.Equals("b1RIGHT"))
                {
                    move_dot(0, "RIGHT");
                }
                if (response.Equals("b1UP"))
                {
                    move_dot(0, "UP");
                }
                if (response.Equals("b1DOWN"))
                {
                    move_dot(0, "DOWN");
                }*/
                string[] words = response.Split(':');
                int N = Convert.ToInt32(words[0]);
                if (N != 0) 
                {
                    //проверяем, появились ли новые
                    if (circles.Count < N) 
                    {
                        for (int i = 0; i < N - circles.Count; i++) 
                        {
                            Rectangle temp = new Rectangle();
                            temp.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            temp.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            temp.Stroke = new SolidColorBrush(Colors.Black);
                            temp.RadiusX = 15.5;
                            temp.RadiusY = 15.5;
                            temp.RenderTransformOrigin = new Point(0.732, 0.634);
                            show(temp);
                            grid1.Children.Add(temp);
                            circles.Add(temp);
                        }
                    }
                    byte R, G, B;
                    int x, y, d;
                    for (int i = 0; i < circles.Count; i++) 
                    {
                        x = Convert.ToInt32(words[6 * i + 1]);
                        y = Convert.ToInt32(words[6 * i + 2]);
                        d = Convert.ToInt32(words[6 * i + 3]);
                        R = Convert.ToByte(words[6 * i + 4]);
                        G = Convert.ToByte(words[6 * i + 5]);
                        B = Convert.ToByte(words[6 * i + 6]);
                        circles[i].Margin = new Thickness(x, y, 0, 0);
                        circles[i].Height = d;
                        circles[i].Width = d;
                        circles[i].Fill = new SolidColorBrush(Color.FromRgb(R, G, B));
                    }
                }
            }
            catch (Exception exept)
            {
                MessageBox.Show("Error: " + exept.Message, "Error!");
            }
        }
        /*
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
                    move_dot(0, "LEFT");
                }
                if (response.Equals("b1RIGHT"))
                {
                    move_dot(0, "RIGHT");
                }
                if (response.Equals("b1UP"))
                {
                    move_dot(0, "UP");
                }
                if (response.Equals("b1DOWN"))
                {
                    move_dot(0, "DOWN");
                }
            }
            catch (Exception exept)
            {
                MessageBox.Show("Error: " + exept.Message, "Error!");
            }
        }*/
        public MainWindow()
        {
            InitializeComponent();
            circles = new List<Rectangle>();
            //circles.Add(blue_dot);
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
            //show(blue_dot);
            show(chat_label);
            show(listbox_msg);
            hide(start_button);
            hide(label1);
            hide(textBox1);
            hide(textBox2);
            hide(label2);
            if (!textBox1.Text.Equals(""))
            {
                IP = textBox1.Text;
            }
            else 
            {
                IP = "127.0.0.1";
            }
            DispatcherTimer timer1 = new DispatcherTimer();
            timer1.Interval = new TimeSpan(0, 0, 0, 0, interval);
            //timer.Tick += get_updates;
            timer1.Tick += get_messages;
            timer1.IsEnabled = true;
            DispatcherTimer timer2 = new DispatcherTimer();
            timer2.Interval = new TimeSpan(0, 0, 0, 0, interval);
            timer2.Tick += get_circles;
            timer2.IsEnabled = true;
        }
    }
}
