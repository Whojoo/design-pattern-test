using UnityEngine;
using System.Collections;

public class UpgradeFactory : MonoBehaviour {



  //  public IUpgrades upgr;
   

    void Start()
    {
        // upgr = upgr.GetComponent<IUpgrades>();

    }
    public UpgradeFactory()
    {

    }


    public IUpgrades spawnNewUpgrade(IUpgrades upgr)
    {
        return  upgr;

      
    }
      
}
