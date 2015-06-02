using UnityEngine;
using System.Collections;

public class GameManagerTest : MonoBehaviour
{

    GameManager GM;

    void Awake()
    {
        GM = GameManager.Instance;
        GM.OnStateChange += HandleOnStateChange;

        Debug.Log("game state on awake: " + GM.gameState);

        GM.SetGameState(GameState.MainMenu);
      
    }

    void Start()
    {
        Debug.Log("game state on start: " + GM.gameState);
    }

    
    public void HandleOnStateChange()
    {
        Debug.Log("Handling state change to: " + GM.gameState);
    }
}
