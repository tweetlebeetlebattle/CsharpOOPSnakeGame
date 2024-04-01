using Snake.GameObjects.Interface;
using Snake.InGameModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.InGameModels
{
    public class EmptyObject : IGameObject
    {
        private bool isFood;
        private bool isPassable;
        private (int, int) coordinates;
        public EmptyObject(int width, int height)
        {
            coordinates.Item1 = width;
            coordinates.Item2 = height;
            isFood = false;
            isPassable = true;
        }
        public bool IsFood { get { return isFood; } }

        public bool IsPassable { get { return isPassable; } }
        public (int, int) Coordinates
        {
            get
            {
                return coordinates;
            }
        }
    }
}
