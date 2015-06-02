﻿// This interface allows objects to register themselves with an event manager
// in order to listen for events.

using UnityEngine;
using System.Collections;

namespace GameEvents
{

    public interface GameEventListener
    {
        void eventReceived(GameEvent e);
    }
}
