using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SelectionCandidat
{
	class Program
	{
		static void Main(string[] args)
		{
			/* Get data from input file */
			// Read file using StreamReader. Reads file line by line  
			string textFile = @"C:\Users\valgo\OneDrive\Bureau\C#\CodingRoom\Partie3\Partie3\data-test3.txt";
			var data = new List<String>();
			string line;
			if (File.Exists(textFile))
			{
				using (StreamReader file = new StreamReader(textFile))
				{
					while ((line = file.ReadLine()) != null)
						data.Add(line);
					file.Close();
				}
			}

			// ADD YOUR CODE HERE
			int square = int.Parse(data[0]);

			int[][] mapSquare = new int[square][];
			for (int k = 0; k < square; k++)
				mapSquare[k] = new int[square];

			int goldCoins = int.Parse(data[3]);
			int goldCoinsRemains = goldCoins;
			int instructionsRemains = int.Parse(data[2]);
			string[] playerPosition = data[1].Split(',');
			string instructions;
			int playerPositionX = int.Parse(playerPosition[0]);
			int playerPositionY = int.Parse(playerPosition[1]);
			// Setting up the needed variables to place every coin on the map
			int goldCoinsPositionX;
			int goldCoinsPositionY;
			string[] goldsCoinsPosition;
			// Each list following is gonna have for value a path to a direction in first string, then the location of each coin in it and a 1 at the end if their is a piece than can be reach with one move.
			List<String> north = new List<string>();
			List<String> east = new List<string>();
			List<String> south = new List<string>();
			List<String> west = new List<string>();
			List<String> northEast = new List<string>();
			List<String> southEast = new List<string>();
			List<String> southWest = new List<string>();
			List<String> northWest = new List<string>();

			// Setting up all the gold coins on the map.
			for (int i = 0; i < goldCoinsRemains; i++)
			{
				goldsCoinsPosition = data[i + 4].Split(',');
				goldCoinsPositionX = int.Parse(goldsCoinsPosition[0]);
				goldCoinsPositionY = int.Parse(goldsCoinsPosition[1]);
				mapSquare[(square - 1) - goldCoinsPositionX][goldCoinsPositionY] = 2;
			}
			// Parsing and applying the move instructions.
			MapBuild(square, ref mapSquare, playerPositionX, playerPositionY, ref goldCoinsRemains);
			// this while apply the move the hero have to make
			while (instructionsRemains-- > 0)
			{
				instructions = MagicCompass(playerPositionX, playerPositionY, square, mapSquare, ref north, ref east, ref south, ref west, ref northEast, ref southEast, ref southWest, ref northWest);
				if (instructions == "u")
					playerPositionY += 1;
				else if (instructions == "ur")
				{
					playerPositionX += 1;
					playerPositionY += 1;
				}
				else if (instructions == "ul")
				{
					playerPositionY += 1;
					playerPositionX -= 1;
				}
				else if (instructions == "l")
					playerPositionX -= 1;
				else if (instructions == "r")
					playerPositionX += 1;
				else if (instructions == "d")
					playerPositionY -= 1;
				else if (instructions == "dr")
				{
					playerPositionX += 1;
					playerPositionY -= 1;
				}
				else if (instructions == "dl")
				{
					playerPositionX -= 1;
					playerPositionY -= 1;
				}
				Console.WriteLine($"{playerPositionX},{playerPositionY}");
				MapBuild(square, ref mapSquare, playerPositionX, playerPositionY, ref goldCoinsRemains);
				// Uncomment the following line if you want to display the map and see the player move on it.
				//DisplayMap(square, mapSquare);
				if (goldCoinsRemains <= 0)
					break;
			}
			Console.Write(goldCoins - goldCoinsRemains);
		}

		// Method to set the map up to date.
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

		// Method to display the map with a 1 at the hero location, a 2 on every coin and a 0 for the empty place
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

		// Method that will determinate the better move to do
		private static string MagicCompass(int playerPositionX, int playerPositionY, int square, int[][] mapSquare, ref List<String> north, ref List<String> east, ref List<String> south, ref List<String> west, ref List<String> northEast, ref List<String> southEast, ref List<String> southWest, ref List<String> northWest)
		{
			List<List<string>> pathlist = new List<List<string>>(8);
			int coins = GetCloseCoins(mapSquare, playerPositionX, playerPositionY, square);

			pathlist.Add(north = GetNorthCoins(playerPositionX, playerPositionY, square, mapSquare));
			pathlist.Add(east = GetEastCoins(playerPositionX, playerPositionY, square, mapSquare));
			pathlist.Add(south = GetSouthCoins(playerPositionX, playerPositionY, square, mapSquare));
			pathlist.Add(west = GetWestCoins(playerPositionX, playerPositionY, square, mapSquare));
			pathlist.Add(northEast = GetNorthEastCoins(playerPositionX, playerPositionY, square, mapSquare));
			pathlist.Add(southEast = GetSouthEastCoins(playerPositionX, playerPositionY, square, mapSquare));
			pathlist.Add(southWest = GetSouthWestCoins(playerPositionX, playerPositionY, square, mapSquare));
			pathlist.Add(northWest = GetNorthWestCoins(playerPositionX, playerPositionY, square, mapSquare));

			// If there is only one piece close then i move on it without using the magic compass
			if (coins == 1)
			{
				foreach (List<string> path in pathlist.ToList())
				{
					if (!String.Equals(path[path.Count - 1], "1"))
						pathlist.Remove(path);
				}
				return pathlist[0][0];
			}
			// If there is more than one coin reachable in one move, then I remove all the other paths
			if (coins > 1)
				foreach (List<string> path in pathlist.ToList())
				{
					if (!String.Equals(path[path.Count - 1], "1"))
						pathlist.Remove(path);
				}
			while (pathlist.Count > 1)
			{
				if (pathlist[0].Count > pathlist[1].Count)
					pathlist.RemoveAt(1);
				else
					pathlist.RemoveAt(0);
			}
			// I sort the paths comparing and removing two path, the one with the lower number of coins in it is removed. 
			// I return the string containing the good path
			return pathlist[0][0];
		}

		// The following method will return the number of coins reachable by one move to the hero
		private static int GetCloseCoins(int[][] mapSquare, int playerPositionX, int playerPositionY, int square)
		{
			int coins = 0;
			int x = playerPositionX;
			int y = playerPositionY + 1;
			for (int i = 0; i < 8; i++)
			{
				if (x < square && x >= 0 && y < square && y >= 0)
				{
					if (mapSquare[square - 1 - x][y] == 2)
						coins++;
				}
				if (x < playerPositionX + 1 && y == playerPositionY + 1)
					x++;
				else if (x == playerPositionX + 1 && y > playerPositionY - 1)
					y--;
				else if (x > playerPositionX - 1 && y == playerPositionY - 1)
					x--;
				else if (y < playerPositionY  + 1 && x == playerPositionX - 1)
					y++;
				if (x == playerPositionX - 1 && y == playerPositionY + 1)
				{
					playerPositionX = x;
					playerPositionY = y;
				}
			}
			return coins;
		}
		// All the methods below build a list that contain the position of each coins in their direction, then add a one at the end if there is a coin reachable by one move.
		private static List<String> GetNorthCoins(int playerPositionX, int playerPositionY, int square, int[][] mapSquare)
		{
			List<String> Coins = new List<String>();
			playerPositionY += 1;
			Coins.Add("u");
			if (playerPositionY >= square || playerPositionX >= square || playerPositionX < 0 || playerPositionY < 0)
				return Coins;
			for (int y = square - 1; y >= playerPositionY; y--)
			{
				for (int x = 0; x <= square - 1; x++)
				{
					if (mapSquare[square - 1 - x][y] == 2)
						Coins.Add($"{x},{y}");
				}
			}
			if (mapSquare[square - 1 - playerPositionX][playerPositionY] == 2)
				Coins.Add("1");
			return (Coins);
		}
		private static List<String> GetEastCoins(int playerPositionX, int playerPositionY, int square, int[][] mapSquare)
		{
			List<String> Coins = new List<String>();
			playerPositionX += 1;
			Coins.Add("r");
			if (playerPositionY >= square || playerPositionX >= square || playerPositionX < 0 || playerPositionY < 0)
				return Coins;
			for (int y = square - 1; y >= 0; y--)
			{
				for (int x = playerPositionX; x <= square - 1; x++)
				{
					if (mapSquare[square - 1 - x][y] == 2)
						Coins.Add($"{x},{y}");
				}
			}
			if (mapSquare[square - 1 - playerPositionX][playerPositionY] == 2)
				Coins.Add("1");
			return (Coins);
		}
		
	private static List<String> GetSouthCoins(int playerPositionX, int playerPositionY, int square, int[][] mapSquare)
		{
			List<String> Coins = new List<String>();
			playerPositionY -= 1;
			Coins.Add("d");
			if (playerPositionY >= square || playerPositionX >= square || playerPositionX < 0 || playerPositionY < 0)
				return Coins;
			for (int y = playerPositionY; y >= 0; y--)
			{
				for (int x = 0; x <= square - 1; x++)
				{
					if (mapSquare[square - 1 - x][y] == 2)
						Coins.Add($"{x},{y}");
				}
			}
			if (mapSquare[square - 1 - playerPositionX][playerPositionY] == 2)
				Coins.Add("1");
			return (Coins);
		}
		private static List<String> GetWestCoins(int playerPositionX, int playerPositionY, int square, int[][] mapSquare)
		{
			List<String> Coins = new List<String>();
			playerPositionX -= 1;
			Coins.Add("l");
			if (playerPositionY >= square || playerPositionX >= square || playerPositionX < 0 || playerPositionY < 0)
				return Coins;
			for (int y = square - 1; y >= 0; y--)
			{
				for (int x = 0; x <= playerPositionX; x++)
				{
					if (mapSquare[square - 1 - x][y] == 2)
						Coins.Add($"{x},{y}");
				}
			}
			if (mapSquare[square - 1 - playerPositionX][playerPositionY] == 2)
				Coins.Add("1");
			return (Coins);
		}
		private static List<String> GetNorthEastCoins(int playerPositionX, int playerPositionY, int square, int[][] mapSquare)
		{
			List<String> Coins = new List<String>();
			playerPositionY += 1;
			playerPositionX += 1;
			Coins.Add("ur");
			if (playerPositionY >= square || playerPositionX >= square || playerPositionX < 0 || playerPositionY < 0)
				return Coins;
			for (int y = square - 1; y >= playerPositionY; y--)
			{
				for (int x = playerPositionX; x <= square - 1; x++)
				{
					if (mapSquare[square - 1 - x][y] == 2)
						Coins.Add($"{x},{y}");
				}
			}
			if (mapSquare[square - 1 - playerPositionX][playerPositionY] == 2)
				Coins.Add("1");
			return (Coins);
		}
		private static List<String> GetSouthEastCoins(int playerPositionX, int playerPositionY, int square, int[][] mapSquare)
		{
			List<String> Coins = new List<String>();
			playerPositionY -= 1;
			playerPositionX += 1;
			Coins.Add("dr");
			if (playerPositionY >= square || playerPositionX >= square || playerPositionX < 0 || playerPositionY < 0)
				return Coins;
			for (int y = playerPositionY; y >= 0; y--)
			{
				for (int x = playerPositionX; x <= square - 1; x++)
				{
					if (mapSquare[square - 1 - x][y] == 2)
						Coins.Add($"{x},{y}");
				}
			}
			if (mapSquare[square - 1 - playerPositionX][playerPositionY] == 2)
				Coins.Add("1");
			return (Coins);
		}
		private static List<String> GetSouthWestCoins(int playerPositionX, int playerPositionY, int square, int[][] mapSquare)
		{
			List<String> Coins = new List<String>();
			playerPositionY -= 1;
			playerPositionX -= 1;
			Coins.Add("dl");
			if(playerPositionY >= square || playerPositionX >= square || playerPositionX < 0 || playerPositionY < 0)
				return Coins;
			for (int y = playerPositionY; y >= 0; y--)
			{
				for (int x = 0; x <= playerPositionX; x++)
				{
					if (mapSquare[square - 1 - x][y] == 2)
						Coins.Add($"{x},{y}");
				}
			}
			if (mapSquare[square - 1 - playerPositionX][playerPositionY] == 2)
				Coins.Add("1");
			return (Coins);
		}
		private static List<String> GetNorthWestCoins(int playerPositionX, int playerPositionY, int square, int[][] mapSquare)
		{
			List<String> Coins = new List<String>();
			playerPositionY += 1;
			playerPositionX -= 1;
			Coins.Add("ul");
			if (playerPositionY >= square || playerPositionX >= square || playerPositionX < 0 || playerPositionY < 0)
				return Coins;
			for (int y = square - 1; y >= playerPositionY; y--)
			{
				for (int x = 0; x <= playerPositionX; x++)
				{
					if (mapSquare[square - 1 - x][y] == 2)
						Coins.Add($"{x},{y}");
				}
			}
			if (mapSquare[square - 1 - playerPositionX][playerPositionY] == 2)
				Coins.Add("1");
			return (Coins);
		}
	}
}
