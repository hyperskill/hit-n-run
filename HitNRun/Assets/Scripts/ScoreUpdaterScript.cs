using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdaterScript : MonoBehaviour
{
    private float highScore;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Text>().text = "0";

        if (PlayerPrefs.HasKey("highScore"))
        {
            highScore = PlayerPrefs.GetFloat("highScore");
        }
        else
        {
            highScore = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float score = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerScript>().score;
        this.GetComponent<Text>().text = ((int)score).ToString();

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetFloat("highScore", highScore);
        }
    }
}
