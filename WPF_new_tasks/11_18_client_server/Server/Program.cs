using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace My_server_12_02_21
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            Console.WriteLine("Program started");
            try
            {
                server = new TcpListener(IPAddress.Parse("127.0.0.1"), 8888);
                server.Start();
            }
            catch(Exception exept)
            {
                Console.WriteLine("Error: " + exept.Message);
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Server running");
            TcpClient client = null;
            string message = "";
            byte[] data = null;
            NetworkStream stream = null;
            string ms_to_be_sent = "";
            while (true)
            {
                try
                {
                    client = server.AcceptTcpClient();
                    stream = client.GetStream();
                    message = "0";
                    data = new byte[256];
                    //считать запрос
                    do
                    {
                        int count = stream.Read(data, 0, data.Length);
                        message += Encoding.UTF8.GetString(data, 0, count);
                    }
                    while (stream.DataAvailable);
                    //анализируем запрос
                    //клиент-командир Андрея просит отправить сообщение
                    if ((message[0] == 'm' && message[1] == 's') || (message[0] == 'b') && message[1] == '1') //уточним
                    {
                        //запомнить
                        ms_to_be_sent = message;
                    }
                    //обратился мой клиент-визуализатор
                    if (message[0] == 'G')
                    {
                        data = Encoding.UTF8.GetBytes(ms_to_be_sent);
                        stream.Write(data, 0, data.Length);
                    }
                    stream.Close();
                    client.Close();

                }
                catch (Exception exept)
                {
                    Console.WriteLine("Error: " + exept.Message);
                }
            }
        }
    }
}
