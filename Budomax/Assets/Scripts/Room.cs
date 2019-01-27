using State;
using System;
using UnityEngine;

public class Room : MonoBehaviour, IRoom
{
    public RoomData roomData;

    public Action<Room, RoomTypeSelection> OnRoomTypeSelected;
    private RoomTypeSelection roomType;

    public Material material {
        set { GetComponent<MeshRenderer>().material = value; } }

    public Vector2 Position { get { return transform.position; } }

    public RoomTypeSelection Type { get { return roomType; } }

    public int MaxWorkers { get; set; }

    public int Workers { get; set; }

    void Awake()
    {
        Workers = 0;
        MaxWorkers = 3;
    }

    private void Start()
    {
        PopupManager.instance.ShowRoomCreationPopup(RoomTypeSelected);
    }

    private void RoomTypeSelected( RoomTypeSelection roomType )
    {
        this.roomType = roomType;
        OnRoomTypeSelected?.Invoke(this, roomType);
    }

    public void Enter()
    {
        if (Workers == MaxWorkers) { return; }
        Workers += 1;
    }

    public void Leave()
    {
        if (Workers == 0) { return; }
        Workers -= 1;
    }
}
