using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using Scene = UnityEngine.SceneManagement.Scene;

public class game_management : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject obstacle;
    public GameObject enemy;
    void Start()
    {
        createObstaclesRandomly();
        StartCoroutine(createEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        
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
        while (true)
        {
            Vector3 camPos = Camera.main.transform.position;
            
            float locationX = Random.Range(-24, camPos.x - 15);
            float locationY = Random.Range(-24, camPos.y - 7);
            
            Instantiate(enemy, new Vector3(locationX, locationY, 0), Quaternion.identity);
            
            yield return new WaitForSeconds(2);
        }
    }
}
