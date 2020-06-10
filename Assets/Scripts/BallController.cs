using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{

    private Rigidbody BallRb;
    public bool isCarried = false;
    // Start is called before the first frame update
    void Start()
    {
        BallRb = GetComponent<Rigidbody>();
        BallRb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void Stop()
    {
        BallRb.isKinematic = true;
    }

    public void Reset()
    {
        transform.parent = null;
        transform.position = new Vector3(0.0f, 2.0f, 0.0f);
        EnableRagdoll();
    }

    public bool getIsCarried()
    {
        return isCarried;
    }

    public void setIsCarried(bool val)
    {
        isCarried = val;
    }

    public void EnableRagdoll()
    {
        BallRb.isKinematic = false;
        BallRb.detectCollisions = true;
        isCarried = false;

    }

    public void DisableRagdoll()
    {
        BallRb.isKinematic = true;
        BallRb.detectCollisions = false;
        isCarried = true;
    }
}
