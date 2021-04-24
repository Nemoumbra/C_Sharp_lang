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
    enum State 
    {
        NoGame,
        PreGame,
        Game,
        PostGame
    }
    enum GameMode 
    {
        TwoTeams,
        NoTeams
    }
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
    class MyClient
    {
        public TcpClient client;
        public List <string> chat;
        public Dictionary <string, Circling> remotes;
        public Random rand;
        public MyClient(ServerPart server_var/*TcpListener server, List<string> game_chat, List<Circling> players, Random random*/)
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
    class Player 
    {
        public string name;
        //public something related to an avatar
        public int score;
        public Team team;
    }
    class Team 
    {
        public string name;
        public List<Player> participants;
        public int players_count 
        {
            get 
            {
                return participants.Count;
            }
        }
        public bool add_player(Player pl) 
        {
            try
            {
                participants.Add(pl);
                return true;
            }
            catch (Exception exept) 
            {
                return false;
            }
        }
        public int score 
        {
            get 
            {
                int res = 0;
                foreach (Player pl in participants) 
                {
                    res += pl.score;
                }
                return res;
            }
        }
    }
    class ServerPart
    {
        public TcpListener server;
        //public TcpClient client;
        public List<string> chat;
        public Dictionary<string, Circling> remotes;
        public Random rand;
        public bool active;
        public ServerPart(string ip, int port, int seed)
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
                            MyClient client = new MyClient(this);
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
    class MyClient2
    {
        public TcpClient client;
        public List<string> chat;
        public Dictionary<string, Circling> remotes;
        public Random rand;
        int time_limit, frequency;
        List<Team> teams;
        GameMode mode;
        State state;
        int timeout;
        public MyClient2(ServerPart2 server_var, int t_out/*TcpListener server, List<string> game_chat, List<Circling> players, Random random*/)
        {
            client = server_var.server.AcceptTcpClient();
            chat = server_var.chat;
            remotes = server_var.remotes;
            rand = server_var.rand;
            teams = server_var.teams;
            time_limit = server_var.time_limit;
            frequency = server_var.frequency;
            mode = server_var.mode;
            state = server_var.state;
            timeout = t_out;
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
                stream.ReadTimeout = timeout;
                string message = "", response = "";
                message = get_data(stream);
                string[] words = message.Split(':');
                //Здесь всё надо всё перепиcать на switch-case и каждый вариант сделать своей функцией
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
                    /*
                    string key = rand.Next(1000000).ToString();
                    remotes[key] = new Circling(rand.Next(100), rand.Next(100), 33, Convert.ToByte(rand.Next(255)), Convert.ToByte(rand.Next(255)), Convert.ToByte(rand.Next(255)));
                    send_data(stream, key);
                    stream.Close();
                    client.Close();
                    return;
                    */
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
                if (words[0].Equals("GET_STATE")) 
                {
                    response = state.ToString();
                    send_data(stream, response);
                    stream.Close();
                    client.Close();
                    return;
                }
                /*
                if (words[0].Equals("GET_MESSAGE"))
                {

                }
                */
                if (words[0].Equals("GET_RULES"))
                {
                    response = "OK";
                    send_data(stream, response);
                    stream.Close();
                    client.Close();
                    return;
                }
                if (words[0].Equals("GET_PLAYERS_DATA"))
                {
                    response = "OK";
                    send_data(stream, response);
                    stream.Close();
                    client.Close();
                    return;
                }
                if (words[0].Equals("SET_NAME"))
                {

                }
                if (words[0].Equals("UPLOAD_PHOTO"))
                {
                    response = "OK";
                    send_data(stream, response);
                    stream.Close();
                    client.Close();
                    return;
                }
                if (words[0].Equals("DOWNLOAD_PHOTO"))
                {
                    response = "OK";
                    send_data(stream, response);
                    stream.Close();
                    client.Close();
                    return;
                }
                if (words[0].Equals("CONFIRM_PARTICIPATION"))
                {
                    response = "OK";
                    send_data(stream, response);
                    stream.Close();
                    client.Close();
                    return;
                }
                if (words[0].Equals("GET_GAME"))
                {
                    response = "OK";
                    send_data(stream, response);
                    stream.Close();
                    client.Close();
                    return;
                }
                if (words[0].Equals("GET_RESULTS"))
                {
                    response = "OK";
                    send_data(stream, response);
                    stream.Close();
                    client.Close();
                    return;
                }
                if (words[0].Equals("MOVE_PLAYER"))
                {
                    response = "OK";
                    send_data(stream, response);
                    stream.Close();
                    client.Close();
                    return;
                }
                if (words[0].Equals("UNREG_REMOTE")) 
                {
                    response = "OK";
                    send_data(stream, response);
                    stream.Close();
                    client.Close();
                    return;
                }
                /*
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
                */
                /*
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
                */

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
    class ServerPart2 
    {
        public TcpListener server;
        //public TcpClient client;
        public List<string> chat;
        public Dictionary<string, Circling> remotes;
        public Random rand;
        public State state;
        public int time_limit, frequency;
        public GameMode mode;
        public List<Team> teams;
        public ServerPart2(string ip, int port)
        {
            try
            {
                server = new TcpListener(IPAddress.Parse(ip), port);
                server.Start();
                Console.WriteLine("Server running");
                state = State.NoGame;
            }
            catch (Exception exept)
            {
                Console.WriteLine("Error: " + exept.Message);
                Console.ReadKey();
                return;
            }
        }
        public void Initialize(int seed, int time, int frnqcy, GameMode md) 
        {
            chat = new List<string>();
            remotes = new Dictionary<string, Circling>();
            rand = new Random(seed);
            mode = md;
            time_limit = time;
            frequency = frnqcy;
            teams = new List<Team>();
        }
        public void process_requests()
        {
            while (true)
            {
                try
                {
                    if (state != State.NoGame)
                    {
                        if (server.Pending())
                        {
                            MyClient2 client = new MyClient2(this, 10000);
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
            /*
            ServerPart server = new ServerPart2(IP, Convert.ToInt32(port), 128);
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
            }*/
            /*
             * Здесь у нас будет код, который заменит текущий Main.
             * $ Игры нет -> ввод настроек -> настройки введены ->
             * -> запустить игру -> прервать игру -> goto $
            */
            ServerPart2 server = new ServerPart2(IP, Convert.ToInt32(port));
            server.Initialize(128, 10000, 10, GameMode.NoTeams);
            server.state = State.PostGame;
            Thread server_thread = new Thread(server.process_requests);
            server_thread.Start();
        }
    }
}
