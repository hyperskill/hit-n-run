
using System.Collections;
using System.Collections.Generic;
using WindowsInput;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class B_ObstaclesTest
{
    private InputSimulator IS = new InputSimulator();
    private GameObject player;
    private List<Vector3> firstPos = new List<Vector3>();
    private List<Vector3> secondPos = new List<Vector3>();

    public B_ObstaclesTest()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 40;
    }
    
    [UnityTest]
    public IEnumerator CheckBorders()
    {
        yield return new WaitForSeconds(1);
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            GameObject.Destroy(g);
        }
        GameObject.Find("Player").AddComponent<TmpDestroyEnemies>();
        float left, right, up, down;
        player = GameObject.Find("Player");
        
        player.transform.position=Vector3.zero;
        
        IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_A);
        yield return new WaitForSeconds(10);
        Vector3 first = player.transform.position;
        yield return new WaitForSeconds(2);
        Vector3 second = player.transform.position;
        IS.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_A);
        Assert.That(Mathf.RoundToInt(first.x)==Mathf.RoundToInt(second.x),
            "There are no bounds of field for the player, or they are placed too far");
        left = player.transform.position.x;
        player.transform.position=Vector3.zero;

        IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_W);
        yield return new WaitForSeconds(10);
        first = player.transform.position;
        yield return new WaitForSeconds(2);
        second = player.transform.position;
        IS.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_W);
        Assert.That(Mathf.RoundToInt(first.y)==Mathf.RoundToInt(second.y),
            "There are no bounds of field for the player, or they are placed too far");
        up = player.transform.position.y;
        player.transform.position=Vector3.zero;
        
        IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_S);
        yield return new WaitForSeconds(10);
        first = player.transform.position;
        yield return new WaitForSeconds(2);
        second = player.transform.position;
        IS.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_S);
        Assert.That(Mathf.RoundToInt(first.y)==Mathf.RoundToInt(second.y),
            "There are no bounds of field for the player, or they are placed too far");
        down = player.transform.position.y;
        player.transform.position=Vector3.zero;
        
        IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_D);
        yield return new WaitForSeconds(10);
        first = player.transform.position;
        yield return new WaitForSeconds(2);
        second = player.transform.position;
        IS.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_D);
        Assert.That(Mathf.RoundToInt(first.x)==Mathf.RoundToInt(second.x),
            "There are no bounds of field for the player, or they are placed too far");
        right = player.transform.position.x;

        float fieldSize = (up - down) * (right - left);
        
        //Checking obstacles
        
        yield return null;
        SceneManager.LoadScene("Game");
        Assert.NotNull(player.GetComponent<Collider2D>(),"Player should have a <Collider2D> component in order to be obstruct by obstacles!");
        yield return new WaitForSeconds(1);
        float obstacleSizeSum = 0;
        Color playerColor = GameObject.Find("Player").GetComponent<SpriteRenderer>().color;
        Color shotgunColor = GameObject.Find("Shotgun").GetComponent<SpriteRenderer>().color;
        Color backColor = GameObject.Find("Main Camera").GetComponent<Camera>().backgroundColor;
        
        GameObject exampleObstacle = GameObject.FindWithTag("Obstacle");
        Assert.NotNull(exampleObstacle,"Obstacles are not been spawned or their tag is misspelled!");
        Assert.NotNull(exampleObstacle.GetComponent<SpriteRenderer>(),"Obstacles are not visible, add <SpriteRenderer> component to them!");
        Color obstacleColor = exampleObstacle.GetComponent<SpriteRenderer>().color;
        
        float difPlayer = Mathf.Abs(playerColor.r - obstacleColor.r) + Mathf.Abs(playerColor.g - obstacleColor.g) +
                          Mathf.Abs(playerColor.b - obstacleColor.b);
        float difShotgun = Mathf.Abs(shotgunColor.r - obstacleColor.r) + Mathf.Abs(shotgunColor.g - obstacleColor.g) +
                           Mathf.Abs(shotgunColor.b - obstacleColor.b);
        float difBack = Mathf.Abs(backColor.r - obstacleColor.r) + Mathf.Abs(backColor.g - obstacleColor.g) +
                        Mathf.Abs(backColor.b - obstacleColor.b);
        
        Assert.Greater(difPlayer, 0.4f, "The difference of colors between \"Player\" and obstacles should be visible!");
        Assert.Greater(difBack, 0.4f, "The difference of colors between obstacles and background should be visible!");
        Assert.Greater(difShotgun, 0.4f, "The difference of colors between \"Shotgun\" and obstacles should be visible!");
        
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            firstPos.Add(g.transform.position);
            obstacleSizeSum += g.transform.localScale.x*g.transform.localScale.y;
            Assert.NotNull(g.GetComponent<Collider2D>(),"Obstacles should have colliders on them in order to obstruct the player!");
        }

        SceneManager.LoadScene("Game");
        yield return new WaitForSeconds(1);
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            secondPos.Add(g.transform.position);
        }

        int num=0;
        if (firstPos.Count < secondPos.Count)
        {
            num = firstPos.Count;
        }
        else
        {
            num = secondPos.Count;
        }
        
        for (int i = 0; i < num; i++)
        {
            Assert.False(firstPos[i].Equals(secondPos[i]),"Obstacles should be spawned randomly!");
        }
        Assert.Less(obstacleSizeSum,fieldSize/2f, "Obstacles should not take more than a half of field!");
        Assert.Greater(obstacleSizeSum,fieldSize/5f, "Obstacles should not take less than a 1/5 of field!");
    }
}
