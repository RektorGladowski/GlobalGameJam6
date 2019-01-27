using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Cam settings")]
    public float borderParameter = 0.2f;
    public float baseCameraFlexibility = 5f;
    public float lerpSpeed = 10f;

    float bottomScreenBorder, topScreenBorder, borderHeight;
    float currentTargetHeight;

    float cameraBottomConstraint;
    float cameraTopConstraint;

    Vector3 currentTargetPosition;

    

    void Awake()
    {
        // Set positions
        currentTargetPosition = transform.position;

        // Set height and constraints
        currentTargetHeight = currentTargetPosition.y;
        cameraBottomConstraint = currentTargetHeight - 4f;
        cameraTopConstraint = currentTargetHeight + 1f;
        
        // Set borders
        borderHeight = borderParameter * Screen.height;
        bottomScreenBorder = borderHeight;
        topScreenBorder = Screen.height - borderHeight;
    }

    void Update()
    {
        CheckForMouseInput();
        if (transform.position != currentTargetPosition) LerpCameraMovement();
    }

    void CheckForMouseInput()
    {
        Vector3 mousePos = Input.mousePosition;

        if (mousePos.y >= topScreenBorder) currentTargetHeight += (mousePos.y - topScreenBorder) / borderHeight * baseCameraFlexibility * Time.deltaTime;
        else if (mousePos.y <= bottomScreenBorder) currentTargetHeight -= (bottomScreenBorder - mousePos.y) / borderHeight * baseCameraFlexibility * Time.deltaTime;

        currentTargetHeight = Mathf.Clamp(currentTargetHeight, cameraBottomConstraint, cameraTopConstraint);
        currentTargetPosition = new Vector3(currentTargetPosition.x, currentTargetHeight, currentTargetPosition.z);
    }

    void LerpCameraMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, lerpSpeed * Time.deltaTime);
    }

    public void UpdateTopConstraint (float constraint)
    {
        if (constraint < cameraBottomConstraint) cameraBottomConstraint = constraint;
        cameraTopConstraint = constraint;        
    }
}
