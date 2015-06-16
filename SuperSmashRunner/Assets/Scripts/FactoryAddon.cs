using UnityEngine;
using System.Collections;

public class FactoryAddon : MonoBehaviour
{
	
	// Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}

	public enum AddonType { 
		Bomb,
		Life,
		Speed,
		Defense
		
	}
	
	/* return a new object*/
	public ICharacterAddon CreateAddon(ICharacterAddon addons,AddonType addonType)
	{        switch (addonType)
		{
		case AddonType.Bomb:
			return new BombAddon(addons);
		//case AddonType.Life:
			//    return new StatAddon(addons,"Life",amount);
		case AddonType.Speed:
			return new StatAddon(addons,"Speed",0.3f);
		//case AddonType.Defense:
			//    return new StatAddon(addons,"Defense",amount);
		default:
			return new BombAddon(addons);
			
		}
		
	}

    public static GameObject CreateEnemy()
    {
        GameObject toReturn = GameObject.CreatePrimitive(PrimitiveType.Cube);
        toReturn.name = "Enemy";

        //Replace the 3D collider with the 2D collider.
        Component.DestroyImmediate(toReturn.GetComponent<BoxCollider>());
        toReturn.AddComponent<BoxCollider2D>();
        toReturn.GetComponent<Renderer>().material.color = new Color(50.0f, 0, 0);

        toReturn.AddComponent<Rigidbody2D>();
		//toReturn.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeRotation; Unity 5.1.0f3 or later
		toReturn.GetComponent<Rigidbody2D> ().fixedAngle = true;
		toReturn.AddComponent<EnemyScript> (); //Add the enemy script to the enemy.

        return toReturn;
    }

    public static GameObject CreatePlatform()
    {
        GameObject toReturn = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //Remove the old BoxCollider (3D version) and add BoxCollider2D.
        Component.DestroyImmediate(toReturn.GetComponent<BoxCollider>()); //Destroy() is delayed so use DestroyImmediate().
        toReturn.AddComponent<BoxCollider2D>();
        toReturn.name = "Platform";

        return toReturn;
    }
}