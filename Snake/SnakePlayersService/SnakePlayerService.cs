using Snake.InGameModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.SnakePlayersService
{
	public class SnakePlayerService // singleton instance, loads a snakePlayer object and manipulates its values || If i use threads for each player to be processed simultaneously the class is not singleton - will need multiple instances working on diferent data at the same time, while one instance can only handle one at a time. But This calculation will repat max 4 times oer cycle, the bottleneck of my aoo wull be how many games can I run at the same time, not how many cycles can i run per round in a game, due to the max players count == 4
	// the bottle neck will be the Engine class that runs an individual game?
	{
		ISnakePlayer player;
		
		// here I read controlls
		// update snake coordinate
		// check for collision
		//	+link up with GameMode ( validate lose/win condition )
		// returns a List of gameBoardIndex of all spaces ocupited by the snake
	}
}
