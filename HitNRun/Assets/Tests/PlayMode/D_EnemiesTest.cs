using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class D_EnemiesTest
{
    private GameObject player;
    private float left, right, up, down;
    [UnityTest, Order(0)]
    public IEnumerator SetUp()
    {
        PMHelper.TurnCollisions(false);
        SceneManager.LoadScene("Game");
        yield return null;
        Time.timeScale = 3;
        
    }
    [UnityTest, Order(1)]
    public IEnumerator CheckCorrectSpawn()
    {
        bool EnemyTagExist = false;
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals("Enemy")) { EnemyTagExist = true; break; }
        }

        if (!EnemyTagExist)
        {
            Assert.Fail("\"Enemy\" tag was not added to project");
        }
        
        //Checking Enemy's parameters
        player=PMHelper.Exist("Player");
        yield return null;
        
        GameObject tmp = GameObject.FindWithTag("Enemy");
        yield return null;
        if (!tmp)
        {
            Assert.Fail("Enemies not spawning instantly after opening scene, or prefab's tag is misspelled");
        }

        SpriteRenderer srEnemy = PMHelper.Exist<SpriteRenderer>(tmp);
        Collider2D collEnemy = PMHelper.Exist<Collider2D>(tmp);
        if (!srEnemy || !srEnemy.enabled)
        {
            Assert.Fail("There is no <SpriteRenderer> component on \"Enemy\" object or it is disabled");
        }
        if (!srEnemy.sprite)
        {
            Assert.Fail("There is no sprite assigned to \"Enemy\"'s <SpriteRenderer>");
        }
        if (!collEnemy)
        {
            Assert.Fail("\"Enemy\" objects' should have assigned <Collider2D> component");
        }
        if (collEnemy.isTrigger)
        {
            Assert.Fail("\"Enemy\" objects' <Collider2D> component should not be triggerable");
        }

        Color enemyColor = srEnemy.color;
        Color playerColor = player.GetComponent<SpriteRenderer>().color;
        Color shotgunColor = PMHelper.Exist<SpriteRenderer>(PMHelper.Exist("Shotgun")).color;
        Color backColor = PMHelper.Exist<Camera>(PMHelper.Exist("Main Camera")).backgroundColor;
        Color obstacleColor = PMHelper.Exist<SpriteRenderer>(GameObject.FindWithTag("Obstacle")).color;

        if (!PMHelper.CheckColorDifference(enemyColor, playerColor))
            Assert.Fail("The difference of colors between \"Player\" and enemies should be visible");
        if (!PMHelper.CheckColorDifference(enemyColor, backColor))
            Assert.Fail("The difference of colors between enemies and background should be visible");
        if (!PMHelper.CheckColorDifference(enemyColor, shotgunColor))
            Assert.Fail("The difference of colors between \"Shotgun\" and enemies should be visible");
        if (!PMHelper.CheckColorDifference(enemyColor, obstacleColor))
            Assert.Fail("The difference of colors between enemies and obstacles should be visible");
        Rigidbody2D enemyRb = PMHelper.Exist<Rigidbody2D>(tmp);
        if (!enemyRb)
        {
            Assert.Fail("Enemies should have assigned <Rigidbody2D> component");
        }

        if (enemyRb.bodyType != RigidbodyType2D.Dynamic)
        {
            Assert.Fail("Enemies' <Rigidbody2D> component should be Dynamic");
        }
        if (!enemyRb.simulated)
        {
            Assert.Fail("Enemies' <Rigidbody2D> component should be simulated");
        }
        if (enemyRb.gravityScale!=0)
        {
            Assert.Fail("Enemies' <Rigidbody2D> component should not be affected by gravity, " +
                        "so it's Gravity Scale parameter should be equal to 0");
        }
        if (enemyRb.interpolation != RigidbodyInterpolation2D.None)
        {
            Assert.Fail("Do not change interpolation of Enemies' <Rigidbody2D> component. Set it as None");
        }
        if (enemyRb.constraints != RigidbodyConstraints2D.None)
        {
            Assert.Fail("Do not freeze any Enemies' <Rigidbody2D> component's constraints");
        }
    }

    [UnityTest, Order(2)]
    public IEnumerator CheckEnemySpawn()
    {
        SceneManager.LoadScene("Game");
        yield return null;

        GameObject.Destroy(GameObject.FindWithTag("Enemy"));
        yield return new WaitForEndOfFrame();
        for (int j = 0; j < 5; j++)
        {
            float start = Time.unscaledTime;
            yield return new WaitUntil(() =>
                GameObject.FindWithTag("Enemy") || (Time.unscaledTime - start) * Time.timeScale > 10);
            if ((Time.unscaledTime - start) * Time.timeScale >= 10)
            {
                Assert.Fail("Enemies not spawning each 2 seconds");
            }

            //Debug.Log((Time.unscaledTime - start) * Time.timeScale);
            if ((Time.unscaledTime - start) * Time.timeScale < 2 || (Time.unscaledTime - start) * Time.timeScale > 3)
            {
                Assert.Fail("Enemies not spawning each 2 seconds");
            }

            GameObject.Destroy(GameObject.FindWithTag("Enemy"));
            yield return new WaitForEndOfFrame();
        }
    }
    
    [UnityTest, Order(3)]
    public IEnumerator CheckInsideBounds()
    {
        Time.timeScale = 10;
        SceneManager.LoadScene("Game");
        yield return null;
        GameObject.Destroy(GameObject.FindWithTag("Enemy"));
        yield return null;
        
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Border"))
        {
            g.layer = LayerMask.NameToLayer("Test");
        }
        
        player = PMHelper.Exist("Player");
        Transform playerT=PMHelper.Exist<Transform>(player);
        yield return null;
        left = PMHelper.RaycastFront2D(playerT.position, Vector2.left,
            1 << LayerMask.NameToLayer("Test")).point.x;
        up = PMHelper.RaycastFront2D(playerT.position, Vector2.up,
            1 << LayerMask.NameToLayer("Test")).point.y;
        right = PMHelper.RaycastFront2D(playerT.position, Vector2.right,
            1 << LayerMask.NameToLayer("Test")).point.x;
        down = PMHelper.RaycastFront2D(playerT.position, Vector2.down,
            1 << LayerMask.NameToLayer("Test")).point.y;

        for (int i = 0; i < 15; i++)
        {
            float start = Time.unscaledTime;
            yield return new WaitUntil(() =>
                GameObject.FindWithTag("Enemy") || (Time.unscaledTime - start) * Time.timeScale > 10);
            if ((Time.unscaledTime - start) * Time.timeScale >= 10)
            {
                Assert.Fail("Enemies not spawning each 2 seconds");
            }
            
            GameObject tmp3 = GameObject.FindWithTag("Enemy");
            yield return null;
            if (!PMHelper.CheckObjectFits(tmp3.transform, new Vector2(left, up), new Vector2(right, down)))
            {
                Assert.Fail("Enemies should be instantiated inside limited area");
            }
            
            GameObject.Destroy(tmp3);
            yield return new WaitForEndOfFrame();
        }
    }

    [UnityTest, Order(4)]
    public IEnumerator CheckEnemyNotInViewAndRandom()
    {
        List<Vector2> list=new List<Vector2>();
        SceneManager.LoadScene("Game");
        yield return null;
        Camera camera = PMHelper.Exist<Camera>(PMHelper.Exist("Main Camera"));
        yield return null;
        GameObject.Destroy(GameObject.FindWithTag("Enemy"));
        yield return null;

        for (int j = 0; j < 15; j++)
        {
            float start = Time.unscaledTime;
            yield return new WaitUntil(() =>
                GameObject.FindWithTag("Enemy") || (Time.unscaledTime - start) * Time.timeScale > 10);
            if ((Time.unscaledTime - start) * Time.timeScale >= 10)
            {
                Assert.Fail("Enemies not spawning each 2 seconds");
            }
            
            GameObject tmp3 = GameObject.FindWithTag("Enemy");
            yield return new WaitForEndOfFrame();
            list.Add(tmp3.transform.position);
            if (PMHelper.CheckVisibility(camera,tmp3.transform,2))
            {
                Assert.Fail("Enemies should not be visible when spawned (should be instantiated off the camera view)");
            }
            GameObject.Destroy(tmp3);
            yield return null;
        }

        for (int i = 0; i < list.Count; i++)
        {
            for (int j = i; j < list.Count; j++)
            {
                if (list[i] == list[j])
                {
                    //Assert.Fail("Enemies should be spawned randomly");
                }
            }
        }
    }
    
    [UnityTest, Order(5)]
    public IEnumerator CheckEnemyMovement()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameObject helper = new GameObject("helper");
        StageHelper helperComp = helper.AddComponent<StageHelper>();

        yield return null;
        
        helperComp.RemoveObstacles();
        yield return null;
        
        player = PMHelper.Exist("Player");
        Transform playerT = player.transform;
        yield return null;
        
        GameObject enemy = GameObject.FindWithTag("Enemy");
        Transform enemyT = enemy.transform;
        yield return null;
        if (enemyT.position == playerT.position)
        {
            Assert.Fail("Enemies are too fast, decrease their speed");
        }

        enemyT.position = new Vector3(left+(right-left)/4, down+(up-down)/4);
        playerT.position = new Vector3(left+(right-left)/4*3, down+(up-down)/4*3);
        
        yield return null;
        if (enemyT.position == playerT.position)
        {
            Assert.Fail("Enemies should move,not teleport to the player");
        }
        
        float start = Time.unscaledTime;
        yield return new WaitUntil(() =>
            Vector2.Distance(enemyT.position,playerT.position)<0.01f || (Time.unscaledTime - start) * Time.timeScale > 20);
        if ((Time.unscaledTime - start) * Time.timeScale >= 20)
        {
            Assert.Fail("Enemies are not moving to a player, or moving too slow");
        }
        
        playerT.position = new Vector3(left+(right-left)/4*3, down+(up-down)/4);
        yield return null;
        start = Time.unscaledTime;
        yield return new WaitUntil(() =>
            Vector2.Distance(enemyT.position,playerT.position)<0.01f || (Time.unscaledTime - start) * Time.timeScale > 20);
        if ((Time.unscaledTime - start) * Time.timeScale >= 20)
        {
            Assert.Fail("Enemies are not moving to a player");
        }
        //Check movement via rigidbody
        Rigidbody2D enemyRb = PMHelper.Exist<Rigidbody2D>(enemy);
        yield return null;
        enemyRb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return null;
        Vector3 bef,now;
        
        bef=enemyT.position;
        playerT.position = new Vector3(left+(right-left)/4*3, down+(up-down)/4*3);
        yield return null;
        yield return null;
        now = enemyT.position;
        if (bef != now)
        {
            Assert.Fail("Enemies' movement was not implemented with <Rigidbody2D> component usage");
        }
        
        enemyRb.constraints = RigidbodyConstraints2D.None;
        
        //Check FixedUpdate() method usage        
        bef=enemyT.position;
        
        yield return null;
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForFixedUpdate();
            now = enemyT.position;
            if (now.Equals(bef))
            {
                Assert.Fail("Use FixedUpdate() event for enemy's physics simulation");
            }
            bef=now;
        }
    }

    [UnityTest, Order(6)]
    public IEnumerator CheckCollisions()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        
        Scene cur = SceneManager.GetActiveScene();

        player = PMHelper.Exist("Player");
        Transform playerT = player.transform;
        Collider2D playerColl = PMHelper.Exist<Collider2D>(player);
        yield return null;
        
        GameObject enemy = GameObject.FindWithTag("Enemy");
        Transform enemyT = enemy.transform;
        Collider2D enemyColl = PMHelper.Exist<Collider2D>(enemy);
        yield return null;
        
        GameObject obstacle = GameObject.FindWithTag("Obstacle");
        Transform obstacleT = obstacle.transform;
        Collider2D obstacleColl = PMHelper.Exist<Collider2D>(obstacle);
        yield return null;
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            if(obs!=obstacle)
                GameObject.Destroy(obs);
        }

        playerColl.enabled = false;
        enemyColl.enabled = false;
        obstacleColl.enabled = false;
        yield return null;
        
        PMHelper.TurnCollisions(true);
        
        playerT.position = new Vector3(left+(right-left)/2, up);
        playerColl.enabled = true;
        yield return null;
        yield return null;
        if (cur != SceneManager.GetActiveScene())
            Assert.Fail("When player collides with bounds scene should not reload");
        playerColl.enabled = false;
        yield return null;
        
        enemyT.position = new Vector3(left+(right-left)/2, up);
        enemyColl.enabled = true;
        yield return null;
        yield return null;
        if (cur != SceneManager.GetActiveScene())
            Assert.Fail("When enemy collides with bounds scene should not reload");
        enemyColl.enabled = false;
        yield return null;

        obstacleT.position = new Vector3(left+(right-left)/2, up);
        obstacleColl.enabled = true;
        yield return null;
        yield return null;
        if (cur != SceneManager.GetActiveScene())
            Assert.Fail("When obstacle collides with bounds scene should not reload");
        obstacleColl.enabled = false;
        yield return null;
        
        enemyT.position = obstacleT.position;
        enemyColl.enabled = true; obstacleColl.enabled = true;
        yield return null;
        yield return null;
        if (cur != SceneManager.GetActiveScene())
            Assert.Fail("When obstacle collides with enemy scene should not reload");
        enemyColl.enabled = false; obstacleColl.enabled = false;
        yield return null;
        
        playerT.position = obstacleT.position;
        playerColl.enabled = true; obstacleColl.enabled = true;
        yield return null;
        yield return null;
        if (cur != SceneManager.GetActiveScene())
            Assert.Fail("When player collides with obstacle scene should not reload");
        playerColl.enabled = false; obstacleColl.enabled = false;
        yield return null;
        
        playerT.position = enemyT.position;
        playerColl.enabled = true; enemyColl.enabled = true;
        yield return null;
        yield return null;
        if (cur == SceneManager.GetActiveScene())
            Assert.Fail("When player collides with an enemy scene should reload");
        yield return null;
    }
}