using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyResourceEvent : DEvent {

    public static void AddEventFromJSON(DCity currentCity, JSONNode json)
    {
        var resources = new DResource[json["resources"].AsArray.Count];
        for (int i = 0; i < resources.Length; i++)
        {
            int amount = json["resources"][i]["value"];
            string name = json["resources"][i]["type"];
            resources[i] = DResource.Create(name, amount);
        }

        DEventSystem.AddEvent(new ModifyResourceEvent(
            json["promptText"],
            currentCity,
            resources,
            json["activationCondition"],
            json["turnsToActivate"],
            json["priority"],
            json["nextEvent"],
            json.ToString()
        ));
    }


    private DResource[] resources;

    private ModifyResourceEvent(string promptText, DCity city, DResource[] resources, 
        string actConditionString, int turnsToActivation = 0, int priority = Constants.EVENT_PRIORITY_DEFAULT, JSONNode nextEvent = null, string json)
    {
        this.promptText = promptText;
        this.city = city;
        this.resources = resources;
        this.actCondition = ParseActivationCondition(actConditionString);
        this.actConditionString = actConditionString;
        this.turnsToActivation = turnsToActivation;
        this.priority = priority;
        this.nextEvent = nextEvent;        
    }

    public override void Activate()
    {
        /*
         * TEMPORARY - PLUG IN UI TO REPLACE ALL LINES BELOW WITH EQUIVALENTS
        */
		//Add new message sound effect here
        Debug.Log(promptText);  // for UI - display (call DEvent.PromptText)
        Debug.Log("<player enters confirmation via UI>");   // for UI - from UI call DGame.Instance().ResolveEvent() when player hits button to close
        dGame.ResolveEvent(); // for UI - per above, remove this when UI call to ResolveEvent() implemented
        
    }

    override public void Resolve(int selection = Constants.NO_INPUT)
    {
        for(int i=0; i<resources.Length; i++)
        {
            city.AddResource(resources[i]);
        }

        if(nextEvent != null)
        {
            var eventType = nextEvent["type"].Value;
            switch (eventType)
            {
                case "modify":
                    DEventSystem.AddEventFromId(Constants.EVT_TYPE.MOD_RESOURCE, city, nextEvent["id"].AsInt);
                    break;
                case "choice":
                    DEventSystem.AddEventFromId(Constants.EVT_TYPE.CHOICE, city, nextEvent["id"].AsInt);
                    break;
                default:
                    throw new Exception("Event type not recognized");
            }
        }
    }

    public override JSONNode SaveToJSON()
    {
        JSONNode resourceJSON = new JSONObject();
        foreach (var r in resources)
        {
            resourceJSON.Add(new JSONObject
            {
                { "type", r.Name },
                { "value", r.Amount }
            });
        }

        return new JSONObject
        {            
            { "priority", priority },
            { "turnsToActivate", turnsToActivation },
            { "promptText", promptText },
            { "activationCondition", actConditionString },
            { "resources", resourceJSON.AsArray }
        };     
    }

    public override void LoadFromJSON(JSONNode json, DCity city)
    {
        AddEventFromJSON(city, json);
    }
}
