using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace My_server_12_02_21
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
    class Program
    {
        static void Main(string[] args)
        {
            List<string> chat = null;
            //List<int> screens = null;
            List<Circling> remotes = null;
            TcpListener server = null;
            Random rand = null;
            Console.WriteLine("Program started");

            string IP="", port="";
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
            }

            Console.WriteLine("Server running");
            chat = new List<string>();
            remotes = new List<Circling>();
            //screens = new List<int>();
            rand = new Random(100);
            TcpClient client = null;
            string message = "";
            byte[] data = null;
            NetworkStream stream = null;
            string response = "";
            while (true)
            {
                try
                {
                    client = server.AcceptTcpClient();
                    //Console.WriteLine("New connection!");
                    stream = client.GetStream();
                    message = "";
                    data = new byte[256];
                    //считать запрос
                    do
                    {
                        int count = stream.Read(data, 0, data.Length);
                        message += Encoding.UTF8.GetString(data, 0, count);
                    }
                    while (stream.DataAvailable);
                    //анализируем запрос
                    string[] words = message.Split(':');
                    //прислать сообщение с пульта
                    if (words[0].Equals("SEND_TEXT"))
                    {
                        if (remotes.Count - 1 >= Convert.ToInt32(words[1]))
                        {
                            chat.Add(words[2]);
                            data = Encoding.UTF8.GetBytes("OK");
                            stream.Write(data, 0, data.Length);
                            stream.Close();
                            client.Close();
                            continue;
                        }
                    }
                    //зарегистрироваться в игре как пульт и получить свой собственный кружок
                    if (words[0].Equals("REG_REMOTE")) 
                    {
                        remotes.Add(new Circling(rand.Next(100), rand.Next(100), 33, Convert.ToByte(rand.Next(255)), Convert.ToByte(rand.Next(255)), Convert.ToByte(rand.Next(255))));
                        data = Encoding.UTF8.GetBytes((remotes.Count - 1).ToString());
                        stream.Write(data, 0, data.Length);
                        stream.Close();
                        client.Close();
                        continue;
                    }
                    /*
                    //зарегистрироваться в игре как экран
                    if (words[0].Equals("REG_SCREEN"))
                    {
                        screens.Add(0);
                        data = Encoding.UTF8.GetBytes("OK");
                        stream.Write(data, 0, data.Length);
                        stream.Close();
                        client.Close();
                        continue;
                    }
                    */
                    //получить новые сообщения экраном
                    if (words[0].Equals("GET_TEXT")) 
                    {
                        int N = Convert.ToInt32(words[1]);
                        /*
                        if (screens.Count - 1 <= Convert.ToInt32(words[1])) 
                        {
                            if (screens[N] != chat.Count) //отправить новые сообщения
                            {
                                response = (chat.Count - screens[N]).ToString() + ":"; //вот столько сообщений будет отправлено
                                for (int i = screens[N]; i < chat.Count; i++) 
                                {
                                    response += (chat[i] + ":");
                                }
                                data = Encoding.UTF8.GetBytes(response);
                                stream.Write(data, 0, data.Length);
                                stream.Close();
                                client.Close();
                                continue;
                            }
                        }
                        */
                        if (/*N==0 && chat.Count == 0 ||*/ N >= 0 && N <= chat.Count)
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
                            continue;
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
                            continue;
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
                        continue;
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
                            continue;
                        }
                    }
                    if (words.Length >= 1)
                    {
                        Console.WriteLine(words[1]);
                    }
                    data = Encoding.UTF8.GetBytes("Wrong request!");
                    stream.Write(data, 0, data.Length);
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





