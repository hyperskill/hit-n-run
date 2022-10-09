using System;
using System.Collections;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class StageHelper : MonoBehaviour
{

    private bool EnemyTagExist = false;
    private bool ObstacleTagExist = false;
    private bool BorderTagExist = false;
    public bool destroyEnemies=false;
    private void Start()
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals("Enemy")) { EnemyTagExist = true;}
            if (t.stringValue.Equals("Obstacle")) { ObstacleTagExist = true;}
            if (t.stringValue.Equals("Border")) { BorderTagExist = true;}
        }
    }

    void Update()
    {
        if(!EnemyTagExist||!destroyEnemies)
            return;
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(g);
        }
    }

    public void RemoveObstacles()
    {
        if(!ObstacleTagExist)
            return;
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Destroy(g);
        }
    }
    
    public void RemoveBorders()
    {
        if(!BorderTagExist)
            return;
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Border"))
        {
            Destroy(g);
        }
    }
}