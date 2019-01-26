using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : MonoBehaviour
{
    public float rotationSpeed = 200f;
    public float gravityScale = 2f;

    WallInteractionMode interactionMode;
    Rigidbody2D wallRb;
    Vector3 mouseOffset;


    void Awake()
    {
        wallRb = GetComponent<Rigidbody2D>();
        wallRb.bodyType = RigidbodyType2D.Kinematic;
        wallRb.gravityScale = gravityScale;

        interactionMode = WallInteractionMode.Targetable;
    }

    void LateUpdate()
    {
        ProcessRotation();   
    }



    public void OnMouseDown()
    {
        if (interactionMode == WallInteractionMode.Targetable)
        {
            interactionMode = WallInteractionMode.Draggable;
            mouseOffset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
        }
    }

    public void OnMouseUp ()
    {
        if (interactionMode == WallInteractionMode.Draggable)
        {
            interactionMode = WallInteractionMode.NotTargetable;

            wallRb.bodyType = RigidbodyType2D.Dynamic;
        }       
    }

    public void OnMouseDrag ()
    {
        if (interactionMode == WallInteractionMode.Draggable)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + mouseOffset;
        }     
    }


    void ProcessRotation ()
    {
        if (interactionMode == WallInteractionMode.Draggable)
        {
            if (Input.GetKey(KeyCode.Q))
                transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime); // Rotate left
            else if (Input.GetKey(KeyCode.E))
                transform.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime); // Rotate right
        }
    }

    enum WallInteractionMode
    {
        Targetable,
        Draggable,
        NotTargetable,
    }
}
