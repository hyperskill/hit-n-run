using System;
using System.Collections;
using System.Resources;
using WindowsInput;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class H_ScenesTest
{
    public InputSimulator IS = new InputSimulator();

    public GameObject play, exit, highScore, canvas, camera;
    public RectTransform playRT, exitRT, highScoreRT;
    public Text text;

    [UnityTest, Order(0)]
    public IEnumerator SetUp()
    {
        PlayerPrefs.DeleteAll();
        PMHelper.TurnCollisions(false);
        Time.timeScale = 10;
        if (!Application.CanStreamedLevelBeLoaded("Main Menu"))
        {
            Assert.Fail("\"Main Menu\" scene is misspelled or was not added to build settings");
        }
        
        SceneManager.LoadScene("Main Menu");
        yield return null;
    }
    
    [UnityTest, Order(1)]
    public IEnumerator CheckObjects()
    {
        canvas = PMHelper.Exist("Canvas");
        play = PMHelper.Exist("Play");
        exit = PMHelper.Exist("Exit");
        highScore = PMHelper.Exist("HighScore");
        camera = PMHelper.Exist("Main Camera");
        yield return null;
        
        if (!canvas)
            Assert.Fail("There is no canvas in scene named \"Canvas\"");
        if (!play)
            Assert.Fail("There is no \"Play\" object in scene, or it is misspelled");
        if (!exit)
            Assert.Fail("There is no \"Exit\" object in scene, or it is misspelled");
        if (!highScore)
            Assert.Fail("There is no \"HighScore\" object in scene, or it is misspelled");
        
        if (!PMHelper.Exist<Canvas>(canvas))
            Assert.Fail("Canvas has no <Canvas> component");
        if (!PMHelper.Exist<CanvasScaler>(canvas))
            Assert.Fail("Canvas has no <Canvas Scaler> component");
        if (!PMHelper.Exist<GraphicRaycaster>(canvas))
            Assert.Fail("Canvas has no <Graphic Raycaster> component");

        playRT = PMHelper.Exist<RectTransform>(play);
        exitRT = PMHelper.Exist<RectTransform>(exit);
        highScoreRT = PMHelper.Exist<RectTransform>(highScore);
        
        if (!playRT)
            Assert.Fail("Play-button has no <Rect Transform> component");
        if (!PMHelper.Exist<CanvasRenderer>(play))
            Assert.Fail("Play-button has no <Canvas Renderer> component");
        if (!PMHelper.Exist<Button>(play))
            Assert.Fail("Play-button has no <Button> component");
        
        if (!exitRT)
            Assert.Fail("Exit-button has no <Rect Transform> component");
        if (!PMHelper.Exist<CanvasRenderer>(exit))
            Assert.Fail("Exit-button has no <Canvas Renderer> component");
        if (!PMHelper.Exist<Button>(exit))
            Assert.Fail("Exit-button has no <Button> component");
        text = PMHelper.Exist<Text>(highScore);
        if (!highScoreRT)
            Assert.Fail("HighScore text object has no <Rect Transform> component");
        if (!PMHelper.Exist<CanvasRenderer>(highScore))
            Assert.Fail("HighScore text object has no <Canvas Renderer> component");
        if (!text)
            Assert.Fail("HighScore text object has no <Text> component");
        if (!text.text.Equals("0"))
        {
            Assert.Fail("\"HighScore\"'s <Text> component's text value should be equal to \"0\" by default");
        }
        
        if (!PMHelper.Child(play, canvas))
            Assert.Fail("\"Play\" object should be a child of a \"Canvas\" object");
        if (!PMHelper.Child(exit, canvas))
            Assert.Fail("\"Exit\" object should be a child of a \"Canvas\" object");
        if (!PMHelper.Child(highScore, canvas))
            Assert.Fail("\"HighScore\" object should be a child of a \"Canvas\" object");

        yield return null;
        
        if (!camera)
        {
            Assert.Fail("There is no camera object in \"Main Menu\" scene, named \"Main Camera\", or it is misspelled");
        }

        if (!PMHelper.Exist<Camera>(camera))
        {
            Assert.Fail("\"Main Camera\" object has no basic component <Camera>");
        }
    }

    [UnityTest, Order(2)]
    public IEnumerator AnchorsCheck()
    {
        yield return null;
        if (!PMHelper.CheckRectTransform(playRT))
        {
            Assert.Fail("Anchors of \"Play\"'s <RectTransform> component are incorrect or it's offsets " +
                        "are not equal to zero, might be troubles with different resolutions");
        }
        if (!PMHelper.CheckRectTransform(exitRT))
        {
            Assert.Fail("Anchors of \"Exit\"'s <RectTransform> component are incorrect or it's offsets " +
                        "are not equal to zero, might be troubles with different resolutions");
        }
        if (!PMHelper.CheckRectTransform(highScoreRT))
        {
            Assert.Fail("Anchors of \"HighScore\"'s <RectTransform> component are incorrect or it's offsets " +
                        "are not equal to zero, might be troubles with different resolutions");
        }
    }

    [UnityTest, Order(3)]
    public IEnumerator CheckGameTab()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        Scene scene = SceneManager.GetActiveScene();
        IS.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.TAB);
        
        float start = Time.unscaledTime;
        yield return new WaitUntil(() =>
            scene!=SceneManager.GetActiveScene() || (Time.unscaledTime - start) * Time.timeScale > 5);
        if ((Time.unscaledTime - start) * Time.timeScale >= 5)
        {
            Assert.Fail("Pressing Tab-key is not providing scene changing from \"Game\" to \"Main Menu\"");
        }

        yield return null;
        Scene mainMenu = SceneManager.GetActiveScene();
        yield return null;
        if (!mainMenu.name.Equals("Main Menu"))
        {
            Assert.Fail("Pressing Tab-key is not providing scene changing from \"Game\" to \"Main Menu\"");
        }
    }

    [UnityTest, Order(4)]
    public IEnumerator CheckGameReload()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        Scene scene = SceneManager.GetActiveScene();
        IS.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_R);
        
        float start = Time.unscaledTime;
        yield return new WaitUntil(() =>
            scene!=SceneManager.GetActiveScene() || (Time.unscaledTime - start) * Time.timeScale > 5);
        if ((Time.unscaledTime - start) * Time.timeScale >= 5)
        {
            Assert.Fail("Pressing R-key is not reloading \"Game\" scene");
        }

        yield return null;
        Scene scene2 = SceneManager.GetActiveScene();
        yield return null;
        if (!scene2.name.Equals("Game"))
        {
            Assert.Fail("Pressing R-key is not reloading \"Game\" scene");
        }
    }

    [UnityTest, Order(5)]
    public IEnumerator CheckMenuPlay()
    {
        SceneManager.LoadScene("Main Menu");
        yield return null;
        Scene mainMenu = SceneManager.GetActiveScene();
        play = PMHelper.Exist("Play");
        yield return null;
        Button playB = PMHelper.Exist<Button>(play);
        yield return null;
        
        playB.onClick.Invoke();

        float start = Time.unscaledTime;
        yield return new WaitUntil(() =>
            mainMenu!=SceneManager.GetActiveScene() || (Time.unscaledTime - start) * Time.timeScale > 5);
        if ((Time.unscaledTime - start) * Time.timeScale >= 5)
        {
            Assert.Fail("Pressing Play-Button is not loading \"Game\" scene!");
        }

        yield return null;
        Scene scene = SceneManager.GetActiveScene();
        yield return null;
        if (!scene.name.Equals("Game"))
        {
            Assert.Fail("Pressing Play-Button is not loading \"Game\" scene!");
        }
    }
    [UnityTest, Order(6)]
    public IEnumerator CheckChangingPlayerPrefs()
    {
        //Deleting playerPrefs and loading Main Menu
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Main Menu");
        yield return null;
        
        Scene mainMenu = SceneManager.GetActiveScene();
        //Checking if high score is 0
        highScore = PMHelper.Exist("HighScore");
        yield return null;
        text = PMHelper.Exist<Text>(highScore);
        yield return null;
        if (!text.text.Equals("0"))
        {
            Assert.Fail("\"HighScore\"'s <Text> component's text value should be equal to \"0\" by default");
        }

        //Opening "Game" scene
        
        play = PMHelper.Exist("Play");
        yield return null;
        Button playB = PMHelper.Exist<Button>(play);
        yield return null;
        playB.onClick.Invoke();

        float start = Time.unscaledTime;
        yield return new WaitUntil(() =>
            mainMenu!=SceneManager.GetActiveScene() || (Time.unscaledTime - start) * Time.timeScale > 5);
        if ((Time.unscaledTime - start) * Time.timeScale >= 5)
        {
            Assert.Fail("Pressing Play-Button is not loading \"Game\" scene!");
        }
        //Disabling player's collider, removing obstacles and borders, getting score object and enemy object
        GameObject player = PMHelper.Exist("Player");
        Collider2D playerColl = PMHelper.Exist<Collider2D>(player);
        playerColl.enabled = false;
        yield return null;
        
        GameObject scoreObj = PMHelper.Exist("Score");
        yield return null;
        Text scoreText = PMHelper.Exist<Text>(scoreObj);
        yield return null;
        
        GameObject helperObj = new GameObject("helper");
        StageHelper helper = helperObj.AddComponent<StageHelper>();
        helper.RemoveBorders();
        helper.RemoveObstacles();
        yield return null;

        GameObject enemy = GameObject.FindWithTag("Enemy");
        Rigidbody2D enemyRb = PMHelper.Exist<Rigidbody2D>(enemy);
        enemyRb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return null;
        
        //Shooting
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
        
        //Copying bullet, destroying enemy and after waiting destroying another one
        GameObject bullet2 = GameObject.Instantiate(bullet);
        bullet.transform.position = enemy.transform.position;
        Time.timeScale = 1;
        PMHelper.TurnCollisions(true);
        
        start = Time.unscaledTime;
        yield return new WaitUntil(() => GameObject.FindWithTag("Enemy") && GameObject.FindWithTag("Enemy")!=enemy 
                                         || (Time.unscaledTime - start) * Time.timeScale > 20);
        if ((Time.unscaledTime - start) * Time.timeScale >= 20)
        {
            Assert.Fail();
        }

        enemy = GameObject.FindWithTag("Enemy");
        yield return null;
        bullet2.transform.position = enemy.transform.position;
        
        start = Time.unscaledTime;
        yield return new WaitUntil(() =>
            enemy==null || (Time.unscaledTime - start) * Time.timeScale > 5);
        if ((Time.unscaledTime - start) * Time.timeScale >= 5)
        {
            Assert.Fail();
        }
        
        //Getting score and loading Main Menu
        int score=-1;
        try
        {
            score = int.Parse(scoreText.text);
        }
        catch (Exception)
        {
            Assert.Fail("After changing score-text it should contain only integer value");
        }
        PMHelper.TurnCollisions(false);

        Scene scene = SceneManager.GetActiveScene();
        IS.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.TAB);
        
        start = Time.unscaledTime;
        yield return new WaitUntil(() =>
            scene!=SceneManager.GetActiveScene() || (Time.unscaledTime - start) * Time.timeScale > 5);
        if ((Time.unscaledTime - start) * Time.timeScale >= 5)
        {
            Assert.Fail("Pressing Tab-key is not providing scene changing from \"Game\" to \"Main Menu\"");
        }
        
        highScore = PMHelper.Exist("HighScore");
        yield return null;
        text = PMHelper.Exist<Text>(highScore);
        yield return null;
        
        //Getting high score and checking if it is the score, we've earned
        int high=-1;
        try
        {
            high = int.Parse(text.text);
        }
        catch (Exception)
        {
            Assert.Fail("After changing HighScore-text it should contain only integer value");
        }

        if (high != score)
        {
            Assert.Fail("High score is not being reloaded, or is being reloaded incorrectly," +
                        " when player gets more points");
        }
        
        //Loading "Game" again and setting up the scene
        SceneManager.LoadScene("Game");
        yield return null;
        scene = SceneManager.GetActiveScene();
        
        helperObj = new GameObject("helper");
        helper = helperObj.AddComponent<StageHelper>();
        helper.RemoveBorders();
        helper.RemoveObstacles();
        yield return null;
        
        enemy = GameObject.FindWithTag("Enemy");
        enemyRb = PMHelper.Exist<Rigidbody2D>(enemy);
        enemyRb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return null;
        
        //Shooting and destroying an enemy
        X = game.position.center.x+game.position.width/4;
        X = X * 65535 / Screen.width;
        Y = game.position.center.y+game.position.height/4;
        Y = Y * 65535 / Screen.height;
        IS.Mouse.MoveMouseTo(Convert.ToDouble(X), Convert.ToDouble(Y));
        yield return null;
        IS.Mouse.LeftButtonClick();
        yield return null;
        
        bullet = GameObject.FindWithTag("Bullet");
        bulletRb = PMHelper.Exist<Rigidbody2D>(bullet);
        bulletRb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return null;
        
        PMHelper.TurnCollisions(true);
        bullet.transform.position = enemy.transform.position;
        
        start = Time.unscaledTime;
        yield return new WaitUntil(() =>
            enemy==null || (Time.unscaledTime - start) * Time.timeScale > 5);
        if ((Time.unscaledTime - start) * Time.timeScale >= 5)
        {
            Assert.Fail();
        }
        
        PMHelper.TurnCollisions(false);
        //Loading "Main Menu" and checking if new highscore is still last one
        IS.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.TAB);
        
        start = Time.unscaledTime;
        yield return new WaitUntil(() =>
            scene!=SceneManager.GetActiveScene() || (Time.unscaledTime - start) * Time.timeScale > 5);
        if ((Time.unscaledTime - start) * Time.timeScale >= 5)
        {
            Assert.Fail("Pressing TAB-key Tab-key is not providing scene changing from \"Game\" to \"Main Menu\"");
        }
        
        highScore = PMHelper.Exist("HighScore");
        yield return null;
        text = PMHelper.Exist<Text>(highScore);
        yield return null;
        
        int cur=-1;
        try
        {
            cur = int.Parse(text.text);
        }
        catch (Exception)
        {
            Assert.Fail("After changing HighScore-text it should contain only integer value");
        }
        
        if (cur != score)
        {
            Assert.Fail("High score should change only when player gets more points");
        }
    }
}