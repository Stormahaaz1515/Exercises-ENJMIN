using System;
using System.Collections.Generic;
using System.IO;

namespace SelectionCandidat
{
    class Program
    {
        static void Main(string[] args)
        {
            string textFile = @"C:\Users\valgo\OneDrive\Bureau\C#\CodingRoom\Partie2\Partie2\data-test3.txt";
            var data = new List<String>();
            string line;
            if (File.Exists(textFile))
            {
                // Read file using StreamReader. Reads file line by line  
                using (StreamReader file = new StreamReader(textFile))
                {
                    while ((line = file.ReadLine()) != null)
                        data.Add(line);
                    file.Close();
                }
            }
             /* Get data from input file */

            // ADD YOUR CODE HERE
            int square = int.Parse(data[0]);

            int[][] mapSquare = new int[square][];
            for (int k = 0; k < square; k++)
                mapSquare[k] = new int[square];

            int goldCoins = int.Parse(data[1]);
            int goldCoinsRemains = goldCoins;
            string[] playerPosition = data[2 + goldCoins].Split(',');
            int playerPositionX = int.Parse(playerPosition[0]);
            int playerPositionY = int.Parse(playerPosition[1]);
            //Setting up all the coins on their place in the map.
            int goldCoinsPositionX;
            int goldCoinsPositionY;
            string[] goldsCoinsPosition;

            for (int i = 0; i < goldCoinsRemains; i++)
            {
                goldsCoinsPosition = data[i + 2].Split(',');
                goldCoinsPositionX = int.Parse(goldsCoinsPosition[0]);
                goldCoinsPositionY = int.Parse(goldsCoinsPosition[1]);
                mapSquare[(square - 1) - goldCoinsPositionX][goldCoinsPositionY] = 2;
            }
            // Parsing of the move instructions.
            for (int k = 0; k < int.Parse(data[3 + goldCoins]); k++)
            {
                MapBuild(square, ref mapSquare, playerPositionX, playerPositionY, ref goldCoinsRemains);
               // DisplayMap(square, mapSquare); // Uncomment this line to display the map on the console.
                if (char.Parse(data[4 + goldCoins + k]) == 'u')
                    playerPositionY += 1;
                else if (char.Parse(data[4 + goldCoins + k]) == 'l')
                    playerPositionX -= 1;
                else if (char.Parse(data[4 + goldCoins + k]) == 'r')
                    playerPositionX += 1;
                else if (char.Parse(data[4 + goldCoins + k]) == 'd')
                    playerPositionY -= 1;
                if (playerPositionX >= square || playerPositionX < 0 || playerPositionY >= square || playerPositionY < 0)
                {
                    Console.WriteLine("out");
                    return;
                }
            }
            Console.Write(goldCoins - goldCoinsRemains);
        }

        //Method to set the position of the player on the map.
        private static void MapBuild(int square, ref int[][] mapSquare, int playerPositionX, int playerPositionY, ref int goldCoinsRemains)
        {
            for (int i = 0; i <= square - 1; i++)
            {
                for (int j = 0; j <= square - 1; j++)
                {
                    if (square - 1 - playerPositionX == i && playerPositionY == j)
                    {
                        if (mapSquare[i][j] == 2)
                            goldCoinsRemains--;
                        mapSquare[i][j] = 1;
                    }
                    else if (mapSquare[i][j] == 2)
                        continue;
                    else
                        mapSquare[i][j] = 0;
                }
            }
        }

        // Method to display the map with a zero at an empty case, one at the player location, and a two as the location of a coin.
        private static void DisplayMap(int square, int[][] mapSquare)
        {
            for (int y = square - 1; y >= 0; y--)
            {
                for (int x = 0; x <= square - 1; x++)
                {
                    Console.Write($"{mapSquare[square - 1 - x][y]} ");
                }
                Console.Write('\n');

            }
            Console.WriteLine("--------------");
        }
    }
}
