using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject myPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        const int numBlobs = 25;
        
        for (int i = 0; i < numBlobs; i++)
        {
            GameObject newBlob = Instantiate(myPrefab);
            float radius = Random.Range(1f, 3f);
            newBlob.transform.localScale = new Vector3(radius * 2, radius * 2, 0);
            newBlob.transform.position = new Vector2(Random.Range(-30f, 50f), Random.Range(-7f, 30f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
