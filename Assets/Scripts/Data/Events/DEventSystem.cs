using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class DEventSystem {

    public static List<DEvent> eventPool = new List<DEvent>();
    public static List<DEvent> eventsToAdd = new List<DEvent>();
    public static List<DEvent> eventsToRemove = new List<DEvent>();

    public static EventController EventController { get; set; }

    public static void AddEventFromJSON(Constants.EVT_TYPE eventType, DCity currentCity, JSONNode json)
    {
        switch (eventType)
        {
            case Constants.EVT_TYPE.MOD_RESOURCE:
                ModifyResourceEvent.AddEventFromJSON(currentCity, json);
                break;
            case Constants.EVT_TYPE.CHOICE:
                ChoiceEvent.AddEventFromJSON(currentCity, json);
                break;
            default:
                throw new System.Exception("No EventType Specified");
        }
    }

    public static void AddEventFromId(Constants.EVT_TYPE eventType, DCity currentCity, int id)
    {
        switch (eventType)
        {
            case Constants.EVT_TYPE.MOD_RESOURCE:
                AddEventFromJSON(eventType, currentCity, JSON.Parse(File.ReadAllText(Constants.EVT_MOD_RESOURCE_EVENTS_PATH))[id]);
                break;
            case Constants.EVT_TYPE.CHOICE:
                AddEventFromJSON(eventType, currentCity, JSON.Parse(File.ReadAllText(Constants.EVT_MOD_RESOURCE_EVENTS_PATH))[id]);
                break;
            default:
                throw new System.Exception("No EventType Specified");
        }
    }

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

    public static JSONNode SaveToJSON()
    {
        var allEvents = new List<DEvent>();

        foreach (var e in eventPool)
        {
            allEvents.Add(e);
        }
        foreach (var e in eventsToAdd)
        {
            allEvents.Add(e);
        }

        foreach (var e in eventsToRemove)
        {
            allEvents.Remove(e);
        }

        JSONNode returnNode = new JSONObject();
        foreach (var e in allEvents)
        {
            returnNode.Add(e.SaveToJSON());
        }

        return returnNode;
    }

    public static void LoadFromJSON(JSONNode json, DCity city)
    {
        // TODO
    }
}
