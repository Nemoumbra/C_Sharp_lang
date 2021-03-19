using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Server
{
    class Circling
    {
        public int x, y, d;
        public string direction;
        public byte red, green, blue;
        public Circling(int x_coord, int y_coord, int diameter, byte R, byte G, byte B)
        {
            x = x_coord;
            y = y_coord;
            d = diameter;
            red = R;
            green = G;
            blue = B;
        }
    }
    class my_client
    {
        public TcpClient client;
        public my_client(TcpListener server)
        {
            client = server.AcceptTcpClient();
        }
        public void deal_with_client()
        {
            NetworkStream stream = client.GetStream();
            string message = "";
            byte[] data = new byte[256];
            do 
            {
                int bytes = stream.Read(data, 0, data.Length);
                message += Encoding.UTF8.GetString(data, 0, bytes);
            }
            while (stream.DataAvailable);
            string[] words = message.Split(':');

        }
    }
    class my_server
    {
        public TcpListener server;
        public List<string> chat;
        public List<Circling> remotes;
        public Random rand;
        public my_server(string ip, int port, int seed)
        {
            chat = new List<string>();
            remotes = new List<Circling>();
            rand = new Random(seed);
            try
            {
                server = new TcpListener(IPAddress.Parse(ip), port);
                server.Start();
                Console.WriteLine("Server running");
            }
            catch (Exception exept)
            {
                Console.WriteLine("Error: " + exept.Message);
                Console.ReadKey();
                return;
            }
        }
        public void process_requests()
        {
            while (true)
            {
                try
                {
                    my_client client = new my_client(server);
                    Thread client_thread = new Thread(client.deal_with_client);
                    client_thread.Start();
                }
                catch (Exception exept)
                {
                    Console.WriteLine("Error: " + exept.Message);
                }
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            /*
            List<string> chat = null;
            //List<int> screens = null;
            List<Circling> remotes = null;
            TcpListener server = null;
            Random rand = null;
            */
            Console.WriteLine("Program started");

            string IP = "", port = "";
            Console.WriteLine("Please enter the IP address or press enter to skip this step");
            IP = Console.ReadLine();
            if (IP.Equals(""))
            {
                IP = "127.0.0.1";
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
            }
            Console.WriteLine("Now enter the port or press enter to skip this step");
            port = Console.ReadLine();
            if (port.Equals(""))
            {
                port = "8888";
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
            }
            /*
            try
            {
                server = new TcpListener(IPAddress.Parse(IP), Convert.ToInt32(port));
                server.Start();
            }
            catch (Exception exept)
            {
                Console.WriteLine("Error: " + exept.Message);
                Console.ReadKey();
                return;
            }*/
            my_server server = new my_server(IP, Convert.ToInt32(port), 123);
            //server.start_server(IP, Convert.ToInt32(port));
            Thread server_thread = new Thread(server.process_requests);
            server_thread.Start();
            //Console.WriteLine("Server running");
            /*
            chat = new List<string>();
            remotes = new List<Circling>();
            //screens = new List<int>();
            rand = new Random(100);
            TcpClient client = null;
            string message = "";
            byte[] data = null;
            NetworkStream stream = null;
            string response = "";
            */
            }
        }
    }
}





