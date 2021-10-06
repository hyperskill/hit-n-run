using System.Collections;
using System.Collections.Generic;
using UnityEditor.Graphs;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class game_management : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject obstacle;
    public GameObject enemy;
    private static float speed = 5f;
    private static int score = 200;
    void Start()
    {
        createObstaclesRandomly();
        StartCoroutine(createEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene("Game");
        }

        if (Input.GetKey(KeyCode.Tab))
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    void createObstaclesRandomly()
    {
        
        for (int i = 0; i < 30; i++)
        {
            float localScale = Random.Range(2, 10);
            
            float x = Random.Range(-24, 24);
            float y = Random.Range(-24, 24);
            Instantiate(obstacle, new Vector3(x, y, 0), Quaternion.identity).transform.localScale = new Vector3(localScale, localScale, 0);
        }
    }

    IEnumerator createEnemy()
    {
        int counter = 0;
        Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        
        while (true)
        {
            Vector3 camPos = Camera.main.transform.position;

            float locationX = Random.Range(-24, camPos.x - 15); 
            //float locationX2 = Random.Range(camPos.x + 15, 24);
            float locationY = Random.Range(-24, camPos.y - 7);
            //float locationY2 = Random.Range(camPos.y + 7, 24);
            
            
            
            GameObject newEnemy = Instantiate(enemy, new Vector3(locationX, locationY, 0), Quaternion.identity);
            color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            newEnemy.GetComponent<Renderer>().material.color = color;
            
            counter++;

            if (counter == 4)
            {
                speed *= 1.4f;
                score += 400;
                counter = 0;
            }
            yield return new WaitForSeconds(2);
        }
    }

    public static float getSpeed()
    {
        return speed;
    }

    public static int getScoreIncrease()
    {
        return score;
    }
}
