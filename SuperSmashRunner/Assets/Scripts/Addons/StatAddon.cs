using UnityEngine;
using System.Collections;


public class StatAddon : ICharacterAddon
{

	private readonly ICharacterAddon next;
	
	public StatAddon(ICharacterAddon add,string type,float amount)
	{
		next = add;
		PlayerMovement controller=GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
		switch (type){
		case "Speed":
			controller.IncreaseSpeed(amount);
		break;
		case "Life":
			//controller.IncreaseSpeed(amount);
		break;
		case "Defense":
			//controller.IncreaseSpeed(amount);
		break;
			
		}
	}
	public void Update()
	{
		next.Update ();
	}
}

