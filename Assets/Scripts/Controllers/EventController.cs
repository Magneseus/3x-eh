using System;
using UnityEngine;

public class EventController : MonoBehaviour {

    private static EventController singleton = null;

    public static EventController Instance()
    {
        if(singleton == null)
        {
            singleton = GameObject.FindObjectOfType<EventController>();
            if (singleton == null)
                singleton = new EventController();
        }
        return singleton;
    }

    public void ResolveEvent(DEvent e)
    {
        e.Resolve();
    }
}
