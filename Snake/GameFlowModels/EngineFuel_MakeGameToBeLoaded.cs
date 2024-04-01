﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Snake.GameObjects;
using Snake.InGameModels;
using Snake.InGameModels.Interface;

namespace Snake
{
    public class EngineFuel_MakeGameToBeLoaded
		// Game should Implement gamemode Interface, so It can choose one of many categories.
		// Create GameMode Classes for my "Injected" GameMode Property.
		// Currently. my implementation only loads graphics and object positions.
	{
		private List<SnakeObject> players;
		private List<Dictionary<string, char>> playerControls;
		private List<int> scores;

		private const int gameBoardWidth = 110;
		private const int gameBoardHeight = 30;
		private List<IGameObject> gameBoard;

        public EngineFuel_MakeGameToBeLoaded(int singleOrMultiplayer)
        {
			gameBoard = new List<IGameObject>();
			playerControls = new List<Dictionary<string,char>>();
			players = new List<SnakeObject>();
			List<int> startingSnakePositions = snakeStartPositionCalculator(singleOrMultiplayer);
			bool directionModifier = true;
			int bodyCharLoader = 1;
			foreach(int position in startingSnakePositions)
			{
				SnakeObject newPlayer = new SnakeObject(position, 15, directionModifier, char.Parse(bodyCharLoader.ToString()));
				if(directionModifier)
				{
					directionModifier = false;
				} else
				{
					directionModifier = true;
				}
				players.Add(newPlayer);
				bodyCharLoader++;
			}
			for(int i = 0; i < players.Count; i++)
			{
				Dictionary<string, char> playerKeyBinds = new Dictionary<string, char>();
				string movement = ""; 
				Console.WriteLine($"KeyBindings: Player1:");
				for(int k = 0; k < 4; k++)
				{
					if( k == 0)
					{
						movement = "Up";
					}
					if( k == 1)
					{
						movement = "Down";
					}
					if( k == 2)
					{
						movement = "Left";
					}
					if( k == 3)
					{
						movement = "Right";
					}
					Console.Write($"{movement}:"); char keyBind = char.Parse(Console.ReadLine());
					playerKeyBinds.Add(movement, keyBind);
				}
				playerControls.Add(playerKeyBinds);
			}
        }
		public bool RenderBoard()
		{
			Console.Clear();
			GenerateAndRenderWalls();
			foreach (var snake in players)
			{
				RenderSnake(snake);
			}
			GenerateAndOrRenderFruit();
			GenerateScore();
			if (CollisionCheck())
			{
				return true;
			}
			ReadControlsToUpdatePlayerCoords();
			Console.ResetColor();
			return false;
			// How to detect collision
			// how to grow snakes on fruit and endgame on colision.
		}
		public bool CollisionCheck()
		{
			for (int i = 0; i < players.Count; i++)
			{
				int headWidth = players[i].Coordinates.Item1;
				int headHeight = players[i].Coordinates.Item2;
				int indexInTable = headHeight * gameBoardWidth + headWidth;
				if (gameBoard[indexInTable] is WallObject)
				{
					Console.Clear();
					Console.WriteLine($"Snake Player: {i + 1} LOSE!");
					Console.BackgroundColor = ConsoleColor.Red;
					return true;
				}
				if (gameBoard[indexInTable] is FruitObject)
				{
					players[i].Eat();
					IFruit fruit = (FruitObject)gameBoard[indexInTable];
					fruit.GetEaten(gameBoard);
					Console.BackgroundColor = ConsoleColor.Green;
				}
			}
			return false;
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
			TimeSpan duration = TimeSpan.FromMilliseconds(200);

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
					foreach(var dictionary in playerControls)
					{
						if(dictionary.ContainsValue(control))
						{
							string action = dictionary.FirstOrDefault(item => item.Value == control).Key;
							allUserInput.Add(action);
							actionNotFound = false;
						} 
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
		}
		public List<SnakeObject> Players
		{
			get
			{
				return players;
			}
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
			if(!gameBoard.Any(obj => obj is FruitObject))	// generate if fruit missing
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
				IGameObject newFruit = new FruitObject(randomValidWidth, randomValidHeight);
				gameBoard[randomValidHeight * gameBoardWidth + randomValidWidth] = newFruit;
			}   // render fruit
			FruitObject fruit = (FruitObject)gameBoard.First(obj => obj is FruitObject);
			Console.SetCursorPosition(fruit.Coordinates.Item1, fruit.Coordinates.Item2);
			Console.Write(fruit.BodyChar);
		}
		private void GenerateAndRenderWalls()
		{
			for (int i = 0; i < gameBoardHeight; i++)
			{
				for (int j = 0; j < gameBoardWidth; j++)
				{
					if (i == 0 || i == (gameBoardHeight - 1))
					{
						Console.Write("x");
						gameBoard.Add(new WallObject(j,i));
					}
					else
					if (j == 0 || j == (gameBoardWidth - 1))
					{
						Console.Write("x");
						gameBoard.Add(new WallObject(j, i));
					}
					else
					{
						Console.Write(" ");
						gameBoard.Add(new EmptyObject(j, i));
					}
				}
				Console.WriteLine();
			}
		}
		private List<int> snakeStartPositionCalculator(int oneOrTwoPlayers)
		{
			List<int> centerPoints = new List<int>();
			if(oneOrTwoPlayers == 1)
			{
				centerPoints.Add((gameBoardWidth - 2) / 2);
			} 
			if(oneOrTwoPlayers == 2)
			{
				centerPoints.Add((gameBoardWidth - 2) / 3);
				centerPoints.Add(((gameBoardWidth - 2) / 3) * 2);
			}
			return centerPoints;
		}
	}
}