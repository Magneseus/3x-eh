using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEventSystem {

    public static List<IEvent> eventPool = new List<IEvent>();
    public static List<IEvent> eventsToAdd = new List<IEvent>();
    public static List<IEvent> eventsToRemove = new List<IEvent>();

    public static EventController EventController { get; set; }

    public static void AddEvent(IEvent e)
    {
        eventsToAdd.Add(e);
    }

    public static void RemoveEvent(IEvent e)
    {
        eventsToRemove.Add(e);
    }

    public static void TurnUpdate()
    {
        foreach (var e in eventsToAdd)
            eventPool.Add(e);
        eventsToAdd.Clear();

        foreach (var e in eventsToRemove)
            eventPool.Remove(e);
        eventsToRemove.Clear();

        foreach (var e in eventPool)
            EventController.ResolveEvent(e);


    }
}
