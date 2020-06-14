using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int[] score;
    public TextMeshProUGUI scoreText;
    public Button startButton;

    public TextMeshProUGUI restartText;
    public Button restartButton;

    public GameObject[] characterList;
    public GameObject ball;
    public GameObject startScreen;
    public GameObject restartScreen;



    // Start is called before the first frame update
    void Start()
    {
        score = new int[] { 0, 0 };
        scoreText.text = score[0] + " - " + score[1];
    }

    // Update is called once per frame
    void Update()
    {
        startButton.onClick.AddListener(StartGame);

        if (Input.GetKeyDown("return"))
        {
            StartGame();
        }
        
        restartButton.onClick.AddListener(RestartGame);

        if (score[0] >= 10)
        {
            Stop();
            restartText.text = "You Won!";
            restartScreen.SetActive(true);
        } else if (score[1] >= 10)
        {
            Stop();
            restartText.text = "You Lose!";
            restartScreen.SetActive(true);
        }

    }

    public void StartGame()
    {
        ball.SendMessage("Reset");

        for (int i = 0; i < characterList.Length; i++)
        {
            characterList[i].SendMessage("Reset");
        }
        startScreen.SetActive(false);
        restartScreen.SetActive(false);
    }

    public void RestartGame()
    {
        Start();
        StartGame();
    }

    IEnumerator Hold()
    {
        ball.SendMessage("Stop");

        yield return new WaitForSeconds(1);              
        StartGame();
    }

    void Stop()
    {
        ball.SendMessage("Stop");

        for (int i = 0; i < characterList.Length; i++)
        {
            characterList[i].SendMessage("Stop");
        }
    }

    IEnumerator UpdateScore(int side)
    {
        score[side] += 2;
        scoreText.text = score[0] + " - " + score[1];
        yield return new WaitForSeconds(1f);
    
        StartCoroutine("Hold");
    }
}
