using System;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace Re_Volt
{
    class Program
    {
        static void Main(string[] args)
        {
            int size = int.Parse(Console.ReadLine());
            int commandsCount = int.Parse(Console.ReadLine());

            char[,] matrix = new char[size, size];

            int playerRow = 0;
            int playerCol = 0;

            for (int row = 0; row < size; row++)
            {
                string data = Console.ReadLine();

                for (int col = 0; col < size; col++)
                {
                    if (data[col] == 'f')
                    {
                        playerRow = row;
                        playerCol = col;
                    }
                    matrix[row, col] = data[col];
                }
            }

            int moveRow = playerRow;
            int moveCol = playerCol;

            for (int i = 0; i < commandsCount; i++)
            {
                string command = Console.ReadLine();

                moveRow = CheckRows(matrix, command, moveRow);
                moveCol = CheckCols(matrix, command, moveCol);

                if (matrix[moveRow, moveCol] == 'B')
                {
                    moveRow = CheckRows(matrix, command, moveRow);
                    moveCol = CheckCols(matrix, command, moveCol);
                }
                else if (matrix[moveRow, moveCol] == 'T')
                {
                    moveRow = playerRow;
                    moveCol = playerCol;

                    continue;
                }

                if (matrix[moveRow, moveCol] == 'F')
                {
                    matrix[playerRow, playerCol] = '-';
                    matrix[moveRow, moveCol] = 'f';

                    Console.WriteLine("Player won!");
                    PrintMatrix(matrix);
                    return;
                }

                matrix[playerRow, playerCol] = '-';
                matrix[moveRow, moveCol] = 'f';

                playerRow = moveRow;
                playerCol = moveCol;
            }

            Console.WriteLine("Player lost!");
            PrintMatrix(matrix);
        }

        static int CheckRows(char[,] matrix, string command, int moveRow)
        {
            if (command == "up")
            {
                moveRow--;
            }
            else if (command == "down")
            {
                moveRow++;
            }

            if (moveRow < 0)
            {
                moveRow = matrix.GetLength(0) - 1;
            }
            else if (moveRow >= matrix.GetLength(0))
            {
                moveRow = 0;
            }

            return moveRow;
        }

        static int CheckCols(char[,] matrix, string command, int moveCol)
        {
            if (command == "left")
            {
                moveCol--;
            }
            else if (command == "right")
            {
                moveCol++;
            }

            if (moveCol < 0)
            {
                moveCol = matrix.GetLength(1) - 1;
            }
            else if (moveCol >= matrix.GetLength(1))
            {
                moveCol = 0;
            }

            return moveCol;
        }

        static void PrintMatrix(char[,] matrix)
        {
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    Console.Write(matrix[row, col]);
                }
                Console.WriteLine();
            }
        }
    }
}
