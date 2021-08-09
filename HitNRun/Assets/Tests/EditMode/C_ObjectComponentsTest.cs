using System;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class C_ObjectComponentsTest
{
    public C_ObjectComponentsTest()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");
    }
    [Test, Order(1)]
    public void PlayerSpriteRenderers()
    {
        Assert.NotNull(GameObject.Find("Player").GetComponent<SpriteRenderer>(),
            "There is no <SpriteRenderer> component on \"Player\" object!");
        Assert.NotNull(GameObject.Find("Shotgun").GetComponent<SpriteRenderer>(),
            "There is no <SpriteRenderer> component on \"Shotgun\" object!");
        Assert.NotNull(GameObject.Find("Player").GetComponent<SpriteRenderer>().sprite,
            "There is no sprite assigned to \"Player\"'s <SpriteRenderer>!");
        Assert.NotNull(GameObject.Find("Shotgun").GetComponent<SpriteRenderer>().sprite,
            "There is no sprite assigned to \"Shotgun\"'s <SpriteRenderer>!");
    }

    [Test, Order(2)]
    public void Colors()
    {
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

    [Test, Order(3)]
    public void CheckOrderRenderers()
    {
        Assert.Greater(GameObject.Find("Player").GetComponent<SpriteRenderer>().sortingOrder,GameObject.Find("Shotgun").GetComponent<SpriteRenderer>().sortingOrder,
            "Player should be visible in front of shotgun, so player's sorting layer should be greater than shotgun's one!");
    }
    
    [Test, Order(4)]
    public void CheckPositions()
    {
        Assert.False(GameObject.Find("Shotgun").transform.position==Vector3.zero,"Shotgun should not be placed at center of Player!");
        Assert.Greater(GameObject.Find("Player").GetComponent<SpriteRenderer>().sortingOrder,GameObject.Find("Shotgun").GetComponent<SpriteRenderer>().sortingOrder,
            "Player should be visible in front of shotgun, so player's sorting layer should be greater than shotgun's one!");
    }
}
