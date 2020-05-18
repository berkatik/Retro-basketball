using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContoller : MonoBehaviour
{
    public GameObject Ball;
    public Vector3 Offset;
    public float moveSpeed = 2.0f;

    void Update()
    {
        float height = Ball.transform.position.y;


        if (height >= 0.55f)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(Ball.transform.position.x, height + Offset.y, Offset.z), Time.deltaTime * moveSpeed);
        } else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(Ball.transform.position.x, Offset.y, Offset.z), Time.deltaTime * moveSpeed);
        }
        
    }
}
