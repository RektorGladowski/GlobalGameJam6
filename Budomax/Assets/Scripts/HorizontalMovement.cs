using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovement : MonoBehaviour
{
    public float speed;
    public float range;
    public bool bounce;
    public bool startLeft;
    private Vector3 direction;
    private Vector3 startPos;
    private float distance;
    // Start is called before the first frame update
    void Start()
    {
        if (startLeft) direction = new Vector3(-speed, 0, 0);
        else direction = new Vector3(speed, 0, 0);



    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Vector3.Distance(startPos, transform.position).ToString());
        transform.Translate(direction);
        if (bounce)
        {
            distance += speed;
            if ( distance >= range)
            {
                direction = direction *-1;
                distance = 0;
                transform.localScale *= -1;          
            }
               
        }

    }
}
