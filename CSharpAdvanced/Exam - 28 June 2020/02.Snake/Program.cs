using System;
using System.Runtime.CompilerServices;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            int size = int.Parse(Console.ReadLine());

            char[,] matrix = new char[size, size];

            int snakeRow = 0;
            int snakeCol = 0;

            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                string data = Console.ReadLine();

                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    if (data[col] == 'S')
                    {
                        snakeRow = row;
                        snakeCol = col;
                    }
                    matrix[row, col] = data[col];
                }
            }

            int foodEaten = 0;
            int moveRow = snakeRow;
            int moveCol = snakeCol;

            while (foodEaten < 10)
            {
                string command = Console.ReadLine();

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
                else if (command == "right")
                {
                    moveCol++;
                }

                if (moveRow >= 0 && moveRow < matrix.GetLength(0) && moveCol >= 0 && moveCol < matrix.GetLength(1))
                {
                    if (matrix[moveRow, moveCol] == '*')
                    {
                        foodEaten++;
                        matrix[moveRow, moveCol] = 'S';
                        matrix[snakeRow, snakeCol] = '.';
                    }
                    else if (matrix[moveRow, moveCol] == 'B')
                    {
                        matrix[moveRow, moveCol] = '.';
                        matrix[snakeRow, snakeCol] = '.';

                        for (int row = 0; row < matrix.GetLength(0); row++)
                        {
                            for (int col = 0; col < matrix.GetLength(1); col++)
                            {
                                if (matrix[row, col] == 'B')
                                {
                                    moveRow = row;
                                    moveCol = col;
                                    matrix[row, col] = 'S';
                                    row = matrix.GetLength(0) - 1;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        matrix[moveRow, moveCol] = 'S';
                        matrix[snakeRow, snakeCol] = '.';
                    }
                }
                else
                {
                    matrix[snakeRow, snakeCol] = '.';
                    break;
                }

                snakeRow = moveRow;
                snakeCol = moveCol;
            }

            if (foodEaten >= 10)
            {
                Console.WriteLine("You won! You fed the snake.");
            }
            else
            {
                Console.WriteLine("Game over!");
            }

            Console.WriteLine($"Food eaten: {foodEaten}");

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
