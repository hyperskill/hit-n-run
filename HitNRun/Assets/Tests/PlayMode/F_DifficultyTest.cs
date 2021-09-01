using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class F_DifficultyTest
{
    public F_DifficultyTest()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 20;
    }
    [UnityTest, Order(1)]
    public IEnumerator CheckSpeedAndColorIncrease()
    {
        yield return null;
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            GameObject.Destroy(g);
        }
        float distanceFirst;
        float distanceSecond;
        Color first;
        Color second;
        GameObject tmp;
        Vector3 startPos;
        Vector3 endPos;
        
        yield return null;
        tmp = GameObject.FindWithTag("Enemy");
        first = tmp.GetComponent<SpriteRenderer>().color;
        startPos = tmp.transform.position;
        yield return new WaitForSeconds(1f);
        endPos = tmp.transform.position;
        GameObject.Destroy(tmp);
        distanceFirst = Vector3.Distance(startPos,endPos);
        
        yield return new WaitForSeconds(1f);
        
        for (int i = 0; i < 5; i++)
        {
            yield return null;
            GameObject.Destroy(GameObject.FindWithTag("Enemy"));
            yield return new WaitForSeconds(2);
        }

        yield return null;
        tmp = GameObject.FindWithTag("Enemy");
        second = tmp.GetComponent<SpriteRenderer>().color;
        startPos = tmp.transform.position;
        yield return new WaitForSeconds(1f);
        endPos = tmp.transform.position;
        GameObject.Destroy(tmp);
        distanceSecond = Vector3.Distance(startPos,endPos);

        Assert.LessOrEqual(distanceFirst * 1.3f, distanceSecond, "Enemies' speed not increasing with time, or increasing is too slow!");
        Assert.AreNotEqual(first,second,"Enemies' color should change with increasing speed!");
    }
}
