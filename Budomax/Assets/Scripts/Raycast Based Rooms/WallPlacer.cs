﻿// Main home does not have this script attached
using System.Collections.Generic;
using UnityEngine;

public class WallPlacer : MonoBehaviour
{
    public float rotationSpeed = 200f;

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
            if (InputManager.GetRotateLeftAction())
                transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime); // Rotate left
            else if (InputManager.GetRotateRightAction())
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
            IAttachable attachable = GetComponent<IAttachable>();
            attachable?.TryAttaching();
        }
    }

    enum WallInteractionMode
    {
        Targetable,
        Draggable,
        NotTargetable,
    }
}
