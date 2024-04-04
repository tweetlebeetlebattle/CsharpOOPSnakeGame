using Snake.InGameModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.InGameModels
{
	public class FruitObjectBasic : Fruit
	{
		private char bodyChar;
		public FruitObjectBasic(int positionWidth, int positionHeight) : base(positionWidth, positionHeight)
		{
			bodyChar = '$';
		}

		public override char BodyChar { get { return this.bodyChar; } }
	}
}
