using UnityEngine;
using System.Collections;

public class CharacterObjectives : MonoBehaviour
{

    public IObjectives objective;


    public void SetObjective(IObjectives newObjective)
    {

        objective = newObjective;
    }

 

}