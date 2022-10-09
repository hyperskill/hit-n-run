using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEditor;

[Description("Hold your fire! Hold your fire!"),Category("5")]
public class Stage5_Tests
{
    [UnityTest, Order(0)]
    public IEnumerator SetUp()
    {
        PMHelper.TurnCollisions(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
        yield return null;
    }

    [UnityTest, Order(1)]
    public IEnumerator CheckSpawnShotComponents()
    {
        bool BulletTagExist = false;
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals("Bullet")) { BulletTagExist = true; break; }
        }

        if (!BulletTagExist)
        {
            Assert.Fail("\"Bullet\" tag was not added to project");
        }
        
        GameObject enemy = GameObject.FindWithTag("Enemy");
        GameObject obstacle = GameObject.FindWithTag("Obstacle");
        GameObject player = GameObject.Find("Player");
        GameObject shotgun = GameObject.Find("Shotgun");
        GameObject cameraObj = GameObject.Find("Main Camera");
        yield return null;

        SpriteRenderer enemySR = PMHelper.Exist<SpriteRenderer>(enemy);
        SpriteRenderer obstacleSR = PMHelper.Exist<SpriteRenderer>(obstacle);
        SpriteRenderer playerSR = PMHelper.Exist<SpriteRenderer>(player);
        SpriteRenderer shotgunSR = PMHelper.Exist<SpriteRenderer>(shotgun);
        Camera camera = PMHelper.Exist<Camera>(cameraObj);
        yield return null;
        
        Color enemyColor = enemySR.color;
        Color obstacleColor = obstacleSR.color;
        Color playerColor = playerSR.color;
        Color shotgunColor = shotgunSR.color;
        Color backColor = camera.backgroundColor;
        
        yield return null;
        
        EditorWindow game=null;
        double X, Y;

        (game,X,Y) = PMHelper.GetCoordinatesOnGameWindow(0.25f, 0.25f);
        VInput.MoveMouseTo(X, Y);
        yield return null;
        VInput.LeftButtonClick();
        yield return null;
        
        GameObject tmp = GameObject.FindWithTag("Bullet");
        if (!tmp)
        {
            Assert.Fail("Bullet is not been spawned, or it's tag is misspelled");
        }
        SpriteRenderer srTmp = PMHelper.Exist<SpriteRenderer>(tmp);
        if (!srTmp || !srTmp.enabled)
        {
            Assert.Fail("There is no <SpriteRenderer> component on \"Bullet\" object's or it is disabled");
        }

        if (!srTmp.sprite)
        {
            Assert.Fail("There is no sprite assigned to \"Bullet\"s' <SpriteRenderer>");
        }
        
        if (playerSR.sortingLayerID != srTmp.sortingLayerID)
        {
            Assert.Fail("You don't need to change the \"Sorting Layer\" parameter in <SpriteRenderer> component," +
                        "in order to change order of rendering. Leave objects on the same sorting layer and change the" +
                        "\"Order in Layer\" parameter");
        }
        if (playerSR.sortingOrder <= srTmp.sortingOrder)
        {
            Assert.Fail("Player should be visible in front of bullets, so player's order in layer should be greater than bullets' one");
        }
        if (shotgunSR.sortingOrder <= srTmp.sortingOrder)
        {
            Assert.Fail("Shotgun should be visible in front of bullets, so shotgun's order in layer should be greater than bullets' one");
        }

        Color shotColor = srTmp.color;
        
        if (!PMHelper.CheckColorDifference(playerColor, shotColor, 0.3f))
            Assert.Fail("The difference of colors between \"Player\" and \"Bullet\" objects should be visible!");
        if (!PMHelper.CheckColorDifference(shotgunColor, shotColor, 0.3f))
            Assert.Fail("The difference of colors between \"Shotgun\" and \"Bullet\" objects should be visible!");
        if (!PMHelper.CheckColorDifference(enemyColor, shotColor, 0.3f))
            Assert.Fail("The difference of colors between \"Enemy\" and \"Bullet\" objects should be visible!");
        if (!PMHelper.CheckColorDifference(obstacleColor, shotColor, 0.3f))
            Assert.Fail("The difference of colors between \"Obstacle\" and \"Bullet\" objects should be visible!");
        if (!PMHelper.CheckColorDifference(backColor, shotColor, 0.3f))
            Assert.Fail("The difference of colors between background and \"Bullet\" objects should be visible!");
        
        Collider2D tmpColl=PMHelper.Exist<Collider2D>(tmp);
        Rigidbody2D tmpRb=PMHelper.Exist<Rigidbody2D>(tmp);
        yield return null;
        if (!tmpColl)
        {
            Assert.Fail("\"Bullet\" objects should have assigned <Collider2D> component");
        }
        if (!tmpColl.isTrigger)
        {
            Assert.Fail("\"Bullet\" objects' <Collider2D> component should be triggerable");
        }

        if (!tmpRb)
        {
            Assert.Fail("\"Bullet\" objects should have assigned <Rigidbody2D> component");
        }

        if (tmpRb.bodyType != RigidbodyType2D.Dynamic)
        {
            Assert.Fail("\"Bullet\" objects' <Rigidbody2D> component should be Dynamic");
        }
        if (!tmpRb.simulated)
        {
            Assert.Fail("\"Bullet\" objects' <Rigidbody2D> component should be simulated");
        }
        if (tmpRb.gravityScale!=0)
        {
            Assert.Fail("\"Bullet\" objects' <Rigidbody2D> component should not be affected by gravity, " +
                        "so it's Gravity Scale parameter should be equal to 0");
        }
        if (tmpRb.interpolation != RigidbodyInterpolation2D.None)
        {
            Assert.Fail("Do not change interpolation of \"Bullet\" objects' <Rigidbody2D> component. Set it as None");
        }
        if (tmpRb.constraints != RigidbodyConstraints2D.None)
        {
            Assert.Fail("Do not freeze any \"Bullet\" objects' <Rigidbody2D> component's constraints");
        }
    }

    [UnityTest, Order(2)]
    public IEnumerator CheckBulletMovement()
    {
        SceneManager.LoadScene("Game");
        yield return null;

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
        VInput.MoveMouseTo(Convert.ToDouble(X), Convert.ToDouble(Y));
        yield return null;
        VInput.LeftButtonClick();
        yield return null;
        
        GameObject tmp = GameObject.FindWithTag("Bullet");
        GameObject player = GameObject.Find("Player");
        GameObject cameraObj = GameObject.Find("Main Camera");
        yield return null;
        
        Transform tmpT = PMHelper.Exist<Transform>(tmp);
        Transform playerT = PMHelper.Exist<Transform>(player);
        Camera camera = PMHelper.Exist<Camera>(cameraObj);
        yield return null;

        Vector3 placeShoot = camera.ScreenToWorldPoint(Input.mousePosition);
        yield return new WaitForSeconds(0.5f);
        
        Vector3 startPos = tmpT.position;
        float distStart = Vector3.Distance(playerT.position, startPos);
        Vector3 way1 = startPos - playerT.position;
        yield return new WaitForSeconds(0.5f);

        Vector3 endPos = tmpT.position;
        float distEnd = Vector3.Distance(playerT.position, endPos);
        Vector3 way2 = endPos - playerT.position;
        Vector3 way3 = placeShoot - playerT.position;

        if (distStart >= distEnd)
        {
            Assert.Fail("\"Bullet\"'s are not moving or not moving from the \"Player\"'s object");
        }
        if (way2.x/way1.x-way2.y/way1.y>=0.01f)
        {
            Assert.Fail("\"Bullet\"'s movement direction is not always the same");
        }
        if (way2.x/way3.x-way2.y/way3.y>=0.01f)
        {
            Assert.Fail("\"Bullet\"'s movement direction differs from the direction, cursor had provided");
        }
        
        //Check movement via rigidbody
        Rigidbody2D tmpRb = PMHelper.Exist<Rigidbody2D>(tmp);
        yield return null;
        tmpRb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return null;
        Vector2 bef,now;
        
        bef=tmpT.position;
        yield return null;
        now = tmpT.position;
        if (bef != now)
        {
            Assert.Fail("\"Bullet\"'s movement was not implemented with <Rigidbody2D> component usage");
        }
    }
    
    [UnityTest, Order(3)]
    public IEnumerator CheckBulletActions()
    {
        GameObject tmp = GameObject.FindWithTag("Bullet");
        GameObject enemy = GameObject.FindWithTag("Enemy");
        GameObject obstacle = GameObject.FindWithTag("Obstacle");
        GameObject player = GameObject.Find("Player");
        GameObject border = GameObject.FindWithTag("Border");
        yield return null;
        
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Border"))
        {
            g.layer = LayerMask.NameToLayer("Test");
        }
        
        Transform playerT=PMHelper.Exist<Transform>(player);
        yield return null;
        float left = PMHelper.RaycastFront2D(playerT.position, Vector2.left,
            1 << LayerMask.NameToLayer("Test")).point.x;
        float up = PMHelper.RaycastFront2D(playerT.position, Vector2.up,
            1 << LayerMask.NameToLayer("Test")).point.y;
        float right = PMHelper.RaycastFront2D(playerT.position, Vector2.right,
            1 << LayerMask.NameToLayer("Test")).point.x;
        float down = PMHelper.RaycastFront2D(playerT.position, Vector2.down,
            1 << LayerMask.NameToLayer("Test")).point.y;
        yield return null;
        
        Vector2 leftUp = new Vector2(left+(right-left)/4, down+(up-down)/4*3);
        Vector2 leftDown = new Vector2(left+(right-left)/4, down+(up-down)/4);
        Vector2 rightUp = new Vector2(left+(right-left)/4*3, down+(up-down)/4*3);
        Vector2 rightDown = new Vector2(left+(right-left)/4*3, down+(up-down)/4);

        Rigidbody2D enemyRb = PMHelper.Exist<Rigidbody2D>(enemy);
        yield return null;
        enemyRb.constraints = RigidbodyConstraints2D.FreezeAll;
        
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            if (!g.Equals(obstacle))
            {
                GameObject.Destroy(g);
            }
        }

        tmp.transform.position = leftDown;
        enemy.transform.position = rightUp;
        player.transform.position = rightDown;
        obstacle.transform.position = leftUp;
        yield return null;
        
        GameObject tmp2 = GameObject.Instantiate(tmp);
        GameObject tmp3 = GameObject.Instantiate(tmp);
        yield return null;

        PMHelper.TurnCollisions(true);
        yield return new WaitForSeconds(0.3f);
        if (!tmp || !tmp2 || !tmp3)
        {
            Assert.Fail("\"Bullet\"s should not be destroyed, when colliding with each other");
        }

        tmp.transform.position = rightDown;
        yield return new WaitForSeconds(0.3f);
        if (!player)
        {
            Assert.Fail("\"Player\" should not be destroyed, when colliding with \"Bullet\"");
        }
        if (!tmp)
        {
            Assert.Fail("\"Bullet\"s should not be destroyed, when colliding with player");
        }
        tmp.transform.position = leftUp;
        yield return new WaitForSeconds(0.3f);
        if (!obstacle)
        {
            Assert.Fail("\"Obstacle\"'s should not be destroyed, when colliding with \"Bullet\"");
        }
        if (!(tmp==null))
        {
            Assert.Fail("\"Bullet\"s should be destroyed, when colliding with \"Obstacle\"s");
        }
        tmp2.transform.position = rightUp;
        yield return new WaitForSeconds(0.3f);
        if (!(enemy==null))
        {
            Assert.Fail("Enemies should be destroyed, when colliding with \"Bullet\"");
        }
        if (!(tmp2==null))
        {
            Assert.Fail("\"Bullet\"s should be destroyed, when colliding with \"Enemy\"");
        }
        tmp3.transform.position = border.transform.position;
        yield return new WaitForSeconds(0.5f);
        if (!border)
        {
            Assert.Fail("\"Border\"'s should not be destroyed, when colliding with \"Bullet\"");
        }
        if (!(tmp3==null))
        {
            Assert.Fail("\"Bullet\"s should be destroyed, when colliding with \"Border\"s");
        }
    }
}