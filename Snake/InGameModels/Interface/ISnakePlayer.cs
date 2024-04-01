using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.InGameModels.Interface
{
    public interface ISnakePlayer
    {
        public int LastDirection { get; }
        public int Score { get; }
        public List<(int, int)> SnakeBodyCoordinates { get; } // Linear gowrth in datasize in runtime, could potentially be improved

        public void Eat();

    }
}
