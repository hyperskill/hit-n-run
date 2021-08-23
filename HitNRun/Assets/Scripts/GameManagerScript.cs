using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManagerScript : MonoBehaviour
{
    public GameObject myPrefab;
    public GameObject enemyPrefab;
    private float difficultyTracker = 1.0f;
    public float score = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemyGenerator());
        StartCoroutine(DifficultyIncrementer());
        
        const int numBlobs = 35;
        
        for (int i = 0; i < numBlobs; i++)
        {
            GameObject newBlob = Instantiate(myPrefab);
            float radius = Random.Range(1f, 2f);
            newBlob.transform.localScale = new Vector3(radius * 2, radius * 2, 0);
            newBlob.transform.position = new Vector2(Random.Range(-23f, 17f), Random.Range(-8f, 13f));
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Game");
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            SceneManager.LoadScene("Main Menu");
        }
        else if (Input.GetKeyDown(KeyCode.F1))
        {
            PlayerPrefs.DeleteKey("highScore");
        }
    }
    
    IEnumerator EnemyGenerator()
    {
        while (true)
        {
            GameObject newEnemy = Instantiate(enemyPrefab);
            
            //Up/down
            var upOrDown = Random.Range(0, 2);
            float verticalPos = 0f;

            if (upOrDown == 1) //up
            {
                verticalPos =
                    Camera.main
                        .ViewportToWorldPoint(new Vector3(0, 1.1f, 0))
                        .y;
            }
            else //Down
            {
                verticalPos =
                    Camera.main
                        .ViewportToWorldPoint(new Vector3(0, -0.1f, 0))
                        .y;
            }
            
            //Out of bounds check
            if (verticalPos > 13f)
            {
                verticalPos =
                    Camera.main
                        .ViewportToWorldPoint(new Vector3(0, -0.1f, 0))
                        .y;
            }
            else if (verticalPos < -8f)
            {
                verticalPos =
                    Camera.main
                        .ViewportToWorldPoint(new Vector3(0, 1.1f, 0))
                        .y;
            }
            
            //Left/Right
            var leftOrRight = Random.Range(0, 2);
            float horPos = 0f;

            if (leftOrRight == 1) //right
            {
                horPos =
                    Camera.main
                        .ViewportToWorldPoint(new Vector3(1.1f, 0, 0))
                        .x;
            }
            else //Left
            {
                horPos =
                    Camera.main
                        .ViewportToWorldPoint(new Vector3(-0.1f, 0, 0))
                        .x;
            }
            
            //Out of bounds check
            if (horPos > 13f)
            {
                horPos =
                    Camera.main
                        .ViewportToWorldPoint(new Vector3(-0.1f, 0, 0))
                        .x;
            }
            else if (horPos < -20f)
            {
                horPos =
                    Camera.main
                        .ViewportToWorldPoint(new Vector3(1.1f, 0, 0))
                        .x;
            }

            //Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
            newEnemy.transform.position = new Vector2(horPos, verticalPos);
            //Debug.Log($"({horPos}, {verticalPos})\n");
            newEnemy.GetComponent<EnemyScript>().diff = difficultyTracker;
            yield return new WaitForSeconds(2);
        }
        
    }

    IEnumerator DifficultyIncrementer()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            difficultyTracker += 1f;
            Debug.Log($"Difficulty increased to: {difficultyTracker}");
        }
        
        
    }
}
