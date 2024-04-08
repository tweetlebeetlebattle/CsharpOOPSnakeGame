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
			Console.WriteLine("For SinglePlayer press 1 or Multiplayer press 2");
			int numberOfPlayers = int.Parse(Console.ReadLine());
			Console.WriteLine("Enter playbox size, available: small, medium, full");
			string size = Console.ReadLine();
			EngineFuel_MakeGameToBeLoaded game = new EngineFuel_MakeGameToBeLoaded(numberOfPlayers, size);
			bool runGame = false;
			while (!runGame)
			{
				runGame = game.GameRunning();
			}
		}
	}
}
