using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

[Description("We're in the clear."),Category("3")]
public class Stage3_Tests
{
    private GameObject player;
    private bool exist;
    private List<Vector3> firstPos = new List<Vector3>();
    private List<Vector3> secondPos = new List<Vector3>();
    private GameObject helperObj;

    [UnityTest]
    public IEnumerator Check()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 40;
        yield return null;
        
        //Check tag and layer existance
        bool ObstacleTagExist = false;
        bool BorderTagExist = false;
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals("Obstacle")) { ObstacleTagExist = true; }
            if (t.stringValue.Equals("Border")) { BorderTagExist = true; }
        }

        if (!BorderTagExist)
        {
            Assert.Fail("Border tag was not added to project");
        }
        
        if (!ObstacleTagExist)
        {
            Assert.Fail("Obstacle tag was not added to project");
        }
        int layer = LayerMask.NameToLayer("Test");
        if (layer < 0)
        {
            Assert.Fail("Please, do not remove \"Test\" layer, it's existance necessary for tests");
        }
        
        //Removing obstacles
        helperObj = new GameObject();
        yield return null;
        StageHelper helper = helperObj.AddComponent<StageHelper>();
        helper.destroyEnemies = true;
        yield return null;
        helper.RemoveObstacles();

        //Check if there are borders
        GameObject[] borders = GameObject.FindGameObjectsWithTag("Border");
        if (borders.Length != 4)
        {
            Assert.Fail("There should be 4 borders with \"Border\" tag");
        }

        foreach (GameObject b in borders)
        {
            Collider2D coll = PMHelper.Exist<Collider2D>(b);
            SpriteRenderer srBorder = PMHelper.Exist<SpriteRenderer>(b);
            if (!srBorder || !srBorder.enabled)
            {
                Assert.Fail("There is no <SpriteRenderer> component on \"Border\" object or it is disabled!");
            }
            if (!srBorder.sprite)
            {
                Assert.Fail("There is no sprite assigned to \"Border\"'s <SpriteRenderer>!");
            }
            if (!coll)
            {
                Assert.Fail("Each \"Border\" object should have assigned <Collider2D> component");
            }

            if (coll.isTrigger)
            {
                Assert.Fail("Each \"Border\" object's <Collider2D> component should not be triggerable");
            }
            b.layer=LayerMask.NameToLayer("Test");
        }

        yield return null;
        (player, exist) = PMHelper.Exist("Player");
        Transform playerT=PMHelper.Exist<Transform>(player);
        yield return null;
        int N = 36;
        for (int i = 0; i < N; i++)
        {
            playerT.Rotate(0,0,360/N);
            if (!PMHelper.RaycastFront2D(playerT.position, playerT.up,
                1 << LayerMask.NameToLayer("Test")).collider)
            {
                Assert.Fail("Player should be surrounded by borders");
            }
        }

        //Check player's components
        Collider2D playerColl=PMHelper.Exist<Collider2D>(player);
        Rigidbody2D playerRb=PMHelper.Exist<Rigidbody2D>(player);
        yield return null;
        if (!playerColl)
        {
            Assert.Fail("Player should have assigned <Collider2D> component");
        }
        if (playerColl.isTrigger)
        {
            Assert.Fail("Player's <Collider2D> component should not be triggerable");
        }

        if (!playerRb)
        {
            Assert.Fail("Player should have assigned <Rigidbody2D> component");
        }

        if (playerRb.bodyType != RigidbodyType2D.Dynamic)
        {
            Assert.Fail("Player's <Rigidbody2D> component should be Dynamic");
        }
        if (!playerRb.simulated)
        {
            Assert.Fail("Player's <Rigidbody2D> component should be simulated");
        }
        if (playerRb.gravityScale!=0)
        {
            Assert.Fail("Player's <Rigidbody2D> component should not be affected by gravity, " +
                        "so it's Gravity Scale parameter should be equal to 0");
        }
        if (playerRb.interpolation != RigidbodyInterpolation2D.None)
        {
            Assert.Fail("Do not change interpolation of Player's <Rigidbody2D> component. Set it as None");
        }
        if (playerRb.constraints != RigidbodyConstraints2D.None)
        {
            Assert.Fail("Do not freeze any Player's <Rigidbody2D> component's constraints");
        }
        //Check movement with freezed rigidbody
        Vector3 start = playerT.position;
        playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
        Vector3 bef,now;
        
        bef=playerT.position;
        VInput.KeyPress(KeyCode.W);
        yield return null;
        VInput.KeyPress(KeyCode.A);
        yield return null;
        now = playerT.position;
        if (bef != now)
        {
            Assert.Fail("Player's movement was not reimplemented with <Rigidbody2D> component usage");
        }
        
        playerRb.constraints = RigidbodyConstraints2D.None;
        
        //Check FixedUpdate() method usage        
        bef=playerT.position;
        VInput.KeyDown(KeyCode.W);
        yield return null;
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForFixedUpdate();
            now = playerT.position;
            if (now.y.Equals(bef.y))
            {
                Assert.Fail("Use FixedUpdate() event for player's physics simulation");
            }
            bef=now;
        }
        VInput.KeyUp(KeyCode.W);

        //Check bounds
        float left, right, up, down;
        
        playerT.position = start;
        PMHelper.TurnCollisions(true);
        yield return null;
        
        VInput.KeyDown(KeyCode.A);
        yield return new WaitForSeconds(10);
        Vector3 first = playerT.position;
        yield return null;
        Vector3 second = playerT.position;
        VInput.KeyUp(KeyCode.A);
        yield return null;
        if (first.x != second.x)
        {
            Assert.Fail("There are no bounds of field for the player, or they are placed too far" +
                        "(Player should be able to reach each border from it's start position in less than 5 seconds;)");
        }
        left = playerT.position.x;

        playerT.position = start;
        
        VInput.KeyDown(KeyCode.W);
        yield return new WaitForSeconds(5);
        first = playerT.position;
        yield return null;
        second = playerT.position;
        VInput.KeyUp(KeyCode.W);
        yield return null;
        if (first.y != second.y)
        {
            Assert.Fail("There are no bounds of field for the player, or they are placed too far" +
                        "(Player should be able to reach each border from it's start position in less than 5 seconds;)");
        }
        up = player.transform.position.y;
        playerT.position = start;
        
        VInput.KeyDown(KeyCode.S);
        yield return new WaitForSeconds(5);
        first = playerT.position;
        yield return null;
        second = playerT.position;
        VInput.KeyUp(KeyCode.S);
        yield return null;
        if (first.y != second.y)
        {
            Assert.Fail("There are no bounds of field for the player, or they are placed too far" +
                        "(Player should be able to reach each border from it's start position in less than 5 seconds;)");
        }
        down = player.transform.position.y;
        playerT.position = start;
        
        VInput.KeyDown(KeyCode.D);
        yield return new WaitForSeconds(5);
        first = playerT.position;
        yield return null;
        second = playerT.position;
        VInput.KeyUp(KeyCode.D);
        yield return null;
        if (first.x != second.x)
        {
            Assert.Fail("There are no bounds of field for the player, or they are placed too far" +
                        "(Player should be able to reach each border from it's start position in less than 5 seconds;)");
        }
        right = player.transform.position.x;

        float fieldSize = (up - down) * (right - left);
        
        //Checking obstacles
        
        PMHelper.TurnCollisions(false);
        
        yield return null;
        SceneManager.LoadScene("Game");
        yield return null;
        yield return null;
        
        float obstacleSizeSum = 0;
        Color playerColor = GameObject.Find("Player").GetComponent<SpriteRenderer>().color;
        Color shotgunColor = GameObject.Find("Shotgun").GetComponent<SpriteRenderer>().color;
        Color backColor = GameObject.Find("Main Camera").GetComponent<Camera>().backgroundColor;
        
        GameObject exampleObstacle = GameObject.FindWithTag("Obstacle");
        if (!exampleObstacle)
        {
            Assert.Fail("Obstacles are not been spawned or their tag is misspelled!");
        }

        SpriteRenderer sr = PMHelper.Exist<SpriteRenderer>(exampleObstacle);
        if (!sr || !sr.enabled)
        {
            Assert.Fail("There is no <SpriteRenderer> component on \"Obstacle\" object or it is disabled!");
        }
        if (!sr.sprite)
        {
            Assert.Fail("There is no sprite assigned to \"Obstacle\"'s <SpriteRenderer>!");
        }
        Color obstacleColor = sr.color;
        
        if (!PMHelper.CheckColorDifference(playerColor, obstacleColor, 0.3f))
        {
            Assert.Fail("The difference of colors between \"Player\" and obstacles should be visible!");
        }
        if (!PMHelper.CheckColorDifference(shotgunColor, obstacleColor, 0.3f))
        {
            Assert.Fail("The difference of colors between \"Shotgun\" and obstacles should be visible!");
        }
        if (!PMHelper.CheckColorDifference(backColor, obstacleColor, 0.3f))
        {
            Assert.Fail("The difference of colors between background and obstacles should be visible!");
        }

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Transform gT = PMHelper.Exist<Transform>(g);
            if (!PMHelper.CheckObjectFits2D(gT, new Vector2(left, up), new Vector2(right, down)))
            {
                Assert.Fail("Obstacles should be created inside limiting zone");
            }
            firstPos.Add(gT.position);
            obstacleSizeSum += gT.localScale.x*gT.localScale.y;
            Collider2D coll = PMHelper.Exist<Collider2D>(g);
            if (!coll)
            {
                Assert.Fail("Obstacles should have assigned <Collider2D> component");
            }
            if (coll.isTrigger)
            {
                Assert.Fail("Obstacles' <Collider2D> component should not be triggerable");
            }
        }

        if (obstacleSizeSum > fieldSize / 2f)
        {
            Assert.Fail("Obstacles should not take more than a half of field. In your solution it is "
                        +(int)((obstacleSizeSum/fieldSize)*10000f)/10000f);
        }
        if (obstacleSizeSum < fieldSize / 10f)
        {
            Assert.Fail("Obstacles should not take less than a 1/10 of field. In your solution it is "
                        +(int)((obstacleSizeSum/fieldSize)*10000f)/10000f);
        }

        SceneManager.LoadScene("Game");
        yield return null;
        yield return null;
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            secondPos.Add(g.transform.position);
        }

        for (int i = 0; i < firstPos.Count; i++)
        {
            for (int j = 0; j < secondPos.Count; j++)
            {
                if (firstPos[i] == secondPos[j])
                {
                    Assert.Fail("Obstacles should be spawned randomly!");
                }
            }
        }
    }
}