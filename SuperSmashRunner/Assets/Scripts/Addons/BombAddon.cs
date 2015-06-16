using UnityEngine;
using System.Collections;

public class BombAddon : ICharacterAddon
{
	float cooldown;
	private GameObject bombsource;
	private GameObject player;
	public ICharacterAddon next;
	public BombAddon(ICharacterAddon add)
	{
		cooldown=0;
		next = add;
		bombsource = Resources.Load("Bomb")as GameObject;
		player=GameObject.FindGameObjectWithTag("Player");
	}


	public void Update()
	{
		if (cooldown<=0&&Input.GetKey(KeyCode.X))
		{
			cooldown=0.2f;
			Debug.Log("Bomb has been planted");
			GameObject clone= GameObject.Instantiate(bombsource,(Vector2)player.transform.position+new Vector2(1,1f),Quaternion.identity) as GameObject;
			clone.GetComponent<Rigidbody2D>().velocity +=new Vector2(3,4)+player.GetComponent<Rigidbody2D>().velocity;
		}
		cooldown-=Time.deltaTime;
		next.Update ();
	}
}





