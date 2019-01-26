namespace State
{
    public interface IPantry : IWall
    {
        int MaxFood { get; }

        int Food { get; }

        void Fill(int amount);

        void Eat(int amount);
    }
}
