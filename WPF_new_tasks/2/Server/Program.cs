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
            Random rand = new Random();
            int number = rand.Next(2000);
            //number = 0;
            //List<TcpClient> clients = new List<TcpClient>();
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
            Console.WriteLine("Number = " + number.ToString());
            string message, response = "";
            
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
                Console.WriteLine("New connection! Message:\n" + message);

                if (message.Equals("Let's play!"))
                {
                    //response = "0 " + number.ToString();
                    response = "Make a guess!";
                }
                else 
                {
                    int guess;
                    try
                    {
                        guess = Convert.ToInt32(message);
                        if (guess == number) 
                        {
                            response = "=";
                        }
                        if (guess > number)
                            response = ">";
                        if (guess < number)
                            response = "<";
                    }
                    catch (Exception exept) 
                    {
                        Console.WriteLine("Wrong message: " + exept.Message);
                        response = "Wrong request!";
                    }
                }
                Console.WriteLine("Responding with \"" + response + "\"");
                data = Encoding.UTF8.GetBytes(response);
                stream.Write(data, 0, data.Length);
            }
        }
    }
}

