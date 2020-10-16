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
        static public void windmill() 
        {
            int k = Convert.ToInt32(Thread.CurrentThread.Name) / 4;
            int X = Console.CursorLeft;
            int Y = Console.CursorTop;
            while (true) 
             {
                lock (lock1)
                {
                     Console.SetCursorPosition(X, Y);
                     Console.Write("\\");
                }
                Thread.Sleep(k);
                lock (lock1)
                {
                    Console.SetCursorPosition(X, Y);
                    Console.Write("|");
                }
                Thread.Sleep(k);
                lock (lock1)
                {
                    Console.SetCursorPosition(X, Y);
                    Console.Write("/");
                }
                Thread.Sleep(k);
                lock (lock1)
                {
                    Console.SetCursorPosition(X, Y);
                    Console.Write("–");
                }
                Thread.Sleep(k);
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
            /*Thread windmill1 = new Thread(windmill);
            windmill1.Name = "1000";
            windmill1.Start();
            Console.SetCursorPosition(20, 30);
            Thread windmill2 = new Thread(windmill);
            windmill2.Name = "600";
            windmill2.Start();
            Thread windmill3 = new Thread(windmill);
            windmill3.Name = "400";
            Console.SetCursorPosition(15, 15);
            windmill3.Start();*/
            Console.SetWindowSize(Console.WindowWidth + 1 - 4*7, Console.WindowHeight);
            Thread tumbleweed = new Thread(wheel);
            tumbleweed.Name = "800";
            tumbleweed.Start();
        }
    }
}
