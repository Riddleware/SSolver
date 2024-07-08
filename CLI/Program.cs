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
            // while (true)
            {
                Console.WriteLine("Input grid line by line:");

                List<int> ns = new();

                // for (int i = 0; i < 9; i++)
                //{
                //    var line = Console.ReadLine();
                //    ns.AddRange(line.ToCharArray().ToList().Select(c => int.Parse(c.ToString())));
               // }

                var sample = @"900030800
                                000000104
                                087560000
                                060002400
                                090000030
                                004600010
                                000049350
                                709000000
                                001080002";
                
                ns = sample
                    .Where(s => s >= '0' && s <= '9')
                    .Select(c => int.Parse(c.ToString())).ToList();

                Grid g = new(ns, 00);
                g.Solve();

                Console.WriteLine("Solved!");
            }

            // Console.ReadKey();
            // return;
        }
    }
}