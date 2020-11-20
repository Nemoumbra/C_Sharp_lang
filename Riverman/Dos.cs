using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Threads
{
    class Program
    {
        static object locker = new object();
        class Riverman 
        {
            public int X, Y;
            public int delta_x;
            public bool is_moving;
            public int delta_time;
            public string texture_front;
            public string texture_back;
            public int start;
            public int end;
            public ConsoleKey stop_key;
            //public string spaces;
            public Riverman(string person_front, string person_back, int speed, int row, int coast1, int coast2, ConsoleKey button)
            {
                X = coast1;
                Y = row;
                delta_x = 1;
                is_moving = true;
                delta_time = speed;
                texture_front = person_front;
                texture_back = person_back;
                start = coast1;
                end = coast2;
                stop_key = button;

            }
            public void change_status() 
            {
                if (is_moving)
                    is_moving = false;
                else
                    is_moving = true;
            }
            public void sail()
            {
                int old_X = X;
                while (true) 
                {
                    if (is_moving)
                    {
                        lock (locker)
                        {
                            Console.SetCursorPosition(old_X, Y);
                            Console.Write("     ");
                        }
                        old_X = X;
                        lock (locker)
                        {
                            Console.SetCursorPosition(X, Y);
                            if (delta_x == 1)
                                Console.Write(texture_front);
                            else
                                Console.Write(texture_back);
                        }
                        if (X + texture_front.Length == end + 1) 
                        {
                            delta_x = -1;

                        }
                        if (X == start) 
                        {
                            delta_x = 1;
                        }
                        X += delta_x;

                    }
                    Thread.Sleep(delta_time);
                }
            } 
        }
        class Controller 
        {
            //int number;
            public Riverman[] array;
            public ConsoleKey key;
            public Controller(Riverman[] data) 
            {
                array = data;
            }
            public void process_keys() 
            {
                while (true)
                {
                    if (Console.KeyAvailable)
                    {
                        key = Console.ReadKey(true).Key;
                        foreach (Riverman person in array)
                        {
                            if (key == person.stop_key)
                            {
                                person.change_status();
                            }
                        }
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            string front = "\\_R_/";
            string back = "\\_Я_/";
            ConsoleKey[] keys = {ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3, ConsoleKey.D4, ConsoleKey.D5, ConsoleKey.D6, ConsoleKey.D7, ConsoleKey.D8, ConsoleKey.D9};
            int n;
            n = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            Riverman[] sailers = new Riverman[n];
            
            for (int i = 0; i < n; i++) 
            {
                Riverman data_for_dos = new Riverman(front, back, 50 * (i+1), 4 * i, 4, Console.WindowWidth - 2, keys[i]);
                sailers[i] = data_for_dos;
            }
            Controller data_for_key_manager = new Controller(sailers);
            Thread key_manager = new Thread(data_for_key_manager.process_keys);
            for (int i = 0; i < n; i++)
            {
                Thread sailer = new Thread(sailers[i].sail);
                sailer.Start();
            }
            key_manager.Start();
        }
    }
}
