using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float highScore = PlayerPrefs.HasKey("highScore") ? PlayerPrefs.GetFloat("highScore") : 0f;
        this.GetComponent<Text>().text = $"{(int)highScore}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
