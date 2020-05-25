using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    protected Rigidbody characterRb;

    public float speed = 5.0f;
    public float jumpForce = 40.0f;

    protected float horizontalLimit = 19.0f;
    protected float verticalLimit = 5;
    protected float jumpLimit = 0.7f;

    public bool hasBall;
    public bool lookingRight = true;

    public GameObject hoop;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }


    /**
     * This method is reponsible of basic movement of the characters.
     *
     * @arg verticalInput: Input for the players movement on the vertical axis.
     * @arg horizontalInput: Input for the players movement on the horizontal axis.
     */
    protected virtual void BaseMovement()
    {

    }


    protected virtual void PickUp(Rigidbody obj)
    {
        obj.transform.position = characterRb.transform.position + new Vector3(0.36f, 0.55f, -0.38f);
    }


    protected void ShootBall()
    {
        print("1");
        GameObject ball = transform.GetChild(transform.childCount - 1).gameObject;
        Rigidbody ballRb = ball.GetComponent<Rigidbody>();
        BallController ballController = ball.GetComponent<BallController>();

        Vector3 distanceVec3 = hoop.transform.position - ballRb.transform.position;
        ball.transform.parent = null;
        ballController.EnableRagdoll();
        //ballRb.AddForce((Vector3.up * 8 + Vector3.right * 5) * transform.position.y, ForceMode.Impulse);
        ballRb.AddForce((distanceVec3 + Vector3.up * 6), ForceMode.Impulse);
        hasBall = false;


    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball") && transform.position.y < 0.1)
        {
            GameObject ball = collision.gameObject;
            Rigidbody ballRb = ball.GetComponent<Rigidbody>();
            BallController ballController = ball.GetComponent<BallController>();

            PickUp(ballRb);
            ball.transform.SetParent(gameObject.transform);

            hasBall = true;
            ballController.DisableRagdoll();

        }
    }

}
