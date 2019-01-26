using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HouseManager))]
public class RoomGenerator : MonoBehaviour
{
    public HouseManager houseManager { get { if (_houseManager == null) _houseManager = GetComponent<HouseManager>(); return _houseManager; } }
    private HouseManager _houseManager;

    [SerializeField] private Material kitchenMaterial;
    [SerializeField] private Material baracksRoomMaterial;
    [SerializeField] private Material scavengerRoomMaterial;

    private void Awake() {
        
        houseManager.OnHouseRebuild = UpdateRooms;
    }
    private void OnDestroy() { houseManager.OnHouseRebuild = null; }

    Dictionary<string, Room> rooms = new Dictionary<string, Room>();

    private void UpdateRooms(RoomData[] roomDatas)
    {
        CreateMissingRooms(roomDatas);
        RemoveRooms(roomDatas);
    }


    void CreateMissingRooms(RoomData[] roomDatas)
    {
        // Create new rooms
        for (int i = 0; i < roomDatas.Length; i++)
        {
            if (!rooms.ContainsKey(roomDatas[i].ID))
            {
                GameObject go = new GameObject("RoomName", typeof(Room), typeof(MeshFilter), typeof(MeshRenderer));
                go.GetComponent<MeshFilter>().mesh = roomDatas[i].Mesh;
                go.GetComponent<MeshRenderer>().material = kitchenMaterial;
                Room room = go.GetComponent<Room>();
                room.OnRoomTypeSelected += OnRoomTypeSelected;
                rooms.Add(roomDatas[i].ID, room);
            }
        }
    }

    private void OnRoomTypeSelected(Room room, RoomTypeSelection roomType)
    {
        switch (roomType)
        {
            case RoomTypeSelection.Kitchen: room.material = kitchenMaterial; break;
            case RoomTypeSelection.Barracks: room.material = baracksRoomMaterial; break;
            case RoomTypeSelection.ScavengerRoom: room.material = scavengerRoomMaterial; break;
        }
    }

    private void RemoveRooms(RoomData[] roomDatas)
    {
        List<string> tmpList = new List<string>();
        List<string> roomDatasToRemove = new List<string>();

        for (int i = 0; i < roomDatas.Length; i++)
        {
            tmpList.Add(roomDatas[i].ID);
        }

        foreach (var room in rooms)
        {
            if (!tmpList.Contains(room.Key))
            {
                roomDatasToRemove.Add(room.Key);
                Debug.Log(room.Key + " to remove");
            }
        }

        for (int i = 0; i < roomDatasToRemove.Count; i++)
        {
            Destroy(rooms[roomDatasToRemove[i]].gameObject);
            rooms.Remove(roomDatasToRemove[i]);
        }

    }
}
