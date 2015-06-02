using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour 
{

    GameManager GM;

	public GameObject Player;
    public GameObject Shop;
    public GameObject Objectives;

	private float xSpeed = 4.0f;
	private float ySpeedMultiplier = 4.0f;

    void Start()
    {
        GM = GameManager.Instance;
    }

	// Update is called once per frame
	void Update () 
	{
		//Don't update if the player is not active.
		if (!Player.activeSelf)
			return;

		var movement = Vector2.zero;
		movement.x += xSpeed; 

		movement.y = (Player.transform.position.y - transform.position.y) * ySpeedMultiplier;

		transform.Translate(movement * Time.deltaTime, Space.Self);

        float camWidth = Camera.main.orthographicSize * 2.0f * Camera.main.aspect;
        float camLeft = Camera.main.transform.position.x - camWidth * 0.5f;

        //Player gameover.
        if (Player.transform.position.x + Player.GetComponent<Renderer>().bounds.size.x < camLeft)
        {
            //Empty level.
            ServiceLocator.GetLevelGenerator().EmptyLevel();

            //Game over.
            Debug.Log("Game Over");
            GM.SetGameState(GameState.GameOver);
            GM.StateSwitch();
            return;
        }

        //Player at end (last 20 spots are solid).
        if (Player.transform.position.x > ServiceLocator.GetLevelGenerator().GetWidth() - 10)
        {
            //Empty level.
            ServiceLocator.GetLevelGenerator().EmptyLevel();
            
            //Open shop.
            if (Shop != null)
                Debug.Log("shop");
                GM.SetGameState(GameState.Shop);
                GM.StateSwitch();

            return;
        }
	}
}
