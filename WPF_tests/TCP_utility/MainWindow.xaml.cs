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

namespace WpfApplication
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
        public MainWindow()
        {
            InitializeComponent();
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
    }
}

