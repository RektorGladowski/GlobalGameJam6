using System.Collections.Generic;
using UnityEngine;
using PolygonArea;
using System;

[RequireComponent(typeof(CompositeCollider2D))]
public class HouseManager : MonoBehaviour
{
    public Action<RoomData[]> OnHouseRebuild;

    // Read all the colliders and create a combined Polygon2D collider from the others. See PolygonCollider2D in the docs, specifically pathCount and SetPath. 
    public static HouseManager instance;

    public CompositeCollider2D compositeCollider { get { if (_compositeCollider == null) _compositeCollider = GetComponent<CompositeCollider2D>(); return _compositeCollider; } }
    private CompositeCollider2D _compositeCollider;

    private RoomData outerWalls;
    private List<RoomData> roomDatas = new List<RoomData>();

    // Rather unnecessary ;)
    private void Awake() { Rebuild(); instance = this; }

    public void Rebuild()
    {
        roomDatas.Clear();
        outerWalls = new RoomData(new Vector2[] { new Vector2(0, 0), new Vector2(0, 0) });

        for (int i = 0; i < compositeCollider.pathCount; i++)
        {
            Vector2[] points = new Vector2[compositeCollider.GetPathPointCount(i)];

            compositeCollider.GetPath(i, points);
            RoomData roomData = new RoomData(points);

            if (outerWalls.Area < roomData.Area) outerWalls = roomData;
            else
            {
                Debug.Log("AREA: " + roomData.Area);
                roomDatas.Add(roomData);
            }
        }

        Debug.Log("Height: " + GetHeight(outerWalls));

        OnHouseRebuild?.Invoke(roomDatas.ToArray());
    }

    public bool IsTouching(Collider2D collider2d) { return compositeCollider.IsTouching(collider2d); }
    public RoomData[] GetRooms() { return roomDatas.ToArray(); }

    private float GetHeight(RoomData roomData)
    {
        float max = 0f;
        for (int i = 0; i < roomData.Points.Length; i++)
        {
            if (roomData.Points[i].y > max) max = roomData.Points[i].y;
        }
        return max;
    }
}

[System.Serializable]
public class RoomData
{
    public Vector2[] Points { get; private set; }
    public float Area { get; private set; }
    public Mesh Mesh { get; private set; }
    public string ID { get; private set; }

    public RoomData(Vector2[] points)
    {
        Points = points;
        Area = points.Area();
        Mesh = MeshGenerator.GenerateMesh(Points);
        ID = Area.ToString() + points.Length + points[0].x.ToString(); 
    }

}
