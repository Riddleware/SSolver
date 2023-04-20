using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace SSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Input grid line by line:");

                List<int> ns = new();

                for (int i = 0; i < 9; i++)
                {
                    var line = Console.ReadLine();
                    ns.AddRange(line.ToCharArray().ToList().Select(c => int.Parse(c.ToString())));
                }

                Grid g = new(ns, 300);
                g.Solve();

                Console.WriteLine("Solved!");
            }

            // Console.ReadKey();
            // return;
        }
    }
}