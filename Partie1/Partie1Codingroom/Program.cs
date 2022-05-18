using System;
using System.Collections.Generic;
using System.IO;

namespace SelectionCandidat
{
	class Program
	{
		static void Main(string[] args)
		{
			string textFile = @"C:\Users\valgo\OneDrive\Bureau\C#\CodingRoom\Partie1Codingroom\Partie1Codingroom\data-test3.txt";
			var data = new List<String>();
			string line;
			if (File.Exists(textFile))
			{
				// Read file using StreamReader. Reads file line by line
				using (StreamReader file = new StreamReader(textFile))
				{
					while ((line = file.ReadLine()) != null)
					{
						data.Add(line);
					}
					file.Close();
				}
			}
			//////
			/* Get data from input file */
			// ADD YOUR CODE HERE
			string[] playerPosition = data[1].Split(',');
			int playerPositionX = int.Parse(playerPosition[0]);
			int playerPositionY = int.Parse(playerPosition[1]);
			char[] instructions = new char[int.Parse(data[2])];

			// Parsing des instructions de deplacements.
			for (int k = 0; k < int.Parse(data[2]); k++)
				instructions[k] = char.Parse(data[3 + k]);
			for (int k = 0; k < int.Parse(data[2]); k++)
			{
				if (instructions[k] == 'u')
					playerPositionY += 1;
				else if (instructions[k] == 'l')
					playerPositionX -= 1;
				else if (instructions[k] == 'r')
					playerPositionX += 1;
				else if (instructions[k] == 'd')
					playerPositionY -= 1;
			}
			if (playerPositionX >= int.Parse(data[0]) || playerPositionX < 0 || playerPositionY >= int.Parse(data[0]) || playerPositionY < 0)
				Console.WriteLine('0');
			else
				Console.WriteLine($"{playerPositionX},{playerPositionY}");
			DisplayMap(int.Parse(data[0]), playerPositionX, playerPositionY);
		}

		// Methode affichant la map avec des 0 aux emplacements ou le joueur n'est pas et un 1 a l'emplacement du joueur.
		private static void DisplayMap(int mapSquare, int playerPositionX, int playerPositionY)
		{
			int[][] square = new int[mapSquare][];

			for (int k = 0; k < mapSquare; k++)
				square[k] = new int[mapSquare];
			for (int j = mapSquare - 1; j >= 0; j--)
			{
				for (int i = 0; i <= mapSquare - 1; i++)
				{
					if (playerPositionX == i && playerPositionY == j)
						Console.Write("1 ");
					else
						Console.Write($"{square[i][j]} ");
				}
				Console.Write('\n');
			}
		}
	}
}
