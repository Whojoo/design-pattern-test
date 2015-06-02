using UnityEngine;
using System.Collections;

//Memento object
public class Memento
{
	public readonly float jumpForce;
	public readonly float playerSpeed;
	public readonly ICharacterAddon addon;
	
	public Memento(float jump, float speed ,ICharacterAddon add)
	{
		jumpForce = jump;
		playerSpeed = speed;
		addon=add;
	}
}