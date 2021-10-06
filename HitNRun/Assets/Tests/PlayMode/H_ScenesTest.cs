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

    public GameObject play, exit, highScore, canvas;

    [UnityTest, Order(1)]
    public IEnumerator CheckObjects()
    {
        SceneManager.LoadScene("Main Menu");
        yield return null;
        play = GameObject.Find("Play");
        exit = GameObject.Find("Exit");
        highScore = GameObject.Find("HighScore");
        canvas = GameObject.Find("Canvas");
        
        Assert.NotNull(play,"There is no object \"Play\" in scene, or it is misspelled!");
        Assert.NotNull(exit,"There is no object \"Exit\" in scene, or it is misspelled!");
        Assert.NotNull(highScore,"There is no object \"HighScore\" in scene, or it is misspelled!");
        Assert.NotNull(canvas,"There is no canvas in scene named \"Canvas\"!");
    }
    
    [UnityTest, Order(2)]
    public IEnumerator BasicCanvasComponents()
    {
        yield return null;
        Assert.NotNull(canvas.GetComponent<Canvas>(),"Canvas has no component <Canvas>!");
        Assert.NotNull(canvas.GetComponent<CanvasScaler>(),"Canvas has no component <Canvas Scaler>!");
        Assert.NotNull(canvas.GetComponent<GraphicRaycaster>(),"Canvas has no component <Graphic Raycaster>!");
        Assert.NotNull(play.GetComponent<RectTransform>(),"Play-button has no component <Rect Transform>!");
        Assert.NotNull(play.GetComponent<CanvasRenderer>(),"Play-button has no component <Canvas Renderer>!");
        Assert.NotNull(play.GetComponent<Button>(),"Play-button has no component <Button>!");
        Assert.NotNull(exit.GetComponent<RectTransform>(),"Exit-button has no component <Rect Transform>!");
        Assert.NotNull(exit.GetComponent<CanvasRenderer>(),"Exit-button has no component <Canvas Renderer>!");
        Assert.NotNull(exit.GetComponent<Button>(),"Exit-button has no component <Button>!");
        Assert.NotNull(highScore.GetComponent<RectTransform>(),"HighScore text object has no component <Rect Transform>!");
        Assert.NotNull(highScore.GetComponent<CanvasRenderer>(),"HighScore text object has no component <Canvas Renderer>!");
        Assert.NotNull(highScore.GetComponent<Text>(),"HighScore text object has no component <Text>!");
    }

    [UnityTest, Order(3)]
    public IEnumerator ObjectsAreChildren()
    {
        yield return null;
        Assert.That(play.transform.IsChildOf(canvas.transform),"Object \"Play\" should be a child of an object \"Canvas\"!");
        Assert.That(exit.transform.IsChildOf(canvas.transform),"Object \"Exit\" should be a child of an object \"Canvas\"!");
        Assert.That(highScore.transform.IsChildOf(canvas.transform),"Object \"HighScore\" should be a child of an object \"Canvas\"!");
    }
    
    [UnityTest, Order(4)]
    public IEnumerator Exists()
    {
        yield return null;
        Assert.NotNull(GameObject.Find("Main Camera"),"There is no camera in scene named <Main Camera>!");
    }
    [UnityTest, Order(5)]
    public IEnumerator BasicCameraComponents()
    {
        yield return null;
        Assert.NotNull(GameObject.Find("Main Camera").GetComponent<Camera>(),"Object <Main Camera> has no component <Camera>!");
    }

    [UnityTest, Order(6)]
    public IEnumerator PlayIsVisible()
    {
        yield return null;
        RectTransform rect = play.GetComponent<RectTransform>();
        bool correct = rect.anchorMax.x <= 1 &&
                       rect.anchorMax.y <= 1 &&
                       rect.anchorMin.x >= 0 &&
                       rect.anchorMin.y >= 0 &&
                       rect.anchorMin.x < rect.anchorMax.x &&
                       rect.anchorMin.y < rect.anchorMax.y;
        Assert.True(correct, "Play-button anchors should be between (0; 0) and (1; 1)");
        bool correct2 = rect.offsetMin == Vector2.zero && rect.offsetMax == Vector2.zero;
        //Assert.True(correct2, "Set all Play-button offsets axis to zero in order not to pass away from bounds of screen");
    }
    
    [UnityTest, Order(7)]
    public IEnumerator ExitIsVisible()
    {
        yield return null;
        RectTransform rect = exit.GetComponent<RectTransform>();
        bool correct = rect.anchorMax.x <= 1 &&
                       rect.anchorMax.y <= 1 &&
                       rect.anchorMin.x >= 0 &&
                       rect.anchorMin.y >= 0 &&
                       rect.anchorMin.x < rect.anchorMax.x &&
                       rect.anchorMin.y < rect.anchorMax.y;
        Assert.True(correct, "Play-button anchors should be between (0; 0) and (1; 1)");
        bool correct2 = rect.offsetMin == Vector2.zero && rect.offsetMax == Vector2.zero;
        //Assert.True(correct2, "Set all Play-button offsets axis to zero in order not to pass away from bounds of screen");
    }
    
    [UnityTest, Order(8)]
    public IEnumerator ScoreIsVisible()
    {
        yield return null;
        RectTransform rect = highScore.GetComponent<RectTransform>();
        bool correct = rect.anchorMax.x <= 1 &&
                       rect.anchorMax.y <= 1 &&
                       rect.anchorMin.x >= 0 &&
                       rect.anchorMin.y >= 0 &&
                       rect.anchorMin.x < rect.anchorMax.x &&
                       rect.anchorMin.y < rect.anchorMax.y;
        Assert.True(correct, "HighScore anchors should be between (0; 0) and (1; 1)");
        bool correct2 = rect.offsetMin == Vector2.zero && rect.offsetMax == Vector2.zero;
        //Assert.True(correct2, "Set all HighScore offsets axis to zero in order not to pass away from bounds of screen");
    }
    
    [UnityTest, Order(9)]
    public IEnumerator CheckGameTab()
    {
        yield return null;
        Time.timeScale = 40;
        SceneManager.LoadScene("Game");
        yield return null;
        Scene game = SceneManager.GetActiveScene();
        String gameName = game.name;
        yield return null;
        IS.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.TAB);
        yield return new WaitForSeconds(1);
        Scene mainMenu = SceneManager.GetActiveScene();
        String mainMenuName = mainMenu.name;
        bool allIsOk = game != mainMenu &&
                       gameName.Equals("Game") &&
                       mainMenuName.Equals("Main Menu");
        Assert.True(allIsOk, "Pressing Tab-key is not providing scene changing from \"Game\" to \"Main Menu\"!");
    }

    [UnityTest, Order(10)]
    public IEnumerator CheckGameReload()
    {
        yield return null;
        Time.timeScale = 40;
        SceneManager.LoadScene("Game");
        yield return null;
        Scene game = SceneManager.GetActiveScene();
        String gameName = game.name;
        yield return null;
        IS.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_R);
        yield return new WaitForSeconds(1);
        Scene game2 = SceneManager.GetActiveScene();
        String game2Name = game2.name;
        bool allIsOk = game != game2 &&
                       gameName.Equals("Game") &&
                       game2Name.Equals("Game");
        Assert.True(allIsOk, "Pressing R-key is not reloading \"Game\" scene!");
    }

    [UnityTest, Order(11)]
    public IEnumerator CheckMenuPlay()
    {
        yield return null;
        Time.timeScale = 40;
        SceneManager.LoadScene("Main Menu");
        yield return null;
        Scene mainMenu = SceneManager.GetActiveScene();
        String mainMenuName = mainMenu.name;
        yield return null;
        GameObject.Find("Play").GetComponent<Button>().onClick.Invoke();
        yield return new WaitForSeconds(1);
        Scene game = SceneManager.GetActiveScene();
        String gameName = game.name;
        bool allIsOk = game != mainMenu &&
                       mainMenuName.Equals("Main Menu") &&
                       gameName.Equals("Game");
        Assert.True(allIsOk, "Pressing Play-Button is not loading \"Game\" scene!");
    }
    [UnityTest, Order(12)]
    public IEnumerator CheckChangingPlayerPrefs()
    {
        yield return null;
        Time.timeScale = 20;
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Game");
        yield return null;
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            GameObject.Destroy(g);
        }

        yield return null;
        float radius = 200f;
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
        tmpBullet.transform.position = tmpEnemy.transform.position;
        yield return new WaitForSeconds(1f);
        int got = int.Parse(GameObject.Find("Score").GetComponent<Text>().text);
        yield return new WaitForSeconds(2);
        tmpEnemy = GameObject.FindWithTag("Enemy");
        tmpEnemy.transform.position = GameObject.Find("Player").transform.position;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Main Menu");
        yield return new WaitForSeconds(1);
        int found = int.Parse(GameObject.Find("HighScore").GetComponent<Text>().text);
        Assert.AreEqual(got,found, "High score is not being reloaded, or reloaded incorrectly!");
        SceneManager.LoadScene("Game");
        yield return new WaitForSeconds(1);
        tmpEnemy = GameObject.FindWithTag("Enemy");
        tmpEnemy.transform.position = GameObject.Find("Player").transform.position;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Main Menu");
        yield return new WaitForSeconds(1);
        found = int.Parse(GameObject.Find("HighScore").GetComponent<Text>().text);
        Assert.AreEqual(got, found, "High score is being reloaded when new score is lower than previous!");
    }
}