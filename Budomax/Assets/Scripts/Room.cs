using State;
using System;
using UnityEngine;

public class Room : MonoBehaviour, IRoom
{
    public Action<Room, RoomTypeSelection> OnRoomTypeSelected;
    public RoomTypeSelection roomType;

    public Material material {
        set { GetComponent<MeshRenderer>().material = value; } }

    private void Start()
    {
        PopupManager.instance.ShowRoomCreationPopup(RoomTypeSelected);
    }

    private void RoomTypeSelected( RoomTypeSelection roomType )
    {
        this.roomType = roomType;
        OnRoomTypeSelected?.Invoke(this, roomType);
    }
}
