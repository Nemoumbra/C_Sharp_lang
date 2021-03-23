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
        byte[] data = null;
        NetworkStream stream = null;
        string key = "", response = "";
        string IP="", port_str="";
        int port = 0;

        void hide(UIElement elem)
        {
            elem.Visibility = Visibility.Hidden;
        }
        void show(UIElement elem)
        {
            elem.Visibility = Visibility.Visible;
        }

        public MainWindow()
        {
            InitializeComponent();
            //client = new TcpClient();
            data = new byte[256];
        }

        private void up_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client = new TcpClient();
                client.Connect(IP, port);
                stream = client.GetStream();
                data = Encoding.UTF8.GetBytes("MOVE_CIRCLE:" + key + ":UP");
                stream.Write(data, 0, data.Length);
                response = "";
                do
                {
                    int bytes = stream.Read(data, 0, data.Length);
                    response += Encoding.UTF8.GetString(data, 0, bytes);
                }
                while (stream.DataAvailable);
                if (!response.Equals("OK"))
                {
                    MessageBox.Show("Ошибка: сервер не вернул ответ \"OK\"!");
                }
                stream.Close();
                client.Close();
            }
            catch (Exception exept)
            {
                MessageBox.Show(exept.Message, "Ошибка!");
            }
        }

        private void left_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client = new TcpClient();
                client.Connect(IP, port);
                stream = client.GetStream();
                data = Encoding.UTF8.GetBytes("MOVE_CIRCLE:" + key + ":LEFT");
                stream.Write(data, 0, data.Length);
                response = "";
                do
                {
                    int bytes = stream.Read(data, 0, data.Length);
                    response += Encoding.UTF8.GetString(data, 0, bytes);
                }
                while (stream.DataAvailable);
                if (!response.Equals("OK"))
                {
                    MessageBox.Show("Ошибка: сервер не вернул ответ \"OK\"!");
                }
                stream.Close();
                client.Close();
            }
            catch (Exception exept)
            {
                MessageBox.Show(exept.Message, "Ошибка!");
            }
        }

        private void down_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client = new TcpClient();
                client.Connect(IP, Convert.ToInt32(port_textbox.Text));
                stream = client.GetStream();
                data = Encoding.UTF8.GetBytes("MOVE_CIRCLE:" + key + ":DOWN");
                stream.Write(data, 0, data.Length);
                response = "";
                do
                {
                    int bytes = stream.Read(data, 0, data.Length);
                    response += Encoding.UTF8.GetString(data, 0, bytes);
                }
                while (stream.DataAvailable);
                if (!response.Equals("OK"))
                {
                    MessageBox.Show("Ошибка: сервер не вернул ответ \"OK\"!");
                }
                stream.Close();
                client.Close();
            }
            catch (Exception exept)
            {
                MessageBox.Show(exept.Message, "Ошибка!");
            }
        }

        private void right_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client = new TcpClient();
                client.Connect(IP, port);
                stream = client.GetStream();
                data = Encoding.UTF8.GetBytes("MOVE_CIRCLE:" + key + ":RIGHT");
                stream.Write(data, 0, data.Length);
                response = "";
                do
                {
                    int bytes = stream.Read(data, 0, data.Length);
                    response += Encoding.UTF8.GetString(data, 0, bytes);
                }
                while (stream.DataAvailable);
                if (!response.Equals("OK"))
                {
                    MessageBox.Show("Ошибка: сервер не вернул ответ \"OK\"!");
                }
                stream.Close();
                client.Close();
            }
            catch (Exception exept)
            {
                MessageBox.Show(exept.Message, "Ошибка!");
            }
        }

        private void send_button_Click(object sender, RoutedEventArgs e)
        {
            if (!input_textbox.Text.Equals(""))
            {
                try
                {
                    client = new TcpClient();
                    client.Connect(IP, port);
                    stream = client.GetStream();
                    data = Encoding.UTF8.GetBytes("SEND_TEXT:" + key + ":" + input_textbox.Text);
                    stream.Write(data, 0, data.Length);
                    response = "";
                    do
                    {
                        int bytes = stream.Read(data, 0, data.Length);
                        response += Encoding.UTF8.GetString(data, 0, bytes);
                    }
                    while (stream.DataAvailable);
                    if (!response.Equals("OK"))
                    {
                        MessageBox.Show("Ошибка: сервер не вернул ответ \"OK\"!");
                    }
                    else
                    {
                        input_textbox.Text = "";
                    }
                    stream.Close();
                    client.Close();
                }
                catch (Exception exept)
                {
                    MessageBox.Show(exept.Message, "Ошибка!");
                }
            }
        }

        private void reg_button_Click(object sender, RoutedEventArgs e)
        {
            //попытка подключения к серверу. Если успешно, убрать лишние детали интерфейса, показать новые
            //если неудачно, выдать MessageBox с описанием ошибки
            
            try
            {
                client = new TcpClient();
                if (localhost_checkbox.IsChecked == true)
                {
                    IP = "127.0.0.1";
                }
                else 
                {
                    if (!ip_textbox.Text.Equals(""))
                    {
                        IP = ip_textbox.Text;
                    }
                    else
                    {
                        MessageBox.Show("Введите IP адрес!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                if (!port_textbox.Text.Equals(""))
                {
                    port_str = port_textbox.Text;
                }
                else 
                {
                    MessageBox.Show("Введите номер порта!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                port = Convert.ToInt32(port_str);

                client.Connect(IP, port);
                stream = client.GetStream();
                data = Encoding.UTF8.GetBytes("REG_REMOTE");
                stream.Write(data, 0, data.Length);
                response = "";
                do
                {
                    int bytes = stream.Read(data, 0, data.Length);
                    response += Encoding.UTF8.GetString(data, 0, bytes);
                }
                while (stream.DataAvailable);
                key = response;
                client.Close();
                stream.Close();

                hide(reg_button);
                hide(ip_textbox);
                hide(ip_label);
                hide(localhost_checkbox);
                hide(port_textbox);
                hide(port_label);
                show(up_button);
                show(down_button);
                show(left_button);
                show(right_button);
                show(input_textbox);
                show(send_button);
                MessageBox.Show("Регистрация прошла успешно!\nВы в игре!", "Успех!");
            }
            catch (Exception exept)
            {
                MessageBox.Show(exept.Message, "Ошибка!");
            }
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            ip_label.IsEnabled = false;
            ip_textbox.IsEnabled = false;
        }

        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            ip_label.IsEnabled = true;
            ip_textbox.IsEnabled = true;
        }
    }
}

