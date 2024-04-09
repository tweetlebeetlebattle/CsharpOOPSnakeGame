using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
	public class GameLoaderEngine
	{
		public void Run()
		{
			int numberOfPlayers = LoadPlayerCount();
			Console.WriteLine("Enter playbox size, available: small, medium, full");
			string size = LoadMapSize();
			EngineFuel_MakeGameToBeLoaded game = new EngineFuel_MakeGameToBeLoaded(numberOfPlayers, size);
			bool runGame = false;
			while (!runGame)
			{
				runGame = game.GameRunning();
			}
		}
		public int LoadPlayerCount()
		{
			Console.WriteLine("For SinglePlayer press 1 or Multiplayer press 2");
			while (true)
			{
				int userOutput = 0;
				string input = Console.ReadLine();
				if(int.TryParse(input, out userOutput))
				{
					if(userOutput == 1 || userOutput == 2)
					{
						return userOutput;
					}
				}
				Console.WriteLine($"Invalid Player count input: {input}. Please enter again! ");
			}
		}
		public string LoadMapSize()
		{
			Console.WriteLine("Enter playbox size, available: small, medium, full");
			while (true)
			{
				string input = Console.ReadLine();
				if (input == "small" || input == "medium" || input == "full")
				{
					return input;
				}
				Console.WriteLine($"Invalid Map size input: {input}. Please enter again! ");
			}
		}
	}
}
