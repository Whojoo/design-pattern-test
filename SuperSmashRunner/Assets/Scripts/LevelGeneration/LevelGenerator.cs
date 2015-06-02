using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LevelGenerator : MonoBehaviour, ILevelGeneratorService
{
    public bool DEBUGCAMERA = false;
    public GameObject DebugCamera;
    public GameObject Player;

    private const int Width = 200;
    private const int Height = 15;
    private Quadtree tree;
    private CellularAutomata generator;

    // Use this for initialization
    void Start()
    {
        //Service locator needs to know about this object.
        ServiceLocator.Provide(this);

        tree = new Quadtree(new Rect(0, 0, Width, Height));
        generator = new CellularAutomata(Width, Height);
        GenerateLevel();

        if (DEBUGCAMERA)
        {
            Camera.main.gameObject.SetActive(false);

            GameObject.Instantiate(DebugCamera, new Vector3(0, 1, -10), transform.rotation);
        }
    }

    public void EmptyLevel()
    {
        ReusablePool.GetInstance().SleepAll();
        if (!DEBUGCAMERA)
        {
            Vector2 pos = new Vector2(0, 1);
            Camera.main.transform.position = pos;
        }

        Player.SetActive(false);
    }

    public int GetWidth()
    {
        return Width;
    }

    /**
     * ILevelGeneratorService Implementation.
     */
    public void GenerateLevel()
    {
        //Create the platforms.
        ReusablePool pool = ReusablePool.GetInstance();
        pool.SleepAll();
        tree.Clear();

        if (!DEBUGCAMERA)
            pool.ChangeEnemyLimitBy(5);

        generator.GenerateMap(100);

        foreach (Block block in generator.GetBlocks())
        {
            pool.AddPlatform(block);
            tree.Insert(block);
        }

        //Spawn enemies until the ReusablePool tells us to stop.
        while (pool.AddEnemyAt(tree.GetNextEnemySpawnBlock())) ;

        Player.SetActive(true);
        Player.transform.position = new Vector2(5, generator.GetFirstY() + 1);
    }

    // Update is called once per frame
    void Update()
    {
        //Only allow spacebar to regenerate during debugging.
        if (DEBUGCAMERA && Input.GetKeyUp(KeyCode.Space))
        {
            GenerateLevel();
        }
    }
}

