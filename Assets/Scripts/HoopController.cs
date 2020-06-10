using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopController : MonoBehaviour
{
    public GameObject ball;
    public GameObject gameManager;

    public AudioClip scoreSound;
    protected AudioSource hoopAudio;


    private Vector3 center;
    private Vector3 ballCenter;

    private float radius = 0.4f;
    public int side;

    public bool scored;

    // Start is called before the first frame update
    void Start()
    {
        hoopAudio = GetComponent<AudioSource>();

        center = transform.position;
        scored = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!scored)
        {
            ballCenter = ball.GetComponent<Rigidbody>().transform.position;
            if (Vector3.Distance(ballCenter, center) < radius && System.Math.Round(ballCenter.y, 1) < center.y)
            {
                hoopAudio.PlayOneShot(scoreSound, 1f);
                gameManager.SendMessage("UpdateScore", side);
                scored = true;
                StartCoroutine("Reset");
            }
        }
    }


    IEnumerator Reset()
    {
        yield return new WaitForSeconds(2);

        scored = false;
    }
}
