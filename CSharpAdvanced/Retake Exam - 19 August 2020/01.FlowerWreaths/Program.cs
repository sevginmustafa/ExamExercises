using System;
using System.Collections.Generic;
using System.Linq;

namespace _01.FlowerWreaths
{
    class Program
    {
        static void Main(string[] args)
        {
            var array1 = Console.ReadLine().Split(", ").Select(int.Parse).ToArray();
            var array2 = Console.ReadLine().Split(", ").Select(int.Parse).ToArray();

            Stack<int> lilies = new Stack<int>(array1);
            Queue<int> roses = new Queue<int>(array2);

            int wreathsCount = 0;
            int storedFlowers = 0;

            while (roses.Count > 0 && lilies.Count > 0)
            {
                int sum = roses.Peek() + lilies.Peek();

                if (sum == 15)
                {
                    roses.Dequeue();
                    lilies.Pop();
                    wreathsCount++;
                }
                else if (sum > 15)
                {
                    lilies.Push(lilies.Pop() - 2);
                }
                else
                {
                    storedFlowers += roses.Dequeue() + lilies.Pop();
                }
            }

            wreathsCount += storedFlowers / 15;

            if (wreathsCount < 5)
            {
                Console.WriteLine($"You didn't make it, you need {5 - wreathsCount} wreaths more!");
            }
            else
            {
                Console.WriteLine($"You made it, you are going to the competition with {wreathsCount} wreaths!");
            }
        }
    }
}
