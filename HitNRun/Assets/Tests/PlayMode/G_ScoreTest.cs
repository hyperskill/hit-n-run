using System;
using System.Collections;
using WindowsInput;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class G_ScoreTest
{
    public InputSimulator IS = new InputSimulator();
    public GameObject text;
    public GameObject canvas;

    [UnityTest, Order(1)]
    public IEnumerator Exists()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        text = GameObject.Find("Score");
        canvas = GameObject.Find("Canvas");
        Assert.NotNull(canvas,"There is no canvas in scene named <Canvas>!");
        Assert.NotNull(text,"There is no score text-field in scene named <Score>!");
    }
    
    [UnityTest, Order(2)]
    public IEnumerator BasicComponents()
    {
        yield return null;
        Assert.NotNull(canvas.GetComponent<Canvas>(),"Canvas has no component <Canvas>!");
        Assert.NotNull(canvas.GetComponent<CanvasScaler>(),"Canvas has no component <Canvas Scaler>!");
        Assert.NotNull(canvas.GetComponent<GraphicRaycaster>(),"Canvas has no component <Graphic Raycaster>!");
        Assert.NotNull(text.GetComponent<RectTransform>(),"Score field has no component <Rect Transform>!");
        Assert.NotNull(text.GetComponent<CanvasRenderer>(),"Score field has no component <Canvas Renderer>!");
        Assert.NotNull(text.GetComponent<Text>(),"Score field has no component <Text>!");
    }
    
    [UnityTest, Order(3)]
    public IEnumerator TextIsChild()
    {
        yield return null;
        Assert.That(text.transform.IsChildOf(canvas.transform),"Object <Score> should be a child of object <Canvas>!");
    }
    
    [UnityTest, Order(4)]
    public IEnumerator TextIsVisible()
    {
        yield return null;
        RectTransform rect = text.GetComponent<RectTransform>();
        bool correct = rect.anchorMax.x <= 1 &&
                       rect.anchorMax.y <= 1 &&
                       rect.anchorMin.x >= 0 &&
                       rect.anchorMin.y >= 0 &&
                       rect.anchorMin.x < rect.anchorMax.x &&
                       rect.anchorMin.y < rect.anchorMax.y;
        Assert.True(correct, "Score-anchors should be between (0; 0) and (1; 1)");
        bool correct2 = rect.offsetMin == Vector2.zero && rect.offsetMax == Vector2.zero;
        Assert.True(correct2, "Set all score-offsets axis to zero in order not to pass away from bounds of screen");
    }
    
    [UnityTest, Order(5)]
    public IEnumerator ZeroAsStandard()
    {
        yield return null;
        int tmp = -1;
        try
        {
            tmp = int.Parse(text.GetComponent<Text>().text);
        }
        catch (Exception)
        {
            Assert.Fail("Start text value should be initialized as \"0\"");
        }
        Assert.That(tmp==0,"Start text value should be initialized as \"0\"");
    }

    [UnityTest, Order(6)]
    public IEnumerator CheckScoreIncrease()
    {
        yield return null;
        Time.timeScale = 40;
        SceneManager.LoadScene("Game");
        yield return null;
        GameObject.Find("Player").GetComponent<Collider2D>().enabled = false;
        yield return null;
        
        text = GameObject.Find("Score");
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
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            GameObject.Destroy(g);
        }

        IS.Mouse.MoveMouseTo(Convert.ToDouble(x), Convert.ToDouble(y));
        IS.Mouse.LeftButtonClick();
        yield return new WaitForSeconds(0.5f);
        GameObject tmpBullet = GameObject.FindWithTag("Shot");
        GameObject tmpEnemy = GameObject.FindWithTag("Enemy");
        tmpEnemy.transform.position = tmpBullet.transform.position;
        yield return new WaitForSeconds(0.2f);
        int tmp2=-1;
        try
        {
            tmp2 = int.Parse(text.GetComponent<Text>().text);
        }
        catch (Exception)
        {
            Assert.Fail("After changing score-text it should contain only integer value!");
        }

        Assert.Greater(tmp2,0,"Score should increase after destroying an enemy!");
        
        yield return new WaitForSeconds(1.3f);
        
        for (int i = 0; i < 5; i++)
        {
            yield return null;
            GameObject.Destroy(GameObject.FindWithTag("Enemy"));
            yield return new WaitForSeconds(2);
        }
        yield return null;
        
        try
        {
            tmp2 = int.Parse(text.GetComponent<Text>().text);
        }
        catch (Exception)
        {
            Assert.Fail("After changing score-text it should contain only integer value!");
        }
        
        IS.Mouse.MoveMouseTo(Convert.ToDouble(x), Convert.ToDouble(y));
        IS.Mouse.LeftButtonClick();
        yield return new WaitForSeconds(0.5f);
        tmpBullet = GameObject.FindWithTag("Shot");
        tmpEnemy = GameObject.FindWithTag("Enemy");
        
        tmpEnemy.transform.position = tmpBullet.transform.position;
        yield return new WaitForSeconds(0.2f);
        int tmp3=-1;
        try
        {
            tmp3 = int.Parse(text.GetComponent<Text>().text);
        }
        catch (Exception)
        {
            Assert.Fail("After changing score-text it should contain only integer value!");
        }
        Assert.Greater(tmp3-tmp2,tmp2,"Killing enemies with increased difficulty should give more points!");
    }
}