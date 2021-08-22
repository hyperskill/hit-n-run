using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyScript : MonoBehaviour
{
    private float speed = 1.0f;
    private GameObject player;
    public float diff = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        float newRed = (200f + 10f * diff) > 255 ? 255 : (200f + 5f * diff);
        float newGreen = (185f - 10f * diff) < 0 ? 0 : (185f - 10f * diff);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(newRed/255f, newGreen/255f, 54f/255f);
        Debug.Log($"Spawned with Diff: {diff}");
    }
    
    // Update is called once per frame
    void Update()
    {
        float step = speed * diff * Time.deltaTime;

        

        // move sprite towards the target location
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)player.transform.position, step);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log(other.gameObject.tag);
        if (other.gameObject == player)
        {
            SceneManager.LoadScene("Game");
        }
    }
}
//newEnemy.transform.position = new Vector2(Random.Range(-23f, 17f), Random.Range(-8f, 13f));