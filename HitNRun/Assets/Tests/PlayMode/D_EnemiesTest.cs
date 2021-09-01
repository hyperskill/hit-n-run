using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class D_EnemiesTest
{
    public D_EnemiesTest()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 40;
    }
    [UnityTest, Order(1)]
    public IEnumerator CheckCorrectSpawn()
    {
        yield return null;
        GameObject tmp = GameObject.FindWithTag("Enemy");
        if (tmp == null)
        {
            Assert.Fail("Enemies not spawning instantly after opening scene, or prefab's tag is misspelled!");
        }

        SpriteRenderer sr = tmp.GetComponent<SpriteRenderer>();
        Assert.NotNull(sr,"Enemies are not visible, there is no <SpriteRenderer> component on it's object!");

        Color enemyColor = sr.color;
        Color playerColor = GameObject.Find("Player").GetComponent<SpriteRenderer>().color;
        Color shotgunColor = GameObject.Find("Shotgun").GetComponent<SpriteRenderer>().color;
        Color backColor = GameObject.Find("Main Camera").GetComponent<Camera>().backgroundColor;
        Color obstacleColor = GameObject.FindWithTag("Obstacle").GetComponent<SpriteRenderer>().color;
        
        float difPlayer = Mathf.Abs(playerColor.r - enemyColor.r) + Mathf.Abs(playerColor.g - enemyColor.g) +
                          Mathf.Abs(playerColor.b - enemyColor.b);
        float difShotgun = Mathf.Abs(shotgunColor.r - enemyColor.r) + Mathf.Abs(shotgunColor.g - enemyColor.g) +
                           Mathf.Abs(shotgunColor.b - enemyColor.b);
        float difBack = Mathf.Abs(backColor.r - enemyColor.r) + Mathf.Abs(backColor.g - enemyColor.g) +
                        Mathf.Abs(backColor.b - enemyColor.b);
        float difObstacle = Mathf.Abs(obstacleColor.r - enemyColor.r) + Mathf.Abs(obstacleColor.g - enemyColor.g) +
                        Mathf.Abs(obstacleColor.b - enemyColor.b);
        
        Assert.Greater(difPlayer, 0.4f, "The difference of colors between \"Player\" and enemies should be visible!");
        Assert.Greater(difBack, 0.4f, "The difference of colors between enemies and background should be visible!");
        Assert.Greater(difShotgun, 0.4f, "The difference of colors between \"Shotgun\" and enemies should be visible!");
        Assert.Greater(difObstacle, 0.4f, "The difference of colors between enemies and obstacles should be visible!");

        Collider2D colPl = GameObject.Find("Player").GetComponent<Collider2D>();
        colPl.enabled = false;
        for (int j = 2; j < 5; j++)
        {
            yield return new WaitForSeconds(2);
            yield return null;
            GameObject[] tmp2 = GameObject.FindGameObjectsWithTag("Enemy");
            yield return null;
            if (tmp2.Length < j)
            {
                Assert.Fail("Enemies not spawning each 2 seconds!");
            }
        }
        colPl.enabled = true;

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            GameObject.Destroy(g);
        }
    }

    [UnityTest, Order(2)]
    public IEnumerator CheckOutsideBounds()
    {
        yield return null;
        SceneManager.LoadScene("Game");
        for (int j = 0; j < 3; j++)
        {
            yield return null;
            GameObject tmp3 = GameObject.FindWithTag("Enemy");
            if (tmp3.GetComponent<Renderer>().isVisible)
            {
                Assert.Fail("Object should not be visible when spawned!");
            }
            GameObject.Destroy(tmp3);
            yield return new WaitForSeconds(2);
        }
    }

    [UnityTest, Order(3)]
    public IEnumerator CheckEnemyMovement()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            GameObject.Destroy(g);
        }
        GameObject tmp = GameObject.FindWithTag("Enemy");
        float startDist = Vector3.Distance(tmp.transform.position,GameObject.Find("Player").transform.position);
        yield return new WaitForSeconds(1);
        float middleDist = Vector3.Distance(tmp.transform.position,GameObject.Find("Player").transform.position);
        yield return new WaitForSeconds(1);
        float endDist = Vector3.Distance(tmp.transform.position,GameObject.Find("Player").transform.position);
        Assert.Less(middleDist, startDist, "Enemies are not following the player!");
        Assert.Less(endDist, startDist, "Enemies are not following the player!");
    }

    [UnityTest, Order(4)]
    public IEnumerator CheckReload()
    {
        yield return null;
        Scene cur = SceneManager.GetActiveScene();
        String curName = cur.name;
        GameObject tmp = GameObject.FindWithTag("Enemy");
        tmp.transform.position = GameObject.Find("Player").transform.position;
        yield return new WaitForSeconds(1);
        Assert.That(SceneManager.GetActiveScene() != cur,"Scene is not being changed after enemies collided with player!");
        Assert.That(SceneManager.GetActiveScene().name.Equals(curName), "Scene is not being reloaded!");
    }
}
