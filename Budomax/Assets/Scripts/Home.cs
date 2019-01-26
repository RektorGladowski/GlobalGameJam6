using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State;

public class Home : MonoBehaviour, IHome
{
    public List<GameObject> RoomObjects;

    public List<GameObject> WallObjects;

    public IEnumerable<IRoom> Rooms
    {
        get
        {
            List<IRoom> rooms = new List<IRoom>();
            foreach (var obj in RoomObjects)
            {
                IRoom room = obj.GetComponent<Kitchen>();
                if (room != null)
                {
                    rooms.Add(room);
                }
            }
            return rooms;
        }
    }

    public IEnumerable<IWall> Walls
    {
        get
        {
            List<IWall> walls = new List<IWall>();
            foreach (var obj in WallObjects)
            {
                IWall wall = obj.GetComponent<Wall>();

                // Hack, proper implementation of IHome should be better
                if (wall == null)
                {
                    wall = obj.GetComponent<Pantry>();
                }

                if (wall != null)
                {
                    walls.Add(wall);
                }
            }
            return walls;
        }
    }
}
