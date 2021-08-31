using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyControl : MonoBehaviour
{
    private float speed = 1.0f;
    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        //Debug.Log("Enemy spawned.");
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;

        

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