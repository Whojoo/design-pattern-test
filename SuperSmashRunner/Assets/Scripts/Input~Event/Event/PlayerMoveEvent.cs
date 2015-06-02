// This event is dispatched when a movement key is pressed.

using UnityEngine;
using System.Collections;
using GameEvents;

public class PlayerMoveEvent : GameEvent
{
    public Vector3 direction;

    public PlayerMoveEvent(Vector3 d)
    {
        direction = d;
    }
}
