using System.Collections;
using UnityEngine;
using System;

public class CharacterController : MonoBehaviour
{
    protected Rigidbody characterRb;
    public GameObject ball;
    public GameObject hoop;
    public GameObject teamMate;

    protected BallController ballController;
    protected CharacterController teamMateController;

    public AudioClip jumpSound;
    public AudioClip bounceSound;
    protected AudioSource characterAudio;


    public float speed = 4.0f;
    public float jumpForce = 35.0f;

    protected float horizontalLimit = 19.0f;
    protected float verticalLimit = 5;
    protected float jumpLimit = 0.1f;

    protected bool hasBall = false;
    protected bool teamHasBall = false;
    public bool lookingRight = true;
    public bool defendent = false;
    public bool isGameActive;

    public Vector3 startPosition;
    public Vector3 startRotation;
    public int team;
    private Vector3 randPos;
    private System.Random rand;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        characterRb = GetComponent<Rigidbody>();
        rand = new System.Random(GetInstanceID());
        ballController = ball.GetComponent<BallController>();
        teamMateController = teamMate.GetComponent<CharacterController>();
        characterAudio = GetComponent<AudioSource>();

        isGameActive = false;
        InvokeRepeating("BallBounce", 0, 1);

        InvokeRepeating("RandomLoc", 0.0f, 5.0f);
    }

    protected virtual void Update()
    {
        if (isGameActive)
        {
            float distanceVecx = ball.transform.position.x - transform.position.x;
            float distanceVecz = ball.transform.position.z - transform.position.z;

            float distanceRadius = (float)Math.Sqrt(Math.Pow(distanceVecx, 2) + Math.Pow(distanceVecz, 2));

            teamHasBall = teamMateController.GetHasBall();

            if (!hasBall && distanceRadius < 5.0f && ball.transform.position.y < 1.0f && !ballController.getIsCarried())
            {
                StartCoroutine(ChaseBall(0.3f, 0.3f));
            }
            else if (hasBall)
            {
                Attack();
            }
            else if (teamHasBall)
            {
                Support();
            }
            else if (!hasBall & distanceRadius < 5.0f && ballController.getIsCarried() && defendent)
            {
                StartCoroutine(ChaseBall(1.0f, 1.0f));
            }
            else
            {
                Retreat();
            }
        }
    }


    /**
     * This method is reponsible of basic movement of the characters.
     *
     * @arg verticalInput: Input for the players movement on the vertical axis.
     * @arg horizontalInput: Input for the players movement on the horizontal axis.
     */

    public virtual void Stop()
    {
        isGameActive = false;
    }

    public virtual void Reset()
    {
        characterAudio.loop = true;
        characterAudio.clip = null;
        characterAudio.Stop();

        transform.position = startPosition;
        transform.rotation = Quaternion.Euler(startRotation);

        if (transform.rotation.y < 0)
        {
            lookingRight = false;
        } else
        {
            lookingRight = true;
        }

        isGameActive = true;
    }

    public virtual bool GetHasBall()
    {
        return hasBall;
    }

    protected virtual void BaseMovement()
    {
    }

    IEnumerator Idle()
    {
        yield return new WaitForSeconds(2);
        // Play Idle Animation or call a function that can do that.
    }

    IEnumerator Evade()
    {
        float randomNum = (float)rand.NextDouble();
        if (randomNum <= 0.45f && transform.position.z < 4.0f) transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0.0f, 0.0f, 2.0f), speed * Time.deltaTime);
        else if (randomNum > 0.45f && randomNum <= 0.9f && transform.position.z > -4.0f) transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0.0f, 0.0f, -2.0f), speed * Time.deltaTime);
        else Pass();

        yield return new WaitForSeconds(2);

    }

    private void RandomLoc()
    {
        float xRand =  + (team == 0 ? 0.0f : 8.0f) - (float)rand.NextDouble() * 8.0f;
        float zRand = (float)rand.NextDouble() * 8.0f - 4.0f;

        randPos = new Vector3(xRand, transform.position.y, zRand);
    }


    protected virtual void Attack()
    {
        float distanceVecx = hoop.transform.position.x - transform.position.x;
        float distanceVecz = hoop.transform.position.z - transform.position.z;

        float distanceRadius = (float)Math.Sqrt(Math.Pow(distanceVecx, 2) + Math.Pow(distanceVecz, 2));

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.0f);

        if (team == 0 && !lookingRight)
        {

            transform.RotateAround(transform.position, transform.up, 180.0f);
            lookingRight = true;
        }
        else if (team == 1 && lookingRight)
        {

            transform.RotateAround(transform.position, transform.up, 180.0f);
            lookingRight = false;
        }

        if (distanceRadius < 5.0f)
        {

            ShootBall();
            StopAllCoroutines();

            StartCoroutine(Idle());
        } else
        {
            int len = hitColliders.Length;
            for (int i = 0; i < len; i++)
            {
                string colliderName = hitColliders[i].gameObject.tag;

                if (team == 0 && colliderName == "Team1" || team == 1 && colliderName == "Team0")
                {
                    StartCoroutine("Evade");
                }
                
            }

            transform.position = Vector3.MoveTowards(transform.position, hoop.transform.position, speed * Time.deltaTime);
        }
    }


    protected virtual void Retreat()
    { 
        if (Math.Round(randPos.x, 1) < Math.Round(transform.position.x, 1) && lookingRight)
        {
            transform.RotateAround(transform.position, transform.up, 180.0f);
            lookingRight = false;
        } else if (Math.Round(randPos.x, 1) > Math.Round(transform.position.x, 1) && !lookingRight)
        {
            transform.RotateAround(transform.position, transform.up, 180.0f);
            lookingRight = true;
        }

        transform.position = Vector3.MoveTowards(transform.position, randPos, speed * Time.deltaTime);
    }


    protected virtual void Support()
    {
        Vector3 oppositeRandPos = randPos + (team == 0 ? new Vector3(8.0f, 0.0f, 0.0f) : new Vector3(-8.0f, 0.0f, 0.0f));
        float direction = (float)Math.Round(oppositeRandPos.x, 1);

        if (direction < (float)Math.Round(transform.position.x, 1) && lookingRight)
        {
            transform.RotateAround(transform.position, transform.up, 180.0f);
            lookingRight = false;
        }
        else if (direction > (float)Math.Round(transform.position.x, 1) && !lookingRight)
        {
            transform.RotateAround(transform.position, transform.up, 180.0f);
            lookingRight = true;
        }

        transform.position = Vector3.MoveTowards(transform.position, oppositeRandPos, speed * Time.deltaTime);
    }


    protected virtual void PickUp(Rigidbody obj)
    {
        obj.transform.position = characterRb.transform.position + (lookingRight ?  new Vector3(0.3f, 1.0f, 0.1f) : new Vector3(-0.3f, 1.0f, 0.1f));

    }

    IEnumerator ChaseBall(float distanceX, float distanceZ)
    {

        float distanceVecx = Math.Round(ball.transform.position.x - transform.position.x, 1) == 0.0f ? 0.0f : ball.transform.position.x - transform.position.x > 0.0f ? 1.0f : -1.0f;
        float distanceVecz = Math.Round(ball.transform.position.z - transform.position.z, 1) == 0.0f ? 0.0f : ball.transform.position.z - transform.position.z > 0.0f ? 1.0f : -1.0f;

        if (distanceVecx < 0 && lookingRight)
        {
            transform.RotateAround(transform.position, transform.up, 180.0f);
            lookingRight = false;
        }
        else if (distanceVecx > 0 && !lookingRight)
        {
            transform.RotateAround(transform.position, transform.up, 180.0f);
            lookingRight = true;
        }

        //Vector3 distanceVec3 = Vector3.right * distanceVecx + Vector3.forward * distanceVecz;
        Vector3 distanceVec3 = new Vector3(distanceVecx, 0, distanceVecz);
        //transform.position = Vector3.MoveTowards(transform.position, distanceVec3, speed * Time.deltaTime);

        if (Math.Abs(transform.position.x - ball.transform.position.x) > distanceX || Math.Abs(transform.position.z - ball.transform.position.z) > distanceZ)
        {
            transform.Translate(distanceVec3 * Time.deltaTime * speed, Space.World);
        }

        yield return new WaitForSeconds(0.1f);

    }

    //protected void ChaseBall()
    //{
    //    float distanceVecx = Math.Round(ball.transform.position.x - transform.position.x, 1) == 0.0f ? 0.0f : ball.transform.position.x - transform.position.x > 0.0f ? 1.0f : -1.0f;
    //    float distanceVecz = Math.Round(ball.transform.position.z - transform.position.z, 1) == 0.0f ? 0.0f : ball.transform.position.z - transform.position.z > 0.0f ? 1.0f : -1.0f;

    //    if (distanceVecx < 0 && lookingRight)
    //    {
    //        transform.RotateAround(transform.position, transform.up, 180.0f);
    //        lookingRight = false;
    //    } else if (distanceVecx > 0 && !lookingRight)
    //    {
    //        transform.RotateAround(transform.position, transform.up, 180.0f);
    //        lookingRight = true;
    //    }

    //        //Vector3 distanceVec3 = Vector3.right * distanceVecx + Vector3.forward * distanceVecz;
    //    Vector3 distanceVec3 = new Vector3(distanceVecx, 0, distanceVecz);
    //    transform.Translate(distanceVec3 * Time.deltaTime * 3, Space.World);

    //}


    protected void Pass()
    {
        characterAudio.loop = true;
        characterAudio.clip = null;
        characterAudio.Stop();

        //Vector3 distanceVec3 = teamMate.transform.position - transform.position;
        float directionForward = teamMate.transform.position.x > transform.position.x ? 1.0f: -1.0f;
        float directionUpward = teamMate.transform.position.z > transform.position.z ? 1.0f : -1.0f;

        float distanceVecx = Math.Abs(teamMate.transform.position.x - transform.position.x) < 8.0f ? (teamMate.transform.position.x - transform.position.x) : 8.0f;
        float distanceVecz = Math.Abs(teamMate.transform.position.z - transform.position.z) < 4.0f ? (teamMate.transform.position.z - transform.position.z) : 4.0f;

        Rigidbody ballRb = ball.GetComponent<Rigidbody>();
        BallController ballController = ball.GetComponent<BallController>();


        if (directionForward == 1.0f)
        {
            if (!lookingRight)
            {
                transform.RotateAround(transform.position, transform.up, 180f);
                lookingRight = true;
            }

        } else if (directionForward == -1.0f)
        {
            if (lookingRight)
            {
                transform.RotateAround(transform.position, transform.up, 180f);
                lookingRight = false;
            }
        }

        ball.transform.position = characterRb.transform.position + (directionForward == 1.0f ? new Vector3(0.55f, 1.2f, 0.01f) : new Vector3(-0.55f, 1.2f, 0.01f));

        ball.transform.parent = null;
        ballController.EnableRagdoll();

        Vector3 forceVec = new Vector3(directionForward * distanceVecx, 2.0f, directionUpward * distanceVecz) * 1.5f;
        ballRb.AddForce(forceVec,ForceMode.Impulse);
        hasBall = false;
        ballController.setIsCarried(false);
    }


    protected virtual void ShootBall()
    {
        characterAudio.loop = true;
        characterAudio.clip = null;
        characterAudio.Stop();

        characterRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        ball.transform.position = characterRb.transform.position + (lookingRight ? new Vector3(0.45f, 1.8f, 0.01f) : new Vector3(-0.45f, 1.8f, 0.01f));

        if (transform.position.y >= jumpLimit)
        {
            characterAudio.PlayOneShot(jumpSound, 0.5f);

            Rigidbody ballRb = ball.GetComponent<Rigidbody>();
            //BallController ballController = ball.GetComponent<BallController>();

            Vector3 distanceVec3 = hoop.transform.position - ball.transform.position;
            ball.transform.parent = null;
            ballController.EnableRagdoll();
            //ballRb.AddForce((Vector3.up * 8 + Vector3.right * 5) * transform.position.y, ForceMode.Impulse);
            ballRb.AddForce((distanceVec3 + Vector3.up * 6), ForceMode.Impulse);
            hasBall = false;
            ballController.setIsCarried(false);
        }
        
    }

    IEnumerator BallBounce()
    {
        characterAudio.loop = true;
        characterAudio.clip = bounceSound;
        characterAudio.volume = 0.1f;
        characterAudio.Play();
        yield return null;
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball") && transform.position.y < 0.1)
        {
            Rigidbody ballRb = ball.GetComponent<Rigidbody>();
            BallController ballController = ball.GetComponent<BallController>();

            if (!ballController.getIsCarried())
            {
                print("1");
                PickUp(ballRb);
                ball.transform.SetParent(gameObject.transform);

                ballController.DisableRagdoll();
                hasBall = true;
                ballController.setIsCarried(true);
                StartCoroutine("BallBounce");
            }
        }
    }

}
