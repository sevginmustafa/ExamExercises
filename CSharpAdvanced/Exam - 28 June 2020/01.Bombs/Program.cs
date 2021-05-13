using System;
using System.Collections.Generic;
using System.Linq;

namespace Bombs
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] arrayOne = Console.ReadLine().Split(", ").Select(int.Parse).ToArray();
            int[] arrayTwo = Console.ReadLine().Split(", ").Select(int.Parse).ToArray();

            int daturaBombs = 0;
            int cherryBombs = 0;
            int smokeDecoyBombs = 0;

            Queue<int> bombEffects = new Queue<int>(arrayOne);
            Stack<int> bombCasings = new Stack<int>(arrayTwo);

            while (bombEffects.Count > 0 && bombCasings.Count > 0)
            {
                if (daturaBombs >= 3 && cherryBombs >= 3 && smokeDecoyBombs >= 3)
                {
                    break;
                }

                int sum = bombEffects.Peek() + bombCasings.Peek();

                if (sum == 40)
                {
                    daturaBombs++;
                }
                else if (sum == 60)
                {
                    cherryBombs++;
                }
                else if (sum == 120)
                {
                    smokeDecoyBombs++;
                }
                else
                {
                    bombCasings.Push(bombCasings.Pop() - 5);
                    continue;
                }

                bombEffects.Dequeue();
                bombCasings.Pop();
            }

            if (daturaBombs >= 3 && cherryBombs >= 3 && smokeDecoyBombs >= 3)
            {
                Console.WriteLine("Bene! You have successfully filled the bomb pouch!");
            }
            else
            {
                Console.WriteLine("You don't have enough materials to fill the bomb pouch.");
            }

            if (bombEffects.Count > 0)
            {
                Console.WriteLine($"Bomb Effects: {string.Join(", ", bombEffects)}");
            }
            else
            {
                Console.WriteLine("Bomb Effects: empty");
            }

            if (bombCasings.Count > 0)
            {
                Console.WriteLine($"Bomb Casings: {string.Join(", ", bombCasings)}");
            }
            else
            {
                Console.WriteLine("Bomb Casings: empty");
            }

            Console.WriteLine($"Cherry Bombs: {cherryBombs}");
            Console.WriteLine($"Datura Bombs: {daturaBombs}");
            Console.WriteLine($"Smoke Decoy Bombs: {smokeDecoyBombs}");
        }
    }
}
