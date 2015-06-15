using UnityEngine;
using System.Collections;

public class mainScript : MonoBehaviour
{
    IObjectives objective = null;
    public GameObject Character;
    public CharacterObjectives charr;

    void Awake()
    {
        charr = Character.GetComponent<CharacterObjectives>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F1))
        {
            objective = new KillObjective();
            charr.SetObjective(objective);
            Debug.Log("Kill Objective");
        }
        if (Input.GetKey(KeyCode.F2))
        {
            objective = new RunObjective();
            charr.SetObjective(objective);
            Debug.Log("Run Objective");
        }
        if (Input.GetKey(KeyCode.F3))
        {
        }
    }

    public void KillObjective()
    {
        objective = new KillObjective();
        charr.SetObjective(objective);
        Debug.Log("Kill Objective");
    }


    public void RunObjective()
    {
        objective = new RunObjective();
        charr.SetObjective(objective);
        Debug.Log("Run Objective");
    }
}

