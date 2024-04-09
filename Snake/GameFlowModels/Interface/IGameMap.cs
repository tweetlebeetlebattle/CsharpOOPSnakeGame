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
		int GameBoardWidth { get; }
		int GameBoardHeight { get; }

		List<IGameObject> gameBoard;
		List<(int, int)> CalculateStartingCoordinates(int numberOfPlayers)
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
	}
}
