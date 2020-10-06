using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Threads
{
    class Program
    {
        static public void my_delegate() 
        {
            for (int i = 0; i < 10; i++) 
            {
                Console.SetCursorPosition(i, i);
                Console.WriteLine("Sas");
                Thread.Sleep(1100);
            }
        }
        static void Main(string[] args)
        {
            Thread my_thread = new Thread(my_delegate);
            Console.SetCursorPosition(15, 15);
            Console.Write("Waiting...");
            my_thread.Start();
            int n = 0;
            while (my_thread.IsAlive) 
            {
                Console.SetCursorPosition(16, 20);
                Console.Write(n.ToString());
                n++;
                Thread.Sleep(1000);
            }
        }
    }
}
