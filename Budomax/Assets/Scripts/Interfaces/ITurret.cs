namespace State
{
    public interface ITurret : IWall
    {
        bool IsEmpty { get; }

        void Enter();

        void Leave();
    }
}
