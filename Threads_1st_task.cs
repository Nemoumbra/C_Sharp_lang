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
        static public void my_delegate() 
        {
            while (true) 
             {
                 if (Thread.CurrentThread.Name == "0") //*
                 {
                     lock (lock1)
                     {
                         Console.SetCursorPosition(0, 0);
                         Console.Write("*");
                     }
                     Thread.Sleep(1000);
                     lock (lock1) 
                     {
                         Console.SetCursorPosition(0, 0);
                         Console.Write(" ");
                     }
                     Thread.Sleep(1000);
                 }
                 else if (Thread.CurrentThread.Name == "1") //@
                 {
                     lock (lock1)
                     {
                         Console.SetCursorPosition(5, 0);
                         Console.Write("@");
                     }
                     Thread.Sleep(700);
                     lock (lock1)
                     {
                         Console.SetCursorPosition(5, 0);
                         Console.Write(" ");
                     }
                     Thread.Sleep(700);
                 }
                 else if (Thread.CurrentThread.Name == "2") //#
                 {
                     lock (lock1)
                     {
                         Console.SetCursorPosition(0, 5);
                         Console.Write("#");
                     }
                     Thread.Sleep(2000);
                     lock (lock1)
                     {
                         Console.SetCursorPosition(0, 5);
                         Console.Write(" ");
                     }
                     Thread.Sleep(2000);
                 }
                 else //$
                 {
                     lock (lock1)
                     {
                         Console.SetCursorPosition(5, 5);
                         Console.Write("$");
                     }
                     Thread.Sleep(1500);
                     lock (lock1)
                     {
                         Console.SetCursorPosition(5, 5);
                         Console.Write(" ");
                     }
                     Thread.Sleep(1500);
                 }
             }
        }
        static void Main(string[] args)
        {
            for (int i = 0; i < 4; i++)
            {
                Thread one_of_four = new Thread(my_delegate);
                one_of_four.Name = i.ToString();
                one_of_four.Start();
            }
        }
    }
}
