using Snake.InGameModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.InGameModels
{
	public class FruitObjectSuperPoints : Fruit
	{
		private char bodyChar;
		private int superPoints;
		public FruitObjectSuperPoints(int positionWidth, int positionHeight) : base(positionWidth, positionHeight)
		{
			bodyChar = '@';
			Random rnd = new Random();
			superPoints = rnd.Next(1,3);
		}
		public override char BodyChar { get { return this.bodyChar; } }
		public int SuperPoints { get { return this.superPoints; } }
	}
}
