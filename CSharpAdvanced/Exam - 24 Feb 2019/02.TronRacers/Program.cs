using System;
using System.Data;

namespace TronRacers
{
    class Program
    {
        static void Main(string[] args)
        {
            int size = int.Parse(Console.ReadLine());

            char[,] matrix = new char[size, size];

            int firstPlayerRow = 0;
            int firstPlayerCol = 0;
            int secondPlayerRow = 0;
            int secondPlayerCol = 0;

            for (int row = 0; row < size; row++)
            {
                string data = Console.ReadLine();

                for (int col = 0; col < size; col++)
                {
                    if (data[col] == 'f')
                    {
                        firstPlayerRow = row;
                        firstPlayerCol = col;
                    }
                    if (data[col] == 's')
                    {
                        secondPlayerRow = row;
                        secondPlayerCol = col;
                    }
                    matrix[row, col] = data[col];
                }
            }

            int firstPlayerMoveRow = firstPlayerRow;
            int firstPlayerMoveCol = firstPlayerCol;
            int secondPlayerMoveRow = secondPlayerRow;
            int secondPlayerMoveCol = secondPlayerCol;

            while (true)
            {
                string[] command = Console.ReadLine().Split();

                firstPlayerMoveRow = MoveRow(command[0], firstPlayerMoveRow);
                firstPlayerMoveCol = MoveCol(command[0], firstPlayerMoveCol);
                secondPlayerMoveRow = MoveRow(command[1], secondPlayerMoveRow);
                secondPlayerMoveCol = MoveCol(command[1], secondPlayerMoveCol);

                if (firstPlayerMoveRow < 0 || firstPlayerMoveRow >= matrix.GetLength(0) ||
                    firstPlayerMoveCol < 0 || firstPlayerMoveCol >= matrix.GetLength(1))
                {
                    if (firstPlayerMoveRow < 0)
                    {
                        firstPlayerMoveRow = size - 1;
                    }
                    if (firstPlayerMoveCol < 0)
                    {
                        firstPlayerMoveCol = size - 1;
                    }
                    if (firstPlayerMoveRow >= size)
                    {
                        firstPlayerMoveRow = 0;
                    }
                    if (firstPlayerMoveCol >= size)
                    {
                        firstPlayerMoveCol = 0;
                    }
                }
                if (matrix[firstPlayerMoveRow, firstPlayerMoveCol] == 's')
                {
                    matrix[firstPlayerMoveRow, firstPlayerMoveCol] = 'x';
                    matrix[firstPlayerRow, firstPlayerCol] = 'f';

                    break;
                }
                else
                {
                    matrix[firstPlayerMoveRow, firstPlayerMoveCol] = 'f';
                }

                if (secondPlayerMoveRow < 0 || secondPlayerMoveRow >= matrix.GetLength(0) ||
                    secondPlayerMoveCol < 0 || secondPlayerMoveCol >= matrix.GetLength(1))
                {
                    if (secondPlayerMoveRow < 0)
                    {
                        secondPlayerMoveRow = size - 1;
                    }
                    if (secondPlayerMoveCol < 0)
                    {
                        secondPlayerMoveCol = size - 1;
                    }
                    if (secondPlayerMoveRow >= size)
                    {
                        secondPlayerMoveRow = 0;
                    }
                    if (secondPlayerMoveCol >= size)
                    {
                        secondPlayerMoveCol = 0;
                    }
                }
                if (matrix[secondPlayerMoveRow, secondPlayerMoveCol] == 'f')
                {
                    matrix[secondPlayerMoveRow, secondPlayerMoveCol] = 'x';
                    matrix[secondPlayerRow, secondPlayerCol] = 's';

                    break;
                }
                else
                {
                    matrix[secondPlayerMoveRow, secondPlayerMoveCol] = 's';
                }

                firstPlayerRow = firstPlayerMoveRow;
                firstPlayerCol = firstPlayerMoveCol;
                secondPlayerRow = secondPlayerMoveRow;
                secondPlayerCol = secondPlayerMoveCol;
            }

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    Console.Write(matrix[row, col]);
                }
                Console.WriteLine();
            }
        }


        public static int MoveRow(string command, int row)
        {
            if (command == "up")
            {
                row--;
            }
            else if (command == "down")
            {
                row++;
            }

            return row;
        }

        public static int MoveCol(string command, int col)
        {
            if (command == "left")
            {
                col--;
            }
            else if (command == "right")
            {
                col++;
            }

            return col;
        }
    }
}
