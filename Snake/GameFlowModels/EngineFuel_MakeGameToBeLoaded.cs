using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Snake.GameObjects;
using Snake.InGameModels;
using Snake.InGameModels.Interface;

namespace Snake
{
    public class EngineFuel_MakeGameToBeLoaded
	{
		private List<SnakeObject> players;
		private List<Dictionary<string, char>> playerControls;

		private  int gameBoardWidth; // 110
		private  int gameBoardHeight; // 30
		private List<IGameObject> gameBoard;

        public EngineFuel_MakeGameToBeLoaded(int singleOrMultiplayer, string screenSize)
        {

			gameBoard = new List<IGameObject>();
			playerControls = new List<Dictionary<string, char>>();
			players = new List<SnakeObject>();

			switch (screenSize)
			{
				case "small":
					gameBoardWidth = 50;
					gameBoardHeight = 15;
					break;
				case "medium":
					gameBoardWidth = 75;
					gameBoardHeight = 22;
					break;
				case "full":
					gameBoardWidth = 110;
					gameBoardHeight = 30;
					break;
			}
			InitializeSnakePlayers(singleOrMultiplayer);
			InitializeKeyBindings();
		}
		private void InitializeSnakePlayers(int singleOrMultiplayer)
		{
			bool directionModifier = true;
			int bodyCharLoader = 1;
			List<(int, int)> startingSnakePositions = CalculateStartingCoordinates(singleOrMultiplayer);
			foreach (var position in startingSnakePositions)
			{
				SnakeObject newPlayer = new SnakeObject(position.Item1, position.Item2, directionModifier, char.Parse(bodyCharLoader.ToString()));
				if (directionModifier)
				{
					directionModifier = false;
				}
				else
				{
					directionModifier = true;
				}
				players.Add(newPlayer);
				bodyCharLoader++;
			}
		}
		List<(int, int)> CalculateStartingCoordinates(int numberOfPlayers)
		{
			List<(int, int)> startCoords = new List<(int, int)>();
			for (int i = 0; i < numberOfPlayers; i++)   // calculate equally spaced starting positions for a maximum of 4 players using a runtime calculation
			{
				int playersCountOffset = numberOfPlayers == 1 ? 0 : 1;
				int widthOffset = (i % 2 == 0) ? 1 : 2;
				int heightOffset = i < 2 ? 1 : 2;
				int coordWidth = (gameBoardWidth / (2 + playersCountOffset) * widthOffset);
				int coordHeight = (gameBoardHeight / (2 + playersCountOffset) * heightOffset);
				(int, int) coords = (coordWidth, coordHeight);
				startCoords.Add(coords);
			}
			return startCoords;
		}
		private void InitializeKeyBindings()
		{
			for (int i = 0; i < players.Count; i++)
			{
				Dictionary<string, char> playerKeyBinds = new Dictionary<string, char>();
				string movement = "";
				Console.WriteLine($"KeyBindings: Player{i + 1}:");
				for (int k = 0; k < 4; k++)
				{
					if (k == 0)
					{
						movement = "Up";
					}
					if (k == 1)
					{
						movement = "Down";
					}
					if (k == 2)
					{
						movement = "Left";
					}
					if (k == 3)
					{
						movement = "Right";
					}
					Console.Write($"{movement}:"); char keyBind = char.Parse(Console.ReadLine());
					playerKeyBinds.Add(movement, keyBind);
				}
				playerControls.Add(playerKeyBinds);
			}
		}
		public bool GameRunning()
		{
			Console.Clear();
			GenerateAndRenderWalls();
			foreach (var snake in players)
			{
				RenderSnake(snake);
			}
				GenerateAndOrRenderFruit();
			GenerateScore();
			if (CollisionCheck()) // a bug in the game causes to load wrong coordinates
			{
				return true;
			}
			ReadControlsToUpdatePlayerCoords();
			Console.ResetColor();
			return false;
		}
		public bool CollisionCheck()
		{
			for (int i = 0; i < players.Count; i++)
			{
				int headWidth = players[i].Coordinates.Item1;
				int headHeight = players[i].Coordinates.Item2;
				int indexInTable = headHeight * gameBoardWidth + (headWidth); // Find the Head coordinates in the 2x2 table
				if (gameBoard[indexInTable] is WallObject)	// check Wall
				{
					Console.Clear();
					Console.WriteLine($"Player {i+1} ate Wall!");
					return true;
				}
				for(int k = 0; k < players.Count; k++) // check collision with another player
				{
					for (int u = 0; u < players[k].SnakeBodyCoordinates.Count; u++)
					{
						if(k == i && u == 0) // skip current players head
						{
							continue;
						}
						if (players[k].SnakeBodyCoordinates.Count > 5)
						{

						}
						if ((players[k].SnakeBodyCoordinates[u] == gameBoard[indexInTable].Coordinates) && (gameBoard[indexInTable].Coordinates == players[i].Coordinates))
						{

							Console.Clear();
							Console.WriteLine($"Player {i + 1} ate Snake!");
							return true;
						}
					}
				}
				if (gameBoard[indexInTable] is Fruit) // Cbeck Fruit
				{
					players[i].Eat((IFruit)gameBoard[indexInTable]);
					IFruit fruit = (IFruit)gameBoard[indexInTable];
					if(fruit is FruitObjectFreezeOthers)
					{
						FreezeAllButOne(players[i]);
					}
					fruit.GetEaten(gameBoard);
					Console.BackgroundColor = ConsoleColor.Green;
				}
			}
			return false;
		}
		private void FreezeAllButOne(ISnakePlayer thisPlayer)
		{
			foreach(var snake in players)
			{
				if( snake == thisPlayer)
				{
					continue;
				}
				snake.IsFrozen = true;
			}
		}
		public void ReadControlsToUpdatePlayerCoords()
		{
			StringBuilder userInputBuilder = new StringBuilder();

			// Set the cursor position
			Console.SetCursorPosition(111, 22);
			Console.Write("Input");
			Console.SetCursorPosition(111, 23);
			Console.Write(":");

			DateTime startTime = DateTime.Now;
			TimeSpan duration = TimeSpan.FromMilliseconds(100);

			while ((DateTime.Now - startTime) < duration)
			{
				if (Console.KeyAvailable)
				{
					ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
					char keyChar = keyInfo.KeyChar;

					if (keyChar == '\r') // Enter key
					{
						break;
					}

					userInputBuilder.Append(keyChar);
				}
				Thread.Sleep(10); 
			}

			string inputString = userInputBuilder.ToString();
			Controls(inputString);
		}
		public void Controls(string inputString)
		{
			List<char> inputStringToList = inputString.ToList();
			List<string> allUserInput = new List<string>();
			for(int i = 0; i < players.Count;  i++)
			{
				bool actionNotFound = true;
				foreach(char control in inputStringToList)
				{
					var dictionary = playerControls[i];
					if (dictionary.ContainsValue(control))
					{
						string action = dictionary.FirstOrDefault(item => item.Value == control).Key;
						allUserInput.Add(action);
						actionNotFound = false;
					}
				}
				if (actionNotFound)
				{
					players[i].MoveSnakeHeadController("KeepPreviousDirection");
				} else
				{
					players[i].MoveSnakeHeadController(allUserInput.Last());
				}
			}
		}
		public void GenerateScore()
		{
			for(int i =0; i < players.Count; i++)
			{
				Console.SetCursorPosition(111, 5 + (i * 5));
				Console.Write($"Player {i + 1}");
				Console.SetCursorPosition(111, 6 + (i * 5));
				Console.Write($"Score:{players[i].Score}");
			}
			Console.SetCursorPosition(111, 8);	Console.Write($"$ Basic");
			Console.SetCursorPosition(111, 9); Console.Write($"@ Super");
			Console.SetCursorPosition(111, 10); Console.Write($"# Freeze");

		}
		public void RenderSnake(SnakeObject snake)
		{
			for(int i =0; i < snake.SnakeBodyCoordinates.Count; i++)
			{
				Console.SetCursorPosition(snake.SnakeBodyCoordinates[i].Item1, snake.SnakeBodyCoordinates[i].Item2);
				if (i == 0)
				{
					Console.Write(snake.HeadCharSymbol);
				}
				else
				{
					Console.Write(snake.BodyCharSymbol);
				}
			}
		}
		public void GenerateAndOrRenderFruit()
		{
			if(!gameBoard.Any(obj => obj is Fruit))	// generate if fruit missing
			{
				List<IGameObject> validEmptySpacesPickFruitSpawnPoint = new List<IGameObject>();
				foreach(var validEmptyObject in gameBoard.Where(obj => obj is EmptyObject))	// validate passable object
				{
					foreach( var snake in players) // validate no snake body
					{
						if(!snake.SnakeBodyCoordinates.Any(data => data == validEmptyObject.Coordinates))
						{
							validEmptySpacesPickFruitSpawnPoint.Add(validEmptyObject);
						}
					}
				}
				Random rnd = new Random();
				int randomIndexToSwapEmptyWithFruit = rnd.Next(validEmptySpacesPickFruitSpawnPoint.Count);
				int randomValidWidth = validEmptySpacesPickFruitSpawnPoint[randomIndexToSwapEmptyWithFruit].Coordinates.Item1;
				int randomValidHeight = validEmptySpacesPickFruitSpawnPoint[randomIndexToSwapEmptyWithFruit].Coordinates.Item2;

				int randomFruitPicker = rnd.Next(6); // if I increase this number, i will increase the odds away from the unique fruit generation
				IGameObject newFruit = null;
				if(randomFruitPicker == 0)
				{
					newFruit = new FruitObjectSuperPoints(randomValidWidth, randomValidHeight);
				}
				else if( randomFruitPicker == 1)
				{
					newFruit = new FruitObjectFreezeOthers(randomValidWidth, randomValidHeight);
				} else
				{
					newFruit = new FruitObjectBasic(randomValidWidth, randomValidHeight);
				}
				
				gameBoard[randomValidHeight * gameBoardWidth + randomValidWidth] = newFruit;
			}   // render fruit
			Fruit fruit = (Fruit)gameBoard.First(obj => obj is Fruit);
			Console.SetCursorPosition(fruit.Coordinates.Item1, fruit.Coordinates.Item2);
			Console.Write(fruit.BodyChar);
		}
		private void GenerateAndRenderWalls()
		{
			for (int i = 0; i < gameBoardHeight; i++)
			{
				for (int j = 0; j < gameBoardWidth; j++)
				{
					int gameBoardIndex = (i * gameBoardWidth) + j;
					if (i == 0 || i == (gameBoardHeight - 1))
					{
						Console.Write($"x");
						if(gameBoard.Count <= gameBoardIndex || gameBoard.Count == 0)
						{
							gameBoard.Add(new WallObject(j, i));
						}
						gameBoard[gameBoardIndex] = (new WallObject(j, i));
					}
					else
					if (j == 0 || j == (gameBoardWidth - 1))
					{
						Console.Write($"x");
						if (gameBoard.Count <= gameBoardIndex || gameBoard.Count == 0)
						{
							gameBoard.Add(new WallObject(j, i));
						}
						gameBoard[gameBoardIndex] = (new WallObject(j, i));
					}
					else
					{
						if(gameBoard.Count > gameBoardIndex && gameBoard[gameBoardIndex] is Fruit)
						{
							Console.Write(" ");
							continue;
						}
						Console.Write(" ");
						if (gameBoard.Count <= gameBoardIndex || gameBoard.Count == 0)
						{
							gameBoard.Add(new EmptyObject(j, i));
						}
						gameBoard[gameBoardIndex] = (new EmptyObject(j, i));
					}
				}
				Console.WriteLine();
			}
		}
	}
}
