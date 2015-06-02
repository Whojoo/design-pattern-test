using UnityEngine;
using System.Collections;

public class KillObjective : IObjectives{

    int enemies;
   public bool ObjectiveProgress()
    {
        enemies += 1;
        if (enemies == 20)
        {
            return true;
        }

        return false;
    }

   public string ObjectiveDescription()
   {
       string killDescription;
       killDescription = "Kill 20 Enemies";
       return killDescription;
   }
   
    public int GetCurrency()
    {
        int currency = 200;
        return currency;
    }
}
