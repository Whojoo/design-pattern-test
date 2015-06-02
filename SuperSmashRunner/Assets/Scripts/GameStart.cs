using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {

    GameManager GM;

    void Start()
    {
        GM = GameManager.Instance;
    }
    public void StartGame()
    {
        GM.SetGameState(GameState.Game);
        GM.StateSwitch();   
    }
}
