using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Threads
{
    class Program
    {
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
            public void process_key() 
            {
                while (true) 
                {
                    if (Console.KeyAvailable) 
                    {
                        if (Console.ReadKey(true).Key == stop_key) 
                        {
                            if (is_moving)
                                is_moving = false;
                            else
                                is_moving = true;
                        }
                    }
                }
            }
            public void sail()
            {
                int old_X = X;
                while (true) 
                {
                    if (is_moving)
                    {
                        Console.SetCursorPosition(old_X, Y);
                        Console.Write("     ");
                        old_X = X;
                        Console.SetCursorPosition(X, Y);
                        if (delta_x == 1)
                            Console.Write(texture_front);
                        else
                            Console.Write(texture_back);

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
        static void Main(string[] args)
        {
            Riverman data_for_uno = new Riverman("\\_R_/", "\\_Я_/", 100, 10, 1, Console.WindowWidth-2, ConsoleKey.D1);
            Thread sailer = new Thread(data_for_uno.sail);
            Thread controller = new Thread(data_for_uno.process_key);
            controller.Start();
            sailer.Start();
        }
    }
}
