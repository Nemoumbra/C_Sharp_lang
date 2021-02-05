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

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TcpClient client = null;
        string message, response;
        int port = 8888;
        string server = "127.0.0.1";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client = new TcpClient();
                client.Connect(server, port);
            }
            catch (Exception exept)
            {
                MessageBox.Show(exept.Message);
            }
            NetworkStream stream = client.GetStream();
            message = Sender_button.Text;
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            response = "";
            do
            {
                int bytes = stream.Read(data, 0, data.Length);
                response += Encoding.UTF8.GetString(data, 0, data.Length);
            }
            while (stream.DataAvailable);
            Receiver_button.Text = response + "\n";
            stream.Close();
            client.Close();
            Sender_button.Text = "";
        }
    }
}
