using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{

    private Rigidbody rb;
    public bool isCarried = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void EnableRagdoll()
    {
        rb.isKinematic = false;
        rb.detectCollisions = true;
        isCarried = false;

}

public void DisableRagdoll()
    {
        rb.isKinematic = true;
        rb.detectCollisions = false;
        isCarried = true;
    }
}
