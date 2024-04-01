using Snake.InGameModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.InGameModels
{
	public class FruitObject : IGameObject, IFruit
	{
		private bool isFood;
		private bool isPassable;
		private bool isEaten;
		private const char bodyChar = '$';
		private (int, int) coordinates;
        public FruitObject(int positionWidth, int positionHeight)
        {
			coordinates.Item1 = positionWidth;
			coordinates.Item2 = positionHeight;
			isFood = true;
			isPassable = true;
			isEaten = false;
        }
        public bool IsFood { get { return isFood; } }

		public bool IsPassable { get { return isPassable; } }

		public char BodyChar { get { return bodyChar; } }


		public void GetEaten(List<IGameObject> objectsList)
		{
			objectsList.Remove(this);
		}

		public (int, int) Coordinates 
		{
			get 
			{
				return this.coordinates;
			}
		}
	}
}
