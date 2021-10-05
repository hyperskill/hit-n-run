using System;
using System.Collections;
using WindowsInput;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using WindowsInput;
using UnityEditor;
using Random = UnityEngine.Random;

public class E_ShootingTest
{
    private InputSimulator IS = new InputSimulator();

    public E_ShootingTest()
    {
        SceneManager.LoadScene("Game");
    }

    [UnityTest, Order(1)]
    public IEnumerator CheckSpawnShotComponents()
    {
        yield return null;
        Time.timeScale = 5;
        
        Color enemyColor = GameObject.FindWithTag("Enemy").GetComponent<SpriteRenderer>().color;
        Color obstacleColor = GameObject.FindWithTag("Obstacle").GetComponent<SpriteRenderer>().color;
        int sortOrderEnemy=GameObject.FindWithTag("Enemy").GetComponent<SpriteRenderer>().sortingOrder;
        int sortOrderObstacle = GameObject.FindWithTag("Obstacle").GetComponent<SpriteRenderer>().sortingOrder;

        yield return null;
        
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            GameObject.Destroy(g);
        }
        GameObject.Find("Player").AddComponent<TmpDestroyEnemies>();
        
        yield return null;
        float x = Screen.width/2;
        float y = Screen.height/2;
        x *= 65535 / Screen.width;
        y *= 65535 / Screen.height;
        yield return null;
        EditorWindow window = EditorWindow.focusedWindow;
 
        window.maximized = !window.maximized;
        yield return null;

        IS.Mouse.MoveMouseTo(Convert.ToDouble(x), Convert.ToDouble(y));
        IS.Mouse.LeftButtonClick();
        yield return new WaitForSeconds(0.5f);
        GameObject tmp = GameObject.FindWithTag("Shot");
        Assert.NotNull(tmp, "Bullet is not been spawned, or it's tag is misspelled!");
        SpriteRenderer srTmp = tmp.GetComponent<SpriteRenderer>();
        Assert.NotNull(srTmp, "Bullet is not visible, there is no <SpriteRenderer> component on it!");
        Assert.LessOrEqual(tmp.transform.localScale.x, 0.5f,
            "Bullet's scale on X-axis should be less or equal to 0.5!");
        Assert.LessOrEqual(tmp.transform.localScale.y, 0.5f,
            "Bullet's scale on Y-axis should be less or equal to 0.5!");
        srTmp.sortingOrder = 3;
        GameObject.Find("Shotgun").GetComponent<SpriteRenderer>().sortingOrder = 4;
        bool order = sortOrderEnemy < srTmp.sortingOrder
                     && sortOrderObstacle < srTmp.sortingOrder
                     && GameObject.Find("Player").GetComponent<SpriteRenderer>().sortingOrder > srTmp.sortingOrder
                     && GameObject.Find("Shotgun").GetComponent<SpriteRenderer>().sortingOrder > srTmp.sortingOrder;
        Assert.True(order, "Bullet's order in layer should be greater than order in layer of obstacles and enemies, " +
                           "but less than order in layer of player and it's shotgun!");

        Color shotColor = srTmp.color;
        Color playerColor = GameObject.Find("Player").GetComponent<SpriteRenderer>().color;
        Color shotgunColor = GameObject.Find("Shotgun").GetComponent<SpriteRenderer>().color;
        Color backColor = GameObject.Find("Main Camera").GetComponent<Camera>().backgroundColor;

        float difPlayer = Mathf.Abs(playerColor.r - shotColor.r) + Mathf.Abs(playerColor.g - shotColor.g) +
                          Mathf.Abs(playerColor.b - shotColor.b);
        float difShotgun = Mathf.Abs(shotgunColor.r - shotColor.r) + Mathf.Abs(shotgunColor.g - shotColor.g) +
                           Mathf.Abs(shotgunColor.b - shotColor.b);
        float difBack = Mathf.Abs(backColor.r - shotColor.r) + Mathf.Abs(backColor.g - shotColor.g) +
                        Mathf.Abs(backColor.b - shotColor.b);
        float difObstacle = Mathf.Abs(obstacleColor.r - shotColor.r) + Mathf.Abs(obstacleColor.g - shotColor.g) +
                            Mathf.Abs(obstacleColor.b - shotColor.b);
        float defEnemy = Mathf.Abs(enemyColor.r - shotColor.r) + Mathf.Abs(enemyColor.g - shotColor.g) +
                         Mathf.Abs(enemyColor.b - shotColor.b);

        Assert.Greater(difPlayer, 0.4f, "The difference of colors between \"Player\" and bullets should be visible!");
        Assert.Greater(difBack, 0.4f, "The difference of colors between bullets and background should be visible!");
        Assert.Greater(difShotgun, 0.4f, "The difference of colors between \"Shotgun\" and bullets should be visible!");
        Assert.Greater(difObstacle, 0.4f, "The difference of colors between obstacles and bullets should be visible!");
        Assert.Greater(defEnemy, 0.4f, "The difference of colors between enemies and bullets should be visible!");
        yield return null;

        Time.timeScale = 40;
        yield return null;
        Collider2D tmpCol = tmp.GetComponent<Collider2D>();
        Assert.NotNull(tmpCol,"Bullets should have an attached 2D collider in order to interact with enemies!");
        Assert.True(tmpCol.isTrigger,"Bullets should trigger events, not collide (isTrigger parameter of 2D collider should be checked)!");
        yield return new WaitForSeconds(2f);
        if(tmp==null)
            Assert.Fail("Bullet should not be destroyed in less than 2.5 seconds after shooting!");
        yield return new WaitForSeconds(2.5f);
        if(tmp!=null)
            Assert.Fail("Bullet should be destroyed in less than 5 seconds after shooting!");
    }

    [UnityTest, Order(2)]
    public IEnumerator CheckBulletMovement()
    {
        yield return null;
        Time.timeScale = 3;
        
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            GameObject.Destroy(g);
        }
        GameObject.Find("Player").AddComponent<TmpDestroyEnemies>();
        
        GameObject player = GameObject.Find("Player");
        yield return new WaitForSeconds(0.5f);
        float radius = 100f;
        float x = Screen.width/2;
        float y = Screen.height/2;
        float angle = Random.Range(0, 360);
        x += Mathf.Cos(angle) * radius;
        y += Mathf.Sin(angle) * radius;
        x *= 65535 / Screen.width;
        y *= 65535 / Screen.height;
        
        yield return null;
        EditorWindow window = EditorWindow.focusedWindow;
        // Assume the game view is focused.
        window.maximized = !window.maximized;
        yield return null;

        IS.Mouse.MoveMouseTo(Convert.ToDouble(x), Convert.ToDouble(y));

        IS.Mouse.LeftButtonClick();
        yield return new WaitForSeconds(0.5f);
        GameObject tmp = GameObject.FindWithTag("Shot");

        Vector3 startPos = tmp.transform.position;
        float destStart = Vector3.Distance(player.transform.position, startPos);
        Vector3 way1 = tmp.transform.position - player.transform.position;
        yield return new WaitForSeconds(0.5f);

        Vector3 endPos = tmp.transform.position;
        float destEnd = Vector3.Distance(player.transform.position, endPos);
        Vector3 way2 = tmp.transform.position - player.transform.position;

        Assert.True(Mathf.Abs(way2.x/way1.x-way2.y/way1.y)<0.001f && destEnd > destStart,way2.x/way1.x-way2.y/way1.y+" "+destEnd+" "+destStart);
    }
    
    [UnityTest, Order(2)]
    public IEnumerator CheckBulletActionEnemy()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        Time.timeScale = 20;
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            GameObject.Destroy(g);
        }

        yield return null;
        float radius = 100f;
        float x = Screen.width/2;
        float y = Screen.height/2;
        float angle = Random.Range(0, 360);
        x += Mathf.Cos(angle) * radius;
        y += Mathf.Sin(angle) * radius;
        x *= 65535 / Screen.width;
        y *= 65535 / Screen.height;
        
        yield return null;
        EditorWindow window = EditorWindow.focusedWindow;
        // Assume the game view is focused.
        window.maximized = !window.maximized;
        yield return null;

        IS.Mouse.MoveMouseTo(Convert.ToDouble(x), Convert.ToDouble(y));

        IS.Mouse.LeftButtonClick();
        yield return new WaitForSeconds(0.5f);
        GameObject tmpBullet = GameObject.FindWithTag("Shot");
        GameObject tmpEnemy = GameObject.FindWithTag("Enemy");
        tmpEnemy.transform.position = tmpBullet.transform.position;
        yield return new WaitForSeconds(0.2f);
        if (tmpBullet != null)
        {
            Assert.Fail("Bullet is not been destroyed, when enemy triggers it!");
        }
        if (tmpEnemy != null)
        {
            Assert.Fail("Enemy is not been destroyed, when bullet shots it!");
        }
    }
    
    [UnityTest, Order(3)]
    public IEnumerator CheckBulletActionObstacle()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        Time.timeScale = 20;
        GameObject[] g = GameObject.FindGameObjectsWithTag("Obstacle");
        for (int i = 1; i < g.Length; i++)
        {
            GameObject.Destroy(g[i]);
        }

        GameObject.Find("Player").AddComponent<TmpDestroyEnemies>();
        yield return null;
        float radius = 100f;
        float x = Screen.width/2;
        float y = Screen.height/2;
        float angle = Random.Range(0, 360);
        x += Mathf.Cos(angle) * radius;
        y += Mathf.Sin(angle) * radius;
        x *= 65535 / Screen.width;
        y *= 65535 / Screen.height;
        
        yield return null;
        EditorWindow window = EditorWindow.focusedWindow;
        // Assume the game view is focused.
        window.maximized = !window.maximized;
        yield return null;

        IS.Mouse.MoveMouseTo(Convert.ToDouble(x), Convert.ToDouble(y));

        IS.Mouse.LeftButtonClick();
        yield return new WaitForSeconds(0.5f);
        GameObject tmpBullet = GameObject.FindWithTag("Shot");
        g[0].transform.position = tmpBullet.transform.position;
        yield return new WaitForSeconds(2f);
        if (tmpBullet != null)
        {
            Assert.Fail("Bullet is not been destroyed, when obstacles triggers it!");
        }
        if (g[0] == null)
        {
            Assert.Fail("Obstacles are been destroyed, when bullet shots it!");
        }
    }
}
