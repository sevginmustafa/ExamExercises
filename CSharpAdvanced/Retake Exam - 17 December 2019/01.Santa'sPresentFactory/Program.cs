using System;
using System.Collections.Generic;
using System.Linq;

namespace Santa_sPresentFactory
{
    class Program
    {
        static void Main(string[] args)
        {
            Stack<int> materials = new Stack<int>(Console.ReadLine().Split().Select(int.Parse));
            Queue<int> magicLevel = new Queue<int>(Console.ReadLine().Split().Select(int.Parse));

            int dolls = 0;
            int woodenTrains = 0;
            int teddyBears = 0;
            int bicycles = 0;

            while (materials.Count > 0 && magicLevel.Count > 0)
            {
                int multiply = materials.Peek() * magicLevel.Peek();

                if (multiply == 150)
                {
                    dolls++;
                    magicLevel.Dequeue();
                    materials.Pop();
                }
                else if (multiply == 250)
                {
                    woodenTrains++;
                    magicLevel.Dequeue();
                    materials.Pop();
                }
                else if (multiply == 300)
                {
                    teddyBears++;
                    magicLevel.Dequeue();
                    materials.Pop();
                }
                else if (multiply == 400)
                {
                    bicycles++;
                    magicLevel.Dequeue();
                    materials.Pop();
                }
                else if (multiply < 0)
                {
                    materials.Push(materials.Pop() + magicLevel.Dequeue());
                }
                else if (multiply > 0)
                {
                    magicLevel.Dequeue();
                    materials.Push(materials.Pop() + 15);
                }

                if (materials.Count > 0 && materials.Peek() == 0)
                {
                    materials.Pop();
                }
                if (magicLevel.Count > 0 && magicLevel.Peek() == 0)
                {
                    magicLevel.Dequeue();
                }
            }

            if ((dolls > 0 && woodenTrains > 0) || (teddyBears > 0 && bicycles > 0))
            {
                Console.WriteLine("The presents are crafted! Merry Christmas!");
            }
            else
            {
                Console.WriteLine("No presents this Christmas!");
            }

            if (materials.Count > 0)
            {
                Console.WriteLine($"Materials left: { string.Join(", ", materials)}");
            }
            if (magicLevel.Count > 0)
            {
                Console.WriteLine($"Magic left: { string.Join(", ", magicLevel)}");
            }

            if (bicycles > 0)
            {
                Console.WriteLine($"Bicycle: {bicycles}");
            }
            if (dolls > 0)
            {
                Console.WriteLine($"Doll: {dolls}");
            }
            if (teddyBears > 0)
            {
                Console.WriteLine($"Teddy bear: {teddyBears}");
            }
            if (woodenTrains > 0)
            {
                Console.WriteLine($"Wooden train: {woodenTrains}");
            }
        }
    }
}
