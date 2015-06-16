using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour
{

    private GameManager GM;
    private FactoryAddon upgrades;

    public GameObject upgrading;
    public PlayerMovement player;
    public CharacterObjectives characterObjective;
    public GameObject charr;

    public GameObject objectiveButtons;
    public GameObject shop;
    IObjectives objective = null;

    // Use this for initialization
    void Start()
    {
        upgrades = upgrading.GetComponent<FactoryAddon>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
		characterObjective = charr.GetComponent<CharacterObjectives>();
        objective = new KillObjective();
        characterObjective.SetObjective(objective);
        //   upgradeGUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(characterObjective.objective.ObjectiveDescription());
    }


    /* Create a new object when pressing Speed Button*/
    public void SpeedButton()
    {
        Debug.Log("Speed Button Pressed");
        player.addon = upgrades.CreateAddon(player.addon, FactoryAddon.AddonType.Speed);

    }
    public void AttackButton()
    {
        Debug.Log("Attack Button Pressed");
        player.addon = upgrades.CreateAddon(player.addon, FactoryAddon.AddonType.Bomb);
    }

    public void KillObjective()
    {
        objective = new KillObjective();
        characterObjective.SetObjective(objective);
        Debug.Log("Kill Objective");
    }


    public void RunObjective()
    {
        objective = new RunObjective();
        characterObjective.SetObjective(objective);
        Debug.Log("Run Objective");
    }

    public void CloseButton()
    {
        shop.SetActive(false);
        Time.timeScale = 1;
        ServiceLocator.GetLevelGenerator().GenerateLevel();
    }

    void OnGUI()
    {
        GUI.TextField(new Rect(10, 20, 200, 50), characterObjective.objective.ObjectiveDescription());
    }
}