// This object polls Unity for input and posts relevant GameEvents.

using UnityEngine;
using System.Collections;
using GameEvents;

public class InputManager : MonoBehaviour
{
    //Keycodes for controls
    private KeyCode up = KeyCode.UpArrow;
    private KeyCode left = KeyCode.LeftArrow;
    private KeyCode right = KeyCode.RightArrow;
    private KeyCode down = KeyCode.DownArrow;
    private KeyCode camLeft = KeyCode.A;
    private KeyCode camRight = KeyCode.D;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(up))
            GameEventManager.post(new PlayerMoveEvent(Vector2.up));

        if (Input.GetKey(left))
            GameEventManager.post(new PlayerMoveEvent(-Vector2.right));

        if (Input.GetKey(right))
            GameEventManager.post(new PlayerMoveEvent(Vector2.right));

        if (Input.GetKey(down))
            GameEventManager.post(new PlayerMoveEvent(-Vector2.up));

        if (Input.GetKey(camLeft))
            GameEventManager.post(new CameraMoveEvent(-Vector2.right));

        if (Input.GetKey(camRight))
            GameEventManager.post(new CameraMoveEvent(Vector2.right));
    }
}
