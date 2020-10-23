using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Threads
{
    class Program
    {
        static object lock1 = new object();
        class Spinner
        {
            public int X, Y, frequency;
            public string frames;
            public char current_frame;
            public Spinner(int Xcoord, int Ycoord, string animation, int delta_time)
            {
                X = Xcoord; Y = Ycoord;
                frames = animation;
                frequency = delta_time;
            }
            public void spin()
            {
                int counter = 0;
                while (true)
                {
                    current_frame = frames[counter];
                    lock (lock1)
                    {
                        Console.SetCursorPosition(X, Y);
                        Console.Write(current_frame);
                    }
                    counter = (counter + 1) % frames.Length;
                    Thread.Sleep(frequency);
                }
            }
        static public void wheel()
        {
            int k = Convert.ToInt32(Thread.CurrentThread.Name) / 4;
            int X = Console.CursorLeft; //Later in the code X points at the position the symbol was put to
            int Y = Console.CursorTop;
            bool first = true;
            while (true)
            {
                if (X == Console.WindowWidth - 1) //Once the end of the screen is reached, X++ will ruin the SetCursorPosition(X, Y)
                {
                    Console.SetCursorPosition(X, Y);
                    Console.Write(" ");
                    X = 0;
                    first = true;
                }
                lock (lock1)
                {
                    Console.SetCursorPosition(X, Y);
                    if (first)
                    {
                        Console.Write("\\");
                        first = false;
                    }
                    else
                    {
                        Console.Write(" \\");
                        X++;
                    }
                }
                if (X == Console.WindowWidth - 1) //Once the end of the screen is reached, X++ will ruin the SetCursorPosition(X, Y)
                {
                    Console.SetCursorPosition(X, Y);
                    Console.Write(" ");
                    X = 0;
                    first = true;
                }
                Thread.Sleep(k);
                lock (lock1)
                {
                    Console.SetCursorPosition(X, Y);
                    if (first)
                    {
                        Console.Write("|");
                        first = false;
                    }
                    else
                    {
                        Console.Write(" |");
                        X++;
                    }
                }
                Thread.Sleep(k);
                if (X == Console.WindowWidth - 1) //Once the end of the screen is reached, X++ will ruin the SetCursorPosition(X, Y)
                {
                    Console.SetCursorPosition(X, Y);
                    Console.Write(" ");
                    X = 0;
                    first = true;
                }
                lock (lock1)
                {
                    Console.SetCursorPosition(X, Y);
                    if (first)
                    {
                        Console.Write("/");
                        first = false;
                    }
                    else
                    {
                        Console.Write(" /");
                        X++;
                    }
                }
                Thread.Sleep(k);
                if (X == Console.WindowWidth - 1) //Once the end of the screen is reached, X++ will ruin the SetCursorPosition(X, Y)
                {
                    Console.SetCursorPosition(X, Y);
                    Console.Write(" ");
                    X = 0;
                    first = true;
                }
                lock (lock1)
                {
                    Console.SetCursorPosition(X, Y);
                    if (first)
                    {
                        Console.Write("-");
                        first = false;
                    }
                    else
                    {
                        Console.Write(" -");
                        X++;
                    }
                }
                Thread.Sleep(k);
            }
        }
        static void Main(string[] args)
        {
            /*Spinner data_windwill_1 = new Spinner(3, 10, "\\|/-", 250);
            Thread windwill_1 = new Thread(data_windwill_1.spin);
            windwill_1.Start();
            Spinner data_weird_windmill = new Spinner(20, 20, "\\|/-/|\\-", 200);
            Thread weird_windmill = new Thread(data_weird_windmill.spin);
            weird_windmill.Start();*/
            Console.SetWindowSize(Console.WindowWidth + 1 - 4*7, Console.WindowHeight);
            Thread tumbleweed = new Thread(wheel);
            tumbleweed.Name = "800";
            tumbleweed.Start();
        }
    }
}
