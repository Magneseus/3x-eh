using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEventSystem {

    public static List<DEvent> eventPool = new List<DEvent>();
    public static List<DEvent> eventsToAdd = new List<DEvent>();
    public static List<DEvent> eventsToRemove = new List<DEvent>();

    public static EventController EventController { get; set; }

    public static void AddEvent(DEvent e)
    {
        eventsToAdd.Add(e);
    }

    public static void RemoveEvent(DEvent e)
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
