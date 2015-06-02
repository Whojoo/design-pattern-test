// This script listens for PlayerMoveEvents and moves the attached object
// accordingly.

using UnityEngine;
using System.Collections;
using GameEvents;

public class PlayerMovement : MonoBehaviour, GameEventListener
{
    GameEventManager GEM;
    private float moveAmount = 5.0f;
    public bool jump = false;
    private float jumpForce = 7.6f;
	private CareTaker history;
	public ICharacterAddon addon;
    void Awake()
    {
        GEM = GameEventManager.Instance;
    }

    // Use this for initialization
    void Start()
    {
        GEM.registerListener(this);
		addon= new BaseAddon();
		history= new CareTaker();
        history.memento= new Memento(jumpForce,moveAmount,addon);
    }

	void Restore(Memento memento)
	{
		jumpForce=memento.jumpForce;
		moveAmount=memento.playerSpeed;
		addon = memento.addon;
	}
	
	void Update()
	{
		addon.Update();	
	}
	
    public void eventReceived(GameEvent e)
    {
        if (e is PlayerMoveEvent)
        {
            Vector2 d = (e as PlayerMoveEvent).direction;
            if (d == Vector2.up)
            {
                if (jump)
                {
                    GetComponent<Rigidbody2D>().velocity += Vector2.up * jumpForce;
                    jump = false;
                }
            }
            else
            {
                transform.Translate(d * Time.deltaTime * moveAmount);
            }
        }
    }

	
	public void IncreaseSpeed(float amount)
	{
		moveAmount+=amount;
		print ("BONUS SPEED");
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Platform")
        {
            jump = true;
        }
    }
}
