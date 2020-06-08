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
    }

    // Update is called once per frame
    void Update()
    {
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
