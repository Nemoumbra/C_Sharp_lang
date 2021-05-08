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
    class GameRules 
    {
        public GameMode game_mode;
        public int coins_frequency;
        public int participants_count;
        //public int? 
        public int time_limit;
        public GameRules(string md, int c_frqnc, int part_c, int t_lim) 
        {
            if (md.Equals("NoTeams"))
                game_mode = GameMode.NoTeams;
            else 
            {
                if (md.Equals("TwoTeams"))
                {
                    game_mode = GameMode.TwoTeams;
                }
                else 
                {
                    //throw something
                }
            }
            coins_frequency = c_frqnc;
            participants_count = part_c;
            time_limit = t_lim;
        }
    }
    class ServerChatMessage 
    {
        public string message;
        public string sender_name;
        public string photo_id;
        public ServerChatMessage(string msg, Player sender) 
        {
            message = msg;
            sender_name = sender.name;
            photo_id = "0";
        }
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
        public bool name_set;
        //public something related to an avatar
        public int? score;
        public Team team;
        public Player() 
        {
            name = "";
            name_set = false;
            score = null;
        }
        public bool remove_from_team() 
        {
            if (team != null) 
            {
                return team.remove_player(this);
            }
            return false;
        }
        public bool participates() 
        {
            return team != null;
        }
    }
    class Team 
    {
        public string name;
        public List<Player> participants;
        public int required_number_of_players;
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
                pl.team = this;
                return true;
            }
            catch (Exception exept) 
            {
                return false;
            }
        }
        public bool remove_player(Player pl) 
        {
            try
            {
                participants.Remove(pl);
                pl.team = null;
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
                    res += pl.score.Value;
                }
                return res;
            }
        }
        public Team(int players_count) 
        {
            participants = new List<Player>();
            required_number_of_players = players_count;
        }
        public bool is_full() 
        {
            return (required_number_of_players == participants.Count);
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
    class ReferenceableInteger 
    {
        public int value
        {
            get;
            set;
        }
    }
    class MyClient2
    {
        public TcpClient client;
        public List<ServerChatMessage> chat;
        //public Dictionary<string, Circling> remotes;
        public Dictionary<string, Player> remotes;
        public Dictionary<string, string> names;
        public Random rand;
        //int time_limit, frequency;
        public GameRules rules;
        List<Team> teams;
        //GameMode mode;
        State state;
        int timeout;
        string[] words;
        NetworkStream stream;
        ReferenceableInteger confirmed_participation;
        public MyClient2(ServerPart2 server_var, int t_out)
        {
            client = server_var.server.AcceptTcpClient();
            chat = server_var.chat;
            remotes = server_var.remotes;
            rand = server_var.rand;
            teams = server_var.teams;
            rules = server_var.rules;
            
            state = server_var.state;
            timeout = t_out;
            names = server_var.names;
            confirmed_participation = server_var.confirmed_participation;
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
        private string API_send_text() 
        {
            string ans;
            if (remotes.Keys.Contains(words[1]))
            {
                //chat.Add(words[2]);
                chat.Add(new ServerChatMessage(words[2], remotes[words[1]]));
                ans = "OK!";
            }
            else 
            {
                ans = "Wrong request";
            }
            return ans;
        }
        private string API_reg_remote()
        {
            string ans;
            if (state == State.PreGame)
            {
                ans = rand.Next(1000000).ToString();
                remotes[ans] = new Player();
                
            }
            else
            {
                ans = "NOT_PRE_GAME";
            }
            return ans;
        }        
        private string API_get_text()
        {
            string ans;
            int N = Convert.ToInt32(words[1]);
            if (N >= 0 && N <= chat.Count)
            {
                ans = (chat.Count - N).ToString();
                for (int i = N; i < chat.Count; i++)
                {
                    ans += ":" + chat[i].message;
                    ans += ":" + chat[i].sender_name;
                    ans += ":" + chat[i].photo_id.ToString();
                }
            }
            else 
            {
                ans = "Wrong request!";
            }
            return ans;
        }
        private string API_get_message()
        {
            string ans;
            int N = Convert.ToInt32(words[1]);
            if (N >= 0 && N < chat.Count)
            {
                ans = chat[N].message;
                ans += ":" + chat[N].sender_name;
                ans += ":" + chat[N].photo_id.ToString();
            }
            else 
            {
                ans = "Wrong request!";
            }
            return ans;
        }
        private string API_get_state()
        {
            string ans;
            ans = state.ToString();
            return ans;
        }
        private string API_get_rules()
        {
            string ans;
            ans = rules.game_mode.ToString() + ":" + rules.time_limit.ToString();
            ans += ":" + rules.coins_frequency.ToString() + ":" + rules.participants_count.ToString();
            return "";
        }
        private string API_get_players_data()
        {
            return "";
        }
        private string API_set_name()
        {
            string ans;
            if (state == State.PreGame && remotes.Keys.Contains(words[1]))
            {
                if (names.Values.Contains(words[2]) && !words[2].Equals(names[words[1]]))
                {
                    ans = "NAME_TAKEN";
                }
                else
                {
                    names[words[1]] = words[2];
                    remotes[words[1]].name = words[2];
                    remotes[words[1]].name_set = true;
                    ans = "OK";
                }
            }
            else 
            {
                ans = "Wrong request!";
            }
            return ans;
        }
        private string API_upload_photo()
        {
            return "";
        }
        private string API_download_photo()
        {
            return "";
        }
        private string API_confirm_participation()
        {
            string ans="";
            if (state == State.PreGame)
            {
                if (!remotes[words[1]].name_set)
                {
                    ans = "NAME_NOT_SELECTED";
                }
                else
                {
                    if (rules.game_mode == GameMode.TwoTeams)
                    {
                        if (!teams[Convert.ToInt32(words[2])].is_full())
                        {
                            teams[Convert.ToInt32(words[2])].add_player(remotes[words[1]]);
                            ans = "OK!";
                        }
                        else
                        {
                            ans = "TEAM_FULL";
                        }
                    }
                    else
                    {
                        if (rules.game_mode == GameMode.NoTeams)
                        {
                            if (confirmed_participation.value <= rules.participants_count) // Принято решение, что не нужно делать ограничений на подключение.
                            { // Пока игра ещё не началась, у всех есть шанс туда попасть.
                                foreach (Team T in teams) //подыскиваем пустое место в списке команд
                                {
                                    if (T.players_count == 0)
                                    {
                                        T.add_player(remotes[words[1]]);
                                        confirmed_participation.value++;
                                        break;
                                    }
                                }
                                ans = "OK!";
                            }
                            else
                            {
                                ans = "MAX_PLAYERS_LIMIT_REACHED";
                            }
                        }
                        /*else
                        {
                            ans = "";// Вообще говоря, до этой ветви кода программа никогда не дойдёт. Это ужасно.
                        }*/
                    }
                }
            }
            else 
            {
                ans = "NOT_PRE_GAME";
            }
            return ans;
        }
        private string API_get_game()
        {
            return "";
        }
        private string API_get_results()
        {
            return "";
        }
        private string API_move_player()
        {
            return "";
        }
        private string API_unreg_remote()
        {
            string ans;
            if (state == State.PreGame && remotes.Keys.Contains(words[1]))
            {
                if (names.Keys.Contains(words[1])) 
                {
                    names.Remove(words[1]);
                }
                if (remotes[words[1]].participates()) 
                {
                    confirmed_participation.value--;
                }
                remotes[words[1]].remove_from_team(); //если он отсутствует в командах, код не ломается
                remotes.Remove(words[1]);
                ans = "OK!";
            }
            else 
            {
                ans = "Wrong request!";
            }
            return ans;
        }

        public void deal_with_client()
        {
            //сразу отсеивать тех, кто подключается в момент NoGame!
            try
            {
                stream = client.GetStream();
                stream.ReadTimeout = timeout;
                string message = "", response = "";
                message = get_data(stream);
                words = message.Split(':');
                //Здесь всё надо всё перепиcать на switch-case и каждый вариант сделать своей функцией
                if (words[0].Equals("SEND_TEXT"))
                {
                    if (remotes.Keys.Contains(words[1]))
                    {
                        chat.Add(new ServerChatMessage(words[2], remotes[words[1]]));
                        response = "OK!";
                    }
                    else
                    {
                        response = "Wrong request";
                    }
                    send_data(stream, response);
                    stream.Close();
                    client.Close();
                    return;
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
                    if (state == State.PreGame)
                    {
                        response = rand.Next(1000000).ToString();
                        remotes[response] = new Player();
                    }
                    else 
                    {
                        response = "NOT_PRE_GAME";
                    }
                    send_data(stream, response);
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
                            response += ":" + chat[i].message;
                            response += ":" + chat[i].sender_name;
                            response += ":" + chat[i].photo_id.ToString();
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
                        response = chat[N].message;
                        response += ":" + chat[N].sender_name;
                        response += ":" + chat[N].photo_id.ToString();
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
                    response = rules.game_mode.ToString() + ":" + rules.time_limit.ToString();
                    response += ":" + rules.coins_frequency.ToString() + ":" + rules.participants_count.ToString();
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
                    if (remotes.Keys.Contains(words[1])) 
                    {
                        if (names.Values.Contains(words[2]) && !words[2].Equals(names[words[1]]))
                        {
                            response = "NAME_TAKEN";
                        }
                        else 
                        {
                            names[words[1]] = words[2];
                            remotes[words[1]].name = words[2];
                            remotes[words[1]].name_set = true;
                            response = "OK!";
                        }
                        send_data(stream, response);
                        stream.Close();
                        client.Close();
                        return;
                    }
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
                    
                    if (!remotes[words[1]].name_set)
                    {
                        response = "NAME_NOT_SELECTED";
                    }
                    else
                    {
                        if (rules.game_mode == GameMode.TwoTeams)
                        {
                            if (!teams[Convert.ToInt32(words[2])].is_full())
                            {
                                teams[Convert.ToInt32(words[2])].add_player(remotes[words[1]]);
                                response = "OK!";
                            }
                            else
                            {
                                response = "TEAM_FULL";
                            }
                        }
                        else
                        {
                            if (rules.game_mode == GameMode.NoTeams)
                            {
                                if (confirmed_participation.value <= rules.participants_count)
                                {
                                    foreach (Team T in teams) //подыскиваем пустое место в списке команд
                                    {
                                        if (T.players_count == 0)
                                        {
                                            T.add_player(remotes[words[1]]);
                                            confirmed_participation.value++;
                                            break;
                                        }
                                    }
                                    response = "OK!";
                                }
                                else
                                {
                                    response = "MAX_PLAYERS_LIMIT_REACHED";
                                }
                            }
                            else
                            {
                                response = "";// Вообще говоря, до этой ветви кода программа никогда не дойдёт. Это ужасно.
                            }
                        }
                    }
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
                    if (remotes.Keys.Contains(words[1]))
                    {
                        if (names.Keys.Contains(words[1]))
                        {
                            names.Remove(words[1]);
                        }
                        if (remotes[words[1]].participates())
                        {
                            confirmed_participation.value--;
                        }
                        remotes[words[1]].remove_from_team(); //если он отсутствует в командах, код не ломается
                        remotes.Remove(words[1]);
                        response = "OK!";
                    }
                    else
                    {
                        response = "Wrong request!";
                    }
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
                /*
                 * Вот здесь у нас будет switch-case с функциями
                */
                /*if (state == State.NoGame)
                {
                    response = "NO_GAME";
                }
                else
                {
                    switch (words[1])
                    {
                        case "SEND_TEXT":
                            {
                                response = API_send_text();
                                break;
                            }
                        case "REG_REMOTE":
                            {
                                response = API_reg_remote();
                                break;
                            }
                        case "GET_TEXT":
                            {
                                response = API_get_text();
                                break;
                            }
                        case "GET_MESSAGE":
                            {
                                response = API_get_message();
                                break;
                            }
                        case "GET_STATE":
                            {
                                response = API_get_state();
                                break;
                            }
                        case "GET_RULES":
                            {
                                response = API_get_rules();
                                break;
                            }
                        case "GET_PLAYERS_DATA":
                            {
                                response = API_get_players_data();
                                break;
                            }
                        case "SET_NAME":
                            {
                                response = API_set_name();
                                break;
                            }
                        case "CONFIRM_PARTICIPATION":
                            {
                                response = API_confirm_participation();
                                break;
                            }
                        case "UPLOAD_PHOTO":
                            {
                                response = API_upload_photo();
                                break;
                            }
                        case "DOWNLOAD_PHOTO":
                            {
                                response = API_download_photo();
                                break;
                            }
                        case "GET_GAME":
                            {
                                response = API_get_game();
                                break;
                            }
                        case "GET_RESULTS":
                            {
                                response = API_get_results();
                                break;
                            }
                        case "MOVE_PLAYER":
                            {
                                response = API_move_player();
                                break;
                            }
                        case "UNREG_REMOTE":
                            {
                                response = API_unreg_remote();
                                break;
                            }
                        default:
                            {
                                response = "Wrong request!";
                                break;
                            }
                    }
                    //send_data(stream, response);
                    //client.Close();
                    //stream.Close();
                }*/
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
        public List<ServerChatMessage> chat;
        public Dictionary<string, Player> remotes;
        public Random rand;
        public State state;
        public List<Team> teams;
        public Dictionary<string, string> names;
        public GameRules rules;
        public ReferenceableInteger confirmed_participation;
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
        /*
        public void Initialize(int seed, GameRules rls) 
        {
            chat = new List<string>();
            remotes = new Dictionary<string, Player>();
            rand = new Random(seed);
            rules = rls;
            teams = new List<Team>();
            names = new Dictionary<string, string>();
        }*/
        public void process_requests()
        {
            int temp_counter=0;
            while (true)
            {
                try
                {
                    if (server.Pending())
                    {
                        MyClient2 client = new MyClient2(this, 10000);
                        Thread client_thread = new Thread(client.deal_with_client);
                        client_thread.Start();
                    }
                    
                    if (state == State.PreGame)
                    {
                        if (rules.game_mode == GameMode.TwoTeams)
                        {
                            // Проверить, не заполнены ли команды
                            if (teams[0].is_full() && teams[1].is_full()) 
                            {
                                temp_counter++;
                                if (temp_counter > 100) 
                                {
                                    state = State.Game;
                                    temp_counter = 0;
                                    Console.WriteLine("lol");
                                }
                            }
                        }
                        else
                        {
                            if (rules.game_mode == GameMode.NoTeams)
                            {
                                // Проверить, не все ли зарегистрированные игроки подтвердили участие
                                if (remotes.Count != 0 && confirmed_participation.value == remotes.Count) 
                                {
                                    temp_counter++;
                                    if (temp_counter > 100)
                                    {
                                        state = State.Game;
                                        temp_counter = 0;
                                    }
                                }
                            }
                        }
                    }
                    Thread.Sleep(30);
                }
                catch (Exception exept)
                {
                    Console.WriteLine("Error: " + exept.Message);
                }
            }
        }
        public void start_new_game(int seed, GameRules rls) 
        {
            chat = new List<ServerChatMessage>();
            remotes = new Dictionary<string, Player>();
            rand = new Random(seed);
            rules = rls;
            teams = new List<Team>();
            if (rules.game_mode == GameMode.TwoTeams)
            {
                teams.Add(new Team(rules.participants_count));
                teams.Add(new Team(rules.participants_count));
            }
            else 
            {
                if (rules.game_mode == GameMode.NoTeams) 
                {
                    for (int i = 0; i < rules.participants_count; i++) 
                    {
                        teams.Add(new Team(1));
                    }
                }
            }
            
            names = new Dictionary<string, string>();
            state = State.PreGame;
            confirmed_participation = new ReferenceableInteger();
            confirmed_participation.value = 0;
        }

        public void stop_current_game() 
        {

        }
    }

    class Program
    {
        public static string IP = "", port = "", frequency = "", game_mode = "";
        public static string time_limit = "", participants = "";
        public static string user_input = "";
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
        static bool collect_game_rules() 
        {
            Console.WriteLine("Please enter the frequency which will define how often a new coin will be generated during the game");
            frequency = Console.ReadLine();
            Console.WriteLine("Please enter the GameMode parameter (either NoTeams or TwoTeams)");
            game_mode = Console.ReadLine();
            if (game_mode.Equals("NoTeams")) 
            {
                Console.WriteLine("Please enter the limit on players (maximum number of participants)");
                participants = Console.ReadLine();
            }
            else if (game_mode.Equals("TwoTeams"))
            {
                Console.WriteLine("Please enter how many players are in each team (two teams have the same number of participants)");
                participants = Console.ReadLine();
            }
            else 
            {
                Console.WriteLine("Wrong GameMode!");
                //restart game
                return false;
            }
            Console.WriteLine("Please enter the time limit which will define how long will the game last");
            time_limit = Console.ReadLine();
            return true;
        }
        static void Main(string[] args)
        {
            /*
            if (args.Length != 0) 
            {
                Console.WriteLine("Command line arguments are not supported yet");
                return;
            }
            */
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
            while (true) 
            {
                user_input = Console.ReadLine();
                if (server.state == State.NoGame && user_input.Equals("Start game"))
                {
                    if (!collect_game_rules()) 
                    {
                        continue;
                    }
                    GameRules game_rules = new GameRules(game_mode, Convert.ToInt32(frequency), Convert.ToInt32(participants), Convert.ToInt32(time_limit));
                    server.start_new_game(128, game_rules);
                    Thread server_thread = new Thread(server.process_requests);
                    server_thread.Start();
                    //или запихнуть запуск потока в server.start_new_game
                }
                else
                {
                    if (server.state != State.NoGame && user_input.Equals("Stop game"))
                    {
                        server.stop_current_game();
                    }
                    else 
                    {
                        Console.WriteLine("Wrong command!");
                    }
                }
            }
            /*collect_game_rules();
            server.Initialize(128, 10000, 10, GameMode.NoTeams);
            server.state = State.PreGame;
            Thread server_thread = new Thread(server.process_requests);
            server_thread.Start();*/
        }
    }
}
