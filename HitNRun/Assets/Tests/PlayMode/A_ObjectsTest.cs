using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class A_ObjectsTest
{
    public A_ObjectsTest()
    {
        SceneManager.LoadScene("Game");
    }
    [UnityTest, Order(1)]
    public IEnumerator CheckPlayerObjects()
    {
        yield return null;
        Assert.NotNull(GameObject.Find("Player"),"There is no object \"Player\" in scene, or it is misspelled!");
        Assert.NotNull(GameObject.Find("Shotgun"),"There is no object \"Shotgun\" in scene, or it is misspelled!");
    }
    
    [UnityTest, Order(2)]
    public IEnumerator CheckPlayerChilds()
    {
        yield return null;
        Assert.True(GameObject.Find("Player").transform.IsChildOf(GameObject.Find("Player").transform),
            "Object \"Shotgun\" is not a child of \"Player\" object");
    }
    
    [UnityTest, Order(3)]
    public IEnumerator CameraExists()
    {
        yield return null;
        Assert.NotNull(GameObject.Find("Main Camera"),"There is no camera in scene named <Main Camera>!");
    }
    [UnityTest, Order(4)]
    public IEnumerator BasicComponents()
    {
        yield return null;
        Assert.NotNull(GameObject.Find("Main Camera").GetComponent<Camera>(),"Object <Main Camera> has no component <Camera>!");
    }
    
    [UnityTest, Order(5)]
    public IEnumerator PlayerSpriteRenderers()
    {
        yield return null;
        Assert.NotNull(GameObject.Find("Player").GetComponent<SpriteRenderer>(),
            "There is no <SpriteRenderer> component on \"Player\" object!");
        Assert.NotNull(GameObject.Find("Shotgun").GetComponent<SpriteRenderer>(),
            "There is no <SpriteRenderer> component on \"Shotgun\" object!");
        Assert.NotNull(GameObject.Find("Player").GetComponent<SpriteRenderer>().sprite,
            "There is no sprite assigned to \"Player\"'s <SpriteRenderer>!");
        Assert.NotNull(GameObject.Find("Shotgun").GetComponent<SpriteRenderer>().sprite,
            "There is no sprite assigned to \"Shotgun\"'s <SpriteRenderer>!");
    }

    [UnityTest, Order(6)]
    public IEnumerator Colors()
    {
        yield return null;
        Color player = GameObject.Find("Player").GetComponent<SpriteRenderer>().color;
        Color shotgun = GameObject.Find("Shotgun").GetComponent<SpriteRenderer>().color;
        Color back = GameObject.Find("Main Camera").GetComponent<Camera>().backgroundColor;

        float difPlayerShotgun = Mathf.Abs(player.r - shotgun.r) + Mathf.Abs(player.g - shotgun.g) +
                                 Mathf.Abs(player.b - shotgun.b);
        float difPlayerBack = Mathf.Abs(player.r - back.r) + Mathf.Abs(player.g - back.g) +
                                 Mathf.Abs(player.b - back.b);
        float difShotgunBack = Mathf.Abs(shotgun.r - back.r) + Mathf.Abs(shotgun.g - back.g) +
                              Mathf.Abs(shotgun.b - back.b);

        Assert.Greater(difPlayerShotgun, 0.4f, "The difference of colors between \"Player\" and \"Shotgun\" should be visible!");
        Assert.Greater(difPlayerBack, 0.4f, "The difference of colors between \"Player\" and background should be visible!");
        Assert.Greater(difShotgunBack, 0.4f, "The difference of colors between \"Shotgun\" and background should be visible!");
    }

    [UnityTest, Order(7)]
    public IEnumerator CheckOrderRenderers()
    {
        yield return null;
        Assert.Greater(GameObject.Find("Player").GetComponent<SpriteRenderer>().sortingOrder,GameObject.Find("Shotgun").GetComponent<SpriteRenderer>().sortingOrder,
            "Player should be visible in front of shotgun, so player's sorting layer should be greater than shotgun's one!");
    }
    
    [UnityTest, Order(8)]
    public IEnumerator CheckPositions()
    {
        yield return null;
        Assert.False(GameObject.Find("Shotgun").transform.position==Vector3.zero,"Shotgun should not be placed at center of Player!");
        Assert.Greater(GameObject.Find("Player").GetComponent<SpriteRenderer>().sortingOrder,GameObject.Find("Shotgun").GetComponent<SpriteRenderer>().sortingOrder,
            "Player should be visible in front of shotgun, so player's sorting layer should be greater than shotgun's one!");
    }
}