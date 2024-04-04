using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.InGameModels.Interface
{
	public abstract class Fruit : IFruit, IGameObject
	{
		private bool isFood;
		private bool isPassable;
		public abstract char BodyChar { get; }
		private (int, int) coordinates;
		public Fruit(int positionWidth, int positionHeight)
		{
			coordinates.Item1 = positionWidth;
			coordinates.Item2 = positionHeight;
			isFood = true;
			isPassable = true;
		}
		public bool IsFood { get { return isFood; } }

		public bool IsPassable { get { return isPassable; } }
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
