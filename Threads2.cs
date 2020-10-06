using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Threads
{
    class Program
    {
        static int n = 0;
        static public void my_delegate() 
        {
            int i = 0;
            while (true) 
             {
                 Console.SetCursorPosition(10, 10 + Convert.ToInt32(Math.Pow(-1, n)));
                 n++;
                 Console.Write(i.ToString());
                 i++;
                 Thread.Sleep(1000);
             }
        }
        static void Main(string[] args)
        {
            Thread my_thread1 = new Thread(my_delegate);
            Thread my_thread2 = new Thread(my_delegate);
            my_thread1.Start();
            Thread.Sleep(500);
            my_thread2.Start();

        }
    }
}
