using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{

    public IObjectives objective;


    public void SetObjective(IObjectives newObjective)
    {

        objective = newObjective;
    }

    public string ShowObjective(IObjectives objectives)
    {

        string description = "";
        if (objectives != null)
        {

            description = objectives.ObjectiveDescription();
        }
        Debug.Log("fefefefefafefewfewf");
        return description;
    }

    public bool objectiveCompleted(IObjectives obj)
    {

        return false;
    }

}