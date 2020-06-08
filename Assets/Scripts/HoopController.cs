using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopController : MonoBehaviour
{
    public GameObject ball;
    private float radius = 0.4f;
    private Vector3 center;
    private Vector3 ballCenter;
    // Start is called before the first frame update
    void Start()
    {
        center = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ballCenter = ball.GetComponent<Rigidbody>().transform.position;
        if (Vector3.Distance(ballCenter, center) < radius && System.Math.Round(ballCenter.y, 1) == center.y)
        {
            print("Scored!");
        }
    }
}
