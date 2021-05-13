using System;

namespace _02.Bee
{
    class Program
    {
        static void Main(string[] args)
        {
            int size = int.Parse(Console.ReadLine());

            char[,] beeTerritory = new char[size, size];

            int beeStartRow = 0;
            int beeStartCol = 0;

            for (int row = 0; row < beeTerritory.GetLength(0); row++)
            {
                string data = Console.ReadLine();

                for (int col = 0; col < beeTerritory.GetLength(1); col++)
                {
                    beeTerritory[row, col] = data[col];

                    if (data[col] == 'B')
                    {
                        beeStartRow = row;
                        beeStartCol = col;
                    }
                }
            }

            string command = Console.ReadLine();

            int beeMoveRow = beeStartRow;
            int beeMoveCol = beeStartCol;
            int pollinatedFlowers = 0;

            while (command != "End")
            {
                if (command == "up")
                {
                    beeMoveRow -= 1;
                }
                else if (command == "down")
                {
                    beeMoveRow += 1;
                }
                else if (command == "left")
                {
                    beeMoveCol -= 1;
                }
                else if (command == "right")
                {
                    beeMoveCol += 1;
                }

                if (beeMoveRow >= 0 && beeMoveRow < beeTerritory.GetLength(0) && beeMoveCol >= 0 && beeMoveCol < beeTerritory.GetLength(1))
                {
                    if (beeTerritory[beeMoveRow, beeMoveCol] == 'f')
                    {
                        beeTerritory[beeMoveRow, beeMoveCol] = 'B';
                        beeTerritory[beeStartRow, beeStartCol] = '.';
                        pollinatedFlowers++;
                    }
                    else if (beeTerritory[beeMoveRow, beeMoveCol] == 'O')
                    {
                        beeTerritory[beeMoveRow, beeMoveCol] = '.';
                        continue;
                    }
                    else
                    {
                        beeTerritory[beeMoveRow, beeMoveCol] = 'B';
                        beeTerritory[beeStartRow, beeStartCol] = '.';
                    }
                }
                else
                {
                    beeTerritory[beeStartRow, beeStartCol] = '.';

                    Console.WriteLine("The bee got lost!");
                    break;
                }

                beeStartRow = beeMoveRow;
                beeStartCol = beeMoveCol;

                command = Console.ReadLine();
            }

            if (pollinatedFlowers < 5)
            {
                Console.WriteLine($"The bee couldn't pollinate the flowers, she needed {5 - pollinatedFlowers} flowers more");
            }
            else
            {
                Console.WriteLine($"Great job, the bee managed to pollinate {pollinatedFlowers} flowers!");
            }

            for (int row = 0; row < beeTerritory.GetLength(0); row++)
            {
                for (int col = 0; col < beeTerritory.GetLength(1); col++)
                {
                    Console.Write(beeTerritory[row, col]);
                }
                Console.WriteLine();
            }
        }
    }
}
