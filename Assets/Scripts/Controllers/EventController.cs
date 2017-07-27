using System;
using UnityEngine;

public class EventController : MonoBehaviour {

    public void Awake()
    {
        DEventSystem.EventController = this;
    }

    public void ResolveEvent(DEvent e)
    {
        e.Resolve();
    }
}
