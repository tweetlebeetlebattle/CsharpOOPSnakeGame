using Snake.InGameModels;
using Snake.InGameModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.GameFlowModels.Interface
{
	public class IGameMap
	{
		private int gameBoardWidth;
		private int gameBoardHeight;
		private List<IGameObject> gameBoard;

		int GameBoardWidth { get; }
		int GameBoardHeight { get; }
		List<IGameObject> GameBoard { get; }

		void Initialize(string mapSize)
		{
			SetBoardSize(mapSize);
		}
		private void SetBoardSize(string screenSize)
		{
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
		}

		List<(int, int)> CalculatePlayersStartingCoordinates(int numberOfPlayers)
		{
			List<(int, int)> startCoords = new List<(int, int)>();
			for(int i = 0; i < numberOfPlayers; i++)	// calculate equally spaced starting positions for a maximum of 4 players using a runtime calculation
			{
				int playersCountOffset = numberOfPlayers == 1 ? 0 : 1;
				int widthOffset = (i % 2 == 0) ? 1 : 2;
				int heightOffset = i < 2 ? 1 : 2;
				int coordWidth = (GameBoardWidth / (2 + playersCountOffset) * widthOffset);
				int coordHeight = (GameBoardHeight / (2 + playersCountOffset) * heightOffset); 
				(int, int) coords = (coordWidth, coordHeight);
				startCoords.Add(coords);
			}
			return startCoords;
		}
		void GenerateEmptyMapAndWalls()
		{
			for (int i = 0; i < GameBoardHeight; i++)
			{
				for (int j = 0; j < GameBoardWidth; j++)
				{
					int gameBoardIndex = (i * GameBoardWidth) + j;
					if (i == 0 || i == (GameBoardHeight - 1))
					{
						gameBoard[gameBoardIndex] = (new WallObject(j, i));
					}
					else
					if (j == 0 || j == (GameBoardWidth - 1))
					{
						gameBoard[gameBoardIndex] = (new WallObject(j, i));
					}
					else
					{
						gameBoard[gameBoardIndex] = (new EmptyObject(j, i));
					}
				}
			}
		}
		void RenderMap()
		{
			for (int i = 0; i < GameBoardHeight; i++)
			{
				for (int j = 0; j < GameBoardWidth; j++)
				{
					int gameBoardIndex = (i * GameBoardWidth) + j;
					Console.Write($"{gameBoard[gameBoardIndex].RenderChar}");
				}
			}
		}
	}
}
