using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 4f;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimeToLive());
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * (speed * Time.deltaTime), Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            return;
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            GameObject.Destroy(this.gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            GameObject.Destroy(other.gameObject);
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            Debug.Log($"Collision with: {other.gameObject.tag}");
        }
    }

    IEnumerator TimeToLive()
    {
        yield return new WaitForSeconds(4);
        GameObject.Destroy(this.gameObject);
    }
}
