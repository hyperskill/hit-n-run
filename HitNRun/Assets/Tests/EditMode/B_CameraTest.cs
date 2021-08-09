using System;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class B_CameraTest
{
    public B_CameraTest()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");
    }
    [Test, Order(1)]
    public void Exists()
    {
        Assert.NotNull(GameObject.Find("Main Camera"),"There is no camera in scene named <Main Camera>!");
    }
    [Test, Order(2)]
    public void BasicComponents()
    {
        Assert.NotNull(GameObject.Find("Main Camera").GetComponent<Camera>(),"Object <Main Camera> has no component <Camera>!");
    }
}
