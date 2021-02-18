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

namespace My_work_12_02_21
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        void add_message(string msg)
        {
            ListBoxItem temp = new ListBoxItem();
            temp.Content = msg;
            listbox_msg.Items.Add(temp);
        }
        public MainWindow()
        {
            InitializeComponent();
            TcpClient client = new TcpClient();
            NetworkStream stream = null;
            string response = "";
            byte[] data = new byte[256];
            while (true)
            {
                try
                {
                    //client = new TcpClient();
                    client.Connect("127.0.0.1", 8888);
                    data = Encoding.UTF8.GetBytes("G");
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
                    if (response[0] == 'm')
                    {
                        add_message(response.Remove(0, 2));
                    }
                    if (response.Equals("b1LEFT"))
                    {

                    }
                    if (response.Equals("b1RIGHT"))
                    {

                    }
                    if (response.Equals("b1UP"))
                    {

                    }
                }
                catch (Exception exept)
                {
                    MessageBox.Show("Error: " + exept.Message, "Error!");
                }
            }
        }
    }
}
