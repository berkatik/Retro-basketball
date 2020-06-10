using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{

    protected override void Start()
    {
        characterRb = GetComponent<Rigidbody>();
        characterAudio = GetComponent<AudioSource>();
    }

    protected override void Update()
    {
        if (isGameActive)
        {
            float forwardInput = Input.GetAxis("Vertical");
            float rightInput = Input.GetAxis("Horizontal");

            BaseMovement(forwardInput, rightInput);

            Vector3 rotationVector = transform.rotation.eulerAngles;
            rotationVector.x = 0;
            rotationVector.z = 0;
            transform.rotation = Quaternion.Euler(rotationVector);
        }
    }

    void BaseMovement(float verticalInput, float horizontalInput)
    {

        // Cancel horizontal and verital movement while jumping.
        if (transform.position.y < 0.1f)
        {
            if (horizontalInput < 0 && lookingRight || horizontalInput > 0 && !lookingRight)
            {
                transform.RotateAround(transform.position, transform.up, 180f);
                lookingRight = !lookingRight;
            }


            if (lookingRight)
            {
                transform.Translate(Vector3.forward * horizontalInput * Time.deltaTime * speed);
                transform.Translate(Vector3.left * verticalInput * Time.deltaTime * speed);

            }
            else if (!lookingRight)
            {
                transform.Translate(Vector3.back * horizontalInput * Time.deltaTime * speed);
                transform.Translate(Vector3.right * verticalInput * Time.deltaTime * speed);
            }
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
            characterRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            if (hasBall)
            {
                if (!lookingRight)
                {
                    transform.RotateAround(transform.position, transform.up, 180f);
                    lookingRight = true;
                }


                ball.transform.position = characterRb.transform.position + new Vector3(0.55f, 2.0f, 0.01f);
            }

        }

        if (Input.GetKey(KeyCode.X) && hasBall)
        {
            Pass();
        }
        

        if (Input.GetKeyUp(KeyCode.Space) && hasBall)
        {

            ShootBall();
            //hasBall = false;
        }
    }

    protected override void ShootBall()
    {
        characterAudio.loop = true;
        characterAudio.clip = null;
        characterAudio.Stop();

        characterAudio.PlayOneShot(jumpSound, 0.5f);

        Rigidbody ballRb = ball.GetComponent<Rigidbody>();
        BallController ballController = ball.GetComponent<BallController>();

        Vector3 distanceVec3 = hoop.transform.position - ballRb.transform.position;
        ball.transform.parent = null;
        ballController.EnableRagdoll();
        //ballRb.AddForce((Vector3.up * 8 + Vector3.right * 5) * transform.position.y, ForceMode.Impulse);
        ballRb.AddForce((distanceVec3 + Vector3.up * 6), ForceMode.Impulse);
        hasBall = false;
        ballController.setIsCarried(false);
    }

}
