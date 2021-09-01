using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.iOS;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameObject myPrefab;
    public GameObject enemyPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(EnemyGenerator());
        
        const int numBlobs = 15;
        
        for (int i = 0; i < numBlobs; i++)
        {
            GameObject newBlob = Instantiate(myPrefab);
            float radius = Random.Range(1f, 2f);
            newBlob.transform.localScale = new Vector3(radius * 2, radius * 2, 0);
            newBlob.transform.position = new Vector2(Random.Range(-16f, 16f), Random.Range(-9f, 9f));
        }

        

    }

    // Update is called once per frame
    void Update()
    {
        
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
                        .ViewportToWorldPoint(new Vector3(0, 1.05f, 0))
                        .y;
            }
            else //Down
            {
                verticalPos =
                    Camera.main
                        .ViewportToWorldPoint(new Vector3(0, -0.05f, 0))
                        .y;
            }
            
            //Out of bounds check
            if (verticalPos > 9f)
            {
                verticalPos =
                    Camera.main
                        .ViewportToWorldPoint(new Vector3(0, -0.05f, 0))
                        .y;
            }
            else if (verticalPos < -9f)
            {
                verticalPos =
                    Camera.main
                        .ViewportToWorldPoint(new Vector3(0, 1.05f, 0))
                        .y;
            }
            
            //Left/Right
            var leftOrRight = Random.Range(0, 2);
            float horPos = 0f;

            if (leftOrRight == 1) //right
            {
                horPos =
                    Camera.main
                        .ViewportToWorldPoint(new Vector3(1.05f, 0, 0))
                        .x;
            }
            else //Left
            {
                horPos =
                    Camera.main
                        .ViewportToWorldPoint(new Vector3(-0.05f, 0, 0))
                        .x;
            }
            
            //Out of bounds check
            if (horPos > 9f)
            {
                horPos =
                    Camera.main
                        .ViewportToWorldPoint(new Vector3(-0.05f, 0, 0))
                        .x;
            }
            else if (horPos < -9f)
            {
                horPos =
                    Camera.main
                        .ViewportToWorldPoint(new Vector3(1.05f, 0, 0))
                        .x;
            }

            //Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
            newEnemy.transform.position = new Vector2(horPos, verticalPos);
            yield return new WaitForSeconds(2.1f);
            //Thread.Sleep(TimeSpan.FromSeconds(2));
        }
        
    }
    
}