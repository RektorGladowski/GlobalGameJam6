using UnityEngine;

[RequireComponent(typeof(HouseManager))]
public class RoomGenerator : MonoBehaviour
{
    public HouseManager houseManager { get { if (_houseManager == null) _houseManager = GetComponent<HouseManager>(); return _houseManager; } }
    private HouseManager _houseManager;

    [SerializeField] private Material kitchenMaterial;
    [SerializeField] private Material baracksRoomMaterial;
    [SerializeField] private Material scavengerRoomMaterial;

    private void Awake() { houseManager.OnHouseRebuild = UpdateRooms; }
    private void OnDestroy() { houseManager.OnHouseRebuild = null; }

    private void UpdateRooms(RoomData[] roomDatas)
    {
        for (int i = 0; i < roomDatas.Length; i++)
        {
            GameObject go = new GameObject("RoomName", typeof(MeshFilter), typeof(MeshRenderer));
            go.GetComponent<MeshFilter>().mesh = roomDatas[i].Mesh;
        }
    }


}
