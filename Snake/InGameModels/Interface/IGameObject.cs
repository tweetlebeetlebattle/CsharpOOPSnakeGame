namespace Snake.InGameModels.Interface
{
    public interface IGameObject
    {
        public bool IsFood { get; }
        public bool IsPassable { get; }
        public (int, int) Coordinates { get; }
    }
}