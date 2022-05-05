using System;
using System.Collections;
using WindowsInput;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class G_ScoreTest
{
    public InputSimulator IS = new InputSimulator();
    public GameObject score;
    public GameObject canvas;

    [UnityTest, Order(0)]
    public IEnumerator SetUp()
    {
        Time.timeScale = 0;
        SceneManager.LoadScene("Game");
        yield return null;
    }

    [UnityTest, Order(1)]
    public IEnumerator CheckExistanceAndComponents()
    {
        canvas = PMHelper.Exist("Canvas");
        score = PMHelper.Exist("Score");
        yield return null;
        if (!canvas)
        {
            Assert.Fail("There is no canvas in scene named \"Canvas\"");
        }
        if (!score)
        {
            Assert.Fail("There is no score text-field in scene named \"Score\"");
        }

        if (!PMHelper.Exist<Canvas>(canvas))
            Assert.Fail("Canvas has no <Canvas> component");
        if (!PMHelper.Exist<CanvasScaler>(canvas))
            Assert.Fail("Canvas has no <Canvas Scaler> component");
        if (!PMHelper.Exist<GraphicRaycaster>(canvas))
            Assert.Fail("Canvas has no <Graphic Raycaster> component");
        if (!PMHelper.Exist<CanvasRenderer>(score))
            Assert.Fail("Score field has no <Canvas Renderer> component");
        if (!PMHelper.Exist<Text>(score))
            Assert.Fail("Score field has no <Text> component");
   
        yield return null;
        if (!PMHelper.Child(score, canvas))
        {
            Assert.Fail("\"Score\" object should be a child of \"Canvas\" object");
        }
   
        yield return null;
        RectTransform rect = PMHelper.Exist<RectTransform>(score);
        if (!PMHelper.CheckRectTransform(rect))
        {
            Assert.Fail("Anchors of \"Score\"'s <RectTransform> component are incorrect or it's offsets " +
                        "are not equal to zero, might be troubles with different resolutions");
        }

        Text text = PMHelper.Exist<Text>(score);
        if (!text.text.Equals("0"))
        {
            Assert.Fail("\"Score\"'s text value should be initialized as \"0\" by default");
        }
    }

    [UnityTest, Order(2)]
    public IEnumerator CheckScoreIncrease()
    {
        SceneManager.LoadScene("Game");
        yield return null;

        GameObject player = PMHelper.Exist("Player");
        Collider2D playerColl = PMHelper.Exist<Collider2D>(player);
        playerColl.enabled = false;
        yield return null;
        
        GameObject helperObj = new GameObject("helper");
        StageHelper helper = helperObj.AddComponent<StageHelper>();
        helper.RemoveBorders();
        helper.RemoveObstacles();
        yield return null;
        
        score = PMHelper.Exist("Score");
        Text text = PMHelper.Exist<Text>(score);
        yield return null;

        GameObject enemy = GameObject.FindWithTag("Enemy");
        Rigidbody2D enemyRb = PMHelper.Exist<Rigidbody2D>(enemy);
        enemyRb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return null;
        
        EditorWindow game=null;
        var windows = (EditorWindow[])Resources.FindObjectsOfTypeAll(typeof(EditorWindow));
        foreach(var window in windows)
        {
            if(window != null && window.GetType().FullName == "UnityEditor.GameView")
            {
                game = window;
                break;
            }
        }

        yield return null;
        float X, Y;
        X = game.position.center.x+game.position.width/4;
        X = X * 65535 / Screen.width;
        Y = game.position.center.y+game.position.height/4;
        Y = Y * 65535 / Screen.height;
        IS.Mouse.MoveMouseTo(Convert.ToDouble(X), Convert.ToDouble(Y));
        yield return null;
        IS.Mouse.LeftButtonClick();
        yield return null;
        
        GameObject bullet = GameObject.FindWithTag("Bullet");
        Rigidbody2D bulletRb = PMHelper.Exist<Rigidbody2D>(bullet);
        bulletRb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return null;
        
        bullet.transform.position = enemy.transform.position;
        Time.timeScale = 1;
        PMHelper.TurnCollisions(true);
        yield return new WaitForSeconds(0.3f);
        int tmp2=-1;
        try
        {
            tmp2 = int.Parse(text.text);
        }
        catch (Exception)
        {
            Assert.Fail("After changing score-text it should contain only integer value");
        }

        if (tmp2 <= 0)
        {
            Assert.Fail("Score should increase after destroying an enemy");
        }

        PMHelper.TurnCollisions(false);
        
        helper.destroyEnemies=true;
        Time.timeScale = 10;
        float start = Time.unscaledTime;
        yield return new WaitUntil(() =>
            (Time.unscaledTime - start) * Time.timeScale > 20);
        
        helper.destroyEnemies=false;
        if (GameObject.FindWithTag("Enemy"))
        {
            GameObject.Destroy(GameObject.FindWithTag("Enemy"));
        }
        
        start = Time.unscaledTime;
        yield return new WaitUntil(() =>
            GameObject.FindWithTag("Enemy") || (Time.unscaledTime - start) * Time.timeScale > 20);
        if ((Time.unscaledTime - start) * Time.timeScale >= 20)
        {
            Assert.Fail();
        }
        Time.timeScale = 0;
        
        enemy = GameObject.FindWithTag("Enemy");
        enemyRb = PMHelper.Exist<Rigidbody2D>(enemy);
        enemyRb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return null;
        
        IS.Mouse.MoveMouseTo(Convert.ToDouble(X), Convert.ToDouble(Y));
        yield return null;
        IS.Mouse.LeftButtonClick();
        yield return null;
        
        bullet = GameObject.FindWithTag("Bullet");
        bulletRb = PMHelper.Exist<Rigidbody2D>(bullet);
        bulletRb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return null;
        
        bullet.transform.position = enemy.transform.position;
        Time.timeScale = 1;
        PMHelper.TurnCollisions(true);
        yield return new WaitForSeconds(0.3f);
        
        int tmp3=-1;
        try
        {
            tmp3 = int.Parse(text.text);
        }
        catch (Exception)
        {
            Assert.Fail("After changing score-text it should contain only integer value");
        }

        if (tmp3 - tmp2 <= tmp2)
        {
            Assert.Fail("Killing enemies with increased difficulty should give more points");
        }
    }
}