using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private float baseSpeed = 3.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float hSpeed = baseSpeed * Input.GetAxisRaw("Horizontal") * Time.deltaTime;
        float vSpeed = baseSpeed * Input.GetAxisRaw("Vertical") * Time.deltaTime;
        
        transform.Translate(hSpeed, vSpeed, 0, Space.World);

        Camera.main.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, Camera.main.transform.position.z);
        
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        Vector2 direction = (mousePos - (Vector2) transform.position).normalized;
        
        transform.right = -direction;
    }
    
    
}
