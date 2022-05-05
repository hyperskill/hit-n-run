using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class A_ObjectsTest
{
    private GameObject player, shotgun;
    private GameObject camera;
    private Camera cameraComp;
    private SpriteRenderer playerSR, shotgunSR;
    
    [UnityTest, Order(1)]
    public IEnumerator CheckPlayerObjects()
    {
        Time.timeScale = 40;
        if (!Application.CanStreamedLevelBeLoaded("Game"))
        {
            Assert.Fail("\"Game\" scene is misspelled or was not added to build settings");
        }
        
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"),LayerMask.NameToLayer("Default"),true);
        PMHelper.TurnCollisions(false);
        yield return null;
        SceneManager.LoadScene("Game");
        yield return null;
        
        player = PMHelper.Exist("Player");
        shotgun = PMHelper.Exist("Shotgun");
        
        if (!player)
        {
            Assert.Fail("There is no object \"Player\" in scene, or it is misspelled");
        }
        if (!shotgun)
        {
            Assert.Fail("There is no object \"Shotgun\" in scene, or it is misspelled");
        }
    }
    
    [UnityTest, Order(2)]
    public IEnumerator CheckPlayerChilds()
    {
        yield return null;
        if (!PMHelper.Child(shotgun, player))
        {
            Assert.Fail("Object \"Shotgun\" is not a child of \"Player\" object");
        }
    }
    
    [UnityTest, Order(3)]
    public IEnumerator CameraExists()
    {
        yield return null;
        camera = PMHelper.Exist("Main Camera");
        
        if (!camera)
        {
            Assert.Fail("There is no camera object in scene, named \"Main Camera\", or it is misspelled");
        }
    }
    [UnityTest, Order(4)]
    public IEnumerator CameraBasicComponents()
    {
        yield return null;
        cameraComp = PMHelper.Exist<Camera>(camera);
        yield return null;
        if (!cameraComp)
        {
            Assert.Fail("\"Main Camera\" object has no basic component <Camera>");
        }
    }
    
    [UnityTest, Order(5)]
    public IEnumerator PlayerSpriteRenderers()
    {
        yield return null;
        playerSR = PMHelper.Exist<SpriteRenderer>(player);
        shotgunSR = PMHelper.Exist<SpriteRenderer>(shotgun);
        if (!playerSR || !playerSR.enabled)
        {
            Assert.Fail("There is no <SpriteRenderer> component on \"Player\" object or it is disabled!");
        }
        if (!shotgunSR || !shotgunSR.enabled)
        {
            Assert.Fail("There is no <SpriteRenderer> component on \"Shotgun\" object or it is disabled!");
        }

        if (!playerSR.sprite)
        {
            Assert.Fail("There is no sprite assigned to \"Player\"'s <SpriteRenderer>!");
        }
        if (!shotgunSR.sprite)
        {
            Assert.Fail("There is no sprite assigned to \"Shotgun\"'s <SpriteRenderer>!");
        }
    }

    [UnityTest, Order(6)]
    public IEnumerator Colors()
    {
        yield return null;

        if (!PMHelper.CheckColorDifference(playerSR.color, shotgunSR.color))
        {
            Assert.Fail("The difference of colors between \"Player\" and \"Shotgun\" should be visible!");
        }
        if (!PMHelper.CheckColorDifference(playerSR.color, cameraComp.backgroundColor))
        {
            Assert.Fail("The difference of colors between \"Player\" and \"Camera\"'s background" +
                        "should be visible!");
        }
        if (!PMHelper.CheckColorDifference(shotgunSR.color, cameraComp.backgroundColor))
        {
            Assert.Fail("The difference of colors between \"Shotgun\" and \"Camera\"'s background" +
                        "should be visible!");
        }
    }
    [UnityTest, Order(6)]
    public IEnumerator CheckVisibility()
    {
        yield return null;
        if (!PMHelper.CheckVisibility(cameraComp, player.transform, 2))
        {
            Assert.Fail("\"Player\" object is not visible by camera");
        }
        if (!PMHelper.CheckVisibility(cameraComp, shotgun.transform, 2))
        {
            Assert.Fail("\"Shotgun\" object is not visible by camera");
        }
    }
    [UnityTest, Order(7)]
    public IEnumerator CheckOrderRenderers()
    {
        yield return null;
        if (playerSR.sortingLayerID != shotgunSR.sortingLayerID)
        {
            Assert.Fail("You don't need to change the \"Sorting Layer\" parameter in <SpriteRenderer> component," +
                        "in order to change order of rendering. Leave objects on the same sorting layer and change the" +
                        "\"Order in Layer\" parameter");
        }
        if (playerSR.sortingOrder <= shotgunSR.sortingOrder)
        {
            Assert.Fail("Player should be visible in front of shotgun, so player's order in layer should be greater than shotgun's one");
        }
    }
    
    [UnityTest, Order(8)]
    public IEnumerator CheckPositions()
    {
        yield return null;
        
        Transform shotgunT = shotgun.transform, playerT=player.transform;
        
        if (shotgunT.localPosition == Vector3.zero)
        {
            Assert.Fail("Shotgun should not be placed at center of Player");
        }
        
        float shotgunX = shotgunT.localPosition.x;
        float shotgunY = shotgunT.localPosition.y;
        if (shotgunX != 0 && shotgunY != 0)
        {
            Assert.Fail("Shotgun's local position should have x-axis equal to zero, or y-axis position equal to zero" +
                        ", so that player is looking up/down/left/right");
        }

        String way="";
        if (shotgunX == 0 && shotgunY > 0) way = "up";
        if (shotgunX == 0 && shotgunY < 0) way = "down";
        if (shotgunX < 0 && shotgunY == 0) way = "left";
        if (shotgunX > 0 && shotgunY == 0) way = "right";
        
        
        shotgunT.rotation=Quaternion.Euler(0,0,0);
        playerT.rotation=Quaternion.Euler(0,0,0);
        
        bool correct = true;
        switch (way)
        {
            case "up":
                correct = shotgunT.position.y - shotgunT.lossyScale.y / 2 <= playerT.position.y + playerT.lossyScale.y / 2;
                break;
            case "down":
                correct = shotgunT.position.y + shotgunT.lossyScale.y / 2 >= playerT.position.y - playerT.lossyScale.y / 2;
                break;
            case "right":
                correct = shotgunT.position.x - shotgunT.lossyScale.x / 2 <= playerT.position.x + playerT.lossyScale.x / 2;
                break;
            case "left":
                correct = shotgunT.position.x + shotgunT.lossyScale.x / 2 >= playerT.position.x - playerT.lossyScale.x / 2;
                break;
        }

        if (!correct)
        {
            Assert.Fail("Make sure, that there is no gap between player and shotgun.");
        }
        
        shotgunT.rotation=Quaternion.Euler(0,0,0);
        playerT.rotation=Quaternion.Euler(0,0,0);
        
        if (shotgunT.lossyScale.x > playerT.lossyScale.x || shotgunT.lossyScale.y > playerT.lossyScale.y)
        {
            Assert.Fail("Make sure, that \"Shotgun\" object is not longer and not wider than the \"Player\" object");
        }
    }
}