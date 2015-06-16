using UnityEngine;
using System.Collections;

public class KillObjective : IObjectives{

   public bool ObjectiveProgress()
    {
        if (ReusablePool.GetInstance().GetActiveEnemyCount() <= 0)
        {
            return true;
        }

        return false;
    }

   public string ObjectiveDescription()
   {
       string killDescription;
		int count = ReusablePool.GetInstance ().GetActiveEnemyCount ();
		killDescription = "Kill " + count + " Enemies";
       return killDescription;
   }
   
    public int GetCurrency()
    {
        int currency = 200;
        return currency;
    }
}
