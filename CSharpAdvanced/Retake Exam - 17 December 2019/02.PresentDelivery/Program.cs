using System;

namespace PresentDelivery
{
    class Program
    {
        static void Main(string[] args)
        {
            int presentsCount = int.Parse(Console.ReadLine());
            int size = int.Parse(Console.ReadLine());

            char[,] matrix = new char[size, size];

            int santaRow = 0;
            int santaCol = 0;
            int niceKids = 0;

            for (int row = 0; row < size; row++)
            {
                string[] input = Console.ReadLine().Split();
                string data = string.Concat(input);

                for (int col = 0; col < size; col++)
                {
                    if (data[col] == 'V')
                    {
                        niceKids++;
                    }
                    if (data[col] == 'S')
                    {
                        santaRow = row;
                        santaCol = col;
                    }
                    if (data[col] != ' ')
                    {
                        matrix[row, col] = data[col];
                    }
                }
            }

            string command = string.Empty;

            int movingRow = santaRow;
            int movingCol = santaCol;

            while ((command = Console.ReadLine()) != "Christmas morning")
            {
                if (command == "up")
                {
                    movingRow--;
                }
                else if (command == "down")
                {
                    movingRow++;
                }
                else if (command == "left")
                {
                    movingCol--;
                }
                else if (command == "right")
                {
                    movingCol++;
                }

                if (matrix[movingRow, movingCol] == 'V')
                {
                    matrix[movingRow, movingCol] = 'S';
                    matrix[santaRow, santaCol] = '-';
                    presentsCount--;
                }
                else if (matrix[movingRow, movingCol] == 'X')
                {
                    matrix[movingRow, movingCol] = 'S';
                    matrix[santaRow, santaCol] = '-';
                }
                else if (matrix[movingRow, movingCol] == 'C')
                {
                    matrix[movingRow, movingCol] = 'S';
                    matrix[santaRow, santaCol] = '-';

                    if (matrix[movingRow - 1, movingCol] != '-' && presentsCount > 0)
                    {
                        matrix[movingRow - 1, movingCol] = '-';
                        presentsCount--;
                    }
                    if (matrix[movingRow + 1, movingCol] != '-' && presentsCount > 0)
                    {
                        matrix[movingRow + 1, movingCol] = '-';
                        presentsCount--;
                    }
                    if (matrix[movingRow, movingCol - 1] != '-' && presentsCount > 0)
                    {
                        matrix[movingRow, movingCol - 1] = '-';
                        presentsCount--;
                    }
                    if (matrix[movingRow, movingCol + 1] != '-' && presentsCount > 0)
                    {
                        matrix[movingRow, movingCol + 1] = '-';
                        presentsCount--;
                    }
                }
                else
                {
                    matrix[movingRow, movingCol] = 'S';
                    matrix[santaRow, santaCol] = '-';
                }

                if (presentsCount == 0)
                {
                    Console.WriteLine("Santa ran out of presents!");
                    break;
                }

                santaRow = movingRow;
                santaCol = movingCol;
            }

            int niceKidsLeft = 0;

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    if (matrix[row, col] == 'V')
                    {
                        niceKidsLeft++;
                    }
                    Console.Write(matrix[row, col] + " ");
                }
                Console.WriteLine();
            }

            if (niceKidsLeft == 0)
            {
                Console.WriteLine($"Good job, Santa! {niceKids} happy nice kid/s.");
            }
            else
            {
                Console.WriteLine($"No presents for {niceKidsLeft} nice kid/s.");
            }
        }
    }
}
