using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

[Description("Remember! This is bandit country."),Category("6")]
public class Stage6_Tests
{
    [UnityTest, Order(0)]
    public IEnumerator SetUp()
    {
        PMHelper.TurnCollisions(false);
        Time.timeScale = 0;
        SceneManager.LoadScene("Game");
        yield return null;
    }
    [UnityTest, Order(1)]
    public IEnumerator CheckSpeedAndColorIncrease()
    {
        
        GameObject helper = new GameObject("helper");
        StageHelper helperComp = helper.AddComponent<StageHelper>();

        yield return null;
        helperComp.RemoveObstacles();
        yield return null;
        
        GameObject firstEnemy = GameObject.FindWithTag("Enemy");
        yield return null;
        
        Transform firstEnemyT = firstEnemy.transform;
        SpriteRenderer firstEnemySR = PMHelper.Exist<SpriteRenderer>(firstEnemy);
        yield return null;
        
        Color first = firstEnemySR.color;

        GameObject player = GameObject.Find("Player");
        Transform playerT = player.transform;
        yield return null;
        
        Time.timeScale = 10;
        float firstStart = Time.unscaledTime;
        yield return new WaitUntil(() =>
            Vector2.Distance(firstEnemyT.position,playerT.position)<0.01f || (Time.unscaledTime - firstStart) * Time.timeScale > 20);
        if ((Time.unscaledTime - firstStart) * Time.timeScale >= 20)
        {
            Assert.Fail("Enemies are not moving to a player");
        }
        float firstEnd = Time.unscaledTime;
        helperComp.destroyEnemies=true;
        float start = Time.unscaledTime;
        yield return new WaitUntil(() =>
            (Time.unscaledTime - start) * Time.timeScale > 20);
        
        helperComp.destroyEnemies=false;
        if (GameObject.FindWithTag("Enemy"))
        {
            GameObject.Destroy(GameObject.FindWithTag("Enemy"));
        }
        
        start = Time.unscaledTime;
        yield return new WaitUntil(() =>
            GameObject.FindWithTag("Enemy") || (Time.unscaledTime - start) * Time.timeScale > 20);
        if ((Time.unscaledTime - start) * Time.timeScale >= 20)
        {
            Assert.Fail();
        }

        GameObject secondEnemy = GameObject.FindWithTag("Enemy");
        yield return null;
        
        Transform secondEnemyT = secondEnemy.transform;
        SpriteRenderer secondEnemySR = PMHelper.Exist<SpriteRenderer>(secondEnemy);
        yield return null;
        
        Color second = secondEnemySR.color;

        float secondStart = Time.unscaledTime;
        yield return new WaitUntil(() =>
            Vector2.Distance(secondEnemyT.position,playerT.position)<0.01f || (Time.unscaledTime - secondStart) * Time.timeScale > 20);
        if ((Time.unscaledTime - secondStart) * Time.timeScale >= 20)
        {
            Assert.Fail("Enemies are not moving to a player");
        }
        float secondEnd = Time.unscaledTime;
        
        if (first == second)
        {
            Assert.Fail("After 20+ seconds of game-time, color of new enemies didn't change");
        }
        if (secondEnd - secondStart >= firstEnd - firstStart)
        {
            Assert.Fail("After 20+ seconds of game-time, speed of new enemies didn't increase");
        }
    }
}