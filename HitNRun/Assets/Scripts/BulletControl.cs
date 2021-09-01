using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletControl : MonoBehaviour
{
    public float speed = 7f;
    public int point=1;
    
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
            var text = GameObject.Find("Score");
            point = GameObject.Find("GameManager").GetComponent<GameManager>().points;
            text.GetComponent<Text>().text = (Convert.ToInt32(text.GetComponent<Text>().text) + point).ToString();
        }
    }

    IEnumerator TimeToLive()
    {
        yield return new WaitForSeconds(4);
        GameObject.Destroy(this.gameObject);
    }
}