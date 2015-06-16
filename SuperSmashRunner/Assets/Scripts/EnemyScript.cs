using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.transform.position.y < -10)
			//Sleep enemy after it fell to deep.
			ReusablePool.GetInstance ().SleepEnemy (gameObject);
	}
}
