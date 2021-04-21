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
        public Dictionary <string, Circling> remotes;
        public Random rand;
        public my_client(server_part server_var/*TcpListener server, List<string> game_chat, List<Circling> players, Random random*/)
        {
            client = server_var.server.AcceptTcpClient();
            chat = server_var.chat;
            remotes = server_var.remotes;
            rand = server_var.rand;
        }

        public void send_data(NetworkStream stream, string message) 
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);
        }

        public string get_data(NetworkStream stream) 
        {
            string message = "";
            byte[] data = new byte[256];
            do
            {
                int bytes = stream.Read(data, 0, data.Length);
                message += Encoding.UTF8.GetString(data, 0, bytes);
            }
            while (stream.DataAvailable);
            return message;
        }
        public void deal_with_client()
        {
            try
            {
                NetworkStream stream = client.GetStream();
                stream.ReadTimeout = 10000;
                string message = "", response = "";
                message = get_data(stream);
                string[] words = message.Split(':');
                if (words[0].Equals("SEND_TEXT"))
                {
                    if (remotes.Keys.Contains(words[1]))
                    {
                        chat.Add(words[2]); 
                        send_data(stream, "OK");
                        stream.Close();
                        client.Close();
                        return;       
                    }
                }
                if (words[0].Equals("REG_REMOTE"))
                {
                    string key = rand.Next(1000000).ToString();
                    remotes[key] = new Circling(rand.Next(100), rand.Next(100), 33, Convert.ToByte(rand.Next(255)), Convert.ToByte(rand.Next(255)), Convert.ToByte(rand.Next(255)));
                    send_data(stream, key);
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
                        send_data(stream, response);
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
                        send_data(stream, response);
                        stream.Close();
                        client.Close();
                        return;
                    }
                }
                if (words[0].Equals("GET_CIRCLES"))
                {
                    response = remotes.Count.ToString();
                    foreach (Circling kruzhok in remotes.Values) 
                    {
                        response += ":" + kruzhok.x.ToString();
                        response += ":" + kruzhok.y.ToString();
                        response += ":" + kruzhok.d.ToString();
                        response += ":" + kruzhok.red.ToString();
                        response += ":" + kruzhok.green.ToString();
                        response += ":" + kruzhok.blue.ToString();
                    }
                    //Console.WriteLine(response);
                    send_data(stream, response);
                    stream.Close();
                    client.Close();
                    return;
                }
                if (words[0].Equals("MOVE_CIRCLE"))
                {
                    string key = words[1];
                    if (remotes.Keys.Contains(key)) 
                    {
                        switch (words[2])
                        {
                            case "UP":
                                {
                                    remotes[key].y -= remotes[key].d / 2;
                                    break;
                                }
                            case "DOWN":
                                {
                                    remotes[key].y += remotes[key].d / 2;
                                    break;
                                }
                            case "LEFT":
                                {
                                    remotes[key].x -= remotes[key].d / 2;
                                    break;
                                }
                            case "RIGHT":
                                {
                                    remotes[key].x += remotes[key].d / 2;
                                    break;
                                }
                        }
                        send_data(stream, "OK");
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
                send_data(stream, "Wrong request!");
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
        public Dictionary<string, Circling> remotes;
        public Random rand;
        public bool active;
        public server_part(string ip, int port, int seed)
        {
            chat = new List<string>();
            remotes = new Dictionary<string, Circling>();
            rand = new Random(seed);
            try
            {
                server = new TcpListener(IPAddress.Parse(ip), port);
                server.Start();
                Console.WriteLine("Server running");
                active = true;
            }
            catch (Exception exept)
            {
                Console.WriteLine("Error: " + exept.Message);
                active = false;
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
                    /*
                    my_client client = new my_client(this);
                    Thread client_thread = new Thread(client.deal_with_client);
                    client_thread.Start();
                    */
                    if (active)
                    {
                        if (server.Pending())
                        {
                            my_client client = new my_client(this);
                            Thread client_thread = new Thread(client.deal_with_client);
                            client_thread.Start();
                        }
                    }
                    Thread.Sleep(40);
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
        public static string IP = "";
        public static string port = "";

        static void collect_ip_and_port() 
        {
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
        }
        static void Main(string[] args)
        {
            if (args.Length != 0) 
            {
                Console.WriteLine("Command line arguments are not supported yet");
                return;
            }
            Console.WriteLine("Program started");
            if (!File.Exists("Settings.txt"))
            {
                Console.WriteLine("Settings.txt not found");
                collect_ip_and_port();
            }
            else 
            {
                Console.WriteLine("Settings.txt found");
                string[] lines = File.ReadAllLines("Settings.txt");
                if (lines.Length == 2)
                {
                    Console.WriteLine("Picking data from Settings.txt");
                    IP = lines[0];
                    port = lines[1];
                }
                else 
                {
                    Console.WriteLine("Wrong file structure");
                    collect_ip_and_port();
                }
            }
            server_part server = new server_part(IP, Convert.ToInt32(port), 128);
            Thread server_thread = new Thread(server.process_requests);
            server_thread.Start();

            ConsoleKey key;
            while (true) 
            {
                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Enter) 
                {
                    server.active = !server.active;
                    Console.WriteLine(server.active.ToString());
                }
            }
        }
    }
}





