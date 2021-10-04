using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class player_functions : MonoBehaviour
{
    // Start is called before the first frame update
    float speed = 12;
    private GameObject shotgun;
    void Start()
    {
        shotgun = GameObject.Find("Shotgun");
    }

    // Update is called once per frame
    void Update()
    {
        move();
        rotate();
    }

    void rotate()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = Input.mousePosition - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        // transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void move()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            float movement = Input.GetAxis("Horizontal") * speed;
            movement *= Time.deltaTime;
            Vector3 newPosition = new Vector3(movement, 0, 0);
            transform.position += newPosition;

            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y,
                Camera.main.transform.position.z);
        }
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            float movement = Input.GetAxis("Horizontal") * speed;
            movement *= Time.deltaTime;
            Vector3 newPosition = new Vector3(movement, 0, 0);
            transform.position += newPosition;

            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y,
                Camera.main.transform.position.z);
        }
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            float movement = Input.GetAxis("Vertical") * speed;
            movement *= Time.deltaTime;
            Vector3 newPosition = new Vector3(0, movement, 0);
            transform.position += newPosition;

            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y,
                Camera.main.transform.position.z);
        }
        
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            float movement = Input.GetAxis("Vertical") * speed;
            movement *= Time.deltaTime;
            Vector3 newPosition = new Vector3(0, movement, 0);
            transform.position += newPosition;

            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y,
                Camera.main.transform.position.z);
        }
    }
}
