// Main home does not have this script attached
using System.Collections.Generic;
using UnityEngine;

public class WallPlacer : MonoBehaviour
{
    public float rotationSpeed = 200f;

    List<GameObject> attachablesInRange = new List<GameObject>();
    List<GameObject> restictedAreasInRange = new List<GameObject>();

    WallInteractionMode interactionMode = WallInteractionMode.Targetable;
    Vector3 mouseOffset;


    void LateUpdate()
    {
        ProcessRotation();
    }

    void ProcessRotation()
    {
        if (interactionMode == WallInteractionMode.Draggable)
        {
            if (Input.GetKey(KeyCode.Q))
                transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime); // Rotate left
            else if (Input.GetKey(KeyCode.E))
                transform.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime); // Rotate right
        }
    }

    public void OnMouseDown()
    {
        if (interactionMode == WallInteractionMode.Targetable)
        {
            interactionMode = WallInteractionMode.Draggable;
            mouseOffset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
        }
    }

    public void OnMouseDrag()
    {
        if (interactionMode == WallInteractionMode.Draggable)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + mouseOffset;
        }
    }

    public void OnMouseUp()
    {
        if (interactionMode == WallInteractionMode.Draggable)
        {
            interactionMode = WallInteractionMode.NotTargetable;
            
            if (CanBePlacedHere()) GetComponent<Wall>()?.PlaceWall(attachablesInRange);
            else GetComponent<Wall>()?.DropWall();
        }
    }



    bool CanBePlacedHere()
    {
        return (attachablesInRange.Count > 0) && restictedAreasInRange.Count == 0;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("AttachableArea") && !attachablesInRange.Contains(collision.gameObject)) attachablesInRange.Add(collision.gameObject);
        if (collision.CompareTag("NotAttachableArea") && !restictedAreasInRange.Contains(collision.gameObject)) restictedAreasInRange.Add(collision.gameObject);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("AttachableArea") && attachablesInRange.Contains(collision.gameObject)) attachablesInRange.Remove(collision.gameObject);
        if (collision.CompareTag("NotAttachableArea") && restictedAreasInRange.Contains(collision.gameObject)) restictedAreasInRange.Remove(collision.gameObject);
    }

    enum WallInteractionMode
    {
        Targetable,
        Draggable,
        NotTargetable,
    }
}
