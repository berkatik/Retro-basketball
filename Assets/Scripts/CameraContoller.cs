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
        float x = Ball.transform.position.x;

        if (x < 8 && x > -8)
        {
            if (height >= 0.75f && height + Offset.y <= 5.0f) transform.position = Vector3.Lerp(transform.position, new Vector3(Ball.transform.position.x, height + Offset.y, Offset.z), Time.deltaTime * moveSpeed);
            else transform.position = Vector3.Lerp(transform.position, new Vector3(Ball.transform.position.x, Offset.y, Offset.z), Time.deltaTime * moveSpeed);
        }
        else if (height >= 0.75f && height + Offset.y <= 5.0f) transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, height + Offset.y, Offset.z), Time.deltaTime * moveSpeed);
    }
}
