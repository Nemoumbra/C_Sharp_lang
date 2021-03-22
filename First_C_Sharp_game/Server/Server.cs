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
        public List <string> chat;
        public List <Circling> remotes;
        public Random rand;
        public my_client(server_part server_var/*TcpListener server, List<string> game_chat, List<Circling> players, Random random*/)
        {
            client = server_var.server.AcceptTcpClient();
            chat = server_var.chat;
            remotes = server_var.remotes;
            rand = server_var.rand;
        }
        public void deal_with_client()
        {
            try
            {
                NetworkStream stream = client.GetStream();
                string message = "", response = "";
                byte[] data = new byte[256];
                do
                {
                    int bytes = stream.Read(data, 0, data.Length);
                    message += Encoding.UTF8.GetString(data, 0, bytes);
                }
                while (stream.DataAvailable);
                string[] words = message.Split(':');
                if (words[0].Equals("SEND_TEXT"))
                {
                    if (remotes.Count - 1 >= Convert.ToInt32(words[1]))
                    {
                        chat.Add(words[2]);
                        data = Encoding.UTF8.GetBytes("OK");
                        stream.Write(data, 0, data.Length);
                        stream.Close();
                        client.Close();
                        return;       
                    }
                }
                if (words[0].Equals("REG_REMOTE"))
                {
                    remotes.Add(new Circling(rand.Next(100), rand.Next(100), 33, Convert.ToByte(rand.Next(255)), Convert.ToByte(rand.Next(255)), Convert.ToByte(rand.Next(255))));
                    data = Encoding.UTF8.GetBytes((remotes.Count - 1).ToString());
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                    client.Close();
                    return;
                }
                if (words[0].Equals("GET_TEXT"))
                {
                    int N = Convert.ToInt32(words[1]);
                    if (N >= 0 && N <= chat.Count)
                    {
                        response = (chat.Count - N).ToString();
                        for (int i = N; i < chat.Count; i++)
                        {
                            response += ":" + chat[i];
                        }
                        data = Encoding.UTF8.GetBytes(response);
                        stream.Write(data, 0, data.Length);
                        stream.Close();
                        client.Close();
                        return;
                    }
                }
                if (words[0].Equals("GET_MESSAGE"))
                {
                    int N = Convert.ToInt32(words[1]);
                    if (N >= 0 && N < chat.Count)
                    {
                        response = chat[N];
                        data = Encoding.UTF8.GetBytes(response);
                        stream.Write(data, 0, data.Length);
                        stream.Close();
                        client.Close();
                        return;
                    }
                }
                if (words[0].Equals("GET_CIRCLES"))
                {
                    response = remotes.Count.ToString();
                    for (int i = 0; i < remotes.Count; i++)
                    {
                        response += ":" + remotes[i].x.ToString();
                        response += ":" + remotes[i].y.ToString();
                        response += ":" + remotes[i].d.ToString();
                        response += ":" + remotes[i].red.ToString();
                        response += ":" + remotes[i].green.ToString();
                        response += ":" + remotes[i].blue.ToString();
                    }
                    data = Encoding.UTF8.GetBytes(response);
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                    client.Close();
                    return;
                }
                if (words[0].Equals("MOVE_CIRCLE"))
                {
                    int N = Convert.ToInt32(words[1]);
                    if (N <= remotes.Count - 1)
                    {
                        switch (words[2])
                        {
                            case "UP":
                                {
                                    remotes[N].y -= remotes[N].d / 2;
                                    break;
                                }
                            case "DOWN":
                                {
                                    remotes[N].y += remotes[N].d / 2;
                                    break;
                                }
                            case "LEFT":
                                {
                                    remotes[N].x -= remotes[N].d / 2;
                                    break;
                                }
                            case "RIGHT":
                                {
                                    remotes[N].x += remotes[N].d / 2;
                                    break;
                                }
                        }
                        data = Encoding.UTF8.GetBytes("OK");
                        stream.Write(data, 0, data.Length);
                        stream.Close();
                        client.Close();
                        return;
                    }
                }
                /*
                if (words.Length > 1) 
                {
                    Console.WriteLine(words[1]);
                }*/
                data = Encoding.UTF8.GetBytes("Wrong request!");
                stream.Write(data, 0, data.Length);
                stream.Close();
                client.Close();
            }
            catch (Exception exept) 
            {
                Console.WriteLine("Error" + exept.Message);
            }
        }
    }
    class server_part
    {
        public TcpListener server;
        //public TcpClient client;
        public List<string> chat;
        public List<Circling> remotes;
        public Random rand;
        public server_part(string ip, int port, int seed)
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
        /*
        public void deal_with_client() 
        {
            NetworkStream stream = client.GetStream();
            string message = "", response="";
            byte[] data = new byte[256];
            do 
            {
                int bytes = stream.Read(data, 0, data.Length);
                message += Encoding.UTF8.GetString(data, 0, bytes);
            }
            while (stream.DataAvailable);
            string[] words = message.Split(':');
        }
        */
        public void process_requests()
        {
            while (true)
            {
                try
                {
                    /*
                    my_client client = new my_client(this);
                    Thread client_thread = new Thread(client.deal_with_client);
                    client_thread.Start();
                    */
                    
                    if (server.Pending()) 
                    {
                        my_client client = new my_client(this);
                        Thread client_thread = new Thread(client.deal_with_client);
                        client_thread.Start();
                    }
                    Thread.Sleep(100);
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
            server_part server = new server_part(IP, Convert.ToInt32(port), 123);
            Thread server_thread = new Thread(server.process_requests);
            server_thread.Start();
            
        }
    }
}





