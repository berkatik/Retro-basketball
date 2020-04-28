using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;

    public float speed = 5.0f;
    public float jumpForce = 20.0f;

    private float horizontalLimit = 19.0f;
    private float verticalLimit = 5;
    private float jumpLimit = 0.7f;

    public bool hasBall;
   

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        hasBall = false;
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        float rightInput = Input.GetAxis("Horizontal");

        BaseMovement(forwardInput, rightInput);        
    }

    /**
     * This method is reponsible of basic movement of the characters.
     *
     * @arg verticalInput: Input for the players movement on the vertical axis.
     * @arg horizontalInput: Input for the players movement on the horizontal axis.
     */
    void BaseMovement(float verticalInput, float horizontalInput)
    {

        // Cancel horizontal and verital movement while jumping.
        if (transform.position.y < 0.1f)
        {
            transform.Translate(Vector3.left * verticalInput * Time.deltaTime * speed);
            transform.Translate(Vector3.forward * horizontalInput * Time.deltaTime * speed);
        }



        // Prevent player from going off screen.
        if (transform.position.z > verticalLimit)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, verticalLimit);
        }
        else if (transform.position.z < -verticalLimit)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -verticalLimit);
        }

        if (transform.position.x > horizontalLimit)
        {
            transform.position = new Vector3(horizontalLimit, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -horizontalLimit)
        {
            transform.position = new Vector3(-horizontalLimit, transform.position.y, transform.position.z);
        }


        // Jumping, limited by y plane position.
        if (Input.GetKey(KeyCode.Space) && transform.position.y < jumpLimit)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            if (hasBall)
            {
                GameObject ball = transform.GetChild(transform.childCount - 1).gameObject;
                Rigidbody ballRb = ball.GetComponent<Rigidbody>();

                ballRb.position = playerRb.transform.position + new Vector3(0.55f, 1.2f, 0.01f);
            }
        }

        if (transform.position.y >= jumpLimit && hasBall)
        {
            shootBall();
            //hasBall = false;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            GameObject ball = collision.gameObject;
            Rigidbody ballRb = ball.GetComponent<Rigidbody>();
            BallController ballController = ball.GetComponent<BallController>();

            pickUp(ballRb);
            ball.transform.SetParent(gameObject.transform);

            hasBall = true;
            ballController.DisableRagdoll();

        }
    }


    private void pickUp(Rigidbody obj)
    {
        obj.transform.position = playerRb.transform.position + new Vector3(0.36f, 0.55f, -0.38f);
    }


    private void shootBall()
    {
        if (hasBall)
        {
            GameObject ball = transform.GetChild(transform.childCount - 1).gameObject;
            Rigidbody ballRb = ball.GetComponent<Rigidbody>();
            BallController ballController = ball.GetComponent<BallController>();

            ball.transform.parent = null;
            ballController.EnableRagdoll();

            ballRb.AddForce((Vector3.up * 12 + Vector3.right * 8)  * transform.position.y, ForceMode.Impulse);

            hasBall = false;
        }
    }

}
