using System;
using System.Collections.Generic;
using System.Linq;

namespace Garden
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] size = Console.ReadLine().Split().Select(int.Parse).ToArray();

            int[,] matrix = new int[size[0], size[1]];

            List<int> coordinates = new List<int>();

            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    matrix[row, col] = 0;
                }
            }

            string input = string.Empty;

            while ((input = Console.ReadLine()) != "Bloom Bloom Plow")
            {
                int[] position = input.Split().Select(int.Parse).ToArray();
                int row = position[0];
                int col = position[1];

                if (row >= 0 && row < matrix.GetLength(0) && col >= 0 && col < matrix.GetLength(1))
                {
                    coordinates.Add(row);
                    coordinates.Add(col);
                }
                else
                {
                    Console.WriteLine("Invalid coordinates.");
                }
            }

            for (int i = 0; i < coordinates.Count - 1; i += 2)
            {
                int row = coordinates[i];
                int col = coordinates[i + 1];

                for (int j = row; j < matrix.GetLength(0); j++)
                {
                    matrix[j, col] += 1;
                }
                for (int j = row - 1; j >= 0; j--)
                {
                    matrix[j, col] += 1;
                }
                for (int j = col + 1; j < matrix.GetLength(1); j++)
                {
                    matrix[row, j] += 1;
                }
                for (int j = col - 1; j >= 0; j--)
                {
                    matrix[row, j] += 1;
                }
            }

            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    Console.Write(matrix[row, col] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
