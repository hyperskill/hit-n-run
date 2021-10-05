using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class enemy_movement : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    private float speed;
    void Start()
    {
        player = GameObject.Find("Player");
        speed = game_management.getSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 0.5f));
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Game");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Shot")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
