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
using System.IO;

namespace Client_prime
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Rectangle> circles = null;
        //TcpClient client = null;
        string IP = "";
        string port = "";
        int failed_attempts = 0;
        bool showing_message_box = false;
        int interval = 0;
        int retry_after = 0;
        DispatcherTimer timer_messages, timer_circles;
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
        void activate_game_screen()
        {
            show(chat_label);
            show(listbox_msg);
            hide(start_button);
            hide(label1);
            hide(textBox1);
            hide(textBox2);
            hide(label2);
            hide(textBox3);
            hide(label3);
            hide(checkBox1);
            /*foreach (UIElement circling in grid1.Children) 
            {
                if (circling is Rectangle) 
                {
                    show(circling);
                }
            }*/
        }
        void deactivate_game_screen()
        {
            hide(chat_label);
            hide(listbox_msg);
            show(start_button);
            show(label1);
            show(textBox1);
            show(textBox2);
            show(label2);
            show(textBox3);
            show(label3);
            show(checkBox1);

            foreach (Rectangle circling in circles)
            {
                if (circling != null)
                {
                    grid1.Children.Remove(circling);
                    //circles.Remove(circling);
                }
            }
            circles.Clear();
            listbox_msg.Items.Clear();
        }
        void goto_pre_game() 
        {

        }
        void goto_game() 
        {

        }
        void goto_post_game() 
        {
            
        }
        void get_messages(object sender, EventArgs e)
        {
            TcpClient client = new TcpClient();
            NetworkStream stream = null;
            string response = "";
            byte[] data = new byte[256];
            try
            {
                //client = new TcpClient();
                client.Connect(IP, Convert.ToInt32(port));
                data = Encoding.UTF8.GetBytes("GET_TEXT:" + listbox_msg.Items.Count.ToString());
                stream = client.GetStream();
                stream.ReadTimeout = 100;
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
                failed_attempts = 0;
                retry_after = interval;
            }
            /*
            catch (Exception exept)
            {
                MessageBox.Show("Exeption type:" + exept.GetType());
                if (exept is SocketException) 
                {
                    //MessageBox.Show("It is a SocketExeption");
                    if ((exept as SocketException).SocketErrorCode == SocketError.ConnectionRefused) 
                    {
                        //MessageBox.Show("It is \"connection refused\"");
                    }
                }
                if (exept is SocketException) 
                {
                    if (failed_attempts > 5) 
                    {
                        MessageBoxResult res = MessageBox.Show("Cannot establish a connection with server\nContinue?", "Error!", MessageBoxButton.YesNo);
                        if (res == MessageBoxResult.Yes) 
                        {
                        }
                    }
                    failed_attempts++;
                }
                //The next line is crucial yet unnecessary
                //MessageBox.Show("Error: " + exept.Message, "Error!");
               
            }
            */
            catch (SocketException exept)
            {
                if (exept.SocketErrorCode == SocketError.ConnectionRefused)
                {
                    if (failed_attempts > 10)
                    {
                        if (!showing_message_box)
                        {
                            timer_circles.Stop();
                            timer_messages.Stop();
                            showing_message_box = true;
                            MessageBoxResult res = MessageBox.Show("Cannot establish a connection with server.\nContinue?", "Error!", MessageBoxButton.YesNo);
                            if (res == MessageBoxResult.No)
                            {
                                deactivate_game_screen();
                            }
                            else
                            {
                                timer_circles.Start();
                                timer_messages.Start();
                            }
                            showing_message_box = false;
                        }
                    }
                    else
                    {
                        retry_after += 200;
                    }
                    failed_attempts++;
                }
                else
                {
                    MessageBox.Show("Error: " + exept.Message, "LOLSock!");
                }
            }
            catch (IOException exept)
            {

                if (failed_attempts > 10)
                {
                    if (!showing_message_box)
                    {
                        timer_circles.Stop();
                        timer_messages.Stop();
                        showing_message_box = true;
                        MessageBoxResult res = MessageBox.Show("Cannot establish a connection with server\nContinue?", "Error!", MessageBoxButton.YesNo);
                        if (res == MessageBoxResult.No)
                        {
                            deactivate_game_screen();
                        }
                        else
                        {
                            timer_circles.Start();
                            timer_messages.Start();
                        }
                        showing_message_box = false;
                    }
                }
                else
                {
                    retry_after += 200;
                }
                failed_attempts++;
            }
            catch (Exception exept)
            {
                /*if (!showing_message_box)
                {
                    showing_message_box = true;
                    MessageBox.Show("Error: " + exept.Message, "Error!");
                    showing_message_box = false;
                }*/
                MessageBox.Show("Error: " + exept.Message + '\n' + exept.GetType(), "LOLExeption!");
                //System.IO.IOExeption
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
                client.Connect(IP, Convert.ToInt32(port));
                data = Encoding.UTF8.GetBytes("GET_CIRCLES");
                stream = client.GetStream();
                stream.ReadTimeout = 100;
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
                failed_attempts = 0;
                retry_after = interval;
            }
            catch (SocketException exept)
            {
                if (exept.SocketErrorCode == SocketError.ConnectionRefused)
                {
                    if (failed_attempts > 10)
                    {
                        if (!showing_message_box)
                        {
                            timer_circles.Stop();
                            timer_messages.Stop();
                            showing_message_box = true;
                            MessageBoxResult res = MessageBox.Show("Cannot establish a connection with server\nContinue?", "Error!", MessageBoxButton.YesNo);
                            if (res == MessageBoxResult.No)
                            {
                                deactivate_game_screen();
                            }
                            else
                            {
                                timer_circles.Start();
                                timer_messages.Start();
                            }
                            showing_message_box = false;
                        }
                    }
                    else
                    {
                        retry_after += 200;
                    }
                    failed_attempts++;
                }
                else
                {
                    MessageBox.Show("Error: " + exept.SocketErrorCode, "LOLSock!");
                }
            }
            catch (IOException exept)
            {

                if (failed_attempts > 10)
                {
                    if (!showing_message_box)
                    {
                        timer_circles.Stop();
                        timer_messages.Stop();
                        showing_message_box = true;
                        MessageBoxResult res = MessageBox.Show("Cannot establish a connection with server\nContinue?", "Error!", MessageBoxButton.YesNo);
                        if (res == MessageBoxResult.No)
                        {
                            deactivate_game_screen();
                        }
                        else
                        {
                            timer_circles.Start();
                            timer_messages.Start();
                        }
                        showing_message_box = false;
                    }
                }
                else
                {
                    retry_after += 200;
                }
                failed_attempts++;
            }
            catch (Exception exept)
            {
                /*if (!showing_message_box)
                {
                    showing_message_box = true;
                    MessageBox.Show("Error: " + exept.Message, "Error!");
                    showing_message_box = false;
                }*/
                MessageBox.Show("Error: " + exept.Message + '\n' + exept.GetType(), "LOLEXE!");
            }
        }
        
        public MainWindow()
        {
            InitializeComponent();
            circles = new List<Rectangle>();
            timer_circles = new DispatcherTimer();
            timer_messages = new DispatcherTimer();
            
        }

        private void start_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                interval = Convert.ToInt32(textBox2.Text);
            }
            catch (Exception exept)
            {
                MessageBox.Show(exept.Message, "Error!");
                return;
            }
            failed_attempts = 0;
            retry_after = interval;

            activate_game_screen();
            if (checkBox1.IsChecked == true)
            {
                IP = "127.0.0.1";
            }
            else
            {
                if (!textBox1.Text.Equals(""))
                {
                    IP = textBox1.Text;
                }
                else
                {
                    IP = "127.0.0.1";
                }
            }
            if (!textBox3.Text.Equals(""))
            {
                port = textBox3.Text;
            }
            else
            {
                port = "8888";
            }
            
            
            timer_circles.Interval = new TimeSpan(0, 0, 0, 0, retry_after);
            timer_circles.Tick += get_circles;
            timer_circles.IsEnabled = true;
            
            timer_messages.Interval = new TimeSpan(0, 0, 0, 0, retry_after);
            timer_messages.Tick += get_messages;
            timer_messages.IsEnabled = true;
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            textBox1.IsEnabled = false;
            label1.IsEnabled = false;
        }

        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            textBox1.IsEnabled = true;
            label1.IsEnabled = true;
        }
    }
}
