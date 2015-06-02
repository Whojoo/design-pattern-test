// This event is dispatched when a movement key is pressed.

using UnityEngine;
using System.Collections;
using GameEvents;

public class CameraMoveEvent : GameEvent
{
    public Vector3 direction;

    public CameraMoveEvent(Vector3 d)
    {
        direction = d;
    }
}
