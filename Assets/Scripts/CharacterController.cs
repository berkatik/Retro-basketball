using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterController : MonoBehaviour
{
    protected Rigidbody characterRb;
    public GameObject ball;
    public GameObject hoop;
    public GameObject teamMate; // Temporary

    public float speed = 4.0f;
    public float jumpForce = 35.0f;

    protected float horizontalLimit = 19.0f;
    protected float verticalLimit = 5;
    protected float jumpLimit = 0.1f;

    protected bool hasBall = false;
    public bool lookingRight = true;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        characterRb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        float distanceVecx = ball.transform.position.x - transform.position.x;
        float distanceVecz = ball.transform.position.z - transform.position.z;

        float distanceRadius = (float) Math.Sqrt(Math.Pow(distanceVecx, 2) + Math.Pow(distanceVecz, 2));

        if (!hasBall && distanceRadius < 10.0f && ball.transform.position.y < 1f) 
        {
            ChaseBall();
        } else if (hasBall)
        {
            Attack();
        }
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

    IEnumerator Idle()
    {
        yield return new WaitForSeconds(2);
        // Play Idle Animation or call a function that can do that.
    }


    protected virtual void Attack()
    {
        float distanceVecx = hoop.transform.position.x - transform.position.x;
        float distanceVecz = hoop.transform.position.z - transform.position.z;

        float distanceRadius = (float)Math.Sqrt(Math.Pow(distanceVecx, 2) + Math.Pow(distanceVecz, 2));

        if (distanceRadius < 5.0f)
        {
            ShootBall();
            StopAllCoroutines();

            StartCoroutine(Idle());
        }
    }


    protected virtual void PickUp(Rigidbody obj)
    {
        obj.transform.position = characterRb.transform.position + (lookingRight ?  new Vector3(0.3f, 1.0f, 0.1f) : new Vector3(-0.3f, 1.0f, 0.1f));

    }

    protected void ChaseBall()
    {
        float distanceVecx = Math.Round(ball.transform.position.x - transform.position.x, 1) == 0.0f ? 0.0f : ball.transform.position.x - transform.position.x > 0.0f ? 1.0f : -1.0f;
        float distanceVecz = Math.Round(ball.transform.position.z - transform.position.z, 1) == 0.0f ? 0.0f : ball.transform.position.z - transform.position.z > 0.0f ? 1.0f : -1.0f;

        if (distanceVecx < 0 && lookingRight)
        {
            transform.RotateAround(transform.position, transform.up, 180.0f);
            lookingRight = !lookingRight;
        }

            //Vector3 distanceVec3 = Vector3.right * distanceVecx + Vector3.forward * distanceVecz;
            Vector3 distanceVec3 = new Vector3(distanceVecx, 0, distanceVecz);
        print(distanceVec3);

        transform.Translate(distanceVec3 * Time.deltaTime * 3, Space.World);

    }


    protected void Pass()
    {
        //Vector3 distanceVec3 = teamMate.transform.position - transform.position;
        float distanceVecx = Math.Abs(teamMate.transform.position.x - transform.position.x) < 10.0f ? (teamMate.transform.position.x - transform.position.x) : 10.0f;
        float distanceVecz = Math.Abs(teamMate.transform.position.z - transform.position.z) < 10.0f ? (teamMate.transform.position.z - transform.position.z) : 10.0f;

        Rigidbody ballRb = ball.GetComponent<Rigidbody>();
        BallController ballController = ball.GetComponent<BallController>();



        if (distanceVecx >= 0)
        {
            if (!lookingRight)
            {
                transform.RotateAround(transform.position, transform.up, 180f);
                lookingRight = true;
            }

            ballRb.position = characterRb.transform.position + new Vector3(0.55f, 1.0f, 0.01f);
        } else
        {
            if (lookingRight)
            {
                transform.RotateAround(transform.position, transform.up, 180f);
                lookingRight = false;
            }
            ballRb.position = characterRb.transform.position + new Vector3(-0.55f, 1.0f, 0.01f);
        }

        ball.transform.parent = null;
        ballController.EnableRagdoll();



        ballRb.AddForce(new Vector3(distanceVecx, 0, distanceVecz) * 2, ForceMode.Impulse);
        hasBall = false;

    }


    protected void ShootBall()
    {
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
            Rigidbody ballRb = ball.GetComponent<Rigidbody>();
            BallController ballController = ball.GetComponent<BallController>();

            PickUp(ballRb);
            ball.transform.SetParent(gameObject.transform);

            hasBall = true;
            ballController.DisableRagdoll();

        }
    }

}
