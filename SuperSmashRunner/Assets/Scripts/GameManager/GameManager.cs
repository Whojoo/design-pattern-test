using UnityEngine;
using System.Collections;

public enum GameState { NullState, MainMenu, Game, Shop,GameOver, End }
public delegate void OnStateChangeHandler();

public class GameManager
{

    private static GameManager _instance = null;
    public event OnStateChangeHandler OnStateChange;
    public GameObject shops;
    public GameObject objectives;
    public GameState gameState { get; private set; }
    protected GameManager() { }

    // Singleton pattern implementation
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }


   public void StateSwitch()
    {
        if (gameState == GameState.NullState)
        {

        }
        if (gameState == GameState.MainMenu)
        {
            Application.LoadLevel(0);
        }
        if (gameState == GameState.Game)
        {
            Application.LoadLevel(1);
        }
        if (gameState == GameState.Shop)
        {
            shops.SetActive(true);
            objectives.SetActive(false);
        }
        if (gameState == GameState.GameOver)
        {
            Application.LoadLevel(2);
        }
        if (gameState == GameState.End)
        {

        }
    }
    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
        if (OnStateChange != null)
        {
            OnStateChange();
        }
       
       
    }
}
