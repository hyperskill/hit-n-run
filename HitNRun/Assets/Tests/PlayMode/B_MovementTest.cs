using System;
using System.Collections;
using WindowsInput;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Random = UnityEngine.Random;

public class B_MovementTest
{
    private InputSimulator IS = new InputSimulator();
    public GameObject player;

    public B_MovementTest()
    {
        Time.timeScale = 40;
    }

    [UnityTest, Order(1)]
    public IEnumerator TypeOfMovementCheck()
    {
        yield return null;
        SceneManager.LoadScene("Game");
        yield return new WaitForSeconds(1);
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            GameObject.Destroy(g);
        }

        GameObject.Find("Player").AddComponent<TmpDestroyEnemies>();
        Assert.NotNull(GameObject.Find("Player").GetComponent<Rigidbody2D>(),
            "Movement should be implemented via Rigidbody2D to improve collisions!");
    }
    [UnityTest, Order(2)]
    public IEnumerator CheckLeftMovement()
    {
        yield return null;
        player = GameObject.Find("Player");
        Vector3 startpos = player.transform.position;
        IS.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_A);
        yield return new WaitForSeconds(1);
        Vector3 endpos = player.transform.position;
        Assert.True(endpos.x<startpos.x&&endpos.y==startpos.y&&endpos.z==startpos.z,"Left movement is not working properly!");
    }
    [UnityTest, Order(3)]
    public IEnumerator CheckRightMovement()
    {
        yield return null;
        player = GameObject.Find("Player");
        Vector3 startpos = player.transform.position;
        IS.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_D);
        yield return new WaitForSeconds(1);
        Vector3 endpos = player.transform.position;
        Assert.True(endpos.x>startpos.x&&endpos.y==startpos.y&&endpos.z==startpos.z,"Left movement is not working properly!");
    }
    [UnityTest, Order(4)]
    public IEnumerator CheckDownMovement()
    {
        yield return null;
        player = GameObject.Find("Player");
        Vector3 startpos = player.transform.position;
        IS.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_S);
        yield return new WaitForSeconds(1);
        Vector3 endpos = player.transform.position;
        Assert.True(endpos.x==startpos.x&&endpos.y<startpos.y&&endpos.z==startpos.z,"Left movement is not working properly!");
    }
    [UnityTest, Order(5)]
    public IEnumerator CheckUpMovement()
    {
        yield return null;
        player = GameObject.Find("Player");
        Vector3 startpos = player.transform.position;
        IS.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_W);
        yield return new WaitForSeconds(1);
        Vector3 endpos = player.transform.position;
        Assert.True(endpos.x==startpos.x&&endpos.y>startpos.y&&endpos.z==startpos.z,"Left movement is not working properly!");
    }
    
    [UnityTest, Order(7)]
    public IEnumerator LookingTest()
    {
        yield return null;
        player = GameObject.Find("Player");
        GameObject shotgun = GameObject.Find("Shotgun");
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(1);
            float x = Random.Range(0, Screen.width);
            float y = Random.Range(0, Screen.height);
            x *= 65535 / Screen.width;
            y *= 65535 / Screen.height;
            IS.Mouse.MoveMouseTo(Convert.ToDouble(x), Convert.ToDouble(y));
            yield return new WaitForSeconds(1);
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float startDist = Vector2.Distance(shotgun.transform.position, mouse);
            player.transform.Rotate(Vector3.forward,10);
            float endDist1 = Vector2.Distance(shotgun.transform.position, mouse);
            player.transform.Rotate(Vector3.forward,-20);
            float endDist2 = Vector2.Distance(shotgun.transform.position, mouse);
            if (startDist >= endDist1 || startDist >= endDist2)
            {
                Assert.Fail("Player rotation is not working properly!");
            }
        }
    }

    [UnityTest, Order(6)]
    public IEnumerator CameraFollowCheck()
    {
        player = GameObject.Find("Player");
        IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_A);
        yield return new WaitForSeconds(10);
        IS.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_A);
        Assert.True(player.GetComponent<Renderer>().isVisible);
        IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_D);
        yield return new WaitForSeconds(10);
        IS.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_D);
        Assert.True(player.GetComponent<Renderer>().isVisible);
        IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_S);
        yield return new WaitForSeconds(10);
        IS.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_S);
        Assert.True(player.GetComponent<Renderer>().isVisible);
        IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_W);
        yield return new WaitForSeconds(10);
        IS.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_W);
        Assert.True(player.GetComponent<Renderer>().isVisible);
        yield return null;
    }
}
