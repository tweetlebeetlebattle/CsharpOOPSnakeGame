using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.InGameModels.Interface
{
    public interface IFruit
    {
        public char BodyChar { get; }
        public void GetEaten(List<IGameObject> objectsList);
    }
}
