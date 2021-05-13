using System;
using System.Collections.Generic;
using System.Linq;

namespace ClubParty
{
    class Program
    {
        static void Main(string[] args)
        {
            int hallCapacity = int.Parse(Console.ReadLine());
            string[] input = Console.ReadLine().Split();

            Stack<string> stack = new Stack<string>(input);
            Queue<string> halls = new Queue<string>();
            List<int> list = new List<int>();

            int currentCapacity = 0;

            while (stack.Count > 0)
            {
                if (!int.TryParse(stack.Peek(), out int result))
                {
                    halls.Enqueue(stack.Pop());
                }
                else if (halls.Count > 0)
                {
                    if (currentCapacity + int.Parse(stack.Peek()) <= hallCapacity)
                    {
                        currentCapacity += int.Parse(stack.Peek());
                        list.Add(int.Parse(stack.Pop()));
                    }
                    else
                    {
                        Console.WriteLine($"{halls.Dequeue()} -> {string.Join(", ", list)}");
                        currentCapacity = 0;
                        list.Clear();
                    }
                }
                else
                {
                    stack.Pop();
                }
            }
        }
    }
}
