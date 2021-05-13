using System;
using System.Collections.Generic;
using System.Linq;

namespace DatingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Stack<int> males = new Stack<int>(Console.ReadLine().Split().Select(int.Parse));
            Queue<int> females = new Queue<int>(Console.ReadLine().Split().Select(int.Parse));

            int matchesCount = 0;

            while (males.Count > 0 && females.Count > 0)
            {
                int m = males.Peek();
                int f = females.Peek();

                if (m <= 0)
                {
                    males.Pop();
                    continue;
                }
                if (f <= 0)
                {
                    females.Dequeue();
                    continue;
                }

                if (m % 25 == 0)
                {
                    males.Pop();
                    males.Pop();
                    continue;
                }
                if (f % 25 == 0)
                {
                    females.Dequeue();
                    females.Dequeue();
                    continue;
                }

                if (m == f)
                {
                    matchesCount++;
                    males.Pop();
                    females.Dequeue();
                }
                else
                {
                    females.Dequeue();
                    males.Push(males.Pop() - 2);
                }
            }

            Console.WriteLine($"Matches: {matchesCount}");

            if (males.Count > 0)
            {
                Console.WriteLine($"Males left: {string.Join(", ", males)}");
            }
            else
            {
                Console.WriteLine("Males left: none");
            }

            if (females.Count > 0)
            {
                Console.WriteLine($"Females left: {string.Join(", ", females)}");
            }
            else
            {
                Console.WriteLine("Females left: none");
            }
        }
    }
}
