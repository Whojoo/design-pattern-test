using UnityEngine;
using System.Collections;

public class RunObjective : IObjectives {

    int runned;
    public bool ObjectiveProgress()
    {
        if (runned == 200)
        {
            runned = 0;
            return true;
        }
        return false;
    }

    public string ObjectiveDescription()
    {
        return "Run 500m" ;
    }

    public int GetCurrency()
    {
        int currency = 100;
        return currency;
    }
	
}






