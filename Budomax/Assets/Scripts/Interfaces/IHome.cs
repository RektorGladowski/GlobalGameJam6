using System.Collections.Generic;

namespace State
{
    public interface IHome
    {
        IEnumerable<IRoom> Rooms { get; }

        IEnumerable<IWall> Walls { get; }
    }
}
