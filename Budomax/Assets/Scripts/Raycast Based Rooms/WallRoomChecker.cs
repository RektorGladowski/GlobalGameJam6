using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRoomChecker : MonoBehaviour
{
    [Header("Room checking settings")]
    public int raycastChecks = 18;
    public ContactFilter2D contactFilter;

    Vector2 roomCheckPoint = Vector2.zero;
    List<Collider2D> detectedWalls = new List<Collider2D>();

    Vector2[] debugRays;



    public void CheckForPossibleRooms ()
    {
        Debug.Log("Checking For possible rooms");
        Collider2D wallCollider = GetComponent<Collider2D>();
        
        RaycastHit2D[] upResults = new RaycastHit2D[1];
        RaycastHit2D[] downResults = new RaycastHit2D[1];
        wallCollider.Raycast(transform.right, contactFilter, upResults);
        wallCollider.Raycast(-transform.right, contactFilter, downResults);

        if (upResults[0].collider != null)
        {
            roomCheckPoint = (Vector2)transform.position + (upResults[0].point - (Vector2)transform.position) / 2f;
            CheckForRoomAtPoint(roomCheckPoint);
        }
        if (downResults[0].collider != null)
        {
            roomCheckPoint = (Vector2)transform.position + (downResults[0].point - (Vector2)transform.position) / 2f;
            CheckForRoomAtPoint(roomCheckPoint);
        }
    }

    void CheckForRoomAtPoint(Vector3 point)
    {
        Debug.Log("Another wall has been detected");
        float additionAngle = 2 * Mathf.PI / raycastChecks;
        RaycastHit2D recentHit;
        debugRays = new Vector2[raycastChecks];

        for (int i = 0; i < raycastChecks; i++)
        {
            debugRays[i] = Rotate2DVector(Vector3.right, i * additionAngle);
            recentHit = Physics2D.Raycast(roomCheckPoint, debugRays[i], 100f, contactFilter.layerMask);
            if (recentHit.collider != null)
            {
                if (!detectedWalls.Contains(recentHit.collider)) detectedWalls.Add(recentHit.collider);
            }
            else
            {
                return;
            }
        }

        Debug.Log("Possible room was detected. Checking...");

        for (int wallID = 0; wallID < detectedWalls.Count - 1; wallID++)
        {
            if (!detectedWalls[wallID].GetComponent<Wall>().neighbours.Contains(detectedWalls[wallID + 1].GetComponent<Wall>())) return;
        }

        if (detectedWalls[detectedWalls.Count - 1].GetComponent<Wall>().neighbours.Contains(detectedWalls[0].GetComponent<Wall>()))
        {
            Debug.Log("Real room has been detected");
        }
    }



    // DEBUG
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        for (int i = 0; i < debugRays.Length; i++)
            Gizmos.DrawLine(roomCheckPoint, roomCheckPoint + (Vector2)(debugRays[i] * 20f));

        Gizmos.color = Color.cyan;
        for (int i = 0; i < detectedWalls.Count; i++)
            Gizmos.DrawCube(detectedWalls[i].transform.position, Vector3.one);
    }

    Vector2 Rotate2DVector(Vector2 point, float radians)
    {
        float sinT = Mathf.Sin(radians); float cosT = Mathf.Cos(radians);
        return new Vector2(point.x * cosT - point.y * sinT, point.x * sinT + point.y * cosT);
    }
}
