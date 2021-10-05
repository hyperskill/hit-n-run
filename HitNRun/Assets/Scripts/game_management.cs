using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game_management : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject obstacle;
    void Start()
    {
        createObstaclesRandomly();
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
}
