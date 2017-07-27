using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEventSystem {

    public static List<DEvent> eventPool = new List<DEvent>();
    public static List<DEvent> eventsToAdd = new List<DEvent>();
    public static List<DEvent> eventsToRemove = new List<DEvent>();

    public static EventController EventController { get { return EventController.Instance(); } }

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
        foreach (var e in eventPool)
            if (e.TurnsToActivation > 0)
                e.TurnsToActivation--;

        foreach (var e in eventsToAdd)
            eventPool.Add(e);
        eventsToAdd.Clear();

        foreach (var e in eventsToRemove)
            eventPool.Remove(e);
        eventsToRemove.Clear();
    }

    public static DEvent NextEvent()
    {
        DEvent result = null;
        if (eventPool.Count == 0)
            return result;
        foreach (DEvent e in eventPool)
            if(e.TurnsToActivation == 0)
                if (result == null || e.Priority > result.Priority)
                    result = e;
        eventPool.Remove(result);
        return result;
    }
}
