using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_sharp_sandbox
{
    class Program
    {
        static int digits(int N)
        {
            int counter = 0;
            while (N > 0) {
                counter++;
                N /= 10;
            }
            return counter;
        }
        static void Main(string[] args)
        {
            bool end = false;
            int x = 1;
            int n = 0;
            while (!end) {
                n = 10 * (x % Convert.ToInt32(Math.Pow(10, digits(x) - 1))) + x / Convert.ToInt32(Math.Pow(10, digits(x) - 1));
                if (3 * x == 2 * n)
                {
                    end = true;
                }
                else {
                    x++;
                }
            }
            Console.WriteLine(x.ToString());
        }
    }
}
