using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Random = UnityEngine.Random;

[Description("Come on, come on. Chop-chop!"),Category("2")]
public class Stage2_Tests
{
    public GameObject player;
    private bool exist;
    public StageHelper helper;
    public Camera camera;
    public GameObject cameraObj;

    [UnityTest, Order(1)]
    public IEnumerator SetUp()
    {
        Time.timeScale = 40;
        yield return null;
        SceneManager.LoadScene("Game");
        yield return null;
        
        player=GameObject.Find("Player");
        
        GameObject helperObject = new GameObject("Helper");
        yield return null;
        helper=helperObject.AddComponent<StageHelper>();
        helper.destroyEnemies = true;
        yield return new WaitForSeconds(0.5f);
        
        helper.RemoveObstacles();
        
    }
    [UnityTest, Order(2)]
    public IEnumerator CheckLeftMovement()
    {
        yield return null;
        Vector3 startpos = player.transform.position;
        
        VInput.KeyDown(KeyCode.A);
        yield return new WaitForSeconds(1);
        VInput.KeyUp(KeyCode.A);
        yield return null;
        
        Vector3 endpos = player.transform.position;
        if (endpos.x >= startpos.x)
        {
            Assert.Fail("By pressing \"A\"-key, player should move to the left, x-axis should decrease");
        }

        if (endpos.y != startpos.y || endpos.z != startpos.z)
        {
            Assert.Fail("By moving left y-axis or z-axis should not change");
        }
    }
    [UnityTest, Order(3)]
    public IEnumerator CheckRightMovement()
    {
        yield return null;
        Vector3 startpos = player.transform.position;
        
        VInput.KeyDown(KeyCode.D);
        yield return new WaitForSeconds(1);
        VInput.KeyUp(KeyCode.D);
        yield return null;
        
        Vector3 endpos = player.transform.position;
        if (endpos.x <= startpos.x)
        {
            Assert.Fail("By pressing \"D\"-key, player should move to the right, x-axis should increase");
        }

        if (endpos.y != startpos.y || endpos.z != startpos.z)
        {
            Assert.Fail("By moving right y-axis or z-axis should not change");
        }
    }
    [UnityTest, Order(4)]
    public IEnumerator CheckDownMovement()
    {
        yield return null;
        Vector3 startpos = player.transform.position;
        
        VInput.KeyDown(KeyCode.S);
        yield return new WaitForSeconds(1);
        VInput.KeyUp(KeyCode.S);
        yield return null;
        
        Vector3 endpos = player.transform.position;
        if (endpos.y >= startpos.y)
        {
            Assert.Fail("By pressing \"S\"-key, player should move down, y-axis should decrease");
        }

        if (endpos.x != startpos.x || endpos.z != startpos.z)
        {
            Assert.Fail("By moving down x-axis or z-axis should not change");
        }
    }
    [UnityTest, Order(5)]
    public IEnumerator CheckUpMovement()
    {
        yield return null;
        Vector3 startpos = player.transform.position;
        
        VInput.KeyDown(KeyCode.W);
        yield return new WaitForSeconds(1);
        VInput.KeyUp(KeyCode.W);
        yield return null;
        
        Vector3 endpos = player.transform.position;
        if (endpos.y <= startpos.y)
        {
            Assert.Fail("By pressing \"W\"-key, player should move up, y-axis should increase");
        }

        if (endpos.x != startpos.x || endpos.z != startpos.z)
        {
            Assert.Fail("By moving up x-axis or z-axis should not change");
        }
    }
    
    [UnityTest, Order(7)]
    public IEnumerator LookingTest()
    {
        yield return null;
        player = GameObject.Find("Player");
        GameObject shotgun = GameObject.Find("Shotgun");
        (cameraObj, exist) = PMHelper.Exist("Main Camera");
        camera = PMHelper.Exist<Camera>(cameraObj);
        
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(1);
            float x = Random.Range(0, Screen.width);
            float y = Random.Range(0, Screen.height);
            x *= 65535 / Screen.width;
            y *= 65535 / Screen.height;
            VInput.MoveMouseTo(Convert.ToDouble(x), Convert.ToDouble(y));
            yield return null;
            
            Vector2 mouse = camera.ScreenToWorldPoint(Input.mousePosition);
            float startDist = Vector2.Distance(shotgun.transform.position, mouse);
            player.transform.Rotate(Vector3.forward,10);
            float endDist1 = Vector2.Distance(shotgun.transform.position, mouse);
            player.transform.Rotate(Vector3.forward,-20);
            float endDist2 = Vector2.Distance(shotgun.transform.position, mouse);
            if (startDist >= endDist1 || startDist >= endDist2)
            {
                Assert.Fail("Player rotation is not working properly, it's shotgun should be facing the mouse cursor directly");
            }
        }
    }

    [UnityTest, Order(6)]
    public IEnumerator CameraFollowCheck()
    {
        player = GameObject.Find("Player");
        (cameraObj, exist) = PMHelper.Exist("Main Camera");
        camera = PMHelper.Exist<Camera>(cameraObj);
        
        VInput.KeyDown(KeyCode.A);
        yield return new WaitForSeconds(10);
        VInput.KeyUp(KeyCode.A);
        yield return null;
        
        if (!PMHelper.CheckVisibility(camera, player.transform, 2))
        {
            Assert.Fail("Camera should follow the \"Player\" object");
        }
        VInput.KeyDown(KeyCode.D);
        yield return new WaitForSeconds(10);
        VInput.KeyUp(KeyCode.D);
        yield return null;
        if (!PMHelper.CheckVisibility(camera, player.transform, 2))
        {
            Assert.Fail("Camera should follow the \"Player\" object");
        }
        VInput.KeyDown(KeyCode.S);
        yield return new WaitForSeconds(10);
        VInput.KeyUp(KeyCode.S);
        yield return null;
        if (!PMHelper.CheckVisibility(camera, player.transform, 2))
        {
            Assert.Fail("Camera should follow the \"Player\" object");
        }
        VInput.KeyDown(KeyCode.W);
        yield return new WaitForSeconds(10);
        VInput.KeyUp(KeyCode.W);
        yield return null;
        if (!PMHelper.CheckVisibility(camera, player.transform, 2))
        {
            Assert.Fail("Camera should follow the \"Player\" object");
        }
        yield return null;
    }
}