using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

public class A_ObjectsTest
{
    public A_ObjectsTest()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");
    }
    [Test, Order(1)]
    public void CheckPlayerObjects()
    {
        Assert.NotNull(GameObject.Find("Player"),"There is no object \"Player\" in scene, or it is misspelled!");
        Assert.NotNull(GameObject.Find("Shotgun"),"There is no object \"Shotgun\" in scene, or it is misspelled!");
    }
    
    [Test, Order(2)]
    public void CheckPlayerChilds()
    {
        Assert.True(GameObject.Find("Player").transform.IsChildOf(GameObject.Find("Player").transform),
            "Object \"Shotgun\" is not a child of \"Player\" object");
    }
}
