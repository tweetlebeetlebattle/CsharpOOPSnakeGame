using Snake.GameObjects.Interface;
using Snake.InGameModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.InGameModels
{
    public class WallObject : IGameObject
    {
        private bool isFood;
        private bool isPassable;
        private (int, int) coordinates;
        private char renderChar = 'x';
        public WallObject(int height, int width)
        {
            isFood = false;
            isPassable = false;
            coordinates.Item1 = width;
            coordinates.Item2 = height;
        }
        public (int,int) Coordinates
        {
            get
            {
                return this.coordinates;
            }
        }
        public bool IsFood { get { return isFood; } }

        public bool IsPassable { get { return isPassable; } }

		public char RenderChar
        {
            get { return renderChar; }
        }
	}
}
