using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemy_movement : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 0.5f));
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 5 * Time.deltaTime);
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
