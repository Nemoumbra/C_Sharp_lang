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

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TcpClient client = null;
        NetworkStream stream = null;
        string message, response;
        int port = 8888;
        string server = "127.0.0.1";
        Random rand;

        public MainWindow()
        {
            InitializeComponent();
            rand = new Random();
        }

        void connect(string server, int port) 
        {
            try
            {
                client = new TcpClient(server, port);
            }
            catch (Exception exept) 
            {
                MessageBox.Show(exept.Message);
                return;
            }
            stream = client.GetStream();
        }

        void disconnect() 
        {
            stream.Close();
            client.Close();
        }

        void send_request(NetworkStream stream, byte[] buffer, string request) 
        {
            buffer = Encoding.UTF8.GetBytes(request);
            stream.Write(buffer, 0, buffer.Length);
        }

        string get_response(NetworkStream stream, byte[] buffer) 
        {
            string resp = "";
            do 
            {
                int bytes = stream.Read(buffer, 0, buffer.Length);
                resp += Encoding.UTF8.GetString(buffer, 0, bytes);
            }
            while (stream.DataAvailable);
            return resp;
        }

        void guess_the_number() 
        {
            int min=0, max=0, temp;
            string response;
            temp = rand.Next(2500);
            byte[] data = new byte[256];
            Thread.Sleep(5000);
            send_request(stream, data, temp.ToString());
            response = get_response(stream, data);
            if (response.Equals("="))
                Receiver_button.Text = temp.ToString();
            else
            {
                if (response.Equals(">"))
                {
                    /*if (temp == 1)
                    {
                        connect(server, port);
                        send_request(stream, data, "0");
                        response = get_response(stream, data);
                        if (response.Equals("="))
                        {
                            Receiver_button.Text = "0";
                            return;
                        }
                        else 
                        {
                            Receiver_button.Text = "But that's nonsense!";
                            window1.Close();
                            MessageBox.Show("...", "...", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }
                    }*/
                    max = temp;
                    do
                    {
                        temp /= 2;
                        connect(server, port);
                        send_request(stream, data, temp.ToString());
                        response = get_response(stream, data);
                    }
                    while (response.Equals(">"));

                    if (response.Equals("="))
                        Receiver_button.Text = temp.ToString();
                    else 
                    {
                        min = temp;
                    }
                }
                else // "<"
                {
                    min = temp;
                    do
                    {
                        temp *= 2;
                        connect(server, port);
                        send_request(stream, data, temp.ToString());
                        response = get_response(stream, data);
                    }
                    while (response.Equals("<"));
                    if (response.Equals("="))
                        Receiver_button.Text = temp.ToString();
                    else
                    {
                        max = temp;
                    }
                }
                Receiver_button.Text = "Min = " + min.ToString() + ", Max = " + max.ToString();

                do
                {
                    temp = (min + max) / 2;
                    connect(server, port);
                    send_request(stream, data, temp.ToString());
                    response = get_response(stream, data);
                    if (response.Equals("="))
                    {
                        Receiver_button.Text = temp.ToString();
                        return;
                    }
                    else
                    {
                        if (response.Equals("<"))
                        {
                            min = temp;
                        }
                        else
                        {
                            max = temp;
                        }
                    }
                }
                while (max - min > 0);
                Receiver_button.Text = "Min = " + min.ToString() + ", Max = " + max.ToString();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            connect(server, port);
            guess_the_number();
            disconnect();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            connect(server, port);

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            byte[]  data = new byte[256];
            Receiver_button.Text = get_response(stream, data);
        }
    }
}

