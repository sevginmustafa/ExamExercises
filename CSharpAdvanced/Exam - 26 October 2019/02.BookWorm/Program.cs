using System;

namespace BookWorm
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = Console.ReadLine();
            int size = int.Parse(Console.ReadLine());

            char[,] matrix = new char[size, size];

            int playerRow = 0;
            int playerCol = 0;

            for (int row = 0; row < size; row++)
            {
                string data = Console.ReadLine();

                for (int col = 0; col < size; col++)
                {
                    if (data[col] == 'P')
                    {
                        playerRow = row;
                        playerCol = col;
                    }
                    matrix[row, col] = data[col];
                }
            }

            int moveRow = playerRow;
            int moveCol = playerCol;

            string command = string.Empty;

            while ((command = Console.ReadLine()) != "end")
            {
                if (command == "up")
                {
                    moveRow--;
                }
                else if (command == "down")
                {
                    moveRow++;
                }
                else if (command == "left")
                {
                    moveCol--;
                }
                else
                {
                    moveCol++;
                }

                if (moveRow < 0 || moveRow >= matrix.GetLength(0) ||
                    moveCol < 0 || moveCol >= matrix.GetLength(1))
                {
                    input = input.Remove(input.Length - 1);
                    moveRow = playerRow;
                    moveCol = playerCol;
                }
                else if (char.IsLetter(matrix[moveRow, moveCol]))
                {
                    input += matrix[moveRow, moveCol];
                    matrix[playerRow, playerCol] = '-';
                    matrix[moveRow, moveCol] = 'P';
                }
                else
                {
                    matrix[playerRow, playerCol] = '-';
                    matrix[moveRow, moveCol] = 'P';
                }

                playerRow = moveRow;
                playerCol = moveCol;
            }

            Console.WriteLine(input);

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    Console.Write(matrix[row, col]);
                }
                Console.WriteLine();
            }
        }
    }
}
