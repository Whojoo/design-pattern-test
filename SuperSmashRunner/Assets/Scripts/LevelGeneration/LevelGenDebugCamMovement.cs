using UnityEngine;
using System.Collections;
using GameEvents;

public class LevelGenDebugCamMovement : MonoBehaviour, GameEventListener
{
    private float speed = 100.0f;
    // Use this for initialization
    void Start()
    {
        GameEventManager.Instance.registerListener(this);
    }

    public void eventReceived(GameEvent e)
    {
        if (!(e is CameraMoveEvent))
            return;

        Vector2 movement = ((CameraMoveEvent)e).direction;

        transform.Translate(movement * speed * Time.deltaTime, Space.Self);
    }
}
