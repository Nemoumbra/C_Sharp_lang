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
        class Twinkler
        {
            public int X, Y, sleep_time;
            public char character;
            public Twinkler(int Xcoord, int Ycoord, char symbol, int time) 
            {
                X = Xcoord;
                Y = Ycoord;
                character = symbol;
                sleep_time = time;
            }
            public void twinkle() 
            {
                while (true) 
                {
                    lock (lock1)
                    {
                        Console.SetCursorPosition(X, Y);
                        Console.Write(character);
                    }
                    Thread.Sleep(sleep_time);
                    lock (lock1)
                    {
                        Console.SetCursorPosition(X, Y);
                        Console.Write(" ");
                    }
                    Thread.Sleep(sleep_time);
                }
            }
        }
        static void Main(string[] args)
        {
            Twinkler data_asterisk = new Twinkler(0, 0, '*', 1000);
            Twinkler data_sharp = new Twinkler(0, 5, '#', 2000);
            Twinkler data_at = new Twinkler(5, 0, '@', 700);
            Twinkler data_dollar = new Twinkler(5, 5, '$', 1500);
            Thread asterisk = new Thread(data_asterisk.twinkle);
            Thread sharp = new Thread(data_sharp.twinkle);
            Thread at = new Thread(data_at.twinkle);
            Thread dollar = new Thread(data_dollar.twinkle);
            asterisk.Start();
            sharp.Start();
            at.Start();
            dollar.Start();
        }
    }
}
