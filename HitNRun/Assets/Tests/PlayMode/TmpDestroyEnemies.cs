using UnityEngine;

public class TmpDestroyEnemies : MonoBehaviour
{
    void Update()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(g);
        }
    }
}
