using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menu_manager : MonoBehaviour
{
    // Start is called before the first frame update
    private Text highScoreText;
    void Start()
    {
        highScoreText = GameObject.Find("HighScore").GetComponent<Text>();
        highScoreText.text = PlayerPrefs.GetInt("highscore").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
