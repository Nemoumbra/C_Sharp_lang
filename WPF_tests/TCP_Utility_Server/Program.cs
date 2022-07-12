using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TCP_Utility_Server {
    class Settings {
        public string IP_str;
        public string port_str;
        public string sleep_timeout_ms_str;
        public string read_timeout_ms_str;

        public Settings(string IP_str, string port_str, string sleep_timeout_ms_str, string read_timeout_ms_str) {
            this.IP_str = IP_str;
            this.port_str = port_str;
            this.sleep_timeout_ms_str = sleep_timeout_ms_str;
            this.read_timeout_ms_str = read_timeout_ms_str;
        }

        public Settings() {
            IP_str = "127.0.0.1";
            port_str = "8888";
            sleep_timeout_ms_str = "40";
            read_timeout_ms_str = "10000";
        }

        public void use_default_settings() {
            IP_str = "127.0.0.1";
            port_str = "8888";
            sleep_timeout_ms_str = "40";
            read_timeout_ms_str = "10000";
        }
    }

    //class AnswerRequest {
    //    TcpClient client;
    //    int read_time_ms;

    //    public void send_data(NetworkStream stream, string message) {
    //        byte[] buffer = Encoding.UTF8.GetBytes(message);
    //        stream.Write(buffer, 0, buffer.Length);
    //    }

    //    public string get_data(NetworkStream stream) {
    //        string message = "";
    //        byte[] data = new byte[256];
    //        do {
    //            int bytes = stream.Read(data, 0, data.Length);
    //            message += Encoding.UTF8.GetString(data, 0, bytes);
    //        }
    //        while (stream.DataAvailable);
    //        return message;
    //    }

    //    public AnswerRequest(TcpClient client, int read_time_ms) {
    //        this.client = client;
    //        this.read_time_ms = read_time_ms;
    //    }

    //    public void answer() {
    //        try {
    //            NetworkStream stream = client.GetStream();
    //            stream.ReadTimeout = read_time_ms;
    //            string message, response;
    //            message = get_data(stream);
    //            lock ()
    //        }
    //        catch (Exception e) {

    //        }
    //    }
    //}
    //class Server {
    //    public TcpListener listener;
    //    IPAddress IP;
    //    int port;

    //    int sleep_time_ms;
    //    int read_time_ms;

    //    public Server(IPAddress IP, int port, int sleep_time_ms, int read_time_ms) {
    //        this.IP = IP;
    //        this.port = port;
    //        this.sleep_time_ms = sleep_time_ms;
    //        this.read_time_ms = read_time_ms;
    //        listener = new TcpListener(IP, port);
    //    }

    //    public void listen() {
    //        while (true) {
    //            try {
    //                if (listener.Pending()) {
    //                    AnswerRequest answerer = new AnswerRequest(listener.AcceptTcpClient(), read_time_ms);
    //                    Thread answerer_thread = new Thread(answerer.answer);
    //                    answerer_thread.Start();
    //                }
    //                Thread.Sleep(sleep_time_ms);
    //            }
    //            catch (Exception e) {
    //                Console.WriteLine("Server error!");
    //                Console.WriteLine(e.Message);
    //            }
    //        }
    //    }
    //}

    class Program {
        static object console_locker = new object();

        internal class AnswerRequest {
            TcpClient client;
            NetworkStream stream;
            int read_time_ms;

            public void send_data(NetworkStream stream, string message) {
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                stream.Write(buffer, 0, buffer.Length);
            }

            public string get_data(NetworkStream stream) {
                string message = "";
                byte[] data = new byte[256];
                do {
                    int bytes = stream.Read(data, 0, data.Length);
                    message += Encoding.UTF8.GetString(data, 0, bytes);
                }
                while (stream.DataAvailable);
                return message;
            }

            public AnswerRequest(TcpClient client, int read_time_ms) {
                this.client = client;
                this.read_time_ms = read_time_ms;
            }

            public void answer() {
                try {
                    stream = client.GetStream();
                    stream.ReadTimeout = read_time_ms;
                    string message, response;
                    message = get_data(stream);

                    lock (console_locker) {
                        Console.WriteLine("Received message: {0}", message);
                    }
                    response = "Test response";
                    lock (console_locker) {
                        Console.WriteLine("Sending back {0}", response);
                    }
                    send_data(stream, response);

                    lock (console_locker) {
                        IPEndPoint remote_endPoint = client.Client.RemoteEndPoint as IPEndPoint;
                        Console.WriteLine("Remote endpoint: IP address {0} (port = {1})",
                            remote_endPoint.Address.ToString(), remote_endPoint.Port);
                    }
                }
                catch (Exception e) {
                    lock (console_locker) {
                        Console.WriteLine("Error in AnswerRequest.answer!");
                        Console.WriteLine(e.Message);
                    }
                }
                finally {
                    stream.Close();
                    client.Close();
                    Console.WriteLine();
                }
            }
        }

        internal class Server {
            public TcpListener listener;
            IPAddress IP;
            int port;

            int sleep_time_ms;
            int read_time_ms;

            public Server(IPAddress IP, int port, int sleep_time_ms, int read_time_ms) {
                this.IP = IP;
                this.port = port;
                this.sleep_time_ms = sleep_time_ms;
                this.read_time_ms = read_time_ms;
                listener = new TcpListener(IP, port);
                //listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                
            }

            public void listen() {
                while (true) {
                    try {
                        if (listener.Pending()) {
                            lock (console_locker) {
                                Console.WriteLine("New pending connection, activating answerer...");
                            }
                            AnswerRequest answerer = new AnswerRequest(listener.AcceptTcpClient(), read_time_ms);
                            Thread answerer_thread = new Thread(answerer.answer);
                            answerer_thread.Start();
                        }
                        Thread.Sleep(sleep_time_ms);
                    }
                    catch (Exception e) {
                        lock (console_locker) {
                            Console.WriteLine("Server error!");
                            Console.WriteLine(e.Message);
                            Console.WriteLine();
                        }
                    }
                }
            }
        }

        public static string IP_str;
        public static string port_str;
        public static string sleep_timeout_ms_str;
        public static string read_timeout_ms_str;

        public static IPAddress IP;
        public static int port;
        public static int sleep_timeout_ms;
        public static int read_timeout_ms;

        public static Settings settings;
        public static Server server;

        static void collect_ip_and_port() {
            Console.WriteLine("Please enter the IP address or press enter to skip this step");

            IP_str = Console.ReadLine();
            if (IP_str == "" || !IPAddress.TryParse(IP_str, out IP)) {
                IP_str = settings.IP_str;
                IP = IPAddress.Parse(IP_str);
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
            }

            Console.WriteLine("Now enter the port or press enter to skip this step");

            port_str = Console.ReadLine();
            if (port_str == "" || !int.TryParse(port_str, out port)) {
                port_str = settings.port_str;
                port = int.Parse(port_str);
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
            }

            Console.WriteLine("Now enter the sleep timeout (milliseconds) or press enter to skip this step");

            sleep_timeout_ms_str = Console.ReadLine();
            if (sleep_timeout_ms_str == "" || !int.TryParse(sleep_timeout_ms_str, out sleep_timeout_ms)) {
                sleep_timeout_ms_str = settings.sleep_timeout_ms_str;
                sleep_timeout_ms = int.Parse(sleep_timeout_ms_str);
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
            }

            Console.WriteLine("Now enter the read timeout (milliseconds) or press enter to skip this step");

            read_timeout_ms_str = Console.ReadLine();
            if (read_timeout_ms_str == "" || !int.TryParse(read_timeout_ms_str, out read_timeout_ms)) {
                read_timeout_ms_str = settings.read_timeout_ms_str;
                read_timeout_ms = int.Parse(read_timeout_ms_str);
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
            }
        }

        static void Main(string[] args) {
            if (args.Length != 0) {
                Console.WriteLine("Command line arguments are not supported yet");
                return;
            }

            settings = new Settings();
            Console.WriteLine("Program started\n");

            if (!File.Exists("Settings.txt")) {
                Console.WriteLine("Settings.txt not found");
                settings.use_default_settings(); // may be omitted
                collect_ip_and_port();
            }
            else {
                Console.WriteLine("Settings.txt found");

                string[] lines = File.ReadAllLines("Settings.txt");
                if (lines.Length == 4) {
                    Console.WriteLine("Picking data from Settings.txt\n");

                    IP_str = lines[0];
                    if (!IPAddress.TryParse(IP_str, out IP)) {
                        Console.WriteLine("Cannot use string from Settings.txt as IP");
                        IP_str = settings.IP_str;
                        IP = IPAddress.Parse(IP_str);
                    }

                    port_str = lines[1];
                    if (!int.TryParse(port_str, out port)) {
                        Console.WriteLine("Cannot use string from Settings.txt as port");
                        port_str = settings.port_str;
                        port = int.Parse(port_str);
                    }

                    sleep_timeout_ms_str = lines[2];
                    if (!int.TryParse(sleep_timeout_ms_str, out sleep_timeout_ms)) {
                        Console.WriteLine("Cannot use string from Settings.txt as sleep timeout");
                        sleep_timeout_ms_str = settings.sleep_timeout_ms_str;
                        sleep_timeout_ms = int.Parse(sleep_timeout_ms_str);
                    }

                    read_timeout_ms_str = lines[3];
                    if (!int.TryParse(read_timeout_ms_str, out read_timeout_ms)) {
                        Console.WriteLine("Cannot use string from Settings.txt as read timeout");
                        read_timeout_ms_str = settings.read_timeout_ms_str;
                        read_timeout_ms = int.Parse(read_timeout_ms_str);
                    }
                }
                else {
                    Console.WriteLine("Wrong file structure");
                    settings.use_default_settings(); // may be omitted
                    collect_ip_and_port();
                }
            }

            try {
                server = new Server(IP, port, sleep_timeout_ms, read_timeout_ms);
                Thread server_thread = new Thread(server.listen);
                server_thread.Start();
            }
            catch (Exception e) {
                Console.WriteLine("Error!");
                Console.WriteLine(e.Message);
                return;
            }
            //server = new Server(IPAddress.Parse(IP_str), )

            //string user_input;
            //ConsoleKey key;
            //while (true) {
            //    user_input = Console.ReadLine();
            //
            //}
        }
    }
}
