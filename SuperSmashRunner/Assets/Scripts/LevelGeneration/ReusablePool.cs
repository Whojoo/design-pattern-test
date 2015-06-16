using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Singleton class containing objects which will be reused instead of re-initialized.
public sealed class ReusablePool
{
    private static readonly ReusablePool instance = new ReusablePool();

    //Constants.
    private const int DefaultEnemyLimit = 25;

    //Limits.
    private int enemyLimit;

    //Lists of active objects.
    private LinkedList<GameObject> activePlatforms;
    private LinkedList<GameObject> activeEnemies;

    //List of sleeping objects.
    private LinkedList<GameObject> sleepingPlatforms;
    private LinkedList<GameObject> sleepingEnemies;

    private ReusablePool()
    {
        enemyLimit = DefaultEnemyLimit;

        activePlatforms = new LinkedList<GameObject>();
        activeEnemies = new LinkedList<GameObject>();

        sleepingPlatforms = new LinkedList<GameObject>();
        sleepingEnemies = new LinkedList<GameObject>();
    }

    public static ReusablePool GetInstance()
    {
        return instance;
    }

    public void AddPlatform(Block block)
    {
        //Get the platform from sleeping or create a new one.
        GameObject platform = sleepingPlatforms.Count > 0 ? GetSleepingFrom(sleepingPlatforms) : FactoryAddon.CreatePlatform();

        //Add to active and set to active.
        activePlatforms.AddLast(platform);

        //Add the position and the scale.
        platform.transform.position = block.GetPosition();
        platform.transform.localScale = block.GetScale();
    }

    public bool AddEnemyAt(Block block)
    {
        //Check if we have reached our limit yet.
        if (activeEnemies.Count == enemyLimit)
            return false;

        //Get a new enemy from either sleeping or new creation.
        GameObject enemy = sleepingEnemies.Count > 0 ? GetSleepingFrom(sleepingEnemies) : FactoryAddon.CreateEnemy();

        //Add to active.
        activeEnemies.AddLast(enemy);

        //Position stuff.
        enemy.transform.position = block.GetRandomPosition() + new Vector2(0, 1);

        //Addition succes.
        return true;
    }

    public void ChangeEnemyLimitBy(int modifier)
    {
        enemyLimit += modifier;
    }

	public int GetActiveEnemyCount()
	{
		return activeEnemies.Count;
	}

    private GameObject GetSleepingFrom(LinkedList<GameObject> sleepList)
    {
        //Get and remove first.
        GameObject toReturn = sleepList.First.Value;
        sleepList.RemoveFirst();

        //Set active and return.
        toReturn.SetActive(true);
        return toReturn;
    }

    public void Reset()
    {
        enemyLimit = DefaultEnemyLimit;
    }

    public void SleepAll()
    {
        SleepList(activePlatforms, sleepingPlatforms);
        SleepList(activeEnemies, sleepingEnemies);
    }

    public void SleepEnemy(GameObject enemy)
    {
        if (activeEnemies.Remove(enemy))
        {
            sleepingEnemies.AddLast(enemy);
            enemy.SetActive(false);
        }
    }

    private void SleepList(LinkedList<GameObject> activeList, LinkedList<GameObject> sleepingList)
    {
        while (activeList.Count > 0)
        {
            GameObject temp = activeList.First.Value;
            activeList.RemoveFirst();
            temp.SetActive(false);
            sleepingList.AddLast(temp);
        }
    }

    /* public void SetObjectsSleeping() {
         foreach (var value in sleepingPlatforms)
         {
             GameObject.SetActive(false);
         }
     }*/
}
