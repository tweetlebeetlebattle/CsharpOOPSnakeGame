using Snake.GameObjects.Interface;
using Snake.InGameModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.InGameModels
{
    public class SnakeObject : ISnakePlayer, IGameObject
    {
        private const int initialSize = 4;
        private const char headCharSymbol = 'O';
        private char bodyCharSymbol;
        private List<(int, int)> coordinatesWholeSnake;
        private (int, int) coordinates;
        private int score;
        private bool isFood;
        private bool isPassable;
        private int laStDirection;
        public SnakeObject(int headWidth, int headHight, bool leftRightDirectionInitialGrowthIndex, char bodyCharSymbol)
        {
            // Console.SetCursorPosition(width, height);
            // snake object coords item1 item2 match the setCursor parameters sequence
            // if true modifier is 1, if false -1

            int directionFacingModifier = leftRightDirectionInitialGrowthIndex ? 1 : -1;
			laStDirection = leftRightDirectionInitialGrowthIndex ? 4 : 6;
			coordinatesWholeSnake = new List<(int, int)>();
            for (int i = 0; i < initialSize; i++)
            {
                (int, int) coordinate = (headWidth + i * directionFacingModifier, headHight);
				coordinatesWholeSnake.Add(coordinate);
            }
            score = 0;
            isFood = false;
            isPassable = false;
            this.bodyCharSymbol = bodyCharSymbol;
        }
        public (int, int) Coordinates
        {
            get
            {
                return coordinatesWholeSnake.First();
			}
        }
        public int LastDirection { get { return laStDirection; } }
        public bool IsFood
        {
            get
            {
                return isFood;
            }
            private set
            {
                isFood = value;
            }
        }
        public bool IsPassable { get { return isPassable; } }
        public char HeadCharSymbol { get { return headCharSymbol; } }
        public char BodyCharSymbol { get { return bodyCharSymbol; } }
        public int Score
        {
            get { return score; }
            private set
            {
                score = value;
            }
        }
        public void RenderSnake()
        {
            for (int i = 0; i < coordinatesWholeSnake.Count; i++)
            {
                Console.SetCursorPosition(coordinatesWholeSnake[i].Item1, coordinatesWholeSnake[i].Item2);
                if (i == 0)
                {
                    Console.Write(headCharSymbol);
                }
                else
                {
                    Console.Write(bodyCharSymbol);
                }
            }
        }
        public List<(int, int)> SnakeBodyCoordinates
        {
            get
            {
                return coordinatesWholeSnake;
            }

        }

        public void Eat()
        {
            coordinatesWholeSnake.Add(coordinatesWholeSnake.Last());
            score++;
        }
        private void MoveSnakeBody()
        {
			for (int i = coordinatesWholeSnake.Count - 1; i >= 0; i--)
			{
				if (!(i == 0))
				{
					coordinatesWholeSnake[i] = coordinatesWholeSnake[i - 1];
				}
			}
		}
        public void MoveSnakeHeadController(string direction)
        {
            int directionCommanded = 0;
            switch (direction)
            {
                case "Up":
					directionCommanded = 8;
                    break;
                case "Down":
					directionCommanded = 2;
					break;
                case "Left":
					directionCommanded = 4;
					break;
                case "Right":
					directionCommanded = 6;
					break;
                default:
					directionCommanded = 5;
                    break;
			}
			MoveSnakeHead(directionCommanded);
        }
        private void MoveSnakeHead(int direction)
        {
            int directionModifier = 0;
            if (direction == 5)
            {
                directionModifier = LastDirection;
            }
            else
            {
                directionModifier = direction;
            }
            int newWidth = 0;
            int newHeight = 0;
            switch (directionModifier)
            {
                case 2:
                    newHeight = coordinatesWholeSnake.First().Item2 + 1;
                    newWidth = coordinatesWholeSnake.First().Item1;
                    laStDirection = 2;
                    break;
                case 4:
                    newHeight = coordinatesWholeSnake.First().Item2;
                    newWidth = coordinatesWholeSnake.First().Item1 - 1;
                    laStDirection = 4;
                    break;
                case 8:
                    newHeight = coordinatesWholeSnake.First().Item2 - 1;
                    newWidth = coordinatesWholeSnake.First().Item1;
                    laStDirection = 8;
                    break;
                case 6:
                    newHeight = coordinatesWholeSnake.First().Item2;
                    newWidth = coordinatesWholeSnake.First().Item1 + 1;
                    laStDirection = 6;
                    break;
            }
            MoveSnakeBody();
			coordinatesWholeSnake[0] = (newWidth, newHeight);
        }
    }
}
