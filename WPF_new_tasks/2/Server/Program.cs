using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                int port = 8888;
                IPAddress local_address = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(local_address, port);
                server.Start();
                Console.WriteLine("Server running");
            }
            catch (Exception exept)
            {
                Console.WriteLine(exept.Message);
                Console.ReadKey();
            }
            string message, response;

            while (true)
            {
                message = "";
                TcpClient client = server.AcceptTcpClient();
                NetworkStream stream = client.GetStream();
                byte[] data = new byte[256];
                do
                {
                    int bytes = stream.Read(data, 0, data.Length);
                    message += Encoding.UTF8.GetString(data, 0, bytes);
                }
                while (stream.DataAvailable);
                Console.WriteLine("Message received: " + message);
                response = "Message received: " + message;
                data = Encoding.UTF8.GetBytes(response);
                stream.Write(data, 0, data.Length);
            }
        }
    }
}

