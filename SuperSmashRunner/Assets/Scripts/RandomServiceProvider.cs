using UnityEngine;
using System.Collections;

public class RandomServiceProvider : MonoBehaviour, IRandomService
{
	private System.Random randy;

	public System.Random GetRandomizer()
	{
		return randy;
	}

	void Awake()
	{
		//Leed seed.
		randy = new System.Random(1337);
		ServiceLocator.Provide(this);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
